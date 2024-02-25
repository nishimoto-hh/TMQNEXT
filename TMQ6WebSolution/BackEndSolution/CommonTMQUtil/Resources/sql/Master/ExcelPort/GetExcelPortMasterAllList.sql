--GetExcelPortMasterList.sqlと同じSQLだが、削除アイテムを含む点が異なる
WITH target AS(
    SELECT
        ms.structure_id,
        ms.structure_group_id,
        ms.factory_id,
        mi.item_id,
        mi.item_translation_id AS translation_id,
        mso.display_order,
        ex1.extension_data AS ex_data1,-- 拡張項目1
        ex2.extension_data AS ex_data2,-- 拡張項目2
        ex3.extension_data AS ex_data3,-- 拡張項目3
        ex4.extension_data AS ex_data4,-- 拡張項目4
        ex5.extension_data AS ex_data5,-- 拡張項目5
        ex6.extension_data AS ex_data6,-- 拡張項目6
        ex7.extension_data AS ex_data7,-- 拡張項目7
        ex8.extension_data AS ex_data8,-- 拡張項目8
        ex9.extension_data AS ex_data9,-- 拡張項目9
        ex10.extension_data AS ex_data10,-- 拡張項目10
        ---------------------------------- 以下は翻訳を取得 ----------------------------------
        (
            SELECT
                tra.translation_text
            FROM
                v_structure_item_all AS tra
            WHERE
                tra.language_id = @LanguageId
            AND tra.location_structure_id = (
                    SELECT
                        MIN(st_f.factory_id)
                    FROM
                        #temp_structure_factory AS st_f
                    WHERE
                        st_f.structure_id = ms.structure_id
                    AND st_f.factory_id IN(0, ms.factory_id)
                )
            AND tra.structure_id = ms.structure_id
        ) AS translation_text, -- アイテム翻訳
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
                        st_f.structure_id = ms.factory_id
                    AND st_f.factory_id IN(0, ms.factory_id)
                )
            AND tra.structure_id = ms.factory_id
        ) AS factory_name -- 工場名
    FROM
        ms_structure ms
        LEFT JOIN
            ms_item mi
        ON  ms.structure_item_id = mi.item_id
        LEFT JOIN
            ms_item_extension ex1 -- 拡張項目1
        ON  ms.structure_item_id = ex1.item_id
        AND ex1.sequence_no = 1
        LEFT JOIN
            ms_item_extension ex2 -- 拡張項目2
        ON  ms.structure_item_id = ex2.item_id
        AND ex2.sequence_no = 2
        LEFT JOIN
            ms_item_extension ex3 -- 拡張項目3
        ON  ms.structure_item_id = ex3.item_id
        AND ex3.sequence_no = 3
        LEFT JOIN
            ms_item_extension ex4 -- 拡張項目4
        ON  ms.structure_item_id = ex4.item_id
        AND ex4.sequence_no = 4
        LEFT JOIN
            ms_item_extension ex5 -- 拡張項目5
        ON  ms.structure_item_id = ex5.item_id
        AND ex5.sequence_no = 5
        LEFT JOIN
            ms_item_extension ex6 -- 拡張項目6
        ON  ms.structure_item_id = ex6.item_id
        AND ex6.sequence_no = 6
        LEFT JOIN
            ms_item_extension ex7 -- 拡張項目7
        ON  ms.structure_item_id = ex7.item_id
        AND ex7.sequence_no = 7
        LEFT JOIN
            ms_item_extension ex8 -- 拡張項目8
        ON  ms.structure_item_id = ex8.item_id
        AND ex8.sequence_no = 8
        LEFT JOIN
            ms_item_extension ex9 -- 拡張項目9
        ON  ms.structure_item_id = ex9.item_id
        AND ex9.sequence_no = 9
        LEFT JOIN
            ms_item_extension ex10 -- 拡張項目10
        ON  ms.structure_item_id = ex10.item_id
        AND ex10.sequence_no = 10
        -- 並び順
        LEFT JOIN
        (
          SELECT
              mso.structure_id,
              mso.factory_id,
              mso.display_order
          FROM
              ms_structure_order mso
        ) mso
        ON ms.structure_id = mso.structure_id
        AND ms.factory_id = mso.factory_id
    WHERE
        ms.structure_group_id = @StructureGroupId
)
SELECT
    target.structure_id,                                -- 構成ID
    target.structure_group_id,                          -- 構成グループID
    target.factory_id,                                  -- 工場ID
    target.item_id,                                     -- アイテムID
    target.factory_id AS factory_id_before,             -- 初期表示時の工場ID(入力チェック時に使用)
    target.factory_name,                                -- 工場名
    target.translation_id,                              -- アイテム翻訳ID
    target.translation_text,                            -- アイテム翻訳
    target.translation_text AS translation_text_before, -- 初期表示時のアイテム翻訳(入力チェックに使用)
    target.ex_data1,                                    -- 拡張項目1
    target.ex_data2,                                    -- 拡張項目2
    target.ex_data3,                                    -- 拡張項目3
    target.ex_data4,                                    -- 拡張項目4
    target.ex_data5,                                    -- 拡張項目5
    target.ex_data6,                                    -- 拡張項目6
    target.ex_data7,                                    -- 拡張項目7
    target.ex_data8,                                    -- 拡張項目8
    target.ex_data9,                                    -- 拡張項目9
    target.ex_data10,                                   -- 拡張項目10
    target.display_order
FROM
    target
WHERE EXISTS(SELECT * FROM #temp_structure_selected temp WHERE temp.structure_group_id = 1000 AND target.factory_id = temp.structure_id)
OR target.factory_id = 0

ORDER BY
 target.factory_id,
 target.display_order,
 target.structure_id
