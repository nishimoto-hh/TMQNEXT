--******************************************************************
--出庫一覧(親)
--******************************************************************
SELECT
    inout_datetime,
    work_no,
    parts_no,
    parts_name,
    old_new_structure_name,
    dimensions,
    shipping_division_name,
    SUM(inout_quantity) AS inout_quantity,
    issue_quantity,
    SUM(inout_quantity * unit_price) AS issue_monney,
    currency_name,
    unit_digit,
    currency_digit,
    unit_round_division,
    currency_round_division,
    parts_id,
    job_structure_id,
    parts_location_id,
    parts_factory_id,
    CONVERT(varchar, work_no) + '_' + CONVERT(varchar, old_new_structure_id) AS nest_key
FROM
    target
GROUP BY
    inout_datetime,
    work_no,
    parts_no,
    parts_name,
    old_new_structure_name,
    dimensions,
    shipping_division_name,
    issue_quantity,
    currency_name,
    unit_digit,
    currency_digit,
    unit_round_division,
    currency_round_division,
    parts_id,
    job_structure_id,
    parts_location_id,
    parts_factory_id,
    CONVERT(varchar, work_no) + '_' + CONVERT(varchar, old_new_structure_id)