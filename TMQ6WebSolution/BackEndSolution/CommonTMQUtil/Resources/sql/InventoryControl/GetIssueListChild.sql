--******************************************************************
--出庫一覧(子)
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
,account AS ( 
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
, inout_division AS ( 
    --ビューより受払区分を取得
    SELECT
        structure_id
        , ie.extension_data
    FROM
        ms_structure AS ms 
        INNER JOIN ms_item_extension AS ie 
            ON ms.structure_item_id = ie.item_id 
            AND ms.structure_group_id = 1950
) 
, work_division AS ( 
    --ビューより作業区分を取得
    SELECT
        structure_id
        , ie.extension_data
    FROM
        ms_structure AS ms 
        INNER JOIN ms_item_extension AS ie 
            ON ms.structure_item_id = ie.item_id 
            AND ms.structure_group_id = 1960 
) 
, number_unit AS(
    --数量管理単位
    SELECT
        structure_id AS unit_id,
        ex.extension_data AS unit_digit
    FROM
        ms_structure unit
        LEFT JOIN
            ms_item_extension ex
        ON  unit.structure_item_id = ex.item_id
        AND ex.sequence_no = 2
    WHERE
        unit.structure_group_id = 1730
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
        , ex.extension_data AS currency_digit
    FROM
        ms_structure unit 
        LEFT JOIN ms_item_extension ex 
            ON unit.structure_item_id = ex.item_id 
            AND ex.sequence_no = 2 
    WHERE
        unit.structure_group_id = 1740 
)
SELECT DISTINCT
    FORMAT(plt.receiving_datetime, 'yyyy/MM/dd') AS receiving_datetime,              --入庫日
    plt.lot_no,                                                                      --ロットNo(画面：入庫No)
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
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = pls.parts_location_id
                AND st_f.factory_id IN(0, ppt.factory_id)
            )
        AND tra.structure_id = pls.parts_location_id
    ) AS  parts_location_name, --棚ID
    pls.parts_location_detail_no,                                                    --棚枝番
    FORMAT(plt.unit_price, '#,###') AS unit_price,                                   --入庫単価
    FORMAT(plt.unit_price, '#,###') AS unit_price_value,                                   --入庫単価
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = plt.currency_structure_id
                    AND st_f.factory_id IN (0, ppt.factory_id)
            ) 
            AND tra.structure_id = plt.currency_structure_id
    ) AS currency_name,                                                              --金額単位名称
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = plt.currency_structure_id
                    AND st_f.factory_id IN (0, ppt.factory_id)
            ) 
            AND tra.structure_id = plt.currency_structure_id
    ) AS currency_name_child,                                                              --金額単位名称  
    pih.inout_quantity,                                                              --出庫数
    pih.inout_quantity AS inout_quantity_value_child,                                --出庫数
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = plt.unit_structure_id
                    AND st_f.factory_id IN (0, ppt.factory_id)
            ) 
            AND tra.structure_id = plt.unit_structure_id
    ) AS unit_name,                                                                  --数量単位名称
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = plt.unit_structure_id
                    AND st_f.factory_id IN (0, ppt.factory_id)
            ) 
            AND tra.structure_id = plt.unit_structure_id
    ) AS unit_name_child,                                                                  --数量単位名称(帳票用)
    FORMAT(pih.inout_quantity * plt.unit_price, '#,###') AS amount_money,            --受払数*入庫単価(画面：出庫金額)
    FORMAT(pih.inout_quantity * plt.unit_price, '#,###') AS issue_monney_child_value,            --受払数*入庫単価(画面：出庫金額)
    dpm.extension_data AS department_cd,                                             --部門CD
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
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = plt.department_structure_id
                AND st_f.factory_id IN(0, ppt.factory_id)
            )
        AND tra.structure_id = plt.department_structure_id
    ) ,
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
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = plt.department_structure_id
                AND st_f.factory_id NOT IN(0, ppt.factory_id)
            )
        AND tra.structure_id = plt.department_structure_id
    ) 
    ) AS department_nm,
    act.extension_data AS subject_cd,                                                --勘定科目CD
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
                    #temp_structure_factory AS st_f
                WHERE
                    st_f.structure_id = plt.account_structure_id
                AND st_f.factory_id IN(0, ppt.factory_id)
            )
        AND tra.structure_id = plt.account_structure_id
    ) AS subject_nm,                                                                 --勘定科目名
    plt.management_no,                                                               --管理No
    plt.management_division,                                                         --管理区分
    pih.work_no,                                                                     --作業No(紐付け用)
    CONVERT(varchar, pih.work_no) + '_' + CONVERT(varchar, plt.old_new_structure_id) AS nest_key,
    pih.inout_datetime,                                                              --受払日時(ソート用)
    ppt.factory_id AS parts_factory_id,                                              --工場ID
    COALESCE(number_unit.unit_digit, 0) AS unit_digit,                               --小数点以下桁数(数量)
    COALESCE(currency_unit.currency_digit, 0) AS currency_digit,                     --小数点以下桁数(金額)
    COALESCE(unit_round.round_division, 0) AS unit_round_division,                   --丸め処理区分(数量)
    COALESCE(unit_round.round_division, 0) AS currency_round_division,               --丸め処理区分(金額)
    pih.inout_history_id                                                             --受払履歴ID
FROM
    pt_parts ppt                              --予備品仕様マスタ
    LEFT JOIN pt_lot plt                      --ロット情報マスタ
        ON ppt.parts_id = plt.parts_id 
    LEFT JOIN pt_location_stock pls           --在庫データマスタ
        ON ppt.parts_id = pls.parts_id 
    LEFT JOIN ( 
        SELECT
           pih.inout_history_id,
           pih.inout_quantity,
           pih.work_no,
           pih.inout_datetime,
           pih.inventory_control_id,
           pih.lot_control_id
        FROM
            pt_inout_history pih
            LEFT JOIN inout_division ids 
                ON pih.inout_division_structure_id = ids.structure_id 
            LEFT JOIN work_division wds 
                ON pih.work_division_structure_id = wds.structure_id 
        WHERE
            ids.extension_data = '2' 
            AND wds.extension_data = '2'
            AND pih.delete_flg = 0
    ) pih                                     --受払履歴マスタ
        ON pls.inventory_control_id = pih.inventory_control_id 
        AND plt.lot_control_id = pih.lot_control_id 
    LEFT JOIN department dpm 
        ON plt.department_structure_id = dpm.structure_id 
    LEFT JOIN account act 
        ON plt.account_structure_id = act.structure_id 
    LEFT JOIN number_unit --数量管理単位
        ON  plt.unit_structure_id = number_unit.unit_id
    LEFT JOIN unit_round --丸め処理区分
        ON  ppt.factory_id = unit_round.factory_id
    LEFT JOIN currency_unit --金額管理単位
        ON  plt.currency_structure_id = currency_unit.currency_id
WHERE
    pih.work_no IN @WorkNoList                --親の出庫No
ORDER BY 
    inout_datetime DESC
