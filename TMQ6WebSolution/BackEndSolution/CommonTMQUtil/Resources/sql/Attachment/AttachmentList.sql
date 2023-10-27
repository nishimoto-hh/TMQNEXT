SELECT
    attachmnt.*, 
    conduct_name.translation_text AS conduct_name,    --添付元
    function_name.translation_text AS function_name, --アクション(ボタン名)
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
            ON  ac.key_id = machine.machine_id
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
            ON  ac.key_id = equipment.equipment_id
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
            ON  ac.key_id = content.management_standards_content_id
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
           ON  ac.key_id = info.mp_information_id
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
            ON  ac.key_id = lnplan.long_plan_id
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
            ON  ac.key_id = summary.summary_id
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
            summary.subject + rs_subject.translation_text AS subject, --件名
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
            ON  ac.key_id = failure.history_failure_id
            LEFT JOIN
                ma_history history
            ON  failure.history_id = history.history_id
            LEFT JOIN
                ma_summary summary
            ON  history.summary_id = summary.summary_id
            LEFT JOIN
                rs_subject
            ON 1 = 1
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
            summary.subject + failure_subject.translation_text AS subject, --件名
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
            ON  ac.key_id = failure.history_failure_id
            LEFT JOIN
                ma_history history
            ON  failure.history_id = history.history_id
            LEFT JOIN
                ma_summary summary
            ON  history.summary_id = summary.summary_id
            LEFT JOIN
                failure_subject
            ON 1 = 1
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
            summary.subject + rs_subject.translation_text AS subject, --件名
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
            ON  ac.key_id = failure.history_failure_id
            LEFT JOIN
                ma_history history
            ON  failure.history_id = history.history_id
            LEFT JOIN
                ma_summary summary
            ON  history.summary_id = summary.summary_id
            LEFT JOIN
                rs_subject
            ON 1 = 1
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
            summary.subject + failure_subject.translation_text AS subject, --件名
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
        ON  ac.key_id = failure.history_failure_id
        LEFT JOIN
            ma_history history
        ON  failure.history_id = history.history_id
        LEFT JOIN
            ma_summary summary
        ON  history.summary_id = summary.summary_id
        LEFT JOIN
            failure_subject
        ON 1 = 1
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
            parts.parts_name + '_' + img_subject.translation_text AS subject, --件名
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
        ON  ac.key_id = parts.parts_id
        LEFT JOIN
            img_subject
        ON 1 = 1
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
            parts.parts_name + '_' + doc_subject.translation_text AS subject, --件名
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
        ON  ac.key_id = parts.parts_id
        LEFT JOIN
            doc_subject
        ON 1 = 1
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
            factory.translation_text + '_' + map_subject.translation_text as subject, --件名
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
            LEFT JOIN
                map_subject
            ON 1 = 1
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
           conduct_name
    on attachmnt.document_type_structure_id = conduct_name.structure_id
    AND conduct_name.location_structure_id = @CommomFactoryId
    LEFT JOIN
        attachmentTypeNo
    ON  attachmnt.attachment_type_structure_id = attachmentTypeNo.structure_id
