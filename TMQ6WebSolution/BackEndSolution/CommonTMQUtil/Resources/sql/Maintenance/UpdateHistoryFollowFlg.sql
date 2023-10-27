UPDATE ma_history 
SET
    [follow_flg] = @FollowFlg
    , [update_serialid] = update_serialid + 1
    , [update_datetime] = @UpdateDatetime
    , [update_user_id] = @UpdateUserId 
WHERE
    [history_id] = @HistoryId
