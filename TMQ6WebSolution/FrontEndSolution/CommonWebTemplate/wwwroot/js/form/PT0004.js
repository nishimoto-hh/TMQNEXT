/* ========================================================================
 *  機能名　    ：   【PT0004】在庫確定状況
 * ======================================================================== */

/**
 * 自身の相対パスを取得
 */
function getPath() {
    var root;
    var scripts = document.getElementsByTagName("script");
    var i = scripts.length;
    while (i--) {
        var match = scripts[i].src.match(/(^|.*\/)PT0004\.js$/);
        if (match) {
            root = match[1];
            break;
        }
    }
    return root;
}

document.write("<script src=\"" + getPath() + "/tmqcommon.js\"></script>");


// 一覧画面 コントロール番号
const FormList = {
    Id: "BODY_020_00_LST_0",            // 在庫確定一覧
    FactoryIdList: "BODY_040_00_LST_0", // 工場ID一覧
    JobIdList: "BODY_050_00_LST_0",     // 職種ID一覧
    FlgList: "BODY_060_00_LST_0",       // 処理フラグ一覧
    BtnActiveFlg: 1,                    // フラグ
    Button: {
        Search: "Search",               // 検索ボタン
        ExecutCommit: "ExecuteCommit",  // 確定実行ボタン
        CancelCommit: "CancelCommit"    // 確定解除ボタン
    }
}

// 確定実行ボタンを非活性にするための値
const JudgeActiveVal = 1;

/**
 *【オーバーライド用関数】実行ﾁｪｯｸ処理 - 前処理
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 */
function registCheckPre(appPath, conductId, formNo, btn) {

    // 一覧にチェックされた行が存在しない場合、遷移をキャンセル
    if (!isCheckedList(FormList.Id)) {
        return false;
    }

    return true;
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

    // 工場ID一覧を非表示
    changeListDisplay(FormList.FactoryIdList, false, false);

    // 職種ID一覧を非表示
    changeListDisplay(FormList.JobIdList, false, false);

    // 処理フラグ一覧を非表示
    changeListDisplay(FormList.FlgList, false, false);

    // 検索ボタンにフォーカスを設定
    setFocusButton(FormList.Button.Search);
}

/**
 * 【オーバーライド用関数】Tabulatorの描画が完了時の処理
 * @param {any} tbl 一覧
 * @param {any} id 一覧のID(#,_FormNo付き)
 */
function postBuiltTabulator(tbl, id) {

    if (id == "#" + FormList.JobIdList + getAddFormNo()) {
        // 対象年月が当月以上の場合
        if (getValue(FormList.FlgList, FormList.BtnActiveFlg, 1, CtrlFlag.Label, false, false) == JudgeActiveVal) {

            // 確定実行ボタンを非活性にする
            setDispMode(getButtonCtrl(FormList.Button.ExecutCommit), true);

            // 確定解除ボタンを非活性にする
            setDispMode(getButtonCtrl(FormList.Button.CancelCommit), true);
        }
    }
}