SELECT
    [numbering_id]                              -- 採番id
    , [numbering_pattern]                       -- 採番パターン
    , [year]                                    -- 年
    , [location_structure_id]                   -- 場所階層id
    , [seq_no]                                  -- 連番
    , [insert_datetime]                         -- 登録日時
    , [insert_user_id]                          -- 登録ユーザー
    , [update_datetime]                         -- 更新日時
    , [update_user_id]                          -- 更新ユーザー
FROM
    ma_request_numbering WITH(TABLOCKX)
WHERE
    [numbering_pattern] = @NumberingPattern     -- 採番パターン
AND [year] = @Year                              -- 年
AND [location_structure_id] = @LocationStructureId  -- 場所階層id
