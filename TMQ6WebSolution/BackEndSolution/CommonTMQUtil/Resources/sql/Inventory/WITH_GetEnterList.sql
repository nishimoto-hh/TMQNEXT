--******************************************************************
--入庫一覧WITH句
--******************************************************************
WITH department AS ( 
    --拡張データを取得(部門)
    SELECT
        structure_id
        , ie.extension_data
    FROM
        ms_structure ms 
        INNER JOIN ms_item_extension ie 
            ON ms.structure_item_id = ie.item_id 
            AND ms.structure_group_id = 1760 
            AND ie.sequence_no = 1
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
, number_unit AS(
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
, unit_round AS(
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
, currency_unit AS(
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
, translate_target AS(
    --******************************************************************
    --棚差調整入庫一覧
    --******************************************************************
    /*******************受払履歴テーブルより抽出*******************/
    SELECT DISTINCT
        FORMAT(pih.inout_datetime, 'yyyy/MM/dd') AS inout_datetime,                     --入庫日
        plt.lot_no,                                                                     --ロットNo
        pih.work_division_structure_id,                                                 --作業区分
        pls.parts_location_id,                                                          --棚ID
        pls.parts_location_detail_no,                                                   --棚枝番
        pps.parts_no,                                                                   --予備品No
        pps.parts_name,                                                                 --予備品名称
        plt.old_new_structure_id,                                                       --新旧区分ID
        pps.standard_size,                                                              --規格・寸法
        dpm.extension_data AS department_cd,                                            --部門CD
        pih.inout_quantity,                                                             --受払数(画面：入庫数)
        pps.unit_structure_id,                                                           --数量管理単位ID
        plt.unit_price,                                                                 --入庫単価
        pps.currency_structure_id,                                                    --金額管理単位ID
        pih.inout_quantity * plt.unit_price AS amount_money,                            --入庫金額
        pps.parts_id,                                                                   --予備品ID
        pps.factory_id,                                                                 --工場ID
        COALESCE(number_unit.unit_digit, 0) AS unit_digit,                              --小数点以下桁数(数量)
        COALESCE(currency_unit.currency_digit, 0) AS currency_digit,                    --小数点以下桁数(金額)
        COALESCE(unit_round.round_division, 0) AS unit_round_division,                  --丸め処理区分(数量)
        COALESCE(unit_round.round_division, 0) AS currency_round_division,              --丸め処理区分(金額)
        0 AS inventory_difference_id,                                                   --棚差調整ID
        0 AS control_flag,                                                              --制御用フラグ
        plt.department_structure_id,                                                    --部門ID
        plt.account_structure_id                                                        --勘定科目ID
    FROM
        pt_parts pps                                --予備品仕様マスタ
        LEFT JOIN pt_lot plt                        --ロット情報マスタ
            ON pps.parts_id = plt.parts_id 
        LEFT JOIN pt_location_stock pls             --在庫データマスタ
            ON pps.parts_id = pls.parts_id 
        LEFT JOIN pt_inventory pit                  --棚卸データ
            ON plt.department_structure_id = pit.department_structure_id
            AND pls.parts_location_id = pit.parts_location_id
        LEFT JOIN ( 
            SELECT
                pih.inout_quantity
                , work_division_structure_id
                , pih.inventory_control_id
                , pih.lot_control_id
                , pih.inout_datetime 
                , pih.update_datetime
            FROM
                pt_inout_history pih 
                LEFT JOIN inout_division ids 
                    ON pih.inout_division_structure_id = ids.structure_id 
            WHERE
                ids.extension_data = '1' --受入
                AND pih.delete_flg = 0
        ) pih                                       --受払履歴マスタ
            ON pls.inventory_control_id = pih.inventory_control_id 
            AND plt.lot_control_id = pih.lot_control_id 
        LEFT JOIN department dpm 
            ON plt.department_structure_id = dpm.structure_id 
        LEFT JOIN number_unit --数量管理単位
            ON  pps.unit_structure_id = number_unit.unit_id
        LEFT JOIN unit_round --丸め処理区分
            ON  pps.factory_id = unit_round.factory_id
        LEFT JOIN currency_unit --金額管理単位
            ON  pps.currency_structure_id = currency_unit.currency_id
    WHERE
        pit.target_month >= @TargetYearMonth                --対象年月
        AND pit.target_month <= @TargetYearMonthNext        --対象年月
        AND pit.parts_location_id IN @PartsLocationIdList   --棚番
        AND pit.department_structure_id IN @DepartmentIdList --部門
        AND pit.inventory_datetime IS NOT NULL               --棚卸実施日時
        AND pih.inout_datetime <= @TargetYearMonthNext        --受払日時 <= 対象年月の末日
        AND ((
            pit.difference_datetime IS NULL --棚差調整日時
                AND pit.preparation_datetime < pih.update_datetime
            )
           OR (
                pit.difference_datetime IS NOT NULL --棚差調整日時
                AND pit.preparation_datetime < pih.update_datetime
                AND pih.update_datetime < pit.difference_datetime
            ))
    /*@FactoryIdList
        AND pps.factory_id IN @FactoryIdList
    @FactoryIdList*/
    /*@JobIdList
        AND pps.job_structure_id IN @JobIdList
    @JobIdList*/
    /*******************棚差調整データテーブルより抽出*******************/
    UNION ALL
    SELECT DISTINCT
        FORMAT(pid.inout_datetime, 'yyyy/MM/dd') AS inout_datetime,                     --入庫日
        plt.lot_no,                                                                     --ロットNo
        pid.work_division_structure_id,                                                 --作業区分
        pit.parts_location_id,                                                          --棚ID
        pit.parts_location_detail_no,                                                   --棚番
        pps.parts_no,                                                                   --予備品No
        pps.parts_name,                                                                 --予備品名称
        pit.old_new_structure_id,                                                       --新旧区分ID
        pps.standard_size,                                                              --規格・寸法
        dpm.extension_data AS department_cd,                                            --部門CD
        pid.inout_quantity,                                                             --受払数(画面：入庫数)
        pps.unit_structure_id,                                                           --数量管理単位ID
        pid.unit_price,                                                                 --入庫単価
        pps.currency_structure_id,                                                    --金額管理単位ID
        pid.inout_quantity * pid.unit_price AS amount_money,                            --入庫金額
        pps.parts_id,                                                                   --予備品ID
        pps.factory_id,                                                                 --工場ID
        COALESCE(number_unit.unit_digit, 0) AS unit_digit,                              --小数点以下桁数(数量)
        COALESCE(currency_unit.currency_digit, 0) AS currency_digit,                    --小数点以下桁数(金額)
        COALESCE(unit_round.round_division, 0) AS unit_round_division,                  --丸め処理区分(数量)
        COALESCE(unit_round.round_division, 0) AS currency_round_division,              --丸め処理区分(金額)
        pid.inventory_difference_id,                                                    --棚差調整ID
        1 AS control_flag,                                                              --制御用フラグ
        pit.department_structure_id,                                                    --部門ID
        pit.account_structure_id                                                        --勘定科目ID
    FROM
        pt_inventory pit 
        /*棚卸データ */
        INNER JOIN ( 
            SELECT
                pid.* 
            FROM
                pt_inventory_difference pid 
                /*棚差調整 */
                LEFT JOIN inout_division ids 
                    ON pid.inout_division_structure_id = ids.structure_id 
            WHERE
                ids.extension_data = '1' 
                /*受入 */
        ) pid 
            ON pit.inventory_id = pid.inventory_id 
        LEFT JOIN pt_lot plt 
        /*ロット情報 */
            ON pid.lot_control_id = plt.lot_control_id 
        LEFT JOIN pt_location_stock pls 
        /*在庫情報 */
            ON plt.lot_control_id = pls.lot_control_id 
        LEFT JOIN department dpm 
            ON pit.department_structure_id = dpm.structure_id 
        LEFT JOIN pt_parts pps 
        /*予備品仕様マスタ */
            ON pit.parts_id = pps.parts_id 
        LEFT JOIN number_unit 
        /*数量管理単位 */
            ON pps.unit_structure_id = number_unit.unit_id 
        LEFT JOIN unit_round 
        /*丸め処理区分 */
            ON pps.factory_id = unit_round.factory_id 
        LEFT JOIN currency_unit 
        /*金額管理単位 */
            ON pps.currency_structure_id = currency_unit.currency_id
    WHERE
        pit.target_month >= @TargetYearMonth                --対象年月
        AND pit.target_month <= @TargetYearMonthNext        --対象年月
        AND pit.parts_location_id IN @PartsLocationIdList   --棚番
        AND pit.department_structure_id IN @DepartmentIdList --部門
        AND pit.difference_datetime IS NOT NULL
    /*@FactoryIdList
        AND pps.factory_id IN @FactoryIdList
    @FactoryIdList*/
    /*@JobIdList
        AND pps.job_structure_id IN @JobIdList
    @JobIdList*/
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
            , 1760
            , 1940
            , 1960
        ) 
        AND language_id = @LanguageId
) 