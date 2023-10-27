select distinct
    parts_id,              -- 予備品ID
    parts_no,              -- 予備品番号
    parts_name,            -- 予備品名称
    standard_size,         -- 規格・寸法
	manufacturer_structure_id, -- メーカID
    --maker.translation_text as maker -- メーカー
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
					structure_factory AS st_f 
				WHERE
					st_f.structure_id = pt.manufacturer_structure_id
					AND st_f.factory_id IN (0, pt.factory_id)
			) 
			AND tra.structure_id = pt.manufacturer_structure_id
		) AS maker -- メーカー
from
    pt_parts pt
    left join
        v_structure_item maker
    on  pt.manufacturer_structure_id = maker.structure_id
    and maker.structure_group_id = 1150
    and maker.language_id = @LanguageId
WHERE   (pt.factory_id = (SELECT v.factory_id 
						FROM mc_machine m,
							ms_structure v
						WHERE m.location_structure_id = v.structure_id
						AND   REPLACE(STR(machine_id),' ','') = @MachineId) -- 対象工場
        or 
		pt.factory_id  = (SELECT structure_id -- 対象工場と同じ地区に属する共通工場
						FROM ms_structure vi,
						ms_item_extension ie
						WHERE vi.structure_item_id = ie.item_id
						AND vi.structure_group_id = 1000
						AND vi.parent_structure_id = (SELECT parent_structure_id 
													  FROM ms_structure 
													  WHERE structure_id =( 
																		  SELECT v.factory_id 
																		  FROM mc_machine m,
																			   ms_structure v
																		  WHERE m.location_structure_id = v.structure_id
																		  AND   REPLACE(STR(machine_id),' ','') = @MachineId)) -- 対象工場と同じ地区に属する共通工場
						AND vi.structure_layer_no = 1 
						AND ie.sequence_no = 3 
						AND ie.extension_data = '1'
						)
		)
AND     (pt.job_structure_id = dbo.get_top_layer_id((SELECT job_structure_id
						FROM mc_machine m
						WHERE REPLACE(STR(machine_id),' ','') = @MachineId)) -- 対象工場
		)		
/*@PartsName
    -- 担当者名
AND parts_name LIKE '%'+ @PartsName +'%'
@PartsName*/

--order by parts_name,standard_size,maker

