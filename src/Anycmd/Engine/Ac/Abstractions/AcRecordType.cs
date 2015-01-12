
namespace Anycmd.Engine.Ac.Abstractions
{
    /// <summary>
    /// 表示访问控制记录类型。
    /// <remarks>
    /// 该枚举成员的命名模式是SubjectType + ObjectType
    /// </remarks>
    /// </summary>
    public enum AcRecordType
    {
        Undefined,
        /// <summary>
        /// 主体是账户，客体也是账户。
        /// </summary>
        AccountAccount = 20,
        /// <summary>
        /// 主体是账户，客体是组织结构。
        /// </summary>
        AccountOrganization,
        AccountRole,
        AccountGroup,
        AccountFunction,
        AccountMenu,
        AccountAppSystem,
        AccountResourceType,
        AccountPrivilege,
        OrganizationOrganization,
        OrganizationRole,
        OrganizationGroup,
        OrganizationFunction,
        OrganizationMenu,
        OrganizationAppSystem,
        OrganizationResourceType,
        OrganizationPrivilege,
        RoleRole,
        RoleGroup,
        RoleFunction,
        RoleMenu,
        RoleAppSystem,
        RoleResourceType,
        RolePrivilege,
        GroupGroup,
        GroupFunction,
        GroupMenu,
        GroupAppSystem,
        GroupResourceType,
        GroupPrivilege,
        FunctionFunction,
        FunctionMenu,
        FunctionAppSystem,
        FunctionResourceType,
        FunctionPrivilege,
        MenuMenu,
        MenuAppSystem,
        MenuResourceType,
        MenuPrivilege,
        AppSystemAppSystem,
        AppSystemResourceType,
        AppSystemPrivilege,
        ResourceTypeResourceType,
        ResourceTypePrivilege,
        PrivilegePrivilege
    }
}
