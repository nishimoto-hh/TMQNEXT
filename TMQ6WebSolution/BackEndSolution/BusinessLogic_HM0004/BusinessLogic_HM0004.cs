using CommonExcelUtil;
using CommonSTDUtil;
using CommonSTDUtil.CommonBusinessLogic;
using CommonWebTemplate.Models.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_HM0004.BusinessLogicDataClass_HM0004;
using TMQConst = CommonTMQUtil.CommonTMQConstants;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using GroupId = CommonTMQUtil.CommonTMQConstants.MsStructure.GroupId;

namespace BusinessLogic_HM0004
{
    /// <summary>
    /// 変更管理 帳票出力
    /// </summary>
    public class BusinessLogic_HM0004 : CommonBusinessLogicBase
    {
        #region private変数
        #endregion

        #region 定数

        /// <summary>
        /// 処理対象コントロールID
        /// </summary>
        private static class TargetCtrlId
        {
            /// <summary>
            /// 検索条件 場所階層のコントロールID
            /// </summary>
            public const string LocationInfo = "CCOND_020_00_LST_0";
            /// <summary>
            /// 検索条件 職種機種のコントロールID
            /// </summary>
            public const string JobInfo = "CCOND_030_00_LST_0";
            /// <summary>
            /// 非表示項目のコントロールID
            /// </summary>
            public const string HideInfo = "CCOND_080_00_LST_0";
            /// <summary>
            /// 警告メッセージ表示一覧
            /// </summary>
            public const string WarningComment = "CCOND_110_00_LST_0";
            /// <summary>
            /// 検索条件のグループ番号リスト
            /// </summary>
            public static ReadOnlyCollection<int> GroupNoList { get; } = new[] { 1000, 1003, 1004, 1005 }.ToList().AsReadOnly();

            public static class Button
            {
                /// <summary>出力</summary>
                public const string Output = "Output";
            }
        }

        /// <summary>
        /// 帳票ID
        /// </summary>
        private static class ReportId
        {
            /// <summary>機器台帳 変更履歴一覧</summary>
            public const string Machine = "RP0420";
            /// <summary>長期計画 変更履歴一覧</summary>
            public const string LongPlan = "RP0430";
        }

        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = @"Common\HistoryManagement";
            /// <summary>警告メッセージ取得SQL</summary>
            public const string GetWarningComment = "GetWarningComment";
        }
            #endregion

            #region コンストラクタ
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public BusinessLogic_HM0004() : base()
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
            this.ResultList = new();

            //遷移元から受け取ったパラメータを取得
            var targetDic = ComUtil.GetDictionaryListByCtrlId(this.searchConditionDictionary, TargetCtrlId.HideInfo, true);
            //遷移元から設定されたパラメータと、本画面で検索条件として設定(search_condition_add)しているデータ2行取得できるのでパラメータが設定されている方を採用
            foreach (Dictionary<string, object> target in targetDic)
            {
                TMQUtil.HistoryCondition param = new TMQUtil.HistoryCondition();
                SetDataClassFromDictionary(target, TargetCtrlId.HideInfo, param, new List<string> { "ConductCode" });

                if (param.ConductCode == null)
                {
                    continue;
                }

                // ページ情報取得
                var pageInfo = GetPageInfo(TargetCtrlId.HideInfo, this.pageInfoList);
                // 値設定
                if (!SetSearchResultsByDataClass<TMQUtil.HistoryCondition>(pageInfo, new List<TMQUtil.HistoryCondition> { param }, 1))
                {
                    return ComConsts.RETURN_RESULT.NG;
                }
            }

            // 警告コメントを設定する
            setWarningComment();

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            return ComConsts.RETURN_RESULT.OK;

            // 警告コメントを設定する
            void setWarningComment()
            {
                // 一覧のページ情報取得
                var pageInfo = GetPageInfo(TargetCtrlId.WarningComment, this.pageInfoList);

                // SQLを取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetWarningComment, out string commentSql);

                // 一覧検索実行
                IList<Dao.WarningComment> comment = db.GetListByDataClass<Dao.WarningComment>(commentSql.ToString(), new { LanguageId = this.LanguageId });

                // 検索結果の設定
                SetFormByDataClass(TargetCtrlId.WarningComment, comment);
            }
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int SearchImpl()
        {
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
            this.ResultList = new();

            // 画面情報取得
            TMQUtil.HistoryCondition condition = getSearchCondition();

            // 帳票ID
            string reportId = condition.ConductCode == ((int)TMQConst.MsStructure.StructureId.OutputItemConduct.HM0001).ToString() ? ReportId.Machine : ReportId.LongPlan;
            // 個別工場ID設定の帳票定義の存在を確認して、存在しない場合は共通の工場IDを設定する
            int userFactoryId = TMQUtil.GetUserFactoryId(this.UserId, this.db);
            int reportFactoryId = TMQUtil.IsExistsFactoryReportDefine(userFactoryId, this.PgmId, reportId, this.db) ? userFactoryId : 0;

            // エクセル出力共通処理
            bool result = TMQUtil.CommonOutputExcel(
                reportFactoryId,            // 工場ID
                this.PgmId,                       // プログラムID
                null,                        // シートごとのパラメータでの選択キー情報リスト
                null,               // 検索条件
                reportId,                    // 帳票ID
                1,                  // テンプレートID
                1,             // 出力パターンID
                this.UserId,                 // ユーザID
                this.LanguageId,             // 言語ID
                this.conditionSheetLocationList,    // 場所階層構成IDリスト
                this.conditionSheetJobList,         // 職種機種構成IDリスト
                this.conditionSheetNameList,        // 検索条件項目名リスト
                this.conditionSheetValueList,       // 検索条件設定値リスト
                out string fileType,         // ファイルタイプ
                out string fileName,         // ファイル名
                out MemoryStream memStream,  // メモリストリーム
                out string message,          // メッセージ
                db,
                null,
                null,
                null,
                null,
                condition,
                this.messageResources);

            if (!result)
            {
                if (string.IsNullOrEmpty(message))
                {
                    // 「出力処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911120006 });
                }
                else
                {
                    this.MsgId = message;
                }

                this.Status = CommonProcReturn.ProcStatus.Warning;
                return ComConsts.RETURN_RESULT.NG;
            }

            // OUTPUTパラメータに設定
            this.OutputFileType = fileType;
            this.OutputFileName = fileName;
            this.OutputStream = memStream;

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            return ComConsts.RETURN_RESULT.OK;

        }

