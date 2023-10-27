INSERT INTO #follow_flg
--フォロー有無
SELECT
    ms.structure_id
    ,mie.extension_data
FROM
    ms_structure ms
    LEFT JOIN
        ms_item_extension mie
    ON  ms.structure_item_id = mie.item_id
WHERE
    ms.structure_group_id = 2130;
