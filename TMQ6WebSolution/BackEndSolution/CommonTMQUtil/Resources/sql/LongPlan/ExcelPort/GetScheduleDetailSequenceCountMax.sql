/*
 * 長期計画 機器別管理基準 ExcelPortアップロード　保全スケジュールの繰り返し回数の最大値を取得する
 */
SELECT
   COALESCE(MAX(sequence_count), 0) AS SequenseCountMax
FROM
    mc_maintainance_schedule_detail

WHERE
    maintainance_schedule_id = @MaintainanceScheduleId
 AND
     FORMAT(schedule_date, 'yyyyMM') < @ScheduleDate