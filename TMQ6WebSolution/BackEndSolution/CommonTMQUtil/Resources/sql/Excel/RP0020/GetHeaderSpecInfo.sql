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
, main AS ( 
    SELECT
        spec.spec_id
        , ex.extension_data spec_type
        , tra.translation_text spec_name
        , ( 
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
        ) unit_name
        , mmsr.display_order
        , mmsr.machine_spec_relation_id 
    FROM
        ms_machine_spec_relation mmsr           -- 機種別仕様関連付マスタ
        INNER JOIN mc_machine machine           -- 機番情報

        -- 機器の工場と一致している かつ、(職種・機種大分類・機種中分類・機種小分類のいずれかと一致している または 職種がNULL)
            ON mmsr.location_structure_id = machine.location_factory_structure_id 
            AND ( 
                mmsr.job_structure_id = coalesce(machine.job_kind_structure_id, 0)
                OR mmsr.job_structure_id = coalesce(machine.job_large_classfication_structure_id, 0)
                OR mmsr.job_structure_id = coalesce(machine.job_middle_classfication_structure_id, 0)
                OR mmsr.job_structure_id = coalesce(machine.job_small_classfication_structure_id, 0)
                OR mmsr.job_structure_id IS NULL
            ) 

        INNER JOIN #temp temp                   -- 一時テーブル(機器台帳一覧で選択されたレコードの機番IDが格納されている)
            ON machine.machine_id = temp.key1 

        INNER JOIN ms_spec spec                  -- 仕様項目マスタ
            ON mmsr.spec_id = spec.spec_id
            AND spec.delete_flg = 0 -- 削除された仕様項目は対象外

        LEFT JOIN ms_translation tra            -- 翻訳マスタ
            ON spec.translation_id = tra.translation_id 
            AND mmsr.location_structure_id = tra.location_structure_id 
            AND tra.language_id = @LanguageId 

        LEFT JOIN ms_structure ms               -- 構成マスタ(入力形式取得用)
            ON spec.spec_type_id = ms.structure_id 

        LEFT JOIN ms_item_extension ex          -- アイテムマスタ拡張(入力形式取得用)
            ON ms.structure_item_id = ex.item_id 
            AND ex.sequence_no = 1 
) 

SELECT
    main.spec_id      -- 仕様項目ID
    , main.spec_type  -- 入力形式 「1：テキスト」「2：数値」「3：数値(範囲)」「4：選択」

    -- 入力形式が「2：数値」「3：数値(範囲)」の場合かつ単位の翻訳が取得できている場合、仕様項目名+単位をカッコで囲んで取得する
    -- ※単位の翻訳が取得できていない場合は仕様項目名のみ
    -- 入力形式が「1：テキスト」「4：選択」の場合は値をそのまま取得する
    , CASE 
        WHEN main.spec_type IN ('2', '3') AND main.unit_name IS NOT NULL 
            THEN main.spec_name + '(' + main.unit_name + ')' 
        ELSE main.spec_name 
        END spec_name -- 仕様項目名
FROM
    main 
ORDER BY
    main.display_order
    , main.machine_spec_relation_id