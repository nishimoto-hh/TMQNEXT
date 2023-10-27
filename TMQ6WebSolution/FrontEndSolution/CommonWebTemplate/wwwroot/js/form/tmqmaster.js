/* ========================================================================
 *  機能名　    ：   【tmqmaster】マスタメンテナンス共通
 * ======================================================================== */

//document.write("<script src=\"" + getPath() + "/tmqcommon.js\"></script>");

// 共通工場ID
const CommonFactoryId = 0;

// 機能ID
const ConductIdMS1000 = "MS1000";
const ConductIdMS1001 = "MS1001";
const ConductIdMS1010 = "MS1010";
const ConductIdMS1020 = "MS1020";
const ConductIdMS1040 = "MS1040";
const ConductIdMS1060 = "MS1060";
const ConductIdMS1130 = "MS1130";
const ConductIdMS1170 = "MS1170";
const ConductIdMS1210 = "MS1210";
const ConductIdMS1230 = "MS1230";
const ConductIdMS1240 = "MS1240";
const ConductIdMS1540 = "MS1540";
const ConductIdMS1600 = "MS1600";
const ConductIdMS1610 = "MS1610";
const ConductIdMS1620 = "MS1620";
const ConductIdMS1630 = "MS1630";
const ConductIdMS1640 = "MS1640";
const ConductIdMS1650 = "MS1650";
const ConductIdMS1660 = "MS1660";
const ConductIdMS1670 = "MS1670";
const ConductIdMS1680 = "MS1680";
const ConductIdMS1690 = "MS1690";
const ConductIdMS1700 = "MS1700";
const ConductIdMS1750 = "MS1750";
const ConductIdMS1780 = "MS1780";
const ConductIdMS1710 = "MS1710";
const ConductIdMS1720 = "MS1720";
const ConductIdMS1730 = "MS1730";
const ConductIdMS1740 = "MS1740";
const ConductIdMS1760 = "MS1760";
const ConductIdMS1770 = "MS1770";
const ConductIdMS1850 = "MS1850";
const ConductIdMS1940 = "MS1940";
const ConductIdMS1980 = "MS1980";
const ConductIdMS2010 = "MS2010";
const ConductIdMS2020 = "MS2020";
const ConductIdMS2040 = "MS2040";
const ConductIdMS2050 = "MS2050";
const ConductIdMS2070 = "MS2070";
const ConductIdMS2080 = "MS2080";
const ConductIdMS9020 = "MS9020";
const ConductIdMS0010 = "MS0010";
const ConductIdMS0020 = "MS0020";

// 画面タイプ
const MasterFormType = {
    // 登録画面
    Regist: 1,
    // 修正画面
    Edit: 2,
}

// アイテム一覧タイプ
const MasterItemListType = {
    // 標準アイテム一覧
    Standard: 1,
    // 工場アイテム一覧
    Factory: 2,
    // 標準・工場アイテム一覧
    StandardFactory: 3,
}

// 対象アイテム一覧
const MasterTargetItemList = {
    // 標準アイテム一覧
    Standard: 1,
    // 工場アイテム一覧
    Factory: 2,
}

// 構成
const Structure = {
    // 構成階層番号
    StructureLayerNo: {
        // 階層0
        Layer0: 0,
        // 階層1
        Layer1: 1,
        // 階層2
        Layer2: 2,
        // 階層3
        Layer3: 3,
        // 階層4
        Layer4: 4,
        // 階層5
        Layer5: 5,
    },
    // 親構成ID
    ParentStructureId: {
        // 最上位階層
        TopLayer : 0,
    },
}

