SELECT
    law.applicable_laws_id,           -- 適用法規ID
    law.applicable_laws_structure_id, -- 適用法規アイテムID
    law.machine_id                    -- 機番ID
FROM
    mc_applicable_laws law
WHERE
    law.machine_id = @MachineId