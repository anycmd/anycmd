
namespace Anycmd.Web.Mvc
{
    using Engine;
    using Engine.Ac;
    using Exceptions;
    using System;
    using System.Web.Mvc;
    using Util;
    using ViewModel;

    /// <summary>
    /// 所有控制器必须继承该类
    /// </summary>
    [AuthorizeFilter(Order = 20)]
    [CompressFilter(Order = 30)]
    [ExceptionFilter(Order = int.MaxValue)]
    public class AnycmdController : BaseController
    {
        private EntityTypeState _entityType;
        protected EntityTypeState EntityType
        {
            get {
                return _entityType ??
                       (_entityType =
                           GetEntityType(new Coder(RouteData.DataTokens["area"].ToString(),
                               RouteData.Values["controller"].ToString())));
            }
        }


        protected IAcSession AcSession
        {
            get {
                return User.Identity.IsAuthenticated ? AcSessionState.AcMethod.GetAcSession(AcDomain, User.Identity.Name) : AcSessionState.Empty;
            }
        }


        protected EntityTypeState GetEntityType(Coder code)
        {
            EntityTypeState entityTypeEntityType;
            if (!AcDomain.EntityTypeSet.TryGetEntityType(code, out entityTypeEntityType))
            {
                throw new InvalidEntityTypeCodeException(code);
            }
            return entityTypeEntityType;
        }
        
        protected ActionResult HandleSeparateGuidString(Action<IAcSession, Guid> action, IAcSession acSession, string id, params char[] separator)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            string[] ids = id.Split(separator);
            var idArray = new Guid[ids.Length];
            for (int i = 0; i < ids.Length; i++)
            {
                Guid tmp;
                if (Guid.TryParse(ids[i], out tmp))
                {
                    idArray[i] = tmp;
                }
                else
                {
                    throw new ValidationException("意外的Guid格式" + ids[i]);
                }
            }
            foreach (var item in idArray)
            {
                action(acSession, item);
            }
            return this.JsonResult(new ResponseData { id = id, success = true });
        }
    }
}
