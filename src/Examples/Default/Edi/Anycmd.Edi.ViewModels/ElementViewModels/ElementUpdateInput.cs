
namespace Anycmd.Edi.ViewModels.ElementViewModels
{
    using Engine.Edi.InOuts;
    using Engine.Edi.Messages;
    using Engine.Messages;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class ElementUpdateInput : IElementUpdateIo
    {
        public ElementUpdateInput()
        {
            HecpOntology = "Element";
            HecpVerb = "Update";
        }

        public string HecpOntology { get; private set; }

        public string HecpVerb { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? InfoDicId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Code { get; set; }
        /// <summary>
        /// 是否是信息标识项
        /// </summary>
        [Required]
        public bool IsInfoIdItem { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string FieldCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? MaxLength { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Nullable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Regex { get; set; }
        /// <summary>
        /// 是否默认导出
        /// </summary>
        public bool IsExport { get; set; }
        /// <summary>
        /// 是否导入
        /// </summary>
        public bool IsImport { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public int SortCode { get; set; }
        /// <summary>
        /// 引用来源
        /// </summary>
        public string Ref { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(1)]
        public int IsEnabled { get; set; }

        #region UI
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(true)]
        public bool IsDetailsShow { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(true)]
        public bool IsInput { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string InputType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public bool IsTotalLine { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? InputWidth { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? InputHeight { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? GroupId { get; set; }
        #endregion

        #region ColumnUI
        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(false)]
        public bool IsGridColumn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(0)]
        public int Width { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        public bool AllowSort { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool AllowFilter { get; set; }
        public string Tooltip { get; set; }
        #endregion

        public IAnycmdCommand ToCommand(IAcSession acSession)
        {
            return new UpdateElementCommand(acSession, this);
        }
    }
}
