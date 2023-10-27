INSERT INTO #temp_structure_factory
-- 使用する構成グループの構成IDを絞込、工場の指定に用いる
SELECT
    structure_id
    ,location_structure_id AS factory_id
FROM
    v_structure_item_all
WHERE
    structure_group_id = @StructureGroupId
AND language_id = @LanguageId
AND structure_layer_no = @StructureLayerNo
;
/*@CreateIndex
CREATE NONCLUSTERED INDEX idx_temp_structure_factory_01
ON  #temp_structure_factory(
        structure_id
    )
;
UPDATE
    STATISTICS #temp_structure_factory
;
@CreateIndex*/