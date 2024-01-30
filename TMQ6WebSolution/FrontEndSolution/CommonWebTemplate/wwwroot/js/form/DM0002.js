/* ========================================================================
 *  機能名　    ：   【DM0002】共通 - 文書管理 詳細画面
 * ======================================================================== */
// 共通画面なので呼出元で呼び出しているであろう他JavaScriptの呼び出しは不要、定義が重複してエラーとなる
// この画面を呼び出す場合は以下を呼出元画面で呼び出してください。
//document.write("<script src=\"/js/form/tmqcommon.js\"></script>");

// 一覧画面 コントロール項目番号
const DM0001_FormList = {
    ConductId: "DM0001",                // 機能ID
    No: 0,                              // 画面番号
    FilterId: "BODY_010_00_LST_0",      // フィルタ機能一覧ID
    Id: "BODY_020_00_LST_0",            // 検索結果一覧
    ModalId: "BODY_020_00_LST_0_0_edit",// 単票モーダル
    Filter: 1,                          // フィルタ
    DocumentHide: 4,                    // 文書種類(詳細検索用、非表示)
    Attachment: 19,                     // 添付種類
    Document: 5,                        // 文書種類
    File: 21,                           // ファイル選択コントロール
    Link: 22,                           // リンク
    ButtonOutputId: "Output",           // 出力ボタン
    Div: "BODY_020_00_LST_0_0_div",     // 一覧タイトル用
    FileName: 20,                       // 登録済ファイル名
    FunctionTypeId: 1,                  // 機能タイプID
    KeyId: 25,                          // キーID
    AttachmentExData: 30                // 添付種類拡張項目
};

// 詳細画面-添付情報一覧 コントロール項目番号
const DM0002_FormDetail = {
    ConductId: "DM0002",                 // 機能ID
    No: 0,                               // 画面番号
    Id: "CBODY_010_00_LST_0_DM0002",     // 検索結果一覧
    ModalId: "CBODY_010_00_LST_0_DM0002_0_edit",// 単票モーダル
    Attachment: 20,                      // 添付種類
    File: 22,                            // ファイル選択コントロール
    Link: 23,                            // リンク
    FileName: 21,                        // 登録済ファイル名
    AttachmentExData: 30,                // 添付種類拡張項目
    BtnUploadId: "Upload"                // 登録ボタン(アップロード)
};

// 詳細画面-件名情報 コントロール項目番号
const DM0002_Subject = {
    Id: "CBODY_000_00_LST_0_DM0002",     // 検索結果一覧
    FunctionTypeId: 2,                   // 機能タイプID
    DocumentTypeValNo: 5,                // 文書種類コンボボックスの項目番号
    DispYearFrom: 6,                     // 表示年度(From) ※予備品詳細画面に戻った際に使用するための値を保持する場所
    DispYearTo: 7                        // 表示年度(To) ※予備品詳細画面に戻った際に使用するための値を保持する場所
};

// 文書種類コンボボックスの項目番号
const DM0002_documentTypeValNo =
{
    Count: 13,
    Machine: 1,                   // 機器台帳-機番添付
    Equipment: 2,                 // 機器台帳-機器添付
    Content: 3,                   // 機器台帳-保全項目一覧-ファイル添付
    MpInfo: 4,                    // 機器台帳-MP情報タブ-ファイル添付
    LongPlan: 5,                  // 件名別長期計画-件名添付
    Summary: 6,                   // 保全活動-件名添付
    HistoryFailureDiagram: 7,     // 保全活動-故障分析情報タブ-略図添付
    HistoryFailureAnalyze: 8,     // 保全活動-故障分析情報タブ-故障原因分析書添付
    HistoryFailureFactDiagram: 9, // 保全活動-故障分析情報(個別工場)タブ-略図添付
    HistoryFailureFactAnalyze: 10,// 保全活動-故障分析情報(個別工場)タブ-故障原因分析書添付
    SpareImage: 11,               // 予備品管理-画像添付
    SpareDocument: 12,            // 予備品管理-文書添付
    SpareMap: 13                  // 予備品管理-予備品地図
};

