--******************************************************************
--構成IDより拡張項目を取得する
--******************************************************************
    SELECT
        ie.extension_data AS language_id
    FROM
        v_structure_item_all si 
        INNER JOIN ms_item_extension ie 
            ON si.structure_item_id = ie.item_id 
            AND si.structure_group_id = 9020 
    WHERE
        si.language_id = @LanguageId
    AND si.structure_id = @LanguageStructureId