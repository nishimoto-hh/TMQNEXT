/* ========================================================================
 *  機能名　    ：   オーバーライド関数のテンプレート
 * ======================================================================== */
//document.write("<script src=\"" + getPath() + "/tmqcommon.js\"></script>");

/*
 * このファイルには、Common.jsに定義されているイベントの処理を拡張したオーバーライト処理が記載されています。
 * Common.jsに関連した変更以外では、このファイルを変更しないでください。
 * このファイルを元に各機能のJavaScript処理を作成する場合、各機能では不要な処理は削除してください。
 * 実装以後に処理の仕様が変更された場合、そちらも変更対象となりますし、動作に影響があるおそれもあります。
 */

//// 一覧のVALの値をJavaScriptではよく使いますが、値をそのまま記載すると後でどの項目を見ているか分からなくなります。
//// また、VALの値を詰めるためなどで変更することが大変になりますので、まとめて定義してください。
//// 以下、定義の例です。画面ごとに定数を作成し、その中に機能で使用する一覧をそれぞれ定義します。
//// 一覧画面の定義
//const SampleFormList = {
//    // Noは画面Noを定義
//    No: 0,
//    // この画面の一覧の内容をそれぞれ定義
//    // 「SearchCondition」「List」は一覧の分かりやすい名前を付ける、変数名のように使用(後述)
//    // 「Id」の値は定義された一覧のコントロールグループID
//    // 「StartDate」、「EndDate」は項目の分かりやすい名前を付ける、変数名のように使用(後述)
//    // 「StartDate」、「EndDate」の値は定義された項目のコントロール番号
//    SearchCondition: { Id: "BODY_010_00_LST_0", StartDate: 1, EndDate: 2 },
//    List: { Id: "BODY_040_00_LST_0", Subject: 1, SubjectNote: 2 }
//};
//// FormNoを取得したい場合
//SampleFormList.No
//// SearchConditionのIDを取得したい場合
//SampleFormList.SearchCondition.Id
//// ListのSubjectNoteのvalNoを取得したい場合
//SampleFormList.List.SubjectNote

//// 以下のように改行せずに1行でも書けます
//// 参照画面の定義
//const SampleFormDetail = { No: 1, HeaderLocation: { Id: "BODY_010_00_LST_1" }, HeaderJob: { Id: "BODY_020_00_LST_1" }, Hidden: { Id: "BODY_100_00_LST_1", LongPlanId: 1 } };

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

}

/**
 * 【オーバーライド用関数】
 * 　選択ボタン初期化処理
 *
 *  @appPath {string}   　　　　     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @formNo {byte} 　　　　          ：呼出元画面番号
 *  @btn {要素} 　　　　             ：選択ボタン要素
 *  @ctrlid {string} 　　　　        ：呼出元コントロールID
 */
function initSelectBtnOriginal(appPath, formNo, btn, ctrlid) {

    //// ﾎﾟｯﾌﾟｱｯﾌﾟ画面の先頭テーブルのVAL1を取得して、呼び出し元のコードにセット
    //var formDetail = $(P_Article).find("#formDetail");
    //var tblDetails = $(formDetail).find(".ctrlId");

    //var selectVal = "";
    //var td = $(tblDetails[0]).find("td[data-name^='VAL1']");
    //if (td != null && td.length > 0) {
    //    var input = $(td).find("input[type='text'], input[type='hidden'], textarea");
    //    if (input.length > 0) {
    //        selectVal = $(input).val();
    //    }
    //    else {
    //        //日付(ブラウザ標準)、時刻(ブラウザ標準)、日時(ブラウザ標準)
    //        var dateTime = $(td).find("input[type='date'], input[type='time'], input[type='datetime-local']");
    //        if (dateTime.length > 0) {
    //            selectVal = $(dateTime).val().replace(/-/g, "/").replace("T", " ");
    //        }
    //        else {
    //            //ﾗﾍﾞﾙ
    //            selectVal = $(td).text();
    //        }
    //    }
    //}
    //$("#" + ctrlid).val(selectVal);

}

/**
 * 【オーバーライド用関数】
 * 　計算実行処理
 *
 *  @appPath {string}   　　　　     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @articleForm {article要素}       ：表示画面ｴﾘｱ要素
 *  @targetTr {要素}   　　          ：計算対象tr要素
 *  @calcColNo {byte} 　　　　       ：計算ラベルColNo
 *  @ctrl {要素}                     ：Changeイベント発行元
 */
