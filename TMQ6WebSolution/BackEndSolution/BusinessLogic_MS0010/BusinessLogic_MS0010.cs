using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonExcelUtil;
using CommonSTDUtil;
using CommonSTDUtil.CommonBusinessLogic;
using CommonWebTemplate.CommonDefinitions;
using CommonWebTemplate.Models.Common;
using static CommonTMQUtil.CommonTMQUtil;
using ComConsts = CommonSTDUtil.CommonConstants;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using ComRes = CommonSTDUtil.CommonResources;
using ComUtil = CommonSTDUtil.CommonSTDUtil.CommonSTDUtil;
using Const = CommonTMQUtil.CommonTMQConstants;
using Dao = BusinessLogic_MS0010.BusinessLogicDataClass_MS0010;
using Master = CommonTMQUtil.CommonTMQUtil.ComMaster;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using ExData = CommonTMQUtil.CommonTMQConstants.MsStructure.StructureId;
using Password = CommonWebTemplate.PasswordEncrypt;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;

namespace BusinessLogic_MS0010
{
    /// <summary>
    /// ユーザーマスタメンテナンス
    /// </summary>
    public class BusinessLogic_MS0010 : CommonBusinessLogicBase
    {
        #region 定数
        /// <summary>
        /// TOP画面の機能ID
        /// </summary>
        private static string topConductGrpId = "CM00001";
        /// <summary>
        /// マスタの機能ID
        /// </summary>
        private static string masterConductGrpId = "MS0001";
        /// <summary>
        /// ユーザマスタの機能ID
        /// </summary>
        private static string userConductGrpId = "MS0010";
        /// <summary>
        /// 地区/工場の機能ID
        /// </summary>
        private static string districtFactoryConductGrpId = "MS1000";

        /// <summary>
        /// 処理対象コントロールID
        /// </summary>
        private static class TargetCtrlId
        {
            /// <summary>
            /// ユーザー情報
            /// </summary>
            public const string UserInfo = "BODY_030_00_LST_1";
            /// <summary>
            /// 本務設定コンボボックス
            /// </summary>
            public const string DutyCombo = "BODY_040_00_LST_1";
            /// <summary>
            /// 場所階層選択一覧
            /// </summary>
            public const string Location = "BODY_050_00_LST_1";
            /// <summary>
            /// 職種選択一覧
            /// </summary>
            public const string Job = "BODY_060_00_LST_1";
            /// <summary>
            /// 機能権限設定一覧
            /// </summary>
            public const string ConductAuth = "BODY_070_00_LST_1";
        }

        /// <summary>
        /// SQLファイル名称
        /// </summary>
        private static class SqlName
        {
            /// <summary>SQL名：ユーザ一覧取得</summary>
            public const string GetUserList = "GetUserList";
            /// <summary>SQL名：選択された構成IDに紐づく工場を取得する</summary>
            public const string GetStructureId = "GetStructureId";
            /// <summary>SQL名：ユーザーIDより役割IDを取得する</summary>
            public const string GetRoleIdList = "GetRoleIdList";
            /// <summary>SQL名：ユーザーIDよりパスワードを複合化して取得する</summary>
            public const string GetPassWord = "GetPassWord";
            /// <summary>SQL名：ユーザーIDより本務工場を取得する</summary>
            public const string GetDuty = "GetDuty";
            /// <summary>SQL名：場所階層取得</summary>
            public const string GetLocation = "GetLocation";
            /// <summary>SQL名：機能権限設定一覧取得</summary>
            public const string GetConductAuthList = "GetConductAuthList";
            /// <summary>SQL名：権限レベルより拡張データを取得する</summary>
            public const string GetExtensionData = "GetExtensionData";
            /// <summary>SQL名：構成IDより工場と職種を取得する</summary>
            public const string GetJobTreeValue = "GetJobTreeValue";
            /// <summary>SQL名：構成IDより上位の地区を取得する</summary>
            public const string GetDistrict = "GetDistrict";
            /// <summary>SQL名：構成IDより下位の職種を取得する</summary>
            public const string GetLowerJob = "GetLowerJob";
            /// <summary>SQL名：指定工場に紐づく職種の件数を取得</summary>
            public const string GetJobCount = "GetJobCount";

            /// <summary>SQL名：ログインID重複チェック</summary>
            public const string GetLoginIdCheck = "GetLoginIdCheck";
            /// <summary>SQL名：構成IDより言語IDを取得する</summary>
            public const string GetLanguageId = "GetLanguageId";
            /// <summary>SQL名：機能IDより機能内使用共通IDを取得する</summary>
            public const string GetCommonConductId = "GetCommonConductId";

            /// <summary>SQL名：ユーザーマスタ登録SQL</summary>
            public const string InsertUser = "InsertUser";
            /// <summary>SQL名：ユーザーマスタ更新SQL</summary>
            public const string UpdateUser = "UpdateUser";
            /// <summary>SQL名：ユーザー役割マスタ登録SQL</summary>
            public const string InsertRole = "InsertRole";
            /// <summary>SQL名：ユーザー役割マスタ削除SQL</summary>
            public const string DeleteRole = "DeleteRole";
            /// <summary>SQL名：ユーザー所属マスタ登録SQL</summary>
            public const string InsertBelong = "InsertBelong";
            /// <summary>SQL名：ユーザー所属マスタ削除SQL</summary>
            public const string DeleteBelong = "DeleteBelong";
            /// <summary>SQL名：ユーザー機能権限マスタ登録SQL</summary>
            public const string InsertAuthority = "InsertAuthority";
            /// <summary>SQL名：ユーザー機能権限マスタ削除SQL</summary>
            public const string DeleteAuthority = "DeleteAuthority";
            /// <summary>SQL名：ログインマスタ登録SQL</summary>
            public const string InsertLogin = "InsertLogin";
            /// <summary>SQL名：ログインマスタ削除SQL</summary>
            public const string DeleteLogin = "DeleteLogin";
            /// <summary>SQL名：更新を行うユーザを承認者にしている件数を取得する</summary>
            public const string GetApprovalCntByUserId = "GetApprovalCntByUserId";

            /// <summary>SQL格納先サブディレクトリ名</summary>
            public const string SubDir = @"Master\User";

            #region 項目カスタマイズ関連
            /// <summary>SQL名：項目カスタマイズの初期値が必要な機能を取得</summary>
            public const string GetUserCustomizeExData = "GetUserCustomizeExData";
            /// <summary>SQL名：機能IDより、項目カスタマイズテーブルに登録するデータを取得するSQL</summary>
            public const string GetUserCustomizeData = "GetUserCustomizeData";
            /// <summary>SQL名：項目カスタマイズテーブルデータを登録するSQL</summary>
            public const string InsertCustomizeData = "InsertCustomizeData";
            /// <summary>SQL名：項目カスタマイズテーブルで初期表示する項目を更新するSQL</summary>
            public const string UpdateCustomizeData = "UpdateCustomizeData";
            #endregion
        }

