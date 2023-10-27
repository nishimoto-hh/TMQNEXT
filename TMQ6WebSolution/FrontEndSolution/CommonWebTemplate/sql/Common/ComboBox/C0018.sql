/*
* ExcelPortダウンロード、工場の一覧を取得するSQL
* システム管理者(99)の場合は全工場、そうでなければユーザ所属マスタの工場
*/
-- C0018
-- 構成マスタより工場または職種を取得
WITH factory AS(
     SELECT
         factory_id AS factoryId
        ,location_structure_id AS translationFactoryId
        ,structure_id AS structure_id
        ,translation_text AS translation_text
        ,delete_flg
        ,dbo.get_target_layer_id(factory_id, 0) AS districtId
    FROM
        v_structure_item_all
    WHERE
        structure_group_id = /*param1*/1000
    AND language_id = /*languageId*/'ja'
    AND structure_layer_no = /*param2*/0

)
-- ユーザ権限を特定するために拡張項目の値を取得
,auth AS(
     SELECT
         st.structure_id
        ,ex.extension_data
    FROM
        v_structure_all AS st
        INNER JOIN
            ms_item_extension AS ex
        ON  (
                st.structure_item_id = ex.item_id
            AND ex.sequence_no = 1
            )
    WHERE
        st.structure_group_id = 9040
)
-- ユーザマスタをログインユーザで絞込
,user_narrow AS(
    SELECT
        *
    FROM
        ms_user
    WHERE
        user_id = /*userId*/0
)
-- ユーザ所属マスタの工場を取得
,user_factory AS(
     SELECT
         us.user_id
        ,st.structure_id AS location_structure_id
    FROM
         user_narrow AS us
        INNER JOIN
            ms_user_belong AS ub
        ON  (
                us.user_id = ub.user_id
            )
        INNER JOIN
            factory AS st
        ON  (
        /*IF param3==1*/
        -- 予備品以外(ExcelPortなど)、自身の所属工場のみを表示する場合
                st.factoryId = dbo.get_target_layer_id(ub.location_structure_id, 1))
            )
        /*END*/
        /*IF param3!=1*/
        -- 予備品の場合、自身の本務工場の所属する地区の工場をすべて表示する場合
                dbo.get_target_layer_id(ub.location_structure_id, 0) = st.districtId
            )
    WHERE
         st.districtId = (-- ユーザーの本務工場の地区と紐付くもの
             SELECT
                 dbo.get_target_layer_id(mub.location_structure_id, 0)
             FROM
                 ms_user_belong mub
             WHERE
                 mub.user_id =/*userId*/0
             AND mub.duty_flg = 1
        )
        /*END*/
)
-- ユーザ権限を取得、管理者なら「all_flg」が1
,user_auth AS(
     SELECT
         us.user_id
        ,CASE au.extension_data
            WHEN '99' THEN 1
            ELSE 0
        END AS all_flg
    FROM
         user_narrow AS us
        INNER JOIN
            auth AS au
        ON  (
                us.authority_level_id = au.structure_id
            )
)
-- 表示する工場の一覧を取得 システム管理者の場合とそうでない場合をそれぞれ取得しUNION(実際はどちらかしか取得できない)
,temp AS(
    -- システム管理者の場合
    SELECT
        *
    FROM
        factory
    WHERE
        EXISTS(
            SELECT
                *
            FROM
                user_auth AS au
            WHERE
                au.all_flg = 1
        )
    UNION
    --  特権・一般ユーザの場合
    SELECT
        fc.*
    FROM
        factory AS fc
        INNER JOIN
            user_factory AS uf
        ON  (
                fc.structure_id = uf.location_structure_id
            )
    WHERE
        EXISTS(
            SELECT
                *
            FROM
                user_auth AS au
            WHERE
                au.all_flg = 0
        )
)

-- コンボの表示用に整形、表示順などを設定
SELECT
     t.factoryId
    ,t.translationFactoryId
    ,t.structure_id AS 'values'
    ,t.translation_text AS labels
    ,t.delete_flg AS deleteFlg

FROM
    temp AS t

    -- 全工場共通の表示順
    LEFT OUTER JOIN ms_structure_order AS order_common
    ON  t.structure_id = order_common.structure_id
    AND order_common.factory_id = 0
ORDER BY

    -- 工場IDの指定が無い場合、表示順は全工場共通(0)より取得
    row_number() over(ORDER BY coalesce(order_common.display_order,32768), t.structure_id)