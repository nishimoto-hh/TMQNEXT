WITH structure_factory AS(
    SELECT
        structure_id,
        location_structure_id AS factory_id
    FROM
        v_structure_item_all
    WHERE
        structure_group_id IN(1010)
    AND language_id = @LanguageId
)
SELECT DISTINCT
     mc.job_structure_id                         --職種～機種小分類
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
                    st_f.structure_id = mc.job_kind_structure_id
                    AND st_f.factory_id IN (0, mc.location_factory_structure_id)
            )
            AND tra.structure_id = mc.job_kind_structure_id
      ) AS job_name
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
                    st_f.structure_id = mc.job_large_classfication_structure_id
                    AND st_f.factory_id IN (0, mc.location_factory_structure_id)
            )
            AND tra.structure_id = mc.job_large_classfication_structure_id
      ) AS large_classfication_name
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
                    st_f.structure_id = mc.job_middle_classfication_structure_id
                    AND st_f.factory_id IN (0, mc.location_factory_structure_id)
            )
            AND tra.structure_id = mc.job_middle_classfication_structure_id
      ) AS middle_classfication_name
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
                    st_f.structure_id = mc.job_small_classfication_structure_id
                    AND st_f.factory_id IN (0, mc.location_factory_structure_id)
            )
            AND tra.structure_id = mc.job_small_classfication_structure_id
      ) AS small_classfication_name
    , mc.machine_no                             --機器番号
    , mc.machine_name                           --機器名称
    , mc.equipment_level_structure_id           --機器レベル
    , mc.conservation_structure_id              --保全方式
    , mc.importance_structure_id                --機器重要度
    , hf.used_days_machine                      --機器使用期間
    , CASE 
        WHEN ie.extension_data = '1'            --機器使用区分が廃棄
            THEN 1 
        ELSE 0 
        END AS gray_out_flg                     --グレーアウトフラグ
    , mc.machine_id                             --機番ID
    , eq.equipment_id                           --機器ID
    , hf.work_record                            --作業記録
FROM
    ma_history hi 
    INNER JOIN ma_history_failure hf 
        ON hi.history_id = hf.history_id 
    INNER JOIN mc_machine mc 
        ON hf.machine_id = mc.machine_id 
    INNER JOIN mc_equipment eq 
        ON hf.equipment_id = eq.equipment_id 
    LEFT JOIN ms_structure ms 
        ON eq.use_segment_structure_id = ms.structure_id 
    LEFT JOIN ms_item_extension ie 
        ON ms.structure_item_id = ie.item_id 
        AND ie.sequence_no = 1 
WHERE
    hi.summary_id = @SummaryId