        /// <summary>
        /// 処理対象グループ番号
        /// </summary>
        private static class TargetGrpNo
        {
            /// <summary>ユーザー情報</summary>
            public const short UserInfo = 1;
            /// <summary>本務工場</summary>
            public const short Duty = 2;
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BusinessLogic_MS0010() : base()
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
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId); // IDで判定
            if (compareId.IsBack() || compareId.IsRegist())
            {
                // 戻るボタン、登録ボタン押下時
                return InitSearch();
            }

            switch (this.FormNo)
            {
                case Master.ConductInfo.FormList.FormNo:   // 一覧検索
                    if (!searchList())
                    {
                        return ComConsts.RETURN_RESULT.NG;
                    }
                    break;
                case Master.ConductInfo.FormEdit.FormNo:   // 詳細画面
                    if (!initDetail())
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
        /// 検索処理
        /// </summary>
        /// <returns>実行成否：正常なら0以上、異常なら-1</returns>
        protected override int SearchImpl()
        {
            this.ResultList = new();

            switch (this.FormNo)
            {
                case Master.ConductInfo.FormList.FormNo:     // 一覧
                    // 一覧検索実行
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
            CompareCtrlIdClass compareId = new CompareCtrlIdClass(this.CtrlId); // IDで判定
            if (compareId.IsRegist())
            {
                // 登録の場合
                // 登録処理実行
                return Regist();
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
            bool resultRegist = false;  // 登録処理戻り値、エラーならFalse

            switch (this.FormNo)
            {
                case Master.ConductInfo.FormEdit.FormNo:
                    // 登録・修正画面の登録処理
                    resultRegist = executeRegistEdit();
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
                    // 「登録処理に失敗しました。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220002, ComRes.ID.ID911200003 });
                }
                return ComConsts.RETURN_RESULT.NG;
            }
            // 正常終了
            this.Status = CommonProcReturn.ProcStatus.Valid;
            //「登録処理に成功しました。」
            this.MsgId = GetResMessage(new string[] { ComRes.ID.ID941220001, ComRes.ID.ID911200003 });

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
        #endregion

        #region privateメソッド
        /// <summary>
        /// 登録・修正画面 初期化処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool initDetail()
        {
            bool isEditFlg = false;

            // 検索結果のデータクラス
            Dao.userList resultObj = new Dao.userList();

            // 選択されたユーザー情報取得
            var targetDic = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, Master.ConductInfo.FormList.ControlId.StarndardItemId);
            // ユーザー情報が取得出来ない場合は新規モード
            if (targetDic != null)
            {
                SetDataClassFromDictionary(targetDic, Master.ConductInfo.FormList.ControlId.StarndardItemId, resultObj, new List<string> { });
                isEditFlg = true; // 編集モード
            }

            // 言語ID設定
            resultObj.LanguageId = this.LanguageId;

            // 編集モード
            if (isEditFlg)
            {
                // 役割：ユーザーIDより取得
                var role = TMQUtil.SqlExecuteClass.SelectList<Dao.userList>(SqlName.GetRoleIdList, SqlName.SubDir, resultObj, this.db);
                if (role != null)
                {
                    // 複数選択されていた場合、区切り文字を挟んで文字列に成形
                    foreach (var id in role)
                    {
                        resultObj.RoleId = resultObj.RoleId + id.RoleId + '|';
                    }
                    // 先頭の区切り文字を削除
                    resultObj.RoleId = resultObj.RoleId.TrimEnd('|');
                }

                // ログインパスワード 複合化して表示
                var pass = TMQUtil.SqlExecuteClass.SelectEntity<Dao.userList>(SqlName.GetPassWord, SqlName.SubDir, resultObj, this.db);
                if (pass != null)
                {
                    resultObj.Password = pass.Password;
                }

                // 権限レベルを取得(js側画面制御用)
                var authLevel = TMQUtil.SqlExecuteClass.SelectEntity<Dao.StructureGetInfo>(SqlName.GetExtensionData, SqlName.SubDir, resultObj, this.db);
                resultObj.ExtensionData = authLevel.extensionData; // 拡張項目をセット

                // 検索結果表示リスト
                List<Dao.userList> result = new();

                // 取得した値を追加
                result.Add(resultObj);

                // ページ情報取得
                var pageInfo = GetPageInfo(TargetCtrlId.UserInfo, this.pageInfoList);

                // 検索結果の設定
                if (!SetSearchResultsByDataClass<Dao.userList>(pageInfo, result, result.Count(), true))
                {
                    // エラー
                    return false;
                }

                // 本務工場取得
                setDutyFactory(resultObj);

                // システム管理者以外の場合は検索処理実行
                if (authLevel.extensionData != (int)ExData.AuthLevel.SystemAdministrator)
                {
                    // 所属設定タブ検索
                    getBelong(resultObj);
                }

                // 機能権限設定タブ検索
                getConductDetail(resultObj, false);
            }
            // 新規モード
            else
            {
                // 機能権限設定タブ検索
                getConductDetail(resultObj, true);
            }

            return true;
        }

        /// <summary>
        /// 本務工場設定(コンボ)
        /// </summary>
        /// <param name="resultObj">一覧より取得したユーザー情報詳細</param>
        /// <returns>正常：True</returns>
        private bool setDutyFactory(Dao.userList resultObj)
        {
            // 本務工場取得
            var duty = TMQUtil.SqlExecuteClass.SelectEntity<Dao.userList>(SqlName.GetDuty, SqlName.SubDir, resultObj, this.db);

            // ページ情報取得
            var pageInfo = GetPageInfo(TargetCtrlId.DutyCombo, this.pageInfoList);

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.userList>(pageInfo, new List<Dao.userList> { duty }, 1, true))
            {
                // 正常終了
                return true;
            }
            return false;
        }

        /// <summary>
        /// 所属設定タブ検索
        /// </summary>
        /// <param name="resultObj">一覧より取得したユーザー情報詳細</param>
        /// <returns>正常：True</returns>
        private bool getBelong(Dao.userList resultObj)
        {
            // SQL文の取得
            string selectSql;
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetLocation, out selectSql);

            // ページ情報取得
            var locationPageInfo = GetPageInfo(TargetCtrlId.Location, this.pageInfoList);

            // 構成グループID(場所階層)
            resultObj.StructureGroupId = (int)Const.MsStructure.GroupId.Location;

            // 検索実行
            IList<Dao.locationList> locationList = db.GetListByDataClass<Dao.locationList>(selectSql.ToString(), resultObj);
            if (locationList == null || locationList.Count == 0)
            {
                return false;
            }

            // 機能場所階層IDと職種機種階層IDから上位の階層を設定
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.locationList>(ref locationList, new List<StructureType> { StructureType.Location, StructureType.Job }, this.db, this.LanguageId, true);

            // 検索結果の設定
            if (!SetSearchResultsByDataClass<Dao.locationList>(locationPageInfo, locationList, locationList.Count))
            {
                return false;
            }

            // 検索条件
            Dao.searchCondition conditionObj = new Dao.searchCondition();

            // 工場取得
            conditionObj.locationIdList = returnLocationId();
            List<int> returnLocationId()
            {
                List<int> list = new();
                foreach (var result in locationList)
                {
                    // 上で取得した構成ID(工場)を条件に追加する
                    list.Add(result.LocationStructureId ?? 0);
                }
                return list;
            }

            // 構成グループID(職種)
            resultObj.StructureGroupId = (int)Const.MsStructure.GroupId.Job;

            // 検索実行
            IList<Dao.locationList> jobList = db.GetListByDataClass<Dao.locationList>(selectSql.ToString(), resultObj);
            if (jobList == null || jobList.Count == 0)
            {
                return false;
            }

            // 職種取得
            conditionObj.jobIdList = returnJobId();
            List<int> returnJobId()
            {
                List<int> list = new();
                foreach (var result in jobList)
                {
                    // 上で取得した構成ID(職種)を条件に追加する
                    list.Add(result.LocationStructureId ?? 0);
                }
                return list;
            }

            // SQL文の取得
            selectSql = string.Empty;
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetJobTreeValue, out selectSql);

