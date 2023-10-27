/*@LocationStructureIds
DROP TABLE IF EXISTS #temp_condition_location; 
CREATE TABLE #temp_condition_location(structure_id int);
INSERT 
INTO #temp_condition_location 
SELECT
    * 
FROM
    STRING_SPLIT(@LocationStructureIds, ',');
@LocationStructureIds*/
/*@JobStructureIds
DROP TABLE IF EXISTS #temp_condition_job; 
CREATE TABLE #temp_condition_job(structure_id int); 
INSERT 
INTO #temp_condition_job 
SELECT
    * 
FROM
    STRING_SPLIT(@JobStructureIds, ',');
@JobStructureIds*/

--変更管理対象の工場を取得
DROP TABLE IF EXISTS #temp_history_factory; 
CREATE TABLE #temp_history_factory(structure_id INT); 
INSERT 
INTO #temp_history_factory 
SELECT
    vs.structure_id
FROM
    v_structure AS vs
WHERE
    EXISTS(
        SELECT
            *
        FROM
            ms_item_extension AS ex
        WHERE
            ex.item_id = vs.structure_item_id
        -- 拡張項目4の値がNullでなければ承認者が設定されていて、変更履歴管理を行う工場となる。
        AND ex.sequence_no = 4
        AND coalesce(ex.extension_data, '') != ''
    )
AND vs.structure_group_id = 1000
AND vs.structure_layer_no = 1;

DROP TABLE IF EXISTS #temp_target_data; 
CREATE TABLE #temp_target_data( 
    machine_id bigint
); 
-- 画面で指定された条件で出力対象の機番IDを検索
INSERT INTO #temp_target_data
SELECT DISTINCT
    ma.machine_id 
FROM
    hm_history_management hm 
    LEFT JOIN hm_mc_machine ma 
        ON hm.history_management_id = ma.history_management_id 
    INNER JOIN hm_mc_equipment eq 
        ON ma.history_management_id = eq.history_management_id 
    LEFT JOIN hm_mc_applicable_laws ap 
        ON ma.history_management_id = ap.history_management_id 
WHERE
    1 = 1 
    AND EXISTS ( 
        SELECT
            * 
        FROM
            #temp_history_factory temp 
        WHERE
            hm.factory_id = temp.structure_id
    ) 
    /*@LocationStructureIds
    AND EXISTS ( 
        SELECT
            * 
        FROM
            #temp_condition_location temp 
        WHERE
            ma.location_structure_id = temp.structure_id
    ) 
    @LocationStructureIds*/
    /*@JobStructureIds
    AND EXISTS ( 
        SELECT
            * 
        FROM
            #temp_condition_job temp 
        WHERE
            ma.job_structure_id = temp.structure_id
    ) 
    @JobStructureIds*/
    /*@MachineNo
    AND ma.machine_no LIKE @MachineNo
    @MachineNo*/
    /*@MachineName
    AND ma.machine_name LIKE @MachineName
    @MachineName*/
    /*@EquipmentLevelStructureIdList
    AND ma.equipment_level_structure_id IN @EquipmentLevelStructureIdList
    @EquipmentLevelStructureIdList*/
    /*@ImportanceStructureIdList
    AND ma.importance_structure_id IN @ImportanceStructureIdList
    @ImportanceStructureIdList*/
    /*@ConservationStructureIdList
    AND ma.conservation_structure_id IN @ConservationStructureIdList
    @ConservationStructureIdList*/
    /*@InstallationLocation
    AND ma.installation_location LIKE @InstallationLocation
    @InstallationLocation*/
    /*@NumberOfInstallationFrom
    AND ma.number_of_installation >= @NumberOfInstallationFrom
    @NumberOfInstallationFrom*/
    /*@NumberOfInstallationTo
    AND ma.number_of_installation <= @NumberOfInstallationTo
    @NumberOfInstallationTo*/
    /*@DateOfInstallationFrom
    AND ma.date_of_installation >= @DateOfInstallationFrom
    @DateOfInstallationFrom*/
    /*@DateOfInstallationTo
    AND ma.date_of_installation <= @DateOfInstallationTo
    @DateOfInstallationTo*/
    /*@ApplicableLawsStructureIdList
    AND ap.applicable_laws_structure_id IN @ApplicableLawsStructureIdList
    @ApplicableLawsStructureIdList*/
    /*@MachineNote
    AND ma.machine_note LIKE @MachineNote
    @MachineNote*/
    /*@ManufacturerStructureIdList
    AND eq.manufacturer_structure_id IN @ManufacturerStructureIdList
    @ManufacturerStructureIdList*/
    /*@ManufacturerType
    AND eq.manufacturer_type LIKE @ManufacturerType
    @ManufacturerType*/
    /*@ModelNo
    AND eq.model_no LIKE @ModelNo
    @ModelNo*/
    /*@SerialNo
    AND eq.serial_no LIKE @SerialNo
    @SerialNo*/
    /*@DateOfManufactureFrom
    AND eq.date_of_manufacture >= @DateOfManufactureFrom
    @DateOfManufactureFrom*/
    /*@DateOfManufactureTo
    AND eq.date_of_manufacture <= @DateOfManufactureTo
    @DateOfManufactureTo*/
    /*@DeliveryDateFrom
    AND eq.delivery_date >= @DeliveryDateFrom
    @DeliveryDateFrom*/
    /*@DeliveryDateTo
    AND eq.delivery_date <= @DeliveryDateTo
    @DeliveryDateTo*/
    /*@UseSegmentStructureIdList
    AND eq.use_segment_structure_id IN @UseSegmentStructureIdList
    @UseSegmentStructureIdList*/
    /*@UseSegmentIsNull
    AND eq.use_segment_structure_id IS NULL
    @UseSegmentIsNull*/
    /*@FixedAssetNo
    AND eq.fixed_asset_no LIKE @FixedAssetNo
    @FixedAssetNo*/
    /*@EquipmentNote
    AND eq.equipment_note LIKE @EquipmentNote
    @EquipmentNote*/
