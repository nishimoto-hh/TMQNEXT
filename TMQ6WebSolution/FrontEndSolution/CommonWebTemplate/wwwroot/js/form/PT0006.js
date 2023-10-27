/* ========================================================================
 *  機能名　    ：   【出庫入力画面】
 * ======================================================================== */
/**
 * 自身の相対パスを取得
 */
function getPath() {
    var root;
    var scripts = document.getElementsByTagName("script");
    var i = scripts.length;
    while (i--) {
        var match = scripts[i].src.match(/(^|.*\/)PT0006\.js$/);
        if (match) {
            root = match[1];
            break;
        }
    }
    return root;
}
// tmqcommon.jsが二重に呼び出されると定数の宣言などでエラーとなるので、既に呼び出されている場合は読み込まないように対応
var isExistsTmqCommon = false;
$.each(document.scripts, function (idx, value) {
    if (value.src.indexOf('tmqcommon') > 0) {
        isExistsTmqCommon = true;
    }
});
if (!isExistsTmqCommon) {
    document.write("<script src=\"" + getPath() + "/tmqcommon.js\"></script>");
}

// 一覧画面 コントロール項目番号
const PT0006_FormList = {
    ConductId: "PT0006",                        // 機能ID
    No: 0,                                      // 画面番号
    Spare: "CBODY_000_00_LST_6",           　　 // 予備品情報
    Department: {
        Id: "CBODY_010_00_LST_6",               // 部門在庫情報
        StockQuantity: 4,                       // 出庫数
        OldNewStructureId: 5,                   // 新旧区分
        DepartmentStructureId: 6,               // 部門
        AccountStructureId: 7                   // 勘定科目
    },
    InputArea: {
        Id: "CBODY_020_00_LST_6",               // 出庫情報入力
        FirstRowNo: 1,                          // 行番号
        IssueDateTime: 1,                       // 出庫日
        IssueQuantity: 2,                       // 出庫数
        IssueDivision: 3,                       // 出庫区分
        Unit: 4,                                // 単位
        ReferenceQuantity: 5,                   // 参照用出庫数
        WorkNo: 7,                              // 作業No
        FormType: 8　　　　　　　　　　　　　　 // 画面タイプ
    },
    Btn: "CBODY_030_00_BTN_6",                  // 出庫引当ボタングループ
    ResultsInventory: {
        Id: "CBODY_040_00_LST_6",               // 在庫一覧
        ReceivingDateTime: 2,                   // 入庫日
        StockQuantity: 6,                       // 在庫数
        IssueQuantity: 7,                       // 出庫数
        WorkNo: 14　　　　　　　　　　　　　　　// 作業No
    },
    PT0002Results: "BODY_050_00_LST_0"          // 出庫一覧
};

// 一覧画面 コントロール項目番号
const PT0001_FormList = {
    ConductId: "PT0001",                        // 機能ID
    No: 0                                       // 画面番号
};

// 一覧画面 コントロール項目番号
const PT0002_FormCtrlList = {
    ResultsIssueParents: "BODY_050_00_LST_0",   // 出庫検索結果一覧
    ResultsIssueChild: "BODY_100_00_LST_0",     // 出庫検索結果一覧(入れ子)
};

// ボタン名
const BtnName = {
    PrepareIssue: "PrepareIssue", // 出庫引当
    Cancel: "Cancel",             // 取消
    Regist: "Regist",             // 登録
    Back:"Back"                   // 戻る
};

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
 *  @data {List<COM_TMPTBL_DATA>}    ：初期表示ﾃﾞｰﾀ
 */
