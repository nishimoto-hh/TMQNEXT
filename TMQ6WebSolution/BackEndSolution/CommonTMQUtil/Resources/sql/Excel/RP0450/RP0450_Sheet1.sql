select
    summary.subject             -- 作業件名
    , summary.completion_date   -- 完了日
    , summary.maintenance_count -- カウント件数
    , tjo.sort
    ---------------- 以下は翻訳を取得 ----------------
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = summary.job_structure_id 
                    and st_f.factory_id in (0, summary.location_factory_structure_id)
            ) 
            and tra.structure_id = summary.job_structure_id
    ) as job_name -- 職種
    , ( 
        select
            tra.translation_text 
        from
            v_structure_item_all as tra 
        where
            tra.language_id = @LanguageId
            and tra.location_structure_id = ( 
                select
                    max(st_f.factory_id) 
                from
                    #temp_structure_factory as st_f 
                where
                    st_f.structure_id = summary.mq_class_structure_id 
                    and st_f.factory_id in (0, summary.location_factory_structure_id)
            ) 
            and tra.structure_id = summary.mq_class_structure_id
    ) as mq_name -- MQ分類
from
    ma_summary summary

    -- 場所階層ツリーで選択されている項目を抽出条件とするため、ツリーで選択された項目の構成IDが格納されている一時テーブルと内部結合をする
    inner join #temp_location tl
        on summary.location_structure_id = tl.structure_id

    -- 職種(出力対象)の並び順が格納されている一時テーブル
    inner join #temp_job_order tjo
       on summary.job_structure_id = tjo.structure_id
where
    -- 画面で指定された年月の初日から末日までが取得対象
    summary.completion_date between @TargetStartDate and @TargetEndDate
order by
    tjo.sort                  -- 職種
    , summary.completion_date -- 完了日

