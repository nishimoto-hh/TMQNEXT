DROP TABLE IF EXISTS #temp_factory; 
-- 対象工場ID登録用一時テーブルを作成
CREATE TABLE #temp_factory(structure_id int); 

-- 対象の工場IDを一時テーブルへ保存
INSERT 
INTO #temp_factory 
SELECT
    * 
FROM
    STRING_SPLIT(@FactoryIdList, ','); 

--選択工場が一般工場のみまたは個別工場のみの場合1件、混在している場合2件取得する
SELECT
    COALESCE(ex.extension_data, '0') 
FROM
    #temp_factory temp 
    LEFT JOIN ms_structure st 
        ON temp.structure_id = st.structure_id 
    LEFT JOIN ms_item_extension ex 
        ON st.structure_item_id = ex.item_id 
        AND ex.sequence_no = @SequenceNo                  --個別工場フラグ
GROUP BY
    ex.extension_data