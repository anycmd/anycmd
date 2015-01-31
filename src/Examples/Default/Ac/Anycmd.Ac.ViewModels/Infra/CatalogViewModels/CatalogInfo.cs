
namespace Anycmd.Ac.ViewModels.Infra.CatalogViewModels
{
    using Engine;
    using Engine.Ac;
    using Exceptions;
    using System;
    using System.Collections.Generic;
    using ViewModel;

    public class CatalogInfo : Dictionary<string, object>
    {
        private CatalogInfo() { }

        public static CatalogInfo Create(DicReader dic)
        {
            if (dic == null)
            {
                return null;
            }
            var data = new CatalogInfo();
            foreach (var item in dic)
            {
                data.Add(item.Key, item.Value);
            }
            if (!data.ContainsKey("CategoryName"))
            {
                data.Add("CategoryName", dic.AcDomain.Translate("Ac", "Catalog", "CategoryName", data["CategoryCode"].ToString()));
            }
            if (data["ParentCode"] != DBNull.Value)
            {
                var parentCode = (string)data["ParentCode"];
                CatalogState parentOrg;
                if (!dic.AcDomain.CatalogSet.TryGetCatalog(parentCode, out parentOrg))
                {
                    throw new AnycmdException("意外的父目录编码" + parentCode);
                }
                data.Add("ParentName", parentOrg.Name);
            }
            else
            {
                data.Add("ParentName", CatalogState.VirtualRoot.Name);
            }

            return data;
        }
    }
}
