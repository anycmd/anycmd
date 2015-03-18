
namespace Anycmd.Xacml.Runtime
{
	using Configuration;
	using Functions;
	using Interfaces;
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Diagnostics;
	using Xacml.Policy.TargetItems;
	using System.Text;
	using System.Xml;
	using System.Xml.XPath;
	using cor = Xacml;
	using ctx = Context;
	using pol = Xacml.Policy;
	using typ = DataTypes;

	/// <summary>
	/// The EvaluationEngine is the PDP（策略决策点） implementation which receives a policy document and a 
	/// context document and perform the evaluation of the policies. This instance is safe to be 
	/// reused but not safe for multithread operations. If multiple operations must be carried on
	/// a new instance must be created or the code must use a single instance per thread.
	/// </summary>
	public class EvaluationEngine
	{
		#region Private members

		private readonly bool _verbose;
		/// <summary>
		/// 所有的内置函数。
		/// </summary>
		private static IDictionary<string, IFunction> _functions;

		/// <summary>
		/// 所有的内置类型。
		/// </summary>
		private static IDictionary<string, IDataType> _dataTypes;

		#endregion

		#region Constructor

		/// <summary>
		/// Creates a new instance of the EvaluationEngine with the tracing disabled.
		/// </summary>
		public EvaluationEngine()
		{
			Prepare();
		}

