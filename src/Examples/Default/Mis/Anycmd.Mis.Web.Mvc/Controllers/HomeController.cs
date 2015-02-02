
namespace Anycmd.Mis.Web.Mvc.Controllers
{
    using Anycmd.Web.Mvc;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using Util;

    /// <summary>
    /// 首页控制器
    /// </summary>
    [Guid("031971DA-10F0-400A-9E20-91422CE40B26")]
    public class HomeController : AnycmdController
    {
        /// <summary>
        /// 首页
        /// </summary>
        [By("xuexs")]
        [CacheFilter]
        [Guid("44E3AAA1-2B7C-4280-9ABE-25B76B2A03EA")]
        public ViewResultBase Index()
        {
            return View();
        }

        [By("xuexs")]
        [Guid("2AA8C669-E202-4C0A-86EA-B357AF6B9901")]
        public ViewResultBase About()
        {
            return ViewResult();
        }

        /// <summary>
        /// 登录
        /// </summary>
        [By("xuexs")]
        [CacheFilter]
        [Guid("C331D9EC-9AAF-4A21-82E0-4A754854FED9")]
        public ActionResult LogOn()
        {
            if (AcSession.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Index");
            }
            return View();
        }

        private static ActionResult _iconImgResult = null;
        [By("xuexs")]
        [IgnoreAuth]
        [Description("获取图标")]
        [CacheFilter(Enable = true)]
        [Guid("341F719B-F254-4171-87E4-75E303FFD706")]
        public ActionResult GetIcons()
        {
            if (_iconImgResult == null)
            {
                //[{'id':1,'phrase':'[呵呵]','url':'1.gif'},{'id':2,'phrase':'[嘻嘻]','url':'2.gif'}]
                var dtinfo = new DirectoryInfo(Server.MapPath("~/Content/icons/16x16/"));
                var files = dtinfo.GetFiles();
                var icons = files.Select(f => new
                {
                    id = f.Name.Substring(0, f.Name.Length - f.Extension.Length).ToLower(),
                    phrase = f.Name.Substring(0, f.Name.Length - f.Extension.Length),
                    icon = f.Name.ToLower(),
                    url = "Content/icons/16x16/" + f.Name,
                    extension = f.Extension
                });

                _iconImgResult = this.JsonResult(icons);
            }

            return _iconImgResult;
        }
    }
}