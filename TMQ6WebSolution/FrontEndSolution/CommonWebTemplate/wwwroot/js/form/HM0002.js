/* ========================================================================
 *  機能名　    ：   【HM0002】長期計画変更管理
 * ======================================================================== */

/**
 * 自身の相対パスを取得
 */
function getPath() {
    var root;
    var scripts = document.getElementsByTagName("script");
    var i = scripts.length;
    while (i--) {
        var match = scripts[i].src.match(/(^|.*\/)HM0002\.js$/);
        if (match) {
            root = match[1];
            break;
        }
    }
    return root;
}
document.write("<script src=\"" + getPath() + "/tmqcommon.js\"></script>");
document.write("<script src=\"" + getPath() + "/SU0001.js\"></script>");
document.write("<script src=\"" + getPath() + "/HM0003.js\"></script>");
document.write("<script src=\"" + getPath() + "/HM0004.js\"></script>");

// 保全活動一覧関連の定義 ↓
// 保全情報一覧の列情報
const MaintList = {
    MachineNo: 1, MachineName: 2, MachineImportance: 3, MainInspection: 4, MainContent: 5, Budget: 6, ScheduleType: 7,
    CycleYear: 8, CycleMonth: 9, CycleDay: 10, DispCycle: 11, StartDate: 12, ScheduleDate: 13, MainKind: 14, MainKindId: 67,
    ApplicationDivisionCode: 62, // 申請区分コード(拡張項目)
};
// 保全情報一覧(点検種別毎)の列情報
const MaintListKind = {
    MachineNo: 1, MachineName: 2, MachineImportance: 3, MainKind: 4, MainInspection: 5, MainContent: 6, Budget: 7, ScheduleType: 8,
    CycleYear: 9, CycleMonth: 10, CycleDay: 11, DispCycle: 12, StartDate: 13, ScheduleDate: 14, MachineId: 56, ContentId: 53, MainKindId: 65,
    ApplicationDivisionCode: 62, // 申請区分コード(拡張項目)
};

/**
 * フォームNoで保全活動一覧を表示する画面か判定する
 * @param {any} formNo フォームNo
 * @return {bool} 表示する場合True
 */
function isMaintListForm(formNo) {
    var formNoList = [FormDetail.No];
    var numFormNo = formNo - 0;
    return $.inArray(numFormNo, formNoList) >= 0;
}
/**
 * 計画内容が機器の時に非表示にする列の一覧を取得
 * @param {any} colInfo 保全活動一覧の列の情報
 * @return {List<int>} 非表示にする列のVAL値のリスト
 */
function getHideColDetailListForEquipment(colInfo) {
    return [colInfo.MainInspection, colInfo.MainContent, colInfo.Budget, colInfo.ScheduleType, colInfo.CycleYear, colInfo.CycleMonth, colInfo.CycleDay, colInfo.DispCycle, colInfo.StartDate, colInfo.ScheduleDate, colInfo.MainKind];
}
/**
 * 計画内容が機器の時に非表示にする列の一覧を取得
 * @param {any} colInfo 保全活動一覧の列の情報
 * @return {List<int>} 非表示にする列のVAL値のリスト
 */
function getHideColDetailListForInspection(colInfo) {
    return [colInfo.MainContent, colInfo.Budget, colInfo.ScheduleType, colInfo.CycleYear, colInfo.CycleMonth, colInfo.CycleDay, colInfo.DispCycle, colInfo.StartDate, colInfo.ScheduleDate, colInfo.MainKind];
}
// 保全活動一覧関連の定義 ↑

// スケジュール表示単位の拡張項目の値
const ScheduleUnit = { Year: 2, Month: 1 };

