/*
* 機種別仕様一覧画面
* 機種別仕様一覧を取得するSQL
*/
SELECT
     msr.machine_spec_relation_id
    -- 職種
    ,msr.job_structure_id
    ,sp.spec_id
    -- 翻訳名称
    ,tra.translation_text as specification_name
    -- 表示順
    ,msr.display_order
    -- 削除フラグ
    ,sp.delete_flg
FROM
    ms_machine_spec_relation AS msr
    INNER JOIN
        ms_spec AS sp
    ON  msr.spec_id = sp.spec_id
    INNER JOIN
        ms_translation AS tra
    ON  (
            sp.translation_id = tra.translation_id
        )
WHERE
    msr.location_structure_id = @FactoryId
AND tra.language_id = @LanguageId
AND sp.delete_flg IN (0 , @DeleteFlg)