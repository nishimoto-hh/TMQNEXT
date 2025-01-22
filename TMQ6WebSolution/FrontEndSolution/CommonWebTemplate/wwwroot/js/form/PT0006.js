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
    DispYearFrom: 7,                            // 表示年度(From) ※予備品詳細画面に戻った際に使用するための値を保持する場所
    DispYearTo: 8,                              // 表示年度(To) ※予備品詳細画面に戻った際に使用するための値を保持する場所
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
        FormType: 8,　　　　　　　　　　　　　  // 画面タイプ
        Digit: 9,                               // 小数点以下桁数
        RoundDivision: 10                       // 丸め処理区分
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

// 予備品一覧画面 コントロール項目番号
const PT0001_FormList = {
    ConductId: "PT0001",                        // 機能ID
    No: 0                                       // 画面番号
};
// 予備品詳細画面 コントロール項目番号
const PT0001_FormDetail = {
    ConductId: "PT0001",                        // 機能ID
    No: 1                                       // 画面番号
}

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
    Back: "Back"                   // 戻る
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
    if (formType == PartsTransFlg.New) {
        // 取消ボタン非活性
        changeInputControl(cancelBtn, false);

        //出庫引当ボタンにフォーカスをセット
        setFocusButton(BtnName.PrepareIssue);

        // 登録ボタン非活性
        var registBtn = getButtonCtrl(BtnName.Regist);
        changeInputControl(registBtn, false);
    }
    // 編集の場合
    else {
        // 入力項目、登録、出庫引当ボタンの非活性
        ChangeDisabled();
    }

    // 参照の場合
    if (formType == PartsTransFlg.Reference) {
        // 取消ボタン非活性
        changeInputControl(cancelBtn, false);
    }

    // 出庫数からフォーカスアウトした場合
    var quantity = getCtrl(PT0006_FormList.InputArea.Id, PT0006_FormList.InputArea.IssueQuantity, 1, CtrlFlag.TextBox, false, false);
    $(quantity).blur(function () {
        PT0006roundQuantity();
    });

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

    // 棚別部門別在庫一覧
    if (id == "#" + PT0006_FormList.Department.Id + getAddFormNo()) {

        // 画面パターンを取得
        var formType = getValue(PT0006_FormList.InputArea.Id, PT0006_FormList.InputArea.FormType, 1, CtrlFlag.Label, false, false);

        // 一覧の行を取得
        var rows = $(tbl.element).find(".tabulator-row");

        // 先頭行のチェックボックス(検索後に選択状態にするためのもの)
        var firstRowCheckBox;

        // 取得した行に対して繰り返し処理
        $(rows).each(function (i, row) {
            // チェックボックスを取得
            var checkBox = $(row).find("div[tabulator-field='SELTAG']")[0];

            // 画面タイプが「新規」以外の場合
            if (formType == PartsTransFlg.New) {

                // 先頭行のチェックボックス格納
                if (!firstRowCheckBox) {
                    firstRowCheckBox = $(checkBox).find("input")[0];
                }

                // チェック時のイベント付与
                $(checkBox).on('change', function () {

                    // チェック時イベント実行
                    checkBoxChangeedByLocationDepartmentList(row, checkBox);
                });
            }
            else {

                // チェックボックスを選択状態にする
                $(checkBox).find("input")[0].click();

                // チェックボックスを非活性にする
                changeInputControl($(checkBox).find("input")[0]);

                // 行の背景色を変更
                setHikiate(row, true);
            }
        });

        // 画面タイプが「新規」以外の場合
        if (formType != PartsTransFlg.New) {

            // 棚別部門別在庫一覧の全選択/全解除を非表示にする
            var allSelect = $(P_Article).find("#" + PT0006_FormList.Department.Id + getAddFormNo() + "_div").find("a[data-actionkbn='1241'],a[data-actionkbn='1242']");
            allSelect.hide();
        }
        else {
            // 画面タイプが「新規」の場合、先頭行を選択状態にする
            $(firstRowCheckBox).click();
        }
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
        else {
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
                if (workNo == inventryWorkNo) {
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

    // 取消ボタンが押下されている場合
    if (btn[0].name == BtnName.Cancel) {

        // 表示年度の値をグローバル変数に格納
        PT0006_postBackBtnProcessForPopup(PT0006_FormList.ConductId);
    }

    if (P_dicIndividual[GlobalKeys.TransParent] == FormList.No) {
        // 一覧画面からの遷移の場合、登録更新データを一覧画面に反映する（再検索を行わず、一覧データに反映）
        // 詳細画面からの遷移の場合は詳細画面の再検索後に反映する
        PT0006_setUpdateDataForList(conductId, false);
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
    // 入庫、出庫、移庫
    var listConductId = [PT0005_ConsuctId, PT0006_ConsuctId, PT0007_ConsuctId];
    if (listConductId.indexOf(backFrom) < 0) {
        return;
    }
    if (conductId != ConductId_PT0001 || formNo != PT0001_FormList.No) {
        // 予備品一覧画面以外に戻る場合
        // 共通画面を閉じた場合、指定した画面ならば再検索を行う
        InitFormDataByCommonModal(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom);
    }
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
 * 在庫一覧検索後処理
 * @param {any} tbl 在庫一覧情報
 */
function GoodsIssueReservation(tbl) {
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
                else {
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

    // 在庫一覧の行数を取得し、1件でもある場合は行を非表示
    if ($(P_listData["#" + PT0006_FormList.ResultsInventory.Id + getAddFormNo()].element).find(".tabulator-row").length) {
        // 在庫一覧非表示
        var ResultsInventoryList = $(P_Article).find("#" + PT0006_FormList.ResultsInventory.Id + getAddFormNo()).find(".tabulator-tableholder");
        ResultsInventoryList.hide();
    }

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


/**
 *【オーバーライド用関数】ページデータ取得後
 * @param {any} appPath   : ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ 
 * @param {any} btn       : クリックされたボタン要素
 * @param {any} conductId : 機能ID
 * @param {any} pgmId     : プログラムID
 * @param {any} formNo    : 画面番号

 * @param {any} listData  : バックエンド側に渡すデータ(何もしない場合はそのまま返す)
 */
function PT0006_postGetPageData(appPath, btn, conductId, pgmId, formNo,) {

    // 出庫引当後にエラーになっているかどうか
    var isError = $(P_Article).parent().parent().find('.message_div').find('.error');

    // エラーになっている場合
    if (isError.length > 0) {
        // 画面タイプ取得
        var formType = getValue(PT0006_FormList.InputArea.Id, PT0006_FormList.InputArea.FormType, PT0006_FormList.InputArea.FirstRowNo, CtrlFlag.Label, false, false);

        // 新規の場合
        if (formType == PartsTransFlg.New) {
            // 取消ボタン非活性
            var cancelBtn = getButtonCtrl(BtnName.Cancel);
            changeInputControl(cancelBtn, false);

            // 登録ボタン非活性
            var registBtn = getButtonCtrl(BtnName.Regist);
            changeInputControl(registBtn, false);
        }
    }
}

/*
 * 出庫数 丸め処理
 * */
function PT0006roundQuantity() {
    var quantityVal = getValue(PT0006_FormList.InputArea.Id, PT0006_FormList.InputArea.IssueQuantity, 1, CtrlFlag.TextBox, false, false).replace(/[^-0-9.]/g, '');
    if (!isNaN(quantityVal) && quantityVal != '') {
        // 小数点以下桁数を取得
        var unitDidit = getValue(PT0006_FormList.InputArea.Id, PT0006_FormList.InputArea.Digit, 1, CtrlFlag.Label, false, false);

        // 丸め処理区分を取得
        var roundDivision = getValue(PT0006_FormList.InputArea.Id, PT0006_FormList.InputArea.RoundDivision, 1, CtrlFlag.Label, false, false);

        // 出庫数の丸め処理
        var quantityDisp = roundDigit(quantityVal, unitDidit, roundDivision);
        // 画面項目に設定する
        setValue(PT0006_FormList.InputArea.Id, PT0006_FormList.InputArea.IssueQuantity, 1, CtrlFlag.TextBox, quantityDisp, false, false);
    }
    else {
        // 空にする
        setValue(PT0006_FormList.InputArea.Id, PT0006_FormList.InputArea.IssueQuantity, 1, CtrlFlag.TextBox, '', false, false);
    }
}


/**
 * 棚別部門別在庫一覧 チェックボックスのチェック時イベント
 * @param {any} row 選択された行
 */
function checkBoxChangeedByLocationDepartmentList(row, checkBox) {

    // チェックボックスがチェックされた場合は行を指定された背景色に変更
    setHikiate(row, $(checkBox).find("input")[0].checked);

    // 在庫一覧非表示、登録ボタン非活性
    hideBtnAndList();
}


/**
 * 【オーバーライド用関数】全選択および全解除ボタンの押下後
 * @param  formNo  : 画面番号
 * @param  tableId : 一覧のコントロールID
 */
function PT0006_afterAllSelectCancelBtn(formNo, tableId) {

    // 機能IDが「出庫入力」ではない場合は何もしない
    if (getConductId() != PT0006_FormList.ConductId) {
        return;
    }

    if (tableId == PT0006_FormList.Department.Id) {
        // 棚別部門別在庫一覧を取得
        var tbl = P_listData["#" + PT0006_FormList.Department.Id + getAddFormNo()];

        // 取得した一覧の行に対して繰り返し処理
        $(tbl.element).find(".tabulator-row").each(function (i, row) {
            // チェックボックスを取得
            var checkBox = $(row).find("div[tabulator-field='SELTAG']")[0];

            // チェック時イベント実行
            checkBoxChangeedByLocationDepartmentList(row, checkBox);

        });
    }
}


/**
 *【オーバーライド用関数】
 *  検索処理前(一覧の選択チェックボックスが選択されているかチェック)
 *
 *  @appPath {string} 　：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @btn {string} 　　　：対象ボタン
 *  @conductId {string} ：機能ID
 *  @pgmId {string} 　　：プログラムID
 *  @formNo {number} 　 ：画面番号
 *  @conductPtn {number}：処理ﾊﾟﾀｰﾝ
 */
function PT0006_checkSelectedRowBeforeSearchBtnProcess(appPath, btn, conductId, pgmId, formNo, conductPtn) {

    // 棚別部門別在庫一覧にチェックされた行が存在しない場合、処理をキャンセル
    if (!isCheckedList(PT0006_FormList.Department.Id)) {

        P_ProcExecuting = false;
        // ボタンを活性化
        $(btn).prop("disabled", false);

        return false;
    }

    // バックエンド側に渡す条件を作成
    // 棚別部門別在庫一覧のデータを取得
    P_dicIndividual['deaprtment'] = P_listData["#" + PT0006_FormList.Department.Id + getAddFormNo()].getData();

    return true;
}


/**
 *【オーバーライド用関数】登録処理前の「listData」個別取得処理
 * @param {any} appPath   : ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 * @param {any} conductId : 機能ID
 * @param {any} pgmId     : プログラムID
 * @param {any} formNo    : 画面番号
 * @param {any} btn       : クリックされたボタン要素
 * @param {any} listData  : バックエンド側に渡すデータ(何もしない場合はそのまま返す)
 */
function PT0006_getListDataForRegist(appPath, conductId, pgmId, formNo, btn, listData) {

    // バックエンド側に渡す条件を作成
    // 棚別部門別在庫一覧のデータを取得
    P_dicIndividual['deaprtment'] = P_listData["#" + PT0006_FormList.Department.Id + getAddFormNo()].getData();

    // 何もしていないのでそのまま返す
    return listData;
}

/**
*【オーバーライド用関数】
*  閉じる処理の後(ポップアップ画面用)
*/
function PT0006_postBackBtnProcessForPopup(conductId) {

    // 出庫入力画面の機能IDでない場合は何もせずに終了
    if (conductId != PT0006_FormList.ConductId) {
        return;
    }

    var val = null;

    // 表示年度(From)がグローバル変数に格納されている場合は一度削除する
    if (P_dicIndividual[DispYearKeyName.YearFrom]) {
        delete P_dicIndividual[DispYearKeyName.YearFrom];
    }

    // 表示年度(From)の値を取得
    val = getValue(PT0006_FormList.Spare, PT0006_FormList.DispYearFrom, 0, CtrlFlag.Label, false, false).trim();

    if (!val) {
        // 入力されていない場合はSQLで扱うことのできる年の最小値を設定
        val = SqlYear.MinYear;
    }

    // グローバル変数に格納
    P_dicIndividual[DispYearKeyName.YearFrom] = val;

    val = null;

    // 表示年度(To)がグローバル変数に格納されている場合は一度削除する
    if (P_dicIndividual[DispYearKeyName.YearTo]) {
        delete P_dicIndividual[DispYearKeyName.YearTo];
    }

    // 表示年度(To)の値を取得
    val = getValue(PT0006_FormList.Spare, PT0006_FormList.DispYearTo, 0, CtrlFlag.Label, false, false).trim();

    if (!val) {
        // 入力されていない場合はSQLで扱うことのできる年の最大値を設定
        val = SqlYear.MaxYear;
    }

    // グローバル変数に格納
    P_dicIndividual[DispYearKeyName.YearTo] = val;
}

/**
* 更新データを一覧画面に反映する
*  @param conductId   ：機能ID
*  @param isDelete    ：削除の場合true
*/
function PT0006_setUpdateDataForList(conductId, isDelete) {
    if (!P_dicIndividual[GlobalKeys.UpdateListData] || conductId != PT0006_FormList.ConductId) {
        //更新データが存在しない場合(添付情報の反映は別のタイミングで行う)
        return;
    }

    //反映するデータ
    var updateData = P_dicIndividual[GlobalKeys.UpdateListData];
    if (!updateData || updateData.length < 1) {
        //処理終了
        return;
    }
    //1行目：ステータス（新規、更新、削除）
    var status = updateData[0].STATUS;
    //2行目：一覧画面用の反映データ
    var data = updateData[1];

    if (isDelete && status != rowStatusDef.Delete) {
        //postRegistProcessから呼ばれた場合は削除処理だけ行う
        //postBuiltTabulatorから呼ばれた場合は登録・更新処理を行う
        return;
    }

    //一覧画面のデータ
    var table = P_listData["#" + FormList.Id + "_" + FormList.No];
    if (!table) {
        // 一覧画面を生成していない場合(別タブ遷移で一覧画面を経由していない場合)、処理終了
        return;
    }

    switch (status) {
        case rowStatusDef.Edit: //更新
            //データの予備品IDを取得
            var partsId = data["VAL" + FormList.PartsId];
            //更新前のROWNOを取得（ROWNOがキー）
            var oldData = table.searchData("VAL" + FormList.PartsId, "=", partsId);
            if (oldData && oldData.length > 0) {
                data.ROWNO = oldData[0].ROWNO;
                //詳細画面から値取得
                PT0006_setLabelValueToListData(data, status, oldData[0]);
                table.updateRow(data.ROWNO, data);
            }
            break;

        default:
            break;
    }

    // 予備品一覧の背景色変更
    postSearchList(table);

    delete P_dicIndividual[GlobalKeys.UpdateListData];
}

/**
* 詳細画面の値から一覧用データに値を反映する
* @param data 一覧用データ
* @param status ステータス(新規or更新)
*/
function PT0006_setLabelValueToListData(data, status, oldData) {
    var unitName;
    var stockQuantity;
    var stockQuantityVal;
    var inoutQuantity = data["VAL" + FormList.StockQuantity];
    var leadTime;
    var leadTimeVal;
    if (oldData) {
        // 一覧の選択行データが渡ってきた場合は一覧画面からの出庫登録
        // 最新在庫数には出庫数が入っていて単位文字列が付加されていないため、一覧の選択行から取得して付加する
        unitName = oldData["VAL" + FormList.UnitName];
        var oldStockQuantity = oldData["VAL" + FormList.StockQuantityExceptUnit];
        stockQuantityVal = parseInt(oldStockQuantity) - parseInt(inoutQuantity);
        stockQuantity = stockQuantityVal + unitName;
        leadTimeVal = parseInt(oldData["VAL" + FormList.LeadTimeExceptUnit]);
    } else {
        // 数量管理単位を取得
        unitName = getValue(FormDetail.PurchaseInfo.Id, FormDetail.PurchaseInfo.UnitName, 1, CtrlFlag.Label);
        // 最新在庫数の値を取得
        stockQuantity = getValue(FormDetail.PurchaseInfo.Id, FormDetail.PurchaseInfo.StockQuantity, 1, CtrlFlag.Label);
        stockQuantityVal = parseInt(stockQuantity.replace(unitName, ""));
        // 発注点
        leadTime = getValue(FormDetail.PurchaseInfo.Id, FormDetail.PurchaseInfo.LeadTime, 1, CtrlFlag.Label);
        leadTimeVal = parseInt(leadTime.replace(unitName, ""));
    }

    //一覧に表示している列に詳細画面の対応する項目値を設定
    $.each(Object.keys(data), function (index, key) {
        if (!key.startsWith("VAL")) {
            return true; // continue
        }

        switch (key) {
            case "VAL" + FormList.StockQuantity: // 最新在庫数(単位有り)
                data[key] = stockQuantity;
                break;
            case "VAL" + FormList.StockQuantityExceptUnit: // 最新在庫数(単位無し)
                data[key] = stockQuantityVal;
                break;
            default:
                // 最新在庫数以外は更新しない
                delete data[key];
                break;
        }
    });

    // 発注アラームを設定
    data["VAL" + FormList.OrderAlert.CtrlNo] = isOrderAlertOn(stockQuantityVal, leadTimeVal);
}
