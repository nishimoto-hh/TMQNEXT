SELECT
    mu.user_id AS userid
    , mu.login_id AS loginid
    , mu.display_name AS username
    , mu.language_id AS languageid
    , CONVERT(int, ISNULL(ie.extension_data, '10')) AS authorityLevelId -- '10'=ゲストユーザー
FROM
    ms_user mu 
LEFT JOIN ms_login ml 
    ON mu.user_id = ml.user_id
LEFT JOIN ms_structure ms
    ON  mu.authority_level_id = ms.structure_id
LEFT JOIN ms_item mi
    ON  ms.structure_item_id = mi.item_id
    AND mi.delete_flg = 0
LEFT JOIN ms_item_extension ie
    ON  mi.item_id = ie.item_id
    AND ie.sequence_no = 1
    
WHERE
    mu.delete_flg != 1 
AND ml.delete_flg != 1 
AND ms.delete_flg != 1 
AND mu.login_id = /*LoginId*/'demo' 
/*IF Password != null*/
    AND CONVERT(NVARCHAR, DECRYPTBYPASSPHRASE(/*EncryptKey*/'key',ml.login_password)) = /*Password*/'demo'
/*END*/
