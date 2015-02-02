
namespace Anycmd.Engine.Host
{

    /// <summary>
    /// 表示该接口的实现类是用户上下文贮存器。
    /// </summary>
    public interface IAcSessionStorage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        void SetData(string key, object data);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object GetData(string key);

        /// <summary>
        /// 
        /// </summary>
        void Clear();
    }
}
