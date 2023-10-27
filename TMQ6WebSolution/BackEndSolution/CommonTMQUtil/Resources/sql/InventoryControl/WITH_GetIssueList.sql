--******************************************************************
--出庫一覧WITH句
--****************************************************************** 
WITH inout_division AS ( 
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
, number_unit AS(
    --数量管理単位
    SELECT
        structure_id AS unit_id,
        translation_text AS unit_name,
        ex.extension_data AS unit_digit
    FROM
        v_structure_item_all unit
        LEFT JOIN
            ms_item_extension ex
        ON  unit.structure_item_id = ex.item_id
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
, currency_unit AS(
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
, structure_factory AS ( 
    -- 使用する構成グループの構成IDを絞込、工場の指定に用いる
    SELECT
        structure_id
        , location_structure_id AS factory_id 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id IN (1940, 1980) --新旧区分、出庫区分
        AND language_id = @LanguageId
) 
