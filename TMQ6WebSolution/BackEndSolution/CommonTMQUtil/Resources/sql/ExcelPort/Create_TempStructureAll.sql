DROP TABLE IF EXISTS #temp_structure_all; 
-- 全階層構成マスタデータ登録用一時テーブルを作成
CREATE TABLE #temp_structure_all(
     structure_id int
    ,factory_id int
    ,structure_group_id int
    ,parent_structure_id int
    ,structure_layer_no int
    ,translation_text varchar(800)
    ,display_order int
); 
