
namespace Anycmd.RavenDBTest
{
    using Raven.Client;
    using Raven.Client.Document;

    public class CsStore : DocumentStore
    {
        public static readonly IDocumentStore SingleInstance = new CsStore { Url = "http://localhost:8081/" };

        static CsStore()
        {
            SingleInstance.Initialize();
        }
    }
}
