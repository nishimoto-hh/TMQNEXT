using CommonWebTemplate.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
//using Ionic.Zip;
//using Ionic.Zlib;
using System.Dynamic;
using Dao = CommonSTDUtil.CommonSTDUtil.CommonSTDUtillDataClass;
using ComDB = CommonSTDUtil.CommonDBManager.CommonDBManager;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Configuration;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using CommonSTDUtil.Properties;
using CommonWebTemplate.CommonDefinitions;
using Password = CommonWebTemplate.PasswordEncrypt;
using System.Security.Cryptography;

namespace CommonSTDUtil.CommonSTDUtil
{
    public class CommonSTDUtil
    {
        #region 定数
        /// <summary>オートコンプリート_SQLID</summary>
        public const string AutoComplete = "A";
        /// <summary>コンボボックス_SQLID</summary>
        public const string CombBox = "C";
        /// <summary>オートコンプリートフラグ</summary>
        /// <remarks>0:前方一致、1:後方一致、2:部分一致、3:完全一致</remarks>
        public const int AutoComplteFlg = 0;
        /// <summary>FromTo分割文字</summary>
        public const char FromToDelimiter = '|';
        /// <summary>数値+単位分割文字</summary>
        public const char NumberUnitDelimiter = '@';
        /// <summary>権限チェック用分割文字</summary>
        public const char AuthorityDelimiter = '|';
        /// <summary>テンポラリフォルダパス格納グループID</summary>
        public const int StructureGroupId = 9200;

        /// <summary>排他ロックデータ用キー名</summary>
        public const string LockDataKeyName = "lockData";

        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            /// <summary>ログイン認証用SQL</summary>
            public const string GetUserInfo = "UserInfo_GetCount";
            /// <summary>パスワード変更用SQL</summary>
            public const string UpdateUserInfo = "UserInfo_Update";
            /// <summary>DBカラム情報用SQL</summary>
            public const string GetTranslation = "Translation_GetList";
            /// <summary>ユーザ権限チェック取得用SQL</summary>
            public const string CheckBtnAuthority = "CheckBtnAuthority";
            /// <summary>ユーザ権限チェック取得用SQL ※バッチ用</summary>
            public const string CheckBtnAuthorityForBatch = "CheckBtnAuthorityForBatch";
            /// <summary>ボタン権限情報取得用SQL</summary>
            public const string GetBtnAuthority = "BtnAuthInfo_GetList";
            /// <summary>一覧行編集情報取得用SQL</summary>
            public const string GetTblRowEditInfo = "TblRowEditInfo_GetList";
            /// <summary>認証有無チェック用SQL</summary>
            public const string CheckConductAuthFlg = "CheckConductAuthFlg";
            /// <summary>階層マスタ取得</summary>
            public const string StructureGetList = "Structure_GetList";
            /// <summary>階層マスタ取得(指定構成IDの上位、下位の階層すべてを取得)</summary>
            public const string StructureGetListAll = "Structure_GetAllList";
            /// <summary>1ページ当たりの行数コンボの取得</summary>
            public const string GetComboRowsPerPage = "GetComboRowsPerPage";
            /// <summary>一覧項目カスタマイズ情報削除</summary>
            public const string DeleteItemCustomizeInfoList = "ItemCustomizeInfoList_Delete";
            /// <summary>一覧項目カスタマイズ情報保存</summary>
            public const string SaveItemCustomizeInfoList = "ItemCustomizeInfoList_Insert";
            /// <summary>言語コンボの取得</summary>
            public const string GetLanguageList = "GetLanguageList";
            /// <summary>ユーザ権限機能マスタから権限情報の取得</summary>
            public const string UserAuthGetListForLogin = "UserAuth_GetListForLogin";
            /// <summary>ユーザ権限機能マスタから権限有無の取得</summary>
            public const string UserAuthGetCount = "UserAuth_GetCount";
            /// <summary>テンポラリフォルダパスの取得</summary>
            public const string GetTemporaryFolderPath = "GetTemporaryFolderPath";
            /// <summary>画像ファイル情報取得</summary>
            public const string GetImageFileInfo = "GetImageFileInfo";
            /// <summary>予備品の工場ID取得</summary>
            /// <remarks>コンボのSQL</remarks>
            public const string GetPartsFactoryId = "C0018";
            /// <summary>工場と職種取得</summary>
            public const string GetFactoryAndJob = "Structure_GetFactoryAndJob";
            /// <summary>ユーザマスタからユーザ情報の取得</summary>
            public const string UserMstGetUserInfo = "UserMst_GetUserInfo";

            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = "Common";
            /// <summary>SQL格納先サブディレクトリ名(オートコンプリート)</summary>
            public const string SubDirAutoComplete = SubDir + "\\AutoComplete";
            /// <summary>SQL格納先サブディレクトリ名(コンボボックス)</summary>
            public const string SubDirComboBox = SubDir + "\\ComboBox";

            /// <summary>SQL名：登録</summary>
            public const string Batch_Get_Status = "Com_bat_sch_GetStatus";
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string Batch_SubDir = "Batch\\Com_bat_sch";
        }

        /// <summary>
        /// バッチ処理用定義
        /// </summary>
        private static class BatchParam
        {
            /// <summary>処理結果：実行中</summary>
            public const string Batch_Def_Run = "batStatusRun";
            /// <summary>処理結果：正常終了</summary>
            public const string Batch_Def_Valid = "batStatusValid";
            /// <summary>処理結果：異常終了</summary>
            public const string Batch_Def_Invalid = "batStatusInValid";

            /// <summary>ステータス：要求</summary>
            public const int Batch_Status_Request = 0;
            /// <summary>ステータス：終了</summary>
            public const int Batch_Status_Finish = 9;
        }

        /// <summary>
        /// ファイル拡張子
        /// </summary>
        public static class FileExtension
        {
            /// <summary>Excel</summary>
            public const string Excel = ".xlsx";
            /// <summary>Excelマクロ</summary>
            public const string ExcelMacro = ".xlsm";
            /// <summary>CSV</summary>
            public const string CSV = ".csv";
            /// <summary>TSV</summary>
            public const string TSV = ".tsv";
        }


        /// <summary>
        /// 文字を定義した定数クラス
        /// </summary>
        public static class CharacterConsts
        {
            /// <summary>
            /// カンマ
            /// </summary>
            public const char Comma = ',';

            /// <summary>
            /// タブ
            /// </summary>
            public const char Tab = '\t';
        }

        /// <summary>
        /// 認証フラグ
        /// </summary>
        public static class AuthFlg
        {
            /// <summary>0:認証不要</summary>
            public static int NotNeed = 0;
            /// <summary>1:認証要</summary>
            public static int Need = 1;
        }

        /// <summary>ログ出力</summary>
        private static CommonLogger.CommonLogger logger = CommonLogger.CommonLogger.GetInstance("logger");
        #endregion

        #region コンストラクタ
        public CommonSTDUtil()
        {
        }
        #endregion

        // 権限一覧(設定ファイル等が決まれば移動する)
        public static readonly Dictionary<int, String> CTRL_ID = new Dictionary<int, String>() {
                                { 0, "VIEW"},               // 閲覧
                                { 1, "APPROVAL"},           // 承認
                                { 2, "APPROVAL_REQUEST"},   // 承認依頼
                                { 5, "REGIST"}              // 登録
        };

