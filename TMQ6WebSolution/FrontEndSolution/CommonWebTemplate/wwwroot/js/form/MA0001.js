/* ========================================================================
 *  機能名　    ：   【MA0001】保全活動
 * ======================================================================== */

/**
 * 自身の相対パスを取得
 */
function getPath() {
    var root;
    var scripts = document.getElementsByTagName("script");
    var i = scripts.length;
    while (i--) {
        var match = scripts[i].src.match(/(^|.*\/)MA0001\.js$/);
        if (match) {
            root = match[1];
            break;
        }
    }
    return root;
}

document.write("<script src=\"" + getPath() + "/tmqcommon.js\"></script>");
document.write("<script src=\"" + getPath() + "/DM0002.js\"></script>");
document.write("<script src=\"" + getPath() + "/SU0001.js\"></script>");
document.write("<script src=\"" + getPath() + "/RM0001.js\"></script>");

//機能ID
const ConductId_MA0001 = "MA0001";

// 活動区分ID(点検：1、故障：2)
const DivisionIdDefine = {
    Inspection: 1,
    Failure: 2
};

//新規登録時の保全活動件名ID
const NewSummaryId = "-1";

//系停止：なし
const StopSystemVal = 0;

//件名別長期計画・機器別長期計画の白丸「○」リンクから遷移してきた際にグローバル変数に値を格納するキー
const KeyNameFromScheduleLink = "MakeScheduleFromLongPlan";

// 個別工場表示対象フラグ(非表示：0、表示：1)
const IndividualFlg = {
    Hide: "0",
    Show: "1"
};
// 管理基準
const ManagementDefine = {
    //管理基準外
    NotManagement: "0",
    //管理基準
    Management: "1"
};

// 一覧画面
const FormList = {
    // 画面No
    No: 0,
    //非表示の情報
    HideInfo: {
        Id: "BODY_010_00_LST_0",
        //製造権限
        Manufacturing: 2,
        //保全権限
        Maintenance: 3,
        //日報出力ボタン表示フラグ
        DailyReportBtnDispFlg: 4,
    },
    //フィルタ
    Filter: {
        Id: "BODY_030_00_LST_0",
        Input: 1,
    },
    //一覧
    List: {
        Id: "BODY_020_00_LST_0",
        //MQ分類
        MqClass: 4,
        //職種
        Job: 13,
        //系停止
        StopSystem: 17,
        //突発区分
        Sudden: 20,
    },
    ButtonId: {
        //点検情報登録
        NewInspection: "NewInspection",
        //故障情報登録
        NewFailure: "NewFailure",
        //新規登録
        New: "New",
        //出力
        Output: "Output",
        //工程表の表示
        ProcessReport: "ProcessReport",
        //建材日報出力
        DailyReportOutput: "DailyReportOutput",
    },
};
// 詳細画面
const FormDetail = {
    // 画面No
    No: 1,
    // 非表示の情報
    HideInfo: {
        Id: "BODY_010_00_LST_1",
        // 活動区分ID列
        DivisionId: 2,
        // 完了日列
        CompletionDate: 3,
        // フォロー計画有無列
        FollowPlanFlg: 4,
        //保全活動件名ID
        SummaryId: 6,
        //フォロー計画キーID
        FollowPlanKeyId: 8,
        //製造権限
        Manufacturing: 9,
        //保全権限
        Maintenance: 10,
        //故障分析個別工場表示フラグ
        FailureIndividualFlg: 11,
        //保全履歴個別工場表示フラグ
        HistoryIndividualFlg: 12,
        //タブ番号
        TabNo: 13,
    },
    //場所階層職種情報
    LocationInfo: {
        Id: "BODY_030_00_LST_1",
    },
    // 対象機器一覧
    MachineList: {
        Id: "BODY_050_00_LST_1",
        // 非表示列
        HideCol: { InspectionSite: 10, InspectionContent: 11, FollowFlg: 12, FollowUpPlanDate: 13, FollowContent: 14, FollowComplitionDate: 15 },
        // 行のグレーアウトを判定する項目
        GrayFlag: 17,
    },
    //依頼概要
    RequestList: {
        Id: "BODY_150_00_LST_1",
        //依頼担当
        Personnel: 1,
        //依頼係長
        Chief: 3,
        //依頼課長
        Manager: 4,
        //依頼職長
        Foreman: 8,
        //依頼担当名
        PersonnelName: 5,
        //依頼係長名
        ChiefName: 6,
        //依頼課長名
        ManagerName: 7,
        //依頼職長名
        ForemanName: 9,
    },
    //履歴情報
    HistoryList: {
        Id: "BODY_190_00_LST_1",
        //施工担当者
        ConstructionPersonnel: 6,
        //施工担当者名
        ConstructionPersonnelName: 12,
    },
    //履歴情報(個別工場)
    HistoryIndividualList: {
        Id: "BODY_230_00_LST_1",
        //SEG担当者
        SegPersonnel: 1,
        //製造担当者
        ConstructionPersonnel: 2,
        //SEG担当者名
        SegPersonnelName: 12,
        //製造担当者名
        ConstructionPersonnelName: 13,
    },
    // 故障情報
    FailureList: {
        Id: "BODY_060_00_LST_1",
        GroupNo: 204,
    },
    // 故障分析情報タブ
    FailureTab: {
        Id: "BODY_300_00_LST_1",
        // 保全履歴故障情報ID列
        HistoryFailureId: 11,
    },
    // 故障分析情報(個別工場)タブ
    FailureIndividualTab: {
        //再発防止対策(要/非～実施予定日)
        List1: {
            Id: "BODY_350_00_LST_1",
        },
        //再発防止対策(教訓～故障原因分析書)
        List2: {
            Id: "BODY_380_00_LST_1",
            //保全履歴故障情報ID列
            HistoryFailureId: 5,
        },
    },
    ButtonId: {
        //複写
        Copy: "Copy",
        //件名添付
        AddSubject: "AddSubject",
        //機器交換
        EquipReplace: "EquipReplace",
        //フォロー計画
        Follow: "Follow",
        //修正
        Edit: "Edit",
        //削除
        Delete: "Delete",
        //略図添付
        AddFailureDiagram: "AddFailureDiagram",
        //故障原因分析書添付
        AddFailureAnalyze: "AddFailureAnalyze",
    },
    //タブ番号
    TabNo: {
        //保全依頼情報
        Request: 1,
        //保全計画情報
        Plan: 2,
        //保全履歴情報
        History: 3,
        //保全履歴情報（個別工場）
        HistoryIndividual: 4,
        //故障分析情報
        Failure: 5,
        //故障分析情報（個別工場）
        FailureIndividual: 6,
    },
    // フォーカスを設定するコントロールID
    ForcusId: "AddSubject",
};
// 登録・修正画面
const FormRegist = {
    // 画面No
    No: 2,
    //依頼No、非表示の情報
    HideInfo: {
        Id: "BODY_000_00_LST_2",
        //依頼No
        RequestNo: 1,
        // 活動区分ID
        DivisionId: 2,
        //保全活動件名ID
        SummaryId: 6,
        //MQ分類：突発区分を必須にしない構成ID（カンマ区切り）
        MqNotRequiredStructureId: 7,
        //製造権限
        Manufacturing: 9,
        //保全権限
        Maintenance: 10,
        //故障分析個別工場表示フラグ
        FailureIndividualFlg: 11,
        //保全履歴個別工場表示フラグ
        HistoryIndividualFlg: 12,
    },
    //件名情報
    SummaryInfo: {
        Id: "BODY_010_00_LST_2",
        //件名
        Subject: 1,
        //作業計画・実施内容
        Content: 2,
    },
    //場所階層情報
    LocationInfo: {
        Id: "BODY_020_00_LST_2",
        //工場
        Factory: 2,
    },
    //作業性格情報
    WorkInfo: {
        Id: "BODY_030_00_LST_2",
        //MQ分類
        MqClass: 1,
        //予算性格区分
        BudgetPersonality: 4,
        //突発区分
        SuddenDivision: 5,
        //系停止
        StopSystem: 6,
        //系停止時間
        StopTime: 7,
    },
    //対象機器一覧
    MachineList: {
        Id: "BODY_040_00_LST_2",
        //フォロー予定年月
        FollowPlanDate: 13,
        //フォロー完了日
        FollowCompletionDate: 15,
        // 機器使用期間
        UseDays: 16,
        // 機番ID
        MachineId: 24,
        // ラベル化する列
        LabelCol: { FollowFlg: 12, FollowUpPlanDate: 13, FollowContent: 14, FollowComplitionDate: 15 },
    },
    // 故障情報
    FailureList: {
        List1: {
            Id: "BODY_050_00_LST_2",
            //保全部位
            InspectionSite: 1,
            //保全内容
            InspectionContent: 2,
        },
        List2: {
            Id: "BODY_060_00_LST_2",
            //フォロー予定年月
            FollowPlanDate: 2,
            //フォロー完了日
            FollowCompletionDate: 4,
        },
        //グループ番号
        GroupNo: 604,
    },
    //依頼概要
    RequestList: {
        List1: {
            Id: "BODY_100_00_LST_2",
            //依頼内容
            RequestContent: 1,
        },
        List2: {
            Id: "BODY_110_00_LST_2",
            //発行日
            IssueDate: 1,
            //着手希望日
            DesiredStartDate: 4,
            //完了希望日
            DesiredEndDate: 5,
        },
        List3: {
            Id: "BODY_130_00_LST_2",
            //依頼担当
            Personnel: 1,
            //依頼係長
            Chief: 3,
            //依頼課長
            Manager: 4,
            //依頼職長
            Foreman: 8,
            //依頼担当名
            PersonnelName: 5,
            //依頼係長名
            ChiefName: 6,
            //依頼課長名
            ManagerName: 7,
            //依頼職長名
            ForemanName: 9,
        }
    },
    //計画情報
    PlanList: {
        Id: "BODY_170_00_LST_2",
        //実施件名
        Subject: 1,
        //着工予定日
        ExpectedConstructionDate: 3,
        //完了予定日
        ExpectedCompletionDate: 4,

    },
    //履歴情報
    HistoryList: {
        Id: "BODY_190_00_LST_2",
        //着工日
        ConstructionDate: 1,
        //完了日
        CompletionDate: 2,
        //呼出
        CallCount: 4,
        //施工担当者
        ConstructionPersonnel: 6,
        //施工担当者
        ConstructionPersonnelName: 12,
    },
    //履歴情報(個別工場)
    HistoryIndividualList: {
        Id: "BODY_240_00_LST_2",
        //SEG担当者
        SegPersonnel: 1,
        //製造担当者
        ConstructionPersonnel: 2,
        //発生時刻
        OccurrenceTime: 6,
        //完了日
        CompletionDate: 7,
        //完了時刻
        CompletionTime: 8,
        //SEG担当者名
        SegPersonnelName: 12,
        //製造担当者名
        ConstructionPersonnelName: 13,
    },
    //故障分析情報タブ
    FailureTab: {
        Id: "BODY_310_00_LST_2",
        //略図
        FailureDiagram: 9,
        //故障原因分析書
        FailureAnalyze: 10,
        //保全履歴故障情報ID列
        HistoryFailureId: 11,
    },
    //故障分析情報(個別工場)タブ
    FailureIndividualTab: {
        //再発防止対策(要/非～実施予定日)
        List1: {
            Id: "BODY_360_00_LST_2",
            //実施予定日列
            MeasurePlanDate: 2,
        },
        //再発防止対策(教訓～故障原因分析書)
        List2: {
            Id: "BODY_390_00_LST_2",
            //略図
            FailureDiagram: 3,
            //故障原因分析書
            FailureAnalyze: 4,
            //保全履歴故障情報ID列
            HistoryFailureId: 5,
        },
    },
    ButtonId: {
        //一括クリア
        AllClear: "AllClear",
        //登録
        Regist: "Regist",
    },
    //タブ番号
    TabNo: {
        //保全依頼情報
        Request: 1,
        //保全計画情報
        Plan: 2,
        //保全履歴情報
        History: 3,
        //保全履歴情報（個別工場）
        HistoryIndividual: 4,
        //故障分析情報
        Failure: 5,
        //故障分析情報（個別工場）
        FailureIndividual: 6,
    },
};
// 機器交換画面
const FormReplace = {
    // 画面No
    No: 3,
    //機器交換
    ReplaceList: {
        //現場機器
        CurrentInfo: {
            Id: "BODY_000_00_LST_3",
            //機番ID
            MachineId: 4,
        },
        //代替機器
        ReplaceInfo: {
            Id: "BODY_030_00_LST_3",
            //機番ID
            MachineId: 4,
        },
        GroupNo: 630,
    },
    //機器交換確認
    ConfirmList: {
        //現場機器(機器番号～設置場所)
        CurrentInfo1: {
            Id: "BODY_060_00_LST_3",
            //機番ID
            MachineId: 4,
        },
        //現場機器(交換前～交換後)
        CurrentInfo2: {
            Id: "BODY_070_00_LST_3",
            //(交換後)使用区分コンボ
            AfterUseSegmentStructureId: 4,
            //(交換後)使用区分ラベル
            AfterUseSegmentStructureIdLabel: 6,
        },
        //代替機器(機器番号～設置場所)
        ReplaceInfo1: {
            Id: "BODY_080_00_LST_3",
            //機番ID
            MachineId: 4,
        },
        //代替機器(交換前～交換後)
        ReplaceInfo2: {
            Id: "BODY_090_00_LST_3",
            //(交換後)使用区分コンボ
            AfterUseSegmentStructureId: 4,
            //(交換後)使用区分ラベル
            AfterUseSegmentStructureIdLabel: 6,
        },
        GroupNo: 631,
    },
    ButtonId: {
        //代替機器検索ボタン
        SearchMachine: "SearchMachine",
        //確認
        Confirm: "Confirm",
        //登録
        Regist: "Regist",
    },
};
// 機器検索画面
const FormSearch = {
    // 画面No
    No: 4,
    //検索条件 場所階層
    StructureCondition: {
        Id: "COND_010_00_LST_4",
    },
    //検索条件 職種機種
    JobCondition: {
        Id: "COND_020_00_LST_4",
    },
    // 検索条件 詳細情報
    SearchList: {
        Id: "COND_030_00_LST_4",
        //使用区分
        UseSegment: 1,
        // スケジュール管理
        Schedule: 9,
        // 保全部位
        InspectionSite: 11,
        // 保全項目
        InspectionContent: 12,
    },
    //一覧
    List: {
        Id: "BODY_050_00_LST_4",
        // 機番ID
        MachineId: 25,
    },
    ButtonId: {
        //閉じる
        Cancel: "Cancell",
        //代替機器検索
        SearchMachine: "SearchMachine",
    },
};
// 機器選択画面
const FormSelectMachine = {
    No: 5,
    //検索条件 場所階層
    StructureCondition: {
        Id: "COND_000_00_LST_5",
    },
    //検索条件 職種機種
    JobCondition: {
        Id: "COND_010_00_LST_5",
    },
    //検索条件 詳細情報
    DetailCondition: {
        Id: "COND_020_00_LST_5",
        //使用区分
        UseSegment: 1,
        // スケジュール管理
        Schedule: 9,
        // 保全部位
        InspectionSite: 11,
        // 保全項目
        InspectionContent: 12,
    },
    //検索条件 非表示項目
    HideInfo: {
        Id: "COND_030_00_BTN_5",
        // 活動区分ID
        DivisionId: 1,
        // 管理基準フラグ（管理基準の場合1、管理基準外の場合0）
        ManagementStandard: 2,
    },
    //機器選択一覧
    List: {
        Id: "BODY_050_00_LST_5",
        //選択（ボタン）列
        Select: 1,
        //保全部位
        InspectionSite: 15,
        //保全内容
        InspectionContent: 16,
    },
    ButtonId: {
        //管理基準外の保全内容を追加する
        ChangeNotManagement: "ChangeNotManagement",
        //管理基準から保全内容を追加する
        ChangeManagement: "ChangeManagement",
        //確定
        Confirm: "Confirm",
        //選択
        Select: "Select",
        //閉じる
        Cancel: "Cancel",
    },
};

