WITH wi_machine_name AS(
    --「機器名称」取得用
    SELECT
        machine.machine_id,
        machine.machine_name
    FROM
        mc_machine machine
),
wi_loop AS( 
    SELECT
        loop.machine_id,
        loop.loop_id,
        loop.loop_moto_id,
        loop.update_serialid,
        1 AS flg_level,
        0 AS flg_child,
        ROW_NUMBER() OVER(ORDER BY wi_machine_name.machine_name) AS first_no,
        0 AS second_no
    FROM
        mc_loop_info loop
        LEFT JOIN
            wi_machine_name
        ON  loop.machine_id = wi_machine_name.machine_id
    WHERE
        EXISTS(
            SELECT
                *
            FROM
                mc_loop_info machine
            WHERE
                machine_id = @MachineId
            AND loop.loop_id = machine.loop_moto_id
        )
),
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
/*****************************    自分自身が「機器」    ****************************/
SELECT
    CASE
        WHEN equip_level.flg_level = 1 THEN  --dbo.get_translation_text(machine.equipment_level_structure_id,machine.location_structure_id,1170,@LanguageId) + '*' --自分自身と自分自身に紐付くものは「*」を付け加える          
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
    machine.machine_no,                                                            --機器番号
    machine.machine_name,                                                          --機器名称
    machine.job_structure_id,                                                      --職種ID
    equip_level.loop_id,                                                           --ループID
    equip_level.update_serialid,                                                   --更新シリアルID
    equip_level.flg_child                                                          --自分の子のデータの場合「1」
FROM
    mc_machine machine
    RIGHT JOIN
        (
            /**********機器レベル「ループ」**********/
            SELECT
                *
            FROM
                wi_loop
            /**********機器レベル「機器」**********/
            UNION ALL
            SELECT
                machine.machine_id,
                machine.loop_id,
                machine.loop_moto_id,
                machine.update_serialid,
                CASE
                    WHEN machine.machine_id = @MachineId THEN 1
                   ELSE 0
                END AS flg_level,
                0 AS flg_child,
                wi_loop.first_no AS first_no,
                ROW_NUMBER() OVER(ORDER BY wi_machine_name.machine_name) AS second_no
            FROM
                (
                    SELECT
                        machine.machine_id,
                        machine.loop_id,
                        machine.loop_moto_id,
                        machine.update_serialid
                    FROM
                        mc_loop_info machine
                    WHERE
                        machine.machine_id = @MachineId
                    UNION ALL
                    SELECT
                        all_child.machine_id,
                        all_child.loop_id,
                        all_child.loop_moto_id,
                        all_child.update_serialid
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
                                        mc_loop_info machine
                                    WHERE
                                        machine.machine_id = @MachineId
                                    AND loop.loop_id = machine.loop_moto_id
                                )
                            AND all_child.machine_id <> @MachineId
                            AND all_child.loop_moto_id = loop.loop_id
                        )
                ) machine
            LEFT JOIN
                wi_loop
            ON  wi_loop.loop_id = machine.loop_moto_id
            LEFT JOIN
                wi_machine_name
            ON  machine.machine_id = wi_machine_name.machine_id
        ) AS equip_level
    ON  machine.machine_id = equip_level.machine_id
ORDER BY
    first_no,
    second_no
