/*
* 一覧の項目の内容を取得するSQL
*/
WITH org_eq_att AS(
     -- 機器情報添付の取得に使用する機器IDを機器情報より取得(トランザクションテーブル)
    SELECT DISTINCT
         mscn.long_plan_id
        ,mc.machine_id
        ,eq.equipment_id
        ,mscn.management_standards_content_id
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
hm_eq_att AS(
     -- 機器情報添付の取得に使用する機器IDを機器情報より取得(変更管理テーブル)
    SELECT DISTINCT
         mscn.long_plan_id
        ,mc.machine_id
        ,eq.equipment_id
        ,mscn.management_standards_content_id
        ,mscn.execution_division
    FROM
        hm_mc_management_standards_content AS mscn
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
eq_att AS ( 
    -- 機器情報添付の取得に使用する機器IDを機器情報より取得
    SELECT DISTINCT
        long_plan_id
        , machine_id
        , equipment_id 
    FROM
        org_eq_att org 
    WHERE
        NOT EXISTS ( 
            SELECT
                * 
            FROM
                hm_eq_att hm 
            WHERE
                hm.execution_division = 5 --保全情報一覧の削除
                AND hm.management_standards_content_id = org.management_standards_content_id
        ) --削除した保全情報一覧の情報は除外する
        
        UNION 
        
    SELECT DISTINCT
        long_plan_id
        , machine_id
        , equipment_id 
    FROM
        hm_eq_att hm 
    WHERE
        hm.execution_division != 5 --削除した保全情報一覧の情報は除外する
)
,
-- 排他処理で使用する項目(長期計画件名IDごとの最大の更新日時)(トランザクションテーブル)
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
        structure_group_id IN(1330, 1280, 1300, 1060, 1290, 1410, 1310, 1320, 2090, 2100)
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
,org_prepare_target AS(
 -- 準備対象列取得用、機器別管理基準内でスケジュールが未完了かつ準備期間に入っているものを取得(トランザクションテーブル)
    SELECT
        lp.long_plan_id,
        msd.schedule_date,
        COALESCE(preparation_period, 0) AS preparation_period,
        msc.management_standards_content_id
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
,hm_prepare_target AS(
 -- 準備対象列取得用、機器別管理基準内でスケジュールが未完了かつ準備期間に入っているものを取得(変更管理テーブル)
    SELECT
        lp.long_plan_id,
        msd.schedule_date,
        COALESCE(preparation_period, 0) AS preparation_period,
        msc.management_standards_content_id,
        msc.execution_division
    FROM
        ln_long_plan lp
        INNER JOIN
            hm_mc_management_standards_content msc
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
,prepare_target AS ( 
    -- 準備対象列取得用、機器別管理基準内でスケジュールが未完了かつ準備期間に入っているものを取得
    SELECT
        *
        , row_number() over(partition BY long_plan_id ORDER BY schedule_date) AS row_num
    FROM
        ( 
            SELECT
                long_plan_id
                , schedule_date
                , preparation_period 
            FROM
                org_prepare_target org 
            WHERE
                NOT EXISTS ( 
                    SELECT
                        * 
                    FROM
                        hm_prepare_target hm 
                    WHERE
                        hm.execution_division = 5 --保全情報一覧の削除
                        AND hm.management_standards_content_id = org.management_standards_content_id
                )                               --削除した保全情報一覧の情報は除外する
                UNION 
            SELECT
                long_plan_id
                , schedule_date
                , preparation_period 
            FROM
                hm_prepare_target hm
            WHERE
                hm.execution_division != 5     --削除した保全情報一覧の情報は除外する
        ) union_data
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
, factory_approval_user AS(
-- 工場の承認ユーザID
    SELECT
        ms.structure_id
        ,ex.extension_data AS ex_data
    FROM
        ms_structure ms 
        LEFT JOIN ms_item_extension ex 
            ON ms.structure_item_id = ex.item_id 
            AND ex.sequence_no = 4 
    WHERE
        ms.structure_group_id = 1000 
        AND ms.structure_layer_no = 1 
)
,target AS(
     -- 表示情報を取得するSQL、翻訳対応のためWITH句へ
    SELECT
        history.key_id AS long_plan_id
        , history.factory_id
        , COALESCE(hmlp.subject, lp.subject) AS subject
        , CASE 
            WHEN division_ex.extension_data = '20' 
                THEN hmlp.subject_note 
            ELSE COALESCE(hmlp.subject_note, lp.subject_note) 
            END AS subject_note
        , COALESCE( 
            hmlp.location_structure_id
            , lp.location_structure_id
        ) AS location_structure_id              --場所階層(変更管理テーブル)
        , COALESCE(hmlp.job_structure_id, lp.job_structure_id) AS job_structure_id --職種機種(変更管理テーブル)
        , lp.location_structure_id AS old_location_structure_id --場所階層(トランザクションテーブル)
        , lp.job_structure_id AS old_job_structure_id --職種機種(トランザクションテーブル)
        , CASE 
            WHEN division_ex.extension_data = '20' 
                THEN hmlp.maintenance_season_structure_id 
            ELSE COALESCE( 
                hmlp.maintenance_season_structure_id
                , lp.maintenance_season_structure_id
            ) 
            END AS maintenance_season_structure_id
        , CASE 
            WHEN division_ex.extension_data = '20' 
                THEN hmlp.person_id 
            ELSE COALESCE(hmlp.person_id, lp.person_id) 
            END AS person_id
        , CASE 
            WHEN division_ex.extension_data = '20' 
                THEN COALESCE(hmperson.display_name, hmlp.person_name) 
            ELSE COALESCE( 
                COALESCE(hmperson.display_name, hmlp.person_name)
                , COALESCE(person.display_name, lp.person_name)
            ) 
            END AS person_name
        , CASE 
            WHEN division_ex.extension_data = '20' 
                THEN hmlp.work_item_structure_id 
            ELSE COALESCE( 
                hmlp.work_item_structure_id
                , lp.work_item_structure_id
            ) 
            END AS work_item_structure_id
        , CASE 
            WHEN division_ex.extension_data = '20' 
                THEN hmlp.budget_management_structure_id 
            ELSE COALESCE( 
                hmlp.budget_management_structure_id
                , lp.budget_management_structure_id
            ) 
            END AS budget_management_structure_id
        , CASE 
            WHEN division_ex.extension_data = '20' 
                THEN hmlp.budget_personality_structure_id 
            ELSE COALESCE( 
                hmlp.budget_personality_structure_id
                , lp.budget_personality_structure_id
            ) 
            END AS budget_personality_structure_id
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
                        eq_att.long_plan_id FOR xml PATH ('')
                )
        ) AS file_link_equip
        , dbo.get_file_download_info( 
            1640
            , COALESCE(hmlp.long_plan_id, lp.long_plan_id)
        ) AS file_link_subject
        , CASE 
            WHEN division_ex.extension_data = '20' 
                THEN hmlp.purpose_structure_id 
            ELSE COALESCE( 
                hmlp.purpose_structure_id
                , lp.purpose_structure_id
            ) 
            END AS purpose_structure_id
        , CASE 
            WHEN division_ex.extension_data = '20' 
                THEN hmlp.work_class_structure_id 
            ELSE COALESCE( 
                hmlp.work_class_structure_id
                , lp.work_class_structure_id
            ) 
            END AS work_class_structure_id
        , CASE 
            WHEN division_ex.extension_data = '20' 
                THEN hmlp.treatment_structure_id 
            ELSE COALESCE( 
                hmlp.treatment_structure_id
                , lp.treatment_structure_id
            ) 
            END AS treatment_structure_id
        , CASE 
            WHEN division_ex.extension_data = '20' 
                THEN hmlp.facility_structure_id 
            ELSE COALESCE( 
                hmlp.facility_structure_id
                , lp.facility_structure_id
            ) 
            END AS facility_structure_id
        ,
        -- 参照画面の排他処理で用いる項目
        max_dt.long_plan_id_dt
        ,max_dt.mc_man_st_con_update_datetime
        ,max_dt.sche_detail_update_datetime
        ,max_dt.attachment_update_datetime
        -- スケジュールと同じ値(長計件名ID)
        ,history.key_id AS key_id
        -- 準備対象列
        ,COALESCE((SELECT 1 FROM prepare_target_narrow AS pt WHERE pt.long_plan_id = history.key_id),0) AS preparation_flg
        ,CASE
            WHEN hmmsc.content_change_flg IS NOT NULL THEN 1
            ELSE 0
         END AS content_change_flg
        ,history.application_status_id
        ,history.application_division_id
        ,history.application_conduct_id
        ,history.application_user_name
        ,approval_user.display_name AS approval_user_name
        ,history.application_date
        ,history.application_reason
        ,history.rejection_reason
        ,division_ex.extension_data AS application_division_code
        ,status_ex.extension_data AS application_status_code
        ,history.history_management_id
        ,history.update_serialid
        ---------- 以下は値の変更があった項目(申請区分が「変更申請：20」のデータ)を取得 ----------
        ,CASE
            WHEN division_ex.extension_data = '20' THEN trim(
                '|'
                FROM
                    (
                       -- ①ファンクションを使用し、変更前後の値を比較する
                       -- ②変更前後の値に差異がある場合は引数に渡した[項目名 + 背景色設定値]を返す(変更が無い場合は空文字が返ってくる)
                       -- ③変更のあった項目をパイプ[|]区切りで連結させて、変更のあった項目 とする
                       -- ※項目名(MachineNoやMachineName)はJavaScriptの背景色設定処理で使用するので統一させる
                       dbo.compare_newVal_with_oldVal((CASE WHEN hmlp.hm_long_plan_id IS NOT NULL THEN hmlp.subject ELSE lp.subject END), lp.subject, 'Subject') + -- 件名
                       dbo.compare_newVal_with_oldVal((CASE WHEN hmlp.hm_long_plan_id IS NOT NULL THEN hmlp.subject_note ELSE lp.subject_note END), lp.subject_note, 'SubjectNote') + -- 件名メモ
                       dbo.compare_newId_with_oldId((CASE WHEN hmlp.hm_long_plan_id IS NOT NULL THEN hmlp.maintenance_season_structure_id ELSE lp.maintenance_season_structure_id END), lp.maintenance_season_structure_id, 'MaintenanceSeason') + -- 保全時期
                       dbo.compare_newVal_with_oldVal((CASE WHEN hmlp.hm_long_plan_id IS NOT NULL THEN COALESCE(hmperson.display_name, hmlp.person_name) ELSE COALESCE(person.display_name, lp.person_name) END), COALESCE(person.display_name, lp.person_name), 'PersonName') + -- 担当
                       dbo.compare_newId_with_oldId((CASE WHEN hmlp.hm_long_plan_id IS NOT NULL THEN hmlp.person_id ELSE lp.person_id END), lp.person_id, 'PersonCode') + -- 担当
                       dbo.compare_newId_with_oldId((CASE WHEN hmlp.hm_long_plan_id IS NOT NULL THEN hmlp.work_item_structure_id ELSE lp.work_item_structure_id END), lp.work_item_structure_id, 'WorkItem') + -- 作業項目
                       dbo.compare_newId_with_oldId((CASE WHEN hmlp.hm_long_plan_id IS NOT NULL THEN hmlp.budget_management_structure_id ELSE lp.budget_management_structure_id END), lp.budget_management_structure_id, 'BudgetManagement') + -- 予算管理区分
                       dbo.compare_newId_with_oldId((CASE WHEN hmlp.hm_long_plan_id IS NOT NULL THEN hmlp.budget_personality_structure_id ELSE lp.budget_personality_structure_id END), lp.budget_personality_structure_id, 'BudgetPersonality') + -- 予算性格区分
                       dbo.compare_newId_with_oldId((CASE WHEN hmlp.hm_long_plan_id IS NOT NULL THEN hmlp.purpose_structure_id ELSE lp.purpose_structure_id END), lp.purpose_structure_id, 'Purpose') + -- 目的区分
                       dbo.compare_newId_with_oldId((CASE WHEN hmlp.hm_long_plan_id IS NOT NULL THEN hmlp.work_class_structure_id ELSE lp.work_class_structure_id END), lp.work_class_structure_id, 'WorkClass') + -- 作業区分
                       dbo.compare_newId_with_oldId((CASE WHEN hmlp.hm_long_plan_id IS NOT NULL THEN hmlp.treatment_structure_id ELSE lp.treatment_structure_id END), lp.treatment_structure_id, 'Treatment') + -- 処置区分
                       dbo.compare_newId_with_oldId((CASE WHEN hmlp.hm_long_plan_id IS NOT NULL THEN hmlp.facility_structure_id ELSE lp.facility_structure_id END), lp.facility_structure_id, 'FacilityDivision') + -- 設備区分
                       CASE
                          WHEN hmmsc.content_change_flg IS NOT NULL THEN 'Content_20|'
                          ELSE ''
                       END --保全情報変更有無
                    )
            )
            ELSE ''
        END AS value_changed
    FROM
        hm_history_management history -- 変更管理
        LEFT JOIN hm_ln_long_plan hmlp -- 長計件名変更管理
            ON history.key_id = hmlp.long_plan_id 
            AND history.history_management_id = hmlp.history_management_id 
        LEFT JOIN ( 
            SELECT
                hmmsc.history_management_id
                , count(hmmsc.hm_management_standards_content_id) AS content_change_flg --保全情報一覧の追加、削除が行われている場合、1以上
            FROM
                hm_mc_management_standards_content hmmsc
            WHERE
                hmmsc.execution_division IN (4, 5) --保全情報一覧の追加、削除
            GROUP BY
                hmmsc.history_management_id
        ) hmmsc -- 機器別管理基準内容変更管理の存在有無
            ON history.history_management_id = hmmsc.history_management_id 
        LEFT OUTER JOIN
            ms_user AS hmperson
        ON  (
                hmlp.person_id = hmperson.user_id
            )
        LEFT JOIN
            ln_long_plan AS lp
        ON  history.key_id = lp.long_plan_id
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
        LEFT JOIN
            ms_structure status_ms -- 構成マスタ(申請状況)
        ON  history.application_status_id = status_ms.structure_id
        LEFT JOIN
            ms_item_extension status_ex -- アイテムマスタ拡張(申請状況)
        ON  status_ms.structure_item_id = status_ex.item_id
        AND status_ex.sequence_no = 1
        LEFT JOIN
            ms_structure division_ms --構成マスタ(申請区分)
        ON  history.application_division_id = division_ms.structure_id
        LEFT JOIN
            ms_item_extension division_ex -- アイテムマスタ拡張(申請区分)
        ON  division_ms.structure_item_id = division_ex.item_id
        AND division_ex.sequence_no = 1
        LEFT JOIN
            factory_approval_user fau
        ON  history.factory_id = fau.structure_id
        LEFT JOIN
            ms_user approval_user
        ON  fau.ex_data = CAST(approval_user.user_id AS nvarchar)
    WHERE
        -- 「申請データ作成中」「承認依頼中」「差戻中」のデータのみ
        status_ex.extension_data IN('10', '20', '30')
        -- 「2：長期計画」のデータのみ
        AND history.application_conduct_id = 2
        
        /*@DispOnlyMySubject
        -- 自分の申請のみ表示
        AND history.application_user_id = @UserId
        @DispOnlyMySubject*/

        /*@IsDetail
        -- 詳細画面の場合、変更管理IDを指定
        AND history.history_management_id = @HistoryManagementId
        @IsDetail*/
)