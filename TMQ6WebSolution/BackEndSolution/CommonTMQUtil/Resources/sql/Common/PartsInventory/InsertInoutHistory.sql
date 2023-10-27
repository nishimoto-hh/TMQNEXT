/*
* 受払履歴登録
* InsertInoutHistory.sql
*/
-- 受払区分と作業区分を拡張項目から取得する
WITH kbn AS(
     SELECT
        st.structure_group_id
       ,st.structure_id
       ,ex.extension_data
    FROM
        v_structure AS st
        INNER JOIN
            ms_item_extension AS ex
        ON  (
                ex.item_id = st.structure_item_id
            AND ex.sequence_no = 1
            )
    WHERE
        st.structure_group_id IN (1950, 1960)
)
INSERT INTO pt_inout_history(
     inout_history_id
    ,inout_division_structure_id
    ,work_division_structure_id
    ,work_no
    ,lot_control_id
    ,inventory_control_id
    ,department_structure_id
    ,account_structure_id
    ,management_division
    ,management_no
    ,inout_datetime
    ,inout_quantity
    ,update_serialid
    ,insert_datetime
    ,insert_user_id
    ,update_datetime
    ,update_user_id
)
VALUES(
     NEXT VALUE FOR seq_pt_inout_history_inout_history_id
    ,(
        SELECT
            structure_id
        FROM
            kbn
        WHERE
            structure_group_id = 1950
        AND extension_data = @InoutDivision
    )
    ,(
        SELECT
            structure_id
        FROM
            kbn
        WHERE
            structure_group_id = 1960
        AND extension_data = @WorkDivision
    )
    ,@WorkNo
    ,@LotControlId
    ,@InventoryControlId
    ,@DepartmentStructureId
    ,@AccountStructureId
    ,@ManagementDivision
    ,@ManagementNo
    ,@InoutDatetime
    ,@InoutQuantity
    ,@UpdateSerialid
    ,@InsertDatetime
    ,@InsertUserId
    ,@UpdateDatetime
    ,@UpdateUserId
)