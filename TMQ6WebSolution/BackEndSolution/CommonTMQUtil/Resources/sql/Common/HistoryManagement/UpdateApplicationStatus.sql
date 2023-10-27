UPDATE
    hm_history_management
SET
    [application_status_id] = @ApplicationStatusId,-- 申請状況ID
    /*@ApprovalUserId
    [approval_user_id] = @ApprovalUserId,          -- 承認者ID
    [approval_user_name] = (
        SELECT
            display_name
        FROM
            ms_user
        WHERE
            user_id = @ApprovalUserId
    ),                                             -- 承認者名称
    @ApprovalUserId*/

    /*@ApplicationDate
    [application_date] = @ApplicationDate,         -- 申請日
    @ApplicationDate*/

    /*@ApprovalDate
    [approval_date] = @ApprovalDate,               -- 承認日
    @ApprovalDate*/

    /*@ApplicationReason
    [application_reason] = @ApplicationReason,     -- 申請理由
    @ApplicationReason*/

    /*@RejectionReason
    [rejection_reason] = @RejectionReason,         -- 否認理由
    @RejectionReason*/

    [update_serialid] = update_serialid + 1,       -- 更新シリアルID
    [update_datetime] = @UpdateDatetime,           -- 更新日時
    [update_user_id] = @UpdateUserId               -- 更新ユーザー
WHERE
    history_management_id = @HistoryManagementId