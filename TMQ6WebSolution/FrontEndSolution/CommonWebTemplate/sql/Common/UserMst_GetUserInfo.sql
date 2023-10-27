SELECT
      mu.user_id AS UserId
    , mu.login_id AS LoginId
    , mu.language_id AS LanguageId
    , mu.authority_level_id AS AuthorityLevelId
    , mu.display_name AS DisplayName
    , mu.family_name AS FamilyName
    , mu.first_name AS FirstName
    , mu.mail_address AS MailAddress
FROM
    ms_user mu 
    
WHERE
    mu.delete_flg != 1 
/*IF UserId != null*/
    AND mu.user_id = /*UserId*/'1001'
/*END*/
/*IF LoginId != null*/
    AND mu.login_id = /*LoginId*/'demo'
/*END*/
/*IF MailAdress != null*/
    AND mu.mail_address = /*MailAdress*/'demo@demo.com'
/*END*/
