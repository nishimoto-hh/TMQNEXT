/*
 * ユーザ認証
 */

select count(*) as Count
  from ms_login
 where user_id is not null
   and user_id = /*userId*/'1001'
   AND CONVERT(NVARCHAR, DECRYPTBYPASSPHRASE(/*encryptKey*/'key',login_password)) = /*password*/'demo'
