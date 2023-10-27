UPDATE mc_mp_information 
SET
	  mp_information = @MpInformation
    , update_serialid = update_serialid+1
    , update_datetime = @UpdateDatetime
    , update_user_id = @UpdateUserId
WHERE
    mp_information_id = @MpInformationId
