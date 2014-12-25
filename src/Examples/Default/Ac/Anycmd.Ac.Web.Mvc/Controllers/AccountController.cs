
using System.Diagnostics;

namespace Anycmd.Ac.Web.Mvc.Controllers
{
    using Anycmd.Web.Mvc;
    using Engine.Ac;
    using Engine.Ac.Abstractions;
    using Engine.Ac.Abstractions.Infra;
    using Engine.Host.Ac;
    using Engine.Host.Ac.Identity;
    using Engine.Host.Ac.InOuts;
    using Exceptions;
    using Extensions;
    using MiniUI;
    using Model;
    using Repositories;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Web.Mvc;
    using Util;
    using ViewModel;
    using ViewModels.Identity.AccountViewModels;
    using ViewModels.PrivilegeViewModels;

    /// <summary>
    /// 账户模型视图控制器<see cref="Account"/>
    /// </summary>
    [Guid("CECA85AD-6D77-49F5-AAE2-26E89B445B02")]
    public class AccountController : AnycmdController
    {
        private readonly EntityTypeState _accountEntityType;

        public AccountController()
        {
            if (!Host.EntityTypeSet.TryGetEntityType("Ac", "Account", out _accountEntityType))
            {
                throw new CoreException("意外的实体类型");
            }
        }

        #region Views
        [By("xuexs")]
        [Description("账户")]
        [Guid("176BCCC1-AD10-4256-99E8-5FF0290AA847")]
        public ViewResultBase Index()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("当前登录账户信息")]
        [Guid("54C6738B-B84B-4232-9D6F-C17C2C2660C5")]
        public ViewResultBase CurrentAccount(string isTooltip)
        {
            if (!string.IsNullOrEmpty(isTooltip))
            {
                var data = GetRequiredService<IAccountQuery>().Get("AccountInfo", Host.UserSession.Account.Id);

                return this.PartialView("Partials/Details", data);
            }
            else
            {
                return this.PartialView("Partials/Details");
            }
        }

        [By("xuexs")]
        [Description("账户详细信息")]
        [Guid("D1274ABD-F180-4C3F-A781-033BC54499EE")]
        public ViewResultBase Details()
        {
            return this.DetailsResult(GetRequiredService<IAccountQuery>(), "AccountInfo");
        }

