INSERT INTO #eq_att
-- 機器情報添付の取得に使用する機器IDを機器情報より取得
SELECT DISTINCT
    mscn.long_plan_id
    ,mc.machine_id
    ,eq.equipment_id
FROM
    mc_management_standards_content AS mscn
    LEFT OUTER JOIN
        mc_management_standards_component AS mscm
    ON  (
            mscn.management_standards_component_id = mscm.management_standards_component_id
        )
    LEFT OUTER JOIN
        mc_machine AS mc
    ON  (
            mscm.machine_id = mc.machine_id
        )
    LEFT OUTER JOIN
        mc_equipment AS eq
    ON  (
            mc.machine_id = eq.machine_id
        )
WHERE
    mscn.long_plan_id IS NOT NULL
AND mc.machine_id IS NOT NULL
AND eq.equipment_id IS NOT NULL
;
INSERT INTO #max_dt
-- 排他処理で使用する項目(長期計画件名IDごとの最大の更新日時)
SELECT
    lp.long_plan_id AS long_plan_id_dt
    ,
    -- 機器別管理基準内容
    MAX(ms_con.update_datetime) AS mc_man_st_con_update_datetime
    ,
    -- スケジュール詳細
    MAX(schedule_detail.update_datetime) AS sche_detail_update_datetime
    ,
    -- 添付情報(長期計画)
    MAX(att.update_datetime) AS attachment_update_datetime
FROM
    ln_long_plan AS lp
LEFT OUTER JOIN
    mc_management_standards_content AS ms_con
ON  (
        ms_con.long_plan_id = lp.long_plan_id
    )
LEFT OUTER JOIN
    mc_maintainance_schedule AS schedule
ON  (
        schedule.management_standards_content_id = ms_con.management_standards_content_id
    )
LEFT OUTER JOIN
    mc_maintainance_schedule_detail AS schedule_detail
ON  (
        schedule_detail.maintainance_schedule_id = schedule.maintainance_schedule_id
    )
LEFT OUTER JOIN
    attachment AS att
ON  (
        att.key_id = lp.long_plan_id
    AND att.function_type_id = 1640
    )
WHERE
/*@ForList
    EXISTS(
        SELECT
            *
        FROM
            #temp_location temp
        WHERE
            lp.location_structure_id = temp.structure_id
    )
AND EXISTS(
        SELECT
            *
        FROM
            #temp_job temp
        WHERE
            lp.job_structure_id = temp.structure_id
    )
@ForList*/
/*@ForDetail
    lp.long_plan_id = @LongPlanId
@ForDetail*/
GROUP BY
    lp.long_plan_id
;
WITH prepare_limit_days AS(
    -- 準備対象列取得用、表示対象の日数を拡張項目より取得
    SELECT
        TOP 1 ex.extension_data AS limit_days
    FROM
        v_structure AS it
        INNER JOIN
            ms_item_extension AS ex
        ON  (
                it.structure_item_id = ex.item_id
            AND ex.sequence_no = 9
            )
    WHERE
        it.structure_group_id = 2080
)
,prepare_target AS(
    -- 準備対象列取得用、機器別管理基準内でスケジュールが未完了かつ準備期間に入っているものを取得
    SELECT
        lp.long_plan_id
        ,msd.schedule_date
        ,COALESCE(preparation_period, 0) AS preparation_period
        ,row_number() over(partition BY lp.long_plan_id ORDER BY msd.schedule_date) AS row_num
    FROM
        ln_long_plan lp
        INNER JOIN
            mc_management_standards_content msc
        ON  lp.long_plan_id = msc.long_plan_id
        INNER JOIN
            mc_maintainance_schedule ms
        ON  msc.management_standards_content_id = ms.management_standards_content_id
        INNER JOIN
            mc_maintainance_schedule_detail msd
        ON  ms.maintainance_schedule_id = msd.maintainance_schedule_id
        AND COALESCE(msd.complition, 0) != 1
    WHERE
        msd.schedule_date >= DATEADD(dd,(
                SELECT
                    limit_days
                FROM
                    prepare_limit_days
            ) * (- 1), GETDATE())
/*@ForList
    AND EXISTS(
            SELECT
                *
            FROM
                #temp_location temp
            WHERE
                lp.location_structure_id = temp.structure_id
        )
    AND EXISTS(
            SELECT
                *
            FROM
                #temp_job temp
            WHERE
                lp.job_structure_id = temp.structure_id
        )
@ForList*/
/*@ForDetail
    AND lp.long_plan_id = @LongPlanId
@ForDetail*/
)
INSERT INTO #prepare_target_narrow
-- 準備対象列取得用、直近1件に絞込、準備期間の日数を引き対象のもののみを表示、これを外部結合し、有無により準備対象列と判定する
SELECT
    pt.long_plan_id
FROM
    prepare_target AS pt
WHERE
    pt.row_num = 1
AND GETDATE() >= DATEADD(dd,(pt.preparation_period) * (- 1), pt.schedule_date)
;