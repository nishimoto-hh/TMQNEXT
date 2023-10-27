--******************************************************************
--予備品　部門の初期表示値を取得する
--オートコンプリート(A0007)の先頭データを取得
--******************************************************************
-- A0007
WITH
factory AS(
    SELECT
        st.structure_id AS factory_id
    FROM
        ms_structure AS st
    WHERE
        st.structure_id IN (
            SELECT
                * 
            FROM
                dbo.get_splitText(@FactoryIdList, default, default)
        )
    UNION
    -- 共通工場は構成マスタに無いので追加
    SELECT
         0
)
,
main AS(
    SELECT
        -- 工場ID
        st.factory_id AS factoryId,
        -- 翻訳工場ID
        st.location_structure_id AS translationFactoryId,
        -- コード
        ex.extension_data AS id,
        -- 名称
        st.translation_text AS name,
        -- 構成ID
        st.structure_id AS structureId,
        -- 拡張データ
        ex2.extension_data,
        -- 表示順用工場ID
        coalesce(ft.factory_id, 0) AS orderFactoryId,
        -- 行番号、表示行数を制限するのでソート順を指定(表示用工場ID毎)
        row_number() over(partition BY coalesce(ft.factory_id, 0) ORDER BY coalesce(coalesce(order_factory.display_order, order_common.display_order), 32768), st.structure_id) row_num
    FROM
        v_structure_item AS st
        INNER JOIN ms_item_extension AS ex
        ON ex.item_id = st.structure_item_id
        AND ex.sequence_no = 1
        LEFT JOIN ms_item_extension AS ex2
        ON ex2.item_id = st.structure_item_id
        AND ex2.sequence_no = 2
        -- 工場ごとに工場別表示順を取得する
        CROSS JOIN factory AS ft
        LEFT OUTER JOIN ms_structure_order AS order_factory
        ON  st.structure_id = order_factory.structure_id
        AND order_factory.factory_id = ft.factory_id
        -- 全工場共通の表示順
        LEFT OUTER JOIN ms_structure_order AS order_common
        ON  st.structure_id = order_common.structure_id
        AND order_common.factory_id = 0
    WHERE
        st.structure_group_id = 1760
    AND st.language_id = @LanguageId
    AND st.factory_id IN (
            SELECT
                * 
            FROM
                dbo.get_splitText(@FactoryIdList, default, default)
        )
        -- 工場別未使用標準アイテムに工場が含まれていないものを表示
        AND
            NOT EXISTS(
                 SELECT
                    *
                FROM
                    ms_structure_unused AS unused
                WHERE
                    unused.factory_id = ft.factory_id
                AND unused.structure_id = st.structure_id
            )
)

SELECT TOP 1
    id AS department_cd,
    structureId AS department_structure_id
FROM
    main
order by row_num,orderFactoryId desc