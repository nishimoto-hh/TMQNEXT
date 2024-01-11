/* ========================================================================
 *  機能名　    ：   【入出庫一覧】
 * ======================================================================== */
/**
 * 自身の相対パスを取得
 */
function getPath() {
    var root;
    var scripts = document.getElementsByTagName("script");
    var i = scripts.length;
    while (i--) {
        var match = scripts[i].src.match(/(^|.*\/)PT0002\.js$/);
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
document.write("<script src=\"" + getPath() + "/PT0005.js\"></script>");　// 入庫入力
document.write("<script src=\"" + getPath() + "/PT0006.js\"></script>");　// 出庫入力
document.write("<script src=\"" + getPath() + "/PT0007.js\"></script>");　// 移庫入力

// 一覧画面 コントロール項目番号
const PT0002_FormList = {
    ConductId: "PT0001",                        // 機能ID
    No: 0,                                      // 画面番号
    ResultsEnter: "BODY_030_00_LST_0",          // 入庫検索結果一覧
    ResultsIssueParents: "BODY_050_00_LST_0",   // 出庫検索結果一覧
    ResultsIssueChild: "BODY_100_00_LST_0",     // 出庫検索結果一覧(入れ子)
    ResultsShed: "BODY_070_00_LST_0",           // 棚番移庫検索結果一覧
    ResultsCategory: "BODY_090_00_LST_0",       // 部門移庫検索結果一覧
    ButtonOutputIdEnter: "OutputEnter",         // 出力ボタン(入庫)
    ButtonOutputLabelEnter: "OutputLabelEnter", // ラベル出力ボタン(入庫)
    ButtonOutputIdIssue: "OutputIssue",         // 出力ボタン(出庫)
    ButtonOutputIdShed: "OutputShed",           // 出力ボタン(棚番移庫)
    ButtonOutputLabelShed: "OutputLabelShed",   // ラベル出力ボタン(棚番移庫)
    ButtonOutputIdCategory: "OutputCategory",   // 出力ボタン(部門移庫)
    ButtonOutputLabelCategory: "OutputLabelCategory", // ラベル出力ボタン(部門移庫)
    ButtonOutputPurchaseDetails: "OutputPurchaseDetails", // 購入明細書ボタン(出庫)
    ParentKeyValueParent: 11,                    // 出庫一覧親キー値
    ParentKeyValueChild: 13,                    // 出庫一覧子キー値
    WorkingDay: "COND_000_00_LST_0",            // 作業日
    WorkingDayKeyValue: 1                       // 作業日キー値
};

// 一覧画面 コントロール項目番号
const PT0007_FormCtrlList = {
    ResultsShed: "CBODY_010_00_LST_7",          // 棚番移庫
    ResultsCategory: "CBODY_040_00_LST_7",      // 部門移庫
};

// グローバルリストのキー名称
const PT0002_GlobalListKey = {
    ConditionTitle1: "PT0002_ConditionTitle1",　　　　　　 // 作業日（タイトル）
    ConditionValue1: "PT0002_ConditionValue1",　　　　　　 // 作業日（値）
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
 *  @data {List<COM_TMPTBL_DATA>}    ：初期表示ﾃﾞｰﾀ
 */
function initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data) {

    // 共通-棚番移庫入力画面
    PT0007_initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data);

    // 一覧の表示状態を切り替え(初期表示時は子要素非表示)
    changeListDisplay(PT0002_FormList.ResultsIssueChild, false, false);

    // 共通-入庫入力画面
    PT0005_initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data);

    // 共通-出庫入力画面
    PT0006_initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data);
}

/**
 *【オーバーライド用関数】
 *  画面状態設定後の個別実装
 *
 * @status {number}       ：ﾍﾟｰｼﾞ状態　0：初期状態、1：検索後、2：明細表示後ｱｸｼｮﾝ、3：ｱｯﾌﾟﾛｰﾄﾞ後
 * @pageRowCount {number} ：ﾍﾟｰｼﾞ全体のﾃﾞｰﾀ行数
 * @conductPtn {byte}     ：com_conduct_mst.ptn
 * @authShori {IList<{ﾎﾞﾀﾝCTRLID：ｽﾃｰﾀｽ}>} ：ｽﾃｰﾀｽ：0:非表示、1(表示(活性))、2(表示(非活性))
 */
function setPageStatusEx(status, pageRowCount, conductPtn, authShori) {

    // 共通-入庫入力画面
    PT0005_setPageStatusEx(status, pageRowCount, conductPtn, authShori);

    // 共通-出庫入力画面
    PT0006_setPageStatusEx(status, pageRowCount, conductPtn, authShori);
}

