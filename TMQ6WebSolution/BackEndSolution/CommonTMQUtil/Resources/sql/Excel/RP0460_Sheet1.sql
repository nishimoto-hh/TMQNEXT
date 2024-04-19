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
    CASE WHEN temp.receiving_datetime < DATEADD(YEAR, -1, DATEADD(MONTH, 1, CONVERT(date,@TargetYearMonthForLongStay + '/01'))) 
            AND temp.receiving_datetime >= DATEADD(YEAR, -2, DATEADD(MONTH, 1, CONVERT(date,@TargetYearMonthForLongStay + '/01'))) THEN @CheckChar ELSE '' END AS one_year,
    -- [対象年月末日の３年前の日付]＜[入庫日]≦[対象年月末日の２年前の日付]
    CASE WHEN temp.receiving_datetime < DATEADD(YEAR, -2, DATEADD(MONTH, 1, CONVERT(date,@TargetYearMonthForLongStay + '/01'))) 
            AND temp.receiving_datetime >= DATEADD(YEAR, -3, DATEADD(MONTH, 1, CONVERT(date,@TargetYearMonthForLongStay + '/01'))) THEN @CheckChar ELSE '' END AS two_years,
    -- [入庫日]≦[対象年月末日の３年前の日付]
    CASE WHEN temp.receiving_datetime < DATEADD(YEAR, -3, DATEADD(MONTH, 1, CONVERT(date,@TargetYearMonthForLongStay + '/01'))) THEN @CheckChar ELSE '' END AS over_three_years,
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
select
    COALESCE(YEAR(CONVERT(date,@TargetYearMonth + '/01')), FORMAT(GETDATE(),'yyyy')) as year  -- 対象年
    ,COALESCE(MONTH(CONVERT(date,@TargetYearMonth + '/01')), FORMAT(GETDATE(),'MM')) as month -- 対象月
    , lot.account_structure_id                                                                -- 勘定科目ID
	, lot.department_structure_id                                                             -- 部門ID
	, parts.parts_no                                                                          -- 予備品No.
	, parts.parts_name                                                                        -- 予備品名
	, parts.factory_id                                                                        -- 工場ID
	, isnull(parts.model_type, '') + isnull(parts.standard_size, '') AS dimensions            -- 型式 + 規格・寸法
	, parts.unit_structure_id                                                                 -- 数量管理単位ID
	, stock.parts_location_id                                                                 -- 棚ID
    , stock.parts_location_detail_no                                                          -- 棚枝番
	, lot.old_new_structure_id                                                                -- 新旧区分ID
	, lot.receiving_datetime                                                                  -- 入庫日
	, summary.stock_quantity                                                                  -- 在庫数
	, isnull(lot.unit_price, 0) AS unit_price                                                 -- 入庫単価
    , FORMAT(summary.amount, 'F' + CAST(@CurrencyDigit AS VARCHAR)) AS amount                 -- 在庫金額
	, summary.amount AS amount_value                                                          -- 在庫金額
    , exa.extension_data AS account_id                                                        -- 勘定科目コード
    , exd.extension_data AS department_id                                                     -- 部門コード
    , exo.extension_data AS old_new_cd                                                        -- 新旧区分コード
    , '' AS parts_location_name                                                               -- 棚番、棚番枝Noを画面側で設定
    , format(sysdatetime(), 'yyyy/MM/dd') AS output_date                                      -- 出力日付

    ----------並び順(予備品No.、新旧区分、棚番、棚枝番)----------
    , ROW_NUMBER() OVER (PARTITION BY lot.account_structure_id, lot.department_structure_id 
        ORDER BY parts.parts_no, 
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
                                st_f.structure_id = lot.old_new_structure_id
                            AND st_f.factory_id IN(0, parts.factory_id)
                        )
                    AND tra.structure_id = lot.old_new_structure_id
                ) -- 新旧区分
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
                                st_f.structure_id = stock.parts_location_id
                            AND st_f.factory_id IN(0, parts.factory_id)
                        )
                    AND tra.structure_id = stock.parts_location_id
                ) -- 棚番
                , stock.parts_location_detail_no -- 棚枝番
				) AS row_no -- 並び順


   ----------以下は翻訳を取得----------
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
                    st_f.structure_id = lot.account_structure_id
                AND st_f.factory_id IN(0, parts.factory_id)
            )
        AND tra.structure_id = lot.account_structure_id
    ) AS account_nm -- 勘定科目
    ,COALESCE(
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
                    st_f.structure_id = lot.department_structure_id
                AND st_f.factory_id IN(0, parts.factory_id)
            )
        AND tra.structure_id = lot.department_structure_id
    ),
    (
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                SELECT
                    MIN(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = lot.department_structure_id
                AND st_f.factory_id NOT IN(0, parts.factory_id)
            )
        AND tra.structure_id = lot.department_structure_id
    )
    ) AS department_nm -- 部門
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
                    st_f.structure_id = parts.unit_structure_id
                AND st_f.factory_id IN(0, parts.factory_id)
            )
        AND tra.structure_id = parts.unit_structure_id
    ) AS unit_name -- 数量管理単位
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
                    st_f.structure_id = lot.old_new_structure_id
                AND st_f.factory_id IN(0, parts.factory_id)
            )
        AND tra.structure_id = lot.old_new_structure_id
    ) AS old_new_nm -- 新旧区分

