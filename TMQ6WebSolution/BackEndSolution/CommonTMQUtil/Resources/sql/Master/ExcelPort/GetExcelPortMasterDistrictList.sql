SELECT
    ms.structure_group_id,                                           -- 構成グループID
    ROW_NUMBER() OVER(ORDER BY ms.structure_id ASC) district_number, -- 地区番号
    ms.structure_id AS district_id,                                  -- 地区ID(構成ID)
        (
            SELECT
                tra.translation_text
            FROM
                v_structure_item_all AS tra
            WHERE
                tra.language_id = @LanguageId
            AND tra.location_structure_id = (
                    SELECT
                        MAX(st_f.factory_id)
                    FROM
                        #temp_structure_factory AS st_f
                    WHERE
                        st_f.structure_id = ms.structure_id
                    AND st_f.factory_id IN(0, ms.factory_id)
                )
            AND tra.structure_id = ms.structure_id
        ) AS district_name                                             -- 地区名
FROM
    ms_structure ms
WHERE
    ms.structure_group_id = 1000
AND ms.structure_layer_no = 0
AND ms.delete_flg = 0
AND ms.structure_id IN
(
  SELECT DISTINCT dbo.get_target_layer_id(temp.structure_id, 0)
  FROM 
      #temp_location temp
)

ORDER BY district_number