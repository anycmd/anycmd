
namespace Anycmd.Util
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Text.RegularExpressions;
    using System.Web;

    /// <summary>
    /// IP访问助手
    /// </summary>
    public static class IpHelper
    {
        public static string GetClientIp()
        {
            if (HttpContext.Current == null)
            {
                return IPAddress.Loopback.ToString();
            }
            var ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.UserHostAddress;
            }
            if (ip == "::1")
            {
                ip = "127.0.0.1";
            }

            return ip;
        }

        #region GetLocalIP
        /// <summary>
        /// 得到本机IP
        /// </summary>
        public static string GetLocalIp()
        {
            var strLocalIp = "";
            var strPcName = Dns.GetHostName();
            var ipEntry = Dns.GetHostEntry(strPcName);
            foreach (var ip in ipEntry.AddressList.Where(ip => IsRightIp(ip.ToString())))
            {
                strLocalIp = ip.ToString();
                break;
            }

            return strLocalIp;
        }
        #endregion

        public static HashSet<string> GetLocalIPs()
        {
            var ips = new HashSet<string>(System.StringComparer.OrdinalIgnoreCase);
            var strPcName = Dns.GetHostName();
            var ipEntry = Dns.GetHostEntry(strPcName);
            foreach (var ip in ipEntry.AddressList.Where(ip => IsRightIp(ip.ToString())))
            {
                ips.Add(ip.ToString());
            }

            return ips;
        }

        #region GetGateway
        /// <summary>
        /// 得到网关地址
        /// </summary>
        /// <returns></returns>
        public static string GetGateway()
        {
            var strGateway = "";
            //获取所有网卡
            var nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var gateways in nics.Select(netWork => netWork.GetIPProperties()).Select(ip => ip.GatewayAddresses))
            {
                foreach (var gateWay in gateways.Where(gateWay => IsPingIp(gateWay.Address.ToString())))
                {
                    strGateway = gateWay.Address.ToString();
                    break;
                }

                if (strGateway.Length > 0)
                {
                    break;
                }
            }

            return strGateway;
        }
        #endregion

        #region IsRightIP
        /// <summary>
        /// 判断是否为正确的IP地址
        /// </summary>
        /// <param name="strIPadd">需要判断的字符串</param>
        /// <returns>true = 是 false = 否</returns>
        public static bool IsRightIp(string strIPadd)
        {
            if (Regex.IsMatch(strIPadd, "[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}"))
            {
                //根据小数点分拆字符串
                var ips = strIPadd.Split('.');
                if (ips.Length == 4 || ips.Length == 6)
                {
                    return System.Int32.Parse(ips[0]) < 256 && System.Int32.Parse(ips[1]) < 256 & System.Int32.Parse(ips[2]) < 256 & System.Int32.Parse(ips[3]) < 256;
                }
                else
                    return false;
            }
            else
                return false;
        }
        #endregion

        #region IsPingIP
        /// <summary>
        /// 尝试Ping指定IP是否能够Ping通
        /// </summary>
        /// <param name="strIp">指定IP</param>
        /// <returns>true 是 false 否</returns>
        public static bool IsPingIp(string strIp)
        {
            try
            {
                var ping = new Ping();
                ping.Send(strIp, 1000);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
