///<summary>
/// 機能名　　：　【共通】
/// タイトル　：　共通データ定数定義クラス
/// 説明　　　：　共通データテーブル内で使用する定数を定義します。
/// 
/// 履歴　　　：　2017.08.01 河村純子　新規作成
///</summary>

namespace CommonWebTemplate.Models.Common
{
    /// <summary>
    /// 共通＿機能マスタ
    /// </summary>
    public static class CONDUCT_MST_CONSTANTS
    {
        /// <summary>
        /// 1階層ｸﾞﾙｰﾌﾟ
        /// </summary>
        public const string conductGrpTop = "0000000";

        /// <summary>
        /// 機能パターン
        /// </summary>
        public static class PTN
        {
            /// <summary>入力機能</summary>
            public const int Input = 10;
            /// <summary>バッチ機能</summary>
            public const int Bat = 11;
            /// <summary>帳票機能</summary>
            public const int Report = 20;
            /// <summary>マスタメンテナンス</summary>
            public const int Master = 30;
        }

        /// <summary>
        /// メニュー表示区分
        /// </summary>
        public static class MENUDISP
        {
            /// <summary>非表示</summary>
            public const short Hide = 0;
            /// <summary>表示</summary>
            public const short Disp = 1;
        }
    }

    /// <summary>
    /// 共通＿画面定義
    /// </summary>
    public static class FORM_DEFINE_CONSTANTS
    {
        /// <summary>
        /// 画面ｺﾝﾄﾛｰﾙﾀｲﾌﾟ
        /// </summary>
        public static class CTRLTYPE
        {
            ///// <summary>一覧</summary>
            //public const byte Ichiran = 0;
            ///// <summary>ボタン</summary>
            //public const byte Button = 1;

            /// <summary>データ用一覧パターン①</summary>
            public const short IchiranPtn1 = 101;
            /// <summary>データ用一覧パターン②</summary>
            public const short IchiranPtn2 = 102;
            /// <summary>データ用一覧パターン③</summary>
            public const short IchiranPtn3 = 103;
            /// <summary>コントロールグループ</summary>
            public const short ControlGroup = 201;
            /// <summary>バッチステータス用定型一覧</summary>
            public const short BatStatus = 301;
            /// <summary>(未指定)</summary>
            public const short None = 9;
        }

        /// <summary>
        /// 定義エリア区分
        /// </summary>
        //public static class AREAKBN {
        //    /// <summary>条件ｴﾘｱ</summary>
        //    public const byte Condition = 0;
        //    /// <summary>明細ｴﾘｱ</summary>
        //    public const byte List = 1;
        //    /// <summary>ﾎﾟｯﾌﾟｱｯﾌﾟ入力ｴﾘｱ</summary>
        //    public const byte Input = 2;
        //    /// <summary>明細ｴﾘｱ、ﾎﾟｯﾌﾟｱｯﾌﾟ入力ｴﾘｱ)</summary>
        //    /// <remarks>※内部処理制御用</remarks>
        //    public const byte Detail = 88;
        //    /// <summary>(未指定)</summary>
        //    public const byte None = 99;
        //    /// <remarks>※内部処理制御用</remarks>
        //}
        public static class AREAKBN
        {
            /// <summary>条件ｴﾘｱ</summary>
            public const short Condition = 1;
            /// <summary>明細ｴﾘｱ</summary>
            public const short List = 2;
            /// <summary>単票表示ｴﾘｱ</summary>
            public const short Input = 3;
            /// <summary>ﾄｯﾌﾟｴﾘｱ</summary>
            public const short Top = 4;
            /// <summary>ﾎﾞﾄﾑｴﾘｱ</summary>
            public const short Bottom = 5;

            /// <summary>明細ｴﾘｱ、ﾎﾟｯﾌﾟｱｯﾌﾟ入力ｴﾘｱ)</summary>
            /// <remarks>※内部処理制御用</remarks>
            public const short Detail = 88;
            /// <summary>(未指定)</summary>
            public const short None = 99;
            /// <remarks>※内部処理制御用</remarks>
        }

        /// <summary>
        /// タブ番号
        /// </summary>
        //public static class DISPKBN
        //{
        //    /// <summary>非表示</summary>
        //    public const short Hide = -1;
        //    /// <summary>並べて表示</summary>
        //    /// <remarks>※デフォルト値</remarks>
        //    public const short Normal = 0;
        //    /// <summary>切替表示</summary>
        //    public const short Tab = 1;
        //}
        public static class TABNO
        {
            ///// <summary>非表示</summary>
            //public const short Hide = -1;
            /// <summary>並べて表示</summary>
            /// <remarks>※デフォルト値</remarks>
            public const short Normal = 0;
            /// <summary>タブ表示</summary>
            public const short Tab = 1;
        }

        ///// <summary>
        ///// ｱｸｼｮﾝ区分
        ///// </summary>
        ///// <remarks>【共通】…共通FWで自動生成</remarks>
        //public static class ACTIONKBN
        //{
        //    /// <summary>(未指定)：※初期化用</summary>
        //    public const short None = 9999;

        //    /// <summary>一覧ソート</summary>
        //    public const short ListSort = -2;
        //    /// <summary>データ取得</summary>
        //    public const short DataGet = -1;
        //    /// <summary>検索</summary>
        //    public const short Search = 0;
        //    /// <summary>実行</summary>
        //    public const short Execute = 1;
        //    /// <summary>クリア</summary>
        //    public const short Clear = 2;
        //    /// <summary>戻る</summary>
        //    public const short Back = 3;
        //    /// <summary>Excel出力</summary>
        //    public const short Report = 5;
        //    /// <summary>Excel出力(非同期)</summary>
        //    public const short ReportHidoki = 51;
        //    /// <summary>取込(CSV,EXCEL)</summary>
        //    public const short Upload = 8;
        //    /// <summary>削除</summary>
        //    public const short Delete = 9;
        //    /// <summary>選択</summary>
        //    public const short Select = 10;

