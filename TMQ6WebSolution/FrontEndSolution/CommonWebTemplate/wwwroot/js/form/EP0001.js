/* ========================================================================
 *  機能名　    ：   【EP0001】ExcelPortダウンロード
 * ======================================================================== */

/**
 * 自身の相対パスを取得
 */
function getPath() {
    var root;
    var scripts = document.getElementsByTagName("script");
    var i = scripts.length;
    while (i--) {
        var match = scripts[i].src.match(/(^|.*\/)EP0001\.js$/);
        if (match) {
            root = match[1];
            break;
        }
    }
    return root;
}

document.write("<script src=\"" + getPath() + "/tmqcommon.js\"></script>");

// ==================================================削除する
const EP0001_FormListDel = {
    ConductId: "EP0001",                // 機能ID
    No: 0,                              // 画面番号
    Id: "BODY_000_00_LST_0",            // コンボボックス一覧ID
    ConductCategoryName: 1,             // 機能分類名コンボボックス
    ConductName: 2,                     // 機能名コンボボックス
    HideConductId: 3,                   // 機能ID(隠し項目)
    HideSheetNo: 4,                     // シート番号(隠し項目)
};
// ==================================================削除する

// ExcelPort 一覧 コントロール項目番号
const EP0001_FormList = {
    ConductId: "EP0001",                // 機能ID
    No: 0,                              // 画面番号
    Id: "BODY_000_00_LST_0",            // コンボボックス一覧ID
    HideDownLoadButton: 4,              // ダウンロードボタン(隠し項目)
    HideConductId: 5,                   // 機能ID(隠し項目)
    HideSheetNo: 6,                     // シート番号(隠し項目)
    HideAddCondition: 7                 // 追加条件区分(隠し項目)
};

// ExcelPort ダウンロード条件 コントロール項目番号
const EP0001_DownLoadCondition = {
    No: 1,                              // 画面番号
    Id: "BODY_000_00_LST_1",            // ダウンロード条件一覧ID
    MaintenanceTarget: 2,               // メンテナンス対象
    Factory: 3,                         // 工場
    HideFactoryRequired: 4,             // 工場単一選択必須区分(隠し項目)
    OccurrenceDate: 7,                  // 発生日
    ExpectedConstructionDate: 8,        // 着工予定日
    CompletionDate: 9,                  // 完了日
    CompletionIndicator: 10,　          // 完了区分
    HideAddCondition: 11,               // 追加条件区分(隠し項目)
    IssueDate: 12                       // 発行日
};

/*
 * 機能IDを判定
 */
