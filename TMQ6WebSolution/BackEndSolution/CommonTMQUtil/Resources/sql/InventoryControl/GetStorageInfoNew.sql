--******************************************************************
--入庫入力(入庫情報情報)：新規
--******************************************************************
WITH number_unit AS ( 
    -- 数量管理単位
    SELECT
        ms.structure_id AS unit_id,
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
, currency_unit AS(
    --金額管理単位
    SELECT
        ms.structure_id AS currency_id,
        ex.extension_data AS currency_digit
    FROM
        ms_structure ms
        LEFT JOIN
            ms_item_extension ex
        ON  ms.structure_item_id = ex.item_id
        AND ex.sequence_no = 2
    WHERE
        ms.structure_group_id = 1740
)
, unit_round AS ( 
    --丸め処理区分
    SELECT
        ms.factory_id,
        ex.extension_data AS round_division 
    FROM
        (
            SELECT
                ms.factory_id,
                MAX(ms.structure_id) AS structure_id
            FROM
                ms_structure ms
            WHERE
                ms.structure_group_id = 2050
            GROUP BY
                ms.factory_id
        ) ms
        LEFT JOIN
            (
                SELECT
                    ms.structure_id,
                    ex.extension_data
                FROM
                    ms_structure ms
                    LEFT JOIN
                        ms_item_extension ex
                    ON  ms.structure_item_id = ex.item_id
                    AND ex.sequence_no = 1
                WHERE
                    ms.structure_group_id = 2050
            ) ex
        ON  ms.structure_id = ex.structure_id
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
,structure_factory as( SELECT
        structure_id
        , location_structure_id AS factory_id 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id IN (1720, 1730, 1740) 
        AND language_id = @LanguageId
)
SELECT
    pp.parts_id                                 --予備品ID
    , pp.vender_structure_id AS vender_id       --仕入先ID
    , pp.vender_structure_id                    --仕入先ID
    ,(
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
                  st_f.structure_id = pp.vender_structure_id
              AND st_f.factory_id IN(0, pp.factory_id)
           )
      AND tra.structure_id = pp.vender_structure_id
    ) AS vender_name                              -- 仕入先名
    --, vender.vender_name                        --仕入先名
    , pp.unit_price                             --入庫単価
    ,(
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
                  st_f.structure_id = pp.unit_structure_id
              AND st_f.factory_id IN(0, pp.factory_id)
           )
      AND tra.structure_id = pp.unit_structure_id
    ) AS unit_name                                -- 数量管理単位
    --, number_unit.unit_name                     -- 数量管理単位
    ,(
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
                  st_f.structure_id = pp.currency_structure_id
              AND st_f.factory_id IN(0, pp.factory_id)
           )
      AND tra.structure_id = pp.currency_structure_id
    ) AS currency_name                            -- 金額管理単位
    --, currency_unit.currency_name               --金額管理単位
    , COALESCE(number_unit.unit_digit, 0) AS unit_digit     --小数点以下桁数(数量)
    , COALESCE(number_unit.unit_digit, 0) AS currency_digit --小数点以下桁数(金額)
    , COALESCE(unit_round.round_division, 0) AS round_division  --丸め処理区分
    , pp.factory_id                             --工場ID
    , pp.factory_id AS parts_factory_id         --管理工場

    -- 新品購入時の最新のロットの入庫単価
    , coalesce(( 
        select
            lot.unit_price -- 入庫単価
        from
            pt_lot lot 
        where
            lot.lot_control_id = ( 
                select
                    max(lot.lot_control_id) -- ロット管理Noの最大値
                from
                    pt_lot lot 
                    left join pt_parts parts 
                        on lot.parts_id = parts.parts_id 
                    left join ms_structure ms 
                        on lot.old_new_structure_id = ms.structure_id 
                    left join ms_item_extension ex 
                        on ms.structure_item_id = ex.item_id 
                        and ex.sequence_no = 1 
                where
                    parts.parts_id = @PartsId

                    -- 新品が対象
                    and ex.extension_data = '0'
            )
    ), 0) as unit_price_by_newest_lot
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
WHERE
    pp.parts_id = @PartsId
