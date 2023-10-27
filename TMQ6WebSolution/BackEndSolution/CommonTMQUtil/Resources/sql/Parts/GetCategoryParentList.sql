--共通のWITH句の続きなので「,」から開始
,
total_stock AS( -- 新旧区分・部門・勘定科目 ごとの在庫数
    SELECT
        lot.old_new_structure_id,
        lot.department_structure_id,
        lot.account_structure_id,
        COALESCE(SUM(stock.stock_quantity), 0) AS stock_quantity
    FROM
        pt_lot lot
        LEFT JOIN
            pt_location_stock stock
        ON  lot.lot_control_id = stock.lot_control_id
        AND lot.parts_id = stock.parts_id
    WHERE
        lot.parts_id = @PartsId
    GROUP BY
        lot.old_new_structure_id,
        lot.department_structure_id,
        lot.account_structure_id
),
amount AS( -- 新旧区分・部門・勘定科目 ごとの在庫金額
    SELECT
        amount.old_new_structure_id,
        amount.department_structure_id,
        amount.account_structure_id,
        COALESCE(SUM(amount.stock_quantity), 0) AS stock_amount
    FROM
        (
            SELECT
                lot.old_new_structure_id,
                lot.department_structure_id,
                lot.account_structure_id,
                lot.unit_price * stock.stock_quantity AS stock_quantity
            FROM
                pt_lot lot
                LEFT JOIN
                    pt_location_stock stock
                ON  lot.lot_control_id = stock.lot_control_id
                AND lot.parts_id = stock.parts_id
            WHERE
                lot.parts_id = @PartsId
        ) amount
    GROUP BY
        amount.old_new_structure_id,
        amount.department_structure_id,
        amount.account_structure_id
)

SELECT *
FROM
(
SELECT DISTINCT
    lot.old_new_structure_id,                                                                                                                                     -- 新旧区分
    lot.management_division,                                                                                                                                      -- 管理区分
    lot.management_no,                                                                                                                                            -- 管理No.
    total_stock.stock_quantity,                                                                                                                                   -- 在庫数
    amount.stock_amount,                                                                                                                                          -- 在庫金額
    CAST(lot.old_new_structure_id AS varchar) + '_' + CAST(lot.department_structure_id AS varchar) + '_' + CAST(lot.account_structure_id AS varchar) AS nest_key, -- 入れ子キー
    COALESCE(unit_digit.extension_data, 0) AS unit_digit,                                                                                                         -- 小数点以下桁数(数量)
    COALESCE(currency_digit.currency_digit, 0) AS currency_digit,                                                                                                 -- 小数点以下桁数(金額)
    COALESCE(round_division.extension_data, 1) AS unit_round_division,                                                                                            -- 丸め処理区分(数量)
    COALESCE(round_division.extension_data, 1) AS currency_round_division,                                                                                        -- 丸め処理区分(金額)
    parts.factory_id,                                                                                                                                             -- 工場ID(結合文字列取得用)
    lot.department_structure_id,                                                                                                                                  -- 部門ID
    lot.account_structure_id,                                                                                                                                     -- 勘定科目ID
             ---------------------------------- 以下は翻訳を取得 ----------------------------------
    department.department_code + ' ' + department_trans.department_name AS department_nm,                                                                         -- 部門
    subject.subject_code + ' ' +
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
                  st_f.structure_id = lot.account_structure_id
              AND st_f.factory_id IN(0, @UserFactoryId)
           )
      AND tra.structure_id = lot.account_structure_id
    ) AS subject_nm,                                                                                                                                               -- 勘定科目
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
                  st_f.structure_id = lot.unit_structure_id
              AND st_f.factory_id IN(0,@UserFactoryId)
           )
      AND tra.structure_id = lot.unit_structure_id
    ) AS unit_name,                                                                                                                                               -- 数量管理単位(名称)
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
                  st_f.structure_id = lot.currency_structure_id
              AND st_f.factory_id IN(0, @UserFactoryId)
           )
      AND tra.structure_id = lot.currency_structure_id
    ) AS currency_name                                                                                                                                            -- 金額管理単位(名称)
FROM
    pt_lot lot
    LEFT JOIN
        total_stock -- 在庫数
    ON  lot.old_new_structure_id = total_stock.old_new_structure_id
    AND lot.department_structure_id = total_stock.department_structure_id
    AND lot.account_structure_id = total_stock.account_structure_id
    LEFT JOIN
        amount -- 在庫金額
    ON  lot.old_new_structure_id = amount.old_new_structure_id
    AND lot.department_structure_id = amount.department_structure_id
    AND lot.account_structure_id = amount.account_structure_id
    LEFT JOIN
        department -- 部門
    ON  lot.department_structure_id = department.department_id
    LEFT JOIN
        subject -- 勘定科目
    ON  lot.account_structure_id = subject.subject_id
    LEFT JOIN
        pt_parts parts
    ON  lot.parts_id = parts.parts_id
    LEFT JOIN
        unit_digit -- 数量管理単位
    ON  parts.unit_structure_id = unit_digit.structure_id
    LEFT JOIN
        currency_digit -- 金額管理単位
    ON  parts.currency_structure_id = currency_digit.currency_id
    LEFT JOIN
        round_division --丸め処理区分
    ON  parts.factory_id = round_division.factory_id
    LEFT JOIN -- 部門名称
        department_trans
    ON  lot.department_structure_id = department_trans.department_id
WHERE
    lot.parts_id = @PartsId -- 在庫があるものを抽出
AND total_stock.stock_quantity > 0
) tbl
ORDER BY
    department_structure_id,
    old_new_structure_id,
    account_structure_id