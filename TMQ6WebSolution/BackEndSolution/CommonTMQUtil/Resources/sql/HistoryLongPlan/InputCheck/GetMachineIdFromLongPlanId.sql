/*
* 長期計画IDより、紐づく機器別管理基準を検索し、機番IDを一意に取得するSQL
*/
/*
* 長期計画IDより、紐づく機器別管理基準を検索し、機番IDを一意に取得するSQL
*/
WITH history AS ( 
    SELECT
        hmcon.management_standards_component_id
        , hmcon.execution_division
    FROM
        hm_history_management hm 
        INNER JOIN hm_mc_management_standards_content hmcon 
            ON hm.history_management_id = hmcon.history_management_id 
        LEFT JOIN ms_structure status_ms        -- 構成マスタ(申請状況)
            ON hm.application_status_id = status_ms.structure_id 
        LEFT JOIN ms_item_extension status_ex   -- アイテムマスタ拡張(申請状況)
            ON status_ms.structure_item_id = status_ex.item_id 
            AND status_ex.sequence_no = 1 
    WHERE
        hm.key_id = @LongPlanId 
        AND status_ex.extension_data IN ('10', '20', '30') --「申請データ作成中」「承認依頼中」「差戻中」のデータのみ
) 
SELECT DISTINCT
    com.machine_id 
FROM
    mc_management_standards_component AS com 
WHERE
    EXISTS ( 
        SELECT
            * 
        FROM
            ( 
                --トランザクション
                SELECT
                    con.management_standards_component_id 
                FROM
                    mc_management_standards_content AS con 
                WHERE
                    con.long_plan_id = @LongPlanId 
                    AND NOT EXISTS ( 
                        SELECT
                            * 
                        FROM
                            history 
                        WHERE
                            history.execution_division = 5 --削除した保全情報一覧の情報
                            AND history.management_standards_component_id = con.management_standards_component_id
                    ) --削除した保全情報一覧の情報は除外する
                    
                    UNION 
                    
                --変更管理
                SELECT
                    history.management_standards_component_id 
                FROM
                    history 
                WHERE
                    history.execution_division = 4 --追加した保全情報一覧の情報
            ) tbl 
        WHERE
            com.management_standards_component_id = tbl.management_standards_component_id
    )
