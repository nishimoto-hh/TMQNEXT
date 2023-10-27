SELECT
    org.translation_id AS messageId
    , org.language_id AS languageCd
    , org.translation_text AS value 
FROM
    ms_translation org JOIN ( 
        SELECT
            MAX(location_structure_id) AS location_structure_id
            , translation_id
            , language_id 
        FROM
            ms_translation 
        /* BEGIN */
        WHERE
            delete_flg != 1 
            /*IF LanguageId != null && LanguageId != ''*/
            AND language_id = /*LanguageId*/'ja'
            /*END*/
            /*IF MessageIdList.Count!=0 */
            AND CAST(translation_id AS varchar(100)) IN /*MessageIdList*/(0)
            /*END*/
            /*IF FactoryIdList.Count!=0 */
            AND location_structure_id IN /*FactoryIdList*/(0)
            /*END*/
        /* END */
        GROUP BY
            translation_id
            , language_id
    ) use_trans 
        ON org.location_structure_id = use_trans.location_structure_id 
        AND org.translation_id = use_trans.translation_id 
        AND org.language_id = use_trans.language_id 
WHERE
     org.delete_flg = 0
ORDER BY
    org.translation_id
