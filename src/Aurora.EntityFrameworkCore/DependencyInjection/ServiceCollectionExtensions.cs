using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aurora.EntityFramework.DependencyInjection;

/// <summary>
/// ServiceCollection拓展
/// </summary>
public static class ServiceCollectionExtensions {
    /// <summary>
    /// 添加 efcore 支持
    /// </summary>
    /// <param name="self"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddEFCore(this IServiceCollection self, ConfigurationManager configuration) {
        var database = configuration.GetSection(nameof(Database)).Get<Database>();

        switch (database.Type) {
            case DatabaseCons.MYSQL:
                self.AddDbContext<AppDbContext>(opts => {
                    opts.UseMySql(database.Connection, ServerVersion.Parse("8.0.23"));
                });
                break;
            case DatabaseCons.POSTGRESQL:
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
                self.AddDbContext<AppDbContext>(opts => {
                    opts.UseNpgsql(database.Connection);
                });
                break;
            default:
                self.AddDbContext<AppDbContext>(opts => {
                    opts.UseSqlServer(database.Connection);
                });
                break;
        }

        return self;
    }
}
