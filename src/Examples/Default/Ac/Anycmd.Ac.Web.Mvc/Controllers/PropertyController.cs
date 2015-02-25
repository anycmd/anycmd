
namespace Anycmd.Ac.Web.Mvc.Controllers
{
    using Anycmd.Web.Mvc;
    using Engine.Ac;
    using Engine.Ac.EntityTypes;
    using Engine.Host.Ac.Infra;
    using Exceptions;
    using MiniUI;
    using Repositories;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Web.Mvc;
    using Util;
    using ViewModel;
    using ViewModels;
    using ViewModels.EntityTypeViewModels;

    /// <summary>
    /// 系统属性模型视图控制器
    /// </summary>
    [Guid("C73511E9-2430-462B-A96D-B2339237C55F")]
    public class PropertyController : AnycmdController
    {
        #region ViewResults
        [By("xuexs")]
        [Description("字段")]
        [Guid("FAD2562A-0CE2-4758-BD72-653CFA6044F2")]
        public ViewResultBase Index()
        {
            return ViewResult();
        }

        [By("xuexs")]
        [Description("字段详细信息")]
        [Guid("B5131C71-F4B8-4CEC-9004-74D3718A39AE")]
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

        #endregion

        [By("xuexs")]
        [Description("根据ID获取字段")]
        [Guid("EDBAB8FD-676F-4048-81A9-162C13123668")]
        public ActionResult Get(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(base.EntityType.GetData(id.Value));
        }

        [By("xuexs")]
        [Description("根据ID获取字段详细信息")]
        [Guid("E29B47BF-6946-4E95-B905-39A28AEA8280")]
        public ActionResult GetInfo(Guid? id)
        {
            if (!id.HasValue)
            {
                throw new ValidationException("未传入标识");
            }
            return this.JsonResult(base.EntityType.GetData(id.Value));
        }

        [By("xuexs")]
        [Description("获取字段提示信息")]
        [Guid("80AFCC53-0279-4EBE-BD9E-DE43B844D900")]
        public ActionResult Tooltip(Guid? propertyId)
        {
            if (propertyId.HasValue)
            {
                PropertyState property;
                if (!AcDomain.EntityTypeSet.TryGetProperty(propertyId.Value, out property))
                {
                    throw new ValidationException("意外的系统属性标识" + propertyId);
                }
                return this.PartialView("Partials/Tooltip", property);
            }
            else
            {
                return this.Content("无效的propertyId");
            }
        }

        [By("xuexs")]
        [Description("编辑字段帮助")]
        [ValidateInput(enableValidation: false)]
        [Guid("E9C2D629-6DC0-4698-A5CC-00E4121A1F43")]
        public ActionResult TooltipEdit(string isInner, string tooltip, Guid? propertyId)
        {
            if (propertyId.HasValue)
            {
                PropertyState property;
                if (!AcDomain.EntityTypeSet.TryGetProperty(propertyId.Value, out property))
                {
                    throw new ValidationException("意外的系统属性标识" + propertyId);
                }
                if (Request.HttpMethod == "POST")
                {
                    var entity = GetRequiredService<IRepository<Property>>().GetByKey(propertyId.Value);
                    AcDomain.Handle(new PropertyUpdateInput
                    {
                        Code = entity.Code,
                        Description = entity.Description,
                        Icon = entity.Icon,
                        Id = entity.Id,
                        DicId = entity.DicId,
                        GuideWords = entity.GuideWords,
                        IsDetailsShow = entity.IsDetailsShow,
                        InputType = entity.InputType,
                        IsDeveloperOnly = entity.IsDeveloperOnly,
                        IsInput = entity.IsInput,
                        IsTotalLine = entity.IsTotalLine,
                        MaxLength = entity.MaxLength,
                        Name = entity.Name,
                        SortCode = entity.SortCode
                    }.ToCommand(AcSession));
                    return this.JsonResult(new ResponseData { success = true });
                }
                else
                {
                    return this.PartialView(property);
                }
            }
            else
            {
                return this.Content("无效的propertyId");
            }
        }

        [By("xuexs")]
        [Description("根据实体类型标识分页获取实体属性")]
        [Guid("4190737D-5124-4F78-8D83-0703DA4C104F")]
        public ActionResult GetPlistProperties(GetPlistProperties requestModel)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            var data = AcDomain.GetPlistProperties(requestModel);

            Debug.Assert(requestModel.Total != null, "requestModel.total != null");
            return this.JsonResult(new MiniGrid<PropertyTr> { total = requestModel.Total.Value, data = data });
        }

        [By("xuexs")]
        [Description("添加字段")]
        [HttpPost]
        [DeveloperFilter(Order = 21)]
        [Guid("0B137CA9-7211-4AB6-8A81-4A392F8D7511")]
        public ActionResult Create(PropertyCreateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            AcDomain.Handle(input.ToCommand(AcSession));

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        [By("xuexs")]
        [Description("创建通用属性")]
        [HttpPost]
        [DeveloperFilter(Order = 21)]
        [Guid("9C8FBB21-9E05-4DDD-A123-4EC000720EC6")]
        public ActionResult CreateCommonProperties(Guid? entityTypeId)
        {
            if (!entityTypeId.HasValue)
            {
                throw new ValidationException("实体类型标识是必须的");
            }
            AcDomain.Handle(new AddCommonPropertiesCommand(AcSession, entityTypeId.Value));

            return this.JsonResult(new ResponseData { id = null, success = true });
        }

        [By("xuexs")]
        [Description("更新字段")]
        [HttpPost]
        [DeveloperFilter(Order = 21)]
        [Guid("291F7FCD-7E7F-452C-B9DD-0E35709675EF")]
        public ActionResult Update(PropertyUpdateInput input)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }
            AcDomain.Handle(input.ToCommand(AcSession));

            return this.JsonResult(new ResponseData { id = input.Id, success = true });
        }

        [By("xuexs")]
        [Description("删除字段")]
        [HttpPost]
        [DeveloperFilter(Order = 21)]
        [Guid("979F63E6-ADE9-4366-B00E-E6520E0121CF")]
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
                    throw new ValidationException("意外的字段标识" + ids[i]);
                }
            }
            foreach (var item in idArray)
            {
                AcDomain.Handle(new RemovePropertyCommand(AcSession, item));
            }

            return this.JsonResult(new ResponseData { id = id, success = true });
        }
    }
}