// 添付種類コンボボックスアイテムのID
const AttachmentTypeId =
{
    File: "1", // ファイル
    Link: "2"  // リンク
};

/*
 *  初期化処理(表示中画面用)
 *  @param appPath   　　 ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param conductId 　　 ：機能ID
 *  @param formNo 　　　　：画面番号
 *  @param articleForm    ：表示画面ｴﾘｱ要素
 *  @param curPageStatus  ：画面表示ｽﾃｰﾀｽ
 *  @param actionCtrlId   :Action(ﾎﾞﾀﾝなど)CTRLID
 *  @param data           :初期表示ﾃﾞｰﾀ
 */
function DM0002_initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data) {
    if (conductId != DM0002_FormDetail.ConductId) {
        // 文書管理詳細画面でない場合は終了
        return;
    }

    // 文書管理詳細画面の場合
    // 初期化処理
    DM0002_init();
}

/*
 *  遷移処理の後
 *  @param appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param transPtn    ：画面遷移ﾊﾟﾀｰﾝ(1：子画面、2：単票入力、4：共通機能、5：他機能別ﾀﾌﾞ、6：他機能表示切替)
 *  @param transDiv    ：画面遷移アクション区分(1：新規、2：修正、3：複製、4：出力、)
 *  @param transTarget ：遷移先(子画面NO / 単票一覧ctrlId / 共通機能ID / 他機能ID|画面NO)
 *  @param dispPtn     ：遷移表示ﾊﾟﾀｰﾝ(1：表示切替、2：ﾎﾟｯﾌﾟｱｯﾌﾟ、3：一覧直下)
 *  @param formNo      ：遷移元formNo
 *  @param ctrlId      ：遷移元の一覧ctrlid
 *  @param btn_ctrlId  ：ボタンのbtn_ctrlid
 *  @param rowNo       ：遷移元の一覧の選択行番号（新規の場合は-1）
 *  @param element     ：ｲﾍﾞﾝﾄ発生要素
 */
function DM0002_postTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element) {

    // 単票以外または文書管理以外の機能の場合は何もしない
    if (transPtn != transPtnDef.Edit || !DM0002_JudgeConductId()) {
        return;
    }

    if (transTarget != DM0002_FormDetail.Id) {
        // 文書管理詳細画面以外の場合終了
        return;
    }

    // 文書管理詳細画面の場合

    // 行追加アイコン(+アイコン)が押された場合、添付種類の拡張項目に「ファイル」を設定する
    if (rowNo == -1) {
        setValue(DM0002_FormDetail.Id, DM0002_FormDetail.AttachmentExData, 1, CtrlFlag.Label, AttachmentTypeId.File, true, false);

        // 添付種類コンボボックスを「ファイル」を選択状態にする
        var attachmentComb = $(P_Article).find('select[name="' + DM0002_FormDetail.Id + getAddFormNo() + 'VAL' + DM0002_FormDetail.Attachment + '"]').children().not('span');
        if ($(attachmentComb).length) {
            $.each($(attachmentComb), function (idx, item) {
                if (item.attributes[1].value == AttachmentTypeId.File) {

                    $(attachmentComb).parent()[0].value = attachmentComb[idx].attributes[0].value;
                }
            });
        }
    }

    // 表示されている文書種類コンボボックスの項目番号を取得
    var document = $("#formDetail_" + DM0002_FormDetail.ConductId).find("#" + DM0002_Subject.Id + getAddFormNo()).find("td[data-name='VAL" + DM0002_Subject.DocumentTypeValNo + "']")[0].innerText;
    // モーダル画面の文書種類コンボボックスにフォーカスをセット
    setFocusDelay(DM0002_FormDetail.Id, document, 0, CtrlFlag.Combo, true);
    // ファイル列・リンク列の表示/非表示 切替処理
    DM0002_changeDisp(DM0002_FormDetail.ModalId, DM0002_FormDetail.FileName, DM0002_FormDetail.File, DM0002_FormDetail.Link, DM0002_FormDetail.AttachmentExData, DM0002_FormDetail.Id, formNo, DM0002_FormDetail.ConductId);
}