// 一覧画面の定義
const FormList = {
    No: 0
    , List: {
        Id: "BODY_040_00_LST_0",
        ColumnNo: {                      // 項目番号
            Subject: 1,                  // 件名
            SubjectNote: 2,              // 件名メモ
            District: 3,                 // 地区
            Factory: 4,                  // 工場
            Plant: 5,                    // プラント
            Series: 6,                   // 系列
            Stroke: 7,                   // 工程
            Facility: 8,                 // 設備
            Job: 9,                      // 職種
            Large: 10,                   // 機種大分類
            Middle: 11,                  // 機種中分類
            Small: 12,                   // 機種小分類
            MaintenanceSeason: 14,       // 保全時期
            PersonName: 15,              // 担当
            WorkItem: 17,                // 作業項目
            BudgetManagement: 20,        // 予算管理区分
            BudgetPersonality: 21,       // 予算性格区分
            Purpose: 22,                 // 目的区分
            WorkClass: 23,               // 作業区分
            Treatment: 24,               // 処置区分
            FacilityDivision: 25,        // 設備区分
            Content: 30,                 // 保全情報変更有無
            ApplicationDivisionCode: 55, // 申請区分コード(拡張項目)
            ValueChanged: 56,            // 変更のあった項目
            LongPlanDivision: 57,        // 長計区分
            LongPlanGroup: 58            // 長計グループ
        }
    } // 一覧
    , Button: {
        New: "New",                           // 新規申請
        ApprovalAllTrans: "ApprovalAllTrans", // 一括承認(画面遷移)※選択行チェックに使用
        DenialAllTrans: "DenialAllTrans",     // 一括否認(画面遷移)※選択行チェックに使用
        ApprovalAll: "ApprovalAll",           // 一括承認(実行)
        DenialAll: "DenialAll",               // 一括否認(実行)
        Output: "Output"                      // 出力
    }
    , Filter: { Id: "BODY_010_00_LST_0", Input: 1 } // 一覧上部のフィルタ
    , Condition: { Id: "BODY_020_00_LST_0", Schedule: { Unit: 1, Year: 2, Month: 3, Ext: 4 } } // スケジュール表示条件※共通
    , IsApprovalUser: { Id: "BODY_050_00_LST_0", IsApprovalUser: 2} //承認権限有無
}
// 参照画面の定義
const FormDetail = {
    No: 1
    // 非表示の退避項目
    , Info: {
        Id: "BODY_100_00_LST_1"
        , LongPlanId: 1
        , IsDisplayMaintainanceKind: 3 // 保全情報一覧(点検種別毎)の表示是非
        , HistoryManagementId: 8 //変更管理ID
        , IsTransitionFlg: 10 //遷移前排他チェック結果フラグ
        , ProcessMode: 11 //処理モード(0：トランザクションモード、1：変更管理モード)
        , ApplicationDivisionCode: 12 //申請区分
        , ValueChanged: 13 //変更対象
        , ApplicationStatusCode: 14 //申請状況
        , IsCertified: 15 //申請の申請者またはシステム管理者の場合True
        , IsCertifiedFactory: 16 //工場の承認ユーザの場合True
    }
    // 保全情報一覧
    , List: { Id: "BODY_070_00_LST_1", ColInfo: MaintList }
    // 保全情報一覧(点検種別毎)
    , ListMaintKind: { Id: "BODY_080_00_LST_1", ColInfo: MaintListKind }
    // 検索条件
    , Condition: {
        Id: "BODY_050_00_LST_1"
        // 計画内容コンボ
        , PlanContent: 3
        // 計画内容コンボの選択値の拡張項目の値
        , PlanContentExt: 4
        // 選択中の計画内容コンボ拡張項目の値の比較用(機器:1、部位:2、保全項目:3)
        , PlanComboOptValue: { Equipment: 1, InspectionSite: 2, Maintainance: 3 }
        // スケジュール表示条件関連
        , Schedule: { Unit: 1, Year: 2, Month: 5, Ext: 6 }
    }
    , Button: {                                                 // ボタンコントロールID
        CopyRequest: "CopyRequest",                             // 複写申請
        ChangeRequest: "ChangeRequest",                         // 変更申請
        DeleteRequest: "DeleteRequest",                         // 削除申請
        ChangeApplicationRequest: "ChangeApplicationRequest",   // 承認依頼
        EditRequest: "EditRequest",                             // 申請内容修正
        CancelRequest: "CancelRequest",                         // 申請内容取消
        PullBackRequest: "PullBackRequest",                     // 承認依頼引戻
        ChangeApplicationApproval: "ChangeApplicationApproval", // 承認
        ChangeApplicationDenial: "ChangeApplicationDenial",     // 否認
        BeforeChange: "BeforeChange"                            // 変更前
    }
    , Person: {
        Id: "BODY_040_00_LST_1"
        , ColumnNo: {
            PersonCode: 1 //担当（コード）
            , Name: 2 //担当（値）
        }
    }
    , Location: {
        Id: "BODY_010_00_LST_1"
        , ColumnNo: {
            District: 1 // 地区
            , Factory: 2 // 工場
            , Plant: 3 // プラント
            , Series: 4 // 系列
            , Stroke: 5 // 工程
            , Facility: 6 // 設備
        }
    }
    , Job: {
        Id: "BODY_020_00_LST_1"
        , ColumnNo: {
            Job: 1 // 職種
            , Large: 2 // 機種大分類
            , Middle: 3 // 機種中分類
            , Small: 4 // 機種小分類
            , BudgetManagement: 5 //予算管理区分
            , BudgetPersonality: 6 //予算性格区分
            , MaintenanceSeason: 7 //保全時期
            , WorkItem: 8 //作業項目
        }
    }
    , Purpose: {
        Id: "BODY_110_00_LST_1"
        , ColumnNo: {
            Purpose: 1 //目的区分
            , WorkClass: 2 //作業区分
            , Treatment: 3 //処置区分
            , FacilityDivision: 4 //設備区分
            , LongPlanDivision: 5 // 長計区分
            , LongPlanGroup: 6 // 長計グループ
        }
    }
    , Subject: {
        Id: "BODY_030_00_LST_1"
        , ColumnNo: {
            Subject: 1 //件名
            , SubjectNote: 2 //件名メモ
        }
    }
    , ApplicationStatus: {
        Id: "BODY_130_00_LST_1"
        , Status: 1 //申請状況
        , Division: 2 //申請区分
        , ApprovalUser: 3 //承認者
    }
    , ApplicationReason: {
        Id: "BODY_140_00_LST_1"
    }
};

// 詳細編集画面の定義
const FormEdit = { No: 2, Button: { Regist: "Regist" }, Hide: { Id: "BODY_050_00_LST_2" } };
// 機器別管理基準選択画面の定義
const FormSelect = {
    No: 3
    , List: { Id: "BODY_040_00_LST_3" }
    , Hide: { Id: "BODY_060_00_LST_3" }
    , Location: { Id: "COND_000_00_LST_3" }
    , Job: { Id: "COND_010_00_LST_3" }
    , DetailCondition: { Id: "COND_020_00_LST_3", UseSegment: 1 }
    , Button: { Search: "Search" }
};

// スケジュールを表示する一覧
// 一覧画面（通常の一覧のみ）
// 参照画面、保全活動作成画面、予定作業一括延期画面（通常の一覧または点検種別毎の一覧）
const ScheduleListIds = [FormList.List.Id,FormDetail.List.Id, FormDetail.ListMaintKind.Id];

//画面遷移前のチェック処理名（「承認済」以外の変更管理が紐づいているかチェック）
const CheckExistsHistory = "checkExistsHistory";
//画面遷移前のチェック処理名（変更管理の排他チェック）
const CheckExclusiveHistory = "checkExclusiveHistory";
/**
 * フォーム番号より一致するフォームの情報を取得する
 * @param {any} formNo フォーム番号
 * @return {formInfo} フォームの定義情報
 */