            // 言語ID
            conditionObj.LanguageId = this.LanguageId;

            // 表示する構成IDのリスト
            IList<Dao.locationList> dispList = new List<Dao.locationList>();

            // 工場に紐づく職種を取得
            IList<Dao.locationList> factoryJobList = db.GetListByDataClass<Dao.locationList>(selectSql.ToString(), conditionObj);
            if (factoryJobList == null || factoryJobList.Count == 0)
            {
                return false;
            }
            else
            {
                // データがある場合は工場に紐づく職種を取得し、全て含まれていたら工場のみを表示する
                dispList = getDispList(factoryJobList, conditionObj);
            }

            // 機能場所階層IDと職種機種階層IDから上位の階層を設定
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.locationList>(ref dispList, new List<StructureType> { StructureType.Location, StructureType.Job }, this.db, this.LanguageId, true);

            // ページ情報取得
            var jobPageInfo = GetPageInfo(TargetCtrlId.Job, this.pageInfoList);
            // 検索結果の設定
            if (!SetSearchResultsByDataClass<Dao.locationList>(jobPageInfo, dispList, dispList.Count))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 取得した職種リストをツリー表示用に整理する
        /// </summary>
        /// <param name="factoryJobList">取得した職種リスト</param>
        /// <param name="conditionObj">検索条件</param>
        /// <returns>表示する職種リスト</returns>
        private IList<Dao.locationList> getDispList(IList<Dao.locationList> factoryJobList, Dao.searchCondition conditionObj)
        {
            // 表示する構成IDのリスト
            IList<Dao.locationList> dispList = new List<Dao.locationList>();

            // 職種追加フラグ
            var jobAddFlg = false;
            // 場所階層追加フラグ
            var locationAddFlg = false;

            foreach (var data in factoryJobList)
            {
                // 場所階層が追加されていたら職種の追加はしない
                if (locationAddFlg && data.LocationStructureId == conditionObj.LocationStructureId)
                {
                    continue;
                }
                // 職種が追加されていたら同じ工場の職種は全て追加
                else if (jobAddFlg && data.LocationStructureId == conditionObj.LocationStructureId)
                {
                    dispList.Add(data);
                    continue;
                }
                else
                {
                    jobAddFlg = false;
                }

                // 場所階層ID取得
                conditionObj.LocationStructureId = data.LocationStructureId ?? 0;

                // 電気・機械・計装の場合
                if (conditionObj.LocationStructureId == 0)
                {
                    dispList.Add(data); // そのままデータ追加
                    continue;
                }

                // 取得したデータから場所階層IDに紐づく職種のレコード件数を取得
                var countList = factoryJobList.Where(x => x.LocationStructureId == conditionObj.LocationStructureId).ToList();

                // 工場に紐づく職種の件数を取得
                var getJobCount = TMQUtil.SqlExecuteClass.SelectEntity<Dao.checkList>(SqlName.GetJobCount, SqlName.SubDir, conditionObj, this.db);
                // ユーザーマスタより取得したデータ件数と登録されている職種の件数が一致すれば工場のみを追加
                if (getJobCount.count == countList.Count())
                {
                    data.JobStructureId = 0;  // 職種を上書き(表示させないため)
                    dispList.Add(data);
                    locationAddFlg = true;
                }
                else
                {
                    dispList.Add(data); // そのままデータ追加
                    jobAddFlg = true;
                }
            }
            return dispList;
        }

        /// <summary>
        /// 機能権限設定タブ検索
        /// </summary>
        /// <param name="resultObj">一覧より取得したユーザー情報詳細</param>
        /// <param name="isNewFlg">新規かどうか</param>
        /// <returns>正常：True</returns>
        private bool getConductDetail(Dao.userList resultObj, bool isNewFlg)
        {
            // 機能権限設定一覧取得
            var results = TMQUtil.SqlExecuteClass.SelectList<Dao.userList>(SqlName.GetConductAuthList, SqlName.SubDir, resultObj, this.db);

            // 編集モードの場合
            if (!isNewFlg)
            {
                // リストの整備
                results = distinctData(resultObj, results);
            }

            // ページ情報取得
            var pageInfo = GetPageInfo(TargetCtrlId.ConductAuth, this.pageInfoList);

            // 検索結果の設定
            if (!SetSearchResultsByDataClass<Dao.userList>(pageInfo, results, results.Count(), true))
            {
                // エラー
                return false;
            }

            return true;
        }

