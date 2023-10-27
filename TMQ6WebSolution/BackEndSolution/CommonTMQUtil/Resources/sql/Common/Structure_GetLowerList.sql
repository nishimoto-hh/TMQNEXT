DROP TABLE IF EXISTS #temp_lower; 
-- 一時テーブルを作成
CREATE TABLE #temp_lower(structure_id int); 
-- 構成IDを一時テーブルへ保存
INSERT 
INTO #temp_lower 
SELECT
    * 
FROM
    STRING_SPLIT(@StructureIdList, ','); 

-- 構成IDリストから配下の構成IDをすべて取得
WITH rec( 
    structure_layer_no
    , structure_id
    , parent_structure_id
) AS ( 
    SELECT
        structure_layer_no
        , structure_id
        , parent_structure_id 
    FROM
        ms_structure st 
    WHERE
        EXISTS ( 
            SELECT
                * 
            FROM
                #temp_lower temp 
            WHERE
                st.structure_id = temp.structure_id
        ) 
        AND delete_flg = 0 
    UNION ALL 
    SELECT
        b.structure_layer_no
        , b.structure_id
        , b.parent_structure_id 
    FROM
        rec a
        , ms_structure b 
    WHERE
        a.structure_id = b.parent_structure_id 
        AND b.delete_flg = 0
) 
SELECT
    structure_id 
FROM
    rec
