/* ========================================================================
 *  機能名　    ：   【MC0001】機器台帳
 * ======================================================================== */

/**
 * 自身の相対パスを取得
 */
function getPath() {
    var root;
    var scripts = document.getElementsByTagName("script");
    var i = scripts.length;
    while (i--) {
        var match = scripts[i].src.match(/(^|.*\/)MC0001\.js$/);
        if (match) {
            root = match[1];
            break;
        }
    }
    return root;
}
document.write("<script src=\"" + getPath() + "/tmqcommon.js\"></script>");
document.write("<script src=\"" + getPath() + "/SP0001.js\"></script>");
document.write("<script src=\"" + getPath() + "/DM0002.js\"></script>");
document.write("<script src=\"" + getPath() + "/RM0001.js\"></script>");
// 機能ID
const ConductId_MC0001 = "MC0001";

// 一覧画面
const SearchList = {
    // 画面No
    No: 0,
    List: {
        Id: "BODY_020_00_LST_0",
    },
    HiddenList: {
        Id: "BODY_030_00_LST_0",
        ColumnNo: {
            HiddenFlg: 1
        }
    },
    ButtonId: {
        New: "New",
        Output: "Output",
        HistoryManagementList: "HistoryManagementList"
    },
    // フォーカスを設定するコントロールID
    ForcusId: "New",
    ForcusId2: "Output",
    Filter: { Id: "BODY_010_00_LST_0", Input: 1 } // 一覧上部のフィルタ
};

// 詳細画面
const MachineDatail = {
    // 画面No
    No: 1,
    HiddenList: {
        Id: "BODY_007_00_LST_1",
        ColumnNo: {
            HiddenFlg: 1
        },
    },
    MachineDatail10: {
        Id: "BODY_010_00_LST_1",
    },
    MachineDatail20: {
        Id: "BODY_020_00_LST_1",
        ColumnNo: {
            MachineNo: 1,
            MachineName: 2,
            Job: 3,
            EquipLevel: 4,
            setLoc: 5,
            SetCnt: 6,
            SetDate: 7,
            InpLank: 8,
            Mante: 9,
            MachineId: 10,
            EquipmentId: 12,
            TabNo: 13
        },
    },
    MachineDatail30: {
        Id: "BODY_030_00_LST_1",
    },
    MachineDatail40: {
        Id: "BODY_040_00_LST_1",
    },
    MachineDatail50: {
        Id: "BODY_050_00_LST_1",
        ColumnNo: {
            UseKbn: 1,
            MaintLank: 10,
            EquipmentId: 11
        },
    },
    MachineDatail60: {
        Id: "BODY_060_00_LST_1",
    },
    MachineDatail70: {
        Id: "BODY_070_00_LST_1",
        ColumnNo: {
            Text: 1,
            Num: 2,
            NumMinMax: 3,
            Select: 4,
        },
    },
    MachineDatail75: {
        Id: "BODY_075_00_LST_1",
        ColumnNo: {
            RelationId: 1,
            SpecId: 2,
            Order: 3,
            SpecType: 4,
            SpecUnitType: 5,
            SpecUnit: 6,
            DecimalPlace: 7,
            Name: 8,
            SpecTypeKbn: 9,
            UnitName: 10,
            Text: 11,
            SelectStructure: 12,
            Num: 13,
            NumMin: 14,
            NumMax: 15
        },
    },
    HideMachineInfo: {
        Id: "BODY_031_00_LST_1",
        MachineId: 1,
        EquipmentId: 2
    },
    ButtonId: {
        KibanAppend: "KibanAppend",
        KikiAppend: "KikiAppend",
        Copy: "Copy",
        Update: "Update",
        Delete: "Delete",
        Back: "Back",
        SpecUpdate: "SpecUpdate",
        HistoryManagementDetail: "HistoryManagementDetail"
    },
    TabNo: {
        LongPlan: 4
    },
    // フォーカスを設定するコントロールID
    ForcusId: "KibanAppend",
};

// 詳細画面 機器別管理基準
const DatailManagementStandard = {
    TabNo: 3,                     // タブ番号
    // 画面No
    No: 1,
    UnDispList: {
        Id: "BODY_080_00_LST_1",
        ColumnNo: {
            MachineId: 1,
        },
    },
    ManagementStandardList: {
        Id: "BODY_090_00_LST_1",
        ColumnNo: {
            EquipLevel: 32,
            MachineNo: 2,
            MachineName: 3,
            ManagementStandardCompId: 19,
            ManagementStandardContId: 20,
            MainteScheduleId: 21,
            MachineId: 23,
        },
        LinkNone: "linkDisable",         // データのNo.リンクを無効にするCSSクラス
        ValueCssStyle: "black"           // データのNo.リンクの文字色スタイル
    },
    Format1List: {
        Id: "BODY_120_00_LST_1",
    },
    ScheduleCondListUnDisp: {
        Id: "BODY_005_00_LST_1",
        ColumnNo: {
            DispType: 1,
            DispYear: 2,
            DispSpan: 3,
            NonDispId: 4,
        },
    },
    ScheduleCondList: {
        Id: "BODY_140_00_LST_1",
        ColumnNo: {
            DispType: 1,
            DispYear: 2,
            DispSpan: 3,
            NonDispId: 4,
        },
    },
    ScheduleList: {
        Id: "BODY_160_00_LST_1",
        ButtonId: {
            ReSearch: "ReSearch" // 再表示ボタン
        },
        ColumnNo:
        {
            CycleYear: 11,
            CycleMonth: 12,
            CycleDay: 13,
            CycleDisp: 14,
            StartDate: 15,
            ScheduleMante: 16,
            CycleYearLabel: 17,
            CycleMonthLabel: 18,
            CycleDayLabel: 19,
            CycleDispLabel: 20,
            StartDateLabel: 21,
            ScheduleManteLabel: 22
        }
    },
    ScheduleLankList: {
        Id: "BODY_170_00_LST_1",
        ColumnNo: {
            Level: 1,
            MachineNo: 2,
            MachineName: 3,
            Importance: 4,
            MainteLank: 5,
            Componet: 6,
            PostImp: 7,
            Method: 8,
            Kbn: 9,
            Content: 10,
            CycleYear: 11,
            CycleMonth: 12,
            CycleDay: 13,
            CycleDisp: 14,
            StartDate: 15,
            ScheduleMante: 16,
            CycleYearLabel: 17,
            CycleMonthLabel: 18,
            CycleDayLabel: 19,
            CycleDispLabel: 20,
            StartDateLabel: 21,
            ScheduleManteLabel: 22,
            ComponentId: 23,
            ContentId: 24,
            SchedulleId: 25,
            UpdDate: 26,
            MachineId: 27,
            KeyId: 28,
            GroupKey: 29,
            MainteLankId: 32
        },
    },
    ButtonId: {
        ManagementStandardList: "ManagementStandardList",
        Format1List: "Format1List",
        ScheduleList: "ScheduleList",
        ManagementStandardAddFile: "ManagementStandardAddFile",
        DeleteManagementStandard: "DeleteManagementStandard",
        RegistSchedule: "RegistSchedule",
        RegistLankSchedule: "RegistLankSchedule",
        RegistOrder: "RegistOrder"
    },
};

// 詳細画面 長期計画
const DatailLongPlan = {
    // 画面No
    No: 1,
    ScheduleCondList: {
        Id: "BODY_190_00_LST_1",
        ColumnNo: {
            DispType: 1,
            DispYear: 2,
            DispSpan: 3,
            NonDispId: 4,
        },
    },
    ScheduleCondListUnDisp: {
        Id: "BODY_006_00_LST_1",
        ColumnNo: {
            DispType: 1,
            DispYear: 2,
            DispSpan: 3,
            NonDispId: 4,
        },
    },
    LongPlanList: {
        Id: "BODY_210_00_LST_1",
        ColumnNo: {
            LongPlanId: 8
        },
    }
}

// 詳細画面 保全活動
const DatailMaintainanceActivity = {
    // 画面No
    No: 1,
    MaintainanceActivityList: {
        Id: "BODY_220_00_LST_1",
        ColumnNo: {
            NoLink: 1,
            Subject: 2,
            SummaryId: 12,
        },
    }
}

// 詳細画面 使用部品
const DetailUseParts = {
    // 画面No
    No: 1,
    UsePartsList: {
        Id: "BODY_230_00_LST_1",
        ColumnNo: {
            PartsName: 1,
            Dimensions: 2,
            Maker: 3,
            UseCount: 4,
            Zaiko: 5,
            UsePartsId: 6,
            MachineId: 8
        }
    },
    ButtonId: {
        DeleteUseParts: "DeleteUseParts"        // 使用部品削除ボタン
    }
}

// 詳細画面　構成機器
const DatailConstitution = {
    TabNo: 7,                           // タブ番号
    HideCondition: {
        Id: "BODY_250_00_LST_1",        // 非表示一覧
        ListId: "BODY_250_00_LST_1_1",  // ID
        EquipmentLevelNo: 1,            // 機器レベル
        FlgList: 2,                     // 一覧フラグ
        MachineId: 3                    // 機番ID
    },
    ParentList: {
        Id: "BODY_260_00_LST_1",        // 親子構成一覧
        FlgChild: 7                     // 子フラグ(自分自身の子かどうか)
    },
    LoopList: {
        Id: "BODY_270_00_LST_1",        // ループ構成一覧
        FlgChild: 7                     // 子フラグ(自分自身の子かどうか)
    },
    ButtonId: {
        Parent: "Parent",               // 一覧表示切替(親子構成一覧表示)
        Loop: "Loop",                   // 一覧表示切替(ループ構成一覧表示)
        DeleteParent: "DeleteParent",   // 構成削除ボタン(親子構成一覧)
        DeleteLoop: "DeleteLoop"        // 構成削除ボタン(ループ構成一覧)
    },
    FlgList: {                          // 行追加アイコンがクリックされた一覧
        Parent: 1,                      // 親子構成一覧
        Loop: 2                         // ループ構成構成一覧
    },
    FlgChildTargetNo: "0"               // 一覧のチェックボックスを非表示にするフラグ
}

// 詳細画面 MP情報
const DetailMpInfo = {
    HideId: "BODY_280_00_LST_1",        // 非表示一覧
    MpInfoList: {
        Id: "BODY_290_00_LST_1",        // MP情報一覧
        MpInfoId: 3                     // MP情報ID
    },
    ButtonId: {
        AddFileMpInfo: "AddFileMpInfo", // ファイル添付ボタン
        DeleteMpInfo: "DeleteMpInfo",   // 削除ボタン
        Regit: "Regist"                 // 登録ボタン
    }
}

// 詳細編集画面
const MachineEditDetail = {
    // 画面No
    No: 2,
    MachineDatail00: {
        Id: "BODY_000_00_LST_2",
    },
    MachineDatail10: {
        Id: "BODY_010_00_LST_2",
    },
    MachineDatail20: {
        Id: "BODY_020_00_LST_2",
    },
    MachineDatail30: {
        Id: "BODY_030_00_LST_2",
    },
    MachineDatail40: {
        Id: "BODY_040_00_LST_2",
    },
    MachineDatail50: {
        Id: "BODY_050_00_LST_2",
    },
    MachineDatail60: {
        Id: "BODY_060_00_LST_2",
    },
    ButtonId: {
        Regist: "Regist",
        Back: "Back"
    },
    // フォーカスを設定するコントロールID
    ForcusId: "Regist",
};

// 機器選択画面
const SelectMachine = {
    No: 3, // 画面番号
    HideCondition: {
        Id: "COND_000_00_LST_3", // 検索条件エリア(非表示)
        EquipmentLevelNo: 1,     // 機器レベル番号
        FlgList: 2,              // 一覧フラグ(行追加アイコンがクリックされた一覧)
        MachineId: 3             // 機番ID
    },
    SearchCondition10: {
        Id: "COND_010_00_LST_3"  // 検索条件エリア(地区～設備)
    },
    SearchCondition20: {
        Id: "COND_020_00_LST_3", // 検索条件エリア(職種～機器名称)
        Equipment: 5             // 機器レベル
    },
    SearchCondition30: {
        Id: "COND_030_00_LST_3", // 検索条件エリア(使用区分～製造年月)
        ScheduleType: 9,         // スケジュール管理
        MainteKind: 10,          // 点検種別毎管理
        InspectionSite: 11,      // 保全部位
        InspectionContent: 12    // 保全項目
    },
    SelectMachineList: {
        Id: "BODY_050_00_LST_3", // 検索結果一覧
        SelectBtn: 1             // 選択ボタン
    }
}


// 機種別仕様編集画面
const SpecEdit = {
    // 画面No
    No: 4,
    SpecDetail00: {
        Id: "BODY_000_00_LST_4",
        ColumnNo: {
            Text: 1,
            Num: 2,
            NumMinMax: 3,
            Select: 4,
        }
    },
    SpecDetail20: {
        Id: "BODY_020_00_LST_4",
        ColumnNo: {
            RelationId: 1,
            SpecId: 2,
            Order: 3,
            SpecType: 4,
            SpecUnitType: 5,
            SpecUnit: 6,
            DecimalPlace: 7,
            Name: 8,
            SpecTypeKbn: 9,
            UnitName: 10,
            Text: 11,
            SelectStructure: 12,
            Num: 13,
            NumMin: 14,
            NumMax: 15
        },
    },
    ButtonId: {
        Regist: "Regist",
        Back: "Back"
    },
    // フォーカスを設定するコントロールID
    Regist: "Regist",
};

// 機器レベルの番号
const EquipmentLevelNo = {
    Parent: 1,    // 親機
    Machine: 2,   // 機器
    Accessory: 3, // 付属品
    Loop: 4       // ループ
};

// 単票のボタン
const SingleVote = {
    Regist: "Regist", // 登録
    Back: "Back"      // 戻る
}

//機種別仕様　入力形式区分
const SpecType = {
    Text: "1", //テキストボックス
    Num: "2", //数値
    NumMinMax: "3", //数値(範囲)
    Select: "4", //コンボボックス
}

