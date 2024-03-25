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
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_RM0001.BusinessLogicDataClass_RM0001;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using ReportDao = CommonSTDUtil.CommonSTDUtil.CommonOutputReportDataClass;

namespace BusinessLogic_RM0001
{
    /// <summary>
    /// 出力パターン登録画面
    /// </summary>
    public partial class BusinessLogic_RM0001 : CommonBusinessLogicBase
    {

        /// <summary>
        /// 編集画面　登録処理
        /// </summary>
        /// <param name="ctrlId">ボタンコントロールID</param>
        /// <param name="programId">機能ID</param>
        /// <param name="patternIdForReSearch">新規登録後のパターンID(再検索で使用)</param>
        /// <returns>エラーの場合False</returns>
        private bool executeRegistEdit(string ctrlId, string programId, out int? patternIdForReSearch)
        {
            patternIdForReSearch = null;

            // 編集、削除の場合
            if (ctrlId.Equals(ConductInfo.FormList.ButtonCtrlId.Regist))
            {
                // 排他チェック
                if (isErrorExclusive())
                {
                    return false;
                }
            }
            // 画面情報取得
            DateTime now = DateTime.Now;
            // 出力パターンマスタ　登録
            ReportDao.MsOutputPatternEntity registPattern = getRegistInfo<ReportDao.MsOutputPatternEntity>(ConductInfo.FormOutPattern.ControlId.PatternName, now);
            // 検索条件を取得
            Dao.searchCondition condition = getSearchInfo<Dao.searchCondition>();

            registPattern.FactoryId = condition.FactoryId;   // 工場ID
            registPattern.ReportId = condition.ReportId;     // 帳票ID
            registPattern.TemplateId = condition.TemplateId; // テンプレートID

            // 入力チェック
            if (isErrorRegist(registPattern, ctrlId))
            {
                return false;
            }

            // 既存項目の削除
            // 編集の場合
            if (ctrlId.Equals(ConductInfo.FormList.ButtonCtrlId.Regist))
            {
                // 出力項目の削除
                if (!executeDelete(ctrlId))
                {
                    // 削除エラー
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return false;
                }
            }

            // 登録帳票定義を取得
            ReportDao.MsOutputReportDefineEntity defineInfo = new ReportDao.MsOutputReportDefineEntity();
            var resultDefine = defineInfo.GetEntity(condition.FactoryId, programId, condition.ReportId, this.db);
            if (resultDefine == null)
            {
                // エラーの場合
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            // 表示順登録用変数
            int displayOrder = 1;
            // 出力パターンID保持変数
            int outputPatternId = 0;

            // 入力された内容を取得
            IList<ReportDao.MsOutputItemEntity> registResults = getRegistInfoList<ReportDao.MsOutputItemEntity>(ConductInfo.FormList.ControlId.List, now);

            // 出力順から、デフォルトセル行、デフォルトセル列を決定
            Dictionary<int, List<int>> dicDefaultCellInfo = new Dictionary<int, List<int>>();
            // 出力パターン登録時、べた表の場合のみ
            // 出力順から、デフォルトセル行、デフォルトセル列を決定する
            if (resultDefine.OutputItemType == 3)
            {
                // 出力順と紐づく出力先のセル行、セル列のリストを取得する
                // べた表のため、シートNoはデフォルト値
                dicDefaultCellInfo = GetDefaultCellInfo(resultDefine.FactoryId, resultDefine.ReportId, OutputPatternDefaultValue.SheetNo, registResults);
            }

            foreach (ReportDao.MsOutputItemEntity registResult in registResults)
            {
                // 初回のみ
                // 出力パターンに対する変更
                if (displayOrder == 1)
                {
                    // 新規の場合のみ、出力パターンマスタを登録
                    if (ctrlId.Equals(ConductInfo.FormOutPattern.ButtonCtrlId.Regist))
                    {
                        // 出力パターンIDの新規採番用に登録済の最大パターンIDを取得する
                        int maxOutputPatternId = GetMaxOutputPatternId(registResult.FactoryId, registResult.ReportId, registResult.TemplateId);

                        // ms_output_patternの出力パターンMAX値（工場ID、帳票ID、テンプレートID）+ 1
                        outputPatternId = maxOutputPatternId + 1;
                        registPattern.OutputPatternId = maxOutputPatternId + 1;

                        // 出力パターンの登録
                        // 登録SQL取得
                        if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.InsertOutputPattern, out string sql))
                        {
                            this.Status = CommonProcReturn.ProcStatus.Error;
                            return false;
                        }

                        int result = this.db.Regist(sql, registPattern);
                        if (result < 0)
                        {
                            // 登録エラー
                            this.Status = CommonProcReturn.ProcStatus.Error;
                            return false;
                        }

                        // 出力パターンが登録されている場合、シート２以降のデータを内部でコピーする
                        if (maxOutputPatternId > 0)
                        {
                            ReportDao.MsOutputPatternEntity registPatternItem = new ReportDao.MsOutputPatternEntity();
                            registPatternItem.FactoryId = registResult.FactoryId;
                            registPatternItem.ReportId = registResult.ReportId;
                            registPatternItem.TemplateId = registResult.TemplateId;
                            registPatternItem.OutputPatternId = maxOutputPatternId;
                            // 出力項目の登録
                            if (!registDb(registPatternItem, SqlName.InsertOutputItemForSheets))
                            {
                                // 該当データなしの場合、処理を継続する
                                //// 登録エラー
                                //this.Status = CommonProcReturn.ProcStatus.Error;
                                //return false;
                            }
                        }
                    }
                }

                // 出力順
                registResult.DisplayOrder = displayOrder;

                // 出力パターン登録時、べた表の場合のみ
                // 出力順から、セル行、セル列を決定する
                if (resultDefine.OutputItemType == 3)
                {
                    if (dicDefaultCellInfo.ContainsKey(displayOrder))
                    {
                        List<int> lstDefaultCellInfo = dicDefaultCellInfo[displayOrder];
                        registResult.DefaultCellRowNo = lstDefaultCellInfo[0];
                        registResult.DefaultCellColumnNo = lstDefaultCellInfo[1];
                    }
                }

                // 出力パターンIDの更新
                if (ctrlId.Equals(ConductInfo.FormOutPattern.ButtonCtrlId.Regist))
                {
                    // 新規の場合、事前に採番済の出力パターンIDを設定
                    registResult.OutputPatternId = outputPatternId;
                }
                else if (ctrlId.Equals(ConductInfo.FormList.ButtonCtrlId.Regist))
                {
                    // 修正の場合、検索条件で指定した出力パターンIDを設定
                    registResult.OutputPatternId = condition.OutputPatternId;
                }

                // 出力項目の登録
                if (!registDb(registResult, SqlName.InsertOutputItem))
                {
                    // 登録エラー
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return false;
                }

                // 標準のカウントアップ
                displayOrder += 1;
            }

