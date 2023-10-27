INSERT INTO mc_machine(
    [machine_id]                    -- 機番ID
    ,[location_structure_id]        -- 機能場所階層ID
    ,[job_structure_id]             -- 職種機種階層ID
    ,[machine_no]                   -- 機器番号
    ,[machine_name]                 -- 機器名称
    ,[installation_location]        -- 設置場所
    ,[number_of_installation]       -- 設置台数
    ,[equipment_level_structure_id] -- 機器レベル
    ,[date_of_installation]         -- 設置日
    ,[importance_structure_id]      -- 重要度
    ,[conservation_structure_id]    -- 保全方式
    ,[machine_note]                 -- 機番メモ
    ,[update_serialid]              -- 更新シリアルID
    ,[insert_datetime]              -- 登録日時
    ,[insert_user_id]               -- 登録ユーザー
    ,[update_datetime]              -- 更新日時
    ,[update_user_id]               -- 更新ユーザー
)
SELECT
    machine_id                    -- 機番ID
    ,location_structure_id        -- 機能場所階層ID
    ,job_structure_id             -- 職種機種階層ID
    ,machine_no                   -- 機器番号
    ,machine_name                 -- 機器名称
    ,installation_location        -- 設置場所
    ,number_of_installation       -- 設置台数
    ,equipment_level_structure_id -- 機器レベル
    ,date_of_installation         -- 設置日
    ,importance_structure_id      -- 重要度
    ,conservation_structure_id    -- 保全方式
    ,machine_note                 -- 機番メモ
    ,0                            -- 更新シリアルID
    ,@InsertDatetime              -- 登録日時
    ,@InsertUserId                -- 登録ユーザー
    ,@UpdateDatetime              -- 更新日時
    ,@UpdateUserId                -- 更新ユーザー
FROM
    hm_mc_machine machine
WHERE
    machine.history_management_id = @HistoryManagementId