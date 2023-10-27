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
using StructureType = CommonTMQUtil.CommonTMQUtil.StructureLayerInfo.StructureType;
using ComDao = CommonTMQUtil.TMQCommonDataClass;
using SpecType = CommonTMQUtil.CommonTMQConstants.MsStructure.StructureId.SpecType;

/// <summary>
/// 機種別仕様マスタメンテ 機種別仕様登録画面
/// </summary>
namespace BusinessLogic_MS0020
{
    /// <summary>
    /// 機種別仕様マスタメンテ 機種別仕様登録画面
    /// </summary>
    public partial class BusinessLogic_MS0020 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 機種別仕様登録画面 初期化処理
        /// </summary>
        /// <param name="isReSearch">再取得の場合True</param>
        /// <returns>エラーの場合False</returns>
        private bool initSpecRegist()
        {
            // 一覧画面の選択行の情報の有無で、新規か修正かを取得
            var targetDic = ComUtil.GetDictionaryByCtrlId(this.searchConditionDictionary, FormInfo.SpecList.List);
            bool isNew = targetDic == null;

            // 前画面より引き継いだ工場IDを取得(起動時に一覧画面の非表示項目から登録画面の仕様の非表示項目に退避)
            Dao.Common.Param searchCond = GetFormDataByCtrlId<Dao.Common.Param>(FormInfo.SpecRegist.SpecInfo, false);
            if (!isNew)
            {
                // 修正の場合、前画面より引き継いだ機種別仕様関連付IDを取得
                // 前画面からの引継ぎ情報
                Dao.SpecList.List param = new();
                SetDataClassFromDictionary(targetDic, FormInfo.SpecList.List, param);
                searchCond.MachineSpecRelationId = param.MachineSpecRelationId;
            }
            else
            {
                // 新規の場合、IDは取得できないので一致しないよう-1を設定
                searchCond.MachineSpecRelationId = -1;
            }

            // 言語一覧の検索
            IList<Dao.Common.Translation> results = TMQUtil.SqlExecuteClass.SelectList<Dao.Common.Translation>(Sql.SpecRegist.GetSpecTranslationList, Sql.SpecRegist.SubDir, searchCond, this.db);

            // ページ情報取得
            var pageInfo = GetPageInfo(FormInfo.SpecRegist.TranslateList, this.pageInfoList);
            // 検索結果の設定
            if (SetSearchResultsByDataClass<Dao.Common.Translation>(pageInfo, results, results.Count))
            {
                // 正常終了
                this.Status = CommonProcReturn.ProcStatus.Valid;
            }

            if (isNew)
            {
                // 新規の場合、工場IDのみを設定
                // 検索結果の設定
                SetFormByDataClass<Dao.Common.Param>(FormInfo.SpecRegist.SpecInfo, new List<Dao.Common.Param> { searchCond });
                return true;
            }

            // 修正の場合、仕様情報の表示
            // 情報取得
            IList<Dao.SpecRegist.SpecInfo> specInfos = TMQUtil.SqlExecuteClass.SelectList<Dao.SpecRegist.SpecInfo>(Sql.SpecRegist.GetSpecInfos, Sql.SpecRegist.SubDir, searchCond, this.db);

            // 機種別仕様関連付けマスタ(排他チェック用)
            SetFormByDataClass<Dao.SpecRegist.SpecInfo>(FormInfo.SpecRegist.RelationInfos, specInfos);

            // 職種機種階層IDから上位の階層を設定
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.SpecRegist.SpecInfo>(ref specInfos, new List<StructureType> { StructureType.Job }, this.db, this.LanguageId, true);
            // 検索結果の設定
            SetFormByDataClass<Dao.SpecRegist.SpecInfo>(FormInfo.SpecRegist.Header, specInfos);
            // 退避した工場IDを再設定
            specInfos[0].FactoryId = searchCond.FactoryId;
            // 機種別仕様関連付ID
            specInfos[0].MachineSpecRelationId = searchCond.MachineSpecRelationId;
            // 検索結果の設定
            SetFormByDataClass<Dao.SpecRegist.SpecInfo>(FormInfo.SpecRegist.SpecInfo, new List<Dao.SpecRegist.SpecInfo> { specInfos[0] });

            return true;
        }

