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

/// <summary>
/// 機種別仕様マスタメンテ 仕様項目選択肢一覧
/// </summary>
namespace BusinessLogic_MS0020
{
    /// <summary>
    /// 機種別仕様マスタメンテ 仕様項目選択肢一覧
    /// </summary>
    public partial class BusinessLogic_MS0020 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 仕様項目選択肢一覧 初期表示
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool initSpecItemList()
        {
            // ヘッダの非表示項目に退避した、前画面より引き継いだ仕様項目IDと工場IDを取得
            Dao.SpecItemList.Result searchCond = GetFormDataByCtrlId<Dao.SpecItemList.Result>(FormInfo.SpecItemList.Header, false);
            searchCond.LanguageId = this.LanguageId;
            // 検索(一覧とヘッダを一緒に取得)
            IList<Dao.SpecItemList.Result> results = TMQUtil.SqlExecuteClass.SelectList<Dao.SpecItemList.Result>(Sql.SpecItemList.GetData, Sql.SpecItemList.SubDir, searchCond, this.db);
            // キー情報をセット
            results[0].FactoryId = searchCond.FactoryId;
            results[0].MachineSpecRelationId = searchCond.MachineSpecRelationId;
            results[0].SpecId = searchCond.SpecId;
            // ヘッダ(先頭行の値)を設定
            SetFormByDataClass<Dao.SpecItemList.Result>(FormInfo.SpecItemList.Header, new List<Dao.SpecItemList.Result> { results[0] });
            // 構成IDがNullのメンバがある場合、一覧表示行が存在しない(1件のはず)
            if (results.Any(x => x.StructureId == null))
            {
                return true;
            }
            // 一覧にセット
            SetFormByDataClass<Dao.SpecItemList.Result>(FormInfo.SpecItemList.List, results);

            return true;
        }
    }
}
