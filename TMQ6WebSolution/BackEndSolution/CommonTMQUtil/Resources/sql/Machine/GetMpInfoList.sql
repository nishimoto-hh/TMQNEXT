SELECT
    info.mp_information,                                                          --MP情報
    dbo.get_file_download_info(1630, info.mp_information_id) AS file_link_information,     --関連ファイル
    info.mp_information_id,                                                       --MP情報ID
    info.update_serialid AS info_update_serialid,                                 --更新シリアルID(mp_info)
    (
        SELECT
            MAX(ac.update_datetime)
        FROM
            attachment ac
        WHERE
            function_type_id = 1630
        AND key_id = info.mp_information_id
    ) AS max_update_datetime                                                      --MP情報に紐付く文書の最大更新日時
FROM
    mc_mp_information info
WHERE
    info.machine_id = @MachineId
ORDER BY
    mp_information_id
