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
using Dao = BusinessLogic_MS0020.BusinessLogicDataClass_MS0020;
using TMQUtil = CommonTMQUtil.CommonTMQUtil;
using Master = CommonTMQUtil.CommonTMQUtil.ComMaster;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using TMQConsts = CommonTMQUtil.CommonTMQConstants;

/// <summary>
/// 機種別仕様マスタメンテ 選択肢登録
/// </summary>
namespace BusinessLogic_MS0020
{
    /// <summary>
    /// 機種別仕様マスタメンテ 選択肢登録
    /// </summary>
    public partial class BusinessLogic_MS0020 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 選択肢登録画面 初期化
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool initSpecItemRegist()
        {
            // 一覧画面の選択行の情報の有無で、新規か修正かを取得
            var targetDic = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, FormInfo.SpecItemList.List);
            var isNew = targetDic == null;

            // 前画面より引き継いだ工場IDを取得(起動時に一覧画面の非表示項目から登録画面の非表示項目に退避)
            Dao.Common.Param searchCond = GetFormDataByCtrlId<Dao.Common.Param>(FormInfo.SpecItemRegist.Info, false);
            if (!isNew)
            {
                // 修正の場合、前画面より引き継いだ構成IDを取得
                // 前画面からの引き継ぎ情報
                Dao.Common.Param param = new();
                SetDataClassFromDictionary(targetDic, FormInfo.SpecItemList.List, param);
                searchCond.StructureId = param.StructureId;
            }
            else
            {
                // 新規の場合、IDは取得できないので一致しないよう-1を設定
                searchCond.StructureId = -1;
            }

            // 言語一覧の検索
            IList<Dao.SpecItemRegist.Result> results = TMQUtil.SqlExecuteClass.SelectList<Dao.SpecItemRegist.Result>(Sql.SpecItemRegist.GetData, Sql.SpecItemRegist.SubDir, searchCond, this.db);
            SetFormByDataClass<Dao.SpecItemRegist.Result>(FormInfo.SpecItemRegist.List, results);

            Dao.SpecItemRegist.Info info = new();
            if (!isNew)
            {
                // 新規の場合、以下は設定不要
                info.DisplayOrder = results[0].DisplayOrder;
                info.DeleteFlg = results[0].DeleteFlg;
                info.StructureUpdId = results[0].StructureUpdId;
                info.ItemId = results[0].ItemId;
                info.ItemUpdId = results[0].ItemUpdId;
            }
            info.FactoryId = searchCond.FactoryId;
            info.MachineSpecRelationId = searchCond.MachineSpecRelationId;
            info.SpecId = searchCond.SpecId;
            info.StructureId = searchCond.StructureId;
            SetFormByDataClass<Dao.SpecItemRegist.Info>(FormInfo.SpecItemRegist.Info, new List<Dao.SpecItemRegist.Info> { info });

