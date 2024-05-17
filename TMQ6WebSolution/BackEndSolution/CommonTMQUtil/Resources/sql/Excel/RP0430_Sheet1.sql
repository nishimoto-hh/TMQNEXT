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
    long_plan_id bigint
); 
-- 画面で指定された条件で出力対象の長計件名IDを検索
INSERT INTO #temp_target_data
SELECT DISTINCT
    hlp.long_plan_id 
FROM
    hm_history_management hm 
    LEFT JOIN hm_ln_long_plan hlp 
        ON hm.history_management_id = hlp.history_management_id 
    LEFT JOIN hm_mc_management_standards_content hmcon 
        ON hlp.long_plan_id = hmcon.long_plan_id 
    LEFT JOIN hm_mc_management_standards_component hmcom 
        ON hmcon.management_standards_component_id = hmcom.management_standards_component_id 
    LEFT JOIN hm_mc_machine hmma 
        ON hmcom.machine_id = hmma.machine_id 
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
    /*@Subject
    AND hlp.subject LIKE @Subject
    @Subject*/
    /*@LocationStructureIds
    AND EXISTS ( 
        SELECT
            * 
        FROM
            #temp_condition_location temp 
        WHERE
            hlp.location_structure_id = temp.structure_id
    ) 
    @LocationStructureIds*/
    /*@JobStructureIds
    AND EXISTS ( 
        SELECT
            * 
        FROM
            #temp_condition_job temp 
        WHERE
            hlp.job_structure_id = temp.structure_id
    ) 
    @JobStructureIds*/
    /*@SubjectNote
    AND hlp.subject_note LIKE @SubjectNote
    @SubjectNote*/
    /*@PersonName
    AND hlp.person_name LIKE @PersonName
    @PersonName*/
    /*@WorkItemStructureIdList
    AND hlp.work_item_structure_id IN @WorkItemStructureIdList
    @WorkItemStructureIdList*/
    /*@BudgetManagementStructureIdList
    AND hlp.budget_management_structure_id IN @BudgetManagementStructureIdList
    @BudgetManagementStructureIdList*/
    /*@BudgetPersonalityStructureIdList
    AND hlp.budget_personality_structure_id IN @BudgetPersonalityStructureIdList
    @BudgetPersonalityStructureIdList*/
    /*@MaintenanceSeasonStructureIdList
    AND hlp.maintenance_season_structure_id IN @MaintenanceSeasonStructureIdList
    @MaintenanceSeasonStructureIdList*/
    /*@PurposeStructureIdList
    AND hlp.purpose_structure_id IN @PurposeStructureIdList
    @PurposeStructureIdList*/
    /*@WorkClassStructureIdList
    AND hlp.work_class_structure_id IN @WorkClassStructureIdList
    @WorkClassStructureIdList*/
    /*@TreatmentStructureIdList
    AND hlp.treatment_structure_id IN @TreatmentStructureIdList
    @TreatmentStructureIdList*/
    /*@FacilityStructureIdList
    AND hlp.facility_structure_id IN @FacilityStructureIdList
    @FacilityStructureIdList*/
    /*@MachineNo
    AND hmma.machine_no LIKE @MachineNo
    @MachineNo*/
    /*@MachineName
    AND hmma.machine_name LIKE @MachineName
    @MachineName*/
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

--長計件名の変更履歴
DROP TABLE IF EXISTS #temp_history_data; 
CREATE TABLE #temp_history_data( 
    long_plan_id bigint
    , history_management_id bigint
    , history_order bigint
    , division_cd nvarchar(400) COLLATE Japanese_CI_AS_KS
    , status nvarchar(400) COLLATE Japanese_CI_AS_KS
    , division nvarchar(400) COLLATE Japanese_CI_AS_KS
    , subject nvarchar(400) COLLATE Japanese_CI_AS_KS
    , subject_note nvarchar(400) COLLATE Japanese_CI_AS_KS
    , person_name nvarchar(400) COLLATE Japanese_CI_AS_KS
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
    , work_item_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , budget_management_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , budget_personality_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , maintenance_season_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , purpose_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , work_class_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , treatment_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , facility_division_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , long_plan_division_name nvarchar(400) COLLATE Japanese_CI_AS_KS
    , long_plan_group_name nvarchar(400) COLLATE Japanese_CI_AS_KS
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

