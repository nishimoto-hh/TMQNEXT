INSERT INTO attachment(
    [attachment_id],                      --添付ID
    [function_type_id],                   --機能タイプID
    [key_id],                             --キーID
    [attachment_type_structure_id],       --添付種類ID
    [file_path],                          --ファイルパス
    [file_name],                          --ファイル名
    [document_type_structure_id],         --文書種類ID
    [document_no],                        --文書番号
    [attachment_note],                    --文書説明
    [attachment_user_id],                 --作成者ID
    [attachment_date],                    --作成日
    [attachment_user_name],               --作成者
    [update_serialid],                    --更新シリアルID
    [insert_datetime],                    --登録日時
    [insert_user_id],                     --登録ユーザー
    [update_datetime],                    --更新日時
    [update_user_id]                      --更新ユーザー
)
VALUES(
    NEXT VALUE FOR seq_attachment_attachment_id,--添付ID
    @FunctionTypeId,                      --機能タイプID
    @KeyId,                               --キーID
    @AttachmentTypeStructureId,           --添付種類ID
    @FilePath,                            --ファイルパス
    @FileName,                            --ファイル名
    @DocumentTypeStructureId,             --文書種類ID
    @DocumentNo,                          --文書番号
    @AttachmentNote,                      --文書説明
    @AttachmentUserId,                    --作成者ID
    @AttachmentDate,                      --作成日
    @AttachmentUserName,                  --作成者
    @UpdateSerialid,                      --更新シリアルID
    @InsertDatetime,                      --登録日時
    @InsertUserId,                        --登録ユーザー
    @UpdateDatetime,                      --更新日時
    @UpdateUserId                         --更新ユーザー
)