/**
 *【オーバーライド用関数】
 *  行削除の前
 *  @param appPath    ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param btn        ：行削除ﾘﾝｸのa要素
 *  @param id         ：行削除ﾘﾝｸの対象一覧のCTRLID
 *  @param checkes    ：削除対象の要素リスト
 */
function DM0002_preDeleteRow(appPath, btn, id, checkes) {
    // 共通処理により非表示の削除ボタンを押下
    // ※当該一覧でない場合は何も行わずTrueを返す、行削除時は隠しの削除ボタンをクリックしてFalseを返す
    return preDeleteRowCommon(id, [DM0002_FormDetail.Id]);
}

/**
 *【オーバーライド用関数】
 *  遷移処理の前
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
 */
function DM0002_prevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element) {
    var conditionDataList = [];
    // 文書管理詳細画面でない場合は処理を行わない
    if (IsExecDM0002_PrevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element)) {
        // 文書管理詳細画面の検索条件を取得
        conditionDataList = DM0002_addSearchConditionDictionary(formNo, false);
    }

    return [true, conditionDataList];
}

/**
 * 文書管理詳細画面の遷移前処理を行うかどうかの判定
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
function IsExecDM0002_PrevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element) {
    var result = transTarget == DM0002_FormDetail.Id;
    return result;
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
function DM0002_postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data) {
    if (conductId != DM0002_FormDetail.ConductId) {
        // 文書管理詳細画面でない場合は処理を行わない
        return;
    }

    // 文書管理詳細画面の場合
    // 初期化
    DM0002_init();
}

/**
 *【オーバーライド用関数】登録前追加条件取得処理
 *  @param appPath       ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param conductId     ：機能ID
 *  @param formNo        ：画面番号
 *  @param btn           ：押下されたボタン要素
 */
function DM0002_addSearchConditionDictionaryForRegist(appPath, conductId, formNo, btn) {

    var conditionDataList = [];

    if (IsExecDM0002_AddSearchConditionDictionaryForRegist(appPath, conductId, formNo, btn)) {
        // 文書管理詳細画面の場合
        conditionDataList = DM0002_addSearchConditionDictionary(formNo, true);
    }

    return conditionDataList;
}

/**
 * 文書管理詳細画面の登録前追加条件取得処理を行うかどうかの判定
 *  @param appPath       ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param conductId     ：機能ID
 *  @param formNo        ：画面番号
 *  @param btn           ：押下されたボタン要素
 *  @returns {bool} Trueの場合はDM0002_addSearchConditionDictionaryForRegistの処理を行う、Falseの場合は他の画面の追加条件取得処理
 */
