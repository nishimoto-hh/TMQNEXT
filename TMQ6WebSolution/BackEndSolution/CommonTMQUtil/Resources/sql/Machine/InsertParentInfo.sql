INSERT 
INTO mc_machine_parent_info( 
     parent_id                      -- 親子構成ID
    ,parent_moto_id                 -- 親子構成元ID
    ,machine_id                     -- 機番ID
    ,update_serialid                -- 更新シリアルID
    ,insert_datetime                -- 登録日時
    ,insert_user_id                 -- 登録ユーザー
    ,update_datetime                -- 更新日時
    ,update_user_id                 -- 更新ユーザー
) 
OUTPUT inserted.parent_id
VALUES ( 
    NEXT VALUE FOR seq_mc_machine_parent_info_parent_id          -- 親子構成ID
    , @ParentMotoId                          -- 親子構成元アイテムID
    , @MachineId                             -- 機番ID
    , 0                                      -- 更新シリアルID
    , @InsertDatetime                        -- 登録日時
    , @InsertUserId                          -- 登録ユーザー
    , @UpdateDatetime                        -- 更新日時
    , @UpdateUserId                          -- 更新ユーザー
); 