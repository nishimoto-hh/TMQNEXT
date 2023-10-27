-- 入出庫一覧-部門タブのラベル出力専用SQL
WITH structure_factory AS(
    SELECT
        structure_id,
        location_structure_id AS factory_id
    FROM
        v_structure_item_all
    WHERE
        structure_group_id IN(1000, 1040, 1150, 1720, 1730, 1740, 1760, 1770)
    AND language_id = @LanguageId
),
unuse AS( -- 未使用の勘定科目・部門
    SELECT
        ex.extension_data,
        unuse.factory_id
    FROM
        ms_structure_unused unuse
        LEFT JOIN
            ms_structure ms
        ON  unuse.structure_id = ms.structure_id
        LEFT JOIN
            ms_item_extension ex
        ON  ms.structure_item_id = ex.item_id
        AND ex.sequence_no = 1
    WHERE
        unuse.structure_group_id IN(1760, 1770)
)
SELECT
    parts.parts_no,                                                                                                                 -- 予備品No.
    parts.parts_name,                                                                                                               -- 予備品名
    parts.model_type,                                                                                                               -- 型式
    parts.standard_size,                                                                                                            -- 規格・寸法
    ex_dep.extension_data AS department_code,                                                                                       -- 部門コード
    ex_act.extension_data AS subject_code,                                                                                          -- 勘定科目コード
    parts.factory_id AS parts_factory_id,                                                                                           -- 管理工場ID
    pls.parts_location_detail_no,                                                                                                   -- 棚枝番
    COALESCE(parts.lead_time, 0) AS lead_time,                                                                                      -- 発注点
    COALESCE(parts.order_quantity, 0) AS order_quantity,                                                                            -- 発注量
    'YN' + RIGHT('     ' + CONVERT(NVARCHAR, parts.parts_no), 5) + ' ' + ex_dep.extension_data + ' ' + ex_act.extension_data AS qrc -- QRコード
    ---------------------------------- 以下は翻訳を取得 ----------------------------------
    ,
    (
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = parts.manufacturer_structure_id
                AND st_f.factory_id IN(0, parts.factory_id)
            )
        AND tra.structure_id = parts.manufacturer_structure_id
    ) AS maker -- メーカー
    ,(
        SELECT
            tra.translation_text
        FROM
            v_structure_item_all AS tra
        WHERE
            tra.language_id = @LanguageId
        AND tra.location_structure_id = (
                SELECT
                    MAX(st_f.factory_id)
                FROM
                    structure_factory AS st_f
                WHERE
                    st_f.structure_id = pls.parts_location_id
                AND st_f.factory_id IN(0, parts.factory_id)
            )
        AND tra.structure_id = pls.parts_location_id
    ) AS shed_name -- 標準棚番
FROM
    pt_inout_history history -- 受払履歴
    LEFT JOIN
        ms_structure ms -- 構成マスタ(受払区分)
    ON  history.inout_division_structure_id = ms.structure_id
    LEFT JOIN
        ms_item_extension ex -- 拡張項目(受払区分)
    ON  ms.structure_item_id = ex.item_id
    AND ex.sequence_no = 1
    LEFT JOIN
        ms_structure ms_dep -- 構成マスタ(部門)
    ON  history.department_structure_id = ms_dep.structure_id
    LEFT JOIN
        ms_item_extension ex_dep -- 拡張項目(部門)
    ON  ms_dep.structure_item_id = ex_dep.item_id
    AND ex_dep.sequence_no = 1
    LEFT JOIN
        ms_structure ms_act -- 構成マスタ(勘定科目)
    ON  history.account_structure_id = ms_act.structure_id
    LEFT JOIN
        ms_item_extension ex_act -- 拡張項目(勘定科目)
    ON  ms_act.structure_item_id = ex_act.item_id
    AND ex_act.sequence_no = 1
    LEFT JOIN
        pt_location_stock pls -- 在庫情報
    ON  history.inventory_control_id = pls.inventory_control_id
    LEFT JOIN
        pt_parts parts -- 予備品使用
    ON  pls.parts_id = parts.parts_id
WHERE
-- 作業Noを指定
    history.work_no = @WorkNo
-- 移庫先が対象
AND ex.extension_data = '1'
-- 未使用の勘定科目・部門は出力対象外
AND NOT EXISTS(
        SELECT
            *
        FROM
            unuse
        WHERE
            unuse.extension_data IN(ex_dep.extension_data, ex_act.extension_data)
        AND unuse.factory_id = parts.factory_id
    )
ORDER BY
    parts.parts_no,
    department_code,
    subject_code