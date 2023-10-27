SELECT
    mac.machine_no,                          -- 機器番号
	mac.machine_name,                        -- 機器名称
	-- '■' AS bui,                             -- 部位(未定)
	pt.parts_name,                           -- 部品名
    mc.parts_id AS parts_cd,                 -- 部品コード
	mac.job_structure_id,                    -- 職種機種階層ID
	[dbo].[get_v_structure_item](use_segment_structure_id, temp.factoryId, temp.languageId) AS management_division,  -- 管理区分
    standard_size AS dimensions,             -- 規格・寸法
    [dbo].[get_v_structure_item](pt.manufacturer_structure_id, temp.factoryId, temp.languageId) AS maker,            -- メーカー
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
                        [dbo].[get_v_structure_item](inspection_site_structure_id, factoryId, languageId) AS inspection_site_name
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

