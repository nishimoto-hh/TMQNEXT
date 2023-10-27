WITH trans AS(
    SELECT
        mcp.management_standards_component_id,
        msc.management_standards_content_id,
        mcp.inspection_site_structure_id,
        msc.inspection_content_structure_id,
        0 AS execution_division
    FROM
        mc_management_standards_component mcp -- 機器別管理基準部位
        ,mc_management_standards_content msc -- 機器別管理基準内容
    WHERE
        mcp.management_standards_component_id = msc.management_standards_component_id
    AND mcp.is_management_standard_conponent = 1 -- 機器別管理基準フラグ
    AND mcp.machine_id = @MachineId -- 機番ID
),
history AS(
    SELECT
        component.management_standards_component_id,
        content.management_standards_content_id,
        component.inspection_site_structure_id,
        content.inspection_content_structure_id,
        content.execution_division
    FROM
        hm_mc_management_standards_component component
        LEFT JOIN
            hm_mc_management_standards_content content
        ON  component.management_standards_component_id = content.management_standards_component_id
        AND component.history_management_id = content.history_management_id
    WHERE
        component.history_management_id = @HistoryManagementId -- 変更管理ID
    AND component.is_management_standard_conponent = 1 -- 機器別管理基準フラグ
),
new_data AS(
    SELECT
        trans.management_standards_content_id,
        coalesce(history.inspection_site_structure_id, trans.inspection_site_structure_id) AS inspection_site_structure_id,
        coalesce(history.inspection_content_structure_id, trans.inspection_content_structure_id) AS inspection_content_structure_id,
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
        history.management_standards_content_id,
        history.inspection_site_structure_id,
        history.inspection_content_structure_id,
        history.execution_division
    FROM history
)
SELECT
    COUNT(*)
FROM
    new_data
WHERE
    new_data.execution_division <> 6 -- 削除行以外
AND new_data.inspection_site_structure_id = @InspectionSiteStructureId -- 部位ID
AND new_data.inspection_content_structure_id = @InspectionContentStructureId -- 点検内容ID(保全項目)
AND new_data.management_standards_content_id <> @ManagementStandardsContentId -- 自分自身以外