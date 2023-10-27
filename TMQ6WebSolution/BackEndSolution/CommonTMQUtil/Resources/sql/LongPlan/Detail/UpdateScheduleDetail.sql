/*
* UpdateScheduleDetail.sql
* 件名別長期計画一覧　参照画面　スケジュール確定ボタン押下時、保全スケジュール詳細を更新するSQL
*/
-- 機器別管理基準内容IDから長期計画IDと機番IDと点検種別順序を取得するためのSQL(同一スケジュールとして一緒に更新する)
WITH same_base AS(
    SELECT
        con.management_standards_content_id,
        com.machine_id,
        con.long_plan_id,
        ie.extension_data AS kind_order
    FROM
        mc_management_standards_content AS con
        INNER JOIN
            mc_management_standards_component AS com
        ON  (
                com.management_standards_component_id = con.management_standards_component_id
            )
        INNER JOIN
            ms_structure AS st
        ON  (
                con.maintainance_kind_structure_id = st.structure_id
            AND st.structure_group_id = 1240
            AND st.factory_id IN @FactoryIdList
            )
        INNER JOIN
            ms_item_extension AS ie
        ON  (
                ie.item_id = st.structure_item_id
            AND ie.sequence_no = 1
            )
    WHERE
        st.delete_flg = 0
),
-- 画面で移動した機器別管理基準内容IDから上記の情報を取得
move AS(
    SELECT
        *
    FROM
        same_base
    WHERE
        management_standards_content_id = @ManagementStandardsContentId
),
-- 画面で移動した機器別管理基準内容IDから、同じ値の長期計画IDと点検種別順序のものを取得、更新対象となる
same AS(
    SELECT
        base.*
    FROM
        same_base AS base
    WHERE
        EXISTS(
            SELECT
                *
            FROM
                move
            WHERE
                base.long_plan_id = move.long_plan_id
            AND base.machine_id = move.machine_id
            AND base.kind_order = move.kind_order
        )
)
UPDATE
    detail
SET
     detail.schedule_date = dateadd(MONTH, @AddMonth, detail.schedule_date)
    ,detail.update_serialid = detail.update_serialid + 1
    ,detail.update_datetime = @UpdateDatetime
    ,detail.update_user_id = @UpdateUserId
FROM
    mc_maintainance_schedule AS head
    INNER JOIN mc_maintainance_schedule_detail AS detail
    ON  (
            head.maintainance_schedule_id = detail.maintainance_schedule_id
        )
WHERE
-- 更新対象の機器別管理基準内容IDを更新する
    EXISTS(
        SELECT
            *
        FROM
            same
        WHERE
            head.management_standards_content_id = same.management_standards_content_id
    )
AND detail.schedule_date BETWEEN @MonthStartDate AND @MonthEndDate
AND detail.complition != 1
