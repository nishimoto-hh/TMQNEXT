/* ========================================================================
 *  機能名　    ：   【共通・画面共通】
 * ======================================================================== */

/*Public定数：検索結果0件の場合に明細/ボトムエリアを非表示にするかどうか*/
var HideDetailAreaNoResult = false; // true：非表示 / false：表示

/*Public変数：現在表示中の画面NO*/
var P_Article = null;

/*Public変数：現在画面の各formのid*/
var P_formSearchId = null;
var P_formDetailId = null;
var P_formTopId = null;
var P_formBottomId = null;
var P_formEditId = null;

/*Public変数：画面変更ﾌﾗｸﾞ*/
var dataEditedFlg = false;

/*Public変数：システム設定*/
var P_SystemDefines = {
    //COM_SYSTEM_DEFINE実装後、空っぽにする
    conditionLock: 0,   //1:検索後条件ｴﾘｱをロックしない/0:ロックする
};

/*Public変数：翻訳済共通ﾒｯｾｰｼﾞ・ﾎﾞﾀﾝﾒｯｾｰｼﾞ*/
var P_ComMsgTranslated = {};

// 一覧の定義情報取得
var P_listDefines = [];
var P_listDefines2 = [];    //ワーク用

// ﾎﾞﾀﾝの権限情報取得
/* ボタン権限制御 切替 start ================================================ */
//var P_buttonDefine = [];    //ﾎﾞﾀﾝﾘｽﾄ
var P_buttonDefine = {};    //{ 機能ID: ﾎﾞﾀﾝﾘｽﾄ}
/* ボタン権限制御 切替 end ================================================ */

/* Public変数：ﾊﾞｯﾁ機能ﾌﾗｸﾞ※ptn=11、ptn=10のﾊﾞｯﾁ共にtrue */
var P_isBat = false;

// 一覧のｵｰﾄｺﾝﾌﾟﾘｰﾄ定義情報
var P_autocompDefines = [];
// 一覧のｺｰﾄﾞ＋翻訳定義情報
var P_codetransDefines = [];

// 検索条件退避用
var P_SearchCondition = {};
// 個別実装用変数
var P_dicIndividual = {};

// 一覧情報（crtltype=103のみ使用）
var P_listData = {};
// 行削除で削除したデータの退避用（crtltype=103のみ使用）
var P_deleteData = {};
// 一覧の列フィルター情報（crtltype=103のみ使用）
var P_colFilterData = {};

//処理中ﾀｽｸ数
var P_ProcessCnt = 0;

var P_IntervalId = null;    //バッチ用

// ブラウザタブ識別番号
var P_BrowserTabNo = null;

/*Public変数：処理実行中フラグ*/
var P_ProcExecuting = false;

/**Public変数：階層ツリーJSONデータ辞書
 * [構成グループID]:[JSONデータ]*/
var P_TreeViewJsonList = {};
/**Public変数：場所階層メニュー - 選択中の工場ID配列*/
var P_SelectedFactoryIdList = null;
/**Public変数：本務工場ID*/
var P_DutyFactoryId = 0;

/**Public変数：階層ツリーJSONデータ辞書
 * [コンボデータキー]:[JSONデータ]*/
var P_ComboBoxJsonList = {};
/**Public変数：コンボボックスデータ取得中リスト*/
var P_GettingComboBoxDataList = [];

/* コンボボックス制御 start ================================================ */
// コンボボックスデータ取得済みフラグ
var P_ComboDataAcquiredFlg = false;
// コンボボックス情報リスト
var P_ComboBoxMemoryItems = {}; // { ID: アイテム }
/* コンボボックス制御 end   ================================================ */

/**Public変数：1ページ当たりの件数コンボリスト*/
var P_PageRowsCombo = [];

/*Public変数：言語コンボリスト*/
var P_LanguageCombo = [];

/**Public変数：RenderCompleteイベント処理実行フラグ */
var P_ExecRenComFlag = false;
var P_TabulatorSortingFlag = false;
var P_TabulatorFilteringFlag = false;

/*Public変数：無効なキーワード*/
var P_InvalidKeywords = [];

/**Public変数：画面定義翻訳リスト */
var P_DefineTransList = [];

/*Public変数：選択行のみ取得フラグ*/
var P_IsSelectedOnly = false;
/**定義 帳票出力の場合は、以下の機能の時に選択行のみ取得フラグをTrueにする*/
const SelectedConductIdsForOutput = ["RM0001", "PT0002", "PT0003"];
/**定義 登録の場合は、以下の機能の時に選択行のみ取得フラグをTrueにする*/
const SelectedConductIdsForExecute = ["PT0003"];
/**定義 選択行のみ取得フラグがTrueの時以下の画面の一覧は選択行のみを取得*/
const SelectedListInfo = [
    { ProgramId: 'MC0001', CtrlGrpId: 'BODY_020_00_LST_0' }, // 機器台帳一覧
    { ProgramId: 'LN0001', CtrlGrpId: 'BODY_040_00_LST_0' }, // 件名別長期計画一覧
    { ProgramId: 'LN0002', CtrlGrpId: 'BODY_040_00_LST_0' }, // 機器別長期計画一覧
    { ProgramId: 'MA0001', CtrlGrpId: 'BODY_020_00_LST_0' }, // 保全活動一覧
    { ProgramId: 'PT0001', CtrlGrpId: 'BODY_020_00_LST_0' }, // 予備品一覧
    { ProgramId: 'PT0002', CtrlGrpId: 'BODY_030_00_LST_0' }, // 入庫一覧
    { ProgramId: 'PT0002', CtrlGrpId: 'BODY_050_00_LST_0' }, // 出庫一覧
    { ProgramId: 'PT0002', CtrlGrpId: 'BODY_070_00_LST_0' }, // 棚番移庫一覧
    { ProgramId: 'PT0002', CtrlGrpId: 'BODY_090_00_LST_0' }, // 部門移庫一覧
    { ProgramId: 'PT0003', CtrlGrpId: 'BODY_040_00_LST_0' }, // 棚卸データ一覧
];

/**定義 Tabulatorイベント待機ステータス */
const tabulatorEventWaitStatusDef = {
    /** 未設定 */
    None: 0,
    /** TableBuiltingイベント待機中 */
    WaitTableBuilting: 1,
    /** TableBuiltイベント待機中 */
    WaitTableBuilt: 2,

    /** その他のイベント待機中 */
    WaitOtherEvent: 9,
}
/**Public変数：Tabulatorイベント待機ステータス */
var P_TabulatorEventWaitStatus = tabulatorEventWaitStatusDef.None;

/*定義：一覧入力項目の最大入力可能桁数(ﾊﾞｲﾄ数)*/
const maxLengthApp = 255;     //中間ﾃｰﾌﾞﾙのVAL値のｴﾘｱに依存
/*定義：明細一覧上部ボタン表示件数*/
const detailTopHideCount = 30;
/*定義：一覧ﾁｪｯｸﾎﾞｯｸｽのﾁｪｯｸ状態表示値*/
const DispValChecked = "◆";     //中間ﾃｰﾌﾞﾙのVAL値のｴﾘｱに依存

/*定義：システム設定キー*/
const systemDefineKeys = [
    "conditionLock",    // 検索後、条件ｴﾘｱﾛｯｸ
]

/*定義：ﾍﾟｰｼﾞ状態*/
const pageStatus = {
    INIT: 0,   //初期状態
    SEARCH: 1,  //検索後
    SEARCH_AF: 2,  //明細表示後アクション
    UPLOAD: 3,  //ファイル取込後
};

/*定義：ｼｽﾃﾑ - 検索後条件ｴﾘｱﾛｯｸ*/
const conditionLockDef = {
    //ﾛｯｸする
    Lock: 0,
    //ﾛｯｸしない
    UnLock: 1,
};

/*定義：機能 - 処理パターン*/
const conPtn = {
    //入力機能
    Input: 10,
    //バッチ機能
    Bat: 11,
    //帳票機能
    Report: 20,
    //バッチ機能
    Master: 30,
};

/*定義：画面項目 - ｺﾝﾄﾛｰﾙﾀｲﾌﾟ */
const ctrlTypeDef = {
    //ﾃﾞｰﾀ用一覧ﾊﾟﾀｰﾝ①
    IchiranPtn1: 101,
    //ﾃﾞｰﾀ用一覧ﾊﾟﾀｰﾝ②
    IchiranPtn2: 102,
    //ﾃﾞｰﾀ用一覧ﾊﾟﾀｰﾝ③
    IchiranPtn3: 103,
    //ｺﾝﾄﾛｰﾙｸﾞﾙｰﾌﾟ
    ControlGroup: 201,
    //ﾊﾞｯﾁｽﾃｰﾀｽ用定型一覧
    BatStatus: 301,
};

/*定義：画面項目 - 定義ｴﾘｱ区分 */
const areaKbnDef = {
    //条件ｴﾘｱ
    Condition: 1,
    //明細ｴﾘｱ
    List: 2,
    //単票表示ｴﾘｱ
    Input: 3,
    //ﾄｯﾌﾟｴﾘｱ
    Top: 4,
    //ﾎﾞﾄﾑｴﾘｱ
    Bottom: 5,
};

/*定義：画面項目 - 画面遷移ﾊﾟﾀｰﾝ */
const transPtnDef = {
    //画面遷移なし
    None: 0,
    //子画面表示
    Child: 1,
    //単票入力表示
    Edit: 2,
    //単票参照表示
    Reference: 3,
    //共通機能表示
    CmConduct: 4,

    //※内部で使用
    //他機能遷移(別タブ)
    OtherTab: 5,
    //他機能遷移(同タブ表示切替)
    OtherShift: 6,

    ////入力画面ﾎﾟｯﾌﾟｱｯﾌﾟ表示
    //DetailPopup: 2,
    ////入力画面ﾎﾟｯﾌﾟｱｯﾌﾟ表示（更新列なし）
    //DetailUpdNone: 21,
    ////子画面遷移
    //ChildTrans: 3,
    ////子画面ﾎﾟｯﾌﾟｱｯﾌﾟ表示
    //ChildPopup: 4,
}
/*定義：画面項目 - 遷移表示ﾊﾟﾀｰﾝ */
const transDispPtnDef = {
    //設定なし
    None: 0,
    //表示切替
    Shift: 1,
    //ポップアップ
    Popup: 2,
    //一覧直下
    UnderList: 3,
};

/*定義：画面項目 - 一覧編集ﾊﾟﾀｰﾝ*/
const editPtnDef = {
    //設定なし
    None: 0,
    //直接編集可
    Input: 1,
    //直接編集不可
    ReadOnly: 2,
}

/*定義：画面項目 - 行追加区分*/
const rowAddKbnDef = {
    //設定なし
    None: 0,
    //新規行追加
    AddNewRow: 1,
    //新規画面表示
    TransNewForm: 2,
    //新規行・コピー行追加
    AddNewOrCopyRow: 3,
    //新規画面・コピー画面表示
    TransNewOrCopyForm: 4,
}

/*定義：画面項目 - 全選択全解除区分*/
const rowSelKbnDef = {
    //設定なし
    None: 0,
    //頁データ
    CurrentPage: 1,
    //全データ
    AllPages: 2,
    //頁データ・全データ両方
    Both: 3,
}

/*定義：一覧項目 - 表示区分 */
const positionKbnDef = {
    //設定なし
    None: 0,
    //上部
    Upper: 1,
    //下部
    Lower: 2,
    //画面右側
    RightSide: 3,
};

/*定義：一覧項目 - 定義種類 */
const defineTypeDef = {
    //データ項目定義
    DataRow: 1,
    //ヘッダ項目定義
    HeadRow: 2,
    //フッタ項目定義
    FootRow: 3,
};

/*定義：一覧項目 - 表示区分 */
const dispKbnDef = {
    //非表示
    Hide: -1,
    //常に表示
    Disp: 1,
    //デフォルト非表示
    DefHide: 2,
};

/*定義：共通使用区分*/
const actionkbn = {
    /* テーブルに定義するコードは3桁 */
    //検索
    Search: 101,
    //バッチ再表示
    ComBatRefresh: 102,
    //戻る
    Back: 111,
    //選択
    Select: 112,
    // 閉じる
    Close: 113,
    //画面遷移
    FormTransition: 121,
    ////子画面新規
    //ChildNew: 121,
    ////単票入力新規
    //EditNew: 122,
    ////共通機能ﾎﾟｯﾌﾟｱｯﾌﾟ
    //TransCm: 131,
    ////他機能遷移(別ﾀﾌﾞ)
    //TransOtherTab: 132,
    ////他機能遷移(同ﾀﾌﾞ遷移)
    //TransOtherShift: 133,
    //実行
    Execute: 201,
    //削除
    Delete: 211,
    //バッチ実行
    ComBatExec: 221,
    //取込ﾌｫｰﾑﾎﾟｯﾌﾟｱｯﾌﾟ
    Upload: 301,
    //ファイル取込実行
    ComUpload: 302,
    //Excel出力
    Report: 311,
    //Excel出力(非同期)
    ReportHidoki: 312,
    //Excelportダウンロード
    ExcelPortDownload: 313,
    //ExcelPortアップロード
    ExcelPortUpload: 314,
    //クリア
    Clear: 401,
    //個別実装
    Individual: 901,

    /* 内部制御で使用するコードは4桁 */
    /* 1XXX：ユーザーのアクションで直接発火するもの */
    //【共通 - TOP遷移】サイトTOPに遷移
    ComRedirectTop: 1001,
    //【共通 - ログアウト】ログアウト実行
    ComLogout: 1002,
    //【共通 - ＩＤ切替】ＩＤ切替ページに遷移
    ComIdChange: 1003,
    //【共通 - パスワード変更】パスワード変更フォームポップアップ
    ComPassChangeForm: 1004,
    //【共通 - パスワード変更】パスワード変更実行
    ComPassChangeExec: 1005,
    //【共通 - 表示切替】検索条件表示切替
    ComSwitch: 1201,
    //一覧表示切替
    ComSwitchTable: 1211,
    //行追加
    AddNewRow: 1221,
    //行コピー
    AddCopyRow: 1222,
    //行削除
    DeleteRow: 1231,
    //全選択
    SelectAll: 1241,
    //全解除
    CancelAll: 1242,
    //ユーザー単位の表示列設定
    ComSetDispItem: 1251,
    //ユーザー単位の表示列設定適用
    ComSetDispItemExec: 1252,
    //ユーザー単位の表示列設定登録
    ComSetDispItemSave: 1253,
    //詳細検索メニュー表示
    ComDetailSearch: 1254,
    //詳細検索実行
    ComDetailSearchExec: 1255,
    //詳細検索OR条件追加
    ComDetailSearchAddCond: 1256,
    //表示項目カスタマイズ
    ComItemCustomize: 1257,
    ////表示項目カスタマイズ保存
    //ComItemCustomizeSave: 1258,
    //表示項目カスタマイズ適用
    ComItemCustomizeApply: 1258,
    //場所階層、職種機種検索
    ComTreeViewSearch: 1259,
    //階層選択モーダル画面表示
    ComTreeViewShow: 1260,
    //階層選択モーダル画面選択
    ComTreeViewSelect: 1261,
    //言語切り替え
    ChangeLanguage: 1262,

    //データ取得(ページング)
    DataGet: 1301,
    //一覧ソート
    ListSort: 1302,

    /* 2XXX：別のアクションに連動して発火するもの */
    //(未指定)：※初期化用
    None: 2001,
    //【共通 - 画面初期化機能】初期値データ取得
    ComInitForm: 2002,
    //【共通 - 双方向通信】ユーザーID取得
    ComGetUserId: 2003,
    //【共通 - 階層ツリー】構成マスタ情報取得
    ComGetStructureList: 2004,
    //【共通 - 個別画面遷移】部分画面レイアウトデータ取得
    ComGetPartialView: 2005,
    //【共通 - 】共有メモリコンボデータ更新
    ComUpdateComboBoxData: 2006,

};

/*定義：一覧項目 - 画面遷移アクション区分 */
const transDivDef = {
    //設定なし
    None: 0,
    //新規
    New: 1,
    //複製
    Copy: 2,
    //修正
    Edit: 3,
    //出力
    Output: 4,

};

/* 権限管理区分 */
const authControlKbnDef = {
    //権限管理なし
    Free: 0,
    //権限管理あり
    Control: 1,
};

/* 変更不可項目区分 */
const unChangeableKbnDef = {
    //設定なし
    Changeable: 0,
    //変更不可項目
    Unchangeable: 1,
}

/* 表示モード区分 */
const referenceModeKbnDef = {
    //編集モード
    Edit: 0,
    //参照モード
    Reference: 1,
}

/* ﾃﾞｰﾀﾀｲﾌﾟ */
const dataTypeDef = {
    //ﾃﾞｰﾀ値
    Input: 0,
    //更新済みﾊﾞｯｸｱｯﾌﾟﾃﾞｰﾀ
    UpdateBk: 1,
    //新規行
    New: 2,
    //Excel出力ﾃﾞｰﾀ
    Report: 8,
    //ﾌｯﾀｰ値
    Footer: 9,
    //列ｽﾀｲﾙ
    ColCss: 11,
    //行ｽﾀｲﾙ
    RowCss: 12,
    //ﾒｯｾｰｼﾞ確認済みｽﾃｰﾀｽ
    ConfirmStatus: 20,
    //ｴﾗｰ詳細
    ErrorDetail: 30,
    //(未指定)
    None: 99,
}

/* 行ｽﾃｰﾀｽ */
const rowStatusDef = {
    //表示のみ行
    ReadOnly: 0,
    //編集行
    Edit: 1,
    //新規行
    New: 2,
    //削除行
    Delete: 9,
}

/* 実行処理後区分 */
const afterExecKbnDef = {
    //画面に留まる（設定なし）
    None: 0,
    //正常終了後、自動で親画面に戻る
    AutoBack: 1,
}

/*定義：共通使用区分*/
const updtag = {
    //未設定
    None: "0",
    //編集中（入力値）
    Input: "1",
    ////選択中
    //Select: "2",
    ////登録済み
    //Update: "3",
    //一時テーブル登録済み
    UpdateToTmp: "2",
    //登録済み
    Updated: "3",
}

/* ﾎﾞﾀﾝ表示区分 */
const btnDispKbnDef = {
    //非表示
    Hide: 0,
    //活性
    Active: 1,
    //非活性
    Disabled: 2,
}

/*定義：業務ﾛｼﾞｯｸﾌﾟﾛｼｰｼﾞｬ～処理ｽﾃｰﾀｽ*/
const procStatus = {
    //正常終了～処理続行、お知らせ表示
    Valid: 0,
    //警告～処理続行、エラー表示
    Warning: 1,
    //警告～処理続行、エラー状態表示(ﾌﾟﾛｼｰｼﾞｬ処理中断)
    WarnDisp: 2,
    //エラー～処理中断、エラー表示
    Error: 10,
    //確認～処理中断、確認メッセージ表示
    Confirm: 20,
    //異常終了～処理中断
    InValid: 100,

    //== WEB画面からのｴﾗｰ ==
    //ｾｯｼｮﾝﾀｲﾑｱｳﾄ
    TimeOut: 990,
    //ﾛｸﾞｲﾝ不正
    LoginError: 991,
    //ｱｸｾｽ不正
    AccessError: 992,
    //システムエラー
    SystemError: 993,

};

/*定義：ﾌｧｲﾙﾀﾞｳﾝﾛｰﾄﾞ制御ｽﾃｰﾀｽ*/
const FileDownloadSet = {
    //ﾌｧｲﾙ作成、およびﾀﾞｳﾝﾛｰﾄﾞを同期処理にて行う
    Douki: 0,
    //ﾌｧｲﾙ作成、およびﾀﾞｳﾝﾛｰﾄﾞを非同期処理にて行う
    Hidouki: 1,
};

/* 画面初期化時のﾃﾞｰﾀ取得ｽｷｯﾌﾟ区分 */
const skipGetDataDef = {
    //ﾃﾞｰﾀ取得する
    GetData: 0,
    //ｽｷｯﾌﾟする
    skip: 1,
}

/* ﾎﾞﾀﾝのﾃﾞｰﾀ収集範囲 */
const getRangeKbnDef = {
    //画面全体
    AllArea: 0,
    //ｴﾘｱ
    Area: 1,
    //ﾀﾌﾞ
    Tab: 2,
    //一覧
    List: 3,
}

/* 共通機能 戻ると選択の見分け用 */
const selFlgDef = {
    //初期状態、戻る
    Default: 0,
    //選択ﾎﾞﾀﾝｸﾘｯｸ
    Selected: 1,
}

/* 確認ﾒｯｾｰｼﾞ表示区分 */
const confirmKbnDef = {
    //表示しない
    NonDisp: "0",
    //表示する
    Disp: "1",
}

/* ﾊﾞﾘﾃﾞｰｼｮﾝ 有無区分 */
const validKbnDef = {
    //ﾊﾞﾘﾃﾞｰｼｮﾝ無効
    NonValid: "0",
    //ﾊﾞﾘﾃﾞｰｼｮﾝ有効
    Valid: "1",
}

/* リンク列のアイコン区分 */
const iconKbnDef = {
    //ペンアイコン
    PencilIcon: 2,
    //矢印アイコン
    ArrowIcon: 4,
}

/* 定義：構成グループ */
const structureGroupDef = {
    //場所階層
    Location: 1000,
    //場所階層(変更履歴管理工場含まず)
    LocationNoHistory: 1001,
    //場所階層(変更履歴管理工場のみ)
    LocationHistory: 1002,
    //ユーザーマスタ用場所階層
    LocationForUserMst: 1004,
    // 2024/07/08 機器別管理基準標準用場所階層ツリー追加 ADD start
    //機器別管理基準標準用場所階層
    LocationForMngStd: 1006,
    // 2024/07/08 機器別管理基準標準用場所階層ツリー追加 ADD end
    //職種機種
    Job: 1010,
    // 原因性格
    FailureCausePersonality: 1020,
    //系停止
    StopSystem: 1130,
    //突発区分
    Sudden: 1400,
    //MQ分類
    MqClass: 1850,
    //予備品
    Parts: 1040
}

/* ローカルストレージキー データ種別 */
const localStorageCode = {
    //場所階層選択データ
    LocationTree: 10,
    //職種機種選択データ
    JobTree: 11,
    //詳細検索データ
    DetailSearch: 20,
    //項目一覧カスタマイズ
    ItemCustomize: 21,
    //ソート順
    SortOrder: 22,
    //ページ表示件数
    PageSize: 23,
    //スケジュール表示条件
    ScheduleCond: 24,
    //スケジュールデータ更新キーリスト
    ScheduleUpdateKeyList: 25,

}

/** セッションストレージキー データ種別 */
const sessionStorageCode = {
    /** 階層ツリー情報 */
    TreeView: 10,
    /** 階層ツリー選択情報 */
    TreeViewSelected: 11,
    /** ページ番号 */
    PageNo: 20,
    /** コンボボックスマスタデータ */
    CboMasterData: 30,
}

/** ツリービュー定義 */
const treeViewDef = {
    /** 左側ツリーメニュー */
    TreeMenu: { Val: 0, Prefix: 'ltv_', Multiple: true, ThreeState: true },
    /** 詳細検索メニュー */
    DetailSearch: { Val: 1, Prefix: 'dtv_', Multiple: true, ThreeState: true },
    /** 階層選択モーダル画面*/
    ModalForm: { Val: 1, Prefix: 'mtv_', Multiple: false, ThreeState: false },
    /** 階層選択モーダル画面*/
    MultiModalForm: { Val: 1, Prefix: 'mtv_', Multiple: true, ThreeState: true }
}

/*共通ﾎﾞﾀﾝ*/
//【共通 - 初期化処理】初期化処理CtrlId
const ctrlIdInit = "Init";
//【共通 - 戻る処理】戻る処理CtrlId
const ctrlIdBack = "Back";
//【共通 - バッチ機能】バッチ実行CtrlId
const ctrlIdComBatExec = "ComBatExec";
//【共通 - 取り込み機能】取込実行CtrlId
const ctrlIdComUpload = "ComUpload";
//【共通 - 取り込み機能】ファイル選択CtrlId
const ctrlIdComUploadFile = "uploadFile";

/*共通一覧*/
//【共通 - バッチ機能】バッチ実行結果一覧CtrlId
const ctrlIdComBatStatus = "ComBatStatus";

/** 定義 構成マスタ用SQLID */
const sqlIdsStructureMst = ["C0001", "C0002", "C0007"];

/** 定義 構成マスタ-階層番号 */
const structureLayerNo = {
    /** 工場 */
    Factory: 1,
};

/** 定義 工場ID連動用列番号 */
const colNoForFactory = 9999;

/** 定義　年テキストボックス */
const YearText = {
    defVal: "SysYear",
    selector: "input[type = 'text'][value *= 'SysYear']"
}

/** 定義 オートコンプリート区分 */
const autocompDivDef = {
    /** オートコンプリート無し */
    None: "0",
    /** オートコンプリート有り(コード＋名称) */
    Default: "1",
    /** オートコンプリート有り(名称のみ) */
    NameOnly: "2",
    /** オートコンプリート有り(コードのみ) */
    CodeOnly: "3"
}

/** 定義 読込件数 */
const selectCntMaxDef = {
    /** デフォルト値 */
    Default: 50,
    /** すべて */
    All: -1
}

/** 定数　非同期処理の完了制御に使用 */
const promise = {
    // オートコンプリート完了制御
    auto_complete: $.Deferred().resolve("Finished").promise(),
    // 将来的な拡張用サンプル
    // 一度処理を行わないと行えない場合
    add_key: $.Deferred().promise(),
    // 一度も行わなくても行える場合
    add_key2: $.Deferred().resolve("Finished").promise()
};

/** 定義 フィルター使用区分 */
const FilterUseKbnDef = {
    /** 設定なし */
    None: 0,
    /** フィルター使用(部分一致) */
    PartialMatch: 1,
    /** フィルター使用(完全一致) */
    ExactMatch: 2
}

/** 定義 一覧画面へ戻る際に再検索しない機能IDリスト */
const notSearchConductIdList = ["MA0001", "MC0001", "MC0002", "LN0001", "DM0001", "PT0001"];

/** 定義 実行処理完了後に再検索しない機能IDとコントロールIDのペアのリスト */
const notSearchIdListForExecute = [["DM0001","Delete"]];

//jquery-ui-datepickerとの競合防止
//var bootstrapDatepicker = $.fn.datepicker.noConflict();
//var bootstrapDatepicker = $.fn.datepicker();
//$.fn.bootstrapDP = bootstrapDatepicker;

// String.prototype.formatが未定義の場合は定義する
if (!String.prototype.format) {
    String.prototype.format = function () {
        var args;
        args = arguments;
        if (args.length === 1 && args[0] !== null && typeof args[0] === 'object') {
            args = args[0];
        }
        return this.replace(/{([^}]*)}/g, function (match, key) {
            return (typeof args[key] !== "undefined" ? args[key] : match);
        });
    };
}

function setSystemDefines(dicSystemDefines) {
    if (dicSystemDefines != null && Object.keys(dicSystemDefines).length > 0) {
        P_SystemDefines = dicSystemDefines;
    }
}

/**
 * 画面編集フラグをtrueにする
 * 行編集フラグを編集中(1)にする
 */
function setupDataEditedFlg(tr) {
    dataEditedFlg = true;
    if (tr != null) {
        var updflg = $(tr).find("input[data-type='updflg']");
        if (updflg != null) {
            $(updflg).val(updtag.Input);
        }
    }
}

/**
 * 行編集フラグを未設定(0)にする
 */
function clearDataEditedFlg(targetId) {
    var updflgs = $(targetId).find("input[data-type='updflg']");
    if (updflgs != null && updflgs.length > 0) {
        $.each($(updflgs), function (i, flg) {
            $(flg).val(updtag.None);
        });
    }
}

/**
 * 画面編集ﾌﾗｸﾞ制御ｲﾍﾞﾝﾄﾊﾝﾄﾞﾗを追加/削除する
 * @param {bool}     on      ：true：onにする、false：offにする
 * @param {html}     trs     ：tr要素指定
 * @param {string}   areaId  ：ｴﾘｱ指定※tr指定がnullの場合は必須
 */
function setEventForEditFlg(on, trs, areaId) {
    if (trs == null || trs.length == 0) {
        trs = $(P_Article).find(areaId).find(".ctrlId").find("tr:not([class^='base_tr'])");
    }
    //var controlItems = $(tr).find("td.control:not([data-name='SELTAG'])");
    var controlItems = $(trs).find("td.control:not('.reference_mode'),.tabulator-cell:not('.readonly')");
    if ($(controlItems).length) {
        if (on) {
            $.each($(trs), function (i, tr) {
                $(tr).on("change.flg",
                    ":text, :checkbox, [type='date'], [type='time'], [type='datetime-local'], select:not('.DispCount'), ul, textarea, :radio",
                    function (event, param) {
                        //changeイベントのparamが設定されている場合は行編集フラグを変更しない
                        if (!param) {
                            setupDataEditedFlg(tr);
                        }
                    });
            });
        }
        else {
            $(controlItems).off("change.flg",
                ":text, :checkbox, [type='date'], [type='time'], [type='datetime-local'], select, ul, textarea, :radio");
        }
    }
    controlItems = null;
}

/**
 *  ﾍﾟｰｼﾞの状態をｽﾃｰﾀｽに合わせて設定
 *  @param {string} ：対象セレクタ
 *  @batFlg {int}   ：1(バッチ機能)
 */
function setPageStatus(status, pageRowCount, conductPtn, isTab) {

    var formNo = $(P_Article).data("formno");
    //条件ｴﾘｱ
    var conditionArea = $(P_Article).find("#" + P_formSearchId);
    var conditionElements = $(conditionArea).find(".action_search_div, .search_div");
    var conditionCtrlGroup = $(conditionArea).find(".action_search_div");
    // 明細ｴﾘｱ
    var detailArea = $(P_Article).find("#" + P_formDetailId);
    var detailLists = $(detailArea).find(".detail_div").children().not("[id$='edit_div']"); //明細ｴﾘｱ一覧
    var detailCtrlGroup = $(detailArea).find(".action_detail_div"); //明細ｴﾘｱｺﾝﾄﾛｰﾙｸﾞﾙｰﾌﾟ
    //単票表示ﾚｲｱｳﾄ
    var editDiv = $(detailArea).find("div[id$='edit_div']");
    //ﾎﾞﾄﾑｴﾘｱ
    var bottomArea = $(P_Article).find("#" + P_formBottomId);
    var bottomLists = $(bottomArea).find(".bottom_div").children(); //ﾎﾞﾄﾑｴﾘｱ一覧
    var bottomCtrlGroup = $(bottomArea).find(".action_bottom_div");

    switch (status) {
        case pageStatus.SEARCH:    //検索⇒明細表示時
        case pageStatus.UPLOAD:    //ファイル取込⇒明細表示時
            //$(P_Article).find(".action_search_div").removeClass("hide");

            if (conductPtn == conPtn.Bat) {
                //※バッチ機能の場合、検索条件をロックしない
                setDisableElements(conditionElements, false);   //条件ｴﾘｱ一覧ﾛｯｸ解除
                setHide(conditionCtrlGroup, false);             //検索条件ｴﾘｱｺﾝﾄﾛｰﾙｸﾞﾙｰﾌﾟ表示
            }
            else {
                //var btn = $('input:button[data-actionkbn="' + actionkbn.Search + '"]');
                //if (btn.length <= 0) {
                //    btn = $('input:button[data-actionkbn="' + actionkbn.Upload + '"]');
                //}

                //if (true == P_isBat || P_SystemDefines.conditionLock == conditionLockDef.UnLock) {
                // クリアボタンを取得
                var clearBtn = $(P_Article).find('input:button[data-actionkbn="' + actionkbn.Clear + '"]');
                if (true == P_isBat || (P_SystemDefines.conditionLock == conditionLockDef.UnLock ||
                    !clearBtn || clearBtn.length == 0)) {
                    // バッチ機能、条件エリアをロックしない設定、クリアボタンが存在しない場合
                    // 検索条件をロックせず、検索ボタン表示
                    setDisableElements(conditionElements, false);       //検索条件ｴﾘｱ入力項目ﾛｯｸ解除
                    setHide(conditionCtrlGroup, false); //検索条件ｴﾘｱｺﾝﾄﾛｰﾙｸﾞﾙｰﾌﾟ表示
                }
                else {
                    //検索条件ｴﾘｱをﾛｯｸ
                    setDisableElements(conditionElements, true);       //検索条件ｴﾘｱ入力項目ﾛｯｸ
                    setHide(conditionCtrlGroup, true); //検索条件ｴﾘｱｺﾝﾄﾛｰﾙｸﾞﾙｰﾌﾟ非表示
                }

                if (conductPtn == conPtn.Master) {
                    //※マスタ機能の場合

                    //　検索後⇒登録ボタンを非活性
                    //　ファイル取込後⇒登録ボタンを活性
                    var isDisable = false;
                    if (status == pageStatus.SEARCH) {
                        isDisable = true;
                    }

                    // 権限ありボタン　活性／非活性
                    var registBtn = $(P_Article).find('.action_detail_div input:button.func_button').not($('input:button[data-authkbn=""]'));
                    if (registBtn != null && registBtn.length > 0) {
                        setDisableBtn(registBtn, isDisable);
                    }
                }
            }

            // 明細ｴﾘｱを表示
            setInitHide(detailLists, false);        //一覧表示
            setInitHide(detailCtrlGroup, false);    //ｺﾝﾄﾛｰﾙｸﾞﾙｰﾌﾟ表示
            // 単票表示ﾚｲｱｳﾄを非表示
            $(editDiv).addClass("hide");
            // 詳細検索メニュー・項目カスタマイズメニューを非表示
            setHide('#detail_search', true);
            setHide('#item_customize', true);

            /* AKAP2.0 START */
            //ﾎﾞﾄﾑｴﾘｱ
            // - 表示
            setInitHide(bottomLists, false);     //一覧表示
            setInitHide(bottomCtrlGroup, false); //ｺﾝﾄﾛｰﾙｸﾞﾙｰﾌﾟ表示
            /* AKAP2.0 END */

            // 1データが複数行の場合のhover設定
            //$('.detail_div table.transLink tbody tr').hover(
            $('.ctrlId:not(.vertical_tbl) tbody tr').hover(
                function () {
                    var rowno = $(this).data('rowno');
                    // data-rownoが同じ行もhoverする
                    $(this).closest("table").find('tr[data-rowno="' + rowno + '"] td').addClass('table_hover');
                },
                function () {
                    var rowno = $(this).data('rowno');
                    // data-rownoが同じ行のhover解除
                    $(this).closest("table").find('tr[data-rowno="' + rowno + '"] td').removeClass('table_hover');
                }
            );

            if (!isTab) {
                var tabBtns = $(P_Article).find(".tab_btn.detail a");
                //先頭ﾀﾌﾞﾎﾞﾀﾝをｸﾘｯｸ
                if (tabBtns != null && tabBtns.length > 0) {
                    // 選択中のタブ番号を取得
                    var tabNoKey = getSelectedFormTabNoKey(tabBtns[0]);
                    var selectedTabNo = getSelectedFormTabNo(tabNoKey);

                    $.each($(tabBtns), function (i, btn) {
                        //$(btn).click();
                        //return false;   //ﾙｰﾌﾟを抜ける
                        if (selectedTabNo == 0) {
                            $(btn).click();
                            return false;   //ﾙｰﾌﾟを抜ける
                        } else {
                            var tabNo = $(btn).data('tabno');
                            if (selectedTabNo == tabNo) {
                                clearSelectedFormTabNo(tabNoKey);
                                $(btn).click();
                                return false;   //ﾙｰﾌﾟを抜ける
                            }
                        }
                    });
                }
            }

            //ページャーの総ページ数が1件の場合、ページャーを非表示
            var pagination = $(P_Article).find('.paginationCommon[data-option="def"]');
            if (pagination != null && pagination.length > 0) {
                $.each(pagination, function () {
                    var ctrlId = $(this).data("ctrlid");

                    var pageCount = getPageCountAll(ctrlId);
                    var hideFlg = false;
                    if (pageCount <= 1) {
                        hideFlg = true;  //非表示
                    }
                    setHide($(P_Article).find(".paginationCommon[data-ctrlid='" + ctrlId + "']"), hideFlg);  //表示設定

                });
            }

            //一覧下部のﾍﾟｰｼﾞｬｰの表示制御（非表示にする）
            // ※一覧件数が一定数を超えない場合
            // ※ﾀﾌﾞ切り替え画面の場合
            hideLowerPagination(pageRowCount);

            $('html,body').animate({ scrollTop: 0 }, '1');      //ｽｸﾛｰﾙを先頭へ移動
            $(P_Article).find(".detail_div").focus();

            if (conductPtn == conPtn.Bat) {
                //11:バッチ機能の場合、検索条件エリアにフォーカスを当てる
                $(P_Article).find(".search_div").focus();
            }

            if (status == pageStatus.SEARCH) {
                // 検索⇒明細表示時
                if (HideDetailAreaNoResult) {
                    // 検索ボタン
                    var searchBtn = $(P_Article).find('input:button[data-actionkbn="' + actionkbn.Search + '"]');
                    if (searchBtn != null && searchBtn.length > 0) {
                        // 一覧件数が0件の場合、明細ｴﾘｱ、ﾎﾞﾄﾑｴﾘｱを非表示
                        if (pageRowCount == 0) {
                            // 明細ｴﾘｱを表示
                            setInitHide(detailLists, true);        //一覧表示
                            setInitHide(detailCtrlGroup, true);    //ｺﾝﾄﾛｰﾙｸﾞﾙｰﾌﾟ表示

                            /* AKAP2.0 START */
                            //ﾎﾞﾄﾑｴﾘｱ
                            // - 表示
                            setInitHide(bottomLists, true);        //一覧表示
                            setInitHide(bottomCtrlGroup, true);    //ｺﾝﾄﾛｰﾙｸﾞﾙｰﾌﾟ表示
                            /* AKAP2.0 END */
                        }
                    }
                }
            }

            break;
        case pageStatus.SEARCH_AF:  //明細表示後アクション

            //一覧下部のﾍﾟｰｼﾞｬｰの表示制御（非表示にする）
            // ※一覧件数が一定数を超えない場合
            // ※ﾀﾌﾞ切り替え画面の場合
            hideLowerPagination(pageRowCount);

            $('html,body').animate({ scrollTop: 0 }, '1');      //ｽｸﾛｰﾙを先頭へ移動
            $(P_Article).find(".detail_div").focus();

            break;
        default:        //初期状態
            //条件ｴﾘｱ
            // - ﾛｯｸ解除
            setDisableElements(conditionElements, false);      //検索条件ｴﾘｱ入力項目ﾛｯｸ解除
            // - 表示               
            var switchBtn = $(conditionArea).find('a[data-actionkbn="' + actionkbn.ComSwitch + '"]');   // 【共通】検索条件表示切替ボタン
            $.each(switchBtn, function () {
                var id = $(this).data('switchid') + "_" + formNo;
                setHideId(id, false);     //表示
                //詳細情報があれば非表示
                var detail = $(P_Article).find('div[id="' + id + '"][data-dispkbn="' + dispKbnDef.DefHide + '"]');
                if (detail.length > 0) {
                    setHideId(id, true);     //非表示
                }
            });

            var switchTableBtn = $(conditionArea).find('a[data-actionkbn="' + actionkbn.ComSwitchTable + '"]');   // 【共通】一覧表示切替ボタン
            $.each(switchTableBtn, function () {
                var id = $(this).data('switchid') + "_" + formNo;
                $("#" + id).show();     //表示
            });
            setHide(conditionCtrlGroup, false); //検索条件ｴﾘｱｺﾝﾄﾛｰﾙｸﾞﾙｰﾌﾟ表示

            //※ﾊﾞｯﾁ使用機能の場合、明細ｴﾘｱ・ﾎﾞﾄﾑｴﾘｱは表示しておく
            var hideFlg = true;
            if (true == P_isBat) {
                hideFlg = false;
            }
            // 明細ｴﾘｱ
            setInitHide(detailLists, hideFlg);     //一覧
            if (hideFlg) {
                //選択中のタブ内容も非表示にする
                $(detailLists).filter(".tab_contents.selected").removeClass('selected');
            }
            setInitHide(detailCtrlGroup, hideFlg); //ｺﾝﾄﾛｰﾙｸﾞﾙｰﾌﾟ
            // 単票表示ﾚｲｱｳﾄを非表示
            $(editDiv).addClass("hide");

            /* AKAP2.0 START */
            //ﾎﾞﾄﾑｴﾘｱ
            setInitHide(bottomLists, hideFlg);     //一覧
            setInitHide(bottomCtrlGroup, hideFlg); //ｺﾝﾄﾛｰﾙｸﾞﾙｰﾌﾟ
            /* AKAP2.0 END */

            $('html,body').animate({ scrollTop: 0 }, '1');      //ｽｸﾛｰﾙを先頭へ移動
            $(conditionArea).find(".search_div").focus();

            break;
    }

    ////ﾎﾞﾀﾝ処理ｽﾃｰﾀｽを反映
    //setButtonStatus(authShori);

    // 【オーバーライド用関数】画面状態設定後
    setPageStatusEx(status, pageRowCount, conductPtn, formNo);
}

/**
 *  ﾎﾞﾀﾝ処理ｽﾃｰﾀｽを反映
 */
/* ボタン権限制御 切替 start ================================================ */
//function setButtonStatus(authShori) {

//    var btns = $(P_Article).find('input:button[data-actionkbn]');
//    $.each($(btns), function (idx, btn) {
//        var btnCtrlid = $(btn).attr("name");
//        var bStatus = btnDispKbnDef.Active; //活性
//        if (authShori != null && authShori[btnCtrlid]) {
//            bStatus = authShori[btnCtrlid];
//        }
//        setButtonStatusDetail(btn, bStatus);
//    });
//}
/* ============================================================================== */
function setButtonStatus() {

    if (P_buttonDefine != null) {
        var conductId = $(P_Article).find("form[id^='formTop'] input:hidden[name='CONDUCTID']").val();  //表示中機能ID
        var formNo = $(P_Article).attr("data-formno");                                                  //表示中画面NO
        $.each(P_buttonDefine, function (curId, btnDefines) {
            if (curId != conductId) {
                return true;    //continue
            }
            //表示中機能についてのみ制御
            $.each(btnDefines, function (idx, btnDefine) {
                var curFormNo = btnDefine.FORMNO;
                if (curFormNo != formNo) {
                    return true;    //continue
                }
                var btn = $(P_Article).find("input:button[name='" + btnDefine.CTRLID + "']");
                if ($(btn).length) {
                    var dispKbn = btnDefine.DISPKBN + "";   //文字列化
                    setButtonStatusDetail(btn, dispKbn);
                }
            });
        });
    }
}
/* ボタン権限制御 切替 end ================================================== */

function setButtonStatusDetail(btn, status) {

    var btnTd = null;
    var ctrlGr = $(btn).closest(".actionlist");
    if ($(ctrlGr).length) {
        btnTd = $(btn).closest("td");
    }

    //ﾎﾞﾀﾝ処理ｽﾃｰﾀｽを反映
    switch (status) {
        case btnDispKbnDef.Active + "":   // 1：活性
            if (isHide(btn)) {
                setHide(btn, false);        //表示
                if ($(btnTd).length) {
                    setHide(btnTd, false);  //表示
                }
            }
            setDisableBtn(btn, false);  //ﾎﾞﾀﾝを活性化
            break;
        case btnDispKbnDef.Disabled + "":   //2：非活性
            if (isHide(btn)) {
                setHide(btn, false);        //表示
                if ($(btnTd).length) {
                    setHide(btnTd, false);  //表示
                }
            }
            setDisableBtn(btn, true);   //ﾎﾞﾀﾝを非活性化
            break;
        default:    //非表示
            if (false == isHide(btn)) {
                setHide(btn, true);         //非表示
                if ($(btnTd).length) {
                    setHide(btnTd, true);   //非表示
                }
            }
            break;
    }
}

/**
 *  連動ｺﾝﾎﾞの状態を生成中かどうかに合わせて設定
 *  @param {select要素} ：対象ｺﾝﾎﾞ要素
 *  @param {bool} ：true(処理中)
 */
function setComboProcessStatus(selector, isProcess) {

    if (isProcess) {
        //※処理中

        if (P_ProcessCnt <= 0) {
            //一覧非活性
            var tables = $(selector).closest('table,.tabulator');    //直近の親要素
            if (tables.length > 0) {
                var tableId = getTableId(tables[0]);
                setDisableId(tableId, true);
            }

            //18.11.22 start 処理中ﾒｯｾｰｼﾞ表示を廃止⇒処理完了時にｴﾗｰﾒｯｾｰｼﾞも消えてしまうため
            ////処理中メッセージ：on, 1:連動ｺﾝﾎﾞ処理中ﾓｰﾄﾞ
            //processMessage(true, 1);
            //18.11.22 end
            P_ProcessCnt = 0;
        }
        P_ProcessCnt++;     //処理中ﾀｽｸ数ｶｳﾝﾄｱｯﾌﾟ
    }
    else {
        //※処理待機中

        if (P_ProcessCnt > 0) {
            P_ProcessCnt--;     //処理中ﾀｽｸ数ｶｳﾝﾄﾀﾞｳﾝ
            if (P_ProcessCnt <= 0) {
                //一覧活性
                var tables = $(selector).closest('table,.tabulator');    //直近の親要素
                if (tables.length > 0) {
                    var tableId = getTableId(tables[0]);
                    setDisableId(tableId, false);
                }

                //18.11.22 start 処理中ﾒｯｾｰｼﾞ表示を廃止⇒処理完了時にｴﾗｰﾒｯｾｰｼﾞも消えてしまうため
                ////処理中メッセージ：off, 1:連動ｺﾝﾎﾞ処理中ﾓｰﾄﾞ
                //processMessage(false, 1);
                //18.11.22 end
                P_ProcessCnt = 0;
            }

        }

    }

}

/**
 *  カレンダーの初期化処理
 *  @param {string} ：対象セレクタ
 *  @param {string} ：一覧再表示時、true
 */
function initDatePicker(selector, isReset) {
    //年月日の初期化
    var dateFmtStr = "";
    var minDateStr = "";
    var maxDateStr = "";
    var numberOfMonths = 1;
    if (!isReset) {
        //※画面初期化時

        var fmt = $(selector).data("format") + '';

        var viewModeVal = 2;    //年→月→日と設定
        var minViewMode = 2;    //"年"まで選択できる

        //書式文字列を生成
        dateFmtStr = fmt;
        if (fmt.indexOf('y') > -1) {
            // jQueryのdatepickerを使用する場合、yyyy→yyに変更
            if (fmt.indexOf("yyyy") > -1) {
                dateFmtStr = dateFmtStr.replace("yyyy", "yy")
            } else if (fmt.indexOf("yy") > -1) {
                dateFmtStr = dateFmtStr.replace("yy", "y")
                // APより移行　二重に置き換えが行われた場合は元に戻す
                var count = (dateFmtStr.match(new RegExp("y", "g")) || []).length;
                if (count == 1) {
                    dateFmtStr = dateFmtStr.replace("y", "yy");
                }
            }
        }
        if (fmt.indexOf('m') > -1) {
            viewModeVal = 1;    //月→日と設定
            minViewMode = 1;    //"月"まで選択できる
        }
        if (fmt.indexOf('d') > -1) {
            viewModeVal = 0;    //(ﾃﾞﾌｫﾙﾄ)日を設定
            minViewMode = 0;    //(ﾃﾞﾌｫﾙﾄ)"日付"まで設定できる
        }
        if (dateFmtStr.length <= 0) {
            //未設定の場合、ﾃﾞﾌｫﾙﾄﾌｫｰﾏｯﾄ
            // jQueryのdatepickerを使用する場合、yyyy→yyに変更
            //dateFmtStr = "yyyy/mm/dd";
            dateFmtStr = "yy/mm/dd";

            viewModeVal = 0;    //(ﾃﾞﾌｫﾙﾄ)日を設定
            minViewMode = 0;    //(ﾃﾞﾌｫﾙﾄ)"日付"まで設定できる
        }
        //ﾌｫｰﾏｯﾄを上書き
        setAttrByNativeJs(selector, "data-format", dateFmtStr);

        //初期値を設定
        var defVal = $(selector).data("def");
        if (defVal != null && defVal.length > 0) {
            //初期値が設定されている場合、datepickerの日付ｾｯﾄ

            if (defVal == "SysDate") {
                // 初期値に「SysDate」が設定されている場合、現在日時をセットする

                //書式フォーマット変換
                var now = new Date();

                var y = now.getFullYear();

                var m = ("00" + (now.getMonth() + 1)).slice(-2);
                var d = ("00" + now.getDate()).slice(-2);

                var setStr = "";
                if (dateFmtStr.indexOf('yy') > -1) {
                    setStr = dateFmtStr.replace('yy', y);
                } else if (dateFmtStr.indexOf('y') > -1) {
                    setStr = dateFmtStr.replace('y', y.toString().substring(2, 4));
                }
                setStr = setStr.replace('mm', m);
                setStr = setStr.replace('dd', d);

                $(selector).val(setStr);
            }
        }
        //値がｾｯﾄされている場合は戻す
        var setVal = $(selector).data("value");
        if (setVal != null && setVal.length > 0) {
            $(selector).val(setVal);
        }

        // 日付選択範囲
        minDateStr = $(selector).data("minval");
        maxDateStr = $(selector).data("maxval");

        // 表示する月数
        var maxlength = $(selector).data("maxlength");
        if (!(maxlength == null || maxlength == "")) {
            if (1 <= maxlength && maxlength <= 4) {
                numberOfMonths = $(selector).data("maxlength");
            }
        }

    }
    else {
        dateFmtStr = $(selector).data("format");
    }

    //  format  日付フォーマット
    //  --------------------------------------------
    //  format: 'dd/mm/yyyy'
    //      d, dd - 日(dd：0埋めを行う)
    //      D, DD - 曜日(D：Mon / DD：Monday)
    //      m, mm, M, MM - 月(m：1, mm：01, M：Jan, MM：January)
    //      yy, yyyy - 年(2桁もしくは4桁)
    //      セパレータ(区切り文字)は'/'(スラッシュ)もしくは'-'(ハイフン)

    //  viewMode  選択モード
    //  --------------------------------------------
    //  viewMode: 0     日を設定(デフォルト)
    //  viewMode: 1     月→日と設定
    //  viewMode: 2     年→月→日と設定

    //  minViewMode  選択
    //  --------------------------------------------
    //  minViewMode: 0  日付まで設定できる
    //  minViewMode: 1  月まで選択できる
    //  minViewMode: 2  年まで選択できる

    /*
    * datepickerの「今日」ボタンが機能しない標準バグを解消
    * override "Today" button to also grab the time and set it to input field.
    */
    //$.datepicker._base_gotoToday = $.datepicker._gotoToday;
    $.datepicker._gotoToday = function (id) {
        var inst = this._getInst($(id)[0]);
        this._base_gotoToday(id);
        var tp_inst = this._get(inst, 'timepicker');
        if (!tp_inst) {
            tp_inst = this._get(inst, 'datepicker');
            var date = new Date();
            this._setDate(inst, date);
            this._hideDatepicker();
        }
        else {
            var tzoffset = $.timepicker.timezoneOffsetNumber(tp_inst.timezone);
            var now = new Date();
            now.setMinutes(now.getMinutes() + now.getTimezoneOffset() + parseInt(tzoffset, 10));
            this._setTime(inst, now);
            this._setDate(inst, now);
            tp_inst._onSelectHandler();
            this._hideDatepicker();
        }

    };

    // 日付を表示する場合
    $(selector).datepicker({
        dateFormat: dateFmtStr,
        clearBtn: false,
        viewMode: viewModeVal,
        minViewMode: minViewMode,
        todayHighlight: true,
        autoclose: true
        , forceParse: false
        , numberOfMonths: numberOfMonths
        , monthNames: [
            P_ComMsgTranslated[911460001], P_ComMsgTranslated[911470001], P_ComMsgTranslated[911480001], P_ComMsgTranslated[911490001],
            P_ComMsgTranslated[911500001], P_ComMsgTranslated[911510001], P_ComMsgTranslated[911520001], P_ComMsgTranslated[911530001],
            P_ComMsgTranslated[911540001], P_ComMsgTranslated[911460002], P_ComMsgTranslated[911460003], P_ComMsgTranslated[911460004]
        ] //["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"]
        , monthNamesShort: [
            P_ComMsgTranslated[911460001], P_ComMsgTranslated[911470001], P_ComMsgTranslated[911480001], P_ComMsgTranslated[911490001],
            P_ComMsgTranslated[911500001], P_ComMsgTranslated[911510001], P_ComMsgTranslated[911520001], P_ComMsgTranslated[911530001],
            P_ComMsgTranslated[911540001], P_ComMsgTranslated[911460002], P_ComMsgTranslated[911460003], P_ComMsgTranslated[911460004]
        ] //['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月']
        , yearSuffix: P_ComMsgTranslated[911240001] //年
        , showMonthAfterYear: true
        , closeText: P_ComMsgTranslated[911200009] //閉じる
        , prevText: P_ComMsgTranslated[911310001] //&#x3C;前
        , nextText: P_ComMsgTranslated[911180003] //次&#x3E;
        , currentText: P_ComMsgTranslated[911070007] //今日
        , dayNames: [
            P_ComMsgTranslated[911220002], P_ComMsgTranslated[911090007], P_ComMsgTranslated[911060004], P_ComMsgTranslated[911130003],
            P_ComMsgTranslated[911350001], P_ComMsgTranslated[911070008], P_ComMsgTranslated[911200007],
        ] //['日曜日', '月曜日', '火曜日', '水曜日', '木曜日', '金曜日', '土曜日']
        , dayNamesShort: [
            P_ComMsgTranslated[911270005], P_ComMsgTranslated[911090008], P_ComMsgTranslated[911060005], P_ComMsgTranslated[911130004],
            P_ComMsgTranslated[911350002], P_ComMsgTranslated[911070009], P_ComMsgTranslated[911200008],
        ] //['日', '月', '火', '水', '木', '金', '土']
        , dayNamesMin: [
            P_ComMsgTranslated[911270005], P_ComMsgTranslated[911090008], P_ComMsgTranslated[911060005], P_ComMsgTranslated[911130004],
            P_ComMsgTranslated[911350002], P_ComMsgTranslated[911070009], P_ComMsgTranslated[911200008],
        ] //['日', '月', '火', '水', '木', '金', '土']
        , weekHeader: P_ComMsgTranslated[911120023] //週
        , firstDay: 0
        , isRTL: false
        , showButtonPanel: true
        , showOtherMonths: true
        , selectOtherMonths: true
        , minDate: minDateStr
        , maxDate: maxDateStr
        , changeYear: true
        , changeMonth: true
        , onClose: function () {
            $(this).focus();
        }
    });

    // ｱｲｺﾝｸﾘｯｸで選択ｴﾘｱ展開
    var icon = $(selector).closest("div").find(".date_icon");
    $(icon).on('click', function () {
        var input = $(icon).closest("div").find("input[type='text']");
        $(input).focus();
    });
    icon = null;
}

/**
 *  時刻テキストの初期化処理
 *  @param {string} ：対象セレクタ
 */
function initTimePicker(selector) {
    //時刻の初期化

    var fmt = $(selector).data("format");

    var timeFmtStr = fmt;
    if (fmt.indexOf('h') > -1) {
        if (fmt.indexOf("hh") > -1) {
            timeFmtStr = timeFmtStr.replace("hh", "HH")
        } else if (fmt.indexOf("h") > -1) {
            timeFmtStr = timeFmtStr.replace("h", "H")
        }
    }
    if (timeFmtStr.length <= 0) {
        //未設定の場合、ﾃﾞﾌｫﾙﾄﾌｫｰﾏｯﾄ
        timeFmtStr = "HH:mm";
    }
    //ﾌｫｰﾏｯﾄを上書き
    setAttrByNativeJs(selector, "data-format", timeFmtStr);

    // 初期値に「SysDate」が設定されている場合、現在日時をセットする
    var setDefault = ($(selector).data("def") == "SysDate");

    if (setDefault) {
        // 初期値(現在日時)をセット
        var time = new Date();
        var hour = time.getHours();
        var minutes = time.getMinutes();
        var seconds = time.getSeconds();

        var timeStr = "";
        if (timeFmtStr.indexOf("HH") > -1) {
            timeStr = timeFmtStr.replace('HH', ("00" + hour).slice(-2));
        } else if (timeFmtStr.indexOf("H") > -1) {
            timeStr = timeFmtStr.replace('H', hour);
        }
        if (timeFmtStr.indexOf("mm") > -1) {
            timeStr = timeStr.replace('mm', ("00" + minutes).slice(-2));
        } else if (timeFmtStr.indexOf("m") > -1) {
            timeStr = timeStr.replace('m', minutes);
        }
        if (timeFmtStr.indexOf("ss") > -1) {
            timeStr = timeStr.replace('ss', ("00" + seconds).slice(-2));
        } else if (timeFmtStr.indexOf("s") > -1) {
            timeStr = timeStr.replace('s', seconds);
        }

        $(selector).val(timeStr);
    }
    //値がｾｯﾄされている場合は戻す
    var setVal = $(selector).data("value");
    if (setVal != null && setVal.length > 0) {
        $(selector).val(setVal);
    }

    // jQuery ui datepickerにアドオンしたtimepickerを使用
    $(selector).timepicker({
        timeFormat: timeFmtStr,
        controlType: "select",
        oneLine: true,
        onClose: function () {
            $(this).focus();
        },
    });

    // ｱｲｺﾝｸﾘｯｸで選択ｴﾘｱ展開
    var icon = $(selector).closest("div").find(".date_icon");
    $(icon).on('click', function () {
        var input = $(icon).closest("div").find("input[type='text']");
        $(input).focus();
    });
    icon = null;
}



/**
 *  カレンダーの初期化処理
 *  @param {string} ：対象セレクタ
 *  @param {string} ：一覧再表示時、true
 */
function initDateTimePicker(selector, isReset) {
    //年月日の初期化
    var dateFmtStr = "";
    var timeFmtStr = "";
    var minDateStr = "";
    var maxDateStr = "";
    var numberOfMonths = 1;


    var fmt = $(selector).data("format");

    var viewModeVal = 2;    //年→月→日と設定
    var minViewMode = 2;    //"年"まで選択できる

    //日付と時刻で書式をスペースで分割
    var fmts = fmt.split(" ");

    //書式文字列を生成(日付)
    dateFmtStr = fmts[0].toLowerCase();
    if (fmts[0].indexOf('y') > -1) {
        // jQueryのdatepickerを使用する場合、yyyy→yyに変更
        if (fmts[0].indexOf("yyyy") > -1) {
            dateFmtStr = dateFmtStr.replace("yyyy", "yy")
        } else if (fmts[0].indexOf("yy") > -1) {
            if (!isReset) {
                //※画面初期化時
                dateFmtStr = dateFmtStr.replace("yy", "y")
                var count = (dateFmtStr.match(new RegExp("y", "g")) || []).length;
                if (count == 1) {
                    dateFmtStr = dateFmtStr.replace("y", "yy");
                }
            }
        }
    }
    if (fmts[0].indexOf('m') > -1) {
        viewModeVal = 1;    //月→日と設定
        minViewMode = 1;    //"月"まで選択できる
    }
    if (fmts[0].indexOf('d') > -1) {
        viewModeVal = 0;    //(ﾃﾞﾌｫﾙﾄ)日を設定
        minViewMode = 0;    //(ﾃﾞﾌｫﾙﾄ)"日付"まで設定できる
    }
    if (dateFmtStr.length <= 0) {
        //未設定の場合、ﾃﾞﾌｫﾙﾄﾌｫｰﾏｯﾄ
        dateFmtStr = "yy/mm/dd";

        viewModeVal = 0;    //(ﾃﾞﾌｫﾙﾄ)日を設定
        minViewMode = 0;    //(ﾃﾞﾌｫﾙﾄ)"日付"まで設定できる
    }

    //書式文字列を生成(時刻)
    if (fmts.length > 1) {
        timeFmtStr = fmts[1].toLowerCase();
        //if (fmts[1].indexOf('h') > -1) {
        if (timeFmtStr.indexOf('h') > -1) {
            //if (fmts[1].indexOf("hh") > -1) {
            if (timeFmtStr.indexOf("hh") > -1) {
                timeFmtStr = timeFmtStr.replace("hh", "HH")
                //} else if (fmts[1].indexOf("h") > -1) {
            } else if (timeFmtStr.indexOf("h") > -1) {
                timeFmtStr = timeFmtStr.replace("h", "H")
            }
        }
    }

    if (timeFmtStr.length <= 0) {
        //未設定の場合、ﾃﾞﾌｫﾙﾄﾌｫｰﾏｯﾄ
        timeFmtStr = "HH:mm";
    }

    // 共通ポップアップ画面で時刻が「HH:10」と表示される不具合修正
    if (!isReset) {
        //※画面初期化時

        //ﾌｫｰﾏｯﾄを上書き
        setAttrByNativeJs(selector, "data-format", dateFmtStr + " " + timeFmtStr);

        //初期値を設定
        var defVal = $(selector).data("def");
        if (defVal != null && defVal.length > 0) {
            //初期値が設定されている場合、datetimepickerの日時セット

            if (defVal == "SysDate") {
                // 初期値に「SysDate」が設定されている場合、現在日時をセットする

                //書式フォーマット変換
                var now = new Date();

                var year = now.getFullYear();
                var month = now.getMonth() + 1;
                var day = now.getDate();

                var hour = now.getHours();
                var minutes = now.getMinutes();
                var seconds = now.getSeconds();

                var setDateStr = dateFmtStr;
                if (setDateStr.indexOf('yy') > -1) {
                    setDateStr = setDateStr.replace('yy', year);
                } else if (setDateStr.indexOf('y') > -1) {
                    setDateStr = setDateStr.replace('y', year.toString().substring(2, 4));
                }
                if (setDateStr.indexOf("mm") > -1) {
                    setDateStr = setDateStr.replace('mm', ("00" + month).slice(-2));
                } else if (setDateStr.indexOf("m") > -1) {
                    setDateStr = setDateStr.replace('m', month);
                }
                if (setDateStr.indexOf("dd") > -1) {
                    setDateStr = setDateStr.replace('dd', ("00" + day).slice(-2));
                } else if (setDateStr.indexOf("d") > -1) {
                    setDateStr = setDateStr.replace('d', day);
                }

                var setTimeStr = timeFmtStr;
                if (setTimeStr.indexOf("HH") > -1) {
                    setTimeStr = setTimeStr.replace('HH', ("00" + hour).slice(-2));
                } else if (setTimeStr.indexOf("H") > -1) {
                    setTimeStr = setTimeStr.replace('H', hour);
                }
                if (setTimeStr.indexOf("mm") > -1) {
                    setTimeStr = setTimeStr.replace('mm', ("00" + minutes).slice(-2));
                } else if (setTimeStr.indexOf("m") > -1) {
                    setTimeStr = setTimeStr.replace('m', minutes);
                }
                if (setTimeStr.indexOf("ss") > -1) {
                    setTimeStr = setTimeStr.replace('ss', ("00" + seconds).slice(-2));
                } else if (setTimeStr.indexOf("s") > -1) {
                    setTimeStr = setTimeStr.replace('s', seconds);
                }

                $(selector).val(setDateStr + " " + setTimeStr);
            }
        }
        //値がｾｯﾄされている場合は戻す
        var setVal = $(selector).data("value");
        if (setVal != null && setVal.length > 0) {
            $(selector).val(setVal);
        }

        // 日付選択範囲
        minDateStr = $(selector).data("minval");
        maxDateStr = $(selector).data("maxval");

        // 表示する月数
        var maxlength = $(selector).data("maxlength");
        if (!(maxlength == null || maxlength == "")) {
            if (1 <= maxlength && maxlength <= 4) {
                numberOfMonths = $(selector).data("maxlength");
            }
        }

    }
    else {
        // 共通ポップアップ画面で時刻が「HH:10」と表示される不具合修正
        //dateFmtStr = $(selector).data("format");

    }

    // 日時を表示する場合
    $(selector).datetimepicker({
        dateFormat: dateFmtStr,
        clearBtn: false,
        viewMode: viewModeVal,
        minViewMode: minViewMode,
        todayHighlight: true,
        autoclose: true
        , forceParse: false
        , numberOfMonths: numberOfMonths
        , monthNames: [
            P_ComMsgTranslated[911460001], P_ComMsgTranslated[911470001], P_ComMsgTranslated[911480001], P_ComMsgTranslated[911490001],
            P_ComMsgTranslated[911500001], P_ComMsgTranslated[911510001], P_ComMsgTranslated[911520001], P_ComMsgTranslated[911530001],
            P_ComMsgTranslated[911540001], P_ComMsgTranslated[911460002], P_ComMsgTranslated[911460003], P_ComMsgTranslated[911460004]
        ] //["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"]
        , monthNamesShort: [
            P_ComMsgTranslated[911460001], P_ComMsgTranslated[911470001], P_ComMsgTranslated[911480001], P_ComMsgTranslated[911490001],
            P_ComMsgTranslated[911500001], P_ComMsgTranslated[911510001], P_ComMsgTranslated[911520001], P_ComMsgTranslated[911530001],
            P_ComMsgTranslated[911540001], P_ComMsgTranslated[911460002], P_ComMsgTranslated[911460003], P_ComMsgTranslated[911460004]
        ] //['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月']
        , yearSuffix: P_ComMsgTranslated[911240001] //年
        , showMonthAfterYear: true
        , dayNames: [
            P_ComMsgTranslated[911270005], P_ComMsgTranslated[911090008], P_ComMsgTranslated[911060005], P_ComMsgTranslated[911130004],
            P_ComMsgTranslated[911350002], P_ComMsgTranslated[911070009], P_ComMsgTranslated[911200008],
        ] //['日', '月', '火', '水', '木', '金', '土']
        , dayNamesMin: [
            P_ComMsgTranslated[911270005], P_ComMsgTranslated[911090008], P_ComMsgTranslated[911060005], P_ComMsgTranslated[911130004],
            P_ComMsgTranslated[911350002], P_ComMsgTranslated[911070009], P_ComMsgTranslated[911200008],
        ] //['日', '月', '火', '水', '木', '金', '土']
        , showButtonPanel: true
        , showOtherMonths: true
        , selectOtherMonths: true
        , timeFormat: timeFmtStr
        , controlType: "select"
        , oneLine: true
        , minDate: minDateStr
        , maxDate: maxDateStr
        , changeYear: true
        , changeMonth: true
        , onClose: function () {
            $(this).focus();
        }
    });

    // ｱｲｺﾝｸﾘｯｸで選択ｴﾘｱ展開
    var icon = $(selector).closest("div").find(".date_icon");
    $(icon).on('click', function () {
        var input = $(icon).closest("div").find("input[type='text']");
        $(input).focus();
    });
    icon = null;
}

/**
 * 年月選択の初期化処理
 *  @param {string} ：対象セレクタ
 *  @param {string} ：一覧再表示時、true
 */
function initYearMonthPicker(selector, isReset) {
    //年月日の初期化
    var dateFmtStr = "";
    var minDateStr = "";
    var maxDateStr = "";
    var numberOfMonths = 1;
    if (!isReset) {
        //※画面初期化時

        var fmt = $(selector).data("format") + '';

        //書式文字列を生成
        dateFmtStr = fmt;
        if (fmt.indexOf('y') > -1) {
            // jQueryのdatepickerを使用する場合、yyyy→yyに変更
            if (fmt.indexOf("yyyy") > -1) {
                dateFmtStr = dateFmtStr.replace("yyyy", "yy")
            } else if (fmt.indexOf("yy") > -1) {
                dateFmtStr = dateFmtStr.replace("yy", "y")
                // APより移行　二重に置き換えが行われた場合は元に戻す
                var count = (dateFmtStr.match(new RegExp("y", "g")) || []).length;
                if (count == 1) {
                    dateFmtStr = dateFmtStr.replace("y", "yy");
                }
            }
        }
        var viewModeVal, minViewMode;
        if (fmt.indexOf('m') > -1) {
            viewModeVal = 1;    //月→日と設定
            minViewMode = 1;    //"月"まで選択できる
        }
        if (dateFmtStr.length <= 0) {
            //未設定の場合、ﾃﾞﾌｫﾙﾄﾌｫｰﾏｯﾄ
            dateFmtStr = "yy/mm";

            viewModeVal = 1;    //月→日と設定
            minViewMode = 1;    //"月"まで選択できる
        }
        //ﾌｫｰﾏｯﾄを上書き
        setAttrByNativeJs(selector, "data-format", dateFmtStr);

        //初期値を設定
        var defVal = $(selector).data("def");
        if (defVal != null && defVal.length > 0) {
            //初期値が設定されている場合、datepickerの日付ｾｯﾄ

            if (defVal == "SysDate") {
                // 初期値に「SysDate」が設定されている場合、現在日時をセットする

                //書式フォーマット変換
                var now = new Date();

                var y = now.getFullYear();

                var m = ("00" + (now.getMonth() + 1)).slice(-2);

                var setStr = "";
                if (dateFmtStr.indexOf('yy') > -1) {
                    setStr = dateFmtStr.replace('yy', y);
                } else if (dateFmtStr.indexOf('y') > -1) {
                    setStr = dateFmtStr.replace('y', y.toString().substring(2, 4));
                }
                setStr = setStr.replace('mm', m);

                $(selector).val(setStr);
            }
        }
        //値がｾｯﾄされている場合は戻す
        var setVal = $(selector).data("value");
        if (setVal != null && setVal.length > 0) {
            $(selector).val(setVal);
        }

        // 日付選択範囲
        minDateStr = $(selector).data("minval");
        maxDateStr = $(selector).data("maxval");

        // 表示する月数
        var maxlength = $(selector).data("maxlength");
        if (!(maxlength == null || maxlength == "")) {
            if (1 <= maxlength && maxlength <= 4) {
                numberOfMonths = $(selector).data("maxlength");
            }
        }
    }
    else {
        dateFmtStr = $(selector).data("format");
    }

    $(selector).ympicker({
        dateFormat: dateFmtStr
        , clearBtn: false
        , monthNames: [
            P_ComMsgTranslated[911460001], P_ComMsgTranslated[911470001], P_ComMsgTranslated[911480001], P_ComMsgTranslated[911490001],
            P_ComMsgTranslated[911500001], P_ComMsgTranslated[911510001], P_ComMsgTranslated[911520001], P_ComMsgTranslated[911530001],
            P_ComMsgTranslated[911540001], P_ComMsgTranslated[911460002], P_ComMsgTranslated[911460003], P_ComMsgTranslated[911460004]
        ] //["1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月"]
        , monthNamesShort: [
            P_ComMsgTranslated[911460001], P_ComMsgTranslated[911470001], P_ComMsgTranslated[911480001], P_ComMsgTranslated[911490001],
            P_ComMsgTranslated[911500001], P_ComMsgTranslated[911510001], P_ComMsgTranslated[911520001], P_ComMsgTranslated[911530001],
            P_ComMsgTranslated[911540001], P_ComMsgTranslated[911460002], P_ComMsgTranslated[911460003], P_ComMsgTranslated[911460004]
        ] //['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月']
        , closeText: P_ComMsgTranslated[911200009] //閉じる
        , prevText: P_ComMsgTranslated[911310001] //&#x3C;前
        , nextText: P_ComMsgTranslated[911180003] //次&#x3E;
        , autoclose: true
        , numberOfMonths: numberOfMonths
        , minDate: minDateStr
        , maxDate: maxDateStr
        , onClose: function () {
            $(this).focus();
        }
    });

    // ｱｲｺﾝｸﾘｯｸで選択ｴﾘｱ展開
    var icon = $(selector).closest("div").find(".date_icon");
    $(icon).on('click', function () {
        var input = $(icon).closest("div").find("input[type='text']");
        $(input).focus();
    });
    icon = null;
}

/**
 * 年入力の初期化処理呼び出し処理
 * */
function initYearTexts() {
    var targets = $(YearText.selector);
    $(targets).each(function (index, element) {
        initYearText(element, false);
    });
}

/**
 * 年入力の初期化処理
 *  @param {string} ：対象セレクタ
 *  @param {string} ：一覧再表示時、true
 */
function initYearText(selector, isReset) {
    //初期値を設定
    var defVal = $(selector).val();
    if (defVal != null && defVal.length > 0) {
        //初期値が設定されている場合

        if (defVal.indexOf(YearText.defVal) > -1) {
            // 初期値に「SysYear」が設定されている場合、現在年をセットする

            //書式フォーマット変換
            var now = new Date();
            var year = now.getFullYear();

            // +-が設定されている場合、計算
            var pIndex = defVal.indexOf('+');
            var mIndex = defVal.indexOf('-');
            if (pIndex > -1 || mIndex > -1) {
                // 計算
                var isPlus = pIndex > mIndex;
                var index = isPlus ? pIndex : mIndex;
                var calcValue = (defVal.substring(index + 1)) - 0;
                if (isPlus) {
                    year += calcValue;
                } else {
                    year -= calcValue;
                }
            }

            $(selector).val(year);
        }
    }
}

/**
 * コンボボックスデータの取得
 * @param {string} appPath  ：アプリケーションルートパス
 * @param {string} param    ：コンボデータ取得用パラメータ(=セッションストレージ識別ID)
 * @return {Array.<object>} 取得結果(0:成功/1:取得中/-1:失敗)、コンボボックスデータ配列
 */
function getComboBoxDataFromSessionStorage(appPath, param) {
    var savedData = getSavedDataFromSessionStorage(sessionStorageCode.CboMasterData, param);
    if (savedData != null && savedData.length > 0) {
        // セッションストレージに存在する場合
        return [false, savedData];
    }

    var params = param.split(',');
    if (!params[0] || params[0] == '-') {
        // SQLIDが空の場合、取得対象外
        return [false, []];
    }

    if (P_GettingComboBoxDataList.indexOf(param) >= 0) {
        // データ取得中の場合
        return [false, null];
    }

    // データ取得実行が必要
    return [true, null];
}

/**
 * コンボボックスデータの取得
 * @param {string} param    ：コンボデータ取得用パラメータ
 * @return {Array.<object>} 取得結果(0:成功/1:取得中/-1:失敗)、コンボボックスデータ配列
 */
function getComboBoxLocalData(param) {
    var savedData = P_ComboBoxJsonList[param];
    if (savedData != null && savedData.length > 0) {
        // グローバル変数に存在する場合
        return [false, savedData];
    }

    //SQLIDとパラメータに分割
    var params = param.split('_');
    if (!params[0] || params[0] == '-') {
        // SQLIDが空の場合、取得対象外
        return [false, []];
    }

    if (P_GettingComboBoxDataList.indexOf(param) >= 0) {
        // データ取得中の場合
        return [false, null];
    }

    var key = param;
    //while (key.endsWith(',')) {
    while (key.endsWith(',') || key.endsWith('_')) {
        key = key.substring(0, key.length - 1);
    }
    var memoryData = P_ComboBoxMemoryItems[key];
    if (memoryData != null && memoryData.length > 0 || key in P_ComboBoxMemoryItems) {
        // 共有メモリに存在する場合(keyが存在してデータが0件でも再取得は行わない)
        return [false, memoryData];
    }

    // データ取得実行が必要
    return [true, null];
}

/**
 * 構成マスタデータの工場IDによる絞り込み
 * @param {Array.<object>} data             ：構成マスタデータ配列
 * @param {Array.<number>} factoryIdList    ：工場ID配列
 */
function filterStructureItemByFactory(data, factoryIdList) {
    if (data == null) {
        return data;
    }

    // 工場IDを含まない場合に絞込を行わない
    if (data.length == data.filter(x => { return (x.FactoryId == "" && x.TranslationFactoryId == "") || (x.FactoryId == null && x.TranslationFactoryId == null) }).length) {
        return data;
    }

    var resultList = [];
    var factoryId = "0";
    var orderFactoryId;
    if (factoryIdList != null && factoryIdList.length > 0) {
        factoryId = factoryIdList[0].toString();
        // 工場IDで絞り込み
        data = $.grep(data, function (obj, index) {
            return (
                (obj.FactoryId.toString() == factoryId || obj.FactoryId.toString() == "0") &&
                (obj.TranslationFactoryId.toString() == factoryId || obj.TranslationFactoryId.toString() == "0"));
        });
        orderFactoryId = factoryId;
    } else {
        orderFactoryId = getSelectedFactoryIdList(null, true, true);
    }
    // 表示順用工場IDで絞込を行う
    data = filterStructureItemByFactoryForOrder(data, orderFactoryId);

    // 構成IDでグループ化
    var groupedList = getGroupedObjectList(data, 'VALUE1');
    $.each(groupedList,
        function (id, list) {
            if (list.length == 1) {
                resultList.push(list[0])
            } else {
                // 翻訳工場IDで絞り込み
                var item = $.grep(list, function (obj, index) {
                    return (obj.TranslationFactoryId.toString() == factoryId);
                });

                if (item.length == 0) {
                    resultList.push(list[0]);
                }
                else {
                    resultList.push(item[0]);
                }          
            }
        }
    );
    return resultList;
}

/**
 * 構成マスタデータの工場IDによる絞り込み(表示順取得用工場ID)
 * 予備品で工場IDで絞り込まない場合があるので処理を分離
 * @param {Array.<object>} data             ：構成マスタデータ配列
 * @param {Array.<number>} factoryIdList    ：工場ID配列
 */
function filterStructureItemByFactoryForOrder(data, factoryId) {
    var rtnData = data;
    if (data.length != data.filter(x => { return x.OrderFactoryId == "" }).length) {
        var getFactoryData = function (pData, pFactoryId) {
            return $.grep(pData, function (obj, index) {
                return (
                    (obj.OrderFactoryId.toString() == pFactoryId));
            });
        }
        var factoryData = getFactoryData(data, factoryId);
        if (factoryData == null || factoryData.length == 0) {
            factoryData = getFactoryData(data, "0");
        }
        rtnData = factoryData;
    }
    return rtnData;
}

function removeComboBoxDataKey(key) {
    const idx = P_GettingComboBoxDataList.indexOf(key);
    if (idx >= 0) {
        P_GettingComboBoxDataList = P_GettingComboBoxDataList.filter((_, index) => index !== idx);
    }
}

/**
 *  コンボボックスの初期化処理
 *  @param {string} appPath：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string} selector：対象セレクタ
 *  @param {string} sqlId：SQL ID
 *  @param {string} param：SQLパラメータ
 *  @param {number} option：1:先頭に「全て」の項目を追加する / 0:追加しない
 *  @param {number} nullCheck：1:必須 / 0:任意
 *  @param {number} changeColNo ：-1:初期化時 / > 0:変更列番号
 *  @param {string} [changeVal=null] 
 *  @param {Array} [factoryIdList=[]] 
 *  @param {boolean} [resetCommonData=false] 
 */
function initComboBox(appPath, selector, sqlId, param, option, nullCheck, changeColNo, changeVal = null, factoryIdList = [], resetCommonData = false) {
    // 変更前値はdata-value属性から取得する
    // 処理開始時のコンボの変更前値
    const preComboValue = $(selector).val();

    if (changeColNo > 0) {
        //※ｺﾝﾄﾛｰﾙchangedｲﾍﾞﾝﾄ時

        // 2022.04.14 連動コンボ処理中ステータス設定処理無効化
        ////連動ｺﾝﾎﾞ処理中：on
        //setComboProcessStatus(selector, true);

        //変更値でﾊﾟﾗﾒｰﾀを書き換え
        //※文字列指定でないﾊﾟﾗﾒｰﾀの場合、ﾊﾟﾗﾒｰﾀ値未設定の場合は「null」を設定
        if (changeVal == null || changeVal.length <= 0) {
            //※ﾊﾟﾗﾒｰﾀ値未設定の場合

            //文字列指定でないﾊﾟﾗﾒｰﾀか？
            var pos = param.indexOf('@' + changeColNo);
            if (pos == 0) {
                changeVal = "null";
            }
            else if (pos > 0) {
                //「@」直近文字列が「'」でないか？
                if (param.substr(pos - 1, 1) != "'") {
                    changeVal = "null";
                }

            }
        }
        if (changeColNo != colNoForFactory) {
            param = param.replace('@' + changeColNo, changeVal);    //「@3」⇒「ｾﾙの値」で置き換え
        } else {
            // 工場コントロールの場合、「factoryIdList」で工場IDを渡す
            param = param.replace(',@' + changeColNo, '');  // 「,@9999」をパラメータ文字列から削除
            if (changeVal != null && (!isString(changeVal) || (changeVal.length > 0 && changeVal != 'null'))) {
                factoryIdList.push(changeVal);
            }
        }
    }

    //ｺﾝﾎﾞ一覧の初期化
    $(selector).children().remove();

    //新規行判定
    var isRowStatusNew = $(selector).hasClass("rowStatusNew");

    //if (nullCheck != "1" || isRowStatusNew) {
    //    // ※任意項目の場合
    //    // ※新規行の場合

    //    //先頭に空白項目を追加
    //    $('<option>').val("").html("").appendTo($(selector));
    //}

    if (option == "1") {
        //　先頭に「すべて」の項目を追加
        $('<option class="ctrloption">').val("").html(P_ComMsgTranslated[911130001]).appendTo($(selector)); // すべて
    }
    else if (option == "2") {
        //先頭に空白項目を追加
        $('<option class="ctrloptionblank">').val("").html("").appendTo($(selector));
    }

    //SQLパラメータの成形(例：'C01','@3')
    var paramStr = param + '';
    var params = (param + '').split(',');   //ｶﾝﾏで分解

    //対象行：trを取得する
    var { tr, isHorizontalTbl } = getDataTr(selector);

    //列指定ﾊﾟﾗﾒｰﾀ（「@」で始まる引数）の値の置き換えと変更時ｲﾍﾞﾝﾄ処理を付与
    var isDynamic = false;  //動的ｺﾝﾎﾞか？
    $.each(params, function () {
        var colNo = -1;
        var paramVal = "";

        if (this.indexOf("@") >= 0) {
            //列指定ﾊﾟﾗﾒｰﾀ（「@」で始まる引数）の場合

            isDynamic = true;

            //列番号を取得
            colNo = parseInt(this.replace("'", "").replace("@", ""), 10);  //先頭の「@」を除去してcolNoとする

            //対象列の値を取得
            //入力項目の場合、変更時ｲﾍﾞﾝﾄ処理を付与
            // ⇒ｺﾝﾎﾞﾎﾞｯｸｽ一覧を再作成する

            //対象ｾﾙ
            var td = getDataTd(tr, colNo);
            var fromLocationTree = false;
            if (!td || td.length == 0) {
                // 対象セルが存在しない場合
                if (colNo == colNoForFactory) {
                    // 工場コントロール指定の場合、画面上から取得
                    if (isHorizontalTbl) {
                        // 横型一覧の場合、同一行から取得する
                        td = getFactoryTdElement(selector, tr);
                    }
                    if (!td || td.length == 0) {
                        // 同一画面上から取得する
                        td = getFactoryTdElement(selector);
                    }
                    if (!td || td.length == 0) {
                        if (getCurrentModalNo(selector) > 0) {
                            // モーダル画面の場合、呼び元の詳細エリアも探す
                            td = getFactoryTdElement($("#detail_divid_" + getFormNo(selector)));
                        }
                    }

                    if (!td || td.length == 0) {
                        // 同一画面上に工場コントロールがない場合、左側の場所階層メニューから取得
                        fromLocationTree = true;
                    }
                } else {
                    return true;    // continue;
                }
            }

            if (fromLocationTree) {
                // 場所階層ツリー(chageイベントの割り当ては行わない)
                var paramList = getSelectedFactoryIdList(null, false, true);
                if (paramList.length > 0) {
                    // 配列をJSON文字列に変換してセット
                    factoryIdList = factoryIdList.concat(paramList);
                }
                if (changeColNo < 0) {
                    // 場所階層の工場フィルター使用フラグをセット
                    setAttrByNativeJs(selector, 'data-usefactoryfilter', true);
                }
            } else if ($(td).data('type') == 'treeLabel') {
                //ツリー選択ラベル
                if (changeColNo < 0) {
                    paramVal = $(td).data('structureid');

                    //ｲﾍﾞﾝﾄ付与
                    $(td).get(0).addEventListener('change', function () {
                        // 工場コントロールのchangeイベント
                        var factoryId = $(this).data('structureid');
                        var cmbFactoryId = $(selector).data('factoryid');
                        if (cmbFactoryId == null || factoryId != cmbFactoryId) {
                            // 工場IDが変更されたらコンボボックスの初期化処理を実行する
                            initComboBox(appPath, selector, sqlId, param, option, nullCheck, colNo, $(this).data('structureid'));
                        }
                        // コンボボックスに工場IDをセットする
                        setAttrByNativeJs(selector, 'data-factoryid', factoryId);
                    }, false);
                }
                else {
                    paramVal = $(td).data('structureid');
                }

            } else {
                //複数選択ﾘｽﾄ（※inputﾀｸﾞと間違わないように先に実施）
                var msul = $(td).find("ul.multiSelect");
                if (msul.length > 0) {
                    if (changeColNo < 0) {
                        paramVal = $(msul).data("value");

                        //ｲﾍﾞﾝﾄ付与(ﾘｽﾄ内すべてのﾁｪｯｸﾎﾞｯｸｽに設定)
                        //$($(msul).find(":checkbox")).off('focusout');
                        $($(msul).find(":checkbox")).on('focusout', function () {
                            var valW = "";
                            var checkes = $(this).parent().parent().find("> li:not(.hide) :checkbox:checked");
                            if (checkes != null && checkes.length > 0) {
                                var vals = [];
                                $.each(checkes, function () {
                                    vals.push($(this).val());
                                });
                                valW = vals.join(',');
                            }
                            initComboBox(appPath, selector, sqlId, param, option, nullCheck, colNo, valW);
                        });
                    }
                    else {
                        var checkes = $(msul).find("> li:not(.hide) :checkbox:checked");
                        if (checkes != null && checkes.length > 0) {
                            var vals = [];
                            $.each(checkes, function () {
                                vals.push($(this).val());
                            });
                            paramVal = vals.join(',');
                        }
                    }
                }
                else {
                    //ｺﾝﾎﾞﾎﾞｯｸｽ
                    var select = $(td).find("select");
                    if (select.length > 0) {
                        if (changeColNo < 0) {
                            paramVal = $(select).data("value");

                            //ｲﾍﾞﾝﾄ付与
                            //$(select).off('change');
                            $(select).on('change', function () {
                                initComboBox(appPath, selector, sqlId, param, option, nullCheck, colNo, $(this).val());
                            });
                        }
                        else {
                            paramVal = $(select).val();
                        }
                    }
                    else {
                        //ﾁｪｯｸﾎﾞｯｸｽ
                        var checkbox = $(td).find(":checkbox");
                        if (checkbox.length > 0) {
                            paramVal = $(checkbox).prop("checked") ? 1 : 0;

                            if (changeColNo < 0) {
                                //ｲﾍﾞﾝﾄ付与
                                //$(checkbox).off('change');
                                $(checkbox).on('change', function () {
                                    var valW = $(this).prop("checked") ? 1 : 0;
                                    initComboBox(appPath, selector, sqlId, param, option, nullCheck, colNo, valW);
                                });
                            }

                        } else {

                            //ﾃｷｽﾄ、数値、ﾃｷｽﾄｴﾘｱ、ｺｰﾄﾞ＋翻訳、日付、時刻、日時
                            var input = $(td).find("input[type='text'], input[type='hidden'], textarea");
                            if (input.length > 0) {
                                paramVal = $(input).val();

                                if (changeColNo < 0) {
                                    //ｲﾍﾞﾝﾄ付与
                                    //$(input).off('change');
                                    $(input).on('change', function () {
                                        initComboBox(appPath, selector, sqlId, param, option, nullCheck, colNo, $(this).val());
                                    });
                                }

                            }
                            else {

                                //日付(ブラウザ標準)、時刻(ブラウザ標準)、日時(ブラウザ標準)
                                var dateTime = $(td).find("input[type='date'], input[type='time'], input[type='datetime-local']");
                                if (dateTime.length > 0) {
                                    paramVal = $(dateTime).val().replace(/-/g, "/").replace("T", " ");
                                    if (dateTime.length > 1) {
                                        paramVal = paramVal + '|' + $(dateTime[1]).val().replace(/-/g, "/").replace("T", " ");
                                    }

                                    if (changeColNo < 0) {
                                        //ｲﾍﾞﾝﾄ付与
                                        //$(dateTime).off('change');
                                        $(dateTime).on('change', function () {
                                            var td = $(this).closest("td");
                                            var inputs = $(td).find("input[type='date'], input[type='time'], input[type='datetime-local']");
                                            var valW = $(inputs).val().replace(/-/g, "/").replace("T", " ");
                                            if (inputs.length > 1) {
                                                valW = valW + '|' + $(inputs[1]).val().replace(/-/g, "/").replace("T", " ");
                                            }
                                            initComboBox(appPath, selector, sqlId, param, option, nullCheck, colNo, valW);
                                        });
                                    }

                                }
                                else {
                                    //ﾗﾍﾞﾙ
                                    paramVal = $(td).text();
                                }
                                dateTime = null;
                            }
                            input = null;
                        }
                        checkbox = null;

                    }
                    select = null;
                }
                msul = null;
            }
            td = null;

            //SQLパラメータに現時点の値を設定
            //※文字列指定でないﾊﾟﾗﾒｰﾀの場合、ﾊﾟﾗﾒｰﾀ値未設定の場合は「null」を設定
            if (paramVal == null || paramVal.length <= 0) {
                //※ﾊﾟﾗﾒｰﾀ値未設定の場合

                //文字列指定でないﾊﾟﾗﾒｰﾀか？
                var pos = paramStr.indexOf('@' + colNo);
                if (pos == 0) {
                    paramVal = "null";
                }
                else if (pos > 0) {
                    //「@」直近文字列が「'」でないか？
                    if (paramStr.substr(pos - 1, 1) != "'") {
                        paramVal = "null";
                    }
                }
            }
            if (colNo != colNoForFactory) {
                paramStr = paramStr.replace('@' + colNo, paramVal);    //「@3」⇒「ｾﾙの値」で置き換え
            } else {
                // 工場コントロールの場合、「factoryIdList」で工場IDを渡す
                paramStr = paramStr.replace(',@' + colNo, '');  // 「,@9999」をパラメータ文字列から削除
                if (!fromLocationTree && paramVal != "null") {
                    factoryIdList.push(paramVal);
                }
            }
        }
    });
    //if (changeColNo < 0 && isDynamic) {
    if (changeColNo < 0) {
        if (isDynamic) {
            //一覧再表示後の連動ｺﾝﾎﾞの選択ﾘｽﾄ再生成時用の設定を付与しておく
            $(selector).addClass("dynamic");
        }
        setAttrByNativeJs(selector, "data-sqlid", sqlId);
        setAttrByNativeJs(selector, "data-param", param);
        setAttrByNativeJs(selector, "data-option", option);
        setAttrByNativeJs(selector, "data-nullcheck", nullCheck);
    }

    // コンボボックスデータの取得
    //const paramKey = sqlId + "," + paramStr;
    const paramKey = sqlId + "_" + paramStr;
    // (2022/11/01) 構成マスタデータをセッションストレージには保存しない
    //var [requiredGetData, data] = getComboBoxDataFromSessionStorage(appPath, paramKey);
    var [requiredGetData, data] = getComboBoxLocalData(paramKey);

    // データ設定処理
    const eventFunc = function () {
        if (data == null) { data = []; }

        if (data.length > 0) {
            // 工場IDで絞り込み
            data = filterStructureItemByFactory(data, factoryIdList);
        }

        //現在設定されている値を退避
        var setVal = $(selector).data("value");

        //ｺﾝﾎﾞ一覧の初期化
        $(selector).children().remove();

        //新規行判定
        var isRowStatusNew = $(selector).hasClass("rowStatusNew");

        //if (nullCheck != "1" || isRowStatusNew) {
        //    // ※任意項目の場合
        //    // ※新規行の場合

        //    //先頭に空白項目を追加
        //    $('<option>').val("").html("").appendTo($(selector));
        //}

        if (option == "1") {
            //　先頭に「すべて」の項目を追加
            $('<option class="ctrloption">').val("").html(P_ComMsgTranslated[911130001]).appendTo($(selector)); // すべて
        }
        else if (option == "2") {
            //先頭に空白項目を追加
            $('<option class="ctrloptionblank">').val("").html("").appendTo($(selector));
        }

        // 元の値はdata-value属性から取得する
        const dataValue = $(selector).data('value') + "";
        let dataValueEx;
        if (changeColNo < 0) {
            if ($(selector).hasClass('DispCount')) {
                // 読込件数コンボの場合、読込件数上限のデフォルト値を取得
                dataValueEx = getSelectCntMaxDefault(null, selector) + "";
            }
        }
        $.each(data, function () {
            // optionタグ追加
            const optionVal = this.VALUE1;
            var option = $('<option>').val(optionVal).html(this.VALUE2);
            // 付属情報を取得可能にする
            var keys = Object.keys(this);
            var dt = this;
            $.each(keys, function (i, key) {
                if (key.indexOf("EXPARAM") !== 0) {
                    return true;    // continue
                }
                if (dt[key] != undefined && dt[key].length > 0) {
                    setAttrByNativeJs(option, key, dt[key]);
                }
            });
            keys = null;

            //if (changeColNo < 0 && $(selector).data("value") == $(option).val()) {
            //    // コンボの設定値と一致する項目を選択状態にセット
            //    setAttrByNativeJs(option, "selected", true);
            //    //if ($(selector).closest("td").find("span.labeling").text() == "") {
            //    //    // 未設定の場合、VALUE2を設定
            //        $(selector).closest("td,.tabulator-cell").find("span.labeling").text(dt.VALUE2);
            ////    }
            //}

            if (changeColNo < 0) {
                // コンボの設定値と一致する項目を選択状態にセット
                let selected = false;
                if (dataValue != null && dataValue.length > 0) {
                    // data-value属性の値を優先する
                    selected = optionVal == dataValue;
                } else if (dataValueEx != null && dataValueEx.length > 0) {
                    // dataValueExが空でない場合、EXPARAM1の値と比較する
                    selected = this.EXPARAM1 == dataValueEx;
                } else {
                    selected = (preComboValue != null && preComboValue.length > 0) && optionVal == preComboValue;
                }
                if (selected) {
                    setAttrByNativeJs(option, "selected", true);
                    $(selector).closest("td,.tabulator-cell").find("span.labeling").text(dt.VALUE2);
                }
            }

            var element = option;
            if (this.DeleteFlg && this.DeleteFlg == 'True') {
                // 削除フラグがTrueの場合、spanタグで囲んで非表示にする
                element = $('<span>').append(option);
            }

            $(element).appendTo($(selector));
            dt = null;
            option = null;
        });
        if (!isNecessaryComboSelectTop(selector) && $(selector).find("option[selected='true']").length == 0) {
            //先頭に「すべて」と空白が無いコンボの場合、先頭が選択されてしまう場合があるためクリアする
            //※ただし、クリアされたくないコンボ(スタイル指定)は除く、内容は「isNecessaryComboSelectTop」参照
            $(selector).val("");
        }

        //表示のみのｴﾘｱに翻訳表示
        if (option == "1") {
            var tempData = {
                VALUE1: "",
                VALUE2: P_ComMsgTranslated[911130001],  // すべて
            };
            data.push(tempData);
        }
        dispHonyakuSelectCol(data, selector, appPath, sqlId, param); // 表示のみの連動データの場合データを取得出来ないため内部で再取得する @200331M

        ////tabulator連動コンボ列　列フィルターの選択肢生成
        ////一覧のID
        //var tblId = $(selector).closest(".tabulator").attr('id');
        //var tbl = P_listData['#' + tblId];
        //if (tbl) {
        //    //列名取得（VAL3等）
        //    var field = $(selector).closest(".tabulator-cell").attr('tabulator-field');
        //    //一覧のレイアウト定義
        //    var layout = tbl.getColumnLayout();
        //    //対象の列を取得
        //    var results = $.grep(layout, function (obj, index) {
        //        return (obj.field === field);
        //    });
        //    $.each(results, function (idx, result) {
        //        //設定済みの列フィルター選択肢を取得
        //        if (result.headerFilterParams) {
        //            var headerOptions = result.headerFilterParams.values;
        //            $.each(data, function (idx, value) {
        //                if (!(value.VALUE1 in headerOptions)) {
        //                    //含まれていなければ追加
        //                    headerOptions[value.VALUE1] = value.VALUE2;
        //                }
        //            });
        //            result['headerFilterParams'] = { values: headerOptions };
        //        }
        //    });
        //}

        if ($(selector).hasClass("dynamic")) {
            //※ｺﾝﾄﾛｰﾙchangedｲﾍﾞﾝﾄ時

            // 2022.04.14 連動コンボ処理中ステータス設定処理無効化
            ////連動ｺﾝﾎﾞ処理中：off
            //setComboProcessStatus(selector, false);

            if ($(selector).data("create") == "0" && changeColNo > 0) {
                //※ｺﾝﾎﾞ生成済み後の場合のみ、該当項目に連動するｺﾝﾎﾞの再生成を促す

                //ｺﾝﾎﾞ未生成
                setAttrByNativeJs(selector, "data-create", "1");

                //ｺﾝﾎﾞﾎﾞｯｸｽの変更ｲﾍﾞﾝﾄを発生させる
                $(selector).change();
            }
        }

        //ｺﾝﾎﾞ生成済みに設定
        setAttrByNativeJs(selector, "data-create", "0");
        if ($(selector).hasClass("dynamic")) {
            if (changeColNo < 0) {
                // 初期化時のみ
                var selFlg = false;

                // 連動コンボの場合、値が設定されていた場合は選択状態にする
                $(selector).children().each(function () {
                    // 既に選択されている要素がある場合
                    if ($(this).attr("selected") == "selected") {
                        // フラグを立てる
                        selFlg = true;

                        // ループを抜ける
                        return false;
                    }
                });

                // 選択要素が無い場合はチェックを行う
                if (!selFlg) {
                    if (setVal != "") {
                        // 退避させた値と一致するアイテムを選択状態に設定
                        $(selector).children().each(function () {
                            if (setVal == $(this).val()) {
                                setAttrByNativeJs(this, "selected", true);
                            }
                        });
                    }
                }
            }

            // 工場ID変更による再生成時に元の値を選択
            if (changeColNo == colNoForFactory) {
                var comboValues = getComboValueList(selector);
                // 元の値はdata-value属性から取得する
                const dataValue = $(selector).data('value') + "";
                if ((preComboValue != null && preComboValue.length > 0) && comboValues.indexOf(preComboValue) > -1) {
                    setDataForSelectBox($(selector).closest('td'), selector, preComboValue);
                } else if (comboValues.indexOf(dataValue) > -1) {
                    setDataForSelectBox($(selector).closest('td'), selector, dataValue);
                }
            }
        }

        // TMQ_NEW 変更時拡張イベントを設定
        //$(selector).off('change');
        $(selector).on('change', function () {
            var formNo = getFormNo();
            var ctrlId = $(this).closest("div.ctrlId").data("ctrlid");
            var name = $(this).closest('td').data("name");
            var valNo = name ? name.replace("VAL", "") : '';
            var selected = $.grep(data,
                function (elem, index) {
                    if (elem == null || elem == undefined) {
                        return false;
                    }
                    //return (elem.VALUE1 == $(selector).val());
                    var val = $(selector).val();
                    if (val == null || val.length == 0) {
                        val = $(selector).data('value');
                    }
                    return (elem.VALUE1 == val);
                }
            );
            // 【オーバーライド用関数】コンボボックス変更時イベント
            setComboOtherValues(appPath, selector, data, selected[0], formNo, ctrlId, valNo);
        });

    };

    if (data != null) {
        // データ取得成功
        // データ設定処理実行
        eventFunc();

    } else if (!requiredGetData) {
        // データ取得中
        // データ取得中の場合、データ取得中リストから消えるまで待機
        var timer = setInterval(function () {
            if (P_GettingComboBoxDataList.indexOf(paramKey) < 0) {
                // (2022/11/01) 構成マスタデータをセッションストレージには保存しない
                //// データ取得中リストから消えた場合、セッションストレージから取得
                //data = getSavedDataFromSessionStorage(sessionStorageCode.CboMasterData, paramKey);
                // データ取得中リストから消えた場合、グローバル変数から取得
                data = P_ComboBoxJsonList[paramKey];
                // 待機を解除する
                clearInterval(timer);
                // データ設定処理実行
                eventFunc();
            }
        }, 100);

    } else {
        // データ取得が必要
        // コンボデータ取得中リストに追加
        P_GettingComboBoxDataList.push(paramKey);

        // コンボデータ取得処理実行
        var url = encodeURI(appPath + "api/CommonSqlKanriApi/" + sqlId + "?param=" + paramStr + "&reset=" + resetCommonData);// 手動URLエンコード
        $.ajax({
            url: url,
            type: 'GET',
            dataType: 'json',
            contentType: 'application/json',
            traditional: true,
            cache: false
        }).then(
            // 1つめは通信成功時のコールバック
            function (datas) {
                //※正常時
                //結果データ - Dictionary<string,object>
                //結果データからdataを取得
                data = separateDicReturn(datas);
                // (2022/11/01) 構成マスタデータをセッションストレージには保存しない
                //// セッションストレージへ保存
                //setSaveDataToSessionStorage(data, sessionStorageCode.CboMasterData, paramKey)
                P_ComboBoxJsonList[paramKey] = data;
                //★インメモリ化対応 start
                if (existsObjectDataKey(P_ComboBoxMemoryItems, paramKey) && P_ComboBoxMemoryItems[paramKey] == null) {
                    P_ComboBoxMemoryItems[paramKey] = data;
                }
                //★インメモリ化対応 end
                // データ設定処理実行
                eventFunc();
            },
            // 2つめは通信失敗時のコールバック
            function (info) {
                //※異常時
                //info.responseJSON：処理結果ｽﾃｰﾀｽ - CommonProcReturn

                var returnInfo = info.responseJSON;

                //処理結果ｽﾃｰﾀｽを画面状態に反映
                setReturnStatus(appPath, returnInfo, null);

                if ($(selector).hasClass("dynamic")) {
                    //※ｺﾝﾄﾛｰﾙchangedｲﾍﾞﾝﾄ時

                    // 2022.04.14 連動コンボ処理中ステータス設定処理無効化
                    ////連動ｺﾝﾎﾞ処理中：off
                    //setComboProcessStatus(selector, false);

                    //ｺﾝﾎﾞ未生成
                    setAttrByNativeJs(selector, "data-create", "1");

                    //ｺﾝﾎﾞﾎﾞｯｸｽの変更ｲﾍﾞﾝﾄを発生させる
                    $(selector).change();
                }

                //ｺﾝﾎﾞ生成済みに設定
                setAttrByNativeJs(selector, "data-create", "1");
            }
        ).always(
            //通信の完了時に必ず実行される
            function (resultInfo) {
                // コンボデータ取得中リストから削除
                removeComboBoxDataKey(paramKey);
            });
    }

    // keydownイベントを付与
    $(selector).off('keydown');
    $(selector).on('keydown', function (e) {
        // Deleteキーが押された場合のみ処理
        if (e.keyCode == 46) {
            // 必須でない場合のみ処理を行う
            if (!$(this).hasClass("validate_required")) {
                // フォーカス中のコンボのみ選択を解除する
                $('option:selected', this).prop('selected', false)
                // チェンジイベントを発火
                $(this).trigger('change');
            }
        }
    });
}

/**
 * コンボボックスの初期化処理
 * @param {string} appPath      ：アプリケーションパス
 * @param {number} formNo       ：画面NO
 * @param {boolean} isExclude   ：除外フラグ(true:指定画面NOを除外する/false:指定画面NOを初期化する)
 */
function initComboBoxes(appPath, formNo, isExclude) {
    var comboBoxes;
    var parents = $('article[data-formno], table[data-formno]');
    if (!isExclude) {
        parents = $(parents).filter('[data-formno=0]');
    } else {
        parents = $(parents).filter('[data-formno!=0]');
    }
    if (parents && parents.length > 0) {
        var comboBoxes = $(parents).find('select[id^="cbo"]');

        $.each(comboBoxes, function (id, cbo) {
            callInitComboBox(appPath, cbo);
        });
        comboBoxes = null;
    }
    parents = null;
}

/**
 * コンボボックスの初期化処理呼び出し
 * @param {string} appPath  ：アプリケーションパス
 * @param {string} cbo      ：対象セレクタor要素
 */
function callInitComboBox(appPath, cbo) {
    const sqlId = $(cbo).data('sqlid');
    const param = $(cbo).data('param');
    const option = $(cbo).data('option');
    const nullCheck = $(cbo).data('nullcheck');
    if (sqlId == null || sqlId == '' || sqlId == '-') { return; }
    initComboBox(appPath, cbo, sqlId, param, option, nullCheck, -1);
}

/**
 * コンボボックスアイテム取得情報設定(コンボボックス、複数選択チェックボックス)
 * @param {any} selector：要素
 * @param {any} sqlId：SQLID
 * @param {any} param：条件
 * @param {any} option：1:先頭に「全て」の項目を追加する / 0:追加しない
 * @param {any} nullcheck：1:必須 / 0:任意
 */
function setComboSelectBoxInfo(selector, sqlId, param, option, nullcheck) {
    setAttrByNativeJs(selector, "data-sqlid", sqlId);
    setAttrByNativeJs(selector, "data-param", param);
    setAttrByNativeJs(selector, "data-option", option);
    setAttrByNativeJs(selector, "data-nullcheck", nullcheck);
}

/**
 * コンボボックスの初期化処理
 * @param {string} appPath      ：アプリケーションパス
 */
function setComboBoxItem(appPath) {
    // コンボボックスリストを取得
    var comboSelectors = $("select");
    $.each(comboSelectors, function (idx, selector) {
        callInitComboBox(appPath, selector);
    });
    P_ComboDataAcquiredFlg = true;
}

/**
 * 共有メモリ上のコンボボックスデータ更新
 * @param {string} appPath  :ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 * @param {string} conductId :機能ID
 * @param {string} grpId    :構成グループID
 */
function updateComboBoxData(appPath, conductId, grpId) {

    var conditionDataList = [{ GrpId: grpId }];

    // POSTデータを生成
    var postdata = {
       conductId: conductId,                   // メニューの機能ID
       conditionData: conditionDataList, // 条件データ
    };

    // 登録処理実行
    $.ajax({
        url: appPath + 'api/CommonProcApi/' + actionkbn.ComUpdateComboBoxData, // 【共通 - 共有メモリコンボボックスデータ更新】
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(postdata),
        headers: { 'X-XSRF-TOKEN': getRequestVerificationToken() },
        traditional: true,
        cache: false
    }).then(
        // 1つめは通信成功時のコールバック
        function (resultInfo) {
            //正常時
        },
        // 2つめは通信失敗時のコールバック
        function (resultInfo) {
            //異常時
        }
    );
}

/**
 * 複数選択セレクトボックスの初期化処理
 * @param {string} appPath      ：アプリケーションパス
 */
function setMultiSelectBoxItem(appPath) {
    var multiSelectors = $('ul.multiSelect:not("#mltItem")');
    $.each(multiSelectors, function (idx, selector) {
        callInitMultiSelectBox(appPath, selector);
    });
}

/**
 *  オートコンプリートの初期化処理
 *  @param {string} appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string} selector    ：対象セレクタ
 *  @param {string} sqlId       ：SQL ID
 *  @param {string} param       ：SQLパラメータ
 *  @param {HTMLElement} modal  ：オートコンプリート表示要素
 *  @param {string} division    ：オートコンプリート区分
 *  @param {string} option      ：付加情報
 */
function initAutoComp(appPath, selector, sqlId, param, modal, division, option) {

    $(function () {
        $(selector).autocomplete({
            source: function (req, res) {
                // ﾊﾟﾗﾒｰﾀ「@3」⇒「ｾﾙの値」で置き換え
                paramStr = getParamVal(param, selector);
                paramStr = encodeURI(paramStr); // POSTの場合は手動URLエンコード
                $.ajax({
                    url: appPath + "api/CommonSqlKanriApi/" + sqlId + "?param=" + paramStr + "&input=" + req.term,
                    dataType: "json",
                    success: function (datas) {
                        var data = separateDicReturn(datas);
                        var data2 = [];
                        if (data != null && data.length > 0) {
                            if (option != "1") {
                                // 工場IDで翻訳を絞り込み
                                var factoryIdList = getSelectedFactoryIdList(null, true, true);
                                data = filterStructureItemByFactory(data, factoryIdList);
                            } else {
                                // 表示順で絞込、工場は全工場共通
                                data = filterStructureItemByFactoryForOrder(data, "0");
                            }

                            $.each(data, function () {
                                //data2.push(this.VALUE1); //候補にコードのみ表示
                                if (division == autocompDivDef.NameOnly) {
                                    data2.push({ label: this.VALUE2, value: this.VALUE2, id: this.VALUE1 }); //候補に名称を表示
                                } else if (division == autocompDivDef.CodeOnly) {
                                    data2.push({ label: this.VALUE1, value: this.VALUE1 }); //候補にコードを表示
                                } else {
                                    data2.push({ label: this.VALUE1 + "  " + this.VALUE2, value: this.VALUE1 }); //候補にコード＋名称を表示
                                }
                            });

                            // オートコンプリート表示時はエラーツールチップを非表示にする
                            var errTooltip = $("label[for='" + $(selector).attr("id") + "']");
                            if (errTooltip != null && errTooltip.length > 0) {
                                $(errTooltip).fadeOut('fast');
                                $("#main_contents").off('scroll.errTooltip');
                            }
                            errTooltip = null;
                        }

                        res(data2);
                    }
                });
            },
            select: function (event, ui) {
                if (division == autocompDivDef.NameOnly) {
                    // オートコンプリート区分=2(名称のみ表示)の場合、hidden項目にIDを設定
                    $(selector).val(ui.item.label);
                    $(selector).siblings('.autocomp_code').val(ui.item.id);
                    return false;
                }
            },
            autoFocus: true,
            delay: 500,
            minLength: 1,
            appendTo: $(modal)
        });
    });

}

/**
 * オートコンプリート初期化（ベース行定義）
 *  @param {string} selector    ：対象セレクタ
 *  @param {string} sqlId       ：SQL ID
 *  @param {string} param       ：SQLパラメータ
 *  @param {string} division    ：オートコンプリート区分
 *  @param {string} option      ：付加情報の値
 */
function initAutoCompDef(selector, sqlId, param, division, option) {
    if (P_autocompDefines == null) {
        P_autocompDefines = [];     //public変数：一覧定義情報を初期化
    }

    var tmpData = {
        key: selector,
        sqlId: sqlId,
        param: param,
        division: division,
        option: option
    };
    P_autocompDefines.push(tmpData);
}

/**
 * ﾊﾟﾗﾒｰﾀ文字列の列指定部を実際の値に置き換えて返却
 */
function getParamVal(param, selector) {

    var paramStr = param + '';
    var params = (param + '').split(',');   //ｶﾝﾏで分解

    //対象行：trを取得する
    var { tr, isHorizontalTbl } = getDataTr(selector);
    var isDynamic = false;
    $.each(params, function () {
        var colNo = -1;
        var paramVal = "";

        if (this.indexOf("@") >= 0) {
            //列指定ﾊﾟﾗﾒｰﾀ（「@」で始まる引数）の場合

            isDynamic = true;

            //列番号を取得
            colNo = parseInt(this.replace("'", "").replace("@", ""), 10);  //先頭の「@」を除去してcolNoとする

            //対象列の値を取得

            //対象ｾﾙ
            var td = getDataTd(tr, colNo);

            //値の取得
            paramVal = getCellVal(td);

            //SQLパラメータに現時点の値を設定
            //※文字列指定でないﾊﾟﾗﾒｰﾀの場合、ﾊﾟﾗﾒｰﾀ値未設定の場合は「null」を設定
            if (paramVal == null || paramVal.length <= 0) {
                //※ﾊﾟﾗﾒｰﾀ値未設定の場合

                //文字列指定でないﾊﾟﾗﾒｰﾀか？
                var pos = paramStr.indexOf('@' + colNo);
                if (pos == 0) {
                    paramVal = "null";
                }
                else if (pos > 0) {
                    //「@」直近文字列が「'」でないか？
                    if (paramStr.substr(pos - 1, 1) != "'") {
                        paramVal = "null";
                    }
                }
            }
            paramStr = paramStr.replace('@' + colNo, paramVal);    //「@3」⇒「ｾﾙの値」で置き換え
        }
    });
    return paramStr;
}

/**
 *  複数選択ﾘｽﾄの初期化処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string} ：対象セレクタ
 *  @param {string} ：SQL ID
 *  @param {string} ：SQLパラメータ
 *  @param {number} ：1:先頭に「全て」の項目を追加する / 0:追加しない
 *  @param {number} ：1:必須 / 0:任意
 */
// 2024/03/15 upd 連動処理追加 by ATTS start
//function initMultiSelectBox(appPath, selector, sqlId, param, option, nullCheck, factoryIdList) {
function initMultiSelectBox(appPath, selector, sqlId, param, option, nullCheck, factoryIdList, changeColNo = -1, changeVal = null) {
// 2024/03/15 upd 連動処理追加 by ATTS end
    //複数選択ｺﾝﾎﾞ(ﾘｽﾄ)の場合、ﾁｪｯｸﾎﾞｯｸｽ付きﾘｽﾄ<li><input type="checkbox">

    var isInitallize = !factoryIdList;

    //選択値
    var vals = $(selector).data("value") + '';   //縦棒区切り
    var aryVal = vals.split('|');

    //「全て」の項目を取得
    var optionli = $(selector).find('li.ctrloption');
    // :checkboxタグを取得
    var optioncheck = $(optionli).find(':checkbox');

    //「すべて」のコード値
    var optionCode = "0";

    //詳細検索条件の複数選択チェックボックスかどうか
    var isDetailSearch = $(selector).closest(".detailsearch").length > 0;

    if (isInitallize) {
        //ﾁｪｯｸﾎﾞｯｸｽ選択時ｲﾍﾞﾝﾄ処理を設定
        $(optioncheck).off('change');
        $(optioncheck).on('change', function () {
            if ($(this).prop('checked')) {
                //ﾁｪｯｸ：onの場合

                if ($(this).hasClass("ctrloption")) {
                    //「全て」ｵﾌﾟｼｮﾝ項目の場合、その他のﾁｪｯｸﾎﾞｯｸｽをﾁｪｯｸ：on 
                    var checkes;
                    if (isDetailSearch) {
                        //詳細検索条件の場合、削除アイテムは除外しない
                        checkes = $(selector).find("li>ul>li:not(.hide) :checkbox:not(.ctrloption):unchecked");
                    } else {
                        //削除アイテムは除外する
                        checkes = $(selector).find("li>ul>li:not(.hide):not(.deleteItem) :checkbox:not(.ctrloption):unchecked");
                    }
                    if (checkes != null && checkes.length > 0) {
                        $(checkes).prop('checked', true);
                    }
                } else {
                    //全ての項目がﾁｪｯｸon？(＝ﾁｪｯｸoffの項目がない？)
                    var checkes;
                    if (isDetailSearch) {
                        //詳細検索条件の場合、削除アイテムは除外しない
                        checkes = $(selector).find("li>ul>li:not(.hide) :checkbox:not(.ctrloption):unchecked");
                    } else {
                        //削除アイテムは除外する
                        checkes = $(selector).find("li>ul>li:not(.hide):not(.deleteItem) :checkbox:not(.ctrloption):unchecked");
                    }
                    if (checkes == null || checkes.length == 0) {
                        //ｵﾌﾟｼｮﾝ項目ﾁｪｯｸﾎﾞｯｸｽをﾁｪｯｸ：on
                        var allchk = $(selector).find(":checkbox.ctrloption");
                        $(allchk).prop('checked', true);
                    }
                    //else {
                    //}
                }
            }
            else {
                //ﾁｪｯｸ：offの場合
                if ($(this).hasClass("ctrloption")) {
                    //「全て」ｵﾌﾟｼｮﾝ項目の場合、その他のﾁｪｯｸﾎﾞｯｸｽをﾁｪｯｸ：off
                    var checkes = $(selector).find(":checkbox:not(.ctrloption):checked");
                    if (checkes != null && checkes.length > 0) {
                        $(checkes).prop('checked', false);
                    }
                } else {
                    //「全て」ｵﾌﾟｼｮﾝ項目以外の場合、「全て」ｵﾌﾟｼｮﾝ項目ﾁｪｯｸﾎﾞｯｸｽをﾁｪｯｸ：off
                    var checkes = $(selector).find(":checkbox.ctrloption:checked");
                    if (checkes != null && checkes.length > 0) {
                        $(checkes).prop('checked', false);
                    }
                }
            }
            var td = $(this).closest("td");

            // 保持している選択値を更新
            var selectVal = getCellVal(td);
            setAttrByNativeJs(selector, "data-value", selectVal);
            //ﾁｪｯｸ:onの表示名をｾｯﾄ
            setMutiSelectCheckOnText(td);
        });
    }
    // 2024/03/15 add 連動処理追加 by ATTS start
    else if (changeColNo > 0) {
        //※ｺﾝﾄﾛｰﾙchangedｲﾍﾞﾝﾄ時

        //変更値でﾊﾟﾗﾒｰﾀを書き換え
        //※文字列指定でないﾊﾟﾗﾒｰﾀの場合、ﾊﾟﾗﾒｰﾀ値未設定の場合は「null」を設定
        if (changeVal == null || changeVal.length <= 0) {
            //※ﾊﾟﾗﾒｰﾀ値未設定の場合

            //文字列指定でないﾊﾟﾗﾒｰﾀか？
            var pos = param.indexOf('@' + changeColNo);
            if (pos == 0) {
                changeVal = "null";
            }
            else if (pos > 0) {
                //「@」直近文字列が「'」でないか？
                if (param.substr(pos - 1, 1) != "'") {
                    changeVal = "null";
                }
            }
        }
        param = param.replace('@' + changeColNo, changeVal);    //「@3」⇒「ｾﾙの値」で置き換え
    }
    // 2024/03/15 add 連動処理追加 by ATTS end

    if (option == "1") {
        //選択値の反映
        if (aryVal.indexOf(optionCode) >= 0) {
            //選択値の場合、選択状態にセット
            $(optioncheck).prop('checked', true);
        }
    }

    //「全て」をコピーして規定<li>を作成
    var baseli = $(optionli).clone(true);
    // :checkboxタグを取得
    var basecheck = $(baseli).find('input[type="checkbox"]');

    //「全て」の項目用の設定を除去
    $(baseli).removeClass("ctrloption");    //ﾚｲｱｳﾄ用に付与したｸﾗｽ
    $(baseli).removeClass("hide");          //ﾚｲｱｳﾄ用に付与したｸﾗｽ
    $(basecheck).removeClass("ctrloption");   //制御用に付与したｸﾗｽ
    $(basecheck).prop('checked', false);

    var factoryParam = '';
    var paramList = [];
    if (sqlIdsStructureMst.indexOf(sqlId) >= 0) {
        // 構成マスタのリストの場合、選択中の工場で絞り込む
        if (isInitallize) {
            // 初期化時、連動先の工場コントロールを取得
            //対象行：trを取得する
            var { tr, isHorizontalTbl } = getDataTr(selector);
            var factoryTd = null;
            var fromLocationTree = false;
            if (isHorizontalTbl) {
                // 横型一覧の場合、同一行から取得する
                factoryTd = getFactoryTdElement(selector, tr);
            }
            if (!factoryTd) {
                // 同一画面上から取得する
                factoryTd = getFactoryTdElement(selector);
            }
            if (!factoryTd) {
                // 同一画面上に工場コントロールがない場合、左側の場所階層メニューから取得
                fromLocationTree = true;
            }
            if (fromLocationTree) {
                // 場所階層ツリー(chageイベントの割り当ては行わない)
                paramList = getSelectedFactoryIdList(null, false, true);
                // 場所階層の工場フィルター使用フラグをセット
                setAttrByNativeJs(selector, 'data-usefactoryfilter', true);

            } else if ($(factoryTd).data('type') == 'treeLabel') {
                //ツリー選択ラベル
                var factoryId = $(factoryTd).data('structureid');
                if (factoryId) {
                    paramList.push(factoryId);
                }

                //ｲﾍﾞﾝﾄ付与
                $(factoryTd).get(0).addEventListener('change', function () {
                    factoryId = $(this).data('structureid');
                    var cmbFactoryId = $(selector).data('factoryid');
                    if (cmbFactoryId == null || factoryId != cmbFactoryId) {
                        // 工場IDが変更されたら複数選択チェックボックスの初期化処理を実行する
                        paramList = [factoryId];
                        //選択されている値を取得⇒data-value属性から取得する
                        //var selectVal = getCellVal($(selector).closest("td"));
                        var selectVal = $(selector).data("value");
                        setAttrByNativeJs(selector, "data-value", selectVal);
                        initMultiSelectBox(appPath, selector, sqlId, param, option, nullCheck, paramList)
                    }
                    // 複数選択チェックボックスに工場IDをセットする
                    setAttrByNativeJs(selector, 'data-factoryid', factoryId);
                }, false);

            } else {
                //ｺﾝﾎﾞﾎﾞｯｸｽ
                var select = $(factoryTd).find("select");
                if (select.length > 0) {
                    //ｲﾍﾞﾝﾄ付与
                    $(select).on('change', function () {
                        paramList = [$(this).val()];
                        //選択されている値を取得
                        var selectVal = getCellVal($(selector).closest("td"));
                        setAttrByNativeJs(selector, "data-value", selectVal);
                        initMultiSelectBox(appPath, selector, sqlId, param, option, nullCheck, paramList)
                    });
                }
                select = null;
            }
            factoryTd = null;
            //一覧再表示後の連動ｺﾝﾎﾞの選択ﾘｽﾄ再生成時用の設定を付与しておく
            setAttrByNativeJs(selector, "data-sqlid", sqlId);
            setAttrByNativeJs(selector, "data-param", param);
            setAttrByNativeJs(selector, "data-option", option);
            setAttrByNativeJs(selector, "data-nullcheck", nullCheck);

        }
    }

    // 2024/03/15 add 連動処理追加 by ATTS start
    //SQLパラメータの成形(例：'C01','@3')
    var paramStr = param + '';
    var params = (param + '').split(',');   //ｶﾝﾏで分解

    //対象行：trを取得する
    var { tr, isHorizontalTbl } = getDataTr(selector);

    //列指定ﾊﾟﾗﾒｰﾀ（「@」で始まる引数）の値の置き換えと変更時ｲﾍﾞﾝﾄ処理を付与
    var isDynamic = false;  //動的コントロールか？
    $.each(params, function () {
        var colNo = -1;
        var paramVal = "";

        if (this.indexOf("@") >= 0) {
            //列指定ﾊﾟﾗﾒｰﾀ（「@」で始まる引数）の場合

            isDynamic = true;

            //列番号を取得
            colNo = parseInt(this.replace("'", "").replace("@", ""), 10);  //先頭の「@」を除去してcolNoとする

            //対象列の値を取得
            //入力項目の場合、変更時ｲﾍﾞﾝﾄ処理を付与
            // ⇒ｺﾝﾎﾞﾎﾞｯｸｽ一覧を再作成する

            //対象ｾﾙ
            var td = getDataTd(tr, colNo);
            if (!td || td.length == 0) {
                // 対象セルが存在しない場合
                return true;    // continue;
            }

            if ($(td).data('type') == 'treeLabel') {
                //ツリー選択ラベル
                if (changeColNo < 0) {
                    paramVal = $(td).data('structureid');

                    //ｲﾍﾞﾝﾄ付与
                    $(td).get(0).addEventListener('change', function () {
                        // 工場コントロールのchangeイベント
                        var factoryId = $(this).data('structureid');
                        var cmbFactoryId = $(selector).data('factoryid');
                        if (cmbFactoryId == null || factoryId != cmbFactoryId) {
                            // 工場IDが変更されたらコントロールの初期化処理を実行する
                            initMultiSelectBox(appPath, selector, sqlId, param, option, nullCheck, factoryIdList, colNo, $(this).data('structureid'));
                        }
                        // コントロールに工場IDをセットする
                        setAttrByNativeJs(selector, 'data-factoryid', factoryId);
                    }, false);
                }
                else {
                    paramVal = $(td).data('structureid');
                }

            } else {
                //複数選択ﾘｽﾄ（※inputﾀｸﾞと間違わないように先に実施）
                var msul = $(td).find("ul.multiSelect");
                if (msul.length > 0) {
                    if (changeColNo < 0) {
                        paramVal = $(msul).data("value");
                        // ※連動先にchangeイベントが正しく追加できないため、clickイベントを使用する
                        // ※念のためイベント名に名前空間を付加して他の処理にイベント解除/追加の影響を与えないようにする
                        $(msul).find(":checkbox").off('click.link');
                        $(msul).find(":checkbox").on('click.link', function () {
                            var valW = "";
                            var checkes = $(this).closest("ul").find("li:not(.hide) :checkbox:checked");
                            if (checkes != null && checkes.length > 0) {
                                var vals = [];
                                $.each(checkes, function () {
                                    vals.push($(this).val());
                                });
                                valW = vals.join('|');
                            }
                            initMultiSelectBox(appPath, selector, sqlId, param, option, nullCheck, factoryIdList, colNo, valW);
                        });
                    }
                    else {
                        var checkes = $(msul).find("> li:not(.hide) :checkbox:checked");
                        if (checkes != null && checkes.length > 0) {
                            var vals = [];
                            $.each(checkes, function () {
                                vals.push($(this).val());
                            });
                            paramVal = vals.join('|');
                        }
                    }
                }
                else {
                    //ｺﾝﾎﾞﾎﾞｯｸｽ
                    var select = $(td).find("select");
                    if (select.length > 0) {
                        if (changeColNo < 0) {
                            paramVal = $(select).data("value");

                            //ｲﾍﾞﾝﾄ付与
                            //$(select).off('change');
                            $(select).on('change', function () {
                                initMultiSelectBox(appPath, selector, sqlId, param, option, nullCheck, factoryIdList, colNo, $(this).val());
                            });
                        }
                        else {
                            paramVal = $(select).val();
                        }
                    }
                    else {
                        //ﾁｪｯｸﾎﾞｯｸｽ
                        var checkbox = $(td).find(":checkbox");
                        if (checkbox.length > 0) {
                            paramVal = $(checkbox).prop("checked") ? 1 : 0;

                            if (changeColNo < 0) {
                                //ｲﾍﾞﾝﾄ付与
                                //$(checkbox).off('change');
                                $(checkbox).on('change', function () {
                                    var valW = $(this).prop("checked") ? 1 : 0;
                                    initMultiSelectBox(appPath, selector, sqlId, param, option, nullCheck, factoryIdList, colNo, valW);
                                });
                            }

                        } else {

                            //ﾃｷｽﾄ、数値、ﾃｷｽﾄｴﾘｱ、ｺｰﾄﾞ＋翻訳、日付、時刻、日時
                            var input = $(td).find("input[type='text'], input[type='hidden'], textarea");
                            if (input.length > 0) {
                                paramVal = $(input).val();

                                if (changeColNo < 0) {
                                    //ｲﾍﾞﾝﾄ付与
                                    //$(input).off('change');
                                    $(input).on('change', function () {
                                        initMultiSelectBox(appPath, selector, sqlId, param, option, nullCheck, factoryIdList, colNo, $(this).val());
                                    });
                                }

                            }
                            else {

                                //日付(ブラウザ標準)、時刻(ブラウザ標準)、日時(ブラウザ標準)
                                var dateTime = $(td).find("input[type='date'], input[type='time'], input[type='datetime-local']");
                                if (dateTime.length > 0) {
                                    paramVal = $(dateTime).val().replace(/-/g, "/").replace("T", " ");
                                    if (dateTime.length > 1) {
                                        paramVal = paramVal + '|' + $(dateTime[1]).val().replace(/-/g, "/").replace("T", " ");
                                    }

                                    if (changeColNo < 0) {
                                        //ｲﾍﾞﾝﾄ付与
                                        //$(dateTime).off('change');
                                        $(dateTime).on('change', function () {
                                            var td = $(this).closest("td");
                                            var inputs = $(td).find("input[type='date'], input[type='time'], input[type='datetime-local']");
                                            var valW = $(inputs).val().replace(/-/g, "/").replace("T", " ");
                                            if (inputs.length > 1) {
                                                valW = valW + '|' + $(inputs[1]).val().replace(/-/g, "/").replace("T", " ");
                                            }
                                            initMultiSelectBox(appPath, selector, sqlId, param, option, nullCheck, factoryIdList, colNo, valW);
                                        });
                                    }
                                }
                                else {
                                    //ﾗﾍﾞﾙ
                                    paramVal = $(td).text();
                                }
                                dateTime = null;
                            }
                            input = null;
                        }
                        checkbox = null;

                    }
                    select = null;
                }
                msul = null;
            }
            td = null;

            //SQLパラメータに現時点の値を設定
            //※文字列指定でないﾊﾟﾗﾒｰﾀの場合、ﾊﾟﾗﾒｰﾀ値未設定の場合は「null」を設定
            if (paramVal == null || paramVal.length <= 0) {
                //※ﾊﾟﾗﾒｰﾀ値未設定の場合

                //文字列指定でないﾊﾟﾗﾒｰﾀか？
                var pos = paramStr.indexOf('@' + colNo);
                if (pos == 0) {
                    paramVal = "null";
                }
                else if (pos > 0) {
                    //「@」直近文字列が「'」でないか？
                    if (paramStr.substr(pos - 1, 1) != "'") {
                        paramVal = "null";
                    }
                }
            }
            paramStr = paramStr.replace('@' + colNo, paramVal);    //「@3」⇒「ｾﾙの値」で置き換え
        }
    });
    if (changeColNo < 0) {
        if (isDynamic) {
            //一覧再表示後の連動ｺﾝﾎﾞの選択ﾘｽﾄ再生成時用の設定を付与しておく
            $(selector).addClass("dynamic");
        }
        setAttrByNativeJs(selector, "data-sqlid", sqlId);
        setAttrByNativeJs(selector, "data-param", param);
        setAttrByNativeJs(selector, "data-option", option);
        setAttrByNativeJs(selector, "data-nullcheck", nullCheck);
    }
    // 2024/03/15 add 連動処理追加 by ATTS end

    //const paramKey = sqlId + "," + param;
    const paramKey = sqlId + "_" + param;
    // (2022/11/01) 構成マスタデータをセッションストレージには保存しない
    //var [requiredGetData, data] = getComboBoxDataFromSessionStorage(appPath, paramKey);
    var [requiredGetData, data] = getComboBoxLocalData(paramKey);

    // データ設定処理
    const eventFunc = function () {
        var childul = $('<ul>');
        if (data != null) {
            if (data.length > 0) {
                // 工場IDで絞り込み
                if (factoryIdList == null || factoryIdList.length == 0) {
                    factoryIdList = getSelectedFactoryIdList(null, true, true);
                }
                data = filterStructureItemByFactory(data, factoryIdList);
            }

            vals = $(selector).data("value") + '';   //縦棒区切り
            aryVal = vals.split('|');
            $.each(data, function () {
                var childli = baseli.clone(true);

                // :checkboxタグを取得して選択ﾘｽﾄのｺｰﾄﾞ値をｾｯﾄ
                var checkW = $(childli).find('input[type="checkbox"]');
                $(checkW).val(this.VALUE1);
                //選択値の反映
                if (aryVal.indexOf(optionCode) >= 0 || aryVal.indexOf(this.VALUE1) >= 0) {
                    //選択値の場合、選択状態にセット
                    $(checkW).prop('checked', true);
                }

                // :spanタグに選択ﾘｽﾄの項目名をｾｯﾄ
                var spanW = $(childli).find('span');
                $(spanW).text(this.VALUE2);

                if (this.DeleteFlg && this.DeleteFlg == 'True') {
                    // 削除フラグがTrueの場合、非表示にする
                    childli.addClass('deleteItem');
                }

                $(childli).appendTo($(childul));
            });
        }

        var li = $('<li>');
        $(childul).appendTo($(li));
        //「すべて」のチェックをoff
        $(optioncheck).prop('checked', false);
        // liタグを一旦クリア
        $(selector).find('li:not(.ctrloption)').remove();
        $(li).appendTo($(selector));
        //全ての項目がﾁｪｯｸon？(＝ﾁｪｯｸoffの項目がない？)(削除アイテムは除外する)
        var checkes = $(selector).find("li>ul>li:not(.hide):not(.deleteItem) :checkbox:not(.ctrloption):unchecked");
        if (checkes == null || checkes.length == 0) {
            //「すべて」のチェックをon
            $(optioncheck).prop('checked', true);
        }

        //ﾁｪｯｸ:onの表示名をｾｯﾄ
        var td = $(selector).closest("td");
        setMutiSelectCheckOnText(td);

        //ﾗﾍﾞﾙ表示用spanﾀｸﾞに表示文字列ｾｯﾄ
        setLabelingSpan(td, $(td).find(".multisel-text").text());

    };

    if (data != null) {
        // データ取得成功
        // データ設定処理実行
        eventFunc();

    } else if (!requiredGetData) {
        // データ取得中
        // データ取得中の場合、データ取得中リストから消えるまで待機
        var timer = setInterval(function () {
            if (P_GettingComboBoxDataList.indexOf(paramKey) < 0) {
                // (2022/11/01) 構成マスタデータをセッションストレージには保存しない
                //// データ取得中リストから消えた場合、セッションストレージから取得
                //data = getSavedDataFromSessionStorage(sessionStorageCode.CboMasterData, paramKey);
                // データ取得中リストから消えた場合、グローバル変数から取得
                data = P_ComboBoxJsonList[paramKey];
                // 待機を解除する
                clearInterval(timer);
                // データ設定処理実行
                eventFunc();
            }
        }, 100);

    } else {
        // データ取得が必要
        // コンボデータ取得中リストに追加
        P_GettingComboBoxDataList.push(paramKey);

        // コンボデータ取得処理実行
        var url = encodeURI(appPath + "api/CommonSqlKanriApi/" + sqlId + "?param=" + paramStr);// 手動URLエンコード
        $.ajax({
            url: url,
            type: 'GET',
            dataType: 'json',
            contentType: 'application/json',
            //data: JSON.stringify(postdata),
            traditional: true,
            cache: false
        }).then(
            // 1つめは通信成功時のコールバック
            function (datas) {
                //※正常時
                //結果データ - Dictionary<string,object>
                //結果データからdataを取得
                data = separateDicReturn(datas);
                // (2022/11/01) 構成マスタデータをセッションストレージには保存しない
                //// セッションストレージへ保存
                //setSaveDataToSessionStorage(data, sessionStorageCode.CboMasterData, paramKey)
                P_ComboBoxJsonList[paramKey] = data;
                //★インメモリ化対応 start
                if (existsObjectDataKey(P_ComboBoxMemoryItems, paramKey) && P_ComboBoxMemoryItems[paramKey] == null) {
                    P_ComboBoxMemoryItems[paramKey] = data;
                }
                //★インメモリ化対応 end

                // データ設定処理実行
                eventFunc();

            },
            // 2つめは通信失敗時のコールバック
            function (info) {
                //※異常時
                //info.responseJSON：処理結果ｽﾃｰﾀｽ - CommonProcReturn

                var returnInfo = info.responseJSON;

                //処理結果ｽﾃｰﾀｽを画面状態に反映
                setReturnStatus(appPath, returnInfo, null);
            }
        ).always(
            //通信の完了時に必ず実行される
            function (resultInfo) {
                // コンボデータ取得中リストから削除
                removeComboBoxDataKey(paramKey);
            });
    }

    if (!isInitallize) { return; }

    //ｸﾘｯｸｲﾍﾞﾝﾄを追加
    var text = $(selector).closest("td").find(".multisel-text");
    $(text).off('click');
    $(text).on('click', function () {
        var td = $(this).closest("td");
        var icon = $(td).find('.multisel-icon');
        if (icon.hasClass('glyphicon-chevron-up')) {
            //展開⇒ｸﾛｰｽﾞ
            icon.removeClass('glyphicon-chevron-up');
            icon.addClass('glyphicon-chevron-down');

            //ﾘｽﾄ非表示
            var list = $(td).find('.multisel-drop');
            //list.addClass('disabled');
            multiselCss = resetMultiSelectListPosition(this, list);
            $(list).fadeOut('fast');

        } else if (icon.hasClass('glyphicon-chevron-down')) {
            //ｸﾛｰｽﾞ⇒展開
            icon.removeClass('glyphicon-chevron-down');
            icon.addClass('glyphicon-chevron-up');

            //ﾘｽﾄ展開
            var list = $(td).find('.multisel-drop');
            //list.removeClass('disabled');
            multiselCss = resetMultiSelectListPosition(this, list);
            $(list).css(multiselCss);
        }

        return false;
    });

    //※Edgeでうまく動作しないため、コメント
    //ﾌｫｰｶｽｱｳﾄｲﾍﾞﾝﾄを追加
    //$(text).on('blur', function () {
    //    //現在のﾌｫｰｶｽ確認
    //    var $focused = $(':focus');
    //    var tdfocus = $($focused).closest("td");
    //    var td = $(this).closest("td");

    //    //同じtdならｸﾛｰｽﾞしない
    //    if ($(td).is($(tdfocus))) {
    //        return false;
    //    }
    //    var icon = $(td).find('.multisel-icon');
    //    //ｸﾛｰｽﾞ
    //    icon.removeClass('glyphicon-chevron-up');
    //    icon.addClass('glyphicon-chevron-down');

    //    //ﾘｽﾄ非表示
    //    var list = $(td).find('.multisel-drop');
    //    //list.addClass('disabled');
    //    $(list).fadeOut('fast');

    //    return false;
    //});

    // ｱｲｺﾝｸﾘｯｸで複数選択ﾘｽﾄ展開
    var icon = $(selector).closest("td").find(".multisel-icon");
    $(icon).off('click');
    $(icon).on('click', function () {
        var text = $(this).closest("td").find(".multisel-text");
        $(text)[0].click();
    });
    icon = null;
    optioncheck = null;
    text = null;
}

/**
 * 複数選択リストの初期化処理
 * @param {string} appPath      ：アプリケーションパス
 * @param {number} formNo       ：画面NO
 * @param {boolean} isExclude   ：除外フラグ(true:指定画面NOを除外する/false:指定画面NOを初期化する)
 */
function initMultiSelectBoxes(appPath, formNo, isExclude) {
    var parents = $('article[data-formno], table[data-formno]');
    if (!isExclude) {
        parents = $(parents).filter('[data-formno=0]');
    } else {
        parents = $(parents).filter('[data-formno!=0]');
    }
    if (parents && parents.length > 0) {
        var selectBoxes = $(parents).find('ul[id^="mlt"]');
        $.each(selectBoxes, function (idx, select) {
            callInitMultiSelectBox(appPath, select);
        });
        selectBoxes = null;
    }
    parents = null;
}

/**
 * 複数選択リストの初期化処理呼び出し
 * @param {string} appPath  ：アプリケーションパス
 * @param {string} cbo      ：対象セレクタor要素
 */
function callInitMultiSelectBox(appPath, select) {
    const sqlId = $(select).data('sqlid');
    const param = $(select).data('param');
    const option = $(select).data('option');
    const nullCheck = $(select).data('nullcheck');
    initMultiSelectBox(appPath, select, sqlId, param, option, nullCheck);
}
/**
 * 付近に開いた状態の複数選択チェックボックスがあれば、閉じる
 * @param {any} target チェック対象コントロール
 */
function closeMultiSelectList(target) {
    if (!$(target).closest('.multisel-drop').length && !$(target).closest('.multisel-icon').length) {
        var drop = $('.multisel-drop');
        $(drop).fadeOut('fast');

        //ｱｲｺﾝもｸﾛｰｽﾞ状態へ
        var icon = $(drop).closest("td").find('.multisel-icon');
        icon.removeClass('glyphicon-chevron-up');
        icon.addClass('glyphicon-chevron-down');
    }
}

/**
 * 複数選択リストの表示位置再設定
 */
function resetMultiSelectListPosition(element, dropListElement) {

    var displayType = "block";

    var offset = $(element).offset();

    var left = offset.left;
    left = left + "px";

    //フッターの位置
    var footer = $("footer").offset();
    //リストの高さ
    var dropList = $(dropListElement).outerHeight(true);

    var top = 0;
    var elOuterHeight = $(element).outerHeight(true);
    if (offset.top + dropList > footer.top) {
        //リストを展開した際に画面外にはみ出て見えなくなる場合、上に表示する
        var top = offset.top - dropList;
    } else {
        //通常は下に展開
        var top = elOuterHeight + offset.top;
    }
    top = top + "px";

    var width = $(element).outerWidth();
    width = width + "px";

    // 複数選択リスト要素のｽﾀｲﾙ
    var multiselCss = {
        "position": "fixed",
        "display": displayType,
        "left": left,
        "top": top,
        "width": width
    }
    return multiselCss;
}

/**
 *  ラジオボタンの初期化処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string} ：対象セレクタ(name属性)
 *  @param {string} ：SQL ID
 *  @param {string} ：SQLパラメータ
 *  @param {number} ：1:必須 / 0:任意
 */
function initRadioButton(appPath, selector, sqlId, param, nullCheck) {
    //選択値
    var val = $(selector).data("value");
    var parent = $(selector).closest('div');

    // 管理SQL制御用WebAPIを呼び出し、項目データを取得
    $.getJSON(encodeURI(appPath + "api/CommonSqlKanriApi/" + sqlId + "?param=" + param),
        function (datas) {
            var data = separateDicReturn(datas);
            if (data && data.length > 0) {
                $.each(data, function () {
                    //labelタグごとコピー
                    var base = $(selector + '.baseradio').parent().clone(true);
                    var radio = $(base).find(':radio');
                    $(radio).removeClass('baseradio');

                    //ｺｰﾄﾞ値をｾｯﾄ
                    $(radio).val(this.VALUE1);
                    //選択値の反映
                    if (val == this.VALUE1) {
                        //選択値の場合、選択状態にセット
                        $(radio).prop('checked', true);
                    }
                    //ラベル値をセット
                    $(base).append(this.VALUE2);

                    $(parent).append($(base));
                });
            }
            //ベース用要素を削除
            $(selector + '.baseradio').parent().remove();
        });

}

/**
 *  計算ラベルの初期化処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 */
function initCalcLabel(appPath) {

    var calcTdList = $("td.calc");
    if (calcTdList != null) {
        $.each(calcTdList, function (i, calcTd) {
            //計算ラベルのcolNo
            var calcColNo = parseInt($(calcTd).data("name").replace("VAL", ""));

            //c_relationparamには計算対象のcolNoが含まれている
            var param = $(calcTd).data("param");
            $(calcTd).removeAttr("data-param"); //不要なので削除

            //SQLパラメータの成形(例：'C01','@3')
            var paramStr = param + '';
            var params = (param + '').split(',');   //ｶﾝﾏで分解

            //対象行：trを取得する
            var { tr, isHorizontalTbl } = getDataTr($(calcTd));
            var isDynamic = false;
            //列指定ﾊﾟﾗﾒｰﾀ（「@」で始まる引数）による変更時ｲﾍﾞﾝﾄ処理を付与
            $.each(params, function () {
                var colNo = -1;
                var paramVal = "";

                if (this.indexOf("@") >= 0) {
                    //列指定ﾊﾟﾗﾒｰﾀ（「@」で始まる引数）の場合

                    isDynamic = true;

                    //列番号を取得
                    colNo = parseInt(this.replace("'", "").replace("@", ""), 10);  //先頭の「@」を除去してcolNoとする

                    //対象列の値を取得
                    //入力項目の場合、変更時ｲﾍﾞﾝﾄ処理を付与

                    //対象ｾﾙ
                    var td = getDataTd(tr, colNo);

                    //複数選択ﾘｽﾄ（※inputﾀｸﾞと間違わないように先に実施）
                    var msul = $(td).find("ul.multiSelect");
                    if (msul.length > 0) {

                        //ｲﾍﾞﾝﾄ付与(ﾘｽﾄ内すべてのﾁｪｯｸﾎﾞｯｸｽに設定)
                        $($(msul).find(":checkbox")).on('focusout', function () {
                            //対象行：trを取得
                            var { targetTr, isHorizontalTbl } = getDataTr($(this));
                            execCalcOriginal(appPath, P_Article, targetTr, calcColNo, this);
                        });
                    }
                    else {
                        //ｺﾝﾎﾞﾎﾞｯｸｽ
                        var select = $(td).find("select");
                        if (select.length > 0) {

                            //ｲﾍﾞﾝﾄ付与
                            $(select).on('change', function () {
                                //対象行：trを取得
                                var { targetTr, isHorizontalTbl } = getDataTr($(this));
                                execCalcOriginal(appPath, P_Article, targetTr, calcColNo, this);
                            });

                        }
                        else {

                            //ﾁｪｯｸﾎﾞｯｸｽ
                            var checkbox = $(td).find(":checkbox");
                            if (checkbox.length > 0) {
                                //ｲﾍﾞﾝﾄ付与
                                $(checkbox).on('change', function () {
                                    //対象行：trを取得
                                    var { targetTr, isHorizontalTbl } = getDataTr($(this));
                                    execCalcOriginal(appPath, P_Article, targetTr, calcColNo, this);
                                });

                            } else {

                                //ﾃｷｽﾄ、数値、日付、時刻、ﾃｷｽﾄｴﾘｱ
                                var input = $(td).find("input[type='text'], input[type='hidden'], textarea");
                                if (input.length > 0) {
                                    //ｲﾍﾞﾝﾄ付与
                                    $(input).on('change', function () {
                                        //対象行：trを取得
                                        var { targetTr, isHorizontalTbl } = getDataTr($(this));
                                        execCalcOriginal(appPath, P_Article, targetTr, calcColNo, this);
                                    });

                                }
                                else {
                                    //ﾗﾍﾞﾙ
                                    //ｲﾍﾞﾝﾄ付与できない
                                }
                            }
                            checkbox = null;
                        }
                        select = null;
                    }
                    msul = null;
                }
            });

        });
    }
}

//★AKAP2.0標準
/**
*  一覧ソートの初期化処理
*  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
*  @param {string} ：機能ID
*  @param {string} ：ﾌﾟﾛｸﾞﾗﾑID
*/
function initListSort(appPath, targetSelector) {
    if (!targetSelector) {
        targetSelector = 'th';
    }
    var target = $(targetSelector);
    if (!target || target.length == 0) {
        target = null;
        return;
    }

    // ﾀﾞﾌﾞﾙｸﾘｯｸした列番号をｷｰに一覧をｿｰﾄする
    $(target).on('dblclick', function () {
        var selector = $(this);
        // 横方向一覧
        if (!$(selector).parents('table').hasClass('vertical_tbl')) {
            // ﾃﾞｰﾀ行を取得
            var tbl = $(selector).closest('table');
            var trs = $(tbl).children('tbody').find("tr:not([class^='base_tr'])");
            if (trs.length > 1) {
                const conductId = getConductId();
                const pgmId = getProgramIdByElement(selector);
                listSort(appPath, conductId, pgmId, selector);    // ｿｰﾄ処理
            }
        }
    });
    target = null;
}

/**
*  一覧ソート処理
*  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
*  @param {string} ：機能ID
*  @param {string} ：ﾌﾟﾛｸﾞﾗﾑID
*  @param {select} ：選択要素
*/
function listSort(appPath, conductId, pgmId, selector) {

    var form = $(P_Article).find("form[id^='formDetail']");
    var formNo = getFormNo();
    var tbl = $(selector).closest('table').first();
    var ctrlId = $(tbl).data("ctrlid");                       // ﾃｰﾌﾞﾙ(一覧)CtrlId
    var pageRows = $(tbl).data("pagerows");                 // 一覧の画面定義の1ページ行数
    var key = $(selector).data("name").replace("VAL", "");   // ｿｰﾄ列番号(VAL1～)

    //明細エリアのエラー状態を初期化
    clearErrorStatus(form);

    //ﾍﾟｰｼﾞ情報
    $.each(P_listDefines, function (i, define) {
        // 一覧の画面定義条件を生成
        if (ctrlId != null && ctrlId == define.CTRLID) {
            var tmpData = {
                VAL4: key
            };
            $.extend(define, tmpData);    // 一覧のソートキー
        }
    });

    var ctrltype = $(tbl).data('ctrltype');
    if (ctrltype == ctrlTypeDef.IchiranPtn1) {
        // 101の場合はソート不要
        return;
    }
    if (dataEditedFlg && ctrltype != ctrlTypeDef.IchiranPtn2) {
        // 画面変更ﾌﾗｸﾞONの場合、ﾃﾞｰﾀ破棄確認ﾒｯｾｰｼﾞ

        //確認ﾒｯｾｰｼﾞを設定
        P_MessageStr = P_ComMsgTranslated[941060005] //『画面の編集内容は破棄されます。よろしいですか？』
        //処理継続用ｺｰﾙﾊﾞｯｸ関数呼出文字列を設定
        var eventFunc = function () {

            var pageNo = 1;
            //ﾍﾟｰｼﾞｺﾝﾄﾛｰﾙの状態ｾｯﾄ
            setPaginationStatus(ctrlId, pageNo);

            // ページデータの取得
            getPageData1(appPath, conductId, pgmId, formNo, pageNo, pageRows, ctrlId, key);
        }
        // 確認ﾒｯｾｰｼﾞを表示
        if (!popupMessage([P_MessageStr], messageType.Confirm, eventFunc)) {
            //『キャンセル』の場合、処理中断
            return false;
        }
    }
    else {

        var pageNo = 1;
        //pageNo = getCurrentPageNo(tblId, "def");                 //選択行番号

        //if ($(selector).attr("name") == "selectNext") {
        //    pageNo = pageNo + 1;
        //}
        //else {
        //    pageNo = pageNo - 1;
        //}
        //ﾍﾟｰｼﾞｺﾝﾄﾛｰﾙの状態ｾｯﾄ
        setPaginationStatus(ctrlId, pageNo);

        // ページデータの取得
        getPageData1(appPath, conductId, pgmId, formNo, pageNo, pageRows, ctrlId, key);
    }
}
//★AKAP2.0標準

/**
 *  Validatorにﾁｪｯｸﾒｿｯﾄﾞを追加
 */
function initValidatorAddMethod() {

    //==ﾒｿｯﾄﾞの生成==
    //日付ﾁｪｯｸ
    $.validator.addMethod('comDate', function (value, element) {

        var optional = this.optional(element);
        if (optional) {
            return optional;
        }

        var fmt = $(element).data("format");    //日付ﾌｫｰﾏｯﾄ

        //年月日のﾁｪｯｸ対象を取得
        var isCheckY = false;
        var isCheckM = false;
        var isCheckD = false;
        if (fmt.indexOf('y') > -1) {
            isCheckY = true;
        }
        if (fmt.indexOf('m') > -1) {
            isCheckM = true;
        }
        if (fmt.indexOf('d') > -1) {
            isCheckD = true;
        }

        if (isCheckM & !isCheckY & !isCheckD) {
            //月のみの場合

            var valM = parseInt(value, 10);
            //1～12月か？
            if (1 <= valM && valM <= 12) {
                return true;
            }
            return false;

        } else if (isCheckD & !isCheckY & !isCheckM) {
            //日のみの場合

            var valD = parseInt(value, 10);
            //1～31日か？
            if (1 <= valD && valD <= 31) {
                return true;
            }
            return false;
        } else {
            if (isCheckY & !isCheckM & !isCheckD) {
                //年のみの場合、max桁数：4桁ﾁｪｯｸ
                var byteLen = getByteLength(value);
                if (4 < byteLen) {
                    //4桁より大きい場合、エラー
                    return false;
                }
            }

            //日付として成立するかﾁｪｯｸ

            //ﾁｪｯｸ値を生成(yyyy/mm/dd)
            var CheckVal = "";

            //現在日付
            var dt = new Date();

            var defY = dt.getFullYear();    //現在日付年
            var defM = "01";
            var defD = "01";

            var valW = value + '';

            if (isCheckY) {   //年が入力対象の場合
                //入力日付の年を付与
                CheckVal += (valW.substr(0, 4) + '/');
                valW = valW.substr(5);      //残りの入力文字列
            } else {
                //ﾃﾞﾌｫﾙﾄの年を付与
                CheckVal += (defY + '/');
            }

            if (isCheckM) {   //月が入力対象の場合
                //入力日付の月を付与
                CheckVal += (valW.substr(0, 2) + '/');
                valW = valW.substr(3);      //残りの入力文字列
            } else {
                //ﾃﾞﾌｫﾙﾄの月を付与
                CheckVal += (defM + '/');
            }
            if (isCheckD) {   //日が入力対象の場合
                //入力日付の日を付与
                CheckVal += valW.substr(0, 2);
            } else {
                //ﾃﾞﾌｫﾙﾄの日を付与
                CheckVal += defD;
            }

            var date = new Date(CheckVal);

            //ﾁｪｯｸﾌｫｰﾏｯﾄ
            var fmt_yyyymmdd = /^\d{4}\/\d{2}\/\d{2}$/;
            var delimiter = '/';

            // 日付文字列でない、またはフォーマット通りに入力されていない場合はNGとなる
            if (/Invalid|NaN/.test(date.toString()) || !fmt_yyyymmdd.test(CheckVal)) {
                return false;
            }

            //日付の他に入力値が存在しないか確認
            if (isCheckY & isCheckM & isCheckD) {
                if (CheckVal != value) return false
            }

            // 入力値とnewDate.toStringを文字列比較する。
            // 実際には無い日付（2013/04/31）をnewDateすると勝手に変換（2013/05/01）するのでその対策。
            // なお、31日だけこの現象が起こる。32日以降はnewDateでもinvalid判定になる。
            var m = '0' + (date.getMonth() + 1);
            var d = '0' + date.getDate();
            var newDateStr = date.getFullYear() + delimiter + m.slice(-2) + delimiter + d.slice(-2);

            return newDateStr === CheckVal;
        }

    }, null);

    //日時ﾁｪｯｸ
    $.validator.addMethod('comDateTime', function (value, element) {

        var optional = this.optional(element);
        if (optional) {
            return optional;
        }

        var fmt = $(element).data("format");    //日時ﾌｫｰﾏｯﾄ

        // ﾌｫｰﾏｯﾄを日付、時刻に分割
        var fmts = fmt.split(" ");
        var fmtDate = fmts[0];
        var fmtTime = fmts.length > 1 ? fmts[1] : "";

        // 値を日付、時刻に分割
        var values = value.split(" ");
        var valDate = values[0];
        var valTime = values.length > 1 ? values[1] : "";

        //年月日のﾁｪｯｸ対象を取得
        var isCheckY = false;
        var isCheckM = false;
        var isCheckD = false;

        if (fmtDate.indexOf('y') > -1) {
            isCheckY = true;
        }
        if (fmtDate.indexOf('m') > -1) {
            isCheckM = true;
        }
        if (fmtDate.indexOf('d') > -1) {
            isCheckD = true;
        }

        if (isCheckM & !isCheckY & !isCheckD) {
            //月のみの場合

            var valM = parseInt(valDate, 10);
            //1～12月か？
            if (!(1 <= valM && valM <= 12)) {
                return false;
            }

        } else if (isCheckD & !isCheckY & !isCheckM) {
            //日のみの場合

            var valD = parseInt(valDate, 10);
            //1～31日か？
            if (!(1 <= valD && valD <= 31)) {
                return false;
            }
        } else {
            if (isCheckY & !isCheckM & !isCheckD) {
                //年のみの場合、max桁数：4桁ﾁｪｯｸ
                var byteLen = getByteLength(valDate);
                if (4 < byteLen) {
                    //4桁より大きい場合、エラー
                    return false;
                }
            }

            //日付として成立するかﾁｪｯｸ

            //ﾁｪｯｸ値を生成(yyyy/mm/dd)
            var CheckVal = "";

            //現在日付
            var dt = new Date();

            var defY = dt.getFullYear();    //現在日付年
            var defM = "01";
            var defD = "01";

            var valW = valDate + '';

            if (isCheckY) {   //年が入力対象の場合
                //入力日付の年を付与
                CheckVal += (valW.substr(0, 4) + '/');
                valW = valW.substr(5);      //残りの入力文字列
            } else {
                //ﾃﾞﾌｫﾙﾄの年を付与
                CheckVal += (defY + '/');
            }

            if (isCheckM) {   //月が入力対象の場合
                //入力日付の月を付与
                CheckVal += (valW.substr(0, 2) + '/');
                valW = valW.substr(3);      //残りの入力文字列
            } else {
                //ﾃﾞﾌｫﾙﾄの月を付与
                CheckVal += (defM + '/');
            }
            if (isCheckD) {   //日が入力対象の場合
                //入力日付の日を付与
                CheckVal += valW.substr(0, 2);
            } else {
                //ﾃﾞﾌｫﾙﾄの日を付与
                CheckVal += defD;
            }

            var date = new Date(CheckVal);

            //ﾁｪｯｸﾌｫｰﾏｯﾄ
            var fmt_yyyymmdd = /^\d{4}\/\d{2}\/\d{2}$/;
            var delimiter = '/';

            // 日付文字列でない、またはフォーマット通りに入力されていない場合はNGとなる
            if (/Invalid|NaN/.test(date.toString()) || !fmt_yyyymmdd.test(CheckVal)) {
                return false;
            }

            //日付の他に入力値が存在しないか確認
            if (isCheckY & isCheckM & isCheckD) {
                if (CheckVal != valDate) return false
            }

            // 入力値とnewDate.toStringを文字列比較する。
            // 実際には無い日付（2013/04/31）をnewDateすると勝手に変換（2013/05/01）するのでその対策。
            // なお、31日だけこの現象が起こる。32日以降はnewDateでもinvalid判定になる。
            var m = '0' + (date.getMonth() + 1);
            var d = '0' + date.getDate();
            var newDateStr = date.getFullYear() + delimiter + m.slice(-2) + delimiter + d.slice(-2);

            if (newDateStr != CheckVal) {
                return false;
            }
        }

        //時分のﾁｪｯｸ対象を取得
        var isCheckH = false;
        var isCheckMi = false;
        var isCheckS = false;
        if (fmtTime.indexOf('H') > -1) {
            isCheckH = true;
        }
        if (fmtTime.indexOf('m') > -1) {
            isCheckMi = true;
        }
        if (fmtTime.indexOf('s') > -1) {
            isCheckS = true;
        }

        //時刻を取り出し
        var valTimeStr = valTime.replace(/:/g, '');   //ｺﾛﾝを除去

        if (isCheckS) {     //分がﾁｪｯｸ対象の場合
            var valNumS = parseInt(valTimeStr.slice(-2), 10);    //数値変換
            //分が0-59の範囲外の場合、ﾁｪｯｸｴﾗｰ
            if (valNumS < 0 || valNumS > 59) {
                return false;
            }

            valTimeStr = valTimeStr.substr(0, valTimeStr.length - 2);   //残りの文字列
        }
        if (isCheckM) {     //分がﾁｪｯｸ対象の場合
            var valNumM = parseInt(valTimeStr.slice(-2), 10);    //数値変換
            //分が0-59の範囲外の場合、ﾁｪｯｸｴﾗｰ
            if (valNumM < 0 || valNumM > 59) {
                return false;
            }

            valTimeStr = valTimeStr.substr(0, valTimeStr.length - 2);   //残りの文字列
        }
        if (isCheckH) {     //時がﾁｪｯｸ対象の場合
            var valNumH = parseInt(valTimeStr.slice(-2), 10);    //数値変換
            //時が0-23の範囲外の場合、ﾁｪｯｸｴﾗｰ
            if (valNumH < 0 || valNumH > 23) {
                return false;
            }
        }

        return true;   //ﾁｪｯｸOK

    }, null);

    //最大文字数ﾁｪｯｸ(ﾊﾞｲﾄ数)
    $.validator.addMethod('comMaxByteLength', function (value, element, param) {
        //param：最大文字数(必須定義項目)

        //未入力の場合、ﾁｪｯｸをｽﾙｰ
        if (this.optional(element)) {
            return true;
        }

        //最大文字数定義なしの場合、ﾃﾞﾌｫﾙﾄ値
        if (param == null) {
            param = 0;
        }
        // 入力可能文字数が255byteより大きいためこのチェックは不可
        //// 中間ﾃｰﾌﾞﾙのｴﾘｱの関係上、255byteより大きい値は設定不可
        //if (param == -1 || param > maxLengthApp) {
        //    param = maxLengthApp;
        //}
        if (param == -1) {
            // チェック無し
            return true;
        }
        //var byteLen = getByteLength(value);
        //if (param >= byteLen) {
        //    //最大文字数以下か？
        //    return true;
        //}
        // 文字列長さ
        var strLen = value.length;
        if (param >= strLen) {
            //最大文字数以下か？
            return true;
        }

        return false;   //ﾁｪｯｸｴﾗｰ
    }, null);

    // 規定の文字数チェックが動作してしまうので、常にTrueを返してエラーとならないようにする
    $.validator.addMethod('maxlength', function (value, element, param) {
        return true;
    });
    //数値範囲ﾁｪｯｸ
    $.validator.addMethod('comNumRange', function (value, element, param) {
        //param[0]：最小値
        //param[1]：最大値

        //未入力の場合、ﾁｪｯｸをｽﾙｰ
        if (this.optional(element)) {
            return true;
        }

        //範囲ﾁｪｯｸ定義なしの場合、ﾁｪｯｸをｽﾙｰ
        var isMinCheck = true;
        if (param[0] == null || (param[0] + '').length <= 0) {
            isMinCheck = false;
        }
        var isMaxCheck = true;
        if (param[1] == null || (param[1] + '').length <= 0) {
            isMaxCheck = false;
        }
        if (isMinCheck == false && isMaxCheck == false) {
            return true;
        }

        //数値を取り出し
        var valNumStr = value.replace(/,/g, '');     //ｶﾝﾏを除去
        var valNum = parseFloat(valNumStr);         //数値変換

        //最小値より小さい場合、ﾁｪｯｸｴﾗｰ
        if (isMinCheck && valNum < param[0]) {
            return false;
        }
        //最大値より大きい場合、ﾁｪｯｸｴﾗｰ
        if (isMaxCheck && valNum > param[1]) {
            return false;
        }

        return true;   //ﾁｪｯｸOK
    }, null);

    // FromTo判別用
    // Toの場合True
    var isEqualNameTo = function (name) {
        // VAL9Toを含むか判定
        var result = name.match(/VAL\d+To/);
        // Not Nullの場合含むのでTrue
        return result != null;
    }
    // ToからFromのIDを求める
    var getFromIdByTo = function (toId) {
        var result = toId.replace(/(VAL\d+)To/, "$1");
        return result;
    }

    //数値大小ﾁｪｯｸ
    $.validator.addMethod('comNumFromTo', function (value, element) {

        //未入力の場合、ﾁｪｯｸをｽﾙｰ
        if (this.optional(element)) {
            return true;
        }

        //FromTo以外の場合、ﾁｪｯｸをｽﾙｰ
        if (!$(element).hasClass("fromto")) {
            return true;
        }

        //Fromの場合、ﾁｪｯｸをｽﾙｰ
        var nameTo = element.name;
        if (!isEqualNameTo(nameTo)) {
            return true;
        }

        //From取得
        var valueFrom = "";
        var nameFrom = getFromIdByTo(nameTo);
        var td = $(element).closest('td');
        var inputs = $(td).find("input");
        if (inputs.length > 0) {
            $.each(inputs, function (idx, input) {
                var name = input.name;
                if (name != null && name.length > 0) {
                    if (name == nameFrom) {
                        valueFrom = $(input).val();
                        return false;
                    }
                }
            });
        }
        //未入力の場合、ﾁｪｯｸをｽﾙｰ
        if (valueFrom.length <= 0) {
            return true;
        }

        //数値変換
        var valNumFrom = parseFloat(valueFrom.replace(/,/g, ''));
        var valNumTo = parseFloat(value.replace(/,/g, ''));

        //To < From の場合、ﾁｪｯｸｴﾗｰ
        if (valNumTo < valNumFrom) {
            return false;
        }

        return true;   //ﾁｪｯｸOK
    }, null);

    //時刻範囲ﾁｪｯｸ
    $.validator.addMethod('comTimeRange', function (value, element) {
        var optional = this.optional(element);
        if (optional) {
            return optional;
        }

        var fmt = $(element).data("format");    //日付ﾌｫｰﾏｯﾄ

        //時分のﾁｪｯｸ対象を取得
        var isCheckH = false;
        var isCheckM = false;
        var isCheckS = false;
        if (fmt.indexOf('H') > -1) {
            isCheckH = true;
        }
        if (fmt.indexOf('m') > -1) {
            isCheckM = true;
        }
        if (fmt.indexOf('s') > -1) {
            isCheckS = true;
        }

        //時刻を取り出し
        var valTimeStr = value.replace(/:/g, '');   //ｺﾛﾝを除去

        if (isCheckS) {     //分がﾁｪｯｸ対象の場合
            var valNumS = parseInt(valTimeStr.slice(-2), 10);    //数値変換
            //分が0-59の範囲外の場合、ﾁｪｯｸｴﾗｰ
            if (valNumS < 0 || valNumS > 59) {
                return false;
            }

            valTimeStr = valTimeStr.substr(0, valTimeStr.length - 2);   //残りの文字列
        }
        if (isCheckM) {     //分がﾁｪｯｸ対象の場合
            var valNumM = parseInt(valTimeStr.slice(-2), 10);    //数値変換
            //分が0-59の範囲外の場合、ﾁｪｯｸｴﾗｰ
            if (valNumM < 0 || valNumM > 59) {
                return false;
            }

            valTimeStr = valTimeStr.substr(0, valTimeStr.length - 2);   //残りの文字列
        }
        if (isCheckH) {     //時がﾁｪｯｸ対象の場合
            var valNumH = parseInt(valTimeStr.slice(-2), 10);    //数値変換
            //時が0-23の範囲外の場合、ﾁｪｯｸｴﾗｰ
            if (valNumH < 0 || valNumH > 23) {
                return false;
            }
        }

        return true;   //ﾁｪｯｸOK
    }, null);

    //パスワード文字数ﾁｪｯｸ(ﾊﾞｲﾄ数)
    $.validator.addMethod('passwordByteLengthRange', function (value, element, param) {
        //param：最大文字数(必須定義項目)

        //未入力の場合、ﾁｪｯｸをｽﾙｰ
        if (this.optional(element)) {
            return true;
        }

        //最小文字数定義なしの場合、ﾃﾞﾌｫﾙﾄ値
        if (param == null) {
            param = 8;
        }
        //最大文字数
        var maxLen = 128;

        var byteLen = getByteLength(value);
        if (param <= byteLen && byteLen <= maxLen) {
            //最小文字数以上かつ最大文字数以下か？
            return true;
        }

        return false;   //ﾁｪｯｸｴﾗｰ
    }, null);

    //新しいパスワード(確認)チェック
    $.validator.addMethod('newPasswordConfirm', function (value, element, param) {
        //param：最大文字数(必須定義項目)

        //未入力の場合、ﾁｪｯｸをｽﾙｰ
        if (this.optional(element)) {
            return true;
        }

        //新しいパスワード(確認)以外の場合、ｽﾙｰ
        if (!param) {
            return true;
        }
        else {
            //最小文字数定義なしの場合、ﾃﾞﾌｫﾙﾄ値
            if (param[1] == null) {
                param[1] = 8;
            }
            //最大文字数
            var maxLen = 128;

            var byteLen = getByteLength(value);
            if (param > byteLen || byteLen > maxLen) {
                //最小文字数以下または最大文字数以上の場合、ﾁｪｯｸをｽﾙｰ
                return true;
            }

            //新しいパスワードと一致する場合、ｽﾙｰ
            var NewLoginPassword = $("#passwordChange_detail_div").find("input:password[name='NewLoginPassword_text']").val();
            if (NewLoginPassword == null) {
                return true;
            }
            if (value == NewLoginPassword) {
                return true;
            }
        }
        return false;   //ﾁｪｯｸｴﾗｰ
    }, null);

    //日付大小ﾁｪｯｸ
    $.validator.addMethod('comDateFromTo', function (value, element) {

        //未入力の場合、ﾁｪｯｸをｽﾙｰ
        if (this.optional(element)) {
            return true;
        }

        //FromTo以外の場合、ﾁｪｯｸをｽﾙｰ
        if (!$(element).hasClass("fromto")) {
            return true;
        }

        //Fromの場合、ﾁｪｯｸをｽﾙｰ
        var nameTo = element.name;
        if (!isEqualNameTo(nameTo)) {
            return true;
        }

        //From取得
        var valueFrom = "";
        var nameFrom = getFromIdByTo(nameTo);
        var td = $(element).closest('td');
        var inputs = $(td).find("input");
        if (inputs.length > 0) {
            $.each(inputs, function (idx, input) {
                var name = input.name;
                if (name != null && name.length > 0) {
                    if (name == nameFrom) {
                        valueFrom = $(input).val();
                        return false;
                    }
                }
            });
        }
        //未入力の場合、ﾁｪｯｸをｽﾙｰ
        if (valueFrom.length <= 0) {
            return true;
        }

        var fmt = $(element).data("format");    //日付ﾌｫｰﾏｯﾄ

        //年月日のﾁｪｯｸ対象を取得
        var isCheckY = false;
        var isCheckM = false;
        var isCheckD = false;
        if (fmt.indexOf('y') > -1) {
            isCheckY = true;
        }
        if (fmt.indexOf('m') > -1) {
            isCheckM = true;
        }
        if (fmt.indexOf('d') > -1) {
            isCheckD = true;
        }

        //現在日付
        var dt = new Date();

        var defY = dt.getFullYear();    //現在日付年
        var defM = "01";
        var defD = "01";

        //日付として成立するかﾁｪｯｸ
        var valStrFrom;
        var valStrTo;
        var checkVals = [valueFrom, value];
        for (var i = 0; i < checkVals.length; i++) {

            var valW = checkVals[i] + '';

            //ﾁｪｯｸ値を生成(yyyy/mm/dd)
            var CheckVal = "";

            if (isCheckY) {   //年が入力対象の場合
                //入力日付の年を付与
                CheckVal += (valW.substr(0, 4) + '/');
                valW = valW.substr(5);      //残りの入力文字列
            } else {
                //ﾃﾞﾌｫﾙﾄの年を付与
                CheckVal += (defY + '/');
            }

            if (isCheckM) {   //月が入力対象の場合
                //入力日付の月を付与
                CheckVal += (valW.substr(0, 2) + '/');
                valW = valW.substr(3);      //残りの入力文字列
            } else {
                //ﾃﾞﾌｫﾙﾄの月を付与
                CheckVal += (defM + '/');
            }
            if (isCheckD) {   //日が入力対象の場合
                //入力日付の日を付与
                CheckVal += valW.substr(0, 2);
            } else {
                //ﾃﾞﾌｫﾙﾄの日を付与
                CheckVal += defD;
            }

            var date = new Date(CheckVal);

            //文字列変換
            var fmt_str = 'yyyyMMdd';
            fmt_str = fmt_str.replace(/yyyy/g, date.getFullYear());
            fmt_str = fmt_str.replace(/MM/g, ('0' + (date.getMonth() + 1)).slice(-2));
            fmt_str = fmt_str.replace(/dd/g, ('0' + date.getDate()).slice(-2));
            if (i < 1) {
                valStrFrom = fmt_str;
            } else {
                valStrTo = fmt_str;
            }
        }

        //数値変換
        var valNumFrom = parseInt(valStrFrom, 10);
        var valNumTo = parseInt(valStrTo, 10);

        //To < From の場合、ﾁｪｯｸｴﾗｰ
        if (valNumTo < valNumFrom) {
            return false;
        }

        return true;   //ﾁｪｯｸOK
    }, null);

    //時刻大小ﾁｪｯｸ
    $.validator.addMethod('comTimeFromTo', function (value, element) {

        //未入力の場合、ﾁｪｯｸをｽﾙｰ
        var optional = this.optional(element);
        if (optional) {
            return optional;
        }

        //FromTo以外の場合、ﾁｪｯｸをｽﾙｰ
        if (!$(element).hasClass("fromto")) {
            return true;
        }

        //Fromの場合、ﾁｪｯｸをｽﾙｰ
        var nameTo = element.name;
        if (!isEqualNameTo(nameTo)) {
            return true;
        }

        //From取得
        var valueFrom = "";
        var nameFrom = getFromIdByTo(nameTo);
        var td = $(element).closest('td');
        var inputs = $(td).find("input");
        if (inputs.length > 0) {
            $.each(inputs, function (idx, input) {
                var name = input.name;
                if (name != null && name.length > 0) {
                    if (name == nameFrom) {
                        valueFrom = $(input).val();
                        return false;
                    }
                }
            });
        }
        //未入力の場合、ﾁｪｯｸをｽﾙｰ
        if (valueFrom.length <= 0) {
            return true;
        }

        var fmt = $(element).data("format");    //日付ﾌｫｰﾏｯﾄ

        //時分秒のﾁｪｯｸ対象を取得
        var isCheckH = false;
        var isCheckM = false;
        var isCheckS = false;
        if (fmt.indexOf('H') > -1) {
            isCheckH = true;
        }
        if (fmt.indexOf('m') > -1) {
            isCheckM = true;
        }
        if (fmt.indexOf('s') > -1) {
            isCheckS = true;
        }

        var valNumFrom;
        var valNumTo;

        var valTimes = [valueFrom, value];
        for (var i = 0; i < valTimes.length; i++) {

            var valNumS = 0;    //秒
            var valNumM = 0;    //分
            var valNumH = 0;    //時

            //時刻を取り出し
            var valTimeStr = valTimes[i].replace(/:/g, '');   //ｺﾛﾝを除去

            if (isCheckS) {     //秒がﾁｪｯｸ対象の場合
                valNumS = parseInt(valTimeStr.slice(-2), 10);    //数値変換
                valTimeStr = valTimeStr.substr(0, valTimeStr.length - 2);   //残りの文字列
            }
            if (isCheckM) {     //分がﾁｪｯｸ対象の場合
                valNumM = parseInt(valTimeStr.slice(-2), 10);    //数値変換
                valTimeStr = valTimeStr.substr(0, valTimeStr.length - 2);   //残りの文字列
            }
            if (isCheckH) {     //時がﾁｪｯｸ対象の場合
                valNumH = parseInt(valTimeStr.slice(-2), 10);    //数値変換
            }

            //秒変換
            var valNum = valNumH * 3600 + valNumM * 60 + valNumS;

            if (i < 1) {
                valNumFrom = valNum;
            } else {
                valNumTo = valNum;
            }
        }

        //To < From の場合、ﾁｪｯｸｴﾗｰ
        if (valNumTo < valNumFrom) {
            return false;
        }

        return true;   //ﾁｪｯｸOK
    }, null);

    //日時大小ﾁｪｯｸ
    $.validator.addMethod('comDateTimeFromTo', function (value, element) {

        //未入力の場合、ﾁｪｯｸをｽﾙｰ
        var optional = this.optional(element);
        if (optional) {
            return optional;
        }

        //FromTo以外の場合、ﾁｪｯｸをｽﾙｰ
        if (!$(element).hasClass("fromto")) {
            return true;
        }

        //Fromの場合、ﾁｪｯｸをｽﾙｰ
        var nameTo = element.name;
        if (!isEqualNameTo(nameTo)) {
            return true;
        }

        //From取得
        var valueFrom = "";
        var nameFrom = getFromIdByTo(nameTo);
        var td = $(element).closest('td');
        var inputs = $(td).find("input");
        if (inputs.length > 0) {
            $.each(inputs, function (idx, input) {
                var name = input.name;
                if (name != null && name.length > 0) {
                    if (name == nameFrom) {
                        valueFrom = $(input).val();
                        return false;
                    }
                }
            });
        }
        //未入力の場合、ﾁｪｯｸをｽﾙｰ
        if (valueFrom.length <= 0) {
            return true;
        }

        var fmt = $(element).data("format");    //日時ﾌｫｰﾏｯﾄ

        // ﾌｫｰﾏｯﾄを日付、時刻に分割
        var fmts = fmt.split(" ");
        var fmtDate = fmts[0];
        var fmtTime = fmts.length > 1 ? fmts[1] : "";

        // 値を日付、時刻に分割
        // From
        var valuesFrom = valueFrom.split(" ");
        var valDateFrom = valuesFrom[0];
        var valTimeFrom = valuesFrom.length > 1 ? valuesFrom[1] : "";
        // To
        var valuesTo = value.split(" ");
        var valDateTo = valuesTo[0];
        var valTimeTo = valuesTo.length > 1 ? valuesTo[1] : "";

        //年月日のﾁｪｯｸ対象を取得
        var isCheckY = false;
        var isCheckM = false;
        var isCheckD = false;

        if (fmtDate.indexOf('y') > -1) {
            isCheckY = true;
        }
        if (fmtDate.indexOf('m') > -1) {
            isCheckM = true;
        }
        if (fmtDate.indexOf('d') > -1) {
            isCheckD = true;
        }

        //現在日付
        var dt = new Date();

        var defY = dt.getFullYear();    //現在日付年
        var defM = "01";
        var defD = "01";

        //日付として成立するかﾁｪｯｸ
        var valDateStrFrom;
        var valDateStrTo;
        var checkVals = [valDateFrom, valDateTo];
        for (var i = 0; i < checkVals.length; i++) {

            var valW = checkVals[i] + '';

            //ﾁｪｯｸ値を生成(yyyy/mm/dd)
            var CheckVal = "";

            if (isCheckY) {   //年が入力対象の場合
                //入力日付の年を付与
                CheckVal += (valW.substr(0, 4) + '/');
                valW = valW.substr(5);      //残りの入力文字列
            } else {
                //ﾃﾞﾌｫﾙﾄの年を付与
                CheckVal += (defY + '/');
            }

            if (isCheckM) {   //月が入力対象の場合
                //入力日付の月を付与
                CheckVal += (valW.substr(0, 2) + '/');
                valW = valW.substr(3);      //残りの入力文字列
            } else {
                //ﾃﾞﾌｫﾙﾄの月を付与
                CheckVal += (defM + '/');
            }
            if (isCheckD) {   //日が入力対象の場合
                //入力日付の日を付与
                CheckVal += valW.substr(0, 2);
            } else {
                //ﾃﾞﾌｫﾙﾄの日を付与
                CheckVal += defD;
            }

            var date = new Date(CheckVal);

            //文字列変換
            var fmt_str = 'yyyyMMdd';
            fmt_str = fmt_str.replace(/yyyy/g, date.getFullYear());
            fmt_str = fmt_str.replace(/MM/g, ('0' + (date.getMonth() + 1)).slice(-2));
            fmt_str = fmt_str.replace(/dd/g, ('0' + date.getDate()).slice(-2));
            if (i < 1) {
                valDateStrFrom = fmt_str;
            } else {
                valDateStrTo = fmt_str;
            }
        }

        //数値変換
        var valDateNumFrom = parseInt(valDateStrFrom, 10);
        var valDateNumTo = parseInt(valDateStrTo, 10);

        //To < From の場合、ﾁｪｯｸｴﾗｰ
        if (valDateNumTo < valDateNumFrom) {
            return false;
        }

        //時分のﾁｪｯｸ対象を取得
        var isCheckH = false;
        var isCheckMi = false;
        var isCheckS = false;
        if (fmtTime.indexOf('H') > -1) {
            isCheckH = true;
        }
        if (fmtTime.indexOf('m') > -1) {
            isCheckMi = true;
        }
        if (fmtTime.indexOf('s') > -1) {
            isCheckS = true;
        }

        var valTimeNumFrom;
        var valTimeNumTo;

        var valTimes = [valTimeFrom, valTimeTo];
        for (var i = 0; i < valTimes.length; i++) {

            var valTimeNumS = 0;    //秒
            var valTimeNumM = 0;    //分
            var valTimeNumH = 0;    //時

            //時刻を取り出し
            var valTimeStr = valTimes[i].replace(/:/g, '');   //ｺﾛﾝを除去

            if (isCheckS) {     //秒がﾁｪｯｸ対象の場合
                valTimeNumS = parseInt(valTimeStr.slice(-2), 10);    //数値変換
                valTimeStr = valTimeStr.substr(0, valTimeStr.length - 2);   //残りの文字列
            }
            if (isCheckM) {     //分がﾁｪｯｸ対象の場合
                valTimeNumM = parseInt(valTimeStr.slice(-2), 10);    //数値変換
                valTimeStr = valTimeStr.substr(0, valTimeStr.length - 2);   //残りの文字列
            }
            if (isCheckH) {     //時がﾁｪｯｸ対象の場合
                valTimeNumH = parseInt(valTimeStr.slice(-2), 10);    //数値変換
            }

            //秒変換
            var valTimeNum = valTimeNumH * 3600 + valTimeNumM * 60 + valTimeNumS;

            if (i < 1) {
                valTimeNumFrom = valTimeNum;
            } else {
                valTimeNumTo = valTimeNum;
            }
        }

        //To < From の場合、ﾁｪｯｸｴﾗｰ
        if (valTimeNumTo < valTimeNumFrom) {
            return false;
        }

        return true;   //ﾁｪｯｸOK

    }, null);

    //詳細検索条件シングルコーテーションチェック
    $.validator.addMethod('detailCondSingleQuote', function (value, element, param) {

        //未入力の場合、ﾁｪｯｸをｽﾙｰ
        if (this.optional(element)) {
            return true;
        }

        // シングルコーテーションが含まれていないかどうか
        if (value.indexOf("'") >= 0) {
            return false;
        }
        return true;   //ﾁｪｯｸOK
    }, null);

    //詳細検索条件ダブルコーテーションチェック
    $.validator.addMethod('detailCondDoubleQuote', function (value, element, param) {

        //未入力の場合、ﾁｪｯｸをｽﾙｰ
        if (this.optional(element)) {
            return true;
        }

        // ダブルコーテーションが含まれている場合、対になっているかどうか(偶数かどうか)
        if (value.indexOf('"') >= 0) {
            const cnt = (value.match(/"/g) || []).length;   // ダブルコーテーションの数
            if (cnt % 2 !== 0) {
                return false;
            }
        }
        return true;   //ﾁｪｯｸOK
    }, null);

    //詳細検索条件括弧チェック
    $.validator.addMethod('detailCondParentheses', function (value, element, param) {

        //未入力の場合、ﾁｪｯｸをｽﾙｰ
        if (this.optional(element)) {
            return true;
        }

        // 括弧が含まれる場合、対になっているかどうか、入れ子になっていないか
        if (value.indexOf('(') >= 0) {
            if (value.indexOf(')') < 0) {
                // 左括弧しかない
                return false;
            }
            const p1 = /\(/g;
            const p2 = /\)/g;
            const p3 = /"/g;
            var result;
            var idxesLeft = [];
            var idxesRight = [];
            var idxesDq = [];
            while (result = p1.exec(value)) {
                idxesLeft.push(result.index);   // 左括弧の出現位置
            }
            while (result = p2.exec(value)) {
                idxesRight.push(result.index);  // 右括弧の出現位置
            }
            while (result = p3.exec(value)) {
                idxesDq.push(result.index);     // ダブルコーテーションの出現位置
            }
            if (idxesDq.length > 0) {
                // ダブルコーテーションが含まれる場合、ダブルコーテーション内の括弧はチェックから外す
                for (let i = 0; i < idxesDq.length; i += 2) {
                    for (let j = idxesLeft.length - 1; j >= 0; j--) {
                        if (idxesDq[i] < idxesLeft[j] && idxesDq[i + 1] > idxesLeft[j]) {
                            idxesLeft.splice(j, 1);
                        }
                    }
                    for (let j = idxesRight.length - 1; j >= 0; j--) {
                        if (idxesDq[i] < idxesRight[j] && idxesDq[i + 1] > idxesRight[j]) {
                            idxesRight.splice(j, 1);
                        }
                    }
                }
            }
            if (idxesLeft.length != idxesRight.length) {
                // 括弧が対になっていない
                return false;
            }
            for (let i = 0; i < idxesLeft.length; i++) {
                if (idxesLeft[i] > idxesRight[i]) {
                    // 括弧が正しく閉じていない
                    return false;
                } else if (i > 0 && idxesLeft[i] < idxesRight[i - 1]) {
                    // 括弧が入れ子になっている
                    return false;
                }
            }
        } else if (value.indexOf(')') >= 0) {
            // 右括弧しかない
            return false;
        }
        return true;   //ﾁｪｯｸOK
    }, null);

    // ツリーの必須チェック
    $.validator.addMethod('requiredTree', function (value, element, param) {
        // 追加した空のinputに対して入力チェックが働く
        // 値はこの親に紐づくラベルなので、さかのぼって要素を取得する
        var value = getTreeLabeltByChecker(element).data("structureid"); // 構成ID
        if (!value) {
            // 値が空の場合、エラー
            return false;
        }
        // 正常終了
        return true;
    }, null);

    // オートコンプリート選択値の妥当性チェック
    $.validator.addMethod('comAutoComplete', function (value, element, param) {
        if (!param) {
            // チェック無の場合は終了
            return true;
        }
        if (!value) {
            // 値が空の場合は終了
            return true;
        }
        // オートコンプリート取得
        var div = $(element).data('autocompdiv');
        if (div != autocompDivDef.NameOnly) {
            // 翻訳がセットされているなら、正しい候補が入力されたとする
            var honyaku = $(element).parent().find('.honyaku').text();
            if (!honyaku) {
                // 値が空の場合、エラー
                return false;
            }
        } else {
            // hiddenのコードがセットされているなら、正しい候補が入力されたとする
            var code = $(element).siblings('.autocomp_code').val();
            if (!code) {
                // 値が空の場合、エラー
                return false;
            }
        }
        // 正常終了
        return true;

    }, null);

    //無効なキーワードが含まれていないかどうか
    $.validator.addMethod('comInvalidKeyword', function (value, element, param) {
        var result = true;

        //未入力の場合、ﾁｪｯｸをｽﾙｰ
        if (this.optional(element)) {
            return true;
        }

        $.each(P_InvalidKeywords, function (i, keyword) {
            if (value.indexOf(keyword) >= 0) {
                // 無効なキーワードが含まれている場合、エラー
                result = false;
                return false;   // break
            }
        });
        return result;
    }, null);
}

/**
 * ツリー必須チェック用処理
 * 必須チェック用に追加したinput要素からラベルの要素を取得する
 * @param {any} checker ツリー必須チェック用に追加したinput要素
 */
function getTreeLabeltByChecker(checker) {
    return $(checker).closest(".tree_select_label").siblings("[data-type='treeLabel']");
}

/**
 *  Validatorの初期化処理
 *  @param {string} ：対象セレクタ
 */
function initValidator(selector) {
    // 指定セレクタのValidatorのルールとメッセージを設定
    var myRules = {};
    var myMessages = {};

    var selectorElm = $(P_Article).find(selector);
    var selectorW = $(P_Article).find(selector + " tbody tr:not([class^='base_tr'])");

    // テキストボックスのルールとメッセージを生成
    //$(selectorW).find(":text").each(function (index, element) {
    $(selectorW).find("input[type='text']").each(function (index, element) {
        var target = $(this);
        var colName = $(this).data("colname");
        //必須項目の場合は必須ﾁｪｯｸ実行
        var required = $(this).hasClass("validate_required");

        if ($(this).data("type") == "text") {
            //1:ﾃｷｽﾄ
            var maxlen = -1;
            if ($(this).data("maxlength") > 0) {
                maxlen = $(this).data("maxlength");
            }

            myRules[element.name] = {
                required: [required, colName],  //必須ﾁｪｯｸ
                comMaxByteLength: maxlen,       //桁数ﾁｪｯｸ(ﾊﾞｲﾄ数)
            };

        }
        else if ($(this).data("type") == "num") {
            //7:数値
            myRules[element.name] = {
                required: [required, colName],          //必須ﾁｪｯｸ
                comMaxByteLength: -1,                   //桁数ﾁｪｯｸ(ﾊﾞｲﾄ数)～tmptbldataを超えるﾊﾞｲﾄ数は設定できない
                number: true,                           //型ﾁｪｯｸ：数値
                comNumRange: [$(this).data("min"), $(this).data("max")],   //範囲ﾁｪｯｸ
                comNumFromTo: true, // 大小ﾁｪｯｸ
            };

        }
        else if ($(this).data("type") == "date") {
            //93:日付
            myRules[element.name] = {
                required: [required, colName],          //必須ﾁｪｯｸ
                comMaxByteLength: -1,                   //桁数ﾁｪｯｸ(ﾊﾞｲﾄ数)～tmptbldataを超えるﾊﾞｲﾄ数は設定できない
                comDate: true,                           //型ﾁｪｯｸ：日付
                //date: true                
                comDateFromTo: true                     //大小ﾁｪｯｸ
            };
        }
        else if ($(this).data("type") == "time") {
            //94:時刻
            myRules[element.name] = {
                required: [required, colName],          //必須ﾁｪｯｸ
                //number: true,                           //型ﾁｪｯｸ：数値
                comMaxByteLength: -1,                   //桁数ﾁｪｯｸ(ﾊﾞｲﾄ数)
                comTimeRange: true,                     //時刻範囲ﾁｪｯｸ
                comTimeFromTo: true                     //大小ﾁｪｯｸ
            };
        }
        else if ($(this).data("type") == "datetime") {
            //95:日時
            myRules[element.name] = {
                required: [required, colName],          //必須ﾁｪｯｸ
                //number: true,                           //型ﾁｪｯｸ：数値
                comMaxByteLength: -1,                   //桁数ﾁｪｯｸ(ﾊﾞｲﾄ数)
                //comDateTime: true,                      //型ﾁｪｯｸ：日時
                comDateTimeFromTo: true                 //大小ﾁｪｯｸ
            };
        }
        else if ($(this).data("type") == "yearmonth") {
            // 年月
            myRules[element.name] = {
                required: [required, colName],          //必須ﾁｪｯｸ
                comMaxByteLength: -1,                   //桁数ﾁｪｯｸ(ﾊﾞｲﾄ数)～tmptbldataを超えるﾊﾞｲﾄ数は設定できない
                comDate: true,                           //型ﾁｪｯｸ：日付            
                comDateFromTo: true                     //大小ﾁｪｯｸ
            };
        }
        else if ($(this).data("type") == "codeTrans") {
            //91:コード＋翻訳
            var maxlen = -1;
            if ($(this).data("maxlength") > 0) {
                maxlen = $(this).data("maxlength");
            }
            // オートコンプリートの場合、候補値の入力チェックを行う
            var isAutoComplete = false;
            if ($(this).data("autocompdiv") != autocompDivDef.None) {
                isAutoComplete = true;
            }

            myRules[element.name] = {
                required: [required, colName],  //必須ﾁｪｯｸ
                comMaxByteLength: maxlen,       //桁数ﾁｪｯｸ(ﾊﾞｲﾄ数)
                //number: true,                           //型ﾁｪｯｸ：数値 --拡張項目の値で行う場合もある(英数字)ため、コメントアウト
                comAutoComplete: isAutoComplete, // オートコンプリート値候補チェック
            };
            // 非表示の場合でも入力チェックを行わせるために、識別用に「hide_valid」を指定する
            if (isHide($(this)) || $(this).css('display') == 'none') {
                $(this).addClass('hide_valid');
            }
        }
        else {
            myRules[element.name] = {
                required: [required, colName],
                comMaxByteLength: -1,                  //桁数ﾁｪｯｸ(ﾊﾞｲﾄ数)～tmptbldataを超えるﾊﾞｲﾄ数は設定できない
            };
        }
        //必須項目でない場合は必須ﾁｪｯｸ解除
        if (!required) { myRules[element.name].required = false; }

        myMessages[element.name] = {
            //★AKAP2.0標準
            //エラーメッセージをツールチップに表示
            required: P_ComMsgTranslated[941220009], //入力して下さい。
            number: P_ComMsgTranslated[941130004], //数値で入力してください。
            comDate: P_ComMsgTranslated[941270003], //日付で入力して下さい。
            comTimeRange: P_ComMsgTranslated[941120015], //時刻で入力してください。
            comMaxByteLength: P_ComMsgTranslated[941060018], //{0}文字以内で入力して下さい。
            comNumRange: P_ComMsgTranslated[941060015], //{0}から{1}の範囲で入力して下さい。
            comTimeRange: P_ComMsgTranslated[941060015], //{0}から{1}の範囲で入力して下さい。
            comDateFromTo: P_ComMsgTranslated[941270004], //日付の大小関係に問題があります。
            comTimeFromTo: P_ComMsgTranslated[941120016], //時刻の大小関係に問題があります。
            comDateTimeFromTo: P_ComMsgTranslated[941220010], //日時の大小関係に問題があります。
            comNumFromTo: P_ComMsgTranslated[941130005], //数値の大小関係に問題があります。
            comAutoComplete: P_ComMsgTranslated[941160006], //正しい候補の値を入力してください。
            //required: null,
            //maxlength: null,
            //range: null,
            //date: null,
            //number: null,
            //comDate: null,
            //comTimeRange: null,
            //comMaxByteLength: null,
            //comNumRange: null,
            //comTimeRange: null,
            //★AKAP2.0標準
        };
    });

    // 6:日付(ブラウザ標準)
    $(selectorW).find("input[type='date']").each(function (index, element) {
        var target = $(this);
        var colName = $(this).data("colname");
        myRules[element.name] = {
            required: [true, colName],          //必須ﾁｪｯｸ
            comMaxByteLength: -1,                   //桁数ﾁｪｯｸ(ﾊﾞｲﾄ数)～tmptbldataを超えるﾊﾞｲﾄ数は設定できない
            //★必要?　comDate: true                           //型ﾁｪｯｸ：日付
            //date: true                
            comDateFromTo: true                     //大小ﾁｪｯｸ
        };
        //必須項目でない場合は必須ﾁｪｯｸ解除
        if (!$(this).hasClass("validate_required")) { myRules[element.name].required = false; }

        myMessages[element.name] = {
            //★AKAP2.0標準
            //エラーメッセージをツールチップに表示
            required: P_ComMsgTranslated[941220009], //入力して下さい。
            number: P_ComMsgTranslated[941130004], //数値で入力してください。
            comDate: P_ComMsgTranslated[941270003], //日付で入力して下さい。
            comTimeRange: P_ComMsgTranslated[941120015], //時刻で入力してください。
            comMaxByteLength: P_ComMsgTranslated[941060018], //{0}文字以内で入力して下さい。
            comNumRange: P_ComMsgTranslated[941060015], //{0}から{1}の範囲で入力して下さい。
            comTimeRange: P_ComMsgTranslated[941060015], //{0}から{1}の範囲で入力して下さい。
            comDateFromTo: P_ComMsgTranslated[941270004], //日付の大小関係に問題があります。
            comTimeFromTo: P_ComMsgTranslated[941120016], //時刻の大小関係に問題があります。
            comDateTimeFromTo: P_ComMsgTranslated[941220010], //日時の大小関係に問題があります。

            //required: null,
            //maxlength: null,
            //range: null,
            //date: null,
            //number: null,
            //comDate: null,
            //comTimeRange: null,
            //comMaxByteLength: null,
            //comNumRange: null,
            //comTimeRange: null,
            //★AKAP2.0標準
        };
    });

    // 61:時刻(ブラウザ標準)
    $(selectorW).find("input[type='time']").each(function (index, element) {
        var target = $(this);
        var colName = $(this).data("colname");
        myRules[element.name] = {
            required: [true, colName],          //必須ﾁｪｯｸ
            comMaxByteLength: -1,                   //桁数ﾁｪｯｸ(ﾊﾞｲﾄ数)
            //★必要?　comTimeRange: true,                     //時刻範囲ﾁｪｯｸ
            comTimeFromTo: true                     //大小ﾁｪｯｸ
        };
        //必須項目でない場合は必須ﾁｪｯｸ解除
        if (!$(this).hasClass("validate_required")) { myRules[element.name].required = false; }

        myMessages[element.name] = {
            //★AKAP2.0標準
            //エラーメッセージをツールチップに表示
            required: P_ComMsgTranslated[941220009], //入力して下さい。
            number: P_ComMsgTranslated[941130004], //数値で入力してください。
            comDate: P_ComMsgTranslated[941270003], //日付で入力して下さい。
            comTimeRange: P_ComMsgTranslated[941120015], //時刻で入力してください。
            comMaxByteLength: P_ComMsgTranslated[941060018], //{0}文字以内で入力して下さい。
            comNumRange: P_ComMsgTranslated[941060015], //{0}から{1}の範囲で入力して下さい。
            comTimeRange: P_ComMsgTranslated[941060015], //{0}から{1}の範囲で入力して下さい。
            comDateFromTo: P_ComMsgTranslated[941270004], //日付の大小関係に問題があります。
            comTimeFromTo: P_ComMsgTranslated[941120016], //時刻の大小関係に問題があります。
            comDateTimeFromTo: P_ComMsgTranslated[941220010], //日時の大小関係に問題があります。

            //required: null,
            //maxlength: null,
            //range: null,
            //date: null,
            //number: null,
            //comDate: null,
            //comTimeRange: null,
            //comMaxByteLength: null,
            //comNumRange: null,
            //comTimeRange: null,
            //★AKAP2.0標準
        };
    });

    // 62:日時(ブラウザ標準)
    $(selectorW).find("input[type='datetime-local']").each(function (index, element) {
        var target = $(this);
        var colName = $(this).data("colname");
        myRules[element.name] = {
            required: [true, colName],          //必須ﾁｪｯｸ
            comMaxByteLength: -1,                   //桁数ﾁｪｯｸ(ﾊﾞｲﾄ数)
            //★必要?comDateTime: true,                      //型ﾁｪｯｸ：日時
            comDateTimeFromTo: true                 //大小ﾁｪｯｸ
        };
        //必須項目でない場合は必須ﾁｪｯｸ解除
        if (!$(this).hasClass("validate_required")) { myRules[element.name].required = false; }

        myMessages[element.name] = {
            //★AKAP2.0標準
            //エラーメッセージをツールチップに表示
            required: P_ComMsgTranslated[941220009], //入力して下さい。
            number: P_ComMsgTranslated[941130004], //数値で入力してください。
            comDate: P_ComMsgTranslated[941270003], //日付で入力して下さい。
            comTimeRange: P_ComMsgTranslated[941120015], //時刻で入力してください。
            comMaxByteLength: P_ComMsgTranslated[941060018], //{0}文字以内で入力して下さい。
            comNumRange: P_ComMsgTranslated[941060015], //{0}から{1}の範囲で入力して下さい。
            comTimeRange: P_ComMsgTranslated[941060015], //{0}から{1}の範囲で入力して下さい。
            comDateFromTo: P_ComMsgTranslated[941270004], //日付の大小関係に問題があります。
            comTimeFromTo: P_ComMsgTranslated[941120016], //時刻の大小関係に問題があります。
            comDateTimeFromTo: P_ComMsgTranslated[941220010], //日時の大小関係に問題があります。

            //required: null,
            //maxlength: null,
            //range: null,
            //date: null,
            //number: null,
            //comDate: null,
            //comTimeRange: null,
            //comMaxByteLength: null,
            //comNumRange: null,
            //comTimeRange: null,
            //★AKAP2.0標準
        };
    });

    // コンボボックスのルールとメッセージを生成
    $(selectorW).find("select").each(function (index, element) {
        //5:ｺﾝﾎﾞﾎﾞｯｸｽ
        if ($(this).hasClass("validate_required")) {    //必須ﾁｪｯｸ
            myRules[element.name] = { required: [true, $(this).data("colname")] };
            //myMessages[element.name] = { required: '{1}を選択して下さい。' }
            myMessages[element.name] = {
                required: P_ComMsgTranslated[941140002], //選択して下さい。
            }
            //myMessages[element.name] = { required: null }
        }
    });

    // テキストエリアのルールとメッセージを生成
    $(selectorW).find("textarea").each(function (index, element) {
        var target = $(this);
        var colName = $(this).data("colname");
        if ($(this).hasClass("validate_required")) {    //必須ﾁｪｯｸ
            myRules[element.name] = { required: [true, colName] };
            myMessages[element.name] = {
                required: P_ComMsgTranslated[941220009], //入力して下さい。
            }
        }
    });

    // パスワードのルールとメッセージを生成
    $(selectorW).find("input[type='password']").each(function (index, element) {
        var target = $(this);
        var colName = $(this).data("colname");
        if ($(this).hasClass("validate_required")) { // 必須ﾁｪｯｸ
            var maxlen = -1;
            if ($(this).data("maxlength") > 0) {
                maxlen = $(this).data("maxlength");
            }
            myRules[element.name] = {
                required: [true, colName],      //必須ﾁｪｯｸ
                comMaxByteLength: maxlen,       //桁数ﾁｪｯｸ(ﾊﾞｲﾄ数)
            };
            myMessages[element.name] = {
                //エラーメッセージをツールチップに表示
                required: P_ComMsgTranslated[941220009], //入力して下さい。
                comMaxByteLength: P_ComMsgTranslated[941060018], //{0}文字以内で入力して下さい。
            };
        }
    });

    // 複数選択ﾘｽﾄのルールとメッセージを生成
    $(selectorW).find("ul.multiSelect").each(function (index, element) {
        //9:複数選択ﾘｽﾄ
        if ($(this).hasClass("validate_required")) {    //必須ﾁｪｯｸ
            var name = $(this).attr('name') + "[]";
            myRules[name] = {
                required: [true, $(this).data("colname")],
            };
            myMessages[name] = {
                required: null
            }
        }
    });

    // ツリーのルールとメッセージを生成
    // ツリーの要素に対して、Validateのトリガー用のinputを追加 ※Validateはラベルに対して行えないため
    $(selectorW).find(".tree_select_label.required").each(function (i, treeLink) {
        // 要素が参照モードなら処理を行わない
        if ($(this).closest(".ctrlId").data("referencemode") == 1) {
            return true; // continue
        }

        // 既に要素が追加済みなら処理を行わない
        if ($(this).find("input[type='tree']").length == 0) {
            // Validateを行う項目は、一意なnameが必要なので付与
            // テーブル名のValNoで一意
            var tblId = $(this).closest("table").attr("id");
            var valNo = $(this).data("name");
            var name = tblId + "_" + valNo;
            // 要素を追加　非表示(hide)だが入力チェックは行うので、識別用にhide_validを指定する
            $(this).append("<input type='tree' id='" + name + "' name='" + name + "' class='hide hide_valid'>");
        }
    });
    // 追加した要素に対して、Validatorのルールとメッセージを付与
    // 要素追加時に行うと、複数回呼び出される場合に正常に動作しないため、他の項目のルール追加処理とタイミングを合わせている
    var treeCheck = $(selectorW).find("input[type='tree']");
    $(treeCheck).each(function (i, element) {
        myRules[element.name] = {
            // requiredTreeは作成した独自ルール。この項目に紐づく表示しているツリーの値に対してチェックする
            requiredTree: true
        };
        myMessages[element.name] = {
            requiredTree: P_ComMsgTranslated[941220009], //入力して下さい。
        };
    });

    //Validatorをリセット
    $(selectorElm).removeData("validator");

    // エラー表示でツリー・非表示項目のために分岐が必要になった処理の定義
    /**
     * エラー表示する要素にエラー表示用のスタイルを設定
     * @param {any} elm 設定する要素
     * @param {boolean} isLabel その項目でなく親要素(td)に対しエラー表示する場合はTrue
     */
    var setErrorClass = function (elm, isLabel) {
        var addClassName = !isLabel ? "" : "disp_error"; // ツリーの場合はエラー表示用にクラスを追加
        $(elm).addClass("errorcom " + addClassName);
    }
    /**
     * エラー表示用ツールチップの場所指定CSSを取得
     * @param {any} isTree ツリーの場合True
     * @param {any} td エラー表示する項目、ツリーの場合に使用
     * @param {any} elm エラー発生の要素(input)、ツリー以外の場合に使用
     */
    var getErrorPositionCss = function (isTree, td, elm) {
        var target = !isTree ? elm : td;
        var errorCss = resetErrorPosition(target);
        return errorCss;
    }
    /**
     * エラーチェック用の要素からツリー表示かどうかを判定する処理
     * @param {any} elm 判定する要素
     * @param {any} isInput 判定する要素が入力チェック対象の要素の場合はTrue、エラー表示要素の場合はFalse
     */
    var getIsTree = function (elm, isInput) {
        if (isInput) {
            return $(elm).is("input[type='tree']");
        }
        return $(elm).is("[data-type='treeLabel']");
    }

    /**
     * ツリー項目の必須チェック用エラーを表示するラベル要素からチェック用に追加したinput要素を取得する
     * @param {any} label エラー表示するラベル要素
     */
    var getTreeCheckerByLabel = function (label) {
        return $(label).siblings(".tree_select_label").find("input[type='tree']");
    }
    /**
     * エラーチェック用の要素から非表示で入力チェックを行う項目かどうかを判定する処理
     * @param {any} elm 判定する要素
     * @param {any} isInput 判定する要素が入力チェック対象の要素の場合はTrue、エラー表示要素の場合はFalse
     */
    var getIsHideValid = function (elm, isInput) {
        if (isInput) {
            return isHideValidator(elm);
        }
        return $(elm).has(".hide_valid").length > 0;
    }
    /**
    * 非表示項目の入力チェック用エラーを表示するラベル要素からチェック対象の項目を取得する
    * @param {any} label エラー表示するラベル要素
    */
    var getHideCheckerByLabel = function (label) {
        return $(label).find('.hide_valid');
    }
    /**
     * エラー表示の際に紐づけられた項目のID
     * @param {any} element エラー表示を行う項目
     * @return {string} 紐づけられた入力項目のID
     */
    var getErrorOutputElementId = function (element) {
        // 通常はそのまま項目のID
        var target = $(element);

        var isTree = getIsTree(element, false);
        if (isTree) {
            // ツリー表示の場合、エラー表示の要素とエラーチェックの項目(非表示で追加)が異なるので取得
            target = getTreeCheckerByLabel(element);
        } else {
            var isHide = getIsHideValid(element, false);
            if (isHide) {
                // 非表示項目の場合、エラー表示の要素とエラーチェックの項目(非表示のinput)が異なるので取得
                target = getHideCheckerByLabel(element);
            }
        }
        return $(target).attr("id");
    }

    // 生成したルールとメッセージをValidatorへセット
    $(selectorElm).validate({
        rules: myRules,
        messages: myMessages,
        formEvent: null,
        inputEvent: null,
        onfocusout: false,
        // 非表示項目に対してもチェックを行いたいので、ignoreオプションを指定
        // 内部的には.notの引数で、標準は:hidden。非表示項目でもチェックを行いたい要素にhide_validを指定したので、これを除外する
        ignore: ':hidden:not(.hide_valid)',
        onkeyup: false,
        //errorElement: null,
        //errorElement: "td",
        //★AKAP2.0標準
        errorPlacement: function (error, element) {
            // ツリーの場合は入力チェックの対象となる項目(空のinput)とエラー表示する項目が異なる
            var isTree = getIsTree(element, true);
            // エラー情報をツールチップに配置
            // 通常は自身にツールチップを表示するが、ツリーの場合はラベルにエラーを表示する
            var td = !isTree ? $(element).closest('td') : getTreeLabeltByChecker(element);
            // 非表示項目のエラーチェックを行った場合もラベルにエラーを表示する
            var isHide = getIsHideValid(element, true);
            setErrorClass(td, isTree || isHide); // エラー表示用のスタイルを設定

            var errTooltipDiv = $("div.errtooltip");

            // コンボボックスの必須チェックが選択する度に増えるので、すでに存在する場合はスキップ
            if ($(errTooltipDiv).find("label[for='" + $(element).attr("id") + "']").length > 0) {
                return;
            }

            var errorCss = getErrorPositionCss(isTree, td, element);
            //$(error).insertAfter(element);
            $(error).appendTo(errTooltipDiv);
            var errorHtml = $(td).find("label[for='" + $(element).attr("id") + "']");
            $(errorHtml).css(errorCss);

            // カーソルを当てたときのツールチップ表示処理を設定
            // ツリーや非表示項目の場合は入力要素でなく、値を表示しているラベル要素
            $(!isTree && !isHide ? element : td).hover(
                function () {
                    // カーソルを当てたとき、ツールチップを表示する
                    // ツールチップで指定された、エラーの発生した項目ID
                    var tooltipId = getErrorOutputElementId(this);
                    var errorCss = resetErrorPosition(this);
                    $("label[for='" + tooltipId + "']").css(errorCss);
                    $("#main_contents").on('scroll.errTooltip', function () {
                        $("label.errorcom").fadeOut('fast');
                        $("#main_contents").off('scroll.errTooltip');
                    });
                },
                function () {
                    // カーソルを外したとき、ツールチップを非表示にする
                    // ツールチップで指定された、エラーの発生した項目ID
                    var tooltipId = getErrorOutputElementId(this);
                    $("label[for='" + tooltipId + "']").fadeOut('fast');
                    $("#main_contents").off('scroll.errTooltip');
                });
        },
        highlight: function (element) {
            var isTree = $(element).is("input[type='tree']");
            //エラー情報をツールチップに配置
            var td = !isTree ? $(element).closest('td') : getTreeLabeltByChecker(element);
            setErrorClass(td, isTree);

            var errorCss = getErrorPositionCss(isTree, td, element);
            var errorHtml = $("label[for='" + $(element).attr("id") + "']");
            $(errorHtml).css(errorCss);
            $(element).addClass('errorcom');
        },
        //★AKAP2.0標準
        errorClass: "errorcom"

    });
}

/**
 * 非表示項目でも入力チェックを行うかどうか判定する処理
 * @param {any} elm チェックする要素
 * @return {bool} 入力チェックを行う場合True
 */
function isHideValidator(elm) {
    // hide_validを持つ場合はValidatorで入力チェックを子なう
    var result = $(elm).hasClass('hide_valid');
    return result;
}

/**
 *  詳細検索条件のValidatorの初期化処理
 */
function initValidatorForDetailSearch() {
    // 詳細検索条件のValidatorのルールとメッセージを設定
    var myRules = {};
    var myMessages = {};

    var selectorElm = $('nav#detail_search form').has('input');
    var selectorW = $(selectorElm).find("tr");

    // テキストボックスのルールとメッセージを生成
    $(selectorW).find("input[type='text']").each(function (index, element) {
        if ($(this).data("type") == "text") {
            //1:ﾃｷｽﾄ
            myRules[element.name] = {
                comMaxByteLength: 255,          //桁数ﾁｪｯｸ(ﾊﾞｲﾄ数)
                detailCondSingleQuote: true,    // シングルコーテーションチェック
                detailCondDoubleQuote: true,    // ダブルコーテーションチェック
                detailCondParentheses: true,    // 括弧チェック
            };

        }
        else if ($(this).data("type") == "num") {
            //7:数値
            myRules[element.name] = {
                comMaxByteLength: -1,                   //桁数ﾁｪｯｸ(ﾊﾞｲﾄ数)～tmptbldataを超えるﾊﾞｲﾄ数は設定できない
                number: true,                           //型ﾁｪｯｸ：数値
                comNumRange: [$(this).data("min"), $(this).data("max")],   //範囲ﾁｪｯｸ
                comNumFromTo: true, // 大小ﾁｪｯｸ
            };

        }
        else if ($(this).data("type") == "date") {
            //93:日付
            myRules[element.name] = {
                comMaxByteLength: -1,                   //桁数ﾁｪｯｸ(ﾊﾞｲﾄ数)～tmptbldataを超えるﾊﾞｲﾄ数は設定できない
                comDate: true,                           //型ﾁｪｯｸ：日付
                comDateFromTo: true                     //大小ﾁｪｯｸ
            };
        }
        else if ($(this).data("type") == "time") {
            //94:時刻
            myRules[element.name] = {
                comMaxByteLength: -1,                   //桁数ﾁｪｯｸ(ﾊﾞｲﾄ数)
                comTimeRange: true,                     //時刻範囲ﾁｪｯｸ
                comTimeFromTo: true                     //大小ﾁｪｯｸ
            };
        }
        else if ($(this).data("type") == "datetime") {
            //95:日時
            myRules[element.name] = {
                comMaxByteLength: -1,                   //桁数ﾁｪｯｸ(ﾊﾞｲﾄ数)
                comDateTimeFromTo: true                 //大小ﾁｪｯｸ
            };
        }

        myMessages[element.name] = {
            //エラーメッセージをツールチップに表示
            required: P_ComMsgTranslated[941220009], //入力して下さい。
            number: P_ComMsgTranslated[941130004], //数値で入力してください。
            comDate: P_ComMsgTranslated[941270003], //日付で入力して下さい。
            comTimeRange: P_ComMsgTranslated[941120015], //時刻で入力してください。
            comMaxByteLength: P_ComMsgTranslated[941060018], //{0}文字以内で入力して下さい。
            comNumRange: P_ComMsgTranslated[941060015], //{0}から{1}の範囲で入力して下さい。
            comNumFromTo: P_ComMsgTranslated[941130005], //数値の大小関係に問題があります。
            comTimeRange: P_ComMsgTranslated[941060015], //{0}から{1}の範囲で入力して下さい。
            comDateFromTo: P_ComMsgTranslated[941270004], //日付の大小関係に問題があります。
            comTimeFromTo: P_ComMsgTranslated[941120016], //時刻の大小関係に問題があります。
            comDateTimeFromTo: P_ComMsgTranslated[941220010], //日時の大小関係に問題があります。
            detailCondSingleQuote: P_ComMsgTranslated[941120017], //シングルクォーテーション「'」が含まれています。
            detailCondDoubleQuote: P_ComMsgTranslated[941160007], //ダブルクォーテーション「"」が正しく閉じていません。
            detailCondParentheses: P_ComMsgTranslated[941060019], //括弧「( )」が正しく閉じていない、または入れ子になっています。
        };
    });

    // 6:日付(ブラウザ標準)
    $(selectorW).find("input[type='date']").each(function (index, element) {
        myRules[element.name] = {
            comMaxByteLength: -1,                   //桁数ﾁｪｯｸ(ﾊﾞｲﾄ数)～tmptbldataを超えるﾊﾞｲﾄ数は設定できない
            comDateFromTo: true                     //大小ﾁｪｯｸ
        };

        myMessages[element.name] = {
            //エラーメッセージをツールチップに表示
            required: P_ComMsgTranslated[941220009], //入力して下さい。
            number: P_ComMsgTranslated[941130004], //数値で入力してください。
            comDate: P_ComMsgTranslated[941270003], //日付で入力して下さい。
            comTimeRange: P_ComMsgTranslated[941120015], //時刻で入力してください。
            comMaxByteLength: P_ComMsgTranslated[941060018], //{0}文字以内で入力して下さい。
            comNumRange: P_ComMsgTranslated[941060015], //{0}から{1}の範囲で入力して下さい。
            comNumFromTo: P_ComMsgTranslated[941130005], //数値の大小関係に問題があります。
            comTimeRange: P_ComMsgTranslated[941060015], //{0}から{1}の範囲で入力して下さい。
            comDateFromTo: P_ComMsgTranslated[941270004], //日付の大小関係に問題があります。
            comTimeFromTo: P_ComMsgTranslated[941120016], //時刻の大小関係に問題があります。
            comDateTimeFromTo: P_ComMsgTranslated[941220010], //日時の大小関係に問題があります。
            detailCondSingleQuote: P_ComMsgTranslated[941120017], //シングルクォーテーション「'」が含まれています。
            detailCondDoubleQuote: P_ComMsgTranslated[941160007], //ダブルクォーテーション「"」が正しく閉じていません。
            detailCondParentheses: P_ComMsgTranslated[941060019], //括弧「( )」が正しく閉じていない、または入れ子になっています。
        };
    });

    // 61:時刻(ブラウザ標準)
    $(selectorW).find("input[type='time']").each(function (index, element) {
        myRules[element.name] = {
            comMaxByteLength: -1,                   //桁数ﾁｪｯｸ(ﾊﾞｲﾄ数)
            comTimeFromTo: true                     //大小ﾁｪｯｸ
        };

        myMessages[element.name] = {
            //エラーメッセージをツールチップに表示
            required: P_ComMsgTranslated[941220009], //入力して下さい。
            number: P_ComMsgTranslated[941130004], //数値で入力してください。
            comDate: P_ComMsgTranslated[941270003], //日付で入力して下さい。
            comTimeRange: P_ComMsgTranslated[941120015], //時刻で入力してください。
            comMaxByteLength: P_ComMsgTranslated[941060018], //{0}文字以内で入力して下さい。
            comNumRange: P_ComMsgTranslated[941060015], //{0}から{1}の範囲で入力して下さい。
            comNumFromTo: P_ComMsgTranslated[941130005], //数値の大小関係に問題があります。
            comTimeRange: P_ComMsgTranslated[941060015], //{0}から{1}の範囲で入力して下さい。
            comDateFromTo: P_ComMsgTranslated[941270004], //日付の大小関係に問題があります。
            comTimeFromTo: P_ComMsgTranslated[941120016], //時刻の大小関係に問題があります。
            comDateTimeFromTo: P_ComMsgTranslated[941220010], //日時の大小関係に問題があります。
            detailCondSingleQuote: P_ComMsgTranslated[941120017], //シングルクォーテーション「'」が含まれています。
            detailCondDoubleQuote: P_ComMsgTranslated[941160007], //ダブルクォーテーション「"」が正しく閉じていません。
            detailCondParentheses: P_ComMsgTranslated[941060019], //括弧「( )」が正しく閉じていない、または入れ子になっています。
        };
    });

    // 62:日時(ブラウザ標準)
    $(selectorW).find("input[type='datetime-local']").each(function (index, element) {
        var colName = $(this).data("colname");
        myRules[element.name] = {
            comMaxByteLength: -1,                   //桁数ﾁｪｯｸ(ﾊﾞｲﾄ数)
            comDateTimeFromTo: true                 //大小ﾁｪｯｸ
        };

        myMessages[element.name] = {
            //エラーメッセージをツールチップに表示
            required: P_ComMsgTranslated[941220009], //入力して下さい。
            number: P_ComMsgTranslated[941130004], //数値で入力してください。
            comDate: P_ComMsgTranslated[941270003], //日付で入力して下さい。
            comTimeRange: P_ComMsgTranslated[941120015], //時刻で入力してください。
            comMaxByteLength: P_ComMsgTranslated[941060018], //{0}文字以内で入力して下さい。
            comNumRange: P_ComMsgTranslated[941060015], //{0}から{1}の範囲で入力して下さい。
            comNumFromTo: P_ComMsgTranslated[941130005], //数値の大小関係に問題があります。
            comTimeRange: P_ComMsgTranslated[941060015], //{0}から{1}の範囲で入力して下さい。
            comDateFromTo: P_ComMsgTranslated[941270004], //日付の大小関係に問題があります。
            comTimeFromTo: P_ComMsgTranslated[941120016], //時刻の大小関係に問題があります。
            comDateTimeFromTo: P_ComMsgTranslated[941220010], //日時の大小関係に問題があります。
            detailCondSingleQuote: P_ComMsgTranslated[941120017], //シングルクォーテーション「'」が含まれています。
            detailCondDoubleQuote: P_ComMsgTranslated[941160007], //ダブルクォーテーション「"」が正しく閉じていません。
            detailCondParentheses: P_ComMsgTranslated[941060019], //括弧「( )」が正しく閉じていない、または入れ子になっています。
        };
    });

    //Validatorをリセット
    $(selectorElm).removeData("validator");

    // 生成したルールとメッセージをValidatorへセット
    $(selectorElm).validate({
        rules: myRules,
        messages: myMessages,
        formEvent: null,
        inputEvent: null,
        onfocusout: false,
        onkeyup: false,
        //errorElement: null,
        //errorElement: "td",
        errorPlacement: function (error, element) {
            //エラー情報をツールチップに配置
            var td = $(element).closest('td');
            $(td).addClass("errorcom");

            var errTooltipDiv = $("div.errtooltip");

            // コンボボックスの必須チェックが選択する度に増えるので、すでに存在する場合はスキップ
            if ($(errTooltipDiv).find("label[for='" + $(element).attr("id") + "']").length > 0) {
                return;
            }

            var errorCss = resetErrorPosition(element);
            //$(error).insertAfter(element);
            $(error).appendTo(errTooltipDiv);
            var errorHtml = $(td).find("label[for='" + $(element).attr("id") + "']");
            $(errorHtml).css(errorCss);

            $(element).hover(
                function () {
                    var errorCss = resetErrorPosition(this);
                    $("label[for='" + $(this).attr("id") + "']").css(errorCss);
                    $("#detail_search").on('scroll.errTooltip', function () {
                        $("label.errorcom").fadeOut('fast');
                        $("#detail_search").off('scroll.errTooltip');
                    });
                },
                function () {
                    $("label[for='" + $(this).attr("id") + "']").fadeOut('fast');
                    $("#detail_search").off('scroll.errTooltip');
                });
        },
        highlight: function (element) {

            var td = $(element).closest('td');
            $(td).addClass("errorcom");
            var errorCss = resetErrorPosition(element);
            var errorHtml = $("label[for='" + $(element).attr("id") + "']");
            $(errorHtml).css(errorCss);
            $(element).addClass('errorcom');
        },
        errorClass: "errorcom"

    });

}

/**
 * 最大のz-indexの値を取得
 * */
function getMaxZindex() {
    // モーダル画面の場合、後ろに表示されてしまうため動的に算出
    const n = $('.modal:visible').length;
    var zIndex = 1040 + (10 * n) + 1;
    return zIndex;
}

/**
 * ｴﾗｰ詳細の表示位置再設定
 */
function resetErrorPosition(element) {

    var displayType = "block";

    var offset = $(element).offset();

    var left = offset.left;
    left = left + "px";

    var elOuterHeight = $(element).outerHeight(true);
    var top = elOuterHeight + offset.top;
    top = top + "px";

    var zIndex = getMaxZindex();

    // ｴﾗｰﾒｯｾｰｼﾞ要素のｽﾀｲﾙ
    var errorCss = {
        "position": "fixed",
        "display": displayType,
        "left": left,
        "top": top,
        "z-index": zIndex
    }
    return errorCss;
}

/**
 *  Validatorの初期化処理(パスワード変更フォーム用)
 */
function initValidatorForPassChange() {

    // 指定セレクタのValidatorのルールとメッセージを設定
    var myRules = {};
    var myMessages = {};

    var selectorElm = $("#passwordChange_detail_div");
    var selectorW = $(selectorElm).find("input:password.validate_required");

    // パスワード
    $(selectorW).each(function (index, element) {
        var target = $(this);
        var colName = $(this).data("colname");
        var minlen = -1;
        if ($(this).data("minlength") > 0) {
            minlen = $(this).data("minlength");
        }
        myRules[element.name] = {
            required: [true, colName],              //必須ﾁｪｯｸ
            passwordByteLengthRange: minlen,        //桁数ﾁｪｯｸ(ﾊﾞｲﾄ数)
            newPasswordConfirm: false,              //一致ﾁｪｯｸ
        };

        //必須項目でない場合は必須ﾁｪｯｸ解除
        if (!$(this).hasClass("validate_required")) { myRules[element.name].required = false; }

        // 新しいパスワード(確認)の場合、一致チェック
        if ($(this).attr("name") == "NewLoginPasswordConfirm_text") {
            myRules[element.name].newPasswordConfirm = [true, minlen];
        }

        myMessages[element.name] = {
            //★AKAP2.0標準
            ////エラーメッセージをツールチップに表示
            required: P_ComMsgTranslated[941220009], //入力して下さい。
            passwordByteLengthRange: P_ComMsgTranslated[941060020], //{0}文字以上128文字以下で入力して下さい。
            newPasswordConfirm: P_ComMsgTranslated[941010005], //新しいパスワードと同じ値を入力してください。
            //required: null,
            //maxlength: null,
            //range: null,
            //date: null,
            //number: null,
            //comDate: null,
            //comTimeRange: null,
            //comMaxByteLength: null,
            //comNumRange: null,
            //comTimeRange: null,
            //★AKAP2.0標準
        };
    });

    //Validatorをリセット
    $(selectorElm).removeData("validator");

    // 生成したルールとメッセージをValidatorへセット
    $(selectorElm).validate({
        rules: myRules,
        messages: myMessages,
        formEvent: null,
        inputEvent: null,
        onfocusout: false,
        onkeyup: false,
        //errorElement: null,
        errorElement: "label",
        //★AKAP2.0標準
        errorPlacement: function (error, element) {
            //エラー情報をツールチップに配置
            //var div = $(element).closest('div');
            $(error).insertAfter(element);
            $(element).addClass('errorcom');

            $(error).hide();
            $(element).hover(
                function () {
                    $(error).css("display", "inline-block");
                },
                function () {
                    $(error).fadeOut('fast');
                });
        },
        //★AKAP2.0標準
        errorClass: "errorcom"

    });
}

/**
 *  入力ﾌｫｰﾏｯﾄ設定処理
 *  @param {string} ：対象セレクタ
 *  @param {bool} ：Tabulator一覧の場合、true(省略可)
 */
function initFormat(selector, isTabulator) {
    var selectorW;
    if (isTabulator) {
        selectorW = $(P_Article).find(selector);
    } else {
        //画面NOｴﾘｱ配下で絞込
        selectorW = $(P_Article).find(selector + " table tbody tr:not([class^='base_tr'])");
    }


    $(selectorW).find(":text").each(function (index, element) {

        if ($(this).data("type") == "text") {
            //1:ﾃｷｽﾄ
        }
        else if ($(this).data("type") == "num") {
            //7:数値

            //書式文字列
            var fmt = $(this).data("format") + '';
            //※数値ﾁｪｯｸ不備対応～フォーマットがない場合もchangeｲﾍﾞﾝﾄを設定
            //if (fmt == null || fmt.length <= 0) return;     //continueに相当

            var fmts = fmt.split('.');

            //整数部～ｶﾝﾏ区切りか？
            var fmt_isComma = (fmts[0].indexOf(',') > -1);
            //小数部～桁数
            var fmt_dLen = 0;
            if (fmts.length >= 2) {
                if (fmts[1].length <= 0) {
                    //小数点以下桁数未設定
                    fmt_dLen = -1;
                } else {
                    fmt_dLen = fmts[1].length;
                }
            }

            //ﾌｫｰｶｽｱｳﾄ時の入力ﾌｫｰﾏｯﾄ処理を設定
            $(this).on('change blur', function () {

                var valW = $(this).val() + '';
                if (valW.length > 0) {
                    //入力値を書式ﾌｫｰﾏｯﾄ

                    //※数値ﾁｪｯｸ不備対応
                    valW = valW.trim();     //空白除去
                    if (valW == '-') {      //マイナスのみの場合→「0」
                        valW = '0';
                    }

                    var fmtVal = '';
                    if (valW.length <= 0 || (fmt == null || fmt.length <= 0)) {
                        //※値が未入力
                        //※ﾌｫｰﾏｯﾄ未設定の場合

                        fmtVal = valW;
                    } else {
                        //※ﾌｫｰﾏｯﾄ設定がされている場合

                        //例）-#,###,##0.000 / #,###,##0.000

                        //ﾏｲﾅｽ符号を取りだしてｾｯﾄ
                        if (valW.slice(0, 1) == '-') {
                            fmtVal = '-';
                            valW = valW.slice(1);   //先頭のﾏｲﾅｽ符号を除去
                        }

                        //入力値桁数が足りない場合、前方0埋め
                        var vals = valW.split('.');

                        //整数部
                        var valIntFmt = '';
                        if (fmt_isComma) {
                            //ｶﾝﾏ区切りで編集
                            var valInt = vals[0].replace(/,/g, '');  //一旦ｶﾝﾏ除去

                            //後ろから3文字取り出し
                            valIntFmt = valInt.slice(-3);
                            valInt = valInt.substr(0, valInt.length - 3);   //残りの文字列

                            while (valInt.length > 0) {     //残りの文字列が存在する間ﾙｰﾌﾟ
                                //後ろから3文字取り出し
                                valIntFmt = (valInt.slice(-3) + ',' + valIntFmt);
                                valInt = valInt.substr(0, valInt.length - 3);   //残りの文字列
                            }

                        } else {
                            valIntFmt += vals[0];
                        }
                        if (valIntFmt.length <= 0) {
                            valIntFmt = '0';
                        }

                        //小数部
                        //※fmt_dLen = 0の場合、小数部なし
                        //※fmt_dLen = -1の場合、小数部を編集しない
                        var valD = '';
                        if (fmt_dLen != 0) {
                            if (vals.length >= 2) valD = vals[1] + '';

                            if (fmt_dLen > 0) {
                                //※小数部桁数に合わせて編集

                                //小数点以下桁数で切り捨て
                                valD = valD.substr(0, fmt_dLen);
                                //右0埋め
                                for (var i = valD.length; i < fmt_dLen; i++) {
                                    valD += '0';
                                }
                            }

                        }

                        if (valD.length <= 0 && valIntFmt == '0') {
                            //「-0」の場合、符号を無視
                            fmtVal = valIntFmt;
                        }
                        else {
                            //符号 + 整数
                            fmtVal += valIntFmt;
                            if (valD.length > 0) {
                                fmtVal += ('.' + valD);
                            }
                        }
                    }

                    $(this).val(fmtVal);
                }

            });
        }
        else if ($(this).data("type") == "date") {
            //6:日付
            // 連紡(生産計画・紡糸)UT対応 【No.21】ATTS三原 start
            //書式文字列
            var fmt = $(this).data("format");
            if (fmt == null || fmt.length <= 0) return;     //continueに相当

            //ﾌｫｰｶｽｱｳﾄ時の入力ﾌｫｰﾏｯﾄ処理を設定
            $(this).on('blur', function () {
                var valW = $(this).val() + '';
                if (valW.length > 0) {
                    // 数値であれば処理続行
                    if (!isNaN(valW)) {
                        var fmtVal = '';
                        var dateLen = 0;

                        // フォーマット判定
                        if (fmt.indexOf('y') > -1) {
                            fmtVal = valW.slice(0, 4);
                            dateLen = 4;
                        }
                        if (fmt.indexOf('m') > -1) {
                            if (dateLen > 0) {
                                fmtVal = fmtVal + "/";
                            }
                            fmtVal = fmtVal + valW.slice(dateLen, dateLen + 2);
                            dateLen = dateLen + 2;

                        }
                        if (fmt.indexOf('d') > -1) {
                            if (dateLen > 0) {
                                fmtVal = fmtVal + "/";
                            }
                            fmtVal = fmtVal + valW.slice(dateLen, dateLen + 2);
                            dateLen = dateLen + 2;
                        }

                        // 日付として有効か確認
                        // yyyy/mm/dd の場合
                        if (dateLen == 8) {
                            var date = new Date(fmtVal);
                            if (date.getFullYear() == fmtVal.split("/")[0]
                                && date.getMonth() == fmtVal.split("/")[1] - 1
                                && date.getDate() == fmtVal.split("/")[2]
                            ) {
                                //入力値を書式ﾌｫｰﾏｯﾄ
                                $(this).val(fmtVal);
                                ////  [UPD_20190717_01][連紡(生産計画・紡糸)UT_No.17] start
                                //// 連紡(生産計画・紡糸)UT対応 【No.17】ATTS三原 start
                                //// datetimepickerの初期値として設定
                                //$(this).datepicker("setDate", fmtVal);

                                ////P_FormatFlg = 1;
                                ////// datetimepickerの初期値として設定
                                ////$(this).datepicker("setDate", fmtVal);
                                ////$(this).next().focus();
                                //// 連紡(生産計画・紡糸)UT対応 【No.17】ATTS三原 end

                                //// datetimepickerの初期値として設定
                                //$(this).bootstrapDP("setDate", fmtVal);
                                ////  [UPD_20190717_01][連紡(生産計画・紡糸)UT_No.17] end
                            }
                        }
                        //  [ADD_20190717_01][連紡(生産計画・紡糸)UT_No.17] start
                        // yyyy/mm の場合
                        else if (dateLen == 6) {
                            var now = new Date();
                            var fmtdate = fmtVal + "/" + "01";
                            var date = new Date(fmtdate);
                            if (date.getFullYear() == fmtVal.split("/")[0]
                                && date.getMonth() == fmtVal.split("/")[1] - 1
                            ) {
                                //入力値を書式ﾌｫｰﾏｯﾄ
                                $(this).val(fmtVal);

                                //// datetimepickerの初期値として設定
                                //$(this).bootstrapDP("setDate", fmtVal);
                            }
                        }
                        //  [ADD_20190717_01][連紡(生産計画・紡糸)UT_No.17] end
                        // mm/dd の場合
                        else if (dateLen == 4) {
                            var now = new Date();
                            var fmtdate = String(now.getFullYear()) + "/" + fmtVal;
                            var date = new Date(fmtdate);
                            if (date.getMonth() == fmtVal.split("/")[0] - 1
                                && date.getDate() == fmtVal.split("/")[1]
                            ) {
                                //入力値を書式ﾌｫｰﾏｯﾄ
                                $(this).val(fmtVal);
                                ////  [UPD_20190717_01][連紡(生産計画・紡糸)UT_No.17] start
                                //// 連紡(生産計画・紡糸)UT対応 【No.17】ATTS三原 start
                                //// datetimepickerの初期値として設定
                                //$(this).datepicker("setDate", fmtVal);

                                ////P_FormatFlg = 1;
                                ////// datetimepickerの初期値として設定
                                ////$(this).datepicker("setDate", fmtVal);
                                ////$(this).next().focus();
                                //// 連紡(生産計画・紡糸)UT対応 【No.17】ATTS三原 end

                                //// datetimepickerの初期値として設定
                                //$(this).bootstrapDP("setDate", fmtVal);
                                ////  [UPD_20190717_01][連紡(生産計画・紡糸)UT_No.17] end
                            }
                        }
                        // dd の場合
                        else if (dateLen == 2) {
                            var now = new Date();
                            var fmtdate = String(now.getFullYear()) + "/" + String(now.getMonth() + 1) + "/" + fmtVal;
                            var date = new Date(fmtdate);
                            if (date.getDate() == fmtVal) {
                                //入力値を書式ﾌｫｰﾏｯﾄ
                                $(this).val(fmtVal);
                                ////  [UPD_20190717_01][連紡(生産計画・紡糸)UT_No.17] start
                                //// 連紡(生産計画・紡糸)UT対応 【No.17】ATTS三原 start
                                //// datetimepickerの初期値として設定
                                //$(this).datepicker("setDate", fmtVal);

                                ////P_FormatFlg = 1;
                                ////// datetimepickerの初期値として設定
                                ////$(this).datepicker("setDate", fmtVal);
                                ////$(this).next().focus();
                                //// 連紡(生産計画・紡糸)UT対応 【No.17】ATTS三原 end

                                //// datetimepickerの初期値として設定
                                //$(this).bootstrapDP("setDate", fmtVal);
                                ////  [UPD_20190717_01][連紡(生産計画・紡糸)UT_No.17] end
                            }
                        }
                    }
                }
            });

        }
        else if ($(this).data("type") == "time") {
            //61:時刻

            //書式文字列
            var fmt = $(this).data("format");
            if (fmt == null || fmt.length <= 0) return;     //continueに相当

            var fmtLen = 0;
            if (fmt.indexOf('h') > -1) {
                fmtLen += 2;
            }
            if (fmt.indexOf('i') > -1) {
                fmtLen += 2;
            }

            //ﾌｫｰｶｽｱｳﾄ時の入力ﾌｫｰﾏｯﾄ処理を設定
            $(this).on('change', function () {

                var valW = $(this).val() + '';
                if (valW.length > 0) {
                    //入力値を書式ﾌｫｰﾏｯﾄ

                    //例）hhii
                    var fmtVal = '';

                    //入力値桁数が足りない場合、前方0埋め
                    //[UPD_190424_01] start
                    //valW = ('0000' + valW).slice((-1) * fmtLen);
                    if (valW.length < fmtLen) {
                        valW = ('0000' + valW).slice((-1) * fmtLen);
                    }

                    //if (fmt.indexOf('h') > -1) {
                    //    //入力時刻の時を付与
                    //    fmtVal += valW.substr(0, 2);
                    //    valW = valW.substr(2);      //残りの入力文字列
                    //}
                    //if (fmt.indexOf('i') > -1) {
                    //    //入力時刻の時を付与
                    //    fmtVal += valW.substr(0, 2);
                    //}
                    fmtVal = valW;
                    //[UPD_190424_01] end

                    $(this).val(fmtVal);
                }

            });
        }
        else {
            //その他
        }
    });

    //$("table select").each(function (index, element) {
    //    //5:ｺﾝﾎﾞﾎﾞｯｸｽ
    //    if ($(this).hasClass("validate_required")) {    //必須
    //        //
    //    }
    //});
    selectorW = null;
}

/**
 *  日付(ブラウザ標準)初期化処理
 */
function initDate() {
    var inputs = $("input[type='date']");
    if (inputs != null) {
        $.each(inputs, function (i, input) {
            // 最小値、最大値の設定
            var minVal = $(input).data("minval");
            var maxVal = $(input).data("maxval");
            var minStr = "";
            var maxStr = "";

            minStr = getDateMinMaxStr(minVal);
            maxStr = getDateMinMaxStr(maxVal);

            setAttrByNativeJs(input, 'min', minStr);
            setAttrByNativeJs(input, 'max', maxStr);

            // 日付ﾌｫｰﾏｯﾄ
            var fmt = $(input).data("format");
            if (fmt == null || fmt.length <= 0) {
                fmt = "yyyy/mm/dd";
            }
            // ﾌｫｰﾏｯﾄを上書き
            $(input).data("format", fmt);

            // 初期値を設定
            setInitDateValue(input);
        });
    }
}

/**
 * 日付項目のデフォルト値がSysDateなら値をセットする
 * @param {any} input 日付の項目
 */
function setInitDateValue(input) {
    // 初期値を設定
    var defVal = $(input).data("def");
    if (defVal != null && defVal.length > 0) {
        // 初期値が設定されている場合、日付セット

        if (defVal == "SysDate") {
            // 初期値に「SysDate」が設定されている場合、現在日付をセットする

            var now = new Date();

            var y = now.getFullYear();
            var m = ("00" + (now.getMonth() + 1)).slice(-2);
            var d = ("00" + now.getDate()).slice(-2);

            $(input).val(y + "-" + m + "-" + d);
        } else {
            $(input).val(defVal.replace(/\//g, '-'));
        }
    }
}

/**
 * 定義のminival,maxvalからmin,max属性にセット可能な文字列を取得する
 * @param {any} defVal
 */
function getDateMinMaxStr(defVal) {
    var date = new Date();

    //定義を解析して、日付範囲を算出
    if (defVal == null || defVal.length <= 0) {
        return "";
    } else {
        var hantei = true;
        var defVals = defVal.split(' ');
        //値チェック
        for (var i = 0; i < defVals.length; i++) {
            var m = defVals[i];
            if (m.length < 3) {
                hantei = false;
            }
            else {
                //1文字目が記号「+,-」
                if (m.substring(0, 1) != "+" && m.substring(0, 1) != "-") {
                    hantei = false;
                }
                else {
                    //末尾が「y,m,w,d」
                    var tani = m.substring(m.length - 1, m.length);
                    if (tani != "y" && tani != "m" && tani != "w" && tani != "d") {
                        hantei = false;
                    }
                    else {
                        //残りが整数
                        var n = Number(m.substring(1, m.length - 1));
                        if (n.isNaN) {
                            hantei = false;
                        }
                    }
                }
            }
        }
        if (hantei) {
            date = new Date();
            for (var i = 0; i < defVals.length; i++) {
                var m = defVals[i];

                var fugo = (m.substring(0, 1) == "+") ? 1 : -1;
                var tani = m.substring(m.length - 1, m.length);
                var num = parseInt(m.substring(1, m.length - 1), 10);

                switch (tani) {
                    case "y": date.setFullYear(date.getFullYear() + fugo * num); break;
                    case "m": date.setMonth(date.getMonth() + fugo * num); break;
                    case "w": date.setDate(date.getDate() + fugo * num * 7); break;
                    case "d": date.setDate(date.getDate() + fugo * num); break;
                    default: break;
                }
            }

            var y = date.getFullYear();
            var m = ("0" + (date.getMonth() + 1)).slice(-2);
            var d = ("0" + date.getDate()).slice(-2);

            return y + "-" + m + "-" + d;
        } else {
            return "";
        }
    }
}

/**
 *  時刻(ブラウザ標準)初期化処理
 */
function initTime() {
    var inputs = $("input[type='time']");
    if (inputs != null) {
        $.each(inputs, function (i, input) {
            // 書式から秒表示するかを判定
            var secondFlg = false;
            var fmt = $(input).data("format");

            if (fmt == null || fmt.length <= 0) {
                fmt = "HH:mm";
            } else {
                // 書式に秒が含まれる場合
                if (fmt.indexOf("s") > -1) {
                    setAttrByNativeJs(input, 'step', 1);
                    secondFlg = true;
                    fmt = "HH:mm:ss";
                } else {
                    fmt = "HH:mm";
                }
            }

            // ﾌｫｰﾏｯﾄを上書き
            setAttrByNativeJs(input, "data-format", fmt);

            // 初期値を設定
            var defVal = $(input).data("def");
            if (defVal != null && defVal.length > 0) {
                // 初期値が設定されている場合、時刻セット

                if (defVal == "SysDate") {
                    // 初期値に「SysDate」が設定されている場合、現在時刻をセットする

                    var now = new Date();

                    var h = ("00" + now.getHours()).slice(-2);
                    var mi = ("00" + now.getMinutes()).slice(-2);
                    var s = ("00" + now.getSeconds()).slice(-2);

                    if (secondFlg) {
                        $(input).val(h + ":" + mi + ":" + s);
                    } else {
                        $(input).val(h + ":" + mi);
                    }
                } else {
                    $(input).val(defVal);
                }

            }
        });
    }
}

/**
 *  日時(ブラウザ標準)初期化処理
 */
function initDateTime() {
    var inputs = $("input[type='datetime-local']");
    if (inputs != null) {
        $.each(inputs, function (i, input) {
            // 最小値、最大値の設定
            var minVal = $(input).data("minval");
            var maxVal = $(input).data("maxval");
            var minStr = "";
            var maxStr = "";

            minStr = getDateMinMaxStr(minVal);
            maxStr = getDateMinMaxStr(maxVal);

            if (minStr.length > 0) {
                setAttrByNativeJs(input, 'min', minStr + "T00:00:00");
            }
            if (maxStr.length > 0) {
                setAttrByNativeJs(input, 'max', maxStr + "T00:00:00");
            }

            // 書式から秒表示するかを判定
            var secondFlg = false;
            var fmt = $(input).data("format");

            if (fmt == null || fmt.length <= 0) {
            } else {
                // 書式に秒が含まれる場合
                if (fmt.indexOf("s") > -1) {
                    setAttrByNativeJs(input, 'step', 1);
                    secondFlg = true;
                }
            }

            //ﾌｫｰﾏｯﾄを上書き
            setAttrByNativeJs(input, "data-format", fmt);

            // 初期値を設定
            var defVal = $(input).data("def");
            if (defVal != null && defVal.length > 0) {
                // 初期値が設定されている場合、日時セット

                if (defVal == "SysDate") {
                    // 初期値に「SysDate」が設定されている場合、現在日時をセットする

                    var now = new Date();

                    var y = now.getFullYear();
                    var m = ("00" + (now.getMonth() + 1)).slice(-2);
                    var d = ("00" + now.getDate()).slice(-2);

                    var ymd = y + "-" + m + "-" + d;

                    var h = ("00" + now.getHours()).slice(-2);
                    var mi = ("00" + now.getMinutes()).slice(-2);
                    var s = ("00" + now.getSeconds()).slice(-2);

                    var time = h + ":" + mi;
                    if (secondFlg) {
                        time = time + ":" + s;
                    }

                    $(input).val(ymd + "T" + time);
                } else {
                    $(input).val(defVal.replace(/\//g, '-'));
                }
            }
        });
    }
}

///**
// *  チェックボックス初期化処理
// */
//function initCheckBox(selector) {
//    if (selector == null) {
//        var inputs = $(":checkbox");
//        if (inputs != null) {
//            $.each(inputs, function (i, input) {
//                // セルのクリックでチェックボックスON、OFFを行う
//                if (input.id != null && input.id != '') {
//                    $(input).click(function () {
//                        $(input).click();
//                    });

//                    $(input).closest("td").click(function () {
//                        $(input).click();
//                    });
//                }
//            });
//        }
//    } else {
//        $(selector).click(function () {
//            $(selector).click();
//        });

//        $(selector).closest("td").click(function () {
//            $(selector).click();
//        });
//    }
//}

/**
 *  チェックボックス初期化処理
 */
function initCheckBox(selector) {
    var direct = false;
    var checked_last = null;
    if (selector == null) {
        var inputs = $(":checkbox");
        if (inputs != null) {
            $.each(inputs, function (i, input) {
                // セルのクリックでチェックボックスON、OFFを行う
                if (input.id != null && input.id != '') {
                    $(input).click(function () {
                        direct = true;
                    });

                    $(input).closest("td").click(function () {
                        if ($(input).is(":disabled")) {
                            return;
                        }
                        if ($(input).is(":visible")) {
                            if (!direct) {
                                var checked = $(input).prop("checked");
                                $(input).prop("checked", !checked);
                                var tr = $(input).closest("tr");
                                setupDataEditedFlg(tr);
                            }
                            clickCheckBoxShiftkey(input);   //★AKAP2.0カスタマイズ★
                        }
                        direct = false;
                    });
                }
            });
        }
    } else {
        $(selector).click(function () {
            direct = true;
        });

        $(selector).closest("td").click(function () {
            if ($(selector).is(":disabled")) {
                return;
            }
            if ($(selector).is(":visible")) {
                if (!direct) {
                    var checked = $(selector).prop("checked");
                    $(selector).prop("checked", !checked);
                    var tr = $(selector).closest("tr");
                    setupDataEditedFlg(tr);
                }
                clickCheckBoxShiftkey(selector);    //★AKAP2.0カスタマイズ★
            }
            direct = false;
        });
    }
}
//★AKAP2.0カスタマイズ★
/**
 * Shift + ｸﾘｯｸでﾁｪｯｸﾎﾞｯｸｽの一括選択(解除)
 * @param {checkbox} selector
 */
function clickCheckBoxShiftkey(selector) {

    if (!$(selector).closest('table').hasClass('vertical_tbl')) {
        if (event.shiftKey && checked_last) {
            var name = $(checked_last).closest('td').data("name");  // VAL～
            var trs = $(checked_last).closest('tbody').find("tr:not([class^='base_tr'])");
            var tr1 = $(checked_last).closest('tr')[0];
            var tr2 = $(selector).closest('tr')[0];
            var p1 = $(trs).index($(tr1));
            var p2 = $(trs).index($(tr2));
            for (var i = Math.min(p1, p2); i <= Math.max(p1, p2); ++i) {
                var td = $(trs).find("[data-name='" + name + "']").get(i);
                var checkbox = $(td).find(":checkbox");
                if (checkbox != null && checkbox.length > 0) {
                    $(checkbox).prop("checked", checked_last.checked);
                }
            }
            // 選択範囲の解除
            window.getSelection().removeAllRanges();
        } else {
            checked_last = selector;
        }
    }
}
//★AKAP2.0カスタマイズ★
////★AKAP2.0標準
///**
// *  チェックボックス初期化処理
// */
//function initShiftCheckBox() {

//    // Shift + ｸﾘｯｸでﾁｪｯｸﾎﾞｯｸｽの一括選択(解除)
//    var checked_last = null;
//    $(":checkbox").on('click', function (event) {
//        if (!$(this).closest('table').hasClass('vertical_tbl')) {
//            if (event.shiftKey && checked_last) {
//                var name = $(checked_last).closest('td').data("name");  // VAL～
//                var trs = $(checked_last).closest('tbody').find("tr:not([class^='base_tr'])");
//                var tr1 = $(checked_last).closest('tr')[0];
//                var tr2 = $(this).closest('tr')[0];
//                var p1 = $(trs).index($(tr1));
//                var p2 = $(trs).index($(tr2));
//                for (var i = Math.min(p1, p2) ; i <= Math.max(p1, p2) ; ++i) {
//                    var td = $(trs).find("[data-name='" + name + "']").get(i);
//                    var checkbox = $(td).find(":checkbox");
//                    if (checkbox != null && checkbox.length > 0) {
//                        $(checkbox).prop("checked", checked_last.checked);
//                    }
//                }
//                // 選択範囲の解除
//                window.getSelection().removeAllRanges();
//            } else {
//                checked_last = this;
//            }
//        }
//    });

//}
////★AKAP2.0標準

/**
 *  コード＋翻訳 初期化処理
 */
function initCodeTrans(appPath, selector, sqlId, param) {

    $(selector).on('change blur', function () {
        //$(selector).on('change', function () {
        if (sqlId == '-') {
            // SQLが指定されない項目は処理を行わない
            return;
        }
        var select_val = $(this).val();
        var select = $(this);
        var paramName = 'code';
        var honyakuSpan = $(select).parent().find('.honyaku');
        var hiddenCode = $(select).parent().find('.autocomp_code');
        if (hiddenCode.length > 0 && select_val.length > 0) {
            //paramName = 'input';
            //非表示で保持しているIDを検索条件とする
            if ($(hiddenCode).val()) {
                /* if ($(hiddenCode).val() && select_val.length > 0) {*/
                select_val = $(hiddenCode).val();
            }
        }
        var formNo = getFormNo();
        if (select_val.length > 0) {
            // ﾊﾟﾗﾒｰﾀ「@3」⇒「ｾﾙの値」で置き換え
            paramStr = getParamVal(param, this);
            paramStr = encodeURI(paramStr); // POSTの場合は手動URLエンコード
            // オートコンプリート終了とボタン押下の処理タイミングを制御するために非同期処理の完了を管理
            promise.auto_complete =
                $.ajax({
                    url: appPath + "api/CommonSqlKanriApi/" + sqlId + "?param=" + paramStr + "&" + paramName + "=" + select_val,
                    type: "get",
                    dataType: "json",
                    success: function (datas) {
                        var data = separateDicReturn(datas);
                        // 工場IDで翻訳を絞り込み
                        var factoryIdList = getSelectedFactoryIdList(null, true, true);
                        data = filterStructureItemByFactory(data, factoryIdList);
                        // 取得した名称(VALUE2)を翻訳にセット
                        if ($(honyakuSpan).length) {
                            if (data != null && data.length > 0) {
                                var honyaku = data[0].VALUE2;
                                $(honyakuSpan).text(honyaku);
                                $(select).closest("td,.tabulator-cell").find("span.labeling").text(honyaku);
                            } else {
                                $(honyakuSpan).text("");
                                $(select).closest("td,.tabulator-cell").find("span.labeling").text("");
                            }
                        }
                        if ($(hiddenCode).length > 0) {
                            if (data != null && data.length > 0) {
                                var code = data[0].VALUE1;
                                var honyaku = data[0].VALUE2;
                                $(hiddenCode).val(code);
                                $(select).closest("td,.tabulator-cell").find("span.labeling").text(honyaku);
                                $(select).val(honyaku);
                            } else {
                                $(hiddenCode).val("");
                                $(select).closest("td,.tabulator-cell").find("span.labeling").text("");
                                //$(select).val("");
                            }
                        }
                        //【オーバーライド用関数】他の項目に値セット処理
                        setCodeTransOtherNames(appPath, formNo, select, data);
                        if (select.attr("data-transinit") != "1") {
                            setAttrByNativeJs(select, 'data-transinit', "1");
                        }
                        // 処理完了 何か返す
                        return 'Finish';
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        //console.log(textStatus);
                        // 処理完了 何か返す
                        return 'Finish';
                    }
                }).promise().then((response) => {
                    // 非同期処理の完了 何か返す
                    return response;
                });
        }
        else {
            if ($(honyakuSpan).length > 0) {
                $(honyakuSpan).text("");
            }
            if ($(hiddenCode).length > 0) {
                $(hiddenCode).val("")
            }
            $(select).closest("td,.tabulator-cell").find("span.labeling").text("");
            //【オーバーライド用関数】他の項目に値セット処理
            setCodeTransOtherNames(appPath, formNo, select);
            if (select.attr("data-transinit") != "1") {
                setAttrByNativeJs(select, 'data-transinit', "1");
            }
        }
    });

}

/**
 *  コード＋翻訳 初期値翻訳表示処理
 */
function initCodeTransInitval() {
    var inputs = $(P_Article).find(":text[data-type= 'codeTrans']");
    if (inputs != null) {
        $.each(inputs, function (i, input) {
            // 翻訳セットのためチェンジイベントを発火(編集フラグを変更しない)
            $(input).trigger('change', [true]);
        });
    }
}

/**
 * 画面初期化処理（画面NO単位の初期化）
 * @param	{string}	appPath
 * @param	{string}	conductId
 * @param	{string}	pgmId
 * @param	{number}	formNo
 * @param	{number}	originNo
 * @param	{string}	btnCtrlId
 * @param	{number}	conductPtn
 * @param	{object}	selectData
 * @param	{string}	targetCtrlId
 * @param	{object}	listData
 * @param	{number}	skipGetData : ﾃﾞｰﾀ取得ｽｷｯﾌﾟ（0：ｽｷｯﾌﾟしない、1：ｽｷｯﾌﾟする）
 * @param	{object}	status
 * @param	{number}	transPtn
 * @param	{number}	selFlg : 共通機能から選択ボタン押下で戻った場合のみ、「1:selFlgDef.Selected」が渡る
 * @param	{string}	backFrom : 共通機能からの戻る処理時、戻る前の共通機能ID※他機能同タブ遷移でも使える？
 */
//function initForm(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, transPtn, selFlg, backFrom) {
function initForm(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, transPtn, selFlg, backFrom, selDataFlg) {
    // selDataFlg = 1の場合、$("#main_contents > div.init_temp_data")のinput:hiddenタグからJSON文字列で受け取るよう修正
    if (selDataFlg + "" == "1") {
        var input = $("#main_contents > div.init_temp_data input:hidden[name='ConditionData_Temp']");
        // デシリアライズ
        selectData = deserializeInputJsonText(input);

        //ｴﾘｱの削除
        $(input).remove();
    }

    if (skipGetData == null) {
        skipGetData = skipGetDataDef.GetData;   //ﾃﾞｰﾀ取得する
    }
    if (transPtn == null) {
        transPtn = transPtnDef.None;
    }
    if (selFlg == null) {
        selFlg = selFlgDef.Default;
    }
    // 選択中の画面のフォーカスをクリア
    removeFocusSelected();

    if (transPtn == transPtnDef.CmConduct) {
        //※共通機能の場合
        P_Article = $('article[name="common_area"][data-conductid="' + conductId + '"]');
        if (P_Article.length > 1) {
            //※複数件見つかった場合、ﾍﾞｰｽﾌｫｰﾑを除外
            P_Article = $('article:not([class="base_form"])[name="common_area"][data-conductid="' + conductId + '"]');
        }
    } else {
        //選択中画面NOｴﾘｱ
        P_Article = $('article[name="main_area"][data-formno="' + formNo + '"]');
        if (P_Article.length > 1) {
            //※複数件見つかった場合、ﾍﾞｰｽﾌｫｰﾑを除外
            P_Article = $('article:not([class="base_form"])[name="main_area"][data-formno="' + formNo + '"]');
        }
    }
    if (P_Article.length > 1) {
        //※まだ複数件存在する場合、モーダル画面のarticleを選択する
        P_Article = $(P_Article).has('section.modal_form');
    }

    P_formSearchId = $(P_Article).find("form[id^='formSearch']").attr("id");
    P_formTopId = $(P_Article).find("form[id^='formTop']").attr("id");
    P_formBottomId = $(P_Article).find("form[id^='formBottom']").attr("id");
    P_formDetailId = $(P_Article).find("form[id^='formDetail']").attr("id");

    if (originNo == -1 || transPtn == transPtnDef.OtherTab) {
        //※メニュー選択/戻る/他機能遷移
        //画面ﾀｲﾄﾙを設定
        var formTitle = $(P_Article).data("formtitle");
        if (formTitle != null && formTitle.length > 0) {
            formTitle = " > " + formTitle;
        }
        $("#top_title_area span.form_title").text(formTitle);
    }

    if (transPtn == transPtnDef.Child && originNo >= 0) {
        //※子画面初期表示の場合

        //遷移元の親画面番号を保持
        var form = $(P_Article).find("form[id^='formDetail']");
        $(form).find('input[name="ORIGINNO"]').val(originNo);
    }

    if (formNo == 0) {
        //一覧画面の場合
        if (('sessionStorage' in window) && (window.sessionStorage !== null)) {
            //場所階層ツリーをセッションストレージに保存された表示状態に切り替え
            setHide($("#tree_view"), Boolean(sessionStorage.getItem("CIM_MENU_STATE")));
        }
    } else {
        //場所階層ツリーを非表示
        setHide($('#tree_view'), true);
    }

    //選択中画面の明細ｴﾘｱ一覧の定義情報取得をpublic変数に保持（※検索処理用）
    P_listDefines = [];     //public変数：一覧定義情報を初期化
    P_listDefines2 = [];    //public変数：一覧定義情報を初期化

    //ﾊﾞｯﾁ定型ﾃｰﾌﾞﾙ
    var batStatusTbl = $(P_Article).find("table[id$='_J']");
    if ($(batStatusTbl).length) {
        //ﾊﾞｯﾁ定型ﾃｰﾌﾞﾙのCTRLID
        var batStatusCtrlId = $(batStatusTbl).attr("id").slice(0, -2);

        //結果ｽﾃｰﾀｽ一覧（固定）
        var tmpData = {
            //CTRLID: ctrlIdComBatStatus,     // 一覧の画面定義のコントロールID
            CTRLID: batStatusCtrlId,     // 一覧の画面定義のコントロールID
            FORMNO: formNo,                 // 画面NO
            CTRLTYPE: '301',                // 一覧のｺﾝﾄﾛｰﾙﾀｲﾌﾟ(バッチは'301'固定)
            VAL1: '1',                      // 現在ﾍﾟｰｼﾞ番号（先頭ﾍﾟｰｼﾞ）
            VAL2: '0',   				    // 一覧の画面定義の出力最大件数
            VAL3: '0',  				    // 一覧の画面定義の1ページ行数
            VAL4: '',                       // 一覧のソートキー
        };
        P_listDefines.push(tmpData);
        P_isBat = true;     //Public変数：ﾊﾞｯﾁ機能ﾌﾗｸﾞ
    }
    //var areas = ["formTop", "formDetail", "formBottom"];
    var areas = ["formTop", "formSearch", "formDetail", "formBottom"];
    $.each(areas, function (idx, area) {
        var areaId = "";
        if (transPtn == transPtnDef.CmConduct) {
            areaId = area + "_" + conductId;
        }
        else {
            areaId = area + "_" + formNo;
        }
        var lists = $(P_Article).find("#" + areaId + " .ctrlId:not([id$='edit'],[id$='_tablebase'])");
        //var vCtrlId = "";
        if ($(lists).length) {
            $.each($(lists), function (i, list) {
                // 一覧の画面定義条件を生成
                var ctrlId = $(list).attr("data-ctrlid");
                var ctrlType = $(list).attr("data-ctrltype");
                var maxrows = "0";
                var pagerows = "0";

                if (true == $(list).hasClass("vertical_tbl")) {
                    // ※縦方向一覧の場合
                    maxrows = 1;
                    pagerows = 1;
                }
                else {
                    //※横方向一覧の場合
                    if ($(list).is("[data-maxrows]")) {
                        maxrows = $(list).data("maxrows");
                    }
                    if ($(list).is("[data-pagerows]")) {
                        pagerows = $(list).data("pagerows");
                    }
                    if (ctrlId == null || ctrlId == undefined) {
                        return true;    //continue
                    }
                }

                var pageNo = '1';
                if (!(transPtn == transPtnDef.Child && originNo >= 0)) {
                    //※子画面初期表示 以外の場合は現在ﾍﾟｰｼﾞ番号を取得
                    pageNo = getCurrentPageNo(ctrlId, 'def');
                }

                var tmpData = {
                    CTRLID: ctrlId,                 // 一覧の画面定義のコントロールID
                    FORMNO: formNo,                 // 画面NO
                    CTRLTYPE: ctrlType,             // 一覧のｺﾝﾄﾛｰﾙﾀｲﾌﾟ
                    VAL1: pageNo,                   // 現在ﾍﾟｰｼﾞ番号（先頭ﾍﾟｰｼﾞ）
                    VAL2: maxrows,   				// 一覧の画面定義の出力最大件数
                    VAL3: pagerows,  				// 一覧の画面定義の1ページ行数
                    VAL4: '',                       // 一覧のソートキー
                };
                P_listDefines.push(tmpData);
                if (targetCtrlId != null && targetCtrlId == ctrlId) {
                    P_listDefines2.push(tmpData);
                }

                // ドラッグ＆ドロップで行移動
                if ($(list).is("[data-rowsortflg]")) {
                    if ($(list).data("rowsortflg") == "True") {
                        $(list).find("tbody").sortable();
                    }
                }
            });
        }
    });

    /* ボタン権限制御 切替 start ==================================================================== */
    ////選択中画面のﾎﾞﾀﾝの権限をpublic変数に保持
    //P_buttonDefine = [];    //public変数：ﾎﾞﾀﾝの権限情報を初期化

    //var buttons = $(P_Article).find("input:button[data-actionkbn][data-authkbn='" + authControlKbnDef.Control + "']");  //権限管理あり
    //if ($(buttons).length) {
    //    $.each($(buttons), function (i, btn) {
    //        // 一覧の画面定義条件を生成
    //        var bCtrlId = $(btn).attr("name");
    //        var bAuthKbn = "";
    //        if ($(btn).is("[data-authkbn]")) {
    //            bAuthKbn = $(btn).data("authkbn");
    //        }

    //        var define = {
    //            ctrlId: bCtrlId,                 // ﾎﾞﾀﾝのCTRLID
    //            authKbn: bAuthKbn,               // ﾎﾞﾀﾝの権限区分
    //        };
    //        P_buttonDefine.push(define);
    //    });
    //}
    /* ボタン権限制御 切替 end ==================================================================== */

    //入力ﾌｫｰﾏｯﾄ設定処理
    initFormat("#" + P_formSearchId);
    initFormat("#" + P_formTopId);
    initFormat("#" + P_formBottomId);
    initFormat("#" + P_formDetailId);

    //Validator初期化
    initValidator("#" + P_formSearchId);
    initValidator("#" + P_formTopId);
    initValidator("#" + P_formBottomId);
    initValidator("#" + P_formDetailId);

    //Validator初期化(パスワード変更フォーム用)
    initValidatorForPassChange();
    //Validator初期化(詳細検索条件用)
    initValidatorForDetailSearch();

    //コード＋翻訳 初期値翻訳表示処理
    initCodeTransInitval();

    //オートコンプリート初期化
    var autoComps = $(P_Article).find("tr:not('.base_tr')")
        .find(":text[data-autocompdiv!=" + autocompDivDef.None + "], textarea[data-autocompdiv!=" + autocompDivDef.None + "]");
    $(autoComps).each(function (index, element) {
        var elementId = "#" + element.id;
        $.each(P_autocompDefines, function (idx, define) {
            if (elementId == define.key) { //完全一致
                var sqlId = define.sqlId;
                var param = define.param;
                var div = define.division;
                var option = define.option;
                initAutoComp(appPath, elementId, sqlId, param, null, div, option);
                return false;
            }
        });
    });

    // 日付コントロールの初期化
    initDateTypePicker($(P_Article).find("tr:not('.base_tr')"), true);

    if (transPtn != transPtnDef.CmConduct) {
        //単票を表示している場合は、単票エリアを選択（単票から共通画面を起動し、戻ってきた場合を想定）
        //var edit = $('section.modal_form.in:visible article[name="edit_area"][data-formno="' + formNo + '"]');
        var edit = $('section.modal_form.in:visible article[data-formno="' + formNo + '"]:not(".hide")');
        if (edit.length > 0) {
            P_Article = edit;
        }
    }

    //【ｵｰﾊﾞｰﾗｲﾄﾞ用関数】画面再表示ﾃﾞｰﾀ取得関数呼出前
    beforeCallInitFormData(appPath, conductId, pgmId, formNo, originNo, btnCtrlId, conductPtn, selectData, targetCtrlId, listData, skipGetData, status, selFlg, backFrom, transPtn);

    //画面再表示ﾃﾞｰﾀ取得
    if (skipGetData == skipGetDataDef.GetData) { //ﾃﾞｰﾀ取得する場合
        initFormData(appPath, conductId, pgmId, formNo, btnCtrlId, conductPtn, selectData, listData, status);
    }
}

/**
 * DatePicker系コントロールの初期化
 * 
 * セレクタと初期化時のパラメータの種類が色々あるため、この処理以外でも行っているので注意
 * */
function initDateTypePicker(element, isReset) {
    // 日付
    var datepickers = $(element).find(":text[data-type='date']");
    $.each(datepickers, function (idx, element) {
        var elementId = "#" + element.id;
        initDatePicker(elementId, isReset);
    });

    // 時刻
    var timepickers = $(element).find(":text[data-type='time']");
    $.each(timepickers, function (idx, element) {
        var elementId = "#" + element.id;
        initTimePicker(elementId, isReset);
    });

    // 日時
    var datetimepickers = $(element).find(":text[data-type='datetime']");
    $.each(datetimepickers, function (idx, element) {
        var elementId = "#" + element.id;
        initDateTimePicker(elementId, isReset);
    });
    // 年月
    var yearmonthpickers = $(element).find(":text[data-type='yearmonth']");
    $.each(yearmonthpickers, function (idx, element) {
        var elementId = "#" + element.id;
        initYearMonthPicker(elementId, isReset);
    });

    // 年
    var yearTexts = $(element).find(YearText.selector);
    $.each(yearTexts, function (idx, element) {
        var elementId = "#" + element.id;
        initYearText(elementId, isReset);
    });
}

/**
 *  画面初期値データ取得処理（画面初期化用）
 *  @appPath    {string}            ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @conductId  {string}            ：機能ID
 *  @pgmId      {string}            ：ﾌﾟﾛｸﾞﾗﾑID
 *  @formNo     {number}            ：画面番号 ※戻る時は親画面番号
 *  @btnCtrlId  {number}            ：ｱｸｼｮﾝﾎﾞﾀﾝのCTRLID
 *  @conductPtn {number}            ：機能処理ﾊﾟﾀｰﾝ(10:入力、11：バッチ、20：帳票、30：マスタ)
 *  @selectData {number}            ：NOﾘﾝｸ選択行のﾃﾞｰﾀ {List<Dictionary<string, object>>}
 *  @listData   {number}            ：明細一覧ﾃﾞｰﾀ {List<Dictionary<string, object>>}(※一覧⇒単票入力パターンで戻る時に設定)
 *  @status0    {CommonProcReturn}  ：実行処理結果ｽﾃｰﾀｽ
 *  @confirmNo  {number}            ：
 */
function initFormData(appPath, conductId, pgmId, formNo, btnCtrlId, conductPtn, selectData, listData, status0, confirmNo) {

    if (confirmNo == null) {
        confirmNo = 0;
    }
    //条件ｴﾘｱﾎﾞﾀﾝ件数
    var btnConCount = $(P_Article).find('form[id^="formSearch"] input[type="button"]').length;

    var conditionDataList = [];         //条件ﾃﾞｰﾀ

    //条件ﾃﾞｰﾀを取得
    var tblSearch = getConditionTable();            //条件一覧要素
    conditionDataList = getConditionData(formNo, tblSearch);   //条件ﾃﾞｰﾀ
    if (selectData != null && selectData.length > 0) {
        if (conditionDataList == null) { conditionDataList = []; }
        //条件ﾃﾞｰﾀを渡された場合
        conditionDataList = conditionDataList.concat(selectData);
    }

    var getPageNoFromStorage = false;
    let isBackBtn = false;

    var btnCtrlIdW = ctrlIdInit;        //「Init」:初期化処理（ﾒﾆｭｰ選択時・共通機能遷移時・他機能遷移時）
    if (btnCtrlId != null && btnCtrlId.length > 0) {
        // 「戻るﾎﾞﾀﾝ」「子画面新規ﾎﾞﾀﾝ」時検索の場合
        //※親画面の表示を初期化        
        clearSearchResult();    // - 検索結果をクリア        
        clearMessage();         // - 明細エリアのエラー状態を初期化
        clearErrorClasses(P_Article); // エラー情報をクリア ※縦方向一覧の背景色が戻っていなかったため

        btnCtrlIdW = btnCtrlId;     //ﾎﾞﾀﾝのCTRLID

        if (btnCtrlId.indexOf(ctrlIdBack) >= 0) {
            // 戻る処理の場合、セッションストレージからページ番号を取得する
            getPageNoFromStorage = true
            isBackBtn = true;
        }
    }

    //処理中メッセージ：on
    processMessage(true);
    dispLoading();

    // 詳細検索条件データ取得
    // 戻るボタン時は詳細条件を画面上から取得する
    //var detailConditionDataList = getDetailConditionData(conductId, formNo, null, true);
    var detailConditionDataList = getDetailConditionData(conductId, formNo, null, !isBackBtn);
    //【オーバーライド用関数】詳細検索条件取得後処理
    afterInitGetDetailConditionData(appPath, conductId, formNo, conditionDataList, detailConditionDataList);
    if (detailConditionDataList != null && detailConditionDataList.length > 0) {
        if (conditionDataList == null) { conditionDataList = []; }
        conditionDataList = conditionDataList.concat(detailConditionDataList);
    }

    // 103(Tabulator)一覧の場合、読込件数上限にデフォルト値をセット
    var listDefines = $.grep(P_listDefines, function (define, idx) {
        return (define.CTRLTYPE == ctrlTypeDef.IchiranPtn3);
    });
    $.each(listDefines, function (idx, define) {
        // 読込件数上限を取得＆設定
        define.VAL5 = !isBackBtn ? getSelectCntMaxDefault(define.CTRLID) : getSelectCntMax(define.CTRLID);
    });

    // 場所階層/職種機種データ取得(ストレージより)
    var locationIdList = getSelectedStructureIdListFromStorage(structureGroupDef.Location, treeViewDef.TreeMenu);
    var jobIdList = getSelectedStructureIdListFromStorage(structureGroupDef.Job, treeViewDef.TreeMenu);

    var W_listDefines = P_listDefines;
    if (P_listDefines2.length > 0) {
        //※単票入力画面からの戻りの場合
        //W_listDefines = P_listDefines2;
    }

    /* ボタン権限制御 切替 start ================================================ */
    //var btnDefines = P_buttonDefine;
    /*  ================================================ */
    var btnDefines = P_buttonDefine[conductId];
    /* ボタン権限制御 切替 end ================================================== */

    //【オーバーライド用関数】画面初期値データ取得前処理(表示中画面用)
    var [outConditionDataList, outListDefines, outConductId, outPgmId] = prevInitFormData(appPath, formNo, btnCtrlId, conditionDataList, W_listDefines, pgmId);
    if (outConductId) { conductId = outConductId; }
    if (outPgmId) { pgmId = outPgmId; }

    conditionDataList = outConditionDataList;
    W_listDefines = outListDefines;

    // 翻訳工場IDを取得
    var transFactoryId = getTransFactoryId(conductId, formNo);

    // POSTデータを生成
    var postdata = {
        conductId: conductId,                   // メニューの機能ID
        pgmId: pgmId,                           // メニューのプログラムID
        formNo: formNo,                         // 画面番号
        ctrlId: btnCtrlIdW,                     // 「Init」、またはBackアクションのCTRLID
        conditionData: conditionDataList,       // 検索条件入力データ
        listDefines: W_listDefines,             // 一覧定義情報
        listData: listData,                     // 明細一覧の入力データ（更新列情報受け渡し用）
        pageNo: 1,                              // ページ番号
        ListIndividual: P_dicIndividual,           // 個別実装用汎用ﾘｽﾄ

        confirmNo: confirmNo,                   //確認ﾒｯｾｰｼﾞ番号
        buttonDefines: btnDefines,              //ﾎﾞﾀﾝ権限情報　※ﾎﾞﾀﾝｽﾃｰﾀｽを取得

        browserTabNo: P_BrowserTabNo,           // ブラウザタブ識別番号

        locationIdList: locationIdList,         // 場所階層構成IDリスト
        jobIdList: jobIdList,                   // 職種機種構成IDリスト
        transFactoryId: transFactoryId,         // 翻訳工場ID

        ComboDataAcquiredFlg: P_ComboDataAcquiredFlg, //コンボボックスデータ取得済みフラグ
    };

    // 初期化処理実行
    $.ajax({
        url: appPath + 'api/CommonProcApi/' + actionkbn.ComInitForm,    // 【共通 - 初期化機能】初期値データ取得
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(postdata),
        headers: { 'X-XSRF-TOKEN': getRequestVerificationToken() },
        traditional: true,
        cache: false
    }).then(
        // 1つめは通信成功時のコールバック
        function (resultInfo) {
            var status = resultInfo[0];                                 //[0]:処理ステータス - CommonProcReturn
            var data = separateDicReturn(resultInfo[1], conductId);     //[1]:結果データ - Dictionary<string, object>※結果ﾃﾞｰﾀ："Result"、個別実装用ﾃﾞｰﾀ："Individual"
            /* ボタン権限制御 切替 start ================================================ */
            //var authShori = resultInfo[2];                              //[2]:ﾎﾞﾀﾝ処理ｽﾃｰﾀｽ  - Dictionary<ﾎﾞﾀﾝCTRLID,ｽﾃｰﾀｽ> ※ｽﾃｰﾀｽ：0:非表示、1(表示(活性))、2(表示(非活性))
            /* ボタン権限制御 切替 end ================================================== */

            // メッセージをクリア
            clearMessage();

            //処理メッセージを表示
            if (status0 != null && status0.MESSAGE != null && status0.MESSAGE.length > 0) {
                // 引数で渡されたstatus0が優先
                addMessage(status0.MESSAGE, status0.STATUS);
            }
            else if (status.MESSAGE != null && status.MESSAGE.length > 0) {
                //正常時、正常ﾒｯｾｰｼﾞ
                //警告、異常時、ｴﾗｰﾒｯｾｰｼﾞ
                addMessage(status.MESSAGE, status.STATUS);
            }
            // ｲﾍﾞﾝﾄを一旦解除
            setEventForEditFlg(false, null, "#" + P_formTopId);
            setEventForEditFlg(false, null, "#" + P_formDetailId);
            setEventForEditFlg(false, null, "#" + P_formBottomId);

            // 画面定義項目の翻訳を反映
            setFormDefineTransData(conductId, formNo);

            // コンボボックスアイテム設定
            if (!P_ComboDataAcquiredFlg) {
                setComboBoxItem(appPath);
                setMultiSelectBoxItem(appPath);
            }

            var pageRowCount = 0;
            if (conductPtn == conPtn.Bat) {
                //処理結果ﾃﾞｰﾀを取得
                /* ボタン権限制御 切替 start ================================================ */
                //dispBatStatusData(data, authShori);
                /* ================================================ */
                dispBatStatusData(data);
                /* ボタン権限制御 切替 end ================================================ */
                pageRowCount = 1;
            }
            else {
                //取得ﾃﾞｰﾀを一覧に表示
                //※明細ｴﾘｱのﾃﾞｰﾀ件数を取得
                pageRowCount = dispListData(appPath, conductId, pgmId, formNo, data, true);
            }

            // 画面変更ﾌﾗｸﾞ初期化
            dataEditedFlg = false;

            var curPageStatus = pageStatus.INIT;

            //ﾍﾟｰｼﾞの状態を設定
            //if (conductPtn == conPtn.Bat ||
            if (true == P_isBat ||
                (btnCtrlId != null && btnCtrlId.length > 0) ||
                btnConCount <= 0 ||
                pageRowCount > 0) {
                //バッチ機能の場合
                //戻る時の再表示時
                //検索条件ｴﾘｱにﾎﾞﾀﾝがない場合
                //明細データ取得済みの場合

                //入力ﾌｫｰﾏｯﾄ設定処理
                initFormat("#" + P_formDetailId);

                //Validator初期化
                initValidator("#" + P_formDetailId);

                //ﾍﾟｰｼﾞの状態を検索後/アップロード後に設定
                if (btnCtrlId == ctrlIdComUpload) {
                    setPageStatus(pageStatus.UPLOAD, pageRowCount, conductPtn);
                    curPageStatus = pageStatus.UPLOAD;
                }
                else {
                    setPageStatus(pageStatus.SEARCH, pageRowCount, conductPtn);
                    curPageStatus = pageStatus.SEARCH;
                }
            }
            else {
                //※検索前の状態、条件エリアのみ表示

                //ﾍﾟｰｼﾞの状態を初期状態に設定
                setPageStatus(pageStatus.INIT, pageRowCount, conductPtn);
            }

            //ﾎﾞﾀﾝ処理ｽﾃｰﾀｽを反映
            /* ボタン権限制御 切替 start ================================================ */
            //setButtonStatus(authShori);
            setButtonStatus();
            /* ボタン権限制御 切替 end ================================================== */

            //103一覧の定義を取得
            var tabulator = $(P_Article).find('div[data-ctrltype="103"]:not(".tabulatorHeader")');
            var blankIds = []; // 空テーブルを同じ一覧で二重に処理しないように処理したIDをリストに保持する
            $.each(tabulator, function (idx, table) {
                var id = $(table).data('ctrlid');
                var tbl_id = '#' + id + '_' + formNo;
                if (blankIds.indexOf(id) < 0) { // 既に処理した場合は行わない
                    if (!P_listData[tbl_id] || !data || $.inArray(id, data.map(x => x.CTRLID)) < 0) {
                        //表示するデータがない場合、ヘッダーのみの空テーブルを表示する
                        var info = {};
                        info['CTRLID'] = id;
                        var header = $(P_Article).find('.' + id + '.tabulatorHeader');
                        dispTabulatorListData(appPath, conductId, pgmId, formNo, [info], header);
                        // 再度処理しないようにIDを追加
                        blankIds.push(id);
                    }
                }
                if (!getPageNoFromStorage) {
                    // セッションストレージのページ番号をクリア
                    removeSaveDataFromSessionStorageForList(sessionStorageCode.PageNo, conductId, formNo, tbl_id.replace('#', ''));
                }

                if (data && data.length > 0) {
                    // 詳細条件適用有無を取得
                    var isDetailConditionApplied = $.grep(data, function (info, idx) {
                        return (info.CTRLID == id && info.IsDetailConditionApplied);
                    });
                    var selectCntDisabled = isDetailConditionApplied != null && isDetailConditionApplied.length > 0;
                    // 表示件数コンボの活性状態を設定
                    setSelectCntControlStatus(id, selectCntDisabled);
                }
            });

            var promise = new Promise((resolve) => {
                //【オーバーライド用関数】初期化処理(表示中画面用)
                initFormOriginal(appPath, conductId, formNo, P_Article, curPageStatus, btnCtrlId, data);

                //処理が完了したことを通知（thenの処理を実行）
                resolve();
            }).then(() => {
                //実行ボタンが全て非活性の場合、戻るor閉じるボタンにフォーカス設定
                setFocusBackOrClose();
            })

            nextFocus(); // タブフォーカスを制御
            // 列固定
            setFixColCss();

            // LocalStorage保存からDB登録へ変更
            //// LocalStorageから項目カスタマイズ設定を取り出して反映
            //var ulItemCustomize = $(".itemcustomize").not('.fixed');
            //initItemCustomizeByLocalStorage(ulItemCustomize);

            // 選択機能のメニューまでスクロール ※別プロジェクトから移植
            var li = $('#side_menu ul.level2 > li.selected');
            if (li.length > 0) {
                $('#side_menu').scrollTop($(li).offset().top - 100);
            }

            // 明細ｴﾘｱのﾁｪﾝｼﾞｲﾍﾞﾝﾄで画面変更ﾌﾗｸﾞON
            setEventForEditFlg(true, null, "#" + P_formTopId);
            setEventForEditFlg(true, null, "#" + P_formDetailId);
            setEventForEditFlg(true, null, "#" + P_formBottomId);

            //console.time('outputList');
        },
        // 2つめは通信失敗時のコールバック
        function (resultInfo) {
            // 画面初期化処理失敗

            //処理結果ｽﾃｰﾀｽ(CommonProcReturn)
            var result = resultInfo.responseJSON;
            var status;
            if (result.length > 1) {
                status = result[0];
                var data = separateDicReturn(result[1], conductId);

                //エラー詳細を表示
                dispErrorDetail(data);
            }
            else {
                status = result;
            }
            var confirmNo = 0;
            if (!isNaN(status.LOGNO)) {
                // 戻るボタンの場合かつ確認メッセージの場合、「キャンセル」押下したら変更フラグがそのままなので戻す
                if (btnCtrlId != null && btnCtrlId.length > 0 && status.STATUS == dataTypeDef.ConfirmStatus) {
                    // 画面変更ﾌﾗｸﾞ初期化
                    dataEditedFlg = false;
                }
                confirmNo = parseInt(status.LOGNO, 10);
            }
            var eventFunc = function () {
                initFormData(appPath, conductId, pgmId, formNo, btnCtrlId, conductPtn, selectData, listData, status0, confirmNo);
            }

            //処理結果ｽﾃｰﾀｽを画面状態に反映
            if (!setReturnStatus(appPath, status, eventFunc)) {
                return false;
            }

        }
    ).always(
        //通信の完了時に必ず実行される
        function (resultInfo) {
            //処理中メッセージ：off
            processMessage(false);

            //    if (!isBackBtn && ((transPtn == transPtnDef.None && formNo == 0) || transPtn == transPtnDef.OtherTab)) {
            //        setTimeout(initComboBoxes, 0, appPath, formNo, true);
            //        setTimeout(initMultiSelectBoxes, 0, appPath, formNo, true);
            //    }
        });

}

/**
 *  各種ボタンの初期化処理
 *  @param {string}  appPath    : ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string}  conductId  : 機能ID
 *  @param {string}  FileSize   : ﾌｧｲﾙｻｲｽﾞ
 *  @param {string}  target     : 対象セレクタ
 */
function initButtons(appPath, FileSize, target) {
    var isInitializing = true;
    if (!target) {
        target = "";
    } else {
        isInitializing = false;
        target += " ";
    }

    //ｺﾝﾄﾛｰﾙｸﾞﾙｰﾌﾟ内のﾕｰｻﾞｰ権限なしでﾎﾞﾀﾝが生成されなかったtdを削除する
    var emptyTd = $("table.actionlist td:has('span.empty')");
    if ($(emptyTd).length) {
        $(emptyTd).remove();
    }

    // ｱﾌﾟﾘ共通ﾍﾟｰｼﾞｬｰの初期化
    var pagination = $(target + '.paginationCommon[data-option="def"]');     //※ﾃﾞﾌｫﾙﾄ表示ﾍﾟｰｼﾞｬｰで絞込み
    if (pagination != null && pagination.length > 0) {
        initPagination(appPath, pagination);
    }
    pagination = null;

    // 検索ボタン
    var searchBtn = $(target + 'input:button[data-actionkbn="' + actionkbn.Search + '"]');
    if (searchBtn != null && searchBtn.length > 0) {
        // 検索ボタンの初期化
        initSearchBtn(appPath, searchBtn);
    }
    searchBtn = null;

    // 実行ボタン
    //　-  1:実行
    //　- 11:実行(更新)
    //　- 12:実行(確定)
    //　- 13:実行(承認)
    var registBtn = $(target + 'input:button[data-actionkbn="' + actionkbn.Execute + '"]');
    if (registBtn != null && registBtn.length > 0) {
        // 登録ボタンの初期化
        initRegistBtn(appPath, registBtn);
    }
    registBtn = null;

    // クリアボタン
    var clearBtn = $(target + 'input:button[data-actionkbn="' + actionkbn.Clear + '"]');
    if (clearBtn != null && clearBtn.length > 0) {
        // クリアボタンの初期化
        initClearBtn(clearBtn);
    }
    clearBtn = null;

    // 戻るボタン
    var backBtn = $(target + 'input:button[data-actionkbn="' + actionkbn.Back + '"]');
    if (backBtn != null && backBtn.length > 0) {
        // 戻るボタンの初期化
        initBackBtn(appPath, backBtn);
    }
    backBtn = null;

    // 閉じるボタン
    var closeBtn = $(target + 'input:button[data-actionkbn="' + actionkbn.Close + '"]');
    if (closeBtn != null && closeBtn.length > 0) {
        // 閉じるボタンの初期化
        initBackBtn(appPath, closeBtn, true);
    }
    closeBtn = null;

    // Excel出力ボタン
    //　- 311:Excel出力
    //　- 312:Excel出力（非同期）
    //  - 313:ExcelPortダウンロード
    var reportBtn = $(target + 'input:button[data-actionkbn="' + actionkbn.Report + '"],input:button[data-actionkbn="' + actionkbn.ReportHidoki + '"],input:button[data-actionkbn="' + actionkbn.ExcelPortDownload + '"]');
    if (reportBtn != null && reportBtn.length > 0) {
        // Excel出力ボタンの初期化
        initReportBtn(appPath, reportBtn);
    }
    reportBtn = null;

    // CSVファイル取込ボタン
    var uploadBtn = $(target + 'input:button[data-actionkbn="' + actionkbn.Upload + '"]');
    if (uploadBtn != null && uploadBtn.length > 0) {
        // 取込ボタンの初期化
        initUploadBtn(appPath, uploadBtn);
    }
    uploadBtn = null;

    // 削除ボタン
    var deleteBtn = $(target + 'input:button[data-actionkbn="' + actionkbn.Delete + '"]');
    if (deleteBtn != null && deleteBtn.length > 0) {
        // 削除ボタンの初期化
        initDeleteBtn(appPath, deleteBtn);
    }
    deleteBtn = null;

    // Template2.0 ADD start
    // 行追加ボタン(リンク)
    var addNewRowBtn = $(target + 'a[data-actionkbn="' + actionkbn.AddNewRow + '"]');
    if (addNewRowBtn != null && addNewRowBtn.length > 0) {
        // 行追加ボタンの初期化
        initAddNewRowBtn(appPath, addNewRowBtn);
    }
    addNewRowBtn = null;

    // 行コピーボタン(リンク)
    var addCopyRowBtn = $(target + target + 'a[data-actionkbn="' + actionkbn.AddCopyRow + '"]');
    if (addCopyRowBtn != null && addCopyRowBtn.length > 0) {
        // 行コピーボタンの初期化
        initAddNewRowBtn(appPath, addCopyRowBtn);
    }
    addCopyRowBtn = null;

    // 行削除ボタン(リンク)
    var deleteRowBtn = $(target + 'a[data-actionkbn="' + actionkbn.DeleteRow + '"]');
    if (deleteRowBtn != null && deleteRowBtn.length > 0) {
        // 行削除ボタンの初期化
        initDeleteRowBtn(appPath, deleteRowBtn);
    }
    deleteRowBtn = null;

    // 全選択ボタン(リンク)
    var selectAllBtn = $(target + 'a[data-actionkbn="' + actionkbn.SelectAll + '"]');
    if (selectAllBtn != null && selectAllBtn.length > 0) {
        // 全選択ボタンの初期化
        initAllSelectCancelBtn(true, appPath, selectAllBtn);
    }
    selectAllBtn = null;

    // 全解除ボタン(リンク)
    var cancelAllBtn = $(target + 'a[data-actionkbn="' + actionkbn.CancelAll + '"]');
    if (cancelAllBtn != null && cancelAllBtn.length > 0) {
        // 全解除ボタンの初期化
        initAllSelectCancelBtn(false, appPath, cancelAllBtn);
    }
    cancelAllBtn = null;

    // 【共通 - マスタ機能】一覧表示切替ボタン
    var switchTableBtn = $(target + 'a[data-actionkbn="' + actionkbn.ComSwitchTable + '"]');
    if (switchTableBtn != null && switchTableBtn.length > 0) {
        // 一覧表示切替ボタンの初期化
        initSwitchTableBtn(appPath, switchTableBtn);
    }
    switchTableBtn = null;
    //// 子画面新規ボタン
    //var childNewBtn = $('input:button[data-actionkbn="' + actionkbn.ChildNew + '"]');
    //if (childNewBtn != null && childNewBtn.length > 0) {
    //    // 子画面新規ボタンの初期化
    //    initChildNewBtn(appPath, childNewBtn);
    //}
    //// 単票入力新規ボタン
    //var editNewBtn = $('input:button[data-actionkbn="' + actionkbn.EditNew + '"]');
    //if (editNewBtn != null && editNewBtn.length > 0) {
    //    // 単票入力新規ボタンの初期化
    //    initEditNewBtn(appPath, editNewBtn);
    //}
    ////共通機能ポップアップボタン
    //var transCmBtn = $('input:button[data-actionkbn="' + actionkbn.TransCm + '"]');
    //if ($(transCmBtn).length) {
    //    // 共通機能ポップアップボタンの初期化
    //    initTransCmBtn(appPath, transCmBtn);
    //}
    ////他機能遷移（別タブ）ボタン
    //var transOtherTabBtn = $('input:button[data-actionkbn="' + actionkbn.TransOtherTab + '"]');
    //if ($(transOtherTabBtn).length) {
    //    // 他機能遷移（別タブ）ボタンの初期化
    //    initTransOtherTabBtn(appPath, transOtherTabBtn);
    //}
    ////他機能遷移（同タブ表示切替）ボタン
    //var transOtherShiftBtn = $('input:button[data-actionkbn="' + actionkbn.TransOtherShift + '"]');
    //if ($(transOtherShiftBtn).length) {
    //    // 他機能遷移（同タブ表示切替）ボタンの初期化
    //    initTransOtherShiftBtn(appPath, transOtherShiftBtn);
    //}
    //画面遷移ボタン
    var formTransitionBtn = $(target + 'input:button[data-actionkbn="' + actionkbn.FormTransition + '"]');
    if ($(formTransitionBtn).length) {
        // 画面遷移ボタンの初期化
        initFormTransitionBtn(appPath, formTransitionBtn);
    }
    formTransitionBtn = null;

    //個別実装ボタン
    var individualImplBtn = $(target + 'input:button[data-actionkbn="' + actionkbn.Individual + '"]');
    if ($(individualImplBtn).length) {
        // 個別実装ボタンの初期化
        initIndividualImplBtn(appPath, individualImplBtn);
    }
    individualImplBtn = null;

    // コード＋翻訳 選ボタン
    var codeTransBtn = $(target + 'input:button[data-type="codeTransBtn"]');
    if (codeTransBtn != null && codeTransBtn.length > 0) {
        // コード＋翻訳 選ボタンの初期化
        initCodeTransBtn(appPath, codeTransBtn);
    }
    codeTransBtn = null;

    // 共通機能用選択ボタン
    var comConductSelectBtn = $(target + 'input:button[data-actionkbn="' + actionkbn.Select + '"]');
    if ($(comConductSelectBtn).length) {
        // 共通機能用選択ボタンの初期化
        initComConductSelectBtn(appPath, comConductSelectBtn);
    }
    comConductSelectBtn = null;

    // 一覧表示列設定ボタン(リンク)
    var setDispItemBtn = $(target + 'a[data-actionkbn="' + actionkbn.ComSetDispItem + '"]');
    if (setDispItemBtn != null && setDispItemBtn.length > 0) {
        // 一覧表示列設定ボタンの初期化
        initSetDispItemBtn(appPath, setDispItemBtn);
    }
    setDispItemBtn = null;

    // 一覧表示列設定　適用ボタン
    var setDispItemExecBtn = $('input:button[data-actionkbn="' + actionkbn.ComSetDispItemExec + '"]');
    if (setDispItemExecBtn != null && setDispItemExecBtn.length > 0) {
        // 一覧表示列設定適用ボタンの初期化
        initSetDispItemExecBtn(appPath, setDispItemExecBtn);
    }
    setDispItemExecBtn = null;

    // 一覧表示列設定　保存ボタン
    var setDispItemSaveBtn = $('input:button[data-actionkbn="' + actionkbn.ComSetDispItemSave + '"]');
    if (setDispItemSaveBtn != null && setDispItemSaveBtn.length > 0) {
        // 一覧表示列設定保存ボタンの初期化
        initSetDispItemSaveBtn(appPath, setDispItemSaveBtn);
    }
    setDispItemSaveBtn = null;
    // Template2.0 ADD end

    // 【共通 - マスタ機能】表示切替ボタン
    var switchBtn = $(target + 'a[data-actionkbn="' + actionkbn.ComSwitch + '"]');
    if (switchBtn != null && switchBtn.length > 0) {
        // 検索条件表示切替ボタンの初期化
        initSwitchBtn(appPath, switchBtn);
    }
    switchBtn = null;

    // 【共通 - 取り込み機能】データ取込ダイアログ - 取り込みボタン、ExcelPortアップロードボタン
    //　※template2.0：画面定義可能なボタンにアップグレード
    var comUploadBtn = $(target + 'input:button[data-actionkbn="' + actionkbn.ComUpload + '"],input:button[data-actionkbn="' + actionkbn.ExcelPortUpload + '"]');
    if (comUploadBtn != null && comUploadBtn.length > 0) {
        // 取り込みボタン、ExcelPortアップロードボタンの初期化
        initComUploadBtn(appPath, comUploadBtn, FileSize);
    }
    comUploadBtn = null;

    // 詳細検索ボタン
    var detailSearchBtn = $(target + 'a[data-actionkbn="' + actionkbn.ComDetailSearch + '"]');
    if (detailSearchBtn != null && detailSearchBtn.length > 0) {
        // 詳細検索ボタンの初期化
        initDetailSearchBtn(appPath, detailSearchBtn);
    }
    detailSearchBtn = null;

    // 階層選択モーダル画面表示ボタン
    var TreeViewShowBtn = $(target + 'a[data-actionkbn="' + actionkbn.ComTreeViewShow + '"]');
    if (TreeViewShowBtn != null && TreeViewShowBtn.length > 0) {
        // 階層選択モーダル画面表示ボタンの初期化
        initTreeViewShowBtn(appPath, TreeViewShowBtn);
    }
    TreeViewShowBtn = null;

    // 以下は共通ボタンのため、初期化は画面生成時のみでOK
    if (!isInitializing) {
        return;
    }

    // 【共通 - バッチ機能】実行ボタン
    var batExecBtn = $('input:button[data-actionkbn="' + actionkbn.ComBatExec + '"]');
    if (batExecBtn != null && batExecBtn.length > 0) {
        // バッチ実行ボタンの初期化
        initBatExecBtn(appPath, batExecBtn);
    }
    batExecBtn = null;

    // 【共通 - バッチ機能】再表示ボタン
    var refreshBtn = $('input:button[data-actionkbn="' + actionkbn.ComBatRefresh + '"]');
    if (refreshBtn != null && refreshBtn.length > 0) {
        // 再表示ボタンの初期化
        initBatRefreshBtn(appPath, refreshBtn);
    }
    refreshBtn = null;

    // 【共通 - ログアウト】ログアウトボタン
    var logoutBtn = $('a[data-actionkbn="' + actionkbn.ComLogout + '"]');
    if (logoutBtn != null && logoutBtn.length > 0) {
        // ログアウトボタンの初期化
        initLogoutBtn(appPath, logoutBtn);
    }
    logoutBtn = null;

    // 【共通 - TOP遷移】サイトTOPに遷移
    var topBtn = $('a[data-actionkbn="' + actionkbn.ComRedirectTop + '"]');
    if (topBtn != null && topBtn.length > 0) {
        // サイトTOPボタンの初期化
        initSiteTopBtn(appPath, topBtn);
    }
    topBtn = null;

    // 【共通 - ＩＤ切替】ＩＤ切替ボタン
    var idchangebtn = $('a[data-actionkbn="' + actionkbn.ComIdChange + '"]');
    if (idchangebtn != null && idchangebtn.length > 0) {
        // ＩＤ切替ボタンの初期化
        initIdChangeBtn(appPath, idchangebtn);
    }
    idchangebtn = null;

    // 【共通 - パスワード変更】パスワード変更フォームポップアップボタン
    var passChangeFormBtn = $('a[data-actionkbn="' + actionkbn.ComPassChangeForm + '"]');
    if (passChangeFormBtn != null && passChangeFormBtn.length > 0) {
        // パスワード変更フォームポップアップボタンの初期化
        initPassChangeFormBtn(appPath, passChangeFormBtn);
    }
    passChangeFormBtn = null;

    // 【共通 - パスワード変更】パスワード変更実行ボタン
    var passChangeExecBtn = $('input:button[data-actionkbn="' + actionkbn.ComPassChangeExec + '"]');
    if (passChangeExecBtn != null && passChangeExecBtn.length > 0) {
        // パスワード変更実行ボタンの初期化
        initPassChangeExecBtn(appPath, passChangeExecBtn);
    }
    passChangeExecBtn = null;

    // 詳細検索実行ボタン
    var detailSearchExecBtn = $('input:button[data-actionkbn="' + actionkbn.ComDetailSearchExec + '"]');
    if (detailSearchExecBtn != null && detailSearchExecBtn.length > 0) {
        // 詳細検索ボタンの初期化
        initDetailSearchExecBtn(appPath, detailSearchExecBtn);
    }
    detailSearchExecBtn = null;

    // 詳細検索条件追加ボタン
    var detailSearchAddCondBtn = $('a[data-actionkbn="' + actionkbn.ComDetailSearchAddCond + '"]');
    if (detailSearchAddCondBtn != null && detailSearchAddCondBtn.length > 0) {
        // 詳細検索条件追加ボタンの初期化
        initDetailSearchAddCondBtn(appPath, detailSearchAddCondBtn);
    }
    detailSearchAddCondBtn = null;

    // 項目カスタマイズボタン
    var itemCustomizeBtn = $('a[data-actionkbn="' + actionkbn.ComItemCustomize + '"]');
    if (itemCustomizeBtn != null && itemCustomizeBtn.length > 0) {
        // 項目カスタマイズボタンの初期化
        initItemCustomizeBtn(appPath, itemCustomizeBtn);
    }
    itemCustomizeBtn = null;

    //// 項目カスタマイズ保存ボタン
    //var itemCustomizeSaveBtn = $('input:button[data-actionkbn="' + actionkbn.ComItemCustomizeSave + '"]');
    //if (itemCustomizeSaveBtn != null && itemCustomizeSaveBtn.length > 0) {
    //    // 項目カスタマイズボタンの初期化
    //    initItemCustomizeSaveBtn(appPath, itemCustomizeSaveBtn);
    //}

    // 項目カスタマイズ適用ボタン
    var itemCustomizeApplyBtn = $('input:button[data-actionkbn="' + actionkbn.ComItemCustomizeApply + '"]');
    if (itemCustomizeApplyBtn != null && itemCustomizeApplyBtn.length > 0) {
        // 項目カスタマイズ適用ボタンの初期化
        initItemCustomizeApplyBtn(appPath, itemCustomizeApplyBtn);
    }
    itemCustomizeApplyBtn = null;

    // 場所階層、職種機種検索ボタン
    var TreeViewSearchBtn = $('input:button[data-actionkbn="' + actionkbn.ComTreeViewSearch + '"]');
    if (TreeViewSearchBtn != null && TreeViewSearchBtn.length > 0) {
        // 場所階層、職種機種検索ボタンの初期化
        initTreeViewSearchBtn(appPath, TreeViewSearchBtn);
    }
    TreeViewSearchBtn = null;
}

/**
 *  Paginationの初期化処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {ul}     ：ﾍﾟｰｼﾞｬｰ（ﾃｰﾌﾞﾙ単位）
 */
function initPagination(appPath, selector) {

    var pagination = $(selector);

    if (pagination != null && pagination.length > 0) {

        //各ﾃｰﾌﾞﾙのﾍﾟｰｼﾞｬｰごとに設定
        $.each(pagination, function () {

            var form = $(this).closest("form");
            var conductIdW = $(form).find("input:hidden[name='CONDUCTID']").val();
            var pgmIdW = $(form).find("input:hidden[name='PGMID']").val();
            var formNoW = $(form).find("input:hidden[name='FORMNO']").val();

            //ｱﾌﾟﾘ共通ﾍﾟｰｼﾞｬｰ初期化処理
            initPaginationCommon(appPath, conductIdW, pgmIdW, formNoW, this);

        });

    }
}

/**
 *  検索ボタンの初期化処理
 *  @param {string}  appPath 　：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string}  btn 　　　：対象ボタン
 */
function initSearchBtn(appPath, btn) {

    // 検索ボタンクリックイベントハンドラの設定
    $(btn).on('click', function (event) {

        if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

        // 検索ボタン押下時、オートコンプリート処理が完了していない場合は完了を待つ
        event.preventDefault(); // イベントはキャンセル
        // 処理完了後に実行、複数の処理完了を待つ場合はPromiseの引数の配列に追加
        Promise.all([promise.auto_complete]).then((response) => {
            // 本来実行する処理をこちらに記載

            //var formTop = $(this).closest("article").find("form[id^='formTop']");
            var form = $(this).closest("article").find("form[id^='form']");
            var conductIdW = $(form).find("input:hidden[name='CONDUCTID']").val();
            var pgmIdW = $(form).find("input:hidden[name='PGMID']").val();
            var conductPtnW = $(form).find("input:hidden[name='CONDUCTPTN']").val();
            var formNoW = $(P_Article).data("formno");

            // 実行中フラグON
            P_ProcExecuting = true;
            // ボタンを不活性化
            $(this).prop("disabled", true);
            // モーダル画面の場合、検索ボタンが取得できないのでボタンを再取得
            // ※2024/08/20 以下の取り方だと対象機能の全ての検索ボタンが取得されるので、P_Articleから取得するよう変更(ただ、thisで問題ないと思う)
            //btn = $('input:button[data-actionkbn="' + actionkbn.Search + '"]');
            btn = $(P_Article).find($('input:button[data-actionkbn="' + actionkbn.Search + '"]'));

            try {
                //【オーバーライド用関数】検索前の個別実装(選択チェックボックスがチェックされているかチェック)
                if (!checkSelectedRowBeforeSearchBtnProcess(appPath, btn, conductIdW, pgmIdW, formNoW, conductPtnW)) {
                    return;
                }

                //【オーバーライド用関数】検索前の個別実装
                beforeSearchBtnProcess(appPath, btn, conductIdW, pgmIdW, formNoW, conductPtnW);

                // 検索処理実行
                executeSearchBtnProcess(appPath, btn, conductIdW, pgmIdW, formNoW, conductPtnW, false, true);

                //【オーバーライド用関数】検索後の個別実装
                afterSearchBtnProcess(appPath, btn, conductIdW, pgmIdW, formNoW, conductPtnW);
            }
            finally {
                // ボタンの活性化はAjax処理完了後に行う
                //$(this).prop("disabled", false);
            }
        });
    });
}

/**
 * 検索処理実行
 * @param {string} appPath                  ：アプリケーションルートパス
 * @param {element} btn                     ：検索ボタン
 * @param {string} conductId                ：機能ID
 * @param {string} pgmId                    ：プログラムID
 * @param {number} formNo                   ：画面番号
 * @param {number} conductPtn               ：処理パターン
 * @param {boolean} saveDetailCondition     ：詳細検索条件保存フラグ
 * @param {boolean} saveTreeMenuCondition   ：左側ツリーメニュー条件保存フラグ
 */
function executeSearchBtnProcess(appPath, btn, conductId, pgmId, formNo, conductPtn, saveDetailCondition, saveTreeMenuCondition) {

    // 検索結果をクリア
    clearSearchResult();
    //検索エリアのエラー状態を初期化
    var element = $(P_Article).find("form[id^='formSearch']");
    clearErrorStatus(element);

    // 検索条件ﾃﾞｰﾀの入力ﾁｪｯｸ
    if (!validConditionData()) {
        //入力ﾁｪｯｸｴﾗｰ
        // 実行中フラグOFF
        P_ProcExecuting = false;
        // ボタンを活性化
        $(btn).prop("disabled", false);
        return false;
    }

    // ページ状態の初期化
    initPageStatus();

    // ページデータの取得
    getPageData(appPath, btn, $(btn).attr("name"), conductId, pgmId, formNo, conductPtn, 0, saveDetailCondition, saveTreeMenuCondition);

}

/**
 *  ページ状態の初期化処理
**/
function initPageStatus() {
    // ページャーの総件数をクリア
    var pagination = $(P_Article).find('.paginationCommon');
    $(pagination).data('totalcnt', 0);

    var pageNo = 1;
    const conductId = $(P_Article).find("form[id^='form'] input:hidden[name='CONDUCTID']").val();
    const formNo = getFormNo();

    $.each(P_listDefines, function (i, define) {
        var ctrlId = define.CTRLID;
        if (ctrlId != null) {
            // 一覧の画面定義条件のソートキーをクリア
            define.VAL4 = '';

            // セッションストレージに保存したページ番号をクリア
            setSaveDataToSessionStorageForList(null, sessionStorageCode.PageNo, conductId, formNo, ctrlId);

            // ページャーの状態を設定(1ページ目表示)
            setPaginationStatus(ctrlId, pageNo);
        }
    });
}

/**
 *  入力コントロールの初期化処理
 *  @parent {element}   ：対象親要素
**/
function initInputControls(parent, selecter, modal) {

    // オートコンプリートの初期化
    var autoComps = $(parent).find(selecter)
        .find(":text[data-autocompdiv!=" + autocompDivDef.None + "], textarea[data-autocompdiv!=" + autocompDivDef.None + "]");
    $(autoComps).each(function (index, element) {
        var elementId = "#" + element.id;
        $.each(P_autocompDefines, function (idx, define) {
            if (elementId.substr(0, elementId.length - 1) == define.key) { //idの先頭が一致する定義情報
                var sqlId = define.sqlId;
                var param = define.param;
                var div = define.division;
                var option = define.option;
                initAutoComp(appPath, elementId, sqlId, param, modal, div, option);
                return false;
            }
        });
    });

    // 日付コントロールの初期化
    initDateTypePicker($(parent).find(selecter), true);
}

/**
 *  検索結果の設定処理※横方向一覧
 *  @param {string} appPath         ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {number} transPtn        ：画面遷移ﾊﾟﾀｰﾝ
 *  @param {string} info            ：検索結果
 *  @param {tr}     baseTr          ：レイアウト用trデータ
 *  @param {number} idx             ：表示インデックス
 */
function dispDataHorizontal(appPath, transPtn, info, baseTr, idx) {

    // レイアウト行をコピーして検索結果をセット
    var cloneTr = $(baseTr).clone(true);

    $(cloneTr).removeAttr("id");
    $(cloneTr).removeClass("base_tr");

    if (info.ROWSTATUS == rowStatusDef.ReadOnly) {
        // 行ステータスが表示のみの場合、入力コントロール、SELTAG列、ROWNO列をﾗﾍﾞﾙ表示
        var tds = $(cloneTr).find('td[data-name="SELTAG"], td[data-name="ROWNO"], td[data-name^="VAL"]');
        $(tds).addClass("readonly");

    }
    else if (info.ROWSTATUS == rowStatusDef.New) {
        // 新規行の場合、lockDataにidxを設定
        $(cloneTr).find("td[data-name='lockData']").text(idx);

        // 行ステータスが新規の場合、ｺﾝﾎﾞﾎﾞｯｸｽの先頭に空白項目を挿入
        var selects = $(cloneTr).find("select");
        $.each(selects, function (idx, select) {
            if ($(select).hasClass("validate_required")) {
                //必須項目の場合も、未選択状態にできるように、空白項目を挿入

                //新規行用のCssClass付与
                $(select).addClass("rowStatusNew");

                // 任意項目の場合、先頭に空白項目を追加
                $('<option>').val("").html("").prependTo($(select));
            }
        });
    } else {
        // 行ステータスが新規以外の場合、既存変更不可項目をﾗﾍﾞﾙ表示
        var unChangeableTds = $(cloneTr).find("td[data-unchangeablekbn='" + unChangeableKbnDef.Unchangeable + "']");
        if ($(unChangeableTds).length) {
            $(unChangeableTds).addClass("unchange_exist");
        }
    }

    setAttrByNativeJs(cloneTr, "data-datatype", info.DATATYPE);
    setAttrByNativeJs(cloneTr, "data-rowno", info.ROWNO);
    setAttrByNativeJs(cloneTr, "data-rowstatus", info.ROWSTATUS);

    var rowStatus = info.ROWSTATUS;


    // id、nameを設定
    var controlTds = $(cloneTr).find('td[data-name^="VAL"]');
    var controls = $(controlTds).find("ul.multiSelect, :checkbox, select, input[data-type='num'], :text, textarea, :radio");
    if ($(controls).length) {
        $.each($(controls), function (i, control) {
            if (!$(control).is(":radio")) {
                setAttrByNativeJs(control, 'id', $(control).attr('id') + '_' + idx);
            }
            setAttrByNativeJs(control, 'name', $(control).attr('name') + '_' + idx);
        });
    }
    //連動ｺﾝﾎﾞ再設定
    var dynamicSelect = $(controlTds).find("select.dynamic");
    if ($(dynamicSelect).length) {
        resetComboBox(appPath, dynamicSelect);
    }
    dynamicSelect = null;

    $.each(info, function (key, value) {
        var td = $(cloneTr).find('td[data-name="' + key + '"]');
        if (td != null && td.length > 0) {
            if (value == null) {
                value = "";
            }

            // データ項目の列に検索結果データをセット
            if (key.substr(0, 3) === "VAL") {
                //複数選択ﾘｽﾄ（※inputﾀｸﾞと間違わないように先に実施）
                var msuls = $(td).find('ul.multiSelect');
                if (msuls != null && msuls.length > 0) {
                    setDataForMultiSelect(td, msuls, value);    //値ｾｯﾄ（複数選択ﾘｽﾄ）
                    return true;
                }

                //ﾁｪｯｸﾎﾞｯｸｽ
                var checkbox = $(td).find(":checkbox");
                if (checkbox != null && checkbox.length > 0) {
                    setDataForCheckBox(td, checkbox, value);    //値ｾｯﾄ（ﾁｪｯｸﾎﾞｯｸｽ）
                    return true;
                }

                //ｺﾝﾎﾞﾎﾞｯｸｽ
                var select = $(td).find("select");
                if (select != null && select.length > 0) {
                    setDataForSelectBox(td, select, value); //値ｾｯﾄ（ｾﾚｸﾄﾎﾞｯｸｽ）
                    return true;
                }

                //ラジオボタン
                var radio = $(td).find(":radio");
                if (radio != null && radio.length > 0) {
                    setDataForRadioButton(td, radio, value); //値ｾｯﾄ（ラジオボタン）
                    return true;
                }

                //数値
                var input = $(td).find("input[data-type='num']");
                if (input != null && input.length > 0) {
                    setDataForTextNum(td, input, value);    //値ｾｯﾄ（数値）
                    return true;
                }

                //ﾃｷｽﾄ、日付、時刻、日時、ﾃｷｽﾄｴﾘｱ、ｺｰﾄﾞ＋翻訳
                var input = $(td).find(":text, textarea");
                if (input != null && input.length > 0) {
                    var txt = "";
                    if ($(input).hasClass('fromto') && input.length > 1) {
                        var values = value.split('|'); //fromto分割
                        // From
                        $(input[0]).val(values[0]);
                        txt = $(input[0]).val();
                        setAttrByNativeJs(input[0], "data-value", values[0]); //※初期化後ｾｯﾄ用に退避

                        // To
                        if (values.length > 1) {
                            $(input[1]).val(values[1]);
                            txt = txt + P_ComMsgTranslated[911060006] + $(input[1]).val();
                            setAttrByNativeJs(input[1], "data-value", values[1]); //※初期化後ｾｯﾄ用に退避
                        }
                    } else {
                        $(input).val(value);
                        txt = $(input).val();
                        setAttrByNativeJs(input, "data-value", value); //※初期化後ｾｯﾄ用に退避
                    }
                    //ﾗﾍﾞﾙ表示用spanﾀｸﾞに表示文字列ｾｯﾄ
                    setLabelingSpan(td, txt);
                    return true;
                }

                // 日付（ブラウザ標準）
                var input = $(td).find("input[type='date']");
                if (input != null && input.length > 0) {
                    setDataForTypeDate(td, input, value);   //値ｾｯﾄ（ﾌﾞﾗｳｻﾞ標準日付）
                    return true;
                }

                // 時刻（ブラウザ標準）
                var input = $(td).find("input[type='time']");
                if (input != null && input.length > 0) {
                    setDataForTypeTime(td, input, value);   //値ｾｯﾄ（ﾌﾞﾗｳｻﾞ標準時刻）
                    return true;
                }

                // 日時（ブラウザ標準）
                var input = $(td).find("input[type='datetime-local']");
                if (input != null && input.length > 0) {
                    setDataForTypeDatetimelocal(td, input, value);  //値ｾｯﾄ（ﾌﾞﾗｳｻﾞ標準日時）
                    return true;
                }

                // ﾌｧｲﾙ選択
                var input = $(td).find("input[type='file']");
                if (input != null && input.length > 0) {
                    //※何もｾｯﾄしない
                    return true;
                }

                // ダウンロードリンク、ファイル参照リンク
                var a = $(td).find("a[data-type='download'],a[data-type='fileOpen']");
                if (a != null && a.length > 0) {
                    setDataForFileDownloadOpenLink(td, a, value);   //値ｾｯﾄ（ダウンロードリンク、ファイル参照リンク）
                    return true;
                }

                // 画像
                var img = $(td).find("img");
                if (img != null && img.length > 0) {
                    setDataForImg(img, value);  //値ｾｯﾄ（画像）
                    return true;
                }

                //ボタン
                var button = $(td).find(":button");
                if (button != null && button.length > 0) {
                    //※何も設定しない
                    return true;
                }
                // リンク項目の場合はリンクテキストにセット
                var a = $(td).find("a");
                if ($(a).length > 0) {
                    $(a).text(value);
                    return true;
                }

                //パスワード
                var password = $(td).find("input[type='password']");
                if ($(password).length > 0) {
                    $(password).val(value);
                    setAttrByNativeJs(password, "data-value", value); //※初期化後ｾｯﾄ用に退避
                    //ﾗﾍﾞﾙ表示用spanﾀｸﾞに表示文字列ｾｯﾄ
                    setLabelingSpan(td, value);
                    return true;
                }

                // 上記以外はtdに該当データを出力する
                $(td).text(value);

            } else if (key == "ROWNO") {
                // 行番号を遷移リンクのｸﾘｯｸｲﾍﾞﾝﾄﾊﾝﾄﾞﾗの引数にセット
                var transLink = $(td).closest("tr").find("a[data-type='transLink']");
                if ($(transLink).length) {
                    $.each($(transLink), function (idx, a) {
                        setAttrByNativeJs(a, "onclick", $(a).attr("onclick").replace("rowNo", value));
                        var iconSpan = $(a).find('span.glyphicon');
                        if (iconSpan == null || iconSpan.length <= 0) {
                            //アイコン表示がない場合は列番号を表示
                            $(a).text(value);
                        }
                    });
                }
                // ROWNO列のﾗﾍﾞﾙ表示用spanﾀｸﾞに表示文字列ｾｯﾄ
                setLabelingSpan(td, value);

            } else if (key == "SELTAG") {
                //選択行の場合
                var checkbox = $(td).find(":checkbox");
                if (checkbox != null && checkbox.length > 0) {
                    setDataForCheckBox(td, checkbox, value);    //値ｾｯﾄ（ﾁｪｯｸﾎﾞｯｸｽ）
                    return true;
                }

                //} else if (key == "UPDTAG" && transPtn == transPtnDef.Edit) {
            } else if (key == "UPDTAG") {
                //$(td).val(value);
                //更新行の場合
                var updcol = $(td).find("input[data-type='updflg']");
                if (updcol != null && updcol.length > 0) {
                    $(updcol).val(value);
                }
            } else if (key == "lockData") {
                //排他ﾃﾞｰﾀ保持用列
                $(td).text(value);
            }
        }
    });

    return cloneTr;
}

/**
 * 検索結果を表示※縦方向一覧
 * @param {string}  appPath ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 * @param {object}  data    ：明細ﾃﾞｰﾀ、およびｽﾀｲﾙﾃﾞｰﾀ [List<Dictionary<string, object>>]
 * @param {number}  formNo  ：画面NO
 * @param {boolean} isEdit  ：単票ｴﾘｱﾌﾗｸﾞ（true：単票表示、false：一覧表示）
 */
function dispDataVertical(appPath, data, formNo, isEdit) {
    if (data == null) {
        return;   //ﾃﾞｰﾀなし
    }
    var ctrlId = "";
    var tbl = null;
    // ツリーで複数選択された場合のみ、縦方向一覧でも値が複数となる
    var valDatas = [];

    var colCssData = null;
    var rowCssVal = '';

    $.each(data, function (idx, info) {
        if (info != null) {
            if (info.DATATYPE == dataTypeDef.ColCss) {
                //列スタイルデータ
                colCssData = info;
            }
            else if (info.DATATYPE == dataTypeDef.RowCss) {
                //行スタイル値
                rowCssVal = info.VAL1 == null ? '' : info.VAL1;
            }
            else {
                valDatas.push(info);
            }
        }
    });
    // 値が複数の場合、ツリーの選択値が複数なので、件数分表示欄を作成
    if (valDatas.length > 1) {
        var targetDiv = $(P_Article).find("#" + valDatas[0].CTRLID + "_" + formNo);
        addTreeLabels(targetDiv, valDatas.length);
        targetDiv = null;
    }
    // データの件数分繰り返し、実質的に繰り返しはツリーのみ
    $.each(valDatas, function (idxValDatas, valData) {
        if (valData != null) {
            ctrlId = valData.CTRLID;

            // 対象一覧要素を取得
            if (isEdit) {
                tbl = $(P_Article).find("#" + ctrlId + "_" + formNo + "_edit");
            }
            else {
                tbl = $(P_Article).find("#" + ctrlId + "_" + formNo);
            }
            if (tbl != null) {
                $(tbl).show();

                var rowNo = valData.ROWNO;
                var rowStatus = valData.ROWSTATUS;

                setAttrByNativeJs(tbl, "data-rowno", rowNo);

                // 工場コントロールの取得
                var factoryTd = getFactoryTdElement(tbl);

                // 明細データを表示
                $.each(valData, function (key, value) {
                    // 値をセットする要素の選択
                    var td = $(tbl).find('tbody td[data-name="' + key + '"]');
                    // 上で追加したツリーの追加行がある場合は、行番号を指定して値をセットする要素を取得する
                    var addTree = $(tbl).find(".addTreeLabel");
                    if (addTree != null && addTree.length > 0) {
                        td = $(tbl).find('table[data-rowno=' + rowNo + ']').find('tbody td[data-name="' + key + '"]');
                    }

                    if (td != null && td.length > 0) {
                        if (value == null) {
                            value = "";
                            // 新規データの場合、初期値を設定
                            if (rowStatus == rowStatusDef.New && $(td).data("initverticalvalue") != null && $(td).data("initverticalvalue") != "") {
                                value = $(td).data("initverticalvalue");
                            }
                        }

                        // データセルに明細データをセット
                        if (key.substr(0, 3) === "VAL") {
                            //ｽﾀｲﾙを初期化
                            // ﾃﾞﾌｫﾙﾄ設定以外のclassをすべて削除
                            $(td).removeClass(function (idx, className) {
                                var targetName = '';
                                if (idx == 0) {
                                    //下記以外のclassを削除
                                    targetName = className.replace('hide', '');
                                    targetName = targetName.replace('center', '');
                                    targetName = targetName.replace('right', '');
                                    targetName = targetName.replace('control', '');
                                    targetName = targetName.replace('validate_required', '');
                                    targetName = targetName.replace('multiSelect', '');
                                }

                                return targetName;
                            });

                            //ｽﾀｲﾙを再設定
                            var tdClassVar = rowCssVal;     //行ｽﾀｲﾙ
                            if (colCssData != null && colCssData[key] != null && colCssData[key].length > 0) {
                                if (tdClassVar.length > 0) {
                                    tdClassVar = tdClassVar + ' ';
                                }
                                tdClassVar = tdClassVar + colCssData[key];
                            }
                            if (tdClassVar.length > 0) {
                                $(td).addClass(tdClassVar);
                            }

                            //==値を設定==

                            //複数選択ﾘｽﾄ(※inoutﾀｸﾞと間違えないように先頭で実施)
                            var msuls = $(td).find('ul.multiSelect');
                            if (msuls != null && msuls.length > 0) {
                                resetMultiSelectBox(appPath, msuls);
                                setDataForMultiSelect(td, msuls, value);    //値ｾｯﾄ（複数選択ﾘｽﾄ）
                                return true;
                            }

                            //ｺﾝﾎﾞﾎﾞｯｸｽ
                            var select = $(td).find("select");
                            if (select != null && select.length > 0) {
                                // 新規行でコードが"0"の場合、初期値を設定
                                if (rowStatus == rowStatusDef.New && value == 0 && $(td).data("initverticalvalue") != "") {
                                    value = $(td).data("initverticalvalue");
                                }
                                setDataForSelectBox(td, select, value); //値ｾｯﾄ（ｾﾚｸﾄﾎﾞｯｸｽ）
                                return true;
                            }

                            //ラジオボタン
                            var radio = $(td).find(":radio");
                            if (radio != null && radio.length > 0) {
                                setDataForRadioButton(td, radio, value); //値ｾｯﾄ（ラジオボタン）
                                return true;
                            }

                            //ﾁｪｯｸﾎﾞｯｸｽ
                            var checkbox = $(td).find(":checkbox");
                            if (checkbox != null && checkbox.length > 0) {
                                setDataForCheckBox(td, checkbox, value);    //値ｾｯﾄ（ﾁｪｯｸﾎﾞｯｸｽ）
                                return true;
                            }

                            // 数値
                            var input = $(td).find("input[data-type='num']");
                            if (input != null && input.length > 0) {
                                setDataForTextNum(td, input, value);    //値ｾｯﾄ（数値）
                                return true;
                            }

                            // ﾃｷｽﾄ、日付系(datepicker系)、ﾃｷｽﾄｴﾘｱ
                            var input = $(td).find(":text, textarea");
                            if (input != null && input.length > 0) {
                                var txt = "";
                                if ($(input).hasClass('fromto') && input.length > 1) {
                                    var values = value.split('|'); //fromto分割
                                    //From
                                    $(input[0]).val(values[0]);
                                    txt = $(input[0]).val();
                                    setAttrByNativeJs(input[0], "data-value", values[0]); //※初期化後ｾｯﾄ用に退避
                                    //To
                                    if (values.length > 1) {
                                        $(input[1]).val(values[1]);
                                        txt = txt + P_ComMsgTranslated[911060006] + $(input[1]).val();
                                        setAttrByNativeJs(input[1], "data-value", values[1]); //※初期化後ｾｯﾄ用に退避
                                    }
                                } else {
                                    $(input).val(value);
                                    setAttrByNativeJs(input, "data-value", value); //※初期化後ｾｯﾄ用に退避
                                    // コード＋翻訳の場合
                                    if ($(input).data("type") == "codeTrans") {
                                        var hiddenCode = $(input).parent().find('.autocomp_code');
                                        if (hiddenCode != null && hiddenCode.length > 0) {
                                            // オートコンプリートのhidden項目も更新
                                            $(hiddenCode).val(value);
                                        }
                                        // AP課題No.214対応
                                        // このチェンジイベント発火により、画面変更フラグが必ず立ってしまう
                                        // イベント発火後に、発火前の値を再設定する
                                        var tempFlg = dataEditedFlg;
                                        // 翻訳セットのためチェンジイベントを発火
                                        $(input).trigger('change');
                                        // 再設定
                                        dataEditedFlg = tempFlg;
                                        return true;
                                    }
                                    txt = $(input).val();
                                }
                                //ﾗﾍﾞﾙ表示用spanﾀｸﾞに表示文字列ｾｯﾄ
                                setLabelingSpan(td, txt);
                                return true;
                            }

                            // 日付（ブラウザ標準）
                            var input = $(td).find("input[type='date']");
                            if (input != null && input.length > 0) {
                                setDataForTypeDate(td, input, value);   //値ｾｯﾄ（ﾌﾞﾗｳｻﾞ標準日付）
                                return true;
                            }

                            // 時刻（ブラウザ標準）
                            var input = $(td).find("input[type='time']");
                            if (input != null && input.length > 0) {
                                setDataForTypeTime(td, input, value);   //値ｾｯﾄ（ﾌﾞﾗｳｻﾞ標準時刻）
                                return true;
                            }

                            // 日時（ブラウザ標準）
                            var input = $(td).find("input[type='datetime-local']");
                            if (input != null && input.length > 0) {
                                setDataForTypeDatetimelocal(td, input, value);  //値ｾｯﾄ（ﾌﾞﾗｳｻﾞ標準日時）
                                return true;
                            }

                            // ﾌｧｲﾙ選択
                            var input = $(td).find("input[type='file']");
                            if (input != null && input.length > 0) {
                                //※何もｾｯﾄしない
                                return true;
                            }

                            // ダウンロードリンク、ファイル参照リンク
                            var a = $(td).find("a[data-type='download'],a[data-type='fileOpen']");
                            if (a != null && a.length > 0) {
                                setDataForFileDownloadOpenLink(td, a, value);   //値ｾｯﾄ（ダウンロードリンク、ファイル参照リンク）
                                return true;
                            }

                            // 画像
                            var img = $(td).find("img");
                            if (img != null && img.length > 0) {
                                setDataForImg(img, value);  //値ｾｯﾄ（画像）
                                return true;
                            }

                            // リンク項目の場合はリンクテキストにセット
                            var a = $(td).find("a");
                            if ($(a).length > 0) {
                                $(a).text(value);
                                return true;
                            }

                            //パスワード
                            var password = $(td).find("input[type='password']");
                            if ($(password).length > 0) {
                                $(password).val(value);
                                setAttrByNativeJs(password, "data-value", value); //※初期化後ｾｯﾄ用に退避
                                //ﾗﾍﾞﾙ表示用spanﾀｸﾞに表示文字列ｾｯﾄ
                                setLabelingSpan(td, value);
                                return true;
                            }

                            // ツリー選択ラベル
                            if ($(td).data('type') == 'treeLabel') {
                                var values = value.split('|'); // 表示文字列と構成IDに分割
                                if (values != null) {
                                    // ツリー選択ラベルに設定
                                    if (values.length == 1) { values.push(''); }
                                    setStructureInfoToTreeLabel(td, values[1], values[0]);
                                }
                                // 場所階層のコントロールの場合、工場コントロールのchangeイベントに連動させない
                                // 2024/07/19 機器別管理基準標準用場所階層ツリー追加 UPD start
                                //if ($(td).data('structuregrpid') == structureGroupDef.Location) { return true; }
                                //const locationGrpIdList = [structureGroupDef.Location, structureGroupDef.LocationForMngStd];
                                const locationGrpIdList = [structureGroupDef.Location, structureGroupDef.LocationNoHistory, structureGroupDef.LocationForMngStd];
                                if (locationGrpIdList.indexOf($(td).data('structuregrpid')) >= 0) { return true; }
                                // 2024/07/19 機器別管理基準標準用場所階層ツリー追加 UPD end

                                if (!factoryTd || factoryTd.length == 0) {
                                    // 同一画面上に工場コントロールがない場合、左側の場所階層メニューに連動
                                    // 場所階層の工場フィルター使用フラグをセット
                                    setAttrByNativeJs(td, 'data-usefactoryfilter', true);
                                } else if ($(factoryTd).data('type') == 'treeLabel') {
                                    //ｲﾍﾞﾝﾄ付与
                                    $(factoryTd).get(0).addEventListener('change', function () {
                                        // 工場コントロールのchangeイベント
                                        var factoryId = $(this).data('structureid');
                                        var tdFactoryId = $(td).data('factoryid');
                                        if (tdFactoryId != null && tdFactoryId != '') {
                                            if (factoryId != tdFactoryId) {
                                                // 工場IDが変更されたらツリー選択ラベルの値をクリアする
                                                setStructureInfoToTreeLabel(td, '', '');
                                            }
                                        }
                                        // ツリー選択ラベルに工場IDをセットする
                                        setAttrByNativeJs(td, 'data-factoryid', factoryId);
                                    }, false);
                                }
                                return true;
                            }

                            // 上記以外はtdに該当データを出力する
                            $(td).text(value);

                        } else if (key == "lockData") {
                            //排他ﾃﾞｰﾀ保持用列
                            $(td).text(value);
                        }

                    }
                });

                //参照モードの入力コントロール制御
                var refTbl = $(tbl).parent().find("[data-referencemode='" + referenceModeKbnDef.Reference + "']");
                if ($(refTbl).length) {
                    // 参照モードの場合、入力コントロールをﾗﾍﾞﾙ表示
                    var refTds = $(refTbl).find('td[data-name^="VAL"]').closest('td');
                    $(refTds).addClass("reference_mode");
                }
                factoryTd = null;

                //既存変更不可項目の入力コントロール制御
                var tds = $(tbl).find("tbody td[data-unchangeablekbn='" + unChangeableKbnDef.Unchangeable + "']");
                if ($(tds).length) {
                    if (rowStatus == rowStatusDef.New) {
                        //※行ステータスが新規の場合
                        $(tds).removeClass("unchange_exist");
                    }
                    else {
                        //※行ステータスが新規以外の場合
                        $(tds).addClass("unchange_exist");
                    }
                }
            }
        }
    });
    // コントロール初期化用
    var dPickers = [];
    var tPickers = [];
    var dtPickers = [];
    var ymPickers = [];
    // 追加行にAKAP標準の日付、時刻、日時コントロールがあればIDを退避
    var datePickers = $(tbl).find(":text[data-type='date']");
    $(datePickers).each(function (index, element) {
        dPickers.push(element.id);
    });
    var timePickers = $(tbl).find(":text[data-type='time']");
    $(timePickers).each(function (index, element) {
        tPickers.push(element.id);
    });
    var datetimePickers = $(tbl).find(":text[data-type='datetime']");
    $(datetimePickers).each(function (index, element) {
        dtPickers.push(element.id);
    });
    var yearmonthPickers = $(tbl).find(":text[data-type='yearmonth']");
    $(yearmonthPickers).each(function (index, element) {
        ymPickers.push(element.id);
    });
    // 追加したdatepicker系の日付、時刻、日時コントロールの初期化
    $(dPickers).each(function (index, dpId) {
        initDatePicker('#' + dpId, false);
    });
    $(tPickers).each(function (index, dpId) {
        initTimePicker('#' + dpId, false);
    });
    $(dtPickers).each(function (index, dpId) {
        initDateTimePicker('#' + dpId, false);
    });
    $(ymPickers).each(function (index, dpId) {
        initYearMonthPicker('#' + dpId, false);
    });
    // 年入力
    var yearTexts = $(tbl).find(YearText.selector);
    $(yearTexts).each(function (index, element) {
        initYearText(element, false);
    });

    //連動ｺﾝﾎﾞの選択ﾘｽﾄを再生成
    var selects = $(tbl).find('tbody select.dynamic');
    resetComboBox(appPath, selects);
    selects = null;

    //validator初期化
    var formId = $(tbl).closest("form[id^='form']").attr("id");
    initValidator("#" + formId);
}

/**
 *  クリアボタンの初期化処理
 *  @param {string} ：対象ボタン
 */
function initClearBtn(btn) {

    // クリアボタンクリックイベントハンドラの設定
    $(btn).on('click', function () {

        if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

        //ﾃﾞﾌｫﾙﾄ確認ﾒｯｾｰｼﾞを設定
        //『～します。よろしいですか？』
        P_MessageStr = P_MessageStrClear;
        //ﾎﾞﾀﾝﾒｯｾｰｼﾞがあれば置き換え
        var btnMsg = getBtnMessage(this);
        if (btnMsg != null) {
            P_MessageStr = btnMsg;
        }

        //処理継続用ｺｰﾙﾊﾞｯｸ関数呼出文字列を設定
        var eventFunc = function () {
            // 検索結果をクリア
            clearSearchResult();
            //明細エリアのエラー状態を初期化
            clearMessage();
            //ﾍﾟｰｼﾞの状態を初期状態に設定
            setPageStatus(pageStatus.INIT);

        }

        // ﾃﾞﾌｫﾙﾄ確認ﾒｯｾｰｼﾞを表示
        //『～します。よろしいですか？』
        if (!popupMessage([P_MessageStr], messageType.Confirm, eventFunc)) {
            //『キャンセル』の場合、処理中断
            return false;
        }
    });
}

/**
 *  コンテンツエリアのクリア処理
 */
function clearContentsArea() {
    $("#main_contents").children().remove();
}

/**
 *  登録ボタンの初期化処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 */
function initRegistBtn(appPath, btn) {

    // 登録ボタンクリックイベントハンドラの設定
    $(btn).on('click', function (event) {

        if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

        // 登録ボタン押下時、オートコンプリート処理が完了していない場合は完了を待つ
        event.preventDefault(); // イベントはキャンセル
        // 処理完了後に実行、複数の処理完了を待つ場合はPromiseの引数の配列に追加
        Promise.all([promise.auto_complete]).then((response) => {
            //単票表示ｴﾘｱか判定
            var isEdit = judgeBtnIsEditPosition(this);

            var form = $(this).closest("article").find("form[id^='form']");
            var conductIdW = $(form).find("input:hidden[name='CONDUCTID']").val();
            var pgmIdW = $(form).find("input:hidden[name='PGMID']").val();
            var conductPtnW = $(form).find("input:hidden[name='CONDUCTPTN']").val();
            var transDivW = $(form).find("input:hidden[name='TRANSACTIONDIV']").val();

            // 取得できていない(遷移していない場合)は未設定:"0"に置き換える
            if (transDivW == "") {
                transDivW = transDivDef.None;
            }
            var formNoW = $(P_Article).data("formno");  //formNo取得

            //エラー状態を初期化
            clearErrorComStatusForAreas(isEdit);

            var thisBtn = $(this);  // イベント発生ボタン
            var relationParam = $(thisBtn).data("relationparam") + ""; // 関連情報パラメータ
            var confirmKbn = confirmKbnDef.Disp;
            var validKbn = validKbnDef.Valid;
            var relationList = relationParam.split('|');
            $(relationList).each(function (i, relation) {
                if (i == 0) {
                    // i=0:確認ﾒｯｾｰｼﾞの有無
                    if (relation != null && relation != "") {
                        confirmKbn = relation;
                    }
                } else if (i == 1) {
                    // i=1:ﾊﾞﾘﾃﾞｰｼｮﾝ有無
                    if (relation != null && relation != "") {
                        validKbn = relation;
                    }
                }
            });


            //【オーバーライド用関数】個別バリデーション処理(共通の前に行う)
            var [isContinue, isError] = prevCommonValidCheck(appPath, conductIdW, formNoW, this);
            // 個別入力チェックでエラーがある場合
            if (isError) {
                return false;
            }

            // 共通の入力チェックを行う場合
            if (isContinue) {

                // ﾊﾞﾘﾃﾞｰｼｮﾝが有効の場合、実施
                if (validKbn == validKbnDef.Valid) {
                    //【オーバーライド用関数】バリデーション前処理
                    validDataPre(appPath, conductIdW, formNoW, this);

                    if (isEdit) {
                        if (!validFormEditData()) {
                            return false;
                        }
                    }
                    else {

                        //ﾄｯﾌﾟｴﾘｱ・明細ｴﾘｱ・ﾎﾞﾄﾑｴﾘｱの入力ﾁｪｯｸ
                        var topValid = validFormTopData();
                        var listValid = validListData();
                        var bottomValid = validFormBottomData();
                        if (!topValid || !listValid || !bottomValid) {
                            //入力ｴﾗｰ
                            return false;
                        }
                    }
                }
            }

            //【オーバーライド用関数】実行ﾁｪｯｸ処理 - 前処理
            if (!registCheckPre(appPath, conductIdW, formNoW, this)) {
                return false;
            }

            //var crtlId = $(this).attr("name");          //処理ﾎﾞﾀﾝctrlId
            //var actionkbnW = $(this).data('actionkbn'); //共通処理区分
            //var authkbnW = $(this).data('authkbn');     //処理権限区分
            //var thisBtn = $(this);  // イベント発生ボタン

            var autoBackFlg = false;                        //正常終了後自動で戻るフラグ　false:戻らない、true:自動で戻る
            var afterExecKbn = $(this).data("afterexeckbn");
            if ((true == isEdit || formNoW > 0) && afterExecKbn != null && afterExecKbn == afterExecKbnDef.AutoBack) {
                // 単票入力画面または子画面で、且つ、自動で戻る区分が 1:自動で戻る の場合
                autoBackFlg = true;
            }

            //var confirmKbn = $(thisBtn).data("relationparam") + "";  //確認メッセージ表示区分
            //ﾒｯｾｰｼﾞ設定
            setMessageStrForBtn(thisBtn, confirmKbn);

            //ﾎﾞﾀﾝﾒｯｾｰｼﾞがあれば置き換え
            var btnMsg = getBtnMessage(this);
            if (btnMsg != null && btnMsg.length > 0) {
                P_MessageStr = btnMsg;
            }

            //処理継続用ｺｰﾙﾊﾞｯｸ関数呼出文字列を設定
            var eventFunc = function () {
                clickRegistBtnConfirmOK(appPath, transDivW, conductIdW, pgmIdW, formNoW, thisBtn, conductPtnW, autoBackFlg, isEdit);
            }

            // ﾃﾞﾌｫﾙﾄ確認ﾒｯｾｰｼﾞを表示
            //『～します。よろしいですか？』
            if (!popupMessage([P_MessageStr], messageType.Confirm, eventFunc, thisBtn)) {
                //『キャンセル』の場合、処理中断
                return false;
            }
        });
    });
}

// 一覧で選択された行のみ取得する機能ID
const ConductIdGetSelectedRow = ["DM0001"];
// 一覧で選択された行のみ取得するボタンコントロールID
const CtrlIdGetSelectedRow = ["Delete"];

/**
 *  登録ボタン - 確認メッセージOK時、実行処理
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
 */
function clickRegistBtnConfirmOK(appPath, transDiv, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, confirmNo) {
    // 選択行のみを取得する機能の場合はフラグをオンにする
    setSelectedOnlyFlg(true, conductId, SelectedConductIdsForExecute);
    // 【オーバーライド用関数】実行ボタン前処理
    if (!preRegistProcess(appPath, transDiv, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, confirmNo)) {
        // 選択行のみを取得する機能の場合はフラグをオフに戻す
        setSelectedOnlyFlg(false, conductId, SelectedConductIdsForExecute);
        return false;
    }

    var ctrlId = $(btn).attr("name");
    var actionkbnW = $(btn).data('actionkbn');

    // 実行中フラグON
    P_ProcExecuting = true;
    // ボタンを非活性化
    //$(btn).prop("disabled", true);

    //対象エリアのエラー状態を初期化
    clearErrorStatusForAreas(isEdit);

    //処理中メッセージ：on
    processMessage(true);

    var listData = null;
    if (confirmNo == null || confirmNo < 0) {
        confirmNo = 0;
    }

    //ﾃﾞｰﾀ収集対象の一覧取得
    var targets = getTargetListElements(btn, isEdit);

    // 【オーバーライド用関数】登録処理前の「listData」個別取得処理
    listData = getListDataForRegist(appPath, conductId, pgmId, formNo, btn, listData);

    // 個別で取得していない場合は共通側で取得する
    if (!listData) {

        // 指定された機能ID・ボタンコントロールIDかどうか
        if (ConductIdGetSelectedRow.includes(conductId) && CtrlIdGetSelectedRow.includes(btn[0].attributes.name.value)) {
            // 選択されたレコードのみ取得
            listData = getListDataElements(targets, formNo, 0, true);    //ｺｰﾄﾞ値を採用
        }
        else {
            // 収集対象一覧の明細ﾃﾞｰﾀﾘｽﾄ(入力値)を取得
            listData = getListDataElements(targets, formNo, 0);    //ｺｰﾄﾞ値を採用
        }
    }

    // 【オーバーライド用関数】登録前追加条件取得処理
    var conditionDataList = addSearchConditionDictionaryForRegist(appPath, conductId, formNo, btn);

    // 個別で追加条件を取得していたら何もしない
    if (!conditionDataList.length) {
        if (false == isEdit) {
            //※条件＋明細画面の場合

            //==再検索時に必要な情報を取得する==
            //検索条件
            var tblSearch = getConditionTable();            //条件一覧要素
            //単票入力ﾎﾟｯﾌﾟｱｯﾌﾟの場合、明細一覧画面のｴﾘｱを取得
            if ($(P_Article).attr("name") == "edit_area") {
                var W_Article = $('article[name="main_area"][data-formno="' + formNo + '"]');
                tblSearch = $(W_Article).find(".search_div .ctrlId");
            }
            conditionDataList = getConditionData(formNo, tblSearch);   //条件ﾃﾞｰﾀ
        }
    }

    // 場所階層/職種機種データ取得
    var locationIdList = getSelectedStructureIdList(structureGroupDef.Location, treeViewDef.TreeMenu, false);
    var jobIdList = getSelectedStructureIdList(structureGroupDef.Job, treeViewDef.TreeMenu, true);

    //一覧ﾍﾟｰｼﾞ情報
    var W_listDefines = [];
    var isSet = true;
    $.each(P_listDefines, function (idx, define) {

        var defineW = $.extend(true, {}, define);
        //現行ﾍﾟｰｼﾞ番号
        var curPageNo = getCurrentPageNo(define.CTRLID, 'def');
        defineW.VAL1 = curPageNo;    //現在ﾍﾟｰｼﾞ
        W_listDefines.push(defineW);
    });

    /* ボタン権限制御 切替 start ================================================ */
    //var btnDefines = P_buttonDefine;
    /*  ================================================ */
    var btnDefines = P_buttonDefine[conductId];
    /* ボタン権限制御 切替 end ================================================== */

    // POSTデータを生成
    var postdata = {
        conductId: conductId,           // メニューの機能ID
        pgmId: pgmId,                   // メニューのプログラムID
        formNo: formNo,                 // 登録ボタンのフォーム番号
        ctrlId: ctrlId,                 // 登録ボタンのコントロールID
        transActionDiv: transDiv,       // 画面遷移アクション区分
        listData: listData,             // 一覧の入力データ
        ListIndividual: P_dicIndividual,   // 個別実装用汎用ﾘｽﾄ

        confirmNo: confirmNo,           //確認ﾒｯｾｰｼﾞ番号
        buttonDefines: btnDefines,  //ﾎﾞﾀﾝ権限情報　※ﾎﾞﾀﾝｽﾃｰﾀｽを取得

        browserTabNo: P_BrowserTabNo,   // ブラウザタブ識別番号

        //==再検索時に必要な情報を設定==
        conditionData: conditionDataList,   //検索条件
        listDefines: W_listDefines,     //一覧ﾍﾟｰｼﾞ情報(CtrlId, 1ﾍﾟｰｼﾞあたりのﾍﾟｰｼﾞ数)
        conductPtn: conductPtn,         //処理ﾊﾟﾀｰﾝ
        locationIdList: locationIdList, // 場所階層構成IDリスト
        jobIdList: jobIdList,           // 職種機種構成IDリスト
    };

    // 登録処理実行
    $.ajax({
        url: appPath + 'api/CommonProcApi/' + actionkbn.Execute,    // 実行
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(postdata),
        headers: { 'X-XSRF-TOKEN': getRequestVerificationToken() },
        traditional: true,
        cache: false
    }).then(
        // 1つめは通信成功時のコールバック
        function (resultInfo) {
            //正常時
            var status = resultInfo[0];                     //[0]:処理ステータス - CommonProcReturn
            var data = separateDicReturn(resultInfo[1], conductId);    //[1]:結果データ - Dictionary<string, object>※結果ﾃﾞｰﾀ："Result"、個別実装用ﾃﾞｰﾀ："Individual"
            /* ボタン権限制御 切替 start ================================================ */
            //var authShori = resultInfo[2];                  //[2]:処理ｽﾃｰﾀｽ  - IList<{ﾎﾞﾀﾝCTRLID：ｽﾃｰﾀｽ}> ※ｽﾃｰﾀｽ：0:非表示、1(表示(活性))、2(表示(非活性))
            /* ボタン権限制御 切替 end ================================================== */

            //完了ﾒｯｾｰｼﾞを表示
            setMessage(status.MESSAGE, status.STATUS);
            addMessageLogNo(status.LOGNO, status.STATUS);

            // 結果ﾃﾞｰﾀをｾｯﾄ
            /* ボタン権限制御 切替 start ================================================ */
            //setExecuteResults(appPath, conductId, pgmId, formNo, ctrlId, conductPtn, isEdit, autoBackFlg, data, authShori);
            //ﾀﾌﾞ内ﾎﾞﾀﾝか判定
            var tab = $(btn).closest(".tab_contents");
            var isTab = $(tab).length;
            setExecuteResults(appPath, conductId, pgmId, formNo, ctrlId, conductPtn, isEdit, autoBackFlg, data, status, isTab);
            /* ボタン権限制御 切替 end ================================================== */

            //【オーバーライド用関数】実行正常終了後処理
            postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data);

        },
        // 2つめは通信失敗時のコールバック
        function (resultInfo) {
            //異常時

            // ボタンを活性化
            $(btn).prop("disabled", false);

            //処理結果ｽﾃｰﾀｽ(CommonProcReturn)
            var result = resultInfo.responseJSON;
            var status;
            if (result.length > 1) {
                status = result[0];
                var data = separateDicReturn(result[1], conductId);

                //エラー詳細を表示
                dispErrorDetail(data);
            }
            else {
                status = result;
            }

            //処理継続用ｺｰﾙﾊﾞｯｸ関数を生成
            var confirmNo = 0;
            if (!isNaN(status.LOGNO)) {
                confirmNo = parseInt(status.LOGNO, 10);
            }
            var eventFunc = function () {
                clickRegistBtnConfirmOK(appPath, transDiv, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, confirmNo);
            }

            //【オーバーライド用関数】実行異常終了後処理
            postRegistProcessFailure(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data);

            //処理結果ｽﾃｰﾀｽを画面状態に反映
            if (!setReturnStatus(appPath, status, eventFunc)) {
                return false;
            }
        }
    ).always(
        //通信の完了時に必ず実行される
        function (resultInfo) {
            //処理中メッセージ：off
            processMessage(false);
            // 実行中フラグOFF
            P_ProcExecuting = false;
            //// ボタンを活性化
            $(btn).prop("disabled", false);
        });

    $('html,body').animate({ scrollTop: 0 }, '1');      //ｽｸﾛｰﾙを先頭へ移動
    if (isEdit) {
        $(P_Article).find(".edit_div").focus();
    }
    else {
        $(P_Article).find(".detail_div").focus();
    }
    // 選択行のみを取得する機能の場合はフラグをオフに戻す
    setSelectedOnlyFlg(false, conductId, SelectedConductIdsForExecute);
}

/**
 * 実行結果をｾｯﾄ
 *  @param {string}     appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string}     conductId   ：機能ID
 *  @param {string}     pgmId       ：プログラムID
 *  @param {number}     formNo      ：画面NO
 *  @param {string}     ctrlId      ：ﾎﾞﾀﾝｺﾝﾄﾛｰﾙID
 *  @param {number}     conductPtn  ：機能処理ﾊﾟﾀｰﾝ
 *  @param {boolean}    isEdit      ：単票表示フラグ
 *  @param {boolean}    autoBackFlg ：ajax正常終了後、自動戻るフラグ　false:戻らない、true:自動で戻る
 *  @param {object}     data        ：結果ﾃﾞｰﾀ
 *  @param {object}     status      ：結果ｽﾃｰﾀｽ
 *  @param {boolean}    isTab       ：タブ内フラグ
 */
/* ボタン権限制御 切替 start ================================================ */
//function setExecuteResults(appPath, conductId, pgmId, formNo, ctrlId, conductPtn, isEdit, autoBackFlg, data, authShori) {
//function setExecuteResults(appPath, conductId, pgmId, formNo, ctrlId, conductPtn, isEdit, autoBackFlg, data) {
function setExecuteResults(appPath, conductId, pgmId, formNo, ctrlId, conductPtn, isEdit, autoBackFlg, data, status, isTab) {
    /* ボタン権限制御 切替 end ================================================== */

    // 画面変更ﾌﾗｸﾞ初期化
    dataEditedFlg = false;
    // ｲﾍﾞﾝﾄを一旦解除
    if (isEdit) {
        setEventForEditFlg(false, null, "#" + P_formEditId);
    }
    else {
        setEventForEditFlg(false, null, "#" + P_formTopId);
        setEventForEditFlg(false, null, "#" + P_formDetailId);
        setEventForEditFlg(false, null, "#" + P_formBottomId);
    }

    //再検索結果を表示
    if (!isEdit) {
        //一覧(Index)画面の場合

        // 検索結果をクリア
        if (notSearchIdListForExecute.join(",").indexOf(conductId + "," + ctrlId) < 0) { //再検索を行わない機能のボタンは検索結果をクリアしない
            clearSearchResult();
        }

        // ページ状態の初期化
        initPageStatus();

        //検索結果ﾃﾞｰﾀを明細一覧に表示
        var pageRowCount = dispListData(appPath, conductId, pgmId, formNo, data, false);

        //ﾍﾟｰｼﾞの状態を検索後に設定
        setPageStatus(pageStatus.SEARCH, pageRowCount, conductPtn, isTab);
    } else {
        //明細入力(Edit)画面
        dispDataVertical(appPath, data, formNo, true);
    }
    //ﾎﾞﾀﾝ処理ｽﾃｰﾀｽを反映
    /* ボタン権限制御 切替 start ================================================ */
    //setButtonStatus(authShori);
    setButtonStatus();
    /* ボタン権限制御 切替 end ================================================== */

    // 列固定
    setFixColCss();

    // ﾌｫｰｶｽを再設定
    removeFocus(); // いったんクリア
    nextFocus();   // 再設定

    // 自動で戻るフラグ = trueの場合
    if (autoBackFlg) {
        autoBackAfterExec(appPath, ctrlId, status);
    }

    // ﾁｪﾝｼﾞｲﾍﾞﾝﾄで画面変更ﾌﾗｸﾞON
    if (isEdit) {
        setEventForEditFlg(true, null, "#" + P_formEditId);
    }
    else {
        setEventForEditFlg(true, null, "#" + P_formTopId);
        setEventForEditFlg(true, null, "#" + P_formDetailId);
        setEventForEditFlg(true, null, "#" + P_formBottomId);
    }
}

/**
 *  Excecl出力ボタンの初期化処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 */
function initReportBtn(appPath, btn) {

    // Excel出力ボタンクリックイベントハンドラの設定
    $(btn).on('click', function () {

        if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

        var form = $(this).closest("article").find("form[id^='form']");
        var conductIdW = $(form).find("input:hidden[name='CONDUCTID']").val();
        var pgmIdW = $(form).find("input:hidden[name='PGMID']").val();
        var conductPtnW = $(form).find("input:hidden[name='CONDUCTPTN']").val();
        var formNoW = $(P_Article).data("formno");

        //検索エリアのエラー状態を初期化(共通)
        var element = $(P_Article).find("form[id^='formSearch']");
        clearErrorcomStatus(element);

        // 検索条件ﾃﾞｰﾀの入力ﾁｪｯｸ
        if (!validConditionData()) {
            //入力ﾁｪｯｸｴﾗｰ
            return false;
        }

        //【オーバーライド用関数】Excel出力ﾁｪｯｸ処理 - 前処理
        if (!reportCheckPre(appPath, conductIdW, formNoW, this)) {
            return false;
        }

        var ctrlId = $(this).attr("name");          //ﾎﾞﾀﾝCtrlId
        var actionkbnW = $(this).data('actionkbn'); //ﾎﾞﾀﾝｱｸｼｮﾝ区分

        var confirmKbn = $(this).data("relationparam") + "";
        if (confirmKbn == null || confirmKbn.length <= 0) {
            confirmKbn = confirmKbnDef.Disp;
        }
        P_MessageStr = "";
        if (confirmKbn == confirmKbnDef.Disp) {
            //ﾃﾞﾌｫﾙﾄ確認ﾒｯｾｰｼﾞを設定
            P_MessageStr = P_ComMsgTranslated[941040003];  //『Excel出力を実行します。よろしいですか？』
            //ﾎﾞﾀﾝﾒｯｾｰｼﾞがあれば置き換え
            var btnMsg = getBtnMessage(this);
            if (btnMsg != null && btnMsg.length > 0) {
                P_MessageStr = btnMsg;
            }
        }

        var thisBtn = $(this); // イベント発生ボタン
        //処理継続用ｺｰﾙﾊﾞｯｸ関数を生成
        var eventFunc = function () {
            clickReportBtnConfirmOK(appPath, thisBtn, conductIdW, pgmIdW, formNoW, conductPtnW, ctrlId, actionkbnW);
        };

        // 確認メッセージ表示
        if (!popupMessage([P_MessageStr], messageType.Confirm, eventFunc, thisBtn)) {
            return false;
        }

        //※確認メッセージ「OK」の場合、下記関数処理を実施
        //イベント関数：clickReportBtnConfirmOK

    });
}

/**
 * 選択行のみ取得するフラグを変更する
 * @param {any} isOn フラグをオンにする場合はTRUE、オフにする場合はFALSE
 * @param {any} conductId 機能ID
 * @param {any} checkList このリスト内の機能なら対象とする
 */
function setSelectedOnlyFlg(isOn, conductId, checkList) {
    if (checkList.includes(conductId)) {
        // 指定された機能なら特定の一覧では選択行のみを取得する
        P_IsSelectedOnly = isOn;
    }
}

/**
 *  Excecl出力ボタン - 確認メッセージOK時、実行処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string} ：機能ID
 *  @param {string} ：プログラムID
 *  @param {number} ：画面番号
 *  @param {string} ：ボタンCtrlId
 *  @param {number} ：定義区分(1:一覧画面 2:明細入力画面 3:単票入力画面)
 *  @param {number} ：確認ﾒｯｾｰｼﾞ番号
 */
function clickReportBtnConfirmOK(appPath, btn, conductId, pgmId, formNo, conductPtn, btnCtrlId, actionkbnW, confirmNo) {
    // 指定された機能なら特定の一覧では選択行のみを取得する
    setSelectedOnlyFlg(true, conductId, SelectedConductIdsForOutput);
    //検索エリアのエラー状態を初期化
    var element = $(P_Article).find("#" + P_formSearchId);
    clearErrorStatus(element);

    var formDetail = $(P_Article).find("#" + P_formDetailId);
    setSubmitCtrlId(formDetail, btnCtrlId);         //submit用にﾎﾞﾀﾝCtrlIdをｾｯﾄ
    setSubmitDataCondition(formNo, formDetail);     //submit用に検索条件を取得してｾｯﾄ
    setSubmitDataList(formDetail, formNo);          //submit用に明細一覧データを取得してｾｯﾄ
    setSubmitDataIndividual(formDetail);          //submit用に個別実装用データを取得してｾｯﾄ
    //submit用にﾎﾞﾀﾝ定義情報ｾｯﾄ
    $(formDetail).find("input:hidden[name='ButtonDefines']").val(JSON.stringify(P_buttonDefine[conductId]));

    if (actionkbnW == actionkbn.Report) {
        // - 5:Excel出力

        // 実行中フラグON
        P_ProcExecuting = true;
        // ボタンを不活性化
        //$(btn).prop("disabled", true);

        // イベントを削除
        $(window).off('beforeunload');

        /*--同期処理--*/
        //reportDownload(appPath, conductId, pgmId, formNo, btnCtrlId);
        //reportCreate(appPath, conductId, pgmId, formNo, conductPtn, btnCtrlId, areakbnW);
        reportCreate(appPath, btn, conductId, pgmId, formNo, conductPtn, btnCtrlId);

        //$(formSearch).attr("action", appPath + "Common/ReportOut");
        //$(formSearch).submit();
        /*--同期処理--*/
    }
    else if (actionkbnW == actionkbn.ExcelPortDownload) {
        // - 313:ExceoPortダウンロード
        // 実行中フラグON
        P_ProcExecuting = true;
        // ボタンを不活性化
        $(btn).prop("disabled", true);

        // イベントを削除
        $(window).off('beforeunload');

        /*--ExcelPortダウンロード処理--*/
        excelPortDownload(appPath, btn, conductId, pgmId, formNo, conductPtn, btnCtrlId);
    }
    else {
        // - 51:Excel出力(非同期)～非同期タブを表示する

        /*--非同期処理--*/
        //↓↓【ﾊﾟﾀｰﾝ①】check/処理＋ﾀﾞｳﾝﾛｰﾄﾞ別ｳｨﾝﾄﾞｳでまとめて実施バージョン
        var childWindow = window.open('about:blank');

        //新規ﾀﾌﾞを開いてReport作成ﾌﾟﾛｼｰｼﾞｬｺｰﾙ、Excelﾀﾞｳﾝﾛｰﾄﾞ
        childWindow.location.href = appPath + 'Common/Report?CONDUCTID=' + conductId;
        childWindow = null;

        //↓↓【ﾊﾟﾀｰﾝ②】check/処理＋ﾀﾞｳﾝﾛｰﾄﾞ実施ｳｨﾝﾄﾞｳ分離バージョン
        //var conditionData = null;
        //if (confirmNo != null && confirmNo >= 1) {

        //} else {
        //    confirmNo = 0;

        //    // 検索条件取得
        //    var tblSearch = getConditionTable();            //条件一覧要素
        //    conditionData = getConditionData(formNo, tblSearch);   //条件ﾃﾞｰﾀ
        //}

        //// POSTデータを生成
        //var postdata = {
        //    conductId: conductId,                       // メニューの機能ID
        //    pgmId: pgmId,                               // メニューのプログラムID
        //    formNo: formNo,                             // 画面番号
        //    ctrlId: btnCtrlId,                          // Excel出力ボタンの画面定義のコントロールID
        //    conditionData: conditionData,               // 出力条件入力データ
        //    FileDownloadSet: FileDownloadSet.Hidouki,   // 1:非同期

        //    confirmNo: confirmNo,                       // 確認ﾒｯｾｰｼﾞ番号
        //};

        //var childWindow = window.open('about:blank');

        //// Excel出力処理実行
        ////　－条件ﾁｪｯｸ、出力ﾌｧｲﾙを作成して通知する
        ////  ※通信成功時、新規ﾀﾌﾞを開いて作成ﾌｧｲﾙのﾀﾞｳﾝﾛｰﾄﾞを行う。
        //$.ajax({
        //    url: appPath + 'api/CommonProcApi/' + actionkbn.Report,     // Excel出力
        //    type: 'POST',
        //    dataType: 'json',
        //    contentType: 'application/json',
        //    data: JSON.stringify(postdata),
        //    traditional: true,
        //    cache: false
        //}).then(
        //    // 1つめは通信成功時のコールバック
        //    function (resultInfo) {
        //        //[0]:処理ステータス - CommonProcReturn

        //        var status = resultInfo;

        //        //処理メッセージを表示
        //        if (status.MESSAGE != null && status.MESSAGE.length > 0) {
        //            //正常時、正常ﾒｯｾｰｼﾞ
        //            //警告、異常時、ｴﾗｰﾒｯｾｰｼﾞ
        //            setMessage(status.MESSAGE, status.STATUS);
        //        }

        //        //ﾎﾞﾀﾝCtrlIdをｾｯﾄ(子画面からの参照用)
        //        setSubmitCtrlId(formSearch, btnCtrlId);

        //        //新規ﾀﾌﾞを開いてExcelﾀﾞｳﾝﾛｰﾄﾞ
        //        childWindow.location.href = appPath + 'Common/Report?CONDUCTID=' + conductId;
        //        childWindow = null;

        //    },
        //// 2つめは通信失敗時のコールバック
        //function (resultInfo) {
        //    // 検索処理失敗
        //    childWindow.close();    //子画面を閉じる
        //    childWindow = null;

        //    //処理結果ｽﾃｰﾀｽ(CommonProcReturn)
        //    var status = resultInfo.responseJSON;
        //    //処理継続用ｺｰﾙﾊﾞｯｸ関数を生成
        //    var confirmNo = 0;
        //    if (!isNaN(status.LOGNO)) {
        //        confirmNo = parseInt(status.LOGNO, 10);
        //    }
        //    var eventFunc = function () {
        //        clickReportBtnConfirmOK(appPath, conductId, pgmId, formNo, btnCtrlId, actionkbnW, confirmNo);
        //    }

        //    //処理結果ｽﾃｰﾀｽを画面状態に反映
        //    if (!setReturnStatus(appPath, status, eventFunc)) {
        //        return false;
        //    }

        //});
        /*--非同期処理--*/
    }
    // 元に戻す
    setSelectedOnlyFlg(false, conductId, SelectedConductIdsForOutput);
}

/*--非同期タブ処理--*/
/**
 *  Excecl出力ボタン - 確認メッセージOK時、実行処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string} ：機能ID
 *  @param {string} ：プログラムID
 *  @param {number} ：画面番号
 *  @param {string} ：ボタンCtrlId
 *  @param {number} ：確認ﾒｯｾｰｼﾞ番号
 */
function reportDownload(appPath, conductId, pgmId, formNo, btnCtrlId, confirmNo) {

    //// メッセージを初期化
    //setMessage("Excel作成中...　しばらくお待ちください。", 0);
    ////処理中メッセージ：on
    //processMessage(true);

    // Excel出力処理実行
    //　－出力Check/出力ﾌﾟﾛｼｰｼﾞｬを実行
    //  ※通信成功時、Excel作成、およびﾀﾞｳﾝﾛｰﾄﾞを行う。
    //Excel作成、およびﾀﾞｳﾝﾛｰﾄﾞ
    var formDetail = $(P_Article).find("#" + P_formDetailId);
    setAttrByNativeJs(formDetail, "method", "POST");
    setAttrByNativeJs(formDetail, "action", appPath + "Common/Report?output=1");

    $(formDetail).submit();

    //if (confirmNo == null || confirmNo < 0) {
    //    confirmNo = 0;
    //}

    //// 検索条件取得
    //var tblSearch = getConditionTable();            //条件一覧要素
    //conditionData = getConditionData(formNo, tblSearch);   //条件ﾃﾞｰﾀ
    //var conditionDataList = [];
    //if (conditionData != null) {
    //    conditionDataList.push(conditionData);
    //}

    ////明細データ
    //listData = $.parseJSON($(P_Article).find("input:hidden[name='ListData']").val());

    //// POSTデータを生成
    //var postdata = {
    //    conductId: conductId,                       // メニューの機能ID
    //    pgmId: pgmId,                               // メニューのプログラムID
    //    formNo: formNo,                             // 画面番号
    //    ctrlId: btnCtrlId,                          // Excel出力ボタンの画面定義のコントロールID
    //    conditionData: conditionDataList,               // 出力条件入力データ
    //    ListData:listData,                          // 明細一覧データ
    //    //FileDownloadSet: FileDownloadSet.Hidouki,   // 1:非同期
    //    FileDownloadSet: FileDownloadSet.Douki,     // 0:同期

    //    confirmNo: confirmNo,                       // 確認ﾒｯｾｰｼﾞ番号
    //    buttonDefines: P_buttonDefine,
    //};

    //$.ajax({
    //    url: appPath + "Common/Report?output=1",     // Excel出力⇒ここでEXCEL作成、およびﾀﾞｳﾝﾛｰﾄﾞする
    //    type: 'POST',
    //    dataType: 'json',
    //    contentType: 'application/json',
    //    data: JSON.stringify(postdata),
    //    traditional: true,
    //    cache: false
    //}).then(
    //    // 1つめは通信成功時のコールバック
    //    function (resultInfo) {
    //        //[0]:処理ステータス - CommonProcReturn

    //        var status = resultInfo;

    //        // メッセージをクリア
    //        clearMessage();

    //        //処理メッセージを表示
    //        if (status.MESSAGE != null && status.MESSAGE.length > 0) {
    //            //正常時、正常ﾒｯｾｰｼﾞ
    //            //警告、異常時、ｴﾗｰﾒｯｾｰｼﾞ
    //            //addMessage(status.MESSAGE, status.STATUS);
    //            //↓↓※正常ﾒｯｾｰｼﾞは出力しない
    //            if (status.STATUS != procStatus.Valid) {
    //                addMessage(status.MESSAGE, status.STATUS);
    //                return false;
    //            }
    //        }

    //        //addMessage("　↓↓", 0);   //
    //        addMessage(P_ComMsgTranslated[941040002], 0);   //Excelファイルをダウンロードしました。
    //    },
    //    // 2つめは通信失敗時のコールバック
    //    function (resultInfo) {
    //        // 検索処理失敗

    //        // メッセージをクリア
    //        clearMessage();

    //        //処理結果ｽﾃｰﾀｽ(CommonProcReturn)
    //        var status = resultInfo.responseJSON;
    //        //処理継続用ｺｰﾙﾊﾞｯｸ関数を生成
    //        var confirmNo = 0;
    //        if (!isNaN(status.LOGNO)) {
    //            confirmNo = parseInt(status.LOGNO, 10);
    //        }
    //        var eventFunc = function () {
    //            reportDownload(appPath, conductId, pgmId, formNo, btnCtrlId, confirmNo);
    //        }

    //        //処理結果ｽﾃｰﾀｽを画面状態に反映
    //        if (!setReturnStatus(appPath, status, eventFunc)) {
    //            return false;
    //        }

    //    }
    //).always(
    //    //通信の完了時に必ず実行される
    //    function (resultInfo) {
    //        //処理中メッセージ：off
    //        processMessage(false);
    //    });

    ////処理中メッセージ：off
    //processMessage(false);

    $('html,body').animate({ scrollTop: 0 }, '1');      //ｽｸﾛｰﾙを先頭へ移動
    //$(P_Article).find("#search_divid").focus();
}
/*--非同期タブ処理--*/

/**
 *  Excecl出力ボタン - 確認メッセージOK時、実行処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string} ：機能ID
 *  @param {string} ：プログラムID
 *  @param {number} ：画面番号
 *  @param {string} ：ボタンCtrlId
 *  @param {number} ：定義区分(1:一覧画面 2:明細入力画面 3:単票入力画面)
 *  @param {number} ：確認ﾒｯｾｰｼﾞ番号
 */
//function reportCreate(appPath, conductId, pgmId, formNo, conductPtn, btnCtrlId, areakbnW, authkbnW, confirmNo) {
function reportCreate(appPath, btn, conductId, pgmId, formNo, conductPtn, btnCtrlId, confirmNo) {

    //処理中メッセージ：on
    processMessage(true);
    dispLoading();

    if (confirmNo == null || confirmNo < 0) {
        confirmNo = 0;
    }

    // 検索条件取得
    var tblSearch = getConditionTable();            //条件一覧要素
    var conditionDataList = [];
    conditionDataList = getConditionData(formNo, tblSearch); //条件ﾃﾞｰﾀ

    // 詳細検索条件データ取得(チェックされた条件のみ)
    var detailConditionDataList = getDetailConditionData(conductId, formNo);
    if (detailConditionDataList != null && detailConditionDataList.length > 0) {
        if (conditionDataList == null) {
            conditionDataList = [];
        }
        conditionDataList = conditionDataList.concat(detailConditionDataList);
    }

    var targetArea = $("nav#detail_search");
    // 「保存して次回検索時に使用する」のチェックが入っている場合、ローカルストレージへ保存する
    chkbox = $(targetArea).find("input[type='checkbox']#ComDetailSearchSave").filter(":checked");
    var saveDetailCondition = chkbox != null && chkbox.length > 0;

    if (saveDetailCondition) {
        // チェック状態を含めた条件データをLocalStorageへ保存
        detailConditionDataList = getDetailConditionData(conductId, formNo, null, false, true);
        if (detailConditionDataList != null && detailConditionDataList.length > 0) {
            $.each(detailConditionDataList, function (idx, condition) {
                setSaveDataToLocalStorage(detailConditionDataList, localStorageCode.DetailSearch, conductId, formNo, condition.CTRLID);
            });
        }
    }

    // 103(Tabulator)一覧の場合、読込件数上限をセット
    var listDefines = $.grep(P_listDefines, function (define, idx) {
        return (define.CTRLTYPE == ctrlTypeDef.IchiranPtn3);
    });
    $.each(listDefines, function (idx, define) {
        // 読込件数上限を取得＆設定
        define.VAL5 = getSelectCntMax(define.CTRLID, formNo);
    });

    // 場所階層/職種機種データ取得
    var locationIdList = getSelectedStructureIdList(structureGroupDef.Location, treeViewDef.TreeMenu, false);
    var jobIdList = getSelectedStructureIdList(structureGroupDef.Job, treeViewDef.TreeMenu, true);
    var saveTreeMenuCondition = true;
    if (saveTreeMenuCondition) {
        // LocalStorageへ保存
        //場所階層
        setSaveDataToLocalStorage(locationIdList, localStorageCode.LocationTree);
        //職種機種
        setSaveDataToLocalStorage(jobIdList, localStorageCode.JobTree);
    }

    //明細データ
    var listData = $(P_Article).find("input:hidden[name='ListData']").val();
    if (listData == null || listData == "") { return false; }
    var listDataJson = $.parseJSON(listData);

    /* ボタン権限制御 切替 start ================================================ */
    //var btnDefines = P_buttonDefine;
    /*  ================================================ */
    var btnDefines = P_buttonDefine[conductId];
    /* ボタン権限制御 切替 end ================================================== */

    //一覧ﾍﾟｰｼﾞ情報
    var W_listDefines = [];
    var isSet = true;
    $.each(P_listDefines, function (idx, define) {

        var defineW = $.extend(true, {}, define);
        //現行ﾍﾟｰｼﾞ番号
        var curPageNo = getCurrentPageNo(define.CTRLID, 'def');
        defineW.VAL1 = curPageNo;    //現在ﾍﾟｰｼﾞ
        W_listDefines.push(defineW);
    });

    // 検索条件シート出力用
    var W_listConditionName = [];
    var W_listConditionValue = [];

    var search_tr = document.getElementsByClassName('default_tr');

    // 全条件についてループ
    for (var i = 0; i < search_tr.length; i = i + 1) {

        // チェックボックスONの項目のみ取得
        if (search_tr[i].cells[0].getElementsByTagName("input")[0].checked) {

            // 条件項目名の取得
            W_listConditionName.push(search_tr[i].cells[1].innerText);

            // 条件値の取得
            if (search_tr[i].cells[2].getElementsByClassName("date_div").length > 0) {
                var valFr = search_tr[i].cells[2].getElementsByTagName("input")[0].value;
                var valTo = search_tr[i].cells[2].getElementsByTagName("input")[1].value;
                W_listConditionValue.push(valFr + " ～ " + valTo);                                          // 範囲(数値、日付)の場合
            } else if (search_tr[i].cells[2].getElementsByTagName("a").length > 0) {
                W_listConditionValue.push(search_tr[i].cells[2].getElementsByTagName("a")[0].innerText);    // 選択の場合
            } else {
                W_listConditionValue.push(search_tr[i].cells[2].getElementsByTagName("input")[0].value);    // テキスト入力の場合
            }
        }
    }

    // POSTデータを生成
    var postdata = {
        conductId: conductId,                       // メニューの機能ID
        pgmId: pgmId,                               // メニューのプログラムID
        formNo: formNo,                             // 画面番号
        ctrlId: btnCtrlId,                          // Excel出力ボタンの画面定義のコントロールID
        conditionData: conditionDataList,           // 出力条件入力データ
        ListData: listDataJson,                     // 明細一覧データ
        listDefines: W_listDefines,                 // 一覧定義情報
        ListIndividual: P_dicIndividual,            // 個別実装用汎用ﾘｽﾄ
        FileDownloadSet: FileDownloadSet.Hidouki,   // 1:非同期
        //FileDownloadSet: FileDownloadSet.Douki,     // 0:同期

        confirmNo: confirmNo,                       // 確認ﾒｯｾｰｼﾞ番号
        buttonDefines: btnDefines,

        browserTabNo: P_BrowserTabNo,   // ブラウザタブ識別番号

        locationIdList: locationIdList,         // 場所階層構成IDリスト
        jobIdList: jobIdList,     // 職種機種構成IDリスト

        ListConditionName: W_listConditionName,     // 検索条件項目名リスト
        ListConditionValue: W_listConditionValue,   // 検索条件設定値リスト
    };



    $.ajax({
        //url: appPath + "Common/Report?output=1",     // Excel出力⇒ここでEXCEL作成、およびﾀﾞｳﾝﾛｰﾄﾞする
        url: appPath + 'api/CommonProcApi/' + actionkbn.Report,   // Excel作成
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(postdata),
        headers: { 'X-XSRF-TOKEN': getRequestVerificationToken() },
        traditional: true,
        cache: false
    }).then(
        // 1つめは通信成功時のコールバック
        function (resultInfo) {
            //正常時
            var status = resultInfo[0];                     //[0]:処理ステータス - CommonProcReturn
            var data = separateDicReturn(resultInfo[1], conductId);    //[1]:結果データ - Dictionary<string, object>※結果ﾃﾞｰﾀ："Result"、個別実装用ﾃﾞｰﾀ："Individual"
            /* ボタン権限制御 切替 start ================================================ */
            //var authShori = resultInfo[2];                  //[2]:処理ｽﾃｰﾀｽ  - IList<{ﾎﾞﾀﾝCTRLID：ｽﾃｰﾀｽ}> ※ｽﾃｰﾀｽ：0:非表示、1(表示(活性))、2(表示(非活性))
            /* ボタン権限制御 切替 end ================================================== */

            // メッセージをクリア
            clearMessage();

            //処理メッセージを表示
            if (status.MESSAGE != null && status.MESSAGE.length > 0) {
                //正常時、正常ﾒｯｾｰｼﾞ
                //警告、異常時、ｴﾗｰﾒｯｾｰｼﾞ
                ////addMessage(status.MESSAGE, status.STATUS);
                ////↓↓※正常ﾒｯｾｰｼﾞは出力しない
                //if (status.STATUS != procStatus.Valid) {
                //    addMessage(status.MESSAGE, status.STATUS);
                //    return false;
                //}
                if (status.MESSAGE == P_ComMsgTranslated[941060001]) {
                    // 「該当データがありません。」の場合はステータスをErrorに設定し直す
                    // (ファイルダウンロード処理を実行させるため、ロジック側からはValidで渡ってくるため)
                    addMessage(status.MESSAGE, procStatus.Error);
                }
                else {
                    addMessage(status.MESSAGE, status.STATUS);
                }
            }

            if (data != null) {
                // 結果データが存在する場合、再検索結果の反映
                /* ボタン権限制御 切替 start ================================================ */
                //setExecuteResults(appPath, conductId, pgmId, formNo, btnCtrlId, conductPtn, false, false, data, authShori);
                setExecuteResults(appPath, conductId, pgmId, formNo, btnCtrlId, conductPtn, false, false, data);
                /* ボタン権限制御 切替 end ================================================== */
            } else if (data == null && P_buttonDefine[conductId] != null && P_buttonDefine[conductId].length != 0) {
                // 結果データが存在せず、ボタン情報が存在する場合、ボタンの権限制御のみ実施
                setButtonStatus();
            }
            // 【オーバーライド用関数】Excel出力後処理
            postOutputExcel(appPath, conductId, pgmId, formNo, btnCtrlId);

            // ダウンロードファイル名
            var fileName = status.FILEDOWNLOADNAME;
            var filePath = status.FILEPATH;

            if (fileName != null && fileName.length > 0) {
                // ダウンロードファイル名が指定されている場合のみ、ダウンロード処理実行
                var formDetail = $(P_Article).find("#" + P_formDetailId);
                setAttrByNativeJs(formDetail, "method", "POST");
                setAttrByNativeJs(formDetail, "action", appPath + "Common/Report?output=2&fileName=" + fileName + "&filePath=" + filePath);

                $(formDetail).submit();
            }
            // 画面変更ﾌﾗｸﾞ初期化
            dataEditedFlg = false;

        },
        // 2つめは通信失敗時のコールバック
        function (resultInfo) {
            // 検索処理失敗

            // ボタンを活性化
            $(btn).prop("disabled", false);

            // メッセージをクリア
            clearMessage();

            //処理結果ｽﾃｰﾀｽ(CommonProcReturn)
            var result = resultInfo.responseJSON;
            var status;
            if (result && result.length > 1) {
                status = result[0];
                var data = separateDicReturn(result[1], conductId);

                //エラー詳細を表示
                dispErrorDetail(data);
            }
            else if (!result) {
                status = {
                    STATUS: procStatus.Error,
                    MESSAGE: getMessageParam(P_ComMsgTranslated[941220002], [P_ComMsgTranslated[911040001]]) //Excel出力に失敗しました。
                }
            }
            else {
                status = result;
            }

            //処理結果ｽﾃｰﾀｽ(CommonProcReturn)
            //var status = resultInfo.responseJSON;
            //処理継続用ｺｰﾙﾊﾞｯｸ関数を生成
            var confirmNo = 0;
            if (!isNaN(status.LOGNO)) {
                confirmNo = parseInt(status.LOGNO, 10);
            }
            var eventFunc = function () {
                reportCreate(appPath, btn, conductId, pgmId, formNo, conductPtn, btnCtrlId, confirmNo);
            }

            //処理結果ｽﾃｰﾀｽを画面状態に反映
            if (!setReturnStatus(appPath, status, eventFunc)) {
                return false;
            }

        }
    ).always(
        //通信の完了時に必ず実行される
        function (resultInfo) {
            //処理中メッセージ：off
            processMessage(false);
            // 実行中フラグOFF
            P_ProcExecuting = false;
            // ボタンを活性化
            $(btn).prop("disabled", false);
        });

    ////処理中メッセージ：off
    //processMessage(false);

    $('html,body').animate({ scrollTop: 0 }, '1');      //ｽｸﾛｰﾙを先頭へ移動
    //$(P_Article).find("#search_divid").focus();
}

/**
 *  ExceclPortダウンロードボタン - 確認メッセージOK時、実行処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string} ：機能ID
 *  @param {string} ：プログラムID
 *  @param {number} ：画面番号
 *  @param {string} ：ボタンCtrlId
 *  @param {number} ：定義区分(1:一覧画面 2:明細入力画面 3:単票入力画面)
 *  @param {number} ：確認ﾒｯｾｰｼﾞ番号
 */
function excelPortDownload(appPath, btn, conductId, pgmId, formNo, conductPtn, btnCtrlId, confirmNo) {

    //処理中メッセージ：on
    processMessage(true);
    dispLoading();

    if (confirmNo == null || confirmNo < 0) {
        confirmNo = 0;
    }

    // 出力条件取得
    var conditionDataList = [];
    if (formNo == 0) {
        // ExcelPortダウンロード一覧画面の場合、選択行データから取得
        var rowNo = $(btn).closest("div.tabulator-row").find("div[tabulator-field=ROWNO]")[0].innerText;
        var tblId = "#" + $(btn).closest("div.ctrlId").attr("id");
        // フィルターを掛けている場合を考慮して以下で行番号を取得
        rowNo = P_listData[tblId].getRowFromPosition(parseInt(rowNo, 10), true).getData().ROWNO;
        conditionDataList.push(getTempDataForTabulator(formNo, rowNo, tblId));

    } else {
        // 条件画面からのダウンロードの場合、条件エリアから取得
        var tblSearch = getConditionTable();            //条件一覧要素
        conditionDataList = getConditionData(formNo, tblSearch); //条件ﾃﾞｰﾀ
    }
    // 実行対象のプログラムIDは選択行の隠し項目から取得
    var pgmIdW = conditionDataList[0]["VAL5"];

    // 場所階層/職種機種データ取得
    var locationIdList = getSelectedStructureIdList(structureGroupDef.Location, treeViewDef.TreeMenu, false);
    var jobIdList = getSelectedStructureIdList(structureGroupDef.Job, treeViewDef.TreeMenu, true);
    var saveTreeMenuCondition = true;
    if (saveTreeMenuCondition) {
        // LocalStorageへ保存
        //場所階層
        setSaveDataToLocalStorage(locationIdList, localStorageCode.LocationTree);
        //職種機種
        setSaveDataToLocalStorage(jobIdList, localStorageCode.JobTree);
    }

    var btnDefines = P_buttonDefine[conductId];

    // POSTデータを生成
    var postdata = {
        conductId: conductId,                       // メニューの機能ID
        pgmId: pgmIdW,                              // 実行対象のプログラムID
        formNo: formNo,                             // 画面番号
        ctrlId: btnCtrlId,                          // Excel出力ボタンの画面定義のコントロールID
        conditionData: conditionDataList,           // 出力条件入力データ
        ListIndividual: P_dicIndividual,            // 個別実装用汎用ﾘｽﾄ
        FileDownloadSet: FileDownloadSet.Hidouki,   // 1:非同期

        confirmNo: confirmNo,                       // 確認ﾒｯｾｰｼﾞ番号
        buttonDefines: btnDefines,

        browserTabNo: P_BrowserTabNo,   // ブラウザタブ識別番号

        locationIdList: locationIdList,         // 場所階層構成IDリスト
        jobIdList: jobIdList,     // 職種機種構成IDリスト
    };

    $.ajax({
        url: appPath + 'api/CommonProcApi/' + actionkbn.ExcelPortDownload,   // ExcelPortダウンロード
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(postdata),
        headers: { 'X-XSRF-TOKEN': getRequestVerificationToken() },
        traditional: true,
        cache: false
    }).then(
        // 1つめは通信成功時のコールバック
        function (resultInfo) {
            //正常時
            var status = resultInfo[0];                     //[0]:処理ステータス - CommonProcReturn
            var data = separateDicReturn(resultInfo[1], conductId);    //[1]:結果データ - Dictionary<string, object>※結果ﾃﾞｰﾀ："Result"、個別実装用ﾃﾞｰﾀ："Individual"

            // メッセージをクリア
            clearMessage();

            //処理メッセージを表示
            if (status.MESSAGE != null && status.MESSAGE.length > 0) {
                addMessage(status.MESSAGE, status.STATUS);
            }

            var afterExecKbn = $(btn).data("afterexeckbn"); //  実行処理後区分
            if (formNo > 0 && afterExecKbn != null && afterExecKbn == afterExecKbnDef.AutoBack) {
                // 自動で戻る場合、実行結果のセット
                setExecuteResults(appPath, conductId, pgmId, formNo, btnCtrlId, conductPtn, false, true, data, status);
            } else {
                // ボタンの権限制御のみ実施
                setButtonStatus();
            }


            // ダウンロードファイル名
            var fileName = status.FILEDOWNLOADNAME;
            var filePath = status.FILEPATH;

            if (fileName != null && fileName.length > 0) {
                // ダウンロードファイル名が指定されている場合のみ、ダウンロード処理実行
                var formDetail = $(P_Article).find("#" + P_formDetailId);
                setAttrByNativeJs(formDetail, "method", "POST");
                setAttrByNativeJs(formDetail, "action", appPath + "Common/Report?output=2&fileName=" + fileName + "&filePath=" + filePath);

                $(formDetail).submit();
            }
            // 画面変更ﾌﾗｸﾞ初期化
            dataEditedFlg = false;

        },
        // 2つめは通信失敗時のコールバック
        function (resultInfo) {
            // 検索処理失敗

            // ボタンを活性化
            $(btn).prop("disabled", false);

            // メッセージをクリア
            clearMessage();

            //処理結果ｽﾃｰﾀｽ(CommonProcReturn)
            var result = resultInfo.responseJSON;
            var status;
            if (result.length > 1) {
                status = result[0];
                var data = separateDicReturn(result[1], conductId);

                //エラー詳細を表示
                dispErrorDetail(data);
            }
            else {
                status = result;
            }

            //処理結果ｽﾃｰﾀｽ(CommonProcReturn)
            //var status = resultInfo.responseJSON;
            //処理継続用ｺｰﾙﾊﾞｯｸ関数を生成
            var confirmNo = 0;
            if (!isNaN(status.LOGNO)) {
                confirmNo = parseInt(status.LOGNO, 10);
            }
            var eventFunc = function () {
                reportCreate(appPath, btn, conductId, pgmId, formNo, conductPtn, btnCtrlId, confirmNo);
            }

            //処理結果ｽﾃｰﾀｽを画面状態に反映
            if (!setReturnStatus(appPath, status, eventFunc)) {
                return false;
            }
        }
    ).always(
        //通信の完了時に必ず実行される
        function (resultInfo) {
            //処理中メッセージ：off
            processMessage(false);
            // 実行中フラグOFF
            P_ProcExecuting = false;
            // ボタンを活性化
            $(btn).prop("disabled", false);
        });

    $('html,body').animate({ scrollTop: 0 }, '1');      //ｽｸﾛｰﾙを先頭へ移動
}

/**
 *  削除ボタンの初期化処理
 *  @param {string} appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} btn         ：対象ボタン
 */
function initDeleteBtn(appPath, btn) {

    // 削除ボタンクリックイベントハンドラの設定
    $(btn).on('click', function () {

        if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

        //単票表示ｴﾘｱか判定
        var isEdit = judgeBtnIsEditPosition(this);

        var form = $(this).closest("article").find("form[id^='form']");
        var conductIdW = $(form).find("input:hidden[name='CONDUCTID']").val();
        var pgmIdW = $(form).find("input:hidden[name='PGMID']").val();
        var conductPtnW = $(form).find("input:hidden[name='CONDUCTPTN']").val();
        var formNoW = $(P_Article).data("formno");

        //明細エリアのエラー状態を初期化(共通)
        clearErrorComStatusForAreas(isEdit);

        var thisBtn = $(this);

        //ﾒｯｾｰｼﾞ設定
        var confirmKbn = $(thisBtn).data("relationparam") + "";
        setMessageStrForBtn(thisBtn, confirmKbn);

        //処理継続用ｺｰﾙﾊﾞｯｸ関数を生成
        var eventFunc = function () {
            //clickDeleteBtnConfirmOK(appPath, conductIdW, pgmIdW, areaKbn, formNoW, conductPtnW, thisBtn, isEdit);
            clickDeleteBtnConfirmOK(appPath, conductIdW, pgmIdW, formNoW, conductPtnW, thisBtn, isEdit);
        }
        // 確認メッセージ表示
        if (!popupMessage([P_MessageStr], messageType.Confirm, eventFunc, thisBtn)) {
            return false;
        }

        //※確認メッセージ「OK」の場合、下記関数処理を実施
        //イベント関数：clickDeleteBtnConfirmOK

    });
}

/**
 *  削除ボタン - 確認メッセージOK時、実行処理
 *  @param {string} appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string} conductId   ：機能ID
 *  @param {string} pgmId       ：プログラムID
 *  @param {number} formNo      ：画面番号
 *  @param {byte}   conductPtn  ：画面ﾊﾟﾀｰﾝ
 *  @param {element}btn         ：ﾎﾞﾀﾝ要素
 *  @param {boolean}isEdit      ：単票ｴﾘｱﾌﾗｸﾞ
 *  @param {int}    confirmNo   ：確認番号(ﾃﾞﾌｫﾙﾄ：0)
 */
//function clickDeleteBtnConfirmOK(appPath, conductId, pgmId, areaKbn, formNo, conductPtn, ctrlId, confirmNo) {
function clickDeleteBtnConfirmOK(appPath, conductId, pgmId, formNo, conductPtn, btn, isEdit, confirmNo) {
    // 【オーバーライド用関数】削除ボタン前処理
    if (!preDeleteProcess(appPath, conductId, pgmId, formNo, conductPtn, btn, isEdit, confirmNo)) {
        return false;
    }

    // 実行中フラグON
    P_ProcExecuting = true;
    // ボタンを不活性化
    //$(btn).prop("disabled", true);

    var ctrlId = $(btn).attr("name");

    //対象エリアのエラー状態を初期化
    clearErrorStatusForAreas(isEdit);

    //処理中メッセージ：on
    processMessage(true);

    if (confirmNo == null || confirmNo < 0) {
        confirmNo = 0;
    }

    var listData = null;
    //// ﾄｯﾌﾟ・明細・ﾎﾞﾄﾑｴﾘｱの全一覧のﾃﾞｰﾀを取得
    //listData = getListDataAll(formNo, 0);    //ｺｰﾄﾞ値を採用
    //ﾃﾞｰﾀ収集対象の一覧取得
    var targets = getTargetListElements(btn, isEdit);
    // 収集対象一覧の明細ﾃﾞｰﾀﾘｽﾄ(入力値)を取得
    listData = getListDataElements(targets, formNo, 0);    //ｺｰﾄﾞ値を採用

    /* ボタン権限制御 切替 start ================================================ */
    //var btnDefines = P_buttonDefine;
    /*  ================================================ */
    var btnDefines = P_buttonDefine[conductId];
    /* ボタン権限制御 切替 end ================================================== */

    // POSTデータを生成
    var postdata = {
        conductId: conductId,           // メニューの機能ID
        pgmId: pgmId,                   // メニューのプログラムID
        formNo: formNo,                 // 登録ボタンのフォーム番号
        ctrlId: ctrlId,                 // 登録ボタンのコントロールID
        listData: listData,             // 一覧の入力データ
        ListIndividual: P_dicIndividual,           // 個別実装用汎用ﾘｽﾄ

        confirmNo: confirmNo,           //確認ﾒｯｾｰｼﾞ番号
        buttonDefines: btnDefines,  //ﾎﾞﾀﾝ権限情報　※ﾎﾞﾀﾝｽﾃｰﾀｽを取得

        browserTabNo: P_BrowserTabNo,   // ブラウザタブ識別番号
    };

    // 削除処理実行
    $.ajax({
        url: appPath + 'api/CommonProcApi/' + actionkbn.Delete,    // 実行
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(postdata),
        headers: { 'X-XSRF-TOKEN': getRequestVerificationToken() },
        traditional: true,
        cache: false
    }).then(
        // 1つめは通信成功時のコールバック
        function (resultInfo) {
            //正常時
            var status = resultInfo[0];                     //[0]:処理ステータス - CommonProcReturn
            var data = separateDicReturn(resultInfo[1], conductId);    //[1]:結果データ - Dictionary<string, object>※結果ﾃﾞｰﾀ："Result"、個別実装用ﾃﾞｰﾀ："Individual"
            /* ボタン権限制御 切替 start ================================================ */
            //var authShori = resultInfo[2];                  //[2]:処理ｽﾃｰﾀｽ  - IList<{ﾎﾞﾀﾝCTRLID：ｽﾃｰﾀｽ}> ※ｽﾃｰﾀｽ：0:非表示、1(表示(活性))、2(表示(非活性))
            /* ボタン権限制御 切替 end ================================================== */

            //完了ﾒｯｾｰｼﾞを表示
            setMessage(status.MESSAGE, status.STATUS);
            addMessageLogNo(status.LOGNO, status.STATUS);

            //ﾀﾌﾞ内ﾎﾞﾀﾝか判定
            var tab = $(btn).closest(".tab_contents");
            var isTab = $(tab).length;
            //ﾍﾟｰｼﾞの状態を検索後に設定
            setPageStatus(pageStatus.SEARCH, -1, conductPtn, isTab);
            //ﾎﾞﾀﾝｽﾃｰﾀｽ設定
            //setButtonStatus(authShori);
            setButtonStatus();
        },
        // 2つめは通信失敗時のコールバック
        function (resultInfo) {
            //異常時

            //処理結果ｽﾃｰﾀｽ(CommonProcReturn)
            var result = resultInfo.responseJSON;
            var status;
            if (result.length > 1) {
                status = result[0];
                var data = separateDicReturn(result[1], conductId);

                //エラー詳細を表示
                dispErrorDetail(data);
            }
            else {
                status = result;
            }

            //処理継続用ｺｰﾙﾊﾞｯｸ関数を生成
            var confirmNo = 0;
            if (!isNaN(status.LOGNO)) {
                confirmNo = parseInt(status.LOGNO, 10);
            }
            var eventFunc = function () {
                clickDeleteBtnConfirmOK(appPath, conductId, pgmId, formNo, conductPtn, btn, isEdit, confirmNo);
            }
            //処理結果ｽﾃｰﾀｽを画面状態に反映
            if (!setReturnStatus(appPath, status, eventFunc)) {
                return false;
            }
        }
    ).always(
        //通信の完了時に必ず実行される
        function (resultInfo) {
            //処理中メッセージ：off
            processMessage(false);
            // 実行中フラグOFF
            P_ProcExecuting = false;
            //// ボタンを活性化
            $(btn).prop("disabled", false);
        });

}

/**
 *  戻る/閉じるボタンの初期化処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 *  @param {bool}   ：閉じるボタンの場合True
 */
function initBackBtn(appPath, btn, isClose) {

    // 戻るボタンクリックイベントハンドラの設定

    //ｸﾘｯｸ時ｲﾍﾞﾝﾄ処理を設定
    $(btn).on('click', function () {

        if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

        var element = $(this).closest(".ctrlId[id$='_edit']");   //単票入力画面ﾎﾞﾀﾝｴﾘｱの配置か？
        var btnCtrlId = $(this).attr("name");

        var confirmKbn = $(this).data("relationparam") + "";
        if (confirmKbn == null || confirmKbn.length <= 0) {
            confirmKbn = confirmKbnDef.Disp;
        }

        //ボタン押下時の処理
        var eventFunc = function () {
            if (isClose) {
                // 閉じる
                window.close();
            } else {
                // 戻る
                divisionBackBtn(appPath, element, btnCtrlId);
            }
            //【オーバーライド用関数】閉じる処理の後
            postBackBtnProcess();
        }

        if (confirmKbn == confirmKbnDef.Disp && dataEditedFlg) {
            // 画面変更ﾌﾗｸﾞONの場合、ﾃﾞｰﾀ破棄確認ﾒｯｾｰｼﾞ

            //ﾃﾞﾌｫﾙﾄ確認ﾒｯｾｰｼﾞを設定
            //『画面の編集内容は破棄されます。よろしいですか？』
            P_MessageStr = P_ComMsgTranslated[941060005];
            //ﾎﾞﾀﾝﾒｯｾｰｼﾞがあれば置き換え
            var btnMsg = getBtnMessage(this);
            if (btnMsg != null && btnMsg.length > 0) {
                P_MessageStr = btnMsg;
            }
            // ﾃﾞﾌｫﾙﾄ確認ﾒｯｾｰｼﾞを表示
            //『～します。よろしいですか？』
            if (!popupMessage([P_MessageStr], messageType.Confirm, eventFunc)) {
                //『キャンセル』の場合、処理中断
                return false;
            }

        }
        else {
            // 戻る処理の分岐
            eventFunc();
        }
    });

}

/**
 * 戻るボタンの初期化(ﾎﾟｯﾌﾟｱｯﾌﾟ時専用Ver.)
 * @param {string}  appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 * @param {button}  backBtn     ：対象ﾎﾞﾀﾝ
 * @param {number}  transPtn    ：画面遷移ﾊﾟﾀｰﾝ(2：単票入力、4：共通機能)
 */
function initBackBtnForPopup(appPath, backBtn, transPtn) {
    if ($(backBtn).length) {
        //既存のｸﾘｯｸｲﾍﾞﾝﾄを削除
        $(backBtn).off('click');

        if (transPtn == transPtnDef.CmConduct) {   //※共通機能の場合
            //新たに専用のｸﾘｯｸｲﾍﾞﾝﾄを付与
            $(backBtn).on('click', function (e) {

                if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

                // ﾎﾟｯﾌﾟｱｯﾌﾟ閉じる
                var modal = $(this).closest('section.modal_form');
                $(modal).modal('hide');

                //【オーバーライド用関数】閉じる処理の後(ポップアップ画面用)
                postBackBtnProcessForPopup();
            });
        }
        else {  //※子画面 or 単票入力の場合
            //新たに専用のｸﾘｯｸｲﾍﾞﾝﾄを付与
            $(backBtn).on('click', function (e) {

                if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

                const modal = $(e.currentTarget).closest('section.modal_form');
                if (dataEditedFlg) {
                    // 画面変更ﾌﾗｸﾞONの場合、ﾃﾞｰﾀ破棄確認ﾒｯｾｰｼﾞをﾎﾟｯﾌﾟｱｯﾌﾟ
                    P_MessageStr = P_ComMsgTranslated[941060005]; //『画面の編集内容は破棄されます。よろしいですか？』
                    // 確認OK時処理を設定
                    var eventFunc = function () {
                        // ﾎﾟｯﾌﾟｱｯﾌﾟ閉じる
                        $(modal).modal('hide');
                    }
                    // 確認ﾒｯｾｰｼﾞを表示
                    if (!popupMessage([P_MessageStr], messageType.Confirm, eventFunc)) {
                        //『キャンセル』の場合、処理中断
                        return false;
                    }
                }
                else {
                    // ﾎﾟｯﾌﾟｱｯﾌﾟ閉じる
                    $(modal).modal('hide');
                }
            });
        }
    }
}

/**
 *  戻る処理の分岐
 */
function divisionBackBtn(appPath, element, btnCtrlId, status) {
    if ($(element).length <= 0) {
        //※子画面の場合
        clickBackBtnForChildForm(appPath, btnCtrlId, status, 0);
    }
    else {
        //※単票入力ﾎﾟｯﾌﾟｱｯﾌﾟの場合
        clickBackBtnForEditPopup(appPath, btnCtrlId, status);
    }
}

/**
 * 子画面からの戻る処理
 *
 */
function clickBackBtnForChildForm(appPath, btnCtrlId, status, parentModalNo) {
    // 子画面 - 戻るﾎﾞﾀﾝｸﾘｯｸ時、ｲﾍﾞﾝﾄ処理

    var getData = skipGetDataDef.GetData;
    // 【オーバーライド用関数】戻る処理の前(単票、子画面共用)
    if (!prevBackBtnProcess(appPath, btnCtrlId, status)) {
        //データ取得をスキップ
        getData = skipGetDataDef.skip;
    }
    // 【オーバーライド用関数】戻る処理の前(子画面共用)
    parentModalNo = prevBackBtnProcessForChild(appPath, btnCtrlId, status, parentModalNo);
    if (parentModalNo < 0) {
        return;
    }

    var form = $(P_Article).find("form[id^='formDetail']");

    var conductId = $(form).find('input[name="CONDUCTID"]').val()
    var pgmId = $(form).find('input[name="PGMID"]').val();
    var conductPtn = $(form).find('input[name="CONDUCTPTN"]').val();
    //var parentNo = $(form).find('input[name="ORIGINNO"]').val();   //親画面番号
    //if (parentNo == null || parentNo.length <= 0) {
    //    //他機能遷移の場合はORIGINNOが空→ﾎﾞﾀﾝに保持
    //    parentNo = $(P_Article).find("input:button[name='" + btnCtrlId + "']").attr("data-relationid");
    //}
    var relationId = $(P_Article).find("input:button[name='" + btnCtrlId + "']").attr("data-relationid");
    var parentNo = $(form).find('input[name="ORIGINNO"]').val();   //親画面番号
    if (parentNo == null || parentNo.length <= 0) {
        //他機能遷移の場合はORIGINNOが空→ﾎﾞﾀﾝに保持
        parentNo = relationId;
    } else if (relationId != null && relationId.length > 0 && relationId != '-') {
        // ボタンの関連情報IDに戻り先の画面番号が指定されている場合はそちらを親画面番号として使用する
        parentNo = relationId;
    }
    if (parentNo == null || parentNo.length <= 0 || parentNo == '-') {
        // 取得できない場合、現在の画面番号-1を設定
        const formNo = $(form).find('input[name="FORMNO"]').val();
        parentNo = formNo - 1;
        if (parentNo < 0) {
            parentNo = 0;
        }
    }

    //※子画面非表示
    // - 条件ｴﾘｱ一覧
    var searchDivId = $(P_Article).find(".search_div").attr("id");
    clearSearchResult(searchDivId + ' table');    //条件一覧クリア    
    clearSearchResult();    //検索結果クリア
    clearArticle(true); //画面一括クリア
    //ｴﾗｰ情報初期化
    clearErrorcomStatus(P_Article);
    //画面ｴﾘｱ非表示
    $(P_Article).removeClass('selected');

    //==選択中のタブ番号と選択状態のクリア==
    // タブ切替用ボタンを取得
    var btnIds = [".tab_btn.search", ".tab_btn.detail"];
    $.each(btnIds, function (idx, id) {
        var tabBtns = $(P_Article).find(id + " a");
        if (tabBtns.length > 0) {
            // タブボタンの選択状態をクリア
            $(tabBtns).removeClass('selected');
            // 選択中のタブ番号キーを取得＆選択中のタブ番号をクリア
            var tabNoKey = getSelectedFormTabNoKey(tabBtns[0]);
            clearSelectedFormTabNo(tabNoKey);
        }
    });
    // タブ内容を取得
    var tabContents = $(P_Article).find('.tab_contents');
    if (tabContents.length > 0) {
        // タブ内容の選択状態をクリア
        $(tabContents).removeClass('selected');
    }

    if (parentModalNo == 0) {
        //※親画面以外非表示
        $('article[name="main_area"][data-formno!="' + parentNo + '"]').removeClass('selected');

        //※親画面表示
        $('article[name="main_area"][data-formno="' + parentNo + '"]').addClass('selected');

    } else {
        //※親のモーダル画面表示
        $('article[name="main_area"][data-formno="' + parentNo + '"]').has('section.modal_form').addClass('selected');
    }
    //親画面に遷移した時の条件を渡す
    var selectData = getSearchCondition(conductId, Number(parentNo));   // ﾘｽﾄ型で保持しているのでそのまま渡す

    // 画面変更ﾌﾗｸﾞ初期化
    dataEditedFlg = false;

    //戻るｱｸｼｮﾝﾎﾞﾀﾝのCTRLIDで画面初期化時ｱｸｼｮﾝの業務ﾛｼﾞｯｸを呼び出す
    initForm(appPath, conductId, pgmId, parentNo, -1, btnCtrlId, conductPtn, selectData, 1, null, getData, status)

}

/**
 * 単票ﾎﾟｯﾌﾟｱｯﾌﾟ表示からの戻る処理
 * 
 */
function clickBackBtnForEditPopup(appPath, btnCtrlId, status) {
    // 単票入力画面 - 戻るﾎﾞﾀﾝｸﾘｯｸ時、ｲﾍﾞﾝﾄ処理

    // 【オーバーライド用関数】戻る処理の前(単票、子画面共用)
    prevBackBtnProcess(appPath, btnCtrlId, status);

    //ｴﾗｰ情報の初期化
    var formEdit = $(P_Article).find('form[id^="formEdit"]');
    clearErrorcomStatus(formEdit);

    //var formNo = $(formEdit).find('input[name="FORMNO"]').val();   //画面番号
    var formNo = $(P_Article).data("formno");   //画面番号
    var targetCtrlId = $(formEdit).attr('data-ctrlid');     //対象一覧のCTRLID

    // 選択中の画面のフォーカスをクリア
    removeFocusSelected();

    //選択中画面NOｴﾘｱ
    //P_Article = $('article[name="main_area"][data-formno="' + formNo + '"]');

    const modalNo = getCurrentModalNo(formEdit);
    var transPtn = transPtnDef.Edit;
    if (modalNo <= 1) {
        // 親画面は通常画面
        P_Article = $('article[name="main_area"][data-formno="' + formNo + '"]');
    } else {
        // 親画面はモーダル画面
        const parent = getModalElement(modalNo - 1);
        P_Article = $(parent).find('article[data-formno="' + formNo + '"]');
        if ($(P_Article).attr('name') == 'common_area') {
            // 親画面は共通機能画面
            transPtn = transPtnDef.CmConduct;
        }
    }

    //親画面情報
    var form_e = $(P_Article).find("#" + P_formDetailId);
    var conductId_e = $(form_e).find('input[name="CONDUCTID"]').val()
    var pgmId_e = $(form_e).find('input[name="PGMID"]').val();
    var conductPtn_e = $(form_e).find('input[name="CONDUCTPTN"]').val();

    //検索結果一覧を退避して渡す（※更新列情報の受け渡し用）
    var tbl = $(form_e).find('table#' + targetCtrlId + "_" + formNo);
    listData = getListDataHorizontal(formNo, tbl);

    //【オーバーライド用関数】遷移処理の前
    var [isContinue, conditionDataList] =
        prevTransForm(appPath, "", "", targetCtrlId, "", formNo, "", btnCtrlId, "", "");

    ////※親画面を再表示
    //// - 検索結果をクリア
    //clearSearchResult();
    //// - 明細エリアのエラー状態を初期化
    //clearMessage();

    //一覧表示画面遷移時の条件を渡す
    var selectData = setSearchCondition(conductId_e, Number(form_e));

    // 個別に条件データを取得している場合はselectDataに格納
    if (conditionDataList.length) {
        selectData = conditionDataList;
    }

    // 画面変更ﾌﾗｸﾞ初期化
    dataEditedFlg = false;

    //戻るｱｸｼｮﾝﾎﾞﾀﾝのCTRLIDで画面初期化時ｱｸｼｮﾝの業務ﾛｼﾞｯｸを呼び出す
    initForm(appPath, conductId_e, pgmId_e, formNo, -1, btnCtrlId, conductPtn_e, selectData, targetCtrlId, listData, skipGetDataDef.skip, status, transPtn);
}

/**
 * 戻るボタンの初期化(一覧直下単票時専用Ver.)
 * @param {string}  appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 * @param {button}  backBtn     ：対象ﾎﾞﾀﾝ
 */
function initBackBtnForEditUnderList(appPath, backBtn) {
    if ($(backBtn).length) {
        //既存のｸﾘｯｸｲﾍﾞﾝﾄを削除
        $(backBtn).off('click');

        //新たに専用のｸﾘｯｸｲﾍﾞﾝﾄを付与
        $(backBtn).on('click', function (e) {

            if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

            if (dataEditedFlg) {
                // 画面変更ﾌﾗｸﾞONの場合、ﾃﾞｰﾀ破棄確認ﾒｯｾｰｼﾞをﾎﾟｯﾌﾟｱｯﾌﾟ
                P_MessageStr = P_ComMsgTranslated[941060005]; //『画面の編集内容は破棄されます。よろしいですか？』
                // 確認OK時処理を設定
                var eventFunc = function () {
                    //単票一覧直下表示用戻る処理
                    clickBackBtnForEditUnderList(appPath, this);
                }
                // 確認ﾒｯｾｰｼﾞを表示
                if (!popupMessage([P_MessageStr], messageType.Confirm, eventFunc)) {
                    //『キャンセル』の場合、処理中断
                    return false;
                }
            }
            else {
                //単票一覧直下表示用戻る処理
                clickBackBtnForEditUnderList(appPath, this);
            }
        });
    }
}

/**
 * 単票一覧直下表示からの戻る処理
 * 
 */
function clickBackBtnForEditUnderList(appPath, backBtn) {

    // 単票入力画面 - 戻るﾎﾞﾀﾝｸﾘｯｸ時、ｲﾍﾞﾝﾄ処理
    var btnCtrlId = $(backBtn).attr("name");
    var targetList = $(backBtn).closest(".ctrlId");
    var targetId = $(targetList).attr("id");
    var targetCtrlId = $(targetList).attr("data-ctrlid");
    var formNo = $(P_Article).attr("data-formno");   //画面番号

    //【オーバーライド用関数】単票一覧直下表示用
    prevBackBtnProcessForEditUnderList(appPath, backBtn);
    //単票ｴﾘｱをｸﾘｱ
    clearSearchResult(targetId + " table:not('.actionlist')");
    //ｴﾗｰ情報の初期化
    clearErrorcomStatus(targetList);

    //画面情報
    var form_e = $(P_Article).find("form[id^='formDetail']");
    var conductId_e = $(form_e).find('input[name="CONDUCTID"]').val()
    var pgmId_e = $(form_e).find('input[name="PGMID"]').val();
    var conductPtn_e = $(form_e).find('input[name="CONDUCTPTN"]').val();

    //検索結果一覧を退避して渡す（※更新列情報の受け渡し用）
    var tbl = $(form_e).find('table#' + targetCtrlId + "_" + formNo);
    listData = getListDataHorizontal(formNo, tbl);

    //※親画面を再表示
    // - 検索結果をクリア
    clearSearchResult();
    // - 明細エリアのエラー状態を初期化
    clearMessage();
    //一覧表示画面遷移時の条件を渡す
    var selectData = getSearchCondition(conductId_e, Number(form_e));

    //戻るｱｸｼｮﾝﾎﾞﾀﾝのCTRLIDで画面初期化時ｱｸｼｮﾝの業務ﾛｼﾞｯｸを呼び出す
    initForm(appPath, conductId_e, pgmId_e, formNo, -1, btnCtrlId, conductPtn_e, selectData, targetCtrlId, listData, skipGetDataDef.GetData, status, transPtnDef.Edit);

}

/**
 * 共通機能ﾎﾟｯﾌﾟｱｯﾌﾟからの戻る処理
 * 
 */
function clickBackBtnForCmConductForm(appPath, btnCtrlId, status) {
    // 共通機能ﾎﾟｯﾌﾟｱｯﾌﾟ - 戻るﾎﾞﾀﾝｸﾘｯｸ時、ｲﾍﾞﾝﾄ処理

    var selflg = $(P_Article).attr("data-selflg");  //選択ボタン押下ﾌﾗｸﾞ
    var form = $(P_Article).find("form[id^='formDetail']");
    var parentNo = $(form).find('input[name="ORIGINNO"]').val();   //親画面番号
    var cmConductId = $(P_Article).attr("data-conductid");          //共通機能ID

    //※共通機能非表示
    //ｴﾗｰ情報初期化
    clearErrorcomStatus(P_Article);
    //画面ｴﾘｱ非表示
    $(P_Article).hide();
    $(P_Article).removeClass('selected');

    //※親画面表示
    var parentArticle = $('article[name="main_area"][data-formno="' + parentNo + '"]');
    if (parentArticle.length > 1) {
        //※親のモーダル画面表示
        parentArticle = $(parentArticle).has('section.modal_form');
    }
    $(parentArticle).addClass('selected');
    var formDetail = $(parentArticle).find("form[id^='formDetail']");
    var conductId = $(formDetail).find("input:hidden[name='CONDUCTID']").val();
    var pgmId = $(formDetail).find("input:hidden[name='PGMID']").val();
    var conductPtn = $(formDetail).find("input:hidden[name='CONDUCTPTN']").val();

    //戻るｱｸｼｮﾝﾎﾞﾀﾝのCTRLIDで画面初期化時ｱｸｼｮﾝの業務ﾛｼﾞｯｸを呼び出す
    initForm(appPath, conductId, pgmId, parentNo, -1, btnCtrlId, conductPtn, null, 1, null, skipGetDataDef.skip, status, null, selflg, cmConductId);
}

/**
 *  実行処理正常終了後、自動で戻る処理
 *  @appPath {string}           : ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @ctrlId  {string}           : 実行ﾎﾞﾀﾝのｺﾝﾄﾛｰﾙID
 *  @status  {CommonProcReturn} : 実行処理結果ｽﾃｰﾀｽ
 */
function autoBackAfterExec(appPath, ctrlId, status) {

    var execBtn = $(P_Article).find("input[type='button'][name='" + ctrlId + "']"); // 実行ボタン
    var element = $(execBtn).closest('form[id^="formEdit"]');  //単票入力画面ﾎﾞﾀﾝｴﾘｱの配置か？
    var backBtnCtrlId = $(execBtn).closest("table.actionlist").find("input[type='button'][data-actionkbn='" + actionkbn.Back + "']").attr("name");  // 同画面上の戻るボタン

    var modal = $(P_Article).closest(".modal_form");
    if ($(modal).length) {
        // ﾎﾟｯﾌﾟｱｯﾌﾟ閉じた後のｲﾍﾞﾝﾄを再設定
        $(modal).off('hidden.bs.modal');
        $(modal).on('hidden.bs.modal', function (e) {
            //$('.modal-backdrop').remove();
            // - ﾎﾟｯﾌﾟｱｯﾌﾟ画面ｴﾘｱ初期化
            const currentModal = e.currentTarget;
            var detailDiv = $(currentModal).find('.form_detail_div');
            $(detailDiv).find('form[id^="formEdit"]').children().not("input[type='hidden']").remove();
            $(detailDiv).find("article[name='main_area']").remove();
            // メッセージをクリア
            var messageDiv = $(currentModal).find('.message_div');
            $(messageDiv).children().remove();
        });
        // ﾎﾟｯﾌﾟｱｯﾌﾟ閉じる
        $(modal).modal('hide');
    }

    // 戻るボタンが存在しなくても戻る処理を呼び出して問題ないのでは？
    // 戻る処理の実行契機となったボタンのコントロールIDを使用して戻るよう変更する
    //if (backBtnCtrlId != null && backBtnCtrlId.length > 0) {
    //    //戻る処理の分岐
    //    divisionBackBtn(appPath, element, backBtnCtrlId, status);
    //}
    //戻る処理の分岐
    divisionBackBtn(appPath, element, ctrlId, status);

    modal = null;
}

/**
 *  行追加ボタンの初期化処理
 *  @param {string} appPath ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} btns    ：対象ボタン
 */
function initAddNewRowBtn(appPath, btns) {
    // 行追加ボタンクリックイベントハンドラの設定
    $.each(btns, function (idx, btn) {

        var addkbn = $(btn).data('addkbn'); // 行追加区分　1：行追加、2：新規画面表示

        if (addkbn == rowAddKbnDef.TransNewForm) {
            //※新規画面表示の場合

            //ﾎﾞﾀﾝにｸﾘｯｸｲﾍﾞﾝﾄ付与
            $(btn).on('click', function () {

                if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

                var formNo = $(P_Article).data("formno");   //画面NO
                var ctrlId = $(this).data('parentid');       //一覧CTRLID
                var tbl = $('#' + ctrlId + "_" + formNo);  //一覧table要素
                var isCopyRow = $(btn).data("actionkbn") == actionkbn.AddCopyRow;
                var ctrltype = $(tbl).data('ctrltype');    //コントロールタイプ

                clearMessage();

                // 選択チェックの取得
                var checkes = $(tbl).find("tr:not('.base_tr') td[data-name='SELTAG'] :checkbox:checked");;
                if (ctrltype == ctrlTypeDef.IchiranPtn3) {
                    //var table = P_listData['#' + ctrlId + "_" + formNo];
                    //var row = table.searchRows("SELTAG", "=", true);
                    checkes = $(tbl).find("input[type='checkbox']:checked");
                }
                if (isCopyRow) {
                    if (checkes == null || checkes.length == 0) {
                        // 行コピーでチェック無しの場合はメッセージをセットして終了
                        // 「対象行が選択されていません。」
                        setMessage([P_ComMsgTranslated[941160003]], messageType.Warning);
                        return;
                    }
                }

                var transPtn = $(tbl).data("transptn");   // 画面遷移ﾊﾟﾀｰﾝ
                var transTarget = null;                     // 遷移先
                if (transPtn == transPtnDef.Child) {
                    //子画面の場合、子画面NOをｾｯﾄ
                    transTarget = $(tbl).data("transtarget");
                }
                else if (transPtn == transPtnDef.Edit) {
                    //単票の場合、一覧CTRLIDをｾｯﾄ
                    transTarget = ctrlId;
                    ctrlId = null;
                }
                else if (transPtn == transPtnDef.CmConduct) {
                    // 共通機能の場合、コントロールIDをセット
                    transTarget = $(tbl).data("transtarget");
                }
                var dispPtn = $(tbl).data("transdispptn");//画面遷移表示ﾊﾟﾀｰﾝ

                // イベントを削除
                $(window).off('beforeunload');

                var rowNo = null;
                if (isCopyRow) {
                    // 行コピーの場合、チェック行の先頭行の行番号を取得
                    // (複数選択の場合は先頭のみを対象とする)
                    var tr = $(checkes[0]).closest('tr');
                    if (ctrltype == ctrlTypeDef.IchiranPtn3) {
                        tr = checkes[0];
                    }
                    var rowNo = parseInt($(tr).attr('data-rowno'), 10);
                }

                //【ｵｰﾊﾞｰﾗｲﾄﾞ用関数】行追加の前
                if (true == prevAddNewRow(appPath, this, isCopyRow, transPtn, transTarget, dispPtn, formNo, ctrlId, rowNo, -1, true)) {
                    // 続行
                    confirmScrapBeforeTrans(appPath, transPtn, transDivDef.None, transTarget, dispPtn, formNo, ctrlId, rowNo, -1, this, true);
                }
            });
        }
        else {
            // 行追加
            $(btn).on('click', function () {

                var formNo = $(P_Article).data("formno");   //画面NO
                var id = $(this).data('parentid');          //一覧CTRLID
                var isCopyRow = $(btn).data("actionkbn") == actionkbn.AddCopyRow;
                var tbl = $('#' + id + "_" + formNo);
                var tbody = $(tbl).find('tbody');
                var maxidx = parseInt($(tbl).data("maxidx"), 10);

                // 選択チェックの取得
                var checkes = $(tbody).find("tr:not('.base_tr') td[data-name='SELTAG'] :checkbox:checked");
                if (isCopyRow) {
                    if (checkes == null || checkes.length == 0) {
                        // 行コピーでチェック無しの場合はメッセージをセットして終了
                        // 「対象行が選択されていません。」
                        setMessage([P_ComMsgTranslated[941160003]], messageType.Warning);
                        return;
                    }
                }

                //【ｵｰﾊﾞｰﾗｲﾄﾞ用関数】行追加の前
                if (true == prevAddNewRow(appPath, this, isCopyRow, null, null, null, formNo, id)) {
                    // 続行
                    var base_tr = $(tbody).find('.base_tr');  //ﾍﾞｰｽﾚｲｱｳﾄ行
                    var rowAdd = false;

                    var addStartRowNo = -1;
                    if (checkes != null && checkes.length > 0) {
                        // 選択チェックありの場合
                        $.each(checkes, function (idx, chk) {
                            var tr = $(chk).closest('tr');
                            var selectedRowNo = parseInt($(tr).attr('data-rowno'), 10);
                            var selected_tr = $(tbody).find("tr[data-rowno='" + selectedRowNo + "']"); // 選択行
                            if (addStartRowNo < 0) {
                                // 追加開始ROWNO=選択行の最初のROWNO+1
                                addStartRowNo = parseInt($(tr).attr('data-rowno'), 10) + 1;
                            }
                            maxidx++;
                            // 選択行をコピーして選択行の前後に追加
                            copyAndAddTableRow(appPath, tbl, tbody, base_tr, selected_tr, 0, maxidx, isCopyRow);
                        });

                        // data-rownoが存在する行
                        var rownoList = $(tbody).find("tr:not('.base_tr')[data-rowno]");
                        var rowNo = 0;
                        var rowCnt = $(base_tr).length;
                        var addStart = false;
                        $(rownoList).each(function (index, element) {
                            var tmpNo = parseInt($(element).attr('data-rowno'), 10);
                            // 追加開始行以降のdata-rownoを更新する
                            if (!addStart && tmpNo > 0 && tmpNo < addStartRowNo) {
                                rowNo = tmpNo;
                                return true;    // continue;
                            }
                            addStart = true;
                            if ((index % rowCnt) == 0) {
                                // 先頭trの場合のみrownoをインクリメント
                                rowNo++;
                            }
                            // data-rownoを更新
                            setAttrByNativeJs(element, 'data-rowno', rowNo);
                        });
                    }
                    else {

                        // data-rownoが存在する行
                        var rownoList = $(tbody).find("tr:not('.base_tr')[data-rowno]");
                        var maxRowno = 0;
                        $(rownoList).each(function (index, element) {
                            // data-rownoの最大値を取得
                            rowno = parseInt($(element).attr('data-rowno'), 10);
                            if (maxRowno < rowno) {
                                maxRowno = rowno;
                            }
                        });
                        maxidx++;
                        // 新規行を一覧末尾に追加
                        copyAndAddTableRow(appPath, tbl, tbody, base_tr, null, maxRowno + 1, maxidx, isCopyRow);
                    }

                    var formId = $(this).closest("form").attr("id");
                    //入力ﾌｫｰﾏｯﾄ設定処理
                    initFormat("#" + formId);
                    // Validator初期化
                    initValidator("#" + formId);

                    // 追加行の列固定設定
                    if (false == $(tbl).hasClass("ctrlId")) {
                        setFixColCssForDataRow(tbl);
                    }

                    // tabindexを再設定
                    nextFocus();

                    //【オーバーライド用関数】行追加後
                    postAddNewRow(appPath, btn, isCopyRow);
                }
            });
        }
    });
}

/**
 *  行コピー⇒追加処理
 *  @param {table} tbl      ：一覧のtable要素
 *  @param {tbody} tbody    ：一覧のtbody要素
 *  @param {tr} base_tr     ：一覧のレイアウト用tr要素
 *  @param {tr} selected_tr ：一覧の選択行のtr要素
 *  @param {number} rowNo   ：行番号
 *  @param {number} maxidx  ：最大インデックス
 *  @param {bool} isCopyRow ：行コピーかどうか
 */
function copyAndAddTableRow(appPath, tbl, tbody, base_tr, selected_tr, rowNo, maxidx, isCopyRow) {

    var new_tr = $(base_tr).clone(true);// 新規行
    $(new_tr).removeClass("base_tr");
    if (isCopyRow) {

    }

    setAttrByNativeJs(new_tr, 'id', '');
    setAttrByNativeJs(new_tr, 'data-datatype', dataTypeDef.New.toString()); // データタイプに2:新規行データをセット
    setAttrByNativeJs(new_tr, 'data-rowstatus', rowStatusDef.New.toString());   // 行ステータスに2:新規行をセット
    setAttrByNativeJs(new_tr, 'data-rowno', rowNo);

    // コントロール初期化用
    var dPickers = [];  //DatePicker
    var tPickers = [];  //TimePicker
    var dtPickers = []; //DateTimePicker
    var ymPickers = []; //YearMonthPicker
    var atComps = [];   //オートコンプリート
    var chkBoxs = [];   //チェックボックス
    var cdTrns = [];    //コード＋翻訳
    var cdTrnsBtns = [];//コード＋翻訳 選ボタン

    // lockDataにｾｯﾄ(ｴﾗｰﾁｪｯｸの行識別の為)
    var lockData = $(new_tr).find("td[data-name='lockData']");
    $(lockData).text(maxidx + "");

    // 更新フラグに「編集中」を設定
    var updflg = $(new_tr).find("input[data-type='updflg']");
    if (updflg != null) {
        $(updflg).val(updtag.Input);
    }

    // 選択チェックを外す
    var check = $(new_tr).find("td[data-name='SELTAG'] :checkbox");
    if (check != null && check.length > 0) {
        $(check).removeAttr('checked').prop('checked', false);
        initCheckBox(check, false);
    }

    var tds = $(new_tr).find("td[data-name^='VAL']");
    if (tds != null && tds.length > 0) {

        $.each(tds, function (idx, td) {
            var valName = $(td).attr('data-name');
            var selected_td = $(selected_tr).find("td[data-name='" + valName + "']");

            // 入力コントロール全般
            var inputs = $(td).find(":text, textarea, select, ul, :checkbox, img, :button, canvas");
            if (inputs != null && inputs.length > 0) {
                $.each(inputs, function (idx, input) {
                    // idの更新
                    var id = $(input).attr('id');
                    if (id != null && id.length > 0) {
                        var name = $(input).attr('name');
                        if (name != null && name.length > 0) {
                            setAttrByNativeJs(input, 'id', name + "_" + maxidx);
                        } else {
                            var baseId = id;
                            var lastIdx = id.lastIndexOf("VAL");
                            lastIdx = id.lastIndexOf("_", lastIdx);
                            if (lastIdx >= 0) {
                                baseId = id.substring(0, lastIdx);
                            }
                            setAttrByNativeJs(input, 'id', baseId + "_" + maxidx);
                        }
                    }
                    // 選択行の値の設定
                    if (isCopyRow) {
                        var tagName = $(input).prop('tagName').toLowerCase();
                        var selected_input = $(selected_td).find(tagName);
                        if (selected_input != null && selected_input.length > 0) {
                            var val = $(selected_input).val();
                            if (val == null || val.length == 0) {
                                val = (selected_input).attr('data-value');
                            }
                            switch (tagName) {
                                case "input":
                                    $(input).val(val);
                                    if ($(selected_input).attr('data-autocompdiv') != autocompDivDef.None) {
                                        val = $(selected_td).find('span.honyaku').text();
                                        $(td).find('span.honyaku').text(val);
                                    }
                                    break;
                                case "textarea":
                                case "select":
                                case "ul":
                                    $(input).val(val);
                                    break;
                                case "img":
                                    setAttrByNativeJs(input, 'src', $(selected_input).attr('src'));
                                    break;
                            }
                        }
                    }
                });
            }
            else {
                if (isCopyRow) {
                    // tdに入力コントロールが存在しない場合、textを設定
                    var text = $(selected_td).first().text();
                    if (text != null && text.length > 0) {
                        $(td).first().text(text);
                    }
                }
            }
        });

        // 日付(AKAP標準)
        var inputs = $(tds).find(":text[data-type='date']");
        if (inputs != null && inputs.length > 0) {
            $.each(inputs, function (idx, input) {
                dPickers.push($(input).attr('id')); //id退避
            });
        }
        // 時刻(AKAP標準)
        var inputs = $(tds).find(":text[data-type='time']");
        if (inputs != null && inputs.length > 0) {
            $.each(inputs, function (idx, input) {
                tPickers.push($(input).attr('id')); //id退避
            });
        }
        // 日時(AKAP標準)
        var inputs = $(tds).find(":text[data-type='datetime']");
        if (inputs != null && inputs.length > 0) {
            $.each(inputs, function (idx, input) {
                dtPickers.push($(input).attr('id')); //id退避
            });
        }
        // 年月
        var inputs = $(tds).find(":text[data-type='yearmonth']");
        if (inputs != null && inputs.length > 0) {
            $.each(inputs, function (idx, input) {
                ymPickers.push($(input).attr('id')); //id退避
            });
        }

        // オートコンプリート
        var inputs = $(tds).find(":text[data-type='text'], textarea[data-type='textarea'], :text[data-type='codeTrans']");
        if (inputs != null && inputs.length > 0) {
            $.each(inputs, function (idx, input) {
                if ($(input).data("autocompdiv") != autocompDivDef.None) {
                    atComps.push($(input).attr('id')); //id退避
                }
            });
        }
        // チェックボックス
        var inputs = $(tds).find(":checkbox");
        if (inputs != null && inputs.length > 0) {
            $.each(inputs, function (idx, input) {
                chkBoxs.push($(input).attr('id')); //id退避
            });
        }
        // コード＋翻訳 選ボタン
        var inputs = $(tds).find("input:button[data-type='codeTransBtn']");
        if (inputs != null && inputs.length > 0) {
            $.each(inputs, function (idx, input) {
                cdTrnsBtns.push($(input).attr('id')); //id退避
            });
        }
        // 画像
        var imgs = $(tds).find("img");
        if (imgs != null && imgs.length > 0) {
            $.each(imgs, function (idx, img) {
                // 行追加時は初期値
                setAttrByNativeJs(img, 'src', $(img).data('def'));
            });
        }

        // 追加行に画面編集ﾌﾗｸﾞの制御ｲﾍﾞﾝﾄ付与
        setEventForEditFlg(true, new_tr);

        if (selected_tr == null) {
            // 選択行がない場合、一覧の末尾に追加
            $(tbody).append($(new_tr));
        } else {
            if (!isCopyRow) {
                // 行追加の場合、選択行の前に追加
                $(selected_tr).first().before($(new_tr));
            }
            else {
                // 行コピーの場合、選択行の後ろに追加
                $(selected_tr).last().after($(new_tr));
            }
        }

        // AKAP標準の日付、時刻、日時コントロールの初期化
        $(dPickers).each(function (index, dpId) {
            var picker = $('#' + dpId);
            initDatePicker(picker[0], false);
        });
        $(tPickers).each(function (index, dpId) {
            var picker = $('#' + dpId);
            initTimePicker(picker[0], false);
        });
        $(dtPickers).each(function (index, dpId) {
            var picker = $('#' + dpId);
            initDateTimePicker(picker[0], false);
        });
        $(ymPickers).each(function (index, dpId) {
            var picker = $('#' + dpId);
            initYearMonthPicker(picker[0], false);
        });
        // 年入力
        var yearTexts = $(tds).find(YearText.selector);
        $(yearTexts).each(function (index, element) {
            initYearText(element, false);
        });
        // オートコンプリートの初期化
        $(atComps).each(function (index, dpId) {
            $.each(P_autocompDefines, function (idx, define) {
                if (('#' + dpId).indexOf(define.key) === 0) {
                    var cmp = $('#' + dpId);
                    var sqlId = define.sqlId;
                    var param = define.param;
                    var div = define.division;
                    var option = define.option;
                    initAutoComp(appPath, cmp[0], sqlId, param, null, div, option);
                    return false;
                }
            });
        });
        // チェックボックスの初期化
        $(chkBoxs).each(function (index, dpId) {
            var chk = $('#' + dpId);
            initCheckBox(chk[0], false);
        });
        // コード＋翻訳 選ボタン 初期化
        $(cdTrnsBtns).each(function (index, dpId) {
            initCodeTransBtn(appPath, $("#" + dpId));
        });
    }

    // MAX値をｾｯﾄ
    setAttrByNativeJs(tbl, "data-maxidx", maxidx);
}

/**
 *  行削除ボタンの初期化処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 */
function initDeleteRowBtn(appPath, btns) {
    // 行削除ボタンクリックイベントハンドラの設定
    $.each(btns, function (idx, btn) {
        $(btn).on('click', function () {

            if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

            var formNo = $(P_Article).data("formno");   //画面NO
            var id = $(btn).data('parentid');
            var tbl = $('#' + id + '_' + formNo + ' tbody');
            var checkes = $(tbl).find("tr:not('.base_tr') td[data-name='SELTAG'] :checkbox:checked");

            var tableId = '#' + id + '_' + formNo;
            var table = P_listData[tableId];
            var targetTbl = $(tableId);
            var ctrltype = $(targetTbl).data('ctrltype');    //コントロールタイプ
            if (ctrltype == ctrlTypeDef.IchiranPtn3) {
                //表示していないページのHTMLは生成されていない為、データのSELTAGの値より取得
                checkes = $.grep(table.getData(),
                    function (obj, idx) {
                        return obj.SELTAG == 1;
                    });
            }

            clearMessage();
            if (checkes == null || checkes.length == 0) {
                // 行コピーでチェック無しの場合はメッセージをセットして終了
                // 「対象行が選択されていません。」
                setMessage([P_ComMsgTranslated[941160003]], messageType.Warning);
                return;
            }

            //【オーバーライド用関数】行削除前処理
            if (!preDeleteRow(appPath, btn, id + '_' + formNo, checkes)) {
                return;
            }

            if (ctrltype == ctrlTypeDef.IchiranPtn3) {
                var deleteDataList = [];
                $.each(checkes, function (idx, chk) {
                    var rowNo = chk.ROWNO;
                    var updateRow = table.searchRows("ROWNO", "=", rowNo);
                    // 行ステータスに9: 削除行をセット
                    //updateRow[0].update({ "ROWSTATUS": rowStatusDef.Delete });
                    updateRow[0].getData().ROWSTATUS = rowStatusDef.Delete;
                    //削除行のデータを取得
                    var deleteData = getTempDataForTabulator(formNo, rowNo, tableId, 0);
                    deleteDataList.push(deleteData);
                    ////行を削除
                    //updateRow[0].delete();
                });
                //削除行のデータを退避
                P_deleteData[tableId] = P_deleteData[tableId].concat(deleteDataList);
                //行を削除
                var deleteRowNoList = deleteDataList.map(function (item) { return item.ROWNO });
                table.deleteRow(deleteRowNoList);
                //再描画
                table.redraw();
            } else {
                $.each(checkes, function (idx, chk) {
                    var basetr = $(chk).closest('tr');
                    var rowno = $(basetr).attr('data-rowno');
                    // data-rownoが同じ行を取得（1データが複数行の場合を考慮）
                    var tr = $(basetr).parent().find('tr[data-rowno="' + rowno + '"]');
                    // 行ステータスに9:削除行をセット
                    setAttrByNativeJs(tr, 'data-rowstatus', '9');
                    // 非表示
                    $(tr).css('display', 'none');
                    $(chk).prop('checked', false);
                    var datatype = $(basetr).attr("data-datatype");
                    if (datatype != dataTypeDef.New) {
                        // 新規行以外の場合
                        // 画面編集フラグON
                        setupDataEditedFlg();
                    }
                });
            }
            table = null;

            //【オーバーライド用関数】行削除後の個別実装
            postDeleteRow(appPath, btn, id + '_' + formNo);
        });
    });
}

/**
 *  全選択および全解除ボタンの初期化処理
 *  @param {bool}   :全選択の場合True、全解除の場合False
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 */
function initAllSelectCancelBtn(isSelect, appPath, btns) {
    // 全選択および全解除ボタンクリックイベントハンドラの設定
    $.each(btns, function (idx, btn) {
        $(btn).on('click', function () {

            if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

            var id = $(btn).data('parentid');
            var formNo = $(P_Article).data("formno");   //画面NO
            var tableid = id + '_' + formNo;
            var tbl = $(this).closest('#' + tableid + '_div').find('#' + tableid);
            var ctrltype = $(tbl).data('ctrltype');
            var checkes = null;
            if (ctrltype == ctrlTypeDef.IchiranPtn3) {
                // 非表示のページも対応
                var table = P_listData['#' + tableid];
                if (!table) {
                    table = null;
                    return;
                }

                //表示中のページのみチェックボックスのON/OFF(見えている行のみ)
                checkes = $(tbl).find(".tabulator-row .tabulator-cell[tabulator-field='SELTAG'] :checkbox");
                //チェックボックスをON/OFFした行
                var checksRowNoList = [];
                $.each(checkes, function (idx, chk) {
                    if (!$(chk).is(":disabled") && $(chk).is(":visible")) {
                        var tr = $(chk).closest(".tabulator-row");
                        setupDataEditedFlg(tr);
                        $(chk).prop('checked', isSelect);
                        checksRowNoList.push($(chk).data("rowno"));
                    }
                });

                var rows = table.getRows("active");//フィルタリングされている行
                if (!rows) { return; }
                $.each(rows, function (i, row) {
                    var val = isSelect ? 1 : 0;
                    if (row.getData().SELTAG == val) {
                        return true;//continue
                    }
                    //チェック状態を設定
                    row.getData().SELTAG = val;

                    //行番号
                    var rowNo = row.getData().ROWNO;
                    //表示中ページ内で見えていない行(HTMLが生成されていない行)のチェックボックスON/OFF設定
                    //if ($.inArray(rowNo, checksRowNoList) == -1 && row.getPosition() != false) {
                    //上記に加えて、画面番号1以降の場合、表示対象外のページの行すべてに対してupdateData()を実施しないと反映されないことがある
                    if ($.inArray(rowNo, checksRowNoList) == -1 && (row.getPosition() != false || formNo > 0)) {
                        //SELTAGに選択状態を設定
                        var item = { ROWNO: rowNo, SELTAG: val };
                        table.updateData([item]);
                    }
                });
                table = null;
            } else {
                checkes = $(tbl).find("tr:not('.base_tr') td[data-name='SELTAG'] :checkbox");
                //var tr = $(checkes).closest("tr");
                //setupDataEditedFlg(tr);//UPDTAG・変更ﾌﾗｸﾞ
                $.each(checkes, function (idx, chk) {
                    if (!$(chk).is(":disabled") && $(chk).is(":visible")) {
                        var tr = $(chk).closest("tr");
                        setupDataEditedFlg(tr);
                        $(chk).prop('checked', isSelect);
                    }
                });
            }
            //$.each(checkes, function (idx, chk) {
            //    $(chk).prop('checked', true);
            //});

            //【オーバーライド用関数】全選択および全解除ボタンの押下後
            afterAllSelectCancelBtn(formNo, id);
        });
    });
}

/**
 *  全選択および全解除ボタン(ページ内)の初期化処理
 *  @param {bool}   :全選択の場合True、全解除の場合False
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 */
function initAllSelectCancelBtnPage(isSelect, appPath, btns) {
    // 表示中のページのみの全選択/全解除処理

    // 全選択および全解除ボタンクリックイベントハンドラの設定
    $.each(btns, function (idx, btn) {
        $(btn).on('click', function () {

            if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

            var id = $(btn).data('parentid');
            var formNo = $(P_Article).data("formno");   //画面NO
            var tableid = id + '_' + formNo;
            var tbl = $(this).closest('#' + tableid + '_div').find('#' + tableid);
            var ctrltype = $(tbl).data('ctrltype');
            var checkes = null;
            if (ctrltype == ctrlTypeDef.IchiranPtn3) {
                var table = P_listData['#' + tableid];
                if (!table) { return; }

                //表示中のページのみチェックボックスのON/OFF
                checkes = $(tbl).find(".tabulator-row .tabulator-cell[tabulator-field='SELTAG'] :checkbox");
                $.each(checkes, function (idx, chk) {
                    if (!$(chk).is(":disabled") && $(chk).is(":visible")) {
                        var tr = $(chk).closest(".tabulator-row");
                        setupDataEditedFlg(tr);
                        $(chk).prop('checked', isSelect);
                    }
                });

                var rows = table.getData("active");//表示中のページの行のみ
                if (!rows) { return; }
                $.each(rows, function (i, row) {
                    var val = isSelect ? 1 : 0;
                    if (row.getData().SELTAG == val) {
                        return true;//continue
                    }
                    //チェック状態を設定（HTMLには反映されないので、表示中のページのみチェックボックスのON/OFF制御を行う）
                    row.getData().SELTAG = val;
                    ////行番号
                    //var rowNo = row.getData().ROWNO;
                    ////SELTAGに選択状態を設定
                    //var item = { ROWNO: rowNo, SELTAG: val };
                    //table.updateData([item]);
                });

                table = null;
            } else {
                checkes = $(tbl).find("tr:not('.base_tr') td[data-name='SELTAG'] :checkbox");
                //var tr = $(checkes).closest("tr");
                //setupDataEditedFlg(tr);//UPDTAG・変更ﾌﾗｸﾞ
                $.each(checkes, function (idx, chk) {
                    if (!$(chk).is(":disabled") && $(chk).is(":visible")) {
                        var tr = $(chk).closest("tr");
                        setupDataEditedFlg(tr);
                        $(chk).prop('checked', isSelect);
                    }
                });
            }
        });
    });
}

/**
 *  【共通 - マスタ機能】一覧表示切替ボタンの初期化処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 */
function initSwitchTableBtn(appPath, btn) {
    // ボタンクリックイベントハンドラの設定
    $(btn).on('click', function () {
        var formNo = $(P_Article).data("formno");   //画面NO
        var ctrlId = $(this).data('switchid');
        var id = ctrlId + '_' + formNo;

        if (('sessionStorage' in window) && (window.sessionStorage !== null)) {
            // 表示状態を退避
            if (id == "side_menu") {
                var state = "";

                if (!isHideId(id) == true) {
                    state = "hide";
                }
                else {
                    state = "";
                }

                sessionStorage.setItem("CIM_MENU_STATE", state);
            }
        }

        //一覧の表示/非表示に合わせて、他のボタンの表示/非表示を切り替える
        $("div.tbl_title > a[data-parentid='" + ctrlId + "']").each(function () {
            setHide($(this), !isHideId(id));
        });

        //表示/非表示切替
        setHideId(id, !isHideId(id));

        //アイコンの表示切替
        var child = $(this).children('span');
        if (child.hasClass('glyphicon-chevron-up')) {
            child.removeClass('glyphicon-chevron-up');
            child.addClass('glyphicon-chevron-down');
        } else if (child.hasClass('glyphicon-chevron-down')) {
            child.removeClass('glyphicon-chevron-down');
            child.addClass('glyphicon-chevron-up');
        }

        //【オーバーライド用関数】一覧表示切替後処理
        postSwitchTable(id, isHideId(id));

        // フォーカス再設定
        removeFocus();
        nextFocus();
    });
}
///**
// *  子画面新規ボタンの初期化処理
// *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
// *  @param {button} ：対象ボタン
// *  @param {byte} ：画面番号
// */
//function initChildNewBtn(appPath, btn, formNo) {

//    // 子画面新規ボタンクリックイベントハンドラの設定
//    $(btn).on('click', function () {

//        if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

//        //formNo取得
//        var formNoW = $(P_Article).data("formno");

//        // イベントを削除
//        $(window).off('beforeunload');

//        var btnCtrlId = $(this).attr("name");           //ﾎﾞﾀﾝCTRLID
//        var transPtn = transPtnDef.Child;               //子画面表示
//        var transTarget = $(this).data("relationid");   //子画面NO
//        var dispPtn = $(this).data("optioninfo");       //遷移表示ﾊﾟﾀｰﾝ

//        //【オーバーライド用関数】子画面新規遷移前ﾁｪｯｸ処置
//        var form = $(this).closest("article").find("form[id^='form']");
//        var conductId = $(form).find("input:hidden[name='CONDUCTID']").val();
//        if (!transChildCheckPre(appPath, conductId, formNoW, this)) {
//            // 処理中断
//            return false;
//        }

//        var confirmFlg = true;
//        var confirmKbn = $(this).data("relationparam") + "";
//        if (confirmKbn != null && confirmKbn.length > 0 && confirmKbn == confirmKbnDef.NonDisp) {
//            confirmFlg = false;
//        }

//        //遷移前の破棄確認メッセージ表示
//        confirmScrapBeforeTrans(appPath, transPtn, transTarget, dispPtn, formNoW, null, btnCtrlId, -1, null, confirmFlg);
//    });
//}
///**
// *  単票入力新規ボタンの初期化処理
// *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
// *  @param {button} ：対象ボタン
// *  @param {byte} ：画面番号
// */
//function initEditNewBtn(appPath, btn, formNo) {

//    // 単票入力新規ボタンクリックイベントハンドラの設定
//    $(btn).on('click', function () {

//        if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

//        //formNo取得
//        var formNoW = $(P_Article).data("formno");

//        // イベントを削除
//        $(window).off('beforeunload');
//        var btnCtrlId = $(this).attr("name");           //ﾎﾞﾀﾝCTRLID
//        var transPtn = transPtnDef.Edit;                //単票入力
//        var transTarget = $(this).data("relationid");   //単票表示する一覧のCTRLID
//        var dispPtn = $(this).data("optioninfo");       //遷移表示ﾊﾟﾀｰﾝ

//        var confirmFlg = true;
//        var confirmKbn = $(this).data("relationparam") + "";
//        if (confirmKbn != null && confirmKbn.length > 0 && confirmKbn == confirmKbnDef.NonDisp) {
//            confirmFlg = false;
//        }

//        //遷移前の破棄確認メッセージ表示
//        confirmScrapBeforeTrans(appPath, transPtn, transTarget, dispPtn, formNoW, null, btnCtrlId, -1, null, confirmFlg);
//    });
//}
///**
// *  共通機能ポップアップボタンの初期化処理
// *  @param {string} appPath ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
// *  @param {button} btn     ：対象ボタン
// */
//function initTransCmBtn(appPath, btn) {

//    // 共通機能ポップアップボタンクリックイベントハンドラの設定
//    $(btn).on('click', function () {

//        if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

//        //formNo取得
//        var formNoW = $(P_Article).data("formno");

//        // イベントを削除
//        // $(window).off('beforeunload');               //共通機能ポップアップ表示は画面切替しないので、いったんコメントアウトにする(おかしくなったら戻す)
//        var btnCtrlId = $(this).attr("name");           //ﾎﾞﾀﾝCTRLID
//        var transPtn = transPtnDef.CmConduct;           //共通機能ポップアップ
//        var transTarget = $(this).data("relationid");   //共通機能ID
//        var dispPtn = transDispPtnDef.Popup;            //遷移表示ﾊﾟﾀｰﾝ※ﾎﾟｯﾌﾟｱｯﾌﾟ固定

//        //遷移前の確認用オーバーライド関数
//        if (!prevTransCmForm(appPath, transTarget, formNoW, this)) {
//            return false;
//        }

//        //遷移前の破棄確認メッセージ表示
//        confirmScrapBeforeTrans(appPath, transPtn, transTarget, dispPtn, formNoW, null, btnCtrlId, -1, this, false);
//    });
//}
///**
// *  他機能遷移（別タブ）ボタンの初期化処理
// *  @param {string} appPath ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
// *  @param {button} btn     ：対象ボタン
// */
//function initTransOtherTabBtn(appPath, btn) {

//    // 他機能遷移（別タブ）ボタンクリックイベントハンドラの設定
//    $(btn).on('click', function () {

//        if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

//        //formNo取得
//        var formNoW = $(P_Article).data("formno");

//        // イベントを削除
//        $(window).off('beforeunload');
//        var btnCtrlId = $(this).attr("name");           //ﾎﾞﾀﾝCTRLID
//        var transPtn = transPtnDef.OtherTab;            //他機能遷移（別タブ）
//        var transTarget = $(this).data("relationid");   //遷移先機能ID|画面NO
//        var dispPtn = transDispPtnDef.None;            //遷移表示ﾊﾟﾀｰﾝ※なし固定

//        //遷移前の破棄確認メッセージ表示
//        confirmScrapBeforeTrans(appPath, transPtn, transTarget, dispPtn, formNoW, null, btnCtrlId, -1, this, false);
//    });
//}
///**
// *  他機能遷移（同タブ表示切替）ボタンの初期化処理
// *  @param {string} appPath ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
// *  @param {button} btn     ：対象ボタン
// */
//function initTransOtherShiftBtn(appPath, btn) {

//    // 他機能遷移（同タブ表示切替）ﾎﾞﾀﾝｸﾘｯｸｲﾍﾞﾝﾄﾊﾝﾄﾞﾗの設定
//    $(btn).on('click', function () {

//        if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

//        //formNo取得
//        var formNoW = $(P_Article).data("formno");

//        // イベントを削除
//        $(window).off('beforeunload');
//        var btnCtrlId = $(this).attr("name");           //ﾎﾞﾀﾝCTRLID
//        var transPtn = transPtnDef.OtherShift;          //他機能遷移（同タブ表示切替）
//        var transTarget = $(this).data("relationid");   //遷移先機能ID|画面NO
//        var dispPtn = transDispPtnDef.Shift;            //遷移表示ﾊﾟﾀｰﾝ※表示切替固定

//        var confirmFlg = true;
//        var confirmKbn = $(this).data("relationparam") + "";
//        if (confirmKbn != null && confirmKbn.length > 0 && confirmKbn == confirmKbnDef.NonDisp) {
//            confirmFlg = false;
//        }

//        //遷移前の破棄確認メッセージ表示
//        confirmScrapBeforeTrans(appPath, transPtn, transTarget, dispPtn, formNoW, null, btnCtrlId, -1, this, confirmFlg);
//    });
//}

/**
 *  画面遷移ボタンの初期化処理
 *  @param {string} appPath ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} btn     ：対象ボタン
 */
function initFormTransitionBtn(appPath, btn) {

    // 画面遷移ﾎﾞﾀﾝｸﾘｯｸｲﾍﾞﾝﾄﾊﾝﾄﾞﾗの設定
    $(btn).on('click', function () {

        if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

        //formNo取得
        var formNoW = $(P_Article).data("formno");
        var form = $(this).closest("article").find("form[id^='form']");
        var conductId = $(form).find("input:hidden[name='CONDUCTID']").val();
        var parentTbl = $(this).closest('table');
        var rowNo = -1;
        if (parentTbl) {
            if ($(parentTbl).hasClass('actionlist')) {
                // コントロールグループ内のボタン
            } else {
                // 一覧内のボタン
                var tr = $(this).closest('tr');
                rowNo = parseInt($(tr).attr('data-rowno'), 10);
            }
        }

        // イベントを削除
        $(window).off('beforeunload');
        var btnCtrlId = $(this).attr("name");           //ボタンCTRLID
        var transPtn = $(this).data("transptn");        //画面遷移パターン
        var transDiv = $(this).data("transdiv");        //画面遷移アクション区分
        var transTarget = $(this).data("relationid");   //遷移先ID
        var dispPtn = $(this).data("optioninfo");       //遷移表示パターン

        var confirmKbn = $(this).data("relationparam") + ""; //遷移前確認区分
        var confirmFlg = true;
        if (confirmKbn != null && confirmKbn.length > 0 && confirmKbn == confirmKbnDef.NonDisp) {
            confirmFlg = false;
        }

        if (transPtn == transPtnDef.Child) {
            //【オーバーライド用関数】遷移前ﾁｪｯｸ処置
            if (!prevTransChildForm(appPath, conductId, formNoW, this)) {
                // 処理中断
                return false;
            }
        } else if (transPtn == transPtnDef.CmConduct) {
            //【オーバーライド用関数】遷移前ﾁｪｯｸ処置
            if (!prevTransCmForm(appPath, transTarget, formNoW, this)) {
                return false;
            }
            dispPtn = transDispPtnDef.Popup;
            confirmFlg = false;
        } else if (transPtn == transPtnDef.OtherTab) {
            dispPtn = transDispPtnDef.None;
            confirmFlg = false;
        } else if (transPtn == transPtnDef.OtherShift) {
            dispPtn = transDispPtnDef.Shift;
        }

        //遷移前の破棄確認メッセージ表示
        confirmScrapBeforeTrans(appPath, transPtn, transDiv, transTarget, dispPtn, formNoW, null, btnCtrlId, rowNo, this, confirmFlg);
    });
}

/**
 * 個別実装ボタン　初期化処理
 *  @param {string} appPath ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {element}btn     ：ｲﾍﾞﾝﾄ発生ﾎﾞﾀﾝ
 */
function initIndividualImplBtn(appPath, btn) {

    //ｲﾍﾞﾝﾄﾊﾝﾄﾞﾗ設置
    $(btn).on('click', function () {

        if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

        // イベントを削除
        $(window).off('beforeunload');

        var formNoW = $(P_Article).data("formno");      //formNo取得
        var btnCtrlId = $(this).attr("name");           //ﾎﾞﾀﾝCTRLID

        //【オーバーライド用関数】個別実装ボタン
        clickIndividualImplBtn(appPath, formNoW, btnCtrlId);
    });
}

/**
 *  コード＋翻訳 選ボタンの初期化処理
 *  @param {string} appPath ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {element}btn     ：ｲﾍﾞﾝﾄ発生ﾎﾞﾀﾝ
 */
function initCodeTransBtn(appPath, btn) {
    // コード＋翻訳 選ボタンクリックイベントハンドラの設定
    $(btn).on('click', function () {

        if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

        //formNo取得
        var formNo = $(P_Article).data("formno");

        // イベントを削除
        $(window).off('beforeunload');

        var transPtn = transPtnDef.CmConduct;                       //共通機能
        var transTarget = $(this).data("childno");                  //共通機能ID
        var ctrlId = $(this).closest(".ctrlId").attr("data-ctrlid"); //一覧CTRLID
        var rowNo = $(this).closest("tr").attr("data-rowno");        //行番号

        //遷移前の破棄確認メッセージ表示off
        confirmScrapBeforeTrans(appPath, transPtn, transDivDef.None, transTarget, null, formNo, ctrlId, null, rowNo, this, false);
    });
}

/**
 * 共通機能用選択ボタン　初期化処理
 *  @param {string} appPath ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {element}btn     ：ｲﾍﾞﾝﾄ発生ﾎﾞﾀﾝ
 */
function initComConductSelectBtn(appPath, btn) {


    //ｲﾍﾞﾝﾄﾊﾝﾄﾞﾗ設置
    $(btn).on('click', function () {

        if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

        // イベントを削除
        $(window).off('beforeunload');

        var cmArticle = $(this).closest("article[name='common_area']");
        var cmConductId = $(cmArticle).attr("data-conductid");
        var fromConductId = $(cmArticle).attr("data-parentconductid");
        var fromCtrlId = $(P_Article).attr("data-fromctrlid");  //共通機能に遷移する際に押下したﾎﾞﾀﾝ/一覧ｺﾝﾄﾛｰﾙID

        //data-selflg属性に1:selFlgDef.Selectedをｾｯﾄ
        setAttrByNativeJs(cmArticle, "data-selflg", selFlgDef.Selected);

        //【オーバーライド用関数】個別実装ボタン
        clickComConductSelectBtn(appPath, cmConductId, cmArticle, this, fromCtrlId, fromConductId);

        //戻るﾎﾞﾀﾝをｸﾘｯｸ
        var backBtn = $(cmArticle).find("input:button[data-actionkbn='" + actionkbn.Back + "']");
        $(backBtn).click();
    });
}

/**
 *  選択ボタンの初期化処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 *  @param {byte} ：呼び出し元画面番号
 *  @param {byte} ：呼び出し元CtrlId
 */
function initSelectBtn(appPath, btn, formNo, ctrlid) {
    // 選択ボタンクリックイベントハンドラの設定
    $(btn).on('click', function () {

        if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

        //【オーバーライド用関数】選択ボタン
        initSelectBtnOriginal(appPath, formNo, this, ctrlid);
    });
}

/**
 *  一覧表示列設定ボタンの初期化
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 */
function initSetDispItemBtn(appPath, btns) {

    // ボタンクリックイベントハンドラの設定
    $.each(btns, function (idx, btn) {
        $(btn).on('click', function () {

            if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

            //表示ﾒｯｾｰｼﾞをｸﾘｱ
            var messageDiv = $("#setDispItem_message_div");
            $(messageDiv).children().remove();

            //「全て」以外のﾘｽﾄをｸﾘｱ
            var element = "#mltItem";
            var lis = $(element).children("li");
            if (lis != null && lis.length > 0) {
                $.each(lis, function (i, li) {
                    if (!$(li).hasClass("ctrloption")) {
                        $(li).remove();
                    }
                });
            }

            //「全て」ｵﾌﾟｼｮﾝ項目ﾁｪｯｸﾎﾞｯｸｽをﾁｪｯｸ：off
            var checkes = $(element).find(":checkbox.ctrloption:checked");
            if (checkes != null && checkes.length > 0) {
                $(checkes).prop('checked', false);
            }

            // 一覧表示項目複数選択ﾘｽﾄ初期化
            initListItemMultiSelectBox(appPath, btn, element);

            //ﾒｯｾｰｼﾞﾎﾟｯﾌﾟｱｯﾌﾟ表示
            $('#setDispItemModal').modal();
            var uploadModal = $("#fileUploadModal");
            if (uploadModal.length <= 0) {
                //※確認ﾒｯｾｰｼﾞを複数回表示する場合のおまじない
                $('#setDispItemModal').off('hidden.bs.modal');
                $('#setDispItemModal').on('hidden.bs.modal', function (e) {
                    $('.modal-backdrop').remove();
                });
            }

            return false;
        });
    });
}

/**
 *  一覧表示列設定適用ボタンの初期化
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 */
function initSetDispItemExecBtn(appPath, btn) {

    // ボタンクリックイベントハンドラの設定
    $(btn).on('click', function () {

        if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

        // エラー状態を初期化
        var element = $("#setDispItem_detail_div");
        var messageDiv = $("#setDispItem_message_div");
        // メッセージをクリア
        if ($(messageDiv).children().text() == P_ComMsgTranslated[941220007]) {
            clearMessage();
        }
        // エラー情報をクリア
        clearErrorClasses(element);

        // 入力ﾁｪｯｸ
        if (!validDispItemData()) {
            //入力ｴﾗｰ
            return false;
        }

        // 非表示項目取得
        var items = getHideItem();
        // 項目を一覧に反映する
        setDispItem(items);
    });
}

/**
 *  一覧表示列設定保存ボタンの初期化
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 */
function initSetDispItemSaveBtn(appPath, btn) {

    // ボタンクリックイベントハンドラの設定
    $(btn).on('click', function () {

        if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

        var conductIdW = $(this).closest("form").find("input:hidden[name='CONDUCTID']").val();

        // エラー状態を初期化
        var element = $("#setDispItem_detail_div");
        var messageDiv = $("#setDispItem_message_div");
        // メッセージをクリア
        if ($(messageDiv).children().text() == P_ComMsgTranslated[941220007]) {
            clearMessage();
        }
        // エラー情報をクリア
        clearErrorClasses(element);

        // 入力ﾁｪｯｸ
        if (!validDispItemData()) {
            //入力ｴﾗｰ
            return false;
        }

        // 非表示項目取得
        var items = getHideItem();
        // 処理継続用ｺｰﾙﾊﾞｯｸ関数を生成
        clickSetDispItemSaveBtn(appPath, btn, conductIdW, items);
        // 項目を一覧に反映する
        setDispItem(items);
    });
}
/**
 *  一覧表示列設定保存ボタン - 実行処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string} ：機能ID
 *  @param {list}   ：非表示項目ﾘｽﾄ
 */
function clickSetDispItemSaveBtn(appPath, btn, conductId, items) {

    // 実行中フラグON
    P_ProcExecuting = true;
    // ボタンを不活性化
    $(btn).prop("disabled", true);

    //エラー状態を初期化
    var element = $("#setDispItem_detail_div");
    clearErrorStatus(element);

    //処理中メッセージ：on
    processMessage(true);

    // 一覧CTRLID
    var ctrlId = $(element).find(":checkbox.ctrloption").prop('name');

    // 非表示項目番号(VAL～)
    var itemNo = "";
    $.each(items, function (i, item) {
        // ｶﾝﾏ区切りで設定する
        itemNo = itemNo + "," + item.replace("VAL", "");
    });
    if (itemNo.length > 0) {
        itemNo = itemNo.substr(1);  // 先頭ｶﾝﾏを削除
    }

    var condition = {
        itemNo: itemNo
    };
    var conditionList = []
    conditionList.push(condition);

    // POSTデータを生成
    var postdata = {
        conductId: conductId,           // メニューの機能ID
        ctrlId: ctrlId,                 // 一覧CTRLID
        conditionData: conditionList,   // 非表示項目番号(ｶﾝﾏ区切り)
    };

    // 登録処理実行
    $.ajax({
        url: appPath + 'api/CommonProcApi/' + actionkbn.ComSetDispItemSave,    // 保存
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(postdata),
        headers: { 'X-XSRF-TOKEN': getRequestVerificationToken() },
        traditional: true,
        cache: false
    }).then(
        // 1つめは通信成功時のコールバック
        function (resultInfo) {
            //正常時
            //[0]:処理ステータス - CommonProcReturn
            //[1]:結果データ - IList<Dictionary<string, object>>

            var status = resultInfo;

            //完了ﾒｯｾｰｼﾞを表示
            //setMessage(status.MESSAGE, status.STATUS);
            //addMessageLogNo(status.LOGNO, status.STATUS);

        },
        // 2つめは通信失敗時のコールバック
        function (resultInfo) {
            //異常時

            //処理結果ｽﾃｰﾀｽ(CommonProcReturn)
            var status = resultInfo.responseJSON;
            //処理継続用ｺｰﾙﾊﾞｯｸ関数を生成
            var confirmNo = 0;
            if (!isNaN(status.LOGNO)) {
                confirmNo = parseInt(status.LOGNO, 10);
            }
            var eventFunc = function () {
                clickSetDispItemSaveBtn(appPath, btn, conductId, items);
            }

            //処理結果ｽﾃｰﾀｽを画面状態に反映
            if (!setReturnStatus(appPath, status, eventFunc)) {
                return false;
            }

        }
    ).always(
        //通信の完了時に必ず実行される
        function (resultInfo) {
            //処理中メッセージ：off
            processMessage(false);
            // 実行中フラグOFF
            P_ProcExecuting = false;
            // ボタンを活性化
            $(btn).prop("disabled", false);
        });
}

/**
 *  一覧表示項目複数選択ﾘｽﾄの初期化処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 *  @param {string} ：対象セレクタ
 */
function initListItemMultiSelectBox(appPath, btn, selector) {
    //複数選択ｺﾝﾎﾞ(ﾘｽﾄ)の場合、ﾁｪｯｸﾎﾞｯｸｽ付きﾘｽﾄ<li><input type="checkbox">

    // 項目データを取得
    var id = $(btn).data('parentid');
    var formNo = $(P_Article).data("formno");
    var tbl = $('#' + id + '_' + formNo + ' thead');
    var base_tr = $(tbl).find('.base_tr_width');  //ﾍﾞｰｽﾚｲｱｳﾄ行
    var tds = $(base_tr).find("td[data-name^='VAL']");  //VAL1～

    //name属性
    var ctrlName = id;
    setAttrByNativeJs(selector, "name", ctrlName);   //要素名=id名

    //選択値
    var vals = $(selector).data("value") + '';   //ｶﾝﾏ区切り
    var aryVal = vals.split(',');

    //「全て」の項目を取得
    var optionli = $(selector).find('li.ctrloption');
    // :checkboxタグを取得
    var optioncheck = $(optionli).find(':checkbox');
    // name属性を設定
    setAttrByNativeJs(optioncheck, "name", ctrlName);
    //ﾁｪｯｸﾎﾞｯｸｽ選択時ｲﾍﾞﾝﾄ処理を設定
    $(optioncheck).on('change', function () {
        if ($(this).prop('checked')) {
            //ﾁｪｯｸ：onの場合

            if ($(this).hasClass("ctrloption")) {
                //「全て」ｵﾌﾟｼｮﾝ項目の場合、その他のﾁｪｯｸﾎﾞｯｸｽをﾁｪｯｸ：off
                var checkes = $(selector).find(":checkbox:not(.ctrloption):checked");
                if (checkes != null && checkes.length > 0) {
                    $(checkes).prop('checked', false);
                }
            } else {
                //「全て」ｵﾌﾟｼｮﾝ項目以外の場合、「全て」ｵﾌﾟｼｮﾝ項目ﾁｪｯｸﾎﾞｯｸｽをﾁｪｯｸ：off
                var checkes = $(selector).find(":checkbox.ctrloption:checked");
                if (checkes != null && checkes.length > 0) {
                    $(checkes).prop('checked', false);
                }
            }
        }
        //else {
        //    //ﾁｪｯｸ：offの場合
        //}

    });
    optioncheck = null;

    //「全て」をコピーして規定<li>を作成
    var baseli = $(optionli).clone(true);
    // :checkboxタグを取得
    var basecheck = $(baseli).find('input[type="checkbox"]');

    //「全て」の項目用の設定を除去
    $(baseli).removeClass("ctrloption");    //ﾚｲｱｳﾄ用に付与したｸﾗｽ
    $(baseli).removeClass("hide");          //ﾚｲｱｳﾄ用に付与したｸﾗｽ
    $(basecheck).removeClass("ctrloption");   //制御用に付与したｸﾗｽ
    $(basecheck).prop('checked', false);

    var childul = $('<ul>');
    $.each(tds, function (i, td) {

        var childli = baseli.clone(true);

        if (!$(td).hasClass('hide') || $(td).hasClass('defhide') || $(td).hasClass('userhide')) {

            // :checkboxタグを取得して選択ﾘｽﾄのｺｰﾄﾞ値をｾｯﾄ
            var checkW = $(childli).find('input[type="checkbox"]');
            var name = $(td).data('name');  //VAL1～
            $(checkW).val(name);
            //選択値の反映
            if (!$(td).hasClass('hide')) {
                //選択値の場合、選択状態にセット
                $(checkW).prop('checked', true);
            }

            //項目名
            var itemname = $(td).data('itemname');
            // :spanタグに選択ﾘｽﾄの項目名をｾｯﾄ
            var spanW = $(childli).find('span');
            $(spanW).text(itemname);

            $(childli).appendTo($(childul));
        }

    });

    var li = $('<li>');
    $(childul).appendTo($(li));
    $(li).appendTo($(selector));

}

/**
 *  一覧表示列設定の入力検証を行う
 *
 *  @return {bool} : ture(OK) false(NG)
 */
function validDispItemData() {

    // 入力ﾁｪｯｸ
    var valid = false;
    var element = $("#setDispItem_detail_div");
    var msul = $(element).find("ul.multiSelect");
    if (msul.length > 0) {
        var checkes = $(msul).find("> li :checkbox:checked");
        if (checkes != null && checkes.length > 0) {
            valid = true;
        }
    }

    if (!valid) {
        //個別エラー状態を初期化
        // メッセージをクリア
        clearMessage();
        //【CJ00000.W01】入力エラーがあります。
        addMessage([P_ComMsgTranslated[941220007]], messageType.Warning);
        return false;
    }

    return true;    //入力ﾁｪｯｸOK
}

/**
 *  非表示項目を取得する
 *
 *  @return {list} : 非表示項目
 */
function getHideItem() {

    var element = $("#setDispItem_detail_div");

    // 非表示項目の取得
    var items = [];

    // 「全て」の場合、取得項目なし
    var checkAll = $(element).find(":checkbox.ctrloption:checked");
    if (checkAll != null && checkAll.length > 0) {
        return items;
    }

    // 「全て」以外の場合、ﾁｪｯｸﾎﾞｯｸｽ未選択の項目を取得する
    var checkes = $(element).find(":checkbox:not(.ctrloption)");
    if (checkes != null && checkes.length > 0) {
        $.each(checkes, function (i, check) {
            if ($(check).prop('checked')) {
                // ﾁｪｯｸﾎﾞｯｸｽ選択
            } else {
                // ﾁｪｯｸﾎﾞｯｸｽ未選択
                items.push($(check).val());
            }
        });
    }

    return items;
}

/**
 *  項目を一覧に反映する
 *
 *  @return {list} :非表示項目
 */
function setDispItem(items) {

    var element = $("#setDispItem_detail_div");

    // id取得
    var id = $(element).find(":checkbox.ctrloption").prop('name');

    // thead
    var tbl = $(P_Article).find('#' + id + ' thead');
    var trs = $(tbl).find("tr");
    if (trs != null && trs.length > 0) {
        // テーブルの幅を取得
        var tblWidth = parseInt($(tbl).parent().width());
        $.each(trs, function (i, tr) {

            var tds;
            if ($(tr).hasClass('base_tr_width')) {   //ﾍﾞｰｽﾚｲｱｳﾄ行
                tds = $(tr).find("td[data-name^='VAL']");  //VAL1～                
            } else {
                tds = $(tr).find("th[data-name^='VAL']");  //VAL1～  
            }

            $.each(tds, function (i, td) {
                // 対象行の表示区分(表示／非表示)を取得する
                var userhide = false;
                $.each(items, function (i, item) {
                    if ($(td).data("name") == item) {
                        userhide = true;
                        return false;
                    }
                });
                // ベースレイアウト行のtdに設定されたwidthを取得
                var tdWidth = parseInt($(td).attr('width') ? $(td).attr('width') : 0);
                if (userhide) {
                    // 表示 → 非表示
                    if (!$(td).hasClass('hide')) {
                        $(td).addClass('hide');
                        $(td).addClass('userhide');
                        tblWidth -= tdWidth;
                    }
                } else {
                    // 非表示 → 表示
                    if ($(td).hasClass('defhide') || $(td).hasClass('userhide')) {
                        $(td).removeClass('hide');
                        $(td).removeClass('defhide');
                        $(td).removeClass('userhide');
                        tblWidth += tdWidth;
                    }
                }
            });

        });
        // 表示・非表示列の幅を調整した幅を設定
        $(tbl).parent().attr('width', tblWidth);
    }

    // tbody
    tbl = $(P_Article).find('#' + id + ' tbody');
    trs = $(tbl).find("tr");
    if (trs != null && trs.length > 0) {
        $.each(trs, function (i, tr) {

            var tds = $(tr).find("td[data-name^='VAL']");  //VAL1～

            $.each(tds, function (i, td) {
                // 対象行の表示区分(表示／非表示)を取得する
                var userhide = false;
                $.each(items, function (i, item) {
                    if ($(td).data("name") == item) {
                        userhide = true;
                        return false;
                    }
                });
                if (userhide) {
                    // 表示 → 非表示
                    if (!$(td).hasClass('hide')) {
                        $(td).addClass('hide');
                        $(td).addClass('userhide');
                    }
                } else {
                    // 非表示 → 表示
                    if ($(td).hasClass('defhide') || $(td).hasClass('userhide')) {
                        $(td).removeClass('hide');
                        $(td).removeClass('defhide');
                        $(td).removeClass('userhide');
                    }
                }
            });

        });
    }
}
// Template2.0 ADD end

/**
 *  CSVファイル取込ボタンの初期化処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 */
function initUploadBtn(appPath, btn) {
    // ボタンクリックイベントハンドラの設定
    $(btn).on('click', function () {

        if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

        // イベントを削除
        $(window).off('beforeunload');

        //formNo取得
        var formNoW = $(P_Article).data("formno");

        //【共通 - 取り込み機能】取込ﾎﾟｯﾌﾟｱｯﾌﾟ画面設定
        var formUpload = $("#formComUpload");
        //入力ﾁｪｯｸ、およびPost用ﾃﾞｰﾀの設定
        if (!uploadSubmitSetting(formNoW, this, formUpload, false)) {
            //※処理ｴﾗｰ
            return false;
        }

        //取込ﾌｧｲﾙ数の設定
        var fileCnt = $(this).data('filecnt');
        var trs = $(formUpload).find('table tr:not([class="base_tr_width"])');
        $.each(trs, function (idx, tr) {
            var rowno = $(tr).data('rowno');
            if (rowno <= fileCnt) {
                if ($(tr).hasClass('hide')) {
                    $(tr).removeClass('hide');
                }
            }
            else {
                if (false == $(tr).hasClass('hide')) {
                    $(tr).addClass('hide');
                }
            }
        });

        //「ｷｬﾝｾﾙ」ﾎﾞﾀﾝ制御
        var btnCancel = $("#ComUploadCancel");
        $(btnCancel).on("click", function () {
            var cancelFunc = function () {
                //【オーバーライド用関数】キャンセル押下後の個別実装
                clickPopupCancelBtn();
            }
            setTimeout(cancelFunc, 1000);
        });
        btnCancel = null;

        //==ﾌｧｲﾙ取込画面ﾞﾎﾟｯﾌﾟｱｯﾌﾟ表示==
        // - ﾎﾟｯﾌﾟｱｯﾌﾟ表示
        var modal = $('#fileUploadModal');
        $(modal).modal();
        $(modal).find('.modal-content').animate({ scrollTop: 0 }, '1');     //ｽｸﾛｰﾙを先頭へ移動
        //※複数回表示する場合のおまじない
        $(modal).off('hidden.bs.modal');
        $(modal).on('hidden.bs.modal', function (e) {
            $('.modal-backdrop').remove();
        });
        modal = null;
    });
}

/**
 *  入力ﾁｪｯｸ、およびPost用ﾃﾞｰﾀの設定
 *  @param {formNo} ：画面NO
 *  @param {btn} ：ｱｸｼｮﾝﾎﾞﾀﾝ
 *  @param {formElement} ：submitｴﾘｱのform要素
 */
function uploadSubmitSetting(formNo, btn, formElement, isEdit) {

    //処理対象ｴﾘｱの入力ﾁｪｯｸ
    var search_div = $(btn).closest(".action_search_div");
    if (search_div.length > 0) {
        var formSearch = $(P_Article).find("#" + P_formSearchId);

        //検索エリアのエラー状態を初期化(共通)
        clearErrorcomStatus(formSearch);
        // 検索条件ﾃﾞｰﾀの入力ﾁｪｯｸ
        if (!validConditionData()) {
            //入力ﾁｪｯｸｴﾗｰ
            return false;
        }
        $(formElement).find("input:hidden[name='ListData']").val("");    //submit用に明細ｴﾘｱﾃﾞｰﾀを初期化
    }
    else {

        P_formEditId = $(P_Article).find("form[id^='formEdit']").attr("id");
        //単票表示ｴﾘｱか判定
        var form;
        if (!isEdit) {
            form = $(P_Article).find("#" + P_formDetailId);
        }
        else {
            form = $(P_Article).find("#" + P_formEditId);
        }

        //明細エリアのエラー状態を初期化(共通)
        clearErrorcomStatus(form);

        if (isEdit) {
            if (!validFormEditData()) {
                return false;
            }
        }
        else {
            //ﾄｯﾌﾟｴﾘｱ・明細ｴﾘｱ・ﾎﾞﾄﾑｴﾘｱの入力ﾁｪｯｸ
            var topValid = validFormTopData();
            var listValid = validListData();
            var bottomValid = validFormBottomData();
            if (!topValid || !listValid || !bottomValid) {
                //入力ｴﾗｰ
                return false;
            }

            setSubmitDataList(formElement, formNo);                 //submit用に明細ｴﾘｱﾃﾞｰﾀを取得してｾｯﾄ
        }      
    }


    //Post用ﾃﾞｰﾀの設定
    var conductId = $(formElement).find("input:hidden[name='CONDUCTID']").val();
    $(formElement).find("input:hidden[name='FORMNO']").val(formNo);     //画面NOをｾｯﾄ
    setSubmitCtrlId(formElement, $(btn).attr("name"));                  //submit用にﾎﾞﾀﾝCtrlIdをｾｯﾄ
    setSubmitDataCondition(formNo, formElement);                        //submit用に検索条件を取得してｾｯﾄ
    $(formElement).find("input:hidden[name='ListDefines']").val(JSON.stringify(P_listDefines));
    //submit用にﾎﾞﾀﾝ定義情報ｾｯﾄ
    $(formElement).find("input:hidden[name='ButtonDefines']").val(JSON.stringify(P_buttonDefine[conductId]));
    $(formElement).find("input:hidden[name='ListIndividual']").val(JSON.stringify(P_dicIndividual));

    return true;   //処理OK
}

/**
 *  【共通 - バッチ機能】実行ボタンの初期化処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 */
function initBatExecBtn(appPath, btn) {
    // ボタンクリックイベントハンドラの設定
    $(btn).on('click', function () {

        if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

        var formTop = $(this).closest("article").find("form[id^='formTop']");
        var conductIdW = $(formTop).find("input:hidden[name='CONDUCTID']").val();
        var pgmIdW = $(formTop).find("input:hidden[name='PGMID']").val();
        var conductPtnW = $(formTop).find("input:hidden[name='CONDUCTPTN']").val();
        var formNoW = $(P_Article).data("formno");

        //検索エリアのエラー状態を初期化
        var element = $(P_Article).find("#" + P_formSearchId);
        clearErrorcomStatus(element);

        // 検索条件ﾃﾞｰﾀの入力ﾁｪｯｸ
        if (!validConditionData()) {
            //入力ﾁｪｯｸｴﾗｰ
            return false;
        }

        if (conductPtnW == conPtn.Input) {
            //エラー状態を初期化
            clearErrorComStatusForAreas(false);

            //ﾄｯﾌﾟｴﾘｱ・明細ｴﾘｱ・ﾎﾞﾄﾑｴﾘｱの入力ﾁｪｯｸ
            var topValid = validFormTopData();
            var listValid = validListData();
            var bottomValid = validFormBottomData();
            if (!topValid || !listValid || !bottomValid) {
                //入力ｴﾗｰ
                return false;
            }
        }

        var thisBtn = $(this);  // イベント発生ボタン

        var confirmKbn = $(thisBtn).data("relationparam") + "";  //確認メッセージ表示区分
        //ﾒｯｾｰｼﾞ設定
        setMessageStrForBtn(thisBtn, confirmKbn);

        //処理継続用ｺｰﾙﾊﾞｯｸ関数を生成
        var eventFunc = function () {
            clickBatExecBtnConfirmOK(appPath, conductIdW, pgmIdW, formNoW, thisBtn, conductPtnW);
        }
        // 確認メッセージ表示
        //『実行します。よろしいですか？』
        if (!popupMessage([P_MessageStr], messageType.Confirm, eventFunc, thisBtn)) {
            return false;
        }
    });
}

/**
 *  【共通 - バッチ機能】実行ボタン - 確認メッセージOK時、実行処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 */
function clickBatExecBtnConfirmOK(appPath, conductId, pgmId, formNo, btn, conductPtn) {

    // ボタンを非活性化
    //$(btn).prop("disabled", true);

    P_ProcExecuting = true; // 実行中フラグON

    //処理中メッセージ：on
    processMessage(true);
    if (P_IntervalId != null) {
        clearInterval(P_IntervalId);
        P_IntervalId = null;
    }

    var btnCtrlId = ctrlIdComBatExec;  //ﾃﾞﾌｫﾙﾄのCTRLID
    // 検索条件取得
    var tblSearch = getConditionTable();            //条件一覧要素
    var conditionDataList = [];   //条件ﾃﾞｰﾀﾘｽﾄ
    conditionDataList = getConditionData(formNo, tblSearch);    //条件ﾃﾞｰﾀ
    if (conductPtn == conPtn.Input) {
        btnCtrlId = $(btn).attr("name");
        ////ﾃﾞｰﾀ収集対象の一覧id取得
        //var targets = getTargetListElements(btn, false);
        //// 収集対象一覧の明細ﾃﾞｰﾀﾘｽﾄ(入力値)を取得
        //var listData = getListDataElements(targets, formNo, 0);    //ｺｰﾄﾞ値を採用
        var listData = getListDataAll(formNo, 0);
        conditionDataList = conditionDataList.concat(listData);
    }

    /* ボタン権限制御 切替 start ================================================ */
    //var btnDefines = P_buttonDefine;
    /*  ================================================ */
    var btnDefines = P_buttonDefine[conductId];
    /* ボタン権限制御 切替 end ================================================== */

    // POSTデータを生成
    var postdata = {
        conductId: conductId,                   // メニューの機能ID
        pgmId: pgmId,                           // メニューのプログラムID
        formNo: formNo,                         // 画面番号
        ctrlId: btnCtrlId,                      // ﾊﾞｯﾁ実行ボタンのCTRLID
        conditionData: conditionDataList,       // 検索条件入力データ
        listDefines: P_listDefines,             // 一覧定義情報
        ListIndividual: P_dicIndividual,           // 個別実装用汎用ﾘｽﾄ

        buttonDefines: btnDefines,          //ﾎﾞﾀﾝ権限情報　※ﾎﾞﾀﾝｽﾃｰﾀｽを取得

        browserTabNo: P_BrowserTabNo,           // ブラウザタブ識別番号
    };

    // 共通 - バッチ機能】再表示処理実行
    $.ajax({
        url: appPath + 'api/CommonProcApi/' + actionkbn.ComBatExec,    // 【共通 - バッチ機能】バッチ実行
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(postdata),
        headers: { 'X-XSRF-TOKEN': getRequestVerificationToken() },
        traditional: true,
        cache: false
    }).then(
        // 1つめは通信成功時のコールバック
        function (resultInfo) {
            var status = resultInfo[0];                     //[0]:処理ステータス - CommonProcReturn
            var data = separateDicReturn(resultInfo[1], conductId);    //[1]:結果データ - Dictionary<string, object>※結果ﾃﾞｰﾀ："Result"、個別実装用ﾃﾞｰﾀ："Individual"
            /* ボタン権限制御 切替 start ================================================ */
            //var authShori = resultInfo[2];                  //[2]:処理ｽﾃｰﾀｽ  - IList<{ﾎﾞﾀﾝCTRLID：ｽﾃｰﾀｽ}> ※ｽﾃｰﾀｽ：0:非表示、1(表示(活性))、2(表示(非活性))
            /* ボタン権限制御 切替 end ================================================== */

            // メッセージをクリア
            clearMessage();

            //処理メッセージを表示
            if (status.MESSAGE != null && status.MESSAGE.length > 0) {
                //正常時、正常ﾒｯｾｰｼﾞ
                //警告、異常時、ｴﾗｰﾒｯｾｰｼﾞ
                addMessage(status.MESSAGE, status.STATUS);
            }

            /* ボタン権限制御 切替 start ================================================ */
            ////処理結果ﾃﾞｰﾀを取得
            //dispBatStatusData(data, authShori);
            ////ﾎﾞﾀﾝｽﾃｰﾀｽ設定
            //setButtonStatus(authShori);
            /* ========================================================================== */
            //処理結果ﾃﾞｰﾀを取得
            dispBatStatusData(data);
            //ﾎﾞﾀﾝｽﾃｰﾀｽ設定
            setButtonStatus();
            /* ボタン権限制御 切替 end ================================================== */

        },
        // 2つめは通信失敗時のコールバック
        function (resultInfo) {
            // 処理失敗

            //[0]:処理ステータス - CommonProcReturn
            var result = resultInfo.responseJSON;
            var status;
            if (result.length > 1) {
                status = result[0];
                var data = separateDicReturn(result[1], conductId);

                //エラー詳細を表示
                dispErrorDetail(data);
            }
            else {
                status = result;
            }

            //確認ﾒｯｾｰｼﾞは無効
            if (status.STATUS == procStatus.Confirm) {
                status.STATUS == procStatus.Error;
            }

            //処理結果ｽﾃｰﾀｽを画面状態に反映
            if (!setReturnStatus(appPath, status, null)) {
                return false;
            }

        }
    ).always(
        //通信の完了時に必ず実行される
        function (resultInfo) {
            //処理中メッセージ：off
            processMessage(false);
            execBatRefresh();   //タイマー再開
            // 実行中フラグOFF
            P_ProcExecuting = false;
            //// ボタンを活性化
            //$(btn).prop("disabled", false);
        });

}

/**
 *  【共通 - バッチ機能】再表示ボタンの初期化処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 */
function initBatRefreshBtn(appPath, btn) {
    // ボタンクリックイベントハンドラの設定
    $(btn).on('click', function () {

        if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける
        var thisBtn = $(this);
        var formTop = $(thisBtn).closest("article").find("form[id^='formTop']");
        var conductIdW = $(formTop).find("input:hidden[name='CONDUCTID']").val();
        var pgmIdW = $(formTop).find("input:hidden[name='PGMID']").val();
        var formNoW = $(P_Article).data("formno");  //formNo
        var btnCtrlid = $(thisBtn).attr("name");       //btn_ctrlid

        P_ProcExecuting = true; // 実行中フラグON
        $(thisBtn).prop("disabled", true);     // ボタンを不活性化

        //処理中メッセージ：on
        processMessage(true);

        /* ボタン権限制御 切替 start ================================================ */
        //var btnDefines = P_buttonDefine;
        /*  ================================================ */
        var btnDefines = P_buttonDefine[conductIdW];
        /* ボタン権限制御 切替 end ================================================== */

        // POSTデータを生成
        var postdata = {
            conductId: conductIdW,               // メニューの機能ID
            pgmId: pgmIdW,                       // メニューのプログラムID
            formNo: formNoW,                    // 画面番号
            ctrlId: btnCtrlid,                  // 再表示ボタンの画面定義のコントロールID
            listDefines: P_listDefines,         // 一覧定義情報
            ListIndividual: P_dicIndividual,    // 個別実装用汎用ﾘｽﾄ

            buttonDefines: btnDefines,      //ﾎﾞﾀﾝ権限情報　※ﾎﾞﾀﾝｽﾃｰﾀｽを取得

            browserTabNo: P_BrowserTabNo,       // ブラウザタブ識別番号
        };

        // 共通 - バッチ機能】再表示処理実行
        $.ajax({
            url: appPath + 'api/CommonProcApi/' + actionkbn.ComBatRefresh,    // 【共通 - バッチ機能】再表示
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json',
            data: JSON.stringify(postdata),
            headers: { 'X-XSRF-TOKEN': getRequestVerificationToken() },
            traditional: true,
            cache: false
        }).then(
            // 1つめは通信成功時のコールバック
            function (resultInfo) {


                //[2]:処理ｽﾃｰﾀｽ  - IList<{ﾎﾞﾀﾝCTRLID：ｽﾃｰﾀｽ}> ※ｽﾃｰﾀｽ：0:非表示、1(表示(活性))、2(表示(非活性))
                var status = resultInfo[0];                     //[0]:処理ステータス - CommonProcReturn
                var data = separateDicReturn(resultInfo[1], conductIdW);    //[1]:結果データ - Dictionary<string, object>※結果ﾃﾞｰﾀ："Result"、個別実装用ﾃﾞｰﾀ："Individual"
                /* ボタン権限制御 切替 start ================================================ */
                //var authShori = resultInfo[2];                  //[2]:処理ｽﾃｰﾀｽ  - IList<{ﾎﾞﾀﾝCTRLID：ｽﾃｰﾀｽ}> ※ｽﾃｰﾀｽ：0:非表示、1(表示(活性))、2(表示(非活性))
                /* ボタン権限制御 切替 end ================================================== */

                // メッセージをクリア
                clearMessage();

                //処理メッセージを表示
                if (status.MESSAGE != null && status.MESSAGE.length > 0) {
                    //正常時、正常ﾒｯｾｰｼﾞ
                    //警告、異常時、ｴﾗｰﾒｯｾｰｼﾞ
                    addMessage(status.MESSAGE, status.STATUS);
                }

                /* ボタン権限制御 切替 start ================================================ */
                ////処理結果ﾃﾞｰﾀを取得
                //dispBatStatusData(data, authShori);
                ////ﾎﾞﾀﾝｽﾃｰﾀｽ設定
                //setButtonStatus(authShori);
                /* ========================================================================== */
                //処理結果ﾃﾞｰﾀを取得
                dispBatStatusData(data);
                //ﾎﾞﾀﾝｽﾃｰﾀｽ設定
                setButtonStatus();
                /* ボタン権限制御 切替 end ================================================== */

            },
            // 2つめは通信失敗時のコールバック
            function (resultInfo) {
                // 処理失敗

                //[0]:処理ステータス - CommonProcReturn
                var result = resultInfo.responseJSON;
                var status;
                if (result.length > 1) {
                    status = result[0];
                    var data = separateDicReturn(result[1], conductIdW);

                    //エラー詳細を表示
                    dispErrorDetail(data);
                }
                else {
                    status = result;
                }

                //確認ﾒｯｾｰｼﾞは無効
                if (status.STATUS == procStatus.Confirm) {
                    status.STATUS == procStatus.Error;
                }

                //処理結果ｽﾃｰﾀｽを画面状態に反映
                if (!setReturnStatus(appPath, status, null)) {
                    return false;
                }
            }
        ).always(
            //通信の完了時に必ず実行される
            function (resultInfo) {
                //処理中メッセージ：off
                processMessage(false);
                // 実行中フラグOFF
                P_ProcExecuting = false;
                // ボタンを活性化
                $(thisBtn).prop("disabled", false);
            });  //$.ajax({

    });
}

/**
 *  一定間隔でﾊﾞｯﾁ再表示を実行の初期化
 *  @param {string} btnCtrlid : ﾊﾞｯﾁ再表示ﾎﾞﾀﾝのbtn_ctrlid
 */
function execBatRefresh(btnCtrlid) {

    if (btnCtrlid == null) {
        btnCtrlid = "ComBatRefresh" //ptn=11（ﾊﾞｯﾁ機能）
    }

    P_IntervalId = setInterval(function () {
        var btn = $(P_Article).find('input:button[name="' + btnCtrlid + '"]');
        if ($(btn).length > 0) {
            $(btn).click();
        }
    }, 10000);     //10秒間隔
}

/**
 *  ﾊﾞｯﾁ実行結果ﾃﾞｰﾀをﾊﾞｯﾁｽﾃｰﾀｽ一覧に表示
 *  @datas {object} ：ﾊﾞｯﾁ実行結果ﾃﾞｰﾀ [List<Dictionary<string, object>>]
 */
/* ボタン権限制御 切替 start ================================================ */
//function dispBatStatusData(datas, authShori) {
//    if (datas == null) {
//        return;   //ﾃﾞｰﾀなし
//    }
//    //ﾊﾞｯﾁ定型ﾃｰﾌﾞﾙのCTRLID
//    var ctrlId = $(P_Article).find("table[id$='_J']").attr("id").slice(0, -2);

//    var data = null;
//    var isfirst = true;
//    $.each(datas, function (idx, info) {
//        if (info.CTRLID == ctrlId) {
//            if (isfirst) {
//                //先頭ﾃﾞｰﾀをｽｷｯﾌﾟ
//                isfirst = false;
//                return true;    //continue
//            }
//            data = info;
//            return false;   //break
//        }
//    });
//    //処理を共通化↓
//    dispBatStatusDataDetail(data, ctrlId, authShori);

//    //ﾍﾟｰｼﾞの状態を検索後に設定
//    setPageStatus(pageStatus.SEARCH, 1, conPtn.Bat);
//}
/* ============================================================================================================ */
function dispBatStatusData(datas) {
    if (datas == null) {
        return;   //ﾃﾞｰﾀなし
    }
    //ﾊﾞｯﾁ定型ﾃｰﾌﾞﾙのCTRLID
    var ctrlId = $(P_Article).find("table[id$='_J']").attr("id").slice(0, -2);

    var data = null;
    var isfirst = true;
    $.each(datas, function (idx, info) {
        if (info.CTRLID == ctrlId) {
            if (isfirst) {
                //先頭ﾃﾞｰﾀをｽｷｯﾌﾟ
                isfirst = false;
                return true;    //continue
            }
            data = info;
            return false;   //break
        }
    });
    //処理を共通化↓
    dispBatStatusDataDetail(data, ctrlId);

    //ﾍﾟｰｼﾞの状態を検索後に設定
    setPageStatus(pageStatus.SEARCH, 1, conPtn.Bat);
}
/* ボタン権限制御 切替 end ================================================ */

/**
 * ﾊﾞｯﾁ実行結果ﾃﾞｰﾀをﾊﾞｯﾁｽﾃｰﾀｽ一覧に表示(共通部分)
 * @param {object} data         : ﾊﾞｯﾁ実行結果ﾃﾞｰﾀ
 * @param {string} ctrlId       : ﾊﾞｯﾁｽﾃｰﾀｽ一覧CTRLID
 */
/* ボタン権限制御 切替 start ================================================ */
//function dispBatStatusDataDetail(data, ctrlId, authShori) {

//    //処理結果ﾃﾞｰﾀを表示
//    //VAL1 - 開始日時
//    //VAL2 - 終了日時
//    //VAL3 - ユーザー
//    //VAL4 - 処理結果
//    //VAL5 - 処理メッセージ
//    //VAL6 - 関連ファイル
//    //VAL7 - 表示設定スタイル(※スタイル設定用)
//    //VAL8 - 実行中フラグ(0:未実行、1:実行中)
//    //VAL9 - 実行条件
//    if (data == null || data.length <= 0) {
//        data = {
//            VAL1: "",
//            VAL2: "",
//            VAL3: "",
//            VAL4: "",
//            VAL5: "実行履歴がありません。",
//            VAL6: "",
//            VAL7: "",
//            VAL8: "",
//            VAL9: "",
//        };
//    }

//    var tables = $(P_Article).find('[id^="' + ctrlId + '"]');
//    for (var colNo = 1; colNo <= 9; colNo++) {
//        var key = "VAL" + colNo;

//        var td = $(tables).find("tbody td[data-name='" + key + "']");
//        if (td != null && td.length > 0) {
//            var val = data[key];
//            if (val == null) {
//                val = "";
//            }
//            if (colNo == 6) {   //VAL6 - 関連ファイル
//                //<a>ﾘﾝｸに設定
//                var a = $(td).find("a");
//                $(a).text(val);
//                if (val == null || val.length <= 0) {
//                    //属性を削除
//                    $(a).removeAttr("href");
//                    $(a).removeAttr("target");
//                }
//                else {
//                    //属性を設定
//                    setAttrByNativeJs(a, "href", val);
//                    setAttrByNativeJs(a, "target", "_blank");
//                }
//            }
//            else {
//                td.text(val);
//                if (colNo == 4) {   //VAL4 - 処理結果
//                    var cssClass = data["VAL10"];
//                    //ｽﾀｲﾙを初期化
//                    $(td).removeClass();
//                    //ｽﾀｲﾙを適用
//                    if (cssClass != null && cssClass.length > 0) {
//                        $(td).addClass(cssClass);
//                    }
//                }
//                else if (colNo == 5) {   //VAL5 - 処理メッセージ
//                    var cssClass = data["VAL7"];
//                    //ｽﾀｲﾙを初期化
//                    $(td).removeClass();
//                    //ｽﾀｲﾙを適用
//                    if (cssClass != null && cssClass.length > 0) {
//                        $(td).addClass(cssClass);
//                    }
//                }
//            }
//        }
//    }

//    //VAL8 - 実行中ﾌﾗｸﾞ(0:未実行、1:実行中)の状態を反映
//    var execFlg = 0;
//    if (data["VAL8"] != null && (data["VAL8"] + "") == "1") {
//        execFlg = 1;
//    }
//    //実行中の場合、ﾊﾞｯﾁ実行ﾎﾞﾀﾝを非活性
//    if (execFlg == 1) {
//        var batExecCtrlid = $("input:button[data-actionkbn='" + actionkbn.ComBatExec + "']").attr("name");
//        /* ボタン権限制御 切替 start ================================================ */
//        if (authShori == null) {
//            authShori = {};
//            authShori[batExecCtrlid] = btnDispKbnDef.Disabled;  //非活性
//        }
//        else {
//            authShori[batExecCtrlid] = btnDispKbnDef.Disabled;  //非活性
//        }
//    }
//}
/* ========================================================================== */
function dispBatStatusDataDetail(data, ctrlId) {

    //処理結果ﾃﾞｰﾀを表示
    //VAL1 - 開始日時
    //VAL2 - 終了日時
    //VAL3 - ユーザー
    //VAL4 - 処理結果
    //VAL5 - 処理メッセージ
    //VAL6 - 関連ファイル
    //VAL7 - 表示設定スタイル(※スタイル設定用)
    //VAL8 - 実行中フラグ(0:未実行、1:実行中)
    //VAL9 - 実行条件
    if (data == null || data.length <= 0) {
        data = {
            VAL1: "",
            VAL2: "",
            VAL3: "",
            VAL4: "",
            VAL5: "実行履歴がありません。",
            VAL6: "",
            VAL7: "",
            VAL8: "",
            VAL9: "",
        };
    }

    var tables = $(P_Article).find('[id^="' + ctrlId + '"]');
    for (var colNo = 1; colNo <= 9; colNo++) {
        var key = "VAL" + colNo;

        var td = $(tables).find("tbody td[data-name='" + key + "']");
        if (td != null && td.length > 0) {
            var val = data[key];
            if (val == null) {
                val = "";
            }
            if (colNo == 6) {   //VAL6 - 関連ファイル
                //<a>ﾘﾝｸに設定
                var a = $(td).find("a");
                $(a).text(val);
                if (val == null || val.length <= 0) {
                    //属性を削除
                    $(a).removeAttr("href");
                    $(a).removeAttr("target");
                }
                else {
                    //属性を設定
                    setAttrByNativeJs(a, "href", val);
                    setAttrByNativeJs(a, "target", "_blank");
                }
            }
            else {
                td.text(val);
                if (colNo == 4) {   //VAL4 - 処理結果
                    var cssClass = data["VAL10"];
                    //ｽﾀｲﾙを初期化
                    $(td).removeClass();
                    //ｽﾀｲﾙを適用
                    if (cssClass != null && cssClass.length > 0) {
                        $(td).addClass(cssClass);
                    }
                }
                else if (colNo == 5) {   //VAL5 - 処理メッセージ
                    var cssClass = data["VAL7"];
                    //ｽﾀｲﾙを初期化
                    $(td).removeClass();
                    //ｽﾀｲﾙを適用
                    if (cssClass != null && cssClass.length > 0) {
                        $(td).addClass(cssClass);
                    }
                }
            }
        }
    }

    //VAL8 - 実行中ﾌﾗｸﾞ(0:未実行、1:実行中)の状態を反映
    var execFlg = 0;
    if (data["VAL8"] != null && (data["VAL8"] + "") == "1") {
        execFlg = 1;
    }
    //実行中の場合、ﾊﾞｯﾁ実行ﾎﾞﾀﾝを非活性
    if (execFlg == 1) {
        var batExecCtrlid = $("input:button[data-actionkbn='" + actionkbn.ComBatExec + "']").attr("name");
        var formNo = $(P_Article).data("formno");
        var conductId = $(P_Article).find("form[id^='formTop'] input:hidden[name='CONDUCTID']");
        if (P_buttonDefine == null) {
            P_buttonDefine = {};
            var btnAuthList = createDisabledBatExecBtnDefine(formNo, batExecCtrlid);
            P_buttonDefine[conductId] = btnAuthList;
        }
        else if (!P_buttonDefine[conductId]) {
            var btnAuthList = createDisabledBatExecBtnDefine(formNo, batExecCtrlid);
            P_buttonDefine[conductId] = btnAuthList;
        }
        else {
            $.each(P_buttonDefine[conductId], function (idx, btnAuth) {
                if (btnAuth.FORMNO == formNo && btnAuth.CTRLID == batExecCtrlid) {
                    btnAuth.DISPKBN = btnDispKbnDef.Disabled;
                    return false;   //break;
                }
            });
        }
    }
}
function createDisabledBatExecBtnDefine(formNo, batExecCtrlid) {

    var btnAuthList = [];
    var btnAuth = {
        FORMNO: formNo,
        CTRLID: batExecCtrlid,
        AUTHFLG: authControlKbnDef.Control, //権限管理あり
        DISPKBN: btnDispKbnDef.Disabled,  //非活性
        AUTHKBN: "-",
    }
    btnAuthList.push(btnAuth);
    return btnAuthList;
}
/* ボタン権限制御 切替 end ================================================ */

/**
 *  【共通 - マスタ機能】検索条件表示切替ボタンの初期化処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 */
function initSwitchBtn(appPath, btn) {
    // ボタンクリックイベントハンドラの設定
    $(btn).on('click', function () {
        var formNo = $(P_Article).data("formno");
        var id = $(this).data('switchid');

        if (('sessionStorage' in window) && (window.sessionStorage !== null)) {
            // 表示状態を退避
            if (id == "top_menu" || id == "side_menu" || id == "tree_view") {
                var state = "";

                //if (!isHideId(id) == true) {
                if (!isHide($('#' + id)) == true) {
                    state = "hide";
                    //ﾒﾆｭｰｱｲｺﾝの表示切替
                    var icon = $(this).children(".ham");
                    icon.toggleClass("select");      // ｱｲｺﾝ選択
                }
                else {
                    state = "";
                }

                sessionStorage.setItem("CIM_MENU_STATE", state);
            }
        }

        //表示/非表示切替
        var element = $(P_Article).find('#' + id + "_" + formNo);
        if (element.length > 0) {
            //※画面NOｴﾘｱ内の要素
            setHide(element, !isHide(element));
        }
        else {
            element = $('#' + id);
            setHide(element, !isHide(element));
        }

        // 横方向一覧の列固定位置を再設定
        setFixColCss();

        if (id == "app_navi_menu") {
            var icon = $(this).children(".glyphicon")
            if (!isHide(icon)) {
                //ﾒﾆｭｰｱｲｺﾝの表示切替
                icon.toggleClass("select");      // ｱｲｺﾝ選択
            }
        }

        // フォーカス再設定
        removeFocus();
        nextFocus();
    });
}

/**
 *  【共通 - 取り込み機能】データ取込ダイアログ - 取り込みボタンの初期化処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 */
function initComUploadBtn(appPath, btn, FileSize) {
    // ボタンクリックイベントハンドラの設定
    $(btn).on('click', function () {

        if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

        var form = $(this).closest("article").find("form[id^='form']");
        var conductIdW = $(form).find("input:hidden[name='CONDUCTID']").val();
        var pgmIdW = $(form).find("input:hidden[name='PGMID']").val();

        var formNoW = $(P_Article).data("formno");
        var isEdit = false;
        var conductPtnW = $(form).find("input:hidden[name='CONDUCTPTN']").val();
        var actionKbn = $(this).data("actionkbn");

        //submitｴﾘｱのform要素
        var form = null;
        var isformPopup = false;
        if ($(this).closest("#fileUploadModal").length > 0) {
            //【共通 - 取込機能】取込ﾎﾟｯﾌﾟｱｯﾌﾟ画面
            form = $("#formComUpload");
            isformPopup = true;     //取込ﾎﾟｯﾌﾟｱｯﾌﾟ
        }
        else {
            var search_div = $(btn).closest(".action_search_div");
            if (search_div.length > 0 || actionKbn == actionkbn.ExcelPortUpload) {
                //条件ｴﾘｱ
                form = $(P_Article).find("form[id^='formSearch']");
            }
            else {
                //単票表示ｴﾘｱか判定
                isEdit = judgeBtnIsEditPosition(this);

                if (!isEdit) {
                    //明細ｴﾘｱ
                    form = $(P_Article).find("form[id^='formDetail']");
                }
                else {
                    //単票ｴﾘｱ
                    form = $(P_Article).find("form[id^='formEdit']");
                }
            }

            //入力ﾁｪｯｸ、およびPost用ﾃﾞｰﾀの設定
            if (!uploadSubmitSetting(formNoW, this, form, isEdit)) {
                //※処理ｴﾗｰ
                return false;
            }
        }

        // 【オーバーライド用関数】取込処理個別入力チェック
        var [isContinue, isError, isAutoBackFlg] = preInputCheckUpload(appPath, conductIdW, formNoW);

        // 個別入力チェックでエラーの場合
        if (isError) {
            return false;
        }

        // 個別入力チェック後も入力チェックを行う場合
        if (isContinue) {

            //ﾌｧｲﾙのValidation
            var elements = $(form).find("input:file");
            if (elements == null || elements.length <= 0) {
                if (isformPopup) {
                    //『ファイルを指定してください。』
                    popupMessage([P_ComMsgTranslated[941280002]], messageType.Warning);
                }
                else {
                    //『入力エラーがあります』
                    addMessage([P_ComMsgTranslated[941220007]], messageType.Warning);
                }
                return false;
            }

            var errorFlg = 0;
            var fileNames = [];
            var colNames_none = [];     //必須で未設定の項目名を退避
            var colNames_ok = [];       //必須で設定済みの項目名を退避
            var selectFlg = false;      //ｱｯﾌﾟﾛｰﾄﾞ対象ﾌｧｲﾙ有無ﾌﾗｸﾞ
            $.each(elements, function (idx, element) {
                // 入力チェック
                var file = element.files[0];
                var colName = $(element).data("colname");

                // ①ファイルが未選択
                // ②ファイルが存在しない
                if (file == null || file.name == null || file.name.length <= 0) {
                    if ($(element).hasClass("validate_required") && colNames_ok.indexOf(colName) < 0) {
                        //※必須項目で1件も設定されていない場合

                        //すでに設定済みか？
                        if (colNames_none.indexOf(colName) < 0) {
                            //未設定の項目名を退避
                            colNames_none.push(colName);
                        }
                    }

                    return true;    //※continue
                }
                else {
                    if ($(element).hasClass("validate_required")) {
                        //※必須項目の場合

                        //未設定項目名から除外
                        var index = colNames_none.indexOf(colName);
                        colNames_none.splice(index, 1);
                        //設定済み項目名を退避
                        colNames_ok.push(colName);
                    }
                }
                selectFlg = true;   //ｱｯﾌﾟﾛｰﾄﾞ対象ﾌｧｲﾙあり

                var fileName = file.name;
                var fileSize = file.size;
                var fileType = file.type;

                // ③ファイルサイズが大きすぎる
                /*
                 * ファイルサイズ入力チェックはWAFで行うので不要
                if (fileSize > FileSize) {
                    if (isformPopup) {
                        //『ファイルサイズが大きすぎます。』
                        popupMessage([P_ComMsgTranslated[941280003]], messageType.Warning);
                        $(element).addClass("errorcom");
                    }
                    else {
                        if (errorFlg != 1) addMessage([P_ComMsgTranslated[941220007]], messageType.Warning);
                        addErrorPlacement([P_ComMsgTranslated[941280003]], element);
                    }
                    errorFlg = 1;
                    return true;    //※continue
                }
                */

                // ④ファイル拡張子が指定されたものに一致しない
                //  ※ファイルタイプ
                var accept = $(element).attr("accept");
                if (accept.length > 0) {
                    var types = accept.split(',');
                    if ($.inArray("*.*", types) < 0) {
                        //※すべてのﾌｧｲﾙが指定されてない場合、ﾁｪｯｸ

                        var names = fileName.split('.');
                        var ext = "." + names[names.length - 1];
                        if ($.inArray(ext.toLowerCase(), types) < 0) {
                            if (isformPopup) {
                                //『ファイル形式をが有効ではありません。』
                                popupMessage([P_ComMsgTranslated[941280004]], messageType.Warning);
                                $(element).addClass("errorcom");
                            }
                            else {
                                if (errorFlg != 1) addMessage([P_ComMsgTranslated[941220007]], messageType.Warning);
                                addErrorPlacement([P_ComMsgTranslated[941280004]], element);
                            }
                            errorFlg = 1;
                            return true;    //※continue
                        }

                    }
                }

                //※共通FW側では実施しない
                //// ⑤指定ファイル名の重複チェック
                //if (fileNames.indexOf(fileName) >= 0) {
                //    if (isformPopup) {
                //        //『ファイル名が重複しています。』
                //        popupMessage([P_ComMsgTranslated[941280005]], messageType.Warning);
                //        $(element).addClass("errorcom");
                //    }
                //    else {
                //        if (errorFlg != 1) addMessage([P_ComMsgTranslated[941220007]], messageType.Warning);
                //        addErrorPlacement([P_ComMsgTranslated[941280005]], element);
                //    }
                //    errorFlg = 1;
                //    return true;    //※continue
                //}
                ////ファイル名を退避（※重複チェック用）
                //fileNames.push(fileName);
            });
            if (colNames_none.length > 0) {
                //必須項目で条件未設定の場合、エラー表示
                $.each(colNames_none, function (idx, colNameNon) {
                    var errelms = $("input:file[data-colname='" + colNameNon + "']");
                    if (isformPopup) {
                        //『ファイルを指定してください。』
                        popupMessage([P_ComMsgTranslated[941280002]], messageType.Warning);
                        $(errelms).addClass("errorcom");
                    }
                    else {
                        if (errorFlg != 1) addMessage([P_ComMsgTranslated[941220007]], messageType.Warning);
                        //addErrorPlacement([P_ComMsgTranslated[941280002]], errelms);
                        $(errelms).addClass("errorcom");
                    }
                    errorFlg = 1;
                });

            }
            if (errorFlg == 1) {
                return false;
            }

            if (selectFlg == false) {
                if (isformPopup) {
                    //『ファイルを指定してください。』
                    popupMessage([P_ComMsgTranslated[941280002]], messageType.Warning);
                    $(elements).addClass("errorcom");
                }
                else {
                    addMessage([P_ComMsgTranslated[941220007]], messageType.Warning);
                    //addErrorPlacement([P_ComMsgTranslated[941280002]], elements);
                    $(elements).addClass("errorcom");
                }
                return false;
            }
        }

        var ctrlId = $(this).attr("name");
        var relationParam = $(this).data("relationparam") + ""; // 関連情報パラメータ
        var confirmKbn = confirmKbnDef.Disp;
        var relationList = relationParam.split('|');
        $(relationList).each(function (i, relation) {
            if (i == 0) {
                // i=0:確認ﾒｯｾｰｼﾞの有無
                if (relation != null && relation != "") {
                    confirmKbn = relation;
                }
            }
        });
        //ﾒｯｾｰｼﾞ設定
        setMessageStrForBtn(this, confirmKbn);

        //処理継続用ｺｰﾙﾊﾞｯｸ関数を生成
        var eventFunc = function () {
            if (actionKbn != actionkbn.ExcelPortUpload) {
                clickComUploadBtnConfirmOK(appPath, btn, conductIdW, pgmIdW, formNoW, ctrlId, form, isEdit, conductPtnW, isAutoBackFlg);
            } else {
                clickExcelPortUploadBtnConfirmOK(appPath, btn, conductIdW, pgmIdW, formNoW, ctrlId, actionKbn, form, isEdit, conductPtnW, isAutoBackFlg, 0);
            }
        }

        // 確認メッセージを表示
        //『（ボタン名）します。よろしいですか？』
        if (!popupMessage([P_MessageStr], messageType.Confirm, eventFunc, btn)) {
            //『キャンセル』の場合、処理中断
            return false;
        }

        // 確認メッセージ表示
        //『ファイルを取り込みます。よろしいですか？』
        //if (!popupMessage([P_ComMsgTranslated[941280006]], messageType.Confirm, eventFunc, btn)) {
        //    return false;
        //}

        //※確認メッセージ「OK」の場合、下記関数処理を実施
        //イベント関数：clickComUploadBtnConfirmOK

    });
}

/**
 *  【共通 - 取り込み機能】データ取込ボタン - 確認メッセージOK時、実行処理
 *  @param {appPath}    ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {btn}        ：ボタン要素
 *  @param {conductId}  ：機能ID
 *  @param {pgmId}      ：プログラムID
 *  @param {formNo}     ：画面番号
 *  @param {ctrlId}     ：ボタンCtrlId
 *  @param {form}       ：submitｴﾘｱのform要素
 */
function clickComUploadBtnConfirmOK(appPath, btn, conductId, pgmId, formNo, ctrlId, form, isEdit, conductPtn, autoBackFlg) {

    // 実行中フラグON
    P_ProcExecuting = true;
    // ボタンを不活性化
    //$(btn).prop("disabled", true);

    //==ﾌｧｲﾙ取込画面ﾞﾎﾟｯﾌﾟｱｯﾌﾟ表示==
    // - ﾎﾟｯﾌﾟｱｯﾌﾟ非表示
    var modal = $('#fileUploadModal');
    $(modal).modal('hide');

    // FormDataの中にファイル情報も含まれるため、個別のファイル要素の取得は不要
    ////ﾌｧｲﾙ要素を取得
    //var files = $(form).find("input:file");

    //FormData生成
    var formData = new FormData($(form).get(0));
    //formData.append("UploadFile", files);

    // クリックされたボタンを判定
    var btnClickedBtn;
    $.each(btn, function (index, element) {
        var tmpPgmId = getProgramIdByElement(element);
        if (tmpPgmId == pgmId) {
            btnClickedBtn = element;
        }
    })

    //ﾃﾞｰﾀ収集対象の一覧取得
    var targets = getTargetListElements(btnClickedBtn, isEdit);
    // 収集対象一覧の明細ﾃﾞｰﾀﾘｽﾄ(入力値)を取得
    var listData = getListDataElements(targets, formNo, 0);    //ｺｰﾄﾞ値を採用
    formData.append("RegistData", JSON.stringify(listData));

    // 【オーバーライド用関数】追加条件取得処理
    var addConditionData = addSearchConditionDictionaryForRegist(appPath, conductId, formNo, btn);
    if (addConditionData.length) {
        formData.append("AddRegistData", JSON.stringify(addConditionData));
    }
    //個別実装用データリストを再設定（オーバライド用関数内で設定する場合を考慮）
    formData.set("ListIndividual", JSON.stringify(P_dicIndividual));
    //ブラウザタブ識別番号を設定
    formData.set("browserTabNo", P_BrowserTabNo);

    var formNo = 0;
    var ctrlId = "";
    var input = $(form).find("input:hidden[name='FORMNO']");
    if (input != null && input.length > 0) {
        formNo = input.val();
    }
    input = $(form).find("input:hidden[name='CTRLID']");
    if (input != null && input.length > 0) {
        ctrlId = input.val();
    }

    $.ajax({
        url: appPath + 'Common/FileUpload/',    // ファイル取込
        method: 'POST',
        data: formData,
        contentType: false,
        processData: false
    }).then(
        // 1つめは通信成功時のコールバック
        function (resultInfo) {
            //正常時
            var status = resultInfo[0];                     //[0]:処理ステータス - CommonProcReturn
            var data = separateDicReturn(resultInfo[1], conductId);    //[1]:結果データ - Dictionary<string, object>※結果ﾃﾞｰﾀ："Result"、個別実装用ﾃﾞｰﾀ："Individual"

            //完了ﾒｯｾｰｼﾞを表示
            setMessage(status.MESSAGE, status.STATUS);
            addMessageLogNo(status.LOGNO, status.STATUS);

            // エラー情報を表示
            dispErrorDetail(data, true);

            // 正常の場合
            if (status.STATUS == procStatus.Valid ||
                status.STATUS == procStatus.Warning ||
                status.STATUS == procStatus.WarnDisp) {

                // 結果ﾃﾞｰﾀをｾｯﾄ
                /* ボタン権限制御 切替 start ================================================ */
                //setExecuteResults(appPath, conductId, pgmId, formNo, ctrlId, conductPtn, isEdit, autoBackFlg, data, authShori);
                //ﾀﾌﾞ内ﾎﾞﾀﾝか判定
                var tab = $(btn).closest(".tab_contents");
                var isTab = $(tab).length;
                setExecuteResults(appPath, conductId, pgmId, formNo, ctrlId, conductPtn, isEdit, autoBackFlg, data, status, isTab);
                /* ボタン権限制御 切替 end ================================================== */

            }

            //【オーバーライド用関数】実行正常終了後処理
            postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data, status);

            //ﾌｧｲﾙ情報をｸﾘｱ ※連続処理を制御
            $(P_Article).find("input:file").val("");

            ////正常時
            ////[0]:処理ステータス - CommonProcReturn
            ////[1]:結果データ - Dictionary<string, object>※結果ﾃﾞｰﾀ："Result"、個別実装用ﾃﾞｰﾀ："Individual"
            ////[2]:処理ｽﾃｰﾀｽ  - IList<{ﾎﾞﾀﾝCTRLID：ｽﾃｰﾀｽ}> ※ｽﾃｰﾀｽ：0:非表示、1(表示(活性))、2(表示(非活性))

            ////異常時
            ////※ビジネスロジック処理の異常時もここで受け取り

            ////[0]:処理ステータス - CommonProcReturn
            //var status = resultInfo[0];

            ////完了ﾒｯｾｰｼﾞ、またはｴﾗｰﾒｯｾｰｼﾞを表示
            //setMessage(status.MESSAGE, status.STATUS);
            //addMessageLogNo(status.LOGNO, status.STATUS);

            //if (status.STATUS == procStatus.Valid ||
            //    status.STATUS == procStatus.Warning ||
            //    status.STATUS == procStatus.WarnDisp) {
            //    //※正常時

            //    var data = separateDicReturn(resultInfo[1], conductId);
            //    /* ボタン権限制御 切替 start ================================================ */
            //    //var authShori = resultInfo[2];
            //    /* ボタン権限制御 切替 end ================================================== */

            //    //定義区分
            //    var areaKbn = areaKbnDef.List;      //1:明細ｴﾘｱ
            //    //単票入力画面ﾎﾞﾀﾝｴﾘｱの配置か？
            //    if ($(P_Article).attr("name") == "edit_area") { //要修正
            //        areaKbn = areaKbnDef.Input;     //2:単票ﾎﾟｯﾌﾟｱｯﾌﾟｴﾘｱ
            //    }

            //    //再検索結果を表示
            //    if (areaKbn != areaKbnDef.Input) {
            //        //一覧(Index)画面の場合

            //        //処理ﾊﾟﾀｰﾝ
            //        var form = $(P_Article).find("#" + P_formDetailId);
            //        var conductPtn = $(form).find('input[name="CONDUCTPTN"]').val();

            //        // 検索結果をクリア
            //        clearSearchResult();
            //        //検索結果ﾃﾞｰﾀを明細一覧に表示
            //        var pageRowCount = dispListData(appPath, conductId, pgmId, formNo, data, true);

            //        //ﾍﾟｰｼﾞの状態を検索後に設定
            //        setPageStatus(pageStatus.SEARCH, conductPtn);

            //    } else {
            //        //単票表示
            //        dispDataVertical(appPath, data, formNo, true);
            //    }
            //    //ﾎﾞﾀﾝｽﾃｰﾀｽ設定
            //    /* ボタン権限制御 切替 start ================================================ */
            //    //setButtonStatus(authShori);
            //    setButtonStatus();
            //    /* ボタン権限制御 切替 end ================================================== */
            //}
            //else if (status.STATUS == procStatus.Error) {
            //    //処理結果ｽﾃｰﾀｽ(CommonProcReturn)
            //    var result = resultInfo;
            //    var status;
            //    if (result.length > 1) {
            //        status = result[0];
            //        var data = separateDicReturn(result[1], conductId);

            //        //エラー詳細を表示
            //       dispErrorDetail(data);
            //    }
            //    else {
            //        status = result;
            //    }
            //}

        },
        // 2つめは通信失敗時のコールバック
        function (resultInfo) {
            //異常時

            // ボタンを活性化
            $(btn).prop("disabled", false);

            // 取込ファイル情報をクリア
            $(P_Article).find("input:file").val("");

            //処理結果ｽﾃｰﾀｽ(CommonProcReturn)
            var result = resultInfo.responseJSON;
            var status;
            if (result.length > 1) {
                status = result[0];
                var data = separateDicReturn(result[1], conductId);

                //エラー詳細を表示
                dispErrorDetail(data);
            }
            else {
                status = result;
            }

            //処理継続用ｺｰﾙﾊﾞｯｸ関数を生成
            var confirmNo = 0;
            if (!isNaN(status.LOGNO)) {
                confirmNo = parseInt(status.LOGNO, 10);
            }
            var eventFunc = function () {
                clickComUploadBtnConfirmOK(appPath, btn, conductId, pgmId, formNo, ctrlId, form, isEdit, conductPtn, autoBackFlg)
            }

            //処理結果ｽﾃｰﾀｽを画面状態に反映
            if (!setReturnStatus(appPath, status, eventFunc)) {
                return false;
            }

            //// ファイル取り込み処理失敗
            ////異常時、ｴﾗｰﾒｯｾｰｼﾞを表示
            //setMessage(resultInfo.responseJSON.MESSAGE, resultInfo.responseJSON.STATUS);
            //addMessageLogNo(resultInfo.responseJSON.LOGNO, resultInfo.responseJSON.STATUS);

        }
    ).always(
        //通信の完了時に必ず実行される
        function (resultInfo) {
            //post用ﾃﾞｰﾀの初期化
            $(form).find("input:hidden[name='ListData']").val("");            //submit用に明細ｴﾘｱﾃﾞｰﾀを初期化
            $(form).find("input:hidden[name='FORMNO']").val("");              //画面NOをｾｯﾄ
            $(form).find("input:hidden[name='CTRLID']").val("");              //submit用にﾎﾞﾀﾝCtrlIdをｾｯﾄ
            $(form).find("input:hidden[name='ConditionData']").val("");       //submit用に検索条件を取得してｾｯﾄ
            $(form).find("input:hidden[name='ListDefines']").val("");         //submit用に一覧定義情報ｾｯﾄ
            $(form).find("input:hidden[name='ButtonDefines']").val("");       //submit用にﾎﾞﾀﾝ定義情報ｾｯﾄ
            $(form).find("input:hidden[name='ListIndividual']").val("");       //submit用に個別実装用ﾃﾞｰﾀｾｯﾄ

            //処理中メッセージ：off
            processMessage(false);
            // 実行中フラグOFF
            P_ProcExecuting = false;
            // ボタンを活性化
            $(btn).prop("disabled", false);
        });
    $('html,body').animate({ scrollTop: 0 }, '1');      //ｽｸﾛｰﾙを先頭へ移動
    if (isEdit) {
        $(P_Article).find(".edit_div").focus();
    }
    else {
        $(P_Article).find(".detail_div").focus();
    }

}

/**
 *  【共通 - ExcelPort】アップロードボタン - 確認メッセージOK時、実行処理
 *  @param {string} appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {Element} btn        ：ボタン要素
 *  @param {string} conductId   ：機能ID
 *  @param {string} pgmId       ：プログラムID
 *  @param {number} formNo      ：画面番号
 *  @param {string} ctrlId      ：ボタンCtrlId
 *  @param {string} actionKbn   ：ボタンアクション区分
 *  @param {Element} form       ：submitｴﾘｱのform要素
 */
function clickExcelPortUploadBtnConfirmOK(appPath, btn, conductId, pgmId, formNo, ctrlId, actionKbn, form, isEdit, conductPtn, autoBackFlg, confirmNo) {

    // 実行中フラグON
    P_ProcExecuting = true;

    if (confirmNo > 0) {
        //処理中メッセージ：on
        processMessage(true);
        dispLoading();
    }

    //FormData生成
    var formData = new FormData($(form).get(0));

    // クリックされたボタンを判定
    var btnClickedBtn;
    $.each(btn, function (index, element) {
        var tmpPgmId = getProgramIdByElement(element);
        if (tmpPgmId == pgmId) {
            btnClickedBtn = element;
        }
    })

    // ボタンのアクション区分の設定
    formData.append("ActionKbn", actionKbn);
    if (confirmNo > 0) {
        // 対象機能確認後の場合、対象機能IDを個別実装条件から取得
        var pgmIdw = P_dicIndividual["TargetConductId"];
        formData.set("PGMID", pgmIdw);
        formData.set("ListIndividual", JSON.stringify(P_dicIndividual));
    }

    // 【オーバーライド用関数】追加条件取得処理
    var addConditionData = addSearchConditionDictionaryForRegist(appPath, conductId, formNo, btn);
    if (addConditionData.length) {
        formData.append("AddRegistData", JSON.stringify(addConditionData));
    }

    var formNo = 0;
    var ctrlId = "";
    var input = $(form).find("input:hidden[name='FORMNO']");
    if (input != null && input.length > 0) {
        formNo = input.val();
    }
    input = $(form).find("input:hidden[name='CTRLID']");
    if (input != null && input.length > 0) {
        ctrlId = input.val();
    }

    $.ajax({
        url: appPath + 'Common/ExcelPortUpload/',   // ExcelPortアップロード
        method: 'POST',
        data: formData,
        contentType: false,
        processData: false
    }).then(
        // 1つめは通信成功時のコールバック
        function (resultInfo) {
            //正常時
            var status = resultInfo[0];                     //[0]:処理ステータス - CommonProcReturn
            var data = separateDicReturn(resultInfo[1], conductId);    //[1]:結果データ - Dictionary<string, object>※結果ﾃﾞｰﾀ："Result"、個別実装用ﾃﾞｰﾀ："Individual"
            confirmNo = 0;

            //完了ﾒｯｾｰｼﾞを表示
            setMessage(status.MESSAGE, status.STATUS);
            addMessageLogNo(status.LOGNO, status.STATUS);

            // エラー情報を表示
            dispErrorDetail(data, true);

            // 正常の場合
            if (status.STATUS == procStatus.Valid ||
                status.STATUS == procStatus.Warning ||
                status.STATUS == procStatus.WarnDisp) {

                // 結果ﾃﾞｰﾀをｾｯﾄ
                //ﾀﾌﾞ内ﾎﾞﾀﾝか判定
                var tab = $(btn).closest(".tab_contents");
                var isTab = $(tab).length;
                setExecuteResults(appPath, conductId, pgmId, formNo, ctrlId, conductPtn, isEdit, autoBackFlg, data, status, isTab);
            } else if (status.STATUS == procStatus.Error) {

                // ※エラー情報シートをダウンロードさせる場合は正常ルートで返ってくる
                // ダウンロードファイル名の取得
                var fileName = status.FILEDOWNLOADNAME;
                var filePath = status.FILEPATH;

                if (fileName != null && fileName.length > 0) {
                    // ダウンロードファイル名が指定されている場合、ダウンロード処理実行
                    var formDetail = $(P_Article).find("#" + P_formDetailId);
                    setAttrByNativeJs(formDetail, "method", "POST");
                    setAttrByNativeJs(formDetail, "action", appPath + "Common/Report?output=2&fileName=" + fileName + "&filePath=" + filePath);
                    $(formDetail).submit();
                }
            }

            //【オーバーライド用関数】実行正常終了後処理
            postRegistProcess(appPath, conductId, pgmId, formNo, btn, conductPtn, autoBackFlg, isEdit, data, status);

            //ﾌｧｲﾙ情報をｸﾘｱ ※連続処理を制御
            $(P_Article).find("input:file").val("");

        },

        // 2つめは通信失敗時のコールバック
        function (resultInfo) {
            var result = resultInfo.responseJSON;
            var status = result[0];                     //[0]:処理ステータス - CommonProcReturn
            var data = separateDicReturn(result[1], conductId);    //[1]:結果データ - Dictionary<string, object>※結果ﾃﾞｰﾀ："Result"、個別実装用ﾃﾞｰﾀ："Individual"

            confirmNo = 0;
            if (status.LOGNO && status.LOGNO.length > 0) {
                confirmNo = parseInt(status.LOGNO, 10);
            }
            if (confirmNo == 0) {
                //エラー詳細を表示
                dispErrorDetail(data);
            }

            //処理継続用ｺｰﾙﾊﾞｯｸ関数を生成
            var eventFunc = function () {
                clickExcelPortUploadBtnConfirmOK(appPath, btn, conductId, pgmId, formNo, ctrlId, actionKbn, form, isEdit, conductPtn, autoBackFlg, confirmNo)
            }

            //処理結果ｽﾃｰﾀｽを画面状態に反映
            if (!setReturnStatus(appPath, status, eventFunc)) {
                return false;
            }
        }
    ).always(
        //通信の完了時に必ず実行される
        function (resultInfo) {
            if (confirmNo == 0) {
                // 個別実装用データ初期化
                $(form).find("input:hidden[name='ListIndividual']").val("");
                P_dicIndividual["TargetConductId"] = null;
                P_dicIndividual["TargetSheetNo"] = null;
                P_dicIndividual["TargetGrpNo"] = null;  //★インメモリ化対応

                // 画面変更ﾌﾗｸﾞ初期化
                dataEditedFlg = false;

                //処理中メッセージ：off
                processMessage(false);
                // 実行中フラグOFF
                P_ProcExecuting = false;
                // ボタンを活性化
                $(btn).prop("disabled", false);
            }

        });
    $('html,body').animate({ scrollTop: 0 }, '1');      //ｽｸﾛｰﾙを先頭へ移動
    if (isEdit) {
        $(P_Article).find(".edit_div").focus();
    }
    else {
        $(P_Article).find(".detail_div").focus();
    }

}

/**
 *  ログアウトボタンの初期化
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 *  @param {string} ：機能ID
 *  @param {string} ：プログラムID
 */
function initLogoutBtn(appPath, btn) {

    // ボタンクリックイベントハンドラの設定
    $(btn).on('click', function () {

        if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

        //処理継続用ｺｰﾙﾊﾞｯｸ関数を生成
        var eventFunc = function () {
            clickLogoutBtnConfirmOK(appPath);
        }

        // ﾒﾆｭｰｱｲｺﾝの表示切替
        var icon = $("#app_navi_icon").find('span.glyphicon-cog');
        $(icon).removeClass("select");      // ｱｲｺﾝ未選択

        // 確認メッセージ表示
        //『ログアウトします。よろしいですか？』
        if (!popupMessage([P_ComMsgTranslated[941430001]], messageType.Confirm, eventFunc)) {
            return false;
        }

        //※確認メッセージ「OK」の場合、下記関数処理を実施
        //イベント関数：clickLogoutBtnConfirmOK
    });
}

/**
 *  ログアウトボタン - 確認メッセージOK時、実行処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 *  @param {string} ：機能ID
 *  @param {string} ：プログラムID
 *  @param {number} ：定義区分
 */
function clickLogoutBtnConfirmOK(appPath) {

    var form = $("#formTop");

    if (('sessionStorage' in window) && (window.sessionStorage !== null)) {
        //// メニュー切替情報を削除
        //sessionStorage.removeItem("CIM_MENU_STATE");
        //// メニュー展開情報を削除
        //sessionStorage.removeItem("MENU_SELECTED");
        // 全てクリア
        sessionStorage.clear();
    }

    // イベントを削除
    $(window).off('beforeunload');

    setAttrByNativeJs(form, "action", appPath + "Home/Logout");
    $(form).submit();

}

/**
 *  サイトTOPボタンの初期化
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 *  @param {string} ：機能ID
 *  @param {string} ：プログラムID
 */
function initSiteTopBtn(appPath, btn) {
    // ボタンクリックイベントハンドラの設定
    $(btn).on('click', function () {

        if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

        //(ﾊﾟﾀｰﾝ①)確認ﾒｯｾｰｼﾞが必要な場合
        ////処理継続用ｺｰﾙﾊﾞｯｸ関数を生成
        //var eventFunc = function () {
        //    clickSiteTopBtnConfirmOK(appPath);
        //}
        //// 確認メッセージ表示
        ////『ログアウトします。よろしいですか？』
        //if (!popupMessage([P_ComMsgTranslated[941430001]], messageType.Confirm, eventFunc)) {
        //    return false;
        //}

        ////※確認メッセージ「OK」の場合、下記関数処理を実施
        ////イベント関数：clickSiteTopBtnConfirmOK

        //(ﾊﾟﾀｰﾝ②)確認ﾒｯｾｰｼﾞなし
        clickSiteTopBtnConfirmOK(appPath);
    });
}

/**
 *  サイトTOPボタン - 確認メッセージOK時、実行処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 *  @param {string} ：機能ID
 *  @param {string} ：プログラムID
 *  @param {number} ：定義区分
 */
function clickSiteTopBtnConfirmOK(appPath) {

    var form = $("#formTop");

    setAttrByNativeJs(form, "action", appPath + "Common/RedirectSiteTop");
    $(form).submit();

}

//  [ADD_20190826_01][内部課題_No.93] start
/**
 *  ＩＤ切替ボタンの初期化
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 *  @param {string} ：機能ID
 *  @param {string} ：プログラムID
 */
function initIdChangeBtn(appPath, btn) {

    // ボタンクリックイベントハンドラの設定
    $(btn).on('click', function () {

        if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

        //処理継続用ｺｰﾙﾊﾞｯｸ関数を生成
        var eventFunc = function () {
            clickIdChangeBtnConfirmOK(appPath);
        }
        // 確認メッセージ表示
        //『ログアウトしてＩＤ切替画面に遷移します。よろしいですか？』
        if (!popupMessage([P_ComMsgTranslated[941430002]], messageType.Confirm, eventFunc)) {
            return false;
        }

        //※確認メッセージ「OK」の場合、下記関数処理を実施
        //イベント関数：clickIdChangeBtnConfirmOK
    });
}

/**
 *  ＩＤ切替ボタン - 確認メッセージOK時、実行処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 *  @param {string} ：機能ID
 *  @param {string} ：プログラムID
 *  @param {number} ：定義区分
 */
function clickIdChangeBtnConfirmOK(appPath) {

    var form = $("#formTop");

    if (('sessionStorage' in window) && (window.sessionStorage !== null)) {
        // メニュー切替情報を削除
        sessionStorage.removeItem("CIM_MENU_STATE");
        // メニュー展開情報を削除
        sessionStorage.removeItem("MENU_SELECTED");
    }

    setAttrByNativeJs(form, "action", appPath + "Home/IdChange");
    $(form).submit();

}
//  [ADD_20190826_01][内部課題_No.93] end

/**
 *  パスワード変更フォームポップアップボタンの初期化
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 *  @param {string} ：機能ID
 *  @param {string} ：プログラムID
 */
function initPassChangeFormBtn(appPath, btn) {

    // ボタンクリックイベントハンドラの設定
    $(btn).on('click', function () {

        if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

        //表示ﾒｯｾｰｼﾞをｸﾘｱ
        var messageDiv = $("#passwordChange_message_div");
        $(messageDiv).children().remove();

        // 入力値をクリア
        var element = $("#passwordChange_detail_div").find("input[type='password']");
        $(element).val("");
        setAttrByNativeJs(element, "title", "");
        $(element).removeClass("errorcom");

        //「ｷｬﾝｾﾙ」ﾎﾞﾀﾝ制御
        var btnCancel = $("#ComPassChangeCancel");
        $(btnCancel).removeClass("hide");     //ﾎﾞﾀﾝ表示

        // ﾒﾆｭｰｱｲｺﾝの表示切替
        var icon = $("#app_navi_icon").find('span.glyphicon-cog');
        $(icon).removeClass("select");      // ｱｲｺﾝ未選択

        //ﾒｯｾｰｼﾞﾎﾟｯﾌﾟｱｯﾌﾟ表示
        $('#passwordChangeModal').modal();
        var uploadModal = $("#fileUploadModal");
        if (uploadModal.length <= 0) {
            //※確認ﾒｯｾｰｼﾞを複数回表示する場合のおまじない
            $('#passwordChangeModal').off('hidden.bs.modal');
            $('#passwordChangeModal').on('hidden.bs.modal', function (e) {
                $('.modal-backdrop').remove();
            });
        }

        return false;
    });
}

/**
 *  パスワード変更実行ボタンの初期化
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 */
function initPassChangeExecBtn(appPath, btn) {

    // ボタンクリックイベントハンドラの設定
    $(btn).on('click', function () {

        if (P_ProcExecuting) { return; }    // 処理実行中の場合は抜ける

        var conductIdW = $(this).closest("article").find("#main_contents .selected input:hidden[name='CONDUCTID']:first").val();

        // エラー状態を初期化
        var element = $("#passwordChange_detail_div");
        messageDiv = $("#passwordChange_message_div");
        // メッセージをクリア
        if ($(messageDiv).children().text() == P_ComMsgTranslated[941220007]) {
            clearMessage();
        }
        //エラー情報をクリア
        clearErrorClassForPassChange2(element);

        //入力ﾁｪｯｸ
        if (!validPassChangeData()) {
            //入力ｴﾗｰ
            return false;
        }
        //処理継続用ｺｰﾙﾊﾞｯｸ関数を生成
        var eventFunc = function () {
            clickPassChangeExecBtnConfirmOK(appPath, btn, conductIdW);
        }
        // 確認メッセージ表示
        //『パスワードを変更します。よろしいですか？』
        if (!popupMessage([P_ComMsgTranslated[941260011]], messageType.Confirm, eventFunc)) {
            return false;
        }
    });
}

/**
 *  パスワード変更実行ボタン - 確認メッセージOK時、実行処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 *  @param {string} ：機能ID
 *  @param {string} ：プログラムID
 *  @param {number} ：定義区分(1:一覧画面 2:明細入力画面)
 */
function clickPassChangeExecBtnConfirmOK(appPath, btn, conductId, confirmNo) {

    // 実行中フラグON
    P_ProcExecuting = true;
    // ボタンを不活性化
    $(btn).prop("disabled", true);

    //エラー状態を初期化
    var element = $("#passwordChange_detail_div");
    clearErrorStatus(element);

    //処理中メッセージ：on
    processMessage(true);

    var passChangeForm = $("#passwordChange_detail_div");
    var loginUser = 'loginUser';
    var loginPassword = $(passChangeForm).find("input:password[name='LoginPassword_text']").val();
    var NewLoginPassword = $(passChangeForm).find("input:password[name='NewLoginPassword_text']").val();
    var NewLoginPasswordConfirm = $(passChangeForm).find("input:password[name='NewLoginPasswordConfirm_text']").val();

    var condition = {
        userId: loginUser,                          // ユーザーID
        userOldPassword: loginPassword,             // 現在のパスワード
        userNewPassword1: NewLoginPassword,         // 新しいパスワード
        userNewPassword2: NewLoginPasswordConfirm,  // 新しいパスワード（確認）
    };
    var conditionList = []
    conditionList.push(condition);

    // POSTデータを生成
    var postdata = {
        conductId: conductId,           // メニューの機能ID
        conditionData: conditionList,
    };

    // 登録処理実行
    $.ajax({
        url: appPath + 'api/CommonProcApi/' + actionkbn.ComPassChangeExec,    // 実行
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(postdata),
        headers: { 'X-XSRF-TOKEN': getRequestVerificationToken() },
        traditional: true,
        cache: false
    }).then(
        // 1つめは通信成功時のコールバック
        function (resultInfo) {
            //正常時
            //[0]:処理ステータス - CommonProcReturn
            //[1]:結果データ - IList<Dictionary<string, object>>

            var status = resultInfo;

            //完了ﾒｯｾｰｼﾞを表示
            setMessage(status.MESSAGE, status.STATUS);
            addMessageLogNo(status.LOGNO, status.STATUS);

        },
        // 2つめは通信失敗時のコールバック
        function (resultInfo) {
            //異常時

            //処理結果ｽﾃｰﾀｽ(CommonProcReturn)
            var status = resultInfo.responseJSON;
            //処理継続用ｺｰﾙﾊﾞｯｸ関数を生成
            var confirmNo = 0;
            if (!isNaN(status.LOGNO)) {
                confirmNo = parseInt(status.LOGNO, 10);
            }
            var eventFunc = function () {
                clickPassChangeExecBtnConfirmOK(appPath, btn, conductId, confirmNo);
            }

            //処理結果ｽﾃｰﾀｽを画面状態に反映
            if (!setReturnStatus(appPath, status, eventFunc)) {
                return false;
            }

        }
    ).always(
        //通信の完了時に必ず実行される
        function (resultInfo) {
            //処理中メッセージ：off
            processMessage(false);
            // 実行中フラグOFF
            P_ProcExecuting = false;
            // ボタンを活性化
            $(btn).prop("disabled", false);
        });
}

/**
 *  パスワード変更の入力検証を行う
 *
 *  @return {bool} : ture(OK) false(NG)
 */
function validPassChangeData() {

    // 共通入力チェック - Validatorの実行
    var formPassChange = $("#passwordChange_detail_div");
    var valid = $(formPassChange).valid();

    if (!valid) {
        //個別エラー状態を初期化
        // メッセージをクリア
        clearMessage();
        ////指定エリアのエラー状態を初期化※多分不要
        //clearErrorClassForPassChange3(formPassChange);
        // エラーをツールチップに表示
        setErrorTooltipForPassChange(formPassChange);
        //【CJ00000.W01】入力エラーがあります。
        addMessage(P_ComMsgTranslated[941220007], 1)
        return false;
    }
    return true;    //入力ﾁｪｯｸOK
}

/**
 * 詳細検索条件エリアの初期化処理
 */
function initDetailSearchCondition(target) {
    if (!target) {
        target = '';
    } else {
        taret = ' ' + target;
    }
    var tables = $('nav#detail_search table.detailsearch' + target);
    if (tables == null || tables.length == 0) { return; }

    $.each($(tables), function (i, tbl) {
        const conductId = $(tbl).data('conductid');
        const ctrlId = $(tbl).data('parentid');
        const formNo = $(tbl).data('formno');

        // ローカルストレージから保存データを取得
        const savedData = getSavedDataFromLocalStorage(localStorageCode.DetailSearch, conductId, formNo, ctrlId);
        if (savedData == null || savedData.length == 0) { return true; }

        // 初期値を設定
        var trs = $(tbl).find('tr');
        $.each($(trs), function (i, tr) {
            const itemNo = $(tr).data('itemno');
            const valNo = 'VAL' + itemNo;
            // チェック状態を設定
            const checked = savedData[0][valNo + '_checked'];

            // 選択チェックボックスの初期化
            var checkBoxSelect = $(tr).find('td.select input[type="checkbox"]');
            $(checkBoxSelect).prop('checked', checked);
            // 選択チェックイベントハンドラの設定
            $(checkBoxSelect).off('click');
            $(checkBoxSelect).on('click', function () {
                const table = $(this).closest('table');
                var checkedCheckBox = $(table).find('td.select input[type="checkbox"]:checked');
                if (!checkedCheckBox || checkedCheckBox.length == 0) {
                    // 選択チェック無しの場合、対象一覧の読込件数コンボとボタンを活性化
                    const ctrlId = $(table).data('parentid');
                    setSelectCntControlStatus(ctrlId, false);
                }
            });

            // NULL検索チェックボックスのチェック状態を設定
            var nullSearch = savedData[0][valNo + '_nullSearch'];
            if (nullSearch) {
                var nullSearchTr = $(tr).next('.null_search_tr[data-itemno="' + itemNo + '"]');
                if (nullSearchTr && nullSearchTr.length > 0) {
                    $(nullSearchTr).find('input[type="checkbox"]').prop('checked', nullSearch);
                }
            }

            // 保存値を条件コントロールへ設定
            const val = savedData[0][valNo];
            if (!val) { return true; }

            const td = $(tr).find('td.input_td');
            // 複数選択チェックボックス
            var msul = $(td).find("ul.multiSelect");
            if (msul.length > 0) {
                setAttrByNativeJs(msul, 'data-value', val);
                return true;
            }

            // チェックボックス
            var checkbox = $(td).find(":checkbox");
            if (checkbox.length > 0) {
                $(checkbox).prop("checked", val == 1);
                return true;
            }

            //ラジオボタン
            var radio = $(td).find(":radio");
            if (radio.length > 0) {
                $(radio).find('[value="' + val + '"]').prop('checked', true);
                return true;
            }

            //ﾃｷｽﾄ、数値、ｺｰﾄﾞ＋翻訳
            var input = $(td).find('input[type="text"], input[type="hidden"]');
            if (input.length > 0) {
                if (input.length == 1) {
                    $(input).val(val);
                } else {
                    var vals = val.split('|');
                    $(input[0]).val(vals[0]);
                    if (vals.length > 1) {
                        $(input[1]).val(vals[1]);
                    }
                }
                return true;
            }

            //日付(ブラウザ標準)、時刻、日時(ブラウザ標準)
            var dateTime = $(td).find("input[type='date'], input[type='time'], input[type='datetime-local']");
            if (dateTime.length > 0) {
                if (dateTime.length == 1) {
                    setAttrByNativeJs(dateTime, 'data-def', val);
                } else {
                    var vals = val.split('|');
                    setAttrByNativeJs(dateTime[0], 'data-def', vals[0]);
                    if (vals.length > 1) {
                        setAttrByNativeJs(dateTime[1], 'data-def', vals[1]);
                    }
                }
                return true;
            }
        });
    });
}

/**
 *  詳細検索メニュー表示ボタンの初期化処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 */
function initDetailSearchBtn(appPath, btn) {
    // ボタンクリックイベントハンドラの設定
    $(btn).on('click', function () {
        var formNo = $(P_Article).data("formno");
        var ctrlId = $(this).data('parentid');

        // 詳細検索メニュー表示切替
        var menuDiv = $('#detail_search');
        var condTable = $(menuDiv).find("table.detailsearch[data-parentid='" + ctrlId + "'][data-formno='" + formNo + "']");
        var otherFormNoTable = $(menuDiv).find("table.detailsearch[data-parentid='" + ctrlId + "']:not([data-formno='" + formNo + "'])");
        var otherCtrlIdTable = $(menuDiv).find("table.detailsearch:not([data-parentid='" + ctrlId + "'])");
        var hide = !isHide(menuDiv);
        if (condTable != null && condTable.length > 0) {
            setHide(menuDiv, hide);
            setHide(condTable, hide);
            if (otherFormNoTable != null && otherFormNoTable.length > 0) {
                setHide(otherFormNoTable, true);
            }
            if (otherCtrlIdTable != null && otherCtrlIdTable.length > 0) {
                setHide(otherCtrlIdTable, true);
            }
        } else {
            setHide(menuDiv, true);
        }

        // 横方向一覧の列固定位置を再設定
        setFixColCss();

    });
}

/**
 *  詳細検索実行ボタンの初期化処理
 *  @param appPath {string}     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param btn {Element}        ：対象ボタン
 *  @param conductId {string}   ：機能ID
 */
function initDetailSearchExecBtn(appPath, btn, conductId) {
    // ボタンクリックイベントハンドラの設定
    $(btn).on('click', function () {
        // 表示中の詳細検索条件テーブルを取得
        var table = $("#detail_search table:not(.hide)");
        if (table == null && table.length == 0) { return; }

        //console.time('javascript');
        P_TabulatorEventWaitStatus = tabulatorEventWaitStatusDef.WaitTableBuilting;
        P_ExecRenComFlag = false;

        var conductId = $(table).data("conductid");
        var formNo = $(table).data("formno");
        var ctrlId = $(table).data('parentid');
        var form = $(P_Article).find("form[id^='form']");
        var pgmId = $(form).find("input:hidden[name='PGMID']").val();
        var conductPtn = $(form).find("input:hidden[name='CONDUCTPTN']").val();

        var targetArea = $("nav#detail_search");
        var formDetailSearch = $(targetArea).find("form")
            .has("table[data-conductid='" + conductId + "'][data-parentid='" + ctrlId + "'][data-formno='" + formNo + "']");

        //エラー状態を初期化
        clearErrorcomStatus(formDetailSearch, targetArea);

        // チェック行を取得
        // ※前者の指定方法でも取得可だが後者の方がパフォーマンス良いらしい
        //var chkbox = $(table).find("td.select input:checkbox:checked");
        var chkbox = $(table).find("td.select input[type='checkbox']").filter(":checked");
        if (chkbox != null && chkbox.length > 0) {
            // 入力チェック実行
            if (!validFormDetailSearchData(formDetailSearch)) {
                return;
            }
        }

        // 「保存して次回検索時に使用する」のチェックが入っている場合、ローカルストレージへ保存する
        chkbox = $(targetArea).find("input[type='checkbox']#ComDetailSearchSave").filter(":checked");
        var saveCondition = chkbox != null && chkbox.length > 0;

        //【オーバーライド用関数】詳細検索前の個別実装
        beforeDetailSearchBtnProcess(appPath, btn, conductId, pgmId, formNo, conductPtn);
        // 検索処理実行
        executeSearchBtnProcess(appPath, btn, conductId, pgmId, formNo, conductPtn, saveCondition, true);
    });
}

/**
 *  詳細検索条件追加ボタンの初期化処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 */
function initDetailSearchAddCondBtn(appPath, btn) {
    // ボタンクリックイベントハンドラの設定
    $(btn).on('click', function () {
        // クリック行を取得
        var tr = $(this).closest("tr");

        // 挿入行を生成
        var newTr = $(tr).clone(false);
        $(newTr).removeClass('default_tr');
        var input = $(newTr).find('.input_td').children();
        $(input).val('');
        $(newTr).find('td').not('.input_td').children().remove();
        $(newTr).find('td').not('.input_td').text('');

        // 挿入先を取得
        var nextTr = $(tr).nextAll('.default_tr').first();
        if (nextTr != null && nextTr.length > 0) {
            $(newTr).insertBefore(nextTr);
        } else {
            var nextTr = $(tr).nextAll().last();
            if (nextTr != null && nextTr.length > 0) {
                $(newTr).insertAfter(nextTr);
            } else {
                $(newTr).insertAfter(tr);
            }
        }
    });
}

/**
 *  項目カスタマイズメニュー表示ボタンの初期化処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 */
function initItemCustomizeBtn(appPath, btn) {
    // ボタンクリックイベントハンドラの設定
    $(btn).on('click', function () {
        var formNo = $(P_Article).data("formno");
        var ctrlId = $(this).data('parentid');

        // 項目カスタマイズメニュー表示切替
        var menuDiv = $('#item_customize');
        var condTable = $(menuDiv).find("ul.itemcustomize[data-parentid='" + ctrlId + "'][data-formno='" + formNo + "']");
        var otherFormNoTable = $(menuDiv).find("ul.itemcustomize[data-parentid='" + ctrlId + "']:not([data-formno='" + formNo + "'])");
        var otherCtrlIdTable = $(menuDiv).find("ul.itemcustomize:not([data-parentid='" + ctrlId + "'])");
        var hide = !isHide(menuDiv);
        if (condTable != null && condTable.length > 0) {
            setHide(menuDiv, hide);
            setHide(condTable, hide);
            if (otherFormNoTable != null && otherFormNoTable.length > 0) {
                setHide(otherFormNoTable, true);
            }
            if (otherCtrlIdTable != null && otherCtrlIdTable.length > 0) {
                setHide(otherCtrlIdTable, true);
            }
        } else {
            setHide(menuDiv, true);
        }
    });
}

/**
 *  項目カスタマイズデータ保存ボタンの初期化処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 */
function initItemCustomizeSaveBtn(appPath, btn) {
    // ボタンクリックイベントハンドラの設定
    $(btn).on('click', function () {
        // 表示中のカスタマイズリストを取得
        var ul = $("#item_customize ul.itemcustomize:not(.hide, .fixed)");
        if (ul == null && ul.length == 0) { return; }

        var conductId = $(ul).data("conductid");
        var formNo = $(ul).data("formno");
        var ctrlId = $(ul).data('parentid');

        // 表示順を取得
        var data = [];
        $.each($(ul).find('li'), function (i, li) {
            var id = $(li).attr('id');
            var itemName = id.replace(ctrlId + "_" + formNo + "_", '');
            var disp = $(li).find('input:checkbox').prop('checked') ? "1" : "0"; // 1:表示/0:非表示
            data.push([itemName, disp]);
        });

        // LocalStorageへ保存
        setSaveDataToLocalStorage(data, localStorageCode.ItemCustomize, conductId, formNo, ctrlId);
    });
}

/**
 *  項目カスタマイズデータ適用ボタンの初期化処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 */
function initItemCustomizeApplyBtn(appPath, btn) {
    // ボタンクリックイベントハンドラの設定
    $(btn).on('click', function () {
        // 項目カスタマイズ情報を保存
        saveItemCustomizeInfo(appPath, true);
    });
}

/**
 *  項目カスタマイズデータ閉じるボタンの初期化処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 */
function initItemCustomizeHideBtn(appPath, btn) {
    // ボタンクリックイベントハンドラの設定
    $(btn).on('click', function () {
        // 項目カスタマイズ情報を保存
        saveItemCustomizeInfo(appPath);

    });
}

function saveItemCustomizeInfo(appPath, isApply) {
    // チェックボックスは配置せず、常に本登録
    //// 「保存して次回検索時に使用する」のチェックが入っている場合、データ区分=1(登録)で登録
    //var targetArea = $("nav#item_customize");
    //var chkbox = $(targetArea).find("input[type='checkbox']#ComItemCustomizeSave").filter(":checked");
    //const dataDiv = chkbox != null && chkbox.length > 0 ? 1 : 0;
    //// 閉じるボタン＆保存チェックなしの場合は保存しない
    //if (!isApply && dataDiv == 0) { return; }
    const dataDiv = 1;

    // 表示中のカスタマイズリストを取得
    var ul = $("#item_customize ul.itemcustomize:not(.hide)");
    if (ul == null && ul.length == 0) { return; }

    const conductId = $(ul).data("conductid");
    const formNo = $(ul).data("formno");
    const ctrlId = $(ul).data('parentid');
    //const factoryIdList = getSelectedFactoryIdList();
    var form = $(P_Article).find("form[id^='formDetail']");
    const pgmId = $(form).find('input[name="PGMID"]').val();

    // 表示順を取得
    var data = [];
    $.each($(ul).find('li'), function (i, li) {
        var id = $(li).attr('id');
        var itemName = id.replace(ctrlId + "_" + formNo + "_", '');
        var disp = $(li).find('input:checkbox').prop('checked') ? "1" : "0"; // 1:表示/0:非表示
        data.push([itemName, disp]);
    });

    var conditionDataList = [];         //条件ﾃﾞｰﾀ
    var conditionData = {
        PGMID: pgmId,               // プログラムID
        FORMNO: formNo,             // 画面番号
        CTRLID: ctrlId,             // 一覧のctrlId
        DATADIV: dataDiv,           // データ区分
        customizeList: data,        // 項目カスタマイズ情報
    };
    conditionDataList.push(conditionData);

    if (isApply) {
        // 適用ボタン押下時、submitして画面再描画
        form = $('#formItemCustomize');

        //submitデータを設定
        $(form).find("input:hidden[name='CONDUCTID']").val(conductId);
        $(form).find("input:hidden[name='PGMID']").val(pgmId);
        $(form).find("input:hidden[name='FORMNO']").val(formNo);
        $(form).find("input:hidden[name='CTRLID']").val(ctrlId);
        $(form).find("input:hidden[name='ConditionData']").val(JSON.stringify(conditionDataList));
        //$(form).find("input:hidden[name='FactoryIdList']").val(JSON.stringify(factoryIdList));
        $(form).submit();

    } else {
        // 閉じるボタン押下時、登録処理のみ

        // POSTデータを生成
        var postdata = {
            conductId: conductId,                   // メニューの機能ID
            pgmId: pgmId,                           // メニューのプログラムID
            formNo: formNo,                         // 画面番号
            ctrlId: ctrlId,                         // 一覧のctrlId
            conditionData: conditionDataList,       // 検索条件入力データ
            browserTabNo: P_BrowserTabNo,           // ブラウザタブ識別番号

            //    factoryIdList: factoryIdList,           // 工場IDリスト
        };

        // 登録処理実行
        $.ajax({
            url: appPath + 'api/CommonProcApi/' + actionkbn.ComItemCustomizeApply,  // 項目カスタマイズ情報の登録
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json',
            data: JSON.stringify(postdata),
            headers: { 'X-XSRF-TOKEN': getRequestVerificationToken() },
            traditional: true,
            cache: false
        }).then(
            // 1つめは通信成功時のコールバック
            function (data) {

            },
            // 2つめは通信失敗時のコールバック
            function (resultInfo) {
                //異常時
                console.log(resultInfo.statusText);
            }
        ).always(
            //通信の完了時に必ず実行される
            function (resultInfo) {
                //処理中メッセージ：off
                processMessage(false);
            });
    }

}

function saveItemCustomizeInfoList(conductId, formNo, ctrlId, dataDiv, data) {

}

/**
 *  項目カスタマイズ一覧の初期化処理
 *  @param {element} ：対象一覧
 */
function initItemCustomize(ul) {
    // 項目カスタマイズ一覧を並べ替え可能に
    $(ul).sortable({
        cancel: "li.fixed",
        cursor: "move",
        //    start: function (event, ui) {
        //        moveItemId = $(ui.item).attr('id');
        //    },
        //    update: function (event, ui) {
        //        // "toArray"で移動後の一覧の並び順で行に付与したidがカンマで連結されたものが取得できる
        //        // ⇒カンマで分割し配列に変換
        //        var movedArray = String($(this).sortable("toArray")).split(',');
        //        var movedIdx = -1;
        //        // 移動後の対象項目の配列内のインデックスを取得
        //        $.each($(movedArray), function (i, id) {
        //            if (id == moveItemId) {
        //                movedIdx = i;
        //                return true;
        //            }
        //        });

        //        // 基本は移動先の「1つ後の項目の前に移動」
        //        var after = false;
        //        var nextIdx = movedIdx + 1;
        //        if (movedIdx == movedArray.length - 1) {
        //            // 末尾に移動した場合は「1つ前の項目の後ろに移動」
        //            after = true;
        //            nextIdx = movedIdx - 1;
        //        }

        //        // 移動対象のtabulatorのtableを取得
        //        var tableId = getItemCustomizeTableId(this)
        //        var table = P_listData['#' + tableId];
        //        var prefix = tableId + '_';

        //        var targetId = moveItemId.replace(prefix, '');
        //        var nextId = movedArray[nextIdx].replace(prefix, '');
        //        // tabulatorのtableの列の移動を実行
        //        table.moveColumn(targetId, nextId, after);
        //    }
    });

    //    // 項目カスタマイズのチェックボックスの初期化
    //    initItemCustomizeCheckBox(ul);
}

/**
 *  項目カスタマイズ一覧のチェックボックスの初期化処理
 *  @param {element} ：対象一覧
 */
function initItemCustomizeCheckBox(ul) {
    var checkBox = $(ul).find('input:checkbox');
    // デフォルトは全てチェック有り
    $(checkBox).prop('checked', true);

    $(checkBox).on('change', function () {
        var li = $(this).closest('li');
        var parentUl = $(li).closest('ul');
        var tableId = getItemCustomizeTableId(parentUl)
        var table = P_listData['#' + tableId];
        var prefix = tableId + '_';
        var id = $(li).attr('id');
        var targetId = id.replace(prefix, '');
        var isChecked = $(this).prop('checked');
        // tabulatorのtableの列の表示切替
        setTabulatorColVisible(li, table, targetId, isChecked);
        table = null;

        // 【オーバーライド用関数】項目カスタマイズ一覧のチェックボックスのチェック変更後処理
        afterChangeItemCustomizeCheckBox(tableId, targetId, isChecked);
    });
    checkBox = null;
}

/**
 *  項目カスタマイズ一覧の初期化処理
 *  @param {element} ：対象一覧
 */
function initItemCustomizeByLocalStorage(element) {
    $.each($(element), function (idx, ul) {
        var conductId = $(ul).data('conductid');
        var formNo = $(P_Article).data("formno");
        var ctrlId = $(ul).data('parentid');
        var baseId = ctrlId + '_' + formNo;

        // LocalStorageから保存データを取得
        var savedData = getSavedDataFromLocalStorage(localStorageCode.ItemCustomize, conductId, formNo, ctrlId);
        if (savedData == null || savedData.length == 0) { return; }

        // デフォルトの並び順を取得
        var defaultArray = String($(ul).sortable('toArray')).split(',');
        var prevId = "";
        var sortStarted = false;
        $.each($(savedData), function (i, item) {
            var key = item[0];
            var checked = item[1] == 1;
            var id = baseId + "_" + key;
            var li = $(ul).find('li' + '#' + id);
            if (li == null || li.length == 0) { return true; }

            // チェックボックスのチェック状態を反映
            $(li).find('input:checkbox').prop('checked', checked);
            if (checked) {
                $(li).css('color', 'black');
            } else {
                $(li).css('color', 'gray');
            }

            var defId = defaultArray[i];
            if (!sortStarted && id == defId) {
                // デフォルトの並び順と同じ
                prevId = id;
                return true;    // =continue;
            }

            // 並べ替え開始
            sortStarted = true;
            if (prevId == "") {
                // 先頭に移動
                $(li).prependTo($(ul));
            } else {
                // 1つ前のliの後ろに移動
                var prevli = $(ul).find('li' + '#' + prevId);
                $(li).insertAfter(prevli);
            }
            prevId = id;
        });
    });
}

/**
 *  項目カスタマイズ情報の設定
 *  @param {element} ：対象一覧
 */
function setItemCustomizeInfoToTabulator(table, conductId, formNo, ctrlId) {

    // LocalStorageから保存データを取得
    var savedData = getSavedDataFromLocalStorage(localStorageCode.ItemCustomize, conductId, formNo, ctrlId);
    if (savedData == null || savedData.length == 0) { return; }

    // デフォルトの並び順を取得
    var cols = table.getColumns();
    var defaultArray = $.grep(cols,
        function (col, idx) {
            var elm = col.getElement();
            // 非表示列、固定列「VAL～」以外の列を除く
            return (
                elm.style.display != 'none' &&
                elm.className.indexOf('tabulator-frozen') < 0 &&
                col.getField().startsWith('VAL'));
        }
    );

    var prevId = "";
    var sortStarted = false;
    $.each($(savedData), function (i, item) {
        if (defaultArray.length < i) { return false; }  // =break;
        var id = item[0];
        var checked = item[1] == 1;

        // tabulatorのtableの列の表示切替
        setTabulatorColVisible(null, table, id, checked);

        var defId = defaultArray[i].getField();
        if (!sortStarted && id == defId) {
            // デフォルトの並び順と同じ
            prevId = id;
            return true;    // =continue;
        }

        // 並べ替え開始
        sortStarted = true;
        if (prevId == "") {
            // 先頭に移動
            table.moveColumn(id, defaultArray[0].getField(), false);
        } else {
            // 1つ前の列の後ろに移動
            table.moveColumn(id, prevId, true);
        }
        prevId = id;
    });
}

/**
 * Tabulatorのtableの列の表示切替
 * @param {Element} li      ：li要素
 * @param {Element} table   ：tablulatorのtable要素
 * @param {string} targetId ：対象のID(項目名:VAL～)
 * @param {boolean} visible ：表示状態(true:表示/false:非表示)
 */
function setTabulatorColVisible(li, table, targetId, visible) {
    if (visible) {
        table.showColumn(targetId);
        if (li) { $(li).css('color', 'black'); }
    } else {
        table.hideColumn(targetId);
        if (li) { $(li).css('color', 'gray') };
    }
}

/**
 * 項目カスタマイズのテーブル要素のIDを取得
 * @param {Element} element ：項目要素
 */
function getItemCustomizeTableId(element) {
    var formNo = $(P_Article).data("formno");
    var ctrlId = $(element).data('parentid');
    return ctrlId + '_' + formNo;
}

/**
 *  場所階層、職種機種検索ボタンの初期化処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {button} ：対象ボタン
 */
function initTreeViewSearchBtn(appPath, btn) {
    // ボタンクリックイベントハンドラの設定
    $(btn).on('click', function () {
        //console.time('javascript');
        P_TabulatorEventWaitStatus = tabulatorEventWaitStatusDef.WaitTableBuilting;
        P_ExecRenComFlag = false;

        // 検索対象機能情報取得
        var form = $(P_Article).find("form[id^='form']");
        var conductId = $(form).find("input:hidden[name='CONDUCTID']").val();
        var pgmId = $(form).find("input:hidden[name='PGMID']").val();
        var conductPtn = $(form).find("input:hidden[name='CONDUCTPTN']").val();
        var formNo = $(P_Article).data("formno");

        //【オーバーライド用関数】場所階層、職種機種検索前の個別実装
        beforeTreeViewSearchBtnProcess(appPath, btn, conductId, pgmId, formNo, conductPtn);
        // 検索処理実行
        executeSearchBtnProcess(appPath, btn, conductId, pgmId, formNo, conductPtn, false, true);
    });
}

/**
 * モーダル画面のツリーへの表示初期値を管理するクラスを取得する処理
 * {number} StructureId          ：構成ID初期値
 * {number} MinLayerNo           ：階層番号最小値(最上位階層番号)
 * {number} MaxLayerNo           ：階層番号最大値(最下位階層番号)
 * */
function InitTreeViewModalParamClass() {
    var initTreeClass = function () { };
    initTreeClass.prototype = {
        StructureId: null, MinLayerNo: 999, MaxLayerNo: -1,
        New: function (structureId, minLayerNo, maxLayerNo) {
            this.StructureId = structureId;
            this.MinLayerNo = minLayerNo;
            this.MaxLayerNo = maxLayerNo;
        }
    };
    var result = new initTreeClass();
    return result;
}

/**
 *  階層選択モーダル画面表示ボタンの初期化処理
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {Element}：対象ボタン
 */
function initTreeViewShowBtn(appPath, btn) {
    // ボタンクリックイベントハンドラの設定
    $(btn).on('click', function () {
        let modal = $('#popupTreeViewModal');
        const formNo = $(P_Article).data('formno');
        const targetDiv = $(this).closest('div').siblings('div.ctrlId');
        const ctrlId = $(targetDiv).data('ctrlid')

        // 構成グループID、構成IDの取得
        const structureGrpId = parseInt($(this).data('structuregrpid'), 10);
        var selectedMaxStructureNo = -1; // 処理を行う階層番号
        var treeValues = []; // 値リスト
        let rowNo = 0; // 複数ある場合があるので、行番号
        let maxLayerNo = -1;
        while (true) {
            rowNo++;
            var treeLabels = $(targetDiv).find('table[data-rowno=' + rowNo + ']').find('td[data-type="treeLabel"]');
            if (treeLabels == null || treeLabels.length == 0) {
                break;
            }
            var treeValue = InitTreeViewModalParamClass();
            selectedMaxStructureNo = -1;
            $.each(treeLabels, function (idx, td) {
                // 階層番号の範囲と選択済みの構成IDを取得
                var structureId = parseInt($(td).data('structureid'), 10);
                var structureNo = parseInt($(td).data('structureno'), 10);
                if (treeValue.MinLayerNo > structureNo) {
                    treeValue.MinLayerNo = structureNo;
                }
                if (treeValue.MaxLayerNo < structureNo) {
                    treeValue.MaxLayerNo = structureNo;
                }
                if (structureId) {
                    if (selectedMaxStructureNo < structureNo) {
                        selectedMaxStructureNo = structureNo;
                        treeValue.StructureId = structureId;
                    }
                }
            });

            if (maxLayerNo < treeValue.MaxLayerNo) {
                maxLayerNo = treeValue.MaxLayerNo;
            }

            // 取得した要素を追加
            treeValues.push(treeValue);
        }

        //// - ﾀｲﾄﾙ設定
        //var formTitle = $(this).closest('.tbl_title').find('span.title').text();
        //$(modal).find(".modal-header-my .title_div span").text(formTitle);

        // 戻るボタンの初期化
        var backBtn = $(modal).find('input:button[data-actionkbn="' + actionkbn.Back + '"]')
        initBackBtnForTreeView(backBtn);

        // 選択ボタンの初期化
        var selectBtn = $(modal).find('input:button[data-actionkbn="' + actionkbn.ComTreeViewSelect + '"]');
        initSelectBtnForTreeView(appPath, selectBtn, ctrlId, structureGrpId, maxLayerNo);

        // 複数選択可否の取得
        var isMultiSelect = $(this).data("multiselect") == "1";

        // 階層選択モーダル画面の初期化
        initTreeViewModalForm(appPath, modal, structureGrpId, treeValues, isMultiSelect);

        $(modal).find('.modal-content').width('');
        $(modal).off('shown.bs.modal');
        $(modal).on('shown.bs.modal', function (e) {
            // モーダル画面の表示イベント後の初期化
            initShownModalForm(e.currentTarget, true);
        });

        $(modal).find('.modal-content').animate({ scrollTop: 0 }, '1');      //ｽｸﾛｰﾙを先頭へ移動
        //    //ﾓｰﾀﾞﾙ閉じた後ｲﾍﾞﾝﾄに戻る処理を設定
        //    $(modal).off('hidden.bs.modal');    //※複数回表示する場合のおまじない
        //    $(modal).on('hidden.bs.modal', function (e) {
        //        // モーダル画面の非表示イベント後の初期化
        //        initHiddenModalForm(appPath, e.currentTarget, transPtnDef.Child);
        //    });
        modal = null;
    });
}

/**
 * 階層選択モーダル画面の戻るボタン初期化処理
 * @param {HTMLElement} btn ：戻るボタン要素
 */
function initBackBtnForTreeView(btn) {
    //既存のｸﾘｯｸｲﾍﾞﾝﾄを削除
    $(btn).off('click');

    // ボタンクリックイベントハンドラの設定
    $(btn).on('click', function () {
        const modal = $('#popupTreeViewModal');
        $(modal).modal('hide');
        // ツリービュー要素のクリア
        $(modal).find('#form_tree_view').children().remove();
    });
}

/**
 * ツリーの選択値の表示欄を指定数まで増やす処理
 * @param {any} targetDiv 選択値の表示欄
 * @param {any} size 設定する表示欄の数
 */
function addTreeLabels(targetDiv, size) {
    // 複数行の場合、表示行数分列を増やす
    $(targetDiv).find("div.vertical_list:gt(0)").remove();
    // コピー元の行
    var orgRow = $(targetDiv).find("div.vertical_list");
    for (var i = 1; i < size; i++) {
        var copy = $(orgRow).clone(true); // コピー元の行からコピーする行を複製
        var change = $(copy).find('table.vertical_tbl'); // 属性を変更する項目を取得
        $(change).addClass("addTreeLabel"); // 追加行識別用クラス付与
        setAttrByNativeJs(change, 'data-rowno', i + 1); // 行番号を変更
        $.each(change, function (index, col) {
            var orgId = col.id;
            setAttrByNativeJs(col, 'id', orgId + '_' + i); // IDも変更
        })
        $(copy).appendTo($(targetDiv)); // 画面に追加
        change = null;
        copy = null;
    }
    orgRow = null;
}

/**
 * 階層選択モーダル画面の選択ボタン初期化処理
 * @param {string} appPath  ：アプリケーションパス
 * @param {HTMLElement} btn ：選択ボタン要素
 * @param {string} ctrlId           ：コントロールID
 * @param {number} structureGrpId   ：構成グループID
 * @param {number} maxStructureNo   ：最下層階層番号
 */
function initSelectBtnForTreeView(appPath, btn, ctrlId, structureGrpId, maxStructureNo) {
    //既存のｸﾘｯｸｲﾍﾞﾝﾄを削除
    $(btn).off('click');

    // ボタンクリックイベントハンドラの設定
    $(btn).on('click', function () {
        const modal = $('#popupTreeViewModal');
        const targetDiv = $(P_Article).find('div.ctrlId[data-ctrlid="' + ctrlId + '"]');
        const id = getTreeViewId(structureGrpId, treeViewDef.ModalForm.Val);
        const tree = $(modal).find('.form_tree_div').find('#' + id);
        // 複数選択かどうかを判定
        const isMultiSelect = $(targetDiv).parent().find("a.tree_select").data("multiselect") == "1";

        // 選択値の取得
        var nodes = $(tree).jstree(true).get_selected(true);
        if (nodes == null || nodes.length == 0) {
            // 2024/07/08 階層選択モーダル画面で未選択状態を許容する UPD start
            //return;
            // 未選択の場合、全階層をクリアする
            var targetRow = $(targetDiv).find('div.vertical_list');
            for (var j = 0; j <= maxStructureNo; j++) {
                setStructureInfoToTreeLabelDiv(targetRow, j, '', '');
            }
            targetRow = null;
            // 2024/07/08 階層選択モーダル画面で未選択状態を許容する UPD end
        }
        // 親要素が選択されている場合、自身を選択リストから削除(複数選択可能な場合)
        selectedNodes = [];
        var ids = nodes.filter(x => x["parent"] != "#").map(x => x["id"]);
        // 2024/07/08 機器別管理基準標準用場所階層ツリー追加 UPD start
        //var locationGrpIds = [structureGroupDef.Location, structureGroupDef.LocationForUserMst, structureGroupDef.LocationHistory, structureGroupDef.LocationNoHistory];
        var locationGrpIds = [structureGroupDef.Location, structureGroupDef.LocationForUserMst, structureGroupDef.LocationHistory, structureGroupDef.LocationNoHistory, structureGroupDef.LocationForMngStd];
        // 2024/07/08 機器別管理基準標準用場所階層ツリー追加 UPD end
        $.each(nodes, function (idx, node) {
            var parent = node.parent;
            //if (!ids.includes(parent) && parent != '#') {
            //    // 親要素が選択されていない場合、リストに追加
            //    selectedNodes.push(node);
            //}
            if (isMultiSelect && locationGrpIds.includes(structureGrpId)) {
                // 複数選択の場所階層の場合
                var layerNo = getTreeViewLayerNo(node);
                if (layerNo == structureLayerNo.Factory || (!ids.includes(parent) && parent != '#' && layerNo > structureLayerNo.Factory)) {
                    // 工場の場合、または親要素が選択されていない場合、リストに追加
                    selectedNodes.push(node);
                }
            } else {
                if (!ids.includes(parent) && parent != '#') {
                    // 親要素が選択されていない場合、リストに追加
                    selectedNodes.push(node);
                }
            }
        });

        // 値の個数(単一選択の場合は1)
        // 2024/07/08 階層選択モーダル画面で未選択状態を許容する UPD start
        //const valueCounts = isMultiSelect ? selectedNodes.length : 1;
        const valueCounts = selectedNodes.length > 0 ? (isMultiSelect ? selectedNodes.length : 1) : 0;
        // 2024/07/08 階層選択モーダル画面で未選択状態を許容する UPD end
        // 複数行の場合、表示行数分列を増やす
        addTreeLabels(targetDiv, valueCounts);

        for (var i = 0; i < valueCounts; i++) {
            var targetRow = $(targetDiv).find('div.vertical_list').eq(i);
            var node = selectedNodes[i];
            // 呼び元に反映
            setStructureInfoToTreeLabelByNode(targetRow, node);
            if (node.parents) {
                // 親ノードの情報を呼び元に反映
                for (parentId of node.parents) {
                    var parentNode = $(tree).jstree(true).get_node(parentId);
                    if (parentNode.parent == '#') {
                        // ルートノードは対象外
                        break;
                    }
                    setStructureInfoToTreeLabelByNode(targetRow, parentNode);
                }
            }
            var structureNo = getTreeViewLayerNo(node);
            for (var j = structureNo + 1; j <= maxStructureNo; j++) {
                // 呼び元の選択ノードより下層の情報をクリア
                setStructureInfoToTreeLabelDiv(targetRow, j, '', '');
            }
            targetRow = null;
        }

        $(modal).modal('hide');
        // ツリービュー要素のクリア
        $(modal).find('#form_tree_view').children().remove();

        //【オーバーライド用関数】階層選択モーダル画面の選択ボタン実行後
        afterSelectBtnForTreeView(appPath, btn, ctrlId, structureGrpId, maxStructureNo, node);

    });
}

/**
 * ツリー選択ラベルへの選択値設定処理(ノード指定)
 * @param {HTMLElement} targetDiv   ：対象DIV要素
 * @param {jstreeNode} node         ：設定値取得先のノード
 */
function setStructureInfoToTreeLabelByNode(targetDiv, node) {
    var structureNo = getTreeViewLayerNo(node);
    var structureId = getTreeViewStructureId(node);

    setStructureInfoToTreeLabelDiv(targetDiv, structureNo, structureId, node.text)
}

/**
 * ツリー選択ラベルへの選択値設定処理(階層番号指定)
 * @param {HTMLElement} targetDiv   ：対象DIV要素
 * @param {number} structureNo      ：設定する階層番号
 * @param {number} structureId      ：設定する構成ID
 * @param {string} text             ：設定する表示文字列
 */
function setStructureInfoToTreeLabelDiv(targetDiv, structureNo, structureId, text) {
    // 対象のtdを取得
    const td = $(targetDiv).find('td[data-type="treeLabel"][data-structureno="' + structureNo + '"]');
    if (td && td.length > 0) {
        // 構成マスタ情報を設定
        setStructureInfoToTreeLabel(td, structureId, text);
    }
}

/**
 * ツリー選択ラベルへの選択値設定処理(td指定)
 * @param {HTMLElement} td          ：対象TD要素
 * @param {number} structureId      ：設定する構成ID
 * @param {string} text             ：設定する表示文字列
 */
function setStructureInfoToTreeLabel(td, structureId, text) {
    const prevStructureId = $(td).data('structureid');
    if (prevStructureId != structureId) {
        // 構成IDが変更される場合のみ、tdに構成IDと表示文字列を設定
        setAttrByNativeJs(td, 'data-structureid', structureId);
        $(td).text(text);
        if ($(td).data('factoryctrl') == 'True') {
            // 工場コントロールの場合、tdのchageイベントを発生させる
            let event = new Event("change");
            $(td).get(0).dispatchEvent(event);
        }
    }
}

/**
 *  エラーをツールチップに表示する
 */
function setErrorTooltipForPassChange(element) {

    var items = $(element).find("input.errorcom");  // エラー項目
    $.each(items, function (i, item) {

        var id = $(item).attr("id");
        var label = $("label.errorcom[for='" + id + "']");  // エラー情報要素
        var text = $(label).text();                         // エラー文言

        // エラー項目のツールチップにエラー文言をセット
        setAttrByNativeJs(item, "title", text);
        // エラー情報要素を削除
        $(label).remove();
    });

}

/**
 *  処理結果ｽﾃｰﾀｽを画面状態に反映(※処理中断ｽﾃｰﾀｽ返却時共通処理)
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {object} ：処理結果ｽﾃｰﾀｽ(CommonProcReturn)
 *  @param {string} ：確認ﾒｯｾｰｼﾞOKﾎﾞﾀﾝ押下時処理継続用ｺｰﾙﾊﾞｯｸ関数呼出文字列
 */
function setReturnStatus(appPath, status, eventFunc) {
    if (status == null || (status.STATUS == null && status.status == null)) {
        return true;
    }
    if (status.status != null) {
        if (status.status == 400) {
            // BadRequestが返ってきた場合はタイムアウトエラーとして処理する
            status.STATUS = procStatus.TimeOut;
        } else if (status.status == 500) {
            status.STATUS = procStatus.SystemError;
        }
    }

    if (status.STATUS == procStatus.Confirm) {
        //処理結果ｽﾃｰﾀｽが確認（処理中断、確認ﾒｯｾｰｼﾞ表示）の場合

        //確認ﾒｯｾｰｼﾞ番号を取得
        var confirmNo = 0;
        if (!isNaN(status.LOGNO)) {
            confirmNo = parseInt(status.LOGNO, 10);
        }
        if ((confirmNo < 1 || confirmNo > 100) && confirmNo != 999) { //VAL1～VAL100の範囲外
            //※confirmNo = 999　⇒出力最大件数超過の確認ﾒｯｾｰｼﾞ
            //※ここに入ることはあり得ない

            //実装不正時、ｴﾗｰﾒｯｾｰｼﾞを表示
            setMessage(P_ComMsgTranslated[941120006], procStatus.InValid);     //【CJ00000.E02】処理不正です。システム担当者に連絡してください。
            return false;
        }

        clearMessage();     //ﾒｯｾｰｼﾞを初期化

        //確認ﾒｯｾｰｼﾞを表示
        //　～OKの場合
        //　　　DATATYPE=20:メッセージ確認済みｽﾃｰﾀｽのﾃﾞｰﾀを中間ﾃｰﾌﾞﾙに保存
        //　　　再度、業務ﾛｼﾞｯｸﾌﾟﾛｼｰｼﾞｬをｺｰﾙする

        // 確認ﾒｯｾｰｼﾞを表示
        if (!popupMessage([status.MESSAGE], messageType.Confirm, eventFunc)) {
            //『キャンセル』の場合、処理中断
            return false;
        }

        return true;

    } else if (status.STATUS == procStatus.TimeOut ||
        status.STATUS == procStatus.LoginError ||
        status.STATUS == procStatus.AccessError ||
        status.STATUS == procStatus.SystemError) {

        // イベントを削除
        $(window).off('beforeunload');

        //ｴﾗｰ画面に遷移
        var form = $(P_Article).find("form[id^='formDetail']");
        if (form == null || form.length == 0) {
            form = $(P_Article).find("form[id^='formEdit']");
        }
        setAttrByNativeJs(form, "method", "POST");
        setAttrByNativeJs(form, "action", appPath + "Common/RedirectAccessError");
        //$(form).validate().cancelSubmit = true;    // Validatorを実行させないようにする
        //$(form).removeData('validator');    // Validatorを実行させないようにする
        if ($(form).data('validator') && $(form).data('validator').cancelSubmit) {
            $(form).data('validator').cancelSubmit = true;
        }
        $(form).submit();

    } else {
        //異常時、ｴﾗｰﾒｯｾｰｼﾞを表示
        setMessage(status.MESSAGE, status.STATUS);
        addMessageLogNo(status.LOGNO, status.STATUS);
    }
}

/**
 *  処理用CtrlIdをhidden項目にｾｯﾄ（※submit用）
 *  @param {要素} ：ﾌｫｰﾑ要素
 *  @param {string} ：処理用CtrlId
 */
function setSubmitCtrlId(form, ctrlId) {
    $(form).find("input:hidden[name='CTRLID']").val(ctrlId);
}

/**
 *  条件ﾃﾞｰﾀをJSON形式文字列にしてhidden項目にｾｯﾄ（※submit用）
 *  @param {number} formNo      ：画面番号
 *  @param {Element} form       ：検索ｴﾘｱﾌｫｰﾑ要素
 *  @param {number} isDispVal   ：値取得ﾓｰﾄﾞ(0:ｺｰﾄﾞ値を採用, 1：表示値を採用)
 *  @param {Array.<object>} conditionDataList   ：検索条件リスト
 */
function setSubmitDataCondition(formNo, form, isDispVal, conditionDataList) {

    if (!conditionDataList) {
        // 検索条件取得
        var tblSearch = getConditionTable();            //条件一覧要素
        //var conditionDataList = [];   //条件ﾃﾞｰﾀﾘｽﾄ
        //var conditionData = getConditionData(formNo, tblSearch, isDispVal);   //条件ﾃﾞｰﾀ
        //if (conditionData != null) {
        //    conditionDataList.push(conditionData);
        //}
        conditionDataList = getConditionData(formNo, tblSearch);    //条件ﾃﾞｰﾀ
    }

    $(form).find("input:hidden[name='ConditionData']").val(JSON.stringify(conditionDataList));
}

/**
 *  明細ﾃﾞｰﾀﾘｽﾄをJSON形式文字列にしてhidden項目にｾｯﾄ（※submit用）
 *  @param {要素} ：明細ｴﾘｱﾌｫｰﾑ要素
 *  @param {byte}      ：画面番号
 *  @param {number}    ：値取得ﾓｰﾄﾞ(0:ｺｰﾄﾞ値を採用, 1：表示値を採用)
 */
function setSubmitDataList(form, formNo, isDispVal) {

    // 明細ﾃﾞｰﾀ取得
    // 一覧の明細ﾃﾞｰﾀﾘｽﾄ(入力値)を取得
    //var areaKbn = 1;      //一覧画面（固定）とする
    //var listData = getListDataAll(areaKbn, formNo, isDispVal);     //ｺｰﾄﾞ値を取得
    var listData = getListDataAll(formNo, isDispVal);   //明細ｴﾘｱ、ﾄｯﾌﾟｴﾘｱ、ﾎﾞﾄﾑｴﾘｱの一覧ﾃﾞｰﾀを全て取得

    $(form).find("input:hidden[name='ListData']").val(JSON.stringify(listData));
}

/**
 *  個別実装用ﾃﾞｰﾀﾘｽﾄをJSON形式文字列にしてhidden項目にｾｯﾄ（※submit用）
 *  @param {要素} ：明細ｴﾘｱﾌｫｰﾑ要素
 */
function setSubmitDataIndividual(form) {

    $(form).find("input:hidden[name='ListIndividual']").val(JSON.stringify(P_dicIndividual));
}

/**
 *  明細ﾃﾞｰﾀﾘｽﾄをJSON形式文字列にしてhidden項目にｾｯﾄ（※submit用）
 *  @param {要素} ：明細ｴﾘｱﾌｫｰﾑ要素
 *  @param {byte}      ：画面番号
 *  @param {number}    ：値取得ﾓｰﾄﾞ(0:ｺｰﾄﾞ値を採用, 1：表示値を採用)
 */
function setSubmitHonyakuList(form, formNo) {

    var listHonyaku = [];

    //明細ｴﾘｱの一覧
    var formDetail = $(P_Article).find("#" + P_formDetailId);
    var tblDetails = $(formDetail).find("table");

    $.each(tblDetails, function (i, tbl) {

        var editKbn = $(tbl).data("editkbn");   // 編集区分
        if (editKbn == editPtnDef.Input) {
            //※編集ﾊﾟﾀｰﾝ：「直接入力」の場合

            //※ベタ表出力時、改ページありの場合、ｺｰﾄﾞ値／なしの場合、表示値を出力
            var pageRows = $(tbl).data("pagerows");   // 改ページ
            if (pageRows > 0) {     //改ページあり⇒ｻｰﾊﾞｰ側で翻訳
                //ｺﾝﾎﾞﾎﾞｯｸｽの翻訳情報を取得
                var listHonyakuW = getHonyakuTable(tbl);
                listHonyaku = listHonyaku.concat(listHonyakuW);
            }
        }
        else {
            //※編集区分：明細入力、親子遷移画面

            //ｺﾝﾎﾞﾎﾞｯｸｽの翻訳情報を取得
            var listHonyakuW = getHonyakuTable(tbl);
            listHonyaku = listHonyaku.concat(listHonyakuW);

        }
    });

    $(form).find("input:hidden[name='ListHonyaku']").val(JSON.stringify(listHonyaku));
}

/**
 *  画面遷移前の編集破棄確認ﾒｯｾｰｼﾞ表示
 *  @param {string}     appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {number}     transPtn    ：画面遷移ﾊﾟﾀｰﾝ(1：子画面、2：単票入力、3：単票参照、4：共通機能、5：他機能別ﾀﾌﾞ、6：他機能表示切替)
 *  @param {number}     transDiv    ：画面遷移アクション区分(1：新規、2：修正、3：複製、4：出力、)
 *  @param {string}     transTarget ：遷移先(子画面NO / 単票一覧ctrlId / 共通機能ID / 他機能ID|画面NO)
 *  @param {number}     dispPtn     ：遷移表示ﾊﾟﾀｰﾝ(1：表示切替、2：ﾎﾟｯﾌﾟｱｯﾌﾟ、3：一覧直下)
 *  @param {number}     formNo      ：遷移元formNo
 *  @param {string}     ctrlId      ：遷移元の一覧ctrlid
 *  @param {string}     btn_ctrlId  ：ボタンのbtn_ctrlid
 *  @param {int}        rowNo       ：遷移元の一覧の選択行番号（新規の場合は-1）
 *  @param {html}       element     ：ｲﾍﾞﾝﾄ発生要素
 *  @param {boolean}    confirmFlg  ：遷移前確認ﾌﾗｸﾞ(true：確認する、false：確認しない)
 */
//function confirmScrapBeforeTrans(appPath, transPtn, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element, confirmFlg) {
function confirmScrapBeforeTrans(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element, confirmFlg) {

    if (confirmFlg && dataEditedFlg) {
        // 画面変更ﾌﾗｸﾞONの場合、ﾃﾞｰﾀ破棄確認ﾒｯｾｰｼﾞをﾎﾟｯﾌﾟｱｯﾌﾟ
        P_MessageStr = P_ComMsgTranslated[941060005]; //『画面の編集内容は破棄されます。よろしいですか？』
        //確認OK時処理を設定
        var eventFunc = function () {
            // 画面遷移処理の分岐
            transForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element);
        }
        // 確認ﾒｯｾｰｼﾞを表示
        if (!popupMessage([P_MessageStr], messageType.Confirm, eventFunc)) {
            //『キャンセル』の場合、処理中断
            return false;
        }
    }
    else {
        // 画面遷移処理の分岐
        transForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element);
    }
}

/**
 *  画面遷移処理
 *  @param {string}     appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {number}     transPtn    ：画面遷移ﾊﾟﾀｰﾝ(1：子画面、2：単票入力、4：共通機能、5：他機能別ﾀﾌﾞ、6：他機能表示切替)
 *  @param {number}     transDiv    ：画面遷移アクション区分(1：新規、2：修正、3：複製、4：出力、)
 *  @param {string}     transTarget ：遷移先(子画面NO / 単票一覧ctrlId / 共通機能ID / 他機能ID|画面NO)
 *  @param {number}     dispPtn     ：遷移表示ﾊﾟﾀｰﾝ(1：表示切替、2：ﾎﾟｯﾌﾟｱｯﾌﾟ、3：一覧直下)
 *  @param {number}     formNo      ：遷移元formNo
 *  @param {string}     ctrlId      ：遷移元の一覧ctrlid
 *  @param {string}     btn_ctrlId  ：ボタンのbtn_ctrlid
 *  @param {int}        rowNo       ：遷移元の一覧の選択行番号（新規の場合は-1）
 *  @param {html}       element     ：ｲﾍﾞﾝﾄ発生要素
 */
//function transForm(appPath, transPtn, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element) {
function transForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element, article) {

    //【オーバーライド用関数】遷移処理の前
    var [isContinue, conditionDataList] =
        prevTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element);

    if (isContinue == false) {
        // 遷移が前処理でキャンセルされた場合は終了
        return;
    }

    // イベント発生要素から現在のモーダル画面番号取得
    var modalNo = getCurrentModalNo(element);

    if (transPtn == transPtnDef.Child) {
        //※子画面表示の場合

        //選択ﾃﾞｰﾀ取得
        var conditionData = null;
        if (ctrlId != null && rowNo > 0) {
            var tbl = $(P_Article).find("#" + ctrlId + "_" + formNo);  //選択一覧
            var ctrlType = $(tbl).data('ctrltype');
            if (ctrlType == ctrlTypeDef.IchiranPtn3) {
                conditionData = getTempDataForTabulator(formNo, rowNo, "#" + ctrlId + "_" + formNo);  //※常にｺｰﾄﾞ値
            } else {
                conditionData = getTempData(formNo, tbl, rowNo);  //行データ取得※常にｺｰﾄﾞ値
            }
            conditionDataList.push(conditionData);  //退避用はﾘｽﾄで保持
            // 選択行ﾃﾞｰﾀを子画面の検索時条件として退避
            var form = $(P_Article).find("form[id^='formDetail']");
            var conductId = $(form).find('input[name="CONDUCTID"]').val();
            setSearchCondition(conductId, Number(transTarget), conditionDataList);
        }

        //子画面遷移処理
        transChildForm(appPath, transTarget, dispPtn, transDiv, formNo, btn_ctrlId, conditionDataList, modalNo, article);

    }
    else if (transPtn == transPtnDef.Edit || transPtn == transPtnDef.Reference) {
        //※単票表示の場合

        //選択ﾃﾞｰﾀ取得
        var rowData = null;
        var tbl = $(P_Article).find("#" + transTarget + "_" + formNo);  //選択一覧
        var ctrlType = $(tbl).data('ctrltype');
        if (rowNo > 0) {
            //※新規モード以外の場合

            if (ctrlType == ctrlTypeDef.IchiranPtn3) {
                rowData = getTempDataForTabulator(formNo, rowNo, "#" + transTarget + "_" + formNo);  //※常にｺｰﾄﾞ値
            } else {
                rowData = getTempData(formNo, tbl, rowNo);     //0:コード値を採用
            }
        }

        // 詳細画面翻訳対応　単票画面の場合、詳細画面の翻訳と同じ翻訳で表示するよう対応
        var headerLabels = $(tbl).find(".tabulator-col-title"); // 一覧ヘッダのラベルリスト(文字列を取得)
        // 変更する列名のリスト、"VAL1"："機器番号"のような連想配列を作成
        var headerChanges = {};
        $.each(headerLabels, function (idx, elm) {
            var colVal = $(elm).closest(".tabulator-col").attr("tabulator-field");
            var colLabel = $(elm).text();
            headerChanges[colVal] = colLabel;
        });

        //単票表示処理
        transEditForm(appPath, transPtn, transTarget, transDiv, dispPtn, formNo, rowData, modalNo, headerChanges);

    }
    else if (transPtn == transPtnDef.CmConduct) {
        //共通機能遷移の場合

        //選択ﾃﾞｰﾀ取得
        var conditionData = null;
        if (ctrlId != null && rowNo > 0) {
            //※一覧内リンクからの遷移の場合だけ
            var tbl = $(P_Article).find("#" + ctrlId + "_" + formNo);  //選択一覧
            var ctrlType = $(tbl).data('ctrltype');
            if (ctrlType == ctrlTypeDef.IchiranPtn3) {
                conditionData = getTempDataForTabulator(formNo, rowNo, "#" + ctrlId + "_" + formNo);  //※常にｺｰﾄﾞ値
            } else {
                conditionData = getTempData(formNo, tbl, rowNo);  //行データ取得※常にｺｰﾄﾞ値
            }
            conditionDataList.push(conditionData);
        }

        //共通機能表示処理
        transCmConduct(appPath, transDiv, transTarget, formNo, conditionDataList, ctrlId, btn_ctrlId, rowNo, element, modalNo);
    }
    else if (transPtn == transPtnDef.OtherTab) {
        //他機能遷移(別タブ)の場合
        transOtherTab(appPath, transDiv, transTarget, formNo, conditionDataList, ctrlId, btn_ctrlId, rowNo, element, "_blank", modalNo);
    }
    else if (transPtn == transPtnDef.OtherShift) {
        //他機能遷移(同タブ表示切替)の場合
        //transOtherShift();
        /* AKAP2.0Version start */
        transOtherTab(appPath, transDiv, transTarget, formNo, conditionDataList, ctrlId, btn_ctrlId, rowNo, element, "_self", modalNo);
        /* AKAP2.0Version end */
    }

    var promise = new Promise((resolve) => {
        //【オーバーライド用関数】遷移処理の後
        postTransForm(appPath, transPtn, transDiv, transTarget, dispPtn, formNo, ctrlId, btn_ctrlId, rowNo, element);
        //処理が完了したことを通知（thenの処理を実行）
        resolve();
    }).then(() => {
        //実行ボタンが全て非活性の場合、戻るor閉じるボタンにフォーカス設定
        setFocusBackOrClose();
    })
}

/**
 * 子画面遷移
 * @param {string}                      appPath         :ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 * @param {number}                      childNo         :子画面NO
 * @param {number}                      dispPtn         :遷移表示ﾊﾟﾀｰﾝ　1：表示切替、2：ﾎﾟｯﾌﾟｱｯﾌﾟ
 * @param {number}                      transDiv        :画面遷移アクション区分(1：新規、2：修正、3：複製、4：出力、)
 * @param {number}                      parentNo        :親画面NO
 * @param {string}                      btn_ctrlId      :ﾎﾞﾀﾝｺﾝﾄﾛｰﾙID(子画面新規ﾎﾞﾀﾝの場合)
 * @param {Array.<Dictionary<string, string>>}  conditionDataList   :条件ﾃﾞｰﾀ
 * @param {number}                      modalNo         :現在のモーダル画面番号
 */
function transChildForm(appPath, childNo, dispPtn, transDiv, parentNo, btn_ctrlId, conditionDataList, modalNo, article) {
    if (!article) {
        form = $(P_Article).find("#" + P_formDetailId);
    } else {
        form = $(article).find("form[id^='formDetail']");
    }
    var conductId = $(form).find('input[name="CONDUCTID"]').val();  //機能ID
    var pgmId = $(form).find('input[name="PGMID"]').val();          //ﾌﾟﾛｸﾞﾗﾑID
    var conductPtn = $(form).find('input[name="CONDUCTPTN"]').val();    //処理ﾊﾟﾀｰﾝ

    var articlePop = $('article.base_form[name="main_area"][data-formno="' + childNo + '"]');   //子画面ｴﾘｱ

    var formTitle = $(articlePop).data("formtitle");    //子画面ﾀｲﾄﾙ

    // base_formのidに"_base"が付加されている場合、元に戻す
    $.each($(articlePop).find('[id$="_base"]'), function (idx, elm) {
        var id = $(elm).attr('id').replace('_base', '');
        setAttrByNativeJs(elm, 'id', id);
    });

    // 画面遷移アクション区分をセット
    $(articlePop).find("input:hidden[name='TRANSACTIONDIV']").val(transDiv);

    if (dispPtn == transDispPtnDef.Shift) {
        //※表示切替の場合

        //画面ﾀｲﾄﾙ設定
        if (formTitle != null && formTitle.length > 0) {
            formTitle = " > " + formTitle;
        }
        $("#top_title_area span.form_title").text(formTitle);

        //親画面をクリアして非表示
        // - 検索結果をクリア
        if (!(parentNo == 0 && notSearchConductIdList.indexOf(conductId) >= 0)) { //詳細画面から一覧画面へ戻る際に再検索を行わない機能は検索結果をクリアしない
            clearSearchResult();
        }
        // - 明細エリアのエラー状態を初期化
        clearMessage();
        // - 画面ｴﾘｱ非表示
        $(P_Article).removeClass('selected');
    }
    else {
        //※ﾎﾟｯﾌﾟｱｯﾌﾟの場合

        // 画面ｺﾋﾟｰ前にｵｰﾄｺﾝﾌﾟﾘｰﾄの削除
        // ※ｺﾋﾟｰ元のｵｰﾄｺﾝﾌﾟﾘｰﾄが残っていると動作しないため
        var autoComps = $(articlePop).find(":text[data-autocompdiv!=" + autocompDivDef.None + "], textarea[data-autocompdiv!=" + autocompDivDef.None + "]");
        destroyAutocomplete(autoComps);
        autoComps = null;

        // 日付コントロールの削除
        destroyDatepickerCtrls(articlePop);

        var articlePopBase = articlePop;

        // - 画面ｺﾋﾟｰ
        articlePop = $(articlePopBase).clone(true);
        $(articlePop).removeClass('base_form');

        // base_formのidに"_base"を付加
        $.each($(articlePopBase).find('*'), function (idx, elm) {
            var id = $(elm).attr('id');
            if (id != undefined && id) {
                setAttrByNativeJs(elm, 'id', id + '_base');
            }
        });

        // - ﾎﾟｯﾌﾟｱｯﾌﾟ画面設定
        let modal = getNextModalElement(modalNo);
        if (false == $(modal).hasClass('childForm')) {
            $(modal).addClass('childForm');
        }

        // 子画面エリア以外の要素を選択解除(削除はNG)
        $(modal).find("article[name!='main_area']").removeClass("selected");

        // - ﾀｲﾄﾙ設定
        $(modal).find(".modal-header-my .title_div span").text(formTitle);

        // -- ｺﾋﾟｰした画面をmodalに挿入
        var detailDiv = $(modal).find('.form_detail_div');
        $(detailDiv).find("article[name='main_area']").remove();
        $(articlePop).appendTo(detailDiv);

        // 戻るボタンの初期化
        var backBtn = $(articlePop).find('input:button[data-actionkbn="' + actionkbn.Back + '"]');
        initBackBtnForPopup(appPath, backBtn, transPtnDef.Child);

        // 日付コントロールの初期化(APより移行)
        initDateTypePicker($(articlePop).find("tr:not('.base_tr')"), false);

        // コンボボックスの初期化
        resetComboBox(appPath, $(articlePop).find('select'));

        // - ﾎﾟｯﾌﾟｱｯﾌﾟ表示
        showModalForm(modal);

        $(modal).find('.modal-content').width('');
        $(modal).off('shown.bs.modal');
        $(modal).on('shown.bs.modal', function (e) {
            // モーダル画面の表示イベント後の初期化
            initShownModalForm(e.currentTarget);
        });

        $(modal).find('.modal-content').animate({ scrollTop: 0 }, '1');      //ｽｸﾛｰﾙを先頭へ移動
        //ﾓｰﾀﾞﾙ閉じた後ｲﾍﾞﾝﾄに戻る処理を設定
        $(modal).off('hidden.bs.modal');    //※複数回表示する場合のおまじない
        $(modal).on('hidden.bs.modal', function (e) {
            // モーダル画面の非表示イベント後の初期化
            initHiddenModalForm(appPath, e.currentTarget, transPtnDef.Child);
        });
        modal = null;
    }

    //子画面表示
    $(articlePop).addClass('selected');

    //CTRLID：「Init」(固定)で画面初期化時ｱｸｼｮﾝの業務ﾛｼﾞｯｸを呼び出す
    initForm(appPath, conductId, pgmId, childNo, parentNo, btn_ctrlId, conductPtn, conditionDataList, null, null, skipGetDataDef.GetData, null, transPtnDef.Child);
}

/**
 * 単票表示
 * @param {string} appPath      :ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 * @param {number} transPtn     :画面遷移ﾊﾟﾀｰﾝ(2：単票入力、3：単票参照)
 * @param {number} transDiv     :画面遷移アクション区分(1：新規、2：修正、3：複製、4：出力、)
 * @param {string} transTarget  :単票一覧ctrlId
 * @param {number} dispPtn      :遷移表示ﾊﾟﾀｰﾝ(2：ﾎﾟｯﾌﾟｱｯﾌﾟ、3：一覧直下)
 * @param {number} formNo       :画面NO
 * @param {Dictionary<string, string>} rowData    :選択ﾃﾞｰﾀ
 * @param {number} modalNo      :現在のモーダル画面番号
 * @param {Array<Dictionary<string, string>>} headerChanges : 置換を行うためのヘッダのラベル(キーに列のdata-name、値にラベル)
*/
function transEditForm(appPath, transPtn, transTarget, transDiv, dispPtn, formNo, rowData, modalNo, headerChanges) {

    //単票表示ｴﾘｱ
    var editDivBase = $(P_Article).find('#' + transTarget + "_" + formNo + '_edit_div');

    // base_formのidに"_base"が付加されている場合、元に戻す
    $.each($(editDivBase).find('[id$="_base"]'), function (idx, elm) {
        var id = $(elm).attr('id').replace('_base', '');
        setAttrByNativeJs(elm, 'id', id);
    });

    if (dispPtn == transDispPtnDef.Popup) {
        //※ﾎﾟｯﾌﾟｱｯﾌﾟの場合

        // 画面ｺﾋﾟｰ前にｵｰﾄｺﾝﾌﾟﾘｰﾄの削除
        // ※ｺﾋﾟｰ元のｵｰﾄｺﾝﾌﾟﾘｰﾄが残っていると動作しないため
        var autoComps = $(editDivBase).find(":text[data-autocompdiv!=" + autocompDivDef.None + "], textarea[data-autocompdiv!=" + autocompDivDef.None + "]");
        destroyAutocomplete(autoComps);
        autoComps = null;

        // 日付コントロールの削除
        destroyDatepickerCtrls(editDivBase);

        // - 画面ｺﾋﾟｰ
        var editDiv = $(editDivBase).clone(true);
        $(editDiv).removeClass('base_form');

        // base_formのidに"_base"を付加
        $.each($(editDivBase).find('*'), function (idx, elm) {
            var id = $(elm).attr('id');
            if (id != undefined && id) {
                setAttrByNativeJs(elm, 'id', id + '_base');
            }
        });

        // 単票の元の一覧に合わせてラベルを置換
        for (var key in headerChanges) {
            var target = $(editDiv).find("th[data-name='" + key + "']"); // 対象のthタグ
            var temp = $(target).find("*"); // 子孫要素を退避
            // ラベルを置換
            var value = headerChanges[key] + '';
            $(target).text(value);
            // 退避した子孫要素を戻す
            $(target).append(temp);
        }

        //ﾓｰﾀﾞﾙ画面設定
        var modal = getNextModalElement(modalNo);
        if (true == $(modal).hasClass('childForm')) {
            $(modal).removeClass('childForm');
        }
        // 単票エリア以外の要素を削除
        $(modal).find("article[name!='edit_area']").remove();

        //単票ﾎﾟｯﾌﾟｱｯﾌﾟ画面ﾀｲﾄﾙ
        var popupTitle = $(P_Article).find('#' + transTarget + "_" + formNo + '_div .ctrlId').data("ctrlname");
        $(modal).find(".modal-header-my .title_div span").text(popupTitle);

        // -- ｺﾋﾟｰした画面を挿入
        var formEdit = $(modal).find('form[id^="formEdit"]');
        $(formEdit).children().not("input[type='hidden']").remove();
        $(editDiv).appendTo(formEdit);
        $(editDiv).removeClass("hide");
        //$(editDiv).removeClass("init_hide");

        //戻るボタンの初期化
        var backBtn = $(editDiv).find('input:button[data-actionkbn="' + actionkbn.Back + '"]');
        initBackBtnForPopup(appPath, backBtn, transPtn);
        backBtn = null;

        //// コンボボックスの初期化⇒一覧への設定処理の後へ移動
        //resetComboBox(appPath, $(editDiv).find('select'));

        // 一覧CTRLIDをdata-ctrlid属性にｾｯﾄ
        setAttrByNativeJs(formEdit, "data-ctrlid", transTarget);

        // FORMNO、CTRLIDをpost用ﾃﾞｰﾀにｾｯﾄ
        $(formEdit).find("input:hidden[name='FORMNO']").val(formNo);
        $(formEdit).find("input:hidden[name='CTRLID']").val(transTarget);
        $(formEdit).find("input:hidden[name='TRANSACTIONDIV']").val(transDiv);
        // PGMID,CONDUCTIDをセット
        $(formEdit).find("input:hidden[name='PGMID']").val(getArticleInfoByElm(editDivBase, "PGMID"));
        $(formEdit).find("input:hidden[name='CONDUCTID']").val(getArticleInfoByElm(editDivBase, "CONDUCTID"));

        // - ｺﾋﾟｰ画面ﾃﾞｰﾀ設定
        // 選択中の画面のフォーカスをクリア
        removeFocusSelected();
        //選択中画面NOｴﾘｱ
        P_Article = $(formEdit).closest("article[name='edit_area']");
        setAttrByNativeJs(P_Article, "data-formno", formNo);
        setAttrByNativeJs(P_Article, "data-ctrlid", transTarget);

        /* ボタン権限制御 切替 start ==================================================================== */
        ////選択中画面のﾎﾞﾀﾝの権限をpublic変数に保持
        //P_buttonDefine = [];    //public変数：ﾎﾞﾀﾝの権限情報を初期化
        ///*権限制御修正時に要修正*/
        //var buttons = $(editDiv).find("input:button[data-actionkbn]");
        //if ($(buttons).length) {
        //    $.each(buttons, function (i, btn) {
        //        // 一覧の画面定義条件を生成
        //        var bCtrlId = $(btn).attr("name");
        //        var bAuthKbn = "";
        //        if ($(btn).is("[data-authkbn]")) {
        //            bAuthKbn = $(btn).data("authkbn");
        //        }
        //        var define = {
        //            ctrlId: bCtrlId,                 // ﾎﾞﾀﾝのCTRLID
        //            authKbn: bAuthKbn,               // ﾎﾞﾀﾝの権限区分
        //        };
        //        P_buttonDefine.push(define);
        //    });
        //}
        /* ボタン権限制御 切替 end ==================================================================== */

        if (rowData != null) {
            //既存ﾃﾞｰﾀの場合、行ﾃﾞｰﾀをｾｯﾄ
            var data = [];
            data.push(rowData);
            dispDataVertical(appPath, data, formNo, true);
        }
        else {
            //新規の場合
            //行Noを0に設定⇒※以降の処理で新規ﾓｰﾄﾞ判定に使用
            var editTbl = $(P_Article).find("#" + transTarget + formNo + "_edit");
            setAttrByNativeJs(editTbl, "data-rowno", 0);

            // 削除ボタンを非活性化
            var deleteBtn = $(P_Article).find('input:button[data-actionkbn="' + actionkbn.Delete + '"]');
            if (deleteBtn.length > 0) {
                setDisableBtn(deleteBtn, true);  //ﾎﾞﾀﾝを非活性化
            }
            deleteBtn = null;
        }

        // コンボボックスの初期化
        resetComboBox(appPath, $(editDiv).find('select'));

        //入力ﾌｫｰﾏｯﾄ設定処理
        var formEditId = "#" + $(formEdit).attr('id');
        initFormat(formEditId);
        // Validator初期化
        initValidator(formEditId);

        //※該当一覧の単票入力画面表示
        $(P_Article).removeClass('hide');
        $(P_Article).addClass('selected');

        // - ﾎﾟｯﾌﾟｱｯﾌﾟ表示
        showModalForm(modal);

        $(modal).find('.modal-content').width('');
        $(modal).off('shown.bs.modal');
        $(modal).on('shown.bs.modal', function (e) {
            // モーダル画面の表示イベント後の初期化
            initShownModalForm(e.currentTarget);
        });

        //閉じた後のｲﾍﾞﾝﾄ処理
        //※複数回表示する場合のおまじない
        $(modal).off('hidden.bs.modal');    //※複数回表示する場合のおまじない
        $(modal).on('hidden.bs.modal', function (e) {
            // モーダル画面の非表示イベント後の初期化
            initHiddenModalForm(appPath, e.currentTarget, transPtnDef.Edit);
        });

        // オートコンプリートの初期化
        var autoComps = $(P_Article).find(formEditId).find(".ctrlId")
            .find(":text[data-autocompdiv!=" + autocompDivDef.None + "], textarea[data-autocompdiv!=" + autocompDivDef.None + "]");
        $(autoComps).each(function (index, element) {
            var elementId = "#" + element.id;
            $.each(P_autocompDefines, function (idx, define) {
                if (elementId.substr(0, elementId.length - 1) == define.key) { //idの先頭が一致する定義情報
                    var sqlId = define.sqlId;
                    var param = define.param;
                    var div = define.division;
                    var option = define.option;
                    initAutoComp(appPath, elementId, sqlId, param, '.modal-dialog', div, option);
                    return false;
                }
            });
        });
        autoComps = null;

        // 日付コントロールの初期化
        initDateTypePicker($(P_Article).find(formEditId).find(".ctrlId"), false);

        // 画面変更ﾌﾗｸﾞ初期化
        dataEditedFlg = false;
        // ｲﾍﾞﾝﾄを再設定
        setEventForEditFlg(true, null, formEditId);
    }
    else if (dispPtn == transDispPtnDef.UnderList) {
        //※一覧直下の場合

        if (rowData != null) {
            //既存ﾃﾞｰﾀの場合、行ﾃﾞｰﾀをｾｯﾄ
            var data = [];
            data.push(rowData);
            dispDataVertical(appPath, data, formNo, true);
        }
        else {
            //新規の場合
            //行Noを0に設定⇒※以降の処理で新規ﾓｰﾄﾞ判定に使用
            var editTbl = $(editDivBase).find("#" + transTarget + formNo + "_edit");
            setAttrByNativeJs(editTbl, "data-rowno", 0);

            // 削除ボタンを非活性化
            var deleteBtn = $(editDivBase).find('input:button[data-actionkbn="' + actionkbn.Delete + '"]');
            if (deleteBtn.length > 0) {
                setDisableBtn(deleteBtn, true);  //ﾎﾞﾀﾝを非活性化
            }
            deleteBtn = null;
        }
        //戻るボタンの初期化
        var backBtn = $(editDivBase).find('input:button[data-actionkbn="' + actionkbn.Back + '"]');
        initBackBtnForEditUnderList(appPath, backBtn);
        backBtn = null;

        //一覧直下単票ｴﾘｱ表示
        $(editDivBase).removeClass("hide");
    }
}

/**
 * 共通機能遷移
 * @param {string}                      appPath         :ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 * @param {number}                      conductId       :共通機能ID
 * @param {number}                      parentNo        :親画面NO
 * @param {Dictionary<string, string>}  conditionData   :条件ﾃﾞｰﾀ
 * @param {string}                      ctrlId          :遷移元の一覧ctrlid
 * @param {string}                      btn_ctrlId      :ボタンのbtn_ctrlid
 * @param {int}                         rowNo           :遷移元の一覧の選択行番号（一覧行でない場合は-1）
 * @param {html}                        element         :ｲﾍﾞﾝﾄ発生要素
 * @param {number}                      modalNo         :現在のモーダル画面番号
*/
function transCmConduct(appPath, transDiv, conductId, parentNo, conditionDataList, ctrlId, btn_ctrlId, rowNo, element, modalNo) {

    //遷移元の個別機能IDを取得
    var parentConductId = $(P_Article).find("#" + P_formDetailId + " input:hidden[name='CONDUCTID']").val();

    //【オーバーライド用関数】共通機能へのパラメータを個別実装用パブリック変数に退避する
    passDataCmConduct(appPath, conductId, parentNo, conditionDataList, ctrlId, btn_ctrlId, rowNo, element, parentConductId);

    var articlePop = $('article.base_form[name="common_area"][data-conductid="' + conductId + '"]');   //共通機能画面ｴﾘｱ

    //共通機能の情報
    var pgmId = $(articlePop).find('input[name="PGMID"]').val();          //ﾌﾟﾛｸﾞﾗﾑID
    var conductPtn = $(articlePop).find('input[name="CONDUCTPTN"]').val();    //処理ﾊﾟﾀｰﾝ

    // base_formのidに"_base"が付加されている場合、元に戻す
    $.each($(articlePop).find('[id$="_base"]'), function (idx, elm) {
        var id = $(elm).attr('id').replace('_base', '');
        setAttrByNativeJs(elm, 'id', id);
    });

    // 画面ｺﾋﾟｰ前にｵｰﾄｺﾝﾌﾟﾘｰﾄの削除
    // ※ｺﾋﾟｰ元のｵｰﾄｺﾝﾌﾟﾘｰﾄが残っていると動作しないため
    var autoComps = $(articlePop).find(":text[data-autocompdiv!=" + autocompDivDef.None + "], textarea[data-autocompdiv!=" + autocompDivDef.None + "]");
    destroyAutocomplete(autoComps);
    autoComps = null;

    // 日付コントロールの削除
    destroyDatepickerCtrls(articlePop);

    // 行追加/行コピー/行削除ボタンのクリックイベントを削除
    var listBtn = $(articlePop).find('a[data-actionkbn="' + actionkbn.AddNewRow + '"],[data-actionkbn="' + actionkbn.AddCopyRow + '"],[data-actionkbn="' + actionkbn.DeleteRow + '"]');
    if (listBtn != null && listBtn.length > 0) {
        // クリックイベントの削除
        $(listBtn).off('click');
    }
    listBtn = null;

    var articlePopBase = articlePop;

    // - 画面ｺﾋﾟｰ
    articlePop = $(articlePopBase).clone(true);
    $(articlePop).removeClass('base_form');

    //遷移元の機能ID・画面NOを共通機能に退避
    setAttrByNativeJs(articlePop, "data-parentconductid", parentConductId);                     //個別機能ID
    $(articlePop).find("form[id^='formDetail'] input:hidden[name='ORIGINNO']").val(parentNo);   //画面NO
    $(articlePop).find("form[id^='formDetail'] input:hidden[name='TRANSACTIONDIV']").val(transDiv); //画面遷移アクション区分
    var fromctrl = (btn_ctrlId != null) ? btn_ctrlId : ctrlId;
    setAttrByNativeJs(articlePop, "data-fromctrlid", fromctrl);                                 //遷移元のﾎﾞﾀﾝｺﾝﾄﾛｰﾙIDまたは一覧ctrlid

    // base_formのidに"_base"を付加
    $.each($(articlePopBase).find('*'), function (idx, elm) {
        var id = $(elm).attr('id');
        if (id != undefined && id) {
            setAttrByNativeJs(elm, 'id', id + '_base');
        }
    });

    // - ﾎﾟｯﾌﾟｱｯﾌﾟ画面設定
    var modal = getNextModalElement(modalNo);
    if (false == $(modal).hasClass('childForm')) {
        $(modal).addClass('childForm');
    }
    //ﾎﾟｯﾌﾟｱｯﾌﾟ画面ﾀｲﾄﾙ
    var popupTitle = $(articlePop).data("formtitle");
    $(modal).find(".modal-header-my .title_div span").text(popupTitle);

    // -- ｺﾋﾟｰした画面をmodalに挿入
    var detailDiv = $(modal).find('.form_detail_div');
    //$(detailDiv).find("article[name='common_area']").remove();
    $(detailDiv).find("article[name!='edit_area']").remove();
    $(detailDiv).find("article[name='edit_area']").addClass('hide');
    $(articlePop).appendTo(detailDiv);

    // 戻るボタンの初期化
    var backBtn = $(articlePop).find('input:button[data-actionkbn="' + actionkbn.Back + '"]');
    initBackBtnForPopup(appPath, backBtn, transPtnDef.CmConduct);

    // 行追加/行コピー/行削除ボタンの初期化
    // 行追加ボタン(リンク)
    var addNewRowBtn = $(articlePop).find('a[data-actionkbn="' + actionkbn.AddNewRow + '"]');
    if (addNewRowBtn != null && addNewRowBtn.length > 0) {
        // 行追加ボタンの初期化
        initAddNewRowBtn(appPath, addNewRowBtn);
    }
    addNewRowBtn = null;

    // 行コピーボタン(リンク)
    var addCopyRowBtn = $(articlePop).find('a[data-actionkbn="' + actionkbn.AddCopyRow + '"]');
    if (addCopyRowBtn != null && addCopyRowBtn.length > 0) {
        // 行コピーボタンの初期化
        initAddNewRowBtn(appPath, addCopyRowBtn);
    }
    addCopyRowBtn = null;

    // 行削除ボタン(リンク)
    var deleteRowBtn = $(articlePop).find('a[data-actionkbn="' + actionkbn.DeleteRow + '"]');
    if (deleteRowBtn != null && deleteRowBtn.length > 0) {
        // 行削除ボタンの初期化
        initDeleteRowBtn(appPath, deleteRowBtn);
    }
    deleteRowBtn = null;

    $(articlePop).show();

    // - ﾎﾟｯﾌﾟｱｯﾌﾟ表示
    showModalForm(modal);

    $(modal).find('.modal-content').width('');
    $(modal).off('shown.bs.modal');
    $(modal).on('shown.bs.modal', function (e) {
        // モーダル画面の表示イベント後の初期化
        initShownModalForm(e.currentTarget);
    });

    $(modal).find('.modal-content').animate({ scrollTop: 0 }, '1');      //ｽｸﾛｰﾙを先頭へ移動
    //ﾓｰﾀﾞﾙ閉じた後ｲﾍﾞﾝﾄに戻る処理を設定
    $(modal).off('hidden.bs.modal');    //※複数回表示する場合のおまじない
    $(modal).on('hidden.bs.modal', function (e) {
        // モーダル画面の非表示イベント後の初期化
        initHiddenModalForm(appPath, e.currentTarget, transPtnDef.CmConduct);
    });
    modal = null;
    //共通機能画面表示
    $(articlePop).addClass('selected');

    //CTRLID：「Init」(固定)で画面初期化時ｱｸｼｮﾝの業務ﾛｼﾞｯｸを呼び出す
    initForm(appPath, conductId, pgmId, 0, parentNo, null, conductPtn, conditionDataList, null, null, skipGetDataDef.GetData, null, transPtnDef.CmConduct);
}

/**
 * ﾄｯﾌﾟｴﾘｱ・明細ｴﾘｱ(単票を除く)・ﾎﾞﾄﾑｴﾘｱの一覧のﾃﾞｰﾀを全て取得
 * @param {number}  formNo      : 画面NO
 * @param {number}  isDispVal   : 値取得ﾓｰﾄﾞ(0:ｺｰﾄﾞ値を採用, 1：表示値を採用)
 */
function getListDataAll(formNo, isDispVal) {

    // 一覧の入力データ取得
    var listData = [];

    if (isDispVal == null) isDispVal = 0;   //初期化

    var areas = ["formTop", "formDetail", "formBottom"];

    $.each(areas, function (idx, area) {
        var areaForm = $("#" + area + "_" + formNo);
        var areaLists = $(areaForm).find(".ctrlId:not([id$='_edit'])");   //単票表示ﾚｲｱｳﾄ以外の一覧

        var listDataW = getListDataElements(areaLists, formNo, isDispVal);

        listData = listData.concat(listDataW);
    });
    return listData;
}

/**
 * ﾄｯﾌﾟｴﾘｱ・明細ｴﾘｱ(単票を除く)・ﾎﾞﾄﾑｴﾘｱの一覧のﾃﾞｰﾀを全て取得
 * @param {number}  isDispVal   : 値取得ﾓｰﾄﾞ(0:ｺｰﾄﾞ値を採用, 1：表示値を採用)
 */
function getListDataAllCmConduct(isDispVal) {

    // 一覧の入力データ取得
    var listData = [];

    if (isDispVal == null) isDispVal = 0;   //初期化

    var formNo = $(P_Article).attr("data-formno");
    var isComConduct = $(P_Article).attr("name") == "common_area";
    var cmConductId = $(P_Article).attr("data-conductid");
    var ex = isComConduct && (cmConductId != null && cmConductId.length > 0) ? cmConductId : formNo;

    var areas = "#formTop" + "_" + ex + ",#formBottom" + "_" + ex + ",#formDetail" + "_" + ex;
    var targetLists = $(P_Article).find(areas).find(".ctrlId:not([id$='_edit'])");

    listData = getListDataElements(targetLists, formNo, isDispVal);

    return listData;
}

/**
 * 渡された一覧要素のﾃﾞｰﾀを取得
 * @param {html}    elements    : 一覧要素(複数可)※.ctrlId単位
 * @param {number}  formNo      : 
 * @param {number}  isDispVal   : 値取得ﾓｰﾄﾞ(0:ｺｰﾄﾞ値を採用, 1：表示値を採用)
 * @param {boolean}  isSelectedOnly      : 選択行のみを取得
 */
function getListDataElements(elements, formNo, isDispVal, isSelectedOnly) {

    // 一覧の入力データ取得
    var listData = [];

    if (isDispVal == null) isDispVal = 0;   //初期化

    // ※渡された一覧要素すべてのﾃﾞｰﾀを取得する。
    $.each($(elements), function (idx, element) {

        //※直接入力一覧の場合のみ、現在ﾍﾟｰｼﾞの入力値をﾊﾞｯｸｱｯﾌﾟ
        //一覧の明細ﾃﾞｰﾀを取得する
        //※現在選択行場合は、UPDTAG=2とする
        //※ベタ表出力時、改ページありの場合、ｺｰﾄﾞ値／なしの場合、表示値を出力
        if (isDispVal == 1) {
            var pageRows = $(element).data("pagerows");   // 改ページ
            if (pageRows > 0) {     //改ページあり⇒ｻｰﾊﾞｰ側で翻訳
                isDispVal = 0;      //0:ｺｰﾄﾞ値
            }
        }
        var listDataW = null;
        if (true == $(element).hasClass("vertical_tbl")) {
            // 縦方向一覧
            listDataW = getListDataVertical(formNo, element);
        }
        else {
            // 横方向一覧
            listDataW = getListDataHorizontal(formNo, element, isDispVal, '', isSelectedOnly);
        }
        //listData.push(listDataW);
        listData = listData.concat(listDataW);
    });
    return listData;
}

/**
 * コントロールIDの配列から一覧要素のデータを取得
 * @param {Array.<string>}  ctrlIdList  : 対象一覧コントロールID配列
 * @param {number}          formNo      : 画面番号
 * @param {number}          isDispVal   : 値取得ﾓｰﾄﾞ(0:ｺｰﾄﾞ値を採用, 1：表示値を採用)
 * @param {boolean}         isEdit      : 
 * @param {boolean}         isSelectedOnly      : 選択行のみを取得
 * @return{Array.<Dictionary<string, string>}   : 一覧要素のデータ配列
 */
function getListDataByCtrlIdList(ctrlIdList, formNo, isDispVal, isEdit, isSelectedOnly) {
    // データ収集対象の一覧要素を取得
    var targets = getTargetListElementsByCtrlId(formNo, ctrlIdList, isEdit);
    // 対象一覧のデータを取得
    return getListDataElements(targets, formNo, isDispVal, isSelectedOnly);
}

/**
 *  ページデータ取得処理（ページング用）
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string} ：機能ID
 *  @param {string} ：プログラムID
 *  @param {number} ：ページ番号
 *  @param {number} ：1ページ当たりの行数
 *  @param {string} ：対象テーブルのコントロールID
 *  @param {string} ：一覧ソートキー
 */
function getPageData1(appPath, conductId, pgmId, formNo, pageNo, pageCount, targetTblId, sortKey) {

    //処理中メッセージ：on
    processMessage(true);

    var listData = [];
    //※直接入力一覧の場合、現在ﾍﾟｰｼﾞの入力値をﾊﾞｯｸｱｯﾌﾟ
    var tbl = $(P_Article).find("#" + targetTblId + "_" + formNo);
    var editKbn = tbl.data("editptn");
    if (editKbn == editPtnDef.Input) {
        // 編集ﾊﾟﾀｰﾝ：「直接入力」の場合

        // 改ﾍﾟｰｼﾞ対象一覧の明細ﾃﾞｰﾀﾘｽﾄ(入力値)を取得
        listData = getListDataHorizontal(formNo, tbl[0]);
    }
    //検索条件
    var tblSearch = getConditionTable();            //条件一覧要素
    //conditionData = getConditionData(formNo, tblSearch);   //条件ﾃﾞｰﾀ
    //var conditionDataList = [];
    //if (conditionData != null) {
    //    conditionDataList.push(conditionData);
    //}
    var conditionDataList = getSearchCondition(conductId, Number(formNo));

    //ﾍﾟｰｼﾞ情報
    W_listDefines = [];
    $.each(P_listDefines, function (i, define) {
        // 一覧の画面定義条件を生成
        if (targetTblId != null && targetTblId == define.CTRLID) {
            var defineW = $.extend(true, {}, define);
            defineW.VAL1 = pageNo;     //現在ﾍﾟｰｼﾞ
            W_listDefines.push(defineW);
        }
    });

    //共通使用区分
    if (sortKey != null && sortKey.length > 0) {
        comuseKbn = actionkbn.ListSort;         // 一覧ソート
    } else {
        comuseKbn = actionkbn.DataGet;          // データ取得
    }

    // POSTデータを生成
    var postdata = {
        conductId: conductId,                   // メニューの機能ID
        pgmId: pgmId,                           // メニューのプログラムID
        formNo: formNo,                         // フォーム番号
        ctrlId: targetTblId,                    // 一覧のコントロールID
        ListIndividual: P_dicIndividual,   // 個別実装用汎用ﾘｽﾄ

        conditionData: conditionDataList,       // 検索条件入力データ
        listDefines: W_listDefines,             // 一覧ﾍﾟｰｼﾞ情報

        browserTabNo: P_BrowserTabNo,           // ブラウザタブ識別番号

        //※表示中ﾍﾟｰｼﾞの入力値ﾊﾞｯｸｱｯﾌﾟ用
        listData: listData,                     // 一覧の入力データ
    };

    // 明細一覧の初期化
    clearSearchResult(targetTblId, formNo);

    // データ取得処理実行
    $.ajax({
        url: appPath + 'api/CommonProcApi/' + comuseKbn,
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(postdata),
        headers: { 'X-XSRF-TOKEN': getRequestVerificationToken() },
        traditional: true,
        cache: false
    }).then(
        // 1つめは通信成功時のコールバック
        function (resultInfo) {
            //[0]:処理ステータス - CommonProcReturn
            //[1]:結果データ - Dictionary<string, object>※結果ﾃﾞｰﾀ："Result"、個別実装用ﾃﾞｰﾀ："Individual"
            var status = resultInfo[0];
            var data = separateDicReturn(resultInfo[1], conductId);

            //処理メッセージを初期化
            clearMessage();

            //処理メッセージを表示
            if (status.MESSAGE != null && status.MESSAGE.length > 0) {
                //正常時、正常ﾒｯｾｰｼﾞ
                //警告、異常時、ｴﾗｰﾒｯｾｰｼﾞ
                addMessage(status.MESSAGE, status.STATUS);
            }
            // 画面変更ﾌﾗｸﾞ初期化
            dataEditedFlg = false;
            // ｲﾍﾞﾝﾄを一旦解除
            setEventForEditFlg(false, null, "#" + P_formTopId);
            setEventForEditFlg(false, null, "#" + P_formDetailId);
            setEventForEditFlg(false, null, "#" + P_formBottomId);

            //検索結果ﾃﾞｰﾀを明細一覧に表示
            var pageRowCount = dispListData(appPath, conductId, pgmId, formNo, data, false);

            //ﾍﾟｰｼﾞの状態を明細表示後アクション時に設定
            setPageStatus(pageStatus.SEARCH_AF, pageRowCount);

            // 明細ｴﾘｱのﾁｪﾝｼﾞｲﾍﾞﾝﾄで画面変更ﾌﾗｸﾞON
            setEventForEditFlg(true, null, "#" + P_formTopId);
            setEventForEditFlg(true, null, "#" + P_formDetailId);
            setEventForEditFlg(true, null, "#" + P_formBottomId);
        },
        // 2つめは通信失敗時のコールバック
        function (resultInfo) {
            //異常時

            //[0]:処理ステータス - CommonProcReturn
            var result = resultInfo.responseJSON;
            var status;
            if (result.length > 1) {
                status = result[0];
                var data = separateDicReturn(result[1], conductId);

                //エラー詳細を表示
                dispErrorDetail(data);
            }
            else {
                status = result;
            }

            //確認ﾒｯｾｰｼﾞは無効
            if (status.STATUS == procStatus.Confirm) {
                status.STATUS == procStatus.Error;
            }

            //処理結果ｽﾃｰﾀｽを画面状態に反映
            if (!setReturnStatus(appPath, status, null)) {
                return false;
            }

        }
    ).always(
        //通信の完了時に必ず実行される
        function (resultInfo) {
            //処理中メッセージ：off
            processMessage(false);
        });

}

/**
 *  ページデータ取得処理（検索用）
 *  @param {string} appPath ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string} ：検索ボタンCtrlId
 *  @param {string} ：機能ID
 *  @param {string} ：プログラムID
 *  @param {number} ：画面番号
 *  @param {number} ：処理ﾊﾟﾀｰﾝ
 *  @param {number} ：確認ﾒｯｾｰｼﾞ番号
 *  @param {boolean} saveDetailCondition     ：詳細検索条件保存フラグ
 *  @param {boolean} saveTreeMenuCondition   ：左側ツリーメニュー条件保存フラグ
 */
function getPageData(appPath, btn, btnCtrlId, conductId, pgmId, formNo, conductPtn, confirmNo, saveDetailCondition, saveTreeMenuCondition) {

    //処理中メッセージ：on
    processMessage(true);
    dispLoading();

    if (confirmNo == null || confirmNo < 0) {
        confirmNo = 0;
    }

    // 検索条件取得
    var conditionDataList = [];
    var tblSearch = getConditionTable();            //条件一覧要素
    conditionDataList = getConditionData(formNo, tblSearch);   //条件ﾃﾞｰﾀ

    //// 検索時の条件を退避
    //setSearchCondition(conductId, Number(formNo), conditionDataList);

    // 詳細検索条件データ取得(チェックされた条件のみ)
    var detailConditionDataList = getDetailConditionData(conductId, formNo);
    if (detailConditionDataList != null && detailConditionDataList.length > 0) {
        if (conditionDataList == null) {
            conditionDataList = [];
        }
        conditionDataList = conditionDataList.concat(detailConditionDataList);
    }

    // 103(Tabulator)一覧の場合、読込件数上限をセット
    var listDefines = $.grep(P_listDefines, function (define, idx) {
        return (define.CTRLTYPE == ctrlTypeDef.IchiranPtn3);
    });
    $.each(listDefines, function (idx, define) {
        // 読込件数上限を取得＆設定
        var selectCnt = getSelectCntMax(define.CTRLID, formNo);
        var detailCondition = $.grep(detailConditionDataList, function (condition, idx2) {
            return (condition.CTRLID == define.CTRLID);
        });
        var select = getSelectCntCombo(define.CTRLID);
        if (detailCondition != null && detailCondition.length > 0) {
            // 詳細検索条件指定有りの場合、読込件数を「すべて」に更新
            selectCnt = selectCntMaxDef.All;
            $(select).find('option:selected').prop('selected', false);
            $(select).find('option[exparam1="' + selectCnt + '"]').prop('selected', true);
        }
        define.VAL5 = selectCnt;
        // 各テーブルの列フィルターの入力クリア
        P_listData["#" + define.CTRLID + '_' + getFormNo()].clearHeaderFilter();
    });
    // フィルターの入力クリア
    P_Article.find('input[data-childno="FILTER"]').closest("td").find('input[data-type="codeTrans"]').val("");

    if (saveDetailCondition) {
        // チェック状態を含めた条件データをLocalStorageへ保存
        detailConditionDataList = getDetailConditionData(conductId, formNo, null, false, true);
        if (detailConditionDataList != null && detailConditionDataList.length > 0) {
            $.each(detailConditionDataList, function (idx, condition) {
                setSaveDataToLocalStorage(detailConditionDataList, localStorageCode.DetailSearch, conductId, formNo, condition.CTRLID);
            });
        }
    }

    // 場所階層/職種機種データ取得
    var locationIdList = getSelectedStructureIdList(structureGroupDef.Location, treeViewDef.TreeMenu, false);
    var jobIdList = getSelectedStructureIdList(structureGroupDef.Job, treeViewDef.TreeMenu, true);
    if (saveTreeMenuCondition) {
        // LocalStorageへ保存
        //場所階層
        setSaveDataToLocalStorage(locationIdList, localStorageCode.LocationTree);
        //職種機種
        setSaveDataToLocalStorage(jobIdList, localStorageCode.JobTree);
    }

    /* ボタン権限制御 切替 start ================================================ */
    //var btnDefines = P_buttonDefine;
    /*  ================================================ */
    var btnDefines = P_buttonDefine[conductId];
    /* ボタン権限制御 切替 end ================================================== */

    // ﾍﾟｰｼﾞの状態を初期状態に設定
    var pageRowCount = 0;
    setPageStatus(pageStatus.INIT, pageRowCount, conductPtn);

    // POSTデータを生成
    var postdata = {
        conductId: conductId,                   // メニューの機能ID
        pgmId: pgmId,                           // メニューのプログラムID
        formNo: formNo,                         // 画面番号
        ctrlId: btnCtrlId,                      // 検索ボタンの画面定義のコントロールID
        conditionData: conditionDataList,           // 検索条件入力データ
        listDefines: P_listDefines,               // 一覧定義情報
        pageNo: 1,                              // ページ番号
        ListIndividual: P_dicIndividual,   // 個別実装用汎用ﾘｽﾄ

        confirmNo: confirmNo,                   //確認ﾒｯｾｰｼﾞ番号
        buttonDefines: btnDefines,          //ﾎﾞﾀﾝ権限情報　※ﾎﾞﾀﾝｽﾃｰﾀｽを取得

        browserTabNo: P_BrowserTabNo,           // ブラウザタブ識別番号

        locationIdList: locationIdList,         // 場所階層構成IDリスト
        jobIdList: jobIdList,     // 職種機種構成IDリスト
    };

    // 検索処理実行
    $.ajax({
        url: appPath + 'api/CommonProcApi/' + actionkbn.Search,    // 検索
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(postdata),
        headers: { 'X-XSRF-TOKEN': getRequestVerificationToken() },
        traditional: true,
        cache: false
    }).then(
        // 1つめは通信成功時のコールバック
        function (resultInfo) {
            var status = resultInfo[0];                     //[0]:処理ステータス - CommonProcReturn
            var data = separateDicReturn(resultInfo[1], conductId);    //[1]:結果データ - Dictionary<string, object>※結果ﾃﾞｰﾀ："Result"、個別実装用ﾃﾞｰﾀ："Individual"
            /* ボタン権限制御 切替 start ================================================ */
            //var authShori = resultInfo[2];                  //[2]:処理ｽﾃｰﾀｽ  - IList<{ﾎﾞﾀﾝCTRLID：ｽﾃｰﾀｽ}> ※ｽﾃｰﾀｽ：0:非表示、1(表示(活性))、2(表示(非活性))
            /* ボタン権限制御 切替 end ================================================== */

            // メッセージをクリア
            clearMessage();

            //処理メッセージを表示
            if (status.MESSAGE != null && status.MESSAGE.length > 0) {
                //正常時、正常ﾒｯｾｰｼﾞ
                //警告、異常時、ｴﾗｰﾒｯｾｰｼﾞ
                addMessage(status.MESSAGE, status.STATUS);
            }

            // 画面変更ﾌﾗｸﾞ初期化
            dataEditedFlg = false;
            // ｲﾍﾞﾝﾄを一旦解除
            setEventForEditFlg(false, null, "#" + P_formTopId);
            setEventForEditFlg(false, null, "#" + P_formDetailId);
            setEventForEditFlg(false, null, "#" + P_formBottomId);

            //検索結果ﾃﾞｰﾀを明細一覧に表示
            pageRowCount = dispListData(appPath, conductId, pgmId, formNo, data, true);

            //ﾍﾟｰｼﾞの状態を検索後に設定
            setPageStatus(pageStatus.SEARCH, pageRowCount, conductPtn);
            //ﾎﾞﾀﾝｽﾃｰﾀｽ設定
            /* ボタン権限制御 切替 start ================================================ */
            //setButtonStatus(authShori);
            setButtonStatus();
            /* ボタン権限制御 切替 end ================================================== */

            if (data && data.length > 0) {
                // 103(Tabulator)一覧を抽出
                var listDefines = $.grep(P_listDefines, function (define, idx) {
                    return (define.CTRLTYPE == ctrlTypeDef.IchiranPtn3);
                });
                $.each(listDefines, function (idx, define) {
                    // 詳細条件適用有無を取得
                    var isDetailConditionApplied = $.grep(data, function (info, idx2) {
                        return (info.CTRLID == define.CTRLID && info.IsDetailConditionApplied);
                    });
                    var selectCntDisabled = isDetailConditionApplied != null && isDetailConditionApplied.length > 0;
                    // 表示件数コンボの活性状態を設定
                    setSelectCntControlStatus(define.CTRLID, selectCntDisabled);
                });
            }

            // 列固定
            setFixColCss();

            // 明細ｴﾘｱのﾁｪﾝｼﾞｲﾍﾞﾝﾄで画面変更ﾌﾗｸﾞON
            setEventForEditFlg(true, null, "#" + P_formTopId);
            setEventForEditFlg(true, null, "#" + P_formDetailId);
            setEventForEditFlg(true, null, "#" + P_formBottomId);

            // tabindexを制御
            nextFocus();

            //console.time('outputList');

            // 【オーバーライド関数】ページデータ取得後
            postGetPageData(appPath, btn, conductId, pgmId, formNo);
        },
        // 2つめは通信失敗時のコールバック
        function (resultInfo) {
            // 検索処理失敗

            //処理結果ｽﾃｰﾀｽ(CommonProcReturn)
            var result = resultInfo.responseJSON;
            var status;
            if (result.length > 1) {
                status = result[0];
                var data = separateDicReturn(result[1], conductId);

                //エラー詳細を表示
                dispErrorDetail(data);
            }
            else {
                status = result;
            }

            //処理継続用ｺｰﾙﾊﾞｯｸ関数呼出文字列を生成(@confirmNoは共通関数内で埋める)
            var confirmNo = 0;
            if (!isNaN(status.LOGNO)) {
                confirmNo = parseInt(status.LOGNO, 10);
            }
            var eventFunc = function () {
                getPageData(appPath, btn, btnCtrlId, conductId, pgmId, formNo, conductPtn, confirmNo, saveDetailCondition, saveTreeMenuCondition);
            }

            //処理結果ｽﾃｰﾀｽを画面状態に反映
            if (!setReturnStatus(appPath, status, eventFunc)) {
                return false;
            }

        }
    ).always(
        //通信の完了時に必ず実行される
        function (resultInfo) {
            //処理中メッセージ：off
            processMessage(false);
            // 実行中フラグOFF
            P_ProcExecuting = false;
            // ボタンを活性化
            $(btn).prop("disabled", false);
        });
}

/**
 *  検索結果ﾃﾞｰﾀを一覧に表示
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string} ：機能ID
 *  @param {string} ：ﾌﾟﾛｸﾞﾗﾑID
 *  @param {object} ：検索結果ﾃﾞｰﾀ List<Dictionary<string, object>>
 *  @param {boolean}：検索時:true/ ﾍﾟｰｼﾞ切替時:false
 */
function dispListData(appPath, conductId, pgmId, formNo, data, isSearch) {

    var ctrlId = "";
    var pageNo = "";    //ﾍﾟｰｼﾞｬｰのﾍﾟｰｼﾞ番号
    var tbl = null;
    var editPtn = "";
    var transPtn = "";
    var layoutTr = null;
    var baseTr = null;
    var cloneTr = null;
    var totalCount = 0;
    var pageRowCount = 0;   //ページ全体のデータ行数
    var isDetail = false;   //明細ｴﾘｱﾌﾗｸﾞ
    var data_ctrlId = null;
    var isBatStatus = false;  // ﾊﾞｯﾁｽﾃｰﾀｽﾌﾗｸﾞ

    // コントロール初期化用
    var dPickers = [];
    var tPickers = [];
    var dtPickers = [];
    var ymPickers = [];
    var yyyyTexts = [];
    var atComps = [];
    var cdTrns = [];

    //ｴﾘｱ退避用
    var areaId = "";
    var areas = [];

    if (data == null) {
        return 0;   //ﾃﾞｰﾀなし
    }
    var maxidx = 0;
    // 連想配列をkeyでグループ化
    var groupByData = function (data, key) {
        return data.reduce(function (rv, x) {
            (rv[x[key]] = rv[x[key]] || []).push(x);
            return rv;
        }, {});
    };

    var ctrlIdList = [];
    // CTRLIDでグルーピングし、各一覧が103型一覧の場合はTabulatorを使用
    $.each(groupByData(data, 'CTRLID'), function (key, info) {
        if (key == null || key == '') { return true; }
        var tabulatorHeader = $('.' + key + '.tabulatorHeader');
        if (tabulatorHeader != null && tabulatorHeader.length > 0) {
            ctrlIdList.push(key);
            pageRowCount = info.length - 1;
            dispTabulatorListData(appPath, conductId, pgmId, formNo, info, tabulatorHeader);
        }
    });

    $.each(data, function (idx, info) {
        if (ctrlIdList.indexOf(info.CTRLID) >= 0) {
            //var tabulator = $(P_Article).find(".tabulator[data-ctrlid='" + info.CTRLID + "']");
            //var id = $(tabulator).attr("id");
            ////表示件数
            //pageRowCount = P_listData["#" + id].getDataCount("display");
            return true;    //continue;
        }
        // 検索結果データを一覧毎に表示
        if (ctrlId != info.CTRLID) {
            //※ﾙｰﾌﾟの一覧が切り替わった時
            maxidx = 0;
            if (ctrlId.length > 0) {
                //※2つ目以降の一覧の時
                if (tbl != null && true == $(tbl).hasClass('vertical_tbl')) {
                    //※縦方向一覧の場合
                    // 取得ﾃﾞｰﾀを画面一覧に反映
                    dispDataVertical(appPath, data_ctrlId, formNo, false);
                    if (editPtn == editPtnDef.Input) {
                        // 編集ﾊﾟﾀｰﾝ：「直接入力」の場合
                        //連動ｺﾝﾎﾞの選択ﾘｽﾄを再生成
                        var selects = $(tbl).find('tbody td select.dynamic');
                        resetComboBox(appPath, selects);
                        selects = null;
                    }
                }
                ////明細入力ﾊﾟﾀｰﾝの場合、ｺﾝﾎﾞﾎﾞｯｸｽを翻訳表示
                dispHonyakuTable(tbl, editPtn, transPtn, layoutTr);

                //指定ﾍﾟｰｼﾞNOをｾｯﾄ
                setCurPageNo(ctrlId, pageNo);

                //if (isSearch) {
                //    // 1つ前のﾃｰﾌﾞﾙ（一覧）のﾍﾟｰｼﾞｬｰｾｯﾄｱｯﾌﾟ
                //    setupPagination(appPath, conductId, pgmId, formNo, ctrlId, totalCount);
                //}
                // 1つ前のﾃｰﾌﾞﾙ（一覧）のﾍﾟｰｼﾞｬｰｾｯﾄｱｯﾌﾟ
                setupPagination(appPath, conductId, pgmId, formNo, ctrlId, totalCount);
            }

            ctrlId = info.CTRLID;
            pageNo = info.PAGENO;

            //※ptn=10のﾊﾞｯﾁ機能の場合
            if (P_isBat) {
                tbl = $(P_Article).find("table#" + ctrlId + "_J");
                if ($(tbl).length) {
                    isBatStatus = true; //ﾊﾞｯﾁｽﾃｰﾀｽﾌﾗｸﾞ
                    return true;    //continue;
                }
            }

            totalCount = info.VAL1;

            // 結果一覧の1行目のレイアウトを取得
            tbl = $(P_Article).find("#" + ctrlId + "_" + formNo);

            // 詳細検索条件適用状況の設定
            setConditionAppliedStatus(tbl, info);

            if (totalCount == 0) {
                return true;
            }

            //ｴﾘｱのidを退避
            areaId = $(tbl).closest("form").attr("id");
            if (areas.indexOf(areaId) < 0) {
                areas.push(areaId);
            }

            if ($(tbl).closest("form[id^='formDetail']").length > 0) {
                //※明細エリアの一覧
                isDetail = true;
            }

            if (tbl != null) {
                editPtn = $(tbl).data("editptn");
                transPtn = $(tbl).data("transptn");

                if (true == $(tbl).hasClass('vertical_tbl')) {
                    //※縦方向一覧の場合
                    data_ctrlId = [];
                }
                else {
                    //※横方向一覧の場合

                    //==一覧ヘッダ行の設定==
                    //NOリンクの幅を調整する
                    adjustNoLink(tbl, totalCount);

                    //==一覧レイアウト行の設定==
                    layoutTr = $(tbl).find(".base_tr");

                    baseTr = $(layoutTr).clone(true);
                    if (editPtn == editPtnDef.Input) {
                        // 編集ﾊﾟﾀｰﾝ：「直接入力」の場合
                    }
                    else {
                        // ﾗﾍﾞﾙ表示用spanタグに表示を切替※NOﾘﾝｸと選択ﾁｪｯｸﾎﾞｯｸｽ以外
                        var baseTds = $(baseTr).find("td:not([data-name='SELTAG'],[data-name='ROWNO']) span.labeling").closest("td");
                        if ($(baseTds).length) {
                            $(baseTds).addClass("readonly");
                        }
                    }
                    $(baseTr).hide();
                    $(tbl).show();
                }
            }
            return true;    // ※continueに相当する制御
        }

        if (true == isBatStatus) {
            //※ﾊﾞｯﾁｽﾃｰﾀｽの場合
            isBatStatus = false;    //ﾊﾞｯﾁｽﾃｰﾀｽﾌﾗｸﾞを戻す
            dispBatStatusDataDetail(info, ctrlId);  //ﾊﾞｯﾁ実行結果ﾃﾞｰﾀをﾊﾞｯﾁｽﾃｰﾀｽ一覧に表示(共通部分)
            return true;    //continue;
        }

        if (true == $(tbl).hasClass('vertical_tbl')) {
            //※縦方向一覧の場合            
            data_ctrlId.push(info); //ﾙｰﾌﾟが次の一覧に移ったﾀｲﾐﾝｸﾞでﾃﾞｰﾀをｾｯﾄするので、ﾃﾞｰﾀを退避しておく            
            return true;    // ※continueに相当する制御
        }

        if (tbl != null && baseTr != null) {
            if (info.DATATYPE == dataTypeDef.ColCss) {
                //列スタイルの場合

                $.each(info, function (key, value) {
                    if (value != null && value.length > 0) {
                        // データ項目の列にスタイルをセット
                        if (key.substr(0, 3) === "VAL") {
                            //列タグ取得
                            var td = $(cloneTr).find('td[data-name="' + key + '"]');
                            if (td != null && td.length > 0) {
                                //スタイルセット
                                $(td).addClass(value);
                            }
                        }
                    }
                });
            }
            else if (info.DATATYPE == dataTypeDef.RowCss) {
                //行スタイルを設定
                $(cloneTr).addClass(info.VAL1);
            }
            else {
                // レイアウト行をコピーして検索結果をセット
                cloneTr = dispDataHorizontal(appPath, transPtn, info, baseTr, maxidx);
                if (editPtn == editPtnDef.Input) {
                    // 編集ﾊﾟﾀｰﾝ：「直接入力」の場合
                    //入力ﾊﾟﾀｰﾝの場合、連動ｺﾝﾎﾞの選択ﾘｽﾄを再生成
                    var selects = $(cloneTr).find('select.dynamic');
                    resetComboBox(appPath, selects);
                    selects = null;
                }
                $(cloneTr).show();
                $(cloneTr).appendTo($(tbl));
                if ($(cloneTr).data("rowstatus") + "" == rowStatusDef.Delete) {
                    $(cloneTr).css('display', 'none');
                }

                // テーブルにMAXのidxをｾｯﾄ
                setAttrByNativeJs(tbl, "data-maxidx", maxidx + "");
                maxidx++;

                // 追加行にAKAP標準の日付、時刻、日時コントロールがあればIDを退避
                var datePickers = $(cloneTr).find(":text[data-type='date']");
                $(datePickers).each(function (index, element) {
                    dPickers.push(element.id);
                });
                var timePickers = $(cloneTr).find(":text[data-type='time']");
                $(timePickers).each(function (index, element) {
                    tPickers.push(element.id);
                });
                var datetimePickers = $(cloneTr).find(":text[data-type='datetime']");
                $(datetimePickers).each(function (index, element) {
                    dtPickers.push(element.id);
                });
                var yearmonthPickers = $(cloneTr).find(":text[data-type='yearmonth']");
                $(yearmonthPickers).each(function (index, element) {
                    ymPickers.push(element.id);
                });
                var yearTexts = $(cloneTr).find(YearText.selector);
                $(yearTexts).each(function (index, element) {
                    yyyyTexts.push(element.id);
                });
                // 追加行にオートコンプリートがあればIDを退避
                var autoComps = $(cloneTr).find(":text[data-type='text'], textarea[data-type='textarea'], :text[data-type='codeTrans']");
                $(autoComps).each(function (index, element) {
                    if ($(element).data("autocompdiv") != autocompDivDef.None) {
                        atComps.push(element.id);
                    }
                });
                // 追加行にチェックボックスがあれば初期化
                var checkboxs = $(cloneTr).find(":checkbox");
                $(checkboxs).each(function (index, element) {
                    initCheckBox(element);
                });
                // 追加行にコード＋翻訳があればIDを退避
                var inputs = $(cloneTr).find(":text[data-type='codeTrans']");
                $.each(inputs, function (index, element) {
                    cdTrns.push(element.id);
                });

                if (isDetail) {
                    pageRowCount = pageRowCount + 1;
                }

                //参照モードの入力コントロール制御
                var refTbl = $(tbl).parent().find("[data-referencemode='" + referenceModeKbnDef.Reference + "']");
                if ($(refTbl).length) {
                    // 参照モードの場合、入力コントロールをﾗﾍﾞﾙ表示
                    var refTds = $(cloneTr).find('td[data-name^="VAL"]').closest('td');
                    $(refTds).addClass("reference_mode");
                }
            }
        }
    });

    // 追加したdatepicker系の日付、時刻、日時コントロールの初期化
    $(dPickers).each(function (index, dpId) {
        initDatePicker('#' + dpId, false);
    });
    $(tPickers).each(function (index, dpId) {
        initTimePicker('#' + dpId, false);
    });
    $(dtPickers).each(function (index, dpId) {
        initDateTimePicker('#' + dpId, false);
    });
    $(ymPickers).each(function (index, dpId) {
        initYearMonthPicker('#' + dpId, false);
    });
    $(yyyyTexts).each(function (index, dpId) {
        initYearText('#' + dpId, false);
    });
    // 追加したオートコンプリートの初期化
    $(atComps).each(function (index, dpId) {
        $.each(P_autocompDefines, function (idx, define) {
            if (('#' + dpId).indexOf(define.key) === 0) {
                var sqlId = define.sqlId;
                var param = define.param;
                var div = define.division;
                var option = define.option;
                initAutoComp(appPath, '#' + dpId, sqlId, param, null, div, option);
                return false;
            }
        });
    });
    // 追加したコード＋翻訳
    $(cdTrns).each(function (index, dpId) {
        // 入力値がない場合、エラーで後続処理ができない対応
        if ($('#' + dpId).val().length != "") {
            // 翻訳セットのためチェンジイベントを発火(編集フラグは変更しない)
            $('#' + dpId).trigger('change', [true]);
        }
    });

    if (ctrlId && ctrlId.length > 0) {

        if (tbl != null && true == $(tbl).hasClass('vertical_tbl')) {
            //※ﾙｰﾌﾟの最後が縦方向一覧の場合、このﾀｲﾐﾝｸﾞでﾃﾞｰﾀをｾｯﾄ
            dispDataVertical(appPath, data_ctrlId, formNo, false);
            if (editPtn == editPtnDef.Input) {
                //※編集ﾊﾟﾀｰﾝ：「直接入力」の場合
                //入力ﾊﾟﾀｰﾝの場合、連動ｺﾝﾎﾞの選択ﾘｽﾄを再生成
                var selects = $(tbl).find('tbody td select.dynamic');
                resetComboBox(appPath, selects);
                selects = null;
            }
        }
        //明細入力ﾊﾟﾀｰﾝの場合、ｺﾝﾎﾞﾎﾞｯｸｽを翻訳表示
        dispHonyakuTable(tbl, editPtn, transPtn, layoutTr);

        //指定ﾍﾟｰｼﾞNOをｾｯﾄ
        setCurPageNo(ctrlId, pageNo);

        //if (isSearch) {
        //    // 1つ前のﾃｰﾌﾞﾙ（一覧）のﾍﾟｰｼﾞｬｰｾｯﾄｱｯﾌﾟ
        //    setupPagination(appPath, conductId, pgmId, formNo, ctrlId, totalCount);
        //}
        // 1つ前のﾃｰﾌﾞﾙ（一覧）のﾍﾟｰｼﾞｬｰｾｯﾄｱｯﾌﾟ
        setupPagination(appPath, conductId, pgmId, formNo, ctrlId, totalCount);
    }

    //各ｴﾘｱ毎に行う初期化処理
    $.each(areas, function (idx, area) {

        //日付系(datepicker系)初期化（一覧再表示用）
        initDateTypePicker($(P_Article).find("#" + area + " table tbody tr:not([class^='base_tr'])"), false);

        //入力ﾌｫｰﾏｯﾄ設定処理
        initFormat("#" + area);
        // Validator初期化
        initValidator("#" + area);
    });

    //データが設定されていないテーブルが参照モードの場合の入力コントロール制御
    var tbls = $(P_Article).find("[data-referencemode='" + referenceModeKbnDef.Reference + "']:not([id$='_tablebase'])");
    $.each(tbls, function (idx, ele) {
        // 参照モードの場合、入力コントロールをﾗﾍﾞﾙ表示
        var tds = $(ele).find('td[data-name^="VAL"]:not(.hide, .reference_mode)').closest('td');
        $(tds).addClass("reference_mode");
    });

    return pageRowCount;
}

/*
* 非表示かどうか判定する処理
* @param elm : 判定を行う要素
*/
function isInvisible(elm) {
    if (true == $(elm).hasClass("hide") || $(elm).css("display") == "none") {
        return true;
    }
    return false;
}

/**
 * 列固定列のCSS設定
 */
function setFixColCss() {

    var ctrlIds = $(P_Article).find(".ctrlId");
    $.each($(ctrlIds), function (idx, ctrlId) {
        // ﾍｯﾀﾞ固定ﾌﾗｸﾞ
        var isHeaderFix = $(ctrlId).closest(".ctrlId_parent").hasClass("headerFix");
        // ﾍｯﾀﾞ行
        var colsH = $(ctrlId).find("thead tr:not([class='base_tr_width']) th");
        $.each($(colsH), function (idx, colH) {
            if (false == isHeaderFix && false == $(colH).hasClass("fixCol")) {
                return true;    //continue;
            }

            if (false == isHeaderFix && isInvisible(colH)) {
                // 非表示列の場合、固定を行わない
                return true;
            }

            var classCss = {};
            var zIndex = 0;
            var position = $(colH).position();  //親要素に対する位置情報
            var strTop = position.top;          //項目ﾄｯﾌﾟ位置
            strTop = strTop + "px";
            var strSumWidth = position.left;    //項目左位置


            classCss = {
                "position": "sticky",
                "top": strTop,
            }
            if (isHeaderFix) {
                zIndex += 1;
            }
            if ($(colH).hasClass("fixCol")) {  // 列固定の対象？
                zIndex += 1;
                strSumWidth = strSumWidth + "px";
                classCss["left"] = strSumWidth;
            }
            classCss["z-index"] = zIndex;
            $(colH).css(classCss);
        });

        // ﾃﾞｰﾀ行
        setFixColCssForDataRow(ctrlId);
    });
}
/**
 * 列固定列のCSS設定_ﾃﾞｰﾀ行
 * @param {table} : 横方向一覧table
 */
function setFixColCssForDataRow(ctrlId) {

    var fixColsD = $(ctrlId).find("tbody tr.base_tr td.fixCol");
    if ($(fixColsD).length) {
        $.each($(fixColsD), function (idx, fixCol) {
            var fixColNo = idx + 1;
            var className = "fixCol_" + fixColNo + "";
            var classCel = $(ctrlId).find("tbody td." + className);
            if ($(classCel).length > 1) {
                if (isInvisible(cel)) {
                    // 非表示列の場合、固定を行わない
                    return true;
                }

                // ﾚｲｱｳﾄ行のみの場合、座標を取得できない
                var cel = $(classCel)[1];
                var position = $(cel).position();
                var strSumWidth = position.left;
                strSumWidth = strSumWidth + "px";

                var classCss = {
                    "position": "sticky",
                    "left": strSumWidth,
                    "z-index": "1"
                }
                $(classCel).css(classCss);
            }
        });
    }
}

/**
 *  エラー詳細表示
 *  @param {object} ：エラー詳細ﾃﾞｰﾀ List<Dictionary<string, object>]>
 */
function dispErrorDetail(data, isModal) {

    if (data == null) {
        return;   //ﾃﾞｰﾀなし
    }
    var formNo = $(P_Article).data("formno");   //画面NO

    // ﾀﾌﾞﾎﾞﾀﾝが存在する場合、一時的に全ﾀﾌﾞ表示して入力ﾁｪｯｸを行う。
    var tabNo = 0;    //選択ﾀﾌﾞ番号

    var tabContents = $(P_Article).find(".tab_contents");
    if (tabContents != null && tabContents.length > 0) {
        $.each($(tabContents), function (i, div) {
            //表示状態ﾁｪｯｸ
            if ($(div).hasClass('selected')) {
                tabNo = $(div).data('tabno');
            }
            else {
                //一時的に表示状態とする
                $(div).addClass('selected');
            }
        });
    }
    // validationのため、畳んだ一覧は展開する
    var switchids = $(P_Article).find("a[data-switchid]");
    var isHiddens = {};
    $.each(switchids, function (idx, switchid) {
        var id = $(switchid).data("switchid") + "_" + formNo;
        var isHidden = isHideId(id);
        isHiddens[id] = isHidden;
        if (isHidden) {
            //表示/非表示切替
            setHideId(id, !isHidden);
        }
    });

    $.each(data, function (idx, info) {
        //ｴﾗｰ詳細以外はｽｷｯﾌﾟ
        if (info["DATATYPE"] != dataTypeDef.ErrorDetail) {
            return true;    // continue
        }
        // 対象の一覧を取得
        var ctrlId = info["CTRLID"];
        var target;
        if (isModal) {
            target = $(P_Article).find("#" + ctrlId + "_" + formNo + "_edit");   //ctrlid単位
        }
        else {
            target = $(P_Article).find("#" + ctrlId + "_" + formNo);   //ctrlid単位
            if ($(target).length == 0 && $(P_Article).closest("section").hasClass("modal")) {
                // モーダル表示時エラー対応
                // モーダルでない場合、表示中のフォームがモーダルかどうかをsectionのmodalクラスの有無によって判定、有ればモーダルとして扱う
                target = $(P_Article).find("#" + ctrlId + "_" + formNo + "_edit");
            }
        }

        var displayType = "block";
        var isTabulatorHorizon = false; // Tabulatorの横方向一覧の場合コントロール取得のセレクタが異なるので場合分け用のフラグ
        if (false == target.hasClass('vertical_tbl')) {
            // 新規行かつ複数行の場合、1行目(※lockDataを保持しているtr)のみエラーが表示されていたので条件を外してみる
            //// 横方向一覧は1行
            //if (info["ROWSTATUS"] == rowStatusDef.New) {
            //    // 新規行の場合
            //    var lockDataIdx = info["lockData"];
            //    var lockDatas = $(target).find("td[data-name='lockData']:contains('" + lockDataIdx + "')");
            //    var tmpTargets = [];
            //    $.each(lockDatas, function (i, lockData) {
            //        if ($(lockData).text() == lockDataIdx) {
            //            target = $(lockData).closest("tr");
            //            return false;
            //        }
            //    })
            //    target = tmpTargets;
            //}
            //else {
            //    var rowno = info["ROWNO"];
            //    target = $(target).find("tr[data-rowno='" + rowno + "']");
            //}
            var rowno = info["ROWNO"];

            if ($(target).data("ctrltype") == ctrlTypeDef.IchiranPtn3) {
                // Tabulatorの場合
                target = $(target).find("div.tabulator-table").find("div.tabulator-row")[rowno - 1];
                isTabulatorHorizon = true;
            } else {
                target = $(target).find("tr[data-rowno='" + rowno + "']");
            }
        }

        for (var key in info) {
            if (key.indexOf("VAL") < 0) {
                //VAL～以外はｽﾙｰ
                continue;
            }
            var value = info[key];    // ｴﾗｰﾒｯｾｰｼﾞ
            var td = $(target).find("td[data-name='" + key + "'] .substance"); // td要素直下の入力ｺﾝﾄﾛｰﾙ格納div
            if (isTabulatorHorizon) {
                // Tabulatorの場合
                td = $(target).find("div[tabulator-field='" + key + "'] .substance");
            }
            var element = $(td).find("[id][name]"); // ｴﾗｰ項目

            if (td.length == 0) {
                td = $(target).find("td[data-name='" + key + "']");
                if (isTabulatorHorizon) {
                    // Tabulatorの場合
                    td = $(target).find("div[tabulator-field='" + key + "']");
                }
                element = $(td).find("[name]"); // ｴﾗｰ項目
            }

            var message = value;

            // fromto項目
            if ($(td).find(".fromto").length) {

                // from項目のｴﾗｰ
                if (value["From"]) {
                    element = $(td).find(".fromto[id][name]:first");
                    message = value["From"];
                    // ｴﾗｰ追加(To項目ﾌﾗｸﾞfalse)
                    dispErrorDetailDetail(element, message, displayType, false);
                }
                // to項目のｴﾗｰ
                if (value["To"]) {
                    element = $(td).find(".fromto[id][name]:last");
                    message = value["To"];
                    // ｴﾗｰ追加(To項目ﾌﾗｸﾞtrue)
                    dispErrorDetailDetail(element, message, displayType, true);
                }
            }
            else {
                // ｴﾗｰ追加(To項目ﾌﾗｸﾞfalse)
                dispErrorDetailDetail(element, message, displayType, false);
            }
        }
    });
    // ｴﾗｰがない一覧の折り畳み状態を元に戻す
    $.each(switchids, function (idx, switchid) {
        var id = $(switchid).data("switchid") + "_" + formNo;
        if (!($("#" + id).find(".errorcom").length)) {
            if (isHiddens[id]) {
                //表示/非表示切替
                setHideId(id, isHiddens[id]);
            }
        }
    });

    //ﾀﾌﾞの表示状態をもとに戻す/もしくは、ｴﾗｰﾀﾌﾞを表示
    if (tabContents != null && tabContents.length > 0) {
        //ｴﾗｰﾀﾌﾞの検索(※選択中のﾀﾌﾞが優先)
        var error_tabNo = null;
        $.each($(tabContents), function (i, div) {
            //ｴﾗｰ存在ﾁｪｯｸ
            var errors = $(div).find(".errorcom");
            if (errors != null && errors.length > 0) {
                //ｴﾗｰが存在する場合
                //表示状態ﾁｪｯｸ
                if ($(div).data('tabno') == tabNo) {
                    //選択ﾀﾌﾞ、かつｴﾗｰありの場合、ｴﾗｰﾀﾌﾞ番号を上書き
                    error_tabNo = tabNo;
                }
                else {
                    //ｴﾗｰがある先頭ﾀﾌﾞを表示
                    if (error_tabNo == null) {
                        error_tabNo = $(div).data('tabno');
                    }
                }
            }

        });
        if (error_tabNo != null) {
            //選択ﾀﾌﾞをｴﾗｰﾀﾌﾞに変更
            tabNo = error_tabNo;
        }

        // 選択対象外のタブを非表示に戻す
        $.each($(tabContents), function (i, div) {
            if ($(div).data('tabno') != tabNo) {
                $(div).removeClass('selected');
            }
        });
    }
}
/**
 *  エラー詳細追加処理
 *  @param {要素} : ｴﾗｰ項目
 *  @param {string} : ｴﾗｰﾒｯｾｰｼﾞ
 *  @param {string} : ｴﾗｰﾒｯｾｰｼﾞ要素のstyle(display)
 *  @param {boolean} : To項目ﾌﾗｸﾞ true:FromToのTo項目
 */
function dispErrorDetailDetail(element, message, displayType, isTo) {
    //複数選択チェックボックスの場合、アイコンリンクにエラーを設定
    var muitiFlg = $(element).hasClass("multiSelect");
    if (muitiFlg) {
        var td = $(element).closest("td");
        element = $(td).find("a.multisel-text");
    }

    // ｴﾗｰ項目にｴﾗｰｸﾗｽを付与
    $(element).addClass("errorcom");
    $(element).closest("td").addClass("errorcom");

    var errTooltipDiv = $("div.errtooltip");

    var id = $(element).attr("id"); // ｴﾗｰ項目id
    var errorHtml = "<label for='" + id + "' class='errorcom' >" + message + "</label>"; // ｴﾗｰﾒｯｾｰｼﾞHTML

    // ｴﾗｰﾒｯｾｰｼﾞHTMLをｴﾗｰ項目直後に挿入
    //$(errorHtml).insertAfter(element);
    $(errorHtml).appendTo(errTooltipDiv);

    var error = $("label[for='" + id + "'][class='errorcom']"); // ｴﾗｰﾒｯｾｰｼﾞ要素

    // ｴﾗｰﾒｯｾｰｼﾞ要素を非表示
    $(error).hide();

    // ｴﾗｰ項目hoverでｴﾗｰ項目要素をﾌｪｰﾄﾞ
    $(element).hover(
        function () {
            var errorCss = resetErrorPosition(this);
            $("label[for='" + $(this).attr("id") + "']").css(errorCss);
            $("#main_contents").on('scroll.errTooltip', function () {
                $("label.errorcom").fadeOut('fast');
                $("#main_contents").off('scroll.errTooltip');
            });
        },
        function () {
            $("label[for='" + $(this).attr("id") + "']").fadeOut('fast');
            $("#main_contents").off('scroll.errTooltip');
        });

}

/**
 *  明細入力ﾊﾟﾀｰﾝの場合、ｺﾝﾎﾞﾎﾞｯｸｽを翻訳表示
 *  @param {table} ：一覧のtable要素
 *  @param {number} ：一覧の編集区分
 *  @param {tr} ：一覧のﾚｲｱｳﾄ行要素
 */
function dispHonyakuTable(tbl, editKbn, transKbn, layoutTr) {

    //ｺﾝﾎﾞﾎﾞｯｸｽの場合は翻訳を表示

    //ﾚｲｱｳﾄ行からﾁｪｯｸﾎﾞｯｸｽ、ｺﾝﾎﾞﾎﾞｯｸｽの列を取得
    var layoutTds = $(layoutTr).find("td[data-name^='VAL']");

    var layoutSelects = $(layoutTds).find('> select');

    if (layoutSelects.length > 0) {

        //ﾃﾞｰﾀ行を取得
        var trs = null;
        if (editKbn == editPtnDef.ReadOnly &&
            (transKbn == transPtnDef.DetailPopup || transKbn == transPtnDef.DetailUpdNone
                || transKbn == transPtnDef.ChildTrans || transKbn == transPtnDef.ChildPopup)) {
            //編集ﾊﾟﾀｰﾝ：「表示のみ」
            //画面遷移ﾊﾟﾀｰﾝ：「明細ﾎﾟｯﾌﾟｱｯﾌﾟ」、「子画面遷移、ﾎﾟｯﾌﾟｱｯﾌﾟ」の場合

            trs = getTrsData(tbl);
        }
        else {
            //表示のみ行で絞込み
            trs = $(tbl).find("tbody tr:not([class^='base_tr'])[data-rowstatus=0]");
        }

        // 明細ﾃﾞｰﾀﾘｽﾄを取得する
        $.each($(trs), function (i, tr) {

            //ｺﾝﾎﾞﾎﾞｯｸｽの翻訳
            $.each($(layoutSelects), function () {
                var key = $(this).parent().data("name");
                var td = $(tr).find("td[data-name='" + key + "']");

                //選択ﾘｽﾄの表示名で表示
                var val = $(td).text();
                //tdタグにコード値を保持
                setAttrByNativeJs(td, "data-value", val);
                var options = $(this).find("option[value='" + val + "']");
                if (options.length > 0) {
                    val = $(options).text();
                }

                $(td).text(val);
            });

        });
    }
}

/**
 *  明細ｴﾘｱの一覧について
 *    明細入力ﾊﾟﾀｰﾝの場合、ﾁｪｯｸﾎﾞｯｸｽ、ｺﾝﾎﾞﾎﾞｯｸｽを翻訳表示
 */
function dispHonyakuTableAll() {

    var tables = $(P_Article).find("form[id^='formDetail'] table");

    $.each(tables, function (i, tbl) {
        var ctrlId = $(tbl).attr("id");
        var editKbn = $(tbl).data("editkbn");
        var transKbn = $(tbl).data("transkbn");
        var layoutTr = $(tbl).find(".base_tr");

        dispHonyakuTable(tbl, editKbn, transKbn, layoutTr);
    });

}

/**
 *  対象列のｺﾝﾎﾞﾎﾞｯｸｽを翻訳表示
 *  @param {object} ：ｺﾝﾎﾞﾎﾞｯｸｽ選択ﾘｽﾄﾃﾞｰﾀ
 *  @param {select} ：ｺﾝﾎﾞﾎﾞｯｸｽ要素
 */
function dispHonyakuSelectCol(data, selector, appPath, sqlId, param) {
    //function dispHonyakuSelectCol(data, selector) {

    //ﾚｲｱｳﾄ行のselect要素から列番号を取得
    // selector:select > td
    //var key = $(selector).parent().data("name");    //VAL1～
    var key = $(selector).closest('td').data("name");    //VAL1～

    //ﾃﾞｰﾀ行を取得
    // selector:select > td > tr > tbody
    var trs = $(selector).closest('tbody').find("tr:not([class^='base_tr'])");
    $.each($(trs), function (i, tr) {

        var td = $(tr).find("td[data-name='" + key + "']");
        if (td.length <= 0) {
            return true;    //continue
        }
        //表示のみか？
        //var selects = $(td).find("> select");
        var selects = $(td).find("select");
        if (selects.length <= 0) {

            // ここでデータ再取得 @200331A
            if (data == null || data.length <= 0) {
                var factoryIdList = [];

                //SQLパラメータの成形(例：'C01','@3')
                var paramStr = param + '';
                var params = (param + '').split(',');   //ｶﾝﾏで分解
                var isDynamic = false;
                $.each(params, function () {
                    var colNo = -1;
                    var paramVal = "";

                    if (this.indexOf("@") >= 0) {
                        //列指定ﾊﾟﾗﾒｰﾀ（「@」で始まる引数）の場合

                        isDynamic = true;

                        //列番号を取得
                        colNo = parseInt(this.replace("'", "").replace("@", ""), 10);  //先頭の「@」を除去してcolNoとする

                        //対象列の値を取得
                        //入力項目の場合、変更時ｲﾍﾞﾝﾄ処理を付与
                        // ⇒ｺﾝﾎﾞﾎﾞｯｸｽ一覧を再作成する

                        //対象ｾﾙ
                        var td = getDataTd(tr, colNo);
                        var fromLocationTree = false;

                        // データ定義行数間連動対応 @200313A
                        if (!td || td.length == 0) {
                            // 対象セルが存在しない場合
                            if (colNo != colNoForFactory) {
                                var table = $(tr).closest('table');
                                if ($(table).hasClass("vertical_tbl")) {
                                    var trs = $(table).find("tr");
                                    if (trs.length > 0) {
                                        for (var i = 1; i <= trs.length; i++) {
                                            td = getDataTd(trs[i - 1], colNo);
                                            if (td.length > 0) break;
                                        }
                                    }
                                }
                                else {
                                    var ths = $(table).find("thead tr");
                                    var trs = $(table).find("tbody tr");
                                    var mltrowcnt = $(table).data("mltrowcnt");
                                    var rowIndex = $(tr).prop("rowIndex") + 1;
                                    if (mltrowcnt > 1 && trs.length > 0) {
                                        var dataIndex = Math.ceil((rowIndex - ths.length) / mltrowcnt);
                                        for (var i = 1; i <= mltrowcnt; i++) {
                                            td = getDataTd(trs[(mltrowcnt * (dataIndex - 1)) + i - 1], colNo);
                                            //td = getDataTd(trs[(mltrowcnt * (dataIndex - 1)) + i - 1], colNo);
                                            if (td.length > 0) break;
                                        }
                                    }
                                }
                            } else {
                                // 工場コントロール指定の場合、画面上から取得
                                // 同一行から取得する
                                td = getFactoryTdElement(selector, tr);
                                if (!td || td.length == 0) {
                                    // 同一画面上から取得する
                                    td = getFactoryTdElement(selector);
                                }
                                if (!td || td.length == 0) {
                                    // 同一画面上に工場コントロールがない場合、左側の場所階層メニューから取得
                                    fromLocationTree = true;
                                }
                            }
                        }
                        //ﾗﾍﾞﾙ
                        paramVal = $(td).text();

                        //SQLパラメータに現時点の値を設定
                        //※文字列指定でないﾊﾟﾗﾒｰﾀの場合、ﾊﾟﾗﾒｰﾀ値未設定の場合は「null」を設定
                        if (paramVal == null || paramVal.length <= 0) {
                            //※ﾊﾟﾗﾒｰﾀ値未設定の場合

                            //文字列指定でないﾊﾟﾗﾒｰﾀか？
                            var pos = paramStr.indexOf('@' + colNo);
                            if (pos == 0) {
                                paramVal = "null";
                            }
                            else if (pos > 0) {
                                //「@」直近文字列が「'」でないか？
                                if (paramStr.substr(pos - 1, 1) != "'") {
                                    paramVal = "null";
                                }

                            }

                        }
                        if (colNo != colNoForFactory) {
                            paramStr = paramStr.replace('@' + colNo, paramVal);    //「@3」⇒「ｾﾙの値」で置き換え
                        } else {
                            // 工場コントロールの場合、「factoryIdList」で工場IDを渡す
                            paramStr = paramStr.replace(',@' + colNo, '');  // 「,@9999」をパラメータ文字列から削除
                            if (!fromLocationTree && paramVal != "null") {
                                factoryIdList.push(paramVal);
                            }
                        }
                    }
                });

                //const paramKey = sqlId + "," + paramStr;
                const paramKey = sqlId + "_" + paramStr;
                // (2022/11/01) 構成マスタデータをセッションストレージには保存しない
                //var [requiredGetData, data] = getComboBoxDataFromSessionStorage(appPath, paramKey);
                var [requiredGetData, data] = getComboBoxLocalData(paramKey);

                // データ設定処理
                const eventFunc = function () {
                    if (data == null) { data = []; }

                    if (factoryIdList.length > 0) {
                        // 工場ID指定の場合、工場IDで絞り込み
                        data = $.grep(data, function (obj, index) {
                            return (factoryIdList.indexOf(obj.FactoryId) >= 0 || obj.FactoryId == 0);
                        });
                    }

                    //選択ﾘｽﾄの表示名で表示
                    var val = $(td).text();
                    $.each(data, function () {
                        if (this.VALUE1 == val) {
                            val = this.VALUE2;
                            return false;   //ﾙｰﾌﾟを抜ける
                        }
                    });
                    $(td).text(val);
                }

                if (data != null) {
                    // データ取得成功
                    // データ設定処理実行
                    eventFunc();

                } else if (!requiredGetData) {
                    // データ取得中
                    // データ取得中の場合、データ取得中リストから消えるまで待機
                    var timer = setInterval(function () {
                        if (P_GettingComboBoxDataList.indexOf(paramKey) < 0) {
                            // (2022/11/01) 構成マスタデータをセッションストレージには保存しない
                            //// データ取得中リストから消えた場合、セッションストレージから取得
                            //data = getSavedDataFromSessionStorage(sessionStorageCode.CboMasterData, paramKey);
                            // データ取得中リストから消えた場合、グローバル変数から取得
                            data = P_ComboBoxJsonList[paramKey];
                            // 待機を解除する
                            clearInterval(timer);
                            // データ設定処理実行
                            eventFunc();
                        }
                    }, 100);

                } else {
                    // データ取得が必要
                    // コンボデータ取得中リストに追加
                    P_GettingComboBoxDataList.push(paramKey);

                    // コンボデータ取得処理実行
                    var url = encodeURI(appPath + "api/CommonSqlKanriApi/" + sqlId + "?param=" + paramStr);// 手動URLエンコード
                    $.ajax({
                        url: url,
                        type: 'GET',
                        dataType: 'json',
                        contentType: 'application/json',
                        //data: JSON.stringify(postdata),
                        traditional: true,
                        cache: false
                    }).then(
                        // 1つめは通信成功時のコールバック
                        function (datas) {
                            //※正常時
                            //結果データ - Dictionary<string, object>※結果ﾃﾞｰﾀ："Result"、個別実装用ﾃﾞｰﾀ："Individual"
                            var data = separateDicReturn(datas);
                            // (2022/11/01) 構成マスタデータをセッションストレージには保存しない
                            //// セッションストレージへ保存
                            //setSaveDataToSessionStorage(data, sessionStorageCode.CboMasterData, paramKey)
                            P_ComboBoxJsonList[paramKey] = data;
                            // データ設定処理実行
                            eventFunc();
                        },
                        // 2つめは通信失敗時のコールバック
                        function (info) {
                        }
                    );
                }
            }
        }

    });

}

/**
 *  連動ｺﾝﾎﾞの選択ﾘｽﾄを再生成
 *  @param {string} appPath                 ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {Array.<Element>} selects        ：連動ｺﾝﾎﾞﾎﾞｯｸｽ
 *  @param {Array.<number>} factoryIdList   ：工場IDリスト
 */
function resetComboBox(appPath, selects, factoryIdList) {
    if (!selects || selects.length == 0) { return; }
    if (!factoryIdList || factoryIdList.length == 0) {
        factoryIdList = getSelectedFactoryIdList(null, true, true);
    }

    resetComboBoxImpl(appPath, selects, factoryIdList);

}

/**
 *  連動ｺﾝﾎﾞの選択ﾘｽﾄを再生成(実処理)
 *  @param {string} appPath                 ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {Array.<Element>} selects        ：連動ｺﾝﾎﾞﾎﾞｯｸｽ
 *  @param {Array.<number>} factoryIdList   ：工場IDリスト
 */
function resetComboBoxImpl(appPath, selects, factoryIdList) {
    $.each(selects, function () {
        var sqlId = $(this).data("sqlid");
        var param = $(this).data("param");
        var ctrlOption = 0;
        var options = $(this).find('option.ctrloption');
        if (options != null && options.length > 0) {
            ctrlOption = 1;
        }
        var optionblank = $(this).find('option.ctrloptionblank');
        if (optionblank != null && optionblank.length > 0) {
            ctrlOption = 2;
        }
        var isNullCheck = $(this).hasClass("validate_required");
        var prmFactoryIdList = [];
        if ($(this).data('factoryctrl') != 'True') {
            // 工場コントロールでない場合、工場IDを指定
            prmFactoryIdList = factoryIdList;
            // コンボに設定されている工場IDをクリア
            setAttrByNativeJs(this, 'data-factoryid', '');
        }

        initComboBox(appPath, this, sqlId, param, ctrlOption, isNullCheck, -1, null, prmFactoryIdList, true);
    });

}

/**
 *  複数選択チェックボックスの選択ﾘｽﾄを再生成
 *  @param {string} appPath                 ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {Array.<Element>} msuls          ：複数選択チェックボックス
 */
function resetMultiSelectBox(appPath, msuls) {
    if (!msuls || msuls.length == 0) { return; }

    $.each(msuls, function () {
        // 要素が参照モードなら処理を行わない
        if ($(this).closest(".ctrlId").data("referencemode") == 1) {
            return true; // continue
        }
        const selector = '#' + $(this).attr('id');
        const sqlId = $(this).data('sqlid');
        const param = $(this).data('param');
        const option = $(this).data('option');
        const nullCheck = $(this).data('nullcheck');

        initMultiSelectBox(appPath, selector, sqlId, param, option, nullCheck);
    });

}

//★del
///**
// *  検索条件ﾃﾞｰﾀ中間ﾃｰﾌﾞﾙ保存処理（取込用）
// *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
// *  @param {button} ：取込ボタン
// *  @param {string} ：機能ID
// *  @param {string} ：プログラムID
// */
//function saveConditionData(appPath, btn, conductId, pgmId, formNo) {

//    // 検索条件取得
//    var tblSearch = getConditionTable();            //条件一覧要素
//    var conditionData = getConditionData(formNo, tblSearch);   //条件ﾃﾞｰﾀ
//    var conditionDataList = [];
//    if (conditionData != null) {
//        conditionDataList.push(conditionData);
//    }

//    // POSTデータを生成
//    var postdata = {
//        conductId: conductId,                   // 機能ID
//        pgmId: pgmId,                           // プログラムID
//        formNo: formNo,                         // 画面番号
//        ctrlId: $(btn).attr("name"),            // 取込ボタンの画面定義のコントロールID
//        conditionData: conditionDataList,       // 検索条件入力データ
//    };

//    // 取込時、検索条件ﾃﾞｰﾀ保存処理実行
//    $.ajax({
//        url: appPath + 'api/CommonProcApi/' + actionkbn.Upload,    // 取込
//        type: 'POST',
//        dataType: 'json',
//        contentType: 'application/json',
//        data: JSON.stringify(postdata),
//        traditional: true,
//        cache: false
//    }).then(
//        // 1つめは通信成功時のコールバック
//        function (resultInfo) {
//            //処理ステータス - CommonProcReturn

//            var status = resultInfo;

//            //// メッセージをクリア
//            //clearMessage();
//            //処理メッセージを表示
//            if (status.MESSAGE != null && status.MESSAGE.length > 0) {
//                // メッセージをクリア
//                clearMessage();
//                //正常時、正常ﾒｯｾｰｼﾞ
//                //警告、異常時、ｴﾗｰﾒｯｾｰｼﾞ
//                addMessage(status.MESSAGE, status.STATUS);
//            }

//        },
//        // 2つめは通信失敗時のコールバック
//        function (resultInfo) {
//            // 検索処理失敗
//            //[0]:処理ステータス - CommonProcReturn
//            var status = resultInfo.responseJSON;

//            //確認ﾒｯｾｰｼﾞは無効
//            if (status.STATUS == procStatus.Confirm) {
//                status.STATUS == procStatus.Error;
//            }

//            //処理結果ｽﾃｰﾀｽを画面状態に反映
//            if (!setReturnStatus(appPath, status, null)) {
//                return false;
//            }

//        });
//}

/**
 *  メニューの初期化処理
 */
function initMenu() {
    //=== 上メニュ(top_menu)表示時の制御 ===
    // 第一階層メニュー処理
    $('#top_menu ul.level1 li').on({
        'mouseenter': function () { // フォーカスが当たったら第二階層を下にスライドして表示
            $(">ul:not(:animated)", this).slideDown("fast")
        },
        'mouseleave': function () { // フォーカスが外れたら第二階層を上にスライドして非表示
            $(">ul", this).slideUp("fast");
        }
    });

    // 第二階層メニュー処理
    $('#top_menu ul.level2 > li > a').on('click', function () {
        // 親の第一階層メニューを取得
        var li = $(this).closest('ul.level1 > li').first();
        if (!$(li).hasClass('selected')) {
            // 選択中でなければ、第一階層の他の選択状態を解除して選択中classをセット
            $('ul.level1 > li.selected').removeClass('selected');
            $(li).addClass('selected');
        }

        // 第二階層の選択状態をすべて解除
        $('#side_menu ul.level2 > li.selected').removeClass('selected');
        clearContentsArea();

        // クリックされた機能IDを取得し、該当するサイドメニューに選択中classをセット
        var conductid = $(this).closest('li').data('conductid');
        $('#side_menu ul.level2 > li[data-conductid="' + conductid + '"]').addClass('selected');

        //サイドメニューを表示
        setHideId("side_menu", false);

        // メニュー切替状態を初期化
        sessionStorage.setItem("CIM_MENU_STATE", "");
    });

    //=== 横メニュ(side_menu)表示時の制御 ===
    // 第一階層メニュー処理
    $('#side_menu ul.level1 > li').on('click', function () {
        // クリックされたサブメニュー配下の表示・非表示を切替
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
            $(this).find('span.glyphicon').removeClass('glyphicon-chevron-up')
            $(this).find('span.glyphicon').addClass('glyphicon-chevron-down')
        }
        else {
            $(this).addClass('selected');
            $(this).find('span.glyphicon').removeClass('glyphicon-chevron-down')
            $(this).find('span.glyphicon').addClass('glyphicon-chevron-up')
        }
    });

    // 第二階層メニュー処理
    $('#side_menu ul.level2 > li > a').on('click', function () {
        // ｾｯｼｮﾝの展開情報を削除
        sessionStorage.removeItem("MENU_SELECTED");
        var data = [];
        $('#side_menu ul.level1 > li.selected').each(function (idx, menu) {
            var conductid = $(menu).data('conductid');
            data.push(conductid);
        });
        // ｾｯｼｮﾝに展開情報を保存
        if (('sessionStorage' in window) && (window.sessionStorage !== null)) {
            sessionStorage.setItem("MENU_SELECTED", JSON.stringify(data));
        }
    });

    // 展開情報を取得
    if (('sessionStorage' in window) && (window.sessionStorage !== null)) {
        var data = sessionStorage.getItem('MENU_SELECTED');
        data = JSON.parse(data);
        if (data != null) {
            for (var i = 0; i < data.length; i++) {
                $('#side_menu ul.level1 li').each(function (idx, menu) {
                    var conductid = $(menu).data('conductid');
                    if (conductid == data[i]) {
                        // 第一階層を選択状態にする
                        $(menu).addClass('selected');
                        $(menu).find('span.glyphicon').removeClass('glyphicon-chevron-down')
                        $(menu).find('span.glyphicon').addClass('glyphicon-chevron-up')
                        return false;
                    }
                });
            }
        }
    }

    if (('sessionStorage' in window) && (window.sessionStorage !== null)) {
        //サイドメニューを表示
        setHideId("side_menu", Boolean(sessionStorage.getItem("CIM_MENU_STATE")));
    }

    //=== トップへ戻るボタンの処理 === 
    var btn = $('.page_top');

    // スクロールしてページトップから1に達したらボタンを表示
    //$(window).on('load scroll', function () {
    $('#main_contents').on('load scroll', function () {
        if ($(this).scrollTop() > 1) {
            btn.addClass('active');
        } else {
            btn.removeClass('active');
        }
    });

    // スクロールしてトップへ戻る
    btn.on('click', function () {
        $('#main_contents').animate({
            scrollTop: 0
        });
    });
}

/**
 * ツリービューの初期化処理
 * @param {string} appPath                      ：アプリケーションルートパス
 * @param {string} conductId                    ：機能ID
 * @param {Array.<number>} structureGrpIdList   ：構成グループID配列
 * @param {Array.<number>} factoryIdList        ：工場ID配列
 * @param {treeViewDef} treeViewType            ：ツリービュー種類
 * @param {Element} model                       ：モーダル画面要素
 * @param {number} initStructureId              ：構成ID初期値
 * @param {number} minLayerNo                   ：階層番号最小値(最上位階層番号)
 * @param {number} maxLayerNo                   ：階層番号最大値(最下位階層番号)
 * @param {Array.<object>} modalValues          ：モーダルのツリーの場合はこちらの値を使用、InitTreeViewModalParamClassメソッドで取得するクラスのリスト
 *                                              ：構成ID初期値、階層番号最小値(最上位階層番号)、階層番号最大値(最下位階層番号)
 * @param {boolean} [refreshData=false]         ：true:ツリーデータをリフレッシュ
 */
function initTreeView(appPath, conductId, structureGrpIdList, factoryIdList, treeViewType, modal, initStructureId, minLayerNo, maxLayerNo, modalValues, refreshData = false) {
    var targetGrpIdList = [];
    $.each(structureGrpIdList, function (idx, grpId) {
        // ローカル端末から構成マスタデータを取得
        var jsonData = getTreeViewLocalData(grpId);
        if (jsonData != null) {
            // ツリービューにデータを設定
            if (treeViewType.Val != treeViewDef.TreeMenu.Val || grpId != structureGroupDef.Job) {
                setTreeView(appPath, grpId, jsonData, treeViewType, modal, initStructureId, minLayerNo, maxLayerNo, modalValues);
            } else {
                // 左側メニューの職種機種の場合は初期化タイミングをずらす
                setTimeout(setTreeView, 500, appPath, grpId, jsonData, treeViewType, modal, initStructureId, minLayerNo, maxLayerNo, modalValues);
            }
        } else {
            // ローカル端末に存在しない場合、サーバからの取得対象に追加
            targetGrpIdList.push(grpId);
        }
    });
    if (targetGrpIdList.length == 0) { return; }

    var conditionDataList= [{ Refresh: refreshData }];

    // グローバル変数に存在しない場合はサーバから取得する
    var postdata = {
        conductId: conductId,                   // メニューの機能ID
        FactoryIdList: factoryIdList,
        StructureGroupList: targetGrpIdList,
        conditionData: conditionDataList,    // データリフレッシュフラグ
    };

    // 階層ツリー要素を取得
    $.ajax({
        url: appPath + 'api/CommonProcApi/' + actionkbn.ComGetStructureList,    //【共通 - 階層ツリー】構成マスタ情報取得
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(postdata),
        headers: { 'X-XSRF-TOKEN': getRequestVerificationToken() },
        traditional: true,
        cache: false
    }).then(
        // 1つめは通信成功時のコールバック
        function (resultInfo) {
            //[0]:処理ステータス - CommonProcReturn
            //[1]:ツリービュー情報
            //[2]:本務工場ID
            var status = resultInfo[0];
            var resultList = resultInfo[1];
            P_DutyFactoryId = resultInfo[2];
            // ツリービュー情報をSessionStorageとグローバル変数へ保存
            for (var key in resultList) {
                var jsonData = resultList[key];
                const grpId = parseInt(key, 10);
                // (2022/11/01) SessionStorageには保存しない
                //setSaveDataToSessionStorage(jsonData, sessionStorageCode.TreeView, grpId);

                P_TreeViewJsonList[grpId] = deepCopyObjectArray(jsonData);
                if (treeViewType.Val != treeViewDef.TreeMenu.Val || grpId != structureGroupDef.Job) {
                    setTreeView(appPath, grpId, jsonData, treeViewType, modal, initStructureId, minLayerNo, maxLayerNo, modalValues);
                } else {
                    setTimeout(setTreeView, 500, appPath, grpId, jsonData, treeViewType, modal, initStructureId, minLayerNo, maxLayerNo, modalValues);
                }
            }
        },
        // 2つめは通信失敗時のコールバック
        function (resultInfo) {
            //異常時
            var status = resultInfo.responseJSON;

            //処理結果ｽﾃｰﾀｽを画面状態に反映
            setReturnStatus(appPath, status, null);
        }
    );
    return null;
}

/**
 * ツリービューのローカル端末からのデータ取得処理
 * @param {number} grpId        ：構成グループID
 */
function getTreeViewLocalData(grpId) {

    // (2022/11/01) 構成マスタデータをセッションストレージには保存しない
    //// SessionStorageから構成マスタデータを取得
    //var jsonData = getSavedDataFromSessionStorage(sessionStorageCode.TreeView, grpId);
    //if (jsonData == null && Array.isArray(P_TreeViewJsonList[grpId])) {
    //    // SessionStorageに存在しない場合、グローバル変数から取得
    //    jsonData = deepCopyObjectArray(P_TreeViewJsonList[grpId]);
    //}
    // グローバル変数から取得
    jsonData = deepCopyObjectArray(P_TreeViewJsonList[grpId]);
    return jsonData
}

/**
 *  ツリービューのデータ設定処理
 * @param {string} appPath          ：アプリケーションルートパス
 * @param {number} grpId            ：構成グループID
 * @param {object} jsonData         ：JSONデータ
 * @param {treeViewDef} treeViewType：ツリービュー種類
 * @param {Element} modal           ：モーダル画面要素
 * @param {number} initStructureId  ：構成ID初期値
 * @param {number} minLayerNo       ：階層番号最小値(最上位階層番号)
 * @param {number} maxLayerNo       ：階層番号最大値(最下位階層番号)
 * @param {Array.<object>} modalValues          ：モーダルのツリーの場合はこちらの値を使用、InitTreeViewModalParamClassメソッドで取得するクラスのリスト
 *                                              ：構成ID初期値、階層番号最小値(最上位階層番号)、階層番号最大値(最下位階層番号)
 */
function setTreeView(appPath, grpId, jsonData, treeViewType, modal, initStructureId, minLayerNo, maxLayerNo, modalValues) {
    var jsonDataW = jsonData;
    const isTreeMenu = treeViewType.Val == treeViewDef.TreeMenu.Val;
    // 場所階層の構成グループは、以下の処理を行わない。場所階層だが取得内容を制御するために構成グループが異なるものも含む
    var noNarrowGrpIds = [structureGroupDef.Location, structureGroupDef.LocationForUserMst, structureGroupDef.LocationHistory, structureGroupDef.LocationNoHistory];
    if (!noNarrowGrpIds.includes(grpId)) {
        // 2024/07/08 機器別管理基準標準用場所階層ツリー追加 UPD start
        //// 予備品ツリーは工場により絞込を行わない
        //if (grpId != structureGroupDef.Parts) {
        // 予備品ツリー/機器別管理基準標準用場所階層ツリーは工場により絞込を行わない
        if (grpId != structureGroupDef.Parts && grpId != structureGroupDef.LocationForMngStd) {
        // 2024/07/08 機器別管理基準標準用場所階層ツリー追加 UPD end
            // 場所階層以外のツリーの場合、選択中の工場IDを取得
            // 左側ツリーメニューの場合、表示中の画面ではなくツリーから取得
            var getFromArticle = !isTreeMenu;
            var factoryIdList = getSelectedFactoryIdList(null, getFromArticle, false);
            // 翻訳で絞り込むかどうか判定
            var isFilterTranslation = grpId == structureGroupDef.FailureCausePersonality;
            // JSONデータを工場IDで絞り込み
            jsonDataW = filterTreeViewJsonDataByFactoryId(jsonDataW, factoryIdList, isFilterTranslation);
        }
        if (isTreeMenu) {
            // 左側メニューの(職種機種の)場合、マージ処理を実行
            //★2024/06/12 TMQ応急対応 C#側でマージ処理実行 start
            //jsonDataW = mergeTreeViewDataList(jsonDataW, 0);
            //★2024/06/12 TMQ応急対応 C#側でマージ処理実行 end
        } else {
            // 左側メニューでない場合、指定階層番号範囲で絞り込み
            if (modalValues != null && modalValues.length > 0) {
                minLayerNo = GetLimitPropValue(modalValues, "MinLayerNo", false);
                maxLayerNo = GetLimitPropValue(modalValues, "MaxLayerNo", true);
            }
            jsonDataW = filterTreeViewJsonDataByStructureNo(jsonDataW, minLayerNo, maxLayerNo);
        }
    }

    $.each(jsonDataW, function (idx, data) {
        data.id = treeViewType.Prefix + data.id;
        if (data.parent != '#') {
            data.parent = treeViewType.Prefix + data.parent;
        }
    });


    //指定階層を展開した状態で表示するツリーの場合true、選択階層までを展開する場合はfalse
    // 2024/07/08 機器別管理基準標準用場所階層ツリー追加 UPD start
    //var openNodeGrpIds = [structureGroupDef.Location, structureGroupDef.LocationForUserMst, structureGroupDef.LocationHistory, structureGroupDef.LocationNoHistory, structureGroupDef.Job];
    var openNodeGrpIds = [structureGroupDef.Location, structureGroupDef.LocationForUserMst, structureGroupDef.LocationHistory, structureGroupDef.LocationNoHistory, structureGroupDef.Job, structureGroupDef.LocationForMngStd];
    // 2024/07/08 機器別管理基準標準用場所階層ツリー追加 UPD end
    const isOpenNode = openNodeGrpIds.includes(grpId);

    var selector = '#' + getTreeViewId(grpId, treeViewType.Val);
    $(selector).jstree({
        plugins: ["checkbox"],
        checkbox: {
            three_state: treeViewType.ThreeState,
            // 選択された行に背景色をつけるか否か
            keep_selected_style: false,
            cascade: "undetermined"
        },
        core: {
            data: jsonDataW,
            //アイコンを表示するか否か
            themes: { icons: false },
            //複数選択
            multiple: treeViewType.Multiple,
            animation: 0,
            //選択ノードの展開有無(true:選択ノード展開、false：未展開)
            expand_selected_onload: !isOpenNode
        }
    }).on('loaded.jstree', function (e, data) {
        if (isTreeMenu) {
            // 左側メニューの場合
            var code;
            var isMerged;
            if (grpId == structureGroupDef.Location) {
                code = localStorageCode.LocationTree;
                isMerged = false;
            } else {
                code = localStorageCode.JobTree;
                isMerged = true;
            }
            // sessionStorate/localStorage保存の選択値の反映
            setStorageTreeData(grpId, code, selector, isMerged, isOpenNode);
            //場所階層ツリーの場合、工場階層まで展開する
            openFactoryNode(grpId, selector);

            $(selector).on('changed.jstree', function (e, data) {
                // 選択状態変更時、選択状態をセッションストレージに保存する
                var structureIdList = getSelectedStructureIdList(grpId, treeViewDef.TreeMenu, grpId != structureGroupDef.Location);
                setSaveDataToSessionStorage(structureIdList, sessionStorageCode.TreeViewSelected, grpId)

                if (grpId == structureGroupDef.Location) {
                    // 場所階層の場合、ローカルデータから職種機種データを取得
                    var jobData = getTreeViewLocalData(structureGroupDef.Job);
                    // ツリーメニューより選択中の工場IDを取得
                    var factoryIdList = getSelectedFactoryIdListFromLocationTree();
                    //if (P_SelectedFactoryIdList == null || JSON.stringify(P_SelectedFactoryIdList) != JSON.stringify(factoryIdList)) {
                        //if (P_SelectedFactoryIdList != null || factoryIdList.length > 0) {
                            // 選択中の工場が変更された場合、グローバル変数に保存
                            P_SelectedFactoryIdList = factoryIdList;
                        //}
                        // 工場IDで絞り込み
                        var filteredList = filterTreeViewJsonDataByFactoryId(jobData, factoryIdList);
                        //★2024/06/12 TMQ応急対応 C#側でマージ処理実行 start
                        // マージ処理実行
                        //filteredList = mergeTreeViewDataList(filteredList, 0);
                        //★2024/06/12 TMQ応急対応 C#側でマージ処理実行 end
                        // ツリービューに設定
                        var jobSelector = '#' + getTreeViewId(structureGroupDef.Job, treeViewDef.TreeMenu.Val);
                        updateTreeViewData(jobSelector, filteredList);

                        // 構成マスタのコントロールの選択アイテムを工場IDで絞り込む
                        var structureCtrls = $('[data-usefactoryfilter="true"]');
                        if (structureCtrls && structureCtrls.length > 0) {
                            $.each(structureCtrls, function (idx, ctrl) {
                                const tagName = $(ctrl).prop('tagName').toLowerCase();
                                let selector = tagName + '#' + $(ctrl).attr('id');
                                const sqlId = $(ctrl).data('sqlid');
                                const param = $(ctrl).data('param');
                                const option = $(ctrl).data('option');
                                const nullCheck = $(ctrl).data('nullcheck');
                                if (tagName == 'select') {
                                    // コンボボックスの初期化
                                    //initComboBox(appPath, selector, sqlId, param, option, nullCheck, colNoForFactory, $(ctrl).val());
                                    initComboBox(appPath, selector, sqlId, param, option, nullCheck, colNoForFactory, null, factoryIdList);
                                } else if (tagName == 'ul') {
                                    // 複数選択チェックボックスの初期化
                                    initMultiSelectBox(appPath, selector, sqlId, param, option, nullCheck, factoryIdList);
                                } else if (tagName == 'td' && $(ctrl).data('type') == 'treeLabel') {
                                    var tdFactoryId = $(ctrl).data('factoryid');
                                    if (tdFactoryId != null && tdFactoryId.length > 0) {
                                        if (factoryIdList.length == 0 || factoryIdList.indexOf(tdFactoryId) < 0) {
                                            // 工場IDが変更されたらツリー選択ラベルの値をクリアする
                                            setStructureInfoToTreeLabel(ctrl, '', '');
                                        }
                                    }
                                    // ツリー選択ラベルに工場IDをセットする
                                    if (factoryIdList.length > 0) {
                                        setAttrByNativeJs(td, 'data-factoryid', factoryIdList[0]);
                                    } else {
                                        setAttrByNativeJs(td, 'data-factoryid', '');
                                    }
                                }
                                selector = null;
                            });
                        }
                    //}

                }
            });
            if (grpId == structureGroupDef.Job) {
                // 職種機種の場合、選択時に他階層に同名の項目があれば選択状態にする
                $(selector).on('select_node.jstree', function (e, data) {
                    if (data.node.state.selected) {
                        //チェックが付いているノードのIDと表示文字列
                        const selectedId = data.node.id;
                        const selectedText = data.node.text;
                        const tree = data.instance;
                        $(tree.get_json('#', { flat: true }))
                            .each(function (index, value) {
                                var node = tree.get_node(this.id);
                                if (node.id != selectedId && node.text == selectedText && !node.state.selected) {
                                    // 表示文字列が一致するノードにチェックを入れる
                                    tree.check_node(node.id);
                                }
                            });
                    }
                });
            }

        } else {
            var selectedIdList = [];
            if (modalValues != null && modalValues.length > 0) {
                $.each(modalValues, function (index, modalValue) {
                    selectedIdList.push(modalValue.StructureId);
                });
            } else {
                if (initStructureId != null) {
                    selectedIdList.push(initStructureId);
                }
            }
            //★2024/06/28 TMQ応急対応 SQL側でマージ処理実行 Mod start
            //setSelectedDataToTreeView(selector, selectedIdList, false, isOpenNode);
            // 職種機種の場合、マージデータ
            var isMerged = grpId == structureGroupDef.Job;
            setSelectedDataToTreeView(selector, selectedIdList, isMerged, isOpenNode);
            //★2024/06/28 TMQ応急対応 SQL側でマージ処理実行 Mod end
            //場所階層ツリーの場合、工場階層まで展開する
            openFactoryNode(grpId, selector);
        }

        if (modal != null && treeViewType.Val == treeViewDef.ModalForm.Val) {
            // 階層ツリーモーダル画面の場合、モーダル画面表示
            showModalForm(modal);
        }
        //【オーバーライド用関数】ツリービュー読込後処理
        afterLoadedTreeView(selector, isTreeMenu, grpId, isOpenNode);

    }).on('refresh.jstree', function (e, data) {
        //【オーバーライド用関数】ツリービューリフレッシュ後処理
        afterRefreshTreeView(selector, isTreeMenu, grpId, isOpenNode);
    });
}

/**
 * 選択中の工場ID配列を取得
 * @param {HTMLElement} tabulatorRow    ：tabulator一覧の行要素
 * @param {boolean} getFromArticle      ：表示中の画面から取得するかどうか
 * @param {boolean} isForComboBox       ：コンボボックス項目の絞り込みパラメータかどうか
 * @return {Array.<number>} 選択中の工場ID配列
 */
function getSelectedFactoryIdList(tabulatorRow, getFromArticle, isForComboBox) {
    var factoryIdList = [];
    if (tabulatorRow) {
        // tabulatorの一覧から工場IDを取得
        factoryIdList = getFactoryIdList(tabulatorRow);
    }
    if (getFromArticle) {
        // 表示中の画面から工場IDを取得
        factoryIdList = getFactoryIdList(P_Article);
        if (factoryIdList.length == 0) {
            if (getCurrentModalNo(P_Article) > 0) {
                // モーダル画面の場合、呼び元の詳細エリアも探す
                factoryIdList = getFactoryIdList($("#detail_divid_" + getFormNo()));
            }
        }
    }

    if (factoryIdList.length == 0) {
        // 表示中の画面から取得できない場合は、場所階層ツリーから取得
        factoryIdList = deepCopyObjectArray(P_SelectedFactoryIdList);
        if (factoryIdList == null || factoryIdList.length == 0) {
            // ローカル端末から構成マスタデータを取得
            var jsonData = getTreeViewLocalData(structureGroupDef.Location);
            // ストレージから選択中の構成IDを取得
            var selectedStructureIdList = getSelectedStructureIdListFromStorage(structureGroupDef.Location);
            var selectedFactoryIdList;
            if (selectedStructureIdList && selectedStructureIdList.length > 0) {
                // ローカルストレージが空でない場合に処理を行う
                selectedStructureIdList = selectedStructureIdList.map(x => x.toString());
                if (jsonData != null) {
                    selectedFactoryIdList = $.grep(jsonData, function (data, idx) {
                        return selectedStructureIdList.indexOf(data.id) >= 0;
                    });
                }
            }
            if (selectedFactoryIdList && selectedFactoryIdList.length > 0) {
                factoryIdList = [];
                $.each(selectedFactoryIdList, function (key, data) {
                    factoryIdList.push(getTreeViewFacrotyId(data));
                });
                P_SelectedFactoryIdList = factoryIdList;
            }
            if (factoryIdList == null || factoryIdList.length == 0) {
                if (!isForComboBox) {
                    // コンボパラメータ以外の場合は空の配列に
                    factoryIdList = [];
                    return;
                } else {
                    // コンボパラメータの場合、本務工場IDを設定
                    factoryIdList = [P_DutyFactoryId];
                }
            }
        }
    }
    if (isForComboBox) {
        if (factoryIdList.length > 1) {
            // 複数選択されている場合
            if (factoryIdList.indexOf(P_DutyFactoryId) >= 0) {
                // 本務工場が含まれている場合、本務工場を選択
                factoryIdList = [P_DutyFactoryId];
            } else {
                // 本務工場が含まれていない場合、先頭工場を選択
                factoryIdList = [factoryIdList[0]];
            }
        }
    }
    return factoryIdList;
}

/**
 * 場所階層メニューより選択中の工場ID配列を取得
 * @return {Array.<number>} 選択中の工場ID配列
 */
function getSelectedFactoryIdListFromLocationTree() {
    var factoryIdList = [];

    //チェックが付いているノードのリスト
    var locationTree = $('#' + getTreeViewId(structureGroupDef.Location, treeViewDef.TreeMenu.Val));
    var selectList = $(locationTree).jstree(true).get_bottom_checked(true);

    $.each(selectList, function (idx, node) {
        //各ノードの工場ID、構成IDを取得
        var factoryId = getTreeViewFacrotyId(node);//data-factoryid属性
        if (factoryIdList.indexOf(factoryId) == -1 && factoryId >= 0) {
            factoryIdList.push(factoryId);
        }
    });
    return factoryIdList;
}

/**
 * 画面上の工場コントロールを配置しているtd要素を取得する
 * @return {HTMLElement} 工場コントロールを配置しているtd要素
 */
function getFactoryTdElement(selector, tr) {
    var target = tr ? tr : $(selector).closest('form');
    var ctrl = $(target).find('[data-factoryctrl="True"]');
    if (!ctrl || ctrl.length == 0) {
        // 同一form上にない場合は他のformから探す
        target = $(target).siblings('form');
        ctrl = $(target).find('[data-factoryctrl="True"]');
        if (!ctrl || ctrl.length == 0) {
            // 表示中の画面上に工場コントロールがない場合はnullを返す
            return null;
        }
    }
    if ($(ctrl).prop('tagName').toLowerCase() == 'td') {
        // tdタグ(=treeLabelの場合)そのまま返す
        return ctrl;
    }
    // それ以外の場合親のtdタグを返す
    return $(ctrl).closest('td');
}

/**
 * 工場コントロール値を取得する
 * @param {HTMLElement} target ：工場コントロール検索対象要素
 * @return {Array.<number>} 工場ID
 */
function getFactoryIdList(target) {
    var factoryIdList = [];
    var factoryCtrl = null;
    factoryCtrl = $(target).find('[data-factoryctrl="True"]');
    if (factoryCtrl && factoryCtrl.length > 0) {
        if ($(factoryCtrl).data('type') == 'treeLabel') {
            if ($(factoryCtrl).data('structureid')) {
                var structureId = parseInt($(factoryCtrl).data('structureid'), 10);
                if (factoryIdList.indexOf(structureId) < 0) {
                    factoryIdList.push(structureId);
                }
            }
            if (factoryCtrl.parents('.addTreeLabel').length > 0) {
                // 複数ある場合、追加
                $.each(factoryCtrl, function (idx, ele) {
                    if ($(ele).data('structureid')) {
                        var structureId = parseInt($(ele).data('structureid'), 10);
                        if (factoryIdList.indexOf(structureId) < 0) {
                            factoryIdList.push(structureId);
                        }
                    }
                });
            }
        } else {
            if ($(factoryCtrl).val()) {
                var structureId = parseInt($(factoryCtrl).val(), 10);
                if (factoryIdList.indexOf(structureId) < 0) {
                    factoryIdList.push(structureId);
                }
            }
        }
    }
    return factoryIdList;
}

/**
 * ツリービューJSONデータの階層番号による絞り込み
 * @param {Array.<Object>} jsonList :JSONデータ配列
 * @param {number} minLayerNo       :階層番号最小値(最上位階層番号)
 * @param {number} maxLayerNo       :階層番号最大値(最下位階層番号)
 */
function filterTreeViewJsonDataByStructureNo(jsonList, minLayerNo, maxLayerNo) {
    var resultList = jsonList;
    if (minLayerNo == null || maxLayerNo == null) {
        return resultList;
    }

    resultList = $.grep(resultList, function (data, idx) {
        var layerNo = getTreeViewLayerNo(data);
        if (layerNo < minLayerNo) {
            // 対象階層より上位の階層はチェックボックスを不活性にセット
            data.state.disabled = true;
        }
        // 対象階層より下位の階層は非表示に
        return (data.parent == '#' || layerNo <= maxLayerNo);
    });
    return resultList;
}

/**
 * ツリービューJSONデータの工場IDによる絞り込み
 * @param {Array.<Object>} jsonList         :JSONデータ配列
 * @param {Array.<number>} factoryIdList    :工場ID配列
 * @param {boolean} isFilterTranslation     :翻訳で絞り込みを行う場合True(原因性格)
 */
function filterTreeViewJsonDataByFactoryId(jsonList, factoryIdList, isFilterTranslation) {
    var resultList = jsonList;
    if (factoryIdList == null || factoryIdList.length == 0) {
        // 工場が未指定の場合、ルート要素のみ表示
        resultList = $.grep(resultList, function (data, idx) {
            return (data.parent == '#');
        });
    } else {
        var resultList2 = [];
        if (!isFilterTranslation) {
            // 翻訳を行わない場合は、リビジョン3734の対応前の処理と同じ
            $.each(resultList, function (idx, data) {
                var factoryId = getTreeViewFacrotyId(data);
                //★2024/06/26 TMQ応急対応 SQL側でマージ処理実行 Mod start
                //★2024/06/12 TMQ応急対応 C#側でマージ処理実行 ADD start
                //if (data.li_attr['data-mergefactory'] && data.li_attr['data-mergefactory'].length > 0) {
                //    var mergeFactoryIdList = getTreeViewMergeFacrotyIdList(data);
                //    if (mergeFactoryIdList && mergeFactoryIdList.length > 0) {
                //        // マージ後の工場IDが含まれる場合、結果リストに追加
                //        $.each(factoryIdList, function (idx, id) {
                //            if (mergeFactoryIdList.indexOf(id) >= 0) {
                //                resultList2.push(data);
                //                return false;   // break;
                //            }
                //        });
                //    }
                //}
                ////★2024/06/12 TMQ応急対応 C#側でマージ処理実行 ADD end
                //else if (data.parent == '#' || (factoryId > 0 && factoryIdList.indexOf(factoryId) >= 0)) {
                if (data.parent == '#' || (factoryId > 0 && factoryIdList.indexOf(factoryId) >= 0)) {
                    // ルート要素または指定工場の要素の場合、結果リストに追加
                    resultList2.push(data);
                }
                else if (data.li_attr['data-mergefactory'] && data.li_attr['data-mergefactory'].length > 0) {
                    var mergeFactoryIdList = getTreeViewMergeFacrotyIdList(data);
                    if (mergeFactoryIdList && mergeFactoryIdList.length > 0) {
                        // マージ後の工場IDが含まれる場合、結果リストに追加
                        if (mergeFactoryIdList.indexOf(0) >= 0) {
                            // 各工場共通アイテムの場合
                            resultList2.push(data);
                        } else {
                            $.each(factoryIdList, function (idx, id) {
                                if (mergeFactoryIdList.indexOf(id) >= 0) {
                                    resultList2.push(data);
                                    return false;   // break;
                                }
                            });
                        }
                    }
                }
                //★2024/06/26 TMQ応急対応 SQL側でマージ処理実行 Mod end
                else if (factoryId == 0) {
                    // 共通工場の要素の場合、構成IDが同一の要素を検索
                    var structureId = getTreeViewStructureId(data);
                    var duplicatedData = $.grep(resultList, function (x, i) {
                        return (structureId == getTreeViewStructureId(x));
                    });
                    if (duplicatedData.length == 1) {
                        // 自分自身しか存在しない場合、結果リストに追加
                        resultList2.push(data);
                    }
                }
            });
        } else {
            // 翻訳を行う場合

            if (factoryIdList.length > 1) {
                // 複数選択されている場合
                if (factoryIdList.indexOf(P_DutyFactoryId) >= 0) {
                    // 本務工場が含まれている場合、本務工場を選択
                    factoryIdList = [P_DutyFactoryId];
                } else {
                    // 本務工場が含まれていない場合、先頭工場を選択
                    factoryIdList = [factoryIdList[0]];
                }
            }
            $.each(resultList, function (idx, data) {
                var factoryId = getTreeViewFacrotyId(data);
                if (data.parent == '#') {
                    // ルート要素の場合、結果リストに追加
                    resultList2.push(data);
                } else if (factoryIdList[0] == 0 && factoryId == 0) {
                    // 共通工場が本務かつ共通工場の要素の場合
                    if (getTreeViewTranslationFactoryId(data) == 0) {
                        // 共通翻訳のデータを結果リストに追加
                        resultList2.push(data);
                    }
                } else if (factoryId > 0 && factoryIdList.indexOf(factoryId) >= 0) {
                    // 工場指定の場合(ツリーで翻訳工場指定の場合、全工場分のデータを保持)
                    var structureId = getTreeViewStructureId(data);
                    // 同一の構成ID、工場IDで絞り込み
                    var duplicatedData = $.grep(resultList, function (x, i) {
                        return (structureId == getTreeViewStructureId(x) && factoryId == getTreeViewFacrotyId(x));
                    });
                    // 工場個別の翻訳がある場合、複数取得される
                    // 共通の翻訳と個別の工場の翻訳を取得し、個別→共通の順に優先して取得
                    var targetFactoryId = factoryIdList[0]; // 個別の工場のID、工場IDが複数の場合は無いと思われる
                    var targetData = $.grep(duplicatedData, function (x, i) {
                        // 個別工場IDの翻訳があるかどうかを取得
                        return (targetFactoryId == getTreeViewTranslationFactoryId(x));
                    });
                    if (targetData.length == 0) {
                        // 無い場合、共通工場の翻訳を追加する
                        targetFactoryId = 0;
                    }
                    //既に追加済みか確認
                    var count = $.grep(resultList2, function (x, i) {
                        //構成IDが一致するもの
                        return (structureId == getTreeViewStructureId(x));
                    }).length;
                    if (getTreeViewTranslationFactoryId(data) == targetFactoryId && count == 0) {
                        // 要素の翻訳IDと対象の工場IDが同じ場合、追加
                        resultList2.push(data);
                    }
                }
            });
        }
        resultList = resultList2;
    }
    return resultList;
}

/**
 * ツリービューの更新
 * @param {any} selector
 * @param {any} jsonList
 */
function updateTreeViewData(selector, jsonList) {
    var tree = $(selector).jstree(true);
    if (tree) {
        tree.settings.core.data = jsonList;
        tree.refresh();
    }
}
/**
 * ツリービューの表示文字列重複データのマージ処理
 * @param {Array.<Object>} jsonList :JSONデータ配列
 * @param {number} layerNo          :階層番号
 * @return {Array.<Object>} マージ後JSONデータ配列
 */
function mergeTreeViewDataList(jsonList, layerNo) {
    var resultList = jsonList;
    if (resultList == null || resultList.length == 0) {
        return resultList;
    }

    // 同一階層のデータを取得
    var sameLayerList = $.grep(resultList, function (data, idx) {
        return (getTreeViewLayerNo(data) == layerNo);
    });
    if (!sameLayerList.length) {
        return resultList;
    }

    // 同一parentでグループ化
    var groupByParentList = getGroupedObjectList(sameLayerList, 'parent');

    // 同一parent単位で重複チェック
    $.each(groupByParentList,
        function (keyParent, list) {
            // 重複を検出したものを抽出
            var duplicateList = list.filter((x, y, z) => z.slice(0, z.length).filter(w => w.text == x.text).length >= 2);
            if (duplicateList == null || duplicateList.length == 0) {
                return true;	// continue;
            }

            // 重複ありの場合、同一表示文字列でグループ化
            var groupByTextList = getGroupedObjectList(duplicateList, 'text');

            $.each(groupByTextList,
                function (keyText, sameTextList) {
                    // マージ構成情報リストを生成
                    var mergeInfoList = [];
                    var nodeId = '';
                    $.each(sameTextList,
                        function (j, data) {
                            // マージ構成情報に構成IDを追加
                            mergeInfoList.push(getTreeViewStructureId(data));
                            if (j == 0) {
                                nodeId = data.id;
                            } else {
                                // 1階層下のJSONデータからparentとノードIDが一致するデータを抽出
                                var lowerList = $.grep(resultList, function (lowerData, k) {
                                    return (getTreeViewLayerNo(lowerData) == layerNo + 1 && lowerData.parent == data.id);
                                });
                                // 先頭データのノードIDで更新
                                $.each(lowerList, function (k, lowerData) {
                                    setTreeViewParentNodeId(resultList, lowerData.id, nodeId)
                                });
                                // マージ後配列から削除
                                resultList = resultList.filter((x) => {
                                    return (x.id != data.id);
                                });
                            }
                        }
                    );
                    // マージ後配列にマージ情報配列を設定
                    setTreeViewMergeInfoList(resultList, nodeId, mergeInfoList);
                }
            );
        }
    );

    // 再帰的に下位の階層のマージ処理を呼び出す
    return mergeTreeViewDataList(resultList, layerNo + 1);
}

/**
 * 連想配列をグループ化
 * @param {Array.<Object>} list :対象連想配列
 * @param {string} keyName      :キー項目名
 * @return {Array.<Object>} :グループ化後連想配列
 */
function getGroupedObjectList(list, keyName) {
    var resultList = list.reduce(function (result, current) {
        //current[keyName]が数値の場合に順序が入れ替わってしまうため（index扱いになる）、文字列化する
        result["ID" + current[keyName]] = result["ID" + current[keyName]] || [];
        result["ID" + current[keyName]].push(current);
        return result;
    }, {});
    return resultList;
}

/**
 * ツリービューの階層番号取得
 * @param {Object} data :JSONデータ
 * @return {number} 階層番号
 */
function getTreeViewLayerNo(data) {
    if (data.li_attr) {
        return parseInt(data.li_attr['data-structureno'], 10);
    } else {
        return -1;
    }
}

/**
 * ツリービューの工場ID取得
 * @param {Object} data :JSONデータ
 * @return {number} 工場ID
 */
function getTreeViewFacrotyId(data) {
    return data.li_attr['data-factoryid'];
}

//★2024/06/12 TMQ応急対応 C#側でマージ処理実行 ADD start
/**
 * ツリービューのマージ後工場IDリスト取得
 * @param {Object} data :JSONデータ
 * @return {number} 工場IDリスト
 */
function getTreeViewMergeFacrotyIdList(data) {
    return data.li_attr['data-mergefactory'];
}
//★2024/06/12 TMQ応急対応 C#側でマージ処理実行 ADD end

/**
 * ツリービューの構成ID取得
 * @param {Object} data :JSONデータ
 * @return {number} 構成ID
 */
function getTreeViewStructureId(data) {
    //★2024/07/19 TMQ応急対応 SQL側でマージ処理実行 Mod start
    //if (data.li_attr) {
    //    return data.li_attr['data-structureid'];
    //} else {
    //    return -1;
    //}
    var structureIdList = getTreeViewStructureIdList(data);
    if (structureIdList.length == 1) {
        return structureIdList[0];
    } else if (structureIdList.length > 1) {
        // マージされている場合
        // 選択中の工場IDを取得
        var selectedFactoryIdList = getSelectedFactoryIdList(null, true, false);
        if (selectedFactoryIdList.length > 0) {
            // 対応する工場ID配列を取得
            var facrotyIdList = getTreeViewMergeFacrotyIdList(data);
            var idx = facrotyIdList.indexOf(selectedFactoryIdList[0]);
            if (idx >= 0) {
                // 工場のインデックスと同じ位置の構成IDを追加
                return structureIdList[idx];
            }
        }
    }
    return -1;
    //★2024/07/19 TMQ応急対応 SQL側でマージ処理実行 Mod end
}

/**
 * ツリービューの翻訳の工場ID取得
 * @param {Object} data :JSONデータ
 * @return {number} 翻訳の工場ID
 */
function getTreeViewTranslationFactoryId(data) {
    if (data.li_attr) {
        return data.li_attr['data-translatefactoryid'];
    } else {
        return -1;
    }
}

/**
 * ツリービューの構成ID配列取得(左側メニュー職種機種用)
 * @param {Object} data :JSONデータ
 * @return {Array.<number>} 構成ID配列
 */
function getTreeViewStructureIdList(data) {
    if (data.li_attr) {
        if (data.li_attr['data-mergeinfo']) {
            return data.li_attr['data-mergeinfo'];
        } else {
            return [data.li_attr['data-structureid']];
        }
    } else {
        return [];
    }
}

/**
 * ツリービューの親ノードIDの設定
 * @param {Array.<Object>} jsonList :JSON配列データ
 * @param {string} nodeId           :設定対象ノードID
 * @param {string} newParentNodeId  :親ノードID
 */
function setTreeViewParentNodeId(jsonList, nodeId, newParentNodeId) {
    $.each(jsonList,
        function (i, data) {
            if (data.id === nodeId) {
                jsonList[i].parent = newParentNodeId;
                return false;   // break;
            }
        }
    );
}

/**
 * ツリービューのマージ情報の設定
 * @param {Array.<Object>} jsonList         :JSON配列データ
 * @param {string} nodeId                   :設定対象ノードID
 * @param {Array.<number>} mergeInfoList    :マージ情報(構成ID)配列
 */
function setTreeViewMergeInfoList(jsonList, nodeId, mergeInfoList) {
    $.each(jsonList,
        function (i, data) {
            if (data.id === nodeId) {
                jsonList[i].li_attr['data-structureid'] = '';
                jsonList[i].li_attr['data-factoryid'] = '';
                jsonList[i].li_attr['data-mergeinfo'] = mergeInfoList;
                return false;   // break;
            }
        }
    );
}

/**
 * ツリービューのセレクタの取得
 * @param {number} grpId            ：構成グループID
 * @param {treeViewDef} treeViewVal ：ツリービュー種類
 * @return {string} セレクタ文字列
 */
function getTreeViewId(grpId, treeViewVal) {
    var selector;
    if (treeViewVal == treeViewDef.TreeMenu.Val) {
        selector = grpId == structureGroupDef.Location ? 'tree_location' : 'tree_job';
    } else {
        selector = 'tree_' + grpId;
    }
    return selector;
}

/**
 * ローカルストレージのキーの取得
 * @param {number} code     ：データ種類コード
 * @param {string} conductId：機能ID
 * @param {number} formNo   ：画面番号
 * @param {string} id       ：コントロールグループID
 * @return {string} ローカルストレージキー
 */
function getLocalStorageKey(code, conductId, formNo, id) {
    // データ種類コード(2桁)＋機能ID(８桁)＋画面番号(2桁)＋コントロールグループID(20桁)
    return paddingText(code, 2, '0', false) + paddingText(conductId, 8, '0', false) + paddingText(formNo, 2, '0', false) + paddingText(id, 20, '0', false);
}

/**
 * セッションストレージのキーの取得(共通)
 * @param {number} code     ：データ種類コード
 * @param {number} id       ：識別ID
 * @return {string} セッションストレージキー
 */
function getSessionStorageKey(code, id) {
    // データ種類コード(2桁)＋識別ID(20桁)
    return paddingText(code, 2, '0', false) + paddingText(id, 20, '0', false);
}

/**
 * セッションストレージのキーの取得(一覧関連)
 * @param {number} code     ：データ種類コード
 * @param {string} conductId：機能ID
 * @param {number} formNo   ：画面番号
 * @param {string} id       ：コントロールグループID
 * @return {string} セッションストレージキー
 */
function getSessionStorageKeyForList(code, conductId, formNo, id) {
    // データ種類コード(2桁)＋機能ID(８桁)＋画面番号(2桁)＋コントロールグループID(20桁)
    return paddingText(code, 2, '0', false) + paddingText(conductId, 8, '0', false) + paddingText(formNo, 2, '0', false) + paddingText(id, 20, '0', false);
}

/**
 * ローカルストレージから情報を取得
 * @param {number} code     :ローカルストレージデータ種類
 * @param {number} conductId:機能ID
 * @param {number} formNo   :画面番号
 * @param {string} id       :識別ID
 * @return {Array.<object>} JSONデータ
 */
function getSavedDataFromLocalStorage(code, conductId = 0, formNo = 0, id = 0) {
    const key = getLocalStorageKey(code, conductId, formNo, id);
    const jsonStr = localStorage.getItem(key);
    if (jsonStr != null) {
        return JSON.parse(jsonStr);
    } else {
        return null;
    }
}

/**
 * セッションストレージから情報を取得
 * @param {number} code     ：セッションストレージデータ種類
 * @param {number} id       ：識別ID
 * @return {Array.<object>} JSONデータ
 */
function getSavedDataFromSessionStorage(code, id) {
    let key = getSessionStorageKey(code, id);
    let jsonStr = sessionStorage.getItem(key);
    if (jsonStr != null) {
        let result = JSON.parse(jsonStr);
        key = null;
        jsonStr = null;
        return result;
    } else {
        return null;
    }
}

/**
 * セッションストレージから情報を取得
 * @param {number} code     ：セッションストレージデータ種類
 * @param {number} id       ：識別ID
 * @return {Array.<object>} JSONデータ
 */
function getSavedDataFromSessionStorageForList(code, conductId, formNo, id) {
    const key = getSessionStorageKeyForList(code, conductId, formNo, id);
    const jsonStr = sessionStorage.getItem(key);
    if (jsonStr != null) {
        return JSON.parse(jsonStr);
    } else {
        return null;
    }
}

/**
 * ローカルストレージへ情報を保存
 * @param {object} jsonData ：JSONデータ
 * @param {number} code     :ローカルストレージデータ種類
 * @param {number} conductId:機能ID
 * @param {number} formNo   :画面番号
 * @param {string} id       :識別ID
 */
function setSaveDataToLocalStorage(jsonData, code, conductId = 0, formNo = 0, id = 0) {
    const key = getLocalStorageKey(code, conductId, formNo, id);
    localStorage.setItem(key, JSON.stringify(jsonData));
}
/**
 * ローカルストレージの保存情報を削除
 * @param {number} code     :ローカルストレージデータ種類
 * @param {number} conductId:機能ID
 * @param {number} formNo   :画面番号
 * @param {string} id       :識別ID
 */
function removeSaveDataFromLocalStorage(code, conductId = 0, formNo = 0, id = 0) {
    const key = getLocalStorageKey(code, conductId, formNo, id);
    localStorage.removeItem(key);
}

/**
 * セッションストレージへJSONデータを設定
 * @param {object} jsonData ：JSONデータ
 * @param {number} code     ：セッションストレージデータ種類
 * @param {number} id       ：識別ID
 */
function setSaveDataToSessionStorage(jsonData, code, id) {
    const key = getSessionStorageKey(code, id);
    sessionStorage.setItem(key, JSON.stringify(jsonData));
}

/**
 * セッションストレージへ一覧関連情報を設定
 * @param {object} jsonData ：JSONデータ
 * @param {number} code     ：セッションストレージデータ種類
 * @param {number} conductId：機能ID
 * @param {number} formNo   ：画面番号
 * @param {number} id       ：コントロールID
 */
function setSaveDataToSessionStorageForList(jsonData, code, conductId, formNo, id) {
    const key = getSessionStorageKeyForList(code, conductId, formNo, id);
    sessionStorage.setItem(key, JSON.stringify(jsonData));
}

/**
 * セッションストレージの保存情報を削除
 * @param {number} code     :セッションストレージデータ種類
 * @param {number} conductId:機能ID
 * @param {number} formNo   :画面番号
 * @param {string} id       :識別ID
 */
function removeSaveDataFromSessionStorage(code, id) {
    const key = getSessionStorageKey(code, id);
    removeSaveDataFromSessionStorageByKey(key);
}

/**
 * セッションストレージの保存情報を削除
 * @param {number} code     :セッションストレージデータ種類
 * @param {number} conductId:機能ID
 * @param {number} formNo   :画面番号
 * @param {string} id       :識別ID
 */
function removeSaveDataFromSessionStorageByKey(key) {
    sessionStorage.removeItem(key);
}

/**
 * セッションストレージの一覧関連情報を削除
 * @param {number} code     :セッションストレージデータ種類
 * @param {number} conductId:機能ID
 * @param {number} formNo   :画面番号
 * @param {string} id       :識別ID
 */
function removeSaveDataFromSessionStorageForList(code, conductId, formNo, id) {
    const key = getSessionStorageKeyForList(code, conductId, formNo, id);
    sessionStorage.removeItem(key);
}

/**
 * セッションストレージのキーを取得
 * @param {string} code     :セッションストレージデータ種類
 * @param {string} keyword  :検索キーワード
 * @return Array.<string> 検索条件に一致したキー
 */
function findSessionStorageKeys(code, keyword) {
    var keys = [];
    const settionStorageKeys = Object.keys(sessionStorage);
    if (keyword) {
        keys = settionStorageKeys.filter(function (key) {
            return key.indexOf(code) == 0 && key.indexOf(keyword, code.length) >= 0;
        });
    } else {
        keys = settionStorageKeys.filter(function (key) {
            return key.indexOf(code) == 0;
        });
    }
    return keys;
}

/**
 * 階層選択モーダル画面の初期化
 * @param {string} appPath                  ：アプリケーションパス
 * @param {Element} modal                   ：モーダル画面要素
 * @param {Array.<number>} structureGrpId   ：構成グループID配列
 * @param {Array.<object>} treeValues       ： InitTreeViewModalParamClassメソッドで取得するクラスのリスト
 *                                          ：構成ID初期値、階層番号最小値(最上位階層番号)、階層番号最大値(最下位階層番号)
 * @param {boolean} isMultiSelect           ：複数選択可否(複数：True、単一：False)
 */
function initTreeViewModalForm(appPath, modal, structureGrpId, treeValues, isMultiSelect) {
    const id = getTreeViewId(structureGrpId, treeViewDef.ModalForm.Val);
    var parentDiv = $(modal).find('.form_tree_div');
    var treeDiv = $(parentDiv).find('#' + id);
    // 該当する構成グループ以外を非表示
    $(parentDiv).children('[id!="' + id + '"]').addClass('hide');

    if (treeDiv != null && treeDiv.length > 0) {
        //// 生成済みの場合、該当する構成グループを表示
        //// ツリービューを取得
        //var selectedIdList = [];
        //if (initStructureId != null) {
        //    selectedIdList.push(initStructureId);
        //}
        //setSelectedDataToTreeView(treeDiv, selectedIdList, false);
        //$(treeDiv).jstree(true).refresh();

        //$(treeDiv).removeClass('hide');
        //// - ﾎﾟｯﾌﾟｱｯﾌﾟ表示
        //showModalForm(modal);
        //return;

        // 一旦ツリービューを破棄して再生成する
        $(treeDiv).jstree(true).destroy();
        $(treeDiv).removeClass('hide');
    } else {
        // divを生成
        $(parentDiv).append('<div id="' + id + '"></div>');
    }

    // ツリービューの初期化
    var structureGrpIdList = [structureGrpId];
    var form = $(P_Article).find("form[id^='form']");
    var conductId = $(form).find('input[name="CONDUCTID"]').val();

    var treeDef = isMultiSelect ? treeViewDef.MultiModalForm : treeViewDef.ModalForm;
    initTreeView(appPath, conductId, structureGrpIdList, null, treeDef, modal, null, null, null, treeValues);
}

/**
 * ツリービューの選択情報を設定する
 * @param {any} tree                        :ツリービュー要素またはセレクタ
 * @param {Array.<number>} structureIdList  :選択中の構成IDリスト
 * @param {boolean} isMerged                : マージ表示かどうか
 * @param {boolean} isOpenNode              : 指定階層を展開した状態で表示するかどうか
 */
function setSelectedDataToTreeView(tree, structureIdList, isMerged, isOpenNode) {
    var nodeIdList = [];
    if (!isMerged) {
        nodeIdList = getTreeViewNodeByStructureId(tree, structureIdList);
    } else {
        nodeIdList = getTreeViewNodeByStructureIdForMerge(tree, structureIdList);
    }
    if (nodeIdList.length > 0) {
        // 指定されたノードをチェック＆展開
        $(tree).jstree(true).select_node(nodeIdList, false, true);
        //$(tree).jstree(true).open_node(nodeIdList);
    } else {
        // ノードのチェックをすべて解除して展開
        $(tree).jstree(true).uncheck_all();
        if (!isOpenNode) {
            $(tree).jstree(true).open_all();
        }
    }
}

/**
 * ツリービューのノードIDを構成IDを指定して取得
 * @param {any} tree                        :ツリービュー要素またはセレクタ
 * @param {Array.<number>} structureIdList  :選択中の構成IDリスト
 * @return {Array.<string>} ノードID配列
 */
function getTreeViewNodeByStructureId(tree, structureIdList) {
    var nodeIdList = [];
    if (structureIdList != null && structureIdList.length > 0) {
        var datas = $(tree).jstree(true)._model.data;
        $.each(datas, function (idx, data) {
            //ノードの構成ID
            var structureId = getTreeViewStructureId(data);
            if (structureIdList.indexOf(structureId) >= 0 && nodeIdList.indexOf(data.id) < 0) {
                //ノードのIDを取得
                nodeIdList.push(data.id);
            }
        });
    }
    return nodeIdList;
}

/**
 * ツリービューのノードIDを構成IDを指定して取得(マージ表示用)
 * @param {any} tree                        :ツリービュー要素またはセレクタ
 * @param {Array.<number>} structureIdList  :選択中の構成IDリスト
 * @return {Array.<string>} ノードID配列
 */
function getTreeViewNodeByStructureIdForMerge(tree, structureIdList) {
    var nodeIdList = [];
    if (structureIdList != null && structureIdList.length > 0) {
        var datas = $(tree).jstree(true)._model.data;
        $.each(datas, function (idx, data) {
            //ノードの構成ID
            var idList = getTreeViewStructureIdList(data);
            if (idList != null && idList.length > 0) {
                $.each(idList, function (i, id) {
                    if (structureIdList.indexOf(id) >= 0 && nodeIdList.indexOf(data.id) < 0) {
                        //ノードのIDを取得
                        nodeIdList.push(data.id);
                    }
                });
            }
        });
    }
    return nodeIdList;
}

/**
 * 場所階層ツリーの工場階層までを展開した状態で表示
 * @param {any} grpId 構成グループID
 * @param {any} selector ツリーのID
 */
function openFactoryNode(grpId, selector) {
    // 2024/07/08 機器別管理基準標準用場所階層ツリー追加 UPD start
    //var grpIds = [structureGroupDef.Location, structureGroupDef.LocationForUserMst, structureGroupDef.LocationHistory, structureGroupDef.LocationNoHistory];
    var grpIds = [structureGroupDef.Location, structureGroupDef.LocationForUserMst, structureGroupDef.LocationHistory, structureGroupDef.LocationNoHistory, structureGroupDef.LocationForMngStd];
    // 2024/07/08 機器別管理基準標準用場所階層ツリー追加 UPD end
    if (grpIds.includes(grpId)) {
        //場所階層ツリーの場合、工場階層まで展開する

        var nodeIdList = [];
        var datas = $(selector).jstree(true)._model.data;
        //地区階層の構成IDリストを取得
        $.each(datas, function (key, data) {
            if (getTreeViewLayerNo(data) != 0) {
                return true;
            }
            if (nodeIdList.indexOf(data.id) < 0) {
                //ノードのIDを取得
                nodeIdList.push(data.id);
            }
        });
        //地区階層を展開し、工場階層が表示された状態にする
        $(selector).jstree(true).open_node(nodeIdList);
    }
}

/**
 * ストレージに保存したツリービューの選択値を取得
 * @param {number} grpId        : 構成グループID
 * @param {string} code         : ローカルストレージデータ種類
 */
function getStorageTreeData(grpId, code) {
    // セッションストレージから選択値を取得
    var selectedIdList = getSavedDataFromSessionStorage(sessionStorageCode.TreeViewSelected, grpId);
    if (selectedIdList == null || selectedIdList.length == 0) {
        // セッションストレージに存在しない場合、ローカルストレージから取得
        selectedIdList = getSavedDataFromLocalStorage(code);
    }
    return selectedIdList;
}

/**
 * ストレージに保存した選択値をツリービューに反映
 * @param {number} grpId        : 構成グループID
 * @param {string} code         : ローカルストレージデータ種類
 * @param {string} selector     : ツリービューのID
 * @param {boolean} isMerged    : マージ表示かどうか
 * @param {boolean} isOpenNode  : 指定階層を展開した状態で表示するかどうか
 */
function setStorageTreeData(grpId, code, selector, isMerged, isOpenNode) {
    // ストレージから選択値を取得
    var selectedIdList = getStorageTreeData(grpId, code);
    setSelectedDataToTreeView(selector, selectedIdList, isMerged, isOpenNode);
}

/**
 * ツリービューの再生成
 * @param {string} appPath  :
 * @param {string} grpId    :構成グループID
 */
function refreshTreeView(appPath, conductId, grpId) {
    // (2022/11/01) 構成マスタデータをセッションストレージには保存しない
    //// セッションストレージデータを削除
    //removeSaveDataFromSessionStorage(sessionStorageCode.TreeView, grpId);
    // グローバル変数のデータを削除
    P_TreeViewJsonList[grpId] = null;
    delete P_TreeViewJsonList[grpId];

    // ツリービューの破棄
    var selector = '#' + getTreeViewId(grpId, treeViewDef.TreeMenu.Val);
    $(selector).jstree("destroy").empty();

    //★インメモリ化対応 start
    //// ツリービューの初期化
    //initTreeView(appPath, conductId, [grpId], null, treeViewDef.TreeMenu);

    //if (grpId == structureGroupDef.Location) {
    //    // 場所階層ツリーの場合、職種機種ツリーも初期化する
    //    refreshTreeView(appPath, conductId, structureGroupDef.Job);
    //}
    var grpIdList = [grpId];
    if (grpId == structureGroupDef.Location) {
        // 場所階層ツリーの場合、職種機種ツリーも初期化する
        // グローバル変数のデータを削除
        P_TreeViewJsonList[structureGroupDef.Job] = null;
        delete P_TreeViewJsonList[structureGroupDef.Job];

        // ツリービューの破棄
        var selector = '#' + getTreeViewId(structureGroupDef.Job, treeViewDef.TreeMenu.Val);
        $(selector).jstree("destroy").empty();
        grpIdList.push(structureGroupDef.Job);
    }

    // ツリービューの初期化
    initTreeView(appPath, conductId, grpIdList, null, treeViewDef.TreeMenu, null, null, null, null, null, true);
    //★インメモリ化対応 end
}

/**
 * コンボボックスの再生成
 * @param {string} appPath      :アプリケーションルートパス
 * @param {string} conductId    :機能ID
 * @param {string} grpId        :構成グループID
 */
function refreshComboBox(appPath, conductId, grpId) {
    // コンボデータをクリア
    clearSavedComboBoxData(grpId);

    var selects = $('select[data-param^="' + grpId + '"]');
    if (selects == null || selects.length == 0) {
        // 共有メモリ上のコンボボックスデータを更新
        updateComboBoxData(appPath, conductId, grpId);
    } else {
        // コンボの再作成
        resetComboBoxImpl(appPath, selects);
    }
}

/**
 * 保持しているコンボボックスデータのクリア
 * @param {number} grpId:構成グループID(未指定の場合は全コンボボックスデータ)
 */
function clearSavedComboBoxData(grpId) {
    // (2022/11/01) 構成マスタデータをセッションストレージには保存しない
    //    var keys = findSessionStorageKeys(sessionStorageCode.CboMasterData, grpId ? (',' + grpId) : null);
    //    $.each(keys, function (idx, key) {
    //        // セッションストレージデータを削除
    //        removeSaveDataFromSessionStorageByKey(key);
    //    });

    //var keys = findObjectDataKeys(P_ComboBoxJsonList, ',' + grpId);
    var keys = findObjectDataKeys(P_ComboBoxJsonList, '_' + grpId);
    $.each(keys, function (idx, key) {
        // グローバル変数データを削除
        P_ComboBoxJsonList[key] = null;
        delete P_ComboBoxJsonList[key];
    });

    //★インメモリ化対応 start
    keys = findObjectDataKeys(P_ComboBoxMemoryItems, '_' + grpId);
    $.each(keys, function (idx, key) {
        // グローバル変数の共有メモリデータをクリア(削除はしない)
        P_ComboBoxMemoryItems[key] = null;
    });
    //★インメモリ化対応 end
}

/**
 * Object配列のキーの存在チェック
 * @param {string} key  :対象キー
 * @return {boolean} true:存在している/false:存在しない
 */
function existsObjectDataKey(objectList, key) {
    return Object.keys(objectList).indexOf(key) >= 0;
}

/**
 * Object配列のキーを取得
 * @param {string} keyword  :検索キーワード
 * @return Array.<string> 検索条件に一致したキー
 */
function findObjectDataKeys(objectList, keyword) {
    var keys = [];
    const objectKeys = Object.keys(objectList);
    keys = objectKeys.filter(function (key) {
        return key.indexOf(keyword) >= 0;
    });
    return keys;
}


/**
 *  タブ切替の初期化処理
 *  @param {string} articleSelector :対象atricle要素セレクタ
 */
function initTab(articleSelector) {
    if (!articleSelector) {
        //articleSelector = 'article[name="main_area"]';
        articleSelector = 'article';
    }
    var articles = $(articleSelector);
    if (!articles || articles.length == 0) {
        return;
    }

    $.each(articles, function (idx, article) {

        var btnIds = [".tab_btn.search", ".tab_btn.detail"];
        var contentsIds = [".search_div", ".detail_div"];

        $.each(btnIds, function (idx, id) {
            // タブ切替用ボタンを取得
            //var tabBtn = $(P_Article).find(id + " a");
            //var tabBtn = $(id + " a");
            var tabBtn = $(article).find(id + " a");

            if (tabBtn.length > 0) {
                // タブ内容を取得し、ボタンクリック時のイベントハンドラをセット
                //var tabContents = $(contentsIds[idx] + " .tab_contents");
                var tabContents = $(article).find(contentsIds[idx] + " .tab_contents");

                // 選択中のタブ番号キーを取得＆選択中のタブ番号をクリア
                var tabNoKey = getSelectedFormTabNoKey(tabBtn[0]);
                clearSelectedFormTabNo(tabNoKey);

                $(tabBtn).on('click', function () {
                    // クリックされたボタンの表示区分を取得
                    var tabNo = $(this).data('tabno');

                    // 選択中のタブ番号を保存
                    tabNoKey = getSelectedFormTabNoKey(this);
                    setSelectedFormTabNo(tabNoKey, tabNo);

                    //// タブ切替用ボタンとタブ内容の選択状態を解除
                    //tabBtn = $($(tabBtn).selector);//モーダルの要素も併せて再取得
                    tabBtn = $(this).siblings($(tabBtn).selector);
                    $(tabBtn).removeClass('selected');
                    //tabContents = $($(tabContents).selector);//モーダルの要素も併せて再取得
                    tabContents = $(this).parent().siblings(".tab_contents");
                    $(tabContents).removeClass('selected');

                    // タブ切替時にフォーカスを再設定を実施(初期化)
                    removeFocus();
                    // クリックされたボタンを選択状態へセット
                    $(this).addClass('selected');
                    $.each($(tabContents), function (i, div) {
                        // クリックされたボタンの表示区分と一致するタブ内容を選択状態(表示)へセット
                        if ($(div).data('tabno') == tabNo) {
                            $(div).addClass('selected');

                            //103一覧がタブ内にある場合、再描画する
                            var redrawTable = $(div).find('div[data-ctrltype="' + ctrlTypeDef.IchiranPtn3 + '"].tabulator');
                            $.each(redrawTable, function (i, table) {
                                var tableId = $(table).attr('id');
                                if (P_listData['#' + tableId]) {
                                    P_listData['#' + tableId].redraw(true);
                                    // 【オーバーライド用関数】タブ切替時
                                    initTabOriginal(tabNo, tableId);
                                }
                            });
                        }
                    });

                    // タブ切替時にフォーカスを再設定を実施(設定)
                    nextFocus();
                });
            }
        });
    });
    articles = null;
}

/**
 *  他機能遷移リンク押下処理
 *  @param {string} appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string} transTarget ：遷移先情報（機能ID|画面NO）
 *  @param {number} formNo      ：遷移元画面NO
 *  @param {Array.<Dictionary<string, string>>}  conditionDataList   ：個別取得の遷移条件データ
 *  @param {string} ctrlId      ：遷移元の一覧ctrlid
 *  @param {string} btn_ctrlId  ：ボタンのbtn_ctrlid
 *  @param {number}    rowNo       ：遷移元の一覧の選択行番号（新規の場合は-1）
 *  @param {Element}   element     ：ｲﾍﾞﾝﾄ発生要素
 *  @param {string} target      ：window.open()のtargetタブ指定文字列（別タブ："_blank"、同タブ："_self"）
 *  @param {number} modalNo     ：現在のモーダル画面番号
*/
function transOtherTab(appPath, transDiv, transTarget, formNo, conditionDataList, ctrlId, btn_ctrlId, rowNo, element, target, modalNo) {

    var urlParam = transTarget.replace(/'/g, "").split("|");
    if (isScheduleLink(btn_ctrlId)) {
        // スケジュールリンクの場合、先頭にスケジュール種類が付加されているため削除
        urlParam = urlParam.slice(1);
    }
    var conductId = urlParam[0];
    // APより転記　他機能遷移先を動的に取得　列の定義で@5とすることでVAL5に記載の機能へ遷移
    // 「@」が付与されている場合、置き換える
    if (conductId.match(/@/)) {
        //    var tr = $(element).closest("tr");
        //    var td = $(tr).find("td[data-name='VAL" + conductId.replace("@", "") + "']");
        //    conductId = getCellVal(td);
        conductId = $(element).text();
    }

    var formNo = urlParam[1];

    //遷移先の戻る時再検索用退避条件(P_SearchCondition)を作成
    /*ﾃｽﾄ切替 start*/
    //var dicBackConditions = {};
    //var conList = [];
    //var conData = {};
    //conData.CTRLID = "Condition";
    //conData.FORMNO = 0;
    //conData.ROWNO = 1;
    //conData.VAL1 = "JU00010061";
    //conData.VAL2 = "";
    //conData.VAL3 = "";
    //conData.VAL4 = "";
    //conData.VAL5 = "";
    //conData.VAL6 = "";
    //conData.VAL7 = "";
    //conData.VAL8 = "";
    //conData.VAL9 = "";
    //conData.VAL10 = "";
    //conData.VAL11 = "";
    //conData.VAL12 = "";
    //conData.VAL13 = "";
    //conData.VAL14 = "";
    //conData.VAL15 = "|";
    //conData.VAL16 = "|";
    //conData.VAL17 = "|";
    //conData.VAL18 = "|";
    //conData.VAL19 = "";
    //conData.VAL20 = "";
    //conData.VAL21 = "";
    //conData.lockData = "";
    //conList.push(conData);
    //dicBackConditions['0'] = JSON.stringify(conList);
    /*ﾃｽﾄ切替↑↓ =====*/
    var dicBackConditions = createParamTransOtherBack(appPath, transTarget, conductId, formNo, ctrlId, btn_ctrlId, rowNo, element);
    /*ﾃｽﾄ切替 end*/

    // 遷移先URL
    var url = appPath + 'Common/Index';
    //window.open('', 'otherTab');  //←ﾃﾞﾊﾞｯｸﾞ時有効化（別タブ）
    var form = '<form id="postForm" style="display:none;">';
    form += '<input type="hidden" name="CONDUCTID" value="' + conductId + '">';
    form += '<input type="hidden" name="FORMNO" value="' + formNo + '">';
    form += '<input type="hidden" name="TRANSACTIONDIV" value="' + transDiv + '">';
    form += '<input type="hidden" name="ConditionData" value="">';
    form += '<input type="hidden" name="ListIndividual" value="">';
    form += '<input name="__RequestVerificationToken" type="hidden" value="' + getRequestVerificationToken() + '">';
    form += '</form>';
    $(form).appendTo("body");
    if (conditionDataList && conditionDataList.length > 0) {
        $("#postForm").find("input:hidden[name='ConditionData']").val(JSON.stringify(conditionDataList));
    }
    if (dicBackConditions && dicBackConditions.length > 0) {
        $("#postForm").find("input:hidden[name='ListIndividual']").val(JSON.stringify(dicBackConditions));

    }
    $("#postForm").attr("action", appPath + 'Common/Index?TRANSFLG=1');
    $("#postForm").attr("method", "POST");
    //$("#postForm").attr("target", "otherTab");    //←ﾃﾞﾊﾞｯｸﾞ時有効化（別タブ）
    //$("#postForm").attr("target", "_self");       //←ﾃﾞﾊﾞｯｸﾞ時有効化（同タブ）
    $("#postForm").attr("target", target);          //←ﾃﾞﾊﾞｯｸﾞ時無効化
    $("#postForm").submit();
    $("#postForm").remove();
}

/**
 * 他機能遷移先初期化時用　戻る時再検索用退避条件(P_SearchCondition)保持
 * @param {Dictionary<string, object>} backConditions
 */
// backConditionsを$("#main_contents > div.init_temp_data")のinput:hiddenタグでJSON文字列で受け取るよう修正
//function retainSearchConditionsForOtherTab(backConditions) {
function retainSearchConditionsForOtherTab() {

    var input = $("#main_contents > div.init_temp_data input:hidden[name='ListIndividual_Temp']");

    //デシリアライズ
    var backConditions = deserializeInputJsonText(input);

    // hiddenタグの削除
    $(input).remove();

    P_SearchCondition = backConditions;
}

///**
// * 他機能遷移時のパラメータ作成
// * 
// */
//function createParamTransOther(linkParam, selector) {

//    var conditionData = {}
//    var conditionDataList = [];

//    if (linkParam != null && linkParam.length > 0) {

//        // パラメータの成形(例：OrderNo=@1,DetailNo=@2)
//        var params = (linkParam + '').split(',');   //ｶﾝﾏで分解

//        //対象行：trを取得する
//        var tr = $(selector).closest('tr');

//        // 列指定ﾊﾟﾗﾒｰﾀ（「@」で始まる引数）の値の置き換え
//        $.each(params, function (i, param) {
//            var colNo = -1;
//            var startNum = -1;
//            var paramVal = "";

//            //「@」開始位置を取得する
//            startNum = this.indexOf("@");

//            if (startNum >= 0) {
//                //列指定ﾊﾟﾗﾒｰﾀ（「@」で始まる引数）の場合

//                //列番号を取得
//                colNo = parseInt(this.substr(startNum + 1), 10);  //「@」を除去してcolNoとする

//                //対象列の値を取得

//                //対象ｾﾙ：tdを取得する
//                var td = getDataTd(tr, colNo);
//                paramVal = $(td)[0].innerText;
//                //paramVal = $(td).text();

//                //※文字列指定でないﾊﾟﾗﾒｰﾀの場合、ﾊﾟﾗﾒｰﾀ値未設定の場合は「null」を設定
//                if (paramVal == null || paramVal.length <= 0) {
//                    //※ﾊﾟﾗﾒｰﾀ値未設定の場合

//                    //文字列指定でないﾊﾟﾗﾒｰﾀか？
//                    var pos = param.indexOf('@' + colNo);
//                    if (pos == 0) {
//                        paramVal = "null";
//                    }
//                    else if (pos > 0) {
//                        //「@」直近文字列が「'」でないか？
//                        if (param.substr(pos - 1, 1) != "'") {
//                            paramVal = "null";
//                        }
//                    }
//                }
//                param = param.replace('@' + colNo, paramVal);    //「@1」⇒「ｾﾙの値」で置き換え
//            }
//            // conditionData生成
//            var paramStr = (param + '').split('=');   //=で分解
//            if (paramStr.length > 1) {
//                var paramKey = paramStr[0];
//                var paramVal = paramStr[1];
//                conditionData[paramKey] = paramVal;
//            }
//        });
//    }
//    conditionDataList.push(conditionData);

//    //// Hidden項目にJSON文字列でセット
//    //$(P_Article).find("form[id^='formDetail'] input:hidden[name='ConditionData']").val(JSON.stringify(conditionDataList));

//    return conditionDataList;
//}

//★AKAP2.0標準
/**
 *  次行へフォーカスを移動する
 *  @param {select} ：選択要素
 */
function nextForcus(selector) {

    // ﾚｲｱｳﾄ行のselect要素から列番号を取得
    // selector:select > td
    var key = $(selector).closest('td').data("name");   // VAL1～

    // ﾃﾞｰﾀ行を取得
    // selector:select > td > tr
    var tr_cur = $(selector).closest('tr');     // 現在行
    var tr_next = $(tr_cur).next();             // 次行

    //if (tr_next.length > 0) {
    //    // 次行の同列へフォーカスを移動する
    //    var tds = $(tr_next).find("[data-name='" + key + "']");
    //    $.each(tds, function (index, td) {
    //        $(td).find('input, textarea, select').first().focus();
    //        return;
    //    });
    //}
    while (true) {
        var flg = false;
        if (tr_next.length > 0) {
            // 次行の同列へフォーカスを移動する
            var tds = $(tr_next).find("[data-name='" + key + "']");
            $.each(tds, function (index, td) {
                $(td).find('input, textarea, select').first().focus();
                flg = true;
                return;
            });
        } else {
            // 処理を抜ける
            flg = true;
        }
        if (flg) {
            // 処理を抜ける
            break;
        }
        // 次行を参照
        tr_next = $(tr_next).next();
    }

}
//★AKAP2.0標準

/**
 * ﾌﾟｯｼｭ通知件数表示
 * @param {number} : 件数
 */
function dispPushCount(count) {

    var iconBell = $("#app_navi_icon").find("a[data-switchid='app_navi_notice'] span"); // ﾍﾞﾙｱｲｺﾝ
    if (count != null && count != 0) {
        // 件数が99件以上の場合、省略表示
        if (count > 99) count = "99+";
        var countHtml = "<div class='count'>" + count + "</div>";
        $(iconBell).css("position", "relative");
        // ﾍﾞﾙｱｲｺﾝの子要素として挿入
        $(countHtml).appendTo(iconBell);
        //ｾｯｼｮﾝｽﾄﾚｰｼﾞに件数保持
        sessionStorage.setItem("PushCount", count + "");
    }
    else {
        // 件数なしの場合
        var countHtml = $(iconBell).find("div.count");
        if ($(countHtml).length) {
            // 件数要素を削除
            $(countHtml).remove();
        }
        //ｾｯｼｮﾝｽﾄﾚｰｼﾞ削除
        var count = sessionStorage.getItem("PushCount");
        if (count != null && count.length > 0) {
            sessionStorage.removeItem("PushCount");
        }
    }
}

/**
 * ﾒﾆｭｰ選択時 ﾌﾟｯｼｭ通知件数をｾｯｼｮﾝｽﾄﾚｰｼﾞから取得
 */
function initPushCount() {
    var count = sessionStorage.getItem("PushCount");
    if (count != null && count.length > 0) {
        dispPushCount(count);
    }
}

/**
 * 双方向通信接続確立 - ﾕｰｻﾞｰID取得
 * @appPath     {string} :ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 * @conductId   {string} :機能ID
 */
function initSignalR(appPath, conductId) {
    // ユーザーIDを取得
    // POSTデータを生成
    var postdata = {
        conductId: conductId,           // メニューの機能ID
    };

    // 登録処理実行
    $.ajax({
        url: appPath + 'api/CommonProcApi/' + actionkbn.ComGetUserId,    // 【共通 - 双方向通信】ユーザーID取得
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(postdata),
        headers: { 'X-XSRF-TOKEN': getRequestVerificationToken() },
        traditional: true,
        cache: false
    }).then(
        // 1つめは通信成功時のコールバック
        function (resultInfo) {
            //正常時
            //[0]:処理ステータス - CommonProcReturn
            //[1]:ユーザーID
            var status = resultInfo[0];
            var userId = resultInfo[1];

            // SignalR
            establishSignalRConnection(userId);

        },
        // 2つめは通信失敗時のコールバック
        function (resultInfo) {
            //異常時

            //処理結果ｽﾃｰﾀｽ(CommonProcReturn)
            var status = resultInfo.responseJSON;
            var eventFunc = function () {
                initSignalR(appPath, conductId);
            }

            //処理結果ｽﾃｰﾀｽを画面状態に反映
            if (!setReturnStatus(appPath, status, eventFunc)) {
                return false;
            }
        }
    );

}

/**
 * 双方向通信接続確立
 * @groupName   {string} : ﾕｰｻﾞｰID(参加ｸﾞﾙｰﾌﾟ名)
 */
function establishSignalRConnection(groupName) {

    var pushConnection = $.connection.push;

    // サーバー側から
    pushConnection.client.pushNotify = function (count) {
        dispPushCount(count);
    };

    // 接続を確立し、自ﾕｰｻﾞｰIDをｸﾞﾙｰﾌﾟ名としてｸﾞﾙｰﾌﾟに参加
    $.connection.hub.start().done(function () {
        pushConnection.server.joinMyGroup(groupName);
    });
}

/**
 * ﾎﾞﾀﾝ権限情報をﾊﾟﾌﾞﾘｯｸ変数に保持
 * @param {List<Dictionary<string, object>>} btnAuthList : ﾎﾞﾀﾝ権限情報
 */
// dicBtnAuthListを引数ではなく、$("#main_contents > div.init_temp_data")のinput:hiddenタグからJSON文字列で受け取るよう修正
//function retainButtonDefines(dicBtnAuthList) {
function retainButtonDefines(input) {
    if (!input) {
        input = $("#main_contents > div.init_temp_data input:hidden[name='UserAuthConductShoris_Temp']");
    }
    //デシリアライズ
    var dicBtnAuthList = deserializeInputJsonText(input);
    if (dicBtnAuthList == null) {
        dicBtnAuthList = {};
    }

    // hiddenタグの削除
    $(input).remove();

    if (!Object.keys(P_buttonDefine).length) {
        P_buttonDefine = dicBtnAuthList;
    } else {
        $.each(dicBtnAuthList, function (key, value) {
            P_buttonDefine[key] = value;
        });
    }
}

/**
 * 共通メッセージ、ボタンメッセージの翻訳をパブリック変数に保持
 * @param  {Dictionary<string, string>} dicTranslated : 翻訳済文言の連想配列(key:翻訳ID, value:翻訳済文言)
 */
//dicTranslatedを引数ではなく、$("#main_contents > div.init_temp_data")のinput:hiddenタグからJSON文字列で受け取るよう修正
//function retainCommonMessages(dicTranslated) {
function retainCommonMessages() {
    var input = $("#main_contents > div.init_temp_data input:hidden[name='TransDictionary_Temp']");
    //デシリアライズ
    var dicTranslated = deserializeInputJsonText(input);
    if (dicTranslated == null) {
        dicTranslated = {};
    }
    // hiddenタグの削除
    $(input).remove();

    P_ComMsgTranslated = dicTranslated;
}

/**
 * 1ページ当たりの行数コンボの内容をパブリック変数に保持
 * @param {int[]} values : 1ページ当たりの行数の配列 
 */
function retainPageRowsCombo(values) {
    P_PageRowsCombo = values;
}

/**
 * 言語コンボの内容をパブリック変数に保持、言語コンボの生成
 * @appPath     {string} :ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 * @param {List<Dictionary<string, string>>} values : 言語の連想配列(key:言語コード(ja,en), value:翻訳済言語ラベル(日本語、英語))
 */
function retainLanguageCombo(appPath, values) {
    P_LanguageCombo = values;

    //言語コンボ要素
    var select = $("#language_select");

    //オプション追加
    var keys = Object.keys(values);
    $.each(keys, function (i, key) {
        var option = $('<option>').val(key).html(P_LanguageCombo[key]);
        $(select).append($(option));
    });

    //ユーザの言語
    var defaultLanguage = $(select).data("userlanguage");
    if (defaultLanguage) {
        //ユーザの言語に合わせて選択
        $(select).val(defaultLanguage);
    }

    $(select).on('change', function () {

        var eventFunc = function () {
            // 画面の情報を取得
            var conductId = $(P_Article).find("input:hidden[name='CONDUCTID']").val();
            var pgmId = $(P_Article).find("input:hidden[name='PGMID']").val();
            var formNo = $(P_Article).find("input:hidden[name='FORMNO']").val();
            //ﾎﾞﾀﾝ権限情報
            var btnDefines = P_buttonDefine[conductId];
            //選択した言語
            var selectLanguage = $(select).val();

            // POSTデータを生成
            var postdata = {
                conductId: conductId,           // メニューの機能ID
                pgmId: pgmId,                   // メニューのプログラムID
                formNo: formNo,                 // フォーム番号
                //ctrlId: "ChangeLanguage",         // コントロールID
                //confirmNo: confirmNo,           //確認ﾒｯｾｰｼﾞ番号
                buttonDefines: btnDefines,  //ﾎﾞﾀﾝ権限情報　※ﾎﾞﾀﾝｽﾃｰﾀｽを取得
                browserTabNo: P_BrowserTabNo,   // ブラウザタブ識別番号
                selectLanguageId: selectLanguage, //選択した言語
            };

            // 初期化処理実行
            $.ajax({
                url: appPath + 'api/CommonProcApi/' + actionkbn.ChangeLanguage,    // 言語切り替え
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json',
                data: JSON.stringify(postdata),
                headers: { 'X-XSRF-TOKEN': getRequestVerificationToken() },
                traditional: true,
                cache: false
            }).then(
                // 1つめは通信成功時のコールバック
                function (resultInfo) {
                    //var status = resultInfo[0];                                 //[0]:処理ステータス - CommonProcReturn
                    //var data = separateDicReturn(resultInfo[1], conductId);     //[1]:結果データ - Dictionary<string, object>※結果ﾃﾞｰﾀ："Result"、個別実装用ﾃﾞｰﾀ："Individual"

                    //セッションストレージのキー一覧を取得
                    var keyList = [];
                    for (i = 0; i < sessionStorage.length; i++) {
                        keyList.push(sessionStorage.key(i));
                    }
                    //セッションストレージに保存している構成マスタの情報をクリアする
                    $.each(keyList, function (i, key) {
                        if (key.startsWith(sessionStorageCode.TreeView) || key.startsWith(sessionStorageCode.CboMasterData)) {
                            sessionStorage.removeItem(key.toString());
                        }
                    });

                    var menuLink = $("#top_menu, #side_menu").find("a[data-conductid='" + conductId + "']");
                    if (menuLink && menuLink.length > 0) {
                        //機能の最初の画面を起動
                        $(menuLink).click();
                    } else {
                        //TOP画面を起動
                        $('a[data-actionkbn="' + actionkbn.ComRedirectTop + '"]').click();
                    }
                },
                // 2つめは通信失敗時のコールバック
                function (resultInfo) {
                    //異常時

                    //処理結果ｽﾃｰﾀｽ(CommonProcReturn)
                    var status = resultInfo.responseJSON;
                    //処理継続用ｺｰﾙﾊﾞｯｸ関数を生成
                    var confirmNo = 0;
                    if (!isNaN(status.LOGNO)) {
                        confirmNo = parseInt(status.LOGNO, 10);
                    }

                    //処理結果ｽﾃｰﾀｽを画面状態に反映
                    if (!setReturnStatus(appPath, status)) {
                        return false;
                    }

                }
            ).always(
                //通信の完了時に必ず実行される
                function (resultInfo) {
                    //処理中メッセージ：off
                    processMessage(false);
                });

        }
        //確認メッセージを設定
        //『画面の編集内容は破棄されます。よろしいですか？』
        P_MessageStr = P_ComMsgTranslated[941060005];
        // 確認メッセージを表示
        if (!popupMessage([P_MessageStr], messageType.Confirm, eventFunc)) {
            //『キャンセル』の場合、処理中断
            return false;
        }
    });
}

/**
 * Tablator一覧の表示
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {string} ：機能ID
 *  @param {string} ：プログラムID
 *  @param {number} ：画面番号
 *  @param {object} ：検索結果
 *  @param {select} ：ヘッダー
 */
function dispTabulatorListData(appPath, conductId, pgmId, formNo, data, tabulatorHeader) {
    const ctrlId = data[0].CTRLID;
    var id = "#" + ctrlId + '_' + formNo;

    // 詳細検索条件適用状況の設定
    setConditionAppliedStatus(id, data[0]);

    // 読込件数エリアの総件数の設定
    setSelectAllCntLabel(ctrlId, data);

    var header = tabulatorHeader.data('header');

    // スケジュール列の定義をクリア
    // ※本来はprevSetTabulatorHeader()内で関数呼び出し等によって対応することが望ましいが、
    // ※ここに直接処理を書かなければ意図した結果が得られなかったため、ここに記載する
    header = header.filter((item) => {
        return (!item.field || item.field.indexOf('Y') != 0);
    });

    //入れ子テーブルのコントロールIDリスト
    var subTableIdList = [];
    // 【オーバーライド用関数】Tabuator一覧のヘッダー描画前処理
    prevSetTabulatorHeader(appPath, id, header, tabulatorHeader, subTableIdList);

    // 表示データ
    var dispData = data.slice(1);//先頭行削除
    // 一覧編集パターン
    var editptn = $(id).data('editptn');
    // 表示モード
    var referenceMode = $(id).data('referencemode');

    var table = Tabulator.findTable(id)[0];
    if (table) {
        //列フィルターの内容を取得
        delete P_colFilterData[id];
        P_colFilterData[id] = table.getHeaderFilters();

        table.destroy();
        table = null;
        delete P_listData[id];
        delete P_deleteData[id];
    }

    //Tabulator全体幅の調整
    //TODO:幅指定すると列が固定されない？
    var divWidth = 2;
    $.each(header, function (idx, head) {
        // 列ヘッダーフィルターのコンボ項目の更新
        if (head.headerFilter == 'list') {
            //ベース用のテーブルからセレクトボックス要素を取得
            var select = $('table' + id + '_tablebase' + ' tbody td[data-name="' + head.field + '"] select');
            var selects = $(select).find('option');
            var headerOptions = { "": "" };
            $.each(selects, function (i, option) {
                var val = $(option).val();
                var text = $(option).text();
                headerOptions[val] = text;
            });
            selects = null;
            select = null;
        }

        if (head.columns) {
            // ヘッダ複数行の場合
            $.each(head.columns, function (idx, lowerRow) {
                divWidth += setHeader(lowerRow, id, editptn, referenceMode, appPath);
            });
        } else {
            divWidth += setHeader(head, id, editptn, referenceMode, appPath);
        }
    });
    //var articleWidth = $(id).closest('article').width();
    //if (articleWidth > divWidth) {
    //    //$(id).css('width', divWidth + 'px');
    //    $(id).width(divWidth);
    //}
    var paginationElement = $(P_Article).find('.' + ctrlId + '.tabulatorPagination')[0];
    //var paginationCounterElement = $(P_Article).find('.' + ctrlId + '.tabulatorPaginationCounter')[0];
    $(paginationElement).empty();

    // ローカルストレージからソート順を取得
    var sortOrder = getSavedDataFromLocalStorage(localStorageCode.SortOrder, conductId, formNo, ctrlId);

    // 照合順序の仕様確認に合わせ、Tabulatorのフィルタ処理を調整する。
    var headerFilter = function (headerValue, rowValue, rowData, filterParams) {
        var result = isMatchCharacterNoDiff(rowValue, headerValue, false);
        return result;
    }

    var isHeaderDisp = $(id).data('headerdispkbn');
    var tableOptions = {
        data: dispData,
        index: "ROWNO",
        //データをツリー化
        dataTree: true,
        //ツリー上のすべてのノードを閉じた状態で表示
        dataTreeStartExpanded: false,  // [true, false]最初のレベルを展開、2番目のレベルを折りたたむ
        //ツリーの階層表示はNoリンク列に表示
        dataTreeElementColumn: "ROWNO",
        // TODO:テーブル列幅以上のウィンドウサイズになると右側に黒枠が出る
        //layout: "fitColumns",
        // TODO:列幅に合わせた枠にしてくれるが、左右のスクロールの位置が変
        //layout:"fitDataTable",
        //ヘッダー表示/非表示
        headerVisible: isHeaderDisp ? isHeaderDisp : true,
        //列移動可否
        //movableColumns: true,
        //行移動可否
        //movableRows: true,
        //ヘッダー情報
        columns: header,
        //ソートは1列のみとする
        columnHeaderSortMulti: false,
        // ソート順初期値
        initialSort: sortOrder,
        //フッターに配置されているページングを一覧の上に移動
        paginationElement: paginationElement,
        //オプションの警告をコンソールに表示しない
        debugInvalidOptions: false,
        //コンポーネント関数の警告をコンソールに表示しない
        debugInvalidComponentFuncs: false,
        //初期化の警告をコンソールに表示しない
        debugInitialization: false,
        //非推奨オプションの警告をコンソールに表示しない
        debugDeprecation: false,
        columnDefaults: {
            //列の最小幅（デフォルトが40の為、小さめに設定）
            minWidth: 10,
            //ヘッダソートで元の順序に戻すオプション追加（昇順、降順、元の順序の3パターン）
            headerSortTristate: true,
            //列フィルターのプレースホルダ（全列）
            headerFilterPlaceholder: "",//デフォルトは「filter column...」
            //ヘッダーのツールチップ
            headerTooltip: true,
            //データのツールチップ
            tooltip: function (e, cell, onRendered) {
                //getValueはコード値が表示されてしまうので、表示されているテキストを取得
                //return cell.getValue();
                return $(cell.getElement()).text();
            },
        },
        ////デフォルトの文言を設定（ページングのボタン等）
        //langs: {
        //    "default": {
        //        "pagination": {
        //            "page_size": P_ComMsgTranslated[111290001],//ページ件数
        //            "page_title": "",//ページボタンのツールチップ
        //            "first": "<<",//先頭ページボタン
        //            "first_title": "",//先頭ページボタンのツールチップ
        //            "last": ">>",//後尾ページボタン
        //            "last_title": "",//後尾ページボタンのツールチップ
        //            "prev": "<",//前ページボタン
        //            "prev_title": "",//前ページボタンのツールチップ
        //            "next": ">",//次ページボタン
        //            "next_title": "",//次ページボタンのツールチップ
        //        },
        //    }
        //},

        // ヘッダー固定はデフォルト

        rowFormatter: function (row) {
            var data = row.getData();
            var rowNo = data.ROWNO;

            //削除行は非表示
            if (data.ROWSTATUS == rowStatusDef.Delete) {
                row.getElement().style.display = "none";
            }

            //入れ子テーブルの設定
            if (subTableIdList != null && subTableIdList.length > 0) {
                var subTableId = subTableIdList[0];
                var subId = subTableId + '_' + rowNo;

                if ($(row.getElement()).find("#" + subId + '_' + formNo).length > 0) {
                    //子テーブルが生成されている場合はスキップ
                    return;
                }

                var holderEl = $('<div>');
                var tableEl = $('<div>');

                //スタイル設定
                $(holderEl).css({
                    'display': 'none',
                    'box-sizing': 'border-box',
                    'padding': '10px 30px 10px 10px',
                });
                $(holderEl).addClass("subTable_" + subId);

                setAttrByNativeJs(tableEl, "id", subId + '_' + formNo);
                $(tableEl).css({
                    'display': 'none',
                });
                $(tableEl).addClass("subTable subTable_" + subId);

                $(holderEl).append(tableEl);
                $(row.getElement()).append(holderEl);

                //子テーブルのデータ
                var subData = $.extend([], data.SubData);
                if (!subData) {
                    subData = [];
                }
                //先頭にCTRLIDのデータを追加する
                subData.unshift({ CTRLID: subId });
                //子テーブルのヘッダー情報
                var subHeader = $(P_Article).find('.' + subTableId + '.tabulatorHeader');
                //子テーブルの作成
                dispTabulatorListData(appPath, conductId, pgmId, formNo, subData, subHeader);
            }
        },
    };

    // 1ページに表示する行数
    var pageSize = $(id).data('pagerows');
    // ローカルストレージに保存されている場合はそちらの値を使用する
    var pageSizeLocal = getTabulatorPageSize(conductId, formNo, id);
    if (pageSizeLocal > 0) {
        pageSize = pageSizeLocal;
    }
    if (pageSize > 0) {
        //ページング設定
        tableOptions["pagination"] = true;
        ////行カウンター　"Showing X-X of X rows"
        ////tableOptions["paginationCounter"] = "rows";
        ////行カウンターの文言をカスタマイズ
        //tableOptions["paginationCounter"] = function (pageSize, currentRow, currentPage, totalRows, totalPages) {
        //    var lastRow = currentRow + pageSize > totalRows ? totalRows : currentRow + pageSize;
        //    //全{0}件中 {1}件目 ～ {2}件目表示
        //    return getMessageParam(P_ComMsgTranslated[911140002], [totalRows, currentRow, lastRow]);
        //}
        //フッターに配置されている行カウンターを一覧の上に移動
        //tableOptions["paginationCounterElement"] = paginationCounterElement;//"#tabulatorPaginationCounter";

        // 1ページ当たりの行数
        var pageRows = P_PageRowsCombo; // DBより取得した内容
        if (pageRows == null || pageRows.length == 0) {
            // 存在しない場合、デフォルトとする(通常は発生しないはず)
            pageRows = true;
        } else {
            if (P_PageRowsCombo.indexOf(pageSize) < 0) {
                // ページ行数コンボにページ行数が無い場合、コンボの先頭をページ行数とする
                // ※ページ行数がコンボに存在しない場合、コンボにページ行数が追加されてしまう
                pageSize = P_PageRowsCombo[0];
            }
        }

        tableOptions["paginationSize"] = pageSize;
        tableOptions["paginationSizeSelector"] = pageRows; //[5, 10, 20, 50, 100];

        //デフォルトの「Page Size」ラベルを削除し、文言を置き換える
        var dispPageSizeStr = $(paginationElement).data('pagestr');
        $(paginationElement).prepend('<span class="pagesize">' + dispPageSizeStr + '</span>');
    }

    // 【オーバーライド用関数】Tabuator一覧の描画前処理
    prevCreateTabulator(appPath, id, tableOptions, header, dispData);

    var table = new Tabulator(id, tableOptions);

    P_listData[id] = table;
    P_deleteData[id] = [];

    //clearHrefForTablator();

    //tabulatorのイベント設定
    setTabulatorEvent(appPath, conductId, pgmId, formNo, ctrlId, table, id, paginationElement, editptn, referenceMode);
    table = null;
    header = null;
}

/**
 * Tablator使用一覧のリンクの設定
 */
//function clearHrefForTablator() {
//    // リンクのhrefの値を削除、セルのクリックイベントを実行するようにする
//    var tabulatorLink = $('.ctrlId_parent .tabulator-cell>a');
//    $(tabulatorLink).attr('href', '');
//    $(tabulatorLink).on('click', function (event) {
//        event.preventDefault();
//    });
//}

/**
 * Tabulatorの全体幅の調整
 *  @param {string} ：セレクタ
 */
function resizeColumn(id) {
    //tabulatorの幅調整
    var headCol = $(id).find('.tabulator-headers .tabulator-col:visible').not('.tabulator-col-group');
    //Tabulator全体幅
    var resizeWidth = 2;
    $.each(headCol, function (idx, col) {
        var colWidth = $(col).css('width');
        if (colWidth) {
            colWidth = colWidth.slice(0, -2);//'px'削除
            resizeWidth += parseInt(colWidth);
        }
    });
    $(id).css('width', resizeWidth + 'px');
}

/**
 * Tabulatorの各列の設定
 *  @param {object} ：ヘッダー
 *  @param {string} ：テーブルID
 *  @param {string} ：一覧編集パターン
 *  @param {string} ：表示モード
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 */
function setHeader(head, id, editptn, referenceMode, appPath) {
    if (!head.mutator) {
        head.mutator = function (value, data, type, params, component) {
            if (value == null && data[component.getField()] != null) {
                //update等で特定の列を更新した際に他の列が空に変換されないよう対策
                value = data[component.getField()];
            } else if (value == null) {
                //NULLを空文字に変換（NULLが含まれるとソートが機能しなくなるため）
                value = "";
            }

            return value;
        }
    }

    if (head.headerFilterFunc && typeof head.headerFilterFunc != 'function' && head.headerFilterFunc != FilterUseKbnDef.None) {
        //列フィルターのマッチタイプ（部分一致or完全一致）
        var isEqual = false;
        if (head.headerFilterFunc == FilterUseKbnDef.ExactMatch) {
            //列フィルターが完全一致の場合
            isEqual = true;
        }
        // 照合順序の仕様確認に合わせ、Tabulatorのフィルタ処理を調整する。
        var headerFilter = function (headerValue, rowValue, rowData, filterParams) {
            var result = isMatchCharacterNoDiff(rowValue, headerValue, isEqual);
            return result;
        }
        head.headerFilterFunc = headerFilter;
    }

    if (head.formatter && head.formatter == "link") {
        //NOリンク列の場合
        setHeaderNoLink(head, id, editptn, referenceMode, appPath);
    } else if (head.formatter && head.formatter == "fileLink") {
        //ファイル参照リンク列、ダウンロードリンク列
        setHeaderFileLink(head, id, editptn, referenceMode, appPath);
    } else if (head.formatter && head.formatter == "transLink") {
        //その他（遷移、ポップアップ）のリンク列
        setHeaderTransLink(head, id, editptn, referenceMode, appPath);
    }

    if (head.formatter == "SELTAG") {
        //選択チェックボックス列
        setHeaderSeltag(head, id, editptn, referenceMode, appPath);
    }

    if (head.formatter == "UPDTAG") {
        //更新列
        setHeaderUpdtag(head, id, editptn, referenceMode, appPath);
    }

    if (head.formatter && head.formatter == "button") {
        //ボタン
        setHeaderButton(head, id, editptn, referenceMode, appPath);
    }

    if (head.formatter && head.formatter.toString().indexOf("multiCheckBox") == 0) {
        //複数選択チェックボックス
        setHeaderMultiCheckBox(head, id, editptn, referenceMode, appPath);
    }

    if (head.formatter && head.formatter.toString().indexOf("checkBox") == 0) {
        //チェックボックス
        setHeaderCheckBox(head, id, editptn, referenceMode, appPath);
    }

    if (head.formatter && head.formatter.toString().indexOf("selectBox") == 0) {
        //セレクトボックス
        setHeaderSelectBox(head, id, editptn, referenceMode, appPath);
    }

    if (head.formatter && head.formatter.toString().indexOf("radioButton") == 0) {
        //ラジオボタン
        setHeaderRadioButton(head, id, editptn, referenceMode, appPath);
    }

    if (head.formatter && head.formatter.toString().indexOf("number") == 0) {
        //数値
        setHeaderNumber(head, id, editptn, referenceMode, appPath);
    }

    if (head.formatter && head.formatter.toString().indexOf("text") == 0) {
        //テキスト、日付(DatePicker)、時刻(TimePicker)、日時(DateTimePicker)、テキストエリア、コード＋翻訳
        setHeaderText(head, id, editptn, referenceMode, appPath);
    }

    if (head.formatter && head.formatter.toString().indexOf("datetime") == 0) {
        //日時（ブラウザ標準）
        setHeaderDatetime(head, id, editptn, referenceMode, appPath);
    }

    if (head.formatter && head.formatter.toString().indexOf("date") == 0) {
        //日付（ブラウザ標準）
        setHeaderDate(head, id, editptn, referenceMode, appPath);
    }

    if (head.formatter && head.formatter.toString().indexOf("time") == 0) {
        //時刻（ブラウザ標準）
        setHeaderTime(head, id, editptn, referenceMode, appPath);
    }

    // カスタム列のTabulatorヘッダー設定処理
    if (typeof setCustomTabulatorHeader == 'function') {
        const formNo = getFormNo();
        setCustomTabulatorHeader(formNo, id, head);
    }

    // 列幅
    var divWidth = 0;
    if (head.width && head.visible) {
        divWidth = parseInt(head.width, 10);
    }
    return divWidth;
}

/**
 * TabulatorのNoリンク列の設定
 *  @param {object} ：ヘッダー
 *  @param {string} ：テーブルID
 *  @param {string} ：一覧編集パターン
 *  @param {string} ：表示モード
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 */
function setHeaderNoLink(head, id, editptn, referenceMode, appPath) {
    //NOリンク列の場合
    head.editor = "";
    // パイプ区切りで分割し、呼び出す関数に渡す引数を取得
    if (head.cellClickVal) {
        var splitStr = head.cellClickVal.split('|');

        head.formatter = function (cell, formatterParams, onRendered) {
            var row = cell.getRow();
            //var rowNo = row.getCell('ROWNO').getValue();
            var rowNo = row.getData().ROWNO;
            //var confirmFlg = splitStr[8].toLowerCase() === 'true'; //boolean型に変換
            var confirmIdx = (splitStr.length < 10) ? 8 : 9;
            var confirmFlg = splitStr[confirmIdx].toLowerCase() === 'true'; //boolean型に変換
            var icon = head.formatterParams && head.formatterParams.label ? head.formatterParams.label : "";
            //var linkHtml = '<a onclick="confirmScrapBeforeTrans(\'' + splitStr[0] + '\', \'' + splitStr[1] + '\', \'' + splitStr[2] + '\', \'' + splitStr[3] + '\', ' + parseInt(splitStr[4], 10) + ',\'' + splitStr[5] + '\',\'' + splitStr[6] + '\',' + parseInt(rowNo, 10) + ',\'' + splitStr[7] + '\',' + confirmFlg + '); return false;" href="" data-type="transLink">';
            var linkHtml = '<a onclick="confirmScrapBeforeTrans(\'' + splitStr[0] + '\', \'' + splitStr[1] + '\', ' + parseInt(splitStr[2], 10) + ', \'';
            if (splitStr.length < 10) {
                linkHtml += splitStr[3] + '\', \'' + splitStr[4] + '\', ' + parseInt(splitStr[5], 10) + ',\'' + splitStr[6] + '\',\'' + splitStr[7] + '\',' + parseInt(rowNo, 10) + ',this,' + confirmFlg + '); return false;" href="#" data-type="transLink">';
            } else {
                linkHtml += splitStr[3] + '|' + splitStr[4] + '\', \'' + splitStr[5] + '\', ' + parseInt(splitStr[6], 10) + ',\'' + splitStr[7] + '\',\'' + splitStr[8] + '\',' + parseInt(rowNo, 10) + ',this,' + confirmFlg + '); return false;" href="#" data-type="transLink">';
            }
            if (icon) {
                //ペンアイコン、矢印アイコン
                //linkHtml += icon == iconKbnDef.PencilIcon ? '<span class="glyphicon glyphicon-pencil"></span>' : '<span class="glyphicon glyphicon-circle-arrow-right"></span>';
                //ペンアイコンはファイルアイコンに変更
                linkHtml += icon == iconKbnDef.PencilIcon ? '<span class="glyphicon glyphicon-file"></span>' : '<span class="glyphicon glyphicon-circle-arrow-right"></span>';
            } else {
                //linkHtml += cell.getValue();
                //NOリンク列の表示テキストは1から連番とする
                //linkHtml += 1;
                linkHtml += parseInt(row.getPosition(true), 10);//row.getPosition(true)は1始まりの行番号
            }
            linkHtml += '</a>';
            return linkHtml;
        }
        //TODO:cellClickを入れると2重で処理が走ってしまうので要修正
        //head.cellClick = function (e, cell) {
        //    var ele = cell.getElement();
        //    var link = $(ele).find('a');
        //    $(link).click();
        //}
    }
}

/**
 * Tabulatorのファイル参照リンク列、ダウンロードリンク列の設定
 *  @param {object} ：ヘッダー
 *  @param {string} ：テーブルID
 *  @param {string} ：一覧編集パターン
 *  @param {string} ：表示モード
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 */
function setHeaderFileLink(head, id, editptn, referenceMode, appPath) {
    //ファイル参照リンク列、ダウンロードリンク列

    //ベース用のテーブルから要素を取得
    var fileLink = $('table' + id + '_tablebase' + ' tbody td[data-name="' + head.field + '"] a');

    head.formatter = function (cell, formatterParams, onRendered) {
        //パイプ2個区切りで各ファイル毎の値を取得
        //[表示文字列1] | [ﾌｧｲﾙ相対ﾊﾟｽ1] || [表示文字列2] | [ﾌｧｲﾙ相対ﾊﾟｽ2] || ...
        var val = cell.getValue() ? cell.getValue() + "" : "";
        var aryVal = val.split('||');

        var html = $('<div>');
        $.each(aryVal, function (idx, file) {
            //パイプ区切りで表示値と相対パスを取得
            var fileInfo = file.split("|");

            var clone = $(fileLink).clone(true);
            if (fileInfo.length > 1) {
                $(clone).text(fileInfo[0]);  //[0]：表示文字列
                setAttrByNativeJs(clone, "href", fileInfo[1]);    //[1]：ﾌｧｲﾙ相対ﾊﾟｽ
                $(html).append(clone);
                //改行
                if (idx < aryVal.length - 1) {
                    $(html).append('<br>');
                }
            }
            else {
                $(html).append(fileInfo);　//ﾗﾍﾞﾙ表示
                //改行
                if (idx < aryVal.length - 1) {
                    $(html).append('<br>');
                }
            }
        });

        //設定されているイベントを取得
        var events = $._data($(fileLink).get(0), "events");

        //formatterがレンダリング後に呼び出す関数
        onRendered(function () {
            var ele = cell.getElement();

            //イベントを設定
            if (events) {
                $.each(events, function (event, func) {
                    $(ele).find('a').on(event, func[0].handler);
                });
            }

            //スタイル設定
            if (head.cellClass) {
                $(ele).addClass(head.cellClass);
            }
            ele = null;
        });

        return $(html).html();
    }
}

/**
 * Tabulatorのその他（遷移、ポップアップ）のリンク列の設定
 *  @param {object} ：ヘッダー
 *  @param {string} ：テーブルID
 *  @param {string} ：一覧編集パターン
 *  @param {string} ：表示モード
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 */
function setHeaderTransLink(head, id, editptn, referenceMode, appPath) {
    //その他（遷移、ポップアップ）のリンク列

    //ベース用のテーブルから要素を取得
    var transLink = $('table' + id + '_tablebase' + ' tbody td[data-name="' + head.field + '"] a');

    head.formatter = function (cell, formatterParams, onRendered) {
        var clone = $(transLink).clone(true);
        var val = cell.getValue() ? cell.getValue() + "" : "";
        var row = cell.getRow();
        //var rowNo = row.getCell('ROWNO').getValue();
        var rowNo = row.getData().ROWNO;
        setAttrByNativeJs(clone, "onclick", $(clone).attr("onclick").replace("rowNo", rowNo));
        $(clone).text(val);

        //設定されているイベントを取得
        var events = $._data($(transLink).get(0), "events");

        //formatterがレンダリング後に呼び出す関数
        onRendered(function () {
            var ele = cell.getElement();

            //イベントを設定
            if (events) {
                $.each(events, function (event, func) {
                    $(ele).find('a').on(event, func[0].handler);
                });
            }

            //スタイル設定
            if (head.cellClass) {
                $(ele).addClass(head.cellClass);
            }
            ele = null;
        });

        return $(clone).prop('outerHTML');
    }
}

/**
 * Tabulatorの選択チェックボックス列の設定
 *  @param {object} ：ヘッダー
 *  @param {string} ：テーブルID
 *  @param {string} ：一覧編集パターン
 *  @param {string} ：表示モード
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 */
function setHeaderSeltag(head, id, editptn, referenceMode, appPath) {
    //選択チェックボックス列
    head.formatter = function (cell, formatterParams, onRendered) {
        var row = cell.getRow();
        //var rowNo = row.getCell('ROWNO').getValue();
        var rowNo = row.getData().ROWNO;
        var val = cell.getValue();
        onRendered(function () {
            //値設定
            var ele = cell.getElement();
            var seltag = $(ele).find('input[data-name="SELTAG"]');
            if (val == "1") {
                $(seltag).prop("checked", true);
            }
            else {
                $(seltag).prop("checked", false);
            }

            $(seltag).on('change', function () {
                //選択状態をデータに反映（全選択・全解除時に必要なためclickイベントで都度設定する）
                row.update({ SELTAG: $(this).prop('checked') ? 1 : 0 });
                if (editptn != editPtnDef.ReadOnly && referenceMode != referenceModeKbnDef.Reference) {
                    //直接編集可

                    //画面編集フラグをtrueにする
                    setupDataEditedFlg(row.getElement());
                }
            });
            seltag = null;
        });
        return '<input type="checkbox" data-name="SELTAG" data-rowno="' + rowNo + '" />';
    }
}

/**
 * Tabulatorの更新列(非表示)の設定
 *  @param {object} ：ヘッダー
 *  @param {string} ：テーブルID
 *  @param {string} ：一覧編集パターン
 *  @param {string} ：表示モード
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 */
function setHeaderUpdtag(head, id, editptn, referenceMode, appPath) {
    //更新列
    head.formatter = function (cell, formatterParams, onRendered) {
        var row = cell.getRow();
        //var rowNo = row.getCell('ROWNO').getValue();
        var rowNo = row.getData().ROWNO;
        var val = cell.getValue() ? cell.getValue() + "" : "";
        //formatterがレンダリング後に呼び出す関数
        onRendered(function () {
            var ele = cell.getElement();
            if (val) {
                var updflg = $(ele).find("input[data-type='updflg']");
                if (updflg != null) {
                    $(updflg).val(val);
                }
            }
        });
        return '<input type="hidden" data-name="UPDTAG" data-type="updflg" data-rowno="' + rowNo + '" />';
    }
}

/**
 * Tabulatorのボタン列の設定
 *  @param {object} ：ヘッダー
 *  @param {string} ：テーブルID
 *  @param {string} ：一覧編集パターン
 *  @param {string} ：表示モード
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 */
function setHeaderButton(head, id, editptn, referenceMode, appPath) {
    //ボタン
    head.formatter = function (cell, formatterParams, onRendered) {
        var button = $('table' + id + '_tablebase' + ' tbody td[data-name="' + head.field + '"]');
        var input = $(button).find('input[type="button"]');
        var clone = $(button).clone(true);

        //設定されているイベントを取得
        var events = $._data($(input).get(0), "events");

        onRendered(function () {
            var ele = cell.getElement();
            var input = $(ele).find('input[type="button"]');

            //イベントを設定
            if (events) {
                $.each(events, function (event, func) {
                    $(input).on(event, func[0].handler);
                });
            }

            //ボタンの幅を少し小さくする
            var width = $(input).width();
            $(input).width(width - 6);
            input = null;
        });

        return $(clone).html();
    }

    head.tooltip = false;
}

/**
 * Tabulatorの複数選択チェックボックス列の設定
 *  @param {object} ：ヘッダー
 *  @param {string} ：テーブルID
 *  @param {string} ：一覧編集パターン
 *  @param {string} ：表示モード
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 */
function setHeaderMultiCheckBox(head, id, editptn, referenceMode, appPath) {
    //複数選択チェックボックス

    //ベース用のテーブルから複数選択チェックボックス要素を取得
    var multi = $('table' + id + '_tablebase' + ' tbody td[data-name="' + head.field + '"] ul.multiSelect');
    head.formatter = function (cell, formatterParams, onRendered) {
        var html = '<span class="labeling">';
        //設定値
        var val = cell.getValue() ? cell.getValue() + "" : "";
        var aryVal = val.split('|');
        //ラベル値を取得
        var ret = getMultiSelectLabel(multi, cell);
        html = html + ret + '</span>';

        var row = cell.getRow();
        //var rowNo = row.getCell('ROWNO').getValue();
        var rowNo = row.getData().ROWNO;
        var rowStatus = row.getData().ROWSTATUS;
        var eventlist = {};
        //ベーステーブルから複数選択チェックボックス要素をコピーして一覧に表示
        var clone = $(multi).closest('.substance').clone(true);
        var multiClone = $(clone).find('ul.multiSelect');
        var cloneId = $(multiClone).attr('id') + "_" + rowNo;
        setAttrByNativeJs(multiClone, "id", cloneId);
        setAttrByNativeJs(multiClone, "name", $(multiClone).attr('name') + "_" + rowNo);
        var checkboxs = $(multi).find('> li:not(.hide) :checkbox');
        //設定されているイベントを取得
        $.each(checkboxs, function (i, checkbox) {
            var cloneval = $(checkbox).val();
            //設定されているイベントを取得
            eventlist[cloneval] = $._data($(multi).find('> li:not(.hide) :checkbox[value="' + cloneval + '"]').get(0), "events");
        });
        html = html + $(clone).prop('outerHTML');

        //formatterがレンダリング後に呼び出す関数
        onRendered(function () {
            var ele = cell.getElement();

            //値設定
            $.each(aryVal, function () {
                $(ele).find(':checkbox[value="' + this + '"]').prop('checked', true);
            });

            //イベントを設定
            var input = $(ele).find('ul.multiSelect > li:not(.hide) :checkbox');
            $.each(input, function (i, checkbox) {
                var events = eventlist[$(checkbox).val()];
                if (events) {
                    $.each(events, function (event, func) {
                        $(checkbox).on(event, func[0].handler);
                    });
                }
            });

            if (editptn == editPtnDef.ReadOnly || referenceMode == referenceModeKbnDef.Reference) {
                //直接編集不可

                //ラベル表示に切り替え
                $(ele).addClass("readonly");
            }

            //スタイル設定
            addClassForCell(head.cellClass, rowStatus, ele);

            ele = null;
        });

        return html;
    }

    //ツールチップ設定
    if (editptn == editPtnDef.ReadOnly || referenceMode == referenceModeKbnDef.Reference) {
        head.tooltip = function (e, cell, onRendered) {
            var item = $(cell.getElement()).find("ul.multiSelect");
            //ラベル値を取得
            return getMultiSelectLabel(item, cell);
        }
    } else {
        head.tooltip = false;
    }

    if (head.headerFilter != "" && (editptn == editPtnDef.ReadOnly || referenceMode == referenceModeKbnDef.Reference)) {
        //列フィルターをセレクトボックスに変更
        head.headerFilter = "list";
        var checkboxes = $(multi).find('> li:not(.hide) :checkbox');
        var headerOptions = { "": "" };
        $.each(checkboxes, function (i, checkbox) {
            var val = $(checkbox).val();
            var text = $(checkbox).next().text();
            headerOptions[val] = text;
        });
        head.headerFilterParams = { "values": headerOptions };
    }
}

/**
 * Tabulatorのチェックボックス列の設定
 *  @param {object} ：ヘッダー
 *  @param {string} ：テーブルID
 *  @param {string} ：一覧編集パターン
 *  @param {string} ：表示モード
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 */
function setHeaderCheckBox(head, id, editptn, referenceMode, appPath) {
    //チェックボックス

    head.formatter = function (cell, formatterParams, onRendered) {
        //ベース用のテーブルからチェックボックス要素を取得
        var checkbox = $('table' + id + '_tablebase' + ' tbody td[data-name="' + head.field + '"] :checkbox');
        //設定値
        var val = cell.getValue();
        var ret = "";
        if (val == "1") {
            ret = " checked";
        }
        //ラベル値
        var html = '<span class="labeling checkbox' + ret + '"></span>';

        var row = cell.getRow();
        //var rowNo = row.getCell('ROWNO').getValue();
        var rowNo = row.getData().ROWNO;
        var rowStatus = row.getData().ROWSTATUS;
        //ベーステーブルからセレクトボックス要素をコピーして一覧に表示
        var clone = $(checkbox).closest('.substance').clone(true);
        var checktClone = $(clone).find(':checkbox');
        var cloneId = $(checktClone).attr('id') + "_" + rowNo;
        setAttrByNativeJs(checktClone, "id", cloneId);
        setAttrByNativeJs(checktClone, "name", $(checktClone).attr('name') + "_" + rowNo);
        //設定されているイベントを取得
        var events = $._data($(checkbox).get(0), "events");
        html = html + $(clone).prop('outerHTML');

        //formatterがレンダリング後に呼び出す関数
        onRendered(function () {
            //値設定
            if (val == "1") {
                $('#' + cloneId).prop("checked", true);
            }
            else {
                $('#' + cloneId).prop("checked", false);
            }

            //イベントを設定
            if (events) {
                $.each(events, function (event, func) {
                    $('#' + cloneId).on(event, func[0].handler);
                });
            }
            var ele = cell.getElement();

            if (editptn == editPtnDef.ReadOnly || referenceMode == referenceModeKbnDef.Reference) {
                //直接編集不可

                //ラベル表示に切り替え
                $(ele).addClass("readonly");
            }

            //スタイル設定
            addClassForCell(head.cellClass, rowStatus, ele);

            ele = null;
        });

        return html;
    }

    //ツールチップを表示しない
    head.tooltip = false;

    if (head.headerFilter != "" && (editptn == editPtnDef.ReadOnly || referenceMode == referenceModeKbnDef.Reference)) {
        //列フィルターをチェックボックスに変更
        head.headerFilter = "tickCross";
        head.headerFilterParams = { "tristate": true };
        head.headerFilterEmptyCheck = function (value) {
            return value === null
        }
    }
}

/**
 * Tabulatorのセレクトボックス列の設定
 *  @param {object} ：ヘッダー
 *  @param {string} ：テーブルID
 *  @param {string} ：一覧編集パターン
 *  @param {string} ：表示モード
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 */
function setHeaderSelectBox(head, id, editptn, referenceMode, appPath) {
    //セレクトボックス

    //ベース用のテーブルからセレクトボックス要素を取得
    let dataName = head.field + '';
    head.formatter = function (cell, formatterParams, onRendered) {
        let select = $('table' + id + '_tablebase' + ' tbody td[data-name="' + dataName + '"] select');
        var html = '<span class="labeling">';
        ////設定値
        var val = cell.getValue() ? cell.getValue() + "" : "";
        var aryVal = val.split('|');
        //ラベル値を取得
        var ret = getSelectBoxLabel(select, cell);
        html = html + ret + '</span>';

        var row = cell.getRow();
        //var rowNo = row.getCell('ROWNO').getValue();
        var rowNo = row.getData().ROWNO;
        var rowStatus = row.getData().ROWSTATUS;
        row = null;
        //ベーステーブルからセレクトボックス要素をコピーして一覧に表示
        var clone = $(select).closest('.substance').clone(true);
        var selectClone = $(clone).find('select');
        var cloneId = $(selectClone).attr('id') + "_" + rowNo;
        setAttrByNativeJs(selectClone, "id", cloneId);
        setAttrByNativeJs(selectClone, "name", $(selectClone).attr('name') + "_" + rowNo);
        setAttrByNativeJs(selectClone, "data-value", val);
        //設定されているイベントを取得
        var events = $._data($(select).get(0), "events");
        html = html + $(clone).prop('outerHTML');
        clone = null;
        //formatterがレンダリング後に呼び出す関数
        onRendered(function () {
            var selectBox = $('#' + cloneId);
            var sqlId = $(selectBox).data('sqlid');
            var param = $(selectBox).data('param');
            var option = $(selectBox).data('option');
            var nullCheck = $(selectBox).data('nullcheck');
            selectBox = null;

            //SQLパラメータの成形(例：'C01','@3')
            var paramStr = param ? param + '' : '';
            var params = (param + '').split(',');   //ｶﾝﾏで分解
            var factoryIdList = [];
            if (paramStr.indexOf("@") >= 0) {
                //連動コンボの場合、再生成
                //var colNos = [];
                //$.each(params, function () {
                //    var colNo = -1;
                //    var paramVal = "";

                //    if (this.indexOf("@") >= 0) {
                //        //列指定ﾊﾟﾗﾒｰﾀ（「@」で始まる引数）の場合

                //        //列番号を取得
                //        colNo = parseInt(this.replace("'", "").replace("@", ""), 10);  //先頭の「@」を除去してcolNoとする
                //        colNos.push(colNo);
                //        //セルの値
                //        if (colNo != colNoForFactory) {
                //            paramVal = row.getCell('VAL' + colNo).getValue();
                //            paramStr = paramStr.replace('@' + colNo, paramVal);    //「@3」⇒「ｾﾙの値」で置き換え
                //        } else {
                //            paramStr = paramStr.replace(',@' + colNo, '');  // 「,@9999」をパラメータ文字列から削除
                //            var paramList = getSelectedFactoryIdList(row.getElement(), true, true);
                //            if (paramList.length > 0) {
                //                factoryIdList = factoryIdList.concat(paramList);
                //            }
                //            if (paramVal != null && (!isString(paramVal) || (paramVal.length > 0 && paramVal != "null"))) {
                //                factoryIdList.push(paramVal);
                //            }
                //        }
                //    }
                //});
                //initComboBox(appPath, '#' + cloneId, sqlId, paramStr, option, nullCheck, -1, null, factoryIdList);
                initComboBox(appPath, '#' + cloneId, sqlId, paramStr, option, nullCheck, -1, null);
            }
            //値設定
            $('#' + cloneId).val(aryVal);

            //イベントを設定
            if (events) {
                $.each(events, function (event, func) {
                    var ctrl = $('#' + cloneId);
                    ctrl.on(event, func[0].handler);
                    ctrl = null;
                });
            }
            var ele = cell.getElement();

            if (editptn == editPtnDef.ReadOnly || referenceMode == referenceModeKbnDef.Reference) {
                //直接編集不可

                //ラベル表示に切り替え
                $(ele).addClass("readonly");
            }

            ////ツールチップ設定
            //if (editptn == editPtnDef.ReadOnly || referenceMode == referenceModeKbnDef.Reference) {
            //    head.tooltip = function (e, cell, onRendered) {
            //        var item = $(cell.getElement()).find('select');
            //        //ラベル値を取得
            //        return getSelectBoxLabel(item, cell);
            //    };
            //}

            //スタイル設定
            addClassForCell(head.cellClass, rowStatus, ele);
            ele = null;
        });
        select = null;
        return html;
    }

    //ツールチップ設定
    if (editptn == editPtnDef.ReadOnly || referenceMode == referenceModeKbnDef.Reference) {
        head.tooltip = function (e, cell, onRendered) {
            var item = $(cell.getElement()).find('select');
            //ラベル値を取得
            let labelValue = getSelectBoxLabel(item, cell);
            item = null;
            return labelValue;
        };
    } else {
        head.tooltip = false;
    }

    if (head.headerFilter != "" && (editptn == editPtnDef.ReadOnly || referenceMode == referenceModeKbnDef.Reference)) {
        //列フィルターをセレクトボックスに変更
        head.headerFilter = "list";
        let select = $('table' + id + '_tablebase' + ' tbody td[data-name="' + head.field + '"] select');
        var selects = $(select).find('option');
        var headerOptions = { "": "" };
        $.each(selects, function (i, option) {
            var val = $(option).val();
            var text = $(option).text();
            headerOptions[val] = text;
        });
        head.headerFilterParams = { "values": headerOptions };
        select = null;
    }
}

/**
 * Tabulatorのラジオボタン列の設定
 *  @param {object} ：ヘッダー
 *  @param {string} ：テーブルID
 *  @param {string} ：一覧編集パターン
 *  @param {string} ：表示モード
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 */
function setHeaderRadioButton(head, id, editptn, referenceMode, appPath) {
    //ラジオボタン

    //ベース用のテーブルからラジオボタン要素を取得
    var td = $('table' + id + '_tablebase' + ' tbody td[data-name="' + head.field + '"]');
    head.formatter = function (cell, formatterParams, onRendered) {
        var html = '<span class="labeling">';
        //設定値
        var val = cell.getValue();
        //ラジオボタンのラベルの値を取得
        var ret = $(td).find(' :radio[value="' + val + '"]').parent().text();
        html = html + escapeHtml(ret) + '</span>';

        var row = cell.getRow();
        //var rowNo = row.getCell('ROWNO').getValue();
        var rowNo = row.getData().ROWNO;
        var rowStatus = row.getData().ROWSTATUS;
        var eventlist = {};

        //ベーステーブルから要素をコピーして一覧に表示
        var clone = $(td).find('.substance').clone(true);
        var radioClone = $(clone).find(':radio');
        var cloneName = $(radioClone).attr('name') + "_" + rowNo;
        setAttrByNativeJs(radioClone, "name", cloneName);
        //設定されているイベントを取得
        $.each(radioClone, function (i, radio) {
            var cloneval = $(radio).val();
            //設定されているイベントを取得
            eventlist[cloneval] = $._data($(td).find(':radio[value="' + cloneval + '"]').get(0), "events");
        });
        html = html + $(clone).prop('outerHTML');

        //formatterがレンダリング後に呼び出す関数
        onRendered(function () {
            //値設定
            $(':radio[name="' + cloneName + '"][value="' + val + '"]').prop('checked', true);

            var ele = cell.getElement();
            var input = $(ele).find(':radio');
            //イベントを設定
            $.each(input, function (i, radio) {
                var events = eventlist[$(radio).val()];
                if (events) {
                    $.each(events, function (event, func) {
                        $(radio).on(event, func[0].handler);
                    });
                }
            });

            if (editptn == editPtnDef.ReadOnly || referenceMode == referenceModeKbnDef.Reference) {
                //直接編集不可

                //ラベル表示に切り替え
                $(ele).addClass("readonly");

            }

            //ツールチップ設定
            if (editptn == editPtnDef.ReadOnly || referenceMode == referenceModeKbnDef.Reference) {
                head.tooltip = function (e, cell, onRendered) {
                    //設定値
                    var val = cell.getValue();
                    //ラジオボタンのラベルの値を取得
                    var ret = $(cell.getElement()).find(' :radio[value="' + val + '"]').parent().text();
                    return escapeHtml(ret);
                }
            } else {
                head.tooltip = false;
            }

            //スタイル設定
            addClassForCell(head.cellClass, rowStatus, ele);

            ele = null;
        });

        return html;
    }

    if (head.headerFilter != "" && (editptn == editPtnDef.ReadOnly || referenceMode == referenceModeKbnDef.Reference)) {
        //列フィルターをセレクトボックスに変更
        head.headerFilter = "list";
        var radios = $(td).find(':radio');
        var headerOptions = { "": "" };
        $.each(radios, function (i, radio) {
            var val = $(radio).val();
            var text = $(radio).parent().text();
            headerOptions[val] = text;
        });
        head.headerFilterParams = { "values": headerOptions };
    }
}

/**
 * Tabulatorの数値列の設定
 *  @param {object} ：ヘッダー
 *  @param {string} ：テーブルID
 *  @param {string} ：一覧編集パターン
 *  @param {string} ：表示モード
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 */
function setHeaderNumber(head, id, editptn, referenceMode, appPath) {
    //数値

    //ベース用のテーブルからテキスト要素を取得
    var input = $('table' + id + '_tablebase' + ' tbody td[data-name="' + head.field + '"] input[data-type="num"]');
    head.formatter = function (cell, formatterParams, onRendered) {
        var html = '<span class="labeling"></span>';

        var fromtoFlag = $(input).hasClass('fromto') && input.length > 1;
        //単位
        var unit = "";
        var value = "";
        if (cell.getValue() != null) {
            var valueunit = (cell.getValue() + '').split("@");
            value = valueunit[0];
            if (valueunit.length > 1) {
                unit = valueunit[1];
            }
        }
        var aryVal = value.split('|');

        var row = cell.getRow();
        //var rowNo = row.getCell('ROWNO').getValue();
        var rowNo = row.getData().ROWNO;
        var rowStatus = row.getData().ROWSTATUS;
        var clone = "";
        var cloneIds = [];
        var eventlist = {};

        if (fromtoFlag) {
            //ベーステーブルからテキスト要素をコピーして一覧に表示
            clone = $(input[0]).closest('.substance').clone(true);
            var inputClone = $(clone).find('input[data-type="num"]');
            $.each(inputClone, function (i, text) {
                setTabulatorControl(text, rowNo, cloneIds, eventlist);
            });
        } else {
            //ベーステーブルからテキスト要素をコピーして一覧に表示
            clone = $(input).closest('.substance').clone(true);
            var inputClone = $(clone).find('input[data-type="num"]');
            setTabulatorControl(inputClone, rowNo, cloneIds, eventlist);
        }
        html = html + $(clone).prop('outerHTML');

        //formatterがレンダリング後に呼び出す関数
        onRendered(function () {
            var ele = cell.getElement();

            //入力ﾌｫｰﾏｯﾄ設定処理
            initFormat(ele, true);

            //値設定
            $.each(cloneIds, function (i, cloneId) {
                //イベントを設定
                var events = eventlist[cloneId];
                if (events) {
                    $.each(events, function (event, func) {
                        $('#' + cloneId).on(event, func[0].handler);
                    });
                }
                //値を設定
                if (fromtoFlag) {
                    $('#' + cloneId).val(aryVal[i]);
                    setCellNumUnit($('#' + cloneId), unit); //単位
                } else {
                    $('#' + cloneId).val(value);
                    setCellNumUnit($('#' + cloneId), unit); //単位
                }

                //フォーマットする(編集中フラグは変更しない)
                $('#' + cloneId).trigger("change", [true]);

            });
            //ラベル表示用spanに値設定
            var ret = "";
            if (fromtoFlag) {
                //"～"で結合して表示
                ret = $('#' + cloneIds[0]).val();
                if (cloneIds.length > 1) {
                    ret = ret + P_ComMsgTranslated[911060006] + $('#' + cloneIds[1]).val() + unit;
                }
            } else {
                ret = $('#' + cloneIds[0]).val() + unit;
            }
            $(ele).find("span.labeling").text(ret);

            if (editptn == editPtnDef.ReadOnly || referenceMode == referenceModeKbnDef.Reference) {
                //直接編集不可

                //ラベル表示に切り替え
                $(ele).addClass("readonly");
            }

            //ツールチップ設定
            if (editptn == editPtnDef.ReadOnly || referenceMode == referenceModeKbnDef.Reference) {
                head.tooltip = function (e, cell, onRendered) {
                    //ラベル値を取得
                    return $(cell.getElement()).find("span.labeling").text();
                }
            } else {
                head.tooltip = false;
            }

            //スタイル設定
            addClassForCell(head.cellClass, rowStatus, ele);

            ele = null;
        });

        return html;
    }
}

/**
 * Tabulatorのテキスト、日付(DatePicker)、時刻(TimePicker)、日時(DateTimePicker)、テキストエリア、コード＋翻訳列の設定
 *  @param {object} ：ヘッダー
 *  @param {string} ：テーブルID
 *  @param {string} ：一覧編集パターン
 *  @param {string} ：表示モード
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 */
function setHeaderText(head, id, editptn, referenceMode, appPath) {
    //テキスト、日付(DatePicker)、時刻(TimePicker)、日時(DateTimePicker)、テキストエリア、コード＋翻訳
    //ベース用のテーブルからテキスト要素を取得
    var td = $('table' + id + '_tablebase' + ' tbody td[data-name="' + head.field + '"]');
    var input = $(td).find(":text, textarea");
    head.formatter = function (cell, formatterParams, onRendered) {
        var fromtoFlag = $(input).hasClass('fromto') && input.length > 1;
        var html = '<span class="labeling">';

        //ラベル値を取得
        var ret = getTextBoxLabel(fromtoFlag, cell);
        var val = cell.getValue() ? cell.getValue() + "" : "";
        var aryVal = val.split('|');
        html = html + ret + '</span>';

        var row = cell.getRow();
        //var rowNo = row.getCell('ROWNO').getValue();
        var rowNo = row.getData().ROWNO;
        var rowStatus = row.getData().ROWSTATUS;
        var clone = "";
        var cloneIds = [];
        var eventlist = {};

        if (fromtoFlag) {
            //ベーステーブルからテキスト要素をコピーして一覧に表示
            clone = $(input[0]).closest('.substance').clone(true);
            var inputClone = $(clone).find(':text, textarea');
            $.each(inputClone, function (i, text) {
                setTabulatorControl(text, rowNo, cloneIds, eventlist);
            });
        } else {
            //ベーステーブルからテキスト要素をコピーして一覧に表示
            clone = $(input).closest('.substance').clone(true);
            var inputClone = $(clone).find(':text, textarea');
            setTabulatorControl(inputClone, rowNo, cloneIds, eventlist);
        }
        html = html + $(clone).prop('outerHTML');

        //formatterがレンダリング後に呼び出す関数
        onRendered(function () {
            $.each(cloneIds, function (i, cloneId) {
                //値設定
                if (fromtoFlag && cell.getValue() != null) {
                    $('#' + cloneId).val(aryVal[i]);
                } else {
                    $('#' + cloneId).val(val);
                }

                //イベントを設定
                var events = eventlist[cloneId];
                if (events) {
                    $.each(events, function (event, func) {
                        $('#' + cloneId).on(event, func[0].handler);
                    });
                }
            });

            var ele = cell.getElement();
            if (editptn == editPtnDef.ReadOnly || referenceMode == referenceModeKbnDef.Reference) {
                //直接編集不可

                //ラベル表示に切り替え
                $(ele).addClass("readonly");

                // 日付(DatePicker)、時刻(TimePicker)、日時(DateTimePicker)コントロールの初期化
                initDateTypePicker($(ele), false);
            } else {
                //直接編集可

                // 日付(DatePicker)、時刻(TimePicker)、日時(DateTimePicker)コントロールの初期化
                initDateTypePicker($(ele), false);
            }
            // オートコンプリートの初期化
            var autoComps = $(ele).find(":text[data-type='text'], textarea[data-type='textarea'], :text[data-type='codeTrans']");
            $(autoComps).each(function (index, element) {
                if ($(element).data("autocompdiv") != autocompDivDef.None) {
                    $.each(P_autocompDefines, function (idx, define) {
                        if (('#' + element.id).indexOf(define.key) === 0) {
                            var sqlId = define.sqlId;
                            var param = define.param;
                            var div = define.division;
                            var option = define.option;
                            initAutoComp(appPath, '#' + element.id, sqlId, param, null, div, option);
                            return false;
                        }
                    });
                }
            });

            // コード＋翻訳の初期化(編集中フラグは変更しない)
            var inputs = $(ele).find(":text[data-type='codeTrans']");
            $.each(inputs, function (index, element) {
                // 翻訳セットのためチェンジイベントを発火
                $('#' + element.id).trigger('change', [true]);
            });

            //スタイル設定
            addClassForCell(head.cellClass, rowStatus, ele);

            ele = null;
        });

        return html;
    }

    //ツールチップ設定
    if (editptn == editPtnDef.ReadOnly || referenceMode == referenceModeKbnDef.Reference) {
        head.tooltip = function (e, cell, onRendered) {
            var item = $(cell.getElement()).find(":text, textarea");
            var fromtoFlag = $(item).hasClass('fromto') && item.length > 1;
            item = null;
            //ラベル値を取得
            let value = getTextBoxLabel(fromtoFlag, cell);
            return value;
        };
    } else {
        head.tooltip = false;
    }
}

/**
 * Tabulatorの日時（ブラウザ標準）列の設定
 *  @param {object} ：ヘッダー
 *  @param {string} ：テーブルID
 *  @param {string} ：一覧編集パターン
 *  @param {string} ：表示モード
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 */
function setHeaderDatetime(head, id, editptn, referenceMode, appPath) {
    //日時（ブラウザ標準）

    //ベース用のテーブルから要素を取得
    var td = $('table' + id + '_tablebase' + ' tbody td[data-name="' + head.field + '"]');
    var input = $(td).find("input[type='datetime-local']");
    head.formatter = function (cell, formatterParams, onRendered) {
        var fromtoFlag = $(input).hasClass('fromto') && input.length > 1;
        var html = '<span class="labeling">';

        //ラベル値を取得
        var ret = getTextBoxLabel(fromtoFlag, cell);
        var val = cell.getValue() ? cell.getValue() + "" : "";
        var aryVal = val.split('|');
        html = html + ret + '</span>';

        var row = cell.getRow();
        //var rowNo = row.getCell('ROWNO').getValue();
        var rowNo = row.getData().ROWNO;
        var rowStatus = row.getData().ROWSTATUS;
        var clone = "";
        var cloneIds = [];
        var eventlist = {};

        if (fromtoFlag) {
            //ベーステーブルからテキスト要素をコピーして一覧に表示
            clone = $(input[0]).closest('.substance').clone(true);
            var inputClone = $(clone).find("input[type='datetime-local']");
            $.each(inputClone, function (i, text) {
                setTabulatorControl(text, rowNo, cloneIds, eventlist);
            });
        } else {
            //ベーステーブルからテキスト要素をコピーして一覧に表示
            clone = $(input).closest('.substance').clone(true);
            var inputClone = $(clone).find("input[type='datetime-local']");
            setTabulatorControl(inputClone, rowNo, cloneIds, eventlist);
        }
        html = html + $(clone).prop('outerHTML');

        //formatterがレンダリング後に呼び出す関数
        onRendered(function () {
            $.each(cloneIds, function (i, cloneId) {
                //値設定
                if (cell.getValue()) {
                    var num = Date.parse(cell.getValue());
                    if (fromtoFlag) {
                        var num = Date.parse(aryVal[i]);
                    }

                    var fmt = $(input).find("format");
                    var date = new Date(num);
                    var y = date.getFullYear();
                    var m = ("0" + (date.getMonth() + 1)).slice(-2);
                    var d = ("0" + date.getDate()).slice(-2);
                    var h = ("0" + date.getHours()).slice(-2);
                    var mi = ("0" + date.getMinutes()).slice(-2);
                    var s = ("0" + date.getSeconds()).slice(-2);
                    var valDate = y + "-" + m + "-" + d;
                    var valTime = (fmt != null && fmt.length > 0 && fmt.indexOf("s")) ? h + ":" + mi + ":" + s : h + ":" + mi;
                    $('#' + cloneId).val(valDate + "T" + valTime);
                }
                //イベントを設定
                var events = eventlist[cloneId];
                if (events) {
                    $.each(events, function (event, func) {
                        $('#' + cloneId).on(event, func[0].handler);
                    });
                }
            });

            var ele = cell.getElement();
            if (editptn == editPtnDef.ReadOnly || referenceMode == referenceModeKbnDef.Reference) {
                //直接編集不可

                //ラベル表示に切り替え
                $(ele).addClass("readonly");
            }

            //ツールチップ設定
            if (editptn == editPtnDef.ReadOnly || referenceMode == referenceModeKbnDef.Reference) {
                head.tooltip = function (e, cell, onRendered) {
                    var item = $(cell.getElement()).find("input[type='datetime-local']");
                    var fromtoFlag = $(item).hasClass('fromto') && item.length > 1;
                    //ラベル値を取得
                    return getTextBoxLabel(fromtoFlag, cell);
                }
            } else {
                head.tooltip = false;
            }

            //スタイル設定
            addClassForCell(head.cellClass, rowStatus, ele);

            ele = null;
        });

        return html;
    }
}

/**
 * Tabulatorの日付（ブラウザ標準）列の設定
 *  @param {object} ：ヘッダー
 *  @param {string} ：テーブルID
 *  @param {string} ：一覧編集パターン
 *  @param {string} ：表示モード
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 */
function setHeaderDate(head, id, editptn, referenceMode, appPath) {
    //日付（ブラウザ標準）

    //ベース用のテーブルから要素を取得
    var td = $('table' + id + '_tablebase' + ' tbody td[data-name="' + head.field + '"]');
    var input = $(td).find("input[type='date']");
    head.formatter = function (cell, formatterParams, onRendered) {
        var fromtoFlag = $(input).hasClass('fromto') && input.length > 1;
        var html = '<span class="labeling">';

        //ラベル値を取得
        var ret = getTextBoxLabel(fromtoFlag, cell);
        var val = cell.getValue() ? cell.getValue() + "" : "";
        var aryVal = val.split('|');
        html = html + ret + '</span>';

        var row = cell.getRow();
        //var rowNo = row.getCell('ROWNO').getValue();
        var rowNo = row.getData().ROWNO;
        var rowStatus = row.getData().ROWSTATUS;
        var clone = "";
        var cloneIds = [];
        var eventlist = {};

        if (fromtoFlag) {
            //ベーステーブルからテキスト要素をコピーして一覧に表示
            clone = $(input[0]).closest('.substance').clone(true);
            var inputClone = $(clone).find("input[type='date']");
            $.each(inputClone, function (i, text) {
                setTabulatorControl(text, rowNo, cloneIds, eventlist);
            });
        } else {
            //ベーステーブルからテキスト要素をコピーして一覧に表示
            clone = $(input).closest('.substance').clone(true);
            var inputClone = $(clone).find("input[type='date']");
            setTabulatorControl(inputClone, rowNo, cloneIds, eventlist);
        }
        html = html + $(clone).prop('outerHTML');

        //formatterがレンダリング後に呼び出す関数
        onRendered(function () {
            $.each(cloneIds, function (i, cloneId) {
                //値設定
                if (cell.getValue()) {
                    var num = Date.parse(cell.getValue());
                    if (fromtoFlag) {
                        var num = Date.parse(aryVal[i]);
                    }
                    var date = new Date(num);
                    var y = date.getFullYear();
                    var m = ("0" + (date.getMonth() + 1)).slice(-2);
                    var d = ("0" + date.getDate()).slice(-2);
                    var txt = y + "-" + m + "-" + d;
                    $('#' + cloneId).val(txt);
                }
                //イベントを設定
                var events = eventlist[cloneId];
                if (events) {
                    $.each(events, function (event, func) {
                        $('#' + cloneId).on(event, func[0].handler);
                    });
                }
            });

            var ele = cell.getElement();
            if (editptn == editPtnDef.ReadOnly || referenceMode == referenceModeKbnDef.Reference) {
                //直接編集不可

                //ラベル表示に切り替え
                $(ele).addClass("readonly");
            }

            //ツールチップ設定
            if (editptn == editPtnDef.ReadOnly || referenceMode == referenceModeKbnDef.Reference) {
                head.tooltip = function (e, cell, onRendered) {
                    var item = $(cell.getElement()).find("input[type='date']");
                    var fromtoFlag = $(item).hasClass('fromto') && item.length > 1;
                    //ラベル値を取得
                    return getTextBoxLabel(fromtoFlag, cell);
                }
            } else {
                head.tooltip = false;
            }

            //スタイル設定
            addClassForCell(head.cellClass, rowStatus, ele);

            ele = null;
        });

        return html;
    }
}

/**
 * Tabulatorの時刻（ブラウザ標準）列の設定
 *  @param {object} ：ヘッダー
 *  @param {string} ：テーブルID
 *  @param {string} ：一覧編集パターン
 *  @param {string} ：表示モード
 *  @param {string} ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 */
function setHeaderTime(head, id, editptn, referenceMode, appPath) {
    //時刻（ブラウザ標準）

    //ベース用のテーブルから要素を取得
    var td = $('table' + id + '_tablebase' + ' tbody td[data-name="' + head.field + '"]');
    var input = $(td).find("input[type='time']");
    head.formatter = function (cell, formatterParams, onRendered) {
        var fromtoFlag = $(input).hasClass('fromto') && input.length > 1;
        var html = '<span class="labeling">';

        //ラベル値を取得
        var ret = getTextBoxLabel(fromtoFlag, cell);
        var val = cell.getValue() ? cell.getValue() + "" : "";
        var aryVal = val.split('|');
        html = html + ret + '</span>';

        var row = cell.getRow();
        //var rowNo = row.getCell('ROWNO').getValue();
        var rowNo = row.getData().ROWNO;
        var rowStatus = row.getData().ROWSTATUS;
        var clone = "";
        var cloneIds = [];
        var eventlist = {};

        if (fromtoFlag) {
            //ベーステーブルからテキスト要素をコピーして一覧に表示
            clone = $(input[0]).closest('.substance').clone(true);
            var inputClone = $(clone).find("input[type='time']");
            $.each(inputClone, function (i, text) {
                setTabulatorControl(text, rowNo, cloneIds, eventlist);
            });
        } else {
            //ベーステーブルからテキスト要素をコピーして一覧に表示
            clone = $(input).closest('.substance').clone(true);
            var inputClone = $(clone).find("input[type='time']");
            setTabulatorControl(inputClone, rowNo, cloneIds, eventlist);
        }
        html = html + $(clone).prop('outerHTML');

        //formatterがレンダリング後に呼び出す関数
        onRendered(function () {
            $.each(cloneIds, function (i, cloneId) {
                //値設定
                if (fromtoFlag) {
                    $('#' + cloneId).val(aryVal[i]);
                } else {
                    $('#' + cloneId).val(val);
                }

                //イベントを設定
                var events = eventlist[cloneId];
                if (events) {
                    $.each(events, function (event, func) {
                        $('#' + cloneId).on(event, func[0].handler);
                    });
                }
            });

            var ele = cell.getElement();
            if (editptn == editPtnDef.ReadOnly || referenceMode == referenceModeKbnDef.Reference) {
                //直接編集不可

                //ラベル表示に切り替え
                $(ele).addClass("readonly");
                head.tooltip = function (e, cell, onRendered) {
                    var item = $(cell.getElement()).find("input[type='time']");
                    var fromtoFlag = $(item).hasClass('fromto') && item.length > 1;
                    //ラベル値を取得
                    return getTextBoxLabel(fromtoFlag, cell);
                }
            } else {
                head.tooltip = false;
            }

            //スタイル設定
            addClassForCell(head.cellClass, rowStatus, ele);

            ele = null;
        });

        return $(clone).html();
    }
}

/**
 * 複数選択チェックボックスの選択値のラベル値を取得し、カンマ区切りの文字列を戻す
 * @param {any} ele 要素
 * @param {any} cell セルコンポーネント
 */
function getMultiSelectLabel(ele, cell) {
    //設定値
    var val = cell.getValue() ? cell.getValue() + "" : "";
    var aryVal = val.split('|');

    //ラベル値
    var texts = [];
    $.each(aryVal, function (i, value) {
        if (value) {
            //ラベル値を取得
            texts.push($(ele).find('> li:not(.hide) :checkbox[value="' + value + '"]').next().text());
        }
    });
    var ret = texts.join(',');
    return escapeHtml(ret);
}

/**
 * セレクトボックスの選択値のラベル値を取得し、カンマ区切りの文字列を戻す
 * @param {any} ele 要素
 * @param {any} cell セルコンポーネント
 */
function getSelectBoxLabel(ele, cell) {
    //設定値
    var val = cell.getValue() ? cell.getValue() + "" : "";
    var aryVal = val.split('|');

    //ラベル値
    var texts = [];
    $.each(aryVal, function () {
        //ラベル値を取得
        texts.push($(ele).find('option[value="' + this + '"]').text());
    });
    var ret = texts.join(',');
    return escapeHtml(ret);
}

/**
 * テキストのラベル値を取得
 * @param {any} fromtoFlag from-toフラグ
 * @param {any} cell セルコンポーネント
 */
function getTextBoxLabel(fromtoFlag, cell) {
    var ret = "";
    var val = cell.getValue() ? cell.getValue() + "" : "";
    var aryVal = val.split('|');
    if (fromtoFlag) {
        //パイプ区切り文字を分割、"～"で結合して表示
        ret = aryVal[0];
        if (aryVal.length > 1) {
            ret = ret + P_ComMsgTranslated[911060006] + aryVal[1];
        }
    } else {
        var ele = cell.getElement();
        var autocomp = $(ele).find("input[data-type='codeTrans'][data-autocompdiv!='" + autocompDivDef.None + "']");
        if (autocomp && autocomp.length > 0) {
            //コード＋翻訳の場合、表示用ラベルから文字列を取得（コード＋翻訳内の処理で左記に翻訳が設定されるため）
            val = $(ele).find("span.labeling").text();
        }
        ele = null;

        ret = val;
    }

    return escapeHtml(ret);
}

/**
 * 文字列をエスケープ（XSS対策）
 * @param {any} text 文字列
 */
function escapeHtml(text) {
    if (!text) {
        return "";
    }
    if (typeof text !== 'string') {
        return text;
    }
    return text.replace(/[&'`"<>]/g, function (match) {
        return {
            '&': '&amp;',
            "'": '&#x27;',
            '`': '&#x60;',
            '"': '&quot;',
            '<': '&lt;',
            '>': '&gt;',
        }[match];
    });
}

/**
 * Tabulatorの各セルのクラス設定
 * @param {any} cellClass クラス
 * @param {any} rowStatus 行のROWSTATUS
 * @param {any} ele 要素
 */
function addClassForCell(cellClass, rowStatus, ele) {
    if (cellClass) {
        var addClass = cellClass;
        if (rowStatus == rowStatusDef.New) {
            // 行ステータスが新規の場合、既存変更不可項目のラベル化解除（変更可にする）
            addClass = cellClass.replace("unchange_exist", "");
        }
        $(ele).addClass(addClass);
    }
}

/**
 * tabulatorのイベント設定
 * @param {any} appPath ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 * @param {any} conductId 機能ID
 * @param {any} pgmId プログラムID
 * @param {any} formNo 画面番号
 * @param {any} ctrlId コントロールID
 * @param {any} table tabulator
 * @param {any} id テーブルID
 * @param {any} paginationElement ページング要素
 * @param {any} editptn 一覧編集パターン
 * @param {any} referenceMode 表示モード
 */
function setTabulatorEvent(appPath, conductId, pgmId, formNo, ctrlId, table, id, paginationElement, editptn, referenceMode) {
    table.on("pageLoaded", function (pageno) {
        //ページが読み込まれた時

        if (P_TabulatorSortingFlag) {
            P_TabulatorEventWaitStatus = tabulatorEventWaitStatusDef.WaitOtherEvent;
        }
        if (P_TabulatorEventWaitStatus == tabulatorEventWaitStatusDef.None) {
            P_ExecRenComFlag = true;
        }

        // ページ番号をセッションストレージへ保存
        saveTabulatorPageNo(conductId, formNo, id, pageno);

        //ページャーの再設定を行う
        var tbl = this;
        var pagination = $(P_Article).find(id + '_div').find('.paginationCommon');
        if (tbl && pagination != null && pagination.length > 0) {
            //全データ件数
            var count = getTotalRowCount(tbl);
            //１ページに表示する行数
            var pagesize = tbl.getPageSize();
            setAttrByNativeJs(pagination, 'data-pagerows', pagesize);
            //ページャーの再設定
            setupPagination(appPath, conductId, pgmId, formNo, ctrlId, count);
            //ページャーの総ページ数が1件の場合、ページャーを非表示
            setHidePagination(id, tbl);

            var pageNo = $(pagination).data('pageno');
            if (table.getPage() != pageNo) {
                if (editptn != editPtnDef.ReadOnly || referenceMode != referenceModeKbnDef.Reference) {
                    //入力欄の変更を保存
                    updateTabulatorDataForChangeVal(tbl, id, formNo);
                }
                //指定のページを表示
                tbl.setPage(pageNo);
            }
            //【オーバーライド用関数】Tabulatorのページ読込後の処理
            postTabulatorChangePage(tbl, id, pageNo, pagesize);

            //縦長行がある場合、レイアウトが崩れるためrenderComplete内でredrawする
            setAttrByNativeJs($(id), "data-redrawflg", true);
            //pageLoaded後にrenderCompleteの処理が実行されない場合があるためここでもredrawしておく
            tbl.redraw();
        }
        tbl = null;
        pagination = null;
    });

    table.on("pageSizeChanged", function (pagesize) {
        //ページサイズ変更時

        P_ExecRenComFlag = false;

        // ローカルストレージへページ表示件数を保存
        saveTabulatorPageSize(conductId, formNo, id, pagesize);

        var tbl = this;

        //１ページに表示する行数を設定
        var pagination = $(P_Article).find(id + '_div').find('.paginationCommon');
        setAttrByNativeJs(pagination, 'data-pagerows', pagesize);

        if (editptn != editPtnDef.ReadOnly || referenceMode != referenceModeKbnDef.Reference) {
            //入力欄の変更を保存
            updateTabulatorDataForChangeVal(tbl, id, formNo);
        }

        //レイアウトが崩れる場合があるためrenderComplete内でredrawする
        setAttrByNativeJs($(id), "data-redrawflg", true);
        tbl = null;
    });

    table.on("dataSorting", function (sorters) {
        //ソート前
    });

    table.on("dataSorted", function (sorters, rows) {
        //ソート後

        if (P_TabulatorEventWaitStatus == tabulatorEventWaitStatusDef.None) {
            if (!P_TabulatorFilteringFlag) {
                P_TabulatorSortingFlag = true;
                P_ExecRenComFlag = false;
            } else {
                P_ExecRenComFlag = true;
            }
        }

        // ローカルストレージからソート順を取得
        var sortOrder = getSavedDataFromLocalStorage(localStorageCode.SortOrder, conductId, formNo, ctrlId);
        if ((!sortOrder && sorters.length == 0) || (sortOrder && sorters.length > 0 && sortOrder[0].column == sorters[0].field && sortOrder[0].dir == sorters[0].dir)) {
            //ヘッダーソート以外で呼ばれた場合、処理終了
            return;
        }

        //ソート時、ページを先頭（1ページ目）にする
        var pageNo = 1;
        setPaginationStatus(ctrlId, pageNo);
        var tbl = this;
        if (editptn != editPtnDef.ReadOnly || referenceMode != referenceModeKbnDef.Reference) {
            //入力欄の変更を保存
            updateTabulatorDataForChangeVal(tbl, id, formNo);
        }
        tbl.setPage(pageNo);

        // ソート順をローカルストレージへ保存
        var saveData = sorters.length > 0 ? sorters[0] : null;
        saveTabulatorSortOrder(conductId, formNo, ctrlId, saveData);

        //縦長行がある場合、レイアウトが崩れるためrenderComplete内でredrawする
        setAttrByNativeJs($(id), "data-redrawflg", true);
    });

    table.on("dataFiltered", function (filters, rows) {
        //列フィルター適応後

        if (P_TabulatorEventWaitStatus == tabulatorEventWaitStatusDef.None) {
            // フィルター入力時
            P_TabulatorFilteringFlag = true;
            P_TabulatorSortingFlag = false;
            P_ExecRenComFlag = false;
        }

        var tbl = this;

        if (editptn != editPtnDef.ReadOnly || referenceMode != referenceModeKbnDef.Reference) {
            //入力欄の変更を保存
            updateTabulatorDataForChangeVal(tbl, id, formNo);
        }

        //件数が多いと展開に時間がかかるためコメントアウト
        //$.each(rows, function (i, row) {
        //    if (tbl.getHeaderFilters().length > 0 && row && row.getTreeChildren().length > 0) {
        //        //ツリーを展開する
        //        row.treeExpand();
        //    }
        //});

        //【オーバーライド用関数】Tabulatorの列フィルター後の処理
        postTabulatorDataFiltered(tbl, id, filters, rows);

        //一覧の縦幅が戻らない場合があるためrenderComplete内でredrawする
        setAttrByNativeJs($(id), "data-redrawflg", true);
    });

    table.on("renderComplete", function () {
        //レンダリング完了後（html要素は描画中のため取得できない）

        //clearHrefForTablator();
        // 以降の処理は一覧表示時に1度のみ実行させる
        if (!P_ExecRenComFlag) {
            // tableBuilt実行前は実行しない
            if (P_TabulatorEventWaitStatus == tabulatorEventWaitStatusDef.WaitOtherEvent) {
                P_ExecRenComFlag = true;
                P_TabulatorSortingFlag = false;
                P_TabulatorEventWaitStatus = tabulatorEventWaitStatusDef.None;
            }
            return;
        }
        P_ExecRenComFlag = false;
        P_TabulatorEventWaitStatus = tabulatorEventWaitStatusDef.None;
        P_TabulatorFilteringFlag = false;

        //ページサイズ切り替えコンボは表示し、ページングボタンは非表示にする
        $('.tabulator-page').hide();

        var tbl = this;

        //ページャーの再設定
        if (tbl) {
            var count = getTotalRowCount(tbl);
            setupPagination(appPath, conductId, pgmId, formNo, ctrlId, count);
        }

        //ページャーの総ページ数が1件の場合、ページャーを非表示
        setHidePagination(id, this);

        //デフォルトの「Page Size」ラベルを削除
        $(paginationElement).find('label').remove();

        //NOリンク列の表示テキストは1から連番とする
        if (tbl) {
            //No列がNoリンクかアイコンリンクか判定
            var flg = true;
            var rowNoObj = $.grep(tbl.getColumnDefinitions(), function (obj, i) {
                return obj.field == "ROWNO";
            });
            if (rowNoObj && rowNoObj.length > 0) {
                if (rowNoObj[0].formatterParams && rowNoObj[0].formatterParams.label) {
                    //ペンアイコン、本アイコンの場合、Noリンク振りなおし処理は行わない
                    flg = false;
                }
            }

            if (flg) {
                // 表示されている行に対してのみ実行する
                //let rows = tbl.getRows("active");
                let rows = tbl.getRows("display");
                let linkIndex = 0;
                $.each(rows, function (idx, row) {
                    //setDispRowNo(row);
                    linkIndex = setDispRowNo2(id, linkIndex, row);
                });
                rows = null;
            }
            rowNoObj = null;
        }

        if (editptn != editPtnDef.ReadOnly || referenceMode != referenceModeKbnDef.Reference) {
            //直接編集可

            //画面編集ﾌﾗｸﾞ制御ｲﾍﾞﾝﾄﾊﾝﾄﾞﾗの設定
            let rows = $(tbl.element).find(".tabulator-row");
            setEventForEditFlg(true, rows);
            rows = null;
        }

        //ソート時等にレイアウトが崩れる場合があるため、フラグがtrueの場合は再描画する
        var redrawFlg = $(id).data('redrawflg');
        if (redrawFlg) {
            setAttrByNativeJs($(id), "data-redrawflg", "");
            tbl.redraw();
        }

        redrawFlg = null;
        // 非表示列に対する列幅変更の非表示化
        // 列幅変更列を取得
        $(id).find('.tabulator-col-resize-handle').each(function (index, element) {
            // 列幅変更列の直前列（値の列）が非表示の場合、列幅変更列も非表示
            if (($(element).prev()[0]).style["display"] == "none") {
                $(element).css("display", "none");
            }
        });

        // 【オーバーライド用関数】Tabulatorのレンダリング完了後の処理
        postTabulatorRenderCompleted(tbl, id);
        tbl = null;

    });

    table.on("columnResized", function (column) {

        //tabulatorの幅調整
        //resizeColumn(id);
    });

    table.on("dataTreeRowExpanded", function (row, level) {
        //ツリー展開

        //ページングボタンは非表示にする
        $('.tabulator-page').hide();

        var tbl = P_listData[id];

        //ページャーの再設定
        var pagination = $(P_Article).find(id + '_div').find('.paginationCommon[data-option="def"]');
        //var total = parseInt($(pagination).data("totalcnt"), 10);
        var total = getTotalRowCount(tbl);
        setupPagination(appPath, conductId, pgmId, formNo, ctrlId, total);

        //ページャーの総ページ数が1件の場合、ページャーを非表示
        setHidePagination(id, tbl);
        // テーブル再読み込み
        tbl.redraw();
        tbl = null;

        // 【オーバーライド関数】ツリー展開時追加処理
        afterDataTreeRowExpanded(id, row, level);
    });

    table.on("dataTreeRowCollapsed", function (row, level) {
        //ツリー折りたたみ

        //ページングボタンは非表示にする
        $('.tabulator-page').hide();

        var tbl = P_listData[id];

        //ページャーの再設定
        var pagination = $(P_Article).find(id + '_div').find('.paginationCommon[data-option="def"]');
        //var total = parseInt($(pagination).data("totalcnt"), 10);
        var total = getTotalRowCount(tbl);
        setupPagination(appPath, conductId, pgmId, formNo, ctrlId, total);

        //ページャーの総ページ数が1件の場合、ページャーを非表示
        setHidePagination(id, tbl);
        // テーブル再読み込み
        tbl.redraw();
        tbl = null;
    });

    table.on("rowMoved", function (row) {
        //行移動後
    });

    table.on("columnMoved", function (column, columns) {
        //列の移動後
    });

    table.on("rowUpdated", function (row) {
        //行更新

        //ページサイズ切り替えコンボは表示し、ページングボタンは非表示にする
        $('.tabulator-page').hide();
    });

    table.on("tableBuilding", function () {
        P_ExecRenComFlag = false;
        P_TabulatorEventWaitStatus = tabulatorEventWaitStatusDef.WaitTableBuilt;
        P_TabulatorSortingFlag = false;
        P_TabulatorFilteringFlag = false;
    });

    table.on("tableBuilt", function () {
        //テーブルのレンダリング完了後

        P_ExecRenComFlag = true;

        var tbl = this;

        tbl.redraw(true);

        // 項目カスタマイズ情報による列の表示切替処理
        //setItemCustomizeInfoToTabulator(tbl, conductId, formNo, ctrlId);

        // セッションストレージからページ番号を取得
        const sessionPageNo = getTabulatorPageNo(conductId, formNo, id);
        const lastPageNo = getPageCountAll(ctrlId);
        //const lastPageNo = tbl.getPageMax();
        if (sessionPageNo > 1 && sessionPageNo <= lastPageNo) {
            setCurPageNo(ctrlId, sessionPageNo);
            tbl.setPage(sessionPageNo);
        }

        // 【オーバーライド用関数】Tabulatorの描画が完了時の処理
        postBuiltTabulator(tbl, id);

        // 前回設定した列フィルターを再設定
        var filters = P_colFilterData[id];
        if (filters && filters.length != 0) {
            $.each(filters, function (idx, data) {
                tbl.setHeaderFilterValue(data.field, data.value);
            });
        }

        //モーダル画面の幅調整
        setModalWidthAfterTableBuilt(tbl, id);
        tbl = null;
    });
}

/**
 * TabulatorのNoリンク列を設定
 * @param {any} ：行
 */
function setDispRowNo(row) {
    //Noリンク列は常に1からナンバリングする
    //Noラベル列はformatter=rownumによって自動でナンバリングする為、考慮不要
    let cell = row.getCell("ROWNO");
    if (cell) {
        let ele = cell.getElement();
        let link = $(ele).find('a');
        //row.getPosition(true)は1始まりの行番号
        let rownum = parseInt(row.getPosition(true), 10);
        //NOリンク列のテキストを変更
        $(link).text(rownum);
        //ツリーを開いている場合
        if (row.isTreeExpanded()) {
            //ツリーの子要素はNoを削除する
            let child = row.getTreeChildren();
            $.each(child, function (i, childRow) {
                let childCell = childRow.getCell("ROWNO");
                if (childCell) {
                    let childele = childCell.getElement();
                    let childlink = $(childele).find('a');
                    //var childlink = $(childele).find('a,.treeRowNo');
                    //row.getPosition(true)は0始まりの行番号
                    //var rownum = parseInt(row.getPosition(true), 10) + 1;
                    //var rownum = tbl.rowManager.activeRows.indexOf(cell.getRow()._getSelf()) + 1;
                    //NOリンク列をテキストに変更
                    //$(childlink).replaceWith('<span class="treeRowNo">' + parseInt(i+1,10) + '</span>');
                    $(childlink).remove();
                    childlink = null;
                    childele = null;
                    childCell = null;
                }
            });
            child = null;
        }
        link = null;
        ele = null;
    }
    cell = null;
}

/**
 * TabulatorのNoリンク列を設定
 * @param {string} id       ：行要素ID
 * @param {number} index    ：行要素インデックス
 * @param {Element} row     ：行要素 
 */
function setDispRowNo2(id, index, row) {
    // 220831 MOD_TEST setDispRowNoのgetCellの処理がメモリを使用している、IDで取得
    // この処理のツリー以外では使っていない
    //Noリンク列は常に1からナンバリングする
    //Noラベル列はformatter=rownumによって自動でナンバリングする為、考慮不要
    let link = $(id).find("div[tabulator-field='ROWNO']").find("a");
    //初期表示時に表示範囲外の行はリンク要素が取れないので、子要素のリンクを消す処理だけ行う用のフラグ
    let flg = true;
    if (!link || index >= link.length) {
        flg = false;
    }

    let nextIndex = index;
    if (row.getTreeParent()) {
        //親が存在＝子要素の場合、子要素のNoリンク削除（前ページに親、表示ページに子がある場合を考慮）
        row = row.getTreeParent();
    } else if (flg) {
        link = link[index];
        let rownum = parseInt(row.getPosition(true), 10); //row.getPosition(true)は1始まりの行番号
        $(link).text(rownum);
        nextIndex = nextIndex + 1;
    }

    //ツリーを開いている場合
    if (row.isTreeExpanded()) {
        //ツリーの子要素はNoを削除する
        let child = row.getTreeChildren();
        $.each(child, function (i, childRow) {
            let childCell = childRow.getCell("ROWNO");
            if (childCell) {
                let childele = childCell.getElement();
                let childlink = $(childele).find('a');
                //var childlink = $(childele).find('a,.treeRowNo');
                //row.getPosition(true)は0始まりの行番号
                //var rownum = parseInt(row.getPosition(true), 10) + 1;
                //var rownum = tbl.rowManager.activeRows.indexOf(cell.getRow()._getSelf()) + 1;
                //NOリンク列をテキストに変更
                //$(childlink).replaceWith('<span class="treeRowNo">' + parseInt(i+1,10) + '</span>');
                $(childlink).remove();
                childlink = null;
                childele = null;
                childCell = null;
            }
        });
        child = null;
    }
    link = null;

    return nextIndex;
}

/**
 * Tabulatorの総件数を取得
 * @param {any} ：テーブル
 */
function getTotalRowCount(table) {
    //絞り込み後のデータ件数
    var count = table.getDataCount("active");
    //ツリー表示のデータの場合、展開されている場合は子の件数もカウントする
    var rows = table.getRows("active");
    var result = getChildrenRowCount(rows, count);
    rows = null;
    return result;
}

/**
 * Tabulatorのツリーの子の件数を取得
 * @param {any} 親要素
 * @param {any} 総件数
 * @param {any} ：ノードの開閉に関わらず件数を取得する場合true
 */
function getChildrenRowCount(rows, count, allFlag) {
    var childCount = count;
    $.each(rows, function (i, row) {
        var isTreeOpen = row.isTreeExpanded();
        if (isTreeOpen || allFlag) {
            childCount += row.getTreeChildren().length;
            getChildrenRowCount(row.getTreeChildren(), childCount, allFlag);
        }
    });
    return childCount;
}

/**
 * Tabulator一覧描画後に、モーダル画面の幅調整を行う
 * @param {any} tbl Tabulator
 * @param {any} id 一覧ID
 */
function setModalWidthAfterTableBuilt(tbl, id) {
    var modal = $(id).closest(".modal-content");
    if (!modal || modal.length == 0 || $(id).is(':hidden')) {
        //モーダル画面以外の場合、処理なし
        return;
    }

    //表示列の幅を取得
    var width = 10;
    var header = tbl.getColumnLayout();
    $.each(header, function (idx, head) {
        if (head.columns) {
            // ヘッダ複数行の場合
            $.each(head.columns, function (idx, lowerRow) {
                if (lowerRow.visible) {
                    width += lowerRow.width;
                }
            });
        } else {
            if (head.visible) {
                width += head.width;
            }
        }
    });
    header = null;
    //モーダル画面の幅調整
    var windowWidth = $(window).width();
    var modalWidth = $(modal).width();
    if (modalWidth < width) {
        var newWidth = 0;
        if (windowWidth - 50 <= width) {
            newWidth = windowWidth - 50;
        } else {
            newWidth = width + 50;
        }
        $(modal).width(newWidth);
    }
    modal = null;
}

/**
 * Tabulatorのコントロールの設定
 * @param {any} 要素
 * @param {any} 行番号
 * @param {any} IDリスト
 * @param {any} イベントリスト
 */
function setTabulatorControl(ele, rowNo, cloneIds, eventlist) {
    var orgId = $(ele).attr('id');
    var cloneId = orgId + "_" + rowNo;
    setAttrByNativeJs(ele, "id", cloneId);
    setAttrByNativeJs(ele, "name", $(ele).attr('name') + "_" + rowNo);
    cloneIds.push(cloneId);

    //設定されているイベントを取得
    eventlist[cloneId] = $._data($('#' + orgId).get(0), "events");
}

/**
 * ページャーを非表示設定
 * @param {any} テーブルID
 * @param {any} テーブル
 */
function setHidePagination(id, tbl) {
    //ページャーの総ページ数が1件の場合、ページャーを非表示
    var pageCount = 0;
    var pagination = $(P_Article).find(id + '_div').find('.paginationCommon');
    if (tbl && pagination != null && pagination.length > 0) {
        $.each(pagination, function () {
            var ctrlId = $(this).data("ctrlid");

            pageCount = getPageCountAll(ctrlId);
            var hideFlg = false;
            if (pageCount <= 1) {
                hideFlg = true;  //非表示
            }
            setHide($(P_Article).find(".paginationCommon[data-ctrlid='" + ctrlId + "']"), hideFlg);  //表示設定
        });

        if (pageCount <= 1) {
            return;
        }

        ////一覧下部のﾍﾟｰｼﾞｬｰの表示制御（非表示にする）
        //// ※一覧件数が一定数を超えない場合
        //// ※ﾀﾌﾞ切り替え画面の場合
        //var count = tbl.getDataCount("display");//表示中の行数
        //hideLowerPagination(count);

        //tabulator一覧の場合、下部ページャーは常に非表示とする(高さ指定可の為)
        var element = $(pagination).filter("[data-option='add']");
        setHide(element, true); //下部ﾍﾟｰｼﾞｬｰを非表示
        element = null;
    }
    pagination = null;
}

/**
 * Tabulatorのソート順データをローカルストレージへ保存
 * @param {string} conductId    ：機能ID
 * @param {number} formNo       ：画面番号
 * @param {string} ctrlId       ：一覧コントロールID
 * @param {object} sorter       ：ソート情報
 */
function saveTabulatorSortOrder(conductId, formNo, ctrlId, sorter) {
    if (sorter) {
        // ソート順が設定されている場合、ローカルストレージへ保存
        var data = [{ column: sorter['field'], dir: sorter['dir'] }];
        setSaveDataToLocalStorage(data, localStorageCode.SortOrder, conductId, formNo, ctrlId);
    } else {
        // ソート順が設定されていない場合、ローカルストレージから削除
        removeSaveDataFromLocalStorage(localStorageCode.SortOrder, conductId, formNo, ctrlId);
    }
}

/**
 * Tabulatorの1ページ表示件数をローカルストレージへ保存
 * @param {string} conductId    ：機能ID
 * @param {number} formNo       ：画面番号
 * @param {string} id           ：一覧ID(セレクタ)
 * @param {number} pageSize     ：Tabulatorの1ページ表示件数
 */
function saveTabulatorPageSize(conductId, formNo, id, pageSize) {
    // Tabulatorの1ページ表示件数を、ローカルストレージへ保存
    setSaveDataToLocalStorage(pageSize, localStorageCode.PageSize, conductId, formNo, id.replace('#', ''));
}

/**
 * Tabulatorの1ページ表示件数をローカルストレージから取得
 * @param {string} conductId    ：機能ID
 * @param {number} formNo       ：画面番号
 * @param {string} id           ：一覧ID(セレクタ)
 * @return {number} ：Tabulatorの1ページ表示件数
 */
function getTabulatorPageSize(conductId, formNo, id) {
    // Tabulatorの1ページ表示件数を、ローカルストレージから取得
    var pageSize = getSavedDataFromLocalStorage(localStorageCode.PageSize, conductId, formNo, id.replace('#', ''));
    return !pageSize ? -1 : pageSize;
}

/**
 * Tabulatorのページ番号をセッションストレージへ保存
 * @param {string} conductId    ：機能ID
 * @param {number} formNo       ：画面番号
 * @param {string} id           ：一覧ID(セレクタ)
 * @param {number} pageNo       ：Tabulatorのページ番号
 */
function saveTabulatorPageNo(conductId, formNo, id, pageNo) {
    // Tabulatorのページ番号を、セッションストレージへ保存
    setSaveDataToSessionStorageForList(pageNo, sessionStorageCode.PageNo, conductId, formNo, id.replace('#', ''));
}

/**
 * Tabulatorのページ番号をセッションストレージから取得
 * @param {string} conductId    ：機能ID
 * @param {number} formNo       ：画面番号
 * @param {string} id           ：一覧ID(セレクタ)
 * @return {number} ：Tabulatorのページ番号
 */
function getTabulatorPageNo(conductId, formNo, id) {
    // Tabulatorの1ページ表示件数を、セッションストレージから取得
    var pageNo = getSavedDataFromSessionStorageForList(sessionStorageCode.PageNo, conductId, formNo, id.replace('#', ''));
    return !pageNo ? 1 : pageNo;
}

/**
 * 数値セルの場合、フォーカス時に","を外す
 *  @param {html} element：ｲﾍﾞﾝﾄ発生要素
 */
function onFocusNum(element) {
    var tmpNum = $(element).val(); // 値を取得
    if (tmpNum != null && tmpNum != "") {
        // 値が設定されている場合、","を外し再設定
        tmpNum = tmpNum.replace(/,/g, "");
        $(element).val(tmpNum);
    }
}

/**
 * モーダル画面の表示
 *  @param {Element} modal：モーダル画面の要素
 */
function showModalForm(modal) {

    // モーダル画面の表示位置をリセット
    var modalContent = $(modal).find('.modal-content');
    var classCss = {
        "top": "0px",
        "left": "0px"
    }
    $(modalContent).css(classCss);

    // モーダル画面表示
    $(modal).modal();
}

/**
 * 現在のモーダル画面番号を取得
 * @param {Element} element：イベント発生要素
 * @return {number} 現在のモーダル画面番号(0:通常画面、1～:モーダル画面)
 */
function getCurrentModalNo(element) {
    if (element == null) { return 0; }

    // モーダル画面上の要素かどうかチェック
    var modal = $(element).closest('section.modal_form');
    if (modal == null || modal.length == 0) { return 0; }

    // モーダル画面番号を取得して返却
    return parseInt($(modal).data('modalno'), 10);
}

/**
 * モーダル画面要素の取得
 * @param   {number} modalNo    :取得対象のモーダル画面番号
 * @return  {HTMLElement}       :モーダル画面要素
 */
function getModalElement(modalNo) {
    var mainContents = $('section#main_contents');
    var modal = $(mainContents).find('.modal_form[data-modalno="' + modalNo + '"]');
    return modal;
}

/**
 * 次のモーダル画面要素の取得
 * @param   {number} currentModalNo   :現在のモーダル画面番号
 * @return  {HTMLElement} :次のモーダル画面要素
 */
function getNextModalElement(currentModalNo) {
    var nextModalNo = currentModalNo + 1;
    var mainContents = $('section#main_contents');
    var modal = $(mainContents).find('.modal_form[data-modalno="' + nextModalNo + '"]');
    P_formEditId = 'formEdit' + nextModalNo;
    if (modal == null || modal.length == 0) {
        // 指定番号のモーダル画面要素が存在しない場合は複製する
        modal = $('.modal_form[data-modalno="1"]').clone();
        setAttrByNativeJs(modal, 'data-modalno', nextModalNo);
        var formEdit = $(modal).find('form[id^="formEdit"]');
        setAttrByNativeJs(formEdit, 'id', P_formEditId);
        $(modal).appendTo(mainContents);
    }
    return modal;
}

/**
 * モーダル画面の表示イベント後の初期化処理
 * @param {Element} modal   ：モーダル画面要素
 */
function initShownModalForm(modal, isTreeView) {
    // 黒い背景のz-indexと透明度を調整
    const n = $('.modal:visible').length;
    const opacity = isTreeView ? 0 : 0.5 / n;
    var zIndex = 1040 + (10 * n);
    $(modal).css('z-index', zIndex);
    setTimeout(function () {
        $('.modal-backdrop')
            .not('.modal-stack')
            .css('z-index', zIndex - 1)
            .css('opacity', opacity)
            .addClass('modal-stack');
    }, 0);

    if ($(modal).find('.modal-content article[name="main_area"], .modal-content article[name="common_area"], .modal-content article[name="edit_area"]').length > 0) {
        //少しだけ幅を広げる
        var modalWidth = $(modal).find('.modal-content').width();
        $(modal).find('.modal-content').width(modalWidth + 30);
    }

    ////103一覧がモーダル内にある場合、再描画する
    //var tables = $(modal).find(".ctrlId.tabulator");
    //$.each(tables, function (i, table) {
    //    var id = $(table).attr('id');
    //    var tbl = P_listData["#" + id];
    //    if (tbl) {
    //        tbl.redraw(true);
    //    }
    //});

    if (!isTreeView) {
        // ドラッグ移動可能にする
        $(modal).find(".modal-content").draggable({ cancel: "table,.pagination_div,.tabulator,.tabulatorPagination" });
    }
}

/**
 * モーダル画面非表示イベント後の初期化処理
 * @param {string}  appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 * @param {Element} modal       ：モーダル画面要素
 * @param {number}  transPtn    ：画面遷移ﾊﾟﾀｰﾝ(1：子画面、2：単票入力、4：共通機能、5：他機能別ﾀﾌﾞ、6：他機能表示切替)
 */
function initHiddenModalForm(appPath, modal, transPtn) {
    backBtn = $(modal).find('.selected input:button[data-actionkbn="' + actionkbn.Back + '"]');
    var btnCtrlId = $(backBtn).attr("name");    //戻るﾎﾞﾀﾝ
    var currentModalNo = getCurrentModalNo(backBtn);

    //戻る処理
    if (transPtn == transPtnDef.Child) {
        // 子画面
        clickBackBtnForChildForm(appPath, btnCtrlId, null, currentModalNo - 1);
    } else if (transPtn == transPtnDef.Edit) {
        // 単票画面
        clickBackBtnForEditPopup(appPath, btnCtrlId, null);
    } else if (transPtn == transPtnDef.CmConduct) {
        // 共通画面
        clickBackBtnForCmConductForm(appPath, btnCtrlId, null);
    }

    //ﾓｰﾀﾞﾙ画面ｴﾘｱ初期化
    var detailDiv = $(modal).find('.form_detail_div');
    $(detailDiv).find("article[name='main_area']").remove();
    //ﾓｰﾀﾞﾙのﾒｯｾｰｼﾞをｸﾘｱ
    var messageDiv = $(modal).find('.message_div');
    $(messageDiv).children().remove();
    $(modal).find('.modal-content').width('');
}

/**
 * ストレージからツリービューで選択された最上部の構成IDの取得
 *  @param {number} grpId：構成グループID
 *  @return{Array.<number>} 選択された最上位の構成IDリスト
 */
function getSelectedStructureIdListFromStorage(grpId) {
    // 構成IDリスト
    var structureIdList = [];
    var code;
    if (grpId == structureGroupDef.Location) {
        code = localStorageCode.LocationTree;
    } else {
        code = localStorageCode.JobTree;
    }
    structureIdList = getStorageTreeData(grpId, code);
    return structureIdList;
}

/**
 * ツリービューで選択された最上部の構成IDの取得
 *  @param {number} grpId               ：構成グループID
 *  @param {treeViewDef} treeViewType   ：ツリービュー種類
 *  @param {boolean} isMerged           ：マージ表示かどうか
 *  @return{Array.<number>} 選択された最上位の構成IDリスト
 */
function getSelectedStructureIdList(grpId, treeViewType, isMerged) {
    var structureIdList = [];
    const treeViewId = '#' + getTreeViewId(grpId, treeViewType.Val);
    if (!$(treeViewId).jstree(true)) { return structureIdList; }

    //チェックが付いている最上部ノードのリストを取得
    var selectedNodes = $(treeViewId).jstree(true).get_top_checked(true);
    var selectList = [];

    // ルートノード(場所階層の場合は工場より上位のノード)の場合は1つ下位のノードを返す
    const maxLayerNo = grpId != structureGroupDef.Location ? 0 : structureLayerNo.Factory;
    $.each(selectedNodes, function (idx, node) {
        selectList = selectList.concat(getSelectedNodes(treeViewId, node, maxLayerNo));
    });

    $.each(selectList, function (idx, data) {
        //各ノードの構成IDを取得
        if (!isMerged) {
            var structureId = getTreeViewStructureId(data);
            if (structureIdList.indexOf(structureId) == -1 && structureId > 0) {
                structureIdList.push(structureId);
            }
        } else {
            var idList = getTreeViewStructureIdList(data);
            if (idList != null && idList.length > 0) {
                //★2024/06/20 TMQ応急対応 C#側でマージ処理実行 start
                //$.each(idList, function (i, id) {
                //    if (structureIdList.indexOf(id) < 0) {
                //        structureIdList.push(id);
                //    }
                //});
                if (idList.length == 1) {
                    structureIdList.push(idList[0]);
                } else {
                    // 対応する工場ID配列を取得
                    var facrotyIdList = getTreeViewMergeFacrotyIdList(data);
                    // 選択中の工場IDと一致する構成IDのみを取得
                    if (P_SelectedFactoryIdList) {
                        $.each(P_SelectedFactoryIdList, function (i, id) {
                            var idx = facrotyIdList.indexOf(id);
                            if (idx >= 0) {
                                // 工場のインデックスと同じ位置の構成IDを追加
                                var structureId = idList[idx];
                                if (structureIdList.indexOf(structureId)) {
                                    structureIdList.push(structureId);
                                }
                            }
                        });
                    }
                }
                //★2024/06/20 TMQ応急対応 C#側でマージ処理実行 end
            }
        }
    });
    return structureIdList;
}

/**
 * ツリービューで選択された最上部のノードの取得
 * @param {string} treeViewId   :ツリービューのID
 * @param {object} node         :選択ノード
 * @param {number} maxLayerNo   :最上位の階層番号
 */
function getSelectedNodes(treeViewId, node, maxLayerNo) {
    var layerNo = getTreeViewLayerNo(node);
    if (layerNo >= maxLayerNo) {
        return [node];
    }

    let childNodes = [];
    $.each(node.children, function (idx, childNodeId) {
        var childNode = $(treeViewId).jstree(true).get_node(childNodeId);
        childNodes = childNodes.concat(getSelectedNodes(treeViewId, childNode, maxLayerNo));
    });
    return childNodes;
}

/**
 * 初期化時にコンボボックスの先頭を常に選択する(未選択にしない)要素か判定する処理
 * @param {any} combo コンボボックス
 */
function isNecessaryComboSelectTop(combo) {
    return $(combo).hasClass("select_top_necessary");
}


/**
 *  画面のリンクによるファイルダウンロード/ページ表示
 *  @param {string}     appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
 *  @param {number}     formNo      ：遷移元formNo
 *  @param {string}     ctrlId      ：遷移元の一覧ctrlid
 *  @param {html}       element     ：ｲﾍﾞﾝﾄ発生要素
 */
function fileDownloadLink(appPath, formNo, ctrlId, element) {
    // reportCreateの処理を参考に作成

    // 実行中フラグON
    P_ProcExecuting = true;
    // 画面の情報を取得
    var form = $(element).closest("article").find("form[id^='form']");
    var conductId = $(form).find("input:hidden[name='CONDUCTID']").val();
    var pgmId = $(form).find("input:hidden[name='PGMID']").val();
    // リンクに設定されたキー情報を取得
    var keyInfo = $(element).attr("href");

    var isLink = function (keyInfo) {
        var keyInfos = keyInfo.split('_'); // キー情報は添付情報などのキーIDを_で繋げたもの
        var isFile = keyInfos[keyInfos.length - 1] == '1'; // ファイル/リンク区分は最後の要素、ファイルなら1、リンクなら2
        // ファイルの場合サーバにアクセスするので、ファイルでなければリンクとして、不明なデータはリンクの場合の処理を行うようにする
        return !isFile;
    }

    if (isLink(keyInfo)) {
        var url = $(element).text();
        window.open(url, "_blank");
        // 実行中フラグOFF
        P_ProcExecuting = false;
        return false;
    }

    // POSTデータを生成
    var postdata = {
        conductId: conductId,                       // メニューの機能ID
        pgmId: pgmId,                               // メニューのプログラムID
        formNo: formNo,                             // 画面番号
        ctrlId: "Download",                          // ファイルダウンロードのコントロールID
        ListIndividual: P_dicIndividual,            // 個別実装用汎用ﾘｽﾄ
        FileDownloadSet: FileDownloadSet.Hidouki,   // 1:非同期
        browserTabNo: P_BrowserTabNo,   // ブラウザタブ識別番号
        FileDownloadInfo: keyInfo, // ダウンロードファイル情報
    };

    $.ajax({
        url: appPath + 'api/CommonProcApi/' + actionkbn.Report,   // Excel作成と同じ
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify(postdata),
        headers: { 'X-XSRF-TOKEN': getRequestVerificationToken() },
        traditional: true,
        cache: false
    }).then(
        // 1つめは通信成功時のコールバック
        function (resultInfo) {
            //正常時
            var status = resultInfo[0];                     //[0]:処理ステータス - CommonProcReturn
            var data = separateDicReturn(resultInfo[1], conductId);    //[1]:結果データ - Dictionary<string, object>※結果ﾃﾞｰﾀ："Result"、個別実装用ﾃﾞｰﾀ："Individual"

            // メッセージをクリア
            clearMessage();

            //処理メッセージを表示
            if (status.MESSAGE != null && status.MESSAGE.length > 0) {
                //正常時、正常ﾒｯｾｰｼﾞ
                if (status.MESSAGE == P_ComMsgTranslated[941060001]) {
                    // 「該当データがありません。」の場合はステータスをErrorに設定し直す
                    // (ファイルダウンロード処理を実行させるため、ロジック側からはValidで渡ってくるため)
                    addMessage(status.MESSAGE, procStatus.Error);
                }
                else {
                    addMessage(status.MESSAGE, status.STATUS);
                }
            }

            if (data != null) {
                // 結果データが存在する場合、再検索結果の反映
                setExecuteResults(appPath, conductId, pgmId, formNo, btnCtrlId, conductPtn, false, false, data);
            } else if (data == null && P_buttonDefine[conductId] != null && P_buttonDefine[conductId].length != 0) {
                // 結果データが存在せず、ボタン情報が存在する場合、ボタンの権限制御のみ実施
                setButtonStatus();
            }

            // ダウンロードファイル名
            var fileName = status.FILEDOWNLOADNAME;
            var filePath = status.FILEPATH;

            if (fileName != null && fileName.length > 0) {
                // ダウンロードファイル名が指定されている場合のみ、ダウンロード処理実行
                var formDetail = $(P_Article).find("#" + P_formDetailId);
                if (formDetail == null || formDetail.length == 0) {
                    // 単票画面の場合(機器台帳の保全項目編集など)、編集画面となる
                    formDetail = $(P_Article).find("#" + P_formEditId);
                }
                setAttrByNativeJs(formDetail, "method", "POST");
                setAttrByNativeJs(formDetail, "action", appPath + "Common/Report?output=2&fileName=" + fileName + "&filePath=" + filePath);

                $(formDetail).submit();
            }
            // 画面変更ﾌﾗｸﾞ初期化
            dataEditedFlg = false;

        },
        // 2つめは通信失敗時のコールバック
        function (resultInfo) {
            // 検索処理失敗

            // メッセージをクリア
            clearMessage();

            //処理結果ｽﾃｰﾀｽ(CommonProcReturn)
            var result = resultInfo.responseJSON;
            var status;
            if (result.length > 1) {
                status = result[0];
                var data = separateDicReturn(result[1], conductId);

                //エラー詳細を表示
                dispErrorDetail(data);
            }
            else {
                status = result;
            }

            var eventFunc = function () {
                fileDownloadLink(appPath, formNo, ctrlId, element);
            }

            //処理結果ｽﾃｰﾀｽを画面状態に反映
            if (!setReturnStatus(appPath, status, eventFunc)) {
                return false;
            }

        }
    ).always(
        //通信の完了時に必ず実行される
        function (resultInfo) {
            //処理中メッセージ：off
            processMessage(false);
            // 実行中フラグOFF
            P_ProcExecuting = false;
        });

}

/**
 * 選択中の画面のフォーカスをクリア
 * */
function removeFocusSelected() {
    //選択中画面NOｴﾘｱ
    if (P_Article != null && P_Article.length > 0) {
        // フォーカスをクリア
        removeFocus();
    }
}

/**
 * コード+翻訳 クリアボタン押下時の処理
 * @param {any} btn クリアボタン
 */
function clearTrans(btn) {
    // クリアボタン押下時、選択されたコードをクリアし、翻訳も削除する
    // クリアボタンのIDを取得
    var idBtnC = $(btn)[0].id;
    // IDの末尾を削ったものが選択されたコードのID
    var idInput = idBtnC.replace("BtnC", "");
    var text = $("#" + idInput);
    // 値をクリア
    text[0].value = "";
    // トリガー処理で翻訳
    $(text).trigger("change");
}

/**
 *  新規ウィンドウ表示処理
 *  @param {string} appPath     ：ｱﾌﾟﾘｹｰｼｮﾝﾙｰﾄﾊﾟｽ
*/
function showNewWindow(appPath) {
    // Form要素生成
    var form = '<form id="postForm" style="display:none;">';
    form += '<input type="hidden" name="CONDUCTID" value="CM00001">';
    form += '<input type="hidden" name="FORMNO" value="0">';
    form += '<input name="__RequestVerificationToken" type="hidden" value="' + getRequestVerificationToken() + '">';
    form += '</form>';
    $(form).appendTo("body");
    $("#postForm").attr("action", appPath + 'Common');
    $("#postForm").attr("method", "POST");
    $("#postForm").attr("target", "_blank");
    $("#postForm").submit();
    $("#postForm").remove();
}
