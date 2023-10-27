using CommonExcelUtil;
using CommonSTDUtil;
using CommonSTDUtil.CommonBusinessLogic;
using CommonWebTemplate.Models.Common;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Microsoft.AspNetCore.Http;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using ComUtilDb = CommonSTDUtil.CommonDBManager;
using Dao = BusinessLogic_DM0002.BusinessLogicDataClass_DM0002;
using DbTransaction = System.Data.IDbTransaction;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using TMQConsts = CommonTMQUtil.CommonTMQConstants;

namespace BusinessLogic_DM0002
{
    /// <summary>
    /// 文書管理　詳細(共通)画面　アップロード
    /// </summary>
    public partial class BusinessLogic_DM0002 : CommonBusinessLogicBase
    {
        /// <summary>
        /// ファイルアップロードクラス
        /// </summary>
        private static class FileUpload
        {
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = @"Attachment";
            /// <summary>
            /// アップロード先フォルダ取得SQL
            /// </summary>
            public const string UploadFolder = "GetUploadFolder";
            /// <summary>
            /// ディレクトリ構成取得SQL
            /// </summary>
            public const string UploadDirectory = "GetUploadDirectory";
            /// <summary>
            /// ディレクトリ取得
            /// </summary>
            public const string GetDirectoryName = "GetDirectoryName";
        }

        /// <summary>
        /// ファイルのアップロード
        /// </summary>
        /// <param name="registInfo">登録情報</param>
        /// <param name="inputStream">選択されたファイル情報</param>
        public static void uploadFile(ref Dao.searchResult registInfo, IFormFile[] inputStream)
        {
            // ファイル選択されていないまたは添付種類が「ファイル」ではない場合は何もしない
            if (inputStream == null || registInfo.AttachmentTypeNo != AttachmentType.File)
            {
                return;
            }

            using (Stream inStream = inputStream[0].OpenReadStream())
            using (MemoryStream storeStream = new MemoryStream())
            {
                // 選択されたファイルをメモリに格納
                storeStream.SetLength(inStream.Length);
                inStream.Read(storeStream.GetBuffer(), 0, (int)inStream.Length);
                storeStream.Flush();
                inStream.Close();

                registInfo.FilePath = registInfo.FilePath + "\\" + registInfo.FileName;
                // メモリの情報を出力ファイルに反映
                saveMemoryStream(storeStream, registInfo.FilePath);

                storeStream.Close();
            }

            // 画面で選択されたファイルのファイル名に戻す
            registInfo.FileName = inputStream[0].FileName;

            void saveMemoryStream(MemoryStream ms, string fileName)
            {
                // 出力ファイルにストリームの情報を書き込み
                using (FileStream outStream = File.OpenWrite(fileName))
                {
                    ms.WriteTo(outStream);
                    outStream.Flush();
                    outStream.Close();
                }
            }
        }

        /// <summary>
        /// アップロードパス作成
        /// </summary>
        /// <param name="registInfo">登録情報</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="db">DBクラス</param>
        /// <returns>エラーの場合False</returns>
        public static bool createUploadPath(ref Dao.searchResult registInfo, string languageId, ComUtilDb.CommonDBManager db)
        {
            // ファイルアップロード先フォルダ取得
            TMQUtil.GetFixedSqlStatement(FileUpload.SubDir, FileUpload.UploadFolder, out string outSql);
            dynamic param = new ExpandoObject();
            param.LanguageId = languageId;
            // SQL実行
            dynamic folderResults = db.GetEntity(outSql, param);

            // 取得できなかった場合
            if (folderResults == null || folderResults.folder == null)
            {
                return false;
            }

            // ファイルアップロード先フォルダ設定
            string uploadPath = folderResults.folder;

            // アップロード先ディレクトリ構成取得
            IList<Dao.directory> directoryResults = TMQUtil.SqlExecuteClass.SelectList<Dao.directory>(FileUpload.UploadDirectory, FileUpload.SubDir, param, db);
            // 取得できなかった場合
            if (directoryResults == null || directoryResults.Count == 0)
            {
                return false;
            }

            // アップロード先のディレクトリ構成を作成
            foreach (var result in directoryResults)
            {
                switch (result.DirectoryName)
                {
                    case "year":
                        // システム日付の「年」
                        uploadPath += "\\" + DateTime.Now.ToString("yyyy");
                        break;
                    case "month":
                        // システム日付の「月」
                        uploadPath += "\\" + DateTime.Now.ToString("MM");
                        break;
                    case "factory":
                        // 工場ID
                        IList<Dao.searchResult> tmpList = new List<Dao.searchResult> { registInfo }; // 一時的にリストを作成
                        if(tmpList[0].FunctionTypeId == (int)TMQConsts.Attachment.FunctionTypeId.SpareMap)
                        {
                            uploadPath += "\\" + tmpList[0].LocationStructureId;
                        }
                        else
                        {
                            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.searchResult>(ref tmpList, new List<StructureType> { StructureType.Location }, db, languageId);
                            uploadPath += "\\" + tmpList[0].FactoryId;
                        }

                        registInfo = tmpList[0];
                        break;
                    case "document":
                        // 文書種類
                        if (!getDirectoryName(registInfo.DocumentTypeStructureId, languageId, db, out string directoryName))
                        {
                            return false;
                        }
                        uploadPath += "\\" + directoryName;
                        break;
                    default:
                        break;
                }
            }

            registInfo.FilePath = uploadPath;
            return true;
        }

        /// <summary>
        /// ディレクトリ名取得
        /// </summary>
        /// <param name="structure_id">文書種類ID(構成ID)</param>
        /// <param name="languageId">言語ID</param>
        /// <param name="db">DBクラス</param>
        /// <param name="directoryName">ディレクトリ名</param>
        /// <returns>取得できなかった場合false</returns>
        public static bool getDirectoryName(int structure_id, string languageId, ComUtilDb.CommonDBManager db, out string directoryName)
        {
            directoryName = string.Empty;

            // SQL取得
            TMQUtil.GetFixedSqlStatement(FileUpload.SubDir, FileUpload.GetDirectoryName, out string outSql);
            // WHERE句パラメータ
            dynamic whereParam = new ExpandoObject();
            whereParam.StructureId = structure_id;
            whereParam.LanguageId = languageId;
            // SQL実行
            dynamic result = db.GetEntity(outSql, whereParam);

            // 取得できなかった場合
            if (result == null || result.directory == null)
            {
                return false;
            }

            directoryName = result.directory;

            return true;
        }

        /// <summary>
        /// ファイル名の取得
        /// </summary>
        /// <param name="fileDirectory">ディレクトリ</param>
        /// <param name="inputStream">選択されたファイル情報</param>
        /// <returns>ファイル名</returns>
        public static string getFileName(string fileDirectory, IFormFile[] inputStream)
        {
            // アップロード先が存在しない場合
            if (!Directory.Exists(fileDirectory))
            {
                // アップロード先作成
                Directory.CreateDirectory(fileDirectory);
            }

            string baseName = Path.GetFileNameWithoutExtension(inputStream[0].FileName); // ファイル名
            string tmpName = Path.GetFileNameWithoutExtension(inputStream[0].FileName);  //一時ファイル名
            string extension = Path.GetExtension(inputStream[0].FileName);               // 拡張子
            int i = 1;

            // ファイルの存在チェック
            while (File.Exists(fileDirectory + "\\" + tmpName + extension))
            {
                // 存在する場合はファイル名を変更
                tmpName = baseName + "(" + i.ToString() + ")";
                i++;
            }

            // 変更後のファイル名 + 拡張子
            return tmpName + extension;
        }
    }
}