        //    /// <summary>行追加</summary>
        //    public const short AddNewRow = 20;
        //    /// <summary>行削除</summary>
        //    public const short DeleteRow = 21;
        //    /// <summary>全選択</summary>
        //    public const short SelectAll = 22;
        //    /// <summary>全解除</summary>
        //    public const short CancelAll = 23;
        //    /// <summary>一覧情報表示</summary>
        //    public const short ComSwitchTable = 24;
        //    /// <summary>一覧表示列設定</summary>
        //    public const short ComSetDispItem = 25;
        //    /// <summary>一覧表示列設定適用</summary>
        //    public const short ComSetDispItemExec = 26;
        //    /// <summary>【一覧表示列設定登録</summary>
        //    public const short ComSetDispItemSave = 27;

        //    /// <summary>【共通 - ログアウト】ログアウト実行</summary>
        //    public const short ComLogout = 80;
        //    /// <summary>【共通 - TOP遷移】サイトTOPに遷移</summary>
        //    public const short ComRedirectTop = 81;
        //    /// <summary>【共通 - ＩＤ切替】ＩＤ切替ページに遷移</summary>
        //    public const short ComIdChange = 82;
        //    /// <summary>【共通 - パスワード変更】パスワード変更フォームポップアップ</summary>
        //    public const short ComPassChangeForm = 83;
        //    /// <summary>【共通 - パスワード変更】パスワード変更実行</summary>
        //    public const short ComPassChangeExec = 84;
        //    /// <summary>【共通 - バッチ機能】バッチ実行</summary>
        //    public const short ComBatExec = 90;
        //    /// <summary>【共通 - バッチ機能】バッチ再表示</summary>
        //    public const short ComBatRefresh = 91;
        //    /// <summary>【共通 - マスタ機能】表示切替</summary>
        //    public const short ComSwitch = 92;
        //    /// <summary>【共通 - 取込機能】ファイル取込</summary>
        //    /// <remarks>※template2.0：画面定義可能なボタンにアップグレード</remarks>
        //    public const short ComUpload = 93;
        //    /// <summary>【共通 - 画面初期化機能】初期値データ取得</summary>
        //    public const short ComInitForm = 94;
        //    /// <summary>【共通 - 個別実装】権限チェック単独実施</summary>
        //    public const short ComOnlyChkAuth = 95;
        //    /// <summary>【共通 - 双方向通信】ユーザーID取得</summary>
        //    public const short ComGetUserId = 96;

        //    /// <summary>子画面新規</summary>
        //    /// <remarks>★AKAP2.0カスタマイズ★</remarks>
        //    public const short ChildNew = 101;
        //    /// <summary>単票入力新規</summary>
        //    /// <remarks>★AKAP2.0カスタマイズ★</remarks>
        //    public const short EditNew = 102;
        //}

        /// <summary>
        /// 画面遷移ﾊﾟﾀｰﾝ
        /// </summary>
        //public static class TRANSKBN
        //{
        //    /// <summary>画面遷移なし</summary>
        //    public const short None = 1;
        //    /// <summary>入力画面ﾎﾟｯﾌﾟｱｯﾌﾟ表示</summary>
        //    public const short DetailPopup = 2;
        //    /// <summary>入力画面ﾎﾟｯﾌﾟｱｯﾌﾟ表示(一覧更新列なし)</summary>
        //    public const short DetailUpdNone = 21;
        //    /// <summary>子画面遷移</summary>
        //    public const short ChildTrans = 3;
        //    /// <summary>子画面ﾎﾟｯﾌﾟｱｯﾌﾟ表示</summary>
        //    public const short ChildPopup = 4;
        //}
        public static class DAT_TRANSPTN
        {
            /// <summary>遷移なし</summary>
            public const short None = 0;
            /// <summary>子画面表示</summary>
            public const short Child = 1;
            /// <summary>単票入力表示</summary>
            public const short Edit = 2;
            /// <summary>単票参照表示</summary>
            public const short Reference = 3;
            /// <summary>共通機能表示</summary>
            public const short CmConduct = 4;
            /// <summary>他機能遷移(別タブ)</summary>
            /// <remarks>※内部で使用</remarks>
            public const short OtherTab = 5;
            /// <summary>他機能遷移(同タブ表示切替)</summary>
            /// <remarks>※内部で使用</remarks>
            public const short OtherShift = 6;

        }
        /// <summary>
        /// 遷移表示ﾊﾟﾀｰﾝ
        /// </summary>
        public static class DAT_TRANSDISPPTN
        {
            /// <summary>設定なし</summary>
            public const short None = 0;
            /// <summary>表示切替</summary>
            public const short Shift = 1;
            /// <summary>ポップアップ</summary>
            public const short Popup = 2;
            /// <summary>一覧直下</summary>
            public const short UnderList = 3;
        }

        /// <summary>
        /// 一覧編集ﾊﾟﾀｰﾝ
        /// </summary>
        //public static class EDITKBN
        //{
        //    /// <summary>表示のみ</summary>
        //    public const short ReadOnly = 0;
        //    /// <summary>直接入力</summary>
        //    public const short Input = 1;
        //}
        public static class DAT_EDITPTN
        {
            /// <summary>設定なし</summary>
            public const short None = 0;
            /// <summary>直接編集可</summary>
            public const short Input = 1;
            /// <summary>直接編集不可</summary>
            public const short ReadOnly = 2;
        }

