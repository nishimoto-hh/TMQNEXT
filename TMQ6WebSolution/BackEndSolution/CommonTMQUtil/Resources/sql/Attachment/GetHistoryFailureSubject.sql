SELECT
    subject + '略図添付' AS subject,
    @FunctionTypeId AS function_type_id,
    @KeyId AS key_id,
    location_structure_id,
    @DocumentTypeValNo AS document_type_val_no
FROM
    ma_history_failure failure
    LEFT JOIN
        ma_history history
    ON  failure.history_id = history.history_id
    LEFT JOIN
        ma_summary summary
    ON  history.summary_id = summary.summary_id
WHERE
    history_failure_id = @KeyId
