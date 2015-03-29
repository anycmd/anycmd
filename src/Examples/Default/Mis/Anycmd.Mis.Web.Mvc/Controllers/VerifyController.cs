using System;
using System.Drawing;
using System.Web.Mvc;

namespace Anycmd.Mis.Web.Mvc.Controllers
{
    public class VerifyController : Controller
    {
        public void Index()
        {
            var v = new VeriCode();
            string code = v.CreateVerifyCode();
            using (var ms = new System.IO.MemoryStream())
            {
                var context = ControllerContext.HttpContext;
                using (Bitmap image = v.CreateImageCode(code))
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                    context.Response.ClearContent();
                    context.Response.ContentType = "image/jpeg";
                    context.Response.BinaryWrite(ms.GetBuffer());

                    image.Dispose();
                }
            }
            Session["VeriCode"] = code;
        }
    }

    public class VeriCode
    {
        #region 字段
        private const double Pi = 3.1415926535897932384626433832795;
        private const double Pi2 = 6.283185307179586476925286766559;
        private int _length = 4;
        private int _fontSize = 18;
        private int _padding = 2;
        private bool _chaos = true;
        private Color _chaosColor = Color.LightGray;
        private Color _backgroundColor = Color.White;
        private Color[] _colors =
        {
            Color.Black, Color.Red, Color.DarkBlue, Color.Green, 
            Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple
        };
        private string _codeSerial = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
        private string[] _fonts = { "Arial", "Georgia" };
        #endregion

        #region 属性
        /// <summary>
        /// 验证码长度（默认4个长度）
        /// </summary>
        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }

        /// <summary>
        /// 验证码字体大小(为了显示扭曲效果，默认18像素，可以自行修改)
        /// </summary>
        public int FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; }
        }

        /// <summary>
        /// 边框补(默认2像素)
        /// </summary>
        public int Padding
        {
            get { return _padding; }
            set { _padding = value; }
        }

        /// <summary>
        /// 是否输出燥点(默认输出)
        /// </summary>
        public bool Chaos
        {
            get { return _chaos; }
            set { _chaos = value; }
        }

        /// <summary>
        /// 输出燥点的颜色(默认灰色)
        /// </summary>
        public Color ChaosColor
        {
            get { return _chaosColor; }
            set { _chaosColor = value; }
        }

        /// <summary>
        /// 自定义背景色(默认白色)
        /// </summary>
        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }

        /// <summary>
        /// 自定义随机颜色数组
        /// </summary>
        public Color[] Colors
        {
            get { return _colors; }
            set { _colors = value; }
        }

        /// <summary>
        /// 自定义字体数组
        /// </summary>
        public string[] Fonts
        {
            get { return _fonts; }
            set { _fonts = value; }
        }

        /// <summary>
        /// 自定义随机码字符串序列(使用逗号分隔)
        /// </summary>
        public string CodeSerial
        {
            get { return _codeSerial; }
            set { _codeSerial = value; }
        }
        #endregion

        #region 产生波形滤镜效果
        /// <summary>  
        /// 正弦曲线Wave扭曲图片
        /// </summary>  
        /// <param name="srcBmp">图片路径</param>  
        /// <param name="bXDir">如果扭曲则选择为True</param>  
        /// <param name="dMultValue">波形的幅度倍数，越大扭曲的程度越高，一般为3</param>  
        /// <param name="dPhase">波形的起始相位，取值区间[0-2*PI)</param>  
        /// <returns></returns>  
        public Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            var destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);

            // 将位图背景填充为白色  
            var graph = Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();

            double dBaseAxisLen = bXDir ? destBmp.Height : destBmp.Width;

            for (var i = 0; i < destBmp.Width; i++)
            {
                for (var j = 0; j < destBmp.Height; j++)
                {
                    double dx = (bXDir ? (Pi2 * j) / dBaseAxisLen : (Pi2 * i) / dBaseAxisLen) + dPhase;
                    double dy = Math.Sin(dx);

                    // 取得当前点的颜色  
                    int nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    int nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                    Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                     && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }

            return destBmp;
        }
        #endregion

        #region 生成校验码图片
        public Bitmap CreateImageCode(string code)
        {
            int fSize = FontSize;
            int fWidth = fSize + Padding;
            int imageWidth = code.Length * fWidth + 6 + Padding * 2;
            int imageHeight = fSize * 2 + Padding;
            var image = new Bitmap(imageWidth, imageHeight);
            Graphics g = Graphics.FromImage(image);
            g.Clear(BackgroundColor);
            var rand = new Random();

            //给背景添加随机生成的燥点  
            if (this.Chaos)
            {

                var pen = new Pen(ChaosColor, 0);
                int c = Length * 10;

                for (int i = 0; i < c; i++)
                {
                    int x = rand.Next(image.Width);
                    int y = rand.Next(image.Height);

                    g.DrawRectangle(pen, x, y, 1, 1);
                }
            }

            int top1 = 1, top2 = 1;

            int n1 = (imageHeight - FontSize - Padding * 2);
            int n2 = n1 / 4;
            top1 = n2;
            top2 = n2 * 2;

            //随机字体和颜色的验证码字符  
            for (int i = 0; i < code.Length; i++)
            {
                int cindex = rand.Next(Colors.Length - 1);
                int findex = rand.Next(Fonts.Length - 1);

                var f = new Font(Fonts[findex], fSize, FontStyle.Bold);
                Brush b = new SolidBrush(Colors[cindex]);

                int top = i % 2 == 1 ? top2 : top1;

                int left = i * fWidth;

                g.DrawString(code.Substring(i, 1), f, b, left, top);
            }

            //画一个边框 边框颜色为Color.Gainsboro  
            g.DrawRectangle(new Pen(Color.Gainsboro, 0), 0, 0, image.Width - 1, image.Height - 1);
            g.Dispose();

            //产生波形（Add By 51aspx.com）  
            image = TwistImage(image, false, 8, 4);

            return image;
        }
        #endregion

        #region 生成随机字符码
        public string CreateVerifyCode()
        {
            string[] arr = CodeSerial.Split(',');

            string code = "";

            var rand = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < Length; i++)
            {
                int randValue = rand.Next(0, arr.Length - 1);

                code += arr[randValue];
            }

            return code;
        }
        #endregion
    }  
}