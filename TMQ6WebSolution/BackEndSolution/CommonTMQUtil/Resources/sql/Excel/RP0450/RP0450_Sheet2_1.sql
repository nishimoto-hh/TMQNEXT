with mq as ( -- 標準アイテム(工場IDも翻訳の工場IDも「0」)のMQ分類を取得
    select
        item.structure_id
        , item.translation_text as mq_name 
    from
        v_structure_item item 
    where
        item.structure_group_id = 1850 
        and item.language_id = @LanguageId
        and item.factory_id = 0 
        and item.location_structure_id = 0
) 
, sort as ( -- 標準工場(工場ID=0)のMQ分類の並び順を取得
    select
        mso.structure_id
        , mso.display_order 
    from
        ms_structure_order mso 
    where
        mso.structure_group_id = 1850 
        and mso.factory_id = 0
) 
, summary as ( -- MQ分類ごとの件数を取得
    select
        summary.mq_class_structure_id
        , count(*) as cnt 
    from
        ma_summary summary

    -- 場所階層ツリーで選択されている項目を抽出条件とするため、ツリーで選択された項目の構成IDが格納されている一時テーブルと内部結合をする
    inner join #temp_location tl
        on summary.location_structure_id = tl.structure_id

    where
        -- 画面で指定された年月の初日から末日までが取得対象
        summary.completion_date between @TargetStartDate and @TargetEndDate

    group by
        summary.mq_class_structure_id
) 

select
    mq.structure_id                   -- MQ分類の構成ID
    , mq.mq_name                      -- MQ分類名
    , coalesce(summary.cnt, 0) as cnt -- MQ分類ごとの件数
from
    mq 
    left join sort 
        on mq.structure_id = sort.structure_id 
    left join summary 
        on mq.structure_id = summary.mq_class_structure_id 
order by
    sort.display_order
