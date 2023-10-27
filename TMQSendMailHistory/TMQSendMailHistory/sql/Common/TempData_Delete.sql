delete from com_temp_data
where 1=1
/*IF UserId != null && UserId != ''*/
and user_id = /*UserId*/'demo'
/*END*/
/*IF DeleteLimitDate != null*/
and (guid in(select guid from com_session where logout_date is null and login_date < /*DeleteLimitDate*/'2021/07/30'))
/*END*/
/*IF GUID != null && GUID != ''*/
and guid = /*GUID*/'4004a80e-4df0-41f5-9f9c-bcb7fa692b38'
/*END*/
/*IF BrowserTabNo != null && BrowserTabNo != ''*/
and tabno = /*BrowserTabNo*/'00016272605448592486'
/*END*/
