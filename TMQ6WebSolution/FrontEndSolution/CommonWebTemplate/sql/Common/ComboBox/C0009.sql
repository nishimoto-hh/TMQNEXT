/*
 * 出力テンプレートマスタ コンボ用データリスト用SQL.(絞り込み条件付き）
 * テンプレートコンボボックス設定
 */
-- C0009.sql
select a.factory_id as factoryId,
        a.template_id as 'values',
        a.template_name as labels
  from ms_output_template a
    where a.report_id = /*param1*/''
    and a.factory_id = /*param2*/-1
    and (a.use_user_id is null or a.use_user_id = /*userId*/-1 )
    and a.delete_flg = 0
order by a.template_id
