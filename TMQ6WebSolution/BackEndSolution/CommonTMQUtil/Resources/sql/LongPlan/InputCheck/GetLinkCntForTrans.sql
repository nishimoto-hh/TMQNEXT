DROP TABLE IF EXISTS #content_id_list; 

CREATE TABLE #content_id_list( 
    maintainance_schedule_detail_id bigint
    , management_standards_content_id bigint
); 

DROP TABLE IF EXISTS #detail_id_list; 

CREATE TABLE #detail_id_list( 
    management_standards_content_id bigint
    , maintainance_schedule_detail_id nvarchar(max)
); 

WITH condition AS ( 
    -- 検索条件
    SELECT
        content.long_plan_id
        , detail.schedule_date 
    FROM
        mc_maintainance_schedule_detail detail 
        INNER JOIN mc_maintainance_schedule schedule 
            ON detail.maintainance_schedule_id = schedule.maintainance_schedule_id 
        INNER JOIN mc_management_standards_content content 
            ON schedule.management_standards_content_id = content.management_standards_content_id 
    WHERE
        detail.maintainance_schedule_detail_id = @MaintainanceScheduleDetailId
)                                               -- 機器別管理基準内容IDに紐付く保全スケジュール詳細IDを取得
INSERT 
INTO #content_id_list 
SELECT
    detail.maintainance_schedule_detail_id
    , content_b.management_standards_content_id 
FROM
    mc_management_standards_content content_b 
    INNER JOIN mc_maintainance_schedule schedule 
        ON content_b.management_standards_content_id = schedule.management_standards_content_id 
    INNER JOIN mc_maintainance_schedule_detail detail 
        ON schedule.maintainance_schedule_id = detail.maintainance_schedule_id 
    INNER JOIN condition 
        ON content_b.long_plan_id = condition.long_plan_id 
        AND format(detail.schedule_date, 'yyyyMM') = format(condition.schedule_date, 'yyyyMM'); 

-- 機器別管理基準内容IDに紐付く保全スケジュール詳細IDをカンマ区切りで取得
INSERT 
INTO #detail_id_list 
SELECT
    content_a.management_standards_content_id
    , trim( 
        ',' 
        FROM
            ( 
                SELECT
                    CAST( 
                        content_id_list.maintainance_schedule_detail_id AS VARCHAR
                    ) + ',' 
                FROM
                    #content_id_list content_id_list 
                WHERE
                    content_id_list.management_standards_content_id = content_a.management_standards_content_id FOR xml 
                    PATH ('')
            )
    ) AS maintainance_schedule_detail_id 
FROM
    mc_management_standards_content content_a 
    INNER JOIN #content_id_list content_id_list 
        ON content_a.management_standards_content_id = content_id_list.management_standards_content_id 
GROUP BY
    content_a.management_standards_content_id; 

-- リンク遷移元のデータと同一長期計画件名に含まれる同年月のデータ
SELECT

    /*@MaintainanceKindManage
    -- クリックされた○リンクの機器が点検種別毎管理の場合は機器ごとの件数を重複を無くして取得
    COUNT(DISTINCT machine_id) 
　　@MaintainanceKindManage*/

    /*@NotMaintainanceKindManage
    -- クリックされた○リンクの機器が点検種別毎管理では無い場合は単純に対象機器一覧に表示する件数を取得
    COUNT(*) 
　　@NotMaintainanceKindManage*/

FROM
    ( 
        SELECT DISTINCT
            machine.machine_id                               -- 機番ID
            , detail_id_list.maintainance_schedule_detail_id -- 保全スケジュール詳細ID
        FROM
            mc_management_standards_content content 
            INNER JOIN #detail_id_list detail_id_list 
                ON content.management_standards_content_id = detail_id_list.management_standards_content_id 
            INNER JOIN mc_management_standards_component component 
                ON content.management_standards_component_id = component.management_standards_component_id 
            INNER JOIN mc_machine machine 
                ON component.machine_id = machine.machine_id 
            INNER JOIN mc_equipment equipment 
                ON machine.machine_id = equipment.machine_id 
            INNER JOIN #content_id_list content_id_list 
                ON content.management_standards_content_id = content_id_list.management_standards_content_id 
            INNER JOIN mc_maintainance_schedule_detail detail 
                ON detail.maintainance_schedule_detail_id = content_id_list.maintainance_schedule_detail_id 
        WHERE
            detail.summary_id IS NULL
    ) main