// グローバル変数のキー、機器検索画面で選択した機番ID
const MA0001_selectMachineId = "MA0001_SelectMachineId";
// グローバル変数のキー、機器選択画面のデータ
const MA0001_SelectMachineList = "MA0001_SelectMachineList";

// 保全実績評価から遷移時の検索条件
const MA0001_DetailSearchCondition = "MA0001_DetailSearchCondition";
// グローバル変数のキー、構成グループID
const MA0001_GroupId = "MA0001_GroupId";
// グローバル変数のキー、拡張データ
const MA0001_ExtensionData = "MA0001_ExtensionData";
// グローバル変数のキー、構成ID
const MA0001_StructureId = "MA0001_StructureId";
// グローバル変数のキー、職種
const MA0001_JobId = "MA0001_JobId";

/**
 * 【オーバーライド用関数】
 * 　初期化処理(表示中画面用)
 *
 *  @appPath {string}   　　　　     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId {string} 　　　　     ：機能ID
 *  @formNo {byte}　　　　　　　     ：画面番号
 *  @articleForm {article要素}       ：表示画面ｴﾘｱ要素
 *  @curPageStatus {定数：pageStatus}：画面表示ｽﾃｰﾀｽ
 *  @actionCtrlId {string} 　　　　　：Action(ﾎﾞﾀﾝなど)CTRLID
 *  @data {List<Dictionary<string, object>>}    ：初期表示ﾃﾞｰﾀ
 */
function initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data) {

    // 機能IDが「帳票出力」の場合
    if (getConductId() == RM00001_ConductId) {
        return RM0001_initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data);
    }

    // 共通-文書管理詳細画面の初期化処理
    DM0002_initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data);

    if (conductId != ConductId_MA0001) {
        // 共通画面の場合
        return;
    }

    if (formNo == FormList.No) {
        //一覧画面

        //製造権限フラグの値取得
        var manufacturingStr = getValue(FormList.HideInfo.Id, FormList.HideInfo.Manufacturing, 1, CtrlFlag.Label);
        var manufacturingFlg = convertStrToBool(manufacturingStr);
        //保全権限フラグの値取得
        var maintenanceStr = getValue(FormList.HideInfo.Id, FormList.HideInfo.Maintenance, 1, CtrlFlag.Label);
        var maintenanceFlg = convertStrToBool(maintenanceStr);
        if (!maintenanceFlg) {
            //ユーザの役割に「保全権限」が含まれない場合は点検情報登録、故障情報登録を非表示とする

            //点検情報登録ボタン非表示
            setHideButton(FormList.ButtonId.NewInspection, true);
            //故障情報登録ボタン非表示
            setHideButton(FormList.ButtonId.NewFailure, true);

            //フォーカス設定
            var isUnAvailable = isUnAvailableButton(FormList.ButtonId.New);
            if (isUnAvailable) {
                setFocusButton(FormList.ButtonId.Output);
            } else {
                setFocusButton(FormList.ButtonId.New);
            }
        }
        if (!manufacturingFlg || (manufacturingFlg && maintenanceFlg)) {
            //ユーザの役割に「製造権限」が含まれない場合は新規登録を非表示とする
            //ユーザの役割に「製造権限」「保全権限」の両方が含まれる場合は、新規登録を非表示とする

            //新規登録ボタン非表示
            setHideButton(FormList.ButtonId.New, true);

            //フォーカス設定
            var isUnAvailable = isUnAvailableButton(FormList.ButtonId.NewInspection);
            if (isUnAvailable) {
                setFocusButton(FormList.ButtonId.Output);
            } else {
                setFocusButton(FormList.ButtonId.NewInspection);
            }
        }

        //日報出力ボタンの表示制御
        //日報出力ボタン表示フラグの値取得
        var dailyReportBtnDispStr = getValue(FormList.HideInfo.Id, FormList.HideInfo.DailyReportBtnDispFlg, 1, CtrlFlag.Label);
        var dailyReportBtnDispFlg = convertStrToBool(dailyReportBtnDispStr);
        if (!dailyReportBtnDispFlg) {
            //日報出力ボタン非表示
            setHideButton(FormList.ButtonId.DailyReportOutput, true);
        }

        //保全実績評価から遷移してきた場合、詳細検索条件を設定する
        setDetailSearchConditionFromMP0001();

        // 件名別長期計画・機器別長期計画で白丸「○」がクリックされて遷移してきた場合、新規登録画面(点検情報)を表示する
        dispFormEditFromScheduleLink();

    } else if (formNo == FormDetail.No) {
        //詳細画面

        //保全活動区分（非表示）の値取得
        var divisionId = getValue(FormDetail.HideInfo.Id, FormDetail.HideInfo.DivisionId, 1, CtrlFlag.Label);
        if (divisionId == DivisionIdDefine.Inspection) {
            //点検の場合

            //故障情報を非表示
            toggleHideGroup(FormDetail.FailureList.GroupNo, true);
            //故障分析情報タブ、故障分析情報(個別工場)タブを非表示
            setHideTab(FormDetail.FailureTab.Id, formNo, true);
            setHideTab(FormDetail.FailureIndividualTab.List1.Id, formNo, true);
        } else if (divisionId == DivisionIdDefine.Failure) {
            //故障の場合

            //故障情報を表示
            toggleHideGroup(FormDetail.FailureList.GroupNo, false);
            //故障分析情報タブ、故障分析情報（個別工場）タブの表示切り替え
            changeDisplayFailureTab(formNo);
        }

        //フォロー計画有無（非表示）の値取得
        var followPlanFlg = getValue(FormDetail.HideInfo.Id, FormDetail.HideInfo.FollowPlanFlg, 1, CtrlFlag.Label);
        if (followPlanFlg == 1) {
            setHideButton(FormDetail.ButtonId.Follow, true);
        } else {
            setHideButton(FormDetail.ButtonId.Follow, false);
        }

        //削除ボタン表示制御
        //保全権限がない場合かつ「進捗状況」が「保全受付」「完了済」の場合は、削除ボタンを非表示とする
        //「完了済」：完了日に値が入っている場合
        //「保全受付」：保全履歴情報タブの施工担当者または保全履歴情報（個別工場）タブの製造担当者に値が入っている場合
        changeDisplayDeleteButton();

        //保全履歴タブ、保全履歴（個別工場）タブの表示切り替え
        changeDisplayHistoryTab(formNo);

        //他機能から遷移してきた場合、指定されたタブを表示する
        var tabNo = getValue(FormDetail.HideInfo.Id, FormDetail.HideInfo.TabNo, 1, CtrlFlag.Label);
        switch (Number(tabNo)) {
            case FormDetail.TabNo.Request: //依頼タブ
                //依頼タブを選択
                selectTab(tabNo);
                break;
            case FormDetail.TabNo.History: //履歴タブ
                //保全履歴個別工場表示フラグの値取得
                var historyIndividualFlg = getValue(FormDetail.HideInfo.Id, FormDetail.HideInfo.HistoryIndividualFlg, 1, CtrlFlag.Label);
                //表示しているタブを選択
                if (historyIndividualFlg == IndividualFlg.Show) {
                    selectTab(FormDetail.TabNo.HistoryIndividual);
                } else {
                    selectTab(FormDetail.TabNo.History);
                }
                break;
        }

        //フォーカス設定
        setFocusButton(FormDetail.ForcusId);
    } else if (formNo == FormRegist.No) {
        //登録・修正画面

        //アクション(ﾎﾞﾀﾝなど)のCTRLID
        P_dicIndividual["actionCtrlId"] = actionCtrlId;

        //製造権限フラグの値取得
        var manufacturingStr = getValue(FormRegist.HideInfo.Id, FormRegist.HideInfo.Manufacturing, 1, CtrlFlag.Label);
        var manufacturingFlg = convertStrToBool(manufacturingStr);
        //保全権限フラグの値取得
        var maintenanceStr = getValue(FormRegist.HideInfo.Id, FormRegist.HideInfo.Maintenance, 1, CtrlFlag.Label);
        var maintenanceFlg = convertStrToBool(maintenanceStr);
        if (manufacturingFlg && !maintenanceFlg) {
            //ユーザの役割に「製造権限」のみが含まれる場合

            //依頼情報タブのみ表示
            var tabBtns = $(P_Article).find(".tab_btn.detail a");
            if (tabBtns != null && tabBtns.length > 0) {
                $.each($(tabBtns), function (i, btn) {
                    //タブ番号
                    var tabNo = $(btn).data('tabno');
                    if (tabNo != FormRegist.TabNo.Request) {
                        //タブボタンを非表示にする
                        $(btn).hide();
                    }
                });
            }

            //故障情報は非表示
            toggleHideGroup(FormRegist.FailureList.GroupNo, true);
        } else if (!manufacturingFlg && maintenanceFlg) {
            //ユーザの役割に「保全権限」のみが含まれる場合

            //依頼情報タブを除く全てのタブを表示
            setHideTab(FormRegist.RequestList.List1.Id, formNo, true);
            var tabBtns = $(P_Article).find(".tab_btn.detail a");
            if (tabBtns != null && tabBtns.length > 0) {
                $.each($(tabBtns), function (i, btn) {
                    //タブ番号
                    var tabNo = $(btn).data('tabno');
                    if (tabNo == FormRegist.TabNo.Plan) {
                        //計画情報タブを選択状態にする
                        $(btn).click();
                        return false; //break
                    }
                });
            }
        } else if (!manufacturingFlg && !maintenanceFlg) {
            //ユーザの役割に「製造権限」「保全権限」が含まれない場合

            //全てのタブを非表示
            $(P_Article).find(".tab_btn.detail,.tab_contents").hide();
            //故障情報は非表示
            toggleHideGroup(FormRegist.FailureList.GroupNo, true);
        }

        //保全活動区分（非表示）の値取得
        var divisionId = getValue(FormRegist.HideInfo.Id, FormRegist.HideInfo.DivisionId, 1, CtrlFlag.Label);
        if (divisionId == DivisionIdDefine.Inspection) {
            //点検の場合

            //故障情報を非表示
            toggleHideGroup(FormRegist.FailureList.GroupNo, true);
            //故障分析情報タブ、故障分析情報(個別工場)タブをを非表示
            setHideTab(FormRegist.FailureTab.Id, formNo, true);
            setHideTab(FormRegist.FailureIndividualTab.List1.Id, formNo, true);
        } else if (divisionId == DivisionIdDefine.Failure) {
            //故障の場合

            if (maintenanceFlg) {
                //故障情報を表示
                toggleHideGroup(FormRegist.FailureList.GroupNo, false);
                //故障分析情報タブ、故障分析情報（個別工場）タブの表示切り替え
                changeDisplayFailureTab(formNo);
            }
        }

        //系停止が「なし」の場合、系停止時間を非活性にする
        var stopSystemEle = getCtrl(FormRegist.WorkInfo.Id, FormRegist.WorkInfo.StopSystem, 1, CtrlFlag.Combo);
        var selectEle = $(stopSystemEle).find("option:selected");
        if (selectEle.attr("exparam1") == StopSystemVal) {
            //非活性
            var stomTimeEle = getCtrl(FormRegist.WorkInfo.Id, FormRegist.WorkInfo.StopTime, 1, CtrlFlag.TextBox);
            changeInputControl(stomTimeEle, false);
        }

        //新規登録、修正またはフォロー計画の場合
        if (actionCtrlId == FormList.ButtonId.NewInspection || actionCtrlId == FormList.ButtonId.NewFailure || actionCtrlId == FormList.ButtonId.New
            || actionCtrlId == FormDetail.ButtonId.Edit || actionCtrlId == FormDetail.ButtonId.Follow) {
            //一括クリアボタン非表示
            setHideButton(FormRegist.ButtonId.AllClear, true);
        }

        //複写の場合
        if (actionCtrlId == FormDetail.ButtonId.Copy) {
            //項目にブランク設定

            //保全活動件名ID（非表示）
            setValue(FormRegist.HideInfo.Id, FormRegist.HideInfo.SummaryId, 1, CtrlFlag.Label, NewSummaryId, false);
            //件名情報　依頼Noにブランク設定
            setValue(FormRegist.HideInfo.Id, FormRegist.HideInfo.RequestNo, 1, CtrlFlag.Label, "", false);
            if (divisionId == DivisionIdDefine.Failure) {
                //故障の場合

                //故障情報　フォロー予定年月にブランク設定
                setValue(FormRegist.FailureList.List2.Id, FormRegist.FailureList.List2.FollowPlanDate, 1, CtrlFlag.Input, "", false);
                //故障情報　フォロー完了日にブランク設定
                setValue(FormRegist.FailureList.List2.Id, FormRegist.FailureList.List2.FollowCompletionDate, 1, CtrlFlag.Input, "", false);

                //故障分析情報　略図にブランク設定
                setValue(FormRegist.FailureTab.Id, FormRegist.FailureTab.FailureDiagram, 1, CtrlFlag.Label, "", false);
                //故障分析情報　故障原因分析書にブランク設定
                setValue(FormRegist.FailureTab.Id, FormRegist.FailureTab.FailureAnalyze, 1, CtrlFlag.Label, "", false);

                //故障分析情報(個別工場)　実施予定日にブランク設定
                setValue(FormRegist.FailureIndividualTab.List1.Id, FormRegist.FailureIndividualTab.List1.MeasurePlanDate, 1, CtrlFlag.Input, "", false);
                //故障分析情報(個別工場)　略図にブランク設定
                setValue(FormRegist.FailureIndividualTab.List2.Id, FormRegist.FailureIndividualTab.List2.FailureDiagram, 1, CtrlFlag.Label, "", false);
                //故障分析情報(個別工場)　故障原因分析書にブランク設定
                setValue(FormRegist.FailureIndividualTab.List2.Id, FormRegist.FailureIndividualTab.List2.FailureAnalyze, 1, CtrlFlag.Label, "", false);
            }

            //依頼情報　着手希望日にブランク設定
            setValue(FormRegist.RequestList.List2.Id, FormRegist.RequestList.List2.DesiredStartDate, 1, CtrlFlag.Input, "", false);
            //依頼情報　完了希望日にブランク設定
            setValue(FormRegist.RequestList.List2.Id, FormRegist.RequestList.List2.DesiredEndDate, 1, CtrlFlag.Input, "", false);

            //計画情報　着工予定日にブランク設定
            setValue(FormRegist.PlanList.Id, FormRegist.PlanList.ExpectedConstructionDate, 1, CtrlFlag.Input, "", false);
            //計画情報　完了予定日にブランク設定
            setValue(FormRegist.PlanList.Id, FormRegist.PlanList.ExpectedCompletionDate, 1, CtrlFlag.Input, "", false);

            //履歴情報　着工日にブランク設定
            setValue(FormRegist.HistoryList.Id, FormRegist.HistoryList.ConstructionDate, 1, CtrlFlag.Input, "", false);
            //履歴情報　完了日にブランク設定
            setValue(FormRegist.HistoryList.Id, FormRegist.HistoryList.CompletionDate, 1, CtrlFlag.Input, "", false);

            //履歴情報(個別工場)　発生時刻にブランク設定
            setValue(FormRegist.HistoryIndividualList.Id, FormRegist.HistoryIndividualList.OccurrenceTime, 1, CtrlFlag.Input, "", false);
            //履歴情報(個別工場)　完了日にブランク設定
            setValue(FormRegist.HistoryIndividualList.Id, FormRegist.HistoryIndividualList.CompletionDate, 1, CtrlFlag.Input, "", false);
            //履歴情報(個別工場)　完了時刻にブランク設定
            setValue(FormRegist.HistoryIndividualList.Id, FormRegist.HistoryIndividualList.CompletionTime, 1, CtrlFlag.Input, "", false);
        }

        if (maintenanceFlg) {
            //保全履歴タブ、保全履歴（個別工場）タブの表示切り替え
            changeDisplayHistoryTab(formNo);
        }

        //予算性格区分をラベル化
        var budgetPersonalityEle = getCtrl(FormRegist.WorkInfo.Id, FormRegist.WorkInfo.BudgetPersonality, 1, CtrlFlag.Combo);
        changeInputReadOnly(budgetPersonalityEle, false);

        //工場IDの取得（工場変更時に、個別工場タブの表示切り替えの判定に使用する）
        var factoryEle = getCtrl(FormRegist.LocationInfo.Id, FormRegist.LocationInfo.Factory, 1, CtrlFlag.Label);
        var factoryId = $(factoryEle).data('structureid');
        P_dicIndividual["factoryId"] = factoryId;

        //フォーカス設定
        setFocusDelay(FormRegist.SummaryInfo.Id, FormRegist.SummaryInfo.Subject, 1, CtrlFlag.TextBox);

        // 指定のキーでグローバル変数から値を削除
        delete P_dicIndividual[KeyNameFromScheduleLink];

    } else if (formNo == FormReplace.No) {
        // 機器交換画面

        // 機器交換情報を表示
        changeDisplayReplaceMachineInfo(false);
        // フォーカス設定(代替機器検索ボタン)
        setFocusButton(FormReplace.ButtonId.SearchMachine);

    } else if (formNo == FormSearch.No) {
        // 機器検索画面

        // 検索条件の使用区分を非表示
        var useSegment = $(getCtrl(FormSearch.SearchList.Id, FormSearch.SearchList.UseSegment, 1, CtrlFlag.Combo)).closest('tr');
        setHide(useSegment, true);
        // 検索条件のスケジュール管理を非表示
        var schedule = $(getCtrl(FormSearch.SearchList.Id, FormSearch.SearchList.Schedule, 1, CtrlFlag.Combo)).closest('tr');
        setHide(schedule, true);
        // 検索条件の保全部位を非表示
        var inspectionSite = $(getCtrl(FormSearch.SearchList.Id, FormSearch.SearchList.InspectionSite, 1, CtrlFlag.TextBox)).closest('tr');
        setHide(inspectionSite, true);
        // 検索条件の保全項目を非表示
        var inspectionContent = $(getCtrl(FormSearch.SearchList.Id, FormSearch.SearchList.InspectionContent, 1, CtrlFlag.TextBox)).closest('tr');
        setHide(inspectionContent, true);
        // 明細エリアを非表示
        changeDisplayDetailArea(true);
    } else if (formNo == FormSelectMachine.No) {
        // 機器選択

        // 検索条件の使用区分を非表示
        var useSegment = $(getCtrl(FormSelectMachine.DetailCondition.Id, FormSelectMachine.DetailCondition.UseSegment, 1, CtrlFlag.Combo)).closest('tr');
        setHide(useSegment, true);
        // 検索条件のスケジュール管理を非表示
        var schedule = $(getCtrl(FormSelectMachine.DetailCondition.Id, FormSelectMachine.DetailCondition.Schedule, 1, CtrlFlag.Combo)).closest('tr');
        setHide(schedule, true);

        //保全活動区分（非表示）の値取得
        var divisionId = getValueByOtherForm(FormRegist.No, FormRegist.HideInfo.Id, FormRegist.HideInfo.DivisionId, 1, CtrlFlag.Label);
        setValue(FormSelectMachine.HideInfo.Id, FormSelectMachine.HideInfo.DivisionId, 0, CtrlFlag.Label, divisionId);
        if (divisionId == DivisionIdDefine.Inspection) {
            //点検情報から起動された場合

            //非表示項目の管理基準に値設定
            setValue(FormSelectMachine.HideInfo.Id, FormSelectMachine.HideInfo.ManagementStandard, 0, CtrlFlag.Label, ManagementDefine.Management);
            //管理基準から保全内容を追加するボタンを非表示
            setHideButton(FormSelectMachine.ButtonId.ChangeManagement, true);
        } else if (divisionId == DivisionIdDefine.Failure) {
            //故障情報から起動された場合

            // 検索条件の保全部位を非表示
            var inspectionSite = $(getCtrl(FormSelectMachine.DetailCondition.Id, FormSelectMachine.DetailCondition.InspectionSite, 1, CtrlFlag.TextBox)).closest('tr');
            setHide(inspectionSite, true);
            // 検索条件の保全項目を非表示
            var inspectionContent = $(getCtrl(FormSelectMachine.DetailCondition.Id, FormSelectMachine.DetailCondition.InspectionContent, 1, CtrlFlag.TextBox)).closest('tr');
            setHide(inspectionContent, true);

            //非表示項目の管理基準に値設定
            setValue(FormSelectMachine.HideInfo.Id, FormSelectMachine.HideInfo.ManagementStandard, 0, CtrlFlag.Label, ManagementDefine.NotManagement);
            //管理基準から保全内容を追加するボタン、管理基準外の保全内容を追加するボタンを非表示
            setHideButton(FormSelectMachine.ButtonId.ChangeManagement, true);
            setHideButton(FormSelectMachine.ButtonId.ChangeNotManagement, true);
            //確定ボタンを非表示
            setHideButton(FormSelectMachine.ButtonId.Confirm, true);
        }
    }
}

