WITH unuse_factory_id AS -- 未使用アイテムの工場ID
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
                        item.translation_text + ','
                    FROM
                        ms_structure_unused unuse_b
                        LEFT JOIN
                            (
                                SELECT
                                    item.factory_id,
                                    item.translation_text
                                FROM
                                    v_structure_item item
                                WHERE
                                    item.structure_group_id = 1000
                                AND item.structure_layer_no = 1
                                AND item.language_id = @LanguageId
                            ) item
                        ON  unuse_b.factory_id = item.factory_id
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
SELECT DISTINCT
    unuse.structure_id,      -- 構成ID
    unuse.structure_group_id,-- 構成グループID
    master_name.master_name, -- マスタ種類
    uin.unuse_item_name,     -- 未使用アイテム名
    ufi.unuse_factory_id,    -- 未使用工場ID
    ufn.unuse_factory_name   -- 未使用工場名
FROM
    ms_structure_unused unuse
    LEFT JOIN
        unuse_factory_id ufi
    ON  unuse.structure_id = ufi.structure_id
    LEFT JOIN
        unuse_factory_name ufn
    ON  unuse.structure_id = ufn.structure_id
    LEFT JOIN
        master_name
    ON  1 = 1 -- 結合条件はない
    LEFT JOIN
        (
            SELECT
                item.structure_id,
                item.translation_text AS unuse_item_name
            FROM
                v_structure_item item
            WHERE
                item.factory_id = 0
            AND item.location_structure_id = 0
            AND item.language_id = @LanguageId
        ) uin
    ON  unuse.structure_id = uin.structure_id
WHERE
    unuse.structure_group_id = @StructureGroupId