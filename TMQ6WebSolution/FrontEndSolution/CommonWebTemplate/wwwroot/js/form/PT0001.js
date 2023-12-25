/* ========================================================================
 *  機能名　    ：   【PT0001】予備品一覧
 * ======================================================================== */
/**
 * 自身の相対パスを取得
 */
function getPath() {
    var root;
    var scripts = document.getElementsByTagName("script");
    var i = scripts.length;
    while (i--) {
        var match = scripts[i].src.match(/(^|.*\/)PT0001\.js$/);
        if (match) {
            root = match[1];
            break;
        }
    }
    return root;
}

document.write("<script src=\"" + getPath() + "/tmqcommon.js\"></script>");
document.write("<script src=\"" + getPath() + "/DM0002.js\"></script>"); // 文書管理
document.write("<script src=\"" + getPath() + "/PT0005.js\"></script>"); // 入庫入力
document.write("<script src=\"" + getPath() + "/PT0006.js\"></script>"); // 出庫入力
document.write("<script src=\"" + getPath() + "/PT0007.js\"></script>"); // 移庫入力
document.write("<script src=\"" + getPath() + "/RM0001.js\"></script>"); // 帳票出力

//機能ID
const ConductId_PT0001 = "PT0001";

// 一覧画面 コントロール項目番号
const FormList = {
    No: 0,                                    // 画面番号
    FilterId: "BODY_010_00_LST_0",            // フィルタ機能一覧ID
    Filter: 1,                                // フィルタの項目番号
    Id: "BODY_020_00_LST_0",                  // 予備品一覧
    PartsId: 38,                              // 予備品ID(非表示)
    OrderAlert: {                             // 発注アラート
        CtrlNo: 32,                           // 項目番号
        Value: "Y",                           // 判定文字
        CssClass: "orderAlert"                // CSSクラス
    },
    CssClassImageHeight: "imageHeght",        // 画像の高さを指定するCSSクラス
    Button: {
        New: "New",                           // 新規
        Output: "Output",                     // 出力
        Label: "Label",                       // ラベル出力
        Enter: "Enter",                       // 入庫
        Issue: "Issue",                       // 出庫
        Move: "Move"                          // 移庫
    }
};

// 詳細画面 コントロール項目番号
const FormDetail = {
    No: 1,                                    // 画面番号
    PartsInfo: "BODY_010_00_LST_1",           // 予備品情報
    LocationInfo: "BODY_020_00_LST_1",        // 標準保管場所情報
    PurchaseInfo: {                           // 購買管理工場
        Id: "BODY_040_00_LST_1",              // 一覧ID
        IsRegistedRequiredItemToOutLabel: 10  // 詳細画面上部のラベル出力ボタン用フラグ(予備品情報「標準棚番」「標準部門」「標準勘定科目」がすべて登録されている場合はTrue)
    },
    InventoryParentList: {                    // 棚別在庫一覧(親一覧)
        Id: "BODY_080_00_LST_1",              // 一覧ID
        Key: 3                                // 入れ子に使用する項目番号
    },
    InventoryChildList: {                     // 棚別在庫一覧(子一覧)
        Id: "BODY_090_00_LST_1",              // 一覧ID
        Key: 11                               // 入れ子に使用する項目番号
    },
    CategoryParentList: {                     // 部門別在庫一覧(親一覧)
        Id: "BODY_100_00_LST_1",              // 一覧ID
        Key: 8                                // 入れ子に使用する項目番号
    },
    CategoryChildList: {                      // 部門別在庫一覧(子一覧)
        Id: "BODY_110_00_LST_1",              // 一覧ID
        Key: 7                                // 入れ子に使用する項目番号
    },
    DispCondition: {                          // 表示条件(入出庫履歴タブ)
        Id: "BODY_120_00_LST_1",              // 一覧ID
        No: 1,                                // 年度FromTo
        HideFrom: 2,                          // 年度From(非表示)
        HideTo: 3                             // 年度To(非表示)
    },
    InoutHistory: {                           // 入出庫履歴一覧
        Id: "BODY_140_00_LST_1",              // 一覧ID
        HistoryEnter: 1,                      // 入庫ボタン
        HistoryIssue: 2,                      // 出庫ボタン
        HistoryShelf: 3,                      // 棚番移庫ボタン
        HistoryCategory: 4,                   // 部門移庫ボタン
        InoutDivision: 5,                     // 入出庫区分
        PartsId: 16,                          // 予備品ID
        InoutHistoryId: 17,                   // 受払履歴ID
        WorkNo: 18,                           // 作業No
        CarryLinkNone: "linkDisable",         // 繰越のデータのNo.リンクを無効にするCSSクラス
        ValueCarryCssStyle: "black"           // 繰越のデータのNo.リンクの文字色スタイル
    },
    LocationDetailNo: 5,                      // 棚枝番
    PartsId: 9,                               // 非表示で定義している予備品IDの項目番号
    FactoryId: 13,                            // 非表示で定義している地区ID(管理工場の地区)の項目番号
    Button: {
        Edit: "Edit",                         // 修正
        Copy: "Copy",                         // 複写
        Image: "Image",                       // 画像
        Document: "Document",                 // 添付
        Map: "Map",                           // 予備品地図
        ShelfInventory: "ShelfInventory",     // 棚別在庫一覧
        DepartInventory: "DepartInventory",   // 部門別在庫一覧
        changeDispCssCls: "clickedButton",    // 棚別在庫一覧、部門別在庫一覧表示切替時のボタン色のスタイル
        reDisp: "ReDisp",                     // 再表示
        HistoryEnter: "HistoryEnter",         // 入庫(入出庫履歴)
        HistoryIssue: "HistoryIssue",         // 出庫(入出庫履歴)
        HistoryShelf: "HistoryShelf",         // 棚番移庫(入出庫履歴)
        HistoryCategory: "HistoryCategory",   // 部門移庫(入出庫履歴)
        OutputLabelDetail: "OutputLabelDetail", // ラベル出力(画面上部)
        OutputLabelDetailShed: "OutputLabelDetailShed" // ラベル出力(棚別在庫情報)
    },
    TabNo:                                    // タブ番号
    {
        InventoryInfo: 1,                     // 在庫情報タブ
        InoutHistory: 2                       // 入出庫履歴タブ
    }
};

// 詳細編集画面 コントロール項目番号
const FormEdit = {
    No: 2,                                    // 画面番号
    PartsInfo: "BODY_010_00_LST_2",           // 予備品情報
    DefalutLocation: {                        // 標準保管場所情報(地区～倉庫)
        Id: "BODY_030_00_LST_2",
        District: 1,                          // 地区
        Factory: 2,                           // 標準工場
        WareHouse: 3,                         // 標準予備品倉庫
    },
    Location: {                               // 標準保管場所情報
        Id: "BODY_040_00_LST_2",
        InputLocationId: 1,                   // 入力された棚番ID
        LocationId: 2,                        // 標準棚番の構成IDを設定する項目
        WareHouseId: 3,                       // 予備品倉庫の構成ID
        LayerNo: 4                            // 棚IDの階層番号
    },
    DetailNo: "BODY_070_00_LST_2",            // 棚枝番
    JoinString: "BODY_080_00_LST_2",          // 棚枝番
    PartsNo: 1,                               // 予備品No
    PartsName: 6,                             // 予備品名
    ManufacureCode: 2,                        // メーカー
    Factory: 4,                               // 管理工場
    UseId: 5,                                 // 使用区分
    JobId: 9,                                 // 職種
    HideJobId: 13,                            // 職種ID(非表示)
    ManufactureStructureId: 14,               // メーカーの構成IDを設定する項目
    DepartmentCode: 15,                       // 予備品情報の標準部門
    DepartmentStructureId: 16,                // 予備品情報の標準部門(構成ID)
    Button: {
        Regist: "Regist",                     // 登録
    },
    Purchase: {                               // 購買管理情報
        Id: "BODY_050_00_LST_2",
        LeadTime: 1,                          // 発注点
        NumUnit: 2,                           // 数量管理単位
        CurUnit: 3,                           // 金額管理単位
        OrderQuantity: 5,                     // 発注量
        Vender: 6,                            // 標準仕入先
        DefaultPrice: 7,                      // 標準単価
        VenderStructureId: 8,                 // 標準仕入先(構成ID)
        RoundDivision: 9,                     // 丸め処理区分
        AccountCode: 10,                      // 標準勘定科目
        AccountStructureId: 11               // 標準勘定科目(構成ID)              
    }
};

// ラベル出力画面 コントロール項目番号
const FormLabel = {
    No: 3,                                    // 画面番号
    SubjectList: "BODY_000_00_LST_3",         // 勘定科目選択一覧
    CategoryList: "BODY_010_00_LST_3",        // 部門選択一覧
    PartsIdList: "BODY_020_00_LST_3"          // 予備品ID一覧
};

