///<summary>
/// 機能名　　：　【共通】
/// タイトル　：　業務ロジックデータクラス
/// 説明　　　：　業務ロジックに使用する情報を格納するデータ型を定義します。
/// 
/// 履歴　　　：　2017.08.01 河村純子　新規作成
///</summary>

using System.Collections.Generic;
using System.Web;
using System.ComponentModel.DataAnnotations;

using CommonWebTemplate.CommonUtil;
using Microsoft.AspNetCore.Http;
using CommonWebTemplate.CommonDefinitions;

namespace CommonWebTemplate.Models.Common
{
    public class CommonProcData
    {
        /// <summary>
        /// 端末番号(IPｱﾄﾞﾚｽ)
        /// </summary>
        public string TerminalNo { get; set; }

        //== ｾｯｼｮﾝ[CIM_USER_INFO]：ﾕｰｻﾞｰ情報から取得(ﾃﾞｰﾀｸﾗｽ：UserInfoDef) ==
        /// <summary>
        /// ﾛｸﾞｲﾝﾕｰｻﾞｰID
        /// </summary>
        public string LoginUserId { get; set; }
        /// <summary>
        /// ﾛｸﾞｲﾝID
        /// </summary>
        public string LoginId { get; set; }
        /// <summary>
        /// ﾛｸﾞｲﾝﾕｰｻﾞｰ表示名
        /// </summary>
        public string LoginUserName { get; set; }
        /// <summary>
        /// 言語ID
        /// </summary>
        public string LanguageId { get; set; }
        /// <summary>ユーザー権限レベルID</summary>
        public int AuthorityLevelId { get; set; }
        /// <summary>
        /// ユーザー機能権限ﾘｽﾄ
        /// ※ﾕｰｻﾞｰに権限のある機能が対象
        /// </summary>
        public IList<CommonConductMst> UserAuthConducts { get; set; }
        /// <summary>
        /// GUID
        /// </summary>
        public string GUID { get; set; }
        //== (ここまで) ==
        /// <summary>
        /// GET:機能ID
        /// </summary>
        [Required(ErrorMessage = "機能IDが必要です")]
        public string ConductId { get; set; }

        /// <summary>
        /// GET:機能処理ﾊﾟﾀｰﾝ
        /// </summary>
        public int ConductPtn { get; set; }

        /// <summary>
        /// GET:ﾌﾟﾛｸﾞﾗﾑID
        /// </summary>
        public string PgmId { get; set; }

        /// <summary>
        /// GET:画面NO
        /// </summary>
        [Required(ErrorMessage = "画面NOが必要です")]
        public short FormNo { get; set; }

        /// <summary>
        /// GET:ｺﾝﾄﾛｰﾙID
        /// </summary>
        public string CtrlId { get; set; }

        /// <summary>
        /// GET:ｱｸｼｮﾝ区分
        /// </summary>
        public short ActionKbn { get; set; }

        /// <summary>
        /// GET:画面遷移ｱｸｼｮﾝ区分
        /// </summary>
        public short TransActionDiv { get; set; }

        /// <summary>
        /// GET:ﾍﾟｰｼﾞ番号
        /// </summary>
        public int PageNo { get; set; }
        /// <summary>
        /// GET:1ﾍﾟｰｼﾞに表示するの行数
        /// </summary>
        public int PageCount { get; set; }
        /// <summary>
        /// GET:一覧の画面定義条件
        /// </summary>
        public List<Dictionary<string, object>> ListDefines { get; set; } = null;
        /// <summary>
        /// GET:画面のボタン定義
        /// </summary>
        public List<Dictionary<string, object>> ButtonDefines { get; set; } = null;

        /// <summary>
        /// GET:確認ﾒｯｾｰｼﾞ番号(※ﾌﾟﾛｼｰｼﾞｬ処理から返却された確認ﾒｯｾｰｼﾞの確認結果がOKの場合に受け取る)
        /// </summary>
        public int ConfirmNo { get; set; }

        /// <summary>
        /// GET:条件ﾃﾞｰﾀ
        /// </summary>
        public List<Dictionary<string, object>> ConditionData { get; set; }

