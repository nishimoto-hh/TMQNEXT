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
using ComConsts = CommonSTDUtil.CommonConstants;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using Dao = BusinessLogic_EP0002.BusinessLogicDataClass_EP0002;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic_EP0002
{
    /// <summary>
    /// ExcelPortアップロード
    /// </summary>
    public class BusinessLogic_EP0002 : CommonBusinessLogicBase
    {
        #region private変数
        #endregion

        #region 定数
        /// <summary>
        /// フォーム種類
        /// </summary>
        private static class FormType
        {
            /// <summary>Excel Port アップロード画面</summary>
            public const byte Upload = 0;
        }

        /// <summary>
        /// 処理対象コントロールID
        /// </summary>
        private static class TargetCtrlId
        {
            /// <summary>
            /// Excel Port アップロード条件のコントロールID
            /// </summary>
            public const string ExcelPortUpdList = "BODY_000_00_LST_0";

            /// <summary>
            /// Excel Port アップロード ボタングループのコントロールID
            /// </summary>
            public const string ExcelPortUpdBtn = "BODY_010_00_BTN_0";
        }

        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDirExcelPort = @"ExcelPort";
        }
        /// <summary>
        /// 拡張データに持っている取得条件
        /// </summary>
        public static class condPartsType
        {
            /// <summary>
            /// Excel Port バージョン
            /// </summary>
            public static class excelPortVer
            {
                /// <summary>構成グループID</summary>
                public const short StructureGroupId = 9210;
                /// <summary>構成ID</summary>
                public const short StructureId = 1370;
                /// <summary>連番</summary>
                public const short Seq = 1;
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_EP0002() : base()
        {
        }
        #endregion

        #region オーバーライドメソッド
        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int InitImpl()
        {
            // 初期検索実行
            return InitSearch();
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int SearchImpl()
        {
            switch (this.FormNo)
            {
                case FormType.Upload:     // 一覧検索
                    if (!searchList())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                default:
                    // この部分は到達不能なので、エラーを返す
                    return ComConsts.RETURN_RESULT.NG;
            }
            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 実行処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int ExecuteImpl()
        {
            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int RegistImpl()
        {
            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int DeleteImpl()
        {
            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 出力処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int ReportImpl()
        {
            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 取込処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int UploadImpl()
        {
            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// ExcelPortアップロード個別処理
        /// </summary>
        /// <param name="file">アップロード対象ファイル</param>
        /// <param name="fileType">ファイル種類</param>
        /// <param name="fileName">ファイル名</param>
        /// <param name="ms">メモリストリーム</param>
        /// <param name="resultMsg">結果メッセージ</param>
        /// <param name="detailMsg">詳細メッセージ</param>
        /// <remarks>アップロード条件の確認後は各機能の個別処理を実行する</remarks>
        /// <returns>実行結果(0:OK/0未満:NG)</returns>
        protected override int ExcelPortUploadImpl(IFormFile file, ref string fileType, ref string fileName, ref MemoryStream ms, ref string resultMsg, ref string detailMsg)
        {
            // ExcelPortクラスの生成
            var excelPort = new TMQUtil.ComExcelPort(
                this.db, this.UserId, this.BelongingInfo, this.LanguageId, this.FormNo, this.searchConditionDictionary, this.messageResources);

            // ExcelPortテンプレートファイル情報初期化
            this.Status = CommonProcReturn.ProcStatus.Valid;
            if (!excelPort.InitializeExcelPortTemplateFile(out resultMsg, out detailMsg))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                return ComConsts.RETURN_RESULT.NG;
            }

            // ExcelPortアップロード条件チェック
            string conductId = string.Empty;
            int sheetNo = 0;
            if(!excelPort.CheckUploadCondition(file, out resultMsg, out conductId, out sheetNo))
            {
                this.Status = CommonProcReturn.ProcStatus.Error;
                this.MsgId = resultMsg;
                return ComConsts.RETURN_RESULT.NG;
            }

            // ExcelPortアップロード条件を個別実装条件へ設定
            this.IndividualDictionary.Add(TMQUtil.ComExcelPort.ConditionValName.TargetConductId, conductId);
            this.IndividualDictionary.Add(TMQUtil.ComExcelPort.ConditionValName.TargetSheetNo, sheetNo);

            // 確認メッセージ表示
            this.Status = CommonProcReturn.ProcStatus.Confirm;
            this.LogNo = CommonProcReturn.ProcStatus.Confirm.ToString();
            this.MsgId = resultMsg;
            return ComConsts.RETURN_RESULT.OK;
        }
        #endregion

        #region privateメソッド
        /// <summary>
        /// Excel Port アップロード条件
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchList()
        {
            var result = new Dao.searchExcelPortUpdCondition();
            //Excel Portバージョンを取得
            result.ExcelPortVer = getItemExData(condPartsType.excelPortVer.StructureGroupId, condPartsType.excelPortVer.Seq, condPartsType.excelPortVer.StructureId);

            // ページ情報取得
            var pageInfo = GetPageInfo(TargetCtrlId.ExcelPortUpdList, this.pageInfoList);

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.searchExcelPortUpdCondition>(pageInfo, new List<Dao.searchExcelPortUpdCondition> { result }, 1))
            {
                // 正常終了
                return true;
            }
            return false;
        }

        /// <summary>
        /// アイテム拡張マスタから拡張データを取得する
        /// </summary>>
        /// <param name="structureGroupId">構成グループID</param>
        /// <param name="seq">連番</param
        /// <param name="structureId">構成ID</param>
        /// <returns>拡張データ</returns>
        private string getItemExData(short structureGroupId, short seq, int structureId)
        {
            string result = null;

            // 構成アイテムを取得するパラメータ設定
            TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();
            // 構成グループID
            param.StructureGroupId = (int)structureGroupId;
            // 連番
            param.Seq = seq;
            // 構成アイテム、アイテム拡張マスタ情報取得
            List<TMQUtil.StructureItemEx.StructureItemExInfo> list = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.db);
            if (list != null)
            {
                // 取得情報から拡張データを取得
                result = list.Where(x => x.StructureId == structureId).Select(x => x.ExData).FirstOrDefault();
            }
            return result;
        }
        #endregion

        #region publicメソッド
        #endregion
    }
}