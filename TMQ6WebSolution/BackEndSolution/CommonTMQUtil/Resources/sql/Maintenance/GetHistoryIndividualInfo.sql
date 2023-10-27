SELECT
    hi.manufacturing_personnel_id                         --製造担当者ID
    , coalesce(us_man.display_name, hi.manufacturing_personnel_name) AS manufacturing_personnel_name --製造担当者
    , hi.construction_personnel_id              --施工担当者ID
    , coalesce( 
        us_con.display_name
        , hi.construction_personnel_name
    ) AS construction_personnel_name            --施工担当者
    , hi.expenditure                            --実績費用(任意)
    , hi.work_failure_division_structure_id     --作業／故障区分
    , pl.occurrence_date                        --発生日
    , hi.occurrence_time                        --発生時刻
    , su.completion_date                        --完了日
    , su.completion_time                        --完了時刻
    , hi.call_count                             --呼出回数
    , hi.stop_count                             --係停止回数
    , hi.discovery_personnel                    --発見者
    , hi.effect_production_structure_id         --生産への影響
    , hi.effect_quality_structure_id            --品質への影響
    , hi.failure_site                           -- 故障部位
    , hi.parts_existence_flg                    --予備品(有無)
    , hi.total_working_time                     --修理時間(Hr)
    , hi.working_time_research                  --調査(Hr)
    , hi.working_time_procure                   --調達(Hr)
    , hi.working_time_repair                    --修復(Hr)
    , hi.working_time_test                      --試運転(Hr)
    , hi.working_time_company                   --施工業者(Hr)
    , hi.history_id                             --履歴ID
    , hi.update_serialid                        --更新シリアルID
FROM
    ma_summary su 
    LEFT JOIN ma_plan pl 
        ON su.summary_id = pl.summary_id 
    LEFT JOIN ma_history hi 
        ON su.summary_id = hi.summary_id 
    LEFT JOIN ms_user us_man 
        ON hi.manufacturing_personnel_id = us_man.user_id 
    LEFT JOIN ms_user us_con 
        ON hi.construction_personnel_id = us_con.user_id 
WHERE
    su.summary_id = @SummaryId
