using CommonExcelUtil;
using CommonSTDUtil;
using CommonSTDUtil.CommonBusinessLogic;
using CommonWebTemplate.Models.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComRes = CommonSTDUtil.CommonResources;
using ComBase = CommonSTDUtil.CommonDataBaseClass;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_PT0001.BusinessLogicDataClass_PT0001;
using DbTransaction = System.Data.IDbTransaction;
using ReportDao = CommonSTDUtil.CommonSTDUtil.CommonOutputReportDataClass;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using TMQConst = CommonTMQUtil.CommonTMQConstants;
using GroupId = CommonTMQUtil.CommonTMQConstants.MsStructure.GroupId;

namespace BusinessLogic_PT0001
{
    /// <summary>
    /// RFタグ取込画面
    /// </summary>
    public partial class BusinessLogic_PT0001 : CommonBusinessLogicBase
    {
        /// <summary>
        /// ファイルを取込み、入力チェック、登録を行う
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool uploadRftag()
        {
            if (this.InputStream == null)
            {
                // 取込画面を閉じない
                setControlFlg();
                // 「アップロード可能なファイルがありません。」
                this.MsgId = GetResMessage(ComRes.ID.ID941010006);
                return false;
            }
            //ファイル情報
            var file = this.InputStream[0];
            // ファイル拡張子チェック
            string extension = Path.GetExtension(file.FileName);
            if (extension != ComUtil.FileExtension.TXT)
            {
                // 取込画面を閉じない
                setControlFlg();
                // 「ファイル形式が有効ではありません。」
                this.MsgId = GetResMessage(ComRes.ID.ID941280004);
                return false;
            }

            // エラー内容格納クラス
            List<ComBase.UploadErrorInfo> errorInfoList = new List<ComBase.UploadErrorInfo>();

            // ファイル読込
            TMQUtil.UploadText uploadText = new TMQUtil.UploadText(file.OpenReadStream(), Encoding.UTF8, ComUtil.CharacterConsts.Comma, "\n", this.LanguageId, this.messageResources, this.db);
            //行番号
            int rowNo = 1;
            //取込データ
            List<Dao.rftagFileInfo> dataList = new();
            foreach (List<string> row in uploadText.getTextData())
            {
                List<ComBase.UploadErrorInfo> tmpErrorInfoList = checkError(row, rowNo, dataList);
                setErrorInfo(ref errorInfoList, tmpErrorInfoList);
                rowNo++;
            }
            if (errorInfoList != null && errorInfoList.Count > 0)
            {
                //出力するエラーの設定
                setOutputMessage(errorInfoList);
                return false;
            }

            //一時テーブルを作成
            //TODO:予備品Noが整数型に変更されるタイミングで下記修正
            bool returnFlag = TMQUtil.SqlExecuteClass.Regist(SqlName.RFUpload.CreateTempTableRftagData, SqlName.SubDir, null, db);

            //権限チェック
            bool notRegistFlg = false;
            //取込対象データ有無
            bool existData = false;
            foreach (Dao.rftagFileInfo data in dataList)
            {
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.RFUpload.GetCountPartsNoByFactory, out string outSql);
                //TODO:予備品Noが整数型に変更されるタイミングで下記修正
                //int count = db.GetEntityByDataClass<int>(outSql, new { UserId = this.UserId, PartsNo = Convert.ToInt64(data.PartsNo) });
                int count = db.GetEntityByDataClass<int>(outSql, new { UserId = this.UserId, PartsNo = data.PartsNo });
                if (count == 0)
                {
                    //権限がない場合、スキップ
                    notRegistFlg = true;
                    continue;
                }

                //一時テーブルへ取込データを登録
                returnFlag = TMQUtil.SqlExecuteClass.Regist(SqlName.RFUpload.InsertTempTableRftagData, SqlName.SubDir, data, db);
                if (!returnFlag)
                {
                    return false;
                }
                existData = true;
            }
            if (!existData)
            {
                //メッセージを設定（取込画面を閉じない）
                setControlFlg();
                // 「取込可能なデータが１件も存在しません。」
                this.MsgId = GetResMessage(ComRes.ID.ID141200009);
                return false;
            }
            //RFタグ予備品マスタへ登録
            //TODO:予備品Noが整数型に変更されるタイミングで下記修正
            returnFlag = TMQUtil.SqlExecuteClass.Regist(SqlName.RFUpload.InsertRftagPartsLink, SqlName.SubDir, new { UserId = this.UserId }, db);
            if (!returnFlag)
            {
                setControlFlg();
                // 「取込可能なデータが１件も存在しません。」
                this.MsgId = GetResMessage(ComRes.ID.ID141200009);
                return false;
            }

