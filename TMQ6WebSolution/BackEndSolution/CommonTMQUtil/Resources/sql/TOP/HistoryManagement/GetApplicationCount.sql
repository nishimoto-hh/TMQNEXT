/*
指定されたユーザが申請者の変更管理の件数を取得するSQL
以下のパターンをそれぞれ取得する
機能：機器台帳、長期計画
申請状況：申請データ作成中、承認依頼中、差戻中
*/
-- 申請状況の構成IDと区分を取得
WITH ex_app_status AS(
    SELECT
        vs.structure_id AS app_status_id
        -- 申請データ作成中
        ,CASE
            WHEN ex.extension_data = '10' THEN 1
            ELSE 0
        END AS is_making
        -- 承認依頼中
        ,CASE
            WHEN ex.extension_data = '20' THEN 1
            ELSE 0
        END AS is_request
        -- 差戻中
        ,CASE
            WHEN ex.extension_data = '30' THEN 1
            ELSE 0
        END AS is_return
    FROM
        v_structure AS vs
        INNER JOIN
            ms_item_extension AS ex
        ON  (
                vs.structure_item_id = ex.item_id
            AND ex.sequence_no = 1
            )
    WHERE
        -- 上記のいずれかの区分の申請状況を取得
        vs.structure_group_id = 2090
    AND ex.extension_data IN('10', '20', '30')
)
SELECT
    -- 取得している各列は同じ、それぞれの場合の件数を横持ちで取得
    -- 条件に一致するなら1、そうでないなら0を返しSUMすることで件数を取得
    -- 機器台帳・申請データ作成中
    SUM(
        CASE
            WHEN application_conduct_id = 1
        AND ex.is_making = 1 THEN 1
            ELSE 0
        END
    ) AS mc_making_count
    -- 機器台帳・承認依頼中
    ,SUM(
        CASE
            WHEN application_conduct_id = 1
        AND ex.is_request = 1 THEN 1
            ELSE 0
        END
    ) AS mc_request_count
    -- 機器台帳・差戻中
    ,SUM(
        CASE
            WHEN application_conduct_id = 1
        AND ex.is_return = 1 THEN 1
            ELSE 0
        END
    ) AS mc_return_count
    -- 長期計画・申請データ作成中
    ,SUM(
        CASE
            WHEN application_conduct_id = 2
        AND ex.is_making = 1 THEN 1
            ELSE 0
        END
    ) AS lp_making_count
    -- 長期計画・承認依頼中
    ,SUM(
        CASE
            WHEN application_conduct_id = 2
        AND ex.is_request = 1 THEN 1
            ELSE 0
        END
    ) AS lp_request_count
    -- 長期計画・差戻中
    ,SUM(
        CASE
            WHEN application_conduct_id = 2
        AND ex.is_return = 1 THEN 1
            ELSE 0
        END
    ) AS lp_return_count
FROM
    hm_history_management AS hm
    INNER JOIN
        ex_app_status AS ex
    ON  (
            hm.application_status_id = ex.app_status_id
        )
WHERE
    hm.application_user_id = @UserId