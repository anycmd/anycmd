
namespace Anycmd.Engine.Edi
{
    using Abstractions;
    using Model;
    using System;

    public sealed class InfoGroupState : StateObject<InfoGroupState>, IInfoGroup, IStateObject
    {
        private InfoGroupState(Guid id) : base(id) { }

        public static InfoGroupState Create(IInfoGroup infoGroup)
        {
            if (infoGroup == null)
            {
                throw new ArgumentNullException("infoGroup");
            }
            return new InfoGroupState(infoGroup.Id)
            {
                Code = infoGroup.Code,
                Description = infoGroup.Description,
                Name = infoGroup.Name,
                OntologyId = infoGroup.OntologyId,
                SortCode = infoGroup.SortCode
            };
        }

        public string Code { get; private set; }

        public string Name { get; private set; }

        public Guid OntologyId { get; private set; }

        public int SortCode { get; private set; }

        public string Description { get; private set; }

        protected override bool DoEquals(InfoGroupState other)
        {
            return
                Id == other.Id &&
                Code == other.Code &&
                Name == other.Name &&
                OntologyId == other.OntologyId &&
                SortCode == other.SortCode &&
                Description == other.Description;
        }
    }
}
