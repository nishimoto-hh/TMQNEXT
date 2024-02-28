WITH structure_factory AS ( 
    SELECT
        structure_id
        , location_structure_id AS factory_id 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id IN (1760) 
        AND language_id = /*languageId*/'ja' 
) 
, user_narrow AS ( 
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
        dbo.get_target_layer_id(ub.location_structure_id, 0) AS district_id 
    FROM
        user_narrow AS us 
        INNER JOIN ms_user_belong AS ub 
            ON (us.user_id = ub.user_id) 
    WHERE
        ub.duty_flg = 1
)                                               
-- 構成マスタより、取得した地区配下の工場を取得
, temp_factory AS ( 
    SELECT
        factory_id
        , location_structure_id AS translationfactoryid
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
-- ユーザ所属マスタの本務工場を取得
, user_factory AS ( 
    SELECT
        us.user_id
        , st.structure_id AS location_structure_id 
    FROM
        user_narrow AS us 
        INNER JOIN ms_user_belong AS ub 
            ON (us.user_id = ub.user_id) 
            AND (ub.duty_flg = 1) 
        INNER JOIN temp_factory AS st 
            ON ( 
                dbo.get_target_layer_id(ub.location_structure_id, 1) = st.structure_id
            )
)                                               
-- 表示する工場の一覧を取得
, temp AS ( 
    SELECT
        factory_id 
    FROM
        temp_factory 
    UNION 
    SELECT
        0
) 
SELECT
    0 AS factoryId
    , 0 AS translationFactoryId
    , ms.structure_id AS 'values'
    , ex.extension_data + ' ' + coalesce( 
        ( 
            SELECT
                tra.translation_text 
            FROM
                v_structure_item_all AS tra 
            WHERE
                tra.language_id = /*languageId*/'ja' 
                AND tra.location_structure_id = ( 
                    SELECT
                        max(st_f.factory_id) 
                    FROM
                        structure_factory AS st_f 
                    WHERE
                        st_f.structure_id = ms.structure_id 
                        AND st_f.factory_id IN (0, uf.location_structure_id)
                ) 
                AND tra.structure_id = ms.structure_id
        ) 
        , ( 
            SELECT
                tra.translation_text 
            FROM
                v_structure_item_all AS tra 
            WHERE
                tra.language_id = /*languageId*/'ja' 
                AND tra.location_structure_id = ( 
                    SELECT
                        min(st_f.factory_id) 
                    FROM
                        structure_factory AS st_f 
                    WHERE
                        st_f.structure_id = ms.structure_id 
                ) 
                AND tra.structure_id = ms.structure_id
        )
    ) AS labels
    , ex.extension_data AS exparam1
    , ms.delete_flg AS deleteFlg
    , 0 AS orderFactoryId
FROM
    ms_structure ms                             
    -- 取得対象の工場ID
    INNER JOIN temp 
        ON ms.factory_id = temp.factory_id      
    -- 拡張項目
    LEFT JOIN ms_item_extension ex 
        ON ms.structure_item_id = ex.item_id 
        AND ex.sequence_no = 1                  
    -- 工場ごとに工場別表示順を取得する
    LEFT OUTER JOIN ms_structure_order AS order_factory 
        ON ms.structure_id = order_factory.structure_id 
        AND ms.factory_id = order_factory.factory_id 
    -- 全工場共通の表示順
    LEFT OUTER JOIN ms_structure_order AS order_common 
        ON ms.structure_id = order_common.structure_id 
        AND order_common.factory_id = 0         
    -- ユーザの本務工場
    CROSS JOIN (SELECT location_structure_id FROM user_factory) uf 
WHERE
    ms.structure_group_id = 1760 
    AND NOT EXISTS ( 
        SELECT
            * 
        FROM
            ms_structure_unused AS unused 
        WHERE
            unused.factory_id = uf.location_structure_id 
            AND unused.structure_id = ms.structure_id
    ) 
ORDER BY
    ms.structure_group_id
    , ms.structure_layer_no                     
    -- 工場IDの表示順追加 20240227
    , ms.factory_id
    -- 工場ID毎の表示順
    , row_number() OVER ( 
        PARTITION BY
            coalesce(temp.factory_id, 0) 
        ORDER BY
            coalesce( 
                coalesce( 
                    order_factory.display_order
                    , order_common.display_order
                ) 
                , 32768
            ) 
            , ms.structure_id
    ) 
    , ms.structure_id
