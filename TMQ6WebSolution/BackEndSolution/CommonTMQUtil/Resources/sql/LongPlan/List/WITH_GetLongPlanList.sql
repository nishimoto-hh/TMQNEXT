/*
* 一覧の項目の内容を取得するSQL
*/
WITH eq_att AS(
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
)
,
-- 排他処理で使用する項目(長期計画件名IDごとの最大の更新日時)
max_dt AS(
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
    GROUP BY
         lp.long_plan_id
)
,structure_factory AS(
     -- 使用する構成グループの構成IDを絞込、工場の指定に用いる
    SELECT
         structure_id
        ,location_structure_id AS factory_id
    FROM
        v_structure_item_all
    WHERE
        structure_group_id IN(1330, 1280, 1300, 1060, 1290, 1410, 1310, 1320)
    AND language_id = @LanguageId
)
,prepare_limit_days AS(
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
        lp.long_plan_id,
        msd.schedule_date,
        COALESCE(preparation_period, 0) AS preparation_period,
        row_number() over(partition BY lp.long_plan_id ORDER BY msd.schedule_date) AS row_num
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
)
,prepare_target_narrow AS(
-- 準備対象列取得用、直近1件に絞込、準備期間の日数を引き対象のもののみを表示、これを外部結合し、有無により準備対象列と判定する
    SELECT
        pt.long_plan_id
    FROM
        prepare_target AS pt
    WHERE
        pt.row_num = 1
    AND GETDATE() >= DATEADD(dd,(pt.preparation_period) * (- 1), pt.schedule_date)
)
,target AS(
     -- 表示情報を取得するSQL、翻訳対応のためWITH句へ
    SELECT
         lp.long_plan_id
        ,dbo.get_target_layer_id(lp.location_structure_id, 1) AS factory_id
        ,lp.subject
        ,lp.subject_note
        ,lp.location_structure_id
        ,lp.job_structure_id
        ,lp.maintenance_season_structure_id
        ,lp.person_id
        ,coalesce(person.display_name, lp.person_name) AS person_name
        ,lp.work_item_structure_id
        ,lp.budget_management_structure_id
        ,lp.budget_personality_structure_id
        ,
        -- 機器添付有無
        -- ひとつの件名に複数の機器別管理基準内容が紐づきうるので、複数の機器の添付情報を結合して表示する
        (
             SELECT
                (
                     SELECT
                         dbo.get_file_download_info(1600, eq_att.machine_id) + dbo.get_file_download_info(1610, eq_att.equipment_id)
                    FROM
                        eq_att
                    WHERE
                        eq_att.long_plan_id = lp.long_plan_id
                    ORDER BY
                         eq_att.long_plan_id FOR xml path('')
                )
        ) AS file_link_equip
        ,dbo.get_file_download_info(1640, lp.long_plan_id) AS file_link_subject
        ,lp.purpose_structure_id
        ,lp.work_class_structure_id
        ,lp.treatment_structure_id
        ,lp.facility_structure_id
        ,lp.update_serialid
        ,
        -- 参照画面の排他処理で用いる項目
        max_dt.long_plan_id_dt
        ,max_dt.mc_man_st_con_update_datetime
        ,max_dt.sche_detail_update_datetime
        ,max_dt.attachment_update_datetime
        -- スケジュールと同じ値
        ,lp.long_plan_id AS key_id
        -- 準備対象列
        ,COALESCE((SELECT 1 FROM prepare_target_narrow AS pt WHERE pt.long_plan_id = lp.long_plan_id),0) AS preparation_flg
    FROM
         ln_long_plan AS lp
        LEFT OUTER JOIN
            ms_user AS person
        ON  (
                lp.person_id = person.user_id
            )
        LEFT OUTER JOIN
            max_dt
        ON  (
                lp.long_plan_id = max_dt.long_plan_id_dt
            )
)