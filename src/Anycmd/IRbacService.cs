
namespace Anycmd
{
    using Engine;
    using Engine.Ac;
    using Engine.Ac.Accounts;
    using Engine.Ac.Dsd;
    using Engine.Ac.Roles;
    using Engine.Ac.Ssd;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 访问控制是对信息系统资源的访问范围以及方式进行限制的策略。
    /// 主体：指对客体有认识和实践能力的对象。比如人、系统、服务提供者。客体：可被主体感知或想象到的任何事物。
    /// 如文件、打印机、终端、数据库记录等。对象/资源：资源是需要进行访问控制的系统资源，例如文件、打印机、终端、数据库记录等。
    /// 资源类型：基于人类发明的分类法对资源按照性质、特点、用途等作为区分的标准而做的第一次分类。
    /// 权限：对受保护的对象执行某个操作的许可。
    /// 操作：一个过程，这个过程通常有输入与输出，这个过程可能影响系统的状态也可能不影响系统的状态，是否影响系统的状态有赖于你的领域边界。
    /// 首先基于资源类型定义一类资源的操作列表，通常这就够了，但也有可能会需要针对特定对象实例定义操作列表。
    /// <remarks>
    /// 所有的Rbac标准接口都放进了一个类是希望把Rbac看成一个整体，不作根据自我理解而施加的分类。接口的使用者可以根据自己的理解再对这套接口进行分类。
    /// 比如接口的使用者可以把以上所有方法分类成User类、Role类、Privilege类等。
    /// 为IRbacService的方法分类是一件困难的事情。因为可以基于很多观察角度进行分类，不同的观察角度对应不同的分类方式。
    /// 比如可以按照Rbac标准上定义的“核心Rbac”、“层次Rbac”、“责任分离Rbac”分类，也可以按照方法的主要参与者是谁进行分类分类成
    /// User类、Role类、Privilege类、责任分离类等，还可以按照方法的功用分类成管理类、支持类、审计类。作为一套稳定规范的编程接口anycmd选择不对Rbac方法进行分类
    /// ，不分类可能是最好的分类，每一个方法都是一个元素，而元素方法是可以按照使用者的需求任意组合的。
    /// 更多信息参见../../docs/materials/GBT-Rbac.pdf
    /// </remarks>
    /// </summary>
    public interface IRbacService
    {
        /// <summary>
        /// 【核心Rbac管理函数】该命令创建一个新用户。当要待创建用户尚不存在于Account集合（Account表）中时，该命令可用。
        /// 命令执行后，Account集合（Account表）被更新，新创建的用户不拥有任何的会话。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="input"></param>
        void AddUser(IAcSession subject, IAccountCreateIo input);

        /// <summary>
        /// 【核心Rbac管理函数】该命令从Rbac数据库中删除一个已经存在的用户。该命令可用当且仅当被删除的用户是Account数据集（Account表）
        /// 的一个成员（记录）。Accounts数据集（Account表）将被更新、Privilege数据集（Privilege表）将被更新，
        /// Privilege集合中主体为当前被删除的账户的成员将被删除。如果一个正处在会话中的用户被删除，anycmd
        /// 的实现会等待该会话结束后删除，但账户管理员是可以看到被删除的账户是否正在会话的且有权的管理员是可以强行终止给定的账户的会话的。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="accountId"></param>
        void DeleteUser(IAcSession subject, Guid accountId);

        /// <summary>
        /// 【核心Rbac管理函数】该命令创建一个新的角色。该命令可用当且仅当要创建的角色尚且不存在于RoleSet数据集中。RoleSet数据集
        /// 将被更新。初始时，新创建的角色没有分配任何用户和权限。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="input"></param>
        void AddRole(IAcSession subject, IRoleCreateIo input);

        /// <summary>
        /// 【核心Rbac管理函数】该命令从Rbac数uk中删除一个角色。该命令可用当且仅当被删除的角色是RoleSet数据集的成员。如果被删除的角色
        /// 在某些会话中尚且是激活的，则Anycmd会从会话中删除这个角色然后允许会话继续执行。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="roleId"></param>
        void DeleteRole(IAcSession subject, Guid roleId);

