using Aurora.Domain.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Aurora.Domain.Shared.Core.Web; 

public abstract class ResultBase {
    
    protected IActionResult JsonContent<T>(T data)
    {
        // return new JsonResult(data);
        return new JsonResult(data)
        {
            // Value = System.Text.Json.JsonSerializer.Serialize(data),
            StatusCode = 200,
            ContentType = "application/json; charset=utf-8"
        };
    }

    protected IActionResult Success(string msg)
    {
        return JsonContent(new JsonResult<string>
        {
            Success = true,
            Message = msg,
            Code = BusCodeType.OK
        });
    }
    
    protected IActionResult Success()
    {
        return Success("操作成功");
    }

    protected IActionResult Success<T>(T data)
    {
        return JsonContent(new JsonResult<T>
        {
            Success = true,
            Data = data,
            Code = BusCodeType.OK
        });
    }

    public IActionResult Error(string msg, BusCodeType code)
    {
        return JsonContent(new JsonResult<string>
        {
            Success = false,
            Message = msg,
            Code = code
        });
    }
}