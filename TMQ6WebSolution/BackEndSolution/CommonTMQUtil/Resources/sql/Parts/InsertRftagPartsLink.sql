DECLARE 
--予備品仕様マスタ
@parts_id bigint,
@department_structure_id bigint,--部門ID
@account_structure_id bigint,--勘定科目ID
--RFタグ予備品マスタ
@rftag_id nvarchar(max),
@serial_no bigint,
@read_datetime datetime

--RFタグ予備品マスタ
BEGIN
    DECLARE cur_rt CURSOR FOR
        WITH user_narrow AS (SELECT * FROM ms_user WHERE user_id = @UserId)
        , user_district AS ( 
            -- ユーザ所属マスタの本務工場の地区を取得
            SELECT
                dbo.get_target_layer_id(ub.location_structure_id, 0) AS district_id 
            FROM
                user_narrow AS us 
                INNER JOIN ms_user_belong AS ub 
                    ON (us.user_id = ub.user_id) 
            WHERE
                ub.duty_flg = 1
        ) 
        , factory AS ( 
            -- 構成マスタより地区配下の工場を取得
            SELECT
                factory_id 
            FROM
                v_structure 
                INNER JOIN user_district AS district 
                    ON parent_structure_id = district.district_id 
            WHERE
                structure_group_id = 1000 
                AND structure_layer_no = 1
        ) 
        SELECT  
                 rf.rftag_id
                ,pt.parts_id
                ,rf.department_structure_id
                ,rf.account_structure_id
                ,rf.read_datetime
                ,ROW_NUMBER() OVER(
                    PARTITION BY pt.parts_id,rf.department_structure_id,rf.account_structure_id
                    ORDER BY pt.parts_id,rf.department_structure_id,rf.account_structure_id
                ) AS serial_no 
        FROM #rf_read_date rf
        INNER JOIN dbo.pt_parts pt on rf.parts_no COLLATE Japanese_bin2 = pt.parts_no
        INNER JOIN factory 
            ON pt.factory_id = factory.factory_id 
        OPEN cur_rt
    FETCH NEXT FROM cur_rt
                INTO
                @rftag_id
                ,@parts_id
                ,@department_structure_id
                ,@account_structure_id
                ,@read_datetime
                ,@serial_no
    WHILE  @@FETCH_STATUS = 0
    BEGIN
        MERGE INTO dbo.pt_rftag_parts_link AS rp
                USING (SELECT @rftag_id AS rftag,@read_datetime AS read_datetime) AS rt
                        
                ON (
                        rp.rftag_id = rt.rftag
                )
                WHEN MATCHED AND rp.update_datetime < rt.read_datetime THEN
                        UPDATE SET
                                 parts_id = @parts_id
                                ,department_structure_id = @department_structure_id
                                ,account_structure_id = @account_structure_id
                                ,serial_no = @serial_no
                                ,update_serialid = update_serialid + 1
                                ,update_datetime = @read_datetime
                                ,update_user_id = @UserId
                WHEN NOT MATCHED THEN
                        INSERT(
                                rftag_id
                                ,parts_id
                                ,department_structure_id
                                ,account_structure_id
                                ,serial_no
                                ,update_serialid
                                ,delete_flg
                                ,insert_datetime
                                ,insert_user_id
                                ,update_datetime
                                ,update_user_id
                                )
                        VALUES(
                                @rftag_id
                                ,@parts_id
                                ,@department_structure_id
                                ,@account_structure_id
                                ,@serial_no
                                ,0
                                ,0
                                ,@read_datetime
                                ,@UserId
                                ,@read_datetime
                                ,@UserId
                        )
        ;
    FETCH NEXT FROM cur_rt INTO
                 @rftag_id
                ,@parts_id
                ,@department_structure_id
                ,@account_structure_id
                ,@read_datetime
                ,@serial_no
    END
    CLOSE cur_rt
    DEALLOCATE cur_rt
END