// RFタグ取込画面
const FormRFUpload = {
    //画面No
    No: 4,
    //ファイルアップロード
    Info: {
        Id: "BODY_010_00_LST_4",
        //エラー出力
        ErrorMessage: 2,
    },
    //非表示
    Hide: {
        Id: "BODY_020_00_LST_4",
        //エラー出力
        Flg: 1,
    },
    Button: {
        Upload: "Upload",
        Back: "BackUpload",
    }
};
// 入出庫区分コンボボックスの拡張アイテム番号
const InoutDivisionNo =
{
    Carry: "0",          // 繰越
    Enter: "1",          // 入庫
    Issue: "2",          // 出庫
    Subject: "3",        // 棚番移庫
    Department: "4",     // 部門移庫
    InventoryEnter: "5", // 棚卸入庫
    InventoryIssue: "6"  // 棚卸出庫
};

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

    // 共通-文書管理詳細画面の初期化処理
    DM0002_initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data);

    // 共通-入庫入力画面の初期化処理
    PT0005_initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data);

    // 共通-出庫入力画面の初期化処理
    PT0006_initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data);

    // 共通-移庫入力画面の初期化処理
    PT0007_initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data);

    // 機能IDが「帳票出力」の場合
    if (getConductId() == RM00001_ConductId) {
        return RM0001_initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data);
    }

    // 画面番号を判定
    if (formNo == FormList.No) { // 一覧画面

        // 新規ボタンにフォーカスを設定する
        setFocusButton(FormList.Button.New);

        //RFタグ取込画面で設定されたメッセージを削除
        if (P_dicIndividual["RFUploadMessage"]) {
            delete P_dicIndividual["RFUploadMessage"];
            return true;
        }

        // グローバル変数に保持している入出庫履歴タブの表示年度(From・To)の値を削除する
        // 一覧画面から詳細画面に遷移する際は前回と同じ予備品の詳細画面を開くとは限らないため
        deleteDispYearValue();
    }
    else if (formNo == FormDetail.No) { // 詳細画面

        // 修正ボタンにフォーカスを設定する
        setFocusButton(FormDetail.Button.Edit);

        // 棚別在庫一覧(子一覧)を非表示
        changeListDisplay(FormDetail.InventoryChildList.Id, false, false);

        // 部門別在庫一覧(子一覧)を非表示
        changeListDisplay(FormDetail.CategoryChildList.Id, false, false);

        // 棚別在庫一覧、部門別在庫一覧表示切替
        getButtonCtrl(FormDetail.Button.ShelfInventory).click();

        // 表示年度を設定
        setDispYear();

        // 標準保管場所情報の「棚枝番」を非表示にする
        var parsLocationDetailNo = $(P_Article).find("#" + FormDetail.LocationInfo + getAddFormNo()).find("*[data-name='VAL" + FormDetail.LocationDetailNo + "']");
        parsLocationDetailNo.hide();

        // 詳細画面上部のラベル出力ボタンの活性/非活性を切り替える
        changeOutLabelBtnStatus();
    }
    else if (formNo == FormEdit.No) { // 詳細編集画面

        // 丸め処理(発注点・発注量・標準単価)
        roundLeadTime();
        roundOrderQuantity();
        roundDefaultPrice();

        // 結合文字列を枝番入力コントロールのヘッダーにセット
        var joinStr = getValue(FormEdit.JoinString, 1, 1, CtrlFlag.Label, false, false);
        setValue(FormEdit.DetailNo, 1, 1, CtrlFlag.Label, joinStr, false, true);

        var combFactory = getCtrl(FormEdit.PartsInfo, FormEdit.Factory, 1, CtrlFlag.Combo, false, false);
        if (combFactory.selectedIndex == -1) {
            // 先頭の値を選択
            var initCombo = function (ctrlId, valNo) {
                var combo = getCtrl(ctrlId, valNo, 0, CtrlFlag.Combo);
                selectComboTop(combo);
            }
            // 数量管理単位、金額管理単位
            initCombo(FormEdit.Purchase.Id, FormEdit.Purchase.NumUnit);
            initCombo(FormEdit.Purchase.Id, FormEdit.Purchase.CurUnit);
        }

        // 標準仕入先の値が「0」の場合は空にする(存在しない構成IDが入ってしまっているため)
        var venderId = getValue(FormEdit.Purchase.Id, FormEdit.Purchase.Vender, 1, CtrlFlag.TextBox, false, false);
        if (venderId == 0) {
            setValue(FormEdit.Purchase.Id, FormEdit.Purchase.Vender, 1, CtrlFlag.TextBox, '', false, false);
            setValue(FormEdit.Purchase.Id, FormEdit.Purchase.VenderStructureId, 1, CtrlFlag.Label, '', false, false);
        }

        // 棚の翻訳は遅らせて表示(オートコンプリート)
        setTimeout(function () {
            var ctrl = getCtrl(FormEdit.Location.Id, FormEdit.Location.InputLocationId, 1, CtrlFlag.TextBox, false, false);
            changeNoEdit(ctrl);
        }, 300); //300ミリ秒

        // 非表示項目の階層番号が2(倉庫)の場合は棚番は空にする
        var layerNo = getValue(FormEdit.Location.Id, FormEdit.Location.LayerNo, 1, CtrlFlag.Label, false, false);
        if (layerNo == '2') {
            setValue(FormEdit.Location.Id, FormEdit.Location.InputLocationId, 1, CtrlFlag.TextBox, '', false, false);
            setValue(FormEdit.Location.Id, FormEdit.Location.LocationId, 1, CtrlFlag.Label, '', false, false);
        }

        // メーカーからフォーカスアウトした場合
        var manufacture = getCtrl(FormEdit.PartsInfo, FormEdit.ManufacureCode, 1, CtrlFlag.TextBox, false, false);
        $(manufacture).blur(function () {
            var manufactureCode = getValue(FormEdit.PartsInfo, FormEdit.ManufacureCode, 1, CtrlFlag.TextBox, false, false);
            // 入力されていない場合は非表示項目を空にする
            if (!manufactureCode || manufactureCode == '') {
                setValue(FormEdit.PartsInfo, FormEdit.ManufactureStructureId, 1, CtrlFlag.Label, '', false, false);
            }
        });

        // 標準棚番からフォーカスアウトした場合
        var location = getCtrl(FormEdit.Location.Id, FormEdit.Location.InputLocationId, 1, CtrlFlag.TextBox, false, false);
        $(location).blur(function () {
            var locationCode = getValue(FormEdit.Location.Id, FormEdit.Location.InputLocationId, 1, CtrlFlag.TextBox, false, false);
            // 入力されていない場合は非表示項目を空にする
            if (!locationCode || locationCode == '') {
                setValue(FormEdit.Location.Id, FormEdit.Location.LocationId, 1, CtrlFlag.Label, '', false, false);
            }
        });

        // 標準仕入先からフォーカスアウトした場合
        var vender = getCtrl(FormEdit.Purchase.Id, FormEdit.Purchase.Vender, 1, CtrlFlag.TextBox, false, false);
        $(vender).blur(function () {
            var venderCode = getValue(FormEdit.Purchase.Id, FormEdit.Purchase.Vender, 1, CtrlFlag.TextBox, false, false);
            // 入力されていない場合は非表示項目を空に設定する
            if (!venderCode || venderCode == '') {
                setValue(FormEdit.Purchase.Id, FormEdit.Purchase.VenderStructureId, 1, CtrlFlag.Label, '', false, false);
            }
        });

        // 発注点からフォーカスアウトした場合
        var leadTime = getCtrl(FormEdit.Purchase.Id, FormEdit.Purchase.LeadTime, 1, CtrlFlag.TextBox, false, false);
        $(leadTime).blur(function () {
            roundLeadTime();
        });

        // 発注量からフォーカスアウトした場合
        var orderQuantity = getCtrl(FormEdit.Purchase.Id, FormEdit.Purchase.OrderQuantity, 1, CtrlFlag.TextBox, false, false);
        $(orderQuantity).blur(function () {
            roundOrderQuantity();
        });

        // 標準単価からフォーカスアウトした場合
        var defaultPrice = getCtrl(FormEdit.Purchase.Id, FormEdit.Purchase.DefaultPrice, 1, CtrlFlag.TextBox, false, false);
        $(defaultPrice).blur(function () {
            roundDefaultPrice();
        });

        // 管理工場コンボボックスが変更された場合
        var factory = getCtrl(FormEdit.PartsInfo, FormEdit.Factory, 1, CtrlFlag.Combo, false, false);
        $(factory).change(function () {
            var factoryId = getValue(FormEdit.PartsInfo, FormEdit.Factory, 1, CtrlFlag.Combo, false, false);
            setValue(FormEdit.Purchase.Id, FormEdit.Purchase.RoundDivision, 1, CtrlFlag.Combo, factoryId, false, false);
        });

        // 標準部門からフォーカスアウトした場合
        var deaprtment = getCtrl(FormEdit.PartsInfo, FormEdit.DepartmentCode, 1, CtrlFlag.TextBox, false, false);
        $(deaprtment).blur(function () {
            var deaprtmentCode = getValue(FormEdit.PartsInfo, FormEdit.DepartmentCode, 1, CtrlFlag.TextBox, false, false);
            // 入力されていない場合は非表示項目を空に設定する
            if (!deaprtmentCode || deaprtmentCode == '') {
                setValue(FormEdit.PartsInfo, FormEdit.DepartmentStructureId, 1, CtrlFlag.Label, '', false, false);
            }
        });

        // 標準勘定科目からフォーカスアウトした場合
        var account = getCtrl(FormEdit.Purchase.Id, FormEdit.Purchase.AccountCode, 1, CtrlFlag.TextBox, false, false);
        $(account).blur(function () {
            var accountCode = getValue(FormEdit.Purchase.Id, FormEdit.Purchase.AccountCode, 1, CtrlFlag.TextBox, false, false);
            // 入力されていない場合は非表示項目を空に設定する
            if (!accountCode || accountCode == '') {
                setValue(FormEdit.Purchase.Id, FormEdit.Purchase.AccountStructureId, 1, CtrlFlag.Label, '', false, false);
            }
        });
    }
    else if (formNo == FormLabel.No) { // ラベル出力画面

        // 予備品ID一覧を非表示
        changeListDisplay(FormLabel.PartsIdList, false, false);
    }
    else if (formNo == FormRFUpload.No) {
        //エラー出力を非活性にする
        var errorMsg = getCtrl(FormRFUpload.Info.Id, FormRFUpload.Info.ErrorMessage, 1, CtrlFlag.Textarea);
        changeInputControl(errorMsg, false);
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

    var conditionDataList = [];

    // 共通-文書管理詳細画面の遷移前処理を行うかどうか判定し、Trueの場合は行う
    if (IsExecDM0002_PrevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element)) {
        return DM0002_prevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element);
    }

    //共通-入庫入力画面部門在庫一覧の遷移前処理を行うかどうか判定し、Trueの場合は行う
    if (IsExecPT0005_PrevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element)) {
        return PT0005_prevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element);
    }

    //共通-出庫入力画面部門在庫一覧の遷移前処理を行うかどうか判定し、Trueの場合は行う
    if (IsExecPT0006_PrevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element)) {
        return PT0006_prevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element);
    }

    //共通-移庫入力画面部門在庫一覧の遷移前処理を行うかどうか判定し、Trueの場合は行う
    if (IsExecPT0007_PrevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element)) {
        return PT0007_prevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element);
    }

    // 一覧フィルタ処理実施
    if (executeListFilter(transTarget, FormList.Id, FormList.FilterId, FormList.Filter)) {
        return [false, conditionDataList];
    }

    // 画面番号を判定
    if (formNo == FormList.No) { // 一覧画面

        if (btn_ctrlId == FormList.Button.Output) { // 出力

            // 一覧にチェックされた行が存在しない場合、遷移をキャンセル
            if (!isCheckedList(FormList.Id)) {
                return [false, conditionDataList];
            }
        }
        else if (btn_ctrlId == FormList.Button.Label) { // ラベル出力

            // 一覧にチェックされた行が存在しない場合、遷移をキャンセル
            if (!isCheckedList(FormList.Id)) {
                return [false, conditionDataList];
            }

            // 予備品一覧をセット
            const ctrlIdList = [FormList.Id];
            conditionDataList = getListDataByCtrlIdList(ctrlIdList, formNo, 0, false, true);

        }
        else if (btn_ctrlId == FormList.Button.Enter) { // 入庫

            // 入庫入力画面の検索条件をセット
            conditionDataList.push(getParamToPT0005(getValueBySelectedRow(element, FormList.PartsId, false), 0, PartsTransFlg.New));
        }
        else if (btn_ctrlId == FormList.Button.Issue) { // 出庫

            // 出庫入力画面の検索条件をセット
            conditionDataList.push(getParamToPT0006(getValueBySelectedRow(element, FormList.PartsId, false), 0, PartsTransFlg.New));
        }
        else if (btn_ctrlId == FormList.Button.Move) { // 移庫

            // 移庫入力画面の検索条件をセット
            conditionDataList.push(getParamToPT0007FromPT0001(FormList.Id, getValueBySelectedRow(element, FormList.PartsId, false)));

        }
    }
    else if (formNo == FormDetail.No) { // 詳細画面

        // 入出庫履歴タブの表示年度(From・To)に表示されている値をグローバル変数に格納
        setDispYearValue();

        // ボタンコントロールIDを判定
        if (btn_ctrlId == FormDetail.Button.Edit || btn_ctrlId == FormDetail.Button.Copy) {// 修正・複写

            // 詳細編集画面の検索条件をセット
            const ctrlIdList = [FormDetail.PartsInfo];
            conditionDataList = getListDataByCtrlIdList(ctrlIdList, formNo, 0);
        }
        else if (btn_ctrlId == FormDetail.Button.Image) { // 画像

            // 文書管理の検索条件をセット
            conditionDataList.push(getParamToDM0002(AttachmentStructureGroupID.SpareImage, getValue(FormDetail.PartsInfo, FormDetail.PartsId, 1, CtrlFlag.TextBox)));
        }
        else if (btn_ctrlId == FormDetail.Button.Document) { // 添付

            // 文書管理の検索条件をセット
            conditionDataList.push(getParamToDM0002(AttachmentStructureGroupID.SpareDocument, getValue(FormDetail.PartsInfo, FormDetail.PartsId, 1, CtrlFlag.TextBox)));
        }
        else if (btn_ctrlId == FormDetail.Button.Map) { // 予備品地図

            // 文書管理の検索条件をセット
            conditionDataList.push(getParamToDM0002(AttachmentStructureGroupID.SpareMap, getValue(FormDetail.PartsInfo, FormDetail.FactoryId, 1, CtrlFlag.Label)));
        }
        else if (btn_ctrlId == FormDetail.Button.HistoryEnter) { // 入庫(入出庫履歴)

            // 入庫入力画面の検索条件をセット
            conditionDataList.push(getParamToPT0005(getValue(FormDetail.PartsInfo, FormDetail.PartsId, 1, CtrlFlag.TextBox, false, false), getValueBySelectedRow(element, FormDetail.InoutHistory.InoutHistoryId, true), PartsTransFlg.Edit));
        }
        else if (btn_ctrlId == FormDetail.Button.HistoryIssue) { // 出庫(入出庫履歴)

            // 出庫入力画面の検索条件をセット
            conditionDataList.push(getParamToPT0006(getValue(FormDetail.PartsInfo, FormDetail.PartsId, 1, CtrlFlag.TextBox, false, false), getValueBySelectedRow(element, FormDetail.InoutHistory.InoutHistoryId, true), PartsTransFlg.Edit));
        }
        else if (btn_ctrlId == FormDetail.Button.HistoryShelf) { // 棚番移庫(入出庫履歴)

            // 移庫入力画面の検索条件をセット(棚番移庫)
            conditionDataList.push(getParamToPT0007(PT0007_SubjectList_CtrlId, getValue(FormDetail.PartsInfo, FormDetail.PartsId, 1, CtrlFlag.TextBox, false, false), getValueBySelectedRow(element, FormDetail.InoutHistory.WorkNo, true)));
        }
        else if (btn_ctrlId == FormDetail.Button.HistoryCategory) { // 部門移庫(入出庫履歴)

            // 移庫入力画面の検索条件をセット(部門移庫)
            conditionDataList.push(getParamToPT0007(PT0007_DepartmentList_CtrlId, getValue(FormDetail.PartsInfo, FormDetail.PartsId, 1, CtrlFlag.TextBox, false, false), getValueBySelectedRow(element, FormDetail.InoutHistory.WorkNo, true)));
        }

        // ↓ここから下は入出庫履歴一覧のNoリンクをクリックした時です
        // Noリンクがクリックされたら、非表示のボタンをクリックするのでもう一度このメソッド(prevTransForm)が呼ばれます
        else if (btn_ctrlId == '') { // 入出庫履歴一覧のNo.リンククリック

            // 入出庫区分のアイテム番号を取得
            var itemNo = $(element).parent().parent().find("div[tabulator-field='VAL" + FormDetail.InoutHistory.InoutDivision + "']").find("select").find("option[selected='true']")[0].attributes[1].value;

            // 入出庫区分に応じて一覧のボタン(非表示)をクリックする
            var val;
            switch (itemNo) {
                case InoutDivisionNo.Enter:         // 入庫
                case InoutDivisionNo.InventoryEnter:// 棚卸入庫
                    val = FormDetail.InoutHistory.HistoryEnter; // 入庫ボタン    
                    break;

                case InoutDivisionNo.Issue:         // 出庫
                case InoutDivisionNo.InventoryIssue:// 棚卸出庫
                    val = FormDetail.InoutHistory.HistoryIssue; // 出庫ボタン   
                    break;

                case InoutDivisionNo.Subject:       // 棚番移庫
                    val = FormDetail.InoutHistory.HistoryShelf; // 棚番移庫ボタン         
                    break;

                case InoutDivisionNo.Department:    // 部門移庫
                    val = FormDetail.InoutHistory.HistoryCategory; // 部門移庫ボタン
                    break;

                default:
                    return [false, conditionDataList];
            }
            clickSelectedRowBtn(element, val);
            return [false, conditionDataList];
        }
    }

    return [true, conditionDataList];
}