;

-- 申請区分ごとの構成IDとコードを取得(10:新規、20：変更、30：削除)
DROP TABLE IF EXISTS #temp_division_info;
CREATE TABLE #temp_division_info( 
    structure_id bigint
    ,division_cd nvarchar(400) COLLATE Japanese_CI_AS_KS
); 
INSERT INTO #temp_division_info
SELECT
    st.structure_id
    , ie.extension_data AS division_cd 
FROM
    v_structure AS st 
    INNER JOIN ms_item_extension AS ie 
        ON (st.structure_item_id = ie.item_id) 
WHERE
    st.structure_group_id = 2100 
    AND ie.sequence_no = 1;

--機器台帳の変更履歴
DROP TABLE IF EXISTS #temp_history_data; 
CREATE TABLE #temp_history_data( 
    machine_id bigint
    , history_management_id bigint
    , history_order bigint
    , division_cd nvarchar(400) COLLATE Japanese_CI_AS_KS
    , status nvarchar(400) COLLATE Japanese_CI_AS_KS
    , division nvarchar(400) COLLATE Japanese_CI_AS_KS
    , district_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , factory_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , plant_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , series_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , stroke_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , facility_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , job_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , large_classfication_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , middle_classfication_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , small_classfication_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , machine_no nvarchar(400) COLLATE Japanese_CI_AS_KS
    , machine_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , equipment_level_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , installation_location nvarchar(400) COLLATE Japanese_CI_AS_KS
    , number_of_installation nvarchar(400) COLLATE Japanese_CI_AS_KS
    , date_of_installation nvarchar(400) COLLATE Japanese_CI_AS_KS
    , importance_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , conservation_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , applicable_laws_name nvarchar(MAX) COLLATE Japanese_CI_AS_KS
    , machine_note nvarchar(400) COLLATE Japanese_CI_AS_KS
    , use_segment_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , circulation_target nvarchar(400) COLLATE Japanese_CI_AS_KS
    , fixed_asset_no nvarchar(400) COLLATE Japanese_CI_AS_KS
    , manufacturer_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , manufacturer_type nvarchar(400) COLLATE Japanese_CI_AS_KS
    , model_no nvarchar(400) COLLATE Japanese_CI_AS_KS
    , serial_no nvarchar(400) COLLATE Japanese_CI_AS_KS
    , date_of_manufacture nvarchar(400) COLLATE Japanese_CI_AS_KS
    , delivery_date nvarchar(400) COLLATE Japanese_CI_AS_KS
    , maintainance_kind_manage nvarchar(400) COLLATE Japanese_CI_AS_KS
    , equipment_note nvarchar(400) COLLATE Japanese_CI_AS_KS
    , application_reason nvarchar(400) COLLATE Japanese_CI_AS_KS
    , application_user_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , application_date DATE
    , approval_user_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , approval_date DATE
    , factory_id INT
    , application_status_id bigint
    , application_division_id bigint
    , execution_division INT
); 
INSERT INTO #temp_history_data
-- 対象の機器全ての変更履歴を表示
SELECT
    hma.machine_id
    , hm.history_management_id
    , row_number() OVER ( 
        PARTITION BY
            hma.machine_id 
        ORDER BY
            hma.history_management_id
    ) AS history_order                      -- 機器ごとに何番目の変更管理かを採番、変更前後の比較に使用
    , div.division_cd                       -- 申請区分のコード
    /*@GetCount
    , hm.application_status_id AS status
    , hm.application_division_id AS division
    , hma.location_district_structure_id AS district_name
    , hma.location_factory_structure_id AS factory_name
    , hma.location_plant_structure_id AS plant_name
    , hma.location_series_structure_id AS series_name
    , hma.location_stroke_structure_id AS stroke_name
    , hma.location_facility_structure_id AS facility_name
    , hma.job_kind_structure_id AS job_name
    , hma.job_large_classfication_structure_id AS large_classfication_name
    , hma.job_middle_classfication_structure_id AS middle_classfication_name
    , hma.job_small_classfication_structure_id AS small_classfication_name
    @GetCount*/
    /*@GetData
    , ( 
        SELECT
            translation_text 
        FROM
            v_structure_item 
        WHERE
            structure_id = hm.application_status_id 
            AND language_id = @LanguageId 
            AND factory_id = 0
    ) AS status                             -- 申請状況の翻訳取得、縦持ちになったら行数が増えるためここで取得
    , ( 
        SELECT
            translation_text 
        FROM
            v_structure_item 
        WHERE
            structure_id = hm.application_division_id 
            AND language_id = @LanguageId 
            AND factory_id = 0
    ) AS division                           -- 申請区分の翻訳取得、縦持ちになったら行数が増えるためここで取得
    , ( 
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hma.location_district_structure_id 
                    AND st_f.factory_id IN (0, hma.location_factory_structure_id)
            ) 
            AND tra.structure_id = hma.location_district_structure_id
    ) AS district_name                          -- 地区
    , ( 
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hma.location_factory_structure_id 
                    AND st_f.factory_id IN (0, hma.location_factory_structure_id)
            ) 
            AND tra.structure_id = hma.location_factory_structure_id
    ) AS factory_name                           --工場
    , ( 
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hma.location_plant_structure_id 
                    AND st_f.factory_id IN (0, hma.location_factory_structure_id)
            ) 
            AND tra.structure_id = hma.location_plant_structure_id
    ) AS plant_name                             --プラント
    , ( 
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hma.location_series_structure_id 
                    AND st_f.factory_id IN (0, hma.location_factory_structure_id)
            ) 
            AND tra.structure_id = hma.location_series_structure_id
    ) AS series_name                            --系列
    , ( 
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hma.location_stroke_structure_id 
                    AND st_f.factory_id IN (0, hma.location_factory_structure_id)
            ) 
            AND tra.structure_id = hma.location_stroke_structure_id
    ) AS stroke_name                            --工程
    , ( 
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hma.location_facility_structure_id 
                    AND st_f.factory_id IN (0, hma.location_factory_structure_id)
            ) 
            AND tra.structure_id = hma.location_facility_structure_id
    ) AS facility_name                          --設備
    , ( 
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hma.job_kind_structure_id 
                    AND st_f.factory_id IN (0, hma.location_factory_structure_id)
            ) 
            AND tra.structure_id = hma.job_kind_structure_id
    ) AS job_name                               --職種
    , ( 
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hma.job_large_classfication_structure_id 
                    AND st_f.factory_id IN (0, hma.location_factory_structure_id)
            ) 
            AND tra.structure_id = hma.job_large_classfication_structure_id
    ) AS large_classfication_name               --機種大分類
    , ( 
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hma.job_middle_classfication_structure_id 
                    AND st_f.factory_id IN (0, hma.location_factory_structure_id)
            ) 
            AND tra.structure_id = hma.job_middle_classfication_structure_id
    ) AS middle_classfication_name              --機種中分類
    , ( 
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hma.job_small_classfication_structure_id 
                    AND st_f.factory_id IN (0, hma.location_factory_structure_id)
            ) 
            AND tra.structure_id = hma.job_small_classfication_structure_id
    ) AS small_classfication_name               --機種小分類
    @GetData*/
    , hma.machine_no                        --機器番号
    , hma.machine_name                      --機器名称
    /*@GetCount
    , hma.equipment_level_structure_id AS equipment_level_name
    @GetCount*/
    /*@GetData
    , ( 
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hma.equipment_level_structure_id 
                    AND st_f.factory_id IN (0, hma.location_factory_structure_id)
            ) 
            AND tra.structure_id = hma.equipment_level_structure_id
    ) AS equipment_level_name               --機器レベル(翻訳)
    @GetData*/
    , hma.installation_location             --設置場所
    , CONVERT(nvarchar, hma.number_of_installation) AS number_of_installation --設置台数
    , CASE 
        WHEN hma.date_of_installation IS NOT NULL 
            THEN FORMAT(hma.date_of_installation, 'yyyy/MM') 
        ELSE NULL                           --設置年月
        END AS date_of_installation
    /*@GetCount
    , hma.importance_structure_id AS importance_name
    , hma.conservation_structure_id AS conservation_name
    @GetCount*/
    /*@GetData
    , ( 
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hma.importance_structure_id 
                    AND st_f.factory_id IN (0, hma.location_factory_structure_id)
            ) 
            AND tra.structure_id = hma.importance_structure_id
    ) AS importance_name                    --重要度(翻訳)
    , ( 
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hma.conservation_structure_id 
                    AND st_f.factory_id IN (0, hma.location_factory_structure_id)
            ) 
            AND tra.structure_id = hma.conservation_structure_id
    ) AS conservation_name                  --保全方式(翻訳)
    @GetData*/
    , ( 
        SELECT
            trim( 
                ',' 
                FROM
                    ( 
                        SELECT
                            /*@GetCount
                            CAST(ap.applicable_laws_structure_id AS nvarchar) + ',' 
                            @GetCount*/
                            /*@GetData
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
                                            #temp_structure_factory AS st_f 
                                        WHERE
                                            st_f.structure_id = ap.applicable_laws_structure_id 
                                            AND st_f.factory_id IN (0, hma.location_factory_structure_id)
                                    ) 
                                    AND tra.structure_id = ap.applicable_laws_structure_id
                            ) + ',' 
                            @GetData*/
                        FROM
                            hm_mc_applicable_laws ap 
                        WHERE
                            ap.history_management_id = hma.history_management_id FOR XML PATH ('')
                    )
            )
    ) AS applicable_laws_name               --適用法規
    , hma.machine_note                      --機番メモ
    /*@GetCount
    , heq.use_segment_structure_id AS use_segment_name
    , CASE 
        WHEN heq.circulation_target_flg = 1 THEN '1' --対象
        WHEN heq.circulation_target_flg = 0 THEN '0' --非対象
        ELSE '' 
        END AS circulation_target           --循環対象
    @GetCount*/
    /*@GetData
    , ( 
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = heq.use_segment_structure_id 
                    AND st_f.factory_id IN (0, hma.location_factory_structure_id)
            ) 
            AND tra.structure_id = heq.use_segment_structure_id
    ) AS use_segment_name                   --使用区分(翻訳)
    , CASE 
        WHEN heq.circulation_target_flg = 1 
            THEN [dbo].[get_rep_translation_text] (hma.location_factory_structure_id, 111160071, @LanguageId) --対象
        WHEN heq.circulation_target_flg = 0 
            THEN [dbo].[get_rep_translation_text] (hma.location_factory_structure_id, 111270034, @LanguageId) --非対象
        ELSE '' 
        END AS circulation_target           --循環対象
    @GetData*/
    , heq.fixed_asset_no                    --固定資産番号
    /*@GetCount
    , heq.manufacturer_structure_id AS manufacturer_name
    @GetCount*/
    /*@GetData
    , ( 
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = heq.manufacturer_structure_id 
                    AND st_f.factory_id IN (0, hma.location_factory_structure_id)
            ) 
            AND tra.structure_id = heq.manufacturer_structure_id
    ) AS manufacturer_name                  --メーカー(翻訳)
    @GetData*/
    , heq.manufacturer_type                 --メーカー型式
    , heq.model_no                          --型式コード
    , heq.serial_no                         --製造番号
    , CASE 
        WHEN heq.date_of_manufacture IS NOT NULL 
            THEN FORMAT(heq.date_of_manufacture, 'yyyy/MM') 
        ELSE '' 
        END AS date_of_manufacture          --製造年月
    , CONVERT(nvarchar, heq.delivery_date) as delivery_date --納期
    /*@GetCount
    , CASE 
        WHEN heq.circulation_target_flg = 1 THEN '1' --対象
        WHEN heq.circulation_target_flg = 0 THEN '0' --非対象
        ELSE '' 
        END AS circulation_target           --点検種別毎管理
    @GetCount*/
    /*@GetData
    , CASE 
        WHEN heq.maintainance_kind_manage = 1 
            THEN [dbo].[get_rep_translation_text] (hma.location_factory_structure_id, 111160071, @LanguageId) --対象
        WHEN heq.maintainance_kind_manage = 0 
            THEN [dbo].[get_rep_translation_text] (hma.location_factory_structure_id, 111270034, @LanguageId) --非対象
        ELSE '' 
        END AS maintainance_kind_manage     --点検種別毎管理
    @GetData*/
    , heq.equipment_note                    --機器メモ
    , hm.application_reason
    , hm.application_user_name
    , hm.application_date
    , hm.approval_user_name
    , hm.approval_date
    , hma.location_factory_structure_id as factory_id
    , hm.application_status_id
    , hm.application_division_id
    , hma.execution_division 
