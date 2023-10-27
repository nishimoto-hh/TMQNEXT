WITH attachmentTypeNo AS
(
SELECT
    structure_id,
    extension_data
FROM
    ms_structure ms
    LEFT JOIN
        ms_item_extension ex
    ON  ms.structure_item_id = ex.item_id
WHERE
    ms.structure_group_id = 1710
AND ex.sequence_no = 1
),
function_name AS
(
SELECT
    ms.structure_id,
    tr.location_structure_id,
    tr.translation_text
FROM
    ms_structure ms
    LEFT JOIN
        ms_item_extension ex
    ON  ms.structure_item_id = ex.item_id
    AND ex.sequence_no = 1
    LEFT JOIN
        ms_translation tr
    ON  ex.extension_data = cast(tr.translation_id AS varchar)
WHERE
    tr.language_id = @LanguageId
),
conduct_name AS
(
SELECT
    ms.structure_id,
    tr.location_structure_id,
    tr.translation_text
FROM
    ms_structure ms
    LEFT JOIN
        ms_item_extension ex
    ON  ms.structure_item_id = ex.item_id
    AND ex.sequence_no = 2
    LEFT JOIN
        ms_translation tr
    ON  ex.extension_data = cast(tr.translation_id AS varchar)
WHERE
    tr.language_id = @LanguageId
),
factory AS(
    SELECT DISTINCT
        structure_id,
        location.translation_text
    FROM
        v_structure_item_all location
    WHERE
        structure_group_id = 1000
    AND structure_layer_no = 0
    AND language_id = @LanguageId
)