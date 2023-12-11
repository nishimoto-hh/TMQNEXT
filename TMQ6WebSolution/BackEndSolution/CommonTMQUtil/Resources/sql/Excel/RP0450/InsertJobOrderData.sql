INSERT INTO #temp_job_order(
    [structure_id], -- 職種ID
    [sort]          -- ソート順
)
VALUES(
    @Structureid, -- 職種ID
    @Sort         -- ソート順
)
