SELECT
    re.request_no
    ,su.subject
    ,su.plan_implementation_content
    ,su.subject_note
    ,su.location_structure_id
    ,su.job_structure_id
    ,su.mq_class_structure_id
    ,su.repair_cost_class_structure_id
    ,su.budget_management_structure_id
    ,su.budget_personality_structure_id
    ,su.sudden_division_structure_id
    ,su.stop_system_structure_id
    ,su.stop_time
    ,su.maintenance_count
    ,su.change_management_structure_id
    ,su.env_safety_management_structure_id
    ,su.update_serialid
    ,su.summary_id
    ,su.activity_division
    ,su.completion_date
    ,(
        SELECT
            CASE
                WHEN COUNT(summary_id) > 0 THEN 1
                ELSE 0
            END
        FROM
            ma_summary
        WHERE
            follow_plan_key_id = @SummaryId
    ) AS follow_plan_flg
    ,su.follow_plan_key_id
    ,su.long_plan_id
FROM
    ma_summary su
    LEFT JOIN ma_request re
    ON  su.summary_id = re.summary_id
WHERE
    su.summary_id = @SummaryId
