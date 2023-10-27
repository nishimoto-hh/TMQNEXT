INSERT INTO ms_output_template(
    [factory_id],               --工場ID
    [report_id],                --帳票ID
    [template_id],              --テンプレートID
    [template_name],            --テンプレート名
    [template_file_path],       --テンプレートファイルパス
    [template_file_name],       --テンプレートファイル名
    [use_user_id],              --使用ユーザID
    [update_serialid],          --更新シリアルID
    [insert_datetime],          --登録日時
    [insert_user_id],           --登録ユーザー
    [update_datetime],          --更新日時
    [update_user_id]            --更新ユーザー
)
VALUES(
    @FactoryId,                 --工場ID
    @ReportId,                  --帳票ID
    @TemplateId,                --テンプレートID
    @TemplateName,              --テンプレート名
    @TemplateFilePath,          --テンプレートファイルパス
    @TemplateFileName,          --テンプレートファイル名
    @UseUserId,                 --使用ユーザID
    @UpdateSerialid,            --更新シリアルID
    @InsertDatetime,            --登録日時
    @InsertUserId,              --登録ユーザー
    @UpdateDatetime,            --更新日時
    @UpdateUserId               --更新ユーザー
)
