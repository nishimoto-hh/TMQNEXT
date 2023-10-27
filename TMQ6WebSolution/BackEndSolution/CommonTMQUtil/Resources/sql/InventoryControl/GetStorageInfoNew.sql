--******************************************************************
--入庫入力(入庫情報情報)：新規
--******************************************************************
WITH number_unit AS ( 
    -- 数量管理単位
    SELECT
        structure_id AS unit_id
        , translation_text AS unit_name
        , ex.extension_data AS unit_digit 
    FROM
        v_structure_item unit 
        LEFT JOIN ms_item_extension ex 
            ON unit.structure_item_id = ex.item_id 
            AND ex.sequence_no = 2 
    WHERE
        unit.structure_group_id = 1730 
        AND unit.language_id = @LanguageId
) 
, currency_unit AS(
    --金額管理単位
    SELECT
        structure_id AS currency_id
        , translation_text AS currency_name
        , ex.extension_data AS currency_digit
    FROM
        v_structure_item unit
        LEFT JOIN
            ms_item_extension ex
        ON  unit.structure_item_id = ex.item_id
        AND ex.sequence_no = 2
    WHERE
        unit.structure_group_id = 1740
    AND unit.language_id = @LanguageId
)
, unit_round AS ( 
    --丸め処理区分
    SELECT
        ms.factory_id
        , ex.extension_data AS round_division 
    FROM
        ms_structure ms 
        LEFT JOIN ms_item item 
            ON ms.structure_item_id = item.item_id 
            AND item.delete_flg = 0 
        LEFT JOIN ms_item_extension ex 
            ON item.item_id = ex.item_id 
            AND ex.sequence_no = 1 
    WHERE
        ms.structure_group_id = 2050 
        AND ms.delete_flg = 0
) 
, location AS ( 
    --拡張データ、翻訳を取得(棚)
    SELECT
        structure_id AS location_id
        , translation_text AS parts_location_name
    FROM
        v_structure_item si 
    WHERE
        structure_group_id = 1040 
    AND
        structure_layer_no = 3
    AND
        si.language_id = @LanguageId
)
, vender AS ( 
    --拡張データ、翻訳を取得(仕入先)
    SELECT
        structure_id AS vender_structure_id
        , translation_text AS vender_name
    FROM
        v_structure_item si 
    WHERE
        structure_group_id = 1720
    AND
        si.language_id = @LanguageId
) 
SELECT
    pp.parts_id                                 --予備品ID
    , dbo.get_target_layer_id(pp.parts_location_id, 2) AS storage_location_id --予備品倉庫ID
    , pp.parts_location_id                      --棚ID
    , location.parts_location_name              --棚番翻訳
    , pp.parts_location_detail_no               --棚枝番
    , dbo.get_target_layer_id(pp.parts_location_id, 2) AS parts_storage_location_id --予備品倉庫ID(棚情報)
    , pp.vender_structure_id AS vender_id       --仕入先ID
    , pp.vender_structure_id                    --仕入先ID
    , vender.vender_name                        --仕入先名
    , pp.unit_price                             --入庫単価
    , number_unit.unit_name                     --数量管理単位
    , currency_unit.currency_name               --金額管理単位
    , COALESCE(number_unit.unit_digit, 0) AS unit_digit     --小数点以下桁数(数量)
    , COALESCE(number_unit.unit_digit, 0) AS currency_digit --小数点以下桁数(金額)
    , COALESCE(unit_round.round_division, 0) AS round_division  --丸め処理区分
    , pp.factory_id                             --工場ID
FROM
    pt_parts AS pp 
    LEFT JOIN number_unit 
        ON pp.unit_structure_id = number_unit.unit_id 
    LEFT JOIN currency_unit 
        ON pp.currency_structure_id = currency_unit.currency_id 
    LEFT JOIN unit_round 
        ON pp.factory_id = unit_round.factory_id 
    LEFT JOIN location 
        ON pp.parts_location_id = location.location_id 
    LEFT JOIN vender 
        ON pp.vender_structure_id = vender.vender_structure_id 
WHERE
    pp.parts_id = @PartsId
