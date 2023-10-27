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
                    st_f.structure_id = target.old_new_structure_id 
                    AND st_f.factory_id IN (0, target.factory_id)
            ) 
            AND tra.structure_id = target.old_new_structure_id
    ) AS old_new_nm  --新旧区分(翻訳)
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
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = target.unit_structure_id 
                    AND st_f.factory_id IN (0, target.factory_id)
            ) 
            AND tra.structure_id = target.unit_structure_id
    ) AS unit  --数量単位名称(翻訳)
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
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = target.department_structure_id 
                    AND st_f.factory_id IN (0, target.factory_id)
            ) 
            AND tra.structure_id = target.department_structure_id
    ) AS department_nm  --部門(翻訳)
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
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = target.account_structure_id 
                    AND st_f.factory_id IN (0, target.factory_id)
            ) 
            AND tra.structure_id = target.account_structure_id
    ) AS subject_nm  --勘定科目(翻訳)
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
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = target.manufacturer_structure_id 
                    AND st_f.factory_id IN (0, target.factory_id)
            ) 
            AND tra.structure_id = target.manufacturer_structure_id
    ) AS manufacturer_name  --メーカー(翻訳)
FROM
    translate_target AS target 