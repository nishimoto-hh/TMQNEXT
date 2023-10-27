--location_structure_id(ツリーで選択されたアイテムを取得する為に記載しています)
--job_structure_id(ツリーで選択されたアイテムを取得する為に記載しています)
SELECT
    factory_job.factory_id,                                -- 工場
    factory_job.job_id AS parts_job_id,                    -- 職種
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
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = factory_job.factory_id
                    AND st_f.factory_id IN (0, factory_job.factory_id)
            ) 
            AND tra.structure_id = factory_job.factory_id
    ) as factory_name,
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
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = factory_job.job_id
                    AND st_f.factory_id IN (0, factory_job.factory_id)
            ) 
            AND tra.structure_id = factory_job.job_id
    ) as job_name
FROM
    factory_job
    LEFT JOIN
        confirm
    ON  factory_job.factory_id = confirm.factory_id
    AND factory_job.job_id = confirm.parts_job_id
    LEFT JOIN
        max_update_date_confirm max_confirm
    ON  factory_job.factory_id = max_confirm.factory_id
    AND factory_job.job_id = max_confirm.parts_job_id
    AND format(confirm.target_month, 'yyyy/MM') = format(max_confirm.target_month, 'yyyy/MM')
    LEFT JOIN
        max_update_date_fixed max_fixed
    ON  factory_job.factory_id = max_fixed.factory_id
    AND factory_job.job_id = max_fixed.job_structure_id
    AND format(confirm.target_month, 'yyyy/MM') = format(max_fixed.target_month, 'yyyy/MM')