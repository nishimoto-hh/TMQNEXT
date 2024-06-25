/*
 * 件名別長期計画一覧　参照画面　保全情報一覧の検索　保全項目
*/
-- WITH句の続き
,
-- 保全スケジュール
schedule_start_date AS ( 
    SELECT
        sc.maintainance_schedule_id
        , sc.management_standards_content_id
        , sc.cycle_year
        , sc.cycle_month
        , sc.cycle_day
        , sc.start_date
        , sc.disp_cycle
        , sc.update_datetime 
    FROM
        mc_maintainance_schedule AS sc 
    WHERE
        NOT EXISTS ( 
            SELECT
                * 
            FROM
                mc_maintainance_schedule AS sub 
            WHERE
                sc.management_standards_content_id = sub.management_standards_content_id 
                AND sc.start_date < sub.start_date
        )
) 
,
-- 上で取得した保全スケジュールを機器別管理基準内容ID、開始日ごとに取得(同じ値なら最大の更新日時のレコード)
schedule_content AS ( 
    SELECT
        main.maintainance_schedule_id
        , main.management_standards_content_id
        , main.cycle_year
        , main.cycle_month
        , main.cycle_day
        , main.start_date
        , main.disp_cycle
        , main.update_datetime 
    FROM
        schedule_start_date AS main 
    WHERE
        NOT EXISTS ( 
            SELECT
                * 
            FROM
                schedule_start_date AS sub 
            WHERE
                main.management_standards_content_id = sub.management_standards_content_id 
                AND main.start_date = sub.start_date 
                AND main.update_datetime < sub.update_datetime
        )
) 
, schedule AS ( 
    SELECT
        main.maintainance_schedule_id
        , main.management_standards_content_id AS management_standards_content_id_schedule
        , main.cycle_year
        , main.cycle_month
        , main.cycle_day
        , main.start_date
        , main.disp_cycle
        , main.update_datetime 
    FROM
        schedule_content main 
    WHERE
        EXISTS ( 
            SELECT
                * 
            FROM
                mc_management_standards_content AS con 
                LEFT JOIN ( 
                    SELECT
                        * 
                    FROM
                        hm_mc_management_standards_content 
                    WHERE
                        history_management_id = @HistoryManagementId
                ) hmcon 
                    ON con.management_standards_content_id = hmcon.management_standards_content_id 
            WHERE
                main.management_standards_content_id = con.management_standards_content_id 
                AND ( 
                    con.long_plan_id = @LongPlanId 
                    OR hmcon.long_plan_id = @LongPlanId --保全項目一覧の追加を考慮
                )
        )
) 
,
-- スケジュール確定排他チェック用更新日時
schedule_updtime AS ( 
    SELECT
        sc_h.management_standards_content_id
        , MAX(sc_h.update_datetime) AS schedule_head_updtime
        , MAX(sc_d.update_datetime) AS schedule_detail_updtime 
    FROM
        mc_maintainance_schedule AS sc_h 
        LEFT OUTER JOIN mc_maintainance_schedule_detail AS sc_d 
            ON ( 
                sc_h.maintainance_schedule_id = sc_d.maintainance_schedule_id
            ) 
    WHERE
        sc_d.complition != 1
        AND sc_d.schedule_date IS NOT NULL
        AND sc_d.schedule_date BETWEEN @ScheduleStart AND @ScheduleEnd 
    GROUP BY
        sc_h.management_standards_content_id
) 
, division_code AS ( 
    SELECT
        hm.key_id AS long_plan_id
        , division_ex.extension_data AS application_division_code 
    FROM
        hm_history_management hm 
        LEFT JOIN ms_structure status_ms        -- 構成マスタ(申請状況)
            ON hm.application_status_id = status_ms.structure_id 
        LEFT JOIN ms_item_extension status_ex   -- アイテムマスタ拡張(申請状況)
            ON status_ms.structure_item_id = status_ex.item_id 
            AND status_ex.sequence_no = 1 
        LEFT JOIN ms_structure division_ms      --構成マスタ(申請区分)
            ON hm.application_division_id = division_ms.structure_id 
        LEFT JOIN ms_item_extension division_ex -- アイテムマスタ拡張(申請区分)
            ON division_ms.structure_item_id = division_ex.item_id 
            AND division_ex.sequence_no = 1 
    WHERE
        hm.key_id = @LongPlanId 
        AND hm.history_management_id = @HistoryManagementId 
        AND status_ex.extension_data IN ('10', '20', '30') --「申請データ作成中」「承認依頼中」「差戻中」のデータのみ
) 
,schedule_date_by_machine as (
    -- 機IDに対するスケジュール情報を取得
    select
	    sch.maintainance_schedule_id
        , cont.management_standards_content_id
        , sch.start_date
        , det.complition
        , det.schedule_date 
    from
        mc_management_standards_component comp 
        left join mc_management_standards_content cont 
            on comp.management_standards_component_id = cont.management_standards_component_id 
        left join mc_maintainance_schedule sch 
            on cont.management_standards_content_id = sch.management_standards_content_id 
        left join mc_maintainance_schedule_detail det 
            on sch.maintainance_schedule_id = det.maintainance_schedule_id 
) 
, max_schedule_date_by_content as ( 
    -- 保全スケジュールID・内容ごとの保全活動が完了したデータの最大のスケジュール日を取得
    select
	    maintainance_schedule_id
        , management_standards_content_id
        , max(schedule_date) schedule_date 
    from
        schedule_date_by_machine 
    where
        complition = 1 
    group by
        maintainance_schedule_id, management_standards_content_id
) 
, next_date_exists_comp as ( 
    -- 保全スケジュールID・内容ごとの保全活動が完了したデータの最大のスケジュール日の次のスケジュール日を取得
    select
	    schedule_date_by_machine.maintainance_schedule_id
        , schedule_date_by_machine.management_standards_content_id
        , min(schedule_date_by_machine.schedule_date) schedule_date
    from
        schedule_date_by_machine 
        inner join max_schedule_date_by_content 
            on schedule_date_by_machine.maintainance_schedule_id = max_schedule_date_by_content.maintainance_schedule_id
			and schedule_date_by_machine.management_standards_content_id = max_schedule_date_by_content.management_standards_content_id
    where
        schedule_date_by_machine.schedule_date > max_schedule_date_by_content.schedule_date
	group by
	    schedule_date_by_machine.maintainance_schedule_id, schedule_date_by_machine.management_standards_content_id
) 
, next_date_not_exists_comp as ( 
    -- 保全スケジュールID・内容ごとの開始日より後のスケジュール日を取得(保全活動が完了していない)
    select
	    schedule_date_by_machine.maintainance_schedule_id
        , schedule_date_by_machine.management_standards_content_id
        , min(schedule_date_by_machine.schedule_date) schedule_date 
    from
        schedule_date_by_machine 
        left join ( 
            select
			    maintainance_schedule_id
                , management_standards_content_id
                , max(start_date) start_date 
            from
                schedule_date_by_machine 
            group by
                maintainance_schedule_id, management_standards_content_id
        ) max_start_date 
            on schedule_date_by_machine.maintainance_schedule_id = max_start_date.maintainance_schedule_id
			and schedule_date_by_machine.management_standards_content_id = max_start_date.management_standards_content_id
    where
        schedule_date_by_machine.start_date >= max_start_date.start_date 
    group by
        schedule_date_by_machine.maintainance_schedule_id, schedule_date_by_machine.management_standards_content_id
)
SELECT
    machine.machine_id
    , machine.machine_no
    , machine.machine_name
    , machine.importance_structure_id
    , (
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = machine.importance_structure_id
                AND st_f.factory_id IN(0, machine.location_factory_structure_id)
            )
        AND tra.structure_id = machine.importance_structure_id
    ) AS importance_name
    , machine.attachment_update_datetime
    , man_com.management_standards_component_id
    , man_com.update_serialid_component
    , man_com.inspection_site_structure_id
    , (
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = man_com.inspection_site_structure_id
                AND st_f.factory_id IN(0, machine.location_factory_structure_id)
            )
        AND tra.structure_id = man_com.inspection_site_structure_id
    ) AS inspection_site_name
    , man_con.management_standards_content_id
    , man_con.update_serialid_content
    , man_con.inspection_content_structure_id
    , (
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = man_con.inspection_content_structure_id
                AND st_f.factory_id IN(0, machine.location_factory_structure_id)
            )
        AND tra.structure_id = man_con.inspection_content_structure_id
    ) AS inspection_content_name
    , man_con.budget_amount
    , man_con.maintainance_kind_structure_id
    , (
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = man_con.maintainance_kind_structure_id
                AND st_f.factory_id IN(0, machine.location_factory_structure_id)
            )
        AND tra.structure_id = man_con.maintainance_kind_structure_id
    ) AS maintainance_kind_name
    , man_con.kind_order
    , man_con.schedule_type_structure_id
    , (
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = man_con.schedule_type_structure_id
                AND st_f.factory_id IN(0, machine.location_factory_structure_id)
            )
        AND tra.structure_id = man_con.schedule_type_structure_id
    ) AS schedule_type_name
    , schedule.maintainance_schedule_id
    , schedule.management_standards_content_id_schedule
    , schedule.start_date
    , schedule.cycle_year
    , schedule.cycle_month
    , schedule.cycle_day
    , schedule.disp_cycle
	-- ●(保全活動が完了したデータ)が存在する場合は、最新の●の次の○のデータの日付
	-- ●が存在しない場合は開始日より後の○の日付
    , coalesce( 
        next_date_exists_comp.schedule_date
        , next_date_not_exists_comp.schedule_date
    ) schedule_date
    , CONCAT_WS( 
        '|'
        , machine.machine_id
        , man_com.management_standards_component_id
        , man_con.management_standards_content_id
    ) AS key_id                                 -- スケジュールと同じ値
    , schedule_updtime.schedule_head_updtime    -- スケジュール確定排他チェック用
    , schedule_updtime.schedule_detail_updtime  -- スケジュール確定排他チェック用
    , machine.attachment_update_datetime        -- 行削除排他チェック用
    , @LongPlanId AS long_plan_id               -- 主キー退避
    , CONCAT_WS('|', machine.machine_id, man_con.kind_order) AS same_mark_key -- スケジュールマークグループ用(スケジュールと同じ値)
    , CASE 
        WHEN history.execution_division = 4 
        OR division_code.application_division_code = '10' --保全情報一覧の追加
            THEN '10' 
        WHEN history.execution_division = 5 
        OR division_code.application_division_code = '30' --保全情報一覧の削除
            THEN '30' 
        ELSE '' 
        END AS application_division_code        --行の背景色設定値
    , history.hm_management_standards_content_id