/**
 *【オーバーライド用関数】Excel出力ﾁｪｯｸ処理 - 前処理
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 */
function reportCheckPre(appPath, conductId, formNo, btn) {
    if (btn.name == PT0002_FormList.ButtonOutputIdEnter ||
        btn.name == PT0002_FormList.ButtonOutputLabelEnter) {

        // 入庫一覧画面で「出力」「ラベル出力」ボタン押下時、一覧にチェックされた行が存在しない場合、遷移をキャンセル
        if (!isCheckedList(PT0002_FormList.ResultsEnter)) {
            return false;
        }
    }

    if (btn.name == PT0002_FormList.ButtonOutputIdIssue || btn.name == PT0002_FormList.ButtonOutputPurchaseDetails) {
        // 出庫一覧画面で「出力/購入明細書」ボタン押下時、一覧にチェックされた行が存在しない場合、遷移をキャンセル
        if (!isCheckedList(PT0002_FormList.ResultsIssueParents)) {
            return false;
        }
    }

    if (btn.name == PT0002_FormList.ButtonOutputIdShed || 
        btn.name == PT0002_FormList.ButtonOutputLabelShed) {
        // 棚番移庫一覧画面で「出力」ボタン押下時、一覧にチェックされた行が存在しない場合、遷移をキャンセル
        if (!isCheckedList(PT0002_FormList.ResultsShed)) {
            return false;
        }
    }

    if (btn.name == PT0002_FormList.ButtonOutputIdCategory ||
        btn.name == PT0002_FormList.ButtonOutputLabelCategory) {
        // 部門移庫一覧画面で「出力」ボタン押下時、一覧にチェックされた行が存在しない場合、遷移をキャンセル
        if (!isCheckedList(PT0002_FormList.ResultsCategory)) {
            return false;
        }
    }

    // 出力条件シート用データ設定
    var tdWorkingDay = $(P_Article).find("#" + PT0002_FormList.WorkingDay + getAddFormNo()).find("td[data-name='VAL" + PT0002_FormList.WorkingDayKeyValue + "']");
    var thWorkingDayTitle = $(P_Article).find("#" + PT0002_FormList.WorkingDay + getAddFormNo()).find("th[data-name='VAL" + PT0002_FormList.WorkingDayKeyValue + "']");
    var paramVal = getCellVal(tdWorkingDay).replace("|", " ～ ");
    var paramTitle = getCellVal(thWorkingDayTitle).trim();

    // 条件値の取得
    operatePdicIndividual(PT0002_GlobalListKey.ConditionTitle1, false, paramTitle);        // 見出し
    operatePdicIndividual(PT0002_GlobalListKey.ConditionValue1, false, paramVal);          // 出力値

    return true;
}

/**
 * 【オーバーライド用関数】Tabuator一覧のヘッダー設定前処理
 * @param {string} appPath          ：アプリケーションルートパス
 * @param {string} id               ：一覧のID(#,_FormNo付き)
 * @param {object} header           ：ヘッダー情報
 * @param {Element} headerElement   ：ヘッダー要素
 * @param {List<string>} subTableIdList ：入れ子一覧の場合、子一覧のID
 */
function prevSetTabulatorHeader(appPath, id, header, headerElement, subTableIdList) {
    if (id == "#" + PT0002_FormList.ResultsIssueParents + getAddFormNo()) {
        setNestedTable(id, header, PT0002_FormList.ResultsIssueParents, PT0002_FormList.ResultsIssueChild, subTableIdList);
    }

}

/**
 * 【オーバーライド用関数】Tabulatorの描画が完了時の処理
 * @param {any} tbl 一覧
 * @param {any} id 一覧のID(#,_FormNo付き)
 */
