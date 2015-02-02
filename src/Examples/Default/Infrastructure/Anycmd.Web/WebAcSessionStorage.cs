
namespace Anycmd.Web
{
    using Engine.Host;
    using System.Web;

    /// <summary>
    /// 基于Web的用户上下文贮存器，存储在Http Session中
    /// </summary>
    public sealed class WebAcSessionStorage : IAcSessionStorage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetData(string key)
        {
            return HttpContext.Current.Session[key];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void SetData(string key, object data)
        {
            HttpContext.Current.Session.Add(key, data);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            HttpContext.Current.Session.Clear();
        }
    }
}
