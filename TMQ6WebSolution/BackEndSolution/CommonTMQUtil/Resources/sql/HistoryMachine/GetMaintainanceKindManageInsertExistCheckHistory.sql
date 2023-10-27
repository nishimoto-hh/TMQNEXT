WITH trans AS(
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
        ,
        (
            SELECT
                a.*
            FROM
                mc_maintainance_schedule AS a -- 保全スケジュール
                INNER JOIN
                    -- 機器別管理基準内容IDごとの開始日最新データを取得
                    (
                        SELECT
                            management_standards_content_id,
                            MAX(start_date) AS start_date,
                            MAX(update_datetime) AS update_datetime
                        FROM
                            mc_maintainance_schedule
                        GROUP BY
                            management_standards_content_id
                    ) b
                ON  a.management_standards_content_id = b.management_standards_content_id
                AND (
                        a.start_date = b.start_date
                    OR  a.start_date IS NULL
                    AND b.start_date IS NULL
                        --null結合を考慮
                    )
                AND (
                        a.update_datetime = b.update_datetime
                    OR  a.update_datetime IS NULL
                    AND b.update_datetime IS NULL
                        --null結合を考慮
                    )
        ) ms, -- 保全スケジュール
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
        ,
        (
            SELECT
                a.*
            FROM
                hm_mc_maintainance_schedule AS a -- 保全スケジュール
                INNER JOIN
                    -- 機器別管理基準内容IDごとの開始日最新データを取得
                    (
                        SELECT
                            management_standards_content_id,
                            MAX(start_date) AS start_date,
                            MAX(update_datetime) AS update_datetime
                        FROM
                            hm_mc_maintainance_schedule
                        GROUP BY
                            management_standards_content_id
                    ) b
                ON  a.management_standards_content_id = b.management_standards_content_id
                AND (
                        a.start_date = b.start_date
                    OR  a.start_date IS NULL
                    AND b.start_date IS NULL
                        --null結合を考慮
                    )
                AND (
                        a.update_datetime = b.update_datetime
                    OR  a.update_datetime IS NULL
                    AND b.update_datetime IS NULL
                        --null結合を考慮
                    )
        ) ms
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