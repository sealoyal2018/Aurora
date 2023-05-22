namespace Aurora.Domain.Shared.Mappers;

/// <summary>
/// 对象映射配置属性
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class MapperAttribute : Attribute {
    /// <summary>
    /// 目标对象
    /// </summary>
    public Type[] TargetTypes { get; }

    /// <summary>
    /// 构建映射属性
    /// </summary>
    /// <param name="targetTypes">映射目标对象类型</param>
    public MapperAttribute(params Type[] targetTypes) {
        TargetTypes = targetTypes;
    }
}