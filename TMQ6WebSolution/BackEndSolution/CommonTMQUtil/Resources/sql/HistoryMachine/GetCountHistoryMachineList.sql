SELECT COUNT(*)
FROM
(
SELECT
    history.history_management_id,
    COALESCE(hmachine.location_structure_id, machine.location_structure_id) AS location_structure_id,
    COALESCE(hmachine.job_structure_id, machine.job_structure_id) AS job_structure_id
FROM
    hm_history_management history -- 変更管理
    LEFT JOIN
        hm_mc_machine hmachine -- 機番情報変更管理
    ON  history.history_management_id = hmachine.history_management_id
    AND history.key_id = hmachine.machine_id
    LEFT JOIN
        mc_machine machine -- 機番情報
    ON history.key_id = machine.machine_id
    LEFT JOIN
        ms_structure status_ms -- 構成マスタ(申請状況)
    ON  history.application_status_id = status_ms.structure_id
    LEFT JOIN
        ms_item_extension status_ex -- アイテムマスタ拡張(申請状況)
    ON  status_ms.structure_item_id = status_ex.item_id
    AND status_ex.sequence_no = 1

WHERE
    -- 「申請データ作成中」「承認依頼中」「差戻中」のデータのみ
    status_ex.extension_data IN('10', '20', '30')
-- 「1：機器台帳」のデータのみ
AND history.application_conduct_id = 1
    /*@DispOnlyMySubject
       -- 自分の件名のみ表示
       AND history.application_user_id = @UserId
    @DispOnlyMySubject*/
) tbl
WHERE
