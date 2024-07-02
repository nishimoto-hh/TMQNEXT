declare @key2 as nvarchar(2) ='ja' ;
declare @key3 as int =4080;

        SELECT

	hi.construction_personnel_name,	 --	1.担当者
	
	su.subject,						 --2.件名
	
	FORMAT(mp.occurrence_date,'yyyy/MM/dd') as occurrence_date,	--3.作業/故障発生日

	'' as buid ,--	4.部位大分類
	
	FORMAT(su.completion_date,'yyyy/MM/dd') as completion_date,--5.完了年月日
	
	'' as buic,--	6.部位中分類

	su.plan_implementation_content,--7.概要/故障状況
    
	(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = temp.languageId
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = hi.work_failure_division_structure_id
                AND st_f.factory_id IN(0, su.location_factory_structure_id)
            )
        AND tra.structure_id = hi.work_failure_division_structure_id
    ) AS work_failure_division_name,--8.作業/故障区分



	'' as buis,		--	9.部位小分類
	'' as zairyou,	--	10.材料費(機器)
	mp.total_budget_cost,--11.予算費用(任意)
	hi.expenditure,--12.実績費用(任意)

            (
                SELECT
                    tra.translation_text
                FROM
                    v_structure_item_all AS tra
                WHERE
                    tra.language_id = temp.languageId
                AND tra.location_structure_id = (
                        SELECT
                            MAX(st_f.factory_id)
                        FROM
                            #temp_structure_factory AS st_f
                        WHERE
                            st_f.structure_id = su.location_plant_structure_id
                        AND st_f.factory_id IN(0, su.location_factory_structure_id)
                    )
                AND tra.structure_id = su.location_plant_structure_id
            ) AS location_plant_name,--系列

            (
                SELECT
                    tra.translation_text
                FROM
                    v_structure_item_all AS tra
                WHERE
                    tra.language_id = temp.languageId
                AND tra.location_structure_id = (
                        SELECT
                            MAX(st_f.factory_id)
                        FROM
                            #temp_structure_factory AS st_f
                        WHERE
                            st_f.structure_id = su.location_series_structure_id
                        AND st_f.factory_id IN(0, su.location_factory_structure_id)
                    )
                AND tra.structure_id = su.location_series_structure_id
            ) AS location_series_structure_name,--工程

			mc.machine_name,--機器名称

			
			(
                SELECT
                    tra.translation_text
                FROM
                    v_structure_item_all AS tra
                WHERE
                    tra.language_id = temp.languageId
                AND tra.location_structure_id = (
                        SELECT
                            --MAX(st_f.factory_id)
							0
                        FROM
                            #temp_structure_factory AS st_f
                        WHERE
                            st_f.structure_id = mc.equipment_level_structure_id
                        AND st_f.factory_id IN(0, su.location_factory_structure_id)
                    )
                AND tra.structure_id = mc.equipment_level_structure_id
            ) as equipment_level_name,--機器レベル

            (
                SELECT
                    tra.translation_text
                FROM
                    v_structure_item_all AS tra
                WHERE
                    tra.language_id = temp.languageId
                AND tra.location_structure_id = (
                        SELECT
                            MAX(st_f.factory_id)
                        FROM
                            #temp_structure_factory AS st_f
                        WHERE
                            st_f.structure_id = hi.failure_equipment_model_structure_id
                        AND st_f.factory_id IN(0, su.location_factory_structure_id)
                    )
                AND tra.structure_id = hi.failure_equipment_model_structure_id
            ) AS failure_equipment_model_structure_name,--故障機種（機器）

            (
                SELECT
                    tra.translation_text
                FROM
                    v_structure_item_all AS tra
                WHERE
                    tra.language_id = temp.languageId
                AND tra.location_structure_id = (
                        SELECT
                            MAX(st_f.factory_id)
                        FROM
                            #temp_structure_factory AS st_f
                        WHERE
                            st_f.structure_id = mc.job_small_classfication_structure_id
                        AND st_f.factory_id IN(0, su.location_factory_structure_id)
                    )
                AND tra.structure_id = mc.job_small_classfication_structure_id
            ) AS job_small_classfication_structure_name,--機種小分類
			         
			hi.parts_existence_flg,--予備品有無
			

            (
                SELECT
                    tra.translation_text
                FROM
                    v_structure_item_all AS tra
                WHERE
                    tra.language_id = temp.languageId
                AND tra.location_structure_id = (
                        SELECT
                            MAX(st_f.factory_id)
                        FROM
                            #temp_structure_factory AS st_f
                        WHERE
                            st_f.structure_id = hi.history_importance_structure_id
                        AND st_f.factory_id IN(0, su.location_factory_structure_id)
                    )
                AND tra.structure_id = hi.history_importance_structure_id
            ) AS history_importance_structure_name,--機器レベル

            (
                SELECT
                    tra.translation_text
                FROM
                    v_structure_item_all AS tra
                WHERE
                    tra.language_id = temp.languageId
                AND tra.location_structure_id = (
                        SELECT
                            MAX(st_f.factory_id)
                        FROM
                            #temp_structure_factory AS st_f
                        WHERE
                            st_f.structure_id = hi.history_conservation_structure_id
                        AND st_f.factory_id IN(0, su.location_factory_structure_id)
                    )
                AND tra.structure_id = hi.history_conservation_structure_id
            ) AS history_conservation_structure_name,--保全方式

			'' as roumuhi,--	労務費(機器)
			hi.discovery_personnel,--発見者

            (
                SELECT
                    tra.translation_text
                FROM
                    v_structure_item_all AS tra
                WHERE
                    tra.language_id = temp.languageId
                AND tra.location_structure_id = (
                        SELECT
                            MAX(st_f.factory_id)
                        FROM
                            #temp_structure_factory AS st_f
                        WHERE
                            st_f.structure_id = req.discovery_methods_structure_id
                        AND st_f.factory_id IN(0, su.location_factory_structure_id)
                    )
                AND tra.structure_id = req.discovery_methods_structure_id
            ) AS discovery_methods_structure_name,--発見方法

			hf.recovery_action,--復旧処置

            (
                SELECT
                    tra.translation_text
                FROM
                    v_structure_item_all AS tra
                WHERE
                    tra.language_id = temp.languageId
                AND tra.location_structure_id = (
                        SELECT
                            MAX(st_f.factory_id)
                        FROM
                            #temp_structure_factory AS st_f
                        WHERE
                            st_f.structure_id = hf.failure_personality_factor_structure_id
                        AND st_f.factory_id IN(0, su.location_factory_structure_id)
                    )
                AND tra.structure_id = hf.failure_personality_factor_structure_id
            ) AS failure_personality_factor_structure_name,--故障原因性格要因

            (
                SELECT
                    tra.translation_text
                FROM
                    v_structure_item_all AS tra
                WHERE
                    tra.language_id = temp.languageId
                AND tra.location_structure_id = (
                        SELECT
                            MAX(st_f.factory_id)
                        FROM
                            #temp_structure_factory AS st_f
                        WHERE
                            st_f.structure_id = hf.failure_personality_class_structure_id
                        AND st_f.factory_id IN(0, su.location_factory_structure_id)
                    )
                AND tra.structure_id = hf.failure_personality_class_structure_id
            ) AS failure_personality_class_structure_name,--故障原因性格分類

            (
                SELECT
                    tra.translation_text
                FROM
                    v_structure_item_all AS tra
                WHERE
                    tra.language_id = temp.languageId
                AND tra.location_structure_id = (
                        SELECT
                            MAX(st_f.factory_id)
                        FROM
                            #temp_structure_factory AS st_f
                        WHERE
                            st_f.structure_id = hi.effect_production_structure_id
                        AND st_f.factory_id IN(0, su.location_factory_structure_id)
                    )
                AND tra.structure_id = hi.effect_production_structure_id
            ) AS effect_production_structure_name,--生産への影響

            (
                SELECT
                    tra.translation_text
                FROM
                    v_structure_item_all AS tra
                WHERE
                    tra.language_id = temp.languageId
                AND tra.location_structure_id = (
                        SELECT
                            MAX(st_f.factory_id)
                        FROM
                            #temp_structure_factory AS st_f
                        WHERE
                            st_f.structure_id = hi.effect_quality_structure_id
                        AND st_f.factory_id IN(0, su.location_factory_structure_id)
                    )
                AND tra.structure_id = hi.effect_quality_structure_id
            ) AS effect_quality_structure_name,--品質への影響



			hi.stop_count,--系停止回数
			su.stop_time,--系停止時間
			'' as zappi,--	雑費(機器)
			'' as kyoutuuhi,--	共通費(機器)
			'' as machine_work_time,--	作業時間(機器)
			'' as machineself_time,--	自社時間(機器)
			'' as machine_sekougaisya,--	施工会社（機器）

            (
                SELECT
                    tra.translation_text
                FROM
                    v_structure_item_all AS tra
                WHERE
                    tra.language_id = temp.languageId
                AND tra.location_structure_id = (
                        SELECT
                            MAX(st_f.factory_id)
                        FROM
                            #temp_structure_factory AS st_f
                        WHERE
                            st_f.structure_id = hf.failure_cause_structure_id
                        AND st_f.factory_id IN(0, su.location_factory_structure_id)
                    )
                AND tra.structure_id = hf.failure_cause_structure_id
            ) AS failure_cause_structure_name,--故障原因

			hi.total_working_time,--修理時間(総計)
			hi.working_time_research,--調査
			hi.working_time_procure,--調達
			hi.working_time_repair,--修復
			hi.working_time_test,--試運転

            (
                SELECT
                    tra.translation_text
                FROM
                    v_structure_item_all AS tra
                WHERE
                    tra.language_id = temp.languageId
                AND tra.location_structure_id = (
                        SELECT
                            MAX(st_f.factory_id)
                        FROM
                            #temp_structure_factory AS st_f
                        WHERE
                            st_f.structure_id = hf.treatment_measure_structure_id
                        AND st_f.factory_id IN(0, su.location_factory_structure_id)
                    )
                AND tra.structure_id = hf.treatment_measure_structure_id
            ) AS treatment_measure_structure_name,--処置・対策

			hi.expenditure,--実績金額

            (
                SELECT
                    tra.translation_text
                FROM
                    v_structure_item_all AS tra
                WHERE
                    tra.language_id = temp.languageId
                AND tra.location_structure_id = (
                        SELECT
                            MAX(st_f.factory_id)
                        FROM
                            #temp_structure_factory AS st_f
                        WHERE
                            st_f.structure_id = su.job_structure_id
                        AND st_f.factory_id IN(0, su.location_factory_structure_id)
                    )
                AND tra.structure_id = su.job_structure_id
            ) AS job_name,--職種

            (
                SELECT
                    tra.translation_text
                FROM
                    v_structure_item_all AS tra
                WHERE
                    tra.language_id = temp.languageId
                AND tra.location_structure_id = (
                        SELECT
                            MAX(st_f.factory_id)
                        FROM
                            #temp_structure_factory AS st_f
                        WHERE
                            st_f.structure_id = mc.location_factory_structure_id
                        AND st_f.factory_id IN(0, su.location_factory_structure_id)
                    )
                AND tra.structure_id = mc.location_factory_structure_id
            ) AS machine_factory_name,--工場(機器)

            (
                SELECT
                    tra.translation_text
                FROM
                    v_structure_item_all AS tra
                WHERE
                    tra.language_id = temp.languageId
                AND tra.location_structure_id = (
                        SELECT
                            MAX(st_f.factory_id)
                        FROM
                            #temp_structure_factory AS st_f
                        WHERE
                            st_f.structure_id = mc.location_stroke_structure_id
                        AND st_f.factory_id IN(0, su.location_factory_structure_id)
                    )
                AND tra.structure_id = mc.location_stroke_structure_id
            ) AS machine_stroke_name,--工程(機器)

			att.file_name as failure_cause_report_name ,--	故障原因分析書
            (
                SELECT
                    tra.translation_text
                FROM
                    v_structure_item_all AS tra
                WHERE
                    tra.language_id = temp.languageId
                AND tra.location_structure_id = (
                        SELECT
                            MAX(st_f.factory_id)
                        FROM
                            #temp_structure_factory AS st_f
                        WHERE
                            st_f.structure_id = mc.location_series_structure_id
                        AND st_f.factory_id IN(0, su.location_factory_structure_id)
                    )
                AND tra.structure_id = mc.location_series_structure_id
            ) AS machine_series_name,--系列(機器)

			hf.improvement_measure,--再発防止対策

			(
                SELECT
                    tra.translation_text
                FROM
                    v_structure_item_all AS tra
                WHERE
                    tra.language_id = temp.languageId
                AND tra.location_structure_id = (
                        SELECT
                            MAX(st_f.factory_id)
                        FROM
                            #temp_structure_factory AS st_f
                        WHERE
                            st_f.structure_id = hf.measure_class1_structure_id
                        AND st_f.factory_id IN(0, su.location_factory_structure_id)
                    )
                AND tra.structure_id = hf.measure_class1_structure_id
            ) AS measure_class1_name,--対策分類Ⅰ

			(
                SELECT
                    tra.translation_text
                FROM
                    v_structure_item_all AS tra
                WHERE
                    tra.language_id = temp.languageId
                AND tra.location_structure_id = (
                        SELECT
                            MAX(st_f.factory_id)
                        FROM
                            #temp_structure_factory AS st_f
                        WHERE
                            st_f.structure_id = hf.measure_class2_structure_id
                        AND st_f.factory_id IN(0, su.location_factory_structure_id)
                    )
                AND tra.structure_id = hf.measure_class2_structure_id
            ) AS measure_class2_name,--対策分類Ⅱ


			'' as applicable_laws_name3,--	適用法規３
			FORMAT(hf.measure_plan_date,'yyyy/MM/dd') as measure_plan_date ,--	再発防止対策予定日
			'' as applicable_laws_name5,--	適用法規５
			            (
                SELECT
                    tra.translation_text
                FROM
                    v_structure_item_all AS tra
                WHERE
                    tra.language_id = temp.languageId
                AND tra.location_structure_id = (
                        SELECT
                            MAX(st_f.factory_id)
                        FROM
                            #temp_structure_factory AS st_f
                        WHERE
                            st_f.structure_id = su.location_factory_structure_id
                        AND st_f.factory_id IN(0, su.location_factory_structure_id)
                    )
                AND tra.structure_id = hf.necessity_measure_structure_id
            ) AS necessity_measure_name,--	要／否

            (
                SELECT
                    tra.translation_text
                FROM
                    v_structure_item_all AS tra
                WHERE
                    tra.language_id = temp.languageId
                AND tra.location_structure_id = (
                        SELECT
                            MAX(st_f.factory_id)
                        FROM
                            #temp_structure_factory AS st_f
                        WHERE
                            st_f.structure_id = su.location_factory_structure_id
                        AND st_f.factory_id IN(0, su.location_factory_structure_id)
                    )
                AND tra.structure_id = su.location_factory_structure_id
            ) AS machine_series_name,--工場

			mc.machine_no,--機器番号
			hi.call_count,--呼出
			hi.failure_time--故障時間(年)


		FROM
			ma_summary su

		LEFT JOIN ma_history hi
			ON hi.summary_id = su.summary_id

	    LEFT JOIN ma_history_machine hm 
			ON hi.history_id = hm.history_id 

		LEFT JOIN ma_history_failure hf
			ON hi.history_id = hf.history_id 

		LEFT JOIN ma_plan mp
	        ON su.summary_id = mp.summary_id

	    LEFT JOIN ma_request req
	        ON su.summary_id = req.summary_id

		LEFT JOIN mc_machine mc 
			ON (hf.machine_id = mc.machine_id or hm.machine_id = mc.machine_id)

		LEFT JOIN mc_equipment eq 
			ON hf.equipment_id = eq.equipment_id 

        LEFT JOIN (SELECT MAX(location_structure_id) AS location_structure_id, structure_id, structure_item_id
					 FROM v_structure_item_all
					 WHERE structure_group_id = 1210
                     AND location_structure_id in (0, @Key3)
                     AND language_id = @Key2
                     GROUP BY structure_id, language_id, structure_item_id) vsi 
            ON eq.use_segment_structure_id = vsi.structure_id 

        LEFT JOIN ms_item_extension ie 
            ON vsi.structure_item_id = ie.item_id 
            AND ie.sequence_no = 1 

		LEFT JOIN ma_history_inspection_site his 
			ON hm.history_machine_id = his.history_machine_id 

	    LEFT JOIN ma_history_inspection_content hic 
			ON his.history_inspection_site_id = hic.history_inspection_site_id

	    LEFT JOIN (select * from  attachment where function_type_id = 1690 ) att  
			ON att.key_id = hf.history_failure_id 
        INNER JOIN #temp temp
        ON su.summary_id = temp.Key1
        ORDER BY
		1,2,
            mc.job_structure_id ASC,
			mc.machine_no ASC,
			his.inspection_site_structure_id ASC,
			hic.inspection_content_structure_id ASC;