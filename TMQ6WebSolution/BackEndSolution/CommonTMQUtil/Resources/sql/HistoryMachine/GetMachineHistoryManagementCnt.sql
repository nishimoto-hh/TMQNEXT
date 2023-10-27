SELECT
    history.history_management_id,
    history.update_serialid,
    machine.hm_machine_id,
    machine.machine_id,
    machine.location_structure_id,
    machine.job_structure_id,
    machine.machine_no,
    machine.machine_name,
    machine.installation_location,
    machine.number_of_installation,
    machine.equipment_level_structure_id,
    machine.date_of_installation,
    machine.importance_structure_id,
    machine.conservation_structure_id AS inspection_site_conservation_structure_id,
    machine.machine_note,
    machine.update_serialid AS mc_update_serialid,
    equipment.hm_equipment_id,
    equipment.equipment_id,
    equipment.circulation_target_flg,
    equipment.manufacturer_structure_id,
    equipment.manufacturer_type,
    equipment.model_no,
    equipment.serial_no,
    equipment.date_of_manufacture,
    equipment.delivery_date,
    equipment.equipment_note,
    equipment.use_segment_structure_id,
    equipment.fixed_asset_no,
    equipment.maintainance_kind_manage,
    equipment.update_serialid AS eq_update_serialid,
    laws.applicable_laws_structure_id
FROM
    hm_history_management history
    LEFT JOIN
        hm_mc_machine machine
    ON  history.history_management_id = machine.history_management_id
    LEFT JOIN
        hm_mc_equipment equipment
    ON  history.history_management_id = equipment.history_management_id
    LEFT JOIN
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
        ) laws
    ON  history.history_management_id = laws.history_management_id
WHERE
    machine.history_management_id = @HistoryManagementId
AND machine.execution_division IN(1, 2)