FROM
    hm_mc_machine hma 
    INNER JOIN #temp_target_data td 
        ON td.machine_id = hma.machine_id -- 出力対象の機器のみ
    INNER JOIN hm_mc_equipment heq 
        ON hma.history_management_id = heq.history_management_id 
    INNER JOIN hm_history_management AS hm 
        ON hm.history_management_id = hma.history_management_id 
    INNER JOIN #temp_division_info AS div 
        ON (div.structure_id = hm.application_division_id)
;
CREATE NONCLUSTERED INDEX idx_temp_history_data_01
    ON #temp_history_data(machine_id); 
CREATE NONCLUSTERED INDEX idx_temp_history_data_02
    ON #temp_history_data(history_management_id);

--保全部位、保全項目の変更履歴
DROP TABLE IF EXISTS #temp_history_maintainance_data; 

CREATE TABLE #temp_history_maintainance_data( 
    machine_id bigint
    , history_management_id bigint
    , history_order bigint
    , division_cd nvarchar(400) COLLATE Japanese_CI_AS_KS
    , status nvarchar(400) COLLATE Japanese_CI_AS_KS
    , division nvarchar(400) COLLATE Japanese_CI_AS_KS
    , application_reason nvarchar(400) COLLATE Japanese_CI_AS_KS
    , application_user_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , application_date DATE
    , approval_user_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , approval_date DATE
    , factory_id INT
    , application_status_id bigint
    , application_division_id bigint
    , management_standards_content_id bigint
    , execution_division INT
    , inspection_site_name nvarchar(800) COLLATE Japanese_CI_AS_KS
    , inspection_site_importance_name nvarchar(800) COLLATE Japanese_CI_AS_KS
    , inspection_site_conservation_name nvarchar(800) COLLATE Japanese_CI_AS_KS
    , maintainance_division_name nvarchar(800) COLLATE Japanese_CI_AS_KS
    , inspection_content_name nvarchar(800) COLLATE Japanese_CI_AS_KS
    , maintainance_kind_name nvarchar(800) COLLATE Japanese_CI_AS_KS
    , budget_amount nvarchar(20) COLLATE Japanese_CI_AS_KS
    , schedule_type_name nvarchar(800) COLLATE Japanese_CI_AS_KS
    , preparation_period nvarchar(800) COLLATE Japanese_CI_AS_KS
    , cycle_year nvarchar(20) COLLATE Japanese_CI_AS_KS
    , cycle_month nvarchar(20) COLLATE Japanese_CI_AS_KS
    , cycle_day nvarchar(20) COLLATE Japanese_CI_AS_KS
    , disp_cycle nvarchar(20) COLLATE Japanese_CI_AS_KS
    , start_date nvarchar(10) COLLATE Japanese_CI_AS_KS
); 

