
namespace Anycmd.Ac.Web.Mvc.Controllers
{
    using Anycmd.Web.Mvc;
    using Engine.Ac;
    using Engine.Ac.Abstractions;
    using Engine.Ac.InOuts;
    using Engine.Ac.Messages;
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
    using ViewModels.GroupViewModels;

    /// <summary>
    /// 工作组模型视图控制器
    /// </summary>
    [Guid("BBDDB497-8892-4FC9-9B18-667CAD500DC4")]
    public class GroupController : AnycmdController
    {
        #region ViewResults
        [By("xuexs")]
        [Description("工作组管理")]
        [Guid("A16841EF-F88C-4F94-975C-152109BB0BE8")]
        public ViewResultBase Index()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("工作组详细信息")]
        [Guid("32DB0B4A-6936-4439-B1A1-7ECF93FE1F12")]
        public ViewResultBase Details()
        {
            if (!string.IsNullOrEmpty(Request["isTooltip"]))
            {
                Guid id;
                if (Guid.TryParse(Request["id"], out id))
                {
                    var dic = base.EntityType.GetData(id);
                    if (dic == null)
                    {
                        throw new NotExistException("给定标识的记录不存在");
                    }
                    if (!dic.ContainsKey("IsEnabledName"))
                    {
                        dic.Add("IsEnabledName", Host.Translate("Ac", "Group", "IsEnabledName", dic["IsEnabled"].ToString()));
                    }
                    return new PartialViewResult { ViewName = "Partials/Details", ViewData = new ViewDataDictionary(dic) };
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
        [Description("工作组拥有的角色列表")]
        [Guid("8CF0E4D7-4A84-4F0A-AB21-D53400128399")]
        public ViewResultBase Roles()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("工作组内的账户列表")]
        [Guid("ADC7D024-48BD-4BE1-9BBE-7DF5A2D06238")]
        public ViewResultBase Accounts()
        {
            return ViewResult();
        }

        #endregion

        [By("xuexs")]
        [Description("根据ID获取工作组")]
        [Guid("5C35481A-219B-4EC2-B519-74FFDC6A8611")]
        public ActionResult Get(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(base.EntityType.GetData(id.Value));
        }

        [By("xuexs")]
        [Description("根据ID获取工作组详细信息")]
        [Guid("49D5C476-BBCA-4711-996E-B00754D03E33")]
        public ActionResult GetInfo(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            var dic = base.EntityType.GetData(id.Value);
            if (dic == null)
            {
                throw new NotExistException("给定标识的记录不存在");
            }
            if (!dic.ContainsKey("IsEnabledName"))
            {
                dic.Add("IsEnabledName", Host.Translate("Ac", "Group", "IsEnabledName", dic["IsEnabled"].ToString()));
            }
            return this.JsonResult(dic);
        }

        [By("xuexs")]
        [Description("分页获取工作组")]
        [Guid("897CD4C5-1762-41E1-8803-D0710FA7362B")]
        public ActionResult GetPlistGroups(GetPlistResult requestModel)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            var data = Host.GetPlistGroups(requestModel);

            Debug.Assert(requestModel.Total != null, "requestModel.total != null");
            return this.JsonResult(new MiniGrid { total = requestModel.Total.Value, data = data.Select(a => a.ToTableRowData()) });
        }

        #region GetPlistAccountGroups
        [By("xuexs")]
        [Description("根据账户ID分页获取工作组")]
        [Guid("2B071D62-4430-4290-AA5C-6639A490B0EC")]
        public ActionResult GetPlistAccountGroups(string key, Guid accountId, bool? isAssigned, int? pageIndex, int? pageSize, string sortField, string sortOrder)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            var data = new List<Dictionary<string, object>>();
            var privilegeType = AcObjectType.Group.ToName();
            var accountGroups = GetRequiredService<IRepository<PrivilegeBigram>>().AsQueryable().Where(a => a.SubjectInstanceId == accountId && a.ObjectType == privilegeType);
            var groups = Host.GroupSet.AsQueryable();
            if (!string.IsNullOrEmpty(key))
            {
                groups = groups.Where(a => a.Name.Contains(key) || a.CategoryCode.Contains(key));
            }
            pageIndex = pageIndex ?? 0;
            pageSize = pageSize ?? 10;
            foreach (var group in groups.OrderBy(sortField + " " + sortOrder).Skip(pageIndex.Value * pageSize.Value).Take(pageSize.Value))
            {
                var accountGroup = accountGroups.FirstOrDefault(a => a.ObjectInstanceId == group.Id);
                if (isAssigned.HasValue)
                {
                    if (isAssigned.Value)
                    {
                        if (accountGroup == null)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (accountGroup != null)
                        {
                            continue;
                        }
                    }
                }
                string createBy = null;
                DateTime? createOn = null;
                Guid? createUserId = null;
                Guid id;
                if (accountGroup != null)
                {
                    createBy = accountGroup.CreateBy;
                    createOn = accountGroup.CreateOn;
                    createUserId = accountGroup.CreateUserId;
                    id = accountGroup.Id;
                    isAssigned = true;
                }
                else
                {
                    id = Guid.NewGuid();
                    isAssigned = false;
                }
                data.Add(new Dictionary<string, object>
                {
                    {"CategoryCode", group.CategoryCode},
                    {"CreateBy", createBy},
                    {"CreateOn", createOn},
                    {"CreateUserId", createUserId},
                    {"GroupId", group.Id},
                    {"Id", id},
                    {"IsAssigned", isAssigned},
                    {"IsEnabled", group.IsEnabled},
                    {"Name", group.Name},
                    {"SortCode", group.SortCode},
                    {"AccountId", accountId}
                });
            }

            return this.JsonResult(new MiniGrid { total = groups.Count(), data = data });
        }
        #endregion

