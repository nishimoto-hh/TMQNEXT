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
using STDData = CommonSTDUtil.CommonSTDUtil.CommonSTDUtillDataClass;
using Dao = BusinessLogic_PT0003.BusinessLogicDataClass_PT0003;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using Const = CommonTMQUtil.CommonTMQConstants;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;

namespace BusinessLogic_PT0003
{
    /// <summary>
    /// 予備品管理　棚卸(新規登録)
    /// </summary>
    public partial class BusinessLogic_PT0003 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 新規登録画面　初期表示
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool setInitValue()
        {
            // 一覧画面の検索条件を画面より取得してデータクラスへセット
            var targetDic = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ConductInfo.FormList.Condition.SearchCondition);
            Dao.searchCondition condition = new();
            SetDataClassFromDictionary(targetDic, ConductInfo.FormList.Condition.SearchCondition, condition);

            //初期値
            Dao.registInfo info = new Dao.registInfo();
            info.LanguageId = this.LanguageId;
            //予備品倉庫
            info.StorageLocationId = condition.StorageLocationId;
            info.LocationStructureId = condition.StorageLocationId;

            // 地区、工場を設定
            IList<Dao.registInfo> infoList = new List<Dao.registInfo>();
            infoList.Add(info);
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.registInfo>(ref infoList, new List<StructureType> { StructureType.Location }, this.db, this.LanguageId);
            info.FactoryIdList = Const.CommonFactoryId.ToString() + ',' + info.FactoryId.ToString();     // 工場IDリスト

            //棚と棚枝番を結合する文字列
            info.JoinString = TMQUtil.GetJoinStrOfPartsLocation(info.FactoryId, this.LanguageId, this.db);
            // 新旧区分の初期表示値取得
            var initOldNewDivisionInfo = TMQUtil.SqlExecuteClass.SelectEntity<Dao.registInfo>(SqlName.GetInitOldNewStructureId, SqlName.SubDirInventry, info, this.db);
            if (initOldNewDivisionInfo != null)
            {
                // 新旧区分(0:新品)
                info.OldNewStructureId = initOldNewDivisionInfo.OldNewStructureId;
            }

            // 部門の初期表示値取得
            var initDepartmentInfo = TMQUtil.SqlExecuteClass.SelectEntity<Dao.registInfo>(SqlName.GetInitDepartmentStructureId, SqlName.SubDirInventry, info, this.db);
            if (initDepartmentInfo != null)
            {
                // 部門(工場表示順の最上位)
                info.DepartmentStructureId = initDepartmentInfo.DepartmentStructureId;
                info.DepartmentCd = initDepartmentInfo.DepartmentCd;
            }

            // 勘定科目の初期表示値取得
            var initAccoutInfo = TMQUtil.SqlExecuteClass.SelectEntity<Dao.registInfo>(SqlName.GetInitAccountStructureId, SqlName.SubDirInventry, info, this.db);
            if (initAccoutInfo != null)
            {
                // 勘定科目(B4140:設備貯蔵品)
                info.AccountStructureId = initAccoutInfo.AccountStructureId;
                info.AccountCd = initAccoutInfo.AccountCd;
                info.AccountOldNewDivision = initAccoutInfo.AccountOldNewDivision;
            }

            //丸め処理区分取得
            //構成アイテムを取得するパラメータ設定
            TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();
            //構成グループID
            param.StructureGroupId = (int)Const.MsStructure.GroupId.RoundDivision;
            //連番
            param.Seq = RoundDivision.Seq;

