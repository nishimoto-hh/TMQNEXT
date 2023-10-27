using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComRes = CommonSTDUtil.CommonResources;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
//using ComST = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using TMQDTCls = TMQSendMailHistory.TMQSendMailHistoryDataClass;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using CommonSTDUtil.CommonLogger;
using CommonWebTemplate.Models.Common;


namespace TMQSendMailHistory
{
    /// <summary>
    /// メール送信クラス
    /// </summary>
    class SendMailUtil
    {
        /// <summary>メールモード 1:自動FAX</summary>
        public const decimal SEND_MODE_AUTO_FAX = 1;

        /// <summary>メールモード 0:メール送信のみ</summary>
        public const decimal SEND_MODE_MAIL_ONLY = 0;

        /// <summary>接続時タイムアウト 初期値:10000(ミリ秒)</summary>
        public const string DEFAULT_TIMEOUT = "20000";

        /// <summary>申請者宛メール送信（承認依頼件数） メール本文置換文字列</summary>
        public const string MAIL_BODY_APPROVAL_REPLACE_STRING = "[approval_counts]";

        /// <summary>>申請者宛メール送信（否認件数）メール本文置換文字列</summary>
        public const string MAIL_BODY_REJECTION_REPLACE_STRING = "[rejection_counts]";

        /// <summary>>承認者宛メール送信（承認依頼件数）メール本文置換文字列</summary>
        public const string MAIL_BODY_APPROVAL_REQUEST_REPLACE_STRING = "[approval_request_counts]";

        /// <summary>>ログ出力：メールアドレス未登録</summary>
        public const string LOG_MAILADDRESS_NOTHING_STRING = "メールアドレスが設定されていません。（ユーザID：{0}）";

        /// <summary>ログ出力</summary>
        private static CommonLogger logger = CommonLogger.GetInstance("logger");

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SendMailUtil()
        {
            List<string> toUserList = new();
            decimal mailFormatId = 0;
            List<string> subjectParamList = new();
            List<string> bodyParamList = new();
            string tantoCd = "";
            //ComDB db = new ComDB();
            ComDB db = new ComDB(logger, "", "");

            //Batch_Common batchCom = new(db);

            //SendMail(toUserList, mailFormatId, subjectParamList, bodyParamList, tantoCd, db);
            string errorMsg = "";

            string languageId = CommonWebTemplate.AppCommonObject.Config.AppSettings.LanguageIdDefault;

            // メール送信
            callSendMail(languageId, ref errorMsg, db);
        }
        #endregion

        /// <summary>
        /// メール送信実行メソッド
        /// </summary>
        /// <param name="fromAddress">送信元アドレス</param>
        /// <param name="fromAddressName">送信元アドレス名称</param>
        /// <param name="toAddressCsv">宛先アドレス</param>
        /// <param name="carbonCopyAddresses">CCアドレス</param>
        /// <param name="blindCarbonCopyAddresses">BCCアドレス</param>
        /// <param name="subject">メール件名</param>
        /// <param name="mainText">メール本文</param>
        /// <param name="filePathList">添付ファイルパス（絶対パス）</param>
        /// <param name="host">メール送信サーバ</param>
        /// <param name="port">PORT番号</param>
        /// <param name="user">ユーザ(ユーザー認証を必要とする場合)</param>
        /// <param name="password">パスワード(ユーザー認証を必要とする場合)</param>
        /// <returns><see cref="Task"/>representing the asynchronous operation</returns>
        [Obsolete("非同期の動作確認中のため、こちらのメソッドは使用しないでください。")]
        public static async Task<bool> SendMailAsync(
            string fromAddress,
            string fromAddressName,
            string toAddressCsv,
            List<string> carbonCopyAddresses,
            List<string> blindCarbonCopyAddresses,
            string subject,
            string mainText,
            List<string> filePathList,
            string host,
            int port,
            string user = "",
            string password = "")
        {
            // 変数宣言
            string[] split_comma = new string[] { "," }; // カンマ(TO,CC,BCC)
            string[] arrayTo = { "" }; // TO
            string to = string.Empty; // 送信先アドレス
            string cc = string.Empty; // CC送信先アドレス
            string bcc = string.Empty; // BCC送信先アドレス
            string title = string.Empty; // 件名

            // 必須入力チェック（下記項目が未入力の場合）
            // パラメータ.宛先アドレス
            // パラメータ.メール件名
            // パラメータ.本文
            if (string.IsNullOrEmpty(toAddressCsv) || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(mainText))
            {
                // エラーログを出力する。
                // 「引数が設定されていません。」
                logger.Error(ComUtil.GetPropertiesMessage(ComRes.ID.ID941270002));
                return false;
            }

            // MimeMessageを作り、宛先やタイトルなどを設定する
            var message = new MimeKit.MimeMessage();

            // 送信アドレス
            if (string.IsNullOrEmpty(fromAddressName))
            {
                fromAddressName = fromAddress;
            }
            message.From.Add(new MimeKit.MailboxAddress(fromAddressName, fromAddress));

            // 宛先アドレス
            arrayTo = toAddressCsv.Split(split_comma, StringSplitOptions.RemoveEmptyEntries);
            // 宛先
            for (int i = 0; i < arrayTo.Length; i++)
            {
                to = arrayTo[i];
                message.To.Add(new MimeKit.MailboxAddress(to, to));
            }

            // CCアドレス
            if (carbonCopyAddresses != null && carbonCopyAddresses.Count > 0)
            {
                for (int i = 0; i < carbonCopyAddresses.Count; i++)
                {
                    cc = carbonCopyAddresses[i];
                    message.Cc.Add(new MimeKit.MailboxAddress(cc, cc));
                }
            }

            // BCCアドレス
            if (blindCarbonCopyAddresses != null && blindCarbonCopyAddresses.Count > 0)
            {
                for (int i = 0; i < blindCarbonCopyAddresses.Count; i++)
                {
                    bcc = blindCarbonCopyAddresses[i];
                    message.Bcc.Add(new MimeKit.MailboxAddress(bcc, bcc));
                }
            }

            // 件名
            message.Subject = subject;

            // 本文
            var textPart = new MimeKit.TextPart();
            // 「@」を利用するとエスケープを行わないでよい。「@quoted(クォート) string」
            textPart.Text = mainText;
            // MimeMessageを完成させる
            var multipart = new MimeKit.Multipart();
            multipart.Add(textPart);

            if (filePathList != null && filePathList.Count > 0)
            {
                for (int i = 0; i < filePathList.Count; i++)
                {
                    // ファイルの拡張子からMIMEタイプを取得する
                    var mimeType = MimeKit.MimeTypes.GetMimeType(filePathList[i]);
                    // MimePartのインスタンスを作成
                    var attachment = new MimeKit.MimePart(mimeType);
                    // 添付ファイルの情報をMIMEコンテンツに設定
                    attachment.Content = new MimeKit.MimeContent(System.IO.File.OpenRead(filePathList[i]));
                    // 添付ファイルの表示形式の設定
                    attachment.ContentDisposition = new MimeKit.ContentDisposition();
                    // メール本文のエンコードの設定
                    attachment.ContentTransferEncoding = MimeKit.ContentEncoding.Base64;
                    // 添付ファイル名の設定
                    attachment.FileName = System.IO.Path.GetFileName(filePathList[i]);
                    // 添付ファイルの情報を設定
                    multipart.Add(attachment);
                }
            }

            message.Body = multipart;

            // SMTPサーバに接続してメールを送信する
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    // 接続
                    await client.ConnectAsync(host, port);

                    // SMTPサーバがユーザー認証を必要とする場合
                    if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(password))
                    {
                        // 認証
                        await client.AuthenticateAsync(user, password);
                    }

