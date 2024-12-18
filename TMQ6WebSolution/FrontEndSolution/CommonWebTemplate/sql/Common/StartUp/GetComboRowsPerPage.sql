/*
BackEndSolutionの共通SQL「Select_StructureItemExDataList」と同じ
構成グループID=9030 より、1ページ当たりの行数コンボの設定を取得するSQL
*/
SELECT
    ie.extension_data AS ex_data
    ,si.language_id
FROM
    v_structure_item si
    LEFT OUTER JOIN ms_item_extension ie
    ON  si.structure_item_id = ie.item_id
WHERE
    si.structure_group_id = 9030
ORDER BY
    CAST(ie.extension_data as int)
