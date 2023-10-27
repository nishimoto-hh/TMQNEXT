/*
* ログインユーザのユーザIDより、所属工場の変更管理の承認者を取得するSQL
*/
-- ユーザ所属マスタの場所より、工場を取得
WITH user_factory AS(
    SELECT DISTINCT
        st.factory_id
    FROM
        ms_structure AS st
    WHERE
        EXISTS(
            SELECT
                *
            FROM
                ms_user_belong AS ub
            WHERE
                ub.user_id = @UserId
            AND ub.location_structure_id = st.structure_id
        )
)
-- 工場の拡張項目4が承認者のユーザID
SELECT
    ex.extension_data AS approval_user_id
FROM
    v_structure AS vs
    INNER JOIN
        ms_item_extension AS ex
    ON  (
            vs.structure_item_id = ex.item_id
        AND ex.sequence_no = 4
        )
WHERE
    EXISTS(
        SELECT
            *
        FROM
            user_factory AS uf
        WHERE
            vs.structure_id = uf.factory_id
    )