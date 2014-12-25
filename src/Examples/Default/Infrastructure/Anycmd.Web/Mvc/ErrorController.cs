
namespace Anycmd.Web.Mvc
{
    using System.Web.Mvc;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public class ErrorController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unknownAction"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public ActionResult Http404(string unknownAction, string url)
        {
            var isAjaxRequest = Request.IsAjaxRequest();
            if (isAjaxRequest)
            {
                return new FormatJsonResult
                {
                    Data = new ResponseData { success = false, msg = "404 " + url }.Error()
                };
            }
            else
            {
                return new ContentResult() { Content = "404 " + url };
            }
        }
    }
}
