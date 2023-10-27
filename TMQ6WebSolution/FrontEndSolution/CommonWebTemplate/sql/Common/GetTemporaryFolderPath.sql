/*
構成グループID=9200より、テンポラリフォルダパスを取得するSQL
*/
SELECT TOP 1
    ie.extension_data AS temp_folder_path
FROM
    v_structure si
    LEFT OUTER JOIN ms_item_extension ie
    ON  si.structure_item_id = ie.item_id
WHERE
    si.structure_group_id = @StructureGroupId

