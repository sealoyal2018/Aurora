using System.ComponentModel.DataAnnotations;

namespace Aurora.Application.Contract;

public class IdsInputDto {

    [Required]
    public List<string> Ids { get; set;}
}
