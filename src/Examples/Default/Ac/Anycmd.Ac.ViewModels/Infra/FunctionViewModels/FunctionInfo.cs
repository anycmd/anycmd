
namespace Anycmd.Ac.ViewModels.Infra.FunctionViewModels
{
    using Engine.Ac;
    using Exceptions;
    using Model;
    using System;
    using System.Collections.Generic;
    using ViewModel;

    public class FunctionInfo : Dictionary<string, object>
    {
        private FunctionInfo() { }

        public static FunctionInfo Create(DicReader dic)
        {
            if (dic == null)
            {
                return null;
            }
            var data = new FunctionInfo();
            foreach (var item in dic)
            {
                data.Add(item.Key, item.Value);
            }
            ResourceTypeState resource;
            if (!dic.Host.ResourceTypeSet.TryGetResource((Guid)data["ResourceTypeId"], out resource))
            {
                throw new AnycmdException("意外的资源标识" + data["ResourceTypeId"]);
            }
            AppSystemState appSystem;
            if (!dic.Host.AppSystemSet.TryGetAppSystem(resource.AppSystemId, out appSystem))
            {
                throw new AnycmdException("意外的区域应用系统标识");
            }
            if (!data.ContainsKey("AppSystemCode"))
            {
                data.Add("AppSystemCode", appSystem.Code);
            }
            if (!data.ContainsKey("AppSystemName"))
            {
                data.Add("AppSystemName", appSystem.Name);
            }

            if (!data.ContainsKey("ResourceCode"))
            {
                data.Add("ResourceCode", resource.Code);
            }
            if (!data.ContainsKey("ResourceName"))
            {
                data.Add("ResourceName", resource.Name);
            }
            if (!data.ContainsKey("IsManagedName"))
            {
                data.Add("IsManagedName", dic.Host.Translate("Ac", "DicItem", "IsManagedName", data["IsManaged"].ToString()));
            }
            if (!data.ContainsKey("IsEnabledName"))
            {
                data.Add("IsEnabledName", dic.Host.Translate("Ac", "DicItem", "IsEnabledName", data["IsEnabled"].ToString()));
            }
            if (!data.ContainsKey("IsUiView"))
            {
                UiViewState view;
                data.Add("IsUiView", dic.Host.UiViewSet.TryGetUiView((Guid)data["Id"], out view));
            }
            if (!data.ContainsKey("DeveloperCode"))
            {
                AccountState developer;
                if (dic.Host.SysUsers.TryGetDevAccount((Guid)data["DeveloperId"], out developer))
                {
                    data.Add("DeveloperCode", developer.LoginName);
                }
                else
                {
                    data.Add("DeveloperCode", "无效的值");
                }
            }

            return data;
        }
    }
}
