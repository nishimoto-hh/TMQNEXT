SELECT
    si.structure_id
    ,si.factory_id
    ,si.structure_group_id
    ,si.parent_structure_id
    ,si.structure_layer_no
    ,si.structure_item_id
    ,si.item_translation_id
    ,coalesce(coalesce(order_factory.display_order, order_common.display_order), 32768) AS display_order
    ,si.location_structure_id
    ,si.language_id
    ,si.translation_text
    ,si.translation_item_description
    ,ie.item_id
    ,ie.sequence_no AS seq
    ,ie.extension_data AS ex_data
    --     , ie.note
    ,ie.insert_datetime
    ,ie.insert_user_id
    ,ie.update_datetime
    ,ie.update_user_id
FROM
    v_structure_item_all si
    LEFT OUTER JOIN ms_item_extension ie
    ON  si.structure_item_id = ie.item_id
    LEFT OUTER JOIN ms_structure_order AS order_factory
    ON  si.structure_id = order_factory.structure_id
    AND order_factory.factory_id = @FactoryId
    LEFT OUTER JOIN ms_structure_order AS order_common
    ON  si.structure_id = order_common.structure_id
    AND order_common.factory_id = 0
WHERE
    si.structure_group_id = @StructureGroupId
/*@Seq
    AND ie.sequence_no = @Seq
@Seq*/
/*@ExData
    AND ie.extension_data = @ExData
@ExData*/
/*@NotDeleteOnly
    AND si.delete_flg = 0
@NotDeleteOnly*/
/*@LanguageId
    AND si.language_id = @LanguageId
@LanguageId*/  