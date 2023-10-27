--******************************************************************
--部門移庫一覧SQL
--******************************************************************
SELECT DISTINCT
    FORMAT(pih.inout_datetime, 'yyyy/MM/dd') AS relocation_date --受払日時
    , pih.work_no AS transfer_no                --作業No
    , pp.parts_no                               --予備品No
    , pp.parts_name                             --予備品名
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
    , pp.standard_size AS dimensions            --規格・寸法
    , pih.inout_quantity AS transfer_count      --受払数
    , pl.lot_no                                 --ロットNo
    , pl.unit_price                             --金額
    , pih.inout_quantity * pl.unit_price AS transfer_amount
    , dp.extension_data AS department_cd        --部門コード
    , COALESCE(
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
                    st_f.structure_id = pih.department_structure_id
                    AND st_f.factory_id IN (0, pp.factory_id)
            ) 
            AND tra.structure_id = pih.department_structure_id
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = pih.department_structure_id
                    AND st_f.factory_id NOT IN (0, pp.factory_id)
            ) 
            AND tra.structure_id = pih.department_structure_id
      )
    ) AS department_nm  --部門(翻訳)
    , sus.extension_data AS subject_cd          --勘定科目コード
    , pih.management_no                         --管理No
    , pih.management_division                   --管理区分
    , tdp.extension_data AS to_department_cd    --部門コード
    , COALESCE(
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
                    st_f.structure_id = tpih.department_structure_id
                    AND st_f.factory_id IN (0, pp.factory_id)
            ) 
            AND tra.structure_id = tpih.department_structure_id
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
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = tpih.department_structure_id
                    AND st_f.factory_id NOT IN (0, pp.factory_id)
            ) 
            AND tra.structure_id = tpih.department_structure_id
      )
    ) AS to_department_nm  --部門(翻訳)

    , tsus.extension_data AS to_subject_cd      --勘定科目コード
    , tpih.management_no AS to_management_no    --管理No
    , tpih.management_division AS to_management_division --管理区分
    , receiving_datetime                        --受払日時
    , pp.parts_id                               --予備品ID
    , tpih.work_no                              --作業No
    , COALESCE(number_unit.unit_digit, 0) AS unit_digit
    , COALESCE(currency_unit.currency_digit, 0) AS currency_digit
    , COALESCE(unit_round.round_division, 0) AS unit_round_division
    , COALESCE(unit_round.round_division, 0) AS currency_round_division
    , '1' AS TransitionFlg                      --遷移フラグ
    , pp.job_structure_id                       --職種
    , pp.parts_location_id                      --棚ID
    , pp.factory_id AS parts_factory_id,        --工場ID
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
                    st_f.structure_id = pih.account_structure_id
                    AND st_f.factory_id IN (0, pp.factory_id)
            ) 
            AND tra.structure_id = pih.account_structure_id
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
                    st_f.structure_id = tpih.account_structure_id
                    AND st_f.factory_id IN (0, pp.factory_id)
            ) 
            AND tra.structure_id = tpih.account_structure_id
    ) AS to_subject_nm,                                                    --勘定科目名
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
    RIGHT JOIN ( 
        SELECT
            pih.inout_division_structure_id
            , pih.work_division_structure_id
            , pih.work_no
            , pih.lot_control_id
            , pih.department_structure_id
            , pih.account_structure_id
            , pih.management_division
            , pih.management_no
            , pih.inout_datetime
            , pih.inout_quantity 
        FROM
            pt_inout_history pih 
            LEFT JOIN inout_division ids 
                ON pih.inout_division_structure_id = ids.structure_id 
            LEFT JOIN work_division wds 
                ON pih.work_division_structure_id = wds.structure_id 
        WHERE
            ids.extension_data = '2' 
            AND wds.extension_data = '4'
            AND pih.delete_flg = 0
    ) AS pih                                    --受払履歴
        ON pl.lot_control_id = pih.lot_control_id 
    RIGHT JOIN ( 
        SELECT
            pih.inout_division_structure_id
            , pih.work_division_structure_id
            , pih.work_no
            , pih.lot_control_id
            , pih.department_structure_id
            , pih.account_structure_id
            , pih.management_division
            , pih.management_no
            , pih.inout_datetime
            , pih.inout_quantity 
        FROM
            pt_inout_history pih 
            LEFT JOIN inout_division ids 
                ON pih.inout_division_structure_id = ids.structure_id 
            LEFT JOIN work_division wds 
                ON pih.work_division_structure_id = wds.structure_id 
        WHERE
            ids.extension_data = '1' 
            AND wds.extension_data = '4'
            AND pih.delete_flg = 0
    ) AS tpih                                   --受払履歴
        ON pl.lot_control_id = tpih.lot_control_id 
        AND pih.work_no = tpih.work_no
    LEFT JOIN pt_location_stock AS pls          --在庫データ
        ON pl.parts_id = pls.parts_id 
    LEFT JOIN Department AS dp                  --部門
        ON pih.department_structure_id = dp.structure_id 
    LEFT JOIN Surveyed_Subjects AS sus          --勘定科目
        ON pih.account_structure_id = sus.structure_id 
    LEFT JOIN Department AS tdp                 --部門
        ON tpih.department_structure_id = tdp.structure_id 
    LEFT JOIN Surveyed_Subjects AS tsus         --勘定科目
        ON tpih.account_structure_id = tsus.structure_id 
    LEFT JOIN number_unit                       --単位
        ON pl.unit_structure_id = number_unit.unit_id 
    LEFT JOIN unit_round                        --丸目処理区分
        ON  pp.factory_id = unit_round.factory_id
    LEFT JOIN currency_unit                     --金額単位数量
        ON pl.currency_structure_id = currency_unit.currency_id

WHERE
    pih.inout_datetime >= @WorkingDay 
    AND pih.inout_datetime < @WorkingDayNext    --作業日
/*@FactoryIdList
    AND pp.factory_id IN @FactoryIdList
@FactoryIdList*/
/*@JobIdList
    AND pp.job_structure_id IN @JobIdList
@JobIdList*/
