/*
 * 件名別長期計画一覧　参照画面　保全情報一覧の検索　保全項目
*/
-- WITH句の続き
,
-- 保全スケジュール
schedule AS(
    SELECT
        schedule.maintainance_schedule_id,
        schedule.management_standards_content_id AS management_standards_content_id_schedule,
        schedule.start_date,
        schedule.cycle_year,
        schedule.cycle_month,
        schedule.cycle_day,
        schedule.disp_cycle,
        schedule_detail_sum.schedule_date
    FROM
        (
            -- 機器別管理基準内容IDごとに最大の開始日時をもつものを取得
            SELECT
                main.maintainance_schedule_id,
                main.management_standards_content_id,
                main.start_date,
                main.cycle_year,
                main.cycle_month,
                main.cycle_day,
                main.disp_cycle
            FROM
                mc_maintainance_schedule AS main
            WHERE
                NOT EXISTS(
                    SELECT
                        *
                    FROM
                        mc_maintainance_schedule AS sub
                    WHERE
                        main.management_standards_content_id = sub.management_standards_content_id
                    AND main.start_date < sub.start_date
                )
            AND EXISTS(
                    SELECT
                        *
                    FROM
                        mc_management_standards_content AS con
                    WHERE
                        main.management_standards_content_id = con.management_standards_content_id
                    AND con.long_plan_id = @LongPlanId
                )
        ) AS schedule
        -- スケジュール詳細から機器別管理基準内容IDごとに最小のスケジュール日を取得
        LEFT OUTER JOIN
            (
            SELECT
              schedule_header.management_standards_content_id
              , MIN(schedule_detail.schedule_date) AS schedule_date 
            FROM
              mc_maintainance_schedule_detail AS schedule_detail 
              INNER JOIN mc_maintainance_schedule AS schedule_header 
                ON ( 
                  schedule_detail.maintainance_schedule_id = schedule_header.maintainance_schedule_id
                ) 
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
              schedule_header.management_standards_content_id
            ) AS schedule_detail_sum
        ON  (
                schedule.management_standards_content_id = schedule_detail_sum.management_standards_content_id
            )
),
-- スケジュール確定排他チェック用更新日時
schedule_updtime AS(
    SELECT
        sc_h.management_standards_content_id,
        MAX(sc_h.update_datetime) AS schedule_head_updtime,
        MAX(sc_d.update_datetime) AS schedule_detail_updtime
    FROM
        mc_maintainance_schedule AS sc_h
        LEFT OUTER JOIN
            mc_maintainance_schedule_detail AS sc_d
        ON  (
                sc_h.maintainance_schedule_id = sc_d.maintainance_schedule_id
            )
    WHERE
        sc_d.complition != 1
    AND sc_d.schedule_date IS NOT NULL
    AND sc_d.schedule_date BETWEEN @ScheduleStart AND @ScheduleEnd
    GROUP BY
        sc_h.management_standards_content_id
)
SELECT
    machine.machine_id,
    machine.machine_no,
    machine.machine_name,
    machine.importance_structure_id,
    (
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
    ) AS importance_name,
    machine.attachment_update_datetime,
    man_com.management_standards_component_id,
    man_com.update_serialid_component,
    man_com.inspection_site_structure_id,
    (
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
    ) AS inspection_site_name,
    man_con.management_standards_content_id,
    man_con.update_serialid_content,
    man_con.inspection_content_structure_id,
    (
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
    ) AS inspection_content_name,
    man_con.budget_amount,
    man_con.maintainance_kind_structure_id,
    (
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
    ) AS maintainance_kind_name,
    man_con.kind_order,
    man_con.long_plan_id,
    man_con.schedule_type_structure_id,
    (
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
    ) AS schedule_type_name,
    schedule.maintainance_schedule_id,
    schedule.management_standards_content_id_schedule,
    schedule.start_date,
    schedule.cycle_year,
    schedule.cycle_month,
    schedule.cycle_day,
    schedule.disp_cycle,
    schedule.schedule_date,
    CONCAT_WS('|',machine.machine_id, man_com.management_standards_component_id,man_con.management_standards_content_id) AS key_id, -- スケジュールと同じ値
    -- スケジュール確定排他チェック用
    schedule_updtime.schedule_head_updtime,
    schedule_updtime.schedule_detail_updtime,
    -- 行削除排他チェック用
    machine.attachment_update_datetime,
    -- 主キー退避
    @LongPlanId AS long_plan_id,
    -- スケジュールマークグループ用(スケジュールと同じ値)
    CONCAT_WS('|',machine.machine_id, man_con.kind_order ) AS same_mark_key
FROM
    base
    INNER JOIN
        machine
    ON  (
            machine.machine_id = base.machine_id
        )
    INNER JOIN
        man_com
    ON  (
            man_com.management_standards_component_id = base.management_standards_component_id
        )
    INNER JOIN
        man_con
    ON  (
            man_con.management_standards_content_id = base.management_standards_content_id
        )
    LEFT OUTER JOIN
        schedule
    ON  (
            schedule.management_standards_content_id_schedule = base.management_standards_content_id
        )
    LEFT OUTER JOIN
        schedule_updtime
    ON  (
            schedule_updtime.management_standards_content_id = base.management_standards_content_id
        )
ORDER BY
    -- ソートキーはビジネスロジックで指定
