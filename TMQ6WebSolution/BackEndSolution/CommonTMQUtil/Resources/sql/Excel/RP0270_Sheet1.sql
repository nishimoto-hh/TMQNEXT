DECLARE @CheckChar NVARCHAR(2)
SET @CheckChar = N'✔'

DECLARE @AccountCdB4170 NVARCHAR(5)
SET @AccountCdB4170 = 'B4170'

-- 勘定科目、部門ごとの件数取得用
/*@GetCntEveryAccountAndDepartment

SELECT
    temp.account_id AS account_id, 
    temp.department_id AS department_id,
    temp.account_structure_id AS account_structure_id, 
    temp.department_structure_id AS department_structure_id,
    SUM(amount_value) AS total,
    COUNT(*) AS data_cnt
FROM (

@GetCntEveryAccountAndDepartment*/

-- ページ番号を指定しての取得用
/*@GetEveryAccountAndDepartment

SELECT
    temp.year, 
    temp.month, 
    temp.account_nm, 
    temp.department_id, 
    temp.department_nm, 
    temp.row_no,
    temp.parts_name, 
    temp.dimensions, 
    temp.unit_name, 
    temp.stock_quantity, 
    temp.unit_price, 
    temp.amount, 
    CASE WHEN temp.old_new_cd = '0' THEN @CheckChar ELSE '' END AS brand_new, -- 新品
    CASE WHEN temp.old_new_cd = '1' THEN @CheckChar ELSE '' END AS used,    -- 中古品
    -- [対象年月末日の２年前の日付]＜[入庫日]≦[対象年月末日の１年前の日付]
    CASE WHEN temp.receiving_datetime < DATEADD(YEAR, -1, DATEADD(MONTH, 1, CONVERT(date,@TargetYearMonth + '/01'))) 
            AND temp.receiving_datetime >= DATEADD(YEAR, -2, CONVERT(date,@TargetYearMonth + '/01')) THEN @CheckChar ELSE '' END AS one_year,
    -- [対象年月末日の３年前の日付]＜[入庫日]≦[対象年月末日の２年前の日付]
    CASE WHEN temp.receiving_datetime < DATEADD(YEAR, -2, DATEADD(MONTH, 1, CONVERT(date,@TargetYearMonth + '/01'))) 
            AND temp.receiving_datetime >= DATEADD(YEAR, -3, CONVERT(date,@TargetYearMonth + '/01')) THEN @CheckChar ELSE '' END AS two_years,
    -- [入庫日]≦[対象年月末日の３年前の日付]
    CASE WHEN temp.receiving_datetime < DATEADD(YEAR, -3, DATEADD(MONTH, 1, CONVERT(date,@TargetYearMonth + '/01'))) THEN @CheckChar ELSE '' END AS over_three_years,
    temp.parts_location_id, 
    temp.parts_location_detail_no, 
    temp.parts_location_name, 
    temp.factory_id, 
    temp.parts_no, 
    temp.output_date,
    @CurrentPage as current_page,
    @LastPage  as last_page,
    @Total as total
    
    , '1' AS output_report_location_name_got_flg                -- 機能場所名称情報取得済フラグ（帳票用）
    , '1' AS output_report_job_name_got_flg                     -- 職種・機種名称情報取得済フラグ（帳票用）

FROM (
@GetEveryAccountAndDepartment*/

-- メイン部
SELECT 
    YEAR(CONVERT(date,@TargetYearMonth + '/01')) as year,
    MONTH(CONVERT(date,@TargetYearMonth + '/01')) as month,

    pl.account_structure_id, -- 勘定項目
    -- [dbo].[get_v_structure_item](pl.account_structure_id, pp.factory_id, @LanguageId) AS account_nm,
    --勘定項目(翻訳)
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
                    st_f.structure_id = pl.account_structure_id
                AND st_f.factory_id IN(0, pp.factory_id)
            )
        AND tra.structure_id = pl.account_structure_id
    ) AS account_nm,

    [dbo].[get_rep_extension_data](pl.account_structure_id, pp.factory_id, @LanguageId, 1) AS account_id,
    
    pl.department_structure_id, -- 部門ID
    -- [dbo].[get_v_structure_item](pl.department_structure_id, pp.factory_id, @LanguageId) AS department_nm,
    --部門(翻訳)
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
                    st_f.structure_id = pl.department_structure_id
                AND st_f.factory_id IN(0, pp.factory_id)
            )
        AND tra.structure_id = pl.department_structure_id
    ) AS department_nm,
    [dbo].[get_rep_extension_data](pl.department_structure_id, pp.factory_id, @LanguageId, 1) AS department_id,

    ROW_NUMBER() OVER (PARTITION BY pl.account_structure_id, pl.department_structure_id 
        ORDER BY pp.parts_no, 
                --[dbo].[get_v_structure_item](pl.old_new_structure_id, pp.factory_id, @LanguageId), 
                --[dbo].[get_v_structure_item](pls.parts_location_id, pp.factory_id, @LanguageId)) AS row_no,
                --新旧区分(翻訳)
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
                ),
                --棚番(翻訳)
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
                )) AS row_no,

    pp.parts_name as parts_name, -- 品名
    ISNULL(pp.model_type,'') + ISNULL(pp.standard_size,'') AS dimensions, -- 規格・寸法

    pp.unit_structure_id, -- 数量管理単位id
    -- [dbo].[get_v_structure_item](pp.unit_structure_id, pp.factory_id, @LanguageId) AS unit_name,
    --数量管理単位(翻訳)
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
                    st_f.structure_id = pp.unit_structure_id
                AND st_f.factory_id IN(0, pp.factory_id)
            )
        AND tra.structure_id = pp.unit_structure_id
    ) AS unit_name,

    ISNULL(pls.stock_quantity, 0) AS stock_quantity, -- 在庫数
    ISNULL(pl.unit_price, 0) AS unit_price, -- 単価
    FORMAT(dbo.get_rep_rounding_value(ISNULL(pls.stock_quantity, 0) * ISNULL(pl.unit_price, 0), @CurrencyDigit, @CurrencyRoundDivision), 'F' + CAST(@CurrencyDigit AS VARCHAR)) AS amount, -- 金額
    dbo.get_rep_rounding_value(ISNULL(pls.stock_quantity, 0) * ISNULL(pl.unit_price, 0), @CurrencyDigit, @CurrencyRoundDivision) AS amount_value, -- 金額
    
    pl.old_new_structure_id, -- 新旧区分
    --[dbo].[get_v_structure_item](pl.old_new_structure_id, pp.factory_id, @LanguageId) AS old_new_nm,
    --新旧区分(翻訳)
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
    ) AS old_new_nm,
    [dbo].[get_rep_extension_data](pl.old_new_structure_id, pp.factory_id, @LanguageId, 1) AS old_new_cd,
    
    pl.receiving_datetime,  -- 入庫日
    
    pls.parts_location_id, -- 棚番
    pls.parts_location_detail_no, 
    pp.factory_id,
    -- 棚番、棚番枝Noを画面側で設定
    -- [dbo].[get_v_structure_item](pls.parts_location_id, pp.factory_id, @LanguageId) AS parts_location_name,
    '' AS parts_location_name,

    pp.parts_no, -- 予備品ｺｰﾄﾞNo.
    
    FORMAT(sysdatetime(),'yyyy/MM/dd') AS output_date

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

