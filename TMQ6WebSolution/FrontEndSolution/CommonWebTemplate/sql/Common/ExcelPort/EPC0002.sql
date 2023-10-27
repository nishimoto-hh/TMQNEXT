/*
 * ExcelPort 予備品-管理工場用 データリストSQL
 */
-- EPC0002
WITH factory AS( -- 構成マスタより工場を取得
    SELECT
        item.structure_id AS id,
        item.parent_structure_id AS parent_id,
        item.translation_text AS name,
        factory_id,
        structure_id,
        dbo.get_target_layer_id(factory_id, 0) AS districtId
    FROM
        v_structure_item item
    LEFT JOIN
        ms_item_extension ex4 -- 変更履歴対象
    ON item.structure_item_id = ex4.item_id
    AND ex4.sequence_no = 4
    LEFT JOIN
        ms_item_extension ex5 -- EcelPort利用可能
    ON item.structure_item_id = ex5.item_id
    AND ex5.sequence_no = 5
    WHERE
        structure_group_id = 1000
    AND language_id = /*languageId*/'ja'
    AND structure_layer_no = 1
    AND ex4.extension_data IS NULL -- 変更履歴対象外
    AND ex5.extension_data = '1' -- ExcelPort使用権限あり
)
,auth AS( -- ユーザ権限を特定するために拡張項目の値を取得
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
,user_narrow AS( -- ユーザマスタをログインユーザで絞込
    SELECT
        *
    FROM
        ms_user
    WHERE
        user_id = /*userId*/1001
)
,user_factory AS( -- ユーザ所属マスタの工場を取得
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
        INNER JOIN
            factory AS st
        ON  (
                -- 予備品の場合、自身の本務工場の所属する地区の工場をすべて表示する
                dbo.get_target_layer_id(ub.location_structure_id, 0) = st.districtId
            )
    WHERE
        st.districtId = (
            -- ユーザーの本務工場の地区と紐付くもの
            SELECT
                dbo.get_target_layer_id(mub.location_structure_id, 0)
            FROM
                ms_user_belong mub
            WHERE
                mub.user_id = /*userId*/1001
            AND mub.duty_flg = 1
        )
)
,user_auth AS( -- ユーザ権限を取得、管理者なら「all_flg」が1
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
,temp AS( -- 表示する工場の一覧を取得 システム管理者の場合とそうでない場合をそれぞれ取得しUNION(実際はどちらかしか取得できない)
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
    t.id,
    t.parent_id,
    t.name,
    t.factory_id,
    coalesce(order_common.display_order, order_factory.display_order, t.structure_id) AS display_order
FROM
    temp AS t
    -- 工場共通表示順
    LEFT OUTER JOIN
        ms_structure_order AS order_common
    ON  t.structure_id = order_common.structure_id
    AND order_common.factory_id = 0
    -- 工場別表示順
    LEFT OUTER JOIN
        ms_structure_order AS order_factory
    ON  t.structure_id = order_factory.structure_id
    AND t.factory_id = order_factory.factory_id
-- 表示順は親構成ID、工場共通表示順、工場別表示順、構成IDの順
ORDER BY
    t.parent_id,
    coalesce(order_common.display_order, order_factory.display_order, t.structure_id)