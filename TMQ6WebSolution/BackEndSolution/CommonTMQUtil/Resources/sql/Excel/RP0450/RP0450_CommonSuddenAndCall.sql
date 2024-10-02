with sudden_summary as ( 
    select
        ex.extension_data as sudden_division
        , sum(summary.maintenance_count) as cnt 
    from
        ma_summary summary 
        left join ms_structure ms 
            on summary.sudden_division_structure_id = ms.structure_id 
        left join ms_item_extension ex 
            on ms.structure_item_id = ex.item_id 
            and ex.sequence_no = 1

    -- 抽出対象の職種IDが格納されている一時テーブルと内部結合をする
    inner join #temp_job_id tj
        on summary.job_structure_id = tj.job_structure_id

    -- 場所階層ツリーで選択されている項目を抽出条件とするため、ツリーで選択された項目の構成IDが格納されている一時テーブルと内部結合をする
    inner join #temp_location tl
        on summary.location_structure_id = tl.structure_id

    where
    /*@TargetMonth
        -- 画面で指定された年月の初日から末日までが取得対象
        summary.completion_date between @TargetStartDate and @TargetEndDate
　　@TargetMonth*/

    /*@Half
        -- 画面で指定された年月の期の開始日から指定された年月末日までが取得対象
        summary.completion_date between @StartMonth and @TargetEndDate
　　@Half*/

    /*@Year
        -- 画面で指定された年月の初日から末日までが取得対象
        summary.completion_date between @BeginningMonth and @TargetEndDate
　　@Year*/

    group by
        ex.extension_data
) 
, main as (
    -- 「突発」のデータ
    select
        '30' as sudden_division
        , (select cnt from sudden_summary where sudden_division = '30') as data
        , 1 as sort
    -- 「計画」のデータ
    union all
    select
        '10' as sudden_division
        , (select cnt from sudden_summary where sudden_division = '10') as data
        , 2 as sort 
    -- 「計画外」
    union all
    select
        '20' as sudden_division
        , (select cnt from sudden_summary where sudden_division = '20') as data
        , 3 as sort

    -- 突発作業率(%) ここでは空のデータを取得する 実際のデータはプログラム側で計算する
    union all
    select
        '-1' as sudden_division
        , 0 as data
        , 4 as sort
    -- 呼出回数の件数
    union all
    select
        '99' as sudden_division
        , count(history.call_count) as data
        , 5 as sort 
    from
        ma_summary summary 
        left join ma_history history 
            on summary.summary_id = history.summary_id

    -- 抽出対象の職種IDが格納されている一時テーブルと内部結合をする
    inner join #temp_job_id tj
        on summary.job_structure_id = tj.job_structure_id

    -- 場所階層ツリーで選択されている項目を抽出条件とするため、ツリーで選択された項目の構成IDが格納されている一時テーブルと内部結合をする
    inner join #temp_location tl
        on summary.location_structure_id = tl.structure_id

    -- 呼出拡張項目で絞り込むための結合
    left join ms_structure ms 
        on summary.location_factory_structure_id = ms.structure_id
    left join ms_item_extension ex 
        on ms.structure_item_id = ex.item_id 
        and ex.sequence_no = 1 

    where
    /*@TargetMonth
        -- 画面で指定された年月の初日から末日までが取得対象
        summary.completion_date between @TargetStartDate and @TargetEndDate
　　@TargetMonth*/

    /*@Half
        -- 画面で指定された年月の期の開始日から指定された年月末日までが取得対象
        summary.completion_date between @StartMonth and @TargetEndDate
　　@Half*/

    /*@Year
        -- 画面で指定された年月の初日から末日までが取得対象
        summary.completion_date between @BeginningMonth and @TargetEndDate
　　@Year*/

    -- 呼出拡張項目を見ているのは保全履歴個別工場フラグが「1」でない工場
    -- かつ、呼出拡張項目(call_count)が「1」のデータが対象
    and coalesce(ex.extension_data, '0') != '1'
    and history.call_count = 1
) 
select
    coalesce(data, 0) as data 
from
    main 
order by
    sort
