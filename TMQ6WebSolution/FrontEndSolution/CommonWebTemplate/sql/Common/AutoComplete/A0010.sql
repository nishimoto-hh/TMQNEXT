/*
* 予備品　部門のオートコンプリート
* ユーザが特権(30)、システム管理者(99)の場合は全工場、そうでなければユーザ所属マスタの本務工場の地区に紐づく部門表示
*/
WITH user_narrow AS(
    SELECT
        *
    FROM
        ms_user
    WHERE
        user_id = /*userId*/1001
)
-- ユーザ所属マスタの本務工場の地区を取得
,
user_district AS(
    SELECT
        dbo.get_target_layer_id(ub.location_structure_id, 0) AS district_id
    FROM
        user_narrow AS us
        INNER JOIN
            ms_user_belong AS ub
        ON  (
                us.user_id = ub.user_id
            )
    WHERE
        ub.duty_flg = 1
)
-- 構成マスタより工場または職種を取得
,
factory AS(
    SELECT
        factory_id AS factoryId,
        location_structure_id AS translationFactoryId,
        structure_id AS structure_id,
        translation_text AS translation_text
    FROM
        v_structure_item_all
        INNER JOIN
            user_district AS district
        ON  parent_structure_id = district.district_id
    WHERE
        structure_group_id = 1000
    AND language_id = /*languageId*/'ja' 
    AND structure_layer_no = 1
)
-- ユーザ権限を特定するために拡張項目の値を取得
,
auth AS(
    SELECT
        st.structure_id,
        ex.extension_data
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
-- ユーザ所属マスタの工場を取得
,
user_factory AS(
    SELECT
        us.user_id,
        st.structure_id AS location_structure_id
    FROM
        user_narrow AS us
        INNER JOIN
            ms_user_belong AS ub
        ON  (
                us.user_id = ub.user_id
            )
        AND (
                ub.duty_flg = 1
            )
        INNER JOIN
            factory AS st
        ON  (
                dbo.get_target_layer_id(ub.location_structure_id, 1) = st.structure_id
            )
)
-- ユーザ権限を取得、特権か管理者なら「all_flg」が1
,
user_auth AS(
    SELECT
        us.user_id,
        CASE au.extension_data
            WHEN '30' THEN 1
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
-- 表示する工場の一覧を取得 特権・システム管理者の場合とそうでない場合をそれぞれ取得しUNION(実際はどちらかしか取得できない)
,
temp AS(
    -- 特権・システム管理者の場合
    SELECT
        factoryId
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
    -- 一般ユーザの場合
    SELECT
        fc.factoryId
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
    UNION
    SELECT
        0
),
facatory_max AS(
    SELECT
        item.structure_id,
        MAX(item.location_structure_id) AS location_structure_id
    FROM
        v_structure_item_all item
    WHERE
        item.structure_group_id = 1760
    AND item.location_structure_id IN (
                                       SELECT  item.factory_id
                                       UNION
                                       SELECT 0
                                       UNION
                                       SELECT dbo.get_target_layer_id(mub.location_structure_id, 1) from ms_user_belong mub where mub.duty_flg = 1 AND mub.user_id = /*userId*/1001 )
    GROUP BY
        item.structure_id
)
SELECT
    * 
FROM
    ( 
        SELECT
            *
            , row_number() over (ORDER BY exparam2) row_num 
        FROM
            ( 
                SELECT distinct
                    0 AS factoryId
                    , 0 AS translationFactoryId
                    , ex.extension_data AS 'values'
                    , item.translation_text AS 'labels'
                    , item.structure_id AS exparam1
                    , COALESCE(ex2.extension_data, 0) AS exparam2 
                FROM
                    v_structure_item_all item 
                    INNER JOIN facatory_max fmax 
                        ON item.structure_id = fmax.structure_id 
                        AND item.location_structure_id = fmax.location_structure_id 
                    CROSS JOIN temp 
                    LEFT JOIN ms_item_extension ex 
                        ON item.structure_item_id = ex.item_id 
                        AND ex.sequence_no = 1 
                    LEFT JOIN ms_item_extension AS ex2 
                        ON ex2.item_id = item.structure_item_id 
                        AND ex2.sequence_no = 2 
                WHERE
                    structure_group_id = /*param1*/1760 
                    AND language_id = /*languageId*/'ja' 
                /*IF param2 != null && param2 != ''*/
                    /*IF !getNameFlg */
                    -- コードで検索
                    AND (ex.extension_data LIKE /*param2*/'%') 
                    /*END*/
                /*END*/
                /*IF getNameFlg */
                    -- 翻訳なのでID
                    AND ex.extension_data = /*param2*/'0' 
                /*END*/
                /*IF factoryIdList != null && factoryIdList.Count > 0*/
                    -- 工場別未使用標準アイテムに工場が含まれていないものを表示
                    AND NOT EXISTS ( 
                        SELECT
                            * 
                        FROM
                            ms_structure_unused AS unused 
                        WHERE
                            temp.factoryId in (0, unused.factory_id) 
                            AND unused.structure_id = item.structure_id
                    ) 
                /*END*/
            ) tbl
    ) tbl2 
/*IF rowLimit != null && rowLimit != ''*/
WHERE row_num < /*rowLimit*/30
/*END*/
