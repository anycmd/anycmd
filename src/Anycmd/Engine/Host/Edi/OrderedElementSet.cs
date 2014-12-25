
namespace Anycmd.Engine.Host.Edi
{
    using Engine.Edi;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// 排序的本体元素集合。
    /// <remarks>
    /// 该集合中的元素是有先后顺序的。
    /// </remarks>
    /// </summary>
    public sealed class OrderedElementSet : IEnumerable<ElementDescriptor>
    {
        private readonly HashSet<ElementDescriptor> _elements = new HashSet<ElementDescriptor>();
        private readonly List<ElementDescriptor> _list = new List<ElementDescriptor>();

        public OrderedElementSet()
        {

        }

        public int Count
        {
            get
            {
                return _elements.Count;
            }
        }

        public ElementDescriptor this[int index]
        {
            get
            {
                return _list[index];
            }
        }

        public void Add(ElementDescriptor element)
        {
            if (!_elements.Contains(element))
            {
                _elements.Add(element);
                _list.Add(element);
            }
        }

        public IEnumerator<ElementDescriptor> GetEnumerator()
        {
            return ((IEnumerable<ElementDescriptor>) _list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}
