-- ‰æ–Ê’è‹`–|–ó
SELECT 
     tr.location_structure_id AS factoryId
    ,tr.translation_id AS messageId
    ,tr.translation_text AS value
    ,tr.language_id AS languageCd
FROM ms_translation tr
WHERE
    tr.delete_flg != 1
-- –|–óID‚ª1Žn‚Ü‚è‚Ü‚½‚Í9Žn‚Ü‚è
AND (tr.translation_id like '1%' OR tr.translation_id like '9%')
ORDER BY tr.translation_id, tr.location_structure_id desc