-- 対象の長期計画件名の全ての変更履歴を表示
INSERT INTO #temp_history_data
SELECT
    hlp.long_plan_id
    , hm.history_management_id
    , row_number() OVER ( 
        PARTITION BY
            hlp.long_plan_id 
        ORDER BY
            ISNULL(hm.application_date,GETDATE()), hlp.history_management_id
    ) AS history_order                      -- 件名ごとに何番目の変更管理かを採番、変更前後の比較に使用
    , div.division_cd                       -- 申請区分のコード
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
    @GetData*/
    , hlp.subject
    , hlp.subject_note
    , hlp.person_name
    /*@GetCount
    , hlp.location_district_structure_id AS district_name
    , hlp.location_factory_structure_id AS factory_name
    , hlp.location_plant_structure_id AS plant_name
    , hlp.location_series_structure_id AS series_name
    , hlp.location_stroke_structure_id AS stroke_name
    , hlp.location_facility_structure_id AS facility_name
    , hlp.job_kind_structure_id AS job_name
    , hlp.job_large_classfication_structure_id AS large_classfication_name
    , hlp.job_middle_classfication_structure_id AS middle_classfication_name
    , hlp.job_small_classfication_structure_id AS small_classfication_name
    , hlp.work_item_structure_id AS work_item_name
    , hlp.budget_management_structure_id AS budget_management_name
    , hlp.budget_personality_structure_id AS budget_personality_name
    , hlp.maintenance_season_structure_id AS maintenance_season_name
    , hlp.purpose_structure_id AS purpose_name
    , hlp.work_class_structure_id AS work_class_name
    , hlp.treatment_structure_id AS treatment_name
    , hlp.facility_structure_id AS facility_division_name
    , hlp.long_plan_division_structure_id AS long_plan_division_name
    , hlp.long_plan_group_structure_id AS long_plan_group_name
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
                    st_f.structure_id = hlp.location_district_structure_id 
                    AND st_f.factory_id IN (0, hlp.location_factory_structure_id)
            ) 
            AND tra.structure_id = hlp.location_district_structure_id
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
                    st_f.structure_id = hlp.location_factory_structure_id 
                    AND st_f.factory_id IN (0, hlp.location_factory_structure_id)
            ) 
            AND tra.structure_id = hlp.location_factory_structure_id
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
                    st_f.structure_id = hlp.location_plant_structure_id 
                    AND st_f.factory_id IN (0, hlp.location_factory_structure_id)
            ) 
            AND tra.structure_id = hlp.location_plant_structure_id
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
                    st_f.structure_id = hlp.location_series_structure_id 
                    AND st_f.factory_id IN (0, hlp.location_factory_structure_id)
            ) 
            AND tra.structure_id = hlp.location_series_structure_id
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
                    st_f.structure_id = hlp.location_stroke_structure_id 
                    AND st_f.factory_id IN (0, hlp.location_factory_structure_id)
            ) 
            AND tra.structure_id = hlp.location_stroke_structure_id
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
                    st_f.structure_id = hlp.location_facility_structure_id 
                    AND st_f.factory_id IN (0, hlp.location_factory_structure_id)
            ) 
            AND tra.structure_id = hlp.location_facility_structure_id
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
                    st_f.structure_id = hlp.job_kind_structure_id 
                    AND st_f.factory_id IN (0, hlp.location_factory_structure_id)
            ) 
            AND tra.structure_id = hlp.job_kind_structure_id
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
                    st_f.structure_id = hlp.job_large_classfication_structure_id 
                    AND st_f.factory_id IN (0, hlp.location_factory_structure_id)
            ) 
            AND tra.structure_id = hlp.job_large_classfication_structure_id
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
                    st_f.structure_id = hlp.job_middle_classfication_structure_id 
                    AND st_f.factory_id IN (0, hlp.location_factory_structure_id)
            ) 
            AND tra.structure_id = hlp.job_middle_classfication_structure_id
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
                    st_f.structure_id = hlp.job_small_classfication_structure_id 
                    AND st_f.factory_id IN (0, hlp.location_factory_structure_id)
            ) 
            AND tra.structure_id = hlp.job_small_classfication_structure_id
    ) AS small_classfication_name               --機種小分類
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
                    st_f.structure_id = hlp.work_item_structure_id 
                    AND st_f.factory_id IN (0, hlp.location_factory_structure_id)
            ) 
            AND tra.structure_id = hlp.work_item_structure_id
    ) AS work_item_name                     --作業項目(翻訳)
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
                    st_f.structure_id = hlp.budget_management_structure_id 
                    AND st_f.factory_id IN (0, hlp.location_factory_structure_id)
            ) 
            AND tra.structure_id = hlp.budget_management_structure_id
    ) AS budget_management_name             --予算管理区分(翻訳)
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
                    st_f.structure_id = hlp.budget_personality_structure_id 
                    AND st_f.factory_id IN (0, hlp.location_factory_structure_id)
            ) 
            AND tra.structure_id = hlp.budget_personality_structure_id
    ) AS budget_personality_name            --予算性格区分(翻訳)
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
                    st_f.structure_id = hlp.maintenance_season_structure_id 
                    AND st_f.factory_id IN (0, hlp.location_factory_structure_id)
            ) 
            AND tra.structure_id = hlp.maintenance_season_structure_id
    ) AS maintenance_season_name            --保全時期(翻訳)
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
                    st_f.structure_id = hlp.purpose_structure_id 
                    AND st_f.factory_id IN (0, hlp.location_factory_structure_id)
            ) 
            AND tra.structure_id = hlp.purpose_structure_id
    ) AS purpose_name                       --目的区分(翻訳)
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
                    st_f.structure_id = hlp.work_class_structure_id 
                    AND st_f.factory_id IN (0, hlp.location_factory_structure_id)
            ) 
            AND tra.structure_id = hlp.work_class_structure_id
    ) AS work_class_name                    --作業区分(翻訳)
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
                    st_f.structure_id = hlp.treatment_structure_id 
                    AND st_f.factory_id IN (0, hlp.location_factory_structure_id)
            ) 
            AND tra.structure_id = hlp.treatment_structure_id
    ) AS treatment_name                     --処置区分(翻訳)
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
                    st_f.structure_id = hlp.facility_structure_id 
                    AND st_f.factory_id IN (0, hlp.location_factory_structure_id)
            ) 
            AND tra.structure_id = hlp.facility_structure_id
    ) AS facility_division_name             --設備区分(翻訳)
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
                    st_f.structure_id = hlp.long_plan_division_structure_id 
                    AND st_f.factory_id IN (0, hlp.location_factory_structure_id)
            ) 
            AND tra.structure_id = hlp.long_plan_division_structure_id
    ) AS long_plan_division_name            --長計区分(翻訳)
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
                    st_f.structure_id = hlp.long_plan_group_structure_id 
                    AND st_f.factory_id IN (0, hlp.location_factory_structure_id)
            ) 
            AND tra.structure_id = hlp.long_plan_group_structure_id
    ) AS long_plan_group_name               --長計グループ(翻訳)
    @GetData*/
    , hm.application_reason
    , hm.application_user_name
    , hm.application_date
    , hm.approval_user_name
    , hm.approval_date 
    , hlp.location_factory_structure_id AS factory_id
    , hm.application_status_id
    , hm.application_division_id
    , hlp.execution_division