        ///// <summary>
        ///// 編集区分ﾊﾟﾀｰﾝ(検索後の条件ｴﾘｱ制御)
        ///// </summary>
        ///// <remarks>※検索ボタンの場合</remarks>
        //public static class EDITKBN_A
        //{
        //    /// <summary>表示のみ</summary>
        //    public const short ReadOnly = 0;
        //    /// <summary>編集可能</summary>
        //    public const short Input = 1;
        //}

        /// <summary>
        /// 一覧表示方向
        /// </summary>
        //public static class DAT_DIRECTION
        //{
        //    /// <summary>横方向に列表示</summary>
        //    /// <remarks>※デフォルト値</remarks>
        //    public const short Horizontal = 0;
        //    /// <summary>縦方向に列表示</summary>
        //    public const short Vertical = 1;
        //}
        public static class DAT_DIRECTION
        {
            /// <summary>未使用</summary>
            public const short None = 0;
            /// <summary>横方向一覧</summary>
            /// <remarks>※デフォルト値</remarks>
            public const short Horizontal = 1;
            /// <summary>縦方向一覧</summary>
            public const short Vertical = 2;
        }

        /// <summary>
        /// ヘッダ行表示区分
        /// </summary>
        public static class DAT_HEADERDISPKBN
        {
            /// <summary>ヘッダ非表示</summary>
            public const short Hide = 0;
            /// <summary>ヘッダ表示</summary>
            /// <remarks>※デフォルト値</remarks>
            public const short Disp = 1;
        }

        /// <summary>
        /// 一覧表示切替設定
        /// </summary>
        public static class DAT_SWITCHKBN
        {
            /// <summary>設定なし</summary>
            /// <remarks>※デフォルト値</remarks>
            public const short None = 0;
            /// <summary>表示</summary>
            public const short Disp = 1;
        }

        /// <summary>
        /// 一覧行追加設定
        /// </summary>
        public static class DAT_ROWADDKBN
        {
            /// <summary>設定なし</summary>
            /// <remarks>※デフォルト値</remarks>
            public const short None = 0;
            /// <summary>表示(新規行追加)</summary>
            public const short AddNewRow = 1;
            /// <summary>表示(新規画面表示)</summary>
            public const short TransNewForm = 2;
            /// <summary>表示(新規行追加、コピー行追加)</summary>
            public const short AddNewOrCopyRow = 3;
            /// <summary>表示(新規画面表示、コピー画面表示)</summary>
            public const short TransNewOrCopyForm = 4;
        }

        /// <summary>
        /// 一覧行削除設定
        /// </summary>
        public static class DAT_ROWDELKBN
        {
            /// <summary>設定なし</summary>
            /// <remarks>※デフォルト値</remarks>
            public const short None = 0;
            /// <summary>表示</summary>
            public const short Disp = 1;
        }

        /// <summary>
        /// 一覧全選択全解除設定
        /// </summary>
        public static class DAT_ROWSELKBN
        {
            /// <summary>設定なし</summary>
            /// <remarks>※デフォルト値</remarks>
            public const short None = 0;
            /// <summary>表示(頁データ)</summary>
            public const short CurrentPage = 1;
            /// <summary>表示(全データ)</summary>
            public const short AllPages = 2;
            /// <summary>表示(頁データ・全データ両方)</summary>
            public const short Both = 3;
        }

        /// <summary>
        /// ユーザー単位の表示列選択設定
        /// </summary>
        public static class DAT_COLSELKBN
        {
            /// <summary>設定なし</summary>
            /// <remarks>※デフォルト値</remarks>
            public const short None = 0;
            /// <summary>表示</summary>
            public const short Disp = 1;
        }

        /// <summary>
        /// 一覧移動設定
        /// </summary>
        public static class DAT_ROWSORTKBN
        {
            /// <summary>設定なし</summary>
            /// <remarks>※デフォルト値</remarks>
            public const short None = 0;
            /// <summary>表示</summary>
            public const short Sortable = 1;
        }

        /// <summary>
        /// 遷移リンク区分
        /// </summary>
        public static class DAT_TRANSICONKBN
        {
            /// <summary>設定なし</summary>
            /// <remarks>※デフォルト値</remarks>
            public const short None = 0;
            /// <summary>行番号リンク</summary>
            public const short RowNumLink = 1;
            /// <summary>鉛筆アイコンリンク</summary>
            public const short PencilIcon = 2;
            /// <summary>行番号ラベル</summary>
            public const short RowNumLabel = 3;
            /// <summary>矢印アイコンリンク</summary>
            public const short ArrowIcon = 4;
        }

        /// <summary>
        /// コントロールグループ配置区分
        /// </summary>
        public static class CTR_POSITIONKBN
        {
            /// <summary>設定なし</summary>
            /// <remarks>※デフォルト値</remarks>
            public const short None = 0;
            /// <summary>上部</summary>
            public const short Upper = 1;
            /// <summary>下部</summary>
            public const short Lower = 2;
            /// <summary>画面右側</summary>
            public const short RightSide = 3;
        }

        /// <summary>
        /// 表示モード区分
        /// </summary>
        public static class REFERENCE_MODEKBN
        {
            /// <summary>編集モード</summary>
            /// <remarks>※デフォルト値</remarks>
            public const short Edit = 0;
            /// <summary>参照モード</summary>
            public const short Reference = 1;
        }
    }

