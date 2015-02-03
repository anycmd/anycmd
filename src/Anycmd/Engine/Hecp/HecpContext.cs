
namespace Anycmd.Engine.Hecp
{
    using Host;
    using Host.Edi;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Util;

    /// <summary>
    /// Hecp上下文。
    /// </summary>
    public sealed class HecpContext : IStackTrace
    {
        private bool _isValid = false;
        private bool _isValidated = false;
        private readonly HashSet<WfAct> _acts = new HashSet<WfAct>();
        private int _actsCount = 0;
        private string _stackTrace = null;
        private IStackTraceFormater _stackTraceFormater = null;
        private readonly IAcDomain _acDomain;

        #region Ctor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="acDomain"></param>
        /// <param name="request"></param>
        public HecpContext(IAcDomain acDomain, HecpRequest request)
        {
            if (acDomain == null)
            {
                throw new ArgumentNullException("acDomain");
            }
            this.Request = request;
            this._acDomain = acDomain;
            if (request == null)
            {
                _isValidated = true;
                _isValid = false;
                this.Response = HecpResponse.Create(string.Empty);
                this.Response.UpdateStatus(Status.InvalidArgument, "请求参数为null");
            }
            else
            {
                this.Response = HecpResponse.Create(request.MessageId);
            }
        }
        #endregion

        public IAcDomain AcDomain { get { return _acDomain; } }

        /// <summary>
        /// 请求
        /// </summary>
        public HecpRequest Request { get; private set; }

        /// <summary>
        /// 响应
        /// </summary>
        public HecpResponse Response { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (!_isValidated)
                {
                    _isValidated = true;
                    _isValid = true;
                    if (this.Request == null)
                    {
                        this.Response.UpdateStatus(Status.InvalidArgument, "请求参数为null");
                        _isValid = false;
                        return _isValid;
                    }
                    #region 非法的Api版本号
                    ApiVersion apiVersion;
                    MessageType requestType;
                    if (!this.Request.Version.TryParse(out apiVersion))
                    {
                        this.Response.UpdateStatus(Status.InvalidApiVersion, "非法的请求版本号");
                        _isValid = false;
                        return _isValid;
                    }
                    #endregion

                    #region 非法的表述类型
                    if (!this.Request.MessageType.TryParse(out requestType) || requestType == MessageType.Undefined)
                    {
                        this.Response.UpdateStatus(Status.InvalidMessageType, "非法的表述类型");
                        _isValid = false;
                        return _isValid;
                    }
                    #endregion

                    #region 空命令
                    if (Request == null)
                    {
                        this.Response.UpdateStatus(Status.NoneCommand, "未包含命令的请求");
                        _isValid = false;
                        return _isValid;
                    }
                    #endregion

                    #region MessageID是必须的且不能超过36个字符长度
                    if (string.IsNullOrEmpty(Request.MessageId))
                    {
                        this.Response.UpdateStatus(Status.InvalidArgument, "MessageID是必须的");
                        _isValid = false;
                        return _isValid;
                    }
                    if (Request.MessageId.Length > 36)
                    {
                        this.Response.UpdateStatus(Status.OutOfLength, "MessageID超过最大36个字符长度");
                        _isValid = false;
                        return _isValid;
                    }
                    #endregion
                    this.Response.UpdateStatus(Status.Ok, "HecpContext验证通过");
                }
                return _isValid;
            }
        }

        public void Trace(WfAct act)
        {
            this._acts.Add(act);
        }

        #region StackTrace
        /// <summary>
        /// 命令描述对象所经历的处理栈
        /// </summary>
        public string StackTrace
        {
            get
            {
                if (_stackTraceFormater == null)
                {
                    _stackTraceFormater = _acDomain.RetrieveRequiredService<IStackTraceFormater>();
                }
                if (_actsCount == _acts.Count) return _stackTrace;
                _actsCount = _acts.Count;
                _stackTrace = _stackTraceFormater.Format(_acts);
                return _stackTrace;
            }
        }
        #endregion

        public IEnumerator<WfAct> GetEnumerator()
        {
            return ((IEnumerable<WfAct>) _acts).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _acts.GetEnumerator();
        }
    }
}
