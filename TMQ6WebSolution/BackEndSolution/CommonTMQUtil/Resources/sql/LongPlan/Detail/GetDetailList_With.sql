/*
 * 件名別長期計画一覧　参照画面　保全情報一覧の検索　With句
*/
-- 基本テーブル
-- 指定された長期計画件名IDに紐づく機番情報、機器別管理基準部位、機器別管理基準内容のキー
-- このテーブルを画面の計画内容に応じて集計し、キーを指定する
WITH base AS(
    SELECT
        machine.machine_id,
        man_com.management_standards_component_id,
        man_con.management_standards_content_id
    FROM
        mc_management_standards_content AS man_con
        INNER JOIN
            mc_management_standards_component AS man_com
        ON  (
                man_con.management_standards_component_id = man_com.management_standards_component_id
            )
        INNER JOIN
            mc_machine AS machine
        ON  (
                man_com.machine_id = machine.machine_id
            )
    WHERE
        man_con.long_plan_id = @LongPlanId
/*@UnComp
    AND EXISTS(
            SELECT
                *
            FROM
                mc_maintainance_schedule_detail AS schedule_detail
                INNER JOIN
                    mc_maintainance_schedule AS schedule
                ON  (
                        schedule_detail.maintainance_schedule_id = schedule.maintainance_schedule_id
                    )
            WHERE
                schedule.management_standards_content_id = man_con.management_standards_content_id
            AND schedule_detail.summary_id IS NULL
        )
@UnComp*/
),
-- 各テーブル
-- 集計に応じて値が不要なテーブルがあるので、各SQLでJOINする
-- 表示する列をそれぞれ取得
-- 機番情報
machine AS(
    SELECT
        machine_id,
        machine_no,
        machine_name,
        location_structure_id,
        importance_structure_id,
        -- 添付情報の更新日時
        (
            SELECT
                MAX(att.update_datetime)
            FROM
                mc_equipment AS eq
                LEFT OUTER JOIN
                    attachment AS att
                ON  (
                        eq.equipment_id = att.key_id
                    AND att.function_type_id = 1640
                    )
            WHERE
                eq.machine_id = machine.machine_id
        ) AS attachment_update_datetime
    FROM
        mc_machine AS machine
),
-- 機器別管理基準部位
man_com AS(
    SELECT
        management_standards_component_id,
        update_serialid AS update_serialid_component,
        inspection_site_structure_id
    FROM
        mc_management_standards_component
),
-- 機器別管理基準内容
man_con AS(
    SELECT
        con.management_standards_content_id,
        con.update_serialid AS update_serialid_content,
        con.inspection_content_structure_id,
        con.budget_amount,
        con.maintainance_kind_structure_id,
        item_ex.extension_data AS kind_order,
        con.long_plan_id,
        con.schedule_type_structure_id
    FROM
        mc_management_standards_content AS con
        LEFT OUTER JOIN
            v_structure_all AS item
        ON  (
                item.structure_group_id = 1240
            AND item.structure_id = con.maintainance_kind_structure_id
            AND item.factory_id IN @FactoryIdList
            )
        LEFT OUTER JOIN
            ms_item_extension AS item_ex
        ON  (
                item_ex.item_id = item.structure_item_id
            AND item_ex.sequence_no = 1
            )
)
