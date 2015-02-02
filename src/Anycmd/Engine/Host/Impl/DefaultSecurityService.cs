
namespace Anycmd.Engine.Host.Impl
{
    using Engine.Ac;
    using System;

    public class DefaultSecurityService : ISecurityService
    {
        public bool Permit(IAcSession user, FunctionState function, IManagedObject data)
        {
            if (function == null)
            {
                throw new ArgumentNullException("function");
            }
            // 如果非托管
            if (!function.IsManaged)
            {
                return true;
            }
            if (!user.Identity.IsAuthenticated)
            {
                return false;
            }
            if (user.IsDeveloper())
            {
                return true;
            }
            var functionIDs = user.AccountPrivilege.AuthorizedFunctionIDs;

            if (!functionIDs.Contains(function.Id))
            {
                return false;
            }
            if (data != null)
            {
                // TODO:验证实体级权限。anycmd 1.0版本暂不支持，后续版本支持
            }
            return true;
        }
    }
}
