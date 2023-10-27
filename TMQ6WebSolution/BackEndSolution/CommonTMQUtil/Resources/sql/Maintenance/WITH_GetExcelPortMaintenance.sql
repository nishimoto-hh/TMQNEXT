WITH structure_factory AS ( 
    -- 使用する構成グループの構成IDを絞込、工場の指定に用いる
    SELECT
        structure_id
        , location_structure_id AS factory_id 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id IN ( 
            1850
            , 1130
            , 1400
            , 1300
            , 1060
            , 1330
            , 1080
            , 1140
            , 1180
            , 1220
            , 1510
            , 1810
            , 1050
            , 1900
            , 1380
            , 1090
            , 1100
            , 1070
            , 1420
            , 1430
            , 1440
            , 1480
            , 1490
            , 1550
            , 1530
        ) 
        AND language_id = @LanguageId
) 
, activity_division AS ( 
    -- 活動区分を取得
    SELECT
        item.structure_id
        , item.factory_id
        , ex.extension_data 
    FROM
        v_structure_item_all item 
        LEFT JOIN ms_item_extension ex 
            ON item.structure_item_id = ex.item_id 
    WHERE
        item.structure_group_id = 1530
) 