/**
 * 保全履歴タブ、保全履歴（個別工場）タブの表示切り替え
 * @param {any} formNo 画面No
 */
function changeDisplayHistoryTab(formNo) {
    var form = formNo == FormDetail.No ? FormDetail : FormRegist;
    //保全履歴個別工場表示フラグの値取得
    var historyIndividualFlg = getValue(form.HideInfo.Id, form.HideInfo.HistoryIndividualFlg, 1, CtrlFlag.Label);
    if (historyIndividualFlg == IndividualFlg.Show) {
        //保全履歴タブ非表示
        setHideTab(form.HistoryList.Id, formNo, true);
        setHideTab(form.HistoryIndividualList.Id, formNo, false);
    } else {
        //保全履歴（個別工場）タブ非表示
        setHideTab(form.HistoryList.Id, formNo, false);
        setHideTab(form.HistoryIndividualList.Id, formNo, true);
    }
}

/**
 * 故障分析情報タブ、故障分析情報（個別工場）タブの表示切り替え
 * @param {any} formNo 画面No
 */
function changeDisplayFailureTab(formNo) {
    var form = formNo == FormDetail.No ? FormDetail : FormRegist;
    //故障分析個別工場表示対象フラグの値取得
    var failureIndividualFlg = getValue(form.HideInfo.Id, form.HideInfo.FailureIndividualFlg, 1, CtrlFlag.Label);
    if (failureIndividualFlg == IndividualFlg.Show) {
        //故障分析情報タブを非表示
        setHideTab(form.FailureTab.Id, formNo, true);
        setHideTab(form.FailureIndividualTab.List1.Id, formNo, false);
    } else {
        //故障分析情報(個別工場)タブを非表示
        setHideTab(form.FailureTab.Id, formNo, false);
        setHideTab(form.FailureIndividualTab.List1.Id, formNo, true);
    }
}

/**
 * 削除ボタンの表示切り替え
 */
function changeDisplayDeleteButton() {
    //保全権限フラグの値取得
    var maintenanceStr = getValue(FormDetail.HideInfo.Id, FormDetail.HideInfo.Maintenance, 1, CtrlFlag.Label);
    var maintenanceFlg = convertStrToBool(maintenanceStr);
    if (maintenanceFlg) {
        //削除ボタン表示
        setHideButton(FormDetail.ButtonId.Delete, false);
        return;
    }

    //完了日の値取得
    var completionDate = getValue(FormDetail.HideInfo.Id, FormDetail.HideInfo.CompletionDate, 1, CtrlFlag.Label);

    //保全履歴個別工場表示フラグの値取得
    var historyIndividualFlg = getValue(FormDetail.HideInfo.Id, FormDetail.HideInfo.HistoryIndividualFlg, 1, CtrlFlag.Label);
    var constructionPersonnel = null;
    if (historyIndividualFlg != IndividualFlg.Show) {
        //保全履歴情報タブの施工担当者の値取得
        constructionPersonnel = getValue(FormDetail.HistoryList.Id, FormDetail.HistoryList.ConstructionPersonnel, 1, CtrlFlag.TextBox);
    } else {
        //保全履歴情報（個別工場）タブの製造担当者の値取得
        constructionPersonnel = getValue(FormDetail.HistoryIndividualList.Id, FormDetail.HistoryIndividualList.ConstructionPersonnel, 1, CtrlFlag.TextBox);
    }

    if ((completionDate != null && completionDate != "") || (constructionPersonnel != null && constructionPersonnel != "")) {
        //保全権限がない場合かつ「進捗状況」が「保全受付」「完了済」の場合は、削除ボタンを非表示とする
        //「完了済」：完了日に値が入っている場合
        //「保全受付」：保全履歴情報タブの施工担当者または保全履歴情報（個別工場）タブの製造担当者に値が入っている場合
        setHideButton(FormDetail.ButtonId.Delete, true);
    } else {
        //削除ボタン表示
        setHideButton(FormDetail.ButtonId.Delete, false);
    }
}

/**
 * 機器交換情報の表示切り替え
 * @param flg true(非表示)、false(表示)
 */
function changeDisplayReplaceMachineInfo(flg) {
    if (flg == true) {
        // 機器交換情報を非表示
        toggleHideGroup(FormReplace.ReplaceList.GroupNo, true);
        toggleHideGroup(FormReplace.ConfirmList.GroupNo, false);
    } else {
        // 機器交換情報を表示
        toggleHideGroup(FormReplace.ConfirmList.GroupNo, true);
        toggleHideGroup(FormReplace.ReplaceList.GroupNo, false);
    }
}

/**
 * 明細ｴﾘｱの表示切り替え
 * @param flg true(非表示)、false(表示)
 */
function changeDisplayDetailArea(flg) {
    // 明細ｴﾘｱの非表示
    var detailArea = $(P_Article).find("#" + P_formDetailId);
    var detailLists = $(detailArea).find(".detail_div").children().not("[id$='edit_div']"); //明細ｴﾘｱ一覧
    var detailCtrlGroup = $(detailArea).find(".action_detail_div"); //明細ｴﾘｱｺﾝﾄﾛｰﾙｸﾞﾙｰﾌﾟ
    // 明細ｴﾘｱを表示
    setInitHide(detailLists, flg);        //一覧表示
    setInitHide(detailCtrlGroup, flg);    //ｺﾝﾄﾛｰﾙｸﾞﾙｰﾌﾟ表示
}

/**
 *【オーバーライド用関数】
 *  検索処理前
 *
 *  @appPath {string} 　：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @btn {string} 　　　：対象ボタン
 *  @conductId {string} ：機能ID
 *  @pgmId {string} 　　：プログラムID
 *  @formNo {number} 　 ：画面番号
 *  @conductPtn {number}：処理ﾊﾟﾀｰﾝ
 */
function beforeSearchBtnProcess(appPath, btn, conductIdW, pgmIdW, formNoW, conductPtnW) {

    // 機能IDが「帳票出力」の場合
    if (conductIdW == RM00001_ConductId) {
        return RM0001_beforeSearchBtnProcess(appPath, btn, conductIdW, pgmIdW, formNoW, conductPtnW);
    }

}

/**
 * 【オーバーライド用関数】画面初期値データ取得前処理(表示中画面用)
 * @param {any} appPath ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 * @param {any} formNo 画面番号
 * @param {any} btnCtrlId ｱｸｼｮﾝﾎﾞﾀﾝのCTRLID
 * @param {any} conditionDataList 条件一覧要素
 * @param {any} listDefines 一覧の定義情報
 * @param {any} pgmId 遷移先のプログラムID
 */
function prevInitFormData(appPath, formNo, btnCtrlId, conditionDataList, listDefines, pgmId) {

    if (formNo != FormSearch.No && formNo != FormSelectMachine.No) {
        // 機器検索、機器選択画面以外の場合は終了
        return [conditionDataList, listDefines];
    }
    // 機器検索、機器選択画面
    if ($(conditionDataList).length) {
        $.each($(conditionDataList), function (idx, conditionData) {
            // 場所階層ツリーラベルまたは職種機種ツリーラベルの一覧の場合
            if ((conditionData.FORMNO == FormSearch.No || conditionData.FORMNO == FormSelectMachine.No)
                && ((conditionData.CTRLID == FormSearch.StructureCondition.Id || conditionData.CTRLID == FormSearch.JobCondition.Id)
                    || (conditionData.CTRLID == FormSelectMachine.StructureCondition.Id || conditionData.CTRLID == FormSelectMachine.JobCondition.Id))) {
                // 一覧の定義情報に格納
                listDefines.push(conditionData);
            }
        });
    }

    return [conditionDataList, listDefines];
}

/**
 * 【オーバーライド用関数】
 *   コンボボックス変更時イベント
 * @param {string} appPath  : アプリケーションルートパス
 * @param {要素} combo      : 変更イベントが発生したコンボボックスの要素
 * @param {List} datas      : 変更イベントが発生したコンボボックスのデータリスト
 * @param {data} selected   : 変更されたコンボボックスの選択されたデータ
 * @param {byte} formNo     : イベントの発生した画面番号
 * @param {string} ctrlId   : イベントの発生した画面のコントロールID
 * @param {byte} valNo      : イベントの発生したコンボボックスのコントロール番号
 */
function setComboOtherValues(appPath, combo, datas, selected, formNo, ctrlId, valNo) {

    // 機能IDが「帳票出力」の場合
    if (getConductId() == RM00001_ConductId) {
        return RM0001_setComboOtherValues(appPath, combo, datas, selected, formNo, ctrlId, valNo);
    }

    // 共通-文書管理詳細画面のコンボボックス変更処理
    DM0002_setComboOtherValues(appPath, combo, datas, selected, formNo, ctrlId, valNo);

    // MQ分類コンボボックス変更時
    if (formNo == FormRegist.No && ctrlId == FormRegist.WorkInfo.Id && valNo == FormRegist.WorkInfo.MqClass) {
        // 選択された要素で、設定したい拡張項目の値を取得(空白選択時は空白)
        var setValue = selected == null ? '' : selected.EXPARAM2;
        // 予算性格区分に値を設定
        setValueAndTriggerNoChange(FormRegist.WorkInfo.Id, FormRegist.WorkInfo.BudgetPersonality, 0, CtrlFlag.Combo, setValue);

        //予算性格区分要素
        var budgetPersonalityEle = getCtrl(FormRegist.WorkInfo.Id, FormRegist.WorkInfo.BudgetPersonality, 1, CtrlFlag.Combo);
        var label = selected == null ? '' : selected.EXPARAM3;
        //ﾗﾍﾞﾙ表示用spanﾀｸﾞに表示文字列ｾｯﾄ
        setLabelingSpan($(budgetPersonalityEle).closest("td"), label);
    }

    // 系停止コンボボックス変更時
    if (formNo == FormRegist.No && ctrlId == FormRegist.WorkInfo.Id && valNo == FormRegist.WorkInfo.StopSystem) {
        // 選択された要素で、設定したい拡張項目の値を取得(空白選択時は空白)
        var setValue = selected == null ? '' : (selected.EXPARAM1 == StopSystemVal ? selected.EXPARAM1 : '');
        if (setValue != "") {
            //「なし」が選択された場合

            // 系停止時間に0を設定
            setValueAndTriggerNoChange(FormRegist.WorkInfo.Id, FormRegist.WorkInfo.StopTime, 0, CtrlFlag.TextBox, setValue);
            //非活性
            var stomTimeEle = getCtrl(FormRegist.WorkInfo.Id, FormRegist.WorkInfo.StopTime, 1, CtrlFlag.TextBox);
            changeInputControl(stomTimeEle, false);
        } else {
            //「なし」以外が選択された場合

            //系停止時間を活性
            var stomTimeEle = getCtrl(FormRegist.WorkInfo.Id, FormRegist.WorkInfo.StopTime, 1, CtrlFlag.TextBox);
            changeInputControl(stomTimeEle, true);
        }
    }
}

