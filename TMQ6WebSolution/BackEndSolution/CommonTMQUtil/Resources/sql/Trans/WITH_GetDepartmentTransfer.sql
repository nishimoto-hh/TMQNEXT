WITH department AS(
    --部門
    SELECT
        translation_text,
        structure_id,
        structure_item_id
    FROM
        v_structure_item
    WHERE
        v_structure_item.structure_group_id = 1760
    AND v_structure_item.language_id = 'ja'
),
department_code AS(
    --部門コード
    SELECT
        extension_data,
        item_id
    FROM
        ms_item_extension
),
subject AS(
    --勘定科目
    SELECT
        translation_text,
        structure_id,
        structure_item_id
    FROM
        v_structure_item
    WHERE
        v_structure_item.structure_group_id = 1770
    AND v_structure_item.language_id = 'ja'
),
subject_code AS(
    --勘定科目コード
    SELECT
        extension_data,
        item_id
    FROM
        ms_item_extension
)
