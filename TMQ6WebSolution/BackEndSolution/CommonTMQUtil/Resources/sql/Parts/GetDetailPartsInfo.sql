
,district AS
(
SELECT
    ms.structure_id,
    ms.parent_structure_id
FROM
    ms_structure ms
WHERE
    ms.structure_group_id = 1000
)
SELECT
    /*************予備品情報*************/
    parts.parts_no,                                                                                              -- 予備品No
    parts.parts_no AS parts_no_before,                                                                           -- 予備品No                                      
    parts.parts_name,                                                                                            -- 予備品名
    parts.manufacturer_structure_id,                                                                             -- メーカー
    parts.manufacturer_structure_id AS manufacture_name,                                                         -- メーカー名
    parts.model_type,                                                                                            -- 型式
    parts.materials,                                                                                             -- 材質
    parts.standard_size,                                                                                         -- 規格・寸法
    COALESCE(parts.job_structure_id, 0) AS job_structure_id,                                                     -- 職種
    parts.parts_service_space,                                                                                   -- 使用場所
    parts.parts_id,                                                                                              -- 予備品ID
    parts.factory_id,                                                                                            -- 工場ID
    parts.factory_id AS parts_factory_id,                                                                        -- 管理工場ID
    district.parent_structure_id AS parts_parent_structure_id,                                                   -- 地区ID
    parts.use_segment_structure_id,                                                                              -- 使用区分
    parts.update_serialid,                                                                                       -- 更新シリアルID
    max_date.max_update_datetime,                                                                                -- 最大更新日時
    COALESCE(tag_count.t_count, 0) AS rf_count,                                                                  -- RFタグ件数
    matter_unit.translation_text AS matter,                                                                      -- RFタグ件数 単位
    parts.department_structure_id,                                                                               -- 標準部門ID
    dep_ex.extension_data AS department_code,                                                                    -- 標準部門コード
    dep_ex.extension_data AS department_cd_enter,                                                                -- 標準部門コード(ラベル出力用)
    /**********標準保管場所情報**********/
    parts.parts_location_id,                                                                                     -- 標準棚番ID
    parts.parts_location_id AS location_structure_id,                                                            -- 標準棚番ID
    parts.parts_location_id AS location_id,                                                                      -- 標準棚番ID
    parts.parts_location_detail_no,                                                                              -- 標準棚枝番
    parts.parts_location_id AS parts_location_id_enter,                                                          -- 標準棚番ID(ラベル出力用)
    parts.parts_location_detail_no AS parts_location_detail_no_enter,                                            -- 標準棚枝番(ラベル出力用)
    (
    SELECT
        ms.structure_layer_no
    FROM
        ms_structure ms
    WHERE
        ms.structure_id = parts.parts_location_id
    ) AS structure_layer_no,                                                                                     -- 棚番の階層番号
    /************購買管理情報************/
    COALESCE(parts.lead_time, 0) AS lead_time,                                                                   -- 発注点
    COALESCE(parts.lead_time, 0) AS lead_time_except_unit,                                                       -- 発注点(単位なし)
    COALESCE(parts.order_quantity, 0) AS order_quantity,                                                         -- 発注量
    COALESCE(parts.order_quantity, 0) AS order_quantity_except_unit,                                             -- 発注量(単位なし)
    parts.unit_structure_id,                                                                                     -- 数量管理単位
    parts.vender_structure_id,                                                                                   -- 標準仕入先
    parts.vender_structure_id AS vender_name,                                                                    -- 標準仕入先名
    parts.currency_structure_id,                                                                                 -- 金額管理単位
    COALESCE(parts.unit_price, 0) AS unit_price,                                                                 -- 標準単価
    COALESCE(parts.unit_price, 0) AS unit_price_except_unit,                                                     -- 標準単価(単位なし)
    COALESCE(inventory.stock_quantity, 0) AS stock_quantity,                                                     -- 最新在庫数
    parts.purchasing_no,                                                                                         -- 購買システムコード
    parts.parts_memo,                                                                                            -- メモ
    COALESCE(unit_digit.extension_data, 0) AS unit_digit,                                                        -- 小数点以下桁数(数量)
    COALESCE(currency_digit.currency_digit, 0) AS currency_digit,                                                -- 小数点以下桁数(金額)
    COALESCE(round_division.extension_data, 1) AS unit_round_division,                                           -- 丸め処理区分(数量)
    COALESCE(round_division.extension_data, 1) AS currency_round_division,                                       -- 丸め処理区分(金額)
    parts.factory_id AS factory_round_division,                                                                  -- 工場の丸め処理区分
    parts.account_structure_id,                                                                                  -- 標準勘定科目ID
    acc_ex.extension_data AS account_code,                                                                       -- 標準勘定科目コード
    acc_ex.extension_data AS subject_cd_enter,                                                                   -- 標準勘定科目コード(ラベル出力用)
    /****************画像****************/
    newest_image.file_path AS image,                                                                             -- 画像
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
                  st_f.structure_id = parts.unit_structure_id
              AND st_f.factory_id IN(0,parts.factory_id)
           )
      AND tra.structure_id = parts.unit_structure_id
    ) AS unit_name,                                                                                              -- 数量管理単位(名称)   
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
                  st_f.structure_id = parts.currency_structure_id
              AND st_f.factory_id IN(0, parts.factory_id)
           )
      AND tra.structure_id = parts.currency_structure_id
    ) AS currency_name,                                                                                          -- 金額管理単位(名称)
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
                  structure_factory AS st_f
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
                  structure_factory AS st_f
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
        max_date -- 最大更新日時
    ON  parts.parts_id = max_date.key_id
    LEFT JOIN
        round_division --丸め処理区分
    ON  parts.factory_id = round_division.factory_id
    LEFT JOIN
        district -- 地区
    ON parts.factory_id = district.structure_id
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
WHERE
    parts.parts_id = @PartsId