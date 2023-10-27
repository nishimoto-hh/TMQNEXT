WITH max_loc AS(
    SELECT
        mt.translation_id,
        MAX(mt.location_structure_id) AS location_structure_id
    FROM
        ms_translation mt
    WHERE
        mt.translation_id IN(111310007,111010004, 111010005, 111050007, 111050008, 111060041, 111060042, 111060043, 111060079, 111060080, 111060081, 111060082, 111060083, 111060084, 111060085, 111100012, 111110001, 111270017, 111270024, 111310006, 111320001)
    AND mt.location_structure_id IN @FactoryIdList
    AND mt.language_id = @LanguageId
    GROUP BY
        mt.translation_id
)
SELECT
    mt.translation_id,
    mt.translation_text AS header_name
FROM
    ms_translation mt
    INNER JOIN
        max_loc
    ON  mt.translation_id = max_loc.translation_id
    AND mt.location_structure_id = max_loc.location_structure_id
WHERE
    mt.language_id = @LanguageId