    /// <summary>
    /// 共通＿一覧項目定義
    /// </summary>
    public static class LISTITEM_DEFINE_CONSTANTS
    {
        /// <summary>
        /// 定義種類
        /// </summary>
        public static class DEFINETYPE
        {
            /// <summary>データ項目定義</summary>
            public const short DataRow = 1;
            /// <summary>ヘッダ項目定義</summary>
            public const short HeadRow = 2;
            /// <summary>フッタ項目定義</summary>
            public const short FootRow = 3;
            /// <summary>(未指定)</summary>
            public const short None = 9;
        }

        /// <summary>
        /// 表示区分（単票表示用設定）
        /// </summary>
        /// <remarks>横方向一覧の場合</remarks>
        public static class DISPKBN
        {
            /// <summary>非表示</summary>
            public const short Hide = -1;
            /// <summary>一覧、単票両方表示</summary>
            public const short Both = 1;
            /// <summary>単票のみ表示</summary>
            public const short InputOnly = 2;
            /// <summary>一覧のみ表示</summary>
            public const short ListOnly = 3;
        }
        /// <summary>
        /// 表示区分（通常設定）
        /// </summary>
        public static class DISPKBN_DEF
        {
            /// <summary>非表示</summary>
            public const short Hide = -1;
            /// <summary>常に表示</summary>
            public const short Disp = 1;
            /// <summary>デフォルト非表示</summary>
            public const short DefHide = 2;
        }

        /// <summary>
        /// 行番号
        /// </summary>
        public static class ROWNO
        {
            /// <summary>未設定</summary>
            public const short None = 0;
            /// <summary>初期値</summary>
            public const short Default = 1;
        }

        /// <summary>
        /// 列番号
        /// </summary>
        public static class COLNO
        {
            /// <summary>未設定</summary>
            public const short None = 0;
            /// <summary>初期値</summary>
            public const short Default = 1;
        }

        /// <summary>
        /// ヘッダタイトル表示位置
        /// </summary>
        public static class HEADER_ALIGN
        {
            /// <summary>未設定(不使用時)</summary>
            public const string None = "0";
            /// <summary>左寄せ</summary>
            public const string left = "1";
            /// <summary>中央</summary>
            public const string center = "2";
            /// <summary>右寄せ</summary>
            public const string right = "3";
        }

        /// セルタイプ
        /// </summary>
        public static class CELLTYPE
        {
            /// <summary>テキスト</summary>
            public const string Text = "0101";
            /// <summary>数値</summary>
            public const string Number = "0102";
            /// <summary>コード＋翻訳</summary>
            public const string CodeTrans = "0103";
            /// <summary>日付(DatePicker)</summary>
            public const string DatePicker = "0104";
            /// <summary>時刻(TimePicker)</summary>
            public const string TimePicker = "0105";
            /// <summary>日時(DateTimePicker)</summary>
            public const string DateTimePicker = "0106";
            /// <summary>年月(DatePicker)</summary>
            public const string YearMonthPicker = "0107";
            /// <summary>テキストエリア</summary>
            public const string Textarea = "0201";
            /// <summary>ラベル</summary>
            public const string Label = "0301";
            ///// <summary>ラベル(左寄せ)</summary>
            //public const string LabelLeft = "0301";
            ///// <summary>ラベル(中央)</summary>
            //public const string LabelCenter = "0302";
            ///// <summary>ラベル(右寄せ)</summary>
            //public const string LabelRight = "0303";
            /// <summary>計算ラベル</summary>
            public const string CalcLabel = "0302";
            /// <summary>階層選択ラベル</summary>
            public const string TreeLabel = "0303";
            /// <summary>チェックボックス</summary>
            public const string CheckBox = "0401";
            /// <summary>削除チェックボックス</summary>
            public const string CheckBoxDel = "0402";
            /// <summary>複数選択チェックボックス</summary>
            public const string MultiCheckBox = "0501";
            /// <summary>ラジオボタン</summary>
            public const string RadioButton = "0601";
            /// <summary>コンボボックス</summary>
            public const string ComboBox = "0701";
            /// <summary>リストボックス</summary>
            public const string ListBox = "0702";
            /// <summary>ボタン</summary>
            public const string Button = "0801";
            /// <summary>日付(ブラウザ標準)</summary>
            public const string Date = "0901";
            /// <summary>時刻(ブラウザ標準)</summary>
            public const string Time = "0902";
            /// <summary>日時(ブラウザ標準)</summary>
            public const string DateTime = "0903";
            /// <summary>ファイル選択</summary>
            public const string File = "1001";
            /// <summary>ダウンロードリンク</summary>
            public const string Download = "1101";
            /// <summary>ファイル参照リンク</summary>
            public const string FileOpen = "1102";
            /// <summary>画面遷移リンク</summary>
            public const string FormTransition = "1111";
            ///// <summary>子画面遷移リンク</summary>
            //public const string TransChild = "1111";
            ///// <summary>共通機能ポップアップリンク</summary>
            //public const string TransCm = "1112";
            ///// <summary>他機能遷移リンク(別タブ)</summary>
            //public const string TransOtherTab = "1113";
            ///// <summary>他機能遷移リンク(同タブ遷移)</summary>
            //public const string TransOtherShift = "1114";
            /// <summary>画像</summary>
            public const string Image = "1201";
            /// <summary>グラフ</summary>
            public const string Graph = "1301";
            /// <summary>ツリービュー</summary>
            public const string TreeView = "1401";
            /// <summary>パスワード</summary>
            public const string PassWord = "1501";

            /// <summary>カスタムコントロール</summary>
            public const string Custom = "9001";

        }

        /// <summary>
        /// FromTo項目区分
        /// </summary>
        public static class FROMTOKBN
        {
            /// <summary>設定なし</summary>
            public const short None = 0;
            /// <summary>FromTo項目</summary>
            public const short FromTo = 1;
        }

