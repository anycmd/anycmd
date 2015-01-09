
namespace Anycmd.Engine.Host.Edi
{
    using Handlers;
    using Hecp;
    using MemorySets;

    public class DefaultNodeHost : NodeHost
    {
        public DefaultNodeHost(IAcDomain host)
        {
            this.StateCodes = new StateCodes(host);
            this.HecpHandler = new HecpHandler();
            this.MessageProducer = new DefaultMessageProducer();
            this.Ontologies = new OntologySet(host);
            this.Processs = new ProcesseSet(host);
            this.Nodes = new NodeSet(host);
            this.InfoDics = new InfoDicSet(host);
            this.InfoStringConverters = new InfoStringConverterSet(host);
            this.InfoRules = new InfoRuleSet(host);
            this.MessageProviders = new MessageProviderSet(host);
            this.EntityProviders = new EntityProviderSet(host);
            this.Transfers = new MessageTransferSet(host);
        }
    }
}