/**
 *【オーバーライド用関数】
 *  遷移処理の前
 *
 *  @param {string}     appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {number}     transPtn    ：画面遷移ﾊﾟﾀｰﾝ(1：子画面、2：単票入力、4：共通機能、5：他機能別ﾀﾌﾞ、6：他機能表示切替)
 *  @param {number}     transDiv    ：画面遷移アクション区分(1：新規、2：修正、3：複製、4：出力、)
 *  @param {string}     transTarget ：遷移先(子画面NO / 単票一覧ctrlId / 共通機能ID / 他機能ID|画面NO)
 *  @param {number}     dispPtn     ：遷移表示ﾊﾟﾀｰﾝ(1：表示切替、2：ﾎﾟｯﾌﾟｱｯﾌﾟ、3：一覧直下)
 *  @param {number}     formNo      ：遷移元formNo
 *  @param {string}     ctrlId      ：遷移元の一覧ctrlid
 *  @param {string}     btn_ctrlId  ：ボタンのbtn_ctrlid
 *  @param {number}     rowNo       ：遷移元の一覧の選択行番号（新規の場合は-1）
 *  @param {Element}    element     ：ｲﾍﾞﾝﾄ発生要素
 *  @return {bool} 遷移を行わない場合はfalse、行う場合はtrue
 *  @return {Array.<Dictionary<string, object>>} 個別取得の遷移条件データ
 */
function prevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element) {
    // 共通-文書管理詳細画面の遷移前処理を行うかどうか判定し、Trueの場合は行う
    if (IsExecDM0002_PrevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element)) {
        return DM0002_prevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element);
    }

    var conditionDataList = [];

    // 一覧フィルタ処理実施
    if (executeListFilter(transTarget, FormList.List.Id, FormList.Filter.Id, FormList.Filter.Input)) {
        return [false, conditionDataList];
    }

    // 一覧画面で出力ボタンまたは工程表の表示ボタン押下時
    if (formNo == FormList.No && (btn_ctrlId == FormList.ButtonId.Output || btn_ctrlId == FormList.ButtonId.ProcessReport)) {
        // 一覧にチェックされた行が存在しない場合、遷移をキャンセル
        if (!isCheckedList(FormList.List.Id)) {
            return [false, conditionDataList];
        }
    }

    //詳細画面で件名添付ボタン、修正ボタン、機器交換ボタン、略図添付ボタン、故障原因分析書添付ボタン押下時
    if (formNo == FormDetail.No && (btn_ctrlId == FormDetail.ButtonId.AddSubject || btn_ctrlId == FormDetail.ButtonId.Edit || btn_ctrlId == FormDetail.ButtonId.EquipReplace || btn_ctrlId == FormDetail.ButtonId.AddFailureDiagram || btn_ctrlId == FormDetail.ButtonId.AddFailureAnalyze)) {

        //完了日の値取得
        var completionDate = getValue(FormDetail.HideInfo.Id, FormDetail.HideInfo.CompletionDate, 1, CtrlFlag.Label);
        var key = "MA0001_isSkipConfirm_" + btn_ctrlId;
        // 確認ウインドウの表示判定に用いる値
        var isSkipConfirm = P_dicIndividual[key];
        delete P_dicIndividual[key];
        // isSkipConfirmがこの値ならスキップする
        const isSkipConfirmValue = 1;
        //件名が完了している場合（完了日の値が取得できた場合）、警告メッセージを表示
        if (completionDate != null && completionDate != "" && isSkipConfirm != isSkipConfirmValue) {
            var strMessage = P_ComMsgTranslated[141300001]; // 「保全履歴が完了していますが、変更してもよろしいですか？」
            // 確認ウインドウでOKを押した場合の処理
            var eventFunc = function () {
                // 確認ウインドウの表示判定に用いる値で、次回は確認表示をスキップするよう設定
                P_dicIndividual[key] = isSkipConfirmValue;
                // この遷移処理はキャンセルされるので、もう一度遷移処理を呼び出す。今度は確認メッセージは出ない。
                transForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element);
                // 処理終了
                return;
            }
            // 警告メッセージを表示
            if (!popupMessage([strMessage], messageType.Confirm, eventFunc)) {
                //『キャンセル』の場合、処理中断
                return [false, conditionDataList];
            }
        }

        //件名添付ボタン押下時
        if (btn_ctrlId == FormDetail.ButtonId.AddSubject) {
            //パラメータ設定
            conditionDataList.push(getParamToDM0002(AttachmentStructureGroupID.Summary, getValue(FormDetail.HideInfo.Id, FormDetail.HideInfo.SummaryId, 1, CtrlFlag.Label)));
        }

        //機器交換ボタン押下時
        if (btn_ctrlId == FormDetail.ButtonId.EquipReplace) {
            // 対象機器一覧にてチェックされた行が存在しない場合、遷移をキャンセル
            if (!isCheckedList(FormDetail.MachineList.Id, getMessageParam(P_ComMsgTranslated[141060001], [P_ComMsgTranslated[111160007]]))) {
                return [false, conditionDataList];
            }
            // 対象機器一覧にて複数件チェックされいる場合、遷移をキャンセル
            if (!isMultipleCheckedList(FormDetail.MachineList.Id, getMessageParam(P_ComMsgTranslated[141060002], [P_ComMsgTranslated[111160007]]))) {
                return [false, conditionDataList];
            }
            // 対象の一覧のコントロールグループIDをctrlIdListへ設定する
            const ctrlIdList = [FormDetail.MachineList.Id, FormDetail.FailureList.Id];
            conditionDataList = getListDataByCtrlIdList(ctrlIdList, formNo, 0, false, true);
        }

        //略図添付ボタン、故障原因分析書添付ボタン押下時
        if (btn_ctrlId == FormDetail.ButtonId.AddFailureDiagram || btn_ctrlId == FormDetail.ButtonId.AddFailureAnalyze) {
            var form = formNo == FormDetail.No ? FormDetail : FormRegist;
            //故障分析個別工場表示対象フラグの値取得
            var failureIndividualFlg = getValue(form.HideInfo.Id, form.HideInfo.FailureIndividualFlg, 1, CtrlFlag.Label);
            //タブID
            var failureTabId = failureIndividualFlg == IndividualFlg.Hide ? form.FailureTab.Id : form.FailureIndividualTab.List2.Id;
            //保全履歴故障情報ID列
            var historyFailureId = failureIndividualFlg == IndividualFlg.Hide ? form.FailureTab.HistoryFailureId : form.FailureIndividualTab.List2.HistoryFailureId;
            //略図添付ボタン押下時
            if (btn_ctrlId == FormDetail.ButtonId.AddFailureDiagram) {
                //略図添付の機能タイプID
                var typeId = failureIndividualFlg == IndividualFlg.Hide ? AttachmentStructureGroupID.HistoryFailureDiagram : AttachmentStructureGroupID.HistoryFailureFactDiagram;
                //パラメータ設定
                conditionDataList.push(getParamToDM0002(typeId, getValue(failureTabId, historyFailureId, 1, CtrlFlag.Label)));
            }
            //故障原因分析書添付ボタン押下時
            if (btn_ctrlId == FormDetail.ButtonId.AddFailureAnalyze) {
                //故障原因分析書添付の機能タイプID
                var typeId = failureIndividualFlg == IndividualFlg.Hide ? AttachmentStructureGroupID.HistoryFailureAnalyze : AttachmentStructureGroupID.HistoryFailureFactAnalyze;
                //パラメータ設定
                conditionDataList.push(getParamToDM0002(typeId, getValue(failureTabId, historyFailureId, 1, CtrlFlag.Label)));
            }
        }
    }

    //フォロー計画押下時
    if (formNo == FormDetail.No && btn_ctrlId == FormDetail.ButtonId.Follow) {
        //現在表示している保全活動件名IDをフォロー計画キーIDに設定
        //保全活動件名IDの値取得
        var summaryId = getValue(FormDetail.HideInfo.Id, FormDetail.HideInfo.SummaryId, 1, CtrlFlag.Label);
        //フォロー計画キーIDに保全活動件名ID設定
        setValue(FormDetail.HideInfo.Id, FormDetail.HideInfo.FollowPlanKeyId, 1, CtrlFlag.Label, summaryId, false);

    }

    // 参照画面から編集画面へ遷移の場合
    if (formNo == FormDetail.No && (transDiv == transDivDef.Edit || transDiv == transDivDef.Copy)) {
        // 対象の一覧のコントロールグループIDをctrlIdListへ設定する
        const ctrlIdList = [FormDetail.HideInfo.Id];
        conditionDataList = getListDataByCtrlIdList(ctrlIdList, formNo, 0);
    }

    // 編集画面から機器選択画面へ遷移の場合
    if (formNo == FormRegist.No && transTarget == FormSelectMachine.No && ctrlId == FormRegist.MachineList.Id) {
        // 対象の一覧のコントロールグループIDをctrlIdListへ設定する
        const ctrlIdList = [FormRegist.LocationInfo.Id];
        conditionDataList = getListDataByCtrlIdList(ctrlIdList, formNo, 0);
    }

    // 機器交換画面から機器検索画面へ遷移の場合
    if (formNo == FormReplace.No && transTarget == FormSearch.No && btn_ctrlId == FormSearch.ButtonId.SearchMachine) {
        // 対象の一覧のコントロールグループIDをctrlIdListへ設定する
        const ctrlIdList = [FormDetail.LocationInfo.Id];
        conditionDataList = getListDataByCtrlIdList(ctrlIdList, FormDetail.No, 0);
    }

    return [true, conditionDataList];
}

/**
 *【オーバーライド用関数】
 *  遷移処理の後
 *
 *  @param {string}     appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {number}     transPtn    ：画面遷移ﾊﾟﾀｰﾝ(1：子画面、2：単票入力、4：共通機能、5：他機能別ﾀﾌﾞ、6：他機能表示切替)
 *  @param {number}     transDiv    ：画面遷移アクション区分(1：新規、2：修正、3：複製、4：出力、)
 *  @param {string}     transTarget ：遷移先(子画面NO / 単票一覧ctrlId / 共通機能ID / 他機能ID|画面NO)
 *  @param {number}     dispPtn     ：遷移表示ﾊﾟﾀｰﾝ(1：表示切替、2：ﾎﾟｯﾌﾟｱｯﾌﾟ、3：一覧直下)
 *  @param {number}     formNo      ：遷移元formNo
 *  @param {string}     ctrlId      ：遷移元の一覧ctrlid
 *  @param {string}     btn_ctrlId  ：ボタンのbtn_ctrlid
 *  @param {number}     rowNo       ：遷移元の一覧の選択行番号（新規の場合は-1）
 *  @param {Element}    element     ：ｲﾍﾞﾝﾄ発生要素
 *  @return {bool} 遷移を行わない場合はfalse、行う場合はtrue
 *  @return {Array.<Dictionary<string, object>>} 個別取得の遷移条件データ
 */
function postTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element) {

    // 共通-文書管理詳細画面の遷移後処理
    DM0002_postTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element);
}

/**
 *【オーバーライド用関数】
 *  戻る処理の前(単票、子画面共用)
 *
 *  @appPath        {string}    ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @btnCtrlId      {byte}      ：ボタンのCtrlId
 *  @codeTransFlg   {int}       ：1:コード＋翻訳 選ボタンから画面遷移/1以外:それ以外
 */
function prevBackBtnProcess(appPath, btnCtrlId, status, codeTransFlg) {

    if (getFormNo() == FormRegist.No && btnCtrlId == FormRegist.ButtonId.Regist) {
        //新規登録画面から登録後、詳細画面に渡すキー情報をセット
        const conditionDataList = getListDataByCtrlIdList([FormRegist.HideInfo.Id], FormRegist.No, 0);
        setSearchCondition(ConductId_MA0001, FormDetail.No, conditionDataList);
    } else if (getFormNo() == FormReplace.No) {
        // 機器交換画面
        if (btnCtrlId == FormReplace.ButtonId.Regist) {
            // 登録ボタン押下後、詳細画面に渡すキー情報をセット
            const conditionDataList = getListDataByCtrlIdList([FormDetail.HideInfo.Id], FormDetail.No, 0);
            setSearchCondition(ConductId_MA0001, FormDetail.No, conditionDataList);
        }
        // グローバルリストから対象のキーを削除する
        delete P_dicIndividual[MA0001_selectMachineId];
    } else if (getFormNo() == FormSearch.No) {
        // 機器検索画面
        if (btnCtrlId == FormSearch.ButtonId.Cancel) {
            // 閉じるボタン押下後、機器交換画面に渡すキー情報をセット
            const ctrlIdList = [FormDetail.MachineList.Id, FormDetail.FailureList.Id];
            const conditionDataList = getListDataByCtrlIdList(ctrlIdList, FormDetail.No, 0);
            setSearchCondition(ConductId_MA0001, FormReplace.No, conditionDataList);
        }
    } else if (getFormNo() == FormSelectMachine.No) {
        //登録画面へ戻る際、再描画は行わない
        return false;
    }
    return true;
}

/**
 *【オーバーライド用関数】
 *  行削除の前
 *
 *  @appPath    {string}    ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @btn        {<a>}       ：行削除ﾘﾝｸのa要素
 *  @id         {string}    ：行削除ﾘﾝｸの対象一覧のCTRLID
 *  @checkes    {要素}      ：削除対象の要素リスト
 */
function preDeleteRow(appPath, btn, id, checkes) {
    // 共通-文書管理詳細画面の行削除前処理
    var comRtn = DM0002_preDeleteRow(appPath, btn, id, checkes);
    if (!comRtn) {
        // 処理終了の場合は終了
        return false;
    }

    // 対象機器一覧の行削除時
    if (id == FormRegist.MachineList.Id + getAddFormNo()) {
        $.each(checkes, function (idx, check) {

            // 削除された行の行ステータスを変更
            check["ROWSTATUS"] = rowStatusDef.Delete;

        });
    }

    return true;
}

/**
 * 【オーバーライド用関数】実行正常終了後処理
 *  @param appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param conductId   ：機能ID
 *  @param pgmId       ：プログラムID
 *  @param formNo      ：画面NO
 *  @param btn         ：ボタン要素
 *  @param conductPtn  ：機能処理ﾊﾟﾀｰﾝ
 *  @param autoBackFlg ：ajax正常終了後、自動戻るフラグ　false:戻らない、true:自動で戻る
 *  @param isEdit      ：単票表示フラグ
 *  @param data        ：結果ﾃﾞｰﾀ
 */
function postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data) {

    // 機能IDが「帳票出力」の場合
    if (getConductId() == RM00001_ConductId) {
        return RM0001_postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data);
    }

    // 共通-文書管理詳細画面の実行正常終了後処理
    DM0002_postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data);
}

/**
 *【オーバーライド用関数】Excel出力ﾁｪｯｸ処理 - 前処理
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 */
function reportCheckPre(appPath, conductId, formNo, btn) {

    // 機能IDが「帳票出力」の場合
    if (getConductId() == RM00001_ConductId) {
        return RM0001_reportCheckPre(appPath, conductId, formNo, btn)
    }

    // 一覧画面で建材日報出力ボタン押下時
    var btnName = $(btn).attr("name");
    if (formNo == FormList.No && btnName == FormList.ButtonId.DailyReportOutput) {
        // 一覧にチェックされた行が存在しない場合、エラーメッセージ表示
        if (!isCheckedList(FormList.List.Id)) {
            return false;
        }
    }
    return true;
}

