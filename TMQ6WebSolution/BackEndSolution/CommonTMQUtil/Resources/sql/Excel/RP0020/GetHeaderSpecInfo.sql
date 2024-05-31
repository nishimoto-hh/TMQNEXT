WITH structure_factory AS ( 
    SELECT
        structure_id
        , location_structure_id AS factory_id 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id IN (1360) 
        AND language_id = @LanguageId
) 
SELECT
    spec.spec_id
    , CASE 
        WHEN ex.extension_data IN ('2', '3') -- 入力形式が「2：数値」「3：数値(範囲)」の場合、仕様項目名+単位をカッコで囲んで取得する
            THEN tra.translation_text + '(' + ( 
            SELECT
                tra.translation_text 
            FROM
                v_structure_item_all AS tra 
            WHERE
                tra.language_id = @LanguageId
                AND tra.location_structure_id = ( 
                    SELECT
                        max(st_f.factory_id) 
                    FROM
                        structure_factory AS st_f 
                    WHERE
                        st_f.structure_id = spec.spec_unit_id 
                        AND st_f.factory_id IN (0, mmsr.location_structure_id)
                ) 
                AND tra.structure_id = spec.spec_unit_id
        ) + ')' 
        ELSE tra.translation_text -- 入力形式が「1：テキスト」「4：選択」の場合は値をそのまま取得する
        END spec_name -- 仕様項目名
     , ex.extension_data AS spec_type -- 入力形式 「1：テキスト」「2：数値」「3：数値(範囲)」「4：選択」
FROM
    ms_machine_spec_relation mmsr -- 機種別仕様関連付マスタ

    INNER JOIN mc_machine machine -- 機番情報
        -- 機器の職種・機種大分類・機種中分類・機種小分類のいずれかと一致している
        ON (mmsr.job_structure_id = coalesce(machine.job_kind_structure_id, 0) or
            mmsr.job_structure_id = coalesce(machine.job_large_classfication_structure_id, 0) or
            mmsr.job_structure_id = coalesce(machine.job_middle_classfication_structure_id, 0) or
            mmsr.job_structure_id = coalesce(machine.job_small_classfication_structure_id, 0))

    INNER JOIN #temp temp -- 一時テーブル(機器台帳一覧で選択されたレコードの機番IDが格納されている)
        ON machine.machine_id = temp.key1

    LEFT JOIN ms_spec spec -- 仕様項目マスタ
        ON mmsr.spec_id = spec.spec_id 

    LEFT JOIN ms_translation tra -- 翻訳マスタ
        ON spec.translation_id = tra.translation_id 
        AND mmsr.location_structure_id = tra.location_structure_id 
        AND tra.language_id = @LanguageId

    LEFT JOIN ms_structure ms -- 構成マスタ(入力形式取得用)
        ON spec.spec_type_id = ms.structure_id 

    LEFT JOIN ms_item_extension ex -- アイテムマスタ拡張(入力形式取得用)
        ON ms.structure_item_id = ex.item_id 
        AND ex.sequence_no = 1 
WHERE
    spec.delete_flg = 0 -- 削除された仕様項目は対象外
ORDER BY
    mmsr.display_order
    , mmsr.machine_spec_relation_id
