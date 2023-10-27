SELECT vi.structure_id,vi.translation_text,vi.location_structure_id
FROM (SELECT max(location_structure_id) location_structure_id,
             item_translation_id,
			 structure_id
	  FROM v_structure_item
	  WHERE structure_group_id = '1360'
	  AND language_id = @LanguageId
      AND structure_id = @SpecUnitId
	  AND location_structure_id in (0,5)
	  GROUP BY item_translation_id,structure_id) vim,
      v_structure_item_all vi
WHERE vim.structure_id = vi.structure_id
AND vim.location_structure_id = vi.location_structure_id
AND language_id = @LanguageId

