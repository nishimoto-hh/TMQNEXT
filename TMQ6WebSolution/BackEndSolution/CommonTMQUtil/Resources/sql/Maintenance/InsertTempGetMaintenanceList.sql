INSERT INTO #temp_progress
--進捗状況
SELECT
    ms.structure_id
    ,mie.extension_data
FROM
    ms_structure ms
    LEFT JOIN
        ms_item mi
    ON  ms.structure_item_id = mi.item_id
    LEFT JOIN
        ms_item_extension mie
    ON  mi.item_id = mie.item_id
WHERE
    ms.structure_group_id = 1900
AND mie.sequence_no = 1;
INSERT INTO #get_factory
--故障分析個別工場フラグ
SELECT
    ms.structure_id
    ,mie.extension_data
FROM
    ms_structure ms
    LEFT JOIN
        ms_item mi
    ON  ms.structure_item_id = mi.item_id
    LEFT JOIN
        ms_item_extension mie
    ON  mi.item_id = mie.item_id
WHERE
    --場所階層
    ms.structure_group_id = 1000
AND
    --工場
    ms.structure_layer_no = 1
AND
    --故障分析個別工場フラグ
    mie.sequence_no = 2;