// 一覧画面
const MasterFormList = {
    // 画面No
    No: 1,
    // 検索条件一覧
    SearchList: {
        Id: "BODY_000_00_LST_0",
        // 工場列
        Factory: 1,
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
        // 工場列
        FactoryName: 1,
        // アイテムID列
        ItemId: 2,
        // アイテム翻訳列
        ItemName: 3,
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
const MasterFormEdit = {
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
            HideCol: { ItemId: 2, ItemName: 3, Order: 21, Unused: 22, Delete: 23 },
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
const MasterFormOrder = {
    // 画面No
    No: 3,
    // アイテム一覧
    ItemList: {
        Id: "BODY_000_00_LST_2",
    },
    ButtonId: {
        // 登録
        Regist: "Regist",
        // キャンセル
        Cancel: "Back",
    },
}

// アイテム検索の構成グループID
const SearchItemStructureGroupID =
{
    Budget: 1060,  // 予算性格区分
}

/**
 * 　初期化処理(表示中画面用) (マスタメンテナンス用)
 *
 *  @appPath {string}   　　　　     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId {string} 　　　　     ：機能ID
 *  @formNo {byte}　　　　　　　     ：画面番号
 *  @articleForm {article要素}       ：表示画面ｴﾘｱ要素
 *  @curPageStatus {定数：pageStatus}：画面表示ｽﾃｰﾀｽ
 *  @actionCtrlId {string} 　　　　　：Action(ﾎﾞﾀﾝなど)CTRLID
 *  @data {List<Dictionary<string, object>>}    ：初期表示ﾃﾞｰﾀ
 *  @itemExCol {List<int>}           ：拡張項目列
 *  @itemExCtrlCol {List<int>}       ：拡張項目コントロール列
 */
function initFormOriginalForMaster(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data, itemExCol, itemExCtrlCol) {

    // 機能IDが「アイテム検索」の場合は何もしない
    if (conductId == SI0001_ConsuctId) {
        return;
    }

    if (formNo == MasterFormList.No) {
        // 一覧画面

        // システム管理者フラグ取得
        var systemAdminFlg = getValue(MasterFormList.HiddenList.Id, MasterFormList.HiddenList.SystemAdminFlg, 1, CtrlFlag.ChkBox);
        if (!systemAdminFlg) {
            // 工場管理者の場合

            // 検索条件の工場の要素取得
            var factory = getCtrl(MasterFormList.SearchList.Id, MasterFormList.SearchList.Factory, 1, CtrlFlag.Combo);
            // 検索条件の工場を必須設定
            $(factory).rules("add", "required");
        }

        // アイテム一覧タイプ取得
        var itemListType = getValueByOtherForm(MasterFormList.No, MasterFormList.HiddenList.Id, MasterFormList.HiddenList.ItemListType, 1, CtrlFlag.Label);

        if (itemListType == MasterItemListType.Standard) {
            // 標準アイテム一覧の場合

            // 検索条件の工場を非表示
            changeColumnDisplay(MasterFormList.SearchList.Id, MasterFormList.SearchList.Factory, false);
        }

        // 非表示情報を非表示
        changeListDisplay(MasterFormList.HiddenList.Id, false, false);

        // フォーカス設定
        setFocusDelay(MasterFormList.SearchList.Id, MasterFormList.SearchList.Factory, 0, CtrlFlag.Combo);

        // 初期検索時のみ初期値セット
        if (actionCtrlId == "") {
            // 対象アイテム一覧取得
            var itemListType = getValue(MasterFormList.HiddenList.Id, MasterFormList.HiddenList.ItemListType, 1, CtrlFlag.Label);
            // 工場アイテムがある場合
            if (itemListType != MasterItemListType.Standard) {
                // 本務工場を初期値にセット
                var factory = getValue(MasterFormList.HiddenList.Id, MasterFormList.HiddenList.DutyFactoryId, 1, CtrlFlag.Label, false, false);
                setValue(MasterFormList.SearchList.Id, MasterFormList.SearchList.Factory, 1, CtrlFlag.Combo, factory);
            }
        }
    } else if (formNo == MasterFormEdit.No) {
        // 登録・修正画面

        // 画面タイプ取得
        var formType = getValueByOtherForm(MasterFormList.No, MasterFormList.HiddenList.Id, MasterFormList.HiddenList.FormType, 1, CtrlFlag.Label);
        if (Number(formType) == MasterFormType.Regist) {
            // 登録画面の初期処理
            formRegistInitForMaster()
        } else if (Number(formType) == MasterFormType.Edit) {
            // 修正画面の初期処理
            formEditInitForMaster(itemExCol, itemExCtrlCol);
        }
    } else if (formNo == MasterFormOrder.No) {
        // 表示順変更画面

        // フォーカス設定
        setFocusButtonAvailable(MasterFormOrder.ButtonId.Regist, MasterFormOrder.ButtonId.Cancel);
    }
}

/**
 * 登録画面の初期処理
 */
function formRegistInitForMaster() {

    // モーダル画面要素取得
    const modalNo = getCurrentModalNo(P_Article);
    var modalEle = getModalElement(modalNo);
    if (modalEle) {
        // タイトル要素取得
        var titleEle = $(modalEle).find('.title_div').find('span');
        // タイトル取得
        var title = $(titleEle).text();
        // タイトル編集
        $(titleEle).text(title + P_ComMsgTranslated[111200003]);
    }

    // アイテムID一覧を非表示
    changeListDisplay(MasterFormEdit.ItemIdList.Id, false, false);
    // アイテム情報一覧の一部列を非表示
    $.each(MasterFormEdit.ItemList.Regist.HideCol, function (key, val) {
    //    changeColumnDisplay(MasterFormEdit.ItemList.Id, val, false);
        var ctrlId = MasterFormEdit.ItemList.Id;
        var isDisplay = false;
        var column = $(P_Article).find("#" + ctrlId + getAddFormNo()).find("[data-name='VAL" + val + "']");
        // 表示状態切り替え
        changeCtrlDisplay(column, isDisplay);
    });

    // フォーカス設定
    setFocusDelay(MasterFormEdit.ItemTranList.Id, MasterFormEdit.ItemTranList.ItemName, 0, CtrlFlag.TextBox);
}

/**
 * 修正画面の初期処理
 *
 *  @itemExCol {List<int>}           ：拡張項目列
 *  @itemExCtrlCol {List<int>}       ：拡張項目コントロール列
 */
function formEditInitForMaster(itemExCol, itemExCtrlCol) {

    // モーダル画面要素取得
    const modalNo = getCurrentModalNo(P_Article);
    var modalEle = getModalElement(modalNo);
    if (modalEle) {
        // タイトル要素取得
        var titleEle = $(modalEle).find('.title_div').find('span');
        // タイトル取得
        var title = $(titleEle).text();
        // タイトル編集
        $(titleEle).text(title + P_ComMsgTranslated[111120007]);
    }

    // 対象アイテム一覧取得
    var targetItemList = getValueByOtherForm(MasterFormList.No, MasterFormList.HiddenList.Id, MasterFormList.HiddenList.TargetItemList, 1, CtrlFlag.Label);
    if (Number(targetItemList) == MasterTargetItemList.Standard) {
        // 標準アイテムの場合

        // 工場ID取得
        var factoryId = getValueByOtherForm(MasterFormList.No, MasterFormList.HiddenList.Id, MasterFormList.HiddenList.FactoryId, 1, CtrlFlag.Label);
        if (factoryId == CommonFactoryId) {
            // 工場が未選択の場合

            //// 未使用を非表示
            //changeColumnDisplay(MasterFormEdit.ItemList.Id, MasterFormList.ItemList.Unused, false);
            // 未使用を非表示
            var ctrlId = MasterFormEdit.ItemList.Id;
            var isDisplay = false;
            var column = $(P_Article).find("#" + ctrlId + getAddFormNo()).find("[data-name='VAL" + MasterFormList.ItemList.Unused + "']");
            // 表示状態切り替え
            changeCtrlDisplay(column, isDisplay);
        } else {
            // 工場が選択済の場合

            // アイテム翻訳一覧の言語「標準アイテム」のアイテム翻訳をラベル化
            var standardItemTranEle = getCtrl(MasterFormEdit.ItemTranList.Id, MasterFormEdit.ItemTranList.ItemName, 0, CtrlFlag.TextBox);
            changeInputReadOnly(standardItemTranEle, false);

            // 拡張項目をラベル化
            $.each(itemExCol, function (key, val) {
                var itemExEle = getCtrl(MasterFormEdit.ItemList.Id, val, 1, itemExCtrlCol[key]);
                changeInputReadOnly(itemExEle, false);
            });

            // 削除をラベル化
            var deleteEle = getCtrl(MasterFormEdit.ItemList.Id, MasterFormList.ItemList.Delete, 1, CtrlFlag.ChkBox);
            changeInputReadOnly(deleteEle, false);
        }
    } else {
        // 工場アイテムの場合

        //// 未使用を非表示
        //changeColumnDisplay(MasterFormEdit.ItemList.Id, MasterFormList.ItemList.Unused, false);
        // 未使用を非表示
        var ctrlId = MasterFormEdit.ItemList.Id;
        var isDisplay = false;
        var column = $(P_Article).find("#" + ctrlId + getAddFormNo()).find("[data-name='VAL" + MasterFormList.ItemList.Unused + "']");
        // 表示状態切り替え
        changeCtrlDisplay(column, isDisplay);
    }

    // アイテム情報一覧の一部列を非表示
    $.each(MasterFormEdit.ItemList.Edit.HideCol, function (key, val) {
        //changeColumnDisplay(MasterFormEdit.ItemList.Id, val, false);
        var ctrlId = MasterFormEdit.ItemList.Id;
        var isDisplay = false;
        var column = $(P_Article).find("#" + ctrlId + getAddFormNo()).find("[data-name='VAL" + val + "']");
        // 表示状態切り替え
        changeCtrlDisplay(column, isDisplay);
   });

    // フォーカス設定
    setFocusDelay(MasterFormEdit.ItemTranList.Id, MasterFormEdit.ItemTranList.ItemName, 0, CtrlFlag.TextBox);
}

/**
 *  遷移処理の前 (マスタメンテナンス用)
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
function prevTransFormForMaster(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element) {
    var conditionDataList = [];

    if (formNo == MasterFormList.No) {
        // 一覧画面

        if (!btn_ctrlId) {
            // 行追加(+)アイコン または NOリンクがクリックされた場合

            var formType;
            var factoryId;

            if (rowNo < 0) {
                // 行追加(+)アイコンがクリックされた場合は登録画面を表示
                formType = MasterFormType.Regist;
            } else {
                // NOリンクがクリックされた場合は修正画面を表示
                formType = MasterFormType.Edit;
            }

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
            const ctrlIdList = [MasterFormList.HiddenList.Id];
            conditionDataList = getListDataByCtrlIdList(ctrlIdList, formNo, 0);

        } else if (btn_ctrlId == MasterFormList.ButtonId.Order) {
            // 表示順変更ボタン押下時

            // 対象の一覧のコントロールグループIDをctrlIdListへ設定する
            const ctrlIdList = [MasterFormList.HiddenList.Id];
            conditionDataList = getListDataByCtrlIdList(ctrlIdList, formNo, 0);
        }
    }

    return [true, conditionDataList];
}

/**
 *  戻る処理の前(単票、子画面共用) (マスタメンテナンス用)
 *
 *  @appPath        {string}    ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @btnCtrlId      {byte}      ：ボタンのCtrlId
 *  @codeTransFlg   {int}       ：1:コード＋翻訳 選ボタンから画面遷移/1以外:それ以外
 *  @return {bool} データ取得する場合はtrue、スキップする場合はfalse（子画面のみ）
 */
function prevBackBtnProcessForMaster(appPath, btnCtrlId, status, codeTransFlg) {

    if (btnCtrlId == MasterFormEdit.ButtonId.Cancel) {
        // キャンセルボタン押下時

        // 一覧画面へ戻る際、再描画は行わない
        return false;
    }

    return true;
}

/**
 * 検索処理前 (マスタメンテナンス用)
 *
 *  @appPath {string} 　：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @btn {string} 　　　：対象ボタン
 *  @conductId {string} ：機能ID
 *  @pgmId {string} 　　：プログラムID
 *  @formNo {number} 　 ：画面番号
 *  @conductPtn {number}：処理ﾊﾟﾀｰﾝ
 */
function beforeSearchBtnProcessForMaster(appPath, btn, conductId, pgmId, formNo, conductPtn) {

    if (formNo == MasterFormList.No) {
        // 一覧画面

        // 検索条件の工場ID取得
        var factoryId = getValue(MasterFormList.SearchList.Id, MasterFormList.SearchList.Factory, 1, CtrlFlag.Combo);
        if (!factoryId) {
            // 未選択の場合は、共通工場「0」を設定
            factoryId = CommonFactoryId;
        }

        // 非表示情報の設定
        setValue(MasterFormList.HiddenList.Id, MasterFormList.HiddenList.FactoryId, 1, CtrlFlag.Label, factoryId);

        // 工場アイテム一覧
        var detailArea = $(P_Article).find("#" + P_formDetailId);
        var factoryList = $(detailArea).find(".detail_div").find("#" + MasterFormList.FactoryItemList.Id + getAddFormNo() + "_div");

        // 工場アイテム一覧の表示切替
        if (factoryId == CommonFactoryId) {
            // 工場が未選択の場合は、一覧を非表示
            //setInitHide(factoryList, true);
            $(factoryList).css('display', 'none');
            // 一覧の選択列を表示
            changeRowControlOriginal(MasterFormList.StandardItemList.Id, true);
        } else {
            // 工場が選択済の場合は、一覧を表示
            //setInitHide(factoryList, false);
            $(factoryList).css('display', '');
            // 一覧の選択列を非表示
            changeRowControlOriginal(MasterFormList.StandardItemList.Id, false);
        }
    }
}

/**
 * 登録前追加条件取得処理 (マスタメンテナンス用)
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 *  @return {Dictionary<string, object>}  追加する条件リスト
 */
function addSearchConditionDictionaryForRegistForMaster(appPath, conductId, formNo, btn) {
    var conditionDataList = [];

    var btnName = $(btn).attr("name");
    if (btnName == MasterFormEdit.ButtonId.Regist) {
        // 登録ボタン押下時

        // 対象の一覧のコントロールグループIDをctrlIdListへ設定する
        const ctrlIdList = [MasterFormList.HiddenList.Id];
        conditionDataList = getListDataByCtrlIdList(ctrlIdList, MasterFormList.No, 0);
    } else if (btnName == MasterFormList.ButtonId.Delete) {
        // 削除ボタン押下時

        // 対象の一覧のコントロールグループIDをctrlIdListへ設定する
        const ctrlIdList = [MasterFormList.SearchList.Id, MasterFormList.HiddenList.Id];
        conditionDataList = getListDataByCtrlIdList(ctrlIdList, MasterFormList.No, 0);
    }

    return conditionDataList;
}

/**
 * Tabuator一覧の描画前処理 (マスタメンテナンス用)
 * @param {string} appPath  ：アプリケーションルートパス
 * @param {string} id       ：一覧のID(#,_FormNo付き)
 * @param {string} options  ： 一覧のオプション情報
 * @param {object} header   ：ヘッダー情報
 * @param {object} dispData ：データ
 */
function prevCreateTabulatorForMaster(appPath, id, options, header, dispData) {

    // 表示順変更画面の一覧のレコードの並べ替えを可能にする
    if (id == "#" + MasterFormOrder.ItemList.Id + getAddFormNo()) {
        // 行移動可
        options["movableRows"] = true;
        if (!header.some(x => x.field === "moveRowBtn")) {
            // 先頭に移動用のアイコン列追加
            header.unshift({ title: "", rowHandle: true, field: "moveRowBtn", formatter: "handle", headerSort: false, frozen: true, width: 30, minWidth: 30 });
        }
    }
}


/**
 * Tabulatorの描画が完了時の処理 (マスタメンテナンス用)
 * @param {any} tbl 一覧
 * @param {any} id 一覧のID(#,_FormNo付き)
 */
function postBuiltTabulatorForMaster(tbl, id) {

    if (getFormNo() == MasterFormList.No) {
        // 一覧画面

        if (id == '#' + MasterFormList.StandardItemList.Id + getAddFormNo()) {
            // 標準アイテム一覧

            // 工場ID取得
            var factoryId = getValue(MasterFormList.HiddenList.Id, MasterFormList.HiddenList.FactoryId, 1, CtrlFlag.Label);
            if (factoryId == CommonFactoryId) {
                // 工場が未選択の場合

                // 一覧の未使用列を非表示
                changeTabulatorColumnDisplay(MasterFormList.StandardItemList.Id, MasterFormList.ItemList.Unused, false);
                // 一覧の選択列を表示
                changeRowControlOriginal(MasterFormList.StandardItemList.Id, true);
            } else {
                // 工場が選択済の場合

                // 一覧の未使用列を表示
                changeTabulatorColumnDisplay(MasterFormList.StandardItemList.Id, MasterFormList.ItemList.Unused, true);
                // 一覧の選択列を非表示
                changeRowControlOriginal(MasterFormList.StandardItemList.Id, false);
            }

        } else if (id == '#' + MasterFormList.FactoryItemList.Id + getAddFormNo()) {
            // 工場アイテム一覧

            // 一覧の未使用列を非表示
            changeTabulatorColumnDisplay(MasterFormList.FactoryItemList.Id, MasterFormList.ItemList.Unused, false);
            if (tbl.getRows() == null || tbl.getRows().length <= 0) {
                //更新後、データ0件の場合に右側に余白が出来てしまうのでクリアする
                tbl.clearData();
            }
        }
    }
}

/*
* 一覧の行追加などと選択列の表示/非表示切替を行う
* @param ctrlId 一覧のコントロールID
* @param isDisplay 表示する場合True
*/
function changeRowControlOriginal(ctrlId, isDisplay) {
    // 行追加など
    var items = ["1221", "1231", "1241", "1242"];
    $.each(items, function (i, item) {
        //var target = $("#" + ctrlId + getAddFormNo() + "_div").find('[data-actionkbn=' + item + ']');
        var target = $(P_Article).find("#" + ctrlId + getAddFormNo() + "_div").find('[data-actionkbn=' + item + ']');
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
}

/**
 * 画面初期値データ取得前処理(表示中画面用) (マスタメンテナンス用)
 * @param {any} appPath ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 * @param {any} formNo 画面番号
 * @param {any} btnCtrlId ｱｸｼｮﾝﾎﾞﾀﾝのCTRLID
 * @param {any} conditionDataList 条件一覧要素
 * @param {any} listDefines 一覧の定義情報
 * @param {any} pgmId 遷移先のプログラムID
 */
function prevInitFormDataForMaster(appPath, formNo, btnCtrlId, conditionDataList, listDefines, pgmId) {

    if (formNo != MasterFormList.No) {
        // 一覧画面以外の場合は終了
        return [conditionDataList, listDefines];
    }

    if ($(conditionDataList).length) {
        $.each($(conditionDataList), function (idx, conditionData) {
            if (conditionData.FORMNO == MasterFormList.No) {
                // 一覧画面
                if (conditionData.CTRLID == MasterFormList.SearchList.Id || conditionData.CTRLID == MasterFormList.HiddenList.Id) {
                    if (btnCtrlId != MasterFormEdit.ButtonId.Regist) {
                        // 検索条件一覧、非表示情報一覧を定義情報に格納
                        listDefines.push(conditionData);
                    }
                }
            }
        });
    }

    return [conditionDataList, listDefines];
}