/**
 *【オーバーライド用関数】登録前追加条件取得処理
 *  @param appPath       ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param conductId     ：機能ID
 *  @param formNo        ：画面番号
 *  @param btn           ：押下されたボタン要素
 */
function addSearchConditionDictionaryForRegist(appPath, conductId, formNo, btn) {
    // 共通-文書管理詳細画面の登録前追加条件取得処理を行うかどうか判定し、Trueの場合は行う
    if (IsExecDM0002_AddSearchConditionDictionaryForRegist(appPath, conductId, formNo, btn)) {
        return DM0002_addSearchConditionDictionaryForRegist(appPath, conductId, formNo, btn);
    }

    // それ以外の場合
    var conditionDataList = [];
    return conditionDataList;
}

/**
 *【オーバーライド用関数】取込処理個別入力チェック
 *  @param appPath       ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param conductId     ：機能ID
 *  @param formNo        ：画面番号
 */
function preInputCheckUpload(appPath, conductId, formNo) {
    if (getConductId() == RM00001_ConductId) {
        // 共通-帳票出力画面 入力チェック
        return RM0001_preInputCheckUpload(appPath, conductId, formNo);
    }

    // 取込処理は添付情報のみのため他機能の考慮不要
    return DM0002_preInputCheckUpload(appPath, conductId, formNo);
}

/**
 *【オーバーライド用関数】バリデーション前処理
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 */
function validDataPre(appPath, conductId, formNo, btn) {
    if (formNo == FormRegist.No) {
        //入力チェック

        //製造権限フラグの値取得
        var manufacturingStr = getValue(FormRegist.HideInfo.Id, FormRegist.HideInfo.Manufacturing, 1, CtrlFlag.Label);
        var manufacturingFlg = convertStrToBool(manufacturingStr);
        //保全権限フラグの値取得
        var maintenanceStr = getValue(FormRegist.HideInfo.Id, FormRegist.HideInfo.Maintenance, 1, CtrlFlag.Label);
        var maintenanceFlg = convertStrToBool(maintenanceStr);

        //保全活動区分（非表示）の値取得
        var divisionId = getValue(FormRegist.HideInfo.Id, FormRegist.HideInfo.DivisionId, 1, CtrlFlag.Label);

        //MQ分類が「設備工事」「撤去工事」以外の場合、突発区分を必須とする
        //「設備工事」「撤去工事」の構成ID取得
        var mqStructureIds = getValue(FormRegist.HideInfo.Id, FormRegist.HideInfo.MqNotRequiredStructureId, 1, CtrlFlag.Label);
        //カンマ区切りで分割
        var mqStructureIdList = mqStructureIds.split(',');
        //MQ分類の値取得
        var mqClass = getCtrl(FormRegist.WorkInfo.Id, FormRegist.WorkInfo.MqClass, 1, CtrlFlag.Combo);
        var mqClassVal = $(mqClass).val();
        //突発区分の要素取得
        var suddenDivision = getCtrl(FormRegist.WorkInfo.Id, FormRegist.WorkInfo.SuddenDivision, 1, CtrlFlag.Combo);
        //必須を外す
        $(suddenDivision).rules("remove", "required");
        if (mqClassVal != "" && $.inArray(mqClassVal, mqStructureIdList) == -1) {
            //「設備工事」「撤去工事」以外の場合、突発区分に必須設定
            setRequiredItem(suddenDivision);
        }

        ////製造権限がある場合
        //if (manufacturingFlg) {
        //    //保全依頼情報タブのいずれかの項目に入力がある場合、依頼内容を必須にする
        //    tabs = $(P_Article).find(".tab_contents[data-tabno='" + FormRegist.TabNo.Request + "']");
        //    //依頼内容
        //    var requestContent = getCtrl(FormRegist.RequestList.List1.Id, FormRegist.RequestList.List1.RequestContent, 1, CtrlFlag.TextBox);
        //    setRequired(requestContent, tabs);
        //}

        //保全権限がある場合
        if (maintenanceFlg) {
            ////保全計画情報タブ、保全履歴情報タブ、保全履歴情報（個別工場）タブ、故障情報、故障分析情報タブ、故障分析情報（個別工場）タブのいずれかの項目に入力がある場合、作業計画・実施内容を必須にする
            //var tabs = $(P_Article).find(".tab_contents[data-tabno='" + FormRegist.TabNo.Plan + "'],[data-tabno='" + FormRegist.TabNo.History
            //    + "']:not(.hide),[data-tabno='" + FormRegist.TabNo.HistoryIndividual + "']:not(.hide),[data-tabno='" + FormRegist.TabNo.Failure + "']:not(.hide),[data-tabno='" + FormRegist.TabNo.FailureIndividual
            //    + "']:not(.hide)," + getGroupId(FormRegist.FailureList.GroupNo) + ":not(.hide)");
            ////作業計画・実施内容
            //var content = getCtrl(FormRegist.SummaryInfo.Id, FormRegist.SummaryInfo.Content, 1, CtrlFlag.Textarea);
            //setRequired(content, tabs);

            //保全履歴情報タブまたは保全履歴情報（個別工場）タブの完了日に入力がある場合、MQ分類を必須にする
            //履歴情報　完了日
            var completionDate = getValue(FormRegist.HistoryList.Id, FormRegist.HistoryList.CompletionDate, 1, CtrlFlag.TextBox);
            //履歴情報(個別工場)　完了日
            var individualCompletionDate = getValue(FormRegist.HistoryIndividualList.Id, FormRegist.HistoryIndividualList.CompletionDate, 1, CtrlFlag.TextBox);
            //MQ分類
            //必須を外す
            $(mqClass).rules("remove", "required");
            if (completionDate != "" || individualCompletionDate != "") {
                //MQ分類に必須設定
                setRequiredItem(mqClass);
            }

            //if (divisionId == DivisionIdDefine.Failure) {
            //    //故障情報に対する入力が行われている場合、保全部位、保全内容を必須とする
            //    //対象機器
            //    var targetTable = P_listData["#" + FormRegist.MachineList.Id + getAddFormNo()];
            //    //保全部位
            //    var inspectionSite = getCtrl(FormRegist.FailureList.List1.Id, FormRegist.FailureList.List1.InspectionSite, 1, CtrlFlag.TextBox);
            //    //保全内容
            //    var inspectionContent = getCtrl(FormRegist.FailureList.List1.Id, FormRegist.FailureList.List1.InspectionContent, 1, CtrlFlag.TextBox);
            //    if (targetTable.getDataCount() > 0) {
            //        //保全部位、保全内容を必須
            //        setRequiredItem(inspectionSite);
            //        setRequiredItem(inspectionContent);
            //    } else {
            //        //必須を外す
            //        $(inspectionSite).rules("remove", "required");
            //        $(inspectionContent).rules("remove", "required");
            //    }
            //}

            ////保全計画情報タブのいずれかの項目に入力がある場合、実施件名、着工予定日、完了予定日を必須にする
            //tabs = $(P_Article).find(".tab_contents[data-tabno='" + FormRegist.TabNo.Plan + "']");
            ////実施件名
            //var subject = getCtrl(FormRegist.PlanList.Id, FormRegist.PlanList.Subject, 1, CtrlFlag.TextBox);
            //setRequired(subject, tabs);
            ////着工予定日
            //var subject = getCtrl(FormRegist.PlanList.Id, FormRegist.PlanList.ExpectedConstructionDate, 1, CtrlFlag.Input);
            //setRequired(subject, tabs);
            ////完了予定日
            //var subject = getCtrl(FormRegist.PlanList.Id, FormRegist.PlanList.ExpectedCompletionDate, 1, CtrlFlag.Input);
            //setRequired(subject, tabs);
        }

    }
}

/**
 * いずれかの項目に入力がある場合、指定した項目に必須を設定
 * @param {any} ele 必須を設定する項目要素
 * @param {any} tabs 入力有無をチェックする範囲
 */
function setRequired(ele, tabs) {
    //コントロールを含むtd要素を取得（非表示項目は除く）
    var tds = $(tabs).find("td[data-name^='VAL']:not(.hide)");
    //必須を外す
    $(ele).rules("remove", "required");
    $.each(tds, function (idx, td) {
        var val = getCellVal(td);
        if (val != "" && val != "0") {
            //値が設定されていた場合、指定した項目を必須にする
            setRequiredItem(ele);
            //ループ終了
            return false;
        }
    });
}

/**
 * 項目に必須を設定する
 * @param {any} ele 必須を設定する項目要素
 */
function setRequiredItem(ele) {
    //必須設定
    var message = { required: P_ComMsgTranslated[941220009] }; //入力して下さい。
    var rule = {};
    rule['required'] = true;
    rule['messages'] = message;
    $(ele).rules('add', rule);
}

/**
 * 【オーバーライド用関数】実行ボタン前処理
 *  @param {string} appPath ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string} transDiv：画面遷移アクション区分
 *  @param {string} conductId：機能ID
 *  @param {string} pgmId ：プログラムID
 *  @param {number} formNo ：画面NO
 *  @param {html} btn  ：ボタン要素
 *  @param {number} conductPtn  ：機能処理ﾊﾟﾀｰﾝ
 *  @param {boolean} autoBackFlg ：ajax正常終了後、自動戻るフラグ　false:戻らない、true:自動で戻る
 *  @param {boolean} isEdit ：単票表示フラグ
 *  @param {number} confirmNo ：確認番号
 *  @return {bool} 処理続行フラグ Trueなら続行、Falseなら処理終了
 */
function preRegistProcess(appPath, transDiv, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, confirmNo) {
    if (formNo == FormDetail.No) {
        //削除ボタン

        // 表示する確認メッセージ(デフォルト)を取得(P_MessageStr)に格納
        setMessageStrForBtn(btn, confirmKbnDef.Disp);
        var errorMsg = P_MessageStr;
        //完了日の値取得
        var completionDate = getValue(FormDetail.HideInfo.Id, FormDetail.HideInfo.CompletionDate, 1, CtrlFlag.Label);
        if (completionDate != null && completionDate != "") {
            // 特定の値の場合、メッセージを変更
            errorMsg = P_ComMsgTranslated[141300002];//保全履歴が完了していますが、削除してもよろしいですか？
        }

        var key = "MA0001_isSkipConfirm_" + FormDetail.ButtonId.Delete;
        // 確認ウインドウの表示判定に用いる値
        var isSkipConfirm = P_dicIndividual[key];
        delete P_dicIndividual[key];
        // isSkipConfirmがこの値ならスキップする
        const isSkipConfirmValue = 1;
        if (isSkipConfirm != isSkipConfirmValue) {
            // 確認ウインドウでOKを押した場合の処理
            var eventFunc = function () {
                // 確認ウインドウの表示判定に用いる値で、次回は確認表示をスキップするよう設定
                P_dicIndividual[key] = isSkipConfirmValue;
                // この削除処理はキャンセルされるので、もう一度削除処理を呼び出す。今度は確認メッセージは出ない。
                clickRegistBtnConfirmOK(appPath, transDiv, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, confirmNo);
                // 処理終了
                return;
            }
            // 確認ﾒｯｾｰｼﾞを表示
            if (!popupMessage([errorMsg], messageType.Confirm, eventFunc)) {
                //『キャンセル』の場合、処理中断
                // ボタンを活性化
                $(btn).prop("disabled", false);
                return false;
            }
        }
    }

    return true;
}

/**
 *【オーバーライド用関数】
 *  個別実装ボタン
 *
 *  @param {string} appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {number} formNo      ：画面NO
 *  @param {string} btnCtrlId   ：ボタンCTRLID
 */
function clickIndividualImplBtn(appPath, formNo, btnCtrlId) {

    // 機能IDが「帳票出力」の場合
    if (getConductId() == RM00001_ConductId) {
        return RM0001_clickIndividualImplBtn(appPath, formNo, btnCtrlId);
    }

    //タブ内一括クリア
    if (btnCtrlId == FormRegist.ButtonId.AllClear) {
        //ボタン要素
        var btn = getButtonCtrl(btnCtrlId);
        //タブ要素
        var tab = $(btn).closest(".tab_contents.selected");
        //タブ内のテーブル一覧
        var tables = $(tab).find("div.vertical_tbl");

        //処理継続用ｺｰﾙﾊﾞｯｸ関数呼出文字列を設定
        var eventFunc = function () {
            $.each(tables, function (idx, table) {
                var id = $(table).attr("id");
                // 検索結果をクリア
                clearSearchResult(id);
            });
            //明細エリアのエラー状態を初期化
            clearMessage();
            // 処理終了
            return;
        }

        var strMessage = P_ComMsgTranslated[141220001]; // 「入力内容をクリアします。よろしいですか？」
        // メッセージ表示
        if (!popupMessage([strMessage], messageType.Confirm, eventFunc)) {
            //『キャンセル』の場合、処理中断
            return false;
        }
    }

    // 機器交換画面の確認ボタン押下時
    if (formNo == FormReplace.No && btnCtrlId == FormReplace.ButtonId.Confirm) {
        var strMessage;
        // 現場機器の機番ID取得
        var currentMachineId = getValueByOtherForm(formNo, FormReplace.ReplaceList.CurrentInfo.Id, FormReplace.ReplaceList.CurrentInfo.MachineId, 0, CtrlFlag.Label);
        // 代替機器情報取得
        var targetCtrl = getCtrl(FormReplace.ReplaceList.ReplaceInfo.Id, FormReplace.ReplaceList.ReplaceInfo.MachineId, 0, CtrlFlag.Label);
        // 代替機器情報が取得できない場合、エラーメッセージを表示
        if (!targetCtrl) {
            // 「代替機器が選択されていません。」
            strMessage = getMessageParam(P_ComMsgTranslated[141060001], [P_ComMsgTranslated[111160010]]);
            setMessage(strMessage, procStatus.Error);
            return false;
        }
        // 代替機器の機番ID取得
        var replaceMachineId = getValueByCtrl(targetCtrl, CtrlFlag.Label);
        // 現場機器と代替機器が同じ場合、エラーメッセージを表示
        if (currentMachineId == replaceMachineId) {
            // 「現場機器と代替機器が重複しています。」
            strMessage = P_ComMsgTranslated[141090001];
            setMessage(strMessage, procStatus.Error);
            return false;
        }
        // 機器交換情報を非表示
        changeDisplayReplaceMachineInfo(true);
        // フォーカス設定(現場機器の(交換後)使用区分)
        var tr = $(P_Article).find("#" + FormReplace.ConfirmList.CurrentInfo2.Id + getAddFormNo()).find("tbody tr:not([class^='base_tr'])");
        $(getElement(tr, FormReplace.ConfirmList.CurrentInfo2.AfterUseSegmentStructureId, CtrlFlag.Input)).focus();
    }

    if (formNo == FormSelectMachine.No) {
        if (btnCtrlId == FormSelectMachine.ButtonId.ChangeManagement) {
            //管理基準から保全内容を追加するボタン押下時
            changeManagement(true);
        } else if (btnCtrlId == FormSelectMachine.ButtonId.ChangeNotManagement) {
            //管理基準外の保全内容を追加するボタン押下時
            changeManagement(false);
        }
    }
}

/**
 * 管理基準の表示切替
 * @param {any} flg 管理基準の場合true,管理基準外の場合false
 */