function getFormInfoByFormNo(formNo) {
    var numFormNo = formNo - 0;
    var list = [FormList, FormDetail, FormEdit, FormSelect];
    for (form of list) {
        if (form.No == numFormNo) {
            return form;
        }
    }
}

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
    //申請状況変更画面の初期化処理
    HM0003_initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data);
    //帳票出力画面の初期化処理
    HM0004_initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data);

    if (conductId != ConductId_HM0002) {
        // HM0002以外なら終了
        return;
    }
    // 以降は長期計画一覧の処理

    // タイミングの問題でスケジュールの期間と年度が両方表示されている場合があるので、その場合の処理
    var controlVisibleScheduleCond = function (formInfo) {
        // 表示期間(From-ToのFrom)
        var scheduleYear = getCtrl(formInfo.Condition.Id, formInfo.Condition.Schedule.Year, 0, CtrlFlag.Input);
        // 表示年度
        var scheduleMonth = getCtrl(formInfo.Condition.Id, formInfo.Condition.Schedule.Month, 0, CtrlFlag.Input);
        // どちらも表示されている場合、コンボの変更イベントを発生させて表示制御を行う
        if ($(scheduleYear).is(":visible") && $(scheduleMonth).is(":visible")) {
            var unit = getCtrl(formInfo.Condition.Id, formInfo.Condition.Schedule.Unit, 0, CtrlFlag.Combo);
            changeNoEdit(unit);
        }
    }

    if (formNo == FormList.No) {
        // 両方表示されている場合は制御
        controlVisibleScheduleCond(FormList);

        //一括承認ボタン、一括否認ボタンの表示制御
        var isApprovalUser = getValue(FormList.IsApprovalUser.Id, FormList.IsApprovalUser.IsApprovalUser, 1, CtrlFlag.Label);
        setHideButton(FormList.Button.ApprovalAllTrans, !convertStrToBool(isApprovalUser));
        setHideButton(FormList.Button.DenialAllTrans, !convertStrToBool(isApprovalUser));

        // 一覧画面の場合、新規申請ボタンにフォーカスをセット
        // 押下不能なら出力ボタン
        setFocusButtonAvailable(FormList.Button.New, FormList.Button.Output);
    } else if (formNo == FormDetail.No) {
        // スケジュールの検索条件コンボが両方表示されている場合は制御
        controlVisibleScheduleCond(FormDetail);
        // 保全内容コンボのみ設定時の初期値設定処理
        setPlanContentInitValueForUnSet();

        //ボタン等の表示制御
        setHideButtonAndItem();

        // 申請区分
        var applicationDivisionCode = getValue(FormDetail.Info.Id, FormDetail.Info.ApplicationDivisionCode, 1, CtrlFlag.Label, false, false);
        // 背景色変更処理
        changeBackGroundColorHistoryDetail(applicationDivisionCode, getColumnList(), FormDetail.Info.Id, FormDetail.Info.ValueChanged);

        //処理モード
        var processMode = getValue(FormDetail.Info.Id, FormDetail.Info.ProcessMode, 1, CtrlFlag.Label);
        // ボタン押下可能なボタンにフォーカスをセット
        setFocusButtonHistory(processMode);
    } else if (formNo == FormEdit.No) {
        // 登録ボタンにフォーカス
        setFocusButton(FormEdit.Button.Regist);
    } else if (formNo == FormSelect.No) {
        // 検索条件の使用区分を非表示
        var useSegment = $(getCtrl(FormSelect.DetailCondition.Id, FormSelect.DetailCondition.UseSegment, 1, CtrlFlag.Combo)).closest('tr');
        setHide(useSegment, true);
        // 検索ボタンにフォーカス
        setFocusButton(FormSelect.Button.Search);
    }

    if (isMaintListForm(formNo)) {
        // 保全活動一覧を表示する画面の場合、表示制御
        setVisibleMaintList(getFormInfoByFormNo(formNo));
    }
}

/**
 * 計画内容コンボの値が未設定の際に設定する処理
 * */
