/*
 * 長期計画 機器別管理基準 ExcelPortダウンロード　保全項目に紐づく保全スケジュール詳細情報を取得する
 */
-- 保全項目ID
WITH management_standards_content_ids AS (
SELECT
    *
FROM
    STRING_SPLIT(@ManagementStandardsContentIdList, ',')
)

SELECT 
     con.management_standards_content_id
    ,ms.maintainance_schedule_id
    ,ms.start_date
    
FROM 
-- 機器別管理基準内容
    mc_management_standards_content con

-- 保全スケジュール
INNER JOIN mc_maintainance_schedule ms
ON con.management_standards_content_id = ms.management_standards_content_id

WHERE
    EXISTS(
        SELECT * FROM management_standards_content_ids temp
        WHERE con.management_standards_content_id = temp.value)
    
ORDER BY 
      con.management_standards_content_id
     ,ms.start_date
