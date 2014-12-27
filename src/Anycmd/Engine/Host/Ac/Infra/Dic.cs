
namespace Anycmd.Engine.Host.Ac.Infra
{
    using Engine.Ac.Abstractions.Infra;
    using Engine.Ac.InOuts;
    using Model;
    using System.Diagnostics;

    /// <summary>
    /// 表示系统字典数据访问实体。
    /// </summary>
    public class Dic : DicBase, IAggregateRoot
    {
        public Dic()
        {
        }

        public static Dic Create(IDicCreateIo input)
        {
            Debug.Assert(input.Id != null, "input.Id != null");
            return new Dic
                {
                    Id = input.Id.Value,
                    Code = input.Code,
                    Name = input.Name,
                    Description = input.Description,
                    SortCode = input.SortCode,
                    IsEnabled = input.IsEnabled
                };
        }

        public void Update(IDicUpdateIo input)
        {
            this.Code = input.Code;
            this.Description = input.Description;
            this.IsEnabled = input.IsEnabled;
            this.SortCode = input.SortCode;
            this.Name = input.Name;
        }
    }
}
