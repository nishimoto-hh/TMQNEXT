SELECT
      igd.sheet_no
    , igd.control_group_id
    , igd.data_direction
    , igd.record_count
    , icd.control_type
    , icd.start_column_no
    , icd.start_row_no
    , icd.required_flg
    , cd.minimum_value
    , cd.maximum_value
    , cd.maximum_length
    , cd.column_name
    , icd.alias_name
    , cd.data_type
    , oid.ep_relation_id
    , oid.ep_relation_parameters
    , mt.translation_text 
FROM
    ms_input_group_define igd 
    LEFT JOIN ms_input_control_define icd 
        ON igd.report_id = icd.report_id 
        AND igd.sheet_no = icd.sheet_no 
        AND igd.control_group_id = icd.control_group_id 
    LEFT JOIN ms_output_report_item_define oid 
        ON igd.report_id = oid.report_id 
        AND igd.sheet_no = oid.sheet_no
        AND icd.control_type = oid.control_type 
        AND icd.control_id = oid.control_id 
    LEFT JOIN cm_control_define cd 
        ON icd.control_type = cd.control_type 
        AND icd.control_id = cd.control_id 
    LEFT JOIN ms_translation mt 
        ON cd.control_id = mt.translation_id 
        AND mt.language_id = @LanguageId 
        AND mt.location_structure_id = @FactoryId
WHERE
    igd.report_id = @ReportId 
    AND igd.sheet_no = @SheetNo 
    AND igd.control_group_id = @ControlGroupId 
ORDER BY
    icd.start_row_no
    , icd.start_column_no
