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
	--保全時期
	/*@MaintenanceSeasonName
		,1330
	@MaintenanceSeasonName*/
	--作業項目
	/*@WorkItemName
		,1280
	@WorkItemName*/
	--予算管理区分
	/*@BudgetManagementName
		,1300
	@BudgetManagementName*/
	--予算性格区分
	/*@BudgetPersonalityName
		,1060
	@BudgetPersonalityName*/
	--目的区分
	/*@PurposeName
		,1290
	@PurposeName*/
	--作業区分
	/*@WorkClassName
		,1410
	@WorkClassName*/
	--処置区分
	/*@TreatmentName
		,1310
	@TreatmentName*/
	--設備区分
	/*@FacilityStructureName
		,1320
	@FacilityStructureName*/
	--長計区分
	/*@LongPlanDivisionName
		,2210
	@LongPlanDivisionName*/
	--長計グループ
	/*@LongPlanGroupName
		,2220
	@LongPlanGroupName*/
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