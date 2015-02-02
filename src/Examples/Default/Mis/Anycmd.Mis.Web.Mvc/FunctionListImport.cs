
namespace Anycmd.Mis.Web.Mvc
{
    using Ac.ViewModels.Infra.FunctionViewModels;
    using Ac.ViewModels.Infra.UIViewViewModels;
    using Anycmd.Web.Mvc;
    using Engine.Ac;
    using Engine.Ac.Abstractions;
    using Engine.Ac.Abstractions.Infra;
    using Engine.Ac.Messages;
    using Engine.Ac.Messages.Infra;
    using Engine.Host;
    using Engine.Host.Ac;
    using Exceptions;
    using Repositories;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using Util;

    /// <summary>
    /// 暂时支持的是导入本系统的功能列表，数据源是通过反射控制器层得到的，后续支持以xml文件的方式导入其它系统的功能列表
    /// </summary>
    public class FunctionListImport : IFunctionListImport
    {
        private static readonly object Locker = new object();
        private static bool _isChanged = true;

        public void Import(IAcDomain host, IAcSession userSession, string appSystemCode)
        {
            if (_isChanged)
            {
                lock (Locker)
                {
                    if (_isChanged)
                    {
                        var privilegeBigramRepository = host.RetrieveRequiredService<IRepository<Privilege>>();
                        if (string.IsNullOrEmpty(appSystemCode))
                        {
                            throw new ArgumentNullException("appSystemCode");
                        }
                        AppSystemState appSystem;
                        if (!host.AppSystemSet.TryGetAppSystem(appSystemCode, out appSystem))
                        {
                            throw new ValidationException("意外的应用系统码" + appSystemCode);
                        }
                        // TODO：提取配置
                        var assemblyStrings = new List<string> {
                            "Anycmd.Ac.Web.Mvc",
                            "Anycmd.Edi.Web.Mvc",
                            "Anycmd.Mis.Web.Mvc"
                        };
                        var dlls = assemblyStrings.Select(Assembly.Load).ToList();
                        var oldPages = host.UiViewSet;
                        var oldFunctions = host.FunctionSet.Cast<IFunction>().ToList();
                        var reflectionFunctions = new List<FunctionId>();
                        #region 通过反射程序集初始化功能和页面列表
                        foreach (var dll in dlls)
                        {
                            // 注意这里约定区域名为二级命名空间的名字
                            var areaCode = dll.GetName().Name.Split('.')[1];

                            var types = dll.GetTypes();
                            var actionResultType = typeof(ActionResult);
                            var controllerType = typeof(AnycmdController);
                            var viewResultType = typeof(ViewResultBase);
                            foreach (var type in types)
                            {
                                var isController = controllerType.IsAssignableFrom(type);
                                // 跳过不是Controller的类型
                                if (!isController)
                                {
                                    continue;
                                }
                                var controller = type.Name.Substring(0, type.Name.Length - "Controller".Length);
                                var resourceCode = controller;// 注意这里约定资源码等于控制器名
                                var methodInfos = type.GetMethods();
                                int sortCode = 10;
                                foreach (var method in methodInfos)
                                {
                                    bool isPage = viewResultType.IsAssignableFrom(method.ReturnType);
                                    bool isAction = isPage || actionResultType.IsAssignableFrom(method.ReturnType);
                                    string action = method.Name;

                                    ResourceTypeState resource;
                                    string description;
                                    Guid developerId;
                                    // 跳过不是Action的方法
                                    if (!isAction)
                                    {
                                        continue;
                                    }
                                    var descriptionAttrs = method.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), inherit: false);
                                    // 如果打有描述文本标记则使用描述文本作为操作说明，否则试用Action方法名
                                    if (descriptionAttrs.Length > 0)
                                    {
                                        description = ((DescriptionAttribute) descriptionAttrs[0]).Description;
                                        if (string.IsNullOrEmpty(description))
                                        {
                                            description = action;
                                        }
                                    }
                                    else
                                    {
                                        description = action;
                                    }
                                    object[] byAttrs = method.GetCustomAttributes(typeof(ByAttribute), inherit: false);
                                    if (byAttrs.Length > 0)
                                    {
                                        string loginName = ((ByAttribute) byAttrs[0]).DeveloperCode;
                                        AccountState developer;
                                        if (!host.SysUserSet.TryGetDevAccount(loginName, out developer))
                                        {
                                            throw new ValidationException("意外的开发人员" + loginName + "在" + controllerType.FullName + "在" + method.Name);
                                        }
                                        developerId = developer.Id;
                                    }
                                    else
                                    {
                                        throw new ValidationException(type.FullName + method.Name);
                                    }

                                    if (!host.ResourceTypeSet.TryGetResource(appSystem, resourceCode, out resource))
                                    {
                                        throw new ValidationException("意外的资源码" + resourceCode);
                                    }
                                    var oldFunction = oldFunctions.FirstOrDefault(
                                            o => o.ResourceTypeId == resource.Id
                                            && string.Equals(o.Code, action, StringComparison.OrdinalIgnoreCase));
                                    if (oldFunction == null)
                                    {
                                        // TODO:持久化托管标识
                                        var function = FunctionId.Create(Guid.NewGuid(), appSystemCode, areaCode, resourceCode, action);
                                        if (reflectionFunctions.Any(a => a.AreaCode == areaCode && a.ResourceCode == resourceCode && string.Equals(a.FunctionCode, action, StringComparison.OrdinalIgnoreCase)))
                                        {
                                            throw new ValidationException("同一Controller下不能有命名相同的Action。" + method.DeclaringType.FullName + "." + method.Name);
                                        }
                                        reflectionFunctions.Add(function);
                                        host.Handle(new FunctionCreateInput()
                                        {
                                            Description = description,
                                            DeveloperId = developerId,
                                            Id = function.Id,
                                            IsEnabled = 1,
                                            IsManaged = false,
                                            ResourceTypeId = resource.Id,
                                            SortCode = sortCode,
                                            Code = function.FunctionCode
                                        }.ToCommand(userSession));
                                        if (isPage)
                                        {
                                            host.Handle(new UiViewCreateInput
                                            {
                                                Id = function.Id
                                            }.ToCommand(userSession));
                                        }
                                    }
                                    else
                                    {
                                        // TODO:持久化托管标识
                                        // 更新作者
                                        if (oldFunction.DeveloperId != developerId)
                                        {
                                            host.Handle(new FunctionUpdateInput
                                            {
                                                Code = oldFunction.Code,
                                                Description = oldFunction.Description,
                                                DeveloperId = developerId,
                                                Id = oldFunction.Id,
                                                SortCode = oldFunction.SortCode
                                            }.ToCommand(userSession));
                                        }
                                        reflectionFunctions.Add(FunctionId.Create(oldFunction.Id, appSystemCode, areaCode, resourceCode, action));
                                        if (isPage)
                                        {
                                            if (oldPages.All(a => a.Id != oldFunction.Id))
                                            {
                                                host.Handle(new UiViewCreateInput
                                                {
                                                    Id = oldFunction.Id
                                                }.ToCommand(userSession));
                                            }
                                        }
                                        else
                                        {
                                            // 删除废弃的页面
                                            if (oldPages.All(a => a.Id != oldFunction.Id))
                                            {
                                                host.Handle(new RemoveUiViewCommand(userSession, oldFunction.Id));
                                            }
                                        }
                                    }
                                    sortCode += 10;
                                }
                            }
                        }
                        #endregion
                        #region 删除废弃的功能
                        foreach (var oldFunction in oldFunctions)
                        {
                            if (reflectionFunctions.All(o => o.Id != oldFunction.Id))
                            {
                                // 删除角色功能
                                var privilegeType = AcElementType.Function.ToName();
                                foreach (var rolePrivilege in privilegeBigramRepository.AsQueryable().Where(a => privilegeType == a.ObjectType && a.ObjectInstanceId == oldFunction.Id).ToList())
                                {
                                    privilegeBigramRepository.Remove(rolePrivilege);
                                    host.EventBus.Publish(new PrivilegeRemovedEvent(userSession, rolePrivilege));
                                }
                                host.EventBus.Commit();
                                privilegeBigramRepository.Context.Commit();
                                host.Handle(new RemoveFunctionCommand(userSession, oldFunction.Id));
                            }
                        }
                        #endregion
                        _isChanged = false;
                    }
                }
            }
        }

        private struct FunctionId
        {
            public static FunctionId Create(Guid id, string appSystemCode, string areaCode, string resourceCode, string functionCode)
            {
                return new FunctionId
                {
                    Id = id,
                    AppSystemCode = appSystemCode,
                    AreaCode = areaCode,
                    ResourceCode = resourceCode,
                    FunctionCode = functionCode
                };
            }

            public Guid Id { get; private set; }
            public string AppSystemCode { get; private set; }
            public string AreaCode { get; private set; }
            public string ResourceCode { get; private set; }
            public string FunctionCode { get; private set; }
        }
    }
}
