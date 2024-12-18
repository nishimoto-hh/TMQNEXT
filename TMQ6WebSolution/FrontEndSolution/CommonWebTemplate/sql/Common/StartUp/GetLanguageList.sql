/*
構成グループID=9020 より、言語コンボの設定を取得するSQL
*/
-- GetLanguageList
-- 構成マスタ内の全工場
WITH factory AS(
    SELECT
        st.structure_id AS factory_id
    FROM
        ms_structure AS st
    WHERE
        st.structure_group_id = 1000    -- 場所階層
    AND st.structure_layer_no = 1       -- 工場
    AND st.delete_flg != 1
    UNION
    -- 共通工場は構成マスタに無いので追加
    SELECT
         0
)
SELECT
     ie.extension_data AS language_code
    ,si.translation_text AS language_label
    ,si.language_id
    ,coalesce(ft.factory_id, 0) AS orderFactoryId
FROM
    v_structure_item si
LEFT OUTER JOIN
    ms_item_extension ie
ON  si.structure_item_id = ie.item_id
    
-- 工場ごとに工場別表示順を取得する
CROSS JOIN factory AS ft
LEFT OUTER JOIN ms_structure_order AS order_factory
ON  si.structure_id = order_factory.structure_id
AND order_factory.factory_id = ft.factory_id
-- 全工場共通の表示順
LEFT OUTER JOIN ms_structure_order AS order_common
ON  si.structure_id = order_common.structure_id
AND order_common.factory_id = 0

WHERE
    si.structure_group_id = 9020

ORDER BY coalesce(ft.factory_id, 0),
    coalesce(coalesce(order_factory.display_order, order_common.display_order), 32768)
    ,si.structure_id