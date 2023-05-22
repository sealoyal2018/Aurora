using System.ComponentModel;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Aurora.Domain.Shared.Pages;

public class PageInput {
    /// <summary>
    /// 当前请求页
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// 请求页数量大小
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// 分页排序, 使用方法: 正序 name_asc, 倒叙 name_desc, 多个英文分号分割
    /// </summary>
    public string Order { get; set; } = "Id_desc";
}

/// <summary>
/// 排序类型
/// </summary>
public enum PageSortType {
    /// <summary>
    /// 增序
    /// </summary>
    [Description("增序")]
    Asc = 0,

    /// <summary>
    /// 倒序
    /// </summary>
    [Description("倒序")]
    Desc = 1,
}

public abstract class PageInputBase: PageInput {
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string Keyword { get; set; }
    public ConditionType ConditionType { get; set; } = 0;
    public string KeywordValue { get; set; }
}

public enum ConditionType {
    [Description("包含")]
    Include = 0,
    
    [Description("小于")]
    Less = 1,
    
    [Description("大于")]
    Greater = 2,
    
    [Description("等于")]
    Equal = 3,
    
    [Description("不等于")]
    NoEqual = 4,
    
    [Description("小于")]
    LessEqual = 5,
    
    [Description("大于")]
    GreaterEqual = 6,
}

public static class ExpressionExtensions{
    public static Expression<Func<T, bool>>? BuildCondition<T>(this PageInputBase self) {
        if (!string.IsNullOrWhiteSpace(self.Keyword) && !string.IsNullOrWhiteSpace(self.KeywordValue)) {
            var expression = self.ConditionType switch {
                ConditionType.Equal => "",
                _ => $@"{self.Keyword}.Contains(@0)",
            };
            
            var newWhere = DynamicExpressionParser.ParseLambda<T, bool>(
                ParsingConfig.Default, false, expression, self.KeywordValue);
            return newWhere;
        }

        return default;
    }
}