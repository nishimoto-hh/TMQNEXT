/*
 * 機能マスタ コンボ用データリスト用SQL.(絞り込み条件付き）
 * 
 */
select a.conduct_id as 'values',
        b.translation_text as labels
  from cm_conduct a
  inner join ms_translation b
    on  b.translation_id = a.conduct_name
    and b.language_id = /*languageId*/'ja' 
  where a.conduct_group_id = /*param1*/'' 
    and a.delete_flg = 0
    and b.delete_flg = 0
order by a.menu_order
