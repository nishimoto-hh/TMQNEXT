/*
* 一覧の項目の内容を取得するSQL
*/
WITH target AS(
     -- 表示情報を取得するSQL、翻訳対応のためWITH句へ
    SELECT
        history.key_id AS long_plan_id
        , COALESCE(hmlp.subject, lp.subject) AS subject
        , CASE 
            WHEN division_ex.extension_data = '20' AND hmlp.hm_long_plan_id IS NOT NULL
                THEN hmlp.subject_note 
            ELSE COALESCE(hmlp.subject_note, lp.subject_note) 
            END AS subject_note
        , COALESCE( 
            hmlp.location_structure_id
            , lp.location_structure_id
        ) AS location_structure_id              --場所階層(変更管理テーブル)
        , CASE 
            WHEN division_ex.extension_data = '20' AND hmlp.hm_long_plan_id IS NOT NULL
                THEN hmlp.location_district_structure_id 
            ELSE COALESCE(hmlp.location_district_structure_id, lp.location_district_structure_id) 
            END AS district_id
        , CASE 
            WHEN division_ex.extension_data = '20' AND hmlp.hm_long_plan_id IS NOT NULL
                THEN hmlp.location_factory_structure_id 
            ELSE COALESCE(hmlp.location_factory_structure_id, lp.location_factory_structure_id) 
            END AS factory_id
        , CASE 
            WHEN division_ex.extension_data = '20' AND hmlp.hm_long_plan_id IS NOT NULL
                THEN hmlp.location_plant_structure_id 
            ELSE COALESCE(hmlp.location_plant_structure_id, lp.location_plant_structure_id) 
            END AS plant_id
        , CASE 
            WHEN division_ex.extension_data = '20' AND hmlp.hm_long_plan_id IS NOT NULL
                THEN hmlp.location_series_structure_id 
            ELSE COALESCE(hmlp.location_series_structure_id, lp.location_series_structure_id) 
            END AS series_id
        , CASE 
            WHEN division_ex.extension_data = '20' AND hmlp.hm_long_plan_id IS NOT NULL
                THEN hmlp.location_stroke_structure_id 
            ELSE COALESCE(hmlp.location_stroke_structure_id, lp.location_stroke_structure_id) 
            END AS stroke_id
        , CASE 
            WHEN division_ex.extension_data = '20' AND hmlp.hm_long_plan_id IS NOT NULL
                THEN hmlp.location_facility_structure_id 
            ELSE COALESCE(hmlp.location_facility_structure_id, lp.location_facility_structure_id) 
            END AS facility_id
        , COALESCE(hmlp.job_structure_id, lp.job_structure_id) AS job_structure_id --職種機種(変更管理テーブル)
        , CASE 
            WHEN division_ex.extension_data = '20' AND hmlp.hm_long_plan_id IS NOT NULL
                THEN hmlp.job_kind_structure_id 
            ELSE COALESCE(hmlp.job_kind_structure_id, lp.job_kind_structure_id) 
            END AS job_id
        , CASE 
            WHEN division_ex.extension_data = '20' AND hmlp.hm_long_plan_id IS NOT NULL
                THEN hmlp.job_large_classfication_structure_id 
            ELSE COALESCE(hmlp.job_large_classfication_structure_id, lp.job_large_classfication_structure_id) 
            END AS large_classfication_id
        , CASE 
            WHEN division_ex.extension_data = '20' AND hmlp.hm_long_plan_id IS NOT NULL
                THEN hmlp.job_middle_classfication_structure_id 
            ELSE COALESCE(hmlp.job_middle_classfication_structure_id, lp.job_middle_classfication_structure_id) 
            END AS middle_classfication_id
        , CASE 
            WHEN division_ex.extension_data = '20' AND hmlp.hm_long_plan_id IS NOT NULL
                THEN hmlp.job_small_classfication_structure_id 
            ELSE COALESCE(hmlp.job_small_classfication_structure_id, lp.job_small_classfication_structure_id) 
            END AS small_classfication_id
        , CASE 
            WHEN division_ex.extension_data = '20' AND hmlp.hm_long_plan_id IS NOT NULL
                THEN hmlp.maintenance_season_structure_id 
            ELSE COALESCE( 
                hmlp.maintenance_season_structure_id
                , lp.maintenance_season_structure_id
            ) 
            END AS maintenance_season_structure_id
        , CASE 
            WHEN division_ex.extension_data = '20' AND hmlp.hm_long_plan_id IS NOT NULL
                THEN hmlp.person_id 
            ELSE COALESCE(hmlp.person_id, lp.person_id) 
            END AS person_id
        , CASE 
            WHEN division_ex.extension_data = '20' AND hmlp.hm_long_plan_id IS NOT NULL
                THEN COALESCE(hmperson.display_name, hmlp.person_name) 
            ELSE COALESCE( 
                COALESCE(hmperson.display_name, hmlp.person_name)
                , COALESCE(person.display_name, lp.person_name)
            ) 
            END AS person_name
        , CASE 
            WHEN division_ex.extension_data = '20' AND hmlp.hm_long_plan_id IS NOT NULL
                THEN hmlp.work_item_structure_id 
            ELSE COALESCE( 
                hmlp.work_item_structure_id
                , lp.work_item_structure_id
            ) 
            END AS work_item_structure_id
        , CASE 
            WHEN division_ex.extension_data = '20' AND hmlp.hm_long_plan_id IS NOT NULL
                THEN hmlp.budget_management_structure_id 
            ELSE COALESCE( 
                hmlp.budget_management_structure_id
                , lp.budget_management_structure_id
            ) 
            END AS budget_management_structure_id
        , CASE 
            WHEN division_ex.extension_data = '20' AND hmlp.hm_long_plan_id IS NOT NULL
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
                        #eq_att AS eq_att 
                    WHERE
                        eq_att.long_plan_id = lp.long_plan_id 
                    ORDER BY
                        eq_att.long_plan_id FOR xml PATH ('')
                )
        ) AS file_link_equip
        ,REPLACE((
                SELECT
                    dbo.get_file_download_info_row(att_temp.file_name, att_temp.attachment_id, att_temp.function_type_id, att_temp.key_id, att_temp.extension_data)
                FROM
                    #temp_attachment as att_temp
                WHERE
                    (hmlp.long_plan_id = att_temp.key_id OR lp.long_plan_id = att_temp.key_id)
                AND att_temp.function_type_id = 1640
                ORDER BY
                    document_no FOR xml path('')
            ), ' ', '') AS file_link_subject
        , CASE 
            WHEN division_ex.extension_data = '20' AND hmlp.hm_long_plan_id IS NOT NULL
                THEN hmlp.purpose_structure_id 
            ELSE COALESCE( 
                hmlp.purpose_structure_id
                , lp.purpose_structure_id
            ) 
            END AS purpose_structure_id
        , CASE 
            WHEN division_ex.extension_data = '20' AND hmlp.hm_long_plan_id IS NOT NULL
                THEN hmlp.work_class_structure_id 
            ELSE COALESCE( 
                hmlp.work_class_structure_id
                , lp.work_class_structure_id
            ) 
            END AS work_class_structure_id
        , CASE 
            WHEN division_ex.extension_data = '20' AND hmlp.hm_long_plan_id IS NOT NULL
                THEN hmlp.treatment_structure_id 
            ELSE COALESCE( 
                hmlp.treatment_structure_id
                , lp.treatment_structure_id
            ) 
            END AS treatment_structure_id
        , CASE 
            WHEN division_ex.extension_data = '20' AND hmlp.hm_long_plan_id IS NOT NULL
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
        ,COALESCE((SELECT 1 FROM #prepare_target_narrow AS pt WHERE pt.long_plan_id = history.key_id),0) AS preparation_flg
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
                       dbo.compare_newId_with_oldId((CASE WHEN hmlp.hm_long_plan_id IS NOT NULL THEN hmlp.location_district_structure_id ELSE lp.location_district_structure_id END), lp.location_district_structure_id, 'District') +                                -- 地区
                       dbo.compare_newId_with_oldId((CASE WHEN hmlp.hm_long_plan_id IS NOT NULL THEN hmlp.location_factory_structure_id ELSE lp.location_factory_structure_id END), lp.location_factory_structure_id, 'Factory') +                                   -- 工場
                       dbo.compare_newId_with_oldId((CASE WHEN hmlp.hm_long_plan_id IS NOT NULL THEN hmlp.location_plant_structure_id ELSE lp.location_plant_structure_id END), lp.location_plant_structure_id, 'Plant') +                                         -- プラント
                       dbo.compare_newId_with_oldId((CASE WHEN hmlp.hm_long_plan_id IS NOT NULL THEN hmlp.location_series_structure_id ELSE lp.location_series_structure_id END), lp.location_series_structure_id, 'Series') +                                      -- 系列
                       dbo.compare_newId_with_oldId((CASE WHEN hmlp.hm_long_plan_id IS NOT NULL THEN hmlp.location_stroke_structure_id ELSE lp.location_stroke_structure_id END), lp.location_stroke_structure_id, 'Stroke') +                                      -- 工程
                       dbo.compare_newId_with_oldId((CASE WHEN hmlp.hm_long_plan_id IS NOT NULL THEN hmlp.location_facility_structure_id ELSE lp.location_facility_structure_id END), lp.location_facility_structure_id, 'Facility') +                                -- 設備
                       dbo.compare_newId_with_oldId((CASE WHEN hmlp.hm_long_plan_id IS NOT NULL THEN hmlp.job_kind_structure_id ELSE lp.job_kind_structure_id END), lp.job_kind_structure_id, 'Job') +                                                       -- 職種
                       dbo.compare_newId_with_oldId((CASE WHEN hmlp.hm_long_plan_id IS NOT NULL THEN hmlp.job_large_classfication_structure_id ELSE lp.job_large_classfication_structure_id END), lp.job_large_classfication_structure_id, 'Large') +                       -- 機種大分類
                       dbo.compare_newId_with_oldId((CASE WHEN hmlp.hm_long_plan_id IS NOT NULL THEN hmlp.job_middle_classfication_structure_id ELSE lp.job_middle_classfication_structure_id END), lp.job_middle_classfication_structure_id, 'Middle') +                    -- 機種中分類
                       dbo.compare_newId_with_oldId((CASE WHEN hmlp.hm_long_plan_id IS NOT NULL THEN hmlp.job_small_classfication_structure_id ELSE lp.job_small_classfication_structure_id END), lp.job_small_classfication_structure_id, 'Small') +                       -- 機種小分類
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
            #max_dt AS max_dt
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
            #factory_approval_user fau
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