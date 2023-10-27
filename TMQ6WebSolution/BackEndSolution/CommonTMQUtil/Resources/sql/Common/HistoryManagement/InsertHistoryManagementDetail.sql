INSERT 
INTO hm_history_management_detail( 
    [history_management_detail_id] -- 変更管理詳細ID
    , [history_management_id]      -- 変更管理ID
    , [execution_division]         -- 実行区分ID
    , [update_serialid]            -- 更新シリアルID
    , [insert_datetime]            -- 登録日時
    , [insert_user_id]             -- 登録ユーザー
    , [update_datetime]            -- 更新日時
    , [update_user_id]             -- 更新ユーザー
) 
OUTPUT inserted.history_management_id
VALUES ( 
    NEXT VALUE FOR seq_hm_history_management_detail_history_management_detail_id -- 変更管理詳細ID
    , @HistoryManagementId                                                       -- 変更管理ID
    , @ExecutionDivision                                                         -- 実行処理区分
    , 0                                                                          -- 更新シリアルID
    , @InsertDatetime                                                            -- 登録日時
    , @InsertUserId                                                              -- 登録ユーザー
    , @UpdateDatetime                                                            -- 更新日時
    , @UpdateUserId                                                              -- 更新ユーザー
); 
