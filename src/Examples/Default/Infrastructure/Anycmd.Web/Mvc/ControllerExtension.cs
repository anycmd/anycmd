
namespace Anycmd.Web.Mvc
{
    using Exceptions;
    using System;
    using System.Web.Mvc;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public static class ControllerExtension
    {
        public static FormatJsonResult JsonResult(this Controller c, object data)
        {
            return new FormatJsonResult { Data = data };
        }

        #region DetailsResult

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="c"></param>
        /// <param name="query"></param>
        /// <param name="tableOrViewName"></param>
        /// <returns></returns>
        public static ViewResultBase DetailsResult(this Controller c, IQuery query, string tableOrViewName)
        {
            if (!string.IsNullOrEmpty(c.Request["isTooltip"]))
            {
                Guid id;
                if (Guid.TryParse(c.Request["id"], out id))
                {
                    var data = query.Get(tableOrViewName, id);
                    return new PartialViewResult { ViewName = "Partials/Details", ViewData = new ViewDataDictionary(data) };
                }
                else
                {
                    throw new ValidationException("非法的Guid标识" + c.Request["id"]);
                }
            }
            else if (!string.IsNullOrEmpty(c.Request["isInner"]))
            {
                return new PartialViewResult { ViewName = "Partials/Details" };
            }
            else
            {
                return new ViewResult { ViewName = "Details" };
            }
        }
        #endregion
    }
}
