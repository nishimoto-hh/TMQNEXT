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
using Dao = BusinessLogic_PT0007.BusinessLogicDataClass_PT0007;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using TMQDao = CommonTMQUtil.CommonTMQUtilDataClass;
using ComDao = CommonTMQUtil.TMQCommonDataClass;

namespace BusinessLogic_PT0007
{
    /// <summary>
    /// 棚番移庫タブ
    /// </summary>
    public partial class BusinessLogic_PT0007 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 棚番移庫タブ初期化
        /// </summary>
        /// <param name="searchCondition">索</param>
        /// <param name="dataCnt">棚別在庫一覧の件数</param>
        /// <returns>エラーの場合False</returns>
        private bool initLocation(Dao.searchCondition searchCondition, out int dataCnt)
        {
            dataCnt = 0;

            // 棚別在庫一覧
            if (!setLocationList(searchCondition, ref dataCnt))
            {
                return false;
            }

            // 移庫先情報
            if (!setLocationInfoTo(searchCondition))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 棚別在庫一覧検索
        /// </summary>
        /// <param name="searchCondition">検索条件</param>
        /// <param name="dataCnt">棚別在庫一覧のデータ件数</param>
        /// <returns>エラーの場合False</returns>
        private bool setLocationList(Dao.searchCondition searchCondition, ref int dataCnt)
        {
            // 修正で部門移庫からの遷移の場合は何もしない
            if (searchCondition.TransFlg == TransFlg.FlgEdit && searchCondition.TransType == TransType.Department)
            {
                return true;
            }

            // 画面遷移フラグを判定
            string sqlName = string.Empty;

            if (searchCondition.TransFlg == TransFlg.FlgNew)
            {
                // 新規の場合
                sqlName = SqlName.Location.GetLocationListNew;
            }
            else
            {
                // 修正の場合
                sqlName = SqlName.Location.GetLocationListEdit;
            }

            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, sqlName, out string outSql);
            TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.CommonWith, out string outWithSql);

            // SQL実行
            IList<Dao.locationList> results = db.GetListByDataClass<Dao.locationList>(outWithSql + outSql, searchCondition);
            if (results == null)
            {
                return false;
            }

            // 棚別在庫一覧のデータ件数
            dataCnt = results.Count;

            // 機能場所階層IDから上位の階層を設定
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.locationList>(ref results, new List<StructureType> { StructureType.SpareLocation }, this.db, this.LanguageId, true); // 予備品場所階層

            // 丸め処理・数量と単位の結合
            results.ToList().ForEach(x => x.JoinStrAndRound(this.LanguageId, this.db));

            // ページ情報取得
            var pageInfo = GetPageInfo(ConductInfo.LocationControlId.LocationInventoryList, this.pageInfoList);

            // 検索結果の設定
            if (!SetSearchResultsByDataClass<Dao.locationList>(pageInfo, results, results.Count))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 移庫先情報検索
        /// </summary>
        /// <param name="searchCondition">検索条件</param>
        /// <returns>エラーの場合False</returns>
        private bool setLocationInfoTo(Dao.searchCondition searchCondition)
        {
            // 新規または部門移庫からの遷移の場合は何もしない
            if (searchCondition.TransFlg == TransFlg.FlgNew || searchCondition.TransType == TransType.Department)
            {
                return true;
            }

            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Location.GetLocationInfoTo, out string outSql);
            TMQUtil.GetFixedSqlStatementWith(SqlName.SubDir, SqlName.CommonWith, out string outWithSql);

            // SQL実行
            IList<Dao.locationInfoTo> results = db.GetListByDataClass<Dao.locationInfoTo>(outWithSql + outSql, searchCondition);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // グループNoよりコントロールIDを取得する
            List<string> ctrlIdList = getResultMappingInfoByGrpNo(ConductInfo.LocationControlId.GroupNo).CtrlIdList;
            IList<Dao.locationInfoTo> result = new List<Dao.locationInfoTo> { results[0] };
            // 機能場所階層IDから上位の階層を設定
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.locationInfoTo>(ref result, new List<StructureType> { StructureType.SpareLocation }, this.db, this.LanguageId, true); // 予備品場所階層

            // 棚と枝番の結合文字列を取得
            result[0].JoinString = TMQUtil.GetJoinStrOfPartsLocation(result[0].PartsFactoryId, this.LanguageId, this.db);
            // 作業Noの設定
            result[0].WorkNo = searchCondition.WorkNo;

            // 丸め処理・数量と単位の結合
            results.ToList().ForEach(x => x.JoinStrAndRound());

