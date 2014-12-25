
namespace Anycmd.DataContracts
{

    /// <summary>
    /// 心跳响应
    /// </summary>
    public interface IBeatResponse
    {
        /// <summary>
        /// 
        /// </summary>
        string Description { get; }
        /// <summary>
        /// 
        /// </summary>
        bool IsAlive { get; }
        /// <summary>
        /// 
        /// </summary>
        string ReasonPhrase { get; }
        /// <summary>
        /// 
        /// </summary>
        int Status { get; }
    }
}