        /// <summary>
        /// 権限チェック
        /// </summary>
        /// <param name="iTantoCd">担当者CD</param>
        /// <param name="iMenuId">メニューID</param>
        /// <param name="iTabId">タブID</param>
        /// <param name="oAuthList">権限リスト(JSON文字列)</param>
        /// <returns>bool true:OK, false:NG</returns>
        public static bool GetControlAuthority(ComDB db, string factoryId, string iTantoCd, int iMenuId, int iTabId, ref string oAuthList)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            };
            try
            {
                var authList = new Dictionary<string, string>();

                // DB接続
                if (!db.Connect())
                {
                    return false;
                }

                // ログイン者情報取得(部署、役職)
                dynamic resultLogin = db.GetEntityByOutsideSql("GET_LOGIN_INFO", db.SqlRootPath + "\\Auth", new { tantoCd = iTantoCd });

                // 各機能について権限の有無を判定
                foreach (var ctrlId in CTRL_ID)
                {
                    dynamic resultAuth = db.GetEntityByOutsideSql("GET_CONTROL_AUTH", db.SqlRootPath + "\\Auth",
                                            new { tantoCd = iTantoCd, organizationCd = resultLogin.organization_cd, postCd = resultLogin.post_id, menuId = iMenuId, tabId = iTabId, controlId = ctrlId.Key });

                    if (resultAuth == null)
                    {
                        authList.Add(ctrlId.Key.ToString(), "0");    // 権限なし

                    }
                    else
                    {
                        authList.Add(ctrlId.Key.ToString(), "1");    // 権限有り
                    }
                }

                // 結果をJSON文字列として返却
                oAuthList = JsonSerializer.Serialize(authList, jsonOptions);
            }
            catch (Exception ex)
            {
                logger.ErrorLog(factoryId, iTantoCd.ToString(), "", ex);

                // 閲覧権限なしで返却
                var authList = new Dictionary<string, string>();
                authList.Add("0", "0");
                oAuthList = JsonSerializer.Serialize(authList, jsonOptions);

                return false;
            }

            return true;
        }

        /// <summary>
        /// CSVファイル出力
        /// </summary>
        /// <param name="iList">出力対象データリスト</param>
        /// <param name="encoding">文字エンコード</param>
        /// <param name="oFileStream">出力Stream</param>
        /// <param name="oErrMsg">エラーメッセージ</param>
        /// <returns></returns>
        public static bool ExportCsvFile(List<object[]> iList, Encoding encoding, out Stream oFileStream, out string oErrMsg)
        {
            oFileStream = new MemoryStream();
            oErrMsg = string.Empty;

            try
            {
                StreamWriter sw = new StreamWriter(oFileStream, encoding);
                foreach (var line in iList)
                {
                    var sb = new StringBuilder();
                    foreach (var cell in line)
                    {
                        if (cell != null)
                        {
                            var value = cell.ToString();

                            if (value.Contains(sw.NewLine) ||
                                value.Contains(",") ||
                                value.Contains("\""))
                            {
                                // 改行コード、カンマ、ダブルクォーテーションはダブルクォーテーションで囲む
                                // ダブルクォーテーションは重ねる
                                value = value.Replace("\"", "\"\"");
                                sb.Append("\"");
                                sb.Append(value);
                                sb.Append("\"");
                            }
                            else
                            {
                                sb.Append(value);
                            }
                        }
                        sb.Append(",");
                    }
                    sb.Remove(sb.Length - 1, 1);

                    sw.WriteLine(sb.ToString());
                }
                sw.Flush();
                oFileStream.Seek(0, SeekOrigin.Begin);
            }
            catch (Exception ex)
            {
                // 出力時に例外発生
                oErrMsg = ex.ToString();
                if (oFileStream != null)
                {
                    oFileStream.Close();
                    oFileStream = null;
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// CSVファイル出力(値をダブルクォーテーションで囲まない)
        /// </summary>
        /// <param name="iList">出力対象データリスト</param>
        /// <param name="encoding">文字エンコード</param>
        /// <param name="oFileStream">出力Stream</param>
        /// <param name="oErrMsg">エラーメッセージ</param>
        /// <returns></returns>
        public static bool ExportCsvFileNotencircleDobleQuotes(List<object[]> iList, Encoding encoding, out Stream oFileStream, out string oErrMsg)
        {
            oFileStream = new MemoryStream();
            oErrMsg = string.Empty;

            try
            {
                StreamWriter sw = new StreamWriter(oFileStream, encoding);
                foreach (var line in iList)
                {
                    var sb = new StringBuilder();
                    foreach (var cell in line)
                    {
                        if (cell != null)
                        {
                            var value = cell.ToString();

                            if (value.Contains(sw.NewLine) ||
                                value.Contains(","))
                            {
                                // 改行コード、カンマはダブルクォーテーションで囲む
                                value = value.Replace("\"", "\"\"");
                                sb.Append("\"");
                                sb.Append(value);
                                sb.Append("\"");
                            }
                            else
                            {
                                sb.Append(value);
                            }
                        }
                        sb.Append(",");
                    }
                    sb.Remove(sb.Length - 1, 1);

                    sw.WriteLine(sb.ToString());
                }
                sw.Flush();
                oFileStream.Seek(0, SeekOrigin.Begin);
            }
            catch (Exception ex)
            {
                // 出力時に例外発生
                oErrMsg = ex.ToString();
                if (oFileStream != null)
                {
                    oFileStream.Close();
                    oFileStream = null;
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// CSVファイル取込
        /// </summary>
        /// <param name="iUploadFiles">取込ファイル(Stream) 複数指定に対応</param>
        /// <param name="iHeaderFlg">見出しフラグ(TRUEなら1行目を見出し扱いとする)</param>
        /// <param name="enc">文字コード</param>
        /// <param name="oErrMsg">エラーメッセージ</param>
        /// <param name="oList">CSVファイル取込結果リスト</param>
        /// <returns>実行結果(true:OK, false:NG)</returns>
        public static bool ImportCsvFiles(List<Stream> iUploadFiles, bool iHeaderFlg, Encoding enc,
            ref List<List<Dictionary<string, object>>> oList, ref string oErrMsg)
        {
            oList = new List<List<Dictionary<string, object>>>();
            oErrMsg = string.Empty;

            try
            {
                string lines = string.Empty;
                string csvLine = string.Empty;

                // 取込ファイル数分ループiUploadFile
                foreach (var uploadFile in iUploadFiles)
                {
                    using (var memStream = new MemoryStream())
                    {
                        // 取込ファイルのバイト配列をコピー
                        uploadFile.CopyTo(memStream);
                        byte[] bytes = memStream.ToArray();
                        if (bytes == null || bytes.Length <= 0)
                        {
                            // 取込ファイルにデータなし
                            // 空のリストを挿入して次のファイルへ
                            // エラーかどうかは呼び元で判断する
                            oList.Add(new List<Dictionary<string, object>>());
                            continue;
                        }

                        // ■文字コードの規定は？
                        //(stt)180411 atts UPD 4/10週例会決定事項　文字コードはSJIS固定
                        ////文字コードを判定
                        //System.Text.Encoding enc = FileUtil.DecisionEncoding(bytes);
                        //if (enc == null)
                        //{
                        //    //判定不能の場合はデフォルト設定
                        //    enc = System.Text.Encoding.GetEncoding("Shift_JIS");   //デフォルト：Shift_JIS
                        //}

                        //// 文字コード：Shift_JIS
                        //System.Text.Encoding enc = System.Text.Encoding.GetEncoding("Shift_JIS");
                        ////(end)180411 stts UPD

                        // 全行取り出し
                        lines = enc.GetString(bytes);
                    }

                    List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
                    Dictionary<string, object> dic2 = new Dictionary<string, object>();

                    // 1行ずつ取り出し
                    long rowNo = 0;
                    foreach (string line in lines.Split(new string[] { "\r\n" }, StringSplitOptions.None))
                    {
                        rowNo++;

                        // １行目が見出しの場合はスキップ
                        if (iHeaderFlg && rowNo == 1) continue;

                        // CSVファイルデータを列項目ごとに分割
                        //※""で囲まれたカンマあり項目を考慮
                        string[] vals = SplitCSV(line, CharacterConsts.Comma);

                        dic2 = new Dictionary<string, object>();

                        // 値をセット
                        for (int colNo = 1; colNo <= vals.Length; colNo++)
                        {
                            string colName = "VAL" + colNo;
                            dic2.Add(colName, vals[colNo - 1].ToString());      // 要素単位で追加
                        }
                        list.Add(dic2);     // 行単位で追加
                    }
                    oList.Add(list);   // ファイル単位で追加
                }
            }
            catch (Exception ex)
            {
                // 取込時に例外発生
                oErrMsg = ex.ToString();
                return false;
            }
            finally
            {
                foreach (var uploadFile in iUploadFiles)
                {
                    uploadFile.Close();
                }
            }

            return true;
        }
        /// <summary>
        /// TSVファイルリスト取込
        /// </summary>
        /// <param name="iUploadFiles">取込ファイル(Stream) 複数指定に対応</param>
        /// <param name="iHeaderFlg">見出しフラグ(TRUEなら1行目を見出し扱いとする)</param>
        /// <param name="encoding">文字コード</param>
        /// <param name="oErrMsg">エラーメッセージ</param>
        /// <param name="oList">CSVファイル取込結果リスト</param>
        /// <returns>実行結果(true:OK, false:NG)</returns>
        public static bool ImportTsvFiles(List<Stream> iUploadFiles, bool iHeaderFlg, Encoding encoding,
            ref List<string[,]> oList, ref string oErrMsg)
        {
            // 区切り文字にタブを指定
            return ImportFilesWithDelimiter(iUploadFiles, iHeaderFlg, encoding, ref oList, ref oErrMsg, CharacterConsts.Tab);
        }

        /// <summary>
        /// CSVファイルリスト取込
        /// </summary>
        /// <param name="iUploadFiles">取込ファイル(Stream) 複数指定に対応</param>
        /// <param name="iHeaderFlg">見出しフラグ(TRUEなら1行目を見出し扱いとする)</param>
        /// <param name="encoding">文字コード</param>
        /// <param name="oErrMsg">エラーメッセージ</param>
        /// <param name="oList">CSVファイル取込結果リスト</param>
        /// <returns>実行結果(true:OK, false:NG)</returns>
        public static bool ImportCsvFiles(List<Stream> iUploadFiles, bool iHeaderFlg, Encoding encoding,
            ref List<string[,]> oList, ref string oErrMsg)
        {
            // 区切り文字にカンマを指定
            return ImportFilesWithDelimiter(iUploadFiles, iHeaderFlg, encoding, ref oList, ref oErrMsg, CharacterConsts.Comma);
        }

        /// <summary>
        /// デリミタを指定したテキストファイルリスト取込
        /// </summary>
        /// <param name="iUploadFiles">取込ファイル(Stream) 複数指定に対応</param>
        /// <param name="iHeaderFlg">見出しフラグ(TRUEなら1行目を見出し扱いとする)</param>
        /// <param name="encoding">文字コード</param>
        /// <param name="oErrMsg">エラーメッセージ</param>
        /// <param name="oList">CSVファイル取込結果リスト</param>
        /// <param name="delimiter">区切り文字</param>
        /// <returns>実行結果(true:OK, false:NG)</returns>
        private static bool ImportFilesWithDelimiter(List<Stream> iUploadFiles, bool iHeaderFlg, Encoding encoding,
            ref List<string[,]> oList, ref string oErrMsg, char delimiter)
        {
            oList = new List<string[,]>();
            oErrMsg = string.Empty;

            try
            {
                // 取込ファイル数分ループiUploadFile
                foreach (var uploadFile in iUploadFiles)
                {
                    string[,] result = null;
                    if (!ImportFileWithDelimiter(uploadFile, iHeaderFlg, encoding, ref result, ref oErrMsg, delimiter))
                    {
                        return false;
                    }
                    oList.Add(result);
                }
            }
            catch (Exception ex)
            {
                // 取込時に例外発生
                oErrMsg = ex.ToString();
                return false;
            }
            finally
            {
                foreach (var uploadFile in iUploadFiles)
                {
                    if (uploadFile != null)
                    {
                        uploadFile.Close();
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// TSVファイル取込
        /// </summary>
        /// <param name="iUploadFiles">取込ファイル(Stream) 複数指定に対応</param>
        /// <param name="iHeaderFlg">見出しフラグ(TRUEなら1行目を見出し扱いとする)</param>
        /// <param name="encoding">文字コード</param>
        /// <param name="oErrMsg">エラーメッセージ</param>
        /// <param name="oList">CSVファイル取込結果リスト</param>
        /// <returns>実行結果(true:OK, false:NG)</returns>
        public static bool ImportTsvFile(Stream iUploadFile, bool iHeaderFlg, Encoding encoding,
            ref string[,] result, ref string oErrMsg)
        {
            // 区切り文字にタブを指定
            return ImportFileWithDelimiter(iUploadFile, iHeaderFlg, encoding, ref result, ref oErrMsg, CharacterConsts.Tab);
        }

        /// <summary>
        /// CSVファイル取込
        /// </summary>
        /// <param name="iUploadFiles">取込ファイル(Stream) 複数指定に対応</param>
        /// <param name="iHeaderFlg">見出しフラグ(TRUEなら1行目を見出し扱いとする)</param>
        /// <param name="encoding">文字コード</param>
        /// <param name="oErrMsg">エラーメッセージ</param>
        /// <param name="oList">CSVファイル取込結果リスト</param>
        /// <returns>実行結果(true:OK, false:NG)</returns>
        public static bool ImportCsvFile(Stream iUploadFile, bool iHeaderFlg, Encoding encoding,
            ref string[,] result, ref string oErrMsg)
        {
            // 区切り文字にカンマを指定
            return ImportFileWithDelimiter(iUploadFile, iHeaderFlg, encoding, ref result, ref oErrMsg, CharacterConsts.Comma);
        }

        /// <summary>
        /// デリミタを指定したテキストファイル取込
        /// </summary>
        /// <param name="iUploadFile">取込ファイル(Stream)</param>
        /// <param name="iHeaderFlg">見出しフラグ(TRUEなら1行目を見出し扱いとする)</param>
        /// <param name="encoding">文字コード</param>
        /// <param name="result">取込結果</param>
        /// <param name="oErrMsg">エラーメッセージ</param>
        /// <param name="delimiter">区切り文字</param>
        /// <returns>実行結果(true:OK, false:NG)</returns>
        private static bool ImportFileWithDelimiter(Stream iUploadFile, bool iHeaderFlg, Encoding encoding,
            ref string[,] result, ref string oErrMsg, char delimiter)
        {
            result = null;
            oErrMsg = string.Empty;

            try
            {
                string fileTexts = string.Empty;
                using (var memStream = new MemoryStream())
                {
                    // 取込ファイルのバイト配列をコピー
                    iUploadFile.CopyTo(memStream);
                    byte[] bytes = memStream.ToArray();
                    if (bytes == null || bytes.Length <= 0)
                    {
                        // 取込ファイルにデータなし
                        // 空の配列を返却する
                        // エラーかどうかは呼び元で判断する
                        result = new string[0, 0];
                        return true;
                    }

                    // 全行取り出し
                    fileTexts = encoding.GetString(bytes);
                }

                // 改行コードで分割
                string[] lines = fileTexts.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                // 1行目をデリミタで分割し、列数を取得
                string line = lines[0];
                string[] vals = SplitCSV(line, delimiter);
                int rowCnt = iHeaderFlg ? lines.Length - 1 : lines.Length;
                int colCnt = vals.Length;
                int rowNo = 0;

                // 空行を取込結果より除くため、空行の数を調べて行数より引く
                int emptyRowCount = lines.Where(x => string.IsNullOrEmpty(x)).Count();
                rowCnt = rowCnt - emptyRowCount;

                result = new string[rowCnt, colCnt];

                for (int i = 0; i < lines.Length; i++)
                {
                    if (i == 0)
                    {
                        // 1行目がヘッダー行の場合はスキップ
                        if (iHeaderFlg) continue;
                    }
                    else
                    {
                        line = lines[i];
                        // 空行はスキップ
                        if (string.IsNullOrEmpty(line)) { continue; }

                        // CSVファイルデータを列項目ごとに分割
                        //※""で囲まれたカンマあり項目を考慮
                        vals = SplitCSV(line, delimiter);
                    }

                    // 値をセット
                    for (int colNo = 0; colNo < colCnt && colNo < vals.Length; colNo++)
                    {
                        result[rowNo, colNo] = vals[colNo];
                    }
                    rowNo++;
                }
            }
            catch (Exception ex)
            {
                // 取込時に例外発生
                oErrMsg = ex.ToString();
                return false;
            }
            finally
            {
                iUploadFile.Close();
                iUploadFile = null;
            }
            return true;
        }

        /// <summary>
        /// CSVファイルデータを列項目ごとに分割
        /// </summary>
        /// <param name="line">1行データ</param>
        /// <param name="delimiter">区切り文字</param>
        /// <remarks>※""で囲まれたカンマあり項目を考慮</remarks>
        public static string[] SplitCSV(string line, char delimiter)
        {
            // コメント文はイメージしにくいので、デリミタをカンマとして記載しています

            const string dQuoteStr = @"""";     //「"」
            const string dQuote2Str = @"""""";  //「""」

            List<string> itemList = new List<string>();

            StringBuilder sb = new StringBuilder();
            sb.Append(delimiter);
            sb.Append(@"""");
            //先に""で囲まれたカンマあり項目を対象に分割
            string[] separator1 = { sb.ToString() };     //「,"」
            sb.Clear();
            sb.Append(@"""");
            sb.Append(delimiter);
            string[] separator2 = { sb.ToString() };     //「",」

            //「,"」で分割
            string[] dQuoteVals = line.Split(separator1, StringSplitOptions.None);
            if (dQuoteVals.Length > 0)
            {
                for (int ii = 0; ii < dQuoteVals.Length; ii++)
                {
                    string dQuoteVal = dQuoteVals[ii];

                    if (ii == 0 && !dQuoteVal.StartsWith(dQuoteStr))
                    {
                        //※先頭項目が「"」始まりでない場合、カンマ区切りで取得
                        itemList.AddRange(dQuoteVal.Split(delimiter));
                    }
                    else
                    {
                        //「",」で分割
                        string[] dQuoteVals2 = dQuoteVal.Split(separator2, StringSplitOptions.None);
                        if (dQuoteVals2.Length <= 0) continue;

                        //先頭項目はそのまま取得
                        string item = dQuoteVals2[0];
                        //※先頭の「"」を除去
                        if (item.StartsWith(dQuoteStr))
                        {
                            item = item.Substring(1);
                        }
                        //※末尾の「"」を除去
                        if (item.EndsWith(dQuoteStr))
                        {
                            item = item.Substring(0, item.Length - 1);
                        }
                        //※文字列中の「""」→「"」に置換
                        itemList.Add(item.Replace(dQuote2Str, dQuoteStr));

                        for (int idx = 1; idx < dQuoteVals2.Length; idx++)
                        {
                            //2番目以降の項目はカンマ区切りで格納
                            itemList.AddRange(dQuoteVals2[idx].Split(delimiter));
                        }
                    }
                }
            }

            return itemList.ToArray();
        }

        /// <summary>
        /// オートコンプリート、コンボボックス用SQL実行
        /// </summary>
        /// <param name="conditionDictionary"></param>
        /// <param name="rootPath"></param>
        /// <param name="list"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public static bool ExecKanriSql(ComDB db, List<Dictionary<string, object>> conditionDictionary,
            string rootPath, ref List<Dictionary<string, object>> list, string userFactoryId, string userId, string languageId = "ja")
        {

            // パラメータを分割する
            var ctrlSQLId = "";
            var ctrlSQLParam = "";
            var ctrlCode = "";
            var ctrlInput = "";
            var factoryId = "";

            var dicList = new Dictionary<string, object>();
            dynamic results = null;

            // 検索条件を再設定
            dynamic searchCondition = null;

            // 検索結果名のペアを生成(Key:検索結果SQL側カラム名、 Value:共通FW側カラム名)
            var resultNamesPair = new Dictionary<string, string>
            {
                { "FactoryId", "FactoryId" },
                { "TranslationFactoryId", "TranslationFactoryId" },
                { "Values", "VALUE1" },
                { "Labels", "VALUE2" },
                { "DeleteFlg", "DeleteFlg" },
                { "OrderFactoryId", "OrderFactoryId" },
            };

            // 拡張コードを追加
            // 増やす場合、データクラスにも追加すること
            for (int i = 1; i <= 30; i++)
            {
                string strIndex = i.ToString();
                resultNamesPair.Add("Exparam" + strIndex, "EXPARAM" + strIndex);
            }

            try
            {
                // 条件を設定する
                foreach (var dic in conditionDictionary)
                {
                    // SQL名
                    if (dic.ContainsKey("CTRLSQLID"))
                    {
                        ctrlSQLId = dic["CTRLSQLID"].ToString();
                    }

                    // パラメータ
                    if (dic.ContainsKey("CTRLSQLPARAM") && dic["CTRLSQLPARAM"] != null)
                    {
                        ctrlSQLParam = dic["CTRLSQLPARAM"].ToString();
                    }

                    // コード
                    if (dic.ContainsKey("CTRLCODE") && dic["CTRLCODE"] != null)
                    {
                        ctrlCode = dic["CTRLCODE"].ToString();
                    }

                    if (dic.ContainsKey("CTRLINPUT") && dic["CTRLINPUT"] != null)
                    {
                        ctrlInput = dic["CTRLINPUT"].ToString();
                    }

                    if (dic.ContainsKey("FACTORYID") && dic["FACTORYID"] != null)
                    {
                        factoryId = dic["FACTORYID"].ToString();
                    }
                }

                // 検索条件名のペアを生成(Key:カラム名、 Value:検索SQL側パラメータ名)
                var condition = new Dictionary<string, object>();

                int index = 1;
                var param = "";

                // パラメータを設定する
                if (!string.IsNullOrEmpty(ctrlSQLParam))
                {
                    string[] listCtrlSQLParam = ctrlSQLParam.Split(','); // カンマ区切りで配列に挿入
                    var newList = reCreateParamList(listCtrlSQLParam);
                    for (int i = 0; i < newList.Count(); i++)
                    {
                        // 最初、末尾の文字がシングルクォーテーションの場合除去を行う
                        if (newList[i].StartsWith("'"))
                        {
                            newList[i] = newList[i].Substring(1, newList[i].Length - 1);
                        }
                        if (newList[i].EndsWith("'"))
                        {
                            newList[i] = newList[i].Substring(0, newList[i].Length - 1);
                        }
                    }

                    for (int i = 0; i < newList.Count(); i++)
                    {
                        param = "param" + index.ToString();
                        // シングルクォーテーションが含まれている場合、in句を判定する
                        if (newList[i].Contains(","))
                        {
                            List<object> tmpList = new List<object>();
                            var tmpDatas = newList[i].Split(',');
                            foreach (var tmpData in tmpDatas)
                            {
                                tmpList.Add(tmpData);
                            }
                            condition.Add(param, tmpList.ToArray());
                        }
                        else
                        {
                            condition.Add(param, newList[i]);
                        }
                        index++;
                    }
                }

                // SQL格納フォルダ(初期値はコンボ)
                string sqlDir = SqlName.SubDirComboBox;

                // オートコンプリートの場合、表示行上限を設定する
                if (ctrlSQLId.StartsWith(AutoComplete))
                {
                    // オートコンプリートの場合、格納フォルダを変更
                    sqlDir = SqlName.SubDirAutoComplete;

                    condition.Add("getNameFlg", false);
                    // コードを設定
                    if (!string.IsNullOrEmpty(ctrlInput))
                    {
                        param = "param" + index.ToString();
                        var value = ctrlInput.Replace("'", "");
                        switch (AutoComplteFlg)
                        {
                            case 0:
                                ctrlInput = ctrlInput + "%";
                                break;
                                // 到達不能なためコメントアウト 将来の拡張を想定
                                //case 1:
                                //    ctrlInput = "%" + ctrlInput;
                                //    break;
                                //case 2:
                                //    ctrlInput = "%" + ctrlInput + "%";
                                //    break;
                                //default:
                                //    break;
                        }
                        condition.Add(param, ctrlInput);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(ctrlCode))
                        {
                            param = "param" + index.ToString();
                            condition.Add(param, ctrlCode.Replace("'", ""));
                            condition["getNameFlg"] = true;
                        }
                    }

                    // 両方未設定の場合、空白などのタブ移動とみなし、名称取得処理を行う
                    if (string.IsNullOrEmpty(ctrlCode) && string.IsNullOrEmpty(ctrlInput))
                    {
                        param = "param" + index.ToString();
                        condition.Add(param, null);
                        condition["getNameFlg"] = true;
                    }

                    // 表示行上限数を設定する
                    var rowLimit = CommonWebTemplate.AppCommonObject.Config.AppSettings.AutoCompleteRowLimit;
                    if (rowLimit != null)
                    {
                        condition.Add("rowLimit", rowLimit);
                    }
                }
                if (!string.IsNullOrEmpty(factoryId))
                {
                    // 工場ID指定の場合
                    var factoryIds = factoryId.Split(",");
                    var factoryIdList = new List<int>();
                    foreach (var id in factoryIds)
                    {
                        if (!string.IsNullOrWhiteSpace(id))
                        {
                            factoryIdList.Add(Convert.ToInt32(id));
                        }
                    }
                    //システム共通の階層も併せて取得する
                    if (!factoryIdList.Contains(STRUCTURE_CONSTANTS.CommonFactoryId))
                    {
                        factoryIdList.Add(STRUCTURE_CONSTANTS.CommonFactoryId);
                    }
                    if (factoryIdList.Count > 0)
                    {
                        condition.Add("factoryIdList", factoryIdList);
                    }
                }

                // 言語コードが設定されている場合、設定を行う
                if (!string.IsNullOrEmpty(languageId))
                {
                    condition.Add("languageId", languageId);
                }

                // ユーザIDが設定されている場合、設定を行う
                if (!string.IsNullOrEmpty(userId))
                {
                    condition.Add("userId", userId);
                }

                // 検索条件を再設定
                searchCondition = setCondition(condition);

                // 条件でキー名称が「param」で始まり、値がnullの件数を取得する
                var paramNullCnt = condition.Where(x => (x.Key.ToString()).StartsWith("param"))
                                            .Where(y => y.Value == null || y.Value.ToString() == "null")
                                            .ToList().Count();


                // コンボ・オートコンプリートとして実行されるSQLで、param〇の中に文字列の「null」が含まれている場合は例外エラーとなるので実行しない
                // 右記SQLは文字列の「null」が入ってくることを想定したSQLのため実行可とする　「"A0004", "C0006", "C0020"」
                // 新たにコンボ・オートコンプリートのSQLを作成した場合は「notExecuteSqlIdList」に含めるかどうかをチェックすること
                List<string> notExecuteSqlIdList = new()
                {
                    "A0001",
                    "A0003",
                    "A0005",
                    "A0006",
                    "A0008",
                    "A0009",
                    "A0011",
                    "C0001",
                    "C0016",
                    "C0017",
                    "C0022",
                    "C0028",
                    "C0030"
                };

                if (notExecuteSqlIdList.Contains(ctrlSQLId) && paramNullCnt > 0)
                {
                    return true;
                }

                // 検索実行
                results = db.GetListByOutsideSql<Dao.AutoCompleteEntity>(ctrlSQLId, sqlDir, searchCondition);
                if (results != null && results.Count > 0)
                {
                    // 検索結果の列名を、resultNamesPairに定義された変換後名称に変換し、結果リストへ値を登録する
                    foreach (var result in results)
                    {
                        // 初期化
                        dicList = new Dictionary<string, object>();

                        var properties = typeof(Dao.AutoCompleteEntity).GetProperties();
                        Type t = typeof(Dao.AutoCompleteEntity);

                        foreach (var item in resultNamesPair)
                        {
                            object value = t.GetProperty(item.Key).GetValue(result);
                            //dicList.Add(item.Value, !CommonUtil.IsNullOrEmpty(value) ? value : "");
                            if (CommonUtil.IsNullOrEmpty(value))
                            {
                                // 値が空の場合
                                if (item.Value.StartsWith("EXPARAM"))
                                {
                                    // 拡張項目の場合は結果リストに格納しない
                                    continue;
                                }
                                else
                                {
                                    // 拡張項目以外の場合は空文字をセットして返す
                                    value = "";
                                }
                            }
                            dicList.Add(item.Value, value);
                        }
                        list.Add(dicList);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.ErrorLog(userFactoryId, userId, "", ex);
                throw;
            }
            return true;
        }

        /// <summary>
        /// パスワード変更
        /// </summary>
        /// <param name="conditionDictionary"></param>
        /// <param name="outParam"></param>
        /// <returns></returns>
        public static int ChangePassword(ComDB db, List<Dictionary<string, object>> conditionDictionary, ref string outParam, string userFactoryId, string loginUserId, string languageId = "ja")
        {
            try
            {
                // DB接続
                if (!db.Connect())
                {
                    return -1;
                }

                // パラメータを分割する
                var userId = "";
                var userOldPassword = "";
                var userNewPassword1 = "";
                var userNewPassword2 = "";

                // 条件を設定する
                foreach (var dic in conditionDictionary)
                {
                    // ユーザID
                    if (dic.ContainsKey("userId"))
                    {
                        userId = dic["userId"].ToString();
                    }

                    // 旧パスワード
                    if (dic.ContainsKey("userOldPassword"))
                    {
                        userOldPassword = dic["userOldPassword"].ToString();
                    }

                    // 新パスワード
                    if (dic.ContainsKey("userNewPassword1"))
                    {
                        userNewPassword1 = dic["userNewPassword1"].ToString();
                    }

                    // 新パスワード（確認用）
                    if (dic.ContainsKey("userNewPassword2"))
                    {
                        userNewPassword2 = dic["userNewPassword2"].ToString();
                    }
                }

                // ユーザ認証を行う
                // 条件のペアを生成(Key:検索結果SQL側カラム名、 Value:共通FW側カラム名)
                var condition = new Dictionary<string, object>
                    {
                        { "userId", userId },
                        { "password", Password.GetNewPassWord(userOldPassword) },
                        { "encryptKey", Password.GetEncryptKey()}
                    };

                var searchCondition = setCondition(condition);

                int cnt = db.GetCountByOutsideSql(SqlName.GetUserInfo, SqlName.SubDir, searchCondition);
                if (cnt < 1)
                {
                    //現在のパスワードが異なっています。
                    outParam = ComUtil.GetPropertiesMessage(CommonResources.ID.ID941090004, languageId, null, db, new List<int> { Convert.ToInt32(userFactoryId) });

                    return -1;
                }

                // 新パスワード入力確認
                if (!userNewPassword1.Equals(userNewPassword2))
                {
                    //新しいパスワードの入力に誤りがあります。
                    outParam = ComUtil.GetPropertiesMessage(CommonResources.ID.ID941010003, languageId, null, db, new List<int> { Convert.ToInt32(userFactoryId) });
                    return -1;
                }
                // 更新条件を設定する
                condition = new Dictionary<string, object>
                    {
                        { "userId", userId },
                        { "password", Password.GetNewPassWord(userNewPassword1) },
                        { "encryptKey",Password.GetEncryptKey()}
                    };

                var updateCondition = setCondition(condition);
                // 更新処理実行
                int result = db.RegistByOutsideSql(SqlName.UpdateUserInfo, SqlName.SubDir, updateCondition);

                if (result < 0)
                {
                    //パスワード変更が失敗しました。
                    outParam = ComUtil.GetPropertiesMessage(CommonResources.ID.ID941260029, languageId, null, db, new List<int> { Convert.ToInt32(userFactoryId) });

                    return -1;
                }

                //パスワードを変更しました。
                outParam = ComUtil.GetPropertiesMessage(CommonResources.ID.ID941260030, languageId, null, db, new List<int> { Convert.ToInt32(userFactoryId) });
            }
            catch (Exception ex)
            {
                logger.ErrorLog(userFactoryId, loginUserId, "", ex);
                throw;
            }
            finally
            {
                if (db != null)
                {
                    db.Close();
                }
            }

            return 0;
        }

        /// <summary>
        /// ログイン認証
        /// </summary>
        /// <param name="loginId">ログインID</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="conditionDictionary">実行条件</param>
        /// <param name="rootPath">ルート物理パス</param>
        /// <param name="outList">返却値(機能IDのリスト)</param>
        /// <returns></returns>
        public static int GetLoginAuthentication(ComDB db, string loginId, string userId, List<Dictionary<string, object>> conditionDictionary, ref List<Dictionary<string, object>> outList, bool isNewLogin)
        {
            var dicList = new Dictionary<string, object> { { "result", "0" } }; // 認証失敗で初期化
            string loginUserId = string.Empty;
            string loginFactoryId = string.Empty;
            try
            {
                // パラメータを分割する
                var userPassword = "";
                var passwordCheckFlg = "";

                foreach (var dic in conditionDictionary)
                {
                    // パスワード
                    if (dic.ContainsKey("userPassword"))
                    {
                        userPassword = dic["userPassword"].ToString();
                    }

                    // パスワードチェックフラグ
                    if (dic.ContainsKey("passwordCheckFlg"))
                    {
                        passwordCheckFlg = dic["passwordCheckFlg"].ToString();
                    }
                }

                string subDir = "Common";

                // ユーザマスタからユーザ情報を取得
                dynamic param = new ExpandoObject();
                if (!string.IsNullOrEmpty(loginId))
                {
                    param.LoginId = loginId;
                }
                if (!string.IsNullOrEmpty(userId))
                {
                    param.UserId = Convert.ToInt32(userId);
                }
                if (passwordCheckFlg == "1")
                {
                    param.Password = Password.GetNewPassWord(userPassword);
                    param.EncryptKey = Password.GetEncryptKey();
                }
                // ユーザ情報
                Dao.LoginEntity result = db.GetEntityByOutsideSql<Dao.LoginEntity>("UserMst_GetLoginInfo", subDir, param);
                if (result == null)
                {
                    // 認証失敗
                    outList.Add(dicList);
                    return -1;
                }
                loginUserId = result.UserId.ToString();
                dicList.Add("userId", result.UserId);                   // ユーザID
                dicList.Add("loginId", result.LoginId);                 // ログインID
                dicList.Add("userName", result.UserName);               // ユーザ名
                dicList.Add("languageId", result.LanguageId);           // 言語ID
                dicList.Add("authorityLevelId", result.AuthorityLevelId);    // 権限レベルID

                if (!isNewLogin)
                {
                    // ログイン済みの場合は以降の処理をスキップ
                    return 0;
                }

                var belongingInfo = new BelongingInfo();
                if (result.AuthorityLevelId != USER_CONSTANTS.AUTHORITY_LEVEL.Administrator)
                {
                    // システム管理者以外の場合、ユーザ所属マスタ取得
                    var belongingList = db.GetListByOutsideSql<StructureInfo>("UserBelonging_GetList", subDir, new { UserId = result.UserId });
                    if (belongingList != null && belongingList.Count > 0)
                    {
                        belongingInfo.SetBelongingList(belongingList.ToList());
                    }
                }
                else
                {
                    // システム管理者の場合、構成マスタから全工場を取得
                    var locationList = db.GetListByOutsideSql<StructureInfo>("Structure_GetInfoList", subDir,
                        new { GroupId = STRUCTURE_CONSTANTS.STRUCTURE_GROUP.Location, LayerNo = STRUCTURE_CONSTANTS.LAYER_NO.Factory });
                    if (locationList != null && locationList.Count > 0)
                    {
                        belongingInfo.SetLocationInfoList(locationList.ToList(), true);
                    }

                    // 構成マスタから全職種を取得して所属職種リストに追加
                    var jobList = db.GetListByOutsideSql<StructureInfo>("Structure_GetInfoList", subDir,
                        new { GroupId = STRUCTURE_CONSTANTS.STRUCTURE_GROUP.Job, LayerNo = STRUCTURE_CONSTANTS.LAYER_NO.Job });
                    if (jobList != null && jobList.Count > 0)
                    {
                        belongingInfo.SetJobInfoList(jobList.ToList());
                    }
                }
                // ログインユーザの本務工場ID
                loginFactoryId = belongingInfo.DutyFactoryId.ToString();
                dicList.Add("belongingInfo", belongingInfo);

                // ユーザ機能権限マスタから権限情報を取得
                int? paramUserId = result.AuthorityLevelId != USER_CONSTANTS.AUTHORITY_LEVEL.Administrator ? result.UserId : null;
                var resultList = db.GetListByOutsideSql<Dao.MsUserConductAuthorityEntity>(SqlName.UserAuthGetListForLogin, subDir, new { UserId = paramUserId });
                List<string> conductList = new List<string>();
                if (resultList != null && resultList.Count > 0)
                {
                    conductList.AddRange(resultList.Select(x => x.ConductId));
                }

                // トランザクション開始
                // トランザクション開始
                db.BeginTransaction();
                try
                {
                    // GUIDを生成
                    string guid = Guid.NewGuid().ToString("D");
                    // セッション管理テーブルへの登録
                    if (!registSessionStartInfo(db, result.UserId.ToString(), guid, DateTime.Now))
                    {
                        // セッション管理情報登録失敗時はGUIDを再生成して登録
                        guid = Guid.NewGuid().ToString("D");
                        if (!registSessionStartInfo(db, result.UserId.ToString(), guid, DateTime.Now))
                        {
                            // 再度登録に失敗した場合はGUIDの重複以外が原因とみなし、認証失敗とする
                            outList.Add(dicList);
                            return -1;
                        }
                    }
                    dicList.Add("guid", guid);  // GUID

                    //// 他ユーザの一時テーブル過去データのクリア
                    //var conf = CommonWebTemplate.AppCommonObject.Config.AppSettings.TmpTableDeleteHoursOther;
                    //if (conf.HasValue)
                    //{
                    //    int delHours = conf.Value;
                    //    if (delHours > 0)
                    //    {
                    //        DeleteTmpTableData(db, "", delHours);
                    //    }
                    //}

                    //// 自ユーザの一時テーブル過去データのクリア
                    //conf = CommonWebTemplate.AppCommonObject.Config.AppSettings.TmpTableDeleteHours;
                    //if (conf.HasValue)
                    //{
                    //    int delHours = conf.Value;
                    //    if (delHours > 0)
                    //    {
                    //        DeleteTmpTableData(db, userId, delHours);
                    //    }
                    //}

                    // コミット
                    db.Commit();
                }
                catch (Exception ex)
                {
                    logger.ErrorLog("", "", "", ex);    // 工場IDとユーザIDは指定できない
                    // 認証結果(失敗)                    // 認証結果(失敗)
                    outList.Add(dicList);
                    // ロールバック
                    db.RollBack();
                    return -1;
                }
                finally
                {
                    // トランザクション終了
                    db.EndTransaction();
                }

                // 機能IDのリストを返却
                dicList.Add("conductIdList", conductList);
                dicList["result"] = "1";    // 認証結果(正常終了)
                outList.Add(dicList);

                return 0;
            }
            catch (Exception ex)
            {
                logger.ErrorLog(loginFactoryId, loginUserId, "", ex);
                // 認証結果(失敗)                    // 認証結果(失敗)
                outList.Add(dicList);
                return -1;
            }
            finally
            {
                if (db != null)
                {
                    db.Close();
                }
            }
        }

        /// <summary>
        /// ボタン権限チェック
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="factoryId">本務工場ID</param>
        /// <param name="userId">登録者ID</param>
        /// <param name="authorityLevelId">権限レベルID</param>
        /// <param name="conductId">機能ID</param>
        /// <param name="conditionDictionary">実行条件</param>
        /// <param name="outList">返却値(コントロールID毎の権限のリスト)</param>
        /// <returns></returns>
        public static int GetCheckAuthority(ComDB db, BelongingInfo belongingInfo, string userId, int authorityLevelId, string conductId, List<Dictionary<string, object>> conditionDictionary, ref List<Dictionary<string, object>> outList)
        {
            outList = new List<Dictionary<string, object>>();
            try
            {
                // DB接続
                if (!db.Connect())
                {
                    return -1;
                }

                //// ■ユーザ権限マスタからユーザ権限情報を取得
                //var sbSql = new StringBuilder();
                //sbSql.AppendLine("select distinct authkbn from com_user_auth");
                //sbSql.AppendLine("where userid = @UserId and conductid = @ConductId");
                //var results = db.GetList(sbSql.ToString(), new { UserId = userId, ConductId = conductId });

                //var userAuths = new List<string>();
                //foreach (var result in results)
                //{
                //    IDictionary<string, object> dicResult = result as IDictionary<string, object>;
                //    if (dicResult.ContainsKey("authkbn"))
                //    {
                //        userAuths.Add((double.Parse(dicResult["authkbn"].ToString())).ToString());
                //    }
                //}

                //// ■ユーザ権限からボタン権限を判定
                //foreach (var condition in conditionDictionary)
                //{
                //    int statusW = 0; //非表示
                //    if (condition.ContainsKey("authKbn"))
                //    {
                //        if (IsNullOrEmpty(condition["authKbn"]))
                //        {
                //            //※権限区分指定なし
                //            statusW = 1; //表示(活性)
                //        }
                //        else if (userAuths.Contains(condition["authKbn"].ToString()))
                //        {
                //            statusW = 1; //表示(活性)
                //        }
                //    }
                //    outList.Add(new Dictionary<string, object>() { { "ctrlId", condition["ctrlId"].ToString() }, { "status", statusW } });
                //}

                //// ↓テーブルレイアウトが変更になったら切り替えること
                //// ■ロール権限マスタからボタン権限を設定
                //IList<BtnAuthority> results = db.GetListByOutsideSqlByDataClass<BtnAuthority>(SqlName.CheckBtnAuthority, SqlName.SubDir,
                //    new { ConductId = conductId, UserId = userId });

                //// データが取得できた場合、フロントに反映させる
                //if (results != null && results.Count > 0)
                //{
                //    // 認証有無を確認
                //    string authFlg = db.GetEntityByOutsideSql<string>(SqlName.CheckConductAuthFlg, SqlName.SubDir, new { ConductId = conductId });
                //    if (string.IsNullOrEmpty(authFlg)) { authFlg = AuthFlg.NotNeed.ToString(); } // 未設定の場合、認証不要を設定

                //    foreach (var result in results)
                //    {
                //        // 初期化
                //        dic = new Dictionary<string, object>();

                //        // 画面No、ボタンコントロール、表示区分を設定
                //        dic.Add("FORMNO", result.Formno);
                //        dic.Add("CTRLID", result.BtnCtrlid);
                //        dic.Add("AUTHFLG", result.BtnAuthcontrolkbn);
                //        dic.Add("DISPKBN", USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF.Active);

                //        // ボタンアクション区分によって処理を分岐
                //        if (result.BtnActionkbn == FORM_DEFINE_CONSTANTS.CTRLTYPE.ControlGroup)
                //        {
                //            // 実行(201)の場合、
                //            if (!string.IsNullOrEmpty(result.Optioninfo))
                //            {
                //                // 不可情報が設定されている場合
                //                dic.Add("AUTHKBN", result.Optioninfo);
                //            }
                //            else
                //            {
                //                // 付加情報が未設定の場合
                //                dic.Add("AUTHKBN", "-");
                //            }
                //        }
                //        else
                //        {
                //            // 実行(201)以外の場合
                //            dic.Add("AUTHKBN", "-");
                //        }
                //        outList.Add(dic);
                //    }
                //}

                // ■画面コントロールマスタからボタン管理区分を取得
                List<int> factoryIdList = new List<int> { belongingInfo.DutyFactoryId };
                if (!factoryIdList.Contains(STRUCTURE_CONSTANTS.CommonFactoryId))
                {
                    factoryIdList.Add(STRUCTURE_CONSTANTS.CommonFactoryId);
                }
                IList<BtnAuthority> results = db.GetListByOutsideSqlByDataClass<BtnAuthority>(SqlName.GetBtnAuthority, SqlName.SubDir,
                    new { ConductId = conductId, FactoryIdList = factoryIdList });
                if (results != null && results.Count > 0)
                {
                    foreach (var result in results)
                    {
                        var dic = new Dictionary<string, object>();
                        // 画面No、ボタンコントロールID、権限区分を設定
                        dic.Add("FORMNO", result.FormNo);
                        dic.Add("CTRLID", result.BtnCtrlId);
                        dic.Add("AUTHDIV", result.BtnAuthDiv);
                        if (result.BtnAuthDiv != LISTITEM_DEFINE_CONSTANTS.BTN_AUTHCONTROLKBN.Free)
                        {
                            // 権限管理有りの場合、ボタンの表示区分を取得して設定
                            var dispDiv = GetBtnDispDivision(authorityLevelId, result.BtnActionDiv, result.TransActionDiv);
                            dic.Add("DISPKBN", dispDiv);
                        }
                        else
                        {
                            // 権限管理無しの場合、活性を設定
                            dic.Add("DISPKBN", USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF.Active);
                        }
                        outList.Add(dic);
                    }
                }

                // ■画面コントロールマスタから共通レイアウトのボタン管理区分を取得
                results = db.GetListByOutsideSqlByDataClass<BtnAuthority>(SqlName.GetBtnAuthority, SqlName.SubDir,
                    new { ConductId = conductId, IsCommonLayout = true, FactoryIdList = factoryIdList });
                if (results != null && results.Count > 0)
                {
                    foreach (var result in results)
                    {
                        var dic = new Dictionary<string, object>();
                        // 画面No、ボタンコントロールID、権限区分を設定
                        dic.Add("FORMNO", result.FormNo);
                        dic.Add("CTRLID", result.BtnCtrlId);
                        dic.Add("AUTHDIV", result.BtnAuthDiv);
                        if (result.BtnAuthDiv != LISTITEM_DEFINE_CONSTANTS.BTN_AUTHCONTROLKBN.Free)
                        {
                            // 権限管理有りの場合、ボタンの表示区分を取得して設定
                            var dispDiv = GetBtnDispDivision(authorityLevelId, result.BtnActionDiv, result.TransActionDiv);
                            dic.Add("DISPKBN", dispDiv);
                        }
                        else
                        {
                            // 権限管理無しの場合、活性を設定
                            dic.Add("DISPKBN", USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF.Active);
                        }
                        outList.Add(dic);
                    }
                }

                // ■画面レイアウトマスタから行編集情報を取得GetTblRowEditInfo
                IList<TblRowAuthority> resultsTbl = db.GetListByOutsideSqlByDataClass<TblRowAuthority>(SqlName.GetTblRowEditInfo, SqlName.SubDir,
                    new { ConductId = conductId });
                if (resultsTbl != null && resultsTbl.Count > 0)
                {
                    foreach (var result in resultsTbl)
                    {
                        var dic = new Dictionary<string, object>();
                        // 画面No、コントロールグループID、権限区分を設定
                        dic.Add("FORMNO", result.FormNo);
                        dic.Add("CTRLGRPID", result.CtrlGrpId);
                        // 行追加区分/行削除区分が未設定以外の場合、権限管理対象
                        var authDiv = (result.RowAddDiv == FORM_DEFINE_CONSTANTS.DAT_ROWADDKBN.None && result.RowDelDiv == FORM_DEFINE_CONSTANTS.DAT_ROWDELKBN.None) ?
                            LISTITEM_DEFINE_CONSTANTS.BTN_AUTHCONTROLKBN.Free : LISTITEM_DEFINE_CONSTANTS.BTN_AUTHCONTROLKBN.Control;
                        dic.Add("AUTHDIV", authDiv);
                        var dispDiv = GetTblRowEditDivision(authorityLevelId, authDiv);
                        dic.Add("DISPKBN", dispDiv);
                        outList.Add(dic);
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                logger.ErrorLog(belongingInfo.DutyFactoryId.ToString(), userId, "", ex);
                return -1;
            }
            finally
            {
                if (db != null)
                {
                    db.Close();
                }
            }
        }

        /// <summary>
        /// ボタン表示区分の取得
        /// </summary>
        /// <param name="authorityLevelId">権限レベルID</param>
        /// <param name="btnActionDiv">ボタンアクション区分</param>
        /// <param name="transActionDiv">画面遷移アクション区分</param>
        /// <returns></returns>
        public static int GetBtnDispDivision(int authorityLevelId, int btnActionDiv, int transActionDiv)
        {
            int resultDiv = USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF.Active;
            if (authorityLevelId == USER_CONSTANTS.AUTHORITY_LEVEL.Guest)
            {
                // ゲストユーザーの場合は参照権限のみ
                // ボタンのアクション区分によって振り分ける
                switch (btnActionDiv)
                {
                    case LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.Execute:       // 実行
                    case LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.Delete:        // 削除
                    case LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ComBatExec:    // バッチ実行
                    case LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.Upload:        // 取り込みフォームポップアップ
                    case LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.ComUpload:     // ファイル取り込み実行
                        // ボタンを非活性に設定
                        resultDiv = USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF.Disabled;
                        break;
                    case LISTITEM_DEFINE_CONSTANTS.ACTIONKBN.FormTransition:    // 画面遷移
                        if (transActionDiv != LISTITEM_DEFINE_CONSTANTS.DAT_TRANS_ACTION_DIV.None)
                        {
                            // 画面遷移アクション区分が未設定以外は非活性
                            resultDiv = USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF.Disabled;
                        }
                        break;
                    default:
                        break;
                }
            }
            return resultDiv;
        }

        /// <summary>
        /// 機能権限チェック
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="belongingInfo">所属情報</param>
        /// <param name="userId">登録者ID</param>
        /// <param name="authorityLevelId">権限レベルID</param>
        /// <param name="conductId">機能ID</param>
        /// <param name="hasAuthority">返却値(機能権限の有無)</param>
        /// <returns></returns>
        public static int CheckConductAuthority(ComDB db, BelongingInfo belongingInfo, string userId, int authorityLevelId, string conductId, out bool hasAuthority)
        {
            hasAuthority = false;
            try
            {
                // 機能権限有無を取得
                int? paramUserId = authorityLevelId != USER_CONSTANTS.AUTHORITY_LEVEL.Administrator ? Convert.ToInt32(userId) : null;
                int cnt = db.GetCountByOutsideSql(SqlName.UserAuthGetCount, SqlName.SubDir,
                    new { ConductId = conductId, UserId = userId });

                hasAuthority = cnt > 0;

                return 0;
            }
            catch (Exception ex)
            {
                logger.ErrorLog(belongingInfo.DutyFactoryId.ToString(), userId, "", ex);
                return -1;
            }
        }

        /// <summary>
        /// 一覧行編集区分の取得
        /// </summary>
        /// <param name="authorityLevelId">権限レベルID</param>
        /// <param name="authDiv">権限管理区分</param>
        /// <param name="transActionDiv">画面遷移アクション区分</param>
        /// <returns></returns>
        public static int GetTblRowEditDivision(int authorityLevelId, int authDiv)
        {
            int resultDiv;
            if (authDiv == LISTITEM_DEFINE_CONSTANTS.BTN_AUTHCONTROLKBN.Control)
            {
                // 権限管理対象の場合
                if (authorityLevelId == USER_CONSTANTS.AUTHORITY_LEVEL.Guest)
                {
                    // ゲストユーザーの場合は参照権限のみ、表示区分＝不活性
                    resultDiv = USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF.Disabled;
                }
                else
                {
                    // ゲストユーザー以外の場合、表示区分＝活性
                    resultDiv = USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF.Active;
                }
            }
            else
            {
                // 権限管理対象外の場合は表示区分＝非表示
                resultDiv = USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF.Hide;
            }
            return resultDiv;
        }

        /// <summary>
        /// 権限チェック(バッチ処理の場合)
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="userId">登録者ID</param>
        /// <param name="conductId">機能ID</param>
        /// <param name="execBtn">コントロールID ※実行</param>
        /// <param param name="reDispBtn">コントロールID ※再描画</param>
        /// <param name="outList">返却値(コントロールID毎の権限のリスト)</param>
        /// <returns></returns>
        public static int GetCheckAuthorityForBatch(ComDB db, string userId, string conductId, string execBtn, string reDispBtn, ref List<Dictionary<string, object>> outList)
        {
            outList = new List<Dictionary<string, object>>();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                // DB接続
                if (!db.Connect())
                {
                    return -1;
                }

                // ↓テーブルレイアウトが変更になったら切り替えること
                // ■ロール権限マスタからボタン権限を設定
                BtnAuthority result = db.GetEntityByOutsideSqlByDataClass<BtnAuthority>(SqlName.CheckBtnAuthorityForBatch, SqlName.SubDir,
                    new { ConductId = conductId, Ctrlid = execBtn, UserId = userId });

                // データが取得できた場合、フロントに反映させる
                if (result != null)
                {
                    // 初期化
                    dic = new Dictionary<string, object>();

                    // 画面No、ボタンコントロール、表示区分を設定
                    dic.Add("FORMNO", result.FormNo);
                    dic.Add("CTRLID", result.BtnCtrlId);
                    dic.Add("AUTHFLG", "-");
                    dic.Add("DISPKBN", USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF.Active);
                    dic.Add("AUTHKBN", "-");

                    outList.Add(dic);
                }

                // 再描画ボタンは常に表示に
                dic = new Dictionary<string, object>(); // 初期化

                // 画面No、ボタンコントロール、表示区分を設定
                dic.Add("FORMNO", 0); // 固定で"0"を設定
                dic.Add("CTRLID", reDispBtn);
                dic.Add("AUTHFLG", "-");
                dic.Add("DISPKBN", USER_AUTH_SHORI_CONSTANTS.BTN_DISPKBN_DEF.Active);
                dic.Add("AUTHKBN", "-");

                outList.Add(dic);

                return 0;
            }
            catch
            {
                return -1;
            }
            finally
            {
                if (db != null)
                {
                    db.Close();
                }
            }
        }

        /// <summary>
        /// 一時テーブルデータの削除
        /// </summary>
        /// <param name="db">DB操作クラス(呼び元でオープン＆クローズすること！)</param>
        /// <param name="userId">登録者ID</param>
        /// <param name="deleteHours">一時テーブル削除期限(時間)</param>
        /// <param name="guid">GUID</param>
        /// <param name="tabNo">ブラウザタブ識別番号</param>
        /// <returns></returns>
        public static int DeleteTmpTableData(ComDB db, string userId, int deleteHours, string guid = "", string tabNo = "")
        {
            try
            {
                string subDir = "Common";

                // 一時テーブルデータを削除
                dynamic param = new ExpandoObject();
                if (!string.IsNullOrEmpty(userId))
                {
                    // ユーザID指定の場合
                    param.UserId = userId;
                }
                if (deleteHours > 0)
                {
                    // 削除期限指定の場合、現在時刻-削除期限(時間)より過去のデータを削除する
                    param.DeleteLimitDate = DateTime.Now.AddHours(-1 * deleteHours);
                }
                if (!string.IsNullOrEmpty(guid))
                {
                    // GUID指定の場合
                    param.GUID = guid;

                    if (!string.IsNullOrEmpty(tabNo))
                    {
                        // ブラウザタブ識別指定の場合
                        param.BrowserTabNo = tabNo;
                    }
                }

                var result = db.RegistByOutsideSql("TempData_Delete", subDir, param);
                if (result < 0)
                {
                    // 削除失敗
                    return -1;
                }
                return 0;
            }
            catch
            {
                // 削除失敗
                return -1;
            }
        }

        /// <summary>
        /// セッション管理テーブルのログアウト日時の更新
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="userId">ログインユーザID</param>
        /// <param name="guid">GUID</param>
        /// <param name="loginDate">ログアウト日時</param>
        /// <returns>登録結果(成功:true/失敗:false)</returns>
        public static bool UpdateLogoutDate(ComDB db, string userId, string guid, DateTime logoutDate)
        {
            try
            {
                var result = db.RegistByOutsideSql("SessionInfo_Update", "Common",
                    new { UserId = userId, GUID = guid, LogoutDate = logoutDate });
                return result == 1;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// keyに対応するメッセージを取得する
        /// </summary>
        /// <param name="key">翻訳ID</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="paramMsg">メッセージリソース、省略した場合はDBより取得</param>
        /// <param name="db">DB接続、メッセージリソースより取得する場合は省略可</param>
        /// <returns>取得したメッセージ</returns>
        /// <remarks>メッセージリソースとDB接続がどちらもNullの場合ファイルより取得</remarks>
        public static string GetPropertiesMessage(string key, string languageId = "ja", MessageResources paramMsg = null, ComDB db = null, List<int> factoryIdList = null)
        {
            if (paramMsg == null && db == null)
            {
                // どちらもNullの場合はファイルより取得する
                return GetPropertiesMessageFile(key, languageId);
            }

            var msgRes = paramMsg;
            if (paramMsg == null)
            {
                // メッセージリソースが取得できない場合はDBより取得
                msgRes = GetMessageResourceFromDb(db, languageId, new string[] { key }, factoryIdList);
            }

            return msgRes.GetMessage(key, languageId); ;
        }

        /// <summary>
        /// keyに対応するメッセージを取得する
        /// </summary>
        /// <param name="keys">翻訳ID</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="paramMsg">メッセージリソース、省略した場合はDBより取得</param>
        /// <param name="db">DB接続、メッセージリソースより取得する場合は省略可</param>
        /// <returns>取得したメッセージ</returns>
        /// <remarks>メッセージリソースとDB接続がどちらもNullの場合ファイルより取得</remarks>
        public static string GetPropertiesMessage(string[] keys, string languageId = "ja", MessageResources paramMsg = null, ComDB db = null, List<int> factoryIdList = null)
        {
            if (paramMsg == null && db == null)
            {
                // どちらもNullの場合はファイルより取得する
                return GetPropertiesMessageFile(keys, languageId);
            }

            var msgRes = paramMsg;
            if (paramMsg == null)
            {
                // メッセージリソースが取得できない場合はDBより取得
                msgRes = GetMessageResourceFromDb(db, languageId, keys, factoryIdList);
            }

            return msgRes.GetMessageJoin(keys, languageId);
        }

        /// <summary>
        /// keyに対応するメッセージを取得する
        /// </summary>
        /// <param name="keys">翻訳ID</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="paramMsg">メッセージリソース、省略した場合はDBより取得</param>
        /// <param name="db">DB接続、メッセージリソースより取得する場合は省略可</param>
        /// <returns>取得したメッセージ</returns>
        /// <remarks>メッセージリソースとDB接続がどちらもNullの場合ファイルより取得</remarks>
        public static List<Dictionary<string, object>> GetPropertiesMessages(string[] keys, string languageId = "ja", MessageResources paramMsg = null, ComDB db = null, List<int> factoryIdList = null)
        {
            Dictionary<string, object> resource = new Dictionary<string, object>();

            // リソースファイルからメッセージを取得
            if (db == null)
            {
                return GetPropertiesAllMessageFile(languageId);
            }

            // DBからメッセージを取得
            return GetMessageResourceFromDbVerDic(db, languageId, keys, factoryIdList);
        }

        /// <summary>
        /// メッセージをすべて取得
        /// </summary>
        /// <param name="languageId">言語ID</param>
        /// <param name="db">DB操作クラス、省略時はリソースファイルから取得</param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> GetPropertiesAllMessage(string languageId = "ja", ComDB db = null, List<int> factoryIdList = null)
        {
            Dictionary<string, object> resource = new Dictionary<string, object>();

            // リソースファイルからメッセージを取得
            if (db == null)
            {
                return GetPropertiesAllMessageFile(languageId);
            }

            // DBからメッセージを取得
            return GetMessageResourceFromDbVerDic(db, languageId, null, factoryIdList);
        }

        /// <summary>
        /// 場所階層・職種機種リスト取得
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="factoryId">本務工場ID</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="languageId">言語ID</param>
        /// <returns></returns>
        public static int GetStructureList(ComDB db, BelongingInfo belongingInfo, string userId, string languageId, List<Dictionary<string, object>> conditionDictionary, ref List<Dictionary<string, object>> outList)
        {
            var dicList = new Dictionary<string, object> { { "result", "0" } }; // 取得失敗で初期化
            try
            {
                // DB接続
                if (!db.Connect())
                {
                    return -1;
                }

                // パラメータ
                List<int> structureGroupIdList = new List<int>();
                List<int> factoryIdList = new List<int>();
                List<int> structureIdList = new List<int>();
                bool exceptCommonFactory = true; // 予備品共通工場除外フラグ
                foreach (var dic in conditionDictionary)
                {
                    // 構成グループ
                    if (dic.ContainsKey("structureGroupList") && dic["structureGroupList"] != null)
                    {
                        structureGroupIdList = dic["structureGroupList"] as List<int>;
                    }
                    // 工場ID
                    if (dic.ContainsKey("factoryIdList") && dic["factoryIdList"] != null)
                    {
                        factoryIdList = dic["factoryIdList"] as List<int>;
                    }
                    // 所属構成ID
                    if (dic.ContainsKey("structureIdList") && dic["structureIdList"] != null)
                    {
                        structureIdList = dic["structureIdList"] as List<int>;
                    }
                }
                // 場所階層の場合(場所階層(1000),変更管理関連(1001,1002))
                // ユーザの本務工場でも、その配下の場所に権限が設定されている場合はその工場の権限を除外する
                // ※本務工場の下のAプラントに権限がある場合、Bプラントの権限は無いので本務工場の権限を持つとBプラントの権限も含まれてしまう
                List<int> dutyStructureGroupList = new List<int> {
                    STRUCTURE_CONSTANTS.STRUCTURE_GROUP .Location, STRUCTURE_CONSTANTS.STRUCTURE_GROUP .LocationNoHistory,
                    STRUCTURE_CONSTANTS.STRUCTURE_GROUP .LocationHistory};
                bool isExistsDutyLocationGroup = structureGroupIdList.Intersect(dutyStructureGroupList).Any();
                // 本務工場が設定されている場合(システム管理者でない)
                if (isExistsDutyLocationGroup && belongingInfo.DutyFactoryId != STRUCTURE_CONSTANTS.CommonFactoryId)
                {
                    // 所属場所階層に本務工場の配下(工場IDが本務工場)で場所階層が本務工場でないものがあるか
                    // ⇒本務工場全体の権限が無いので、所属構成IDから本務工場を削除

                    int dutyFactoryId = belongingInfo.DutyFactoryId; // 本務工場ID(変数名が長いので定義)
                    // 本務工場配下有無フラグ(工場IDが本務工場で構成IDが本務工場でない所属場所階層があればTrue)
                    var isExistsDutyFollower = belongingInfo.LocationInfoList.Where(x => x.FactoryId == dutyFactoryId && x.StructureId != dutyFactoryId).Any();
                    if (isExistsDutyFollower)
                    {
                        // 構成IDから本務工場を削除
                        structureIdList.Remove(dutyFactoryId);
                    }
                }

                // 予備品の場合
                if (structureGroupIdList.Contains(STRUCTURE_CONSTANTS.STRUCTURE_GROUP.SpareLocation))
                {
                    // 管理工場を取得(コンボと同じ仕様)
                    var result = db.GetListByOutsideSqlByDataClass<Dao.AutoCompleteEntity>(SqlName.GetPartsFactoryId, SqlName.SubDirComboBox,
                           new { languageId = languageId, param1 = STRUCTURE_CONSTANTS.STRUCTURE_GROUP.Location, param2 = STRUCTURE_CONSTANTS.LAYER_NO.Factory, userId = userId }).ToList();
                    // 工場IDに設定
                    factoryIdList = result.Select(x => int.Parse(x.Values)).ToList();
                    // 構成マスタによる絞込は行わない
                    structureIdList = new();
                    // 予備品共通工場は表示する
                    exceptCommonFactory = false;
                }
                // ツリー項目で工場個別の翻訳、並び替えを行う場合True
                bool isTransFactoryOrderTree = false;
                // 原因性格の場合
                if (structureGroupIdList.Contains(STRUCTURE_CONSTANTS.STRUCTURE_GROUP.Cause))
                {
                    // 構成マスタによる絞込は行わない
                    structureIdList = new();
                    if (structureGroupIdList.Count == 1)
                    {
                        isTransFactoryOrderTree = true;
                    }
                }

                if (structureIdList != null && structureIdList.Count > 0)
                {
                    // 構成ID指定の場合は工場IDを指定しない
                    factoryIdList = new();
                    //システム共通の階層も併せて取得する
                    if (!structureIdList.Contains(STRUCTURE_CONSTANTS.CommonFactoryId))
                    {
                        structureIdList.Add(STRUCTURE_CONSTANTS.CommonFactoryId);
                    }
                }
                else
                {
                    //システム共通の階層も併せて取得する
                    if (!factoryIdList.Contains(STRUCTURE_CONSTANTS.CommonFactoryId))
                    {
                        factoryIdList.Add(STRUCTURE_CONSTANTS.CommonFactoryId);
                    }
                }

                bool isLocationForUserUst = structureGroupIdList.Contains(STRUCTURE_CONSTANTS.STRUCTURE_GROUP.LocationForUserMst);
                if (isLocationForUserUst)
                {
                    // ユーザーマスタの場所階層の場合、予備品共通工場を表示する
                    exceptCommonFactory = false;
                    changeTreeStructureGroupId(structureGroupIdList, STRUCTURE_CONSTANTS.STRUCTURE_GROUP.LocationForUserMst, STRUCTURE_CONSTANTS.STRUCTURE_GROUP.Location);
                }

                // 工場と職種の場合、実行するSQLが違う
                string sqlName = SqlName.StructureGetListAll;
                if (structureGroupIdList.Contains(STRUCTURE_CONSTANTS.STRUCTURE_GROUP.FactoryAndJob))
                {
                    sqlName = SqlName.GetFactoryAndJob;
                }

                // 変更管理工場の考慮
                // 変更管理する工場、しない工場どちらかのみを表示する場合
                int narrowHistoryFactory = 0; // SQLパラメータ初期値
                bool isHistory = false; // 変更管理の考慮をする場合FALSE
                if (structureGroupIdList.Contains(STRUCTURE_CONSTANTS.STRUCTURE_GROUP.LocationHistory))
                {
                    // 変更管理する工場のみを表示する場合
                    isHistory = true;
                    narrowHistoryFactory = STRUCTURE_CONSTANTS.STRUCTURE_GROUP.LocationHistory;
                    // 構成グループIDの置換
                    changeTreeStructureGroupId(structureGroupIdList, STRUCTURE_CONSTANTS.STRUCTURE_GROUP.LocationHistory, STRUCTURE_CONSTANTS.STRUCTURE_GROUP.Location);
                }
                if (structureGroupIdList.Contains(STRUCTURE_CONSTANTS.STRUCTURE_GROUP.LocationNoHistory))
                {
                    // 変更管理しない工場のみを表示する場合
                    isHistory = true;
                    narrowHistoryFactory = STRUCTURE_CONSTANTS.STRUCTURE_GROUP.LocationNoHistory;
                    // 構成グループIDの置換
                    changeTreeStructureGroupId(structureGroupIdList, STRUCTURE_CONSTANTS.STRUCTURE_GROUP.LocationNoHistory, STRUCTURE_CONSTANTS.STRUCTURE_GROUP.Location);
                }

                // 構成リストを取得
                var structureList = db.GetListByOutsideSql<Dao.VStructureItemEntity>(sqlName, SqlName.SubDir,
                      new
                      {
                          LanguageId = languageId,
                          FactoryIdList = factoryIdList,
                          StructureIdList = structureIdList,
                          StructureGroupIdList = structureGroupIdList,
                          ExceptCommonFactory = exceptCommonFactory,
                          NarrowHistoryFactory = narrowHistoryFactory,
                          IsTransFactoryOrderTree = isTransFactoryOrderTree
                      }).ToList();

                if (isLocationForUserUst)
                {
                    // 現状のデータだと予備品の共通工場だけでなく倉庫まで取得できるので、構成グループIDが1040のものは除く
                    structureList = structureList.Where(x => x.StructureGroupId != STRUCTURE_CONSTANTS.STRUCTURE_GROUP.SpareLocation).ToList();

                    // ユーザマスタの場所階層の場合、構成グループIDを設定し直す
                    structureList = structureList.Select(x => { x.StructureGroupId = STRUCTURE_CONSTANTS.STRUCTURE_GROUP.LocationForUserMst; return x; }).ToList();
                    changeTreeStructureGroupId(structureGroupIdList, STRUCTURE_CONSTANTS.STRUCTURE_GROUP.Location, STRUCTURE_CONSTANTS.STRUCTURE_GROUP.LocationForUserMst);
                }

                if (isHistory)
                {
                    // 現状のデータだと予備品の共通工場だけでなく倉庫まで取得できるので、構成グループIDが1040のものは除く
                    structureList = structureList.Where(x => x.StructureGroupId != STRUCTURE_CONSTANTS.STRUCTURE_GROUP.SpareLocation).ToList();
                    // 変更履歴を行う工場の場合、構成グループIDを設定し直す
                    structureList = structureList.Select(x => { x.StructureGroupId = narrowHistoryFactory; return x; }).ToList();
                    changeTreeStructureGroupId(structureGroupIdList, STRUCTURE_CONSTANTS.STRUCTURE_GROUP.Location, narrowHistoryFactory);
                }

                dicList.Add("structureList", structureList);

                dicList["result"] = "1";    // 取得結果(正常終了)
                outList.Add(dicList);

                return 0;
            }
            catch (Exception ex)
            {
                logger.ErrorLog(belongingInfo.DutyFactoryInfo.StructureId.ToString(), userId, "", ex);
                // 取得結果(失敗)
                outList.Add(dicList);
                return -1;
            }
            finally
            {
                if (db != null)
                {
                    db.Close();
                }
            }
            // ツリーに異なる構成IDを指定してツリーの内容を制御する場合、この処理のより構成グループIDを置換しないと、表示されない
            void changeTreeStructureGroupId(List<int> structureGroupIdList, int target, int setValue)
            {
                structureGroupIdList[structureGroupIdList.IndexOf(target)] = setValue;
            }
        }

        /// <summary>
        /// keyに対応するメッセージを取得する
        /// </summary>
        /// <param name="key">翻訳ID</param>
        /// <param name="languageId">言語ID</param>
        /// <returns></returns>
        /// <remarks>旧メソッドはprivateに変更</remarks>
        private static string GetPropertiesMessageFile(string key, string languageId = "ja")
        {
            string message = "";

            System.Globalization.CultureInfo resourceCulture = System.Globalization.CultureInfo.GetCultureInfo(languageId);

            if (!string.IsNullOrEmpty(key))
            {
                message = Properties.Resources.ResourceManager.GetString(key, resourceCulture);

                // キーでメッセージを取得できない場合Nullとなるが、キーをメッセージとして返す
                if (string.IsNullOrEmpty(message))
                {
                    message = key;
                }
            }

            return message;
        }

        /// <summary>
        /// keyに対応するメッセージを取得する
        /// </summary>
        /// <param name="keys">翻訳ID</param>
        /// <param name="languageId">言語ID</param>
        /// <returns></returns>
        /// <remarks>旧メソッドはprivateに変更</remarks>
        private static string GetPropertiesMessageFile(string[] keys, string languageId = "ja")
        {

            string message = "";
            string[] wKeys = null;

            if (keys != null)
            {
                if (keys.Count() > 1)
                {
                    wKeys = new string[keys.Count() - 1];
                }

                // 翻訳取得
                for (int i = 0; i < keys.Count(); i++)
                {
                    if (i == 0)
                    {
                        message = GetPropertiesMessageFile(keys[i], languageId);
                    }
                    else
                    {
                        wKeys[i - 1] = GetPropertiesMessageFile(keys[i], languageId);
                    }
                }

                if (wKeys != null)
                {
                    message = string.Format(message, wKeys);
                }
            }

            return message;
        }

        /// <summary>
        /// リソースファイルから翻訳を取得する
        /// </summary>
        /// <param name="languageId">言語ID</param>
        /// <returns></returns>
        private static List<Dictionary<string, object>> GetPropertiesAllMessageFile(string languageId = "ja")
        {
            List<Dictionary<string, object>> resources = new List<Dictionary<string, object>>();
            Dictionary<string, object> dicRes = new Dictionary<string, object>();

            System.Globalization.CultureInfo resourceCulture = null;
            if (languageId.Equals("ja"))
            {
                resourceCulture = System.Globalization.CultureInfo.GetCultureInfo("ja_JP");
            }
            else if (languageId.Equals("en"))
            {
                resourceCulture = System.Globalization.CultureInfo.GetCultureInfo("en_US");
            }

            if (resourceCulture != null)
            {
                System.Resources.ResourceManager myResourceClass = new System.Resources.ResourceManager(typeof(Resources));
                System.Resources.ResourceSet resourceSet = myResourceClass.GetResourceSet(resourceCulture, true, true);
                foreach (System.Collections.DictionaryEntry entry in resourceSet)
                {
                    dicRes = new Dictionary<string, object>();
                    dicRes.Add(entry.Key.ToString(), entry.Value);
                    resources.Add(dicRes);
                }
            }

            return resources;
        }

        ///// <summary>
        ///// 条件オブジェクトの型変換を行う
        ///// </summary>
        ///// <param name="mappingList">マッピング情報リスト</param>
        ///// <param name="colInfoList">カラム情報リスト</param>
        ///// <param name="condition">更新対象条件オブジェクト</param>
        ///// <param name="convType">対象条件種類(検索条件/実行条件/検索結果)</param>
        //public static void ConvertColumnType(List<DBMappingInfo> mappingList, IList<DBColumnInfo> colInfoList, IDictionary<string, object> condition, ConvertType convType)
        //{
        //    try
        //    {
        //        // 条件を設定していない場合、そのまま処理を終わらせる
        //        if (condition == null) { return; }

        //        var dicResult = condition as IDictionary<string, object>;

        //        // カラム情報が取得できない場合、終了
        //        if (colInfoList == null || colInfoList.Count == 0) { return; }

        //        // マッピング情報をもとに、データ変換を行う
        //        foreach (var mapInfo in mappingList)
        //        {
        //            // カラム名が空の場合、スキップ
        //            if (string.IsNullOrEmpty(mapInfo.ColOrgName)) { continue; }

        //            string keyName = convType == ConvertType.Result ? mapInfo.ColName : mapInfo.ParamName;

        //            // パラメータ名が一致するレコードがない場合、スキップ
        //            if (!dicResult.ContainsKey(keyName)) { continue; }
        //            var dicVal = dicResult[keyName];

        //            // 値が入っていない場合、変換対象外
        //            if (CommonUtil.IsNullOrEmpty(dicVal)) { continue; }

        //            // カラム情報から型名を取得(この取得にはオリジナルのカラム名を使用する)
        //            var colType = colInfoList.FirstOrDefault(x => x.ColName.Equals(mapInfo.ColOrgName) &&
        //                (x.TypeName.Equals("numeric") || x.TypeName.Equals("timestamp") || x.TypeName.Equals("varchar")));
        //            // 該当カラムの型でない場合、スキップ
        //            if (CommonUtil.IsNullOrEmpty(colType)) { continue; }

        //            // カラムの型によって処理を分岐
        //            // ※数値型/日付型/文字列型のみ対応
        //            var typeName = colType.TypeName;

        //            try
        //            {
        //                var valText = dicVal.ToString();

        //                if (typeName.Equals("varchar") && convType == ConvertType.Search && mapInfo.LikePatternEnum != MatchPattern.ExactMatch)
        //                {
        //                    // 文字列型で検索条件の場合、マッチパターン指定
        //                    string param = valText;
        //                    switch (mapInfo.LikePatternEnum)
        //                    {
        //                        case MatchPattern.ForwardMatch: // 前方一致
        //                            param = param + "%";
        //                            break;
        //                        case MatchPattern.BackwardMatch:    // 後方一致
        //                            param = "%" + param;
        //                            break;
        //                        case MatchPattern.PartialMatch: // 部分一致
        //                            param = "%" + param + "%";
        //                            break;
        //                        default:    // 完全一致
        //                            break;
        //                    }
        //                    dicResult[keyName] = CommonUtil.GetMatchPatternText(valText, mapInfo.LikePatternEnum);
        //                    continue;
        //                }

        //                if (!valText.Contains(FromToDelimiter.ToString()))
        //                {
        //                    // デリミタで区切られていない場合
        //                    if (typeName.Equals("numeric"))
        //                    {
        //                        decimal num;
        //                        // 数値型変換
        //                        if (valText.Contains(NumberUnitDelimiter.ToString()))
        //                        {
        //                            var numVals = valText.Split(NumberUnitDelimiter);
        //                            num = decimal.Parse(numVals[0]);
        //                            dicResult.Add(keyName + "_Unit", numVals[1]);
        //                        }
        //                        else
        //                        {
        //                            num = decimal.Parse(valText);
        //                        }
        //                        if (string.IsNullOrEmpty(mapInfo.Format) || convType == ConvertType.Execute)
        //                        {
        //                            // フォーマット未指定または実行条件の場合、そのまま設定
        //                            dicResult[keyName] = num;
        //                        }
        //                        else
        //                        {
        //                            // フォーマット指定時はフォーマット変換して設定
        //                            dicResult[keyName] = num.ToString(mapInfo.Format);
        //                        }
        //                    }
        //                    else if (typeName.Equals("timestamp"))
        //                    {
        //                        // 日付型変換
        //                        var date = DateTime.Parse(valText);
        //                        if (string.IsNullOrEmpty(mapInfo.Format) || convType == ConvertType.Execute)
        //                        {
        //                            // フォーマット未指定または実行条件の場合、そのまま設定
        //                            dicResult[keyName] = date;
        //                        }
        //                        else
        //                        {
        //                            // フォーマット指定時はフォーマット変換して設定
        //                            dicResult[keyName] = date.ToString(mapInfo.Format);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    // デリミタで区切られている場合
        //                    if (typeName.Equals("numeric"))
        //                    {
        //                        // 数値型変換
        //                        var values = valText.Split(FromToDelimiter);
        //                        var list = new List<object>();
        //                        foreach (var val in values)
        //                        {
        //                            if (string.IsNullOrEmpty(val))
        //                            {
        //                                if (mapInfo.IsFromTo)
        //                                {
        //                                    list.Add(null);
        //                                }
        //                                continue;
        //                            }
        //                            // 数値型変換
        //                            if (val.ToString().Contains(NumberUnitDelimiter.ToString()))
        //                            {
        //                                var numVals = val.ToString().Split(NumberUnitDelimiter);
        //                                list.Add(decimal.Parse(numVals[0]));
        //                                dicResult.Add(keyName + "_Unit", numVals[1]);
        //                            }
        //                            else
        //                            {
        //                                list.Add(decimal.Parse(val));
        //                            }
        //                        }
        //                        dicResult[keyName] = list.ToArray();
        //                    }
        //                    else if (typeName.Equals("timestamp"))
        //                    {
        //                        // 日付型変換
        //                        var values = valText.Split(FromToDelimiter);
        //                        var list = new List<object>();
        //                        foreach (var val in values)
        //                        {
        //                            if (!string.IsNullOrEmpty(val))
        //                            {
        //                                var date = DateTime.Parse(val);
        //                                if (string.IsNullOrEmpty(mapInfo.Format) && convType != ConvertType.Execute)
        //                                {
        //                                    list.Add(date);
        //                                }
        //                                else
        //                                {
        //                                    // フォーマット指定時はフォーマット変換して設定
        //                                    list.Add(date.ToString(mapInfo.Format));
        //                                }
        //                            }
        //                            else
        //                            {
        //                                list.Add(null);
        //                            }
        //                        }
        //                        dicResult[keyName] = list.ToArray();
        //                    }
        //                    else if (typeName.Equals("varchar"))
        //                    {
        //                        // 文字列の場合
        //                        // 画面では日付項目だが、データベースが文字列などの場合
        //                        var values = valText.Split(FromToDelimiter);
        //                        var list = new List<object>();
        //                        foreach (var val in values)
        //                        {
        //                            if (string.IsNullOrEmpty(val))
        //                            {
        //                                if (mapInfo.IsFromTo)
        //                                {
        //                                    list.Add(null);
        //                                }
        //                                continue;
        //                            }
        //                            list.Add(val);
        //                        }
        //                        dicResult[keyName] = list.ToArray();
        //                    }
        //                }
        //            }
        //            catch
        //            {
        //                // 特に処理をしない
        //                continue;
        //            }

        //            // FromTo分割し、条件に設定
        //            if (convType != ConvertType.Result && mapInfo.IsFromTo)
        //            {
        //                SetFromToValue(ref condition, keyName);
        //            }

        //            // IN句のパラメータが配列でない場合に配列に変換する
        //            if (mapInfo.IsInClause && convType == ConvertType.Search)
        //            {
        //                ConvertToDBInParam(ref condition, keyName);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex.Message);
        //        throw;
        //    }
        //}

        /// <summary>
        /// 条件オブジェクトの型変換を行う
        /// </summary>
        /// <param name="mappingList">マッピング情報リスト</param>
        /// <param name="colInfoList">カラム情報リスト</param>
        /// <param name="condition">更新対象条件オブジェクト</param>
        /// <param name="convType">対象条件種類(検索条件/実行条件/検索結果)</param>
        public static void ConvertColumnType(List<DBMappingInfo> mappingList, IDictionary<string, object> condition, ConvertType convType)
        {
            try
            {
                // 条件を設定していない場合、そのまま処理を終わらせる
                if (condition == null) { return; }

                var dicResult = condition as IDictionary<string, object>;

                // マッピング情報をもとに、データ変換を行う
                foreach (var mapInfo in mappingList)
                {
                    // カラム名が空の場合、スキップ
                    if (string.IsNullOrEmpty(mapInfo.ColOrgName)) { continue; }

                    string keyName = convType == ConvertType.Result ? mapInfo.ColName : mapInfo.ParamName;

                    // パラメータ名が一致するレコードがない場合、スキップ
                    if (!dicResult.ContainsKey(keyName)) { continue; }
                    var dicVal = dicResult[keyName];

                    // 値が入っていない場合、変換対象外
                    if (CommonUtil.IsNullOrEmpty(dicVal)) { continue; }

                    try
                    {
                        var valText = dicVal.ToString();
                        if (mapInfo.DataType == DBColumnDataType.String && convType == ConvertType.Search && mapInfo.LikePatternEnum != MatchPattern.ExactMatch)
                        {
                            // 文字列型で検索条件の場合、マッチパターン指定
                            dicResult[keyName] = CommonUtil.GetMatchPatternText(valText, mapInfo.LikePatternEnum);
                            continue;
                        }

                        if (!valText.Contains(FromToDelimiter.ToString()))
                        {
                            // デリミタで区切られていない場合
                            if (mapInfo.DataType == DBColumnDataType.Decimal)
                            {
                                decimal num;
                                // 数値型変換
                                if (valText.Contains(NumberUnitDelimiter.ToString()))
                                {
                                    var numVals = valText.Split(NumberUnitDelimiter);
                                    num = decimal.Parse(numVals[0]);
                                    dicResult.Add(keyName + "_Unit", numVals[1]);
                                }
                                else
                                {
                                    num = decimal.Parse(valText);
                                }
                                if (string.IsNullOrEmpty(mapInfo.Format) || convType == ConvertType.Execute)
                                {
                                    // フォーマット未指定または実行条件の場合、そのまま設定
                                    dicResult[keyName] = num;
                                }
                                else
                                {
                                    // フォーマット指定時はフォーマット変換して設定
                                    dicResult[keyName] = num.ToString(mapInfo.Format);
                                }
                            }
                            else if (mapInfo.DataType == DBColumnDataType.DateTime)
                            {
                                // 日付型変換
                                var date = DateTime.Parse(valText);
                                if (string.IsNullOrEmpty(mapInfo.Format) || convType == ConvertType.Execute)
                                {
                                    // フォーマット未指定または実行条件の場合、そのまま設定
                                    dicResult[keyName] = date;
                                }
                                else
                                {
                                    // フォーマット指定時はフォーマット変換して設定
                                    dicResult[keyName] = date.ToString(mapInfo.Format);
                                }
                            }
                        }
                        else
                        {
                            // デリミタで区切られている場合
                            if (mapInfo.DataType == DBColumnDataType.Decimal)
                            {
                                // 数値型変換
                                var values = valText.Split(FromToDelimiter);
                                var list = new List<object>();
                                foreach (var val in values)
                                {
                                    if (string.IsNullOrEmpty(val))
                                    {
                                        if (mapInfo.IsFromTo)
                                        {
                                            list.Add(null);
                                        }
                                        continue;
                                    }
                                    // 数値型変換
                                    if (val.ToString().Contains(NumberUnitDelimiter.ToString()))
                                    {
                                        var numVals = val.ToString().Split(NumberUnitDelimiter);
                                        list.Add(decimal.Parse(numVals[0]));
                                        dicResult.Add(keyName + "_Unit", numVals[1]);
                                    }
                                    else
                                    {
                                        list.Add(decimal.Parse(val));
                                    }
                                }
                                dicResult[keyName] = list.ToArray();
                            }
                            else if (mapInfo.DataType == DBColumnDataType.DateTime)
                            {
                                // 日付型変換
                                var values = valText.Split(FromToDelimiter);
                                var list = new List<object>();
                                foreach (var val in values)
                                {
                                    if (!string.IsNullOrEmpty(val))
                                    {
                                        var date = DateTime.Parse(val);
                                        if (string.IsNullOrEmpty(mapInfo.Format) && convType != ConvertType.Execute)
                                        {
                                            list.Add(date);
                                        }
                                        else
                                        {
                                            // フォーマット指定時はフォーマット変換して設定
                                            list.Add(date.ToString(mapInfo.Format));
                                        }
                                    }
                                    else
                                    {
                                        list.Add(null);
                                    }
                                }
                                dicResult[keyName] = list.ToArray();
                            }
                            else if (mapInfo.DataType == DBColumnDataType.String)
                            {
                                // 文字列の場合
                                // 画面では日付項目だが、データベースが文字列などの場合
                                var values = valText.Split(FromToDelimiter);
                                var list = new List<object>();
                                foreach (var val in values)
                                {
                                    if (string.IsNullOrEmpty(val))
                                    {
                                        if (mapInfo.IsFromTo)
                                        {
                                            list.Add(null);
                                        }
                                        continue;
                                    }
                                    list.Add(val);
                                }
                                dicResult[keyName] = list.ToArray();
                            }
                        }
                    }
                    catch
                    {
                        // 特に処理をしない
                        continue;
                    }

                    // FromTo分割し、条件に設定
                    if (convType != ConvertType.Result && mapInfo.IsFromTo)
                    {
                        SetFromToValue(ref condition, keyName);
                    }

                    // IN句のパラメータが配列でない場合に配列に変換する
                    if (mapInfo.IsInClause && convType == ConvertType.Search)
                    {
                        ConvertToDBInParam(ref condition, keyName);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.ErrorLog("", "", "", ex);
                throw;
            }
        }

        //public static void ConvertColumnType(DBMappingInfo mapInfo, string valText, string keyName, IDictionary<string, object> condition, ConvertType convType)
        public static void ConvertColumnType(DBMappingInfo mapInfo, short dataType, string valText, string keyName, IDictionary<string, object> condition, ConvertType convType)
        {
            if (dataType == DBColumnDataType.String && convType == ConvertType.Search && mapInfo.LikePatternEnum != MatchPattern.ExactMatch)
            {
                // 文字列型で検索条件の場合、マッチパターン指定
                condition[keyName] = CommonUtil.GetMatchPatternText(valText, mapInfo.LikePatternEnum);
                return;
            }

            if (!valText.Contains(FromToDelimiter.ToString()))
            {
                // デリミタで区切られていない場合
                if (dataType == DBColumnDataType.Int)
                {
                    int num;
                    // 数値型変換
                    if (valText.Contains(NumberUnitDelimiter.ToString()))
                    {
                        var numVals = valText.Split(NumberUnitDelimiter);
                        num = int.Parse(numVals[0]);
                        condition.Add(keyName + "_Unit", numVals[1]);
                    }
                    else
                    {
                        num = int.Parse(valText);
                    }
                    if (string.IsNullOrEmpty(mapInfo.Format) || convType == ConvertType.Execute)
                    {
                        // フォーマット未指定または実行条件の場合、そのまま設定
                        condition[keyName] = num;
                    }
                    else
                    {
                        // フォーマット指定時はフォーマット変換して設定
                        condition[keyName] = num.ToString(mapInfo.Format);
                    }
                }
                else if (dataType == DBColumnDataType.Decimal)
                {
                    decimal num;
                    // 数値型変換
                    if (valText.Contains(NumberUnitDelimiter.ToString()))
                    {
                        var numVals = valText.Split(NumberUnitDelimiter);
                        num = decimal.Parse(numVals[0]);
                        condition.Add(keyName + "_Unit", numVals[1]);
                    }
                    else
                    {
                        num = decimal.Parse(valText);
                    }
                    if (string.IsNullOrEmpty(mapInfo.Format))
                    {
                        // フォーマット未指定の場合、そのまま設定
                        condition[keyName] = num;
                    }
                    else
                    {
                        // フォーマット指定時はフォーマット変換
                        var formatedVal = num.ToString(mapInfo.Format);
                        if (convType == ConvertType.Execute)
                        {
                            // 実行条件の場合、再度数値に変換して設定
                            condition[keyName] = decimal.Parse(formatedVal);
                        }
                        else
                        {
                            // 実行条件以外はフォーマット変換後の文字列を設定
                            condition[keyName] = formatedVal;
                        }
                    }
                }
                else if (dataType == DBColumnDataType.DateTime)
                {
                    // 日付型変換
                    var date = DateTime.Parse(valText);
                    if (string.IsNullOrEmpty(mapInfo.Format))
                    {
                        // フォーマット未指定の場合、そのまま設定
                        condition[keyName] = date;
                    }
                    else
                    {
                        // フォーマット指定時はフォーマット変換
                        var formatedDate = date.ToString(mapInfo.Format);
                        if (convType == ConvertType.Execute)
                        {
                            // 実行条件の場合、再度日付に変換して設定
                            condition[keyName] = DateTime.Parse(formatedDate);
                        }
                        else
                        {
                            // 実行条件以外はフォーマット変換後の文字列を設定
                            condition[keyName] = formatedDate;
                        }
                    }
                }
            }
            else
            {
                // デリミタで区切られている場合
                if (dataType == DBColumnDataType.Int)
                {
                    // 数値型変換
                    var values = valText.Split(FromToDelimiter);
                    var list = new List<object>();
                    foreach (var val in values)
                    {
                        if (string.IsNullOrEmpty(val))
                        {
                            if (mapInfo.IsFromTo)
                            {
                                list.Add(null);
                            }
                            continue;
                        }
                        // 数値型変換
                        if (val.ToString().Contains(NumberUnitDelimiter.ToString()))
                        {
                            var numVals = val.ToString().Split(NumberUnitDelimiter);
                            list.Add(int.Parse(numVals[0]));
                            condition.Add(keyName + "_Unit", numVals[1]);
                        }
                        else
                        {
                            list.Add(int.Parse(val));
                        }
                    }
                    condition[keyName] = list.ToArray();
                }
                else if (dataType == DBColumnDataType.Decimal)
                {
                    // 数値型変換
                    var values = valText.Split(FromToDelimiter);
                    var list = new List<object>();
                    foreach (var val in values)
                    {
                        if (string.IsNullOrEmpty(val))
                        {
                            if (mapInfo.IsFromTo)
                            {
                                list.Add(null);
                            }
                            continue;
                        }
                        // 数値型変換
                        if (val.ToString().Contains(NumberUnitDelimiter.ToString()))
                        {
                            var numVals = val.ToString().Split(NumberUnitDelimiter);
                            list.Add(decimal.Parse(numVals[0]));
                            condition.Add(keyName + "_Unit", numVals[1]);
                        }
                        else
                        {
                            list.Add(decimal.Parse(val));
                        }
                    }
                    condition[keyName] = list.ToArray();
                }
                else if (dataType == DBColumnDataType.DateTime)
                {
                    // 日付型変換
                    var values = valText.Split(FromToDelimiter);
                    var list = new List<object>();
                    foreach (var val in values)
                    {
                        if (!string.IsNullOrEmpty(val))
                        {
                            var date = DateTime.Parse(val);
                            if (string.IsNullOrEmpty(mapInfo.Format))
                            {
                                // フォーマット未指定の場合、そのまま設定
                                list.Add(date);
                            }
                            else
                            {
                                // フォーマット指定時はフォーマット変換
                                var formatedDate = date.ToString(mapInfo.Format);
                                if (convType == ConvertType.Execute)
                                {
                                    // 実行条件の場合、再度日付に変換して設定
                                    list.Add(DateTime.Parse(formatedDate));
                                }
                                else
                                {
                                    // 実行条件以外はフォーマット変換後の文字列を設定
                                    list.Add(formatedDate);
                                }
                            }
                        }
                        else
                        {
                            list.Add(null);
                        }
                    }
                    condition[keyName] = list.ToArray();
                }
                else if (dataType == DBColumnDataType.String)
                {
                    // 文字列の場合
                    // 画面では日付項目だが、データベースが文字列などの場合
                    var values = valText.Split(FromToDelimiter);
                    var list = new List<object>();
                    foreach (var val in values)
                    {
                        if (string.IsNullOrEmpty(val))
                        {
                            if (mapInfo.IsFromTo)
                            {
                                list.Add(null);
                            }
                            continue;
                        }
                        list.Add(val);
                    }
                    condition[keyName] = list.ToArray();
                }
            }
        }

        /// <summary>
        /// 指定クラスのプロパティの型に値を変換して設定する
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="prop">プロパティ情報</param>
        /// <param name="target">対象クラス変数</param>
        /// <param name="val">設定値</param>
        public static void SetPropertyValue<T>(PropertyInfo prop, T target, object val)
        {
            // Nullの場合
            if (CommonUtil.IsNullOrEmpty(val))
            {
                setNull();
                return;
            }
            // 文字列の場合そのままセットして終了
            if (prop.PropertyType == typeof(string))
            {
                prop.SetValue(target, val.ToString());
                return;
            }
            // 空白の場合もNullと同様に終了
            if (string.IsNullOrWhiteSpace(val.ToString()))
            {
                setNull();
                return;
            }
            // Nullをセットする処理
            void setNull()
            {
                if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) { prop.SetValue(target, null); }
            }

            if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
            {
                prop.SetValue(target, Convert.ToInt32(val));
            }
            else if (prop.PropertyType == typeof(short) || prop.PropertyType == typeof(short?))
            {
                prop.SetValue(target, Convert.ToInt16(val));
            }
            else if (prop.PropertyType == typeof(byte) || prop.PropertyType == typeof(byte?))
            {
                prop.SetValue(target, Convert.ToByte(val));
            }
            else if (prop.PropertyType == typeof(long) || prop.PropertyType == typeof(long?))
            {
                prop.SetValue(target, Convert.ToInt64(val));
            }
            else if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
            {
                prop.SetValue(target, Convert.ToDecimal(val));
            }
            else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
            {
                prop.SetValue(target, Convert.ToDateTime(val));
            }
            else if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
            {
                prop.SetValue(target, Convert.ToBoolean(val));
            }
            else
            {
                prop.SetValue(target, val);
            }
        }

        /// <summary>
        /// 日付フォーマットを行う
        /// </summary>
        /// <param name="conditionObj">条件</param>
        /// <param name="dateFormat">フォーマット形式</param>
        public static void FormatDate(ref dynamic conditionObj, Dictionary<string, string> dateFormat, string keyInfo = null)
        {
            foreach (var dateInfo in dateFormat)
            {
                var dic = conditionObj as IDictionary<string, object>;
                if (!dic.ContainsKey(dateInfo.Key)) { continue; }

                // キー情報が指定されている場合、キー情報と一致しない場合、処理をスキップ
                if (!string.IsNullOrEmpty(keyInfo) && !dateInfo.Key.Equals(keyInfo)) { continue; }
                if (dic[dateInfo.Key] is DateTime?[])
                {
                    var dataInfo1 = dic[dateInfo.Key] as DateTime?[];
                    var dataInfo2 = new string[dataInfo1.Length];
                    for (int i = 0; i < dataInfo1.Count(); i++)
                    {
                        if (!string.IsNullOrEmpty(dataInfo1[i].ToString()))
                        {
                            dataInfo2[i] = dataInfo1[i]?.ToString(dateInfo.Value);
                        }
                        else
                        {
                            dataInfo2[i] = "";
                        }
                    }
                    dic[dateInfo.Key] = dataInfo2;
                }
                else if (dic[dateInfo.Key] is DateTime?)
                {
                    var dateInfo1 = dic[dateInfo.Key] as DateTime?;
                    var dateInfo2 = dateInfo1 != null ? dateInfo1?.ToString(dateInfo.Value) : "";
                    dic[dateInfo.Key] = dateInfo2;
                }
                // キー情報が設定されている場合、処理を返す
                if (!string.IsNullOrEmpty(keyInfo) && dateInfo.Key.Equals(keyInfo)) { return; }
            }
        }

        /// <summary>
        /// FromTo分割し、条件に設定
        /// </summary>
        /// <param name="conditionObj"></param>
        /// <param name="nameList"></param>
        public static void SetFromToValue(ref IDictionary<string, object> dic, string name)
        {
            if (!dic.ContainsKey(name)) { return; }

            var val = dic[name];
            if (val == null || !(val is Array)) { return; }

            var values = val as Array;
            for (int i = 0; i < values.Length; i++)
            {
                if (i == 0)
                {
                    dic[name + "From"] = values.GetValue(i);
                }
                else
                {
                    dic[name + "To"] = values.GetValue(i);
                    break;
                }
            }
            // 分割後、元のキーを削除する
            dic.Remove(name);
        }

        /// <summary>
        /// IN句のパラメータが配列でない場合に配列に変換する
        /// </summary>
        /// <param name="conditionObj"></param>
        /// <param name="nameList"></param>
        public static void ConvertToDBInParam(ref IDictionary<string, object> dic, string name)
        {
            if (!dic.ContainsKey(name)) { return; }

            var val = dic[name];
            if (CommonUtil.IsNullOrEmpty(val) || val is Array) { return; }

            // IN句のパラメータが配列でない場合に配列に格納してセットし直す
            dic[name] = new object[] { val };
        }

        /// <summary>
        /// Pascal、CamelケースをSnakeケースに変換する
        /// </summary>
        /// <param name="str">対象文字列</param>
        /// <returns>Snakeケース文字列</returns>
        public static string GetSnakeCase(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                var regex = new System.Text.RegularExpressions.Regex("[a-z][A-Z]");
                return regex.Replace(str, s => $"{s.Groups[0].Value[0]}_{s.Groups[0].Value[1]}").ToLower();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// SnakeケースをPascalケースに変換する
        /// </summary>
        /// <param name="str">対象文字列</param>
        /// <returns>Pascalケース文字列</returns>
        public static string SnakeCaseToPascalCase(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                var regex = new Regex(@"\b[a-z]");
                return regex.Replace(str.Replace("_", " "), s => s.Value.ToUpper()).Replace(" ", "");
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 文字列をdecimal型に変換
        /// </summary>
        /// <param name="str">対象文字列</param>
        /// <returns>decimal数値</returns>
        /// <remarks>文字列がnullもしくは数値変換不可の場合はnullを返す</remarks>
        public static decimal? ConvertDecimal(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            string num = str.Replace(",", "");
            decimal? res = null;
            try
            {
                res = decimal.Parse(num);
            }
            catch
            {
                // 有効な数値文字列でない場合
                res = null;
            }

            return res;
        }

        /// <summary>
        /// 文字列をdecimal型に変換
        /// </summary>
        /// <param name="str">対象文字列</param>
        /// <returns>decimal数値</returns>
        /// <remarks>文字列がnullもしくは数値変換不可の場合はnullを返す</remarks>
        public static decimal ConvertDecimalToZero(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return decimal.Zero;
            }

            string num = str.Replace(",", "");
            decimal res = decimal.Zero;
            try
            {
                res = decimal.Parse(num);
            }
            catch
            {
                // 有効な数値文字列でない場合
                res = decimal.Zero;
            }

            return res;
        }

        /// <summary>
        /// 文字列をDateTime型に変換
        /// </summary>
        /// <param name="str">対象文字列</param>
        /// <returns>DateTime型</returns>
        /// <remarks>文字列がnullもしくは日付変換不可の場合はnullを返す</remarks>
        public static DateTime? ConvertDateTime(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            DateTime dateTime = DateTime.Now;
            try
            {
                dateTime = DateTime.Parse(str);
            }
            catch
            {
                return null;
            }
            return dateTime;
        }

        /// <summary>
        /// 文字列(yyyyMMdd)をDateTime型に変換
        /// </summary>
        /// <param name="ymd">yyyyMMddの文字列</param>
        /// <remarks>文字列がnullもしくは日付変換不可の場合はnullを返す</remarks>
        public static DateTime? ConvertDateTimeFromYyyymmddString(string ymd)
        {
            string formatYmd = ymd.Substring(0, 4) + "/" + ymd.Substring(4, 2) + "/" + ymd.Substring(6, 2);
            return ConvertDateTime(formatYmd);
        }

        /// <summary>
        /// 日付型をyyyyMMdd形式の文字列に変換
        /// </summary>
        /// <param name="date">日付</param>
        /// <returns>日付をyyyyMMdd形式に変換した文字列(日付がNullなら空文字列)</returns>
        public static string ConvertDatetimeToYmdString(DateTime? date)
        {
            if (date == null)
            {
                // Nullの場合は空文字列を返す
                return string.Empty;
            }
            return string.Format("{0:yyyyMMdd}", date);
        }

        /// <summary>
        /// 日付型をyyyyMMdd形式の数値に変換
        /// </summary>
        /// <param name="date">日付</param>
        /// <returns>日付をyyyyMMdd形式に変換した数値(日付がNullならNull)</returns>
        public static int? ConvertDatetimeToYmdInt(DateTime? date)
        {
            if (date == null)
            {
                // Nullの場合はNullを返す
                return null;
            }
            return int.Parse(ConvertDatetimeToYmdString(date));
        }

        /// <summary>
        /// Null許容のdecimalをString型に変換
        /// </summary>
        /// <param name="value">Null許容のdecimalの値</param>
        /// <returns>Nullの場合は空文字の文字列</returns>
        public static string ConvertDecimalToString(decimal? value)
        {
            if (value == null)
            {
                // Nullの場合、空文字
                return string.Empty;
            }
            // Nullでない場合、文字列に変換
            return ((decimal)value).ToString();
        }

        /// <summary>
        /// 文字列をIntに変換、変換不能な場合はNull
        /// </summary>
        /// <param name="value">変換する文字列</param>
        /// <returns>Intの値、変換不能な場合はNull</returns>
        public static int? ConvertStringToInt(string value)
        {
            if (int.TryParse(value, out int i))
            {
                // 変換可能な場合
                return (int?)i;
            }
            else
            {
                // 変換不能な場合
                return null;
            }
        }

        /// <summary>
        /// 与えられた日付の月の最終日の日付を返す処理
        /// </summary>
        /// <param name="date">最終日を求める日付</param>
        /// <returns>最終日の日付</returns>
        /// <remarks>2022/1/1なら2022/1/31を返す</remarks>
        public static DateTime GetDateMonthLastDay(DateTime date)
        {
            var lastDay = DateTime.DaysInMonth(date.Year, date.Month);
            var result = new DateTime(date.Year, date.Month, lastDay);
            return result;
        }

        /// <summary>
        /// 与えられた日付の年の最終日の日付を返す処理
        /// </summary>
        /// <param name="date">最終日を求める日付</param>
        /// <returns>2022/01/01なら2022/12/31を返す</returns>
        public static DateTime GetDateYearLastDay(DateTime date)
        {
            // 年末は12月31日
            var result = new DateTime(date.Year, 12, 31);
            return result;
        }

        /// <summary>
        /// 与えられた年度の年度最終日を返す処理
        /// </summary>
        /// <param name="year">対象年</param>
        /// <param name="monthStartNendo">年度開始月</param>
        /// <returns>年度最終日</returns>
        public static DateTime GetNendoLastDay(int year, int monthStartNendo)
        {
            // 年度開始日
            DateTime startDate = new DateTime(year, monthStartNendo, 1);
            // 開始日 +1年 -1日 2022/04/01 → 2023/04/01 → 2023/03/31
            DateTime result = startDate.AddYears(1).AddDays(-1);
            return result;
        }

        /// <summary>
        /// 与えられた日付の年度最終日を返す処理
        /// </summary>
        /// <param name="target">取得したい日付</param>
        /// <param name="monthStartNendo">年度開始月</param>
        /// <returns>年度最終日</returns>
        public static DateTime GetNendoLastDay(DateTime target, int monthStartNendo)
        {
            // 年度開始日
            DateTime start = GetNendoStartDay(target, monthStartNendo);
            DateTime last = GetNendoLastDay(start.Year, monthStartNendo);
            return last;
        }

        /// <summary>
        /// 与えられた日付の年度開始日を取得する処理
        /// </summary>
        /// <param name="target">取得したい日付</param>
        /// <param name="monthStartNendo">年度開始月</param>
        /// <returns>年度開始日</returns>
        public static DateTime GetNendoStartDay(DateTime target, int monthStartNendo)
        {
            // 月の場合は計算
            int startMonth = monthStartNendo; // 年度開始月
            int backMonths = (startMonth - 1) * -1; // 年月日から引く月数(4月なら-3)
            int nendo = target.AddMonths(backMonths).Year;  // 年月日から上記月数を引いた年(2022-03-01 - 3か月 = 2021-12-01 → 2021)
            DateTime result = new DateTime(nendo, monthStartNendo, 1);
            return result;
        }

        /// <summary>
        /// 与えられた日付の半期開始日を取得する処理
        /// </summary>
        /// <param name="target">取得したい日付</param>
        /// <param name="monthStartNendo">年度開始月</param>
        /// <returns>年度開始日</returns>
        public static DateTime GetNendoHalfPeriodStartDay(DateTime target, int monthStartNendo)
        {
            // 下期のフラグ
            bool secondHalfFlag = false;
            // 対象の月 から 年度開始月 を引いて上期か下期を判定
            int monthDiff = target.Month - monthStartNendo;
            if (monthDiff < 0)
            {
                monthDiff = monthDiff + 12;
            }
            // 対象月が年度開始月から６か月以上異なる場合
            if (monthDiff >= 6)
            {
                // 下期のフラグをtrue
                secondHalfFlag = true;
            }
            // 年度開始日を取得
            DateTime result = GetNendoStartDay(target, monthStartNendo);
            // 下期の場合
            if (secondHalfFlag == true)
            {
                // ６か月加算
                result = result.AddMonths(6);
            }
            return result;
        }

        /// <summary>
        /// 二つの年月日の月数の差を求める処理(日は考慮しない)
        /// </summary>
        /// <param name="date1">大きい方の日付</param>
        /// <param name="date2">小さいほうの日付</param>
        /// <returns>月数の差</returns>
        /// <remarks>小さいほうが大きい場合、マイナスを返す</remarks>
        public static int GetMonthDiff(DateTime date1, DateTime date2)
        {
            int diff = ((date1.Year - date2.Year) * 12) + date1.Month - date2.Month;
            return diff;
        }

        /// <summary>
        /// ファイル圧縮
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="zipFilePath"></param>
        /// <returns></returns>
        public static bool FileZip(string filePath, string zipFilePath, string password)
        {
            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(Encoding.GetEncoding("shift_jis")))
            {
                // 圧縮レベル
                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;

                if (!string.IsNullOrEmpty(password))
                {
                    zip.Password = password;
                }

                // ファイルやディレクトリをZIPアーカイブに追加
                var isDirectry = File.GetAttributes(filePath).HasFlag(FileAttributes.Directory);
                if (isDirectry)
                {
                    // フォルダの場合
                    zip.AddDirectory(filePath, "");
                }
                else
                {
                    // ファイルの場合
                    zip.AddFile(filePath, "");
                }

                // 保存
                zip.Save(zipFilePath);
            }
            return true;
        }

        /// <summary>
        /// ファイル解凍
        /// </summary>
        /// <param name="zipFilePath"></param>
        /// <param name="unZipDir"></param>
        /// <returns></returns>
        public static bool FileUnZip(string zipFilePath, string unZipDir, string password)
        {
            using (var zip = new Ionic.Zip.ZipFile(zipFilePath))
            {

                if (!string.IsNullOrEmpty(password))
                {
                    zip.Password = password;
                }
                // 一括で指定パスに解凍
                zip.ExtractAll(unZipDir);

                // 全エントリーを走査して個別解凍
                foreach (var entry in zip.Entries)
                {
                    entry.Extract();
                }
            }

            return true;
        }

        /// <summary>
        /// 埋め込みリソースのテキストファイルから文字列を取得
        /// </summary>
        /// <param name="assemblyName">アセンブリ名</param>
        /// <param name="resourceName">埋め込みリソースファイル名([アセンブリ名].[ディレクトリパス(「.」区切り)].[ファイル名]または論理名)</param>
        /// <param name="result">取得した文字列</param>
        /// <param name="encoding">文字コード(デフォルト値：UTF-8)</param>
        /// <returns>取得結果(true:取得OK/false:取得NG )</returns>
        public static bool GetEmbeddedResourceStr(string assemblyName, string resourceName, out string result, string encoding = "UTF-8")
        {
            result = string.Empty;
            // 読込対象のアセンブリを取得
            Assembly asm = Assembly.Load(assemblyName);
            if (asm == null)
            {
                throwNotFoundException();
            }

            // 埋め込みリソースを読込
            var names = asm.GetManifestResourceNames();
            using (var stream = asm.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throwNotFoundException();
                }

                // ファイル内容を読込
                using (var sr = new StreamReader(stream, Encoding.GetEncoding(encoding)))
                {
                    result = sr.ReadToEnd();
                }
            }
            return true;

            // 例外スロー処理
            void throwNotFoundException()
            {
                throw new Exception("AssemblyName:" + assemblyName + ",ResourceName:" + resourceName + " is not found.");
            }
        }

        /// <summary>
        /// メッセージリソースをデータベースより取得
        /// </summary>
        /// <param name="db">データベース接続</param>
        /// <param name="languageId">取得する言語、省略した場合は全て</param>
        /// <param name="messageIdList">取得するメッセージID、省略した場合は全て</param>
        /// <returns>取得したメッセージリソース</returns>
        public static MessageResources GetMessageResourceFromDb(ComDB db, string languageId = "", string[] messageIdList = null, List<int> factoryIdList = null)
        {
            // メッセージIDのリストは配列でなくListで渡すため変換
            // 省略時(全件取得)は、空(Count=0)のリスト
            List<string> paramMessageIdList = new List<string>();
            if (messageIdList != null)
            {
                // 指定したメッセージIDのみのリスト
                paramMessageIdList = new List<string>(messageIdList);
            }
            List<int> paramFactoryIdList = new List<int>();
            if (factoryIdList != null)
            {
                paramFactoryIdList = factoryIdList;
                if (!paramFactoryIdList.Contains(STRUCTURE_CONSTANTS.CommonFactoryId))
                {
                    paramFactoryIdList.Add(STRUCTURE_CONSTANTS.CommonFactoryId);
                }
            }
            // DBより取得
            var getMessageList = db.GetListByOutsideSql<MessageResources.Translation>(SqlName.GetTranslation, SqlName.SubDir, new { LanguageId = languageId, MessageIdList = paramMessageIdList, FactoryIdList = paramFactoryIdList });
            // メッセージリソースクラスを宣言して返す
            return new MessageResources(getMessageList);
        }

        /// <summary>
        /// メッセージリソースをデータベースより取得
        /// </summary>
        /// <param name="db">データベース接続</param>
        /// <param name="languageId">取得する言語、省略した場合は全て</param>
        /// <returns>取得したメッセージリソース</returns>
        public static List<Dictionary<string, object>> GetMessageResourceFromDbVerDic(ComDB db, string languageId = "", string[] messageIdList = null, List<int> factoryIdList = null)
        {
            List<Dictionary<string, object>> resources = new List<Dictionary<string, object>>();
            Dictionary<string, object> dicRes = new Dictionary<string, object>();

            // メッセージIDのリストは配列でなくListで渡すため変換
            // 省略時(全件取得)は、空(Count=0)のリスト
            List<string> paramMessageIdList = new List<string>();
            if (messageIdList != null)
            {
                // 指定したメッセージIDのみのリスト
                paramMessageIdList = new List<string>(messageIdList);
            }

            List<int> paramFactoryIdList = new List<int>();
            if (factoryIdList != null)
            {
                paramFactoryIdList = factoryIdList;
                if (!paramFactoryIdList.Contains(STRUCTURE_CONSTANTS.CommonFactoryId))
                {
                    paramFactoryIdList.Add(STRUCTURE_CONSTANTS.CommonFactoryId);
                }
            }
            // DBより取得
            IList<MessageResources.Translation> messageList = db.GetListByOutsideSql<MessageResources.Translation>(SqlName.GetTranslation, SqlName.SubDir, new { LanguageId = languageId, MessageIdList = paramMessageIdList, FactoryIdList = paramFactoryIdList });
            for (int i = 0; i < messageList.Count; i++)
            {
                dicRes = new Dictionary<string, object>();
                dicRes.Add(messageList[i].messageId, messageList[i].value);
                resources.Add(dicRes);
            }

            // メッセージリソースクラスを宣言して返す
            return resources;
        }

        /// <summary>
        /// 第一引数がNullまたは空文字なら第二引数、そうでなければ第一引数を返す
        /// </summary>
        /// <param name="checkValue">Null(空文字)かどうか判定する値</param>
        /// <param name="nullReturnValue">第一引数がNull(空文字)ならこちらの値を返す</param>
        /// <returns>第一引数、Null(空文字)の場合は第二引数</returns>
        /// <remarks>PostgreSQLのCoalesce、OracleのNVLのイメージ</remarks>
        public static string getNullValueLogicString(string checkValue, string nullReturnValue)
        {
            if (string.IsNullOrEmpty(checkValue))
            {
                // Null(空文字)の場合、第二引数
                return nullReturnValue;
            }
            // Null(空文字)でない場合、第一引数
            return checkValue;
        }

        /// <summary>
        /// 検索条件を再設定
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static dynamic SetCondition(Dictionary<string, string> condition)
        {
            dynamic searchCondition = new ExpandoObject();
            if (condition != null)
            {
                foreach (var item in condition)
                {
                    ((IDictionary<string, object>)searchCondition).Add(item.Key, item.Value);
                }
            }
            return searchCondition;
        }

        /// <summary>
        /// JSON文字列をディクショナリに変換する処理
        /// </summary>
        /// <typeparam name="T">ディクショナリの値の型</typeparam>
        /// <param name="json">JSON文字列</param>
        /// <returns>変換したディクショナリ</returns>
        public static List<Dictionary<string, T>> DeserializeJsonStrToDictionaryList<T>(string json)
        {
            var list = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(json);
            var resultList = new List<Dictionary<string, T>>();
            foreach (var dic in list)
            {
                var newDic = new Dictionary<string, T>();
                foreach (var item in dic)
                {
                    newDic.Add(item.Key, JsonSerializer.Deserialize<T>(item.Value.ToString()));
                }
                resultList.Add(newDic);
            }
            return resultList;
        }

        /// <summary>
        /// Dictionaryリスト用ユーティリティクラス
        /// </summary>
        /// <remarks>resultInfoDictionaryやsearchConditionDictionaryの型List<Dictionary<string,object>>がややこしいので簡略化したい</remarks>
        public class DictionaryList
        {
            /// <summary>
            /// ディクショナリのリスト
            /// </summary>
            public List<Dictionary<string, object>> Value { get; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="dictionaryList">利用するディクショナリリストの値</param>
            public DictionaryList(List<Dictionary<string, object>> dictionaryList)
            {
                // 複製でなくそのまま参照するので、このクラスでの変更は反映される
                this.Value = dictionaryList;
            }

            /// <summary>
            /// ctrlIdを指定して要素を取得
            /// </summary>
            /// <param name="ctrlId">項目ID</param>
            /// <returns>Listから取得した要素</returns>
            public Dictionary<string, object> getMember(string ctrlId)
            {
                return GetDictionaryByCtrlId(this.Value, ctrlId);
            }
        }

        /// <summary>
        /// ディクショナリに指定されたキーが存在するかチェックする処理
        /// </summary>
        /// <param name="targetDictionary">チェックするディクショナリ</param>
        /// <param name="keyList">チェックするキーのリスト</param>
        /// <returns>存在しない場合、存在しても値がNullの場合はFalse</returns>
        private static bool isExistsKeysDictionary(Dictionary<string, object> targetDictionary, List<string> keyList)
        {
            foreach (var key in keyList)
            {
                if (!targetDictionary.ContainsKey(key))
                {
                    return false;
                }
                if (targetDictionary[key] == null)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 画面のコントロールIDで対象のディクショナリを取得
        /// </summary>
        /// <param name="dictionaryList">ディクショナリのリスト、searchConditionDictionaryやresultInfoDictionary</param>
        /// <param name="ctrlId">取得対象の画面のコントロールID</param>
        /// <returns>対象のディクショナリ</returns>
        public static Dictionary<string, object> GetDictionaryByCtrlId(List<Dictionary<string, object>> dictionaryList, string ctrlId)
        {
            // 先頭
            return dictionaryList.Where(x => isExistsKeysDictionary(x, new List<string> { "CTRLID" })).FirstOrDefault(x => ctrlId.Equals(x["CTRLID"].ToString()));
        }

        /// <summary>
        /// 画面のコントロールIDで対象のディクショナリのリストを取得(明細用)
        /// </summary>
        /// <param name="dictionaryList">ディクショナリのリスト、searchConditionDictionaryやresultInfoDictionary</param>
        /// <param name="ctrlId">取得対象の画面のコントロールID</param>
        /// <param name="needNone">削除行を含む場合、True(デフォルトはfalse)</param>
        /// <returns>対象のディクショナリのリスト</returns>
        public static List<Dictionary<string, object>> GetDictionaryListByCtrlId(List<Dictionary<string, object>> dictionaryList, string ctrlId, bool needNone = false)
        {
            if (needNone)
            {
                // 削除行を含む場合、指定項目値が一致するリスト
                return dictionaryList.Where(x => isExistsKeysDictionary(x, new List<string> { "CTRLID" })).Where(x => ctrlId.Equals(x["CTRLID"].ToString())).ToList();
            }
            // コントロールIDが一致するリストを返却
            return dictionaryList.Where(x => isExistsKeysDictionary(x, new List<string> { "CTRLID", "ROWSTATUS" })).Where(x => ctrlId.Equals(x["CTRLID"].ToString()) && !TMPTBL_CONSTANTS.ROWSTATUS.None.ToString().Equals(x["ROWSTATUS"].ToString())).ToList();
        }

        /// <summary>
        /// 画面のコントロールIDで対象のディクショナリのリストを取得(明細用)
        /// </summary>
        /// <param name="dictionaryList">ディクショナリのリスト、searchConditionDictionaryやresultInfoDictionary</param>
        /// <param name="ctrlId">取得対象の画面のコントロールID</param>
        /// <param name="needNone">削除行を含む場合、True(デフォルトはfalse)</param>
        /// <returns>対象のディクショナリのリスト</returns>
        public static List<Dictionary<string, object>> GetDictionaryListByCtrlIdList(List<Dictionary<string, object>> dictionaryList, List<string> ctrlIdList, bool needNone = false)
        {
            if (needNone)
            {
                // 削除行を含む場合、コントロールIDが一致するリスト
                return dictionaryList.Where(x => isExistsKeysDictionary(x, new List<string> { "CTRLID" })).Where(x => ctrlIdList.Contains(x["CTRLID"].ToString())).ToList();
            }
            // コントロールIDが一致するリストを返却
            return dictionaryList.Where(x => isExistsKeysDictionary(x, new List<string> { "CTRLID", "ROWSTATUS" })).Where(x => ctrlIdList.Contains(x["CTRLID"].ToString()) && !TMPTBL_CONSTANTS.ROWSTATUS.None.ToString().Equals(x["ROWSTATUS"].ToString())).ToList();
        }

        /// <summary>
        /// 画面のコントロールIDで対象のディクショナリの最小の行番号の要素を取得(明細用)
        /// </summary>
        /// <param name="dictionaryList">ディクショナリのリスト、searchConditionDictionaryやresultInfoDictionary</param>
        /// <param name="ctrlId">取得対象の画面のコントロールID</param>
        /// <param name="needNone">削除行を含む場合、True(デフォルトはfalse)</param>
        /// <returns>行番号が最小のディクショナリ</returns>
        public static Dictionary<string, object> GetDictionaryMinByCtrlId(List<Dictionary<string, object>> dictionaryList, string ctrlId, bool needNone = false)
        {
            var temp = GetDictionaryListByCtrlId(dictionaryList, ctrlId, needNone);
            var minRowNo = temp.Min(x => x["ROWNO"]);
            return temp.FirstOrDefault(x => x["ROWNO"] == minRowNo);
        }

        /// <summary>
        /// 選択チェックボックスがチェックされているかを取得する
        /// </summary>
        /// <param name="dictionary">選択チェックボックスが存在する行のディクショナリ</param>
        /// <returns>チェックされている場合True</returns>
        public static bool IsSelectedRowDictionary(Dictionary<string, object> dictionary)
        {
            // ディクショナリに「SELTAG」が含まれていない場合はfalseを返す
            if (!isExistsKeysDictionary(dictionary, new List<string>() { "SELTAG" }))
            {
                return false;
            }

            // resultInfoDIctionary/searchConditionDictionaryのキー名「SELTAG」にチェック状態が格納されている
            var seltag = dictionary["SELTAG"] != null ? dictionary["SELTAG"].ToString() : null;
            // チェック有なら1
            return seltag == "1";
        }

        /// <summary>
        /// 行の状態を引数の状態と比較する
        /// </summary>
        /// <param name="dictionary">行のディクショナリ</param>
        /// <param name="rowStatus">比較するRowStatus(CommonDataConstants.TMPTBL_CONSTANTS.ROWSTATUS)</param>
        /// <returns>一致するならTrue</returns>
        public static bool IsEqualRowStatus(Dictionary<string, object> dictionary, short rowStatus)
        {
            string dicValue = dictionary["ROWSTATUS"].ToString();
            return rowStatus.ToString() == dicValue;
        }

        #region private処理
        /// <summary>
        /// 検索条件を再設定
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        private static dynamic setCondition(Dictionary<string, object> condition)
        {
            dynamic searchCondition = new ExpandoObject();
            if (condition != null)
            {
                foreach (var item in condition)
                {
                    ((IDictionary<string, object>)searchCondition).Add(item.Key, item.Value);
                }
            }
            return searchCondition;
        }

        /// <summary>
        /// 検索パラメータ再生成
        /// </summary>
        /// <param name="targetParam"></param>
        /// <returns></returns>
        private static string[] reCreateParamList(string[] targetParam)
        {
            int index = 1;
            string[] workList = targetParam;

            for (int i = 0; i < targetParam.Length; i++)
            {
                if (!string.IsNullOrEmpty(workList[i]))
                {
                    // 先頭の文字がシングルクォーテーションの場合、
                    if (workList[i].StartsWith("'") && !workList[i].EndsWith("'"))
                    {
                        while (true)
                        {
                            // 次の要素を結合する
                            if (i + index <= targetParam.Length)
                            {
                                workList[i] = workList[i] + "," + workList[i + index];
                                workList[i + index] = string.Empty;
                            }

                            // 結合した文字列の末尾がシングルクォーテーションの場合、処理を抜ける
                            if (workList[i].EndsWith("'"))
                            {
                                break;
                            }

                            index++;
                        }
                    }
                }
            }

            string[] newList = new string[0];

            index = 0;
            for (int i = 0; i < workList.Length; i++)
            {
                if (string.IsNullOrEmpty(workList[i]))
                {
                    continue;
                }

                Array.Resize(ref newList, newList.Length + 1);
                newList[index] = workList[i];
                index++;

                // 配列最後の場合、処理を抜ける
                if (i == workList.Length)
                {
                    break;
                }
            }
            return newList;
        }

        /// <summary>
        /// セッション開始情報の登録
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="userId">ログインユーザID</param>
        /// <param name="guid">GUID</param>
        /// <param name="loginDate">ログイン日時</param>
        /// <returns>登録結果(成功:true/失敗:false)</returns>
        private static bool registSessionStartInfo(ComDB db, string userId, string guid, DateTime loginDate)
        {
            try
            {
                var result = db.RegistByOutsideSql("SessionInfo_Insert", "Common",
                    new { UserId = userId, GUID = guid, LoginDate = loginDate });
                return result == 1;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 入力チェック
        /// <summary>
        /// nullまたは空かどうかのチェック
        /// </summary>
        /// <param name="val">対象オブジェクト</param>
        /// <returns>チェック結果(true:nullまたは空/false:空でない)</returns>
        public static bool IsNullOrEmpty(object val)
        {
            return val != null ? string.IsNullOrEmpty(val.ToString()) : true;
        }

        /// <summary>
        /// 文字列が英数字かどうか判定
        /// </summary>
        /// <param name="str">対象文字列</param>
        /// <returns>true:文字列が英数字、false:左記以外</returns>
        /// <remarks>大文字・小文字の区別なし</remarks>
        public static bool IsAlphaNumeric(string str)
        {
            return new Regex("^[0-9a-zA-Z]+$").IsMatch(str);
        }

        /// <summary>
        /// 半角英数記号チェック
        /// </summary>
        /// <param name="str">対象文字列</param>
        /// <returns>true:文字列が英数字、false:左記以外</returns>
        /// <remarks>大文字・小文字の区別なし</remarks>
        public static bool IsAlphaNumericSymbol(string str)
        {
            return new Regex("^[!-~]*$").IsMatch(str);
        }

        /// <summary>
        /// 文字列が日付に変更できるか判定
        /// </summary>
        /// <param name="date">対象文字列</param>
        /// <returns>true:正常：false:以上</returns>
        public static bool IsDate(string date)
        {
            DateTime dt;
            if (DateTime.TryParse(date, out dt))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 文字列が日時に変更できるか判定
        /// </summary>
        /// <param name="date">判定する文字列</param>
        /// <returns>判定できる場合True</returns>
        public static bool IsDateTime(string date)
        {
            DateTime dt;

            if (DateTime.TryParse(date, out dt))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 文字列(yyyymmdd)が日付に変更できるか判定
        /// </summary>
        /// <param name="date">判定する文字列(yyyymmdd)</param>
        /// <returns>判定できる場合True</returns>
        public static bool IsDateYyyymmdd(string date)
        {
            DateTime dt;

            if (DateTime.TryParseExact(date, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out dt))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 文字列が指定した書式で日時に変更できるか判定
        /// </summary>
        /// <param name="date">判定する文字列</param>
        /// <param name="format">書式</param>
        /// <returns>判定できる場合True</returns>
        public static bool IsDateTimeFormat(string date, string format)
        {
            DateTime dt;

            if (DateTime.TryParseExact(date, format, null, System.Globalization.DateTimeStyles.None, out dt))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 文字列が整数型に変更できるか判定
        /// </summary>
        /// <param name="str">判定する文字列</param>
        /// <returns>true:正常、false:異常</returns>
        public static bool IsLong(string str)
        {
            long num;
            if (long.TryParse(str, out num))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 文字列がDecimal型に変更できるか判定
        /// </summary>
        /// <param name="str">判定する文字列</param>
        /// <returns>true:正常、false:異常</returns>
        public static bool IsDecimal(string str)
        {
            decimal num;
            if (decimal.TryParse(str, out num))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 文字列が半角カナかどうか判定
        /// </summary>
        /// <param name="str">対象文字列</param>
        /// <returns>true:半角カナ、false:左記以外</returns>
        public static bool IsHalfKatakana(string str)
        {
            return new Regex("^[\uFF65-\uFF9F]+$").IsMatch(str);
        }

        /// <summary>
        /// 文字列が半角カナかどうか判定
        /// </summary>
        /// <param name="str">対象文字列</param>
        /// <returns>true:半角カナを含む、false:半角カナを含まない</returns>
        public static bool ExistHalfKatakana(string str)
        {
            return new Regex("[\uFF65-\uFF9F]").IsMatch(str);
        }

        /// <summary>
        /// 日付比較(※VAL値が異なる場合に使用)
        /// </summary>
        /// <param name="sDate">(開始)日付１</param>
        /// <param name="eDate">(終了)日付２</param>
        /// <returns>1:sDateの方が未来,0:同日,-1:sDateの方が過去</returns>
        public static int CompareDate(DateTime sDate, DateTime eDate)
        {
            return sDate.CompareTo(eDate.Date);
        }

        /// <summary>
        /// 日付比較(※VAL値が異なる場合に使用)
        /// </summary>
        /// <param name="start">(開始)日付１</param>
        /// <param name="end">(終了)日付２</param>
        /// <returns>1:sDateの方が未来,0:同日,-1:sDateの方が過去,-9:エラー(変換失敗など)</returns>
        public static int CompareDate(string start, string end)
        {
            // 比較結果(初期値)
            int result = -9;

            DateTime sDate, eDate;

            // 日付型に型変更
            if (!(DateTime.TryParse(start, out sDate) && DateTime.TryParse(end, out eDate)))
            {
                return result; // 変換失敗
            }

            return CompareDate(sDate, eDate);
        }
        #endregion

        #region 変換処理
        /// <summary>
        /// int値からEnum値へ変換
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static T IntToEnum<T>(int val)
        {
            return (T)Enum.ToObject(typeof(T), val);
        }

        /// <summary>
        /// 文字列からEnum値へ変換
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static T StringToEnum<T>(string val) where T : struct
        {
            T result;
            if (Enum.TryParse<T>(val, out result))
            {
                return result;
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// マッチパターンに応じた検索条件文字列の取得
        /// </summary>
        /// <param name="text">対象文字列</param>
        /// <param name="pattern">マッチパターン</param>
        /// <returns></returns>
        public static string GetMatchPatternText(string text, MatchPattern pattern)
        {
            string result = text;
            switch (pattern)
            {
                case MatchPattern.ForwardMatch: // 前方一致
                    result = result + "%";
                    break;
                case MatchPattern.BackwardMatch:    // 後方一致
                    result = "%" + result;
                    break;
                case MatchPattern.PartialMatch: // 部分一致
                    result = "%" + result + "%";
                    break;
                default:    // 完全一致
                    break;
            }
            return result;
        }

        /// <summary>
        /// クラスのリストをディクショナリのリストに変換
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <param name="pList">クラスのリスト</param>
        /// <returns>ディクショナリのリスト</returns>
        public static IList<Dictionary<string, object>> ConvertClassToDictionary<T>(IList<T> pList)
        {
            // 戻り値
            List<Dictionary<string, object>> rList = new();
            // クラスのプロパティを列挙
            var props = typeof(T).GetProperties();
            // クラスのリストで繰り返し(行単位)
            foreach (T row in pList)
            {
                // 行の値をセットするディクショナリ
                Dictionary<string, object> rDic = new();
                // クラスのプロパティで繰り返し(列単位)
                foreach (var prop in props)
                {
                    var value = prop.GetValue(row); // 値
                    rDic.Add(prop.Name, value); // 行のディクショナリに追加
                }
                rList.Add(rDic); // 戻り値に行を追加
            }

            return rList;
        }
        #endregion

        #region バッチ処理共通
        /// <summary>
        /// バッチ二重起動チェック
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="conductId">機能名(=実行するバッチ名)</param>
        /// <returns>true:実行可能、false:既に実行中</returns>
        public static bool CheckBatchStatus(ComDB db, string conductId)
        {
            // DB接続
            if (!db.Connect())
            {
                return false;
            }

            // 検索条件を設定する
            dynamic resultCondition = new ExpandoObject();
            ((IDictionary<string, object>)resultCondition).Add("ConductId", conductId);

            // 検索実行
            var results = db.GetListByOutsideSql(SqlName.Batch_Get_Status, SqlName.Batch_SubDir, resultCondition);

            // 取得データなしならば実行可能
            if (results == null || results.Count == 0)
            {
                return true;
            }

            // 実行状態が「9:終了」であれば実行可能
            if (results[0].status == null || results[0].status == 9)
            {
                return true;
            }

            // その他の場合は既に実行中
            return false;
        }

        /// <summary>
        /// バッチ画面の処理結果欄に表示する
        /// </summary>
        /// <param name="db">DB操作クラス</param>
        /// <param name="conductId">機能名(=実行するバッチ名)</param>
        /// <returns>対象バッチの最新状態のリスト</returns>
        public static List<Dictionary<string, object>> GetBatchResult(ComDB db, string conductId)
        {
            // DB接続
            if (!db.Connect())
            {
                return null;
            }

            var resultList = new List<Dictionary<string, object>>();

            // 検索条件を設定する
            dynamic resultCondition = new ExpandoObject();
            ((IDictionary<string, object>)resultCondition).Add("ConductId", conductId);

            // 検索実行
            var results = db.GetListByOutsideSql(SqlName.Batch_Get_Status, SqlName.Batch_SubDir, resultCondition);

            // 取得データなし(表示内容はフロント側で作成)
            if (results == null || results.Count == 0)
            {
                return null;
            }

            // １件目はダミー(返却値のフォーマットを検索画面とバッチ画面で合わせる)
            Dictionary<string, object> dic1 = new Dictionary<string, object>
            {
                { "CTRLID", "ComBatStatus" },
                { "DATATYPE", 0 },
                { "ROWNO", 1 },
                { "ROWSTATUS", 1 },
                { "UPDTAG", 1 },
                { "VAL1", "" },
                { "VAL2", "" },
                { "VAL3", "" },
                { "VAL4", "" },
                { "VAL5", "" },
                { "VAL6", "" },
                { "VAL7", "" },
                { "VAL8", 0 },
                { "VAL9", "" },
                { "VAL10", "" },
                { "lockData", "" }
            };

            string val4 = string.Empty;
            int val8;
            string val10 = string.Empty;
            int bat_status = Convert.ToInt32(results[0].status);

            // 可変部分を実行ステータスごとに編集
            switch (bat_status)
            {
                case BatchParam.Batch_Status_Request:     // 要求
                    val4 = "未実行";
                    val8 = 0;
                    val10 = string.Empty;
                    break;
                case BatchParam.Batch_Status_Finish:     // 終了
                    if (results[0].ret_cd == 0)
                    {
                        // 正常終了
                        val4 = "正常終了";
                        val8 = 2;
                        val10 = BatchParam.Batch_Def_Valid;

                    }
                    else
                    {
                        // 異常終了
                        val4 = "異常終了";
                        val8 = 3;
                        val10 = BatchParam.Batch_Def_Invalid;

                    }
                    break;
                default:    // 実行中
                    val4 = "実行中";
                    val8 = 1;
                    val10 = BatchParam.Batch_Def_Run;
                    break;
            }

            // ２件目は実際のバッチ実行結果
            Dictionary<string, object> dic2 = new Dictionary<string, object>
            {
                { "CTRLID", "ComBatStatus" },
                { "DATATYPE", 0 },
                { "ROWNO", 1 },
                { "ROWSTATUS", 1 },
                { "UPDTAG", 1 },
                { "VAL1", results[0].run_datetime },
                { "VAL2", results[0].extstr3 },
                { "VAL3", results[0].user_id },
                { "VAL4", val4 },
                { "VAL5", results[0].ret_msg },
                { "VAL6", "" },
                { "VAL7", "" },
                { "VAL8", val8 },
                { "VAL9", "" },
                { "VAL10", val10 },
                { "lockData", "" }
            };

            resultList.Add(dic1);   // ダミーデータ
            resultList.Add(dic2);   // バッチ実行結果

            return resultList;
        }
        #endregion

        /// <summary>
        /// 変換対象条件種類
        /// </summary>
        public enum ConvertType : int
        {
            /// <summary>検索条件</summary>
            Search,
            /// <summary>実行条件</summary>
            Execute,
            /// <summary>検索結果</summary>
            Result
        }

        ///// <summary>
        ///// 検索マッチパターン
        ///// </summary>
        //public enum MatchPattern : int
        //{
        //    /// <summary>完全一致</summary>
        //    ExactMatch,
        //    /// <summary>前方一致</summary>
        //    ForwardMatch,
        //    /// <summary>後方一致</summary>
        //    BackwardMatch,
        //    /// <summary>部分一致</summary>
        //    PartialMatch
        //}

        /// <summary>
        /// DBマッピング情報クラス
        /// </summary>
        public class DBMappingInfo
        {
            /// <summary>プログラムID</summary>
            public string PgmId { get; set; }
            /// <summary>グループ番号</summary>
            public short GrpNo { get; set; }
            /// <summary>コントロールID</summary>
            public string CtrlId { get; set; }
            /// <summary>コントロール種類</summary>
            public string CtrlType { get; set; }
            /// <summary>項目番号</summary>
            public int ItemNo { get; set; }
            /// <summary>テーブル名</summary>
            public string TblName { get; set; }
            /// <summary>実カラム名</summary>
            public string ColOrgName { get; set; }
            /// <summary>別名</summary>
            public string AliasName { get; set; }
            /// <summary>実パラメータ名</summary>
            public string ParamOrgName { get; set; }
            /// <summary>データ種類</summary>
            public short DataType { get; set; }
            /// <summary>LIKE検索パターン</summary>
            public int LikePattern { get; set; }
            /// <summary>From/To項目区分</summary>
            public short FromToKbn { get; set; }
            /// <summary>IN句項目区分</summary>
            public short InClauseKbn { get; set; }
            /// <summary>フォーマット</summary>
            public string Format { get; set; }
            /// <summary>排他ロック種類(0:対象外/1:ロック値/2:ロックキー)</summary>
            public int LockType { get; set; }
            /// <summary>項目名</summary>
            public string KeyName { get; set; }
            /// <summary>排他ロック対象テーブル名</summary>
            public string LockTblName { get; set; }
            /// <summary>詳細検索コントロール種類</summary>
            public string DetailSearchCtrlType { get; set; }
            /// <summary>詳細検索実カラム名</summary>
            public string DetailSearchColOrgName { get; set; }
            /// <summary>詳細検索データ種類</summary>
            public short DetailSearchDataType { get; set; }

            /// <summary>From/To項目かどうか</summary>
            public bool IsFromTo
            {
                get { return this.FromToKbn == 1; }
            }
            /// <summary>IN句項目かどうか</summary>
            public bool IsInClause
            {
                get { return this.InClauseKbn == 1; }
            }
            /// <summary>項目名</summary>
            public string ValName
            {
                get
                {
                    return string.Format("VAL{0}", this.ItemNo);
                }
            }
            /// <summary>カラム名</summary>
            public string ColName
            {
                get
                {
                    // 別名が設定されている場合は別名を返す
                    return string.IsNullOrEmpty(this.AliasName) ? this.ColOrgName : this.AliasName;
                }
            }
            /// <summary>パラメータ名</summary>
            public string ParamName
            {
                get
                {
                    // パラメータ名が設定されていない場合はカラム名をPascalケースに変換して返す
                    if (string.IsNullOrEmpty(this.ParamOrgName))
                    {
                        return SnakeCaseToPascalCase(this.ColName);
                    }
                    else
                    {
                        return this.ParamOrgName;
                    }
                }
            }
            /// <summary>LIKE検索パターン</summary>
            public MatchPattern LikePatternEnum
            {
                get
                {
                    return CommonUtil.IntToEnum<MatchPattern>(this.LikePattern);
                }
            }
            /// <summary>排他ロック種類</summary>
            public LockType LockTypeEnum
            {
                get
                {
                    return CommonUtil.IntToEnum<LockType>(this.LockType);
                }
            }
            /// <summary>詳細検索カラム名</summary>
            public string DetailSearchColName
            {
                get
                {
                    // 詳細検索実カラム名が設定されている場合は詳細検索実カラム名を返す
                    return string.IsNullOrEmpty(this.DetailSearchColOrgName) ? this.ColName : this.DetailSearchColOrgName;
                }
            }
            /// <summary>詳細検索パラメータ名</summary>
            public string DetailSearchParamName
            {
                get
                {
                    // 詳細検索カラム名をPascalケースに変換して返す
                    return SnakeCaseToPascalCase(this.DetailSearchColName);
                }
            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public DBMappingInfo()
            {
                this.PgmId = string.Empty;
                this.GrpNo = 0;
                this.CtrlId = string.Empty;
                this.CtrlType = string.Empty;
                this.ItemNo = 0;
                this.TblName = string.Empty;
                this.ColOrgName = string.Empty;
                this.AliasName = string.Empty;
                this.ParamOrgName = string.Empty;
                this.DataType = DBColumnDataType.String;
                this.LikePattern = 0;
                this.FromToKbn = 0;
                this.InClauseKbn = 0;
                this.Format = string.Empty;
                this.KeyName = string.Empty;
                this.DetailSearchColOrgName = string.Empty;
                this.DetailSearchCtrlType = string.Empty;
                this.DetailSearchDataType = DBColumnDataType.String;
            }
        }

        /// <summary>
        /// DBカラム情報クラス
        /// </summary>
        public class DBColumnInfo
        {
            /// <summary>カラム名</summary>
            public string ColName { get; set; }
            /// <summary>カラムの型名</summary>
            public string TypeName { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public DBColumnInfo()
            {
                this.ColName = string.Empty;
                this.TypeName = string.Empty;
            }
        }

        /// <summary>
        /// DBカラムの型
        /// </summary>
        public class DBColumnDataType
        {
            /// <summary>文字列</summary>
            public const short String = 0;
            /// <summary>整数</summary>
            public const short Int = 1;
            /// <summary>実数</summary>
            public const short Decimal = 2;
            /// <summary>日時</summary>
            public const short DateTime = 3;
        }

        /// <summary>
        /// 多言語対応メッセージリソース管理クラス
        /// </summary>
        /// <remarks>翻訳マスタの内容を保持し、必要なメッセージを渡す機能をもつ</remarks>
        public class MessageResources
        {
            /// <summary>
            /// 翻訳マスタより取得したレコード情報の定義クラス
            /// </summary>
            /// <remarks>翻訳マスタの1行に相当する</remarks>
            public class Translation
            {
                /// <summary>
                /// メッセージID
                /// </summary>
                public string messageId { get; }
                /// <summary>
                /// 言語コード
                /// </summary>
                public string languageCd { get; }
                /// <summary>
                /// 翻訳値
                /// </summary>
                public string value { get; }

                /// <summary>
                /// コンストラクタ
                /// </summary>
                public Translation()
                {
                    this.messageId = string.Empty;
                    this.languageCd = string.Empty;
                    this.value = string.Empty;
                }
            }
            /// <summary>
            /// メッセージ情報のリスト
            /// </summary>
            private IList<Translation> value;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="getValue">取得したメッセージ情報のリスト</param>
            public MessageResources(IList<Translation> getValue)
            {
                this.value = getValue;
            }

            /// <summary>
            /// 単一のメッセージIDよりメッセージを取得
            /// </summary>
            /// <param name="messageId">メッセージID</param>
            /// <param name="languageCd">言語コード</param>
            /// <returns>引数のIDのメッセージ</returns>
            public string GetMessage(string messageId, string languageCd)
            {
                // IDと言語の一致する要素を取得
                var target = this.value != null ? this.value.FirstOrDefault(x => x.messageId == messageId && x.languageCd == languageCd) : null;
                // 取得できない場合は、IDを返す
                // {0}行目：{1}　のようなメッセージは、どちらの引数も値となり、リソースより取得できないため必要
                string returnMessage = messageId;
                if (target != null)
                {
                    // 取得できた場合はメッセージを取得
                    returnMessage = target.value;
                }

                return returnMessage;
            }

            /// <summary>
            /// 複数のメッセージIDより、メッセージを取得
            /// </summary>
            /// <param name="messageIdArray">メッセージIDリストの配列</param>
            /// <param name="languageId">言語コード</param>
            /// <returns>引数のIDリスト(2番目以降)でフォーマットしたメッセージ(先頭)</returns>
            public string GetMessageJoin(string[] messageIdArray, string languageId)
            {
                // 引数のメッセージIDリストの各メッセージを取得してリストへ格納
                List<string> messageList = new List<string>(messageIdArray).Select(x => GetMessage(x, languageId)).ToList();
                // 先頭がフォーマットされるメッセージ、2番目以降が引数
                return string.Format(messageList.First(), messageList.Skip(1).ToArray());
            }

            /// <summary>
            /// 言語を指定して、メッセージリソースを取得
            /// </summary>
            /// <param name="languageCd">言語コード</param>
            /// <returns>指定した言語のメッセージリソース</returns>
            public MessageResources GetLanguageResources(string languageCd)
            {
                return new MessageResources(this.value.Where(x => x.languageCd == languageCd).ToList());
            }
        }

        /// <summary>
        /// ボタン権限チェック管理クラス
        /// </summary>
        public class BtnAuthority
        {
            /// <summary>Gets or sets 画面No</summary>
            /// <value>画面No</value>
            public int FormNo { get; set; }
            /// <summary>Gets or sets ボタンコントロールID</summary>
            /// <value>ボタンコントロールID</value>
            public string BtnCtrlId { get; set; }
            /// <summary>Gets or sets ボタンアクション区分</summary>
            /// <value>ボタンアクション区分</value>
            public short BtnActionDiv { get; set; }
            /// <summary>Gets or sets ボタン権限管理区分</summary>
            /// <value>ボタン権限管理区分</value>
            public short BtnAuthDiv { get; set; }
            /// <summary>Gets or sets 画面遷移アクション区分</summary>
            /// <value>画面遷移アクション区分</value>
            public short TransActionDiv { get; set; }
        }

        /// <summary>
        /// 一覧行編集権限チェック管理クラス
        /// </summary>
        public class TblRowAuthority
        {
            /// <summary>Gets or sets 画面No</summary>
            /// <value>画面No</value>
            public int FormNo { get; set; }
            /// <summary>Gets or sets 一覧コントロールグループID</summary>
            /// <value>一覧コントロールグループID</value>
            public string CtrlGrpId { get; set; }
            /// <summary>Gets or sets 行追加区分</summary>
            /// <value>行追加区分</value>
            public short RowAddDiv { get; set; }
            /// <summary>Gets or sets 行削除区分</summary>
            /// <value>行削除区分</value>
            public short RowDelDiv { get; set; }
        }
        /// <summary>
        /// 検索条件キーワードクラス
        /// </summary>
        public class ConditionKeyword
        {
            /// <summary>検索キーワード</summary>
            public string Keyword { get; set; }

            /// <summary>完全一致フラグ</summary>
            public bool IsExactMatch { get; set; }

            /// <summary>複数条件開始フラグ</summary>
            public bool StartMultiCondition { get; set; }

            /// <summary>複数条件終了フラグ</summary>
            public bool EndMultiCondition { get; set; }

            /// <summary>OR条件フラグ</summary>
            public bool IsOrCondition { get; set; }

            /// <summary>否定条件フラグ</summary>
            public bool IsNegativeCondition { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ConditionKeyword()
            {
                this.Keyword = string.Empty;
                this.IsExactMatch = false;
                this.StartMultiCondition = false;
                this.EndMultiCondition = false;
                this.IsOrCondition = false;
                this.IsNegativeCondition = false;
            }
        }

        /// <summary>
        /// 条件文字列の解析
        /// </summary>
        /// <param name="conditionStr">条件文字列</param>
        /// <returns>条件キーワードリスト</returns>
        public static List<ConditionKeyword> AnalysisConditionString(string conditionStr)
        {
            var list = new List<ConditionKeyword>();
            var cond = new ConditionKeyword();
            var idxSt = -1;
            for (int i = 0; i < conditionStr.Length; i++)
            {
                var ch = conditionStr[i];
                if (ch == '"')
                {
                    // ダブルコーテーションの場合
                    if (!cond.IsExactMatch)
                    {
                        // 完全一致フラグOFFの場合
                        cond.IsExactMatch = true;
                        idxSt = i + 1;  // キーワードは次の文字から開始
                    }
                    else
                    {
                        if (idxSt >= 0)
                        {
                            // 開始インデックス～現在インデックス-1まで位置の文字列を切り取り
                            var length = i - idxSt;
                            if (length > 0)
                            {
                                cond.Keyword = conditionStr.Substring(idxSt, length);
                                list.Add(cond);
                                cond = new ConditionKeyword();
                            }
                            idxSt = -1;
                        }
                    }
                }
                else if (ch == '(')
                {
                    // 左括弧の場合
                    if (!cond.IsExactMatch)
                    {
                        // 完全一致フラグOFFの場合、複数条件開始フラグON
                        cond.StartMultiCondition = true;
                    }
                }
                else if (ch == ')')
                {
                    // 右括弧の場合
                    if (!cond.IsExactMatch)
                    {
                        // 完全一致フラグOFFの場合、複数条件終了フラグON
                        cond.EndMultiCondition = true;
                        if (idxSt >= 0)
                        {
                            // 開始インデックス～現在インデックス-1まで位置の文字列を切り取り
                            var length = i - idxSt;
                            if (length > 0)
                            {
                                cond.Keyword = conditionStr.Substring(idxSt, length);
                                list.Add(cond);
                                cond = new ConditionKeyword();
                            }
                            idxSt = -1;
                        }
                    }
                }
                else if (ch == '-')
                {
                    // ハイフンの場合
                    if (!cond.IsExactMatch)
                    {
                        // 完全一致フラグOFFの場合
                        if (i == 0)
                        {
                            // 先頭文字の場合、否定条件フラグON
                            cond.IsNegativeCondition = true;
                        }
                        else
                        {
                            var prevCh = conditionStr[i - 1];
                            if (prevCh == ')' || prevCh == '"' || char.IsWhiteSpace(prevCh))
                            {
                                // ひとつ前の文字が右括弧、ダブルコーテーション、または空白文字の場合
                                // 否定条件フラグON
                                cond.IsNegativeCondition = true;
                            }
                        }
                    }
                }
                else if (char.IsWhiteSpace(ch))
                {
                    // 空白文字の場合
                    if (!cond.IsExactMatch)
                    {
                        // 完全一致フラグOFFの場合
                        if (idxSt >= 0)
                        {
                            // 開始インデックス～現在インデックス-1まで位置の文字列を切り取り
                            var length = i - idxSt;
                            if (length > 0)
                            {
                                cond.Keyword = conditionStr.Substring(idxSt, length);
                                list.Add(cond);
                                cond = new ConditionKeyword();
                            }
                            idxSt = -1;
                        }
                    }
                }
                else if (ch == 'O')
                {
                    // アルファベットの「O」の場合
                    if (!cond.IsExactMatch)
                    {
                        // 完全一致フラグOFFの場合
                        if (i > 1)
                        {
                            var prevCh = conditionStr[i - 1];
                            var nextCh = i < conditionStr.Length - 2 ? conditionStr[i + 1] : char.MinValue;
                            if ((prevCh == ')' || prevCh == '"' || char.IsWhiteSpace(prevCh)) && nextCh == 'R')
                            {
                                // ひとつ前の文字が右括弧、ダブルコーテーション、または空白文字、かつ次の文字が「R」の場合
                                nextCh = i < conditionStr.Length - 3 ? conditionStr[i + 2] : char.MinValue;
                                if (nextCh == '(' || nextCh == '"' || char.IsWhiteSpace(nextCh))
                                {
                                    // さらにもう一つ次の文字が左括弧、ダブルコーテーション、または空白文字の場合
                                    // OR条件フラグON
                                    cond.IsOrCondition = true;
                                    // 「R」の分インデックスをインクリメント
                                    i++;
                                }
                            }
                        }
                        if (idxSt < 0 && !cond.IsOrCondition)
                        {
                            // OR条件の「O」でない場合、開始インデックスをセット
                            idxSt = i;
                        }
                    }
                }
                else
                {
                    // 上記以外の文字の場合
                    if (idxSt < 0)
                    {
                        // 開始インデックスをセット
                        idxSt = i;
                    }
                }
            }
            if (idxSt >= 0)
            {
                // 開始インデックス～現在インデックス-1まで位置の文字列を切り取り
                var length = conditionStr.Length - idxSt;
                if (length > 0)
                {
                    cond.Keyword = conditionStr.Substring(idxSt, length);
                    list.Add(cond);
                }
            }
            return list;
        }

        /// <summary>
        /// 詳細検索条件SQL文字列とパラメータの取得
        /// </summary>
        /// <param name="inputText">入力文字列</param>
        /// <param name="mapInfo">マッピング情報</param>
        /// <param name="param">パラメータオブジェクト</param>
        /// <returns></returns>
        public static string GetDetailSearchConditionSqlAndParam(string inputText, DBMappingInfo mapInfo, ref IDictionary<string, object> param)
        {
            string sql = string.Empty;
            if (!string.IsNullOrEmpty(inputText))
            {
                //if (mapInfo.DataType != DBColumnDataType.String)
                if (mapInfo.DetailSearchDataType != DBColumnDataType.String)
                {
                    // データ種類が文字列以外の場合
                    sql = ConvertConditionStringAndParam(inputText, mapInfo, ref param);
                }
                else
                {
                    // データ種類が文字列の場合
                    sql = ConvertConditionStringAndParamForText(inputText, mapInfo, ref param);
                }
            }
            return sql;
        }

        /// <summary>
        /// 条件文字列への変換＆パラメータの型変換(文字列型以外)
        /// </summary>
        /// <param name="inputText">入力文字列</param>
        /// <param name="mapInfo">マッピング情報</param>
        /// <param name="param">パラメータオブジェクト</param>
        /// <returns>条件文字列</returns>
        public static string ConvertConditionStringAndParam(string inputText, DBMappingInfo mapInfo, ref IDictionary<string, object> param)
        {
            StringBuilder sbSql = new StringBuilder();
            //bool isMultiCheckBox = mapInfo.CtrlType == LISTITEM_DEFINE_CONSTANTS.CELLTYPE.MultiCheckBox; // 複数選択チェックボックスの場合、True
            bool isMultiCheckBox = mapInfo.DetailSearchCtrlType == LISTITEM_DEFINE_CONSTANTS.CELLTYPE.MultiCheckBox; // 複数選択チェックボックスの場合、True
            if (!isMultiCheckBox && inputText.Contains(FromToDelimiter))
            {
                // 複数選択チェックボックスの場合は「|」区切りでも以下の処理を行わない

                // 「|」区切りの場合
                if (inputText.Trim().Length == 1)
                {
                    // 「|」のみの場合、空文字を返す
                }
                //if (mapInfo.CtrlType == LISTITEM_DEFINE_CONSTANTS.CELLTYPE.ComboBox)
                if (mapInfo.DetailSearchCtrlType == LISTITEM_DEFINE_CONSTANTS.CELLTYPE.ComboBox)
                {
                    // コンボボックスの場合、IN句を生成する
                    mapInfo.InClauseKbn = 1;
                }
                else
                {
                    // コンボボックス以外の場合、From-To指定
                    mapInfo.FromToKbn = 1;
                }
            }
            // 型変換実行
            //var paramName = mapInfo.ParamName;
            //var colName = mapInfo.ColName;
            var paramName = mapInfo.DetailSearchParamName;
            var colName = mapInfo.DetailSearchColName;
            //ConvertColumnType(mapInfo, inputText, paramName, param, ConvertType.Execute);
            ConvertColumnType(mapInfo, mapInfo.DetailSearchDataType, inputText, paramName, param, ConvertType.Execute);

            // SQL生成
            if (mapInfo.IsInClause)
            {
                sbSql.AppendLine(string.Format("{0} IN @{1}", colName, paramName));

            }
            else if (mapInfo.IsFromTo)
            {
                var array = param[paramName] as object[];
                var existsFrom = false;
                sbSql.Append("(");
                for (int i = 0; i < array.Length; i++)
                {
                    if (IsNullOrEmpty(array[i])) { continue; }
                    var paramName2 = paramName;
                    if (i == 0)
                    {
                        // From
                        existsFrom = true;
                        paramName2 = paramName + "From";
                        sbSql.Append(string.Format("{0} >= @{1}", colName, paramName2));
                    }
                    else
                    {
                        // To
                        if (existsFrom)
                        {
                            sbSql.Append(" AND ");
                        }
                        paramName2 = paramName + "To";
                        sbSql.Append(string.Format("{0} <= @{1}", colName, paramName2));
                    }
                    param.Add(paramName2, array[i]);
                }
                sbSql.AppendLine(")");
                param.Remove(paramName);
            }
            else if (isMultiCheckBox)
            {
                // 複数選択チェックボックスの場合
                // DBには「100|101|110」のように入っているので、「列名 LIKE '%選択値%'」 のように検索する
                // 複数ある場合は「(列名 LIKE '%選択値1%' OR 列名 LIKE '%選択値2%')」

                object[] array;

                if (!inputText.Contains(FromToDelimiter))
                {
                    // 値が単一の場合
                    array = new[] { param[paramName] };
                }
                else
                {
                    // 値が複数の場合
                    // パラメータリストを配列として変換
                    array = param[paramName] as object[];
                }

                bool isFirst = true; // 2番目のメンバからORを追加する
                sbSql.Append("("); // SQL開始は左括弧
                for (int i = 0; i < array.Length; i++)
                {
                    // 配列の要素数分繰り返し
                    if (IsNullOrEmpty(array[i])) { continue; }
                    // 番号を付けたパラメータを追加
                    var paramName2 = paramName + (i + 1).ToString();
                    if (!isFirst)
                    {
                        // 先頭でなければORを追加
                        sbSql.Append(" OR ");
                    }
                    // LIKEの部分一致検索
                    sbSql.Append(string.Format("{0} LIKE @{1}", colName, paramName2));
                    // 部分一致
                    string paramVal = array[i].ToString();
                    paramVal = GetMatchPatternText(paramVal, MatchPattern.PartialMatch);
                    // 検索パラメータに追加
                    param.Add(paramName2, paramVal);
                    isFirst = false; // 処理を行ったので先頭の要素ではなくなる
                }
                sbSql.AppendLine(")"); // SQL終了は右括弧
                // 本来の値（リスト）を検索パラメータから削除
                param.Remove(paramName);
            }
            else
            {
                sbSql.AppendLine(string.Format("{0} = @{1}", colName, paramName));
            }
            return sbSql.ToString();
        }

        /// <summary>
        /// 条件文字列への変換＆パラメータの変換(文字列型)
        /// </summary>
        /// <param name="inputText">入力文字列</param>
        /// <param name="mapInfo">マッピング情報</param>
        /// <param name="param">パラメータオブジェクト</param>
        /// <returns>条件文字列</returns>
        public static string ConvertConditionStringAndParamForText(string inputText, DBMappingInfo mapInfo, ref IDictionary<string, object> param)
        {
            StringBuilder sbSql = new StringBuilder();
            var keywordList = AnalysisConditionString(inputText);
            if (keywordList.Count > 0)
            {
                sbSql.Append("(");
                for (int i = 0; i < keywordList.Count; i++)
                {
                    var keyword = keywordList[i];
                    if (i > 0)
                    {
                        if (!keyword.IsOrCondition)
                        {
                            // AND条件
                            sbSql.Append("AND ");
                        }
                        else
                        {
                            // OR条件
                            sbSql.Append("OR ");
                        }
                    }

                    if (keyword.StartMultiCondition)
                    {
                        // 複数条件開始
                        sbSql.Append("(");
                    }

                    // カラム名
                    sbSql.Append(mapInfo.ColName).Append(" ");

                    // パラメータ名
                    //var paramName = string.Format("{0}{1}", mapInfo.ParamName, i + 1);
                    var paramName = string.Format("{0}{1}", mapInfo.DetailSearchParamName, i + 1);
                    var paramVal = keyword.Keyword;
                    if (!keyword.IsExactMatch)
                    {
                        // LIKE検索
                        if (keyword.IsNegativeCondition)
                        {
                            // 否定条件
                            sbSql.Append("NOT ");
                        }

                        // LIKEキーワード
                        sbSql.Append("LIKE @").Append(paramName);
                        // 部分一致
                        paramVal = Regex.Replace(paramVal, @"[%_\[]", "[$0]"); // Like検索で"%","[","_"を検索する場合に検索できないのでエスケープ
                        paramVal = GetMatchPatternText(paramVal, MatchPattern.PartialMatch);
                    }
                    else
                    {
                        if (keyword.IsNegativeCondition)
                        {
                            // 否定条件
                            sbSql.Append("!");
                        }
                        // 完全一致
                        sbSql.Append("= @").Append(paramName);
                    }

                    // 複数条件終了
                    sbSql.AppendLine(!keyword.EndMultiCondition ? "" : ")");

                    // パラメータ設定
                    param[paramName] = paramVal;
                }
                sbSql.Append(")");
            }
            return sbSql.ToString();
        }

        /// <summary>
        /// クラスより、値がNullでないメンバ名を取得する処理
        /// </summary>
        /// <typeparam name="T">クラスの型</typeparam>
        /// <param name="targetClass">チェックするクラス</param>
        /// <returns>値がNullでないメンバ名のリスト</returns>
        public static List<string> GetNotNullNameByClass<T>(T targetClass)
        {
            List<string> nullPropertyNames = new();

            PropertyInfo[] targetProps = typeof(T).GetProperties(); // 設定するデータクラスの情報を取得
            foreach (var target in targetProps)
            {
                // 値を取得して、Nullでなければ検索条件として有効なのでアンコメントする
                var value = target.GetValue(targetClass);
                if (value != null)
                {
                    if (target.PropertyType == typeof(List<int>))
                    {
                        //複数選択チェックボックスの選択値が無い場合、空のリストのため要素があるかチェック
                        if (((List<int>)value).Any())
                        {
                            nullPropertyNames.Add(target.Name);
                        }
                    }
                    else if (target.PropertyType == typeof(List<long>))
                    {
                        //複数選択チェックボックスの選択値が無い場合、空のリストのため要素があるかチェック
                        if (((List<long>)value).Any())
                        {
                            nullPropertyNames.Add(target.Name);
                        }
                    }
                    else
                    {
                        nullPropertyNames.Add(target.Name);
                    }
                }
            }

            return nullPropertyNames;
        }

        /// <summary>
        /// 1ページ当たりの行数コンボ取得処理
        /// </summary>
        /// <param name="userId">ログインユーザID</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="db">DB接続</param>
        /// <returns></returns>
        public static List<int> GetComboRowsPerPage(string userId, string languageId, ComDB db)
        {
            // SQL実行
            var param = new { LanguageId = languageId };
            IList<int> result = db.GetListByOutsideSqlByDataClass<int>(SqlName.GetComboRowsPerPage, SqlName.SubDir, param);
            return result.ToList();
        }

        /// <summary>
        /// 項目カスタマイズ情報保存処理
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <param name="userId">ユーザID</param>
        /// <param name="condition">登録条件</param>
        /// <returns></returns>
        public static bool SaveCustomizeListInfo(ComDB db, string userId, Dictionary<string, object> condition)
        {
            //if (condition.ContainsKey("factoryId") && condition.ContainsKey("customizeList"))
            if (condition.ContainsKey("customizeList"))
            {
                //var factoryIdList = condition["factoryIdList"] as List<int>;
                var customizeList = condition["customizeList"] as List<Dictionary<string, object>>;

                DateTime now = DateTime.Now;
                Dao.CmControlUserCustomizeEntity param = new Dao.CmControlUserCustomizeEntity();
                param.UserId = Convert.ToInt32(userId);
                param.ProgramId = customizeList[0]["PGMID"].ToString();
                param.FormNo = Convert.ToInt32(customizeList[0]["FORMNO"].ToString());
                param.ControlGroupId = customizeList[0]["CTRLID"].ToString();
                param.DataDivision = Convert.ToInt32(customizeList[0]["DATADIV"].ToString());
                //param.FactoryIdList = factoryIdList;
                param.InsertDatetime = now;
                param.UpdateDatetime = now;

                // 削除処理実行(仮登録の場合は仮登録データのみ)
                if (db.RegistByOutsideSql(SqlName.DeleteItemCustomizeInfoList, "Common", param) < 0)
                {
                    return false;
                }

                // 登録処理実行
                var dispList = customizeList[0]["customizeList"] as List<object>;
                //foreach (var factoryId in factoryIdList)
                //{
                //param.LocationStructureId = factoryId;
                for (int i = 0; i < dispList.Count; i++)
                {
                    var info = dispList[i] as List<object>;
                    param.DisplayOrder = i + 1;
                    param.ControlNo = Convert.ToInt32(info[0].ToString().Replace("VAL", ""));
                    param.DisplayFlg = info[1].ToString() == "1";

                    if (db.RegistByOutsideSql(SqlName.SaveItemCustomizeInfoList, "Common", param) < 0)
                    {
                        return false;
                    }
                }
                //}
            }
            return true;
        }

        /// <summary>
        /// 言語コンボ取得処理
        /// </summary>
        /// <param name="userId">ログインユーザID</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="db">DB接続</param>
        /// <returns></returns>
        public static List<CommonDataBaseClass.LanguageInfo> GetLanguageItemList(string userId, string languageId, ComDB db)
        {
            // SQL実行
            var param = new { LanguageId = languageId, UserId = userId };
            IList<CommonDataBaseClass.LanguageInfo> result = db.GetListByOutsideSqlByDataClass<CommonDataBaseClass.LanguageInfo>(SqlName.GetLanguageList, SqlName.SubDir, param);
            return result.ToList();
        }

        /// <summary>
        /// 機能IDリスト取得処理
        /// </summary>
        /// <param name="userId">ログインユーザID</param>
        /// <param name="authorityLevelId">ユーザー権限レベルID</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="db">DB接続</param>
        /// <returns></returns>
        public static List<string> GetUserConductAuthorityList(string userId, int authorityLevelId, string languageId, ComDB db)
        {
            // ユーザ機能権限マスタから権限情報を取得
            int? paramUserId = authorityLevelId != USER_CONSTANTS.AUTHORITY_LEVEL.Administrator ? Convert.ToInt32(userId) : null;
            var resultList = db.GetListByOutsideSql<Dao.MsUserConductAuthorityEntity>(SqlName.UserAuthGetListForLogin, SqlName.SubDir, new { UserId = paramUserId });
            List<string> conductList = new List<string>();
            if (resultList != null && resultList.Count > 0)
            {
                //機能IDリスト
                conductList.AddRange(resultList.Select(x => x.ConductId));
            }
            return conductList;
        }

        /// <summary>
        /// テンポラリフォルダパス取得処理
        /// </summary>
        /// <param name="db">DB接続</param>
        /// <returns></returns>
        public static List<CommonDataBaseClass.TempFolderPathInfo> GetTemporaryFolderPath(ComDB db)
        {
            // SQL実行
            var param = new { StructureGroupId = StructureGroupId };
            IList<CommonDataBaseClass.TempFolderPathInfo> result = db.GetListByOutsideSqlByDataClass<CommonDataBaseClass.TempFolderPathInfo>(SqlName.GetTemporaryFolderPath, SqlName.SubDir, param);
            return result.ToList();
        }

        /// <summary>
        /// 画像ファイル情報認証・取得
        /// </summary>
        /// <param name="condition">画像ファイルのキー情報を含むディクショナリ</param>
        /// <param name="db">DB接続</param>
        /// <param name="filePath">out 表示する画像ファイルパス</param>
        /// <returns></returns>
        public static bool GetImageFileInfo(Dictionary<string, object> condition, ComDB db, out string filePath)
        {
            filePath = string.Empty;
            // 表示画像ファイルのキー
            const string condKey = "ImageFileInfo";
            if (!condition.ContainsKey(condKey) || condition[condKey] == null)
            {
                // キーが無い場合、終了
                return false;
            }
            // キーは添付情報テーブルの　添付ID|キーID|更新日時の指定日からの秒数 |で分割し、所定の形式でなければエラー
            string fileKey = condition[condKey].ToString();
            string[] fileKeys = fileKey.Split("|");
            if (fileKeys == null || fileKeys.Length != 3)
            {
                return false;
            }
            // SQLで検索、添付IDから上記キーを再取得する。合致したときは一緒に取得したファイルパスを設定する
            var param = new { AttachmentId = fileKeys[0] };
            CommonDataBaseClass.ImageFileInfo result = db.GetEntityByOutsideSqlByDataClass<CommonDataBaseClass.ImageFileInfo>(SqlName.GetImageFileInfo, SqlName.SubDir, param);

            if (result == null || String.IsNullOrEmpty(result.FileInfo) || !result.FileInfo.Equals(fileKey))
            {
                // 取得結果が無い、引数のキー情報と一致しない(情報改変)場合は終了
                return false;
            }
            // 取得したファイルパスを返す
            filePath = result.FilePath;
            return true;
        }

        /// <summary>
        /// 復号化データ取得
        /// </summary>
        /// <param name="condition">暗号化データを含むディクショナリ</param>
        /// <returns>復号化データ</returns>
        public static string GetDecryptedData(Dictionary<string, object> condition)
        {
            var decryptedData = string.Empty;
            // 暗号化データのキー
            const string condKey = "EncryptedData";
            if (condition.ContainsKey(condKey) && !IsNullOrEmpty(condition[condKey]))
            {
                var encryptedData = condition[condKey].ToString();

                // 復号化
                decryptedData = DcryptUtils.Decrypt(encryptedData);
            }
            return decryptedData;
        }

        /// <summary>
        /// 復号化ユーティリティ
        /// </summary>
        class DcryptUtils
        {
            // AESで使用する初期ベクトル（暗号化時と共通）
            private static readonly string AesIV = @"rx3XdS6jYm-8E6GM"; // 半角16文字のランダムな文字列

            // AESで使用するキー（暗号化時と共通）
            private static readonly string AesKey = @"XEeP8aTAeeAsQiair5fEgJMaerAkkN2R"; // 半角32文字のランダムな文字列

            // キーサイズ
            private static readonly int KeySize = 256;

            // ブロックサイズ
            private static readonly int BlockSize = 128;

            /// <summary>
            /// 暗号化されたBase64文字列を復号化
            /// </summary>
            /// <param name="decryptedData">暗号化されたBase64文字列</param>
            /// <returns>復号化された文字列</returns>
            public static string Decrypt(string decryptedData)
            {
                // URLデコード
                byte[] data = Encoding.UTF8.GetBytes(decryptedData);
                string strUrl = System.Web.HttpUtility.UrlDecode(data, System.Text.Encoding.GetEncoding("UTF-8"));
                // 暗号化されたBase64文字列をバイトデータに変換
                byte[] cipherText = Convert.FromBase64String(strUrl);
                if (cipherText == null || cipherText.Length <= 0) throw new ArgumentException(nameof(cipherText));

                // CBCの場合は、「128bit / 8 = 16byte」の初期ベクトルを用意する
                byte[] iv = Encoding.UTF8.GetBytes(AesIV);
                if (iv.Length != 16) throw new ArgumentException(nameof(iv));

                // AES-256の場合は、「256bit / 8 = 32byte」の鍵を用意する
                byte[] key = Encoding.UTF8.GetBytes(AesKey);
                if (key.Length != 32) throw new ArgumentException(nameof(key));

                string plaintext = null;

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.KeySize = KeySize;
                    aesAlg.BlockSize = BlockSize;
                    aesAlg.Mode = CipherMode.CBC;
                    aesAlg.Padding = PaddingMode.PKCS7;
                    aesAlg.Key = key;
                    aesAlg.IV = iv;

                    //復号化
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    byte[] decrypt = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
                    //復号化したバイトデータを文字列に変換
                    plaintext = Encoding.UTF8.GetString(decrypt);
                }

                return plaintext;
            }
        }

        /// <summary>
        /// ユーザ情報取得処理
        /// </summary>
        /// <param name="mailAdress">メールアドレス</param>
        /// <param name="db">DB接続</param>
        /// <returns></returns>
        public static List<Dao.MsUserEntity> GetUserInfoByMailAdress(Dictionary<string, object> condition, ComDB db)
        {
            List<Dao.MsUserEntity> userInfoList = null;

            const string condKey = "MailAdress";
            if (condition.ContainsKey(condKey) && !IsNullOrEmpty(condition[condKey]))
            {
                // ユーザマスタからメールアドレスをキーにユーザ情報を取得
                var resultList = db.GetListByOutsideSql<Dao.MsUserEntity>(SqlName.UserMstGetUserInfo, SqlName.SubDir, new { MailAdress = condition[condKey] });
                if (resultList != null && resultList.Count > 0)
                {
                    // ユーザ情報
                    userInfoList = resultList.ToList();
                }
            }
            return userInfoList;
        }

        /// <summary>
        /// 条件のor句を生成する
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="colum">カラム名</param>
        /// <param name="list">パラメータ</param>
        /// <returns>or句の文字列</returns>
        public static string GetWhereSqlString<T>(string colum, List<T> list)
        {
            //where句の条件を生成（IN句にはパラメータの個数制限があるので、すべてORで繋げる）
            var paramWhere = new StringBuilder();
            List<T> paramList = list.Distinct().ToList();
            foreach (T param in paramList)
            {
                if (paramWhere.Length > 0)
                {
                    paramWhere.Append("OR ");
                }
                //例：structure_id = 123
                paramWhere.Append(colum).Append(" = ").AppendLine(param.ToString());
            }
            if (paramWhere.Length > 0)
            {
                //括弧で囲む
                paramWhere.Insert(0, "(");
                paramWhere.Append(")");
            }
            //例：structure_id = 123 or structure_id = 456...
            return paramWhere.ToString();
        }

        /// <summary>
        /// 条件の設定
        /// </summary>
        /// <param name="list">変換対象辞書リスト</param>
        /// <param name="mappingList">マッピング情報リスト</param>
        /// <param name="condition">条件オブジェクト</param>
        /// <param name="convType">変換対象条件種類(検索/実行/検索結果)</param>
        /// <param name="paramList">パラメータ名リスト(パラメータ名による対象条件の絞り込みが必要な場合のみ指定)</param>
        public static bool SetConditionByDataClass(string ctrlId, IList<DBMappingInfo> mapInfoList, List<Dictionary<string, object>> list, dynamic condition, ComUtil.ConvertType convType, string languageId, List<string> paramList = null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list.Count > i)
                {
                    SetConditionByDataClass(ctrlId, mapInfoList, condition, list[i], convType, paramList);
                }
            }
            // 言語ID
            condition.LanguageId = languageId;

            return true;
        }

        /// <summary>
        /// 条件の設定
        /// </summary>
        /// <param name="ctrlId">コントロールID</param>
        /// <param name="mapInfoList">マッピング情報リスト</param>
        /// <param name="condition">条件オブジェクト</param>
        /// <param name="dic">変換対象辞書</param>
        /// <param name="convType">変換対象条件種類(検索/実行/検索結果)</param>
        /// <param name="paramList">パラメータ名リスト(パラメータ名による対象条件の絞り込みが必要な場合のみ指定)</param>
        public static bool SetConditionByDataClass(string ctrlId, IList<DBMappingInfo> mapInfoList, dynamic condition, Dictionary<string, object> dic, ConvertType convType, List<string> paramList = null)
        {
            List<DBMappingInfo> mappingList;
            if (paramList == null || paramList.Count == 0)
            {
                mappingList = mapInfoList.Where(x => x.CtrlId.Equals(ctrlId)).ToList();
            }
            else
            {
                mappingList = mapInfoList.Where(x => x.CtrlId.Equals(ctrlId) && paramList.Contains(x.ParamName)).ToList();
            }

            return SetConditionByDataClass(mappingList, condition, dic, convType, paramList);
        }

        /// <summary>
        /// 条件の設定
        /// </summary>
        /// <param name="mappingList">マッピング情報リスト</param>
        /// <param name="condition">条件オブジェクト</param>
        /// <param name="dic">変換対象辞書</param>
        /// <param name="convType">変換対象条件種類(検索/実行/検索結果)</param>
        /// <param name="paramList">パラメータ名リスト(パラメータ名による対象条件の絞り込みが必要な場合のみ指定)</param>
        public static bool SetConditionByDataClass(List<DBMappingInfo> mappingList, dynamic condition, Dictionary<string, object> dic, ConvertType convType, List<string> paramList = null)
        {
            foreach (var mapInfo in mappingList)
            {
                // VAL名またはカラム名が未設定の場合はスキップ
                if (string.IsNullOrEmpty(mapInfo.ValName) || string.IsNullOrEmpty(mapInfo.ColName)) { continue; }
                if (dic.ContainsKey(mapInfo.ValName))
                {
                    // 指定した条件名が条件辞書に存在する場合、条件オブジェクトへ追加
                    var key = mapInfo.ParamName;
                    var val = dic[mapInfo.ValName];

                    if (CommonUtil.IsNullOrEmpty(val))
                    {
                        if (mapInfo.IsInClause)
                        {
                            // IN句の場合、Listに初期値を設定
                            PropertyInfo prop = condition.GetType().GetProperty(key + "List");
                            if (prop == null) { continue; }
                            if (prop.PropertyType == typeof(List<decimal>))
                            {
                                List<decimal> list = new List<decimal>();
                                setValueToClassCon(mapInfo, key + "List", list, condition, convType);
                                continue;
                            }
                            else if (prop.PropertyType == typeof(List<int>))
                            {
                                List<int> list = new List<int>();
                                setValueToClassCon(mapInfo, key + "List", list, condition, convType);
                                continue;
                            }
                            else if (prop.PropertyType == typeof(List<long>))
                            {
                                List<long> list = new List<long>();
                                setValueToClassCon(mapInfo, key + "List", list, condition, convType);
                                continue;
                            }
                            else if (prop.PropertyType == typeof(List<string>))
                            {
                                List<string> list = new List<string>();
                                setValueToClassCon(mapInfo, key + "List", list, condition, convType);
                                continue;
                            }
                            else if (prop.PropertyType == typeof(List<DateTime>))
                            {
                                List<DateTime> list = new List<DateTime>();
                                setValueToClassCon(mapInfo, key + "List", list, condition, convType);
                                continue;
                            }
                            else
                            {
                                List<object> list = new List<object>();
                                setValueToClassCon(mapInfo, key + "List", list, condition, convType);
                                continue;
                            }
                        }
                        continue;
                    }

                    string value = val.ToString();
                    // FromTo分割の場合、分割後の値で設定を行う
                    if (mapInfo.IsFromTo)
                    {
                        // デリミタ文字が含まれていない場合、そのまま設定する
                        if (!value.Contains(FromToDelimiter.ToString()))
                        {
                            setValueToClassCon(mapInfo, key, value, condition, convType);
                            continue;
                        }

                        var values = value.Split(FromToDelimiter);
                        int count = 0;
                        foreach (var data in values)
                        {
                            var setValue = data;
                            if (data.Contains(NumberUnitDelimiter))
                            {
                                setValue = data.Split(NumberUnitDelimiter)[0];
                            }
                            var tmpKey = key;
                            if (count == 0) { tmpKey += "From"; } else { tmpKey += "To"; }
                            setValueToClassCon(mapInfo, tmpKey, setValue, condition, convType);
                            count++;
                        }
                        continue;
                    }

                    // IN句パラメータの場合、配列に格納しなおし、設定を行う
                    if (mapInfo.IsInClause)
                    {
                        PropertyInfo prop = condition.GetType().GetProperty(key + "List");
                        if (prop == null) { continue; }

                        if (prop.PropertyType == typeof(List<decimal>))
                        {
                            List<decimal> list = new List<decimal>();
                            if (value.Contains(FromToDelimiter))
                            {
                                var data = value.Split(FromToDelimiter);
                                for (int i = 0; i < data.Count(); i++)
                                {
                                    list.Add(decimal.Parse(data[i]));
                                }
                            }
                            else
                            {
                                list.Add(decimal.Parse(value));
                            }
                            setValueToClassCon(mapInfo, key + "List", list, condition, convType);
                            setValueToClassCon(mapInfo, key, list.ToArray(), condition, convType);
                            continue;
                        }
                        else if (prop.PropertyType == typeof(List<int>))
                        {
                            List<int> list = new List<int>();
                            if (value.Contains(FromToDelimiter))
                            {
                                var data = value.Split(FromToDelimiter);
                                for (int i = 0; i < data.Count(); i++)
                                {
                                    list.Add(int.Parse(data[i]));
                                }
                            }
                            else
                            {
                                list.Add(int.Parse(value));
                            }
                            setValueToClassCon(mapInfo, key + "List", list, condition, convType);
                            setValueToClassCon(mapInfo, key, list.ToArray(), condition, convType);
                            continue;
                        }
                        else if (prop.PropertyType == typeof(List<long>))
                        {
                            List<long> list = new List<long>();
                            if (value.Contains(FromToDelimiter))
                            {
                                var data = value.Split(FromToDelimiter);
                                for (int i = 0; i < data.Count(); i++)
                                {
                                    list.Add(long.Parse(data[i]));
                                }
                            }
                            else
                            {
                                list.Add(long.Parse(value));
                            }
                            setValueToClassCon(mapInfo, key + "List", list, condition, convType);
                            setValueToClassCon(mapInfo, key, list.ToArray(), condition, convType);
                            continue;
                        }
                        else if (prop.PropertyType == typeof(List<string>))
                        {
                            List<string> list = new List<string>();
                            if (value.Contains(FromToDelimiter))
                            {
                                var data = value.Split(FromToDelimiter);
                                for (int i = 0; i < data.Count(); i++)
                                {
                                    list.Add(data[i]);
                                }
                            }
                            else
                            {
                                list.Add(value);
                            }
                            setValueToClassCon(mapInfo, key + "List", list, condition, convType);
                            setValueToClassCon(mapInfo, key, list.ToArray(), condition, convType);
                            continue;
                        }
                        else if (prop.PropertyType == typeof(List<DateTime>))
                        {
                            List<DateTime> list = new List<DateTime>();
                            if (value.Contains(FromToDelimiter))
                            {
                                var data = value.Split(FromToDelimiter);
                                for (int i = 0; i < data.Count(); i++)
                                {
                                    list.Add(DateTime.Parse(data[i]));
                                }
                            }
                            else
                            {
                                list.Add(DateTime.Parse(value));
                            }
                            setValueToClassCon(mapInfo, key + "List", list, condition, convType);
                            setValueToClassCon(mapInfo, key, list.ToArray(), condition, convType);
                            continue;
                        }
                        else
                        {
                            List<object> list = new List<object>();
                            if (value.Contains(FromToDelimiter))
                            {
                                var data = value.Split(FromToDelimiter);
                                for (int i = 0; i < data.Count(); i++)
                                {
                                    list.Add(data[i]);
                                }
                            }
                            else
                            {
                                list.Add(value);
                            }
                            setValueToClassCon(mapInfo, key + "List", list, condition, convType);
                            setValueToClassCon(mapInfo, key, list.ToArray(), condition, convType);
                            continue;
                        }
                    }

                    // セルタイプが数値で 数値+単位分割文字が設定されている場合
                    if (mapInfo.CtrlType == LISTITEM_DEFINE_CONSTANTS.CELLTYPE.Number && value.Contains(NumberUnitDelimiter))
                    {
                        var values = value.Split(NumberUnitDelimiter);
                        setValueToClassCon(mapInfo, key, values[0], condition, convType);
                        continue;
                    }

                    setValueToClassCon(mapInfo, key, val, condition, convType);
                }
            }
            return true;
        }

        /// <summary>
        /// データクラスに値を設定する
        /// </summary>
        /// <param name="mappingInfo">マッピング情報</param>
        /// <param name="key">キー情報</param>
        /// <param name="val">設定値</param>
        /// <param name="condition">条件データ</param>
        /// <param name="convType">検索条件</param>
        public static void setValueToClassCon(DBMappingInfo mappingInfo, string key, object val, dynamic condition, ConvertType convType)
        {
            // プロパティを取得
            PropertyInfo property = condition.GetType().GetProperty(key);
            if (property == null) { return; }

            // 文字型で検索条件の場合、マッチパターン指定
            if (!CommonUtil.IsNullOrEmpty(val) && property.PropertyType == typeof(string) && convType == ConvertType.Search)
            {
                switch (mappingInfo.LikePatternEnum)
                {
                    case MatchPattern.ForwardMatch:
                        val = val + "%";
                        break;
                    case MatchPattern.BackwardMatch:
                        val = "%" + val;
                        break;
                    case MatchPattern.PartialMatch:
                        val = "%" + val + "%";
                        break;
                    default:
                        break;
                }
            }

            if (property != null)
            {
                SetPropertyValue(property, condition, val);
            }
            return;
        }

        /// <summary>
        /// 固定SQL文の取得
        /// </summary>
        /// <param name="subDir">Resources\sql配下のサブディレクトリパス(サブディレクトリが複数階層の場合、パス区切り文字は"\"ではなく".")</param>
        /// <param name="fileName">SQLテキストファイル名</param>
        /// <param name="sql">out 取得したSQL文</param>
        /// <param name="listUnComment">省略可能 SQLの中でコメントアウトを解除したい箇所のリスト</param>
        /// <returns>取得結果(true:取得OK/false:取得NG )</returns>
        public static bool GetFixedSqlStatement(string subDir, string fileName, out string sql, List<string> listUnComment = null)
        {
            sql = string.Empty;
            string assemblyName = CommonWebTemplate.AppCommonObject.Config.AppSettings.FixedSqlStatementAssemblyName;

            // リソース名を生成
            StringBuilder resourceName = new StringBuilder();
            resourceName.Append(assemblyName).Append(".").Append(CommonWebTemplate.AppCommonObject.Config.AppSettings.FixedSqlStatementDir).Append(".");
            if (!string.IsNullOrEmpty(subDir))
            {
                // サブディレクトリパスの追加(念のためパス区切り文字を"."に変換しておく)
                resourceName.Append(subDir.Replace(@"\", ".")).Append(".");
            }
            resourceName.Append(fileName).Append(".sql");

            // 埋め込みリソースからSQL文を取得
            bool result = ComUtil.GetEmbeddedResourceStr(assemblyName, resourceName.ToString(), out sql);

            // SQLの動的制御　コメントアウトを解除
            if (listUnComment != null && listUnComment.Count > 0)
            {
                foreach (string wordUnComment in listUnComment)
                {
                    // @+指定された文字列がコメントアウトの前後に付いているので除去
                    // /*@Hoge と @Hoge*/ を除去すれば、囲われたSQLが有効になる
                    Regex startReplace = new Regex("\\/\\*@" + wordUnComment + "[^a-zA-Z\\d_]");
                    sql = startReplace.Replace(sql, string.Empty).Replace("@" + wordUnComment + "*/", string.Empty);
                }
            }

            return result;
        }
    }
}
