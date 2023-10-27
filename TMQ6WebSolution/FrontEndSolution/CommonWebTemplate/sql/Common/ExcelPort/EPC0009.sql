/*
 * ExcelPortダウンロード対象機能 長期計画件名コンボ用データリスト用SQL
 * exparam1以降:自動表示用項目(件名メモ、担当者名)
 */
-- EPC0009
SELECT
     lp.long_plan_id as id          -- 長期計画件名ID
    ,lp.subject as name             -- 件名
    ,lp.subject_note as exparam1    -- 件名メモ
    ,lp.person_name as exparam2     -- 担当者名
    ,ms.factory_id                  -- 工場ID

FROM
    ln_long_plan lp

LEFT JOIN ms_structure ms
ON lp.location_structure_id = ms.structure_id

WHERE
    EXISTS(
        SELECT * FROM #temp_structure_all temp 
        WHERE lp.location_structure_id = temp.structure_id)

AND
    EXISTS(
        SELECT * FROM #temp_structure_all temp 
        WHERE lp.job_structure_id = temp.structure_id)
