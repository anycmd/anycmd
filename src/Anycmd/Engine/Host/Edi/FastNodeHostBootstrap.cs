using System;

namespace Anycmd.Engine.Host.Edi
{
    using Dapper;
    using Engine.Rdb;
    using Entities;
    using Exceptions;
    using System.Data;
    using System.Linq;

    public class FastNodeHostBootstrap : INodeHostBootstrap
    {
        private readonly IAcDomain _host;
        private readonly Guid _dbId = new Guid("A6FDCDC1-E12B-4D92-938F-59FC7D86DF49");
        private RdbDescriptor _db;

        public FastNodeHostBootstrap(IAcDomain host)
        {
            this._host = host;
        }

        public RdbDescriptor Db
        {
            get
            {
                if (_db == null)
                {
                    if (!_host.Rdbs.TryDb(_dbId, out _db))
                    {
                        throw new AnycmdException("意外的数据库标识" + _dbId);
                    }
                }
                return _db;
            }
        }

        public Archive[] GetArchives()
        {
            using (var conn = Db.GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<Archive>("select * from [Archive]").ToArray();
            }
        }

        public Element[] GetElements()
        {
            using (var conn = Db.GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<Element>("select * from [Element] where DeletionStateCode=0").ToArray();
            }
        }

        public InfoDicItem[] GetInfoDicItems()
        {
            using (var conn = Db.GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<InfoDicItem>("select * from [InfoDicItem] where DeletionStateCode=0").ToArray();
            }
        }

        public InfoDic[] GetInfoDics()
        {
            using (var conn = Db.GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<InfoDic>("select * from [InfoDic] where DeletionStateCode=0").ToArray();
            }
        }

        public NodeElementAction[] GetNodeElementActions()
        {
            using (var conn = Db.GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<NodeElementAction>("select * from [NodeElementAction]").ToArray();
            }
        }

        public NodeElementCare[] GetNodeElementCares()
        {
            using (var conn = Db.GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<NodeElementCare>("select * from [NodeElementCare]").ToArray();
            }
        }

        public NodeOntologyCare[] GetNodeOntologyCares()
        {
            using (var conn = Db.GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<NodeOntologyCare>("select * from [NodeOntologyCare]").ToArray();
            }
        }

        public NodeOntologyOrganization[] GetNodeOntologyOrganizations()
        {
            using (var conn = Db.GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<NodeOntologyOrganization>("select * from [NodeOntologyOrganization]").ToArray();
            }
        }

        public Node[] GetNodes()
        {
            using (var conn = Db.GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<Node>("select * from [Node] where DeletionStateCode=0").ToArray();
            }
        }

        public Ontology[] GetOntologies()
        {
            using (var conn = Db.GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<Ontology>("select * from [Ontology] where DeletionStateCode=0").ToArray();
            }
        }

        public InfoGroup[] GetInfoGroups()
        {
            using (var conn = Db.GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<InfoGroup>("select * from [InfoGroup]").ToArray();
            }
        }

        public Action[] GetActions()
        {
            using (var conn = Db.GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<Action>("select * from [Action]").ToArray();
            }
        }

        public Topic[] GetTopics()
        {
            using (var conn = Db.GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<Topic>("select * from [Topic]").ToArray();
            }
        }

        public OntologyOrganization[] GetOntologyOrganizations()
        {
            using (var conn = Db.GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<OntologyOrganization>("select * from [OntologyOrganization]").ToArray();
            }
        }

        public Process[] GetProcesses()
        {
            using (var conn = Db.GetConnection())
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<Process>("select * from [Process]").ToArray();
            }
        }
    }
}