		/// <summary>
		/// Creates a new instance of the EvaluationEngine with the tracing information.
		/// </summary>
		/// <param name="verbose">Whether the trace will be sent to the console or not.</param>
		public EvaluationEngine(bool verbose)
			: this()
		{
			_verbose = verbose;
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Static method used to get the internal data type description using the data type id.
		/// </summary>
		/// <param name="typeId">The data type Id.</param>
		/// <returns>The data type descriptor.</returns>
		public static IDataType GetDataType(string typeId)
		{
			IDataType dataType;
			if (_dataTypes.TryGetValue(typeId, out dataType)) return dataType;
			if (ConfigurationRoot.Config == null) return null;
			foreach (IDataTypeRepository rep in ConfigurationRoot.Config.DataTypeRepositories)
			{
				dataType = rep.GetDataType(typeId);
				if (dataType != null)
				{
					return dataType;
				}
			}
			return null;
		}

		/// <summary>
		/// The factory creates a new instance of the PolicyCombiningAlgorithm.
		/// </summary>
		/// <param name="policyCombiningAlgorithmId">The name of the policy combining algorithm.</param>
		/// <returns>A new instance of the policy combining algorithm.</returns>
		public static IPolicyCombiningAlgorithm CreatePolicyCombiningAlgorithm(string policyCombiningAlgorithmId)
		{
			switch (policyCombiningAlgorithmId)
			{
				case Consts.Schema1.PolicyCombiningAlgorithms.DenyOverrides:
					return new PolicyCombiningAlgorithmDenyOverrides();
				case Consts.Schema1.PolicyCombiningAlgorithms.PermitOverrides:
					return new PolicyCombiningAlgorithmPermitOverrides();
				case Consts.Schema1.PolicyCombiningAlgorithms.FirstApplicable:
					return new PolicyCombiningAlgorithmFirstApplicable();
				case Consts.Schema1.PolicyCombiningAlgorithms.OnlyOneApplicable:
					return new PolicyCombiningAlgorithmOnlyOneApplicable();
				case Consts.Schema1.PolicyCombiningAlgorithms.OrderedDenyOverrides:
					return new PolicyCombiningAlgorithmOrderedDenyOverrides();
				case Consts.Schema1.PolicyCombiningAlgorithms.OrderedPermitOverrides:
					return new PolicyCombiningAlgorithmOrderedPermitOverrides();
				default:
					{
						if (ConfigurationRoot.Config == null) return null;
						foreach (IPolicyCombiningAlgorithmRepository rep in ConfigurationRoot.Config.PolicyCombiningAlgorithmRepositories)
						{
							IPolicyCombiningAlgorithm pca = rep.GetPolicyCombiningAlgorithm(policyCombiningAlgorithmId);
							if (pca != null)
							{
								return pca;
							}
						}
						return null;
					}
			}
		}

		/// <summary>
		/// The factory creates a new instance of the derived classes.
		/// </summary>
		/// <param name="ruleCombiningAlgorithmId">The name of the rule combining algorithm.</param>
		/// <returns>A new instance of the rule combinig algorithm.</returns>
		public static IRuleCombiningAlgorithm CreateRuleCombiningAlgorithm(string ruleCombiningAlgorithmId)
		{
			switch (ruleCombiningAlgorithmId)
			{
				case Consts.Schema1.RuleCombiningAlgorithms.DenyOverrides:
					return new RuleCombiningAlgorithmDenyOverrides();
				case Consts.Schema1.RuleCombiningAlgorithms.PermitOverrides:
					return new RuleCombiningAlgorithmPermitOverrides();
				case Consts.Schema1.RuleCombiningAlgorithms.FirstApplicable:
					return new RuleCombiningAlgorithmFirstApplicable();
				case Consts.Schema1.RuleCombiningAlgorithms.OrderedDenyOverrides:
					return new RuleCombiningAlgorithmOrderedDenyOverrides();
				case Consts.Schema1.RuleCombiningAlgorithms.OrderedPermitOverrides:
					return new RuleCombiningAlgorithmOrderedPermitOverrides();
				default:
					{
						if (ConfigurationRoot.Config == null) return null;
						foreach (IRuleCombiningAlgorithmRepository rep in ConfigurationRoot.Config.RuleCombiningAlgorithmRepositories)
						{
							IRuleCombiningAlgorithm rca = rep.GetRuleCombiningAlgorithm(ruleCombiningAlgorithmId);
							if (rca != null)
							{
								return rca;
							}
						}
						return null;
					}
			}
		}

		/// <summary>
		/// Evaluate the context document using a specified policy
		/// </summary>
		/// <param name="contextDocument">The context document instance</param>
		/// <returns>The response document.</returns>
		public ctx.ResponseElement Evaluate(ctx.ContextDocument contextDocument)
		{
			var context = new EvaluationContext(this, null, contextDocument);

			try
			{
				// Validates the configuration file was found.
				if (ConfigurationRoot.Config != null)
				{
					// Search all the policies repositories to find a policy that matches the 
					// context document
					pol.PolicyDocument policyDocument = null;
					foreach (IPolicyRepository policyRep in ConfigurationRoot.Config.PolicyRepositories)
					{
						if (policyDocument == null)
						{
							policyDocument = policyRep.Match(context);
						}
						else
						{
							throw new EvaluationException(Properties.Resource.exc_duplicated_policy_in_repository);
						}
					}

					// If the policy was found evaluate the context document, otherwise use the
					// Evaluate method to generate a Response context document.
					if (policyDocument != null)
					{
						return Evaluate(policyDocument, contextDocument);
					}
					else
					{
						return Evaluate(null, null);
					}
				}
				else
				{
					throw new EvaluationException(Properties.Resource.exc_configuration_file_not_found);
				}
			}
			catch (EvaluationException e)
			{
				context.Trace("ERR: {0}", e.Message);
				return Evaluate(null, null);
			}
		}

		/// <summary>
		/// Evaluate the context document using a specified policy.
		/// </summary>
		/// <param name="policyDocument">The policy instance.</param>
		/// <param name="contextDocument">The context document instance.</param>
		/// <param name="schemaVersion">The version of the schema used to validate the document.</param>
		/// <returns>The response document</returns>
		public ctx.ResponseElement Evaluate(string policyDocument, string contextDocument, XacmlVersion schemaVersion)
		{
			return Evaluate(
				(pol.PolicyDocument)PolicyLoader.LoadPolicyDocument(new FileStream(policyDocument, FileMode.Open), schemaVersion, DocumentAccess.ReadOnly),
				(ctx.ContextDocument)ContextLoader.LoadContextDocument(new FileStream(contextDocument, FileMode.Open), schemaVersion));
		}

		/// <summary>
		/// Evaluate the context document using a specified policy
		/// </summary>
		/// <param name="policyDocument">The policy instance</param>
		/// <param name="contextDocument">The context document instance</param>
		/// <returns>The response document</returns>
		public ctx.ResponseElement Evaluate(pol.PolicyDocument policyDocument, ctx.ContextDocument contextDocument)
		{
			if (policyDocument == null) throw new ArgumentNullException("policyDocument");
			if (contextDocument == null) throw new ArgumentNullException("contextDocument");
			var context = new EvaluationContext(this, policyDocument, contextDocument);

			context.Trace("Start evaluation");
			context.AddIndent();

			// Check if both documents are valid
			if (!policyDocument.IsValidDocument || !contextDocument.IsValidDocument)
			{
				// If a validation error was found a response is created with the syntax error message
				var response =
					new ctx.ResponseElement(
						new[] {
							new ctx.ResultElement( 
								null, 
								Decision.Indeterminate, 
								new ctx.StatusElement( 
									new ctx.StatusCodeElement(Consts.ContextSchema.StatusCodes.SyntaxError, null, policyDocument.Version ), 
									null, 
									null, policyDocument.Version ), 
								null, policyDocument.Version ) },
					policyDocument.Version);
				return response;
			}

			// Create a new response
			contextDocument.Response = new ctx.ResponseElement((ctx.ResultElement[])null, policyDocument.Version);

			try
			{
				// Create the evaluable policy intance
				IMatchEvaluable policy = null;
				if (policyDocument.PolicySet != null)
				{
					policy = new PolicySet(this, (pol.PolicySetElement)policyDocument.PolicySet);
				}
				else if (policyDocument.Policy != null)
				{
					policy = new Policy((pol.PolicyElement)policyDocument.Policy);
				}

				// Evaluate the policy or policy set
				if (policy != null)
				{
					// Creates the evaluable policy set
					if (policy.AllResources.Count == 0)
					{
						policy.AllResources.Add("");
					}

					string requestedResourceString = String.Empty;
					Uri requestedResource = null;

					foreach (ctx.ResourceElement resource in contextDocument.Request.Resources)
					{
						// Keep the requested resource
						if (resource.IsHierarchical)
						{
							foreach (ctx.AttributeElement attribute in resource.Attributes)
							{
								if (attribute.AttributeId == Consts.ContextSchema.ResourceElement.ResourceId)
								{
									if (context.PolicyDocument.Version == XacmlVersion.Version10 ||
										context.PolicyDocument.Version == XacmlVersion.Version11)
									{
										requestedResourceString = attribute.AttributeValues[0].Contents;
									}
									else
									{
										if (attribute.AttributeValues.Count > 1)
										{
											throw new NotSupportedException("resources contains a bag of values");
										}
										requestedResourceString = attribute.AttributeValues[0].Contents;
									}
								}
							}
							if (!string.IsNullOrEmpty(requestedResourceString))
							{
								requestedResource = new Uri(requestedResourceString);
							}
						}

						// Iterate through the policy resources evaluating each resource in the context document request 
						foreach (string resourceName in policy.AllResources)
						{
							bool mustEvaluate = false;
							if (resource.IsHierarchical)
							{
								//Validate if the resource is hierarchically desdendant or children 
								//of the requested resource
								Uri policyResource = new Uri(resourceName);

								Debug.Assert(requestedResource != null, "requestedResource != null");
								if (!(mustEvaluate = requestedResource.Equals(policyResource)))
								{
									// Perform the hierarchical evaluation
									if (resource.ResourceScopeValue == ctx.ResourceScope.Children)
									{
										mustEvaluate = typ.AnyUri.IsChildrenOf(requestedResource, policyResource);
									}
									else if (resource.ResourceScopeValue == ctx.ResourceScope.Descendants)
									{
										mustEvaluate = typ.AnyUri.IsDescendantOf(requestedResource, policyResource);
									}
								}

								if (mustEvaluate)
								{
									foreach (ctx.AttributeElementReadWrite attribute in context.CurrentResource.Attributes)
									{
										if (attribute.AttributeId == Consts.ContextSchema.ResourceElement.ResourceId)
										{
											attribute.AttributeValues[0].Contents = resourceName;
											break;
										}
									}
								}
							}
							else
							{
								context.CurrentResource = resource;
								mustEvaluate = true;
							}

							if (mustEvaluate)
							{
								// Evaluates the policy set
								Decision decision = policy.Evaluate(context);

								// Create a status code using the policy execution state
								ctx.StatusCodeElement scode;
								if (context.IsMissingAttribute)
								{
									scode = new ctx.StatusCodeElement(
										Consts.ContextSchema.StatusCodes.MissingAttribute, null, policyDocument.Version);
								}
								else if (context.ProcessingError)
								{
									scode = new ctx.StatusCodeElement(
										Consts.ContextSchema.StatusCodes.ProcessingError, null, policyDocument.Version);
								}
								else
								{
									scode = new ctx.StatusCodeElement(
										Consts.ContextSchema.StatusCodes.Ok, null, policyDocument.Version);
								}

								//Stop the iteration if there is not a hierarchical request
								if (!resource.IsHierarchical)
								{
									// Ussually when a single resource is requested the ResourceId is not specified in the result
									var oblig = policy as IObligationsContainer;
									contextDocument.Response.Results.Add(
										new ctx.ResultElement("", decision,
											new ctx.StatusElement(scode, "", "", policyDocument.Version), oblig.Obligations, policyDocument.Version));
									break;
								}
								else
								{
									// Adding a resource for each requested resource, using the resourceName as the resourceId of the result
									var oblig = policy as IObligationsContainer;
									contextDocument.Response.Results.Add(
										new ctx.ResultElement(resourceName, decision,
											new ctx.StatusElement(scode, "", "", policyDocument.Version), oblig.Obligations, policyDocument.Version));
								}
							} // if( mustEvaluate )
						} // foreach( string resourceName in policy.AllResources )
					}
				} //if( policy != null )
			}
			catch (EvaluationException e)
			{
				// If a validation error was found a response is created with the syntax error message
				contextDocument.Response =
					new ctx.ResponseElement(
						new ctx.ResultElement[] {
							new ctx.ResultElement( 
								null, 
								Decision.Indeterminate, 
								new ctx.StatusElement( 
									new ctx.StatusCodeElement(Consts.ContextSchema.StatusCodes.ProcessingError, null, policyDocument.Version ), 
									e.Message, 
									e.StackTrace, policyDocument.Version ), 
								null, policyDocument.Version ) },
					policyDocument.Version);
			}

			return contextDocument.Response;
		}


		/// <summary>
		/// Get the function for the id specified
		/// </summary>
		/// <param name="functionId">The function id</param>
		/// <returns>The function from the inner functions repository.</returns>
		public static IFunction GetFunction(string functionId)
		{
			// Search in the internal function list.
			IFunction fun = _functions[functionId];
			if (fun != null)
			{
				return fun;
			}

			// Search the repositories.
			foreach (IFunctionRepository rep in ConfigurationRoot.Config.FunctionRepositories)
			{
				fun = rep.GetFunction(functionId);
				if (fun != null)
				{
					return fun;
				}
			}

			// Function not found
			return null;
		}

		/// <summary>
		/// Resolves the AttributeSelector in the context document using the XPath sentence.
		/// </summary>
		/// <param name="context">The evaluation context instance.</param>
		/// <param name="attributeSelector">The attribute selector.</param>
		/// <returns>A bag of values with the contents of the node.</returns>
		public static BagValue Resolve(EvaluationContext context, pol.AttributeSelectorElement attributeSelector)
		{
			var bagValue = new BagValue(GetDataType(attributeSelector.DataType));
			var content = (ctx.ResourceContentElement)context.CurrentResource.ResourceContent;
			if (content != null)
			{
				XmlDocument doc = context.ContextDocument.XmlDocument;
				if (context.ContextDocument.XmlNamespaceManager == null)
				{
					context.ContextDocument.AddNamespaces(context.PolicyDocument.Namespaces);
				}
				try
				{
					string xpath = attributeSelector.RequestContextPath;
					Debug.Assert(doc.DocumentElement != null, "doc.DocumentElement != null");
					XmlNodeList nodeList = doc.DocumentElement.SelectNodes(xpath, context.ContextDocument.XmlNamespaceManager);
					if (nodeList != null)
					{
						foreach (XmlNode node in nodeList)
						{
							var ave =
								new pol.AttributeValueElement(
								attributeSelector.DataType,
								node.InnerText,
								attributeSelector.SchemaVersion);
							bagValue.Add(ave);
						}
					}
				}
				catch (XPathException e)
				{
					context.Trace("ERR: {0}", e.Message);
					bagValue = new BagValue(GetDataType(attributeSelector.DataType));
				}
			}
			return bagValue;
		}

		/// <summary>
		/// Resolves the PolicySetReferenceId using the policy repository
		/// </summary>
		/// <param name="policyReference">The policySet reference</param>
		/// <returns>The policySet found</returns>
		public static pol.PolicySetElement Resolve(pol.PolicySetIdReferenceElement policyReference)
		{
			if (ConfigurationRoot.Config != null)
			{
				// Search for attributes in the configured repositories
				foreach (IPolicyRepository repository in ConfigurationRoot.Config.PolicyRepositories)
				{
					pol.PolicySetElement policySet = repository.GetPolicySet(policyReference);
					if (policySet != null)
					{
						return policySet;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Resolves the PolicyReferenceId using the policy repository
		/// </summary>
		/// <param name="policyReference">The policy reference</param>
		/// <returns>The policy found</returns>
		public static pol.PolicyElement Resolve(pol.PolicyIdReferenceElement policyReference)
		{
			if (ConfigurationRoot.Config == null) return null;
			// Search for attributes in the configured repositories
			foreach (IPolicyRepository repository in ConfigurationRoot.Config.PolicyRepositories)
			{
				pol.PolicyElement policy = repository.GetPolicy(policyReference);
				if (policy != null)
				{
					return policy;
				}
			}
			return null;
		}

		/// <summary>
		/// Solves the attribute designator in the context document using the attribute designator type
		/// </summary>
		/// <param name="context">The evaluation context instance.</param>
		/// <param name="attributeDesignator">The attribute designator to resolve</param>
		/// <returns>A bag value with the values found in the context document</returns>
		public static BagValue Resolve(EvaluationContext context, pol.AttributeDesignatorBase attributeDesignator)
		{
			if (attributeDesignator is SubjectAttributeDesignatorElement)
			{
				if (context.ContextDocument.Request != null && context.ContextDocument.Request.Subjects != null)
				{
					var bag = new BagValue(GetDataType(attributeDesignator.DataType));
					foreach (ctx.SubjectElement subject in context.ContextDocument.Request.Subjects)
					{
						if (((SubjectAttributeDesignatorElement)attributeDesignator).SubjectCategory == null ||
							((SubjectAttributeDesignatorElement)attributeDesignator).SubjectCategory == subject.SubjectCategory)
						{
							foreach (ctx.AttributeElement attrib in FindAttribute(context, attributeDesignator, subject).Elements)
							{
								bag.Add(attrib);
							}
						}
					}
					return bag;
				}
			}
			else if (attributeDesignator is ResourceAttributeDesignatorElement)
			{
				if (context.ContextDocument.Request != null && context.CurrentResource != null)
				{
					return FindAttribute(context, attributeDesignator, context.CurrentResource);
				}
				else
				{
					return BagValue.Empty;
				}
			}
			else if (attributeDesignator is ActionAttributeDesignatorElement)
			{
				if (context.ContextDocument.Request != null && context.ContextDocument.Request.Action != null)
				{
					return FindAttribute(context, attributeDesignator, context.ContextDocument.Request.Action);
				}
				else
				{
					return BagValue.Empty;
				}
			}
			else if (attributeDesignator is EnvironmentAttributeDesignatorElement)
			{
				if (context.ContextDocument.Request != null && context.ContextDocument.Request.Environment != null)
				{
					return FindAttribute(context, attributeDesignator, context.ContextDocument.Request.Environment);
				}
				else
				{
					return BagValue.Empty;
				}
			}
			throw new EvaluationException(Properties.Resource.exc_invalid_attribute_designator);
		}

		/// <summary>
		/// Search for the attribute in the context target item using the attribute designator specified.
		/// </summary>
		/// <param name="context">The evaluation context instance.</param>
		/// <param name="attributeDesignator">The attribute designator instance.</param>
		/// <param name="targetItem">The target item to search in.</param>
		/// <returns>A bag value with the values of the attributes found.</returns>
		public static BagValue FindAttribute(EvaluationContext context, pol.AttributeDesignatorBase attributeDesignator, ctx.TargetItemBase targetItem)
		{
			var bag = new BagValue(GetDataType(attributeDesignator.DataType));
			foreach (ctx.AttributeElement attribute in targetItem.Attributes)
			{
				if (attribute.Match(attributeDesignator))
				{
					context.Trace("Adding target item attribute designator: {0}", attribute.ToString());
					bag.Add(attribute);
				}
			}
			return bag;
		}

		/// <summary>
		/// Resolves the attribute reference defined within the given match.
		/// </summary>
		/// <param name="context">The evaluation context instance.</param>
		/// <param name="match">The target item match.</param>
		/// <param name="contextTargetItem">The context target item.</param>
		/// <returns>The context attribute.</returns>
		public static Context.AttributeElement Resolve(EvaluationContext context, TargetMatchBaseReadWrite match, ctx.TargetItemBase contextTargetItem)
		{
			Context.AttributeElementReadWrite attribute = null;
			if (match.AttributeReference is pol.AttributeDesignatorBase)
			{
				var attrDesig = (pol.AttributeDesignatorBase)match.AttributeReference;
				context.Trace("Looking for attribute: {0}", attrDesig.AttributeId);
				foreach (Context.AttributeElementReadWrite tempAttribute in contextTargetItem.Attributes)
				{
					if (tempAttribute.Match(attrDesig))
					{
						attribute = tempAttribute;
						break;
					}
				}

				if (attribute == null)
				{
					context.Trace("Attribute not found, loading searching an external repository");
					attribute = GetAttribute(context, attrDesig);
				}
			}
			else if (match.AttributeReference is pol.AttributeSelectorElement)
			{
				var attrSelec = (pol.AttributeSelectorElement)match.AttributeReference;
				var content = (ctx.ResourceContentElement)((ctx.ResourceElement)contextTargetItem).ResourceContent;
				if (content != null)
				{
					XmlDocument doc = context.ContextDocument.XmlDocument;

					if (context.ContextDocument.XmlNamespaceManager == null)
					{
						context.ContextDocument.AddNamespaces(context.PolicyDocument.Namespaces);
					}

					string xpath = attrSelec.RequestContextPath;
					try
					{
						Debug.Assert(doc.DocumentElement != null, "doc.DocumentElement != null");
						XmlNode node = doc.DocumentElement.SelectSingleNode(xpath, context.ContextDocument.XmlNamespaceManager);
						if (node != null)
						{
							attribute = new ctx.AttributeElement(null, attrSelec.DataType, null, null, node.InnerText, attrSelec.SchemaVersion);
						}
					}
					catch (XPathException e)
					{
						context.Trace("ERR: {0}", e.Message);
						context.ProcessingError = true;
					}
				}
			}

			if (!context.ProcessingError && attribute == null && match.AttributeReference.MustBePresent)
			{
				context.IsMissingAttribute = true;
				context.AddMissingAttribute(match.AttributeReference);
			}

			if (attribute != null)
			{
				return new Context.AttributeElement(attribute.AttributeId, attribute.DataType, attribute.Issuer, attribute.IssueInstant,
					attribute.Value, attribute.SchemaVersion);
			}
			return null;
		}

		/// <summary>
		/// Resolve the attribute desingator that can't be found in the context document
		/// </summary>
		/// <param name="context">The evaluation context instance.</param>
		/// <param name="designator">The attribute designator instance</param>
		public static ctx.AttributeElement GetAttribute(EvaluationContext context, pol.AttributeDesignatorBase designator)
		{
			// Resolve internal attributes
			switch (designator.AttributeId)
			{
				case Consts.ContextSchema.EnvironmentAttributes.CurrentDate:
					return new ctx.AttributeElement(
						designator.AttributeId,
						Consts.Schema1.InternalDataTypes.XsdDate,
						null,
						null,
						XmlConvert.ToString(DateTime.Now, "yyyy-MM-dd"),
						designator.SchemaVersion);
				case Consts.ContextSchema.EnvironmentAttributes.CurrentTime:
					return new ctx.AttributeElement(
						designator.AttributeId,
						Consts.Schema1.InternalDataTypes.XsdTime,
						null,
						null,
						XmlConvert.ToString(DateTime.Now, "HH:mm:sszzzzzz"),
						designator.SchemaVersion);
				case Consts.ContextSchema.EnvironmentAttributes.CurrentDateTime:
					return new ctx.AttributeElement(
						designator.AttributeId,
						Consts.Schema1.InternalDataTypes.XsdDateTime,
						null,
						null,
						XmlConvert.ToString(DateTime.Now, "yyyy-MM-ddTHH:mm:sszzzzzz"),
						designator.SchemaVersion);
				default:
					{
						if (Configuration.ConfigurationRoot.Config != null)
						{
							// Search for attributes in the configured repositories
							foreach (IAttributeRepository repository in Configuration.ConfigurationRoot.Config.AttributeRepositories)
							{
								ctx.AttributeElement attribute = repository.GetAttribute(context, designator);
								if (attribute != null)
								{
									return attribute;
								}
							}
						}
						return null;
					}
			}
		}

		/// <summary>
		/// Evaluates a function and also validates it's return value and parameter data types
		/// </summary>
		/// <param name="context">The evaluation context instance.</param>
		/// <param name="functionInstance">The function to call</param>
		/// <param name="arguments">The function arguments</param>
		/// <returns>The return value of the function</returns>
		public static EvaluationValue EvaluateFunction(EvaluationContext context, IFunction functionInstance, params IFunctionParameter[] arguments)
		{
			if (context == null) throw new ArgumentNullException("context");
			// If the caller is in a missing attribute state the function should not be called
			if (context.IsMissingAttribute)
			{
				context.Trace("There's a missing attribute in the parameters"); 
				return EvaluationValue.Indeterminate;
			}
			else
			{
				// Validate function defined arguments
				int functionArgumentIdx;
				for (functionArgumentIdx = 0; functionArgumentIdx < functionInstance.Arguments.Length; functionArgumentIdx++)
				{
					// Validate the value is not an Indeterminate value
					if (arguments[functionArgumentIdx] is EvaluationValue &&
						((EvaluationValue)arguments[functionArgumentIdx]).IsIndeterminate)
					{
						if (!context.IsMissingAttribute)
						{
							context.ProcessingError = true;
						}
						context.Trace("There's a parameter with Indeterminate value");
						return EvaluationValue.Indeterminate;
					}

					// Compare the function and the value data type
					if (((functionInstance.Arguments[functionArgumentIdx] != arguments[functionArgumentIdx].GetType(context)) &&
						((functionInstance.Arguments[functionArgumentIdx] != DataTypeDescriptor.Bag) && (arguments[functionArgumentIdx] is BagValue))))
					{
						context.ProcessingError = true;
						context.Trace("There's a parameter with an invalid datatype"); 
						return EvaluationValue.Indeterminate;
					}
				}

				//If the function supports variable arguments, the last datatype is used to validate the
				//rest of the parameters
				if (functionInstance.VarArgs)
				{
					functionArgumentIdx--;
					for (int argumentValueIdx = functionArgumentIdx; argumentValueIdx < arguments.Length; argumentValueIdx++)
					{
						// Validate the value is not an Indeterminate value
						if (arguments[argumentValueIdx] is EvaluationValue && ((EvaluationValue)arguments[argumentValueIdx]).IsIndeterminate)
						{
							if (!context.IsMissingAttribute)
							{
								context.ProcessingError = true;
							}
							context.Trace("There's a parameter with Indeterminate value"); 
							return EvaluationValue.Indeterminate;
						}

						// Compare the function and the value data type
						if ((functionInstance.Arguments[functionArgumentIdx] != arguments[argumentValueIdx].GetType(context)) &&
							((arguments[argumentValueIdx] is BagValue) && (functionInstance.Arguments[functionArgumentIdx] != DataTypeDescriptor.Bag)))
						{
							context.ProcessingError = true;
							context.Trace("There's a parameter with an invalid datatype"); 
							return EvaluationValue.Indeterminate;
						}
					}
				}

				var sb = new StringBuilder();

				// Call the function in a controlled evironment to capture any exception
				try
				{
					sb.Append(functionInstance.Id);
					sb.Append("( ");
					bool isFirst = true;
					foreach (IFunctionParameter param in arguments)
					{
						if (isFirst)
						{
							isFirst = false;
						}
						else
						{
							sb.Append(", ");
						}
						sb.Append(param.ToString());
					}
					sb.Append(" )");
					sb.Append(" = ");

					EvaluationValue returnValue = functionInstance.Evaluate(context, arguments);

					sb.Append(returnValue.ToString());
					context.Trace(sb.ToString());

					return returnValue;
				}
				catch (EvaluationException e)
				{
					context.Trace(sb.ToString());
					context.ProcessingError = true;
					context.Trace("Error: {0}", e.Message); 
					return EvaluationValue.Indeterminate;
				}
			}
		}

		#endregion

		#region Private members

		/// <summary>
		/// Prepare the evaluation engine with the function instances.
		/// </summary>
		private static void Prepare()
		{
			if (_functions != null && _dataTypes != null)
			{
				return;
			}
			_functions = new Dictionary<string, IFunction>();
			_dataTypes = new Dictionary<string, IDataType>();

			_functions.Add(Consts.Schema1.InternalFunctions.AnyUriEqual, new AnyUriEqual());
			_functions.Add(Consts.Schema1.InternalFunctions.AnyUriBagSize, new AnyUriBagSize());
			_functions.Add(Consts.Schema1.InternalFunctions.AnyUriOneAndOnly, new AnyUriOneAndOnly());
			_functions.Add(Consts.Schema1.InternalFunctions.AnyUriIsIn, new AnyUriIsIn());
			_functions.Add(Consts.Schema1.InternalFunctions.AnyUriBag, new AnyUriBag());
			_functions.Add(Consts.Schema1.InternalFunctions.AnyOf, new AnyOf());
			_functions.Add(Consts.Schema1.InternalFunctions.AnyOfAny, new AnyOfAny());
			_functions.Add(Consts.Schema1.InternalFunctions.AnyOfAll, new AnyOfAll());
			_functions.Add(Consts.Schema1.InternalFunctions.AnyUriAtLeastOneMemberOf, new AnyUriAtLeastOneMemberOf());
			_functions.Add(Consts.Schema1.InternalFunctions.AnyUriIntersection, new AnyUriIntersection());
			_functions.Add(Consts.Schema1.InternalFunctions.AnyUriSetEquals, new AnyUriSetEquals());
			_functions.Add(Consts.Schema1.InternalFunctions.AnyUriSubset, new AnyUriSubset());
			_functions.Add(Consts.Schema1.InternalFunctions.AnyUriUnion, new AnyUriUnion());
			_functions.Add(Consts.Schema1.InternalFunctions.AllOf, new AllOf());
			_functions.Add(Consts.Schema1.InternalFunctions.AllOfAny, new AllOfAny());
			_functions.Add(Consts.Schema1.InternalFunctions.AllOfAll, new AllOfAll());
			_functions.Add(Consts.Schema1.InternalFunctions.And, new AndFunction());
			_functions.Add(Consts.Schema1.InternalFunctions.Base64BinaryEqual, new Base64BinaryEqual());
			_functions.Add(Consts.Schema1.InternalFunctions.Base64BinaryBagSize, new Base64BinaryBagSize());
			_functions.Add(Consts.Schema1.InternalFunctions.Base64BinaryBag, new Base64BinaryBag());
			_functions.Add(Consts.Schema1.InternalFunctions.Base64BinaryIsIn, new Base64BinaryIsIn());
			_functions.Add(Consts.Schema1.InternalFunctions.Base64BinaryOneAndOnly, new Base64BinaryOneAndOnly());
			_functions.Add(Consts.Schema1.InternalFunctions.Base64BinaryAtLeastOneMemberOf, new Base64BinaryAtLeastOneMemberOf());
			_functions.Add(Consts.Schema1.InternalFunctions.Base64BinaryIntersection, new Base64BinaryIntersection());
			_functions.Add(Consts.Schema1.InternalFunctions.Base64BinarySetEquals, new Base64BinarySetEquals());
			_functions.Add(Consts.Schema1.InternalFunctions.Base64BinarySubset, new Base64BinarySubset());
			_functions.Add(Consts.Schema1.InternalFunctions.Base64BinaryUnion, new Base64BinaryUnion());
			_functions.Add(Consts.Schema1.InternalFunctions.BooleanEqual, new BooleanEqual());
			_functions.Add(Consts.Schema1.InternalFunctions.BooleanBagSize, new BooleanBagSize());
			_functions.Add(Consts.Schema1.InternalFunctions.BooleanOneAndOnly, new BooleanOneAndOnly());
			_functions.Add(Consts.Schema1.InternalFunctions.BooleanIsIn, new BooleanIsIn());
			_functions.Add(Consts.Schema1.InternalFunctions.BooleanBag, new BooleanBag());
			_functions.Add(Consts.Schema1.InternalFunctions.BooleanAtLeastOneMemberOf, new BooleanAtLeastOneMemberOf());
			_functions.Add(Consts.Schema1.InternalFunctions.BooleanIntersection, new BooleanIntersection());
			_functions.Add(Consts.Schema1.InternalFunctions.BooleanSetEquals, new BooleanSetEquals());
			_functions.Add(Consts.Schema1.InternalFunctions.BooleanSubset, new BooleanSubset());
			_functions.Add(Consts.Schema1.InternalFunctions.BooleanUnion, new BooleanUnion());
			_functions.Add(Consts.Schema1.InternalFunctions.DateEqual, new DateEqual());
			_functions.Add(Consts.Schema1.InternalFunctions.DateBagSize, new DateBagSize());
			_functions.Add(Consts.Schema1.InternalFunctions.DateBag, new DateBag());
			_functions.Add(Consts.Schema1.InternalFunctions.DateIsIn, new DateIsIn());
			_functions.Add(Consts.Schema1.InternalFunctions.DateOneAndOnly, new DateOneAndOnly());
			_functions.Add(Consts.Schema1.InternalFunctions.DateGreaterThanOrEqual, new DateGreaterThanOrEqual());
			_functions.Add(Consts.Schema1.InternalFunctions.DateGreaterThan, new DateGreaterThan());
			_functions.Add(Consts.Schema1.InternalFunctions.DateLessThanOrEqual, new DateLessThanOrEqual());
			_functions.Add(Consts.Schema1.InternalFunctions.DateLessThan, new DateLessThan());
			_functions.Add(Consts.Schema1.InternalFunctions.DateAddYearMonthDuration, new DateAddYearMonthDuration());
			_functions.Add(Consts.Schema1.InternalFunctions.DateSubtractYearMonthDuration, new DateSubtractYearMonthDuration());
			_functions.Add(Consts.Schema1.InternalFunctions.DateAtLeastOneMemberOf, new DateAtLeastOneMemberOf());
			_functions.Add(Consts.Schema1.InternalFunctions.DateIntersection, new DateIntersection());
			_functions.Add(Consts.Schema1.InternalFunctions.DateSetEquals, new DateSetEquals());
			_functions.Add(Consts.Schema1.InternalFunctions.DateSubset, new DateSubset());
			_functions.Add(Consts.Schema1.InternalFunctions.DateUnion, new DateUnion());
			_functions.Add(Consts.Schema1.InternalFunctions.DateTimeBagSize, new Functions.DateTimeDataType.BagSize());
			_functions.Add(Consts.Schema1.InternalFunctions.DateTimeBag, new Functions.DateTimeDataType.Bag());
			_functions.Add(Consts.Schema1.InternalFunctions.DateTimeEqual, new Functions.DateTimeDataType.Equal());
			_functions.Add(Consts.Schema1.InternalFunctions.DateTimeOneAndOnly, new Functions.DateTimeDataType.OneAndOnly());
			_functions.Add(Consts.Schema1.InternalFunctions.DateTimeIsIn, new Functions.DateTimeDataType.IsIn());
			_functions.Add(Consts.Schema1.InternalFunctions.DateTimeGreaterThanOrEqual, new Functions.DateTimeDataType.GreaterThanOrEqual());
			_functions.Add(Consts.Schema1.InternalFunctions.DateTimeGreaterThan, new Functions.DateTimeDataType.GreaterThan());
			_functions.Add(Consts.Schema1.InternalFunctions.DateTimeLessThanOrEqual, new Functions.DateTimeDataType.LessThanOrEqual());
			_functions.Add(Consts.Schema1.InternalFunctions.DateTimeLessThan, new Functions.DateTimeDataType.LessThan());
			_functions.Add(Consts.Schema1.InternalFunctions.DateTimeAddDaytimeDuration, new Functions.DateTimeDataType.AddDaytimeDuration());
			_functions.Add(Consts.Schema1.InternalFunctions.DateTimeAddYearMonthDuration, new Functions.DateTimeDataType.AddYearMonthDuration());
			_functions.Add(Consts.Schema1.InternalFunctions.DateTimeSubtractDaytimeDuration, new Functions.DateTimeDataType.SubtractDaytimeDuration());
			_functions.Add(Consts.Schema1.InternalFunctions.DateTimeSubtractYearMonthDuration, new Functions.DateTimeDataType.SubtractYearMonthDuration());
			_functions.Add(Consts.Schema1.InternalFunctions.DateTimeAtLeastOneMemberOf, new Functions.DateTimeDataType.AtLeastOneMemberOf());
			_functions.Add(Consts.Schema1.InternalFunctions.DateTimeIntersection, new Functions.DateTimeDataType.Intersection());
			_functions.Add(Consts.Schema1.InternalFunctions.DateTimeSetEquals, new Functions.DateTimeDataType.SetEquals());
			_functions.Add(Consts.Schema1.InternalFunctions.DateTimeSubset, new Functions.DateTimeDataType.Subset());
			_functions.Add(Consts.Schema1.InternalFunctions.DateTimeUnion, new Functions.DateTimeDataType.Union());
			_functions.Add(Consts.Schema1.InternalFunctions.DaytimeDurationEqual, new DaytimeDurationEqual());
			_functions.Add(Consts.Schema1.InternalFunctions.DaytimeDurationBag, new DaytimeDurationBag());
			_functions.Add(Consts.Schema1.InternalFunctions.DaytimeDurationBagSize, new DaytimeDurationBagSize());
			_functions.Add(Consts.Schema1.InternalFunctions.DaytimeDurationIsIn, new DaytimeDurationIsIn());
			_functions.Add(Consts.Schema1.InternalFunctions.DaytimeDurationOneAndOnly, new DaytimeDurationOneAndOnly());
			_functions.Add(Consts.Schema1.InternalFunctions.DaytimeDurationAtLeastOneMemberOf, new DaytimeDurationAtLeastOneMemberOf());
			_functions.Add(Consts.Schema1.InternalFunctions.DaytimeDurationIntersection, new DaytimeDurationIntersection());
			_functions.Add(Consts.Schema1.InternalFunctions.DaytimeDurationSetEquals, new DaytimeDurationSetEquals());
			_functions.Add(Consts.Schema1.InternalFunctions.DaytimeDurationSubset, new DaytimeDurationSubset());
			_functions.Add(Consts.Schema1.InternalFunctions.DaytimeDurationUnion, new DaytimeDurationUnion());
			_functions.Add(Consts.Schema1.InternalFunctions.DoubleGreaterThanOrEqual, new DoubleGreaterThanOrEqual());
			_functions.Add(Consts.Schema1.InternalFunctions.DoubleGreaterThan, new DoubleGreaterThan());
			_functions.Add(Consts.Schema1.InternalFunctions.DoubleLessThanOrEqual, new DoubleLessThanOrEqual());
			_functions.Add(Consts.Schema1.InternalFunctions.DoubleLessThan, new DoubleLessThan());
			_functions.Add(Consts.Schema1.InternalFunctions.DoubleOneAndOnly, new DoubleOneAndOnly());
			_functions.Add(Consts.Schema1.InternalFunctions.DoubleAdd, new DoubleAdd());
			_functions.Add(Consts.Schema1.InternalFunctions.DoubleMultiply, new DoubleMultiply());
			_functions.Add(Consts.Schema1.InternalFunctions.DoubleSubtract, new DoubleSubtract());
			_functions.Add(Consts.Schema1.InternalFunctions.DoubleDivide, new DoubleDivide());
			_functions.Add(Consts.Schema1.InternalFunctions.DoubleAbs, new DoubleAbs());
			_functions.Add(Consts.Schema1.InternalFunctions.DoubleToInteger, new DoubleToInteger());
			_functions.Add(Consts.Schema1.InternalFunctions.DoubleEqual, new DoubleEqual());
			_functions.Add(Consts.Schema1.InternalFunctions.DoubleBagSize, new DoubleBagSize());
			_functions.Add(Consts.Schema1.InternalFunctions.DoubleBag, new DoubleBag());
			_functions.Add(Consts.Schema1.InternalFunctions.DoubleIsIn, new DoubleIsIn());
			_functions.Add(Consts.Schema1.InternalFunctions.DoubleAtLeastOneMemberOf, new DoubleAtLeastOneMemberOf());
			_functions.Add(Consts.Schema1.InternalFunctions.DoubleIntersection, new DoubleIntersection());
			_functions.Add(Consts.Schema1.InternalFunctions.DoubleSetEquals, new DoubleSetEquals());
			_functions.Add(Consts.Schema1.InternalFunctions.DoubleSubset, new DoubleSubset());
			_functions.Add(Consts.Schema1.InternalFunctions.DoubleUnion, new DoubleUnion());
			_functions.Add(Consts.Schema1.InternalFunctions.Floor, new Floor());
			_functions.Add(Consts.Schema1.InternalFunctions.HexBinaryEqual, new HexBinaryEqual());
			_functions.Add(Consts.Schema1.InternalFunctions.HexBinaryBagSize, new HexBinaryBagSize());
			_functions.Add(Consts.Schema1.InternalFunctions.HexBinaryBag, new HexBinaryBag());
			_functions.Add(Consts.Schema1.InternalFunctions.HexBinaryIsIn, new HexBinaryIsIn());
			_functions.Add(Consts.Schema1.InternalFunctions.HexBinaryOneAndOnly, new HexBinaryOneAndOnly());
			_functions.Add(Consts.Schema1.InternalFunctions.HexBinaryAtLeastOneMemberOf, new HexBinaryAtLeastOneMemberOf());
			_functions.Add(Consts.Schema1.InternalFunctions.HexBinaryIntersection, new HexBinaryIntersection());
			_functions.Add(Consts.Schema1.InternalFunctions.HexBinarySetEquals, new HexBinarySetEquals());
			_functions.Add(Consts.Schema1.InternalFunctions.HexBinarySubset, new HexBinarySubset());
			_functions.Add(Consts.Schema1.InternalFunctions.HexBinaryUnion, new HexBinaryUnion());
			_functions.Add(Consts.Schema1.InternalFunctions.IntegerEqual, new IntegerEqual());
			_functions.Add(Consts.Schema1.InternalFunctions.IntegerBagSize, new IntegerBagSize());
			_functions.Add(Consts.Schema1.InternalFunctions.IntegerBag, new IntegerBag());
			_functions.Add(Consts.Schema1.InternalFunctions.IntegerIsIn, new IntegerIsIn());
			_functions.Add(Consts.Schema1.InternalFunctions.IntegerGreaterThanOrEqual, new IntegerGreaterThanOrEqual());
			_functions.Add(Consts.Schema1.InternalFunctions.IntegerGreaterThan, new IntegerGreaterThan());
			_functions.Add(Consts.Schema1.InternalFunctions.IntegerLessThanOrEqual, new IntegerLessThanOrEqual());
			_functions.Add(Consts.Schema1.InternalFunctions.IntegerLessThan, new IntegerLessThan());
			_functions.Add(Consts.Schema1.InternalFunctions.IntegerSubtract, new IntegerSubtract());
			_functions.Add(Consts.Schema1.InternalFunctions.IntegerAdd, new IntegerAdd());
			_functions.Add(Consts.Schema1.InternalFunctions.IntegerMultiply, new IntegerMultiply());
			_functions.Add(Consts.Schema1.InternalFunctions.IntegerDivide, new IntegerDivide());
			_functions.Add(Consts.Schema1.InternalFunctions.IntegerMod, new IntegerMod());
			_functions.Add(Consts.Schema1.InternalFunctions.IntegerAbs, new IntegerAbs());
			_functions.Add(Consts.Schema1.InternalFunctions.IntegerToDouble, new IntegerToDouble());
			_functions.Add(Consts.Schema1.InternalFunctions.IntegerOneAndOnly, new IntegerOneAndOnly());
			_functions.Add(Consts.Schema1.InternalFunctions.IntegerAtLeastOneMemberOf, new IntegerAtLeastOneMemberOf());
			_functions.Add(Consts.Schema1.InternalFunctions.IntegerIntersection, new IntegerIntersection());
			_functions.Add(Consts.Schema1.InternalFunctions.IntegerSetEquals, new IntegerSetEquals());
			_functions.Add(Consts.Schema1.InternalFunctions.IntegerSubset, new IntegerSubset());
			_functions.Add(Consts.Schema1.InternalFunctions.IntegerUnion, new IntegerUnion());
			_functions.Add(Consts.Schema1.InternalFunctions.Map, new MapFunction());
			_functions.Add(Consts.Schema1.InternalFunctions.Not, new NotFunction());
			_functions.Add(Consts.Schema1.InternalFunctions.Nof, new NofFunction());
			_functions.Add(Consts.Schema1.InternalFunctions.Or, new OrFunction());
			_functions.Add(Consts.Schema1.InternalFunctions.RegexpStringMatch, new StringRegexpMatch());
			_functions.Add(Consts.Schema1.InternalFunctions.Rfc822NameEqual, new Rfc822NameEqual());
			_functions.Add(Consts.Schema1.InternalFunctions.Rfc822NameBagSize, new Rfc822NameBagSize());
			_functions.Add(Consts.Schema1.InternalFunctions.Rfc822NameBag, new Rfc822NameBag());
			_functions.Add(Consts.Schema1.InternalFunctions.Rfc822NameIsIn, new Rfc822NameIsIn());
			_functions.Add(Consts.Schema1.InternalFunctions.Rfc822NameMatch, new Rfc822NameMatch());
			_functions.Add(Consts.Schema1.InternalFunctions.Rfc822NameOneAndOnly, new Rfc822NameOneAndOnly());
			_functions.Add(Consts.Schema1.InternalFunctions.Rfc822NameAtLeastOneMemberOf, new Rfc822NameAtLeastOneMemberOf());
			_functions.Add(Consts.Schema1.InternalFunctions.Rfc822NameIntersection, new Rfc822NameIntersection());
			_functions.Add(Consts.Schema1.InternalFunctions.Rfc822NameSetEquals, new Rfc822NameSetEquals());
			_functions.Add(Consts.Schema1.InternalFunctions.Rfc822NameSubset, new Rfc822NameSubset());
			_functions.Add(Consts.Schema1.InternalFunctions.Rfc822NameUnion, new Rfc822NameUnion());
			_functions.Add(Consts.Schema1.InternalFunctions.Round, new Round());
			_functions.Add(Consts.Schema1.InternalFunctions.StringEqual, new StringEqual());
			_functions.Add(Consts.Schema1.InternalFunctions.StringOneAndOnly, new StringOneAndOnly());
			_functions.Add(Consts.Schema1.InternalFunctions.StringIsIn, new StringIsIn());
			_functions.Add(Consts.Schema1.InternalFunctions.StringBagSize, new StringBagSize());
			_functions.Add(Consts.Schema1.InternalFunctions.StringBag, new StringBag());
			_functions.Add(Consts.Schema1.InternalFunctions.StringGreaterThanOrEqual, new StringGreaterThanOrEqual());
			_functions.Add(Consts.Schema1.InternalFunctions.StringGreaterThan, new StringGreaterThan());
			_functions.Add(Consts.Schema1.InternalFunctions.StringLessThanOrEqual, new StringLessThanOrEqual());
			_functions.Add(Consts.Schema1.InternalFunctions.StringLessThan, new StringLessThan());
			_functions.Add(Consts.Schema1.InternalFunctions.StringNormalizeSpace, new StringNormalizeSpace());
			_functions.Add(Consts.Schema1.InternalFunctions.StringNormalizeToLowercase, new StringNormalizeToLowercase());
			_functions.Add(Consts.Schema1.InternalFunctions.StringAtLeastOneMemberOf, new StringAtLeastOneMemberOf());
			_functions.Add(Consts.Schema1.InternalFunctions.StringIntersection, new StringIntersection());
			_functions.Add(Consts.Schema1.InternalFunctions.StringSetEquals, new StringSetEquals());
			_functions.Add(Consts.Schema1.InternalFunctions.StringSubset, new StringSubset());
			_functions.Add(Consts.Schema1.InternalFunctions.StringUnion, new StringUnion());
			_functions.Add(Consts.Schema1.InternalFunctions.TimeOneAndOnly, new TimeOneAndOnly());
			_functions.Add(Consts.Schema1.InternalFunctions.TimeBagSize, new TimeBagSize());
			_functions.Add(Consts.Schema1.InternalFunctions.TimeBag, new TimeBag());
			_functions.Add(Consts.Schema1.InternalFunctions.TimeIsIn, new TimeIsIn());
			_functions.Add(Consts.Schema1.InternalFunctions.TimeEqual, new TimeEqual());
			_functions.Add(Consts.Schema1.InternalFunctions.TimeGreaterThanOrEqual, new TimeGreaterThanOrEqual());
			_functions.Add(Consts.Schema1.InternalFunctions.TimeGreaterThan, new TimeGreaterThan());
			_functions.Add(Consts.Schema1.InternalFunctions.TimeLessThanOrEqual, new TimeLessThanOrEqual());
			_functions.Add(Consts.Schema1.InternalFunctions.TimeLessThan, new TimeLessThan());
			_functions.Add(Consts.Schema1.InternalFunctions.TimeAtLeastOneMemberOf, new TimeAtLeastOneMemberOf());
			_functions.Add(Consts.Schema1.InternalFunctions.TimeIntersection, new TimeIntersection());
			_functions.Add(Consts.Schema1.InternalFunctions.TimeSetEquals, new TimeSetEquals());
			_functions.Add(Consts.Schema1.InternalFunctions.TimeSubset, new TimeSubset());
			_functions.Add(Consts.Schema1.InternalFunctions.TimeUnion, new TimeUnion());
			_functions.Add(Consts.Schema1.InternalFunctions.X500NameEqual, new X500NameEqual());
			_functions.Add(Consts.Schema1.InternalFunctions.X500NameBagSize, new X500NameBagSize());
			_functions.Add(Consts.Schema1.InternalFunctions.X500NameBag, new X500NameBag());
			_functions.Add(Consts.Schema1.InternalFunctions.X500NameIsIn, new X500NameIsIn());
			_functions.Add(Consts.Schema1.InternalFunctions.X500NameMatch, new X500NameMatch());
			_functions.Add(Consts.Schema1.InternalFunctions.X500NameOneAndOnly, new X500NameOneAndOnly());
			_functions.Add(Consts.Schema1.InternalFunctions.X500NameAtLeastOneMemberOf, new X500NameAtLeastOneMemberOf());
			_functions.Add(Consts.Schema1.InternalFunctions.X500NameIntersection, new X500NameIntersection());
			_functions.Add(Consts.Schema1.InternalFunctions.X500NameSetEquals, new X500NameSetEquals());
			_functions.Add(Consts.Schema1.InternalFunctions.X500NameSubset, new X500NameSubset());
			_functions.Add(Consts.Schema1.InternalFunctions.X500NameUnion, new X500NameUnion());
			_functions.Add(Consts.Schema1.InternalFunctions.YearMonthDurationBag, new YearMonthDurationBag());
			_functions.Add(Consts.Schema1.InternalFunctions.YearMonthDurationBagSize, new YearMonthDurationBagSize());
			_functions.Add(Consts.Schema1.InternalFunctions.YearMonthDurationEqual, new YearMonthDurationEqual());
			_functions.Add(Consts.Schema1.InternalFunctions.YearMonthDurationIsIn, new YearMonthDurationIsIn());
			_functions.Add(Consts.Schema1.InternalFunctions.YearMonthDurationOneAndOnly, new YearMonthDurationOneAndOnly());
			_functions.Add(Consts.Schema1.InternalFunctions.YearMonthDurationAtLeastOneMemberOf, new YearMonthDurationAtLeastOneMemberOf());
			_functions.Add(Consts.Schema1.InternalFunctions.YearMonthDurationIntersection, new YearMonthDurationIntersection());
			_functions.Add(Consts.Schema1.InternalFunctions.YearMonthDurationSetEquals, new YearMonthDurationSetEquals());
			_functions.Add(Consts.Schema1.InternalFunctions.YearMonthDurationSubset, new YearMonthDurationSubset());
			_functions.Add(Consts.Schema1.InternalFunctions.YearMonthDurationUnion, new YearMonthDurationUnion());
			_functions.Add(Consts.Schema1.InternalFunctions.XPathNodeCount, new XPathNodeCount());
			_functions.Add(Consts.Schema1.InternalFunctions.XPathNodeEqual, new XPathNodeEqual());
			_functions.Add(Consts.Schema1.InternalFunctions.XPathNodeMatch, new XPathNodeMatch());


			_functions.Add(Consts.Schema2.InternalFunctions.DnsNameAtLeastOneMemberOf, new DnsNameAtLeastOneMemberOf());
			_functions.Add(Consts.Schema2.InternalFunctions.DnsNameBag, new DnsNameBag());
			_functions.Add(Consts.Schema2.InternalFunctions.DnsNameBagSize, new DnsNameBagSize());
			_functions.Add(Consts.Schema2.InternalFunctions.DnsNameIntersection, new DnsNameIntersection());
			_functions.Add(Consts.Schema2.InternalFunctions.DnsNameUnion, new DnsNameUnion());
			_functions.Add(Consts.Schema2.InternalFunctions.DnsNameSetEquals, new DnsNameSetEquals());
			_functions.Add(Consts.Schema2.InternalFunctions.DnsNameOneAndOnly, new DnsNameOneAndOnly());
			_functions.Add(Consts.Schema2.InternalFunctions.DnsNameEqual, new DnsNameEqual());
			_functions.Add(Consts.Schema2.InternalFunctions.DnsNameIsIn, new DnsNameIsIn());
			_functions.Add(Consts.Schema2.InternalFunctions.DnsNameSubset, new DnsNameSubset());
			_functions.Add(Consts.Schema2.InternalFunctions.StringConcatenate, new StringConcatenate());
			_functions.Add(Consts.Schema2.InternalFunctions.UrlStringConcatenate, new UrlStringConcatenate());
			_functions.Add(Consts.Schema2.InternalFunctions.IpAddressEqual, new IpAddressEqual());
			_functions.Add(Consts.Schema2.InternalFunctions.IpAddressIsIn, new IpAddressIsIn());
			_functions.Add(Consts.Schema2.InternalFunctions.IpAddressSubset, new IpAddressSubset());
			_functions.Add(Consts.Schema2.InternalFunctions.IpAddressIntersection, new IpAddressIntersection());
			_functions.Add(Consts.Schema2.InternalFunctions.IpAddressAtLeastOneMemberOf, new IpAddressAtLeastOneMemberOf());
			_functions.Add(Consts.Schema2.InternalFunctions.IpAddressUnion, new IpAddressUnion());
			_functions.Add(Consts.Schema2.InternalFunctions.IpAddressSetEquals, new IpAddressSetEquals());
			_functions.Add(Consts.Schema2.InternalFunctions.IpAddressBag, new IpAddressBag());
			_functions.Add(Consts.Schema2.InternalFunctions.IpAddressBagSize, new IpAddressBagSize());
			_functions.Add(Consts.Schema2.InternalFunctions.IpAddressOneAndOnly, new IpAddressOneAndOnly());
			_functions.Add(Consts.Schema2.InternalFunctions.StringRegexpMatch, new StringRegexpMatch());
			_functions.Add(Consts.Schema2.InternalFunctions.IpAddressRegexpMatch, new IpAddressRegexpMatch());
			_functions.Add(Consts.Schema2.InternalFunctions.DnsNameRegexpMatch, new DnsNameRegexpMatch());
			_functions.Add(Consts.Schema2.InternalFunctions.AnyUriRegexpMatch, new AnyUriRegexpMatch());
			_functions.Add(Consts.Schema2.InternalFunctions.Rfc822NameRegexpMatch, new Rfc822NameRegexpMatch());
			_functions.Add(Consts.Schema2.InternalFunctions.X500NameRegexpMatch, new X500NameRegexpMatch());

			// Add the supported data types
			_dataTypes.Add(Consts.Schema1.InternalDataTypes.X500Name, DataTypeDescriptor.X500Name);
			_dataTypes.Add(Consts.Schema1.InternalDataTypes.Rfc822Name, DataTypeDescriptor.Rfc822Name);
			_dataTypes.Add(Consts.Schema1.InternalDataTypes.XsdString, DataTypeDescriptor.String);
			_dataTypes.Add(Consts.Schema1.InternalDataTypes.XsdBoolean, DataTypeDescriptor.Boolean);
			_dataTypes.Add(Consts.Schema1.InternalDataTypes.XsdInteger, DataTypeDescriptor.Integer);
			_dataTypes.Add(Consts.Schema1.InternalDataTypes.XsdDouble, DataTypeDescriptor.Double);
			_dataTypes.Add(Consts.Schema1.InternalDataTypes.XsdTime, DataTypeDescriptor.Time);
			_dataTypes.Add(Consts.Schema1.InternalDataTypes.XsdDate, DataTypeDescriptor.Date);
			_dataTypes.Add(Consts.Schema1.InternalDataTypes.XsdDateTime, DataTypeDescriptor.DateTime);
			_dataTypes.Add(Consts.Schema1.InternalDataTypes.XsdAnyUri, DataTypeDescriptor.AnyUri);
			_dataTypes.Add(Consts.Schema1.InternalDataTypes.XsdHexBinary, DataTypeDescriptor.HexBinary);
			_dataTypes.Add(Consts.Schema1.InternalDataTypes.XsdBase64Binary, DataTypeDescriptor.Base64Binary);
			_dataTypes.Add(Consts.Schema1.InternalDataTypes.XQueryDaytimeDuration, DataTypeDescriptor.DaytimeDuration);
			_dataTypes.Add(Consts.Schema1.InternalDataTypes.XQueryYearMonthDuration, DataTypeDescriptor.YearMonthDuration);
			_dataTypes.Add(Consts.Schema2.InternalDataTypes.DnsName, DataTypeDescriptor.DnsName);
			_dataTypes.Add(Consts.Schema2.InternalDataTypes.IpAddress, DataTypeDescriptor.IpAddress);
		}
		#endregion
	}
}
