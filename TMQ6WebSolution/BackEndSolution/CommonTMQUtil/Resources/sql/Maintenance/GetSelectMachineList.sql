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
                    st_f.structure_id = target.importance_structure_id 
                    AND st_f.factory_id IN (0, target.factoryId)
            ) 
            AND tra.structure_id = target.importance_structure_id
    ) AS importance_name  --機器重要度(翻訳)
    /*@SelectManagementStandards
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
                    st_f.structure_id = target.inspection_site_structure_id 
                    AND st_f.factory_id IN (0, target.factoryId)
            ) 
            AND tra.structure_id = target.inspection_site_structure_id
    ) AS inspection_site_name  --保全部位(翻訳)
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
                    st_f.structure_id = target.inspection_content_structure_id 
                    AND st_f.factory_id IN (0, target.factoryId)
            ) 
            AND tra.structure_id = target.inspection_content_structure_id
    ) AS inspection_content_name  --保全内容(翻訳)
    @SelectManagementStandards*/
FROM
    translate_target AS target