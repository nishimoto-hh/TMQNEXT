SELECT
    history.history_management_id
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

-- 「申請データ作成中」「承認依頼中」「差戻中」のデータのみ
AND ex.extension_data IN('10', '20', '30')
