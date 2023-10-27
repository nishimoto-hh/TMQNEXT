INSERT 
INTO hm_mc_applicable_laws( 
    [hm_applicable_laws_id]               -- 適用法規変更管理ID
    , [history_management_detail_id]      -- 変更管理詳細ID
    , [applicable_laws_id]                -- 適用法規ID
    , [applicable_laws_structure_id]      -- 適用法規アイテムID
    , [machine_id]                        -- 機番ID
    , [update_serialid]                   -- 更新シリアルID
    , [insert_datetime]                   -- 登録日時
    , [insert_user_id]                    -- 登録ユーザー
    , [update_datetime]                   -- 更新日時
    , [update_user_id]                    -- 更新ユーザー
) 
VALUES ( 
    NEXT VALUE FOR seq_hm_mc_applicable_laws_hm_applicable_laws_id -- 適用法規変更管理ID
    , @HistoryManagementDetailId                                   -- 変更管理詳細ID
    , @ApplicableLawsId                                            -- 適用法規ID
    , @ApplicableLawsStructureId                                   -- 適用法規アイテムID
    , @MachineId                                                   -- 機番ID
    , 0                                                            -- 更新シリアルID
    , @InsertDatetime                                              -- 登録日時
    , @InsertUserId                                                -- 登録ユーザー
    , @UpdateDatetime                                              -- 更新日時
    , @UpdateUserId                                                -- 更新ユーザー
); 
