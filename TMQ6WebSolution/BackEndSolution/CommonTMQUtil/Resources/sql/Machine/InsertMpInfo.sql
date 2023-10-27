INSERT 
INTO mc_mp_information( 
     mp_information_id              -- MP情報ID
    ,machine_id                     -- 機番ID
	,mp_information                 -- MP情報
    ,update_serialid                -- 更新シリアルID
    ,insert_datetime                -- 登録日時
    ,insert_user_id                 -- 登録ユーザー
    ,update_datetime                -- 更新日時
    ,update_user_id                 -- 更新ユーザー
) 
VALUES ( 
    NEXT VALUE FOR seq_mc_mp_information_mp_information_id  -- MP情報ID
    , @MachineId                     -- 機番ID
    , @MpInformation                 -- MP情報
    , 0                              -- 更新シリアルID
    , @InsertDatetime                -- 登録日時
    , @InsertUserId                  -- 登録ユーザー
    , @UpdateDatetime                -- 更新日時
    , @UpdateUserId                  -- 更新ユーザー
); 