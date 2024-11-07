WITH structure_factory AS (
    SELECT
        structure_id,
        location_structure_id AS factory_id
    FROM
        v_structure_item_all
    WHERE
        structure_group_id IN (1170,1200,1180,1030,1240,1220)
        AND language_id = @LanguageId
),
-- 保全スケジュールを機器別管理基準内容IDごとに取得(同じ値なら最大の開始日のレコード)
schedule_start_date AS (
    SELECT
        sc.maintainance_schedule_id,
        sc.management_standards_content_id,
        sc.cycle_year,
        sc.cycle_month,
        sc.cycle_day,
        sc.start_date,
        sc.is_cyclic,
        sc.disp_cycle,
        sc.update_datetime
    FROM
        mc_maintainance_schedule AS sc
    WHERE
        NOT EXISTS (
            SELECT *
            FROM mc_maintainance_schedule AS sub
            WHERE sc.management_standards_content_id = sub.management_standards_content_id
              AND sc.start_date < sub.start_date
        )
),
-- 上で取得した保全スケジュールを機器別管理基準内容ID、開始日ごとに取得(同じ値なら最大の更新日時のレコード)
schedule_content AS (
    SELECT
        main.maintainance_schedule_id,
        main.management_standards_content_id,
        main.cycle_year,
        main.cycle_month,
        main.cycle_day,
        main.start_date,
        main.is_cyclic,
        main.disp_cycle,
        main.update_datetime
    FROM
        schedule_start_date AS main
    WHERE
        NOT EXISTS (
            SELECT *
            FROM schedule_start_date AS sub
            WHERE main.management_standards_content_id = sub.management_standards_content_id
              AND main.start_date = sub.start_date
              AND main.update_datetime < sub.update_datetime
        )
)

