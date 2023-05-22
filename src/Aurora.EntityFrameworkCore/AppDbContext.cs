using Aurora.Domain;
using Aurora.Domain.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Aurora.EntityFramework;

/// <summary>
/// 应用数据上下文
/// </summary>
public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext {
    /// <summary>
    /// 应用数据上下文
    /// </summary>
    /// <param name="opts"></param>
    /// <param name="userProvider">当前用户提供者</param>
    public AppDbContext(DbContextOptions opts) : base(opts) {
    }

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
#if DEBUG
        optionsBuilder.EnableDetailedErrors();
#endif
        base.OnConfiguring(optionsBuilder);
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.AutoGenerate();
        base.OnModelCreating(modelBuilder);
    }

    /// <inheritdoc />
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default) {
        OnBeforeSaving();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    /// <summary>
    /// 使用审计
    /// </summary>
    private void OnBeforeSaving() {
        var now = DateTime.Now;
        foreach (var entry in ChangeTracker.Entries<EntityBase>()) {
            switch (entry.State) {
                case EntityState.Added:
                    break;

                case EntityState.Modified:
                    entry.CurrentValues["IsDeleted"] = false;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.CurrentValues["IsDeleted"] = true;
                    //if(userProvider.CurrentUser != null)
                    //{
                    //    entry.CurrentValues["DeleterId"] = userProvider.CurrentUser.Id;
                    //    entry.CurrentValues["DeletedTime"] = now;
                    //}
                    break;
                default:
                    break;
            }
        }
    }
}