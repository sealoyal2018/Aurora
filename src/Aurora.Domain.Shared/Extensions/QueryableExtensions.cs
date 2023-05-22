using Aurora.Domain.Shared.Exceptions;
using Aurora.Domain.Shared.Pages;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using System.Linq;
using System.Linq.Expressions;
using static Aurora.Domain.Shared.Cons.PermissionCons;

namespace Aurora.Domain.Shared.Extensions;

/// <summary>
/// queryable 拓展类
/// </summary>
public static class QueryableExtensions {
    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="self"></param>
    /// <param name="input"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static async Task<PageResult<TEntity>> ToPageAsync<TEntity>(this IQueryable<TEntity> self, PageInput input) {
        var res = new PageResult<TEntity> {
            RowCount = await self.CountAsync()
        };
        res.PageCount = res.RowCount / input.PageSize;
        res.PageIndex = input.PageIndex;
        var orders = input.Order.Split(',');
        IQueryable<TEntity> queryable = null;
        var properties = typeof(TEntity).GetProperties()?.ToList();

        foreach (var item in orders) {
            var arr = item.Split('_');
            if(arr.Length <2) {
                continue;
            }
            var key = arr[0];
            var value = arr[1];
            var prop = properties.FirstOrDefault(t => t.Name == key);
            if(prop is null) {
                continue;
            }
            var param = Expression.Parameter(typeof(TEntity), key);
            var sortingDir = "OrderBy";
            if (string.Compare(value, "desc", true) == 0) {
                sortingDir = "OrderByDescending";
            }
            var types = new Type[2];
            types[0] = typeof(TEntity);
            types[1] = prop.PropertyType;

            if (queryable is null) {
                Expression expr = Expression.Call(typeof(Queryable), sortingDir, types, self.Expression, Expression.Lambda(Expression.Property(param, key), param));
                queryable = self.AsQueryable().Provider.CreateQuery<TEntity>(expr);
            } else {
                var expr = Expression.Call(typeof(Queryable), sortingDir, types, queryable.Expression, Expression.Lambda(Expression.Property(param, key), param));
                queryable = queryable.AsQueryable().Provider.CreateQuery<TEntity>(expr);
            }
        }
        if (queryable is null) {
            res.Data = await self.Skip((input.PageIndex-1)*input.PageSize).Take(input.PageSize).ToListAsync();
        } else {
            res.Data = await queryable.Skip((input.PageIndex - 1) * input.PageSize).Take(input.PageSize).ToListAsync();
        }
        res.Code = BusCodeType.OK;
        res.Message = "查询成功";
        return res;
    }


    /// <summary>
    /// where
    /// </summary>
    /// <param name="queryable"></param>
    /// <param name="condition"></param>
    /// <param name="predicate"></param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static IQueryable<TEntity> WhereIf<TEntity>(this IQueryable<TEntity> queryable, bool condition, Expression<Func<TEntity, bool>> predicate) {
        return !condition ? queryable : queryable.Where(predicate);
    }

}