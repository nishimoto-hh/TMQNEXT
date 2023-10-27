DELETE
FROM
    ms_structure_unused
WHERE
    structure_group_id = @StructureGroupId

-- エクセルポートの使用権限がある工場かつ、変更管理対象外の工場が削除対象
AND factory_id IN(
        SELECT
            ms.factory_id
        FROM
            ms_structure ms
            -- エクセルポート使用権限
            LEFT JOIN
                ms_item_extension ex
            ON  ms.structure_item_id = ex.item_id
            AND ex.sequence_no = 5
            -- 変更管理対象外
            LEFT JOIN
                ms_item_extension ex2
            ON  ms.structure_item_id = ex2.item_id
            AND ex2.sequence_no = 4
        WHERE
            ms.structure_group_id = 1000
        AND ms.structure_layer_no = 1
        -- エクセルポート使用権限
        AND ex.extension_data = '1'

        -- 変更管理対象外
        AND ex2.extension_data IS NULL
    )