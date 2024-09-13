declare @startMonth1 int;
declare @factory_id int; 
declare @selectDate nvarchar(max);
declare @selectMonth int;
declare @selectYear int;
declare @baseDate date;
declare @language_id nvarchar(max);


select @language_id = 'ja';
set @selectDate = @TargetStartDate;
set @startMonth1 = 4;

select @selectMonth = cast(FORMAT(convert(date,@TargetStartDate),'MM') as int);
select @selectYear  = cast(FORMAT(convert(date,@TargetStartDate),'yyyy') as int);
IF @selectMonth < @startMonth1 set @selectYear = @selectYear -1;
	set @baseDate = cast(cast(@selectYear as nvarchar)+ '/' + cast(@startMonth1 as nvarchar) + '/1' as date);

with summary as (
select
	sum(maintenance_count) as cont
	,cast(FORMAT(completion_date,'yyyy/MM/1') as date) as month
	,location_factory_structure_id as factory_id
from
	ma_summary su
	inner join #temp_location tl on su.location_structure_id = tl.structure_id
where
	completion_date > @baseDate 
	and completion_date < dateadd(year, 1,@baseDate)
group by
	FORMAT(completion_date,'yyyy/MM/1') , location_factory_structure_id
)

select
	 v.translation_text as factory_name
	,summary01.cont  as cnt1
	,summary02.cont as cnt2
	,summary03.cont as cnt3
	,summary04.cont as cnt4
	,summary05.cont as cnt5
	,summary06.cont as cnt6
	,summary07.cont as cnt7
	,summary08.cont as cnt8
	,summary09.cont as cnt9
	,summary10.cont as cnt10
	,summary11.cont as cnt11
	,summary12.cont as cnt12
	,@selectYear as year
	,1 as sortno
from
	(select * from v_structure_item_all where language_id = @language_id) as v
	inner join (select * from summary where month = FORMAT(@baseDate,'yyyy/MM/1')) as summary01  on v.structure_id = summary01.factory_id
	left join (select * from summary where month = dateadd(month, 1,  FORMAT(@baseDate,'yyyy/MM/1'))) as summary02 on summary01.month = dateadd(month, -1,summary02.month) and v.structure_id = summary02.factory_id
	left join (select * from summary where month = dateadd(month, 2,  FORMAT(@baseDate,'yyyy/MM/1'))) as summary03 on summary02.month = dateadd(month, -1,summary03.month) and v.structure_id = summary03.factory_id
	left join (select * from summary where month = dateadd(month, 3,  FORMAT(@baseDate,'yyyy/MM/1'))) as summary04 on summary03.month = dateadd(month, -1,summary04.month) and v.structure_id = summary04.factory_id
	left join (select * from summary where month = dateadd(month, 4,  FORMAT(@baseDate,'yyyy/MM/1'))) as summary05 on summary04.month = dateadd(month, -1,summary05.month) and v.structure_id = summary05.factory_id
	left join (select * from summary where month = dateadd(month, 5,  FORMAT(@baseDate,'yyyy/MM/1'))) as summary06 on summary05.month = dateadd(month, -1,summary06.month) and v.structure_id = summary06.factory_id
	left join (select * from summary where month = dateadd(month, 6,  FORMAT(@baseDate,'yyyy/MM/1'))) as summary07 on summary06.month = dateadd(month, -1,summary07.month) and v.structure_id = summary07.factory_id
	left join (select * from summary where month = dateadd(month, 7,  FORMAT(@baseDate,'yyyy/MM/1'))) as summary08 on summary07.month = dateadd(month, -1,summary08.month) and v.structure_id = summary08.factory_id
	left join (select * from summary where month = dateadd(month, 8,  FORMAT(@baseDate,'yyyy/MM/1'))) as summary09 on summary08.month = dateadd(month, -1,summary09.month) and v.structure_id = summary09.factory_id
	left join (select * from summary where month = dateadd(month, 9,  FORMAT(@baseDate,'yyyy/MM/1'))) as summary10 on summary09.month = dateadd(month, -1,summary10.month) and v.structure_id = summary10.factory_id
	left join (select * from summary where month = dateadd(month, 10, FORMAT(@baseDate,'yyyy/MM/1'))) as summary11 on summary10.month = dateadd(month, -1,summary11.month) and v.structure_id = summary11.factory_id
	left join (select * from summary where month = dateadd(month, 11, FORMAT(@baseDate,'yyyy/MM/1'))) as summary12 on summary11.month = dateadd(month, -1,summary12.month) and v.structure_id = summary12.factory_id
	;