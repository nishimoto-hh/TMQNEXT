with mq as ( 
    -- 標準工場(工場ID=0)のMQ分類を取得
    select
        ms.structure_id 
    from
        ms_structure ms 
    where
        ms.structure_group_id = 1850
        and ms.factory_id = 0

    -- MQ分類が空のデータ用の構成ID
        union all 
    select
        - 1
) 
, sort as ( 
    -- 標準工場(工場ID=0)のMQ分類の並び順を取得
    select
        mso.structure_id
        , mso.display_order 
    from
        ms_structure_order mso 
    where
        mso.structure_group_id = 1850 
        and mso.factory_id = 0
        
    -- MQ分類が空のデータ用の並び順
        union all 
    select
        - 1 as structure_id
        , - 1 as display_order
) 
, summary as ( 
    -- MQ分類ごとの指定された項目の合計値を取得
    select
        summary.mq_class_structure_id

        /*@MaintenanceCount
        -- カウント件数
        , sum(summary.maintenance_count) as data
　　    @MaintenanceCount*/

        /*@Expenditure
        -- 実績金額
        , sum(history.expenditure) as data
　　    @Expenditure*/

        /*@WorkingTimeSelf
        -- 作業時間(自係)
        , sum(history.working_time_self) as data
　　    @WorkingTimeSelf*/

        /*@WorkingTimeCompany
        -- 作業時間(施工会社)
        , sum(history.working_time_company) as data
　　    @WorkingTimeCompany*/

        /*@WorkingTimeSelfAndCompany
        -- 作業時間(自係) + 作業時間(施工会社)
        , sum(isnull(history.working_time_self, 0) + isnull(history.working_time_company, 0)) as data
　　    @WorkingTimeSelfAndCompany*/

    from
        ma_summary summary

    -- 抽出対象の職種IDが格納されている一時テーブルと内部結合をする
    inner join #temp_job_id tj
        on summary.job_structure_id = tj.job_structure_id

    -- 場所階層ツリーで選択されている項目を抽出条件とするため、ツリーで選択された項目の構成IDが格納されている一時テーブルと内部結合をする
    inner join #temp_location tl
        on summary.location_structure_id = tl.structure_id

    left join ma_history history
        on summary.summary_id = history.summary_id

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
        summary.mq_class_structure_id

    -- MQ分類が空のデータ
    union all 
    select
        - 1 as mq_class_structure_id

        /*@MaintenanceCount
        -- カウント件数
        , sum(summary.maintenance_count) as data
　　    @MaintenanceCount*/

        /*@Expenditure
        -- 実績金額
        , sum(history.expenditure) as data
　　    @Expenditure*/

        /*@WorkingTimeSelf
        -- 作業時間(自係)
        , sum(history.working_time_self) as data
　　    @WorkingTimeSelf*/

        /*@WorkingTimeCompany
        -- 作業時間(施工会社)
        , sum(history.working_time_company) as data
　　    @WorkingTimeCompany*/

        /*@WorkingTimeSelfAndCompany
        -- 作業時間(自係) + 作業時間(施工会社)
        , sum(history.working_time_self + history.working_time_company) as data
　　    @WorkingTimeSelfAndCompany*/

    from
        ma_summary summary

    -- 抽出対象の職種IDが格納されている一時テーブルと内部結合をする
    inner join #temp_job_id tj
        on summary.job_structure_id = tj.job_structure_id

    -- 場所階層ツリーで選択されている項目を抽出条件とするため、ツリーで選択された項目の構成IDが格納されている一時テーブルと内部結合をする
    inner join #temp_location tl
        on summary.location_structure_id = tl.structure_id

    left join ma_history history
        on summary.summary_id = history.summary_id

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

        and summary.mq_class_structure_id is null
) 
select
    mq.structure_id                             -- MQ分類の構成ID
    , coalesce(summary.data, 0) as data           -- MQ分類ごとの件数
from
    mq 
    left join sort 
        on mq.structure_id = sort.structure_id 
    left join summary 
        on mq.structure_id = summary.mq_class_structure_id 
order by
    sort.display_order
