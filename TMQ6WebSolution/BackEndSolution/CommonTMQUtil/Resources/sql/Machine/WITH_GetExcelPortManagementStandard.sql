WITH structure_factory AS (
    SELECT
        structure_id
        , location_structure_id AS factory_id 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id IN (1170,1180,1200,1220,1030,1230,1240,1890) 
        AND language_id = @LanguageId
) 
