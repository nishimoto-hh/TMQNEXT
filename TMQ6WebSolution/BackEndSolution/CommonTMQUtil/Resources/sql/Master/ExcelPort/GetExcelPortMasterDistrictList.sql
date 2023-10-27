SELECT
    item.structure_group_id,                                           -- 構成グループID
    ROW_NUMBER() OVER(ORDER BY item.structure_id ASC) district_number, -- 地区番号
    item.structure_id AS district_id,                                  -- 地区ID(構成ID)
    item.translation_text AS district_name                             -- 地区名
FROM
    v_structure_item item
WHERE
    item.structure_group_id = 1000
AND item.structure_layer_no = 0
AND item.language_id = @LanguageId
AND item.structure_id IN
(
  SELECT DISTINCT dbo.get_target_layer_id(temp.structure_id, 0)
  FROM 
      #temp_location temp
)