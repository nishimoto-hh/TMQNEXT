/*
 * 長期計画スケジュール
*/
-- 基本テーブル
-- 指定された長期計画件名IDに紐づく機番情報、機器別管理基準部位、機器別管理基準内容のキー
-- このテーブルを画面の計画内容に応じて集計し、キーを指定する
WITH base AS(
    SELECT
        machine.machine_id,                                     -- 機番ID
        man_com.management_standards_component_id,              -- 機器別管理基準部位ID
        man_con.management_standards_content_id                 -- 機器別管理基準内容ID
    FROM
        mc_management_standards_content AS man_con              -- 機器別管理基準内容
        INNER JOIN
            #temp temp                                          -- 一時テーブル（一覧画面にて選択）
        ON (
               man_con.long_plan_id = temp.Key1                 -- 長計件名ID
            )

        INNER JOIN
            mc_management_standards_component AS man_com        -- 機器別管理基準部位
        ON  (
                -- 機器別管理基準部位ID
                man_con.management_standards_component_id = man_com.management_standards_component_id
            )
        INNER JOIN
            mc_machine AS machine                               -- 機番情報
        ON  (
                man_com.machine_id = machine.machine_id         -- 機番ID
            )
            
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
        machine_id,                                             -- 機番ID
        machine_no,                                             -- 機器番号
        machine_name,                                           -- 機器名称
        importance_structure_id,                                -- 重要度
        -- 添付情報の更新日時
        (
            SELECT
                MAX(att.update_datetime)                        -- 更新日時
            FROM
                mc_equipment AS eq                              -- 機器情報
                LEFT OUTER JOIN
                    attachment AS att                           -- 添付情報
                ON  (
                        eq.equipment_id = att.key_id            -- 機器ID
                    AND att.function_type_id = 1640
                    )
            WHERE
                eq.machine_id = machine.machine_id              -- 機番ID
        ) AS attachment_update_datetime
    FROM
        mc_machine AS machine                                   -- 機番情報
),
-- 機器別管理基準部位
man_com AS(
    SELECT
        management_standards_component_id,                      -- 機器別管理基準部位ID
        update_serialid AS update_serialid_component,           -- 更新シリアルID
        inspection_site_structure_id                            -- 部位ID
    FROM
        mc_management_standards_component                       -- 機器別管理基準部位
),
-- 機器別管理基準内容
man_con AS(
    SELECT
        con.management_standards_content_id,                    -- 機器別管理基準内容ID
        con.update_serialid AS update_serialid_content,         -- 更新シリアルID
        con.inspection_content_structure_id,                    -- 点検内容ID
        con.budget_amount,                                      -- 予算金額
        con.maintainance_kind_structure_id,                     -- 点検種別
--        item_ex.extension_data AS kind_order,
        con.long_plan_id,                                       -- 長計件名ID
        con.schedule_type_structure_id                          -- スケジュール管理基準ID
    FROM
        mc_management_standards_content AS con                  -- 機器別管理基準内容
--        LEFT OUTER JOIN
--            v_structure_item AS item
--        ON  (
--                item.structure_group_id = 1240
--            AND item.structure_id = con.maintainance_kind_structure_id
--            AND item.factory_id IN @FactoryIdList
--            AND item.language_id = @LanguageId
--            )
--        LEFT OUTER JOIN
--            ms_item_extension AS item_ex
--        ON  (
--                item_ex.item_id = item.structure_item_id
--            AND item_ex.sequence_no = 1
--            )
/*
 * 件名別長期計画一覧　参照画面　保全情報一覧の検索　保全項目
*/
-- WITH句の続き
),
-- 保全スケジュール
schedule AS(
    SELECT
        schedule.maintainance_schedule_id,                      -- 保全スケジュールID
        -- 機器別管理基準内容ID
        schedule.management_standards_content_id AS management_standards_content_id_schedule,
        schedule.start_date,                                    -- 開始日
        schedule.cycle_year,                                    -- 周期(年)
        schedule.cycle_month,                                   -- 周期(月)
        schedule.cycle_day,                                     -- 周期(日)
        schedule.disp_cycle,                                    -- 表示周期
        schedule_detail_sum.schedule_date                       -- スケジュール日
    FROM
        (
            -- 機器別管理基準内容IDごとに最大の開始日時をもつものを取得
            SELECT
                main.maintainance_schedule_id,                  -- 保全スケジュールID
                main.management_standards_content_id,           -- 機器別管理基準内容ID
                main.start_date,                                -- 開始日
                main.cycle_year,                                -- 周期(年)
                main.cycle_month,                               -- 周期(月)
                main.cycle_day,                                 -- 周期(日)
                main.disp_cycle                                 -- 表示周期
            FROM
                mc_maintainance_schedule AS main                -- 保全スケジュール
            WHERE
                NOT EXISTS(
                    SELECT
                        *
                    FROM
                        mc_maintainance_schedule AS sub         -- 保全スケジュール
                    WHERE
                        -- 機器別管理基準内容ID
                        main.management_standards_content_id = sub.management_standards_content_id
                    AND main.start_date < sub.start_date        -- 開始日
                )
        ) AS schedule                                           -- スケジュール
        -- スケジュール詳細から保全スケジュールIDごとに最小のスケジュール日を取得
        LEFT OUTER JOIN
            (
                SELECT
                    schedule_detail.maintainance_schedule_id,   -- 保全スケジュールID
                    -- スケジュール日
                    MIN(schedule_detail.schedule_date) AS schedule_date
                FROM
                    -- 保全スケジュール詳細
                    mc_maintainance_schedule_detail AS schedule_detail
                WHERE
                    complition = 0                              -- 完了フラグ
                AND EXISTS(
                        SELECT
                            *
                        FROM
                            ma_summary AS summary               -- 保全活動件名
                        INNER JOIN
                            #temp temp                          -- 一時テーブル（画面にて選択）
                        ON  (
                            summary.long_plan_id = temp.Key1    -- 長計件名ID
                            )
                        WHERE
                            -- 保全活動件名ID
                            schedule_detail.summary_id = summary.summary_id
                    )
                GROUP BY
                    schedule_detail.maintainance_schedule_id    -- 保全スケジュールID
            ) AS schedule_detail_sum                            -- 保全スケジュール詳細グループ化
        ON  (
                -- 保全スケジュールID
                schedule.maintainance_schedule_id = schedule_detail_sum.maintainance_schedule_id
            )
),
-- スケジュール確定排他チェック用更新日時
schedule_updtime AS(
    SELECT
        sc_h.management_standards_content_id,                   -- 機器別管理基準内容ID
        MAX(sc_h.update_datetime) AS schedule_head_updtime,     -- 更新日時
        MAX(sc_d.update_datetime) AS schedule_detail_updtime    -- 更新日時
    FROM
        mc_maintainance_schedule AS sc_h                        -- 保全スケジュール
        LEFT OUTER JOIN
            mc_maintainance_schedule_detail AS sc_d             -- 保全スケジュール詳細
        ON  (
                -- 保全スケジュールID
                sc_h.maintainance_schedule_id = sc_d.maintainance_schedule_id
            )
    WHERE
        sc_d.schedule_date IS NOT NULL                          -- スケジュール日
    AND sc_d.complition != 1                                    -- 完了フラグ
    AND sc_d.schedule_date                                      -- スケジュール日
        BETWEEN @ScheduleStart AND @ScheduleEnd
    GROUP BY
        sc_h.management_standards_content_id                    -- 機器別管理基準内容ID
)
/*
 * 件名別長期計画一覧　参照画面　保全情報のスケジュール一覧の検索　保全項目
*/
-- WITH句(GetDetailList_With)の続き