        /// <summary>
        /// 必須項目区分
        /// </summary>
        public static class NULLCHKKBN
        {
            /// <summary>設定なし</summary>
            public const short None = 0;
            /// <summary>必須入力項目</summary>
            public const short Required = 1;
        }

        /// <summary>
        /// オートコンプリート区分
        /// </summary>
        public static class TXT_AUTOCOMPKBN
        {
            /// <summary>設定なし</summary>
            public const short None = 0;
            /// <summary>オートコンプリート項目(コード＋翻訳)</summary>
            public const short AutoComp = 1;
            /// <summary>オートコンプリート項目(翻訳)</summary>
            public const short AutoCompTransOnly = 2;
            /// <summary>オートコンプリート項目(コード)</summary>
            public const short AutoCompCodeOnly = 3;
        }

        /// <summary>
        /// リストボックス選択区分
        /// </summary>
        public static class LISTBOXKBN
        {
            /// <summary>単一選択</summary>
            public const short Single = 0;
            /// <summary>複数選択</summary>
            public const short Multiple = 1;
        }

        /// <summary>
        /// ｱｸｼｮﾝ区分
        /// </summary>
        /// <remarks>【共通】…共通FWで自動生成</remarks>
        public static class ACTIONKBN
        {
            /* テーブルに定義するコードは3桁 */
            /// <summary>検索</summary>
            public const short Search = 101;
            /// <summary>バッチ再表示</summary>
            public const short ComBatRefresh = 102;
            /// <summary>戻る</summary>
            public const short Back = 111;
            /// <summary>選択</summary>
            public const short Select = 112;
            /// <summary>画面遷移</summary>
            public const short FormTransition = 121;
            ///// <summary>子画面新規</summary>
            //public const short ChildNew = 121;
            ///// <summary>単票入力新規</summary>
            //public const short EditNew = 122;
            ///// <summary>共通機能ポップアップ</summary>
            //public const short TransCm = 131;
            ///// <summary>他機能遷移(別タブ)</summary>
            //public const short TransOtherTab = 132;
            ///// <summary>他機能遷移(同タブ遷移)</summary>
            //public const short TransOtherShift = 133;
            /// <summary>実行</summary>
            public const short Execute = 201;
            /// <summary>削除</summary>
            public const short Delete = 211;
            /// <summary>バッチ実行</summary>
            public const short ComBatExec = 221;
            /// <summary>取込フォームポップアップ</summary>
            public const short Upload = 301;
            /// <summary>ファイル取込実行</summary>
            public const short ComUpload = 302;
            /// <summary>Excel出力</summary>
            public const short Report = 311;
            /// <summary>Excel出力(非同期)</summary>
            public const short ReportHidoki = 312;
            /// <summary>ExcelPort(ダウンロード)</summary>
            public const short ExcelPortDownload = 313;
            /// <summary>ExcelPort(アップロード)</summary>
            public const short ExcelPortUpload = 314;

            /// <summary>クリア</summary>
            public const short Clear = 401;
            /// <summary>個別実装</summary>
            public const short Individual = 901;

            /* 内部制御で使用するコードは4桁 */
            /* 1XXX：ユーザーのアクションで直接発火するもの */
            /// <summary>【共通 - TOP遷移】サイトTOPに遷移</summary>
            public const short ComRedirectTop = 1001;
            /// <summary>【共通 - ログアウト】ログアウト実行</summary>
            public const short ComLogout = 1002;
            /// <summary>【共通 - ＩＤ切替】ＩＤ切替ページに遷移</summary>
            public const short ComIdChange = 1003;
            /// <summary>【共通 - パスワード変更】パスワード変更フォームポップアップ</summary>
            public const short ComPassChangeForm = 1004;
            /// <summary>【共通 - パスワード変更】パスワード変更実行</summary>
            public const short ComPassChangeExec = 1005;
            /// <summary>【共通 - 表示切替】検索条件表示切替</summary>
            public const short ComSwitch = 1201;
            /// <summary>一覧表示切替</summary>
            public const short ComSwitchTable = 1211;
            /// <summary>行追加</summary>
            public const short AddNewRow = 1221;
            /// <summary>行コピー</summary>
            public const short AddCopyRow = 1222;
            /// <summary>行削除</summary>
            public const short DeleteRow = 1231;
            /// <summary>全選択</summary>
            public const short SelectAll = 1241;
            /// <summary>全解除</summary>
            public const short CancelAll = 1242;
            /// <summary>ユーザー単位の表示列設定</summary>
            public const short ComSetDispItem = 1251;
            /// <summary>ユーザー単位の表示列設定適用</summary>
            public const short ComSetDispItemExec = 1252;
            /// <summary>ユーザー単位の表示列設定登録</summary>
            public const short ComSetDispItemSave = 1253;
            /// <summary>詳細検索メニュー表示</summary>
            public const short ComDetailSearch = 1254;
            /// <summary>詳細検索実行</summary>
            public const short ComDetailSearchExec = 1255;
            /// <summary>詳細検索OR条件追加</summary>
            public const short ComDetailSearchAddCond = 1256;
            /// <summary>表示項目カスタマイズ</summary>
            public const short ComItemCustomize = 1257;
            /// <summary>表示項目カスタマイズ保存</summary>
            public const short ComItemCustomizeSave = 1258;
            /// <summary>表示項目カスタマイズ適用</summary>
            public const short ComItemCustomizeApply = 1258;
            /// <summary>場所階層、職種機種検索</summary>
            public const short ComTreeViewSearch = 1259;
            /// <summary>階層選択モーダル画面表示</summary>
            public const short ComTreeViewShow = 1260;
            /// <summary>階層選択モーダル画面決定</summary>
            public const short ComTreeViewSelect = 1261;
            /// <summary>言語切り替え</summary>
            public const short ChangeLanguage = 1262;