function setPlanContentInitValueForUnSet() {
    // 計画内容コンボ拡張項目の値を取得
    var content = getValue(FormDetail.Condition.Id, FormDetail.Condition.PlanContentExt, 0, CtrlFlag.Label);
    if (content == null || content == undefined || content == '') {
        // 計画内容コンボの初期値を設定する(拡張項目1の値が保全項目)
        selectComboByExparam(FormDetail.Condition.Id, FormDetail.Condition.PlanContent, 1, FormDetail.Condition.PlanComboOptValue.Maintainance);
        // 変更時イベントを発生させて拡張項目の値を設定
        var combo = getCtrl(FormDetail.Condition.Id, FormDetail.Condition.PlanContent, 0, CtrlFlag.Combo);
        changeNoEdit(combo);
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

    if (pgmId != ConductId_HM0002) {
        // HM0002以外なら終了
        return [conditionDataList, listDefines];
    }

    // スケジュール表示単位コンボを強制的に変更し、変更時イベントを設定する処理を呼び出し
    var scheduleExec = function (listInfo) {
        setComboExValueOnPrevInitFormData(conditionDataList, listInfo.No, listInfo.Condition.Id, listInfo.Condition.Schedule.Unit, listInfo.Condition.Schedule.Ext, CtrlFlag.Label);
    }
    // スケジュールの項目に値を設定し、検索条件の内容を更新する
    var setScheduleInitValue = function (formInfo) {
        // スケジュール表示期間の初期値を設定する処理
        var setYearScheduleFromTo = function (formInfo) {
            // Fromの項目
            var from = getCtrl(formInfo.Condition.Id, formInfo.Condition.Schedule.Year, 0, CtrlFlag.Input);
            // TOの項目
            var to = $(from).parent().siblings("div").find("input");
            //値
            var fromYear = $(from).val();
            var toYear = $(to).val();
            // 検索条件に反映
            setValueToConditionDataList(conditionDataList, formInfo.No, formInfo.Condition.Id, formInfo.Condition.Schedule.Year, fromYear + "|" + toYear);
        }
        // スケジュール表示年の初期値
        var setStartYear = function (formInfo) {
            //値
            var year = getValue(formInfo.Condition.Id, formInfo.Condition.Schedule.Month, 0, CtrlFlag.TextBox);
            // 検索条件に反映
            setValueToConditionDataList(conditionDataList, formInfo.No, formInfo.Condition.Id, formInfo.Condition.Schedule.Month, year);
        }

        setYearScheduleFromTo(formInfo); //年From-To
        setStartYear(formInfo); // 開始年
    }


    if (formNo == FormSelect.No) {
        // 機器別管理基準選択画面
        if ($(conditionDataList).length) {
            var condTreeList = [FormSelect.Location.Id, FormSelect.Job.Id];
            $.each($(conditionDataList), function (idx, conditionData) {
                // 場所階層、職種機種のツリーラベルを含む一覧の場合
                if (conditionData.FORMNO == FormSelect.No && condTreeList.indexOf(conditionData.CTRLID) > -1) {
                    // 一覧の定義情報に格納
                    listDefines.push(conditionData);
                }
            });
        }
    } else if (formNo == FormDetail.No) {
        // 参照画面
        // 計画内容コンボが未設定の際に再設定を行う
        setPlanContentInitValueForUnSet();

        // 変更した値を取得して検索条件にセット(使用しないけど、変更値をセットしないと年度設定の際に上書きされてしまう)
        var selectedValue = getValue(FormDetail.Condition.Id, FormDetail.Condition.PlanContent, 0, CtrlFlag.Combo);
        setValueToConditionDataList(conditionDataList, FormDetail.No, FormDetail.Condition.Id, FormDetail.Condition.PlanContent, selectedValue);

        // 初期表示時、計画内容コンボの拡張項目の値がセットされていないため、値をセットしないと初期表示時の検索で値が取得できない
        setComboExValueOnPrevInitFormData(conditionDataList, FormDetail.No, FormDetail.Condition.Id, FormDetail.Condition.PlanContent, FormDetail.Condition.PlanContentExt, CtrlFlag.Label);
        // 初期表示時、スケジュール表示単位の拡張項目の値がセットされていないため、値をセットしないと初期表示時の検索で値が取得できない
        scheduleExec(FormDetail);

        // スケジュール表示年の初期値設定
        setScheduleInitValue(FormDetail);

        // 条件エリアもページ情報に追加
        listDefines = addSearchConditionToListDefine(listDefines, transPtnDef.Child, FormDetail.No, FormList.No);

        // 遷移先情報を設定(URL指定起動)
        conditionDataList = getParamByUrl(ConductId_HM0002, conditionDataList);
    } else if (formNo == FormList.No) {
        // 一覧画面
        // 初期表示時、スケジュール表示単位の拡張項目の値がセットされていないため、値をセットしないと初期表示時の検索で値が取得できない
        scheduleExec(FormList);

        // スケジュール表示年の初期値設定
        setScheduleInitValue(FormList);

        // 条件エリアもページ情報に追加
        listDefines = addSearchConditionToListDefine(listDefines, transPtnDef.None, FormList.No, -1);
    }
    else {
        // 上記の画面以外の場合は終了
        return [conditionDataList, listDefines];
    }

    return [conditionDataList, listDefines];
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

    // 共通-申請状況変更画面の画面再表示前処理
    // 共通-申請状況変更画面を閉じたときの再表示処理はこちらで行うので、各機能での呼出は不要
    HM0003_beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom);

    // 共通-担当者検索画面を閉じたとき
    SU0001_beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom);

    // 参照画面の場合、戻る/閉じるボタン表示制御
    if (formNo == FormDetail.No) {
        setDisplayCloseBtn(transPtn);
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
    if (conductId == ConductId_HM0002) {
        // 長期計画の場合
        if (formNo == FormSelect.No) {
            // 機器別管理基準選択画面の場合
            // 選択チェック
            if (!isCheckedList(FormSelect.List.Id)) {
                // 一覧が未選択の場合エラー
                return false;
            }
        }
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
    if (isMaintListForm(transTarget)) {
        // 保全活動一覧を表示する画面の場合、表示制御
        setVisibleMaintList(getFormInfoByFormNo(transTarget));
    }
}

/*
 * 明細から削除済みでない行を取得する
 *  @return {element} 一覧の削除されていない行リスト
*/
function getRows(ctrlId) {
    // 削除されていない行を取得
    var list = $(P_Article).find("div#" + ctrlId + getAddFormNo() + ".ctrlId").find("div:not([class^='tabulator-header'])").find("div.tabulator-row");
    return list;
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
    if (executeListFilter(transTarget, FormList.List.Id, FormList.Filter.Id, FormList.Filter.Input)) {
        return [false, conditionDataList];
    }

    if (isScheduleLink(btn_ctrlId)) {
        // スケジュールのリンクの場合
        var conditionDataList = getParamToMA0001BySchedule(transTarget);
        return [true, conditionDataList];
    }

    if (formNo == FormDetail.No) {
        // 参照画面で遷移する場合
        if (btn_ctrlId == FormDetail.Button.BeforeChange) { // 変更前

            // 別タブで件名別長期計画詳細画面(LN0001)を開くための検索条件を設定する(長期計画件名ID：非表示項目を取得)
            conditionDataList = getParamToLN0001(getValue(FormDetail.Info.Id, FormDetail.Info.LongPlanId, 1, CtrlFlag.Label, false, false));
        }

        // 複写申請、変更申請、申請内容修正（編集or複写）で詳細編集画面へ遷移する場合True
        var isToEdit = transTarget == FormEdit.No && (transDiv == transDivDef.Edit || transDiv == transDivDef.Copy);
        // 機器別管理基準選択画面に遷移する場合
        var isSelect = transTarget == FormSelect.No;
        // 処理モード
        var processMode = getValue(FormDetail.Info.Id, FormDetail.Info.ProcessMode, 1, CtrlFlag.Label);
        if (btn_ctrlId == FormDetail.Button.ChangeRequest || (isSelect && processMode == ProcessMode.Transaction)) {
            var message = P_ComMsgTranslated[141160016]; //「対象データに申請が存在するため処理を行えません。」

            //変更申請またはトランザクションモードで行追加ボタンを押下時、「承認済み」以外の変更管理が紐づいている場合、遷移をキャンセル
            var flg = checkTransition(appPath, formNo, ConductId_HM0002, CheckExistsHistory, FormDetail.Info.IsTransitionFlg, message);
            if (!flg) {
                return [false, conditionDataList];
            }
        }

        if (btn_ctrlId == FormDetail.Button.ChangeApplicationRequest || btn_ctrlId == FormDetail.Button.EditRequest || btn_ctrlId == FormDetail.Button.ChangeApplicationDenial) {
            var message = P_ComMsgTranslated[941290001]; //「編集していたデータは他のユーザにより更新されました。もう一度編集する前にデータを再表示してください。」

            //承認依頼、申請内容修正、否認押下時、変更管理テーブルの排他チェックを行いエラーの場合は遷移をキャンセル
            var flg = checkTransition(appPath, formNo, ConductId_HM0002, CheckExclusiveHistory, FormDetail.Info.IsTransitionFlg, message);
            if (!flg) {
                return [false, conditionDataList];
            }
        }

        if (btn_ctrlId == FormDetail.Button.ChangeApplicationRequest || btn_ctrlId == FormDetail.Button.ChangeApplicationDenial) {
            //承認依頼、否認押下時、申請状況変更画面(HM0003)を開くための条件を設定する
            // 変更管理ID
            var historyManagementId = getValue(FormDetail.Info.Id, FormDetail.Info.HistoryManagementId, 1, CtrlFlag.Label);
            conditionDataList = getParamToHM0003(btn_ctrlId == FormDetail.Button.ChangeApplicationRequest, historyManagementId);
        }

        // 検索条件に追加する一覧のID
        var ctrlIdList = [FormDetail.Info.Id]; // 非表示項目退避
        if (isToEdit || isSelect) {
            // 検索条件に追加
            conditionDataList = getListDataByCtrlIdList(ctrlIdList, formNo, 0);
        }
    } else if (formNo == FormList.No) {
        // 一覧画面で遷移する場合

        if (transTarget == FormDetail.No) {
            // 参照画面への遷移の場合
            conditionDataList = getConditionDataListToDetail();
        }
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
            $(getButtonCtrl(FormList.Button.ApprovalAll)).click()
            return [false, conditionDataList];
        }
        else if (btn_ctrlId == FormList.Button.DenialAllTrans) { // 一括否認(画面遷移)

            // 画面遷移をキャンセルして一括否認処理実行
            $(getButtonCtrl(FormList.Button.DenialAll)).click()
            return [false, conditionDataList];
        }

        if (btn_ctrlId == FormList.Button.Output) {
            //出力押下時、変更管理帳票出力画面(HM0004)を開くための条件を設定する
            conditionDataList = getParamToHM0004(ConductId_HM0002);
        }
    }

    return [true, conditionDataList];
}

