--******************************************************************
--入庫一覧
--******************************************************************
SELECT DISTINCT
    FORMAT(plt.receiving_datetime, 'yyyy/MM/dd') AS receiving_datetime,             --入庫日
    plt.lot_no,                                                                     --ロットNo
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
              AND st_f.factory_id IN (0, pps.factory_id)
           )
      AND tra.structure_id = pls.parts_location_id
    ) AS parts_location_name,--棚ID(名称)
    pls.parts_location_detail_no,                                                   --棚枝番
    pls.parts_location_detail_no AS parts_location_detail_no_enter,                 --棚枝番(ラベル出力用)
    pps.parts_no,                                                                   --予備品No
    pps.parts_name,                                                                 --予備品名称
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
                  st_f.structure_id = plt.old_new_structure_id
              AND st_f.factory_id IN (0, pps.factory_id)
           )
      AND tra.structure_id = plt.old_new_structure_id
    ) AS old_new_structure_name,--新旧区分
    pps.standard_size AS dimensions,                                                --規格・寸法
    pih.inout_quantity,                                                             --受払数(画面：入庫数)
    plt.unit_price,                                                                 --入庫単価
    pih.inout_quantity * plt.unit_price AS amount_money,                            --入庫金額
    dpm.extension_data AS department_cd,                                            --部門CD
    act.extension_data AS subject_cd,                                               --勘定科目CD
    dpm.extension_data AS department_cd_enter,                                      --部門CD(ラベル出力用)
    act.extension_data AS subject_cd_enter,                                         --勘定科目CD(ラベル出力用)
    plt.management_no,                                                              --管理No
    plt.management_division,                                                        --管理区分
    pps.parts_id,                                                                   --予備品ID
    pps.factory_id AS parts_factory_id,                                             --工場ID
    COALESCE(number_unit.unit_digit, 0) AS unit_digit,                              --小数点以下桁数(数量)
    COALESCE(currency_unit.currency_digit, 0) AS currency_digit,                    --小数点以下桁数(金額)
    COALESCE(unit_round.round_division, 0) AS unit_round_division,                  --丸め処理区分(数量)
    COALESCE(unit_round.round_division, 0) AS currency_round_division,              --丸め処理区分(金額)
    '1' AS transition_flg,                                                          --遷移フラグ
    COALESCE(pps.job_structure_id, 0) AS job_structure_id ,                         --職種ID
    pps.parts_location_id,                                                          --棚ID
    pls.parts_location_id AS parts_location_id_enter,                               --棚ID(ラベル出力用)
    pih.inout_history_id,                                                           --受払履歴ID
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
                    AND st_f.factory_id IN (0, pps.factory_id)
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
                    AND st_f.factory_id NOT IN (0, pps.factory_id)
            ) 
            AND tra.structure_id = plt.department_structure_id
    ) ) as department_nm,                                                  --部門名  
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
                    AND st_f.factory_id IN (0, pps.factory_id)
            ) 
            AND tra.structure_id = plt.account_structure_id
    ) AS subject_nm,                                                    --勘定科目名
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
                    AND st_f.factory_id IN (0, pps.factory_id)
            ) 
            AND tra.structure_id = plt.unit_structure_id
    ) AS unit_name,                                                    --数量単位名称   
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
                    AND st_f.factory_id IN (0, pps.factory_id)
            ) 
            AND tra.structure_id = plt.currency_structure_id
    ) AS currency_name                                                    --金額単位名称   
FROM
    pt_parts pps                                --予備品仕様マスタ
    LEFT JOIN pt_lot plt                        --ロット情報マスタ
        ON pps.parts_id = plt.parts_id 
    LEFT JOIN pt_location_stock pls             --在庫データマスタ
        ON pps.parts_id = pls.parts_id 
    LEFT JOIN ( 
        SELECT
            pih.inout_history_id
            , pih.inout_quantity
            , pih.inventory_control_id
            , pih.lot_control_id
            , pih.inout_datetime 
        FROM
            pt_inout_history pih 
            LEFT JOIN inout_division ids 
                ON pih.inout_division_structure_id = ids.structure_id 
            LEFT JOIN work_division wds 
                ON pih.work_division_structure_id = wds.structure_id 
        WHERE
            ids.extension_data = '1' 
            AND wds.extension_data = '1'
            AND pih.delete_flg = 0
    ) pih                                       --受払履歴マスタ
        ON pls.inventory_control_id = pih.inventory_control_id 
        AND plt.lot_control_id = pih.lot_control_id 
    LEFT JOIN department dpm 
        ON plt.department_structure_id = dpm.structure_id 
    LEFT JOIN account act 
        ON plt.account_structure_id = act.structure_id 
    LEFT JOIN number_unit --数量管理単位
        ON  plt.unit_structure_id = number_unit.unit_id
    LEFT JOIN unit_round --丸め処理区分
        ON  pps.factory_id = unit_round.factory_id
    LEFT JOIN currency_unit --金額管理単位
        ON  plt.currency_structure_id = currency_unit.currency_id
 WHERE
     pih.inout_datetime >= @WorkingDay
     AND pih.inout_datetime < @WorkingDayNext     --作業日

AND EXISTS(SELECT * FROM #temp_location temp WHERE pps.factory_id = temp.structure_id)AND EXISTS(SELECT * FROM #temp_job temp WHERE COALESCE(pps.job_structure_id, 0) = temp.structure_id)
