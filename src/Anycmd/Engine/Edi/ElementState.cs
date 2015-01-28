
namespace Anycmd.Engine.Edi
{
    using Abstractions;
    using Exceptions;
    using Hecp;
    using Serialization;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class ElementState : StateObject<ElementState>, IElement, IStateObject
    {
        private Dictionary<Verb, ElementActionState> _elementActionDic;
        private List<ElementInfoRuleState> _elementInfoRuleList;
        private List<InfoRuleState> _infoRules;
        private Guid _ontologyId;
        private Guid? _infoDicId;

        private ElementState(Guid id) : base(id) { }

        public static ElementState Create(IAcDomain host, ElementBase element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            var data = new ElementState(element.Id)
            {
                AcDomain = host,
                Actions = element.Actions,
                AllowFilter = element.AllowFilter,
                AllowSort = element.AllowSort,
                Code = element.Code,
                CreateBy = element.CreateBy,
                CreateOn = element.CreateOn,
                CreateUserId = element.CreateUserId,
                DeletionStateCode = element.DeletionStateCode,
                Description = element.Description,
                FieldCode = element.FieldCode,
                ForeignElementId = element.ForeignElementId,
                GroupId = element.GroupId,
                Icon = element.Icon,
                InfoChecks = element.InfoChecks,
                InfoDicId = element.InfoDicId,
                InfoRules = element.InfoRules,
                InputHeight = element.InputHeight,
                InputType = element.InputType,
                InputWidth = element.InputWidth,
                IsDetailsShow = element.IsDetailsShow,
                IsEnabled = element.IsEnabled,
                IsExport = element.IsExport,
                IsGridColumn = element.IsGridColumn,
                IsImport = element.IsImport,
                IsInfoIdItem = element.IsInfoIdItem,
                IsInput = element.IsInput,
                IsTotalLine = element.IsTotalLine,
                MaxLength = element.MaxLength,
                ModifiedBy = element.ModifiedBy,
                ModifiedOn = element.ModifiedOn,
                ModifiedUserId = element.ModifiedUserId,
                Name = element.Name,
                Nullable = element.Nullable,
                OntologyId = element.OntologyId,
                OType = element.OType,
                Ref = element.Ref,
                Regex = element.Regex,
                SortCode = element.SortCode,
                Tooltip = element.Tooltip,
                Triggers = element.Triggers,
                UniqueElementIds = element.UniqueElementIds,
                Width = element.Width
            };
            var elementActionDic = new Dictionary<Verb, ElementActionState>();
            data._elementActionDic = elementActionDic;
            if (data.Actions != null)
            {
                var elementActions = host.JsonSerializer.Deserialize<ElementAction[]>(data.Actions);
                if (elementActions != null)
                {
                    foreach (var elementAction in elementActions)
                    {
                        OntologyDescriptor ontology;
                        if (!host.NodeHost.Ontologies.TryGetOntology(data.OntologyId, out ontology))
                        {
                            throw new AnycmdException("意外的本体元素本体标识" + data.OntologyId);
                        }
                        var actionDic = host.NodeHost.Ontologies.GetActons(ontology);
                        var verb = actionDic.Where(a => a.Value.Id == elementAction.ActionId).Select(a => a.Key).FirstOrDefault();
                        if (verb != null)
                        {
                            elementActionDic.Add(verb, ElementActionState.Create(elementAction));
                        }
                    }
                    if (elementActions.Count() != elementActionDic.Count)
                    {
                        // TODO:发现无效数据，重新序列化并持久化
                    }
                }
            }
            var elementInfoRuleList = new List<ElementInfoRuleState>();
            var infoRules = new List<InfoRuleState>();
            data._elementInfoRuleList = elementInfoRuleList;
            data._infoRules = infoRules;
            if (data.InfoRules != null)
            {
                var elementInfoRules = host.JsonSerializer.Deserialize<ElementInfoRule[]>(data.InfoRules);
                if (elementInfoRules != null)
                {
                    foreach (var elementInfoRule in elementInfoRules)
                    {
                        InfoRuleState infoRule;
                        if (host.NodeHost.InfoRules.TryGetInfoRule(elementInfoRule.InfoRuleId, out infoRule))
                        {
                            elementInfoRuleList.Add(ElementInfoRuleState.Create(host, elementInfoRule));
                            infoRules.Add(infoRule);
                        }
                    }
                    if (elementInfoRules.Count() != elementInfoRuleList.Count)
                    {
                        // TODO:发现无效数据，重新序列化并持久化
                    }
                }
            }
            return data;
        }

        #region IElement 成员
        public IAcDomain AcDomain { get; private set; }

        public Guid OntologyId
        {
            get { return _ontologyId; }
            private set
            {
                OntologyDescriptor ontology;
                if (!AcDomain.NodeHost.Ontologies.TryGetOntology(value, out ontology))
                {
                    throw new ValidationException("意外的本体标识" + value);
                }
                _ontologyId = value;
            }
        }

        /// <summary>
        /// 引用本体元素
        /// </summary>
        public Guid? ForeignElementId { get; private set; }

        public string Actions { get; private set; }

        public IReadOnlyDictionary<Verb, ElementActionState> ElementActions
        {
            get { return _elementActionDic; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string UniqueElementIds { get; private set; }

        public string InfoChecks { get; private set; }

        public string InfoRules { get; private set; }

        public IReadOnlyCollection<IElementInfoRule> ElementInfoRules
        {
            get { return _elementInfoRuleList; }
        }

        public IReadOnlyCollection<InfoRuleState> GetInfoRules()
        {
            return _infoRules;
        }

        public string Triggers { get; private set; }

        public string Code { get; private set; }

        public string FieldCode { get; private set; }

        public string Name { get; private set; }

        public string Ref { get; private set; }

        public int? MaxLength { get; private set; }

        public string OType { get; private set; }

        public bool Nullable { get; private set; }

        public Guid? InfoDicId
        {
            get { return _infoDicId; }
            private set
            {
                if (value.HasValue)
                {
                    InfoDicState infoDic;
                    if (!AcDomain.NodeHost.InfoDics.TryGetInfoDic(value.Value, out infoDic))
                    {
                        throw new ValidationException("意外的信息字典标识" + value);
                    }
                }
                _infoDicId = value;
            }
        }

        public string Regex { get; private set; }

        public bool IsInfoIdItem { get; private set; }

        public int SortCode { get; private set; }

        public string Description { get; private set; }

        public int DeletionStateCode { get; private set; }

        public int IsEnabled { get; private set; }

        public DateTime? CreateOn { get; private set; }

        public Guid? CreateUserId { get; private set; }

        public string CreateBy { get; private set; }

        public DateTime? ModifiedOn { get; private set; }

        public Guid? ModifiedUserId { get; private set; }

        public string ModifiedBy { get; private set; }

        public Guid? GroupId { get; private set; }

        public string Tooltip { get; set; }

        public string Icon { get; private set; }

        public bool IsDetailsShow { get; private set; }

        public bool IsExport { get; private set; }

        public bool IsImport { get; private set; }

        public bool IsInput { get; private set; }

        public string InputType { get; private set; }

        public bool IsTotalLine { get; private set; }

        public int? InputWidth { get; private set; }

        public int? InputHeight { get; private set; }

        public bool IsGridColumn { get; private set; }

        public int Width { get; private set; }

        public bool AllowSort { get; private set; }

        public bool AllowFilter { get; private set; }
        #endregion

        protected override bool DoEquals(ElementState other)
        {
            return Id == other.Id &&
                OntologyId == other.OntologyId &&
                ForeignElementId == other.ForeignElementId &&
                Actions == other.Actions &&
                UniqueElementIds == other.UniqueElementIds &&
                InfoChecks == other.InfoChecks &&
                InfoRules == other.InfoRules &&
                Triggers == other.Triggers &&
                Code == other.Code &&
                FieldCode == other.FieldCode &&
                Name == other.Name &&
                Ref == other.Ref &&
                MaxLength == other.MaxLength &&
                OType == other.OType &&
                Nullable == other.Nullable &&
                InfoDicId == other.InfoDicId &&
                Regex == other.Regex &&
                IsInfoIdItem == other.IsInfoIdItem &&
                SortCode == other.SortCode &&
                IsEnabled == other.IsEnabled &&
                GroupId == other.GroupId &&
                Tooltip == other.Tooltip &&
                Icon == other.Icon &&
                IsDetailsShow == other.IsDetailsShow &&
                IsExport == other.IsExport &&
                IsImport == other.IsImport &&
                IsInput == other.IsInput &&
                InputType == other.InputType &&
                IsTotalLine == other.IsTotalLine &&
                InputWidth == other.InputWidth &&
                InputHeight == other.InputHeight &&
                IsGridColumn == other.IsGridColumn &&
                Width == other.Width &&
                AllowSort == other.AllowSort &&
                AllowFilter == other.AllowFilter;
        }
    }
}