/**
 * 【オーバーライド用関数】Tabulatorの描画が完了時の処理
 * @param {any} tbl 一覧
 * @param {any} id 一覧のID(#,_FormNo付き)
 */
function postBuiltTabulator(tbl, id) {

    // 機能IDが「帳票出力」の場合
    if (getConductId() == RM00001_ConductId) {
        return RM0001_postBuiltTabulator(tbl, id);
    }

    // 文書管理 詳細画面
    DM0002_postBuitTabulator(tbl, id);

    // 入出庫一覧 出庫入力画面
    PT0006_postBuiltTabulator(tbl, id);

    // 移庫入力画面
    PT0007_postBuiltTabulator(tbl, id);

    // 描画された一覧を判定
    if (id == "#" + FormList.Id + getAddFormNo()) { // 一覧画面 予備品一覧

        // 予備品一覧の背景色変更、画像の高さ変更
        postSearchList(tbl);

    }
    else if (id == "#" + FormDetail.InventoryParentList.Id + getAddFormNo()) { // 詳細画面 棚別在庫一覧
        // 一覧の入れ子
        convertTabulatorListToNestedTable(id, FormDetail.InventoryParentList.Id, FormDetail.InventoryChildList.Id, FormDetail.InventoryParentList.Key, FormDetail.InventoryChildList.Key);
    }
    else if (id == "#" + FormDetail.CategoryParentList.Id + getAddFormNo()) { // 詳細画面 部門別在庫一覧
        // 一覧の入れ子
        convertTabulatorListToNestedTable(id, FormDetail.CategoryParentList.Id, FormDetail.CategoryChildList.Id, FormDetail.CategoryParentList.Key, FormDetail.CategoryChildList.Key);
    }
    else if (id == "#" + FormDetail.InoutHistory.Id + getAddFormNo()) { // 詳細画面 入出庫履歴一覧

        // 繰越のデータのリンクを無効にする
        noneLinkCarryData();
    }
}

