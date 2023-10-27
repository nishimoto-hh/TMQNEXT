WITH 
unit_digit AS -- 小数点以下桁数(数値)
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
round_division AS( -- 丸め処理区分
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
currency_digit as -- 小数点以下桁数(金額)
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
newest_image AS( --予備品に紐付く最新の画像
    SELECT
        attachment.key_id,
        --file_path.file_path
        dbo.get_img_key(attachment.attachment_id) AS file_path
    FROM
        (
            SELECT
                ac.key_id,
                MAX(ac.attachment_id) AS attachment_id
            FROM
                attachment ac
                LEFT JOIN
                    ms_structure ms
                ON  ac.attachment_type_structure_id = ms.structure_id
                LEFT JOIN
                    ms_item_extension ex
                ON  ms.structure_item_id = ex.item_id
                AND ex.sequence_no = 1
            WHERE
                ac.function_type_id = 1700
            AND ex.extension_data = '1'
            GROUP BY
                ac.key_id
        ) attachment
        LEFT JOIN
            (
                SELECT
                    ac.attachment_id,
                    ac.file_path
                FROM
                    attachment ac
            ) file_path
        ON  attachment.attachment_id = file_path.attachment_id
),
rfid AS( -- RFIDタグ
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
max_date AS( -- 予備品に紐付く画像、文書の最大更新日時
    SELECT
        ac.key_id,
        MAX(ac.update_datetime) AS max_update_datetime
    FROM
        attachment ac
    WHERE
        ac.function_type_id IN(1700, 1750)
    GROUP BY
        ac.key_id
),
judge_flg AS( -- 在庫数判定用(1:在庫数<=発注点、2：在庫数<発注点)
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
), tag_count AS(
SELECT
    tag.parts_id,
    COUNT(tag.parts_id) AS t_count
FROM
    pt_rftag_parts_link tag
GROUP BY
    tag.parts_id
), matter_unit AS(
SELECT TOP 1
    tr.translation_text
FROM
    ms_translation tr
WHERE
    tr.translation_id = 111090061
AND tr.language_id = @LanguageId
AND tr.location_structure_id = 0
)
,structure_factory as( SELECT
        structure_id
        , location_structure_id AS factory_id 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id IN (1000, 1150, 1720, 1730,1740, 1760, 1770 ,1000,1010,1040) 
        AND language_id = @LanguageId
)