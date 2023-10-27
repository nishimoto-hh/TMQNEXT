using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using Dao = CommonTMQUtil.CommonTMQUtilDataClass;
using STDDao = CommonSTDUtil.CommonSTDUtil.CommonSTDUtillDataClass;
//using ComDao = CommonSTDUtil.CommonDataBaseClass;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;

namespace TMQSendMailHistory
{
    class TMQSendMailHistoryDataClass
    {
        public class MailSendServerInfo : ComDao.CommonTableItem
        {
            /// <summary>
            /// 送信元メールサーバURL
            /// </summary>
            public string MailSendServerURL { get; set; }
            /// <summary>
            /// 送信元メールサーバポート
            /// </summary>
            public string MailSendServerPort { get; set; }
            /// <summary>
            /// 送信元メールユーザ名
            /// </summary>
            public string MailSendServerUser { get; set; }
            /// <summary>
            /// 送信元メールユーザパスワード
            /// </summary>
            public string MailSendServerPassword { get; set; }
            /// <summary>
            /// 件名
            /// </summary>
            public string MailTitle { get; set; }
            /// <summary>
            /// 本文
            /// </summary>
            public string MailBody { get; set; }
            /// <summary>
            /// 対象取得条件(日数)
            /// </summary>
            public string Condition { get; set; }
            /// <summary>
            /// 送信元メールアドレス
            /// </summary>
            public string MailSendServerAddress { get; set; }
            /// <summary>
            /// 対象対象除外条件(日数)
            /// </summary>
            public string ExcludeCondition { get; set; }
        }

        /// <summary>
        /// メールテンプレートマスタ
        /// </summary>
        public class MailTemplateEntity : ComDao.CommonTableItem
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public MailTemplateEntity()
            {
                TableName = "mail_template";
            }

            /// <summary>Gets テーブル名</summary>
            /// <value>テーブル名</value>
            public string TableName { get; }
            /// <summary>Gets or sets メールフォーマットID</summary>
            /// <value>メールフォーマットID</value>
            public long MailFormatId { get; set; }
            /// <summary>Gets or sets 説明</summary>
            /// <value>説明</value>
            public string Description { get; set; }
            /// <summary>Gets or sets 件名</summary>
            /// <value>件名</value>
            public string TextTitle { get; set; }
            /// <summary>Gets or sets 本文</summary>
            /// <value>本文</value>
            public string TextBody { get; set; }

            /// <summary>
            /// プライマリーキー
            /// </summary>
            public class PrimaryKey
            {
                /// <summary>Gets or sets メールフォーマットID</summary>
                /// <value>メールフォーマットID</value>
                public long MailFormatId { get; set; }
                /// <summary>Gets or sets 言語コード</summary>
                /// <value>言語コード</value>
                public string LanguageId { get; set; }
                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="mailFormatId">メールフォーマットID</param>
                /// <param name="languageId">言語コード</param>
                public PrimaryKey(long mailFormatId, string languageId)
                {
                    MailFormatId = mailFormatId;
                    LanguageId = languageId;
                }
            }

            /// <summary>
            /// プライマリーキー情報
            /// </summary>
            /// <returns>主キー情報</returns>
            public PrimaryKey PK()
            {
                PrimaryKey pk = new PrimaryKey(this.MailFormatId, this.LanguageId);
                return pk;
            }

            ///// <summary>
            ///// エンティティ
            ///// </summary>
            ///// <param name="mailFormatId">メールフォーマットID</param>
            ///// <param name="languageId">言語コード</param>
            ///// <param name="db">DB操作クラス</param>
            ///// <returns>該当のデータを返す</returns>
            //public MailTemplateEntity GetEntity(long mailFormatId, string languageId, ComDB db)
            //{
            //    MailTemplateEntity.PrimaryKey condition = new MailTemplateEntity.PrimaryKey(mailFormatId, languageId);
            //    return GetEntity<MailTemplateEntity>(this.TableName, condition, db);
            //}
        }

        public class SendMailHistorySerachTime
        {
            /// <summary>
            /// 申請者宛送信対象取得条件(分)
            /// </summary>
            public long SerachTime { get; set; }
        }
        public class SendMailHistoryApplication
        {
            /// <summary>
            /// 申請者ID
            /// </summary>
            public int application_user_id { get; set; }
            ///// <summary>
            /// 差戻中件数
            /// </summary>
            public long ReturnCnt { get; set; }
            /// <summary>
            /// 承認済件数
            /// </summary>
            public long ApprovedCnt { get; set; }
            /// <summary>
            /// メールアドレス
            /// </summary>
            public string MailAddress { get; set; }
            /// <summary>
            /// メール件名
            /// </summary>
            public string LanguageId { get; set; }
            /// <summary>
            /// メール本文
            /// </summary>
            public string MailMessage { get; set; }
        }
        public class SendMailHistoryApproval
        {
            /// <summary>
            /// 承認者ID
            /// </summary>
            public int user_id { get; set; }
            /// <summary>
            /// 承認者メールアドレス
            /// </summary>
            public string MailAddress { get; set; }
            /// <summary>
            /// 言語ID
            /// </summary>
            public string LanguageId { get; set; }
            /// <summary>
            /// 承認依頼中件数
            /// </summary>
            public long RequestCnt { get; set; }
        }
    }
}
