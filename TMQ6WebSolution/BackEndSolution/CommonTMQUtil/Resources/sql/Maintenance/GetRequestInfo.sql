SELECT
    re.request_content                          --依頼内容
    , re.issue_date                             --発行日
    , re.urgency_structure_id                   --緊急度
    , re.discovery_methods_structure_id         --発見方法
    , re.desired_start_date                     --着手希望日
    , re.desired_end_date                       --完了希望日
    , re.request_department_clerk_id            --依頼部課係
    , re.request_personnel_id                   --依頼担当ID
    , coalesce( 
        us_person.display_name
        , re.request_personnel_name
    ) AS request_personnel_name                 --依頼担当
    , re.request_personnel_tel                  --担当TEL
    , re.request_department_chief_id            --依頼係長ID
    , coalesce( 
        us_chief.display_name
        , re.request_department_chief_name
    ) AS request_department_chief_name          --依頼係長
    , re.request_department_manager_id          --依頼課長ID
    , coalesce( 
        us_manager.display_name
        , re.request_department_manager_name
    ) AS request_department_manager_name        --依頼課長
    , re.request_department_foreman_id          --依頼職長ID
    , coalesce( 
        us_foreman.display_name
        , re.request_department_foreman_name
    ) AS request_department_foreman_name        --依頼職長
    , re.maintenance_department_clerk_id        --保全部課係
    , re.request_reason                         --事由
    , re.examination_result                     --件名検討結果
    , re.construction_division_structure_id     --工事区分
    , re.request_id                             --依頼ID
    , re.update_serialid                        --更新シリアルID
FROM
    ma_request re 
    LEFT JOIN ms_user us_person 
        ON re.request_department_clerk_id = us_person.user_id 
    LEFT JOIN ms_user us_chief 
        ON re.request_department_chief_id = us_chief.user_id 
    LEFT JOIN ms_user us_manager 
        ON re.request_department_manager_id = us_manager.user_id 
    LEFT JOIN ms_user us_foreman
        ON re.request_department_foreman_id = us_foreman.user_id 
WHERE
    re.summary_id = @SummaryId