/*==94:初期化処理==*/
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
    //var btn = $(articleForm).find('input:button[name="Search"]');     **ﾎﾞﾀﾝ

    // 機能IDが「帳票出力」の場合
    if (getConductId() == RM00001_ConductId) {
        return RM0001_initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data);
    }

    // 共通-文書管理詳細画面の初期化処理
    DM0002_initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data);

    // 機能IDが機器台帳以外の場合は何もしない
    if (conductId != ConductId_MC0001) {
        return;
    }

    if (formNo == SearchList.No) {
        //一覧画面

        //フォーカス設定
        var btn = isUnAvailableButton(SearchList.ForcusId);
        if (btn) {
            setFocusButton(SearchList.ForcusId2);
        } else {
            setFocusButton(SearchList.ForcusId);
        }

        // 変更管理ボタンの表示制御
        var hideBtns = [SearchList.ButtonId.New]; //新規ボタンを非表示にする
        var flagValue = getHistoryManagementFlgValue(true); // フラグの値
        if (flagValue == HistoryManagementDisplayFlag.Dual) {
            // 変更管理する工場としない工場が混在するなら、新規ボタンは非表示にしない
            hideBtns = [];
        }
        // 空のリストをセット
        var hideLists = [];
        setHistoryManagementCtrlDisplay(getIsHisotyManagement(true), SearchList.ButtonId.HistoryManagementList, hideBtns, hideLists);


    } else if (formNo == MachineDatail.No) {
        //詳細画面

        //フォーカス設定
        setFocusButton(MachineDatail.ForcusId);

        // 表示単位に応じてスケジュール表示年度orスケジュール表示期間の表示切替
        var typeVal = getValue(DatailManagementStandard.ScheduleCondList.Id, DatailManagementStandard.ScheduleCondList.ColumnNo.DispType, 1, CtrlFlag.Combo);
        var typeYear = getValue(DatailManagementStandard.ScheduleCondList.Id, DatailManagementStandard.ScheduleCondList.ColumnNo.NonDispId, 1, CtrlFlag.Label);
        if (typeVal == typeYear) {
            changeColumnDisplay(DatailManagementStandard.ScheduleCondList.Id, DatailManagementStandard.ScheduleCondList.ColumnNo.DispYear, true);
            changeColumnDisplay(DatailManagementStandard.ScheduleCondList.Id, DatailManagementStandard.ScheduleCondList.ColumnNo.DispSpan, false);
        } else {
            changeColumnDisplay(DatailManagementStandard.ScheduleCondList.Id, DatailManagementStandard.ScheduleCondList.ColumnNo.DispYear, false);
            changeColumnDisplay(DatailManagementStandard.ScheduleCondList.Id, DatailManagementStandard.ScheduleCondList.ColumnNo.DispSpan, true);
        }        // 表示単位に応じてスケジュール表示年度orスケジュール表示期間の表示切替
        var typeVal = getValue(DatailLongPlan.ScheduleCondList.Id, DatailLongPlan.ScheduleCondList.ColumnNo.DispType, 1, CtrlFlag.Combo);
        var typeYear = getValue(DatailLongPlan.ScheduleCondList.Id, DatailLongPlan.ScheduleCondList.ColumnNo.NonDispId, 1, CtrlFlag.Label);
        if (typeVal == typeYear) {
            changeColumnDisplay(DatailLongPlan.ScheduleCondList.Id, DatailLongPlan.ScheduleCondList.ColumnNo.DispYear, true);
            changeColumnDisplay(DatailLongPlan.ScheduleCondList.Id, DatailLongPlan.ScheduleCondList.ColumnNo.DispSpan, false);
        } else {
            changeColumnDisplay(DatailLongPlan.ScheduleCondList.Id, DatailLongPlan.ScheduleCondList.ColumnNo.DispYear, false);
            changeColumnDisplay(DatailLongPlan.ScheduleCondList.Id, DatailLongPlan.ScheduleCondList.ColumnNo.DispSpan, true);
        }
        // 機器別管理基準データが0件の際に初期表示一覧を設定
        var pageRowCount = P_listData["#" + DatailManagementStandard.ManagementStandardList.Id + getAddFormNo()].getDataCount("display");
        if (pageRowCount == 0) {
            // 保全項目一覧をクリック
            var button = getButtonCtrl(DatailManagementStandard.ButtonId.ManagementStandardList);
            $(button).click();
        }

        // 構成機器タブ
        initTabConstitution();

        //機種別仕様タブ レイアウトテンプレート項目を非表示に設定
        changeColumnDisplay(MachineDatail.MachineDatail70.Id, MachineDatail.MachineDatail70.ColumnNo.Text, false);
        changeColumnDisplay(MachineDatail.MachineDatail70.Id, MachineDatail.MachineDatail70.ColumnNo.Num, false);
        changeColumnDisplay(MachineDatail.MachineDatail70.Id, MachineDatail.MachineDatail70.ColumnNo.NumMinMax, false);
        changeColumnDisplay(MachineDatail.MachineDatail70.Id, MachineDatail.MachineDatail70.ColumnNo.Select, false);
        //機種別仕様 隠し一覧を非表示に設定
        changeListDisplay(MachineDatail.MachineDatail75.Id, false);


        //他機能から遷移してきた場合、指定されたタブを表示する
        var tabNo = getValue(MachineDatail.MachineDatail20.Id, MachineDatail.MachineDatail20.ColumnNo.TabNo, 1, CtrlFlag.Label);
        switch (Number(tabNo)) {
            case MachineDatail.TabNo.LongPlan: //タブ
                //タブを選択
                selectTab(tabNo);
                break;
        }

        // 変更管理ボタンの表示制御
        var hideBtns = [MachineDatail.ButtonId.Copy, MachineDatail.ButtonId.Update, MachineDatail.ButtonId.Delete];
        var hideLists = [];
        setHistoryManagementCtrlDisplay(getIsHisotyManagement(false), MachineDatail.ButtonId.HistoryManagementDetail, hideBtns, hideLists);

    } else if (formNo == MachineEditDetail.No) {
        //詳細画面

        //フォーカス設定
        setFocusButton(MachineEditDetail.ForcusId);
    }
    else if (formNo == SelectMachine.No) {
        // 機器選択画面

        // 構成機器タブの非表示一覧に格納されている値を取得
        var equipmentLevelNo = getValueByOtherForm(MachineDatail.No, DatailConstitution.HideCondition.Id, DatailConstitution.HideCondition.EquipmentLevelNo, 1, CtrlFlag.Label); // 機器レベル番号
        var flgList = getValueByOtherForm(MachineDatail.No, DatailConstitution.HideCondition.Id, DatailConstitution.HideCondition.FlgList, 1, CtrlFlag.Label);                   // 一覧フラグ
        var machineId = getValueByOtherForm(MachineDatail.No, DatailConstitution.HideCondition.Id, DatailConstitution.HideCondition.MachineId, 1, CtrlFlag.Label);               // 機番ID

        // 取得した値を検索条件の非表示一覧に設定
        setValue(SelectMachine.HideCondition.Id, SelectMachine.HideCondition.EquipmentLevelNo, 1, CtrlFlag.Label, equipmentLevelNo, false, false); // 機器レベル番号
        setValue(SelectMachine.HideCondition.Id, SelectMachine.HideCondition.FlgList, 1, CtrlFlag.Label, flgList, false, false);                   // 一覧フラグ
        setValue(SelectMachine.HideCondition.Id, SelectMachine.HideCondition.MachineId, 1, CtrlFlag.Label, machineId, false, false);                // 機番ID

        // 検索条件エリアの機器レベルコンボボックスを非活性にする
        disableEquipmentCmb();

        // 検索条件エリアの項目を非表示にする
        // スケジュール管理
        var hideItem = $("#" + SelectMachine.SearchCondition30.Id + getAddFormNo()).find("*[data-name='VAL" + SelectMachine.SearchCondition30.ScheduleType + "']");
        hideItem.hide();
        // 保全部位
        hideItem = $("#" + SelectMachine.SearchCondition30.Id + getAddFormNo()).find("*[data-name='VAL" + SelectMachine.SearchCondition30.InspectionSite + "']");
        hideItem.hide();
        //保全項目
        hideItem = $("#" + SelectMachine.SearchCondition30.Id + getAddFormNo()).find("*[data-name='VAL" + SelectMachine.SearchCondition30.InspectionContent + "']");
        hideItem.hide();
        //点検種別毎管理
        hideItem = $("#" + SelectMachine.SearchCondition30.Id + getAddFormNo()).find("*[data-name='VAL" + SelectMachine.SearchCondition30.MainteKind + "']");
        hideItem.hide();
    }
    else if (formNo == SpecEdit.No) {

        //フォーカス設定
        setFocusButton(SpecEdit.Regist);

        //機種別仕様 レイアウトテンプレート項目を非表示に設定
        changeColumnDisplay(SpecEdit.SpecDetail00.Id, SpecEdit.SpecDetail00.ColumnNo.Text, false);
        changeColumnDisplay(SpecEdit.SpecDetail00.Id, SpecEdit.SpecDetail00.ColumnNo.Num, false);
        changeColumnDisplay(SpecEdit.SpecDetail00.Id, SpecEdit.SpecDetail00.ColumnNo.NumMinMax, false);
        changeColumnDisplay(SpecEdit.SpecDetail00.Id, SpecEdit.SpecDetail00.ColumnNo.Select, false);
        //機種別仕様 隠し一覧を非表示に設定
        changeListDisplay(SpecEdit.SpecDetail20.Id, false);


        //$("#BODY_000_00_LST_4_4DispROWNO1 tbody tr:nth-child(3)") // 
        //    .clone(true)                     // 指定した行のHTML要素を複製する
        //    .appendTo("#BODY_000_00_LST_4_4DispROWNO1 tbody");   // 複製した要素をtbodyに追加する
        //$("#BODY_000_00_LST_4_4DispROWNO1 tbody tr:last-child input").val("aaaaa");
        //$("#BODY_000_00_LST_4_4DispROWNO1 tbody tr:last-child input").addClass("valid");
        //var aaa = $("#BODY_000_00_LST_4_4DispROWNO1 tbody tr:last-child input");
        //setAttrByNativeJs(aaa, "id", "BODY_000_00_LST_4_4VAL" + String(InputRowNo));

        ////選択中画面NOｴﾘｱ
        //P_Article = $('article[name="main_area"][data-formno="' + formNo + '"]');
        //if (P_Article.length > 1) {
        //    //※複数件見つかった場合、ﾍﾞｰｽﾌｫｰﾑを除外
        //    P_Article = $('article:not([class="base_form"])[name="main_area"][data-formno="' + formNo + '"]');
        //}
        //initValidator("#BODY_000_00_LST_4_4");


    }
}

// 変更管理のコントロールの表示フラグ
const HistoryManagementDisplayFlag = {
    // 変更管理の表示をしない
    NoHisotry: "0",
    // 変更管理の表示のみ行う
    History: "1",
    // 両方表示する(一覧のみ)
    Dual: "2"
};

/**
 * 変更管理の要素の表示フラグの値を取得
 * @param {any} isList 一覧画面ならTrue、参照画面ならFalse
 */
function getHistoryManagementFlgValue(isList) {
    var flagValue;
    if (isList) {
        flagValue = getValue(SearchList.HiddenList.Id, SearchList.HiddenList.ColumnNo.HiddenFlg, 0, CtrlFlag.Label);
    } else {
        flagValue = getValue(MachineDatail.HiddenList.Id, MachineDatail.HiddenList.ColumnNo.HiddenFlg, 0, CtrlFlag.Label);
    }
    return flagValue;
}
/**
 * 変更管理の要素を表示するか判定
 * @param {any} isList 一覧画面ならTrue、参照画面ならFalse
 * @returns  {bool} 変更管理を表示するならTrue
 */
