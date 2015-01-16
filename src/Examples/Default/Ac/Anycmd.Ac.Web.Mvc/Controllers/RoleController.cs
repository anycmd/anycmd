
namespace Anycmd.Ac.Web.Mvc.Controllers
{
    using Anycmd.Web.Mvc;
    using Engine.Ac;
    using Engine.Ac.Abstractions;
    using Engine.Ac.Messages;
    using Engine.Ac.Messages.Rbac;
    using Engine.Host.Ac;
    using Exceptions;
    using MiniUI;
    using Repositories;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Mvc;
    using Util;
    using ViewModel;
    using ViewModels;
    using ViewModels.PrivilegeViewModels;
    using ViewModels.RoleViewModels;

    /// <summary>
    /// 系统角色模型视图控制器
    /// </summary>
    [Guid("78C0154F-9F40-4491-910F-B9443CF53122")]
    public class RoleController : AnycmdController
    {
        #region ViewResults
        [By("xuexs")]
        [Description("角色管理")]
        [Guid("D0A30CB6-B397-4EE4-83CC-3BA27B635764")]
        public ViewResultBase Index()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("角色详细信息")]
        [Guid("F875CB89-4645-48BD-80BD-6634F1B7095F")]
        public ViewResultBase Details()
        {
            if (!string.IsNullOrEmpty(Request["isTooltip"]))
            {
                Guid id;
                if (Guid.TryParse(Request["id"], out id))
                {
                    var data = base.EntityType.GetData(id);
                    return new PartialViewResult { ViewName = "Partials/Details", ViewData = new ViewDataDictionary(data) };
                }
                else
                {
                    throw new ValidationException("非法的Guid标识" + Request["id"]);
                }
            }
            else if (!string.IsNullOrEmpty(Request["isInner"]))
            {
                return new PartialViewResult { ViewName = "Partials/Details" };
            }
            else
            {
                return this.View();
            }
        }

