SELECT
    FORMAT(pi.target_month,'yyyy/MM')  AS target_month, -- 対象年月

    pls.parts_location_id, -- 棚番
    pls.parts_location_detail_no, 
    temp.factoryId as factory_id,
    -- 棚番、棚番枝Noを画面側で設定
    -- [dbo].[get_v_structure_item](pls.parts_location_id, temp.factoryId, temp.languageId) AS parts_location_name,
    '' AS parts_location_name,

    pp.manufacturer_structure_id, -- メーカー
    [dbo].[get_v_structure_item](pp.manufacturer_structure_id, temp.factoryId, temp.languageId) AS manufacturer_name,

    pp.parts_no, -- 予備品ｺｰﾄﾞNo.
    pp.parts_name, -- 品名
    ISNULL(pp.model_type,'') + ISNULL(pp.standard_size,'') AS model_type, -- 型式(仕様)

    pp.manufacturer_structure_id, -- メーカー
    [dbo].[get_v_structure_item](pp.manufacturer_structure_id, temp.factoryId, temp.languageId) AS manufacturer_name,

    pi.old_new_structure_id, -- 新旧区分
    [dbo].[get_v_structure_item](pi.old_new_structure_id, temp.factoryId, temp.languageId)  AS old_new_name,
    
    -- 在庫金額 
    FORMAT(dbo.get_rep_rounding_value(ISNULL(pls.stock_quantity, 0) * ISNULL(pp.unit_price, 0), @CurrencyDigit, @CurrencyRoundDivision), 'F' + CAST(@CurrencyDigit AS VARCHAR)) as stock_amount, -- 出庫金額

    pl.account_structure_id, -- 勘定項目
    [dbo].[get_v_structure_item](pl.account_structure_id, temp.factoryId, temp.languageId)  AS account_name,
    [dbo].[get_rep_extension_data](pl.account_structure_id, temp.factoryId, temp.languageId, 1) AS account_cd,
    
    pl.department_structure_id, -- 部門ID
    [dbo].[get_v_structure_item](pl.department_structure_id, temp.factoryId, temp.languageId)  AS department_name,
    [dbo].[get_rep_extension_data](pl.department_structure_id, temp.factoryId, temp.languageId, 1) AS department_cd,

    -- 在庫数
    pi.stock_quantity AS stock_quantity,
    -- 棚卸準備日時	
    pi.preparation_datetime AS preparation_datetime,
    -- 棚卸実施日時
    pi.inventory_datetime AS inventory_datetime,
     -- 棚卸数
    pi.inventory_quantity AS inventory_quantity,
    -- 棚差調整数
    -- 受入はプラス、払出はマイナスで集計
    ISNULL(pid_in.in_quantity, 0) - ISNULL(pid_out.out_quantity, 0) AS inout_quantity,
    -- 棚差
    -- 　在庫数－棚卸数＋棚差調整数
    ISNULL(pi.stock_quantity, 0) - ISNULL(pi.inventory_quantity, 0) + (ISNULL(pid_in.in_quantity, 0) - ISNULL(pid_out.out_quantity, 0)) as inventory_diff,
    -- 棚卸確定日時	
    pi.fixed_datetime AS fixed_datetime

FROM
    pt_inventory pi
    INNER JOIN #temp temp
        ON pi.inventory_id = temp.Key1
    INNER JOIN pt_parts pp -- 予備品仕様マスタ
        ON pp.parts_id = pi.parts_id
        AND pp.delete_flg = 0
    INNER JOIN pt_lot pl -- ロット情報
         ON pl.parts_id = pi.parts_id
        AND pl.delete_flg = 0
    INNER JOIN pt_location_stock pls -- 在庫データ
         ON pls.parts_id = pi.parts_id
        AND pls.parts_location_id = pi.parts_location_id 
        AND pls.delete_flg = 0
    INNER JOIN pt_inout_history pih -- 受払履歴
         ON pih.lot_control_id = pl.lot_control_id
        AND pih.inventory_control_id = pls.inventory_control_id
        AND pih.delete_flg = 0
    LEFT JOIN 
        (
            SELECT inventory_id, SUM(ISNULL(inout_quantity, 0)) AS in_quantity FROM pt_inventory_difference
            -- 受払区分 1：受入（構成グループID：1950、受入）
            WHERE inout_division_structure_id IN
            (
                SELECT
                    structure_id
                FROM
                    v_structure_all AS si 
                INNER JOIN ms_item_extension AS ie 
                ON si.structure_item_id = ie.item_id 
                AND si.structure_group_id = 1950 
                WHERE
                    ie.extension_data = '1'
            )
            AND   delete_flg = 0
            GROUP BY inventory_id
        ) pid_in
         ON pid_in.inventory_id = pi.inventory_id
    LEFT JOIN 
        (
            SELECT inventory_id, SUM(ISNULL(inout_quantity, 0)) AS out_quantity FROM pt_inventory_difference
            -- 受払区分 2：払出 （構成グループID：1950、払出）
            WHERE inout_division_structure_id IN
            (
                SELECT
                    structure_id
                FROM
                    v_structure_all AS si 
                INNER JOIN ms_item_extension AS ie 
                ON si.structure_item_id = ie.item_id 
                AND si.structure_group_id = 1950 
                WHERE
                    ie.extension_data = '2'
            )
            AND   delete_flg = 0
            GROUP BY inventory_id
        ) pid_out
         ON pid_out.inventory_id = pi.inventory_id
WHERE 
    pi.delete_flg = 0
ORDER BY
    [dbo].[get_v_structure_item](pls.parts_location_id, temp.factoryId, temp.languageId), -- 棚番
    pls.parts_location_detail_no, 
    pp.parts_no, -- 予備品ｺｰﾄﾞNo.
    [dbo].[get_v_structure_item](pi.old_new_structure_id, temp.factoryId, temp.languageId), -- 新旧区分
    [dbo].[get_rep_extension_data](pl.department_structure_id, temp.factoryId, temp.languageId, 1), -- 部門コード
    [dbo].[get_rep_extension_data](pl.account_structure_id, temp.factoryId, temp.languageId, 1) -- 勘定科目コード
