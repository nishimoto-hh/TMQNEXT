SELECT
    tbl.data_order,                                                                                          -- 並び順
    tbl.work_division_structure_id,                                                                          -- 入出庫区分
    tbl.date,                                                                                                -- 日付
    tbl.lot_no,                                                                                              -- 入庫No.
    tbl.old_new_structure_id,                                                                                -- 新旧区分
    tbl.inout_quantity,                                                                                      -- 入庫数
    tbl.unit_price,                                                                                          -- 入庫単価
    tbl.storage_amount,                                                                                      -- 入庫金額
    tbl.shipping_quantity,                                                                                   -- 出庫数
    tbl.shipping_amount,                                                                                     -- 出庫金額
    COALESCE(unit_digit.extension_data, 0) AS unit_digit,                                                    -- 小数点以下桁数(数量)
    COALESCE(currency_digit.currency_digit, 0) AS currency_digit,                                            -- 小数点以下桁数(金額)
    COALESCE(round_division.extension_data, 1) AS unit_round_division,                                       -- 丸め処理区分(数量)
    COALESCE(round_division.extension_data, 1) AS currency_round_division,                                   -- 丸め処理区分(金額)
    tbl.inout_quantity_calc,                                                                                 -- 入庫数(在庫数計算用)
    tbl.shipping_quantity_calc,                                                                              -- 出庫数(在庫数計算用)
    tbl.storage_amount_calc,                                                                                 -- 入庫金額(在庫金額計算用)
    tbl.shipping_amount_calc,                                                                                -- 出庫金額(在庫金額計算用)
    tbl.stock_quantity,                                                                                      -- 在庫数
    tbl.stock_amount,                                                                                        -- 在庫金額
    tbl.Flg,                                                                                                 -- 在庫数・金額計算用フラグ
    tbl.inout_history_id,                                                                                    -- 受払履歴ID
    tbl.work_no,                                                                                             -- 作業No
    ---------------------------------- 以下は翻訳を取得 ----------------------------------
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
                  structure_factory AS st_f
              WHERE
                  st_f.structure_id = tbl.unit_structure_id
              AND st_f.factory_id IN(0,@UserFactoryId)
           )
      AND tra.structure_id = tbl.unit_structure_id
    ) AS unit_name,                                                                                           -- 数量管理単位(名称)   
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
                  structure_factory AS st_f
              WHERE
                  st_f.structure_id = tbl.currency_structure_id
              AND st_f.factory_id IN(0, @UserFactoryId)
           )
      AND tra.structure_id = tbl.currency_structure_id
    ) AS currency_name                                                                                        -- 金額管理単位(名称)
