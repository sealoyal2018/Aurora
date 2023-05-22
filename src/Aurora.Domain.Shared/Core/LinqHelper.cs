using System.Linq.Expressions;

namespace Aurora.Domain.Shared.Core;

public class LinqHelper {
    public static Expression<Func<T, bool>> True<T>() {
        return x => true;
    }

    public static Expression<Func<T, bool>> False<T>() {
        return x => false;
    }
}