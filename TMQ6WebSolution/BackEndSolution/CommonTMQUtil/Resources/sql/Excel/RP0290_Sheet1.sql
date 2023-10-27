SELECT
    ROW_NUMBER() OVER (ORDER BY parts_name) AS row_no,
    pp.parts_name, -- 品名
    ISNULL(pp.model_type,'') + ISNULL(pp.standard_size,'') AS model_type, -- 型式(仕様)

    pp.manufacturer_structure_id, -- メーカー
    [dbo].[get_v_structure_item](pp.manufacturer_structure_id, temp.factoryId, temp.languageId) AS manufacturer_name,

    pp.order_quantity AS order_quantity,

    pp.unit_structure_id, -- 数量管理単位id
    [dbo].[get_v_structure_item](pp.unit_structure_id, temp.factoryId, temp.languageId) AS unit_name
FROM
    pt_parts pp 
    INNER JOIN #temp temp
        ON pp.parts_id = temp.Key1
ORDER BY
    pp.parts_name                                       -- 品名
