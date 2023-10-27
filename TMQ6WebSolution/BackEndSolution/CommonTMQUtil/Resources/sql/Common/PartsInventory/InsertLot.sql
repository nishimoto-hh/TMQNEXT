/*
* ロット情報登録SQL
* InsertLot.sql
*/
INSERT INTO pt_lot(
     lot_control_id
    ,parts_id
    ,lot_no
    ,old_new_structure_id
    ,department_structure_id
    ,account_structure_id
    ,management_division
    ,management_no
    ,receiving_datetime
    ,unit_structure_id
    ,unit_price
    ,currency_structure_id
    ,vender_structure_id
    ,update_serialid
    ,insert_datetime
    ,insert_user_id
    ,update_datetime
    ,update_user_id
) output inserted.lot_control_id
SELECT
     NEXT VALUE FOR seq_pt_lot_lot_control_id
    ,@PartsId
    ,NEXT VALUE FOR seq_pt_lot_lot_no
    ,@OldNewStructureId
    ,@DepartmentStructureId
    ,@AccountStructureId
    ,@ManagementDivision
    ,@ManagementNo
    ,@ReceivingDatetime
    ,parts.unit_structure_id
    ,@UnitPrice
    ,parts.currency_structure_id
    ,@VenderStructureId
    ,@UpdateSerialid
    ,@InsertDatetime
    ,@InsertUserId
    ,@UpdateDatetime
    ,@UpdateUserId
FROM
     pt_parts AS parts
WHERE
    parts.parts_id = @PartsId