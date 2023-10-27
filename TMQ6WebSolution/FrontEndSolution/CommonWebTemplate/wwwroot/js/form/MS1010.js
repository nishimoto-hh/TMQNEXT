/* ========================================================================
 *  機能名　    ：   【MS1010】職種・機種マスタ
 * ======================================================================== */

///**
// * 自身の相対パスを取得
// */
//function getPath() {
//    var root;
//    var scripts = document.getElementsByTagName("script");
//    var i = scripts.length;
//    while (i--) {
//        var match = scripts[i].src.match(/(^|.*\/)MS1010\.js$/);
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
const ConductId_MS1010 = "MS1010";

// 一覧画面
const MS1010_FormList = {
    // 画面No
    No: 1,
    // 検索条件一覧
    SearchList: {
        Id: "BODY_000_00_LST_0",
        // 工場列
        Factory: 1,
        // 職種列
        Job: 2,
        // 機種大分類列
        LargeClassfication: 3,
        // 機種中分類列
        MiddleClassfication: 4,
        // コンボ生成用工場ID列(非表示列)
        HiddenFactoryId: 6,
        // コンボ生成用職種ID列(非表示列)
        HiddenJobId: 7,
        // コンボ生成用機種大分類ID列(非表示列)
        HiddenLargeClassficationId: 8,
        // コンボ生成用機種中分類ID列(非表示列)
        HiddenMiddleClassficationId: 9,
    },
    // 標準アイテム一覧
    StandardItemList: {
        Id: "BODY_020_00_LST_0",
    },
    // 工場アイテム一覧
    FactoryItemList: {
        Id: "BODY_030_00_LST_0",
    },
    // 非表示情報一覧
    HiddenList: {
        Id: "BODY_050_00_LST_0",
        // システム管理者フラグ列
        SystemAdminFlg: 1,
        // 言語ID列
        LanguageId: 2,
        // 構成グループID列
        StructureGroupId: 3,
        // 工場ID列
        FactoryId: 4,
        // 画面タイプ列
        FormType: 5,
        // アイテム一覧タイプ列
        ItemListType: 6,
        // 対象アイテム一覧列
        TargetItemList: 7,
        // 階層列
        StructureLayerNo: 8,
        // 親構成ID列
        ParentStructureId: 9,
        // 本務工場構成ID列
        DutyFactoryId: 10
    },
    // アイテム一覧
    ItemList: {
        // アイテムID列
        ItemId: 5,
        // アイテム翻訳列
        ItemName: 6,
        // 拡張項目1(保全実績集計職種コード)
        ItemEx1: 11,
        // 表示順列
        Order: 21,
        // 削除列
        Delete: 22,
        // 構成ID列
        StructureId: 23,
        // 工場ID列
        FactoryId: 24,
        // 構成グループID列
        StructureGroupId: 25,
        // アイテム翻訳ID列
        ItemTranId: 26,
    },
    ButtonId: {
        // 検索
        Search: "Search",
        // 表示順変更
        Order: "Order",
        // 削除
        Delete: "Delete",
    }
}

// 登録・修正画面
const MS1010_FormEdit = {
    // 画面No
    No: 2,
    // アイテムID一覧
    ItemIdList: {
        Id: "BODY_000_00_LST_1",
    },
    // アイテム翻訳一覧
    ItemTranList: {
        Id: "BODY_010_00_LST_1",
        // アイテム翻訳列
        ItemName: 2,
    },
    // アイテム情報一覧
    ItemList: {
        Id: "BODY_020_00_LST_1",
        // 登録画面
        Regist: {
            // 非表示列
            HideCol: { ItemId: 5, ItemName: 6, Order: 21, Delete: 22 },
        },
        // 修正画面
        Edit: {
            // 非表示列
            HideCol: { ItemId: 5, ItemName: 6, Order: 21 },
        },
    },
    ButtonId: {
        // 登録
        Regist: "Regist",
        // キャンセル
        Cancel: "Back",
    },
}

// 表示順変更画面
const MS1010_FormOrder = {
    // 画面No
    No: 3,
    // アイテム一覧
    ItemList: {
        Id: "BODY_000_00_LST_2",
        // 非表示列
        HideCol: { Standard: 1, Unused: 4 },
    },
    // ボタン一覧
    ButtonList: {
        Id: "BODY_010_00_BTN_2",
    },
    ButtonId: {
        // 登録
        Regist: "Regist",
        // キャンセル
        Cancel: "Back",
    },
}

