
namespace Anycmd.Query
{
    using Exceptions;
    using System;

    /// <summary>
    /// 数据查询筛选器
    /// </summary>
    public class FilterData
    {
        private object _value;

        public FilterData() { }

        #region 工厂方法
        /// <summary>
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public static FilterData EQ(string property, Guid value)
        {
            return new FilterData
            {
                type = "guid",
                value = value,
                field = property,
                comparison = "eq"
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static FilterData IsNull(string property)
        {
            return new FilterData
            {
                type = "null",
                value = null,
                field = property,
                comparison = "isnull"
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static FilterData IsNotNull(string property)
        {
            return new FilterData
            {
                type = "null",
                value = null,
                field = property,
                comparison = "isnotnull"
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public static FilterData EQ(string property, int value)
        {
            return new FilterData
            {
                type = "numeric",
                value = value,
                field = property,
                comparison = "eq"
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FilterData EQ(string property, bool value)
        {
            return new FilterData
            {
                type = "boolean",
                value = value,
                field = property,
                comparison = "eq"
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FilterData EQ(string property, string value)
        {
            return Create(property, "eq", value);
        }

        /// <summary>
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FilterData Like(string property, string value)
        {
            return Create(property, "like", value);
        }

        /// <summary>
        /// </summary>
        /// <param name="property"></param>
        /// <param name="comparison"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FilterData Create(string property, string comparison, string value)
        {
            return new FilterData
            {
                type = "string",
                value = value,
                field = property,
                comparison = comparison
            };
        }
        #endregion

        /// <summary>
        /// <example>string、guid、boolean、numeric、date、list</example>
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object value
        {
            get { return _value; }
            set
            {
                if (this.type == "numeric")
                {
                    if (value == null)
                    {
                        throw new ValidationException(this.field + " value is null");
                    }
                    int v;
                    if (!int.TryParse(value.ToString(), out v))
                    {
                        throw new ValidationException("字段" + this.field + "的值" + this.value.ToString() + "不是数字");
                    }
                    _value = v;
                }
                else if (this.type == "guid")
                {
                    Guid guidValue;
                    if (!Guid.TryParse(value.ToString(), out guidValue))
                    {
                        throw new ValidationException("意外的guid" + value);
                    }
                    _value = guidValue;
                }
                else if (this.type == "boolean")
                {
                    bool boolValue;
                    if (!bool.TryParse(value.ToString(), out boolValue))
                    {
                        throw new ValidationException("意外的boolean" + value);
                    }
                    _value = boolValue;
                }
                else if (value != null && this.type == "string")
                {
                    _value = value.ToString().Trim();
                }
                else
                {
                    _value = value;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string field { get; set; }
        /// <summary>
        /// <example>lt、gt、eq、like</example>
        /// </summary>
        public string comparison { get; set; }

        public string ToPredicate()
        {
            string res = string.Empty;
            switch (comparison)
            {
                case "lt":
                    res = this.field + "<@0";
                    break;
                case "gt":
                    res = this.field + ">@0";
                    break;
                case "eq":
                    res = this.field + "=@0";
                    break;
                case "like":
                    res = this.field + ".ToLower().Contains(@0.ToLower())";
                    break;
                case "isnull":
                    res = this.field + " == null ";
                    break;
                case "isnotnull":
                    res = this.field + " != null ";
                    break;
            }
            return res;
        }
    }
}