/**
 * 【オーバーライド用関数】Tabulatorのページ変更後の処理
 * @param {any} tbl 処理対象の一覧
 * @param {any} id 処理対象の一覧のID(#,_FormNo付き)
 * @param {any} pageNo 表示するページNo
 * @param {any} pagesize 表示中のページの件数
 */
function postTabulatorChangePage(tbl, id, pageNo, pagesize) {

    if (id == "#" + FormList.Id + getAddFormNo()) {

        // 予備品一覧の背景色変更、画像の高さ変更
        postSearchList(tbl);
    }

}

/**
 *【オーバーライド用関数】
 *  遷移処理の後
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
function postTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element) {

    // 共通-文書管理詳細画面の遷移後処理
    DM0002_postTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element);

    // 現在の画面番号
    formNo = getFormNo();

    // 画面番号を判定
    if (formNo == FormEdit.No) {
        if (transDiv == transDivDef.New || transDiv == transDivDef.Edit || transDiv == transDivDef.Copy) {

            // 予備品名にフォーカスをセットする
            setFocusDelay(FormEdit.PartsInfo, FormEdit.PartsName, 2, CtrlFlag.TextBox, false);
        }

        if (transDiv == transDivDef.New) {
            // 新規入力の場合使用区分を非活性にする
            changeInputControl(getCtrl(FormEdit.PartsInfo, FormEdit.UseId, 1, CtrlFlag.Combo, false), false);
        }
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
 *  @param {number} transPtn ：画面遷移のパターン、transPtnDef
 */
function beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom, transPtn) {

    // 機能IDが「帳票出力」の場合
    RM0001_beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom);

    // 共通-文書管理詳細画面の画面再表示前処理
    // 共通-文書管理詳細画面を閉じたときの再表示処理はこちらで行うので、各機能での呼出は不要
    DM0002_beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom);

    // 共通-出庫一覧の画面再表示前処理
    PT0006_beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom);
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
function afterSearchBtnProcess(appPath, btn, conductId, pgmId, formNo, conductPtn) {
    if (conductId != PT0006_FormList.ConductId) {
        return;
    }
    PT0006_afterSearchBtnProcess(appPath, btn, conductId, pgmId, formNo, conductPtn);
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
function beforeSearchBtnProcess(appPath, btn, conductIdW, pgmIdW, formNoW, conductPtnW) {

    // 機能IDが「帳票出力」の場合
    if (conductIdW == RM00001_ConductId) {
        return RM0001_beforeSearchBtnProcess(appPath, btn, conductIdW, pgmIdW, formNoW, conductPtnW);
    }

}

/**
 *【オーバーライド用関数】
 *  行削除の前
 *
 *  @appPath    {string}    ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @btn        {<a>}       ：行削除ﾘﾝｸのa要素
 *  @id         {string}    ：行削除ﾘﾝｸの対象一覧のCTRLID
 *  @checkes    {要素}      ：削除対象の要素リスト
 */
function preDeleteRow(appPath, btn, id, checkes) {

    // 共通-文書管理詳細画面の行削除前処理
    var comRtn = DM0002_preDeleteRow(appPath, btn, id, checkes);
    if (!comRtn) {
        // 処理終了の場合は終了
        return false;
    }

    return preDeleteRowCommon(id, [FormDetail.List.Id, FormDetail.ListMaintKind.Id]);
}

/**
 * 【オーバーライド用関数】実行ボタン前処理
 *  @param {string} appPath ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string} transDiv：画面遷移アクション区分
 *  @param {string} conductId：機能ID
 *  @param {string} pgmId ：プログラムID
 *  @param {number} formNo ：画面NO
 *  @param {html} btn  ：ボタン要素
 *  @param {number} conductPtn  ：機能処理ﾊﾟﾀｰﾝ
 *  @param {boolean} autoBackFlg ：ajax正常終了後、自動戻るフラグ　false:戻らない、true:自動で戻る
 *  @param {boolean} isEdit ：単票表示フラグ
 *  @param {number} confirmNo ：確認番号
 *  @return {bool} 処理続行フラグ Trueなら続行、Falseなら処理終了
 */
function preRegistProcess(appPath, transDiv, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, confirmNo) {

    // 詳細編集画面(複写)で登録ボタンが押下された場合
    if (formNo == FormEdit.No && btn[0].name == FormEdit.Button.Regist) {

        // グローバル変数に保持している入出庫履歴タブの表示年度(From・To)の値を削除する
        // 複写で登録した場合は予備品を新しく作成することになるため表示年度の値は保持する必要はない
        deleteDispYearValue();
    }

    return true;
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
function postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data) {

    // 機能IDが「帳票出力」の場合
    if (getConductId() == RM00001_ConductId) {
        return RM0001_postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data);
    }

    // 共通-文書管理詳細画面の実行正常終了後処理
    DM0002_postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data);

    // 共通-入庫入力詳細画面の実行正常終了後処理
    PT0005_postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data);

    // 共通-出庫入力詳細画面の実行正常終了後処理
    PT0006_postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data);

    // 共通-移庫入力詳細画面の実行正常終了後処理
    PT0007_postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data);

    if (formNo == FormDetail.No) {
        setDispYear();
    } else if (formNo == FormRFUpload.No) {
        //RFタグ取込画面

        //エラーなく登録が完了した場合（エラー出力に値が設定されていない場合）、画面を閉じる
        var error = getValue(FormRFUpload.Hide.Id, FormRFUpload.Hide.Flg, 1, CtrlFlag.Label);
        if (!convertStrToBool(error)) {
            // 取込画面を閉じて、一覧画面へ戻る
            var back = getButtonCtrl(FormRFUpload.Button.Back);
            var modal = $(back).closest('section.modal_form');
            $(modal).modal('hide');
        }
    }
}


