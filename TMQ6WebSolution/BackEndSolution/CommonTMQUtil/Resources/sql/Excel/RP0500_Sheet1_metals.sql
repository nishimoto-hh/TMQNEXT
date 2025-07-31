declare @key2 as nvarchar(2) ='ja' ;
declare @key3 as int =4126;

DROP TABLE IF EXISTS #temp_structure_factory;
CREATE TABLE #temp_structure_factory(
    structure_id int
    ,factory_id int
);

INSERT INTO #temp_structure_factory
-- 使用する構成グループの構成IDを絞込、工場の指定に用いる
SELECT
    structure_id
    ,location_structure_id AS factory_id
FROM
    v_structure_item_all
WHERE
    structure_group_id IN (1000,1010,1200,1850,1080,1400,1810,1020,1510)
AND factory_id IN(0,4126,4127,4128,4129,4130,4131,4132,4133,4134,4135,4136,4137,4138,4139,4140,4141,4142,330184) 
AND language_id = @key2;


DROP TABLE IF EXISTS #temp_excel_output;

--MQ分類並び順設定

select
	translation_text,display_order
into #temp_order
from
	v_structure_item_all v
	inner join ms_structure_order o on v.structure_id = o.structure_id and v.factory_id = o.factory_id
where
	v.structure_group_id = 1850 
	and v.location_structure_id = 4126;

        SELECT
		su.summary_id
	,(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @key2
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = su.mq_class_structure_id
                AND st_f.factory_id IN(0, su.location_factory_structure_id)
            )
        AND tra.structure_id = mq_class_structure_id
    ) AS mq_class_name
    ,req.issue_date
    ,su.completion_date
	,
		(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @key2
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
    ) AS plant_name

	,su.subject
	,su.plan_implementation_content
	,
		(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @key2
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
    ) AS job_name
	,hi.construction_personnel_name
	,hi.construction_company
	,isnull(
		(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @key2
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = mc.importance_structure_id
                AND st_f.factory_id IN(0, su.location_factory_structure_id)
            )
        AND tra.structure_id = mc.importance_structure_id
    )
	,'未設定')  AS importance_name,
		(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @key2
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
    ) AS discovery_methods_name

	,hi.total_working_time

	,
		(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @key2
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = su.sudden_division_structure_id
                AND st_f.factory_id IN(0, su.location_factory_structure_id)
            )
        AND tra.structure_id = su.sudden_division_structure_id
    ) AS sudden_division_name
	
	,
		(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @key2
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
    ) AS failure_cause_name
	
	,
		(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @key2
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = hf.failure_cause_personality_structure_id
                AND st_f.factory_id IN(0, su.location_factory_structure_id)
            )
        AND tra.structure_id = hf.failure_cause_personality_structure_id
    ) AS failure_cause_personality_name

	,
		(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @key2
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = hf.phenomenon_structure_id
                AND st_f.factory_id IN(0, su.location_factory_structure_id)
            )
        AND tra.structure_id = hf.phenomenon_structure_id
    ) AS phenomenon_name

	,expenditure
	
	into #temp_excel_output		


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

        INNER JOIN #temp temp         ON su.summary_id = temp.Key1
        ORDER BY
		su.mq_class_structure_id , su.completion_date ASC;

DROP TABLE IF EXISTS #temp_excel_output2

		
select 
*
,ROW_NUMBER() OVER( PARTITION BY summary_id ORDER BY importance_name asc) AS 'seq1'
into #temp_excel_output2
from 
    #temp_excel_output e
    LEFT JOIN #temp_order o on e.mq_class_name = o.translation_text;



select 
    display_order,
    isnull(mq_class_name,'') mq_class_name,
    ROW_NUMBER() OVER(PARTITION BY mq_class_name ORDER BY completion_date asc) AS 'seq',
    isnull(issue_date,'') issue_date,
    completion_date,
    isnull(plant_name,'') plant_name,
    isnull(subject,'') subject,
    isnull(plan_implementation_content,'') plan_implementation_content,
    isnull(job_name,'') job_name,
    isnull(construction_personnel_name,'') construction_personnel_name,
    isnull(construction_company,'') construction_company,
    replace(isnull(importance_name,''),'未設定','') importance_name,
    isnull(discovery_methods_name,'') discovery_methods_name,
    isnull(total_working_time,0) total_working_time,
    isnull(sudden_division_name,'') sudden_division_name,
    isnull(failure_cause_name,'') failure_cause_name,
    isnull(failure_cause_personality_name,'') failure_cause_personality_name,
    isnull(phenomenon_name,'') phenomenon_name,
    isnull(expenditure,0) expenditure
from
    #temp_excel_output2
where
    seq1 = 1 
order by
    display_order,completion_date asc
