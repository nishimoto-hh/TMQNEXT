INSERT INTO ms_translation(
    [location_structure_id]
    ,[translation_id]
    ,[language_id]
    ,[translation_text]
    ,[translation_item_description]
    ,[update_serialid]
    ,[insert_datetime]
    ,[insert_user_id]
    ,[update_datetime]
    ,[update_user_id]
) OUTPUT inserted.translation_id
VALUES(
    @LocationStructureId
    ,@TranslationId
    ,@LanguageId
    ,@TranslationText
    ,@TranslationItemDescription
    ,@UpdateSerialid
    ,@InsertDatetime
    ,@InsertUserId
    ,@UpdateDatetime
    ,@UpdateUserId
)
