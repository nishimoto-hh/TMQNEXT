WITH wi_ma AS(
    SELECT
        machine.machine_id,
        machine.parent_id,
        machine.update_serialid,
        1 AS flg_level,
        0 AS flg_child,
        0 AS first_no,
        0 AS second_no
    FROM
        mc_machine_parent_info machine
    WHERE
        machine_id = @MachineId
),
wi_machine_name AS(
    --「機器名称」取得用
    SELECT
        machine.machine_id,
        machine.machine_name
    FROM
        mc_machine machine
)
/*****************************    自分自身    ****************************/
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
            /**********自分自身**********/
            SELECT
                *
            FROM
                wi_ma
            /**********自分自身に紐付く付属品**********/
            UNION ALL
            SELECT
                accessory.machine_id,
                accessory.parent_id,
                accessory.update_serialid,
                1 AS flg_level,
                1 AS flg_child,
                1 AS first_no,
                ROW_NUMBER() OVER(ORDER BY wi_machine_name.machine_name) AS second_no
            FROM
                mc_machine_parent_info accessory
                INNER JOIN
                wi_ma
            ON  accessory.parent_moto_id = wi_ma.parent_id
            LEFT JOIN
                wi_machine_name
            ON  accessory.machine_id = wi_machine_name.machine_id
                --             
        ) AS equip_level
    ON  machine.machine_id = equip_level.machine_id
ORDER BY
    first_no,
    second_no