// 拡張項目列
const ItemExCol_MS1010 = { ExData1: 11 }
// 拡張項目コントロール列(TextBox: 0, Label: 1, Combo: 2, ChkBox: 3, Link: 4, Input: 5, Textarea: 6, Password: 7, Button: 8)
const ItemExCtrlCol_MS1010 = { ExData1: 2 }

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
function initFormOriginalForMS1010(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data) {
    if (formNo == MS1010_FormList.No) {
        // 一覧画面

        // 標準アイテム一覧
        var detailArea = $(P_Article).find("#" + P_formDetailId);
        var standardList = $(detailArea).find(".detail_div").find("#" + MS1010_FormList.StandardItemList.Id + getAddFormNo() + "_div");
        // 標準アイテムを非表示
        //changeListDisplay(MS1010_FormList.StandardItemList.Id, false, false);
        $(standardList).css('display', 'none');
        // 非表示情報を非表示
        changeListDisplay(MS1010_FormList.HiddenList.Id, false, false);

        // フォーカス設定
        setFocusDelay(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.Factory, 0, CtrlFlag.Combo);

        // 初期検索時のみ初期値セット
        if (actionCtrlId == "") {
            // 本務工場を初期値にセット
            var factory = getValue(MS1010_FormList.HiddenList.Id, MS1010_FormList.HiddenList.DutyFactoryId, 1, CtrlFlag.Label, false, false);
            setValueAndTriggerNoChange(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.Factory, 1, CtrlFlag.Combo, factory);

        } else if (actionCtrlId == MS1010_FormEdit.ButtonId.Regist) {
            // 登録処理後

            // 検索条件のコンボを再設定

            // 退避していた検索条件を取得
            var factoryId = getValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenFactoryId, 1, CtrlFlag.Label);
           var jobId = getValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenJobId, 1, CtrlFlag.Label);
            var largeClassficationId = getValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenLargeClassficationId, 1, CtrlFlag.Label);
            var middleClassficationId = getValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenMiddleClassficationId, 1, CtrlFlag.Label);

            // 工場コンボの値セット＆changeイベント発生
            setValueAndTriggerNoChange(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.Factory, 1, CtrlFlag.Combo, factoryId);
            var cnt = 1;
            const waitSeconds = 500;
            if (jobId) {
                // 職種が選択されている場合、退避した検索条件をコンボにセット
                // 職種コンボの値セット＆changeイベント発生
                setTimeout(setValueAndTriggerNoChange, waitSeconds * cnt++, MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.Job, 1, CtrlFlag.Combo, jobId);
            }
            if (largeClassficationId) {
                // 機種大分類が選択されている場合、退避した検索条件をコンボにセット
                // 機種大分類コンボの値セット＆changeイベント発生
                setTimeout(setValueAndTriggerNoChange, waitSeconds * cnt++, MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.LargeClassfication, 1, CtrlFlag.Combo, largeClassficationId);
            }
            if (middleClassficationId) {
                // 機種中分類が選択されている場合、退避した検索条件をコンボにセット
                // 機種中分類コンボの値セット＆changeイベント発生
                setTimeout(setValueAndTriggerNoChange, waitSeconds * cnt++, MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.MiddleClassfication, 1, CtrlFlag.Combo, middleClassficationId);
            }
        }

    } else if (formNo == MS1010_FormEdit.No) {
        // 登録・修正画面

        var title;

        // 階層取得
        var structureLayerNo = getValueByOtherForm(MS1010_FormList.No, MS1010_FormList.HiddenList.Id, MS1010_FormList.HiddenList.StructureLayerNo, 1, CtrlFlag.Label);
        // 階層名称取得
        var title = getLocationNameForMS1010(structureLayerNo);

        // 画面タイプ取得
        var formType = getValueByOtherForm(MS1010_FormList.No, MS1010_FormList.HiddenList.Id, MS1010_FormList.HiddenList.FormType, 1, CtrlFlag.Label);
        if (Number(formType) == MasterFormType.Regist) {
            // 登録画面の初期処理
            formRegistInitForMS1010(title, structureLayerNo);
        } else if (Number(formType) == MasterFormType.Edit) {
            // 修正画面の初期処理
            formEditInitForMS1010(title, structureLayerNo);
        }

    } else if (formNo == MS1010_FormOrder.No) {
        // 表示順変更画面

        // TODO:一覧件数取得
        //var rowCount = P_listData["#" + FormOrder.ItemList.Id + getAddFormNo()].getDataCount();
        //if (rowCount == 0) {
        //    // エラーメッセージを設定する
        //    setMessage(P_ComMsgTranslated[941060001], procStatus.Error);
        //    // 登録ボタンを非活性にする
        //    setDispMode(getButtonCtrl(FormOrder.ButtonId.Regist), true);
        //    // キャンセルボタンにフォーカス設定
        //    setFocusButton(FormOrder.ButtonId.Cancel);
        //    return;
        //}

        // フォーカス設定
        setFocusButtonAvailable(MS1010_FormOrder.ButtonId.Regist, MS1010_FormOrder.ButtonId.Cancel);
    }
}

/**
 * 登録画面の初期処理
 *
 *  @title {string}                ：画面タイトル
 *  @structureLayerNo {string}     ：階層番号
 */
