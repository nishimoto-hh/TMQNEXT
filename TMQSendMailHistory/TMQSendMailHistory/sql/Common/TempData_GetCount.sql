/*
 * 一時テーブルからの総件数取得
 */

select count(*) as Count
  from com_temp_data
where guid = /*GUID*/
and tabno = /*TabNo*/'%'
