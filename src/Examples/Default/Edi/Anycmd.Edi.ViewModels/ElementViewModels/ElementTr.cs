
namespace Anycmd.Edi.ViewModels.ElementViewModels
{
    using Engine.Edi;
    using Engine.Host.Ac.Infra;
    using Exceptions;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public partial class ElementTr
    {
        private readonly IAcDomain _host;

        public ElementTr(IAcDomain host)
        {
            this._host = host;
        }

        public static ElementTr Create(ElementState element)
        {
            return new ElementTr(element.AcDomain)
            {
                Nullable = element.Nullable,
                Code = element.Code,
                CreateOn = element.CreateOn,
                OType = element.OType,
                FieldCode = element.FieldCode,
                GroupId = element.GroupId ?? Guid.Empty,
                Icon = element.Icon,
                OntologyId = element.OntologyId,
                Id = element.Id,
                InfoDicId = element.InfoDicId,
                IsDetailsShow = element.IsDetailsShow,
                IsEnabled = element.IsEnabled,
                IsExport = element.IsExport,
                IsGridColumn = element.IsGridColumn,
                IsImport = element.IsImport,
                IsInfoIdItem = element.IsInfoIdItem,
                IsInput = element.IsInput,
                MaxLength = element.MaxLength,
                Name = element.Name,
                Ref = element.Ref,
                Regex = element.Regex,
                SortCode = element.SortCode
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FieldCode { get; set; }
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
        public Guid OntologyId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OntologyCode
        {
            get
            {
                return this.Ontology.Ontology.Code;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OntologyName
        {
            get
            {
                return this.Ontology.Ontology.Name;
            }
        }
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
        public string OType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Regex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid? InfoDicId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string InfoDicName
        {
            get
            {
                if (!this.InfoDicId.HasValue)
                {
                    return string.Empty;
                }
                return this.InfoDic.Name;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string InfoDicCode
        {
            get
            {
                if (!this.InfoDicId.HasValue)
                {
                    return string.Empty;
                }
                return this.InfoDic.Code;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsEnabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Ref { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsInfoIdItem { get; set; }
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
        public Guid GroupId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDetailsShow { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsExport { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsImport { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool? IsInput { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsGridColumn { get; set; }

        private ElementDataSchema _dataSchema;
        private ElementDataSchema DataSchema
        {
            get
            {
                if (_dataSchema == null)
                {
                    _dataSchema = _host.NodeHost.Ontologies.GetElement(this.Id).DataSchema;
                }

                return _dataSchema;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsConfigValid
        {
            get
            {
                bool isValid = true;
                if (DataSchema == null)
                {
                    isValid = false;
                }
                else if (DataSchema.MaxLength.HasValue && DataSchema.MaxLength > 0 && this.MaxLength > DataSchema.MaxLength)
                {
                    isValid = false;
                }

                return isValid;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool DbIsNullable
        {
            get
            {
                if (DataSchema == null)
                {
                    return false;
                }
                return DataSchema.IsNullable;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DbTypeName
        {
            get
            {
                if (DataSchema == null)
                {
                    return string.Empty;
                }
                return DataSchema.TypeName;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DbMaxLength
        {
            get
            {
                if (DataSchema == null)
                {
                    return null;
                }
                return DataSchema.MaxLength;
            }
        }

        private OntologyDescriptor _ontology;
        private OntologyDescriptor Ontology
        {
            get
            {
                if (_ontology == null)
                {
                    if (!_host.NodeHost.Ontologies.TryGetOntology(this.OntologyId, out _ontology))
                    {
                        throw new AnycmdException("意外的本体标识" + this.OntologyId);
                    }
                }
                return _ontology;
            }
        }

        private InfoDicState _infoDic;
        private InfoDicState InfoDic
        {
            get
            {
                if (!this.InfoDicId.HasValue)
                {
                    return null;
                }
                if (_infoDic == null)
                {
                    if (!_host.NodeHost.InfoDics.TryGetInfoDic(this.InfoDicId.Value, out _infoDic))
                    {
                        throw new AnycmdException("意外的信息字典标识" + this.InfoDicId);
                    }
                }
                return _infoDic;
            }
        }
    }
}
