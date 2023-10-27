SELECT
    numbering_id -- 採番ID
    , seq_no     -- 連番
FROM
    pt_partsno_numbering WITH(TABLOCKX)
WHERE
-- 指定された地区のデータ
    district_id = @DistrictId