            return true;
        }

        /// <summary>
        /// 選択肢登録画面 登録処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool registSpecItemRegist()
        {
            // この処理内で共通で使用する項目
            // 非表示のキー情報
            Dao.Common.Param keyInfo = GetFormDataByCtrlId<Dao.Common.Param>(FormInfo.SpecItemRegist.Info, true);
            // 新規か更新かを判定
            bool isNew = !(keyInfo.StructureId > 0);

            // 更新の場合、排他チェック
            if (!isNew && isErrorExclusive())
            {
                return false;
            }

            // 入力チェック
            if (isErrorTranslationList(false, keyInfo, out List<Dictionary<string, object>> errorInfoDictionary))
            {
                // エラー情報を画面に反映
                SetJsonResult(errorInfoDictionary);
                return false;
            }

            // 登録
            if (!executeRegist())
            {
                return false;
            }

            // 画面再読み込みのためにキー情報を再設定する
            SetFormByDataClass<Dao.Common.Param>(FormInfo.SpecItemRegist.Info, new List<Dao.Common.Param> { keyInfo });

            return true;

            // 排他チェック
            bool isErrorExclusive()
            {
                // 翻訳一覧
                bool isError = isErrorExclusiveList(FormInfo.SpecItemRegist.List, true);
                if (isError)
                {
                    return true;
                }
                // アイテムマスタ
                isError = !checkExclusiveSingle(FormInfo.SpecItemRegist.Info);
                if (isError)
                {
                    return true;
                }
                // 構成マスタ

                return false;
            }

            // 登録処理
            bool executeRegist()
            {
                // この処理内で共通で使用する項目
                DateTime now = DateTime.Now;

                // 翻訳マスタ登録
                registTranslation(FormInfo.SpecItemRegist.List, isNew, keyInfo.FactoryId, now, out int translationId);
                // アイテムマスタ登録
                registItem(translationId, out int itemId);
                // アイテムマスタ拡張登録
                if (isNew)
                {
                    // 新規登録時のみ
                    registItemExtension(itemId);
                }

                // 構成マスタ登録
                registStructure(itemId, out int structureId);

                // 工場別アイテム表示順マスタ登録
                registOrder(structureId);

                return true;

                // アイテムマスタ登録
                bool registItem(int translationId, out int itemId)
                {
                    itemId = -1;
                    // 登録情報
                    ComDao.MsItemEntity regist = new();
                    // 共通部分を設定
                    setCommonValueToCondition(ref regist);
                    // 登録する項目を設定
                    regist.ItemTranslationId = translationId; // 翻訳ID
                    regist.StructureGroupId = (int)TMQConsts.MsStructure.GroupId.SpecSelectItem; // 構成グループID

                    if (!isNew)
                    {
                        // 更新の場合、前画面より引き継いだ構成IDよりアイテムマスタIDを取得
                        ComDao.MsStructureEntity structure = new();
                        structure = structure.GetEntity(keyInfo.StructureId ?? -1, this.db);
                        regist.ItemId = structure.StructureItemId ?? -1;
                        TMQUtil.SqlExecuteClass.Regist(Master.SqlName.UpdateMsItemInfo, Master.SqlName.SubDir, regist, this.db);
                        itemId = regist.ItemId;
                    }
                    else
                    {
                        // 新規の場合、採番したキーの値を取得
                        TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<int>(out itemId, Master.SqlName.InsertMsItemInfo, Master.SqlName.SubDir, regist, this.db);
                    }
                    return true;
                }
                // アイテム拡張マスタ登録
                bool registItemExtension(int itemId)
                {
                    ComDao.MsItemExtensionEntity regist = new();
                    // 共通部分を設定
                    setCommonValueToCondition(ref regist);
                    // アイテムIDは登録した値
                    regist.ItemId = itemId;
                    // 連番は1固定
                    regist.SequenceNo = 1;
                    // 拡張項目の値は引き継いだ仕様項目ID
                    regist.ExtensionData = (keyInfo.SpecId ?? -1).ToString();

                    TMQUtil.SqlExecuteClass.Regist(Master.SqlName.InsertMsItemExtensionInfo, Master.SqlName.SubDir, regist, this.db);
                    return true;
                }
                // 構成マスタ登録
                bool registStructure(int itemId, out int structureId)
                {
                    ComDao.MsStructureEntity regist = new();
                    //画面の情報を設定
                    var dicInfo = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, FormInfo.SpecItemRegist.Info);
                    SetExecuteConditionByDataClass(dicInfo, FormInfo.SpecItemRegist.Info, regist, now, this.UserId, this.UserId, new List<string> { "DeleteFlg" });
                    regist.FactoryId = keyInfo.FactoryId;
                    regist.StructureGroupId = (int)TMQConsts.MsStructure.GroupId.SpecSelectItem; // 構成グループID
                    regist.ParentStructureId = null;
                    regist.StructureLayerNo = null;
                    regist.StructureItemId = itemId;

                    if (!isNew)
                    {
                        // 更新の場合、前画面より引き継いだ構成IDを設定
                        regist.StructureId = keyInfo.StructureId ?? -1;
                        TMQUtil.SqlExecuteClass.Regist(Master.SqlName.UpdateMsStructureInfo, Master.SqlName.SubDir, regist, this.db);
                        structureId = regist.StructureId;
                    }
                    else
                    {
                        // 新規の場合、採番したキーの値を取得
                        TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<int>(out structureId, Master.SqlName.InsertMsStructureInfo, Master.SqlName.SubDir, regist, this.db);
                    }
                    return true;
                }

                // 工場別アイテム表示順マスタ登録
                bool registOrder(int structureId)
                {
                    ComDao.MsStructureOrderEntity regist = new();
                    //画面の情報を設定
                    var dicInfo = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, FormInfo.SpecItemRegist.Info);
                    SetExecuteConditionByDataClass(dicInfo, FormInfo.SpecItemRegist.Info, regist, now, this.UserId, this.UserId, new List<string> { "DisplayOrder" });
                    // その他の項目を設定
                    regist.StructureId = structureId;
                    regist.FactoryId = keyInfo.FactoryId;
                    regist.StructureGroupId = (int)TMQConsts.MsStructure.GroupId.SpecSelectItem; // 構成グループID

                    // 変更の場合は、DELETE→INSERT
                    if (!isNew)
                    {
                        // DELETE
                        new ComDao.MsStructureOrderEntity().DeleteByPrimaryKey(regist.StructureId, regist.FactoryId, this.db);
                    }
                    // INSERT
                    TMQUtil.SqlExecuteClass.Regist(Master.SqlName.InsertMsStructureOrder, Master.SqlName.SubDir, regist, this.db);

                    return true;
                }

                void setCommonValueToCondition<T>(ref T condition)
                     where T : CommonDataBaseClass.CommonTableItem
                {
                    int userId = int.Parse(this.UserId);
                    setExecuteConditionByDataClassCommon<T>(ref condition, now, userId, userId);
                }
            }
        }
    }
}