            //丸め処理区分の構成アイテム情報取得
            List<TMQUtil.StructureItemEx.StructureItemExInfo> list = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.db);
            if (list != null)
            {
                // 取得情報から拡張データを取得
                info.RoundDivision = list.Where(x => x.FactoryId == info.FactoryId).Select(x => x.ExData).FirstOrDefault();
            }

            // ページ情報取得
            var partsLocationPageInfo = GetPageInfo(ConductInfo.FormRegist.PartsLocation, this.pageInfoList);
            var pageInfo = GetPageInfo(ConductInfo.FormRegist.Info, this.pageInfoList);
            //値を設定
            SetSearchResultsByDataClass<Dao.registInfo>(partsLocationPageInfo, infoList, 1);
            SetSearchResultsByDataClass<Dao.registInfo>(pageInfo, infoList, 1);

            return true;
        }

        /// <summary>
        /// 新規登録画面　登録処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool executeNewRegist()
        {
            //システム日時
            DateTime now = DateTime.Now;

            // 一覧画面の検索条件を画面より取得してデータクラスへセット
            var targetDic = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, ConductInfo.FormList.Condition.SearchCondition);
            Dao.searchCondition condition = new();
            SetDataClassFromDictionary(targetDic, ConductInfo.FormList.Condition.SearchCondition, condition);

            //新規登録画面の値を取得してデータクラスへセット
            Dao.registInfo info = getRegistInfoList<Dao.registInfo>(TargetGrpNo.NewRegist, now);

            //予備品仕様マスタを検索
            ComDao.PtPartsEntity parts = TMQUtil.SqlExecuteClass.SelectEntity<ComDao.PtPartsEntity>(SqlName.GetParts, SqlName.SubDir, info, this.db);

            // 入力チェック
            if (isErrorNewRegist(info, parts, condition, now))
            {
                return false;
            }

            ComDao.PtInventoryEntity inventory = new ComDao.PtInventoryEntity();
            //対象年月
            inventory.TargetMonth = condition.TargetYearMonth;
            //予備品ID
            inventory.PartsId = parts.PartsId;
            //棚ID
            inventory.PartsLocationId = info.PartsLocationId ?? -1;
            //棚番
            inventory.PartsLocationDetailNo = info.PartsLocationDetailNo;
            //新旧区分ID
            inventory.OldNewStructureId = info.OldNewStructureId;
            //部門ID
            inventory.DepartmentStructureId = info.DepartmentStructureId;
            //勘定科目ID
            inventory.AccountStructureId = info.AccountStructureId;
            //在庫数
            inventory.StockQuantity = 0;
            //棚卸数
            inventory.InventoryQuantity = info.InventoryQuantity;
            //数量単位ID
            inventory.UnitStructureId = parts.UnitStructureId;
            //金額単位ID
            inventory.CurrencyStructureId = parts.CurrencyStructureId;
            //棚卸準備日時
            inventory.PreparationDatetime = info.InventoryDatetime;
            //棚卸実施日時
            inventory.InventoryDatetime = info.InventoryDatetime;
            //作成区分
            inventory.CreationDivisionStructureId = getCreationDivisionStructureId((int)Const.MsStructure.StructureId.CreationDivision.Input);
            // 更新者ID、登録者IDの変換
            bool chkUpd = int.TryParse(this.UserId, out int updUserId);
            bool chkIns = int.TryParse(this.UserId, out int insUserId);
            // 共通の更新日時などを設定
            setExecuteConditionByDataClassCommon<ComDao.PtInventoryEntity>(ref inventory, now, updUserId, insUserId);
            //登録
            bool returnFlag = TMQUtil.SqlExecuteClass.Regist(SqlName.InsertInventory, SqlName.SubDir, inventory, this.db);
            if (!returnFlag)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 新規登録画面の入力チェック
        /// </summary>
        /// <param name="info">入力値</param>
        /// <param name="parts">予備品仕様マスタの情報</param>
        /// <param name="condition">検索条件</param>
        /// <param name="now">システム日時</param>
        /// <returns>エラーの場合True</returns>
        private bool isErrorNewRegist(Dao.registInfo info, ComDao.PtPartsEntity parts, Dao.searchCondition condition, DateTime now)
        {
            // エラー情報セット用Dictionary
            var errorInfoDictionary = new List<Dictionary<string, object>>();

            //棚枝番は半角英数字かチェック
            if (checkPartsLocationDetailNo())
            {
                //入力エラーがあります。
                this.MsgId = GetResMessage(ComRes.ID.ID941220005);
                return true;
            }

            //予備品Noが予備品仕様マスタに存在するかチェック
            if (parts == null)
            {
                //入力エラーがあります。
                this.MsgId = GetResMessage(ComRes.ID.ID941220005);
                //予備品仕様マスタに存在しません。
                string errMsg = GetResMessage(ComRes.ID.ID141380003);
                setErrorInfo(ConductInfo.FormRegist.Info, KeyName.PartsNo, errMsg);
                return true;
            }

            //棚卸日のチェック
            if (isErrorInventoryDatetime())
            {
                //入力エラーがあります。
                this.MsgId = GetResMessage(ComRes.ID.ID941220005);
                return true;
            }

            //新旧区分と勘定科目の組み合わせチェック
            //新旧区分IDよりコード値を取得
            var oldNewStructureCd = getOldNewExData(info.OldNewStructureId);
            if (oldNewStructureCd != info.AccountOldNewDivision)
            {
                //新旧区分と勘定科目の組み合わせが不正の場合、エラー

                //入力エラーがあります。
                this.MsgId = GetResMessage(ComRes.ID.ID941220005);
                //新旧区分と勘定科目の正しい組み合わせを入力してください。
                string errMsg = GetResMessage(ComRes.ID.ID141120005);
                setErrorInfo(ConductInfo.FormRegist.Info, KeyName.Account, errMsg);
                return true;
            }

            //部門IDが検索条件で指定された部門に含まれているかチェック
            if (condition.DepartmentIdList.IndexOf(info.DepartmentStructureId) < 0)
            {
                //入力エラーがあります。
                this.MsgId = GetResMessage(ComRes.ID.ID941220005);
                //指定された部門が検索条件に含まれていません。
                string errMsg = GetResMessage(ComRes.ID.ID141120006);
                setErrorInfo(ConductInfo.FormRegist.Info, KeyName.Department, errMsg);
                return true;
            }

            //重複チェック
            //予備品ID＋棚ID＋棚枝番＋部門ID＋勘定科目ID＋新旧区分の重複データの件数取得
            if (!TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.GetCountDuplicationData, out string sql))
            {
                return false;
            }
            info.PartsId = parts.PartsId;
            int cnt = db.GetCount(sql, info);
            if(cnt > 0)
            {
                //指定された情報は一覧に存在します。一覧の棚卸数を変更してください。
                this.MsgId = GetResMessage(ComRes.ID.ID141120007);
                return true;
            }

            return false;

            //棚枝番は半角英数字かチェック
            bool checkPartsLocationDetailNo()
            {
                if (!string.IsNullOrEmpty(info.PartsLocationDetailNo))
                {
                    var enc = Encoding.GetEncoding("Shift_JIS");
                    if (enc.GetByteCount(info.PartsLocationDetailNo) != info.PartsLocationDetailNo.Length)
                    {
                        // 半角英数字で入力してください。
                        string errMsg = GetResMessage(ComRes.ID.ID141260002);
                        setErrorInfo(ConductInfo.FormRegist.PartsLocation, KeyName.PartsLocationDetailNo, errMsg);
                        return true;
                    }
                }

                return false;
            }

            //棚卸日のチェック
            bool isErrorInventoryDatetime()
            {
                // システム日付
                DateTime nowDate = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
                //入力された棚卸日時
                DateTime inventoryDatetime = info.InventoryDatetime ?? now;

                if (nowDate.CompareTo(inventoryDatetime) == -1)
                {
                    //未来日付が指定された場合、エラー

                    //未来の日付は入力できません
                    string errMsg = GetResMessage(ComRes.ID.ID141320001);
                    setErrorInfo(ConductInfo.FormRegist.Info, KeyName.InventoryDatetime, errMsg);
                    return true;
                }
                //指定された予備品IDの最大対象年月を取得
                DateTime? maxTargetMonth = TMQUtil.SqlExecuteClass.SelectEntity<DateTime?>(SqlName.GetFixedTargetMonth, SqlName.SubDir, parts, this.db);
                if (maxTargetMonth != null && inventoryDatetime.CompareTo(maxTargetMonth) == -1)
                {
                    //在庫確定前の日付の場合、エラー

                    //在庫確定前の日付は入力できません。
                    string errMsg = GetResMessage(ComRes.ID.ID141110004);
                    setErrorInfo(ConductInfo.FormRegist.Info, KeyName.InventoryDatetime, errMsg);
                    return true;
                }

                if (inventoryDatetime.CompareTo(condition.TargetYearMonth) == -1)
                {
                    //検索条件の対象年月より前の日付の場合、エラー

                    //棚卸確定前の日付は入力できません。
                    string errMsg = GetResMessage(ComRes.ID.ID141160002);
                    setErrorInfo(ConductInfo.FormRegist.Info, KeyName.InventoryDatetime, errMsg);
                    return true;
                }
                return false;
            }

            //新旧区分の拡張データ取得
            string getOldNewExData(int structureId)
            {
                string result = null;

                //構成アイテムを取得するパラメータ設定
                TMQUtil.StructureItemEx.StructureItemExInfo param = new TMQUtil.StructureItemEx.StructureItemExInfo();
                //構成グループID
                param.StructureGroupId = (int)Const.MsStructure.GroupId.OldNewDivition;
                //連番
                param.Seq = OldNewDivition.Seq;

                //新旧区分の構成アイテム情報取得
                List<TMQUtil.StructureItemEx.StructureItemExInfo> list = TMQUtil.StructureItemEx.GetStructureItemExData(param, this.db);
                if (list != null)
                {
                    // 取得情報から拡張データを取得
                    result = list.Where(x => x.StructureId == structureId).Select(x => x.ExData).FirstOrDefault();
                }
                return result;
            }

            //エラー情報設定
            void setErrorInfo(string ctrlId, string keyName, string message)
            {
                // エラー情報を画面に設定するためのマッピング情報リスト
                var mapping = getResultMappingInfo(ctrlId);
                // エラーチェックを行う画面の項目を取得　コントロールIDで絞り込み
                // 単一の内容を取得
                var targetDic = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ctrlId);
                // エラー情報格納クラス
                var errorInfo = new ErrorInfo(targetDic);
                string val = mapping.getValName(keyName); // エラーをセットする項目番号

                errorInfo.setError(message, val); // エラー情報をセット
                errorInfoDictionary.Add(errorInfo.Result); // エラー情報を追加
                // エラー情報を画面に反映
                SetJsonResult(errorInfoDictionary);
            }
        }

        /// <summary>
        /// 画面に登録する内容をデータクラスで取得(単一)
        /// </summary>
        /// <typeparam name="T">データクラスの型</typeparam>
        /// <param name="groupNo">取得するグループ</param>
        /// <param name="now">システム日時</param>
        /// <returns>登録内容のデータクラス</returns>
        private T getRegistInfoList<T>(short groupNo, DateTime now)
            where T : CommonDataBaseClass.CommonTableItem, new()
        {
            T resultInfo = new();
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
            return resultInfo;
        }

    }
}
