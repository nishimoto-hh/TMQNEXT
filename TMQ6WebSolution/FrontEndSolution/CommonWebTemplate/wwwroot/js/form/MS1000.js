/* ========================================================================
 *  機能名　    ：   【MS1000】地区/工場マスタ
 * ======================================================================== */

///**
// * 自身の相対パスを取得
// */
//function getPath() {
//    var root;
//    var scripts = document.getElementsByTagName("script");
//    var i = scripts.length;
//    while (i--) {
//        var match = scripts[i].src.match(/(^|.*\/)MS1000\.js$/);
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
const ConductId_MS1000 = "MS1000";

// 一覧画面
const MS1000_FormList = {
    // 画面No
    No: 1,
    // 検索条件一覧
    SearchList: {
        Id: "BODY_000_00_LST_0",
        // 地区列
        District: 1,
        // コンボ生成用地区ID列(非表示列)
        HiddenDistrictId: 3,
    },
    // 地区アイテム一覧
    DistrictItemList: {
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
        // 本務工場地区構成ID列
        DutyFactoryLocation: 11
    },
    // アイテム一覧
    ItemList: {
        // アイテムID列
        ItemId: 2,
        // アイテム翻訳列
        ItemName: 3,
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
    },
}

// 登録・修正画面
const MS1000_FormEdit = {
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
    // 地区アイテム情報一覧
    DistrictItemList: {
        Id: "BODY_020_00_LST_1",
    },
    // 工場アイテム情報一覧
    FactoryItemList: {
        Id: "BODY_030_00_LST_1",
    },
    // アイテム情報一覧(地区/工場共通)
    ItemList: {
        // 登録画面
        Regist: {
            // 非表示列
            HideCol: { ItemId: 2, ItemName: 3, Order: 21, Delete: 22 },
        },
        // 修正画面
        Edit: {
            // 非表示列
            HideCol: { ItemId: 2, ItemName: 3, Order: 21 },
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
const MS1000_FormOrder = {
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

//// 拡張項目列
//const MS1000_ItemExCol = {
//    // 地区
//    District: {},
//    // 工場
//    Factory: { ExData1: 11, ExData2: 12, ExData3: 13, ExData4: 14 },
//}
//// 拡張項目コントロール列(TextBox: 0, Label: 1, Combo: 2, ChkBox: 3, Link: 4, Input: 5, Textarea: 6, Password: 7, Button: 8)
//const MS1000_ItemExCtrlCol = {
//    // 地区
//    District: {},
//    // 工場
//    Factory: { ExData1: 2, ExData2: 2, ExData3: 2, ExData4: 0 },
//}

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
function initFormOriginalForMS1000(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data) {
    if (formNo == MS1000_FormList.No) {
        // 一覧画面

        // 非表示情報を非表示
        changeListDisplay(MS1000_FormList.HiddenList.Id, false, false);

        // フォーカス設定
        setFocusDelay(MS1000_FormList.SearchList.Id, MS1000_FormList.SearchList.District, 0, CtrlFlag.Combo);

        // 初期検索時のみ初期値セット
        if (actionCtrlId == "") {
            // 本務地区を初期値にセット
            var location = getValue(MS1000_FormList.HiddenList.Id, MS1000_FormList.HiddenList.DutyFactoryLocation, 1, CtrlFlag.Label, false, false);
            setValue(MS1000_FormList.SearchList.Id, MS1000_FormList.SearchList.District, 1, CtrlFlag.Combo, location);
        } else if (actionCtrlId == MS1000_FormEdit.ButtonId.Regist) {
            // 登録処理後

            // 検索条件のコンボを再設定

            // 退避していた検索条件を取得
            var districtId = getValue(MS1000_FormList.SearchList.Id, MS1000_FormList.SearchList.HiddenDistrictId, 1, CtrlFlag.Label);

            if (districtId) {
                // 地区が選択されている場合、退避した検索条件をコンボにセット
                setValue(MS1000_FormList.SearchList.Id, MS1000_FormList.SearchList.District, 1, CtrlFlag.Combo, districtId);
            }

        }

        // 地区コンボボックス変更時
        var districtComb = getCtrl(MS1000_FormList.SearchList.Id, MS1000_FormList.SearchList.District, 1, CtrlFlag.Combo, false, false);
        $(districtComb).change(function () {

            districtCombChangeEventForMS1000(appPath);

        });

    } else if (formNo == MS1000_FormEdit.No) {
        // 登録・修正画面

        var ctrlId;
        var title;

        // 階層取得
        var structureLayerNo = getValueByOtherForm(MS1000_FormList.No, MS1000_FormList.HiddenList.Id, MS1000_FormList.HiddenList.StructureLayerNo, 1, CtrlFlag.Label);
        if (Number(structureLayerNo) == Structure.StructureLayerNo.Layer0) {
            // 地区アイテムの場合

            // コントロールID
            ctrlId = MS1000_FormEdit.DistrictItemList.Id;
            // 画面タイトル
            title = P_ComMsgTranslated[111170008];
        } else if (Number(structureLayerNo) == Structure.StructureLayerNo.Layer1) {
            // 工場アイテムの場合

            // コントロールID
            ctrlId = MS1000_FormEdit.FactoryItemList.Id;
            // 画面タイトル
            title = P_ComMsgTranslated[111100012];
        }

        // 画面タイプ取得
        var formType = getValueByOtherForm(MS1000_FormList.No, MS1000_FormList.HiddenList.Id, MS1000_FormList.HiddenList.FormType, 1, CtrlFlag.Label);
        if (Number(formType) == MasterFormType.Regist) {
            // 登録画面の初期処理
            formRegistInitForMS1000(title, ctrlId);
        } else if (Number(formType) == MasterFormType.Edit) {
            // 修正画面の初期処理
            formEditInitForMS1000(title, ctrlId);
        }

    } else if (formNo == MS1000_FormOrder.No) {
        // 表示順変更画面

        //// TODO:一覧件数取得
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
        setFocusButtonAvailable(MS1000_FormOrder.ButtonId.Regist, MS1000_FormOrder.ButtonId.Cancel);
    }
}

/**
 * 登録画面の初期処理
 *
 *  @title {string}     ：画面タイトル
 *  @ctrlId {string}    ：コントロールID
 */
function formRegistInitForMS1000(title, ctrlId) {

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
    changeListDisplay(MS1000_FormEdit.ItemIdList.Id, false, false);
    // アイテム情報一覧を表示
    changeListDisplay(MS1000_FormEdit.DistrictItemList.Id, false);
    changeListDisplay(MS1000_FormEdit.FactoryItemList.Id, false);
    changeListDisplay(ctrlId, true, false);
    // アイテム情報一覧の一部列を非表示
    $.each(MS1000_FormEdit.ItemList.Regist.HideCol, function (key, val) {
        changeColumnDisplay(ctrlId, val, false);
    });

    // フォーカス設定
    setFocusDelay(MS1000_FormEdit.ItemTranList.Id, MS1000_FormEdit.ItemTranList.ItemName, 0, CtrlFlag.TextBox);
}

/**
 * 修正画面の初期処理
 *
 *  @title {string}     ：画面タイトル
 *  @ctrlId {string}    ：コントロールID
 */
function formEditInitForMS1000(title, ctrlId) {

    // モーダル画面要素取得
    const modalNo = getCurrentModalNo(P_Article);
    var modalEle = getModalElement(modalNo);
    if (modalEle) {
        // タイトル要素取得
        var titleEle = $(modalEle).find('.title_div').find('span');
        // タイトル編集
        $(titleEle).text(title + P_ComMsgTranslated[111120007]);
    }

    // アイテム情報一覧を表示
    changeListDisplay(MS1000_FormEdit.DistrictItemList.Id, false);
    changeListDisplay(MS1000_FormEdit.FactoryItemList.Id, false);
    changeListDisplay(ctrlId, true, false);
    // アイテム情報一覧の一部列を非表示
    $.each(MS1000_FormEdit.ItemList.Edit.HideCol, function (key, val) {
        changeColumnDisplay(ctrlId, val, false);
    });

    // フォーカス設定
    setFocusDelay(MS1000_FormEdit.ItemTranList.Id, MS1000_FormEdit.ItemTranList.ItemName, 0, CtrlFlag.TextBox);
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
function prevTransFormForMS1000(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element) {

    var conditionDataList = [];

    if (formNo == MS1000_FormList.No) {
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
            setValue(MS1000_FormList.HiddenList.Id, MS1000_FormList.HiddenList.FormType, 1, CtrlFlag.Label, formType);

            // 対象の一覧のコントロールグループIDをctrlIdListへ設定する
            const ctrlIdList = [MS1000_FormList.HiddenList.Id];
            conditionDataList = getListDataByCtrlIdList(ctrlIdList, formNo, 0);

        } else if (btn_ctrlId == MS1000_FormList.ButtonId.Order) {
            // 表示順変更ボタン押下時

            // 対象の一覧のコントロールグループIDをctrlIdListへ設定する
            const ctrlIdList = [MS1000_FormList.HiddenList.Id];
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
function prevBackBtnProcessForMS1000(appPath, btnCtrlId, status, codeTransFlg) {

    if (btnCtrlId == MS1000_FormEdit.ButtonId.Cancel) {
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
//    //// 共通処理により非表示の削除ボタンを押下
//    //return preDeleteRowCommon(id, [MasterFormList.StandardItemList.Id, MasterFormList.FactoryItemList.Id]);
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
function beforeSearchBtnProcessForMS1000(appPath, btn, conductId, pgmId, formNo, conductPtn) {

    if (formNo == MS1000_FormList.No) {
        // 一覧画面

        // アイテム一覧
        var detailArea = $(P_Article).find("#" + P_formDetailId);
        var districtList = $(detailArea).find(".detail_div").find("#" + MS1000_FormList.DistrictItemList.Id + getAddFormNo() + "_div");
        var factoryList = $(detailArea).find(".detail_div").find("#" + MS1000_FormList.FactoryItemList.Id + getAddFormNo() + "_div");

        // 検索条件の地区ID取得
        var districtId = getValue(MS1000_FormList.SearchList.Id, MS1000_FormList.SearchList.District, 1, CtrlFlag.Combo);
        if (!districtId) {
            // 未選択の場合は、工場アイテム一覧を非表示
            //changeListDisplay(MS1000_FormList.FactoryItemList.Id, false);
            //changeListDisplay(MS1000_FormList.DistrictItemList.Id, true);
            $(districtList).css('display', '');
            $(factoryList).css('display', 'none');
        } else {
            // 選択済の場合は、地区アイテム一覧を非表示
            //changeListDisplay(MS1000_FormList.DistrictItemList.Id, false);
            //changeListDisplay(MS1000_FormList.FactoryItemList.Id, true);
            $(districtList).css('display', 'none');
            $(factoryList).css('display', '');
        }

        // 階層
        var structureLayerNo = !districtId ? Structure.StructureLayerNo.Layer0 : Structure.StructureLayerNo.Layer1;
        // 親構成ID
        var parentStrutureId = !districtId ? Structure.ParentStructureId.TopLayer : districtId;

        // 非表示情報の設定
        setValue(MS1000_FormList.HiddenList.Id, MS1000_FormList.HiddenList.StructureLayerNo, 1, CtrlFlag.Label, structureLayerNo);
        setValue(MS1000_FormList.HiddenList.Id, MS1000_FormList.HiddenList.ParentStructureId, 1, CtrlFlag.Label, parentStrutureId);
    }
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
function addSearchConditionDictionaryForRegistForMS1000(appPath, conductId, formNo, btn) {

    var conditionDataList = [];

    var btnName = $(btn).attr("name");
    if (btnName == MS1000_FormEdit.ButtonId.Regist) {
        // 登録ボタン押下時

        // 対象の一覧のコントロールグループIDをctrlIdListへ設定する
        const ctrlIdList = [MS1000_FormList.HiddenList.Id];
        conditionDataList = getListDataByCtrlIdList(ctrlIdList, MS1000_FormList.No, 0);
    } else if (btnName == MS1000_FormList.ButtonId.Delete) {
        // 削除ボタン押下時

        // 対象の一覧のコントロールグループIDをctrlIdListへ設定する
        const ctrlIdList = [MS1000_FormList.SearchList.Id, MS1000_FormList.HiddenList.Id];
        conditionDataList = getListDataByCtrlIdList(ctrlIdList, MS1000_FormList.No, 0);
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
//function prevCreateTabulatorForMS1000(appPath, id, options, header, dispData) {

//    // 表示順変更画面の一覧のレコードの並べ替えを可能にする
//    if (id == "#" + MS1000_FormOrder.ItemList.Id + getAddFormNo()) {

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
function postBuiltTabulatorForMS1000(tbl, id) {

    if (getFormNo() == MS1000_FormOrder.No) {
        // 表示順変更画面

        if (id == '#' + MS1000_FormOrder.ItemList.Id + getAddFormNo()) {
            // アイテム一覧の一部列を非表示
            $.each(MS1000_FormOrder.ItemList.HideCol, function (key, val) {
                changeTabulatorColumnDisplay(MS1000_FormOrder.ItemList.Id, val, false);
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
function prevInitFormDataForMS1000(appPath, formNo, btnCtrlId, conditionDataList, listDefines, pgmId) {

    if (formNo != MS1000_FormList.No) {
        // 一覧画面以外の場合は終了
        return [conditionDataList, listDefines];
    }

    if ($(conditionDataList).length) {
        $.each($(conditionDataList), function (idx, conditionData) {
            if (conditionData.FORMNO == MS1000_FormList.No) {
                // 一覧画面
                if (conditionData.CTRLID == MS1000_FormList.SearchList.Id || conditionData.CTRLID == MS1000_FormList.HiddenList.Id) {
                    if (btnCtrlId != MS1000_FormEdit.ButtonId.Regist) {
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
 * 地区コンボボックス変更時イベント
 * */
function districtCombChangeEventForMS1000(appPath) {
    var comb = getCtrl(MS1000_FormList.SearchList.Id, MS1000_FormList.SearchList.District, 1, CtrlFlag.Combo, false, false);
    var districtId = $(comb)[0][comb.selectedIndex].value;

    // 構成IDを非表示の項目に設定する
    setValue(MS1000_FormList.SearchList.Id, MS1000_FormList.SearchList.HiddenDistrictId, 1, CtrlFlag.Label, districtId, false, false);
}