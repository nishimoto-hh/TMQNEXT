SELECT
    concat_ws( 
        '|'
        , ln.long_plan_id
        , machine.machine_id
        , comp.management_standards_component_id
        , cont.management_standards_content_id
        , schedule.maintainance_schedule_id
    ) AS key_id
    , detail.schedule_date                         -- スケジュール日
    , detail.complition                            -- 完了フラグ
    , cont.maintainance_kind_structure_id          -- 点検種別ID
    , ex.extension_data AS maintainance_kind_level -- 点検種別の拡張項目
    , '' AS maintainance_kind_char
    , detail.summary_id                            -- 保全活動件名ID
FROM
    mc_management_standards_component comp -- 機器別管理基準部位
    LEFT JOIN mc_management_standards_content cont -- 機器別管理基準内容
        ON comp.management_standards_component_id = cont.management_standards_component_id 
    LEFT JOIN ln_long_plan ln -- 長計件名
        ON cont.long_plan_id = ln.long_plan_id 
    INNER JOIN mc_machine machine -- 機番情報
        ON comp.machine_id = machine.machine_id 
    INNER JOIN #temp temp -- 出力対象の機番IDが格納されている一時テーブル
        ON machine.machine_id = temp.key1 
    LEFT JOIN (SELECT a.*
	 		FROM mc_maintainance_schedule AS a -- 保全スケジュール
	 		INNER JOIN
	 		-- 機器別管理基準内容IDごとの開始日最新データを取得
	 		(SELECT management_standards_content_id,
	 				MAX(start_date) AS start_date,
					MAX(update_datetime) AS update_datetime
	 		 FROM mc_maintainance_schedule
	 		 GROUP BY management_standards_content_id
	 		) b
	 		ON a.management_standards_content_id = b.management_standards_content_id
	 		AND (a.start_date = b.start_date 
	 			 OR a.start_date IS NULL AND b.start_date IS NULL --null結合を考慮
	 			)
	 		AND (a.update_datetime = b.update_datetime 
	 			 OR a.update_datetime IS NULL AND b.update_datetime IS NULL --null結合を考慮
	 			)
	 ) schedule -- 保全スケジュール
        ON cont.management_standards_content_id = schedule.management_standards_content_id
    LEFT JOIN mc_maintainance_schedule_detail detail -- 保全スケジュール詳細
        ON schedule.maintainance_schedule_id = detail.maintainance_schedule_id 
    LEFT JOIN ms_structure ms -- 構成マスタ(点検種別)
        ON cont.maintainance_kind_structure_id = ms.structure_id 
        AND ms.structure_group_id = 1240 
    LEFT JOIN ms_item_extension ex -- アイテムマスタ拡張(点検種別)
        ON ms.structure_item_id = ex.item_id 
        AND ex.sequence_no = 1 
WHERE
    detail.schedule_date BETWEEN @ScheduleStart AND @ScheduleEnd