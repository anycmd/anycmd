
namespace Anycmd.Storage.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// 表示where子句建造结果。
    /// </summary>
    public sealed class WhereClauseBuildResult
    {
        #region Public Properties
        /// <summary>
        /// 读取或设置表示where子句的字符串值 a <c>System.String</c>。
        /// </summary>
        public string WhereClause { get; set; }

        /// <summary>
        /// 读取或设置一个包含where子句的参数和参数值的 <c>Dictionary&lt;string, object&gt;</c> 对象实例。
        /// </summary>
        public Dictionary<string, object> ParameterValues { get; set; }
        #endregion

        #region Ctor
        /// <summary>
        /// 初始化一个 <c>WhereClauseBuildResult</c> 类型的对象。
        /// </summary>
        public WhereClauseBuildResult() { }

        /// <summary>
        /// 初始化一个 <c>WhereClauseBuildResult</c> 类型的对象。
        /// </summary>
        /// <param name="whereClause">表示where子句的字符串值 a <c>System.String</c>。</param>
        /// <param name="parameterValues">一个包含where子句的参数和参数值的 <c>Dictionary&lt;string, object&gt;</c> 对象实例。</param>
        public WhereClauseBuildResult(string whereClause, Dictionary<string, object> parameterValues)
        {
            WhereClause = whereClause;
            ParameterValues = parameterValues;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns a <c>System.String</c> object which represents the content of the Where Clause
        /// Build Result.
        /// </summary>
        /// <returns>A <c>System.String</c> object which represents the content of the Where Clause
        /// Build Result.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(WhereClause);
            sb.Append(Environment.NewLine);
            ParameterValues.ToList().ForEach(kvp =>
                {
                    sb.Append(string.Format("{0} = [{1}] (Type: {2})", kvp.Key, kvp.Value.ToString(), kvp.Value.GetType().FullName));
                    sb.Append(Environment.NewLine);
                });
            return sb.ToString();
        }
        #endregion
    }
}