        /// <summary>
        /// 権限ありのデータを優先させてリストに追加する
        /// </summary>
        /// <param name="resultObj">ユーザー情報</param>
        /// <param name="results">一覧検索結果</param>
        /// <returns>リスト情報</returns>
        private List<Dao.userList> distinctData(Dao.userList resultObj, List<Dao.userList> results)
        {
            // リスト初期化
            var resultsList = new List<Dao.userList>();

            bool isDispMaster = false;

            // 権限ありの場合、連続して同一データが取得される
            foreach (var list in results)
            {
                // 権限なし
                if (list.Selection == 0)
                {
                    resultsList.Add(list); 　　　　　　　　　　　//権限無しのデータとして表示するリストに追加
                    continue;
                }
                // 権限あり
                else
                {
                    resultsList.RemoveAt(resultsList.Count - 1); //リストの最後の要素を削除する(権限無しのデータを削除)
                    resultsList.Add(list);                       //権限ありのデータに差し替え
                }
            }
            return resultsList;
        }

        /// <summary>
        /// 検索条件取得
        /// </summary>
        /// <param name="listUnComment">SQLをアンコメントするキーのリスト</param>
        /// <returns>検索条件</returns>
        private Dao.searchCondition getCondition(out List<string> listUnComment)
        {
            listUnComment = new List<string>();

            // 検索条件
            Dao.searchCondition conditionObj = new Dao.searchCondition();
            // 検索条件のディクショナリを取得
            var targetDic = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, Master.ConductInfo.FormList.ControlId.SearchId);

            // 検索条件の取得
            SetDataClassFromDictionary(targetDic, Master.ConductInfo.FormList.ControlId.SearchId, conditionObj, new List<string> { });

            // 言語IDセット
            conditionObj.LanguageId = this.LanguageId;

            // SQLのアンコメントする条件を設定
            // データクラスの中で値がNullでないものをSQLの検索条件に含めるので、メンバ名を取得
            listUnComment = ComUtil.GetNotNullNameByClass<Dao.searchCondition>(conditionObj);

            return conditionObj;
        }

