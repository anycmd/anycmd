
namespace Anycmd.Engine.Host.Edi
{
    using Engine.Edi.Abstractions;
    using Entities;

    /// <summary>
    /// 
    /// </summary>
    public interface INodeHostBootstrap
    {
        /// <summary>
        /// 获取所有归档<see cref="IArchive"/>
        /// </summary>
        /// <returns></returns>
        Archive[] GetArchives();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Element[] GetElements();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        InfoDicItem[] GetInfoDicItems();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        InfoDic[] GetInfoDics();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        NodeElementAction[] GetNodeElementActions();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        NodeElementCare[] GetNodeElementCares();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        NodeOntologyCare[] GetNodeOntologyCares();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        NodeOntologyCatalog[] GetNodeOntologyCatalogs();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Node[] GetNodes();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Ontology[] GetOntologies();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        InfoGroup[] GetInfoGroups();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Action[] GetActions();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Topic[] GetTopics();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        OntologyCatalog[] GetOntologyCatalogs();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Process[] GetProcesses();
    }
}
