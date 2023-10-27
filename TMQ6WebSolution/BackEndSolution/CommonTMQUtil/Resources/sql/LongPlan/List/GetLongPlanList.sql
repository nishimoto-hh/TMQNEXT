SELECT
     target.long_plan_id
    ,target.subject
    ,target.subject_note
    ,target.location_structure_id
    ,target.job_structure_id
    ,target.maintenance_season_structure_id
    ,target.person_id
    ,target.person_name
    ,target.work_item_structure_id
    ,target.budget_management_structure_id
    ,target.budget_personality_structure_id
    ,target.file_link_equip
    ,target.file_link_subject
    ,target.purpose_structure_id
    ,target.work_class_structure_id
    ,target.treatment_structure_id
    ,target.facility_structure_id
    ,target.update_serialid
    ,target.long_plan_id_dt
    ,target.mc_man_st_con_update_datetime
    ,target.sche_detail_update_datetime
    ,target.attachment_update_datetime
    ,target.key_id
    -- 翻訳
    -- 保全時期
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.maintenance_season_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.maintenance_season_structure_id
    ) AS maintenance_season_name
    -- 作業項目
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.work_item_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.work_item_structure_id
    ) AS work_item_name
    -- 予算管理区分
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.budget_management_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.budget_management_structure_id
    ) AS budget_management_name
    -- 予算性格区分
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.budget_personality_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.budget_personality_structure_id
    ) AS budget_personality_name
    -- 目的区分
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.purpose_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.purpose_structure_id
    ) AS purpose_name
    -- 作業区分
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.work_class_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.work_class_structure_id
    ) AS work_class_name
    -- 処置区分
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.treatment_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.treatment_structure_id
    ) AS treatment_name
    -- 設備区分
    ,(
         SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                 SELECT
                    MAX(st_f.factory_id)
                FROM
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = target.facility_structure_id
                AND st_f.factory_id IN(0, target.factory_id)
            )
        AND tra.structure_id = target.facility_structure_id
    ) AS facility_structure_name
FROM
    target