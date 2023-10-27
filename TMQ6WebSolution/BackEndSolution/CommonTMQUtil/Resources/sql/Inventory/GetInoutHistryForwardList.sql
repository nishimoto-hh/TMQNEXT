--******************************************************************
--入出庫履歴一覧(繰越)
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
, translate_target AS(
    SELECT
        0 AS work_division_structure_id                                                                 --入出庫区分(繰越)
        , EOMONTH(pfs.insert_datetime) AS date                                                          --日付
        , null AS inout_no                                                                              --入出庫No
        , SUM(pfs.inventory_quantity) AS inventory_quantity                                             --入庫数
        , pfs.unit_structure_id                                                                         --数量管理単位ID
        , pfs.unit_price                                                                                --入庫単価
        , pfs.currency_structure_id                                                                     --金額管理単位ID
        , SUM(pfs.inventory_amount) AS inventory_amount                                                 --入庫金額
        , null AS issue_quantity                                                                        --出庫数
        , null AS issue_amount                                                                          --出庫金額
        , SUM(pfs.inventory_quantity) AS stock_quantity                                                 --在庫数
        , SUM(pfs.inventory_amount) AS stock_amount                                                     --在庫金額
        , 0 AS transition_division                                                                      --遷移区分
        , COALESCE(number_unit.unit_digit, 0) AS unit_digit                                             -- 小数点以下桁数(数量)
        , COALESCE(currency_unit.currency_digit, 0) AS currency_digit                                   -- 小数点以下桁数(金額)
        , COALESCE(unit_round.round_division, 0) AS unit_round_division                                 --丸め処理区分(数量)
        , COALESCE(unit_round.round_division, 0) AS currency_round_division                             --丸め処理区分(金額)
        , pfs.target_month                                                                              --対象年月
        , pps.factory_id                                                                                --工場ID
    FROM
        pt_fixed_stock pfs 
        LEFT JOIN pt_lot plt 
            ON pfs.lot_control_id = plt.lot_control_id 
            AND pfs.parts_id = plt.parts_id 
        LEFT JOIN pt_location_stock pls 
            ON plt.lot_control_id = pls.lot_control_id 
            AND pfs.inventory_control_id = pls.inventory_control_id 
        LEFT JOIN pt_parts pps
            ON pfs.parts_id = pps.parts_id 
            AND plt.parts_id = pps.parts_id 
        LEFT JOIN number_unit --数量管理単位
            ON  pfs.unit_structure_id = number_unit.unit_id
        LEFT JOIN unit_round --丸め処理区分
            ON  pps.factory_id = unit_round.factory_id
        LEFT JOIN currency_unit --金額管理単位
            ON  pfs.currency_structure_id = currency_unit.currency_id
    WHERE
        pfs.parts_id = @PartsId                                         --予備品ID
        AND plt.old_new_structure_id = @OldNewStructureId               --新旧区分
        AND plt.department_structure_id = @DepartmentStructureId        --部門
        AND plt.account_structure_id = @AccountStructureId              --勘定科目
        AND pls.parts_location_id = @PartsLocationId                    --棚ID
        AND COALESCE(pls.parts_location_detail_no, '') = @PartsLocationDetailNo       --棚枝番
        AND pfs.target_month = CASE @Status 
            WHEN 0 THEN ( 
                --準備リスト未作成
                SELECT  DISTINCT
                    target_month 
                FROM
                    pt_fixed_stock 
                WHERE
                    target_month = ( 
                        SELECT
                            MAX(target_month) 
                        FROM
                            pt_fixed_stock 
                        WHERE
                            target_month < GETDATE()
                        AND parts_id = @PartsId
                    )
            ) 
            WHEN 1 THEN ( 
                --準備リスト作成
                SELECT  DISTINCT
                    target_month 
                FROM
                    pt_fixed_stock 
                WHERE
                    target_month = ( 
                        SELECT
                            MAX(pfs.target_month) 
                        FROM
                            pt_fixed_stock pfs 
                            LEFT JOIN pt_inventory pit 
                                ON pfs.parts_id = pit.parts_id 
                        WHERE
                            pfs.target_month < pit.preparation_datetime
                        AND pfs.parts_id = @PartsId
                    )
            ) 
            WHEN 2 THEN ( 
                --棚卸確定時
                SELECT  DISTINCT
                    target_month 
                FROM
                    pt_fixed_stock 
                WHERE
                    target_month = ( 
                        SELECT
                            MAX(pfs.target_month) 
                        FROM
                            pt_fixed_stock pfs 
                            LEFT JOIN pt_inventory pit 
                                ON pfs.parts_id = pit.parts_id 
                        WHERE
                            pfs.target_month < pit.fixed_datetime
                        AND pfs.parts_id = @PartsId
                    )
            ) 
            END 
    GROUP BY
        pfs.insert_datetime
        , pfs.parts_id
        , plt.old_new_structure_id
        , plt.department_structure_id
        , plt.account_structure_id
        , pfs.unit_structure_id
        , pfs.unit_price
        , pfs.currency_structure_id
        , number_unit.unit_digit
        , currency_unit.currency_digit
        , unit_round.round_division
        , pfs.target_month
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
        ) 
        AND language_id = @LanguageId
) 
SELECT
    *
    , ( 
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
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = target.unit_structure_id 
                    AND st_f.factory_id IN (0, target.factory_id)
            ) 
            AND tra.structure_id = target.unit_structure_id
    ) AS unit_name  --数量単位名称(翻訳)
    , ( 
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
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = target.currency_structure_id 
                    AND st_f.factory_id IN (0, target.factory_id)
            ) 
            AND tra.structure_id = target.currency_structure_id
    ) AS currency_name  --金額単位名称(翻訳)
FROM
    translate_target AS target 