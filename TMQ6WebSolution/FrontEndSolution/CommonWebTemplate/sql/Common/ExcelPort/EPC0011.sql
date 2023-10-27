/*
 * ExcelPortダウンロード対象機能 機種別仕様関連付の 仕様項目 コンボ用
 */
-- EPC0011
WITH target AS(
    SELECT
        spec.spec_id AS id,
        re.factory_id,
        NULL AS parent_id,
        (
            SELECT
                tra.translation_text
            FROM
                ms_translation AS tra
            WHERE
                tra.language_id = /*languageId*/'ja'
            AND tra.translation_id = spec.translation_id
            AND tra.location_structure_id = (
                    SELECT
                        MAX(tra.location_structure_id)
                    FROM
                        ms_translation AS tra
                    WHERE
                        tra.language_id = /*languageId*/'ja'
                    AND tra.location_structure_id IN(0, re.factory_id)
                    AND tra.translation_id = spec.translation_id
                )
        ) AS name
    FROM
        ms_spec spec
        LEFT JOIN
            (
                SELECT DISTINCT
                    re.spec_id,
                    re.location_structure_id AS factory_id
                FROM
                    ms_machine_spec_relation re
            ) re
        ON  spec.spec_id = re.spec_id

        LEFT JOIN
            ms_structure ms
        ON  spec.spec_type_id = ms.structure_id
        LEFT JOIN
            ms_item item
        ON  ms.structure_item_id = item.item_id
        LEFT JOIN
            ms_item_extension ex
        ON  item.item_id = ex.item_id
        AND ex.sequence_no = 1

    WHERE EXISTS(SELECT * FROM #temp_structure_selected temp WHERE temp.structure_group_id = 1000 
    AND re.factory_id = temp.structure_id)

    /*IF param1 == 1 */
    -- 「選択」項目のみ
    AND ex.extension_data = '4'
    /*END*/
)
,structure_factory AS ( 
    -- 使用する構成グループの構成IDを絞込、工場の指定に用いる
    SELECT
        item.structure_id
        , item.location_structure_id AS factory_id 
    FROM
        v_structure_item_all item
    WHERE
        item.structure_group_id = 1000
        AND item.structure_layer_no = 1
        AND item.language_id = /*languageId*/'ja'
) 
SELECT
    target.id,
    factory_id,
    target.parent_id,
    target.name,
    target.factory_id AS exparam1,
     ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = /*languageId*/'ja' 
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = target.factory_id
                    AND st_f.factory_id IN (0, target.factory_id)
            ) 
            AND tra.structure_id = target.factory_id
    ) AS exparam2
FROM
    target
ORDER BY
    name DESC