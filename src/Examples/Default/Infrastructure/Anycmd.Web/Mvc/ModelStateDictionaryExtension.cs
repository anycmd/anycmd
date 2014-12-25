
namespace Anycmd.Web.Mvc
{
    using System.Web.Mvc;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public static class ModelStateDictionaryExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static FormatJsonResult ToJsonResult(this ModelStateDictionary modelState)
        {
            string msg = string.Empty;
            foreach (var item in modelState)
            {
                foreach (var e in item.Value.Errors)
                {
                    msg += e.ErrorMessage;
                }
            }
            var result = new FormatJsonResult()
            {
                Data = new ResponseData { msg = msg, success = false }.Warning()
            };

            return result;
        }
    }
}