                    // 送信
                    await client.SendAsync(message);
                    // 切断
                    await client.DisconnectAsync(true);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// メール送信実行メソッド
        /// </summary>
        /// <param name="fromAddress">送信元アドレス</param>
        /// <param name="fromAddressName">送信元アドレス名称</param>
        /// <param name="toAddressCsv">宛先アドレス</param>
        /// <param name="carbonCopyAddresses">CCアドレス</param>
        /// <param name="blindCarbonCopyAddresses">BCCアドレス</param>
        /// <param name="subject">メール件名</param>
        /// <param name="mainText">メール本文</param>
        /// <param name="filePathList">添付ファイルパス（絶対パス）</param>
        /// <param name="host">メール送信サーバ</param>
        /// <param name="port">PORT番号</param>
        /// <param name="user">ユーザ(ユーザー認証を必要とする場合)</param>
        /// <param name="password">パスワード(ユーザー認証を必要とする場合)</param>
        /// <returns>処理結果 true:成功、false:失敗</returns>
        public static bool SendMailImpl(
            string fromAddress,
            string fromAddressName,
            string toAddressCsv,
            List<string> carbonCopyAddresses,
            List<string> blindCarbonCopyAddresses,
            string subject,
            string mainText,
            List<string> filePathList,
            string host,
            int port,
            string user = "",
            string password = "")
        {
            // 変数宣言
            string[] split_comma = new string[] { "," }; // カンマ(TO,CC,BCC)
            string[] arrayTo = { "" }; // TO
            string to = string.Empty; // 送信先アドレス
            string cc = string.Empty; // CC送信先アドレス
            string bcc = string.Empty; // BCC送信先アドレス
            string title = string.Empty; // 件名

            // 必須入力チェック（下記項目が未入力の場合）
            // パラメータ.宛先アドレス
            // パラメータ.メール件名
            // パラメータ.本文
            if (string.IsNullOrEmpty(toAddressCsv) || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(mainText))
            {
                // エラーログを出力する。
                // 「引数が設定されていません。」
                logger.Error(ComUtil.GetPropertiesMessage(ComRes.ID.ID941270002));
                return false;
            }

            // MimeMessageを作り、宛先やタイトルなどを設定する
            var message = new MimeKit.MimeMessage();

            // 送信アドレス
            if (string.IsNullOrEmpty(fromAddressName))
            {
                fromAddressName = fromAddress;
            }
            message.From.Add(new MimeKit.MailboxAddress(fromAddressName, fromAddress));

            // 宛先アドレス
            arrayTo = toAddressCsv.Split(split_comma, StringSplitOptions.RemoveEmptyEntries);
            // 宛先
            for (int i = 0; i < arrayTo.Length; i++)
            {
                to = arrayTo[i];
                message.To.Add(new MimeKit.MailboxAddress(to, to));
            }

            // CCアドレス
            if (carbonCopyAddresses != null && carbonCopyAddresses.Count > 0)
            {
                for (int i = 0; i < carbonCopyAddresses.Count; i++)
                {
                    cc = carbonCopyAddresses[i];
                    message.Cc.Add(new MimeKit.MailboxAddress(cc, cc));
                }
            }

            // BCCアドレス
            if (blindCarbonCopyAddresses != null && blindCarbonCopyAddresses.Count > 0)
            {
                for (int i = 0; i < blindCarbonCopyAddresses.Count; i++)
                {
                    bcc = blindCarbonCopyAddresses[i];
                    message.Bcc.Add(new MimeKit.MailboxAddress(bcc, bcc));
                }
            }

            // 件名
            message.Subject = subject;

            // 本文
            var textPart = new MimeKit.TextPart();
            // 「@」を利用するとエスケープを行わないでよい。「@quoted(クォート) string」
            textPart.Text = mainText;
            // MimeMessageを完成させる
            var multipart = new MimeKit.Multipart();
            multipart.Add(textPart);

            if (filePathList != null && filePathList.Count > 0)
            {
                for (int i = 0; i < filePathList.Count; i++)
                {
                    // ファイルの拡張子からMIMEタイプを取得する
                    var mimeType = MimeKit.MimeTypes.GetMimeType(filePathList[i]);
                    // MimePartのインスタンスを作成
                    var attachment = new MimeKit.MimePart(mimeType);
                    // 添付ファイルの情報をMIMEコンテンツに設定
                    attachment.Content = new MimeKit.MimeContent(System.IO.File.OpenRead(filePathList[i]));
                    // 添付ファイルの表示形式の設定
                    attachment.ContentDisposition = new MimeKit.ContentDisposition();
                    // メール本文のエンコードの設定
                    attachment.ContentTransferEncoding = MimeKit.ContentEncoding.Base64;
                    // 添付ファイル名の設定
                    attachment.FileName = System.IO.Path.GetFileName(filePathList[i]);
                    // 添付ファイルの情報を設定
                    multipart.Add(attachment);
                }
            }

            message.Body = multipart;

            // SMTPサーバに接続してメールを送信する
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    // 接続
                    client.Connect(host, port);

                    // SMTPサーバがユーザー認証を必要とする場合
                    if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(password))
                    {
                        // 認証
                        client.Authenticate(user, password);
                    }

