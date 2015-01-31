
namespace Anycmd.Engine.Host.Edi.Entities
{
    using Engine.Edi.Abstractions;
    using Engine.Edi.InOuts;
    using Model;
    using System.Diagnostics;

    /// <summary>
    /// 表示节点数据访问实体。
    /// </summary>
    public class Node : NodeBase, IAggregateRoot
    {
        public Node() { }

        public static Node Create(INodeCreateIo input)
        {
            Debug.Assert(input.Id != null, "input.Id != null");
            return new Node
            {
                Abstract = input.Abstract,
                AnycmdApiAddress = input.AnycmdApiAddress,
                AnycmdWsAddress = input.AnycmdWsAddress,
                BeatPeriod = input.BeatPeriod,
                Code = input.Code,
                Description = input.Description,
                Email = input.Email,
                Icon = input.Icon,
                Id = input.Id.Value,
                IsEnabled = input.IsEnabled,
                Mobile = input.Mobile,
                Name = input.Name,
                TransferId = input.TransferId,
                Telephone = input.Telephone,
                Steward = input.Steward,
                SortCode = input.SortCode,
                SecretKey = input.SecretKey,
                Qq = input.Qq,
                PublicKey = input.PublicKey,
                Catalog = input.Catalog
            };
        }

        public void Update(INodeUpdateIo input)
        {
            this.Abstract = input.Abstract;
            this.AnycmdApiAddress = input.AnycmdApiAddress;
            this.AnycmdWsAddress = input.AnycmdWsAddress;
            this.BeatPeriod = input.BeatPeriod;
            this.Code = input.Code;
            this.Description = input.Description;
            this.Email = input.Email;
            this.Icon = input.Icon;
            this.IsEnabled = input.IsEnabled;
            this.Mobile = input.Mobile;
            this.Name = input.Name;
            this.Catalog = input.Catalog;
            this.PublicKey = input.PublicKey;
            this.Qq = input.Qq;
            this.SecretKey = input.SecretKey;
            this.SortCode = input.SortCode;
            this.Steward = input.Steward;
            this.Telephone = input.Telephone;
            this.TransferId = input.TransferId;
        }
    }
}
