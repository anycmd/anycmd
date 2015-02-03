
namespace Anycmd.Edi.Queries.Ef
{
    using Anycmd.Ef;
    using ViewModels.PluginViewModels;

    /// <summary>
    /// 查询接口实现<see cref="IPluginQuery"/>
    /// </summary>
    public class PluginQuery : QueryBase, IPluginQuery
    {
        public PluginQuery(IAcDomain acDomain)
            : base(acDomain, "EdiEntities")
        {
        }
    }
}
