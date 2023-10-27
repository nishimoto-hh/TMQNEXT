SELECT
    ms.structure_id,                            -- 構成ID
    ms.structure_item_id AS item_id,            -- アイテムID
    item.item_translation_id AS translation_id, -- 翻訳ID
    re.factory_id,                              -- 工場ID
    spec.spec_id,                               -- 仕様項目ID
    ord.display_order,                          -- 並び順
    (
        SELECT
            mt.translation_text
        FROM
            ms_translation mt
        WHERE
            mt.location_structure_id = re.factory_id
        AND mt.translation_id = spec.translation_id
        AND mt.language_id = @LanguageId
    ) spec_name,                                -- 仕様項目名
    (
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                SELECT
                    MIN(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = re.factory_id
                AND st_f.factory_id IN(0, re.factory_id)
            )
        AND tra.structure_id = re.factory_id
    ) AS factory_name,                        -- 工場名
    (
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                SELECT
                    MIN(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = ms.structure_id
                AND st_f.factory_id IN(0, ms.factory_id)
            )
        AND tra.structure_id = ms.structure_id
    ) AS translation_text,                        -- 仕様項目選択肢名
    (
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                SELECT
                    MIN(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = ms.structure_id
                AND st_f.factory_id IN(0, ms.factory_id)
            )
        AND tra.structure_id = ms.structure_id
    ) AS translation_text_before                  -- 仕様項目選択肢名(変更前)
FROM
    ms_structure ms -- 構成マスタ
    LEFT JOIN
        ms_item item -- アイテムマスタ
    ON  ms.structure_item_id = item.item_id
    LEFT JOIN
        ms_item_extension ex -- アイテムマスタ拡張(選択肢と紐付く仕様項目IDが設定されている)
    ON  item.item_id = ex.item_id
    AND ex.sequence_no = 1
    LEFT JOIN
        ms_spec spec -- 仕様項目マスタ
    ON  ex.extension_data = CAST(spec.spec_id AS varchar)
    LEFT JOIN -- 仕様項目IDに対する工場ID
        (
            SELECT DISTINCT
                re.spec_id,
                re.location_structure_id AS factory_id
            FROM
                ms_machine_spec_relation re
        ) re
    ON  spec.spec_id = re.spec_id
    LEFT JOIN -- 並び順
        ms_structure_order ord
    ON  ms.structure_id = ord.structure_id
    AND ord.factory_id = re.factory_id
    LEFT JOIN -- 機種別仕様関連付けの並び順
        (
            SELECT DISTINCT
                re.spec_id,
                re.display_order
            FROM
                ms_machine_spec_relation re
        ) relation_order
    ON  spec.spec_id = relation_order.spec_id
WHERE
    ms.structure_group_id = 1340 
AND EXISTS(SELECT * FROM #temp_structure_selected temp WHERE temp.structure_group_id = 1000 AND re.factory_id = temp.structure_id)
AND ms.delete_flg = 0
ORDER BY
    re.factory_id,
    COALESCE(relation_order.display_order, 0),
    spec.spec_id,
    COALESCE(ord.display_order, 0),
    ms.structure_id