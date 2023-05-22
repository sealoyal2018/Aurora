using Aurora.Domain.System;

namespace Aurora.Application.Contract.Systems.Dtos.TenantDtos;
public class CreateTenantDto {
    public Tenant Tenant { get; set; }
    public User User { get; set; }
}
