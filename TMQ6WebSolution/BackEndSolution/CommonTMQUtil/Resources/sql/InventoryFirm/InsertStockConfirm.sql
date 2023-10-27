INSERT INTO pt_stock_confirm(
    [stock_confirm_id],                                  -- 確定管理ID
    [target_month],                                      -- 対象年月
    [factory_id],                                        -- 工場ID
    [parts_job_id],                                      -- 職種ID
    [execution_datetime],                                -- 実行日時
    [execution_user_id],                                 -- 実行ユーザーID
    [update_serialid],                                   -- 更新シリアルID
    [delete_flg],                                        -- 削除フラグ
    [insert_datetime],                                   -- 登録日時
    [insert_user_id],                                    -- 登録ユーザー
    [update_datetime],                                   -- 更新日時
    [update_user_id]                                     -- 更新ユーザー
)
VALUES(
    NEXT VALUE FOR seq_pt_stock_confirm_stock_confirm_id,-- 確定管理ID
    @TargetMonth,                                        -- 対象年月
    @FactoryId,                                          -- 工場ID
    @PartsJobId,                                         -- 職種ID
    @ExecutionDatetime,                                  -- 実行日時
    @ExecutionUserId,                                    -- 実行ユーザーID
    @UpdateSerialid,                                     -- 更新シリアルID
    0,                                                   -- 削除フラグ
    @InsertDatetime,                                     -- 登録日時
    @InsertUserId,                                       -- 登録ユーザー
    @UpdateDatetime,                                     -- 更新日時
    @UpdateUserId                                        -- 更新ユーザー
)
