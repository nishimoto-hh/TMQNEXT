SELECT
     CONCAT_WS('|',lp.long_plan_id,machine.machine_id,mscm.management_standards_component_id,mscn.management_standards_content_id,msh.maintainance_schedule_id) AS key_id
    ,msd.schedule_date
    ,msd.complition
    ,mscn.maintainance_kind_structure_id
    ,ie.extension_data AS maintainance_kind_level
    ,dbo.get_translation_text_all(mscn.maintainance_kind_structure_id,lp.location_structure_id,1240,@LanguageId) AS maintainance_kind_char
    ,msd.summary_id
FROM
    ln_long_plan AS lp
    INNER JOIN
        mc_management_standards_content AS mscn
    ON  (
            lp.long_plan_id = mscn.long_plan_id
        )

    INNER JOIN
        mc_management_standards_component AS mscm
    ON  (
            mscm.management_standards_component_id = mscn.management_standards_component_id
        )
    INNER JOIN
        mc_machine AS machine
    ON  (
            machine.machine_id = mscm.machine_id
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
    LEFT OUTER JOIN
        v_structure AS st
    ON  (
            mscn.maintainance_kind_structure_id = st.structure_id
        AND st.structure_group_id = 1240
        AND st.factory_id IN @FactoryIdList
        )
    LEFT OUTER JOIN
        ms_item_extension AS ie
    ON  (
            st.structure_item_id = ie.item_id
        AND ie.sequence_no = 1
        )
WHERE
    msd.schedule_date IS NOT NULL
AND msd.schedule_date BETWEEN @ScheduleStart AND @ScheduleEnd
