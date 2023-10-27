WITH st_com( 
    structure_layer_no
    , structure_id
    , parent_structure_id
    , org_structure_id
) AS ( 
    SELECT
        st.structure_layer_no
        , st.structure_id
        , st.parent_structure_id
        , st.structure_id 
    FROM
        ms_structure AS st 
    WHERE
        st.structure_group_id = 1000
) 
, tmp_top1 (languageId, factoryId
) AS (
    SELECT TOP 1 languageId, factoryId FROM #temp
)
, rec_up( 
    structure_layer_no
    , structure_id
    , parent_structure_id
    , org_structure_id
) AS ( 
    SELECT
        st.structure_layer_no
        , st.structure_id
        , st.parent_structure_id
        , st.structure_id 
    FROM
        st_com AS st 
    UNION ALL 
    SELECT
        b.structure_layer_no
        , b.structure_id
        , b.parent_structure_id
        , a.org_structure_id 
    FROM
        rec_up a 
        INNER JOIN ms_structure b 
            ON (b.structure_id = a.parent_structure_id)
) 
, get_factory AS ( 
    --機能場所階層IDから工場の故障分析個別工場フラグを取得
    SELECT TOP 1
        vs.structure_id
        , vs.translation_text
        , vs.structure_group_id
        , vs.structure_layer_no
        , up.org_structure_id
        , mie.extension_data 
    FROM
        rec_up AS up 
        LEFT OUTER JOIN v_structure_item_all AS vs 
            ON (up.structure_id = vs.structure_id) 
        LEFT OUTER JOIN ms_item_extension mie 
            ON vs.structure_item_id = mie.item_id 
        ,tmp_top1
    WHERE
            vs.language_id = tmp_top1.languageId COLLATE Japanese_CI_AS
        AND vs.location_structure_id = tmp_top1.factoryId
        AND vs.structure_layer_no = 1           --工場
        AND mie.sequence_no = 2                 --故障分析個別工場フラグ
        AND mie.extension_data IS NOT NULL
    ORDER BY vs.location_structure_id DESC
) 
, summary_list AS ( 
    SELECT
        pl.occurrence_date                      --発生日
        , su.completion_date                    --完了日
        , su.subject                            --件名
        , su.mq_class_structure_id              --MQ分類
        , coalesce(maf.machine_no, ma.machine_no) AS machine_no --機器番号
        , coalesce(maf.machine_name, ma.machine_name) AS machine_name --機器名称
        , su.location_structure_id              --地区～設備
        , coalesce(maf.job_structure_id, ma.job_structure_id) AS job_structure_id --職種～機種小分類
        , su.stop_system_structure_id           --系停止
--        , su.stop_time                          --系停止時間(Hr)
        , FORMAT(su.stop_time, '0.##') AS stop_time --系停止時間(Hr)
        , hi.cost_note                          --費用メモ
        , su.sudden_division_structure_id       --突発区分
        , pl.expected_construction_date         --着工予定日
        , case 
            when get_factory.extension_data = '1' 
                then dbo.get_file_link(1690, hf.history_failure_id) 
            else dbo.get_file_link(1670, hf.history_failure_id) 
            end AS file_link_failure            --故障原因分析書
        , su.budget_management_structure_id     --予算管理区分
        , su.budget_personality_structure_id    --予算性格区分
        , pl.total_budget_cost                  --予算金額(k円)
        , hi.maintenance_season_structure_id    --保全時期
        , coalesce( 
            mu_request.display_name
            , re.request_personnel_name
        ) AS request_personnel_name             --依頼担当
        , coalesce(mu.display_name, hi.construction_personnel_name) AS construction_personnel_name --施工担当者
        , hi.total_working_time                 --作業時間(Hr)
        , hi.working_time_self                  --自係(Hr)
        , re.discovery_methods_structure_id     --発見方法
        , hi.actual_result_structure_id         --実績結果
        , hi.construction_company               --施工会社
        , su.maintenance_count                  --カウント件数
        , su.plan_implementation_content        --作業計画・実施内容
        , su.subject_note                       --件名メモ
        , dbo.get_file_link(1650, su.summary_id) AS file_link_subject --件名添付有無
        , coalesce(hf.maintenance_site, [dbo].[get_v_structure_item](his.inspection_site_structure_id, tmp_top1.factoryId, tmp_top1.languageId)) AS inspection_site_name --保全部位
        , coalesce( 
            hf.maintenance_content
            , [dbo].[get_v_structure_item](hic.inspection_content_structure_id, tmp_top1.factoryId, tmp_top1.languageId)
        ) AS inspection_content_name            --保全内容
        , hi.expenditure                        --実績金額(k円)
        , hf.phenomenon_structure_id            --現象
        , hf.phenomenon_note                    --現象補足
        , hf.failure_cause_structure_id         --原因
        , hf.failure_cause_note                 --原因補足
        , hf.failure_cause_personality_structure_id --原因性格1、原因性格2
        , hf.failure_cause_personality_note     --性格補足
        , hf.treatment_measure_structure_id     --処置対策
        , hf.failure_cause_addition_note        --故障原因
        , hf.failure_status                     --故障状況
        , hf.previous_situation                 --故障前の保全実施状況
        , hf.recovery_action                    --復旧措置
        , hf.improvement_measure                --改善対策
        , hf.system_feed_back                   --保全システムのフィードバック
        , hf.lesson                             --教訓
        , hf.failure_note                       --特記（メモ）
        , coalesce(hf.follow_flg, hic.follow_flg) AS follow_flg --フォロー有無
        , coalesce(hf.follow_plan_date, hic.follow_plan_date) AS follow_plan_date --フォロー予定年月
        , coalesce(hf.follow_content, hic.follow_content) AS follow_content --フォロー内容
        , re.request_no                         --依頼No.
        , CASE 
            WHEN su.completion_date IS NOT NULL 
                THEN 1 
            WHEN hi.construction_personnel_id IS NOT NULL 
                THEN 2 
            ELSE 3 
            END AS progress_no                  --進捗状況
        , su.summary_id                         --保全活動件名ID(非表示)
    FROM
        ma_summary su 
        LEFT JOIN ma_request re 
            ON su.summary_id = re.summary_id 
        LEFT JOIN ma_plan pl 
            ON su.summary_id = pl.summary_id 
        LEFT JOIN ma_history hi 
            ON su.summary_id = hi.summary_id 
        LEFT JOIN ma_history_machine hm 
            ON hi.history_id = hm.history_id 
        LEFT JOIN ma_history_inspection_site his 
            ON hm.history_machine_id = his.history_machine_id 
        LEFT JOIN ma_history_inspection_content hic 
            ON his.history_inspection_site_id = hic.history_inspection_site_id 
        LEFT JOIN mc_machine ma 
            ON hm.machine_id = ma.machine_id 
        LEFT JOIN ma_history_failure hf 
            ON hi.history_id = hf.history_id 
        LEFT JOIN mc_machine maf 
            ON hf.machine_id = maf.machine_id 
        LEFT JOIN ms_user mu 
            ON hi.construction_personnel_id = mu.user_id 
        LEFT JOIN ms_user mu_request 
            ON re.request_personnel_id = mu_request.user_id 
        LEFT JOIN get_factory 
            ON su.location_structure_id = get_factory.org_structure_id
        ,tmp_top1
) 

