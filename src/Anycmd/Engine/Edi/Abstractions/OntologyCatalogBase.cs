
namespace Anycmd.Engine.Edi.Abstractions
{
    using Exceptions;
    using Model;
    using System;

    public abstract class OntologyCatalogBase : EntityBase, IOntologyCatalog
    {
        private Guid _orgnizationId;
        private Guid _ontologyId;

        protected OntologyCatalogBase() { }

        /// <summary>
        /// 目录标识
        /// </summary>
        public Guid CatalogId
        {
            get { return _orgnizationId; }
            set
            {
                if (value == _orgnizationId) return;
                if (_orgnizationId != Guid.Empty)
                {
                    throw new AnycmdException("不能更改所属目录");
                }
                _orgnizationId = value;
            }
        }

        /// <summary>
        /// 本体标识
        /// </summary>
        public Guid OntologyId
        {
            get { return _ontologyId; }
            set
            {
                if (value == _ontologyId) return;
                if (_ontologyId != Guid.Empty)
                {
                    throw new AnycmdException("不能更改所属本体");
                }
                _ontologyId = value;
            }
        }

        public string Actions { get; set; }
    }
}
