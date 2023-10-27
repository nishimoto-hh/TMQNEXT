INSERT INTO ma_request_numbering(
    [numbering_id]                              -- 採番id
    , [numbering_pattern]                       -- 採番パターン
    , [year]                                    -- 年
    , [location_structure_id]                   -- 場所階層id
    , [seq_no]                                  -- 連番
    , [insert_datetime]                         -- 登録日時
    , [insert_user_id]                          -- 登録ユーザー
    , [update_datetime]                         -- 更新日時
    , [update_user_id]                          -- 更新ユーザー
) 
OUTPUT
    inserted.seq_no 
VALUES(
    NEXT VALUE FOR seq_ma_request_numbering_numbering_id  -- 採番id
    , @NumberingPattern                         -- 採番パターン
    , @Year                                     -- 年
    , @LocationStructureId                      -- 場所階層id
    , @SeqNo                                    -- 連番
    , @InsertDatetime                           -- 登録日時
    , @InsertUserId                             -- 登録ユーザー
    , @UpdateDatetime                           -- 更新日時
    , @UpdateUserId                             -- 更新ユーザー
)
