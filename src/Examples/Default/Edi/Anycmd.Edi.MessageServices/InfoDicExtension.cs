
namespace Anycmd.Edi.MessageServices
{
    using Engine.Edi;
    using ServiceModel.Types;
    using System.Linq;

    /// <summary>
    /// 提供将<see cref="IInfoDic"/>转换为<see cref="InfoDicData"/>数据传输对象的方法。
    /// </summary>
    public static class InfoDicExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="infoDic"></param>
        /// <returns></returns>
        public static InfoDicData ToInfoDicData(this InfoDicState infoDic)
        {
            var infoDicData = new InfoDicData()
            {
                Name = infoDic.Name,
                Code = infoDic.Code
            };
            foreach (var item in infoDic.AcDomain.NodeHost.InfoDics.GetInfoDicItems(infoDic).OrderBy(a => a.SortCode))
            {
                infoDicData.InfoDicItems.Add(new InfoDicItemData
                {
                    Code = item.Code,
                    Description = item.Description,
                    Level = item.Level,
                    Name = item.Name
                });
            }

            return infoDicData;
        }
    }
}
