WITH CONDITION AS(
    -- 検索条件
    SELECT
        content.long_plan_id,
        detail.schedule_date
    FROM
        mc_maintainance_schedule_detail detail
        INNER JOIN
            mc_maintainance_schedule schedule
        ON  detail.maintainance_schedule_id = schedule.maintainance_schedule_id
        INNER JOIN
            mc_management_standards_content content
        ON  schedule.management_standards_content_id = content.management_standards_content_id
    WHERE
        detail.maintainance_schedule_detail_id = @MaintainanceScheduleDetailId
),
detail_id_list AS(
    -- 機器別管理基準内容IDに紐付く保全スケジュール詳細IDをカンマ区切りで取得
    SELECT
        content_a.management_standards_content_id,
        trim(
            ','
            FROM
                (
                    SELECT
                        cast(detail.maintainance_schedule_detail_id AS varchar) + ','
                    FROM
                        mc_management_standards_content content_b
                        INNER JOIN
                            mc_maintainance_schedule schedule
                        ON  content_b.management_standards_content_id = schedule.management_standards_content_id
                        INNER JOIN
                            mc_maintainance_schedule_detail detail
                        ON  schedule.maintainance_schedule_id = detail.maintainance_schedule_id
                        INNER JOIN
                            CONDITION
                        ON  content_b.long_plan_id = CONDITION.long_plan_id
                        AND FORMAT(detail.schedule_date, 'yyyyMM') = FORMAT(CONDITION.schedule_date, 'yyyyMM')
                    WHERE
                        content_b.management_standards_content_id = content_a.management_standards_content_id FOR XML PATH('')
                )
        ) AS maintainance_schedule_detail_id
    FROM
        mc_management_standards_content content_a
    GROUP BY
        content_a.management_standards_content_id
),
base AS(
    -- リンク遷移元のデータ
    SELECT
        machine.job_structure_id,
        machine.machine_no,
        machine.machine_name,
        machine.equipment_level_structure_id,
        machine.conservation_structure_id,
        machine.importance_structure_id,
        component.inspection_site_structure_id,
        content.inspection_content_structure_id,
        content.long_plan_id,
        detail.schedule_date,
        machine.machine_id,
        equipment.equipment_id,
        CASE
            WHEN ie.extension_data = '1'
            --機器使用区分が廃棄
            THEN 1
            ELSE 0
        END AS gray_out_flg,
        detail.summary_id,
        detail_id_list.maintainance_schedule_detail_id
    FROM
        mc_maintainance_schedule_detail detail
        INNER JOIN
            mc_maintainance_schedule schedule
        ON  detail.maintainance_schedule_id = schedule.maintainance_schedule_id
        INNER JOIN
            mc_management_standards_content content
        ON  schedule.management_standards_content_id = content.management_standards_content_id
        INNER JOIN
            mc_management_standards_component component
        ON  content.management_standards_component_id = component.management_standards_component_id
        INNER JOIN
            mc_machine machine
        ON  component.machine_id = machine.machine_id
        INNER JOIN
            mc_equipment equipment
        ON  machine.machine_id = equipment.machine_id
        LEFT JOIN
            ms_structure ms
        ON  equipment.use_segment_structure_id = ms.structure_id
        LEFT JOIN
            ms_item_extension ie
        ON  ms.structure_item_id = ie.item_id
        AND ie.sequence_no = 1
        LEFT JOIN
            detail_id_list
        ON  content.management_standards_content_id = detail_id_list.management_standards_content_id
    WHERE
        detail.maintainance_schedule_detail_id = @MaintainanceScheduleDetailId
)
-- リンク遷移元のデータ
SELECT
    base.job_structure_id,                -- 職種
    base.machine_no,                      -- 機器番号
    base.machine_name,                    -- 機器名称
    base.equipment_level_structure_id,    -- 機器レベル
    base.conservation_structure_id,       -- 保全方式
    base.importance_structure_id,         -- 機器重要度
    base.inspection_site_structure_id,    -- 保全部位
    base.inspection_content_structure_id, -- 保全内容
    base.machine_id,                      -- 機番ID
    base.equipment_id,                    -- 機器ID
    base.gray_out_flg,                    -- グレーアウトフラグ
    base.maintainance_schedule_detail_id  -- 保全スケジュール詳細ID
FROM
    base
WHERE
    base.summary_id IS NULL
-- リンク遷移元のデータと同一長期計画件名に含まれる同年月のデータ
UNION
SELECT
    machine.job_structure_id,                      -- 職種
    machine.machine_no,                            -- 機器番号
    machine.machine_name,                          -- 機器名称
    machine.equipment_level_structure_id,          -- 機器レベル
    machine.conservation_structure_id,             -- 保全方式
    machine.importance_structure_id,               -- 機器重要度
    component.inspection_site_structure_id,        -- 保全部位
    content.inspection_content_structure_id,       -- 保全内容
    machine.machine_id,                            -- 機番ID
    equipment.equipment_id,                        -- 機器ID
    CASE
        WHEN ie.extension_data = '1'
        --機器使用区分が廃棄
        THEN 1
        ELSE 0
    END AS gray_out_flg,                           -- グレーアウトフラグ
    detail_id_list.maintainance_schedule_detail_id -- 保全スケジュール詳細ID
FROM
    mc_management_standards_content content
    INNER JOIN
        mc_management_standards_component component
    ON  content.management_standards_component_id = component.management_standards_component_id
    INNER JOIN
        mc_machine machine
    ON  component.machine_id = machine.machine_id
    INNER JOIN
        mc_equipment equipment
    ON  machine.machine_id = equipment.machine_id
    LEFT JOIN
        ms_structure ms
    ON  equipment.use_segment_structure_id = ms.structure_id
    LEFT JOIN
        ms_item_extension ie
    ON  ms.structure_item_id = ie.item_id
    AND ie.sequence_no = 1
    INNER JOIN
        mc_maintainance_schedule schedule
    ON  content.management_standards_content_id = schedule.management_standards_content_id
    INNER JOIN
        mc_maintainance_schedule_detail detail
    ON  schedule.maintainance_schedule_id = detail.maintainance_schedule_id
    INNER JOIN
        base
    ON  content.long_plan_id = base.long_plan_id
    AND FORMAT(detail.schedule_date, 'yyyyMM') = FORMAT(base.schedule_date, 'yyyyMM')
    LEFT JOIN
        detail_id_list
    ON  content.management_standards_content_id = detail_id_list.management_standards_content_id
WHERE
    detail.summary_id IS NULL