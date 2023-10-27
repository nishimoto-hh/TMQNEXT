/*
 * MakePlanとMakeMaintainanceのこのファイルは同一
 * 変更時は注意、処理での都合で機能でフォルダを分けているため。
*/
/*
* 保全スケジュール詳細更新
* 同時に下位の点検種別も更新する
*/
-- 保全スケジュール詳細から機器IDと点検種別を取得できるSQL
WITH base AS(
    SELECT
        msd.maintainance_schedule_detail_id,
        com.machine_id,
        con.maintainance_kind_structure_id,
        ext.extension_data AS kind_order,
        msd.schedule_date,
        msd.summary_id
    FROM
        mc_maintainance_schedule_detail AS msd
        INNER JOIN
            mc_maintainance_schedule AS msh
        ON  (
                msd.maintainance_schedule_id = msh.maintainance_schedule_id
            )
        INNER JOIN
            mc_management_standards_content AS con
        ON  (
                msh.management_standards_content_id = con.management_standards_content_id
            )
        INNER JOIN
            mc_management_standards_component AS com
        ON  (
                con.management_standards_component_id = com.management_standards_component_id
            )
        INNER JOIN
            ms_structure AS st
        ON  (
                con.maintainance_kind_structure_id = st.structure_id
            )
        INNER JOIN
            ms_item_extension AS ext
        ON  (
                st.structure_item_id = ext.item_id
            )
    WHERE
        st.delete_flg = 0
),
-- 選択された保全スケジュール詳細IDの情報を取得
origin AS(
    SELECT
        *
    FROM
        base
    WHERE
        base.maintainance_schedule_detail_id = @MaintainanceScheduleDetailId
),
-- 下位の点検種別で保全活動未作成の保全スケジュール詳細を取得
down AS(
    SELECT
        *
    FROM
        base
    WHERE
        EXISTS(
            SELECT
                *
            FROM
                origin
            WHERE
                origin.machine_id = base.machine_id
            AND origin.maintainance_kind_structure_id <> base.maintainance_kind_structure_id
            AND origin.kind_order < base.kind_order
        )
    AND base.schedule_date BETWEEN @ScheduleDateFrom AND @ScheduleDateTo
    AND base.summary_id IS NULL
)
-- まとめて更新
UPDATE
    upd
SET
    upd.summary_id = @SummaryId,
    upd.update_serialid = upd.update_serialid + 1,
    upd.update_datetime = @UpdateDatetime,
    upd.update_user_id = @UpdateUserId
FROM
    mc_maintainance_schedule_detail AS upd
WHERE
    upd.maintainance_schedule_detail_id = @MaintainanceScheduleDetailId
OR  EXISTS(
        SELECT
            *
        FROM
            down
        WHERE
            upd.maintainance_schedule_detail_id = down.maintainance_schedule_detail_id
    )
