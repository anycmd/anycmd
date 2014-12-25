
namespace Anycmd.Engine.Host.Info
{
    using Exceptions;

    /// <summary>
    /// 两个信息元组。
    /// </summary>
    public sealed class TowInfoTuple
    {
        private InfoItem[] _singleValue;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="infoTuple1"></param>
        /// <param name="infoTuple2"></param>
        public TowInfoTuple(InfoItem[] infoTuple1, InfoItem[] infoTuple2)
        {
            this.InfoTuple1 = infoTuple1;
            this.InfoTuple2 = infoTuple2;

            bool infoValue1IsNullOrEmpty = infoTuple1 == null || infoTuple1.Length == 0;
            bool infoValue2IsNullOrEmpty = infoTuple2 == null || infoTuple2.Length == 0;
            BothHasValue = !infoValue1IsNullOrEmpty && !infoValue2IsNullOrEmpty;
            BothNoValue = infoValue1IsNullOrEmpty && infoValue2IsNullOrEmpty;
            HasValue = !infoValue1IsNullOrEmpty || !infoValue2IsNullOrEmpty;
            if (!BothHasValue && !BothNoValue)
            {
                if (infoValue1IsNullOrEmpty)
                {
                    _singleValue = InfoTuple2;
                }
                else
                {
                    _singleValue = InfoTuple1;
                }
            }
        }

        /// <summary>
        /// 信息值1
        /// </summary>
        public InfoItem[] InfoTuple1 { get; private set; }

        /// <summary>
        /// 信息值2
        /// </summary>
        public InfoItem[] InfoTuple2 { get; private set; }

        /// <summary>
        /// 断定只有一个实体并返回该实体。如果有两个实体或者0个实体将引发异常。
        /// </summary>
        /// <exception cref="CoreException">当两条都有值或都没值的时候引发</exception>
        public InfoItem[] SingleInfoTuple
        {
            get
            {
                if (BothHasValue || BothNoValue)
                {
                    throw new CoreException();
                }

                return _singleValue;
            }
            private set { _singleValue = value; }
        }

        /// <summary>
        /// 两条都有值
        /// </summary>
        public bool BothHasValue { get; private set; }

        /// <summary>
        /// 两条都没值
        /// </summary>
        public bool BothNoValue { get; private set; }

        /// <summary>
        /// 至少一条有值
        /// </summary>
        public bool HasValue { get; private set; }
    }
}
