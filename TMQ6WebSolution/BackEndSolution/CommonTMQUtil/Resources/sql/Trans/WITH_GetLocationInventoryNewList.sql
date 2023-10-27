WITH pt_location AS(
    SELECT
        parts_id,
        --予備品ID
        lot_control_id,
        --ロットNo
        parts_location_id,
        --棚番
        stock_quantity,
        --在庫数
        parts_location_detail_no
        --棚枝番
    FROM
        pt_location_stock
),
location AS(
    --棚番
    SELECT
        translation_text,
        structure_id
    FROM
        v_structure_item
    WHERE
        v_structure_item.structure_group_id = 1040
    AND v_structure_item.language_id = 'ja'
    AND structure_layer_no = 1
),
parts_ware AS(
    --予備品倉庫(棚ID)
    SELECT
        translation_text,
        structure_id
    FROM
        v_structure_item
    WHERE
        v_structure_item.structure_group_id = 1040
    AND v_structure_item.language_id = 'ja'
    AND structure_layer_no = 0
),
old_new AS(
    --新旧区分
    SELECT
        translation_text,
        structure_id
    FROM
        v_structure_item
    WHERE
        v_structure_item.structure_group_id = 1940
    AND v_structure_item.language_id = 'ja'
),
department AS(
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
department_code AS(
    --部門コード
    SELECT
        extension_data,
        item_id
    FROM
        ms_item_extension
),
subject_code AS(
    --勘定科目コード
    SELECT
        extension_data,
        item_id
    FROM
        ms_item_extension
),
history AS(
    --受払履歴
    SELECT
        work_no,
        lot_control_id,
        inout_division_structure_id
    FROM
        pt_inout_history
),
iot AS(
    --受払区分
    SELECT
        translation_text,
        structure_id,
        structure_item_id
    FROM
        v_structure_item
    WHERE
        v_structure_item.structure_group_id = 1950
    AND v_structure_item.language_id = 'ja'
)