/**
 * 一覧画面から参照画面へ遷移する場合に追加で設定する検索条件を取得する処理
 * */
function getConditionDataListToDetail() {
    // 参照画面の検索条件を取得
    return getListDataByCtrlIdList([FormDetail.Condition.Id], FormDetail.No, 0);
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
        var conditionDataList = getListDataByCtrlIdList([FormEdit.Hide.Id], FormEdit.No, 0);
        // 一覧から参照へ遷移する場合と同様に、参照画面の検索条件を追加
        conditionDataList.concat(getConditionDataListToDetail());
        setSearchCondition(ConductId_HM0002, FormDetail.No, conditionDataList);
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

    return preDeleteRowCommon(id, [FormDetail.List.Id, FormDetail.ListMaintKind.Id]);
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
        var btnName = $(btn).filter("input").attr("name");
        // 申請内容取消・承認後は一覧画面へ戻るため何もしない
        if (btnName == FormDetail.Button.CancelRequest || btnName == FormDetail.Button.ChangeApplicationApproval) {
            return;
        }

        //ボタン等の表示制御
        setHideButtonAndItem();

        // 申請区分
        var applicationDivisionCode = getValue(FormDetail.Info.Id, FormDetail.Info.ApplicationDivisionCode, 1, CtrlFlag.Label, false, false);
        // 背景色変更処理
        changeBackGroundColorHistoryDetail(applicationDivisionCode, getColumnList(), FormDetail.Info.Id, FormDetail.Info.ValueChanged);

        //処理モード
        var processMode = getValue(FormDetail.Info.Id, FormDetail.Info.ProcessMode, 1, CtrlFlag.Label);
        // ボタン押下可能なボタンにフォーカスをセット
        setFocusButtonHistory(processMode);
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
    var btnName = $(btn)[0].name;

    if (formNo == FormSelect.No) {
        // 機器別管理基準選択画面の場合、非表示項目を追加
        // 初期表示時に設定した値が検索で消えるためボトムエリアに定義、登録時に値が渡されないため
        return getListDataByCtrlIdList([FormSelect.Hide.Id], formNo, 0, true);
    }
    // それ以外の場合
    var conditionDataList = [];
    return conditionDataList;
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
    // スケジュール表示単位コンボの変更時イベント
    var eventChangeScheduleUnit = function (formInfo) {
        if (!(ctrlId == formInfo.Condition.Id && valNo == formInfo.Condition.Schedule.Unit)) {
            // 表示単位コンボでなければ終了
            return;
        }

        // 拡張項目の値を隠し項目にセット
        setComboExValue(formInfo.Condition.Id, formInfo.Condition.Schedule.Ext, CtrlFlag.Label, selected);
        // 年を選択時、月を非表示。月を選択時、年を非表示
        var extValue = getValue(formInfo.Condition.Id, formInfo.Condition.Schedule.Ext, 0, CtrlFlag.Label);
        var isYearDisplay = extValue == ScheduleUnit.Year; // 年を選択している場合True

        /**
         * 対象の表示状態を切り替えるコントロールを取得
         * @param {any} isGetYear 年の場合True
         */
        var getTargetCtrl = function (isGetYear) {
            const valNo = isGetYear ? formInfo.Condition.Schedule.Year : formInfo.Condition.Schedule.Month;
            var target = getCtrl(formInfo.Condition.Id, valNo, 0, CtrlFlag.TextBox);
            return $(target).closest("tr");
        }
        // 年の表示制御
        setHide(getTargetCtrl(true), !isYearDisplay);
        // 月の表示制御
        setHide(getTargetCtrl(false), isYearDisplay);
    }

    // 所定のコンボでなければ終了
    if (formNo == FormDetail.No) {
        // 参照画面
        if (ctrlId == FormDetail.Condition.Id && valNo == FormDetail.Condition.PlanContent) {
            // 計画内容コンボ
            setComboExValue(FormDetail.Condition.Id, FormDetail.Condition.PlanContentExt, CtrlFlag.Label, selected);
        }
        // スケジュール表示単位コンボ
        eventChangeScheduleUnit(FormDetail);
    } else if (formNo == FormList.No) {
        // 一覧画面
        // スケジュール表示単位コンボ
        eventChangeScheduleUnit(FormList);
    }
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
    if (formNo != FormDetail.No) {
        // 参照画面以外の場合、終了
        return;
    }
    //参照画面のユーザIDの場合、削除されたユーザだと翻訳を取得できないので、トランザクションテーブルの値を表示する
    if (!data && $(ctrl).val() != "") {
        var name = getValue(FormDetail.Person.Id, FormDetail.Person.ColumnNo.Name, 0, CtrlFlag.Label);
        setNameToCodeTrans(ctrl, name);
    }
}