FROM
    hm_ln_long_plan hlp
    INNER JOIN #temp_target_data td 
        ON td.long_plan_id = hlp.long_plan_id -- 出力対象の件名のみ
    INNER JOIN hm_history_management AS hm 
        ON ( 
            hm.history_management_id = hlp.history_management_id
        ) 
    INNER JOIN #temp_division_info AS div 
        ON (div.structure_id = hm.application_division_id) 
;
CREATE NONCLUSTERED INDEX idx_temp_history_data_01
    ON #temp_history_data(long_plan_id); 
CREATE NONCLUSTERED INDEX idx_temp_history_data_02
    ON #temp_history_data(history_management_id);

--保全部位、保全項目の変更履歴
DROP TABLE IF EXISTS #temp_history_maintainance_data; 
CREATE TABLE #temp_history_maintainance_data( 
    long_plan_id bigint
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
    , machine_no nvarchar(800) COLLATE Japanese_CI_AS_KS
    , machine_name nvarchar(800) COLLATE Japanese_CI_AS_KS
    , importance_name nvarchar(800) COLLATE Japanese_CI_AS_KS
    , inspection_site_name nvarchar(800) COLLATE Japanese_CI_AS_KS
    , inspection_content_name nvarchar(800) COLLATE Japanese_CI_AS_KS
    , budget_amount nvarchar(20) COLLATE Japanese_CI_AS_KS
    , maintainance_kind_name nvarchar(800) COLLATE Japanese_CI_AS_KS
    , schedule_type_name nvarchar(800) COLLATE Japanese_CI_AS_KS
    , start_date nvarchar(10) COLLATE Japanese_CI_AS_KS
    , cycle_year nvarchar(20) COLLATE Japanese_CI_AS_KS
    , cycle_month nvarchar(20) COLLATE Japanese_CI_AS_KS
    , cycle_day nvarchar(20) COLLATE Japanese_CI_AS_KS
    , disp_cycle nvarchar(20) COLLATE Japanese_CI_AS_KS
); 