/**
 *【オーバーライド用関数】バリデーション前処理
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 */
function validDataPre(appPath, conductId, formNo, btn) {

    // 予備品一覧の詳細画面で「再表示」ボタンが押下されている場合
    if (conductId == ConductId_PT0001 && formNo == FormDetail.No && btn.name == FormDetail.Button.reDisp) {

        //エラー状態を初期化
        //clearErrorComStatusForAreas(false);

        // 表示年度(From)の要素を取得
        var dispYearFromEle = $(P_Article).find("#" + FormDetail.DispCondition.Id + getAddFormNo() + "VAL" + FormDetail.DispCondition.No)[0];

        // 表示年度(To) の要素を取得
        var dispYearToEle = $(P_Article).find("#" + FormDetail.DispCondition.Id + getAddFormNo() + "VAL" + FormDetail.DispCondition.No + "To")[0];

        // 表示年度(From)の値を取得
        var dispYearFromVal = dispYearFromEle.value;

        // 表示年度(To) の値を取得
        var dispYearToVal = dispYearToEle.value;

        // 表示年度のFrom・To どちらも未入力の場合はエラーとする
        if (dispYearFromVal == "" && dispYearToVal == "") {

            // エラー情報を作成
            var message = { required: P_ComMsgTranslated[141270006] }; // 表示年度の開始または終了どちらか一方は入力して下さい。
            var rule = {};
            rule['required'] = true;
            rule['messages'] = message;

            // エラー情報を表示年度のFrom・Toに設定する
            $(dispYearFromEle).rules('add', rule);
            $(dispYearToEle).rules('add', rule);

            return false;
        }

        // 表示年度(From)が空または「1753」未満の場合
        if (dispYearFromVal == "" || dispYearFromVal < SqlYear.MinYear) {

            // SQLで扱える年の最小値で補完する
            dispYearFromEle.value = SqlYear.MinYear;
        }

        // 表示年度(To)が空の場合
        if (dispYearToVal == "") {

            // SQLで扱える年の最大値で補完する
            dispYearToEle.value = SqlYear.MaxYear;
        }
        // 表示年度(To)が「1753」未満の場合
        else if (dispYearToVal < SqlYear.MinYear) {

            // SQLで扱える年の最小値で補完する
            dispYearToEle.value = SqlYear.MinYear;
        }
    }

    return true;
}

/**
 *【オーバーライド用関数】登録前追加条件取得処理
 *  @param appPath       ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param conductId     ：機能ID
 *  @param formNo        ：画面番号
 *  @param btn           ：押下されたボタン要素
 */
function addSearchConditionDictionaryForRegist(appPath, conductId, formNo, btn) {

    // 共通-文書管理詳細画面の登録前追加条件取得処理を行うかどうか判定し、Trueの場合は行う
    if (IsExecDM0002_AddSearchConditionDictionaryForRegist(appPath, conductId, formNo, btn)) {
        return DM0002_addSearchConditionDictionaryForRegist(appPath, conductId, formNo, btn);
    }

    // 共通-移庫入力画面の登録前追加条件取得処理を行うかどうか判定し、Trueの場合は行う
    if (IsExecPT0007_AddSearchConditionDictionaryForRegist(appPath, conductId, formNo, btn)) {
        return PT0007_addSearchConditionDictionaryForRegist(appPath, conductId, formNo, btn);
    }

    // それ以外の場合
    var conditionDataList = [];
    return conditionDataList;
}

/**
 *【オーバーライド用関数】取込処理個別入力チェック
 *  @param appPath       ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param conductId     ：機能ID
 *  @param formNo        ：画面番号
 */
function preInputCheckUpload(appPath, conductId, formNo) {

    if (getConductId() == RM00001_ConductId) {
        // 共通-帳票出力画面 入力チェック
        return RM0001_preInputCheckUpload(appPath, conductId, formNo);
    }
    if (conductId == DM0002_ConductId) {
        // 添付情報
        return DM0002_preInputCheckUpload(appPath, conductId, formNo);
    }
    //取込画面(自動で画面を閉じない)
    return [true, false, false];
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

    // 機能IDが「帳票出力」の場合
    if (getConductId() == RM00001_ConductId) {
        return RM0001_setComboOtherValues(appPath, combo, datas, selected, formNo, ctrlId, valNo);
    }

    // 共通-文書管理詳細画面のコンボボックス変更処理
    DM0002_setComboOtherValues(appPath, combo, datas, selected, formNo, ctrlId, valNo);

    // 共通-入庫入力画面のコンボボックス変更処理
    PT0005_setComboOtherValues(appPath, combo, datas, selected, formNo, ctrlId, valNo);

    // 共通-移庫入力画面のコンボボックス変更処理
    PT0007_setComboOtherValues(appPath, combo, datas, selected, formNo, ctrlId, valNo);

    // 詳細編集画面
    if (formNo == FormEdit.No) {
        // 数量管理単位コンボボックス
        if (ctrlId == FormEdit.Purchase.Id && (valNo == FormEdit.Purchase.NumUnit || valNo == FormEdit.Purchase.CurUnit)) {
            // 丸め処理(発注点・発注量・標準単価)
            roundLeadTime();
            roundOrderQuantity();
            roundDefaultPrice();
        }
    }
}

/**
 * 【オーバーライド用関数】Tabuator一覧のヘッダー設定前処理
 * @param {string} appPath          ：アプリケーションルートパス
 * @param {string} id               ：一覧のID(#,_FormNo付き)
 * @param {object} header           ：ヘッダー情報
 * @param {Element} headerElement   ：ヘッダー要素
 * @param {List<string>} subTableIdList ：入れ子一覧の場合、子一覧のID
 */
function prevSetTabulatorHeader(appPath, id, header, headerElement, subTableIdList) {

    // 一覧を入れ子にする
    // 一覧を判定
    if (id == "#" + FormDetail.InventoryParentList.Id + getAddFormNo()) {

        // 詳細画面 棚別在庫一覧
        setNestedTable(id, header, FormDetail.InventoryParentList.Id, FormDetail.InventoryChildList.Id, subTableIdList);
    }
    else if (id == "#" + FormDetail.CategoryParentList.Id + getAddFormNo()) {

        // 詳細画面 部門別在庫一覧
        setNestedTable(id, header, FormDetail.CategoryParentList.Id, FormDetail.CategoryChildList.Id, subTableIdList);
    }
}

/**
 *【オーバーライド用関数】
 *  個別実装ボタン
 *
 *  @param {string} appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {number} formNo      ：画面NO
 *  @param {string} btnCtrlId   ：ボタンCTRLID
 */
function clickIndividualImplBtn(appPath, formNo, btnCtrlId) {

    // 機能IDが「帳票出力」の場合
    if (getConductId() == RM00001_ConductId) {
        return RM0001_clickIndividualImplBtn(appPath, formNo, btnCtrlId);
    }

    // ボタンコントロールIDを判定
    if (btnCtrlId == FormDetail.Button.ShelfInventory) { // 棚別在庫一覧

        // 一覧の表示状態を切替
        changeListDisplay(FormDetail.InventoryParentList.Id, true); // 棚別在庫一覧を表示
        changeListDisplay(FormDetail.CategoryParentList.Id, false); // 部門別在庫一覧を非表示
        setHideButton(FormDetail.Button.OutputLabelDetailShed, false); // ラベル出力ボタンを表示

        // ボタン色を変更
        getButtonCtrl(FormDetail.Button.ShelfInventory).removeClass(FormDetail.Button.changeDispCssCls);     // 棚別在庫一覧
        getButtonCtrl(FormDetail.Button.DepartInventory).addClass(FormDetail.Button.changeDispCssCls); // 部門別在庫一覧
    }
    else if (btnCtrlId == FormDetail.Button.DepartInventory) { // 部門別在庫一覧

        // 一覧の表示状態を切替
        changeListDisplay(FormDetail.InventoryParentList.Id, false); // 棚別在庫一覧を非表示
        changeListDisplay(FormDetail.CategoryParentList.Id, true);   // 部門別在庫一覧を表示
        setHideButton(FormDetail.Button.OutputLabelDetailShed, true); // ラベル出力ボタンを非表示

        // ボタン色を変更
        getButtonCtrl(FormDetail.Button.ShelfInventory).addClass(FormDetail.Button.changeDispCssCls); // 棚別在庫一覧
        getButtonCtrl(FormDetail.Button.DepartInventory).removeClass(FormDetail.Button.changeDispCssCls);   // 部門別在庫一覧
    }
}

