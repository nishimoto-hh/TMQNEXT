SELECT
    CASE
        -- ã@äÌë‰í†(HM0001)ÇÃèÍçá
        WHEN @ApplicationConductId = 1 THEN(
            SELECT
                ex.extension_data
            FROM
                ms_structure ms
                LEFT JOIN
                    ms_item_extension ex
                ON  ms.structure_item_id = ex.item_id
                AND ex.sequence_no = 4
            WHERE
                ms.structure_group_id = 1000
            AND ms.structure_layer_no = 1
            AND ms.structure_id = (
                    SELECT
                        dbo.get_target_layer_id(coalesce(machine.location_structure_id, old_machine.location_structure_id), 1)
                    FROM
                        hm_history_management history
                        LEFT JOIN
                            hm_history_management_detail detail
                        ON  history.history_management_id = detail.history_management_id
                        LEFT JOIN
                            hm_mc_machine machine
                        ON  detail.history_management_detail_id = machine.history_management_detail_id
                        LEFT JOIN
                            hm_mc_management_standards_component component
                        ON  detail.history_management_detail_id = component.history_management_detail_id
                        LEFT JOIN
                            mc_machine old_machine
                        ON  component.machine_id = old_machine.machine_id
                    WHERE
                        history.history_management_id = @HistoryManagementId
                )
        )
        -- í∑ä˙åvâÊÇÃèÍçá(HM0002)
        WHEN @ApplicationConductId = 2 THEN(





            SELECT
                1





        )
        ELSE 0
    END AS ex_data