DECLARE @AccountCdB4170 NVARCHAR(5);
SET @AccountCdB4170 = 'B4170';

WITH history_data AS ( 
    -- 受払情報取得
    SELECT
        history.inventory_control_id            -- 在庫管理ID
        , lot.lot_control_id                    -- ロット管理ID
        , history.inout_quantity                -- 受払数
        , lot.unit_price                        -- 入庫単価
        , ex.extension_data                     -- 受払区分の拡張項目
    FROM
        pt_inout_history history                -- 受払履歴
        LEFT JOIN pt_location_stock stock       -- 在庫データ
            ON history.inventory_control_id = stock.inventory_control_id
            AND stock.delete_flg = 0
            
        LEFT JOIN pt_lot lot                    -- ロット情報
            ON history.lot_control_id = lot.lot_control_id
            AND lot.delete_flg = 0

        LEFT JOIN pt_parts parts                -- 予備品仕様マスタ
            ON lot.parts_id = parts.parts_id 

        LEFT JOIN ms_structure ms               -- 構成マスタ(受払区分)
            ON history.inout_division_structure_id = ms.structure_id 

        LEFT JOIN ms_item_extension ex          -- アイテムマスタ拡張(受払区分の拡張項目)
            ON ms.structure_item_id = ex.item_id 
            AND ex.sequence_no = 1

        LEFT JOIN ms_structure msd
             ON lot.department_structure_id = msd.structure_id
    WHERE
        history.delete_flg = 0

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
			    AND   factory_id = parts.factory_id
			    UNION ALL
			    SELECT factory_id, structure_id FROM v_structure_all
			    WHERE structure_group_id = 2070 -- 工場毎会計提出表出力条件
			    AND   factory_id = 0
		    ) sub_v
		    -- 個別工場ID設定を優先する
		    ORDER BY sub_v.structure_id DESC
	    ) sub2_v2
        -- 新旧区分が「新品」「中古品」のデータのみを対象とする
	    WHERE lot.old_new_structure_id IN (
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
        ISNULL(lot.unit_price, 0) >= case when sub2_v2.exitem2 = '' then 2000 else ISNULL(sub2_v2.exitem2, 2000) end
        AND
        [dbo].[get_rep_extension_data](lot.account_structure_id, parts.factory_id, @LanguageId, 1) = @AccountCdB4170
        OR 
        -- 勘定科目コードが"B4170"でない
        [dbo].[get_rep_extension_data](lot.account_structure_id, parts.factory_id, @LanguageId, 1) <> @AccountCdB4170
        )
    )

    /*@TargetMaxDate
        -- 指定された年月末までのデータが対象になる
        -- 指定されなければ期間の絞り込みは行わないため、最新の在庫数・在庫金額が取得できる
        -- 「2024/03」が指定された場合は@TargetMaxDateには2024/04/01が格納されている
     AND history.inout_datetime < @TargetMaxDate
     @TargetMaxDate*/

    /*@FactoryIdList
    AND
        msd.factory_id in @FactoryIdList
    @FactoryIdList*/

    /*@JobId
    AND
        parts.job_structure_id = @JobId
    @JobId*/

    /*@PartsNo
    AND
        parts.parts_no = @PartsNo
    @PartsNo*/

    /*@PartsName
    AND
        parts.parts_name LIKE CONCAT('%',@PartsName,'%')
    @PartsName*/

    /*@ModelType
    AND
        parts.model_type LIKE CONCAT('%',@ModelType,'%')
    @ModelType*/

    /*@StandardSize
    AND
        parts.standard_size LIKE CONCAT('%',@StandardSize,'%')
    @StandardSize*/

    /*@ManufacturerStructureId
    AND
        parts.manufacturer_structure_id = @ManufacturerStructureId
    @ManufacturerStructureId*/

    /*@PartsServiceSpace
    AND
        parts.parts_service_space LIKE CONCAT('%',@PartsServiceSpace,'%')
    @PartsServiceSpace*/

    /*@VenderStructureId
    AND
        lot.vender_structure_id = @VenderStructureId
    @VenderStructureId*/

    /*@StorageLocationId
    AND
        stock.parts_location_id IN ( 
            SELECT structure_id 
            FROM ms_structure 
            WHERE structure_group_id = 1040 
            AND structure_layer_no = 3
            AND parent_structure_id = @StorageLocationId 
        )
    @StorageLocationId*/

    /*@PartsLocationId
    AND
        stock.parts_location_id = @PartsLocationId
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
                lot.lot_control_id = pih.lot_control_id
            AND
                stock.inventory_control_id = pih.inventory_control_id
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
                lot.lot_control_id = pih.lot_control_id
            AND
                stock.inventory_control_id = pih.inventory_control_id
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
                lot.lot_control_id = pih.lot_control_id
            AND
                stock.inventory_control_id = pih.inventory_control_id
            AND 
                pih.delete_flg = 0
        )
    @WorkDivisionNo*/

    /*@OldNewStructureId
    AND
        lot.old_new_structure_id = @OldNewStructureId
    @OldNewStructureId*/

    /*@AccountStructureId
    AND
        lot.account_structure_id = @AccountStructureId
    @AccountStructureId*/

    /*@DepartmentIdList
    AND
        lot.department_structure_id IN @DepartmentIdList 
    @DepartmentIdList*/

    /*@ManagementDivision
    AND
        lot.management_division = @ManagementDivision
    @ManagementDivision*/

    /*@ManagementNo
    AND
        lot.management_no = @ManagementNo
    @ManagementNo*/
)

