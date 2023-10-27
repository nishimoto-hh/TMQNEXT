SELECT 
    pls.parts_location_id, -- 棚番
    pls.parts_location_detail_no, 
    pp.factory_id,
    -- 棚番、棚番枝Noを画面側で設定
    -- [dbo].[get_v_structure_item](pls.parts_location_id, pp.factory_id, @LanguageId) AS parts_location_name,
    '' AS parts_location_name,

    pp.parts_name, -- 品名
    ISNULL(pp.model_type,'') + ISNULL(pp.standard_size,'') AS model_type, -- 型式(仕様)

    pp.manufacturer_structure_id, -- メーカー
    [dbo].[get_v_structure_item](pp.manufacturer_structure_id, pp.factory_id, @LanguageId) AS manufacturer_name,

    pp.parts_no, -- 予備品ｺｰﾄﾞNo.

    pl.old_new_structure_id, -- 新旧区分
    [dbo].[get_v_structure_item](pl.old_new_structure_id, pp.factory_id, @LanguageId) AS old_new_name,

    ISNULL(pls.stock_quantity, 0) as stock_quantity, -- 在庫数
    FORMAT(dbo.get_rep_rounding_value(ISNULL(pls.stock_quantity, 0) * ISNULL(pl.unit_price, 0), @CurrencyDigit, @CurrencyRoundDivision), 'F' + CAST(@CurrencyDigit AS VARCHAR)) as stock_amount, -- 購入金額
    FORMAT(pl.receiving_datetime,'yyyy/MM') as receiving_datetime, -- 入庫日

    pl.account_structure_id, -- 勘定項目
    [dbo].[get_rep_extension_data](pl.account_structure_id, pp.factory_id, @LanguageId, 1) AS account_cd,

    pl.department_structure_id, -- 部門ID
    [dbo].[get_rep_extension_data](pl.department_structure_id, pp.factory_id, @LanguageId, 1) AS department_cd,

    pl.management_no, -- 管理No
    pl.management_division -- 管理区分
FROM pt_lot pl -- ロット情報
    -- 予備品仕様マスタ
    INNER JOIN pt_parts pp 
         ON  pp.parts_id = pl.parts_id 
    INNER JOIN pt_location_stock pls -- 在庫データ
         ON pls.lot_control_id = pl.lot_control_id 
        AND pls.parts_id = pp.parts_id 
        AND pls.delete_flg = 0

/*@TargetYearMonth
    -- 確定在庫データ
    LEFT OUTER JOIN pt_fixed_stock pfs 
         ON pfs.parts_id = pp.parts_id
@TargetYearMonth*/
WHERE
    1 = 1
AND
    pl.delete_flg = 0

/*@TargetYearMonth
AND
    pfs.target_month >= CONVERT(date,@TargetYearMonth + '/01')
AND
    pfs.target_month < DATEADD(MONTH, 1, CONVERT(date,@TargetYearMonth + '/01'))
@TargetYearMonth*/

/*@FactoryId
AND
    pp.factory_id = @FactoryId
@FactoryId*/

/*@PartsNo
AND
    pp.parts_no = @PartsNo
@PartsNo*/

/*@PartsName
AND
    pp.parts_name LIKE CONCAT('%',@PartsName,'%')
@PartsName*/

/*@ModelType
AND
    pp.model_type LIKE CONCAT('%',@ModelType,'%')
@ModelType*/

/*@StandardSize
AND
    pp.standard_size LIKE CONCAT('%',@StandardSize,'%')
@StandardSize*/

/*@ManufacturerStructureId
AND
    pp.manufacturer_structure_id = @ManufacturerStructureId
@ManufacturerStructureId*/

/*@PartsServiceSpace
AND
    pp.parts_service_space LIKE CONCAT('%',@PartsServiceSpace,'%')
@PartsServiceSpace*/

/*@VenderStructureId
AND
    pl.vender_structure_id = @VenderStructureId
@VenderStructureId*/

/*@StorageLocationId
AND
    pls.parts_location_id IN ( 
        SELECT structure_id 
        FROM v_structure_item_all 
        WHERE structure_group_id = 1040 
        AND structure_layer_no = 1 
        AND parent_structure_id = @StorageLocationId 
        AND factory_id = pp.factory_id
        AND language_id =  @LanguageId
    )
@StorageLocationId*/

/*@PartsLocationId
AND
    pls.parts_location_id = @PartsLocationId
@PartsLocationId*/

/*@WorkDivisionDateFrom
AND
    EXISTS (
        SELECT * FROM pt_inout_history pih
        WHERE 
            1 = 1
        AND
            pih.inout_datetime >= CONVERT(datetime, @WorkDivisionDateFrom)
        AND
            pl.lot_control_id = pih.lot_control_id
        AND
            pls.inventory_control_id = pih.inventory_control_id
        AND 
            pih.delete_flg = 0
    )
@WorkDivisionDateFrom*/

/*@WorkDivisionDateTo
AND
    EXISTS (
        SELECT * FROM pt_inout_history pih
        WHERE 
            1 = 1
        AND
            pih.inout_datetime < DATEADD(DAY, 1, CONVERT(datetime, @WorkDivisionDateTo))
        AND
            pl.lot_control_id = pih.lot_control_id
        AND
            pls.inventory_control_id = pih.inventory_control_id
        AND 
            pih.delete_flg = 0
    )
@WorkDivisionDateTo*/

/*@WorkDivisionNo
AND
    EXISTS (
        SELECT * FROM pt_inout_history pih
        WHERE 
            1 = 1
        AND
            pih.work_no = @WorkDivisionNo
        AND
            pl.lot_control_id = pih.lot_control_id
        AND
            pls.inventory_control_id = pih.inventory_control_id
        AND 
            pih.delete_flg = 0
    )
@WorkDivisionNo*/

/*@OldNewStructureId
AND
    pl.old_new_structure_id = @OldNewStructureId
@OldNewStructureId*/

/*@AccountStructureId
AND
    pl.account_structure_id = @AccountStructureId
@AccountStructureId*/

/*@DepartmentIdList
AND
    pl.department_structure_id IN @DepartmentIdList 
@DepartmentIdList*/

/*@ManagementDivision
AND
    pl.management_division = @ManagementDivision
@ManagementDivision*/

/*@ManagementNo
AND
    pl.management_no = @ManagementNo
@ManagementNo*/

ORDER BY
    -- 棚番、予備品ｺｰﾄﾞNo.、新旧区分、勘定科目、部門コード、管理No、管理区分
     [dbo].[get_v_structure_item](pls.parts_location_id, pp.factory_id, @LanguageId)
    ,pls.parts_location_detail_no
    ,pp.parts_no
    ,[dbo].[get_v_structure_item](pl.old_new_structure_id, pp.factory_id, @LanguageId)
    ,[dbo].[get_rep_extension_data](pl.account_structure_id, pp.factory_id, @LanguageId, 1)
    ,[dbo].[get_rep_extension_data](pl.department_structure_id, pp.factory_id, @LanguageId, 1)
    ,pl.management_no
    ,pl.management_division
