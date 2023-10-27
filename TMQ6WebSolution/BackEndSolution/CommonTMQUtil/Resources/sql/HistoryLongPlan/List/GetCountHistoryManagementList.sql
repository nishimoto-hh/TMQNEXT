SELECT
    COUNT(*) 
FROM
    ( 
        SELECT
            history.history_management_id
            , COALESCE( 
                hmlp.location_structure_id
                , lp.location_structure_id
            ) AS location_structure_id
            , COALESCE(hmlp.job_structure_id, lp.job_structure_id) AS job_structure_id 
        FROM
            hm_history_management history       -- 変更管理
            LEFT JOIN hm_ln_long_plan hmlp      -- 長計件名変更管理
                ON history.key_id = hmlp.long_plan_id 
                AND history.history_management_id = hmlp.history_management_id 
            LEFT JOIN ln_long_plan AS lp        -- 長計件名
                ON history.key_id = lp.long_plan_id 
            LEFT JOIN ms_structure status_ms    -- 構成マスタ(申請状況)
                ON history.application_status_id = status_ms.structure_id 
            LEFT JOIN ms_item_extension status_ex -- アイテムマスタ拡張(申請状況)
                ON status_ms.structure_item_id = status_ex.item_id 
                AND status_ex.sequence_no = 1 
        WHERE
            -- 「申請データ作成中」「承認依頼中」「差戻中」のデータのみ
            status_ex.extension_data IN ('10', '20', '30') -- 「2：長期計画」のデータのみ
            AND history.application_conduct_id = 2 
            /*@DispOnlyMySubject
            -- 自分の申請のみ表示
            AND history.application_user_id = @UserId
            @DispOnlyMySubject*/
    ) tbl 
WHERE
