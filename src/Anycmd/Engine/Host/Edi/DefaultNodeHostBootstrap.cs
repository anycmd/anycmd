
namespace Anycmd.Engine.Host.Edi
{
    using Entities;
    using Repositories;
    using System.Linq;

    public class DefaultNodeHostBootstrap : INodeHostBootstrap
    {
        private readonly IAcDomain _acDomain;

        public DefaultNodeHostBootstrap(IAcDomain acDomain)
        {
            this._acDomain = acDomain;
        }

        public Archive[] GetArchives()
        {
            var repository = _acDomain.RetrieveRequiredService<IRepository<Archive>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToArray();
            }
        }

        public Element[] GetElements()
        {
            var repository = _acDomain.RetrieveRequiredService<IRepository<Element>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().Where(a => a.DeletionStateCode == 0).ToArray();
            }
        }

        public InfoDicItem[] GetInfoDicItems()
        {
            var repository = _acDomain.RetrieveRequiredService<IRepository<InfoDicItem>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().Where(a => a.DeletionStateCode == 0).ToArray();
            }
        }

        public InfoDic[] GetInfoDics()
        {
            var repository = _acDomain.RetrieveRequiredService<IRepository<InfoDic>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().Where(a => a.DeletionStateCode == 0).ToArray();
            }
        }

        public NodeElementAction[] GetNodeElementActions()
        {
            var repository = _acDomain.RetrieveRequiredService<IRepository<NodeElementAction>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToArray();
            }
        }

        public NodeElementCare[] GetNodeElementCares()
        {
            var repository = _acDomain.RetrieveRequiredService<IRepository<NodeElementCare>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToArray();
            }
        }

        public NodeOntologyCare[] GetNodeOntologyCares()
        {
            var repository = _acDomain.RetrieveRequiredService<IRepository<NodeOntologyCare>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToArray();
            }
        }

        public NodeOntologyCatalog[] GetNodeOntologyCatalogs()
        {
            var repository = _acDomain.RetrieveRequiredService<IRepository<NodeOntologyCatalog>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToArray();
            }
        }

        public Node[] GetNodes()
        {
            var repository = _acDomain.RetrieveRequiredService<IRepository<Node>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().Where(a => a.DeletionStateCode == 0).ToArray();
            }
        }

        public Ontology[] GetOntologies()
        {
            var repository = _acDomain.RetrieveRequiredService<IRepository<Ontology>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().Where(a => a.DeletionStateCode == 0).ToArray();
            }
        }

        public InfoGroup[] GetInfoGroups()
        {
            var repository = _acDomain.RetrieveRequiredService<IRepository<Ontology>>();
            using (var context = repository.Context)
            {
                return context.Query<InfoGroup>().ToArray();
            }
        }

        public Action[] GetActions()
        {
            var repository = _acDomain.RetrieveRequiredService<IRepository<Ontology>>();
            using (var context = repository.Context)
            {
                return context.Query<Action>().ToArray();
            }
        }

        public Topic[] GetTopics()
        {
            var repository = _acDomain.RetrieveRequiredService<IRepository<Ontology>>();
            using (var context = repository.Context)
            {
                return context.Query<Topic>().ToArray();
            }
        }

        public OntologyCatalog[] GetOntologyCatalogs()
        {
            var repository = _acDomain.RetrieveRequiredService<IRepository<OntologyCatalog>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToArray();
            }
        }

        public Process[] GetProcesses()
        {
            var repository = _acDomain.RetrieveRequiredService<IRepository<Process>>();
            using (var context = repository.Context)
            {
                return repository.AsQueryable().ToArray();
            }
        }
    }
}
