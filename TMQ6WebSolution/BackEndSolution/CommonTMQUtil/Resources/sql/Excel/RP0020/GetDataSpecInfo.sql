WITH structure_factory AS ( 
    SELECT
        structure_id
        , location_structure_id AS factory_id 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id IN (1340) 
        AND language_id = @LanguageId
) 
, spec_val AS ( 
    SELECT
        mes.equipment_id -- 機器ID

        -- 仕様項目の入力形式の拡張項目で値の取得方法を変える
        , CASE 
            WHEN ex.extension_data = '1' -- 「1：テキスト」
                THEN mes.spec_value 
            WHEN ex.extension_data = '2' -- 「2：数値」
                THEN convert( 
                nvarchar
                , format( 
                    mes.spec_num
                    , 'F' + convert(nvarchar, spec.spec_num_decimal_places)
                )
            ) 
            WHEN ex.extension_data = '3' -- 「3：数値(範囲)」
            THEN CASE 
                -- 最小値・最大値どちらも値が設定されていない場合は空とする
                WHEN mes.spec_num_min IS NULL AND mes.spec_num_max IS NULL 
                THEN '' 
                ELSE coalesce(convert(nvarchar, format(mes.spec_num_min, 'F' + convert(nvarchar, spec.spec_num_decimal_places))), '')
                    + '~' 
                    + coalesce(convert(nvarchar, format(mes.spec_num_max, 'F' + convert(nvarchar, spec.spec_num_decimal_places))), '') 
                END 
            WHEN ex.extension_data = '4' -- 「4：選択」
                THEN ( 
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
                            st_f.structure_id = mes.spec_structure_id 
                            AND st_f.factory_id IN (0, machine.location_factory_structure_id)
                    ) 
                    AND tra.structure_id = mes.spec_structure_id
            ) 
            ELSE '' 
            END spec_value 
    FROM
        mc_machine machine -- 機番情報

        INNER JOIN #temp temp -- 一時テーブル(機器台帳一覧で選択されたレコードの機番IDが格納されている)
            ON machine.machine_id = temp.key1 

        INNER JOIN mc_equipment equipment -- 機器情報
            ON machine.machine_id = equipment.machine_id
            
        INNER JOIN mc_equipment_spec mes -- 仕様情報
            ON equipment.equipment_id = mes.equipment_id 

        INNER JOIN ms_spec spec -- 仕様項目マスタ
            ON mes.spec_id = spec.spec_id 

        INNER JOIN #temp_spec_id tsi -- 一時テーブル(検索対象の仕様項目IDが格納されている)
            ON spec.spec_id = tsi.spec_id 

        LEFT JOIN ms_structure ms -- 構成マスタ(入力形式用)
            ON spec.spec_type_id = ms.structure_id 

        LEFT JOIN ms_item_extension ex -- アイテムマスタ拡張(入力形式用)
            ON ms.structure_item_id = ex.item_id 
            AND ex.sequence_no = 1
) 
SELECT
    spec_val.spec_value
FROM
    mc_machine machine 
    INNER JOIN #temp temp -- 一時テーブル(機器台帳一覧で選択されたレコードの機番IDが格納されている)
        ON machine.machine_id = temp.key1 
    LEFT JOIN mc_equipment equipment 
        ON machine.machine_id = equipment.machine_id 
    LEFT JOIN spec_val 
        ON equipment.equipment_id = spec_val.equipment_id 
ORDER BY
    machine.machine_no
