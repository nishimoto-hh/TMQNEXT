SELECT
    mac.machine_no,                          -- 機器番号
	mac.machine_name,                        -- 機器名称
	pt.parts_name,                           -- 部品名
    mc.parts_id AS parts_cd,                 -- 部品コード
	mac.job_structure_id,                    -- 職種機種階層ID
	--職種(翻訳)
    (
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.languageId
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = mac.job_kind_structure_id
                AND st_f.factory_id IN(0, temp.factoryId)
            )
        AND tra.structure_id = mac.job_kind_structure_id
    ) AS job_name,
    (
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = temp.languageId
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = use_segment_structure_id
                    AND st_f.factory_id IN (0, temp.factoryId)
            )
            AND tra.structure_id = use_segment_structure_id
    ) AS management_division,              -- 管理区分
    standard_size AS dimensions,             -- 規格・寸法
    (
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = temp.languageId
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = pt.manufacturer_structure_id
                    AND st_f.factory_id IN (0, temp.factoryId)
            )
            AND tra.structure_id = pt.manufacturer_structure_id
    ) AS maker,                            -- メーカー
	unit_price,                              -- 単価
    use_quantity,                            -- 使用個数
	unit_structure_id,                       -- 単位コード
	parts_memo,                              -- 部品メモ
    machine_use_parts_id                     -- 機番使用部品情報ID
    ,STUFF(
        (
            -- 機器に紐づく機器別管理基準部位の名称を機器ごとにカンマ区切りで取得
            SELECT ',' + inspection_site_name 
            FROM
            (
                SELECT machine_id,
                        (
                         SELECT
                             tra.translation_text 
                         FROM
                             v_structure_item_all AS tra 
                         WHERE
                             tra.language_id = temp.languageId
                         AND tra.location_structure_id = ( 
                             SELECT
                                 MAX(st_f.factory_id) 
                             FROM
                                 #temp_structure_factory AS st_f 
                             WHERE
                                 st_f.structure_id = inspection_site_structure_id
                             AND st_f.factory_id IN (0, temp.factoryId)
                         )
                         AND tra.structure_id = inspection_site_structure_id
                        ) AS inspection_site_name
                FROM (
                    SELECT mc_machine.machine_id, inspection_site_structure_id, temp.factoryId, temp.languageId
                    FROM mc_machine
                    INNER JOIN #temp temp
                        ON mc_machine.machine_id = temp.Key1
                    INNER JOIN mc_management_standards_component
                        ON mc_machine.machine_id = mc_management_standards_component.machine_id
                    GROUP BY mc_machine.machine_id, inspection_site_structure_id, temp.factoryId, temp.languageId
                ) tbl1
            ) tbl2
            WHERE tbl2.machine_id = mac.machine_id
            FOR XML PATH(''))
        , 1, 1, ''        
    ) AS inspection_site_name -- 部位
    , '1' AS output_report_location_name_got_flg                -- 機能場所名称情報取得済フラグ（帳票用）
    , '1' AS output_report_job_name_got_flg                     -- 職種・機種名称情報取得済フラグ（帳票用）
FROM
    mc_machine mac
    INNER JOIN #temp temp
        ON mac.machine_id = temp.Key1
	JOIN
	    mc_machine_use_parts mc
	ON  mac.machine_id = mc.machine_id
	JOIN
        pt_parts pt
    ON  mc.parts_id = pt.parts_id
WHERE  mc.machine_id = temp.Key1
ORDER BY
    mac.machine_no, mac.machine_name, machine_use_parts_id

