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
/*****************************    自分自身が「ループ」    ****************************/
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
    machine.machine_no,                                                --機器番号
    machine.machine_name,                                              --機器名称
    machine.job_structure_id,                                          --職種ID
    equip_level.loop_id,                                               --ループID
    equip_level.update_serialid,                                       --更新シリアルID
    equip_level.flg_child                                              --自分の子のデータの場合「1」
FROM
    mc_machine machine
    RIGHT JOIN
        (
            /**********①：自分自身(ループ)**********/
            SELECT
                loop.machine_id,
                loop.loop_id,
                loop.update_serialid,
                1 AS flg_level,
                0 AS flg_child,
                0 AS orderSort
            FROM
                mc_loop_info loop
            WHERE
                machine_id = @MachineId
            /**********②：①に紐づけられた機器レベル「機器」**********/
        UNION ALL
        SELECT
            child.machine_id,
            child.loop_id,
            child.update_serialid,
            1 AS flg_level,
            1 AS flg_child,
            1 AS orderSort
        FROM
            mc_loop_info child
        WHERE
            EXISTS(
                SELECT
                    *
                FROM
                    mc_loop_info loop
                WHERE
                    loop.machine_id = @MachineId
                AND child.loop_moto_id = loop.loop_id
            )
        ) AS equip_level
    ON  machine.machine_id = equip_level.machine_id
    ORDER BY orderSort,machine.machine_no
