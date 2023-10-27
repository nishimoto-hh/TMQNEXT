/*
 * 機器別管理基準内容に紐づいたスケジュールに保全活動が作成済みの件数を取得するSQL
*/
SELECT
    COUNT(sc_d.maintainance_schedule_detail_id)
FROM
    mc_maintainance_schedule_detail AS sc_d
    INNER JOIN
        mc_maintainance_schedule AS sc
    ON  (
            sc.maintainance_schedule_id = sc_d.maintainance_schedule_id
        )
WHERE
    sc_d.summary_id IS NOT NULL
AND sc.management_standards_content_id = @ManagementStandardsContentId