function execCalcOriginal(appPath, articleForm, targetTr, calcColNo, ctrl) {
    //var resultVal = 0;
    //switch (calcColNo) {
    //    case 11:
    //        //計算式：val6 * val9
    //        var val6 = 0; //数量
    //        var val9 = 0; //標準単価
    //        var td_val6 = $(targetTr).find("td[data-name^='VAL6']");
    //        var td_val9 = $(targetTr).find("td[data-name^='VAL9']");
    //        if (td_val6 != null) {
    //            val6 = parseInt(getCellVal(td_val6));
    //        }
    //        if (td_val9 != null ) {
    //            val9 = parseFloat(getCellVal(td_val9).replace(",", ""));
    //        }
    //        resultVal = val6 * val9;
    //        break;

    //    default:
    //        break;
    //}

    ////計算結果をセット
    //var calcLabelTd = $(targetTr).find("td[data-name^='VAL" + calcColNo + "']");
    //$(calcLabelTd).text(resultVal);
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
    //if (data != null && data.length > 0) {
    //    var shozoku = data[0].VALUE2;
    //    $("#L_S_ConditionVAL8").val(shozoku);
    //}
    //else {
    //    $("#L_S_ConditionVAL8").val("");
    //}
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
    //// 変更されたコンボボックスの種類を、formNo,ctrlId,valNoで特定
    //if (formNo == FormRegist.No && ctrlId == FormRegist.WorkInfo.Id && valNo == FormRegist.WorkInfo.MQ) {
    //    // 選択された要素で、設定したい拡張項目の値を取得(空白選択時は空白)
    //    var setValue = selected == null ? '' : selected.EXPARAM1;
    //    // 設定したい項目に値を設定
    //    setValueAndTriggerNoChange(FormRegist.WorkInfo.Id, FormRegist.WorkInfo.Yosan, 0, CtrlFlag.Combo, setValue);
    //}
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

    // 画面遷移時のパラメータとして個別に取得したい場合
    //// 遷移元画面が詳細画面で修正/複製画面への遷移の場合
    //if (formNo == 1 && (transDiv == transDivDef.Edit || transDiv == transDivDef.Copy)) {
    //    // ①一覧からまとめてデータを取得する
    //    // 対象の一覧のコントロールグループIDをctrlIdListへ設定する
    //    const ctrlIdList = ['List1', 'List2'];
    //    conditionDataList = getListDataByCtrlIdList(ctrlIdList, formNo, 0)

    //    // ②直接値を取得して条件データを生成する
    //    var conditionData = {};
    //    conditionData['CTRLID'] = 'InitCondition';    // コントロールIDは任意の値でOK
    //    conditionData['VAL1'] = 'ConditionData1';	// 実際には画面から値を取得して設定する
    //    conditionData['VAL2'] = 'ConditionData2';
    //    conditionDataList.push(conditionData);
    //}

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

}


/**
 *【オーバーライド用関数】
 *  戻る処理の前(単票、子画面共用)
 *
 *  @appPath        {string}    ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @btnCtrlId      {byte}      ：ボタンのCtrlId
 *  @codeTransFlg   {int}       ：1:コード＋翻訳 選ボタンから画面遷移/1以外:それ以外
 *  @return {bool} データ取得する場合はtrue、スキップする場合はfalse（子画面のみ）
 */
function prevBackBtnProcess(appPath, btnCtrlId, status, codeTransFlg) {

    if (codeTransFlg != 1) {
        codeTransFlg = 0;
    }

    return true;
}

/**
 *【オーバーライド用関数】
 *  戻る処理の前(子画面用)
 *
 *  @appPath        {string}    ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @btnCtrlId      {byte}      ：ボタンのCtrlId
 *  @status         {any}       ：遷移ステータス
 *  @parentModalNo  {int}       ：親画面のモーダル番号、1以上ならモーダル
 *  @return {int} 親画面がモーダルなら1、そうでなければ0
 */
function prevBackBtnProcessForChild(appPath, btnCtrlId, status, parentModalNo) {

    return parentModalNo;
}

/**
 *【オーバーライド用関数】
 *  戻る処理の前(単票一覧直下表示用)
 *  @param {string}     appPath ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {element}    backBtn ：ボタン要素
 */
function prevBackBtnProcessForEditUnderList(appPath, backBtn) {

}


/**
 *【オーバーライド用関数】
 *  閉じる処理の後(ポップアップ画面用)
 */
