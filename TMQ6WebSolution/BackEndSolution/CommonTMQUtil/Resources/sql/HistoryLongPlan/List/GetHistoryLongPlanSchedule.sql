/*
* スケジュールの内容を取得するSQL
*/

--トランザクション
SELECT
    lp.long_plan_id AS key_id
    , msd.schedule_date
    , msd.complition
    , mscn.maintainance_kind_structure_id
    , ie.extension_data AS maintainance_kind_level
    , dbo.get_translation_text_all( 
        mscn.maintainance_kind_structure_id
        , lp.location_structure_id
        , 1240
        , @LanguageId
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
    msd.schedule_date IS NOT NULL 
    AND msd.schedule_date BETWEEN @ScheduleStart AND @ScheduleEnd 
    AND NOT EXISTS ( 
        SELECT
            * 
        FROM
            hm_history_management_detail hmd 
            LEFT JOIN hm_mc_management_standards_content hmsc 
                ON hmd.history_management_detail_id = hmsc.history_management_detail_id 
        WHERE
            hmd.execution_division = 5          --保全情報一覧の削除
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
    , dbo.get_translation_text_all( 
        hmmsc.maintainance_kind_structure_id
        , COALESCE( 
            hmlp.location_structure_id
            , lp.location_structure_id
        ) 
        , 1240
        , @LanguageId
    ) AS maintainance_kind_char
    , msd.summary_id
    , msd.maintainance_schedule_detail_id AS new_maintainance_key 
FROM
    hm_history_management hm 
    LEFT JOIN hm_history_management_detail hmd 
        ON hm.history_management_id = hmd.history_management_id 
    LEFT JOIN hm_ln_long_plan hmlp              -- 長計件名変更管理
        ON hm.key_id = hmlp.long_plan_id 
        AND hmd.history_management_detail_id = hmlp.history_management_detail_id 
    LEFT JOIN ln_long_plan lp                   -- 長計件名
        ON hm.key_id = lp.long_plan_id 
    LEFT JOIN hm_mc_management_standards_content hmmsc -- 機器別管理基準内容変更管理
        ON hm.key_id = hmmsc.long_plan_id 
        AND hmd.history_management_detail_id = hmmsc.history_management_detail_id 
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
    msd.schedule_date IS NOT NULL 
    AND msd.schedule_date BETWEEN @ScheduleStart AND @ScheduleEnd 
    AND hmd.execution_division != 5             --削除した保全情報一覧の情報は除外する
