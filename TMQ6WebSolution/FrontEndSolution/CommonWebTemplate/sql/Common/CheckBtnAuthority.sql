/*
 * ロール別権限チェック
 */

select distinct
       item.formno,
       item.btn_ctrlid,
       item.btn_actionkbn,
       item.btn_authcontrolkbn,
       item.optioninfo
  from (select *
          from com_listitem_define
         where pgmid = /*ConductId*/'%'
           and celltype like '08%'
           and btn_authcontrolkbn = 0
       ) item

union

select distinct
       item.formno,
       item.btn_ctrlid,
       item.btn_actionkbn,
       item.btn_authcontrolkbn,
       item.optioninfo
  from (select * 
          from com_listitem_define
         where pgmid = /*ConductId*/'%'
           and celltype like '08%'
           and btn_authcontrolkbn <> 0
       ) item
       inner join control_authority auth on
           auth.conductid = /*ConductId*/'%'
       and item.formno = auth.formno
       and item.ctrlid = auth.ctrlid
       and item.definetype = auth.definetype
       and item.itemno = auth.itemno
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
