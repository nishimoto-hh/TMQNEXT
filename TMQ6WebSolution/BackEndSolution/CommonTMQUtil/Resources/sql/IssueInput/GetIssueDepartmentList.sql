--******************************************************************
--出庫一覧(部門在庫情報)
--******************************************************************
WITH department AS ( 
    --拡張データ、翻訳を取得(部門)
    SELECT
        structure_id
        , ie.extension_data
    FROM
        ms_structure ms 
        INNER JOIN ms_item_extension ie 
            ON ms.structure_item_id = ie.item_id 
            AND ms.structure_group_id = 1760 
            AND ie.sequence_no = 1
) 
, account AS ( 
    --拡張データ、翻訳を取得(勘定科目)
    SELECT
        structure_id
        , ie.extension_data
    FROM
        ms_structure ms 
        INNER JOIN ms_item_extension ie 
            ON ms.structure_item_id = ie.item_id 
            AND ms.structure_group_id = 1770
            AND ie.sequence_no = 1
) 
, number_unit AS(
    --数量管理単位
    SELECT
        ms.structure_id AS unit_id,
        ex.extension_data AS unit_digit
    FROM
        ms_structure ms
        LEFT JOIN
            ms_item_extension ex
        ON  ms.structure_item_id = ex.item_id
        AND ex.sequence_no = 2
    WHERE
        ms.structure_group_id = 1730
)
, unit_round AS(
    --丸め処理区分
    SELECT
        ms.factory_id,
        ex.extension_data AS unit_round_division
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
)
, structure_factory AS ( 
    -- 使用する構成グループの構成IDを絞込、工場の指定に用いる
    SELECT
        structure_id
        , location_structure_id AS factory_id 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id IN ( 
            1730,1760,1770
        ) 
        AND language_id = @LanguageId
)
,target AS (
SELECT DISTINCT
    plt.old_new_structure_id,                          --新旧区分ID
    dpm.extension_data AS department_cd,               --部門CD
    COALESCE(
    ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = plt.department_structure_id
                    AND st_f.factory_id IN (0, pps.factory_id)
            ) 
            AND tra.structure_id = plt.department_structure_id
    ), 
    ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId
            AND tra.location_structure_id = ( 
                SELECT
                    MIN(st_f.factory_id) 
                FROM
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = plt.department_structure_id
                    AND st_f.factory_id NOT IN (0, pps.factory_id)
            ) 
            AND tra.structure_id = plt.department_structure_id
    )) AS department_nm,  --部門名
    act.extension_data AS subject_cd,                  --勘定科目CD
    COALESCE(
    ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = plt.account_structure_id
                    AND st_f.factory_id IN (0, pps.factory_id)
            ) 
            AND tra.structure_id = plt.account_structure_id
    ), 
    ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId
            AND tra.location_structure_id = ( 
                SELECT
                    MIN(st_f.factory_id) 
                FROM
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = plt.account_structure_id
                    AND st_f.factory_id NOT IN (0, pps.factory_id)
            ) 
            AND tra.structure_id = plt.account_structure_id
    )) AS subject_nm,                                  --勘定科目名
    --SUM(pls.stock_quantity) AS inventry,               --在庫数
    pls.stock_quantity AS inventry,
    (
      SELECT
          tra.translation_text
      FROM
         v_structure_item_all AS tra
      WHERE
          tra.language_id = @LanguageId
      AND tra.location_structure_id = (
              SELECT
                  MAX(st_f.factory_id)
              FROM
                  structure_factory AS st_f
              WHERE
                  st_f.structure_id = plt.unit_structure_id
              AND st_f.factory_id IN(0, pps.factory_id)
           )
      AND tra.structure_id = plt.unit_structure_id
    ) AS unit,                                    -- 数量管理単位
    --number_unit.unit_name AS unit,                     --(数量単位名称)
    plt.old_new_structure_id AS old_new_nm,            --新旧区分(在庫一覧検索用)
    plt.department_structure_id AS to_department_nm,   --部門(在庫一覧検索用)
    plt.account_structure_id AS to_subject_nm,         --勘定科目(在庫一覧検索用)
    COALESCE(number_unit.unit_digit, 0) AS unit_digit, --小数点以下桁数(数量)
    COALESCE(unit_round.unit_round_division, 0) AS unit_round_division --丸め処理区分(数量)
FROM
    pt_lot plt 
    LEFT JOIN ( 
    SELECT
        parts_id
        , stock_quantity
        , lot_control_id
    FROM
        pt_location_stock                           --在庫データ
    WHERE
        parts_id = @PartsId 
    ) pls
    ON plt.lot_control_id = pls. lot_control_id
    AND plt.parts_id = pls.parts_id 
    LEFT JOIN pt_parts pps
        ON plt.parts_id = pps.parts_id
        AND pls.parts_id = pps.parts_id
    LEFT JOIN department dpm 
        ON plt.department_structure_id = dpm.structure_id 
    LEFT JOIN account act 
        ON plt.account_structure_id = act.structure_id 
    LEFT JOIN number_unit --数量管理単位
        ON  plt.unit_structure_id = number_unit.unit_id
    LEFT JOIN unit_round --丸め処理区分
        ON  pps.factory_id = unit_round.factory_id
WHERE
    plt.parts_id = @PartsId 
    AND pls.stock_quantity > 0                 --在庫数が0以上のものを表示
)

SELECT DISTINCT
old_new_structure_id
,department_cd
,department_nm
,subject_cd
,subject_nm
,unit
,old_new_nm
,to_department_nm
,to_subject_nm
,unit_digit
,unit_round_division
,SUM(inventry) AS inventry
FROM
 target

GROUP BY
old_new_structure_id
,department_cd
,department_nm
,subject_cd
,subject_nm
,unit
,old_new_nm
,to_department_nm
,to_subject_nm
,unit_digit
,unit_round_division
ORDER BY old_new_structure_id,to_department_nm,to_subject_nm
