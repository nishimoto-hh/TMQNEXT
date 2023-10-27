/*
 * ワークフロー操作ログ新規登録SQL
 */

insert into workflow_log
(
    wf_no,
    log_seq,
    user_cd,
    operation,
    operation_date,
    comments,
    input_date,
    inputor_cd,
    update_date,
    updator_cd
)
values
(
    /*WfNo*/'%',
    /*LogSeq*/0,
    /*UserCd*/null,
    /*Operation*/null,
    /*OperationDate*/null,
    /*Comments*/null,
    /*InputDate*/null,
    /*InputUserId*/null,
    /*UpdateDate*/null,
    /*UpdateUserId*/null
)
