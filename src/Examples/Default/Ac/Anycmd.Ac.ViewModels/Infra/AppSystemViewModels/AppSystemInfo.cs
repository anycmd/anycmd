
namespace Anycmd.Ac.ViewModels.Infra.AppSystemViewModels
{
    using Engine;
    using Engine.Ac;
    using Exceptions;
    using System;
    using System.Collections.Generic;
    using ViewModel;

    public class AppSystemInfo : Dictionary<string, object>
    {
        private AppSystemInfo(DicReader dic)
            : base(dic)
        {
            AccountState principal;
            if (!dic.AcDomain.SysUserSet.TryGetDevAccount((Guid)this["PrincipalId"], out principal))
            {
                throw new AnycmdException("意外的开发人员标识" + this["PrincipalId"]);
            }
            if (!this.ContainsKey("PrincipalName"))
            {
                this.Add("PrincipalName", principal.LoginName);
            }
            if (!this.ContainsKey("IsEnabledName"))
            {
                this.Add("IsEnabledName", dic.AcDomain.Translate("Ac", "AppSystem", "IsEnabledName", this["IsEnabled"].ToString()));
            }
        }

        public static AppSystemInfo Create(DicReader dic)
        {
            if (dic == null)
            {
                return null;
            }
            var data = new AppSystemInfo(dic);

            return data;
        }
    }
}
