INSERT INTO #temp_progress
--進捗状況
SELECT
    ms.structure_id
    ,mie.extension_data
FROM
    ms_structure ms
    LEFT JOIN
        ms_item mi
    ON  ms.structure_item_id = mi.item_id
    LEFT JOIN
        ms_item_extension mie
    ON  mi.item_id = mie.item_id
WHERE
    ms.structure_group_id = 1900
AND mie.sequence_no = 1;
INSERT INTO #get_factory
--故障分析個別工場フラグ
SELECT
    ms.structure_id
    ,mie.extension_data
FROM
    ms_structure ms
    LEFT JOIN
        ms_item mi
    ON  ms.structure_item_id = mi.item_id
    LEFT JOIN
        ms_item_extension mie
    ON  mi.item_id = mie.item_id
WHERE
    --場所階層
    ms.structure_group_id = 1000
AND
    --工場
    ms.structure_layer_no = 1
AND
    --故障分析個別工場フラグ
    mie.sequence_no = 2;

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
    /*@MqClassName
        ,1850
    @MqClassName*/
    /*@StopSystemName
        ,1130
    @StopSystemName*/
    /*@SuddenDivisionName
        ,1400
    @SuddenDivisionName*/
    /*@BudgetManagementName
        ,1300
    @BudgetManagementName*/
    /*@BudgetPersonalityName
        ,1060
    @BudgetPersonalityName*/
    /*@MaintenanceSeasonName
        ,1330
    @MaintenanceSeasonName*/
    /*@DiscoveryMethodsName
        ,1080
    @DiscoveryMethodsName*/
    /*@ActualResultName
        ,1140
    @ActualResultName*/
    /*@PhenomenonName
        ,1510
    @PhenomenonName*/
    /*@FailureCauseName
        ,1810
    @FailureCauseName*/
    /*@TreatmentMeasureName
        ,1050
    @TreatmentMeasureName*/
    /*@ProgressName
        ,1900
    @ProgressName*/
    /*@Location
        ,1000
    @Location*/
    /*@FailureCausePersonality
        ,1020
    @FailureCausePersonality*/
    )
AND language_id = @LanguageId
/*@JobName
UNION ALL
SELECT
    structure_id
    ,location_structure_id AS factory_id
FROM
    v_structure_item_all
WHERE
    structure_group_id = 1010
AND language_id = @LanguageId
AND structure_layer_no = 0
@JobName*/
;
CREATE NONCLUSTERED INDEX idx_temp_structure_factory_01
ON  #temp_structure_factory(
        structure_id
    )
;
UPDATE
    STATISTICS #temp_structure_factory
;