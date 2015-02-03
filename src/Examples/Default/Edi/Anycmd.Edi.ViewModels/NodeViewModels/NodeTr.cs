
namespace Anycmd.Edi.ViewModels.NodeViewModels
{
    using Engine.Edi;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public partial class NodeTr
    {
        private bool _isSelfDetected = false;
        private bool _isSelf = false;
        private bool _isCenterDetected = false;
        private bool _isCenter = false;
        private readonly IAcDomain _acDomain;

        private NodeTr(IAcDomain acDomain)
        {
            this._acDomain = acDomain;
        }

        public static NodeTr Create(NodeDescriptor node)
        {
            return new NodeTr(node.AcDomain)
            {
                AnycmdApiAddress = node.Node.AnycmdApiAddress,
                AnycmdWsAddress = node.Node.AnycmdWsAddress,
                BeatPeriod = node.Node.BeatPeriod,
                Code = node.Node.Code,
                CreateOn = node.Node.CreateOn,
                Email = node.Node.Email,
                Icon = node.Node.Icon,
                Id = node.Node.Id,
                IsDistributeEnabled = node.Node.IsDistributeEnabled,
                IsEnabled = node.Node.IsEnabled,
                IsExecuteEnabled = node.Node.IsExecuteEnabled,
                IsProduceEnabled = node.Node.IsProduceEnabled,
                IsReceiveEnabled = node.Node.IsReceiveEnabled,
                Mobile = node.Node.Mobile,
                Name = node.Node.Name,
                Catalog = node.Node.Catalog,
                PublicKey = node.Node.PublicKey,
                QQ = node.Node.Qq,
                SortCode = node.Node.SortCode,
                Steward = node.Node.Steward,
                Telephone = node.Node.Telephone,
                TransferId = node.Node.TransferId
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 接入单位名称
        /// </summary>
        public string Catalog { get; set; }
        /// <summary>
        /// 专员
        /// </summary>
        public string Steward { get; set; }
        /// <summary>
        /// 固定电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 电子信箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int IsEnabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsExecuteEnabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsProduceEnabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsReceiveEnabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDistributeEnabled { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int SortCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid TransferId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PublicKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateOn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AnycmdApiAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AnycmdWsAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? BeatPeriod { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSelf
        {
            get
            {
                if (!_isSelfDetected)
                {
                    _isSelfDetected = true;
                    _isSelf = this.Id == _acDomain.NodeHost.Nodes.ThisNode.Node.Id;
                }
                return _isSelf;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsCenter
        {
            get
            {
                if (!_isCenterDetected)
                {
                    _isCenterDetected = true;
                    _isCenter = this.Id == _acDomain.NodeHost.Nodes.CenterNode.Node.Id;
                }
                return _isCenter;
            }
        }
    }
}