        /// <summary>
        /// 【核心Rbac、通用角色层次、静态职责分离管理函数】该命令给用户分配角色。该命令可用当且仅当该用户是Account数据集（Account表）的成员（记录），该角色是RoleSet
        /// 数据集的成员，并且该角色尚未分配给该用户。数据集（表）Privilege将被更新。
        /// 1 该用户是Account数据集（表）的成员（记录）；
        /// 2 该角色是RoleSet数据集的成员；
        /// 3 该用户还没有被分配该角色；
        /// 4 所有Ssd约束在执行完命令后仍然被满足。
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="subject"></param>
        /// <param name="accountId"></param>
        void AssignUser(IAcSession subject, Guid accountId, Guid roleId);

        /// <summary>
        /// 【核心Rbac管理函数】该命令删除一个角色role到用户account的分配。该命令可用当且仅当account是Account数据集的成员，role
        /// 是RoleSet数据集的成员，并且角色role已经分配给了用户account。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="accountId"></param>
        /// <param name="roleId"></param>
        void DeassignUser(IAcSession subject, Guid accountId, Guid roleId);

        /// <summary>
        /// 【核心Rbac管理函数】该命令给一个角色分配对一个对象执行某个操作的权限。该命令可用当且仅当给定的（操作，对象）代表了
        /// 一项权限并且该角色是RoleSet数据集的成员。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="functionId"></param>
        /// <param name="roleId"></param>
        void GrantPermission(IAcSession subject, Guid functionId, Guid roleId);

        /// <summary>
        /// 【核心Rbac管理函数】该命令从分配给角色的权限集中撤销对某个对象执行某个操作的权限。该命令可用当且仅当（操作、对象）
        /// 代表一项权限，并且该权限已经分配给了该角色。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="functionId"></param>
        /// <param name="roleId"></param>
        void RevokePermission(IAcSession subject, Guid functionId, Guid roleId);

        /// <summary>
        /// 【核心Rbac、通用角色层次支持系统函数】该函数创建一个新的会话，以指定的用户作为会话拥有者，以指定的角色集作为激活角色集。该函数可用
        /// 当且仅当：
        /// 1 该用户是Account数据集（表）的成员（记录）；
        /// 2 该会话的激活角色集是该用户分配的角色集的子集。
        /// 3 该会话的激活角色集满足所有的Dsd约束。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="sessionId"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        IAcSession CreateSession(IAcSession subject, Guid sessionId, AccountState account);

        /// <summary>
        /// 【核心Rbac支持系统函数】该函数删除一个会话。该函数可用当且仅当会话标识符是AcSession数据集（表）的成员（记录）。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="sessionId"></param>
        void DeleteSession(IAcSession subject, Guid sessionId);

        /// <summary>
        /// 【核心Rbac、通用角色层次支持系统函数】该函数为给定的用户会话增加一个激活角色。该函数可用当且仅当：
        /// 1 该用户是Account数据集（表）的成员（记录）；
        /// 2 该角色是RoleSet数据集的成员；
        /// 3 会话标识符是AcSession数据集（表）的成员（记录）；
        /// 4 该角色已经分配给了该用户；
        /// 5 该用户拥有该会话；
        /// 6 在给该会话增加新的激活角色后，所有的Dsd约束都被满足。
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="subject"></param>
        /// <param name="targetSession"></param>
        void AddActiveRole(IAcSession subject, IAcSession targetSession, Guid roleId);

        /// <summary>
        /// 【核心Rbac支持系统函数】该函数从给定用户会话中删除一个激活角色。该函数可用当且仅当该用户是Account数据集（表）的成员（记录），
        /// 会话标识是AcSession数据集（表）的成员（记录），该用户是该会话的拥有者并且该角色是该会话的一个激活角色。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="targetSession"></param>
        /// <param name="roleId"></param>
        void DropActiveRole(IAcSession subject, IAcSession targetSession, Guid roleId);

        /// <summary>
        /// 【核心Rbac支持系统函数】该函数决定一个给定的会话的主体是否允许对给定的对象执行某个给定的操作并返回一个布尔值。该函数可用当且仅当
        /// 会话标识符是AcSession数据集（表）的成员（记录），该对象是它对应类型的对象的数据集的成员，该操作是FunctionSet
        /// 数据集的成员。会话的主体可以对该对象执行该操作当且仅当会话的某个激活角色拥有对应的权限。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="targetSession"></param>
        /// <param name="functionId"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool CheckAccess(IAcSession subject, IAcSession targetSession, Guid functionId, IManagedObject obj);