function changeManagement(flg) {

    //処理継続用ｺｰﾙﾊﾞｯｸ関数呼出文字列を設定
    var eventFunc = function () {
        // 検索結果をクリア
        clearSearchResult(FormSelectMachine.List.Id, FormSelectMachine.No);
        //明細エリアを非表示
        changeDisplayDetailArea(true);

        //管理基準から保全内容を追加するボタン、管理基準外の保全内容を追加するボタンの表示切替
        setHideButton(FormSelectMachine.ButtonId.ChangeManagement, flg);
        setHideButton(FormSelectMachine.ButtonId.ChangeNotManagement, !flg);

        //非表示項目の管理基準に値設定
        var val = flg ? ManagementDefine.Management : ManagementDefine.NotManagement;
        setValue(FormSelectMachine.HideInfo.Id, FormSelectMachine.HideInfo.ManagementStandard, 0, CtrlFlag.Label, val);

        // 管理基準外の場合は検索条件の保全部位を非表示
        var inspectionSite = $(getCtrl(FormSelectMachine.DetailCondition.Id, FormSelectMachine.DetailCondition.InspectionSite, 1, CtrlFlag.TextBox)).closest('tr');
        setHide(inspectionSite, !flg);
        // 管理基準外の場合は検索条件の保全項目を非表示
        var inspectionContent = $(getCtrl(FormSelectMachine.DetailCondition.Id, FormSelectMachine.DetailCondition.InspectionContent, 1, CtrlFlag.TextBox)).closest('tr');
        setHide(inspectionContent, !flg);

        // 処理終了
        return;
    }

    var strMessage = "";
    var table = P_listData["#" + FormSelectMachine.List.Id + getAddFormNo()];
    if (table.getDataCount() > 0) {
        strMessage = P_ComMsgTranslated[141090002];//「検索結果は破棄されます。よろしいですか？」
    }
    // メッセージ表示
    if (!popupMessage([strMessage], messageType.Confirm, eventFunc)) {
        //『キャンセル』の場合、処理中断
        return false;
    }
}

/**
 *【オーバーライド用関数】
 *  行削除の後
 *
 *  @appPath    {string}    ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @btn        {<a>}       ：行削除ﾘﾝｸのa要素
 *  @id         {string}    ：行削除ﾘﾝｸの対象一覧のCTRLID
 */
function postDeleteRow(appPath, btn, id) {
    //保全活動区分（非表示）の値取得
    var divisionId = getValue(FormRegist.HideInfo.Id, FormRegist.HideInfo.DivisionId, 1, CtrlFlag.Label);
    if (divisionId == DivisionIdDefine.Failure) {
        //故障情報の場合

        //行追加ボタンを表示する
        setHideButtonTopForList(FormRegist.MachineList.Id, actionkbn.AddNewRow, false);
    }

    // 対象機器一覧で行削除された場合
    if (id == FormRegist.MachineList.Id + getAddFormNo()) {

        // 機器使用期間の入力可能レコードを制限する
        // 同一機器に対する先頭行のレコードのみ入力可能とする
        unEnableUseDays(P_listData["#" + FormRegist.MachineList.Id + getAddFormNo()].getRows());
    }
}

/**
 *【オーバーライド用関数】
 *  画面再表示ﾃﾞｰﾀ取得関数呼出前
 *
 *  @appPath {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId {string} ：機能ID
 *  @pgmId {string} ：ﾌﾟﾛｸﾞﾗﾑID
 *  @formNo {number} ：画面番号
 *  @originNo {number} ：遷移元の親画面番号
 *  @btnCtrlId {number} ：ｱｸｼｮﾝﾎﾞﾀﾝのCTRLID
 *  @conductPtn {number} ：機能処理ﾊﾟﾀｰﾝ(10:入力、11：バッチ、20：帳票、30：マスタ)
 *  @selectData {number} ：NOﾘﾝｸ選択行のﾃﾞｰﾀ {List<Dictionary<string, object>>}
 *  @targetCtrlId {number} ：単票入力画面から戻る時、該当一覧のCTRLID
 *  @listData {} ：
 *  @codeTransFlg {int}    ：1:コード＋翻訳 選ボタンから画面遷移/1以外:それ以外
 *  @status  {CommonProcReturn} : 実行処理結果ｽﾃｰﾀｽ
 *  @param	{number}	selFlg : 共通機能から選択ボタン押下で戻った場合のみ、「1:selFlgDef.Selected」が渡る
 *  @param	{string}	backFrom : 共通機能からの戻る処理時、戻る前の共通機能ID ※他機能同タブ遷移でも使える？
 *  @param {number} transPtn ：画面遷移のパターン、transPtnDef
 */
function beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom, transPtn) {

    // 機能IDが「帳票出力」の場合
    RM0001_beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom);

    // 共通-文書管理詳細画面の画面再表示前処理
    // 共通-文書管理詳細画面を閉じたときの再表示処理はこちらで行うので、各機能での呼出は不要
    DM0002_beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom);

    // 共通-担当者検索画面を閉じたとき
    SU0001_beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom);

    if (formNo == FormDetail.No) {
        //戻る・閉じるボタンの表示制御
        setDisplayCloseBtn(transPtn);
    }
    else if (formNo == FormRegist.No) {
        if (KeyNameFromScheduleLink in P_dicIndividual == true) {

            // 件名別長期計画・機器別長期計画の白丸「○」で遷移してきた際のキーがグローバルリストに存在する場合
            setDisplayCloseBtn(transPtnDef.OtherTab);
        }
        else {
            setDisplayCloseBtn(transPtn);
        }
    }

    if (formNo == FormRegist.No && btnCtrlId == FormSelectMachine.ButtonId.Cancel) {
        //機器選択画面から登録画面へ戻った際、選択された機器データを対象機器に追加する
        var data = P_dicIndividual[MA0001_SelectMachineList];
        if (data && data.length > 0) {
            setSelectMachineDataForTargetList(appPath, data);
        }
        delete P_dicIndividual[MA0001_SelectMachineList];
    }
}

/**
 * 機器選択画面で選択されたデータを登録画面の対象機器に追加する
 * @param {any} appPath ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 * @param {any} machineData 機器選択画面で選択されたデータ
 */
function setSelectMachineDataForTargetList(appPath, machineData) {
    //処理継続用ｺｰﾙﾊﾞｯｸ関数呼出文字列を設定
    var eventFunc = function (status, data) {
        if (!data || data.length <= 0) {
            // 処理終了
            return;
        }

        // 表示データ
        var addData = data.slice(1);//先頭行削除
        if (!addData || addData.length <= 0) {
            // 処理終了
            return;
        }

        var table = P_listData["#" + FormRegist.MachineList.Id + getAddFormNo()];
        var machineListData = table.getData();

        var maxRowNo = 0;
        if (machineListData && machineListData.length > 0) {
            var rowNoList = machineListData.map(x => x.ROWNO);
            //対象機器のROWNO最大値を取得
            maxRowNo = Math.max.apply(null, rowNoList);
        }
        //追加するデータのROWNO、ROWSTATUSを再設定
        $.each(addData, function (idx, rowData) {
            //ROWNOに対象機器の最大値以降の値を設定
            rowData.ROWNO = maxRowNo + 1;
            maxRowNo++;
            //ROWSTATUSに新規を設定
            rowData.ROWSTATUS = rowStatusDef.New;
        });
        //データ追加
        table.addData(addData, false);
        //ユーザの役割に「製造権限」のみが含まれる場合、対象機器の入力項目はラベル化
        changeLabelForMachineList(table.getRows());
        table.redraw();

        //保全活動区分（非表示）の値取得
        var divisionId = getValue(FormRegist.HideInfo.Id, FormRegist.HideInfo.DivisionId, 1, CtrlFlag.Label);
        if (divisionId == DivisionIdDefine.Failure) {
            //故障情報の場合

            //行追加ボタンを非表示
            setHideButtonTopForList(FormRegist.MachineList.Id, actionkbn.AddNewRow, true);
        }

        // 機器使用期間の入力可能レコードを制限する
        // 同一機器に対する先頭行のレコードのみ入力可能とする
        unEnableUseDays(table.getRows());

        // 処理終了
        return;
    }

    //ajax通信で選択した機器の情報を取得
    ajaxCommon("GetSelectMachineData", appPath, FormSelectMachine.No, ConductId_MA0001, true, machineData, eventFunc);
}

/**
 * 【オーバーライド用関数】階層選択モーダル画面の選択ボタン実行後
 * @param {any} appPath アプリケーションパス
 * @param {any} btn 選択ボタン要素
 * @param {any} ctrlId コントロールID
 * @param {any} structureGrpId 構成グループID
 * @param {any} maxStructureNo 最下層階層番号
 * @param {any} node 設定値取得先のノード
 */
function afterSelectBtnForTreeView(appPath, btn, ctrlId, structureGrpId, maxStructureNo, node) {
    //個別工場表示フラグを取得してタブの表示を切り替える
    //保全権限があり、工場が変更された場合のみ

    if (structureGroupDef.Location != structureGrpId) {
        //場所階層以外の場合、終了
        return;
    }

    //保全権限フラグの値取得
    var maintenanceStr = getValue(FormRegist.HideInfo.Id, FormRegist.HideInfo.Maintenance, 1, CtrlFlag.Label);
    var maintenanceFlg = convertStrToBool(maintenanceStr);
    if (!maintenanceFlg) {
        //保全権限がない場合、終了（依頼情報タブのみ表示の為）
        return;
    }

    //選択前の工場ID
    var oldFactoryId = P_dicIndividual["factoryId"];
    //選択された工場ID
    var factoryId = getTreeViewFacrotyId(node);
    if (factoryId == 0 || factoryId.toString() == oldFactoryId.toString()) {
        //工場の指定がないまたは工場の変更がない場合、終了
        return;
    }
    //工場IDを設定
    P_dicIndividual["factoryId"] = factoryId;

    //処理継続用ｺｰﾙﾊﾞｯｸ関数呼出文字列を設定
    var eventFunc = function (status, data) {
        //工場変更前の保全履歴個別工場表示フラグ
        var oldHistoryIndividualFlg = getValue(FormRegist.HideInfo.Id, FormRegist.HideInfo.HistoryIndividualFlg, 1, CtrlFlag.Label);
        //工場変更前の故障分析個別工場表示フラグ
        var oldFailureIndividualFlg = getValue(FormRegist.HideInfo.Id, FormRegist.HideInfo.FailureIndividualFlg, 1, CtrlFlag.Label);

        //工場変更後の保全履歴個別工場表示フラグ
        var newHistoryIndividualFlg = P_dicIndividual["historyIndividualFlg"];
        //工場変更後の故障分析個別工場表示フラグ
        var newFailureIndividualFlg = P_dicIndividual["failureIndividualFlg"];

        //保全活動区分（非表示）の値取得
        var divisionId = getValue(FormRegist.HideInfo.Id, FormRegist.HideInfo.DivisionId, 1, CtrlFlag.Label);

        if (oldHistoryIndividualFlg == newHistoryIndividualFlg && (divisionId == DivisionIdDefine.Failure ? oldFailureIndividualFlg == newFailureIndividualFlg : true)) {
            //タブの表示切替が不要の場合（個別工場表示フラグが一致する場合）、処理終了
            return;
        }

        var msgFunc = function () {
            //保全履歴、故障分析タブの表示切替
            changeDisplayTab(oldHistoryIndividualFlg, oldFailureIndividualFlg, newHistoryIndividualFlg, newFailureIndividualFlg, divisionId);
            return;
        }
        //メッセージ「保全履歴情報、故障分析情報の入力内容が変更になります。」
        var strMessage = P_ComMsgTranslated[141300006];
        popupMessage([strMessage], messageType.Info, msgFunc);
        // 処理終了
        return;
    }

    //ajax通信で個別工場表示フラグを取得
    ajaxCommon("GetIndividualFlg", appPath, FormRegist.No, ConductId_MA0001, true, null, eventFunc);
}

/**
 * 保全履歴、故障分析タブの表示切替
 * @param {any} oldHistoryIndividualFlg 工場変更前の保全履歴個別工場表示フラグ
 * @param {any} oldFailureIndividualFlg 工場変更前の故障分析個別工場表示フラグ
 * @param {any} newHistoryIndividualFlg 工場変更後の保全履歴個別工場表示フラグ
 * @param {any} newFailureIndividualFlg 工場変更後の故障分析個別工場表示フラグ
 * @param {any} divisionId 保全活動区分(点検・故障)
 */
function changeDisplayTab(oldHistoryIndividualFlg, oldFailureIndividualFlg, newHistoryIndividualFlg, newFailureIndividualFlg, divisionId) {
    //非表示の保全履歴個別工場表示フラグに値設定
    setValue(FormRegist.HideInfo.Id, FormRegist.HideInfo.HistoryIndividualFlg, 1, CtrlFlag.Label, newHistoryIndividualFlg);
    //非表示の故障分析個別工場表示フラグに値設定
    setValue(FormRegist.HideInfo.Id, FormRegist.HideInfo.FailureIndividualFlg, 1, CtrlFlag.Label, newFailureIndividualFlg);

    //選択中のタブ要素
    var tab = $(P_Article).find(".tab_contents.selected");
    //選択中のタブ番号
    var tabNo = $(tab).data("tabno");

    //保全履歴タブの表示切替
    changeDisplayHistoryTab(FormRegist.No);
    //選択中のタブが表示切替対象の場合、タブの選択も切り替える
    if (oldHistoryIndividualFlg != newHistoryIndividualFlg && (tabNo == FormRegist.TabNo.History || tabNo == FormRegist.TabNo.HistoryIndividual)) {
        var selectTabNo = newHistoryIndividualFlg == IndividualFlg.Show ? FormRegist.TabNo.HistoryIndividual : FormRegist.TabNo.History;
        var tabBtn = $(P_Article).find(".tab_btn.detail a[data-tabno='" + selectTabNo + "']");
        if (tabBtn != null && tabBtn.length > 0) {
            //選択状態にする
            $(tabBtn[0]).click();
        }
    }

    if (divisionId == DivisionIdDefine.Failure) {
        //故障情報の場合、故障分析情報タブの表示切替
        changeDisplayFailureTab(FormRegist.No);
        //選択中のタブが表示切替対象の場合、タブの選択も切り替える
        if (oldFailureIndividualFlg != newFailureIndividualFlg && (tabNo == FormRegist.TabNo.Failure || tabNo == FormRegist.TabNo.FailureIndividual)) {
            var selectTabNo = newFailureIndividualFlg == IndividualFlg.Show ? FormRegist.TabNo.FailureIndividual : FormRegist.TabNo.Failure;
            var tabBtn = $(P_Article).find(".tab_btn.detail a[data-tabno='" + selectTabNo + "']");
            if (tabBtn != null && tabBtn.length > 0) {
                //選択状態にする
                $(tabBtn[0]).click();
            }
        }
    }
}

/**
 * 【オーバーライド用関数】Tabuator一覧の描画前処理
 * @param {any} id 一覧のID(#,_FormNo付き)
 * @param {any} options 一覧のオプション情報
 * @param {any} header ヘッダー情報
 * @param {any} dispData データ
 */
function prevCreateTabulator(appPath, id, options, header, dispData) {

    // 機能IDが「帳票出力」の場合
    if (getConductId() == RM00001_ConductId) {
        return RM0001_prevCreateTabulator(appPath, id, options, header, dispData);
    }

    if (getFormNo() == FormSelectMachine.No && id == '#' + FormSelectMachine.List.Id + getAddFormNo()) {
        //機器選択画面

        //管理基準（非表示）の値取得
        var managementStandard = getValue(FormSelectMachine.HideInfo.Id, FormSelectMachine.HideInfo.ManagementStandard, 1, CtrlFlag.Label);
        $.each(header, function (idx, head) {
            if (head.field == "VAL" + FormSelectMachine.List.InspectionSite || head.field == "VAL" + FormSelectMachine.List.InspectionContent) {
                if (managementStandard == ManagementDefine.NotManagement) {
                    //管理基準外の場合

                    //保全部位、保全内容はコンボボックスを表示
                    if (head.cssClass) {
                        if (head.cssClass.indexOf("not_readonly") == -1) {
                            head.cssClass = head.cssClass + " not_readonly";
                        }
                    } else {
                        head.cssClass = "not_readonly";
                    }
                } else {
                    if (head.cssClass) {
                        //保全部位、保全内容はラベル表示
                        head.cssClass = head.cssClass.replace("not_readonly", "");
                    }
                }
            }
        });
    }
}

