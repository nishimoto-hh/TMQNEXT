/*
* 長期計画IDより、紐づく機器別管理基準を検索し、機番IDを一意に取得するSQL
*/
SELECT DISTINCT
    com.machine_id
FROM
    mc_management_standards_component AS com
WHERE
    EXISTS(
        SELECT
            *
        FROM
            mc_management_standards_content AS con
        WHERE
            com.management_standards_component_id = con.management_standards_component_id
        AND con.long_plan_id = @LongPlanId
    )