WITH history_management AS ( 
    SELECT
        hm.* 
    FROM
        hm_history_management AS hm 
    WHERE
        hm.application_conduct_id = 1           --機器台帳
        AND EXISTS ( 
            SELECT
                * 
            FROM
                #temp_target_data td 
            WHERE
                td.machine_id = hm.key_id
        )
) 
, schedule AS ( 
    -- 保全スケジュール
    SELECT
        a.* 
    FROM
        hm_mc_maintainance_schedule AS a        -- 保全スケジュール
        INNER JOIN                              -- 機器別管理基準内容IDごとの開始日最新データを取得
        ( 
            SELECT
                history_management_id
                , management_standards_content_id
                , MAX(start_date) AS start_date
                , MAX(update_datetime) AS update_datetime 
            FROM
                hm_mc_maintainance_schedule 
            GROUP BY
                history_management_id, management_standards_content_id
        ) b 
            ON a.management_standards_content_id = b.management_standards_content_id 
            AND a.history_management_id = b.history_management_id
            AND ( 
                a.start_date = b.start_date 
                OR a.start_date IS NULL 
                AND b.start_date IS NULL        --null結合を考慮
            ) 
            AND ( 
                a.update_datetime = b.update_datetime 
                OR a.update_datetime IS NULL 
                AND b.update_datetime IS NULL   --null結合を考慮
            )
    WHERE
        EXISTS ( 
            SELECT
                * 
            FROM
                history_management hm 
                INNER JOIN hm_mc_management_standards_content hmcon 
                    ON hm.history_management_id = hmcon.history_management_id 
            WHERE
                a.management_standards_content_id = hmcon.management_standards_content_id
        )
) 
-- 対象の機器の全ての変更履歴を表示
INSERT 
INTO #temp_history_maintainance_data 
SELECT
    hm.key_id AS machine_id
    , hm.history_management_id
    , DENSE_RANK() OVER ( 
        PARTITION BY
            hmcon.management_standards_content_id, hm.key_id 
        ORDER BY
            hm.history_management_id
    ) AS history_order                          -- 機器ごとに何番目の変更管理かを採番、変更前後の比較に使用
    , div.division_cd                           -- 申請区分のコード
    /*@GetCount
    , hm.application_status_id AS status
    , hm.application_division_id AS division
    @GetCount*/
    /*@GetData
    , ( 
        SELECT
            translation_text 
        FROM
            v_structure_item 
        WHERE
            structure_id = hm.application_status_id 
            AND language_id = @LanguageId 
            AND factory_id = 0
    ) AS status                                 -- 申請状況の翻訳取得、縦持ちになったら行数が増えるためここで取得
    , ( 
        SELECT
            translation_text 
        FROM
            v_structure_item 
        WHERE
            structure_id = hm.application_division_id 
            AND language_id = @LanguageId 
            AND factory_id = 0
    ) AS division                               -- 申請区分の翻訳取得、縦持ちになったら行数が増えるためここで取得
    @GetData*/
    , hm.application_reason
    , hm.application_user_name
    , hm.application_date
    , hm.approval_user_name
    , hm.approval_date
    , hm.factory_id
    , hm.application_status_id
    , hm.application_division_id
    , hmcon.management_standards_content_id
    , hmcon.execution_division
    /*@GetCount
    , hmcom.inspection_site_structure_id AS inspection_site_name
    , hmcon.inspection_site_importance_structure_id AS inspection_site_importance_name
    , hmcon.inspection_site_conservation_structure_id AS inspection_site_conservation_name
    , hmcon.maintainance_division AS maintainance_division_name
    , hmcon.inspection_content_structure_id AS inspection_content_name
    , hmcon.maintainance_kind_structure_id AS maintainance_kind_name
    @GetCount*/
    /*@GetData
    , ( 
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hmcom.inspection_site_structure_id 
                    AND st_f.factory_id IN (0, machine.location_factory_structure_id)
            ) 
            AND tra.structure_id = hmcom.inspection_site_structure_id
    ) AS inspection_site_name
    , ( 
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hmcon.inspection_site_importance_structure_id 
                    AND st_f.factory_id IN (0, machine.location_factory_structure_id)
            ) 
            AND tra.structure_id = hmcon.inspection_site_importance_structure_id
    ) AS inspection_site_importance_name
    , ( 
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hmcon.inspection_site_conservation_structure_id 
                    AND st_f.factory_id IN (0, machine.location_factory_structure_id)
            ) 
            AND tra.structure_id = hmcon.inspection_site_conservation_structure_id
    ) AS inspection_site_conservation_name
    , ( 
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hmcon.maintainance_division 
                    AND st_f.factory_id IN (0, machine.location_factory_structure_id)
            ) 
            AND tra.structure_id = hmcon.maintainance_division
    ) AS maintainance_division_name
    , ( 
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hmcon.inspection_content_structure_id 
                    AND st_f.factory_id IN (0, machine.location_factory_structure_id)
            ) 
            AND tra.structure_id = hmcon.inspection_content_structure_id
    ) AS inspection_content_name
    , ( 
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hmcon.maintainance_kind_structure_id 
                    AND st_f.factory_id IN (0, machine.location_factory_structure_id)
            ) 
            AND tra.structure_id = hmcon.maintainance_kind_structure_id
    ) AS maintainance_kind_name
    @GetData*/
    , format(hmcon.budget_amount, '#,#') AS budget_amount
    /*@GetCount
    , hmcon.schedule_type_structure_id AS schedule_type_name
    @GetCount*/
    /*@GetData
    , ( 
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = hmcon.schedule_type_structure_id 
                    AND st_f.factory_id IN (0, machine.location_factory_structure_id)
            ) 
            AND tra.structure_id = hmcon.schedule_type_structure_id
    ) AS schedule_type_name
    @GetData*/
    , CAST(hmcon.preparation_period AS nvarchar) AS preparation_period
    , CAST(schedule.cycle_year AS nvarchar) AS cycle_year
    , CAST(schedule.cycle_month AS nvarchar) AS cycle_month
    , CAST(schedule.cycle_day AS nvarchar) AS cycle_day
    , schedule.disp_cycle
    , format(schedule.start_date, 'yyyy/MM/dd') AS start_date 
