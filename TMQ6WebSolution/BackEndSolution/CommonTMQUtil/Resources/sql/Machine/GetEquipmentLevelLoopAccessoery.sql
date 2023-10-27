WITH
structure_factory AS (
    SELECT
        structure_id
        , location_structure_id AS factory_id 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id IN (1170) 
        AND language_id = @LanguageId
) 
/*****************************    自分自身が「付属品」    ****************************/
SELECT
    CASE
        WHEN flg_level = 1 THEN --dbo.get_translation_text(machine.equipment_level_structure_id,machine.location_structure_id,1170,@LanguageId) + '*' --自分自身と自分自身に紐付くものは「*」を付け加える          
		( 
		SELECT
			tra.translation_text 
		FROM
			v_structure_item_all AS tra 
		WHERE
			tra.language_id = @LanguageId
			AND tra.location_structure_id = ( 
				SELECT
					MAX(st_f.factory_id) 
				FROM
					structure_factory AS st_f 
				WHERE
					st_f.structure_id = machine.equipment_level_structure_id
					AND st_f.factory_id IN (0, machine.location_factory_structure_id)
			) 
			AND tra.structure_id = machine.equipment_level_structure_id
		) + '*' 
        ELSE -- dbo.get_translation_text(machine.equipment_level_structure_id,machine.location_structure_id,1170,@LanguageId)
        ( 
		SELECT
			tra.translation_text 
		FROM
			v_structure_item_all AS tra 
		WHERE
			tra.language_id = @LanguageId
			AND tra.location_structure_id = ( 
				SELECT
					MAX(st_f.factory_id) 
				FROM
					structure_factory AS st_f 
				WHERE
					st_f.structure_id = machine.equipment_level_structure_id
					AND st_f.factory_id IN (0, machine.location_factory_structure_id)
			) 
			AND tra.structure_id = machine.equipment_level_structure_id
		)
    END AS equipment_level,                                            --機器レベル
    machine_no,                                                        --機器番号
    machine_name,                                                      --機器名称
    job_structure_id,                                                  --職種ID
    equip_level.loop_id,                                               --ループID
    equip_level.update_serialid,                                       --更新シリアルID
    flg_child                                                          --自分の子のデータの場合「1」
FROM
    mc_machine machine
    RIGHT JOIN
        (
            /**********①：自分自身(付属品)**********/
            SELECT
                accessory.machine_id,
                accessory.loop_id,
                accessory.update_serialid,
                1 AS flg_level,
                0 AS flg_child
            FROM
                mc_loop_info accessory
            WHERE
                machine_id = @MachineId
            /**********②：①に紐づけられた機器レベル「機器」**********/
        UNION ALL
        SELECT
            child.machine_id,
            child.loop_id,
            child.update_serialid,
            1 AS flg_level,
            0 AS flg_child
        FROM
            mc_loop_info child
        WHERE
            EXISTS(
                SELECT
                    *
                FROM
                    mc_loop_info accessory
                WHERE
                    machine_id = @MachineId
                AND child.loop_id = accessory.loop_moto_id
            )
        /**********③：②に紐づけられた機器レベル「ループ」**********/
    UNION ALL
    SELECT
        loop.machine_id,
        loop.loop_id,
        loop.update_serialid,
        0 AS flg_level,
        0 AS flg_child
    FROM
        mc_loop_info loop
    WHERE
        EXISTS(
            SELECT
                *
            FROM
                mc_loop_info child
            WHERE
                EXISTS(
                    SELECT
                        *
                    FROM
                        mc_loop_info accessory
                    WHERE
                        machine_id = @MachineId
                    AND child.loop_id = accessory.loop_moto_id
                )
            AND loop.loop_id = child.loop_moto_id
        )
    /**********④：③に紐づけられたすべての機器レベル「機器」**********/
UNION ALL
SELECT
    all_child.machine_id,
    all_child.loop_id,
    all_child.update_serialid,
    0 AS flg_level,
    0 AS flg_child
FROM
    mc_loop_info all_child
WHERE
    EXISTS(
        SELECT
            *
        FROM
            mc_loop_info loop
        WHERE
            EXISTS(
                SELECT
                    *
                FROM
                    mc_loop_info child
                WHERE
                    EXISTS(
                        SELECT
                            *
                        FROM
                            mc_loop_info accessory
                        WHERE
                            machine_id = @MachineId
                        AND child.loop_id = accessory.loop_moto_id
                    )
                AND loop.loop_id = child.loop_moto_id
            )
        AND all_child.loop_moto_id = loop.loop_id
    )
AND NOT EXISTS(
        SELECT
            *
        FROM
            mc_loop_info except_child
        WHERE
            EXISTS(
                SELECT
                    *
                FROM
                    mc_loop_info accessory
                WHERE
                    machine_id = @MachineId
                AND except_child.loop_id = accessory.loop_moto_id
            )
        AND all_child.machine_id = except_child.machine_id
    )
/**********⑤：③に紐づけられた機器の自分自身以外のすべての付属品**********/
UNION ALL
SELECT
    other_accessory.machine_id,
    other_accessory.loop_id,
    other_accessory.update_serialid,
    0 AS flg_level,
    0 AS flg_child
FROM
    mc_loop_info other_accessory
WHERE
    EXISTS(
        SELECT
            *
        FROM
            mc_loop_info all_child
        WHERE
            EXISTS(
                SELECT
                    *
                FROM
                    mc_loop_info loop
                WHERE
                    EXISTS(
                        SELECT
                            *
                        FROM
                            mc_loop_info child
                        WHERE
                            EXISTS(
                                SELECT
                                    *
                                FROM
                                    mc_loop_info accessory
                                WHERE
                                    machine_id = @MachineId
                                AND child.loop_id = accessory.loop_moto_id
                            )
                        AND loop.loop_id = child.loop_moto_id
                    )
                AND all_child.loop_moto_id = loop.loop_id
            )
        AND other_accessory.loop_moto_id = all_child.loop_id
    )
AND machine_id <> @MachineId
        ) AS equip_level
    ON  machine.machine_id = equip_level.machine_id
    ORDER BY machine.machine_no