        /// <summary>
        /// 【核心Rbac查看函数】该函数返回被分配给了某个指定角色的用户。该函数可用当且仅当该角色是RoleSet数据集的成员。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        IReadOnlyCollection<AccountState> AssignedUsers(IAcSession subject, Guid roleId);

        /// <summary>
        /// 【核心Rbac查看函数】该函数返回分配给了一个给定用户的角色。该函数可用当且仅当该用户是Account数据集（表）的成员（记录）。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        IReadOnlyCollection<RoleState> AssignedRoles(IAcSession subject, Guid accountId);

        /// <summary>
        /// 【核心Rbac高级查看函数】该函数返回给定会话的激活角色。该函数可用当且仅当该会话标识符是AcSession数据集（表）的成员（记录）。
        /// </summary>
        /// <returns></returns>
        IReadOnlyCollection<RoleState> SessionRoles(IAcSession subject, IAcSession targetSession);

        /// <summary>
        /// 【核心Rbac高级查看函数】该函数返回给定会话的权限，即该会话的激活角色拥有的权限。该函数可用当且仅当会话标识符是
        /// AcSession数据集（表）的成员（记录）。
        /// </summary>
        /// <returns></returns>
        IReadOnlyCollection<FunctionState> SessionPermissions(IAcSession subject, IAcSession targetSession);

        /// <summary>
        /// 【核心Rbac高级查看函数】该函数返回分配给一个给定角色的权限。该函数可用当且仅当该角色是RoleSet数据集的成员。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        IReadOnlyCollection<FunctionState> RolePermissions(IAcSession subject, Guid roleId);

        /// <summary>
        /// 【核心Rbac高级查看函数】该函数返回一个给定用户的权限。该函数可用当且仅当该用户是Account数据集（表）的成员（记录）。
        /// </summary>
        /// <returns></returns>
        IReadOnlyCollection<FunctionState> UserPermissions(IAcSession subject, IAcSession targetSession);

        /// <summary>
        /// 【核心Rbac高级查看函数】该函数返回一个给定角色被允许的对给定对象执行的操作。该函数可用当且仅当该角色是RoleSet数据集的成员，
        /// 该对象是obj对应类型数据集的成员。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="targetSession"></param>
        /// <param name="roleId"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        IReadOnlyCollection<FunctionState> RoleOperationsOnObject(IAcSession subject, IAcSession targetSession, Guid roleId, IManagedObject obj);

        /// <summary>
        /// 【核心Rbac高级查看函数】该函数返回给定用户被允许的针对给定角色执行的操作。该函数可用当且仅当该用户是Account数据集（表）
        /// 的成员（记录），该对象是obj对应类型数据集的成员。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="targetSession"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        IReadOnlyCollection<FunctionState> UserOperationsOnObject(IAcSession subject, IAcSession targetSession, IManagedObject obj);

        /// <summary>
        /// 【通用角色层次、静态职责分离管理函数】该命令在两个已经存在的角色r_asc/客体和r_desc/主体之间建立直接继承关系。r_asc>>r_desc。该命令可用
        /// 当且仅当：
        /// 1 r_asc/客体和r_desc/主体都是RoleSet数据集的成员；
        /// 2 r_asc/客体不是r_desc/主体的直接祖先，
        /// 3 r_desc/主体不继承r_asc/客体（不免产生回路）。
        /// 4 Ssd约束在该命令执行后扔是满足的。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="subjectRoleId">r_desc/主体</param>
        /// <param name="objectRoleId">r_asc/客体</param>
        void AddInheritance(IAcSession subject, Guid subjectRoleId, Guid objectRoleId);

        /// <summary>
        /// 【通用角色层次管理函数】该命令删除已经存在的直接继承关系r_asc>>r_desc。该命令可用当且仅当r_asc/客体和r_desc/主体都是
        /// RoleSet数据集的成员，r_asc/客体是r_desc/主体的直接祖先。在执行完该命令以后，新的继承关系是新的
        /// 直接继承关系的自反传递包。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="subjectRoleId">r_desc/主体</param>
        /// <param name="objectRoleId">r_asc/客体</param>
        void DeleteInheritance(IAcSession subject, Guid subjectRoleId, Guid objectRoleId);

        /// <summary>
        /// 【通用角色层次管理函数】该命令创建一个新角色r_asc/客体/父角色，并作为现存角色r_desc/主体/子角色 的直接祖先插入到角色层次中去。该命令可用
        /// 当且仅当r_asc/客体/父角色 不是RoleSet数据集的成员，r_desc/子角色 是RoleSet数据集的成员。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="childRoleId">r_desc/主体</param>
        /// <param name="parentRoleCreateInput">r_asc/客体</param>
        void AddAscendant(IAcSession subject, Guid childRoleId, IRoleCreateIo parentRoleCreateInput);

