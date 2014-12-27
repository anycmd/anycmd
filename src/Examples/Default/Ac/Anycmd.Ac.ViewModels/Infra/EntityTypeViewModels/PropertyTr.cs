
namespace Anycmd.Ac.ViewModels.Infra.EntityTypeViewModels
{
    using Engine.Ac;
    using Exceptions;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public partial class PropertyTr
    {
        private readonly IAcDomain _host;

        private PropertyTr(IAcDomain host)
        {
            this._host = host;
        }

        public static PropertyTr Create(PropertyState property)
        {
            if (property == null)
            {
                return null;
            }
            string clrPropertyType = string.Empty;
            string clrPropertyName = string.Empty;
            if (property.PropertyInfo != null)
            {
                clrPropertyType = property.PropertyInfo.PropertyType.Name;
                if (clrPropertyType == typeof(Nullable<>).Name)
                {
                    clrPropertyType = property.PropertyInfo.PropertyType.GetGenericArguments()[0].Name + "?";
                }
                clrPropertyName = property.PropertyInfo.Name;
            }
            return new PropertyTr(property.AcDomain)
            {
                Code = property.Code,
                CreateOn = property.CreateOn,
                DicId = property.DicId,
                EntityTypeId = property.EntityTypeId,
                Icon = property.Icon,
                Id = property.Id,
                InputType = property.InputType,
                IsDetailsShow = property.IsDetailsShow,
                IsDeveloperOnly = property.IsDeveloperOnly,
                IsViewField = property.IsViewField,
                MaxLength = property.MaxLength,
                Name = property.Name,
                SortCode = property.SortCode,
                IsConfigValid = property.IsConfigValid,
                DbIsNullable = property.DbIsNullable,
                DbMaxLength = property.DbMaxLength,
                DbTypeName = property.DbTypeName,
                ClrPropertyType = clrPropertyType,
                ClrPropertyName = clrPropertyName
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid EntityTypeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? MaxLength { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? DicId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int SortCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateOn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DicName
        {
            get
            {
                if (!this.DicId.HasValue)
                {
                    return string.Empty;
                }
                DicState dic;
                if (!_host.DicSet.TryGetDic(this.DicId.Value, out dic))
                {
                    throw new AnycmdException("意外的系统字典标识" + this.DicId);
                }
                return dic.Name;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDetailsShow { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDeveloperOnly { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsViewField { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string InputType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsConfigValid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool DbIsNullable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DbTypeName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? DbMaxLength { get; set; }

        public string ClrPropertyType { get; set; }

        public string ClrPropertyName { get; set; }
    }
}
