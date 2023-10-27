/*
 * 予備品のオートコンプリート
 * 予備品No、予備品名、メーカー、材質、型式を取得
 */
 -- A0009
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
main AS ( 
    SELECT
        pp.factory_id AS factoryId              -- 工場ID
        , vs.location_structure_id AS translationFactoryId -- 翻訳工場ID
        , pp.parts_no
        , COALESCE(pp.parts_name, '') + ' ' + COALESCE(vs.translation_text, '') + ' ' + COALESCE(pp.materials, '')
         + ' ' + COALESCE(pp.model_type, '') AS name --予備品名、メーカー、材質、型式
        , pp.parts_name                         --予備品名
        , vs.translation_text AS manufacturer   --メーカー
        , pp.materials                          --材質
        , pp.model_type                         --型式
        , pp.unit_structure_id                  --数量管理単位
        
        /*IF factoryIdList != null && factoryIdList.Count > 0*/
        -- 表示順用工場ID
        , COALESCE(ft.factory_id, 0) AS orderFactoryId
        -- 行番号、表示行数を制限するのでソート順を指定(表示用工場ID毎)
        , row_number() over(partition BY COALESCE(ft.factory_id, 0) ORDER BY COALESCE(COALESCE(order_factory.display_order, order_common.display_order), 32768), vs.structure_id) row_num
        /*END*/
        /*IF factoryIdList == null || factoryIdList.Count == 0*/
        , row_number() over(ORDER BY COALESCE(order_common.display_order,32768), vs.structure_id) row_num
        /*END*/
    FROM
        pt_parts pp 
        LEFT JOIN
        /*IF !getNameFlg */
        v_structure_item
        /*END*/
        /*IF getNameFlg */
        v_structure_item_all
        /*END*/
         AS vs
            ON pp.manufacturer_structure_id = vs.structure_id 
            AND vs.structure_group_id = 1150 
            AND vs.language_id = /*languageId*/'ja' 
        LEFT JOIN ms_structure ms
            ON pp.factory_id = ms.structure_id
            AND ms.structure_group_id = 1000
        /*IF factoryIdList != null && factoryIdList.Count > 0*/
        -- 工場ごとに工場別表示順を取得する
        CROSS JOIN factory AS ft
        LEFT OUTER JOIN ms_structure_order AS order_factory
        ON  vs.structure_id = order_factory.structure_id
        AND order_factory.factory_id = ft.factory_id
        /*END*/
        -- 全工場共通の表示順
        LEFT OUTER JOIN ms_structure_order AS order_common
        ON  vs.structure_id = order_common.structure_id
        AND order_common.factory_id = 0
            
    WHERE
        1 = 1 
    /*IF factoryIdList != null && factoryIdList.Count > 0*/
        AND vs.factory_id IN /*factoryIdList*/(0) 
    /*END*/
    /*IF param1 != null && param1 != ''*/
        AND ms.parent_structure_id = /*param1*/'1' 
    /*END*/
    /*IF param2 != null && param2 != ''*/
        AND pp.parts_no LIKE /*param2*/'%' 
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
            AND unused.structure_id = vs.structure_id
        )
   /*END*/
) 
SELECT
    0 AS factoryId --共通側の絞り込みで除外されないよう0を設定
    , translationFactoryId
    , parts_no AS 'values'
    , name AS 'labels'
    , parts_name AS exparam1
    , manufacturer AS exparam2
    , materials AS exparam3
    , model_type AS exparam4
    , unit_structure_id AS exparam5 
    /*IF factoryIdList != null && factoryIdList.Count > 0*/
    , orderFactoryId
    /*END*/
FROM
    main 
WHERE
/*IF rowLimit != null && rowLimit != ''*/
    row_num < /*rowLimit*/30 
/*END*/
ORDER BY
    parts_no
