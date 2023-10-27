/* オートコンプリート
* 予備品　棚のオートコンプリート(factoryIdList未使用)
*/
-- A0011

WITH max_factory AS -- 構成IDに対する翻訳の工場IDを取得(ユーザーの本務工場IDと「0」の最大)
(
SELECT
    item.structure_id,
    MAX(item.location_structure_id) AS location_structure_id
FROM
    v_structure_item_all item
WHERE
    item.structure_group_id = 1040
AND item.structure_layer_no = 3
AND (item.location_structure_id IN(
        SELECT
            dbo.get_target_layer_id(mub.location_structure_id, 1)
        FROM
            ms_user_belong mub
        WHERE
            mub.duty_flg = 1
        AND mub.user_id = /*userId*/1001
        UNION
        SELECT
            0
    )
OR item.location_structure_id = item.factory_id)
GROUP BY
    item.structure_id
)
,main AS
(
SELECT
    -- 工場ID
    0 AS factoryId,
    -- 翻訳工場ID
    0 AS translationFactoryId,
    -- コード
    item.structure_id AS id,
    -- 名称
    item.translation_text AS name,
    -- 構成ID
    item.structure_id AS structureId,
    -- 表示順(棚の名称)
    row_number() over(ORDER BY coalesce(item.translation_text, ''), item.structure_id) row_num
FROM
    v_structure_item item
    INNER JOIN
        max_factory
    ON  item.structure_id = max_factory.structure_id
    AND item.location_structure_id = max_factory.location_structure_id
WHERE
    item.structure_group_id = 1040
AND item.language_id =/*languageId*/'ja'
AND item.structure_layer_no = 3
AND item.parent_structure_id =/*param1*/0
    /*IF param2 != null && param2 != ''*/
    /*IF !getNameFlg */
    -- 翻訳で検索
    AND (item.translation_text LIKE /*param2*/'%')
    /*END*/
    /*END*/
    /*IF getNameFlg */
    -- 翻訳なのでID
    AND CONVERT(nvarchar, item.structure_id) = CONVERT(nvarchar, /*param2*/0)
    /*END*/
)

SELECT
    factoryId,
    translationFactoryId,
    id AS 'values',
    name AS 'labels',
    structureId AS exparam1
FROM
    main
WHERE
    /*IF rowLimit != null && rowLimit != ''*/
    row_num < /*rowLimit*/30
    /*END*/