FROM
    base 
    INNER JOIN machine 
        ON (machine.machine_id = base.machine_id) 
    INNER JOIN man_com 
        ON ( 
            man_com.management_standards_component_id = base.management_standards_component_id
        ) 
    INNER JOIN man_con 
        ON ( 
            man_con.management_standards_content_id = base.management_standards_content_id
        ) 
    LEFT OUTER JOIN schedule 
        ON ( 
            schedule.management_standards_content_id_schedule = base.management_standards_content_id
        ) 
    LEFT OUTER JOIN schedule_updtime 
        ON ( 
            schedule_updtime.management_standards_content_id = base.management_standards_content_id
        )
    LEFT OUTER JOIN
        next_date_exists_comp
    ON  (
            schedule.maintainance_schedule_id = next_date_exists_comp.maintainance_schedule_id
        )
    AND (
            base.management_standards_content_id = next_date_exists_comp.management_standards_content_id
        )
    LEFT OUTER JOIN
        next_date_not_exists_comp
    ON  (
            schedule.maintainance_schedule_id = next_date_not_exists_comp.maintainance_schedule_id 
        )
    AND (
            base.management_standards_content_id = next_date_not_exists_comp.management_standards_content_id
        )
    LEFT JOIN ( 
        SELECT
            hmcon.* 
        FROM
            hm_history_management hm 
            LEFT JOIN hm_mc_management_standards_content hmcon 
                ON hm.history_management_id = hmcon.history_management_id 
        WHERE
            hm.history_management_id = @HistoryManagementId
            AND hmcon.execution_division IN (4, 5) --保全情報一覧の追加、削除
    ) history 
        ON history.management_standards_content_id = base.management_standards_content_id 
    LEFT JOIN division_code 
        ON division_code.long_plan_id = base.long_plan_id
ORDER BY
    -- ソートキーはビジネスロジックで指定