function getIsHisotyManagement(isList) {
    var flagValue = getHistoryManagementFlgValue(isList);
    return flagValue == HistoryManagementDisplayFlag.History || flagValue == HistoryManagementDisplayFlag.Dual;
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

    // 一覧フィルタ処理実施
    if (executeListFilter(transTarget, SearchList.List.Id, SearchList.Filter.Id, SearchList.Filter.Input)) {
        return [false, conditionDataList];
    }

    var conditionDataList = [];

    if (isScheduleLink(btn_ctrlId)) {
        // スケジュールのリンクの場合
        conditionDataList = getParamToMA0001BySchedule(transTarget);
        return [true, conditionDataList];
    }

    // 各画面ごとに遷移前処理を実施
    if (formNo == SearchList.No) {
        // 一覧画面
        if (btn_ctrlId == SearchList.ButtonId.Output) {
            // 出力ボタン押下時
            // 一覧にチェックされた行が存在しない場合、遷移をキャンセル
            if (!isCheckedList(SearchList.List.Id)) {
                return [false, conditionDataList]
            }

            // 一覧情報をセット
            //const ctrlIdList = [SearchList.List.Id];
            //conditionDataList = getListDataByCtrlIdList(ctrlIdList, formNo, 0)

        } else if (btn_ctrlId == SearchList.ButtonId.New) {
            // 新規

        } else if (transTarget == "1") {
            // 子画面遷移
            var condition = getSavedDataFromLocalStorage(localStorageCode.ScheduleCond, ConductId_MC0001, 1, DatailManagementStandard.ScheduleCondList.Id);
            if (condition != null && condition.length != 0) {
                setSearchCondition(ConductId_MC0001, MachineDatail.No, condition);
                conditionDataList.push(condition)
            }
            var condition2 = getSavedDataFromLocalStorage(localStorageCode.ScheduleCond, ConductId_MC0001, 1, DatailLongPlan.ScheduleCondList.Id);
            if (condition2 != null && condition2.length != 0) {
                setSearchCondition(ConductId_MC0001, MachineDatail.No, condition2);
                conditionDataList.push(condition2)
            }
        }
    } else if (formNo == MachineDatail.No) {
        // 参照画面
        if (btn_ctrlId == MachineDatail.ButtonId.KibanAppend) {
            // 機番添付
            conditionDataList.push(getParamToDM0002(AttachmentStructureGroupID.Machine, getValue(MachineDatail.MachineDatail20.Id, MachineDatail.MachineDatail20.ColumnNo.MachineId, 0, CtrlFlag.Label)));
        } else if (btn_ctrlId == MachineDatail.ButtonId.KikiAppend) {
            // 機器添付
            conditionDataList.push(getParamToDM0002(AttachmentStructureGroupID.Equipment, getValue(MachineDatail.MachineDatail20.Id, MachineDatail.MachineDatail20.ColumnNo.EquipmentId, 0, CtrlFlag.Label)));
        } else if (btn_ctrlId == MachineDatail.ButtonId.Copy) {
            // 複写
            // 機番ID情報をセット
            const ctrlIdList = [MachineDatail.MachineDatail20.Id];
            conditionDataList = getListDataByCtrlIdList(ctrlIdList, formNo, 0);
        } else if (btn_ctrlId == MachineDatail.ButtonId.Update || btn_ctrlId == MachineDatail.ButtonId.SpecUpdate) {
            // 修正
            // 機番ID情報をセット
            const ctrlIdList = [MachineDatail.MachineDatail20.Id];
            conditionDataList = getListDataByCtrlIdList(ctrlIdList, formNo, 0);
        } else if (btn_ctrlId == MachineDatail.ButtonId.Delete) {
            // 削除
            // 機番ID情報をセット
            const ctrlIdList = [MachineDatail.MachineDatail20.Id];
            conditionDataList = getListDataByCtrlIdList(ctrlIdList, formNo, 0);
        } else if (btn_ctrlId == MachineDatail.ButtonId.Back) {
            // 戻る
            // 機番ID情報をセット
            const ctrlIdList = [MachineDatail.MachineDatail20.Id];
            conditionDataList = getListDataByCtrlIdList(ctrlIdList, formNo, 0);
        } else if (btn_ctrlId == MachineEditDetail.ButtonId.Regist) {
            // 登録
            // 機番ID情報をセット
            const ctrlIdList = [MachineDatail.MachineDatail20.Id];
            conditionDataList = getListDataByCtrlIdList(ctrlIdList, formNo, 0);
        } else if (btn_ctrlId == DatailManagementStandard.ButtonId.ManagementStandardAddFile) {
            // 保全項目一覧 ファイル添付
            // 一覧にチェックされた行が存在しない場合、遷移をキャンセル
            if (!isCheckedList(DatailManagementStandard.ManagementStandardList.Id)) {
                return [false, conditionDataList]
            }

            // 一覧にチェックされた行が複数存在する場合、遷移をキャンセル
            if (!isMultipleCheckedList(DatailManagementStandard.ManagementStandardList.Id, P_ComMsgTranslated[141280001])) { // 複数のデータが選択されています。
                return [false, conditionDataList]
            }

            // 保全項目一覧より選択チェックボックスが選択されているデータの行番号を取得
            var selectedRowNo = getRowNoByList(DatailManagementStandard.ManagementStandardList.Id);

            // 共通処理
            conditionDataList.push(getParamToDM0002(AttachmentStructureGroupID.Content, getValueDataRow(DatailManagementStandard.ManagementStandardList.Id, DatailManagementStandard.ManagementStandardList.ColumnNo.ManagementStandardContId, selectedRowNo, CtrlFlag.Label)));


        } else if (btn_ctrlId == null && ctrlId == DatailLongPlan.LongPlanList.Id) {
            // 長期計画一覧Noリンククリック
            var longplanId = getSiblingsValue(element, DatailLongPlan.LongPlanList.ColumnNo.LongPlanId, CtrlFlag.Label);
            conditionDataList = getParamToLN0001(longplanId);

            // 参照画面への遷移の場合
            //conditionDataList = getListDataByCtrlIdList([DatailMaintainanceActivity.MaintainanceActivityList.Id], DatailMaintainanceActivity.No, 0);

        } else if (btn_ctrlId == null && ctrlId == DatailMaintainanceActivity.MaintainanceActivityList.Id) {
            // 保全活動一覧Noリンククリック
            var sumId = getSiblingsValue(element, DatailMaintainanceActivity.MaintainanceActivityList.ColumnNo.SummaryId, CtrlFlag.Label);
            conditionDataList = getParamToMA0001(sumId, 1);

            //conditionDataList = getListDataByCtrlIdList([DatailLongPlan.LongPlanList.Id], DatailLongPlan.No, 0);
        } else if (btn_ctrlId == DetailMpInfo.ButtonId.AddFileMpInfo) {
            // MP情報 ファイル添付ボタン押下時
            // 一覧にチェックされた行が存在しない場合、遷移をキャンセル
            if (!isCheckedList(DetailMpInfo.MpInfoList.Id)) {
                return [false, conditionDataList]
            }

            // 一覧にチェックされた行が複数存在する場合、遷移をキャンセル
            if (!isMultipleCheckedList(DetailMpInfo.MpInfoList.Id, P_ComMsgTranslated[141280001])) { // 複数のデータが選択されています。
                return [false, conditionDataList]
            }

            // MP情報一覧より選択チェックボックスが選択されているデータの行番号を取得
            var selectedRowNo = getRowNoByList(DetailMpInfo.MpInfoList.Id);

            // 共通処理
            conditionDataList.push(getParamToDM0002(AttachmentStructureGroupID.MpInfo, getValueDataRow(DetailMpInfo.MpInfoList.Id, DetailMpInfo.MpInfoList.MpInfoId, selectedRowNo, CtrlFlag.Label)));
        }
        else if (btn_ctrlId == MachineDatail.ButtonId.HistoryManagementDetail) {
            // 変更管理(非表示の機番IDを取得し条件に設定)
            conditionDataList.push(getParamToHM0001FormDetail(getValue(MachineDatail.MachineDatail20.Id, MachineDatail.MachineDatail20.ColumnNo.MachineId, 1, CtrlFlag.Label, false, false)));
        }
    } else if (formNo == MachineEditDetail.No) {
        // 参照画面
        if (btn_ctrlId == MachineEditDetail.ButtonId.Regist) {
            // 複写
            // 機番ID情報をセット
            const ctrlIdList = [MachineEditDetail.MachineDatail10.Id];
            conditionDataList = getListDataByCtrlIdList(ctrlIdList, formNo, 0);

        } else if (btn_ctrlId == MachineEditDetail.ButtonId.Back) {
            // 戻る


        }
    }

    return [true, conditionDataList];
}

function beforeSearchBtnProcess(appPath, btn, conductIdW, pgmIdW, formNoW, conductPtnW) {

    // 機能IDが「帳票出力」の場合
    if (conductIdW == RM00001_ConductId) {
        return RM0001_beforeSearchBtnProcess(appPath, btn, conductIdW, pgmIdW, formNoW, conductPtnW);
    }
    if (formNoW == 3 || formNoW == 0 || conductIdW == SP0001_ConductId) {
        return;
    }
    // ここでローカルストレージに保存
    // 機器別管理基準スケジューリング一覧
    var conditionDataList = getListDataByCtrlIdList([DatailManagementStandard.ScheduleCondList.Id], MachineDatail.No, 0);
    setSaveDataToLocalStorage(conditionDataList, localStorageCode.ScheduleCond, ConductId_MC0001, 1, DatailManagementStandard.ScheduleCondListUnDisp.Id);
    //setSearchCondition(ConductId_MC0001, MachineDatail.No, conditionDataList);

    //const savedData = getSavedDataFromLocalStorage(localStorageCode.DetailSearch, conductId, formNo, ctrlId);
    //if (savedData == null || savedData.length == 0) { return true; }
    var unit = getValueByOtherForm(MachineDatail.No, DatailManagementStandard.ScheduleCondList.Id, DatailManagementStandard.ScheduleCondList.ColumnNo.DispType, 1, CtrlFlag.Combo);
    setValue(DatailManagementStandard.ScheduleCondListUnDisp.Id, DatailManagementStandard.ScheduleCondListUnDisp.ColumnNo.DispType, 0, CtrlFlag.Label, unit, false, false);
    var year = getValueByOtherForm(MachineDatail.No, DatailManagementStandard.ScheduleCondList.Id, DatailManagementStandard.ScheduleCondList.ColumnNo.DispYear, 1, CtrlFlag.TextBox);
    setValue(DatailManagementStandard.ScheduleCondListUnDisp.Id, DatailManagementStandard.ScheduleCondListUnDisp.ColumnNo.DispYear, 0, CtrlFlag.Label, year, false, false);

    var spanTo = document.getElementById('BODY_140_00_LST_1_1VAL3To').value;
    var span = getValueByOtherForm(MachineDatail.No, DatailManagementStandard.ScheduleCondList.Id, DatailManagementStandard.ScheduleCondList.ColumnNo.DispSpan, 1, CtrlFlag.TextBox);
    span = span + "|" + spanTo;
    setValue(DatailManagementStandard.ScheduleCondListUnDisp.Id, DatailManagementStandard.ScheduleCondListUnDisp.ColumnNo.DispSpan, 0, CtrlFlag.Label, span, false, false);

    var manthId = getValueByOtherForm(MachineDatail.No, DatailManagementStandard.ScheduleCondList.Id, DatailManagementStandard.ScheduleCondList.ColumnNo.NonDispId, 1, CtrlFlag.Label);
    setValue(DatailManagementStandard.ScheduleCondListUnDisp.Id, DatailManagementStandard.ScheduleCondListUnDisp.ColumnNo.NonDispId, 0, CtrlFlag.Label, manthId, false, false);

    // 長期計画スケジュール
    var conditionDataList1 = getListDataByCtrlIdList([DatailLongPlan.ScheduleCondList.Id], MachineDatail.No, 0);
    setSaveDataToLocalStorage(conditionDataList1, localStorageCode.ScheduleCond, ConductId_MC0001, 1, DatailLongPlan.ScheduleCondListUnDisp.Id);
    //setSearchCondition(ConductId_MC0001, MachineDatail.No, conditionDataList);

    //const savedData = getSavedDataFromLocalStorage(localStorageCode.DetailSearch, conductId, formNo, ctrlId);
    //if (savedData == null || savedData.length == 0) { return true; }

    var unit1 = getValueByOtherForm(MachineDatail.No, DatailLongPlan.ScheduleCondList.Id, DatailLongPlan.ScheduleCondList.ColumnNo.DispType, 1, CtrlFlag.Combo);
    setValue(DatailLongPlan.ScheduleCondListUnDisp.Id, DatailLongPlan.ScheduleCondListUnDisp.ColumnNo.DispType, 0, CtrlFlag.Label, unit1, false, false);

    var year1 = getValueByOtherForm(MachineDatail.No, DatailLongPlan.ScheduleCondList.Id, DatailLongPlan.ScheduleCondList.ColumnNo.DispYear, 1, CtrlFlag.TextBox);
    setValue(DatailLongPlan.ScheduleCondListUnDisp.Id, DatailLongPlan.ScheduleCondListUnDisp.ColumnNo.DispYear, 0, CtrlFlag.Label, year1, false, false);

    var spanTo1 = document.getElementById('BODY_190_00_LST_1_1VAL3To').value;
    var span1 = getValueByOtherForm(MachineDatail.No, DatailLongPlan.ScheduleCondList.Id, DatailLongPlan.ScheduleCondList.ColumnNo.DispSpan, 1, CtrlFlag.TextBox);
    span1 = span1 + "|" + spanTo1;
    setValue(DatailLongPlan.ScheduleCondListUnDisp.Id, DatailLongPlan.ScheduleCondListUnDisp.ColumnNo.DispSpan, 0, CtrlFlag.Label, span1, false, false);

    var manthId1 = getValueByOtherForm(MachineDatail.No, DatailLongPlan.ScheduleCondList.Id, DatailLongPlan.ScheduleCondList.ColumnNo.NonDispId, 1, CtrlFlag.Label);
    setValue(DatailLongPlan.ScheduleCondListUnDisp.Id, DatailLongPlan.ScheduleCondListUnDisp.ColumnNo.NonDispId, 0, CtrlFlag.Label, manthId1, false, false);

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

    var formNo = getFormNo();
    if (formNo == MachineEditDetail.No && btnCtrlId == MachineEditDetail.ButtonId.Regist) {
        //新規登録画面から登録後、参照画面に渡すキー情報をセット
        var conditionDataList = getListDataByCtrlIdList([MachineEditDetail.MachineDatail10.Id], MachineEditDetail.No, 0);
        // 一覧から参照へ遷移する場合と同様に、参照画面の検索条件を追加
        setSearchCondition(ConductId_MC0001, MachineDatail.No, conditionDataList);
    }
    return true;
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

    // 詳細画面
    if (formNo == MachineDatail.No) {
        // 単票入力
        if (transPtn == transPtnDef.Edit) {
            if (transTarget == DatailManagementStandard.ManagementStandardList.Id) { // 保全項目一覧
                // 設定なし
                if (rowNo == -1) {
                    // 詳細画面より機器情報を取得してセット
                    // 機器レベル
                    var equipLevel = getValueByOtherForm(MachineDatail.No, MachineDatail.MachineDatail20.Id, MachineDatail.MachineDatail20.ColumnNo.EquipLevel, 1, CtrlFlag.Combo);
                    setValue(DatailManagementStandard.ManagementStandardList.Id, DatailManagementStandard.ManagementStandardList.ColumnNo.EquipLevel, 1, CtrlFlag.Combo, equipLevel, true, false);
                    // 機器番号
                    var machineNo = getValueByOtherForm(MachineDatail.No, MachineDatail.MachineDatail20.Id, MachineDatail.MachineDatail20.ColumnNo.MachineNo, 1, CtrlFlag.TextBox);
                    setValue(DatailManagementStandard.ManagementStandardList.Id, DatailManagementStandard.ManagementStandardList.ColumnNo.MachineNo, 1, CtrlFlag.TextBox, machineNo, true, false);
                    // 機器名称
                    var machineName = getValueByOtherForm(MachineDatail.No, MachineDatail.MachineDatail20.Id, MachineDatail.MachineDatail20.ColumnNo.MachineName, 1, CtrlFlag.TextBox);
                    setValue(DatailManagementStandard.ManagementStandardList.Id, DatailManagementStandard.ManagementStandardList.ColumnNo.MachineName, 1, CtrlFlag.TextBox, machineName, true, false);
                    // 機番ID
                    var machineId = getValueByOtherForm(MachineDatail.No, MachineDatail.MachineDatail20.Id, MachineDatail.MachineDatail20.ColumnNo.MachineId, 1, CtrlFlag.Label);
                    // キー項目に-1
                    setValue(DatailManagementStandard.ManagementStandardList.Id, DatailManagementStandard.ManagementStandardList.ColumnNo.MachineId, 1, CtrlFlag.Label, machineId, true, false);
                    setValue(DatailManagementStandard.ManagementStandardList.Id, DatailManagementStandard.ManagementStandardList.ColumnNo.ManagementStandardCompId, 1, CtrlFlag.Label, "-1", true, false);
                    setValue(DatailManagementStandard.ManagementStandardList.Id, DatailManagementStandard.ManagementStandardList.ColumnNo.ManagementStandardContId, 1, CtrlFlag.Label, "-1", true, false);
                    setValue(DatailManagementStandard.ManagementStandardList.Id, DatailManagementStandard.ManagementStandardList.ColumnNo.MainteScheduleId, 1, CtrlFlag.Label, "-1", true, false);

                    // 非活性
                    var editEquipLevel = getCtrl(DatailManagementStandard.ManagementStandardList.Id, DatailManagementStandard.ManagementStandardList.ColumnNo.EquipLevel, 1, CtrlFlag.Combo, true, false);
                    changeInputControl(editEquipLevel, false);
                    var editMachineNo = getCtrl(DatailManagementStandard.ManagementStandardList.Id, DatailManagementStandard.ManagementStandardList.ColumnNo.MachineNo, 1, CtrlFlag.TextBox, true, false);
                    changeInputControl(editMachineNo, false);
                    var editMachineName = getCtrl(DatailManagementStandard.ManagementStandardList.Id, DatailManagementStandard.ManagementStandardList.ColumnNo.MachineName, 1, CtrlFlag.TextBox, true, false);
                    changeInputControl(editMachineName, false);

                }
            } else if (transTarget == DetailUseParts.UsePartsList.Id) { // 使用部品一覧                
                // 設定なし
                if (rowNo == -1) {
                    // 詳細画面より機番IDを取得してセット// 機番ID
                    var machineId = getValueByOtherForm(MachineDatail.No, MachineDatail.MachineDatail20.Id, MachineDatail.MachineDatail20.ColumnNo.MachineId, 1, CtrlFlag.Label);
                    // キー項目に-1
                    setValue(DetailUseParts.UsePartsList.Id, DetailUseParts.UsePartsList.ColumnNo.MachineId, 1, CtrlFlag.Label, machineId, true, false);
                    setValue(DetailUseParts.UsePartsList.Id, DetailUseParts.UsePartsList.ColumnNo.UsePartsId, 1, CtrlFlag.Label, "-1", true, false);

                    // 初期値セット後の再生成
                    var trs = $(P_Article).find("#" + DetailUseParts.UsePartsList.Id + getAddFormNo() + "_edit");
                    var selects2 = $(trs).find('tbody td select.dynamic');
                    //連動ｺﾝﾎﾞの選択ﾘｽﾄを再生成
                    resetComboBox(appPath, selects2);
                }
            } else if (transTarget == DetailMpInfo.MpInfoList.Id) { // MP情報一覧
                // 登録ボタンにフォーカスを設定する(描画に間に合っていないので少し待つ)     
                setTimeout(function () {
                    setFocusButton(DetailMpInfo.ButtonId.Regit);
                }, 300); //300ミリ秒
            }
        }
    }

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
    // 行削除を行った一覧が特定の一覧の場合、削除処理を行う
    //if (id == FormList.Id + getAddFormNo() || id == FormDetail.Id + getAddFormNo()) {
    //    // 削除ボタン(非表示)をクリック
    //    var button = getButtonCtrl("Delete");
    //    $(button).click();
    //    return false;
    //} else {
    //    // 他の一覧の場合は通常通り行削除を行う
    //    return true;
    //}

    // 共通-文書管理詳細画面の行削除前処理
    var comRtn = DM0002_preDeleteRow(appPath, btn, id, checkes);
    if (!comRtn) {
        // 処理終了の場合は終了
        return false;
    }

    // 保全項目一覧の行削除ボタン
    if (id == DatailManagementStandard.ManagementStandardList.Id + getAddFormNo()) {
        return preDeleteRowCommonDesignateBtn(id, [DatailManagementStandard.ManagementStandardList.Id], DatailManagementStandard.ButtonId.DeleteManagementStandard);
    }

    // 使用部品一覧の行削除ボタン
    if (id == DetailUseParts.UsePartsList.Id + getAddFormNo()) {
        return preDeleteRowCommonDesignateBtn(id, [DetailUseParts.UsePartsList.Id], DetailUseParts.ButtonId.DeleteUseParts);
    }

    // 親子構成一覧の行削除ボタン
    if (id == DatailConstitution.ParentList.Id + getAddFormNo()) {
        return preDeleteRowCommonDesignateBtn(id, [DatailConstitution.ParentList.Id], DatailConstitution.ButtonId.DeleteParent);
    }

    // ループ構成一覧の行削除ボタン
    if (id == DatailConstitution.LoopList.Id + getAddFormNo()) {
        return preDeleteRowCommonDesignateBtn(id, [DatailConstitution.LoopList.Id], DatailConstitution.ButtonId.DeleteLoop);
    }

    // MP情報一覧の行削除ボタン
    if (id == DetailMpInfo.MpInfoList.Id + getAddFormNo()) {
        return preDeleteRowCommonDesignateBtn(id, [DetailMpInfo.MpInfoList.Id], DetailMpInfo.ButtonId.DeleteMpInfo);
    }

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

    //機器別管理基準タブ 表示一覧選択ボタン
    if (btnCtrlId == DatailManagementStandard.ButtonId.ManagementStandardList) { // 保全項目一覧

        // 一覧の表示状態を切り替え
        changeListDisplay(DatailManagementStandard.ManagementStandardList.Id, true);
        changeListDisplay(DatailManagementStandard.Format1List.Id, false);
        changeListDisplay(DatailManagementStandard.ScheduleCondList.Id, false);
        changeListDisplay(DatailManagementStandard.ScheduleList.Id, false);
        changeListDisplay(DatailManagementStandard.ScheduleLankList.Id, false);
        // ボタン色を変更
        setGrayButton(DatailManagementStandard.ButtonId.ManagementStandardList, false);
        setGrayButton(DatailManagementStandard.ButtonId.Format1List, true);
        setGrayButton(DatailManagementStandard.ButtonId.ScheduleList, true);


    } else if (btnCtrlId == DatailManagementStandard.ButtonId.Format1List) { // 様式１一覧
        // 一覧の表示状態を切り替え
        changeListDisplay(DatailManagementStandard.ManagementStandardList.Id, false);
        changeListDisplay(DatailManagementStandard.Format1List.Id, true);
        changeListDisplay(DatailManagementStandard.ScheduleCondList.Id, false);
        changeListDisplay(DatailManagementStandard.ScheduleList.Id, false);
        changeListDisplay(DatailManagementStandard.ScheduleLankList.Id, false);
        // ボタン色を変更
        setGrayButton(DatailManagementStandard.ButtonId.ManagementStandardList, true);
        setGrayButton(DatailManagementStandard.ButtonId.Format1List, false);
        setGrayButton(DatailManagementStandard.ButtonId.ScheduleList, true);

    } else if (btnCtrlId == DatailManagementStandard.ButtonId.ScheduleList) { // スケジューリング

        // 描画に間に合っていないため間隔をあけて実行
        setTimeout(function () {
            setEventFlg();
        }, 500);

        // 一覧の表示状態を切り替え
        changeListDisplay(DatailManagementStandard.ManagementStandardList.Id, false);
        changeListDisplay(DatailManagementStandard.Format1List.Id, false);
        changeListDisplay(DatailManagementStandard.ScheduleCondList.Id, true);

        // 変更管理対象かどうかを取得
        var historyManagementFlg = getIsHisotyManagement(false);

        // 点検種別毎フラグを取得
        var flg = getValue(MachineDatail.MachineDatail50.Id, 10, 1, CtrlFlag.ChkBox, false, false);
        if (flg == true) {
            changeListDisplay(DatailManagementStandard.ScheduleList.Id, false);
            changeListDisplay(DatailManagementStandard.ScheduleLankList.Id, true);
            // 罫線制御処理
            // 罫線制御処理
            setTimeout(function () { setMaintKindListGroupingMachine(); }, 500);

            //// 点検種別でなければ終了
            //var isDisplayMaintainanceKind = getValue(MachineDatail.MachineDatail50.Id, MachineDatail.MachineDatail50.ColumnNo.MaintLank, 0, CtrlFlag.ChkBox);
            //isDisplayMaintainanceKind = convertStrToBool(isDisplayMaintainanceKind); // 変換
            //if (isDisplayMaintainanceKind) {
            //    // グループ化の単位、機器番号と機器名称
            //    var ListMachineGroups = [DatailManagementStandard.ScheduleLankList.ColumnNo.Level, DatailManagementStandard.ScheduleLankList.ColumnNo.MachineNo, DatailManagementStandard.ScheduleLankList.ColumnNo.MachineName, DatailManagementStandard.ScheduleLankList.ColumnNo.Importance, DatailManagementStandard.ScheduleLankList.ColumnNo.MainteLank];
            //    setTimeout(function () { setListGroupStyle(DatailManagementStandard.ScheduleLankList.Id, DatailManagementStandard.ScheduleLankList.ColumnNo.GroupKey, ListMachineGroups); }, 500);
            //}

        } else {
            changeListDisplay(DatailManagementStandard.ScheduleList.Id, true);
            changeListDisplay(DatailManagementStandard.ScheduleLankList.Id, false);

        }

        // ボタン色を変更
        setGrayButton(DatailManagementStandard.ButtonId.ManagementStandardList, true);
        setGrayButton(DatailManagementStandard.ButtonId.Format1List, true);
        setGrayButton(DatailManagementStandard.ButtonId.ScheduleList, false);
    }
    else if (btnCtrlId == DatailConstitution.ButtonId.Parent) { // 親子構成
        // 一覧の表示状態を切り替え
        changeListDisplay(DatailConstitution.ParentList.Id, true);
        changeListDisplay(DatailConstitution.LoopList.Id, false);
        // ボタン色を変更
        setGrayButton(DatailConstitution.ButtonId.Parent, false);
        setGrayButton(DatailConstitution.ButtonId.Loop, true);

        // 描画に間に合っていないため間隔をあけて実行
        setTimeout(function () {
            changeDispCheckBox(DatailConstitution.ParentList);
        }, 50);
    }
    else if (btnCtrlId == DatailConstitution.ButtonId.Loop) { // ループ構成
        // 一覧の表示状態を切り替え
        changeListDisplay(DatailConstitution.ParentList.Id, false);
        changeListDisplay(DatailConstitution.LoopList.Id, true);
        // ボタン色を変更
        setGrayButton(DatailConstitution.ButtonId.Parent, true);
        setGrayButton(DatailConstitution.ButtonId.Loop, false);

        // 描画に間に合っていないため間隔をあけて実行
        setTimeout(function () {
            changeDispCheckBox(DatailConstitution.LoopList);
        }, 50);
    }
}


