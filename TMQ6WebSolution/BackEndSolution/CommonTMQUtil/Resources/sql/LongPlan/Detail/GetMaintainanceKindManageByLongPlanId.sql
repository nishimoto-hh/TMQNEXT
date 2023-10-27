/*
* 長期計画に紐づく機器の点検種別毎管理を取得するSQL
* 長期計画内で1種類のはずだが、有→無の順に取得する
*/
SELECT
    TOP 1 equip.maintainance_kind_manage
FROM
    mc_management_standards_content AS con
    INNER JOIN
        mc_management_standards_component AS com
    ON  (
            con.management_standards_component_id = com.management_standards_component_id
        )
    INNER JOIN
        mc_machine AS machine
    ON  (
            com.machine_id = machine.machine_id
        )
    INNER JOIN
        mc_equipment AS equip
    ON  (
            machine.machine_id = equip.machine_id
        )
WHERE
    con.long_plan_id = @LongPlanId
ORDER BY
    equip.maintainance_kind_manage DESC
