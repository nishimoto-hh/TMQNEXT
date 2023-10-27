-- 翻訳マスタから保全項目、保全部位を抽出
WITH item AS(
    SELECT
         structure_id
        ,location_structure_id AS factory_id
        ,translation_text
    FROM
        v_structure_item
    WHERE
        structure_group_id IN(1180, 1220)
    AND language_id = @LanguageId
)

SELECT 
     mc.machine_no
    ,mc.machine_name
    ,mc.location_structure_id
    ,mc.job_structure_id
    ,st_loc.factory_id
    ,com.inspection_site_structure_id
    ,item_com.translation_text AS inspection_site_name
    ,con.management_standards_content_id
    ,con.inspection_content_structure_id
    ,item_con.translation_text AS inspection_content_name
    ,lp.long_plan_id
    ,lp.subject
    ,lp.subject_note
    ,lp.person_id
    ,lp.person_name

-- 機番情報
FROM 
    mc_machine mc

-- 場所階層
INNER JOIN ms_structure st_loc
ON  st_loc.structure_id = mc.location_structure_id 
AND st_loc.delete_flg != 1

-- 機器別管理基準部位
LEFT JOIN mc_management_standards_component com
ON mc.machine_id = com.machine_id

-- 機器別管理基準内容
LEFT JOIN mc_management_standards_content con
ON com.management_standards_component_id = con.management_standards_component_id

-- 長期計画件名
LEFT JOIN ln_long_plan lp
ON con.long_plan_id = lp.long_plan_id

-- 保全部位翻訳
LEFT JOIN item item_com
ON com.inspection_site_structure_id = item_com.structure_id

-- 保全項目翻訳
LEFT JOIN item item_con
ON con.inspection_content_structure_id = item_con.structure_id

WHERE
    EXISTS(
        SELECT * FROM #temp_structure_selected temp 
        WHERE mc.location_structure_id = temp.structure_id)

AND
    EXISTS(
        SELECT * FROM #temp_structure_selected temp 
        WHERE mc.job_structure_id = temp.structure_id)

AND 
    com.is_management_standard_conponent = 1    -- 機器別管理基準フラグ
   
AND 
    item_com.factory_id = (
        SELECT MAX(factory_id) FROM item 
        WHERE 
            structure_id = com.inspection_site_structure_id
        AND
            factory_id IN (0, st_loc.factory_id))
        
AND 
    item_con.factory_id = (
        SELECT MAX(factory_id) FROM item 
        WHERE 
            structure_id = con.inspection_content_structure_id
        AND
            factory_id IN (0, st_loc.factory_id))

ORDER BY 
     mc.machine_no
    ,com.inspection_site_structure_id
    ,con.inspection_content_structure_id
