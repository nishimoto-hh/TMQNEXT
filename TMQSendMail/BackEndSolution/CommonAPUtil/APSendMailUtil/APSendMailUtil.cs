using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Configuration;
using CommonSTDUtil.CommonDBManager;
using CommonSTDUtil.CommonLogger;

using APConsts = APConstants.APConstants;
using APResources = CommonAPUtil.APCommonUtil.APResources;
using ComDao = CommonAPUtil.APCommonUtil.APCommonDataClass;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;

namespace CommonAPUtil.APSendMailUtil
{
    /// <summary>
    /// メール送信クラス
    /// </summary>
    public class APSendMailUtil
    {
        /// <summary>メールモード 1:自動FAX</summary>
        public const decimal SEND_MODE_AUTO_FAX = 1;

        /// <summary>メールモード 0:メール送信のみ</summary>
        public const decimal SEND_MODE_MAIL_ONLY = 0;

        /// <summary>接続時タイムアウト 初期値:10000(ミリ秒)</summary>
        public const string DEFAULT_TIMEOUT = "20000";

        /// <summary>会社コード</summary>
        public const string COMPANY_CD = "000001";

        /// <summary>自動FAX</summary>
        public const decimal AUTO_FAX = 10801;

        /// <summary>注文請書メール</summary>
        public const decimal ORDER_MAIL = 20103;

        /// <summary>注文書メール</summary>
        public const decimal PURCHASE_MAIL = 20104;
        /// <summary>注文書(取消)メール</summary>
        public const decimal PURCHASE_CANCEL_MAIL = 20105;

        /// <summary>納品書メール</summary>
        public const decimal DELIVERY_MAIL = 20106;

        /// <summary>DB操作クラス</summary>
        //protected CommonDBManager db;

        /// <summary>ルート物理パス</summary>
        //private string rootPatha = "a";

