/*
* 機種別仕様登録画面
* 登録された機種別仕様の情報を取得
* 複数の職種が表示されるが、他の項目の値は同じとなる
*/
-- 数値書式は仕様項目マスタの値は拡張項目なのでその値より構成IDを取得する必要がある
-- 構成マスタの内容は固定
WITH num_type AS (
SELECT
    st.structure_id,
    ex.extension_data
FROM
    ms_structure AS st
    INNER JOIN
        ms_item_extension AS ex
    ON  (
            st.structure_item_id = ex.item_id
        AND ex.sequence_no = 1
        )
WHERE
    st.structure_group_id = 2060
)

SELECT
     -- 職種
     msr_disp.job_structure_id
     -- 入力形式
    ,sp.spec_type_id
     -- 数値書式
    ,num_type.structure_id AS spec_num_decimal_places
     -- 単位種別
    ,sp.spec_unit_type_id
     -- 単位
    ,sp.spec_unit_id
     -- 表示順
    ,msr_disp.display_order
     -- 削除フラグ
    ,sp.delete_flg
     -- 排他チェック用
    ,sp.spec_id
    ,sp.update_serialid
    ,msr_disp.machine_spec_relation_id
    ,msr_disp.update_serialid AS update_serialid_relation
FROM
    ms_spec AS sp
    INNER JOIN
        ms_machine_spec_relation AS msr_disp
    ON  sp.spec_id = msr_disp.spec_id
    -- 数値書式
    LEFT OUTER JOIN
        num_type AS num_type
    ON  CAST(sp.spec_num_decimal_places AS VARCHAR) = num_type.extension_data
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
        AND sp.spec_id = msr_narrow.spec_id
        AND msr_narrow.location_structure_id = msr_disp.location_structure_id
    )