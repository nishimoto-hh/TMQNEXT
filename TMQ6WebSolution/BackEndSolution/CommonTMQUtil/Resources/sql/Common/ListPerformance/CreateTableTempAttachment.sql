DROP TABLE IF EXISTS #temp_attachment; 
CREATE TABLE #temp_attachment(
    key_id bigint
    ,function_type_id int
    ,file_name nvarchar(800)
    ,attachment_id bigint
    ,document_no nvarchar(200)
    ,extension_data nvarchar(400)
);