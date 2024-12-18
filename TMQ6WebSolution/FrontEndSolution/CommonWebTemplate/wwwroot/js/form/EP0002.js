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

//★インメモリ化対応 start
/** 個別対応機能ID */
const EP0002_ConductId = {
    /** 機種別仕様 */
    MS0020: "MS0020"
};
//★インメモリ化対応 end

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

/*==8:取込処理==*/

/**
 *【オーバーライド用関数】
 * キャンセル押下後
 */
function clickPopupCancelBtn() {
    // 個別実装用データクリア
    var form = $(P_Article).find("form[id^='form']");
    $(form).find("input:hidden[name='ListIndividual']").val("");
    P_dicIndividual["TargetConductId"] = null;
    P_dicIndividual["TargetSheetNo"] = null;

    // 画面変更ﾌﾗｸﾞ初期化
    dataEditedFlg = false;

    //処理中メッセージ：off
    processMessage(false);
    // 実行中フラグOFF
    P_ProcExecuting = false;

    // アップロードボタンを活性化
    var btn = $(form).find("input:button[data-actionkbn='" + actionkbn.ExcelPortUpload + "']");
    $(btn).prop("disabled", false);

    // 取込ファイル情報をクリア
    $(form).find("input:file").val("");
}

//★インメモリ化対応 start
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
 *  @param {Dictionary<string>}         status      ：処理ステータス
 */
function postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data, status) {
    // 機能IDが「ExselPortアップロード」の場合のみ処理を行う
    if (!EP0002_JudgeConductId()) {
        return;
    }

    // 正常終了時のみ処理を行う
    if (status.STATUS != procStatus.Valid) {
        return;
    }

    // 対象の構成グループIDを取得
    const grpId = getTargetStructureGrpId();
    if (grpId == null) {
        return;
    }

    if (grpId == structureGroupDef.Location || grpId == structureGroupDef.Job || grpId == structureGroupDef.FailureCausePersonality) {
        // 場所階層、職種・機種、原因性格の場合

        // ツリービューの再作成
        refreshTreeView(appPath, conductId, grpId);
    }

    // コンボボックスの再作成
    refreshComboBox(appPath, conductId, grpId);
}

/*
* 対象の構成グループIDを取得
*/

/**
 * 対象の構成グループIDを取得
 * @returns {string} 構成グループID
 */
function getTargetStructureGrpId() {
    // 機能IDを取得
    var targetConductId = P_dicIndividual["TargetConductId"];
    if (!targetConductId || !targetConductId.startsWith('MS')) {
        return null;
    }
    // グローバルデータから構成グループIDを取得
    var targetGrpId = P_dicIndividual["TargetGrpId"];
    if (targetGrpId == null && targetConductId != EP0002_ConductId.MS0020) {
        // 機種別仕様以外は機能IDから抽出
        targetGrpId = targetConductId.replace('MS', '');
    }
    return targetGrpId;
}
//★インメモリ化対応 end
