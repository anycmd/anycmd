
namespace Anycmd.Storage
{
    /// <summary>
    /// 表示该接口的实现类是仓库映射转换器。
    /// </summary>
    public interface IStorageMappingResolver
    {
        /// <summary>
        /// 将给定的.NET类型转换为仓库表名。
        /// </summary>
        /// <typeparam name="T">要被转换的.NET对象类型。</typeparam>
        /// <returns>仓库表名。</returns>
        string ResolveTableName<T>() where T : class, new();

        /// <summary>
        /// 通过给定.NET类型和属性名转换仓库字段名。
        /// </summary>
        /// <typeparam name="T">要被转换的.NET对象的类型。</typeparam>
        /// <param name="propertyName">属性名。</param>
        /// <returns>字段名。</returns>
        string ResolveFieldName<T>(string propertyName) where T : class, new();

        /// <summary>
        /// 判断给定的类型的给定属性名是否是被转换为仓库中的自增字段。
        /// </summary>
        /// <typeparam name="T">要被转换的.NET对象类型。</typeparam>
        /// <param name="propertyName">属性名。</param>
        /// <returns>true表示转换为自增字段。</returns>
        bool IsAutoIdentityField<T>(string propertyName) where T : class, new();
    }
}
