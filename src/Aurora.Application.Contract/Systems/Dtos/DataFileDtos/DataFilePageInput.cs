using Aurora.Domain.Shared.Pages;

namespace Aurora.Application.Contract.Systems.Dtos.DataFileDtos; 

public class DataFilePageInput : PageInputBase {
    public int? Types { get; set; }
    
}