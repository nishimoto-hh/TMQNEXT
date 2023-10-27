WITH max_loc AS -- 構成IDに対する翻訳の工場IDを取得(ユーザの本務工場を優先)
(
    SELECT
        ms.structure_id,
        coalesce(MAX(mt.location_structure_id), 0) AS location_structure_id
    FROM
        ms_structure ms
        LEFT JOIN
            ms_item mi
        ON  ms.structure_item_id = mi.item_id
        LEFT JOIN
            ms_translation mt
        ON  mi.item_translation_id = mt.translation_id
        -- 共通ID(0)とユーザの本務工場IDが対象
    AND mt.location_structure_id IN @FactoryIdList
    GROUP BY
        ms.structure_id
),
factory_name AS -- 工場名
(
    SELECT
        ms.structure_id AS factory_id,
        mt.translation_text AS factory_name
    FROM
        ms_structure ms
        LEFT JOIN
            max_loc
        ON  ms.structure_id = max_loc.structure_id
        LEFT JOIN
            ms_item mi
        ON  ms.structure_item_id = mi.item_id
        LEFT JOIN
            ms_translation mt
        ON  mi.item_translation_id = mt.translation_id
        AND max_loc.location_structure_id = mt.location_structure_id
        AND mt.language_id = @LanguageId
    WHERE
        ms.structure_group_id = 1000
    AND ms.structure_layer_no = 1
),
ex_data AS -- 拡張データ
(
    SELECT
        ex.item_id,
        ex.sequence_no,
        ex.extension_data
    FROM
        ms_item_extension ex
),
master_name AS( --マスタ種類
    SELECT
        msg.structure_group_id,
        mt.translation_text AS master_name,
        msg.display_order
    FROM
        ms_structure_group msg
        LEFT JOIN
            ms_translation mt
        ON  msg.structure_group_translation_id = mt.translation_id
        AND mt.location_structure_id = 0
),
item_order AS( -- 表示順
    SELECT
        mso.structure_id,
        mso.factory_id,
        mso.display_order
    FROM
        ms_structure_order mso
    WHERE
        mso.factory_id IN @FactoryIdList
)
SELECT *
FROM
(
SELECT
    mn.master_name  ,                                                 -- マスタ種類
    CASE
        WHEN factory_name.factory_name IS NULL THEN NULL
        ELSE factory_name.factory_name
    END AS factory_name,                                              -- 工場
    CASE
        WHEN factory_name.factory_name IS NULL THEN '○'
        ELSE NULL
    END AS default_item,                                              -- 標準(=工場なし)
    CASE
	WHEN ms.structure_group_id = 1040 and ms.structure_layer_no = 2			-- 予備品倉庫のとき
	THEN 'AR'+RIGHT('          ' + CONVERT(NVARCHAR, ms.structure_item_id), 10)		-- structure_item_id標記変更　by　AEC
	ELSE　CONVERT(NVARCHAR, ms.structure_item_id)
    END　AS structure_item_id,                                             -- アイテムID
    mt.translation_text,                                              -- アイテム翻訳
    pms.structure_item_id AS parent_structure_item_id,                -- 親階層アイテムID
    pmt.translation_text AS parent_translation_text,                  -- 親階層アイテム翻訳
    CASE
        WHEN factory_name.factory_name IS NULL THEN iod.display_order
        ELSE iof.display_order
    END display_order,                                                -- 表示順
    CASE
        WHEN msu.structure_id IS NULL THEN ''
        ELSE '○'
    END AS unused,                                                    -- 未使用
    CASE
        WHEN ms.delete_flg = 1 THEN '○'
        ELSE ''
    END AS delete_flg,                                                -- 削除フラグ
    ex1.extension_data AS extension_data1,                            -- 拡張項目1
    ex2.extension_data AS extension_data2,                            -- 拡張項目2
    ex3.extension_data AS extension_data3,                            -- 拡張項目3
    ex4.extension_data AS extension_data4,                            -- 拡張項目4
    ex5.extension_data AS extension_data5,                            -- 拡張項目5
    ex6.extension_data AS extension_data6,                            -- 拡張項目6
    ex7.extension_data AS extension_data7,                            -- 拡張項目7
    ex8.extension_data AS extension_data8,                            -- 拡張項目8
    ex9.extension_data AS extension_data9,                            -- 拡張項目9
    ex10.extension_data AS extension_data10,                          -- 拡張項目10
    ms.factory_id,                                                    -- 工場ID(並び替え用)
    mn.display_order as group_display_order,                           -- 構成グループ表示順(並び替え用)
    ms.structure_layer_no                                             -- 追加　by　AEC
FROM
    ms_structure ms
    LEFT JOIN
        max_loc ml
    ON  ms.structure_id = ml.structure_id
    LEFT JOIN
        ms_item mi
    ON  ms.structure_item_id = mi.item_id
    LEFT JOIN
        ms_translation mt
    ON  mi.item_translation_id = mt.translation_id
    AND ml.location_structure_id = mt.location_structure_id
    AND mt.language_id = @LanguageId
    -- マスタ種類名称 --
    LEFT JOIN
        master_name mn
    ON  ms.structure_group_id = mn.structure_group_id

    -- 親階層のアイテムIDと翻訳 --
    LEFT JOIN
        ms_structure pms
    ON  ms.parent_structure_id = pms.structure_id
    LEFT JOIN
        max_loc pml
    ON  pms.structure_id = pml.structure_id
    LEFT JOIN
        ms_item pmi
    ON  pms.structure_item_id = pmi.item_id
    LEFT JOIN
        ms_translation pmt
    ON  pmi.item_translation_id = pmt.translation_id
    AND pml.location_structure_id = pmt.location_structure_id
    AND pmt.language_id = @LanguageId

    -- 拡張項目1～10 --
    LEFT JOIN
        ex_data ex1
    ON  ms.structure_item_id = ex1.item_id
    AND ex1.sequence_no = 1
    LEFT JOIN
        ex_data ex2
    ON  ms.structure_item_id = ex2.item_id
    AND ex2.sequence_no = 2
    LEFT JOIN
        ex_data ex3
    ON  ms.structure_item_id = ex3.item_id
    AND ex3.sequence_no = 3
    LEFT JOIN
        ex_data ex4
    ON  ms.structure_item_id = ex4.item_id
    AND ex4.sequence_no = 4
    LEFT JOIN
        ex_data ex5
    ON  ms.structure_item_id = ex5.item_id
    AND ex5.sequence_no = 5
    LEFT JOIN
        ex_data ex6
    ON  ms.structure_item_id = ex6.item_id
    AND ex6.sequence_no = 6
    LEFT JOIN
        ex_data ex7
    ON  ms.structure_item_id = ex7.item_id
    AND ex7.sequence_no = 7
    LEFT JOIN
        ex_data ex8
    ON  ms.structure_item_id = ex8.item_id
    AND ex8.sequence_no = 8
    LEFT JOIN
        ex_data ex9
    ON  ms.structure_item_id = ex9.item_id
    AND ex9.sequence_no = 9
    LEFT JOIN
        ex_data ex10
    ON  ms.structure_item_id = ex10.item_id
    AND ex10.sequence_no = 10

    -- 工場別未使用標準アイテム --
    LEFT JOIN
        ms_structure_unused msu
    ON  ms.structure_id = msu.structure_id
    AND msu.factory_id = @UserFactoryId

    -- 工場名 --
    LEFT JOIN
        factory_name
    ON  ms.factory_id = factory_name.factory_id

    -- 表示順(標準) --
    LEFT JOIN
        item_order iod
    ON  ms.structure_id = iod.structure_id
    AND iod.factory_id = @UserFactoryId

    -- 表示順(工場) --
    LEFT JOIN
        item_order iof
    ON  ms.structure_id = iof.structure_id
    AND ms.factory_id = iof.factory_id

WHERE
     ms.factory_id IN @AuthFactoryId 
AND ms.structure_group_id IN @StructureGroupId
) tbl
ORDER BY
    group_display_order,
    structure_layer_no,		-- 追加　by　AEC
    factory_id,
    display_order