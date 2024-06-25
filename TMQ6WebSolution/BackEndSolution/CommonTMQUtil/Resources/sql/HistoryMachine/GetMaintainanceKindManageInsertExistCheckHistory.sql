WITH 
-- 保全スケジュールを機器別管理基準内容IDごとに取得(同じ値なら最大の開始日のレコード)
schedule_start_date AS(
    SELECT
	    sc.maintainance_schedule_id,
        sc.management_standards_content_id,
        sc.cycle_year,
        sc.cycle_month,
        sc.cycle_day,
        sc.start_date,
		sc.is_cyclic,
		sc.disp_cycle,
		sc.update_datetime
    FROM
        mc_maintainance_schedule AS sc
    WHERE
        NOT EXISTS(
            SELECT
                *
            FROM
                mc_maintainance_schedule AS sub
            WHERE
                sc.management_standards_content_id = sub.management_standards_content_id
            AND sc.start_date < sub.start_date
        )
),
-- 上で取得した保全スケジュールを機器別管理基準内容ID、開始日ごとに取得(同じ値なら最大の更新日時のレコード)
schedule_content AS(
    SELECT
	    main.maintainance_schedule_id,
        main.management_standards_content_id,
        main.cycle_year,
        main.cycle_month,
        main.cycle_day,
        main.start_date,
		main.is_cyclic,
		main.disp_cycle,
		main.update_datetime
    FROM
        schedule_start_date AS main
    WHERE
        NOT EXISTS(
            SELECT
                *
            FROM
                schedule_start_date AS sub
            WHERE
                main.management_standards_content_id = sub.management_standards_content_id
            AND main.start_date = sub.start_date
            AND main.update_datetime < sub.update_datetime
        )
),
-- 保全スケジュール(変更管理)を機器別管理基準内容IDごとに取得(同じ値なら最大の開始日のレコード)
schedule_start_date_history AS(
    SELECT
        sc.hm_maintainance_schedule_id,
        sc.history_management_id,
	    sc.maintainance_schedule_id,
        sc.management_standards_content_id,
        sc.cycle_year,
        sc.cycle_month,
        sc.cycle_day,
        sc.start_date,
		sc.is_cyclic,
		sc.disp_cycle,
		sc.update_datetime,
        sc.next_schedule_date,
        sc.is_update_schedule
    FROM
        hm_mc_maintainance_schedule AS sc
    WHERE
        NOT EXISTS(
            SELECT
                *
            FROM
                hm_mc_maintainance_schedule AS sub
            WHERE
                sc.management_standards_content_id = sub.management_standards_content_id
            AND sc.start_date < sub.start_date
            AND sc.history_management_id = @HistoryManagementId
        )
        AND sc.history_management_id = @HistoryManagementId
),
-- 上で取得した保全スケジュール(変更管理)を機器別管理基準内容ID、開始日ごとに取得(同じ値なら最大の更新日時のレコード)
schedule_content_history AS(
    SELECT
        main.hm_maintainance_schedule_id,
        main.history_management_id,
	    main.maintainance_schedule_id,
        main.management_standards_content_id,
        main.cycle_year,
        main.cycle_month,
        main.cycle_day,
        main.start_date,
		main.is_cyclic,
		main.disp_cycle,
		main.update_datetime,
        main.next_schedule_date,
        main.is_update_schedule
    FROM
        schedule_start_date_history AS main
    WHERE
        NOT EXISTS(
            SELECT
                *
            FROM
                schedule_start_date_history AS sub
            WHERE
                main.management_standards_content_id = sub.management_standards_content_id
            AND main.start_date = sub.start_date
            AND main.update_datetime < sub.update_datetime
        )
),
-- 上で取得した保全スケジュール(変更管理)を機器別管理基準内容ID、開始日、更新日時ごとに取得(同じ値なら最大の保全スケジュール変更管理IDのレコード)
-- 削除申請されたデータは開始日と更新日時が同一のデータが存在するため必要
schedule AS(
    SELECT
        main.hm_maintainance_schedule_id,
        main.history_management_id,
	    main.maintainance_schedule_id,
        main.management_standards_content_id,
        main.cycle_year,
        main.cycle_month,
        main.cycle_day,
        main.start_date,
		main.is_cyclic,
		main.disp_cycle,
		main.update_datetime,
        main.next_schedule_date,
        main.is_update_schedule
    FROM
        schedule_content_history AS main
    WHERE
        NOT EXISTS(
            SELECT
                *
            FROM
                schedule_content_history AS sub
            WHERE
                main.management_standards_content_id = sub.management_standards_content_id
            AND main.start_date = sub.start_date
            AND main.update_datetime = sub.update_datetime
            AND main.hm_maintainance_schedule_id < sub.hm_maintainance_schedule_id
        )
),
trans AS(
    SELECT
        mcp.management_standards_component_id,
        msc.management_standards_content_id,
        msc.maintainance_kind_structure_id,
        ms.cycle_year,
        ms.cycle_month,
        ms.cycle_day,
        ms.start_date,
        0 AS execution_division
    FROM
        mc_management_standards_component mcp -- 機器別管理基準部位
        , mc_management_standards_content msc -- 機器別管理基準内容
        , schedule_content ms, -- 保全スケジュール
        mc_machine ma
    WHERE
        mcp.management_standards_component_id = msc.management_standards_component_id
    AND msc.management_standards_content_id = ms.management_standards_content_id
    AND mcp.machine_id = ma.machine_id
    AND mcp.is_management_standard_conponent = 1 -- 機器別管理基準フラグ
    AND mcp.machine_id = @MachineId
),
history AS(
    SELECT
        mcp.management_standards_component_id,
        msc.management_standards_content_id,
        msc.maintainance_kind_structure_id,
        ms.cycle_year,
        ms.cycle_month,
        ms.cycle_day,
        ms.start_date,
        msc.execution_division
    FROM
        hm_history_management history,
        hm_mc_management_standards_component mcp -- 機器別管理基準部位
        , hm_mc_management_standards_content msc -- 機器別管理基準内容
        , schedule ms
    WHERE
        history.history_management_id = mcp.history_management_id
    AND mcp.management_standards_component_id = msc.management_standards_component_id
    AND msc.management_standards_content_id = ms.management_standards_content_id
    AND mcp.history_management_id = msc.history_management_id
    AND mcp.is_management_standard_conponent = 1 -- 機器別管理基準フラグ
    AND history.history_management_id = @HistoryManagementId
),
new_data AS(
    SELECT
        trans.management_standards_component_id,
        trans.management_standards_content_id,
        coalesce(history.maintainance_kind_structure_id, trans.maintainance_kind_structure_id) AS maintainance_kind_structure_id,
        coalesce(history.cycle_year, trans.cycle_year) AS cycle_year,
        coalesce(history.cycle_month, trans.cycle_month) AS cycle_month,
        coalesce(history.cycle_day, trans.cycle_day) AS cycle_day,
        coalesce(history.start_date, trans.start_date) AS start_date,
        coalesce(history.execution_division, trans.execution_division) AS execution_division
    FROM
        trans
        LEFT JOIN
            history
        ON  trans.management_standards_component_id = history.management_standards_component_id
        AND trans.management_standards_content_id = history.management_standards_content_id
    -- 行追加されたデータは↑のSQLでは取得できないのでUNIONする
    UNION
    SELECT
        history.management_standards_component_id,
        history.management_standards_content_id,
        history.maintainance_kind_structure_id,
        history.cycle_year,
        history.cycle_month,
        history.cycle_day,
        history.start_date,
        history.execution_division
    FROM
        history
)
SELECT
    COUNT(*)
FROM
    new_data
WHERE
    new_data.execution_division <> 6 -- 削除行以外
AND new_data.maintainance_kind_structure_id = @MaintainanceKindStructureId -- 同一点検種別
AND (
        ISNULL(new_data.cycle_year, 0) <> ISNULL(@CycleYear, 0)
    OR  ISNULL(new_data.cycle_month, 0) <> ISNULL(@CycleMonth, 0)
    OR  ISNULL(new_data.cycle_day, 0) <> ISNULL(@CycleDay, 0)
    OR  new_data.start_date <> @StartDate
    )