            /// <summary>データ取得(ページング)</summary>
            public const short DataGet = 1301;
            /// <summary>一覧ソート</summary>
            public const short ListSort = 1302;

            /* 2XXX：別のアクションに連動して発火するもの */
            /// <summary>(未指定)：※初期化用</summary>
            public const short None = 2001;
            /// <summary>【共通 - 画面初期化機能】初期値データ取得</summary>
            public const short ComInitForm = 2002;
            /// <summary>【共通 - 双方向通信】ユーザーID取得</summary>
            public const short ComGetUserId = 2003;
            /// <summary>【共通 - 階層ツリー】構成マスタ情報取得</summary>
            public const short ComGetStructureList = 2004;
        }

        /// <summary>
        /// ボタン権限管理区分
        /// </summary>
        public static class BTN_AUTHCONTROLKBN
        {
            /// <summary>権限管理なし</summary>
            public const short Free = 0;
            /// <summary>権限管理あり</summary>
            public const short Control = 1;
            /// <summary>権限管理あり(権限設定マスターメンテナンス対象外)</summary>
            public const short ControlNoMasterMainte = 2;
        }

        /// <summary>
        /// ボタン実行後区分
        /// </summary>
        public static class BTN_AFTEREXECKBN
        {
            /// <summary>設定なし</summary>
            public const short None = 0;
            /// <summary>親画面に戻る</summary>
            public const short AutoBack = 1;
            /// <summary>指定画面に遷移する</summary>
            public const short FormTransition = 2;
        }

        /// <summary>
        /// セレクトボックス「全て」追加区分
        /// </summary>
        public static class OPTIONINFO_SELECT
        {
            /// <summary>「全て」を追加</summary>
            public const string AddAll = "1";
        }

        /// <summary>
        /// 実行ボタンの承認処理区分
        /// </summary>
        public static class OPTIONINFO_APPROVAL
        {
            /// <summary>承認依頼</summary>
            public const string Request = "11";
            /// <summary>承認依頼取消</summary>
            public const string RequestCancel = "12";
            /// <summary>承認</summary>
            public const string Approval = "21";
            /// <summary>承認取消</summary>
            public const string ApprovalCancel = "22";
            /// <summary>否認</summary>
            public const string Denial = "8";
        }

        /// <summary>
        /// 子画面表示方法
        /// </summary>
        public static class OPTIONINFO_CHILDDISP
        {
            /// <summary>表示切替</summary>
            public const string Shift = "1";
            /// <summary>ポップアップ</summary>
            public const string Popup = "2";
        }

        /// <summary>
        /// 変更不可項目区分
        /// </summary>
        public static class UNCHANGEABLEKBN
        {
            /// <summary>設定なし</summary>
            public const short Changeable = 0;
            /// <summary>変更不可項目</summary>
            public const short Unchangeable = 1;
        }

        /// <summary>
        /// 列固定項目区分
        /// </summary>
        public static class COLFIXKBN
        {
            /// <summary>設定なし</summary>
            public const short None = 0;
            /// <summary>列固定項目</summary>
            public const short FixCol = 1;
        }

        /// <summary>
        /// フィルター使用区分
        /// </summary>
        public static class FILTERUSEKBN
        {
            /// <summary>設定なし</summary>
            public const short None = 0;
            /// <summary>フィルター使用(部分一致)</summary>
            public const short PartialMatch = 1;
            /// <summary>フィルター使用(完全一致)</summary>
            public const short ExactMatch = 2;
        }

        /// <summary>
        /// ソート区分
        /// </summary>
        public static class SORT_DIVISION
        {
            /// <summary>ソート無効</summary>
            public const short Invalid = 0;
            /// <summary>ソート有効</summary>
            public const short Valid = 1;
            /// <summary>ソート有効(数値のソート)</summary>
            public const short ValidNumber = 2;
        }

        /// <summary>
        /// 条件パターン
        /// </summary>
        public static class LIKE_PATTERN
        {
            /// <summary>完全一致</summary>
            public const short PerfectMatch = 0;
            /// <summary>前方一致</summary>
            public const short Front = 1;
            /// <summary>後方一致</summary>
            public const short Backward = 2;
            /// <summary>部分一致</summary>
            public const short Partial = 3;
        }

        /// <summary>
        /// IN句判定区分
        /// </summary>
        public static class IN_CLAUSE_KBN
        {
            /// <summary>設定なし</summary>
            public const short None = 0;
            /// <summary>IN句の項目</summary>
            public const short InClause = 1;
        }

        /// <summary>
        /// 排他ロック種類
        /// </summary>
        public static class LOCK_TYPE
        {
            /// <summary>対象外</summary>
            public const short None = 0;
            /// <summary>ロック値</summary>
            public const short LockVal = 1;
            /// <summary>ロックキー</summary>
            public const short LockKey = 2;
        }

        /// <summary>
        /// 画面遷移アクション区分
        /// </summary>
        public static class DAT_TRANS_ACTION_DIV
        {
            /// <summary>未設定</summary>
            public const short None = 0;
            /// <summary>新規</summary>
            public const short New = 1;
            /// <summary>複写</summary>
            public const short Copy = 2;
            /// <summary>修正</summary>
            public const short Edit = 3;
            /// <summary>出力</summary>
            public const short Output = 4;

        }

        /// <summary>
        /// 詳細検索区分
        /// </summary>
        public static class DETAILED_SEARCH_DIV
        {
            /// <summary>対象外</summary>
            public const short None = 0;
            /// <summary>詳細検索有効</summary>
            public const short Enabled = 1;
            /// <summary>詳細検索有効＆NULL検索有り</summary>
            public const short EnabledNullSearch = 2;
        }

    }

