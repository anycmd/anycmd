using System;

namespace Anycmd.Engine.Edi
{
    using Abstractions;
    using Exceptions;
    using Model;
    using Util;

    public sealed class ProcessState : StateObject<ProcessState>, IProcess, IStateObject
    {
        private string _type;

        private ProcessState(Guid id) : base(id) { }

        public static ProcessState Create(IProcess process)
        {
            if (process == null)
            {
                throw new ArgumentNullException("process");
            }
            return new ProcessState(process.Id)
            {
                CreateOn = process.CreateOn,
                Description = process.Description,
                IsEnabled = process.IsEnabled,
                Name = process.Name,
                NetPort = process.NetPort,
                OntologyId = process.OntologyId,
                OrganizationCode = process.OrganizationCode,
                Type = process.Type
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public string Type
        {
            get { return _type; }
            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ValidationException("进程类型不能为空");
                }
                ProcessType processType;
                if (!value.TryParse(out processType))
                {
                    throw new CoreException("非法的进程类型");
                }
                _type = value;
            }
        }

        public string Name { get; private set; }

        public int NetPort { get; private set; }

        public int IsEnabled { get; private set; }

        public Guid OntologyId { get; private set; }

        public string OrganizationCode { get; private set; }

        public string Description { get; private set; }

        public DateTime? CreateOn { get; private set; }

        protected override bool DoEquals(ProcessState other)
        {
            return
                Id == other.Id &&
                Type == other.Type &&
                Name == other.Name &&
                NetPort == other.NetPort &&
                IsEnabled == other.IsEnabled &&
                OntologyId == other.OntologyId &&
                OrganizationCode == other.OrganizationCode;
        }
    }
}
