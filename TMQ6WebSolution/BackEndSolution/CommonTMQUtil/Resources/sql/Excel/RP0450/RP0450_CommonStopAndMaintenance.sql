-- 「件数」～「自係＋外注」の共通部分(系停止回数～総作業件数)の値を取得するためのSQL
-- 共通テンプレート専用SQL

with stop_summary as (
    -- 系停止区分ごとの系停止時間、カウント件数の合計値を取得
    select
        ex.extension_data as stop_division
        , sum(summary.stop_time) as stop_time
        , sum(summary.maintenance_count) maintenance_count 
    from
        ma_summary summary 
        left join ms_structure ms 
            on summary.stop_system_structure_id = ms.structure_id 
        left join ms_item_extension ex 
            on ms.structure_item_id = ex.item_id 
            and ex.sequence_no = 1
        left join ms_structure ms2 
            on summary.mq_class_structure_id = ms2.structure_id 
        left join ms_item_extension ex2 
            on ms2.structure_item_id = ex2.item_id 
            and ex2.sequence_no = 2

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

    and ex2.extension_data in ('10', '20', '30', '40') --設備工事、撤去工事以外

    group by
        ex.extension_data 
    union all

    -- 系停止区分の条件なしでカウント件数の合計値を取得
    select
        '99' as stop_division
        , 0 as stop_time
        , sum(summary.maintenance_count) maintenance_count 
    from
        ma_summary summary
        left join ms_structure ms2 
            on summary.mq_class_structure_id = ms2.structure_id 
        left join ms_item_extension ex2 
            on ms2.structure_item_id = ex2.item_id 
            and ex2.sequence_no = 2


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

    and ex2.extension_data in ('10', '20', '30', '40') --設備工事、撤去工事以外
) 
, main as (
    -- 保全要因のカウント件数
    select
        1 as sort
        , (select maintenance_count from stop_summary where stop_division = '10') as data
    -- 保全要因の系停止時間
    union 
    select
        2 as sort
        , (select stop_time from stop_summary where stop_division = '10') as data
    -- 工程のカウント件数
    union 
    select
        3 as sort
        , (select maintenance_count from stop_summary where stop_division = '30') as data
    -- 工程の系停止時間
    union 
    select
        4 as sort
        , (select stop_time from stop_summary where stop_division = '30') as data
    -- 系停止区分の条件なしのカウント件数
    union 
    select
        5 as sort
        , (select maintenance_count from stop_summary where stop_division = '99') as data
) 
select
    coalesce(data, 0) as data 
from
    main 
order by
    sort
