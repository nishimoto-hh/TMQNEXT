/* ========================================================================
 *  機能名　    ：   【MS1040】予備品ﾛｹｰｼｮﾝ(予備品倉庫・棚)マスタ
 * ======================================================================== */

///**
// * 自身の相対パスを取得
// */
//function getPath() {
//    var root;
//    var scripts = document.getElementsByTagName("script");
//    var i = scripts.length;
//    while (i--) {
//        var match = scripts[i].src.match(/(^|.*\/)MS1040\.js$/);
//        if (match) {
//            root = match[1];
//            break;
//        }
//    }
//    return root;
//}

//document.write("<script src=\"" + getPath() + "/tmqcommon.js\"></script>");
//document.write("<script src=\"" + getPath() + "/tmqmaster.js\"></script>");
////document.write("<script src=\"" + getPath() + "/SI0001.js\"></script>");  // 拡張項目の入力コントロールに選択画面がある場合、使用

// 機能ID
const ConductId_MS1040 = "MS1040";

//// 共通工場ID
//const CommonFactoryId = 0;

//// 画面タイプ
//const FormType = {
//    // 登録画面
//    Regist: 1,
//    // 修正画面
//    Edit: 2,
//}

// 一覧画面
const MS1040_FormList = {
    // 画面No
    No: 1,
    // 検索条件一覧
    SearchList: {
        Id: "BODY_000_00_LST_0",
        // 工場列
        FactoryId:1,
        // 予備品倉庫列
        SpareStrageId: 2,
        // コンボ生成用工場ID列(非表示列)
        HiddenFactoryId: 4,
        // コンボ生成用予備品倉庫ID列(非表示列)
        HiddenSpareStrageId: 5,
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
        // 工場名称列
        FactoryName: 1,
        // 予備品倉庫名称列
        SpareStrageName: 2,
        // アイテムID列
        ItemId: 3,
        // アイテム翻訳列
        ItemName: 4,
        // 表示順列
        Order: 21,
        // 未使用列
        Unused: 22,
        // 削除列
        Delete: 23,
        // 構成ID列
        StructureId: 24,
        // 工場ID列
        FactoryId: 25,
        // 構成グループID列
        StructureGroupId: 26,
        // アイテム翻訳ID列
        ItemTranId: 27,
        // 翻訳用工場ID列
        TranFactoryId: 28,
    //    // 削除列
    //    Delete: 22,
    //    // 構成ID列
    //    StructureId: 23,
    //    // 工場ID列
    //    FactoryId: 24,
    //    // 構成グループID列
    //    StructureGroupId: 25,
    //    // アイテム翻訳ID列
    //    ItemTranId: 26,
    },
    ButtonId: {
        // 検索
        Search: "Search",
        // 表示順変更
        Order: "Order",
        // 削除
        Delete: "Delete",
    },
//    SqlId:                                          // SqlId（コンボボックス）
//    {
//        Factory: "C0017",
//        OutputTemplate: "C0009",
//        OutputPattern: "C0010",
//    },
//    Param:                                          // Parameter（コンボボックス）
//    {
//        Factory: "@1,911070005,99,@9",
//        OutputTemplate: "@1,@2",
//        OutputPattern: "@1,@2,@3",
//    }
}

// 登録・修正画面
const MS1040_FormEdit = {
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
            HideCol: { ItemId: 3, ItemName: 4, Order: 21, Unused: 22, Delete: 23 },
        },
        // 修正画面
        Edit: {
            // 非表示列
            HideCol: { ItemId: 3, ItemName: 4, Order: 21, Unused: 22 },
        },
    },
    //// 工場アイテム情報一覧
    //FactoryItemList: {
    //    Id: "BODY_030_00_LST_1",
    //},
    //// アイテム情報一覧
    //ItemList: {
    //    // 登録画面
    //    Regist: {
    //        // 非表示列
    //        HideCol: { ItemId: 2, ItemName: 3, Order: 21, Delete: 22 },
    //    },
    //    // 修正画面
    //    Edit: {
    //        // 非表示列
    //        HideCol: { ItemId: 2, ItemName: 3, Order: 21 },
    //    },
    //},
    ButtonId: {
        // 登録
        Regist: "Regist",
        // キャンセル
        Cancel: "Back",
    },
}