function formRegistInitForMS1010(title, structureLayerNo) {

    // モーダル画面要素取得
    const modalNo = getCurrentModalNo(P_Article);
    var modalEle = getModalElement(modalNo);
    if (modalEle) {
        // タイトル要素取得
        var titleEle = $(modalEle).find('.title_div').find('span');
        // タイトル編集
        $(titleEle).text(title + P_ComMsgTranslated[111200003]);
    }

    // アイテムID一覧を非表示
    changeListDisplay(MS1010_FormEdit.ItemIdList.Id, false, false);
    // アイテム情報一覧の一部列を非表示
    $.each(MS1010_FormEdit.ItemList.Regist.HideCol, function (key, val) {
        changeColumnDisplay(MS1010_FormEdit.ItemList.Id, val, false);
    });

    // 職種アイテムの場合のみ、拡張項目を表示する
    if (Number(structureLayerNo) == Structure.StructureLayerNo.Layer0) {
        changeColumnDisplay(MS1010_FormEdit.ItemList.Id, MS1010_FormList.ItemList.ItemEx1, true);
    } else {
        changeColumnDisplay(MS1010_FormEdit.ItemList.Id, MS1010_FormList.ItemList.ItemEx1, false);
    }

    // フォーカス設定
    setFocusDelay(MS1010_FormEdit.ItemTranList.Id, MS1010_FormEdit.ItemTranList.ItemName, 0, CtrlFlag.TextBox);
}

/**
 * 修正画面の初期処理
 *
 *  @title {string}                ：画面タイトル
 *  @structureLayerNo {string}     ：階層番号
 */
