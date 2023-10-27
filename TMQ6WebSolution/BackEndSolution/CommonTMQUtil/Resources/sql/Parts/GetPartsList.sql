SELECT
    parts.parts_no,                                                                                              -- 予備品No
    parts.parts_name,                                                                                            -- 予備品名
    parts.manufacturer_structure_id,                                                                             -- メーカー
    parts.model_type,                                                                                            -- 型式
    parts.materials,                                                                                             -- 材質
    parts.standard_size,                                                                                         -- 規格・寸法
    parts.factory_id,                                                                                            -- 管理工場
    parts.factory_id AS parts_factory_id,                                                                        -- 管理工場(ツリーの絞り込み用)
    parts.location_factory_structure_id AS default_factory_id,                                                   -- 棚番から取得した工場ID
    COALESCE(parts.job_structure_id, 0) AS job_structure_id,                                                     -- 職種
    parts.parts_service_space,                                                                                   -- 使用場所
    parts.parts_location_id,                                                                                     -- 標準棚番ID
    parts.parts_location_detail_no,                                                                              -- 枝番
    COALESCE(parts.lead_time, 0) AS lead_time,                                                                   -- 発注点
    COALESCE(parts.order_quantity, 0) AS order_quantity,                                                         -- 発注量
    COALESCE(parts.lead_time, 0) AS lead_time_except_unit,                                                       -- 発注点(単位なし)
    COALESCE(parts.order_quantity, 0) AS order_quantity_except_unit,                                             -- 発注量(単位なし)
    parts.unit_structure_id,                                                                                     -- 数量管理単位
    parts.vender_structure_id,                                                                                   -- 標準仕入先
    parts.currency_structure_id,                                                                                 -- 金額管理単位
    COALESCE(parts.unit_price, 0) AS unit_price,                                                                 -- 標準単価
    COALESCE(parts.unit_price, 0) AS unit_price_except_unit,                                                     -- 標準単価(単位なし)
    COALESCE(inventory.stock_quantity, 0) AS stock_quantity,                                                     -- 最新在庫数
    COALESCE(inventory.stock_quantity, 0) AS stock_quantity_except_unit,                                         -- 最新在庫数(単位なし)
    CASE
        WHEN judge_flg.extension_data = '2' THEN
        -- 在庫数 < 発注点 を判定
        CASE
            WHEN COALESCE(inventory.stock_quantity, 0) < COALESCE(parts.lead_time, 0) THEN 'Y'
            -- 発注アラーム
            ELSE ''
        END
        ELSE
        -- 在庫数 <= 発注点 を判定
        CASE
            WHEN COALESCE(inventory.stock_quantity, 0) <= COALESCE(parts.lead_time, 0) THEN 'Y'
            -- 発注アラーム
            ELSE ''
        END
    END AS order_alert,                                                                                          -- 発注アラーム
    parts.purchasing_no,                                                                                         -- 購買システムコード
    parts.use_segment_structure_id AS use_segment_structure_id,                                                  -- 使用区分
    newest_image.file_path AS image,                                                                             -- 画像
    parts.parts_id,                                                                                              -- 予備品ID
    COALESCE(unit_digit.extension_data, 0) AS unit_digit,                                                        -- 小数点以下桁数(数量)
    COALESCE(currency_digit.currency_digit, 0) AS currency_digit,                                                -- 小数点以下桁数(金額)
    COALESCE(round_division.extension_data, 1) AS unit_round_division,                                           -- 丸め処理区分(数量)
    COALESCE(round_division.extension_data, 1) AS currency_round_division,                                       -- 丸め処理区分(金額)
    rfid.rf_id_tag,                                                                                              -- RFIDタグ
    COALESCE(tag_count.t_count, 0) AS rf_count,                                                                  -- RFIDタグ件数
    COALESCE(tag_count.t_count, 0) AS rf_count_hide,                                                             -- RFIDタグ件数(詳細検索用)
    matter_unit.translation_text AS matter,                                                                      -- RFIDタグ件数 単位
        REPLACE((
                SELECT
                    dbo.get_file_download_info_row(att_temp.file_name, att_temp.attachment_id, att_temp.function_type_id, att_temp.key_id, att_temp.extension_data)
                FROM
                    #temp_attachment as att_temp
                WHERE
                    parts.parts_id = att_temp.key_id
                AND att_temp.function_type_id = 1750
                ORDER BY
                    document_no FOR xml path('')
            ), ' ', '') AS file_link_document,-- ダウンロードリンク(文書)
        REPLACE((
                SELECT
                    dbo.get_file_download_info_row(att_temp.file_name, att_temp.attachment_id, att_temp.function_type_id, att_temp.key_id, att_temp.extension_data)
                FROM
                    #temp_attachment as att_temp
                WHERE
                    parts.parts_id = att_temp.key_id
                AND att_temp.function_type_id = 1700
                ORDER BY
                    document_no FOR xml path('')
            ), ' ', '') AS file_link_image,-- ダウンロードリンク(画像)
    parts.department_structure_id,                                                                               -- 標準部門
    parts.account_structure_id,                                                                                  -- 標準勘定科目
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
                  #temp_structure_factory AS st_f
              WHERE
                  st_f.structure_id = parts.manufacturer_structure_id
              AND st_f.factory_id IN(0, parts.factory_id)
           )
      AND tra.structure_id = parts.manufacturer_structure_id
    ) AS manufacture_name,                                                                                       -- メーカー名
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
                  #temp_structure_factory AS st_f
              WHERE
                  st_f.structure_id = parts.factory_id
              AND st_f.factory_id IN(0, parts.factory_id)
           )
      AND tra.structure_id = parts.factory_id
    ) AS parts_factory_name,                                                                                      -- 管理工場名
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
                  #temp_structure_factory AS st_f
              WHERE
                  st_f.structure_id = parts.unit_structure_id
              AND st_f.factory_id IN(0, parts.factory_id)
           )
      AND tra.structure_id = parts.unit_structure_id
    ) AS unit_name,                                                                                                -- 数量管理単位
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
                  #temp_structure_factory AS st_f
              WHERE
                  st_f.structure_id = parts.currency_structure_id
              AND st_f.factory_id IN(0, parts.factory_id)
           )
      AND tra.structure_id = parts.currency_structure_id
    ) AS currency_name,                                                                                            -- 金額管理単位
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
                  #temp_structure_factory AS st_f
              WHERE
                  st_f.structure_id = parts.vender_structure_id
              AND st_f.factory_id IN(0, parts.factory_id)
           )
      AND tra.structure_id = parts.vender_structure_id
    ) AS vender_name,                                                                                              -- 標準仕入先
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
                  #temp_structure_factory AS st_f
              WHERE
                  st_f.structure_id = parts.location_district_structure_id
              AND st_f.factory_id IN(0, parts.factory_id)
           )
      AND tra.structure_id = parts.location_district_structure_id
    ) AS district_name,                                                                                       -- 標準棚_地区名
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
                  #temp_structure_factory AS st_f
              WHERE
                  st_f.structure_id = parts.location_factory_structure_id
              AND st_f.factory_id IN(0, parts.factory_id)
           )
      AND tra.structure_id = parts.location_factory_structure_id
    ) AS factory_name,                                                                                        -- 標準棚_工場名
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
                  #temp_structure_factory AS st_f
              WHERE
                  st_f.structure_id = parts.location_warehouse_structure_id
              AND st_f.factory_id IN(0, parts.factory_id)
           )
      AND tra.structure_id = parts.location_warehouse_structure_id
    ) AS warehouse_name,                                                                                       -- 標準棚_倉庫名
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
                  #temp_structure_factory AS st_f
              WHERE
                  st_f.structure_id = parts.location_rack_structure_id
              AND st_f.factory_id IN(0, parts.factory_id)
           )
      AND tra.structure_id = parts.location_rack_structure_id
    ) AS rack_name,                                                                                           -- 標準棚_地区名
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
                  #temp_structure_factory AS st_f
              WHERE
                  st_f.structure_id = parts.job_structure_id
              AND st_f.factory_id IN(0, parts.factory_id)
           )
      AND tra.structure_id = parts.job_structure_id
    ) AS job_name,                                                                                             -- 職種
    dep_ex.extension_data + ' ' + (
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
                  #temp_structure_factory AS st_f
              WHERE
                  st_f.structure_id = parts.department_structure_id
              AND st_f.factory_id IN(0, parts.factory_id)
           )
      AND tra.structure_id = parts.department_structure_id
    ) AS department_name,                                                                                        -- 部門コード + 部門名
    acc_ex.extension_data + ' ' + (
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
                  #temp_structure_factory AS st_f
              WHERE
                  st_f.structure_id = parts.account_structure_id
              AND st_f.factory_id IN(0, parts.factory_id)
           )
      AND tra.structure_id = parts.account_structure_id
    ) AS account_name                                                                                            -- 勘定科目コード + 勘定科目名
