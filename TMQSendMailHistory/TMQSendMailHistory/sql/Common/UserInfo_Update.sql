/*
 * パスワード変更
 */

update ms_login
set
	login_password = ENCRYPTBYPASSPHRASE(/*encryptKey*/'key', /*password*/'%'),
	update_datetime = CURRENT_TIMESTAMP,
	update_user_id = /*userId*/'%'
where user_id = /*userId*/'%'
