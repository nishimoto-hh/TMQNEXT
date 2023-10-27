/*
 * 一時テーブルからの総件数取得
 */

select *
  from com_temp_data
where guid = /*GUID*/'%'
  and tabno = /*TabNo*/'%'
  and ctrlid = /*Ctrlid*/'%'
/*IF Updtag != null && Updtag != ''*/
  and updtag = /*Updtag*/1
/*END*/
/*IF Seltag != null && Seltag != ''*/
  and seltag = /*Seltag*/1
/*END*/
