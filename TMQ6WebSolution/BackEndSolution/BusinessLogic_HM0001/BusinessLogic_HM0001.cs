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
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Dao = BusinessLogic_HM0001.BusinessLogicDataClass_HM0001;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using TMQConst = CommonTMQUtil.CommonTMQConstants;
using HistoryManagementDao = CommonTMQUtil.CommonTMQUtilDataClass;
using ComDao = CommonTMQUtil.TMQCommonDataClass;

namespace BusinessLogic_HM0001
{
    /// <summary>
    /// 変更管理 機器台帳
    /// </summary>
    public partial class BusinessLogic_HM0001 : CommonBusinessLogicBase
    {
        #region 定数
        /// <summary>
        /// 実行処理区分
        /// </summary>
        private enum executionDiv
        {
            machineNew = 1,     // 機器の新規・複写登録
            machineEdit = 2,    // 機器の修正
            machineDelete = 3,  // 機器の削除
            componentNew = 4,   // 保全項目一覧の追加
            componentEdit = 5,  // 保全項目一覧の項目編集
            componentDelete = 6 // 保全項目一覧の削除
        }

        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = @"HistoryMachine";
            /// <summary>機器台帳(MC0001)のSQL格納先サブディレクトリ名</summary>
            public const string SubDirMachine = @"Machine";

            /// <summary>
            /// 一覧画面SQL
            /// </summary>
            public static class List
            {
                /// <summary>一覧情報取得SQL</summary>
                public const string GetHistoryMachineList = "GetHistoryMachineList";
            }
            /// <summary>
            /// 詳細画面SQL
            /// </summary>
            public static class Detail
            {
                /// <summary>保全項目一覧(変更管理)取得SQL</summary>
                public const string GetHistoryManagementStandardsList = "GetHistoryManagementStandardsList";
                /// <summary>機番IDから見た申請状況の拡張項目を取得する</summary>
                public const string GetApplicationStatusFromMachineId = "GetApplicationStatusFromMachineId";

                #region 機器台帳(MC0001)のSQLを使用
                /// <summary>保全項目一覧取得(機器台帳：MC0001のSQLを使用)</summary>
                public const string GetManagementStandard = "GetManagementStandard";
                /// <summary>機番・機器情報(トランザクション)取得SQL</summary>
                public const string GetMachineDetail = "MachineDetail";
                #endregion
            }
            /// <summary>
            /// 編集画面SQL
            /// </summary>
            public static class Edit
            {
                /// <summary>機番情報登録SQL</summary>
                public const string InsertMachineInfo = "InsertMachineInfo";
                /// <summary>機器情報登録SQL</summary>
                public const string InsertEquipmentInfo = "InsertEquipmentInfo";
            }
        }

        /// <summary>
        /// 機能のコントロール情報
        /// </summary>
        private static class ConductInfo
        {
            /// <summary>
            /// 一覧画面
            /// </summary>
            public static class FormList
            {
                /// <summary>
                /// フォーム番号
                /// </summary>
                public const short FormNo = 0;
                /// <summary>
                /// コントロールID
                /// </summary>
                public static class ControlId
                {
                    /// <summary>
                    /// 検索条件(自分の件名のみ表示)
                    /// </summary>
                    public const string Condition = "BODY_020_00_LST_0";
                    /// <summary>
                    /// 一覧
                    /// </summary>
                    public const string List = "BODY_040_00_LST_0";
                }
                /// <summary>
                /// ボタンコントロールID
                /// </summary>
                public static class ButtonId
                {
                    /// <summary>
                    /// 新規申請
                    /// </summary>
                    public const string New = "New";
                    /// <summary>
                    /// 一括承認
                    /// </summary>
                    public const string ApprovalAll = "ApprovalAll";
                    /// <summary>
                    /// 一括否認
                    /// </summary>
                    public const string DenialAll = "DenialAll";
                }
            }

            /// <summary>
            /// 詳細画面
            /// </summary>
            public static class FormDetail
            {
                /// <summary>
                /// フォーム番号
                /// </summary>
                public const short FormNo = 1;
                /// <summary>
                /// グループ番号(変更管理情報)
                /// </summary>
                public const short GroupNoHistory = 201;
                /// <summary>
                /// グループ番号(機番情報)
                /// </summary>
                public const short GroupNoMachine = 202;
                /// <summary>
                /// グループ番号(機器情報)
                /// </summary>
                public const short GroupNoEquipment = 203;
                /// <summary>
                /// コントロールID
                /// </summary>
                public static class ControlId
                {
                    /// <summary>
                    /// 機番情報(機器番号～保全方式)
                    /// </summary>
                    public const string Machine = "BODY_030_00_LST_1";

