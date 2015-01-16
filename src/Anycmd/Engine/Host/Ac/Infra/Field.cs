
namespace Anycmd.Engine.Host.Ac.Infra
{
    using Engine.Ac.Abstractions.Infra;
    using Engine.Ac.InOuts;
    using System;
    using System.Diagnostics;

    public class Field : FieldBase
    {
        public Field() { }

        public static Field Create(IFieldCreateIo input)
        {
            Debug.Assert(input.Id != null, "input.Id != null");
            return new Field
            {
                Id = input.Id.Value,
                ResourceTypeId = input.ResourceTypeId,
                Code = input.Code,
                Name = input.Name,
                Description = input.Description,
                Icon = input.Icon,
                SortCode = input.SortCode,
                CreateOn = DateTime.Now
            };
        }

        public void Update(IFieldUpdateIo input)
        {
            this.Code = input.Code;
            this.Description = input.Description;
            this.Icon = input.Icon;
            this.Name = input.Name;
            this.SortCode = input.SortCode;
        }
    }
}
