
namespace Anycmd.Ac.ViewModels.Infra.AppSystemViewModels
{
    using Engine.Ac;
    using Exceptions;
    using Model;
    using System;
    using System.Collections.Generic;
    using ViewModel;

    public class AppSystemInfo : Dictionary<string, object>
    {
        private AppSystemInfo() { }

        public static AppSystemInfo Create(DicReader dic)
        {
            if (dic == null)
            {
                return null;
            }
            var data = new AppSystemInfo();
            foreach (var item in dic)
            {
                data.Add(item.Key, item.Value);
            }
            AccountState principal;
            if (!dic.Host.SysUsers.TryGetDevAccount((Guid)data["PrincipalId"], out principal))
            {
                throw new AnycmdException("意外的开发人员标识" + data["PrincipalId"]);
            }
            if (!data.ContainsKey("PrincipalName"))
            {
                data.Add("PrincipalName", principal.LoginName);
            }
            if (!data.ContainsKey("IsEnabledName"))
            {
                data.Add("IsEnabledName", dic.Host.Translate("Ac", "AppSystem", "IsEnabledName", data["IsEnabled"].ToString()));
            }

            return data;
        }
    }
}