function formEditInitForMS1010(title, structureLayerNo) {

    // モーダル画面要素取得
    const modalNo = getCurrentModalNo(P_Article);
    var modalEle = getModalElement(modalNo);
    if (modalEle) {
        // タイトル要素取得
        var titleEle = $(modalEle).find('.title_div').find('span');
        // タイトル編集
        $(titleEle).text(title + P_ComMsgTranslated[111120007]);
    }

    // アイテム情報一覧の一部列を非表示
    $.each(MS1010_FormEdit.ItemList.Edit.HideCol, function (key, val) {
        changeColumnDisplay(MS1010_FormEdit.ItemList.Id, val, false);
    });

    // 職種アイテムの場合のみ、拡張項目を表示する
    if (Number(structureLayerNo) == Structure.StructureLayerNo.Layer0) {
        changeColumnDisplay(MS1010_FormEdit.ItemList.Id, MS1010_FormList.ItemList.ItemEx1, true);
    } else {
        changeColumnDisplay(MS1010_FormEdit.ItemList.Id, MS1010_FormList.ItemList.ItemEx1, false);
    }

    // フォーカス設定
    setFocusDelay(MS1010_FormEdit.ItemTranList.Id, MS1010_FormEdit.ItemTranList.ItemName, 0, CtrlFlag.TextBox);
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
function setComboOtherValuesForMS1010(appPath, combo, datas, selected, formNo, ctrlId, valNo) {
    // 一覧画面の検索条件のコンボが変更された時、連動コンボを初期化する
    if (formNo == MS1010_FormList.No && ctrlId == MS1010_FormList.SearchList.Id) {

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
function prevTransFormForMS1010(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element) {

    var conditionDataList = [];

    if (formNo == MS1010_FormList.No) {
        // 一覧画面

        if (!btn_ctrlId) {
            // 行追加(+)アイコン または NOリンクがクリックされた場合

            // 画面タイプ
            var formType;
            if (rowNo < 0) {
                // 行追加(+)アイコンがクリックされた場合は登録画面を表示
                formType = MasterFormType.Regist;
            } else {
                // NOリンクがクリックされた場合は修正画面を表示
                formType = MasterFormType.Edit;
            }

            // 非表示情報の設定
            setValue(MS1010_FormList.HiddenList.Id, MS1010_FormList.HiddenList.FormType, 1, CtrlFlag.Label, formType);

            // 対象の一覧のコントロールグループIDをctrlIdListへ設定する
            const ctrlIdList = [MS1010_FormList.HiddenList.Id];
            conditionDataList = getListDataByCtrlIdList(ctrlIdList, formNo, 0);

        } else if (btn_ctrlId == MS1010_FormList.ButtonId.Order) {
            // 表示順変更ボタン押下時

            // 対象の一覧のコントロールグループIDをctrlIdListへ設定する
            const ctrlIdList = [MS1010_FormList.HiddenList.Id];
            conditionDataList = getListDataByCtrlIdList(ctrlIdList, formNo, 0);
        }
    }

    return [true, conditionDataList];
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
function prevBackBtnProcessForMS1010(appPath, btnCtrlId, status, codeTransFlg) {

    if (btnCtrlId == MS1010_FormEdit.ButtonId.Cancel) {
        // キャンセルボタン押下時

        // 一覧画面へ戻る際、再描画は行わない
        return false;
    }

    return true;
}

///**
// *【オーバーライド用関数】
// *  行追加の前
// *  
// *  @param {string}     appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
// *  @param {html}       element     ：ｲﾍﾞﾝﾄ発生要素
// *  @param {boolean}    isCopyRow   ：行コピーフラグ(true：行コピー、false：行追加)
// *  @param {number}     transPtn    ：画面遷移ﾊﾟﾀｰﾝ(1：子画面、2：単票入力、3：単票参照、4：共通機能、5：他機能別ﾀﾌﾞ、6：他機能表示切替)
// *  @param {string}     transTarget ：遷移先(子画面NO / 単票一覧ctrlId / 共通機能ID / 他機能ID|画面NO)
// *  @param {number}     dispPtn     ：遷移表示ﾊﾟﾀｰﾝ(1：表示切替、2：ﾎﾟｯﾌﾟｱｯﾌﾟ、3：一覧直下)
// *  @param {number}     formNo      ：遷移元formNo
// *  @param {string}     ctrlId      ：遷移元の一覧ctrlid
// *  @param {string}     btn_ctrlId  ：ボタンのbtn_ctrlid
// *  @param {int}        rowNo       ：遷移元の一覧の選択行番号（新規の場合は-1）
// *  @param {boolean}    confirmFlg  ：遷移前確認ﾌﾗｸﾞ(true：確認する、false：確認しない)
// */
////function prevAddNewRow(appPath, element, transPtn, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, confirmFlg) {
//function prevAddNewRow(appPath, element, isCopyRow, transPtn, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, confirmFlg) {
//    return true;    // 個別実装で以後の処理の実行可否を制御 true：続行、false：中断
//}

///**
// *【オーバーライド用関数】
// *  行削除の前
// *
// *  @appPath    {string}    ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
// *  @btn        {<a>}       ：行削除ﾘﾝｸのa要素
// *  @id         {string}    ：行削除ﾘﾝｸの対象一覧のCTRLID
// *  @checkes    {要素}      ：削除対象の要素リスト(tabulator一覧の場合、削除対象のデータリスト)
// */
//function preDeleteRow(appPath, btn, id, checkes) {
//    // 共通処理により非表示の削除ボタンを押下
//    return preDeleteRowCommon(id, [MasterFormList.StandardItemList.Id, MasterFormList.FactoryItemList.Id]);
//}

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
function beforeSearchBtnProcessForMS1010(appPath, btn, conductId, pgmId, formNo, conductPtn) {

    if (formNo == MS1010_FormList.No) {
        // 一覧画面

        // 検索条件より検索結果一覧に表示する階層、親構成IDを取得する
        var existFlg = false;
        var structureLayerNo;
        var parentStrutureId;

        // 検索条件取得
        var factoryId = getValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.Factory, 1, CtrlFlag.Combo);
        var jobId = getValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.Job, 1, CtrlFlag.Combo);
        var largeClassficationId = getValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.LargeClassfication, 1, CtrlFlag.Combo);
        var middleClassficationId = getValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.MiddleClassfication, 1, CtrlFlag.Combo);

        // 検索条件の退避情報をクリア
        setValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenFactoryId, 1, CtrlFlag.Label, "");
        setValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenJobId, 1, CtrlFlag.Label, "");
        setValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenLargeClassficationId, 1, CtrlFlag.Label, "");
        setValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenMiddleClassficationId, 1, CtrlFlag.Label, "");

        if (middleClassficationId) {
            // 階層:3(機種小分類)、親構成ID:機種中分類ID
            structureLayerNo = Structure.StructureLayerNo.Layer3;
            parentStrutureId = middleClassficationId;
            // 検索条件を退避
            setValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenFactoryId, 1, CtrlFlag.Label, factoryId);
            setValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenJobId, 1, CtrlFlag.Label, jobId);
            setValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenLargeClassficationId, 1, CtrlFlag.Label, largeClassficationId);
            setValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenMiddleClassficationId, 1, CtrlFlag.Label, middleClassficationId);
           existFlg = true;
        }
        if (!existFlg && largeClassficationId) {
            // 階層:2(機種中分類)、親構成ID:機種大分類ID
            structureLayerNo = Structure.StructureLayerNo.Layer2;
            parentStrutureId = largeClassficationId;
            // 検索条件を退避
            setValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenFactoryId, 1, CtrlFlag.Label, factoryId);
            setValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenJobId, 1, CtrlFlag.Label, jobId);
            setValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenLargeClassficationId, 1, CtrlFlag.Label, largeClassficationId);
           existFlg = true;
        }
        if (!existFlg && jobId) {
            // 階層:1(機種大分類)、親構成ID:職種ID
            structureLayerNo = Structure.StructureLayerNo.Layer1;
            parentStrutureId = jobId;
            // 検索条件を退避
            setValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenFactoryId, 1, CtrlFlag.Label, factoryId);
            setValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenJobId, 1, CtrlFlag.Label, jobId);
           existFlg = true;
        }
        if (!existFlg && factoryId) {
            // 階層:0(職種)、親構成ID:0
            structureLayerNo = Structure.StructureLayerNo.Layer0;
            parentStrutureId = Structure.ParentStructureId.TopLayer;
            // 検索条件を退避
            setValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenFactoryId, 1, CtrlFlag.Label, factoryId);
           existFlg = true;
        }

        // 非表示情報の設定
        setValue(MS1010_FormList.HiddenList.Id, MS1010_FormList.HiddenList.FactoryId, 1, CtrlFlag.Label, factoryId);
        setValue(MS1010_FormList.HiddenList.Id, MS1010_FormList.HiddenList.StructureLayerNo, 1, CtrlFlag.Label, structureLayerNo);
        setValue(MS1010_FormList.HiddenList.Id, MS1010_FormList.HiddenList.ParentStructureId, 1, CtrlFlag.Label, parentStrutureId);
    }
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
function afterSearchBtnProcessForMS1010(appPath, btn, conductId, pgmId, formNo, conductPtn) {

    if (formNo == MS1010_FormList.No) {
        // 一覧画面

        // 階層取得
        var structureLayerNo = getValue(MS1010_FormList.HiddenList.Id, MS1010_FormList.HiddenList.StructureLayerNo, 1, CtrlFlag.Label);

        // 階層より一覧のタイトルを取得する
        var title = getLocationNameForMS1010(structureLayerNo);

        // 一覧タイトル編集

        // 一覧タイトル要素取得
        var titleEle = $(P_Article).find("#" + MS1010_FormList.FactoryItemList.Id + getAddFormNo() + "_div").find('.title');
        if (titleEle) {
            // タイトル編集
            $(titleEle).text(title);
        }
    }
}

