--******************************************************************
--権限レベルより拡張データを取得する
--******************************************************************
SELECT
    mie.extension_data 
FROM
    ms_item_extension mie 
    LEFT JOIN v_structure_item vsi 
        ON mie.item_id = vsi.structure_item_id
        AND language_id = @LanguageId
WHERE vsi.structure_id = @AuthorityLevelId