SELECT
    ---------- 申請の申請者かシステム管理者の場合は「1」それ以外は「0」----------
    CASE
        WHEN(
            SELECT
                history.application_user_id
            FROM
                hm_history_management history
            WHERE
                history.history_management_id = @HistoryManagementId
        ) = @UserId
    OR  (
            SELECT
                COALESCE(ex.extension_data, 0)
            FROM
                ms_user mu
                LEFT JOIN
                    ms_structure ms
                ON  mu.authority_level_id = ms.structure_id
                AND ms.structure_group_id = 9040
                LEFT JOIN
                    ms_item_extension ex
                ON  ms.structure_item_id = ex.item_id
                AND ex.sequence_no = 1
            WHERE
                mu.user_id = @UserId
        ) = '99' THEN 1
        ELSE 0
    END AS is_certified,
    ----------- 変更管理IDが紐付く機番情報の場所階層IDに設定されている工場の拡張項目がログインユーザIDの場合は「1」それ以外は「0」 -----------
    (
        SELECT
            CASE
                WHEN ex.extension_data = @UserId THEN 1
                ELSE 0
            END
        FROM
            ms_structure ms
            LEFT JOIN
                ms_item_extension ex
            ON  ms.structure_item_id = ex.item_id
            AND ex.sequence_no = 4
        WHERE
            ms.structure_group_id = 1000
        AND ms.structure_layer_no = 1
        AND ms.structure_id = (
                SELECT
                    dbo.get_target_layer_id(COALESCE(machine.location_structure_id, old_machine.location_structure_id), 1)
                FROM
                    hm_history_management history
                    LEFT JOIN
                        hm_history_management_detail detail
                    ON  history.history_management_id = detail.history_management_id
                    LEFT JOIN
                        hm_mc_machine machine
                    ON  detail.history_management_detail_id = machine.history_management_detail_id
                    LEFT JOIN
                        hm_mc_management_standards_component component
                    ON  detail.history_management_detail_id = component.history_management_detail_id
                    LEFT JOIN
                        mc_machine old_machine
                    ON  component.machine_id = old_machine.machine_id
                WHERE
                    history.history_management_id = @HistoryManagementId
            )
    ) AS is_certified_factory