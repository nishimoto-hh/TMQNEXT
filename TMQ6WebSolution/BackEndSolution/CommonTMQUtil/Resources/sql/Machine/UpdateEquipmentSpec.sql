UPDATE mc_equipment_spec 
SET
    [spec_value] = @SpecValue                   -- 設定値(テキスト)
    , [spec_structure_id] = @SpecStructureId    -- 設定値(選択)
    , [spec_num] = @SpecNum                     -- 設定値(数値)
    , [spec_num_min] = @SpecNumMin              -- 設定値(数値(範囲))最小値
    , [spec_num_max] = @SpecNumMax              -- 設定値(数値(範囲))最大値
    , [update_serialid] = update_serialid + 1   -- 更新シリアルID
    , [update_datetime] = @UpdateDatetime       -- 更新日時
    , [update_user_id] = @UpdateUserId          -- 更新ユーザー
WHERE
    [equipment_spec_id] = @EquipmentSpecId    -- 機種別仕様id
