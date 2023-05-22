namespace Aurora.Domain.Shared.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple =true, Inherited = true)]
public class ExcelSheetAttribute : Attribute {

    /// <summary>
    /// 列标题
    /// </summary>
    public string ColumnName { get; set; }

    /// <summary>
    /// 数据类型,必填
    /// </summary>
    public string Sheet { get; set; }

    /// <summary>
    /// 逻辑填词, 使用|分隔,前者为真,后者为否
    /// </summary>
    public string BooleanValues { get; set; }
}