        #endregion

        #region privateメソッド
        /// <summary>
        /// 一覧検索処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private TMQUtil.HistoryCondition getSearchCondition()
        {
            // 検索条件を画面より取得してデータクラスへセット
            TMQUtil.HistoryCondition condition = GetConditionInfoByGroupNoList<TMQUtil.HistoryCondition>(TargetCtrlId.GroupNoList.ToList());
            // 検索条件（場所階層）
            condition.LocationStructureIds = getLocationJobList(true);
            // 検索条件（職種機種）
            condition.JobStructureIds = getLocationJobList(false);
            // 使用区分追加条件
            condition.UseSegmentIsNull = condition.UseSegmentStructureIdList.Count == 0 && condition.UseSegmentFlg ? condition.UseSegmentFlg.ToString() : null;
            // データクラスの中で値がNullでないものをSQLの検索条件に含めるので、メンバ名を取得
            condition.ListUnComment = ComUtil.GetNotNullNameByClass<TMQUtil.HistoryCondition>(condition);

            return condition;
        }

        /// <summary>
        /// 検索条件（場所階層、職種機種）をデータクラスで取得(リスト)
        /// </summary>
        /// <param name="isLocation">場所階層の場合true</param>
        /// <returns>構成IDカンマ区切りの文字列</returns>
        private string getLocationJobList(bool isLocation)
        {
            string ctrlId = isLocation ? TargetCtrlId.LocationInfo : TargetCtrlId.JobInfo;
            // 画面項目定義の情報
            var mappingInfo = getResultMappingInfo(ctrlId);
            // コントロールIDにより画面の項目(一覧)を取得
            List<Dictionary<string, object>> resultList = ComUtil.GetDictionaryListByCtrlId(this.searchConditionDictionary, ctrlId, true);
            // 構成IDのリスト
            List<int> structureIdList = new();
            IList<Dao.locationJobCondition> list = new List<Dao.locationJobCondition>();
            // 一覧を繰り返し、データクラスに変換、リストへ追加する
            foreach (var resultRow in resultList)
            {
                Dao.locationJobCondition info = new();
                //データクラスに変換
                SetDataClassFromDictionary(resultRow, ctrlId, info);

                // 共通処理で階層IDを設定するが、画面の検索条件は画面定義の関係でプロパティ名にIDでなく名称を使用しているので置き換える
                if (isLocation)
                {
                    // 場所階層
                    info.DistrictId = info.DistrictName;
                    info.FactoryId = info.FactoryName;
                    info.PlantId = info.PlantName;
                    info.SeriesId = info.SeriesName;
                    info.StrokeId = info.StrokeName;
                    info.FacilityId = info.FacilityName;
                }
                else
                {
                    // 職種
                    info.JobId = info.JobName;
                    info.LargeClassficationId = info.LargeClassficationName;
                    info.MiddleClassficationId = info.MiddleClassficationName;
                    info.SmallClassficationId = info.SmallClassficationName;
                }

                list.Add(info);
            }

            if (list.Count != 0)
            {
                //設定されている最下層の構成IDを取得する
                TMQUtil.StructureLayerInfo.setBottomLayerStructureIdToDataClass<Dao.locationJobCondition>(ref list, new List<StructureType> { isLocation ? StructureType.Location : StructureType.Job });
                structureIdList.AddRange(list.Select(x => isLocation ? x.LocationStructureId ?? -1 : x.JobStructureId ?? -1).Distinct().ToList());
            }
            if (structureIdList.Count == 0 || (structureIdList.Count == 1 && structureIdList[0] == -1))
            {
                if (isLocation && (this.BelongingInfo.LocationInfoList != null && this.BelongingInfo.LocationInfoList.Count > 0))
                {
                    // 場所階層の指定なしの場合、所属場所階層を渡す
                    structureIdList = getBelongLocationIdList();
                }
                else if (!isLocation && (this.BelongingInfo.JobInfoList != null && this.BelongingInfo.JobInfoList.Count > 0))
                {
                    // 職種機種の指定なしの場合、所属職種機種を渡す
                    structureIdList = this.BelongingInfo.JobInfoList.Select(x => x.StructureId).ToList();
                }
                if (structureIdList.Count == 0)
                {
                    //取得できない場合、データを表示しない
                    return "-1";
                }
            }

            //配下の構成IDを取得しカンマ区切りの文字列にする
            string structureIds = string.Join(',', GetLowerStructureIdList(structureIdList));
            return structureIds;
        }

        #endregion
    }
}