            //取込完了後、一覧画面に出力するメッセージを設定
            string msg = null;
            if (notRegistFlg)
            {
                //「権限のない予備品は処理が行われませんでした。」
                msg = GetResMessage(ComRes.ID.ID141090007);
            }
            else
            {
                //「取込処理に成功しました。」
                msg = GetResMessage(new string[] { ComRes.ID.ID941220001, ComRes.ID.ID911200004 });
            }
            //グローバルリストに設定
            SetGlobalData(ConductInfo.FormRFUpload.GlobalKey, msg);

            return true;
        }

        /// <summary>
        /// ファイルの内容チェック
        /// </summary>
        /// <param name="row">行データ</param>
        /// <param name="rowNo">行番号</param>
        /// <returns>エラー内容</returns>
        private List<ComBase.UploadErrorInfo> checkError(List<string> row, int rowNo, List<Dao.rftagFileInfo> dataList)
        {
            // エラー内容一時格納クラス
            List<ComBase.UploadErrorInfo> tmpErrorInfoList = new List<ComBase.UploadErrorInfo>();

            if (row.Count == 0 || string.IsNullOrWhiteSpace(row[0]))
            {
                //空行はスキップ
                return tmpErrorInfoList;
            }
            //行データ(改行コードを取り除く)
            string rowData = row[0].Replace("\r", "");

            //レコード長チェック
            if (rowData.Length != ConductInfo.FormRFUpload.FileRowLength)
            {
                // 「ファイルレイアウトが有効ではありません。」
                string msg = GetResMessage(ComRes.ID.ID941280008);
                // エラーを設定し、スキップ
                tmpErrorInfoList.Add(TMQUtil.setTmpErrorInfo(rowNo, 0, null, msg, 0));
                return tmpErrorInfoList;
            }

            //各項目を取得（固定長のため空白を除去）
            Dao.rftagFileInfo data = new();
            data.RftagId = rowData.Substring(0, 21).Trim();
            data.PartsNo = rowData.Substring(21, 5).Trim();
            data.DepartmentCode = rowData.Substring(27, 6).Trim();
            data.AccountCode = rowData.Substring(34, 5).Trim();
            data.ReadDatetimeStr = rowData.Substring(39, 17).Trim();

            //桁数チェック
            if (string.IsNullOrWhiteSpace(data.RftagId) || string.IsNullOrWhiteSpace(data.PartsNo) || string.IsNullOrWhiteSpace(data.DepartmentCode) ||
                string.IsNullOrWhiteSpace(data.AccountCode) || string.IsNullOrWhiteSpace(data.ReadDatetimeStr))
            {
                // 「ファイルレイアウトが有効ではありません。」
                string msg = GetResMessage(ComRes.ID.ID941280008);
                // エラーを設定し、スキップ
                tmpErrorInfoList.Add(TMQUtil.setTmpErrorInfo(rowNo, 0, null, msg, 0));
                return tmpErrorInfoList;
            }

            //整合性チェック
            if (!ComUtil.IsLong(data.PartsNo))
            {
                //予備品Noが整数型でない場合、エラー

                // 「{0}が不正です。」
                string msg = GetResMessage(new string[] { ComRes.ID.ID941060003, ComRes.ID.ID111380022 });
                // エラーを設定
                tmpErrorInfoList.Add(TMQUtil.setTmpErrorInfo(rowNo, 0, null, msg, 0));
            }
            else
            {
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Edit.GetPartsNoCount, out string outSql);
                //TODO:予備品Noが整数型に変更されるタイミングで下記修正
                //if (db.GetEntityByDataClass<int>(outSql, new { PartsNo = Convert.ToInt64(data.PartsNo) }) == 0)
                if (db.GetEntityByDataClass<int>(outSql, new { PartsNo = data.PartsNo }) == 0)
                {
                    //予備品Noが予備品仕様マスタに登録されていない場合、エラー

                    // 「予備品仕様マスタに存在しません。」
                    string msg = GetResMessage(ComRes.ID.ID141380003);
                    // エラーを設定
                    tmpErrorInfoList.Add(TMQUtil.setTmpErrorInfo(rowNo, 0, null, msg, 0));
                }
            }

