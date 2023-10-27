WITH history_data AS( -- 今回の受払情報取得
    SELECT
        parts.parts_id,
        lot.lot_control_id,
        history.inout_division_structure_id,
        history.inventory_control_id,
        history.inout_quantity,
        lot.unit_structure_id,
        lot.unit_price,
        lot.currency_structure_id,
        ex.extension_data
    FROM
        pt_inout_history history
        LEFT JOIN
            pt_lot lot
        ON  history.lot_control_id = lot.lot_control_id
        LEFT JOIN
            pt_parts parts
        ON  lot.parts_id = parts.parts_id
        LEFT JOIN
            ms_structure ms
        ON  history.inout_division_structure_id = ms.structure_id
        LEFT JOIN
            ms_item_extension ex
        ON  ms.structure_item_id = ex.item_id
        AND ex.sequence_no = 1
    WHERE
        history.delete_flg = 0
    AND parts.factory_id = @FactoryId -- 一覧で選択されたレコードの工場
    AND COALESCE(parts.job_structure_id, 0) = COALESCE(@PartsJobId, 0) -- 一覧で選択されたレコードの職種
    --受払日時
    /*@InoutDatetimeFrom
    AND history.inout_datetime >= @InoutDatetimeFrom
    @InoutDatetimeFrom*/
    AND history.inout_datetime < @InoutDatetimeTo
),
data_inout AS( --ロット管理ID・在庫管理ID・受払区分ごとに集計
    SELECT
        data.lot_control_id,
        data.inventory_control_id,
        data.extension_data,
        SUM(data.inout_quantity) AS quantity_in,
        SUM(data.inout_quantity * data.unit_price) AS price_in,
        0 AS quantity_out,
        0 AS price_out
    FROM
        history_data data
    WHERE
        data.extension_data = '1'
    GROUP BY
        data.lot_control_id,
        data.inventory_control_id,
        data.extension_data
    UNION ALL
    SELECT
        data.lot_control_id,
        data.inventory_control_id,
        data.extension_data,
        0 AS quantity_in,
        0 AS price_in,
        SUM(data.inout_quantity) AS quantity_out,
        SUM(data.inout_quantity * data.unit_price) AS price_out
    FROM
        history_data data
    WHERE
        data.extension_data = '2'
    GROUP BY
        data.lot_control_id,
        data.inventory_control_id,
        data.extension_data
),
data_inout_summary AS( --ロット管理ID・在庫管理IDごとに集計
    SELECT
        data_inout.lot_control_id,
        data_inout.inventory_control_id,
        SUM(data_inout.quantity_in) AS quantity_in,
        SUM(data_inout.price_in) AS price_in,
        SUM(data_inout.quantity_out) AS quantity_out,
        SUM(data_inout.price_out) AS price_out
    FROM
        data_inout
    GROUP BY
        data_inout.lot_control_id,
        data_inout.inventory_control_id
),
max_target_month AS( -- 確定在庫データより一覧で選択された対象年月未満の最大の対象年月のデータをロット管理ID・在庫管理IDごとに取得
    SELECT
        stock_c.lot_control_id,
        stock_c.inventory_control_id,
        stock_c.inventory_quantity,
        stock_c.inventory_amount
    FROM
        pt_fixed_stock stock_c
    WHERE
        EXISTS(
            SELECT
                *
            FROM
                (
                    SELECT
                        stock_a.lot_control_id,
                        stock_a.inventory_control_id,
                        MAX(stock_a.target_month) AS target_month
                    FROM
                        pt_fixed_stock stock_a
                    WHERE
                        format(stock_a.target_month, 'yyyy/MM') = @LastConfirmedDate
                    GROUP BY
                        stock_a.lot_control_id,
                        stock_a.inventory_control_id
                ) AS stock_b
            WHERE
                stock_c.target_month = stock_b.target_month
            AND stock_c.lot_control_id = stock_b.lot_control_id
            AND stock_c.inventory_control_id = stock_b.inventory_control_id
        )
)
SELECT DISTINCT
    history_data.parts_id,                                                          -- 予備品ID
    history_data.lot_control_id,                                                    -- ロット管理ID
    history_data.inventory_control_id,                                              -- 在庫管理ID
    history_data.unit_structure_id,                                                 -- 数量単位
    history_data.unit_price,                                                        -- 入庫単価
    history_data.currency_structure_id,                                             -- 金額単位
    data_inout_summary.quantity_in AS storage_quantity,                             -- 入庫数
    data_inout_summary.price_in AS storage_amount,                                  -- 入庫金額
    data_inout_summary.quantity_out AS shipping_quantity,                           -- 出庫数
    data_inout_summary.price_out AS shipping_amount,                                -- 出庫金額
    coalesce(max_target_month.inventory_quantity, 0) + data_inout_summary.quantity_in - data_inout_summary.quantity_out AS inventory_quantity, -- 末在庫数
    coalesce(max_target_month.inventory_amount, 0) + data_inout_summary.price_in - data_inout_summary.price_out AS inventory_amount            -- 末在庫金額
FROM
    history_data
    LEFT JOIN
        data_inout_summary
    ON  history_data.lot_control_id = data_inout_summary.lot_control_id
    AND history_data.inventory_control_id = data_inout_summary.inventory_control_id
    LEFT JOIN
        max_target_month
    ON  history_data.lot_control_id = max_target_month.lot_control_id
    AND history_data.inventory_control_id = max_target_month.inventory_control_id
