DROP TABLE IF EXISTS #temp_progress; 
CREATE TABLE #temp_progress(
    structure_id int
    ,extension_data nvarchar(400)
);
DROP TABLE IF EXISTS #get_factory; 
CREATE TABLE #get_factory(
    structure_id int
    ,extension_data nvarchar(400)
);