/**
 * 保全活動一覧の表を点検種別毎かどうかを判定し表示を切り替える処理
 * @param {any} formInfo 保全活動一覧をメンバに持つフォームの情報
 */
function setVisibleMaintList(formInfo) {
    // 保全活動一覧(点検種別毎)の表示フラグ
    var isDisplayMaintainanceKind = getIsDisplayMaintainanceKind(formInfo.Info);
    // 一覧の表示状態を切り替え
    changeListDisplay(formInfo.List.Id, !isDisplayMaintainanceKind);
    changeListDisplay(formInfo.ListMaintKind.Id, isDisplayMaintainanceKind);
}

/**
 * 点検種別毎一覧の表示フラグを取得
 * @param {any} listInfo 表示フラグの有無(IsDisplayMaintainanceKind)をメンバに持つ一覧の情報
  * @returns bool 点検種別ごと一覧を表示する場合True
 */
function getIsDisplayMaintainanceKind(listInfo) {
    var isDisplayMaintainanceKind = getValue(listInfo.Id, listInfo.IsDisplayMaintainanceKind, 0, CtrlFlag.Label);
    isDisplayMaintainanceKind = convertStrToBool(isDisplayMaintainanceKind); // 変換
    return isDisplayMaintainanceKind;
}

/**
 * 表示する保全活動一覧の定義を取得
 * @param {any} formInfo 保全活動一覧をメンバに持つフォームの情報
 * @returns {listInfo} 表示する保全活動一覧の定義
 */
function getDisplayMaintListInfo(formInfo) {
    var flag = getIsDisplayMaintainanceKind(formInfo.Info);
    return flag ? formInfo.ListMaintKind : formInfo.List;
}

/**
 * 保全活動一覧の集計単位に合わせて表示する列を切り替える
 * @param {int} formNo 表示中のFormNo
 */
function setMaintListColumn(formNo) {
    // FormNoより画面の情報を取得
    // 参照画面の場合True
    var isDetail = formNo == FormDetail.No;
    var formInfo = getFormInfoByFormNo(formNo);

    // 計画内容コンボの選択値
    // 参照画面でない場合は保全項目別を表示する仕様
    var valuePlanContent = isDetail ?
        getValue(FormDetail.Condition.Id, FormDetail.Condition.PlanContentExt, 0, CtrlFlag.Label) :
        FormDetail.Condition.PlanComboOptValue.Maintainance;
    // 表示する一覧の情報
    var listInfo = getDisplayMaintListInfo(formInfo);

    function setColumn() {
        // 一覧の表示状態を切り替える処理
        var setDisplayColumn = function (listId, colList, isDisplay) {
            for (valNo of colList) {
                changeTabulatorColumnDisplay(listId, valNo, isDisplay);
            }
        }

        // 非表示にする前に、一度全てを表示に戻す
        // 最上位の機器の非表示リストを取得
        var colList = getHideColDetailListForEquipment(listInfo.ColInfo);
        // 列を表示に戻す
        setDisplayColumn(listInfo.Id, colList, true);
        // 行追加などを表示に戻す
        changeRowControl(listInfo.Id, true);

        if (valuePlanContent == FormDetail.Condition.PlanComboOptValue.Maintainance) {
            // 計画内容コンボの値が保全項目の場合は全ての列を表示するので終了

            // 行追加、行削除ボタンの表示制御
            setHideRowControl(listInfo.Id);
            return;
        }

        if (isDetail) {
            // 参照画面の場合は選択列と関連コントロールをを非表示にする
            changeRowControl(listInfo.Id, false);
        }

        if (valuePlanContent == FormDetail.Condition.PlanComboOptValue.InspectionSite) {
            // 計画内容コンボの値が部位の場合は非表示にする列のリストを部位に変更する
            colList = getHideColDetailListForInspection(listInfo.ColInfo);
        }
        // 非表示
        setDisplayColumn(listInfo.Id, colList, false);
    }

    // 列表示切替
    setColumn();
}

/**
 * 点検種別ごとの表の罫線を調整する処理
 * 機器、点検種別、保全部位ごとに罫線で囲まれるようにする
 * @param {any} listInfo 保全活動一覧(点検種別毎)の定義情報
 */
