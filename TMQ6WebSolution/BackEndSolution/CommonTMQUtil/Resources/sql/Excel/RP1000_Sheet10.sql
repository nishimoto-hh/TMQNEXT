/*
 * ExcelPort用レイアウト定義情報シートデータ取得用SQL
 */
SELECT
      tmp.Key2 AS sheet_no                                          -- 出力時シート番号
    , it.item_id                                                    -- 項目ID
    , COALESCE(NULLIF(it.column_name,''), co.column_name) AS column_name    -- カラム名
    --, it.control_type                                               -- コントロール種類
    , CASE                                                          -- 列種類
        WHEN it.control_type IN('0102') THEN 2                          -- 数値
        WHEN it.control_type IN('0104', '0107', '0901') THEN 3          -- 日付
        WHEN it.control_type IN('0105', '0902') THEN 4                  -- 時刻
        WHEN it.control_type IN('0103','0701','0702') THEN 5            -- コード＋翻訳、コンボボックス、リストボックス⇒コンボボックス
        WHEN it.control_type IN('0501') THEN 6                          -- 複数選択チェックボックス⇒複数選択リストボックス
        WHEN it.control_type IN('0401') THEN 7                          -- チェックボックス        
        WHEN it.control_type IN('0703') THEN 8                          -- ExcelPort用検索コントロール
        WHEN it.control_type IN('0201') THEN 9                          -- テキストエリア
        ELSE 1                                                          -- 上記以外は文字列
      END AS column_type
    , it.ep_column_division                                         -- 列区分
    ,[dbo].[get_rep_translation_text](tmp.factoryId, it.control_id, tmp.languageId) AS item_name -- 項目名
    , it.default_cell_column_no AS column_no                        -- 列番号
    , CONCAT(tmp.Key2, '_', it.default_cell_column_no) AS sheet_no_and_column_no -- 出力時シート番号_列
    , it.ep_required_flg                                            -- 必須項目区分(ExcelPort用)
    , it.ep_relation_id                                             -- 関連情報ID(ExcelPort選択項目生成用)
    , it.ep_relation_parameters                                     -- 関連情報パラメータ(ExcelPort選択項目生成用)
    , it.ep_select_group_id                                         -- 選択項目グループID(ExcelPort用)
    , it.ep_select_id_column_no                                     -- 選択項目ID格納先列番号(ExcelPort用)
    , it.ep_select_link_column_no                                   -- 選択項目連動元列番号(ExcelPort用)
    , it.ep_auto_extention_column_no                                -- 自動表示拡張列番号(ExcelPort用)
    , oi.display_order                                              -- 表示順
    , co.data_type                                                  -- データ種別
    , co.minimum_value                                              -- 最小値
    , co.maximum_value                                              -- 最大値
    , co.maximum_length                                             -- 最大桁数
    ,[dbo].[get_rep_translation_text](tmp.factoryId, co.format_translation_id, tmp.languageId) AS format_text    -- 書式文字列
FROM
    ms_output_report_item_define it         -- 出力帳票項目定義
    INNER JOIN     
        (SELECT TOP 1 * FROM #temp) tmp
    ON it.sheet_no = tmp.Key1               -- 対象シート番号
    INNER JOIN
        ms_output_item oi                   -- 出力項目
    ON
        it.factory_id = oi.factory_id       -- 工場ID
    and it.report_id = oi.report_id         -- 帳票ID
    and it.sheet_no = oi.sheet_no           -- シート番号
    and it.item_id = oi.item_id             -- 項目ID
    INNER JOIN
        cm_control_define co                -- コントロール定義
    ON
        it.control_id = co.control_id       -- コントロールID
    and it.control_type = co.control_type   -- コントロールタイプ
WHERE
    it.factory_id = 0                       -- 工場ID
AND it.report_id = 'RP1000'                 -- 帳票ID
AND it.ep_define_flg = 1                    -- ExcelPort定義情報シート出力フラグ
AND oi.template_id = 1            -- テンプレートID
AND oi.output_pattern_id = 1 -- 出力パターンID
ORDER BY
    oi.display_order                        -- 表示順