/**
 *  スケジューリング一覧の変更フラグ設定
 */
function setEventFlg() {
    var tbl = $(P_Article).find('#' + DatailManagementStandard.ScheduleList.Id + getAddFormNo()).find("div[class='tabulator-table']").children();
    if ($(tbl).length) {
        setEventForEditFlg(true, tbl);
    }

    var tbl1 = $(P_Article).find('#' + DatailManagementStandard.ScheduleLankList.Id + getAddFormNo()).find("div[class='tabulator-table']").children();
    if ($(tbl1).length) {
        setEventForEditFlg(true, tbl1);
    }

}

/**
 * ボタンコントロールIDを指定して通常カラー/グレーを切り替える
 * @param {any} name ボタンのコントロール
 * @param {any} flg true(非表示)、false(表示)
 */
function setGrayButton(name, flg) {
    //ボタン要素取得
    var button = getButtonCtrl(name);
    setClickedColor(button, flg);
}
/**
 *  指定要素を非表示とする。
 *  @element   {string} ：<div>要素指定文字列
 *  @flg       {bool}   ：true(非表示)、false(表示)
 */
function setClickedColor(element, flg) {
    //true(グレー)、false(通常)
    if (flg) {
        $(element).addClass('clickedButton');
    }
    else {
        $(element).removeClass('clickedButton');
    }
}

/**
 * 【オーバーライド用関数】
 *   コンボボックス変更時イベント
 * @param  appPath    : アプリケーションルートパス
 * @param  combo      : 変更イベントが発生したコンボボックスの要素
 * @param  datas      : 変更イベントが発生したコンボボックスのデータリスト
 * @param  selected   : 変更されたコンボボックスの選択されたデータ
 * @param  formNo     : イベントの発生した画面番号
 * @param  ctrlId     : イベントの発生した画面のコントロールID
 * @param  valNo      : イベントの発生したコンボボックスのコントロール番号
 */
function setComboOtherValues(appPath, combo, datas, selected, formNo, ctrlId, valNo) {

    // 機能IDが「帳票出力」の場合
    if (getConductId() == RM00001_ConductId) {
        return RM0001_setComboOtherValues(appPath, combo, datas, selected, formNo, ctrlId, valNo);
    }

    // 共通-文書管理詳細画面のコンボボックス変更処理
    DM0002_setComboOtherValues(appPath, combo, datas, selected, formNo, ctrlId, valNo);

    // 変更されたコンボボックスを判定

    // 使用部品一覧の単票の部品名コンボボックスの場合
    if (formNo == DetailUseParts.No && ctrlId == DetailUseParts.UsePartsList.Id && valNo == DetailUseParts.UsePartsList.ColumnNo.PartsName) {

        // 予備品ID(コンボボックスのvalue)に紐付く「規格・寸法」を画面の項目に設定する
        var dimensions = selected == null ? '' : selected.EXPARAM1;
        setValue(DetailUseParts.UsePartsList.Id, DetailUseParts.UsePartsList.ColumnNo.Dimensions, 1, CtrlFlag.Label, dimensions, true, false);

        // 予備品ID(コンボボックスのvalue)に紐付く「メーカー」を画面の項目に設定する
        var maker = selected == null ? '' : selected.EXPARAM2;
        setValue(DetailUseParts.UsePartsList.Id, DetailUseParts.UsePartsList.ColumnNo.Maker, 1, CtrlFlag.Label, maker, true, false);

        // スケジューリング表示条件 スケジュール表示単位の場合
    } else if (formNo == MachineDatail.No && ctrlId == DatailManagementStandard.ScheduleCondList.Id && valNo == DatailManagementStandard.ScheduleCondList.ColumnNo.DispType) {
        // 表示単位に応じてスケジュール表示年度orスケジュール表示期間の表示切替
        var typeVal = getValue(DatailManagementStandard.ScheduleCondList.Id, DatailManagementStandard.ScheduleCondList.ColumnNo.DispType, 1, CtrlFlag.Combo);
        var typeYear = getValue(DatailManagementStandard.ScheduleCondList.Id, DatailManagementStandard.ScheduleCondList.ColumnNo.NonDispId, 1, CtrlFlag.Label);
        if (typeVal == typeYear) {
            changeColumnDisplay(DatailManagementStandard.ScheduleCondList.Id, DatailManagementStandard.ScheduleCondList.ColumnNo.DispYear, true);
            changeColumnDisplay(DatailManagementStandard.ScheduleCondList.Id, DatailManagementStandard.ScheduleCondList.ColumnNo.DispSpan, false);
        } else {
            changeColumnDisplay(DatailManagementStandard.ScheduleCondList.Id, DatailManagementStandard.ScheduleCondList.ColumnNo.DispYear, false);
            changeColumnDisplay(DatailManagementStandard.ScheduleCondList.Id, DatailManagementStandard.ScheduleCondList.ColumnNo.DispSpan, true);
        }
    } else if (formNo == MachineDatail.No && ctrlId == DatailLongPlan.ScheduleCondList.Id && valNo == DatailLongPlan.ScheduleCondList.ColumnNo.DispType) {
        // 表示単位に応じてスケジュール表示年度orスケジュール表示期間の表示切替
        var typeVal = getValue(DatailLongPlan.ScheduleCondList.Id, DatailLongPlan.ScheduleCondList.ColumnNo.DispType, 1, CtrlFlag.Combo);
        var typeYear = getValue(DatailLongPlan.ScheduleCondList.Id, DatailLongPlan.ScheduleCondList.ColumnNo.NonDispId, 1, CtrlFlag.Label);
        if (typeVal == typeYear) {
            changeColumnDisplay(DatailLongPlan.ScheduleCondList.Id, DatailLongPlan.ScheduleCondList.ColumnNo.DispYear, true);
            changeColumnDisplay(DatailLongPlan.ScheduleCondList.Id, DatailLongPlan.ScheduleCondList.ColumnNo.DispSpan, false);
        } else {
            changeColumnDisplay(DatailLongPlan.ScheduleCondList.Id, DatailLongPlan.ScheduleCondList.ColumnNo.DispYear, false);
            changeColumnDisplay(DatailLongPlan.ScheduleCondList.Id, DatailLongPlan.ScheduleCondList.ColumnNo.DispSpan, true);
        }
    }

}

