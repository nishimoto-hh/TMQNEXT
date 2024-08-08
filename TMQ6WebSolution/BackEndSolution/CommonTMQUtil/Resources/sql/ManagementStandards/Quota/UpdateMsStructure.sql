-- 構成マスタの削除フラグを更新
UPDATE
    ms_structure 
SET
    [delete_flg] = @DeleteFlg,               -- 削除フラグ
    [update_serialid] = update_serialid + 1, -- 更新シリアルID
    [update_datetime] = @updatedatetime,     -- 更新日時
    [update_user_id] = @updateuserid         -- 更新ユーザー
WHERE
    structure_id = @StructureId