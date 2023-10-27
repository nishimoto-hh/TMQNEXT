WITH location AS(
    -- 構成IDの階層番号を取得
    SELECT
        ms.structure_id,
        ms.structure_layer_no
    FROM
        ms_structure ms
    WHERE
        ms.structure_group_id = 1040
    AND ms.structure_id = @PartsLocationIdEnter
),
structure_factory AS(
    SELECT
        structure_id,
        location_structure_id AS factory_id
    FROM
        v_structure_item_all
    WHERE
        structure_group_id IN(1000, 1040, 1150, 1720, 1730, 1740, 1760, 1770)
    AND language_id = @LanguageId
),
unuse AS(
    -- 未使用の勘定科目・部門
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
),
main AS(
    SELECT
        parts.parts_no,                                                                                               -- 予備品No.
        parts.parts_name,                                                                                             -- 予備品名
        parts.model_type,                                                                                             -- 型式
        parts.standard_size,                                                                                          -- 規格・寸法
        @DepartmentCdEnter AS department_code,                                                                             -- 部門コード
        @SubjectCdEnter AS subject_code,                                                                                   -- 勘定科目コード
        parts.factory_id AS parts_factory_id,                                                                         -- 管理工場ID
        @PartsLocationDetailNoEnter AS parts_location_detail_no,                                                           -- 棚枝番
        COALESCE(parts.lead_time, 0) AS lead_time,                                                                    -- 発注点
        COALESCE(parts.order_quantity, 0) AS order_quantity,                                                          -- 発注量
        'YN' + RIGHT('     ' + CONVERT(NVARCHAR, parts.parts_no), 5) + ' ' + @DepartmentCdEnter + ' ' + @SubjectCdEnter AS qrc, -- QRコード
        ---------------------------------- 以下は翻訳を取得 ----------------------------------
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
        ) AS maker, -- メーカー
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
                        st_f.structure_id = @PartsLocationIdEnter
                    AND st_f.factory_id IN(0, parts.factory_id)
                )
            AND tra.structure_id = @PartsLocationIdEnter
        ) AS shed_name -- 標準棚番
    FROM
        pt_parts parts
    WHERE
        -- 未使用の勘定科目・部門は出力対象外
        NOT EXISTS(
            SELECT
                *
            FROM
                unuse
            WHERE
                unuse.extension_data IN(@DepartmentCdEnter, @SubjectCdEnter)
            AND unuse.factory_id = parts.factory_id
        )
    AND parts.parts_id = @PartsId
)
SELECT
    *
FROM
    main
ORDER BY
    main.parts_no,
    main.department_code,
    main.subject_code