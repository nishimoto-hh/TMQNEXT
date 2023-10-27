/*
 * 長期計画スケジュール
*/
-- 基本テーブル
-- 指定された長期計画件名IDに紐づく機番情報、機器別管理基準部位、機器別管理基準内容のキー
-- このテーブルを画面の計画内容に応じて集計し、キーを指定する
WITH base AS(
    SELECT
        machine.machine_id,                                     -- 機番ID
        man_com.management_standards_component_id,              -- 機器別管理基準部位ID
        man_con.management_standards_content_id                 -- 機器別管理基準内容ID
        ,temp.factoryId
        ,temp.languageId
    FROM
        mc_management_standards_content AS man_con              -- 機器別管理基準内容
        INNER JOIN
            #temp temp                                          -- 一時テーブル（一覧画面にて選択）
        ON (
               man_con.long_plan_id = temp.Key1                 -- 長計件名ID
            )
        INNER JOIN
            mc_management_standards_component AS man_com        -- 機器別管理基準部位
        ON  (
                -- 機器別管理基準部位ID
                man_con.management_standards_component_id = man_com.management_standards_component_id
            )
        INNER JOIN
            mc_machine AS machine                               -- 機番情報
        ON  (
                man_com.machine_id = machine.machine_id         -- 機番ID
            )
            
--    WHERE man_con.long_plan_id = @LongPlanId                        -- 長計件名ID
--    WHERE man_con.long_plan_id in (1,2,135)                       -- 長計件名ID
/*@UnComp
    AND EXISTS(
            SELECT
                *
            FROM
                mc_maintainance_schedule_detail AS schedule_detail
                INNER JOIN
                    mc_maintainance_schedule AS schedule
                ON  (
                        schedule_detail.maintainance_schedule_id = schedule.maintainance_schedule_id
                    )
            WHERE
                schedule.management_standards_content_id = man_con.management_standards_content_id
            AND schedule_detail.summary_id IS NULL
        )
@UnComp*/
),
-- 各テーブル
-- 集計に応じて値が不要なテーブルがあるので、各SQLでJOINする
-- 表示する列をそれぞれ取得
-- 機番情報
machine AS(
    SELECT
        machine_id,                                             -- 機番ID
        machine_no,                                             -- 機器番号
        machine_name,                                           -- 機器名称
        importance_structure_id,                                -- 重要度
        -- 添付情報の更新日時
        (
            SELECT
                MAX(att.update_datetime)                        -- 更新日時
            FROM
                mc_equipment AS eq                              -- 機器情報
                LEFT OUTER JOIN
                    attachment AS att                           -- 添付情報
                ON  (
                        eq.equipment_id = att.key_id            -- 機器ID
                    AND att.function_type_id = 1640
                    )
            WHERE
                eq.machine_id = machine.machine_id              -- 機番ID
        ) AS attachment_update_datetime
    FROM
        mc_machine AS machine                                   -- 機番情報
),
-- 機器別管理基準部位
man_com AS(
    SELECT
        management_standards_component_id,                      -- 機器別管理基準部位ID
        update_serialid AS update_serialid_component,           -- 更新シリアルID
        inspection_site_structure_id                            -- 部位ID
    FROM
        mc_management_standards_component                       -- 機器別管理基準部位
),
-- 機器別管理基準内容
man_con AS(
    SELECT
        con.management_standards_content_id,                    -- 機器別管理基準内容ID
        con.update_serialid AS update_serialid_content,         -- 更新シリアルID
        con.inspection_content_structure_id,                    -- 点検内容ID
        con.budget_amount,                                      -- 予算金額
        con.maintainance_kind_structure_id,                     -- 点検種別
--        item_ex.extension_data AS kind_order,
        con.long_plan_id,                                       -- 長計件名ID
        con.schedule_type_structure_id                          -- スケジュール管理基準ID
    FROM
        mc_management_standards_content AS con                  -- 機器別管理基準内容
--        LEFT OUTER JOIN
--            v_structure_item AS item
--        ON  (
--                item.structure_group_id = 1240
--            AND item.structure_id = con.maintainance_kind_structure_id
--            AND item.factory_id IN @FactoryIdList
--            AND item.language_id = @LanguageId
--            )
--        LEFT OUTER JOIN
--            ms_item_extension AS item_ex
--        ON  (
--                item_ex.item_id = item.structure_item_id
--            AND item_ex.sequence_no = 1
--            )
/*
 * 件名別長期計画一覧　参照画面　保全情報一覧の検索　保全項目
*/
-- WITH句の続き
),
-- 保全スケジュール
schedule AS(
    SELECT
        schedule.maintainance_schedule_id,                      -- 保全スケジュールID
        -- 機器別管理基準内容ID
        schedule.management_standards_content_id AS management_standards_content_id_schedule,
        schedule.start_date,                                    -- 開始日
        schedule.cycle_year,                                    -- 周期(年)
        schedule.cycle_month,                                   -- 周期(月)
        schedule.cycle_day,                                     -- 周期(日)
        schedule.disp_cycle,                                    -- 表示周期
        schedule_detail_sum.schedule_date                       -- スケジュール日
    FROM
        (
            -- 機器別管理基準内容IDごとに最大の開始日時をもつものを取得
            SELECT
                main.maintainance_schedule_id,                  -- 保全スケジュールID
                main.management_standards_content_id,           -- 機器別管理基準内容ID
                main.start_date,                                -- 開始日
                main.cycle_year,                                -- 周期(年)
                main.cycle_month,                               -- 周期(月)
                main.cycle_day,                                 -- 周期(日)
                main.disp_cycle                                 -- 表示周期
            FROM
                mc_maintainance_schedule AS main                -- 保全スケジュール
            WHERE
                NOT EXISTS(
                    SELECT
                        *
                    FROM
                        mc_maintainance_schedule AS sub         -- 保全スケジュール
                    WHERE
                        -- 機器別管理基準内容ID
                        main.management_standards_content_id = sub.management_standards_content_id
                    AND main.start_date < sub.start_date        -- 開始日
                )
        ) AS schedule                                           -- スケジュール
        -- スケジュール詳細から保全スケジュールIDごとに最小のスケジュール日を取得
        LEFT OUTER JOIN
            (
                SELECT
                    schedule_detail.maintainance_schedule_id,   -- 保全スケジュールID
                    -- スケジュール日
                    MIN(schedule_detail.schedule_date) AS schedule_date
                FROM
                    -- 保全スケジュール詳細
                    mc_maintainance_schedule_detail AS schedule_detail
                WHERE
                    complition = 0                              -- 完了フラグ
                AND EXISTS(
                        SELECT
                            *
                        FROM
                            ma_summary AS summary               -- 保全活動件名
                        INNER JOIN
                            #temp temp                          -- 一時テーブル（画面にて選択）
                        ON  (
                            summary.long_plan_id = temp.Key1    -- 長計件名ID
                            )
                        WHERE
                            -- 保全活動件名ID
                            schedule_detail.summary_id = summary.summary_id
---------------------------------------------------------------------
--                        AND summary.long_plan_id = @LongPlanId
--                        AND summary.long_plan_id in (1,2,135)
---------------------------------------------------------------------
                    )
                GROUP BY
                    schedule_detail.maintainance_schedule_id    -- 保全スケジュールID
            ) AS schedule_detail_sum                            -- 保全スケジュール詳細グループ化
        ON  (
                -- 保全スケジュールID
                schedule.maintainance_schedule_id = schedule_detail_sum.maintainance_schedule_id
            )
),
-- スケジュール確定排他チェック用更新日時
schedule_updtime AS(
    SELECT
        sc_h.management_standards_content_id,                   -- 機器別管理基準内容ID
        MAX(sc_h.update_datetime) AS schedule_head_updtime,     -- 更新日時
        MAX(sc_d.update_datetime) AS schedule_detail_updtime    -- 更新日時
    FROM
        mc_maintainance_schedule AS sc_h                        -- 保全スケジュール
        LEFT OUTER JOIN
            mc_maintainance_schedule_detail AS sc_d             -- 保全スケジュール詳細
        ON  (
                -- 保全スケジュールID
                sc_h.maintainance_schedule_id = sc_d.maintainance_schedule_id
            )
    WHERE
        sc_d.schedule_date IS NOT NULL                          -- スケジュール日
    AND sc_d.complition != 1                                    -- 完了フラグ
    AND sc_d.schedule_date                                      -- スケジュール日
        BETWEEN @StartDate AND @EndDate
    GROUP BY
        sc_h.management_standards_content_id                    -- 機器別管理基準内容ID
)
SELECT
    lp.subject AS subject,                                      -- 件名