            //部門コードから部門IDを取得
            data.DepartmentStructureId = getDepartmentId(data.DepartmentCode, data.PartsNo);
            if (data.DepartmentStructureId <= 0)
            {
                // 「部門コードが存在しません。」
                string msg = GetResMessage(ComRes.ID.ID141280003);
                // エラーを設定
                tmpErrorInfoList.Add(TMQUtil.setTmpErrorInfo(rowNo, 0, null, msg, 0));
            }

            //勘定科目コードから勘定科目IDを取得
            data.AccountStructureId = getStrucutureId((int)GroupId.Account, 1, data.AccountCode);
            if (data.AccountStructureId <= 0)
            {
                // 「勘定科目コードが存在しません。」
                string msg = GetResMessage(ComRes.ID.ID141060010);
                // エラーを設定
                tmpErrorInfoList.Add(TMQUtil.setTmpErrorInfo(rowNo, 0, null, msg, 0));
            }

            if (!ComUtil.IsDateTimeFormat(data.ReadDatetimeStr, ConductInfo.FormRFUpload.ReadDatetimeFormat))
            {
                //読取日時が日時に変換できない場合、エラー

                // 「{0}が不正です。」
                string msg = GetResMessage(new string[] { ComRes.ID.ID941060003, ComRes.ID.ID111380072 });
                // エラーを設定
                tmpErrorInfoList.Add(TMQUtil.setTmpErrorInfo(rowNo, 0, null, msg, 0));
            }
            else
            {
                data.ReadDatetime = DateTime.ParseExact(data.ReadDatetimeStr, ConductInfo.FormRFUpload.ReadDatetimeFormat, null);
            }

            dataList.Add(data);
            return tmpErrorInfoList;

            //拡張データから構成IDを取得(標準アイテムのみを対象)
            long? getStrucutureId(int groupId, int seq, string extensionData)
            {
                //構成アイテムを取得するパラメータ設定
                TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();
                //構成グループID
                param.StructureGroupId = groupId;
                //連番
                param.Seq = seq;
                //拡張データ
                param.ExData = extensionData;

                //構成アイテム情報取得
                List<TMQUtil.StructureItemEx.StructureItemExInfo> list = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.db);
                if (list.Count == 0 || !list.Exists(x => x.FactoryId == TMQConst.CommonFactoryId))
                {
                    return -1;
                }

