
namespace Anycmd.Engine.Edi
{
	using Ac;
	using Exceptions;
	using Hecp;
	using Host.Ac.Infra;
	using Host.Edi;
    using System;
	using System.Collections.Generic;
	using System.Data;
	using Util;
	using elementCode = System.String;

	/// <summary>
	/// 本体元素描述对象。描述对象往往长久贮存在内存中。
	/// </summary>
	public sealed class ElementDescriptor : StateObject<ElementDescriptor>
	{
		private bool _isCodeValue;
		private bool _isCodeValueDetected;
		private DbType _dbType;

		#region const string
		/// <summary>
		/// 数字自增标识编码
		/// </summary>
		public static readonly DbField IncrementIdCode = new DbField("IncrementId", System.Data.DbType.Int32, false, null);
		/// <summary>
		/// 实体的Guid标识编码
		/// </summary>
		public static readonly DbField IdElementCode = new DbField("Id", System.Data.DbType.Guid, false, null);
		/// <summary>
		/// 实体级权限
		/// </summary>
		public static readonly DbField EntityActionElementCode = new DbField("EntityAction", System.Data.DbType.String, true, 500);
		/// <summary>
		/// 实体元素级权限
		/// </summary>
		public static readonly DbField EntityElementActionElementCode = new DbField("EntityElementAction", System.Data.DbType.String, true, 500);
		/// <summary>
		/// 访问控制表政策
		/// </summary>
		public static readonly DbField EntityAclPolicyElementCode = new DbField("EntityAclPolicy", System.Data.DbType.String, true, 500);
		/// <summary>
		/// 
		/// </summary>
		public static readonly DbField EntityJavascriptElementCode = new DbField("EntityJavascript", System.Data.DbType.String, true, 1000);
		/// <summary>
		/// 创建时间编码
		/// </summary>
		public static readonly DbField CreateOnCode = new DbField("CreateOn", System.Data.DbType.DateTime, true, null);
		/// <summary>
		/// 最后修改时间编码
		/// </summary>
		public static readonly DbField ModifiedOnCode = new DbField("ModifiedOn", System.Data.DbType.DateTime, true, null);
		/// <summary>
		/// 创建者标识编码
		/// </summary>
		public static readonly DbField CreateUserIdCode = new DbField("CreateUserId", System.Data.DbType.Guid, true, null);
		/// <summary>
		/// 最后修改者标识编码
		/// </summary>
		public static readonly DbField ModifiedUserIdCode = new DbField("ModifiedUserId", System.Data.DbType.Guid, true, null);
		/// <summary>
		/// 创建节点标识编码
		/// </summary>
		public static readonly DbField CreateNodeIdCode = new DbField("CreateNodeId", System.Data.DbType.Guid, false, null);
		/// <summary>
		/// 最后修改节点标识编码
		/// </summary>
		public static readonly DbField ModifiedNodeIdCode = new DbField("ModifiedNodeId", System.Data.DbType.Guid, false, null);
		/// <summary>
		/// 创建命令标识编码
		/// </summary>
		public static readonly DbField CreateCommandIdCode = new DbField("CreateCommandId", System.Data.DbType.Guid, true, null);
		/// <summary>
		/// 最后修改命令标识编码
		/// </summary>
		public static readonly DbField ModifiedCommandIdCode = new DbField("ModifiedCommandId", System.Data.DbType.Guid, true, null);
		#endregion

		/// <summary>
		/// 
		/// </summary>
		public static readonly Dictionary<elementCode, DbField> SystemElementCodes = new Dictionary<elementCode, DbField>(StringComparer.OrdinalIgnoreCase) {
			{IncrementIdCode.Name, IncrementIdCode},
			{IdElementCode.Name,IdElementCode},
			{EntityActionElementCode.Name,EntityActionElementCode},
			{EntityElementActionElementCode.Name,EntityElementActionElementCode},
			{EntityAclPolicyElementCode.Name,EntityAclPolicyElementCode},
			{EntityJavascriptElementCode.Name,EntityJavascriptElementCode},
			{CreateOnCode.Name,CreateOnCode},
			{ModifiedOnCode.Name,ModifiedOnCode},
			{CreateUserIdCode.Name,CreateUserIdCode},
			{ModifiedUserIdCode.Name,ModifiedUserIdCode},
			{CreateNodeIdCode.Name,CreateNodeIdCode},
			{ModifiedNodeIdCode.Name,ModifiedNodeIdCode},
			{CreateCommandIdCode.Name,CreateCommandIdCode},
			{ModifiedCommandIdCode.Name,ModifiedCommandIdCode}
		};

		private readonly IAcDomain _host;

		#region Ctor

		/// <summary>
		/// 
		/// </summary>
		/// <param name="host"></param>
		/// <param name="element"></param>
		/// <param name="id"></param>
		public ElementDescriptor(IAcDomain host, ElementState element, Guid id)
			: base(id)
		{
			this._host = host;
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			this.Element = element;
			if (!element.OType.TryParse(out _dbType))
			{
				throw new AnycmdException("意外的数据库类型" + element.OType);
			}
			string fieldName = element.Code;
			if (!string.IsNullOrEmpty(element.FieldCode))
			{
				fieldName = element.FieldCode;
			}
			this.FieldSchema = new DbField(fieldName, _dbType, element.Nullable, element.MaxLength);
			this.IsRuntimeElement = SystemElementCodes.ContainsKey(element.Code);
		}
		#endregion