FROM
    (
        /*******************繰越のデータ*******************/
        SELECT
            0 AS data_order,
            (
                -- 「繰越」の構成ID取得
                SELECT
                    ms.structure_id
                FROM
                    ms_structure ms
                    LEFT JOIN
                        ms_item_extension ex
                    ON  ms.structure_item_id = ex.item_id
                    AND ex.sequence_no = 1
                WHERE
                    ms.structure_group_id = 1960
                AND ex.extension_data = 0
            ) AS work_division_structure_id,                             -- 入出庫区分
            EOMONTH(CONVERT(datetime, new_data.target_month)) AS date,   -- 日付
            NULL AS lot_no,                                              -- 入庫No.
            - 1 AS old_new_structure_id,                                 -- 新旧区分
            cast(new_data.inout_quantity AS varchar) AS inout_quantity,  -- 入庫数
            '' AS unit_price,                                            -- 入庫単価
            cast(new_data.storage_amount AS varchar) AS storage_amount,  -- 入庫金額
            '' AS shipping_quantity,                                     -- 出庫数
            '' AS shipping_amount,                                       -- 出庫金額
            COALESCE(new_data.inout_quantity, 0) AS inout_quantity_calc, -- 入庫数(在庫数計算用)
            0 AS shipping_quantity_calc,                                 -- 出庫数(在庫数計算用)
            COALESCE(new_data.storage_amount, 0) AS storage_amount_calc, -- 入庫金額(在庫金額計算用)
            0 AS shipping_amount_calc,                                   -- 出庫金額(在庫金額計算用)
            cast(new_data.stock_quantity AS varchar) AS stock_quantity,  -- 在庫数
            cast(new_data.stock_amount AS varchar) AS stock_amount,      -- 在庫金額
            '0' AS Flg,                                                  -- 在庫数・金額計算用フラグ
            0 AS inout_history_id,                                       -- 受払履歴ID
            parts.parts_location_id,                                     -- 棚番
            parts.unit_structure_id,                                     -- 数量管理単位
            parts.currency_structure_id,                                 -- 金額管理単位
            0 AS work_no,                                                -- 作業No
            parts.factory_id                                             -- 管理工場ID
        FROM
            (
                SELECT
                    fix.parts_id,
                    fix.target_month,
                    SUM(fix.inventory_quantity) AS inout_quantity, -- 入庫数
                    SUM(fix.inventory_amount) AS storage_amount,   -- 入庫金額
                    SUM(fix.inventory_quantity) AS stock_quantity, -- 在庫数
                    SUM(fix.inventory_amount) AS stock_amount      -- 在庫金額
                FROM
                    pt_fixed_stock fix
                    LEFT JOIN
                        (
                            SELECT
                                ROW_NUMBER() OVER(ORDER BY fix.target_month DESC) AS order_num,
                                fix.target_month
                            FROM
                                pt_fixed_stock fix
                                WHERE
                                   fix.target_month < @DispYearFrom
                                AND fix.parts_id = @PartsId
                        ) new_date
                    ON  fix.target_month = new_date.target_month
                WHERE
                    new_date.order_num = 1
                AND fix.parts_id = @PartsId
                GROUP BY
                    fix.parts_id,
                    fix.target_month
            ) new_data
            LEFT JOIN
                pt_parts parts
            ON  new_data.parts_id = parts.parts_id
            /*******************入出庫履歴のデータ*******************/
        UNION ALL
        SELECT
            1 data_order,
            history.work_division_structure_id,                                                                          -- 入出庫区分
            history.inout_datetime AS date,                                                                              -- 日付
            lot.lot_no,                                                                                                  -- 入庫No.
            lot.old_new_structure_id,                                                                                    -- 新旧区分
            CASE
                WHEN inout_div.inout_code = 1 THEN cast(COALESCE(history.inout_quantity, 0) AS varchar)
                ELSE ''
            END AS inout_quantity,                                                                                       -- 入庫数
            CASE
                WHEN inout_div.inout_code = 1 THEN cast(COALESCE(lot.unit_price, 0) AS varchar)
                ELSE ''
            END AS unit_price,                                                                                           -- 入庫単価
            CASE
                WHEN inout_div.inout_code = 1 THEN cast(COALESCE(history.inout_quantity * lot.unit_price, 0) AS varchar)
                ELSE ''
            END AS storage_amount,                                                                                       -- 入庫金額
            CASE
                WHEN inout_div.inout_code = 2 THEN cast(COALESCE(history.inout_quantity, 0) AS varchar)
                ELSE ''
            END AS shipping_quantity,                                                                                    -- 出庫数
            CASE
                WHEN inout_div.inout_code = 2 THEN cast(COALESCE(history.inout_quantity * lot.unit_price, 0) AS varchar)
                ELSE ''
            END AS shipping_amount,                                                                                      -- 出庫金額
            CASE
                WHEN inout_div.inout_code = 1 THEN COALESCE(history.inout_quantity, 0)
                ELSE 0
            END AS inout_quantity_calc,                                                                                  -- 入庫数(在庫数計算用)
            CASE
                WHEN inout_div.inout_code = 2 THEN COALESCE(history.inout_quantity, 0)
                ELSE 0
            END AS shipping_quantity_calc,                                                                               -- 出庫数(在庫数計算用)
            CASE
                WHEN inout_div.inout_code = 1 THEN COALESCE(history.inout_quantity * lot.unit_price, 0)
                ELSE 0
            END AS storage_amount_calc,                                                                                  -- 入庫金額(在庫金額計算用)
            CASE
                WHEN inout_div.inout_code = 2 THEN COALESCE(history.inout_quantity * lot.unit_price, 0)
                ELSE 0
            END AS shipping_amount_calc,                                                                                 -- 出庫金額(在庫金額計算用)
            '0' AS stock_quantity,                                                                                       -- 在庫数
            '0' AS stock_amount,                                                                                         -- 在庫金額
            '1' AS Flg,                                                                                                  -- 在庫数・金額計算用フラグ
            history.inout_history_id,                                                                                    -- 受払履歴ID
            parts.parts_location_id,                                                                                     -- 棚番
            parts.unit_structure_id,                                                                                     -- 数量管理単位
            parts.currency_structure_id,                                                                                 -- 金額管理単位
            history.work_no,                                                                                             -- 作業No
            parts.factory_id                                                                                             -- 管理工場ID
        FROM
            pt_inout_history history
        LEFT JOIN
            pt_lot lot
        ON  history.lot_control_id = lot.lot_control_id
        LEFT JOIN
            inout_div
        ON  history.inout_division_structure_id = inout_div.inout_id
        LEFT JOIN
            pt_parts parts
        ON  lot.parts_id = parts.parts_id
        WHERE
            lot.parts_id = @PartsId
        AND history.inout_datetime BETWEEN @DispYearFrom AND @DispYearTo
        AND history.delete_flg <> 1
    ) tbl
    LEFT JOIN
        unit_digit -- 数量管理単位
    ON  tbl.unit_structure_id = unit_digit.structure_id
    LEFT JOIN
        currency_digit -- 金額管理単位
    ON  tbl.currency_structure_id = currency_digit.currency_id
    LEFT JOIN
        round_division --丸め処理区分
    ON  tbl.factory_id = round_division.factory_id
ORDER BY
    data_order,
    date,
    work_no