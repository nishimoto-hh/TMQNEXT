WITH target AS (
SELECT
    re.machine_spec_relation_id,            -- 機種別仕様関連付ID
    re.spec_id,                             -- 仕様項目ID
    re.spec_id AS spec_id_before,           -- 仕様項目ID(変更前)
    re.job_structure_id,                    -- 職種階層ID
    re.location_structure_id AS factory_id, -- 工場ID
    re.display_order,                       -- 並び順
    job_ord.display_order AS job_display_order, -- 職種のアイテム並び順
    (
        SELECT
            ms.structure_layer_no
        FROM
            ms_structure ms
        WHERE
            ms.structure_id = re.job_structure_id
    ) AS layer_no,                          -- 職種の階層番号
    ---------------------------------- 以下は翻訳を取得 ----------------------------------
    (
        SELECT
            mt.translation_text
        FROM
            ms_translation mt
        WHERE
            mt.location_structure_id = re.location_structure_id
        AND mt.translation_id = spec.translation_id
        AND mt.language_id = @LanguageId
    ) spec_name,                            -- 仕様項目名
    (
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
                  #temp_structure_factory AS st_f
              WHERE
                  st_f.structure_id = re.location_structure_id
              AND st_f.factory_id IN(0, re.location_structure_id)
           )
      AND tra.structure_id = re.location_structure_id
    ) AS factory_name                       -- 工場名
FROM
    ms_machine_spec_relation re
    LEFT JOIN
        ms_spec spec
    ON  re.spec_id = spec.spec_id
    LEFT JOIN
        (
            SELECT
                ord.structure_id,
                ord.display_order
            FROM
                ms_structure_order ord
            WHERE
                ord.structure_group_id = 1010
        ) job_ord
    ON  re.job_structure_id = job_ord.structure_id

WHERE
    -- ExcelPortで仕様項目を新規追加した場合職種がNULLのデータが存在するため取得しないようにする
    re.job_structure_id IS NOT NULL 
AND EXISTS(SELECT * FROM #temp_structure_selected temp WHERE temp.structure_group_id = 1000 
     AND re.location_structure_id = temp.structure_id)
) 

SELECT *
FROM target
ORDER BY
    target.factory_id,
    COALESCE(target.display_order, 0),
    target.spec_id,
    target.layer_no,
    target.job_display_order