function IsExecDM0002_AddSearchConditionDictionaryForRegist(appPath, conductId, formNo, btn) {
    var result = conductId == DM0002_FormDetail.ConductId;
    return result;
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
function DM0002_setComboOtherValues(appPath, combo, datas, selected, formNo, ctrlId, valNo) {

    // 文書管理以外の機能の場合は何もしない
    if (!DM0002_JudgeConductId()) {
        return;
    }

    // 単票の添付種類コンボボックスのアイテムが変更された時に変更されたアイテムに応じてコントロールの表示/非表示を切り替える
    if (ctrlId == DM0002_FormDetail.Id && valNo == DM0002_FormDetail.Attachment) {

        // 添付種類コンボボックスの選択されているアイテムの拡張項目を設定する
        setComboExValue(DM0002_FormDetail.Id, DM0002_FormDetail.AttachmentExData, CtrlFlag.Label, selected, true);

        // 文書管理詳細画面
        DM0002_changeDisp(DM0002_FormDetail.ModalId, DM0002_FormDetail.FileName, DM0002_FormDetail.File, DM0002_FormDetail.Link, DM0002_FormDetail.AttachmentExData, DM0002_FormDetail.Id, DM0002_FormDetail.No, DM0002_FormDetail.ConductId);
    }
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
 */
function DM0002_beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom) {
    if (backFrom == DM0002_ConductId) {
        // 共通画面を閉じた場合、指定した画面ならば再検索を行う
        InitFormDataByCommonModal(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom);
    }

    // 単票画面を閉じた場合
    // 単票で、「登録」ボタンを閉じたときに再検索を行う
    InitFormDataByOwnModal(DM0002_FormDetail.ConductId, DM0002_FormDetail.No, DM0002_FormDetail.Id, DM0002_FormDetail.BtnUploadId, appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom);
}

/**
 *【オーバーライド用関数】取込処理個別入力チェック
 *  @param appPath       ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param conductId     ：機能ID
 *  @param formNo        ：画面番号
 */
function DM0002_preInputCheckUpload(appPath, conductId, formNo) {
    // 画面
    var form;

    if (conductId == DM0001_FormList.ConductId) {
        // 一覧画面
        form = DM0001_FormList;
    }
    else if (conductId == DM0002_FormDetail.ConductId) {
        // 詳細画面
        form = DM0002_FormDetail;
    }
    else {
        return [false, false, true];
    }

    //添付種類が「ファイル」でなければ何もしない
    if (getValue(form.Id, form.AttachmentExData, 1, CtrlFlag.Label, true) != AttachmentTypeId.File) {
        return [false, false, true];
    }

    // 新規行なければ何もしない
    if (!DM0002_isNewData(conductId)) {
        return [false, false, true];
    }

    // 入力チェックエラーフラグ
    var isError = false;

    //ファイル選択コントロールの要素取得
    var selectFile = $(P_Article).find("#" + form.ModalId).find("td[data-name='VAL" + form.File + "'] input[type='file']");
    var file = selectFile[0].files[0];
    if (file == null || file.name == null || file.name.length <= 0) {
        // エラー表示
        $(selectFile).addClass("errorcom");
        addMessage([P_ComMsgTranslated[941220007]], messageType.Warning);
        isError = true;
    }

    return [false, isError, true];
}

/*
 * 検索条件を取得
 * @param formNo        ：画面番号
 * @param isEdit        ：モーダルの場合はtrue
 */
function DM0002_addSearchConditionDictionary(formNo, isEdit) {

    // 件名情報の値を取得
    var targetElements = [];
    targetElements.push($("#formDetail_" + DM0002_FormDetail.ConductId).find("#" + DM0002_Subject.Id + getAddFormNo())[0]);
    return getListDataElements(targetElements, formNo, 0);
}

/*
 * 初期処理
 */
function DM0002_init() {

    // 引数で与えられたミリ秒後、フォーカスをセット
    // 共通画面では、すぐに画面が表示されないため、表示されるまで待ってセットする
    setTimeout(function () {
        // 行追加ボタン(+アイコン)にフォーカスを設定
        $(P_Article).find("a[data-actionkbn=1221]").first().focus();
    }, 300); //300ミリ秒

}

/*
 * 文書管理コンボボックス表示切替
 */
function DM0002_hideAttachmentColumn() {
    // テーブル取得
    var table = P_listData['#' + DM0002_FormDetail.Id + getAddFormNo()];

    // 「文書種類」をすべて非表示にする
    for (var i = 1; i <= DM0002_documentTypeValNo.Count; i++) {

        // 添付情報一覧の「文書種類」を非表示
        table.hideColumn("VAL" + i);

        // 単票の「文書種類」を非表示
        changeDispColumn(DM0002_FormDetail.ModalId, i, false);
    }

    // 機能タイプID(構成グループID)取得
    var functionTypeId = parseInt($(P_Article).find("#" + DM0002_Subject.Id + getAddFormNo()).find("td[data-name= 'VAL" + DM0002_Subject.FunctionTypeId + "']")[0].innerText);
    // 文書種類コンボボックスの項目番号
    var documentType = "";

    // 添付元を判定
    switch (functionTypeId) {
        case AttachmentStructureGroupID.Machine:                        // 機器台帳 機番添付
            documentType = DM0002_documentTypeValNo.Machine;
            break;
        case AttachmentStructureGroupID.Equipment:                      // 機器台帳 機器添付
            documentType = DM0002_documentTypeValNo.Equipment;
            break;
        case AttachmentStructureGroupID.Content:                        // 機器台帳 保全項目一覧 ファイル添付
            documentType = DM0002_documentTypeValNo.Content;
            break;
        case AttachmentStructureGroupID.MpInfo:                         // 機器台帳 MP情報タブ ファイル添付
            documentType = DM0002_documentTypeValNo.MpInfo;
            break;
        case AttachmentStructureGroupID.LongPlan:                       // 件名別長期計画 件名添付
            documentType = DM0002_documentTypeValNo.LongPlan;
            break;
        case AttachmentStructureGroupID.Summary:                        // 保全活動 件名添付
            documentType = DM0002_documentTypeValNo.Summary;
            break;
        case AttachmentStructureGroupID.HistoryFailureDiagram:          // 保全活動 故障分析情報タブ 略図添付
            documentType = DM0002_documentTypeValNo.HistoryFailureDiagram;
            break;
        case AttachmentStructureGroupID.HistoryFailureAnalyze:          // 保全活動 故障分析情報タブ 故障原因分析書添付
            documentType = DM0002_documentTypeValNo.HistoryFailureAnalyze;
            break;
        case AttachmentStructureGroupID.HistoryFailureFactDiagram:      // 保全活動 故障分析情報(個別工場)タブ 略図添付
            documentType = DM0002_documentTypeValNo.HistoryFailureFactDiagram;
            break;
        case AttachmentStructureGroupID.HistoryFailureFactAnalyze:      // 保全活動 故障分析情報(個別工場)タブ 故障原因分析書添付
            documentType = DM0002_documentTypeValNo.HistoryFailureFactAnalyze;
            break;
        case AttachmentStructureGroupID.SpareImage:                     // 予備品管理 画像添付
            documentType = DM0002_documentTypeValNo.SpareImage;
            break;
        case AttachmentStructureGroupID.SpareDocument:                  // 予備品管理 文書添付
            documentType = DM0002_documentTypeValNo.SpareDocument;
            break;
        case AttachmentStructureGroupID.SpareMap:                       // 予備品管理 予備品地図
            documentType = DM0002_documentTypeValNo.SpareMap;
            break;
        default:
            break;
    }

    // 添付元に応じて添付情報一覧と単票の「文書種類」を表示する
    changeTabulatorColumnDisplay(DM0002_FormDetail.Id, documentType, true);
    changeDispColumn(DM0002_FormDetail.ModalId, documentType, true);
}

/*
 * ファイル列・リンク列の表示/非表示 切替処理
 * @param ctrlId          :一覧のコントロールID
 * @param fileValNo       :登録済ファイル名の項目番号
 * @param selectFileValNo :ファイル選択コントロールの項目番号
 * @param linkValNo       :リンク入力コントロールの項目番号
 * @param attachmentValNo :添付種類拡張項目コンボボックスの項目番号
 * @param listId          :検索結果一覧のコントロールID
 * @param formNo          :画面番号
 * @param conductId       :機能ID
 */
function DM0002_changeDisp(ctrlId, fileValNo, selectFileValNo, linkValNo, attachmentValNo, ListId, formNo, conductId) {

    // 添付種類コンボボックスの選択されているアイテム取得
    var attachmentTypeId = getValue(ListId, attachmentValNo, 1, CtrlFlag.Label, true);

    // 添付種類コンボボックスの選択に応じて表示/非表示の切替
    if (attachmentTypeId == AttachmentTypeId.File) {

        // ファイルが選択されている場合
        changeDispColumn(ctrlId, fileValNo, true);       // ファイル名を表示
        changeDispColumn(ctrlId, selectFileValNo, true); // ファイル選択コントロールを表示
        changeDispColumn(ctrlId, linkValNo, false);      // リンクを非表示 

        // 行追加ボタン(+アイコン)によって開かれたモーダルの場合
        if (DM0002_isNewData(conductId)) {

            // 登録済ファイルは非表示
            changeDispColumn(ctrlId, fileValNo, false);// ファイル名を非表示

            // ファイル選択コントロールのヘッダー名を「ファイル登録」に変更する
            DM0002_setHeaderName(conductId);
        }
    }
    else {

        // リンクが選択されている場合
        changeDispColumn(ctrlId, fileValNo, false);      // ファイル名を非表示
        changeDispColumn(ctrlId, selectFileValNo, false);// ファイル選択コントロールを非表示
        changeDispColumn(ctrlId, linkValNo, true);       // リンクを表示
    }
}

/**
 *  新規登録か更新か判定
 *  @param conductId        ：機能ID
 */
function DM0002_isNewData(conductId) {

    // 画面
    var form;

    if (conductId == DM0001_FormList.ConductId) {
        // 一覧画面
        form = DM0001_FormList;
    }
    else if (conductId == DM0002_FormDetail.ConductId) {
        // 詳細画面
        form = DM0002_FormDetail;
    }
    else {
        return;
    }

    //登録済ファイル名を取得
    var fileName = getValue(form.Id, form.FileName, 1, CtrlFlag.Label, true);

    if (fileName.trim() == "") {
        // ファイルが無い場合新規扱い
        return true;
    }
    else {
        // ファイルがある場合は更新
        return false;
    }
}

/**
 *  ファイル選択コントロールのヘッダーの名称を変更
 *  @param conductId        ：機能ID
 */
function DM0002_setHeaderName(conductId) {

    // 画面
    var form;

    if (conductId == DM0001_FormList.ConductId) {
        // 一覧画面
        form = DM0001_FormList;
    }
    else if (conductId == DM0002_FormDetail.ConductId) {
        // 詳細画面
        form = DM0002_FormDetail;
    }
    else {
        return;
    }

    // ヘッダーの文言を「ファイル登録」に変更
    setValue(form.Id, form.File, 1, CtrlFlag.Label, P_ComMsgTranslated[111280040], true, true);
}

/*
 * 機能IDを判定
 */
function DM0002_JudgeConductId() {
    // 機能IDを取得
    var conductId = $(P_Article).find('input[name="CONDUCTID"]').val();

    // 文書管理かつ文書管理(共通機能)の機能IDでなければfalse
    if (conductId != DM0001_FormList.ConductId && conductId != DM0002_FormDetail.ConductId) {
        return false;
    }
    else {
        return true;
    }
}

/**
 * 【オーバーライド用関数】Tabulatorの描画が完了時の処理
 * @param {any} tbl 一覧
 * @param {any} id 一覧のID(#,_FormNo付き)
 */
function DM0002_postBuitTabulator(tbl, id) {

    if (getConductId() == DM0002_ConductId && id == "#" + DM0002_FormDetail.Id + getAddFormNo()) {

        // 「文書種類」の表示/非表示切替
        DM0002_hideAttachmentColumn();

        // テーブルの再描画
        tbl.redraw(true);
    }
}

/**
 *【オーバーライド用関数】
 *  閉じる処理の後(ポップアップ画面用)
 */
function DM0002_postBackBtnProcessForPopup(conductId) {

    // 文書管理画面の機能IDでない場合は何もせずに終了
    if (conductId != DM0002_FormDetail.ConductId) {
        return;
    }

    var val = null;

    // 表示年度(From)がグローバル変数に格納されている場合は一度削除する
    if (P_dicIndividual[DispYearKeyName.YearFrom]) {
        delete P_dicIndividual[DispYearKeyName.YearFrom];
    }

    // 表示年度(From)の値を取得
    val = getValue(DM0002_Subject.Id, DM0002_Subject.DispYearFrom, 1, CtrlFlag.Label, false, false).trim();

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
    val = getValue(DM0002_Subject.Id, DM0002_Subject.DispYearTo, 1, CtrlFlag.Label, false, false).trim();

    if (!val) {
        // 入力されていない場合はSQLで扱うことのできる年の最大値を設定
        val = SqlYear.MaxYear;
    }

    // グローバル変数に格納
    P_dicIndividual[DispYearKeyName.YearTo] = val;
}