INSERT 
INTO mc_machine_use_parts( 
     machine_use_parts_id           -- 機番使用部品ID
    ,machine_id                     -- 機番ID
    ,parts_id                       -- 予備品ID
	,use_quantity                   -- 使用個数
    ,update_serialid                -- 更新シリアルID
    ,insert_datetime                -- 登録日時
    ,insert_user_id                 -- 登録ユーザー
    ,update_datetime                -- 更新日時
    ,update_user_id                 -- 更新ユーザー
) 
OUTPUT inserted.machine_use_parts_id
VALUES ( 
    NEXT VALUE FOR seq_mc_machine_use_parts_machine_use_parts_id  -- 機番使用部品ID
    , @MachineId                     -- 機番ID
    , @PartsId                       -- 予備品ID
	, @UseQuantity                   -- 使用個数
    , 0                              -- 更新シリアルID
    , @InsertDatetime                -- 登録日時
    , @InsertUserId                  -- 登録ユーザー
    , @UpdateDatetime                -- 更新日時
    , @UpdateUserId                  -- 更新ユーザー
); 