FROM
    #temp_report summary -- 集計用一時テーブル
    LEFT JOIN pt_lot lot --ロット情報
        ON summary.lot_control_id = lot.lot_control_id 

    LEFT JOIN pt_parts parts -- 予備品仕様マスタ
        ON lot.parts_id = parts.parts_id 

    LEFT JOIN pt_location_stock stock -- 在庫データ
        ON summary.inventory_control_id = stock.inventory_control_id 

    LEFT JOIN ms_structure msa -- 構成マスタ(勘定科目コード取得用)
        ON lot.account_structure_id = msa.structure_id 
    LEFT JOIN ms_item_extension exa -- アイテムマスタ拡張(勘定科目コード取得用)
        ON msa.structure_item_id = exa.item_id 
        AND exa.sequence_no = 1 

    LEFT JOIN ms_structure msd -- 構成マスタ(部門コード取得用)
        ON lot.department_structure_id = msd.structure_id 
    LEFT JOIN ms_item_extension exd -- アイテムマスタ拡張(部門コード取得用)
        ON msd.structure_item_id = exd.item_id 
        AND exd.sequence_no = 1 

    LEFT JOIN ms_structure mso -- 構成マスタ(新旧区分コード取得用)
        ON lot.old_new_structure_id = mso.structure_id 
    LEFT JOIN ms_item_extension exo -- アイテムマスタ拡張(新旧区分コード取得用)
        ON mso.structure_item_id = exo.item_id 
        AND exo.sequence_no = 1
WHERE 1 = 1
    -- 集約した一時テーブルからさらに勘定科目と部門ごとにシートを分けるため、部門と勘定科目はここでも指定する
    /*@AccountStructureId
    AND
        lot.account_structure_id = @AccountStructureId
    @AccountStructureId*/

    /*@DepartmentIdList
    AND
        lot.department_structure_id IN @DepartmentIdList 
    @DepartmentIdList*/

/*@GetCntEveryAccountAndDepartment

) temp
GROUP BY
    temp.account_id, 
    temp.department_id, 
    temp.account_structure_id, 
    temp.department_structure_id
ORDER BY
    -- 勘定科目コード、部門コード
    temp.account_id, temp.department_id

@GetCntEveryAccountAndDepartment*/

/*@GetEveryAccountAndDepartment

) temp
WHERE temp.row_no >= ( @CurrentPage - 1) * @ListMaxCnt + 1
AND temp.row_no <= @CurrentPage * @ListMaxCnt
ORDER BY
    temp.row_no

@GetEveryAccountAndDepartment*/
