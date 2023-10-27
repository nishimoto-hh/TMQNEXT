WITH unit_digit AS
-- 小数点以下桁数(数値)
(
    SELECT
        ms.structure_id,
        ex.extension_data
    FROM
        ms_structure ms
        LEFT JOIN
            ms_item_extension ex
        ON  ms.structure_item_id = ex.item_id
        AND ex.sequence_no = 2
    WHERE
        ms.structure_group_id = 1730
),
round_division AS(
    -- 丸め処理区分
    SELECT
        ms.factory_id,
        ex.extension_data
    FROM
        (
            SELECT
                ms.factory_id,
                MAX(ms.structure_id) AS structure_id
            FROM
                ms_structure ms
            WHERE
                ms.structure_group_id = 2050
            GROUP BY
                ms.factory_id
        ) ms
        LEFT JOIN
            (
                SELECT
                    ms.structure_id,
                    ex.extension_data
                FROM
                    ms_structure ms
                    LEFT JOIN
                        ms_item_extension ex
                    ON  ms.structure_item_id = ex.item_id
                    AND ex.sequence_no = 1
                WHERE
                    ms.structure_group_id = 2050
            ) ex
        ON  ms.structure_id = ex.structure_id
),
currency_digit AS
-- 小数点以下桁数(金額)
(
    SELECT
        ms.structure_id AS currency_id,
        ex.extension_data AS currency_digit
    FROM
        ms_structure ms
        LEFT JOIN
            ms_item_extension ex
        ON  ms.structure_item_id = ex.item_id
        AND ex.sequence_no = 2
    WHERE
        ms.structure_group_id = 1740
),
rfid AS(
    -- RFIDタグ
    SELECT
        tag_a.parts_id,
        trim(
            ','
            FROM
                (
                    SELECT
                        tag_b.rftag_id + ','
                    FROM
                        pt_rftag_parts_link tag_b
                    WHERE
                        tag_b.parts_id = tag_a.parts_id FOR XML PATH('')
                )
        ) AS rf_id_tag
    FROM
        pt_rftag_parts_link tag_a
    GROUP BY
        tag_a.parts_id
),
judge_flg AS(
    -- 在庫数判定用(1:在庫数<=発注点、2：在庫数<発注点)
SELECT
    ms.factory_id,
    ex.extension_data
FROM
    (
        SELECT
            ms.factory_id,
            MAX(ms.structure_id) AS structure_id
        FROM
            ms_structure ms
        WHERE
            ms.structure_group_id = 2040
        GROUP BY
            ms.factory_id
    ) ms
    LEFT JOIN
        (
            SELECT
                ms.structure_id,
                ex.extension_data
            FROM
                ms_structure ms
                LEFT JOIN
                    ms_item_extension ex
                ON  ms.structure_item_id = ex.item_id
                AND ex.sequence_no = 1
            WHERE
                ms.structure_group_id = 2040
        ) ex
    ON  ms.structure_id = ex.structure_id
),
warehouse_to_district AS
-- 倉庫から地区までのIDを取得
(
    SELECT
        ware_house.ware_house_id,
        factory.factory_id,
        district.district_id
    FROM
        (
            SELECT
                ware_house.structure_id AS ware_house_id,
                ware_house.parent_structure_id
            FROM
                ms_structure ware_house
            WHERE
                ware_house.structure_group_id = 1040
            AND ware_house.structure_layer_no = 2
        ) ware_house
        LEFT JOIN
            (
                SELECT
                    factory.structure_id AS factory_id,
                    factory.parent_structure_id
                FROM
                    ms_structure factory
                WHERE
                    factory.structure_group_id = 1000
                AND factory.structure_layer_no = 1
            ) factory
        ON  ware_house.parent_structure_id = factory.factory_id
        LEFT JOIN
            (
                SELECT
                    district.structure_id AS district_id,
                    district.parent_structure_id
                FROM
                    ms_structure district
                WHERE
                    district.structure_group_id = 1000
                AND district.structure_layer_no = 0
            ) district
        ON  factory.parent_structure_id = district.district_id
),
rack_to_district AS(
    SELECT
        rack.structure_id AS rack_id,
        warehouse_to_district.ware_house_id,
        warehouse_to_district.factory_id,
        warehouse_to_district.district_id
    FROM
        ms_structure rack
        LEFT JOIN
            warehouse_to_district
        ON  rack.parent_structure_id = warehouse_to_district.ware_house_id
    WHERE
        rack.structure_group_id = 1040
    AND rack.structure_layer_no = 3
),
structure_factory AS(
    SELECT
        structure_id,
        location_structure_id AS factory_id
    FROM
        v_structure_item_all
    WHERE
        structure_group_id IN(1000, 1010, 1040, 1150, 1720, 1730, 1740, 1970)
    AND language_id = (
                        SELECT DISTINCT
                            temp.LanguageId COLLATE Japanese_CI_AS
                        FROM
                            #temp temp
                       )
)
SELECT
    parts.parts_no,                                                           -- 予備品No.
    parts.parts_name,                                                         -- 予備品名
    parts.model_type,                                                         -- 型式
    parts.materials,                                                          -- 材質
    parts.standard_size,                                                      -- 規格・寸法
    parts.parts_service_space,                                                -- 使用場所
    parts.factory_id,                                                         -- 管理工場
    COALESCE(parts.parts_location_detail_no, '') AS parts_location_detail_no, -- 枝番
    COALESCE(parts.lead_time, 0) AS lead_time,                                -- 発注点
    COALESCE(parts.order_quantity, 0) AS order_quantity,                      -- 発注量
    COALESCE(parts.unit_price, 0) AS unit_price,                              -- 標準単価
    COALESCE(parts_inventory.stock_quantity, 0) AS stock_quantity,            -- 最新在庫数
    CASE
        WHEN judge_flg.extension_data = '2' THEN
        -- 在庫数 < 発注点 を判定
        CASE
            WHEN COALESCE(parts_inventory.stock_quantity, 0) < COALESCE(parts.lead_time, 0) THEN 'Y'
            ELSE ''
        END
        ELSE
        -- 在庫数 <= 発注点 を判定
        CASE
            WHEN COALESCE(parts_inventory.stock_quantity, 0) <= COALESCE(parts.lead_time, 0) THEN 'Y'
            ELSE ''
        END
    END AS order_alert,                                                    -- 発注アラーム
    parts.purchasing_no,                                                   -- 購買システムコード
    rfid.rf_id_tag,                                                        -- RFIDタグ
    COALESCE(unit_digit.extension_data, 0) AS unit_digit,                  -- 小数点以下桁数(数量)
    COALESCE(currency_digit.currency_digit, 0) AS currency_digit,          -- 小数点以下桁数(金額)
    COALESCE(round_division.extension_data, 1) AS unit_round_division,     -- 丸め処理区分(数量)
    COALESCE(round_division.extension_data, 1) AS currency_round_division, -- 丸め処理区分(金額)
    ---------------------------------- 以下は翻訳を取得 ----------------------------------
    (
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.LanguageId COLLATE Japanese_CI_AS
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = parts.manufacturer_structure_id
                AND st_f.factory_id IN(0, parts.factory_id)
            )
        AND tra.structure_id = parts.manufacturer_structure_id
    ) AS manufacturer_name, -- メーカー
    (
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.LanguageId COLLATE Japanese_CI_AS
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = parts.factory_id
                AND st_f.factory_id IN(0, parts.factory_id)
            )
        AND tra.structure_id = parts.factory_id
    ) AS factory_name, -- 管理工場
    (
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.LanguageId COLLATE Japanese_CI_AS
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = parts.job_structure_id
                AND st_f.factory_id IN(0, parts.factory_id)
            )
        AND tra.structure_id = parts.job_structure_id
    ) AS job_name, -- 職種
    (
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.LanguageId COLLATE Japanese_CI_AS
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = parts.unit_structure_id
                AND st_f.factory_id IN(0, parts.factory_id)
            )
        AND tra.structure_id = parts.unit_structure_id
    ) AS unit_name, -- 数量管理単位
    (
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.LanguageId COLLATE Japanese_CI_AS
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = parts.vender_structure_id
                AND st_f.factory_id IN(0, parts.factory_id)
            )
        AND tra.structure_id = parts.vender_structure_id
    ) AS vender_name, -- 標準仕入先
    (
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.LanguageId COLLATE Japanese_CI_AS
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
    ) AS currency_name, -- 金額管理単位
    (
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.LanguageId COLLATE Japanese_CI_AS
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = parts.use_segment_structure_id
                AND st_f.factory_id IN(0, parts.factory_id)
            )
        AND tra.structure_id = parts.use_segment_structure_id
    ) AS use_segment_name, -- 使用区分
    CASE
        WHEN ms.structure_layer_no = 2 THEN( -- pt_partsの標準棚IDが倉庫IDの場合
            SELECT
                tra.translation_text
            FROM
                v_structure_item_all AS tra
            WHERE
                tra.language_id = temp.LanguageId COLLATE Japanese_CI_AS
            AND tra.location_structure_id = (
                    SELECT
                        MAX(st_f.factory_id)
                    FROM
                        structure_factory AS st_f
                    WHERE
                        st_f.structure_id = warehouse_to_district.district_id
                    AND st_f.factory_id IN(0, warehouse_to_district.factory_id)
                )
            AND tra.structure_id = warehouse_to_district.district_id
        )
        ELSE( -- pt_partsの標準棚IDが棚IDの場合
            SELECT
                tra.translation_text
            FROM
                v_structure_item_all AS tra
            WHERE
                tra.language_id = temp.LanguageId COLLATE Japanese_CI_AS
            AND tra.location_structure_id = (
                    SELECT
                        MAX(st_f.factory_id)
                    FROM
                        structure_factory AS st_f
                    WHERE
                        st_f.structure_id = rack_to_district.district_id
                    AND st_f.factory_id IN(0, rack_to_district.factory_id)
                )
            AND tra.structure_id = rack_to_district.district_id
        )
    END AS district_name, -- 地区
    CASE
        WHEN ms.structure_layer_no = 2 THEN( -- pt_partsの標準棚IDが倉庫IDの場合
            SELECT
                tra.translation_text
            FROM
                v_structure_item_all AS tra
            WHERE
                tra.language_id = temp.LanguageId COLLATE Japanese_CI_AS
            AND tra.location_structure_id = (
                    SELECT
                        MAX(st_f.factory_id)
                    FROM
                        structure_factory AS st_f
                    WHERE
                        st_f.structure_id = warehouse_to_district.factory_id
                    AND st_f.factory_id IN(0, warehouse_to_district.factory_id)
                )
            AND tra.structure_id = warehouse_to_district.factory_id
        )
        ELSE( -- pt_partsの標準棚IDが棚IDの場合
            SELECT
                tra.translation_text
            FROM
                v_structure_item_all AS tra
            WHERE
                tra.language_id = temp.LanguageId COLLATE Japanese_CI_AS
            AND tra.location_structure_id = (
                    SELECT
                        MAX(st_f.factory_id)
                    FROM
                        structure_factory AS st_f
                    WHERE
                        st_f.structure_id = rack_to_district.factory_id
                    AND st_f.factory_id IN(0, rack_to_district.factory_id)
                )
            AND tra.structure_id = rack_to_district.factory_id
        )
    END AS defalut_favtory_name, -- 工場
    CASE
        WHEN ms.structure_layer_no = 2 THEN( -- pt_partsの標準棚IDが倉庫IDの場合
            SELECT
                tra.translation_text
            FROM
                v_structure_item_all AS tra
            WHERE
                tra.language_id = temp.LanguageId COLLATE Japanese_CI_AS
            AND tra.location_structure_id = (
                    SELECT
                        MAX(st_f.factory_id)
                    FROM
                        structure_factory AS st_f
                    WHERE
                        st_f.structure_id = warehouse_to_district.ware_house_id
                    AND st_f.factory_id IN(0, warehouse_to_district.factory_id)
                )
            AND tra.structure_id = warehouse_to_district.ware_house_id
        )
        ELSE( -- pt_partsの標準棚IDが棚IDの場合
            SELECT
                tra.translation_text
            FROM
                v_structure_item_all AS tra
            WHERE
                tra.language_id = temp.LanguageId COLLATE Japanese_CI_AS
            AND tra.location_structure_id = (
                    SELECT
                        MAX(st_f.factory_id)
                    FROM
                        structure_factory AS st_f
                    WHERE
                        st_f.structure_id = rack_to_district.ware_house_id
                    AND st_f.factory_id IN(0, rack_to_district.factory_id)
                )
            AND tra.structure_id = rack_to_district.ware_house_id
        )
    END AS warehouse_name, -- 倉庫
    CASE
        WHEN ms.structure_layer_no = 3 THEN( -- pt_partsの標準棚IDが棚IDの場合
            SELECT
                tra.translation_text
            FROM
                v_structure_item_all AS tra
            WHERE
                tra.language_id = temp.LanguageId COLLATE Japanese_CI_AS
            AND tra.location_structure_id = (
                    SELECT
                        MAX(st_f.factory_id)
                    FROM
                        structure_factory AS st_f
                    WHERE
                        st_f.structure_id = parts.parts_location_id
                    AND st_f.factory_id IN(0, parts.factory_id)
                )
            AND tra.structure_id = parts.parts_location_id
        )
        ELSE ''
    END AS rack_name -- 棚番