SELECT
    mcp.management_standards_component_id,        -- 機器別管理基準部位ID
    mcp.machine_id,                               -- 機番ID
    ma.equipment_level_structure_id,              -- 機器レベル
    (
        SELECT tra.translation_text
        FROM v_structure_item_all AS tra
        WHERE tra.language_id = @LanguageId
          AND tra.location_structure_id = (
              SELECT MAX(st_f.factory_id)
              FROM structure_factory AS st_f
              WHERE st_f.structure_id = ma.equipment_level_structure_id
                AND st_f.factory_id IN (0, ma.location_factory_structure_id)
          )
          AND tra.structure_id = ma.equipment_level_structure_id
    ) AS equipment_level_name,
    ma.machine_no,                                -- 機器番号
    ma.machine_name,                              -- 機器名称
    ma.importance_structure_id,                   -- 機器重要度
    (
        SELECT tra.translation_text
        FROM v_structure_item_all AS tra
        WHERE tra.language_id = @LanguageId
          AND tra.location_structure_id = (
              SELECT MAX(st_f.factory_id)
              FROM structure_factory AS st_f
              WHERE st_f.structure_id = ma.importance_structure_id
                AND st_f.factory_id IN (0, ma.location_factory_structure_id)
          )
          AND tra.structure_id = ma.importance_structure_id
    ) AS importance_name,-- 機器重要度
    mcp.inspection_site_structure_id,             -- 部位ID
    (
        SELECT tra.translation_text
        FROM v_structure_item_all AS tra
        WHERE tra.language_id = @LanguageId
          AND tra.location_structure_id = (
              SELECT MAX(st_f.factory_id)
              FROM structure_factory AS st_f
              WHERE st_f.structure_id = mcp.inspection_site_structure_id
                AND st_f.factory_id IN (0, ma.location_factory_structure_id)
          )
          AND tra.structure_id = mcp.inspection_site_structure_id
    ) AS inspection_site_name,                   -- 部位名称
    msc.inspection_site_importance_structure_id,  -- 部位重要度ID
    (
        SELECT tra.translation_text
        FROM v_structure_item_all AS tra
        WHERE tra.language_id = @LanguageId
          AND tra.location_structure_id = (
              SELECT MAX(st_f.factory_id)
              FROM structure_factory AS st_f
              WHERE st_f.structure_id = msc.inspection_site_importance_structure_id
                AND st_f.factory_id IN (0, ma.location_factory_structure_id)
          )
          AND tra.structure_id = msc.inspection_site_importance_structure_id
    ) AS inspection_site_importance_nam, -- 部位重要度
    msc.inspection_site_conservation_structure_id,-- 部位保全方式ID
    (
        SELECT tra.translation_text
        FROM v_structure_item_all AS tra
        WHERE tra.language_id = @LanguageId
          AND tra.location_structure_id = (
              SELECT MAX(st_f.factory_id)
              FROM structure_factory AS st_f
              WHERE st_f.structure_id = msc.inspection_site_conservation_structure_id
                AND st_f.factory_id IN (0, ma.location_factory_structure_id)
          )
          AND tra.structure_id = msc.inspection_site_conservation_structure_id
    ) AS inspection_site_conservation_n, -- 部位保全方式名称
    mcp.is_management_standard_conponent,         -- 機器別管理基準フラグ
    msc.management_standards_content_id,          -- 機器別管理基準内容ID
    msc.inspection_content_structure_id,          -- 点検内容ID
    (
        SELECT tra.translation_text
        FROM v_structure_item_all AS tra
        WHERE tra.language_id = @LanguageId
          AND tra.location_structure_id = (
              SELECT MAX(st_f.factory_id)
              FROM structure_factory AS st_f
              WHERE st_f.structure_id = msc.inspection_content_structure_id
                AND st_f.factory_id IN (0, ma.location_factory_structure_id)
          )
          AND tra.structure_id = msc.inspection_content_structure_id
    ) AS inspection_content_name, -- 点検内容
    msc.maintainance_division,                    -- 保全区分
    (
        SELECT tra.translation_text
        FROM v_structure_item_all AS tra
        WHERE tra.language_id = @LanguageId
          AND tra.location_structure_id = (
              SELECT MAX(st_f.factory_id)
              FROM structure_factory AS st_f
              WHERE st_f.structure_id = msc.maintainance_division
                AND st_f.factory_id IN (0, ma.location_factory_structure_id)
          )
          AND tra.structure_id = msc.maintainance_division
    ) AS maintainance_division_name,      -- 保全区分
    msc.maintainance_kind_structure_id,           -- 点検種別ID
    (
        SELECT tra.translation_text
        FROM v_structure_item_all AS tra
        WHERE tra.language_id = @LanguageId
          AND tra.location_structure_id = (
              SELECT MAX(st_f.factory_id)
              FROM structure_factory AS st_f
              WHERE st_f.structure_id = msc.maintainance_kind_structure_id
                AND st_f.factory_id IN (0, ma.location_factory_structure_id)
          )
          AND tra.structure_id = msc.maintainance_kind_structure_id
    ) AS maintainance_kind_name,                 -- 点検種別
    msc.budget_amount,                            -- 予算金額
    msc.preparation_period,                       -- 準備期間(日)
    msc.order_no,                                 -- 並び順
    msc.long_plan_id,                             -- 長期計画件名ID
    msc.schedule_type_structure_id,               -- スケジュール管理区分
    sc.maintainance_schedule_id,                  -- 保全スケジュールID
    sc.is_cyclic,                                 -- 周期ありフラグ
    sc.cycle_year,                                -- 周期(年)
    sc.cycle_month,                               -- 周期(月)
    sc.cycle_day,                                 -- 周期(日)
    CASE WHEN ex.extension_data = '1' 
    THEN   -- 1:定期検査であれば点検内容ID
        (
            SELECT tra.translation_text
            FROM v_structure_item_all AS tra
            WHERE tra.language_id = @LanguageId
              AND tra.location_structure_id = (
                  SELECT MAX(st_f.factory_id)
                  FROM structure_factory AS st_f
                  WHERE st_f.structure_id = msc.inspection_content_structure_id
                    AND st_f.factory_id IN (0, ma.location_factory_structure_id)
              )
              AND tra.structure_id = msc.inspection_content_structure_id
        )
    ELSE null
    END AS periodic_inspection,                   -- 定期検査・内容
    CASE WHEN ex.extension_data = '1' THEN sc.disp_cycle -- 1:定期検査であれば表示周期
    ELSE null
    END AS periodic_cycle,                        -- 定期検査・周期

    CASE WHEN ex.extension_data = '2' 
    THEN   -- 2:定期修理であれば点検内容ID
        (
            SELECT tra.translation_text
            FROM v_structure_item_all AS tra
            WHERE tra.language_id = @LanguageId
              AND tra.location_structure_id = (
                  SELECT MAX(st_f.factory_id)
                  FROM structure_factory AS st_f
                  WHERE st_f.structure_id = msc.inspection_content_structure_id
                    AND st_f.factory_id IN (0, ma.location_factory_structure_id)
              )
              AND tra.structure_id = msc.inspection_content_structure_id
        )
    ELSE null
    END AS repair_inspection,                   -- 定期修理・内容
    CASE WHEN ex.extension_data = '2' THEN sc.disp_cycle -- 2:定期修理であれば表示周期
    ELSE null
    END AS repair_cycle,                        -- 定期修理・周期

    CASE WHEN ex.extension_data = '3' 
    THEN   -- 3:日常点検であれば点検内容ID
        (
            SELECT tra.translation_text
            FROM v_structure_item_all AS tra
            WHERE tra.language_id = @LanguageId
              AND tra.location_structure_id = (
                  SELECT MAX(st_f.factory_id)
                  FROM structure_factory AS st_f
                  WHERE st_f.structure_id = msc.inspection_content_structure_id
                    AND st_f.factory_id IN (0, ma.location_factory_structure_id)
              )
              AND tra.structure_id = msc.inspection_content_structure_id
        )
    ELSE null
    END AS daily_inspection,                   -- 日常点検・内容
    CASE WHEN ex.extension_data = '3' THEN sc.disp_cycle -- 3:日常点検であれば表示周期
    ELSE null
    END AS daily_cycle,                        -- 日常点検・周期
    sc.disp_cycle,                              -- 表示周期
    sc.start_date,                              -- 開始日
    -- 3テーブルのうち最大更新日付を取得
    CASE WHEN mcp.update_datetime > msc.update_datetime AND mcp.update_datetime > sc.update_datetime THEN mcp.update_datetime
        WHEN msc.update_datetime > mcp.update_datetime AND msc.update_datetime > sc.update_datetime THEN msc.update_datetime
        ELSE sc.update_datetime
    END update_datetime                         -- 更新日付
FROM 
    mc_management_standards_component mcp -- 機器別管理基準部位
    JOIN mc_management_standards_content msc   -- 機器別管理基準内容
        ON mcp.management_standards_component_id = msc.management_standards_component_id
    JOIN schedule_content sc   -- 保全スケジュール
        ON msc.management_standards_content_id = sc.management_standards_content_id
    JOIN mc_machine ma          -- 機番情報
        ON mcp.machine_id = ma.machine_id
    JOIN (
        SELECT it.structure_id, ex.extension_data
        FROM ms_structure it
        JOIN ms_item_extension ex
            ON it.structure_item_id = ex.item_id
        WHERE it.structure_group_id = 1230
        AND ex.extension_data IN ('1','2','3') -- 保全区分が定期検査or定期修理or日常点検のみ表示
    ) ex ON msc.maintainance_division = ex.structure_id -- 保全区分
WHERE
    mcp.is_management_standard_conponent = 1    -- 機器別管理基準フラグ
    AND mcp.machine_id = @MachineId
ORDER BY
    msc.order_no, 
    mcp.inspection_site_structure_id,
    msc.inspection_content_structure_id; -- 並び順