		public IAcDomain Host
		{
			get { return _host; }
		}

		/// <summary>
		/// 
		/// </summary>
		public ElementState Element { get; private set; }

		/// <summary>
		/// 
		/// </summary>
		public DbField FieldSchema { get; private set; }

		#region IsCodeValue
		/// <summary>
		/// 是否是码值。码值对人类来说不易读，码值在展示给人类的时候往往需要翻译。
		/// </summary>
		public bool IsCodeValue
		{
			get
			{
				if (!_isCodeValueDetected)
				{
					_isCodeValueDetected = true;
					_isCodeValue = false;
					if (this.Ontology.Ontology.IsOrganizationalEntity && this.Element.Code.Equals("ZZJGM", StringComparison.OrdinalIgnoreCase))
					{
						_isCodeValue = true;
					}
					else if (this == this.Ontology.CreateNodeIdElement)
					{
						_isCodeValue = true;
					}
					else if (this == this.Ontology.ModifiedNodeIdElement)
					{
						_isCodeValue = true;
					}
					else if (this.Element.InfoDicId.HasValue)
					{
						_isCodeValue = true;
					}
				}
				return _isCodeValue;
			}
			internal set
			{
				_isCodeValue = value;
			}
		}
		#endregion

		/// <summary>
		/// 本体
		/// </summary>
		public OntologyDescriptor Ontology
		{
			get
			{
				return Host.NodeHost.Ontologies[this.Element.OntologyId];
			}
		}

		/// <summary>
		/// 数据库列
		/// </summary>
		public ElementDataSchema DataSchema
		{
			get
			{
				return this.Ontology.EntityProvider.GetElementDataSchema(this);
			}
		}

		/// <summary>
		/// 是否是运行时元素。True表示是，False不是。
		/// </summary>
		public bool IsRuntimeElement { get; private set; }

		/// <summary>
		/// 1，“配置验证”项通过的条件是：首先，主题元素对应编码的字段在数据库中是存在的；其次，主题元素上配置的长度是小于等于数据库中对应字段的长度的。
		/// </summary>
		public bool IsConfigValid
		{
			get
			{
				if (DataSchema == null)
				{
					return false;
				}
				bool isValid = true;
				if (Element.MaxLength > DataSchema.MaxLength)
				{
					isValid = false;
				}
				else if (DataSchema == null)
				{
					isValid = false;
				}

				return isValid;
			}
		}

		#region TranslateValue
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public string TranslateValue(string value)
		{
			if (!this.IsCodeValue)
			{
				return value;
			}
			// 是否能成功翻译，如果不能成功翻译则会返回原始值。
			// 翻译组织结构码为组织结构名
			if (this.Ontology.Ontology.IsOrganizationalEntity
				&& this.Element.Code.Equals("ZZJGM", StringComparison.OrdinalIgnoreCase))
			{
				OrganizationState org;
				return Host.OrganizationSet.TryGetOrganization(value, out org) ? org.Name : "非法的无法翻译的组织结构码";
			}
			// 翻译节点标识为节点名
			else if (this == this.Ontology.CreateNodeIdElement)
			{
				NodeDescriptor node;
				return Host.NodeHost.Nodes.TryGetNodeById(value, out node) ? node.Node.Name : "非法的无法翻译的节点标识";
			}
			// 翻译节点标识为节点名
			else if (this == this.Ontology.ModifiedNodeIdElement)
			{
				NodeDescriptor node;
				if (Host.NodeHost.Nodes.TryGetNodeById(value, out node))
				{
					return node.Node.Name;
				}
				else
				{
					return "非法的无法翻译的节点标识";
				}
			}
			// 翻译字典项编码为字典项名称
			else if (this.Element.InfoDicId.HasValue)
			{
				if (!string.IsNullOrWhiteSpace(value))
				{
					InfoDicState infoDic;
					if (!Host.NodeHost.InfoDics.TryGetInfoDic(this.Element.InfoDicId.Value, out infoDic))
					{
						return value;
					}
					InfoDicItemState infoDicItem;
					return Host.NodeHost.InfoDics.TryGetInfoDicItem(infoDic, value, out infoDicItem) ? infoDicItem.Name : "非法的无法翻译的字典值";
				}
				else
				{
					return string.Empty;
				}
			}
			else
			{
				return value;
			}
		}
		#endregion

		/// <summary>
		/// 获取给定节点的节点元素级权限
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public IReadOnlyDictionary<Verb, NodeElementActionState> GetActions(NodeDescriptor node)
		{
			return Host.NodeHost.Nodes.GetNodeElementActions(node, this);
		}

		protected override bool DoEquals(ElementDescriptor other)
		{
			return this.Element == other.Element || this.Element.Id == other.Element.Id;
		}
	}
}