        /// <summary>
        /// 【通用角色层次管理函数】该命令创建一个新的角色作为现存角色 r_asc/客体/父角色 的直接后代插入到角色层次中。该命令可用当且仅当r_desc/主体
        /// 不是RoleSet数据集的成员，r_asc/客体/父角色 是RoleSet数据集的成员。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="parentRoleId"></param>
        /// <param name="childRoleCreateInput"></param>
        void AddDescendant(IAcSession subject, Guid parentRoleId, IRoleCreateIo childRoleCreateInput);

        /// <summary>
        /// 【通用角色层次查看函数】该函数返回拥有给定角色的授权用户。该函数可用当且仅当给定角色是RoleSet的成员。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="targetSession"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        IReadOnlyCollection<AccountState> AuthorizedUsers(IAcSession subject, IAcSession targetSession, Guid roleId);

        /// <summary>
        /// 【通用角色层次查看函数】该函数返回给定用户的授权角色。该函数可用当且仅当该用户是Account数据集（表）的成员（记录）。
        /// </summary>
        /// <returns></returns>
        IReadOnlyCollection<RoleState> AuthorizedRoles(IAcSession subject, IAcSession targetSession);

        /// <summary>
        /// 【通用角色层次、Ssd关系管理函数】该命令创建一个命名的Ssd角色集合，并设定相应的阀值。该命令可用当且仅当：
        /// 1 Ssd角色集的标识和名称还没有被使用；
        /// 2 Ssd角色集中的角色都是RoleSet数据集的成员；
        /// 3 SsdCard是一个大于或等于2的自然数，同事还要小于或等于Ssd角色集的基数（基数：有限集的元素个数）；
        /// 4 新的Ssd角色集的约束当前是被满足的。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="input"></param>
        void CreateSsdSet(IAcSession subject, ISsdSetCreateIo input);

        /// <summary>
        /// 【Ssd关系管理函数】该命令删除一个Ssd角色集。该命令可用当且仅当该Ssd角色集存在。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="ssdSetId"></param>
        void DeleteSsdSet(IAcSession subject, Guid ssdSetId);

        /// <summary>
        /// 【通用角色层次、Ssd关系管理函数】该命令为Ssd角色集增加一个角色，该Ssd角色集关联的阀值不发生改变。
        /// 该命令可用当且仅当：
        /// 1 该Ssd角色集存在；
        /// 2 该角色是RoleSet数据集的成员，并且尚不属于该Ssd角色集；
        /// 3 该命令执行之后，Ssd约束仍然是满足的。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="ssdSetId"></param>
        /// <param name="roleId"></param>
        void AddSsdRoleMember(IAcSession subject, Guid ssdSetId, Guid roleId);

