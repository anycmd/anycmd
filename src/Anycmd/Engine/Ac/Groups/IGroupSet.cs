
namespace Anycmd.Engine.Ac.Groups
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 表示该接口的实现类是组集。
    /// </summary>
    public interface IGroupSet : IEnumerable<GroupState>
    {
        /// <summary>
        /// 
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        bool TryGetGroup(Guid groupId, out GroupState group);
    }
}
