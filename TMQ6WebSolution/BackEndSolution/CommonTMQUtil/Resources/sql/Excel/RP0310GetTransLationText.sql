WITH max_location_structure_id AS
-- 翻訳IDごとの最大の工場IDを取得
(
    SELECT
        ms.translation_id,
        MAX(ms.location_structure_id) AS location_structure_id
    FROM
        ms_translation ms
    WHERE
        ms.translation_id IN
        (
         111380061,-- 棚卸準備表
         111160041,-- 棚卸準備日時
         111160019,-- 棚番
         111380022,-- 予備品No.
         111380023,-- 予備品名
         111060075,-- 型式(使用)
         111340003,-- メーカー
         111120068,-- 新旧区分
         111110025,-- 在庫金額
         111280034,-- 部門コード
         111060022,-- 勘定科目
         111110003,-- 在庫数
         111160042,-- 棚卸数
         111270027,-- 備考
         111160059,-- 棚ID
         111010011 -- RFIDタグ
        )
    AND ms.language_id =@LanguageId
    AND ms.location_structure_id IN @FactoryIdList
    GROUP BY
        ms.translation_id
)
SELECT
    ms.translation_id,
    ms.translation_text
FROM
    ms_translation ms
    INNER JOIN
        max_location_structure_id ml
    ON  ms.translation_id = ml.translation_id
    AND ms.location_structure_id = ml.location_structure_id
WHERE
    ms.language_id = @LanguageId
