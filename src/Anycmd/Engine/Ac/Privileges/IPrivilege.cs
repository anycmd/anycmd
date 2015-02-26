
namespace Anycmd.Engine.Ac.Privileges
{
    using System;

    /// <summary>
    /// Ac二元组。它是9种Ac元素对象的二元组。但它区分Subject（主体）和Object（客体）从而为二元关系规定了方向。
    /// 9大Ac元素是：Account、Catalog、Role、Group、Function、Menu、AppSystem、ResourceType、Privilege。
    /// Ac二元组模型化的是9种Ac元素的两两组合，为了简化问题我们把二元中的其中一员指定为Subject，另一元指定为Object。
    /// 可以认为Subject就是Ac中的主体，主体是有主观能动性的事物，而Object是可被Subject感知的客体。
    /// 可以将主体分作两类：一是系统主体、二是用户主体。Account、Role、Catalog属于用户主体。
    /// 而Menu、Function、Group、AppSystem、ResourceType等充当主体的时候它们都是系统主体，系统主体是由开发人员管理的，系统主体是不经常变化的，
    /// 而用户主体是随着安全管理员对系统的使用而一直变化着的，中心节点与各个接入的业务节点所交换的就是用户主体类别的权限记录，而不是系统主体类别的记录。
    /// 优先实现对用户类别的主体Account、Role、Catalog的支持。
    /// <para>Subject是account，Object是role时表示授予账户该角色</para>
    /// <para>Subject是role，Object是function时表示授予该角色该功能权限</para>
    /// <para>Subject是catalog，Object是role时表示授予该目录该角色，从而该目录下的账户是逻辑地间接得到这些角色的。</para>
    /// 
    /// <para>Subject是account，Object是account时表示Object账户委托权限给Subject账户，这时Object账户外出了，被委托的Subject账户临时具有他的权限。</para>
    /// 所有的两两组合都有意义。
    /// <remarks>
    /// 权限数据交换所交换的就是它。
    /// </remarks>
    /// </summary>
    public interface IPrivilege
    {
        Guid Id { get; }
        /// <summary>
        /// 主体类型<see cref="UserAcSubjectType"/>
        /// </summary>
        string SubjectType { get; }
        /// <summary>
        /// 主体实例标识
        /// </summary>
        Guid SubjectInstanceId { get; }
        /// <summary>
        /// 客体类型<see cref="AcElementType"/>
        /// </summary>
        string ObjectType { get; }
        /// <summary>
        /// 客体实例标识
        /// </summary>
        Guid ObjectInstanceId { get; }

        string AcContentType { get; }
        string AcContent { get; }
    }
}
