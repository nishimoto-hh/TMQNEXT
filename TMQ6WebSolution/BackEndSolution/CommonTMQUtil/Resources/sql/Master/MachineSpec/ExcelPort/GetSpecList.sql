WITH spec_to_factory AS( -- 仕様項目IDに対する工場を取得
    SELECT
        spec.spec_id,
        re.factory_id,
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
                      st_f.structure_id = re.factory_id
                  AND st_f.factory_id IN(0, re.factory_id)
               )
          AND tra.structure_id = re.factory_id
        ) AS factory_name
    FROM
        ms_spec spec
    LEFT JOIN
        (
            SELECT DISTINCT
                re.spec_id,
                re.location_structure_id AS factory_id
            FROM
                ms_machine_spec_relation re
        ) re
    ON spec.spec_id = re.spec_id
)

SELECT
    spec.spec_id,                             -- 仕様項目ID
    stf.factory_id,                           -- 工場ID
    stf.factory_id AS factory_id_before,      -- 工場ID(変更前)
    stf.factory_name,                         -- 工場名
    spec.translation_id,                      -- 翻訳ID
    (
        SELECT
            mt.translation_text
        FROM
            ms_translation mt
        WHERE
            mt.location_structure_id = stf.factory_id
        AND mt.translation_id = spec.translation_id
        AND mt.language_id = @LanguageId
    ) spec_name,                              -- 仕様項目名
    (
        SELECT
            mt.translation_text
        FROM
            ms_translation mt
        WHERE
            mt.location_structure_id = stf.factory_id
        AND mt.translation_id = spec.translation_id
        AND mt.language_id = @LanguageId
    ) spec_name_before,                       -- 仕様項目名(変更前)
    spec.spec_type_id,                        -- 入力形式(構成ID)
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
                  st_f.structure_id = spec.spec_type_id
              AND st_f.factory_id IN(0, stf.factory_id)
           )
      AND tra.structure_id = spec.spec_type_id
    ) AS spec_type_name,                      -- 入力形式(名称)
    sndp.spec_num_decimal_places_id,          -- 数値書式(構成ID)
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
                  st_f.structure_id = sndp.spec_num_decimal_places_id
              AND st_f.factory_id IN(0, stf.factory_id)
           )
      AND tra.structure_id = sndp.spec_num_decimal_places_id
    ) AS spec_num_decimal_places_name,        -- 数値書式(名称)
    spec.spec_unit_type_id,                   -- 単位種別(構成ID)
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
                  st_f.structure_id = spec.spec_unit_type_id
              AND st_f.factory_id IN(0, stf.factory_id)
           )
      AND tra.structure_id = spec.spec_unit_type_id
    ) AS spec_unit_type_name,                 -- 単位種別(名称)
    spec.spec_unit_id,                        -- 単位(構成ID)
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
                  st_f.structure_id = spec.spec_unit_id
              AND st_f.factory_id IN(0, stf.factory_id)
           )
      AND tra.structure_id = spec.spec_unit_id
    ) AS spec_unit_name                       -- 単位(名称)
FROM
    ms_spec spec
    LEFT JOIN
        spec_to_factory stf
    ON  spec.spec_id = stf.spec_id
    LEFT JOIN -- 数値書式
        (
            SELECT
                ex.extension_data AS spec_num_decimal_places,
                ms.structure_id AS spec_num_decimal_places_id
            FROM
                ms_structure ms
                LEFT JOIN
                    ms_item_extension ex
                ON  ms.structure_item_id = ex.item_id
                AND ex.sequence_no = 1
            WHERE
                ms.structure_group_id = 2060
        ) sndp
    ON  spec.spec_num_decimal_places = sndp.spec_num_decimal_places
    LEFT JOIN
        (
            SELECT DISTINCT
                re.spec_id,
                re.display_order
            FROM
                ms_machine_spec_relation re
        ) re
    ON  spec.spec_id = re.spec_id

WHERE EXISTS(SELECT * FROM #temp_structure_selected temp WHERE temp.structure_group_id = 1000 AND stf.factory_id = temp.structure_id)
AND spec.delete_flg = 0

ORDER BY
    stf.factory_id,
    COALESCE(re.display_order, 0),
    spec.spec_id