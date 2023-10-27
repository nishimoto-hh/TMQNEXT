SELECT
    parts.parts_id,                       -- 予備品ID
    parts.parts_no,                       -- 予備品№
    parts.parts_no AS parts_no_before,    -- 予備品№(変更前)
    parts.parts_name,                     -- 予備品名称
    parts.factory_id AS parts_factory_id, -- 管理工場ID
    parts.location_district_structure_id AS district_id,   -- 地区ID
    parts.location_factory_structure_id AS factory_id,     -- 工場ID
    parts.location_warehouse_structure_id AS warehouse_id, -- 倉庫ID
    parts.location_rack_structure_id AS rack_id,           -- 棚ID
    parts.job_structure_id AS job_id,     -- 職種ID
    parts.manufacturer_structure_id,      -- メーカーID
    parts.model_type,                     -- 型式
    parts.materials,                      -- 材質
    parts.standard_size,                  -- 規格・寸法
    parts.use_segment_structure_id,       -- 使用区分
    parts.parts_service_space,            -- 使用場所
    parts.parts_location_id,              -- 標準棚ID
    parts.parts_location_detail_no,       -- 標準棚枝番
    parts.lead_time,                      -- 発注点
    parts.order_quantity,                 -- 発注量
    parts.unit_structure_id,              -- 数量管理単位id
    parts.currency_structure_id,          -- 金額管理単位id
    parts.vender_structure_id,            -- 仕入先ID
    parts.unit_price,                     -- 標準単価
    parts.purchasing_no,                  -- 購買システムコード
    parts.parts_memo,                     -- メモ
    ---------------------------------- 以下は翻訳を取得 ----------------------------------
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
                    st_f.structure_id = parts.manufacturer_structure_id
                AND st_f.factory_id IN(0, parts.factory_id)
            )
        AND tra.structure_id = parts.manufacturer_structure_id
    ) AS manufacture_name, -- メーカー名
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
                    st_f.structure_id = parts.factory_id
                AND st_f.factory_id IN(0, parts.factory_id)
            )
        AND tra.structure_id = parts.factory_id
    ) AS parts_factory_name, -- 管理工場名
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
                    st_f.structure_id = parts.unit_structure_id
                AND st_f.factory_id IN(0, parts.factory_id)
            )
        AND tra.structure_id = parts.unit_structure_id
    ) AS unit_name, -- 数量管理単位
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
                    st_f.structure_id = parts.currency_structure_id
                AND st_f.factory_id IN(0, parts.factory_id)
            )
        AND tra.structure_id = parts.currency_structure_id
    ) AS currency_name, -- 金額管理単位
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
                    st_f.structure_id = parts.vender_structure_id
                AND st_f.factory_id IN(0, parts.factory_id)
            )
        AND tra.structure_id = parts.vender_structure_id
    ) AS vender_name, -- 標準仕入先
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
                    st_f.structure_id = parts.use_segment_structure_id
                AND st_f.factory_id IN(0, parts.factory_id)
            )
        AND tra.structure_id = parts.use_segment_structure_id
    ) AS use_segment_name, -- 使用区分
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
                    st_f.structure_id = parts.location_district_structure_id
                AND st_f.factory_id IN(0, parts.factory_id)
            )
        AND tra.structure_id = parts.location_district_structure_id
    ) AS district_name, -- 地区
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
                    st_f.structure_id = parts.location_factory_structure_id
                AND st_f.factory_id IN(0, parts.factory_id)
            )
        AND tra.structure_id = parts.location_factory_structure_id
    ) AS factory_name, -- 工場
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
                    st_f.structure_id = parts.location_warehouse_structure_id
                AND st_f.factory_id IN(0, parts.factory_id)
            )
        AND tra.structure_id = parts.location_warehouse_structure_id
    ) AS warehouse_name, -- 標準予備品倉庫
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
                    st_f.structure_id = parts.location_rack_structure_id
                AND st_f.factory_id IN(0, parts.factory_id)
            )
        AND tra.structure_id = parts.location_rack_structure_id
    ) AS rack_name, -- 標準棚
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
                    st_f.structure_id = parts.job_structure_id
                AND st_f.factory_id IN(0, parts.factory_id)
            )
        AND tra.structure_id = parts.job_structure_id
    ) AS job_name -- 職種
FROM
    pt_parts AS parts
WHERE EXISTS(SELECT * FROM #temp_structure_selected temp WHERE temp.structure_group_id = 1000 AND parts.factory_id = temp.structure_id)
AND EXISTS(SELECT * FROM #temp_structure_selected temp WHERE temp.structure_group_id = 1010 AND  parts.job_structure_id = temp.structure_id)