            // グローバル変数に登録パターンIDをセットする
            SetGlobalData(GlobalListKeyRM0001.RegistFactoryId, registPattern.FactoryId);
            SetGlobalData(GlobalListKeyRM0001.RegistTemplateId, registPattern.TemplateId);
            SetGlobalData(GlobalListKeyRM0001.RegistPatternId, outputPatternId == 0 ? condition.OutputPatternId : outputPatternId);

            // パターンの新規登録の場合は登録したパターンIDで再検索を行うため返り値に設定する
            if (ctrlId.Equals(ConductInfo.FormOutPattern.ButtonCtrlId.Regist))
            {
                patternIdForReSearch = outputPatternId;
            }

            return true;

            /// <summary>
            /// 登録処理エラー時の情報設定
            /// </summary>
            void setInsertError()
            {
                // エラー終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 未設定時にエラーメッセージを設定
                if (string.IsNullOrEmpty(this.MsgId))
                {
                    // 「登録処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911200003 });
                }
            }

            /// <summary>
            /// 現在のms_output_patternの出力パターンMAX値（工場ID、帳票ID、テンプレートID）を取得
            /// </summary>
            int GetMaxOutputPatternId(int factoryId, string reportId, int templateId)
            {
                // 最大更新日時取得SQL
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetMaxOutputPatternId, out string outSql);
                ReportDao.MsOutputPatternEntity getMaxOutputPatternIdParam = new ReportDao.MsOutputPatternEntity();

