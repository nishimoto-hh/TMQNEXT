/*
 * LongPlanId取得用SQL
 */
SELECT DISTINCT
    man_con.long_plan_id                                    -- 長期計画件名ID
FROM
    mc_management_standards_content AS man_con              -- 機器別管理基準内容
    INNER JOIN
        mc_management_standards_component AS man_com        -- 機器別管理基準部位
    ON  (
            -- 機器別管理基準部位ID
            man_con.management_standards_component_id = man_com.management_standards_component_id
        )
    INNER JOIN
        mc_machine AS machine                               -- 機番情報
    ON  (
            man_com.machine_id = machine.machine_id         -- 機番ID
        )
WHERE
    man_con.long_plan_id IS NOT NULL                        -- 長期計画件名ID
AND machine.machine_id IN @MachineIdList                    -- 機番ID
ORDER BY
    man_con.long_plan_id                                    -- 長期計画件名ID