        /// <summary>ログ出力</summary>
        private static CommonLogger logger = CommonLogger.GetInstance("logger");

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
                logger.Error(ComUtil.GetPropertiesMessage(APResources.ID.MB00005));
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
                logger.Error(ComUtil.GetPropertiesMessage(APResources.ID.MB00005));
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
        /// メール送信メソッド（添付ファイルあり）
        /// </summary>
        /// <param name="toUserList">メール送信先ユーザのリスト</param>
        /// <param name="mailFormatId">メールフォーマットID</param>
        /// <param name="subjectParamList">メール件名のパラメータリスト</param>
        /// <param name="bodyParamList">メール本文のパラメータリスト</param>
        /// <param name="filePathList">添付ファイルパス</param>
        /// <param name="tantoCd">担当者コード</param>
        /// <param name="sendMode">送信モード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>処理結果 true:成功、false:失敗</returns>
        public static bool SendMail(
            List<string> toUserList,
            decimal mailFormatId,
            List<string> subjectParamList,
            List<string> bodyParamList,
            List<string> filePathList,
            string tantoCd,
            decimal sendMode,
            ComDB db)
        {

            bool boolRtn = false;
            bool isError = false;
            string subject = string.Empty;
            string body = string.Empty;
            string sql = "";

            // パラメータ.メール送信先ユーザのリスト分、以下の処理を実行する。
            if (toUserList == null || toUserList.Count == 0)
            {
                // 「引数が設定されていません。」
                logger.Error(ComUtil.GetPropertiesMessage(APResources.ID.MB00005));
                return false;
            }

            // パラメータ.メール送信先ユーザのリスト分、以下の処理を実行する。
            for (int i = 0; i < toUserList.Count; i++)
            {
                // Login user = new Login();
                string languageId = string.Empty;
                string toAddress = string.Empty;
                string fromAddress = string.Empty;
                // メール送信先ユーザから送信先担当コードを取得する。
                string toTantoCd = toUserList[i];

                // 送信先担当コードから送信先担当者情報を取得する。
                sql = "";
                sql = sql + "select ";
                sql = sql + "    * ";
                sql = sql + "from ";
                sql = sql + "    login ";
                sql = sql + "where ";
                sql = sql + "    user_id = @UserId ";
                sql = sql + "; ";

                ComDao.LoginEntity loginTo = new ComDao.LoginEntity();
                loginTo = db.GetEntity<ComDao.LoginEntity>(
                    sql,
                    new
                    {
                        UserId = toTantoCd
                    });

                // 送信先担当者情報が取得できなかった場合
                if (loginTo == null)
                {
                    // エラーログを出力する。
                    // 「該当データがありません。」
                    logger.Error(ComUtil.GetPropertiesMessage(APResources.ID.MS00001));
                    // エラーフラグをtrueにする。
                    isError = true;
                    // 次のリストのデータ処理に移動する。
                    continue;
                }

                // 送信先担当者情報から言語IDを取得する。
                languageId = loginTo.LanguageId;
                // 送信先担当者情報から送信先メールアドレスを取得する。
                toAddress = loginTo.MailAddress;

                // パラメータ.担当コードから送信元担当者情報を取得する。
                sql = "";
                sql = sql + "select ";
                sql = sql + "    * ";
                sql = sql + "from ";
                sql = sql + "    login ";
                sql = sql + "where ";
                sql = sql + "    user_id = @UserId";
                sql = sql + "; ";

                ComDao.LoginEntity loginFrom = new ComDao.LoginEntity();
                loginFrom = db.GetEntity<ComDao.LoginEntity>(
                    sql,
                    new
                    {
                        UserId = tantoCd
                    });
                // 送信元担当者情報が取得できなかった場合
                if (loginFrom == null)
                {
                    // エラーログを出力する。
                    // 「該当データがありません。」
                    logger.Error(ComUtil.GetPropertiesMessage(APResources.ID.MS00001));
                    // エラーフラグをtrueにする。
                    isError = true;
                    // 次のリストのデータ処理に移動する。
                    continue;
                }
                // 送信元担当者情報から送信元メールアドレスを取得する。
                fromAddress = loginFrom.MailAddress;
                // メールテンプレートを取得する。
                sql = "";
                sql = sql + "select ";
                sql = sql + "    * ";
                sql = sql + "from ";
                sql = sql + "    mail_template ";
                sql = sql + "where ";
                sql = sql + "    mail_format_id = @MailFormatId ";
                sql = sql + "and language_id = @LanguageId ";
                sql = sql + "; ";

                ComDao.MailTemplateEntity mailTemplate = new ComDao.MailTemplateEntity();
                mailTemplate = db.GetEntity<ComDao.MailTemplateEntity>(
                    sql,
                    new
                    {
                        MailFormatId = mailFormatId,
                        LanguageId = languageId
                    });

                // メールテンプレートが取得できなかった場合
                if (mailTemplate == null || string.IsNullOrEmpty(mailTemplate.TextTitle) || string.IsNullOrEmpty(mailTemplate.TextBody))
                {
                    // エラーログを出力する。
                    // 「該当データがありません。」
                    logger.Error(ComUtil.GetPropertiesMessage(APResources.ID.MS00001));
                    // エラーフラグをtrueにする。
                    isError = true;
                    // 次のリストのデータ処理に移動する。
                    continue;
                }
                try
                {
                    // メールテンプレートから件名を取得する。
                    subject = string.Empty;
                    if (subjectParamList == null)
                    {
                        //引数がない場合そのまま設定
                        subject = mailTemplate.TextTitle;
                    }
                    else
                    {
                        //引数がある場合、フォーマット変換する
                        object[] subject_objects = new object[subjectParamList.Count];
                        for (int j = 0; j < subjectParamList.Count; j++)
                        {
                            subject_objects[j] = subjectParamList[j];
                        }
                        subject = string.Format(mailTemplate.TextTitle, subject_objects);
                    }
                }
                catch (Exception ex)
                {
                    // エラーログを出力する。
                    logger.Error(ex.Message);
                    // エラーフラグをtrueにする。
                    isError = true;
                    // 次のリストのデータ処理に移動する。
                    continue;
                }

                try
                {
                    // メールテンプレートから本文を取得する。
                    body = string.Empty;
                    if (bodyParamList == null)
                    {
                        //引数がない場合そのまま設定
                        body = mailTemplate.TextBody;
                    }
                    else
                    {
                        //引数がある場合、フォーマット変換する
                        object[] body_objects = new object[bodyParamList.Count];
                        for (int j = 0; j < bodyParamList.Count; j++)
                        {
                            body_objects[j] = bodyParamList[j];
                        }
                        body = string.Format(mailTemplate.TextBody, body_objects);

                    }
                    // 改行文字を置き換え
                    body = body.Replace("\\n", Environment.NewLine);
                }
                catch (Exception ex)
                {
                    // エラーログを出力する。
                    logger.Error(ex.Message);
                    // エラーフラグをtrueにする。
                    isError = true;
                    // 次のリストのデータ処理に移動する。
                    continue;
                }

                // 自社マスタから設定取得
                sql = "";
                sql = sql + "select ";
                sql = sql + "    * ";
                sql = sql + "from ";
                sql = sql + "    company ";
                sql = sql + "where ";
                sql = sql + "    company_cd = '" + COMPANY_CD + "' ";
                sql = sql + "; ";

                MailSendServerInfo sendInfo = GetMailSendServerInfo(db);

                if (sendInfo.IsError)
                {
                    // 「該当データがありません。」
                    logger.Error(ComUtil.GetPropertiesMessage(APResources.ID.MS00001));
                    // エラーフラグをtrueにする。
                    isError = true;
                }

                // メール送信実行
                //if (!SendMailImpl(
                //        toTantoCd,
                //        toAddress,
                //        null,
                //        null,
                //        null,
                //        fromAddress,
                //        null,
                //        subject,
                //        body,
                //        filePathList,
                //        mailFormatId,
                //        tantoCd,
                //        sendMode
                //    ))
                //{
                //    isError = true;
                //}
                bool ret = SendMailImpl(
                    fromAddress,
                    fromAddress,
                    toAddress,
                    null,
                    null,
                    subject,
                    body,
                    filePathList,
                    sendInfo.Url,
                    sendInfo.Port,
                    sendInfo.AuthUser,
                    sendInfo.AuthPassword);
                if (ret == false)
                {
                    isError = true;
                }

            }
            if (isError)
            {
                // １件でもエラーがあればメール送信失敗とする
                boolRtn = false;
            }
            else
            {
                boolRtn = true;

            }
            return boolRtn;
        }