        /// <summary>
        /// 【Ssd关系管理函数】该命令从Ssd角色集中删除一个角色，其关联的阀值不发生改变。该命令有效当且仅当：
        /// 1 Ssd角色集已经存在；
        /// 2 要删除的角色是该Ssd角色集的成员；
        /// 3 该Ssd角色集关联的阀值小于该角色集的成员数。
        /// <remarks>
        /// 注意：修改后的Ssd约束当前应该是被满足的。
        /// </remarks>
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="ssdRoleId"></param>
        void DeleteSsdRoleMember(IAcSession subject, Guid ssdRoleId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="ssdSetId"></param>
        /// <param name="roleId"></param>
        void DeleteSsdRoleMember(IAcSession subject, Guid ssdSetId, Guid roleId);

        /// <summary>
        /// 【通用角色层次、Ssd关系管理函数】该命令设定与给定的Ssd角色集关联的阀值。该命令可用当且仅当：
        /// 1 该Ssd角色集存在；
        /// 2 新的阀值是一个大于或等于2的自然数，它要小于或等于Ssd角色集的基（基：有限集元素的个数）；
        /// 3 新的Ssd约束当前应该是被满足的。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="ssdSetId"></param>
        /// <param name="cardinality"></param>
        void SetSsdCardinality(IAcSession subject, Guid ssdSetId, int cardinality);

        /// <summary>
        /// 【静态职责分离查看函数】该函数返回所有的Ssd角色集。
        /// </summary>
        /// <returns></returns>
        IReadOnlyCollection<SsdRoleState> SsdRoleSets(IAcSession subject);

        /// <summary>
        /// 【静态职责分离查看函数】该函数返回与一个指定Ssd角色集合相关联的角色的集合
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="ssdSetId"></param>
        /// <returns></returns>
        IReadOnlyCollection<RoleState> SsdRoleSetRoles(IAcSession subject, Guid ssdSetId);

        /// <summary>
        /// 【静态职责分离查看函数】该函数返回与给定Ssd角色集关联的阀值。该函数可用当且仅当该角色集存在。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="ssdSetId"></param>
        /// <returns></returns>
        int SsdRoleSetCardinality(IAcSession subject, Guid ssdSetId);

        /// <summary>
        /// 创建一个动态责任分离角色集并设定相应的阀值。该Dsd约束要求Dsd角色集中
        /// 任何【阀值】个或者更多角色不能都在某个用户会话过程中被激活。
        /// 该命令可用当且仅当：
        /// 1 Dsd角色集的标识和名称还没有被使用；
        /// 2 Dsd角色集中的角色都是RoleSet数据集的成员；
        /// 3 SsdCard是一个大于或等于2的自然数；
        /// 4 新的Dsd角色集的约束当前是被满足的。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="input"></param>
        void CreateDsdSet(IAcSession subject, IDsdSetCreateIo input);

        /// <summary>
        /// 该命令删除一个Dsd角色集。该命令可用当且仅当该Dsd角色集存在。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="dsdSetId"></param>
        void DeleteDsdSet(IAcSession subject, Guid dsdSetId);

        /// <summary>
        /// 该命令为一个给定的Dsd角色集增加一个角色，Dsd角色集关联的阀值不发生改变。该命令有效当且仅当：
        /// 1 该Dsd角色集存在；
        /// 2 该角色是RoleSet数据集的成员，并且尚不属于该Dsd角色集；
        /// 3 该命令执行成功之后，Dsd约束仍然是满足的。
        /// <remarks>
        /// SsdCard要小于或等于Dsd角色集的基（基：有限集元素的个数）
        /// </remarks>
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="dsdSetId"></param>
        /// <param name="roleId"></param>
        void AddDsdRoleMember(IAcSession subject, Guid dsdSetId, Guid roleId);

        /// <summary>
        /// 该命令从Dsd角色集中删除一个角色，其关联的阀值不发生改变。该命令有效当且仅当：
        /// 1 Dsd角色集已经存在；
        /// 2 要删除的角色是该Dsd角色集的成员；
        /// 3 该Dsd角色集关联的阀值小于该角色集的成员数。
        /// <remarks>
        /// 注意：修改后的Dsd约束当前应该是被满足的。
        /// </remarks>
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="dsdRoleId"></param>
        void DeleteDsdRoleMember(IAcSession subject, Guid dsdRoleId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="dsdRoleId"></param>
        /// <param name="roleId"></param>
        void DeleteDsdRoleMember(IAcSession subject, Guid dsdRoleId, Guid roleId);

        /// <summary>
        /// 该命令设定与给定的Dsd角色集关联的阀值。该命令可用当且仅当：
        /// 1 该Dsd角色集存在；
        /// 2 新的阀值是一个大于或等于2的自然数，它要小于或等于Dsd角色集的基（基：有限集元素的个数）；
        /// 3 新的Dsd约束当前应该是被满足的。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="dsdSetId"></param>
        /// <param name="cardinality"></param>
        void SetDsdCardinality(IAcSession subject, Guid dsdSetId, int cardinality);

        /// <summary>
        /// 该函数返回所有的Dsd角色集。
        /// </summary>
        /// <returns></returns>
        IReadOnlyCollection<DsdRoleState> DsdRoleSets(IAcSession subject);

        /// <summary>
        /// 该函数返回给定的Dsd角色集中的角色。该函数可用当且仅当该角色集存在。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="dsdSetId"></param>
        /// <returns></returns>
        IReadOnlyCollection<RoleState> DsdRoleSetRoles(IAcSession subject, Guid dsdSetId);

        /// <summary>
        /// 该函数返回与给定Dsd角色集关联的阀值。该函数可用当且仅当该角色集存在。
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="dsdSetId"></param>
        /// <returns></returns>
        int DsdRoleSetCardinality(IAcSession subject, Guid dsdSetId);
    }
}