/**
 * 階層より階層名称を取得
 *
 *  @structureLayerNo {string}     ：階層
 */
function getLocationNameForMS1010(structureLayerNo) {

    // 階層より階層名称を取得する
    if (Number(structureLayerNo) == Structure.StructureLayerNo.Layer0) {
        // 階層:0 職種
        return P_ComMsgTranslated[111120002];
    }
    if (Number(structureLayerNo) == Structure.StructureLayerNo.Layer1) {
        // 階層:1 機種大分類
        return P_ComMsgTranslated[111070005];
    }
    if (Number(structureLayerNo) == Structure.StructureLayerNo.Layer2) {
        // 階層:2 機種中分類
        return P_ComMsgTranslated[111070006];
    }
    if (Number(structureLayerNo) == Structure.StructureLayerNo.Layer3) {
        // 階層:3 機種小分類
        return P_ComMsgTranslated[111070007];
    }

    return "";
}

///**
// *【オーバーライド用関数】実行ﾁｪｯｸ処理 - 前処理
// *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
// *  @conductId   {string}   ：機能ID
// *  @formNo      {number}   ：画面番号
// *  @btn         {button}   ：押下されたボタン要素
// */
//function registCheckPre(appPath, conductId, formNo, btn) {
//    return true;
//}

///**
// * 【オーバーライド用関数】実行ボタン前処理
// *  @param {string} appPath ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
// *  @param {string} transDiv：画面遷移アクション区分
// *  @param {string} conductId：機能ID
// *  @param {string} pgmId ：プログラムID
// *  @param {number} formNo ：画面NO
// *  @param {html} btn  ：ボタン要素
// *  @param {number} conductPtn  ：機能処理ﾊﾟﾀｰﾝ
// *  @param {boolean} autoBackFlg ：ajax正常終了後、自動戻るフラグ　false:戻らない、true:自動で戻る
// *  @param {boolean} isEdit ：単票表示フラグ
// *  @param {number} confirmNo ：確認番号
// *  @return {bool} 処理続行フラグ Trueなら続行、Falseなら処理終了
// */
//function preRegistProcess(appPath, transDiv, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, confirmNo) {
//    // 独自の確認メッセージを表示するために使用、関連情報パラメータは0に設定してください。
//    return true;
//}

///*==9:削除処理==*/
///**
// * 【オーバーライド用関数】削除ボタン前処理
// *  @param {string} appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
// *  @param {string} conductId   ：機能ID
// *  @param {string} pgmId       ：プログラムID
// *  @param {number} formNo      ：画面番号
// *  @param {byte}   conductPtn  ：画面ﾊﾟﾀｰﾝ
// *  @param {element}btn         ：ﾎﾞﾀﾝ要素
// *  @param {boolean}isEdit      ：単票ｴﾘｱﾌﾗｸﾞ
// *  @param {int}    confirmNo   ：確認番号(ﾃﾞﾌｫﾙﾄ：0)
// *  @return {bool} 処理続行フラグ Trueなら続行、Falseなら処理終了
// */
//function preDeleteProcess(appPath, conductId, pgmId, formNo, conductPtn, btn, isEdit, confirmNo) {
//    // 独自の確認メッセージを表示するために使用、関連情報パラメータは0に設定してください。
//    return true;
//}

///**
// *【オーバーライド用関数】子画面新規遷移前ﾁｪｯｸ処置
// *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
// *  @conductId   {string}   ：機能ID
// *  @formNo      {number}   ：画面番号
// *  @btn         {button}   ：押下されたボタン要素
// */
//function prevTransChildForm(appPath, conductId, formNo, btn) {
//    return true;
//}

