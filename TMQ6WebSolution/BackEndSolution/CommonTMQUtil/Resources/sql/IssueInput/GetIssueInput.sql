--******************************************************************
--出庫情報取得
--******************************************************************
WITH unit AS ( 
    --翻訳を取得(単位)
    SELECT
        structure_id
        , translation_text 
    FROM
        v_structure_item_all si 
        INNER JOIN ms_item_extension ie 
            ON si.structure_item_id = ie.item_id 
            AND si.structure_group_id = 1730
    WHERE
        si.language_id = @LanguageId
) 
SELECT DISTINCT
    unt.translation_text AS unit_translation_text, --単位
    ppt.parts_id
FROM
    pt_parts ppt
    LEFT JOIN unit unt 
        ON ppt.unit_structure_id = unt.structure_id 
WHERE
    parts_id = @PartsId