                    /// <summary>
                    /// 保全項目一覧
                    /// </summary>
                    public const string ManagementStandardsList = "BODY_080_00_LST_1";
                }
                /// <summary>
                /// ボタンコントロールID
                /// </summary>
                public static class ButtonId
                {
                    /// <summary>
                    /// 複写申請
                    /// </summary>
                    public const string CopyRequest = "CopyRequest";
                    /// <summary>
                    /// 変更申請
                    /// </summary>
                    public const string ChangeRequest = "ChangeRequest";
                    /// <summary>
                    /// 削除申請
                    /// </summary>
                    public const string DeleteRequest = "DeleteRequest";
                    /// <summary>
                    /// 申請内容修正
                    /// </summary>
                    public const string EditRequest = "EditRequest";
                }
            }

            /// <summary>
            /// 詳細編集画面
            /// </summary>
            public static class FormEdit
            {
                /// <summary>
                /// フォーム番号
                /// </summary>
                public const short FormNo = 2;
                /// <summary>
                /// グループ番号(機番情報)
                /// </summary>
                public const short GroupNoMachine = 301;
                /// <summary>
                /// グループ番号(機器情報)
                /// </summary>
                public const short GroupNoEquipment = 302;
                /// <summary>
                /// コントロールID
                /// </summary>
                public static class ControlId
                {
                    /// <summary>
                    /// 機番情報(機器番号～保全方式)
                    /// </summary>
                    public const string Machine = "BODY_010_00_LST_2";


                }

                /// <summary>
                /// ボタンコントロールID
                /// </summary>
                public static class ButtonId
                {
                    /// <summary>
                    /// 登録
                    /// </summary>
                    public const string Regist = "Regist";
                    /// <summary>
                    /// 戻る
                    /// </summary>
                    public const string Back = "Back";
                }
            }
        }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_HM0001() : base()
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

            // 初期検索実行
            return InitSearch();
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int SearchImpl()
        {
            this.ResultList = new();
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId); // IDで判定

            switch (this.FormNo)
            {
                case ConductInfo.FormList.FormNo: // 一覧検索
                    // 初期表示の場合は引数にTrueを設定
                    if (!searchList(compareId.IsInit()))
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;

                case ConductInfo.FormDetail.FormNo: // 詳細画面
                    if (!searchDetailList())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;

                case ConductInfo.FormEdit.FormNo: // 詳細編集画面
                    if (!searchEditList())
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
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId); // IDで判定

            switch (this.CtrlId)
            {
                case ConductInfo.FormList.ButtonId.ApprovalAll:
                case ConductInfo.FormList.ButtonId.DenialAll:
                    return Regist();
                    break;
            }

            if (compareId.IsRegist())
            {
                // 登録の場合
                // 登録処理実行
                return Regist();
            }
            else if (compareId.IsDelete())
            {
                // 削除の場合
                // 削除処理実行
                return Delete();
            }
            else if (compareId.IsStartId("checkIsInProgress"))
            {
                // 変更申請・承認依頼・申請内容修正ボタンクリック時の排他チェック(Ajax通信)
                // 機番IDから見た申請状況が仕掛中(承認依頼中 または 差戻中)かどうか判定する
                return isInProgress();
            }

            // 登録・削除と異なる、機能固有の処理の場合
            string metodName = string.Empty;
            string processName = string.Empty;

            if(this.FormNo == ConductInfo.FormDetail.FormNo && compareId.IsStartId(ConductInfo.FormDetail.ButtonId.DeleteRequest))
            {
                // 詳細画面 削除申請ボタン
                metodName = "updateSchedule";
                processName = ComRes.ID.ID111130006;
            }