WITH history_management AS ( 
    SELECT
        hm.*
    FROM
        hm_history_management AS hm 
    WHERE
        hm.application_conduct_id = 2           --長期計画
        AND EXISTS ( 
            SELECT
                * 
            FROM
                #temp_target_data td 
            WHERE
                td.long_plan_id = hm.key_id
        ) -- 出力対象の件名のみ
) 
, schedule AS ( 
    -- 保全スケジュール
    SELECT
        schedule.maintainance_schedule_id
        , schedule.management_standards_content_id AS management_standards_content_id_schedule
        , schedule.start_date
        , schedule.cycle_year
        , schedule.cycle_month
        , schedule.cycle_day
        , schedule.disp_cycle 
    FROM
        ( 
            -- 機器別管理基準内容IDごとに最大の開始日時をもつものを取得
            SELECT
                main.maintainance_schedule_id
                , main.management_standards_content_id
                , main.start_date
                , main.cycle_year
                , main.cycle_month
                , main.cycle_day
                , main.disp_cycle 
            FROM
                mc_maintainance_schedule AS main 
            WHERE
                NOT EXISTS ( 
                    SELECT
                        * 
                    FROM
                        mc_maintainance_schedule AS sub 
                    WHERE
                        main.management_standards_content_id = sub.management_standards_content_id 
                        AND main.start_date < sub.start_date
                )
                AND EXISTS ( 
                    SELECT
                        * 
                    FROM
                        history_management hm 
                        INNER JOIN hm_mc_management_standards_content hmcon 
                            ON hm.history_management_id = hmcon.history_management_id 
                    WHERE
                        main.management_standards_content_id = hmcon.management_standards_content_id
                ) -- 出力対象の件名に紐づくデータのみ
        ) AS schedule
) 
-- 対象の長期計画件名の全ての変更履歴を表示
INSERT 
INTO #temp_history_maintainance_data 
SELECT
    hm.key_id AS long_plan_id
    , hm.history_management_id
    , DENSE_RANK() OVER ( 
        PARTITION BY
            hmcon.management_standards_content_id, hm.key_id 
        ORDER BY
            ISNULL(hm.application_date,GETDATE()), hm.history_management_id
    ) AS history_order -- 件名ごとに何番目の変更管理かを採番、変更前後の比較に使用
    , div.division_cd -- 申請区分のコード
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
    ) AS status -- 申請状況の翻訳取得、縦持ちになったら行数が増えるためここで取得
    , ( 
        SELECT
            translation_text 
        FROM
            v_structure_item 
        WHERE
            structure_id = hm.application_division_id 
            AND language_id = @LanguageId 
            AND factory_id = 0
    ) AS division -- 申請区分の翻訳取得、縦持ちになったら行数が増えるためここで取得
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
    , machine.machine_no
    , machine.machine_name
    /*@GetCount
    , machine.importance_structure_id AS importance_name
    , com.inspection_site_structure_id AS inspection_site_name
    , hmcon.inspection_content_structure_id AS inspection_content_name
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
                    st_f.structure_id = machine.importance_structure_id 
                    AND st_f.factory_id IN (0, machine.location_factory_structure_id)
            ) 
            AND tra.structure_id = machine.importance_structure_id
    ) AS importance_name 
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
                    st_f.structure_id = com.inspection_site_structure_id 
                    AND st_f.factory_id IN (0, machine.location_factory_structure_id)
            ) 
            AND tra.structure_id = com.inspection_site_structure_id
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
                    st_f.structure_id = hmcon.inspection_content_structure_id 
                    AND st_f.factory_id IN (0, machine.location_factory_structure_id)
            ) 
            AND tra.structure_id = hmcon.inspection_content_structure_id
    ) AS inspection_content_name
    @GetData*/
    , FORMAT(hmcon.budget_amount, '#,#') AS budget_amount
    /*@GetCount
    , hmcon.maintainance_kind_structure_id AS maintainance_kind_name
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
                    st_f.structure_id = hmcon.maintainance_kind_structure_id 
                    AND st_f.factory_id IN (0, machine.location_factory_structure_id)
            ) 
            AND tra.structure_id = hmcon.maintainance_kind_structure_id
    ) AS maintainance_kind_name
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
    , FORMAT(schedule.start_date, 'yyyy/MM/dd') AS start_date
    , CAST(schedule.cycle_year AS nvarchar) AS cycle_year
    , CAST(schedule.cycle_month AS nvarchar) AS cycle_month
    , CAST(schedule.cycle_day AS nvarchar) AS cycle_day
    , schedule.disp_cycle 
FROM
    history_management hm 
    INNER JOIN hm_mc_management_standards_content hmcon 
        ON hm.history_management_id = hmcon.history_management_id 
    LEFT JOIN mc_management_standards_component com 
        ON hmcon.management_standards_component_id = com.management_standards_component_id 
    LEFT JOIN mc_machine AS machine 
        ON com.machine_id = machine.machine_id 
    LEFT JOIN schedule 
        ON schedule.management_standards_content_id_schedule = hmcon.management_standards_content_id 
    LEFT JOIN #temp_division_info AS div 
        ON (div.structure_id = hm.application_division_id) 
;
CREATE NONCLUSTERED INDEX idx_temp_history_maintainance_data_01
    ON #temp_history_maintainance_data(long_plan_id); 
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
            AND temp.ex_data = '2' --長計件名
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
            AND temp.ex_data = '4' --保全部位、保全項目
    ) 
    AND rci.sequence_no = 2; 
    
/*@GetData
DROP TABLE IF EXISTS #temp_output_data; 
CREATE TABLE #temp_output_data( 
    long_plan_id bigint
    , history_management_id bigint
    , job_name nvarchar(400)
    , subject nvarchar(400)
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
, t_long_plan AS (
    -- 長計件名と工場IDを取得
    SELECT
        lp.*
        , lp.location_factory_structure_id AS factory_id --工場ID
    FROM
        ln_long_plan lp
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
        a.long_plan_id
        , a.history_management_id
        , CASE 
            WHEN lp.job_structure_id IS NOT NULL 
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
                            st_f.structure_id = lp.job_kind_structure_id 
                            AND st_f.factory_id IN (0, lp.factory_id)
                    ) 
                    AND tra.structure_id = lp.job_kind_structure_id
            )
            ELSE hd.job_name 
            END AS job_name                         --職種
        , COALESCE(lp.subject, hd.subject) AS subject -- 長計件名
        , NULL AS machine_no
        , NULL AS machine_name
        , NULL AS inspection_site_name
        , NULL AS inspection_content_name
        , a.col_name                                --変更項目
        , CASE a.division_cd 
            WHEN '10' THEN '' 
            ELSE b.col_value 
            END AS col_value_before                 -- 変更前の値、新規の場合はブランク
        , CASE a.division_cd 
            WHEN '30' THEN '' 
            ELSE a.col_value 
            END AS col_value_after                  -- 変更後の値、削除の場合はブランク
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
        history_data_vertical AS a                  -- 変更後(aはAFTERのA)
        LEFT OUTER JOIN history_data_vertical AS b  -- 変更前(bはBEFOREのB) 、新規申請の場合は紐づかないので外部結合
            ON ( 
                a.long_plan_id = b.long_plan_id 
                AND a.history_order - 1 = b.history_order 
                AND a.col_no = b.col_no
            ) 
        LEFT JOIN t_long_plan lp 
            ON a.long_plan_id = lp.long_plan_id 
        LEFT JOIN ( 
            SELECT
                max(hd.history_management_id) AS history_management_id
                , hd.long_plan_id 
            FROM
                #temp_history_data hd 
            GROUP BY
                hd.long_plan_id
        ) hmlp 
            ON a.long_plan_id = hmlp.long_plan_id 
        LEFT JOIN #temp_history_data hd 
            ON hd.history_management_id = hmlp.history_management_id 
    WHERE
        (
            COALESCE(a.col_value, '') <> COALESCE(b.col_value, '') -- 変更前後で値が異なる
            OR a.division_cd IN ('10', '30')            -- または新規or削除申請のもの
        )
        AND a.division_cd <> '0' -- 帳票出力対象外のデータを除去
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
        a.long_plan_id
        , a.history_management_id
        , CASE 
            WHEN lp.job_structure_id IS NOT NULL 
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
                            st_f.structure_id = lp.job_kind_structure_id 
                            AND st_f.factory_id IN (0, lp.factory_id)
                    ) 
                    AND tra.structure_id = lp.job_kind_structure_id
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
                            st_f.structure_id = hmlp.job_kind_structure_id 
                            AND st_f.factory_id IN (0, hmlp.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = hmlp.job_kind_structure_id
            ) 
            END AS job_name                          --職種
        , COALESCE(lp.subject, hmlp.subject) AS subject -- 長計件名
        , COALESCE(mc.machine_no, hmc.machine_no) AS machine_no --機器番号
        , COALESCE(mc.machine_name, hmc.machine_name) AS machine_name --機器名称
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
                        st_f.structure_id = COALESCE(mscom.inspection_site_structure_id, hmscom.inspection_site_structure_id) 
                        AND st_f.factory_id IN (0, COALESCE(lp.factory_id, hmlp.location_factory_structure_id))
                ) 
                AND tra.structure_id = COALESCE(mscom.inspection_site_structure_id, hmscom.inspection_site_structure_id)
        ) AS inspection_site_name                   --保全部位(翻訳)
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
                        st_f.structure_id = COALESCE(mscon.inspection_content_structure_id, hcon.inspection_content_structure_id) 
                        AND st_f.factory_id IN (0, COALESCE(lp.factory_id, hmlp.location_factory_structure_id))
                ) 
                AND tra.structure_id = COALESCE(mscon.inspection_content_structure_id, hcon.inspection_content_structure_id)
        ) AS inspection_content_name                --保全項目(翻訳)
        , a.col_name                                --変更項目
        , CASE 
            WHEN a.division_cd = '10' OR a.execution_division = 4 THEN '' 
            ELSE b.col_value 
            END AS col_value_before                 -- 変更前の値、新規の場合はブランク
        , CASE
            WHEN a.division_cd = '30' OR a.execution_division = 5 THEN '' 
            ELSE a.col_value 
            END AS col_value_after                  -- 変更後の値、削除の場合はブランク
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
        history_maintainance_data_vertical AS a                  -- 変更後(aはAFTERのA)
        LEFT OUTER JOIN history_maintainance_data_vertical AS b  -- 変更前(bはBEFOREのB) 、新規申請の場合は紐づかないので外部結合
            ON ( 
                a.management_standards_content_id = b.management_standards_content_id 
                AND a.long_plan_id = b.long_plan_id
                AND a.history_order - 1 = b.history_order 
                AND a.col_no = b.col_no
            ) 
        LEFT JOIN t_long_plan lp 
            ON a.long_plan_id = lp.long_plan_id 
        LEFT JOIN mc_management_standards_content mscon 
            ON a.management_standards_content_id = mscon.management_standards_content_id 
        LEFT JOIN mc_management_standards_component mscom 
            ON mscon.management_standards_component_id = mscom.management_standards_component_id 
        LEFT JOIN mc_machine mc 
            ON mscom.machine_id = mc.machine_id 
        LEFT JOIN ( 
            SELECT
                max(hmlp.history_management_id) AS history_management_id
                , hmlp.long_plan_id 
            FROM
                hm_ln_long_plan hmlp 
            WHERE
                EXISTS ( 
                    SELECT
                        * 
                    FROM
                        #temp_target_data temp 
                    WHERE
                        hmlp.long_plan_id = temp.long_plan_id
                ) 
            GROUP BY
                long_plan_id
        ) history --件名単位で最新の変更管理
            ON a.long_plan_id = history.long_plan_id 
        LEFT JOIN hm_ln_long_plan hmlp 
            ON history.history_management_id = hmlp.history_management_id 
        LEFT JOIN hm_mc_management_standards_content hcon 
            ON history.history_management_id = hcon.history_management_id 
            AND a.management_standards_content_id = hcon.management_standards_content_id 
        LEFT JOIN mc_management_standards_component hmscom 
            ON hcon.management_standards_component_id = hmscom.management_standards_component_id 
        LEFT JOIN mc_machine hmc 
            ON hmscom.machine_id = hmc.machine_id 
    WHERE
        (
            COALESCE(a.col_value, '') <> COALESCE(b.col_value, '') -- 変更前後で値が異なる
            OR a.division_cd IN ('10', '30')            -- または新規or削除申請のもの
            OR a.execution_division IN (4, 5) -- 保全項目の追加または削除
        )
        AND a.division_cd <> '0' -- 帳票出力対象外のデータを除去
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
                    , long_plan_id
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