                //構成IDを戻す(標準アイテムのみを対象)
                return list.Where(x => x.FactoryId == TMQConst.CommonFactoryId).Select(x => x.StructureId).FirstOrDefault();
            }

            // 部門コードから部門IDを取得
            long? getDepartmentId(string extensionData, string partsNo)
            {
                // 部門コードから部門IDを取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.RFUpload.GetDepartmentIdByCode, out string outSql);

                // ①　ログインユーザの本務工場の地区配下の工場で予備品Noを条件に予備品を検索(予備品Noは地区内一意なので特定できるはず)
                // ②　①で特定した予備品の工場(pt_parts.factory_id)の工場アイテムで、アップロードされたファイルの部門コードの部門アイテムのID(ms_structure.structure_id)を取得
                // ③　②で部門IDが取得できれば使用し、取得できなければ標準アイテムの部門アイテムのIDを取得する
                long? retVal = db.GetEntity<long?>(outSql, new { UserId = this.UserId, ExtensionData = extensionData, PartsNo = partsNo });

                // 取得した部門IDを返す(取得できなかった場合は「0」)
                return retVal == null ? 0 : retVal;
            }
        }

        /// <summary>
        /// エラー情報をマージして設定
        /// </summary>
        /// <param name="errorInfoList">エラー情報リスト</param>
        /// <param name="tmpErrorInfo">対象行のエラー情報</param>
        private void setErrorInfo(ref List<ComBase.UploadErrorInfo> errorInfoList, List<ComBase.UploadErrorInfo> tmpErrorInfo)
        {
            foreach (ComBase.UploadErrorInfo tmp in tmpErrorInfo)
            {
                //エラー内容が同じものが設定されているかチェック(メッセージはエラーの種類ごとにまとめて表示する為)
                int count = errorInfoList.Where(x => x.ErrorInfo == tmp.ErrorInfo).Count();
                if (count > 0)
                {
                    //対象のインデックス番号を取得
                    int index = errorInfoList.Select((e, index) => (e, index)).Where(x => x.e.ErrorInfo == tmp.ErrorInfo).Select(x => x.index).DefaultIfEmpty(-1).First();
                    // 行番号を追加
                    if (index >= 0)
                    {
                        if (!errorInfoList[index].RowNo.Contains(tmp.RowNo[0]))
                        {
                            errorInfoList[index].RowNo.Add(tmp.RowNo[0]);
                        }
                    }
                    else
                    {
                        errorInfoList.Add(tmp);
                    }
                }
                else
                {
                    errorInfoList.Add(tmp);
                }
            }
        }

        /// <summary>
        /// 出力するエラーメッセージの設定
        /// </summary>
        /// <param name="errorInfoList">エラー内容格納クラス</param>
        private void setOutputMessage(List<ComBase.UploadErrorInfo> errorInfoList)
        {
            //出力するエラーの整形
            List<string> outputMessages = new List<string>();
            foreach (ComDao.UploadErrorInfo error in errorInfoList)
            {
                string errRow = string.Join(",", error.RowNo);
                // エラーメッセージ（例：「予備品Noが不正です。(1,2行目)」）
                outputMessages.Add(error.ErrorInfo + "(" + errRow + GetResMessage(ComRes.ID.ID141070004) + ")");
            }

            //エラー内容を画面に設定
            Dao.uploadInfo info = new Dao.uploadInfo();
            if (outputMessages != null)
            {
                // エラーメッセージの設定
                info.ErrorMessage = string.Join('\n', outputMessages);
                setControlFlg();
            }
            var pageInfo = GetPageInfo(ConductInfo.FormRFUpload.ControlId.Info, this.pageInfoList);
            SetSearchResultsByDataClass<Dao.uploadInfo>(pageInfo, new List<Dao.uploadInfo> { info }, 1);

            // 「入力エラーがあります。」
            this.MsgId = GetResMessage(ComRes.ID.ID941220005);

        }

        /// <summary>
        /// 取込画面を閉じないための制御用フラグを設定
        /// </summary>
        private void setControlFlg()
        {
            //取込画面を閉じないための制御用フラグを設定
            Dao.uploadInfo info = new Dao.uploadInfo();
            info.Flg = true;
            var pageInfo = GetPageInfo(ConductInfo.FormRFUpload.ControlId.Hide, this.pageInfoList);
            SetSearchResultsByDataClass<Dao.uploadInfo>(pageInfo, new List<Dao.uploadInfo> { info }, 1);
        }
    }
}