FROM
    history_management hm 
    LEFT JOIN hm_mc_management_standards_content hmcon 
        ON hm.history_management_id = hmcon.history_management_id 
    LEFT JOIN hm_mc_management_standards_component hmcom 
        ON hm.history_management_id = hmcom.history_management_id 
        AND hmcon.management_standards_component_id = hmcom.management_standards_component_id 
    LEFT JOIN mc_machine AS machine 
        ON hm.key_id = machine.machine_id 
    LEFT JOIN mc_equipment equipment 
        ON machine.machine_id = equipment.machine_id 
    LEFT JOIN schedule 
        ON schedule.management_standards_content_id = hmcon.management_standards_content_id 
        AND schedule.history_management_id = hmcon.history_management_id
    LEFT JOIN #temp_division_info AS div 
        ON (div.structure_id = hm.application_division_id) 
WHERE
    hmcom.is_management_standard_conponent = 1; 
CREATE NONCLUSTERED INDEX idx_temp_history_maintainance_data_01
    ON #temp_history_maintainance_data(machine_id); 
CREATE NONCLUSTERED INDEX idx_temp_history_maintainance_data_02
    ON #temp_history_maintainance_data(history_management_id); 

DROP TABLE IF EXISTS #report_col_info; 

CREATE TABLE #report_col_info(col_name nvarchar(MAX), col_order INT); 

