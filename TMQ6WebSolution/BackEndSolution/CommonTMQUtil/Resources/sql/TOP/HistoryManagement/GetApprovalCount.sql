/*
指定されたユーザの承認待ちの変更管理の件数を取得するSQL
*/
-- 申請状況の承認待ちの構成IDを取得
WITH ex_app_status AS(
    SELECT
        vs.structure_id AS app_status_id
    FROM
        v_structure AS vs
        INNER JOIN
            ms_item_extension AS ex
        ON  (
                vs.structure_item_id = ex.item_id
            AND ex.sequence_no = 1
            )
    WHERE
        -- 申請状況・承認待ちを取得
        vs.structure_group_id = 2090
    AND ex.extension_data = '20'
)
-- ユーザが承認者に設定されている工場
,user_approval_factory AS(
    SELECT
        vs.factory_id
    FROM
        v_structure AS vs
        INNER JOIN
            ms_item_extension AS ex
        ON  (
                vs.structure_item_id = ex.item_id
            AND ex.sequence_no = 4
            )
    WHERE
        vs.structure_group_id = '1000'
    AND vs.structure_layer_no = 1
    AND ex.extension_data = CONVERT(nvarchar(400),@UserId)
)
SELECT
     -- 機器台帳の件数
     SUM(
        CASE
            WHEN hm.application_conduct_id = 1 THEN 1
            ELSE 0
        END
    ) AS mc_app_req_count
    -- 長期計画の件数
    ,SUM(
        CASE
            WHEN hm.application_conduct_id = 2 THEN 1
            ELSE 0
        END
    ) AS lp_app_req_count
FROM
    hm_history_management AS hm
WHERE
    -- 承認待ちと、ユーザが承認者の工場で絞り込み
    EXISTS(
        SELECT
            *
        FROM
            ex_app_status AS ex
        WHERE
            ex.app_status_id = hm.application_status_id
    )
AND EXISTS(
        SELECT
            *
        FROM
            user_approval_factory AS factory
        WHERE
            factory.factory_id = hm.factory_id
    )