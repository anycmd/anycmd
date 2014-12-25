
namespace jDTS.EDI.ViewModels.NodeViewModels {
    using System;
    using ViewModel;

    public class GetPlistNodeOntologyCares : GetPlistResult {
        public Guid nodeID { get; set; }
        public string key { get; set; }
        public bool? isAssigned { get; set; }
    }
}
