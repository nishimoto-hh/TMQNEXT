SELECT
    law.hm_applicable_laws_id,        -- 適用法規変更管理ID
    law.applicable_laws_id,           -- 適用法規ID
    law.applicable_laws_structure_id, -- 適用法規アイテムID
    law.machine_id                    -- 機番ID
FROM
    hm_mc_applicable_laws law
WHERE
    law.history_management_id = @HistoryManagementId