/*
 * 長期計画 機器別管理基準 ExcelPortアップロード　保全項目に紐づく完了済みの保全スケジュール詳細情報を取得する
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
    ,md.maintainance_schedule_detail_id
    ,md.schedule_date
    ,5 as schedule_id   -- 保全履歴完了
    
FROM 
-- 機器別管理基準部位
    mc_management_standards_component com

-- 機器別管理基準内容
INNER JOIN mc_management_standards_content con
ON com.management_standards_component_id = con.management_standards_component_id

-- 保全スケジュール
INNER JOIN mc_maintainance_schedule ms
ON con.management_standards_content_id = ms.management_standards_content_id

-- 保全スケジュール詳細
INNER JOIN mc_maintainance_schedule_detail md
ON ms.maintainance_schedule_id = md.maintainance_schedule_id

WHERE
    EXISTS(
        SELECT * FROM management_standards_content_ids temp
        WHERE con.management_standards_content_id = temp.value)

AND
    FORMAT(md.schedule_date, 'yyyyMM') BETWEEN @ScheduleDateFrom AND @ScheduleDateTo

AND
    com.is_management_standard_conponent = 1    -- 機器別管理基準フラグ

AND
    md.complition = 1   -- 完了フラグ

ORDER BY 
      con.management_standards_content_id
     ,ms.maintainance_schedule_id
     ,md.schedule_date
