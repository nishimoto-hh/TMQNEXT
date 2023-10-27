--location_structure_id(ツリーで選択されたアイテムを取得する為に記載しています)
SELECT * 
FROM
(
SELECT
    ex.extension_data AS item_extension_data_1, -- 勘定科目コード
    ex.extension_data + ' ' + trim(
        ','
        FROM
            (
                SELECT
                    item2.translation_text + ','
                FROM
                    v_structure_item_all item2
                    LEFT JOIN
                        ms_item_extension ex2
                    ON  item2.structure_item_id = ex2.item_id
                    AND ex2.sequence_no = 1
                WHERE
                    item2.structure_group_id = 1770
                AND item2.delete_flg = 0
                AND item2.language_id = @LanguageId
                AND EXISTS(
                 SELECT * 
                 FROM #temp_location temp
                 WHERE item2.factory_id = temp.structure_id
                )
                AND ex.extension_data = ex2.extension_data FOR XML PATH('')
            )
    ) AS subject_nm -- 勘定科目コード + 勘定科目名
FROM
    v_structure_all item
    LEFT JOIN
        ms_item_extension ex
    ON  item.structure_item_id = ex.item_id
    AND ex.sequence_no = 1
WHERE
item.structure_group_id = 1770
GROUP BY
    ex.extension_data

) tbl

WHERE subject_nm <> ''

ORDER BY item_extension_data_1