/**
 *【オーバーライド用関数】
 *  戻る処理の前(単票、子画面共用)
 *
 *  @appPath        {string}    ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @btnCtrlId      {byte}      ：ボタンのCtrlId
 *  @codeTransFlg   {int}       ：1:コード＋翻訳 選ボタンから画面遷移/1以外:それ以外
 */
function prevBackBtnProcess(appPath, btnCtrlId, status, codeTransFlg) {
    var formNo = getFormNo();
    if (formNo == FormEdit.No && btnCtrlId == FormEdit.Button.Regist) {

        //新規登録画面から登録後、参照画面に渡すキー情報をセット
        var conditionDataList = getListDataByCtrlIdList([FormEdit.PartsInfo], FormEdit.No, 0);
        // 一覧から参照へ遷移する場合と同様に、参照画面の検索条件を追加
        setSearchCondition(ConductId_PT0001, FormDetail.No, conditionDataList);
    }
    return true;
}

/**
 *【オーバーライド用関数】
 *  画面状態設定後の個別実装
 *
 * @status {number}       ：ﾍﾟｰｼﾞ状態　0：初期状態、1：検索後、2：明細表示後ｱｸｼｮﾝ、3：ｱｯﾌﾟﾛｰﾄﾞ後
 * @pageRowCount {number} ：ﾍﾟｰｼﾞ全体のﾃﾞｰﾀ行数
 * @conductPtn {byte}     ：com_conduct_mst.ptn
 * @formNo {number}       ：画面番号
 */
function setPageStatusEx(status, pageRowCount, conductPtn, formNo) {

    // 機能IDが「帳票出力」の場合
    if (getConductId() == RM00001_ConductId) {
        return RM0001_setPageStatusEx(status, pageRowCount, conductPtn, formNo);
    }

    // 共通-入庫入力画面
    PT0005_setPageStatusEx(status, pageRowCount, conductPtn, formNo);

    // 共通-出庫入力画面
    PT0006_setPageStatusEx(status, pageRowCount, conductPtn, formNo);

}


/**
 *【オーバーライド用関数】タブ切替時
 * @tabNo ：タブ番号
 * @tableId : 一覧のコントロールID
 */
function initTabOriginal(tabNo, tableId) {

    // 移庫入力画面 タブ切替時
    PT0007_initTabOriginal(tabNo, tableId);

    // 入出庫履歴タブが開かれた場合
    if (tabNo == FormDetail.TabNo.InoutHistory) {

        // 繰越のデータのリンクを無効にする
        noneLinkCarryData();
    }

}

