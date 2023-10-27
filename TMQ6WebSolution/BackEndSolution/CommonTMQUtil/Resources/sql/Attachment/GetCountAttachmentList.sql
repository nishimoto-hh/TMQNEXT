WITH factory AS(
    SELECT DISTINCT
        structure_id,
        location.translation_text
    FROM
        v_structure_item_all location
    WHERE
        structure_group_id = 1000
    AND structure_layer_no = 0
    AND language_id = @LanguageId
),
target AS(
    /*******************機器台帳-機番添付*******************/
    SELECT
        machine.location_structure_id, --場所階層ID
        machine.job_structure_id       --職種ID
    FROM
        attachment ac
        LEFT JOIN
            mc_machine machine
        ON  ac.key_id = machine.machine_id
    WHERE
        function_type_id = 1600
    /*******************機器台帳-機器添付*******************/
    UNION ALL
    SELECT
        machine.location_structure_id, --場所階層ID
        machine.job_structure_id       --職種ID
    FROM
        attachment ac
    LEFT JOIN
        mc_equipment equipment
    ON  ac.key_id = equipment.equipment_id
    LEFT JOIN
        mc_machine machine
    ON  equipment.machine_id = machine.machine_id
    WHERE
        function_type_id = 1610
    /*******************機器台帳-機器別管理基準タブ-ファイル添付*******************/
    UNION ALL
    SELECT
        machine.location_structure_id, --場所階層ID
        machine.job_structure_id       --職種ID
    FROM
        attachment ac
    LEFT JOIN
        mc_management_standards_content content
    ON  ac.key_id = content.management_standards_content_id
    LEFT JOIN
        mc_management_standards_component component
    ON  content.management_standards_component_id = component.management_standards_component_id
    LEFT JOIN
        mc_machine machine
    ON  component.machine_id = machine.machine_id
    WHERE
        function_type_id = 1620
    /*******************機器台帳-MP情報-ファイル添付*******************/
    UNION ALL
    SELECT
        machine.location_structure_id, --場所階層ID
        machine.job_structure_id       --職種ID
    FROM
        attachment ac
    LEFT JOIN
        mc_mp_information info
    ON  ac.key_id = info.mp_information_id
    LEFT JOIN
        mc_machine machine
    ON  info.machine_id = machine.machine_id
    WHERE
        function_type_id = 1630
    /*******************件名別長期計画-件名添付*******************/
    UNION ALL
    SELECT
        lnplan.location_structure_id, --場所階層ID
        lnplan.job_structure_id       --職種ID
    FROM
        attachment ac
    LEFT JOIN
        ln_long_plan lnplan
    ON  ac.key_id = lnplan.long_plan_id
    WHERE
        function_type_id = 1640
    /*******************保全活動-件名添付*******************/
    UNION ALL
    SELECT
        summary.location_structure_id, --場所階層ID
        summary.job_structure_id       --職種ID
    FROM
        attachment ac
    LEFT JOIN
        ma_summary summary
    ON  ac.key_id = summary.summary_id
    WHERE
        function_type_id = 1650
    /*******************保全活動-故障分析情報タブ-略図添付*******************/
    UNION ALL
    SELECT
        summary.location_structure_id, --場所階層ID
        summary.job_structure_id       --職種ID
    FROM
        attachment ac
    LEFT JOIN
        ma_history_failure failure
    ON  ac.key_id = failure.history_failure_id
    LEFT JOIN
        ma_history history
    ON  failure.history_id = history.history_id
    LEFT JOIN
        ma_summary summary
    ON  history.summary_id = summary.summary_id
    WHERE
        function_type_id = 1660
    /*******************保全活動-故障分析情報タブ-故障原因分析書添付*******************/
    UNION ALL
    SELECT
        summary.location_structure_id, --場所階層ID
        summary.job_structure_id       --職種ID
    FROM
        attachment ac
    LEFT JOIN
        ma_history_failure failure
    ON  ac.key_id = failure.history_failure_id
    LEFT JOIN
        ma_history history
    ON  failure.history_id = history.history_id
    LEFT JOIN
        ma_summary summary
    ON  history.summary_id = summary.summary_id
    WHERE
        function_type_id = 1670
    /*******************保全活動-故障分析情報(個別工場)タブ-略図添付*******************/
    UNION ALL
    SELECT
        summary.location_structure_id, --場所階層ID
        summary.job_structure_id       --職種ID
    FROM
        attachment ac
    LEFT JOIN
        ma_history_failure failure
    ON  ac.key_id = failure.history_failure_id
    LEFT JOIN
        ma_history history
    ON  failure.history_id = history.history_id
    LEFT JOIN
        ma_summary summary
    ON  history.summary_id = summary.summary_id
    WHERE
        function_type_id = 1680
    /*******************保全活動-故障分析情報(個別工場)タブ-故障原因分析書添付*******************/
    UNION ALL
    SELECT
        summary.location_structure_id, --場所階層ID
        summary.job_structure_id       --職種ID
    FROM
        attachment ac
    LEFT JOIN
        ma_history_failure failure
    ON  ac.key_id = failure.history_failure_id
    LEFT JOIN
        ma_history history
    ON  failure.history_id = history.history_id
    LEFT JOIN
        ma_summary summary
    ON  history.summary_id = summary.summary_id
    WHERE
        function_type_id = 1690
    /*******************予備品管理-詳細画面-画像添付*******************/
    UNION ALL
    SELECT
        parts.factory_id AS location_structure_id, --場所階層ID
        parts.job_structure_id                     --職種ID
    FROM
        attachment ac
    LEFT JOIN
        pt_parts parts
    ON  ac.key_id = parts.parts_id
    WHERE
        function_type_id = 1700
    /*******************予備品管理-詳細画面-文書添付*******************/
    UNION ALL
    SELECT
        parts.factory_id AS location_structure_id, --場所階層ID
        parts.job_structure_id                     --職種ID
    FROM
        attachment ac
    LEFT JOIN
        pt_parts parts
    ON  ac.key_id = parts.parts_id
    WHERE
        function_type_id = 1750
    /*******************予備品管理-詳細画面-予備品地図*******************/
    UNION ALL
    SELECT
        ac.key_id AS location_structure_id, --場所階層ID
        0 AS job_structure_id               --職種ID  
    FROM
        attachment ac
    LEFT JOIN
        factory
    ON  ac.key_id = factory.structure_id
    WHERE
        function_type_id = 1780
)

SELECT
    COUNT(*)
FROM
    target
WHERE
    EXISTS(
        SELECT
            *
        FROM
            #temp_location temp
        WHERE
            location_structure_id = temp.structure_id
    )
AND EXISTS(
        SELECT
            *
        FROM
            #temp_job temp
        WHERE
            job_structure_id = temp.structure_id
    )