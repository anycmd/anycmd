
namespace Anycmd.Engine.Edi
{
    using Abstractions;
    using Exceptions;
    using Model;
    using System;
    using Util;

    public sealed class ArchiveState : StateObject<ArchiveState>, IArchive, IStateObject
    {
        public static readonly ArchiveState Empty = new ArchiveState(Guid.Empty)
        {
            ArchiveOn = SystemTime.MinDate,
            Title = string.Empty,
            CreateBy = string.Empty,
            CreateOn = SystemTime.MinDate,
            CreateUserId = Guid.Empty,
            DataSource = string.Empty,
            FilePath = string.Empty,
            NumberId = 0,
            _ontologyId = Guid.Empty,
            Password = string.Empty,
            _rdbmsType = string.Empty,
            UserId = string.Empty
        };
        private string _rdbmsType;
        private Guid _ontologyId;
        private OntologyDescriptor _ontology = null;

        private ArchiveState(Guid id) : base(id) { }

        public static ArchiveState Create(IAcDomain acDomain, ArchiveBase archive)
        {
            if (archive == null)
            {
                throw new ArgumentNullException("archive");
            }
            var data = new ArchiveState(archive.Id)
            {
                AcDomain = acDomain,
                ArchiveOn = archive.ArchiveOn,
                CreateBy = archive.CreateBy,
                CreateOn = archive.CreateOn,
                CreateUserId = archive.CreateUserId,
                DataSource = archive.DataSource,
                FilePath = archive.FilePath,
                NumberId = archive.NumberId,
                OntologyId = archive.OntologyId,
                Password = archive.Password,
                RdbmsType = archive.RdbmsType,
                Title = archive.Title,
                UserId = archive.UserId
            };

            return data;
        }

        public IAcDomain AcDomain { get; private set; }

        public string RdbmsType
        {
            get { return _rdbmsType; }
            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ValidationException("数据库类型不能为空");
                }
                Rdb.RdbmsType dbType;
                if (!value.TryParse(out dbType))
                {
                    throw new AnycmdException("意外的关系数据库类型" + value);
                }
                _rdbmsType = value;
            }
        }

        public string DataSource { get; private set; }

        public string FilePath { get; private set; }

        public int NumberId { get; private set; }

        public string UserId { get; private set; }

        public string Password { get; private set; }

        public string Title { get; private set; }

        public Guid OntologyId
        {
            get { return _ontologyId; }
            private set
            {
                OntologyDescriptor ontology;
                if (!AcDomain.NodeHost.Ontologies.TryGetOntology(value, out ontology))
                {
                    throw new ValidationException("意外的本体标识" + value);
                }
                _ontologyId = value;
            }
        }

        public DateTime ArchiveOn { get; private set; }

        public DateTime? CreateOn { get; private set; }

        public string CreateBy { get; private set; }

        public Guid? CreateUserId { get; private set; }

        /// <summary>
        /// 归档本体
        /// </summary>
        public OntologyDescriptor Ontology
        {
            get
            {
                if (_ontology == null)
                {
                    if (!AcDomain.NodeHost.Ontologies.TryGetOntology(this.OntologyId, out _ontology))
                    {
                        throw new AnycmdException("意外的本体Id" + this.OntologyId);
                    }
                }
                return _ontology;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberId"></param>
        public void Archive(int numberId)
        {
            OntologyDescriptor ontology;
            if (!AcDomain.NodeHost.Ontologies.TryGetOntology(this.OntologyId, out ontology))
            {
                throw new AnycmdException("非法的本体" + this.OntologyId.ToString());
            }
            if (this.Id == Guid.Empty)
            {
                throw new AnycmdException();
            }
            this.ArchiveOn = DateTime.Now;
            this.NumberId = numberId;
            this.FilePath = AcDomain.Config.EntityArchivePath;
            ontology.EntityProvider.Archive(ontology, this);
        }

        protected override bool DoEquals(ArchiveState other)
        {
            return
                Id == other.Id &&
                RdbmsType == other.RdbmsType &&
                DataSource == other.DataSource &&
                FilePath == other.FilePath &&
                NumberId == other.NumberId &&
                UserId == other.UserId &&
                Password == other.Password &&
                Title == other.Title;
        }
    }
}
