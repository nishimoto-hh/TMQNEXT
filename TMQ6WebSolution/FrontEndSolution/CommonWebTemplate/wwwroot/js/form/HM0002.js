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
document.write("<script src=\"" + getPath() + "/RM0001.js\"></script>");

// 保全活動一覧関連の定義 ↓
// 保全情報一覧の列情報
const MaintList = {
    MachineNo: 1, MachineName: 2, MachineImportance: 3, MainInspection: 4, MainContent: 5, Budget: 6, ScheduleType: 7,
    CycleYear: 8, CycleMonth: 9, CycleDay: 10, DispCycle: 11, StartDate: 12, ScheduleDate: 13, MainKind: 14
};
// 保全情報一覧(点検種別毎)の列情報
const MaintListKind = {
    MachineNo: 1, MachineName: 2, MachineImportance: 3, MainKind: 4, MainInspection: 5, MainContent: 6, Budget: 7, ScheduleType: 8,
    CycleYear: 9, CycleMonth: 10, CycleDay: 11, DispCycle: 12, StartDate: 13, ScheduleDate: 14, MachineId: 56, ContentId: 53
};

/**
 * フォームNoで保全活動一覧を表示する画面か判定する
 * @param {any} formNo フォームNo
 * @return {bool} 表示する場合True
 */