/**
 *【オーバーライド用関数】
 *  画面状態設定後の個別実装
 *
 * @status        ：ﾍﾟｰｼﾞ状態　0：初期状態、1：検索後、2：明細表示後ｱｸｼｮﾝ、3：ｱｯﾌﾟﾛｰﾄﾞ後
 * @pageRowCount  ：ﾍﾟｰｼﾞ全体のﾃﾞｰﾀ行数
 * @conductPtn    ：com_conduct_mst.ptn
 * @formNo        ：画面番号
 */
function setPageStatusEx(status, pageRowCount, conductPtn, formNo) {

    // 機能IDが「帳票出力」の場合
    if (getConductId() == RM00001_ConductId) {
        return RM0001_setPageStatusEx(status, pageRowCount, conductPtn, formNo);
    }

}

/**
 * 【オーバーライド用関数】Tabulatorの描画が完了時の処理
 * @param {any} tbl 一覧
 * @param {any} id 一覧のID(#,_FormNo付き)
 */
function postBuiltTabulator(tbl, id) {

    // 機能IDが「帳票出力」の場合
    if (getConductId() == RM00001_ConductId) {
        return RM0001_postBuiltTabulator(tbl, id);
    }

    // 文書管理 詳細画面
    DM0002_postBuitTabulator(tbl, id);

    if (getFormNo() == FormList.No && id == '#' + FormList.List.Id + getAddFormNo()) {
        // 一覧フィルタ処理実施
        callExecuteListFilter(FormList.List.Id, FormList.Filter.Id, FormList.Filter.Input);
    }

    if (getFormNo() == FormDetail.No && id == '#' + FormDetail.MachineList.Id + getAddFormNo()) {
        //詳細画面の対象機器一覧

        var rows = tbl.getRows();

        //保全活動区分（非表示）の値取得
        var divisionId = getValue(FormDetail.HideInfo.Id, FormDetail.HideInfo.DivisionId, 1, CtrlFlag.Label);
        if (divisionId == DivisionIdDefine.Inspection) {
            //点検の場合
            setTimeout(function () { //ブラウザに処理が戻った際に実行
                //対象機器一覧の一部列を表示に戻す
                $.each(FormDetail.MachineList.HideCol, function (key, val) {
                    changeTabulatorColumnDisplay(FormDetail.MachineList.Id, val, true);
                });
                if (rows == null || rows.length <= 0) {
                    tbl.clearData();
                }
            }, 0);
        } else if (divisionId == DivisionIdDefine.Failure) {
            //故障の場合
            setTimeout(function () { //ブラウザに処理が戻った際に実行
                //対象機器一覧の一部列を非表示
                $.each(FormDetail.MachineList.HideCol, function (key, val) {
                    changeTabulatorColumnDisplay(FormDetail.MachineList.Id, val, false);
                });
                if (rows == null || rows.length <= 0) {
                    tbl.clearData();
                }
            }, 0);
        }

        //廃止の行はグレーアウトする
        if (rows != null && rows.length > 0) {
            $.each(rows, function (i, row) {
                //グレーアウトフラグの取得
                var flag = row.getData()['VAL' + FormDetail.MachineList.GrayFlag];
                if (flag == 1) {
                    var ele = row.getElement();
                    $(ele).addClass('row_gray');
                }
            });
        }

    }
    if (getFormNo() == FormRegist.No && id == '#' + FormRegist.MachineList.Id + getAddFormNo()) {
        //登録画面の対象機器一覧

        //全行
        var rows = tbl.getRows();

        //アクションのID
        var actionCtrlId = P_dicIndividual["actionCtrlId"];

        //保全活動区分（非表示）の値取得
        var divisionId = getValue(FormRegist.HideInfo.Id, FormRegist.HideInfo.DivisionId, 1, CtrlFlag.Label);
        if (divisionId == DivisionIdDefine.Inspection) {
            //点検の場合
            setTimeout(function () { //ブラウザに処理が戻った際に実行
                //対象機器一覧の一部列を表示に戻す
                $.each(FormDetail.MachineList.HideCol, function (key, val) {
                    changeTabulatorColumnDisplay(FormRegist.MachineList.Id, val, true);
                });
                if (rows == null || rows.length <= 0) {
                    tbl.clearData();
                }
            }, 0);

            if (actionCtrlId == FormDetail.ButtonId.Copy) {
                //複写の場合
                $.each(rows, function (idx, row) {
                    //対象機器　フォロー予定年月、フォロー完了日にブランク設定
                    var updateVal = {};
                    updateVal["VAL" + FormRegist.MachineList.FollowPlanDate] = "";
                    updateVal["VAL" + FormRegist.MachineList.FollowCompletionDate] = "";
                    row.update(updateVal);
                });
            }

            // 機器使用期間の入力可能レコードを制限する
            // 同一機器に対する先頭行のレコードのみ入力可能とする
            unEnableUseDays(rows);

        } else if (divisionId == DivisionIdDefine.Failure) {
            //故障の場合
            setTimeout(function () { //ブラウザに処理が戻った際に実行
                //対象機器一覧の一部列を非表示
                $.each(FormDetail.MachineList.HideCol, function (key, val) {
                    changeTabulatorColumnDisplay(FormRegist.MachineList.Id, val, false);
                });
                if (rows == null || rows.length <= 0) {
                    tbl.clearData();
                }
            }, 0);

            if (rows.length > 0) {
                //対象機器は１件のみの為、行追加ボタンを非表示にする
                setHideButtonTopForList(FormRegist.MachineList.Id, actionkbn.AddNewRow, true);
            }
        }

        if (actionCtrlId == FormDetail.ButtonId.Copy || actionCtrlId == FormList.ButtonId.NewInspection || actionCtrlId == FormList.ButtonId.New) {
            //複写の場合、新規（長期計画の白丸「○」リンクから遷移）の場合
            //対象機器の各行のROWSTATUSを新規に設定
            $.each(rows, function (idx, row) {
                row.update({ ROWSTATUS: rowStatusDef.New });
            });
        }

        //ユーザの役割に「製造権限」のみが含まれる場合、対象機器の入力項目はラベル化
        changeLabelForMachineList(rows);
    }
    if (getFormNo() == FormSearch.No && id == '#' + FormSearch.List.Id + getAddFormNo()) {
        // 検索選択画面の一覧の選択チェックボックス列を非表示
        changeRowControl(FormSearch.List.Id, false);
    }

    if (getFormNo() == FormSelectMachine.No && id == '#' + FormSelectMachine.List.Id + getAddFormNo()) {
        //活動区分（非表示）の値取得
        var divisionId = getValue(FormSelectMachine.HideInfo.Id, FormSelectMachine.HideInfo.DivisionId, 1, CtrlFlag.Label);
        if (divisionId == DivisionIdDefine.Inspection) {
            //点検情報の場合

            var rows = tbl.getRows();

            // 機器選択画面の一覧の選択（ボタン）列を非表示
            changeTabulatorColumnDisplay(FormSelectMachine.List.Id, FormSelectMachine.List.Select, false);
        } else {
            // 一覧の全選択/全解除ボタン、選択チェックボックス列を非表示
            changeRowControl(FormSelectMachine.List.Id, false);
            // 一覧の保全部位列を非表示
            changeTabulatorColumnDisplay(FormSelectMachine.List.Id, FormSelectMachine.List.InspectionSite, false);
            // 一覧の保全内容列を非表示
            changeTabulatorColumnDisplay(FormSelectMachine.List.Id, FormSelectMachine.List.InspectionContent, false);
        }
    }
}

/**
 * 【オーバーライド用関数】Tabulatorのページ変更後の処理
 * @param {any} tbl 処理対象の一覧
 * @param {any} id 処理対象の一覧のID(#,_FormNo付き)
 * @param {any} pageNo 表示するページNo
 * @param {any} pagesize 表示中のページの件数
 */
function postTabulatorChangePage(tbl, id, pageNo, pagesize) {
    var rows = tbl.getRows();
    if (getFormNo() == FormRegist.No && id == '#' + FormRegist.MachineList.Id + getAddFormNo()) {
        //ユーザの役割に「製造権限」のみが含まれる場合、対象機器の入力項目はラベル化
        changeLabelForMachineList(rows);

        // 機器使用期間の入力可能レコードを制限する
        // 同一機器に対する先頭行のレコードのみ入力可能とする
        unEnableUseDays(rows);
    }
}

/**
 * 対象機器の入力項目をラベル化
 * @param {any} rows 行
 */
function changeLabelForMachineList(rows) {

    //保全権限フラグの値取得
    var maintenanceStr = getValue(FormRegist.HideInfo.Id, FormRegist.HideInfo.Maintenance, 1, CtrlFlag.Label);
    var maintenanceFlg = convertStrToBool(maintenanceStr);
    if (!maintenanceFlg) {
        //ユーザの役割に「保全権限」が含まれない場合
        //対象機器の入力項目はラベル化
        if (rows.length > 0) {
            $.each(rows, function (idx, row) {
                //ブラウザに処理が戻った際に実行
                setTimeout(function () {
                    var ele = row.getElement();
                    var visibleEle = $(ele).find("[tabulator-field^='VAL']:not(.readonly) input");
                    var cell = $(visibleEle).closest("div.tabulator-cell");
                    $(cell).addClass("readonly");
                }, 0);
            });
        }
    }
}

/**
 *【オーバーライド用関数】実行ﾁｪｯｸ処理 - 前処理
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 */
function registCheckPre(appPath, conductId, formNo, btn) {
    if (formNo == FormSearch.No) {
        // 機器検索画面

        // 対象行取得
        var row = $(btn).closest(".tabulator-row");
        if (row && row.length > 0) {
            // 対象セル(機番ID)取得
            var td = getDataTd(row, FormSearch.List.MachineId);
            // 機番IDをグローバル変数にセット
            P_dicIndividual[MA0001_selectMachineId] = getCellVal(td);
        }

        // 閉じるボタンをクリック
        var backBtn = $(P_Article).find("input:button[data-actionkbn='" + actionkbn.Back + "']");
        $(backBtn).click();

        return false;
    }

    if (formNo == FormSelectMachine.No) {
        // 機器選択画面

        //ボタン名
        var btnName = $(btn).attr("name");
        // 画面変更ﾌﾗｸﾞ初期化(編集内容破棄メッセージを表示しないようにする)
        dataEditedFlg = false;

        if (btnName == FormSelectMachine.ButtonId.Select) {
            //選択（ボタン）押下時

            // 対象行取得
            var ele = $(btn).closest(".tabulator-row");
            if (ele && ele.length > 0) {
                //非表示の選択チェックボックス列にチェックを付ける
                var checkbox = $(ele).find("div[tabulator-field='SELTAG'] input[type='checkbox']");
                $(checkbox).prop("checked", true);
                $(checkbox).trigger("change");
            }
            // 検索結果を保存
            P_dicIndividual[MA0001_SelectMachineList] = getListDataAll(FormSelectMachine.No, 0);
            // 閉じるボタンをクリック
            var backBtn = $(P_Article).find("input:button[data-actionkbn='" + actionkbn.Back + "']");
            $(backBtn).click();

        } else if (btnName == FormSelectMachine.ButtonId.Confirm) {
            //確定ボタン押下時

            // 機器一覧にてチェックされた行が存在しない場合、エラーメッセージ表示
            if (!isCheckedList(FormSelectMachine.List.Id, getMessageParam(P_ComMsgTranslated[141060001], [P_ComMsgTranslated[111070019]]))) {
                return false;
            }
            // 検索結果を保存
            P_dicIndividual[MA0001_SelectMachineList] = getListDataAll(FormSelectMachine.No, 0);

            // 閉じるボタンをクリック
            var backBtn = $(P_Article).find("input:button[data-actionkbn='" + actionkbn.Back + "']");
            $(backBtn).click();
        }
        return false;
    }
    return true;
}

/**
 *【オーバーライド用関数】行追加の前
 *  @param {string}     appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {html}       element     ：ｲﾍﾞﾝﾄ発生要素
 *  @param {boolean}    isCopyRow   ：行コピーフラグ(true：行コピー、false：行追加)
 *  @param {number}     transPtn    ：画面遷移ﾊﾟﾀｰﾝ(1：子画面、2：単票入力、3：単票参照、4：共通機能、5：他機能別ﾀﾌﾞ、6：他機能表示切替)
 *  @param {string}     transTarget ：遷移先(子画面NO / 単票一覧ctrlId / 共通機能ID / 他機能ID|画面NO)
 *  @param {number}     dispPtn     ：遷移表示ﾊﾟﾀｰﾝ(1：表示切替、2：ﾎﾟｯﾌﾟｱｯﾌﾟ、3：一覧直下)
 *  @param {number}     formNo      ：遷移元formNo
 *  @param {string}     ctrlId      ：遷移元の一覧ctrlid
 *  @param {string}     btn_ctrlId  ：ボタンのbtn_ctrlid
 *  @param {int}        rowNo       ：遷移元の一覧の選択行番号（新規の場合は-1）
 *  @param {boolean}    confirmFlg  ：遷移前確認ﾌﾗｸﾞ(true：確認する、false：確認しない)
 */
function prevAddNewRow(appPath, element, isCopyRow, transPtn, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, confirmFlg) {
    if (ctrlId == FormRegist.MachineList.Id) {
        //対象機器一覧のプラスアイコン押下時、変更破棄メッセージを出さない
        confirmScrapBeforeTrans(appPath, transPtn, transDivDef.None, transTarget, dispPtn, formNo, ctrlId, rowNo, -1, element, false);
        return false;
    }

    return true;
}

/**
 *【オーバーライド用関数】
 *  共通機能用選択ボタン押下時処理
 *  @param {string}     appPath     :ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string}     cmConductId :共通機能ID
 *  @param {article}    cmArticle   :共通機能articleタグ
 *  @param {button}     btn         :選択ボタン要素
 *  @param {string}     fromCtrlId  :共通機能遷移時に押下したﾎﾞﾀﾝ/一覧のｺﾝﾄﾛｰﾙID
 */
function clickComConductSelectBtn(appPath, cmConductId, cmArticle, btn, fromCtrlId) {

    // 担当者検索画面
    SU0001_clickComConductSelectBtn(appPath, cmConductId, cmArticle, btn, fromCtrlId, ConductId_MA0001);

}

/**
 *【オーバーライド用関数】
 * 共通機能へデータを渡す
 * @param {string}                      appPath         :ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 * @param {number}                      conductId       :共通機能ID
 * @param {number}                      parentNo        :親画面NO
 * @param {Array.<Dictionary<string, string>>}  conditionDataList   :条件ﾃﾞｰﾀ
 * @param {string}                      ctrlId          :遷移元の一覧ctrlid
 * @param {string}                      btn_ctrlId      :ボタンのbtn_ctrlid
 * @param {number}                      rowNo           :遷移元の一覧の選択行番号（一覧行でない場合は-1）
 * @param {Element}                     element         :ｲﾍﾞﾝﾄ発生要素
 * @param {string}                      parentConductId :遷移元の個別機能ID
 */
function passDataCmConduct(appPath, conductId, parentNo, conditionDataList, ctrlId, btn_ctrlId, rowNo, element, parentConductId) {

    // 共通-帳票出力画面の個別実装ボタン
    RM0001_passDataCmConduct(appPath, conductId, parentNo, conditionDataList, ctrlId, btn_ctrlId, rowNo, element, parentConductId);

    // 担当者検索画面
    SU0001_passDataCmConduct(appPath, conductId, parentNo, conditionDataList, ctrlId, btn_ctrlId, rowNo, element, parentConductId);
}

