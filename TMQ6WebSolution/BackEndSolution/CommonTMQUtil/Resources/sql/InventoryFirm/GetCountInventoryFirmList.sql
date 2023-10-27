WITH factory_job AS( -- 工場IDと職種IDの組み合わせ
SELECT DISTINCT
    factory.factory_id,
    factory.job_id
FROM
    (
        SELECT
            factory.factory_id,
            job.job_id
        FROM
            (
                SELECT
                    ms.structure_id AS factory_id
                FROM
                    ms_structure ms
                WHERE
                    ms.structure_group_id = 1000
                AND ms.structure_layer_no = 1
                AND EXISTS(
                 SELECT * 
                 FROM #temp_location temp
                 WHERE ms.structure_id = temp.structure_id
                )
            ) factory
            INNER JOIN
                (
                    SELECT
                        ms.factory_id,
                        ms.structure_id AS job_id
                    FROM
                        ms_structure ms
                    WHERE
                        ms.structure_group_id = 1010
                    AND ms.structure_layer_no = 0
                    AND EXISTS(
                     SELECT * 
                     FROM #temp_job temp
                     WHERE ms.structure_id = temp.structure_id
                    )
                ) job
            ON  factory.factory_id = job.factory_id
        UNION
        SELECT
            factory.factory_id,
            job.job_id
        FROM
            (
                SELECT
                    ms.structure_id AS factory_id
                FROM
                    ms_structure ms
                WHERE
                    ms.structure_group_id = 1000
                AND ms.structure_layer_no = 1
                AND EXISTS(
                 SELECT * 
                 FROM #temp_location temp
                 WHERE ms.structure_id = temp.structure_id
                )
            ) factory,
            (
                SELECT
                    ms.structure_id AS job_id
                FROM
                    ms_structure ms
                WHERE
                    ms.structure_group_id = 1010
                AND ms.structure_layer_no = 0
                AND ms.factory_id = 0
                AND EXISTS(
                     SELECT * 
                     FROM #temp_job temp
                     WHERE ms.structure_id = temp.structure_id
                    )
            ) job
    ) factory
),
factory_job_main AS( -- 工場IDと職種IDの組み合わせに職種がNULLのレコードを追加したもの
    SELECT
        * 
    FROM
        factory_job 
    UNION ALL 
    SELECT DISTINCT
        factory_id
        , 0 
    FROM
        factory_job
),
confirm AS( -- 在庫確定管理データ
    SELECT
        confirm_max.factory_id,    -- 工場
        confirm_max.parts_job_id,  -- 職種
        confirm_max.target_month,  -- 最終確定年月
        user_name.display_name,    -- 最終確定者
        confirm.execution_datetime -- 最終確定日時
    FROM
        (
            --工場・職種ごとの最終確定年月を取得
            SELECT
                confirm.factory_id,
                confirm.parts_job_id,
                MAX(confirm.target_month) AS target_month
            FROM
                pt_stock_confirm confirm
            WHERE
                format(confirm.target_month, 'yyyy/MM') <= @TargetMonth
            AND confirm.delete_flg = 0
            GROUP BY
                confirm.factory_id,
                confirm.parts_job_id
        ) AS confirm_max
        LEFT JOIN
            pt_stock_confirm confirm
        ON  confirm_max.factory_id = confirm.factory_id
        AND COALESCE(confirm_max.parts_job_id, 0) = COALESCE(confirm.parts_job_id, 0)
        AND confirm_max.target_month = confirm.target_month
        AND confirm.delete_flg = 0
        LEFT JOIN
            ms_user user_name
        ON  confirm.execution_user_id = user_name.user_id
        AND user_name.delete_flg = 0
)
SELECT
    COUNT(*)
FROM
    factory_job_main
    LEFT JOIN
        confirm
    ON  factory_job_main.factory_id = confirm.factory_id
    AND COALESCE(factory_job_main.job_id, 0) = COALESCE(confirm.parts_job_id, 0)

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


