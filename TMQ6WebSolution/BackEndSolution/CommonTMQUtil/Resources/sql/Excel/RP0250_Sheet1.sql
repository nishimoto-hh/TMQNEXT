SELECT 
    pls.parts_location_id, -- 棚番
    pls.parts_location_detail_no, 
    pp.factory_id,
    -- 棚番、棚番枝Noを画面側で設定
    '' AS parts_location_name,
    pp.parts_name, -- 品名
    ISNULL(pp.model_type,'') + ISNULL(pp.standard_size,'') AS model_type, -- 型式(仕様)

    pp.manufacturer_structure_id, -- メーカー
    --[dbo].[get_v_structure_item](pp.manufacturer_structure_id, pp.factory_id, @LanguageId) AS manufacturer_name,
    (
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
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = pp.manufacturer_structure_id
                AND st_f.factory_id IN(0, pp.factory_id)
            )
        AND tra.structure_id = pp.manufacturer_structure_id
    ) AS manufacturer_name, -- メーカー(翻訳)
    pp.parts_no, -- 予備品ｺｰﾄﾞNo. 
    pl.old_new_structure_id, -- 新旧区分
    --[dbo].[get_v_structure_item](pl.old_new_structure_id, pp.factory_id, @LanguageId) AS old_new_name,
    (
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
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = pl.old_new_structure_id
                AND st_f.factory_id IN(0, pp.factory_id)
            )
        AND tra.structure_id = pl.old_new_structure_id
    ) AS old_new_name, -- 新旧区分(翻訳)  
    SUM(ISNULL(pih.inout_quantity, 0)) as inout_quantity, -- 入庫数
    ISNULL(pl.unit_price, 0) as unit_price, -- 入庫単価
    SUM(dbo.get_rep_rounding_value(pih.inout_quantity * ISNULL(pl.unit_price, 0), @CurrencyDigit, @CurrencyRoundDivision)) as amount_money,
    FORMAT(pih.inout_datetime,'yyyy/MM') as inout_datetime, -- 検収年月
    pih.account_structure_id, -- 勘定項目
    [dbo].[get_rep_extension_data](pih.account_structure_id, pp.factory_id, @LanguageId, 1) AS account_cd,
    pih.department_structure_id, -- 部門ID
    [dbo].[get_rep_extension_data](pih.department_structure_id, pp.factory_id, @LanguageId, 1) AS department_cd,

    pih.management_no, -- 管理No
    pih.management_division, -- 管理区分
    '1' AS output_report_location_name_got_flg,                            -- 機能場所名称情報取得済フラグ（帳票用）
    '1' AS output_report_job_name_got_flg                                 -- 職種・機種名称情報取得済フラグ（帳票用）
FROM pt_inout_history pih -- 受払履歴
    INNER JOIN pt_lot pl -- ロット情報
         ON pl.lot_control_id = pih.lot_control_id
        AND pl.delete_flg = 0
    -- 予備品仕様マスタ
    INNER JOIN pt_parts pp 
         ON  pp.parts_id = pl.parts_id 
    INNER JOIN pt_location_stock pls -- 在庫データ
         ON pls.inventory_control_id = pih.inventory_control_id
        AND pls.lot_control_id = pih.lot_control_id 
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
    pih.delete_flg = 0
-- 受払区分 1：受入（構成グループID：1950）
AND pih.inout_division_structure_id IN (
    SELECT 
        ms.structure_id
    FROM
        ms_structure ms
    INNER JOIN
        ms_item_extension ex
    ON  ms.structure_item_id = ex.item_id
    WHERE
         ms.structure_group_id = 1950
    AND  ex.extension_data = '1'
)

-- 作業区分 1：入庫、4：部門移庫、5：棚卸入庫
-- （構成グループID：1960）
AND pih.work_division_structure_id IN (
    SELECT
        ms.structure_id
    FROM
        ms_structure ms
    INNER JOIN
        ms_item_extension ex
    ON  ms.structure_item_id = ex.item_id
    WHERE
        ms.structure_group_id = 1960
    AND ex.extension_data in ('1','4','5')
) 

/*@TargetYearMonth
AND
    pfs.target_month >= CONVERT(date,@TargetYearMonth + '/01')
@TargetYearMonth*/

/*@FactoryId
AND
    pp.factory_id = @FactoryId
@FactoryId*/

/*@JobId
AND
    pp.job_structure_id = @JobId
@JobId*/

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
    pih.inout_datetime >= CONVERT(datetime, @WorkDivisionDateFrom)
@WorkDivisionDateFrom*/

/*@WorkDivisionDateTo
AND
    pih.inout_datetime < DATEADD(DAY, 1, CONVERT(datetime, @WorkDivisionDateTo))
@WorkDivisionDateTo*/

/*@WorkDivisionNo
AND
    pih.work_no = @WorkDivisionNo
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
    pih.department_structure_id IN @DepartmentIdList 
@DepartmentIdList*/

/*@ManagementDivision
AND
    pih.management_division = @ManagementDivision
@ManagementDivision*/

/*@ManagementNo
AND
    pih.management_no = @ManagementNo
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
    ,FORMAT(pih.inout_datetime,'yyyy/MM')
    ,pih.account_structure_id
    ,pih.department_structure_id
    ,pih.management_no
    ,pih.management_division
    ,pp.factory_id
ORDER BY
    -- 棚番、予備品ｺｰﾄﾞNo.、新旧区分、出庫年月、勘定科目、部門コード、管理No、管理区分
     --[dbo].[get_v_structure_item](pls.parts_location_id, pp.factory_id, @LanguageId)
    (
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
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = pls.parts_location_id
                AND st_f.factory_id IN(0, pp.factory_id)
            )
        AND tra.structure_id = pls.parts_location_id
    )
    ,pls.parts_location_detail_no
    ,pp.parts_no
    --,[dbo].[get_v_structure_item](pl.old_new_structure_id, pp.factory_id, @LanguageId)
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
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = pl.old_new_structure_id
                AND st_f.factory_id IN(0, pp.factory_id)
            )
        AND tra.structure_id = pl.old_new_structure_id
    )
    ,FORMAT(pih.inout_datetime,'yyyy/MM')
    ,[dbo].[get_rep_extension_data](pih.account_structure_id, pp.factory_id, @LanguageId, 1)
    ,[dbo].[get_rep_extension_data](pih.department_structure_id, pp.factory_id, @LanguageId, 1)
    ,pih.management_no
    ,pih.management_division