SELECT * FROM (
SELECT DISTINCT
    summary.occurrence_date                     --発生日
    , summary.completion_date                   --完了日
    , summary.subject                           --件名
    , [dbo].[get_v_structure_item](summary.mq_class_structure_id, temp.factoryId, temp.languageId) AS mq_class_name                      --MQ分類
    , summary.machine_no                        --機器番号
    , summary.machine_name                      --機器名称
    , summary.location_structure_id             --地区～設備
    , summary.job_structure_id                  --職種～機種小分類
    , [dbo].[get_v_structure_item](summary.stop_system_structure_id, temp.factoryId, temp.languageId) AS stop_system_name                --系停止
    , summary.stop_time                         --系停止時間(Hr)
    , summary.cost_note                         --費用メモ
    , [dbo].[get_v_structure_item](summary.sudden_division_structure_id, temp.factoryId, temp.languageId) AS sudden_division_name        --突発区分
    , summary.expected_construction_date        --着工予定日
    , summary.file_link_failure                 --故障原因分析書
    , [dbo].[get_v_structure_item](summary.budget_management_structure_id, temp.factoryId, temp.languageId) AS budget_management_name    --予算管理区分
    , [dbo].[get_v_structure_item](summary.budget_personality_structure_id, temp.factoryId, temp.languageId) AS budget_personality_name  --予算性格区分
    , summary.total_budget_cost                 --予算金額(k円)
    , [dbo].[get_v_structure_item](summary.maintenance_season_structure_id, temp.factoryId, temp.languageId) AS maintenance_season_name  --保全時期
    , summary.request_personnel_name            --依頼担当
    , summary.construction_personnel_name       --施工担当者
    , summary.total_working_time                --作業時間(Hr)
    , summary.working_time_self                 --自係(Hr)
    , [dbo].[get_v_structure_item](summary.discovery_methods_structure_id, temp.factoryId, temp.languageId) AS discovery_methods_name    --発見方法
    , [dbo].[get_v_structure_item](summary.actual_result_structure_id, temp.factoryId, temp.languageId) AS actual_result_name            --実績結果
    , summary.construction_company              --施工会社
    , summary.maintenance_count                 --カウント件数
    , summary.plan_implementation_content       --作業計画・実施内容
    , summary.subject_note                      --件名メモ
    , summary.file_link_subject                 --件名添付有無
    , summary.inspection_site_name              --保全部位
    , summary.inspection_content_name           --保全内容
    , summary.expenditure                       --実績金額(k円)
    , [dbo].[get_v_structure_item](summary.phenomenon_structure_id, temp.factoryId, temp.languageId) AS phenomenon_name                  --現象
    , summary.phenomenon_note                   --現象補足
    , [dbo].[get_v_structure_item](summary.failure_cause_structure_id, temp.factoryId, temp.languageId) AS failure_cause_name            --原因
    , summary.failure_cause_note                --原因補足
    , [dbo].[get_failure_cause_personality](summary.failure_cause_personality_structure_id, 1, temp.factoryId, temp.languageId) AS failure_cause_personality_name1    --原因性格1
    , [dbo].[get_failure_cause_personality](summary.failure_cause_personality_structure_id, 2, temp.factoryId, temp.languageId) AS failure_cause_personality_name2    --原因性格2
    , summary.failure_cause_personality_note    --性格補足
    , [dbo].[get_v_structure_item](summary.treatment_measure_structure_id, temp.factoryId, temp.languageId) AS treatment_measure_name    --処置対策
    , summary.failure_cause_addition_note       --故障原因
    , summary.failure_status                    --故障状況
    , summary.previous_situation                --故障前の保全実施状況
    , summary.recovery_action                   --復旧措置
    , summary.improvement_measure               --改善対策
    , summary.system_feed_back                  --保全システムのフィードバック
    , summary.lesson                            --教訓
    , summary.failure_note                      --特記（メモ）
    , CASE ISNULL(summary.follow_flg, 0)
          WHEN 1 THEN '○'
          ELSE ''
      END AS follow_flg                         --フォロー有無
    , FORMAT(summary.follow_plan_date, 'yyyy/MM') AS follow_plan_date  --フォロー予定年月
    , summary.follow_content                    --フォロー内容
    , summary.request_no                        --依頼No.
    , CASE progress_no
          WHEN 1 THEN '完了済'
          WHEN 2 THEN '保全受付'
          ELSE '作成済'
      END AS progress_name                      --進捗状況
    , summary.summary_id                        --保全活動件名ID(非表示)
--    , ex.structure_id AS progress_id 
FROM
    summary_list summary 
    INNER JOIN #temp temp
    ON summary.summary_id = temp.Key1
) tbl
ORDER BY
occurrence_date desc
,summary_id desc
