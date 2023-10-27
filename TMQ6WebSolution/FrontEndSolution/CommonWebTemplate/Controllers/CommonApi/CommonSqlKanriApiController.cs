///<summary>
/// 機能名　　：　【共通】
/// タイトル　：　管理SQL制御用WebAPIController
/// 説明　　　：　SQLIDで管理されたSQL文を発行してデータ取得を行います。
/// 
/// 履歴　　　：　2017.08.01 河村純子　新規作成
///</summary>

using System;
using System.Collections.Generic;
using System.Net;

using CommonWebTemplate.Models.Common;
using CommonWebTemplate.CommonUtil;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace CommonWebTemplate.Controllers.CommonApi
{

    [Route("api/[controller]")]
    [ApiController]
    public class CommonSqlKanriApiController : BaseApiController
    {
        /// <summary>
        /// SQLIDで指定されたSQL文のデータ取得処理
        /// </summary>
        /// <param name="id">SQLID</param>
        /// <param name="param">SQLﾊﾟﾗﾒｰﾀ</param>
        /// <param name="code">ｺｰﾄﾞ名称用のﾊﾟﾗﾒｰﾀ</param>
        /// <param name="input">ｵｰﾄｺﾝﾌﾟﾘｰﾄの入力文字</param>
        /// <returns>取得データ</returns>
        /// <remarks>
        /// 取得用URL:http://localhost:****/api/CommonSqlKanriApi/[SQLID]?param=[SQLPARAM]
        ///      (例):http://localhost:****/api/CommonSqlKanriApi/CC001?param='S12'
        /// </remarks>
        [HttpGet("{id}")]
        public ActionResult Get(string id, [FromQuery] string param = "", [FromQuery] string code = "", [FromQuery] string input = "", [FromQuery] string factoryId = "")
        {
            try
            {
                CommonProcData procData = new CommonProcData();
                string prmFactoryId = factoryId;

                //ﾘｸｴｽﾄ情報から業務ﾛｼﾞｯｸ処理に必要な情報を取得
                SetRequestInfo(ref procData);

                if (string.IsNullOrEmpty(prmFactoryId) && procData.BelongingInfo != null)
                {
                    prmFactoryId = string.Join(',', procData.BelongingInfo.BelongingFactoryIdList);
                }

                //SQLIDで管理されたﾃﾞｰﾀを取得する
                BusinessLogicIO logicIO = new BusinessLogicIO(procData);
                object results = null;
                CommonProcReturn returnInfo = logicIO.CallDllBusinessLogic_CtrlSql(id, param, code, input, prmFactoryId, out results);

                if (returnInfo.IsProcEnd())
                {
                    return BadRequest(returnInfo);
                }

                return Ok(results);

            }
            catch (Exception ex)
            {
                //return InternalServerError(ex);
                System.Diagnostics.Debug.WriteLine(ex.Message);

                CommonProcData procData = new CommonProcData();

                //ﾘｸｴｽﾄ情報から業務ﾛｼﾞｯｸ処理に必要な情報を取得
                SetRequestInfo(ref procData);

                BusinessLogicUtil blogic = new BusinessLogicUtil();
                //メッセージ取得
                blogic.GetResourceName(procData, new List<string> { "941100002" }, out IDictionary<string, string> resources);
                //処理に失敗しました。
                string message = blogic.ConvertResourceName("941100002", resources);
                //コンボ、オートコンプリート処理に失敗しました。
                return BadRequest(new CommonProcReturn(CommonProcReturn.ProcStatus.InValid, message));

            }
        }

    }
}