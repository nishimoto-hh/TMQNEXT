/*
 * 件名別長期計画一覧　参照画面　保全情報一覧のスケジュールの検索　機器
*/
-- WITH句(GetDetailList_With)の続き
-- 機器で集計
,
base_group AS(
    SELECT
        base.machine_id
    FROM
        base
    GROUP BY
        base.machine_id
)
-- GetDetailListのSQLと同じキーの単位で取得する
SELECT
    machine.machine_id as key_id
    , msd.schedule_date
    , msd.complition
    , mscn.maintainance_kind_structure_id
    , ie.extension_data AS maintainance_kind_level
    , (
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
                AND st_f.factory_id IN(0, machine.location_factory_structure_id)
            )
        AND tra.structure_id = mscn.maintainance_kind_structure_id
    ) AS maintainance_kind_char
    , msd.summary_id
    , msd.maintainance_schedule_detail_id AS new_maintainance_key 
FROM
    base_group 
    INNER JOIN machine 
        ON (machine.machine_id = base_group.machine_id) 
    INNER JOIN ( 
        SELECT
            org.management_standards_content_id
            , COALESCE(hmmsc.management_standards_component_id, org.management_standards_component_id) AS management_standards_component_id
            , COALESCE(hmmsc.maintainance_kind_structure_id, org.maintainance_kind_structure_id) AS maintainance_kind_structure_id 
        FROM
            mc_management_standards_content org 
            LEFT JOIN hm_mc_management_standards_content hmmsc 
                ON org.management_standards_content_id = hmmsc.management_standards_content_id
    ) AS mscn 
        ON ( 
            mscn.management_standards_component_id IN ( 
                SELECT
                    base.management_standards_component_id 
                FROM
                    base 
                WHERE
                    base.machine_id = machine.machine_id
            )
        ) 
    LEFT OUTER JOIN mc_maintainance_schedule AS msh 
        ON ( 
            msh.management_standards_content_id = mscn.management_standards_content_id
        ) 
    LEFT OUTER JOIN mc_maintainance_schedule_detail AS msd 
        ON ( 
            msd.maintainance_schedule_id = msh.maintainance_schedule_id
        ) 
    LEFT OUTER JOIN v_structure_all AS st 
        ON ( 
            mscn.maintainance_kind_structure_id = st.structure_id 
            AND st.structure_group_id = 1240 
            AND st.factory_id IN @FactoryIdList
        ) 
    LEFT OUTER JOIN ms_item_extension AS ie 
        ON ( 
            st.structure_item_id = ie.item_id 
            AND ie.sequence_no = 1
        ) 
WHERE
    msd.schedule_date IS NOT NULL 
    AND msd.schedule_date BETWEEN @ScheduleStart AND @ScheduleEnd