-- GetDetailListのSQLと同じキーの単位で取得する
SELECT
    CONCAT_WS('|',man_con.long_plan_id, machine.machine_id, man_com.management_standards_component_id,man_con.management_standards_content_id) AS key_id,
    -- 機器、点検種別でグループとなる
    DENSE_RANK() OVER(ORDER BY man_con.long_plan_id, machine.machine_id) AS group_key,
    machine.machine_id,
    man_com.management_standards_component_id,
    man_con.management_standards_content_id,
    msd.schedule_date,
    msd.complition,
    mscn.maintainance_kind_structure_id,						-- 点検種別
--    ie.extension_data AS maintainance_kind_level,				-- 点検種別レベル
--    st.translation_text AS maintainance_kind_char,
	--map.total_budget_cost AS budget_amount,						-- 全体予算金額
	man_con.budget_amount AS budget_amount,						-- 全体予算金額
	mah.expenditure AS expenditure,								-- 実績金額
    '' AS maintainance_kind_char,
    msd.summary_id
    , 
    /* スケジュールマークグループ用 */
    CONCAT_WS('|', machine.machine_id, ie.extension_data) AS same_mark_key 
FROM
    base                                                        -- 基本テーブル
    INNER JOIN
        machine                                                 -- 機番情報
    ON  (
            machine.machine_id = base.machine_id                -- 機番ID
        )
    INNER JOIN
        man_com                                                 -- 機器別管理基準部位
    ON  (
            -- 機器別管理基準部位ID
            man_com.management_standards_component_id = base.management_standards_component_id
        )
    INNER JOIN
        man_con                                                 -- 機器別管理基準内容
    ON  (
            -- 機器別管理基準内容ID
            man_con.management_standards_content_id = base.management_standards_content_id
        )
    INNER JOIN
        mc_management_standards_content AS mscn
    ON  (
            man_con.management_standards_content_id = mscn.management_standards_content_id
        )
    LEFT OUTER JOIN
        mc_maintainance_schedule AS msh
    ON  (
            msh.management_standards_content_id = mscn.management_standards_content_id
        )
    LEFT OUTER JOIN
        mc_maintainance_schedule_detail AS msd
    ON  (
            msd.maintainance_schedule_id = msh.maintainance_schedule_id
        )
    --LEFT OUTER JOIN
    --    ma_plan AS map                                           -- 保全計画
    --ON  (
    --        map.summary_id = msd.summary_id
    --    )
    LEFT OUTER JOIN
        ma_history AS mah                                        -- 保全履歴
    ON  (
            mah.summary_id = msd.summary_id
        )
--     LEFT OUTER JOIN
--         v_structure_item AS st
--     ON  (
--             mscn.maintainance_kind_structure_id = st.structure_id
--         AND st.structure_group_id = 1240
--         AND st.factory_id IN @FactoryIdList
--         AND st.language_id = @LanguageId
--         )
    LEFT OUTER JOIN
		ms_structure st
	ON  (
			mscn.maintainance_kind_structure_id = st.structure_id
		AND st.structure_group_id = 1240
        )
    LEFT OUTER JOIN
        ms_item_extension AS ie
    ON  (
            st.structure_item_id = ie.item_id
        AND ie.sequence_no = 1
        )
WHERE
    msd.schedule_date IS NOT NULL
AND msd.schedule_date BETWEEN @ScheduleStart AND @ScheduleEnd
