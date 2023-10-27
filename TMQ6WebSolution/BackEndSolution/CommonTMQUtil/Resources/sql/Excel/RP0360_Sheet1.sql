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
    WHERE
        language_id = @LanguageId
        AND ie.sequence_no = 1
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
    WHERE
        language_id = @LanguageId
        AND ie.sequence_no = 1
)
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

    sum(ISNULL(pih_to.inout_quantity, 0)) as inout_quantity, -- 出庫数
    FORMAT(SUM(dbo.get_rep_rounding_value(pih_to.inout_quantity * ISNULL(pl.unit_price, 0), @CurrencyDigit, @CurrencyRoundDivision)), 'F' + CAST(@CurrencyDigit AS VARCHAR)) as transfer_amount, -- 移庫金額
    FORMAT(pih_to.inout_datetime,'yyyy/MM') as inout_datetime, -- 検収年月

    pih.account_structure_id, -- 勘定項目
    --[dbo].[get_rep_extension_data](pih.account_structure_id, pp.factory_id, @LanguageId, 1) AS account_cd,
    sus.extension_data AS account_cd,

    pih.department_structure_id, -- 部門ID
    --[dbo].[get_rep_extension_data](pih.department_structure_id, pp.factory_id, @LanguageId, 1) AS department_cd,
    dp.extension_data AS department_cd,

    pih.management_no, -- 管理No
    pih.management_division, -- 管理区分

    pih_to.account_structure_id, -- 勘定項目（移庫先）
    -- [dbo].[get_rep_extension_data](pih_to.account_structure_id, pp.factory_id, @LanguageId, 1) AS to_account_cd,
    tsus.extension_data AS to_account_cd,

    pih_to.department_structure_id, -- 部門ID（移庫先）
    --[dbo].[get_rep_extension_data](pih_to.department_structure_id, pp.factory_id, @LanguageId, 1) AS to_department_cd,
    tdp.extension_data AS to_department_cd, 


    pih_to.management_no as to_management_no, -- 管理No（移庫先）
    pih_to.management_division as to_management_division -- 管理区分（移庫先）

FROM pt_inout_history pih -- 受払履歴（移行元）
    INNER JOIN pt_inout_history pih_to -- 受払履歴（移行先）
         ON pih_to.lot_control_id = pih.lot_control_id
        AND pih_to.inventory_control_id = pih.inventory_control_id
        AND pih_to.work_no = pih.work_no 
        AND pih.delete_flg = 0
        AND pih_to.delete_flg = 0
        -- 受払区分 1：受入（構成グループID：1950、受入：412）
        AND pih_to.inout_division_structure_id IN (
            SELECT
                structure_id
            FROM
                v_structure_item_all AS si 
            INNER JOIN ms_item_extension AS ie 
            ON si.structure_item_id = ie.item_id 
            AND si.structure_group_id = 1950 
            WHERE
                language_id = 'ja'
            AND ie.extension_data = '1'
        )
        -- 受払区分 2：払出 （構成グループID：1950、払出：413）
        AND pih.inout_division_structure_id IN (
            SELECT
                structure_id
            FROM
                v_structure_item_all AS si 
            INNER JOIN ms_item_extension AS ie 
            ON si.structure_item_id = ie.item_id 
            AND si.structure_group_id = 1950 
            WHERE
                language_id = 'ja'
            AND ie.extension_data = '2'
        ) 
        -- 作業区分 4：部門移庫
        -- （構成グループID：1960、部門移庫：402）
        AND pih_to.work_division_structure_id IN (
            SELECT
                structure_id
            FROM
                v_structure_item_all AS si 
            INNER JOIN ms_item_extension AS ie 
            ON si.structure_item_id = ie.item_id 
            AND si.structure_group_id = 1960 
            WHERE
                language_id = 'ja'
            AND ie.extension_data = '4'
        ) 
        AND pih.work_division_structure_id IN (
            SELECT
                structure_id
            FROM
                v_structure_item_all AS si 
            INNER JOIN ms_item_extension AS ie 
            ON si.structure_item_id = ie.item_id 
            AND si.structure_group_id = 1960 
            WHERE
                language_id = 'ja'
            AND ie.extension_data = '4'
        ) 
    INNER JOIN pt_lot pl -- ロット情報
         ON pl.lot_control_id = pih_to.lot_control_id
        AND pl.delete_flg = 0
    -- 予備品仕様マスタ
    INNER JOIN pt_parts pp 
         ON  pp.parts_id = pl.parts_id 
    INNER JOIN pt_location_stock pls -- 在庫データ
         ON pls.inventory_control_id = pih_to.inventory_control_id
        AND pls.lot_control_id = pih_to.lot_control_id 
        AND pls.parts_id = pp.parts_id 
        AND pls.delete_flg = 0
    LEFT JOIN Department AS dp
        ON pih.department_structure_id = dp.structure_id 
    LEFT JOIN Surveyed_Subjects AS sus
        ON pih.account_structure_id = sus.structure_id 
    LEFT JOIN Department AS tdp 
        ON pih_to.department_structure_id = tdp.structure_id 
    LEFT JOIN Surveyed_Subjects AS tsus 
        ON pih_to.account_structure_id = tsus.structure_id 