///**
// *【オーバーライド用関数】他機能遷移時パラメータを個別実装で作成
// *  @param {string} appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
// *  @param {string} transTarget ：遷移先情報（機能ID|画面NO）
// *  @param {number} formNo      ：遷移元画面NO
// *  @param {string} ctrlId      ：遷移元の一覧ctrlid
// *  @param {string} btn_ctrlId  ：ボタンのbtn_ctrlid
// *  @param {int}    rowNo       ：遷移元の一覧の選択行番号（新規の場合は-1）
// *  @param {html}   element     ：ｲﾍﾞﾝﾄ発生要素
// *
// *  @return {List<Dictionary<string, object>>}  conditionDataList
// */
//function createParamTransOtherIndividual(appPath, transTarget, formNo, ctrlId, btn_ctrlId, rowNo, element) {

//    var conditionDataList = [];

//    return conditionDataList;
//}

///**
// *【オーバーライド用関数】他機能遷移先の戻る時再検索用退避条件(P_SearchCondition)を作成
// *  @param {string} appPath         ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
// *  @param {string} transTarget     ：遷移先情報（機能ID|画面NO|親画面NO）
// *  @param {string} transConductId  ：遷移先の機能ID
// *  @param {number} formNo          ：遷移元画面NO
// *  @param {string} ctrlId          ：遷移元の一覧ctrlid
// *  @param {string} btn_ctrlId      ：ボタンのbtn_ctrlid
// *  @param {int}    rowNo           ：遷移元の一覧の選択行番号（新規の場合は-1）
// *  @param {html}   element         ：ｲﾍﾞﾝﾄ発生要素
// *
// *  @return {Dictionary<string, object>}  dicBackConditions
// */
//function createParamTransOtherBack(appPath, transTarget, transConductId, formNo, ctrlId, btn_ctrlId, rowNo, element) {

//    var dicBackConditions = {};

//    return dicBackConditions;
//}

///**
// *【オーバーライド用関数】共通機能ポップアップ画面遷移前ﾁｪｯｸ処置
// *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
// *  @conductId   {string}   ：遷移先情報
// *  @formNo      {number}   ：画面番号
// *  @btn         {button}   ：押下されたボタン要素
// */
//function prevTransCmForm(appPath, transTarget, formNo, btn) {
//    return true;
//}

///**
// *【オーバーライド用関数】Excel出力ﾁｪｯｸ処理 - 前処理
// *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
// *  @conductId   {string}   ：機能ID
// *  @formNo      {number}   ：画面番号
// *  @btn         {button}   ：押下されたボタン要素
// */
//function reportCheckPre(appPath, conductId, formNo, btn) {
//    return true;
//}

/**
 *【オーバーライド用関数】登録前追加条件取得処理
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 *  
 *  @return {Dictionary<string, object>}  追加する条件リスト
 */
function addSearchConditionDictionaryForRegistForMS1010(appPath, conductId, formNo, btn) {

    var conditionDataList = [];

    var btnName = $(btn).attr("name");
    if (btnName == MS1010_FormEdit.ButtonId.Regist) {
        // 登録ボタン押下時

        // 対象の一覧のコントロールグループIDをctrlIdListへ設定する
        const ctrlIdList = [MS1010_FormList.HiddenList.Id];
        conditionDataList = getListDataByCtrlIdList(ctrlIdList, MS1010_FormList.No, 0);
    } else if (btnName == MS1010_FormList.ButtonId.Delete) {
        // 削除ボタン押下時

        // 対象の一覧のコントロールグループIDをctrlIdListへ設定する
        const ctrlIdList = [MS1010_FormList.SearchList.Id, MS1010_FormList.HiddenList.Id];
        conditionDataList = getListDataByCtrlIdList(ctrlIdList, MS1010_FormList.No, 0);
    }

    return conditionDataList;
}

///**
// *【オーバーライド用関数】取込処理個別入力チェック
// *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
// *  @conductId   {string}   ：機能ID
// *  @formNo      {number}   ：画面番号
// *  
// *  @return {bool}:個別入力チェック後も既存の入力チェックを行う場合はtrue
// *  @return {bool}:個別の入力チェックでエラーの場合はtrue
// */
//function preInputCheckUpload(appPath, conductId, formNo) {
//    var isContinue = true
//    var isError = false;
//    var isAutoBackFlg = true;

//    return [isContinue, isError, isAutoBackFlg];
//}

///**
// * 【オーバーライド用関数】Tabuator一覧の描画前処理
// * @param {string} appPath  ：アプリケーションルートパス
// * @param {string} id       ：一覧のID(#,_FormNo付き)
// * @param {string} options  ： 一覧のオプション情報
// * @param {object} header   ：ヘッダー情報
// * @param {object} dispData ：データ
// */
//function prevCreateTabulatorForMS1010(appPath, id, options, header, dispData) {

//    // 表示順変更画面の一覧のレコードの並べ替えを可能にする
//    if (id == "#" + MS1010_FormOrder.ItemList.Id + getAddFormNo()) {

