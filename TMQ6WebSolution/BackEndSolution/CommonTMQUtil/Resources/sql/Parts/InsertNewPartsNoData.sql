INSERT INTO pt_partsno_numbering(
    [numbering_id]                              -- 採番ID
    , [district_id]                             -- 地区ID
    , [seq_no]                                  -- 連番
    , [insert_datetime]                         -- 登録日時
    , [insert_user_id]                          -- 登録ユーザー
    , [update_datetime]                         -- 更新日時
    , [update_user_id]                          -- 更新ユーザー
) 
OUTPUT
    inserted.seq_no 
VALUES(
    NEXT VALUE FOR seq_pt_partsno_numbering_numbering_id  -- 採番ID
    , @DistrictId                               -- 地区ID
    , @SeqNo                                    -- 連番
    , @InsertDatetime                           -- 登録日時
    , @InsertUserId                             -- 登録ユーザー
    , @UpdateDatetime                           -- 更新日時
    , @UpdateUserId                             -- 更新ユーザー
)
