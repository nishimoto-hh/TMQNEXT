--******************************************************************
--出庫一覧(親)
--******************************************************************
SELECT DISTINCT
    FORMAT(pih.inout_datetime, 'yyyy/MM/dd') AS inout_datetime,                    --受払日時(画面：出庫日)
    pih.work_no,                                                                   --作業No(画面：出庫No)
    pps.parts_no,                                                                  --予備品No
    pps.parts_name,                                                                --予備品名称
             ( 
        SELECT
            tra.translation_text --新旧区分
        FROM
            v_structure_item AS tra 
        WHERE
            tra.language_id = @LanguageId
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = plt.old_new_structure_id
                    AND st_f.factory_id = 0
            ) 
            AND tra.structure_id = plt.old_new_structure_id
    ) AS old_new_structure_name,
    pps.standard_size AS dimensions                                              --規格・寸法
            , ( 
        SELECT
            tra.translation_text --出庫区分
        FROM
            v_structure_item AS tra 
        WHERE
            tra.language_id = @LanguageId
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    structure_factory AS st_f 
                WHERE
                    st_f.structure_id = pih.shipping_division_structure_id
                    AND st_f.factory_id = 0
            ) 
            AND tra.structure_id = pih.shipping_division_structure_id
    ) AS shipping_division_name,
    SUM(pih.inout_quantity) AS inout_quantity,                                     --受払数(画面：出庫数)
    number_unit.unit_name AS issue_quantity,                                       --(数量単位名称)
    SUM(pih.inout_quantity * plt.unit_price) AS issue_monney,                      --受払数*入庫単価(画面：出庫金額)
    currency_unit.currency_name,                                                   --(金額単位名称)
    COALESCE(number_unit.unit_digit, 0) AS unit_digit,                             --小数点以下桁数(数量)
    COALESCE(currency_unit.currency_digit, 0) AS currency_digit,                   --小数点以下桁数(金額)
    COALESCE(unit_round.round_division, 0) AS unit_round_division,                 --丸め処理区分(数量)
    COALESCE(unit_round.round_division, 0) AS currency_round_division,             --丸め処理区分(金額)
    pps.parts_id,                                                                  --画面遷移用
    pps.job_structure_id,                                                          --職種ID
    pps.parts_location_id,                                                         --棚ID
    pps.factory_id AS parts_factory_id                                             --工場ID
FROM
    pt_parts pps                             --予備品仕様マスタ
    LEFT JOIN pt_lot plt                     --ロット情報マスタ
        ON pps.parts_id = plt.parts_id 
    LEFT JOIN pt_location_stock pls          --在庫データマスタ
        ON pps.parts_id = pls.parts_id 
    LEFT JOIN ( 
        SELECT
            inout_datetime,
            work_no,
            shipping_division_structure_id,
            inout_quantity,
            inventory_control_id,
            lot_control_id
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
            
    ) pih                                    --受払履歴マスタ
        ON pls.inventory_control_id = pih.inventory_control_id 
        AND plt.lot_control_id = pih.lot_control_id 
    LEFT JOIN number_unit --数量管理単位
        ON  plt.unit_structure_id = number_unit.unit_id
    LEFT JOIN unit_round --丸め処理区分
        ON  pps.factory_id = unit_round.factory_id
    LEFT JOIN currency_unit --金額管理単位
        ON  plt.currency_structure_id = currency_unit.currency_id
 WHERE
     pih.inout_datetime >= @WorkingDay
     AND pih.inout_datetime < @WorkingDayNext --作業日
/*@FactoryIdList
    AND pps.factory_id IN @FactoryIdList
@FactoryIdList*/
/*@JobIdList
    AND pps.job_structure_id IN @JobIdList
@JobIdList*/
GROUP BY pih.inout_datetime
         ,pih.work_no
         ,pps.parts_no
         ,pps.parts_name
         ,plt.old_new_structure_id
         ,pps.standard_size
         ,pih.shipping_division_structure_id
         ,number_unit.unit_name
         ,currency_unit.currency_name
         ,number_unit.unit_digit
         ,currency_unit.currency_digit
         ,unit_round.round_division
         ,pps.parts_id
         ,pps.job_structure_id
         ,pps.parts_location_id
         ,pps.factory_id