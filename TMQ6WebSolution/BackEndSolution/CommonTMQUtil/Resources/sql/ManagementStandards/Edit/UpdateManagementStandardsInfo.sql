UPDATE
    mc_management_standards
SET
    [location_structure_id] = @LocationStructureId,                               -- 機能場所階層ID
    [location_district_structure_id] = @LocationDistrictStructureId,              -- 地区ID
    [location_factory_structure_id] = @LocationFactoryStructureId,                -- 工場ID
    [job_structure_id] = @JobStructureId,                                         -- 職種機種階層ID
    [job_kind_structure_id] = @JobKindStructureId,                                -- 職種ID
    [job_large_classfication_structure_id] = @JobLargeClassficationStructureId,   -- 機種大分類ID
    [job_middle_classfication_structure_id] = @JobMiddleClassficationStructureId, -- 機種中分類ID
    [job_small_classfication_structure_id] = @JobSmallClassficationStructureId,   -- 機種小分類ID
    [management_standards_name] = @ManagementStandardsName,                       -- 標準名称
    [memo] = @Memo,                                                               -- メモ
    [update_serialid] = update_serialid + 1,                                      -- 更新シリアルID
    [update_datetime] = @UpdateDatetime,                                          -- 更新日時
    [update_user_id] = @UpdateUserId                                              -- 更新ユーザー
WHERE
    management_standards_id = @ManagementStandardsId
