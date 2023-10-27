/*
 * 工場ごとの丸め処理区分を取得
 */

SELECT
    ms.structure_id AS 'values',
    coalesce(round_division.extension_data, '1') AS labels
FROM
    ms_structure ms
    LEFT JOIN
        (
            SELECT
                ms.factory_id,
                ex.extension_data
            FROM
                (
                    SELECT
                        ms.factory_id,
                        MAX(ms.structure_id) AS structure_id
                    FROM
                        ms_structure ms
                    WHERE
                        ms.structure_group_id = 2050
                    GROUP BY
                        ms.factory_id
                ) ms
                LEFT JOIN
                    (
                        SELECT
                            ms.structure_id,
                            ex.extension_data
                        FROM
                            ms_structure ms
                            LEFT JOIN
                                ms_item_extension ex
                            ON  ms.structure_item_id = ex.item_id
                            AND ex.sequence_no = 1
                        WHERE
                            ms.structure_group_id = 2050
                    ) ex
                ON  ms.structure_id = ex.structure_id
        ) round_division
    ON  ms.structure_id = round_division.factory_id
WHERE
    ms.structure_group_id = 1000
AND ms.structure_layer_no = 1