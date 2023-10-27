SELECT
    inout_datetime                       --出庫日(受払日時)
    , inout_no                           --出庫No(作業No)
    , work_division_structure_id         --入出庫区分
    , parts_no                           --予備品No
    , parts_name                         --予備品名
    , old_new_structure_id               --新旧区分
    , standard_size                      --規格・寸法
    , issue_quantity                     --受払数(出庫数)
    , unit_structure_id                  --数量管理単位ID
    , issue_amount                       --受払数*入庫単価
    , currency_structure_id              --金額管理単位ID
    , unit_digit                         --小数点以下桁数(数量)
    , currency_digit                     --小数点以下桁数(金額)
    , unit_round_division                --丸め処理区分(数量)
    , currency_round_division            --丸め処理区分(金額)
    , control_char                       --棚卸ID_1(子要素との連携用)
    , control_flag                       --制御用フラグ
    , department_structure_id            --部門ID
    , account_structure_id               --勘定科目ID
    , factory_id                         --工場ID
    , parts_factory_id                   --工場ID(ツリーの絞り込み用)
    , job_structure_id                   --職種機種ID(ツリーの絞り込み用)
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
                    st_f.structure_id = target.work_division_structure_id 
                    AND st_f.factory_id IN (0, target.factory_id)
            ) 
            AND tra.structure_id = target.work_division_structure_id
    ) AS work_division_name  --入出庫区分(翻訳)
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
    ) AS unit_name  --数量単位名称(翻訳)
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
                    st_f.structure_id = target.currency_structure_id 
                    AND st_f.factory_id IN (0, target.factory_id)
            ) 
            AND tra.structure_id = target.currency_structure_id
    ) AS currency_name  --金額単位名称(翻訳)
FROM
    translate_target AS target 