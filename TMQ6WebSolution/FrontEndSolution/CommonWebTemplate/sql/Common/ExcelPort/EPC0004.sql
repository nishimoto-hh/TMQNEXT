/*
 * ExcelPortダウンロード対象機能 機器コンボ用データリスト用SQL
 * exparam1:自動表示用項目(場所階層、所属階層)
 */
-- EPC0004
SELECT tbl.id,
       tbl.exparam2id AS factory_id,
       tbl.parent_id,
	   tbl.name,
	   v1.translation_text AS exparam1,
	   tbl.exparam2id AS exparam2,
	   v3.translation_text AS exparam3,
	   v4.translation_text AS exparam4,
	   v5.translation_text AS exparam5,
	   v6.translation_text AS exparam6,
	   v7.translation_text AS exparam7,
	   v8.translation_text AS exparam8,
	   v9.translation_text AS exparam9,
	   v10.translation_text AS exparam10,
	   v11.translation_text AS exparam11
FROM (
SELECT
     mc.machine_id AS id
    ,NULL AS parent_id
    ,mc.machine_no + ' ' + mc.machine_name AS name
    ,dbo.get_target_layer_id(mc.location_structure_id,0) AS exparam1id
    ,dbo.get_target_layer_id(mc.location_structure_id,1) AS exparam2id
    ,dbo.get_target_layer_id(mc.location_structure_id,2) AS exparam4id
    ,dbo.get_target_layer_id(mc.location_structure_id,3) AS exparam5id
    ,dbo.get_target_layer_id(mc.location_structure_id,4) AS exparam6id
    ,dbo.get_target_layer_id(mc.location_structure_id,5) AS exparam7id
    ,dbo.get_target_layer_id(mc.job_structure_id,0) AS exparam8id
    ,dbo.get_target_layer_id(mc.job_structure_id,1) AS exparam9id
    ,dbo.get_target_layer_id(mc.job_structure_id,2) AS exparam10id
    ,dbo.get_target_layer_id(mc.job_structure_id,3) AS exparam11id

FROM mc_machine AS mc

---- 対象構成マスタ情報一時テーブルと機番情報の場所階層を結合する
/*IF factoryIdList != null && factoryIdList.Count > 0*/
INNER JOIN #temp_structure_all st
ON mc.location_structure_id = st.structure_id
/*END*/

) tbl
LEFT JOIN 
(SELECT * 
 FROM v_structure_item_all 
 WHERE  language_id = /*languageId*/'ja'
 AND  structure_layer_no = 0
 AND structure_group_id = 1000
 ) v1
ON tbl.exparam1id = v1.structure_id

LEFT JOIN 
(SELECT * 
 FROM v_structure_item_all 
 WHERE  language_id = /*languageId*/'ja'
 AND  structure_layer_no = 1
 AND structure_group_id = 1000
 ) v3
ON tbl.exparam2id = v3.structure_id

LEFT JOIN 
(SELECT * 
 FROM v_structure_item_all 
 WHERE  language_id = /*languageId*/'ja'
 AND  structure_layer_no = 2
 AND structure_group_id = 1000
 ) v4
ON tbl.exparam4id = v4.structure_id

LEFT JOIN
(SELECT * 
 FROM v_structure_item_all 
 WHERE  language_id = /*languageId*/'ja'
 AND  structure_layer_no = 3
 AND structure_group_id = 1000
 ) v5
ON tbl.exparam5id = v5.structure_id

LEFT JOIN
(SELECT * 
 FROM v_structure_item_all 
 WHERE  language_id = /*languageId*/'ja'
 AND  structure_layer_no = 4
 AND structure_group_id = 1000
 ) v6
ON tbl.exparam6id = v6.structure_id

LEFT JOIN
(SELECT * 
 FROM v_structure_item_all 
 WHERE  language_id = /*languageId*/'ja'
 AND  structure_layer_no = 5
 AND structure_group_id = 1000
 ) v7
ON tbl.exparam7id = v7.structure_id

LEFT JOIN
(SELECT * 
 FROM v_structure_item_all 
 WHERE  language_id = /*languageId*/'ja'
 AND  structure_layer_no = 0
 AND structure_group_id = 1010
 ) v8
ON tbl.exparam8id = v8.structure_id

LEFT JOIN
(SELECT * 
 FROM v_structure_item_all 
 WHERE  language_id = /*languageId*/'ja'
 AND  structure_layer_no = 1
 AND structure_group_id = 1010
 ) v9
ON tbl.exparam9id = v9.structure_id

LEFT JOIN
(SELECT * 
 FROM v_structure_item_all 
 WHERE  language_id = /*languageId*/'ja'
 AND  structure_layer_no = 2
 AND structure_group_id = 1010
 ) v10
ON tbl.exparam10id = v10.structure_id

LEFT JOIN
(SELECT * 
 FROM v_structure_item_all 
 WHERE  language_id = /*languageId*/'ja'
 AND  structure_layer_no = 3
 AND structure_group_id = 1010
 ) v11
ON tbl.exparam11id = v11.structure_id

ORDER BY
name

