--******************************************************************
--ユーザー一覧
--******************************************************************
SELECT DISTINCT
    mus.user_id                                 --ユーザーID
    , mus.login_id                              --ログインID
    , mus.display_name                          --表示名
    , mus.family_name                           --姓
    , mus.first_name                            --名
    , mus.mail_address                          --メールアドレス
    , ( 
        SELECT                                  --権限レベル
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
                    st_f.structure_id = mus.authority_level_id
                    AND st_f.factory_id = 0
            ) 
            AND tra.structure_id = mus.authority_level_id
    ) AS authority_level
    , ( 
        SELECT                                   --言語
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
                    st_f.structure_id = lng.structure_id
                    AND st_f.factory_id = 0
            ) 
            AND tra.structure_id = lng.structure_id
    ) AS language_structure_name
    , mus.delete_flg                            --削除フラグ
    , mus.update_serialid                       --更新シリアルID
    , mus.authority_level_id                    --権限レベル(子画面用)
    , lng.structure_id AS language_structure_id --言語ID(子画面用)
FROM
    ms_user mus 
    LEFT JOIN ms_user_belong mub
        ON mus.user_id = mub.user_id
    LEFT JOIN languageId lng --言語
        ON mus.language_id = lng.extension_data
WHERE
    (mus.delete_flg = 0 OR mus.delete_flg = @DeleteFlg)
/*@locationIdList
    AND mub.location_structure_id IN @locationIdList
@locationIdList*/
/*@MailAddress
    AND mus.mail_address LIKE '%' + @MailAddress + '%'
@MailAddress*/
/*@AuthorityLevelId
    AND mus.authority_level_id = @AuthorityLevelId
@AuthorityLevelId*/
/*@UserName
    AND mus.display_name LIKE '%' + @UserName + '%'
    OR mus.family_name LIKE '%' + @UserName + '%'
    OR mus.first_name LIKE '%' + @UserName + '%'
@UserName*/