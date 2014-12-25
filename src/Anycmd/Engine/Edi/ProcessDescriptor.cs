
namespace Anycmd.Engine.Edi
{
    using Abstractions;
    using Engine.Ac;
    using Exceptions;
    using Model;
    using System;
    using System.Net;
    using Util;

    /// <summary>
    /// 进程描述对象。进程描述对象上可以读取进程配置信息。描述对象往往长久贮存在内存中。
    /// </summary>
    public sealed class ProcessDescriptor : StateObject<ProcessDescriptor>
    {
        private OntologyDescriptor _ontology;
        ProcessType _type;
        private string _hostName;
        private string _webApiBaseAddress;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="process"></param>
        /// <param name="id"></param>
        public ProcessDescriptor(IAcDomain host, ProcessState process, Guid id)
            : base(id)
        {
            if (process == null)
            {
                throw new ArgumentNullException("process");
            }
            this.Host = host;
            this.Process = process;
            if (!process.Type.TryParse(out _type))
            {
                throw new CoreException("意外的进程类型");
            }
            if (!string.IsNullOrEmpty(process.OrganizationCode))
            {
                OrganizationState org;
                if (!Host.OrganizationSet.TryGetOrganization(process.OrganizationCode, out org))
                {
                    throw new CoreException("意外的组织结构码" + process.OrganizationCode);
                }
            }
        }

        public IAcDomain Host { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public ProcessState Process { get; private set; }

        /// <summary>
        /// 进程类型
        /// </summary>
        public ProcessType ProcessType
        {
            get { return _type; }
            private set { _type = value; }
        }

        /// <summary>
        /// 标题。由本节点名称+本进程名称+本进程绑定的本体的名称拼接而来
        /// </summary>
        public string Title
        {
            get
            {
                return Host.NodeHost.Nodes.ThisNode.Node.Name + this.Process.Name + " - " + this.Ontology.Ontology.Name;
            }
        }

        /// <summary>
        /// 本体。进程必须关联一个本体。一个进程只能处理一个本体的事务，不能既处理教师又处理学生。
        /// </summary>
        public OntologyDescriptor Ontology
        {
            get
            {
                if (_ontology != null) return _ontology;
                if (!Host.NodeHost.Ontologies.TryGetOntology(this.Process.OntologyId, out _ontology))
                {
                    throw new CoreException("非法本体标识" + this.Process.OntologyId);
                }
                return _ontology;
            }
        }

        /// <summary>
        /// 进程标识。使用Url来标识一个进程。
        /// </summary>
        public string WebApiBaseAddress
        {
            get {
                return _webApiBaseAddress ??
                       (_webApiBaseAddress = "http://" + HostName + ":" + this.Process.NetPort.ToString() + "/");
            }
        }

        /// <summary>
        /// 进程所在的主机名。
        /// </summary>
        private string HostName
        {
            get
            {
                if (_hostName == null)
                {
                    _hostName = this.Ontology.EntityProvider.GetEntityDataSource(this.Ontology);
                }
                return _hostName;
            }
        }

        /// <summary>
        /// 注意：每次访问都会引发网络请求。以Http Get请求GetWebApiBaseAddress+IsAlive地址。
        /// </summary>
        /// <returns></returns>
        public bool IsRuning()
        {
            bool isOnline = false;
            var r = WebRequest.Create(this.WebApiBaseAddress + "IsAlive") as HttpWebRequest;
            r.Method = "GET";
            try
            {
                var s = r.GetResponse() as HttpWebResponse;
                if (s.StatusCode == HttpStatusCode.OK)
                {
                    isOnline = true;
                }
            }
            catch
            {
                isOnline = false;
            }
            return isOnline;
        }

        protected override bool DoEquals(ProcessDescriptor other)
        {
            return Process == other.Process;
        }
    }
}
