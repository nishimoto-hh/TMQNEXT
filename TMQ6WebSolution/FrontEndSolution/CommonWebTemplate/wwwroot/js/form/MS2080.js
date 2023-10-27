/* ========================================================================
 *  機能名　    ：   【MS2080】長期計画メール送信情報マスタ
 * ======================================================================== */

///**
// * 自身の相対パスを取得
// */
//function getPath() {
//    var root;
//    var scripts = document.getElementsByTagName("script");
//    var i = scripts.length;
//    while (i--) {
//        var match = scripts[i].src.match(/(^|.*\/)MS2080\.js$/);
//        if (match) {
//            root = match[1];
//            break;
//        }
//    }
//    return root;
//}

//document.write("<script src=\"" + getPath() + "/tmqcommon.js\"></script>");
//document.write("<script src=\"" + getPath() + "/tmqmaster.js\"></script>");

// 機能ID
const ConductId_MS2080 = "MS2080";

// 拡張項目列
const ItemExCol = { ExData1: 11, ExData2: 12, ExData3: 13, ExData4: 14, ExData5: 15, ExData6: 16, ExData7: 17, ExData8: 18, ExData9: 19  }
// 拡張項目コントロール列(TextBox: 0, Label: 1, Combo: 2, ChkBox: 3, Link: 4, Input: 5, Textarea: 6, Password: 7, Button: 8)
const ItemExCtrlCol = { ExData1: 0, ExData2: 0, ExData3: 0, ExData4: 0, ExData5: 0, ExData6: 0, ExData7: 0, ExData8: 0, ExData9: 0 }

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
    // 初期化処理（マスタメンテナンス用）
    initFormOriginalForMaster(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data, ItemExCol, ItemExCtrlCol);
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
    // 遷移処理の前処理（マスタメンテナンス用）
    return prevTransFormForMaster(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element);
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
    // 戻る処理の前(単票、子画面共用)（マスタメンテナンス用）
    return prevBackBtnProcessForMaster(appPath, btnCtrlId, status, codeTransFlg);
}

/**
 *【オーバーライド用関数】
 *  行追加の前
 *  
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
 *【オーバーライド用関数】
 *  行削除の前
 *
 *  @appPath    {string}    ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @btn        {<a>}       ：行削除ﾘﾝｸのa要素
 *  @id         {string}    ：行削除ﾘﾝｸの対象一覧のCTRLID
 *  @checkes    {要素}      ：削除対象の要素リスト(tabulator一覧の場合、削除対象のデータリスト)
 */
function preDeleteRow(appPath, btn, id, checkes) {
    // 共通処理により非表示の削除ボタンを押下
    return preDeleteRowCommon(id, [MasterFormList.StandardItemList.Id, MasterFormList.FactoryItemList.Id]);
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
    // 検索処理前（マスタメンテナンス用）
    beforeSearchBtnProcessForMaster(appPath, btn, conductId, pgmId, formNo, conductPtn)
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
    // 登録前追加条件取得処理（マスタメンテナンス用）
    return addSearchConditionDictionaryForRegistForMaster(appPath, conductId, formNo, btn);
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
    var isContinue = true
    var isError = false;
    var isAutoBackFlg = true;

    return [isContinue, isError, isAutoBackFlg];
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
    // Tabuator一覧の描画前処理 (マスタメンテナンス用)
    prevCreateTabulatorForMaster(appPath, id, options, header, dispData)
}

/**
 * 【オーバーライド用関数】Tabulatorの描画が完了時の処理
 * @param {any} tbl 一覧
 * @param {any} id 一覧のID(#,_FormNo付き)
 */
function postBuiltTabulator(tbl, id) {
    // Tabulatorの描画が完了時の処理 (マスタメンテナンス用)
    postBuiltTabulatorForMaster(tbl, id);
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
    // 画面初期値データ取得前処理(表示中画面用) (マスタメンテナンス用)
    return prevInitFormDataForMaster(appPath, formNo, btnCtrlId, conditionDataList, listDefines, pgmId);
}