--******************************************************************
--予備品　部門の初期表示値を取得する
--オートコンプリート(A0007)の先頭データを取得
--******************************************************************
WITH factory AS ( 
    SELECT
        st.structure_id AS factory_id 
    FROM
        ms_structure AS st 
    WHERE
        st.structure_id IN ( 
            SELECT
                * 
            FROM
                dbo.get_splitText(@FactoryIdList, DEFAULT, DEFAULT) --共通と指定工場
        ) 
    UNION                                       -- 共通工場は構成マスタに無いので追加
    SELECT
        0
) 
, main AS ( 
    SELECT
        st.structure_id
        , ex.extension_data
        , coalesce(ft.factory_id, 0) AS order_factory_id -- 表示順用工場ID
        -- 行番号、表示行数を制限するのでソート順を指定(表示用工場ID毎)
        , row_number() OVER ( 
            PARTITION BY
                coalesce(ft.factory_id, 0) 
            ORDER BY
                coalesce( 
                    coalesce( 
                        order_factory.display_order
                        , order_common.display_order
                    ) 
                    , 32768
                ) 
                , st.structure_id
        ) row_num 
    FROM
        v_structure st 
        INNER JOIN ms_item_extension AS ex 
            ON ex.item_id = st.structure_item_id 
            AND ex.sequence_no = 1 
        CROSS JOIN factory AS ft                -- 工場ごとに工場別表示順を取得する
        LEFT OUTER JOIN ms_structure_order AS order_factory 
            ON st.structure_id = order_factory.structure_id 
            AND order_factory.factory_id = ft.factory_id 
        LEFT OUTER JOIN ms_structure_order AS order_common -- 全工場共通の表示順
            ON st.structure_id = order_common.structure_id 
            AND order_common.factory_id = 0 
    WHERE
        st.structure_group_id = 1760 
        AND st.factory_id IN ( 
            SELECT
                * 
            FROM
                dbo.get_splitText(@FactoryIdList, DEFAULT, DEFAULT)
        ) 
        AND NOT EXISTS ( 
            -- 工場別未使用標準アイテムに工場が含まれていないものを表示
            SELECT
                * 
            FROM
                ms_structure_unused AS unused 
            WHERE
                unused.factory_id = ft.factory_id 
                AND unused.structure_id = st.structure_id
        )
) 
SELECT
    TOP 1 extension_data AS department_cd
    , structure_id AS department_structure_id 
FROM
    main 
ORDER BY
    row_num
    , order_factory_id DESC