function postBuiltTabulator(tbl, id) {
    // Tabulatorの描画が完了後（renderComplete完了後）の処理
    convertTabulatorListToNestedTable(id, PT0002_FormList.ResultsIssueParents, PT0002_FormList.ResultsIssueChild, PT0002_FormList.ParentKeyValueParent, PT0002_FormList.ParentKeyValueChild)

    // 入出庫一覧 出庫入力画面
    PT0006_postBuiltTabulator(tbl, id);

    // 入出庫一覧 移庫入力画面
    PT0007_postBuiltTabulator(tbl, id);
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

    // 遷移元が入庫入力の場合
    if (ctrlId == PT0002_FormList.ResultsEnter) {
        PT0005_prevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element);
        return [true, conditionDataList];
    }

    // 遷移元が部門在庫情報(出庫入力)の場合
    if (ctrlId == PT0006_FormList.Department.Id) {
        PT0006_prevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element);
        return [false, conditionDataList];
    }

    // 遷移元が棚番移庫一覧、もしくは部門移庫一覧の場合
    if (ctrlId == PT0007_FormCtrlList.ResultsShed || ctrlId == PT0007_FormCtrlList.ResultsCategory) {
        PT0007_prevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element);
        return [false, conditionDataList];
    }

    return [true, conditionDataList];
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

    // 共通-入庫入力詳細画面の実行正常終了後処理
    PT0005_postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data);

    // 共通-出庫入力詳細画面の実行正常終了後処理
    PT0006_postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data);

    // 共通-移庫入力詳細画面の実行正常終了後処理
    PT0007_postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data);
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

    // 共通-出庫一覧の画面再表示前処理
    PT0006_beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom);
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
function afterSearchBtnProcess(appPath, btn, conductId, pgmId, formNo, conductPtn) {
    if (conductId != PT0006_FormList.ConductId) {
        return;
    }
    PT0006_afterSearchBtnProcess(appPath, btn, conductId, pgmId, formNo, conductPtn);
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

    // 共通-入庫入力画面のコンボボックス変更処理
    PT0005_setComboOtherValues(appPath, combo, datas, selected, formNo, ctrlId, valNo);

    // 移庫入力画面 コンボボックス変更時イベント
    PT0007_setComboOtherValues(appPath, combo, datas, selected, formNo, ctrlId, valNo);
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

    // 入庫入力画面 オートコンプリート設定時イベント
    PT0005_setCodeTransOtherNames(appPath, formNo, ctrl, data);

    // 移庫入力画面 オートコンプリート設定時イベント
    PT0007_setCodeTransOtherNames(appPath, formNo, ctrl, data);

}

/**
 *【オーバーライド用関数】実行ﾁｪｯｸ処理 - 前処理
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 */
function prevCommonValidCheck(appPath, conductId, formNo, btn) {

    // 機能IDを判定
    if (conductId == PT0005_ConsuctId) {

        // 入庫入力
        return PT0005_prevCommonValidCheck(appPath, conductId, formNo, btn);
    }
    else if (conductId == PT0007_ConsuctId) {

        // 移庫入力
        return PT0007_prevCommonValidCheck(appPath, conductId, formNo, btn);
    }

    // 上記に該当しない場合、共通の入力チェックを行う「true」、入力エラーなし「false」を返す
    return [true, false];

}

/**
 *【オーバーライド用関数】タブ切替時
 * @tabNo ：タブ番号
 * @tableId : 一覧のコントロールID
 */
function initTabOriginal(tabNo, tableId) {

    // 移庫入力画面 タブ切替時
    PT0007_initTabOriginal(tabNo, tableId);

}

/**
 *【オーバーライド用関数】登録前追加条件取得処理
 *  @param appPath       ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param conductId     ：機能ID
 *  @param formNo        ：画面番号
 *  @param btn           ：押下されたボタン要素
 */
function addSearchConditionDictionaryForRegist(appPath, conductId, formNo, btn) {

    // 共通-移庫入力画面の登録前追加条件取得処理を行うかどうか判定し、Trueの場合は行う
    if (IsExecPT0007_AddSearchConditionDictionaryForRegist(appPath, conductId, formNo, btn)) {
        return PT0007_addSearchConditionDictionaryForRegist(appPath, conductId, formNo, btn);
    }

    // それ以外の場合
    var conditionDataList = [];
    return conditionDataList;
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
function postGetPageData(appPath, btn, conductId, pgmId, formNo) {

    // 出庫入力画面
    PT0006_postGetPageData(appPath, btn, conductId, pgmId, formNo);
}

/**
 * 【オーバーライド用関数】全選択および全解除ボタンの押下後
 * @param  formNo  : 画面番号
 * @param  tableId : 一覧のコントロールID
 */
function afterAllSelectCancelBtn(formNo, tableId) {

    // 出庫入力画面
    PT0006_afterAllSelectCancelBtn(formNo, tableId);
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
function checkSelectedRowBeforeSearchBtnProcess(appPath, btn, conductId, pgmId, formNo, conductPtn) {

    if (conductId == PT0006_ConsuctId) {
        // 出庫入力画面
        return PT0006_checkSelectedRowBeforeSearchBtnProcess(appPath, btn, conductId, pgmId, formNo, conductPtn);
    }

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
function getListDataForRegist(appPath, conductId, pgmId, formNo, btn, listData) {

    if (conductId == PT0006_ConsuctId) {
        // 出庫入力画面
        PT0006_getListDataForRegist(appPath, conductId, pgmId, formNo, btn, listData);
    }

    // 何もしていないのでそのまま返す
    return listData;
}