DROP TABLE IF EXISTS #report_maintainance_col_info; 

CREATE TABLE #report_maintainance_col_info(col_name nvarchar(MAX), col_order INT); 

DROP TABLE IF EXISTS #report_col_info_temp; 

CREATE TABLE #report_col_info_temp( 
    structure_id INT
    , col_name nvarchar(MAX)
    , ex_data nvarchar(MAX)
    , sequence_no INT
); 

-- 帳票出力項目の構成マスタの定義を取得
-- 連番1と2の拡張項目の値を使用するので両方取得して、次のサブクエリで使用する作業用
INSERT 
INTO #report_col_info_temp 
SELECT
    st.structure_id
    , st.translation_text AS col_name
    , ie.extension_data AS ex_data
    , ie.sequence_no 
FROM
    v_structure_item AS st 
    INNER JOIN ms_item_extension AS ie 
        ON (st.structure_item_id = ie.item_id) 
WHERE
    st.structure_group_id = 2150 
    AND st.language_id = @LanguageId 
    AND st.factory_id = 0
    AND st.location_structure_id = 0;

-- 帳票出力対象の列の定義を取得(機器台帳or長期計画)
-- 作業用の帳票出力項目定義を取得するために連番1で機能を絞り、連番2の表示順を取得
INSERT 
INTO #report_col_info 
SELECT DISTINCT
    rci.col_name
    , CONVERT(INT, rci.ex_data) AS col_order 
FROM
    #report_col_info_temp AS rci 
WHERE
    EXISTS ( 
        SELECT
            * 
        FROM
            #report_col_info_temp AS temp 
        WHERE
            temp.structure_id = rci.structure_id 
            AND temp.sequence_no = 1 
            AND temp.ex_data = '1' --機器台帳
    ) 
    AND rci.sequence_no = 2;
INSERT 
INTO #report_maintainance_col_info 
SELECT
    rci.col_name
    , CONVERT(INT, rci.ex_data) AS col_order 
FROM
    #report_col_info_temp AS rci 
WHERE
    EXISTS ( 
        SELECT
            * 
        FROM
            #report_col_info_temp AS temp 
        WHERE
            temp.structure_id = rci.structure_id 
            AND temp.sequence_no = 1 
            AND temp.ex_data = '3' --保全部位、保全項目
    ) 
    AND rci.sequence_no = 2; 
