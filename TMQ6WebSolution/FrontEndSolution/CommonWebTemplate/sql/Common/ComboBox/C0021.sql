/*
* ユーザーマスタ 地区/工場コンボ取得
* ユーザがシステム管理者(99)の場合は全工場、そうでなければユーザ所属マスタの工場
*/
-- 構成マスタより地区/工場を取得
WITH factory AS(
     SELECT
         vi.factory_id AS factoryId
        ,vi.location_structure_id AS translationFactoryId
        ,vi.structure_id AS structure_id
        ,v2.structure_id AS factory_structure_id
        ,coalesce(vi.translation_text,'')  + ' / ' + coalesce(v2.translation_text,'') AS translation_text
        ,vi.delete_flg AS deleteFlg
        -- 工場の表示順
        ,coalesce(order_common.display_order, 32768) AS factory_order
    FROM
        v_structure_item_all vi
    INNER JOIN v_structure_item_all v2
        ON vi.structure_id = v2.parent_structure_id
        AND v2.structure_layer_no = 1 --工場
        AND v2.language_id = /*languageId*/'ja'
    --LEFT JOIN --予備品の共通工場は除く
        --ms_item_extension ie
        --ON v2.structure_item_id = ie.item_id
        --AND ie.sequence_no = 3
    -- 表示順(工場共通)
    LEFT OUTER JOIN
        ms_structure_order AS order_common
    ON  (
            v2.structure_id = order_common.structure_id
        AND order_common.factory_id = 0
        )
    WHERE
        vi.structure_group_id = 1000
    AND vi.language_id = /*languageId*/'ja'
    AND vi.structure_layer_no = 0  --地区
    /*IF factoryIdList != null && factoryIdList.Count > 0*/
    AND vi.factory_id IN /*factoryIdList*/(0)
    /*END*/
    --AND ie.extension_data IS NULL OR ie.extension_data != '1'
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
                dbo.get_target_layer_id(ub.location_structure_id, 1) = st.structure_id
            )
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
    -- 特権・一般ユーザの場合
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
/*IF factoryIdList != null && factoryIdList.Count > 0*/
-- 工場IDで絞込を行う場合、指定された工場IDの表示順を取得する
-- 工場ID一覧を生成するための構成マスタより取得
,factory_list AS(
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
/*END*/
-- コンボの表示用に整形、表示順などを設定
SELECT
     t.factoryId
    ,t.translationFactoryId
    ,t.factory_structure_id AS 'values'
    ,t.translation_text AS labels
    ,t.deleteFlg
    /*IF factoryIdList != null && factoryIdList.Count > 0*/
    -- JavaScript側で画面の工場IDにより絞り込みを行うので、絞込用の工場IDと工場ID毎に表示順を取得する
    -- 表示順用工場ID
    ,coalesce(ft.factory_id, 0) AS orderFactoryId
    /*END*/
FROM
    temp AS t
    /*IF factoryIdList != null && factoryIdList.Count > 0*/
    -- 工場ごとに工場別表示順を取得する
    CROSS JOIN factory_list AS ft
    /*END*/
    -- 全工場共通の表示順
    LEFT OUTER JOIN ms_structure_order AS order_common
    ON  t.structure_id = order_common.structure_id
    AND order_common.factory_id = 0
/*IF factoryIdList != null && factoryIdList.Count > 0*/
-- 工場別未使用標準アイテムに地区or工場が含まれていないものを表示
WHERE
    NOT EXISTS(
         SELECT
            *
        FROM
            ms_structure_unused AS unused
        WHERE
            unused.factory_id = ft.factory_id
        AND unused.structure_id IN(t.structure_id, t.factory_structure_id)
    )
   -- 共通工場のレコードまたは絞込用工場IDと表示順用工場IDが一致するもののみ抽出
   AND (t.factoryId = 0 and t.translationFactoryId in (coalesce(ft.factory_id, 0), 0) OR coalesce(ft.factory_id, 0) in (t.factoryId, 0))
/*END*/
ORDER BY
     orderFactoryId
    ,coalesce(order_common.display_order, 32768)
    ,t.structure_id
    ,t.factory_order
    ,t.factory_structure_id