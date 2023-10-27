/*
* 長期計画に紐づく機器の点検種別毎管理を取得するSQL
* 長期計画内で1種類のはずだが、有→無の順に取得する
*/
WITH history AS ( 
    SELECT
        hmcon.management_standards_component_id
        , hmcon.execution_division
        , hm.key_id AS long_plan_id 
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
, content AS ( 
    --トランザクション
    SELECT
        con.management_standards_component_id
        , con.long_plan_id 
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
                history.execution_division = 5  --削除した保全情報一覧の情報
                AND history.management_standards_component_id = con.management_standards_component_id
        )                                       --削除した保全情報一覧の情報は除外する
        
        UNION 
        
    --変更管理
    SELECT
        history.management_standards_component_id
        , history.long_plan_id 
    FROM
        history 
) 
SELECT
    TOP 1 equip.maintainance_kind_manage 
FROM
    content AS con 
    INNER JOIN mc_management_standards_component AS com 
        ON ( 
            con.management_standards_component_id = com.management_standards_component_id
        ) 
    INNER JOIN mc_machine AS machine 
        ON (com.machine_id = machine.machine_id) 
    INNER JOIN mc_equipment AS equip 
        ON (machine.machine_id = equip.machine_id) 
WHERE
    con.long_plan_id = @LongPlanId 
ORDER BY
    equip.maintainance_kind_manage DESC