function EP0001_JudgeConductId() {
    // 機能IDを取得
    var conductId = $(P_Article).find('input[name="CONDUCTID"]').val();

    // ExcelPortダウンロードの機能IDでなければfalse
    if (conductId != EP0001_FormList.ConductId) {
        return false;
    }
    else {
        return true;
    }
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
    // 機能IDが「ExselPortダウンロード」の場合のみ処理を行う
    if (!EP0001_JudgeConductId()) {
        return;
    }

    // ダウンロード条件画面の場合
    if (formNo == EP0001_DownLoadCondition.No) {

        // コントロール要素の定義
        var FactoryCtrl = $(getCtrl(EP0001_DownLoadCondition.Id, EP0001_DownLoadCondition.Factory, 1, CtrlFlag.Combo));
        var MaintenanceTarget = $(getCtrl(EP0001_DownLoadCondition.Id, EP0001_DownLoadCondition.MaintenanceTarget, 1, CtrlFlag.Combo));
        var OccurrenceDateCtrl = $(getCtrl(EP0001_DownLoadCondition.Id, EP0001_DownLoadCondition.OccurrenceDate, 1, CtrlFlag.Input));
        var ExpectedConstructionDateCtrl = $(getCtrl(EP0001_DownLoadCondition.Id, EP0001_DownLoadCondition.ExpectedConstructionDate, 1, CtrlFlag.Input));
        var CompletionDateCtrl = $(getCtrl(EP0001_DownLoadCondition.Id, EP0001_DownLoadCondition.CompletionDate, 1, CtrlFlag.Input));
        var CompletionIndicatorCtrl = $(getCtrl(EP0001_DownLoadCondition.Id, EP0001_DownLoadCondition.CompletionIndicator, 1, CtrlFlag.Combo));
        var IssueDateCtrl = $(getCtrl(EP0001_DownLoadCondition.Id, EP0001_DownLoadCondition.IssueDate, 1, CtrlFlag.Input));

        // 工場非表示
        $(FactoryCtrl).closest("td").hide();
        $(FactoryCtrl).closest("tr").find("th").hide();

        // 追加条件区分の取得
        var addCondition = getValue(EP0001_DownLoadCondition.Id, EP0001_DownLoadCondition.HideAddCondition, 1, CtrlFlag.Label);

        // 追加条件区分が1の場合
        if (addCondition == 1) {
            // 発生日非表示
            $(OccurrenceDateCtrl).closest("td").hide();
            $(OccurrenceDateCtrl).closest("tr").find("th").hide();
            // 着工予定日非表示
            $(ExpectedConstructionDateCtrl).closest("td").hide();
            $(ExpectedConstructionDateCtrl).closest("tr").find("th").hide();
            // 完了日非表示
            $(CompletionDateCtrl).closest("td").hide();
            $(CompletionDateCtrl).closest("tr").find("th").hide();
            // 完了区分非表示
            $(CompletionIndicatorCtrl).closest("td").hide();
            $(CompletionIndicatorCtrl).closest("tr").find("th").hide();
            // 発行日非表示
            $(IssueDateCtrl).closest("td").hide();
            $(IssueDateCtrl).closest("tr").find("th").hide();

            // メンテナンス対象を必須にする
            setRequiredElement($(MaintenanceTarget).closest("td").find("select"), true);
            setRequiredHeaderElement($(MaintenanceTarget).closest("tr").find("th"), true);

            // メンテナンス対象のヘッダーにspan要素がない場合
            if (!$(MaintenanceTarget).closest("tr").find("th").find('span').length) {
                // span要素を追加する
                $(MaintenanceTarget).closest("tr").find("th").append('<span></span>');
            }
            // span要素に必須マークのクラスを付与する
            $(MaintenanceTarget).closest("tr").find("th").find("span").addClass('mark');

        }
        // 追加条件区分が2の場合
        else if (addCondition == 2) {
            // メンテナンス対象非表示
            $(MaintenanceTarget).closest("td").hide();
            $(MaintenanceTarget).closest("tr").find("th").hide();

            // 工場のコンボボックス非表示
            $(FactoryCtrl).closest("td").hide();
            $(FactoryCtrl).closest("tr").find("th").hide();

            // 完了区分を必須にする
            setRequiredElement($(CompletionIndicatorCtrl).closest("td").find("select"), true);
            setRequiredHeaderElement($(CompletionIndicatorCtrl).closest("tr").find("th"), true);

            // 完了区分のヘッダーにspan要素がない場合
            if (!$(CompletionIndicatorCtrl).closest("tr").find("th").find('span').length) {
                // span要素を追加する
                $(CompletionIndicatorCtrl).closest("tr").find("th").append('<span></span>');
            }
            // span要素に必須マークのクラスを付与する
            $(CompletionIndicatorCtrl).closest("tr").find("th").find("span").addClass('mark');

        }
        //Validator初期化
        initValidator("#" + P_formDetailId);
    }
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

    // 機能IDが「ExselPortダウンロード」の場合のみ処理を行う
    if (!EP0001_JudgeConductId()) {
        return;
    }

    // ==================================================削除する
    if (selected != undefined && ctrlId == EP0001_FormListDel.Id) {
        // 機能分類名のコンボボックスが変更された場合
        if (valNo == EP0001_FormListDel.ConductCategoryName) {
            selectComboByExparam(EP0001_FormListDel.Id, EP0001_FormListDel.ConductName, 3, selected.VALUE1);
            // 拡張項目1のデータを画面の隠し項目にセットする
            setComboExValue(EP0001_FormListDel.Id, EP0001_FormListDel.HideConductId, 1, selected, false, 1);
            // シート番号(隠し項目)をクリアする
            setValue(EP0001_FormListDel.Id, EP0001_FormListDel.HideSheetNo, EP0001_FormListDel.HideSheetNo, 1, '', false, false);
        }
        // 機能名のコンボボックスが変更された場合
        else if (valNo == EP0001_FormListDel.ConductName) {
            // 拡張項目1のデータを画面の隠し項目にセットする
            setComboExValue(EP0001_FormListDel.Id, EP0001_FormListDel.HideConductId, 1, selected, false, 1);
            // 拡張項目2のデータを画面の隠し項目にセットする
            setComboExValue(EP0001_FormListDel.Id, EP0001_FormListDel.HideSheetNo, 1, selected, false, 2);
        }    
    }
    // ==================================================削除する
    if (selected != undefined && ctrlId == EP0001_DownLoadCondition.Id) {
        // コントロール要素の定義
        var HideFactoryRequiredCtrl = $(getCtrl(EP0001_DownLoadCondition.Id, EP0001_DownLoadCondition.HideFactoryRequired, 1, CtrlFlag.Label));
        var FactoryCtrl = $(getCtrl(EP0001_DownLoadCondition.Id, EP0001_DownLoadCondition.Factory, 1, CtrlFlag.Combo));

        // メンテナンス対象のコンボボックスが変更された場合
        if (valNo == EP0001_DownLoadCondition.MaintenanceTarget) {
            // メンテナンス対象のコンボボックスにC0002を指定し、1つ目のexparamに拡張項目の値がくるため拡張項目番号1を指定する
            setComboExValue(EP0001_DownLoadCondition.Id, EP0001_DownLoadCondition.HideFactoryRequired, 1, selected, false, 1);

            // 工場単一選択必須区分が1の場合
            if (getValue(EP0001_DownLoadCondition.Id, EP0001_DownLoadCondition.HideFactoryRequired, 1, CtrlFlag.Label) == "1") {
                // 工場のコンボボックス表示
                $(FactoryCtrl).closest("td").show();
                $(FactoryCtrl).closest("tr").find("th").show();

                // 工場のコンボボックスを必須にする
                setRequiredElement($(FactoryCtrl).closest("td").find("select"), true);
                setRequiredHeaderElement($(FactoryCtrl).closest("tr").find("th"), true);

                // 工場のコンボボックスのヘッダーにspan要素がない場合
                if (!$(FactoryCtrl).closest("tr").find("th").find('span').length) {
                    // span要素を追加する
                    $(FactoryCtrl).closest("tr").find("th").append('<span></span>');
                }
                // span要素に必須マークのクラスを付与する
                $(FactoryCtrl).closest("tr").find("th").find("span").addClass('mark');
            }
            else {

                // 工場のコンボボックス非表示
                $(FactoryCtrl).closest("td").hide();
                $(FactoryCtrl).closest("tr").find("th").hide();

                //工場単一選択必須区分(隠し項目)をクリアする
                setValue(EP0001_DownLoadCondition.Id, EP0001_DownLoadCondition.HideFactoryRequired, EP0001_DownLoadCondition.HideFactoryRequired, 1, '', false, false);
            }

            //Validator初期化
            initValidator("#" + P_formDetailId);
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
function prevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element) {
    // 機能IDが「ExselPortダウンロード」の場合のみ処理を行う
    if (!EP0001_JudgeConductId()) {
        return;
    }

    var conditionDataList = [];
    // 選択された行を取得
    var selectedRow = $(element).parent().parent();
    // 追加条件区分(隠し列)の取得
    var addCondition = $(selectedRow).find("div[tabulator-field='VAL" + EP0001_FormList.HideAddCondition + "']")[0].innerText;
    // 追加条件区分が1もしくは2の場合
    if (addCondition == 1 || addCondition == 2) {
        // 選択されたRowNoの取得
        var rowNo = $(element).closest("div.tabulator-row").find("div[tabulator-field=ROWNO]")[0].innerText;
        var tblId = "#" + $(element).closest("div.ctrlId").attr("id");
        // フィルターを掛けている場合を考慮して以下で行番号を取得
        rowNo = P_listData[tblId].getRowFromPosition(parseInt(rowNo, 10), true).getData().ROWNO;
        // 検索条件へ追加
        conditionDataList.push(getTempDataForTabulator(formNo, rowNo, tblId));

        return [true, conditionDataList];
    }
    // 隠しダウンロードボタンをクリック
    $(selectedRow).find("div[tabulator-field='VAL" + EP0001_FormList.HideDownLoadButton + "']").find("input")[0].click();
    // 遷移させない
    return [false, conditionDataList];
}

/**
 * 【オーバーライド用関数】Tabulatorの描画が完了時の処理
 * @param {any} tbl 一覧
 * @param {any} id 一覧のID(#,_FormNo付き)
 */
function postBuiltTabulator(tbl, id) {
    // 機能IDが「ExselPortダウンロード」の場合のみ処理を行う
    if (!EP0001_JudgeConductId()) {
        return;
    }
    // 画面にある行番号を非表示にする
    $("div[tabulator-field=ROWNO]").hide();
}

/**
 * 【オーバーライド用関数】Tabulatorのページ読込後の処理
 */
function postTabulatorChangePage() {
    // 機能IDが「ExselPortダウンロード」の場合のみ処理を行う
    if (!EP0001_JudgeConductId()) {
        return;
    }
    // 画面にある行番号を非表示にする
    $("div[tabulator-field=ROWNO]").hide();
}

/**
 * 【オーバーライド用関数】Tabulatorの列フィルター後の処理
 */
function postTabulatorDataFiltered(){
    // 機能IDが「ExselPortダウンロード」の場合のみ処理を行う
    if (!EP0001_JudgeConductId()) {
        return;
    }
    // 画面にある行番号を非表示にする
    $("div[tabulator-field=ROWNO]").hide();
}

/**
 * 【オーバーライド用関数】Tabulatorのレンダリング完了後の処理
 */
function postTabulatorRenderCompleted() {
    // 機能IDが「ExselPortダウンロード」の場合のみ処理を行う
    if (!EP0001_JudgeConductId()) {
        return;
    }
    // 画面にある行番号を非表示にする
    $("div[tabulator-field=ROWNO]").hide();
}

/*
* 必須項目の「＊」をセットする(ヘッダー)
* @param elm セットする項目
* @param isSet true:セットする/false:セットされたのを外す
*/
function setRequiredHeaderElement(elm, isSet) {
    if (isSet) {
        elm.addClass('required');
    } else {
        elm.removeClass('required');
    }
}