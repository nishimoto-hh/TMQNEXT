WITH -- 保全スケジュールを機器別管理基準内容IDごとに取得(同じ値なら最大の開始日のレコード)
schedule_start_date AS(
    SELECT
	    sc.maintainance_schedule_id,
        sc.management_standards_content_id,
        sc.cycle_year,
        sc.cycle_month,
        sc.cycle_day,
        sc.start_date,
		sc.update_datetime
    FROM
        mc_maintainance_schedule AS sc
    INNER JOIN mc_management_standards_content cont 
        ON sc.management_standards_content_id = cont.management_standards_content_id 
    INNER JOIN mc_management_standards_component comp 
        ON cont.management_standards_component_id = comp.management_standards_component_id 
        AND comp.machine_id = @MachineId
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
)

SELECT
    cont.management_standards_content_id
    , ms.maintainance_schedule_id
    , ms.start_date
FROM
    mc_management_standards_component comp 
    LEFT JOIN mc_management_standards_content cont 
        ON comp.management_standards_component_id = cont.management_standards_component_id 
    LEFT JOIN schedule_content ms
        ON cont.management_standards_content_id = ms.management_standards_content_id 
WHERE
    comp.is_management_standard_conponent = 1 
    AND comp.machine_id = @MachineId 
    AND COALESCE(cont.maintainance_kind_structure_id, 0) = COALESCE(@MaintainanceKindStructureId, 0) 
    AND ( 
        isnull(ms.cycle_year, 0) <> isnull(@CycleYear, 0) 
        OR isnull(ms.cycle_month, 0) <> isnull(@CycleMonth, 0) 
        OR isnull(ms.cycle_day, 0) <> isnull(@CycleDay, 0) 
        OR ms.start_date <> @StartDate
    )
