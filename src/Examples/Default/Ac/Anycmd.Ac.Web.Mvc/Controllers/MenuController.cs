
namespace Anycmd.Ac.Web.Mvc.Controllers
{
    using Anycmd.Web.Mvc;
    using Engine.Ac;
    using Engine.Ac.Abstractions;
    using Engine.Host.Ac;
    using Engine.Host.Ac.Infra.Messages;
    using Engine.Host.Ac.InOuts;
    using Engine.Host.Ac.Messages;
    using Exceptions;
    using MiniUI;
    using Repositories;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Web.Mvc;
    using Util;
    using ViewModel;
    using ViewModels.Infra.MenuViewModels;

    /// <summary>
    /// 系统菜单模型视图控制器
    /// </summary>
    [Guid("2F6E0B9E-F751-44A7-9A30-28085CB44BEF")]
    public class MenuController : AnycmdController
    {
        private readonly EntityTypeState _menuEntityType;

        public MenuController()
        {
            if (!Host.EntityTypeSet.TryGetEntityType("Ac", "Menu", out _menuEntityType))
            {
                throw new CoreException("意外的实体类型");
            }
        }

        #region ViewResults
        [By("xuexs")]
        [Description("菜单管理")]
        [Guid("D6268FD0-C0E1-4173-96A9-5A8D11C9AF9C")]
        public ViewResultBase Index()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("菜单详细信息")]
        [Guid("BF2C89DB-0FC6-413F-B3CC-D94629662B4F")]
        public ViewResultBase Details()
        {
            if (!string.IsNullOrEmpty(Request["isTooltip"]))
            {
                Guid id;
                if (Guid.TryParse(Request["id"], out id))
                {
                    var data = _menuEntityType.GetData(id);
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
        [Description("子菜单列表")]
        [Guid("688D99C4-0C96-4EEF-8A2E-289B258B3E50")]
        public ViewResultBase Children()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("拥有该菜单的角色列表")]
        [Guid("ADA6EB41-EF1C-4634-8BEF-B95CFC219CAD")]
        public ViewResultBase Roles()
        {
            return ViewResult();
        }

        #endregion

        [By("xuexs")]
        [Description("根据ID获取菜单")]
        [Guid("C0C31A5C-AA17-4FCF-A465-3330B933D966")]
        public ActionResult Get(Guid? id)
        {
            if (id == Guid.Empty)
            {
                id = null;
            }
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(_menuEntityType.GetData(id.Value));
        }

        [By("xuexs")]
        [Description("根据ID获取菜单详细信息")]
        [Guid("80956D0C-8062-4702-BF96-349A3A119828")]
        public ActionResult GetInfo(Guid? id)
        {
            if (id == Guid.Empty)
            {
                id = null;
            }
            if (id.HasValue)
            {
                return this.JsonResult(_menuEntityType.GetData(id.Value));
            }
            else
            {
                return this.JsonResult(new Dictionary<string, Object> {
                    {"CreateBy", string.Empty},
                    {"CreateOn", DateTime.MinValue},
                    {"CreateUserId", Guid.Empty},
                    {"Description", string.Empty},
                    {"Icon", string.Empty},
                    {"ModifiedBy", string.Empty},
                    {"ModifiedOn", DateTime.MinValue},
                    {"ModifiedUserId", Guid.Empty},
                    {"Name", string.Empty},
                    {"ParentId", Guid.Empty},
                    {"ParentName", string.Empty},
                    {"Url", string.Empty}
                });
            }
        }

        #region GetNodesByParentId
        [By("xuexs")]
        [Description("根据父菜单ID获取子菜单")]
        [Guid("EFF929C2-0C43-4BA5-8853-B9D4C3828F59")]
        public ActionResult GetNodesByParentId(Guid? parentId, string rootNodeName)
        {
            if (parentId == Guid.Empty)
            {
                parentId = null;
            }
            var nodes = Host.MenuSet.Where(a => a.ParentId == parentId).Select(a => MenuMiniNode.Create(Host, a)).ToList();
            if (string.IsNullOrEmpty(rootNodeName))
            {
                rootNodeName = "全部";
            }
            if (!parentId.HasValue)
            {
                var rootNode = new MenuMiniNode(Host)
                {
                    Id = Guid.Empty,
                    Name = rootNodeName,
                    ParentId = null,
                    expanded = true
                };
                foreach (var node in nodes)
                {
                    if (node.ParentId == null)
                    {
                        node.ParentId = rootNode.Id;
                    }
                }
                nodes.Add(rootNode);
            }

            return this.JsonResult(nodes);
        }
        #endregion

        #region GetNodesByRoleID
        [By("xuexs")]
        [Description("根据角色ID和父菜单ID获取子菜单")]
        [Guid("084244DD-351E-4267-A9C7-C42B120B1091")]
        public ActionResult GetNodesByRoleId(Guid roleId)
        {
            RoleState role;
            if (!Host.RoleSet.TryGetRole(roleId, out role))
            {
                throw new ValidationException("意外的角色标识" + roleId);
            }
            var roleMenus = Host.PrivilegeSet.Where(a => a.SubjectType == AcSubjectType.Role && a.ObjectType == AcObjectType.Menu && a.SubjectInstanceId == roleId);
            var menus = Host.MenuSet;
            var data = (from m in menus
                        let @checked = roleMenus.Any(a => a.ObjectInstanceId == m.Id)
                        let isLeaf = menus.All(a => a.ParentId != m.Id)
                        select new
                        {
                            @checked = @checked,
                            expanded = @checked,// 如果选中则展开
                            m.Id,
                            isLeaf = isLeaf,
                            MenuId = m.Id,
                            m.Name,
                            ParentId = m.ParentId,
                            RoleId = roleId,
                            img = m.Icon
                        });


            return this.JsonResult(data);
        }
        #endregion

        [By("xuexs")]
        [Description("根据父菜单ID获取子菜单")]
        [Guid("DF900507-F60E-4789-99CD-F65C9E8E8AB2")]
        public ActionResult GetPlistMenuChildren(GetPlistMenuChildren requestModel)
        {
            if (requestModel.ParentId == Guid.Empty)
            {
                requestModel.ParentId = null;
            }
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            foreach (var filter in requestModel.Filters)
            {
                PropertyState property;
                if (!_menuEntityType.TryGetProperty(filter.field, out property))
                {
                    throw new ValidationException("意外的Menu实体类型属性" + filter.field);
                }
            }
            int pageIndex = requestModel.PageIndex;
            int pageSize = requestModel.PageSize;
            var queryable = Host.MenuSet.Where(a => a.ParentId == requestModel.ParentId).Select(MenuTr.Create).AsQueryable();
            foreach (var filter in requestModel.Filters)
            {
                queryable = queryable.Where(filter.ToPredicate(), filter.value);
            }
            var list = queryable.OrderBy(requestModel.SortField + " " + requestModel.SortOrder).Skip(pageIndex * pageSize).Take(pageSize);

            return this.JsonResult(new MiniGrid<MenuTr> { total = queryable.Count(), data = list });
        }

        [By("xuexs")]
        [Description("添加菜单")]
        [HttpPost]
        [Guid("9488A6F4-AEBA-4423-BA35-B04D2A5D1185")]
        public ActionResult Create(MenuCreateInput input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ModelState.ToJsonResult();
            }
            if (input.ParentId == Guid.Empty) 
            {
                input.ParentId = null;
            }
            Host.Handle(new AddMenuCommand(input));

            return this.JsonResult(new ResponseData { success = true, id = input.Id });
        }

        [By("xuexs")]
        [Description("更新菜单")]
        [HttpPost]
        [Guid("A18A79B5-BF3F-43F7-8A16-5EED11F97D01")]
        public ActionResult Update(MenuUpdateInput input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ModelState.ToJsonResult();
            }
            Host.Handle(new UpdateMenuCommand(input));

            return this.JsonResult(new ResponseData { success = true, id = input.Id });
        }