function prevCreateTabulator(appPath, id, options, header, dispData) {

    // 機能IDが「帳票出力」の場合
    if (getConductId() == RM00001_ConductId) {
        return RM0001_prevCreateTabulator(appPath, id, options, header, dispData);
    }

    if (id == "#" + DatailManagementStandard.Format1List.Id + "_1") {
        //行移動可
        options["movableRows"] = true;
        if (!header.some(x => x.field === "moveRowBtn")) {
            //先頭に移動用のアイコン列追加
            header.unshift({ title: "", rowHandle: true, field: "moveRowBtn", formatter: "handle", headerSort: false, frozen: true, width: 30, minWidth: 30 });
        }
    } else if (id == "#" + DatailLongPlan.LongPlanList.Id + "_1") {
        setTimeout(function () {
            rowNoLinkChangeLongPlan();
        }, 50);

    } else if (id == "#" + DatailMaintainanceActivity.MaintainanceActivityList.Id + "_1") {
        setTimeout(function () {
            rowNoLinkChangeMa();
        }, 50);

    } else if (id == "#" + DatailManagementStandard.ManagementStandardList.Id + "_1") {

        //変更管理対象ならNoリンク非活性制御
        if (getIsHisotyManagement(false)) {
            //ブラウザに処理が戻った際に実行
            setTimeout(function () {
                setHideDetailLinkStandartList();
            }, 0);
        }

    }
}

/**
 * 構成機器タブ 初期化処理
 */
function initTabConstitution() {

    // ボタンを活性状態にする
    setDispMode(getButtonCtrl(DatailConstitution.ButtonId.Parent), false); // 親子構成
    setDispMode(getButtonCtrl(DatailConstitution.ButtonId.Loop), false);   // ループ構成

    // 親子構成一覧・ループ構成一覧の行追加ボタン(+アイコン)を表示する
    $("a[data-actionkbn=1221][data-parentid='" + DatailConstitution.ParentList.Id + "']").first().show();
    $("a[data-actionkbn=1221][data-parentid='" + DatailConstitution.LoopList.Id + "']").first().show();

    // 非表示で定義されている一覧より機器レベルの番号を取得
    var equipmentLevelNo = getValue(DatailConstitution.HideCondition.Id, DatailConstitution.HideCondition.EquipmentLevelNo, 1, CtrlFlag.Label, false, false);

    var clickBtnId = "";   // クリックするボタン
    var disableBtnId = ""; // 非活性にするボタン

    // 取得した機器レベルに応じてボタン・一覧の表示/非表示の切替を行う
    if (equipmentLevelNo == EquipmentLevelNo.Parent) {
        // 親機 の場合
        clickBtnId = DatailConstitution.ButtonId.Parent; // 親子構成(クリックするボタン)
        disableBtnId = DatailConstitution.ButtonId.Loop; // ループ構成(非活性にするボタン)
    }
    else if (equipmentLevelNo == EquipmentLevelNo.Machine) {
        // 機器 の場合
        clickBtnId = DatailConstitution.ButtonId.Parent; // 親子構成(クリックするボタン)

        // ループ構成一覧の行追加ボタン(+アイコン)を非表示にする
        $("a[data-actionkbn=1221][data-parentid='" + DatailConstitution.LoopList.Id + "']").first().hide();
    }
    else if (equipmentLevelNo == EquipmentLevelNo.Accessory) {
        // 付属品 の場合
        clickBtnId = DatailConstitution.ButtonId.Parent; // 親子構成(クリックするボタン)
        disableBtnId = DatailConstitution.ButtonId.Loop; // ループ構成(非活性にするボタン)

        // 親子構成一覧の行追加ボタン(+アイコン)を非表示にする
        $("a[data-actionkbn=1221][data-parentid='" + DatailConstitution.ParentList.Id + "']").first().hide();
    }
    else {
        // ループ の場合
        clickBtnId = DatailConstitution.ButtonId.Loop;     // ループ構成(クリックするボタン)
        disableBtnId = DatailConstitution.ButtonId.Parent; // 親子構成(非活性にするボタン)
    }

    // ボタンを非活性にする
    if (disableBtnId != "") {
        setDispMode(getButtonCtrl(disableBtnId), true);
    }

    // ボタンクリック
    var button = getButtonCtrl(clickBtnId);
    $(button).click();
}


/**
 *【オーバーライド用関数】タブ切替の初期化処理
 * @tabNo ：タブ番号
 * @tableId : 一覧のコントロールID
 */
function initTabOriginal(tabNo, tableId) {
    // 表示するタブ番号を判定
    if (tabNo == DatailConstitution.TabNo) { // 構成機器タブ

        // 構成機器タブ
        initTabConstitution();
        // 親子構成ボタンがクリックされていたか判定
        var button = getButtonCtrl(DatailConstitution.ButtonId.Parent);
        if (button.hasClass('clickedButton')) {
            // クリックされていなかった場合
            button = getButtonCtrl(DatailConstitution.ButtonId.Loop);
            $(button).click();
        }
        else {
            // クリックされていた場合
            $(button).click();
        }

    } else if (tabNo == DatailManagementStandard.TabNo) { // 機器別管理基準タブ
        // スケジューリング一覧の行データが取得できた後に非表示処理を行う。
        //// 初期状態で非表示とすると更新フラグイベント付与が正常に動作しない

        if (tableId == DatailManagementStandard.ScheduleList.Id + getAddFormNo()) {

            // 描画に間に合っていないため間隔をあけて実行
            setTimeout(function () {
                setEventFlg();
            }, 500);

        } else if (tableId == DatailManagementStandard.ScheduleLankList.Id + getAddFormNo()) {

            // 描画に間に合っていないため間隔をあけて実行
            setTimeout(function () {
                setEventFlg();
            }, 500);

        }
        //変更管理対象ならNoリンク非活性制御
        if (getIsHisotyManagement(false)) {
            //ブラウザに処理が戻った際に実行
            setTimeout(function () {
                setHideDetailLinkStandartList();
            }, 0);
        }

    } else if (tabNo == 4) { // タブ// 長期計画一覧
        setTimeout(function () {
            rowNoLinkChangeLongPlan();
        }, 50);
    } else if (tabNo == 5) { // タブ// 保全活動
        setTimeout(function () {
            rowNoLinkChangeMa();
        }, 50);
    }


    //// 点検種別毎フラグを取得
    //var flg = getValue(MachineDatail.MachineDatail50.Id, 10, 1, CtrlFlag.ChkBox, false, false);
    //if (flg == true) {
    //    var pageRowCount = P_listData["#" + DatailManagementStandard.ScheduleLankList.Id + getAddFormNo()].getDataCount("display");
    //    if (pageRowCount == 0) {
    //        // 機器別管理基準タブ初期表示一覧設定
    //        // 保全項目一覧をクリック
    //        var button = getButtonCtrl(DatailManagementStandard.ButtonId.ManagementStandardList);
    //        $(button).click();
    //    }

    //} else {
    //    var pageRowCount = P_listData["#" + DatailManagementStandard.ScheduleList.Id + getAddFormNo()].getDataCount("display");
    //    if (pageRowCount == 0) {
    //        // 機器別管理基準タブ初期表示一覧設定
    //        // 保全項目一覧をクリック
    //        var button = getButtonCtrl(DatailManagementStandard.ButtonId.ManagementStandardList);
    //        $(button).click();
    //    }
    //}
}

/**
 * 一覧のチェックボックスを非表示にする
 * @param list : 一覧
 */
function changeDispCheckBox(list) {
    // 一覧データ取得
    var tbl = $(P_Article).find('#' + list.Id + getAddFormNo()).find("div[class='tabulator-table']").children();
    if ($(tbl).length) {
        $.each($(tbl), function (idx, row) {
            // 子フラグ取得
            var value = getValue(list.Id, list.FlgChild, idx, CtrlFlag.Label, false, false);
            // 自分自身の子として紐付けられているデータ以外は選択チェックボックスを非表示とする
            if (value == DatailConstitution.FlgChildTargetNo) {
                $(row).find(":checkbox").remove();
            }
        });
    }
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

    // クリックされた一覧を判定
    if (ctrlId == DatailConstitution.ParentList.Id) {
        // 親子構成一覧
        // 一覧番号を設定
        setValue(DatailConstitution.HideCondition.Id, DatailConstitution.HideCondition.FlgList, 1, CtrlFlag.Label, DatailConstitution.FlgList.Parent, false, false);

    } else if (ctrlId == DatailConstitution.LoopList.Id) {
        // ループ構成一覧
        // 一覧番号を設定
        setValue(DatailConstitution.HideCondition.Id, DatailConstitution.HideCondition.FlgList, 1, CtrlFlag.Label, DatailConstitution.FlgList.Loop, false, false);
    }

    return true;    // 個別実装で以後の処理の実行可否を制御 true：続行、false：中断
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

    if (formNo == SelectMachine.No) {
        // 機器選択画面
        if ($(conditionDataList).length) {
            var flgAdded = false;
            $.each($(conditionDataList), function (idx, conditionData) {
                // 場所階層、職種機種のツリーラベルを含む一覧の場合
                if (conditionData.FORMNO == SelectMachine.No && (conditionData.CTRLID == SelectMachine.SearchCondition10.Id || conditionData.CTRLID == SelectMachine.SearchCondition20.Id)) {
                    // 一覧の定義情報に格納
                    listDefines.push(conditionData);

                    // 構成機器タブの非表示一覧の値を取得
                    if (!flgAdded) {
                        // 件名情報の値を取得
                        var machineId = getListDataByCtrlIdList([DatailConstitution.HideCondition.Id], MachineDatail.No, 0, false);
                        conditionDataList.push(machineId[0]);
                        flgAdded = true;
                    }
                }
            });
        }
    } else if (formNo == MachineDatail.No) {

        // 遷移先情報を設定(URL指定起動)
        conditionDataList = getParamByUrl(MC0001_ConductId, conditionDataList);
    }

    return [conditionDataList, listDefines];
}

/**
 *【オーバーライド用関数】実行ﾁｪｯｸ処理 - 前処理
 *  @appPath        ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId      ：機能ID
 *  @formNo         ：画面番号
 *  @btn            ：押下されたボタン要素
 */
function registCheckPre(appPath, conductId, formNo, btn) {

    // 画面番号を判定
    if (formNo == SelectMachine.No) {
        // 機器選択画面

        // 検索結果一覧の要素取得
        var targetTbl = $('#' + SelectMachine.SelectMachineList.Id + '_' + formNo);
        checkes = $(targetTbl).find("[tabulator-field='SELTAG'] :checkbox:checked");

        clearMessage();
        // 一覧で選択チェックボックスがチェックされていなければエラー
        if (checkes == null || checkes.length == 0) {
            // 「対象行が選択されていません。」
            setMessage([P_ComMsgTranslated[941160003]], messageType.Warning);
            return false;
        }
    }
    return true;
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

    // 機器選択画面の検索後
    if (status == pageStatus.SEARCH && formNo == SelectMachine.No) {
        // 検索条件エリアの機器レベルコンボボックスを非活性にする
        disableEquipmentCmb();
    }
}


/**
 * 検索条件エリアの機器レベルコンボボックスを非活性にする
 * */
function disableEquipmentCmb() {
    // 検索条件エリアの機器レベルコンボボックスの要素を取得
    var id = SelectMachine.SearchCondition20.Id + "_" + SelectMachine.No;
    var cmbEquipmentLevel = $("#" + id + "DispROWNO1").find("select[name='" + id + "VAL" + SelectMachine.SearchCondition20.Equipment + "']");
    // コンボボックスを非活性にする
    setDisableBtn(cmbEquipmentLevel, true);
}

/**
 * 【オーバーライド用関数】全選択および全解除ボタンの押下後
 * @param  formNo  : 画面番号
 * @param  tableId : 一覧のコントロールID
 */