AND EXISTS
(
    -- 抽出条件（工場ごとに設定変更可能とする）
    -- 新旧区分が「新品」「中古品」のデータのみを対象とする（循環予備品等、新旧区分マスタに追加登録されても対象としない）
    -- 勘定科目(※)が[B4170]洗替貯蔵品の場合は、入庫単価が2,000円以上のロットのみを対象とする
	SELECT * FROM
	(
        -- structure_group_id:2070 工場毎会計提出表出力条件
		SELECT TOP 1 
			dbo.get_rep_extension_data(sub_v.structure_id, sub_v.factory_id, @LanguageId, 1) exitem1,
			dbo.get_rep_extension_data(sub_v.structure_id, sub_v.factory_id, @LanguageId, 2) exitem2
		FROM
		(
            -- 個別工場ID設定を優先して該当する設定がない場合は共通の設定を採用
			SELECT factory_id, structure_id FROM v_structure_all
			WHERE structure_group_id = 2070 -- 工場毎会計提出表出力条件
			AND   factory_id = pp.factory_id
			UNION ALL
			SELECT factory_id, structure_id FROM v_structure_all
			WHERE structure_group_id = 2070 -- 工場毎会計提出表出力条件
			AND   factory_id = 0
		) sub_v
		-- 個別工場ID設定を優先する
		ORDER BY sub_v.structure_id DESC
	) sub2_v2
    -- 新旧区分が「新品」「中古品」のデータのみを対象とする
	WHERE pl.old_new_structure_id IN (
        SELECT structure_id FROM ms_structure
        INNER JOIN ms_item_extension 
            ON ms_structure.structure_item_id = ms_item_extension.item_id 
            AND ms_item_extension.sequence_no = 1
            AND ms_item_extension.extension_data in
            (
                SELECT Text FROM dbo.get_splitText(sub2_v2.exitem1,'|',default)
            )
        WHERE ms_structure.structure_group_id = 1940
    )
	AND 
    (
    -- 勘定科目コードが"B4170"で入庫単価が2,000円以上を対象とする
    -- ISNULL(pl.unit_price, 0) >= sub2_v2.exitem2
    ISNULL(pl.unit_price, 0) >= case when sub2_v2.exitem2 = '' then 2000 else ISNULL(sub2_v2.exitem2, 2000) end
    AND
    [dbo].[get_rep_extension_data](pl.account_structure_id, pp.factory_id, @LanguageId, 1) = @AccountCdB4170
    OR 
    -- 勘定科目コードが"B4170"でない
    [dbo].[get_rep_extension_data](pl.account_structure_id, pp.factory_id, @LanguageId, 1) <> @AccountCdB4170
    )
)


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

GROUP BY
    pl.account_structure_id, -- 勘定項目
    pl.department_structure_id, -- 部門ID
    pp.parts_name, -- 品名
    pp.model_type, -- 規格・寸法
    pp.standard_size,
    pp.unit_structure_id, -- 数量管理単位id
    pls.stock_quantity, -- 在庫数
    pl.unit_price, -- 単価
    pl.old_new_structure_id, -- 新旧区分
    pl.receiving_datetime,  -- 入庫日
    pls.parts_location_id, -- 棚番
    pls.parts_location_detail_no,
    pp.parts_no, -- 予備品ｺｰﾄﾞNo.
    pp.factory_id

/*@GetCntEveryAccountAndDepartment

) temp
GROUP BY
    temp.account_id, 
    temp.department_id, 
    temp.account_structure_id, 
    temp.department_structure_id
ORDER BY
    -- 勘定科目コード、部門コード、予備品No、新旧区分、棚番
    temp.account_id, temp.department_id

@GetCntEveryAccountAndDepartment*/

/*@GetEveryAccountAndDepartment

) temp
WHERE temp.row_no >= ( @CurrentPage - 1) * @ListMaxCnt + 1
AND temp.row_no <= @CurrentPage * @ListMaxCnt
ORDER BY
    temp.row_no

@GetEveryAccountAndDepartment*/