function isMaintListForm(formNo) {
    var formNoList = [FormDetail.No, FormMakeMaintainance.No, FormPostpone.No];
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
            Facility: 25,                // 設備区分
            Content: 30,                 // 保全情報変更有無
            ApplicationDivisionCode: 55, // 申請区分コード(拡張項目)
            ValueChanged: 56             // 変更のあった項目
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
}
// 参照画面の定義
const FormDetail = {
    No: 1
    // 非表示の退避項目
    , Info: {
        Id: "BODY_100_00_LST_1"
        , LongPlanId: 1
        , IsDisplayMaintainanceKind: 3 // 保全情報一覧(点検種別毎)の表示是非
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
    , Button: { MakeMaintainance: "btnMakeMaintainance", AddSubject: "btnAddSubject", Schedule: "btnUpdateSchedule" }
    , Person: { Id: "BODY_040_00_LST_1", Code: 1, Name: 2 }
};

// 詳細編集画面の定義
const FormEdit = { No: 2, Button: { Regist: "Regist" }, Hide: { Id: "BODY_050_00_LST_2" } };
// 計画一括作成画面の定義
const FormMakePlan = { No: 3 };
// 保全活動作成画面の定義
const FormMakeMaintainance = {
    No: 4
    , Info: {
        Id: "BODY_000_00_LST_4", IsDisplayMaintainanceKind: 3 // 保全情報一覧(点検種別毎)の表示是非
    }
    , List: { Id: "BODY_010_00_LST_4", ColInfo: MaintList }
    , ListMaintKind: { Id: "BODY_020_00_LST_4", ColInfo: MaintListKind }
};
// 機器別管理基準選択画面の定義
const FormSelect = {
    No: 5
    , List: { Id: "BODY_040_00_LST_5" }
    , Hide: { Id: "BODY_060_00_LST_5" }
    , Location: { Id: "COND_000_00_LST_5" }
    , Job: { Id: "COND_010_00_LST_5" }
};
// 予算出力画面の定義
const FormReport = {
    No: 6
}
// 予定作業一括延期画面の定義
const FormPostpone = {
    No: 7
    , Info: {
        Id: "BODY_000_00_LST_7", IsDisplayMaintainanceKind: 3 // 保全情報一覧(点検種別毎)の表示是非
    }
    // 保全情報一覧
    , List: { Id: "BODY_010_00_LST_7", ColInfo: MaintList }
    // 保全情報一覧(点検種別毎)
    , ListMaintKind: { Id: "BODY_020_00_LST_7", ColInfo: MaintListKind }
};

// スケジュールを表示する一覧
// 一覧画面（通常の一覧のみ）
// 参照画面、保全活動作成画面、予定作業一括延期画面（通常の一覧または点検種別毎の一覧）
const ScheduleListIds = [FormList.List.Id,
FormDetail.List.Id, FormDetail.ListMaintKind.Id,
FormMakeMaintainance.List.Id, FormMakeMaintainance.ListMaintKind.Id,
FormPostpone.List.Id, FormPostpone.ListMaintKind.Id
];


/**
 * フォーム番号より一致するフォームの情報を取得する
 * @param {any} formNo フォーム番号
 * @return {formInfo} フォームの定義情報
 */
function getFormInfoByFormNo(formNo) {
    var numFormNo = formNo - 0;
    var list = [FormList, FormDetail, FormEdit, FormMakePlan, FormMakeMaintainance, FormSelect, FormPostpone];
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

    // 機能IDが「帳票出力」の場合
    if (getConductId() == RM00001_ConductId) {
        return RM0001_initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data);
    }

    if (conductId != ConductId_HM0002) {
        // HM0002以外なら終了
        return;
    }
    // 以降は件名別長期計画一覧の処理

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

        // 一覧画面の場合、新規申請ボタンにフォーカスをセット
        // 押下不能なら出力ボタン
        setFocusButtonAvailable(FormList.Button.New, FormList.Button.Output);
    } else if (formNo == FormDetail.No) {
        // 両方表示されている場合は制御
        controlVisibleScheduleCond(FormDetail);

        // 参照画面　件名添付
        setFocusButton(FormDetail.Button.AddSubject);
    }

    if (isMaintListForm(formNo)) {
        // 保全活動一覧を表示する画面の場合、表示制御
        setVisibleMaintList(getFormInfoByFormNo(formNo));
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
        // 計画内容コンボの初期値を設定する(拡張項目1の値が保全項目)
        selectComboByExparam(FormDetail.Condition.Id, FormDetail.Condition.PlanContent, 1, FormDetail.Condition.PlanComboOptValue.Maintainance);
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

    // 機能IDが「帳票出力」の場合
    RM0001_beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom);


    // 共通-担当者検索画面を閉じたとき
    SU0001_beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom);

    // 参照画面の場合、戻る/閉じるボタン表示制御
    if (formNo == FormDetail.No) {
        setDisplayCloseBtn(transPtn);
    }
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
        // 件名別長期計画の場合
        if (formNo == FormMakeMaintainance.No) {
            // 保全活動作成画面の場合
            // 表示している保全情報一覧のID
            var listInfo = getDisplayMaintListInfo(FormMakeMaintainance);
            // 選択チェック
            if (!isCheckedList(listInfo.Id)) {
                // 一覧が未選択の場合エラー
                return false;
            }
        } else if (formNo == FormSelect.No) {
            // 機器別管理基準選択画面の場合
            // 選択チェック
            if (!isCheckedList(FormSelect.List.Id)) {
                // 一覧が未選択の場合エラー
                return false;
            }
        } else if (formNo == FormPostpone.No) {
            // 予定作業一括延期画面の場合
            // チェック対象の一覧のID
            var listId = getDisplayMaintListInfo(FormPostpone).Id;
            // 選択チェック
            if (!isCheckedList(listId)) {
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

        // 検索条件に追加する一覧のID
        var ctrlIdList = [FormDetail.Info.Id]; // 非表示項目退避

        // 編集or複写で詳細編集画面へ遷移する場合True
        var isToEdit = transTarget == FormEdit.No && (transDiv == transDivDef.Edit || transDiv == transDivDef.Copy);
        // 保全活動作成で保全活動作成画面へ遷移する場合True
        var isToMakeMaint = transTarget == FormMakeMaintainance.No;
        // 機器別管理基準選択画面に遷移する場合
        var isSelect = transTarget == FormSelect.No;
        // 予定作業一括延期画面に遷移する場合
        var isPostpone = transTarget == FormPostpone.No;

        if (isToMakeMaint || isPostpone) { //保全活動作成か予定作業一括延期の場合
            // スケジュールの表示条件が必要なので追加
            ctrlIdList.push(FormDetail.Condition.Id);
        }

        if (isToEdit || isToMakeMaint || isSelect || isPostpone) {
            // 検索条件に追加
            conditionDataList = getListDataByCtrlIdList(ctrlIdList, formNo, 0);
        }
    } else if (formNo == FormList.No) {
        // 一覧画面で遷移する場合
        //if (transTarget == FormMakePlan.No || transTarget == FormReport.No || transTarget == RM00001_ConductId) {
        //    // 計画一括作成 or 予算出力への遷移の場合 or 共通の出力画面への遷移の場合
        //    // 一覧が選択されているか判定
        //    var result = isCheckedList(FormList.List.Id);
        //    if (!result) {
        //        // 未選択ならエラーなので終了
        //        return [false, conditionDataList];
        //    }
        //    // 選択された行をパラメータに設定する
        //    const ctrlIdList = [FormList.List.Id, FormList.Condition.Id];
        //    conditionDataList = getListDataByCtrlIdList(ctrlIdList, formNo, 0)
        //    return [result, conditionDataList];
        //} else if (transTarget == FormDetail.No) {
        //    // 参照画面への遷移の場合
        //    conditionDataList = getConditionDataListToDetail();
        //}
        // 一括承認(画面遷移)、一括否認(画面遷移)、出力ボタンがクリックされた場合
        if (btn_ctrlId == FormList.Button.ApprovalAllTrans || btn_ctrlId == FormList.Button.DenialAllTrans || btn_ctrlId == FormList.Button.Output) {

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

    // 機能IDが「帳票出力」の場合
    if (getConductId() == RM00001_ConductId) {
        return RM0001_postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data);
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
    } else if (formNo == FormDetail.No && btnName == FormDetail.Button.Schedule) {
        // 参照画面のスケジュール確定ボタンの場合、非表示項目を追加
        return getListDataByCtrlIdList([FormDetail.Info.Id, FormDetail.Condition.Id], formNo, 0, true);
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

    return [false, false, true];
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
        var name = getValue(FormDetail.Person.Id, FormDetail.Person.Name, 0, CtrlFlag.Label);
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
        var [newPrevKindId, isTopKind, isBottomKind] = getGroupSetInfo(listInfo.ColInfo.MainKind, prevKindId, index, list);
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

    // 機能IDが「帳票出力」の場合
    if (getConductId() == RM00001_ConductId) {
        return RM0001_postBuiltTabulator(tbl, id);
    }

    // 保全一覧のスタイル設定処理
    callSetMaintListStyle(id);

    // 描画された一覧を判定
    if (id == "#" + FormList.List.Id + getAddFormNo()) { // 一覧画面

        // 背景色変更処理
        commonChangeBackGroundColorHistory(tbl, FormList.List.ColumnNo.ApplicationDivisionCode, FormList.List.ColumnNo.ValueChanged, FormList.List.ColumnNo);
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

    // 共通-帳票出力画面の個別実装ボタン
    RM0001_passDataCmConduct(appPath, conductId, parentNo, conditionDataList, ctrlId, btn_ctrlId, rowNo, element, parentConductId);

    // 担当者検索画面
    SU0001_passDataCmConduct(appPath, conductId, parentNo, conditionDataList, ctrlId, btn_ctrlId, rowNo, element, parentConductId);
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
    return true;
}

/**
 * 【オーバーライド用関数】Tabuator一覧のヘッダー設定前処理
 * @param {string} appPath          ：アプリケーションルートパス
 * @param {string} id               ：一覧のID(#,_FormNo付き)
 * @param {object} header           ：ヘッダー情報
 * @param {Element} headerElement   ：ヘッダー要素
 */
function prevSetTabulatorHeader(appPath, id, header, headerElement) {
    // Tabuator一覧に個別で追加したい列等の設定が行えます
    // 機能IDが「帳票出力」の場合
    if (getConductId() == RM00001_ConductId) {
        return;
    }

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
 * 【オーバーライド用関数】Tabuator一覧の描画前処理
 * @param {string} appPath  ：アプリケーションルートパス
 * @param {string} id       ：一覧のID(#,_FormNo付き)
 * @param {string} options  ： 一覧のオプション情報
 * @param {object} header   ：ヘッダー情報
 * @param {object} dispData ：データ
 */
function prevCreateTabulator(appPath, id, options, header, dispData) {

    // 機能IDが「帳票出力」の場合
    if (getConductId() == RM00001_ConductId) {
        return RM0001_prevCreateTabulator(appPath, id, options, header, dispData);
    }

}