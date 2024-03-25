/* オートコンプリート
* 予備品　勘定科目のオートコンプリート
* 選択された新旧区分に応じて勘定科目の選択肢を制約する
* 拡張項目のコードと翻訳名称を表示する
*/
-- A0008
WITH
/*IF factoryIdList != null && factoryIdList.Count > 0*/
factory AS(
     SELECT
        st.structure_id AS factory_id
    FROM
        ms_structure AS st
    WHERE
        st.structure_id IN /*factoryIdList*/(0)
    UNION
    -- 共通工場は構成マスタに無いので追加
    SELECT
         0
)
,
/*END*/
/*IF param2 != null && param2 != ''*/
ex_new as
(
select dbo.get_rep_extension_data( /*param2*/375, /*param3*/5, /*languageId*/'ja', /*param4*/1) as extension_data
),
/*END*/
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
        /*IF factoryIdList != null && factoryIdList.Count > 0*/
        -- 表示順用工場ID
        coalesce(ft.factory_id, 0) AS orderFactoryId,
        -- 行番号、表示行数を制限するのでソート順を指定(表示用工場ID毎)
        row_number() over(partition BY coalesce(ft.factory_id, 0) ORDER BY coalesce(coalesce(order_factory.display_order, order_common.display_order), 32768), st.structure_id) row_num
        /*END*/
        /*IF factoryIdList == null || factoryIdList.Count == 0*/
        row_number() over(ORDER BY coalesce(order_common.display_order,32768), st.structure_id) row_num
        /*END*/
    FROM
        /*IF !getNameFlg */
        v_structure_item
        /*END*/
        /*IF getNameFlg */
        v_structure_item_all
        /*END*/
         AS st
        INNER JOIN ms_item_extension AS ex
        ON ex.item_id = st.structure_item_id
        AND ex.sequence_no = 1
        LEFT JOIN ms_item_extension AS ex2
        ON ex2.item_id = st.structure_item_id
        AND ex2.sequence_no = 2
        /*IF factoryIdList != null && factoryIdList.Count > 0*/
        -- 工場ごとに工場別表示順を取得する
        CROSS JOIN factory AS ft
        LEFT OUTER JOIN ms_structure_order AS order_factory
        ON  st.structure_id = order_factory.structure_id
        AND order_factory.factory_id = ft.factory_id
        /*END*/
        -- 全工場共通の表示順
        LEFT OUTER JOIN ms_structure_order AS order_common
        ON  st.structure_id = order_common.structure_id
        AND order_common.factory_id = 0
    WHERE
        st.structure_group_id = /*param1*/1770
    AND st.language_id = /*languageId*/'ja'
    /*IF param2 != null && param2 != ''*/
    -- 新旧区分を指定(新品・中古品の場合は拡張項目を参照し、循環予備品などの拡張項目がない項目は全て取得対象とする)
        AND ( 
            ex2.extension_data = ( 
                CASE 
                    WHEN (SELECT extension_data FROM ex_new) = '0' 
                    OR (SELECT extension_data FROM ex_new) = '1' 
                        THEN (SELECT extension_data FROM ex_new) 
                    ELSE '0' 
                    END
            ) 
            OR ex2.extension_data = ( 
                CASE 
                    WHEN (SELECT extension_data FROM ex_new) = '0' 
                    OR (SELECT extension_data FROM ex_new) = '1' 
                        THEN (SELECT extension_data FROM ex_new) 
                    ELSE '1' 
                    END
            )
        )
    /*END*/
    /*IF factoryIdList != null && factoryIdList.Count > 0*/
    AND st.factory_id IN /*factoryIdList*/(0)
    -- 共通工場のレコードまたは絞込用工場IDと表示順用工場IDが一致するもののみ抽出
    --AND (st.factory_id = 0 and st.location_structure_id IN (coalesce(ft.factory_id, 0), 0) OR coalesce(ft.factory_id, 0) IN (st.factory_id, 0))
    /*END*/
        /*IF param5 != null && param5 != ''*/
            /*IF !getNameFlg */
                -- コードで検索
                AND (ex.extension_data LIKE /*param5*/'%')
            /*END*/
        /*END*/

        /*IF getNameFlg */
            -- 翻訳なのでID
            AND ex.extension_data =/*param5*/'0'
        /*END*/
        
        /*IF factoryIdList != null && factoryIdList.Count > 0*/
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
       /*END*/
)

SELECT
    factoryId,
    translationFactoryId,
    id AS 'values',
    name AS 'labels',
    structureId AS exparam1,
    COALESCE(extension_data, 0) AS exparam2
    /*IF factoryIdList != null && factoryIdList.Count > 0*/
    ,orderFactoryId
    /*END*/
FROM
    main
WHERE
/*IF rowLimit != null && rowLimit != ''*/
   row_num < /*rowLimit*/30
/*END*/
