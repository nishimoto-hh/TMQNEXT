WITH management_standards_transaction AS(
    SELECT
        mcp.management_standards_component_id,         -- 機器別管理基準部位ID
        mcp.machine_id,                                -- 機番ID
        ma.equipment_level_structure_id,               -- 機器レベル
        ma.machine_no,                                 -- 機器番号
        ma.machine_name,                               -- 機器名称
        eq.maintainance_kind_manage,                   -- 点検種別毎管理
        mcp.inspection_site_structure_id,              -- 部位ID
        msc.inspection_site_importance_structure_id,   -- 部位重要度ID
        msc.inspection_site_conservation_structure_id, -- 部位保全方式ID
        mcp.is_management_standard_conponent,          -- 機器別管理基準フラグ
        msc.management_standards_content_id,           -- 機器別管理基準内容ID
        msc.inspection_content_structure_id,           -- 点検内容ID
        msc.maintainance_division,                     -- 保全区分
        msc.maintainance_kind_structure_id,            -- 点検種別ID
        msc.budget_amount,                             -- 予算金額
        msc.preparation_period,                        -- 準備期間(日)
        msc.order_no,                                  -- 並び順
        msc.long_plan_id,                              -- 長期計画件名ID
        msc.schedule_type_structure_id,                -- スケジュール管理区分
        ms.maintainance_schedule_id,                   -- 保全スケジュールID
        ms.is_cyclic,                                  -- 周期ありフラグ
        ms.cycle_year,                                 -- 周期(年)
        ms.cycle_month,                                -- 周期(月)
        ms.cycle_day,                                  -- 周期(日)
        ms.disp_cycle,                                 -- 表示周期
        ms.start_date,                                 -- 開始日
        -- 3テーブルのうち最大更新日付を取得
        CASE
            WHEN mcp.update_datetime > msc.update_datetime
        AND mcp.update_datetime > ms.update_datetime THEN mcp.update_datetime
            WHEN msc.update_datetime > mcp.update_datetime
        AND msc.update_datetime > ms.update_datetime THEN msc.update_datetime
            ELSE ms.update_datetime
        END update_datetime,                          -- 更新日付
        dbo.get_file_download_info(1620, msc.management_standards_content_id) AS attachment_file,-- 添付ファイル
        (
            SELECT
                MAX(ac.update_datetime)
            FROM
                attachment ac
            WHERE
                function_type_id = 1620
            AND key_id = msc.management_standards_content_id
        ) AS max_update_datetime--添付ファイルの最大更新日時
    FROM
        mc_management_standards_component mcp -- 機器別管理基準部位
        , mc_management_standards_content msc -- 機器別管理基準内容
        ,
        (
            SELECT
                a.*
            FROM
                mc_maintainance_schedule AS a -- 保全スケジュール
                INNER JOIN
                    -- 機器別管理基準内容IDごとの開始日最新データを取得
                    (
                        SELECT
                            management_standards_content_id,
                            MAX(start_date) AS start_date,
                            MAX(update_datetime) AS update_datetime
                        FROM
                            mc_maintainance_schedule
                        GROUP BY
                            management_standards_content_id
                    ) b
                ON  a.management_standards_content_id = b.management_standards_content_id
                AND (
                        a.start_date = b.start_date
                    OR  a.start_date IS NULL
                    AND b.start_date IS NULL
                        --null結合を考慮
                    )
                AND (
                        a.update_datetime = b.update_datetime
                    OR  a.update_datetime IS NULL
                    AND b.update_datetime IS NULL
                        --null結合を考慮
                    )
        ) ms, -- 保全スケジュール
        mc_machine ma,
        mc_equipment eq
    WHERE
        mcp.management_standards_component_id = msc.management_standards_component_id
    AND msc.management_standards_content_id = ms.management_standards_content_id
    AND mcp.machine_id = ma.machine_id
    AND ma.machine_id = eq.machine_id
    AND mcp.is_management_standard_conponent = 1 -- 機器別管理基準フラグ
AND mcp.machine_id = @MachineId
),
management_standards_history AS(
    SELECT
        history.history_management_id,
        hcomponent.hm_management_standards_component_id,
        hcontent.hm_management_standards_content_id,
        hcontent.execution_division,
        hcomponent.management_standards_component_id,
        hcomponent.machine_id,
        COALESCE(hmachine.equipment_level_structure_id, machine.equipment_level_structure_id) AS equipment_level_structure_id,
        COALESCE(hmachine.machine_no, machine.machine_no) AS machine_no,
        COALESCE(hmachine.machine_name, machine.machine_name) AS machine_name,
        COALESCE(hequipment.maintainance_kind_manage, equipment.maintainance_kind_manage) AS maintainance_kind_manage,
        hcomponent.inspection_site_structure_id,
        hcontent.inspection_site_importance_structure_id,
        hcontent.inspection_site_conservation_structure_id,
        hcomponent.is_management_standard_conponent,
        hcontent.management_standards_content_id,
        hcontent.inspection_content_structure_id,
        hcontent.maintainance_division,
        hcontent.maintainance_kind_structure_id,
        hcontent.budget_amount,
        hcontent.preparation_period,
        hcontent.order_no,
        hcontent.long_plan_id,
        hcontent.schedule_type_structure_id,
        schedule.hm_maintainance_schedule_id,
        schedule.maintainance_schedule_id,
        schedule.is_cyclic,
        schedule.cycle_year,
        schedule.cycle_month,
        schedule.cycle_day,
        schedule.disp_cycle,
        schedule.start_date,
        -- 3テーブルのうち最大更新日付を取得
        CASE
            WHEN hcomponent.update_datetime > hcontent.update_datetime
        AND hcomponent.update_datetime > schedule.update_datetime THEN hcomponent.update_datetime
            WHEN hcontent.update_datetime > hcomponent.update_datetime
        AND hcontent.update_datetime > schedule.update_datetime THEN hcontent.update_datetime
            ELSE schedule.update_datetime
        END update_datetime,
        -- 更新日付
        dbo.get_file_download_info(1620, hcontent.management_standards_content_id) AS attachment_file, -- 添付ファイル
        (
            SELECT
                MAX(ac.update_datetime)
            FROM
                attachment ac
            WHERE
                function_type_id = 1620
            AND key_id = hcontent.management_standards_content_id
        ) AS max_update_datetime --添付ファイルの最大更新日時
    FROM
        hm_history_management history -- 変更管理
        LEFT JOIN
            hm_mc_management_standards_component hcomponent -- 機器別管理基準部位変更管理
    ON  history.history_management_id = hcomponent.history_management_id
    LEFT JOIN
        mc_management_standards_component component -- 機器別管理基準部位
    ON  hcomponent.management_standards_component_id = component.management_standards_component_id
    LEFT JOIN
        mc_machine machine -- 機番情報
    ON  history.key_id = machine.machine_id
    LEFT JOIN
        mc_equipment equipment -- 機器情報
    ON  machine.machine_id = equipment.machine_id
    LEFT JOIN
        hm_mc_machine hmachine -- 機番情報
    ON  history.history_management_id = hmachine.history_management_id
    LEFT JOIN
        hm_mc_equipment hequipment -- 機器情報
    ON  history.history_management_id = hequipment.history_management_id
    LEFT JOIN
        hm_mc_management_standards_content hcontent -- 機器別管理基準内容変更管理
    ON  history.history_management_id = hcontent.history_management_id
    AND hcomponent.management_standards_component_id = hcontent.management_standards_component_id
    LEFT JOIN
        mc_management_standards_content content -- 機器別管理基準内容
    ON  hcontent.management_standards_content_id = content.management_standards_content_id
    LEFT JOIN
    (
        SELECT
            a.*
        FROM
            hm_mc_maintainance_schedule AS a -- 保全スケジュール
            INNER JOIN -- 機器別管理基準内容IDごとの開始日最新データを取得
                (
                    SELECT
                        management_standards_content_id,
                        MAX(start_date) AS start_date,
                        MAX(update_datetime) AS update_datetime
                    FROM
                        hm_mc_maintainance_schedule
                    GROUP BY
                        management_standards_content_id
                ) b
            ON  a.management_standards_content_id = b.management_standards_content_id
            AND (
                    a.start_date = b.start_date
                OR  a.start_date IS NULL
                AND b.start_date IS NULL
                    --null結合を考慮
                )
            AND (
                    a.update_datetime = b.update_datetime
                OR  a.update_datetime IS NULL
                AND b.update_datetime IS NULL
                    --null結合を考慮
                )
    ) schedule -- 保全スケジュール
ON  hcontent.management_standards_content_id = schedule.management_standards_content_id
    WHERE
        hcomponent.is_management_standard_conponent = 1
    AND history.history_management_id = @HistoryManagementId
),
mamagement_data AS(
    SELECT
        msh.history_management_id,                                                                                                                           -- 変更管理ID
        COALESCE(msh.hm_management_standards_component_id, 0) AS hm_management_standards_component_id,                                                       -- 機器別管理基準部位変更管理ID                       
        COALESCE(msh.hm_management_standards_content_id, 0) AS hm_management_standards_content_id,                                                           -- 機器別管理基準内容変更管理ID                     
        COALESCE(msh.hm_maintainance_schedule_id, 0) AS hm_maintainance_schedule_id,                                                                         -- 保全スケジュール変更管理ID                                             
        CASE
            WHEN msh.execution_division = 5 THEN '20'
            WHEN msh.execution_division = 6 THEN '30'
            ELSE '0'                                                                                                                                
        END AS application_division_code,                                                                                                                    -- 申請区分
        mst.management_standards_component_id,                                                                                                               -- 機器別管理基準部位ID
        mst.machine_id,                                                                                                                                      -- 機番ID
        COALESCE(msh.equipment_level_structure_id, mst.equipment_level_structure_id) AS equipment_level_structure_id,                                        -- 機器レベル
        COALESCE(msh.machine_no, mst.machine_no) AS machine_no,                                                                                              -- 機器番号
        COALESCE(msh.machine_name, mst.machine_name) AS machine_name,                                                                                        -- 機器名称
        mst.maintainance_kind_manage,                                                                                                                        -- 点検種別毎管理
        COALESCE(msh.inspection_site_structure_id, mst.inspection_site_structure_id) AS inspection_site_structure_id,                                        -- 部位ID
        COALESCE(msh.inspection_site_importance_structure_id, mst.inspection_site_importance_structure_id) AS inspection_site_importance_structure_id,       -- 部位重要度ID
        COALESCE(msh.inspection_site_conservation_structure_id, mst.inspection_site_conservation_structure_id) AS inspection_site_conservation_structure_id, -- 部位保全方式ID
        mst.is_management_standard_conponent,                                                                                                                -- 機器別管理基準フラグ
        mst.management_standards_content_id,                                                                                                                 -- 機器別管理基準内容ID
        COALESCE(msh.inspection_content_structure_id, mst.inspection_content_structure_id) AS inspection_content_structure_id,                               -- 点検内容ID
        COALESCE(msh.maintainance_division, mst.maintainance_division) AS maintainance_division,                                                             -- 保全区分
        COALESCE(msh.maintainance_kind_structure_id, mst.maintainance_kind_structure_id) AS maintainance_kind_structure_id,                                  -- 点検種別ID
        COALESCE(msh.budget_amount, mst.budget_amount) AS budget_amount,                                                                                     -- 予算金額
        COALESCE(msh.preparation_period, mst.preparation_period) AS preparation_period,                                                                      -- 準備期間(日)
        mst.order_no,                                                                                                                                        -- 並び順
        mst.long_plan_id,                                                                                                                                    -- 長期計画件名ID
        COALESCE(msh.schedule_type_structure_id, mst.schedule_type_structure_id) AS schedule_type_structure_id,                                              -- スケジュール管理区分
        mst.maintainance_schedule_id,                                                                                                                        -- 保全スケジュールID
        mst.is_cyclic,                                                                                                                                       -- 周期ありフラグ
        COALESCE(msh.cycle_year, mst.cycle_year) AS cycle_year,                                                                                              -- 周期(年)
        COALESCE(msh.cycle_month, mst.cycle_month) AS cycle_month,                                                                                           -- 周期(月)
        COALESCE(msh.cycle_day, mst.cycle_day) AS cycle_day,                                                                                                 -- 周期(日)
        COALESCE(msh.disp_cycle, mst.disp_cycle) AS disp_cycle,                                                                                              -- 表示周期
        COALESCE(msh.start_date, mst.start_date) AS start_date,                                                                                              -- 開始日
        COALESCE(msh.execution_division, 0) AS execution_division,                                                                                           -- 実行処理区分
        mst.update_datetime,                                                                                                                                 -- 更新日付
        mst.attachment_file,                                                                                                                                 -- 添付ファイル
        mst.max_update_datetime,                                                                                                                             --添付ファイルの最大更新日時
        CASE
            WHEN msh.execution_division = 5 THEN 
            dbo.compare_newId_with_oldId(msh.equipment_level_structure_id, mst.equipment_level_structure_id, 'EquipmenLevel') +                                        -- 機器レベル
            dbo.compare_newVal_with_oldVal(msh.machine_no, mst.machine_no, 'MachineNo') +                                                                              -- 機器番号
            dbo.compare_newVal_with_oldVal(msh.machine_name, mst.machine_name, 'MachineName') +                                                                        -- 機器名称
            dbo.compare_newId_with_oldId(msh.inspection_site_structure_id, mst.inspection_site_structure_id, 'InspectionSite') +                                       -- 保全部位
            dbo.compare_newId_with_oldId(msh.inspection_site_importance_structure_id, mst.inspection_site_importance_structure_id, 'InspectionSiteImportance') +       -- 部位重要度
            dbo.compare_newId_with_oldId(msh.inspection_site_conservation_structure_id, mst.inspection_site_conservation_structure_id, 'InspectionSiteConservation') + -- 保全方式
            dbo.compare_newId_with_oldId(msh.maintainance_division, mst.maintainance_division, 'MaintainanceDivision') +                                               -- 保全区分
            dbo.compare_newId_with_oldId(msh.inspection_content_structure_id, mst.inspection_content_structure_id, 'InspectionContent') +                              -- 点々内容(保全区分)
            dbo.compare_newId_with_oldId(msh.maintainance_kind_structure_id, mst.maintainance_kind_structure_id, 'MaintainanceKind') +                                 -- 点検種別 
            dbo.compare_newVal_with_oldVal(msh.budget_amount, mst.budget_amount, 'BudgetAmount') +                                                                     -- 予算金額
            dbo.compare_newId_with_oldId(msh.schedule_type_structure_id, mst.schedule_type_structure_id, 'ScheduleType') +                                             -- スケジュール管理
            dbo.compare_newVal_with_oldVal(msh.preparation_period, mst.preparation_period, 'PreparationPeriod') +                                                      -- 準備期間(日)
            dbo.compare_newVal_with_oldVal(msh.cycle_year, mst.cycle_year, 'CycleYear') +                                                                              -- 周期(年)
            dbo.compare_newVal_with_oldVal(msh.cycle_month, mst.cycle_month, 'CycleMonth') +                                                                           -- 周期(月)
            dbo.compare_newVal_with_oldVal(msh.cycle_day, mst.cycle_day, 'CycleDay') +                                                                                 -- 周期(日)
            dbo.compare_newVal_with_oldVal(msh.disp_cycle, mst.disp_cycle, 'DispCycle') +                                                                              -- 表示周期
            dbo.compare_newVal_with_oldVal(msh.start_date, mst.start_date, 'StartDate')                                                                                -- 開始日
            ELSE ''
        END AS value_changed
    FROM
        management_standards_transaction mst
        LEFT JOIN
            management_standards_history msh
        ON  mst.management_standards_component_id = msh.management_standards_component_id
        AND mst.management_standards_content_id = msh.management_standards_content_id
        AND msh.execution_division IN(5, 6) -- 「保全項目一覧の項目編集」「保全項目一覧の削除」が対象
    UNION ALL
    SELECT
        msh.history_management_id,                     -- 変更管理ID
        msh.hm_management_standards_component_id,      -- 機器別管理基準部位変更杏里ID
        msh.hm_management_standards_content_id,        -- 機器別管理基準内容犯行管理ID 
        msh.hm_maintainance_schedule_id,               -- 保全スケジュール変更管理ID
        '10' AS application_division_code,             -- 申請区分
        msh.management_standards_component_id,         -- 機器別管理基準部位ID
        msh.machine_id,                                -- 機番ID
        msh.equipment_level_structure_id,              -- 機器レベル
        msh.machine_no,                                -- 機器番号
        msh.machine_name,                              -- 機器名称
        msh.maintainance_kind_manage,                  -- 点検種別毎管理
        msh.inspection_site_structure_id,              -- 部位ID
        msh.inspection_site_importance_structure_id,   -- 部位重要度ID
        msh.inspection_site_conservation_structure_id, -- 部位保全方式ID
        msh.is_management_standard_conponent,          -- 機器別管理基準フラグ
        msh.management_standards_content_id,           -- 機器別管理基準内容ID
        msh.inspection_content_structure_id,           -- 点検内容ID
        msh.maintainance_division,                     -- 保全区分
        msh.maintainance_kind_structure_id,            -- 点検種別ID
        msh.budget_amount,                             -- 予算金額
        msh.preparation_period,                        -- 準備期間(日)
        msh.order_no,                                  -- 並び順
        msh.long_plan_id,                              -- 長期計画件名ID
        msh.schedule_type_structure_id,                -- スケジュール管理区分
        msh.maintainance_schedule_id,                  -- 保全スケジュールID
        msh.is_cyclic,                                 -- 周期ありフラグ
        msh.cycle_year,                                -- 周期(年)
        msh.cycle_month,                               -- 周期(月)
        msh.cycle_day,                                 -- 周期(日)
        msh.disp_cycle,                                -- 表示周期
        msh.start_date,                                -- 開始日
        msh.execution_division,                        -- 実行処理区分
        msh.update_datetime,                           -- 更新日付
        msh.attachment_file,                           -- 添付ファイル
        msh.max_update_datetime,                       -- 添付ファイルの最大更新日時
        '' AS value_changed                            -- 追加の場合は変更のあった項目なし
    FROM
        management_standards_history msh
    WHERE
        msh.execution_division = 4 -- 「保全項目一覧の追加」が対象
)
SELECT
    *
FROM
    mamagement_data md
/*@ComponentId
-- 保全項目を単一で取得する
where md.hm_management_standards_component_id = @HmManagementStandardsComponentId
@ComponentId*/
ORDER BY
    md.inspection_site_structure_id,
    md.inspection_content_structure_id