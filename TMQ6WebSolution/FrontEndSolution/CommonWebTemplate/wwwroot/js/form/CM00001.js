/* ========================================================================
 *  機能名　    ：   【CM00001】トップ画面
 * ======================================================================== */

/**
 * 自身の相対パスを取得
 */
function getPath() {
    var root;
    var scripts = document.getElementsByTagName("script");
    var i = scripts.length;
    while (i--) {
        var match = scripts[i].src.match(/(^|.*\/)CM00001\.js$/);
        if (match) {
            root = match[1];
            break;
        }
    }
    return root;
}

document.write("<script src=\"" + getPath() + "/tmqcommon.js\"></script>");

// 件数がこの値の場合、非表示にする
const CountValueNoDisplay = -1;
// 変更管理画面へ渡すグローバル変数(申請者なら1、承認者なら2)
const CM00001_GlobalKeyHistory = "CM00001_HistoryParam";

// トップ画面の定義
const FormTop = {
    No: 0
    // 申請件数
    , Application: { Id: "ApplicationHistory", McMaking: 1, McReturn: 2, McRequest: 3, LnMaking: 4, LnReturn: 5, LnRequest: 6 }
    // 承認件数
    , Approval: { Id: "ApprovalHistory", Machine: 1, Longplan: 2 }
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
 *  @data {List<Dictionary<string, object>>}    ：初期表示ﾃﾞｰﾀ
 */
function initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data) {
    setFormatCount(true); // 申請件数の値に応じて非表示
    setFormatCount(false);// 承認件数の値に応じて非表示
}

/**
 * 件数の値に応じ、表示ランを非表示にする処理
 * @param {any} isApplication 申請件数の場合True、承認件数の場合False
 */
function setFormatCount(isApplication) {
    // 画面から取得する項目(申請件数or承認件数)
    var targetId = isApplication ? FormTop.Application.Id : FormTop.Approval.Id;
    var targetVals = isApplication
        ? [FormTop.Application.McMaking, FormTop.Application.McReturn, FormTop.Application.McRequest, FormTop.Application.LnMaking, FormTop.Application.LnReturn, FormTop.Application.LnRequest]
        : [FormTop.Approval.Machine, FormTop.Approval.Longplan];
    // 表示するか判定
    var isDisplay = true;
    $.each(targetVals, function (index, value) {
        // 画面の表示件数を繰り返し取得し、非表示の値といずれかが一致するのなら、非表示とする
        var count = getValue(targetId, value, 0, CtrlFlag.Link, false, false);
        if (count == CountValueNoDisplay) {
            isDisplay = false;
            return true;
        }
        setValue(targetId, value, 0, CtrlFlag.Link, setNumberToComma(count), false, false);
    });
    // 非表示に変更
    changeListDisplay(targetId, isDisplay, false);
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

    var paramHistory = 0;
    // グローバル変数にトップ画面からの遷移フラグを設定し、変更管理画面側で検索条件の制御を行う
    if (ctrlId == FormTop.Application.Id) {
        // 申請件数で遷移する場合
        paramHistory = 1;
    }
    if (ctrlId == FormTop.Approval.Id) {
        // 承認件数で遷移する場合
        paramHistory = 2;
    }
    if (paramHistory > 0) {
        // 申請or承認件数で遷移する場合、検索条件に値をセットし、変更管理画面で呼出後、削除
        // グローバル変数は別機能遷移で使用できず、検索条件を上書きする場合は既定の処理と判別できないため
        var conditionData = {};
        conditionData[CM00001_GlobalKeyHistory] = paramHistory;
        conditionDataList.push(conditionData);
    }
    return [true, conditionDataList];
}