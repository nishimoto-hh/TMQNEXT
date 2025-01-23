with structure_factory as ( 
    SELECT
        structure_id
        , location_structure_id AS factory_id 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id IN (1010) 
        AND language_id = @LanguageId
) 
select
    machine.machine_id                     -- 機番ID
    , equip.equipment_id                   -- 機器ID
    , machine.machine_name                 -- 機器名称
    , machine.machine_no                   -- 機器番号
    , machine.equipment_level_structure_id -- 機器レベルID
    , machine.conservation_structure_id    -- 保全方式ID
    , machine.importance_structure_id      -- 機器重要度ID
    ---------- ここから翻訳を取得 ----------
    , ( 
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
                    st_f.structure_id = machine.job_kind_structure_id 
                    AND st_f.factory_id IN (0, machine.location_factory_structure_id)
            ) 
            AND tra.structure_id = machine.job_kind_structure_id
    ) as job_name -- 職種
    , ( 
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
                    st_f.structure_id = machine.job_large_classfication_structure_id 
                    AND st_f.factory_id IN (0, machine.location_factory_structure_id)
            ) 
            AND tra.structure_id = machine.job_large_classfication_structure_id
    ) as large_classfication_name -- 機種大分類
    , ( 
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
                    st_f.structure_id = machine.job_middle_classfication_structure_id 
                    AND st_f.factory_id IN (0, machine.location_factory_structure_id)
            ) 
            AND tra.structure_id = machine.job_middle_classfication_structure_id
    ) as middle_classfication_name -- 機種中分類
    , ( 
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
                    st_f.structure_id = machine.job_small_classfication_structure_id 
                    AND st_f.factory_id IN (0, machine.location_factory_structure_id)
            ) 
            AND tra.structure_id = machine.job_small_classfication_structure_id
    ) as small_classfication_name  -- 機種小分類
from
    mc_machine machine 
    inner join mc_equipment equip 
        on machine.machine_id = equip.machine_id
where
    machine.machine_id = @MachineId
