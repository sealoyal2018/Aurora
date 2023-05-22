using System.ComponentModel.DataAnnotations;

namespace Aurora.Application.Contract;
public class IdInputDto {
    [Required]
    public string Id { get; set; }
}
