/*
 * 出力帳票定義マスタ コンボ用データリスト用SQL.(絞り込み条件付き）
 * 帳票コンボボックス設定
 */

select a.factory_id as factoryId,
       a.factory_id as translationFactoryId,
--       b.location_structure_id as translationFactoryId,
       a.report_id as 'values',
       b.translation_text as labels
  from ms_output_report_define a
  left join ms_translation b
    on a.report_name_translation_id = b.translation_id
    and b.language_id = /*languageId*/'ja' 
where 1 = 1

/*IF param1 != null && param1 != 'null' && param1 != '' */
   and a.program_id = /*param1*/''
/*END*/

/*IF param2 != null && param2 != 'null' && param2 != '' */
   and a.report_id = /*param2*/''
/*END*/

/*IF param2 == null || param2 == 'null' || param2 == '' */
   -- 各業務からの起動の場合は出力項目固定帳票は対象外とする
   and a.output_item_type <> 1
/*END*/

   and a.delete_flg = 0
   and b.delete_flg = 0
 
/*IF factoryIdList != null && factoryIdList.Count > 0*/
   and a.factory_id in /*factoryIdList*/(0)
/*END*/

order by ISNULL(a.display_order, 2147483647), a.report_id, b.translation_text
