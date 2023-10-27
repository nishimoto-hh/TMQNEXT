/*
 * ExcelPortダウンロード対象機能 マスタ-工場毎年度期首月(1～12)
 */
-- EPC0012
WITH tbl AS(
    SELECT
        *
    FROM
        STRING_SPLIT('1,2,3,4,5,6,7,8,9,10,11,12', ',')
)
SELECT
    tbl.VALUE AS id,
    tbl.VALUE AS name,
    0 AS factory_id
FROM
    tbl