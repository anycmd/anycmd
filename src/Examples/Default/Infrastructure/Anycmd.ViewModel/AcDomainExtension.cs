
namespace Anycmd.ViewModel
{
    using Engine.Ac;
    using Exceptions;
    using System.Collections.Generic;
    using Util;

    /// <summary>
    /// 为AcDomain提供扩展方法。<see cref="IAcDomain"/>
    /// </summary>
    public static class AcDomainExtension
    {
        /// <summary>
        /// 根据系统字典将字典项码翻译为字典项名
        /// </summary>
        /// <param name="acDomain"></param>
        /// <param name="dicCode"></param>
        /// <param name="dicItemCode"></param>
        /// <returns></returns>
        public static string Translate(this IAcDomain acDomain, string dicCode, int dicItemCode)
        {
            return Translate(acDomain, dicCode, dicItemCode.ToString());
        }

        /// <summary>
        /// 根据系统字典将字典项码翻译为字典项名
        /// </summary>
        /// <param name="acDomain"></param>
        /// <param name="dicCode"></param>
        /// <param name="dicItemCode"></param>
        /// <returns></returns>
        public static string Translate(this IAcDomain acDomain, string dicCode, bool dicItemCode)
        {
            return Translate(acDomain, dicCode, dicItemCode.ToString());
        }

        /// <summary>
        /// 根据系统字典将字典项码翻译为字典项名
        /// </summary>
        /// <param name="acDomain"></param>
        /// <param name="dicCode"></param>
        /// <param name="dicItemCode"></param>
        /// <returns></returns>
        public static string Translate(this IAcDomain acDomain, string dicCode, string dicItemCode)
        {
            CatalogState dicItem;
            if (acDomain.CatalogSet.TryGetCatalog(acDomain.AppSystemSet.SelfAppSystem.Code + "." + dicCode + "." + dicItemCode, out dicItem))
            {
                return dicItem.Name;
            }
            return dicItemCode;
        }

        /// <summary>
        /// 根据系统字典将字典项码翻译为字典项名
        /// </summary>
        /// <param name="acDomain"></param>
        /// <param name="codespace"></param>
        /// <param name="entityTypeCode"></param>
        /// <param name="propertyCode"></param>
        /// <param name="dicItemCode"></param>
        /// <returns></returns>
        public static string Translate(this IAcDomain acDomain, string codespace, string entityTypeCode, string propertyCode, int dicItemCode)
        {
            return Translate(acDomain, codespace, entityTypeCode, propertyCode, dicItemCode.ToString());
        }

        /// <summary>
        /// 根据系统字典将字典项码翻译为字典项名
        /// </summary>
        /// <param name="acDomain"></param>
        /// <param name="codespace"></param>
        /// <param name="entityTypeCode"></param>
        /// <param name="propertyCode"></param>
        /// <param name="dicItemCode"></param>
        /// <returns></returns>
        public static string Translate(this IAcDomain acDomain, string codespace, string entityTypeCode, string propertyCode, bool dicItemCode)
        {
            return Translate(acDomain, codespace, entityTypeCode, propertyCode, dicItemCode.ToString());
        }

        /// <summary>
        /// 根据系统字典将字典项码翻译为字典项名
        /// </summary>
        /// <param name="acDomain"></param>
        /// <param name="codespace"></param>
        /// <param name="entityTypeCode"></param>
        /// <param name="propertyCode"></param>
        /// <param name="dicItemCode"></param>
        /// <returns></returns>
        public static string Translate(this IAcDomain acDomain, string codespace, string entityTypeCode, string propertyCode, string dicItemCode)
        {
            EntityTypeState entityType;
            if (!acDomain.EntityTypeSet.TryGetEntityType(new Coder(codespace, entityTypeCode), out entityType))
            {
                throw new AnycmdException("意外的实体类型" + codespace + entityTypeCode);
            }
            PropertyState property;
            if (!acDomain.EntityTypeSet.TryGetProperty(entityType, propertyCode, out property))
            {
                return dicItemCode;
            }
            if (property.DicId.HasValue)
            {
                CatalogState dicState;
                if (!acDomain.CatalogSet.TryGetCatalog(property.DicId.Value, out dicState))
                {
                    return dicItemCode;
                }
                CatalogState dicitem;
                if (acDomain.CatalogSet.TryGetCatalog(dicState.Code + "." + dicItemCode, out dicitem))
                {
                    return dicitem.Name;
                }
            }
            return dicItemCode;
        }
    }
}
