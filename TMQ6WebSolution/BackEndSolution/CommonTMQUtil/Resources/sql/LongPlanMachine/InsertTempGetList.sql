INSERT INTO #temp_structure_factory
-- 使用する構成グループの構成IDを絞込、工場の指定に用いる
SELECT
    structure_id
    ,location_structure_id AS factory_id
FROM
    v_structure_item_all
WHERE
    structure_group_id IN (
        0
	-- 点検種別
	,1240
    -- 部位
    /*@InspectionSiteName
        ,1180
    @InspectionSiteName*/
    -- 部位重要度
    /*@ImportanceName
        ,1200
    @ImportanceName*/
    -- 保全方式
    /*@InspectionSiteConservationName
        ,1030
    @InspectionSiteConservationName*/
    -- 保全項目
    /*@InspectionContentName
        ,1220
    @InspectionContentName*/
    -- 予算性格区分
    /*@BudgetPersonalityName
        ,1060
    @BudgetPersonalityName*/
    )
AND language_id = @LanguageId
;
CREATE NONCLUSTERED INDEX idx_temp_structure_factory_01
ON  #temp_structure_factory(
        structure_id
    )
;
UPDATE
    STATISTICS #temp_structure_factory
;