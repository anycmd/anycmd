
namespace Anycmd.Ac.ViewModels.Infra.OrganizationViewModels
{
    using Engine;
    using Engine.Ac;
    using Exceptions;
    using System;
    using System.Collections.Generic;
    using ViewModel;

    public class OrganizationInfo : Dictionary<string, object>
    {
        private OrganizationInfo() { }

        public static OrganizationInfo Create(DicReader dic)
        {
            if (dic == null)
            {
                return null;
            }
            var data = new OrganizationInfo();
            foreach (var item in dic)
            {
                data.Add(item.Key, item.Value);
            }
            if (!data.ContainsKey("CategoryName"))
            {
                data.Add("CategoryName", dic.Host.Translate("Ac", "Organization", "CategoryName", data["CategoryCode"].ToString()));
            }
            if (data["ParentCode"] != DBNull.Value)
            {
                var parentCode = (string)data["ParentCode"];
                OrganizationState parentOrg;
                if (!dic.Host.OrganizationSet.TryGetOrganization(parentCode, out parentOrg))
                {
                    throw new AnycmdException("意外的父目录编码" + parentCode);
                }
                data.Add("ParentName", parentOrg.Name);
            }
            else
            {
                data.Add("ParentName", OrganizationState.VirtualRoot.Name);
            }

            return data;
        }
    }
}
