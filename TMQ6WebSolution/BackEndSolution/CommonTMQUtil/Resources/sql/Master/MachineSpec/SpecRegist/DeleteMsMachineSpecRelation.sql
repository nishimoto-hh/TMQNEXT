/*
* 機種別仕様登録画面
* 機種別仕様関連付けマスタ　削除、および修正登録(DELETE→INSERT)
*/
DELETE msr_del
FROM
    ms_machine_spec_relation AS msr_del
WHERE
    -- 一覧で選択された機種別仕様関連付IDの仕様項目IDと同じ仕様項目を表示するが、場所階層が異なる場合は表示しない
    -- (正しいデータならないはず)
    EXISTS(
        SELECT
            *
        FROM
            ms_machine_spec_relation AS msr_narrow
        WHERE
            msr_narrow.machine_spec_relation_id = @MachineSpecRelationId
        AND msr_del.spec_id = msr_narrow.spec_id
        AND msr_narrow.location_structure_id = msr_del.location_structure_id
    )