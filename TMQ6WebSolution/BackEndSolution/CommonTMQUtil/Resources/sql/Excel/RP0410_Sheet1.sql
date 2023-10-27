WITH
    unit AS( -- 数量管理単位
        SELECT
            ms.structure_id AS unit_id,
            ex.extension_data AS unit_digit
        FROM
            ms_structure ms
            LEFT JOIN
                ms_item item
            ON  ms.structure_item_id = item.item_id
            AND item.delete_flg = 0
            LEFT JOIN
                ms_item_extension ex
            ON  item.item_id = ex.item_id
            AND ex.sequence_no = 2
        WHERE
            ms.structure_group_id = 1730
        AND ms.delete_flg = 0
    ),
    unit_round AS( --丸め処理区分(数量管理単位)
        SELECT
            ms.factory_id,
            ex.extension_data AS unit_round_division
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
    ),
    currency AS( -- 金額管理単位
        SELECT
            ms.structure_id AS currency_id,
            ex.extension_data AS currency_digit,
            ex2.extension_data AS currency_round_division
        FROM
            ms_structure ms
            LEFT JOIN
                ms_item item
            ON  ms.structure_item_id = item.item_id
            AND item.delete_flg = 0
            LEFT JOIN
                ms_item_extension ex
            ON  item.item_id = ex.item_id
            AND ex.sequence_no = 2
            LEFT JOIN
                ms_item_extension ex2
            ON  item.item_id = ex2.item_id
            AND ex2.sequence_no = 3
        WHERE
            ms.structure_group_id = 1740
        AND ms.delete_flg = 0
    )

SELECT 
    FORMAT(tbl.target_month,'yyyy/MM')  AS target_month, -- 対象年月
    tbl.preparation_datetime AS preparation_datetime, -- 棚卸準備日時

    tbl.parts_location_id, -- 棚番
    tbl.parts_location_detail_no, 
    tbl.factoryId as factory_id,
    -- 棚番、棚番枝Noを画面側で設定
    '' AS parts_location_name,
    tbl.parts_no, -- 予備品ｺｰﾄﾞNo.
    tbl.parts_name, -- 品名
    ISNULL(tbl.model_type,'') + ISNULL(tbl.standard_size,'') AS model_type, -- 型式(仕様)

    tbl.manufacturer_structure_id, -- メーカー
    [dbo].[get_v_structure_item](tbl.manufacturer_structure_id, tbl.factoryId, tbl.languageId) AS manufacturer_name,

    tbl.old_new_structure_id, -- 新旧区分
    [dbo].[get_v_structure_item](tbl.old_new_structure_id, tbl.factoryId, tbl.languageId) AS old_new_name,
    
    -- 在庫金額 
    tbl.stock_amount, -- 出庫金額

    tbl.account_structure_id, -- 勘定項目
    [dbo].[get_v_structure_item](tbl.account_structure_id, tbl.factoryId, tbl.languageId) AS account_name,
    [dbo].[get_rep_extension_data](tbl.account_structure_id, tbl.factoryId, tbl.languageId, 1) AS account_cd,

    tbl.department_structure_id, -- 部門ID
    [dbo].[get_v_structure_item](tbl.department_structure_id, tbl.factoryId, tbl.languageId) AS department_name,
    [dbo].[get_rep_extension_data](tbl.department_structure_id, tbl.factoryId, tbl.languageId, 1) AS department_cd,

    -- 在庫数
    tbl.stock_quantity AS stock_quantity,
    -- 棚卸ID	
    tbl.inventory_id AS inventory_id,

    COALESCE(tbl.unit_digit, 0) AS unit_digit,                                    -- 小数点以下桁数(数量)
    COALESCE(tbl.unit_round_division, 1) AS unit_round_division,                  -- 丸め処理区分(数量)
    COALESCE(tbl.currency_digit, 0) AS currency_digit,                            -- 小数点以下桁数(金額)
    COALESCE(tbl.currency_round_division, 1) AS currency_round_division           -- 丸め処理区分(金額)

    -- 検索条件
    ,'' as storage_location_id -- 予備品倉庫ID（条件）
    ,'' as department_id_list -- 部門ID（条件）
    ,'' as factory_name -- 工場名
    ,'' as warehouse_name -- 予備品倉庫名
