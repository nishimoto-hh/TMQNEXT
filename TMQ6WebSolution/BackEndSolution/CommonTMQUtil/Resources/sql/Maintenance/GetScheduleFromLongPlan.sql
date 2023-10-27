SELECT
    llp.subject,                               -- Œ–¼
    llp.subject_note,                          -- Œ–¼ƒƒ‚
    llp.location_structure_id,                 -- êŠî•ñ
    llp.job_structure_id,                      -- Eíî•ñ
    llp.budget_personality_structure_id,       -- —\Z«Ši‹æ•ª
    llp.budget_management_structure_id,        -- —\ZŠÇ—‹æ•ª
    llp.maintenance_season_structure_id,       -- •Û‘SŠú
    llp.person_id AS construction_personnel_id -- {H’S“–Ò
FROM
    mc_maintainance_schedule_detail masd -- •Û‘SƒXƒPƒWƒ…[ƒ‹Ú×
    INNER JOIN
        mc_maintainance_schedule mas -- •Û‘SƒXƒPƒWƒ…[ƒ‹
    ON  masd.maintainance_schedule_id = mas.maintainance_schedule_id
    INNER JOIN
        mc_management_standards_content mmsc -- ‹@Ší•ÊŠÇ—Šî€“à—e
    ON  mas.management_standards_content_id = mmsc.management_standards_content_id
    INNER JOIN
        ln_long_plan llp -- ’·ŒvŒ–¼
    ON  mmsc.long_plan_id = llp.long_plan_id
WHERE
    masd.maintainance_schedule_detail_id = @MaintainanceScheduleDetailId