function afterAllSelectCancelBtn(formNo, tableId) {

    if (formNo == MachineDatail.No && tableId == DatailConstitution.ParentList.Id) {
        // 詳細画面 構成機器タブ 親子構成一覧
        changeDispCheckBox(DatailConstitution.ParentList);
    }
    else if (formNo == MachineDatail.No && tableId == DatailConstitution.LoopList.Id) {
        // 詳細画面 構成機器タブ ループ構成一覧
        changeDispCheckBox(DatailConstitution.LoopList);
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

    if (id == "#" + DatailManagementStandard.ScheduleList.Id + getAddFormNo()) {

        // スケジューリング一覧
        // ラベル列/入力可能列の表示切替を行う
        hideColumnOfScheduleBaseList(getIsHisotyManagement(false));

        // 描画に間に合っていないため間隔をあけて実行
        setTimeout(function () {
            setEventFlg();
        }, 500);

    }
    else if (id == "#" + DatailManagementStandard.ScheduleLankList.Id + getAddFormNo()) {

        // ラベル列/入力可能列の表示切替を行う
        hideColumnOfScheduleLankList(getIsHisotyManagement(false));

        // 詳細画面 機器別管理基準タブ 点検種別毎スケジューリング一覧
        // 保全活動一覧(点検種別毎)の場合、罫線調整処理を呼び出す
        // 一覧の非表示行を設定しているので、少し遅らせる
        setTimeout(function () { setMaintKindListGroupingMachine(); }, 500);
        // 描画に間に合っていないため間隔をあけて実行
        setTimeout(function () {
            setEventFlg();
        }, 500);

    }
    else if (id == "#" + DatailConstitution.ParentList.Id + getAddFormNo()) {
        // 詳細画面 構成機器タブ 親子構成一覧
        changeDispCheckBox(DatailConstitution.ParentList);
    }
    else if (id == "#" + DatailConstitution.LoopList.Id + getAddFormNo()) {
        // 詳細画面 構成機器タブ ループ構成一覧
        changeDispCheckBox(DatailConstitution.LoopList);
    }
    else if (id == "#" + SelectMachine.SelectMachineList.Id + getAddFormNo()) {
        // 機器選択画面 検索結果一覧
        // 選択ボタン列を非表示にする
        var table = P_listData['#' + SelectMachine.SelectMachineList.Id + getAddFormNo()];
        table.hideColumn("VAL" + SelectMachine.SelectMachineList.SelectBtn);
        tbl.redraw(true);
    } else if (id == '#' + MachineDatail.MachineDatail75.Id + getAddFormNo()) {
        // 参照画面 機種別仕様タブ レイアウトと値の設定
        initEquipmentSpec(MachineDatail.MachineDatail75);
    } else if (id == '#' + SpecEdit.SpecDetail20.Id + getAddFormNo()) {
        // 機種別仕様編集画面 機種別仕様 レイアウトと値の設定
        initEquipmentSpec(SpecEdit.SpecDetail20);
    } else if (id == '#' + DatailLongPlan.LongPlanList.Id + getAddFormNo()) {
        setTimeout(function () {
            rowNoLinkChangeLongPlan();
        }, 50);
    } else if (id == '#' + DatailMaintainanceActivity.MaintainanceActivityList.Id + getAddFormNo()) {
        // 描画に間に合っていないため間隔をあけて実行
        setTimeout(function () {
            rowNoLinkChangeMa();
        }, 50);
    } else if (id == '#' + DatailManagementStandard.ManagementStandardList.Id + getAddFormNo()) {

        //変更管理対象ならNoリンク非活性制御
        if (getIsHisotyManagement(false)) {
            //ブラウザに処理が戻った際に実行
            setTimeout(function () {
                setHideDetailLinkStandartList();
            }, 0);
        }
    }
}

/**
 * 保全項目のNoリンク非表示制御
 */
function setHideDetailLinkStandartList() {
    var hideBtns = [];
    var hideLists = [DatailManagementStandard.ManagementStandardList.Id];
    setHistoryManagementCtrlDisplay(getIsHisotyManagement(false), MachineDatail.ButtonId.HistoryManagementDetail, hideBtns, hideLists, true);
}

/**
 * データのリンクを無効にする
 * */
function noneLinkData(rowNo) {

    // コントロールID
    var ctrlId;

    ctrlId = DatailManagementStandard.ManagementStandardList.Id;

    //  一覧情報取得
    var rows = P_listData["#" + ctrlId + getAddFormNo()].searchRows("ROWNO", "=", rowNo);
    var rowEle = rows[0].getElement();
    rows = null;
    if (rowEle && $(rowEle).length > 0) {
        // レコードのNo.リンクを無効化する
        var link = $(rowEle).find("div[tabulator-field='ROWNO']");
        $(link).addClass("linkDisable");
        // 文字色を黒に変更
        var value = $(link).find("a");
        if (value && value.length > 0) {
            $(value)[0].style.color = "black";
        }

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
 */
function beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom, transPtn) {

    // 機能IDが「帳票出力」の場合
    RM0001_beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom);

    // 共通-文書管理詳細画面の画面再表示前処理
    // 共通-文書管理詳細画面を閉じたときの再表示処理はこちらで行うので、各機能での呼出は不要
    DM0002_beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom);

    // 共通-予備品検索画面を閉じたときの再表示処理はこちらで行うので、各機能での呼出は不要
    SP0001_beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom);

    // 参照画面の場合、戻る/閉じるボタン表示制御
    if (formNo == MachineDatail.No) {
        setDisplayCloseBtn(transPtn);
    }

    // (保全項目一覧、使用部品一覧、親子構成一覧、ループ構成一覧、MP情報一覧)の単票で登録された場合 初期化処理を行う
    if (btnCtrlId == SingleVote.Regist &&
        (targetCtrlId == DatailManagementStandard.ManagementStandardList.Id ||
            targetCtrlId == DetailUseParts.UsePartsList.Id ||
            targetCtrlId == DatailConstitution.ParentList.Id ||
            targetCtrlId == DatailConstitution.LoopList.Id ||
            targetCtrlId == DetailMpInfo.MpInfoList.Id)) {

        initFormData(appPath, conductId, pgmId, formNo, btnCtrlId, conductPtn, selectData, listData, status);
    }
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

    // 構成機器タブ
    initTabConstitution();
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

    if (formNo == MachineDatail.No) {
        // 検索条件用の値を取得
        if ($(btn).attr("name") == DatailManagementStandard.ButtonId.RegistSchedule || $(btn).attr("name") == DetailUseParts.ButtonId.DeleteUseParts || $(btn).attr("name") == DatailManagementStandard.ButtonId.DeleteManagementStandard
            || $(btn).attr("name") == DatailConstitution.ButtonId.DeleteParent || $(btn).attr("name") == DatailConstitution.ButtonId.DeleteLoop || $(btn).attr("name") == DetailMpInfo.ButtonId.DeleteMpInfo
            || $(btn).attr("name") == DatailManagementStandard.ButtonId.RegistOrder || $(btn).attr("name") == DatailManagementStandard.ButtonId.RegistLankSchedule) {
            var unit = getValueByOtherForm(MachineDatail.No, DatailManagementStandard.ScheduleCondList.Id, DatailManagementStandard.ScheduleCondList.ColumnNo.DispType, 1, CtrlFlag.Combo);
            setValue(DatailManagementStandard.ScheduleCondListUnDisp.Id, DatailManagementStandard.ScheduleCondListUnDisp.ColumnNo.DispType, 0, CtrlFlag.Label, unit, false, false);
            var year = getValueByOtherForm(MachineDatail.No, DatailManagementStandard.ScheduleCondList.Id, DatailManagementStandard.ScheduleCondList.ColumnNo.DispYear, 1, CtrlFlag.TextBox);
            setValue(DatailManagementStandard.ScheduleCondListUnDisp.Id, DatailManagementStandard.ScheduleCondListUnDisp.ColumnNo.DispYear, 0, CtrlFlag.Label, year, false, false);

            var spanTo = document.getElementById('BODY_140_00_LST_1_1VAL3To').value;
            var span = getValueByOtherForm(MachineDatail.No, DatailManagementStandard.ScheduleCondList.Id, DatailManagementStandard.ScheduleCondList.ColumnNo.DispSpan, 1, CtrlFlag.TextBox);
            span = span + "|" + spanTo;
            setValue(DatailManagementStandard.ScheduleCondListUnDisp.Id, DatailManagementStandard.ScheduleCondListUnDisp.ColumnNo.DispSpan, 0, CtrlFlag.Label, span, false, false);

            var manthId = getValueByOtherForm(MachineDatail.No, DatailManagementStandard.ScheduleCondList.Id, DatailManagementStandard.ScheduleCondList.ColumnNo.NonDispId, 1, CtrlFlag.Label);
            setValue(DatailManagementStandard.ScheduleCondListUnDisp.Id, DatailManagementStandard.ScheduleCondListUnDisp.ColumnNo.NonDispId, 0, CtrlFlag.Label, manthId, false, false);

            var unit1 = getValueByOtherForm(MachineDatail.No, DatailLongPlan.ScheduleCondList.Id, DatailLongPlan.ScheduleCondList.ColumnNo.DispType, 1, CtrlFlag.Combo);
            setValue(DatailLongPlan.ScheduleCondListUnDisp.Id, DatailLongPlan.ScheduleCondListUnDisp.ColumnNo.DispType, 0, CtrlFlag.Label, unit1, false, false);

            var year1 = getValueByOtherForm(MachineDatail.No, DatailLongPlan.ScheduleCondList.Id, DatailLongPlan.ScheduleCondList.ColumnNo.DispYear, 1, CtrlFlag.TextBox);
            setValue(DatailLongPlan.ScheduleCondListUnDisp.Id, DatailLongPlan.ScheduleCondListUnDisp.ColumnNo.DispYear, 0, CtrlFlag.Label, year1, false, false);

            var spanTo1 = document.getElementById('BODY_190_00_LST_1_1VAL3To').value;
            var span1 = getValueByOtherForm(MachineDatail.No, DatailLongPlan.ScheduleCondList.Id, DatailLongPlan.ScheduleCondList.ColumnNo.DispSpan, 1, CtrlFlag.TextBox);
            span1 = span1 + "|" + spanTo1;
            setValue(DatailLongPlan.ScheduleCondListUnDisp.Id, DatailLongPlan.ScheduleCondListUnDisp.ColumnNo.DispSpan, 0, CtrlFlag.Label, span1, false, false);

            var manthId1 = getValueByOtherForm(MachineDatail.No, DatailLongPlan.ScheduleCondList.Id, DatailLongPlan.ScheduleCondList.ColumnNo.NonDispId, 1, CtrlFlag.Label);
            setValue(DatailLongPlan.ScheduleCondListUnDisp.Id, DatailLongPlan.ScheduleCondListUnDisp.ColumnNo.NonDispId, 0, CtrlFlag.Label, manthId1, false, false);

            return getListDataByCtrlIdList([MachineDatail.MachineDatail20.Id, DetailMpInfo.HideId, DatailManagementStandard.ScheduleCondListUnDisp.Id, DatailLongPlan.ScheduleCondListUnDisp.Id], formNo, 0, true);
        } else {
            return getListDataByCtrlIdList([MachineDatail.MachineDatail20.Id, DetailMpInfo.HideId,], formNo, 0, true);
        }
    }

    if (formNo == SpecEdit.No) {
        // 検索条件用の値を取得
        return getListDataByCtrlIdList([MachineDatail.MachineDatail20.Id], MachineDatail.No, 0, true);
    }

    // それ以外の場合
    var conditionDataList = [];
    return conditionDataList;
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
        return RM0001_reportCheckPre(appPath, conductId, formNo, btn);
    }

    return true;
}

/**
 *【オーバーライド用関数】取込処理個別入力チェック
 *  @param appPath       ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param conductId     ：機能ID
 *  @param formNo        ：画面番号
 */
function preInputCheckUpload(appPath, conductId, formNo) {

    if (conductId == DM0002_ConductId) {

        // 共通-文書管理詳細画面 入力チェック
        return DM0002_preInputCheckUpload(appPath, conductId, formNo);
    }
    else {
        // 共通-帳票出力画面 入力チェック
        return RM0001_preInputCheckUpload(appPath, conductId, formNo);
    }
}

/**
 * 指定された一覧より、選択チェックボックスが選択されているデータの行番号を返す
 * @param {any} ctrlId: 一覧のID
 */
function getRowNoByList(ctrlId) {
    var rowNo = -1;
    var table = P_listData['#' + ctrlId + getAddFormNo()];
    if (table) {
        var trs = table.getRows();
        if (trs != null && trs.length > 0) {
            // 検索結果一覧を参照し、選択チェックボックスが選択されているデータの行番号(rowNo)を返す
            $(trs).each(function (i, tr) {
                var ele = tr.getElement();
                var chkbox = $(ele).find("div[tabulator-field='SELTAG'] input[type='checkbox']");
                if ($(chkbox[0]).prop('checked')) {
                    rowNo = $(chkbox).data("rowno");
                    return;
                }
            });
        }
    }
    return rowNo - 0;
}

/** 
 * 機種別仕様のレイアウト作成と値設定
 * @param {any} define 定義（MachineDatail.MachineDatail75 or SpecEdit.SpecDetail20）
 */
function initEquipmentSpec(define) {

    var columnNo = getFormNo() == MachineDatail.No ? MachineDatail.MachineDatail70.ColumnNo : SpecEdit.SpecDetail00.ColumnNo;
    //レイアウトテンプレートの最大VAL値
    var valIndex = getMaxVal(columnNo);

    // 前回表示されていた分の要素を削除
    var delValIdx = valIndex + 1;
    var id = getFormNo() == MachineDatail.No ? MachineDatail.MachineDatail70.Id : SpecEdit.SpecDetail00.Id;
    var item = $(P_Article).find("#" + id + getAddFormNo() + " table tbody td[data-name='VAL" + delValIdx + "']");
    while (item.length > 0) {
        item.closest("tr").remove();
        delValIdx = delValIdx + 1;
        item = $(P_Article).find("#" + id + getAddFormNo() + " table tbody td[data-name='VAL" + delValIdx + "']");
    }

    var table = P_listData['#' + define.Id + getAddFormNo()];
    if (!table) {
        return;
    }
    //非表示一覧のデータを取得
    var data = table.getData();

    //var columnNo = getFormNo() == MachineDatail.No ? MachineDatail.MachineDatail70.ColumnNo : SpecEdit.SpecDetail00.ColumnNo;
    ////レイアウトテンプレートの最大VAL値
    //var valIndex = getMaxVal(columnNo);
    $.each(data, function (idx, rowData) {
        //レイアウトテンプレートのVAL値（コピー対象のVAL値）
        var templateVal = 0;
        //テンプレートのコントロールタイプ
        var ctrlType = 0;
        //追加する項目のVAL値
        var addVal = valIndex + idx + 1;

        //入力形式区分
        var specTypeKbn = rowData["VAL" + define.ColumnNo.SpecTypeKbn];
        switch (specTypeKbn) {
            case SpecType.Text: //テキストボックス
                templateVal = columnNo.Text;
                ctrlType = CtrlFlag.TextBox;

                //項目追加
                var td = addItemLayout(templateVal, ctrlType, addVal, rowData, define);
                var input = $(td).find(":text");
                //値
                var val = rowData["VAL" + define.ColumnNo.Text];
                //値設定
                $(input).val(val);
                //ﾗﾍﾞﾙ表示用spanﾀｸﾞに表示文字列ｾｯﾄ
                setLabelingSpan(td, $(input).val());

                break;

            case SpecType.Num: //数値
                templateVal = columnNo.Num;
                ctrlType = CtrlFlag.TextBox;

                //項目追加
                var td = addItemLayout(templateVal, ctrlType, addVal, rowData, define);
                var input = $(td).find("input[data-type='num']");

                //値
                var val = rowData["VAL" + define.ColumnNo.Num];
                if (val == null) {
                    val = "";
                }
                //単位
                var unit = rowData["VAL" + define.ColumnNo.UnitName];
                //値と単位を@で結合
                var str = val + "@" + unit;
                //値設定
                setDataForTextNum(td, input, str);
                break;

            case SpecType.NumMinMax: //数値(範囲)
                templateVal = columnNo.NumMinMax;
                ctrlType = CtrlFlag.TextBox;

                //項目追加
                var td = addItemLayout(templateVal, ctrlType, addVal, rowData, define);
                var input = $(td).find("input[data-type='num']");

                //値
                var min = rowData["VAL" + define.ColumnNo.NumMin];
                if (min == null) {
                    min = "";
                }
                var max = rowData["VAL" + define.ColumnNo.NumMax];
                if (max == null) {
                    max = "";
                }
                //単位
                var unit = rowData["VAL" + define.ColumnNo.UnitName];
                //値と単位を@で結合
                var str = min + "|" + max + "@" + unit;
                //値設定
                setDataForTextNum(td, input, str);
                break;

            case SpecType.Select: //コンボボックス
                templateVal = columnNo.Select;
                ctrlType = CtrlFlag.Combo;

                //項目追加
                var td = addItemLayout(templateVal, ctrlType, addVal, rowData, define);
                var select = $(td).find("select");

                //選択アイテムの絞り込み(ブランクと仕様項目IDが一致するものを残す)
                var specId = rowData["VAL" + define.ColumnNo.SpecId];
                $(select).children('option[value!=""][exparam1!="' + specId + '"]').remove();

                //値
                var val = rowData["VAL" + define.ColumnNo.SelectStructure];
                //値設定
                setDataForSelectBox(td, select, val);
                break;

            default:
                break;
        }
    });
}

