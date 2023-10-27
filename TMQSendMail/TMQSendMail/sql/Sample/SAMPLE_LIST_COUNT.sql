select
/*IF IsCount == False */
*
-- ELSE count(*)
/*END*/

from location
where location_cd is not null

/*IF LocationCd != null && LocationCd != ''*/
and (location_cd like /*LocationCd*/'%' or location_name like /*LocationCd*/'%')
/*END*/

/*IF AreaCd != null && AreaCd != ''*/
and area_cd = /*AreaCd*/'0'
/*END*/

order by LOCATION_CD

/*IF IsCount == False */
/*IF PageSize != null && PageSize > 0 */
FETCH NEXT /*PageSize*/10 ROWS ONLY
OFFSET /*Offset*/0 ROWS 
/*END*/
/*END*/
