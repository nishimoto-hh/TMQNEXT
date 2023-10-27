--******************************************************************
--入庫入力(入庫情報情報)：修正・参照
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
        v_structure_item_all si 
    WHERE
        structure_group_id = 1040 
    AND
        structure_layer_no = 3
    AND
        si.language_id = @LanguageId
)
, department AS ( 
    --拡張データ、翻訳を取得(部門)
    SELECT
        ms.structure_id AS department_structure_id,
        ex.extension_data AS department_cd
    FROM
        ms_structure ms
        LEFT JOIN
            ms_item_extension ex
        ON  ms.structure_item_id = ex.item_id
        AND ex.sequence_no = 1
    WHERE
        ms.structure_group_id = 1760
) 
, account AS ( 
    --拡張データ、翻訳を取得(勘定科目)
    SELECT
        structure_id AS account_structure_id
        , ex.extension_data AS account_cd
        , ex2.extension_data AS account_old_new_division
    FROM
        ms_structure ms
        LEFT JOIN
            ms_item_extension ex
        ON  ms.structure_item_id = ex.item_id
        AND ex.sequence_no = 1
        LEFT JOIN
            ms_item_extension ex2
        ON  ms.structure_item_id = ex2.item_id
        AND ex2.sequence_no = 2
    WHERE
        ms.structure_group_id = 1770
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
    , pih.inout_datetime                        --入庫日
    , dbo.get_target_layer_id(pls.parts_location_id, 2) AS storage_location_id      --予備品倉庫ID
    , pls.parts_location_id                     --棚ID
    , location.parts_location_name              --棚番翻訳
    , pls.parts_location_detail_no              --棚枝番
    , dbo.get_target_layer_id(pls.parts_location_id, 2) AS parts_storage_location_id      --予備品倉庫ID(棚情報)
    , pl.old_new_structure_id                   --新旧区分
    , pih.department_structure_id               --部門ID
    , department.department_cd                  --部門コード
    , pih.account_structure_id                  --勘定科目ID
    , account.account_cd                        --勘定科目コード
    , account.account_old_new_division          --勘定科目の新旧区分
    , pih.management_division                   --管理区分
    , pih.management_no                         --管理No.
    , pl.vender_structure_id AS vender_id       --仕入先ID
    , pl.vender_structure_id                    --仕入先ID
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
                  st_f.structure_id = pl.vender_structure_id
              AND st_f.factory_id IN(0, pp.factory_id)
           )
      AND tra.structure_id = pl.vender_structure_id
    ) AS vender_name                              -- 仕入先名
    --, vender.vender_name                        --仕入先名
    , pih.inout_quantity AS storage_quantity    --入庫数
    , pl.unit_price                             --入庫単価
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
    --, number_unit.unit_name                     --数量管理単位
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
    , pih.work_no                               --作業No
    , pih.inout_history_id                      --受払履歴ID
    , pih.update_serialid                       --更新シリアルID
    , pp.factory_id AS parts_factory_id         --管理工場
FROM
    pt_inout_history AS pih
    LEFT JOIN pt_lot AS pl
        ON pih.lot_control_id = pl.lot_control_id
    LEFT JOIN pt_location_stock AS pls
        ON pih.inventory_control_id = pls.inventory_control_id
    LEFT JOIN pt_parts AS pp
        ON pl.parts_id = pp.parts_id
    LEFT JOIN number_unit 
        ON pl.unit_structure_id = number_unit.unit_id 
    LEFT JOIN currency_unit 
        ON pl.currency_structure_id = currency_unit.currency_id 
    LEFT JOIN unit_round 
        ON pp.factory_id = unit_round.factory_id 
    LEFT JOIN location 
        ON pls.parts_location_id = location.location_id 
    LEFT JOIN department 
        ON pih.department_structure_id = department.department_structure_id 
    LEFT JOIN account 
        ON pih.account_structure_id = account.account_structure_id 
WHERE
    pih.inout_history_id = @InoutHistoryId
