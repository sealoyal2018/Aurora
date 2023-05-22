using Aurora.Domain.Shared.Exceptions;

namespace Aurora.Domain.Shared.Core.Web;

public abstract class JsonResultBase
{
    public BusCodeType Code { get; set; }
    public string Message { get; set; }
    public bool Success { get; set; } = true;
}