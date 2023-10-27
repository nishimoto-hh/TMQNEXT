INSERT 
INTO mc_accessory_info( 
     accessory_id                   -- ループ構成ID
    ,accessory_moto_id              -- ループ構成元ID
    ,machine_id                     -- 機番ID
    ,update_serialid                -- 更新シリアルID
    ,insert_datetime                -- 登録日時
    ,insert_user_id                 -- 登録ユーザー
    ,update_datetime                -- 更新日時
    ,update_user_id                 -- 更新ユーザー
) 
OUTPUT inserted.accessory_id
VALUES ( 
    NEXT VALUE FOR seq_mc_accessory_info_accessory_id  -- 付属品構成ID
    , @AccessoryMotoId                        -- 付属品構成元ID
    , @MachineId                             -- 機番ID
    , 0                                      -- 更新シリアルID
    , @InsertDatetime                        -- 登録日時
    , @InsertUserId                          -- 登録ユーザー
    , @UpdateDatetime                        -- 更新日時
    , @UpdateUserId                          -- 更新ユーザー
); 