        [By("xuexs")]
        [Description("角色成员(账户)列表")]
        [Guid("7F611BEF-80C6-4DFA-9916-1963DA7865CF")]
        public ViewResultBase Accounts()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("角色功能权限列表")]
        [Guid("A983226A-268D-4123-B143-49FA00C2FA4D")]
        public ViewResultBase Permissions()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("角色菜单")]
        [Guid("2270E1BA-9898-46BF-AB59-FBC13AB9B6BB")]
        public ViewResultBase Menus()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("角色工作组列表")]
        [Guid("DC26460D-C39B-46F1-8542-06A5770427CF")]
        public ViewResultBase Groups()
        {
            return ViewResult();
        }

        #endregion

        [By("xuexs")]
        [Description("根据ID获取角色")]
        [Guid("D4772B33-211C-4C66-B96A-681B14809826")]
        public ActionResult Get(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(base.EntityType.GetData(id.Value));
        }

        [By("xuexs")]
        [Description("根据ID获取角色详细信息")]
        [Guid("B52D21E6-C453-44FE-93FB-2DF310A6DD31")]
        public ActionResult GetInfo(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(base.EntityType.GetData(id.Value));
        }

        [By("xuexs")]
        [Description("跟夜获取角色信息")]
        [Guid("1A23E383-1634-4CA7-BD90-BC11DDDFB2E0")]
        public ActionResult GetPlistRoles(GetPlistResult requestModel)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            var data = AcDomain.GetPlistRoles(requestModel);

            Debug.Assert(requestModel.Total != null, "requestModel.total != null");
            return this.JsonResult(new MiniGrid { total = requestModel.Total.Value, data = data.Select(a => a.ToTableRowData()) });
        }

        #region GetPlistAccountRoles
        [By("xuexs")]
        [Description("根据账户ID分页获取角色")]
        [Guid("2680E779-62CA-4168-A169-5F00E5D87E5D")]
        public ActionResult GetPlistAccountRoles(GetPlistAccountRoles requestData)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            var data = new List<AccountAssignRoleTr>();
            var privilegeType = AcElementType.Role.ToName();
            var accountRoles = GetRequiredService<IRepository<Privilege>>().AsQueryable().Where(a => a.SubjectInstanceId == requestData.AccountId && a.ObjectType == privilegeType);
            if (requestData.IsAssigned.HasValue)
            {
                if (requestData.IsAssigned.Value)
                {
                    foreach (var ar in accountRoles)
                    {
                        RoleState role;
                        if (!AcDomain.RoleSet.TryGetRole(ar.ObjectInstanceId, out role))
                        {
                            throw new AnycmdException("意外的角色标识" + ar.ObjectInstanceId);
                        }
                        data.Add(new AccountAssignRoleTr
                        {
                            AccountId = requestData.AccountId,
                            IsAssigned = true,
                            RoleId = ar.ObjectInstanceId,
                            CreateBy = ar.CreateBy,
                            CreateOn = ar.CreateOn,
                            CreateUserId = ar.CreateUserId,
                            Id = ar.Id,
                            IsEnabled = role.IsEnabled,
                            CategoryCode = role.CategoryCode,
                            Name = role.Name,
                            SortCode = role.SortCode,
                            Icon = role.Icon
                        });
                    }
                }
                else
                {
                    foreach (var role in AcDomain.RoleSet)
                    {
                        if (!accountRoles.Any(a => a.ObjectInstanceId == role.Id))
                        {
                            data.Add(new AccountAssignRoleTr
                            {
                                AccountId = requestData.AccountId,
                                IsAssigned = false,
                                RoleId = role.Id,
                                CreateBy = null,
                                CreateOn = null,
                                CreateUserId = null,
                                Id = Guid.NewGuid(),
                                IsEnabled = role.IsEnabled,
                                CategoryCode = role.CategoryCode,
                                Name = role.Name,
                                SortCode = role.SortCode,
                                Icon = role.Icon
                            });
                        }
                    }
                }
            }
            else
            {
                foreach (var role in AcDomain.RoleSet)
                {
                    var ar = accountRoles.FirstOrDefault(a => a.ObjectInstanceId == role.Id);
                    if (ar == null)
                    {
                        data.Add(new AccountAssignRoleTr
                        {
                            AccountId = requestData.AccountId,
                            IsAssigned = false,
                            RoleId = role.Id,
                            CreateBy = null,
                            CreateOn = null,
                            CreateUserId = null,
                            Id = Guid.NewGuid(),
                            IsEnabled = role.IsEnabled,
                            CategoryCode = role.CategoryCode,
                            Name = role.Name,
                            SortCode = role.SortCode,
                            Icon = role.Icon
                        });
                    }
                    else
                    {
                        data.Add(new AccountAssignRoleTr
                        {
                            AccountId = requestData.AccountId,
                            IsAssigned = true,
                            RoleId = ar.ObjectInstanceId,
                            CreateBy = ar.CreateBy,
                            CreateOn = ar.CreateOn,
                            CreateUserId = ar.CreateUserId,
                            Id = ar.Id,
                            IsEnabled = role.IsEnabled,
                            CategoryCode = role.CategoryCode,
                            Name = role.Name,
                            SortCode = role.SortCode,
                            Icon = role.Icon
                        });
                    }
                }
            }
            int pageIndex = requestData.PageIndex;
            int pageSize = requestData.PageSize;
            var queryable = data.AsQueryable();
            if (!string.IsNullOrEmpty(requestData.Key))
            {
                queryable = queryable.Where(a => a.Name.Contains(requestData.Key) || a.CategoryCode.Contains(requestData.Key));
            }
            var list = queryable.OrderBy(requestData.SortField + " " + requestData.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);

            return this.JsonResult(new MiniGrid<AccountAssignRoleTr> { total = queryable.Count(), data = list });
        }
        #endregion

        #region GetPlistSsdSetRoles
        [By("xuexs")]
        [Description("根据静态职责分离角色集标识分页获取角色")]
        [Guid("5E2A0B43-8D37-4AA2-8FDD-B56880763608")]
        public ActionResult GetPlistSsdSetRoles(GetPlistSsdSetRoles requestData)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            SsdSetState ssdSet;
            if (!AcDomain.SsdSetSet.TryGetSsdSet(requestData.SsdSetId, out ssdSet))
            {
                throw new ValidationException("非法的静态职责分离角色集标识" + requestData.SsdSetId);
            }
            var data = new List<SsdSetAssignRoleTr>();
            var privilegeType = AcElementType.Role.ToName();
            var ssdSetRoles = AcDomain.SsdSetSet.GetSsdRoles(ssdSet);
            if (requestData.IsAssigned.HasValue)
            {
                if (requestData.IsAssigned.Value)
                {
                    foreach (var ar in ssdSetRoles)
                    {
                        RoleState role;
                        if (!AcDomain.RoleSet.TryGetRole(ar.RoleId, out role))
                        {
                            throw new AnycmdException("意外的角色标识" + ar.RoleId);
                        }
                        data.Add(new SsdSetAssignRoleTr
                        {
                            SsdSetId = requestData.SsdSetId,
                            IsAssigned = true,
                            RoleId = ar.RoleId,
                            Id = ar.Id,
                            IsEnabled = role.IsEnabled,
                            CategoryCode = role.CategoryCode,
                            Name = role.Name,
                            SortCode = role.SortCode,
                            Icon = role.Icon
                        });
                    }
                }
                else
                {
                    data.AddRange(from role in AcDomain.RoleSet
                                  where ssdSetRoles.All(a => a.RoleId != role.Id)
                                  select new SsdSetAssignRoleTr
                                  {
                                      SsdSetId = requestData.SsdSetId,
                                      IsAssigned = false,
                                      RoleId = role.Id,
                                      CreateOn = null,
                                      Id = Guid.NewGuid(),
                                      IsEnabled = role.IsEnabled,
                                      CategoryCode = role.CategoryCode,
                                      Name = role.Name,
                                      SortCode = role.SortCode,
                                      Icon = role.Icon
                                  });
                }
            }
            else
            {
                foreach (var role in AcDomain.RoleSet)
                {
                    var ar = ssdSetRoles.FirstOrDefault(a => a.RoleId == role.Id);
                    if (ar == null)
                    {
                        data.Add(new SsdSetAssignRoleTr
                        {
                            SsdSetId = requestData.SsdSetId,
                            IsAssigned = false,
                            RoleId = role.Id,
                            CreateOn = null,
                            Id = Guid.NewGuid(),
                            IsEnabled = role.IsEnabled,
                            CategoryCode = role.CategoryCode,
                            Name = role.Name,
                            SortCode = role.SortCode,
                            Icon = role.Icon
                        });
                    }
                    else
                    {
                        data.Add(new SsdSetAssignRoleTr
                        {
                            SsdSetId = requestData.SsdSetId,
                            IsAssigned = true,
                            RoleId = ar.RoleId,
                            CreateOn = ar.CreateOn,
                            Id = ar.Id,
                            IsEnabled = role.IsEnabled,
                            CategoryCode = role.CategoryCode,
                            Name = role.Name,
                            SortCode = role.SortCode,
                            Icon = role.Icon
                        });
                    }
                }
            }
            int pageIndex = requestData.PageIndex;
            int pageSize = requestData.PageSize;
            var queryable = data.AsQueryable();
            if (!string.IsNullOrEmpty(requestData.Key))
            {
                queryable = queryable.Where(a => a.Name.Contains(requestData.Key) || a.CategoryCode.Contains(requestData.Key));
            }
            var list = queryable.OrderBy(requestData.SortField + " " + requestData.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);

            return this.JsonResult(new MiniGrid<SsdSetAssignRoleTr> { total = queryable.Count(), data = list });
        }
        #endregion

        #region GetPlistDsdSetRoles
        [By("xuexs")]
        [Description("根据动态职责分离角色集标识分页获取角色")]
        [Guid("6A70E96B-4827-429E-A9F6-218506F563C0")]
        public ActionResult GetPlistDsdSetRoles(GetPlistDsdSetRoles requestData)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            DsdSetState dsdSet;
            if (!AcDomain.DsdSetSet.TryGetDsdSet(requestData.DsdSetId, out dsdSet))
            {
                throw new ValidationException("非法的动态职责分离角色集标识" + requestData.DsdSetId);
            }
            var data = new List<DsdSetAssignRoleTr>();
            var privilegeType = AcElementType.Role.ToName();
            var ssdSetRoles = AcDomain.DsdSetSet.GetDsdRoles(dsdSet);
            if (requestData.IsAssigned.HasValue)
            {
                if (requestData.IsAssigned.Value)
                {
                    foreach (var ar in ssdSetRoles)
                    {
                        RoleState role;
                        if (!AcDomain.RoleSet.TryGetRole(ar.RoleId, out role))
                        {
                            throw new AnycmdException("意外的角色标识" + ar.RoleId);
                        }
                        data.Add(new DsdSetAssignRoleTr
                        {
                            DsdSetId = requestData.DsdSetId,
                            IsAssigned = true,
                            RoleId = ar.RoleId,
                            Id = ar.Id,
                            IsEnabled = role.IsEnabled,
                            CategoryCode = role.CategoryCode,
                            Name = role.Name,
                            SortCode = role.SortCode,
                            Icon = role.Icon
                        });
                    }
                }
                else
                {
                    data.AddRange(from role in AcDomain.RoleSet
                                  where ssdSetRoles.All(a => a.RoleId != role.Id)
                                  select new DsdSetAssignRoleTr
                                  {
                                      DsdSetId = requestData.DsdSetId,
                                      IsAssigned = false,
                                      RoleId = role.Id,
                                      CreateOn = null,
                                      Id = Guid.NewGuid(),
                                      IsEnabled = role.IsEnabled,
                                      CategoryCode = role.CategoryCode,
                                      Name = role.Name,
                                      SortCode = role.SortCode,
                                      Icon = role.Icon
                                  });
                }
            }
            else
            {
                foreach (var role in AcDomain.RoleSet)
                {
                    var ar = ssdSetRoles.FirstOrDefault(a => a.RoleId == role.Id);
                    if (ar == null)
                    {
                        data.Add(new DsdSetAssignRoleTr
                        {
                            DsdSetId = requestData.DsdSetId,
                            IsAssigned = false,
                            RoleId = role.Id,
                            CreateOn = null,
                            Id = Guid.NewGuid(),
                            IsEnabled = role.IsEnabled,
                            CategoryCode = role.CategoryCode,
                            Name = role.Name,
                            SortCode = role.SortCode,
                            Icon = role.Icon
                        });
                    }
                    else
                    {
                        data.Add(new DsdSetAssignRoleTr
                        {
                            DsdSetId = requestData.DsdSetId,
                            IsAssigned = true,
                            RoleId = ar.RoleId,
                            CreateOn = ar.CreateOn,
                            Id = ar.Id,
                            IsEnabled = role.IsEnabled,
                            CategoryCode = role.CategoryCode,
                            Name = role.Name,
                            SortCode = role.SortCode,
                            Icon = role.Icon
                        });
                    }
                }
            }
            int pageIndex = requestData.PageIndex;
            int pageSize = requestData.PageSize;
            var queryable = data.AsQueryable();
            if (!string.IsNullOrEmpty(requestData.Key))
            {
                queryable = queryable.Where(a => a.Name.Contains(requestData.Key) || a.CategoryCode.Contains(requestData.Key));
            }
            var list = queryable.OrderBy(requestData.SortField + " " + requestData.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);

            return this.JsonResult(new MiniGrid<DsdSetAssignRoleTr> { total = queryable.Count(), data = list });
        }
        #endregion

        #region GetPlistGroupRoles
        [By("xuexs")]
        [Description("根据工作组ID分页获取角色")]
        [Guid("E3B216F9-690A-4BB4-A2E1-53681FEB72D9")]
        public ActionResult GetPlistGroupRoles(GetPlistGroupRoles requestData)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            var data = AcDomain.GetPlistGroupRoles(requestData);

            Debug.Assert(requestData.Total != null, "requestData.total != null");
            return this.JsonResult(new MiniGrid<GroupAssignRoleTr> { total = requestData.Total.Value, data = data });
        }
        #endregion

        [By("xuexs")]
        [Description("根据菜单ID分页获取角色")]
        [Guid("19BF32FD-23E5-462C-ACE9-7923FD46DB15")]
        public ActionResult GetPlistMenuRoles(GetPlistMenuRoles requestData)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            if (requestData.MenuId.HasValue && requestData.MenuId.Value != Guid.Empty)
            {
                var data = AcDomain.GetPlistMenuRoles(requestData);

                Debug.Assert(requestData.Total != null, "requestData.total != null");
                return this.JsonResult(new MiniGrid<MenuAssignRoleTr> { total = requestData.Total.Value, data = data });
            }
            else
            {
                return this.JsonResult(new MiniGrid<MenuAssignRoleTr> { total = 0, data = new List<MenuAssignRoleTr>() });
            }
        }

        [By("xuexs")]
        [Description("创建角色")]
        [HttpPost]
        [Guid("3F7D0CC8-7E50-4339-8A98-B87F54535025")]
        public ActionResult Create(RoleCreateInput input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ModelState.ToJsonResult();
            }
            AcDomain.Handle(new AddRoleCommand(input));

            return this.JsonResult(new ResponseData { success = true, id = input.Id });
        }

        [By("xuexs")]
        [Description("更新角色")]
        [HttpPost]
        [Guid("6BF67B1F-41BF-480C-972D-B79C72717945")]
        public ActionResult Update(RoleUpdateInput input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ModelState.ToJsonResult();
            }
            AcDomain.Handle(new UpdateRoleCommand(input));

            return this.JsonResult(new ResponseData { success = true, id = input.Id });
        }

        [By("xuexs")]
        [Description("删除角色")]
        [HttpPost]
        [Guid("4DBB9E8A-D8B3-41A4-AD32-9BAE2AA28FA6")]
        public ActionResult Delete(string id)
        {
            string[] ids = id.Split(',');
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
                    throw new ValidationException("意外的角色标识" + ids[i]);
                }
            }
            foreach (var item in idArray)
            {
                AcDomain.Handle(new RemoveRoleCommand(item));
            }

            return this.JsonResult(new ResponseData { id = id, success = true });
        }

        #region GrantOrDenyRoleFunctions
        [By("xuexs")]
        [Description("添加或禁用权限")]
        [HttpPost]
        [Guid("1911899E-F9C7-4722-AE1F-07F7B27BD8CA")]
        public ActionResult GrantOrDenyRoleFunctions()
        {
            String json = Request["data"];
            var rows = (ArrayList)MiniJSON.Decode(json);
            foreach (Hashtable row in rows)
            {
                var id = new Guid(row["Id"].ToString());
                //根据记录状态，进行不同的增加、删除、修改操作
                String state = row["_state"] != null ? row["_state"].ToString() : "";

                //更新：_state为空或modified
                if (state == "modified" || state == "")
                {
                    bool isAssigned = bool.Parse(row["IsAssigned"].ToString());
                    Privilege entity = GetRequiredService<IRepository<Privilege>>().GetByKey(id);
                    if (entity != null)
                    {
                        if (!isAssigned)
                        {
                            AcDomain.Handle(new RemovePrivilegeCommand(entity.Id));
                        }
                        else
                        {
                            if (row.ContainsKey("AcContent"))
                            {
                                AcDomain.Handle(new PrivilegeUpdateIo
                                {
                                    Id = entity.Id,
                                    AcContent = row["AcContent"] == null ? null : row["AcContent"].ToString()
                                }.ToCommand());
                            }
                        }
                    }
                    else if (isAssigned)
                    {

                        var createInput = new PrivilegeCreateIo()
                        {
                            Id = new Guid(row["Id"].ToString()),
                            SubjectType = UserAcSubjectType.Role.ToName(),
                            SubjectInstanceId = new Guid(row["RoleId"].ToString()),
                            ObjectInstanceId = new Guid(row["FunctionId"].ToString()),
                            ObjectType = AcElementType.Function.ToName(),
                            AcContent = null,
                            AcContentType = null
                        };
                        if (row.ContainsKey("AcContent"))
                        {
                            createInput.AcContent = row["AcContent"] == null ? null : row["AcContent"].ToString();
                        }
                        AcDomain.Handle(createInput.ToCommand());
                    }
                }
            }

            return this.JsonResult(new ResponseData { success = true });
        }
        #endregion

        #region AddOrRemoveMenus
        [By("xuexs")]
        [Description("往角色中添加或移除菜单")]
        [HttpPost]
        [Guid("4DD3A892-42D2-4422-A276-410510D10723")]
        public ActionResult AddOrRemoveMenus(Guid roleId, string addMenuIDs, string removeMenuIDs)
        {
            string[] addIDs = addMenuIDs.Split(',');
            string[] removeIDs = removeMenuIDs.Split(',');
            var subjectType = UserAcSubjectType.Role.ToName();
            var acObjectType = AcElementType.Menu.ToName();
            foreach (var item in addIDs)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    var mId = new Guid(item);
                    var entity = GetRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.SubjectType == subjectType && a.SubjectInstanceId == roleId && a.ObjectType == acObjectType && a.ObjectInstanceId == mId);
                    if (entity == null)
                    {
                        var createInput = new PrivilegeCreateIo
                        {
                            Id = Guid.NewGuid(),
                            SubjectType = subjectType,
                            SubjectInstanceId = roleId,
                            ObjectInstanceId = mId,
                            ObjectType = acObjectType,
                            AcContentType = null,
                            AcContent = null
                        };
                        AcDomain.Handle(createInput.ToCommand());
                    }
                }
            }
            foreach (var item in removeIDs)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    var mId = new Guid(item);
                    var entity = GetRequiredService<IRepository<Privilege>>().AsQueryable().FirstOrDefault(a => a.SubjectType == subjectType && a.SubjectInstanceId == roleId && a.ObjectType == acObjectType && a.ObjectInstanceId == mId);
                    if (entity != null)
                    {
                        AcDomain.Handle(new RemovePrivilegeCommand(entity.Id));
                    }
                }
            }

            return this.JsonResult(new ResponseData { success = true });
        }
        #endregion

        [By("xuexs")]
        [Description("添加角色成员账户")]
        [HttpPost]
        [Guid("98EE15B7-91C6-4003-81A9-A8464F33FB58")]
        public ActionResult AddRoleAccounts(string accountIDs, Guid roleId)
        {
            string[] aIds = accountIDs.Split(',');
            foreach (var item in aIds)
            {
                var accountId = new Guid(item);
                AcDomain.Handle(new PrivilegeCreateIo
                {
                    Id = Guid.NewGuid(),
                    ObjectInstanceId = roleId,
                    SubjectInstanceId = accountId,
                    SubjectType = UserAcSubjectType.Account.ToName(),
                    ObjectType = AcElementType.Role.ToName()
                }.ToCommand());
            }

            return this.JsonResult(new ResponseData { success = true, id = accountIDs });
        }

        [By("xuexs")]
        [Description("移除角色成员账户")]
        [HttpPost]
        [Guid("B69DA48F-1098-4B9C-BE04-02F154958C9D")]
        public ActionResult RemoveRoleAccounts(string id)
        {
            string[] ids = id.Split(',');
            foreach (var item in ids)
            {
                AcDomain.Handle(new RemovePrivilegeCommand(new Guid(item)));
            }

            return this.JsonResult(new ResponseData { success = true, id = id });
        }

        #region GrantOrDenyGroups
        [By("xuexs")]
        [Description("将角色授予或收回工作组")]
        [HttpPost]
        [Guid("0C4A561C-2C12-46CB-9264-9F766EA2F047")]
        public ActionResult GrantOrDenyGroups()
        {
            String json = Request["data"];
            var rows = (ArrayList)MiniJSON.Decode(json);
            foreach (Hashtable row in rows)
            {
                var id = new Guid(row["Id"].ToString());
                //根据记录状态，进行不同的增加、删除、修改操作
                String state = row["_state"] != null ? row["_state"].ToString() : "";

                //更新：_state为空或modified
                if (state == "modified" || state == "")
                {
                    bool isAssigned = bool.Parse(row["IsAssigned"].ToString());
                    var entity = GetRequiredService<IRepository<Privilege>>().GetByKey(id);
                    if (entity != null)
                    {
                        if (!isAssigned)
                        {
                            AcDomain.Handle(new RemovePrivilegeCommand(id));
                        }
                        else
                        {
                            if (row.ContainsKey("AcContent"))
                            {
                                AcDomain.Handle(new PrivilegeUpdateIo
                                {
                                    Id = id,
                                    AcContent = row["AcContent"].ToString()
                                }.ToCommand());
                            }
                        }
                    }
                    else if (isAssigned)
                    {
                        var createInput = new PrivilegeCreateIo()
                        {
                            Id = new Guid(row["Id"].ToString()),
                            ObjectInstanceId = new Guid(row["GroupId"].ToString()),
                            ObjectType = AcElementType.Group.ToName(),
                            SubjectType = UserAcSubjectType.Role.ToName(),
                            SubjectInstanceId = new Guid(row["RoleId"].ToString()),
                            AcContent = null,
                            AcContentType = null
                        };
                        if (row.ContainsKey("AcContent"))
                        {
                            createInput.AcContent = row["AcContent"].ToString();
                        }
                        AcDomain.Handle(createInput.ToCommand());
                    }
                }
            }

            return this.JsonResult(new ResponseData { success = true });
        }
        #endregion
    }
}
