UPDATE ms_output_template SET
    [template_name] = @TemplateName,                --テンプレート名
    [template_file_path] = @TemplateFilePath,       --テンプレートファイルパス
    [template_file_name] = @TemplateFileName,       --テンプレートファイル名
    [use_user_id] = @UseUserId,                     --使用ユーザID
    [update_serialid] = @UpdateSerialid,            --更新シリアルID
    [delete_flg] = @DeleteFlg,                      --削除フラグ
    [update_datetime] = @UpdateDatetime,            --更新日時
    [update_user_id] = @UpdateUserId                --更新ユーザー
WHERE
    [factory_id] = @FactoryId                   --工場ID
AND
    [report_id] = @ReportId                     --帳票ID
AND
    [template_id] = @TemplateId                 --テンプレートID
