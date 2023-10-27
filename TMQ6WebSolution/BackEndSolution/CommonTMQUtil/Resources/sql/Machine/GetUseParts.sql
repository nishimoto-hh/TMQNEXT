SELECT
    mc.parts_id,                    -- 予備品ID
    standard_size AS dimensions,    -- 規格・寸法
    -- maker.translation_text AS maker,-- メーカー
	dbo.get_translation_text(pt.manufacturer_structure_id,@FactoryId,1150,@LanguageId) AS maker,
    use_quantity,                   -- 使用個数
    (SELECT SUM(stock_quantity) 
	 FROM pt_location_stock 
	 WHERE parts_id = mc.parts_id) AS stock,    -- 品目在庫数
    machine_use_parts_id,           -- 機番使用部品情報ID
    mc.update_serialid,             -- 更新シリアルID
    @MachineId AS machine_id        -- 機番ID
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
