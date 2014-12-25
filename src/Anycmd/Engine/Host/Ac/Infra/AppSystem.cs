
namespace Anycmd.Engine.Host.Ac.Infra
{
    using Engine.Ac.Abstractions.Infra;
    using InOuts;
    using Model;
    using System.Diagnostics;

    /// <summary>
    /// 表示应用系统数据访问实体。
    /// </summary>
    public class AppSystem : AppSystemBase, IAggregateRoot, IAppSystem
    {
        public AppSystem()
        {
        }

        public static AppSystem Create(IAppSystemCreateIo input)
        {
            Debug.Assert(input.Id != null, "input.Id != null");
            return new AppSystem
            {
                Code = input.Code,
                Id = input.Id.Value,
                Name = input.Name,
                Description = input.Description,
                Icon = input.Icon,
                PrincipalId = input.PrincipalId,
                SsoAuthAddress = input.SsoAuthAddress,
                IsEnabled = input.IsEnabled,
                SortCode = input.SortCode,
                ImageUrl = input.ImageUrl
            };
        }

        public void Update(IAppSystemUpdateIo input)
        {
            this.Code = input.Code;
            this.Description = input.Description;
            this.Icon = input.Icon;
            this.ImageUrl = input.ImageUrl;
            this.IsEnabled = input.IsEnabled;
            this.Name = input.Name;
            this.PrincipalId = input.PrincipalId;
            this.SortCode = input.SortCode;
            this.SsoAuthAddress = input.SsoAuthAddress;
        }
    }
}
