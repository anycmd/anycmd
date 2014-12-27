
namespace Anycmd.Engine.Host.Edi
{
    using Entities;
    using Repositories;
    using System.Linq;

    public class DefaultNodeHostBootstrap : INodeHostBootstrap
    {
        private readonly IAcDomain _host;

        public DefaultNodeHostBootstrap(IAcDomain host)
        {
            this._host = host;
        }

        public Archive[] GetArchives()
        {
            var repository = _host.RetrieveRequiredService<IRepository<Archive>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToArray();
            }
        }

        public Element[] GetElements()
        {
            var repository = _host.RetrieveRequiredService<IRepository<Element>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().Where(a => a.DeletionStateCode == 0).ToArray();
            }
        }

        public InfoDicItem[] GetInfoDicItems()
        {
            var repository = _host.RetrieveRequiredService<IRepository<InfoDicItem>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().Where(a => a.DeletionStateCode == 0).ToArray();
            }
        }

        public InfoDic[] GetInfoDics()
        {
            var repository = _host.RetrieveRequiredService<IRepository<InfoDic>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().Where(a => a.DeletionStateCode == 0).ToArray();
            }
        }

        public NodeElementAction[] GetNodeElementActions()
        {
            var repository = _host.RetrieveRequiredService<IRepository<NodeElementAction>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToArray();
            }
        }

        public NodeElementCare[] GetNodeElementCares()
        {
            var repository = _host.RetrieveRequiredService<IRepository<NodeElementCare>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToArray();
            }
        }

        public NodeOntologyCare[] GetNodeOntologyCares()
        {
            var repository = _host.RetrieveRequiredService<IRepository<NodeOntologyCare>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToArray();
            }
        }

        public NodeOntologyOrganization[] GetNodeOntologyOrganizations()
        {
            var repository = _host.RetrieveRequiredService<IRepository<NodeOntologyOrganization>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToArray();
            }
        }

        public Node[] GetNodes()
        {
            var repository = _host.RetrieveRequiredService<IRepository<Node>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().Where(a => a.DeletionStateCode == 0).ToArray();
            }
        }

        public Ontology[] GetOntologies()
        {
            var repository = _host.RetrieveRequiredService<IRepository<Ontology>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().Where(a => a.DeletionStateCode == 0).ToArray();
            }
        }

        public InfoGroup[] GetInfoGroups()
        {
            var repository = _host.RetrieveRequiredService<IRepository<Ontology>>();
            using (var context = repository.Context)
            {
                return context.Query<InfoGroup>().ToArray();
            }
        }

        public Action[] GetActions()
        {
            var repository = _host.RetrieveRequiredService<IRepository<Ontology>>();
            using (var context = repository.Context)
            {
                return context.Query<Action>().ToArray();
            }
        }

        public Topic[] GetTopics()
        {
            var repository = _host.RetrieveRequiredService<IRepository<Ontology>>();
            using (var context = repository.Context)
            {
                return context.Query<Topic>().ToArray();
            }
        }

        public OntologyOrganization[] GetOntologyOrganizations()
        {
            var repository = _host.RetrieveRequiredService<IRepository<OntologyOrganization>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToArray();
            }
        }

        public Process[] GetProcesses()
        {
            var repository = _host.RetrieveRequiredService<IRepository<Process>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToArray();
            }
        }
    }
}
