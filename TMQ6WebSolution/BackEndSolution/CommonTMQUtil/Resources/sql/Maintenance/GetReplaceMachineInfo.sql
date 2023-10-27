SELECT
    mc.machine_id                             --機番ID
    , mc.machine_no                           --機器番号
    , mc.machine_name                         --機器名称
    , mc.installation_location                --設置場所
    , eq.equipment_id                         --機器ID
    , eq.serial_no                            --製造番号
    , eq.use_segment_structure_id             --使用区分
    , eq.update_serialid                      --更新シリアルID
FROM
    mc_machine mc
    INNER JOIN mc_equipment eq
    ON mc.machine_id = eq.machine_id
WHERE
    mc.machine_id = @MachineId
