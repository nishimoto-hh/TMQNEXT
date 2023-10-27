-- 一時テーブル作成
CREATE TABLE #TEMPATTACHMENT (
key_id bigint                       -- キーID
,document_no nvarchar(200)          -- 文書番号
,attachment_type_structure_id int   -- 添付種類ID
,document_type_structure_id int     -- 文書種類ID
,attachment_note nvarchar(800)      -- 文書説明
,attachment_date date               -- 作成日
,attachment_user_id int             -- 作成者ID
,attachment_user_name nvarchar(200) -- 作成者
,subject nvarchar(400)              -- 件名
,location_structure_id int          -- 場所階層ID
,job_structure_id int               -- 職種ID
,name nvarchar(200)                 -- ファイル名
,file_path nvarchar(800)            -- ファイルパス
,function_type_id int               -- 機能タイプID
,attachment_id int                  -- 添付ID
,update_serialid int                -- 更新シリアルID
,conduct_name nvarchar(200)         -- 添付元
,function_name nvarchar(200)        -- アクション名
,person_name nvarchar(200)          -- 作成者
,file_link_name nvarchar(800)       -- ファイル/リンク
,link nvarchar(800)                 -- リンク
,file_name nvarchar(800)            -- ファイル名
,extension_data nvarchar(200)       -- 添付種類拡張項目
);

WITH attachmentTypeNo AS
(
SELECT
    structure_id,
    extension_data
FROM
    ms_structure ms
    LEFT JOIN
        ms_item_extension ex
    ON  ms.structure_item_id = ex.item_id
WHERE
    ms.structure_group_id = 1710
AND ex.sequence_no = 1
),
function_name AS
(
SELECT
    ms.structure_id,
    tr.location_structure_id,
    tr.translation_text
FROM
    ms_structure ms
    LEFT JOIN
        ms_item_extension ex
    ON  ms.structure_item_id = ex.item_id
    AND ex.sequence_no = 1
    LEFT JOIN
        ms_translation tr
    ON  ex.extension_data = cast(tr.translation_id AS varchar)
WHERE
    tr.language_id = @LanguageId
),
conduct_name AS
(
SELECT
    ms.structure_id,
    tr.location_structure_id,
    tr.translation_text
FROM
    ms_structure ms
    LEFT JOIN
        ms_item_extension ex
    ON  ms.structure_item_id = ex.item_id
    AND ex.sequence_no = 2
    LEFT JOIN
        ms_translation tr
    ON  ex.extension_data = cast(tr.translation_id AS varchar)
WHERE
    tr.language_id = @LanguageId
),
factory AS(
    SELECT DISTINCT
        structure_id,
        location.translation_text
    FROM
        v_structure_item_all location
    WHERE
        structure_group_id = 1000
    AND structure_layer_no = 0
    AND language_id = @LanguageId
)