            // 一覧に対して繰り返し値を設定する
            foreach (var ctrlId in ctrlIdList)
            {
                // 画面項目に値を設定
                if (!SetFormByDataClass(ctrlId, result))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 登録・取消処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool executeLocation()
        {
            // 予備品情報を取得
            Dao.partsInfo partsInfo = getPartsInfo();

            // 移庫先情報を取得
            Dao.locationInfoTo locationInfo = getLocationInfo();

            // 棚枝番に入力がない場合はnullではなく空文字にする
            locationInfo.PartsLocationDetailNo = ConvertNullToStringEmpty(locationInfo.PartsLocationDetailNo);

            // 登録条件の作成
            TMQUtil.PartsInventory.MoveLocation moveLocation = new(this.db, this.UserId, this.LanguageId);
            TMQDao.PartsInventory.MoveLocation condition = setNewCondition();

            // ボタンコントロールIDを判定
            if (this.CtrlId == ConductInfo.ButtonCtrlId.RegistLocation)
            {
                // 登録処理
                if (!registLocation(partsInfo, locationInfo, moveLocation, condition))
                {
                    return false;
                }
            }
            else
            {
                // 取消処理
                if (!cancelLocation(locationInfo, moveLocation))
                {
                    return false;
                }
            }

            return true;

            TMQDao.PartsInventory.MoveLocation setNewCondition()
            {
                TMQDao.PartsInventory.MoveLocation condition = new();
                condition.LotControlId = locationInfo.LotControlId;                   // ロット管理ID
                condition.InventoryControlId = locationInfo.InventoryControlId;       // 在庫管理ID
                condition.InoutDatetime = locationInfo.RelocationDate;                // 移庫日
                condition.InoutQuantity = locationInfo.TransferCount;                 // 移庫数
                condition.PartsLocationId = locationInfo.PartsLocationId;             // 棚番
                condition.PartsLocationDetailNo = locationInfo.PartsLocationDetailNo; // 枝番

                return condition;
            }
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <param name="partsInfo">予備品情報</param>
        /// <param name="locationInfo">移庫先情報</param>
        /// <param name="moveLocation">SQL実行クラス</param>
        /// <param name="condition">DB登録情報</param>
        /// <returns>エラーの場合False</returns>
        private bool registLocation(Dao.partsInfo partsInfo, Dao.locationInfoTo locationInfo, TMQUtil.PartsInventory.MoveLocation moveLocation, TMQDao.PartsInventory.MoveLocation condition)
        {
            // 棚卸確定日を取得するための条件を作成
            List<Dao.getInventoryDate> inventoryCondition = getInventoryDateCondition();

            // 入力チェック
            if (isErrorRegistCommon(partsInfo, ConductInfo.LocationControlId.LocationTransferInfoToWarehouse, locationInfo.RelocationDate, inventoryCondition, locationInfo.LotControlId))
            {
                return false;
            }

            // タブ内入力チェック
            if (isErrorRegistLocation(locationInfo))
            {
                return false;
            }

            // 新規か修正を判定
            if (partsInfo.TransFlg == TransFlg.FlgNew)
            {
                // 新規
                if (!moveLocation.New(condition, out long workNo))
                {
                    return false;
                }
            }
            else
            {
                // 修正

                // 排他チェック
                if (!checkExclusiveLocation(locationInfo))
                {
                    return false;
                }

                // 移庫先に対する受払が発生しているかチェック
                if (partsInfo.TransFlg == TransFlg.FlgEdit && isAppearInoutData(locationInfo))
                {
                    return false;
                }

                if (!moveLocation.Update(condition, locationInfo.WorkNo, out long newWorkNo))
                {
                    return false;
                }
            }

            return true;

            List<Dao.getInventoryDate> getInventoryDateCondition()
            {
                // 条件リスト
                List<Dao.getInventoryDate> list = new();
                // 移庫元情報
                Dao.getInventoryDate beforeData = new();
                beforeData.OldNewStructureId = locationInfo.OldNewStructureId;   // 新旧区分
                beforeData.DepartmentStructureId = locationInfo.DepartmentId;    // 部門ID
                beforeData.AccountStructureId = locationInfo.AccountStructureId; // 勘定科目ID
                beforeData.PartsLocationId = locationInfo.PartsLocationIdBefore; // 棚ID
                beforeData.PartsLocationDetailNo = locationInfo.DetailNoBefore;  // 枝番
                beforeData.PartsId = partsInfo.PartsId;                          // 予備品ID

                // 移庫先情報
                Dao.getInventoryDate afterData = new();
                afterData.OldNewStructureId = locationInfo.OldNewStructureId;         // 新旧区分
                afterData.DepartmentStructureId = locationInfo.DepartmentId;          // 部門ID
                afterData.AccountStructureId = locationInfo.AccountStructureId;       // 勘定科目ID
                afterData.PartsLocationId = locationInfo.PartsLocationId;             // 棚ID
                afterData.PartsLocationDetailNo = locationInfo.PartsLocationDetailNo; // 枝番
                afterData.PartsId = partsInfo.PartsId;                                // 予備品ID

                list.Add(beforeData);
                list.Add(afterData);

                return list;
            }
        }

        /// <summary>
        /// 取消処理
        /// </summary>
        /// <param name="locationInfo">移庫先情報</param>
        /// <param name="moveLocation">SQL実行クラス</param>
        /// <returns>エラーの場合False</returns>
        private bool cancelLocation(Dao.locationInfoTo locationInfo, TMQUtil.PartsInventory.MoveLocation moveLocation)
        {
            // 排他チェック
            if (!checkExclusiveLocation(locationInfo))
            {
                return false;
            }

            // 移庫先に対する受払が発生している場合はエラー
            if (isAppearInoutData(locationInfo))
            {
                return false;
            }

            // 取消処理
            if (!moveLocation.Cancel(locationInfo.WorkNo))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 移庫先の取得
        /// </summary>
        /// <returns>登録内容のデータクラス</returns>
        private Dao.locationInfoTo getLocationInfo()
        {
            // 登録対象グループの画面項目定義の情報
            var grpMapInfo = getResultMappingInfoByGrpNo(ConductInfo.LocationControlId.GroupNo);

            // 対象グループのコントロールIDの結果情報のみ抽出
            var ctrlIdList = grpMapInfo.CtrlIdList;
            List<Dictionary<string, object>> conditionList = ComUtil.GetDictionaryListByCtrlIdList(this.resultInfoDictionary, ctrlIdList);

            Dao.locationInfoTo locationInfo = new();
            // コントロールIDごとに繰り返し
            foreach (var ctrlId in ctrlIdList)
            {
                // コントロールIDより画面の項目を取得
                Dictionary<string, object> condition = ComUtil.GetDictionaryByCtrlId(conditionList, ctrlId);
                var mapInfo = grpMapInfo.selectByCtrlId(ctrlId);
                // 登録データの設定
                if (!SetExecuteConditionByDataClass(condition, mapInfo.Value, locationInfo, DateTime.Now, this.UserId, this.UserId))
                {
                    // エラーの場合終了
                    return locationInfo;
                }
            }

            locationInfo.UserFactoryId = TMQUtil.GetUserFactoryId(this.UserId, db); // 本務工場
            return locationInfo;
        }

        /// <summary>
        /// 入力チェック
        /// </summary>
        /// <param name="locationInfo">移庫先情報</param>
        /// <returns>エラーの場合True</returns>
        private bool isErrorRegistLocation(Dao.locationInfoTo locationInfo)
        {
            // エラー情報セット用Dictionary
            var errorInfoDictionary = new List<Dictionary<string, object>>();

            // 単一の項目の場合のイメージ
            if (isErrorRegistForSingle(ref errorInfoDictionary))
            {
                // エラー情報を画面に反映
                SetJsonResult(errorInfoDictionary);
                return true;
            }

            return false;

            bool isErrorRegistForSingle(ref List<Dictionary<string, object>> errorInfoDictionary)
            {
                // エラー情報格納クラス
                var targetDicLocation = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ConductInfo.LocationControlId.LocationTransferInfoToWarehouseId);
                var targetDicQuantity = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ConductInfo.LocationControlId.LocationTransferInfoToMoney);
                var targetDetailNo = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, ConductInfo.LocationControlId.LocationTransferInfoToJoinStr);
                ErrorInfo errorInfoLocation = new ErrorInfo(targetDicLocation);
                ErrorInfo errorInfoQuantity = new ErrorInfo(targetDicQuantity);
                ErrorInfo errorInfoDetailNo = new ErrorInfo(targetDetailNo);

                // エラー情報を画面に設定するためのマッピング情報リスト
                var infoLocation = getResultMappingInfo(ConductInfo.LocationControlId.LocationTransferInfoToWarehouseId);
                var infoQuantity = getResultMappingInfo(ConductInfo.LocationControlId.LocationTransferInfoToMoney);
                var infoDetailNo = getResultMappingInfo(ConductInfo.LocationControlId.LocationTransferInfoToJoinStr);

                string errMsg = string.Empty; // エラーメッセージ
                string val = string.Empty;    // エラー項目の項目番号

                // 移庫元と移庫先が同一(移庫元の棚ID = 移庫先の棚ID かつ 移庫元の枝番 = 移庫先の枝番)の場合はエラー
                if (locationInfo.PartsLocationId == locationInfo.PartsLocationIdBefore && locationInfo.PartsLocationDetailNo == locationInfo.DetailNoBefore)
                {
                    errMsg = GetResMessage(new string[] { ComRes.ID.ID141020002 });　// 「移庫先の棚番が変更されていません。」
                    val = infoLocation.getValName("PartsLocationId");
                    errorInfoLocation.setError(errMsg, val); // エラー情報をセット
                    errorInfoDictionary.Add(errorInfoLocation.Result); // エラー情報を追加
                    return true;
                }

                // 移庫数が0以下の場合はエラー
                if (locationInfo.TransferCount <= 0)
                {
                    // 「移庫数は0以下で登録できません。」
                    errMsg = GetResMessage(new string[] { ComRes.ID.ID141060007, ComRes.ID.ID111020027 });
                    val = infoQuantity.getValName("StockQuantity");
                    errorInfoQuantity.setError(errMsg, val); // エラー情報をセット
                    errorInfoDictionary.Add(errorInfoQuantity.Result); // エラー情報を追加
                    return true;
                }

                // 移庫数が10桁より多い場合エラー
                if (locationInfo.TransferCount > 9999999999.99m)
                {
                    errMsg = GetResMessage(new string[] { ComRes.ID.ID941260008, ComRes.ID.ID111020027, ComRes.ID.ID911140003, "10" }); // 「移庫数は整数部10桁以下で入力してください。」
                    val = infoQuantity.getValName("StockQuantity");
                    errorInfoQuantity.setError(errMsg, val); // エラー情報をセット
                    errorInfoDictionary.Add(errorInfoQuantity.Result); // エラー情報を追加
                    return true;
                }

                // 移庫元の在庫数 < 移庫数の場合はエラー
                if (locationInfo.StockQuantityExeptUnit < locationInfo.TransferCount)
                {
                    errMsg = GetResMessage(new string[] { ComRes.ID.ID141020003 });　// 「移庫元の在庫数以下の移庫数を指定してください。」
                    val = infoQuantity.getValName("StockQuantity");
                    errorInfoQuantity.setError(errMsg, val); // エラー情報をセット
                    errorInfoDictionary.Add(errorInfoQuantity.Result); // エラー情報を追加
                    return true;
                }

                // 予備品倉庫と棚の関連がない場合はエラー
                // 棚IDより倉庫IDを取得するSQLを取得
                TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Location.GetParentIdByLocationId, out string outSql);
                // 倉庫IDを取得
                int wareHouseId = db.GetEntityByDataClass<int>(outSql, locationInfo);
                if (wareHouseId == null || locationInfo.WarehouseId != wareHouseId)
                {
                    errMsg = GetResMessage(new string[] { ComRes.ID.ID141380001 }); // 予備品倉庫に紐付く棚を選択してください。
                    val = infoLocation.getValName("PartsLocationId");
                    errorInfoLocation.setError(errMsg, val); // エラー情報をセット
                    errorInfoDictionary.Add(errorInfoLocation.Result); // エラー情報を追加
                    return true;
                }

                // 枝番が半角英数字でない場合はエラー
                if (!string.IsNullOrEmpty(locationInfo.PartsLocationDetailNo))
                {
                    var enc = Encoding.GetEncoding("Shift_JIS");
                    if (enc.GetByteCount(locationInfo.PartsLocationDetailNo) != locationInfo.PartsLocationDetailNo.Length || !ComUtil.IsAlphaNumeric(locationInfo.PartsLocationDetailNo))
                    {
                        errMsg = GetResMessage(new string[] { ComRes.ID.ID141260002 }); // 半角英数字で入力してください。
                        val = infoDetailNo.getValName("DetailNo");
                        errorInfoDetailNo.setError(errMsg, val); // エラー情報をセット
                        errorInfoDictionary.Add(errorInfoDetailNo.Result); // エラー情報を追加
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// 排他チェック
        /// </summary>
        /// <param name="locationInfo">移庫先情報</param>
        /// <returns>エラーの場合False</returns>
        private bool checkExclusiveLocation(Dao.locationInfoTo locationInfo)
        {
            // SQLを取得
            TMQUtil.GetFixedSqlStatement(SqlName.SubDir, SqlName.Location.GetMaxUpdateDateLocation, out string outSql);

            // 処理時の更新シリアルID
            int updateDatetime = db.GetEntity<int>(outSql, locationInfo);

            // 初期表示時の更新シリアルIDと処理時の更新シリアルIDを比較
            if (locationInfo.UpdateSerialid != updateDatetime)
            {
                // 排他エラー
                setExclusiveError();
                return false;
            }

            return true;
        }
    }
}
