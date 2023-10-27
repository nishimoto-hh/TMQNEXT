INSERT 
INTO hm_ln_long_plan( 
    [hm_long_plan_id]                           -- 長計件名変更管理id
    , [history_management_id]                   -- 変更管理id
    , [long_plan_id]                            -- 長期計画件名id
    , [execution_division]                      -- 処理区分
    , [subject]                                 -- 件名
    , [location_structure_id]                   -- 機能場所階層id
    , [job_structure_id]                        -- 職種機種階層id
    , [subject_note]                            -- 件名メモ
    , [person_id]                               -- 担当者
    , [person_name]                             -- 担当者名
    , [work_item_structure_id]                  -- 作業項目
    , [budget_management_structure_id]          -- 予算管理区分
    , [budget_personality_structure_id]         -- 予算性格区分
    , [maintenance_season_structure_id]         -- 保全時期
    , [purpose_structure_id]                    -- 目的区分
    , [work_class_structure_id]                 -- 作業区分
    , [treatment_structure_id]                  -- 処置区分
    , [facility_structure_id]                   -- 設備区分
    , [update_serialid]                         -- 更新シリアルID
    , [insert_datetime]                         -- 登録日時
    , [insert_user_id]                          -- 登録ユーザー
    , [update_datetime]                         -- 更新日時
    , [update_user_id]                          -- 更新ユーザー
) 
VALUES ( 
    NEXT VALUE FOR seq_hm_ln_long_plan_hm_long_plan_id -- 長計件名変更管理id
    , @HistoryManagementId                      -- 変更管理id
    , @LongPlanId                               -- 長期計画件名id
    , @ExecutionDivision                        -- 処理区分
    , @Subject                                  -- 件名
    , @LocationStructureId                      -- 機能場所階層id
    , @JobStructureId                           -- 職種機種階層id
    , @SubjectNote                              -- 件名メモ
    , @PersonId                                 -- 担当者
    /*@New
    -- 新規登録・複写
    , (SELECT display_name FROM ms_user WHERE user_id = @PersonId)
    @New*/
    /*@DeleteRequest
    , @PersonName
    @DeleteRequest*/
    , @WorkItemStructureId                      -- 作業項目
    , @BudgetManagementStructureId              -- 予算管理区分
    , @BudgetPersonalityStructureId             -- 予算性格区分
    , @MaintenanceSeasonStructureId             -- 保全時期
    , @PurposeStructureId                       -- 目的区分
    , @WorkClassStructureId                     -- 作業区分
    , @TreatmentStructureId                     -- 処置区分
    , @FacilityStructureId                      -- 設備区分
    , @UpdateSerialid                           -- 更新シリアルID
    , @InsertDatetime                           -- 登録日時
    , @InsertUserId                             -- 登録ユーザー
    , @UpdateDatetime                           -- 更新日時
    , @UpdateUserId                             -- 更新ユーザー
)
