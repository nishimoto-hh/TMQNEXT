SELECT 
     item.structure_id as [values]
    ,item.translation_text AS [labels]

FROM 
    v_structure_item_all AS item

-- 拡張項目1
INNER JOIN ms_item_extension AS item_ex1 
ON item.structure_item_id=item_ex1.item_id 
AND item_ex1.sequence_no=1

-- 拡張項目2
INNER JOIN ms_item_extension AS item_ex2
ON item.structure_item_id=item_ex2.item_id 
AND item_ex2.sequence_no=2

WHERE
    item.structure_group_id = 2110
AND item.language_id = @LanguageId
AND item_ex1.extension_data = @ConductId
AND item_ex2.extension_data = @SheetNo