function setMaintKindListGrouping(formInfo) {

    // 行の罫線を調整
    if (!getIsDisplayMaintainanceKind(formInfo.Info)) {
        // 点検種別でなければ終了
        return;
    }
    var listInfo = formInfo.ListMaintKind;
    // 表示中の一覧を取得
    var list = getRows(listInfo.Id);
    // キー値比較用
    var prevMachineId, prevKindId, prevContentId;

    // 点検種別一覧から値を取得
    var getListKindValue = function (index, valNo) {
        var value = getValue(listInfo.Id, valNo, index, CtrlFlag.Label);
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

    // グループの状態に応じてスタイルを変更
    var setGroupToCols = function (index, targetCols, isTop, isBottom) {
        $.each(targetCols, function (colIndex, colVal) {
            var targetCell = getCtrl(listInfo.Id, colVal, index, CtrlFlag.Label);
            setGroupStyle(targetCell, isTop, isBottom);
        });
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

    $.each(list, function (index, row) {
        // 機器
        // 機器番号、機器名称、機器重要度を調整
        var [newPrevMachineId, isTopMachine, isBottomMachine] = getGroupSetInfo(listInfo.ColInfo.MachineId, prevMachineId, index, list);
        prevMachineId = newPrevMachineId;
        var changeCols = [listInfo.ColInfo.MachineNo, listInfo.ColInfo.MachineName, listInfo.ColInfo.MachineImportance];
        setGroupToCols(index, changeCols, isTopMachine, isBottomMachine);

        // 点検種別
        // 点検種別を調整
        var [newPrevKindId, isTopKind, isBottomKind] = getGroupSetInfo(listInfo.ColInfo.MainKindId, prevKindId, index, list);
        prevKindId = newPrevKindId;
        changeCols = [listInfo.ColInfo.MainKind, listInfo.ColInfo.CycleYear, listInfo.ColInfo.CycleMonth, listInfo.ColInfo.CycleDay, listInfo.ColInfo.DispCycle, listInfo.ColInfo.StartDate, listInfo.ColInfo.ScheduleDate];
        setGroupToCols(index, changeCols, isTopMachine || isTopKind, isBottomMachine || isBottomKind);
        // スケジュール部分をグループ化
        setGroupToSchedules(row, isTopMachine || isTopKind, isBottomMachine || isBottomKind);

        // 保全部位
        // 保全部位を調整
        var [newPrevContentId, isTopContent, isBottomContent] = getGroupSetInfo(listInfo.ColInfo.ContentId, prevContentId, index, list);
        prevContentId = newPrevContentId;
        changeCols = [listInfo.ColInfo.MainInspection];
        setGroupToCols(index, changeCols, isTopMachine || isTopContent, isBottomMachine || isBottomContent);
    });
}

/**
 * 【オーバーライド用関数】Tabulatorの描画が完了時の処理
 * @param {any} tbl 一覧
 * @param {any} id 一覧のID(#,_FormNo付き)
 */
function postBuiltTabulator(tbl, id) {
    // 保全一覧のスタイル設定処理、行追加、行削除ボタンの表示制御
    callSetMaintListStyle(id);

    // 描画された一覧を判定
    if (id == "#" + FormList.List.Id + getAddFormNo()) { // 一覧画面
        // 一覧フィルタ処理実施
        callExecuteListFilter(FormList.List.Id, FormList.Filter.Id, FormList.Filter.Input);
        // 背景色変更処理
        commonChangeBackGroundColorHistory(tbl, FormList.List.ColumnNo.ApplicationDivisionCode, FormList.List.ColumnNo.ValueChanged, FormList.List.ColumnNo);
    } else if (id == "#" + FormDetail.List.Id + getAddFormNo()) { // 詳細画面 保全情報一覧
        // 背景色変更処理
        commonChangeBackGroundColorHistory(tbl, FormDetail.List.ColInfo.ApplicationDivisionCode);
    } else if (id == "#" + FormDetail.ListMaintKind.Id + getAddFormNo()) { // 詳細画面 点検種別毎保全情報一覧
        // 背景色変更処理
        commonChangeBackGroundColorHistory(tbl, FormDetail.ListMaintKind.ColInfo.ApplicationDivisionCode);
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
    // 保全一覧のスタイル設定処理
    callSetMaintListStyle(id);

    // 描画された一覧を判定
    if (id == "#" + FormList.List.Id + getAddFormNo()) { // 一覧画面

        // 背景色変更処理
        commonChangeBackGroundColorHistory(tbl, FormList.List.ColumnNo.ApplicationDivisionCode, FormList.List.ColumnNo.ValueChanged, FormList.List.ColumnNo);
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
    if (id == "#" + FormList.List.Id + getAddFormNo()) { // 一覧画面

        // 背景色変更処理
        commonChangeBackGroundColorHistory(tbl, FormList.List.ColumnNo.ApplicationDivisionCode, FormList.List.ColumnNo.ValueChanged, FormList.List.ColumnNo, rows);
    }
}

/**
 * 保全情報一覧のスタイル設定処理を呼び出す処理
 * @param {any} id 処理対象の一覧のID(#,_FormNo付き)
 */
function callSetMaintListStyle(id) {
    var conductid = getConductId();
    var formNo = getFormNo();
    // 機能IDが自身の機能
    if (conductid == ConductId_HM0002) {
        // 保全活動作成を表示する画面でない場合、処理終了
        if (!isMaintListForm(formNo)) {
            return;
        }
        // 以降は保全活動作成を表示する画面の処理
        // 一覧の表示を調整
        setMaintListColumn(formNo);
        // イベントの発生した一覧が、フォームの保全情報一覧(点検種別毎)かどうか判定
        var formInfo = getFormInfoByFormNo(formNo);
        if (id == "#" + formInfo.ListMaintKind.Id + getAddFormNo()) {
            // 保全活動一覧(点検種別毎)の場合、罫線調整処理を呼び出す
            // 一覧の非表示行を設定しているので、少し遅らせる
            setTimeout(function () { setMaintKindListGrouping(formInfo); }, 100);
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

    // 担当者検索画面
    SU0001_clickComConductSelectBtn(appPath, cmConductId, cmArticle, btn, fromCtrlId, ConductId_HM0002);

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

    // 担当者検索画面
    SU0001_passDataCmConduct(appPath, conductId, parentNo, conditionDataList, ctrlId, btn_ctrlId, rowNo, element, parentConductId);
}

/**
 * 【オーバーライド用関数】Tabuator一覧のヘッダー設定前処理
 * @param {string} appPath          ：アプリケーションルートパス
 * @param {string} id               ：一覧のID(#,_FormNo付き)
 * @param {object} header           ：ヘッダー情報
 * @param {Element} headerElement   ：ヘッダー要素
 */
function prevSetTabulatorHeader(appPath, id, header, headerElement) {
    var compareIds = []
    $.each(ScheduleListIds, function (index, value) {
        var settedId = "#" + value + getAddFormNo();
        compareIds.push(settedId);
    });

    if (compareIds.indexOf(id) > -1) {
        // スケジュール表示用ヘッダー情報の設定
        setScheduleHeaderInfo(appPath, header, headerElement);
    }

}

/**
 * 背景色を変更するセルのリストを取得
 */
function getColumnList() {
    var list = [];
    list.push(FormDetail.Location); // 地区～設備
    list.push(FormDetail.Job); // 職種～保全時期
    list.push(FormDetail.Purpose); // 目的区分～長計グループ
    list.push(FormDetail.Subject); // 件名～件名メモ
    list.push(FormDetail.Person); // 担当

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
 * 詳細画面のボタン、項目の表示制御
 */
function setHideButtonAndItem() {
    //処理モード
    var processMode = getValue(FormDetail.Info.Id, FormDetail.Info.ProcessMode, 1, CtrlFlag.Label);
    //トランザクションモードの場合、true
    var isTransactionMode = processMode == ProcessMode.Transaction;
    // 申請区分
    var applicationDivisionCode = getValue(FormDetail.Info.Id, FormDetail.Info.ApplicationDivisionCode, 1, CtrlFlag.Label, false, false);
    // 申請状況
    var applicationStatusCode = getValue(FormDetail.Info.Id, FormDetail.Info.ApplicationStatusCode, 1, CtrlFlag.Label, false, false);
    // 申請の申請者またはシステム管理者の場合True
    var isCertified = getValue(FormDetail.Info.Id, FormDetail.Info.IsCertified, 1, CtrlFlag.Label, false, false);
    // 工場の承認ユーザの場合True
    var isCertifiedFactory = getValue(FormDetail.Info.Id, FormDetail.Info.IsCertifiedFactory, 1, CtrlFlag.Label, false, false);

    //ボタン等の表示制御
    commonButtonHideHistory(isTransactionMode, applicationStatusCode, applicationDivisionCode, convertStrToBool(isCertified), convertStrToBool(isCertifiedFactory));

    if (processMode == ProcessMode.Transaction) {
        //申請区分、承認者、申請理由、否認理由を非表示
        changeColumnDisplay(FormDetail.ApplicationStatus.Id, FormDetail.ApplicationStatus.Division, false);
        changeColumnDisplay(FormDetail.ApplicationStatus.Id, FormDetail.ApplicationStatus.ApprovalUser, false);
        changeListDisplay(FormDetail.ApplicationReason.Id, false);
    } else {
        //申請区分、承認者、申請理由、否認理由を表示
        changeColumnDisplay(FormDetail.ApplicationStatus.Id, FormDetail.ApplicationStatus.Division, true);
        changeColumnDisplay(FormDetail.ApplicationStatus.Id, FormDetail.ApplicationStatus.ApprovalUser, true);
        changeListDisplay(FormDetail.ApplicationReason.Id, true);
    }
}

/**
 * 行追加、行削除ボタンの表示制御
 * @param {any} ctrlId 一覧のID
 */
function setHideRowControl(ctrlId) {
    //処理モード
    var processMode = getValue(FormDetail.Info.Id, FormDetail.Info.ProcessMode, 1, CtrlFlag.Label);
    //トランザクションモードの場合、true
    var isTransactionMode = processMode == ProcessMode.Transaction;
    // 申請区分
    var applicationDivisionCode = getValue(FormDetail.Info.Id, FormDetail.Info.ApplicationDivisionCode, 1, CtrlFlag.Label, false, false);
    // 申請状況
    var applicationStatusCode = getValue(FormDetail.Info.Id, FormDetail.Info.ApplicationStatusCode, 1, CtrlFlag.Label, false, false);
    // 申請の申請者またはシステム管理者の場合True
    var isCertified = getValue(FormDetail.Info.Id, FormDetail.Info.IsCertified, 1, CtrlFlag.Label, false, false);

    //行追加、行削除ボタンの表示フラグ
    //トランザクションモードの場合、または、申請状況が「申請データ作成中」or「差戻中」かつ申請者（システム管理者含む）かつ申請区分が「削除」以外　の場合表示
    var flg = (isTransactionMode ||
        (!isTransactionMode && ((applicationStatusCode == ApplicationStatus.Making && isCertified && applicationDivisionCode != ApplicationDivision.Delete)
            || (applicationStatusCode == ApplicationStatus.Return && isCertified && applicationDivisionCode != ApplicationDivision.Delete))));
    if (!flg) {
        //非表示
        changeRowControl(ctrlId, flg);
    }
}

/**
 *【オーバーライド用関数】
 *  画面状態設定後の個別実装
 *
 * @status {number}       ：ﾍﾟｰｼﾞ状態　0：初期状態、1：検索後、2：明細表示後ｱｸｼｮﾝ、3：ｱｯﾌﾟﾛｰﾄﾞ後
 * @pageRowCount {number} ：ﾍﾟｰｼﾞ全体のﾃﾞｰﾀ行数
 * @conductPtn {byte}     ：com_conduct_mst.ptn
 * @formNo {number}       ：画面番号
 */
function setPageStatusEx(status, pageRowCount, conductPtn, formNo) {
    if (formNo == FormDetail.No) {
        // 申請区分
        var applicationDivisionCode = getValue(FormDetail.Info.Id, FormDetail.Info.ApplicationDivisionCode, 1, CtrlFlag.Label, false, false);
        // 背景色変更処理
        changeBackGroundColorHistoryDetail(applicationDivisionCode, getColumnList(), FormDetail.Info.Id, FormDetail.Info.ValueChanged);
    }
}