
namespace Anycmd.Engine.Edi
{
    using Abstractions;
    using System;

    public sealed class InfoDicState : StateObject<InfoDicState>, IInfoDic, IStateObject
    {
        private InfoDicState(Guid id) : base(id) { }

        public static InfoDicState Create(IAcDomain host, IInfoDic infoDic)
        {
            if (infoDic == null)
            {
                throw new ArgumentNullException("infoDic");
            }
            return new InfoDicState(infoDic.Id)
            {
                Host = host,
                Code = infoDic.Code,
                CreateOn = infoDic.CreateOn,
                IsEnabled = infoDic.IsEnabled,
                Name = infoDic.Name,
                SortCode = infoDic.SortCode
            };
        }

        public IAcDomain Host { get; private set; }

        public string Code { get; private set; }

        public int IsEnabled { get; private set; }

        public int SortCode { get; private set; }

        public string Name { get; private set; }

        public DateTime? CreateOn { get; private set; }

        protected override bool DoEquals(InfoDicState other)
        {
            return
                Id == other.Id &&
                Code == other.Code &&
                IsEnabled == other.IsEnabled &&
                SortCode == other.SortCode &&
                Name == other.Name;
        }
    }
}
