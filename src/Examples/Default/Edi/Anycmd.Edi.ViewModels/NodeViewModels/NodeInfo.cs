
namespace Anycmd.Edi.ViewModels.NodeViewModels
{
    using Engine.Host.Edi.Handlers;
    using Model;
    using System;
    using System.Collections.Generic;
    using ViewModel;

    /// <summary>
    /// 
    /// </summary>
    public partial class NodeInfo : Dictionary<string, object>
    {
        public NodeInfo() { }

        public NodeInfo(DicReader dic)
        {
            if (dic == null)
            {
                throw new ArgumentNullException("dic");
            }
            foreach (var item in dic)
            {
                this.Add(item.Key, item.Value);
            }
            if (!this.ContainsKey("TransferName"))
            {
                string transferName = string.Empty;
                IMessageTransfer transfer;
                if (dic.AcDomain.NodeHost.Transfers.TryGetTransfer((Guid)this["TransferId"], out transfer))
                {
                    transferName = transfer.Name; ;
                }
                this.Add("TransferName", transferName);
            }
            if (!this.ContainsKey("DeletionStateName"))
            {
                this.Add("DeletionStateName", dic.AcDomain.Translate("Edi", "Node", "DeletionStateName", (int)this["DeletionStateCode"]));
            }
            if (!this.ContainsKey("IsEnabledName"))
            {
                this.Add("IsEnabledName", dic.AcDomain.Translate("Edi", "Node", "IsEnabledName", (int)this["IsEnabled"]));
            }
        }
    }
}
