using System.Collections;

namespace Aurora.Domain.Shared.Extensions; 

public static class ObjectExtensions {
    public static bool IsNullOrEmpty(this object? self) {
        if (self is null)
            return true;
        if (self is string s)
            return string.IsNullOrWhiteSpace(s);
        if (self is IEnumerable t) {
            return t.IsNullOrEmpty();
        }

        return false;
    }
    public static bool IsNotNullOrEmpty(this object? self) {
        if (self is null)
            return false;
        if (self is string s)
            return !string.IsNullOrWhiteSpace(s);
        if (self is IEnumerable t) {
            return !t.IsNullOrEmpty();
        }

        return true;
    }

    public static void Fill<U, T>(this U self, T value)
        where T : class
        where U : class {
        if(self is null) {
            return;
        }
        var exclude = new [] {
            "Id",
            "CreatedTime",
            "CreatorId",
            "IsDeleted",
        };
        var type = self.GetType();
        var properties = type.GetProperties();
        foreach( var property in properties ) {

            if(exclude.Contains(property.Name)) {
                continue;
            }
            var propertyValue = property.GetValue(value);
            property.SetValue(self, propertyValue);
        }
    }

    public static T Default<T>(this object? self, T defaultValue) {
        if (self is null) {
            return defaultValue;
        }

        return (T)self;
    }
    
}