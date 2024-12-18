/*
 * 一覧行編集情報の取得
 */

SELECT
     program_id AS PgmId
    ,form_no AS FormNo
    ,control_group_id AS CtrlGrpId
    ,dat_row_add_division AS RowAddDiv
    ,dat_row_delete_division AS RowDelDiv
FROM cm_form_define
WHERE
    delete_flg != 1 
AND program_id != '0'
AND control_group_type IN (101, 102, 103)