-- 一時テーブルにデータを保存
INSERT
INTO #TEMPATTACHMENT
SELECT
    attachmnt.*,
    COALESCE(conduct_name.translation_text,conduct_name_common.translation_text) AS conduct_name,    --添付元(工場ごとの翻訳が無ければ、共通の翻訳を表示)
    COALESCE(function_name.translation_text,function_name_common.translation_text) AS function_name, --アクション(ボタン名、工場ごとの翻訳が無ければ、共通の翻訳を表示)
    COALESCE(person.display_name, attachmnt.attachment_user_name) AS person_name,      --作成者
    dbo.get_file_download_info_row(name , attachment_id , function_type_id, key_id , attachmentTypeNo.extension_data) AS file_link_name,  --ファイル/リンク
    CASE
        WHEN attachmentTypeNo.extension_data = 1 THEN ''
        ELSE name
    END AS link,                                                                       --リンク
    CASE
        WHEN attachmentTypeNo.extension_data = 2 THEN ''
        ELSE name
    END AS file_name,                                                                  --ファイル名
    attachmentTypeNo.extension_data AS ExtensionData                                   --添付種類拡張項目
     FROM
    (
        /*******************機器台帳-機番添付*******************/
        SELECT
            key_id,                                               --キーID
            document_no,                                          --文書番号
            attachment_type_structure_id,                         --添付種類ID
            document_type_structure_id,                           --文書種類ID
            attachment_note,                                      --文書説明
            attachment_date,                                      --作成日
            attachment_user_id,                                   --作成者ID
            attachment_user_name,                                 --作成者
            machine.machine_name AS subject,                      --件名
            machine.location_structure_id,                        --場所階層ID
            machine.job_structure_id,                             --職種ID
            file_name AS name,                                    --ファイル名
            file_path,                                            --ファイルパス
            function_type_id,                                     --機能タイプID
            attachment_id,                                        --添付ID
            ac.update_serialid                                    --更新シリアルID
        FROM
            attachment ac
            LEFT JOIN
                mc_machine machine
            ON  CAST(ac.key_id AS bigint) = machine.machine_id
        WHERE
            function_type_id = 1600
        /*******************機器台帳-機器添付*******************/
        UNION ALL
        SELECT
            key_id,                                               --キーID
            document_no,                                          --文書番号
            attachment_type_structure_id,                         --添付種類ID
            document_type_structure_id,                           --文書種類ID
            attachment_note,                                      --文書説明
            attachment_date,                                      --作成日
            attachment_user_id,                                   --作成者ID
            attachment_user_name,                                 --作成者
            machine.machine_name AS subject,                      --件名
            machine.location_structure_id,                        --場所階層ID
            machine.job_structure_id,                             --職種ID
            file_name AS name,                                    --ファイル名
            file_path,                                            --ファイルパス
            function_type_id,                                     --機能タイプID
            attachment_id,                                        --添付ID
            ac.update_serialid                                    --更新シリアルID
        FROM
            attachment ac
            LEFT JOIN
                mc_equipment equipment
            ON  CAST(ac.key_id AS bigint) = equipment.equipment_id
            LEFT JOIN
                mc_machine machine
            ON  equipment.machine_id = machine.machine_id
        WHERE
            function_type_id = 1610
        /*******************機器台帳-機器別管理基準タブ-ファイル添付*******************/
        UNION ALL
        SELECT
            key_id,                                               --キーID
            document_no,                                          --文書番号
            attachment_type_structure_id,                         --添付種類ID
            document_type_structure_id,                           --文書種類ID
            attachment_note,                                      --文書説明
            attachment_date,                                      --作成日
            attachment_user_id,                                   --作成者ID
            attachment_user_name,                                 --作成者
            machine.machine_name AS subject,                      --件名
            machine.location_structure_id,                        --場所階層ID
            machine.job_structure_id,                             --職種ID
            file_name AS name,                                    --ファイル名
            file_path,                                            --ファイルパス
            function_type_id,                                     --機能タイプID
            attachment_id,                                        --添付ID
            ac.update_serialid                                    --更新シリアルID
        FROM
            attachment ac
            LEFT JOIN
                mc_management_standards_content content
            ON  CAST(ac.key_id AS bigint) = content.management_standards_content_id
            LEFT JOIN
                mc_management_standards_component component
            ON  content.management_standards_component_id = component.management_standards_component_id
            LEFT JOIN
                mc_machine machine
            ON  component.machine_id = machine.machine_id
        WHERE
            function_type_id = 1620
        /*******************機器台帳-MP情報-ファイル添付*******************/
        UNION ALL
        SELECT
           key_id,                                                --キーID
           document_no,                                           --文書番号
           attachment_type_structure_id,                          --添付種類ID
           document_type_structure_id,                            --文書種類ID
           attachment_note,                                       --文書説明
           attachment_date,                                       --作成日
           attachment_user_id,                                    --作成者ID
           attachment_user_name,                                  --作成者
           machine.machine_name AS subject,                       --件名
           machine.location_structure_id,                         --場所階層ID
           machine.job_structure_id,                              --職種ID
           file_name AS name,                                     --ファイル名
           file_path,                                             --ファイルパス
           function_type_id,                                      --機能タイプID
           attachment_id,                                         --添付ID
           ac.update_serialid                                     --更新シリアルID
        FROM
           attachment ac
           LEFT JOIN
               mc_mp_information info
           ON  CAST(ac.key_id AS bigint) = info.mp_information_id
           LEFT JOIN
               mc_machine machine
           ON  info.machine_id = machine.machine_id
        WHERE
           function_type_id = 1630
        /*******************件名別長期計画-件名添付*******************/
        UNION ALL
        SELECT
            key_id,                                               --キーID
            document_no,                                          --文書番号
            attachment_type_structure_id,                         --添付種類ID
            document_type_structure_id,                           --文書種類ID
            attachment_note,                                      --文書説明
            attachment_date,                                      --作成日
            attachment_user_id,                                   --作成者ID
            attachment_user_name,                                 --作成者
            lnplan.subject AS subject,                            --件名
            lnplan.location_structure_id,                         --場所階層ID
            lnplan.job_structure_id,                              --職種ID
            file_name AS name,                                    --ファイル名
            file_path,                                            --ファイルパス
            function_type_id,                                     --機能タイプID
            attachment_id,                                        --添付ID
            ac.update_serialid                                    --更新シリアルID
        FROM
            attachment ac
            LEFT JOIN
                ln_long_plan lnplan
            ON  CAST(ac.key_id AS bigint) = lnplan.long_plan_id
        WHERE
            function_type_id = 1640
        /*******************保全活動-件名添付*******************/
        UNION ALL
        SELECT
            key_id,                                              --キーID
            document_no,                                         --文書番号
            attachment_type_structure_id,                        --添付種類ID
            document_type_structure_id,                          --文書種類ID
            attachment_note,                                     --文書説明
            attachment_date,                                     --作成日
            attachment_user_id,                                  --作成者ID
            attachment_user_name,                                --作成者
            summary.subject AS subject,                          --件名
            summary.location_structure_id,                       --場所階層ID
            summary.job_structure_id,                            --職種ID
            file_name AS name,                                   --ファイル名
            file_path,                                           --ファイルパス
            function_type_id,                                    --機能タイプID
            attachment_id,                                       --添付ID
            ac.update_serialid                                   --更新シリアルID
        FROM
            attachment ac
            LEFT JOIN
                ma_summary summary
            ON  CAST(ac.key_id AS bigint) = summary.summary_id
        WHERE
            function_type_id = 1650
        /*******************保全活動-故障分析情報タブ-略図添付*******************/
        UNION ALL
        SELECT
            key_id,                                              --キーID
            document_no,                                         --文書番号
            attachment_type_structure_id,                        --添付種類ID
            document_type_structure_id,                          --文書種類ID
            attachment_note,                                     --文書説明
            attachment_date,                                     --作成日
            attachment_user_id,                                  --作成者ID
            attachment_user_name,                                --作成者
            summary.subject + '略図添付' AS subject,             --件名
            summary.location_structure_id,                       --場所階層ID
            summary.job_structure_id,                            --職種ID
            file_name AS name,                                   --ファイル名
            file_path,                                           --ファイルパス
            function_type_id,                                    --機能タイプID
            attachment_id,                                       --添付ID
            ac.update_serialid                                   --更新シリアルID
        FROM
            attachment ac
            LEFT JOIN
                ma_history_failure failure
            ON  CAST(ac.key_id AS bigint) = failure.history_failure_id
            LEFT JOIN
                ma_history history
            ON  failure.history_id = history.history_id
            LEFT JOIN
                ma_summary summary
            ON  history.summary_id = summary.summary_id
        WHERE
            function_type_id = 1660
        /*******************保全活動-故障分析情報タブ-故障原因分析書添付*******************/
        UNION ALL
        SELECT
            key_id,                                              --キーID
            document_no,                                         --文書番号
            attachment_type_structure_id,                        --添付種類ID
            document_type_structure_id,                          --文書種類ID
            attachment_note,                                     --文書説明
            attachment_date,                                     --作成日
            attachment_user_id,                                  --作成者ID
            attachment_user_name,                                --作成者
            summary.subject + '故障原因分析書添付' AS subject,   --件名
            summary.location_structure_id,                       --場所階層ID
            summary.job_structure_id,                            --職種ID
            file_name AS name,                                   --ファイル名
            file_path,                                           --ファイルパス
            function_type_id,                                    --機能タイプID
            attachment_id,                                       --添付ID
            ac.update_serialid                                   --更新シリアルID
        FROM
            attachment ac
            LEFT JOIN
                ma_history_failure failure
            ON  CAST(ac.key_id AS bigint) = failure.history_failure_id
            LEFT JOIN
                ma_history history
            ON  failure.history_id = history.history_id
            LEFT JOIN
                ma_summary summary
            ON  history.summary_id = summary.summary_id
        WHERE
            function_type_id = 1670
        /*******************保全活動-故障分析情報(個別工場)タブ-略図添付*******************/
        UNION ALL
        SELECT
            key_id,                                              --キーID
            document_no,                                         --文書番号
            attachment_type_structure_id,                        --添付種類ID
            document_type_structure_id,                          --文書種類ID
            attachment_note,                                     --文書説明
            attachment_date,                                     --作成日
            attachment_user_id,                                  --作成者ID
            attachment_user_name,                                --作成者
            summary.subject + '略図添付' AS subject,             --件名
            summary.location_structure_id,                       --場所階層ID
            summary.job_structure_id,                            --職種ID
            file_name AS name,                                   --ファイル名
            file_path,                                           --ファイルパス
            function_type_id,                                    --機能タイプID
            attachment_id,                                       --添付ID
            ac.update_serialid                                   --更新シリアルID
        FROM
            attachment ac
            LEFT JOIN
                ma_history_failure failure
            ON  CAST(ac.key_id AS bigint) = failure.history_failure_id
            LEFT JOIN
                ma_history history
            ON  failure.history_id = history.history_id
            LEFT JOIN
                ma_summary summary
            ON  history.summary_id = summary.summary_id
        WHERE
            function_type_id = 1680
        /*******************保全活動-故障分析情報(個別工場)タブ-故障原因分析書添付*******************/
        UNION ALL
        SELECT
            key_id,                                             --キーID
            document_no,                                        --文書番号
            attachment_type_structure_id,                       --添付種類ID
            document_type_structure_id,                         --文書種類ID
            attachment_note,                                    --文書説明
            attachment_date,                                    --作成日
            attachment_user_id,                                 --作成者ID
            attachment_user_name,                               --作成者
            summary.subject + '故障原因分析書添付' AS subject,  --件名
            summary.location_structure_id,                      --場所階層ID
            summary.job_structure_id,                           --職種ID
            file_name AS name,                                  --ファイル名
            file_path,                                          --ファイルパス
            function_type_id,                                   --機能タイプID
            attachment_id,                                      --添付ID
            ac.update_serialid                                  --更新シリアルID
        FROM
            attachment ac
        LEFT JOIN
            ma_history_failure failure
        ON  CAST(ac.key_id AS bigint) = failure.history_failure_id
        LEFT JOIN
            ma_history history
        ON  failure.history_id = history.history_id
        LEFT JOIN
            ma_summary summary
        ON  history.summary_id = summary.summary_id
        WHERE
            function_type_id = 1690
        /*******************予備品管理-詳細画面-画像添付*******************/
        UNION ALL
        SELECT
            key_id,                                             --キーID
            document_no,                                        --文書番号
            attachment_type_structure_id,                       --添付種類ID
            document_type_structure_id,                         --文書種類ID
            attachment_note,                                    --文書説明
            attachment_date,                                    --作成日
            attachment_user_id,                                 --作成者ID
            attachment_user_name,                               --作成者
            parts.parts_name + '_画像' AS subject,              --件名
            parts.factory_id AS location_structure_id,          --場所階層ID
            parts.job_structure_id,                             --職種ID
            file_name AS name,                                  --ファイル名
            file_path,                                          --ファイルパス
            function_type_id,                                   --機能タイプID
            attachment_id,                                      --添付ID
            ac.update_serialid                                  --更新シリアルID
        FROM
            attachment ac
        LEFT JOIN
            pt_parts parts
        ON  CAST(ac.key_id AS bigint) = parts.parts_id
        WHERE
            function_type_id = 1700
        /*******************予備品管理-詳細画面-文書添付*******************/
        UNION ALL
        SELECT
            key_id,                                             --キーID
            document_no,                                        --文書番号
            attachment_type_structure_id,                       --添付種類ID
            document_type_structure_id,                         --文書種類ID
            attachment_note,                                    --文書説明
            attachment_date,                                    --作成日
            attachment_user_id,                                 --作成者ID
            attachment_user_name,                               --作成者
            parts.parts_name + '_文書' AS subject,              --件名
            parts.factory_id AS location_structure_id,          --場所階層ID
            parts.job_structure_id,                             --職種ID
            file_name AS name,                                  --ファイル名
            file_path,                                          --ファイルパス
            function_type_id,                                   --機能タイプID
            attachment_id,                                      --添付ID
            ac.update_serialid                                  --更新シリアルID
        FROM
            attachment ac
        LEFT JOIN
            pt_parts parts
        ON  CAST(ac.key_id AS bigint) = parts.parts_id
        WHERE
            function_type_id = 1750
        /*******************予備品管理-詳細画面-予備品地図*******************/
        UNION ALL
        SELECT
            key_id,                                             --キーID
            document_no,                                        --文書番号
            attachment_type_structure_id,                       --添付種類ID
            document_type_structure_id,                         --文書種類ID
            attachment_note,                                    --文書説明
            attachment_date,                                    --作成日
            attachment_user_id,                                 --作成者ID
            attachment_user_name,                               --作成者
            factory.translation_text + '_予備品地図' as subject,--件名
            ac.key_id AS location_structure_id,                 --場所階層ID
            0 AS job_structure_id,                              --職種ID  
            file_name AS name,                                  --ファイル名
            file_path,                                          --ファイルパス
            function_type_id,                                   --機能タイプID
            attachment_id,                                      --添付ID
            ac.update_serialid                                  --更新シリアルID
        FROM
            attachment ac
            LEFT JOIN
                factory
            ON  ac.key_id = factory.structure_id
        WHERE
            function_type_id = 1780

    ) attachmnt
    LEFT JOIN
        ms_user AS person
    ON  (
            attachmnt.attachment_user_id = person.user_id
        )
    AND person.language_id = @LanguageId
    LEFT JOIN
        function_name
    ON  attachmnt.document_type_structure_id = function_name.structure_id
    AND function_name.location_structure_id = @CommomFactoryId
    LEFT JOIN
        function_name AS function_name_common
    ON  attachmnt.document_type_structure_id = function_name_common.structure_id
    AND function_name_common.location_structure_id = dbo.get_target_layer_id(attachmnt.location_structure_id, 1)
    LEFT JOIN
           conduct_name
    on attachmnt.document_type_structure_id = conduct_name.structure_id
    AND conduct_name.location_structure_id = @CommomFactoryId
    LEFT JOIN
           conduct_name as conduct_name_common
    on attachmnt.document_type_structure_id = conduct_name_common.structure_id
    AND conduct_name_common.location_structure_id =  dbo.get_target_layer_id(attachmnt.location_structure_id, 1)   

    LEFT JOIN
        attachmentTypeNo
    ON  attachmnt.attachment_type_structure_id = attachmentTypeNo.structure_id
