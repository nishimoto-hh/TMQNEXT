/*
 * ロール別権限チェック
 */

select distinct
       auth.formno,
       auth.ctrlid as btn_ctrlid
  from control_authority auth
 where conductid = /*Conductid*/'%'
   and ctrlid = /*Ctrlid*/'%'
   and auth.role_id in (select belong_role.role_id
                          from login
                               inner join belong on
                                   login.user_id = belong.user_id
                               inner join belong_role on
                                   belong.organization_cd = belong_role.organization_cd
                               and belong.post_id = belong_role.post_id
                         where login.user_id = /*UserId*/'%'

                        union

                        select user_role.role_id
                          from login
                               inner join user_role on
                                   login.user_id = user_role.user_id
                         where login.user_id = /*UserId*/'%'
                           )

order by formno
