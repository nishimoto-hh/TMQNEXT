/* ========================================================================
 *  機能名　    ：   【RM0001】担当者検索
 * ======================================================================== */
/**
 * 自身の相対パスを取得
 */
function getPath() {
    var root;
    var scripts = document.getElementsByTagName("script");
    var i = scripts.length;
    while (i--) {
        var match = scripts[i].src.match(/(^|.*\/)RM0001\.js$/);
        if (match) {
            root = match[1];
            break;
        }
    }
    return root;
}
// 他機能からこの画面が呼び出された場合、tmqcommon.jsが二重に呼び出される
// 定数の宣言などでエラーとなるので、既に呼び出されている場合は読み込まないように対応
var isExistsTmqCommon = false;
$.each(document.scripts, function (idx, value) {
    if (value.src.indexOf('tmqcommon') > 0) {
        isExistsTmqCommon = true;
    }
});
if (!isExistsTmqCommon) {
    document.write("<script src=\"" + getPath() + "/tmqcommon.js\"></script>");
}

// 帳票出力　抽出条件 コントロール項目番号
const RM0001_FormCondition = {
    Id: "CBODY_000_00_LST_0_RM0001",                // 抽出条件
    OutputReport: 1,                                // 出力帳票
    OutputTemplate: 3,                              // テンプレート名
    OutputPattern: 4,                               // 出力パターン
    Factory: 2,                                     // 工場
    ParentPgmId: 5,                                 // 遷移元のプログラムID
    TargetReportId: 6,                              // 遷移元の帳票ID
    TemplateFilePath: 7,                            // テンプレートファイルパス
    TemplateFileName: 8,                            // テンプレートファイル名
    AuthorityLevel: 9,                              // 権限レベル
    TargetFactoryId: 10,                             //
    TargetTemplateId: 11,                             //
    TargetPatternId: 12,                             //
    SqlId:                                          // SqlId（コンボボックス）
    {
        Factory: "C0008",
        OutputTemplate: "C0009",
        OutputPattern: "C0010",
    },
    Param:                                          // Parameter（コンボボックス）
    {
        Factory: "@1,911070005,@3",
        OutputTemplate: "@1,@2",
        OutputPattern: "@1,@2,@3",
    },
    WaitTime: 300                                   // 待ち時間
}

// 帳票出力　抽出条件 ボタングループ コントロール項目番号
const RM0001_FormConditionBtnGrp = {
    Id: "CBODY_010_00_BTN_0_RM0001",                // 抽出条件ボタングループ
    BtnSearch: "RM0001_Search",                     // 検索
    BtnClear: "RM0001_Clear",                       // クリア
}

// 帳票出力　出力項目一覧 ボタングループ コントロール項目番号
const RM0001_FormListBtnGrp = {
    Id: "CBODY_030_00_BTN_0_RM0001",                // 抽出条件ボタングループ
    BtnOutput: "RM0001_Output",                     // 出力
    BtnBack: "RM0001_Back",                         // 戻る
}

// 一覧画面 コントロール項目番号
const RM0001_FormList = {
    Id: "CBODY_020_00_LST_0_RM0001",                // 検索結果一覧
    OutputItemId: 8,                                // 項目ID（出力項目マスタ）
    OutputItemType: 9,                              // 出力項目種別
    TemplateFilePath: 10,                           // テンプレートファイルパス
    TemplateFileName: 11,                           // テンプレートファイル名
    BtnOutput: "RM0001_Output",                     // 出力ボタン
    SearchArea: "search_divid_switch_RM0001",       // 検索条件エリア
    OutputItemTypePatternExists : "3"               // 出力項目種別出力パターン指定あり
}

// 帳票出力項目一覧 ボタングループ上部 コントロール項目番号
const RM0001_FormOutItemList = {
    Id: "CBODY_015_00_LST_0_RM0001",                // 出力パターン
    BtnRegist: "RM0001_Regist",                     // 更新
    BtnDelete: "RM0001_Delete",                     // 削除
    BtnNew: "RM0001_New",                           // 新規
    BtnUpload: "RM0001_Upload",                     // アップロード
    BtnDownload: "RM0001_Download",                 // ダウンロード
}

// 出力パターン登録画面 コントロール項目番号
const RM0001_FormOutPattern = {
    Id: "CBODY_040_00_LST_0_RM0001",                // 出力パターン
    BtnRegistOutPattern: "RM0001_RegistOutPattern", // 登録ボタン
    BtnCancelOutPattern: "RM0001_CancelOutPattern", // キャンセルボタン
    PatternName: 1,                                 // パターン名コントロール
}

// 雛形ファイル登録画面 コントロール項目番号
const RM0001_FormUploadTemp = {
    Id: "CBODY_060_00_LST_0_RM0001",                // ファイル名
    BtnCtrlId: "CBODY_070_00_BTN_0_RM0001",         // ボタンコントロールID
    BtnUploadTmpFile: "RM0001_UploadTmpFile",       // 登録ボタン
    BtnCancelTmpFile: "RM0001_CancelTmpFile",       // キャンセルボタン
    File: 1,                                        // ファイル選択コントロール
    TempName: 2                                     // テンプレート名
}

// 項目を非表示にするクラス
const RM0001_Hide = "hide";

// グローバルリストのキー名称
const RM0001_GlobalListKey = {
    ParentConductId: "RM0001_ParentConductId",　　　　　　 // 呼び元画面の機能ID
    ParentPgmId: "RM0001_ParentPgmId",　　　　　　 　　　　// 呼び元画面のプログラムID
    TargetReportId: "RM0001_TargetReportId",　　　　　　 　// 呼び元画面の選択帳票ID
    TargetCtrlId: "RM0001_TargetCtrlId",　　　　　　       // 呼び元画面の対象のコントロールID
    TargetOptionCtrlId: "RM0001_TargetOptionCtrlId",　　　 // 呼び元画面の対象のコントロールID（長期スケジュール用）
    OptionDataList: "RM0001_OptionDataList",　　　         // 呼び元画面の対象のコントロールID（長期スケジュール用）の設定値一式
    AuthorityLevel: "RM0001_AuthorityLevel",　　　         // ユーザーの権限レベル（画面側から取得）
    RegistFactoryId: "RM0001_RegistFactoryId",　　　         // 工場パターンID（画面側から取得）
    RegistPatternId: "RM0001_RegistPatternId",　　　         // 登録パターンID（画面側から取得）
    RegistTemplateId: "RM0001_RegistTemplateId",　　　         // 登録テンプレートID（画面側から取得）
}

// 帳票管理画面 コントロール項目番号（パラメータ引き渡し用）
const RM0002_FormList = {
    Id: "BODY_020_00_LST_0",                        // 出力項目一覧
    FactoryId: 2,                                   // 工場ID
    ProgramId: 3,                                   // プログラムID
    ReportId: 4                                     // 帳票ID
}

