/*
BackEndSolutionの共通SQL「Select_StructureItemExDataList」と同じ
構成グループID=9030 より、1ページ当たりの行数コンボの設定を取得するSQL
*/
WITH group_ex AS(
    SELECT
        sge.structure_group_id
        ,sge.sequence_no
        ,ie.extension_data
    FROM
        ms_structure_group_extension sge
        LEFT OUTER JOIN v_structure_item si
        ON  sge.data_type_structure_id = si.structure_id
        LEFT OUTER JOIN ms_item_extension ie
        ON  si.structure_item_id = ie.item_id
    WHERE
        sge.structure_group_id = 9030
)
SELECT
    ie.extension_data AS ex_data
FROM
    v_structure_item si
    LEFT OUTER JOIN ms_item_extension ie
    ON  si.structure_item_id = ie.item_id
    LEFT OUTER JOIN group_ex
    ON  si.structure_group_id = group_ex.structure_group_id
    AND ie.sequence_no = group_ex.sequence_no
WHERE
    si.structure_group_id = 9030
    AND si.language_id = @LanguageId
ORDER BY
    CAST(ie.extension_data as int)
