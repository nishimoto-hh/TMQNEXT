-- 変更管理IDより、変更管理詳細IDを取得する
SELECT
    detail.history_management_detail_id -- 変更管理詳細ID
FROM
    hm_history_management_detail detail
WHERE
    detail.history_management_id = @HistoryManagementId