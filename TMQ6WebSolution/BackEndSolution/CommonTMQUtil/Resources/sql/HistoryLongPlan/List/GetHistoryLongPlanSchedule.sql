/*
* スケジュールの内容を取得するSQL
*/
WITH long_plan_ids AS (
SELECT
    *
FROM
    STRING_SPLIT(@LongPlanIdList, ',')
)
--トランザクション
SELECT
    lp.long_plan_id AS key_id
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
                AND st_f.factory_id IN(0, lp.location_factory_structure_id)
            )
        AND tra.structure_id = mscn.maintainance_kind_structure_id
    ) AS maintainance_kind_char
    , msd.summary_id
    , msd.maintainance_schedule_detail_id AS new_maintainance_key 
FROM
    ln_long_plan AS lp 
    INNER JOIN mc_management_standards_content AS mscn 
        ON (lp.long_plan_id = mscn.long_plan_id) 
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
    EXISTS(
        SELECT
            *
        FROM
            long_plan_ids temp
        WHERE
            lp.long_plan_id = temp.value
    )
    AND msd.schedule_date IS NOT NULL 
    AND msd.schedule_date BETWEEN @ScheduleStart AND @ScheduleEnd 
    AND NOT EXISTS ( 
        SELECT
            * 
        FROM
            hm_mc_management_standards_content hmsc
        WHERE
            hmsc.execution_division = 5          --保全情報一覧の削除
            AND hmsc.management_standards_content_id = mscn.management_standards_content_id
    )                                           --削除した保全情報一覧の情報は除外する
    
    UNION 

--変更管理
SELECT
    hm.key_id
    , msd.schedule_date
    , msd.complition
    , hmmsc.maintainance_kind_structure_id
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
                    st_f.structure_id = hmmsc.maintainance_kind_structure_id
                AND st_f.factory_id IN(0, COALESCE(hmlp.location_structure_id, lp.location_structure_id))
            )
        AND tra.structure_id = hmmsc.maintainance_kind_structure_id
    ) AS maintainance_kind_char
    , msd.summary_id
    , msd.maintainance_schedule_detail_id AS new_maintainance_key 
FROM
    hm_history_management hm 
    LEFT JOIN hm_ln_long_plan hmlp              -- 長計件名変更管理
        ON hm.key_id = hmlp.long_plan_id 
        AND hm.history_management_id = hmlp.history_management_id 
    LEFT JOIN ln_long_plan lp                   -- 長計件名
        ON hm.key_id = lp.long_plan_id 
    LEFT JOIN hm_mc_management_standards_content hmmsc -- 機器別管理基準内容変更管理
        ON hm.key_id = hmmsc.long_plan_id 
        AND hm.history_management_id = hmmsc.history_management_id 
    LEFT OUTER JOIN mc_maintainance_schedule AS msh 
        ON ( 
            msh.management_standards_content_id = hmmsc.management_standards_content_id
        ) 
    LEFT OUTER JOIN mc_maintainance_schedule_detail AS msd 
        ON ( 
            msd.maintainance_schedule_id = msh.maintainance_schedule_id
        ) 
    LEFT OUTER JOIN v_structure_all AS st 
        ON ( 
            hmmsc.maintainance_kind_structure_id = st.structure_id 
            AND st.structure_group_id = 1240 
            AND st.factory_id IN @FactoryIdList
        ) 
    LEFT OUTER JOIN ms_item_extension AS ie 
        ON ( 
            st.structure_item_id = ie.item_id 
            AND ie.sequence_no = 1
        ) 
WHERE
    EXISTS(
        SELECT
            *
        FROM
            long_plan_ids temp
        WHERE
            hm.key_id = temp.value
    )
    AND msd.schedule_date IS NOT NULL 
    AND msd.schedule_date BETWEEN @ScheduleStart AND @ScheduleEnd 
    AND hmmsc.execution_division != 5             --削除した保全情報一覧の情報は除外する
