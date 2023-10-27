SELECT
	 COUNT(msd.schedule_date)
FROM
    mc_management_standards_component AS msc
    INNER JOIN
        mc_management_standards_content AS mscn
    ON  (
            msc.management_standards_component_id = mscn.management_standards_component_id
        )
    LEFT OUTER JOIN
        mc_maintainance_schedule AS msh
    ON  (
            msh.management_standards_content_id = mscn.management_standards_content_id
        )
    LEFT OUTER JOIN
        mc_maintainance_schedule_detail AS msd
    ON  (
            msd.maintainance_schedule_id = msh.maintainance_schedule_id
        )
WHERE
    msd.schedule_date IS NOT NULL
AND msd.schedule_date > @StartDate
AND msc.machine_id = @MachineId
AND mscn.maintainance_kind_structure_id = @MaintainanceKindStructureId
AND msd.summary_id IS NOT NULL
