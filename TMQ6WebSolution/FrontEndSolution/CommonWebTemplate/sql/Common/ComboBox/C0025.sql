/*
* 予備品　予備品倉庫の一覧を取得するSQL
* ユーザが特権(30)、システム管理者(99)の場合は全工場、そうでなければユーザ所属マスタの本務工場の地区に紐づく工場を予備品倉庫として表示(factoryIdList未使用)
*/
-- C0025
-- ユーザマスタをログインユーザで絞込
WITH user_narrow AS ( 
    SELECT
        *
    FROM
        ms_user 
    WHERE
        user_id = /*userId*/1001
)
-- ユーザ所属マスタの本務工場の地区を取得
, user_district AS ( 
    SELECT
        dbo.get_target_layer_id(ub.location_structure_id , 0) AS district_id
    FROM
        user_narrow AS us 
        INNER JOIN ms_user_belong AS ub 
            ON (us.user_id = ub.user_id) 
    WHERE
        ub.duty_flg = 1
)          
-- 構成マスタより工場または職種を取得
, factory AS ( 
    SELECT
        factory_id AS factoryId
        , location_structure_id AS translationFactoryId
        , structure_id AS structure_id
        , translation_text AS translation_text
    FROM
        v_structure_item_all 
        INNER JOIN user_district AS district
            ON parent_structure_id = district.district_id
    WHERE
        structure_group_id = 1000 
        AND language_id = /*languageId*/'ja' 
        AND structure_layer_no = 1 
)                                               
-- ユーザ権限を特定するために拡張項目の値を取得
, auth AS ( 
    SELECT
        st.structure_id
        , ex.extension_data 
    FROM
        v_structure_all AS st 
        INNER JOIN ms_item_extension AS ex 
            ON ( 
                st.structure_item_id = ex.item_id 
                AND ex.sequence_no = 1
            ) 
    WHERE
        st.structure_group_id = 9040
)                                           
-- ユーザ所属マスタの工場を取得
, user_factory AS ( 
    SELECT
        us.user_id
        , st.structure_id AS location_structure_id 
    FROM
        user_narrow AS us 
        INNER JOIN ms_user_belong AS ub 
            ON (us.user_id = ub.user_id) 
        INNER JOIN factory AS st 
            ON (dbo.get_target_layer_id(ub.location_structure_id, 1) = st.structure_id)
)                                                 
-- ユーザ権限を取得、特権か管理者なら「all_flg」が1
, user_auth AS ( 
    SELECT
        us.user_id
        , CASE au.extension_data 
            WHEN '30' THEN 1 
            WHEN '99' THEN 1 
            ELSE 0 
            END AS all_flg 
    FROM
        user_narrow AS us 
        INNER JOIN auth AS au 
            ON (us.authority_level_id = au.structure_id)
)                                               
-- 表示する工場の一覧を取得 特権・システム管理者の場合とそうでない場合をそれぞれ取得しUNION(実際はどちらかしか取得できない)
, temp AS ( 
    -- 特権・システム管理者の場合
    SELECT
        factory_id AS factoryId
        , location_structure_id AS translationFactoryId
        , structure_id AS structure_id
        , translation_text AS translation_text
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id = 1000 
        AND language_id = /*languageId*/'ja' 
        AND structure_layer_no = 1 
        AND EXISTS ( 
            SELECT
                * 
            FROM
                user_auth AS au 
            WHERE
                au.all_flg = 1
        ) 
    UNION
    -- 一般ユーザの場合
    SELECT
        fc.* 
    FROM
        factory AS fc 
    WHERE
        EXISTS ( 
            SELECT
                * 
            FROM
                user_auth AS au 
            WHERE
                au.all_flg = 0
        )
) 
-- 倉庫の一覧を取得
, storage AS ( 
    SELECT
        factory_id
        , location_structure_id
        , structure_id
        , translation_text
        , delete_flg
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id = 1040 
        AND language_id = /*languageId*/'ja' 
        AND structure_layer_no = 2
)

-- コンボの表示用に整形、表示順などを設定
SELECT
    0 AS factoryId --共通側の絞り込みで除外されないよう0を設定
    , s.location_structure_id AS translationFactoryId
    , s.structure_id AS 'values'
    , s.translation_text AS labels 
    , s.factory_id AS exparam1
    , s.delete_flg AS deleteFlg
FROM
    temp t 
    LEFT JOIN storage s 
        ON t.factoryId = s.factory_id 
    -- 全工場共通の表示順
    LEFT OUTER JOIN ms_structure_order AS order_common
    ON  s.structure_id = order_common.structure_id
    AND order_common.factory_id = 0
ORDER BY
    row_number() over(ORDER BY coalesce(order_common.display_order,32768), s.structure_id)