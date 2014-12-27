
namespace Anycmd.Engine.Info
{
    using Host.Edi;
    using System;

    /// <summary>
    /// <see cref="IDataTuples"/>
    /// </summary>
    public sealed class DataTuple : IDataTuples
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="rows"></param>
        public DataTuple(OrderedElementSet columns, object[][] rows)
        {
            if (columns == null || columns.Count == 0)
            {
                throw new ArgumentNullException("columns");
            }
            if (rows == null)
            {
                throw new ArgumentNullException("rows");
            }
            this.Columns = columns;
            this.Tuples = rows;
        }

        public OrderedElementSet Columns { get; private set; }

        public object[][] Tuples { get; private set; }
    }
}
