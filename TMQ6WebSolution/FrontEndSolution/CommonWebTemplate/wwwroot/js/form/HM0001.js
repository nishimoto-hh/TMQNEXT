/* ========================================================================
 *  機能名　    ：   【HM0001】機器台帳変更管理
 * ======================================================================== */

/**
 * 自身の相対パスを取得
 */
function getPath() {
    var root;
    var scripts = document.getElementsByTagName("script");
    var i = scripts.length;
    while (i--) {
        var match = scripts[i].src.match(/(^|.*\/)HM0001\.js$/);
        if (match) {
            root = match[1];
            break;
        }
    }
    return root;
}
document.write("<script src=\"" + getPath() + "/tmqcommon.js\"></script>");

//機能ID
const ConductId_HM0001 = "HM0001";

// 一覧画面 コントロール番号
const FormList = {
    No: 0, // 画面番号
    Filter: // フィルタ
    {
        Id: "BODY_010_00_LST_0", // コントロールグループID
        ColumnNo: 1              // 項目番号
    },
    List: { // 検索結果一覧
        Id: "BODY_040_00_LST_0",         // コントロールグループID
        ColumnNo: {                      // 項目番号
            MachineNo: 1,                // 機器番号
            MachineName: 2,              // 機器名称
            EquipmentLevel: 3,           // 機器レベル
            District: 4,                 // 地区
            Factory: 5,                  // 工場
            Plant: 6,                    // プラント
            Series: 7,                   // 系列
            Stroke: 8,                   // 工程
            Facility: 9,                 // 設備
            Importance: 10,              // 重要度
            Conservation: 11,            // 保全方式
            InstallationLocation: 12,    // 設置場所
            NumberOfInstallation: 13,    // 設置台数
            DateOfInstallation: 14,      // 設置年月
            ApplicableLaws: 15,          // 適用法規
            MachineNote: 16,             // 機番メモ
            Job: 17,                     // 職種
            Large: 18,                   // 機種大分類
            Middle: 19,                  // 機種中分類
            Small: 20,                   // 機種小分類
            Manufacturer: 21,            // メーカー
            ManufacturerType: 22,        // メーカー型式
            ModelNo: 23,                 // 型式コード
            SerialNo: 24,                // 製造番号
            DateOfManufacture: 25,       // 製造年月
            DeliveryDate: 26,            // 納期
            UseSegment: 27,              // 使用区分
            FixedAssetNo: 30,            // 固定資産番号
            EquipmentNote: 31,           // 機器メモ
            Component: 32,               // 機器別管理基準変更有無
            ApplicationDivisionCode: 39, // 申請区分(拡張項目)
            ValueChanged: 40             // 変更のあった項目
        }
    },
    Button: {                                 // ボタンコントロールID
        New: "New",                           // 新規申請
        ApprovalAllTrans: "ApprovalAllTrans", // 一括承認(画面遷移)※選択行チェックに使用
        DenialAllTrans: "DenialAllTrans",     // 一括否認(画面遷移)※選択行チェックに使用
        ApprovalAll: "ApprovalAll",           // 一括承認(実行)
        DenialAll: "DenialAll",               // 一括否認(実行)
        Output: "Output"                      // 出力
    }
}

