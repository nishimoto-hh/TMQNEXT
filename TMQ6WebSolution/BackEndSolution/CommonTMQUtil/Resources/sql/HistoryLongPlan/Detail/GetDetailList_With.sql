/*
 * 件名別長期計画一覧　参照画面　保全情報一覧の検索　With句
*/
-- 基本テーブル
-- 指定された長期計画件名IDに紐づく機番情報、機器別管理基準部位、機器別管理基準内容のキー
-- このテーブルを画面の計画内容に応じて集計し、キーを指定する
WITH org_base AS ( 
    --トランザクション
    SELECT
        machine.machine_id
        , man_com.management_standards_component_id
        , man_con.management_standards_content_id
        , man_con.long_plan_id 
    FROM
        mc_management_standards_content AS man_con 
        INNER JOIN mc_management_standards_component AS man_com 
            ON ( 
                man_con.management_standards_component_id = man_com.management_standards_component_id
            ) 
        INNER JOIN mc_machine AS machine 
            ON (man_com.machine_id = machine.machine_id) 
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
) 
, hm_base AS ( 
    --変更管理
    SELECT
        machine.machine_id
        , man_com.management_standards_component_id
        , man_con.management_standards_content_id
        , man_con.long_plan_id 
    FROM
        hm_history_management hm 
        INNER JOIN hm_mc_management_standards_content man_con 
            ON hm.history_management_id = man_con.history_management_id 
        INNER JOIN mc_management_standards_component AS man_com 
            ON ( 
                man_con.management_standards_component_id = man_com.management_standards_component_id
            ) 
        INNER JOIN mc_machine AS machine 
            ON (man_com.machine_id = machine.machine_id) 
        LEFT JOIN ms_structure status_ms        -- 構成マスタ(申請状況)
            ON hm.application_status_id = status_ms.structure_id 
        LEFT JOIN ms_item_extension status_ex   -- アイテムマスタ拡張(申請状況)
            ON status_ms.structure_item_id = status_ex.item_id 
            AND status_ex.sequence_no = 1 
    WHERE
        man_con.long_plan_id = @LongPlanId 
        AND status_ex.extension_data IN ('10', '20', '30') --「申請データ作成中」「承認依頼中」「差戻中」のデータのみ
        AND hm.history_management_id = @HistoryManagementId 
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
) 
, base AS ( 
    SELECT
        machine_id
        , management_standards_component_id
        , management_standards_content_id
        , long_plan_id 
    FROM
        org_base 
    WHERE
        NOT EXISTS ( 
            SELECT
                * 
            FROM
                hm_mc_management_standards_content hmsc 
            WHERE
                hmsc.execution_division = 5      --保全情報一覧の削除
                AND hmsc.management_standards_content_id = org_base.management_standards_content_id
        )                                       --削除した保全情報一覧の情報は除外する
        UNION 
    SELECT
        machine_id
        , management_standards_component_id
        , management_standards_content_id
        , long_plan_id 
    FROM
        hm_base
) 
, 
-- 各テーブル
-- 集計に応じて値が不要なテーブルがあるので、各SQLでJOINする
-- 表示する列をそれぞれ取得
-- 機番情報
machine AS ( 
    SELECT
        machine_id
        , machine_no
        , machine_name
        , location_structure_id
        , importance_structure_id
        ,                                       -- 添付情報の更新日時
        ( 
            SELECT
                MAX(att.update_datetime) 
            FROM
                mc_equipment AS eq 
                LEFT OUTER JOIN attachment AS att 
                    ON ( 
                        eq.equipment_id = att.key_id 
                        AND att.function_type_id = 1640
                    ) 
            WHERE
                eq.machine_id = machine.machine_id
        ) AS attachment_update_datetime 
    FROM
        mc_machine AS machine
) 
,
-- 機器別管理基準部位
man_com AS ( 
    SELECT
        management_standards_component_id
        , update_serialid AS update_serialid_component
        , inspection_site_structure_id 
    FROM
        mc_management_standards_component
) 
,
--点検種別の拡張アイテム
item_ex AS ( 
    SELECT
        item.structure_id
        , item_ex.extension_data 
    FROM
        v_structure_all AS item 
        LEFT OUTER JOIN ms_item_extension AS item_ex 
            ON ( 
                item_ex.item_id = item.structure_item_id 
                AND item_ex.sequence_no = 1
            ) 
    WHERE
        item.structure_group_id = 1240
        AND item.factory_id IN @FactoryIdList
) 
,
-- 機器別管理基準内容
man_con AS ( 
    SELECT
        con.management_standards_content_id
        , COALESCE(hmcon.update_serialid, con.update_serialid) AS update_serialid_content
        , COALESCE( 
            hmcon.inspection_content_structure_id
            , con.inspection_content_structure_id
        ) AS inspection_content_structure_id
        , COALESCE(hmcon.budget_amount, con.budget_amount) AS budget_amount
        , COALESCE( 
            hmcon.maintainance_kind_structure_id
            , con.maintainance_kind_structure_id
        ) AS maintainance_kind_structure_id
        , COALESCE(hmitem.extension_data, item.extension_data) AS kind_order
        , COALESCE( 
            hmcon.schedule_type_structure_id
            , con.schedule_type_structure_id
        ) AS schedule_type_structure_id 
    FROM
        mc_management_standards_content AS con 
        LEFT OUTER JOIN item_ex AS item 
            ON ( 
                item.structure_id = con.maintainance_kind_structure_id
            ) 
        LEFT OUTER JOIN ( 
            SELECT
                hmcon.* 
            FROM
                hm_history_management hm 
                LEFT JOIN hm_mc_management_standards_content hmcon 
                    ON hm.history_management_id = hmcon.history_management_id 
            WHERE
                hm.history_management_id = @HistoryManagementId
                AND hmcon.execution_division IN (4, 5) --保全情報一覧の追加、削除
        ) hmcon 
            ON ( 
                con.management_standards_content_id = hmcon.management_standards_content_id
            ) 
        LEFT OUTER JOIN item_ex AS hmitem 
            ON ( 
                hmitem.structure_id = hmcon.maintainance_kind_structure_id
            )
) 