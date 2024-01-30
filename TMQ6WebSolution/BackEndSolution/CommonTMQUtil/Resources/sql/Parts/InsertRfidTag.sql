INSERT INTO pt_rftag_parts_link(
    [rftag_id],                -- RFタグ管理ID
    [parts_id],                -- 予備品ID
    [department_structure_id], -- 部門ID
    [account_structure_id],    -- 勘定科目ID
    [serial_no],               -- 連番
    [update_serialid],         -- 更新シリアルID
    [delete_flg],              -- 削除フラグ
    [insert_datetime],         -- 登録日時
    [insert_user_id],          -- 登録ユーザー
    [update_datetime],         -- 更新日時
    [update_user_id]           -- 更新ユーザー
)
VALUES(
    @RftagId,               -- RFタグ管理ID
    @PartsId,               -- 予備品ID
    @DepartmentStructureId, -- 部門ID
    @AccountStructureId,    -- 部門ID
    @SerialNo,              -- 連番
    0,                      -- 更新シリアルID
    0,                      -- 削除フラグ
    @InsertDatetime,        -- 登録日時
    @InsertUserId,          -- 登録ユーザー
    @UpdateDatetime,        -- 更新日時
    @UpdateUserId           -- 更新ユーザー
)
