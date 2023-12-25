/* オートコンプリート
* 予備品　棚のオートコンプリート(factoryIdList未使用)
* ユーザがシステム管理者(99)の場合は全工場、そうでなければユーザ所属マスタの本務工場の地区に紐づく工場が対象
*/
-- A0011
WITH user_narrow AS ( 
    -- ユーザマスタをログインユーザで絞込
    SELECT
        * 
    FROM
        ms_user 
    WHERE
        user_id = /*userId*/1001
) 
, user_district AS ( 
    -- ユーザ所属マスタの本務工場の地区を取得
    SELECT
        dbo.get_target_layer_id(ub.location_structure_id, 0) AS district_id 
    FROM
        user_narrow AS us 
        INNER JOIN ms_user_belong AS ub 
            ON (us.user_id = ub.user_id) 
    WHERE
        ub.duty_flg = 1
) 
, district AS ( 
    -- 地区の取得
    SELECT
        si.structure_id AS district_id
        , si.translation_text AS district_translation_text 
    FROM
        v_structure_item_all si 
    WHERE
        structure_group_id = 1000 
        AND language_id = /*languageId*/'ja' 
        AND structure_layer_no = 0
) 
, factory AS ( 
    -- 構成マスタより工場を取得
    SELECT
        si.factory_id
        , si.location_structure_id
        , si.structure_id
        , si.translation_text AS factory_translation_text
        , district.district_id
        , district.district_translation_text 
    FROM
        v_structure_item_all si 
        INNER JOIN user_district 
            ON si.parent_structure_id = user_district.district_id 
        INNER JOIN district 
            ON si.parent_structure_id = district.district_id 
    WHERE
        si.structure_group_id = 1000 
        AND si.language_id = /*languageId*/'ja' 
        AND si.structure_layer_no = 1
) 
, auth AS ( 
    -- ユーザ権限を特定するために拡張項目の値を取得
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
, user_auth AS ( 
    -- ユーザ権限を取得、管理者なら「all_flg」が1
    SELECT
        us.user_id
        , CASE au.extension_data 
            WHEN '99' THEN 1 
            ELSE 0 
            END AS all_flg 
    FROM
        user_narrow AS us 
        INNER JOIN auth AS au 
            ON (us.authority_level_id = au.structure_id)
) 
, temp AS ( 
    -- 表示する工場の一覧を取得 システム管理者の場合とそうでない場合をそれぞれ取得しUNION(実際はどちらかしか取得できない)
    -- システム管理者の場合
    SELECT
        si.factory_id
        , si.location_structure_id
        , si.structure_id
        , si.translation_text AS factory_translation_text
        , district.district_id
        , district.district_translation_text 
    FROM
        v_structure_item_all si 
        INNER JOIN district 
            ON si.parent_structure_id = district.district_id 
    WHERE
        si.structure_group_id = 1000 
        AND si.language_id = /*languageId*/'ja' 
        AND si.structure_layer_no = 1 
        AND EXISTS ( 
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
, storage AS ( 
    -- 倉庫の一覧を取得
    SELECT
        factory_id
        , location_structure_id
        , structure_id AS storage_id
        , translation_text AS storage_translation_text
        , delete_flg 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id = 1040 
        AND language_id = /*languageId*/'ja' 
        AND structure_layer_no = 2
) 
, main AS ( 
    SELECT
        -- 工場ID
        0 AS factoryId, 
        -- 翻訳工場ID
        0 AS translationFactoryId, 
        -- コード
        item.structure_id AS id, 
        -- 名称 (棚＋工場＋予備品倉庫)
        item.translation_text + ' (' + t.factory_translation_text + ' ' + s.storage_translation_text + ')' AS name, 
        -- 構成ID
        item.structure_id, 
        -- 表示順(棚の名称)
        row_number() OVER ( 
            ORDER BY
                coalesce(item.translation_text, ''), 
                item.structure_id
        ) row_num, 
        item.translation_text,
        t.district_id, 
        t.district_translation_text, 
        t.factory_id, 
        t.factory_translation_text, 
        s.storage_id, 
        s.storage_translation_text 
    FROM
        temp t 
        LEFT JOIN storage s 
            ON t.factory_id = s.factory_id 
        LEFT JOIN v_structure_item item 
            ON t.factory_id = item.factory_id 
            AND item.parent_structure_id = s.storage_id
    WHERE
        item.structure_group_id = 1040 
        AND item.language_id = /*languageId*/'ja' 
        AND item.structure_layer_no = 3         
    /*IF param1 != null && param1 != 'null' && param1 != ''*/
        AND item.parent_structure_id =/*param1*/0
    /*END*/
    /*IF param2 != null && param2 != ''*/
        /*IF !getNameFlg */
        -- 翻訳で検索
        AND (item.translation_text LIKE /*param2*/'%')
        /*END*/
        /*IF getNameFlg */
        -- 翻訳なのでID
        AND CONVERT(nvarchar, item.structure_id) = CONVERT(nvarchar, /*param2*/0)
        /*END*/
    /*END*/
) 
SELECT
    factoryId
    , translationFactoryId
    , id AS 'values'
    , name AS 'labels'
    , structure_id AS exparam1
    , translation_text AS exparam2
    , district_id AS exparam3
    , district_translation_text AS exparam4
    , factory_id AS exparam5
    , factory_translation_text AS exparam6
    , storage_id AS exparam7
    , storage_translation_text AS exparam8
FROM
    main
WHERE
    /*IF rowLimit != null && rowLimit != ''*/
    row_num < /*rowLimit*/30
    /*END*/