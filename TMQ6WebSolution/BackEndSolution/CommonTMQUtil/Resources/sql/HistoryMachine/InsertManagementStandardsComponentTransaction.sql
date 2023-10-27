INSERT INTO mc_management_standards_component(
    [management_standards_component_id] -- 機器別管理基準部位ID
    ,[machine_id]                       -- 機番ID
    ,[inspection_site_structure_id]     -- 部位情報
    ,[is_management_standard_conponent] -- 機器別管理基準フラグ
    ,[update_serialid]                  -- 更新シリアルID
    ,[insert_datetime]                  -- 登録日時
    ,[insert_user_id]                   -- 登録ユーザー
    ,[update_datetime]                  -- 更新日時
    ,[update_user_id]                   -- 更新ユーザー
)
SELECT
     management_standards_component_id -- 機器別管理基準部位ID
    ,machine_id                        -- 機番ID
    ,inspection_site_structure_id      -- 部位情報
    ,is_management_standard_conponent  -- 機器別管理基準フラグ
    ,0                                 -- 更新シリアルID
    ,@InsertDatetime                   -- 登録日時
    ,@InsertUserId                     -- 登録ユーザー
    ,@UpdateDatetime                   -- 更新日時
    ,@UpdateUserId                     -- 更新ユーザー
FROM
    hm_mc_management_standards_component component
WHERE
    component.hm_management_standards_component_id = @HmManagementStandardsComponentId