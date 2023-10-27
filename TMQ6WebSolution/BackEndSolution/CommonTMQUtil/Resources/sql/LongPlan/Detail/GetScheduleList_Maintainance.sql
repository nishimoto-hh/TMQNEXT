/*
 * 件名別長期計画一覧　参照画面　保全情報のスケジュール一覧の検索　保全項目
*/
-- WITH句(GetDetailList_With)の続き

-- GetDetailListのSQLと同じキーの単位で取得する
SELECT
    CONCAT_WS('|',machine.machine_id, man_com.management_standards_component_id,man_con.management_standards_content_id) AS key_id,
    -- 機器、点検種別でグループとなる
    DENSE_RANK() OVER(ORDER BY machine.machine_id) AS group_key,
    machine.machine_id,
    man_com.management_standards_component_id,
    man_con.management_standards_content_id,
    msd.schedule_date,
    msd.complition,
    mscn.maintainance_kind_structure_id,
    ie.extension_data AS maintainance_kind_level,
    dbo.get_translation_text_all(mscn.maintainance_kind_structure_id,machine.location_structure_id,1240,@LanguageId) AS maintainance_kind_char,
    msd.summary_id,
    -- スケジュールマークグループ用
    CONCAT_WS('|',machine.machine_id,ie.extension_data ) AS same_mark_key,
    msd.maintainance_schedule_detail_id AS new_maintainance_key
FROM
    base
    INNER JOIN
        machine
    ON  (
            machine.machine_id = base.machine_id
        )
    INNER JOIN
        man_com
    ON  (
            man_com.management_standards_component_id = base.management_standards_component_id
        )
    INNER JOIN
        man_con
    ON  (
            man_con.management_standards_content_id = base.management_standards_content_id
        )
    INNER JOIN
        mc_management_standards_content AS mscn
    ON  (
            man_con.management_standards_content_id = mscn.management_standards_content_id
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
ORDER BY
    machine.machine_name
   ,machine.machine_id
   ,man_con.kind_order
   ,man_com.inspection_site_structure_id
   ,man_com.management_standards_component_id
   ,man_con.inspection_content_structure_id
   ,man_con.management_standards_content_id