                    // 送信
                    client.Send(message);
                    // 切断
                    client.Disconnect(true);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// メール送信メソッドを呼び出すために必要な情報を取得して送信する処理
        /// </summary>
        /// <param name="languageId">言語ID</param>
        /// <param name="errorMsg">エラーメッセージ</param>
        /// <returns>成功した場合、True</returns>
        //private bool callSendMail(EntityDao.UtilityEntity mailFrom, List<WorkflowGetNoticeUserListResult> noticeUserList, EntityDao.MailTemplateEntity mailText,
        private bool callSendMail(string languageId, ref string errorMsg, ComDB db)
        {
            string logMsg = string.Empty;

            try
            {
                string msg01 = ComUtil.GetPropertiesMessage(ComRes.ID.ID941260020, languageId, null, db, null);  // ID941260020　パラメータ設定エラー
                string msgStart = ComUtil.GetPropertiesMessage(new string[] { ComRes.ID.ID941060012, "141290006" }, languageId, null, db, null); // 長期計画メール送信開始
                string msgEnd = ComUtil.GetPropertiesMessage(new string[] { ComRes.ID.ID941120011, "141290006" }, languageId, null, db, null); // 長期計画メール送信終了


                // 変更管理メール送信マスタメンテ情報取得
                MailSendServerInfo server = GetMailSendServerInfo(languageId, db);

                if (server.IsError)
                {
                    logger.Error(msg01); // パラメータ設定エラー
                    return false;
                }

                //// 送信メールテンプレート情報取得
                //int trnsKey1 = 0;
                //int trnsKey2 = 0;
                //int.TryParse(server.MailSendTitle, out trnsKey1);
                //int.TryParse(server.MailSendBody, out trnsKey2);

                // 申請者宛送信対象取得条件(分)取得
                IList<TMQSendMailHistoryDataClass.SendMailHistorySerachTime> templateInfo = GetSendMailHistoryTime(db);
                IList<TMQDTCls.SendMailHistorySerachTime> SerachTime = templateInfo.Where(x => x.SerachTime > 0).ToList();
                if (templateInfo == null || templateInfo.Count == 0)
                {
                    // 申請者宛送信対象取得条件(分)未登録
                    return true;
                }

                // メール送信開始
                // ログ出力
                logger.WriteLog(CommonLoggerBase.LogLevel.Info, CommonLoggerBase.LogType.None, msgStart); // 変更管理メール送信開始

                // 送信元メールアドレス
                string fromAddress = server.SendFromAddress;
                string fromAddressName = "";
                // 件名
                string mailTextTitle = "";
                string sendSubjectByPerson = string.Empty;

                // *****************************************************
                // 申請者宛
                // *****************************************************

                // 申請者宛メール情報取得
                string sTime = SerachTime[0].SerachTime.ToString();
                IList<TMQSendMailHistoryDataClass.SendMailHistoryApplication> ApplicationInfo = GetSendMailHistoryApplicationInfo(sTime, db);
                if (ApplicationInfo == null || ApplicationInfo.Count == 0)
                {
                    // 申請者宛メール該当なし
                    // 処理継続
                }
                else
                {
                    //申請したユーザーID毎にメールを送信する
                    foreach (TMQSendMailHistoryDataClass.SendMailHistoryApplication ApplicationData in ApplicationInfo)
                    {
                        //メールアドレス確認
                        if (string.IsNullOrEmpty(ApplicationData.MailAddress))
                        {
                            //メールアドレス未登録
                            //ログメッセージのユーザーID置換
                            string logText = SendMailUtil.LOG_MAILADDRESS_NOTHING_STRING.Replace("{0}", ApplicationData.application_user_id.ToString());
                            // ログ出力
                            logger.WriteLog(CommonLoggerBase.LogLevel.Info, CommonLoggerBase.LogType.None, logText);
                            continue;
                        }

                        // メールアドレスのある場合、メール送信
                        string userlanguageId = ApplicationData.LanguageId;

                        // ユーザーの言語に応じたメッセージ取得
                        string msg02 = ComUtil.GetPropertiesMessage(ComRes.ID.ID141290002, userlanguageId, null, db, null);  // 行った承認依頼が決裁されました
                        string msg03 = ComUtil.GetPropertiesMessage(ComRes.ID.ID141290003, userlanguageId, null, db, null);  // 承認依頼が[approval_counts]件承認され、[rejection_counts]件否認されました。

                        // 件名
                        mailTextTitle = msg02;

                        if ((ApplicationData.ApprovedCnt + ApplicationData.ReturnCnt) <= 0)
                        {
                            //　対象データなし
                            continue;
                        }

                        // メール本文作成
                        // 承認依頼件数
                        string mailTextBody = msg03.Replace(MAIL_BODY_APPROVAL_REPLACE_STRING, ApplicationData.ApprovedCnt.ToString());
                        // 否認件数
                        mailTextBody = mailTextBody.Replace(MAIL_BODY_REJECTION_REPLACE_STRING, ApplicationData.ReturnCnt.ToString());

                        // メール送信サーバ情報セット
                        string mailSendServer = server.Url; // サーバURL
                        int mailSendPort = server.Port;  // ポート
                        string mailAuthUser = server.AuthUser;   // 承認ユーザ
                        string mailAuthPasswd = server.AuthPassword;   // 承認パスワード
                                                                       // 送信
                        if (!SendMailImpl(fromAddress, fromAddressName, ApplicationData.MailAddress, null, null, mailTextTitle, mailTextBody, null, mailSendServer, mailSendPort, mailAuthUser, mailAuthPasswd))
                        {
                            // ログ出力
                            logger.WriteLog(CommonLoggerBase.LogLevel.Error, CommonLoggerBase.LogType.None, "担当者メールアドレス [" + ApplicationData.MailAddress + "] 件名 [" + mailTextTitle + "]" + "送信失敗");
                        }
                        else
                        {
                            // ログ出力
                            logger.WriteLog(CommonLoggerBase.LogLevel.Info, CommonLoggerBase.LogType.None, "担当者メールアドレス [" + ApplicationData.MailAddress + "] 件名 [" + mailTextTitle + "]" + "送信完了");
                        }
                    }
                }

                // *****************************************************
                // 承認者宛
                // *****************************************************

                // 承認者宛メール情報取得
                IList<TMQSendMailHistoryDataClass.SendMailHistoryApproval> ApprovalInfo = GetSendMailHistoryApprovalInfo(db);
                if (ApprovalInfo == null || ApprovalInfo.Count == 0)
                {
                    // 承認メール該当なし
                    //return true;
                }
                else
                {
                    // 承認者宛にメールを送信する
                    foreach (TMQSendMailHistoryDataClass.SendMailHistoryApproval ApprovalData in ApprovalInfo)
                    {
                        //メールアドレス確認
                        if (string.IsNullOrEmpty(ApprovalData.MailAddress))
                        {
                            //メールアドレス未登録
                            //ログメッセージのユーザーID置換
                            string logText = SendMailUtil.LOG_MAILADDRESS_NOTHING_STRING.Replace("{0}", ApprovalData.user_id.ToString());
                            // ログ出力
                            logger.WriteLog(CommonLoggerBase.LogLevel.Info, CommonLoggerBase.LogType.None, logText);

                            continue;
                        }

                        // メールアドレスのある場合、メール送信

                        //long ApprovedCnt = 0;   // 承認済件数
                        //long ReturnCnt = 0;     // 差戻中件数
                        string userlanguageId = ApprovalData.LanguageId;

                        // ユーザーの言語に応じたメッセージ取得
                        string msg02 = ComUtil.GetPropertiesMessage(ComRes.ID.ID141290004, userlanguageId, null, db, null);  // 決裁が必要な承認依頼が届いています
                        string msg03 = ComUtil.GetPropertiesMessage(ComRes.ID.ID141290005, userlanguageId, null, db, null);  // 承認依頼が[approval_request_counts]件届いています。変更管理画面より承認か否認を行ってください。

                        // 件名
                        mailTextTitle = msg02;

                        if (ApprovalData.RequestCnt <= 0)
                        {
                            //　対象データなし
                            continue;
                        }

                        // メール本文作成
                        // 承認依頼件数
                        string mailTextBody = msg03.Replace(MAIL_BODY_APPROVAL_REQUEST_REPLACE_STRING, ApprovalData.RequestCnt.ToString());

                        // メール送信サーバ情報セット
                        string mailSendServer = server.Url; // サーバURL
                        int mailSendPort = server.Port;  // ポート
                        string mailAuthUser = server.AuthUser;   // 承認ユーザ
                        string mailAuthPasswd = server.AuthPassword;   // 承認パスワード
                                                                       // 送信
                        if (!SendMailImpl(fromAddress, fromAddressName, ApprovalData.MailAddress, null, null, mailTextTitle, mailTextBody, null, mailSendServer, mailSendPort, mailAuthUser, mailAuthPasswd))
                        {
                            // ログ出力
                            logger.WriteLog(CommonLoggerBase.LogLevel.Error, CommonLoggerBase.LogType.None, "担当者メールアドレス [" + ApprovalData.MailAddress + "] 件名 [" + mailTextTitle + "]" + "送信失敗");
                        }
                        else
                        {
                            // ログ出力
                            logger.WriteLog(CommonLoggerBase.LogLevel.Info, CommonLoggerBase.LogType.None, "担当者メールアドレス [" + ApprovalData.MailAddress + "] 件名 [" + mailTextTitle + "]" + "送信完了");
                        }
                    }
                }

                // 変更管理メール送信終了
                // ログ出力
                logger.WriteLog(CommonLoggerBase.LogLevel.Info, CommonLoggerBase.LogType.None, msgEnd); // 変更管理メール送信終了

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// メール送信情報取得
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <returns>メール送信情報</returns>
        public static MailSendServerInfo GetMailSendServerInfo(string languageId, ComDB db)
        {
            MailSendServerInfo info = new MailSendServerInfo(languageId, db);
            return info;
        }

        /// <summary>
        /// 変更管理メール送信情報取得SQL文の生成
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <returns>エラーの場合False</returns>
        public static IList<TMQDTCls.MailSendServerInfo> GetItemExtensionInfo(
            ComDB db)
        {

            //送信情報は、長期計画と同じものを使用する
            int groupId = (int)CommonTMQUtil.CommonTMQConstants.MsStructure.GroupId.LongPlanSendMail;
            // 変更管理メール送信情報取得SQL文の生成
            var selectSql = new StringBuilder();
            selectSql.AppendLine("SELECT st.structure_id");
            selectSql.AppendLine(", st.structure_item_id");
            selectSql.AppendLine(", ex1.extension_data AS MailSendServerAddress");
            selectSql.AppendLine(", ex4.extension_data AS Condition");
            selectSql.AppendLine(", ex5.extension_data AS MailSendServerURL");
            selectSql.AppendLine(", ex6.extension_data AS MailSendServerPort");
            selectSql.AppendLine(", ex7.extension_data AS MailSendServerUser");
            selectSql.AppendLine(", ex8.extension_data AS MailSendServerPassword");
            selectSql.AppendLine(" FROM ms_structure st");
            selectSql.AppendLine("LEFT OUTER JOIN ms_item_extension ex1 ON ex1.item_id = st.structure_item_id AND ex1.sequence_no = 1");
            selectSql.AppendLine("LEFT OUTER JOIN ms_item_extension ex4 ON ex4.item_id = st.structure_item_id AND ex4.sequence_no = 4");
            selectSql.AppendLine("LEFT OUTER JOIN ms_item_extension ex5 ON ex5.item_id = st.structure_item_id AND ex5.sequence_no = 5");
            selectSql.AppendLine("LEFT OUTER JOIN ms_item_extension ex6 ON ex6.item_id = st.structure_item_id AND ex6.sequence_no = 6");
            selectSql.AppendLine("LEFT OUTER JOIN ms_item_extension ex7 ON ex7.item_id = st.structure_item_id AND ex7.sequence_no = 7");
            selectSql.AppendLine("LEFT OUTER JOIN ms_item_extension ex8 ON ex8.item_id = st.structure_item_id AND ex8.sequence_no = 8");
            selectSql.AppendLine("WHERE st.structure_group_id = " + groupId);
            selectSql.AppendLine("  AND st.factory_id = 0");
            selectSql.AppendLine("  AND st.delete_flg != 1");
            // 変更管理メール送信情報取得
            IList<TMQDTCls.MailSendServerInfo> results = db.GetListByDataClass<TMQDTCls.MailSendServerInfo>(selectSql.ToString());
            if (results == null || results.Count == 0)
            {
                return null;
            }

            return results;
        }

        /// <summary>
        /// 申請者宛送信対象取得条件(分)取得
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <returns>エラーの場合False</returns>
        public static IList<TMQDTCls.SendMailHistorySerachTime> GetSendMailHistoryTime(
            ComDB db)
        {
            int groupId = (int)CommonTMQUtil.CommonTMQConstants.MsStructure.GroupId.HistoryManagementSendMail;

            // 申請者宛送信対象取得条件(分)取得SQL文の生成
            var selectSql = new StringBuilder();
            selectSql.AppendLine("SELECT CONVERT(DECIMAL, ie.extension_data) AS SerachTime");
            selectSql.AppendLine("FROM  v_structure AS vs");
            selectSql.AppendLine("LEFT OUTER JOIN ms_item_extension AS ie ON (ie.item_id = vs.structure_item_id)");
            selectSql.AppendLine("WHERE vs.structure_group_id = " + groupId);
            selectSql.AppendLine("AND ie.sequence_no = 1");  //
            IList<TMQDTCls.SendMailHistorySerachTime> results = db.GetListByDataClass<TMQDTCls.SendMailHistorySerachTime>(selectSql.ToString());
            if (results == null || results.Count == 0)
            {
                return null;
            }

            return results;
        }

        /// <summary>
        /// 変更管理 申請者情報取得
        /// </summary>
        /// <param name="sTime">申請者宛送信対象取得条件(分)</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>エラーの場合False</returns>
        public static IList<TMQDTCls.SendMailHistoryApplication> GetSendMailHistoryApplicationInfo(
            string sTime,
            ComDB db)
        {

            // 申請情報取得SQL文の生成
            var selectSql = new StringBuilder();
            // 申請状況(差戻と承認済)
            addSql("WITH app_status AS ( ");
            addSql("  SELECT");
            addSql("    vs.structure_id AS app_status_id");
            addSql("    , ex.extension_data AS app_status_code ");
            addSql("  FROM");
            addSql("    v_structure AS vs ");
            addSql("    INNER JOIN ms_item_extension AS ex ");
            addSql("      ON ( ");
            addSql("        vs.structure_item_id = ex.item_id ");
            addSql("        AND ex.sequence_no = 1");
            addSql("      ) ");
            addSql("  WHERE");
            addSql("    vs.structure_group_id = @GroupIdApplicationStatus ");
            addSql("    AND ex.extension_data IN (@ReturnCode, @ApprovedCode)");
            addSql(") ");
            // 承認ユーザごとの差戻と承認済の件数
            addSql(", user_history AS ( ");
            addSql("  SELECT");
            addSql("    hm.application_user_id");
            addSql("    , sum( ");
            addSql("      CASE st.app_status_code ");
            addSql("        WHEN @ApprovedCode THEN 1 ");
            addSql("        ELSE 0 ");
            addSql("        END");
            addSql("    ) AS ApprovedCnt");
            addSql("    , sum( ");
            addSql("      CASE st.app_status_code ");
            addSql("        WHEN @ReturnCode THEN 1 ");
            addSql("        ELSE 0 ");
            addSql("        END");
            addSql("    ) AS ReturnCnt ");
            addSql("  FROM");
            addSql("    hm_history_management AS hm ");
            addSql("    INNER JOIN app_status AS st ");
            addSql("      ON (hm.application_status_id = st.app_status_id) ");
            addSql("  WHERE");
            // ○分以内に更新
            addSql("    update_datetime > DATEADD(MINUTE, @DiffMinutes , sysdatetime()) ");
            addSql("    AND ( ");
            // 承認済
            addSql("      st.app_status_code = @ApprovedCode ");
            // 差戻(更新ユーザが申請者と異なる)
            addSql("      OR ( ");
            addSql("        st.app_status_code = @ReturnCode ");
            addSql("        AND hm.update_user_id != hm.application_user_id");
            addSql("      )");
            addSql("    ) ");
            // 複数工場を考慮しユーザで集計
            addSql("  GROUP BY");
            addSql("    hm.application_user_id");
            addSql(") ");
            // メールアドレスなどユーザの情報を結合
            addSql("SELECT");
            addSql("  uh.application_user_id");
            addSql("  , uh.ApprovedCnt");
            addSql("  , uh.ReturnCnt");
            addSql("  , us.mail_address");
            addSql("  , us.language_id ");
            addSql("FROM");
            addSql("  user_history AS uh ");
            addSql("  LEFT OUTER JOIN ms_user AS us ");
            addSql("    ON (uh.application_user_id = us.user_id) ");
            addSql("WHERE");
            addSql("  us.delete_flg = 0");
            // パラメータを作成
            var param = new
            {
                GroupIdApplicationStatus = (int)CommonTMQUtil.CommonTMQConstants.MsStructure.GroupId.ApplicationStatus,
                ReturnCode = (int)CommonTMQUtil.CommonTMQConstants.MsStructure.StructureId.ApplicationStatus.Return,
                ApprovedCode = (int)CommonTMQUtil.CommonTMQConstants.MsStructure.StructureId.ApplicationStatus.Approved,
                DiffMinutes = (0 - long.Parse(sTime.ToString()))
            };

            IList<TMQDTCls.SendMailHistoryApplication> results = db.GetListByDataClass<TMQDTCls.SendMailHistoryApplication>(selectSql.ToString(), param);
            if (results == null || results.Count == 0)
            {
                return null;
            }

            return results;

            void addSql(string sql)
            {
                // 作成したSQLを文字列置換でコードにするが、長いからメソッドに変更
                selectSql.AppendLine(sql);
            }

            /*
             以下のSQL文を文字列として設定
             WITH app_status AS ( 
               SELECT
                 vs.structure_id AS app_status_id
                 , ex.extension_data AS app_status_code 
               FROM
                 v_structure AS vs 
                 INNER JOIN ms_item_extension AS ex 
                   ON ( 
                     vs.structure_item_id = ex.item_id 
                     AND ex.sequence_no = 1
                   ) 
               WHERE
                 vs.structure_group_id = 2090 
                 AND ex.extension_data IN ('40', '30')
             ) 
             , user_history AS ( 
               SELECT
                 hm.application_user_id
                 , sum( 
                   CASE st.app_status_code 
                     WHEN '40' THEN 1 
                     ELSE 0 
                     END
                 ) AS ApprovedCnt
                 , sum( 
                   CASE st.app_status_code 
                     WHEN '30' THEN 1 
                     ELSE 0 
                     END
                 ) AS ReturnCnt 
               FROM
                 hm_history_management AS hm 
                 INNER JOIN app_status AS st 
                   ON (hm.application_status_id = st.app_status_id) 
               WHERE
                 update_datetime > DATEADD(MINUTE, - 5000, sysdatetime()) 
                 AND ( 
                   st.app_status_code = '40' 
                   OR ( 
                     st.app_status_code = '30' 
                     AND hm.update_user_id != hm.application_user_id
                   )
                 ) 
               GROUP BY
                 hm.application_user_id
             ) 
             SELECT
               uh.application_user_id
               , uh.ApprovedCnt
               , uh.ReturnCnt
               , us.mail_address
               , us.language_id 
             FROM
               user_history AS uh 
               LEFT OUTER JOIN ms_user AS us 
                 ON (uh.application_user_id = us.user_id) 
             WHERE
               us.delete_flg = 0 
             */
        }

        /// <summary>
        /// 変更管理 承認者情報取得
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>エラーの場合False</returns>
        public static IList<TMQDTCls.SendMailHistoryApproval> GetSendMailHistoryApprovalInfo(
            ComDB db)
        {
            //string sTime = SerachTime.ToString();

            // 承認情報取得SQL文の生成
            var selectSql = new StringBuilder();
            addSql("WITH app_status AS ( ");
            addSql("  SELECT");
            addSql("    vs.structure_id AS app_status_id");
            addSql("    , ex.extension_data AS app_status_code ");
            addSql("  FROM");
            addSql("    v_structure AS vs ");
            addSql("    INNER JOIN ms_item_extension AS ex ");
            addSql("      ON ( ");
            addSql("        vs.structure_item_id = ex.item_id ");
            addSql("        AND ex.sequence_no = 1");
            addSql("      ) ");
            addSql("  WHERE");
            addSql("    vs.structure_group_id = @GroupIdApplicationStatus ");
            addSql("    AND ex.extension_data = @RequestCode");
            addSql(") ");
            addSql(", factory_history AS ( ");
            addSql("  SELECT");
            addSql("    hm.factory_id");
            addSql("    , count(*) factory_count ");
            addSql("  FROM");
            addSql("    hm_history_management AS hm ");
            addSql("  WHERE EXISTS (SELECT * FROM app_status AS st WHERE hm.application_status_id = st.app_status_id)");
            addSql("  GROUP BY");
            addSql("    hm.factory_id");
            addSql(") ");
            addSql(", user_history AS ( ");
            addSql("  SELECT");
            addSql("    ex.extension_data AS user_id");
            addSql("    , sum(fh.factory_count) AS RequestCnt ");
            addSql("  FROM");
            addSql("    factory_history AS fh ");
            addSql("    INNER JOIN v_structure AS vs ");
            addSql("      ON (fh.factory_id = vs.structure_id) ");
            addSql("    LEFT OUTER JOIN ms_item_extension AS ex ");
            addSql("      ON ( ");
            addSql("        vs.structure_item_id = ex.item_id ");
            addSql("        AND ex.sequence_no = 4");
            addSql("      ) ");
            addSql("    WHERE ex.extension_data IS NOT NULL ");
            addSql("  GROUP BY");
            addSql("    ex.extension_data");
            addSql(") ");
            addSql("SELECT");
            addSql("  us.user_id");
            addSql("  , us.mail_address");
            addSql("  , us.language_id");
            addSql("  , uh.RequestCnt ");
            addSql("FROM");
            addSql("  user_history AS uh ");
            addSql("  LEFT OUTER JOIN ms_user AS us ");
            addSql("    ON (uh.user_id = us.user_id)");
            // パラメータ
            var param = new
            {
                GroupIdApplicationStatus = (int)CommonTMQUtil.CommonTMQConstants.MsStructure.GroupId.ApplicationStatus,
                RequestCode = (int)CommonTMQUtil.CommonTMQConstants.MsStructure.StructureId.ApplicationStatus.Request
            };

            IList<TMQDTCls.SendMailHistoryApproval> results = db.GetListByDataClass<TMQDTCls.SendMailHistoryApproval>(selectSql.ToString(), param);
            if (results == null || results.Count == 0)
            {
                return null;
            }

            return results;

            void addSql(string sql)
            {
                // 作成したSQLを文字列置換でコードにするが、長いからメソッドに変更
                selectSql.AppendLine(sql);
            }

            /*
            WITH app_status AS ( 
              SELECT
                vs.structure_id AS app_status_id
                , ex.extension_data AS app_status_code 
              FROM
                v_structure AS vs 
                INNER JOIN ms_item_extension AS ex 
                  ON ( 
                    vs.structure_item_id = ex.item_id 
                    AND ex.sequence_no = 1
                  ) 
              WHERE
                vs.structure_group_id = 2090 
                AND ex.extension_data = '20'
            ) 
            , factory_history AS ( 
              SELECT
                hm.factory_id
                , count(*) factory_count 
              FROM
                hm_history_management AS hm 
              WHERE EXISTS (SELECT * FROM app_status AS st WHERE hm.application_status_id = st.app_status_id)
              GROUP BY
                hm.factory_id
            ) 
            , user_history AS ( 
              SELECT
                ex.extension_data AS user_id
                , sum(fh.factory_count) AS RequestCnt 
              FROM
                factory_history AS fh 
                INNER JOIN v_structure AS vs 
                  ON (fh.factory_id = vs.structure_id) 
                LEFT OUTER JOIN ms_item_extension AS ex 
                  ON ( 
                    vs.structure_item_id = ex.item_id 
                    AND ex.sequence_no = 4
                  ) 
              WHERE ex.extension_data IS NOT NULL
              GROUP BY
                ex.extension_data
            ) 
            SELECT
              us.user_id
              , us.mail_address
              , us.language_id
              , uh.RequestCnt 
            FROM
              user_history AS uh 
              LEFT OUTER JOIN ms_user AS us 
                ON (uh.user_id = us.user_id)
                        */
        }

        #region メール送信情報クラス
        /// <summary>
        /// メール送信に必要な情報クラス
        /// </summary>
        public class MailSendServerInfo
        {
            /// <summary>Gets 送信サーバURL</summary>
            /// <value>送信サーバURL</value>
            public string Url { get; }
            /// <summary>Gets 送信ポート番号</summary>
            /// <value>送信ポート番号</value>
            public int Port { get; }
            /// <summary>Gets メール送信元アドレス</summary>
            /// <value>メール送信元アドレス</value>
            public string SendFromAddress { get; }
            /// <summary>Gets 認証ユーザ</summary>
            /// <value>認証ユーザ</value>
            public string AuthUser { get; }
            /// <summary>Gets 認証パスワード</summary>
            /// <value>認証パスワード</value>
            public string AuthPassword { get; }
            /// <summary>Gets a value indicating whether gets 情報が取得できなかった場合、True</summary>
            /// <value>情報が取得できなかった場合、True</value>
            public bool IsError { get; }
            /// <summary>Gets メールタイトル</summary>
            /// <value>メールタイトル</value>
            public string MailSendTitle { get; }
            /// <summary>Gets メール本文</summary>
            /// <value>メール本文</value>
            public string MailSendBody { get; }
            /// <summary>Gets 送信対象取得条件(日数)</summary>
            /// <value>送信対象取得条件(日数)</value>
            public string SendCondition { get; }
            /// <summary>Gets 送信対象取得条件(日数)</summary>
            /// <value>送信対象除外条件(日数)</value>
            public decimal SendExcludeCondition { get; }

            /// <summary>
            /// コンストラクタ　メール送信情報マスタより内容を設定
            /// </summary>
            /// <param name="db">DB接続</param>
            public MailSendServerInfo(string languageId, ComDB db)
            {
                IList<TMQDTCls.MailSendServerInfo> mailsendinfo = GetItemExtensionInfo(db);

                // 長期計画メール送信情報マスタより情報を取得
                // （変更管理のメール送信も長期計画の送信情報と同じものを使用する）
                if (mailsendinfo == null || mailsendinfo.Count == 0)
                {
                    // 取得できなければエラー
                    this.IsError = true;
                    return;
                }

                // 取得件数チェック
                if (mailsendinfo.Count > 1)
                {
                    // 2件以上取得した場合はエラー
                    this.IsError = true;
                    return;
                }

                // サーバURLかポート番号がNullかどうか判定
                if (string.IsNullOrEmpty(mailsendinfo[0].MailSendServerURL) || mailsendinfo[0].MailSendServerPort == null)
                {
                    // いずれかがNullならエラー
                    this.IsError = true;
                    return;
                }

                // 正常な場合、値を設定
                this.Url = mailsendinfo[0].MailSendServerURL;
                int iport;
                int.TryParse(mailsendinfo[0].MailSendServerPort, out iport);
                //this.Port = iport ?? -1;   // ポート番号がNullはチェックしたのであり得ない
                this.Port = iport;   // ポート番号がNullはチェックしたのであり得ない
                this.AuthUser = mailsendinfo[0].MailSendServerUser;
                this.AuthPassword = mailsendinfo[0].MailSendServerPassword;
                this.MailSendTitle = mailsendinfo[0].MailTitle;
                this.MailSendBody = mailsendinfo[0].MailBody;
                this.SendCondition = mailsendinfo[0].Condition;
                this.SendFromAddress = mailsendinfo[0].MailSendServerAddress;
                decimal decval = 0;
                if (mailsendinfo[0].ExcludeCondition != null && mailsendinfo[0].ExcludeCondition.Length > 0)
                {
                    decimal.TryParse(mailsendinfo[0].ExcludeCondition, out decval);
                }
                this.SendExcludeCondition = decval;

                // エラーなし
                this.IsError = false;
            }
        }
        #endregion

    }
}
