
namespace Anycmd.Web.Mvc
{
    using Exceptions;
    using Newtonsoft.Json;
    using Query;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Web.Mvc;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public sealed class PlistModelBinder : DefaultModelBinder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="bindingContext"></param>
        /// <param name="propertyDescriptor"></param>
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
        {
            if (typeof(IGetPlistResult).IsAssignableFrom(bindingContext.ModelType))
            {
                if (propertyDescriptor.Name.Equals("filters", StringComparison.OrdinalIgnoreCase))
                {
                    var model = bindingContext.Model as IGetPlistResult;
                    if (model == null)
                    {
                        throw new CoreException();
                    }
                    var json = controllerContext.RequestContext.HttpContext.Request.Params["filters"];
                    List<FilterData> value = null;
                    if (!string.IsNullOrEmpty(json))
                    {
                        value = JsonConvert.DeserializeObject<List<FilterData>>(json);
                    }
                    if (value == null)
                    {
                        value = new List<FilterData>();
                    }
                    model.Filters = value;
                }
                else
                {
                    base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
                }
            }
            else
            {
                base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
            }
        }
    }
}
