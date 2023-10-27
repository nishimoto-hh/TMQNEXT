DROP TABLE IF EXISTS #temp;
CREATE TABLE #temp (
    key1 bigint NOT NULL
    , key2 bigint NULL
    , key3 bigint NULL
    , languageId nvarchar(2)  COLLATE database_default
    , factoryId int
);

