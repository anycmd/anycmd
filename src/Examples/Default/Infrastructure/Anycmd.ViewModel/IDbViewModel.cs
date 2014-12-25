
namespace Anycmd.ViewModel
{
    using Model;

    /// <summary>
    /// 标记接口，表示该接口的实现类是数据库视图。
    /// <remarks>
    ///     ViewModel所继承的IDbViewModel接口是一个标记接口，
    ///     所有的与数据库View对应的ViewModel必须继承该接口。
    ///     标记接口用于识别类型，同时它也提供了一个扩展点，可附加扩展方法。
    /// </remarks>
    /// </summary>
    public interface IDbViewModel : IEntity, IViewModel
    {
    }
}
