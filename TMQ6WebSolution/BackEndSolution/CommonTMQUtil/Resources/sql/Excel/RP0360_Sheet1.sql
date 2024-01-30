WITH Department AS ( 
    --ビューより翻訳を取得(部門)
    SELECT
        ms.structure_id
        , ex.extension_data
    FROM
        ms_structure ms
    INNER JOIN
        ms_item_extension ex
    ON  ms.structure_item_id = ex.item_id
    AND ex.sequence_no = 1
    WHERE ms.structure_group_id = 1760 
) 
, Surveyed_Subjects AS ( 
    --ビューより翻訳を取得(勘定科目)
    SELECT
        ms.structure_id
        , ex.extension_data
    FROM
        ms_structure ms
    INNER JOIN
        ms_item_extension ex
    ON  ms.structure_item_id = ex.item_id
    AND ex.sequence_no = 1
    WHERE ms.structure_group_id = 1770
)
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
    sum(ISNULL(pih_to.inout_quantity, 0)) as inout_quantity, -- 出庫数
    SUM(dbo.get_rep_rounding_value(pih_to.inout_quantity * ISNULL(pl.unit_price, 0), @CurrencyDigit, @CurrencyRoundDivision)) as transfer_amount, -- 移庫金額
    FORMAT(pih_to.inout_datetime,'yyyy/MM') as inout_datetime, -- 検収年月

    pih.account_structure_id, -- 勘定項目
    sus.extension_data AS account_cd,

    pih.department_structure_id, -- 部門ID
    dp.extension_data AS department_cd,

    pih.management_no, -- 管理No
    pih.management_division, -- 管理区分

    pih_to.account_structure_id, -- 勘定項目（移庫先）
    tsus.extension_data AS to_account_cd,

    pih_to.department_structure_id, -- 部門ID（移庫先）
    tdp.extension_data AS to_department_cd, 

    pih_to.management_no as to_management_no, -- 管理No（移庫先）
    pih_to.management_division as to_management_division, -- 管理区分（移庫先）

    '1' AS output_report_location_name_got_flg,                            -- 機能場所名称情報取得済フラグ（帳票用）
    '1' AS output_report_job_name_got_flg                                 -- 職種・機種名称情報取得済フラグ（帳票用）

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
        -- 受払区分 2：払出 （構成グループID：1950、払出：413）
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
             AND  ex.extension_data = '2'
        ) 
        -- 作業区分 4：部門移庫
        -- （構成グループID：1960、部門移庫：402）
        AND pih_to.work_division_structure_id IN (
            SELECT
                ms.structure_id
            FROM
                ms_structure ms
            INNER JOIN
                ms_item_extension ex
            ON  ms.structure_item_id = ex.item_id
            WHERE
                ms.structure_group_id = 1960
            AND ex.extension_data = '4'
        ) 
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
            AND ex.extension_data = '4'
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
    LEFT JOIN ms_structure ms
         ON pls.parts_location_id = ms.structure_id

/*@TargetYearMonth
    -- 確定在庫データ
    LEFT OUTER JOIN pt_fixed_stock pfs 
         ON pfs.parts_id = pp.parts_id
         AND pfs.inventory_control_id = pls.inventory_control_id
@TargetYearMonth*/

WHERE
    1 = 1

/*@TargetYearMonth
AND
    pfs.target_month >= CONVERT(date,@TargetYearMonth + '/01')
AND
    pfs.target_month < DATEADD(MONTH, 1, CONVERT(date,@TargetYearMonth + '/01'))
@TargetYearMonth*/

/*@FactoryIdList
AND
    ms.factory_id in @FactoryIdList
@FactoryIdList*/

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
        FROM ms_structure 
        WHERE structure_group_id = 1040 
        AND structure_layer_no = 3 
        AND parent_structure_id = @StorageLocationId 
        AND factory_id = pp.factory_id
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
    ,FORMAT(pih_to.inout_datetime,'yyyy/MM')
    ,[dbo].[get_rep_extension_data](pih.account_structure_id, pp.factory_id, @LanguageId, 1)
    ,[dbo].[get_rep_extension_data](pih.department_structure_id, pp.factory_id, @LanguageId, 1)
    ,pih_to.management_no
    ,pih_to.management_division