// 帳票一覧画面 機能ID
const RM0001_ConductIdList = {
    ConductIdRM0002: "RM0002"                              // 帳票管理
}

// 帳票一覧画面 プログラムID
const RM0001_ProgramIdList = {
    ProgramIdMC0001: "MC0001",                             // 機器台帳
    ProgramIdLN0001: "LN0001",                             // 件名別長期計画
    ProgramIdLN0002: "LN0002",                             // 機器別長期計画
    ProgramIdMA0001: "MA0001",                             // 保全活動
    ProgramIdPT0001: "PT0001",                             // 予備品管理
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
function RM0001_setPageStatusEx(status, pageRowCount, conductPtn, authShori) {

    if (status == pageStatus.INIT) {

        //　帳票出力　抽出条件ボタン制御（初期状態）
        changeFormConditionBtnGrp(pageStatus.INIT);
        // 実行中は行わない
        if (P_ProcExecuting == false) {
            // 帳票コンボボックスの再作成
            var selects = $(P_Article).find("#" + RM0001_FormCondition.Id + getAddFormNo()).find("td[data-name='VAL" + RM0001_FormCondition.OutputReport + "'] select.dynamic");
            // 描画に間に合っていないので少し待つ
            setTimeout(function () {
                // 帳票コンボの設定イベントを行う
                $(selects).trigger('change');
            }, RM0001_FormCondition.WaitTime); //300ミリ秒
        }
    } else if (status == pageStatus.SEARCH) {

    }
}

/**
 *   コンボボックス変更時イベント
 * @param  appPath    : アプリケーションルートパス
 * @param  ctrlId     : イベントの発生した画面のコントロールID
 * @param  valNo      : イベントの発生したコンボボックスのコントロール番号
 */
function changeComboValues(appPath, ctrlId, valNo) {

    // 連動コンボで同じイベントを何度も発生させないための対応
    if (ctrlId == RM0001_FormCondition.Id && valNo == RM0001_FormCondition.OutputReport) {
        var sqlId = RM0001_FormCondition.SqlId.Factory;
        var param = RM0001_FormCondition.Param.Factory; // "@1,911070005,@3";

        // 工場コンボボックスの初期化
        var tdFactory = $(P_Article).find("#" + RM0001_FormCondition.Id + getAddFormNo()).find("td[data-name='VAL" + RM0001_FormCondition.Factory + "']");
        var selectFactory = $(tdFactory).find("select");
        //ｺﾝﾎﾞ一覧の初期化
        $(selectFactory).children().remove();
        $(selectFactory).off('change');

        // 帳票コンボボックスの値をパラメータに埋め込み
        var tdOutputReport = $(P_Article).find("#" + RM0001_FormCondition.Id + getAddFormNo()).find("td[data-name='VAL" + RM0001_FormCondition.OutputReport + "']");
        var changeVal = getCellVal(tdOutputReport);
        // 値が取れない場合、終了する
        if (changeVal == ""){ return; }
        param = param.replace("@1", changeVal);

        var tdAuthority = $(P_Article).find("#" + RM0001_FormCondition.Id + getAddFormNo()).find("td[data-name='VAL" + RM0001_FormCondition.AuthorityLevel + "']");
        var authorityVal = getCellVal(tdAuthority);
        // 権限レベルが管理者かどうか
        if (authorityVal == "99") {
            param = param.replace("@3", "0");
        }
        else {
            param = param.replace("@3", "1");
        }
        
        // 工場コンボボックスの再設定
        var ctrlOption = 0; // 空白なし
        // var ctrlOption = 2; // 空白なし
        // var tdFactory = $(P_Article).find("#" + RM0001_FormCondition.Id + getAddFormNo()).find("td[data-name='VAL" + RM0001_FormCondition.Factory + "']");
        // var selectFactory = $(tdFactory).find("select");
        var isNullCheck = $(selectFactory).hasClass("validate_required");

        // 工場コンボボックスのセッションストレージ上の情報をクリア
        const paramKey = sqlId + "," + param;
        removeSaveDataFromSessionStorage(sessionStorageCode.CboMasterData, paramKey);
        
        initComboBox(appPath, $(selectFactory), sqlId, param, ctrlOption, isNullCheck, -1);

        setTimeout(function () {
            // 工場コンボの先頭を選択して、設定イベントを行う
            // 登録処理後の場合、グローバル変数に登録値を保持している為、そちらを選択する
            var registFactoryId = P_dicIndividual[RM0001_GlobalListKey.RegistFactoryId];
            if (registFactoryId != null && $(registFactoryId).length > 0) {
                $(selectFactory).val(registFactoryId);
                // 設定後、値をクリアする
                operatePdicIndividual(RM0001_GlobalListKey.RegistFactoryId, true, "");
            }
            else {
                selectComboTop($(selectFactory));
            }

            setTimeout(function () {
                $(selectFactory).trigger('change');
            }, RM0001_FormCondition.WaitTime); //150ミリ秒
        }, RM0001_FormCondition.WaitTime); //150ミリ秒
    }
    if (ctrlId == RM0001_FormCondition.Id && valNo == RM0001_FormCondition.Factory) {
        var sqlId = RM0001_FormCondition.SqlId.OutputTemplate;
        var param = RM0001_FormCondition.Param.OutputTemplate; // "@1,@2"

        // テンプレートコンボボックスの初期化
        var tdOutputTemplate = $(P_Article).find("#" + RM0001_FormCondition.Id + getAddFormNo()).find("td[data-name='VAL" + RM0001_FormCondition.OutputTemplate + "']");
        var selectOutputTemplate = $(tdOutputTemplate).find("select");
        //ｺﾝﾎﾞ一覧の初期化
        $(selectOutputTemplate).children().remove();
        $(selectOutputTemplate).off('change');

        // 工場コンボボックスの値をパラメータに埋め込み
        var tdFactory = $(P_Article).find("#" + RM0001_FormCondition.Id + getAddFormNo()).find("td[data-name='VAL" + RM0001_FormCondition.Factory + "']");
        var changeVal = getCellVal(tdFactory);
        // 値が取れない場合、終了する
        if (changeVal == "") { return; }
        param = param.replace("@2", changeVal);

        // 帳票コンボボックスの値をパラメータに埋め込み
        var tdOutputReport = $(P_Article).find("#" + RM0001_FormCondition.Id + getAddFormNo()).find("td[data-name='VAL" + RM0001_FormCondition.OutputReport + "']");
        changeVal = getCellVal(tdOutputReport);
        // 値が取れない場合、終了する
        if (changeVal == "") { return; }
        param = param.replace("@1", changeVal);

        // テンプレートコンボボックスの再設定
        var ctrlOption = 2; // 空白あり
        //var tdOutputTemplate = $(P_Article).find("#" + RM0001_FormCondition.Id + getAddFormNo()).find("td[data-name='VAL" + RM0001_FormCondition.OutputTemplate + "']");
        //var selectOutputTemplate = $(tdOutputTemplate).find("select");
        var isNullCheck = $(selectOutputTemplate).hasClass("validate_required");

        // 工場コンボボックスのセッションストレージ上の情報をクリア
        const paramKey = sqlId + "," + param;
        removeSaveDataFromSessionStorage(sessionStorageCode.CboMasterData, paramKey);

        // コンボボックスの初期化
        initComboBox(appPath, $(selectOutputTemplate), sqlId, param, ctrlOption, isNullCheck, -1);

        setTimeout(function () {
            // テンプレートコンボの先頭を選択して、設定イベントを行う

            // 登録処理後の場合、グローバル変数に登録値を保持している為、そちらを選択する
            var registTemplateId = P_dicIndividual[RM0001_GlobalListKey.RegistTemplateId];
            if (registTemplateId != null && $(registTemplateId).length > 0)
            {
                $(selectOutputTemplate).val(registTemplateId);
                // 設定後、値をクリアする
                operatePdicIndividual(RM0001_GlobalListKey.RegistTemplateId, true, "");
            }
            else
            {
                selectComboIdx($(selectOutputTemplate), 1); // 空白ありのため、先頭の次を選択
            }
            setTimeout(function () {
                $(selectOutputTemplate).trigger('change');
            }, RM0001_FormCondition.WaitTime); //150ミリ秒
        }, RM0001_FormCondition.WaitTime); //150ミリ秒
    }
    if (ctrlId == RM0001_FormCondition.Id && valNo == RM0001_FormCondition.OutputTemplate) {
        // 出力パターンコンボボックスの再設定
        var sqlId = RM0001_FormCondition.SqlId.OutputPattern;
        var param = RM0001_FormCondition.Param.OutputPattern; // "@1,@2,@3";

        // パターンコンボボックスの初期化
        var tdOutputPattern = $(P_Article).find("#" + RM0001_FormCondition.Id + getAddFormNo()).find("td[data-name='VAL" + RM0001_FormCondition.OutputPattern + "']");
        var selectOutputPattern = $(tdOutputPattern).find("select");
        //ｺﾝﾎﾞ一覧の初期化
        $(selectOutputPattern).children().remove();
        $(selectOutputPattern).off('change');

        // テンプレートコンボボックスの値をパラメータに埋め込み
        var tdOutputTemplate = $(P_Article).find("#" + RM0001_FormCondition.Id + getAddFormNo()).find("td[data-name='VAL" + RM0001_FormCondition.OutputTemplate + "']");
        var changeVal = getCellVal(tdOutputTemplate);
        // 値が取れない場合、終了する
        if (changeVal == "") { return; }
        param = param.replace("@3", changeVal);

        // 工場コンボボックスの値をパラメータに埋め込み
        var tdFactory = $(P_Article).find("#" + RM0001_FormCondition.Id + getAddFormNo()).find("td[data-name='VAL" + RM0001_FormCondition.Factory + "']");
        changeVal = getCellVal(tdFactory);
        // 値が取れない場合、終了する
        if (changeVal == "") { return; }
        param = param.replace("@2", changeVal);

        // 帳票コンボボックスの値をパラメータに埋め込み
        var tdOutputReport = $(P_Article).find("#" + RM0001_FormCondition.Id + getAddFormNo()).find("td[data-name='VAL" + RM0001_FormCondition.OutputReport + "']");
        changeVal = getCellVal(tdOutputReport);
        // 値が取れない場合、終了する
        if (changeVal == "") { return; }
        param = param.replace("@1", changeVal);

        // パターンコンボボックスの再設定
        var ctrlOption = 2; // 空白あり
        var tdOutputPattern = $(P_Article).find("#" + RM0001_FormCondition.Id + getAddFormNo()).find("td[data-name='VAL" + RM0001_FormCondition.OutputPattern + "']");
        var selectOutputPattern = $(tdOutputPattern).find("select");
        var isNullCheck = $(selectOutputPattern).hasClass("validate_required");

        // 工場コンボボックスのセッションストレージ上の情報をクリア
        const paramKey = sqlId + "," + param;
        removeSaveDataFromSessionStorage(sessionStorageCode.CboMasterData, paramKey);

        // コンボボックスの初期化
        initComboBox(appPath, $(selectOutputPattern), sqlId, param, ctrlOption, isNullCheck, -1);

        // 出力パターンコンボの先頭を選択
        setTimeout(function () {
            // 登録処理後の場合、グローバル変数に登録値を保持している為、そちらを選択する
            var registPatternId = P_dicIndividual[RM0001_GlobalListKey.RegistPatternId];
            if (registPatternId != null && $(registPatternId).length > 0) {
                $(selectOutputPattern).val(registPatternId);
                // 設定後、値をクリアする
                operatePdicIndividual(RM0001_GlobalListKey.RegistPatternId, true, "");
            }
            else {
                selectComboIdx($(selectOutputPattern), 1); // 空白ありのため、先頭の次を選択
            }
        }, RM0001_FormCondition.WaitTime); //150ミリ秒
    }
}

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
function RM0001_initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data) {

    //セッションストレージに保存しているコンボボックスの選択値をクリア
    //setSaveDataToSessionStorage([], sessionStorageCode.CboMasterData, structureGroupDef.Job);

    ////　帳票出力　抽出条件ボタン制御（初期状態）
    //changeFormConditionBtnGrp(pageStatus.INIT);

    // 権限レベルをバック側から取得して画面の隠し項目に設定
    var tdAuthorityLevel = $(P_Article).find("#" + RM0001_FormCondition.Id + getAddFormNo()).find("td[data-name='VAL" + RM0001_FormCondition.AuthorityLevel + "']");
    var authorityLevel = P_dicIndividual[RM0001_GlobalListKey.AuthorityLevel];
    $(tdAuthorityLevel).text(authorityLevel);

    // 遷移後に遷移元画面からの情報をグローバル変数から受け取り、コンボボックスを再作成する対応
    // 遷移元画面のプログラムIDを画面の隠し項目に設定
    var tdParentPgmId = $(P_Article).find("#" + RM0001_FormCondition.Id + getAddFormNo()).find("td[data-name='VAL" + RM0001_FormCondition.ParentPgmId + "']");
    var parentPgmId = P_dicIndividual[RM0001_GlobalListKey.ParentPgmId];
    $(tdParentPgmId).text(parentPgmId);

    // 遷移元画面で選択した帳票IDを画面の隠し項目に設定
    var tdTargetReportId = $(P_Article).find("#" + RM0001_FormCondition.Id + getAddFormNo()).find("td[data-name='VAL" + RM0001_FormCondition.TargetReportId + "']");
    var targetReportId = P_dicIndividual[RM0001_GlobalListKey.TargetReportId];
    $(tdTargetReportId).text(targetReportId);

    // 帳票コンボボックスの再作成
    var selects = $(P_Article).find("#" + RM0001_FormCondition.Id + getAddFormNo()).find("td[data-name='VAL" + RM0001_FormCondition.OutputReport + "'] select.dynamic");

    //連動ｺﾝﾎﾞの選択ﾘｽﾄを再生成
    resetComboBox(appPath, selects);

    // 描画に間に合っていないので少し待つ
    setTimeout(function () {
        // 帳票コンボの先頭を選択して、設定イベントを行う
        selectComboTop($(selects));
        setTimeout(function () {
            $(selects).trigger("change");
        }, RM0001_FormCondition.WaitTime); //300ミリ秒
    }, RM0001_FormCondition.WaitTime); //300ミリ秒
}

