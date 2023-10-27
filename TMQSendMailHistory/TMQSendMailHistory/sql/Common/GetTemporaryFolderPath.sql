/*
構成グループID=9200より、テンポラリフォルダパスを取得するSQL
*/
WITH group_ex AS(
    SELECT
        sge.structure_group_id
        ,sge.sequence_no
        ,ie.extension_data
    FROM
        ms_structure_group_extension sge
        LEFT OUTER JOIN ms_structure ms
        ON  sge.data_type_structure_id = ms.structure_id
        LEFT OUTER JOIN ms_item_extension ie
        ON  ms.structure_item_id = ie.item_id
    WHERE
        sge.structure_group_id = @StructureGroupId
)
SELECT TOP 1
    ie.extension_data AS temp_folder_path
FROM
    v_structure_item si
    LEFT OUTER JOIN ms_item_extension ie
    ON  si.structure_item_id = ie.item_id
    LEFT OUTER JOIN group_ex
    ON  si.structure_group_id = group_ex.structure_group_id
    AND ie.sequence_no = group_ex.sequence_no
WHERE
    si.structure_group_id = @StructureGroupId

