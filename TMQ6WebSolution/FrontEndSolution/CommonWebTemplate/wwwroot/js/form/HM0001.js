/* ========================================================================
 *  機能名　    ：   【HM0001】機器台帳変更管理
 * ======================================================================== */

/**
 * 自身の相対パスを取得
 */
function getPath() {
    var root;
    var scripts = document.getElementsByTagName("script");
    var i = scripts.length;
    while (i--) {
        var match = scripts[i].src.match(/(^|.*\/)HM0001\.js$/);
        if (match) {
            root = match[1];
            break;
        }
    }
    return root;
}
document.write("<script src=\"" + getPath() + "/tmqcommon.js\"></script>");
document.write("<script src=\"" + getPath() + "/HM0003.js\"></script>");
document.write("<script src=\"" + getPath() + "/HM0004.js\"></script>");

// 一覧画面 コントロール番号
const FormList = {
    No: 0, // 画面番号
    Filter: // フィルタ
    {
        Id: "BODY_010_00_LST_0", // コントロールグループID
        ColumnNo: 1              // 項目番号
    },
    SearchCondition: { // 検索条件
        Id: "BODY_020_00_LST_0", // コントロールグループID
        ColumnNo: {
            IsApprovalUser: 2    // 承認権限の有無
        }
    },
    List: { // 検索結果一覧
        Id: "BODY_040_00_LST_0",         // コントロールグループID
        ColumnNo: {                      // 項目番号
            MachineNo: 1,                // 機器番号
            MachineName: 2,              // 機器名称
            EquipmentLevel: 3,           // 機器レベル
            District: 4,                 // 地区
            Factory: 5,                  // 工場
            Plant: 6,                    // プラント
            Series: 7,                   // 系列
            Stroke: 8,                   // 工程
            Facility: 9,                 // 設備
            Importance: 10,              // 重要度
            Conservation: 11,            // 保全方式
            InstallationLocation: 12,    // 設置場所
            NumberOfInstallation: 13,    // 設置台数
            DateOfInstallation: 14,      // 設置年月
            ApplicableLaws: 15,          // 適用法規
            MachineNote: 16,             // 機番メモ
            Job: 17,                     // 職種
            Large: 18,                   // 機種大分類
            Middle: 19,                  // 機種中分類
            Small: 20,                   // 機種小分類
            Manufacturer: 21,            // メーカー
            ManufacturerType: 22,        // メーカー型式
            ModelNo: 23,                 // 型式コード
            SerialNo: 24,                // 製造番号
            DateOfManufacture: 25,       // 製造年月
            DeliveryDate: 26,            // 納期
            UseSegment: 27,              // 使用区分
            FixedAssetNo: 30,            // 固定資産番号
            EquipmentNote: 31,           // 機器メモ
            Component: 32,               // 機器別管理基準変更有無
            ApplicationDivisionCode: 38, // 申請区分(拡張項目)
            ValueChanged: 39             // 変更のあった項目
        }
    },
    Button: {                                 // ボタンコントロールID
        New: "New",                           // 新規申請
        ApprovalAllTrans: "ApprovalAllTrans", // 一括承認(画面遷移)※選択行チェックに使用
        DenialAllTrans: "DenialAllTrans",     // 一括否認(画面遷移)※選択行チェックに使用
        ApprovalAll: "ApprovalAll",           // 一括承認(実行)
        DenialAll: "DenialAll",               // 一括否認(実行)
        Output: "Output"                      // 出力
    }
}

