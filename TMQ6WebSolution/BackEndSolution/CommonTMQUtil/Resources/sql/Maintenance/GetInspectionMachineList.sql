WITH structure_factory AS(
    SELECT
        structure_id,
        location_structure_id AS factory_id
    FROM
        v_structure_item_all
    WHERE
        structure_group_id IN(1010, 1180, 1220)
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
                    st_f.structure_id = his.inspection_site_structure_id
                    AND st_f.factory_id IN (0, mc.location_factory_structure_id)
            )
            AND tra.structure_id = his.inspection_site_structure_id
      ) AS inspection_site_name
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
                    st_f.structure_id = hic.inspection_content_structure_id
                    AND st_f.factory_id IN (0, mc.location_factory_structure_id)
            )
            AND tra.structure_id = hic.inspection_content_structure_id
      ) AS inspection_content_name
    , mc.machine_no                             --機器番号
    , mc.machine_name                           --機器名称
    , mc.equipment_level_structure_id           --機器レベル
    , mc.conservation_structure_id              --保全方式
    , mc.importance_structure_id                --機器重要度
    , his.inspection_site_structure_id          --保全部位
    , hic.inspection_content_structure_id       --保全内容
    , hic.follow_flg                            --フォロー有無
    , hic.follow_plan_date                      --フォロー予定年月
    , hic.follow_content                        --フォロー内容
    , hic.follow_completion_date                --フォロー完了日
    , hm.used_days_machine                      --機器使用期間
    , CASE 
        WHEN ie.extension_data = '1'            --機器使用区分が廃棄
            THEN 1 
        ELSE 0 
        END AS gray_out_flg                     --グレーアウトフラグ
    , hm.history_machine_id                     --保全履歴機器ID
    , hm.update_serialid as update_serialid_hm  --更新シリアルID(保全履歴機器)
    , his.history_inspection_site_id            --保全履歴機器部位ID
    , his.update_serialid as update_serialid_his --更新シリアルID(保全履歴機器部位)
    , hic.history_inspection_content_id         --保全履歴点検内容ID
    , hic.update_serialid as update_serialid_hic --更新シリアルID(保全履歴点検内容)
    , mc.machine_id                             --機番ID
    , eq.equipment_id                           --機器ID
    , mc.location_factory_structure_id AS factory_id -- 工場ID
    , hic.work_record                                -- 作業記録
FROM
    ma_history hi 
    INNER JOIN ma_history_machine hm 
        ON hi.history_id = hm.history_id 
    INNER JOIN ma_history_inspection_site his 
        ON hm.history_machine_id = his.history_machine_id 
    INNER JOIN ma_history_inspection_content hic 
        ON his.history_inspection_site_id = hic.history_inspection_site_id 
    INNER JOIN mc_machine mc 
        ON hm.machine_id = mc.machine_id 
    INNER JOIN mc_equipment eq 
        ON hm.equipment_id = eq.equipment_id 
    LEFT JOIN ms_structure ms 
        ON eq.use_segment_structure_id = ms.structure_id 
    LEFT JOIN ms_item_extension ie 
        ON ms.structure_item_id = ie.item_id 
        AND ie.sequence_no = 1 
WHERE
    hi.summary_id = @SummaryId 
ORDER BY
    mc.job_structure_id ASC
    , mc.machine_no ASC
    , his.inspection_site_structure_id ASC
    , hic.inspection_content_structure_id ASC