/*@GetData
DROP TABLE IF EXISTS #temp_output_data; 
CREATE TABLE #temp_output_data( 
    machine_id bigint
    , history_management_id bigint
    , job_name nvarchar(400)
    , machine_no nvarchar(400)
    , machine_name nvarchar(400)
    , inspection_site_name nvarchar(400)
    , inspection_content_name nvarchar(400)
    , col_name nvarchar(400)
    , col_value_before nvarchar(MAX)
    , col_value_after nvarchar(MAX)
    , application_reason nvarchar(400)
    , application_user_name nvarchar(400)
    , application_date DATE
    , approval_user_name nvarchar(400)
    , approval_date DATE
    , status nvarchar(400)
    , division nvarchar(400)
    , col_no INT
    , management_standards_content_id bigint
); 
@GetData*/
WITH history_data_vertical AS (
    -- 項目ごとの縦持ちに変更
    @HistoryVertical
)
, history_maintainance_data_vertical AS ( 
    -- 項目ごとの縦持ちに変更
    @HistoryMaintainanceVertical 
)
, t_machine AS ( 
    -- 機器と工場IDを取得
    SELECT
        ma.*
        , ma.location_factory_structure_id AS factory_id --工場ID
    FROM
        mc_machine ma
)
-- history_orderの前後で紐づけて、変更前後を横持ちにした帳票を出力
/*@GetCount
, get_count AS (
@GetCount*/
/*@GetData
    INSERT INTO #temp_output_data
@GetData*/
    SELECT
        /*@GetCount
        COUNT(*) AS cnt
        @GetCount*/
        /*@GetData
        a.machine_id
        , a.history_management_id
        , CASE 
            WHEN ma.job_structure_id IS NOT NULL 
                THEN ( 
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
                            #temp_structure_factory AS st_f 
                        WHERE
                            st_f.structure_id = ma.job_kind_structure_id 
                            AND st_f.factory_id IN (0, ma.factory_id)
                    ) 
                    AND tra.structure_id = ma.job_kind_structure_id
            ) 
            ELSE hd.job_name 
            END AS job_name                 --職種
        , COALESCE(ma.machine_no, hd.machine_no) AS machine_no --機器番号
        , COALESCE(ma.machine_name, hd.machine_name) AS machine_name --機器名称
        , NULL AS inspection_site_name
        , NULL AS inspection_content_name
        , a.col_name                        --変更項目
        , CASE a.division_cd 
            WHEN '10' THEN '' 
            ELSE b.col_value 
            END AS col_value_before         -- 変更前の値、新規の場合はブランク
        , CASE a.division_cd 
            WHEN '30' THEN '' 
            ELSE a.col_value 
            END AS col_value_after          -- 変更後の値、削除の場合はブランク
        , a.application_reason
        , a.application_user_name
        , a.application_date
        , a.approval_user_name
        , a.approval_date
        , a.status
        , a.division 
        , a.col_no
        , 0 AS management_standards_content_id 
        @GetData*/
    FROM
        history_data_vertical AS a          -- 変更後(aはAFTERのA)
        LEFT OUTER JOIN history_data_vertical AS b -- 変更前(bはBEFOREのB) 、新規申請の場合は紐づかないので外部結合
            ON ( 
                a.machine_id = b.machine_id 
                AND a.history_order - 1 = b.history_order 
                AND a.col_no = b.col_no
            ) 
        LEFT JOIN t_machine ma 
            ON a.machine_id = ma.machine_id 
        LEFT JOIN ( 
            SELECT
                max(hd.history_management_id) AS history_management_id
                , hd.machine_id 
            FROM
                #temp_history_data hd 
            GROUP BY
                hd.machine_id
        ) hma 
            ON a.machine_id = hma.machine_id 
        LEFT JOIN #temp_history_data hd 
            ON hd.history_management_id = hma.history_management_id 
    WHERE
        ( 
            COALESCE(a.col_value, '') <> COALESCE(b.col_value, '') -- 変更前後で値が異なる
            OR a.division_cd IN ('10', '30') -- または新規or削除申請のもの
        ) 
        /*@ApplicationStatusIdList
        AND a.application_status_id IN @ApplicationStatusIdList
        @ApplicationStatusIdList*/
        /*@ApplicationDivisionIdList
        AND a.application_division_id IN @ApplicationDivisionIdList
        @ApplicationDivisionIdList*/
        /*@ApplicationUserName
        AND a.application_user_name LIKE @ApplicationUserName
        @ApplicationUserName*/
        /*@ApplicationDateFrom
        AND a.application_date >= @ApplicationDateFrom
        @ApplicationDateFrom*/
        /*@ApplicationDateTo
        AND a.application_date <= @ApplicationDateTo
        @ApplicationDateTo*/
        /*@ApprovalUserName
        AND a.approval_user_name LIKE @ApprovalUserName
        @ApprovalUserName*/
        /*@ApprovalDateFrom
        AND a.approval_date >= @ApprovalDateFrom
        @ApprovalDateFrom*/
        /*@ApprovalDateTo
        AND a.approval_date <= @ApprovalDateTo
        @ApprovalDateTo*/
        /*@ApplicationReason
        AND a.application_reason LIKE @ApplicationReason
        @ApplicationReason*/

        UNION ALL 

    SELECT
        /*@GetCount
        COUNT(*) AS cnt
        @GetCount*/
        /*@GetData
        a.machine_id
        , a.history_management_id
        , CASE 
            WHEN ma.job_structure_id IS NOT NULL 
                THEN ( 
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
                            #temp_structure_factory AS st_f 
                        WHERE
                            st_f.structure_id = ma.job_kind_structure_id 
                            AND st_f.factory_id IN (0, ma.factory_id)
                    ) 
                    AND tra.structure_id = ma.job_kind_structure_id
            ) 
            ELSE ( 
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
                            #temp_structure_factory AS st_f 
                        WHERE
                            st_f.structure_id = hmma.job_kind_structure_id 
                            AND st_f.factory_id IN (0, hmma.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = hmma.job_kind_structure_id
            ) 
            END AS job_name                 --職種
        , COALESCE(ma.machine_no, hmma.machine_no) AS machine_no --機器番号
        , COALESCE(ma.machine_name, hmma.machine_name) AS machine_name --機器名称
        , ( 
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
                        #temp_structure_factory AS st_f 
                    WHERE
                        st_f.structure_id = COALESCE( 
                            mscom.inspection_site_structure_id
                            , hmscom.inspection_site_structure_id
                        ) 
                        AND st_f.factory_id IN ( 
                            0
                            , COALESCE( 
                                ma.factory_id
                                , hmma.location_factory_structure_id
                            )
                        )
                ) 
                AND tra.structure_id = COALESCE( 
                    mscom.inspection_site_structure_id
                    , hmscom.inspection_site_structure_id
                )
        ) AS inspection_site_name           --保全部位(翻訳)
        , ( 
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
                        #temp_structure_factory AS st_f 
                    WHERE
                        st_f.structure_id = COALESCE( 
                            mscon.inspection_content_structure_id
                            , hmscon.inspection_content_structure_id
                        ) 
                        AND st_f.factory_id IN ( 
                            0
                            , COALESCE( 
                                ma.factory_id
                                , hmma.location_factory_structure_id
                            )
                        )
                ) 
                AND tra.structure_id = COALESCE( 
                    mscon.inspection_content_structure_id
                    , hmscon.inspection_content_structure_id
                )
        ) AS inspection_content_name        --保全項目(翻訳)
        , a.col_name                        --変更項目
        , CASE 
            WHEN a.division_cd = '10' 
            OR a.execution_division = 4 
                THEN '' 
            ELSE b.col_value 
            END AS col_value_before         -- 変更前の値、新規の場合はブランク
        , CASE 
            WHEN a.division_cd = '30' 
            OR a.execution_division = 6 
                THEN '' 
            ELSE a.col_value 
            END AS col_value_after          -- 変更後の値、削除の場合はブランク
        , a.application_reason
        , a.application_user_name
        , a.application_date
        , a.approval_user_name
        , a.approval_date
        , a.status
        , a.division
        , a.col_no
        , a.management_standards_content_id 
        @GetData*/
    FROM
        history_maintainance_data_vertical AS a          -- 変更後(aはAFTERのA)
        LEFT OUTER JOIN history_maintainance_data_vertical AS b -- 変更前(bはBEFOREのB) 、新規申請の場合は紐づかないので外部結合
            ON ( 
                a.management_standards_content_id = b.management_standards_content_id 
                AND a.machine_id = b.machine_id
                AND a.history_order - 1 = b.history_order 
                AND a.col_no = b.col_no
            ) 
        LEFT JOIN t_machine ma 
            ON a.machine_id = ma.machine_id 
        LEFT JOIN mc_management_standards_content mscon 
            ON a.management_standards_content_id = mscon.management_standards_content_id 
        LEFT JOIN mc_management_standards_component mscom 
            ON mscon.management_standards_component_id = mscom.management_standards_component_id 
        LEFT JOIN ( 
            SELECT
                max(hmma.history_management_id) AS history_management_id
                , hmma.machine_id 
            FROM
                hm_mc_machine hmma 
            WHERE
                EXISTS ( 
                    SELECT
                        * 
                    FROM
                        #temp_target_data temp 
                    WHERE
                        hmma.machine_id = temp.machine_id
                ) 
            GROUP BY
                machine_id
        ) history  --機器単位で最新の変更管理
            ON a.machine_id = history.machine_id 
        LEFT JOIN hm_mc_machine hmma 
            ON history.history_management_id = hmma.history_management_id 
        LEFT JOIN hm_mc_management_standards_content hmscon 
            ON history.history_management_id = hmscon.history_management_id 
            AND a.management_standards_content_id = hmscon.management_standards_content_id 
        LEFT JOIN hm_mc_management_standards_component hmscom 
            ON history.history_management_id = hmscom.history_management_id 
            AND hmscon.management_standards_component_id = hmscom.management_standards_component_id 
    WHERE
        ( 
            COALESCE(a.col_value, '') <> COALESCE(b.col_value, '') -- 変更前後で値が異なる
            OR a.division_cd IN ('10', '30') -- または新規or削除申請のもの
            OR a.execution_division IN (4, 6) --保全項目の追加または削除
        ) 
        /*@ApplicationStatusIdList
        AND a.application_status_id IN @ApplicationStatusIdList
        @ApplicationStatusIdList*/
        /*@ApplicationDivisionIdList
        AND a.application_division_id IN @ApplicationDivisionIdList
        @ApplicationDivisionIdList*/
        /*@ApplicationUserName
        AND a.application_user_name LIKE @ApplicationUserName
        @ApplicationUserName*/
        /*@ApplicationDateFrom
        AND a.application_date >= @ApplicationDateFrom
        @ApplicationDateFrom*/
        /*@ApplicationDateTo
        AND a.application_date <= @ApplicationDateTo
        @ApplicationDateTo*/
        /*@ApprovalUserName
        AND a.approval_user_name LIKE @ApprovalUserName
        @ApprovalUserName*/
        /*@ApprovalDateFrom
        AND a.approval_date >= @ApprovalDateFrom
        @ApprovalDateFrom*/
        /*@ApprovalDateTo
        AND a.approval_date <= @ApprovalDateTo
        @ApprovalDateTo*/
        /*@ApplicationReason
        AND a.application_reason LIKE @ApplicationReason
        @ApplicationReason*/

/*@GetCount
)
SELECT
    SUM(cnt)
FROM
    get_count
@GetCount*/
/*@GetData
;
SELECT
    *
FROM
    ( 
        SELECT
            DENSE_RANK() OVER ( 
                ORDER BY
                    IIF(application_date IS NOT NULL, 0, 1)
                    , application_date
                    , machine_id
                    , history_management_id
            ) AS row_num
            , * 
        FROM
            #temp_output_data
    ) tbl 
ORDER BY
    tbl.row_num
    , tbl.management_standards_content_id
    , tbl.col_no
@GetData*/
