INSERT INTO mc_management_standards(
    [management_standards_id],               -- 機器別管理基準標準ID
    [location_structure_id],                 -- 機能場所階層ID
    [location_district_structure_id],        -- 地区ID
    [location_factory_structure_id],         -- 工場ID
    [job_structure_id],                      -- 職種機種階層ID
    [job_kind_structure_id],                 -- 職種ID
    [job_large_classfication_structure_id],  -- 機種大分類ID
    [job_middle_classfication_structure_id], -- 機種中分類ID
    [job_small_classfication_structure_id],  -- 機種小分類ID
    [management_standards_name],             -- 標準名称
    [memo],                                  -- メモ
    [update_serialid],                       -- 更新シリアルID
    [insert_datetime],                       -- 登録日時
    [insert_user_id],                        -- 登録ユーザー
    [update_datetime],                       -- 更新日時
    [update_user_id]                         -- 更新ユーザー
)OUTPUT inserted.management_standards_id
VALUES(
    NEXT VALUE FOR seq_mc_management_standards_management_standards_id, -- 機器別管理基準標準ID
    @LocationStructureId,                                               -- 機能場所階層ID
    @LocationDistrictStructureId,                                       -- 地区ID
    @LocationFactoryStructureId,                                        -- 工場ID
    @JobStructureId,                                                    -- 職種機種階層ID
    @JobKindStructureId,                                                -- 職種ID
    @JobLargeClassficationStructureId,                                  -- 機種大分類ID
    @JobMiddleClassficationStructureId,                                 -- 機種中分類ID
    @JobSmallClassficationStructureId,                                  -- 機種小分類ID
    @ManagementStandardsName,                                           -- 標準名称
    @Memo,                                                              -- メモ
    @UpdateSerialid,                                                    -- 更新シリアルID
    @InsertDatetime,                                                    -- 登録日時
    @InsertUserId,                                                      -- 登録ユーザー
    @UpdateDatetime,                                                    -- 更新日時
    @UpdateUserId                                                       -- 更新ユーザー
)
