SELECT
    relocation_date,
    unit_price,
    SUM(transfer_count) AS transfer_count,
    SUM(transfer_amount) AS transfer_amount,
    department_id,
    department_code,
    subject_id,
    subject_code,
    management_division,
    management_no,
    unit_name,
    currency_name,
    unit_digit,
    currency_digit,
    unit_round_division,
    currency_round_division,
    department_flg,
    lot_control_id,
    old_new_structure_id,
    parts_factory_id,
    update_serialid AS update_serialid
FROM
    (
        SELECT
            history.inout_datetime AS relocation_date,                                                                 -- 移庫日
            lot.unit_price,                                                                                            -- 入庫単価(移庫単価)
            history.inout_quantity AS transfer_count,                                                                  -- 移庫数
            lot.unit_price * history.inout_quantity AS transfer_amount,                                                -- 移庫金額
            lot.department_structure_id AS department_id,                                                              -- 部門ID
            department.department_code,                                                                                -- 部門コード
            lot.account_structure_id AS subject_id,                                                                    -- 勘定科目ID
            subject.subject_code,                                                                                      -- 勘定科目コード 
            lot.management_division,                                                                                   -- 管理区分
            lot.management_no,                                                                                         -- 管理No
            COALESCE(unit_digit.extension_data, 0) AS unit_digit,                                                      -- 小数点以下桁数(数量)
            COALESCE(currency_digit.currency_digit, 0) AS currency_digit,                                              -- 小数点以下桁数(金額)
            COALESCE(round_division.extension_data, 1) AS unit_round_division,                                         -- 丸め処理区分(数量)
            COALESCE(round_division.extension_data, 1) AS currency_round_division,                                     -- 丸め処理区分(金額)
            (
            SELECT
                COALESCE(ex.extension_data, '0') AS department_flg
            FROM
                (
                 SELECT DISTINCT
                     history.department_structure_id
                 FROM
                     pt_inout_history history
                 LEFT JOIN
                     inout_div -- 受払区分
                 ON  history.inout_division_structure_id = inout_div.inout_id
                 LEFT JOIN work_div -- 作業区分
                 ON  history.work_division_structure_id = work_div.work_id
            WHERE
                 history.work_no = @WorkNo
            -- 「払出」のデータ
            AND inout_div.inout_code = '2'
            -- 「部門移庫」のデータ
            AND work_div.work_code = '4'
            ) tbl
            LEFT JOIN
                ms_structure ms
            ON  tbl.department_structure_id = ms.structure_id
            AND ms.structure_group_id = 1760
            LEFT JOIN
                ms_item_extension ex
            ON  ms.structure_item_id = ex.item_id
            AND ex.sequence_no = 2
            ) AS department_flg,                                                                                       -- 部門フラグ
            history.lot_control_id,                                                                                    -- ロット管理ID
            lot.old_new_structure_id,                                                                                  -- 新旧区分
            parts.factory_id AS parts_factory_id,                                                                      -- 管理工場ID
            update_serialid.update_serialid,                                                                           -- 更新シリアルID
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
                  st_f.structure_id = lot.unit_structure_id
              AND st_f.factory_id IN(0,@UserFactoryId)
           )
      AND tra.structure_id = lot.unit_structure_id
    ) AS unit_name,                                                                                                     -- 数量管理単位(名称)   
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
    ) AS currency_name                                                                                                  -- 金額管理単位(名称)

        FROM
            pt_inout_history history
            LEFT JOIN
                pt_lot lot
            -- ロット情報
        ON  history.lot_control_id = lot.lot_control_id
        AND lot.delete_flg = 0
        LEFT JOIN
            pt_parts parts -- 予備品仕様
        ON  lot.parts_id = parts.parts_id
        LEFT JOIN
            inout_div -- 受払区分
        ON  history.inout_division_structure_id = inout_div.inout_id
        LEFT JOIN
            work_div -- 作業区分
        ON  history.work_division_structure_id = work_div.work_id
        LEFT JOIN
            unit_digit -- 小数点以下桁数(数量)
        ON  lot.unit_structure_id = unit_digit.structure_id
        LEFT JOIN
            currency_digit -- 小数点以下桁数(金額)
        ON  lot.currency_structure_id = currency_digit.currency_id
        LEFT JOIN
            round_division -- 丸め処理区分
        ON  parts.factory_id = round_division.factory_id
        LEFT JOIN
            department -- 部門
        ON  lot.department_structure_id = department.department_id
        LEFT JOIN
            subject -- 勘定科目
        ON  lot.account_structure_id = subject.subject_id
        LEFT JOIN
    (
       SELECT
           pih.work_no,
           MAX(pih.update_serialid) AS update_serialid
       FROM
           pt_inout_history pih
       WHERE
           pih.work_no = @WorkNo
       GROUP BY
           pih.work_no
    ) update_serialid
ON  history.work_no = update_serialid.work_no
WHERE
    history.work_no = @WorkNo
     -- 「受入」のデータ
    AND inout_div.inout_code = '1'
    -- 「部門移庫」のデータ
    AND work_div.work_code = '4'
) tbl
GROUP BY
    relocation_date,
    unit_price,
    department_id,
    department_code,
    subject_id,
    subject_code,
    management_division,
    management_no,
    unit_name,
    currency_name,
    unit_digit,
    currency_digit,
    unit_round_division,
    currency_round_division,
    department_flg,
    lot_control_id,
    old_new_structure_id,
    parts_factory_id,
    update_serialid