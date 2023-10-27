WITH unit_digit AS -- 小数点以下桁数(数値)
(
    SELECT
        ms.structure_id,
        ex.extension_data
    FROM
        ms_structure ms
        LEFT JOIN
            ms_item_extension ex
        ON  ms.structure_item_id = ex.item_id
        AND ex.sequence_no = 2
    WHERE
        ms.structure_group_id = 1730
),
round_division AS( -- 丸め処理区分
    SELECT
        ms.factory_id,
        ex.extension_data
    FROM
        (
            SELECT
                ms.factory_id,
                MAX(ms.structure_id) AS structure_id
            FROM
                ms_structure ms
            WHERE
                ms.structure_group_id = 2050
            GROUP BY
                ms.factory_id
        ) ms
        LEFT JOIN
            (
                SELECT
                    ms.structure_id,
                    ex.extension_data
                FROM
                    ms_structure ms
                    LEFT JOIN
                        ms_item_extension ex
                    ON  ms.structure_item_id = ex.item_id
                    AND ex.sequence_no = 1
                WHERE
                    ms.structure_group_id = 2050
            ) ex
        ON  ms.structure_id = ex.structure_id
),
currency_digit as -- 小数点以下桁数(金額)
(
    SELECT
        ms.structure_id AS currency_id,
        ex.extension_data AS currency_digit
    FROM
        ms_structure ms
        LEFT JOIN
            ms_item_extension ex
        ON  ms.structure_item_id = ex.item_id
        AND ex.sequence_no = 2
    WHERE
        ms.structure_group_id = 1740
),
department AS -- 部門
(
    SELECT
        ms.structure_id AS department_id,
        ex.extension_data AS department_code,
        ex2.extension_data AS department_flg
    FROM
        ms_structure ms
        LEFT JOIN
            ms_item_extension ex
        ON  ms.structure_item_id = ex.item_id
        AND ex.sequence_no = 1
        LEFT JOIN
            ms_item_extension ex2
        ON  ms.structure_item_id = ex2.item_id
        AND ex2.sequence_no = 2
    WHERE
        ms.structure_group_id = 1760
),
subject AS -- 勘定科目
(
    SELECT
        ms.structure_id AS subject_id,
        ex.extension_data AS subject_code
    FROM
        ms_structure ms
        LEFT JOIN
            ms_item_extension ex
        ON  ms.structure_item_id = ex.item_id
        AND ex.sequence_no = 1
    WHERE
        ms.structure_group_id = 1770
),
inout_div AS -- 受払区分
(
    SELECT
        ms.structure_id AS inout_id,
        ex.extension_data AS inout_code
    FROM
        ms_structure ms
        LEFT JOIN
            ms_item_extension ex
        ON  ms.structure_item_id = ex.item_id
        AND ex.sequence_no = 1
    WHERE
        ms.structure_group_id = 1950
)
,structure_factory as( SELECT
        structure_id
        , location_structure_id AS factory_id 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id IN (1000, 1040, 1150, 1720, 1730,1740, 1760, 1770) 
        AND language_id = @LanguageId
)
, department_trans AS( -- 部門の翻訳を取得
SELECT DISTINCT
    department_trans.structure_id AS department_id,
    coalesce(common_user.translation_text, except_common_user.translation_text) AS department_name
FROM
    v_structure_item_all department_trans
    -- 構成IDに対する共通工場ID「0」とユーザーの工場IDのうち、最大値の翻訳IDを取得
    LEFT JOIN
        (
            SELECT
                parent.structure_id,
                parent.translation_text
            FROM
                v_structure_item_all parent
                INNER JOIN
                    (
                        SELECT
                            item.structure_id,
                            MAX(item.location_structure_id) AS location_structure_id
                        FROM
                            v_structure_item_all item
                        WHERE
                            item.structure_group_id = 1760
                        AND item.language_id = @LanguageId
                        AND item.location_structure_id IN(0, @UserFactoryId)
                        GROUP BY
                            item.structure_id
                    ) child
                ON  parent.structure_id = child.structure_id
                AND parent.location_structure_id = child.location_structure_id
            WHERE
                parent.language_id = @LanguageId
        ) common_user
    ON  department_trans.structure_id = common_user.structure_id
        -- 構成IDに対する共通工場ID「0」とユーザーの工場ID以外で、最小値の翻訳IDを取得
    LEFT JOIN
        (
            SELECT
                parent.structure_id,
                parent.translation_text
            FROM
                v_structure_item_all parent
                INNER JOIN
                    (
                        SELECT
                            item.structure_id,
                            MIN(item.location_structure_id) AS location_structure_id
                        FROM
                            v_structure_item_all item
                        WHERE
                            item.structure_group_id = 1760
                        AND item.language_id = @LanguageId
                        AND item.location_structure_id NOT IN(0, @UserFactoryId)
                        GROUP BY
                            item.structure_id
                    ) child
                ON  parent.structure_id = child.structure_id
                AND parent.location_structure_id = child.location_structure_id
            WHERE
                parent.language_id = @LanguageId
        ) except_common_user
    ON  department_trans.structure_id = except_common_user.structure_id
WHERE
    department_trans.structure_group_id = 1760
AND department_trans.language_id = @LanguageId
)
,rack_trans AS( -- 棚の翻訳を取得
SELECT DISTINCT
    rack_trans.structure_id AS rack_id,
    coalesce(common_user.translation_text, except_common_user.translation_text) AS rack_name
FROM
    v_structure_item_all rack_trans
    -- 構成IDに対する共通工場ID「0」とユーザーの工場IDのうち、最大値の翻訳IDを取得
    LEFT JOIN
        (
            SELECT
                parent.structure_id,
                parent.translation_text
            FROM
                v_structure_item_all parent
                INNER JOIN
                    (
                        SELECT
                            item.structure_id,
                            MAX(item.location_structure_id) AS location_structure_id
                        FROM
                            v_structure_item_all item
                        WHERE
                            item.structure_group_id = 1040
                        AND item.structure_layer_no = 3
                        AND item.language_id = @LanguageId
                        AND item.location_structure_id IN(0, @UserFactoryId)
                        GROUP BY
                            item.structure_id
                    ) child
                ON  parent.structure_id = child.structure_id
                AND parent.location_structure_id = child.location_structure_id
            WHERE
                parent.language_id = @LanguageId
        ) common_user
    ON  rack_trans.structure_id = common_user.structure_id
        -- 構成IDに対する共通工場ID「0」とユーザーの工場ID以外で、最小値の翻訳IDを取得
    LEFT JOIN
        (
            SELECT
                parent.structure_id,
                parent.translation_text
            FROM
                v_structure_item_all parent
                INNER JOIN
                    (
                        SELECT
                            item.structure_id,
                            MIN(item.location_structure_id) AS location_structure_id
                        FROM
                            v_structure_item_all item
                        WHERE
                            item.structure_group_id = 1040
                        AND item.structure_layer_no = 3
                        AND item.language_id = @LanguageId
                        AND item.location_structure_id NOT IN(0, @UserFactoryId)
                        GROUP BY
                            item.structure_id
                    ) child
                ON  parent.structure_id = child.structure_id
                AND parent.location_structure_id = child.location_structure_id
            WHERE
                parent.language_id = @LanguageId
        ) except_common_user
    ON  rack_trans.structure_id = except_common_user.structure_id
WHERE
    rack_trans.structure_group_id = 1040
AND rack_trans.structure_layer_no = 3
AND rack_trans.language_id = @LanguageId
)