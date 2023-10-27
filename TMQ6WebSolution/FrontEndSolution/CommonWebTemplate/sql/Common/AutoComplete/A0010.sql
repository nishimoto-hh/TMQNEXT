/*
* 予備品　部門のオートコンプリート
* ユーザがシステム管理者の場合は本務工場の地区配下の工場と標準工場のアイテムを表示
* ユーザがシステム管理者以外の場合は本務工場と標準工場のアイテムを表示
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
-- 構成マスタより、取得した地区配下の工場を取得
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
-- ユーザ所属マスタの本務工場を取得
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
-- ユーザ権限を取得、管理者なら「all_flg」が1
,
user_auth AS(
    SELECT
        us.user_id,
        CASE au.extension_data
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
,
temp AS(
    -- システム管理者の場合
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
    -- 特権・一般ユーザの場合
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
main AS(
    SELECT
        st.factory_id AS factoryId,                       -- 工場ID
        st.location_structure_id AS translationFactoryId, -- 翻訳工場ID
        ex.extension_data AS id,                          -- コード
        --st.translation_text AS name,                    -- 名称
        COALESCE((
                SELECT
                    tra.translation_text
                FROM
                    v_structure_item_all AS tra
                WHERE
                    tra.structure_id = st.structure_id
                AND tra.language_id = /*languageId*/'ja'
                AND tra.location_structure_id = st.location_structure_id
            ),(
                SELECT
                    tra.translation_text
                FROM
                    v_structure_item_all AS tra
                WHERE
                    tra.structure_id = st.structure_id
                AND tra.language_id = /*languageId*/'ja'
                AND tra.location_structure_id = 0
            )) AS name,                                   -- 名称
        st.structure_id AS structureId,                   -- 構成ID
        ex2.extension_data,                               -- 拡張データ
        0 AS orderFactoryId,      -- 表示順用工場ID
        row_number() over(partition BY coalesce(ft.factoryId, 0) ORDER BY coalesce(coalesce(order_factory.display_order, order_common.display_order), 32768), st.structure_id) row_num -- 行番号、表示行数を制限するのでソート順を指定(表示用工場ID毎)
    FROM
        /*IF !getNameFlg */
              v_structure_item
        /*END*/
        /*IF getNameFlg */
              v_structure_item_all
        /*END*/
        AS st
        INNER JOIN
            temp
        ON  st.factory_id = temp.factoryId
        INNER JOIN
            ms_item_extension AS ex
        ON  ex.item_id = st.structure_item_id
        AND ex.sequence_no = 1
        LEFT JOIN
            ms_item_extension AS ex2
        ON  ex2.item_id = st.structure_item_id
        AND ex2.sequence_no = 2
        -- 工場ごとに工場別表示順を取得する
        CROSS JOIN
            factory AS ft
        LEFT OUTER JOIN
            ms_structure_order AS order_factory
        ON  st.structure_id = order_factory.structure_id
        AND order_factory.factory_id = /*param2*/0
        -- 全工場共通の表示順
        LEFT OUTER JOIN
            ms_structure_order AS order_common
        ON  st.structure_id = order_common.structure_id
        AND order_common.factory_id = 0
    WHERE
        st.structure_group_id = /*param1*/1760
    AND st.language_id = /*languageId*/'ja'
   /*IF param3 != null && param3 != ''*/
        /*IF !getNameFlg */
        -- コードで検索
        AND (ex.extension_data LIKE '%'+/*param3*/'%')
        /*END*/
   /*END*/
   /*IF getNameFlg */
        -- 翻訳なのでID
        AND ex.extension_data =/*param3*/'0'
   /*END*/
    -- 工場別未使用標準アイテムに工場が含まれていないものを表示
AND NOT EXISTS(
        SELECT
            *
        FROM
            ms_structure_unused AS unused
        WHERE
            unused.factory_id = /*param2*/0
        AND unused.structure_id = st.structure_id
    )
)
SELECT
    0 AS factoryId,
    0 AS translationFactoryId,
    id AS 'values',
    name AS 'labels',
    structureId AS exparam1,
    COALESCE(extension_data, 0) AS exparam2,
    row_num,
    orderFactoryId
FROM
    main
WHERE
/*IF rowLimit != null && rowLimit != ''*/
    row_num < /*rowLimit*/30 
/*END*/