FROM
    pt_parts AS parts
    LEFT JOIN
        unit_digit -- 数量管理単位
    ON  parts.unit_structure_id = unit_digit.structure_id
    LEFT JOIN
        currency_digit -- 金額管理単位
    ON  parts.currency_structure_id = currency_digit.currency_id
    LEFT JOIN
        v_pt_parts_inventory inventory -- 最新在庫数
    ON  parts.parts_id = inventory.parts_id
    LEFT JOIN
        newest_image -- 画像
    ON  parts.parts_id = newest_image.key_id
    LEFT JOIN
        rfid -- RFIDタグ
    ON  parts.parts_id = rfid.parts_id
    LEFT JOIN
        judge_flg -- 在庫数判定フラグ
    ON  parts.factory_id = judge_flg.factory_id
    LEFT JOIN
        round_division --丸め処理区分
    ON  parts.factory_id = round_division.factory_id
    LEFT JOIN
        tag_count -- RFIDタグ件数
    ON parts.parts_id = tag_count.parts_id
    LEFT JOIN -- RFIDタグ件数単位
        matter_unit
    ON 1 = 1
    LEFT JOIN
        ms_structure dep_ms -- 部門コード(構成マスタ)
    ON parts.department_structure_id = dep_ms.structure_id
    LEFT JOIN
        ms_item_extension dep_ex  -- 部門コード(拡張)
    ON dep_ms.structure_item_id = dep_ex.item_id
    AND dep_ex.sequence_no = 1
    LEFT JOIN
        ms_structure acc_ms -- 勘定科目コード(構成マスタ)
    ON parts.account_structure_id = acc_ms.structure_id
    LEFT JOIN
        ms_item_extension acc_ex  -- 勘定科目コード(拡張)
    ON acc_ms.structure_item_id = acc_ex.item_id
    AND acc_ex.sequence_no = 1