// 表示順変更画面
const MS1040_FormOrder = {
    // 画面No
    No: 3,
    // アイテム一覧
    ItemList: {
        Id: "BODY_000_00_LST_2",
        // 非表示列
        HideCol: { Standard: 1 },
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

//======== 拡張項目の登録がないため、配列に空白を設定 ========
//// 拡張項目列
//const ItemExCol = { ExData1: 11, ExData2: 12, ExData3: 13, ExData4: 14, ExData5: 15 }
//// 拡張項目コントロール列(TextBox: 0, Label: 1, Combo: 2, ChkBox: 3, Link: 4, Input: 5, Textarea: 6, Password: 7, Button: 8)
//const ItemExCtrlCol = { ExData1: 0, ExData2: 2, ExData3: 2, ExData4: 2, ExData5: 2 }
// 拡張項目列
const ItemExCol_MS1040 = {}
// 拡張項目コントロール列(TextBox: 0, Label: 1, Combo: 2, ChkBox: 3, Link: 4, Input: 5, Textarea: 6, Password: 7, Button: 8)
const ItemExCtrlCol_MS1040 = {}
//============================================================

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
function initFormOriginalForMS1040(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data) {
    // 初期化処理（マスタメンテナンス用）
    //initFormOriginalForMaster(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data, ItemExCol, ItemExCtrlCol);

    //拡張項目がある場合、使用
    //    // アイテム検索画面用
    //    if (formNo == MasterFormEdit.No) {
    //        // 登録・修正画面

    //        // 工場ID取得
    //        var factoryId = getValueByOtherForm(MasterFormList.No, MasterFormList.HiddenList.Id, MasterFormList.HiddenList.FactoryId, 1, CtrlFlag.Label);
    //        // アイテム情報一覧の翻訳用工場IDに工場IDを設定する
    //        setValue(MasterFormEdit.ItemList.Id, MasterFormList.ItemList.TranFactoryId, 1, CtrlFlag.Label, factoryId)
    //    }
    if (formNo == MS1040_FormList.No) {
        // 一覧画面

        // 非表示情報を非表示
        changeListDisplay(MS1040_FormList.HiddenList.Id, false, false);

        // フォーカス設定
        setFocusDelay(MS1040_FormList.SearchList.Id, MS1040_FormList.SearchList.FactoryId, 0, CtrlFlag.Combo);

        // 初期検索時のみ初期値セット
        if (actionCtrlId == "") {
            // 本務工場を初期値にセット
            var factory = getValue(MS1040_FormList.HiddenList.Id, MS1040_FormList.HiddenList.DutyFactoryId, 1, CtrlFlag.Label, false, false);
            setValueAndTrigger(MS1040_FormList.SearchList.Id, MS1040_FormList.SearchList.FactoryId, 1, CtrlFlag.Combo, factory);
        } else if (actionCtrlId == MS1040_FormEdit.ButtonId.Regist) {
            // 登録処理後

            // 検索条件のコンボを再設定

            // 退避していた検索条件を取得
            var spareStrageId = getValue(MS1040_FormList.SearchList.Id, MS1040_FormList.SearchList.HiddenSpareStrageId, 1, CtrlFlag.Label);

            if (spareStrageId) {
                // 予備品倉庫が選択されている場合、退避した検索条件をコンボにセット
                setValue(MS1040_FormList.SearchList.Id, MS1040_FormList.SearchList.SpareStrageId, 1, CtrlFlag.Combo, spareStrageId);
            }

        }

        // 予備品倉庫コンボボックス変更時
        var spareStrageIdComb = getCtrl(MS1040_FormList.SearchList.Id, MS1040_FormList.SearchList.SpareStrageId, 1, CtrlFlag.Combo, false, false);
        $(spareStrageIdComb).change(function () {

            spareStrageIdCombChangeEventForMS1040(appPath);

        });

    } else if (formNo == MS1040_FormEdit.No) {
        // 登録・修正画面

        var ctrlId;
        var title;

        // 画面タイプ取得
        var formType = getValueByOtherForm(MS1040_FormList.No, MS1040_FormList.HiddenList.Id, MS1040_FormList.HiddenList.FormType, 1, CtrlFlag.Label);
        if (Number(formType) == MasterFormType.Regist) {
            // 登録画面の初期処理
            formRegistInitForMS1040(title, ctrlId);
        } else if (Number(formType) == MasterFormType.Edit) {
            // 修正画面の初期処理
            formEditInitForMS1040(title, ctrlId);
        }

    } else if (formNo == MS1040_FormOrder.No) {
        // 表示順変更画面

        // フォーカス設定
        setFocusButtonAvailable(MS1040_FormOrder.ButtonId.Regist, MS1040_FormOrder.ButtonId.Cancel);
    }
}

/**
 * 登録画面の初期処理
 *
 *  @title {string}     ：画面タイトル
 *  @ctrlId {string}    ：コントロールID
 */
function formRegistInitForMS1040(title, ctrlId) {

    // モーダル画面要素取得
    const modalNo = getCurrentModalNo(P_Article);
    var modalEle = getModalElement(modalNo);
    if (modalEle) {
        // タイトル要素取得
        var titleEle = $(modalEle).find('.title_div').find('span');
        // タイトル取得
        var title = $(titleEle).text();
        var structureLayerNo = getValueByOtherForm(MS1040_FormList.No, MS1040_FormList.HiddenList.Id, MS1040_FormList.HiddenList.StructureLayerNo, 1, CtrlFlag.Label);
        if (Number(structureLayerNo) == Structure.StructureLayerNo.Layer2) {
            title = P_ComMsgTranslated[111150016];
        } else {
            title = P_ComMsgTranslated[111160069];
        }
        // タイトル編集
        $(titleEle).text(title + P_ComMsgTranslated[111200003]);
    }

    // アイテムID一覧を非表示
    changeListDisplay(MS1040_FormEdit.ItemIdList.Id, false, false);
    // アイテム情報一覧の一部列を非表示
    $.each(MS1040_FormEdit.ItemList.Regist.HideCol, function (key, val) {
        //changeColumnDisplay(MS1040_FormEdit.ItemList.Id, val, false);
        var ctrlId = MasterFormEdit.ItemList.Id;
        var isDisplay = false;
        var column = $(P_Article).find("#" + ctrlId + getAddFormNo()).find("[data-name='VAL" + val + "']");
        // 表示状態切り替え
        changeCtrlDisplay(column, isDisplay);
    });

    // フォーカス設定
    setFocusDelay(MS1040_FormEdit.ItemTranList.Id, MS1040_FormEdit.ItemTranList.ItemName, 0, CtrlFlag.TextBox);
}

/**
 * 修正画面の初期処理
 *
 *  @title {string}     ：画面タイトル
 *  @ctrlId {string}    ：コントロールID
 */
function formEditInitForMS1040(title, ctrlId) {

    // モーダル画面要素取得
    const modalNo = getCurrentModalNo(P_Article);
    var modalEle = getModalElement(modalNo);
    if (modalEle) {
        // タイトル要素取得
        var titleEle = $(modalEle).find('.title_div').find('span');
        // タイトル取得
        var title = $(titleEle).text();
        var structureLayerNo = getValueByOtherForm(MS1040_FormList.No, MS1040_FormList.HiddenList.Id, MS1040_FormList.HiddenList.StructureLayerNo, 1, CtrlFlag.Label);
        if (Number(structureLayerNo) == Structure.StructureLayerNo.Layer2) {
            title = P_ComMsgTranslated[111150016];
        } else {
            title = P_ComMsgTranslated[111160069];
        }
        // タイトル編集
        $(titleEle).text(title + P_ComMsgTranslated[111120007]);
    }

    // 対象アイテム一覧取得
    var targetItemList = getValueByOtherForm(MS1040_FormList.No, MS1040_FormList.HiddenList.Id, MS1040_FormList.HiddenList.TargetItemList, 1, CtrlFlag.Label);
    if (Number(targetItemList) == MasterTargetItemList.Standard) {
        // 標準アイテムの場合

        // 工場ID取得
        var factoryId = getValueByOtherForm(MS1040_FormList.No, MS1040_FormList.HiddenList.Id, MS1040_FormList.HiddenList.FactoryId, 1, CtrlFlag.Label);
        if (factoryId == CommonFactoryId) {
            // 工場が未選択の場合

            //// 未使用を非表示
            //changeColumnDisplay(MS1040_FormEdit.ItemList.Id, MS1040_FormList.ItemList.Unused, false);
            // 未使用を非表示
            var ctrlId = MS1040_FormEdit.ItemList.Id;
            var isDisplay = false;
            var column = $(P_Article).find("#" + ctrlId + getAddFormNo()).find("[data-name='VAL" + MS1040_FormList.ItemList.Unused + "']");
            // 表示状態切り替え
            changeCtrlDisplay(column, isDisplay);
       } else {
            // 工場が選択済の場合

            // アイテム翻訳一覧の言語「標準アイテム」のアイテム翻訳をラベル化
            var standardItemTranEle = getCtrl(MS1040_FormEdit.ItemTranList.Id, MS1040_FormEdit.ItemTranList.ItemName, 0, CtrlFlag.TextBox);
            changeInputReadOnly(standardItemTranEle, false);

            //// 拡張項目をラベル化
            //$.each(itemExCol, function (key, val) {
            //    var itemExEle = getCtrl(FormEdit.ItemList.Id, val, 1, itemExCtrlCol[key]);
            //    changeInputReadOnly(itemExEle, false);
            //});

            // 削除をラベル化
            var deleteEle = getCtrl(MS1040_FormEdit.ItemList.Id, MS1040_FormList.ItemList.Delete, 1, CtrlFlag.ChkBox);
            changeInputReadOnly(deleteEle, false);
        }
    } else {
        // 工場アイテムの場合

        //// 未使用を非表示
        //changeColumnDisplay(MS1040_FormEdit.ItemList.Id, MS1040_FormList.ItemList.Unused, false);
        // 未使用を非表示
        var ctrlId = MS1040_FormEdit.ItemList.Id;
        var isDisplay = false;
        var column = $(P_Article).find("#" + ctrlId + getAddFormNo()).find("[data-name='VAL" + MS1040_FormList.ItemList.Unused + "']");
        // 表示状態切り替え
        changeCtrlDisplay(column, isDisplay);
    }

    // アイテム情報一覧の一部列を非表示
    $.each(MS1040_FormEdit.ItemList.Edit.HideCol, function (key, val) {
        //changeColumnDisplay(MS1040_FormEdit.ItemList.Id, val, false);
        var ctrlId = MS1040_FormEdit.ItemList.Id;
        var isDisplay = false;
        var column = $(P_Article).find("#" + ctrlId + getAddFormNo()).find("[data-name='VAL" + val + "']");
        // 表示状態切り替え
        changeCtrlDisplay(column, isDisplay);
    });

    // フォーカス設定
    setFocusDelay(MS1040_FormEdit.ItemTranList.Id, MS1040_FormEdit.ItemTranList.ItemName, 0, CtrlFlag.TextBox);

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
function setComboOtherValuesForMS1040(appPath, combo, datas, selected, formNo, ctrlId, valNo) {
    // 一覧画面の検索条件の工場コンボが変更された時、連動コンボを初期化する
    if (formNo == MS1040_FormList.No && ctrlId == MS1040_FormList.SearchList.Id && (valNo >= MS1040_FormList.SearchList.FactoryId && valNo <= MS1040_FormList.SearchList.FactoryId)) {
        // 予備品倉庫コンボボックスの再作成
        var tdFactory = $(P_Article).find("#" + MS1040_FormList.SearchList.Id + getAddFormNo()).find("td[data-name='VAL" + MS1040_FormList.SearchList.FactoryId + "']");
        var tdFactoryId = $(P_Article).find("#" + MS1040_FormList.SearchList.Id + getAddFormNo()).find("td[data-name='VAL" + MS1040_FormList.SearchList.HiddenFactoryId + "']");
        var factoryval = getCellVal(tdFactory);
        if (factoryval == null || factoryval == '') {
            // 工場コンボが未選択の場合、工場共通とする
            factoryval = '0';
        }
        $(tdFactoryId).text(factoryval);

        // 予備品倉庫ID(非表示列)の項目に空白を設定する
        setValue(MS1040_FormList.SearchList.Id, MS1040_FormList.SearchList.HiddenSpareStrageId, 1, CtrlFlag.Label, "", false, false);

        // 予備品倉庫コンボボックスの再作成
        var selects = $(P_Article).find("#" + MS1040_FormList.SearchList.Id + getAddFormNo()).find("td[data-name='VAL" + MS1040_FormList.SearchList.SpareStrageId + "'] select.dynamic");
        // 連動ｺﾝﾎﾞの選択ﾘｽﾄを再生成
        resetComboBox(appPath, selects);
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
function prevTransFormForMS1040(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element) {

    var conditionDataList = [];

    if (formNo == MS1040_FormList.No) {
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

            //// 非表示情報の設定
            //setValue(FormList.HiddenList.Id, FormList.HiddenList.FormType, 1, CtrlFlag.Label, formType);
            // 検索条件の工場ID取得
            var factoryId = getValue(MasterFormList.SearchList.Id, MasterFormList.SearchList.Factory, 1, CtrlFlag.Combo);
            if (!factoryId) {
                // 未選択の場合は、共通工場「0」を設定
                factoryId = CommonFactoryId;
            }

            // 対象アイテム一覧
            var targetItemList = (ctrlId == MasterFormList.StandardItemList.Id) ? MasterTargetItemList.Standard : MasterTargetItemList.Factory;

            // 非表示情報の設定
            setValue(MasterFormList.HiddenList.Id, MasterFormList.HiddenList.FactoryId, 1, CtrlFlag.Label, factoryId);
            setValue(MasterFormList.HiddenList.Id, MasterFormList.HiddenList.FormType, 1, CtrlFlag.Label, formType);
            setValue(MasterFormList.HiddenList.Id, MasterFormList.HiddenList.TargetItemList, 1, CtrlFlag.Label, targetItemList);

            // 対象の一覧のコントロールグループIDをctrlIdListへ設定する
            const ctrlIdList = [MS1040_FormList.HiddenList.Id];
            conditionDataList = getListDataByCtrlIdList(ctrlIdList, formNo, 0);

        } else if (btn_ctrlId == MS1040_FormList.ButtonId.Order) {
            // 表示順変更ボタン押下時

            // 対象の一覧のコントロールグループIDをctrlIdListへ設定する
            const ctrlIdList = [MS1040_FormList.HiddenList.Id];
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
function prevBackBtnProcessForMS1040(appPath, btnCtrlId, status, codeTransFlg) {
//    // 戻る処理の前(単票、子画面共用)（マスタメンテナンス用）
//    return prevBackBtnProcessForMaster(appPath, btnCtrlId, status, codeTransFlg);

    if (btnCtrlId == MS1040_FormEdit.ButtonId.Cancel) {
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

//===== 拡張項目の入力コントロールに選択画面がある場合、使用 =====
///**
// *【オーバーライド用関数】
// *  画面再表示ﾃﾞｰﾀ取得関数呼出前
// *
// *  @appPath {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
// *  @conductId {string} ：機能ID
// *  @pgmId {string} ：ﾌﾟﾛｸﾞﾗﾑID
// *  @formNo {number} ：画面番号
// *  @originNo {number} ：遷移元の親画面番号
// *  @btnCtrlId {number} ：ｱｸｼｮﾝﾎﾞﾀﾝのCTRLID
// *  @conductPtn {number} ：機能処理ﾊﾟﾀｰﾝ(10:入力、11：バッチ、20：帳票、30：マスタ)
// *  @selectData {number} ：NOﾘﾝｸ選択行のﾃﾞｰﾀ {List<Dictionary<string, object>>}
// *  @targetCtrlId {number} ：単票入力画面から戻る時、該当一覧のCTRLID
// *  @listData {} ：
// *  @codeTransFlg {int}    ：1:コード＋翻訳 選ボタンから画面遷移/1以外:それ以外
// *  @status  {CommonProcReturn} : 実行処理結果ｽﾃｰﾀｽ
// *  @param	{number}	selFlg : 共通機能から選択ボタン押下で戻った場合のみ、「1:selFlgDef.Selected」が渡る
// *  @param	{string}	backFrom : 共通機能からの戻る処理時、戻る前の共通機能ID ※他機能同タブ遷移でも使える？
// *  @param {number} transPtn ：画面遷移のパターン、transPtnDef
// */
//function beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom, transPtn) {
//    // 共通-アイテム検索画面を閉じたとき
//    SI0001_beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom);
//}
//=================================================================

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
function beforeSearchBtnProcessForMS1040(appPath, btn, conductId, pgmId, formNo, conductPtn) {
//    // 検索処理前（マスタメンテナンス用）
//    beforeSearchBtnProcessForMaster(appPath, btn, conductId, pgmId, formNo, conductPtn)
    if (formNo == MS1040_FormList.No) {
        // 一覧画面

        // 検索条件の工場ID取得
        var factoryId = getValue(MS1040_FormList.SearchList.Id, MS1040_FormList.SearchList.FactoryId, 1, CtrlFlag.Combo);
        if (!factoryId) {
            // 未選択の場合は、共通工場「0」を設定
            factoryId = CommonFactoryId;
        }
        // 検索条件の予備品倉庫ID取得
        var spareStrageId = getValue(MS1040_FormList.SearchList.Id, MS1040_FormList.SearchList.SpareStrageId, 1, CtrlFlag.Combo);


        // 非表示情報の設定
        setValue(MS1040_FormList.HiddenList.Id, MS1040_FormList.HiddenList.FactoryId, 1, CtrlFlag.Label, factoryId);

        // 工場アイテム一覧
        var detailArea = $(P_Article).find("#" + P_formDetailId);
        var factoryList = $(detailArea).find(".detail_div").find("#" + MasterFormList.FactoryItemList.Id + getAddFormNo() + "_div");

        // 工場アイテム一覧のみ表示(工場は必須選択のため)
        changeListDisplay(MS1040_FormList.StandardItemList.Id, false);
        $(factoryList).css('display', '');
        //changeListDisplay(MS1040_FormList.FactoryItemList.Id, true);
        // 階層
        var structureLayerNo = !spareStrageId ? Structure.StructureLayerNo.Layer2 : Structure.StructureLayerNo.Layer3;
        // 親構成ID
        var parentStrutureId = !spareStrageId ? factoryId : spareStrageId;

        // 非表示情報の設定
        setValue(MS1040_FormList.HiddenList.Id, MS1040_FormList.HiddenList.StructureLayerNo, 1, CtrlFlag.Label, structureLayerNo);
        setValue(MS1040_FormList.HiddenList.Id, MS1040_FormList.HiddenList.ParentStructureId, 1, CtrlFlag.Label, parentStrutureId);
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
function afterSearchBtnProcessForMS1040(appPath, btn, conductId, pgmId, formNo, conductPtn) {

    if (formNo == MS1040_FormList.No) {
        // 一覧画面

        // 階層取得
        var structureLayerNo = getValue(MS1040_FormList.HiddenList.Id, MS1040_FormList.HiddenList.StructureLayerNo, 1, CtrlFlag.Label);

        // 階層より一覧のタイトルを取得する
        var title = getLocationNameForMS1040(structureLayerNo);

        // 一覧タイトル編集

        // 一覧タイトル要素取得
        var titleEle = $(P_Article).find("#" + MS1040_FormList.FactoryItemList.Id + getAddFormNo() + "_div").find('.title');
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
function getLocationNameForMS1040(structureLayerNo) {

    if (Number(structureLayerNo) == Structure.StructureLayerNo.Layer2) {
        // 階層:2 倉庫
        return P_ComMsgTranslated[111150016];
    }
    if (Number(structureLayerNo) == Structure.StructureLayerNo.Layer3) {
        // 階層:3 棚
        return P_ComMsgTranslated[111160069];
    }

    return "";
}

//===== 拡張項目の入力コントロールに選択画面がある場合、使用 =====
///**
// *【オーバーライド用関数】
// *  共通機能用選択ボタン押下時処理
// *  @param {string}     appPath     :ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
// *  @param {string}     cmConductId :共通機能ID
// *  @param {article}    cmArticle   :共通機能articleタグ
// *  @param {button}     btn         :選択ボタン要素
// *  @param {string}     fromCtrlId  :共通機能遷移時に押下したﾎﾞﾀﾝ/一覧のｺﾝﾄﾛｰﾙID
// */
//function clickComConductSelectBtn(appPath, cmConductId, cmArticle, btn, fromCtrlId) {
//    // アイテム検索画面
//    SI0001_clickComConductSelectBtn(appPath, cmConductId, cmArticle, btn, fromCtrlId, ConductId_MS1040);
//}
//=================================================================

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

//===== 拡張項目の入力コントロールに選択画面がある場合、使用 =====
///**
// *【オーバーライド用関数】
// * 共通機能へデータを渡す
// * @param {string}                      appPath         :ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
// * @param {number}                      conductId       :共通機能ID
// * @param {number}                      parentNo        :親画面NO
// * @param {Array.<Dictionary<string, string>>}  conditionDataList   :条件ﾃﾞｰﾀ
// * @param {string}                      ctrlId          :遷移元の一覧ctrlid
// * @param {string}                      btn_ctrlId      :ボタンのbtn_ctrlid
// * @param {number}                      rowNo           :遷移元の一覧の選択行番号（一覧行でない場合は-1）
// * @param {Element}                     element         :ｲﾍﾞﾝﾄ発生要素
// * @param {string}                      parentConductId :遷移元の個別機能ID
// */
//function passDataCmConduct(appPath, conductId, parentNo, conditionDataList, ctrlId, btn_ctrlId, rowNo, element, parentConductId) {
//    // アイテム検索画面
//    SI0001_passDataCmConduct(appPath, conductId, parentNo, conditionDataList, ctrlId, btn_ctrlId, rowNo, element, parentConductId);
//}
//=================================================================

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
function addSearchConditionDictionaryForRegistForMS1040(appPath, conductId, formNo, btn) {
//    // 登録前追加条件取得処理（マスタメンテナンス用）
//    return addSearchConditionDictionaryForRegistForMaster(appPath, conductId, formNo, btn);

    var conditionDataList = [];

    var btnName = $(btn).attr("name");
    if (btnName == MS1040_FormEdit.ButtonId.Regist) {
        // 登録ボタン押下時

        // 対象の一覧のコントロールグループIDをctrlIdListへ設定する
        const ctrlIdList = [MS1040_FormList.HiddenList.Id];
        conditionDataList = getListDataByCtrlIdList(ctrlIdList, MS1040_FormList.No, 0);
    } else if (btnName == MS1040_FormList.ButtonId.Delete) {
        // 削除ボタン押下時

        // 対象の一覧のコントロールグループIDをctrlIdListへ設定する
        const ctrlIdList = [MS1040_FormList.SearchList.Id, MS1040_FormList.HiddenList.Id];
        conditionDataList = getListDataByCtrlIdList(ctrlIdList, MS1040_FormList.No, 0);
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
//function prevCreateTabulatorForMS1040(appPath, id, options, header, dispData) {
//    // Tabuator一覧の描画前処理 (マスタメンテナンス用)
//    prevCreateTabulatorForMaster(appPath, id, options, header, dispData)

////    // 表示順変更画面の一覧のレコードの並べ替えを可能にする
////    if (id == "#" + MS1040_FormOrder.ItemList.Id + getAddFormNo()) {

////        // 行移動可
////        options["movableRows"] = true;
////        if (!header.some(x => x.field === "moveRowBtn")) {
////            // 先頭に移動用のアイコン列追加
////            header.unshift({ title: "", rowHandle: true, field: "moveRowBtn", formatter: "handle", headerSort: false, frozen: true, width: 30, minWidth: 30 });
////        }
////    }
//}

/**
 * 【オーバーライド用関数】Tabulatorの描画が完了時の処理
 * @param {any} tbl 一覧
 * @param {any} id 一覧のID(#,_FormNo付き)
 */
function postBuiltTabulatorForMS1040(tbl, id) {
    //// Tabulatorの描画が完了時の処理 (マスタメンテナンス用)
    //postBuiltTabulatorForMaster(tbl, id);

    if (getFormNo() == MS1040_FormOrder.No) {
        // 表示順変更画面

        if (id == '#' + MS1040_FormOrder.ItemList.Id + getAddFormNo()) {
            // アイテム一覧の一部列を非表示
            $.each(MS1040_FormOrder.ItemList.HideCol, function (key, val) {
                changeTabulatorColumnDisplay(MS1040_FormOrder.ItemList.Id, val, false);
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
function prevInitFormDataForMS1040(appPath, formNo, btnCtrlId, conditionDataList, listDefines, pgmId) {
//    // 画面初期値データ取得前処理(表示中画面用) (マスタメンテナンス用)
//    return prevInitFormDataForMaster(appPath, formNo, btnCtrlId, conditionDataList, listDefines, pgmId);

    if (formNo != MS1040_FormList.No) {
        // 一覧画面以外の場合は終了
        return [conditionDataList, listDefines];
    }

    if ($(conditionDataList).length) {
        $.each($(conditionDataList), function (idx, conditionData) {
            if (conditionData.FORMNO == MS1040_FormList.No) {
                // 一覧画面
                if (conditionData.CTRLID == MS1040_FormList.SearchList.Id || conditionData.CTRLID == MS1040_FormList.HiddenList.Id) {
                    if (btnCtrlId != MS1040_FormEdit.ButtonId.Regist) {
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
 * 予備品倉庫コンボボックス変更時イベント
 * */
function spareStrageIdCombChangeEventForMS1040(appPath) {
    var comb = getCtrl(MS1040_FormList.SearchList.Id, MS1040_FormList.SearchList.SpareStrageId, 1, CtrlFlag.Combo, false, false);
    var spareStrageId = $(comb)[0][comb.selectedIndex].value;

    // 構成IDを非表示の項目に設定する
    setValue(MS1040_FormList.SearchList.Id, MS1040_FormList.SearchList.HiddenSpareStrageId, 1, CtrlFlag.Label, spareStrageId, false, false);
}