
namespace Anycmd.Edi.Queries.Ef
{
    using Anycmd.Ef;
    using ViewModels.StateCodeViewModels;

    /// <summary>
    /// 查询接口实现<see cref="IStateCodeQuery"/>
    /// </summary>
    public class StateCodeQuery : QueryBase, IStateCodeQuery
    {
        public StateCodeQuery(IAcDomain acDomain)
            : base(acDomain, "EdiEntities")
        {
        }
    }
}
