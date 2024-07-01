WITH structure_factory AS (
    SELECT
        structure_id
        , location_structure_id AS factory_id 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id IN (1150) 
        AND language_id = @LanguageId
) 

SELECT
    mc.parts_id,                    -- 予備品ID
    pt.parts_name,
    standard_size AS dimensions,    -- 規格・寸法
    -- maker.translation_text AS maker,-- メーカー
	-- dbo.get_translation_text(pt.manufacturer_structure_id,@FactoryId,1150,@LanguageId) AS maker,    
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
					AND st_f.factory_id IN (0, (SELECT location_factory_structure_id from mc_machine where machine_id = @MachineId))
			) 
			AND tra.structure_id = pt.manufacturer_structure_id
		) AS maker, -- 
    use_quantity,                   -- 使用個数
    (SELECT SUM(stock_quantity) 
	 FROM pt_location_stock 
	 WHERE parts_id = mc.parts_id) AS stock,    -- 品目在庫数
    machine_use_parts_id,           -- 機番使用部品情報ID
    mc.update_serialid,             -- 更新シリアルID
    @MachineId AS machine_id,       -- 機番ID
    model_type                      -- 予備品型式
FROM
    mc_machine_use_parts mc
	JOIN
        pt_parts pt
    ON  mc.parts_id = pt.parts_id
    --LEFT JOIN
    --    v_structure_item maker
    --ON  pt.manufacturer_structure_id = maker.structure_id
    --AND maker.structure_group_id = 1150
    --AND maker.language_id = @LanguageId
WHERE  mc.machine_id = @MachineId
ORDER BY
    machine_use_parts_id
