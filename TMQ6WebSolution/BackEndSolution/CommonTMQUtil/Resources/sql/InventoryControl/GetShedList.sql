--******************************************************************
--棚番移庫一覧
--******************************************************************
SELECT DISTINCT
    FORMAT(ts.inout_datetime, 'yyyy/MM/dd') AS relocation_date --移庫日
    , ts.work_no AS transfer_no                                --移庫No.
    , pp.parts_no                                             --予備品No.
    , pp.parts_name                                           --予備品名
    , ts.parts_location_id AS to_storage_location_id             --移庫先 予備品倉庫
    , ( 
        SELECT
            tra.translation_text
        FROM
            v_structure_item AS tra 
        WHERE
            tra.language_id = @LanguageId
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = ts.parts_location_id
                    AND st_f.factory_id  IN (0, pp.factory_id)
            ) 
            AND tra.structure_id = ts.parts_location_id
    ) AS to_location_id                           --移庫先 棚番
    , ts.parts_location_id AS parts_location_id_enter --移庫先 棚番(ラベル出力用)
    , ts.parts_location_detail_no AS to_parts_location_detail_no --移庫先 棚枝番
    , ts.parts_location_detail_no AS parts_location_detail_no_enter --移庫先 棚枝番(ラベル出力用)
    , td.parts_location_id AS storage_location_id --移庫先 予備品倉庫
    , ( 
        SELECT
            tra.translation_text
        FROM
            v_structure_item AS tra 
        WHERE
            tra.language_id = @LanguageId
            AND tra.location_structure_id = ( 
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = td.parts_location_id
                    AND st_f.factory_id  IN (0, pp.factory_id)
            ) 
            AND tra.structure_id = td.parts_location_id
    ) AS location_id                           --移庫先 棚番
    , td.parts_location_detail_no                 --移庫先 棚枝番
            , ( 
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = pl.old_new_structure_id
                    AND st_f.factory_id = 0
            ) 
            AND tra.structure_id = pl.old_new_structure_id
    ) AS old_new_structure_name
    , pp.standard_size AS dimensions                          --規格・寸法
    , ts.inout_quantity AS transfer_count                      --移庫数
    , pl.lot_no                                 --ロットNo
    , pl.unit_price
    , ts.inout_quantity * pl.unit_price AS transfer_amount
    , dp.extension_data AS department_cd        --部門CD
    , sus.extension_data AS subject_cd          --勘定科目CD
    , dp.extension_data AS department_cd_enter  --部門CD(ラベル出力用)
    , sus.extension_data AS subject_cd_enter    --勘定科目CD(ラベル出力用)
    , pl.management_no                          --管理No
    , pl.management_division                    --管理区分
    , pp.parts_id                               --予備品ID
    , ts.work_no                               --作業No
    , COALESCE(number_unit.unit_digit, 0) AS unit_digit --小数点以下桁数
    , COALESCE(currency_unit.currency_digit, 0) AS currency_digit --小数点以下桁数
    , COALESCE(unit_round.round_division, 0) AS unit_round_division                  --丸め処理区分(数量)
    , COALESCE(unit_round.round_division, 0) AS currency_round_division              --丸め処理区分(金額)
    , '1' AS TransitionFlg                      --遷移フラグ
    , pl.receiving_datetime
    , COALESCE(pp.job_structure_id, 0) AS job_structure_id --職種ID
    , pp.parts_location_id                      --棚ID
    , pp.factory_id AS parts_factory_id,        --工場ID
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
                    st_f.structure_id = pl.department_structure_id
                    AND st_f.factory_id IN (0, pp.factory_id)
            ) 
            AND tra.structure_id = pl.department_structure_id
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
                    st_f.structure_id = pl.department_structure_id
                    AND st_f.factory_id NOT IN (0, pp.factory_id)
            ) 
            AND tra.structure_id = pl.department_structure_id
    )
    ) AS department_nm,--部門
    
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
                    st_f.structure_id = pl.account_structure_id
                    AND st_f.factory_id IN (0, pp.factory_id)
            ) 
            AND tra.structure_id = pl.account_structure_id
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
                    st_f.structure_id = pl.unit_structure_id
                    AND st_f.factory_id IN (0, pp.factory_id)
            ) 
            AND tra.structure_id = pl.unit_structure_id
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
                    st_f.structure_id = pl.currency_structure_id
                    AND st_f.factory_id IN (0, pp.factory_id)
            ) 
            AND tra.structure_id = pl.currency_structure_id
    ) AS currency_name                                                    --金額単位名称   
FROM
    pt_parts pp                                 --予備品仕様マスタ
    LEFT JOIN pt_lot AS pl                      --ロット情報
        ON pp.parts_id = pl.parts_id 
    RIGHT JOIN transfer_source AS ts            --移庫先受払履歴
        ON pl.lot_control_id = ts.lot_control_id 
    RIGHT JOIN transfer_destination AS td        --移庫元受払履歴
        ON pl.lot_control_id = td.lot_control_id 
        AND ts.work_no = td.work_no
    LEFT JOIN Department AS dp                  --部門
        ON pl.department_structure_id = dp.structure_id 
    LEFT JOIN Surveyed_Subjects AS sus          --勘定科目
        ON pl.account_structure_id = sus.structure_id 
    LEFT JOIN number_unit                       --数量管理単位
        ON pl.unit_structure_id = number_unit.unit_id 
    LEFT JOIN unit_round                        --丸め処理区分
        ON  pp.factory_id = unit_round.factory_id
    LEFT JOIN currency_unit                     --金額管理単位
        ON pl.currency_structure_id = currency_unit.currency_id
WHERE
    ts.inout_datetime >= @WorkingDay 
    AND ts.inout_datetime < @WorkingDayNext 

AND EXISTS(SELECT * FROM #temp_location temp WHERE pp.factory_id = temp.structure_id)AND EXISTS(SELECT * FROM #temp_job temp WHERE COALESCE(pp.job_structure_id, 0) = temp.structure_id)