        #region GetPlistRoleGroups
        [By("xuexs")]
        [Description("根据角色ID分页获取工作组")]
        [Guid("8CB3CAFC-5AB2-4E28-9B55-E592E3DE4FE5")]
        public ActionResult GetPlistRoleGroups(GetPlistRoleGroups requestData)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            var data = Host.GetPlistRoleGroups(requestData);

            Debug.Assert(requestData.Total != null, "requestData.total != null");
            return this.JsonResult(new MiniGrid<Dictionary<string, object>> { total = requestData.Total.Value, data = data });
        }
        #endregion

        [By("xuexs")]
        [Description("添加工作组")]
        [HttpPost]
        [Guid("C53E271A-CF43-4A29-B4CE-8DCA91D24EC9")]
        public ActionResult Create(GroupCreateInput input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ModelState.ToJsonResult();
            }
            if (!"Ac".Equals(input.TypeCode, StringComparison.OrdinalIgnoreCase))
            {
                throw new ValidationException("非法的操作，试图越权。");
            }
            Host.Handle(new AddGroupCommand(input));

            return this.JsonResult(new ResponseData { success = true, id = input.Id });
        }

        [By("xuexs")]
        [Description("更新工作组")]
        [HttpPost]
        [Guid("2935AFF9-BFC9-4978-A3BE-3865ADC6EB8C")]
        public ActionResult Update(GroupUpdateInput input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ModelState.ToJsonResult();
            }
            Host.Handle(new UpdateGroupCommand(input));

            return this.JsonResult(new ResponseData { success = true, id = input.Id });
        }

        [By("xuexs")]
        [Description("删除工作组")]
        [HttpPost]
        [Guid("B5328390-63FB-4071-A522-75C432276A7F")]
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
                    throw new ValidationException("意外的字典项标识" + ids[i]);
                }
            }
            foreach (var item in idArray)
            {
                Host.Handle(new RemoveGroupCommand(item));
            }

            return this.JsonResult(new ResponseData { id = id, success = true });
        }

        [By("xuexs")]
        [Description("为工作组授予或收回角色")]
        [HttpPost]
        [Guid("00625DF3-38A1-4E30-8840-ABC7FBC14C7D")]
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
                            Host.Handle(new RemovePrivilegeBigramCommand(entity.Id));
                        }
                        else
                        {
                            if (row.ContainsKey("PrivilegeConstraint"))
                            {
                                Host.Handle(new UpdatePrivilegeBigramCommand(new PrivilegeBigramUpdateIo
                                {
                                    Id = id,
                                    PrivilegeConstraint = row["PrivilegeConstraint"].ToString()
                                }));
                            }
                        }
                    }
                    else if (isAssigned)
                    {
                        var createInput = new PrivilegeBigramCreateIo()
                        {
                            Id = new Guid(row["Id"].ToString()),
                            SubjectType = AcSubjectType.Role.ToName(),
                            SubjectInstanceId = new Guid(row["RoleId"].ToString()),
                            ObjectInstanceId = new Guid(row["GroupId"].ToString()),
                            ObjectType = AcObjectType.Group.ToName(),
                            PrivilegeConstraint = null,
                            PrivilegeOrientation = 1
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

        [By("xuexs")]
        [Description("添加工作组成员账户")]
        [HttpPost]
        [Guid("0CEAF1FB-246D-4438-96E6-2B84E67410D6")]
        public ActionResult AddGroupAccounts(string accountIDs, Guid groupId)
        {
            string[] aIds = accountIDs.Split(',');
            foreach (var item in aIds)
            {
                var accountId = new Guid(item);
                Host.Handle(new AddPrivilegeBigramCommand(new PrivilegeBigramCreateIo
                {
                    Id = Guid.NewGuid(),
                    ObjectType = AcObjectType.Group.ToName(),
                    SubjectType = AcSubjectType.Account.ToName(),
                    ObjectInstanceId = groupId,
                    SubjectInstanceId = accountId
                }));
            }

            return this.JsonResult(new ResponseData { success = true, id = accountIDs });
        }

        [By("xuexs")]
        [Description("移除工作组成员账户")]
        [HttpPost]
        [Guid("1EEE3ADA-1B9A-478F-B023-FCEBFFB8E576")]
        public ActionResult RemoveGroupAccounts(string id)
        {
            string[] ids = id.Split(',');
            foreach (var item in ids)
            {
                Host.Handle(new RemovePrivilegeBigramCommand(new Guid(item)));
            }

            return this.JsonResult(new ResponseData { success = true, id = id });
        }
    }
}
