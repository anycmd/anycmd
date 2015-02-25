
namespace Anycmd.Engine.Host.Ac.Infra
{
    using Engine.Ac.Catalogs;
    using Model;
    using System.Diagnostics;

    /// <summary>
    /// 表示目录数据访问实体。
    /// </summary>
    public class Catalog : CatalogBase, IAggregateRoot
    {
        #region Ctor
        public Catalog()
        {
            IsEnabled = 1;
        }
        #endregion

        public static Catalog Create(ICatalogCreateIo input)
        {
            Debug.Assert(input.Id != null, "input.Id != null");
            return new Catalog
            {
                Id = input.Id.Value,
                CategoryCode = input.CategoryCode,
                Code = input.Code,
                Description = input.Description,
                IsEnabled = input.IsEnabled,
                Name = input.Name,
                ParentCode = input.ParentCode,
                SortCode = input.SortCode
            };
        }

        public void Update(ICatalogUpdateIo input)
        {
            this.CategoryCode = input.CategoryCode;
            this.Code = input.Code;
            this.Description = input.Description;
            this.IsEnabled = input.IsEnabled;
            this.Name = input.Name;
            this.ParentCode = input.ParentCode;
            this.SortCode = input.SortCode;
        }
    }
}
