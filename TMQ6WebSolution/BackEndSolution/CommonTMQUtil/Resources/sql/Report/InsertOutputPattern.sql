INSERT INTO ms_output_pattern( 
    [factory_id],               -- 工場ID
    [report_id],                -- 帳票ID
    [template_id],              -- テンプレートID
    [output_pattern_id],        -- 出力パターンID
    [output_pattern_name],      -- 出力パターン名
    [use_user_id],              -- 使用ユーザID
    [update_serialid],          -- 更新シリアルID
    [insert_datetime],          -- 登録日時
    [insert_user_id],           -- 登録ユーザー
    [update_datetime],          -- 更新日時
    [update_user_id]            -- 更新ユーザー
) 
VALUES ( 
    @FactoryId,                 -- 工場ID
    @ReportId,                  -- 帳票ID
    @TemplateId,                -- テンプレートID
    @OutputPatternId,           -- 出力パターンID
    @OutputPatternName,         -- 出力パターン名
    @UseUserId,                 -- 使用ユーザID
    @UpdateSerialid,            -- 更新シリアルID
    @InsertDatetime,            -- 登録日時
    @InsertUserId,              -- 登録ユーザー
    @UpdateDatetime,            -- 更新日時
    @UpdateUserId               -- 更新ユーザー
)
