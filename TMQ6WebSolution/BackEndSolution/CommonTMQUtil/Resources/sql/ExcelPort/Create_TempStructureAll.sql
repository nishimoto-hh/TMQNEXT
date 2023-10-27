-- 全階層構成マスタデータ登録用一時テーブルを作成
/*@All
DROP TABLE IF EXISTS #temp_structure_all; 
CREATE TABLE #temp_structure_all(
@All*/

/*@Selected
DROP TABLE IF EXISTS #temp_structure_selected;
CREATE TABLE #temp_structure_selected(
@Selected*/
     structure_id int
    ,factory_id int
    ,structure_group_id int
    ,parent_structure_id int
    ,structure_layer_no int
    ,translation_text varchar(800)
    ,display_order int
); 
