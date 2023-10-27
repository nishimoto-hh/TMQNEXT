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

INSERT INTO #get_factory
--故障情報個別工場フラグ
SELECT
    ms.structure_id
    ,mie.extension_data
FROM
    ms_structure ms
    LEFT JOIN
        ms_item_extension mie
    ON  ms.structure_item_id = mie.item_id
WHERE
    --場所階層
    ms.structure_group_id = 1000
AND
    --工場
    ms.structure_layer_no = 1
AND
    --故障情報個別工場フラグ
    mie.sequence_no = 2;