SELECT DISTINCT
    dbo.get_target_layer_id(location_structure_id,0) AS districtId
FROM
    (
        SELECT
            ms.structure_id AS location_structure_id
        FROM
            ms_structure ms
        WHERE
            ms.structure_group_id IN(1000)
    ) AS tbl 