
namespace Anycmd.Edi.ViewModels.ElementViewModels
{
    using Engine.Edi;
    using Engine.Host.Ac.Infra;
    using Exceptions;
    using Model;
    using System;
    using System.Collections.Generic;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public partial class ElementInfo : Dictionary<string, object>
    {
        private readonly IAcDomain _acDomain;

        private ElementInfo(IAcDomain acDomain)
        {
            this._acDomain = acDomain;
        }

        public static ElementInfo Create(DicReader dic)
        {
            if (dic == null)
            {
                return null;
            }
            var data = new ElementInfo(dic.AcDomain);
            foreach (var item in dic)
            {
                data.Add(item.Key, item.Value);
            }
            data.Id = (Guid)dic["Id"];
            OntologyDescriptor ontology;
            if (!dic.AcDomain.NodeHost.Ontologies.TryGetOntology((Guid)data["OntologyId"], out ontology))
            {
                throw new AnycmdException("意外的本体标识" + data["OntologyId"]);
            }
            if (!data.ContainsKey("OntologyCode"))
            {
                data.Add("OntologyCode", ontology.Ontology.Code);
            }
            if (!data.ContainsKey("OntologyName"))
            {
                data.Add("OntologyName", ontology.Ontology.Name);
            }
            if (data["MaxLength"] == DBNull.Value)
            {
                data.MaxLength = null;
            }
            else
            {
                data.MaxLength = (int?)data["MaxLength"];
            }
            if (!data.ContainsKey("DeletionStateName"))
            {
                data.Add("DeletionStateName", dic.AcDomain.Translate("Edi", "Element", "DeletionStateName", data["DeletionStateCode"].ToString()));
            }
            if (!data.ContainsKey("IsEnabledName"))
            {
                data.Add("IsEnabledName", dic.AcDomain.Translate("Edi", "Element", "IsEnabledName", data["IsEnabled"].ToString()));
            }
            if (data["InfoDicId"] == DBNull.Value)
            {
                data.InfoDicId = null;
            }
            else
            {
                data.InfoDicId = (Guid?)data["InfoDicId"];
            }
            if (data.InfoDicId.HasValue && !data.ContainsKey("InfoDicName"))
            {
                InfoDicState infoDic = null;
                if (data.InfoDicId != null && !dic.AcDomain.NodeHost.InfoDics.TryGetInfoDic(data.InfoDicId.Value, out infoDic))
                {
                    throw new AnycmdException("意外的信息字典标识" + data.InfoDicId.Value);
                }
                if (infoDic != null) data.Add("InfoDicName", infoDic.Name);
            }
            if (!data.ContainsKey("IsConfigValid"))
            {
                data.Add("IsConfigValid", data.IsConfigValid);
            }
            if (!data.ContainsKey("DbIsNullable"))
            {
                data.Add("DbIsNullable", data.DbIsNullable);
            }
            if (!data.ContainsKey("DbTypeName"))
            {
                data.Add("DbTypeName", data.DbTypeName);
            }
            if (!data.ContainsKey("DbMaxLength"))
            {
                data.Add("DbMaxLength", data.DbMaxLength);
            }

            return data;
        }
        private Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        private int? MaxLength { get; set; }
        /// <summary>
        /// 
        /// </summary>
        private Guid? InfoDicId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        private bool IsConfigValid
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
        private bool DbIsNullable
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
        private string DbTypeName
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
        private int? DbMaxLength
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

        private ElementDataSchema _dataSchema;
        private ElementDataSchema DataSchema
        {
            get
            {
                if (_dataSchema == null)
                {
                    _dataSchema = _acDomain.NodeHost.Ontologies.GetElement(this.Id).DataSchema;
                }

                return _dataSchema;
            }
        }
    }
}