        /// <summary>
        /// メール送信メソッド（メールのみ送信）
        /// </summary>
        /// <param name="toUserList">メール送信先ユーザのリスト</param>
        /// <param name="mailFormatId">メールフォーマットID</param>
        /// <param name="subjectParamList">メール件名のパラメータリスト</param>
        /// <param name="bodyParamList">メール本文のパラメータリスト</param>
        /// <param name="tantoCd">担当者コード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>処理結果 true:成功、false:失敗</returns>
        public static bool SendMail(
            List<string> toUserList,
            decimal mailFormatId,
            List<string> subjectParamList,
            List<string> bodyParamList,
            string tantoCd,
            ComDB db)
        {
            return SendMail(
                toUserList,
                mailFormatId,
                subjectParamList,
                bodyParamList,
                null,
                tantoCd,
                SEND_MODE_MAIL_ONLY,
                db);
        }

        /// <summary>
        /// メール送信メソッド（添付ファイルなし、送信モードあり）
        /// </summary>
        /// <param name="toUserList">メール送信先ユーザのリスト</param>
        /// <param name="mailFormatId">メールフォーマットID</param>
        /// <param name="subjectParamList">メール件名のパラメータリスト</param>
        /// <param name="bodyParamList">メール本文のパラメータリスト</param>
        /// <param name="tantoCd">担当者コード</param>
        /// <param name="sendMode">送信モード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>処理結果 true:成功、false:失敗</returns>
        public static bool SendMail(
            List<string> toUserList,
            decimal mailFormatId,
            List<string> subjectParamList,
            List<string> bodyParamList,
            string tantoCd,
            decimal sendMode,
            ComDB db)
        {
            return SendMail(
                toUserList,
                mailFormatId,
                subjectParamList,
                bodyParamList,
                null,
                tantoCd,
                sendMode,
                db);
        }

