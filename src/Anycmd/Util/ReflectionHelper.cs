
using System.Linq;

namespace Anycmd.Util
{
    using Engine.Host;
    using Serialization;
    using System;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// 反射助手
    /// </summary>
    public static class ReflectionHelper
    {
        #region Private Constants
        private const int InitialPrime = 23;
        private const int FactorPrime = 29;
        #endregion

        #region Extension Methods
        /// <summary>
        /// 获取给定的.NET类型的签名字符串。
        /// </summary>
        /// <param name="type">给定的.NET类型。</param>
        /// <returns>签名字符串。</returns>
        public static string GetSignature(this Type type)
        {
            var sb = new StringBuilder();

            if (type.IsGenericType)
            {
                sb.Append(type.GetGenericTypeDefinition().FullName);
                sb.Append("[");
                var i = 0;
                var genericArgs = type.GetGenericArguments();
                foreach (var genericArg in genericArgs)
                {
                    sb.Append(genericArg.GetSignature());
                    if (i != genericArgs.Length - 1)
                        sb.Append(", ");
                    i++;
                }
                sb.Append("]");
            }
            else
            {
                if (!string.IsNullOrEmpty(type.FullName))
                    sb.Append(type.FullName);
                else if (!string.IsNullOrEmpty(type.Name))
                    sb.Append(type.Name);
                else
                    sb.Append(type.ToString());

            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取给定的.NET方法的签名字符串。
        /// </summary>
        /// <param name="method">给定的.NET方法。</param>
        /// <returns>签名字符串</returns>
        public static string GetSignature(this MethodInfo method)
        {
            var sb = new StringBuilder();
            sb.Append(method.ReturnType.GetSignature());
            sb.Append(" ");
            sb.Append(method.Name);
            if (method.IsGenericMethod)
            {
                sb.Append("[");
                var genericTypes = method.GetGenericArguments();
                var i = 0;
                foreach (var genericType in genericTypes)
                {
                    sb.Append(genericType.GetSignature());
                    if (i != genericTypes.Length - 1)
                        sb.Append(", ");
                    i++;
                }
                sb.Append("]");
            }
            sb.Append("(");
            var parameters = method.GetParameters();
            if (parameters.Length > 0)
            {
                var i = 0;
                foreach (var parameter in parameters)
                {
                    sb.Append(parameter.ParameterType.GetSignature());
                    if (i != parameters.Length - 1)
                        sb.Append(", ");
                    i++;
                }
            }
            sb.Append(")");
            return sb.ToString();
        }

        /// <summary>
        /// 使用给定的.NET对象序列化器，从给定的字节数组反序列化得到给定类型的.NET对象。
        /// </summary>
        /// <param name="serializer">给定的.NET对象序列化器。</param>
        /// <param name="type">反序列化得到的对象的类型。</param>
        /// <param name="stream">数据</param>
        /// <returns>反序列化后得到的.NET对象。</returns>
        public static object Deserialize(this IObjectSerializer serializer, Type type, byte[] stream)
        {
            var deserializeMethodInfo = serializer.GetType().GetMethod("Deserialize");
            return deserializeMethodInfo.MakeGenericMethod(type).Invoke(serializer, new object[] { stream });
        }

        /// <summary>
        /// 获取给定类型的特性对象。
        /// </summary>
        /// <typeparam name="T">CustomAttribute Type</typeparam>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(this ICustomAttributeProvider provider)
            where T : Attribute
        {
            var attributes = provider.GetCustomAttributes(typeof(T), false);

            return attributes.Length > 0 ? attributes[0] as T : default(T);
        }

        /// <summary>
        /// 获取给定程序集的运行时模式。
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static JitMode GetJitMode(this Assembly assembly)
        {
            var att = assembly.GetCustomAttribute<DebuggableAttribute>();
            if (att == null)
            {
                return JitMode.Unknown;
            }
            return att.IsJITTrackingEnabled ? JitMode.Debug : JitMode.Release;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the hash code for an object based on the given array of hash
        /// codes from each property of the object.
        /// </summary>
        /// <param name="hashCodesForProperties">The array of the hash codes
        /// that are from each property of the object.</param>
        /// <returns>The hash code.</returns>
        public static int GetHashCode(params int[] hashCodesForProperties)
        {
            unchecked
            {
                return hashCodesForProperties.Aggregate(InitialPrime, (current, code) => current*FactorPrime + code);
            }
        }
        /// <summary>
        /// Generates a unique identifier represented by a <see cref="System.String"/> value
        /// with the specified length.
        /// </summary>
        /// <param name="length">The length of the identifier to be generated.</param>
        /// <returns>The unique identifier represented by a <see cref="System.String"/> value.</returns>
        public static string GetUniqueIdentifier(int length)
        {
            var maxSize = length;
            var chars = new char[62];
            string a;
            a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            chars = a.ToCharArray();
            var size = maxSize;
            var data = new byte[1];
            var crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            var result = new StringBuilder(size);
            foreach (var b in data)
            {
                result.Append(chars[b % (chars.Length - 1)]);
            }
            // Unique identifiers cannot begin with 0-9
            if (result[0] >= '0' && result[0] <= '9')
            {
                return GetUniqueIdentifier(length);
            }
            return result.ToString();
        }
        #endregion

        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <typeparam name="T">要创建对象的类型</typeparam>
        /// <param name="assemblyName">类型所在程序集名称</param>
        /// <param name="classFullName">类型名</param>
        /// <returns></returns>
        public static T CreateInstance<T>(string assemblyName, string classFullName)
        {
            try
            {
                object ect = Assembly.Load(assemblyName).CreateInstance(classFullName);
                return (T)ect;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lambda"></param>
        /// <returns></returns>
        public static MemberInfo GetProperty(LambdaExpression lambda)
        {
            Expression expression = lambda;
            for (; ; )
            {
                switch (expression.NodeType)
                {
                    case ExpressionType.Lambda:
                        expression = ((LambdaExpression)expression).Body;
                        break;
                    case ExpressionType.Convert:
                        expression = ((UnaryExpression)expression).Operand;
                        break;
                    case ExpressionType.MemberAccess:
                        var memberExpression = (MemberExpression)expression;
                        MemberInfo mi = memberExpression.Member;
                        return mi;
                    default:
                        return null;
                }
            }
        }
    }
}
