--******************************************************************
--予備品　新旧区分の初期表示値(0:新品)を取得する
--******************************************************************
SELECT
    ms.structure_id AS old_new_structure_id,
    ex1.extension_data AS old_new_division_cd,
    (
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.location_structure_id)
                FROM
                    v_structure_item_all AS st_f
                WHERE
                    st_f.location_structure_id IN(0, @FactoryId)
                AND st_f.structure_id = ms.structure_id
            )
        AND tra.structure_id = ms.structure_id
    ) AS old_new_division_name
FROM
    ms_structure ms
    LEFT JOIN
        ms_item_extension ex1
    ON  ms.structure_item_id = ex1.item_id
    AND ex1.sequence_no = 1
WHERE
    ms.structure_group_id = 1940
AND ex1.extension_data = '0'
and ms.delete_flg = 0