function PT0006_initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data) {

    // 機能IDが「出庫入力」ではない場合は何もしない
    if (getConductId() != PT0006_FormList.ConductId) {
        return;
    }

    // 画面タイプ取得
    var formType = getValue(PT0006_FormList.InputArea.Id, PT0006_FormList.InputArea.FormType, PT0006_FormList.InputArea.FirstRowNo, CtrlFlag.Label, false, false);

    // 取消ボタン要素取得
    var cancelBtn = getButtonCtrl(BtnName.Cancel);

    // 新規の場合
    if (formType == PartsTransFlg.New)
    {
        // 取消ボタン非活性
        changeInputControl(cancelBtn, false);

        //出庫引当ボタンにフォーカスをセット
        setFocusButton(BtnName.PrepareIssue);

        // 登録ボタン非活性
        var registBtn = getButtonCtrl(BtnName.Regist);
        changeInputControl(registBtn, false);
    }
    // 編集の場合
    else
    {
        // 入力項目、登録、出庫引当ボタンの非活性
        ChangeDisabled();
    }

    // 参照の場合
    if (formType == PartsTransFlg.Reference)
    {
        // 取消ボタン非活性
        changeInputControl(cancelBtn, false);
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
function PT0006_setPageStatusEx(status, pageRowCount, conductPtn, formNo) {
    // 機能IDが「出庫入力」ではない場合は何もしない
    if (getConductId() != PT0006_FormList.ConductId) {
        return;
    }

    // 出庫情報入力エリア取得
    var input = getCtrl(PT0006_FormList.InputArea.Id, PT0006_FormList.InputArea.IssueQuantity, PT0006_FormList.InputArea.FirstRowNo, CtrlFlag.Input, false, false);
    var unitArea = $(input).closest("div").find(".unit");
    
    // 単位なので直接値セット
    $(unitArea)[0].innerText = getValue(PT0006_FormList.InputArea.Id, PT0006_FormList.InputArea.Unit, PT0006_FormList.InputArea.FirstRowNo, CtrlFlag.Label, false, false);

}

/**
 * 【オーバーライド用関数】Tabulatorの描画が完了時の処理
 * @param {any} tbl 一覧
 * @param {any} id 一覧のID(#,_FormNo付き)
 */
function PT0006_postBuiltTabulator(tbl, id) {

    // 機能IDが「出庫入力」ではない場合は何もしない
    if (getConductId() != PT0006_FormList.ConductId) {
        return;
    }

    // 部門在庫一覧
    if (id == "#" + PT0006_FormList.Department.Id + getAddFormNo())
    {
        // 予備品一覧条件追加
        endSelectDepartmentList(tbl);
    }

    // 在庫一覧
    else if (id == "#" + PT0006_FormList.ResultsInventory.Id + getAddFormNo()) {

        // 画面タイプ取得
        var formType = getValue(PT0006_FormList.InputArea.Id, PT0006_FormList.InputArea.FormType, PT0006_FormList.InputArea.FirstRowNo, CtrlFlag.Label, false, false);

        // 新規の場合
        if (formType == PartsTransFlg.New) {

            // 在庫一覧出庫引当処理
            GoodsIssueReservation(tbl);

            // 取消ボタン非活性
            var cancelBtn = getButtonCtrl(BtnName.Cancel);
            changeInputControl(cancelBtn, false);
        }
        else
        {
            // 出庫情報の作業No取得
            var workNo = getValue(PT0006_FormList.InputArea.Id, PT0006_FormList.InputArea.WorkNo, PT0006_FormList.InputArea.FirstRowNo, CtrlFlag.Label, false, false);

            // 対象コントール内の全行取得
            var trs = tbl.getRows();
            $(trs).each(function (i, tr) {

                // html要素取得
                var ele = tr.getElement();

                // 在庫一覧の作業No取得
                var inventryWorkNo = tr.getData()['VAL' + PT0006_FormList.ResultsInventory.WorkNo];

                // 作業Noが同一であれば背景色水色
                if (workNo == inventryWorkNo)
                {
                    // 背景色水色
                    setHikiate(ele, true);
                }
                // 出庫日取得
                var inputval = getValue(PT0006_FormList.InputArea.Id, PT0006_FormList.InputArea.IssueDateTime, PT0006_FormList.InputArea.FirstRowNo, CtrlFlag.TextBox, false, false);
                // 日付に変換
                var date1 = new Date(inputval);

                // 入庫日取得
                var inDate = tr.getData()['VAL' + PT0006_FormList.ResultsInventory.ReceivingDateTime];
                //日付に変換
                var date2 = new Date(inDate);

                // 大小チェック(出庫日が入庫日より早ければ背景グレーに着色)
                if (lowerThanDateOnly(date1, date2)) {
                    $(ele).addClass("hikiateGray");
                }

            });
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
function PT0006_prevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element) {
    var conditionDataList = [];

    // 遷移元が部門在庫情報の場合
    if (ctrlId == PT0006_FormList.Department.Id)
    {
        // 部門在庫一覧取得
        var table = P_listData['#' + ctrlId + getAddFormNo()];
        if (table)
        {
            var trs = table.getRows();
            $(trs).each(function (i, tr) {
                var ele = tr.getElement();
                if (i == rowNo - 1) {
                    // 選択行背景水色
                    setHikiate(ele, true);

                    // 在庫一覧の条件を追加
                    selectConditionList(element);

                    // 部門在庫情報が1件より多い場合
                    if (i != 0)
                    {
                        // 在庫一覧非表示、登録ボタン非活性
                        hideBtnAndList();
                    }
                }
                else
                {
                    setHikiate(ele, false);
                }
            });
        }
        return [false, conditionDataList];
    }
    return [true, conditionDataList];

}

/**
 * 【オーバーライド用関数】実行正常終了後処理
 *  @param {string}                     appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string}                     conductId   ：機能ID
 *  @param {string}                     pgmId       ：プログラムID
 *  @param {number}                     formNo      ：画面NO
 *  @param {html}                       btn         ：ボタン要素
 *  @param {number}                     conductPtn  ：機能処理ﾊﾟﾀｰﾝ
 *  @param {boolean}                    autoBackFlg ：ajax正常終了後、自動戻るフラグ　false:戻らない、true:自動で戻る
 *  @param {boolean}                    isEdit      ：単票表示フラグ
 *  @param {List<Dictionary<string>>}   data        ：結果ﾃﾞｰﾀ
 */
function PT0006_postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data) {
    // 機能IDが「出庫入力」ではない場合は何もしない
    if (conductId != PT0006_FormList.ConductId) {
        return;
    }

    // 登録・取消ボタン実行正常終了後画面を閉じて遷移元に移動
    var modal = $(btn).closest('section.modal_form');
    $(modal).modal('hide');
}

/**
 * 出庫入力画面の遷移前処理を行うかどうかの判定
 *  @param appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param transPtn    ：画面遷移ﾊﾟﾀｰﾝ(1：子画面、2：単票入力、4：共通機能、5：他機能別ﾀﾌﾞ、6：他機能表示切替)
 *  @param transDiv    ：画面遷移アクション区分(1：新規、2：修正、3：複製、4：出力、)
 *  @param transTarget ：遷移先(子画面NO / 単票一覧ctrlId / 共通機能ID / 他機能ID|画面NO)
 *  @param dispPtn     ：遷移表示ﾊﾟﾀｰﾝ(1：表示切替、2：ﾎﾟｯﾌﾟｱｯﾌﾟ、3：一覧直下)
 *  @param formNo      ：遷移元formNo
 *  @param pctrlId      ：遷移元の一覧ctrlid
 *  @param btn_ctrlId  ：ボタンのbtn_ctrlid
 *  @param rowNo       ：遷移元の一覧の選択行番号（新規の場合は-1）
 *  @param element     ：ｲﾍﾞﾝﾄ発生要素
 *  @returns {bool} Trueの場合はDM0002_prevTransFormの処理を行う、Falseの場合は他の画面に遷移する際の前処理
 */
function IsExecPT0006_PrevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element) {
    var result = ctrlId == PT0006_FormList.Department.Id;
    return result;
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
function PT0006_beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom, transPtn) {

    // 共通画面を閉じた場合、指定した画面ならば再検索を行う
    InitFormDataByCommonModal(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom);
}

/**
 *【オーバーライド用関数】
 *  検索処理後
 *
 *  @appPath {string} 　：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @btn {string} 　　　：対象ボタン
 *  @conductId {string} ：機能ID
 *  @pgmId {string} 　　：プログラムID
 *  @formNo {number} 　 ：画面番号
 *  @conductPtn {number}：処理ﾊﾟﾀｰﾝ
 */
function PT0006_afterSearchBtnProcess(appPath, btn, conductId, pgmId, formNo, conductPtn) {
    if (conductId != PT0006_FormList.ConductId) {
        return;
    }
    setPageStatus(pageStatus.SEARCH, 0, conductPtn);
}

/**
 * 部門在庫一覧検索後処理
 * @param {any} tbl 部門在庫一覧情報
 */
function endSelectDepartmentList(tbl)
{
    // 対象コントール内の全行取得
    var trsDpt = tbl.getRows();
    $(trsDpt).each(function (i, tr) {

        // 1行目に着色
        if (i == 0) {
            // html要素取得
            var ele = tr.getElement();
            setHikiate(ele, true);

            // 在庫一覧の条件を追加
            selectConditionListByRowNo(0);
        }
    });
}

/**
 * 在庫一覧検索後処理
 * @param {any} tbl 在庫一覧情報
 */
function GoodsIssueReservation(tbl)
{
    // 出庫日取得
    var inputval = getValue(PT0006_FormList.InputArea.Id, PT0006_FormList.InputArea.IssueDateTime, PT0006_FormList.InputArea.FirstRowNo, CtrlFlag.TextBox, false, false);
    // 日付に変換
    var date1 = new Date(inputval);

    // 出庫数取得
    var inputval = getValue(PT0006_FormList.InputArea.Id, PT0006_FormList.InputArea.IssueQuantity, PT0006_FormList.InputArea.FirstRowNo, CtrlFlag.TextBox, false, false);
    if (inputval.slice(0, 1) == '.') {
        inputval = "0" + inputval// 先頭が小数点の場合は0を付ける
    }

    // 引き当てされてから登録まで数量が変わらないか見る用保持
    setValue(PT0006_FormList.InputArea.Id, PT0006_FormList.InputArea.ReferenceQuantity, PT0006_FormList.FirstRowNo, CtrlFlag.Label, inputval);

    // 対象コントール内の全行取得
    var trs = tbl.getRows();
    $(trs).each(function (i, tr) {

        // html要素取得
        var ele = tr.getElement();

        // 入庫日取得
        var inDate = tr.getData()['VAL' + PT0006_FormList.ResultsInventory.ReceivingDateTime];
        //日付に変換
        var date2 = new Date(inDate);

        // 大小チェック(出庫日が入庫日より早ければ背景グレーに着色)
        if (lowerThanDateOnly(date1, date2)) {
            $(ele).addClass("hikiateGray");
        }
    });

    // 繰り越し在庫量
    let carryOver = -1;

    // 初回フラグ
    var firstFlg = true;

    // 引き当て
    $(trs).each(function (i, tr) {

        //割り当て済みであればスルー
        if (carryOver == 0) { return true; }

        // html要素取得
        var ele = tr.getElement();

        // 在庫数取得
        var stockNum = tr.getData()['VAL' + PT0006_FormList.ResultsInventory.StockQuantity].replace(/[^0-9.]/g, '');

        // 出庫数単位取得
        var outNumUnit = tr.getData()['VAL' + PT0006_FormList.ResultsInventory.IssueQuantity].replace(/[0-9.]/g, '');

        // 初回のみ入力値で比較
        if (carryOver != -1) { firstFlg = false; }

        // 出庫数が在庫数より多い場合は次の行を見る & 背景水色
        if (firstFlg && parseFloat(inputval) >= parseFloat(stockNum)) {
            // 背景水色
            setHikiate(ele, true);

            // 出庫数に値をセット
            setValue(PT0006_FormList.ResultsInventory.Id, PT0006_FormList.ResultsInventory.IssueQuantity, i, CtrlFlag.Label, stockNum + outNumUnit);

            // 残出庫数
            carryOver = parseFloat(inputval) - parseFloat(stockNum);
        }
        else if (carryOver > parseFloat(stockNum)) {
            // 背景水色
            setHikiate(ele, true);

            // 出庫数に値をセット
            setValue(PT0006_FormList.ResultsInventory.Id, PT0006_FormList.ResultsInventory.IssueQuantity, i, CtrlFlag.Label, stockNum + outNumUnit);
            // 残出庫数
            carryOver = carryOver - parseFloat(stockNum);
        }
        // 出庫数があれば終了
        else if (parseFloat(inputval) > 0) {
            // 背景水色
            setHikiate(ele, true);

            if (carryOver != -1) {
                if (String(carryOver).includes(".")) {
                    // 出庫数に値をセット
                    setValue(PT0006_FormList.ResultsInventory.Id, PT0006_FormList.ResultsInventory.IssueQuantity, i, CtrlFlag.Label, carryOver.toFixed(2) + outNumUnit);
                }
                else
                {
                    setValue(PT0006_FormList.ResultsInventory.Id, PT0006_FormList.ResultsInventory.IssueQuantity, i, CtrlFlag.Label, carryOver + outNumUnit);
                }

            }
            else {
                if (inputval.includes(".")) {
                    // 出庫数に値をセット
                    setValue(PT0006_FormList.ResultsInventory.Id, PT0006_FormList.ResultsInventory.IssueQuantity, i, CtrlFlag.Label, parseFloat(inputval).toFixed(2) + outNumUnit);
                }
                else {
                    setValue(PT0006_FormList.ResultsInventory.Id, PT0006_FormList.ResultsInventory.IssueQuantity, i, CtrlFlag.Label, inputval + outNumUnit);
                }
            }
            // 割り当て完了
            carryOver = 0;
        }
    });
}

/**
 * 在庫一覧の条件を追加
 * @param {any} element 行の要素
 */
function selectConditionList(element)
{
    var conditionData = {};
    conditionData['CTRLID'] = 'SelectCondition';
    conditionData['StockQuantity'] = getSiblingsValue(element, PT0006_FormList.Department.StockQuantity, CtrlFlag.Label);                     // 選択した部門の在庫数
    conditionData['OldNewStructureId'] = getSiblingsValue(element, PT0006_FormList.Department.OldNewStructureId, CtrlFlag.Label);             // 新旧区分
    conditionData['DepartmentStructureId'] = getSiblingsValue(element, PT0006_FormList.Department.DepartmentStructureId, CtrlFlag.Label);     // 部門
    conditionData['AccountStructureId'] = getSiblingsValue(element, PT0006_FormList.Department.AccountStructureId, CtrlFlag.Label);           // 勘定科目

    // 個別実装用変数に条件追加
    P_dicIndividual = conditionData;
}

/**
 * 部門在庫一覧の条件を追加
 * @param {any} rowNo 行番号
 */
function selectConditionListByRowNo(rowNo) {
    var conditionData = {};
    conditionData['CTRLID'] = 'SelectCondition';
    conditionData['StockQuantity'] = getValue(PT0006_FormList.Department.Id, PT0006_FormList.Department.StockQuantity, rowNo, CtrlFlag.Label, false, false);                    // 選択した部門の在庫数
    conditionData['OldNewStructureId'] = getValue(PT0006_FormList.Department.Id, PT0006_FormList.Department.OldNewStructureId, rowNo, CtrlFlag.Label, false, false);            // 新旧区分
    conditionData['DepartmentStructureId'] = getValue(PT0006_FormList.Department.Id, PT0006_FormList.Department.DepartmentStructureId, rowNo, CtrlFlag.Label, false, false);    // 部門
    conditionData['AccountStructureId'] = getValue(PT0006_FormList.Department.Id, PT0006_FormList.Department.AccountStructureId, rowNo, CtrlFlag.Label, false, false);          // 勘定科目

    // 個別実装用変数に条件追加
    P_dicIndividual = conditionData;
}

/**
 * 入力項目の非活性
 */
function ChangeDisabled() {

    // 出庫日の活性/非活性
    var issueDay = getCtrl(PT0006_FormList.InputArea.Id, PT0006_FormList.InputArea.IssueDateTime, PT0006_FormList.InputArea.FirstRowNo, CtrlFlag.Input);
    changeInputControl(issueDay, false);

    // 出庫数の活性/非活性
    var issueNum = getCtrl(PT0006_FormList.InputArea.Id, PT0006_FormList.InputArea.IssueQuantity, PT0006_FormList.InputArea.FirstRowNo, CtrlFlag.TextBox);
    changeInputControl(issueNum, false);

    // 出庫区分の活性/非活性
    var issueClass = getCtrl(PT0006_FormList.InputArea.Id, PT0006_FormList.InputArea.IssueDivision, PT0006_FormList.InputArea.FirstRowNo, CtrlFlag.Combo);
    changeInputControl(issueClass, false);

    // 出庫引当ボタン非活性
    var issueBtn = getButtonCtrl(BtnName.PrepareIssue);
    changeInputControl(issueBtn, false);

    // 登録ボタン非活性
    var registBtn = getButtonCtrl(BtnName.Regist);
    changeInputControl(registBtn, false);
    
}

/**
 * 在庫一覧非表示、登録ボタン非活性
 */
function hideBtnAndList() {
    // 在庫一覧非表示
    var ResultsInventoryList = $(P_Article).find("#" + PT0006_FormList.ResultsInventory.Id + getAddFormNo()).find(".tabulator-tableholder");
    ResultsInventoryList.hide();

    // 登録ボタン非活性
    var registBtn = getButtonCtrl(BtnName.Regist);
    $(registBtn).prop("disabled", true);
}

/**
 * 日付比較関数
 */
function lowerThanDateOnly(date1, date2) {

    // 日付1の年月日取得
    var year1 = date1.getFullYear();
    var month1 = date1.getMonth() + 1;
    var day1 = date1.getDate();

    // 日付2の年月日取得
    var year2 = date2.getFullYear();
    var month2 = date2.getMonth() + 1;
    var day2 = date2.getDate();

    // 年月日ごとに比較
    if (year1 == year2) {
        if (month1 == month2) {
            return day1 < day2;
        }
        else {
            return month1 < month2;
        }
    } else {
        return year1 < year2;
    }
}