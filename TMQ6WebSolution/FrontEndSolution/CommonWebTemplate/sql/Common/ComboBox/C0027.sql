/*
 * 工場コンボ用SQL.
 * ユーザが特権(30)、システム管理者(99)の場合は全工場、そうでなければユーザ所属マスタの本務工場の地区に紐づく工場を表示(factoryIdList未使用)
 */
-- C0027
WITH user_narrow AS ( 
    SELECT
        *
    FROM
        ms_user 
    WHERE
        user_id = /*userId*/11
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
-- 構成マスタよりユーザ所属マスタの本務工場の地区配下の工場を取得
, factory AS ( 
    SELECT
        factory_id
        , location_structure_id
        , structure_id
        , translation_text
        , delete_flg
    FROM
        v_structure_item_all 
        INNER JOIN user_district AS district
            ON parent_structure_id = district.district_id
    WHERE
        structure_group_id = /*param1*/1000
        AND language_id = /*languageId*/'ja' 
        AND structure_layer_no = /*param2*/1
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
        factory_id
        , location_structure_id
        , structure_id
        , translation_text
        , delete_flg
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id = /*param1*/1000
        AND language_id = /*languageId*/'ja' 
        AND structure_layer_no = /*param2*/1
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
SELECT t.factory_id as factoryId,
       t.location_structure_id as translationFactoryId,
       t.structure_id as 'values',
       t.translation_text as labels,
       t.delete_flg AS deleteFlg
  from temp AS t
       -- 全工場共通の表示順
       LEFT OUTER JOIN ms_structure_order AS order_common
       ON  t.structure_id = order_common.structure_id
       AND order_common.factory_id = 0

order by 
-- 表示順は全工場共通(0)より取得
row_number() over(ORDER BY coalesce(order_common.display_order,32768), t.structure_id)
