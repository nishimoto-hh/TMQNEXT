--******************************************************************
--出庫一覧(在庫一覧) 新規
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
        , translation_text 
    FROM
        v_structure_item_all si 
        INNER JOIN ms_item_extension ie 
            ON si.structure_item_id = ie.item_id 
            AND si.structure_group_id = 1770 
            AND ie.sequence_no = 1
    WHERE
        si.language_id = @LanguageId
) 
, location_stock AS ( 
   --在庫データ
    SELECT
        lot_control_id
        , inventory_control_id
        , parts_id
        , parts_location_id
        , parts_location_detail_no
        , stock_quantity
        , update_serialid
    FROM
        pt_location_stock
    WHERE
        parts_id = @PartsId 
) 
, location AS ( 
    --拡張データ、翻訳を取得(棚)
    SELECT
        structure_id
        , translation_text 
    FROM
        v_structure_item_all si 
    WHERE
        structure_group_id = 1040 
    AND
        structure_layer_no = 3
    AND
        si.language_id = @LanguageId
)
, number_unit AS(
    --数量管理単位
    SELECT
        structure_id AS unit_id,
        translation_text AS unit_name,
        ex.extension_data AS unit_digit
    FROM
        v_structure_item_all unit
        LEFT JOIN
            ms_item_extension ex
        ON  unit.structure_item_id = ex.item_id
        AND ex.sequence_no = 2
    WHERE
        unit.structure_group_id = 1730
    AND unit.language_id = @LanguageId
)
, unit_round AS(
    --丸め処理区分
    SELECT
        ms.factory_id,
        ex.extension_data AS round_division
    FROM
        ms_structure ms
        LEFT JOIN
            ms_item item
        ON  ms.structure_item_id = item.item_id
        AND item.delete_flg = 0
        LEFT JOIN
            ms_item_extension ex
        ON  item.item_id = ex.item_id
        AND ex.sequence_no = 1
    WHERE
        ms.structure_group_id = 2050
        AND ms.delete_flg = 0
)
, currency_unit AS(
    --金額管理単位
    SELECT
        structure_id AS currency_id
        , translation_text AS currency_name
        , ex.extension_data AS currency_digit
    FROM
        v_structure_item_all unit 
        LEFT JOIN ms_item_extension ex 
            ON unit.structure_item_id = ex.item_id 
            AND ex.sequence_no = 2 
    WHERE
        unit.structure_group_id = 1740 
        AND unit.language_id = @LanguageId
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
            1760
        ) 
        AND language_id = @LanguageId
) 
SELECT
    plt.lot_no,                                                         --ロットNo
    FORMAT(plt.receiving_datetime, 'yyyy/MM/dd') AS receiving_datetime, --入庫日
    lcn.translation_text AS parts_location_cd,                          --棚
    pls.parts_location_id,                                              --棚ID
    pls.parts_location_detail_no,                                       --棚枝番
    plt.old_new_structure_id,                                           --新旧区分ID
    plt.unit_price,                                                     --入庫単価
    currency_unit.currency_name,                                        --(金額単位名称)
    pls.stock_quantity AS inventry,                                     --在庫数
    number_unit.unit_name,                                              --(数量単位名称)
    0 AS issue_quantity,                                                --受払数(画面：出庫数)
    dpm.extension_data AS department_cd,                                --部門CD
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
    ) AS department_nm,  --部門(翻訳)
    act.extension_data AS subject_cd,                                   --勘定科目CD
    act.translation_text AS subject_nm,                                 --勘定科目名
    plt.management_no,                                                  --管理No
    plt.management_division,                                            --管理区分
    plt.lot_control_id,                                                 --ロット管理ID(登録用)
    pls.inventory_control_id,                                           --在庫管理ID(登録用)
    0 AS work_no,                                                       --作業No(登録用)
    pps.factory_id,                                                     --工場ID
    COALESCE(number_unit.unit_digit, 0) AS unit_digit,                  --小数点以下桁数(数量)
    COALESCE(currency_unit.currency_digit, 0) AS currency_digit,        --小数点以下桁数(金額)
    COALESCE(unit_round.round_division, 0) AS unit_round_division,      --丸め処理区分(数量)
    COALESCE(unit_round.round_division, 0) AS currency_round_division,  --丸め処理区分(金額)
    pls.update_serialid                                                 --更新シリアルID(排他チェック用)
FROM
    pt_lot plt 
    LEFT JOIN location_stock pls  
        ON plt.lot_control_id = pls.lot_control_id
        AND plt.parts_id = pls.parts_id 
    LEFT JOIN pt_parts pps
        ON plt.parts_id = pps.parts_id
        AND pls.parts_id = pps.parts_id
    LEFT JOIN department dpm 
        ON plt.department_structure_id = dpm.structure_id 
    LEFT JOIN account act 
        ON plt.account_structure_id = act.structure_id 
    LEFT JOIN location lcn 
        ON pls.parts_location_id = lcn.structure_id 
    LEFT JOIN number_unit --数量管理単位
        ON  plt.unit_structure_id = number_unit.unit_id
    LEFT JOIN unit_round --丸め処理区分
        ON  pps.factory_id = unit_round.factory_id
    LEFT JOIN currency_unit --金額管理単位
        ON  plt.currency_structure_id = currency_unit.currency_id
WHERE
    plt.parts_id = @PartsId 
    AND pls.stock_quantity > 0                 --在庫数が0以上のものを表示
    AND plt.old_new_structure_id = @OldNewStructureId             --新旧区分
    AND plt.department_structure_id = @DepartmentStructureId      --部門
    AND plt.account_structure_id = @AccountStructureId            --勘定科目
ORDER BY
    plt.lot_no