/**
 * 【オーバーライド用関数】
 * 　コード＋翻訳 他の項目に値セット処理
 *
 *  @appPath {string}   　　　　     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @formNo {byte}                   ：画面番号
 *  @ctrl {要素}   　　              ：コード＋翻訳要素
 *  @data {List}                     ：取得データ
 */
function setCodeTransOtherNames(appPath, formNo, ctrl, data) {
    //担当者項目の制御
    if (!data && $(ctrl).val() != "") {
        //ユーザマスタに紐づくユーザが取得できない場合、トランザクションテーブルに登録されている名前を表示する
        if (formNo == FormDetail.No) {
            //詳細画面
            setPersonnelName(ctrl, formNo);
        }
        if (formNo == FormRegist.No) {
            //登録画面
            setPersonnelName(ctrl, formNo);
        }
    }
}

/**
 * 担当者項目に名前を設定
 * @param {any} ctrl コード＋翻訳要素
 * @param {any} formNo 画面No
 */
function setPersonnelName(ctrl, formNo) {
    var formRequestDefine = formNo == FormDetail.No ? FormDetail.RequestList : FormRegist.RequestList.List3;
    var formHistoryDefine = formNo == FormDetail.No ? FormDetail : FormRegist;

    //処理対象のコントロールのID属性を取得
    var id = $(ctrl).attr("id");
    //依頼タブ 依頼担当のコントロールのID属性を取得
    var personnelCtrlId = getTextBoxId(formRequestDefine.Id, formRequestDefine.Personnel);
    //依頼タブ 依頼係長のコントロールのID属性を取得
    var chiefCtrlId = getTextBoxId(formRequestDefine.Id, formRequestDefine.Chief);
    //依頼タブ 依頼課長のコントロールのID属性を取得
    var managerCtrlId = getTextBoxId(formRequestDefine.Id, formRequestDefine.Manager);
    //依頼タブ 依頼職長のコントロールのID属性を取得
    var foremanCtrlId = getTextBoxId(formRequestDefine.Id, formRequestDefine.Foreman);
    //履歴タブ 施工担当者のコントロールのID属性を取得
    var constructionCtrlId = getTextBoxId(formHistoryDefine.HistoryList.Id, formHistoryDefine.HistoryList.ConstructionPersonnel);
    //履歴(個別工場)タブ SEG担当者のコントロールのID属性を取得
    var segCtrlId = getTextBoxId(formHistoryDefine.HistoryIndividualList.Id, formHistoryDefine.HistoryIndividualList.SegPersonnel);
    //履歴(個別工場)タブ 製造担当者のコントロールのID属性を取得
    var constructionIndividualCtrlId = getTextBoxId(formHistoryDefine.HistoryIndividualList.Id, formHistoryDefine.HistoryIndividualList.ConstructionPersonnel);

    var name = "";
    switch (id) {
        case personnelCtrlId:
            //依頼担当名の取得
            name = getValue(formRequestDefine.Id, formRequestDefine.PersonnelName, 1, CtrlFlag.Label);
            break;
        case chiefCtrlId:
            //依頼係長名の取得
            name = getValue(formRequestDefine.Id, formRequestDefine.ChiefName, 1, CtrlFlag.Label);
            break;
        case managerCtrlId:
            //依頼課長名の取得
            name = getValue(formRequestDefine.Id, formRequestDefine.ManagerName, 1, CtrlFlag.Label);
            break;
        case foremanCtrlId:
            //依頼職長名の取得
            name = getValue(formRequestDefine.Id, formRequestDefine.ForemanName, 1, CtrlFlag.Label);
            break;
        case constructionCtrlId:
            //施工担当者の取得
            name = getValue(formHistoryDefine.HistoryList.Id, formHistoryDefine.HistoryList.ConstructionPersonnelName, 1, CtrlFlag.Label);
            break;
        case segCtrlId:
            //SEG担当者の取得
            name = getValue(formHistoryDefine.HistoryIndividualList.Id, formHistoryDefine.HistoryIndividualList.SegPersonnelName, 1, CtrlFlag.Label);
            break;
        case constructionIndividualCtrlId:
            //製造担当者の取得
            name = getValue(formHistoryDefine.HistoryIndividualList.Id, formHistoryDefine.HistoryIndividualList.ConstructionPersonnelName, 1, CtrlFlag.Label);
            break;
    }
    if (name) {
        //ユーザ名を設定
        setNameToCodeTrans(ctrl, name);
    }
}

/**
 * 【オーバーライド用関数】画面初期値データ取得時の詳細検索条件取得後処理
 * @param {any} appPath ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 * @param {any} conductId 機能ID
 * @param {any} formNo 画面番号
 * @param {any} conditionDataList 条件データ
 * @param {any} detailConditionDataList 詳細検索条件データ
 */
function afterInitGetDetailConditionData(appPath, conductId, formNo, conditionDataList, detailConditionDataList) {
    if (formNo != FormList.No) {
        //一覧画面以外の場合、処理なし
        return;
    }
    //保全実績評価から遷移時の検索条件を取得
    var data = [];
    if (conditionDataList != null && conditionDataList.length > 0) {
        data = $.grep(conditionDataList, function (elem, index) {
            return elem.CTRLID == MA0001_DetailSearchCondition;
        });
    }

    if ((!data || data.length == 0) && !P_dicIndividual[MA0001_DetailSearchCondition]) {
        //保全実績評価からの遷移でない場合
        return;
    }
    if (data && data.length > 0) {
        //保全実績評価から遷移してきた初期表示時

        //検索条件の中で構成IDを使用する項目の設定
        setDataStructureId(appPath, data[0]);
    } else {
        //詳細画面等から一覧へ戻ってきた場合

        //退避しておいた検索条件を設定
        data.push(P_dicIndividual[MA0001_DetailSearchCondition]);
    }

    var info = [];
    info['FORMNO'] = FormList.No;// 画面番号
    info['ROWNO'] = 1;// 行番号
    info['IsDetailCondition'] = true;// 詳細検索条件フラグ：ON
    var conditionData = Object.assign({}, info, data[0]);
    conditionData['CTRLID'] = FormList.List.Id;// 一覧のctrlId
    var checked = false;
    $.each(conditionData, function (name, data) {
        if (name.indexOf('VAL') < 0 || name.indexOf('_checked') < 0) { return true; }
        if (!convertStrToBool(data)) {
            // チェック無しの場合、設定値をリストから削除
            delete conditionData[name.replace('_checked', '')];
        } else {
            // チェック有り
            checked = true;
        }
        // チェック状態格納用のデータをリストから削除
        delete conditionData[name];
    });
    if (checked) {
        //ローカルストレージに保存された検索条件は使用せず、保全実績評価から遷移時の検索条件を使用する
        if (detailConditionDataList != null && detailConditionDataList.length > 0) {
            detailConditionDataList.splice(0, 1, conditionData);
        } else {
            detailConditionDataList.push(conditionData);
        }
        if (!P_dicIndividual[MA0001_DetailSearchCondition]) {
            //検索条件を退避
            P_dicIndividual[MA0001_DetailSearchCondition] = Object.assign({}, data[0]);
        }
    }
}

/**
 * 検索条件の中で構成IDを使用する項目の設定
 * @param {any} appPath ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 * @param {any} conditionData 検索条件
 */
function setDataStructureId(appPath, conditionData) {
    if (conditionData["VAL" + FormList.List.Job]) {
        //職種
        if (conditionData["VAL" + FormList.List.Job] != JobAll) {
            //職種の指定有り
            getStructureIdList(appPath, structureGroupDef.Job, conditionData["VAL" + FormList.List.Job], FormList.List.Job, conditionData);
        }
        P_dicIndividual[MA0001_JobId] = conditionData["VAL" + FormList.List.Job];
    }
    if (conditionData["VAL" + FormList.List.MqClass]) {
        //MQ分類
        getStructureIdList(appPath, structureGroupDef.MqClass, conditionData["VAL" + FormList.List.MqClass], FormList.List.MqClass, conditionData);
    }
    if (conditionData["VAL" + FormList.List.StopSystem]) {
        //系停止
        getStructureIdList(appPath, structureGroupDef.StopSystem, conditionData["VAL" + FormList.List.StopSystem], FormList.List.StopSystem, conditionData);
    }
    if (conditionData["VAL" + FormList.List.Sudden]) {
        //突発区分
        getStructureIdList(appPath, structureGroupDef.Sudden, conditionData["VAL" + FormList.List.Sudden], FormList.List.Sudden, conditionData);
    }
}

/**
 * 拡張データから対象の構成IDのリストを取得する
 * @param {any} appPath ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 * @param {any} groupId 構成グループID
 * @param {any} extensionData 拡張データ(パイプ区切り)
 * @param {any} val VAL値
 * @param {any} extensionData 検索条件
 */
function getStructureIdList(appPath, groupId, extensionData, val, conditionData) {
    //構成グループID
    P_dicIndividual[MA0001_GroupId] = groupId;
    //拡張データ
    P_dicIndividual[MA0001_ExtensionData] = extensionData;

    //処理継続用ｺｰﾙﾊﾞｯｸ関数呼出文字列を設定
    var eventFunc = function (status, data) {
        //構成IDリスト
        conditionData["VAL" + val] = P_dicIndividual[MA0001_StructureId];
        delete P_dicIndividual[MA0001_StructureId];

        // 処理終了
        return;
    }

    //ajax通信で個別工場表示フラグを取得
    ajaxCommon("GetStructureIdList", appPath, FormList.No, ConductId_MA0001, false, null, eventFunc);
}

/*
 * 保全実績評価から遷移してきた場合、詳細検索条件を設定する
 */
function setDetailSearchConditionFromMP0001() {
    //保全実績評価から指定された検索条件を取得
    var conditionData = P_dicIndividual[MA0001_DetailSearchCondition];

    if (conditionData) {
        //詳細条件に値設定
        setDetailSearchCondition(conditionData);

        //読込件数を「すべて」に更新し、非活性化
        var define = $.grep(P_listDefines, function (define, idx) {
            return (define.CTRLTYPE == ctrlTypeDef.IchiranPtn3 && define.CTRLID == FormList.List.Id);
        });

        // 読込件数コンボの取得
        var select = getSelectCntCombo(FormList.List.Id);
        // 詳細検索条件指定有りのため、読込件数を「すべて」に更新
        var selectCnt = selectCntMaxDef.All;
        $(select).find('option:selected').prop('selected', false);
        $(select).find('option[exparam1="' + selectCnt + '"]').prop('selected', true);
        define[0].VAL5 = selectCnt;

        // 読込件数コンボとボタンの活性状態を設定
        setSelectCntControlStatus(define[0].CTRLID, true);
    }
}

/**
 * //【オーバーライド用関数】ツリービュー読込後処理
 * @param {any} selector ツリービューのセレクタ
 * @param {any} isTreeMenu 左側メニューの場合true
 * @param {any} grpId 構成グループID
 * @param {any} isTreeMenu 指定階層を展開した状態で表示するツリーの場合true、選択階層までを展開する場合はfalse
 */
function afterLoadedTreeView(selector, isTreeMenu, grpId, isOpenNode) {
    if (!isTreeMenu || grpId != structureGroupDef.Job) {
        return;
    }
    // 保全実績評価から指定された検索条件
    var conditionData = P_dicIndividual[MA0001_DetailSearchCondition];
    if (!conditionData || conditionData.length <= 0) {
        return;
    }
    // 検索条件(職種)
    var jobId = conditionData["VAL" + FormList.List.Job];
    if (!jobId) {
        return;
    }
    //保存している職種の値をクリアする（場所階層と職種の選択変更時に、保存している値で上書きしないように）
    conditionData["VAL" + FormList.List.Job] = "";

    // ノードのチェックをすべて解除し閉じる
    $(selector).jstree(true).uncheck_all();
    //$(selector).jstree(true).close_all();
    if (jobId != JobAll) {
        //職種IDリスト
        var selectedIdList = jobId.split("|").map(Number);
        //職種ツリーの選択
        setSelectedDataToTreeView(selector, selectedIdList, true, isOpenNode);
    } else {
        //セッションストレージに保存している職種の選択値をクリア
        setSaveDataToSessionStorage([], sessionStorageCode.TreeViewSelected, structureGroupDef.Job);
    }
}

/**
 * 【オーバーライド用関数】場所階層、職種機種検索前の個別実装
 * @param {any} appPath ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 * @param {any} btn 対象ボタン
 * @param {any} conductId 機能ID
 * @param {any} pgmId プログラムID
 * @param {any} formNo 画面番号
 * @param {any} conductPtn 処理パターン
 */
function beforeTreeViewSearchBtnProcess(appPath, btn, conductId, pgmId, formNo, conductPtn) {
    //保全実績評価から遷移時に保存した検索条件を破棄する
    delete P_dicIndividual[MA0001_DetailSearchCondition];
}

/**
 * 【オーバーライド用関数】詳細検索前の個別実装
 * @param {any} appPath ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 * @param {any} btn 対象ボタン
 * @param {any} conductId 機能ID
 * @param {any} pgmId プログラムID
 * @param {any} formNo 画面番号
 * @param {any} conductPtn 処理パターン
 */
function beforeDetailSearchBtnProcess(appPath, btn, conductId, pgmId, formNo, conductPtn) {
    //保全実績評価から遷移時に保存した検索条件を破棄する
    delete P_dicIndividual[MA0001_DetailSearchCondition];
}

/**
 * 件名別長期計画・機器別長期計画で白丸「○」がクリックされて遷移してきた場合、新規登録画面(点検情報)を表示する
 * */
function dispFormEditFromScheduleLink() {

    // 件名別長期計画・機器別長期計画の白丸「○」で遷移してきた際のキーがグローバルリストに存在するか判定
    if (KeyNameFromScheduleLink in P_dicIndividual == false) {
        //存在しなければ何もしない
        return;
    }

    var button;

    // 「点検情報登録」ボタン、「新規」ボタンのうち、表示されている方をクリック
    if (!isUnAvailableButton(FormList.ButtonId.NewInspection)) {
        // 「点検情報登録」ボタン
        button = getButtonCtrl(FormList.ButtonId.NewInspection);
    }
    else {
        // 「新規」ボタン
        button = getButtonCtrl(FormList.ButtonId.New);
    }

    // ボタンをクリック
    $(button).click();
}


/**
 * 対象機器一覧 機器使用期間の入力可能なレコードを制限する
 * @param {any} rows
 */
function unEnableUseDays(rows) {
    // 機器使用期間の入力可能レコードを制限する
    // 同一機器に対する先頭行のレコードのみ入力可能とする
    var machineIdList = []; // 機番IDリスト
    var machineId;          // 機番ID
    var useDaysEle;
    $.each(rows, function (idx, row) {

        // レコードの機番IDを取得
        //machineId = $(row._row.element).find("div[tabulator-field=VAL" + FormRegist.MachineList.MachineId + "]")[0].innerText;
        machineId = row.getData()["VAL" + FormRegist.MachineList.MachineId];

        // レコードの機器使用期間のテキストボックス要素
        useDaysEle = $(row._row.element).find("div[tabulator-field=VAL" + FormRegist.MachineList.UseDays + "]").find("input")[0];

        // レコードの機番IDが機番IDリストに格納されているか判定
        if (machineIdList.indexOf(machineId) > -1) {
            // 含まれている場合 = 機器どとの先頭レコードではない(自身のレコードより上に同一の機番IDのレコードが存在する)
            // 自身のレコードの機器使用期間の値をクリアする
            useDaysEle.value = "";

            // 自身のレコードの機器使用期間を入力不可能(非活性)にする
            changeInputControl(useDaysEle, false);
        }
        else {
            // 含まれていない場合 = 機器ごとの先頭レコード
            // 機番IDリストにレコードの機番IDを格納する
            machineIdList.push(machineId);

            // 自身のレコードの機器使用期間を入力可能(活性)にする
            changeInputControl(useDaysEle, true);
        }
    });
}