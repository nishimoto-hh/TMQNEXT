/* ========================================================================
 *  機能名　    ：   【EP0002】ExcelPortアップロード
 * ======================================================================== */

/**
 * 自身の相対パスを取得
 */
function getPath() {
    var root;
    var scripts = document.getElementsByTagName("script");
    var i = scripts.length;
    while (i--) {
        var match = scripts[i].src.match(/(^|.*\/)EP0002\.js$/);
        if (match) {
            root = match[1];
            break;
        }
    }
    return root;
}
document.write("<script src=\"" + getPath() + "/tmqcommon.js\"></script>");
// 一覧画面の定義
const EP0002_FormList = {
    ConductId: "EP0002",                // 機能ID
    No: 0,                              // 画面番号
    Id: "BODY_000_00_LST_0",            // コンボボックス一覧ID
    ConductCategoryName: 1,             // 機能分類名コンボボックス
    ConductName: 2,                     // 機能名コンボボックス
    HideConductId: 3,                   // 機能ID(隠し項目)
    HideSheetNo: 4,                     // シート番号(隠し項目)
    UploadFile: 5                       //アップロードファイル
};

/*
 * 機能IDを判定
 */
function EP0002_JudgeConductId() {
    // 機能IDを取得
    var conductId = $(P_Article).find('input[name="CONDUCTID"]').val();

    // ExcelPortアップロードの機能IDでなければfalse
    if (conductId != EP0002_FormList.ConductId) {
        return false;
    }
    else {
        return true;
    }
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

    // 機能IDが「ExselPortアップロード」の場合のみ処理を行う
    if (!EP0002_JudgeConductId()) {
        return;
    }

    if (selected != undefined && ctrlId == EP0002_FormList.Id) {
        // 機能分類名のコンボボックスが変更された場合
        if (valNo == EP0002_FormList.ConductCategoryName) {
            selectComboByExparam(EP0002_FormList.Id, EP0002_FormList.ConductName, 3, selected.VALUE1);
            // 拡張項目1のデータを画面の隠し項目にセットする
            setComboExValue(EP0002_FormList.Id, EP0002_FormList.HideConductId, 1, selected, false, 1);
            // シート番号(隠し項目)をクリアする
            setValue(EP0002_FormList.Id, EP0002_FormList.HideSheetNo, EP0002_FormList.HideSheetNo, 1, '', false, false);
        }
        // 機能名のコンボボックスが変更された場合
        else if (valNo == EP0002_FormList.ConductName) {
            // 拡張項目1のデータを画面の隠し項目にセットする
            setComboExValue(EP0002_FormList.Id, EP0002_FormList.HideConductId, 1, selected, false, 1);
            // 拡張項目2のデータを画面の隠し項目にセットする
            setComboExValue(EP0002_FormList.Id, EP0002_FormList.HideSheetNo, 1, selected, false, 2);
        }
    }
}

