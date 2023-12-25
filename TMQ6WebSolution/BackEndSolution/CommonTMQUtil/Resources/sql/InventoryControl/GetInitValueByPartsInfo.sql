-- 予備品IDより、予備品情報に登録されている部門ID、部門コード、勘定科目ID、勘定科目コード、新旧区分IDを取得
WITH old_new_division_list AS ( 
    -- 新旧区分の拡張項目と構成ID
    SELECT
        ex.extension_data
        , ms.structure_id 
        , ms.factory_id 
    FROM
        ms_structure ms 
        LEFT JOIN ms_item_extension ex 
            ON ms.structure_item_id = ex.item_id 
            AND ex.sequence_no = 1 
    WHERE
        ms.structure_group_id = 1940 
        AND ms.delete_flg = 0
) 
SELECT
    parts.department_structure_id                   -- 部門ID
    , dep_ex.extension_data AS department_code      -- 部門コード
    , parts.account_structure_id                    -- 勘定科目ID
    , acc_ex1.extension_data AS account_code        -- 勘定科目コード
    , acc_ex2.extension_data AS old_new_division    -- 勘定科目コード
    , ( 
        SELECT TOP 1
            structure_id 
        FROM
            old_new_division_list 
        WHERE
            extension_data = acc_ex2.extension_data
            AND factory_id in (0, parts.factory_id)
        ORDER BY
            factory_id
    ) AS old_new_structure_id                  -- 新旧区分
FROM
    pt_parts parts                              -- 予備品情報
    LEFT JOIN ms_structure dep_ms               -- 部門(構成マスタ)
        ON parts.department_structure_id = dep_ms.structure_id 
    LEFT JOIN ms_item_extension dep_ex          -- 部門(拡張項目1)
        ON dep_ms.structure_item_id = dep_ex.item_id 
        AND dep_ex.sequence_no = 1 
    LEFT JOIN ms_structure acc_ms               -- 勘定科目(構成マスタ)
        ON parts.account_structure_id = acc_ms.structure_id 
    LEFT JOIN ms_item_extension acc_ex1         -- 勘定科目(拡張項目1)
        ON acc_ms.structure_item_id = acc_ex1.item_id 
        AND acc_ex1.sequence_no = 1 
    LEFT JOIN ms_item_extension acc_ex2         -- 勘定科目(拡張項目2)
        ON acc_ms.structure_item_id = acc_ex2.item_id 
        AND acc_ex2.sequence_no = 2 
WHERE
    parts.parts_id = @PartsId
