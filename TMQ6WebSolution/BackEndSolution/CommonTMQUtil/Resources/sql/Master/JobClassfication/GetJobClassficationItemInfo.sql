SELECT
    st.structure_id                             -- 構成ID
    , st.structure_group_id                     -- 構成グループID
    , dbo.get_translation_text_all( 
        st.factory_id
        , 0
        , 1000
        , @LanguageId
    ) AS factory_name                           -- 工場名称
    , CASE 
        WHEN @StructureLayerNo <= 0 
            THEN NULL 
        ELSE dbo.get_translation_text_all( 
            dbo.get_target_layer_id(@ParentStructureId, 0)
            , @FactoryId
            , 1010
            , @LanguageId
        ) 
        END AS job_name                         -- 職種名称
    , CASE 
        WHEN @StructureLayerNo <= 1 
            THEN NULL 
        ELSE dbo.get_translation_text_all( 
            dbo.get_target_layer_id(@ParentStructureId, 1)
            , @FactoryId
            , 1010
            , @LanguageId
        ) 
        END AS large_classfication_name         -- 機種大分類名称
    , CASE 
        WHEN @StructureLayerNo <= 2 
            THEN NULL 
        ELSE dbo.get_translation_text_all( 
            dbo.get_target_layer_id(@ParentStructureId, 2)
            , @FactoryId
            , 1010
            , @LanguageId
        ) 
        END AS middle_classfication_name        -- 機種中分類名称
    , st.factory_id                             -- 工場ID
    , st.structure_item_id AS item_id           -- アイテムID
    , it.item_translation_id AS translation_id  -- アイテム翻訳ID
    , dbo.get_translation_text_all( 
        st.structure_id
        , @FactoryId
        , @StructureGroupId
        , @LanguageId
    ) AS translation_text                       -- アイテム翻訳名称
    , it_ex1.extension_data AS ex_data1         -- 拡張データ1
    , it_ex2.extension_data AS ex_data2         -- 拡張データ2
    , it_ex3.extension_data AS ex_data3         -- 拡張データ3
    , it_ex4.extension_data AS ex_data4         -- 拡張データ4
    , it_ex5.extension_data AS ex_data5         -- 拡張データ5
    , it_ex6.extension_data AS ex_data6         -- 拡張データ6
    , it_ex7.extension_data AS ex_data7         -- 拡張データ7
    , it_ex8.extension_data AS ex_data8         -- 拡張データ8
    , it_ex9.extension_data AS ex_data9         -- 拡張データ9
    , it_ex10.extension_data AS ex_data10       -- 拡張データ10
    , ord.display_order                         -- 表示順
    , st.delete_flg                             -- 削除フラグ
    , st.update_serialid                        -- 更新シリアルID(構成ID)
    , it_ex1.update_serialid AS update_serialid_ex1         -- 更新シリアルID(拡張データ1)
    , it_ex2.update_serialid AS update_serialid_ex2         -- 更新シリアルID(拡張データ2)
    , it_ex3.update_serialid AS update_serialid_ex3         -- 更新シリアルID(拡張データ3)
    , it_ex4.update_serialid AS update_serialid_ex4         -- 更新シリアルID(拡張データ4)
    , it_ex5.update_serialid AS update_serialid_ex5         -- 更新シリアルID(拡張データ5)
    , it_ex6.update_serialid AS update_serialid_ex6         -- 更新シリアルID(拡張データ6)
    , it_ex7.update_serialid AS update_serialid_ex7         -- 更新シリアルID(拡張データ7)
    , it_ex8.update_serialid AS update_serialid_ex8         -- 更新シリアルID(拡張データ8)
    , it_ex9.update_serialid AS update_serialid_ex9         -- 更新シリアルID(拡張データ9)
    , it_ex10.update_serialid AS update_serialid_ex10       -- 更新シリアルID(拡張データ10)
FROM
    ms_structure AS st 
    LEFT JOIN ms_item AS it 
        ON st.structure_item_id = it.item_id
    -- 拡張データ1
    LEFT JOIN ( 
        SELECT
            item_id
            , extension_data
            , update_serialid
        FROM
            ms_item_extension 
        WHERE
            sequence_no = 1
    ) AS it_ex1 
        ON st.structure_item_id = it_ex1.item_id
    -- 拡張データ2
    LEFT JOIN ( 
        SELECT
            item_id
            , extension_data
            , update_serialid
        FROM
            ms_item_extension 
        WHERE
            sequence_no = 2
    ) AS it_ex2 
        ON st.structure_item_id = it_ex2.item_id
    -- 拡張データ3
    LEFT JOIN ( 
        SELECT
            item_id
            , extension_data
            , update_serialid
        FROM
            ms_item_extension 
        WHERE
            sequence_no = 3
    ) AS it_ex3 
        ON st.structure_item_id = it_ex3.item_id
    -- 拡張データ4
    LEFT JOIN ( 
        SELECT
            item_id
            , extension_data
            , update_serialid
        FROM
            ms_item_extension 
        WHERE
            sequence_no = 4
    ) AS it_ex4 
        ON st.structure_item_id = it_ex4.item_id
    -- 拡張データ5
    LEFT JOIN ( 
        SELECT
            item_id
            , extension_data
            , update_serialid
        FROM
            ms_item_extension 
        WHERE
            sequence_no = 5
    ) AS it_ex5 
        ON st.structure_item_id = it_ex5.item_id
    -- 拡張データ6
    LEFT JOIN ( 
        SELECT
            item_id
            , extension_data
            , update_serialid
        FROM
            ms_item_extension 
        WHERE
            sequence_no = 6
    ) AS it_ex6 
        ON st.structure_item_id = it_ex6.item_id
    -- 拡張データ7
    LEFT JOIN ( 
        SELECT
            item_id
            , extension_data
            , update_serialid
        FROM
            ms_item_extension 
        WHERE
            sequence_no = 7
    ) AS it_ex7 
        ON st.structure_item_id = it_ex7.item_id
    -- 拡張データ8
    LEFT JOIN ( 
        SELECT
            item_id
            , extension_data
            , update_serialid
        FROM
            ms_item_extension 
        WHERE
            sequence_no = 8
    ) AS it_ex8 
        ON st.structure_item_id = it_ex8.item_id
    -- 拡張データ9
    LEFT JOIN ( 
        SELECT
            item_id
            , extension_data
            , update_serialid
        FROM
            ms_item_extension 
        WHERE
            sequence_no = 9
    ) AS it_ex9 
        ON st.structure_item_id = it_ex9.item_id
    -- 拡張データ10
    LEFT JOIN ( 
        SELECT
            item_id
            , extension_data
            , update_serialid
        FROM
            ms_item_extension 
        WHERE
            sequence_no = 10
    ) AS it_ex10 
        ON st.structure_item_id = it_ex10.item_id
    -- 表示順
    LEFT JOIN ms_structure_order AS ord
        ON st.structure_id = ord.structure_id
        AND ord.factory_id = @FactoryId
WHERE
    st.structure_id = @StructureId
