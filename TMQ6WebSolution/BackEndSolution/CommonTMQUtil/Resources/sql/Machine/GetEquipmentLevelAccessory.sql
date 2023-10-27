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
        0 AS flg_level,
        0 AS flg_child,
        0 AS first_no,
        0 AS second_no
    FROM
        mc_machine_parent_info parent
        INNER JOIN
            mc_machine_parent_info machine
        ON  parent.parent_id = machine.parent_moto_id
        INNER JOIN
            mc_machine_parent_info accessory
        ON  machine.parent_id = accessory.parent_moto_id
        AND accessory.machine_id = @MachineId
),
wi_ma AS(
    --「機器」取得
    SELECT
        machine.machine_id,
        machine.parent_id,
        machine.update_serialid,
        CASE
            WHEN(
                SELECT
                    accessory.parent_moto_id
                FROM
                    mc_machine_parent_info accessory
                WHERE
                    accessory.machine_id = @MachineId
            ) = machine.parent_id THEN 1
            ELSE 0
        END AS flg_level,
        0 AS flg_child,
        ROW_NUMBER() OVER(ORDER BY wi_machine_name.machine_name) AS first_no,
        wi_pa.first_no AS second_no
    FROM
        mc_machine_parent_info machine
        INNER JOIN
            wi_pa
        ON  machine.parent_moto_id = wi_pa.parent_id
        LEFT JOIN
            wi_machine_name
        ON  machine.machine_id = wi_machine_name.machine_id
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
/*****************************    自分自身が「付属品」    ****************************/
SELECT
    CASE
        WHEN equip_level.flg_level = 1 
        THEN --dbo.get_translation_text(machine.equipment_level_structure_id,machine.location_structure_id,1170,@LanguageId) + '*' --自分自身と自分自身に紐付くものは「*」を付け加える          
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
    equip_level.parent_id,                                             --親子構成ID
    equip_level.update_serialid,                                       --更新シリアルID
    equip_level.flg_child,                                            --自分の子のデータの場合「1」
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
                WHEN accessory.machine_id = @MachineId THEN 1
                ELSE 0
            END AS flg_level,
            0 AS flg_child,
            wi_ma.first_no AS first_no,
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
