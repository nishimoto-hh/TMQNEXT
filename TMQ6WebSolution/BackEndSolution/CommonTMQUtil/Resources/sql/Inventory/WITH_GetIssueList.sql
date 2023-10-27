--******************************************************************
--出庫一覧WITH句
--******************************************************************
WITH number_unit AS ( 
    --数量管理単位
    SELECT
        structure_id AS unit_id,
        ex.extension_data AS unit_digit
    FROM
        ms_structure ms
        LEFT JOIN
            ms_item_extension ex
        ON  ms.structure_item_id = ex.item_id
        AND ex.sequence_no = 2
    WHERE
        ms.structure_group_id = 1730
) 
, unit_round AS ( 
    --丸め処理区分
    SELECT
        ms.factory_id,
        ex.extension_data AS round_division
    FROM
        ms_structure ms
        LEFT JOIN
            ms_item_extension ex
        ON  ms.structure_item_id = ex.item_id
        AND ex.sequence_no = 1
    WHERE
        ms.structure_group_id = 2050
        AND ms.delete_flg = 0
) 
, currency_unit AS ( 
    --金額管理単位
    SELECT
        structure_id AS currency_id
        , ex.extension_data AS currency_digit
    FROM
        ms_structure ms 
        LEFT JOIN ms_item_extension ex 
            ON ms.structure_item_id = ex.item_id 
            AND ex.sequence_no = 2 
    WHERE
        ms.structure_group_id = 1740 
) 
, inout_division AS ( 
    --ビューより受払区分を取得
    SELECT
        structure_id
        , ie.extension_data
    FROM
        ms_structure AS ms 
        INNER JOIN ms_item_extension AS ie 
            ON ms.structure_item_id = ie.item_id 
            AND ms.structure_group_id = 1950 
            AND ie.sequence_no = 1
) 
, translate_target AS(
    --******************************************************************
    --棚差調整出庫一覧
    --******************************************************************
         /*******************受払履歴テーブルより抽出*******************/
    SELECT DISTINCT
        FORMAT(pih.inout_datetime, 'yyyy/MM/dd') AS inout_datetime                       --出庫日(受払日時)
        , pih.work_no AS inout_no                                                        --出庫No(作業No)
        , pih.work_division_structure_id                                                 --入出庫区分
        , pps.parts_no                                                                   --予備品No
        , pps.parts_name                                                                 --予備品名
        , plt.old_new_structure_id                                                       --新旧区分
        , pps.standard_size                                                              --規格・寸法
        , SUM(pih.inout_quantity) AS issue_quantity                                      --受払数(出庫数)
        , plt.unit_structure_id                                                          --数量管理単位ID
        , SUM(pih.inout_quantity * plt.unit_price) AS issue_amount                       --受払数*入庫単価
        , plt.currency_structure_id                                                     --金額管理単位
        , COALESCE(number_unit.unit_digit, 0) AS unit_digit                              --小数点以下桁数(数量)
        , COALESCE(currency_unit.currency_digit, 0) AS currency_digit                    --小数点以下桁数(金額)
        , COALESCE(unit_round.round_division, 0) AS unit_round_division                  --丸め処理区分(数量)
        , COALESCE(unit_round.round_division, 0) AS currency_round_division              --丸め処理区分(金額)
        , CAST(pih.work_no AS varchar) + '_0'  AS control_char                           --作業No_0(子要素との連携用)
        , 0 AS control_flag                                                              --制御用フラグ
        , plt.department_structure_id                                                    --部門ID
        , plt.account_structure_id                                                       --勘定科目ID
        , pps.factory_id                                                                 --工場ID
    FROM
        pt_inout_history pih                                --受払履歴
        LEFT JOIN pt_lot plt                                --ロット情報
            ON pih.lot_control_id = plt.lot_control_id 
        LEFT JOIN pt_parts pps                              --予備品仕様マスタ
            ON plt.parts_id = pps.parts_id 
        LEFT JOIN pt_location_stock pls                     --在庫データマスタ
            ON pps.parts_id = pls.parts_id 
            AND pih.inventory_control_id = pls.inventory_control_id 
        LEFT JOIN pt_inventory pit                          --棚卸データ
            ON plt.department_structure_id = pit.department_structure_id 
            AND pls.parts_location_id = pit.parts_location_id 
        LEFT JOIN number_unit                               --数量管理単位
            ON plt.unit_structure_id = number_unit.unit_id 
        LEFT JOIN unit_round                                --丸め処理区分
            ON pps.factory_id = unit_round.factory_id 
        LEFT JOIN currency_unit                             --金額管理単位
            ON plt.currency_structure_id = currency_unit.currency_id 
        LEFT JOIN inout_division 
            ON pih.inout_division_structure_id = inout_division.structure_id 
    WHERE
        pit.target_month >= @TargetYearMonth                 --対象年月
        AND pit.target_month <= @TargetYearMonthNext         --対象年月
        AND pit.parts_location_id IN @PartsLocationIdList    --棚番
        AND pit.department_structure_id IN @DepartmentIdList  --部門
        AND pit.inventory_datetime IS NOT NULL                --棚卸実施日時
        AND pih.inout_datetime <= @TargetYearMonthNext         --受払日時 < 対象年月の末日
        AND (( 
            pit.difference_datetime IS NULL                   --棚差調整日時
            AND pit.preparation_datetime < pih.update_datetime
        ) 
        OR ( 
            pit.difference_datetime IS NOT NULL               --棚差調整日時
            AND pit.preparation_datetime < pih.update_datetime 
            AND pih.update_datetime < pit.difference_datetime
        ))
        AND inout_division.extension_data = '2'                --払出
        AND pih.delete_flg = 0
        /*@FactoryIdList
        AND pps.factory_id IN @FactoryIdList
        @FactoryIdList*/
        /*@JobIdList
        AND pps.job_structure_id IN @JobIdList
        @JobIdList*/
    GROUP BY
        pih.inout_datetime
        , pih.work_no
        , pih.work_division_structure_id
        , pps.parts_no
        , pps.parts_name
        , plt.old_new_structure_id
        , pps.standard_size
        , plt.unit_structure_id
        , plt.currency_structure_id
        , number_unit.unit_digit
        , currency_unit.currency_digit
        , unit_round.round_division
        , plt.department_structure_id
        , plt.account_structure_id
        , pps.factory_id
             /*******************棚差調整データテーブルより抽出*******************/
    UNION ALL
    SELECT DISTINCT
        FORMAT(pid.inout_datetime, 'yyyy/MM/dd') AS inout_datetime                       --出庫日(受払日時)
        , pid.work_no AS inout_no                                                        --出庫No(作業No)
        , pid.work_division_structure_id                                                 --入出庫区分
        , pps.parts_no                                                                   --予備品No
        , pps.parts_name                                                                 --予備品名
        , pit.old_new_structure_id                                                       --新旧区分
        , pps.standard_size                                                              --規格・寸法
        , SUM(pid.inout_quantity) AS issue_quantity                                      --受払数(出庫数)
        , plt.unit_structure_id                                                          --数量管理単位ID
        , SUM(pid.inout_quantity * plt.unit_price) AS issue_amount                       --受払数*入庫単価
        , plt.currency_structure_id                                                      --金額管理単位ID
        , COALESCE(number_unit.unit_digit, 0) AS unit_digit                              --小数点以下桁数(数量)
        , COALESCE(currency_unit.currency_digit, 0) AS currency_digit                    --小数点以下桁数(金額)
        , COALESCE(unit_round.round_division, 0) AS unit_round_division                  --丸め処理区分(数量)
        , COALESCE(unit_round.round_division, 0) AS currency_round_division              --丸め処理区分(金額)
        , CAST(pit.inventory_id AS varchar) + '_1'  AS control_char                      --棚卸ID_1(子要素との連携用)
        , 0 AS control_flag                                                              --制御用フラグ
        , pit.department_structure_id                                                    --部門ID
        , pit.account_structure_id                                                       --勘定科目ID
        , pps.factory_id                                                                 --工場ID
    FROM
        pt_inventory_difference pid                 --棚差調整データ
        LEFT JOIN pt_inventory pit                  --棚卸データ
            ON pit.inventory_id = pid.inventory_id 
        LEFT JOIN pt_lot plt                        --ロット情報
            ON pid.lot_control_id = plt.lot_control_id 
            AND pit.department_structure_id = plt.department_structure_id
        LEFT JOIN pt_parts pps                      --予備品仕様マスタ
            ON plt.parts_id = pps.parts_id 
        LEFT JOIN pt_location_stock pls                     --在庫データマスタ
            ON pps.parts_id = pls.parts_id 
            AND pit.parts_location_id = pls.parts_location_id
            AND pid.inventory_control_id = pls.inventory_control_id 
        LEFT JOIN number_unit                       --数量管理単位
            ON plt.unit_structure_id = number_unit.unit_id 
        LEFT JOIN unit_round                        --丸め処理区分(数量)
            ON pps.factory_id = unit_round.factory_id 
        LEFT JOIN currency_unit                     --金額管理単位
            ON plt.currency_structure_id = currency_unit.currency_id 
        LEFT JOIN inout_division 
            ON pid.inout_division_structure_id = inout_division.structure_id 
    WHERE
        pit.target_month >= @TargetYearMonth                 --対象年月
        AND pit.target_month <= @TargetYearMonthNext         --対象年月
        AND pit.parts_location_id IN @PartsLocationIdList    --棚番
        AND pit.department_structure_id IN @DepartmentIdList --部門
        AND pit.difference_datetime IS NOT NULL
        AND pid.inout_datetime <= @TargetYearMonthNext        --受払日時 < 対象年月の末日
        AND inout_division.extension_data = '2'              --払出
    /*@FactoryIdList
        AND pps.factory_id IN @FactoryIdList
    @FactoryIdList*/
    /*@JobIdList
        AND pps.job_structure_id IN @JobIdList
    @JobIdList*/
    GROUP BY
        pit.inventory_id
        , pid.inout_datetime
        , pid.work_no
        , pid.work_division_structure_id
        , pps.parts_no
        , pps.parts_name
        , pit.old_new_structure_id
        , pps.standard_size
        , plt.unit_structure_id
        , plt.currency_structure_id
        , number_unit.unit_digit
        , currency_unit.currency_digit
        , unit_round.round_division
        , pit.department_structure_id
        , pit.account_structure_id
        , pps.factory_id
)
, structure_factory AS ( 
    -- 使用する構成グループの構成IDを絞込、工場の指定に用いる
    SELECT
        structure_id
        , location_structure_id AS factory_id 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id IN ( 
            1730
            , 1740
            , 1940
            , 1960
        ) 
        AND language_id = @LanguageId
) 