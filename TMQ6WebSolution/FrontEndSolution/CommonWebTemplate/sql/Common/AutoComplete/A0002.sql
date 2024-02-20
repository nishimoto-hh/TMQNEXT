/*
* オートコンプリート
* 構成マスタより名称に合致する値を候補に表示して選択させる
* 構成グループIDをパラメータとして定義で指定する
*/
-- A0002
WITH
-- 工場IDで絞込を行う場合、指定された工場IDの表示順を取得する
-- 工場ID一覧を生成するための構成マスタより取得
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
main AS(
    SELECT
        -- 工場ID
        st.factory_id as factoryId,
        -- 翻訳工場ID
        st.location_structure_id as translationFactoryId,
        -- コード
        st.structure_id AS id,
        -- 名称
        st.translation_text AS name,
        
        /*IF factoryIdList != null && factoryIdList.Count > 0*/
        -- JavaScript側で画面の工場IDにより絞り込みを行うので、絞込用の工場IDと工場ID毎に表示順を取得する
        -- 表示順用工場ID
        coalesce(ft.factory_id, 0) AS orderFactoryId,
        -- 行番号、表示行数を制限するのでソート順を指定(表示用工場ID毎)
        row_number() over(partition BY coalesce(ft.factory_id, 0) ORDER BY coalesce(coalesce(order_factory.display_order, order_common.display_order), 32768), st.structure_id) row_num
        /*END*/
        /*IF factoryIdList == null || factoryIdList.Count == 0*/
        -- 工場IDの指定が無い場合、表示順は全工場共通(0)より取得
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
        st.structure_group_id = /*param1*/0
    AND st.language_id = /*languageId*/'ja'
    /*IF factoryIdList != null && factoryIdList.Count > 0*/
    AND st.factory_id IN /*factoryIdList*/(0)
    -- 共通工場のレコードまたは絞込用工場IDと表示順用工場IDが一致するもののみ抽出
    AND ((st.factory_id = 0 and st.location_structure_id IN (coalesce(ft.factory_id, 0), 0)) OR coalesce(ft.factory_id, 0) IN (st.factory_id, 0))
    /*END*/
        /*IF param2 != null && param2 != ''*/
	        /*IF !getNameFlg */
	            -- 名称で検索
	            AND st.translation_text LIKE '%'+/*param2*/'%'
	        /*END*/
        /*END*/

        /*IF getNameFlg */
            -- 翻訳なのでID
            AND CONVERT(nvarchar, st.structure_id) = CONVERT(nvarchar, /*param2*/0)
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
    name AS 'labels'
    /*IF factoryIdList != null && factoryIdList.Count > 0*/
    ,orderFactoryId
    /*END*/
FROM
    main
WHERE
/*IF rowLimit != null && rowLimit != ''*/
   row_num < /*rowLimit*/30
/*END*/
