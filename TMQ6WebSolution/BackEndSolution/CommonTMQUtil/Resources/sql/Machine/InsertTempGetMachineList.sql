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
    -- 機器レベル
    /*@EquipmentLevel
        ,1170
    @EquipmentLevel*/
	-- 重要度
	/*@ImportanceName
        ,1200
    @ImportanceName*/
	-- 保全方式
	/*@InspectionSiteConservationName
        ,1030
    @InspectionSiteConservationName*/
	-- メーカー
	/*@ManufacturerName
        ,1150
    @ManufacturerName*/
	-- 使用区分
	/*@UseSegmentName
        ,1210
    @UseSegmentName*/
	-- 適用法規
	/*@ApplicableLawsName
        ,1160
    @ApplicableLawsName*/
	-- 場所階層
	/*@Location
        ,1000
    @Location*/
	-- 職種機種
	/*@Job
        ,1010
    @Job*/
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