SELECT
    COUNT(history.history_management_id) AS cnt
FROM
    hm_history_management history
    LEFT JOIN
        ms_structure ms
    ON  history.application_status_id = ms.structure_id
    LEFT JOIN
        ms_item_extension ex
    ON  ms.structure_item_id = ex.item_id
    AND ex.sequence_no = 1
WHERE
    history.key_id = @KeyId
AND history.application_conduct_id =@ApplicationConductId

/*@Include
-- 指定された申請状況のデータ
AND ex.extension_data = @AppliCationStatus
@Include*/

/*@Except
-- された申請状況以外のデータ
AND ex.extension_data != @AppliCationStatus
@Except*/
