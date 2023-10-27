WITH stock AS ( 
    --在庫データ、ロット情報から在庫数を集計
    SELECT
        pl.parts_id
        , pl.old_new_structure_id
        , pl.department_structure_id
        , pl.account_structure_id
        , pls.parts_location_id
        , pls.parts_location_detail_no
        , SUM(pls.stock_quantity) AS stock_quantity 
    FROM
        pt_lot pl 
        LEFT JOIN pt_location_stock pls 
            ON pl.lot_control_id = pls.lot_control_id 
    GROUP BY
        pl.parts_id
        , pl.old_new_structure_id
        , pl.department_structure_id
        , pl.account_structure_id
        , pls.parts_location_id
        , pls.parts_location_detail_no
) 
, inventory_diff AS ( 
    --棚差調整数
    SELECT
        parts_id
        , parts_location_id
        , parts_location_detail_no
        , old_new_structure_id
        , department_structure_id
        , account_structure_id
        , SUM(inout_quantity) AS inout_quantity 
    FROM
        ( 
            SELECT
                pi.parts_id
                , pi.parts_location_id
                , pi.parts_location_detail_no
                , pi.old_new_structure_id
                , pi.department_structure_id
                , pi.account_structure_id
                , CASE 
                    WHEN mie.extension_data = '1' 
                        THEN pid.inout_quantity 
                    WHEN mie.extension_data = '2' 
                        THEN pid.inout_quantity * - 1 
                    END AS inout_quantity 
            FROM
                pt_inventory pi 
                LEFT JOIN pt_inventory_difference pid 
                    ON pi.inventory_id = pid.inventory_id 
                LEFT JOIN ms_structure ms 
                    ON pid.inout_division_structure_id = ms.structure_id 
                LEFT JOIN ms_item_extension mie 
                    ON ms.structure_item_id = mie.item_id 
                    AND mie.sequence_no = 1 
            WHERE
                pi.target_month = @TargetYearMonth 
                AND pi.parts_location_id IN @PartsLocationIdList 
                AND pi.department_structure_id IN @DepartmentIdList 
            UNION ALL 
            SELECT
                pi.parts_id
                , pi.parts_location_id
                , pi.parts_location_detail_no
                , pi.old_new_structure_id
                , pi.department_structure_id
                , pi.account_structure_id
                , CASE 
                    WHEN mie.extension_data = '1' 
                        THEN pih.inout_quantity 
                    WHEN mie.extension_data = '2' 
                        THEN pih.inout_quantity * - 1 
                    END AS inout_quantity 
            FROM
                pt_inventory pi 
                LEFT JOIN pt_lot pl 
                    ON pi.parts_id = pl.parts_id 
                    AND pi.old_new_structure_id = pl.old_new_structure_id 
                    AND pi.department_structure_id = pl.department_structure_id 
                    AND pi.account_structure_id = pl.account_structure_id 
                LEFT JOIN pt_location_stock pls 
                    ON pi.parts_location_id = pls.parts_location_id 
                    AND pi.parts_location_detail_no = pls.parts_location_detail_no 
                LEFT JOIN pt_inout_history pih 
                    ON pl.lot_control_id = pih.lot_control_id 
                    AND pls.inventory_control_id = pih.inventory_control_id 
                    AND pih.inout_datetime <= @TargetYearMonthNext 
                    AND ( 
                        ( 
                            pi.difference_datetime IS NULL 
                            AND pi.preparation_datetime < pih.update_datetime
                        ) 
                        OR ( 
                            pi.difference_datetime IS NOT NULL 
                            AND pi.preparation_datetime < pih.update_datetime 
                            AND pih.update_datetime < pi.difference_datetime
                        )
                    ) 
                LEFT JOIN ms_structure ms 
                    ON pih.inout_division_structure_id = ms.structure_id 
                LEFT JOIN ms_item_extension mie 
                    ON ms.structure_item_id = mie.item_id 
                    AND mie.sequence_no = 1 
            WHERE
                pi.target_month = @TargetYearMonth 
                AND pi.parts_location_id IN @PartsLocationIdList 
                AND pi.department_structure_id IN @DepartmentIdList
        ) tbl 
    GROUP BY
        parts_id
        , parts_location_id
        , parts_location_detail_no
        , old_new_structure_id
        , department_structure_id
        , account_structure_id
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
    --丸め処理区分(数量管理単位)
    SELECT
        ms.factory_id,
        ex.extension_data AS unit_round_division
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
, department AS ( 
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
, account AS ( 
    --拡張データを取得(勘定科目)
    SELECT
        structure_id
        , ie.extension_data
    FROM
        ms_structure ms 
        INNER JOIN ms_item_extension ie 
            ON ms.structure_item_id = ie.item_id 
            AND ms.structure_group_id = 1770 
            AND ie.sequence_no = 1
) 
, translate_target AS(
    --棚卸一覧（ロット情報、在庫データが存在するデータ）
    SELECT DISTINCT
        pls.parts_location_id                       --棚番ID
        , pls.parts_location_detail_no              --棚枝番
        , pp.parts_no                               --予備品No
        , pp.parts_name                             --予備品名
        , pl.old_new_structure_id                   --新旧区分
        , pl.department_structure_id                --部門ID
        , department.extension_data AS department_cd --部門コード
        , pl.account_structure_id                   --勘定科目ID
        , account.extension_data AS subject_cd      --勘定科目コード
        , CASE 
            WHEN ( 
                CASE 
                    WHEN pi.inventory_id IS NULL 
                        THEN stock.stock_quantity 
                    ELSE pi.stock_quantity 
                    END
            ) > 0 
                THEN 1 
            ELSE 0 
            END AS stock_quantity_flg               --在庫あり
        , CASE 
            WHEN @InventoryIdFlg = 0 
                THEN CASE 
                WHEN pi.inventory_quantity IS NOT NULL 
                AND pi.stock_quantity + COALESCE(inventory_diff.inout_quantity, 0) - pi.inventory_quantity <> 0 
                    THEN 1 
                ELSE 0 
                END 
            ELSE CASE                               --取込の場合、取込値を使用する
                WHEN pi.temp_inventory_quantity IS NOT NULL 
                AND pi.stock_quantity + COALESCE(inventory_diff.inout_quantity, 0) - pi.temp_inventory_quantity <> 0 
                    THEN 1 
                ELSE 0 
                END 
            END AS inventory_diff_flg               --棚差あり
        , CASE 
            WHEN pi.inventory_id IS NULL 
                THEN stock.stock_quantity 
            ELSE pi.stock_quantity 
            END AS stock_quantity                   --在庫数
        , pi.preparation_datetime                   --棚卸準備日時
        , pi.inventory_datetime                     --棚卸日時
        , pi.difference_datetime                    --棚卸調整日時
        , CASE 
            WHEN @InventoryIdFlg = 0  
                THEN pi.inventory_quantity 
            ELSE pi.temp_inventory_quantity 
            END AS inventory_quantity               --棚卸数(取込の場合、取込値を表示)
        , CASE 
            WHEN @InventoryIdFlg = 0 
                THEN CASE 
                WHEN pi.inventory_quantity IS NULL 
                    THEN NULL 
                ELSE COALESCE(inventory_diff.inout_quantity, 0) 
                END 
            ELSE CASE                               --取込の場合、取込値を使用する
                WHEN pi.temp_inventory_quantity IS NULL 
                    THEN NULL 
                ELSE COALESCE(inventory_diff.inout_quantity, 0) 
                END 
            END AS inout_quantity                   --棚卸調整数
        , CASE 
            WHEN @InventoryIdFlg = 0  
                THEN CASE 
                WHEN pi.inventory_quantity IS NULL 
                    THEN NULL 
                ELSE pi.stock_quantity + COALESCE(inventory_diff.inout_quantity, 0) - pi.inventory_quantity 
                END 
            ELSE CASE                               --取込の場合、取込値を使用する
                WHEN pi.temp_inventory_quantity IS NULL 
                    THEN NULL 
                ELSE pi.stock_quantity + COALESCE(inventory_diff.inout_quantity, 0) - pi.temp_inventory_quantity 
                END 
            END AS inventory_diff                   --棚差
        , pp.manufacturer_structure_id              --メーカー
        , pp.materials                              --材質
        , pp.model_type                             --型式
        , pi.fixed_datetime                         --棚卸確定日時
        , pp.parts_id                               --予備品ID
        , pp.unit_structure_id                      --数量単位ID
        , pp.currency_structure_id                  --金額単位ID
        , pp.factory_id                             --工場ID
        , pi.inventory_id                           --棚卸ID
        , pi.update_serialid                        --棚卸データ.更新シリアルID
        , COALESCE(number_unit.unit_digit, 0) AS unit_digit --小数点以下桁数(数量)
        , COALESCE(unit_round.unit_round_division, 0) AS unit_round_division --丸め処理区分(数量)
        , pp.factory_id as parts_factory_id         --工場ID(ツリーの絞り込み用)
        , pp.job_structure_id                       --職種機種ID(ツリーの絞り込み用)
    FROM
        pt_parts pp 
        LEFT JOIN pt_lot pl                         --ロット情報
            ON pp.parts_id = pl.parts_id 
        LEFT JOIN pt_location_stock pls             --在庫データ
            ON pl.lot_control_id = pls.lot_control_id 
        LEFT JOIN pt_inventory pi                   --棚卸データ
            ON pl.parts_id = pi.parts_id 
            AND pl.old_new_structure_id = pi.old_new_structure_id 
            AND pl.department_structure_id = pi.department_structure_id 
            AND pl.account_structure_id = pi.account_structure_id 
            AND pls.parts_location_id = pi.parts_location_id 
            AND pls.parts_location_detail_no = pi.parts_location_detail_no
            AND pi.target_month = @TargetYearMonth
        LEFT JOIN stock                             --在庫データ、ロット情報から在庫数を集計したデータ
            ON pl.parts_id = stock.parts_id 
            AND pl.old_new_structure_id = stock.old_new_structure_id 
            AND pl.department_structure_id = stock.department_structure_id 
            AND pl.account_structure_id = stock.account_structure_id 
            AND pls.parts_location_id = stock.parts_location_id 
            AND pls.parts_location_detail_no = stock.parts_location_detail_no
        LEFT JOIN inventory_diff                    --棚差調整数（受払履歴）
            ON pl.parts_id = inventory_diff.parts_id 
            AND pls.parts_location_id = inventory_diff.parts_location_id 
            AND pls.parts_location_detail_no = inventory_diff.parts_location_detail_no 
            AND pl.old_new_structure_id = inventory_diff.old_new_structure_id 
            AND pl.department_structure_id = inventory_diff.department_structure_id 
            AND pl.account_structure_id = inventory_diff.account_structure_id 
        LEFT JOIN number_unit --数量管理単位
            ON  pp.unit_structure_id = number_unit.unit_id
        LEFT JOIN unit_round --丸め処理区分(数量)
            ON  pp.factory_id = unit_round.factory_id
        LEFT JOIN department --拡張データを取得(部門)
            ON  pl.department_structure_id = department.structure_id
        LEFT JOIN account --拡張データを取得(勘定科目)
            ON  pl.account_structure_id = account.structure_id
    WHERE
        pls.parts_location_id IN @PartsLocationIdList --棚番
        AND pl.department_structure_id IN @DepartmentIdList --部門
    /*@Created
        AND pi.preparation_datetime IS NOT NULL
    @Created*/
    /*@NotYet
        AND pi.inventory_id IS NULL 
    @NotYet*/
    /*@InventoryIdList
        AND pi.inventory_id IN @InventoryIdList
    @InventoryIdList*/

    UNION

    -- 棚卸一覧(ロット情報、在庫データが存在しない、新規登録画面から登録されたデータ)
    SELECT
        pi.parts_location_id                       --棚番ID
        , pi.parts_location_detail_no              --棚枝番
        , pp.parts_no                               --予備品No
        , pp.parts_name                             --予備品名
        , pi.old_new_structure_id                   --新旧区分
        , pi.department_structure_id                --部門ID
        , department.extension_data AS department_cd --部門コード
        , pi.account_structure_id                   --勘定科目ID
        , account.extension_data AS subject_cd      --勘定科目コード
        , CASE 
            WHEN pi.stock_quantity > 0 
                THEN 1 
            ELSE 0 
            END AS stock_quantity_flg               --在庫あり
        , CASE 
            WHEN @InventoryIdFlg = 0 
                THEN CASE 
                WHEN pi.inventory_quantity IS NOT NULL 
                AND pi.stock_quantity + COALESCE(inventory_diff.inout_quantity, 0) - pi.inventory_quantity <> 0 
                    THEN 1 
                ELSE 0 
                END 
            ELSE CASE                               --取込の場合、取込値を使用する
                WHEN pi.temp_inventory_quantity IS NOT NULL 
                AND pi.stock_quantity + COALESCE(inventory_diff.inout_quantity, 0) - pi.temp_inventory_quantity <> 0 
                    THEN 1 
                ELSE 0 
                END 
            END AS inventory_diff_flg               --棚差あり
        , pi.stock_quantity                         --在庫数
        , pi.preparation_datetime                   --棚卸準備日時
        , pi.inventory_datetime                     --棚卸日時
        , pi.difference_datetime                    --棚卸調整日時
        , CASE 
            WHEN @InventoryIdFlg = 0  
                THEN pi.inventory_quantity 
            ELSE pi.temp_inventory_quantity 
            END AS inventory_quantity               --棚卸数(取込の場合、取込値を表示)
        , CASE 
            WHEN @InventoryIdFlg = 0 
                THEN CASE 
                WHEN pi.inventory_quantity IS NULL 
                    THEN NULL 
                ELSE COALESCE(inventory_diff.inout_quantity, 0) 
                END 
            ELSE CASE                               --取込の場合、取込値を使用する
                WHEN pi.temp_inventory_quantity IS NULL 
                    THEN NULL 
                ELSE COALESCE(inventory_diff.inout_quantity, 0) 
                END 
            END AS inout_quantity                   --棚卸調整数
        , CASE 
            WHEN @InventoryIdFlg = 0  
                THEN CASE 
                WHEN pi.inventory_quantity IS NULL 
                    THEN NULL 
                ELSE pi.stock_quantity + COALESCE(inventory_diff.inout_quantity, 0) - pi.inventory_quantity 
                END 
            ELSE CASE                               --取込の場合、取込値を使用する
                WHEN pi.temp_inventory_quantity IS NULL 
                    THEN NULL 
                ELSE pi.stock_quantity + COALESCE(inventory_diff.inout_quantity, 0) - pi.temp_inventory_quantity 
                END 
            END AS inventory_diff                   --棚差
        , pp.manufacturer_structure_id              --メーカー
        , pp.materials                              --材質
        , pp.model_type                             --型式
        , pi.fixed_datetime                         --棚卸確定日時
        , pp.parts_id                               --予備品ID
        , pp.unit_structure_id                      --数量単位ID
        , pp.currency_structure_id                  --金額単位ID
        , pp.factory_id                             --工場ID
        , pi.inventory_id                           --棚卸ID
        , pi.update_serialid                        --棚卸データ.更新シリアルID
        , COALESCE(number_unit.unit_digit, 0) AS unit_digit --小数点以下桁数(数量)
        , COALESCE(unit_round.unit_round_division, 0) AS unit_round_division --丸め処理区分(数量)
        , pp.factory_id as parts_factory_id         --工場ID(ツリーの絞り込み用)
        , pp.job_structure_id                       --職種機種ID(ツリーの絞り込み用)
    FROM
        pt_parts pp 
        LEFT JOIN pt_inventory pi                   --棚卸データ
            ON pp.parts_id = pi.parts_id 
            AND pi.target_month = @TargetYearMonth
        LEFT JOIN inventory_diff                    --棚差調整数（受払履歴）
            ON pi.parts_id = inventory_diff.parts_id 
            AND pi.parts_location_id = inventory_diff.parts_location_id 
            AND pi.parts_location_detail_no = inventory_diff.parts_location_detail_no 
            AND pi.old_new_structure_id = inventory_diff.old_new_structure_id 
            AND pi.department_structure_id = inventory_diff.department_structure_id 
            AND pi.account_structure_id = inventory_diff.account_structure_id 
        LEFT JOIN number_unit --数量管理単位
            ON  pp.unit_structure_id = number_unit.unit_id
        LEFT JOIN unit_round --丸め処理区分(数量)
            ON  pp.factory_id = unit_round.factory_id
        LEFT JOIN department --拡張データを取得(部門)
            ON  pi.department_structure_id = department.structure_id
        LEFT JOIN account --拡張データを取得(勘定科目)
            ON  pi.account_structure_id = account.structure_id
    WHERE
        pi.parts_location_id IN @PartsLocationIdList --棚番
        AND pi.department_structure_id IN @DepartmentIdList --部門
    /*@Created
        AND pi.preparation_datetime IS NOT NULL
    @Created*/
    /*@NotYet
        AND pi.inventory_id IS NULL 
    @NotYet*/
    /*@InventoryIdList
        AND pi.inventory_id IN @InventoryIdList
    @InventoryIdList*/
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
            1940
            , 1730
            , 1760
            , 1770
            , 1150
        ) 
        AND language_id = @LanguageId
) 