-- 機器別管理基準標準に紐付く機器別管理基準標準詳細を削除する
DELETE 
FROM
    mc_management_standards_detail 
WHERE
    management_standards_id = @ManagementStandardsId
