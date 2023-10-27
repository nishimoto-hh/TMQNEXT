WITH wi_machine_name AS(
    --「機器名称」取得用
    SELECT
        machine.machine_id,
        machine.machine_name
    FROM
        mc_machine machine
),
wi_pa AS(
    --「親機」取得
    SELECT
        parent.machine_id,
        parent.parent_id,
        parent.update_serialid,
        1 AS flg_level,
        0 AS flg_child,
        0 AS first_no,
        0 AS second_no
    FROM
        mc_machine_parent_info parent
        INNER JOIN
            mc_machine_parent_info machine
        ON  parent.parent_id = machine.parent_moto_id
        AND machine.machine_id = @MachineId
),
wi_ma AS(
    --「機器」取得
    SELECT
        machine.machine_id,
        machine.parent_id,
        machine.update_serialid,
        CASE
            WHEN machine.machine_id = @MachineId THEN 1
            ELSE 0
        END flg_level,
        0 AS flg_child,
        ROW_NUMBER() OVER(ORDER BY wi_machine_name.machine_name) first_no,
        wi_pa.first_no AS second_no
    FROM
        mc_machine_parent_info machine
        INNER JOIN
            wi_pa
        ON  machine.parent_moto_id = wi_pa.parent_id
        LEFT JOIN
            wi_machine_name
        ON  machine.machine_id = wi_machine_name.machine_id
)
/*****************************    自分自身が「機器」    ****************************/
SELECT
    CASE
        WHEN equip_level.flg_level = 1 THEN dbo.get_translation_text(machine.equipment_level_structure_id,machine.location_structure_id,1170,@LanguageId) + '*' --自分自身と自分自身に紐付くものは「*」を付け加える
        ELSE dbo.get_translation_text(machine.equipment_level_structure_id,machine.location_structure_id,1170,@LanguageId)
    END AS equipment_level,                                            --機器レベル
    machine.machine_no,                                                --機器番号
    machine.machine_name,                                              --機器名称
    machine.job_structure_id,                                          --職種ID
    equip_level.parent_id,                                             --親子構成ID
    equip_level.update_serialid,                                       --更新シリアルID
    equip_level.flg_child,                                             --自分の子のデータの場合「1」
    equip_level.first_no,
    equip_level.second_no
FROM
    mc_machine machine
    RIGHT JOIN
        (
            /**********①：親機**********/
            SELECT
                *
            FROM
                wi_pa
            /**********②：機器**********/
            UNION ALL
            SELECT
                *
            FROM
                wi_ma
            /**********③：付属品**********/
            UNION ALL
            SELECT
                accessory.machine_id,
                accessory.parent_id,
                accessory.update_serialid,
            CASE
                WHEN wi_ma.machine_id = @MachineId THEN 1
                ELSE 0
            END flg_level,
            CASE
                WHEN wi_ma.machine_id = @MachineId THEN 1
            ELSE 0
            END flg_child,
            wi_ma.first_no,
            ROW_NUMBER() OVER(ORDER BY wi_machine_name.machine_name) AS second_no
            FROM
                mc_machine_parent_info accessory
            INNER JOIN
                wi_ma
            ON  accessory.parent_moto_id = wi_ma.parent_id
            LEFT JOIN
            wi_machine_name
        ON  accessory.machine_id = wi_machine_name.machine_id
        ) AS equip_level
    ON  machine.machine_id = equip_level.machine_id
ORDER BY
    first_no,
    second_no
