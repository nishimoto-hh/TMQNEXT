/* ========================================================================
 *  機能名　    ：   【LN0002】機器別長期計画画面
 * ======================================================================== */
/**
 * 自身の相対パスを取得
 */
function getPath() {
    var root;
    var scripts = document.getElementsByTagName("script");
    var i = scripts.length;
    while (i--) {
        var match = scripts[i].src.match(/(^|.*\/)LN0002\.js$/);
        if (match) {
            root = match[1];
            break;
        }
    }
    return root;
}
document.write("<script src=\"" + getPath() + "/tmqcommon.js\"></script>");
document.write("<script src=\"" + getPath() + "/RM0001.js\"></script>");

// 機能ID
const ConductId_LN0002 = "LN0002";

// 一覧画面の定義
const FormList = {
    No: 0
    , List: { Id: "BODY_040_00_LST_0", MachineNo: 1, MachineName: 2, ComponentId: 3, ImpotanceId: 4, ConversationId: 5, ContentId: 6, GroupKey: 51, MachineId: 52, KeyId: 53 }
    , Filter: { Id: "BODY_010_00_LST_0", Input: 1 }
    , Condition: { Id: "BODY_020_00_LST_0", Schedule: { Unit: 1, Year: 2, Month: 3, Ext: 4 } } // スケジュール表示条件※共通
    , Button: { Output: "btnOutPut" }
}
// 予算出力画面の定義
const FormReport = {
    No: 1
}
// グループ化の単位、機器番号と機器名称
const ListMachineGroups = [FormList.List.MachineNo, FormList.List.MachineName];
// グループ化の単位、部位
const ListComponentGroups = [FormList.List.ComponentId];
// グループ化の単位、内容
const ListContentGroups = [FormList.List.ContentId, FormList.List.ImpotanceId, FormList.List.ConversationId];

// スケジュール表示単位の拡張項目の値
const ScheduleUnit = { Year: 2, Month: 1 };

