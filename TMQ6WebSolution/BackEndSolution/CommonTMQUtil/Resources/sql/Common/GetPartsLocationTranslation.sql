DROP TABLE IF EXISTS #temp_parts_location; 
-- 一時テーブルを作成
CREATE TABLE #temp_parts_location(structure_id int); 
-- 構成IDを一時テーブルへ保存
INSERT 
INTO #temp_parts_location 
SELECT
    * 
FROM
    STRING_SPLIT(@StructureIdList, ','); 

SELECT
    structure_id
    , translation_text 
FROM
    v_structure_item_all st 
WHERE
    EXISTS ( 
        SELECT
            * 
        FROM
            #temp_parts_location temp 
        WHERE
            st.structure_id = temp.structure_id
    ) 
    AND language_id = @LanguageId;