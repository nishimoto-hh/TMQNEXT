/*
 * ボタンコントロールIDか判定 1つでも設定されている場合、ボタンと判定する
 */

SELECT COUNT(*) AS CNT
FROM
	cm_form_control_define
WHERE
	delete_flg != 1
AND program_id = /*Pgmid*/'MC0001'
AND	form_no = /*FormNo*/0
AND	button_control_id = /*BtnCtrlid*/'New'
/*IF FacrotyIdList != null && FactoryIdList.Count > 0*/
AND location_structure_id IN /*FactoryIdList*/(0, 5)
/*END*/