// 詳細画面 コントロール番号
const FormDetail = {
    No: 1, // 画面番号
    HistoryManagementInfo: { // 変更管理情報
        Info: { // 申請状況～承認者
            Id: "BODY_100_00_LST_1",    // コントロールグループID
            ColumnNo: {                 // 項目番号
                ApplicationDivision: 2, // 申請区分
                ApprovalUser: 3         // 承認者
            }
        },
        Reason: { // 申請理由～否認理由
            Id: "BODY_110_00_LST_1", // コントロールグループID
        },
        CssClass: {
            HistoryInfoTitle: "HistoryInfoTitle" // 一覧タイトルと折り畳みアイコンを非表示にするためのCSSクラス
        }

    },
    MachineInfo: { // 機番情報
        Location: { // 地区～設備
            Id: "BODY_020_00_LST_1", // コントロールグループID
            ColumnNo: {              // 項目番号
                District: 1,         // 地区
                Factory: 2,          // 工場
                Plant: 3,            // プラント
                Series: 4,           // 系列
                Stroke: 5,           // 工程
                Facility: 6          // 設備
            }
        },
        Machine: { // 機器番号～保全方式
            Id: "BODY_030_00_LST_1",                   // コントロールグループID
            ColumnNo: {                                // 項目番号
                MachineNo: 1,                          // 機器番号
                MachineName: 2,                        // 機器名称
                EquipmentLevel: 3,                     // 機器レベル
                InstallationLocation: 4,               // 設置場所
                NumberOfInstallation: 5,               // 設置台数
                DateOfInstallation: 6,                 // 設置年月
                Importance: 7,                         // 重要度
                Conservation: 8,                       // 保全方式
                ValueChanged: 9,                       // 変更のあった項目
                ApplicationDivisionCode: 10,           // 申請区分(拡張項目)
                ApplicationStatusCode: 11,             // 申請状況(拡張項目)
                MachineId: 12,                         // 機番ID
                HistoyManagementId: 13,                // 変更管理ID
                ProcessMode: 14,                       // 処理モード(0：トランザクションモード、1：変更管理モード)
                IsCertified: 15,                       // 申請の申請者またはシステム管理者の場合はTrue
                IsCertifiedFactory: 16,                // 変更管理IDが紐付く機番情報の場所階層IDに設定されている工場の拡張項目がログインユーザIDの場合はTrue
                StatusCntExceptApproved: 23,           // キーIDに紐付く「承認済」以外のデータ有無
                TabChangeCnt: 24                       // タブ切替回数(背景色変更などに使用)
            }
        },
        Reason: { // 適用法規～否認理由
            Id: "BODY_040_00_LST_1",  // コントロールグループID
            ColumnNo: {               // 項目番号
                ApplicableLaws: 1,    // 適用法規
                MachineNote: 2,       // 機番メモ
                ApplicationReason: 3, // 申請理由
                RejectionReason: 4    // 否認理由
            }
        },
        CssClass: {
            Condition_HM0001: ".condition_HM0001" // 機番情報に付与しているCSSクラス(保全項目一覧の登録に使用)
        }
    },
    EquipmentInfo: { // 機器情報
        TabNo: 1, // タブ番号
        Job: { // 職種～機種小分類
            Id: "BODY_050_00_LST_1", // コントロールグループID
            ColumnNo: {              // 項目番号
                Job: 1,              // 職種
                Large: 2,            // 機種大分類
                Middle: 3,           // 機種中分類
                Small: 4             // 機種小分類
            }
        },
        Equipment: { // 使用区分～点検種別毎管理
            Id: "BODY_060_00_LST_1",       // コントロールグループID
            ColumnNo: {                    // 項目番号
                UseSegment: 1,             // 使用区分
                CirculationTargetFlg: 2,   // 循環対象
                FixedAssetNo: 3,           // 固定資産番号
                Manufacturer: 4,           // メーカー
                ManufacturerType: 5,       // メーカー型式
                ModelNo: 6,                // 型式コード
                SerialNo: 7,               // 製造番号
                DateOfManufacture: 8,      // 製造年月
                DeliveryDate: 9,           // 納期
                MaintainanceKindManage: 10 // 点検種別毎管理
            }
        },
        EquipmentNote: { // 機器メモ
            Id: "BODY_070_00_LST_1", // コントロールグループID
            ColumnNo: {              // 項目番号
                EquipmentNote: 1     // 機器メモ
            }
        },
    },
    ManagementStandardsList: { // 保全項目一覧
        Id: "BODY_080_00_LST_1",                // コントロールグループID
        TabNo: 2,                               // タブ番号
        ColumnNo: {                             // 項目番号
            EquipmentLevel: 1,                  // 機器レベル
            MachineNo: 2,                       // 機器番号
            MachineName: 3,                     // 機器名称
            InspectionSite: 25,                 // 保全部位
            InspectionSiteImportance: 5,        // 部位重要度
            InspectionSiteConservation: 6,      // 保全方式
            MaintainanceDivision: 7,            // 保全区分
            InspectionContent: 26,              // 保全項目
            MaintainanceKind: 9,                // 点検種別
            BudgetAmount: 10,                   // 予算金額
            ScheduleType: 11,                   // スケジュール管理
            PreparationPeriod: 12,              // 準備期間(日)
            CycleYear: 13,                      // 周期(年)
            CycleMonth: 14,                     // 周期(月)
            CycleDay: 15,                       // 周期(日)
            DispCycle: 16,                      // 表示周期
            StartDate: 17,                      // 開始日
            ManagementStandardsComponentId: 19, // 機器別管理基準部位ID
            ManagementStandardsContentId: 20,   // 機器別管理基準内容ID
            MaintainanceScheduleId: 21,         // 保全スケジュールID
            MachineId: 23,                      // 機番ID
            ApplicationDivisionCode: 27,        // 申請区分(拡張項目)
            ValueChanged: 28,                   // 変更のあった項目
            EquipmentLevelLabel: 38              // 機器レベル(コンボ)
        }
    },
    Button: {                                                   // ボタンコントロールID
        CopyRequest: "CopyRequest",                             // 複写申請
        ChangeRequest: "ChangeRequest",                         // 変更申請
        DeleteRequest: "DeleteRequest",                         // 削除申請
        ChangeApplicationRequest: "ChangeApplicationRequest",   // 承認依頼
        EditRequest: "EditRequest",                             // 申請内容修正
        CancelRequest: "CancelRequest",                         // 申請内容取消
        PullBackRequest: "PullBackRequest",                     // 承認依頼引戻
        ChangeApplicationApproval: "ChangeApplicationApproval", // 承認
        ChangeApplicationDenial: "ChangeApplicationDenial",     // 否認
        BeforeChange: "BeforeChange",                           // 変更前
        ManagementStandardRegist: "ManagementStandardRegist",   // 保全項目一覧 登録
        Deletet: "Delete"                                       // 保全項目一覧 非表示 削除
    },
    CssClass: {
        displayNone: "displayNone" // ボタンの非表示
    }
}

