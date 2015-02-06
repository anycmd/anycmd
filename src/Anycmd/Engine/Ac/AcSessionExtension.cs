
namespace Anycmd.Engine.Ac
{
    using Exceptions;
    using Host;
    using System;

    public static class AcSessionExtension
    {
        #region IsDeveloper
        /// <summary>
        /// 判断当前用户是否是超级管理员
        /// </summary>
        /// <returns>True表示是超级管理员，False不是</returns>
        public static bool IsDeveloper(this IAcSession user)
        {
            if (user == null)
            {
                return false;
            }
            AccountState account;
            return user.Identity.IsAuthenticated && user.AcDomain.SysUserSet.TryGetDevAccount(user.Account.Id, out account);
        }
        #endregion

        #region 用户会话级数据存取接口

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="user"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetData<T>(this IAcSession user, string key)
        {
            var acSessionStorage = user.AcDomain.RetrieveRequiredService<IAcSessionStorage>();
            var obj = acSessionStorage.GetData(key);
            if (obj is T)
            {
                return (T)obj;
            }
            return default(T);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public static void SetData(this IAcSession user, string key, object data)
        {
            var acSessionStorage = user.AcDomain.RetrieveRequiredService<IAcSessionStorage>();
            acSessionStorage.SetData(key, data);
        }
        #endregion

        #region Permit

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="resourceCode"></param>
        /// <param name="functionCode"></param>
        /// <returns></returns>
        public static bool Permit(this IAcSession user, string resourceCode, string functionCode)
        {
            var securityService = user.AcDomain.RetrieveRequiredService<ISecurityService>();
            CatalogState resource;
            if (!user.AcDomain.CatalogSet.TryGetCatalog(user.AcDomain.AppSystemSet.SelfAppSystem.Code +"." + resourceCode, out resource))
            {
                throw new ValidationException("意外的资源码" + resourceCode);
            }
            FunctionState function;
            if (!user.AcDomain.FunctionSet.TryGetFunction(resource, functionCode, out function))
            {
                return true;
            }
            return securityService.Permit(user, function, null);
        }

        public static bool Permit<TEntity, TInput>(this IAcSession user, string resourceCode, string functionCode, IManagedObject currentEntity)
            where TEntity : IManagedPropertyValues
            where TInput : IManagedPropertyValues
        {
            var securityService = user.AcDomain.RetrieveRequiredService<ISecurityService>();
            CatalogState resource;
            if (!user.AcDomain.CatalogSet.TryGetCatalog(user.AcDomain.AppSystemSet.SelfAppSystem.Code +"." + resourceCode, out resource))
            {
                throw new ValidationException("意外的资源码" + resourceCode);
            }
            FunctionState function;
            if (!user.AcDomain.FunctionSet.TryGetFunction(resource, functionCode, out function))
            {
                return true;
            }
            return securityService.Permit(user, function, currentEntity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public static bool Permit(this IAcSession user, UiViewState view)
        {
            var securityService = user.AcDomain.RetrieveRequiredService<ISecurityService>();
            if (view == null)
            {
                throw new ArgumentNullException("view");
            }
            if (view == UiViewState.Empty)
            {
                return true;
            }
            FunctionState function;
            if (!user.AcDomain.FunctionSet.TryGetFunction(view.Id, out function))
            {
                return true;
            }
            return securityService.Permit(user, function, null);
        }

        public static bool Permit<TEntity, TInput>(this IAcSession user, UiViewState view, IManagedObject currentEntity)
            where TEntity : IManagedPropertyValues
            where TInput : IManagedPropertyValues
        {
            var securityService = user.AcDomain.RetrieveRequiredService<ISecurityService>();
            if (view == null)
            {
                throw new ArgumentNullException("view");
            }
            if (view == UiViewState.Empty)
            {
                return true;
            }
            FunctionState function;
            if (!user.AcDomain.FunctionSet.TryGetFunction(view.Id, out function))
            {
                return true;
            }
            return securityService.Permit(user, function, currentEntity);
        }


        // 延迟加载当前账户的权限列表，延迟到当用户触发托管操作时，节省内存
        // TODO:考虑按资源划分会话
        /// <summary>
        /// 判断当前用户是否具有给定的权限码标识的权限
        /// </summary>
        /// <returns>True表示有权，False无权</returns>
        public static bool Permit(this IAcSession user, Guid functionId)
        {
            var securityService = user.AcDomain.RetrieveRequiredService<ISecurityService>();
            FunctionState function;
            if (!user.AcDomain.FunctionSet.TryGetFunction(functionId, out function))
            {
                return true;
            }
            return securityService.Permit(user, function, null);
        }

        public static bool Permit<TEntity, TInput>(this IAcSession user, Guid functionId, IManagedObject currentEntity)
            where TEntity : IManagedPropertyValues
            where TInput : IManagedPropertyValues
        {
            var securityService = user.AcDomain.RetrieveRequiredService<ISecurityService>();
            FunctionState function;
            if (!user.AcDomain.FunctionSet.TryGetFunction(functionId, out function))
            {
                return true;
            }
            return securityService.Permit(user, function, currentEntity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        public static bool Permit(this IAcSession user, FunctionState function)
        {
            var securityService = user.AcDomain.RetrieveRequiredService<ISecurityService>();

            return securityService.Permit(user, function, null);
        }

        public static bool Permit<T, TInput>(this IAcSession user, FunctionState function, IManagedObject currentEntity)
            where T : IManagedPropertyValues
            where TInput : IManagedPropertyValues
        {
            var securityService = user.AcDomain.RetrieveRequiredService<ISecurityService>();

            return securityService.Permit(user, function, currentEntity);
        }

        public static bool Permit(this IAcSession user, FunctionState function, ManagedObject currentEntity)
        {
            var securityService = user.AcDomain.RetrieveRequiredService<ISecurityService>();

            return securityService.Permit(user, function, currentEntity);
        }
        #endregion
    }
}
