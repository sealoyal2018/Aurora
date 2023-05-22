namespace Aurora.Domain.Shared.Core.Web;

public class JsonResult<T> : JsonResultBase {
    public T Data { get; set; }
}