function postBackBtnProcessForPopup() {

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
//function prevAddNewRow(appPath, element, transPtn, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, confirmFlg) {
function prevAddNewRow(appPath, element, isCopyRow, transPtn, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, confirmFlg) {
    return true;    // 個別実装で以後の処理の実行可否を制御 true：続行、false：中断
}

/**
 * 【オーバーライド用関数】行追加後
 *  @param {string}     appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {html}       element     ：ｲﾍﾞﾝﾄ発生要素
 *  @param {boolean}    isCopyRow   ：行コピーフラグ(true：行コピー、false：行追加)
 */
//function postAddNewRow(appPath, element) {
function postAddNewRow(appPath, element, isCopyRow) {

}

/**
 *【オーバーライド用関数】
 *  行削除の前
 *
 *  @appPath    {string}    ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @btn        {<a>}       ：行削除ﾘﾝｸのa要素
 *  @id         {string}    ：行削除ﾘﾝｸの対象一覧のCTRLID
 *  @checkes    {要素}      ：削除対象の要素リスト(tabulator一覧の場合、削除対象のデータリスト)
 */
function preDeleteRow(appPath, btn, id, checkes) {
    return true;   // 個別実装で以後の処理の実行可否を制御 true：続行、false：中断
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
function beforeSearchBtnProcess(appPath, btn, conductId, pgmId, formNo, conductPtn) {

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

}

/**
 *【オーバーライド用関数】
 *  共通機能用選択ボタン押下時処理
 *  @param {string}     appPath     :ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string}     cmConductId :共通機能ID
 *  @param {article}    cmArticle   :共通機能articleタグ
 *  @param {button}     btn         :選択ボタン要素
 *  @param {string}     fromCtrlId  :共通機能遷移時に押下したﾎﾞﾀﾝ/一覧のｺﾝﾄﾛｰﾙID
 *  @param {string}     fromConductId  :共通機能遷移元の機能ID
 */
function clickComConductSelectBtn(appPath, cmConductId, cmArticle, btn, fromCtrlId, fromConductId) {
    //※共通機能から親画面に返す値があるときはここで個別実装用パブリック変数に退避させて下さい。
    //※親画面に実際に反映させるのはbeforeCallInitFormDataで行う想定です。
}

/*==1:実行処理==*/

/**
 *【オーバーライド用関数】バリデーション前処理
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 */
function validDataPre(appPath, conductId, formNo, btn) {

}

/**
 *【オーバーライド用関数】実行ﾁｪｯｸ処理 - 前処理
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 */
function registCheckPre(appPath, conductId, formNo, btn) {
    return true;
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
    // 独自の確認メッセージを表示するために使用、関連情報パラメータは0に設定してください。
    return true;
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
function postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data) {

}

/*==5:出力処理==*/

/*==9:削除処理==*/
/**
 * 【オーバーライド用関数】削除ボタン前処理
 *  @param {string} appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string} conductId   ：機能ID
 *  @param {string} pgmId       ：プログラムID
 *  @param {number} formNo      ：画面番号
 *  @param {byte}   conductPtn  ：画面ﾊﾟﾀｰﾝ
 *  @param {element}btn         ：ﾎﾞﾀﾝ要素
 *  @param {boolean}isEdit      ：単票ｴﾘｱﾌﾗｸﾞ
 *  @param {int}    confirmNo   ：確認番号(ﾃﾞﾌｫﾙﾄ：0)
 *  @return {bool} 処理続行フラグ Trueなら続行、Falseなら処理終了
 */
function preDeleteProcess(appPath, conductId, pgmId, formNo, conductPtn, btn, isEdit, confirmNo) {
    // 独自の確認メッセージを表示するために使用、関連情報パラメータは0に設定してください。
    return true;
}

/*==8:取込処理==*/

/**
 *【オーバーライド用関数】
 * キャンセル押下後
 */
function clickPopupCancelBtn() {

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

}

/**
 *【オーバーライド用関数】子画面新規遷移前ﾁｪｯｸ処置
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 */
function prevTransChildForm(appPath, conductId, formNo, btn) {
    return true;
}

/**
 *【オーバーライド用関数】他機能遷移時パラメータを個別実装で作成
 *  @param {string} appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string} transTarget ：遷移先情報（機能ID|画面NO）
 *  @param {number} formNo      ：遷移元画面NO
 *  @param {string} ctrlId      ：遷移元の一覧ctrlid
 *  @param {string} btn_ctrlId  ：ボタンのbtn_ctrlid
 *  @param {int}    rowNo       ：遷移元の一覧の選択行番号（新規の場合は-1）
 *  @param {html}   element     ：ｲﾍﾞﾝﾄ発生要素
 *
 *  @return {List<Dictionary<string, object>>}  conditionDataList
 */
function createParamTransOtherIndividual(appPath, transTarget, formNo, ctrlId, btn_ctrlId, rowNo, element) {

    var conditionDataList = [];
    //var conditionData = {};
    ////遷移先の機能＆画面NOの初期検索に必要な情報のみ作成すればよいです（例は納期回答→受注Form1）
    //conditionData['CTRLID'] = 'Result';
    //conditionData['VAL1'] = 'JU00010061';	//実際には画面から値を取得してｾｯﾄ
    //conditionDataList.push(conditionData);

    return conditionDataList;
}

/**
 *【オーバーライド用関数】他機能遷移先の戻る時再検索用退避条件(P_SearchCondition)を作成
 *  @param {string} appPath         ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string} transTarget     ：遷移先情報（機能ID|画面NO|親画面NO）
 *  @param {string} transConductId  ：遷移先の機能ID
 *  @param {number} formNo          ：遷移元画面NO
 *  @param {string} ctrlId          ：遷移元の一覧ctrlid
 *  @param {string} btn_ctrlId      ：ボタンのbtn_ctrlid
 *  @param {int}    rowNo           ：遷移元の一覧の選択行番号（新規の場合は-1）
 *  @param {html}   element         ：ｲﾍﾞﾝﾄ発生要素
 *
 *  @return {Dictionary<string, object>}  dicBackConditions
 */
function createParamTransOtherBack(appPath, transTarget, transConductId, formNo, ctrlId, btn_ctrlId, rowNo, element) {

    var dicBackConditions = {};
    //通常ルートで取得する検索条件を作成
    //var conList = [];
    //var conData = {};
    ////例)ROR0010
    //conData.CTRLID = "Condition";
    //conData.FORMNO = 0;
    //conData.ROWNO = 1;
    //conData.VAL1 = "JU00010061";
    //conData.VAL2 = "";
    //conData.VAL3 = "";
    //conData.VAL4 = "";
    //conData.VAL5 = "";
    //conData.VAL6 = "";
    //conData.VAL7 = "";
    //conData.VAL8 = "";
    //conData.VAL9 = "";
    //conData.VAL10 = "";
    //conData.VAL11 = "";
    //conData.VAL12 = "";
    //conData.VAL13 = "";
    //conData.VAL14 = "";
    //conData.VAL15 = "|";
    //conData.VAL16 = "|";
    //conData.VAL17 = "|";
    //conData.VAL18 = "|";
    //conData.VAL19 = "";
    //conData.VAL20 = "";
    //conData.VAL21 = "";
    //conData.lockData = "";
    //conList.push(conData);
    //dicBackConditions['0'] = JSON.stringify(conList);   //JSON変換してｾｯﾄして下さい。

    return dicBackConditions;
}

/**
 *【オーバーライド用関数】共通機能ポップアップ画面遷移前ﾁｪｯｸ処置
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：遷移先情報
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 */
function prevTransCmForm(appPath, transTarget, formNo, btn) {
    return true;
}

/**
 *【オーバーライド用関数】Excel出力ﾁｪｯｸ処理 - 前処理
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 */
function reportCheckPre(appPath, conductId, formNo, btn) {
    return true;
}

/**
 *【オーバーライド用関数】登録前追加条件取得処理
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 *  
 *  @return {Dictionary<string, object>}  追加する条件リスト
 */
function addSearchConditionDictionaryForRegist(appPath, conductId, formNo, btn) {
    var conditionDataList = [];
    return conditionDataList;
}

/**
 *【オーバーライド用関数】取込処理個別入力チェック
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  
 *  @return {bool}:個別入力チェック後も既存の入力チェックを行う場合はtrue
 *  @return {bool}:個別の入力チェックでエラーの場合はtrue
 */
function preInputCheckUpload(appPath, conductId, formNo) {
    var isContinue = true;
    var isError = false;
    var isAutoBackFlg = true;

    return [isContinue, isError, isAutoBackFlg];
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
    // Tabuator一覧に個別で追加したい列等の設定が行えます

    // スケジュール表示列の追加をする場合
    //if (id == "#" + FormList.List.Id + getAddFormNo()) {
    //    // スケジュール表示用ヘッダー情報の設定
    //    setScheduleHeaderInfo(appPath, header, headerElement);
    //}

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
    // Tabuator一覧に個別で設定したいオプション等の設定が行えます
}

/**
 * 【オーバーライド用関数】Tabulatorの描画が完了時の処理
 * @param {any} tbl 一覧
 * @param {any} id 一覧のID(#,_FormNo付き)
 */
function postBuiltTabulator(tbl, id) {
    // Tabulatorの描画が完了後（renderComplete完了後）の処理

}

/**
 * 【オーバーライド用関数】Tabulatorのページ変更後の処理
 * @param {any} tbl 処理対象の一覧
 * @param {any} id 処理対象の一覧のID(#,_FormNo付き)
 * @param {any} pageNo 表示するページNo
 * @param {any} pagesize 表示中のページの件数
 */
function postTabulatorChangePage(tbl, id, pageNo, pagesize) {

}

/**
 * 【オーバーライド用関数】Tabulatorの列フィルター後の処理
 * @param {any} tbl 処理対象の一覧
 * @param {any} id 処理対象の一覧のID(#,_FormNo付き)
 * @param {any} filters フィルターの条件
 * @param {any} rows フィルター後の行rows
 */
function postTabulatorDataFiltered(tbl, id, filters, rows) {

}

/**
 * 【オーバーライド用関数】Tabulatorのレンダリング完了後の処理
 * @param {any} tbl 処理対象の一覧
 * @param {any} id 処理対象の一覧のID(#,_FormNo付き)
 */
function postTabulatorRenderCompleted(tbl, id) {

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

}

/**
 *【オーバーライド用関数】タブ切替時
 * @tabNo ：タブ番号
 * @tableId : 一覧のコントロールID
 */
function initTabOriginal(tabNo, tableId) {

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

    return [conditionDataList, listDefines];
}

/**
 * 【オーバーライド関数】ツリー展開時追加処理
 * @param {string} listId 一覧のID(#,_FormNo付き)
 * @param {any} row 展開対象の行
 * @param {any} level 展開ツリーのレベル
 */
function afterDataTreeRowExpanded(listId, row, level) {

}

/**
 * 【オーバーライド用関数】全選択および全解除ボタンの押下後
 * @param  formNo  : 画面番号
 * @param  tableId : 一覧のコントロールID
 */
function afterAllSelectCancelBtn(formNo, tableId) {

}

/**
 * 【オーバーライド用関数】項目カスタマイズ一覧のチェックボックスのチェック変更後処理
 * @param {any} tableId 一覧のID(#,_FormNo付き)
 * @param {any} targetId チェック変更された列のVAL(VAL1など)
 * @param {any} isChecked チェックが付いた場合True、外れた場合False
 */
function afterChangeItemCustomizeCheckBox(tableId, targetId, isChecked) {

}

/**
 * 【オーバーライド用関数】一覧表示切替後処理
 * @param {any} id 一覧表示切替ボタンの要素
 * @param {any} isHide 表示→非表示になった場合はtrue
 */
function postSwitchTable(id, isHide) {

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

}

/**
 * //【オーバーライド用関数】ツリービュー読込後処理
 * @param {any} selector ツリービューのセレクタ
 * @param {any} isTreeMenu 左側メニューの場合true
 * @param {any} grpId 構成グループID
 */
function afterLoadedTreeView(selector, isTreeMenu, grpId) {

}

/**
 * //【オーバーライド用関数】ツリービューリフレッシュ後処理
 * @param {any} selector ツリービューのセレクタ
 * @param {any} isTreeMenu 左側メニューの場合true
 * @param {any} grpId 構成グループID
 */
function afterRefreshTreeView(selector, isTreeMenu, grpId) {
    //職種ツリーのマージ処理後に職種ツリーがリフレッシュされる際の処理
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

}

/**
 *【オーバーライド用関数】実行ﾁｪｯｸ処理 - 前処理
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 */
function prevCommonValidCheck(appPath, conductId, formNo, btn) {

    var isContinue = true; // 個別処理後も共通の入力チェックを行う場合はtrue
    var isError = false;   // エラーがある場合はtrue

    return [isContinue, isError];
}

function prevAfterBackExec(appPath, conductId, pgmId, formNo, ctrlId) {
    return false;
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

    // 何もしない場合はそのまま返す
    return listData;
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


}