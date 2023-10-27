select
    * 
from
    ( 
        select distinct
            fd.program_id as PgmId
            , fd.group_no as GrpNo
            , fd.control_group_id as CtrlId
            , fcd.control_no as ItemNo
            , fcd.control_type as CtrlType
            , ISNULL(fcd.detailed_search_control_type, fcd.control_type) as DetailSearchCtrlType
            , fcd.from_to_division as FromToKbn
            , trans.translation_text as Format
            , fcd.expansion_table_name as TblName
            , cd.column_name as ColOrgName
            , cd.data_type as DataType
            , cd2.column_name as DetailSearchColOrgName
            , ISNULL(cd2.data_type, cd.data_type) as DetailSearchDataType
            , fcd.expansion_alias_name as AliasName
            , fcd.expansion_parameters_name as ParamOrgName
            , fcd.expansion_key_name as KeyName
            , coalesce(fcd.expansion_like_pattern, 0) as LikePattern
            , coalesce(fcd.expansion_in_clause_division, 0) as InClauseKbn
            , coalesce(fcd.expansion_lock_type, 0) as LockType
            , fcd.expansion_lock_table_name as LockTblName
        from
            cm_form_define fd 
            join cm_form_control_define fcd 
                on fd.program_id = fcd.program_id 
                and fd.form_no = fcd.form_no 
                and fd.control_group_id = fcd.control_group_id 
                and ( 
                    fd.common_form_no is null 
                    or fd.common_form_no = 0
                ) 
            left join cm_control_define cd 
                on fcd.control_id = cd.control_id 
                and fcd.control_type = cd.control_type 
            left join cm_control_define cd2
                on fcd.control_id = cd2.control_id 
                and fcd.detailed_search_control_type = cd2.control_type
            left join ms_translation trans 
                on cd.format_translation_id = trans.translation_id 
                /*IF LanguageId != null && LanguageId != ''*/
                and trans.language_id = /*LanguageId*/'ja'
                /*END*/
                and trans.delete_flg = 0
        where
            fcd.define_type = 1 
            and fcd.delete_flg = 0 
            
        union 
        
        select distinct
            fd.program_id as PgmId
            , fd.group_no as GrpNo
            , fd.control_group_id as CtrlId
            , fcd.control_no as ItemNo
            , fcd.control_type as CtrlType
            , ISNULL(fcd.detailed_search_control_type, fcd.control_type) as DetailSearchCtrlType
            , fcd.from_to_division as FromToKbn
            , trans.translation_text as Format
            , fcd.expansion_table_name as TblName
            , cd.column_name as ColOrgName
            , cd.data_type as DataType
            , cd2.column_name as DetailSearchColOrgName
            , ISNULL(cd2.data_type, cd.data_type) as DetailSearchDataType
            , fcd.expansion_alias_name as AliasName
            , fcd.expansion_parameters_name as ParamOrgName
            , fcd.expansion_key_name as KeyName
            , coalesce(fcd.expansion_like_pattern, 0) as LikePattern
            , coalesce(fcd.expansion_in_clause_division, 0) as InClauseKbn
            , coalesce(fcd.expansion_lock_type, 0) as LockType 
            , fcd.expansion_lock_table_name as LockTblName
        from
            cm_form_define fd 
            join cm_form_control_define fcd 
                on fd.common_form_no = fcd.form_no 
                and fd.common_form_no is not null 
                and fd.common_form_no != 0 
                and fcd.program_id = '0' 
            left join cm_control_define cd 
                on fcd.control_id = cd.control_id 
                and fcd.control_type = cd.control_type 
            left join cm_control_define cd2
                on fcd.control_id = cd2.control_id 
                and fcd.detailed_search_control_type = cd2.control_type
            left join ms_translation trans 
                on cd.format_translation_id = trans.translation_id 
                /*IF LanguageId != null && LanguageId != ''*/
                and trans.language_id = /*LanguageId*/'ja'
                /*END*/
                and trans.delete_flg = 0
        where
            fcd.define_type = 1 
            and fcd.delete_flg = 0
    ) as listitem 
where
/*IF PgmId != null && PgmId != ''*/
    listitem.PgmId = /*PgmId*/'MA0001' 
/*END*/

/*IF GrpNo != null && GrpNo != ''*/
    and listitem.GrpNo = /*GrpNo*/1
/*END*/

/*IF CtrlId != null && CtrlId != ''*/
    and listitem.CtrlId = /*CtrlId*/'L_M_Search'
/*END*/

/*IF ItemNo != null */
    and listitem.ItemNo = /*ItemNo*/0
/*END*/

order by
    PgmId
    , GrpNo
    , CtrlId
    , ItemNo
