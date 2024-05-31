-- クリックされた○リンクの保全スケジュール詳細IDから遡り、機器が点検種別毎管理かどうか取得する
SELECT
    equipment.* 
FROM
    mc_machine machine 
    LEFT JOIN mc_equipment equipment 
        ON machine.machine_id = equipment.machine_id 
WHERE
    machine.machine_id = ( 
        SELECT
            component.machine_id 
        FROM
            mc_management_standards_component component 
        WHERE
            component.management_standards_component_id = ( 
                SELECT
                    content.management_standards_component_id 
                FROM
                    mc_management_standards_content content 
                WHERE
                    content.management_standards_content_id = ( 
                        SELECT
                            schedule.management_standards_content_id 
                        FROM
                            mc_maintainance_schedule schedule 
                        WHERE
                            schedule.maintainance_schedule_id = ( 
                                SELECT
                                    detail.maintainance_schedule_id 
                                FROM
                                    mc_maintainance_schedule_detail detail 
                                WHERE
                                    detail.maintainance_schedule_detail_id = @MaintainanceScheduleDetailId
                            )
                    )
            )
    )