        /// <summary>
        /// メール送信メソッド（取引先の送信用）
        /// </summary>
        /// <param name="venderDivision">取引先区分</param>
        /// <param name="venderCd">取引先コード</param>
        /// <param name="venderActiveDate">取引先開始有効日</param>
        /// <param name="mailFormatId">メールフォーマットID</param>
        /// <param name="subjectParamList">メール件名のパラメータリスト</param>
        /// <param name="bodyParamList">メール本文のパラメータリスト</param>
        /// <param name="filePathList">添付ファイルパス</param>
        /// <param name="tantoCd">担当者コード</param>
        /// <param name="sendMode">送信モード</param>
        /// <param name="db">DB操作クラス</param>
        /// <returns>処理結果 true:成功、false:失敗</returns>
        public static bool SendMailVender(
            string venderDivision,
            string venderCd,
            DateTime venderActiveDate,
            decimal mailFormatId,
            List<string> subjectParamList,
            List<string> bodyParamList,
            List<string> filePathList,
            string tantoCd,
            decimal sendMode,
            ComDB db)
        {
            string sql = "";
            string fromAddress = string.Empty;
            string toAddress = string.Empty;
            string faxno = string.Empty;
            // Login user = new Login();
            string languageId = string.Empty;

            // パラメータ.担当コードから送信元担当者情報を取得する。
            sql = "";
            sql = sql + "select ";
            sql = sql + "    * ";
            sql = sql + "from ";
            sql = sql + "    login ";
            sql = sql + "where ";
            sql = sql + "    user_id = @UserID ";
            sql = sql + "; ";
            ComDao.LoginEntity loginTo = new ComDao.LoginEntity();
            loginTo = db.GetEntity<ComDao.LoginEntity>(
                sql,
                new
                {
                    UserID = tantoCd
                });
            // 送信先担当者情報が取得できなかった場合
            if (loginTo == null)
            {
                // エラーログを出力する。
                // 「該当データがありません。」
                logger.Error(ComUtil.GetPropertiesMessage(APResources.ID.MS00001));
                // 戻り値falseを返却する。
                return false;
            }
            // 送信元担当者情報から言語IDを取得する。
            languageId = loginTo.LanguageId;
            // 送信モードがメール送信のみの場合
            if (SEND_MODE_MAIL_ONLY.CompareTo(sendMode) == 0)
            {
                // 送信元担当者情報から送信元メールアドレスを取得する。
                fromAddress = loginTo.MailAddress;
            }

            string subject = string.Empty;
            string body = string.Empty;

            // 取引先マスタを検索する。
            sql = "";
            sql = sql + "select ";
            sql = sql + "    * ";
            sql = sql + "from ";
            sql = sql + "    vender ";
            sql = sql + "where ";
            sql = sql + "    vender_division = @VenderDivision ";
            sql = sql + "and vender_cd = @VenderCd ";
            sql = sql + "and active_date = @VenderActiveDate ";
            sql = sql + "; ";

            ComDao.VenderEntity vender = new ComDao.VenderEntity();
            vender = db.GetEntity<ComDao.VenderEntity>(
                sql,
                new
                {
                    VenderDivision = venderDivision,
                    VenderCd = venderCd,
                    VenderActiveDate = venderActiveDate
                });
            // 取引先情報が取得できた場合
            if (vender != null)
            {
                // メールアドレスを設定する。
                toAddress = vender.Mail;
                // 国内番号を国際番号に変換する。
                string convertTelNo = vender.FaxNo;
                if (!string.IsNullOrEmpty(convertTelNo))
                {
                    // 先頭が0であれば0を消去する
                    if (convertTelNo.Substring(0, 1).CompareTo("0") == 0)
                    {
                        convertTelNo = convertTelNo.Substring(1, convertTelNo.Length - 1);
                    }
                    // 先頭に日本の国際番号「81」を追加
                    convertTelNo = "81" + convertTelNo;
                    // 文字列中のハイフンを消去
                    convertTelNo = convertTelNo.Replace("-", "");
                }
                // FAX番号を設定する。
                faxno = convertTelNo;
            }

            // 送信モードがメール送信のみの場合も
            // 送信モードが自動FAXの場合も同じロジックなので場合分けしない。
            // メールテンプレートを取得する。
            sql = "";
            sql = sql + "select ";
            sql = sql + "    * ";
            sql = sql + "from ";
            sql = sql + "    mail_template ";
            sql = sql + "where ";
            sql = sql + "    mail_format_id = @MailFormatId ";
            sql = sql + "and language_id = @LanguageId ";
            sql = sql + "; ";

            ComDao.MailTemplateEntity mailTemplate = new ComDao.MailTemplateEntity();
            mailTemplate = db.GetEntity<ComDao.MailTemplateEntity>(
                sql,
                new
                {
                    MailFormatId = mailFormatId,
                    LanguageId = languageId
                });

            // メールテンプレートが取得できなかった場合
            if (mailTemplate == null || string.IsNullOrEmpty(mailTemplate.TextTitle) || string.IsNullOrEmpty(mailTemplate.TextBody))
            {
                // エラーログを出力する。
                // 「該当データがありません。」
                logger.Error(ComUtil.GetPropertiesMessage(APResources.ID.MS00001));
                return false;
            }
            try
            {
                // メールテンプレートから件名を取得する。
                subject = string.Empty;
                if (subjectParamList == null)
                {
                    //引数がない場合そのまま設定
                    subject = mailTemplate.TextTitle;
                }
                else
                {
                    //引数がある場合、フォーマット変換する
                    object[] subject_objects = new object[subjectParamList.Count];
                    for (int i = 0; i < subjectParamList.Count; i++)
                    {
                        subject_objects[i] = subjectParamList[i];
                    }
                    subject = string.Format(mailTemplate.TextTitle, subject_objects);
                }
            }
            catch (Exception ex)
            {
                // エラーログを出力する。
                logger.Error(ex.Message);
                return false;
            }

            try
            {
                // メールテンプレートから本文を取得する。
                body = string.Empty;
                if (bodyParamList == null)
                {
                    //引数がない場合そのまま設定
                    body = mailTemplate.TextBody;
                }
                else
                {
                    //引数がある場合、フォーマット変換する
                    object[] body_objects = new object[bodyParamList.Count];
                    for (int i = 0; i < bodyParamList.Count; i++)
                    {
                        body_objects[i] = bodyParamList[i];
                    }
                    body = string.Format(mailTemplate.TextBody, body_objects);

                }
                // 改行文字を置き換え
                body = body.Replace("\\n", Environment.NewLine);
            }
            catch (Exception ex)
            {
                // エラーログを出力する。
                logger.Error(ex.Message);
                return false;
            }
            // 自社マスタから設定取得
            sql = "";
            sql = sql + "select ";
            sql = sql + "    * ";
            sql = sql + "from ";
            sql = sql + "    company ";
            sql = sql + "where ";
            sql = sql + "    company_cd = '" + COMPANY_CD + "' ";
            sql = sql + "; ";

            MailSendServerInfo sendInfo = GetMailSendServerInfo(db);

            if (sendInfo.IsError)
            {
                // 「該当データがありません。」
                logger.Error(ComUtil.GetPropertiesMessage(APResources.ID.MS00001));
                return false;
            }

            // メール送信実行
            //if (!SendMailImpl(
            //        null,
            //        toAddress,
            //        faxno,
            //        null,
            //        null,
            //        fromAddress,
            //        null,
            //        subject,
            //        body,
            //        filePathList,
            //        mailFormatId,
            //        tantoCd,
            //        sendMode
            //    ))
            //{
            //    Debug.WriteLine("メール送信実行エラー");
            //    return false;
            //}
            bool ret = SendMailImpl(
                fromAddress,
                fromAddress,
                toAddress,
                null,
                null,
                subject,
                body,
                filePathList,
                sendInfo.Url,
                sendInfo.Port,
                sendInfo.AuthUser,
                sendInfo.AuthPassword);
            if (ret == false)
            {
                // エラーログを出力する。
                logger.Error(ComUtil.GetPropertiesMessage(APResources.ID.MS00068));
                return false;
            }
            return true;
        }

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
            /// <summary>Gets 認証ユーザ</summary>
            /// <value>認証ユーザ</value>
            public string AuthUser { get; }
            /// <summary>Gets 認証パスワード</summary>
            /// <value>認証パスワード</value>
            public string AuthPassword { get; }
            /// <summary>Gets a value indicating whether gets 情報が取得できなかった場合、True</summary>
            /// <value>情報が取得できなかった場合、True</value>
            public bool IsError { get; }