--    vwki.translation_text AS work_item_name,                    -- 作業項目名称
--    viss.translation_text AS inspection_site_name,              -- 保全部位
--    vcon.translation_text AS inspection_content_name,           -- 保全項目
    [dbo].[get_v_structure_item](lp.work_item_structure_id, base.factoryId, base.languageId) AS work_item_name,                         -- 作業項目名称
    [dbo].[get_v_structure_item](man_com.inspection_site_structure_id, base.factoryId, base.languageId) AS inspection_site_name,        -- 保全部位
    [dbo].[get_v_structure_item](man_con.inspection_content_structure_id, base.factoryId, base.languageId) AS inspection_content_name,  -- 保全項目
--    machine.machine_id,                                         -- 機番ID
    machine.machine_no,                                         -- 機器番号
    machine.machine_name,                                       -- 機器名称
--    machine.importance_structure_id,                            -- 重要度
--    machine.attachment_update_datetime,                         -- 更新日時
--    man_com.management_standards_component_id,                  -- 機器別管理基準部位ID
--    man_com.update_serialid_component,                          -- 更新シリアルID
--    man_com.inspection_site_structure_id,                       -- 部位ID
--    man_con.management_standards_content_id,                    -- 機器別管理基準内容ID
--    man_con.update_serialid_content,                            -- 更新シリアルID
--    man_con.inspection_content_structure_id,                    -- 点検内容ID
--    man_con.budget_amount,                                      -- 予算金額
--    man_con.maintainance_kind_structure_id,                     -- 点検種別
----    man_con.kind_order,                                           -- 
--    man_con.long_plan_id,                                       -- 長計件名ID
--    man_con.schedule_type_structure_id,                         -- スケジュール管理基準ID
--    schedule.maintainance_schedule_id,                          -- 保全スケジュールID
--    schedule.management_standards_content_id_schedule,          -- 機器別管理基準内容ID
--    schedule.start_date,                                        -- 開始日
    schedule.cycle_year,                                        -- 周期(年)
    schedule.cycle_month,                                       -- 周期(月)
    schedule.cycle_day,                                         -- 周期(日)
