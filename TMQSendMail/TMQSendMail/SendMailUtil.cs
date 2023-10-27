using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComRes = CommonSTDUtil.CommonResources;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
//using ComST = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using TMQDTCls = TMQSendMail.TMQSendMailDataClass;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using CommonSTDUtil.CommonLogger;
using CommonWebTemplate.Models.Common;


namespace TMQSendMail
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

        /// <summary>会社コード</summary>
        public const string COMPANY_CD = "000001";

        /// <summary>長期計画件名メール送信 メール本文置換文字列</summary>
        public const string MAIL_BODY_REPLACE_STRING = "[subject_text]";

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
                //string msg01 = ComUtil.GetPropertiesMessage(ComRes.ID.ID941260020, languageId, null, db, null); // パラメータ設定エラー
                string msg01 = "長期計画メール送信情報の設定に誤りがあります。"; // パラメータ設定エラー
                string msg02 = ComUtil.GetPropertiesMessage(new string[] { ComRes.ID.ID941160005, "111170038" }, languageId, null, db, null); // 長期計画メール送信対象情報がありません。
                string msg03 = ComUtil.GetPropertiesMessage(new string[] { ComRes.ID.ID941100001, "111170032", "[subject]" }, languageId, null, db, null); // 長計件名:[subject]
                string msg04 = ComUtil.GetPropertiesMessage(new string[] { ComRes.ID.ID941060012, "111170038" }, languageId, null, db, null); // 長期計画メール送信開始
                string msg05 = ComUtil.GetPropertiesMessage(new string[] { ComRes.ID.ID941120011, "111170038" }, languageId, null, db, null); // 長期計画メール送信終了

                // 長期計画メール送信マスタメンテ情報取得
                MailSendServerInfo server = GetMailSendServerInfo(languageId, db);

                if (server.IsError)
                {
                    logger.Error(msg01); // パラメータ設定エラー
                    return false;
                }

                // 送信メールテンプレート情報取得
                int trnsKey1 = 0;
                int trnsKey2 = 0;
                int.TryParse(server.MailSendTitle, out trnsKey1);
                int.TryParse(server.MailSendBody, out trnsKey2);
                IList<TMQSendMailDataClass.SendMailTemplateInfo> templateInfo = GetSendMailTemplateInfo(trnsKey1, trnsKey2, db);
                

                // メール送信対象件名取得
                IList<TMQDTCls.SendMailLongPlanInfo> sendLongPlanList = null;
                GetSendMailLongPlanSubject(languageId, server.SendCondition, ref sendLongPlanList, db);
                if (sendLongPlanList == null || sendLongPlanList.Count == 0)
                {
                    // メール送信対象件名なし
                    // ログ出力
                    logger.WriteLog(CommonLoggerBase.LogLevel.Info, CommonLoggerBase.LogType.None, msg02); // 長期計画メール送信対象情報がありません。
                    return true;
                }

                // 送信対象のうち、送信対象除外となる長期計画件名を取得
                IList<TMQDTCls.SendMailLongPlanInfo> sendExcludeLongPlanList = sendLongPlanList.Where(x => x.DiffDays > server.SendExcludeCondition).ToList();
                if (sendExcludeLongPlanList != null && sendExcludeLongPlanList.Count > 0)
                {
                    //List<string> excludeList = sendExcludeLongPlanList.Select(x => x.Subject).Distinct().ToList();
                    logMsg = string.Empty;

                    string strWkMsg = msg03; // 長計件名:[subject]

                    // ログ出力
                    logger.WriteLog(CommonLoggerBase.LogLevel.Info, CommonLoggerBase.LogType.None, "以下の長期計画件名はスケジュール日から一定期間(" + server.SendExcludeCondition + "日)経過しているため送信対象から除外");
                    foreach (TMQDTCls.SendMailLongPlanInfo excludeInfo in sendExcludeLongPlanList)
                    {
                        //logMsg = strWkMsg.Replace("[subject]", excludeInfo.Subject);
                        logMsg = "   長期計画件名 [" + excludeInfo.Subject + "]";
                        logMsg += " スケジュール日 [" + excludeInfo.ScheduleDate.ToString("yyyy/MM/dd") + "]";
                        logMsg += " スケジュール日経過日数 [" + excludeInfo.DiffDays.ToString() + "]";
                        logger.WriteLog(CommonLoggerBase.LogLevel.Info, CommonLoggerBase.LogType.None, logMsg); // 長計件名:送信対象除外となる長期計画件名 + スケジュール日:スケジュール日 + :経過日数
                    }
                }

                // 送信対象絞り込み(送信対象除外分を除く)
                IList<TMQDTCls.SendMailLongPlanInfo> sendLongPlanList2 = sendLongPlanList.Where(x => x.DiffDays <= server.SendExcludeCondition).ToList();
                if (sendLongPlanList2 == null || sendLongPlanList2.Count == 0)
                {
                    // メール送信対象件名なし
                    // ログ出力
                    logger.WriteLog(CommonLoggerBase.LogLevel.Info, CommonLoggerBase.LogType.None, msg02); // 長期計画メール送信対象情報がありません。
                    return true;
                }

                // メール送信開始
                // ログ出力
                logger.WriteLog(CommonLoggerBase.LogLevel.Info, CommonLoggerBase.LogType.None, msg04); // 長期計画メール送信開始

                // 通知対象ユーザのリストをユーザIDのリストに変換
                List<int> userIdList = sendLongPlanList2.Select(x => x.PersonId).Distinct().ToList();
                if (userIdList == null || userIdList.Count == 0)
                {
                    // ユーザIDが設定されていない

                    return false;
                }

                // 送信元メールアドレス
                string fromAddress = server.SendFromAddress;
                string fromAddressName = "";
                //// ユーザIDのリストで、ログインテーブルのリストを取得
                //IList<string> mailAddressList = getMailAddressList(userIdList);
                //if (mailAddressList == null || mailAddressList.Count == 0)
                //{
                //    // 取得できなかった場合エラー
                //    return false;
                //}
                // メールアドレスのリストをカンマ区切りの文字列に変換する
                //string toAddressCsv = string.Join(",", mailAddressList.Distinct().ToList());
                string toAddressCsv = string.Empty;
                string sendSubjectByPerson = string.Empty;
                foreach (int userId in userIdList)
                {
                    if (userId == 0)
                    {
                        logger.WriteLog(CommonLoggerBase.LogLevel.Error, CommonLoggerBase.LogType.None, "以下の長期計画件名は、担当者が設定されていません");
                        List<string> subjectWkList = sendLongPlanList2.Where(x => x.PersonId == 0).Select(x => x.Subject).Distinct().ToList();
                        foreach (string subjectwk in subjectWkList)
                        {
                            logger.WriteLog(CommonLoggerBase.LogLevel.Error, CommonLoggerBase.LogType.None, "   長期計画件名 [" + subjectwk + "]");
                        }
                        continue;
                    }
                    var toSendMailByPersonList = sendLongPlanList2.Where(x => x.PersonId == userId).Select(x => x.Subject).Distinct().ToList();
                    var toSendMailAddressByPersonList = sendLongPlanList2.Where(x => x.PersonId == userId).Select(x => x.PersonMailAddress).Distinct().ToList();
                    if (toSendMailAddressByPersonList[0] == null || toSendMailAddressByPersonList[0].Length == 0)
                    {
                        // メールアドレスが未設定
                        // ログ出力
                        logger.WriteLog(CommonLoggerBase.LogLevel.Error, CommonLoggerBase.LogType.None, "以下の長期計画件名は、担当者のメールアドレスが設定されていません");
                        IList<TMQDTCls.SendMailLongPlanInfo> toSendMailAddressNoneByPersonList = sendLongPlanList2.Where(x => x.PersonId == userId).ToList();
                        List<string> noneAddressList = toSendMailAddressNoneByPersonList.Select(x => x.Subject).Distinct().ToList();
                        foreach (string subjectwk in noneAddressList)
                        {
                            logger.WriteLog(CommonLoggerBase.LogLevel.Error, CommonLoggerBase.LogType.None, "   長期計画件名 [" + subjectwk + "] 担当者ID [" + userId + "]");
                        }
                    }
                    else
                    {
                        //if (toAddressCsv.Length > 0)
                        //{
                        //    toAddressCsv += ",";
                        //}
                        //toAddressCsv += toAddressList[0].PersonMailAddress;
                        toAddressCsv = toSendMailAddressByPersonList[0];

                        // 担当者の言語IDを取得
                        List<string> personLanguageId = sendLongPlanList2.Where(x => x.PersonId == userId).Select(x => x.PersonLanguageId).Distinct().ToList();
                        // 担当者の言語IDに紐つく送信メール件名、メール本文を取得
                        IList<TMQDTCls.SendMailTemplateInfo> mailTemplate = templateInfo.Where(x => x.LanguageId == personLanguageId[0]).ToList();

                        // 件名
                        sendSubjectByPerson = string.Join("\n", toSendMailByPersonList.Distinct().ToList());
                        // メール本文
                        //string mailTextTitle = server.MailSendTitle;
                        List<string> mailTitleList = mailTemplate.Where(x => x.TranslationId == trnsKey1).Select(x => x.TranslationText).ToList();
                        List<string> mailBodyList = mailTemplate.Where(x => x.TranslationId == trnsKey2).Select(x => x.TranslationText).ToList();
                        string mailTextTitle = mailTitleList[0];
                        // 改行を置換
                        //string mailTextBody = server.MailSendBody.Replace("\\n", "\n");
                        string mailTextBody = mailBodyList[0].Replace("\\n", "\n");
                        // メール本文の長期計画件名部分([subject_text])を取得した件名に置換
                        mailTextBody = mailTextBody.Replace(MAIL_BODY_REPLACE_STRING, sendSubjectByPerson);

                        // メール送信サーバ情報セット
                        string mailSendServer = server.Url; // サーバURL
                        int mailSendPort = server.Port;  // ポート
                        string mailAuthUser = server.AuthUser;   // 承認ユーザ
                        string mailAuthPasswd = server.AuthPassword;   // 承認パスワード
                        // 送信
                        if (!SendMailImpl(fromAddress, fromAddressName, toAddressCsv, null, null, mailTextTitle, mailTextBody, null, mailSendServer, mailSendPort, mailAuthUser, mailAuthPasswd))
                        {
                            // ログ出力
                            foreach (string subject in toSendMailByPersonList.Distinct())
                            {
                                logger.WriteLog(CommonLoggerBase.LogLevel.Error, CommonLoggerBase.LogType.None, "担当者メールアドレス [" + toAddressCsv + "] 長期計画件名 [" + subject + "]" + "送信失敗");
                            }
                        }
                        else
                        {
                            // ログ出力
                            foreach (string subject in toSendMailByPersonList.Distinct())
                            {
                                logger.WriteLog(CommonLoggerBase.LogLevel.Info, CommonLoggerBase.LogType.None, "担当者メールアドレス [" + toAddressCsv + "] 長期計画件名 [" + subject + "]" + "送信完了");
                            }
                        }

                    }

                }
                // 長期計画メール送信終了
                // ログ出力
                logger.WriteLog(CommonLoggerBase.LogLevel.Info, CommonLoggerBase.LogType.None, msg05); // 長期計画メール送信終了

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 長期計画メール送信情報取得
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="condition"></param>
        /// <param name="sendLongPlanList"></param>
        /// <param name="db"></param>
        private void GetSendMailLongPlanSubject(string languageId, string condition ,　ref IList<TMQDTCls.SendMailLongPlanInfo> sendLongPlanList, ComDB db)
        {
            // 長期計画メール送信マスタメンテに設定している対象取得条件(日数)をセット
            string checkDay = "";
            checkDay = condition;
            if (checkDay.Length == 0)
            {
                checkDay = "0";
            }

            // 長期計画メール送信情報取得SQL文の生成
            //executeSql = TMQUtil.GetSqlStatementSearch(false, baseSql, whereSql, withSql);
            var selectSql = new StringBuilder();
            selectSql.AppendLine("SELECT lp.long_plan_id AS LongPlanId");
            selectSql.AppendLine("     , lp.subject AS Subject");
            selectSql.AppendLine("     , lp.person_id AS PersonId");
            selectSql.AppendLine("     , us.mail_address AS PersonMailAddress");
            selectSql.AppendLine("     , us.language_id AS PersonLanguageId");
            selectSql.AppendLine("     , msd.schedule_date AS ScheduleDate");
            selectSql.AppendLine("     , DATEDIFF(day, msd.schedule_date, GETDATE()) AS DiffDays");
            selectSql.AppendLine(" FROM ln_long_plan lp");
            selectSql.AppendLine("INNER JOIN mc_management_standards_content msc ON");
            selectSql.AppendLine("    lp.long_plan_id = msc.long_plan_id");
            selectSql.AppendLine("INNER JOIN mc_maintainance_schedule ms ON");
            selectSql.AppendLine("    msc.management_standards_content_id = ms.management_standards_content_id");
            selectSql.AppendLine("INNER JOIN mc_maintainance_schedule_detail msd ON");
            selectSql.AppendLine("    ms.maintainance_schedule_id = msd.maintainance_schedule_id");
            selectSql.AppendLine("    AND ISNULL(msd.complition, 0) != 1"); // 完了フラグが1でないもの
            selectSql.AppendLine("LEFT JOIN ms_user us ON");
            selectSql.AppendLine("    lp.person_id = us.user_id");
            selectSql.AppendLine("WHERE GETDATE() >= DATEADD(dd, (ISNULL(msc.preparation_period, 0) + " + checkDay.ToString() + ")*(-1), msd.schedule_date)");
            selectSql.AppendLine("  AND msd.schedule_date <= GETDATE()");
            selectSql.AppendLine("GROUP BY lp.long_plan_id, msc.long_plan_id, lp.subject, lp.person_id, us.mail_address, us.language_id");
            selectSql.AppendLine("     , msd.schedule_date");
            selectSql.AppendLine("     , DATEDIFF(day, msd.schedule_date, GETDATE())");
            selectSql.AppendLine("ORDER BY lp.person_id, lp.long_plan_id");
            // メール送信対象件名取得
            IList<TMQDTCls.SendMailLongPlanInfo> results = db.GetListByDataClass<TMQDTCls.SendMailLongPlanInfo>(selectSql.ToString());
            if (results == null || results.Count == 0)
            {
                //return null;
            }
            sendLongPlanList = results;

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
        /// 一覧画面 アイテム一覧取得（マスタメンテナンス用）
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="sqlName">SQLファイル名(拡張子は含まない)</param>
        /// <param name="results">検索結果</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>エラーの場合False</returns>
        public static IList<TMQDTCls.MailSendServerInfo> GetItemExtensionInfo(
            //SearchConditionForMaster condition,
            //string sqlName,
            //ref IList<MailSendServerInfo> results,
            ComDB db)
        {

            int groupId = (int)CommonTMQUtil.CommonTMQConstants.MsStructure.GroupId.LongPlanSendMail;
            // 長期計画メール送信情報取得SQL文の生成
            var selectSql = new StringBuilder();
            selectSql.AppendLine("SELECT st.structure_id");
            selectSql.AppendLine(", st.structure_item_id");
            selectSql.AppendLine(", ex1.extension_data AS MailSendServerAddress");
            selectSql.AppendLine(", ex2.extension_data AS MailTitle");
            selectSql.AppendLine(", ex3.extension_data AS MailBody");
            selectSql.AppendLine(", ex4.extension_data AS Condition");
            selectSql.AppendLine(", ex5.extension_data AS MailSendServerURL");
            selectSql.AppendLine(", ex6.extension_data AS MailSendServerPort");
            selectSql.AppendLine(", ex7.extension_data AS MailSendServerUser");
            selectSql.AppendLine(", ex8.extension_data AS MailSendServerPassword");
            selectSql.AppendLine(", ex9.extension_data AS ExcludeCondition");
            selectSql.AppendLine(" FROM ms_structure st");
            selectSql.AppendLine("LEFT OUTER JOIN ms_item_extension ex1 ON ex1.item_id = st.structure_item_id AND ex1.sequence_no = 1");
            selectSql.AppendLine("LEFT OUTER JOIN ms_item_extension ex2 ON ex2.item_id = st.structure_item_id AND ex2.sequence_no = 2");
            selectSql.AppendLine("LEFT OUTER JOIN ms_item_extension ex3 ON ex3.item_id = st.structure_item_id AND ex3.sequence_no = 3");
            selectSql.AppendLine("LEFT OUTER JOIN ms_item_extension ex4 ON ex4.item_id = st.structure_item_id AND ex4.sequence_no = 4");
            selectSql.AppendLine("LEFT OUTER JOIN ms_item_extension ex5 ON ex5.item_id = st.structure_item_id AND ex5.sequence_no = 5");
            selectSql.AppendLine("LEFT OUTER JOIN ms_item_extension ex6 ON ex6.item_id = st.structure_item_id AND ex6.sequence_no = 6");
            selectSql.AppendLine("LEFT OUTER JOIN ms_item_extension ex7 ON ex7.item_id = st.structure_item_id AND ex7.sequence_no = 7");
            selectSql.AppendLine("LEFT OUTER JOIN ms_item_extension ex8 ON ex8.item_id = st.structure_item_id AND ex8.sequence_no = 8");
            selectSql.AppendLine("LEFT OUTER JOIN ms_item_extension ex9 ON ex9.item_id = st.structure_item_id AND ex9.sequence_no = 9");
            selectSql.AppendLine("WHERE st.structure_group_id = " + groupId);
            selectSql.AppendLine("  AND st.factory_id = 0");
            selectSql.AppendLine("  AND st.delete_flg != 1");
            // 長期計画メール送信情報取得
            IList<TMQDTCls.MailSendServerInfo> results = db.GetListByDataClass<TMQDTCls.MailSendServerInfo>(selectSql.ToString());
            if (results == null || results.Count == 0)
            {
                return null;
            }

            return results;
        }

        /// <summary>
        /// 一覧画面 アイテム一覧取得（マスタメンテナンス用）
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="sqlName">SQLファイル名(拡張子は含まない)</param>
        /// <param name="results">検索結果</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>エラーの場合False</returns>
        public static IList<TMQDTCls.SendMailTemplateInfo> GetSendMailTemplateInfo(
            int trnsKey1,
            int trnsKey2,
            ComDB db)
        {

            // 長期計画メール送信情報取得SQL文の生成
            var selectSql = new StringBuilder();
            selectSql.AppendLine("SELECT trs.translation_id AS TranslationId, trs.language_id AS LanguageId, trs.translation_text AS TranslationText");
            selectSql.AppendLine("FROM ms_translation trs");
            selectSql.AppendLine("WHERE trs.location_structure_id = 0");
            selectSql.AppendLine("  AND trs.translation_id = " + trnsKey1); // 送信メール件名
            selectSql.AppendLine("UNION ALL");
            selectSql.AppendLine("SELECT trs2.translation_id AS TranslationId, trs2.language_id AS LanguageId, trs2.translation_text AS TranslationText");
            selectSql.AppendLine("FROM ms_translation trs2");
            selectSql.AppendLine("WHERE trs2.location_structure_id = 0");
            selectSql.AppendLine("  AND trs2.translation_id = " + trnsKey2); // 送信メール本文
            IList<TMQDTCls.SendMailTemplateInfo> results = db.GetListByDataClass<TMQDTCls.SendMailTemplateInfo>(selectSql.ToString());
            if (results == null || results.Count == 0)
            {
                return null;
            }

            return results;
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
            /// コンストラクタ　長期計画メール送信情報マスタより内容を設定
            /// </summary>
            /// <param name="db">DB接続</param>
            public MailSendServerInfo(string languageId, ComDB db)
            {
                IList<TMQDTCls.MailSendServerInfo> mailsendinfo = GetItemExtensionInfo(db);

                // 長期計画メール送信情報マスタより情報を取得
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
