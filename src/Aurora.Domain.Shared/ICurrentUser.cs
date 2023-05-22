namespace Aurora.Domain.Shared;

public interface ICurrentUser {
    string UserName { get; }
    string UserId { get; }
    bool IsAuthenticated { get; }
    string[] RoleIds { get; }
    string TenantId { get; }
}
