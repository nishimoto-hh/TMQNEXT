/* ========================================================================
 *  機能名　    ：   【MS1001】場所階層マスタ
 * ======================================================================== */

///**
// * 自身の相対パスを取得
// */
//function getPath() {
//    var root;
//    var scripts = document.getElementsByTagName("script");
//    var i = scripts.length;
//    while (i--) {
//        var match = scripts[i].src.match(/(^|.*\/)MS1001\.js$/);
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
const ConductId_MS1001 = "MS1001";

// 一覧画面
const MS1001_FormList = {
    // 画面No
    No: 1,
    // 検索条件一覧
    SearchList: {
        Id: "BODY_000_00_LST_0",
        // 工場列
        Factory: 1,
        // プラント列
        Plant: 2,
        // 系列列
        Series: 3,
        // 工程列
        Stroke: 4,
        // コンボ生成用工場ID列(非表示列)
        HiddenFactoryId: 6,
        // コンボ生成用プラントID列(非表示列)
        HiddenPlantId: 7,
        // コンボ生成用系列ID列(非表示列)
        HiddenSeriesId: 8,
        // コンボ生成用工程ID列(非表示列)
        HiddenStrokeId: 9
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
        DutyFactoryId: 10,
        // 本務工場地区構成ID列
        DutyFactoryLocation: 11
    },
    // アイテム一覧
    ItemList: {
        // アイテムID列
        ItemId: 6,
        // アイテム翻訳列
        ItemName: 7,
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
const MS1001_FormEdit = {
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
            HideCol: { ItemId: 6, ItemName: 7, Order: 21, Delete: 22 },
        },
        // 修正画面
        Edit: {
            // 非表示列
            HideCol: { ItemId: 6, ItemName: 7, Order: 21 },
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
const MS1001_FormOrder = {
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
//const ItemExCol = {}
//// 拡張項目コントロール列(TextBox: 0, Label: 1, Combo: 2, ChkBox: 3, Link: 4, Input: 5, Textarea: 6, Password: 7, Button: 8)
//const ItemExCtrlCol = {}

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
function initFormOriginalForMS1001(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data) {
    if (formNo == MS1001_FormList.No) {
        // 一覧画面

        // 標準アイテム一覧
        var detailArea = $(P_Article).find("#" + P_formDetailId);
        var standardList = $(detailArea).find(".detail_div").find("#" + MS1001_FormList.StandardItemList.Id + getAddFormNo() + "_div");
        // 標準アイテムを非表示
        //changeListDisplay(MS1001_FormList.StandardItemList.Id, false, false);
        $(standardList).css('display', 'none');
        // 非表示情報を非表示
        changeListDisplay(MS1001_FormList.HiddenList.Id, false, false);

        // フォーカス設定
        setFocusDelay(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.Factory, 0, CtrlFlag.Combo);

        // 初期検索時のみ初期値セット
        if (actionCtrlId == "") {
            // 本務工場を初期値にセット
            var factory = getValue(MS1001_FormList.HiddenList.Id, MS1001_FormList.HiddenList.DutyFactoryId, 1, CtrlFlag.Label, false, false);
            setValueAndTriggerNoChange(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.Factory, 1, CtrlFlag.Combo, factory);

        } else if (actionCtrlId == MS1001_FormEdit.ButtonId.Regist) {
            // 登録処理後

            // 検索条件のコンボを再設定

            // 退避していた検索条件を取得
            var factoryId = getValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenFactoryId, 1, CtrlFlag.Label);
            var plantId = getValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenPlantId, 1, CtrlFlag.Label);
            var seriesId = getValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenSeriesId, 1, CtrlFlag.Label);
            var strokeId = getValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenStrokeId, 1, CtrlFlag.Label);

            // 工場コンボの値セット＆changeイベント発生
            setValueAndTriggerNoChange(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.Factory, 1, CtrlFlag.Combo, factoryId);
            var cnt = 1;
            const waitSeconds = 500;
            if (plantId) {
                // プラントが選択されている場合、退避した検索条件をコンボにセット
                // プラントコンボの値セット＆changeイベント発生
                setTimeout(setValueAndTriggerNoChange, waitSeconds * cnt++, MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.Plant, 1, CtrlFlag.Combo, plantId);
            }
            if (seriesId) {
                // 系列が選択されている場合、退避した検索条件をコンボにセット
                // 系列コンボの値セット＆changeイベント発生
                setTimeout(setValueAndTriggerNoChange, waitSeconds * cnt++, MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.Series, 1, CtrlFlag.Combo, seriesId);
            }
            if (strokeId) {
                // 工程が選択されている場合、退避した検索条件をコンボにセット
                setTimeout(setValueAndTriggerNoChange, waitSeconds * cnt++, MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.Stroke, 1, CtrlFlag.Combo, strokeId);
            }

        }

    } else if (formNo == MS1001_FormEdit.No) {
        // 登録・修正画面
        var title;

        // 階層取得
        var structureLayerNo = getValueByOtherForm(MS1001_FormList.No, MS1001_FormList.HiddenList.Id, MS1001_FormList.HiddenList.StructureLayerNo, 1, CtrlFlag.Label);
        // 階層名称取得
        var title = getLocationNameForMS1001(structureLayerNo);

        // 画面タイプ取得
        var formType = getValueByOtherForm(MS1001_FormList.No, MS1001_FormList.HiddenList.Id, MS1001_FormList.HiddenList.FormType, 1, CtrlFlag.Label);
        if (Number(formType) == MasterFormType.Regist) {
            // 登録画面の初期処理
            formRegistInitForMS1001(title);
        } else if (Number(formType) == MasterFormType.Edit) {
            // 修正画面の初期処理
            formEditInitForMS1001(title);
        }

    } else if (formNo == MS1001_FormOrder.No) {
        // 表示順変更画面

        // フォーカス設定
        setFocusButtonAvailable(MS1001_FormOrder.ButtonId.Regist, MS1001_FormOrder.ButtonId.Cancel);
    }
}

