INSERT 
INTO hm_history_management( 
    [history_management_id]     -- 変更管理ID
    , [application_status_id]   -- 申請状況ID
    , [application_division_id] -- 申請区分ID
    , [application_conduct_id]  -- 申請機能ID
    , [application_user_id]     -- 申請者ID
    , [application_user_name]   -- 申請者名称
    , [update_serialid]         -- 更新シリアルID
    , [insert_datetime]         -- 登録日時
    , [insert_user_id]          -- 登録ユーザー
    , [update_datetime]         -- 更新日時
    , [update_user_id]          -- 更新ユーザー
) 
OUTPUT inserted.history_management_id
VALUES ( 
    NEXT VALUE FOR seq_hm_history_management_history_management_id                   -- 変更管理ID
    , @ApplicationStatusId                                                           -- 申請状況ID
    , @ApplicationDivisionId                                                         -- 申請区分ID
    , @ApplicationConductId                                                          -- 申請機能ID
    , @ApplicationUserId                                                             -- 申請者ID
    , (SELECT mu.display_name FROM ms_user mu WHERE mu.user_id = @ApplicationUserId) -- 申請者名称(ログインユーザIDより名称を取得)
    , 0                                                                              -- 更新シリアルID
    , @InsertDatetime                                                                -- 登録日時
    , @InsertUserId                                                                  -- 登録ユーザー
    , @UpdateDatetime                                                                -- 更新日時
    , @UpdateUserId                                                                  -- 更新ユーザー
); 
