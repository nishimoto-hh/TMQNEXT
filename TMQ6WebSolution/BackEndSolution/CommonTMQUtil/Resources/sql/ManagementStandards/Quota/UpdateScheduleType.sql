-- 割当対象の機器が点検種別毎管理の場合、機器に紐付く機器別管理基準のスケジュール管理基準(開始日基準or完了日基準)を指定された値に更新する
UPDATE mc_management_standards_content 
SET
    [schedule_type_structure_id] = @ScheduleTypeStructureId, -- スケジュール管理基準
    [update_serialid] = update_serialid + 1,                 -- 更新シリアルID
    [update_datetime] = @UpdateDatetime,                     -- 更新日時
    [update_user_id] = @UpdateUserId                         -- 更新ユーザー
WHERE
    management_standards_component_id IN ( 
        SELECT
            comp.management_standards_component_id 
        FROM
            mc_management_standards_component comp 
        WHERE
            comp.machine_id = @MachineId
    ) 
    AND COALESCE(maintainance_kind_structure_id, 0) = COALESCE(@MaintainanceKindStructureId, 0)    -- 登録を行っているデータと同一の点検種別
    AND management_standards_content_id <> @ManagementStandardsContentId -- 登録を行っているデータ以外が対象
