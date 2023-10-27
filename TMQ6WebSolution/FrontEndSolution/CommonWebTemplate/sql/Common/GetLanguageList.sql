/*
構成グループID=9020 より、言語コンボの設定を取得するSQL
*/
-- GetLanguageList
-- ログインユーザの本務の工場ID
WITH user_factory AS(
    SELECT
        TOP 1 dbo.get_target_layer_id(location_structure_id, 1) AS factory_id
    FROM
        ms_user_belong
    WHERE
        user_id = @UserId
    AND duty_flg = 1
)
SELECT
     ie.extension_data AS language_code
    ,si.translation_text AS language_label
FROM
    v_structure_item si
    LEFT OUTER JOIN
        ms_item_extension ie
    ON  si.structure_item_id = ie.item_id
    -- 表示順(上の本務工場)
    LEFT OUTER JOIN
        ms_structure_order AS order_factory
    ON  (
            si.structure_id = order_factory.structure_id
        AND order_factory.factory_id = (
                SELECT
                    factory_id
                FROM
                    user_factory
            )
        )
    -- 表示順(工場共通)
    LEFT OUTER JOIN
        ms_structure_order AS order_common
    ON  (
            si.structure_id = order_common.structure_id
        AND order_common.factory_id = 0
        )
WHERE
    si.structure_group_id = 9020
AND si.language_id = @LanguageId
ORDER BY
    coalesce(coalesce(order_factory.display_order, order_common.display_order), 32768)
    ,si.structure_id