// 詳細画面 コントロール番号
const FormDetail = {
    No: 1, // 画面番号
    MachineInfo: { // 機番情報
        Location: { // 地区～設備
            Id: "BODY_020_00_LST_1", // コントロールグループID
            ColumnNo: {              // 項目番号
                District: 1,         // 地区
                Factory: 2,          // 工場
                Plant: 3,            // プラント
                Series: 4,           // 系列
                Stroke: 5,           // 工程
                Facility: 6          // 設備
            }
        },
        Machine: { // 機器番号～保全方式
            Id: "BODY_030_00_LST_1",         // コントロールグループID
            ColumnNo: {                      // 項目番号
                MachineNo: 1,                // 機器番号
                MachineName: 2,              // 機器名称
                EquipmentLevel: 3,           // 機器レベル
                InstallationLocation: 4,     // 設置場所
                NumberOfInstallation: 5,     // 設置台数
                DateOfInstallation: 6,       // 設置年月
                Importance: 7,               // 重要度
                Conservation: 8,             // 保全方式
                ValueChanged: 9,             // 変更のあった項目
                ApplicationDivisionCode: 10, // 申請区分(拡張項目)
                ApplicationStatusCode: 11,   // 申請状況(拡張項目)
                MachineId: 12,               // 機番ID
                HistoyManagementId: 13,      // 変更管理ID
                ProcessMode: 14,             // 処理モード(0：トランザクションモード、1：変更管理モード)
                IsCertified: 15,             // 申請の申請者またはシステム管理者の場合「1」、それ以外は「0」
                IsCertifiedFactory: 16       // 変更管理IDが紐付く機番情報の場所階層IDに設定されている工場の拡張項目がログインユーザIDの場合は「1」それ以外は「0」
            }
        },
        Reason: { // 適用法規～否認理由
            Id: "BODY_040_00_LST_1",  // コントロールグループID
            ColumnNo: {               // 項目番号
                ApplicableLaws: 1,    // 適用法規
                MachineNote: 2,       // 機番メモ
                ApplicationReason: 3, // 申請理由
                RejectionReason: 4    // 否認理由
            }
        }
    },
    EquipmentInfo: { // 機器情報
        Job: { // 職種～機種小分類
            Id: "BODY_050_00_LST_1", // コントロールグループID
            ColumnNo: {              // 項目番号
                Job: 1,              // 職種
                Large: 2,            // 機種大分類
                Middle: 3,           // 機種中分類
                Small: 4             // 機種小分類
            }
        },
        Equipment: { // 使用区分～点検種別毎管理
            Id: "BODY_060_00_LST_1",       // コントロールグループID
            ColumnNo: {                    // 項目番号
                UseSegment: 1,             // 使用区分
                CirculationTargetFlg: 2,   // 循環対象
                FixedAssetNo: 3,           // 固定資産番号
                Manufacturer: 4,           // メーカー
                ManufacturerType: 5,       // メーカー型式
                ModelNo: 6,                // 型式コード
                SerialNo: 7,               // 製造番号
                DateOfManufacture: 8,      // 製造年月
                DeliveryDate: 9,           // 納期
                MaintainanceKindManage: 10 // 点検種別毎管理
            }
        },
        EquipmentNote: { // 機器メモ
            Id: "BODY_070_00_LST_1", // コントロールグループID
            ColumnNo: {              // 項目番号
                EquipmentNote: 1     // 機器メモ
            }
        },
    },
    ManagementStandardsList: { // 保全項目一覧
        Id: "BODY_080_00_LST_1" // コントロールグループID
    },
    Button: {                                                   // ボタンコントロールID
        CopyRequest: "CopyRequest",                             // 複写申請
        ChangeRequest: "ChangeRequest",                         // 変更申請
        DeleteRequest: "DeleteRequest",                         // 削除申請
        ChangeApplicationRequest: "ChangeApplicationRequest",   // 承認依頼
        EditRequest: "EditRequest",                             // 修正
        CancelRequest: "CancelRequest",                         // 取消
        PullBackRequest: "PullBackRequest",                     // 引戻
        ChangeApplicationApproval: "ChangeApplicationApproval", // 承認
        ChangeApplicationDenial: "ChangeApplicationDenial",     // 否認
        BeforeChange: "BeforeChange"                            // 変更前
    },
    CssClass: {
        displayNone: "displayNone" // ボタンの非表示
    },
}

// 処理モード
const ProcessMode =
{
    Transaction: "0", // トランザクションモード
    History: "1"      // 変更管理モード
}

// フラグ判定用定数
const JudgeFlg = {
    False: "0", // False
    True: "1"   // True
}

// 詳細画面の 変更前 ボタンクリック時に表示する機器台帳詳細画面(MC0001)のタブ番号
const tabNoToMC0001 = 1;

/**
 * 【オーバーライド用関数】
 *  初期化処理(表示中画面用)
 * 
 * @param {any} appPath      ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 * @param {any} conductId    ：機能ID
 * @param {any} formNo       ：画面番号
 * @param {any} articleForm  ：表示画面ｴﾘｱ要素
 * @param {any} curPageStatus：画面表示ｽﾃｰﾀｽ
 * @param {any} actionCtrlId ：Action(ﾎﾞﾀﾝなど)CTRLID
 * @param {any} data          :初期表示ﾃﾞｰﾀ
 */