// 詳細編集画面 コントロール番号
const FormEdit = {
    No: 2, // 画面番号

    MachineInfo: { // 機番情報
        Machine: { // 機器番号～保全方式
            Id: "BODY_010_00_LST_2" // コントロールグループID
        }
    },
    Button: { // ボタンコントロールID
        Regist: "Regist", // 登録
    }
}

// 詳細画面の 変更前 ボタンクリック時に表示する機器台帳詳細画面(MC0001)のタブ番号
const tabNoToMC0001 = 1;

/**
 * 【オーバーライド用関数】
 *  初期化処理(表示中画面用)
 * 
 * @param {any} appPath      ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 * @param {any} conductId    ：機能ID
 * @param {any} formNo       ：画面番号
 * @param {any} articleForm  ：表示画面ｴﾘｱ要素
 * @param {any} curPageStatus：画面表示ｽﾃｰﾀｽ
 * @param {any} actionCtrlId ：Action(ﾎﾞﾀﾝなど)CTRLID
 * @param {any} data          :初期表示ﾃﾞｰﾀ
 */
function initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data) {

    //申請状況変更画面の初期化処理
    HM0003_initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data);
    //帳票出力画面の初期化処理
    HM0004_initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data);

    // 画面番号を判定
    if (formNo == FormList.No) { // 一覧画面

        //一括承認ボタン、一括否認ボタンの表示制御
        var isApprovalUser = getValue(FormList.SearchCondition.Id, FormList.SearchCondition.ColumnNo.IsApprovalUser, 1, CtrlFlag.Label, false, false);
        setHideButton(FormList.Button.ApprovalAllTrans, !convertStrToBool(isApprovalUser));
        setHideButton(FormList.Button.DenialAllTrans, !convertStrToBool(isApprovalUser));

        //フォーカス設定(新規申請ボタンが不可の場合、出力ボタン)
        setFocusButtonAvailable(FormList.Button.New, FormList.Button.Output)
    }
    else if (formNo == FormDetail.No) { // 詳細画面

        // 変更管理情報の一覧タイトルと折り畳みアイコンを非表示にする
        var historyInfoTitle = $($(P_Article).find("#detail_divid_1").find(".title")[0]).parent().find("a");
        historyInfoTitle.addClass(FormDetail.HistoryManagementInfo.CssClass.HistoryInfoTitle);

        // タブ切替回数を「0」にする
        setValue(FormDetail.MachineInfo.Machine.Id, FormDetail.MachineInfo.Machine.ColumnNo.TabChangeCnt, 1, CtrlFlag.Label, 0, false, false);

        // ボタン非表示
        var condition = getConditionToHideButton();
        commonButtonHideHistory(condition.isTransactionMode, condition.applicationStatusCode, condition.applicationDivisionCode, condition.isCertified, condition.isCertifiedFactory);

        //申請区分、承認者、申請理由、否認理由を一度すべて表示する
        changeColumnDisplay(FormDetail.HistoryManagementInfo.Info.Id, FormDetail.HistoryManagementInfo.Info.ColumnNo.ApplicationDivision, true);
        changeColumnDisplay(FormDetail.HistoryManagementInfo.Info.Id, FormDetail.HistoryManagementInfo.Info.ColumnNo.ApprovalUser, true);
        changeListDisplay(FormDetail.HistoryManagementInfo.Reason.Id, true);

        // 変更管理情報 非表示
        if (condition.isTransactionMode) {
            //申請区分、承認者、申請理由、否認理由を非表示
            changeColumnDisplay(FormDetail.HistoryManagementInfo.Info.Id, FormDetail.HistoryManagementInfo.Info.ColumnNo.ApplicationDivision, false);
            changeColumnDisplay(FormDetail.HistoryManagementInfo.Info.Id, FormDetail.HistoryManagementInfo.Info.ColumnNo.ApprovalUser, false);
            changeListDisplay(FormDetail.HistoryManagementInfo.Reason.Id, false);
        }

        // 背景色変更処理
        changeBackGroundColorHistoryDetail(condition.applicationDivisionCode, getColumnList(), FormDetail.MachineInfo.Machine.Id, FormDetail.MachineInfo.Machine.ColumnNo.ValueChanged);

        // ボタン押下可能なボタンにフォーカスをセット
        var processMode = getValue(FormDetail.MachineInfo.Machine.Id, FormDetail.MachineInfo.Machine.ColumnNo.ProcessMode, 1, CtrlFlag.Label, false, false);
        setFocusButtonHistory(processMode);

        // 「機器台帳」タブクリック時イベントを設定
        var machineTab = $(P_Article).find("a[data-tabno='" + FormDetail.EquipmentInfo.TabNo + "']")[0];
        $(machineTab).click(function () {
            // タブ切替回数を「0」にする
            setValue(FormDetail.MachineInfo.Machine.Id, FormDetail.MachineInfo.Machine.ColumnNo.TabChangeCnt, 1, CtrlFlag.Label, 0, false, false);
        });
    }
    else if (formNo == FormEdit.No) {

        // 登録ボタンにフォーカス
        setFocusButton(FormEdit.Button.Regist);
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

    var conditionDataList = [];

    // 一覧フィルタ処理実施
    if (executeListFilter(transTarget, FormList.List.Id, FormList.Filter.Id, FormList.Filter.ColumnNo)) {
        return [false, conditionDataList];
    }

    // 画面番号を判定
    if (formNo == FormList.No) { // 一覧画面

        // 一括承認(画面遷移)、一括否認(画面遷移)がクリックされた場合
        if (btn_ctrlId == FormList.Button.ApprovalAllTrans || btn_ctrlId == FormList.Button.DenialAllTrans) {

            // 一覧にチェックされた行が存在しない場合、遷移をキャンセル
            if (!isCheckedList(FormList.List.Id)) {
                return [false, conditionDataList];
            }
        }

        // クリックされたボタンを判定
        if (btn_ctrlId == FormList.Button.ApprovalAllTrans) { // 一括承認(画面遷移)

            // 遷移をキャンセルして一括承認処理実行
            $(getButtonCtrl(FormList.Button.ApprovalAll)).click();
            return [false, conditionDataList];
        }
        else if (btn_ctrlId == FormList.Button.DenialAllTrans) { // 一括否認(画面遷移)

            // 画面遷移をキャンセルして一括否認処理実行
            $(getButtonCtrl(FormList.Button.DenialAll)).click();
            return [false, conditionDataList];
        }
        else if (btn_ctrlId == FormList.Button.Output) {
            //出力押下時、変更管理帳票出力画面(HM0004)を開くための条件を設定する
            conditionDataList = getParamToHM0004(ConductId_HM0001);
        }
    }
    else if (formNo == FormDetail.No) { // 詳細画面

        // クリックされたボタンを判定
        if (btn_ctrlId == FormDetail.Button.BeforeChange) { // 変更前

            // 別タブで機器台帳詳細画面(MC0001)を開くための検索条件を設定する(機番ID：非表示項目を取得、タブ番号：1)
            conditionDataList = getParamToMC0001(getValue(FormDetail.MachineInfo.Machine.Id, FormDetail.MachineInfo.Machine.ColumnNo.MachineId, 1, CtrlFlag.Label, false, false), tabNoToMC0001);
        }
        else if (btn_ctrlId == FormDetail.Button.CopyRequest) { // 複写申請

            // 詳細編集画面の検索のための情報を設定
            conditionDataList = getListDataByCtrlIdList([FormDetail.MachineInfo.Machine.Id], FormDetail.No, 0);
        }
        else if (btn_ctrlId == FormDetail.Button.ChangeRequest) { // 変更申請

            // 排他チェック(機器に変更管理が紐付いている場合はエラー)
            if (!isExistsHistoryManagement(appPath, formNo)) {
                return [false, conditionDataList];
            }

            // 詳細編集画面の検索のための情報を設定
            conditionDataList = getListDataByCtrlIdList([FormDetail.MachineInfo.Machine.Id], FormDetail.No, 0);
        }
        else if (btn_ctrlId == FormDetail.Button.EditRequest) { // 申請内容修正

            // 変更管理テーブルの排他チェック エラーの場合は遷移しない
            if (!checkHisoryManagemtntExclusive(appPath, formNo)) {
                return [false, conditionDataList];
            }

            // 詳細編集画面の検索のための情報を設定
            conditionDataList = getListDataByCtrlIdList([FormDetail.MachineInfo.Machine.Id], FormDetail.No, 0);
        }
        else if (btn_ctrlId == FormDetail.Button.ChangeApplicationRequest || btn_ctrlId == FormDetail.Button.ChangeApplicationDenial) { // 承認依頼・否認

            // 変更管理テーブルの排他チェック エラーの場合は遷移しない
            if (!checkHisoryManagemtntExclusive(appPath, formNo)) {
                return [false, conditionDataList];
            }

            // 詳細申請状況変更画面(HM0003)を開くための条件を設定する
            var historyManagementId = getValue(FormDetail.MachineInfo.Machine.Id, FormDetail.MachineInfo.Machine.ColumnNo.HistoyManagementId, 1, CtrlFlag.Label, false, false);
            conditionDataList = getParamToHM0003(btn_ctrlId == FormDetail.Button.ChangeApplicationRequest, historyManagementId);

        }
        else if ((!btn_ctrlId || btn_ctrlId == "") && transTarget == FormDetail.ManagementStandardsList.Id) { // 保全項目一覧の単票に遷移

            // 変更管理IDを取得する
            var historyManagementId = parseInt(getValue(FormDetail.MachineInfo.Machine.Id, FormDetail.MachineInfo.Machine.ColumnNo.HistoyManagementId, 1, CtrlFlag.Label, false, false));
            if (historyManagementId <= 0) {

                // 変更管理テーブルの排他チェック エラーの場合は遷移しない
                if (!isExistsHistoryManagement(appPath, formNo)) {
                    return [false, conditionDataList];
                }
            }
        }
    }

    return [true, conditionDataList];
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
    if (formNo == FormEdit.No && btnCtrlId == FormEdit.Button.Regist) {
        //新規登録画面から登録後、参照画面に渡すキー情報をセット
        var conditionDataList = getListDataByCtrlIdList([FormEdit.MachineInfo.Machine.Id], FormEdit.No, 0);
        // 一覧から参照へ遷移する場合と同様に、参照画面の検索条件を追加
        setSearchCondition(ConductId_HM0001, FormDetail.No, conditionDataList);
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

    //申請状況変更画面の実行後処理
    HM0003_postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data);

    if (formNo == FormDetail.No) {

        // 申請内容取消・承認後は再検索しないため何もしない
        if (btn[0].attributes.name.value == FormDetail.Button.CancelRequest ||
            btn[0].attributes.name.value == FormDetail.Button.ChangeApplicationApproval) {
            return;
        }

        // タブ切替回数を「0」にする
        setValue(FormDetail.MachineInfo.Machine.Id, FormDetail.MachineInfo.Machine.ColumnNo.TabChangeCnt, 1, CtrlFlag.Label, 0, false, false);

        // ボタン非表示
        var condition = getConditionToHideButton();
        commonButtonHideHistory(condition.isTransactionMode, condition.applicationStatusCode, condition.applicationDivisionCode, condition.isCertified, condition.isCertifiedFactory);

        //申請区分、承認者、申請理由、否認理由を一度すべて表示する
        changeColumnDisplay(FormDetail.HistoryManagementInfo.Info.Id, FormDetail.HistoryManagementInfo.Info.ColumnNo.ApplicationDivision, true);
        changeColumnDisplay(FormDetail.HistoryManagementInfo.Info.Id, FormDetail.HistoryManagementInfo.Info.ColumnNo.ApprovalUser, true);
        changeListDisplay(FormDetail.HistoryManagementInfo.Reason.Id, true);

        // 変更管理情報 非表示
        if (condition.isTransactionMode) {
            //申請区分、承認者、申請理由、否認理由を非表示
            changeColumnDisplay(FormDetail.HistoryManagementInfo.Info.Id, FormDetail.HistoryManagementInfo.Info.ColumnNo.ApplicationDivision, false);
            changeColumnDisplay(FormDetail.HistoryManagementInfo.Info.Id, FormDetail.HistoryManagementInfo.Info.ColumnNo.ApprovalUser, false);
            changeListDisplay(FormDetail.HistoryManagementInfo.Reason.Id, false);
        }

        // 背景色変更処理
        changeBackGroundColorHistoryDetail(condition.applicationDivisionCode, getColumnList(), FormDetail.MachineInfo.Machine.Id, FormDetail.MachineInfo.Machine.ColumnNo.ValueChanged);

        // ボタン押下可能なボタンにフォーカスをセット
        var processMode = getValue(FormDetail.MachineInfo.Machine.Id, FormDetail.MachineInfo.Machine.ColumnNo.ProcessMode, 1, CtrlFlag.Label, false, false);
        setFocusButtonHistory(processMode);
    }
}