            /// <summary>
            /// コンストラクタ　自社マスタより内容を設定
            /// </summary>
            /// <param name="db">DB接続</param>
            public MailSendServerInfo(ComDB db)
            {
                // 自社マスタより情報を取得
                var company = new ComDao.CompanyEntity().GetEntity(APConsts.COMPANY.COMPANY_CD, db);
                if (company == null)
                {
                    // 取得できなければエラー
                    this.IsError = true;
                    return;
                }
                // サーバURLかポート番号がNullかどうか判定
                if (string.IsNullOrEmpty(company.MailSendServer) || company.MailSendServerPort == null)
                {
                    // いずれかがNullならエラー
                    this.IsError = true;
                    return;
                }

                // 正常な場合、値を設定
                this.Url = company.MailSendServer;
                this.Port = company.MailSendServerPort ?? -1;   // ポート番号がNullはチェックしたのであり得ない
                this.AuthUser = company.MailSendServerUser;
                this.AuthPassword = company.MailSendServerPassword;

                // エラーなし
                this.IsError = false;
            }
        }

        /// <summary>
        /// メール送信情報取得
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <returns>メール送信情報</returns>
        public static MailSendServerInfo GetMailSendServerInfo(ComDB db)
        {
            MailSendServerInfo info = new MailSendServerInfo(db);
            return info;
        }
    }
}