/**
 *【オーバーライド用関数】Excel出力ﾁｪｯｸ処理 - 前処理
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 */
function reportCheckPre(appPath, conductId, formNo, btn) {

    // 機能IDが「帳票出力」の場合
    if (getConductId() == RM00001_ConductId) {
        return RM0001_reportCheckPre(appPath, conductId, formNo, btn)
    }

    if (formNo == FormLabel.No) { // ラベル出力画面

        // 勘定科目選択一覧にチェックされた行が存在しない場合、遷移をキャンセル
        if (!isCheckedList(FormLabel.SubjectList)) {
            return false;
        }

        // 部門選択一覧にチェックされた行が存在しない場合、遷移をキャンセル
        if (!isCheckedList(FormLabel.CategoryList)) {
            return false;
        }
    }
    else if (formNo == FormDetail.No) { // 詳細画面

        if (btn.name == FormDetail.Button.OutputLabelDetailShed) {

            // 棚別在庫一覧(子一覧)のデータ件数を取得
            var dataCnt = P_listData['#' + FormDetail.InventoryChildList.Id + getAddFormNo()].getData().length;

            // 棚別在庫一覧にデータが存在しない場合はエラーメッセージを表示して終了
            if (dataCnt <= 0) {
                // 「対象行が選択されていません。」
                setMessage(P_ComMsgTranslated[941160003], procStatus.Error);
                return false;
            }

            var ctrlId; // 入れ子になっている子一覧のコントロールID(入れ子になるとデフォルトのコントロールIDではなくなるため)   
            for (var i = 1; i <= dataCnt; i++) {

                // チェック対象の一覧のコントロールIDを設定
                ctrlId = FormDetail.InventoryChildList.Id + "_" + i;

                // 一覧が存在しない場合はスキップ
                var table = P_listData['#' + ctrlId + getAddFormNo()];
                if (!table) {
                    continue;
                }

                // 在庫情報タブの「ラベル出力」ボタン押下時、棚別在庫一覧(子一覧)一覧にチェックされた行が存在する場合、終了して確認メッセージ表示
                if (isCheckedList(ctrlId)) {
                    return true;
                }
            }

            // 在庫情報タブの「ラベル出力」ボタン押下時、棚別在庫一覧(子一覧)一覧にチェックされた行が存在しない場合、遷移をキャンセル
            return false;
        }
    }
    return true;
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
function passDataCmConduct(appPath, conductId, parentNo, conditionDataList, ctrlId, btn_ctrlId, rowNo, element, parentConductId) {

    // 共通-帳票出力画面の個別実装ボタン
    RM0001_passDataCmConduct(appPath, conductId, parentNo, conditionDataList, ctrlId, btn_ctrlId, rowNo, element, parentConductId);
}

/**
 * 【オーバーライド用関数】Tabuator一覧の描画前処理
 * @param {string} appPath  ：アプリケーションルートパス
 * @param {string} id       ：一覧のID(#,_FormNo付き)
 * @param {string} options  ： 一覧のオプション情報
 * @param {object} header   ：ヘッダー情報
 * @param {object} dispData ：データ
 */
function prevCreateTabulator(appPath, id, options, header, dispData) {

    // 機能IDが「帳票出力」の場合
    if (getConductId() == RM00001_ConductId) {
        return RM0001_prevCreateTabulator(appPath, id, options, header, dispData);
    }

}

/**
 * 【オーバーライド用関数】
 * 　コード＋翻訳 他の項目に値セット処理
 *
 *  @appPath {string}   　　　　     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @formNo {byte}                   ：画面番号
 *  @ctrl {要素}   　　              ：コード＋翻訳要素
 *  @data {List}                     ：取得データ
 */
function setCodeTransOtherNames(appPath, formNo, ctrl, data) {

    // 入庫入力画面
    PT0005_setCodeTransOtherNames(appPath, formNo, ctrl, data);

    // 移庫入力画面
    PT0007_setCodeTransOtherNames(appPath, formNo, ctrl, data);

    // 詳細編集画面
    if (formNo == FormEdit.No && data && data.length > 0) {
        var id = ctrl[0].id + '';
        if (id.indexOf(FormEdit.Location.Id) > -1 && id.indexOf("VAL" + FormEdit.Location.InputLocationId) > -1) {
            // 標準棚番(オートコンプリート)選択時、構成IDを非表示の項目に設定する
            setValue(FormEdit.Location.Id, FormEdit.Location.LocationId, 1, CtrlFlag.Label, data[0].EXPARAM1, false, false);
            setValue(FormEdit.Location.Id, FormEdit.Location.WareHouseId, 1, CtrlFlag.Label, data[0].EXPARAM7, false, false);
            // 棚番を再設定（付属情報を削除）
            setValue(FormEdit.Location.Id, FormEdit.Location.InputLocationId, 1, CtrlFlag.Input, data[0].EXPARAM2, false, false);
            // 上位階層の地区～予備品倉庫を設定
            var district = getCtrl(FormEdit.DefalutLocation.Id, FormEdit.DefalutLocation.District, 1, CtrlFlag.Label);
            setStructureInfoToTreeLabel(district, data[0].EXPARAM3, data[0].EXPARAM4);
            var factory = getCtrl(FormEdit.DefalutLocation.Id, FormEdit.DefalutLocation.Factory, 1, CtrlFlag.Label);
            setStructureInfoToTreeLabel(factory, data[0].EXPARAM5, data[0].EXPARAM6);
            var wareHouse = getCtrl(FormEdit.DefalutLocation.Id, FormEdit.DefalutLocation.WareHouse, 1, CtrlFlag.Label);
            setStructureInfoToTreeLabel(wareHouse, data[0].EXPARAM7, data[0].EXPARAM8);
        }
        else if (id.indexOf(FormEdit.PartsInfo) > -1 && id.indexOf("VAL" + FormEdit.ManufacureCode) > -1) {
            // メーカー(オートコンプリート)選択時、構成IDを非表示の項目に設定する
            setValue(FormEdit.PartsInfo, FormEdit.ManufactureStructureId, 1, CtrlFlag.Label, data[0].VALUE1, false, false);
        }
        else if (id.indexOf(FormEdit.Purchase.Id) > -1 && id.indexOf("VAL" + FormEdit.Purchase.Vender) > -1) {
            // 標準仕入先(オートコンプリート)選択時、構成IDを非表示の項目に設定する
            setValue(FormEdit.Purchase.Id, FormEdit.Purchase.VenderStructureId, 1, CtrlFlag.Label, data[0].VALUE1, false, false);
        }
        else if (id.indexOf(FormEdit.PartsInfo) > -1 && id.indexOf("VAL" + FormEdit.DepartmentCode) > -1) {
            // 標準部門(オートコンプリート)選択時、構成IDを非表示の項目に設定する
            setValue(FormEdit.PartsInfo, FormEdit.DepartmentStructureId, 1, CtrlFlag.Label, data[0].EXPARAM1, false, false);
        }
        else if (id.indexOf(FormEdit.Purchase.Id) > -1 && id.indexOf("VAL" + FormEdit.Purchase.AccountCode) > -1) {
            // 標準勘定科目(オートコンプリート)選択時、構成IDを非表示の項目に設定する
            setValue(FormEdit.Purchase.Id, FormEdit.Purchase.AccountStructureId, 1, CtrlFlag.Label, data[0].EXPARAM1, false, false);
        }
    }
}

/**
 * 【オーバーライド用関数】階層選択モーダル画面の選択ボタン実行後
 * @param {any} appPath アプリケーションパス
 * @param {any} btn 選択ボタン要素
 * @param {any} ctrlId コントロールID
 * @param {any} structureGrpId 構成グループID
 * @param {any} maxStructureNo 最下層階層番号
 * @param {any} node 設定値取得先のノード
 */
function afterSelectBtnForTreeView(appPath, btn, ctrlId, structureGrpId, maxStructureNo, node) {

    // 詳細編集画面の標準保管場所情報ツリーの場合
    if (ctrlId == FormEdit.DefalutLocation.Id) {

        // 取得した最下層の値(構成ID)を退避する
        var wareHouseId = node.original.li_attr["data-structureid"];
        var structureNo = node.original.li_attr["data-structureno"];
        if (structureNo == 2) {
            // 予備品倉庫まで選択されている場合
            setValue(FormEdit.Location.Id, FormEdit.Location.WareHouseId, 1, CtrlFlag.Label, wareHouseId, false, false);
        } else {
            // 予備品倉庫は未選択の場合
            setValue(FormEdit.Location.Id, FormEdit.Location.WareHouseId, 1, CtrlFlag.Label, "", false, false);
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
function prevInitFormData(appPath, formNo, btnCtrlId, conditionDataList, listDefines, pgmId) {

    if (formNo == FormDetail.No) {

        // 遷移先情報を設定(URL指定起動)
        conditionDataList = getParamByUrl(PT0001_ConductId, conditionDataList);
    }

    return [conditionDataList, listDefines];
}

/**
 *【オーバーライド用関数】実行ﾁｪｯｸ処理 - 前処理
 *  @appPath     {string}   ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId   {string}   ：機能ID
 *  @formNo      {number}   ：画面番号
 *  @btn         {button}   ：押下されたボタン要素
 */
function prevCommonValidCheck(appPath, conductId, formNo, btn) {

    return PT0007_prevCommonValidCheck(appPath, conductId, formNo, btn);

}

/**
 *【オーバーライド用関数】
 *  閉じる処理の後(ポップアップ画面用)
 */
function postBackBtnProcessForPopup() {

    // 機能IDを取得
    var conductId = getConductId();

    // 文書管理
    DM0002_postBackBtnProcessForPopup(conductId);

    // 入庫入力
    PT0005_postBackBtnProcessForPopup(conductId);

    // 出庫入力
    PT0006_postBackBtnProcessForPopup(conductId);

    // 移庫入力
    PT0007_postBackBtnProcessForPopup(conductId);
}

/**
 * 表示年度に値を設定する
 * */
function setDispYear() {
    var yearFrom = getValue(FormDetail.DispCondition.Id, FormDetail.DispCondition.HideFrom, 1, CtrlFlag.Label, false, false);
    if (yearFrom) {
        $(P_Article).find("#" + FormDetail.DispCondition.Id + getAddFormNo() + "VAL" + FormDetail.DispCondition.No)[0].value = yearFrom;
    }
    var yearTo = getValue(FormDetail.DispCondition.Id, FormDetail.DispCondition.HideTo, 1, CtrlFlag.Label, false, false);
    if (yearTo) {
        $(P_Article).find("#" + FormDetail.DispCondition.Id + getAddFormNo() + "VAL" + FormDetail.DispCondition.No + "To")[0].value = yearTo;
    }
}

/**
 * 選択されたレコードのボタンをクリックする
 * @param {any} element イベント発生要素
 * @param {any} val ボタンのコントロール番号
 */
function clickSelectedRowBtn(element, val) {
    $(element).parent().parent().find("div[tabulator-field=VAL" + val + "]").find("input").click();
}

/**
 * クリックされたボタンのレコードの値を取得
 * @param {any} element イベント発生要素
 * @param {any} val 値を取得する列の項目番号
 * @param {any} 入出庫履歴一覧の場合True
 */
function getValueBySelectedRow(element, val, isInoutHistory) {
    var value;
    if (isInoutHistory) {
        value = $(element).parent().parent().find("div[tabulator-field='VAL" + val + "']")[0].innerText;
    }
    else {
        value = $(element).parent().parent().find("div[tabulator-field='VAL" + val + "']").find(".labeling")[0].innerText;
    }
    return value;
}

/**
 * 繰越のデータのリンクを無効にする
 * */
function noneLinkCarryData() {

    // 入出庫一覧の要素取得
    var tbl = $("#" + FormDetail.InoutHistory.Id + getAddFormNo() + "_div").find("div .tabulator-row");
    if (tbl.length > 0) {

        // 入出庫区分が「繰越」の場合
        if ($(tbl[0]).find("select").find("option[selected='true']")[0].attributes[1].value == InoutDivisionNo.Carry) {

            // 繰越のデータのレコードのNo.リンクを無効化する
            var link = $("#" + FormDetail.InoutHistory.Id + getAddFormNo() + "_div").find("div .tabulator-row").find("div[tabulator-field='ROWNO']")[0];
            $(link).addClass(FormDetail.InoutHistory.CarryLinkNone);
            // 文字色を黒に変更
            var value = $(link).find("a");
            $(value)[0].style.color = FormDetail.InoutHistory.ValueCarryCssStyle;
        }
    }
}

/*
 * 予備品一覧の背景色変更、画像高さ変更
 * */
function postSearchList(tbl) {

    // 検索結果のレコードを取得
    var table = $(tbl.element).find(".tabulator-table").children();
    if ($(table).length) {
        $.each($(table), function (idx, row) {
            // 在庫数 <= 発注点(発注アラーム列の値が「Y」)の場合は背景色を黄色に変更する
            if ($($(row).find("div[tabulator-field=VAL" + FormList.OrderAlert.CtrlNo + "]").children()[0]).hasClass('checked')) {
                $(row).addClass(FormList.OrderAlert.CssClass);
            }
        });
    }
}

/*
 * 発注点 丸め処理
 * */
function roundLeadTime() {
    var leadTimeVal = getValue(FormEdit.Purchase.Id, FormEdit.Purchase.LeadTime, 1, CtrlFlag.TextBox, false, false).replace(/[^-0-9.]/g, '');;
    if (!isNaN(leadTimeVal) && leadTimeVal != '') {
        // 数量管理単位の小数点以下桁数を取得
        var cmbUnitDigit = getCtrl(FormEdit.Purchase.Id, FormEdit.Purchase.NumUnit, 1, CtrlFlag.Combo, false, false);
        var cmbUnitIndex = cmbUnitDigit.selectedIndex;
        // アイテムが選択されていない場合は何もしない
        if (cmbUnitIndex == -1) {
            return;
        }
        var unitDidit = $(cmbUnitDigit)[0][cmbUnitIndex].attributes[1].value;
        // 丸め処理区分を取得
        var cmbRoundDivision = getCtrl(FormEdit.Purchase.Id, FormEdit.Purchase.RoundDivision, 1, CtrlFlag.Combo, false, false);
        var cmbRoundIndex = cmbRoundDivision.selectedIndex;
        var roundDivision;
        if (cmbRoundIndex < 0) {
            roundDivision = 1;
        }
        else {
            roundDivision = $(cmbRoundDivision)[0][cmbRoundIndex].innerText;
        }
        // 発注点の丸め処理
        var leadTimeDisp = roundDigit(leadTimeVal, unitDidit, roundDivision);
        // 画面項目に設定する
        setValue(FormEdit.Purchase.Id, FormEdit.Purchase.LeadTime, 1, CtrlFlag.TextBox, leadTimeDisp, false, false);
    }
    else {
        // 空にする
        setValue(FormEdit.Purchase.Id, FormEdit.Purchase.LeadTime, 1, CtrlFlag.TextBox, '', false, false);
    }
}

/*
 * 発注量 丸め処理
 * */
function roundOrderQuantity() {
    var orderQuantityVal = getValue(FormEdit.Purchase.Id, FormEdit.Purchase.OrderQuantity, 1, CtrlFlag.TextBox, false, false).replace(/[^-0-9.]/g, '');;
    if (!isNaN(orderQuantityVal) && orderQuantityVal != '') {
        // 数量管理単位の小数点以下桁数を取得
        var cmbUnitDigit = getCtrl(FormEdit.Purchase.Id, FormEdit.Purchase.NumUnit, 1, CtrlFlag.Combo, false, false);
        var cmbUnitIndex = cmbUnitDigit.selectedIndex;
        if (cmbUnitIndex == -1) {
            return;
        }
        var unitDidit = $(cmbUnitDigit)[0][cmbUnitIndex].attributes[1].value;
        // 丸め処理区分を取得
        var cmbRoundDivision = getCtrl(FormEdit.Purchase.Id, FormEdit.Purchase.RoundDivision, 1, CtrlFlag.Combo, false, false);
        var cmbRoundIndex = cmbRoundDivision.selectedIndex;
        var roundDivision;
        if (cmbRoundIndex < 0) {
            roundDivision = 1;
        }
        else {
            roundDivision = $(cmbRoundDivision)[0][cmbRoundIndex].innerText;
        }
        // 発注量の丸め処理
        var orderQuantityDisp = roundDigit(orderQuantityVal, unitDidit, roundDivision);
        // 画面項目に設定する
        setValue(FormEdit.Purchase.Id, FormEdit.Purchase.OrderQuantity, 1, CtrlFlag.TextBox, orderQuantityDisp, false, false);
    }
    else {
        // 空にする
        setValue(FormEdit.Purchase.Id, FormEdit.Purchase.OrderQuantity, 1, CtrlFlag.TextBox, '', false, false);
    }
}

/*
 * 標準単価 丸め処理
 * */
function roundDefaultPrice() {
    var defaultPriceVal = getValue(FormEdit.Purchase.Id, FormEdit.Purchase.DefaultPrice, 1, CtrlFlag.TextBox, false, false).replace(/[^-0-9.]/g, '');;
    if (!isNaN(defaultPriceVal) && defaultPriceVal != '') {
        // 金額管理単位の小数点以下桁数を取得
        var cmbCurrencyDigit = getCtrl(FormEdit.Purchase.Id, FormEdit.Purchase.CurUnit, 1, CtrlFlag.Combo, false, false);
        var cmbCurrencyIndex = cmbCurrencyDigit.selectedIndex;
        if (cmbCurrencyIndex == -1) {
            return;
        }
        var currencyDigit = $(cmbCurrencyDigit)[0][cmbCurrencyIndex].attributes[1].value;
        // 丸め処理区分を取得
        var cmbRoundDivision = getCtrl(FormEdit.Purchase.Id, FormEdit.Purchase.RoundDivision, 1, CtrlFlag.Combo, false, false);
        var cmbRoundIndex = cmbRoundDivision.selectedIndex;
        var roundDivision;
        if (cmbRoundIndex < 0) {
            roundDivision = 1;
        }
        else {
            roundDivision = $(cmbRoundDivision)[0][cmbRoundIndex].innerText;
        }
        // 標準単価の丸め処理
        var defaultPriceDisp = roundDigit(defaultPriceVal, currencyDigit, roundDivision);
        // 画面項目に設定する
        setValue(FormEdit.Purchase.Id, FormEdit.Purchase.DefaultPrice, 1, CtrlFlag.TextBox, defaultPriceDisp, false, false);
    }
    else {
        // 空にする
        setValue(FormEdit.Purchase.Id, FormEdit.Purchase.DefaultPrice, 1, CtrlFlag.TextBox, '', false, false);
    }
}

/**
 *【オーバーライド用関数】ページデータ取得後
 * @param {any} appPath   : ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ 
 * @param {any} btn       : クリックされたボタン要素
 * @param {any} conductId : 機能ID
 * @param {any} pgmId     : プログラムID
 * @param {any} formNo    : 画面番号

 * @param {any} listData  : バックエンド側に渡すデータ(何もしない場合はそのまま返す)
 */
function postGetPageData(appPath, btn, conductId, pgmId, formNo) {

    // 出庫入力画面
    PT0006_postGetPageData(appPath, btn, conductId, pgmId, formNo);
}

/**
 * 詳細画面上部のラベル出力ボタンを活性/非活性の切り替えを行う
 * */
function changeOutLabelBtnStatus() {

    // 予備品情報に「標準棚番」「標準部門」「標準勘定科目」がすべて登録されている場合はTrue、いずれかが登録されていない場合はFalse
    var judgeFlg = convertStrToBool(getValue(FormDetail.PurchaseInfo.Id, FormDetail.PurchaseInfo.IsRegistedRequiredItemToOutLabel, 1, CtrlFlag.Label, false, false));

    // ラベル出力ボタン コントロールを取得
    var btn = getButtonCtrl(FormDetail.Button.OutputLabelDetail);

    // ラベル出力ボタンを活性/非活性状態を変更
    setDisableBtn(btn, !judgeFlg);
}


/**
 * 【オーバーライド用関数】全選択および全解除ボタンの押下後
 * @param  formNo  : 画面番号
 * @param  tableId : 一覧のコントロールID
 */
function afterAllSelectCancelBtn(formNo, tableId) {

    // 出庫入力画面
    PT0006_afterAllSelectCancelBtn(formNo, tableId);
}

/**
 *【オーバーライド用関数】
 *  検索処理前(一覧の選択チェックボックスが選択されているかチェック)
 *
 *  @appPath {string} 　：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @btn {string} 　　　：対象ボタン
 *  @conductId {string} ：機能ID
 *  @pgmId {string} 　　：プログラムID
 *  @formNo {number} 　 ：画面番号
 *  @conductPtn {number}：処理ﾊﾟﾀｰﾝ
 */
function checkSelectedRowBeforeSearchBtnProcess(appPath, btn, conductId, pgmId, formNo, conductPtn) {

    if (conductId == PT0006_ConsuctId) {
        // 出庫入力画面
        return PT0006_checkSelectedRowBeforeSearchBtnProcess(appPath, btn, conductId, pgmId, formNo, conductPtn);
    }

    return true;
}

/**
 *【オーバーライド用関数】登録処理前の「listData」個別取得処理
 * @param {any} appPath   : ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 * @param {any} conductId : 機能ID
 * @param {any} pgmId     : プログラムID
 * @param {any} formNo    : 画面番号
 * @param {any} btn       : クリックされたボタン要素
 * @param {any} listData  : バックエンド側に渡すデータ(何もしない場合はそのまま返す)
 */
function getListDataForRegist(appPath, conductId, pgmId, formNo, btn, listData) {

    if (conductId == PT0006_ConsuctId) {
        // 出庫入力画面
        PT0006_getListDataForRegist(appPath, conductId, pgmId, formNo, btn, listData);
    }

    // 何もしていないのでそのまま返す
    return listData;
}

/**
 * 入出庫履歴タブの表示年度(From・To)の値を取得してグローバル変数に格納
 * @returns
 */
function setDispYearValue() {

    var val = null;

    // 表示年度(From)がグローバル変数に格納されている場合は一度削除する
    if (P_dicIndividual[DispYearKeyName.YearFrom]) {
        delete P_dicIndividual[DispYearKeyName.YearFrom];
    }

    // 表示年度(From)の値を取得
    val = $(P_Article).find("#" + FormDetail.DispCondition.Id + getAddFormNo() + "VAL" + FormDetail.DispCondition.No)[0].value;

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
    val = $(P_Article).find("#" + FormDetail.DispCondition.Id + getAddFormNo() + "VAL" + FormDetail.DispCondition.No + "To")[0].value;

    if (!val) {
        // 入力されていない場合はSQLで扱うことのできる年の最大値を設定
        val = SqlYear.MaxYear;
    }

    // グローバル変数に格納
    P_dicIndividual[DispYearKeyName.YearTo] = val;

}

/**
 * グローバル変数に保持している入出庫履歴タブの表示年度(From・To)の値を削除する
 */
function deleteDispYearValue() {

    // 表示年度(From)がグローバル変数に格納されている場合は一度削除する
    if (P_dicIndividual[DispYearKeyName.YearFrom]) {
        delete P_dicIndividual[DispYearKeyName.YearFrom];
    }

    // 表示年度(To)がグローバル変数に格納されている場合は一度削除する
    if (P_dicIndividual[DispYearKeyName.YearTo]) {
        delete P_dicIndividual[DispYearKeyName.YearTo];
    }
}