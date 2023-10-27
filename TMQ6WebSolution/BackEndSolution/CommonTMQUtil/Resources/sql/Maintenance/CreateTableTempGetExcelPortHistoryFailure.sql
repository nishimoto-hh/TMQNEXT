DROP TABLE IF EXISTS #follow_flg; 
CREATE TABLE #follow_flg(
    structure_id int
    ,extension_data nvarchar(400)
);
DROP TABLE IF EXISTS #get_factory; 
CREATE TABLE #get_factory(
    structure_id int
    ,extension_data nvarchar(400)
);