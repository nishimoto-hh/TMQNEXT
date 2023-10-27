SELECT
    ROW_NUMBER() OVER(ORDER BY item.structure_id ASC) factory_number, -- 工場番号
    item.structure_id AS factory_id,                                  -- 工場ID(構成ID)
    item.parent_structure_id AS factory_parent_id,                    -- 工場の親構成ID
    item.structure_id AS location_structure_id,                       -- 絞り込みを行うために記載
    item.translation_text AS factory_name                             -- 工場名
FROM
    v_structure_item item
WHERE
    item.structure_group_id = 1000
AND item.structure_layer_no = 1
AND item.language_id = @LanguageId
AND item.structure_id IN
(
  SELECT DISTINCT dbo.get_target_layer_id(temp.structure_id, 1)
  FROM 
      #temp_location temp
)