/**
 * 【オーバーライド用関数】Tabulatorの描画が完了時の処理
 * @param {any} tbl 一覧
 * @param {any} id 一覧のID(#,_FormNo付き)
 */
function postBuiltTabulator(tbl, id) {

    // 描画された一覧を判定
    if (id == "#" + FormList.List.Id + getAddFormNo()) { // 一覧画面 機器台帳一覧

        // 背景色変更処理
        commonChangeBackGroundColorHistory(tbl, FormList.List.ColumnNo.ApplicationDivisionCode, FormList.List.ColumnNo.ValueChanged, FormList.List.ColumnNo);
    }
    else if (id == "#" + FormDetail.ManagementStandardsList.Id + getAddFormNo()) { // 詳細画面 保全項目一覧

        // 機器別管理基準タブが選択されている場合、背景色変更処理
        if (isTabManagementStandardsSelected()) {
            commonChangeBackGroundColorHistory(tbl, FormDetail.ManagementStandardsList.ColumnNo.ApplicationDivisionCode, FormDetail.ManagementStandardsList.ColumnNo.ValueChanged, FormDetail.ManagementStandardsList.ColumnNo);

            //　行追加・行削除アイコン非表示
            var condition = getConditionToHideButton();
            hideCtrlManagementStandardsList(condition);
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

    // 描画された一覧を判定
    if (id == "#" + FormList.List.Id + getAddFormNo()) { // 一覧画面 機器台帳一覧

        // 背景色変更処理
        commonChangeBackGroundColorHistory(tbl, FormList.List.ColumnNo.ApplicationDivisionCode, FormList.List.ColumnNo.ValueChanged, FormList.List.ColumnNo);
    }
    else if (id == "#" + FormDetail.ManagementStandardsList.Id + getAddFormNo()) { // 詳細画面 保全項目一覧

        // 背景色変更処理
        commonChangeBackGroundColorHistory(tbl, FormDetail.ManagementStandardsList.ColumnNo.ApplicationDivisionCode, FormDetail.ManagementStandardsList.ColumnNo.ValueChanged, FormDetail.ManagementStandardsList.ColumnNo);
    }
}

/**
 * 【オーバーライド用関数】Tabulatorの列フィルター後の処理
 * @param {any} tbl 処理対象の一覧
 * @param {any} id 処理対象の一覧のID(#,_FormNo付き)
 * @param {any} filters フィルターの条件
 * @param {any} rows フィルター後の行rows
 */
function postTabulatorDataFiltered(tbl, id, filters, rows) {

    // 描画された一覧を判定
    if (id == "#" + FormList.List.Id + getAddFormNo()) { // 一覧画面 機器台帳一覧

        // 背景色変更処理
        commonChangeBackGroundColorHistory(tbl, FormList.List.ColumnNo.ApplicationDivisionCode, FormList.List.ColumnNo.ValueChanged, FormList.List.ColumnNo, rows);
    }
    else if (id == "#" + FormDetail.ManagementStandardsList.Id + getAddFormNo()) { // 詳細画面 保全項目一覧

        // 背景色変更処理
        commonChangeBackGroundColorHistory(tbl, FormDetail.ManagementStandardsList.ColumnNo.ApplicationDivisionCode, FormDetail.ManagementStandardsList.ColumnNo.ValueChanged, FormDetail.ManagementStandardsList.ColumnNo);
    }
}

/**
 *【オーバーライド用関数】登録前追加条件取得処理
 *  @param appPath       ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param conductId     ：機能ID
 *  @param formNo        ：画面番号
 *  @param btn           ：押下されたボタン要素
 */
function addSearchConditionDictionaryForRegist(appPath, conductId, formNo, btn) {

    var conditionDataList = [];

    // 詳細画面 保全項目一覧の単票の登録ボタンの場合
    if (formNo == FormDetail.No && btn[0].attributes.name.value == FormDetail.Button.ManagementStandardRegist ||
        formNo == FormDetail.No && btn[0].attributes.name.value == FormDetail.Button.Deletet) {

        // 機番情報・機器情報の一覧を独自のCSSクラスを指定して取得
        var targetElements = [];
        targetElements.push($(FormDetail.MachineInfo.CssClass.Condition_HM0001)[0]);
        targetElements.push($(FormDetail.MachineInfo.CssClass.Condition_HM0001)[1]);
        targetElements.push($(FormDetail.MachineInfo.CssClass.Condition_HM0001)[2]);
        targetElements.push($(FormDetail.MachineInfo.CssClass.Condition_HM0001)[3]);
        targetElements.push($(FormDetail.MachineInfo.CssClass.Condition_HM0001)[4]);
        targetElements.push($(FormDetail.MachineInfo.CssClass.Condition_HM0001)[5]);
        targetElements.push(($(P_Article).find("div #" + FormDetail.ManagementStandardsList.Id + getAddFormNo())[0]));
        return getListDataElements(targetElements, formNo, 0);
    }

    // それ以外の場合
    return conditionDataList;
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

    // 共通-申請状況変更画面の画面再表示前処理
    // 共通-申請状況変更画面を閉じたときの再表示処理はこちらで行うので、各機能での呼出は不要
    HM0003_beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom);

    // 保全項目一覧の単票で登録された場合 初期化処理を行う
    if (btnCtrlId == FormDetail.Button.ManagementStandardRegist && targetCtrlId == FormDetail.ManagementStandardsList.Id) {

        initFormData(appPath, conductId, pgmId, formNo, btnCtrlId, conductPtn, selectData, listData, status);
    }
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

    // 詳細画面の保全項目一覧で単票が開かれた場合
    if (formNo = FormDetail.No && transPtn == transPtnDef.Edit && transTarget == FormDetail.ManagementStandardsList.Id) {

        //行追加アイコンが詳細リンクか判定
        if (rowNo == -1) {

            // 詳細画面より機器の情報を取得して設定する
            // 機器レベル
            var equipmentLevel = getValueByOtherForm(FormDetail.No, FormDetail.MachineInfo.Machine.Id, FormDetail.MachineInfo.Machine.ColumnNo.EquipmentLevel, 1, CtrlFlag.Combo);
            setValue(FormDetail.ManagementStandardsList.Id, FormDetail.ManagementStandardsList.ColumnNo.EquipmentLevelLabel, 1, CtrlFlag.Combo, equipmentLevel, true, false);

            // 機器番号
            var machineNo = getValueByOtherForm(FormDetail.No, FormDetail.MachineInfo.Machine.Id, FormDetail.MachineInfo.Machine.ColumnNo.MachineNo, 1, CtrlFlag.TextBox);
            setValue(FormDetail.ManagementStandardsList.Id, FormDetail.ManagementStandardsList.ColumnNo.MachineNo, 1, CtrlFlag.TextBox, machineNo, true, false);

            // 機器名称
            var machineName = getValueByOtherForm(FormDetail.No, FormDetail.MachineInfo.Machine.Id, FormDetail.MachineInfo.Machine.ColumnNo.MachineName, 1, CtrlFlag.TextBox);
            setValue(FormDetail.ManagementStandardsList.Id, FormDetail.ManagementStandardsList.ColumnNo.MachineName, 1, CtrlFlag.TextBox, machineName, true, false);

            // 機番ID
            var machineId = getValueByOtherForm(FormDetail.No, FormDetail.MachineInfo.Machine.Id, FormDetail.MachineInfo.Machine.ColumnNo.MachineId, 1, CtrlFlag.Label);
            setValue(FormDetail.ManagementStandardsList.Id, FormDetail.ManagementStandardsList.ColumnNo.MachineId, 1, CtrlFlag.Label, machineId, true, false);

            // キー項目に「-1」を設定
            // 機器別管理基準部位ID
            setValue(FormDetail.ManagementStandardsList.Id, FormDetail.ManagementStandardsList.ColumnNo.ManagementStandardsComponentId, 1, CtrlFlag.Label, "-1", true, false);

            // 機器別管理基準内容ID
            setValue(FormDetail.ManagementStandardsList.Id, FormDetail.ManagementStandardsList.ColumnNo.ManagementStandardsContentId, 1, CtrlFlag.Label, "-1", true, false);

            // 保全スケジュールID
            setValue(FormDetail.ManagementStandardsList.Id, FormDetail.ManagementStandardsList.ColumnNo.MaintainanceScheduleId, 1, CtrlFlag.Label, "-1", true, false);

            // 入力エリアを非活性にする
            // 機器レベル
            var equipmentLevelCtrl = getCtrl(FormDetail.ManagementStandardsList.Id, FormDetail.ManagementStandardsList.ColumnNo.EquipmentLevelLabel, 1, CtrlFlag.Combo, true, false);
            changeInputControl(equipmentLevelCtrl, false);

            // 機器番号
            var machineNoCtrl = getCtrl(FormDetail.ManagementStandardsList.Id, FormDetail.ManagementStandardsList.ColumnNo.MachineNo, 1, CtrlFlag.TextBox, true, false);
            changeInputControl(machineNoCtrl, false);

            // 機器名称
            var machineNameCtrl = getCtrl(FormDetail.ManagementStandardsList.Id, FormDetail.ManagementStandardsList.ColumnNo.MachineName, 1, CtrlFlag.TextBox, true, false);
            changeInputControl(machineNameCtrl, false);
        }
        else {

            // 選択されたレコードの申請状況を取得
            var applicationDivisionCode = getValue(FormDetail.ManagementStandardsList.Id, FormDetail.ManagementStandardsList.ColumnNo.ApplicationDivisionCode, rowNo, CtrlFlag.Label, true, false);

            // 行削除が行われている場合は登録ボタンを非表示にする
            if (applicationDivisionCode == ApplicationDivision.Delete) {

                getButtonCtrl(FormDetail.Button.ManagementStandardRegist).hide();
            }
        }

        // 登録ボタンにフォーカス(描画に間に合わないので少し遅らせる)
        setTimeout(function () {
            setFocusButton(FormDetail.Button.ManagementStandardRegist);
        }, 300); //300ミリ秒
    }
}

/**
 *【オーバーライド用関数】タブ切替時
 * @tabNo ：タブ番号
 * @tableId : 一覧のコントロールID
 */
function initTabOriginal(tabNo, tableId) {

    if (tabNo == FormDetail.ManagementStandardsList.TabNo) {
        // 機器別管理基準タブ
        // 初回(タブ切替数 = 0)の場合のみ保全項目一覧の背景色変更・コントロール非表示処理を行う
        var tabChangeCnt = parseInt(getValue(FormDetail.MachineInfo.Machine.Id, FormDetail.MachineInfo.Machine.ColumnNo.TabChangeCnt, 1, CtrlFlag.Label, false, false));

        if (tabChangeCnt == 0) {

            // 保全項目一覧の背景色変更処理
            var table = P_listData['#' + FormDetail.ManagementStandardsList.Id + getAddFormNo()];
            commonChangeBackGroundColorHistory(table, FormDetail.ManagementStandardsList.ColumnNo.ApplicationDivisionCode, FormDetail.ManagementStandardsList.ColumnNo.ValueChanged, FormDetail.ManagementStandardsList.ColumnNo);

            //　行追加・行削除アイコン非表示
            var condition = getConditionToHideButton();
            hideCtrlManagementStandardsList(condition);

            // タブ切替数を加算
            setValue(FormDetail.MachineInfo.Machine.Id, FormDetail.MachineInfo.Machine.ColumnNo.TabChangeCnt, 1, CtrlFlag.Label, tabChangeCnt + 1, false, false);

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

    return preDeleteRowCommon(id, [FormDetail.ManagementStandardsList.Id]);
}

/**
 * 詳細画面 ボタン非表示処理の条件作成
 * */
function getConditionToHideButton() {

    var condition = [];
    // トランザクションモードの場合True、変更管理モードの場合False
    condition.isTransactionMode = ProcessMode.Transaction == getValue(FormDetail.MachineInfo.Machine.Id, FormDetail.MachineInfo.Machine.ColumnNo.ProcessMode, 1, CtrlFlag.Label, false, false);

    // 申請状況(10：申請データ作成中、20：承認依頼中、30：差戻中、40：承認済み)
    condition.applicationStatusCode = getValue(FormDetail.MachineInfo.Machine.Id, FormDetail.MachineInfo.Machine.ColumnNo.ApplicationStatusCode, 1, CtrlFlag.Label, false, false);

    // 申請区分(10：新規登録申請、20：変更申請、30：削除申請)
    condition.applicationDivisionCode = getValue(FormDetail.MachineInfo.Machine.Id, FormDetail.MachineInfo.Machine.ColumnNo.ApplicationDivisionCode, 1, CtrlFlag.Label, false, false);

    // 申請の申請者またはシステム管理者の場合True、それ以外はFalse
    condition.isCertified = convertStrToBool(getValue(FormDetail.MachineInfo.Machine.Id, FormDetail.MachineInfo.Machine.ColumnNo.IsCertified, 1, CtrlFlag.Label, false, false));

    // 工場の承認ユーザの場合True
    condition.isCertifiedFactory = convertStrToBool(getValue(FormDetail.MachineInfo.Machine.Id, FormDetail.MachineInfo.Machine.ColumnNo.IsCertifiedFactory, 1, CtrlFlag.Label, false, false));

    return condition;
}

/**
 * 詳細画面 背景色を変更するセルのリストを取得
 */
function getColumnList() {
    var list = [];
    list.push(FormDetail.MachineInfo.Location);        // 地区～設備
    list.push(FormDetail.MachineInfo.Machine);         // 機器番号～保全方式
    list.push(FormDetail.MachineInfo.Reason);          // 適用法規～否認理由
    list.push(FormDetail.EquipmentInfo.Job);           // 職種～機種小分類
    list.push(FormDetail.EquipmentInfo.Equipment);     // 使用区分～点検種別毎管理
    list.push(FormDetail.EquipmentInfo.EquipmentNote); // 機器メモ

    var columnList = [];
    $.each(list, function (i, obj) {
        $.each(obj.ColumnNo, function (key, val) {
            //例：{CtrlId: 'BODY_010_00_LST_1', Key: 'District', Val: 1}
            columnList.push({ CtrlId: obj.Id, Key: key, Val: val })
        });
    });
    return columnList;
}

/**
 * 保全項目一覧のコントロールを非表示にする
 * @param {any} condition 非表示条件
 */
function hideCtrlManagementStandardsList(condition) {

    // 保全項目一覧の行追加・行削除・詳細リンク 非表示処理
    //行追加、行削除ボタンの表示フラグ
    //トランザクションモードの場合、または、申請状況が「申請データ作成中」or「差戻中」かつ申請者（システム管理者含む）かつ申請区分が「削除」以外　の場合表示
    var flg = (condition.isTransactionMode ||
        (!condition.isTransactionMode && ((condition.applicationStatusCode == ApplicationStatus.Making && condition.isCertified && condition.applicationDivisionCode != ApplicationDivision.Delete)
            || (condition.applicationStatusCode == ApplicationStatus.Return && condition.isCertified && condition.applicationDivisionCode != ApplicationDivision.Delete))));

    //　行追加・行削除アイコン・詳細リンク 非表示
    changeRowControlAndDispRowNo(FormDetail.ManagementStandardsList.Id, flg);
}

/**
 * 機器別管理基準タブが選択状態かどうかを返す
 * */
function isTabManagementStandardsSelected() {

    // 機器別管理基準タブが選択状態かどうかを返す
    return $($("#tab_btn_detail_1").find("a[data-tabno='" + FormDetail.ManagementStandardsList.TabNo + "']")[0]).hasClass("selected");
}

/**
 * 機器に変更管理が紐付いているかチェックする
 * */
function isExistsHistoryManagement(appPath, formNo) {

    var strMessage = P_ComMsgTranslated[141160016]; //「対象データに申請が存在するため処理を行えません。」

    return checkTransition(appPath, formNo, ConductId_HM0001, "judgeStatusCntExceptApproved", FormDetail.MachineInfo.Machine.ColumnNo.StatusCntExceptApproved, strMessage);

}

/**
 * 変更管理の排他チェック
 */
function checkHisoryManagemtntExclusive(appPath, formNo) {

    var strMessage = P_ComMsgTranslated[941290001]; //「編集していたデータは他のユーザにより更新されました。もう一度編集する前にデータを再表示してください。」

    return checkTransition(appPath, formNo, ConductId_HM0001, "checkHisoryManagemtntExclusive", FormDetail.MachineInfo.Machine.ColumnNo.StatusCntExceptApproved, strMessage);
}