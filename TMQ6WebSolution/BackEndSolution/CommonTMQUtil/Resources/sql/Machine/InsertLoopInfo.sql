INSERT 
INTO mc_loop_info( 
     loop_id                        -- ループ構成ID
    ,loop_moto_id                   -- ループ構成元ID
    ,machine_id                     -- 機番ID
    ,update_serialid                -- 更新シリアルID
    ,insert_datetime                -- 登録日時
    ,insert_user_id                 -- 登録ユーザー
    ,update_datetime                -- 更新日時
    ,update_user_id                 -- 更新ユーザー
) 
OUTPUT inserted.loop_id
VALUES ( 
    NEXT VALUE FOR seq_mc_loop_info_loop_id  -- ループID
    , @LoopMotoId                            -- ループ元ID
    , @MachineId                             -- 機番ID
    , 0                                      -- 更新シリアルID
    , @InsertDatetime                        -- 登録日時
    , @InsertUserId                          -- 登録ユーザー
    , @UpdateDatetime                        -- 更新日時
    , @UpdateUserId                          -- 更新ユーザー
); 