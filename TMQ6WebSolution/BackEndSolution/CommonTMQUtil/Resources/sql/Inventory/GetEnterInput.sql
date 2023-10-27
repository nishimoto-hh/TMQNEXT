--******************************************************************
--入庫単位入力画面
--******************************************************************
WITH number_unit AS(
    --数量管理単位
    SELECT
        structure_id AS unit_id,
        ex.extension_data AS unit_digit
    FROM
        ms_structure ms
        LEFT JOIN
            ms_item_extension ex
        ON  ms.structure_item_id = ex.item_id
        AND ex.sequence_no = 2
    WHERE
        ms.structure_group_id = 1730
)
, unit_round AS(
    --丸め処理区分(数量管理単位)
    SELECT
        ms.factory_id,
        ex.extension_data AS unit_round_division
    FROM
        ms_structure ms
        LEFT JOIN
            ms_item_extension ex
        ON  ms.structure_item_id = ex.item_id
        AND ex.sequence_no = 1
    WHERE
        ms.structure_group_id = 2050
        AND ms.delete_flg = 0
)
, translate_target AS(
    SELECT DISTINCT
        pps.parts_no,                                                                   --予備品No
        pps.parts_name,                                                                 --予備品名
        pid.inout_quantity,                                                             --入庫数
        pit.unit_structure_id,                                                          --数量管理単位ID
        pid.unit_price,                                                                 --入庫単価
        pit.currency_structure_id,                                                      --金額管理単位ID
        COALESCE(number_unit.unit_digit, 0) AS unit_digit,                              --小数点以下桁数(数量)
        COALESCE(unit_round.unit_round_division, 0) AS unit_round_division,             --丸め処理区分(数量)
        pid.inventory_difference_id,                                                    --棚差調整ID
        pid.update_serialid,                                                            --更新シリアルID
        pps.factory_id                                                                  --工場ID
    FROM
        pt_inventory_difference pid                     --棚差調整データ
        LEFT JOIN pt_inventory pit                      --棚卸
            ON pid.inventory_id = pit.inventory_id
        LEFT JOIN pt_parts pps                          --予備品仕様
            ON pit.parts_id = pps.parts_id
        LEFT JOIN number_unit                           --数量管理単位
            ON  pit.unit_structure_id = number_unit.unit_id
        LEFT JOIN unit_round --丸め処理区分(数量)
            ON  pps.factory_id = unit_round.factory_id
    WHERE pid.inventory_difference_id = @InventoryDifferenceId
)
, structure_factory AS ( 
    -- 使用する構成グループの構成IDを絞込、工場の指定に用いる
    SELECT
        structure_id
        , location_structure_id AS factory_id 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id IN ( 
            1730
            , 1740
        ) 
        AND language_id = @LanguageId
) 
SELECT
    *
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = target.unit_structure_id 
                    AND st_f.factory_id IN (0, target.factory_id)
            ) 
            AND tra.structure_id = target.unit_structure_id
    ) AS unit_name  --数量単位名称(翻訳)
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = target.currency_structure_id 
                    AND st_f.factory_id IN (0, target.factory_id)
            ) 
            AND tra.structure_id = target.currency_structure_id
    ) AS currency_name  --金額単位名称(翻訳)
FROM
    translate_target AS target 