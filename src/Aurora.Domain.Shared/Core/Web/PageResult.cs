using Aurora.Domain.Shared.Core.Web;

namespace Aurora.Domain.Shared.Pages;

/// <summary>
/// 分页数据
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class PageResult<T> : JsonResult<IEnumerable<T>> {
    /// <summary>
    /// 总行数
    /// </summary>
    public int RowCount { get; set; }

    /// <summary>
    /// 总页数
    /// </summary>
    public int PageCount { get; set; }

    /// <summary>
    /// 当前页
    /// </summary>
    public int PageIndex { get; set; }

    public PageResult() {
        Data = new List<T>();
    }
}