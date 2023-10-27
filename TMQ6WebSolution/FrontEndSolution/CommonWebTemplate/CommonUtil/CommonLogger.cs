using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonWebTemplate.CommonUtil
{
    public class CommonLogger
    {
        #region === privete変数 ===
        private long generation;
        private readonly string logFileName;
        private readonly string appPath;
        private readonly string logPath;
        private readonly string extention;
        private readonly string writetarget;
        private readonly long maxFileSize;

        private static CommonLogger singleton = null;
        #endregion

        #region === ｺﾝｽﾄﾗｸﾀ ===
        /// <summary>
        /// インスタンス生成
        /// </summary>
        /// <param name="loggerName">ロガー名</param>
        /// <param name="rootPath">ログ出力先ディレクトリルートパス</param>
        /// <returns></returns>
        public static CommonLogger GetInstance()
        {
            if (singleton == null)
            {
                singleton = new CommonLogger();
            }
            return singleton;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private CommonLogger()
        {
            //世代管理数
            generation = 10;
            //ログファイル名
            logFileName = "FrontEndErrorLog";
            //アプリケーションパス
            appPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            //ログファイル出力先パス
            logPath = System.IO.Path.Combine(appPath, "logs");
            //ログファイル拡張子
            extention = ".log";
            //
            writetarget = System.IO.Path.Combine(logPath, logFileName + extention);
            //ファイルサイズによる世代管理の上限サイズ
            maxFileSize = 1048576;
        }
        #endregion

        #region === private処理 ===

        /// <summary>
        /// ファイルサイズの確認
        /// <param name="target">対象ファイル情報</param>
        /// <param name="maxFileSize">ファイルの最大容量</param>
        /// <returns>true:ファイルサイズが最大容量をオーバー false:ファイルサイズは最大容量以下</returns>
        /// </summary>
        private static bool chkFileSize(string target, long maxFileSize)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(target);

            // ファイルサイズが最大容量をオーバーする場合、trueを返却
            if (fi.Length > maxFileSize)
            {
                return true;
            }

            return false;
        }
        #endregion

        #region === public処理 ===
        /// <summary>
        /// ログファイルの書き込み
        /// </summary>
        public void WriteLog(string Message)
        {
            // フォルダが存在しない場合、自動生成
            if (!System.IO.Directory.Exists(logPath))
            {
                System.IO.Directory.CreateDirectory(logPath);
            }

            // ファイルが存在しない場合、自動生成
            if (!System.IO.File.Exists(writetarget))
            {
                System.IO.File.Create(writetarget).Close();
            }

            // ログファイルが一定サイズ以上の場合、世代管理を行う
            if (chkFileSize(writetarget, maxFileSize))
            {
                // 世代管理
                for (long i = generation; 0 < i; i--)
                {
                    // 指定された最大世代ループ時
                    if (i == generation)
                    {
                        string target = System.IO.Path.Combine(logPath, logFileName + i.ToString().PadLeft(2, '0') + extention);

                        // 存在する場合は削除
                        if (System.IO.File.Exists(target))
                        {
                            System.IO.File.Delete(target);
                        }
                    }

                    // 最大未満～当世代未満の場合
                    if (1 < i && i < generation)
                    {
                        string target = System.IO.Path.Combine(logPath, logFileName + i.ToString().PadLeft(2, '0') + extention);

                        // 存在する場合はコピーして削除
                        if (System.IO.File.Exists(target))
                        {

                            // 2世代目の場合
                            if (i == 2)
                            {
                                // 1世代上のファイルがあればコピーして削除
                                if (System.IO.File.Exists(System.IO.Path.Combine(logPath, logFileName + extention)))
                                {
                                    System.IO.File.Copy(target, System.IO.Path.Combine(logPath, logFileName + (i + 1).ToString().PadLeft(2, '0') + extention));
                                    System.IO.File.Delete(target);
                                }
                            }
                            else
                            {
                                // 1世代上のファイルがあればコピーして削除
                                if (System.IO.File.Exists(System.IO.Path.Combine(logPath, logFileName + (i - 1).ToString().PadLeft(2, '0') + extention)))
                                {
                                    System.IO.File.Copy(target, System.IO.Path.Combine(logPath, logFileName + (i + 1).ToString().PadLeft(2, '0') + extention));
                                    System.IO.File.Delete(target);
                                }
                            }
                        }
                    }

                    // 当世代の場合
                    if (i == 1)
                    {
                        string target = System.IO.Path.Combine(logPath, logFileName + extention);

                        // 存在する場合はコピーして削除して当世代新規作成
                        if (System.IO.File.Exists(target))
                        {
                            System.IO.File.Copy(target, System.IO.Path.Combine(logPath, logFileName + (i + 1).ToString().PadLeft(2, '0') + extention));
                            System.IO.File.Delete(target);
                            System.IO.File.Create(target).Close();
                        }
                    }
                }
            }

            // 書き込み対象のログにメッセージを追加する
            System.IO.File.AppendAllText(writetarget, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + "：" + Message + Environment.NewLine);
        }
        #endregion
    }
}