    /// <summary>
    /// 共通＿中間データ
    /// </summary>
    public static class TMPTBL_CONSTANTS
    {
        /// <summary>
        /// ﾃﾞｰﾀﾀｲﾌﾟ
        /// </summary>
        public static class DATATYPE
        {
            /// <summary>結果値</summary>
            public const byte Result = 0;
            /// <summary>
            /// 画面入力値
            /// ※180406 UPD By ATTS ﾍﾞﾝﾍﾞﾙｸﾞ版では1→0に統合～中間ﾃｰﾌﾞﾙでは結果値(DB値)と入力値の切り分けを行わない
            /// </summary>
            //public const byte Input = 1;
            public const short Input = 0;

            /// <summary>更新済みﾊﾞｯｸｱｯﾌﾟﾃﾞｰﾀ(※更新列表示用)</summary>
            public const short UpdateBk = 1;

            /// <summary>新規行ﾃﾞｰﾀ</summary>
            public const short New = 2;

            /// <summary>検索条件</summary>
            public const short Condition = 3;

            /// <summary>ページ情報</summary>
            public const short PageInfo = 4;

            /// <summary>帳票ﾃﾞｰﾀ(※帳票用)</summary>
            public const short Report = 8;

            /// <summary>列ｽﾀｲﾙ</summary>
            public const short ColCss = 11;
            /// <summary>行ｽﾀｲﾙ</summary>
            public const short RowCss = 12;

            /// <summary>ﾒｯｾｰｼﾞ確認済みｽﾃｰﾀｽ</summary>
            public const short ConfirmStatus = 20;

            /// <summary>ｴﾗｰ詳細</summary>
            public const short ErrorDetail = 30;

            /// <summary>(未指定)</summary>
            public const short None = 99;
        }

        /// <summary>
        /// 行ｽﾃｰﾀｽ
        /// </summary>
        public static class ROWSTATUS
        {
            /// <summary>表示のみ行</summary>
            public const short ReadOnly = 0;
            /// <summary>編集行</summary>
            public const short Edit = 1;
            /// <summary>新規行</summary>
            public const short New = 2;
            /// <summary>削除行</summary>
            public const short None = 9;
        }

        /// <summary>
        /// 更新ﾀｸﾞ
        /// </summary>
        public static class UPDTAG
        {
            /// <summary>編集中（入力値）</summary>
            public const string Input = "1";
            ///// <summary>選択中</summary>
            //public const string Select = "2";
            ///// <summary>登録済み</summary>
            //public const string Update = "3";
            /// <summary>一時テーブル登録済み</summary>
            public const string UpdatedToTmp = "2";
            /// <summary>登録済み</summary>
            public const string Updated = "3";
        }
    }

    /// <summary>
    /// 共通＿ﾕｰｻﾞｰ処理権限（ﾎﾞﾀﾝ権限）
    /// </summary>
    public static class USER_AUTH_SHORI_CONSTANTS
    {
        /// <summary>
        /// 権限区分
        /// </summary>
        public static class AUTHKBN
        {
            /// <summary>更新</summary>
            public const short Update = 11;
            /// <summary>確定</summary>
            public const short Fixed = 12;
            /// <summary>承認</summary>
            public const short Approve = 13;
        }

        /// <summary>
        /// ボタン表示区分
        /// </summary>
        public static class BTN_DISPKBN_DEF
        {
            /// <summary>非表示</summary>
            public const short Hide = 0;
            /// <summary>活性</summary>
            public const short Active = 1;
            /// <summary>非活性</summary>
            public const short Disabled = 2;
        }

        /// <summary>
        /// ボタン表示区分
        /// </summary>
        /// <remarks>メソッドの戻り値で使用したいので、列挙型で定義</remarks>
        public enum BTN_DISPKBN_DEF_ENUM
        {
            /// <summary>なし</summary>
            None,
            /// <summary>非表示</summary>
            Hide,
            /// <summary>活性</summary>
            Active,
            /// <summary>非活性</summary>
            Disabled
        }

        /// <summary>
        /// 一覧編集権限区分
        /// </summary>
        public static class DAT_EDIT_DIV
        {
            /// <summary>編集不可(行追加/行削除不可)</summary>
            public const short UnEditable = 0;
            /// <summary>編集可(行追加/行削除可)</summary>
            public const short Editable = 1;
        }
    }

    /// <summary>
    /// 【共通 - 共通制御機能】
    /// </summary>
    public static class COM_CTRL_CONSTANTS
    {
        /// <summary>
        /// ｺﾝﾄﾛｰﾙID 固定値
        /// </summary>
        /// <remarks>【共通】…共通で自動制御</remarks>
        public static class CTRLID
        {
            /// <summary>【共通 - バッチ機能】処理ステータス(一覧)</summary>
            public const string ComBatStatus = "ComBatStatus";
            /// <summary>【共通 - バッチ機能】実行ボタン</summary>
            public const string ComBatExec = "ComBatExec";
            /// <summary>【共通 - バッチ機能】再表示ボタン</summary>
            public const string ComBatRefresh = "ComBatRefresh";

            /// <summary>【共通 - 取り込み機能】取り込みボタン</summary>
            public const string ComUpload = "ComUpload";
            /// <summary>【共通 - 取り込み機能】ファイル選択</summary>
            public const string ComUploadFile = "uploadFile";

        }