FROM
    #temp temp
    INNER JOIN
          pt_parts parts
    ON temp.Key1 = parts.parts_id
    LEFT JOIN
        v_pt_parts_inventory parts_inventory -- 最新在庫数
    ON  parts.parts_id = parts_inventory.parts_id
    LEFT JOIN
        judge_flg -- 在庫数判定用
    ON  parts.factory_id = judge_flg.factory_id
    LEFT JOIN
        rfid -- RFIDタグ
    ON  parts.parts_id = rfid.parts_id
    LEFT JOIN
       unit_digit -- 小数点以下桁数(数量)
    ON  parts.unit_structure_id = unit_digit.structure_id
    LEFT JOIN
        currency_digit -- 小数点以下桁数(金額)
    ON  parts.currency_structure_id = currency_digit.currency_id
    LEFT JOIN
        round_division -- 丸め処理区分
    ON  parts.factory_id = round_division.factory_id
    LEFT JOIN -- 構成マスタ
        ms_structure ms
    ON  parts.parts_location_id = ms.structure_id
    LEFT JOIN -- 倉庫IDから地区IDまでのリスト(pt_partsの標準棚IDが倉庫IDの場合)
        warehouse_to_district
    ON  parts.parts_location_id = warehouse_to_district.ware_house_id
    LEFT JOIN -- 棚IDから地区IDまでのリスト(pt_partsの標準棚IDが棚IDの場合)
        rack_to_district
    ON  parts.parts_location_id = rack_to_district.rack_id
ORDER BY
    parts.parts_no,
    parts.parts_name