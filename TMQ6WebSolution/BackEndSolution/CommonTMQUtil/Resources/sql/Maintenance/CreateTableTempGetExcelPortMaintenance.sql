DROP TABLE IF EXISTS #activity_division; 
CREATE TABLE #activity_division(
    structure_id int
    ,extension_data nvarchar(400)
);
DROP TABLE IF EXISTS #call_count; 
CREATE TABLE #call_count(
    structure_id int
    ,extension_data nvarchar(400)
);
DROP TABLE IF EXISTS #parts_existence; 
CREATE TABLE #parts_existence(
    structure_id int
    ,extension_data nvarchar(400)
);
DROP TABLE IF EXISTS #get_factory; 
CREATE TABLE #get_factory(
    structure_id int
    ,extension_data nvarchar(400)
);