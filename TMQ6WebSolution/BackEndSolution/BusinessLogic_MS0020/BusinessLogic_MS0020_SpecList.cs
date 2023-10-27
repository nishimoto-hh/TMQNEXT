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
using TMQConsts = CommonTMQUtil.CommonTMQConstants;
/// <summary>
/// 機種別仕様マスタメンテ 機種別仕様一覧画面
/// </summary>
namespace BusinessLogic_MS0020
{
    /// <summary>
    /// 機種別仕様マスタメンテ 機種別仕様一覧画面
    /// </summary>
    public partial class BusinessLogic_MS0020 : CommonBusinessLogicBase
    {
        /// <summary>
        /// 機種別仕様一覧画面 一覧検索処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool searchSpecList()
        {
            // 検索条件取得
            TMQUtil.SearchConditionForMaster cond = GetFormDataByCtrlId<TMQUtil.SearchConditionForMaster>(FormInfo.SpecList.Condition, false);
            cond.LanguageId = this.LanguageId;
            // 検索
            IList<Dao.SpecList.List> results = TMQUtil.SqlExecuteClass.SelectList<Dao.SpecList.List>(Sql.SpecList.GetList, Sql.SpecList.SubDir, cond, this.db);
            if (results == null || results.Count == 0)
            {
                return false;
            }

            // 機能場所階層IDと職種機種階層IDから上位の階層を設定
            TMQUtil.StructureLayerInfo.SetStructureLayerInfoToDataClass<Dao.SpecList.List>(ref results, new List<StructureType> { StructureType.Job }, this.db, this.LanguageId);
            // 検索結果の設定
            SetFormByDataClass<Dao.SpecList.List>(FormInfo.SpecList.List, results);

            return true;
        }

        /// <summary>
        /// 機種別仕様一覧画面 初期化処理
        /// </summary>
        /// <returns>エラーの場合False</returns>
        private bool initSpecList()
        {
            TMQUtil.HiddenInfoForMaster result = new();

            // ユーザ情報
            var userEntity = new ComDao.MsUserEntity().GetEntity(Convert.ToInt32(this.UserId), this.db);
            // 権限レベル取得
            var authorityLevel = getItemExData((int)TMQConsts.MsStructure.GroupId.Authority, Master.Authority.Seq, userEntity.AuthorityLevelId);
            if (authorityLevel != null)
            {
                // システム管理者か判定する
                result.SystemAdminFlg = Convert.ToInt32(authorityLevel) == Master.Authority.SystemAdmin;
            }

            // ユーザの本務工場を取得
            int userFactoryId = TMQUtil.GetUserFactoryId(this.UserId, this.db);
            result.MainFactoryId = userFactoryId;
            // 検索結果の設定
            SetFormByDataClass<TMQUtil.HiddenInfoForMaster>(FormInfo.SpecList.Hide, new List<TMQUtil.HiddenInfoForMaster> { result });
            return true;

            /// <summary>
            /// アイテム拡張マスタから拡張データを取得する
            /// </summary>
            /// <param name="structureGroupId">構成グループID</param>
            /// <param name="seq">連番</param>
            /// <param name="structureId">構成ID</param>
            /// <returns>拡張データ</returns>
            string getItemExData(
                int structureGroupId,
                short seq,
                int structureId)
            {
                string result = null;

                // 構成アイテムを取得するパラメータ設定
                TMQUtil.StructureItemEx.StructureItemExInfo param = new()
                {
                    // 構成グループID
                    StructureGroupId = structureGroupId,
                    // 連番
                    Seq = seq
                };
                // 構成アイテム、アイテム拡張マスタ情報取得
                List<TMQUtil.StructureItemEx.StructureItemExInfo> list = TMQUtil.StructureItemEx.GetStructureItemExData(param, db);
                if (list != null)
                {
                    // 取得情報から構成IDを指定して拡張データを取得
                    result = list.Where(x => x.StructureId == structureId).Select(x => x.ExData).FirstOrDefault();
                }
                return result;
            }
        }
    }
}