        /// <summary>
        /// 【共通 - バッチ機能】処理結果ﾃﾞｰﾀ列定義
        /// </summary>
        public static class COLNO
        {
            /// <summary>VAL1：開始日時</summary>
            public const int StDate = 1;
            /// <summary>VAL2：終了日時</summary>
            public const int EdDate = 2;
            /// <summary>VAL3：ユーザー</summary>
            public const int User = 3;
            /// <summary>VAL4：処理結果</summary>
            public const int Status = 4;
            /// <summary>VAL5：処理メッセージ</summary>
            public const int Message = 5;
            /// <summary>VAL6：関連ファイル</summary>
            public const int File = 6;
            /// <summary>VAL7：表示設定スタイル(※スタイル設定用)</summary>
            public const int CssClass = 7;
            /// <summary>VAL8：実行中フラグ</summary>
            public const int JikkoFlg = 8;
            /// <summary>VAL9：実行条件</summary>
            public const int Joken = 9;
        }

        /// <summary>
        /// 【共通 - バッチ機能】処理結果 列タイトルを取得する
        /// </summary>
        public static string getColNameStr(int colNo)
        {
            string[] colNames = new string[] {
                "開始日時",
                "終了日時",
                "ユーザー",
                "処理結果",
                "処理メッセージ",
                "関連ファイル",
                "メッセージスタイル名",
                "実行フラグ",
                "実行条件",
            };
            if (colNo > colNames.Length)
            {
                return "";
            }

            return colNames[colNo - 1];
        }

        /// <summary>
        /// 【共通 - バッチ機能】処理結果 列タイトルの翻訳IDを取得する
        /// </summary>
        public static string getColNameId(int colNo)
        {
            string[] colIds = new string[] {
                "911060001", // 開始日時
                "911120012", // 終了日時
                "911370002", // ユーザー
                "911120013", // 処理結果
                "911120014", // 処理メッセージ
                "111060003", // 関連ファイル
                "911340001", // メッセージスタイル名
                "911120015", // 実行フラグ
                "911120016"  // 実行条件
            };
            if (colNo > colIds.Length)
            {
                return "";
            }

            return colIds[colNo - 1];
        }
    }


    /// <summary>
    /// 構成マスタ
    /// </summary>
    public static class STRUCTURE_CONSTANTS
    {
        /// <summary>システム共通工場ID</summary>
        public const int CommonFactoryId = 0;

        /// <summary>
        /// 構成グループ
        /// </summary>
        public static class STRUCTURE_GROUP
        {
            /// <summary>場所階層</summary>
            public const short Location = 1000;

            /// <summary>職種機種</summary>
            public const short Job = 1010;

            /// <summary>故障原因</summary>
            public const short Cause = 1020;

            /// <summary>MQ分類</summary>
            public const short MqClass = 1030;

            /// <summary>予備品場所階層</summary>
            public const short SpareLocation = 1040;

            /// <summary>ユーザーマスタ用場所階層</summary>
            /// <remarks>この構成グループIDは存在しない、ツリー表示用</remarks>
            public const short LocationForUserMst = 1004;

            /// <summary>工場と場所</summary>
            /// <remarks>この構成グループIDは存在しない、ツリー表示用</remarks>
            public const short FactoryAndJob = 1005;

            /// <summary>変更履歴管理を行わない工場のみを表示</summary>
            /// <remarks>この構成グループIDは存在しない、ツリー表示用</remarks>
            public const short LocationNoHistory = 1001;

            /// <summary>変更履歴管理を行う工場のみを表示</summary>
            /// <remarks>この構成グループIDは存在しない、ツリー表示用</remarks>
            public const short LocationHistory = 1002;

            // 2024/07/08 機器別管理基準標準用場所階層ツリー追加 ADD start
            /// <summary>機器別管理基準標準用場所階層</summary>
            /// <remarks>この構成グループIDは存在しない、ツリー表示用</remarks>
            public const short LocationForMngStd = 1006;
            // 2024/07/08 機器別管理基準標準用場所階層ツリー追加 ADD end

            /// <summary>工場取得のための場所階層として扱う構成グループIDのリスト</summary>
            // 2024/07/08 機器別管理基準標準用場所階層ツリー追加 UPD start
            //public static int[] Locations = { Location, LocationForUserMst, LocationNoHistory, LocationHistory };
            public static int[] Locations = { Location, LocationForUserMst, LocationNoHistory, LocationHistory, LocationForMngStd };
            // 2024/07/08 機器別管理基準標準用場所階層ツリー追加 UPD end
        }

        /// <summary>
        /// 検索条件キー
        /// </summary>
        public static class CONDITION_KEY
        {
            /// <summary>場所階層</summary>
            public const string Location = "locationIdList";

            /// <summary>職種機種</summary>
            public const string Job = "jobIdList";
        }

        /// <summary>
        /// 階層番号
        /// </summary>
        public static class LAYER_NO
        {
            /// <summary>工場</summary>
            public const short Factory = 1;

            /// <summary>職種</summary>
            public const short Job = 0;
        }

        /// <summary>
        /// 構成マスタ用SQLID
        /// </summary>
        public static string[] SQLIDs = { "C0001", "C0002", "C0007", "C0012", "C0016", "C0018", "C0024", "C0026", "C0027" };

    }

    /// <summary>
    /// ユーザー関連定義
    /// </summary>
    public static class USER_CONSTANTS
    {
        /// <summary>
        /// 権限レベル
        /// </summary>
        public static class AUTHORITY_LEVEL
        {
            /// <summary>ゲストユーザー</summary>
            public const short Guest = 10;
            /// <summary>一般ユーザー</summary>
            public const short General = 20;
            /// <summary>特権ユーザー</summary>
            public const short Privileged = 30;
            /// <summary>システム管理者</summary>
            public const short Administrator = 99;
        }
    }
}
