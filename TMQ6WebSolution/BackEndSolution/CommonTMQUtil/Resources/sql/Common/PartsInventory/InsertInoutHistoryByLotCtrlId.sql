/*
* 受払履歴登録(出庫入力)
* InsertInoutHistoryByLotCtrlId.sql
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
    ,shipping_division_structure_id
    ,update_serialid
    ,insert_datetime
    ,insert_user_id
    ,update_datetime
    ,update_user_id
)
SELECT
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
    ,lt.lot_control_id
    ,@InventoryControlId
    ,lt.department_structure_id
    ,lt.account_structure_id
    ,lt.management_division
    ,lt.management_no
    ,@InoutDatetime
    ,@InoutQuantity
    ,COALESCE(@ShippingDivisionStructureId
         -- 出庫区分 払出(受払区分が2)の場合、出庫区分(1980)の拡張項目が通常(1)の構成ID。受入の場合Null
         ,CASE @InoutDivision
         WHEN '2' THEN 
            (SELECT st.structure_id
               FROM
                    v_structure AS st INNER JOIN ms_item_extension AS ex
                    ON (st.structure_item_id = ex.item_id AND ex.sequence_no = 1)
             WHERE
                    st.structure_group_id = 1980
                AND ex.extension_data = '1')
         ELSE NULL END
     )
    ,@UpdateSerialid
    ,@InsertDatetime
    ,@InsertUserId
    ,@UpdateDatetime
    ,@UpdateUserId
FROM
    pt_lot AS lt
WHERE
    lt.lot_control_id = @LotControlId
