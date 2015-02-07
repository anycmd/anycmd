
namespace Anycmd.Edi.Web.Mvc
{
    using System.Web.Mvc;

    /// <summary>
    /// 提供在 ASP.NET MVC 应用程序内注册Edi区域的方式。
    /// </summary>
    public class EdiAreaRegistration : AreaRegistration
    {
        /// <summary>
        /// 区域名：值为Edi
        /// </summary>
        public override string AreaName
        {
            get
            {
                return "Edi";
            }
        }

        /// <summary>
        /// 使用指定区域的上下文信息在 ASP.NET MVC 应用程序内注册某个区域。
        /// </summary>
        /// <param name="context">对注册区域所需的信息进行封装</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Edi_default",
                "Edi/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "Anycmd.Edi.Web.Mvc.Controllers" }
            );
        }
    }
}
