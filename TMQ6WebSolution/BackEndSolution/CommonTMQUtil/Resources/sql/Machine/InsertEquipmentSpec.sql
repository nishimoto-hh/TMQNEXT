INSERT 
INTO mc_equipment_spec( 
    [equipment_spec_id]                         -- 機種別仕様id
    , [equipment_id]                            -- 機器id
    , [spec_id]                                 -- 仕様項目id
    , [spec_value]                              -- 設定値(テキスト)
    , [spec_structure_id]                       -- 設定値(選択)
    , [spec_num]                                -- 設定値(数値)
    , [spec_num_min]                            -- 設定値(数値(範囲))最小値
    , [spec_num_max]                            -- 設定値(数値(範囲))最大値
    , [update_serialid]                         -- 更新シリアルID
    , [insert_datetime]                         -- 登録日時
    , [insert_user_id]                          -- 登録ユーザー
    , [update_datetime]                         -- 更新日時
    , [update_user_id]                          -- 更新ユーザー
) 
VALUES ( 
    NEXT VALUE FOR seq_mc_equipment_spec_equipment_spec_id     -- 機種別仕様id
    , @EquipmentId                              -- 機器id
    , @SpecId                                   -- 仕様項目id
    , @SpecValue                                -- 設定値(テキスト)
    , @SpecStructureId                          -- 設定値(選択)
    , @SpecNum                                  -- 設定値(数値)
    , @SpecNumMin                               -- 設定値(数値(範囲))最小値
    , @SpecNumMax                               -- 設定値(数値(範囲))最大値
    , @UpdateSerialid                           -- 更新シリアルID
    , @InsertDatetime                           -- 登録日時
    , @InsertUserId                             -- 登録ユーザー
    , @UpdateDatetime                           -- 更新日時
    , @UpdateUserId                             -- 更新ユーザー
)
