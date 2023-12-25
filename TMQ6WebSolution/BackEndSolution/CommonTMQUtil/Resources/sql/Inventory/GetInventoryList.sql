SELECT
    parts_location_id                        --棚番ID
    , parts_location_detail_no               --棚枝番
    , parts_no                               --予備品No
    , parts_name                             --予備品名
    , old_new_structure_id                   --新旧区分
    , department_structure_id                --部門ID
    , department_cd                          --部門コード
    , account_structure_id                   --勘定科目ID
    , subject_cd                             --勘定科目コード
    , stock_quantity_flg                     --在庫あり
    , inventory_diff_flg                     --棚差あり
    , stock_quantity                         --在庫数
    , preparation_datetime                   --棚卸準備日時
    , inventory_datetime                     --棚卸日時
    , difference_datetime                    --棚卸調整日時
    , inventory_quantity                     --棚卸数(取込の場合、取込値を表示。棚卸数がNULLの場合、在庫数を表示)
    , inout_quantity                         --棚卸調整数
    , inventory_diff                         --棚差
    , manufacturer_structure_id              --メーカー
    , materials                              --材質
    , model_type                             --型式
    , fixed_datetime                         --棚卸確定日時
    , parts_id                               --予備品ID
    , unit_structure_id                      --数量単位ID
    , currency_structure_id                  --金額単位ID
    , factory_id                             --工場ID
    , inventory_id                           --棚卸ID
    , update_serialid                        --棚卸データ.更新シリアルID
    , unit_digit                             --小数点以下桁数(数量)
    , unit_round_division                    --丸め処理区分(数量)
    , parts_factory_id                       --工場ID(ツリーの絞り込み用)
    , job_structure_id                       --職種機種ID(ツリーの絞り込み用)
    , rftag_id                               --RFIDタグ
    , work_user_name                         --作業者
    , @InventoryIdFlg AS upload_flg          --取込の場合true
    , ( 
        SELECT
            tra.translation_text 
        FROM
            v_structure_item_all AS tra 
        WHERE
            tra.language_id = @LanguageId
            AND tra.location_structure_id = ( 
                -- 工場IDまたは0で合致する最大の工場IDを取得→工場IDのレコードがなければ0となる
                SELECT
                    MAX(st_f.factory_id) 
                FROM
                    #temp_structure_factory AS st_f 
                WHERE
                    st_f.structure_id = target.old_new_structure_id 
                    AND st_f.factory_id IN (0, target.factory_id)
            ) 
            AND tra.structure_id = target.old_new_structure_id
    ) AS old_new_nm  --新旧区分(翻訳)
    , ( 
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
                    st_f.structure_id = target.unit_structure_id 
                    AND st_f.factory_id IN (0, target.factory_id)
            ) 
            AND tra.structure_id = target.unit_structure_id
    ) AS unit  --数量単位名称(翻訳)
    , ( 
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
                    st_f.structure_id = target.department_structure_id 
                    AND st_f.factory_id IN (0, target.factory_id)
            ) 
            AND tra.structure_id = target.department_structure_id
    ) AS department_nm  --部門(翻訳)
    , ( 
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
                    st_f.structure_id = target.account_structure_id 
                    AND st_f.factory_id IN (0, target.factory_id)
            ) 
            AND tra.structure_id = target.account_structure_id
    ) AS subject_nm  --勘定科目(翻訳)
    , ( 
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
                    st_f.structure_id = target.manufacturer_structure_id 
                    AND st_f.factory_id IN (0, target.factory_id)
            ) 
            AND tra.structure_id = target.manufacturer_structure_id
    ) AS manufacturer_name  --メーカー(翻訳)
FROM
    translate_target AS target 