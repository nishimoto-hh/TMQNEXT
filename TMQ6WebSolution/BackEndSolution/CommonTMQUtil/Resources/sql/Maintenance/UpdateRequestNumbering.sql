UPDATE ma_request_numbering 
SET
    [seq_no] = seq_no + 1                       -- 連番
    , [update_datetime] = @UpdateDatetime
    , [update_user_id] = @UpdateUserId 
OUTPUT inserted.seq_no
WHERE
    [numbering_id] = @NumberingId              -- 採番id