//        // 行移動可
//        options["movableRows"] = true;
//        if (!header.some(x => x.field === "moveRowBtn")) {
//            // 先頭に移動用のアイコン列追加
//            header.unshift({ title: "", rowHandle: true, field: "moveRowBtn", formatter: "handle", headerSort: false, frozen: true, width: 30, minWidth: 30 });
//        }
//    }
//}

/**
 * 【オーバーライド用関数】Tabulatorの描画が完了時の処理
 * @param {any} tbl 一覧
 * @param {any} id 一覧のID(#,_FormNo付き)
 */
function postBuiltTabulatorForMS1010(tbl, id) {

    if (getFormNo() == MS1010_FormList.No) {
        // 一覧画面

        if (id == '#' + MS1010_FormList.FactoryItemList.Id + getAddFormNo()) {
            // 工場アイテム一覧


            // 階層取得
            var structureLayerNo = getValue(MS1010_FormList.HiddenList.Id, MS1010_FormList.HiddenList.StructureLayerNo, 1, CtrlFlag.Label);

            // 職種アイテムの場合のみ、拡張項目を表示する
            if (Number(structureLayerNo) == Structure.StructureLayerNo.Layer0) {
                changeTabulatorColumnDisplay(MS1010_FormList.FactoryItemList.Id, MS1010_FormList.ItemList.ItemEx1, true);
            } else {
                changeTabulatorColumnDisplay(MS1010_FormList.FactoryItemList.Id, MS1010_FormList.ItemList.ItemEx1, false);
            }
        }
    } else if (getFormNo() == MS1010_FormOrder.No) {
        // 表示順変更画面

        if (id == '#' + MS1010_FormOrder.ItemList.Id + getAddFormNo()) {
            // アイテム一覧の一部列を非表示
            $.each(MS1010_FormOrder.ItemList.HideCol, function (key, val) {
                changeTabulatorColumnDisplay(MS1010_FormOrder.ItemList.Id, val, false);
            });
        }
    }
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
function prevInitFormDataForMS1010(appPath, formNo, btnCtrlId, conditionDataList, listDefines, pgmId) {

    if (formNo != MS1010_FormList.No) {
        // 一覧画面以外の場合は終了
        return [conditionDataList, listDefines];
    }

    if ($(conditionDataList).length) {
        $.each($(conditionDataList), function (idx, conditionData) {
            if (conditionData.FORMNO == MS1010_FormList.No) {
                // 一覧画面
                if (conditionData.CTRLID == MS1010_FormList.SearchList.Id || conditionData.CTRLID == MS1010_FormList.HiddenList.Id) {
                    if (btnCtrlId != MS1010_FormEdit.ButtonId.Regist) {
                        // 検索条件一覧、非表示情報一覧を定義情報に格納
                        listDefines.push(conditionData);
                    }
                }
            }
        });
    }

    return [conditionDataList, listDefines];
}

/**
 * 工場コンボボックス変更時イベント
 * */
function factoryCombChangeEventForMS1010(appPath) {
    var comb = getCtrl(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.Factory, 1, CtrlFlag.Combo, false, false);
    var factoryId = $(comb)[0][comb.selectedIndex].value;

    // 構成IDを非表示の項目に設定する
    setValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenFactoryId, 1, CtrlFlag.Label, factoryId, false, false);

    // 職種コンボボックス初期化
    var selects = $(P_Article).find("#" + MS1010_FormList.SearchList.Id + getAddFormNo()).find("td[data-name='VAL" + MS1010_FormList.SearchList.Job + "'] select.dynamic");
    //initComboBox(appPath, selects, 'C0022', '1010,0,@6,@9999', 2, false, -1, null, [factoryId]);
    initComboBox(
        appPath,
        selects,
        selects[0].getAttribute("data-sqlid"),
        selects[0].getAttribute("data-param"),
        selects[0].getAttribute("data-option"),
        selects[0].getAttribute("data-nullcheck"),
        -1,
        null,
        [factoryId]
    );

    // 職種大分類コンボ一覧を初期化
    var largeClassfication = getCtrl(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.LargeClassfication, 1, CtrlFlag.Combo, false, false);
    $(largeClassfication).children().remove();

    // 職種中分類コンボ一覧を初期化
    var middleClassfication = getCtrl(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.MiddleClassfication, 1, CtrlFlag.Combo, false, false);
    $(middleClassfication).children().remove();

    // 職種、機種大分類、機種中分類(非表示列)の項目に空白を設定する
    setValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenJobId, 1, CtrlFlag.Label, "", false, false);
    setValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenLargeClassficationId, 1, CtrlFlag.Label, "", false, false);
    setValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenMiddleClassficationId, 1, CtrlFlag.Label, "", false, false);
}

/**
 * 職種コンボボックス変更時イベント
 * */
