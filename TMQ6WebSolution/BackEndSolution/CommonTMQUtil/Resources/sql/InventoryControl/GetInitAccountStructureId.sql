--******************************************************************
--予備品　勘定科目の初期表示値(B4140:設備貯蔵品)を取得する
--******************************************************************
SELECT
    ms.structure_id AS account_structure_id,
    ex1.extension_data AS account_cd,
    ex2.extension_data AS account_old_new_division,
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
    ) AS account_name
FROM
    ms_structure ms
    LEFT JOIN
        ms_item_extension ex1
    ON  ms.structure_item_id = ex1.item_id
    AND ex1.sequence_no = 1
    LEFT JOIN
        ms_item_extension ex2
    ON  ms.structure_item_id = ex2.item_id
    AND ex2.sequence_no = 2
WHERE
    ms.structure_group_id = 1770
AND ex1.extension_data = 'B4140'