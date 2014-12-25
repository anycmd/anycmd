
namespace Anycmd.Edi.Client
{
    using NPOI.SS.UserModel;

    public static class CellExtension
    {
        /// <summary>
        /// 安全地读取Excel单元格中的值。
        /// </summary>
        /// <param name="cell"></param>
        /// <returns>如果单元格为null则返回空字符串</returns>
        public static string SafeToStringTrim(this ICell cell)
        {
            if (cell == null)
            {
                return string.Empty;
            }
            else
            {
                return cell.ToString().Trim();
            }
        }
    }
}
