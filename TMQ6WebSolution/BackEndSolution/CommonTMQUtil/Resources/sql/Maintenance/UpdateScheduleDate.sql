WITH complition AS ( 
    --更新対象の保全スケジュールID取得
    SELECT
        maintainance_schedule_id
        , MIN(sequence_count) AS sequence_count 
    FROM
        mc_maintainance_schedule_detail 
    WHERE
        summary_id = @SummaryId 
        AND schedule_date = @ScheduleDate 
        AND complition = @Complition 
    GROUP BY
        maintainance_schedule_id
) 
, target_list AS ( 
    --更新対象のレコード取得
    SELECT
        msd.maintainance_schedule_detail_id
        , msd.sequence_count - complition.sequence_count AS sequence_count
        , ms.cycle_year
        , ms.cycle_month
        , ms.cycle_day 
    FROM
        mc_maintainance_schedule_detail msd 
        INNER JOIN complition 
            ON msd.maintainance_schedule_id = complition.maintainance_schedule_id 
            AND msd.sequence_count > complition.sequence_count 
        INNER JOIN mc_maintainance_schedule ms 
            ON msd.maintainance_schedule_id = ms.maintainance_schedule_id 
        INNER JOIN mc_management_standards_content msc 
            ON ms.management_standards_content_id = msc.management_standards_content_id 
        INNER JOIN ms_structure st 
            ON msc.schedule_type_structure_id = st.structure_id 
        INNER JOIN ms_item_extension ie 
            ON st.structure_item_id = ie.item_id 
            AND ie.sequence_no = 1 
            AND ie.extension_data = '2'         --完了日基準
    WHERE
        msd.complition != @Complition 
        AND ms.is_cyclic = 1
) UPDATE mc_maintainance_schedule_detail 
SET
    [schedule_date] = DATEADD( 
        YEAR
        , IsNull(target_list.cycle_year, 0) * target_list.sequence_count
        , DATEADD( 
            MONTH
            , IsNull(target_list.cycle_month, 0) * target_list.sequence_count
            , DATEADD( 
                day
                , IsNull(target_list.cycle_day, 0) * target_list.sequence_count
                , @ScheduleDate
            )
        )
    ) 
    , [update_serialid] = update_serialid + 1
    , [update_datetime] = @UpdateDatetime
    , [update_user_id] = @UpdateUserId 
FROM
    mc_maintainance_schedule_detail msd 
    INNER JOIN target_list 
        ON msd.maintainance_schedule_detail_id = target_list.maintainance_schedule_detail_id
