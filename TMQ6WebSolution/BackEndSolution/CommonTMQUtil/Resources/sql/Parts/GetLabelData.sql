WITH parts_id_list AS( -- 予備品ID
    SELECT
        VALUE AS parts_id
    FROM
        STRING_SPLIT(@PartsIdList, ',')
),
subject_code_list AS( -- 勘定科目コード
    SELECT
        VALUE AS subject_code
    FROM
        STRING_SPLIT(@SubjectCodeList, ',')
),
department_code_list AS( -- 部門コード
    SELECT
        VALUE AS department_code
    FROM
        STRING_SPLIT(@DepartmentCodeList, ',')
),
location AS( -- 構成IDの階層番号を取得
SELECT
    ms.structure_id,
    ms.structure_layer_no
FROM
    ms_structure ms
WHERE
    ms.structure_group_id = 1040
)
,structure_factory as( SELECT
        structure_id
        , location_structure_id AS factory_id 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id IN (1000, 1040, 1150, 1720, 1730,1740, 1760, 1770) 
        AND language_id = @LanguageId
)

SELECT
    parts.parts_no,                                                                                          -- 予備品No.
    parts.parts_name,                                                                                        -- 予備品名
    parts.model_type,                                                                                        -- 型式
    parts.standard_size,                                                                                     -- 規格・寸法
    label.department_code,                                                                                   -- 部門コード
    label.subject_code,                                                                                      -- 勘定科目コード
    location.structure_layer_no,                                                                             -- 標準棚番の階層番号
    parts.factory_id AS parts_factory_id,                                                                    -- 管理工場ID
    parts.parts_location_detail_no,                                                                          -- 棚枝番
    COALESCE(parts.lead_time, 0) AS lead_time,                                                               -- 発注点
    COALESCE(parts.order_quantity, 0) AS order_quantity,                                                     -- 発注量
    'YN'+RIGHT('     ' + CONVERT(NVARCHAR, parts.parts_no), 5) 
    + ' ' + label.department_code + ' ' + label.subject_code AS qrc,                                         -- QRコード(parts_no標記変更 by AEC)	
            ---------------------------------- 以下は翻訳を取得 ----------------------------------
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
                  structure_factory AS st_f
              WHERE
                  st_f.structure_id = parts.manufacturer_structure_id
              AND st_f.factory_id IN(0, parts.factory_id)
           )
      AND tra.structure_id = parts.manufacturer_structure_id
    ) AS maker,                                                                                             -- メーカー
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
                  structure_factory AS st_f
              WHERE
                  st_f.structure_id = parts.parts_location_id
              AND st_f.factory_id IN(0, parts.factory_id)
           )
      AND tra.structure_id = parts.parts_location_id
    ) AS shed_name                                                                                          -- 標準棚番
FROM
    (
        SELECT
            parts_id_list.parts_id,
            subject_code_list.subject_code,
            department_code_list.department_code
        FROM
            parts_id_list,
            subject_code_list,
            department_code_list
    ) label
    LEFT JOIN
        pt_parts parts
    ON  label.parts_id = parts.parts_id
    LEFT JOIN
        location
    ON  parts.parts_location_id = location.structure_id
ORDER BY
    parts.parts_no,
    label.department_code,
    label.subject_code
