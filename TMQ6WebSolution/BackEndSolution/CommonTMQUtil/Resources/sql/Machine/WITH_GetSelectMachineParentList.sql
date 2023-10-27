WITH item AS ( 
    --循環対象の拡張データを取得
    SELECT
        structure_id
        , ie.extension_data 
    FROM
        ms_structure si 
        INNER JOIN ms_item_extension ie 
            ON si.structure_item_id = ie.item_id 
            AND si.structure_group_id = 1920 
            AND ie.sequence_no = 1
),
structure_factory AS (
    SELECT
        structure_id
        , location_structure_id AS factory_id 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id IN (1170,1200,1030,1150,1210) 
        AND language_id = @LanguageId
) 
