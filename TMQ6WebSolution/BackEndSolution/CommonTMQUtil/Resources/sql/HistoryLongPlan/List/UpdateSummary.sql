/*
 * 承認時、削除申請の長期計画IDに紐づく保全活動件名を更新するSQL
 */
UPDATE ma_summary 
SET
    [long_plan_id] = NULL
    , [update_serialid] = update_serialid + 1
    , [update_datetime] = @UpdateDatetime
    , [update_user_id] = @UpdateUserId 
WHERE
    [long_plan_id] = @LongPlanId

