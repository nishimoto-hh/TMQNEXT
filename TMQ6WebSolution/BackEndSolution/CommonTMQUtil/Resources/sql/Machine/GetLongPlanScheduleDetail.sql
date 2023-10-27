SELECT
	 mscn.management_standards_content_id AS key_id  -- 機器別管理基準部位ID(スケジュール紐づけキー)
	,DENSE_RANK() OVER(ORDER BY msc.machine_id, mscn.maintainance_kind_structure_id) AS group_key
	,msd.maintainance_schedule_id
    ,msd.schedule_date
    ,msd.complition
    ,mscn.maintainance_kind_structure_id
    ,ie.extension_data AS maintainance_kind_level
	,dbo.get_translation_text(mscn.maintainance_kind_structure_id,lp.location_structure_id,1240,@LanguageId) AS maintainance_kind_char
    --,st.translation_text AS maintainance_kind_char
	,mscn.long_plan_id
    ,msd.summary_id
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
    LEFT OUTER JOIN
        v_structure_all AS st
    ON  (
            mscn.maintainance_kind_structure_id = st.structure_id
        AND st.structure_group_id = 1240
        AND st.factory_id IN @FactoryIdList
   --     AND st.language_id = @LanguageId
        )
    LEFT OUTER JOIN
        ms_item_extension AS ie
    ON  (
            st.structure_item_id = ie.item_id
        AND ie.sequence_no = 1
        )
    LEFT OUTER JOIN
	   ln_long_plan lp
	ON mscn.long_plan_id = lp.long_plan_id
WHERE
    msd.schedule_date IS NOT NULL
AND msd.schedule_date BETWEEN @ScheduleStart AND @ScheduleEnd
AND msc.machine_id = @MachineId
AND mscn.long_plan_id IS NOT NULL
ORDER BY  mscn.management_standards_component_id  ,msd.schedule_date,ie.extension_data