        /// <summary>
        /// 機種別仕様登録画面 登録処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool registSpecRegist()
        {
            // この処理内で共通で使用する項目
            // 非表示のキー情報
            Dao.Common.Param keyInfo = GetFormDataByCtrlId<Dao.Common.Param>(FormInfo.SpecRegist.SpecInfo, true);
            // 新規か更新かを判定
            bool isNew = !(keyInfo.MachineSpecRelationId > 0);

            // 更新の場合、排他チェック
            if (!isNew && isErrorExclusive())
            {
                return false;
            }

            // 入力チェック
            if (isErrorTranslationList(true, keyInfo, out List<Dictionary<string, object>> errorInfoDictionary))
            {
                // エラー情報を画面に反映
                SetJsonResult(errorInfoDictionary);
                return false;
            }
            // 登録
            if (!executeRegist(out List<int> relationIds))
            {
                return false;
            }

            return true;

            // 排他チェック
            bool isErrorExclusive()
            {
                // 翻訳一覧
                bool isError = isErrorExclusiveList(FormInfo.SpecRegist.TranslateList, true);
                if (isError)
                {
                    return true;
                }
                // 仕様項目マスタ
                isError = !checkExclusiveSingle(FormInfo.SpecRegist.SpecInfo);
                if (isError)
                {
                    return true;
                }
                // 機種別仕様関連付けマスタ(非表示)
                isError = isErrorExclusiveList(FormInfo.SpecRegist.RelationInfos);
                if (isError)
                {
                    return true;
                }

                return false;

            }

            // 登録処理
            bool executeRegist(out List<int> relationIds)
            {
                // この処理内で共通で使用する項目
                DateTime now = DateTime.Now;

                // 翻訳マスタ登録
                registTranslation(FormInfo.SpecRegist.TranslateList, isNew, keyInfo.FactoryId, now, out int translationId);
                // 仕様項目マスタ
                registSpec(translationId, out int specId);
                // 機種別仕様関連付けマスタ
                registMachineSpecRelation(specId, out relationIds);

                return true;

                // 仕様項目マスタ登録
                bool registSpec(int translationId, out int specId)
                {
                    // 仕様項目マスタのID
                    specId = -1;
                    // 画面の情報を取得
                    var dicSpecInfo = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, FormInfo.SpecRegist.SpecInfo);
                    ComDao.MsSpecEntity regist = new();
                    SetExecuteConditionByDataClass(dicSpecInfo, FormInfo.SpecRegist.SpecInfo, regist, now, this.UserId, this.UserId);
                    regist.TranslationId = translationId;
                    if (isNotNumber(dicSpecInfo))
                    {
                        // 数値でないとき、以下はNULLで登録
                        regist.SpecUnitTypeId = null;
                        regist.SpecUnitId = null;
                        regist.SpecNumDecimalPlaces = null;
                    }
                    else
                    {
                        // 数値の場合、数値書式コンボの値から拡張項目の値を取得して、DB登録内容に設定
                        var structure = new ComDao.MsStructureEntity().GetEntity(regist.SpecNumDecimalPlaces ?? -1, this.db);
                        int itemId = structure.StructureItemId ?? -1;
                        var itemEx = new ComDao.MsItemExtensionEntity().GetEntity(itemId, 1, this.db);
                        regist.SpecNumDecimalPlaces = int.Parse(itemEx.ExtensionData);
                    }
                    if (!isNew)
                    {
                        // 更新の場合、前画面より引き継いだ機種別仕様関連付けマスタのキー値より、仕様項目マスタのキー値を取得
                        ComDao.MsMachineSpecRelationEntity relation = new();
                        relation = relation.GetEntity(keyInfo.MachineSpecRelationId ?? -1, this.db);
                        regist.SpecId = relation.SpecId ?? -1;
                        TMQUtil.SqlExecuteClass.Regist(Sql.SpecRegist.UpdateMsSpec, Sql.SpecRegist.SubDir, regist, this.db);
                        // 機種別仕様関連付けマスタで使用するので値を設定
                        specId = regist.SpecId;
                    }
                    else
                    {
                        // 新規の場合、採番したキーの値を取得
                        TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<int>(out specId, Sql.SpecRegist.InsertMsSpec, Sql.SpecRegist.SubDir, regist, this.db);
                    }

                    return true;
                    // 仕様項目の値が数値項目かどうか判定
                    bool isNotNumber(Dictionary<string, object> dicSpecInfo)
                    {
                        Dao.SpecRegist.SpecInfo info = new();
                        SetDataClassFromDictionary(dicSpecInfo, FormInfo.SpecRegist.SpecInfo, info);
                        List<int> notNumberValues = new List<int> { (int)SpecType.Text, (int)SpecType.Select };
                        bool isNotNumber = notNumberValues.Select(x => x.ToString()).Any(x => x == info.InputTypeExtension);
                        return isNotNumber;
                    }
                }

                // 機種別仕様関連付けマスタ登録
                bool registMachineSpecRelation(int specId, out List<int> relationIds)
                {
                    relationIds = new();

                    // 画面の情報を取得(職種)
                    IList<Dao.Common.Job> jobList = convertDicListToClassList<Dao.Common.Job>(this.resultInfoDictionary, FormInfo.SpecRegist.Header);
                    // 階層情報を設定
                    TMQUtil.StructureLayerInfo.setBottomLayerStructureIdToDataClass<Dao.Common.Job>(ref jobList, new List<StructureType> { StructureType.Job });
                    // 画面の情報を取得(仕様情報)
                    var dicSpecInfo = ComUtil.GetDictionaryByCtrlId(this.resultInfoDictionary, FormInfo.SpecRegist.SpecInfo);
                    ComDao.MsMachineSpecRelationEntity regist = new();
                    SetExecuteConditionByDataClass(dicSpecInfo, FormInfo.SpecRegist.SpecInfo, regist, now, this.UserId, this.UserId);
                    // 場所階層IDと仕様ID
                    regist.LocationStructureId = keyInfo.FactoryId;
                    regist.SpecId = specId;

                    if (!isNew)
                    {
                        // 更新の場合、DELETE→INSERT
                        regist.MachineSpecRelationId = keyInfo.MachineSpecRelationId ?? -1;
                        TMQUtil.SqlExecuteClass.Regist(Sql.SpecRegist.DeleteMsMachineSpecRelation, Sql.SpecRegist.SubDir, regist, this.db);
                    }
                    // 職種の分繰り返しINSERT
                    foreach (var jobRow in jobList)
                    {
                        regist.JobStructureId = jobRow.JobStructureId ?? -1;
                        TMQUtil.SqlExecuteClass.RegistAndGetKeyValue<int>(out int relationId, Sql.SpecRegist.InsertMsMachineSpecRelation, Sql.SpecRegist.SubDir, regist, this.db);
                        relationIds.Add(relationId);
                    }

                    return true;
                }
            }
        }
    }
}