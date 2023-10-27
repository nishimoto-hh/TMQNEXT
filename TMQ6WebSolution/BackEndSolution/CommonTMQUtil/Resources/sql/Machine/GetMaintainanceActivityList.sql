SELECT row_number() over(order by ISNULL(completion_date,'9999/12/31') desc) as row_no,
       *
FROM
(SELECT sm.summary_id, -- 件名ID
	   sm.subject,    -- 件名
	   sm.completion_date,  -- 完了日
	   hs.expenditure, --実績金額 
	   (SELECT TOP(1)translation_text FROM v_structure_item_all WHERE structure_id = his.inspection_site_structure_id  AND language_id = @LanguageId AND location_structure_id IN @FactoryIdList order by location_structure_id desc)as maintenance_site , -- 作業部位
	   (SELECT TOP(1)translation_text FROM v_structure_item_all WHERE structure_id =hic.inspection_content_structure_id  AND language_id = @LanguageId AND location_structure_id IN @FactoryIdList order by location_structure_id desc)as maintenance_content , -- 作業項目
	   hs.total_working_time, -- 作業時間
	   null as failure_cause_structure_id, -- 故障原因
       null as failure_cause_personality_structure_id, -- 原因性格 
	   null as treatment_measure_structure_id,         -- 処置対策
	   sm.plan_implementation_content                  -- 作業内容結果
FROM ma_summary sm,
     ma_history hs,
	 ma_history_machine hsm,
	 ma_history_inspection_site his,
	 ma_history_inspection_content hic
WHERE sm.summary_id = hs.summary_id
and   hs.history_id = hsm.history_id
and   hsm.history_machine_id = his.history_machine_id
and   his.history_inspection_site_id = hic.history_inspection_site_id
and   sm.activity_division = 1 -- 点検
and   hsm.machine_id = @MachineId

UNION ALL

SELECT sm.summary_id, -- 件名ID
       sm.subject,    -- 件名
	   sm.completion_date, -- 完了日
	   hs.expenditure, --実績金額
	   hf.maintenance_site , -- 作業部位
	   hf.maintenance_content, -- 作業項目
	   hs.total_working_time,  -- 作業時間
	   hf.failure_cause_structure_id, -- 故障原因
       hf.failure_cause_personality_structure_id,  -- 原因性格
	   hf.treatment_measure_structure_id,  -- 処置対策
	   sm.plan_implementation_content -- 作業内容結果
FROM ma_summary sm,
     ma_history hs,
	 ma_history_failure hf
WHERE sm.summary_id = hs.summary_id
and   hs.history_id = hf.history_id
and   sm.activity_division = 2 -- 故障
and   hf.machine_id = @MachineId
)tbl

order by  ISNULL(completion_date,'9999/12/31') desc
