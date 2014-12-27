
namespace Anycmd.Edi.InfoConstraints.Rules
{
    using Engine.Host.Edi;
    using Engine.Info;
    using System;
    using System.ComponentModel.Composition;

    // TODO:使用OntologyTrigger或ElementTrigger验证。因为身份证件号+身份证件类型两个本体元素的联合是一个业务元素整体，涉及到两个或两个以上本体元素的规则属于本体规则而非元素规则。
    /// <summary>
    /// 身份证件号验证器
    /// </summary>
    [Export(typeof(IInfoRule))]
    public sealed class IdCardInfoCheck : InfoRuleBase, IInfoRule
    {
        private const string ActorId = "048A8089-1771-4D9A-B0EA-4AFE00120209";
        private static readonly Guid id = new Guid(ActorId);
        private const string title = "身份证件号验证器";
        private const string description = "检测身份证件号是否合法，否则将当前命令的状态码置为InvalidInfoValue";
        private const string author = "xuexs";

        private const string ADDRESS =
@"11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
        private static readonly string[] VarifyCode1 = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
        private static readonly string[] VarifyCode2 = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');

        public IdCardInfoCheck()
            : base(id, title, author, description)
        {

        }

        /// <summary>
        /// <remarks>
        /// 注意身份证件号要结合着身份证件类型验证。身份证件类型通过CommandInfoItemDescriptor的InfoValueDic属性中读取
        /// </remarks>
        /// </summary>
        /// <param name="value">命令信息项描述器</param>
        /// <returns></returns>
        public ProcessResult Valid(string value)
        {
            try
            {
                bool isValid = true;
                var stateCode = Status.Ok;
                string msg = "身份证件号验证通过";
                if (!string.IsNullOrEmpty(value))
                {
                    // 验证身份证件号
                    value = value.Trim();
                    isValid = CheckIdCard(value);
                    if (!isValid)
                    {
                        stateCode = Status.InvalidInfoValue;
                        msg = "非法的身份证件号";
                    }
                    // TODO:验军官号等
                }

                return new ProcessResult(isValid, stateCode, msg);
            }
            catch (Exception ex)
            {
                return new ProcessResult(ex);
            }
        }

        public static bool CheckIdCard(string value)
        {
            if (value.Length == 18)
            {
                return CheckIdCard18(value);
            }
            else if (value.Length == 15)
            {
                return CheckIdCard15(value);
            }
            else
            {
                return false;
            }
        }

        public static bool CheckIdCard18(string value)
        {
            Int64 n = 0;
            if (Int64.TryParse(value.Remove(17), out n) == false
                || n < Math.Pow(10, 16)
                || Int64.TryParse(value.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }
            if (ADDRESS.IndexOf(value.Remove(2), StringComparison.Ordinal) == -1)
            {
                return false;//省份验证
            }
            string birth = value.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time;
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            char[] Ai = value.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(VarifyCode2[i]) * int.Parse(Ai[i].ToString());
            }

            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (VarifyCode1[y] != value.Substring(17, 1).ToLower())
            {
                return false;//校验码验证
            }

            return true;//符合GB11643-1999标准
        }

        public static bool CheckIdCard15(string value)
        {
            value = value.Replace(" ", "");
            Int64 n = 0;
            if (Int64.TryParse(value, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证
            }
            if (ADDRESS.IndexOf(value.Remove(2), StringComparison.Ordinal) == -1)
            {
                return false;//省份验证
            }

            string birth = value.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time;
            return DateTime.TryParse(birth, out time) != false;
            //符合15位身份证标准
        }

        protected override void Dispose(bool disposing)
        {

        }
    }
}
