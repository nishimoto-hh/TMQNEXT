/*
 * 出力パターンマスタ コンボ用データリスト用SQL.(絞り込み条件付き）
 * 出力パターンコンボボックス設定
 */
-- C0010.sql
select a.factory_id as factoryId,
        a.output_pattern_id as 'values',
        a.output_pattern_name as labels
  from ms_output_pattern a
  where a.report_id = /*param1*/''
    and a.factory_id = /*param2*/-1
    and a.template_id = /*param3*/-1
    and (a.use_user_id is null or a.use_user_id = /*userId*/-1 )
    and a.delete_flg = 0
order by a.output_pattern_id