                getMaxOutputPatternIdParam.FactoryId = factoryId;
                getMaxOutputPatternIdParam.ReportId = reportId;
                getMaxOutputPatternIdParam.TemplateId = templateId;
                // SQL実行
                var maxOutputPatternId = db.GetEntity(outSql, getMaxOutputPatternIdParam);
                // パターンがない場合は最大値を０で返す
                if (maxOutputPatternId.max_output_pattern_id == null)
                {
                    return 0;
                }
                return maxOutputPatternId.max_output_pattern_id;
            }

            /// <summary>
            /// 対象帳票定義に含まれる共通カラムの項目idのリストを取得
            /// </summary>
            IList<int> GetExItemCdForComReport(int factoryId, string reportId, int sheetNo)
            {
                string comColumnNameList = string.Empty;
                // 共通カラム名をカンマ区切りにする
                comColumnNameList = string.Join(",", new string[] { TMQUtil.ColumnName.ComTitle, TMQUtil.ColumnName.ComDate, TMQUtil.ColumnName.ComTime });
                var param = new { FactoryId = factoryId, ReportId = reportId, SheetNo = sheetNo, ColumnNameList = comColumnNameList };

                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetItemIdByColumnName, out string outSql);

                // SQL実行
                IList<int> itemCdList = this.db.GetList<int>(outSql, param);
                return itemCdList;
            }

            /// <summary>
            /// 対象帳票の出力開始位置と繰り返し方向を取得
            /// </summary>
            void GetStartCellInfo(int factoryId, string reportId, int sheetNo,
                out int defaultCellRowNo, out int defaultCellColumnNo, out int outputMethod)
            {
                defaultCellRowNo = 0;
                defaultCellColumnNo = 0;
                outputMethod = 0;

                string comColumnNameList = string.Empty;
                // 共通カラム名をカンマ区切りにする
                comColumnNameList = string.Join(",", new string[] { TMQUtil.ColumnName.ComTitle, TMQUtil.ColumnName.ComDate, TMQUtil.ColumnName.ComTime });
                var param = new { FactoryId = factoryId, ReportId = reportId, SheetNo = sheetNo, ExColumnNameList = comColumnNameList };

                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetStartCellInfo, out string outSql);

                // SQL実行
                var outputReportDefineItemInfo = db.GetList(outSql, param);
                // パターンがない場合は最大値を０で返す
                if (outputReportDefineItemInfo != null)
                {
                    // 複数取得の場合、先頭行を取得
                    defaultCellRowNo = outputReportDefineItemInfo[0].default_cell_row_no;
                    defaultCellColumnNo = outputReportDefineItemInfo[0].default_cell_column_no;
                    outputMethod = outputReportDefineItemInfo[0].output_method;
                }
                return;
            }

            /// <summary>
            // 出力順と紐づく出力先のセル行、セル列のリストを取得する
            /// </summary>
            Dictionary<int, List<int>> GetDefaultCellInfo(int factoryId, string reportId, int sheetNo, IList<ReportDao.MsOutputItemEntity> outputItemList)
            {
                int displayOrder = 1;
                Dictionary<int, List<int>> dicDefaultCellInfo = new Dictionary<int, List<int>>();
                IList<int> listExItemCd = new List<int>();

                int defaultCellRowNo = 0;
                int defaultCellColumnNo = 0;
                int outputMethod = 0;
                // 対象帳票の開始位置と繰り返し方向を特定
                GetStartCellInfo(factoryId, reportId, sheetNo,
                    out defaultCellRowNo, out defaultCellColumnNo, out outputMethod);

                // 対象帳票に紐づくシステム共通の項目IDを特定する
                listExItemCd = GetExItemCdForComReport(factoryId, reportId, sheetNo);

                foreach (ReportDao.MsOutputItemEntity outputItem in outputItemList)
                {
                    List<int> lstDefaultCellInfo = new List<int>();

                    // システム共通の項目IDは対象外とする
                    if (listExItemCd.Contains(outputItem.ItemId) == true)
                    {
                        continue;
                    }

                    // 0:DefaultCellRowNo
                    // 1:DefaultCellColumnNo
                    lstDefaultCellInfo.Add(defaultCellRowNo);
                    lstDefaultCellInfo.Add(defaultCellColumnNo);

                    dicDefaultCellInfo.Add(displayOrder, lstDefaultCellInfo);

                    displayOrder += 1;

                    // 繰り返し方向に合わせてカウントアップする
                    if (outputMethod == 2)
                    {
                        defaultCellColumnNo += 1;
                    }
                    else
                    {
                        defaultCellRowNo += 1;
                    }
                }

                return dicDefaultCellInfo;
            }
        }

        /// <summary>
        /// 編集画面　削除処理
        /// </summary>
        /// <param name="ctrlId">コントロールID</param>
        /// <returns>エラーの場合False</returns>
        private bool executeDelete(string ctrlId)
        {
            // 編集、削除の場合
            if (ctrlId.Equals(ConductInfo.FormList.ButtonCtrlId.Delete))
            {
                // 排他チェック
                if (isErrorExclusive())
                {
                    return false;
                }
            }

            List<string> listUnComment = new List<string>();

            Dao.searchCondition condition = getSearchInfo<Dao.searchCondition>();

            // 削除ボタンの場合のみ
            if (ctrlId.Equals(ConductInfo.FormList.ButtonCtrlId.Delete))
            {
                // 検索条件より削除SQLを実行
                if (!registDeleteDb<Dao.searchCondition>(condition, SqlName.DeleteOutputPattern))
                {
                    // エラーの場合
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return false;
                }
            }

            // 修正ボタンの場合のみ
            if (ctrlId.Equals(ConductInfo.FormList.ButtonCtrlId.Regist))
            {
                // シート番号=1のデータのみの削除となるため、シートNoを条件に追加する
                condition.SheetNo = OutputPatternDefaultValue.SheetNo;
                listUnComment.Add("SheetNo");
            }

            // 検索条件より削除SQLを実行
            if (!registDeleteDb<Dao.searchCondition>(condition, SqlName.DeleteOutputItem, listUnComment))
            {
                // エラーの場合
                this.Status = CommonProcReturn.ProcStatus.Error;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 登録処理　入力チェック
        /// </summary>
        /// <param name="outputPattern">出力パターン</param>
        /// <returns>入力チェックエラーがある場合True</returns>
        private bool isErrorRegist(ReportDao.MsOutputPatternEntity outputPattern, string ctrlId)
        {
            // 一覧画面のコントロールID
            string targetCtrlId = ConductInfo.FormOutPattern.ControlId.PatternName;

            // エラー情報セット用Dictionary
            var errorInfoDictionary = new List<Dictionary<string, object>>();

            // 単一の項目の場合のイメージ
            if (isErrorRegistForSingle(ref errorInfoDictionary, outputPattern, ctrlId))
            {
                // エラー情報を画面に反映
                SetJsonResult(errorInfoDictionary);
                return true;
            }

            return false;

            /// <summary>
            /// 登録処理　入力チェック（単一の項目の場合の）
            /// </summary>
            /// <param name="outputPattern">出力パターン</param>
            /// <returns>入力チェックエラーがある場合True</returns>
            bool isErrorRegistForSingle(ref List<Dictionary<string, object>> errorInfoDictionary, ReportDao.MsOutputPatternEntity outputPattern, string ctrlId)
            {
                bool isError = false;   // 処理全体でエラーの有無を保持

                // エラー情報を画面に設定するためのマッピング情報リスト
                var info = getResultMappingInfo(targetCtrlId);

                // エラーチェックを行う画面の項目を取得　コントロールIDで絞り込み
                // 単一の内容を取得
                var targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, targetCtrlId);

                // Dictionaryをデータクラスに変換
                Dao.searchResult result = new();
                SetDataClassFromDictionary(targetDic, targetCtrlId, result);

                // エラー情報格納クラス
                ErrorInfo errorInfo = new ErrorInfo(targetDic);

                // 新規の場合のみ、出力パターンマスタを登録
                if (ctrlId.Equals(ConductInfo.FormOutPattern.ButtonCtrlId.Regist))
                {
                    // 同一パターン名重複チェック
                    if (!checkContentDuplicate(outputPattern))
                    {
                        isError = true;
                        // エラーの場合
                        // エラーメッセージとエラーを設定する画面項目を取得してセット
                        // 「指定された出力パターン名がすでに登録されています。」
                        string errMsg = GetResMessage(new string[] { ComRes.ID.ID141120003, ComRes.ID.ID111120088 }); // エラーメッセージはリソースに定義されたもののみが利用可能です
                        string val = info.getValName("OutputPatternName"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                        errorInfo.setError(errMsg, val); // エラー情報をセット
                        errorInfoDictionary.Add(errorInfo.Result); // エラー情報を追加
                    }
                }

                return isError;
            }

            /// <summary>
            /// 同一パターン名重複チェック
            /// </summary>
            /// <param name="outputPattern">出力パターン</param>
            /// <returns>エラーの場合false</returns>
            bool checkContentDuplicate(ReportDao.MsOutputPatternEntity outputPattern)
            {
                // 検索SQL文の取得
                dynamic whereParam = null; // WHERE句パラメータ
                string sql = string.Empty;
                if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetOutputPatternCountCheck, out sql))
                {
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return false;
                }
                whereParam = new { FactoryId = outputPattern.FactoryId, ReportId = outputPattern.ReportId, TemplateId = outputPattern.TemplateId, OutputPatternName = outputPattern.OutputPatternName };
                // 総件数を取得
                int cnt = db.GetCount(sql, whereParam);
                if (cnt > 0)
                {
                    return false;
                }

                return true;
            }

        }
        /// <summary>
        /// 排他チェック
        /// </summary>
        /// <returns>エラーの場合True</returns>
        private bool isErrorExclusive()
        {

            // 排他チェックに必要な項目が複数のコントロールにまたがって定義されていることは無いと思われるので、コントロールIDで指定

            // 排他ロック用マッピング情報取得
            var lockValMaps = GetLockValMaps(ConductInfo.FormList.ControlId.List);
            var lockKeyMaps = GetLockKeyMaps(ConductInfo.FormList.ControlId.List);

            // 明細(複数)の場合の排他チェック
            var list = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ConductInfo.FormList.ControlId.List, true);
            // 排他チェック
            foreach (var dic in list)
            {
                // delete-insertの為、既存データ１件の更新日時で排他チェックを実施する
                int outputItemCd = int.Parse(dic["VAL8"].ToString());
                if (outputItemCd > 0)
                {
                    if (!CheckExclusiveStatus(dic, lockValMaps, lockKeyMaps))
                    {
                        // 排他エラー
                        return true;
                    }
                    break;
                }
            }

            // 排他チェックOK
            return false;
        }
    }
}