function jobCombChangeEventForMS1010(appPath) {
    var comb = getCtrl(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.Job, 1, CtrlFlag.Combo, false, false);
    var jobId = $(comb)[0][comb.selectedIndex].value;

    // 構成IDを非表示の項目に設定する
    setValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenJobId, 1, CtrlFlag.Label, jobId, false, false);

    // 職種大分類コンボボックス初期化
    var selects = $(P_Article).find("#" + MS1010_FormList.SearchList.Id + getAddFormNo()).find("td[data-name='VAL" + MS1010_FormList.SearchList.LargeClassfication + "'] select.dynamic");
    //initComboBox(appPath, selects, 'C0001', '1010,1,@7,@9999', 2, false, -1, null, [getValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.Factory, 1, CtrlFlag.Combo, false, false)]);
    initComboBox(
        appPath,
        selects,
        selects[0].getAttribute("data-sqlid"),
        selects[0].getAttribute("data-param"),
        selects[0].getAttribute("data-option"),
        selects[0].getAttribute("data-nullcheck"),
        -1,
        null,
        [getValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.Factory, 1, CtrlFlag.Combo, false, false)]
    );

    // 職種中分類コンボボックスのアイテムを削除
    var middleClassfication = getCtrl(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.MiddleClassfication, 1, CtrlFlag.Combo, false, false);
    $(middleClassfication).children().remove();

    // 機種大分類、機種中分類(非表示列)の項目に空白を設定する
    setValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenLargeClassficationId, 1, CtrlFlag.Label, "", false, false);
    setValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenMiddleClassficationId, 1, CtrlFlag.Label, "", false, false);
}

/**
 * 職種大分類コンボボックス変更時イベント
 * */
function largeClassficationCombChangeEventForMS1010(appPath) {
    var comb = getCtrl(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.LargeClassfication, 1, CtrlFlag.Combo, false, false);
    var largeClassficationId = $(comb)[0][comb.selectedIndex].value;

    // 構成IDを非表示の項目に設定する
    setValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenLargeClassficationId, 1, CtrlFlag.Label, largeClassficationId, false, false);

    // 職種中分類コンボボックス初期化
    var selects = $(P_Article).find("#" + MS1010_FormList.SearchList.Id + getAddFormNo()).find("td[data-name='VAL" + MS1010_FormList.SearchList.MiddleClassfication + "'] select.dynamic");
    //initComboBox(appPath, selects, 'C0001', '1010,2,@8,@9999', 2, false, -1, null, [getValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.Factory, 1, CtrlFlag.Combo, false, false)]);
    initComboBox(
        appPath,
        selects,
        selects[0].getAttribute("data-sqlid"),
        selects[0].getAttribute("data-param"),
        selects[0].getAttribute("data-option"),
        selects[0].getAttribute("data-nullcheck"),
        -1,
        null,
        [getValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.Factory, 1, CtrlFlag.Combo, false, false)]
    );

    // 機種中分類(非表示列)の項目に空白を設定する
    setValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenMiddleClassficationId, 1, CtrlFlag.Label, "", false, false);
}

/**
 * 職種中分類コンボボックス変更時イベント
 * */
function middleClassficationCombChangeEventForMS1010(appPath) {
    var comb = getCtrl(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.MiddleClassfication, 1, CtrlFlag.Combo, false, false);
    var middleClassficationId = $(comb)[0][comb.selectedIndex].value;

    // 構成IDを非表示の項目に設定する
    setValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenMiddleClassficationId, 1, CtrlFlag.Label, middleClassficationId, false, false);
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
function postRegistProcessForMS1010(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data) {

    // 選択行削除処理後

    // 検索条件のコンボを再設定

    // 退避していた検索条件を取得
    var factoryId = getValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenFactoryId, 1, CtrlFlag.Label);
    var jobId = getValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenJobId, 1, CtrlFlag.Label);
    var largeClassficationId = getValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenLargeClassficationId, 1, CtrlFlag.Label);
    var middleClassficationId = getValue(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.HiddenMiddleClassficationId, 1, CtrlFlag.Label);

    // 工場コンボの値セット＆changeイベント発生
    setValueAndTriggerNoChange(MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.Factory, 1, CtrlFlag.Combo, factoryId);
    var cnt = 1;
    const waitSeconds = 1000;
    if (jobId) {
        // 職種が選択されている場合、退避した検索条件をコンボにセット
        // 職種コンボの値セット＆changeイベント発生
        setTimeout(setValueAndTriggerNoChange, waitSeconds * cnt++, MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.Job, 1, CtrlFlag.Combo, jobId);
    }
    if (largeClassficationId) {
        // 機種大分類が選択されている場合、退避した検索条件をコンボにセット
        // 機種大分類コンボの値セット＆changeイベント発生
        setTimeout(setValueAndTriggerNoChange, waitSeconds * cnt++, MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.LargeClassfication, 1, CtrlFlag.Combo, largeClassficationId);
    }
    if (middleClassficationId) {
        // 機種中分類が選択されている場合、退避した検索条件をコンボにセット
        // 機種中分類コンボの値セット＆changeイベント発生
        setTimeout(setValueAndTriggerNoChange, waitSeconds * cnt++, MS1010_FormList.SearchList.Id, MS1010_FormList.SearchList.MiddleClassfication, 1, CtrlFlag.Combo, middleClassficationId);
    }
}