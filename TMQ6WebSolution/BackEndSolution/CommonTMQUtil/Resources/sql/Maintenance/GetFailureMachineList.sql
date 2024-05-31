SELECT DISTINCT
    mc.job_structure_id                         --職種～機種小分類
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
