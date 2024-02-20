with sudden as ( -- 突発計画区分ごとのカウント件数の合計値を取得
    select
        ex.extension_data
        , sum(summary.maintenance_count) as cnt 
    from
        ma_summary summary 
        left join ms_structure ms 
            on summary.sudden_division_structure_id = ms.structure_id 
        left join ms_item_extension ex 
            on ms.structure_item_id = ex.item_id 
            and ex.sequence_no = 1

    -- 場所階層ツリーで選択されている項目を抽出条件とするため、ツリーで選択された項目の構成IDが格納されている一時テーブルと内部結合をする
    inner join #temp_location tl
        on summary.location_structure_id = tl.structure_id

    where
        -- 画面で指定された年月の初日から末日までが取得対象
        summary.completion_date between @TargetStartDate and @TargetEndDate

    group by
        ex.extension_data
) 
, main as (
    -- 「突発」のデータ
    select
        '30' as sudden_division
        , (select cnt from sudden where sudden.extension_data = '30') as cnt
        , 1 as sort 

    -- 「計画」のデータ
    union 
    select
        '10' as sudden_division
        , (select cnt from sudden where sudden.extension_data = '10') as cnt
        , 2 as sort
    
    -- 「計画外」
    union 
    select
        '20' as sudden_division
        , (select cnt from sudden where sudden.extension_data = '20') as cnt
        , 3 as sort
) 
select
    main.sudden_division           -- 突発計画区分
    , coalesce(main.cnt, 0) as cnt -- 突発計画区分ごとのカウント件数の合計値
from
    main 
order by
    sort