// 機器台帳の長期計画タブのNo
const TabNoMc0001 = 4;

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

        setFocusButton(FormList.Button.Output);
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

    setListGroupStyleCall(id);
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
 * 【オーバーライド関数】ツリー展開時追加処理
 * @param {string} listId 一覧のID(#,_FormNo付き)
 * @param {any} row 展開対象の行
 * @param {any} level 展開ツリーのレベル
 */
function afterDataTreeRowExpanded(listId, row, level) {
    setListGroupStyleCall(listId);
}

/**
 * 【オーバーライド用関数】Tabulatorのページ変更後の処理
 * @param {any} tbl 処理対象の一覧
 * @param {any} id 処理対象の一覧のID(#,_FormNo付き)
 * @param {any} pageNo 表示するページNo
 * @param {any} pagesize 表示中のページの件数
 */
function postTabulatorChangePage(tbl, id, pageNo, pagesize) {
    setListGroupStyleCall(id);
}

/**
 * 一覧のグループごと書式設定を呼び出す処理
 * @param {any} id 処理対象の一覧のID(#,_FormNo付き)
 */
function setListGroupStyleCall(id) {
    // Tabulatorの描画が完了後（renderComplete完了後）の処理
    var formNo = getFormNo();
    if (formNo != FormList.No) {
        // 一覧画面以外の場合、終了
        return;
    }
    if (id != "#" + FormList.List.Id + getAddFormNo()) {
        return;
    }
    // 罫線調整処理
    setListGroupStyle(FormList.List.Id, [FormList.List.GroupKey], ListMachineGroups); // 機器
    setListGroupStyle(FormList.List.Id, [FormList.List.GroupKey, FormList.List.ComponentId], ListComponentGroups); // 部位
    setListGroupStyle(FormList.List.Id, [FormList.List.GroupKey, FormList.List.ComponentId, FormList.List.ContentId], ListContentGroups); // 内容

    // 子の行の選択を無効化
    var table = $(P_Article).find("#" + FormList.List.Id + getAddFormNo()).find("div.tabulator-table"); // 一覧
    var rows = getRows(table, 'SELTAG').find("input"); // 選択行のチェックボックスのリスト
    $(rows).each(function (index, chk) { // 繰り返し
        var row = $(chk).closest('.tabulator-row'); // チェックボックスの行
        var rowno = getRows(row, "ROWNO").find("a"); // 行番号の列のリンク
        if (rowno.length == 0) {
            // 子の行は行番号リンクが無いので、選択を非表示にする
            setHide($(chk), true);
        }
    });
}
/**
 * 要素から指定列の集合を取得
 * @param {any} parent 取得対象の要素
 * @param {any} target 取得対象の列
 * @return {any} 取得行の集合
 */
function getRows(parent, target) {
    var rows = $(parent).find("div[tabulator-field='" + target + "']");
    return rows;
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

    if (id == "#" + FormList.List.Id + getAddFormNo()) {
        var copyVals = [FormList.List.MachineId, FormList.List.KeyId];
        convertTabulatorListToGroup(options, FormList.List.GroupKey, ListMachineGroups, copyVals);
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

    if (formNo == FormList.No) {
        // 一覧画面
        if (transTarget == FormReport.No || transTarget == RM00001_ConductId) {
            // 帳票出力画面に遷移する場合、選択行が無い場合にエラーとする
            // 一覧が選択されているか判定
            var result = isCheckedList(FormList.List.Id);
            if (!result) {
                // 未選択ならエラーなので終了
                return [false, conditionDataList];
            }
            // 選択された行をパラメータに設定する
            const ctrlIdList = [FormList.List.Id, FormList.Condition.Id];
            conditionDataList = getListDataByCtrlIdList(ctrlIdList, formNo, 0)
        } else if (transPtn == transPtnDef.OtherTab && transTarget.startsWith('MC0001')) {
            // 機器台帳画面に遷移する場合、パラメータを設定する
            var machineId = getSiblingsValue(element, FormList.List.MachineId, CtrlFlag.Label);
            conditionDataList = getParamToMC0001(machineId, TabNoMc0001);
        }
    }

    return [true, conditionDataList];
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
    // スケジュール表示単位コンボを強制的に変更し、変更時イベントを設定する処理を呼び出し
    var scheduleExec = function (listInfo) {
        setComboExValueOnPrevInitFormData(conditionDataList, listInfo.No, listInfo.Condition.Id, listInfo.Condition.Schedule.Unit, listInfo.Condition.Schedule.Ext, CtrlFlag.Label);
    }
    // スケジュール表示期間の初期値を設定する処理
    var setYearScheduleFromTo = function () {
        // Fromの項目
        var from = getCtrl(FormList.Condition.Id, FormList.Condition.Schedule.Year, 0, CtrlFlag.Input);
        // TOの項目
        var to = $(from).parent().siblings("div").find("input");
        //値
        var fromYear = $(from).val();
        var toYear = $(to).val();
        // 検索条件に反映
        setValueToConditionDataList(conditionDataList, FormList.No, FormList.Condition.Id, FormList.Condition.Schedule.Year, fromYear + "|" + toYear);
    }
    // スケジュール表示年の初期値
    var setStartYear = function () {
        //値
        var year = getValue(FormList.Condition.Id, FormList.Condition.Schedule.Month, 0, CtrlFlag.TextBox);
        setValueToConditionDataList(conditionDataList, FormList.No, FormList.Condition.Id, FormList.Condition.Schedule.Month, year);
    }

    if (formNo == FormList.No) {
        // 一覧画面
        // 初期表示時、スケジュール表示単位の拡張項目の値がセットされていないため、値をセットしないと初期表示時の検索で値が取得できない
        scheduleExec(FormList);

        // スケジュール表示年度の初期値設定
        setYearScheduleFromTo();　// FromとToに年を設定
        setStartYear(); // 年をセット

        // 条件エリアもページ情報に追加
        listDefines = addSearchConditionToListDefine(listDefines, transPtnDef.None, FormList.No, -1);
    }


    return [conditionDataList, listDefines];
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
    if (formNo == FormList.No) {
        // 一覧画面
        // スケジュール表示単位コンボ
        eventChangeScheduleUnit(FormList);
    }
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

    if (id == "#" + FormList.List.Id + getAddFormNo()) {
        // スケジュール表示用ヘッダー情報の設定
        setScheduleHeaderInfo(appPath, header, headerElement);
    }

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

}

