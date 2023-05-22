namespace Aurora.Domain.Shared.Extensions;
public static class StringExtensions {
    public static bool IsNotNullOrEmpty(this string? self) {
        return !string.IsNullOrWhiteSpace(self);
    }

    public static bool IsNullOrEmpty(this string? self) {
        return string.IsNullOrWhiteSpace(self);
    }

    public static bool IsIn(this string self, params string[] arr) {
        return arr.Contains(self);
    }
    
    public static bool IsNotIn(this string self, params string[] arr) {
        return !arr.Contains(self);
    }
}
