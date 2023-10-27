--******************************************************************
--入庫入力予備品情報一覧WITH句
--******************************************************************
WITH quantity_unit AS ( 
    --ビューより翻訳を取得(数量管理単位)
    SELECT
        structure_id
        , translation_text 
    FROM
        v_structure_item AS si 
    WHERE
        structure_group_id = 1730 
        AND language_id = @LanguageId
) , location AS ( 
    --拡張データ、翻訳を取得(棚)
    SELECT
        structure_id
        , translation_text 
    FROM
        v_structure_item si 
    WHERE
        structure_group_id = 1040 
    AND
        structure_layer_no = 1
    AND
        si.language_id = @LanguageId
)
