/*
 ユーザーマスタ(ms_user)更新登録SQL
*/
UPDATE
    ms_user
SET 
    login_id = @LoginId,
    language_id = @LanguageId,
    authority_level_id = @AuthorityLevelId,
    display_name = @DisplayName,
    family_name = @FamilyName,
    first_name = @FirstName,
    mail_address = @MailAddress,
    update_serialid = update_serialid + 1,
    delete_flg = @DeleteFlg,
    update_datetime = @UpdateDatetime,
    update_user_id = @UpdateUserId
WHERE
    user_id = @UserId