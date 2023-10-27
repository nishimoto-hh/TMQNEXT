/*
 * 件名別長期計画一覧　参照画面　保全情報一覧の検索　保全項目
*/
-- WITH句の続き
,
-- 保全スケジュール
schedule AS ( 
    SELECT
        schedule.maintainance_schedule_id
        , schedule.management_standards_content_id AS management_standards_content_id_schedule
        , schedule.start_date
        , schedule.cycle_year
        , schedule.cycle_month
        , schedule.cycle_day
        , schedule.disp_cycle
        , schedule_detail_sum.schedule_date 
    FROM
        ( 
            -- 機器別管理基準内容IDごとに最大の開始日時をもつものを取得
            SELECT
                main.maintainance_schedule_id
                , main.management_standards_content_id
                , main.start_date
                , main.cycle_year
                , main.cycle_month
                , main.cycle_day
                , main.disp_cycle 
            FROM
                mc_maintainance_schedule AS main 
            WHERE
                NOT EXISTS ( 
                    SELECT
                        * 
                    FROM
                        mc_maintainance_schedule AS sub 
                    WHERE
                        main.management_standards_content_id = sub.management_standards_content_id 
                        AND main.start_date < sub.start_date
                )
        ) AS schedule                           -- スケジュール詳細から保全スケジュールIDごとに最小のスケジュール日を取得
        LEFT OUTER JOIN ( 
            SELECT
                schedule_detail.maintainance_schedule_id
                , MIN(schedule_detail.schedule_date) AS schedule_date 
            FROM
                mc_maintainance_schedule_detail AS schedule_detail 
            WHERE
                complition = 0 
                AND EXISTS ( 
                    SELECT
                        * 
                    FROM
                        ma_summary AS summary 
                    WHERE
                        schedule_detail.summary_id = summary.summary_id 
                        AND summary.long_plan_id = @LongPlanId
                ) 
            GROUP BY
                schedule_detail.maintainance_schedule_id
        ) AS schedule_detail_sum 
            ON ( 
                schedule.maintainance_schedule_id = schedule_detail_sum.maintainance_schedule_id
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
        sc_d.schedule_date IS NOT NULL 
        AND sc_d.complition != 1 
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
SELECT
    machine.machine_id
    , machine.machine_no
    , machine.machine_name
    , machine.importance_structure_id
    , machine.attachment_update_datetime
    , man_com.management_standards_component_id
    , man_com.update_serialid_component
    , man_com.inspection_site_structure_id
    , man_con.management_standards_content_id
    , man_con.update_serialid_content
    , man_con.inspection_content_structure_id
    , man_con.budget_amount
    , man_con.maintainance_kind_structure_id
    , man_con.kind_order
    , man_con.schedule_type_structure_id
    , schedule.maintainance_schedule_id
    , schedule.management_standards_content_id_schedule
    , schedule.start_date
    , schedule.cycle_year
    , schedule.cycle_month
    , schedule.cycle_day
    , schedule.disp_cycle
    , schedule.schedule_date
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