FROM
(
    SELECT
        pin.target_month, -- 対象年月
        pin.preparation_datetime, -- 棚卸準備日時
        -- COALESCE(pls.parts_location_id, pin.parts_location_id) AS parts_location_id, -- 棚番
        -- COALESCE(pls.parts_location_detail_no, pin.parts_location_detail_no) AS parts_location_detail_no, 
        pin.parts_location_id AS parts_location_id, -- 棚番
        pin.parts_location_detail_no AS parts_location_detail_no, 
        temp.factoryId,
        temp.languageId,
        pp.parts_no, -- 予備品ｺｰﾄﾞNo.
        pp.parts_name, -- 品名
        pp.model_type,
        pp.standard_size,
        pp.manufacturer_structure_id, -- メーカー
        pin.old_new_structure_id, -- 新旧区分
        -- SUM(COALESCE(pls.stock_quantity, COALESCE(pin.stock_quantity, 0)) * ISNULL(pp.unit_price, 0)) as stock_amount, -- 出庫金額
        SUM(COALESCE(pin.stock_quantity, 0) * ISNULL(pp.unit_price, 0)) as stock_amount, -- 出庫金額
        pin.account_structure_id, -- 勘定項目
        pin.department_structure_id, -- 部門ID
        pin.stock_quantity AS stock_quantity,   -- 在庫数
        pin.inventory_id AS inventory_id,       -- 棚卸ID
        unit.unit_digit,                    -- 小数点以下桁数(数量)
        unit_round.unit_round_division,     -- 丸め処理区分(数量)
        currency.currency_digit,            -- 小数点以下桁数(金額)
        currency.currency_round_division    -- 丸め処理区分(金額)
    FROM
        #temp temp
        INNER JOIN pt_inventory pin 
            ON pin.inventory_id = temp.Key1
        LEFT JOIN pt_parts pp
            ON pp.parts_id = pin.parts_id
            AND pp.delete_flg = 0        
--        LEFT JOIN pt_lot pl 
--        /*ロット情報 */
--            ON pp.parts_id = pl.parts_id 
--            AND pl.old_new_structure_id = pin.old_new_structure_id 
--            AND pl.old_new_structure_id = pin.old_new_structure_id 
--            AND pl.department_structure_id = pin.department_structure_id 
--            AND pl.account_structure_id = pin.account_structure_id 
--        LEFT JOIN pt_location_stock pls 
--        /*在庫データ */
--            ON pl.lot_control_id = pls.lot_control_id 
--            AND pls.parts_location_id = pin.parts_location_id 
--            AND pls.parts_location_detail_no = pin.parts_location_detail_no 
        LEFT JOIN
            unit -- 数量管理単位
        ON  pp.unit_structure_id = unit.unit_id
        LEFT JOIN
            currency -- 金額管理単位
        ON  pp.currency_structure_id = currency.currency_id
        LEFT JOIN
            unit_round --丸め処理区分(数量管理単位)
        -- ON  dbo.get_target_layer_id(pp.parts_location_id, 1) = unit_round.factory_id
        ON  pp.factory_id = unit_round.factory_id
    WHERE 
        pin.delete_flg = 0

    GROUP BY
        pin.target_month,
        pin.preparation_datetime,
--        pls.parts_location_id,
--        pls.parts_location_detail_no, 
        pin.parts_location_id,
        pin.parts_location_detail_no,
        temp.factoryId,
        temp.languageId,
        pp.parts_no,
        pp.parts_name,
        pp.model_type,
        pp.standard_size,
        pp.manufacturer_structure_id,
        pin.old_new_structure_id,
        pin.account_structure_id,
        pin.department_structure_id,
        pin.stock_quantity,
        pin.inventory_id,
        unit.unit_digit,
        unit_round.unit_round_division,
        currency.currency_digit,
        currency.currency_round_division
) tbl
ORDER BY
    [dbo].[get_v_structure_item](tbl.parts_location_id, tbl.factoryId, tbl.languageId), -- 棚番
    ISNULL(parts_location_detail_no,''), -- 棚番枝番  
    tbl.parts_no, -- 予備品ｺｰﾄﾞNo.
    [dbo].[get_v_structure_item](tbl.old_new_structure_id, tbl.factoryId, tbl.languageId), -- 新旧区分
    [dbo].[get_rep_extension_data](tbl.department_structure_id, tbl.factoryId, tbl.languageId, 1), -- 部門コード
    [dbo].[get_rep_extension_data](tbl.account_structure_id, tbl.factoryId, tbl.languageId, 1) -- 勘定科目コード