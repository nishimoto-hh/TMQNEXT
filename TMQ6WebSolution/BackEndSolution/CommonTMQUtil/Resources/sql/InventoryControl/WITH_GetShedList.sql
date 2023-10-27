--******************************************************************
--棚番移庫一覧WITH句
--******************************************************************
WITH Department AS ( 
    --ビューより翻訳を取得(部門)
    SELECT
        structure_id
        , ie.extension_data
        , translation_text 
    FROM
        v_structure_item_all AS si 
        INNER JOIN ms_item_extension AS ie 
            ON si.structure_item_id = ie.item_id 
            AND si.structure_group_id = 1760 
            AND ie.sequence_no = 1
    WHERE
        language_id = @LanguageId
) 
, Surveyed_Subjects AS ( 
    --ビューより翻訳を取得(勘定科目)
    SELECT
        structure_id
        , ie.extension_data
        , translation_text 
    FROM
        v_structure_item_all AS si 
        INNER JOIN ms_item_extension AS ie 
            ON si.structure_item_id = ie.item_id 
            AND si.structure_group_id = 1770 
            AND ie.sequence_no = 1
    WHERE
        language_id = @LanguageId
) 
, location AS ( 
    --拡張データ、翻訳を取得(棚)
    SELECT
        structure_id
        , translation_text 
    FROM
        v_structure_item_all si 
    WHERE
        structure_group_id = 1040 
        AND si.language_id = @LanguageId
        AND si.structure_layer_no = 3
) 
, work_division AS ( 
    --ビューより作業区分を取得
    SELECT
        structure_id
        , ie.extension_data
        , translation_text 
    FROM
        v_structure_item_all AS si 
        INNER JOIN ms_item_extension AS ie 
            ON si.structure_item_id = ie.item_id 
            AND si.structure_group_id = 1960 
    WHERE
        language_id = @LanguageId
) 
, inout_division AS ( 
    --ビューより受払区分を取得
    SELECT
        structure_id
        , ie.extension_data
        , translation_text 
    FROM
        v_structure_item_all AS si 
        INNER JOIN ms_item_extension AS ie 
            ON si.structure_item_id = ie.item_id 
            AND si.structure_group_id = 1950 
    WHERE
        language_id = @LanguageId
) 
, number_unit AS ( 
    --数量管理単位
    SELECT
        structure_id AS unit_id
        , translation_text AS unit_name
        , ex.extension_data AS unit_digit
    FROM
        v_structure_item_all unit 
        LEFT JOIN ms_item_extension ex 
            ON unit.structure_item_id = ex.item_id 
            AND ex.sequence_no = 2 
    WHERE
        unit.structure_group_id = 1730 
        AND unit.language_id = @LanguageId
) 
, unit_round AS(
    --丸め処理区分
    SELECT
        ms.factory_id,
        ex.extension_data AS round_division
    FROM
        ms_structure ms
        LEFT JOIN
            ms_item item
        ON  ms.structure_item_id = item.item_id
        AND item.delete_flg = 0
        LEFT JOIN
            ms_item_extension ex
        ON  item.item_id = ex.item_id
        AND ex.sequence_no = 1
    WHERE
        ms.structure_group_id = 2050
    AND ms.delete_flg = 0
)
, currency_unit AS ( 
    --金額管理単位
    SELECT
        structure_id AS currency_id
        , translation_text AS currency_name
        , ex.extension_data AS currency_digit
    FROM
        v_structure_item_all unit 
        LEFT JOIN ms_item_extension ex 
            ON unit.structure_item_id = ex.item_id 
            AND ex.sequence_no = 2 
    WHERE
        unit.structure_group_id = 1740 
        AND unit.language_id = @LanguageId
) 
, transfer_source AS ( 
    --移庫元受払履歴
        SELECT
            pih.work_no
            , pih.lot_control_id
            , pih.inout_datetime
            , pih.inout_quantity 
            , pls.parts_location_id
            , pls.parts_location_detail_no
        FROM
            pt_inout_history pih 
            LEFT JOIN inout_division ids 
                ON pih.inout_division_structure_id = ids.structure_id 
            LEFT JOIN work_division wds 
                ON pih.work_division_structure_id = wds.structure_id 
            LEFT JOIN pt_location_stock AS pls          --在庫データ
                 ON pih.inventory_control_id = pls.inventory_control_id
        WHERE
            wds.extension_data = '3'
            AND ids.extension_data = '1'
            AND pih.delete_flg = 0
                          AND pih.inout_datetime >= @WorkingDay 
               AND pih.inout_datetime < @WorkingDayNext 
) 
, transfer_destination AS ( 
    --移庫先受払履歴
        SELECT
            pih.lot_control_id
            , pih.work_no
            , pls.parts_location_id
            , pls.parts_location_detail_no
        FROM
            pt_inout_history pih 
            LEFT JOIN inout_division ids 
                ON pih.inout_division_structure_id = ids.structure_id 
            LEFT JOIN work_division wds 
                ON pih.work_division_structure_id = wds.structure_id 
            LEFT JOIN pt_location_stock AS pls          --在庫データ
                 ON pih.inventory_control_id = pls.inventory_control_id
        WHERE
            wds.extension_data = '3'
            AND ids.extension_data = '2'
            AND pih.delete_flg = 0
            AND pih.inout_datetime >= @WorkingDay 
            AND pih.inout_datetime < @WorkingDayNext 
) 
, structure_factory AS ( 
    -- 使用する構成グループの構成IDを絞込、工場の指定に用いる
    SELECT
        structure_id
        , location_structure_id AS factory_id 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id IN (1730, 1740, 1760, 1770, 1940,1760)
        AND language_id = @LanguageId
) 