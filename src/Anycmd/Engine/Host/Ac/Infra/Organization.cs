
namespace Anycmd.Engine.Host.Ac.Infra
{
    using Engine.Ac.Abstractions.Infra;
    using Engine.Ac.InOuts;
    using Model;
    using System.Diagnostics;

    /// <summary>
    /// 表示目录数据访问实体。
    /// </summary>
    public class Organization : OrganizationBase, IAggregateRoot
    {
        #region Ctor
        public Organization()
        {
            IsEnabled = 1;
        }
        #endregion

        public static Organization Create(IOrganizationCreateIo input)
        {
            Debug.Assert(input.Id != null, "input.Id != null");
            return new Organization
            {
                Id = input.Id.Value,
                Address = input.Address,
                CategoryCode = input.CategoryCode,
                Code = input.Code,
                Description = input.Description,
                Fax = input.Fax,
                Icon = input.Icon,
                IsEnabled = input.IsEnabled,
                InnerPhone = input.InnerPhone,
                Name = input.Name,
                OuterPhone = input.OuterPhone,
                ParentCode = input.ParentCode,
                Postalcode = input.PostalCode,
                ShortName = input.ShortName,
                SortCode = input.SortCode,
                WebPage = input.WebPage,
                ContractorId=input.ContractorId
            };
        }

        public void Update(IOrganizationUpdateIo input)
        {
            this.Address = input.Address;
            this.CategoryCode = input.CategoryCode;
            this.Code = input.Code;
            this.Description = input.Description;
            this.Fax = input.Fax;
            this.Icon = input.Icon;
            this.InnerPhone = input.InnerPhone;
            this.IsEnabled = input.IsEnabled;
            this.Name = input.Name;
            this.OuterPhone = input.OuterPhone;
            this.ParentCode = input.ParentCode;
            this.Postalcode = input.Postalcode;
            this.ShortName = input.ShortName;
            this.SortCode = input.SortCode;
            this.WebPage = input.WebPage;
            this.ContractorId = input.ContractorId;
        }
    }
}
