using CommonWebTemplate.CommonUtil;
using CommonWebTemplate.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CommonWebTemplate.CommonUtil.RequestManageUtil;

namespace CommonWebTemplate.Controllers.CommonApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        /// <summary>
        /// ﾘｸｴｽﾄ情報から業務ﾛｼﾞｯｸ処理に必要な情報を設定して返す
        ///  - WebApi Controller用
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="procData">業務ﾛｼﾞｯｸﾃﾞｰﾀ(I/O)</param>
        protected void SetRequestInfo(ref CommonProcData procData)
        {
            //IPｱﾄﾞﾚｽを取得
            if ("test".Equals(AppCommonObject.Config.AppSettings.DeployMode) ||
                "prot".Equals(AppCommonObject.Config.AppSettings.DeployMode))
            {
                //プロト作成モード
                procData.TerminalNo = @"::1";
            }
            else
            {
                if (String.IsNullOrEmpty(procData.TerminalNo))
                {
                    procData.TerminalNo = SystemUtil.GetClientIpAdress(ControllerContext);
                }
            }
            //Urlﾊﾟｽを取得
            procData.BaseUrl = SystemUtil.GetBaseUrlFromRequest(ControllerContext);

            //ｾｯｼｮﾝに保持しているﾕｰｻﾞｰ情報を取得
            procData.LoginUserId = null;
            procData.LoginId = null;
            procData.LoginUserName = null;
            if (ControllerContext.HttpContext.Session.Keys.Contains(SessionKey.CIM_USER_INFO))
            {
                UserInfoDef userInfo = ControllerContext.HttpContext.Session.GetObject<UserInfoDef>(SessionKey.CIM_USER_INFO);
                procData.LoginUserId = userInfo.UserId;
                procData.LoginId = userInfo.LoginId;
                procData.LoginUserName = userInfo.UserName;
                procData.LanguageId = userInfo.LanguageId;
                procData.GUID = userInfo.GUID;
                procData.AuthorityLevelId = userInfo.AuthorityLevelId;
                procData.BelongingInfo = userInfo.BelongingInfo;

                //=== 権限情報 ==
                //ﾕｰｻﾞｰ機能権限のある機能ﾏｽﾀﾘｽﾄ
                procData.UserAuthConducts = userInfo.UserAuthConducts;
                //★インメモリ化対応 start
                procData.CustomizeList = userInfo.CustomizeList;
                //★インメモリ化対応 end
            }
        }

        /// <summary>
        /// ブラウザの言語を取得
        /// </summary>
        /// <returns>ブラウザの言語</returns>
        protected string GetBrowserLanguage()
        {
            return Request.GetTypedHeaders().AcceptLanguage.OrderByDescending(x => x.Quality ?? 1).Select(x => x.Value.ToString()).ToList().FirstOrDefault();
        }
    }
}
