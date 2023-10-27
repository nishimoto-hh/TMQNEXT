SELECT
    inout_datetime                --出庫日(受払日時)
    , inout_no          --入庫No(ロットNo)
    , parts_location_id           --棚ID
    , parts_location_detail_no    --棚枝番
    , unit_price                  --入庫単価
    , currency_structure_id       --金額管理単位ID
    , inout_quantity              --受払数(出庫数)
    , unit_structure_id           --数量管理単位ID
    , issue_amount                --受払数*入庫単価
    , department_cd               --部門コード
    , department_structure_id     --部門ID
    , subject_cd                  --勘定科目コード
    , account_structure_id        --勘定科目ID
    , management_no               --管理No
    , management_division         --管理区分
    , factory_id                  --工場ID(棚番結合用)
    , unit_digit                  --小数点以下桁数(数量)
    , currency_digit              --小数点以下桁数(金額)
    , unit_round_division         --丸め処理区分(数量)
    , currency_round_division     --丸め処理区分(金額)
    , control_char                --棚卸ID_1(子要素との連携用)
    , parts_factory_id            --工場ID(ツリーの絞り込み用)
    , job_structure_id            --職種機種ID(ツリーの絞り込み用)
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
FROM
    translate_target AS target 