        [By("xuexs")]
        [Description("账户拥有的角色列表")]
        [Guid("464123A4-FE7F-4275-B5D2-8E12BBB8C9B4")]
        public ViewResultBase Roles()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("账户所在的工作组列表")]
        [Guid("7E14F010-C827-410C-B078-CB67DB68208B")]
        public ViewResultBase Groups()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("账户所在的组织结构")]
        [Guid("50F16224-B924-4FFD-9B1A-8D5CFBE28665")]
        public ViewResultBase Organizations()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("包工头列表")]
        [Guid("46C8FE4C-896E-4FB3-8FCF-733B973E2F81")]
        public ViewResultBase Contractors()
        {
            return ViewResult();
        }
        #endregion

        #region GetAccountInfo
        [By("xuexs")]
        [Description("获取登录信息")]
        [Guid("F3236A82-DA72-4009-90CC-F0460E3479DE")]
        public ActionResult GetAccountInfo()
        {
            if (Host.UserSession.Principal.Identity.IsAuthenticated)
            {
                var account = Host.UserSession.Account;
                var menuList = Host.UserSession.AccountPrivilege.AuthorizedMenus.Cast<IMenu>().ToList();
                var menus = menuList.Select(m => new
                {
                    id = m.Id,
                    text = m.Name,
                    pid = m.ParentId,
                    url = m.Url,
                    img = m.Icon
                });
                return this.JsonResult(new
                {
                    isLogined = Host.UserSession.Principal.Identity.IsAuthenticated,
                    loginName = Host.UserSession.IsDeveloper() ? string.Format("{0}(开发人员)", account.LoginName) : account.LoginName,
                    wallpaper = Host.UserSession.GetData<string>("CurrentUser_Wallpaper") ?? string.Empty,
                    backColor = Host.UserSession.GetData<string>("CurrentUser_BackColor") ?? string.Empty,
                    menus = menus,
                    roles = Host.UserSession.AccountPrivilege.Roles,
                    groups = Host.UserSession.AccountPrivilege.Groups
                });
            }
            else
            {
                return this.JsonResult(new ResponseData { success = false, msg = "对不起，您没有登录" }.Error());
            }
        }
        #endregion

        [By("xuexs")]
        [Description("登录")]
        [HttpPost]
        [IgnoreAuth]
        [Guid("C8533933-6AA7-4EFE-B238-E0AFCCC3B761")]
        public ActionResult SignIn(string loginName, string password, string rememberMe)
        {
            Host.SignIn(new Dictionary<string, object>
            {
                {"loginName", loginName},
                {"password", password},
                {"rememberMe", rememberMe}
            });

            return this.JsonResult(new ResponseData { success = true });
        }

        [By("xuexs")]
        [Description("登出")]
        [HttpPost]
        [Guid("E0F71820-7225-4CE6-B6FF-68D33EE47CB9")]
        public ActionResult SignOut()
        {
            Host.SignOut();

            return this.JsonResult(new ResponseData { success = true });
        }

        [By("xuexs")]
        [Description("修改指定账户的密码")]
        [HttpPost]
        [Guid("0E463625-47BC-470E-BDF4-18ABD2B54A62")]
        public ActionResult AssignPassword(PasswordAssignInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            Host.AssignPassword(input);

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        [By("xuexs")]
        [Description("修改自己的密码")]
        [HttpPost]
        [Guid("0BC8759E-1D38-44C1-8991-84A33BAA56C8")]
        public ActionResult ChangeSelfPassword(string oldPassword, string password, string passwordAgain)
        {
            if (password != passwordAgain)
            {
                throw new ValidationException("两次输入的密码不一致");
            }
            Host.ChangePassword(new PasswordChangeInput
            {
                LoginName = Host.UserSession.Principal.Identity.Name,
                OldPassword = oldPassword,
                NewPassword = password
            });

            return this.JsonResult(new ResponseData { success = true });
        }

        [By("xuexs")]
        [Description("根据ID获取账户")]
        [Guid("6C84BB7F-744D-42A4-A043-C6CD48F39B4D")]
        public ActionResult Get(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(_accountEntityType.GetData(id.Value));
        }

        [By("xuexs")]
        [Description("根据ID获取账户详细")]
        [Guid("20B33E6E-8CA0-4B8A-BC2C-012E085C295A")]
        public ActionResult GetInfo(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(GetRequiredService<IAccountQuery>().Get("AccountInfo", id.Value));
        }

        #region GetPlistAccounts
        [By("xuexs")]
        [Description("根据用户分页获取账户")]
        [Guid("C891E24F-D124-449D-B0FC-3AAB7479C55B")]
        public ActionResult GetPlistAccounts(GetPlistAccounts requestModel)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            EntityTypeState entityType;
            if (!Host.EntityTypeSet.TryGetEntityType("Ac", "Account", out entityType))
            {
                throw new CoreException("意外的实体类型Ac.Account");
            }
            foreach (var filter in requestModel.Filters)
            {
                PropertyState property;
                if (!Host.EntityTypeSet.TryGetProperty(entityType, filter.field, out property))
                {
                    throw new ValidationException("意外的Account实体类型属性" + filter.field);
                }
            }
            requestModel.IncludeDescendants = requestModel.IncludeDescendants ?? false;
            List<DicReader> userAccountTrs = null;
            // 如果组织机构为空则需要检测是否是开发人员，因为只有开发人员才可以看到全部用户。组织结构为空表示查询全部组织结构。
            if (string.IsNullOrEmpty(requestModel.OrganizationCode))
            {
                if (!Host.UserSession.IsDeveloper())
                {
                    throw new ValidationException("对不起，您没有查看全部账户的权限");
                }
                else
                {
                    userAccountTrs = GetRequiredService<IAccountQuery>().GetPlistAccountTrs(requestModel.Filters, requestModel.OrganizationCode
                , requestModel.IncludeDescendants.Value, requestModel);
                }
            }
            else
            {
                userAccountTrs = GetRequiredService<IAccountQuery>().GetPlistAccountTrs(requestModel.Filters, requestModel.OrganizationCode
                , requestModel.IncludeDescendants.Value, requestModel);
            }
            Debug.Assert(requestModel.Total != null, "requestModel.total != null");
            var data = new MiniGrid<Dictionary<string, object>> { total = requestModel.Total.Value, data = userAccountTrs };

            return this.JsonResult(data);
        }
        #endregion

        [By("xuexs")]
        [Description("根据工作组ID分页获取账户")]
        [Guid("E81C2463-612F-4C5E-9876-CF221F572F10")]
        public ActionResult GetPlistGroupAccounts(GetPlistGroupAccounts requestModel)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            Debug.Assert(requestModel.GroupId != null, "requestModel.GroupId != null");
            var groupUserAccountTrs = GetRequiredService<IAccountQuery>().GetPlistGroupAccountTrs(
                requestModel.Key, requestModel.GroupId.Value, requestModel);
            Debug.Assert(requestModel.Total != null, "requestModel.total != null");
            var data = new MiniGrid<Dictionary<string, object>> { total = requestModel.Total.Value, data = groupUserAccountTrs };

            return this.JsonResult(data);
        }

        [By("xuexs")]
        [Description("根据角色ID分页获取账户")]
        [Guid("073C1CFC-EAB3-4BE9-AB2F-57A13E9E0F91")]
        public ActionResult GetPlistRoleAccounts(GetPlistRoleAccounts requestModel)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            Debug.Assert(requestModel.RoleId != null, "requestModel.RoleId != null");
            var roleUserAccountTrs = GetRequiredService<IAccountQuery>().GetPlistRoleAccountTrs(
                requestModel.Key, requestModel.RoleId.Value, requestModel);
            Debug.Assert(requestModel.Total != null, "requestModel.total != null");
            var data = new MiniGrid<Dictionary<string, object>> { total = requestModel.Total.Value, data = roleUserAccountTrs };

            return this.JsonResult(data);
        }

        [By("xuexs")]
        [Description("添加账户")]
        [HttpPost]
        [Guid("075125A2-FDBD-4ED4-A1BB-4B28C225F180")]
        public ActionResult Create(AccountCreateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            Host.AddAccount(input);

            return this.JsonResult(new ResponseData { success = true, id = input.Id });
        }

        [By("xuexs")]
        [Description("更新账户")]
        [HttpPost]
        [Guid("421147B1-D8EC-4244-9EFE-F6066C6A3229")]
        public ActionResult Update(AccountUpdateInput requestModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ModelState.ToJsonResult();
            }
            Host.UpdateAccount(requestModel);

            return this.JsonResult(new ResponseData { success = true, id = requestModel.Id });
        }

        [By("xuexs")]
        [Description("启用账户")]
        [HttpPost]
        [Guid("07F3055C-A6DC-4C41-89D2-5C90C7766456")]
        public ActionResult EnableAccount(string id)
        {
            string[] ids = id.Split(',');
            foreach (var item in ids)
            {
                var entity = GetRequiredService<IRepository<Account>>().GetByKey(new Guid(item));
                if (entity == null)
                {
                    throw new NotExistException(item);
                }
                entity.IsEnabled = 1;
                GetRequiredService<IRepository<Account>>().Context.Commit();
            }

            return this.JsonResult(new ResponseData { success = true, id = id });
        }

        [By("xuexs")]
        [Description("禁用账户")]
        [HttpPost]
        [Guid("F01F2562-298B-41B9-A620-A4A00E48F057")]
        public ActionResult DisableAccount(string id)
        {
            string[] ids = id.Split(',');
            foreach (var item in ids)
            {
                var entity = GetRequiredService<IRepository<Account>>().GetByKey(new Guid(item));
                if (entity == null)
                {
                    throw new NotExistException(item);
                }
                entity.IsEnabled = 0;
                GetRequiredService<IRepository<Account>>().Context.Commit();
            }

            return this.JsonResult(new ResponseData { success = true, id = id });
        }

        [By("xuexs")]
        [Description("删除账户")]
        [HttpPost]
        [Guid("AF3793B3-9CBB-4F53-9307-1910F9473058")]
        public ActionResult Delete(string id)
        {
            return this.HandleSeparateGuidString(Host.RemoveAccount, id, ',');
        }

        #region GrantOrDenyRoles
        [By("xuexs")]
        [Description("授予或收回角色")]
        [HttpPost]
        [Guid("C557EF15-23FB-4509-82CE-8F9E2995FA25")]
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
                            Host.RemovePrivilegeBigram(id);
                        }
                        else
                        {
                            if (row.ContainsKey("PrivilegeConstraint"))
                            {
                                Host.UpdatePrivilegeBigram(new PrivilegeBigramUpdateIo
                                {
                                    Id = id,
                                    PrivilegeConstraint = row["PrivilegeConstraint"].ToString()
                                });
                            }
                        }
                    }
                    else if (isAssigned)
                    {
                        var createInput = new PrivilegeBigramCreateIo
                        {
                            Id = new Guid(row["Id"].ToString()),
                            ObjectType = AcObjectType.Role.ToName(),
                            ObjectInstanceId = new Guid(row["RoleId"].ToString()),
                            SubjectInstanceId = new Guid(row["AccountId"].ToString()),
                            SubjectType = AcSubjectType.Account.ToName(),
                            PrivilegeConstraint = null,
                            PrivilegeOrientation = 1
                        };
                        if (row.ContainsKey("PrivilegeConstraint"))
                        {
                            createInput.PrivilegeConstraint = row["PrivilegeConstraint"].ToString();
                        }
                        Host.AddPrivilegeBigram(createInput);
                    }
                }
            }

            return this.JsonResult(new ResponseData { success = true });
        }
        #endregion

        #region JoinOrLeaveGroups
        [By("xuexs")]
        [Description("加入或脱离工作组")]
        [HttpPost]
        [Guid("BA4C4625-379A-41E5-B49C-DA9795E6650A")]
        public ActionResult JoinOrLeaveGroups()
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
                            Host.RemovePrivilegeBigram(id);
                        }
                    }
                    else if (isAssigned)
                    {
                        Host.AddPrivilegeBigram(new PrivilegeBigramCreateIo
                        {
                            Id = id,
                            ObjectType = AcObjectType.Group.ToName(),
                            ObjectInstanceId = new Guid(row["GroupId"].ToString()),
                            SubjectInstanceId = new Guid(row["AccountId"].ToString()),
                            SubjectType = AcSubjectType.Account.ToName()
                        });
                    }
                }
            }

            return this.JsonResult(new ResponseData { success = true });
        }
        #endregion

        [By("xuexs")]
        [Description("根据组织结构ID分页获取数据集管理员")]
        [Guid("B4183494-8E10-484D-A536-0905A3EB37BE")]
        public ActionResult GetPlistAccountOrganizationPrivileges(GetPlistAccountOrganizationPrivileges requestData)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            requestData.IncludeDescendants = requestData.IncludeDescendants ?? false;
            List<DicReader> data;
            if (string.IsNullOrEmpty(requestData.OrganizationCode))
            {
                if (!Host.UserSession.IsDeveloper())
                {
                    throw new ValidationException("对不起，您没有查看全部管理员的权限");
                }
                else
                {
                    data = GetRequiredService<IPrivilegeQuery>().GetPlistOrganizationAccountTrs(requestData.Key.SafeTrim(),
                    requestData.OrganizationCode, requestData.IncludeDescendants.Value, requestData);
                }
            }
            else
            {
                data = GetRequiredService<IPrivilegeQuery>().GetPlistOrganizationAccountTrs(requestData.Key.SafeTrim(),
                    requestData.OrganizationCode, requestData.IncludeDescendants.Value, requestData);
            }

            Debug.Assert(requestData.Total != null, "requestData.total != null");
            return this.JsonResult(new MiniGrid<Dictionary<string, object>> { total = requestData.Total.Value, data = data });
        }

        [By("xuexs")]
        [Description("分页获取包工头")]
        [Guid("5602D2C4-BFDA-4698-A6F4-A54E5953BBF3")]
        public ActionResult GetPlistContractors(GetPlistContractors requestData)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            requestData.IncludeDescendants = requestData.IncludeDescendants ?? false;
            List<DicReader> userTrs = null;
            // 如果组织机构为空则需要检测是否是超级管理员，因为只有超级管理员才可以看到全部包工头
            if (string.IsNullOrEmpty(requestData.OrganizationCode))
            {
                if (!Host.UserSession.IsDeveloper())
                {
                    throw new ValidationException("对不起，您没有查看全部包工头的权限");
                }
                else
                {
                    userTrs = GetRequiredService<IAccountQuery>().GetPlistContractorTrs(
                        requestData.Filters, requestData.OrganizationCode, requestData.IncludeDescendants.Value, requestData);
                }
            }
            else
            {
                userTrs = GetRequiredService<IAccountQuery>().GetPlistContractorTrs(
                    requestData.Filters, requestData.OrganizationCode, requestData.IncludeDescendants.Value, requestData);
            }

            Debug.Assert(requestData.Total != null, "requestData.total != null");
            var data = new MiniGrid<Dictionary<string, object>> { total = requestData.Total.Value, data = userTrs };

            return this.JsonResult(data);
        }
    }
}
