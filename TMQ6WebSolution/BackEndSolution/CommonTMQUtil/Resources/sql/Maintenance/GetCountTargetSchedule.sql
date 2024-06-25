WITH complition AS ( 
    --更新対象の保全スケジュールID取得
    SELECT
        maintainance_schedule_id
        , MAX(sequence_count) AS sequence_count 
    FROM
        mc_maintainance_schedule_detail 
    WHERE
        summary_id = @SummaryId 
        AND schedule_date = @ScheduleDate 
        AND complition = @Complition 
    GROUP BY
        maintainance_schedule_id
) 
SELECT
    COUNT(msd.maintainance_schedule_detail_id) 
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
        AND ie.extension_data = '2'             --完了日基準
WHERE
    msd.complition != @Complition 
    AND ms.is_cyclic = 1
