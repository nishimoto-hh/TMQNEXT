DROP TABLE IF EXISTS #temp_item_ex; 

CREATE TABLE #temp_item_ex(structure_id INT, extension_data nvarchar(400)); 

DROP TABLE IF EXISTS #content_id_list; 

CREATE TABLE #content_id_list( 
    maintainance_schedule_detail_id bigint
    , management_standards_content_id bigint
); 

DROP TABLE IF EXISTS #detail_id_list; 

CREATE TABLE #detail_id_list( 
    management_standards_content_id bigint
    , maintainance_schedule_detail_id nvarchar(MAX)
); 

--使用区分
INSERT 
INTO #temp_item_ex
SELECT
    ms.structure_id
    , mie.extension_data 
FROM
    ms_structure ms 
    LEFT JOIN ms_item_extension mie 
        ON ms.structure_item_id = mie.item_id 
WHERE
    ms.structure_group_id = 1210 
    AND mie.sequence_no = 1; 

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
)

-- 機器別管理基準内容IDに紐付く保全スケジュール詳細IDを取得
INSERT 
INTO #content_id_list
SELECT
    detail.maintainance_schedule_detail_id
    , content_b.management_standards_content_id 
FROM
    mc_management_standards_content content_b 
    INNER JOIN mc_maintainance_schedule schedule 
        ON content_b.management_standards_content_id = schedule.management_standards_content_id 
    INNER JOIN mc_maintainance_schedule_detail detail 
        ON schedule.maintainance_schedule_id = detail.maintainance_schedule_id 
    INNER JOIN CONDITION 
        ON content_b.long_plan_id = CONDITION.long_plan_id 
        AND FORMAT(detail.schedule_date, 'yyyyMM') = FORMAT(CONDITION.schedule_date, 'yyyyMM'); 

-- 機器別管理基準内容IDに紐付く保全スケジュール詳細IDをカンマ区切りで取得
INSERT 
INTO #detail_id_list 
SELECT
    content_a.management_standards_content_id
    , trim( 
        ',' 
        FROM
            ( 
                SELECT
                    CAST( 
                        content_id_list.maintainance_schedule_detail_id AS VARCHAR
                    ) + ',' 
                FROM
                    #content_id_list content_id_list 
                WHERE
                    content_id_list.management_standards_content_id = content_a.management_standards_content_id FOR XML 
                    PATH ('')
            )
    ) AS maintainance_schedule_detail_id 
FROM
    mc_management_standards_content content_a 
    INNER JOIN #content_id_list content_id_list 
        ON content_a.management_standards_content_id = content_id_list.management_standards_content_id 
GROUP BY
    content_a.management_standards_content_id; 

-- リンク遷移元のデータと同一長期計画件名に含まれる同年月のデータ
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
        #detail_id_list detail_id_list
    ON  content.management_standards_content_id = detail_id_list.management_standards_content_id 
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
        #temp_item_ex ie
    ON  ie.structure_id = equipment.use_segment_structure_id
    INNER JOIN
        #content_id_list content_id_list
    ON  content.management_standards_content_id = content_id_list.management_standards_content_id
    INNER JOIN
        mc_maintainance_schedule_detail detail
    ON  detail.maintainance_schedule_detail_id = content_id_list.maintainance_schedule_detail_id
WHERE
    detail.summary_id IS NULL