
namespace Anycmd.Engine.Host.Ac.Infra
{
    using Engine.Ac.Abstractions.Infra;
    using Engine.Ac.InOuts;
    using Model;
    using System.Diagnostics;

    /// <summary>
    /// 表示实体类型数据访问实体。
    /// <remarks>该模型是程序开发模型，被程序员使用，用户不关心本概念。</remarks>
    /// </summary>
    public class EntityType : EntityTypeBase, IAggregateRoot
    {
        public EntityType() { }

        public static EntityType Create(IEntityTypeCreateIo input)
        {
            Debug.Assert(input.Id != null, "input.Id != null");
            return new EntityType
            {
                Id = input.Id.Value,
                Code = input.Code,
                Codespace = input.Codespace,
                DatabaseId = input.DatabaseId,
                Description = input.Description,
                DeveloperId = input.DeveloperId,
                IsCatalogued = input.IsCatalogued,
                Name = input.Name,
                EditHeight = input.EditHeight,
                EditWidth = input.EditWidth,
                SchemaName = input.SchemaName,
                TableName = input.TableName,
                SortCode = input.SortCode
            };
        }

        public void Update(IEntityTypeUpdateIo input)
        {
            this.Code = input.Code;
            this.Codespace = input.Codespace;
            this.IsCatalogued = input.IsCatalogued;
            this.DatabaseId = input.DatabaseId;
            this.Description = input.Description;
            this.DeveloperId = input.DeveloperId;
            this.EditWidth = input.EditWidth;
            this.EditHeight = input.EditHeight;
            this.Name = input.Name;
            this.SchemaName = input.SchemaName;
            this.SortCode = input.SortCode;
            this.TableName = input.TableName;
        }
    }
}
