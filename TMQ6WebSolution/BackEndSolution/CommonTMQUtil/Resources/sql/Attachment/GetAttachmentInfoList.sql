WITH attachmentTypeNo AS
(
SELECT
    structure_id,
    extension_data
FROM
    ms_structure ms
    LEFT JOIN
        ms_item_extension ex
    ON  ms.structure_item_id = ex.item_id
WHERE
    ms.structure_group_id = 1710
AND ex.sequence_no = 1
)
SELECT
    attachment_id,                                                                         --添付ID
    document_type_structure_id,                                                            --文書種類ID
    document_no,                                                                           --文書番号
    ac.update_serialid,                                                                    --更新シリアルID
    dbo.get_file_download_info_row(file_name , attachment_id , function_type_id , key_id , attachmentTypeNo.extension_data)  AS file_link_name, --ファイル/リンク
    CASE
        WHEN attachmentTypeNo.extension_data = 1 THEN ''
        ELSE file_name
    END AS link,                                                                           --リンク
    CASE
        WHEN attachmentTypeNo.extension_data = 2 THEN ''
        ELSE file_name
    END AS file_name,                                                                      --ファイル名
    attachment_type_structure_id,                                                          --添付種類ID
    function_type_id,                                                                      --機能タイプID
    attachment_note,                                                                       --文書説明
    attachment_date,                                                                       --作成日
    coalesce(person.display_name, attachment_user_name) AS person_name,                    --作成者
    key_id,                                                                                --キーID
    attachmentTypeNo.extension_data AS ExtensionData                                       --添付種類拡張項目
FROM
    attachment ac
    LEFT JOIN
        ms_user AS person
    ON  (
            ac.attachment_user_id = person.user_id
        )
    LEFT JOIN
        attachmentTypeNo
    ON  ac.attachment_type_structure_id = attachmentTypeNo.structure_id
WHERE
    function_type_id = @FunctionTypeId
AND 
    key_id = @KeyId

ORDER BY
    attachment_date DESC,
    document_type_structure_id,
    document_no
