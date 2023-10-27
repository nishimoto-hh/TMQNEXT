INSERT 
INTO mc_applicable_laws( 
     applicable_laws_id                      -- 適用法規ID
    ,applicable_laws_structure_id            -- 適用法規アイテムID
    ,machine_id                              -- 機番ID
    ,update_serialid                         -- 更新シリアルID
    ,insert_datetime                         -- 登録日時
    ,insert_user_id                          -- 登録ユーザー
    ,update_datetime                         -- 更新日時
    ,update_user_id                          -- 更新ユーザー
) 
OUTPUT inserted.applicable_laws_id
VALUES ( 
    NEXT VALUE FOR seq_mc_applicable_laws_applicable_laws_id          -- 適用法規ID
    , @ApplicableLawsStructureId             -- 適用法規アイテムID
    , @MachineId                             -- 機番ID
    , 0                                      -- 更新シリアルID
    , @InsertDatetime                        -- 登録日時
    , @InsertUserId                          -- 登録ユーザー
    , @UpdateDatetime                        -- 更新日時
    , @UpdateUserId                          -- 更新ユーザー
); 