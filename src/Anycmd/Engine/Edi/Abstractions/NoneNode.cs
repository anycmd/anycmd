
namespace Anycmd.Engine.Edi.Abstractions
{
    using System;
    using Util;

    public class NoneNode : INode
    {
        public static readonly NoneNode SingleInstance = new NoneNode();

        private NoneNode() { }

        public Guid Id
        {
            get { return Guid.Empty; }
        }

        public string Name
        {
            get { return "未定义的节点"; }
        }

        public string Code
        {
            get { return string.Empty; }
        }

        public string Actions
        {
            get { return null; }
        }

        public string Abstract
        {
            get { return string.Empty; }
        }

        public string Catalog
        {
            get { return string.Empty; }
        }

        public string Steward
        {
            get { return string.Empty; }
        }

        public string Telephone
        {
            get { return string.Empty; }
        }

        public string Email
        {
            get { return string.Empty; }
        }

        public string Mobile
        {
            get { return string.Empty; }
        }

        public string Qq
        {
            get { return string.Empty; }
        }

        public string Icon
        {
            get { return string.Empty; }
        }

        public int IsEnabled
        {
            get { return 0; }
        }

        public bool IsExecuteEnabled
        {
            get { return false; }
        }

        public bool IsProduceEnabled
        {
            get { return false; }
        }

        public bool IsReceiveEnabled
        {
            get { return false; }
        }

        public bool IsDistributeEnabled
        {
            get { return false; }
        }

        public Guid TransferId
        {
            get { return Guid.Empty; }
        }

        public string AnycmdApiAddress
        {
            get { return string.Empty; }
        }

        public string AnycmdWsAddress
        {
            get { return string.Empty; }
        }

        public int? BeatPeriod
        {
            get { return int.MaxValue; }
        }

        public string PublicKey
        {
            get { return string.Empty; }
        }

        public string SecretKey
        {
            get { return string.Empty; }
        }

        public int SortCode
        {
            get { return -1; }
        }

        public DateTime? CreateOn
        {
            get { return SystemTime.MinDate; }
        }
    }
}