        /// <summary>
        /// GET:一覧ﾃﾞｰﾀ(明細)
        /// </summary>
        public List<Dictionary<string, object>> ListData { get; set; }
        /// <summary>
        /// GET:一覧ｺﾝﾎﾞﾎﾞｯｸｽ翻訳ﾃﾞｰﾀ(明細)
        /// </summary>
        public List<Dictionary<string, object>> ListHonyaku { get; set; }

        /// <summary>
        /// GET:個別実装用汎用ﾃﾞｰﾀ
        /// </summary>
        public Dictionary<string, object> ListIndividual { get; set; }

        /// <summary>
        /// GET:検索条件項目名リスト
        /// </summary>
        public List<string> ListConditionName { get; set; } = null;
        /// <summary>
        /// GET:検索条件設定値リスト
        /// </summary>
        public List<string> ListConditionValue { get; set; } = null;

        /// <summary>
        /// GET:取込ファイル情報
        /// </summary>
        public IFormFile[] UploadFile { get; set; }

        /// <summary>
        /// GET:ブラウザタブ識別番号
        /// </summary>
        public string BrowserTabNo { get; set; }

        /// <summary>
        /// ﾌｧｲﾙﾀﾞｳﾝﾛｰﾄﾞ非同期設定(※ﾌｧｲﾙ出力用)
        /// </summary>
        public int FileDownloadSet { get; set; }

        /// <summary>
        /// 基底URL(※帳票出力用)
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// サーバルート物理パス(※帳票出力用)
        /// </summary>
        public string ServerRootPath { get; set; }

        /// <summary>
        /// 所属情報
        /// </summary>
        public BelongingInfo BelongingInfo { get; set; }

        /// <summary>
        /// 工場IDリスト
        /// </summary>
        public List<int> FactoryIdList { get; set; }

        /// <summary>
        /// 構成グループリスト
        /// </summary>
        public List<int> StructureGroupList { get; set; }

        /// <summary>
        /// 場所階層構成IDリスト
        /// </summary>
        public List<int> LocationIdList { get; set; }

        /// <summary>
        /// 職種機種構成IDリスト
        /// </summary>
        public List<int> JobIdList { get; set; }

        /// <summary>
        /// ファイルダウンロード情報
        /// </summary>
        public string FileDownloadInfo { get; set; }

        /// <summary>
        /// 言語コンボボックスで選択した言語ID
        /// </summary>
        public string SelectLanguageId { get; set; }

        /// <summary>
        /// 翻訳に使用している工場ID
        /// </summary>
        public int TransFactoryId { get; set; }

        #region === コンストラクタ ===
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommonProcData()
        {
            //プロパティを初期化
            this.ActionKbn = LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.None;  //(未指定)※初期化用
        }
        #endregion

        #region === public 処理 ===
        /// <summary>
        /// ﾌｧｲﾙﾀﾞｳﾝﾛｰﾄﾞ非同期判定
        /// </summary>
        /// <returns>true:非同期, false:同期</returns>
        public bool IsFileDownloadHidouki()
        {
            if (AppConstants.FileDownloadSet.Hidouki.GetHashCode().Equals(FileDownloadSet))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// ﾃﾞｰﾀ管理ｷｰ項目をｺﾋﾟｰする
        /// </summary>
        public static CommonProcData CopyKey(CommonProcData baseSource)
        {
            CommonProcData copySource = new CommonProcData();

            copySource.TerminalNo = baseSource.TerminalNo;
            copySource.LoginUserId = baseSource.LoginUserId;
            copySource.LanguageId = baseSource.LanguageId;
            copySource.ConductId = baseSource.ConductId;
            copySource.PgmId = baseSource.PgmId;
            copySource.FormNo = baseSource.FormNo;

            copySource.PageNo = baseSource.PageNo;
            copySource.PageCount = baseSource.PageCount;

            copySource.ConfirmNo = baseSource.ConfirmNo;

            copySource.GUID = baseSource.GUID;
            copySource.BrowserTabNo = baseSource.BrowserTabNo;
            copySource.AuthorityLevelId = baseSource.AuthorityLevelId;
            copySource.BelongingInfo = baseSource.BelongingInfo;

            return copySource;

        }
        #endregion
    }
}