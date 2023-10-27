SELECT
    *
FROM
    (
        SELECT
            history.history_management_id,
            machine.execution_division,
            history.key_id AS machine_id,
            machine.hm_machine_id,
            equipment.hm_equipment_id,
            equipment.equipment_id,
            laws.applicable_laws_structure_id,
            0 AS hm_management_standards_component_id,
            0 AS hm_management_standards_content_id,
            0 AS hm_maintainance_schedule_id,
            0 AS management_standards_component_id,
            0 AS management_standards_content_id,
            0 AS maintainance_schedule_id,
            history.update_datetime
        FROM
            hm_history_management history
            LEFT JOIN
                hm_mc_machine machine
            ON  history.history_management_id = machine.history_management_id
            LEFT JOIN
                hm_mc_equipment equipment
            ON  history.history_management_id = equipment.history_management_id
                -- 変更管理詳細に紐付く適用法規
            LEFT JOIN
                (
                    SELECT
                        tbl.history_management_id,
                        tbl.applicable_laws_structure_id
                    FROM
                        (
                            SELECT
                                history.history_management_id,
                                trim(
                                    '|'
                                    FROM
                                        (
                                            SELECT
                                                cast(laws.applicable_laws_structure_id AS varchar) + '|'
                                            FROM
                                                hm_mc_applicable_laws laws
                                            WHERE
                                                history.history_management_id = laws.history_management_id FOR XML PATH('')
                                        )
                                ) AS applicable_laws_structure_id
                            FROM
                                hm_history_management history
                            GROUP BY
                                history.history_management_id
                        ) AS tbl
                    WHERE
                        tbl.applicable_laws_structure_id IS NOT NULL
                ) laws
            ON  history.history_management_id = laws.history_management_id
        WHERE
            history.history_management_id = @HistoryManagementId
        AND machine.execution_division IN(1, 2, 3)
        UNION ALL
        SELECT
            history.history_management_id,
            content.execution_division,
            history.key_id AS machine_id,
            hmachine.hm_machine_id,
            hequipment.hm_equipment_id,
            equipment.equipment_id,
            '0' AS applicable_laws_structure_id,
            component.hm_management_standards_component_id,
            content.hm_management_standards_content_id,
            schedule.hm_maintainance_schedule_id,
            component.management_standards_component_id,
            content.management_standards_content_id,
            schedule.maintainance_schedule_id,
            history.update_datetime
        FROM
            hm_history_management history
        LEFT JOIN
            hm_mc_management_standards_component component
        ON  history.history_management_id = component.history_management_id
        LEFT JOIN
            hm_mc_management_standards_content content
        ON  component.management_standards_component_id = content.management_standards_component_id
        AND history.history_management_id = content.history_management_id
        LEFT JOIN
            hm_mc_maintainance_schedule schedule
        ON  content.management_standards_content_id = schedule.management_standards_content_id
        AND history.history_management_id = schedule.history_management_id
        LEFT JOIN
            hm_mc_machine hmachine
        ON  history.history_management_id = hmachine.history_management_id
        LEFT JOIN
            hm_mc_equipment hequipment
        ON  history.history_management_id = hequipment.history_management_id
        LEFT JOIN
            mc_machine machine
        ON  history.key_id = machine.machine_id
        LEFT JOIN
            mc_equipment equipment
        ON  machine.machine_id = equipment.machine_id
        WHERE
            history.history_management_id = @HistoryManagementId
        AND content.execution_division IN(4, 5, 6)
    ) tbl
ORDER BY
    tbl.update_datetime