
namespace Anycmd.Engine.Host
{
    using System;
    using Util;

    /// <summary>
    /// 一个资源的一次表演。<see cref="BuiltInResourceKind"/>
    /// </summary>
    public sealed class WfAct : IDisposable
    {
        private readonly IAcDomain _host;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="acts"></param>
        /// <param name="actor"></param>
        /// <param name="name"></param>
        public WfAct(IAcDomain host, IStackTrace acts, IWfResource actor, string name)
        {
            if (actor == null)
            {
                throw new ArgumentNullException("actor");
            }
            if (host == null)
            {
                throw new ArgumentNullException("host");
            }
            this._host = host;
            if (host.Config.TraceIsEnabled)
            {
                this.ActorId = actor.Id;
                this.ActorName = actor.Name;
                this.ActorType = actor.BuiltInResourceKind.ToName();
                this.ActingOn = DateTime.Now;
                this.Name = name;
                acts.Trace(this);
            }
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime ActingOn { get; private set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime ActedOn { get; private set; }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 资源标识
        /// </summary>
        public Guid ActorId { get; private set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ActorName { get; private set; }

        /// <summary>
        /// 资源类型
        /// </summary>
        public string ActorType { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (_host.Config.TraceIsEnabled)
            {
                this.ActedOn = DateTime.Now;
            }
        }
    }
}
