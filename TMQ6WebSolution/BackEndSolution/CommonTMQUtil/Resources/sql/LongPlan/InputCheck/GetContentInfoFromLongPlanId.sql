/*
* 機器別管理基準内容の情報を件名別長期計画IDより取得するSQL
*/
SELECT
    con.management_standards_content_id,
    con.maintainance_kind_structure_id
FROM
    mc_management_standards_content AS con
WHERE
    con.long_plan_id = @LongPlanId