/**
 * 【オーバーライド用関数】Tabulatorの描画が完了時の処理
 * @param {any} tbl 一覧
 * @param {any} id 一覧のID(#,_FormNo付き)
 */
function RM0001_postBuiltTabulator(tbl, id) {

    if (id == "#" + RM0001_FormList.Id + getAddFormNo()) {
        // 検索結果のデータを取得
        var pTable = P_listData[id];
        var pData = pTable.getData();

        $.each(pData, function (i, pRowData) {
            // 項目ID（出力項目マスタ）の有無でチェックをつける
            var value = pRowData["VAL" + RM0001_FormList.OutputItemId];
            if (value != "" && value != 0) {
                pRowData["SELTAG"] = 1;
            }
        });
        pTable.setData(pData);

        if (pData.length > 0) {
            // 検索後に検索結果からテンプレートファイルパス、テンプレートファイル名を一覧から取得して検索条件に保存する対応
            // テンプレートファイルパス
            var templateFilePath = pData[0]["VAL" + RM0001_FormList.TemplateFilePath];
            var tdTemplateFilePath = $(P_Article).find("#" + RM0001_FormCondition.Id + getAddFormNo()).find("td[data-name='VAL" + RM0001_FormCondition.TemplateFilePath + "']");
            $(tdTemplateFilePath).text(templateFilePath);
            // テンプレートファイル名
            var templateFileName = pData[0]["VAL" + RM0001_FormList.TemplateFileName];
            var tdTemplateFileName = $(P_Article).find("#" + RM0001_FormCondition.Id + getAddFormNo()).find("td[data-name='VAL" + RM0001_FormCondition.TemplateFileName + "']");
            $(tdTemplateFileName).text(templateFileName);
        }

        // 一覧に対する制御
        // 対象帳票の出力項目種別が出力パターン指定ありの場合のみ、一覧を触れる制御にする
        if (isOutputItemTypePatternExists() == true) {
            //行追加ボタンを表示する
            setHideButtonTopForList(RM0001_FormList.Id, actionkbn.SelectAll, true);
            setHideButtonTopForList(RM0001_FormList.Id, actionkbn.CancelAll, true);

            //選択列を表示にする
            changeRowControlForRM0001(RM0001_FormList.Id, true);
        }
        else {
            //行追加ボタンを非表示にする
            setHideButtonTopForList(RM0001_FormList.Id, actionkbn.SelectAll, false);
            setHideButtonTopForList(RM0001_FormList.Id, actionkbn.CancelAll, false);

            //選択列を非表示にする
            changeRowControlForRM0001(RM0001_FormList.Id, false);
        }
    }
}
/*
* 一覧の行追加などと選択列の表示/非表示切替を行う（tmqcommon.js参照）
* @param ctrlId 一覧のコントロールID
* @param isDisplay 表示する場合True
*/
function changeRowControlForRM0001(ctrlId, isDisplay) {
    // 行追加など
    var items = [actionkbn.AddNewRow, actionkbn.DeleteRow, actionkbn.SelectAll, actionkbn.CancelAll];
    $.each(items, function (i, item) {
        var target = $("#" + ctrlId + getAddFormNo() + "_div").find('[data-actionkbn=' + item + ']');
        setHide(target, !isDisplay);
    });

    // 選択列の表示制御
    var table = P_listData['#' + ctrlId + getAddFormNo()];
    if (isDisplay) {
        table.showColumn("SELTAG");
    } else {
        table.hideColumn("SELTAG");
    }
    table.redraw();
    table = null;

    // 選択列の表示制御（行選択）
    var table = P_listData['#' + ctrlId + getAddFormNo()];
    if (isDisplay) {
        table.showColumn("moveRowBtn");
    } else {
        table.hideColumn("moveRowBtn");
    }
    table.redraw();
    table = null;
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
function RM0001_beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom, transPtn) {

    if (btnCtrlId == RM0001_FormListBtnGrp.BtnBack)
    {
        // グローバルリストのキーを消す
        operatePdicIndividual(RM0001_GlobalListKey.TargetCtrlId, true, "");         // 呼び元画面の対象のコントロールID
        operatePdicIndividual(RM0001_GlobalListKey.ParentConductId, true, "");      // 呼び元画面の機能ID
        operatePdicIndividual(RM0001_GlobalListKey.ParentPgmId, true, "");          // 呼び元画面のプログラムID
        operatePdicIndividual(RM0001_GlobalListKey.TargetReportId, true, "");       // 呼び元画面の選択帳票ID
        operatePdicIndividual(RM0001_GlobalListKey.TargetOptionCtrlId, true, "");
        operatePdicIndividual(RM0001_GlobalListKey.OptionDataList, true, "");
        operatePdicIndividual(RM0001_GlobalListKey.AuthorityLevel, true, "");
        operatePdicIndividual(RM0001_GlobalListKey.RegistFactoryId, true, "");
        operatePdicIndividual(RM0001_GlobalListKey.RegistPatternId, true, "");
        operatePdicIndividual(RM0001_GlobalListKey.RegistTemplateId, true, "");
    }
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
function RM0001_beforeSearchBtnProcess(appPath, btn, conductId, pgmId, formNo, conductPtn) {
    // 他画面の機能を呼ばないようする為の実装
    // 処理自体は実装なし
}

/**
 *  個別実装ボタン
 *
 *  @param {string} appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {number} formNo      ：画面NO
 *  @param {string} btnCtrlId   ：ボタンCTRLID
 */
function RM0001_clickIndividualImplBtn(appPath, formNo, btnCtrlId) {

    // 表示メッセージのクリア
    clearMessage();

    // ボタンコントロールIDを判定
    if (btnCtrlId == RM0001_FormOutItemList.BtnNew) {              // 一覧画面 新規
        dispFormList(false);                                           //   一覧画面 非表示
        changeDispFormOutPattern(true);                                //   出力パターン登録画面 表示
    }
    else if (btnCtrlId == RM0001_FormOutItemList.BtnUpload) {      // 一覧画面 アップロード
        dispFormList(false);                                           //   一覧画面 非表示
        changeDispFormUploadTemp(true);                                //   雛形ファイル登録画面 表示
        removeSucMessage();                                            //   雛形ファイル登録画面 処理成功メッセージ削除
    }
    else if (btnCtrlId == RM0001_FormOutPattern.BtnCancelOutPattern) { // 出力パターン登録画面 キャンセル
        if (dataEditedFlg) {
            // 画面変更ﾌﾗｸﾞONの場合、ﾃﾞｰﾀ破棄確認ﾒｯｾｰｼﾞをﾎﾟｯﾌﾟｱｯﾌﾟ
            P_MessageStr = P_ComMsgTranslated[941060005]; //『画面の編集内容は破棄されます。よろしいですか？』

            //確認OK時処理を設定
            var eventFunc = function () {
                dataEditedFlg = false;
                dispFormList(true);                                            //   一覧画面 表示
                changeDispFormOutPattern(false);                               //   出力パターン登録画面 非表示
            }
            // 確認ﾒｯｾｰｼﾞを表示
            if (!popupMessage([P_MessageStr], messageType.Confirm, eventFunc)) {
                //『キャンセル』の場合、処理中断
                return false;
            }
        }
        dispFormList(true);                                            //   一覧画面 表示
        changeDispFormOutPattern(false);                               //   出力パターン登録画面 非表示
    }
    else if (btnCtrlId == RM0001_FormUploadTemp.BtnCancelTmpFile) {    // 雛形ファイル登録画面 キャンセル
        if (dataEditedFlg) {
            // 画面変更ﾌﾗｸﾞONの場合、ﾃﾞｰﾀ破棄確認ﾒｯｾｰｼﾞをﾎﾟｯﾌﾟｱｯﾌﾟ
            P_MessageStr = P_ComMsgTranslated[941060005]; //『画面の編集内容は破棄されます。よろしいですか？』

            //確認OK時処理を設定
            var eventFunc = function () {
                dataEditedFlg = false;
                dispFormList(true);                                            //   一覧画面 表示
                initSelectFileElement();                                       //   雛形ファイル登録画面　ファイル選択コントロール初期化
                removeErrMessage();                                            //   雛形ファイル登録画面 エラーメッセージ削除
                changeDispFormUploadTemp(false);                               //   雛形ファイル登録画面 非表示
            }
            // 確認ﾒｯｾｰｼﾞを表示
            if (!popupMessage([P_MessageStr], messageType.Confirm, eventFunc)) {
                //『キャンセル』の場合、処理中断
                return false;
            }
        }

        dispFormList(true);                                            //   一覧画面 表示
        initSelectFileElement();                                       //   雛形ファイル登録画面　ファイル選択コントロール初期化
        removeErrMessage();                                            //   雛形ファイル登録画面 エラーメッセージ削除
        changeDispFormUploadTemp(false);                               //   雛形ファイル登録画面 非表示
    }
}

/**
 * 【オーバーライド用関数】Tabuator一覧の描画前処理
 * @param {string} appPath  ：アプリケーションルートパス
 * @param {string} id       ：一覧のID(#,_FormNo付き)
 * @param {string} options  ： 一覧のオプション情報
 * @param {object} header   ：ヘッダー情報
 * @param {object} dispData ：データ
 */
function RM0001_prevCreateTabulator(appPath, id, options, header, dispData) {

    // 出力仕様項目一覧のレコードの並べ替えを可能にする
    if (id == "#" + RM0001_FormList.Id + getAddFormNo()) {
        //行移動可
        options["movableRows"] = true;
        if (!header.some(x => x.field === "moveRowBtn")) {
            //先頭に移動用のアイコン列追加
            header.unshift({ title: "", rowHandle: true, field: "moveRowBtn", formatter: "handle", headerSort: false, frozen: true, width: 30, minWidth: 30 });
        }
    }

    // 出力パターン登録画面を非表示にする
    changeDispFormOutPattern(false);

    // 雛形ファイル登録画面を非表示にする
    changeDispFormUploadTemp(false);

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
function RM0001_postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data) {

    // エラーが起きた場合は何もしない
    var errorEle = $(P_Article).parent().parent().find(".error");
    if (errorEle.length != 0) {

        // エラーをクリアして表示しなおす
        // エラー情報を表示
        dispErrorDetail(data);
        return;
    }

    // 削除ボタンクリックの場合の個別処理
    if ($(btn).attr("name") == RM0001_FormOutItemList.BtnDelete) {
        var selects = $(P_Article).find("#" + RM0001_FormCondition.Id + getAddFormNo()).find("td[data-name='VAL" + RM0001_FormCondition.OutputPattern + "'] select.dynamic");
        //連動ｺﾝﾎﾞの選択ﾘｽﾄを再生成
        resetComboBox(appPath, selects);

        // 実行中フラグOFF
        P_ProcExecuting = false;

        // クリアボタンのイベント実行
        var backClear = $(P_Article).find("input:button[data-actionkbn='" + actionkbn.Clear + "']");
        $(backClear).click();

        // 実行中フラグON
        P_ProcExecuting = true;

        RM0001_setPageStatusEx(pageStatus.INIT, null, null, null);

        strMessage = getMessageParam(P_ComMsgTranslated[941220001], [P_ComMsgTranslated[911110001]]);
        setMessage(strMessage, procStatus.Valid);

        return;

    }
    // パターン登録、テンプレートアップロード時
    else if ($(btn).attr("name") == RM0001_FormUploadTemp.BtnUploadTmpFile || $(btn).attr("name") == RM0001_FormOutPattern.BtnRegistOutPattern) {

        // 一覧画面を表示する
        dispFormList(true);

        // 出力パターン登録画面を非表示にする
        changeDispFormOutPattern(false);

        // 雛形ファイル登録画面　ファイル選択コントロール初期化
        initSelectFileElement();

        // 雛形ファイル登録画面を非表示にする
        changeDispFormUploadTemp(false);

        // 実行中フラグOFF
        P_ProcExecuting = false;

        // コンボボックスの再作成
        RM0001_setPageStatusEx(pageStatus.INIT, null, null, null);

        // 実行中フラグON
        P_ProcExecuting = true;

        setTimeout(function () {

            // 実行中フラグOFF
            P_ProcExecuting = false;

            // 検索ボタンのイベント実行
            var backRegist = $(P_Article).find("input:button[data-actionkbn='" + actionkbn.Search + "']");
            $(backRegist).click();

            // 実行中フラグON
            P_ProcExecuting = true;

            strMessage = getMessageParam(P_ComMsgTranslated[941220001], [P_ComMsgTranslated[911200003]]);
            setMessage(strMessage, procStatus.Valid);

        }, RM0001_FormCondition.WaitTime * 10); //3000ミリ秒

        return;

    }

    // 一覧画面を表示する
    dispFormList(true);

    // 出力パターン登録画面を非表示にする
    changeDispFormOutPattern(false);

    // 雛形ファイル登録画面　ファイル選択コントロール初期化
    initSelectFileElement();

    // 雛形ファイル登録画面を非表示にする
    changeDispFormUploadTemp(false);

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
function RM0001_passDataCmConduct(appPath, conductId, parentNo, conditionDataList, ctrlId, btn_ctrlId, rowNo, element, parentConductId) {

    // 機能IDが「帳票出力」ではない場合は何もしない
    if (conductId != RM00001_ConductId) {
        return;
    }

    var reportId;
    var programId;
    var factoryId;

    operatePdicIndividual(RM0001_GlobalListKey.ParentConductId, false, parentConductId);        // 呼び元画面の機能ID

    // 遷移元画面が帳票管理の場合
    if (parentConductId == RM0001_ConductIdList.ConductIdRM0002) {

        // 選択した帳票の帳票IDを取得
        $.each($(conditionDataList), function (idx, conditionData) {
            // 先頭行の条件から帳票IDを取得してループを脱出する
            reportId = conditionData['VAL' + RM0002_FormList.ReportId];
            programId = conditionData['VAL' + RM0002_FormList.ProgramId];
            return false;
        });

        // グローバルリストに格納
        operatePdicIndividual(RM0001_GlobalListKey.ParentPgmId, false, programId);            　　  // 出力帳票定義マスタのプログラムID
        operatePdicIndividual(RM0001_GlobalListKey.TargetReportId, false, reportId);            　　// 呼び元画面の対象のプログラムID

        return;
    } else {

        // pgmIdを取得
        var parentPgmId = getArticleInfoByElm(element, "PGMID");
        // グローバルリストに格納
        operatePdicIndividual(RM0001_GlobalListKey.ParentPgmId, false, parentPgmId);           // 呼び元画面の対象のプログラムID

        // 機器台帳用個別設定
        if (parentPgmId == RM0001_ProgramIdList.ProgramIdMC0001) {
            // 一覧の対象コントロールIDの設定
            var listCtrlId = "BODY_020_00_LST_0";
            operatePdicIndividual(RM0001_GlobalListKey.TargetCtrlId, false, listCtrlId);           // 呼び元画面の対象のコントロールID
        //    operatePdicIndividual(RM0001_GlobalListKey.TargetSqlParamName1, false, "MachineId");   // 対象SQLパラメータ項目No１
        }

        // 件名別長期計画用個別設定
        if (parentPgmId == RM0001_ProgramIdList.ProgramIdLN0001) {
            // 一覧の対象コントロールIDの設定
            var listCtrlId = "BODY_040_00_LST_0";
            // 一覧の対象コントロールID（オプション使用）の設定
            var optCtrlId = "BODY_020_00_LST_0";
            // 対象のコントロールIDから一覧要素のデータを取得
            const ctrlIdList = [optCtrlId];
            var optionDataList = getListDataByCtrlIdList(ctrlIdList, parentNo, 0)

            operatePdicIndividual(RM0001_GlobalListKey.TargetCtrlId, false, listCtrlId);           // 呼び元画面の対象のコントロールID
            operatePdicIndividual(RM0001_GlobalListKey.TargetOptionCtrlId, false, optCtrlId);      // 呼び元画面の対象のコントロールID（長期スケジュール用）
            operatePdicIndividual(RM0001_GlobalListKey.OptionDataList, false, optionDataList);
        }

        // 機器別長期計画用個別設定
        if (parentPgmId == RM0001_ProgramIdList.ProgramIdLN0002) {
            // 一覧の対象コントロールIDの設定
            var listCtrlId = "BODY_040_00_LST_0";
            // 一覧の対象コントロールID（オプション使用）の設定
            var optCtrlId = "BODY_020_00_LST_0";
            // 対象のコントロールIDから一覧要素のデータを取得
            const ctrlIdList = [optCtrlId];
            var optionDataList = getListDataByCtrlIdList(ctrlIdList, parentNo, 0)

            operatePdicIndividual(RM0001_GlobalListKey.TargetCtrlId, false, listCtrlId);           // 呼び元画面の対象のコントロールID
            operatePdicIndividual(RM0001_GlobalListKey.TargetOptionCtrlId, false, optCtrlId);      // 呼び元画面の対象のコントロールID（長期スケジュール用）
            operatePdicIndividual(RM0001_GlobalListKey.OptionDataList, false, optionDataList);
        }

        // 保全活動用個別設定
        if (parentPgmId == RM0001_ProgramIdList.ProgramIdMA0001) {
            // 一覧の対象コントロールIDの設定
            var listCtrlId = "BODY_020_00_LST_0";
            operatePdicIndividual(RM0001_GlobalListKey.TargetCtrlId, false, listCtrlId);           // 呼び元画面の対象のコントロールID
        }

        // 予備品管理用個別設定
        if (parentPgmId == RM0001_ProgramIdList.ProgramIdPT0001) {
            // 一覧の対象コントロールIDの設定
            var listCtrlId = "BODY_020_00_LST_0";
            operatePdicIndividual(RM0001_GlobalListKey.TargetCtrlId, false, listCtrlId);           // 呼び元画面の対象のコントロールID
        }

        return;
    }

}

/**
 *【オーバーライド用関数】Excel出力ﾁｪｯｸ処理 - 前処理
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 */
function RM0001_reportCheckPre(appPath, conductId, formNo, btn) {

    // 出力項目が未設定の場合、エラー
    var pTable = P_listData["#" + RM0001_FormList.Id + getAddFormNo()];
    var pData = pTable.getData();
    var errFlg = true;

    $.each(pData, function (i, pRowData) {
        // 選択データがあれば、OKとする
        if (pRowData["SELTAG"] == 1) {
            errFlg = false;
        }
    });

    if (errFlg == true) {
        // 出力仕様項目が選択されていません。
        strMessage = getMessageParam(P_ComMsgTranslated[141060001], [P_ComMsgTranslated[111120087]]);
        setMessage(strMessage, procStatus.Error);
        return false;
    }

    return true;
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
function RM0001_preInputCheckUpload(appPath, conductId, formNo) {

    // 機能IDが「帳票出力」ではない場合は何もしない
    if (getConductId() != RM00001_ConductId) {
        return;
    }

    // ファイル選択コントロールを取得
    var templateFile = getSelectFileElement();
    var file = templateFile[0].files[0];

    // ファイルが選択されているかどうか判定
    if (file == null || file.name == null || file.name.length <= 0) {
        // エラー表示
        $(templateFile).addClass("errorcom");
        addMessage([P_ComMsgTranslated[941220007]], messageType.Warning);
        return [false, true, false];
    }

    return [false, false, false];
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
function RM0001_setComboOtherValues(appPath, combo, datas, selected, formNo, ctrlId, valNo) {
    if (ctrlId == RM0001_FormCondition.Id && valNo == RM0001_FormCondition.OutputReport) {
        changeComboValues(appPath, RM0001_FormCondition.Id, RM0001_FormCondition.OutputReport);
    }
    if (ctrlId == RM0001_FormCondition.Id && valNo == RM0001_FormCondition.Factory) {
        changeComboValues(appPath, RM0001_FormCondition.Id, RM0001_FormCondition.Factory);
    }
    if (ctrlId == RM0001_FormCondition.Id && valNo == RM0001_FormCondition.OutputTemplate) {
        changeComboValues(appPath, RM0001_FormCondition.Id, RM0001_FormCondition.OutputTemplate);
    }
}

/**
 * @status {number}       ：ﾍﾟｰｼﾞ状態　0：初期状態、1：検索後、2：明細表示後ｱｸｼｮﾝ、3：ｱｯﾌﾟﾛｰﾄﾞ後
 * 帳票出力　ボタン制御（初期状態）
 */
function changeFormConditionBtnGrp(status) {

    // 初期表示
    // 検索ボタン以外は非活性
    if (status == pageStatus.INIT) {
        // 検索
        setDispMode(getButtonCtrl(RM0001_FormConditionBtnGrp.BtnSearch), false);
        // クリア
        setDispMode(getButtonCtrl(RM0001_FormConditionBtnGrp.BtnClear), true);
        // 登録
        setDispMode(getButtonCtrl(RM0001_FormOutItemList.BtnRegist), true);
        // 削除
        setDispMode(getButtonCtrl(RM0001_FormOutItemList.BtnDelete), true);
        // 新規
        setDispMode(getButtonCtrl(RM0001_FormOutItemList.BtnNew), true);
        // アップロード
        setDispMode(getButtonCtrl(RM0001_FormOutItemList.BtnUpload), true);
        // ダウンロード
        setDispMode(getButtonCtrl(RM0001_FormOutItemList.BtnDownload), true);
    } else {
        //        // 検索
        //        setDispMode(getButtonCtrl(RM0001_FormConditionBtnGrp.BtnSearch), true);
        //        // クリア
        //        // 検索後（クリアボタンは活性）
        //        setDispMode(getButtonCtrl(RM0001_FormConditionBtnGrp.BtnClear), false);

        //        // アップロード
        //        // ダウンロード
        //        // 管理一覧からの遷移でない（各機能からの遷移）場合
        //        if (isTransFromRM0002() == false) {
        //            setDispMode(getButtonCtrl(RM0001_FormConditionBtnGrp.BtnUpload), true);
        //            setDispMode(getButtonCtrl(RM0001_FormConditionBtnGrp.BtnDownload), true);
        //        } else {
        //            setDispMode(getButtonCtrl(RM0001_FormConditionBtnGrp.BtnUpload), false);
        //            setDispMode(getButtonCtrl(RM0001_FormConditionBtnGrp.BtnDownload), false);
        //        }

        //        // 更新
        //        // 削除
        //        // 新規
        //        // 対象帳票の出力項目種別が出力パターン指定ありの場合
        //        if (isOutputItemTypePatternExists() == true) {
        //            setDispMode(getButtonCtrl(RM0001_FormConditionBtnGrp.BtnRegist), true);
        //            setDispMode(getButtonCtrl(RM0001_FormConditionBtnGrp.BtnDelete), true);
        //            setDispMode(getButtonCtrl(RM0001_FormConditionBtnGrp.BtnNew), true);
        //        } else {
        //            setDispMode(getButtonCtrl(RM0001_FormConditionBtnGrp.BtnRegist), false);
        //            setDispMode(getButtonCtrl(RM0001_FormConditionBtnGrp.BtnDelete), false);
        //            setDispMode(getButtonCtrl(RM0001_FormConditionBtnGrp.BtnNew), false);
        //        }
    }
}

///**
// * @status {number}       ：ﾍﾟｰｼﾞ状態　0：初期状態、1：検索後、2：明細表示後ｱｸｼｮﾝ、3：ｱｯﾌﾟﾛｰﾄﾞ後
// * 帳票出力　ボタン制御（初期状態）
// */
//function changeFormListBtnGrp(status) {

//    // 戻るボタン
//    // 常に表示

//    // 初期表示
//    // 検索ボタン以外は非活性
//    if (status == pageStatus.INIT) {
//        // 何もしない
//    } else {
//        // 出力
//        // 管理一覧からの遷移でない（各機能からの遷移）場合、かつ
//        // Template、または出力パターンが指定されていない場合、出力できない
//        if (isTransFromRM0002() == false && isSelectTemplateOrPattern() == true) {
//            setDispMode(getButtonCtrl(RM0001_FormListBtnGrp.BtnOutput), false);
//        } else {
//            setDispMode(getButtonCtrl(RM0001_FormListBtnGrp.BtnOutput), true);
//        }
//    }
//}

///**
// * 管理一覧からの遷移かどうかを判定
// *
// */
//function isTransFromRM0002()
//{
//    // グローバル変数：遷移元プログラムIDより管理一覧からの遷移かどうかを確認
//    var conductId = P_dicIndividual[RM0001_GlobalListKey.ParentConductId];
//    if (conductId != null && conductId.length > 0 && conductId == RM0001_ConductIdList.ConductIdRM0002) {
//        return true;
//    }
//    return false;
//}

/**
 * 対象帳票の出力項目種別が出力パターン指定ありかどうかを判定
 *
 */
function isOutputItemTypePatternExists() {

    // 対象帳票の出力項目種別が出力パターン指定ありかどうか
    var isChkFlg = false;

    // 検索結果のデータを取得
    var pTable = P_listData["#" + RM0001_FormList.Id + getAddFormNo()];
    if (pTable != null && $(pTable).length > 0) {
        var pData = pTable.getData();

        $.each(pData, function (i, pRowData) {
            // 先頭行の出力項目種別で判断
            var value = pRowData["VAL" + RM0001_FormList.OutputItemType];
            if (value != null && $(value).length > 0 && value == RM0001_FormList.OutputItemTypePatternExists) {
                isChkFlg = true;
            }
            return false;
        });
    } else {
        isChkFlg = true;
    }
    return isChkFlg;
}

///**
// * 対象テンプレート、対象パターンの何れを選択しているかを判定
// *
// */
//function isSelectTemplateOrPattern() {

//    // 対象テンプレートが選択されているか
//    var tdTarget = $(P_Article).find("#" + RM0001_FormCondition.Id + getAddFormNo()).find("td[data-name='VAL" + RM0001_FormCondition.OutputTemplate + "']");
//    var tdText = getCellVal(tdTarget);
//    if (tdText != null && tdText.length > 0)
//    {
//        return true;
//    }

//    // 対象パターンが選択されているか
//    var tdTarget = $(P_Article).find("#" + RM0001_FormCondition.Id + getAddFormNo()).find("td[data-name='VAL" + RM0001_FormCondition.OutputPattern + "']");
//    var tdText = getCellVal(tdTarget);
//    if (tdText != null && tdText.length > 0) {
//        return true;
//    }

//    return false;
//}

/**
 * 一覧画面の表示/非表示を切り替える
 * @param {any} isDisp
 */
function dispFormList(isDisp) {

    if (isDisp) {
        // 表示
        $(P_Article).find("#" + RM0001_FormList.SearchArea).removeClass(RM0001_Hide);
        $(P_Article).find("#" + RM0001_FormList.Id + getAddFormNo() + "_div").removeClass(RM0001_Hide);
        $(P_Article).find("input[name='" + RM0001_FormList.BtnOutput + "']").closest("table").removeClass(RM0001_Hide);

        $(P_Article).find("#" + RM0001_FormOutItemList.Id + getAddFormNo() + "_div").removeClass(RM0001_Hide);
        $(P_Article).find("input[name='" + RM0001_FormOutItemList.BtnRegist + "']").closest("table").removeClass(RM0001_Hide);
        $(P_Article).find("input[name='" + RM0001_FormOutItemList.BtnDelete + "']").closest("table").removeClass(RM0001_Hide);
        $(P_Article).find("input[name='" + RM0001_FormOutItemList.BtnNew + "']").closest("table").removeClass(RM0001_Hide);
        $(P_Article).find("input[name='" + RM0001_FormOutItemList.BtnUpload + "']").closest("table").removeClass(RM0001_Hide);
        $(P_Article).find("input[name='" + RM0001_FormOutItemList.BtnDownload + "']").closest("table").removeClass(RM0001_Hide);
    }
    else {
        // 非表示
        $(P_Article).find("#" + RM0001_FormList.SearchArea).addClass(RM0001_Hide);
        $(P_Article).find("#" + RM0001_FormList.Id + getAddFormNo() + "_div").addClass(RM0001_Hide);
        $(P_Article).find("input[name='" + RM0001_FormList.BtnOutput + "']").closest("table").addClass(RM0001_Hide);

        $(P_Article).find("#" + RM0001_FormOutItemList.Id + getAddFormNo() + "_div").addClass(RM0001_Hide);
        $(P_Article).find("input[name='" + RM0001_FormOutItemList.BtnRegist + "']").closest("table").addClass(RM0001_Hide);
        $(P_Article).find("input[name='" + RM0001_FormOutItemList.BtnDelete + "']").closest("table").addClass(RM0001_Hide);
        $(P_Article).find("input[name='" + RM0001_FormOutItemList.BtnNew + "']").closest("table").addClass(RM0001_Hide);
        $(P_Article).find("input[name='" + RM0001_FormOutItemList.BtnUpload + "']").closest("table").addClass(RM0001_Hide);
        $(P_Article).find("input[name='" + RM0001_FormOutItemList.BtnDownload + "']").closest("table").addClass(RM0001_Hide);
    }
}

/**
 * 出力パターン登録画面の表示/非表示を切り替える
 * @param {any} isDisp 表示する場合はtrue
 */
function changeDispFormOutPattern(isDisp) {

    if (isDisp) {
        // 表示
        $(P_Article).find("#" + RM0001_FormOutPattern.Id + getAddFormNo() + "_div").removeClass(RM0001_Hide);
        $(P_Article).find("input[name='" + RM0001_FormOutPattern.BtnRegistOutPattern + "']").closest("table").removeClass(RM0001_Hide);

    }
    else {
        // 非表示
        $(P_Article).find("#" + RM0001_FormOutPattern.Id + getAddFormNo() + "_div").addClass(RM0001_Hide);
        $(P_Article).find("input[name='" + RM0001_FormOutPattern.BtnRegistOutPattern + "']").closest("table").addClass(RM0001_Hide);

        // テンプレート名の要素取得
        var patternName = getCtrl(RM0001_FormOutPattern.Id, RM0001_FormOutPattern.PatternName, 1, CtrlFlag.TextBox, false, false);
        $(patternName).val("");
    }
}

/**
 * 雛形ファイル登録画面の表示/非表示を切り替える
 * @param {any} isDisp 表示する場合はtrue
 */
function changeDispFormUploadTemp(isDisp) {

    if (isDisp) {
        // 表示
        $(P_Article).find("#" + RM0001_FormUploadTemp.Id + getAddFormNo() + "_div").removeClass(RM0001_Hide);
        $(P_Article).find("input[name='" + RM0001_FormUploadTemp.BtnUploadTmpFile + "']").closest("table").removeClass(RM0001_Hide);
    }
    else {
        // 画面上部のエラーメッセージの要素を削除

        // 非表示
        $(P_Article).find("#" + RM0001_FormUploadTemp.Id + getAddFormNo() + "_div").addClass(RM0001_Hide);
        $(P_Article).find("input[name='" + RM0001_FormUploadTemp.BtnUploadTmpFile + "']").closest("table").addClass(RM0001_Hide);

        // テンプレート名の要素取得
        var templateName = getCtrl(RM0001_FormUploadTemp.Id, RM0001_FormUploadTemp.TempName, 1, CtrlFlag.TextBox, false, false);
        $(templateName).val("");
    }
}

/**
 * 雛形ファイル登録画面のファイル選択コントロールの要素を取得
 */
function getSelectFileElement() {
    return $(P_Article).find("#" + RM0001_FormUploadTemp.Id + getAddFormNo()).find("td[data-name='VAL" + RM0001_FormUploadTemp.File + "'] input[type='file']");
}

/**
 * 雛形ファイル登録画面の入力コントロールを初期状態に戻す
 */
function initSelectFileElement() {

    // ファイル選択コントロールの要素取得
    var file = getSelectFileElement();
    // ファイル選択クリア
    file.val('');
    // エラ―クラス削除
    file.removeClass("errorcom")

    // テンプレート名の要素取得
    //var tmpFileName = getCtrl(RM0001_FormUploadTemp.Id, RM0001_FormUploadTemp.TempName, 1, CtrlFlag.TextBox, false, false);

}

/**
 * 画面上部のエラーメッセージの要素を削除
 */
function removeErrMessage() {

    var ele = $(P_Article).parent().parent().find(".error");

    if (ele.length != 0) {
        ele.remove();
    }
}

/**
 * 画面上部の処理成功メッセージの要素を削除
 */
function removeSucMessage() {

    var ele = $(P_Article).parent().parent().find(".success");

    if (ele.length != 0) {
        ele.remove();
    }
}

