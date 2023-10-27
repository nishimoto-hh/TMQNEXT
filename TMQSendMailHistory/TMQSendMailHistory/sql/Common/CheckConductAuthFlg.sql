/*
 * 認証有無チェック
 */

select extend_info1
  from names
 where name_division = 'WFDV'
   and (name02 = /*ConductId*/'%'
    or  name03 = /*ConductId*/'%')