/**
 * 連想配列の値の最大値を取得
 * @param {any} data 連想配列
 */
function getMaxVal(data) {
    var max = 0;
    $.each(data, function (key, val) {
        if (max < val) {
            max = val;
        }
    });
    return max;
}

// NOリンクを変更
function rowNoLinkChangeLongPlan() {
    // 一覧データ取得
    var tbl = $(P_Article).find('#' + DatailLongPlan.LongPlanList.Id + getAddFormNo()).find("div[class='tabulator-table']").children();
    if ($(tbl).length) {
        $.each($(tbl), function (idx, row) {
            var target = getCtrl(DatailLongPlan.LongPlanList.Id, 1, idx, CtrlFlag.Link, false, false);
            target.innerHTML = '<span class="glyphicon glyphicon-file"></span>';
        });
    }
}

function rowNoLinkChangeMa() {
    // 一覧データ取得
    var tbl = $(P_Article).find('#' + DatailMaintainanceActivity.MaintainanceActivityList.Id + getAddFormNo()).find("div[class='tabulator-table']").children();
    if ($(tbl).length) {
        $.each($(tbl), function (idx, row) {
            var target = getCtrl(DatailMaintainanceActivity.MaintainanceActivityList.Id, 1, idx, CtrlFlag.Link, false, false);
            target.innerHTML = '<span class="glyphicon glyphicon-file"></span>';
        });
    }
}

/**
 * 機種別仕様に項目を追加
 * @param {any} templateVal レイアウトテンプレートのVAL値（コピー対象のVAL値）
 * @param {any} ctrlType レイアウトテンプレートのコントロールタイプ
 * @param {any} addVal 追加する項目のVAL値
 * @param {any} rowData 追加する項目のデータ
 * @param {any} define 定義（MachineDatail.MachineDatail75 or SpecEdit.SpecDetail20）
 */
function addItemLayout(templateVal, ctrlType, addVal, rowData, define) {
    //項目が追加済みかチェック
    var id = getFormNo() == MachineDatail.No ? MachineDatail.MachineDatail70.Id : SpecEdit.SpecDetail00.Id;
    var item = $(P_Article).find("#" + id + getAddFormNo() + " table tbody td[data-name='VAL" + addVal + "']");
    if (item.length > 0) {
        //追加済みの為、スキップ
        return item;
    }

    //コピー対象のtrを取得する
    var templateCtrl = getCtrl(id, templateVal, 1, ctrlType);
    var templateTr = $(templateCtrl).closest("tr");
    //テンプレートをコピーする
    var cloneTr = $(templateTr).clone(true);

    var inputClone = $(cloneTr).find('input, select');
    $.each(inputClone, function (i, ele) {
        //コピーする要素のid,name属性を設定し直す

        //ID属性値
        var orgId = $(ele).attr('id');
        var cloneId = "";
        if (orgId.indexOf("VAL") >= 0) {
            //テキスト、数値の場合
            cloneId = orgId.replace('VAL' + templateVal, 'VAL' + addVal);
        } else {
            //コンボボックスの場合
            cloneId = orgId + 'VAL' + addVal;
        }
        //ID属性
        setAttrByNativeJs(ele, "id", cloneId);
        //name属性
        setAttrByNativeJs(ele, "name", $(ele).attr('name').replace('VAL' + templateVal, 'VAL' + addVal));

        //小数部の桁数
        var decimalPlace = rowData["VAL" + define.ColumnNo.DecimalPlace];
        //if (decimalPlace) {
        //    //data-format属性(#.###等)
        //    setAttrByNativeJs(ele, "data-format", "." + "#".repeat(decimalPlace));
        //}
        if (decimalPlace) {
            //data-format属性(#.###等)
            setAttrByNativeJs(ele, "data-format", "." + "#".repeat(decimalPlace));
        } else {
            // 少数桁0の場合にフォーマット
            if (decimalPlace == 0) {
                //data-format属性(#.###等)
                setAttrByNativeJs(ele, "data-format", "###");
            }
        }
    });

    //th,tdのdata-name属性値の修正
    var th = $(cloneTr).children("th");
    var td = $(cloneTr).children("td");
    setAttrByNativeJs(th, "data-name", $(th).data("name").replace('VAL' + templateVal, 'VAL' + addVal));
    setAttrByNativeJs(td, "data-name", $(td).data("name").replace('VAL' + templateVal, 'VAL' + addVal));

    //ヘッダ名変更
    $(th).text(rowData["VAL" + define.ColumnNo.Name]);

    //項目を追加
    var tbody = $(templateTr).closest("tbody");
    $(tbody).append(cloneTr);

    //表示設定
    changeColumnDisplay(id, addVal, true);

    //入力ﾌｫｰﾏｯﾄ設定処理
    initFormat("#" + P_formDetailId);

    //Validator初期化
    initValidator("#" + P_formDetailId);

    //td要素を戻す（値設定時に必要）
    return td;
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
    if (formNo == SpecEdit.No) {
        //機種別仕様　登録

        // 入力された値を取得
        listData = getListDataVertical(formNo, $(P_Article).find("#" + SpecEdit.SpecDetail00.Id + getAddFormNo()))[0];

        //レイアウトテンプレートの最大VAL値
        var maxVal = getMaxVal(SpecEdit.SpecDetail00.ColumnNo);

        //非表示一覧を取得
        var table = P_listData['#' + SpecEdit.SpecDetail20.Id + getAddFormNo()];
        var rows = table.getRows();

        //入力された値を非表示一覧に反映する
        $.each(rows, function (idx, row) {
            var data = row.getData();

            //入力形式区分
            var specTypeKbn = data["VAL" + SpecEdit.SpecDetail20.ColumnNo.SpecTypeKbn];
            switch (specTypeKbn) {
                case SpecType.Text: //テキストボックス
                    var column = "VAL" + (maxVal + idx + 1);
                    var value = listData[column];

                    //対象データを更新
                    var updateData = {};
                    updateData["VAL" + SpecEdit.SpecDetail20.ColumnNo.Text] = value;
                    row.update(updateData);
                    break;

                case SpecType.Num: //数値
                    var column = "VAL" + (maxVal + idx + 1);
                    var str = listData[column];
                    //単位を削除
                    var value = str.slice(0, str.indexOf("@"));

                    //対象データを更新
                    var updateData = {};
                    updateData["VAL" + SpecEdit.SpecDetail20.ColumnNo.Num] = value;
                    row.update(updateData);
                    break;

                case SpecType.NumMinMax: //数値(範囲)
                    var column = "VAL" + (maxVal + idx + 1);
                    var str = listData[column];
                    //単位を削除
                    var minmax = str.slice(0, str.indexOf("@"));
                    //最大、最小を分割
                    var min = minmax.split("|")[0];
                    var max = minmax.split("|")[1];

                    //対象データを更新
                    var updateData = {};
                    updateData["VAL" + SpecEdit.SpecDetail20.ColumnNo.NumMin] = min;
                    updateData["VAL" + SpecEdit.SpecDetail20.ColumnNo.NumMax] = max;
                    row.update(updateData);
                    break;

                case SpecType.Select: //コンボボックス
                    var column = "VAL" + (maxVal + idx + 1);
                    var value = listData[column];

                    //対象データを更新
                    var updateData = {};
                    updateData["VAL" + SpecEdit.SpecDetail20.ColumnNo.SelectStructure] = value;
                    row.update(updateData);
                    break;

                default:
                    break;
            }
        });

    }
    return true;
}
/*
 * 明細から削除済みでない行を取得する
 *  @return {element} 一覧の削除されていない行リスト
*/
function getRowsLank(ctrlId) {
    // 削除されていない行を取得
    var list = $(P_Article).find("div#" + ctrlId + getAddFormNo() + ".ctrlId").find("div:not([class^='tabulator-header'])").find("div.tabulator-row");
    return list;
}
/**
 * 点検種別ごとの表の罫線を調整する処理
 * 機器、点検種別、保全部位ごとに罫線で囲まれるようにする
 */
function setMaintKindListGroupingMachine() {

    // 点検種別でなければ終了
    var isDisplayMaintainanceKind = getValue(MachineDatail.MachineDatail50.Id, MachineDatail.MachineDatail50.ColumnNo.MaintLank, 0, CtrlFlag.ChkBox);
    isDisplayMaintainanceKind = convertStrToBool(isDisplayMaintainanceKind); // 変換
    if (!isDisplayMaintainanceKind) {
        return;
    }
    // 表示中の一覧を取得
    var list = getRowsLank(DatailManagementStandard.ScheduleLankList.Id);
    // キー値比較用
    var prevMachineId, prevKindId, prevContentId;
    var prevContent2Id;

    // 点検種別一覧から値を取得
    var getListKindValue = function (index, valNo) {
        var ctrlFlag = (valNo == DatailManagementStandard.ScheduleLankList.ColumnNo.MainteLankId) ? CtrlFlag.Combo : CtrlFlag.Label;
        var value = getValue(DatailManagementStandard.ScheduleLankList.Id, valNo, index, ctrlFlag);
        return value;
    }
    // グループの先頭かどうかを取得
    var getIsTop = function (prevValue, nowValue) {
        var isTop = false;
        if (prevValue != nowValue) {
            isTop = true;
        }
        return isTop;
    }
    // グループの末尾かどうかを取得
    var getIsBottom = function (index, list, nowValue, valNo) {
        var isBottom = false;
        var nextValue;
        if (index + 1 != list.length) {
            // 最終行でない場合
            nextValue = getListKindValue(index + 1, valNo);
        }
        if (nowValue != nextValue) {
            isBottom = true;
        }
        return isBottom;
    }
    // グループの状態に応じてスタイルを変更
    var setGroupToCols = function (index, targetCols, isTop, isBottom) {
        $.each(targetCols, function (colIndex, colVal) {
            var targetCell = getCtrl(DatailManagementStandard.ScheduleLankList.Id, colVal, index, CtrlFlag.Label);
            if (isTop) {
                // 先頭、上の罫線あり
                $(targetCell).removeClass("group-untop");
            } else {
                // 先頭でない、上の罫線なし
                $(targetCell).addClass("group-untop");
            }
            if (isBottom) {
                // 末尾、下の罫線あり
                $(targetCell).removeClass("group-unbottom");
            } else {
                // 末尾、下の罫線なし
                $(targetCell).addClass("group-unbottom");
            }
        });
    }
    // グループのスタイルを設定するための情報を取得
    var getGroupSetInfo = function (checkCol, prevValue, index, list) {
        var newPrevValue = prevValue;
        var nowValue = getListKindValue(index, checkCol);
        var isTop = getIsTop(prevValue, nowValue);
        if (isTop) {
            newPrevValue = nowValue;
        }
        var isBottom = getIsBottom(index, list, nowValue, checkCol);
        return [newPrevValue, isTop, isBottom];
    }

    // スケジュールのグループ化
    var setGroupToSchedules = function (row, isTop, isBottom) {
        // 行の各列に対し、スケジュールのもののみを取得
        var cols = $(row).find("div[tabulator-field]").filter(function (index, col) {
            // 列名(SELTAG,VAL1,YM202207など)
            var fieldName = $(col).attr("tabulator-field");
            // 年の場合、Y9999
            var isYear = fieldName.match(/Y\d{4}/);
            // 年月の場合、YM999999
            var isYearMonth = fieldName.match(/YM\d{6}/);
            // 年または年月ならスケジュール
            return isYear || isYearMonth;
        });
        $.each(cols, function (index, cell) {
            // 取得したスケジュール列に対しグループ化
            setGroupStyle(cell, isTop, isBottom);
        });
    }
    // セルに対し上下を判定しスタイルを指定
    var setGroupStyle = function (targetCell, isTop, isBottom) {
        if (isTop) {
            // 先頭、上の罫線あり
            $(targetCell).removeClass("group-untop");
        } else {
            // 先頭でない、上の罫線なし
            $(targetCell).addClass("group-untop");
        }
        if (isBottom) {
            // 末尾、下の罫線あり
            $(targetCell).removeClass("group-unbottom");
        } else {
            // 末尾、下の罫線なし
            $(targetCell).addClass("group-unbottom");
        }
    }

    $.each(list, function (index, row) {
        // 機器
        // 機器レベル、機器番号、機器名称、機器重要度を調整
        var [newPrevMachineId, isTopMachine, isBottomMachine] = getGroupSetInfo(DatailManagementStandard.ScheduleLankList.ColumnNo.MachineId, prevMachineId, index, list);
        prevMachineId = newPrevMachineId;
        var changeCols = [DatailManagementStandard.ScheduleLankList.ColumnNo.Level, DatailManagementStandard.ScheduleLankList.ColumnNo.MachineNo, DatailManagementStandard.ScheduleLankList.ColumnNo.MachineName, DatailManagementStandard.ScheduleLankList.ColumnNo.Importance];
        setGroupToCols(index, changeCols, isTopMachine, isBottomMachine);

        // 点検種別
        // 点検種別を調整
        var [newPrevKindId, isTopKind, isBottomKind] = getGroupSetInfo(DatailManagementStandard.ScheduleLankList.ColumnNo.MainteLankId, prevKindId, index, list);
        prevKindId = newPrevKindId;
        changeCols = [DatailManagementStandard.ScheduleLankList.ColumnNo.MainteLank, DatailManagementStandard.ScheduleLankList.ColumnNo.CycleYear, DatailManagementStandard.ScheduleLankList.ColumnNo.CycleMonth, DatailManagementStandard.ScheduleLankList.ColumnNo.CycleDay, DatailManagementStandard.ScheduleLankList.ColumnNo.CycleDisp, DatailManagementStandard.ScheduleLankList.ColumnNo.StartDate, DatailManagementStandard.ScheduleLankList.ColumnNo.ScheduleMante];
        setGroupToCols(index, changeCols, isTopMachine || isTopKind, isBottomMachine || isBottomKind);

        // スケジュール部分をグループ化
        setGroupToSchedules(row, isTopMachine || isTopKind, isBottomMachine || isBottomKind);

        //// 保全部位
        //// 保全部位を調整
        //var [newPrevContentId, isTopContent, isBottomContent] = getGroupSetInfo(DatailManagementStandard.ScheduleLankList.ColumnNo.Componet, prevContentId, index, list);
        //prevContentId = newPrevContentId;
        //changeCols = [DatailManagementStandard.ScheduleLankList.ColumnNo.Componet, DatailManagementStandard.ScheduleLankList.ColumnNo.PostImp, DatailManagementStandard.ScheduleLankList.ColumnNo.Method];
        //setGroupToCols(index, changeCols, isTopMachine || isTopContent, isBottomMachine || isBottomContent);

        // 保全部位
        // 保全部位を調整
        var [newPrevContentId, isTopContent, isBottomContent] = getGroupSetInfo(DatailManagementStandard.ScheduleLankList.ColumnNo.Componet, prevContentId, index, list);
        prevContentId = newPrevContentId;
        changeCols = [DatailManagementStandard.ScheduleLankList.ColumnNo.Componet];
        setGroupToCols(index, changeCols, isTopMachine || isTopContent, isBottomMachine || isBottomContent);

        // 保全部位重要度
        // 保全部位重要度を調整
        var [newPrevContent2Id, isTopContent2, isBottomContent2] = getGroupSetInfo(DatailManagementStandard.ScheduleLankList.ColumnNo.PostImp, prevContent2Id, index, list);
        prevContent2Id = newPrevContent2Id;
        changeCols = [DatailManagementStandard.ScheduleLankList.ColumnNo.PostImp, DatailManagementStandard.ScheduleLankList.ColumnNo.Method];
        setGroupToCols(index, changeCols, isTopMachine || isTopContent2, isBottomMachine || isBottomContent2);
    });
}

