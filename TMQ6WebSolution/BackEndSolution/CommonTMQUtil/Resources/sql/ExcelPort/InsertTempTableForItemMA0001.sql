-- 対象の場所階層・職種機種の構成IDを格納する一時テーブル
DROP TABLE IF EXISTS #temp_structure; 
CREATE TABLE #temp_structure(structure_id int); 
-- 対象の構成IDを一時テーブルへ保存
INSERT 
INTO #temp_structure 
SELECT
    * 
FROM
    STRING_SPLIT(@StructureIdList, ','); 


-- 対象の工場の構成IDを格納する一時テーブル
DROP TABLE IF EXISTS #temp_factory; 
CREATE TABLE #temp_factory(structure_id int); 
-- 対象の工場IDを一時テーブルへ保存
INSERT 
INTO #temp_factory 
SELECT
    * 
FROM
    STRING_SPLIT(@FactoryIdList, ','); 

WITH 
st_com(structure_layer_no, structure_id, parent_structure_id, org_structure_id) AS(
    SELECT
        st.structure_layer_no,
        st.structure_id,
        st.parent_structure_id,
        st.structure_id
    FROM
        ms_structure AS st
    WHERE
        EXISTS ( 
            SELECT
                * 
            FROM
                #temp_structure temp 
            WHERE
                st.structure_id = temp.structure_id
        ) 
    AND st.delete_flg = 0
),
rec_down(structure_layer_no, structure_id, parent_structure_id, org_structure_id) AS(
    SELECT
        com.structure_layer_no,
        com.structure_id,
        com.parent_structure_id,
        com.structure_id
    FROM
        st_com AS com
    UNION ALL
    SELECT
        b.structure_layer_no,
        b.structure_id,
        b.parent_structure_id,
        a.org_structure_id
    FROM
        rec_down AS a
        INNER JOIN
            ms_structure AS b
        ON  (
                a.structure_id = b.parent_structure_id
            )
        AND b.delete_flg = 0
),
rec_up(structure_layer_no, structure_id, parent_structure_id, org_structure_id) AS(
    SELECT
        com.structure_layer_no,
        com.structure_id,
        com.parent_structure_id,
        com.structure_id
    FROM
        st_com AS com
    UNION ALL
    SELECT
        b.structure_layer_no,
        b.structure_id,
        b.parent_structure_id,
        a.org_structure_id
    FROM
        rec_up AS a
        INNER JOIN
            ms_structure AS b
        ON  (
                b.structure_id = a.parent_structure_id
            )
        AND b.delete_flg = 0   
),
rec(structure_layer_no, structure_id, parent_structure_id, org_structure_id) AS(

    SELECT
        up.structure_layer_no,
        up.structure_id,
        up.parent_structure_id,
        up.org_structure_id
    FROM
        rec_up AS up
    UNION

    SELECT
        down.structure_layer_no,
        down.structure_id,
        down.parent_structure_id,
        down.org_structure_id
    FROM
        rec_down AS down
)
, temp_structure_all AS(
SELECT DISTINCT
     vs.structure_id
FROM
    rec AS com
    LEFT OUTER JOIN
        ms_structure AS vs
    ON  (
            com.structure_id = vs.structure_id
        )
    LEFT OUTER JOIN
        ms_structure_order AS order_factory
    ON  (
            com.structure_id = order_factory.structure_id
        AND order_factory.factory_id = vs.factory_id
        )
    LEFT OUTER JOIN
        ms_structure_order AS order_common
    ON  (
            com.structure_id = order_common.structure_id
        AND order_common.factory_id = 0
        )
)
,target_factory AS ( -- ExcelPort対象工場を取得
    SELECT
        ms.structure_id 
    FROM
        ms_structure ms 
        INNER JOIN #temp_factory tf
            ON ms.structure_id = tf.structure_id
        LEFT JOIN ms_item_extension ex 
            ON ms.structure_item_id = ex.item_id 
            AND ex.sequence_no = 5 
    WHERE
        ms.structure_group_id = 1000 
        AND ms.structure_layer_no = 1 
        AND ms.delete_flg = 0 
        AND ex.extension_data IS NOT NULL 
    UNION 
    SELECT
        0
) 
, base AS ( -- 対象アイテムを取得(EPC0001、EPC0005から取得できるアイテム)
    SELECT DISTINCT
        CASE 
            WHEN item.structure_group_id = 2120 
                THEN 0 
            WHEN item.structure_group_id = 1000 
            AND item.structure_layer_no = 0 
                THEN 1 
            WHEN item.structure_group_id = 1000 
            AND item.structure_layer_no = 1 
                THEN 2 
            WHEN item.structure_group_id = 1000 
            AND item.structure_layer_no = 2 
                THEN 3 
            WHEN item.structure_group_id = 1000 
            AND item.structure_layer_no = 3 
                THEN 4 
            WHEN item.structure_group_id = 1000 
            AND item.structure_layer_no = 4 
                THEN 5 
            WHEN item.structure_group_id = 1000 
            AND item.structure_layer_no = 5 
                THEN 6 
            WHEN item.structure_group_id = 1010 
            AND item.structure_layer_no = 0 
                THEN 7 
            WHEN item.structure_group_id = 1530 
                THEN 8 
            WHEN item.structure_group_id = 1380 
                THEN 10 
            WHEN item.structure_group_id = 1300 
                THEN 11 
            WHEN item.structure_group_id = 1400 
                THEN 12 
            WHEN item.structure_group_id = 1130 
                THEN 13 
            WHEN item.structure_group_id = 1090 
                THEN 14 
            WHEN item.structure_group_id = 1100 
                THEN 15 
            WHEN item.structure_group_id = 1070 
                THEN 16 
            WHEN item.structure_group_id = 1080 
                THEN 17 
            WHEN item.structure_group_id = 1420 
                THEN 18 
            WHEN item.structure_group_id = 1930 
                THEN 20 
            WHEN item.structure_group_id = 1430 
                THEN 21 
            WHEN item.structure_group_id = 1440 
                THEN 22 
            WHEN item.structure_group_id = 1330 
                THEN 23 
            WHEN item.structure_group_id = 1110 
                THEN 24 
            WHEN item.structure_group_id = 1140 
                THEN 25 
            WHEN item.structure_group_id = 1550 
                THEN 26 
            WHEN item.structure_group_id = 1480 
                THEN 27 
            WHEN item.structure_group_id = 1490 
                THEN 28 
            WHEN item.structure_group_id = 2130 
                THEN 29 
            ELSE NULL 
            END ep_select_group_id
        , CASE 
            WHEN item.structure_group_id IN (2120,1530,1110,2130) 
                THEN CONVERT(INT, item_ex1.extension_data) 
            ELSE item.structure_id 
            END structure_id
        , CASE 
            WHEN item.factory_id = 0 
                THEN item.location_structure_id -- 構成マスタの工場IDが0の場合は翻訳マスタの場所階層ID(工場ID)を返す
            ELSE coalesce(item.factory_id, 0)   -- 上記以外は構成マスタの工場ID
            END AS factory_id
        , item.parent_structure_id
        , item.translation_text
        , item_ex1.extension_data AS exparam1
        , item_ex2.extension_data AS exparam2
        , item_ex3.extension_data AS exparam3
        , item_ex4.extension_data AS exparam4
        , item_ex5.extension_data AS exparam5
        , item_ex6.extension_data AS exparam6
        , NULL AS exparam7
        , NULL AS exparam8
        , NULL AS exparam9
        , NULL AS exparam10
        , NULL AS exparam11
        , NULL AS exparam12
        , NULL AS exparam13
        , NULL AS exparam14
        , NULL AS exparam15
        , NULL AS exparam16
        , NULL AS exparam17
        , NULL AS exparam18
        , NULL AS exparam19
        , NULL AS exparam20
        , unused_factory.unuse_factory_id
        , coalesce( 
            order_common.display_order
            , order_factory.display_order
            , item.structure_id
        ) AS display_order
        , item.structure_group_id
        , item.structure_layer_no 
    FROM
        v_structure_item AS item                -- 工場共通表示順
        LEFT OUTER JOIN ms_structure_order AS order_common 
            ON item.structure_id = order_common.structure_id 
            AND order_common.factory_id = 0     -- 工場別表示順
        LEFT OUTER JOIN ms_structure_order AS order_factory 
            ON item.structure_id = order_factory.structure_id 
            AND item.factory_id = order_factory.factory_id -- 拡張項目
        LEFT OUTER JOIN ms_item_extension AS item_ex1 
            ON item.structure_item_id = item_ex1.item_id 
            AND item_ex1.sequence_no = 1 
        LEFT OUTER JOIN ms_item_extension AS item_ex2 
            ON item.structure_item_id = item_ex2.item_id 
            AND item_ex2.sequence_no = 2 
        LEFT OUTER JOIN ms_item_extension AS item_ex3 
            ON item.structure_item_id = item_ex3.item_id 
            AND item_ex3.sequence_no = 3 
        LEFT OUTER JOIN ms_item_extension AS item_ex4 
            ON item.structure_item_id = item_ex4.item_id 
            AND item_ex4.sequence_no = 4 
        LEFT OUTER JOIN ms_item_extension AS item_ex5 
            ON item.structure_item_id = item_ex5.item_id 
            AND item_ex5.sequence_no = 5 
        LEFT OUTER JOIN ms_item_extension AS item_ex6 
            ON item.structure_item_id = item_ex6.item_id 
            AND item_ex6.sequence_no = 6 
        LEFT OUTER JOIN ( 
            SELECT
                unuse_a.structure_id
                , trim( 
                    '|' 
                    FROM
                        ( 
                            SELECT
                                CAST(unuse_b.factory_id AS VARCHAR) + '|' 
                            FROM
                                ms_structure_unused unuse_b 
                            WHERE
                                unuse_b.structure_group_id IN ( 
                                    1000
                                    , 1010
                                    , 1380
                                    , 1300
                                    , 1400
                                    , 1130
                                    , 1090
                                    , 1100
                                    , 1070
                                    , 1080
                                    , 1420
                                    , 1930
                                    , 1430
                                    , 1440
                                    , 1330
                                    , 1140
                                    , 1550
                                    , 1480
                                    , 1490
                                    , 2120
                                    , 1530
                                    , 1110
                                    , 2130
                                ) 
                                AND unuse_b.structure_id = unuse_a.structure_id FOR XML PATH ('')
                        )
                ) AS unuse_factory_id 
            FROM
                ms_structure_unused unuse_a 
            WHERE
                unuse_a.structure_group_id IN ( 
                    1000
                    , 1010
                    , 1380
                    , 1300
                    , 1400
                    , 1130
                    , 1090
                    , 1100
                    , 1070
                    , 1080
                    , 1420
                    , 1930
                    , 1430
                    , 1440
                    , 1330
                    , 1140
                    , 1550
                    , 1480
                    , 1490
                    , 2120
                    , 1530
                    , 1110
                    , 2130
                ) 
            GROUP BY
                unuse_a.structure_id
        ) unused_factory                        -- 標準アイテム未使用工場
            ON item.structure_id = unused_factory.structure_id 
    WHERE
        item.structure_group_id IN ( 
            1000
            , 1010
            , 1380
            , 1300
            , 1400
            , 1130
            , 1090
            , 1100
            , 1070
            , 1080
            , 1420
            , 1930
            , 1430
            , 1440
            , 1330
            , 1140
            , 1550
            , 1480
            , 1490
            , 2120
            , 1530
            , 1110
            , 2130
        ) 
        AND item.language_id = @LanguageId
        AND item.factory_id IN (SELECT * FROM target_factory) 
        AND item.location_structure_id IN (SELECT * FROM target_factory) -- 工場別未使用標準アイテムに工場が含まれていないものを表示
        AND NOT EXISTS ( 
            SELECT
                * 
            FROM
                ms_structure_unused AS unused 
            WHERE
                unused.structure_id = item.structure_id 
                AND unused.factory_id = item.factory_id
        ) -- 表示順は親構成ID、工場共通表示順、工場別表示順、構成IDの順

        -- 職種機種で取得するのは一番上の階層のみ(機種大分類・機種中分類・機種小分類は取得しない)
        AND ( 
            item.structure_group_id != 1010 
            OR ( 
                item.structure_group_id = 1010 
                AND item.structure_layer_no = 0
            )
        )
) 
, mq_class AS (  -- 対象アイテムを取得(EPC0006から取得できるアイテム)
    --MQ分類
    SELECT
        st.structure_id
        , target_factory.structure_id AS factory_id
        , item_ex1.extension_data AS ex1
        , ex_item_1.structure_id AS budget_personality_structure_id
        , item_ex5.extension_data AS ex5
        , ( 
            SELECT
                count(tra.translation_text) 
            FROM
                v_structure_item_all AS tra 
            WHERE
                tra.language_id = @LanguageId
                AND tra.location_structure_id = target_factory.structure_id 
                AND tra.structure_id = st.structure_id
        ) AS mq_factory_flg                     --MQ分類
        , ( 
            SELECT
                count(tra.translation_text) 
            FROM
                v_structure_item_all AS tra 
            WHERE
                tra.language_id = @LanguageId
                AND tra.location_structure_id = target_factory.structure_id 
                AND tra.structure_id = ex_item_1.structure_id
        ) AS budget_factory_flg                 --予算性格区分
        , coalesce( 
            order_common.display_order
            , order_factory.display_order
            , st.structure_id
        ) AS display_order 
    FROM
        v_structure AS st 
        CROSS JOIN target_factory 
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
            AND ex_item_1.structure_group_id = 1060 
        LEFT OUTER JOIN ms_item_extension AS item_ex5 -- 拡張項目5(点検or故障)
            ON st.structure_item_id = item_ex5.item_id 
            AND item_ex5.sequence_no = 5 
    WHERE
        st.structure_group_id = 1850 
        AND NOT ( 
            st.factory_id != 0 
            AND st.factory_id != target_factory.structure_id
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
, base2 AS (  -- 対象アイテムを取得(上記の「mq_class」を一時テーブルに格納するデータに加工)
    SELECT
        mq_class.structure_id
        , mq_class.factory_id
        , mq_class.ex5 AS parent_structure_id
        , ( 
            SELECT
                tra.translation_text 
            FROM
                v_structure_item_all AS tra 
            WHERE
                tra.language_id = @LanguageId
                AND tra.location_structure_id = ( 
                    CASE 
                        WHEN mq_class.mq_factory_flg = 1 
                            THEN mq_class.factory_id 
                        ELSE 0 
                        END
                ) 
                AND tra.structure_id = mq_class.structure_id
        ) AS name                               --MQ分類
        , mq_class.budget_personality_structure_id AS exparam1
        , ( 
            SELECT
                tra.translation_text 
            FROM
                v_structure_item_all AS tra 
            WHERE
                tra.language_id = @LanguageId
                AND tra.location_structure_id = ( 
                    CASE 
                        WHEN mq_class.budget_factory_flg = 1 
                            THEN mq_class.factory_id 
                        ELSE 0 
                        END
                ) 
                AND tra.structure_id = mq_class.budget_personality_structure_id
        ) AS exparam2                           --予算性格区分
        , mq_class.display_order 
    FROM
        mq_class 
    WHERE
        NOT (mq_factory_flg = 0 AND budget_factory_flg = 0)
) 
, select_user AS (  -- 対象アイテムを取得(EPC0007から取得できるアイテム)
    SELECT
        19 AS ep_select_group_id
        , users.user_id AS structure_id         -- 担当者ID(ユーザID)
        , 0 AS factory_id
        , NULL AS parent_structure_id
        , users.display_name AS name            -- 担当者名(ユーザ表示名)
        , users.display_name AS display_order 
    FROM
        ms_user users 
    WHERE
        EXISTS ( 
            SELECT
                * 
            FROM
                ( 
                    SELECT DISTINCT
                        user_id 
                    FROM
                        ms_user_belong belong 
                    WHERE
                        EXISTS ( 
                            SELECT
                                * 
                            FROM
                                ms_user_belong my_belong 
                            WHERE
                                belong.location_structure_id = my_belong.location_structure_id 
                                AND my_belong.user_id = @UserId
                        )
                ) belong 
            WHERE
                users.user_id = belong.user_id
        ) 
        AND users.delete_flg = 0
) 
, base3 AS ( 
    -- 場所階層・職種機種のみ
    SELECT
        ep_select_group_id
        , structure_id
        , parent_structure_id
        , translation_text
        , factory_id
        , exparam1
        , exparam2
        , exparam3
        , exparam4
        , exparam5
        , exparam6
        , exparam7
        , exparam8
        , exparam9
        , exparam10
        , exparam11
        , exparam12
        , exparam13
        , exparam14
        , exparam15
        , exparam16
        , exparam17
        , exparam18
        , exparam19
        , exparam20
        , unuse_factory_id
        , display_order 
        , NULL AS display_order2
    FROM
        base
    WHERE
        ( 
            structure_group_id IN (1000, 1010) 
            AND structure_id IN (SELECT structure_id FROM temp_structure_all)
        ) 
    -- 場所階層・職種機種以外のデータ
    UNION
    SELECT
        ep_select_group_id
        , structure_id
        , parent_structure_id
        , translation_text
        , factory_id
        , exparam1
        , exparam2
        , exparam3
        , exparam4
        , exparam5
        , exparam6
        , exparam7
        , exparam8
        , exparam9
        , exparam10
        , exparam11
        , exparam12
        , exparam13
        , exparam14
        , exparam15
        , exparam16
        , exparam17
        , exparam18
        , exparam19
        , exparam20
        , unuse_factory_id
        , display_order 
        , NULL AS display_order2
    FROM
        base
    WHERE
        structure_group_id NOT IN (1000, 1010)
    -- MQ分類のデータ
    UNION 
    SELECT
        9 AS ep_select_group_id
        , structure_id
        , parent_structure_id
        , name
        , factory_id
        , exparam1
        , exparam2
        , NULL AS exparam3
        , NULL AS exparam4
        , NULL AS exparam5
        , NULL AS exparam6
        , NULL AS exparam7
        , NULL AS exparam8
        , NULL AS exparam9
        , NULL AS exparam10
        , NULL AS exparam11
        , NULL AS exparam12
        , NULL AS exparam13
        , NULL AS exparam14
        , NULL AS exparam15
        , NULL AS exparam16
        , NULL AS exparam17
        , NULL AS exparam18
        , NULL AS exparam19
        , NULL AS exparam20
        , NULL AS unuse_factory_id
        , display_order 
        , NULL AS display_order2
    FROM
        base2 
    -- ユーザー選択用
    UNION 
    SELECT
        ep_select_group_id
        , structure_id
        , parent_structure_id
        , name
        , factory_id
        , NULL AS exparam1
        , NULL AS exparam2
        , NULL AS exparam3
        , NULL AS exparam4
        , NULL AS exparam5
        , NULL AS exparam6
        , NULL AS exparam7
        , NULL AS exparam8
        , NULL AS exparam9
        , NULL AS exparam10
        , NULL AS exparam11
        , NULL AS exparam12
        , NULL AS exparam13
        , NULL AS exparam14
        , NULL AS exparam15
        , NULL AS exparam16
        , NULL AS exparam17
        , NULL AS exparam18
        , NULL AS exparam19
        , NULL AS exparam20
        , NULL AS unuse_factory_id
        , NULL AS display_order 
        , display_order AS display_order2
    FROM
        select_user
) 
-- 一時テーブルに登録
INSERT 
INTO #sheet_item 
SELECT
    ep_select_group_id
    , structure_id
    , parent_structure_id
    , translation_text
    , factory_id
    , exparam1
    , exparam2
    , exparam3
    , exparam4
    , exparam5
    , exparam6
    , exparam7
    , exparam8
    , exparam9
    , exparam10
    , exparam11
    , exparam12
    , exparam13
    , exparam14
    , exparam15
    , exparam16
    , exparam17
    , exparam18
    , exparam19
    , exparam20
    , unuse_factory_id 
FROM
    base3 
ORDER BY
    ep_select_group_id
    , parent_structure_id
    , factory_id
    , display_order
	, display_order2
    , structure_id