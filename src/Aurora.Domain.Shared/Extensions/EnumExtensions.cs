using Castle.Core.Internal;
using System.ComponentModel;

namespace Aurora.Domain.Shared.Extensions;

public static class EnumExtensions {
    /// <summary>
    /// 获取 description
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static string GetDescriptionText(this Enum self) {
        var type = self.GetType();

        var name = Enum.GetName(type, self);

        var attrs = type.GetField(name).GetAttribute<DescriptionAttribute>();

        return attrs?.Description;
    }
}