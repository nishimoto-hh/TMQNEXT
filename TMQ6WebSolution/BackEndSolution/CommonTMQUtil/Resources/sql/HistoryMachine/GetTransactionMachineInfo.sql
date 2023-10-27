SELECT
    machine.machine_id,                                                             -- 機番ID
    machine.machine_no,                                                             -- 機器番号
    machine.machine_name,                                                           -- 機器名称
    machine.equipment_level_structure_id,                                           -- 機器レベル
    machine.installation_location,                                                  -- 設置場所
    machine.number_of_installation,                                                 -- 設置台数
    machine.date_of_installation,                                                   -- 設置年月
    machine.importance_structure_id,                                                -- 重要度
    machine.conservation_structure_id AS inspection_site_conservation_structure_id, -- 保全方式
    machine.machine_note,                                                           -- 機番メモ
    machine.location_structure_id,                                                  -- 場所階層
    machine.job_structure_id,                                                       -- 職種機種階層
    equipment.use_segment_structure_id,                                             -- 使用区分
    equipment.circulation_target_flg,                                               -- 循環対象
    equipment.fixed_asset_no,                                                       -- 固定資産番号
    equipment.manufacturer_structure_id,                                            -- メーカー
    equipment.manufacturer_type,                                                    -- メーカー型式
    equipment.model_no,                                                             -- 型式コード
    equipment.serial_no,                                                            -- 製造番号
    equipment.date_of_manufacture,                                                  -- 製造年月
    equipment.delivery_date,                                                        -- 納期
    equipment.maintainance_kind_manage,                                             -- 点検種別毎管理
    equipment.equipment_note,                                                       -- 機器メモ
    law.applicable_laws_structure_id                                                -- 適用法規
FROM
    mc_machine machine -- 機番情報
    LEFT JOIN
        mc_equipment equipment -- 機器情報
ON  machine.machine_id = equipment.machine_id
LEFT JOIN
    (
        SELECT
            machine.machine_id,
            trim(
                '|'
                FROM
                    (
                        SELECT
                            cast(law.applicable_laws_structure_id AS varchar) + '|'
                        FROM
                            mc_applicable_laws law
                        WHERE
                            machine.machine_id = law.machine_id FOR XML PATH('')
                    )
            ) AS applicable_laws_structure_id
        FROM
            mc_machine machine
        GROUP BY
            machine.machine_id
    ) law -- 適用法規
ON  machine.machine_id = law.machine_id
WHERE
    machine.machine_id = @MachineId