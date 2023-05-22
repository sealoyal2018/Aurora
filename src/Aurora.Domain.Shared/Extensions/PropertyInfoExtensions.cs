using Castle.Core.Internal;
using System.ComponentModel;
using System.Reflection;

namespace Aurora.Domain.Shared.Extensions;
public static class PropertyInfoExtensions {
    public static string GetDescriptionName(this PropertyInfo propertyInfo) {
        if (propertyInfo == null) {
            return string.Empty;
        }
        var attr = propertyInfo.GetAttribute<DescriptionAttribute>();
        if(attr is null) { 
            return string.Empty; 
        }
        return attr.Description;
    }
}
