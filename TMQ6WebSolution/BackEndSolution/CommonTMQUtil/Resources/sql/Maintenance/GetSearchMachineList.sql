SELECT
    *
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId
            AND tra.location_structure_id = ( 
                -- 工場IDまたは0で合致する最大の工場IDを取得→工場IDのレコードがなければ0となる
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = target.equipment_level_structure_id 
                    AND st_f.factory_id IN (0, target.factoryId)
            ) 
            AND tra.structure_id = target.equipment_level_structure_id
    ) AS equipment_level  --機器レベル(翻訳)
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId
            AND tra.location_structure_id = ( 
                -- 工場IDまたは0で合致する最大の工場IDを取得→工場IDのレコードがなければ0となる
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = target.importance_structure_id 
                    AND st_f.factory_id IN (0, target.factoryId)
            ) 
            AND tra.structure_id = target.importance_structure_id
    ) AS importance_name  --機器重要度(翻訳)
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId
            AND tra.location_structure_id = ( 
                -- 工場IDまたは0で合致する最大の工場IDを取得→工場IDのレコードがなければ0となる
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = target.inspection_site_conservation_structure_id 
                    AND st_f.factory_id IN (0, target.factoryId)
            ) 
            AND tra.structure_id = target.inspection_site_conservation_structure_id
    ) AS inspection_site_conservation_name  --保全方式(翻訳)
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId
            AND tra.location_structure_id = ( 
                -- 工場IDまたは0で合致する最大の工場IDを取得→工場IDのレコードがなければ0となる
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = target.manufacturer_structure_id 
                    AND st_f.factory_id IN (0, target.factoryId)
            ) 
            AND tra.structure_id = target.manufacturer_structure_id
    ) AS manufacturer_name  --メーカー(翻訳)
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId
            AND tra.location_structure_id = ( 
                -- 工場IDまたは0で合致する最大の工場IDを取得→工場IDのレコードがなければ0となる
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = target.use_segment_structure_id 
                    AND st_f.factory_id IN (0, target.factoryId)
            ) 
            AND tra.structure_id = target.use_segment_structure_id
    ) AS use_segment_name  --使用区分(翻訳)
FROM
    translate_target AS target 