            // この部分は到達不能なので、エラーを返す
            return ComConsts.RETURN_RESULT.NG;
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <param name="transaction">トランザクション</param>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int RegistImpl()
        {
            bool resultRegist = false;             // 登録処理戻り値、エラーならFalse
            string processName = setProcessName(); // 処理名称
            //画面番号で分岐
            switch (this.FormNo)
            {
                case ConductInfo.FormList.FormNo: // 一覧画面
                    resultRegist = registFormList();
                    break;

                case ConductInfo.FormDetail.FormNo: // 詳細画面
                    resultRegist = registFormDetail();
                    break;

                case ConductInfo.FormEdit.FormNo: // 詳細編集画面
                    resultRegist = registFormEdit();
                    break;
                default:
                    // この部分は到達不能なので、エラーを返す
                    return ComConsts.RETURN_RESULT.NG;
            }

            // 登録処理結果によりエラー処理を行う
            if (!resultRegist)
            {
                // エラー終了
                this.Status = CommonProcReturn.ProcStatus.Error;
                // 未設定時にエラーメッセージを設定
                if (string.IsNullOrEmpty(this.MsgId))
                {
                    // 「○○処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, processName });
                }
                return ComConsts.RETURN_RESULT.NG;
            }

            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            //「○○処理に成功しました。」
            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220001, processName });

            return ComConsts.RETURN_RESULT.OK;

            string setProcessName()
            {
                if (this.CtrlId == ConductInfo.FormList.ButtonId.ApprovalAll)
                {
                    return ComRes.ID.ID111020051; // 一括承認
                }
                else if (this.CtrlId == ConductInfo.FormList.ButtonId.DenialAll)
                {
                    return ComRes.ID.ID111020052; // 一括否認
                }

                return ComRes.ID.ID911200003;
            }
        }

        /// <summary>
        /// 削除処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int DeleteImpl()
        {
            int a = 1;
            //this.ResultList = new();

            //// 一覧のチェックされた行のレコードを削除するイメージ
            //// 削除SQL取得
            //TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Delete, out string sql);
            //// 削除処理実行
            //// 第三引数は機能独自の入力チェック
            //DeleteSelectedList<Dao.searchResult>(TargetCtrlId.SearchResult1, sql, isErrorDeleteRow);

            //// 行削除エラーチェック処理、削除処理の引数として渡す
            //// エラーの時True
            //bool isErrorDeleteRow(string listCtrlId, List<Dictionary<string, object>> deleteList)
            //{
            //    // 削除対象の行を繰り返しチェック
            //    foreach (var deleteRow in deleteList)
            //    {
            //        Dao.searchResult row = new();
            //        SetDataClassFromDictionary(deleteRow, listCtrlId, row);

            //        if (row.ItemNameTest != null)
            //        {
            //            // エラーメッセージを設定してTrueを返す
            //            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141300004 });
            //            return true;
            //        }
            //    }
            //    // エラーが無い場合Falseを返す
            //    return false;
            //}

            //// 再検索処理
            //if (!searchList())
            //{
            //    return ComConsts.RETURN_RESULT.NG;
            //}

            return ComConsts.RETURN_RESULT.OK;
        }
        #endregion

        #region privateメソッド


        private int isInProgress()
        {
            // 検索条件を取得
            Dao.detailSearchCondition condition = getDetailSearchCondition();

            // 申請状況の拡張項目を取得
            string applicationStatusCode = getApplicationStatusCode(condition, false);

            // 返り値を設定(実際は画面に設定しない)
            Dao.searchResult result = new() { ApplicationStatusCode = applicationStatusCode };
            SetFormByDataClass(ConductInfo.FormDetail.ControlId.Machine, new List<Dao.searchResult>() { result });

            return ComConsts.RETURN_RESULT.OK;
        }

        /// <summary>
        /// 申請状況の拡張項目を取得
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <param name="getByHistoryManagementId">変更管理IDより取得する場合はTrue</param>
        /// <returns>申請状況の拡張項目</returns>
        private string getApplicationStatusCode(Dao.detailSearchCondition condition, bool getByHistoryManagementId)
        {
            if (getByHistoryManagementId)
            {
                TMQUtil.HistoryManagement historyManagement = new(this.db, this.UserId, this.LanguageId, DateTime.Now, TMQConst.MsStructure.StructureId.ApplicationConduct.HM0001);
                return historyManagement.getApplicationStatusByHistoryManagementId(new ComDao.HmHistoryManagementEntity() { HistoryManagementId = condition.HistoryManagementId });
            }
            else
            {
                // SQL取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Detail.GetApplicationStatusFromMachineId, out string appStatusSql);

                // SQL実行
                IList<Dao.searchResult> applicationStatusCode = db.GetListByDataClass<Dao.searchResult>(appStatusSql, condition);
                if (applicationStatusCode == null || applicationStatusCode.Count == 0)
                {
                    // 取得できない場合は空文字を返す
                    return string.Empty;
                }

                return applicationStatusCode[0].ApplicationStatusCode;
            }

            return string.Empty;
        }

        /// <summary>
        /// 検索結果を一覧に設定する
        /// </summary>
        /// <param name="groupNo">一覧のグループ番号</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool setSearchResult(short groupNo, IList<Dao.searchResult> results)
        {
            // 画面定義のグループ番号よりコントロールグループIDを取得
            List<string> ctrlIdList = getResultMappingInfoByGrpNo(groupNo).CtrlIdList;

            // グループ番号内の一覧に対して繰り返し値を設定する
            foreach (var ctrlId in ctrlIdList)
            {
                // 画面項目に値を設定
                if (!SetFormByDataClass(ctrlId, results))
                {
                    // エラーの場合
                    this.Status = CommonProcReturn.ProcStatus.Error;
                    return false;
                }
            }

            return true;
        }
















        /// <summary>
        /// 編集画面　登録処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool executeRegistEdita()
        {
            //// 排他チェック
            //if (isErrorExclusive())
            //{
            //    return false;
            //}

            //// 入力チェック
            //if (isErrorRegist())
            //{
            //    return false;
            //}

            //// 画面情報取得
            //DateTime now = DateTime.Now;
            //// ここでは登録する内容を取得するので、テーブルのデータクラスを指定
            //Dao.searchResult registInfo = getRegistInfo<Dao.searchResult>(TargetGrpNo.GroupTest, now);

            //// 登録
            //if (!registDb(registInfo))
            //{
            //    return false;
            //}

            //// 再検索
            //if (!searchList())
            //{
            //    return false;
            //}
            return true;
        }

        /// <summary>
        /// 排他チェック
        /// </summary>
        /// <returns>エラーの場合True</returns>
        private bool isErrorExclusive()
        {
            //// 排他チェックに必要な項目が複数のコントロールにまたがって定義されていることは無いと思われるので、コントロールIDで指定

            //// 排他ロック用マッピング情報取得
            //var lockValMaps = GetLockValMaps(TargetCtrlId.SearchResult1);
            //var lockKeyMaps = GetLockKeyMaps(TargetCtrlId.SearchResult1);

            //// 単一の場合の排他チェック
            //if (!checkExclusiveSingle(TargetCtrlId.SearchResult1))
            //{
            //    // エラーの場合
            //    return true;
            //}

            //// 明細(複数)の場合の排他チェック
            //var list = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, TargetCtrlId.SearchResult1, true);
            //if (!checkExclusiveList(TargetCtrlId.SearchResult1, list))
            //{
            //    // エラーの場合
            //    return true;
            //}

            // 排他チェックOK
            return false;
        }

        /// <summary>
        /// 画面に登録する内容をデータクラスで取得(単一)
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <param name="groupNo">取得するグループ</param>
        /// <param name="now">システム日時</param>
        /// <returns>登録内容のデータクラス</returns>
        private T getRegistInfo<T>(short groupNo, DateTime now)
            where T : CommonDataBaseClass.CommonTableItem, new()
        {
            // 相称型を使用することで、色々な型を呼出元で宣言することができる
            // 登録対象グループの画面項目定義の情報
            var grpMapInfo = getResultMappingInfoByGrpNo(groupNo);

            // 対象グループのコントロールIDの結果情報のみ抽出
            var ctrlIdList = grpMapInfo.CtrlIdList;
            List<Dictionary<string, object>> conditionList = ComUtil.GetDictionaryListByCtrlIdList(this.resultInfoDictionary, ctrlIdList);

            T resultInfo = new();
            // コントロールIDごとに繰り返し
            foreach (var ctrlId in ctrlIdList)
            {
                // コントロールIDより画面の項目を取得
                Dictionary<string, object> condition = ComUtil.GetDictionaryByCtrlId(conditionList, ctrlId);
                var mapInfo = grpMapInfo.selectByCtrlId(ctrlId);
                // 登録データの設定
                if (!SetExecuteConditionByDataClass(condition, mapInfo.Value, resultInfo, now, this.UserId, this.UserId))
                {
                    // エラーの場合終了
                    return resultInfo;
                }
            }
            return resultInfo;
        }

        private T getSearchCondition<T>(string ctrlId)
            where T : CommonDataBaseClass.CommonTableItem, new()
        {
            // コントロールIDにより画面の項目(一覧)を取得
            Dictionary<string, object> result = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ctrlId);

            // 登録対象の画面項目定義の情報
            var mappingInfo = getResultMappingInfo(ctrlId);

            T searchCondition = new();
            SetExecuteConditionByDataClass<T>(result, ctrlId, searchCondition, DateTime.Now, this.UserId, this.UserId);

            // エラーの場合終了
            return searchCondition;





        }



        /// <summary>
        /// 画面に登録する内容をデータクラスで取得(単一)
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <param name="groupNoList">取得するグループ番号のリスト</param>
        /// <param name="now">システム日時</param>
        /// <returns>登録内容のデータクラス</returns>
        private T getRegistInfo<T>(List<short> groupNoList, DateTime now)
        where T : CommonDataBaseClass.CommonTableItem, new()
        {
            // 相称型を使用することで、色々な型を呼出元で宣言することができる

            T resultInfo = new();

            foreach (short groupNo in groupNoList)
            {
                // 登録対象グループの画面項目定義の情報
                var grpMapInfo = getResultMappingInfoByGrpNo(groupNo);

                // 対象グループのコントロールIDの結果情報のみ抽出
                var ctrlIdList = grpMapInfo.CtrlIdList;
                List<Dictionary<string, object>> conditionList = ComUtil.GetDictionaryListByCtrlIdList(this.resultInfoDictionary, ctrlIdList);

                // コントロールIDごとに繰り返し
                foreach (var ctrlId in ctrlIdList)
                {
                    // コントロールIDより画面の項目を取得
                    Dictionary<string, object> condition = ComUtil.GetDictionaryByCtrlId(conditionList, ctrlId);
                    var mapInfo = grpMapInfo.selectByCtrlId(ctrlId);
                    // 登録データの設定
                    if (!SetExecuteConditionByDataClass(condition, mapInfo.Value, resultInfo, now, this.UserId, this.UserId))
                    {
                        // エラーの場合終了
                        return resultInfo;
                    }
                }
            }
            return resultInfo;
        }






        /// <summary>
        /// 画面に登録する内容をデータクラスで取得(リスト)
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <param name="ctrlId">取得するコントロールID</param>
        /// <param name="now">システム日時</param>
        /// <returns>登録内容のデータクラスのリスト</returns>
        private List<T> getRegistInfoList<T>(string ctrlId, DateTime now)
            where T : CommonDataBaseClass.CommonTableItem, new()
        {
            //// 登録対象の画面項目定義の情報
            //var mappingInfo = getResultMappingInfo(TargetCtrlId.SearchResult1);
            //// コントロールIDにより画面の項目(一覧)を取得
            //List<Dictionary<string, object>> resultList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, TargetCtrlId.SearchResult1);
            //// 戻り値となるデータクラスのリスト
            //List<T> registInfoList = new();
            //// 一覧を繰り返し、データクラスに変換、リストへ追加する
            //foreach (var resultRow in resultList)
            //{
            //    T registInfo = new();
            //    if (!SetExecuteConditionByDataClass<T>(resultRow, TargetCtrlId.SearchResult1, registInfo, now, this.UserId, this.UserId))
            //    {
            //        // エラーの場合終了
            //        return registInfoList;
            //    }
            //    registInfoList.Add(registInfo);
            //}
            //return registInfoList;

            return new List<T>();
        }

        /// <summary>
        /// 登録処理　入力チェック
        /// </summary>
        /// <returns>入力チェックエラーがある場合True</returns>
        private bool isErrorRegist()
        {
            // エラー情報セット用Dictionary
            var errorInfoDictionary = new List<Dictionary<string, object>>();

            // 明細の場合のイメージ
            if (isErrorRegistForList(ref errorInfoDictionary))
            {
                // エラー情報を画面に反映
                SetJsonResult(errorInfoDictionary);
                return true;
            }

            // 単一の項目の場合のイメージ
            if (isErrorRegistForSingle(ref errorInfoDictionary))
            {
                // エラー情報を画面に反映
                SetJsonResult(errorInfoDictionary);
                return true;
            }

            return false;

            // このメソッドの中でしか使用しない処理は、以下のようにローカル関数として記載できます
            // 他のメソッドから不要に参照できなくなります
            bool isErrorRegistForList(ref List<Dictionary<string, object>> errorInfoDictionary)
            {
                //// 一覧の場合の入力チェックのサンプルです。
                //// グループの中に一つの一覧のみが配置されている想定で処理を行っています。

                //bool isError = false;   // 処理全体でエラーの有無を保持

                //// エラー情報を画面に設定するためのマッピング情報リスト
                //var info = getResultMappingInfo(TargetCtrlId.SearchResult1);

                //// エラーチェックを行う画面の項目を取得　コントロールIDで絞り込み
                //// 画面に表示されている(=削除されていない)項目を取得
                //var targetDicList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, TargetCtrlId.SearchResult1);

                //// 一覧の件数分絞り込み
                //foreach (var rowDic in targetDicList)
                //{
                //    // Dictionaryをデータクラスに変換
                //    Dao.searchResult result = new();
                //    SetDataClassFromDictionary(rowDic, TargetCtrlId.SearchResult1, result);
                //    // エラー情報格納クラス
                //    ErrorInfo errorInfo = new ErrorInfo(rowDic);
                //    bool isErrorRow = false; // 行単位でエラーの有無を保持
                //    // 実際の入力チェック(内容はサンプル)
                //    if (string.IsNullOrEmpty(result.ItemNameTest))
                //    {
                //        isErrorRow = true;
                //        // エラーの場合
                //        // エラーメッセージとエラーを設定する画面項目を取得してセット
                //        string errMsg = GetResMessage(new string[] { ComRes.ID.ID941260007, ComRes.ID.ID111090032 }); // エラーメッセージはリソースに定義されたもののみが利用可能です
                //        string val = info.getValName("ItemNameTest"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                //        errorInfo.setError(errMsg, val); // エラー情報をセット
                //    }

                //    if (isErrorRow)
                //    {
                //        // 行でエラーのあった場合、エラー情報を設定する
                //        errorInfoDictionary.Add(errorInfo.Result);
                //        isError = true;
                //    }
                //}
                //return isError;

                return true;
            }

            bool isErrorRegistForSingle(ref List<Dictionary<string, object>> errorInfoDictionary)
            {
                //// 一覧ではない、単一の項目の場合の入力チェックのサンプルです。
                //// 一つのグループの中に複数のコントロールIDの項目が配置されていることを想定しています。
                //// ですが、入力チェックはおそらくコントロールIDごとに行うと思われます。

                //bool isError = false;   // 処理全体でエラーの有無を保持

                //// エラー情報を画面に設定するためのマッピング情報リスト
                //var info = getResultMappingInfo(TargetCtrlId.SearchResult1);

                //// エラーチェックを行う画面の項目を取得　コントロールIDで絞り込み
                //// 単一の内容を取得
                //var targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, TargetCtrlId.SearchResult1);

                //// Dictionaryをデータクラスに変換
                //Dao.searchResult result = new();
                //SetDataClassFromDictionary(targetDic, TargetCtrlId.SearchResult1, result);

                //// エラー情報格納クラス
                //ErrorInfo errorInfo = new ErrorInfo(targetDic);

                //// 実際の入力チェック(内容はサンプル)
                //if (string.IsNullOrEmpty(result.ItemNameTest))
                //{
                //    isError = true;
                //    // エラーの場合
                //    // エラーメッセージとエラーを設定する画面項目を取得してセット
                //    string errMsg = GetResMessage(new string[] { ComRes.ID.ID941260007, ComRes.ID.ID111090032 }); // エラーメッセージはリソースに定義されたもののみが利用可能です
                //    string val = info.getValName("ItemNameTest"); // エラーをセットする項目のID　マッピング情報を定義されたkey_nameで絞り込み取得
                //    errorInfo.setError(errMsg, val); // エラー情報をセット
                //    errorInfoDictionary.Add(errorInfo.Result); // エラー情報を追加
                //}

                //return isError;

                return true;
            }

        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <param name="registInfo">登録データ</param>
        /// <returns>エラーの場合False</returns>
        private bool registDb(Dao.searchResult registInfo)
        {
            //// TODO:registInfoは登録するテーブルの型、データクラス作成後に変更
            //string sqlName;

            //// 画面遷移アクション区分に応じてINSERT/UPDATEを分岐していますが、ボタンによって処理が明らかならば必要ありません。
            //// 同じボタンでINSERT/UPDATEを切り替える場合は、画面にキー値を保持しているかで判定してください。
            //if (this.TransActionDiv == LISTITEM_DEFINE_CONSTANTS.DAT_TRANS_ACTION_DIV.Edit)
            //{
            //    // 修正ボタンの場合
            //    // 更新SQL文の取得
            //    sqlName = SqlName.Update;
            //}
            //else
            //{
            //    // 新規・複写ボタンの場合
            //    // TODO:シーケンスを採番する処理

            //    // 新規登録SQL文の取得
            //    sqlName = SqlName.Insert;
            //}
            //// 登録SQL実行
            //bool result = TMQUtil.SqlExecuteClass.Regist(sqlName, SqlName.SubDir, registInfo, this.db);
            //return result;

            return true;
        }

        #endregion

        #region 見直し予定
        /// <summary>
        /// 出力処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int ReportImpl()
        {
            int result = 0;
            //this.ResultList = new();

            //// 実装の際は、不要な帳票に対する分岐は削除して構いません

            //bool outputExcel = false;
            //bool outputPdf = false;

            //switch (this.CtrlId)
            //{
            //    case TargetCtrlId.Button.ReportExcel:
            //        outputExcel = true;
            //        break;
            //    case TargetCtrlId.Button.ReportPdf:
            //        outputExcel = true;
            //        outputPdf = true;
            //        break;
            //    case TargetCtrlId.Button.ReportCsv:
            //        break;
            //    default:
            //        this.Status = CommonProcReturn.ProcStatus.Error;
            //        // 「コントロールIDが不正です。」
            //        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941060003, ComRes.ID.ID911100001 });

            //        // エラーログ出力
            //        logger.Error(this.MsgId);
            //        return ComConsts.RETURN_RESULT.NG;
            //}

            //// ファイル名
            //string baseFileName = string.Format("{0:yyyyMMddHHmmss}_{1}_{2}", DateTime.Now, this.ConductId, this.CtrlId);

            //// データ検索
            //var resultList = searchListForReport();
            //if (resultList == null || resultList.Count == 0)
            //{
            //    // 警告メッセージで終了
            //    this.Status = CommonProcReturn.ProcStatus.Warning;
            //    // 「該当データがありません。」
            //    this.MsgId = GetResMessage(ComRes.ID.ID941060001);
            //    return result;
            //}

            //string msg = string.Empty;
            //if (outputExcel)
            //{
            //    // Excel出力が必要な場合

            //    // マッピング情報生成
            //    // 以下はA列から順番にカラム名リストに一致するデータを行単位でマッピングする
            //    List<CommonExcelPrtInfo> prtInfoList = CommonExcelUtil.CommonExcelUtil.CreateMappingList(resultList, "Sheet1", 2, "A");

            //    // コマンド情報生成
            //    // セルの結合や罫線を引く等のコマンド実行が必要な場合はここでセットする。不要な場合はnullでOK
            //    List<CommonExcelCmdInfo> cmdInfoList = null;

            //    // Excel出力実行
            //    var excelStream = new MemoryStream();
            //    if (!CommonExcelUtil.CommonExcelUtil.CreateExcelFile(TemplateName.Report, this.UserId, prtInfoList, cmdInfoList, ref excelStream, ref msg))
            //    {
            //        this.Status = CommonProcReturn.ProcStatus.Error;
            //        // 「Excel出力に失敗しました。」
            //        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911040001 });

            //        // エラーログ出力
            //        logger.Error(this.MsgId);
            //        logger.Error(msg);

            //        return ComConsts.RETURN_RESULT.NG;
            //    }

            //    if (outputPdf)
            //    {
            //        // PDF出力の場合

            //        // PDF出力実行
            //        var pdfStream = new MemoryStream();
            //        try
            //        {
            //            if (!CommonExcelUtil.CommonExcelUtil.CreatePdfFile(excelStream, ref pdfStream, ref msg))
            //            {
            //                pdfStream.Close();

            //                this.Status = CommonProcReturn.ProcStatus.Error;
            //                // 「PDF出力に失敗しました。」
            //                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911270004 });

            //                // エラーログ出力
            //                logger.Error(this.MsgId);
            //                logger.Error(msg);

            //                return ComConsts.RETURN_RESULT.NG;
            //            }
            //        }
            //        finally
            //        {
            //            // ExcelファイルのStreamは閉じる
            //            excelStream.Close();
            //        }
            //        this.OutputFileType = "3";  // PDF
            //        this.OutputFileName = baseFileName + ".pdf";
            //        this.OutputStream = pdfStream;
            //    }
            //    else
            //    {
            //        // Excel出力の場合
            //        this.OutputFileType = "1";  // Excel
            //        this.OutputFileName = baseFileName + ".xlsx";
            //        this.OutputStream = excelStream;
            //    }
            //}
            //else
            //{
            //    // CSV出力の場合

            //    // CSV出力実行
            //    Stream csvStream = new MemoryStream();
            //    if (!CommonSTDUtil.CommonSTDUtil.CommonSTDUtil.ExportCsvFile(
            //        resultList, Encoding.GetEncoding("Shift-JIS"), out csvStream, out msg))
            //    {
            //        this.Status = CommonProcReturn.ProcStatus.Error;
            //        // 「CSV出力に失敗しました。」
            //        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911120007 });

            //        // エラーログ出力
            //        logger.Error(this.MsgId);
            //        logger.Error(msg);

            //        return ComConsts.RETURN_RESULT.NG;
            //    }
            //    this.OutputFileType = "2";  // CSV
            //    this.OutputFileName = baseFileName + ".csv";
            //    this.OutputStream = csvStream;
            //}

            //// 正常終了
            //this.Status = CommonProcReturn.ProcStatus.Valid;

            return result;

        }

        /// <summary>
        /// 取込処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int UploadImpl()
        {
            // 実装の際は、不要な帳票に対する分岐は削除して構いません

            int result = 0;
            //string msg = string.Empty;
            //this.ResultList = new();

            //List<string[,]> uploadList = new List<string[,]>();

            //List<Stream> excelList = new List<Stream>();
            //List<Stream> csvList = new List<Stream>();
            //foreach (var file in this.InputStream)
            //{
            //    switch (Path.GetExtension(file.FileName))
            //    {
            //        case ComUtil.FileExtension.Excel:   // Excelファイル
            //            excelList.Add(file.OpenReadStream());
            //            break;
            //        case ComUtil.FileExtension.CSV:    // CSVファイル
            //            csvList.Add(file.OpenReadStream());
            //            break;
            //        default:
            //            this.Status = CommonProcReturn.ProcStatus.Error;
            //            // 「ファイルの種類が不正です。」
            //            this.MsgId = GetResMessage(ComRes.ID.ID941280004);

            //            // エラーログ出力
            //            logger.Error(this.MsgId);
            //            return ComConsts.RETURN_RESULT.NG;
            //    }
            //}

            //if (excelList.Count > 0)
            //{
            //    // Excelファイル読込
            //    if (!CommonExcelUtil.CommonExcelUtil.ReadExcelFiles(excelList, "", "", ref uploadList, ref msg))
            //    {
            //        this.Status = CommonProcReturn.ProcStatus.Error;
            //        // 「Excel取込に失敗しました。」
            //        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911040002 });

            //        // エラーログ出力
            //        logger.Error(this.MsgId);
            //        logger.Error(msg);

            //        return ComConsts.RETURN_RESULT.NG;
            //    }
            //}

            //if (csvList.Count > 0)
            //{
            //    // CSVファイル読込
            //    if (!ComUtil.ImportCsvFiles(
            //        csvList, true, Encoding.GetEncoding(CommonConstants.UPLOAD_INFILE_CHAR_CODE), ref uploadList, ref msg))
            //    {
            //        this.Status = CommonProcReturn.ProcStatus.Error;
            //        // 「CSV取込に失敗しました。」
            //        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911120008 });

            //        // エラーログ出力
            //        logger.Error(this.MsgId);
            //        logger.Error(msg);

            //        return ComConsts.RETURN_RESULT.NG;
            //    }
            //}

            //// ↓↓↓ コントロールIDで取込対象を切り分ける場合 ↓↓↓
            ////switch (this.CtrlId)
            ////{
            ////    case TargetCtrlId.UploadExcel:
            ////        // Excelファイル読込
            ////        if (!CommonExcelUtil.CommonExcelUtil.ReadExcelFiles(excelList, "", "", ref uploadList, ref msg))
            ////        {
            ////            this.Status = CommonProcReturn.ProcStatus.Error;
            ////            // 「Excel取込に失敗しました。」
            ////            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911040002 });

            ////            // エラーログ出力
            ////            logger.Error(this.MsgId);
            ////            logger.Error(msg);

            ////            return -1;
            ////        }
            ////        break;
            ////    case TargetCtrlId.UploadCsv:
            ////        // CSVファイル読込
            ////        if (!ComUtil.ImportCsvFiles(
            ////            csvList, true, Encoding.GetEncoding("Shift-JIS"), ref uploadList, ref msg))
            ////        {
            ////            this.Status = CommonProcReturn.ProcStatus.Error;
            ////            // 「CSV取込に失敗しました。」
            ////            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911120008 });

            ////            // エラーログ出力
            ////            logger.Error(this.MsgId);
            ////            logger.Error(msg);

            ////            return -1;
            ////        }
            ////        break;
            ////    default:
            ////        this.Status = CommonProcReturn.ProcStatus.Error;
            ////        // 「コントロールIDが不正です。」
            ////        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941060003, ComRes.ID.ID911100001 });

            ////        // エラーログ出力
            ////        logger.Error(this.MsgId);
            ////        return -1;
            ////}
            //// ↑↑↑ コントロールIDで取込対象を切り分ける場合 ↑↑↑

            //// ↓↓↓ 表示用データを返却する場合 ↓↓↓
            //// 表示用データを返却する場合、コントロールID指定で変換する
            //var resultList = ConvertToUploadResultDictionary("[コントロールID]", uploadList);
            //// 取込結果の設定
            //SetJsonResult(resultList);
            //// ↑↑↑ 表示用データを返却する場合 ↑↑↑

            //// ↓↓↓ 登録処理を実行する場合 ↓↓↓
            ////// 登録処理を実行する場合、コントロール未指定で変換する
            ////this.resultInfoDictionary = ConvertToUploadResultDictionary("", uploadList);
            ////// トランザクション開始
            ////using (var transaction = this.db.Connection.BeginTransaction())
            ////{
            ////    try
            ////    {
            ////        // 登録処理実行
            ////        result = RegistImpl();

            ////        if (result > 0)
            ////        {
            ////            // コミット
            ////            transaction.Commit();
            ////        }
            ////        else
            ////        {
            ////            // ロールバック
            ////            transaction.Rollback();
            ////        }
            ////    }
            ////    catch (Exception ex)
            ////    {
            ////        if (transaction != null)
            ////        {
            ////            // ロールバック
            ////            transaction.Rollback();
            ////        }
            ////        this.Status = CommonProcReturn.ProcStatus.Error;
            ////        // 「取込処理に失敗しました。」
            ////        this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911200004 });
            ////        this.LogNo = string.Empty;

            ////        logger.Error(this.MsgId, ex);
            ////        return -1;
            ////    }
            ////}
            //// ↑↑↑ 登録処理を実行する場合 ↑↑↑

            //// 正常終了
            //this.Status = CommonProcReturn.ProcStatus.Valid;

            return result;
        }
        #endregion

    }
}