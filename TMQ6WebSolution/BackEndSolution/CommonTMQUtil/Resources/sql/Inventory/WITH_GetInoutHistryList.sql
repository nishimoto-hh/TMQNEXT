--******************************************************************
--入出庫履歴一覧(WITH句)
--******************************************************************
WITH inout_division AS ( 
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
, work_division AS ( 
    --ビューより作業区分を取得
    SELECT
        structure_id
        , ie.extension_data
    FROM
        ms_structure AS ms 
        INNER JOIN ms_item_extension AS ie 
            ON ms.structure_item_id = ie.item_id 
            AND ms.structure_group_id = 1960 
            AND ie.sequence_no = 1
) 
, warehousing AS ( 
    SELECT
        pih.work_no
        , SUM(pih.inout_quantity) AS in_quantity --入庫数
        , SUM(plt.unit_price) AS unit_price --入庫単価
        , SUM(pih.inout_quantity * plt.unit_price) AS inventory_amount --入庫金額
    FROM
        pt_inout_history pih 
        LEFT JOIN pt_lot plt 
            ON pih.lot_control_id = plt.lot_control_id 
        LEFT JOIN inout_division ids 
            ON pih.inout_division_structure_id = ids.structure_id 
    WHERE
        ids.extension_data = '1'                --受入
        AND plt.parts_id = @PartsId                           --予備品ID
        AND plt.old_new_structure_id = @OldNewStructureId --新旧区分ID
        AND plt.department_structure_id = @DepartmentStructureId --部門ID
        AND plt.account_structure_id = @AccountStructureId --勘定科目ID
    GROUP BY
        pih.work_no
        , plt.unit_structure_id
        , plt.currency_structure_id
) 
, issue AS ( 
    SELECT
        pih.inout_history_id
        , SUM(pih.inout_quantity) AS issue_quantity --出庫数
        , SUM(pih.inout_quantity * plt.unit_price) AS issue_amount   --出庫金額
    FROM
        pt_inout_history pih 
        LEFT JOIN pt_lot plt 
            ON pih.lot_control_id = plt.lot_control_id 
        LEFT JOIN inout_division ids 
            ON pih.inout_division_structure_id = ids.structure_id 
    WHERE
        ids.extension_data = '2'                --払出
        AND plt.parts_id = @PartsId                           --予備品ID
        AND plt.old_new_structure_id = @OldNewStructureId --新旧区分ID
        AND plt.department_structure_id = @DepartmentStructureId --部門ID
        AND plt.account_structure_id = @AccountStructureId --勘定科目ID
    GROUP BY
        pih.inout_history_id
        , plt.unit_structure_id
        , plt.currency_structure_id
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
    --入出庫履歴一覧
    --******************************************************************
         /*******************受払区分が受入のデータ*******************/
    SELECT DISTINCT
        1 AS enter                                  --遷移リンク
        , 2 AS issue                                --遷移リンク
        , 3 AS shed                                 --遷移リンク
        , 4 AS category                             --遷移リンク
        , pih.work_division_structure_id            --作業区分
        , pih.inout_datetime AS date                --受払日時
        , plt.lot_no AS inout_no                                 --入出庫No
        , COALESCE(whi.in_quantity, 0) AS inventory_quantity     --入庫数
        , plt.unit_structure_id                                  --数量管理単位ID
        , COALESCE(whi.unit_price, 0) AS unit_price              --入庫単価
        , plt.currency_structure_id                              --金額管理単位ID
        , COALESCE(whi.inventory_amount, 0) AS inventory_amount  --入庫金額
        , null AS issue_quantity                                 --出庫数
        , null AS issue_amount                                   --出庫金額
        , null AS stock_quantity                                 --在庫数
        , null AS stock_amount                                   --在庫金額
        , CONVERT(int,wds.extension_data) AS transition_division --遷移区分
        , COALESCE(number_unit.unit_digit, 0) AS unit_digit                              -- 小数点以下桁数(数量)
        , COALESCE(currency_unit.currency_digit, 0) AS currency_digit                    -- 小数点以下桁数(金額)
        , COALESCE(unit_round.round_division, 0) AS unit_round_division                   --丸め処理区分(数量)
        , COALESCE(unit_round.round_division, 0) AS currency_round_division               --丸め処理区分(金額)
        , pih.inout_history_id
        , '1' AS control_flag -- 制御用フラグ
        , pih.work_no --作業No
        , pps.factory_id      --工場ID
    FROM
        pt_inout_history pih 
        LEFT JOIN pt_lot plt 
            ON pih.lot_control_id = plt.lot_control_id 
        LEFT JOIN pt_location_stock pls 
            ON plt.lot_control_id = pls.lot_control_id 
            AND pih.inventory_control_id = pls.inventory_control_id
        LEFT JOIN pt_parts pps
            ON plt.parts_id = pps.parts_id 
            AND pls.parts_id = pps.parts_id 
        LEFT JOIN warehousing whi 
            ON pih.work_no = whi.work_no 
        LEFT JOIN inout_division ids 
            ON pih.inout_division_structure_id = ids.structure_id 
        LEFT JOIN work_division wds
            ON pih.work_division_structure_id = wds.structure_id
        LEFT JOIN number_unit --数量管理単位
            ON  plt.unit_structure_id = number_unit.unit_id
        LEFT JOIN unit_round --丸め処理区分
            ON  pps.factory_id = unit_round.factory_id
        LEFT JOIN currency_unit --金額管理単位
            ON  plt.currency_structure_id = currency_unit.currency_id
    WHERE
        pih.delete_flg = 0
        AND plt.lot_control_id IN ( 
            SELECT DISTINCT
                lot_control_id 
            FROM
                pt_lot 
            WHERE
                parts_id = @PartsId                 --予備品ID
                AND old_new_structure_id = @OldNewStructureId --新旧区分ID
                AND department_structure_id = @DepartmentStructureId --部門ID
                AND account_structure_id = @AccountStructureId --勘定科目ID
        ) 
        AND ids.extension_data = '1'                --受入
        AND pls.parts_location_id = @PartsLocationId --棚ID
        AND COALESCE(pls.parts_location_detail_no, '') = @PartsLocationDetailNo --棚枝番
                AND ((
                @Status = 0
                AND pih.inout_datetime BETWEEN dateadd(day, 1, @CleateDate) AND @MonthYear
            )                                           --準備リスト未作成の場合は繰越データ作成日の翌日～棚卸画面の対象年月の末日まで
            OR (
                @Status = 1                             --準備リスト作成済の場合は繰越データ作成日の翌日～棚卸画面の対象年月の末日までかつ更新日時が棚卸準備日時以前のデータを表示
                AND  pih.inout_datetime BETWEEN dateadd(day, 1, @CleateDate) AND @MonthYear
                AND pih.update_datetime <= (
                    SELECT
                        MAX(preparation_datetime) AS preparation_datetime
                    FROM
                        pt_inventory
                    WHERE
                        parts_id = @PartsId             --予備品ID
                        AND old_new_structure_id = @OldNewStructureId --新旧区分
                        AND department_structure_id = @DepartmentStructureId --部門
                        AND account_structure_id = @AccountStructureId --勘定科目
                        AND parts_location_id = @PartsLocationId --棚ID
                        AND COALESCE(parts_location_detail_no, '') = @PartsLocationDetailNo --棚枝番
                )
            )
            OR (
                @Status = 2                             --在庫確定済の場合は繰越データ作成日の翌日から棚卸画面の対象年月の翌日かつ更新日時が棚卸確定日時以前のデータを表示
                AND pih.inout_datetime BETWEEN dateadd(day, 1, @CleateDate) AND @MonthYear
                AND pih.update_datetime <= (
                    SELECT
                        MAX(fixed_datetime) AS fixed_datetime
                    FROM
                        pt_inventory
                    WHERE
                        parts_id = @PartsId             --予備品ID
                        AND old_new_structure_id = @OldNewStructureId --新旧区分
                        AND department_structure_id = @DepartmentStructureId --部門
                        AND account_structure_id = @AccountStructureId --勘定科目
                        AND parts_location_id = @PartsLocationId --棚ID
                        AND COALESCE(parts_location_detail_no, '') = @PartsLocationDetailNo --棚枝番
                )
            ))
        /*******************受払区分が払出のデータ*******************/
       UNION ALL 
    SELECT DISTINCT
        1 AS enter                                  --遷移リンク
        , 2 AS issue                                --遷移リンク
        , 3 AS shed                                 --遷移リンク
        , 4 AS category                             --遷移リンク
        , pih.work_division_structure_id             --作業区分
        , pih.inout_datetime AS date                --受払日時
        , pih.work_no AS inout_no                   --入出庫No
        , null AS inventory_quantity                --入庫数
        , plt.unit_structure_id                     --数量管理単位ID
        , null AS unit_price                        --入庫単価
        , plt.currency_structure_id                 --金額管理単位ID
        , null AS inventory_amount                  --入庫金額
        , COALESCE(ise.issue_quantity, 0) AS issue_quantity    --出庫数
        , COALESCE(ise.issue_amount, 0) AS issue_amount        --出庫金額
        , null AS stock_quantity                    --在庫数
        , null AS stock_amount                      --在庫金額
        , CONVERT(int,wds.extension_data) AS transition_division --遷移区分
        , COALESCE(number_unit.unit_digit, 0) AS unit_digit                                -- 小数点以下桁数(数量)
        , COALESCE(currency_unit.currency_digit, 0) AS currency_digit                      -- 小数点以下桁数(金額)
        , COALESCE(unit_round.round_division, 0) AS unit_round_division                    --丸め処理区分(数量)
        , COALESCE(unit_round.round_division, 0) AS currency_round_division                --丸め処理区分(金額)
        , pih.inout_history_id
        , '2' AS control_flag -- 制御用フラグ
        , pih.work_no         --作業No
        , pps.factory_id      --工場ID
    FROM
        pt_inout_history pih 
        LEFT JOIN pt_lot plt 
            ON pih.lot_control_id = plt.lot_control_id 
        LEFT JOIN pt_location_stock pls 
            ON plt.lot_control_id = pls.lot_control_id 
            AND pih.inventory_control_id = pls.inventory_control_id
        LEFT JOIN pt_parts pps
            ON plt.parts_id = pps.parts_id 
            AND pls.parts_id = pps.parts_id 
        LEFT JOIN issue ise 
            ON pih.inout_history_id = ise.inout_history_id 
        LEFT JOIN inout_division ids 
            ON pih.inout_division_structure_id = ids.structure_id 
        LEFT JOIN work_division wds
            ON pih.work_division_structure_id = wds.structure_id
        LEFT JOIN number_unit --数量管理単位
            ON  plt.unit_structure_id = number_unit.unit_id
        LEFT JOIN unit_round --丸め処理区分
            ON  pps.factory_id = unit_round.factory_id
        LEFT JOIN currency_unit --金額管理単位
            ON  plt.currency_structure_id = currency_unit.currency_id
    WHERE
        pih.delete_flg = 0
        AND plt.lot_control_id IN ( 
            SELECT DISTINCT
                lot_control_id 
            FROM
                pt_lot 
            WHERE
                parts_id = @PartsId                 --予備品ID
                AND old_new_structure_id = @OldNewStructureId --新旧区分ID
                AND department_structure_id = @DepartmentStructureId --部門ID
                AND account_structure_id = @AccountStructureId --勘定科目ID
        ) 
        AND ids.extension_data = '2'                --払出
        AND plt.parts_id = @PartsId                 --予備品ID
        AND pls.parts_location_id = @PartsLocationId --棚ID
        AND COALESCE(pls.parts_location_detail_no, '') = @PartsLocationDetailNo --棚枝番
                AND ((
                @Status = 0
                AND pih.inout_datetime BETWEEN dateadd(day, 1, @CleateDate) AND @MonthYear
            )                                           --準備リスト未作成の場合は繰越データ作成日の翌日以降～棚卸画面の対象年月の末日まで
            OR (
                @Status = 1                             --準備リスト作成済の場合は繰越データ作成日の翌日～棚卸画面の対象年月の末日までかつ更新日時が棚卸準備日時以前のデータを表示
                AND  pih.inout_datetime BETWEEN dateadd(day, 1, @CleateDate) AND @MonthYear
                AND pih.update_datetime <= (
                    SELECT
                        MAX(preparation_datetime) AS preparation_datetime
                    FROM
                        pt_inventory
                    WHERE
                        parts_id = @PartsId             --予備品ID
                        AND old_new_structure_id = @OldNewStructureId --新旧区分
                        AND department_structure_id = @DepartmentStructureId --部門
                        AND account_structure_id = @AccountStructureId --勘定科目
                        AND parts_location_id = @PartsLocationId --棚ID
                        AND COALESCE(parts_location_detail_no, '') = @PartsLocationDetailNo --棚枝番
                )
            )
            OR (
                @Status = 2                             --在庫確定済の場合は繰越データ作成日の翌日から棚卸画面の対象年月の翌日かつ更新日時が棚卸確定日時以前のデータを表示
                AND pih.inout_datetime BETWEEN dateadd(day, 1, @CleateDate) AND @MonthYear
                AND pih.update_datetime <= (
                    SELECT
                        MAX(fixed_datetime) AS fixed_datetime
                    FROM
                        pt_inventory
                    WHERE
                        parts_id = @PartsId             --予備品ID
                        AND old_new_structure_id = @OldNewStructureId --新旧区分
                        AND department_structure_id = @DepartmentStructureId --部門
                        AND account_structure_id = @AccountStructureId --勘定科目
                        AND parts_location_id = @PartsLocationId --棚ID
                        AND COALESCE(parts_location_detail_no, '') = @PartsLocationDetailNo --棚枝番
                )
            ))
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
            , 1960
        ) 
        AND language_id = @LanguageId
) 