function initFormOriginal(appPath, conductId, formNo, articleForm, curPageStatus, actionCtrlId, data) {

    // 画面番号を判定
    if (formNo == FormList.No) { // 一覧画面

        //フォーカス設定(新規申請ボタンが不可の場合、出力ボタン)
        setFocusButtonAvailable(FormList.Button.New, FormList.Button.Output)
    }
    else if (formNo == FormDetail.No) { // 詳細画面

        // ボタン非表示
        var condition = getConditionToHideButton();        
        commonButtonHideHistory(condition.isTransactionMode, condition.applicationStatusCode, condition.applicationDivisionCode, condition.isCertified, condition.isCertifiedFactory);

        // 背景色変更処理
        changeBackGroundColorDetail();
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

    // 一覧フィルタ処理実施
    if (executeListFilter(transTarget, FormList.List.Id, FormList.Filter.Id, FormList.Filter.ColumnNo)) {
        return [false, conditionDataList];
    }

    // 画面番号を判定
    if (formNo == FormList.No) { // 一覧画面

        // 一括承認(画面遷移)、一括否認(画面遷移)、出力ボタンがクリックされた場合
        if (btn_ctrlId == FormList.Button.ApprovalAllTrans || btn_ctrlId == FormList.Button.DenialAllTrans || btn_ctrlId == FormList.Button.Output) {

            // 一覧にチェックされた行が存在しない場合、遷移をキャンセル
            if (!isCheckedList(FormList.List.Id)) {
                return [false, conditionDataList];
            }
        }

        // クリックされたボタンを判定
        if (btn_ctrlId == FormList.Button.ApprovalAllTrans) { // 一括承認(画面遷移)

            // 遷移をキャンセルして一括承認処理実行
            $(getButtonCtrl(FormList.Button.ApprovalAll)).click()
            return [false, conditionDataList];
        }
        else if (btn_ctrlId == FormList.Button.DenialAllTrans) { // 一括否認(画面遷移)

            // 画面遷移をキャンセルして一括否認処理実行
            $(getButtonCtrl(FormList.Button.DenialAll)).click()
            return [false, conditionDataList];
        }
    }
    else if (formNo == FormDetail.No) { // 詳細画面

        // クリックされたボタンを判定
        if (btn_ctrlId == FormDetail.Button.BeforeChange) { // 変更前

            // 別タブで機器台帳詳細画面(MC0001)を開くための検索条件を設定する(機番ID：非表示項目を取得、タブ番号：1)
            conditionDataList = getParamToMC0001(getValue(FormDetail.MachineInfo.Machine.Id, FormDetail.MachineInfo.Machine.ColumnNo.MachineId, 1, CtrlFlag.Label, false, false), tabNoToMC0001);
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

    // 描画された一覧を判定
    if (id == "#" + FormList.List.Id + getAddFormNo()) { // 一覧画面 機器台帳一覧

        // 背景色変更処理
        commonChangeBackGroundColorHistory(tbl, FormList.List.ColumnNo.ApplicationDivisionCode, FormList.List.ColumnNo.ValueChanged, FormList.List.ColumnNo);
    }
}


/**
 * 詳細画面 ボタン非表示処理の条件作成
 * */
function getConditionToHideButton() {

    var condition = [];
    // トランザクションモードの場合True、変更管理モードの場合False
    condition.isTransactionMode = ProcessMode.Transaction == getValue(FormDetail.MachineInfo.Machine.Id, FormDetail.MachineInfo.Machine.ColumnNo.ProcessMode, 1, CtrlFlag.Label, false, false);

    // 申請状況(10：申請データ作成中、20：承認依頼中、30：差戻中、40：承認済み)
    condition.applicationStatusCode = getValue(FormDetail.MachineInfo.Machine.Id, FormDetail.MachineInfo.Machine.ColumnNo.ApplicationStatusCode, 1, CtrlFlag.Label, false, false);

    // 申請区分(10：新規登録申請、20：変更申請、30：削除申請)
    condition.applicationDivisionCode = getValue(FormDetail.MachineInfo.Machine.Id, FormDetail.MachineInfo.Machine.ColumnNo.ApplicationDivisionCode, 1, CtrlFlag.Label, false, false);

    // 申請の申請者またはシステム管理者の場合True、それ以外はFalse
    condition.isCertified = JudgeFlg.True == getValue(FormDetail.MachineInfo.Machine.Id, FormDetail.MachineInfo.Machine.ColumnNo.IsCertified, 1, CtrlFlag.Label, false, false);

    // 工場の承認ユーザの場合True
    condition.isCertifiedFactory = JudgeFlg.True == getValue(FormDetail.MachineInfo.Machine.Id, FormDetail.MachineInfo.Machine.ColumnNo.IsCertifiedFactory, 1, CtrlFlag.Label, false, false);

    return condition;
}

/**
 * 詳細画面 背景色変更
 * */
function changeBackGroundColorDetail() {

    var valueChanged; // 変更のあった項目名
    var colName;      // 背景色を変更する項目名(列名と申請区分に分割)
    var ctrlId;       // 背景色変更対象の一覧ID 
    var val;          // 背景色変更対象の項目番号

    // 申請区分(拡張項目を取得)
    var applicationDivisionCode = getValue(FormDetail.MachineInfo.Machine.Id, FormDetail.MachineInfo.Machine.ColumnNo.ApplicationDivisionCode, 1, CtrlFlag.Label, false, false);

    // 取得した申請区分を判定
    if (applicationDivisionCode == ApplicationDivision.New || applicationDivisionCode == ApplicationDivision.Delete) { // 新規登録申請・削除申請

        // 変更管理対象項目の全セルの背景色を変更
        valueChanged = getValueChangedAllItem(applicationDivisionCode);
    }
    else { // 変更申請

        // 変更のあった項目名を取得(「|」区切りになっているので分割)
        var valueChanged = getValue(FormDetail.MachineInfo.Machine.Id, FormDetail.MachineInfo.Machine.ColumnNo.ValueChanged, 1, CtrlFlag.Label, false, false).split("|");
    }

    // 変更対象項目より背景色を変更
    valueChanged.forEach(function (columnDetail, colIdx) {

        // 変更のあった項目の列名が存在する場合
        if ((columnDetail.trim()).length) {

            // 列名と申請区分に分割
            colName = columnDetail.split("_");

            // 変更のあった項目を表示している一覧のIDと項目番号を判定
            if (FormDetail.MachineInfo.Location.ColumnNo[colName[0]]) {

                // 地区～設備
                ctrlId = FormDetail.MachineInfo.Location.Id;
                val = FormDetail.MachineInfo.Location.ColumnNo[colName[0]];
            }
            else if (FormDetail.MachineInfo.Machine.ColumnNo[colName[0]]) {

                // 機器番号～保全方式
                ctrlId = FormDetail.MachineInfo.Machine.Id;
                val = FormDetail.MachineInfo.Machine.ColumnNo[colName[0]];
            }
            else if (FormDetail.MachineInfo.Reason.ColumnNo[colName[0]]) {

                // 適用法規～否認理由
                ctrlId = FormDetail.MachineInfo.Reason.Id;
                val = FormDetail.MachineInfo.Reason.ColumnNo[colName[0]];
            }
            else if (FormDetail.EquipmentInfo.Job.ColumnNo[colName[0]]) {

                // 職種～機種小分類
                ctrlId = FormDetail.EquipmentInfo.Job.Id;
                val = FormDetail.EquipmentInfo.Job.ColumnNo[colName[0]];
            }
            else if (FormDetail.EquipmentInfo.Equipment.ColumnNo[colName[0]]) {

                // 使用区分～点検種別毎管理
                ctrlId = FormDetail.EquipmentInfo.Equipment.Id;
                val = FormDetail.EquipmentInfo.Equipment.ColumnNo[colName[0]];
            }
            else if (FormDetail.EquipmentInfo.EquipmentNote.ColumnNo[colName[0]]) {

                // 機器メモ
                ctrlId = FormDetail.EquipmentInfo.EquipmentNote.Id;
                val = FormDetail.EquipmentInfo.EquipmentNote.ColumnNo[colName[0]];
            }
            else {
                return false;
            }

            // 背景色変更処理
            changeBackGroundColorHistory(ctrlId, val, colName[1]);
        }
    });
}

/**
 * 詳細画面 背景色変更時の「変更対象項目」を作成(新規登録申請・削除申請の場合)
 * @param {any} applicationDivisionCode
 */
function getValueChangedAllItem(applicationDivisionCode) {

    // 背景色を変更するセルの項目名を取得
    var list = [];
    list.push(Object.keys(FormDetail.MachineInfo.Location.ColumnNo));        // 地区～設備
    list.push(Object.keys(FormDetail.MachineInfo.Machine.ColumnNo));         // 機器番号～保全方式
    list.push(Object.keys(FormDetail.MachineInfo.Reason.ColumnNo));          // 適用法規～否認理由
    list.push(Object.keys(FormDetail.EquipmentInfo.Job.ColumnNo));           // 職種～機種小分類
    list.push(Object.keys(FormDetail.EquipmentInfo.Equipment.ColumnNo));     // 使用区分～点検種別毎管理
    list.push(Object.keys(FormDetail.EquipmentInfo.EquipmentNote.ColumnNo)); // 機器メモ

    // 「項目名_背景色設定値」のリストを作成
    var valueChanged = [];
    list.forEach(function (item, listIdx) {
        item.forEach(function (name, itemIdx) {

            valueChanged.push(name + "_" + applicationDivisionCode);
        });
    });

    return valueChanged;
}