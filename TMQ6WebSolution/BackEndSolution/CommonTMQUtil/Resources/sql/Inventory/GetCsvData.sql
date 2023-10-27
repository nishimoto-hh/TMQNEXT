-- 棚卸IDを保存する一時テーブル
CREATE TABLE #temp_inventory_id(inventory_id bigint);

--カンマ区切りを分割し一時テーブルへ登録
INSERT
INTO #temp_inventory_id
SELECT
    *
FROM
    STRING_SPLIT(@InventoryIds, ',');

WITH unit AS ( 
    -- 数量管理単位
    SELECT
        ms.structure_id AS unit_id
        , ex.extension_data AS unit_digit 
    FROM
        ms_structure ms 
        LEFT JOIN ms_item item 
            ON ms.structure_item_id = item.item_id 
            AND item.delete_flg = 0 
        LEFT JOIN ms_item_extension ex 
            ON item.item_id = ex.item_id 
            AND ex.sequence_no = 2 
    WHERE
        ms.structure_group_id = 1730 
        AND ms.delete_flg = 0
) 
, unit_round AS ( 
    --丸め処理区分(数量管理単位、金額管理単位)
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
, currency AS ( 
    -- 金額管理単位
    SELECT
        ms.structure_id AS currency_id
        , ex.extension_data AS currency_digit 
    FROM
        ms_structure ms 
        LEFT JOIN ms_item item 
            ON ms.structure_item_id = item.item_id 
            AND item.delete_flg = 0 
        LEFT JOIN ms_item_extension ex 
            ON item.item_id = ex.item_id 
            AND ex.sequence_no = 2 
    WHERE
        ms.structure_group_id = 1740 
        AND ms.delete_flg = 0
) 
, target_list AS ( 
    SELECT
        FORMAT(pin.target_month, 'yyyy/MM') AS target_month -- 対象年月
        , FORMAT(pin.preparation_datetime, 'yyyy/MM/dd HH:mm') AS preparation_datetime -- 棚卸準備日時
        , pin.parts_location_id AS parts_location_id -- 棚ID
        , pin.parts_location_detail_no AS parts_location_detail_no -- 棚番
        , pp.parts_no                           -- 予備品No.
        , pp.parts_name                         -- 予備品名
        , ISNULL(pp.model_type, '') + ISNULL(pp.standard_size, '') AS model_type -- 型式(仕様)
        , [dbo].[get_v_structure_item]( 
            pp.manufacturer_structure_id
            , @FactoryId
            , @LanguageId
        ) AS manufacturer_name                  -- メーカー
        , [dbo].[get_v_structure_item]( 
            pin.old_new_structure_id
            , @FactoryId
            , @LanguageId
        ) AS old_new_name                       -- 新旧区分
        , COALESCE(pin.stock_quantity, 0) * ISNULL(pp.unit_price, 0) as stock_amount -- 在庫金額
        , [dbo].[get_rep_extension_data]( 
            pin.department_structure_id
            , @FactoryId
            , @LanguageId
            , 1
        ) AS department_cd                      -- 部門コード
        , [dbo].[get_rep_extension_data]( 
            pin.account_structure_id
            , @FactoryId
            , @LanguageId
            , 1
        ) AS account_cd                         -- 勘定科目コード
        , pin.stock_quantity AS stock_quantity  -- 在庫数
        , pin.inventory_id AS inventory_id      -- 棚卸ID
        , COALESCE(unit.unit_digit, 0) AS unit_digit -- 小数点以下桁数(数量)
        , COALESCE(unit_round.round_division, 1) AS round_division -- 丸め処理区分
        , COALESCE(currency.currency_digit, 0) AS currency_digit -- 小数点以下桁数(金額)
        , pp.parts_location_id AS standard_parts_location_id -- 標準棚ID
    FROM
        pt_inventory pin 
        LEFT JOIN pt_parts pp 
            ON pp.parts_id = pin.parts_id 
            AND pp.delete_flg = 0 
        LEFT JOIN unit                          -- 数量管理単位
            ON pp.unit_structure_id = unit.unit_id 
        LEFT JOIN currency                      -- 金額管理単位
            ON pp.currency_structure_id = currency.currency_id 
        LEFT JOIN unit_round                    -- 丸め処理区分
            ON pp.factory_id = unit_round.factory_id 
    WHERE
        pin.delete_flg = 0
        AND EXISTS (
            SELECT
                *
            FROM
                #temp_inventory_id temp
            WHERE
                pin.inventory_id = temp.inventory_id
        )
) 
SELECT
    target_month                                -- 対象年月
    , STRING_AGG(preparation_datetime, '|') AS preparation_datetime -- 棚卸準備日時（パイプ区切り）
    , STRING_AGG( 
        cast(parts_location_id as nvarchar) + '|' + ISNULL(parts_location_detail_no, ' ')
        , '||'
    ) WITHIN GROUP (ORDER BY parts_location_id) AS parts_location --  棚ID、棚枝番（パイプ区切り）
    , parts_no                                  -- 予備品No.
    , parts_name                                -- 予備品名
    , model_type                                -- 型式(仕様)
    , manufacturer_name                         -- メーカー
    , old_new_name                              -- 新旧区分
    , SUM(stock_amount) AS stock_amount         -- 在庫金額
    , department_cd                             -- 部門コード
    , account_cd                                -- 勘定科目コード
    , SUM(stock_quantity) AS stock_quantity     -- 在庫数
    , STRING_AGG(inventory_id, '|') AS inventory_id -- 棚卸ID（パイプ区切り）
    , unit_digit                                -- 小数点以下桁数(数量)
    , round_division                            -- 丸め処理区分
    , currency_digit                            -- 小数点以下桁数(金額)
    , standard_parts_location_id                -- 標準棚ID
FROM
    target_list tbl 
GROUP BY
    target_month
    , parts_no
    , parts_name
    , model_type
    , manufacturer_name
    , old_new_name
    , department_cd
    , account_cd
    , unit_digit
    , round_division
    , currency_digit
    , standard_parts_location_id
ORDER BY
    STRING_AGG( 
        cast(parts_location_id as nvarchar) + '|' + ISNULL(parts_location_detail_no, ' ')
        , '||'
    ) WITHIN GROUP (ORDER BY parts_location_id) -- 棚番
    , parts_no                                  -- 予備品ｺｰﾄﾞNo.
    , old_new_name                              -- 新旧区分
    , department_cd                             -- 部門コード
    , account_cd                                -- 勘定科目コード
