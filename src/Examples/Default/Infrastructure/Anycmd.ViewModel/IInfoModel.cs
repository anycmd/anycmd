
namespace Anycmd.ViewModel
{

    /// <summary>
    /// 表示该接口的实现类是实体详细信息视图模型。
    /// <remarks>
    /// 数据库的视图也是用来作为展示模型的，所以该接口是继承数据库视图模型接口的，对于在数据库中有外键引用关系的列应在数据库中完成
    /// 引用标识列到人类可读的名称列的转化，而对于字典型的字段和对于常驻内存的那些字段的编码或标识到名称的转化应在应用系统的内存中
    /// 完成而不应在数据库进程完成。
    /// </remarks>
    /// </summary>
    public interface IInfoModel : IDbViewModel
    {
    }
}
