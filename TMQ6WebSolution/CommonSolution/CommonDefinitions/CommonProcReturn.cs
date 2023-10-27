///<summary>
/// 機能名　　：　【共通】
/// タイトル　：　業務ロジックプロシージャ戻り値設定用データクラス
/// 説明　　　：　業務ロジックプロシージャからの戻り値を格納するデータ型を定義します。
/// 
/// 履歴　　　：　2017.08.01 河村純子　新規作成
///</summary>


namespace CommonWebTemplate.Models.Common
{
    public class CommonProcReturn
    {
        #region === 定数 ===
        /// <summary>
        /// 業務ﾛｼﾞｯｸﾌﾟﾛｼｰｼﾞｬ処理ｽﾃｰﾀｽ
        /// </summary>
        public static class ProcStatus
        {
            public const int Valid = 0;           //正常終了～処理続行、お知らせ表示
            public const int Warning = 1;         //警告～処理続行、エラー表示
            public const int WarnDisp = 2;        //警告～ﾌﾟﾛｼｰｼﾞｬ処理中断、WEB画面処理続行、エラー状態表示
            public const int Error = 10;          //エラー～処理中断、エラー表示
            public const int Confirm = 20;        //確認～処理中断、確認メッセージ表示
            public const int InValid = 100;       //異常終了～処理中断

            //WEB画面からのｴﾗｰ
            public const int TimeOut = 990;         //ｾｯｼｮﾝﾀｲﾑｱｳﾄ
            public const int LoginError = 991;      //ﾛｸﾞｲﾝ不正
            public const int AccessError = 992;     //ｱｸｾｽ不正
        }
        #endregion

        /// <summary>
        /// ステータス
        /// </summary>
        public int STATUS { get; set; }

        /// <summary>
        /// メッセージＩＤ
        /// </summary>
        public string MESSAGE { get; set; }

        /// <summary>
        /// 問合せ番号
        /// </summary>
        public string LOGNO { get; set; }

        /// <summary>
        /// 作成ﾌｧｲﾙ名
        /// </summary>
        /// <remarks>AppConstants.FileDownloadSetting = Hidoukiの場合に使用</remarks>
        public string FILENAME { get; set; }

        /// <summary>
        /// 作成ﾌｧｲﾙパス
        /// </summary>
        /// <remarks>AppConstants.FileDownloadSetting = Hidoukiの場合に使用</remarks>
        public string FILEPATH { get; set; }

        /// <summary>
        /// 作成ﾌｧｲﾙﾃﾞｰﾀ
        /// </summary>
        /// <remarks>AppConstants.FileDownloadSetting = Doukiの場合に使用</remarks>
        public byte[] FILEDATA { get; set; }

        /// <summary>
        /// ﾌｧｲﾙﾀﾞｳﾝﾛｰﾄﾞ名
        /// </summary>
        public string FILEDOWNLOADNAME { get; set; }

        /// <summary>
        /// ユーザー情報の更新
        /// </summary>
        public bool UpdateUserInfo { get; set; }

        #region === ｺﾝｽﾄﾗｸﾀ ===
        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// </summary>
        public CommonProcReturn()
        {
            STATUS = ProcStatus.Valid;   //0:正常終了
            UpdateUserInfo = false;
        }
        
        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// 　-　正常ﾒｯｾｰｼﾞIDを設定
        /// </summary>
        public CommonProcReturn(string msgId)
        {
            STATUS = ProcStatus.Valid;   //0:正常終了
            MESSAGE = msgId;
            UpdateUserInfo = false;
        }

        /// <summary>
        /// ｺﾝｽﾄﾗｸﾀ
        /// 　-　異常ﾒｯｾｰｼﾞIDを設定
        /// </summary>
        public CommonProcReturn(int status, string msgId)
        {
            STATUS = status;
            MESSAGE = msgId;
            UpdateUserInfo = false;
        }
        #endregion

        #region === public 処理 ===
        /// <summary>
        /// 処理中断判定
        /// </summary>
        /// <returns>true:ｴﾗｰあり、処理を中断、false:処理を続行</returns>
        public bool IsProcEnd()
        {
            if (STATUS == CommonProcReturn.ProcStatus.WarnDisp ||
                STATUS == CommonProcReturn.ProcStatus.Warning ||
                STATUS == CommonProcReturn.ProcStatus.Valid)
            {
                return false;   //処理を続行
            }
            return true;
        }
        #endregion
    }
}