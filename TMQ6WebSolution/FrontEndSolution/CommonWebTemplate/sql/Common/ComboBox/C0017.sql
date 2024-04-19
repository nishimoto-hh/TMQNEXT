/*
 * 原因性格・倉庫/棚マスタメンテ コンボ用データリスト用SQL.(絞り込み条件付き）
 */
-- C0017
-- 工場IDで絞込を行う場合、指定された工場IDの表示順を取得する
-- 工場ID一覧を生成するための構成マスタより取得
WITH structure_factory AS ( 
    SELECT
        structure_id
        , location_structure_id AS factory_id 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id IN (/*param1*/0) 
        AND language_id = /*languageId*/'ja'
) 
SELECT
    0 AS factoryId
    , 0 AS translationFactoryId
    , ms.structure_id AS 'values'
    , ( 
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
                    AND st_f.factory_id IN (0, /*param3*/0)
            ) 
            AND tra.structure_id = ms.structure_id
    ) AS labels
    , ms.delete_flg AS deleteFlg
    , 0 AS orderFactoryId
FROM
    ms_structure ms
    -- 工場ごとに工場別表示順を取得する
    LEFT OUTER JOIN ms_structure_order AS order_factory 
        ON ms.structure_id = order_factory.structure_id 
        AND order_factory.factory_id = /*param3*/0        
        -- 全工場共通の表示順
    LEFT OUTER JOIN ms_structure_order AS order_common 
        ON ms.structure_id = order_common.structure_id 
        AND order_common.factory_id = 0
WHERE
    ms.structure_group_id = /*param1*/0
    AND ms.structure_layer_no = /*param2*/0
    AND ms.factory_id IN (0, /*param3*/0) 

    -- 工場別未使用標準アイテムに工場が含まれていないものを表示
    AND NOT EXISTS ( 
        SELECT
            * 
        FROM
            ms_structure_unused AS unused 
        WHERE
            unused.factory_id = /*param3*/0
            AND unused.structure_id = ms.structure_id
    )

ORDER BY
    ms.structure_group_id
    , ms.structure_layer_no
    , row_number() OVER ( 
        PARTITION BY
            coalesce(/*param3*/0, 0) 
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
