SELECT
key_id                        -- キーID
,document_no                  -- 文書番号
,attachment_type_structure_id -- 添付種類ID
,document_type_structure_id   -- 文書種類ID
,attachment_note              -- 文書説明
,attachment_date              -- 作成日
,attachment_user_id           -- 作成者ID
,attachment_user_name         -- 作成者
,subject                      -- 件名
,location_structure_id        -- 場所階層ID
,job_structure_id             -- 職種ID
,name                         -- ファイル名
,file_path                    -- ファイルパス
,function_type_id             -- 機能タイプID
,attachment_id                -- 添付ID
,update_serialid              -- 更新シリアルID
,conduct_name                 -- 添付元
,function_name                -- アクション名
,person_name                  -- 作成者
,file_link_name               -- ファイル/リンク
,link                         -- リンク
,file_name                    -- ファイル名
,extension_data               -- 添付種類拡張項目
FROM
    #TEMPATTACHMENT temp