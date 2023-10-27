UPDATE
    attachment
SET
    attachment_type_structure_id = @AttachmentTypeStructureId,
    file_path = @FilePath,
    file_name = @FileName,
    document_type_structure_id = @DocumentTypeStructureId,
    document_no = @DocumentNo,
    attachment_note = @AttachmentNote,
    attachment_user_id = @AttachmentUserId,
    attachment_date = @AttachmentDate,
    attachment_user_name = @AttachmentUserName,
    update_serialid = update_serialid + 1,
    update_datetime = @UpdateDatetime,
    update_user_id = @UpdateUserId
WHERE
    attachment_id = @AttachmentId
