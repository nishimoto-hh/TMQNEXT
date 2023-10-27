--******************************************************************
--ユーザー一覧WITH句
--******************************************************************
WITH languageId AS ( 
    --構成ID、拡張データを取得(言語)
    SELECT
        si.structure_id
        , ie.extension_data
    FROM
        v_structure_item si 
        INNER JOIN ms_item_extension ie 
            ON si.structure_item_id = ie.item_id 
            AND si.structure_group_id = 9020 
    WHERE
        si.language_id = @LanguageId
)
, structure_factory AS ( 
    -- 使用する構成グループの構成IDを絞込、工場の指定に用いる
    SELECT
        structure_id
        , location_structure_id AS factory_id 
    FROM
        v_structure_item_all 
    WHERE
        structure_group_id IN ( 
         9020,9040
        ) 
        AND language_id = @LanguageId
) 