        /// <summary>
        /// 一覧検索処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchList()
        {
            // 検索条件を画面より取得してデータクラスへセット
            Dao.searchCondition conditionObj = getCondition(out List<string> listUnComment);

            // 権限レベルはデフォルトで0が入るため、0であれば条件なし
            if (conditionObj.AuthorityLevelId == 0)
            {
                listUnComment.Remove("AuthorityLevelId");
            }

            // 初期化
            conditionObj.locationIdList = new List<int>();

            // 場所階層ツリーの取得
            List<int> locationIdList = GetLocationTreeValues();
            // IDのリストより紐づくを検索し、階層情報のリストを取得
            var param = new { StructureIdList = locationIdList, LanguageId = this.LanguageId };
            List<Dao.StructureGetInfo> structureInfoList = SqlExecuteClass.SelectList<Dao.StructureGetInfo>(SqlName.GetStructureId, SqlName.SubDir, param, db);
            if (structureInfoList != null)
            {
                foreach (var structureId in structureInfoList)
                {
                    // 条件に追加
                    conditionObj.locationIdList.Add(structureId.StructureId);
                }
            }

            // 検索SQLの取得
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetUserList, out string baseSql, listUnComment))
            {
                return false;
            }
            // WITH句の取得
            if (!TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.GetUserList, out string withSql, listUnComment))
            {
                return false;
            }

            // 総件数取得SQL文の取得
            string execSql = TMQUtil.GetSqlStatementSearch(true, baseSql, null, withSql);

            // ページ情報取得
            var pageInfo = GetPageInfo(Master.ConductInfo.FormList.ControlId.StarndardItemId, this.pageInfoList);

            // 総件数を取得
            int cnt = db.GetCount(execSql, conditionObj);
            // 総件数のチェック
            if (!CheckSearchTotalCount(cnt, pageInfo))
            {
                return false;
            }

            // 一覧検索SQL文の取得
            execSql = TMQUtil.GetSqlStatementSearch(false, baseSql, string.Empty, withSql);

            // 一覧検索実行
            var results = db.GetListByDataClass<Dao.userList>(execSql, conditionObj);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.userList>(pageInfo, results, cnt))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }

            return true;
        }

        /// <summary>
        /// 編集画面　登録処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool executeRegistEdit()
        {
            // 排他チェック
            if (isErrorExclusive())
            {
                return false;
            }

            // 入力チェック
            if (isErrorRegist())
            {
                return false;
            }

            // 登録処理
            if (!registDb())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 排他チェック
        /// </summary>
        /// <returns>エラーの場合True</returns>
        private bool isErrorExclusive()
        {
            // 単一の場合の排他チェック
            if (!checkExclusiveSingle(TargetCtrlId.UserInfo))
            {
                // エラーの場合
                return true;
            }

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
            // 登録対象の画面項目定義の情報
            var mappingInfo = getResultMappingInfo(ctrlId);
            // コントロールIDにより画面の項目(一覧)を取得
            List<Dictionary<string, object>> resultList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, ctrlId);
            // 戻り値となるデータクラスのリスト
            List<T> registInfoList = new();
            // 一覧を繰り返し、データクラスに変換、リストへ追加する
            foreach (var resultRow in resultList)
            {
                T registInfo = new();
                if (!SetExecuteConditionByDataClass<T>(resultRow, ctrlId, registInfo, now, this.UserId, this.UserId))
                {
                    // エラーの場合終了
                    return registInfoList;
                }
                registInfoList.Add(registInfo);
            }
            return registInfoList;
        }

        /// <summary>
        /// 登録処理　入力チェック
        /// </summary>
        /// <returns>入力チェックエラーがある場合True</returns>
        private bool isErrorRegist()
        {
            // ページ情報取得(条件エリア)
            Dao.searchCondition conditionObj = new Dao.searchCondition();

            // ページ情報取得
            var stockInfo = GetPageInfo(TargetCtrlId.UserInfo, this.pageInfoList);

            // 指定されたコントロールIDの結果情報のみ抽出
            Dictionary<string, object> condition = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, TargetCtrlId.UserInfo);

            // 検索条件データの取得
            if (!SetSearchConditionByDataClass(new List<Dictionary<string, object>> { condition }, TargetCtrlId.UserInfo, conditionObj, stockInfo))
            {
                // エラーの場合終了
                return true;
            }

            // エラー情報セット用Dictionary
            var errorInfoDictionary = new List<Dictionary<string, object>>();

            // ログインIDの重複チェック
            if (checkLoginId(getDictionaryKeyValue(condition, "login_id"), ref errorInfoDictionary))
            {
                // エラーの場合終了
                SetJsonResult(errorInfoDictionary);
                return true;
            }

            // 言語ID
            conditionObj.LanguageId = this.LanguageId;

            // 権限レベルより拡張項目を取得する
            var authLevel = TMQUtil.SqlExecuteClass.SelectEntity<Dao.StructureGetInfo>(SqlName.GetExtensionData, SqlName.SubDir, conditionObj, this.db);
            if (authLevel.extensionData != (int)ExData.AuthLevel.SystemAdministrator)
            {
                // 権限がシステム管理者以外で所属が未選択の場合はエラーメッセージを表示し、処理を中止する
                if (selectCheckTree(authLevel.extensionData))
                {
                    return true;
                }
            }

            // 更新するユーザを承認者に設定している工場があるかチェック
            if (checkApprovalUser(authLevel.extensionData, ref errorInfoDictionary))
            {
                // エラーの場合終了
                SetJsonResult(errorInfoDictionary);
                return true;
            }

            // 正常
            return false;
        }

        /// <summary>
        /// ログインID重複チェック
        /// </summary>
        /// <param name="loginId">ログインID</param>
        /// <param name="errorInfoDictionary">エラー情報リスト</param>
        /// <returns>エラーがあればTrue</returns>
        private bool checkLoginId(string loginId, ref List<Dictionary<string, object>> errorInfoDictionary)
        {
            // エラー情報セット用Dictionary
            errorInfoDictionary = new List<Dictionary<string, object>>();

            // エラー情報を画面に設定するためのマッピング情報リスト
            var info = getResultMappingInfo(TargetCtrlId.UserInfo);
            string val3 = info.getValName("login_id");  // エラーセットVAL値

            // 単一の内容を取得
            var targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, TargetCtrlId.UserInfo);

            // エラー情報格納クラス
            var errorInfo = new ErrorInfo(targetDic);
            string errMsg = string.Empty;

            // 検索結果のデータクラス
            Dao.userList resultObj = new Dao.userList();
            resultObj.LoginId = loginId;

            // 重複チェック
            var distinct = TMQUtil.SqlExecuteClass.SelectEntity<Dao.checkList>(SqlName.GetLoginIdCheck, SqlName.SubDir, resultObj, this.db);
            if (distinct == null)
            {
                // 重複無し
                return false;
            }
            // 既にデータに登録されているユーザーIDと画面のユーザーIDが異なればエラー
            else if (distinct.UserId.ToString() != getDictionaryKeyValue(targetDic, "user_id"))
            {
                // 「ログインIDが他のユーザーで既に使用されています。」
                errMsg = GetResMessage(new string[] { ComRes.ID.ID141060008, ComRes.ID.ID911430004 });
                errorInfo.setError(errMsg, val3);           // エラー情報をセット
                errorInfoDictionary.Add(errorInfo.Result);  // エラー情報を追加
                return true;
            }

            // 正常
            return false;
        }

        /// <summary>
        /// 所属選択チェック
        /// </summary>
        /// <param name="extensionData">権限レベルの拡張項目</param>
        /// <returns>エラーがあればTrue</returns>
        private bool selectCheckTree(int extensionData)
        {
            // 所属場所階層設定取得
            List<Dictionary<string, object>> locationList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, TargetCtrlId.Location);

            // 所属職種階層設定取得
            List<Dictionary<string, object>> jobList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, TargetCtrlId.Job);

            // 先頭行に何も値がない場合はエラー
            if (getDictionaryKeyValue(locationList[0], "district_name") == "" || getDictionaryKeyValue(jobList[0], "factory_name") == "")
            {
                // 「所属が設定されていません。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141120008 });
                return true;
            }

            bool inConclude = false;

            // 本務工場取得
            List<Dictionary<string, object>> duty = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, TargetCtrlId.DutyCombo);
            string selectFactory = getDictionaryKeyValue(duty[0], "combo"); // 取得された工場コンボ値取得

            // 検索条件追加
            Dao.userList resultObj = new Dao.userList();
            resultObj.FactoryId = int.Parse(selectFactory);  // 工場ID
            resultObj.LanguageId = this.LanguageId;          // 言語ID

            // 取得した構成ID(工場)より上位の地区を取得する
            var district = TMQUtil.SqlExecuteClass.SelectEntity<Dao.locationList>(SqlName.GetDistrict, SqlName.SubDir, resultObj, this.db);

            foreach (var loc in locationList)
            {
                // 地区に値が入っており、工場に値がない場合は全工場の権限を持っているとみなす
                if (getDictionaryKeyValue(loc, "district_name") != "" && getDictionaryKeyValue(loc, "factory_name") == "")
                {
                    // 上位の地区と画面の選択された地区が等しければスルー
                    if (district.DistrictId == int.Parse(getDictionaryKeyValue(loc, "district_name")))
                    {
                        inConclude = true;
                        break;
                    }
                }
                //  本務工場が所属場所階層で設定した工場に含まれていればスルー
                else if (selectFactory == getDictionaryKeyValue(loc, "factory_name"))
                {
                    inConclude = true;
                    break;
                }
            }

            if (!inConclude)
            {
                // 「本務工場が所属場所階層設定にて選択されていません。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID111300075 });
                return true;
            }

            // 機能権限設定一覧取得
            List<Dictionary<string, object>> authList = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, TargetCtrlId.ConductAuth);

            bool isCheck = false;
            foreach (var list in authList)
            {
                // 1つでもチェックが付いていればOK
                if (list.ContainsKey("SELTAG"))
                {
                    if (list["SELTAG"].ToString() == "1")
                    {
                        isCheck = true;
                        return false;
                    }
                }
            }

            // チェックが1つもされていない場合はエラー
            if (!isCheck)
            {
                // 「機能権限が設定されていません。」
                this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141070005 });
                return true;
            }
            return false;
        }

        /// <summary>
        /// 更新するユーザを承認者に設定している工場があるかチェック
        /// </summary>
        /// <param name="authLevel">権限レベルの拡張項目</param>
        /// <param name="errorInfoDictionary">エラー情報リスト</param>
        /// <returns>エラーがあればTrue</returns>
        private bool checkApprovalUser(int authLevel, ref List<Dictionary<string, object>> errorInfoDictionary)
        {
            // 入力された内容を取得
            Dao.userList registUserInfo = getRegistInfo<Dao.userList>(TargetGrpNo.UserInfo, DateTime.Now);

            // 更新の場合(取得したユーザIDが付与されているかで判定)
            if (registUserInfo.UserId > 0)
            {
                // 選択された権限レベルが「システム管理者」または「ゲスト」の場合
                if (authLevel == (int)ExData.AuthLevel.SystemAdministrator ||
                    authLevel == (int)ExData.AuthLevel.Guest)
                {
                    // 更新するユーザを承認者に設定している工場の件数を取得
                    TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetApprovalCntByUserId, out string sql);
                    int cnt = db.GetCount(sql, new { @UserId = registUserInfo.UserId });

                    // 1件以上あればエラー
                    if (cnt > 0)
                    {
                        var targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, TargetCtrlId.UserInfo);
                        // エラー情報格納クラス
                        ErrorInfo errorInfo = new ErrorInfo(targetDic);
                        var info = getResultMappingInfo(TargetCtrlId.UserInfo);  // エラー情報を画面に設定するためのマッピング情報リスト
                        string errMsg = GetResMessage(ComRes.ID.ID141290009);    // 変更管理の承認者は一般ユーザか特権ユーザを指定してください。
                        string val = info.getValName("auth_level");
                        errorInfo.setError(errMsg, val);
                        errorInfoDictionary.Add(errorInfo.Result);

                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool registDb()
        {
            // 画面情報取得
            DateTime now = DateTime.Now;

            // ユーザーマスタ・ログインマスタ登録
            if (!registUserLogin(now, out int authLebel, out bool isNew, out int userId))
            {
                return false;
            }

            // ユーザー役割マスタ登録
            if (!registRole(now, isNew, userId))
            {
                return false;
            }

            // 所属マスタ登録
            if (!registBelong(now, authLebel, isNew, userId))
            {
                return false;
            }

            // 機能権限マスタ登録
            if (!registAuth(now, authLebel, isNew, userId))
            {
                return false;
            }

            // 画面コントロールユーザーカスタマイズマスタ登録(項目カスタマイズ情報)
            if (!registControlCustomize(isNew, userId))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// ユーザー・ログインマスタ登録
        /// </summary>
        /// <param name="now">現在日時</param>
        /// <param name="authLebel">権限レベルの拡張項目</param>
        /// <param name="isNew">新規フラグ</param>
        /// <param name="userId">ユーザーID</param>
        /// <returns>エラーの場合False</returns>
        private bool registUserLogin(DateTime now, out int authLebel, out bool isNew, out int userId)
        {
            // ここでは登録する内容を取得するので、テーブルのデータクラスを指定
            Dao.userList registUserInfo = getRegistInfo<Dao.userList>(TargetGrpNo.UserInfo, now);

            // ユーザーID(新規時は0)
            userId = registUserInfo.UserId;

            // 言語コンボより言語を取得する
            registUserInfo.LanguageId = this.LanguageId; // ログインユーザーの言語設定
            var language = TMQUtil.SqlExecuteClass.SelectEntity<Dao.userList>(SqlName.GetLanguageId, SqlName.SubDir, registUserInfo, this.db);
            registUserInfo.LanguageId = language.LanguageId;

            // 権限レベルの拡張項目をセット
            authLebel = registUserInfo.ExtensionData;

            // ユーザーIDが0であれば新規
            if (registUserInfo.UserId == 0)
            {
                isNew = true;
                TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<int>(out userId, SqlName.InsertUser, SqlName.SubDir, registUserInfo, db);
                registUserInfo.UserId = userId; // 新規採番したユーザーIDをセット
            }
            else
            {
                isNew = false;

                // ユーザーマスタ修正登録
                TMQUtil.SqlExecuteClass.Regist(SqlName.UpdateUser, SqlName.SubDir, registUserInfo, this.db);
            }

            // ログインパスワードが入力されていたらログインマスタに登録
            if (!string.IsNullOrEmpty(registUserInfo.Password))
            {
                // 修正の場合は削除
                if (!isNew)
                {
                    // 削除SQL実行
                    TMQUtil.SqlExecuteClass.Regist(SqlName.DeleteLogin, SqlName.SubDir, registUserInfo, this.db);
                }
                // パスワードの暗号化
                registUserInfo.Password = Password.GetNewPassWord(registUserInfo.Password);
                // 暗号化キーの取得
                registUserInfo.EncryptKey = Password.GetEncryptKey();

                // 登録SQL実行
                TMQUtil.SqlExecuteClass.Regist(SqlName.InsertLogin, SqlName.SubDir, registUserInfo, this.db);
            }
            return true;
        }

        /// <summary>
        /// 役割マスタ登録
        /// </summary>
        /// <param name="now">現在日時</param>
        /// <param name="isNew">新規フラグ</param>
        /// <param name="userId">ユーザーID</param>
        /// <returns>エラーの場合False</returns>
        private bool registRole(DateTime now, bool isNew, int userId)
        {
            // ここでは登録する内容を取得するので、テーブルのデータクラスを指定
            Dao.userList registUserInfo = getRegistInfo<Dao.userList>(TargetGrpNo.UserInfo, now);
            registUserInfo.UserId = userId; // ユーザーIDセット

            // 修正の場合は削除
            if (!isNew)
            {
                // 削除SQL実行
                TMQUtil.SqlExecuteClass.Regist(SqlName.DeleteRole, SqlName.SubDir, registUserInfo, this.db);
            }

            // 役割が入力されて居なければ終了
            if (registUserInfo.RoleId == null)
            {
                return true;
            }
            // 複数役割が振られている場合はループして登録
            else if (registUserInfo.RoleId.Contains('|'))
            {
                var role = registUserInfo.RoleId.Split('|');
                foreach (string roleId in role)
                {
                    registUserInfo.RoleId = roleId;
                    TMQUtil.SqlExecuteClass.Regist(SqlName.InsertRole, SqlName.SubDir, registUserInfo, this.db);
                }
            }
            else
            {
                // 登録SQL実行
                TMQUtil.SqlExecuteClass.Regist(SqlName.InsertRole, SqlName.SubDir, registUserInfo, this.db);
            }
            return true;
        }

        /// <summary>
        /// 所属マスタ登録
        /// </summary>
        /// <param name="now">現在日時</param>
        /// <param name="authLebel">権限レベルの拡張項目</param>
        /// <param name="isNew">新規フラグ</param>
        /// <param name="userId">ユーザーID</param>
        /// <returns>エラーの場合False</returns>
        private bool registBelong(DateTime now, int authLebel, bool isNew, int userId)
        {
            // ここでは登録する内容を取得するので、テーブルのデータクラスを指定
            Dao.userList registDutyInfo = getRegistInfo<Dao.userList>(TargetGrpNo.Duty, now);
            registDutyInfo.UserId = userId; // ユーザーIDセット
            registDutyInfo.DutyFlg = 1;     // 本務工場フラグセット

            // 修正の場合は削除
            if (!isNew)
            {
                // 削除SQL実行
                TMQUtil.SqlExecuteClass.Regist(SqlName.DeleteBelong, SqlName.SubDir, registDutyInfo, this.db);
            }

            // 登録SQL実行
            TMQUtil.SqlExecuteClass.Regist(SqlName.InsertBelong, SqlName.SubDir, registDutyInfo, this.db);

            // システム管理者でない場合はレコード登録
            if (authLebel != (int)ExData.AuthLevel.SystemAdministrator)
            {
                // 場所階層テーブルのデータクラスを指定
                List<Dao.locationList> registBelongLocationInfo = getRegistInfoList<Dao.locationList>(TargetCtrlId.Location, now);
                registBelongLocationInfo[0].UserId = userId; // ユーザーID設定
                // 登録
                if (!registGetLowerStructureId(registBelongLocationInfo, true))
                {
                    return false;
                }

                // 職種テーブルのデータクラスを指定
                List<Dao.locationList> registBelongJobInfo = getRegistInfoList<Dao.locationList>(TargetCtrlId.Job, now);
                registBelongJobInfo[0].UserId = userId; // ユーザーID設定
                // 登録
                if (!registGetLowerStructureId(registBelongJobInfo, false))
                {
                    return false;
                }

            }

            return true;
        }

        /// <summary>
        /// 最下層の構成IDを取得して所属マスタに登録する処理
        /// </summary>
        /// <param name="registBelongInfo">階層情報</param>
        /// <param name="isLocation">場所階層であればTrue</param>
        /// <returns>エラーの場合False</returns>
        private bool registGetLowerStructureId(List<Dao.locationList> registBelongInfo, bool isLocation)
        {
            // 構成グループIDリスト
            List<int> structureIdList = new List<int>();

            // 職種登録の場合の工場のみが渡ってきているかを見るフラグ
            bool factoryOnlyFlg = false;

            // 検索条件
            Dao.searchCondition condition = new Dao.searchCondition();
            condition.LanguageId = this.LanguageId; // 言語ID

            // 初期化
            condition.locationIdList = new List<int>();
            condition.jobIdList = new List<int>();

            // ループして登録する構成IDを取得
            foreach (var info in registBelongInfo)
            {
                // 職種に値が入っていればその値を取得する
                if (!string.IsNullOrEmpty(info.JobName))
                {
                    structureIdList.Add(int.Parse(info.JobName));
                    condition.jobIdList.Add(int.Parse(info.JobName));
                }
                // 設備に値が入っていればその値を取得する
                else if (!string.IsNullOrEmpty(info.FacilityName))
                {
                    structureIdList.Add(int.Parse(info.FacilityName));
                    condition.locationIdList.Add(int.Parse(info.FacilityName));
                }
                // 工程に値が入っていればその値を取得する
                else if (!string.IsNullOrEmpty(info.StrokeName))
                {
                    structureIdList.Add(int.Parse(info.StrokeName));
                    condition.locationIdList.Add(int.Parse(info.StrokeName));
                }
                // 系列に値が入っていればその値を取得する
                else if (!string.IsNullOrEmpty(info.SeriesName))
                {
                    structureIdList.Add(int.Parse(info.SeriesName));
                    condition.locationIdList.Add(int.Parse(info.SeriesName));
                }
                // プラントに値が入っていればその値を取得する
                else if (!string.IsNullOrEmpty(info.PlantName))
                {
                    structureIdList.Add(int.Parse(info.PlantName));
                    condition.locationIdList.Add(int.Parse(info.PlantName));
                }
                // 工場(場所階層)に値が入っていればその値を取得する
                else if (isLocation && !string.IsNullOrEmpty(info.FactoryName))
                {
                    structureIdList.Add(int.Parse(info.FactoryName));
                    condition.locationIdList.Add(int.Parse(info.FactoryName));
                }
                // 工場(職種)に値が入っていればその値を取得する
                else if (!isLocation && !string.IsNullOrEmpty(info.FactoryName))
                {
                    structureIdList.Add(int.Parse(info.FactoryName));
                    condition.jobIdList.Add(int.Parse(info.FactoryName));
                    factoryOnlyFlg = true;
                }
                // 地区に値が入っていればその値を取得する
                else if (!string.IsNullOrEmpty(info.DistrictName))
                {
                    structureIdList.Add(int.Parse(info.DistrictName));
                    condition.locationIdList.Add(int.Parse(info.DistrictName));
                }
            }

            // 構成IDが取得出来たらそのまま所属マスタに登録する
            if (!factoryOnlyFlg && structureIdList.Count() > 0)
            {
                foreach (var id in structureIdList)
                {
                    registBelongInfo[0].FactoryId = id;
                    // 登録SQL実行
                    TMQUtil.SqlExecuteClass.Regist(SqlName.InsertBelong, SqlName.SubDir, registBelongInfo[0], this.db);
                }
            }
            // 構成IDが取得出来て、所属マスタに登録する際に工場のみの選択がある場合
            else if (factoryOnlyFlg && structureIdList.Count() > 0)
            {
                // 登録処理
                if (!regist(SqlName.GetLowerJob))
                {
                    return false;
                }
            }

            // 下位の構成IDを取得して登録する
            bool regist(string sqlName)
            {
                var results = TMQUtil.SqlExecuteClass.SelectList<Dao.locationList>(sqlName, SqlName.SubDir, condition, this.db);
                if (results == null)
                {
                    // 「所属が設定されていません。」
                    this.MsgId = GetResMessage(new string[] { ComRes.ID.ID141120008 });
                    // 工場に紐づく職種がない場合はエラー
                    return false;
                }

                foreach (var id in results)
                {
                    registBelongInfo[0].FactoryId = id.LocationStructureId;
                    // 登録SQL実行
                    TMQUtil.SqlExecuteClass.Regist(SqlName.InsertBelong, SqlName.SubDir, registBelongInfo[0], this.db);
                }
                return true;
            }

            return true;
        }

        /// <summary>
        /// 機能権限マスタ登録
        /// </summary>
        /// <param name="now">現在日時</param>
        /// <param name="authLebel">権限レベルの拡張項目</param>
        /// <param name="isNew">新規フラグ</param>
        /// <param name="userId">ユーザーID</param>
        /// <returns>エラーの場合False</returns>
        private bool registAuth(DateTime now, int authLebel, bool isNew, int userId)
        {
            // ここでは登録する内容を取得するので、テーブルのデータクラスを指定
            List<Dao.userList> registAuthInfo = getRegistInfoList<Dao.userList>(TargetCtrlId.ConductAuth, now);
            registAuthInfo[0].UserId = userId; // ユーザーIDセット

            // 修正の場合は削除
            if (!isNew)
            {
                // 削除SQL実行
                TMQUtil.SqlExecuteClass.Regist(SqlName.DeleteAuthority, SqlName.SubDir, registAuthInfo[0], this.db);
            }

            // 機能IDリスト
            List<string> conductIdList = new List<string>();

            // 登録する機能権限IDのリストを取得する
            conductIdList = getAddConductId(registAuthInfo[0], authLebel);

            // 追加された分権限マスタに登録
            foreach (var conductId in conductIdList)
            {
                registAuthInfo[0].ConductId = conductId; // 機能権限IDセット

                // 登録SQL実行
                TMQUtil.SqlExecuteClass.Regist(SqlName.InsertAuthority, SqlName.SubDir, registAuthInfo[0], this.db);
            }
            return true;
        }

        /// <summary>
        /// 項目カスタマイズテーブルに初期表示する項目を登録する
        /// </summary>
        /// <param name="isNew">新規登録の場合True</param>
        /// <param name="userId">ユーザーID</param>
        /// <returns>エラーの場合はFalse</returns>
        private bool registControlCustomize(bool isNew, int userId)
        {
            // 新規登録の場合のみ登録する(更新の場合は何もしない)
            if (!isNew)
            {
                return true;
            }

            string sql = string.Empty;
            // 項目カスタマイズの初期設定に必要な値を取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetUserCustomizeExData, out sql);
            IList<Dao.UserCustomizeClass> customizeData = db.GetListByDataClass<Dao.UserCustomizeClass>(sql);

            // 取得した機能分登録する
            foreach (Dao.UserCustomizeClass customize in customizeData)
            {
                // SQLを取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetUserCustomizeData, out sql);
                // テーブルに登録するデータを取得
                IList<Dao.UserCustomizeClass> results = db.GetListByDataClass<Dao.UserCustomizeClass>(sql, new { UserId = userId, ProgramId = customize.ProgramId, ControlGroupId = customize.ControlGroupId, FormNo = customize.FormNo });

                // データを登録
                foreach (Dao.UserCustomizeClass result in results)
                {
                    // 登録SQL実行
                    if (!TMQUtil.SqlExecuteClass.Regist(SqlName.InsertCustomizeData, SqlName.SubDir, result, this.db))
                    {
                        return false;
                    }
                }

                // 初期表示する項目をリストに格納
                string[] strControlNoList = customize.ControlNo.Split(",");
                List<int> controlNoList = new();
                foreach (string controlNo in strControlNoList)
                {
                    controlNoList.Add(int.Parse(controlNo));
                }

                // 初期表示する項目のdisplay_flg = trueに更新する
                if (!TMQUtil.SqlExecuteClass.Regist(SqlName.UpdateCustomizeData, SqlName.SubDir, new { UserId = userId, ProgramId = customize.ProgramId, FormNo = customize.FormNo, ControlGroupId = customize.ControlGroupId, ControlNoList = controlNoList }, this.db))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 登録する機能権限IDのリストを取得する
        /// </summary>
        /// <param name="info">リスト情報</param>
        /// <param name="authLebel">権限レベルの拡張項目</param>
        /// <returns>登録する機能権限IDのリスト</returns>
        private List<string> getAddConductId(Dao.userList info, int authLebel)
        {
            // 機能権限一覧取得
            List<Dictionary<string, object>> registAuthInfo = ComUtil.GetDictionaryListByCtrlId(this.resultInfoDictionary, TargetCtrlId.ConductAuth);

            // 機能IDリスト
            List<string> conductIdList = new List<string>();

            // TOP画面の権限は必ず追加
            conductIdList.Add(topConductGrpId); // CM00001

            // マスタ追加フラグ
            bool addMaster = false;

            foreach (var list in registAuthInfo)
            {
                // 機能ID・機能グループID取得
                var conductId = getDictionaryKeyValue(list, "conduct_id");
                var conductGrpId = getDictionaryKeyValue(list, "conduct_group_id");

                // 機能権限レベルの拡張項目
                switch (authLebel)
                {
                    // システム管理者の場合
                    case (int)ExData.AuthLevel.SystemAdministrator:

                        // 権限リストに追加(全ての権限を付与)
                        conductIdList.Add(conductId);
                        conductIdList.Add(conductGrpId);

                        break;

                    // その他権限
                    default:

                        // システム管理者でない場合はチェックしているもののみを登録
                        if (list.ContainsKey("SELTAG"))
                        {
                            if (list["SELTAG"].ToString() == "0")
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }

                        // 機能IDがマスタの場合は
                        if (conductId.Contains("MS"))
                        {
                            if (authLebel != (int)ExData.AuthLevel.Privilege)
                            {
                                // 特権ユーザでない場合(ゲスト、一般)
                                continue;
                            }
                            // 権限レベルが特権ユーザーの場合

                            // 地区/工場マスタとユーザマスタは登録出来ないようにする(システム管理者のみ閲覧可)
                            if (conductId == districtFactoryConductGrpId || conductId == userConductGrpId)
                            {
                                continue;
                            }
                            // 権限リストに追加
                            conductIdList.Add(conductId);
                            conductIdList.Add(conductGrpId);

                            // マスタが選択されている場合はおおもとのマスタメンテナンス機能を追加
                            if (!addMaster && conductId.Contains("MS"))
                            {
                                conductIdList.Add(masterConductGrpId); // MS0001
                                addMaster = true;                      // 何度も追加しないように制御
                            }
                        }
                        else
                        {
                            // マスタ以外の場合
                            // 権限リストに追加
                            conductIdList.Add(conductId);
                            conductIdList.Add(conductGrpId);
                        }

                        break;
                }

                // 機能IDより機能内使用共通IDを取得し、あればそれも追加
                info.ConductId = conductId; // 機能IDセット
                var commonId = TMQUtil.SqlExecuteClass.SelectEntity<Dao.userList>(SqlName.GetCommonConductId, SqlName.SubDir, info, this.db);
                if (commonId != null)
                {
                    if (commonId.CommonConductId != null)
                    {
                        // 複数の機能IDが設定されている場合
                        if (commonId.CommonConductId.Contains('|'))
                        {
                            var cid = commonId.CommonConductId.Split('|');
                            foreach (string id in cid)
                            {
                                // 分割して機能ID追加
                                conductIdList.Add(id);
                                conductIdList.Add(conductGrpId);
                            }
                        }
                        else
                        {
                            // そのまま機能ID追加
                            conductIdList.Add(commonId.CommonConductId);
                            conductIdList.Add(conductGrpId);
                        }
                    }
                }
            }

            // 重複する機能IDを削除
            var uniqueList = conductIdList.Distinct().ToList();
            return uniqueList;
        }
        #endregion
    }
}