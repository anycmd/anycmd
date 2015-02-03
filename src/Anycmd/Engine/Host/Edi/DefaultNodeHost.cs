
namespace Anycmd.Engine.Host.Edi
{
    using Handlers;
    using Hecp;
    using MemorySets;

    public class DefaultNodeHost : NodeHost
    {
        public DefaultNodeHost(IAcDomain acDomain)
        {
            this.StateCodes = new StateCodeSet(acDomain);
            this.HecpHandler = new HecpHandler();
            this.MessageProducer = new DefaultMessageProducer();
            this.Ontologies = new OntologySet(acDomain);
            this.Processs = new ProcesseSet(acDomain);
            this.Nodes = new NodeSet(acDomain);
            this.InfoDics = new InfoDicSet(acDomain);
            this.InfoStringConverters = new InfoStringConverterSet(acDomain);
            this.InfoRules = new InfoRuleSet(acDomain);
            this.MessageProviders = new MessageProviderSet(acDomain);
            this.EntityProviders = new EntityProviderSet(acDomain);
            this.Transfers = new MessageTransferSet(acDomain);
        }
    }
}
