SELECT
    /*@OccurrenceDate
    --発生日
    summary.occurrence_date,
    @OccurrenceDate*/
    /*@CompletionDate
    --完了日
    summary.completion_date,
    @CompletionDate*/
    /*@Subject
    --件名
    summary.subject,
    @Subject*/
    /*@MqClassName
    --MQ分類
    summary.mq_class_structure_id,
    @MqClassName*/
    ----地区～設備
    --,summary.location_structure_id
    ----地区ID
    --,summary.district_id
    ----工場ID
    --,summary.factory_id
    ----プラントID
    --,summary.plant_id
    ----系列ID
    --,summary.series_id
    ----工程ID
    --,summary.stroke_id
    ----設備ID
    --,summary.facility_id
    ----職種
    --,summary.job_structure_id
    /*@StopSystemName
    --系停止
    summary.stop_system_structure_id,
    @StopSystemName*/
    ----系停止時間(Hr)
    --summary.stop_time,
    /*@StopTimeDisp
    --系停止時間(Hr)(表示用)
    summary.stop_time_disp,
    @StopTimeDisp*/
    /*@CostNote
    --費用メモ
    summary.cost_note,
    @CostNote*/
    ----突発区分
    --,summary.sudden_division_structure_id
    /*@ExpectedConstructionDate
    --着工予定日
    summary.expected_construction_date,
    @ExpectedConstructionDate*/
    /*@FileLinkFailure
    --故障原因分析書
    summary.file_link_failure,
    @FileLinkFailure*/
    ----予算管理区分
    --,summary.budget_management_structure_id
    ----予算性格区分
    --,summary.budget_personality_structure_id
    ----予算金額(k円)
    --,summary.total_budget_cost
    /*@TotalBudgetCostDisp
    --予算金額(k円)(表示用)
    summary.total_budget_cost_disp,
    @TotalBudgetCostDisp*/
    ----保全時期
    --,summary.maintenance_season_structure_id
    /*@RequestPersonnelName
    --依頼担当
    summary.request_personnel_name,
    @RequestPersonnelName*/
    /*@ConstructionPersonnelName
    --施工担当者
    summary.construction_personnel_name,
    @ConstructionPersonnelName*/
    ----作業時間(Hr)
    --,summary.total_working_time
    /*@TotalWorkingTimeDisp
    --作業時間(Hr)(表示用)
    summary.total_working_time_disp,
    @TotalWorkingTimeDisp*/
    ----自係(Hr)
    --,summary.working_time_self
    /*@WorkingTimeSelfDisp
    --自係(Hr)(表示用)
    summary.working_time_self_disp,
    @WorkingTimeSelfDisp*/
    ----発見方法
    --,summary.discovery_methods_structure_id
    ----実績結果
    --,summary.actual_result_structure_id
    /*@ConstructionCompany
    --施工会社
    summary.construction_company,
    @ConstructionCompany*/
    /*@MaintenanceCount
    --カウント件数
    summary.maintenance_count,
    @MaintenanceCount*/
    /*@PlanImplementationContent
    --作業計画・実施内容
    summary.plan_implementation_content,
    @PlanImplementationContent*/
    /*@SubjectNote
    --件名メモ
    summary.subject_note,
    @SubjectNote*/
    /*@FileLinkSubject
    --件名添付有無
    summary.file_link_subject,
    @FileLinkSubject*/
    ----実績金額(k円)
    --,summary.expenditure
    /*@ExpenditureDisp
    --実績金額(k円)(表示用)
    summary.expenditure_disp,
    @ExpenditureDisp*/
    ----現象
    --,summary.phenomenon_structure_id
    /*@PhenomenonNote
    --現象補足
    summary.phenomenon_note,
    @PhenomenonNote*/
    ----原因
    --,summary.failure_cause_structure_id
    /*@FailureCauseNote
    --原因補足
    summary.failure_cause_note,
    @FailureCauseNote*/
    ----原因性格1、原因性格2
    --,summary.failure_cause_personality_structure_id
    ----原因性格1
    --,summary.failure_cause_personality1_structure_id
    ----原因性格2
    --,summary.failure_cause_personality2_structure_id
    /*@FailureCausePersonalityNote
    --性格補足
    summary.failure_cause_personality_note,
    @FailureCausePersonalityNote*/
    ----処置対策
    --,summary.treatment_measure_structure_id
    /*@FailureCauseAdditionNote
    --故障原因
    summary.failure_cause_addition_note,
    @FailureCauseAdditionNote*/
    /*@FailureStatus
    --故障状況
    summary.failure_status,
    @FailureStatus*/
    /*@PreviousSituation
    --故障前の保全実施状況
    summary.previous_situation,
    @PreviousSituation*/
    /*@RecoveryAction
    --復旧措置
    summary.recovery_action,
    @RecoveryAction*/
    /*@ImprovementMeasure
    --改善対策
    summary.improvement_measure,
    @ImprovementMeasure*/
    /*@SystemFeedBack
    --保全システムのフィードバック
    summary.system_feed_back,
    @SystemFeedBack*/
    /*@Lesson
    --教訓
    summary.lesson,
    @Lesson*/
    /*@FailureNote
    --特記（メモ）
    summary.failure_note,
    @FailureNote*/
    /*@FollowFlg
    --フォロー有無
    summary.follow_flg,
    @FollowFlg*/
    /*@FollowContent
    --フォロー内容
    summary.follow_content,
    @FollowContent*/
    /*@RequestNo
    --依頼No.
    summary.request_no,
    @RequestNo*/
    /*@MaintenanceSiteName
    --保全部位(翻訳)
    summary.maintenance_site_name,
    @MaintenanceSiteName*/
    /*@MaintenanceContentName
    --保全内容(翻訳)
    summary.maintenance_content_name,
    @MaintenanceContentName*/
    ----フォロー予定年月
    --,summary.follow_plan_date
    /*@FollowPlanDateDisp
    --フォロー予定年月
    FORMAT(summary.follow_plan_date, 'yyyy/MM') AS follow_plan_date_disp,
    @FollowPlanDateDisp*/
    ----進捗状況
    --,ex.structure_id AS progress_id
    /*@MqClassName
    --MQ分類(翻訳)
    (
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                -- 工場IDまたは0で合致する最大の工場IDを取得→工場IDのレコードがなければ0となる
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = summary.mq_class_structure_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.mq_class_structure_id
    ) AS mq_class_name,
    @MqClassName*/
    /*@StopSystemName
    --系停止(翻訳)
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
                    st_f.structure_id = summary.stop_system_structure_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.stop_system_structure_id
    ) AS stop_system_name,
    @StopSystemName*/
    /*@SuddenDivisionName
    --突発区分(翻訳)
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
                    st_f.structure_id = summary.sudden_division_structure_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.sudden_division_structure_id
    ) AS sudden_division_name,
    @SuddenDivisionName*/
    /*@BudgetManagementName
    --予算管理区分(翻訳)
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
                    st_f.structure_id = summary.budget_management_structure_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.budget_management_structure_id
    ) AS budget_management_name,
    @BudgetManagementName*/
    /*@BudgetPersonalityName
    --予算性格区分(翻訳)
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
                    st_f.structure_id = summary.budget_personality_structure_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.budget_personality_structure_id
    ) AS budget_personality_name,
    @BudgetPersonalityName*/
    /*@MaintenanceSeasonName
    --保全時期(翻訳)
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
                    st_f.structure_id = summary.maintenance_season_structure_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.maintenance_season_structure_id
    ) AS maintenance_season_name,
    @MaintenanceSeasonName*/
    /*@DiscoveryMethodsName
    --発見方法(翻訳)
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
                    st_f.structure_id = summary.discovery_methods_structure_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.discovery_methods_structure_id
    ) AS discovery_methods_name,
    @DiscoveryMethodsName*/
    /*@ActualResultName
    --実績結果(翻訳)
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
                    st_f.structure_id = summary.actual_result_structure_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.actual_result_structure_id
    ) AS actual_result_name,
    @ActualResultName*/
    /*@PhenomenonName
    --現象(翻訳)
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
                    st_f.structure_id = summary.phenomenon_structure_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.phenomenon_structure_id
    ) AS phenomenon_name,
    @PhenomenonName*/
    /*@FailureCauseName
    --原因(翻訳)
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
                    st_f.structure_id = summary.failure_cause_structure_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.failure_cause_structure_id
    ) AS failure_cause_name,
    @FailureCauseName*/
    /*@TreatmentMeasureName
    --処置対策(翻訳)
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
                    st_f.structure_id = summary.treatment_measure_structure_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.treatment_measure_structure_id
    ) AS treatment_measure_name,
    @TreatmentMeasureName*/
    /*@ProgressName
    --進捗状況(翻訳)
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
                    st_f.structure_id = summary.progress_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.progress_id
    ) AS progress_name,
    @ProgressName*/
    /*@DistrictName
    --地区(翻訳)
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
                    st_f.structure_id = summary.district_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.district_id
    ) AS district_name,
    @DistrictName*/
    /*@FactoryName
    --工場(翻訳)
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
                    st_f.structure_id = summary.factory_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.factory_id
    ) AS factory_name,
    @FactoryName*/
    /*@PlantName
    --プラント(翻訳)
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
                    st_f.structure_id = summary.plant_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.plant_id
    ) AS plant_name,
    @PlantName*/
    /*@SeriesName
    --系列(翻訳)
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
                    st_f.structure_id = summary.series_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.series_id
    ) AS series_name,
    @SeriesName*/
    /*@StrokeName
    --工程(翻訳)
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
                    st_f.structure_id = summary.stroke_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.stroke_id
    ) AS stroke_name,
    @StrokeName*/
    /*@FacilityName
    --設備(翻訳)
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
                    st_f.structure_id = summary.facility_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.facility_id
    ) AS facility_name,
    @FacilityName*/
    /*@JobName
    --職種(翻訳)
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
                    st_f.structure_id = summary.job_structure_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.job_structure_id
    ) AS job_name,
    @JobName*/
    /*@FailureCausePersonality1StructureName
    --原因性格1(翻訳)
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
                    st_f.structure_id = summary.failure_cause_personality1_structure_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.failure_cause_personality1_structure_id
    ) AS failure_cause_personality1_structure_name,
    @FailureCausePersonality1StructureName*/
    /*@FailureCausePersonality2StructureName
    --原因性格2(翻訳)
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
                    st_f.structure_id = summary.failure_cause_personality2_structure_id
                AND st_f.factory_id IN(0, summary.factory_id)
            )
        AND tra.structure_id = summary.failure_cause_personality2_structure_id
    ) AS failure_cause_personality2_structure_name,
    @FailureCausePersonality2StructureName*/
    summary.issue_date, --20230911 shiraishi add
    --,summary.call_count
    --保全活動件名ID(非表示)
    summary.summary_id
FROM
    join_summary AS summary
    --LEFT JOIN
    --    #temp_progress AS ex
    --ON  summary.progress_no = ex.extension_data
