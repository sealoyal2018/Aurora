using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;

namespace Aurora.Domain.Shared.Extensions;

public static class ModelBuilderExtensions {
    private static readonly ValueConverter<DateTime, DateTime> _dateTimeConverter
        = new ValueConverter<DateTime, DateTime>(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Local));

    public static void AutoGenerate(this ModelBuilder self) {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        var entityTypes = assemblies.SelectMany(p => p.GetTypes())
            .Where(p => p.GetCustomAttribute(typeof(TableAttribute), false) != null);

        foreach (var entityType in entityTypes) {
            var entity = self.Entity(entityType);
            ConfigureSoftDelete(entity);
        }

        foreach (var entityType in self.Model.GetEntityTypes()) {
            foreach (var property in entityType.GetProperties()) {
                //DateTime默认为Local
                if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    property.SetValueConverter(_dateTimeConverter);
            }
        }
    }

    /// <summary>
    /// 过滤器增加软删除过滤
    /// </summary>
    /// <param name="builder"></param>
    private static void ConfigureSoftDelete(EntityTypeBuilder entityTypeBuilder) {
        var entityType = entityTypeBuilder.Metadata;
        var prop = entityType.FindProperty("IsDeleted");
        if (prop is null)
            return;
        

        if (!prop.DeclaringEntityType.ClrType.GetProperties().Any(t => t.Name == "IsDeleted"))
            return;

        var parameter = Expression.Parameter(entityType.ClrType, "IsDeleted");

        // 添加过滤器
        var body = Expression.Equal(
            Expression.Call(typeof(EF), nameof(EF.Property), new[] { typeof(bool) }, parameter,
                Expression.Constant("IsDeleted")),
            Expression.Constant(false));
        entityTypeBuilder.HasQueryFilter(Expression.Lambda(body, parameter));
    }
}