/*@TargetYearMonth
    -- 確定在庫データ
    LEFT OUTER JOIN pt_fixed_stock pfs 
         ON pfs.parts_id = pp.parts_id
@TargetYearMonth*/

WHERE
    1 = 1

/*@TargetYearMonth
AND
    pfs.target_month >= CONVERT(date,@TargetYearMonth + '/01')
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
    pih_to.inout_datetime >= CONVERT(datetime, @WorkDivisionDateFrom)
@WorkDivisionDateFrom*/

/*@WorkDivisionDateTo
AND
    pih_to.inout_datetime < DATEADD(DAY, 1, CONVERT(datetime, @WorkDivisionDateTo))
@WorkDivisionDateTo*/

/*@WorkDivisionNo
AND
    pih_to.work_no = @WorkDivisionNo
@WorkDivisionNo*/

/*@OldNewStructureId
AND
    pl.old_new_structure_id = @OldNewStructureId
@OldNewStructureId*/

/*@AccountStructureId
AND
    pih_to.account_structure_id = @AccountStructureId
@AccountStructureId*/

/*@DepartmentIdList
AND
    pih_to.department_structure_id IN @DepartmentIdList 
@DepartmentIdList*/

/*@ManagementDivision
AND
    pih_to.management_division = @ManagementDivision
@ManagementDivision*/

/*@ManagementNo
AND
    pih_to.management_no = @ManagementNo
@ManagementNo*/

GROUP BY
    -- 棚番、予備品、部門、新旧区分、勘定科目、管理区分、管理No、受払年月
     pls.parts_location_id
    ,pls.parts_location_detail_no
    ,pp.parts_name
    ,pp.model_type
    ,pp.standard_size
    ,pp.manufacturer_structure_id
    ,pp.parts_no
    ,pl.old_new_structure_id
    ,pl.unit_price
    ,FORMAT(pih_to.inout_datetime,'yyyy/MM')
    ,pih.account_structure_id
    ,pih.department_structure_id
    ,pih.management_no
    ,pih.management_division
    ,pih_to.account_structure_id
    ,pih_to.department_structure_id
    ,pih_to.management_no
    ,pih_to.management_division
    ,pp.factory_id
    ,sus.extension_data
    ,dp.extension_data
    ,tsus.extension_data
    ,tdp.extension_data 
ORDER BY
    -- 棚番、予備品ｺｰﾄﾞNo.、新旧区分、移庫年月、勘定科目、勘定科目（移庫先）、管理区分（移庫先）、管理No（移庫先）
     [dbo].[get_v_structure_item](pls.parts_location_id, pp.factory_id, @LanguageId)
    ,pls.parts_location_detail_no
    ,pp.parts_no
    ,[dbo].[get_v_structure_item](pl.old_new_structure_id, pp.factory_id, @LanguageId)
    ,FORMAT(pih_to.inout_datetime,'yyyy/MM')
    ,[dbo].[get_rep_extension_data](pih.account_structure_id, pp.factory_id, @LanguageId, 1)
    ,[dbo].[get_rep_extension_data](pih.department_structure_id, pp.factory_id, @LanguageId, 1)
    ,pih_to.management_no
    ,pih_to.management_division
