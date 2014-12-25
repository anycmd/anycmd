
namespace Anycmd.Engine.Host.Edi
{
    using Exceptions;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    /// 状态码枚举 Info [0-200}success [200-300}fail[400-500] 
    /// </summary>
    public enum Status : short
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("未定义")]
        Undefined = 0,

        #region 信息 [0-200}
        /// <summary>
        /// 非法的版本号
        /// </summary>
        [Description("非法的版本号")]
        InvalidApiVersion = 1,
        /// <summary>
        /// 非法的表述类型
        /// </summary>
        [Description("非法的表述类型")]
        InvalidMessageType = 2,
        /// <summary>
        /// 非法的事件源类型
        /// </summary>
        [Description("非法的事件源类型")]
        InvalidEventSourceType = 3,
        /// <summary>
        /// 非法的事件主题码
        /// </summary>
        [Description("非法的事件主题码")]
        InvalidEventSubject = 4,
        /// <summary>
        /// 非法的客户类型
        /// </summary>
        [Description("非法的客户类型")]
        InvalidClientType = 5,
        /// <summary>
        /// 意外的证书类型
        /// </summary>
        [Description("非法的证书类型")]
        InvalidCredentialType = 6,
        /// <summary>
        /// 非法的状态码或原因短语
        /// </summary>
        [Description("非法的状态码")]
        InvalidStatus = 7,
        /// <summary>
        /// 节点异常
        /// </summary>
        [Description("节点异常。如，目标节点对数据交换协议实现不正确。")]
        NodeException = 8,
        /// <summary>
        /// 非法的用户类型
        /// </summary>
        [Description("非法的用户类型")]
        InvalidUserType = 9,
        /// <summary>
        /// 不支持
        /// </summary>
        [Description("不支持")]
        Nonsupport = 10,

        /// <summary>
        /// 传入的参数错误
        /// </summary>
        [Description("传入的参数错误。服务端未解析出必选参数。")]
        InvalidArgument = 100,
        /// <summary>
        /// 未传入证书对象
        /// </summary>
        [Description("未传入证书对象。")]
        NoneCredential = 101,
        /// <summary>
        /// 非法的时间戳
        /// </summary>
        [Description("非法的时间戳")]
        InvalidTicks = 102,
        /// <summary>
        /// 非法的客户标识
        /// </summary>
        [Description("非法的客户标识。")]
        InvalidClientId = 103,
        /// <summary>
        /// 已禁止接收来自本发送节点的命令
        /// </summary>
        [Description("已禁止接收来自本发送节点的命令。")]
        ReceiveIsDisabled = 104,
        /// <summary>
        /// 节点身份未认证通过
        /// </summary>
        [Description("节点身份未认证通过。")]
        NotAuthorized = 105,
        /// <summary>
        /// 节点已被禁用
        /// </summary>
        [Description("节点已被禁用。")]
        NodeIsDisabled = 106,
        /// <summary>
        /// 已禁止向该节点发送命令
        /// </summary>
        [Description("已禁止向该节点发送命令。")]
        SendIsDisabled = 107,
        /// <summary>
        /// 已禁止执行来自该节点的命令
        /// </summary>
        [Description("已禁止执行来自该节点的命令。")]
        ExecuteIsDisabled = 108,
        /// <summary>
        /// 未包含命令的请求
        /// </summary>
        [Description("未包含命令的请求。")]
        NoneCommand = 109,
        /// <summary>
        /// 服务当前不可用，请稍后访问
        /// </summary>
        [Description("服务当前不可用，请稍后访问")]
        ServiceIsNotAlive = 110,
        /// <summary>
        /// 非法的分页索引
        /// </summary>
        [Description("非法的分页索引")]
        InvalidPageIndex = 111,
        /// <summary>
        /// 非法的分页尺寸
        /// </summary>
        [Description("非法的分页尺寸")]
        InvalidPageSize = 112,
        #endregion

        #region 成功 [200-300}
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Ok = 200,
        /// <summary>
        /// 成功
        /// </summary>
        [Description("接收成功。")]
        ReceiveOk = 201,
        /// <summary>
        /// 执行成功
        /// </summary>
        [Description("执行成功。")]
        ExecuteOk = 202,
        /// <summary>
        /// 分发成功
        /// </summary>
        [Description("分发成功")]
        DistributeOk = 203,
        /// <summary>
        /// 待审计
        /// </summary>
        [Description("待审计")]
        ToAudit = 204,
        /// <summary>
        /// 审核通过
        /// </summary>
        [Description("审核通过")]
        AuditApproved = 205,
        #endregion

        #region 失败 [400-500]
        /// <summary>
        /// 失败
        /// </summary>
        [Description("失败")]
        Fail = 400,
        /// <summary>
        /// 接收失败
        /// </summary>
        [Description("接收失败。")]
        ReceiveFail = 401,
        /// <summary>
        /// 执行失败
        /// </summary>
        [Description("执行失败。")]
        ExecuteFail = 402,
        /// <summary>
        /// 分发失败
        /// </summary>
        [Description("分发失败")]
        DistributeFail = 403,
        /// <summary>
        /// 不存在
        /// </summary>
        [Description("不存在。给定的信息标识标识的实体记录不存在。")]
        NotExist = 404,
        /// <summary>
        /// 已存在
        /// </summary>
        [Description("已存在。给定的信息标识标识的实体记录已存在。")]
        AlreadyExist = 405,
        /// <summary>
        /// 没有权限
        /// </summary>
        [Description("没有权限。")]
        NoPermission = 406,
        /// <summary>
        /// 非法的本体码
        /// </summary>
        [Description("非法的本体码。")]
        InvalidOntology = 408,
        /// <summary>
        /// 非法的本体元素码
        /// </summary>
        [Description("非法的本体元素码。")]
        InvalidElement = 409,
        /// <summary>
        /// 非法的组织结构码
        /// </summary>
        [Description("非法的组织结构码。")]
        InvalidOrganization = 410,
        /// <summary>
        /// 非法的字典值
        /// </summary>
        [Description("非法的字典值。")]
        InvalidDicItemValue = 411,
        /// <summary>
        /// 非法的信息格式
        /// </summary>
        [Description("非法的信息格式。")]
        InvalidInfoFormat = 412,
        /// <summary>
        /// 非法的动作码
        /// </summary>
        [Description("非法的动作码。")]
        InvalidVerb = 413,
        /// <summary>
        /// 非法的信息标识
        /// </summary>
        [Description("非法的信息标识。")]
        InvalidInfoId = 414,
        /// <summary>
        /// 非法的信息值
        /// </summary>
        [Description("非法的信息值。")]
        InvalidInfoValue = 415,
        /// <summary>
        /// 根据给定的信息标识不能唯一确定记录
        /// </summary>
        [Description("根据给定的信息标识不能唯一确定记录。")]
        UnUnique = 416,
        /// <summary>
        /// 超过最大长度
        /// </summary>
        [Description("超过最大长度。")]
        OutOfLength = 417,
        /// <summary>
        /// 非法的时间戳
        /// </summary>
        [Description("非法的时间戳。通常这个时间戳不能是一年前或者未来。")]
        InvalidCommandTicks = 418,
        /// <summary>
        /// 审核未通过
        /// </summary>
        [Description("审核未通过。")]
        AuditUnapproved = 419,
        #endregion

        #region 异常
        /// <summary>
        /// 内部服务器错误
        /// </summary>
        [Description("内部服务器错误。")]
        InternalServerError = 500
        #endregion
    }

    /// <summary>
    /// 扩展命令状态码枚举类型和字符串类型和int型，提供高效的命令状态码枚举类型值与字符串类型值和int型值的转换方法。
    /// </summary>
    public static class StatusExtension
    {
        /// <summary>
        /// 转化为枚举名
        /// </summary>
        /// <param name="stateCode"></param>
        /// <returns></returns>
        public static string ToName(this Status stateCode)
        {
            return StateCodeDic.Current[stateCode];
        }

        /// <summary>
        /// 将int数字转化为命令状态码枚举
        /// </summary>
        /// <param name="number"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryParse(this int number, out Status value)
        {
            if (number != 0) return StateCodeDic.TryGetValue(number, out value);
            value = default(Status);
            return false;
        }

        #region StateCodeDic
        /// <summary>
        /// 状态码字典
        /// <remarks>
        /// 枚举类型的ToString()内部是通过反射进行的所以速度很慢，
        /// 当需要状态码枚举对应的原因短语时应通过本字典索引而不
        /// 要通过调枚举值的ToString()方法的形式获取。
        /// </remarks>
        /// </summary>
        private class StateCodeDic
        {
            public static readonly StateCodeDic Current = new StateCodeDic();

            private static readonly Dictionary<int, string> DicByInt = new Dictionary<int, string>();
            private static readonly Dictionary<int, Status> DicByNumber = new Dictionary<int, Status>();

            static StateCodeDic()
            {
                Current.Init();
            }

            // 置为私有
            private StateCodeDic() { }

            /// <summary>
            /// 根据状态码枚举索引原因短语
            /// </summary>
            /// <param name="stateCode">状态码枚举对象</param>
            /// <returns></returns>
            public string this[Status stateCode]
            {
                get
                {
                    return DicByInt[(int)stateCode];
                }
            }

            public static bool TryGetValue(int number, out Status value)
            {
                return DicByNumber.TryGetValue(number, out value);
            }

            /// <summary>
            /// 根据状态码枚举数字索引原因短语
            /// </summary>
            /// <param name="stateCode">状态码枚举数字</param>
            /// <returns></returns>
            public string this[int stateCode]
            {
                get
                {
                    if (!DicByInt.ContainsKey(stateCode))
                    {
                        throw new CoreException("意外的状态码:" + stateCode.ToString());
                    }

                    return DicByInt[stateCode];
                }
            }

            #region private methods
            private void Init()
            {
                string[] names = Enum.GetNames(typeof(Status));
                var values = Enum.GetValues(typeof(Status)) as short[];
                if (values == null)
                {
                    throw new CoreException("枚举Status不是int型的");
                }
                for (int i = 0; i < values.Length; i++)
                {
                    DicByInt.Add(values[i], names[i]);
                    DicByNumber.Add(values[i], (Status)values[i]);
                }
            }
            #endregion
        }
        #endregion
    }
}
