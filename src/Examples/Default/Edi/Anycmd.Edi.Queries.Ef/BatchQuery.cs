
namespace Anycmd.Edi.Queries.Ef
{
    using Anycmd.Ef;
    using ViewModels.BatchViewModels;

    /// <summary>
    /// 查询接口实现<see cref="IBatchQuery"/>
    /// </summary>
    public class BatchQuery : QueryBase, IBatchQuery
    {
        public BatchQuery(IAcDomain acDomain)
            : base(acDomain, "EdiEntities")
        {
        }
    }
}
