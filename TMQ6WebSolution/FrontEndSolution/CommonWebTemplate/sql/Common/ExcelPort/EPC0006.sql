WITH factory_list AS ( 
    --工場ID取得
    SELECT
        0 AS factory_id 
    UNION ALL 
    SELECT
        structure_id AS factory_id 
    FROM
        ms_structure 
    WHERE
        structure_group_id = 1000 
        AND structure_layer_no = 1 
    /*IF param1 != 1000 && param1 != 1010 && factoryIdList != null && factoryIdList.Count > 0*/
        AND structure_id IN /*factoryIdList*/(0, 5, 6) 
    /*END*/
) 
, mq_class AS ( 
    --MQ分類
    SELECT
        st.structure_id
        , factory_list.factory_id
        , item_ex1.extension_data AS ex1
        , ex_item_1.structure_id AS budget_personality_structure_id
        , item_ex5.extension_data AS ex5
        , ( 
            SELECT
                count(tra.translation_text) 
            FROM
                v_structure_item_all AS tra 
            WHERE
                tra.language_id = /*languageId*/'ja' 
                AND tra.location_structure_id = factory_list.factory_id 
                AND tra.structure_id = st.structure_id
        ) AS mq_factory_flg                     --MQ分類
        , ( 
            SELECT
                count(tra.translation_text) 
            FROM
                v_structure_item_all AS tra 
            WHERE
                tra.language_id = /*languageId*/'ja' 
                AND tra.location_structure_id = factory_list.factory_id 
                AND tra.structure_id = ex_item_1.structure_id
        ) AS budget_factory_flg                 --予算性格区分
        , coalesce( 
            order_common.display_order
            , order_factory.display_order
            , st.structure_id
        ) AS display_order 
    FROM
        v_structure AS st 
        CROSS JOIN factory_list 
        LEFT OUTER JOIN ms_structure_order AS order_common -- 工場共通表示順
            ON st.structure_id = order_common.structure_id 
            AND order_common.factory_id = 0 
        LEFT OUTER JOIN ms_structure_order AS order_factory -- 工場別表示順
            ON st.structure_id = order_factory.structure_id 
            AND st.factory_id = order_factory.factory_id 
        LEFT OUTER JOIN ms_item_extension AS item_ex1 -- 拡張項目1
            ON st.structure_item_id = item_ex1.item_id 
            AND item_ex1.sequence_no = 1 
        LEFT OUTER JOIN v_structure AS ex_item_1 -- 拡張項目1に設定されている予算性格区分のアイテム
            ON item_ex1.extension_data = CAST(ex_item_1.structure_item_id AS VARCHAR) 
        LEFT OUTER JOIN ms_item_extension AS item_ex5 -- 拡張項目5(点検or故障)
            ON st.structure_item_id = item_ex5.item_id 
            AND item_ex5.sequence_no = 5 
    WHERE
        st.structure_group_id = /*param1*/1850 
        AND NOT ( 
            st.factory_id != 0 
            AND st.factory_id != factory_list.factory_id
        )                                       --工場個別アイテムは工場翻訳のみ
        -- 工場別未使用標準アイテムに工場が含まれていないものを表示
        AND NOT EXISTS ( 
            SELECT
                * 
            FROM
                ms_structure_unused AS unused 
            WHERE
                unused.factory_id = st.factory_id 
                AND unused.structure_id = st.structure_id
        )
) 
SELECT
    mq_class.structure_id AS id
    , mq_class.factory_id
    , mq_class.ex5 AS parent_id
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = /*languageId*/'ja' 
            AND tra.location_structure_id = ( 
                CASE 
                    WHEN mq_class.mq_factory_flg = 1 
                        THEN mq_class.factory_id 
                    ELSE 0 
                    END
            ) 
            AND tra.structure_id = mq_class.structure_id
    ) AS name                                   --MQ分類
    , mq_class.budget_personality_structure_id AS exparam1
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = /*languageId*/'ja' 
            AND tra.location_structure_id = ( 
                CASE 
                    WHEN mq_class.budget_factory_flg = 1 
                        THEN mq_class.factory_id 
                    ELSE 0 
                    END
            ) 
            AND tra.structure_id = mq_class.budget_personality_structure_id
    ) AS exparam2                               --予算性格区分
    , mq_class.display_order 
FROM
    mq_class 
WHERE
    NOT (mq_factory_flg = 0 AND budget_factory_flg = 0) 
-- 表示順は親構成ID、工場共通表示順、工場別表示順、構成IDの順
ORDER BY
    mq_class.ex5
    , mq_class.display_order
    , mq_class.factory_id