--ロット管理ID・在庫管理ID・受払区分ごとに受入・払出の数量を集計
, data_inout AS ( 
    SELECT
        DATA.lot_control_id                                      -- ロット管理ID
        , DATA.inventory_control_id                              -- 在庫管理ID
        , DATA.extension_data                                    -- 拡張項目
        , sum(DATA.inout_quantity) AS quantity_in                -- 受入数
        , sum(DATA.inout_quantity * DATA.unit_price) AS price_in -- 受入金額
        , 0 AS quantity_out                                      -- 払出数(受入が対象なので「0」)
        , 0 AS price_out                                         -- 払出金額(受入が対象なので「0」)
    FROM
        history_data DATA 
    WHERE
        -- 受入のデータを取得
        DATA.extension_data = '1' 
    GROUP BY
        DATA.lot_control_id
        , DATA.inventory_control_id
        , DATA.extension_data 
    UNION ALL 
    SELECT
        DATA.lot_control_id                                       -- ロット管理ID
        , DATA.inventory_control_id                               -- 在庫管理ID
        , DATA.extension_data                                     -- 拡張項目
        , 0 AS quantity_in                                        -- 受入数(払出が対象なので「0」)
        , 0 AS price_in                                           -- 受入金額(払出が対象なので「0」)
        , sum(DATA.inout_quantity) AS quantity_out                -- 払出数
        , sum(DATA.inout_quantity * DATA.unit_price) AS price_out -- 払出金額
    FROM
        history_data DATA 
    WHERE
        -- 払出のデータを取得
        DATA.extension_data = '2' 
    GROUP BY
        DATA.lot_control_id
        , DATA.inventory_control_id
        , DATA.extension_data
)

--ロット管理ID・在庫管理IDごとに受入・払出の数量を集計
, data_inout_summary AS ( 
    SELECT
        data_inout.lot_control_id
        , data_inout.inventory_control_id
        , sum(data_inout.quantity_in) AS quantity_in
        , sum(data_inout.price_in) AS price_in
        , sum(data_inout.quantity_out) AS quantity_out
        , sum(data_inout.price_out) AS price_out 
    FROM
        data_inout 
    GROUP BY
        data_inout.lot_control_id
        , data_inout.inventory_control_id
)

-- 集計結果を一時テーブルに登録する
INSERT INTO #temp_report
SELECT 
    data_inout_summary.inventory_control_id -- 在庫管理ID
    , data_inout_summary.lot_control_id     -- ロット管理ID

    -- 在庫数
	, ISNULL(data_inout_summary.quantity_in - data_inout_summary.quantity_out, 0) as stock_quantity

    -- 在庫金額
    , dbo.get_rep_rounding_value(ISNULL(data_inout_summary.price_in - data_inout_summary.price_out, 0) , @CurrencyDigit, @CurrencyRoundDivision) AS amount
FROM
    data_inout_summary 
    LEFT JOIN pt_lot lot 
        ON data_inout_summary.lot_control_id = lot.lot_control_id 
        AND data_inout_summary.lot_control_id = lot.lot_control_id

WHERE
     -- 在庫が0より大きいものを対象にする
     ISNULL(data_inout_summary.quantity_in - data_inout_summary.quantity_out, 0) > 0