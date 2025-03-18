WITH schedule_start_date AS ( -- 保全スケジュールを機器別管理基準内容IDごとに取得(同じ値なら最大の開始日のレコード)
    SELECT
        sc.maintainance_schedule_id
        , sc.management_standards_content_id
        , sc.cycle_year
        , sc.cycle_month
        , sc.cycle_day
        , sc.start_date
        , sc.disp_cycle
        , sc.update_datetime 
    FROM
        mc_maintainance_schedule AS sc 
    WHERE
        NOT EXISTS ( 
            SELECT
                * 
            FROM
                mc_maintainance_schedule AS sub 
            WHERE
                sc.management_standards_content_id = sub.management_standards_content_id 
                AND sc.start_date < sub.start_date
        )
)
, -- 上で取得した保全スケジュールを機器別管理基準内容ID、開始日ごとに取得(同じ値なら最大の更新日時のレコード)
schedule AS ( 
    SELECT
        main.maintainance_schedule_id
        , main.management_standards_content_id
        , main.cycle_year
        , main.cycle_month
        , main.cycle_day
        , main.start_date
        , main.disp_cycle 
    FROM
        schedule_start_date AS main 
    WHERE
        NOT EXISTS ( 
            SELECT
                * 
            FROM
                schedule_start_date AS sub 
            WHERE
                main.management_standards_content_id = sub.management_standards_content_id 
                AND main.start_date = sub.start_date 
                AND main.update_datetime < sub.update_datetime
        )
) 
SELECT
     CONCAT_WS('|',lp.long_plan_id,machine.machine_id,mscm.management_standards_component_id,mscn.management_standards_content_id,msh.maintainance_schedule_id) AS key_id
    ,msd.schedule_date
    ,msd.complition
    ,mscn.maintainance_kind_structure_id
    ,ie.extension_data AS maintainance_kind_level
--    st.translation_text AS maintainance_kind_char,
    ,'' AS maintainance_kind_char
    ,msd.summary_id
--    , 
    /* スケジュールマークグループ用 */
--    CONCAT_WS('|', machine.machine_id, ie.extension_data) AS same_mark_key 
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
    INNER JOIN
        #temp temp                                          -- 一時テーブル（一覧画面にて選択）
    ON (
            machine.machine_id = temp.Key1                  -- 機番ID
        )
    LEFT OUTER JOIN
        schedule AS msh
    ON  (
            msh.management_standards_content_id = mscn.management_standards_content_id
        )
    LEFT OUTER JOIN
        mc_maintainance_schedule_detail AS msd
    ON  (
            msd.maintainance_schedule_id = msh.maintainance_schedule_id
        )
--     LEFT OUTER JOIN
--         v_structure_item AS st
--     ON  (
--             mscn.maintainance_kind_structure_id = st.structure_id
--         AND st.structure_group_id = 1240
--         AND st.factory_id IN @FactoryIdList
--         AND st.language_id = @LanguageId
--         )
    LEFT OUTER JOIN
		ms_structure st
	ON  (
			mscn.maintainance_kind_structure_id = st.structure_id
		AND st.structure_group_id = 1240
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
