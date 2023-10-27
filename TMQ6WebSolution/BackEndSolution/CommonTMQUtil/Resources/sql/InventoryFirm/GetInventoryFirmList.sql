SELECT
    factory_job_main.factory_id,                           -- 工場
    factory_job_main.job_id AS parts_job_id,               -- 職種
    confirm.target_month AS last_confirmed_date,           -- 最終確定年月
    confirm.display_name AS last_confirmed_person,         -- 最終確定者
    confirm.execution_datetime AS last_confirmed_datetime, -- 最終確定日時
    max_confirm.max_update_datetime_confirm,               -- 最大更新日時(在庫確定管理データ)
    max_fixed.max_update_datetime_fixed,                   -- 最大更新日時(確定在庫データ)
    ---------------------------------- 以下は翻訳を取得 ----------------------------------
    (
        SELECT
            tra.translation_text 
        FROM
            v_structure_item AS tra 
        WHERE
            tra.language_id = @LanguageId
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = factory_job_main.factory_id
                    AND st_f.factory_id IN (0, factory_job_main.factory_id)
            ) 
            AND tra.structure_id = factory_job_main.factory_id
    ) as factory_name,
    CASE
       WHEN factory_job_main.job_id = 0
            THEN ( 
            SELECT
                tr.translation_text 
            FROM
                ms_translation tr 
            WHERE
                tr.location_structure_id = 0 
                and tr.translation_id = 111120264 
                and tr.language_id = @LanguageId
        )
        else (
              SELECT
                   tra.translation_text 
              FROM
                   v_structure_item AS tra 
              WHERE
                   tra.language_id = @LanguageId
                   AND tra.location_structure_id = ( 
                       SELECT
                            MAX(st_f.factory_id) 
                       FROM
                            #temp_structure_factory AS st_f 
                       WHERE
                           st_f.structure_id = factory_job_main.job_id
                           AND st_f.factory_id IN (0, factory_job_main.factory_id)
                    ) 
              AND tra.structure_id = factory_job_main.job_id
             )
    END as job_name -- 職種IDが「0」でない場合は翻訳を取得し、「0」の場合は「職種なし」を表示
FROM
    factory_job_main
    LEFT JOIN
        confirm
    ON  factory_job_main.factory_id = confirm.factory_id
    AND COALESCE(factory_job_main.job_id, 0) = COALESCE(confirm.parts_job_id, 0)
    LEFT JOIN
        max_update_date_confirm max_confirm
    ON  factory_job_main.factory_id = max_confirm.factory_id
    AND COALESCE(factory_job_main.job_id, 0) = COALESCE(max_confirm.parts_job_id, 0)
    AND FORMAT(confirm.target_month, 'yyyy/MM') = FORMAT(max_confirm.target_month, 'yyyy/MM')
    LEFT JOIN
        max_update_date_fixed max_fixed
    ON  factory_job_main.factory_id = max_fixed.factory_id
    AND COALESCE(factory_job_main.job_id, 0) = COALESCE(max_fixed.job_structure_id, 0)
    AND FORMAT(confirm.target_month, 'yyyy/MM') = FORMAT(max_fixed.target_month, 'yyyy/MM')

-- ツリーで職種が選択されている場合、選択された職種を対象とするため「職種なし」のデータは表示しない
/*@isJobSelected
WHERE
    factory_job_main.job_id != 0
@isJobSelected*/


-- ツリーで職種が選択されていない場合、「職種なし」のデータか、対象年月で確定されているデータのみ表示する
/*@isJobNotSelected
WHERE
    factory_job_main.job_id = 0
    OR FORMAT(confirm.execution_datetime, 'yyyy/MM') = @TargetMonth
--@isJobNotSelected*/

