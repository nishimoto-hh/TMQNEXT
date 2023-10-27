SELECT
MAX(pih.update_serialid) AS update_serialid
FROM
    pt_inout_history pih
WHERE
   pih.work_no = @WorkNo
