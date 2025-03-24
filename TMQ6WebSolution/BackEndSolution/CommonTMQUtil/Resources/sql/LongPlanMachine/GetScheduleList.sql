/*
* スケジュールの内容を取得するSQL
*/
WITH key_ids AS (
SELECT
    *
FROM
    STRING_SPLIT(@KeyIdList, ',')
)
SELECT
     CONCAT_WS('|',lp.long_plan_id,machine.machine_id,mscm.management_standards_component_id,mscn.management_standards_content_id) AS key_id
    ,msd.schedule_date
    ,msd.complition
    ,mscn.maintainance_kind_structure_id
    ,ie.extension_data AS maintainance_kind_level
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = mscn.maintainance_kind_structure_id
                AND st_f.factory_id IN(0, lp.location_factory_structure_id)
            )
        AND tra.structure_id = mscn.maintainance_kind_structure_id
    ) AS maintainance_kind_char
    ,msd.summary_id
    ,msd.maintainance_schedule_detail_id AS new_maintainance_key
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
    EXISTS(
        SELECT
            *
        FROM
            key_ids temp
        WHERE
            CONCAT_WS('|',lp.long_plan_id,machine.machine_id,mscm.management_standards_component_id,mscn.management_standards_content_id) = temp.value
    )
AND msd.schedule_date IS NOT NULL
AND msd.schedule_date BETWEEN @ScheduleStart AND @ScheduleEnd