        #region GrantOrDenyRoles
        [By("xuexs")]
        [Description("将菜单授予或收回角色")]
        [HttpPost]
        [Guid("AEE9AB01-1E84-48B2-8C61-49A11CF91F83")]
        public ActionResult GrantOrDenyRoles()
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
                    var entity = GetRequiredService<IRepository<PrivilegeBigram>>().GetByKey(id);
                    if (entity != null)
                    {
                        if (!isAssigned)
                        {
                            Host.Handle(new RemovePrivilegeBigramCommand(id));
                        }
                        else
                        {
                            if (row.ContainsKey("PrivilegeConstraint"))
                            {
                                Host.Handle(new UpdatePrivilegeBigramCommand(new PrivilegeBigramUpdateIo
                                {
                                    Id = entity.Id,
                                    PrivilegeConstraint = row["PrivilegeConstraint"].ToString()
                                }));
                            }
                        }
                    }
                    else if (isAssigned)
                    {
                        var createInput = new PrivilegeBigramCreateIo
                        {
                            Id = new Guid(row["Id"].ToString()),
                            SubjectType = AcSubjectType.Role.ToName(),
                            SubjectInstanceId = new Guid(row["RoleId"].ToString()),
                            ObjectInstanceId = new Guid(row["MenuId"].ToString()),
                            ObjectType = AcObjectType.Menu.ToName(),
                            PrivilegeOrientation = 1,
                            PrivilegeConstraint = null
                        };
                        if (row.ContainsKey("PrivilegeConstraint"))
                        {
                            createInput.PrivilegeConstraint = row["PrivilegeConstraint"].ToString();
                        }
                        Host.Handle(new AddPrivilegeBigramCommand(createInput));
                    }
                }
            }

            return this.JsonResult(new ResponseData { success = true });
        }
        #endregion

        [By("xuexs")]
        [Description("删除菜单")]
        [HttpPost]
        [Guid("1FD652F8-A483-4E7C-8883-621CFAE559D4")]
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
                    throw new ValidationException("意外的菜单标识" + ids[i]);
                }
            }
            foreach (var item in idArray)
            {
                Host.Handle(new RemoveMenuCommand(item));
            }

            return this.JsonResult(new ResponseData { id = id, success = true });
        }
    }
}