/**
 * 登録画面の初期処理
 *
 *  @title {string}     ：画面タイトル
 */
function formRegistInitForMS1001(title) {

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
    changeListDisplay(MS1001_FormEdit.ItemIdList.Id, false, false);
    // アイテム情報一覧の一部列を非表示
    $.each(MS1001_FormEdit.ItemList.Regist.HideCol, function (key, val) {
        changeColumnDisplay(MS1001_FormEdit.ItemList.Id, val, false);
    });

    // フォーカス設定
    setFocusDelay(MS1001_FormEdit.ItemTranList.Id, MS1001_FormEdit.ItemTranList.ItemName, 0, CtrlFlag.TextBox);
}

/**
 * 修正画面の初期処理
 *
 *  @title {string}     ：画面タイトル
 */
function formEditInitForMS1001(title) {

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
    $.each(MS1001_FormEdit.ItemList.Edit.HideCol, function (key, val) {
        changeColumnDisplay(MS1001_FormEdit.ItemList.Id, val, false);
    });

    // フォーカス設定
    setFocusDelay(MS1001_FormEdit.ItemTranList.Id, MS1001_FormEdit.ItemTranList.ItemName, 0, CtrlFlag.TextBox);
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
function setComboOtherValuesForMS1001(appPath, combo, datas, selected, formNo, ctrlId, valNo) {
    // 一覧画面の検索条件のコンボが変更された時、連動コンボを初期化する
    if (formNo == MS1001_FormList.No && ctrlId == MS1001_FormList.SearchList.Id) {
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
function prevTransFormForMS1001(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element) {

    var conditionDataList = [];

    if (formNo == MS1001_FormList.No) {
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
            setValue(MS1001_FormList.HiddenList.Id, MS1001_FormList.HiddenList.FormType, 1, CtrlFlag.Label, formType);

            // 対象の一覧のコントロールグループIDをctrlIdListへ設定する
            const ctrlIdList = [MS1001_FormList.HiddenList.Id];
            conditionDataList = getListDataByCtrlIdList(ctrlIdList, formNo, 0);

        } else if (btn_ctrlId == MS1001_FormList.ButtonId.Order) {
            // 表示順変更ボタン押下時

            // 対象の一覧のコントロールグループIDをctrlIdListへ設定する
            const ctrlIdList = [MS1001_FormList.HiddenList.Id];
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
function prevBackBtnProcessForMS1001(appPath, btnCtrlId, status, codeTransFlg) {

    if (btnCtrlId == MS1001_FormEdit.ButtonId.Cancel) {
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
function beforeSearchBtnProcessForMS1001(appPath, btn, conductId, pgmId, formNo, conductPtn) {

    if (formNo == MS1001_FormList.No) {
        // 一覧画面

        // 検索条件より検索結果一覧に表示する階層、親構成IDを取得する
        var existFlg = false;
        var structureLayerNo;
        var parentStrutureId;

        // 検索条件取得
        var factoryId = getValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.Factory, 1, CtrlFlag.Combo);
        var plantId = getValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.Plant, 1, CtrlFlag.Combo);
        var seriesId = getValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.Series, 1, CtrlFlag.Combo);
        var strokeId = getValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.Stroke, 1, CtrlFlag.Combo);

        // 検索条件の退避情報をクリア
        setValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenFactoryId, 1, CtrlFlag.Label, "");
        setValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenPlantId, 1, CtrlFlag.Label, "");
        setValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenSeriesId, 1, CtrlFlag.Label, "");
        setValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenStrokeId, 1, CtrlFlag.Label, "");

        if (strokeId) {
            // 階層:5(設備)、親構成ID:工程ID
            structureLayerNo = Structure.StructureLayerNo.Layer5;
            parentStrutureId = strokeId;
            // 検索条件を退避
            setValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenFactoryId, 1, CtrlFlag.Label, factoryId);
            setValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenPlantId, 1, CtrlFlag.Label, plantId);
            setValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenSeriesId, 1, CtrlFlag.Label, seriesId);
            setValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenStrokeId, 1, CtrlFlag.Label, strokeId);
            existFlg = true;
        }
        if (!existFlg && seriesId) {
            // 階層:4(工程)、親構成ID:系列ID
            structureLayerNo = Structure.StructureLayerNo.Layer4;
            parentStrutureId = seriesId;
            // 検索条件を退避
            setValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenFactoryId, 1, CtrlFlag.Label, factoryId);
            setValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenPlantId, 1, CtrlFlag.Label, plantId);
            setValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenSeriesId, 1, CtrlFlag.Label, seriesId);
            existFlg = true;
        }
        if (!existFlg && plantId) {
            // 階層:3(系列)、親構成ID:プラントID
            structureLayerNo = Structure.StructureLayerNo.Layer3;
            parentStrutureId = plantId;
            // 検索条件を退避
            setValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenFactoryId, 1, CtrlFlag.Label, factoryId);
            setValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenPlantId, 1, CtrlFlag.Label, plantId);
            existFlg = true;
        }
        if (!existFlg && factoryId) {
            // 階層:2(プラント)、親構成ID:工場ID
            structureLayerNo = Structure.StructureLayerNo.Layer2;
            parentStrutureId = factoryId;
            // 検索条件を退避
            setValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenFactoryId, 1, CtrlFlag.Label, factoryId);
            existFlg = true;
        }

        // 非表示情報の設定
        setValue(MS1001_FormList.HiddenList.Id, MS1001_FormList.HiddenList.FactoryId, 1, CtrlFlag.Label, factoryId);
        setValue(MS1001_FormList.HiddenList.Id, MS1001_FormList.HiddenList.StructureLayerNo, 1, CtrlFlag.Label, structureLayerNo);
        setValue(MS1001_FormList.HiddenList.Id, MS1001_FormList.HiddenList.ParentStructureId, 1, CtrlFlag.Label, parentStrutureId);
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
function afterSearchBtnProcessForMS1001(appPath, btn, conductId, pgmId, formNo, conductPtn) {

    if (formNo == MS1001_FormList.No) {
        // 一覧画面

        // 階層取得
        var structureLayerNo = getValue(MS1001_FormList.HiddenList.Id, MS1001_FormList.HiddenList.StructureLayerNo, 1, CtrlFlag.Label);

        // 階層より一覧のタイトルを取得する
        var title = getLocationNameForMS1001(structureLayerNo);

        // 一覧タイトル編集

        // 一覧タイトル要素取得
        var titleEle = $(P_Article).find("#" + MS1001_FormList.FactoryItemList.Id + getAddFormNo() + "_div").find('.title');
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
function getLocationNameForMS1001(structureLayerNo) {

    // 階層より階層名称を取得する
    if (structureLayerNo == Structure.StructureLayerNo.Layer2) {
        // 階層:2 プラント
        return P_ComMsgTranslated[111280011];
    }
    if (structureLayerNo == Structure.StructureLayerNo.Layer3) {
        // 階層:3 系列
        return P_ComMsgTranslated[111090018];
    }
    if (structureLayerNo == Structure.StructureLayerNo.Layer4) {
        // 階層:4 工程
        return P_ComMsgTranslated[111100013];
    }
    if (structureLayerNo == Structure.StructureLayerNo.Layer5) {
        // 階層:5 設備
        return P_ComMsgTranslated[111140018];
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
function addSearchConditionDictionaryForRegistForMS1001(appPath, conductId, formNo, btn) {

    var conditionDataList = [];

    var btnName = $(btn).attr("name");
    if (btnName == MS1001_FormEdit.ButtonId.Regist) {
        // 登録ボタン押下時

        // 対象の一覧のコントロールグループIDをctrlIdListへ設定する
        const ctrlIdList = [MS1001_FormList.HiddenList.Id];
        conditionDataList = getListDataByCtrlIdList(ctrlIdList, MS1001_FormList.No, 0);
    } else if (btnName == MS1001_FormList.ButtonId.Delete) {
        // 削除ボタン押下時

        // 対象の一覧のコントロールグループIDをctrlIdListへ設定する
        const ctrlIdList = [MS1001_FormList.SearchList.Id, MS1001_FormList.HiddenList.Id];
        conditionDataList = getListDataByCtrlIdList(ctrlIdList, MS1001_FormList.No, 0);
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
//function prevCreateTabulatorForMS1001(appPath, id, options, header, dispData) {

//    // 表示順変更画面の一覧のレコードの並べ替えを可能にする
//    if (id == "#" + MS1001_FormOrder.ItemList.Id + getAddFormNo()) {

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
function postBuiltTabulatorForMS1001(tbl, id) {

    if (getFormNo() == MS1001_FormOrder.No) {
        // 表示順変更画面

        if (id == '#' + MS1001_FormOrder.ItemList.Id + getAddFormNo()) {
            // アイテム一覧の一部列を非表示
            $.each(MS1001_FormOrder.ItemList.HideCol, function (key, val) {
                changeTabulatorColumnDisplay(MS1001_FormOrder.ItemList.Id, val, false);
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
function prevInitFormDataForMS1001(appPath, formNo, btnCtrlId, conditionDataList, listDefines, pgmId) {

    if (formNo != MS1001_FormList.No) {
        // 一覧画面以外の場合は終了
        return [conditionDataList, listDefines];
    }

    if ($(conditionDataList).length) {
        $.each($(conditionDataList), function (idx, conditionData) {
            if (conditionData.FORMNO == MS1001_FormList.No) {
                // 一覧画面
                if (conditionData.CTRLID == MS1001_FormList.SearchList.Id || conditionData.CTRLID == MS1001_FormList.HiddenList.Id) {
                    if (btnCtrlId != MS1001_FormEdit.ButtonId.Regist) {
                        // 検索条件一覧、非表示情報一覧を定義情報に格納
                        listDefines.push(conditionData);
                    }
                }
            }
        });
    }

    return [conditionDataList, listDefines];
}

///**
// * 工場コンボボックス変更時イベント
// * */
//function factoryCombChangeEventForMS1001(appPath) {
//    var comb = getCtrl(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.Factory, 1, CtrlFlag.Combo, false, false);
//    var factoryId = $(comb)[0][comb.selectedIndex].value;

//    // 構成IDを非表示の項目に設定する
//    setValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenFactoryId, 1, CtrlFlag.Label, factoryId, false, false);

//    // プラントコンボボックス初期化
//    var selects = $(P_Article).find("#" + MS1001_FormList.SearchList.Id + getAddFormNo()).find("td[data-name='VAL" + MS1001_FormList.SearchList.Plant + "'] select.dynamic");
//    //initComboBox(appPath, selects, 'C0001', '1000,2,@6,@9999', 2, false, -1, null, [factoryId]);
//    initComboBox(
//        appPath,
//        selects,
//        selects[0].getAttribute("data-sqlid"),
//        selects[0].getAttribute("data-param"),
//        selects[0].getAttribute("data-option"),
//        selects[0].getAttribute("data-nullcheck"),
//        -1,
//        null,
//        [factoryId]
//    );

//    // 系列コンボ一覧を初期化
//    var series = getCtrl(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.Series, 1, CtrlFlag.Combo, false, false);
//    $(series).children().remove();

//    // 工程コンボ一覧を初期化
//    var stroke = getCtrl(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.Stroke, 1, CtrlFlag.Combo, false, false);
//    $(stroke).children().remove();

//    // プラント、系列、工程(非表示列)の項目に空白を設定する
//    setValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenPlantId, 1, CtrlFlag.Label, "", false, false);
//    setValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenSeriesId, 1, CtrlFlag.Label, "", false, false);
//    setValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenStrokeId, 1, CtrlFlag.Label, "", false, false);
//}

///**
// * プラントコンボボックス変更時イベント
// * */
//function plantCombChangeEventForMS1001(appPath) {
//    var comb = getCtrl(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.Plant, 1, CtrlFlag.Combo, false, false);
//    var plantId = $(comb)[0][comb.selectedIndex].value;

//    // 構成IDを非表示の項目に設定する
//    setValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenPlantId, 1, CtrlFlag.Label, plantId, false, false);

//    // 系列コンボボックス初期化
//    var selects = $(P_Article).find("#" + MS1001_FormList.SearchList.Id + getAddFormNo()).find("td[data-name='VAL" + MS1001_FormList.SearchList.Series + "'] select.dynamic");
//    //initComboBox(appPath, selects, 'C0001', '1000,3,@7,@9999', 2, false, -1, null, [getValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.Factory, 1, CtrlFlag.Combo, false, false)]);
//    initComboBox(
//        appPath,
//        selects,
//        selects[0].getAttribute("data-sqlid"),
//        selects[0].getAttribute("data-param"),
//        selects[0].getAttribute("data-option"),
//        selects[0].getAttribute("data-nullcheck"),
//        -1,
//        null,
//        [getValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.Factory, 1, CtrlFlag.Combo, false, false)]
//    );

//    // 工程コンボボックスのアイテムを削除
//    var stroke = getCtrl(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.Stroke, 1, CtrlFlag.Combo, false, false);
//    $(stroke).children().remove();

//    // 系列、工程(非表示列)の項目に空白を設定する
//    setValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenSeriesId, 1, CtrlFlag.Label, "", false, false);
//    setValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenStrokeId, 1, CtrlFlag.Label, "", false, false);
//}

///**
// * 系列コンボボックス変更時イベント
// * */
//function seriesCombChangeEventForMS1001(appPath) {
//    var comb = getCtrl(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.Series, 1, CtrlFlag.Combo, false, false);
//    var seriesId = $(comb)[0][comb.selectedIndex].value;

//    // 構成IDを非表示の項目に設定する
//    setValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenSeriesId, 1, CtrlFlag.Label, seriesId, false, false);

//    // 工程コンボボックス初期化
//    var selects = $(P_Article).find("#" + MS1001_FormList.SearchList.Id + getAddFormNo()).find("td[data-name='VAL" + MS1001_FormList.SearchList.Stroke + "'] select.dynamic");
//    //initComboBox(appPath, selects, 'C0001', '1000,4,@8,@9999', 2, false, -1, null, [getValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.Factory, 1, CtrlFlag.Combo, false, false)]);
//    initComboBox(
//        appPath,
//        selects,
//        selects[0].getAttribute("data-sqlid"),
//        selects[0].getAttribute("data-param"),
//        selects[0].getAttribute("data-option"),
//        selects[0].getAttribute("data-nullcheck"),
//        -1,
//        null,
//        [getValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.Factory, 1, CtrlFlag.Combo, false, false)]
//    );

//    // 工程(非表示列)の項目に空白を設定する
//    setValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenStrokeId, 1, CtrlFlag.Label, "", false, false);
//}

///**
// * 工程コンボボックス変更時イベント
// * */
//function strokeCombChangeEventForMS1001(appPath) {
//    var comb = getCtrl(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.Stroke, 1, CtrlFlag.Combo, false, false);
//    var strokeId = $(comb)[0][comb.selectedIndex].value;

//    // 構成IDを非表示の項目に設定する
//    setValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenStrokeId, 1, CtrlFlag.Label, strokeId, false, false);
//}

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
function postRegistProcessForMS1001(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data) {

    // 選択行削除処理後

    // 検索条件のコンボを再設定

    // 退避していた検索条件を取得
    var factoryId = getValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenFactoryId, 1, CtrlFlag.Label);
    var plantId = getValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenPlantId, 1, CtrlFlag.Label);
    var seriesId = getValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenSeriesId, 1, CtrlFlag.Label);
    var strokeId = getValue(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.HiddenStrokeId, 1, CtrlFlag.Label);

    // 工場コンボの値セット＆changeイベント発生
    setValueAndTriggerNoChange(MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.Factory, 1, CtrlFlag.Combo, factoryId);
    var cnt = 1;
    const waitSeconds = 1000;
    if (plantId) {
        // プラントが選択されている場合、退避した検索条件をコンボにセット
        // プラントコンボの値セット＆changeイベント発生
        setTimeout(setValueAndTriggerNoChange, waitSeconds * cnt++, MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.Plant, 1, CtrlFlag.Combo, plantId);
    }
    if (seriesId) {
        // 系列が選択されている場合、退避した検索条件をコンボにセット
        // 系列コンボの値セット＆changeイベント発生
        setTimeout(setValueAndTriggerNoChange, waitSeconds * cnt++, MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.Series, 1, CtrlFlag.Combo, seriesId);
    }
    if (strokeId) {
        // 工程が選択されている場合、退避した検索条件をコンボにセット
        setTimeout(setValueAndTriggerNoChange, waitSeconds * cnt++, MS1001_FormList.SearchList.Id, MS1001_FormList.SearchList.Stroke, 1, CtrlFlag.Combo, strokeId);
    }
}