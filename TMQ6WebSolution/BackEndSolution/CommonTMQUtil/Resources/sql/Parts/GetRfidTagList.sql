WITH structure_factory AS ( 
    SELECT
        structure_id
        , location_structure_id AS factory_id 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id IN (1760, 1770) 
        AND language_id = @LanguageId
) 
SELECT
    tag.rftag_id                                                    -- RFIDタグ
    , tag.rftag_id AS rftag_id_before                               -- RFIDタグ(変更前)
    , tag.serial_no                                                 -- 連番
    , tag.department_structure_id                                   -- 部門ID
    , tag.account_structure_id                                      -- 勘定科目ID
    , tag.update_datetime AS saved_update_datetime                  -- 更新日時
    , exd.extension_data AS department_code                         -- 部門コード
    , exa.extension_data AS account_code                            -- 勘定科目コード
    , tag.department_structure_id                                   -- 部門ID
    , tag.department_structure_id AS department_structure_id_before -- 部門ID(変更前)
    , tag.account_structure_id                                      -- 勘定科目ID
    , tag.account_structure_id AS account_structure_id_before       -- 勘定科目ID(勘定科目ID)
    , tag.parts_id                                                  -- 予備品ID
    , exd.extension_data + ' ' + coalesce(
        ( 
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
                        st_f.structure_id = tag.department_structure_id 
                        AND st_f.factory_id IN (0, parts.factory_id)
                ) 
                AND tra.structure_id = tag.department_structure_id
        ) 
        , ( 
            SELECT
                tra.translation_text 
            FROM
                v_structure_item_all AS tra 
            WHERE
                tra.language_id = @LanguageId
                AND tra.location_structure_id = ( 
                    SELECT
                        min(st_f.factory_id) 
                    FROM
                        structure_factory AS st_f 
                    WHERE
                        st_f.structure_id = tag.department_structure_id 
                        AND st_f.factory_id NOT IN (0, parts.factory_id)
                ) 
                AND tra.structure_id = tag.department_structure_id
        )
    ) AS department_name                        -- 部門名
    , exa.extension_data + ' ' + coalesce( 
        ( 
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
                        st_f.structure_id = tag.account_structure_id 
                        AND st_f.factory_id IN (0, parts.factory_id)
                ) 
                AND tra.structure_id = tag.account_structure_id
        ) 
        , ( 
            SELECT
                tra.translation_text 
            FROM
                v_structure_item_all AS tra 
            WHERE
                tra.language_id = @LanguageId
                AND tra.location_structure_id = ( 
                    SELECT
                        min(st_f.factory_id) 
                    FROM
                        structure_factory AS st_f 
                    WHERE
                        st_f.structure_id = tag.account_structure_id 
                        AND st_f.factory_id NOT IN (0, parts.factory_id)
                ) 
                AND tra.structure_id = tag.account_structure_id
        )
    ) AS account_name                           -- 勘定科目名
FROM
    pt_rftag_parts_link tag                     -- RFタグ予備品マスタ
    INNER JOIN pt_parts parts                   -- 予備品仕様マスタ
        ON tag.parts_id = parts.parts_id 
    LEFT JOIN ms_structure msd                  -- 構成マスタ(部門コード取得用)
        ON tag.department_structure_id = msd.structure_id 
    LEFT JOIN ms_item_extension exd             -- アイテムマスタ拡張(部門コード取得用)
        ON msd.structure_item_id = exd.item_id 
        AND exd.sequence_no = 1 
    LEFT JOIN ms_structure msa                  -- 構成マスタ(勘定科目コード取得用)
        ON tag.account_structure_id = msa.structure_id 
    LEFT JOIN ms_item_extension exa             -- アイテムマスタ拡張(勘定科目コード取得用)
        ON msa.structure_item_id = exa.item_id 
        AND exa.sequence_no = 1
WHERE
    tag.parts_id = @PartsId
ORDER BY
    tag.rftag_id -- RFIDタグの昇順