--    schedule.disp_cycle,                                        -- 表示周期
--    schedule.schedule_date                                     -- 次回実施予定日（スケジュール日）
    FORMAT(schedule.schedule_date, 'yyyy/MM/dd') AS schedule_date,  -- 次回実施予定日（スケジュール日）
    CONCAT_WS('|',man_con.long_plan_id, machine.machine_id, man_com.management_standards_component_id,man_con.management_standards_content_id) AS key_id -- スケジュールと同じ値
    -- スケジュール確定排他チェック用
--    schedule_updtime.schedule_head_updtime,
--    schedule_updtime.schedule_detail_updtime,
    -- 行削除排他チェック用
--    machine.attachment_update_datetime
    -- 主キー退避
--    @LongPlanId AS long_plan_id
FROM
    base                                                        -- 基本テーブル
    INNER JOIN
        machine                                                 -- 機番情報
    ON  (
            machine.machine_id = base.machine_id                -- 機番ID
        )
    INNER JOIN
        man_com                                                 -- 機器別管理基準部位
    ON  (
            -- 機器別管理基準部位ID
            man_com.management_standards_component_id = base.management_standards_component_id
        )
    INNER JOIN
        man_con                                                 -- 機器別管理基準内容
    ON  (
            -- 機器別管理基準内容ID
            man_con.management_standards_content_id = base.management_standards_content_id
        )
    LEFT OUTER JOIN
        schedule                                                -- 保全スケジュール
    ON  (
            -- 保全スケジュールID
            schedule.management_standards_content_id_schedule = base.management_standards_content_id
        )
    LEFT OUTER JOIN
        schedule_updtime                                        -- スケジュール確定排他チェック用更新日時
    ON  (
            -- 機器別管理基準内容ID
            schedule_updtime.management_standards_content_id = base.management_standards_content_id
        )
    LEFT OUTER JOIN
        ln_long_plan lp
    ON  (
            lp.long_plan_id = man_con.long_plan_id              -- 長計件名ID
        )
--    LEFT OUTER JOIN
--        v_structure_item vwki                                   -- 作業項目
--    ON  (
--            lp.work_item_structure_id = vwki.structure_id
--        )
--    LEFT OUTER JOIN
--        v_structure_item viss                                   -- 保全部位
--    ON  (
--            man_com.inspection_site_structure_id = viss.structure_id
--        )
--    LEFT OUTER JOIN
--        v_structure_item vcon                                   -- 保全項目
--    ON  (
--            man_con.inspection_content_structure_id = vcon.structure_id
--        )
ORDER BY
    -- ソートキーはビジネスロジックで指定
    -- man_con.long_plan_id
    -- 画面に合わせて件名で並び替え
    lp.subject, man_con.long_plan_id    
    
    
