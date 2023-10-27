WITH auth_factory AS -- エクセルポートの使用権限がある工場かつ、変更管理対象外の工場
(
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
),
unuse_factory_id AS -- 未使用アイテムの工場ID
(
    SELECT
        unuse_a.structure_id,
        trim(
            '|'
            FROM
                (
                    SELECT
                        cast(unuse_b.factory_id AS varchar) + '|'
                    FROM
                        ms_structure_unused unuse_b
                    WHERE
                        unuse_b.structure_group_id = @StructureGroupId
                    AND unuse_b.factory_id IN(SELECT factory_id FROM auth_factory)
                    AND unuse_b.structure_id = unuse_a.structure_id FOR XML PATH('')
                )
        ) AS unuse_factory_id
    FROM
        ms_structure_unused unuse_a
    WHERE
        unuse_a.structure_group_id = @StructureGroupId
    GROUP BY
        unuse_a.structure_id
),
unuse_factory_name AS -- 未使用アイテムの工場名
(
    SELECT
        unuse_a.structure_id,
        trim(
            '|,'
            FROM
                (
                    SELECT
                       (
                           SELECT
                               tra.translation_text
                           FROM
                               v_structure_item_all AS tra
                           WHERE
                               tra.language_id =@LanguageId
                           AND tra.location_structure_id = (
                                   SELECT
                                       MAX(st_f.factory_id)
                                   FROM
                                       #temp_structure_factory AS st_f
                                   WHERE
                                       st_f.structure_id = unuse_b.factory_id
                                              )
                                          AND tra.structure_id = unuse_b.factory_id
                        )  + ','
                        FROM
                        ms_structure_unused unuse_b
                        WHERE
                        unuse_b.factory_id IN (SELECT factory_id FROM auth_factory)
                        AND unuse_b.structure_id = unuse_a.structure_id FOR XML PATH('')
                )
        ) AS unuse_factory_name
    FROM
        ms_structure_unused unuse_a
    WHERE
        unuse_a.structure_group_id = @StructureGroupId
    GROUP BY
        unuse_a.structure_id
),
master_name AS( -- マスタ名
    SELECT
        ms.translation_text AS master_name
    FROM
        ms_translation ms
    WHERE
        ms.translation_id = @MasterTransLationId
    AND ms.location_structure_id = 0
    AND ms.language_id = @LanguageId
)

SELECT
    @ProcessId AS process_id,            -- 送信時処理ID
    pn.translation_text as process_name, -- 送信時処理名
    item.structure_id,                   -- 構成ID
    item.structure_group_id,             -- 構成グループID
    master_name.master_name,             -- マスタ種類
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
                        st_f.structure_id = item.structure_id
                    AND st_f.factory_id IN(0, item.factory_id)
                )
            AND tra.structure_id = item.structure_id
        ) AS unuse_item_name, -- 未使用アイテム名
    ufi.unuse_factory_id,     -- 未使用工場ID
    ufn.unuse_factory_name    -- 未使用工場名
FROM
    ms_structure item
    LEFT JOIN
        master_name
    ON  1 = 1 -- 結合条件はない
    LEFT JOIN
        unuse_factory_id ufi
    ON  item.structure_id = ufi.structure_id
    LEFT JOIN
        unuse_factory_name ufn
    ON  item.structure_id = ufn.structure_id
    LEFT JOIN
        (
            SELECT
                item.translation_text
            FROM
                v_structure_item item
                LEFT JOIN
                    ms_item_extension ex
                ON  item.structure_item_id = ex.item_id
                AND ex.sequence_no = 1
            WHERE
                item.structure_group_id = 2120
            AND ex.extension_data = @ProcessId
            AND item.language_id = @LanguageId
        ) pn
    ON  1 = 1 -- 結合条件はない
WHERE
    item.structure_group_id = @StructureGroupId
AND item.factory_id = 0
AND item.delete_flg = 0
ORDER BY
    item.structure_id