// スケジュールを表示する一覧
const MachineScheduleListIds = [DatailManagementStandard.ScheduleLankList.Id,
DatailManagementStandard.ScheduleList.Id, DatailLongPlan.LongPlanList.Id
];

/**
 * 【オーバーライド用関数】Tabuator一覧のヘッダー設定前処理
 * @param {string} appPath          ：アプリケーションルートパス
 * @param {string} id               ：一覧のID(#,_FormNo付き)
 * @param {object} header           ：ヘッダー情報
 * @param {Element} headerElement   ：ヘッダー要素
 */
function prevSetTabulatorHeader(appPath, id, header, headerElement) {
    // Tabuator一覧に個別で追加したい列等の設定が行えます

    //var compareIds = []
    //$.each(MachineScheduleListIds, function (index, value) {
    //    var settedId = "#" + value + getAddFormNo();
    //    compareIds.push(settedId);
    //});

    if (id == "#" + DatailManagementStandard.ScheduleLankList.Id + getAddFormNo()) {
        // スケジュール表示用ヘッダー情報の設定
        setScheduleHeaderInfo(appPath, header, headerElement, 1);
    }

    if (id == "#" + DatailManagementStandard.ScheduleList.Id + getAddFormNo()) {
        // スケジュール表示用ヘッダー情報の設定
        setScheduleHeaderInfo(appPath, header, headerElement, 2);
    }

    if (id == "#" + DatailLongPlan.LongPlanList.Id + getAddFormNo()) {
        // スケジュール表示用ヘッダー情報の設定
        setScheduleHeaderInfo(appPath, header, headerElement, 3);
    }

    //if (compareIds.indexOf(id) > -1) {
    //    // スケジュール表示用ヘッダー情報の設定
    //    setScheduleHeaderInfo(appPath, header, headerElement);
    //}

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



    // 機能IDが「予備品検索」ではない場合は何もしない
    if (conductId == SP0001_ConductId) {
        var machineId = getValueByOtherForm(MachineDatail.No, MachineDatail.MachineDatail20.Id, MachineDatail.MachineDatail20.ColumnNo.MachineId, 1, CtrlFlag.Label);

        // 予備品検索画面
        SP0001_passDataCmConduct(appPath, conductId, parentNo, conditionDataList, ctrlId, btn_ctrlId, rowNo, element, parentConductId, machineId);

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

    if (id == "#" + DatailConstitution.ParentList.Id + getAddFormNo()) {
        // 詳細画面 構成機器タブ 親子構成一覧
        changeDispCheckBox(DatailConstitution.ParentList);
    }
    else if (id == "#" + DatailConstitution.LoopList.Id + getAddFormNo()) {
        // 詳細画面 構成機器タブ ループ構成一覧
        changeDispCheckBox(DatailConstitution.LoopList);
    }
    else if (id == "#" + DatailLongPlan.LongPlanList.Id + getAddFormNo()) {
        setTimeout(function () {
            rowNoLinkChangeLongPlan();
        }, 50);
    }
    else if (id == "#" + DatailMaintainanceActivity.MaintainanceActivityList.Id + getAddFormNo()) {
        setTimeout(function () {
            rowNoLinkChangeMa();
        }, 50);
    }
    else if (id == "#" + DatailManagementStandard.ManagementStandardList.Id + getAddFormNo()) {
        //変更管理対象ならNoリンク非活性制御
        if (getIsHisotyManagement(false)) {
            //ブラウザに処理が戻った際に実行
            setTimeout(function () {
                setHideDetailLinkStandartList();
            }, 0);
        }
    }
}


/**
 * 【オーバーライド用関数】Tabulatorの列フィルター後の処理
 * @param {any} tbl 処理対象の一覧
 * @param {any} id 処理対象の一覧のID(#,_FormNo付き)
 */
function postTabulatorRenderCompleted(tbl, id) {

    if (id == "#" + DatailConstitution.ParentList.Id + getAddFormNo()) {
        // 詳細画面 構成機器タブ 親子構成一覧
        changeDispCheckBox(DatailConstitution.ParentList);
    }
    else if (id == "#" + DatailConstitution.LoopList.Id + getAddFormNo()) {
        // 詳細画面 構成機器タブ ループ構成一覧
        changeDispCheckBox(DatailConstitution.LoopList);
    }
    else if (id == "#" + DatailLongPlan.LongPlanList.Id + getAddFormNo()) {
        setTimeout(function () {
            rowNoLinkChangeLongPlan();
        }, 50);
    }
    else if (id == "#" + DatailMaintainanceActivity.MaintainanceActivityList.Id + getAddFormNo()) {
        setTimeout(function () {
            rowNoLinkChangeMa();
        }, 50);
    }
    else if (id == "#" + DatailManagementStandard.ManagementStandardList.Id + getAddFormNo()) {
        //変更管理対象ならNoリンク非活性制御
        if (getIsHisotyManagement(false)) {
            //ブラウザに処理が戻った際に実行
            setTimeout(function () {
                setHideDetailLinkStandartList();
            }, 0);
        }
    }
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

    // 予備品検索画面
    SP0001_clickComConductSelectBtn(appPath, cmConductId, cmArticle, btn, fromCtrlId, ConductId_MC0001);

}

/**
 * 点検種別毎スケジューリング一覧のラベル列/入力可能列の表示切替を行う
 * */
function hideColumnOfScheduleLankList(historyManagementFlg) {

    // 点検種別毎スケジューリング一覧
    var table = P_listData['#' + DatailManagementStandard.ScheduleLankList.Id + getAddFormNo()];
    // 変更管理対象か判定
    if (historyManagementFlg) {
        // 入力項目列を非表示にする
        hideColumnOfScheduleList(table, false, DatailManagementStandard.ScheduleLankList.ColumnNo.CycleYear);     // 周期(年)
        hideColumnOfScheduleList(table, false, DatailManagementStandard.ScheduleLankList.ColumnNo.CycleMonth);    // 周期(月)
        hideColumnOfScheduleList(table, false, DatailManagementStandard.ScheduleLankList.ColumnNo.CycleDay);      // 周期(日)
        hideColumnOfScheduleList(table, false, DatailManagementStandard.ScheduleLankList.ColumnNo.CycleDisp);     // 表示周期
        hideColumnOfScheduleList(table, false, DatailManagementStandard.ScheduleLankList.ColumnNo.StartDate);     // 開始日
        hideColumnOfScheduleList(table, false, DatailManagementStandard.ScheduleLankList.ColumnNo.ScheduleMante); // スケジュール管理

        // ラベル列を表示する
        hideColumnOfScheduleList(table, true, DatailManagementStandard.ScheduleLankList.ColumnNo.CycleYearLabel);     // 周期(年)
        hideColumnOfScheduleList(table, true, DatailManagementStandard.ScheduleLankList.ColumnNo.CycleMonthLabel);    // 周期(月)
        hideColumnOfScheduleList(table, true, DatailManagementStandard.ScheduleLankList.ColumnNo.CycleDayLabel);      // 周期(日)
        hideColumnOfScheduleList(table, true, DatailManagementStandard.ScheduleLankList.ColumnNo.CycleDispLabel);     // 表示周期
        hideColumnOfScheduleList(table, true, DatailManagementStandard.ScheduleLankList.ColumnNo.StartDateLabel);     // 開始日
        hideColumnOfScheduleList(table, true, DatailManagementStandard.ScheduleLankList.ColumnNo.ScheduleManteLabel); // スケジュール管理
    }
    else {
        // 入力項目列を表示する
        hideColumnOfScheduleList(table, true, DatailManagementStandard.ScheduleLankList.ColumnNo.CycleYear);     // 周期(年)
        hideColumnOfScheduleList(table, true, DatailManagementStandard.ScheduleLankList.ColumnNo.CycleMonth);    // 周期(月)
        hideColumnOfScheduleList(table, true, DatailManagementStandard.ScheduleLankList.ColumnNo.CycleDay);      // 周期(日)
        hideColumnOfScheduleList(table, true, DatailManagementStandard.ScheduleLankList.ColumnNo.CycleDisp);     // 表示周期
        hideColumnOfScheduleList(table, true, DatailManagementStandard.ScheduleLankList.ColumnNo.StartDate);     // 開始日
        hideColumnOfScheduleList(table, true, DatailManagementStandard.ScheduleLankList.ColumnNo.ScheduleMante); // スケジュール管理

        // ラベル列を非表示する
        hideColumnOfScheduleList(table, false, DatailManagementStandard.ScheduleLankList.ColumnNo.CycleYearLabel);     // 周期(年)
        hideColumnOfScheduleList(table, false, DatailManagementStandard.ScheduleLankList.ColumnNo.CycleMonthLabel);    // 周期(月)
        hideColumnOfScheduleList(table, false, DatailManagementStandard.ScheduleLankList.ColumnNo.CycleDayLabel);      // 周期(日)
        hideColumnOfScheduleList(table, false, DatailManagementStandard.ScheduleLankList.ColumnNo.CycleDispLabel);     // 表示周期
        hideColumnOfScheduleList(table, false, DatailManagementStandard.ScheduleLankList.ColumnNo.StartDateLabel);     // 開始日
        hideColumnOfScheduleList(table, false, DatailManagementStandard.ScheduleLankList.ColumnNo.ScheduleManteLabel); // スケジュール管理
    }
}

/**
 * スケジューリング一覧のラベル列/入力可能列の表示切替を行う
 * */
function hideColumnOfScheduleBaseList(historyManagementFlg) {

    // スケジューリング一覧
    var table = P_listData['#' + DatailManagementStandard.ScheduleList.Id + getAddFormNo()];

    // 変更管理対象か判定
    if (historyManagementFlg) {
        // 入力項目列を非表示にする
        hideColumnOfScheduleList(table, false, DatailManagementStandard.ScheduleList.ColumnNo.CycleYear);     // 周期(年)
        hideColumnOfScheduleList(table, false, DatailManagementStandard.ScheduleList.ColumnNo.CycleMonth);    // 周期(月)
        hideColumnOfScheduleList(table, false, DatailManagementStandard.ScheduleList.ColumnNo.CycleDay);      // 周期(日)
        hideColumnOfScheduleList(table, false, DatailManagementStandard.ScheduleList.ColumnNo.CycleDisp);     // 表示周期
        hideColumnOfScheduleList(table, false, DatailManagementStandard.ScheduleList.ColumnNo.StartDate);     // 開始日
        hideColumnOfScheduleList(table, false, DatailManagementStandard.ScheduleList.ColumnNo.ScheduleMante); // スケジュール管理

        // ラベル列を表示する
        hideColumnOfScheduleList(table, true, DatailManagementStandard.ScheduleList.ColumnNo.CycleYearLabel);     // 周期(年)
        hideColumnOfScheduleList(table, true, DatailManagementStandard.ScheduleList.ColumnNo.CycleMonthLabel);    // 周期(月)
        hideColumnOfScheduleList(table, true, DatailManagementStandard.ScheduleList.ColumnNo.CycleDayLabel);      // 周期(日)
        hideColumnOfScheduleList(table, true, DatailManagementStandard.ScheduleList.ColumnNo.CycleDispLabel);     // 表示周期
        hideColumnOfScheduleList(table, true, DatailManagementStandard.ScheduleList.ColumnNo.StartDateLabel);     // 開始日
        hideColumnOfScheduleList(table, true, DatailManagementStandard.ScheduleList.ColumnNo.ScheduleManteLabel); // スケジュール管理
    }
    else {
        // 入力項目列を表示する
        hideColumnOfScheduleList(table, true, DatailManagementStandard.ScheduleList.ColumnNo.CycleYear);     // 周期(年)
        hideColumnOfScheduleList(table, true, DatailManagementStandard.ScheduleList.ColumnNo.CycleMonth);    // 周期(月)
        hideColumnOfScheduleList(table, true, DatailManagementStandard.ScheduleList.ColumnNo.CycleDay);      // 周期(日)
        hideColumnOfScheduleList(table, true, DatailManagementStandard.ScheduleList.ColumnNo.CycleDisp);     // 表示周期
        hideColumnOfScheduleList(table, true, DatailManagementStandard.ScheduleList.ColumnNo.StartDate);     // 開始日
        hideColumnOfScheduleList(table, true, DatailManagementStandard.ScheduleList.ColumnNo.ScheduleMante); // スケジュール管理

        // ラベル列を非表示する
        hideColumnOfScheduleList(table, false, DatailManagementStandard.ScheduleList.ColumnNo.CycleYearLabel);     // 周期(年)
        hideColumnOfScheduleList(table, false, DatailManagementStandard.ScheduleList.ColumnNo.CycleMonthLabel);    // 周期(月)
        hideColumnOfScheduleList(table, false, DatailManagementStandard.ScheduleList.ColumnNo.CycleDayLabel);      // 周期(日)
        hideColumnOfScheduleList(table, false, DatailManagementStandard.ScheduleList.ColumnNo.CycleDispLabel);     // 表示周期
        hideColumnOfScheduleList(table, false, DatailManagementStandard.ScheduleList.ColumnNo.StartDateLabel);     // 開始日
        hideColumnOfScheduleList(table, false, DatailManagementStandard.ScheduleList.ColumnNo.ScheduleManteLabel); // スケジュール管理
    }
}

/**
 * 機器別管理基準タブ スケジュール一覧の列表示/非表示
 * @param {any} table 制御を行う一覧
 * @param {any} isDisp 表示する場合True
 * @param {any} ColNo 列番号
 */
function hideColumnOfScheduleList(table, isDisp, ColNo) {

    if (isDisp) {
        table.showColumn("VAL" + ColNo);
    }
    else {
        table.hideColumn("VAL" + ColNo);
    }

}