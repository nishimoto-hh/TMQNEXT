/*
 * 出力帳票定義マスタ コンボ用データリスト用SQL.(絞り込み条件付き）
 * 工場コンボボックス設定
 */
-- C0008.sql

select d.factoryId, d.values_sub as 'values', d.labels 
from
(
    -- "共通"レコードの挿入
    select 0 as factoryId,
           0 as values_sub,
           c.translation_text as labels
      from ms_translation c
    where c.location_structure_id = 0
      and c.translation_id = /*param2*/911070005
      and c.language_id = /*languageId*/'ja' 
      and c.delete_flg = 0
    union all
    -- 管理者でない場合の抽出SQL（工場IDを条件に追加）
    select a.factory_id as factoryId,
           a.factory_id as values_sub,
           b.translation_text as labels
      from ms_output_report_define a
      inner join v_structure_item_all b
        on a.factory_id = b.factory_id
        and b.structure_layer_no = 1 
        and b.structure_group_id = 1040
        and b.language_id = /*languageId*/'ja' 
/*IF factoryIdList != null && factoryIdList.Count > 0*/
        and a.factory_id in /*factoryIdList*/(0)
/*END*/
    where a.report_id = /*param1*/''
      and a.delete_flg = 0
      and /*param3*/-1 != 0
    -- 管理者の場合の抽出SQL（工場IDを条件に追加）
    union all
    select a.factory_id as factoryId,
           a.factory_id as values_sub,
           b.translation_text as labels
      from ms_output_report_define a
      inner join v_structure_item_all b
        on a.factory_id = b.factory_id
        and b.structure_layer_no = 1 
        and b.structure_group_id = 1040
        and b.language_id = /*languageId*/'ja' 
    where a.report_id = /*param1*/''
      and a.delete_flg = 0
      and /*param3*/-1 = 0
) d
order by d.factoryId
