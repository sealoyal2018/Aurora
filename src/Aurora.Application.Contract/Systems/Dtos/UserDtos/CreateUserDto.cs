using Aurora.Domain.System.Shared;

namespace Aurora.Application.Contract.Systems.Dtos.UserDtos; 

public class CreateUpdateUserDto {
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string RePassword { get; set; }
    public string NickName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public GenderType Gender { get; set; }
    public DataAccessType AccessType { get; set; }
    public List<string> OrganizeIds { get; set; }
}