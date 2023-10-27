/*
* 画像ファイル情報取得
* Select_GetImageFileInfo
*/
SELECT
     dbo.get_img_key(att.attachment_id) AS file_info
    ,att.file_path
FROM
    attachment AS att
WHERE
    att.attachment_id = @AttachmentId
