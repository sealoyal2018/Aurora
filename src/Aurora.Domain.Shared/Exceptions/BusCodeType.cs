using System.ComponentModel;

namespace Aurora.Domain.Shared.Exceptions;

/// <summary>
/// 业务回应码
/// </summary>
public enum BusCodeType : Int32 {
    /// <summary>
    /// 请求成功
    /// </summary>
    [Description("请求成功")]
    OK = 200,

    /// <summary>
    /// 成功请求并创建了新的资源
    /// </summary>
    [Description("成功请求并创建了新的资源")]
    Created = 201,

    /// <summary>
    /// 服务器成功处理，但未返回内容
    /// </summary>
    [Description("服务器成功处理，但未返回内容")]
    NoContent = 204,

    /// <summary>
    /// 客户端请求的语法错误，服务器无法理解
    /// </summary>
    [Description("客户端请求的语法错误，服务器无法理解")]
    BadRequest = 400,
    /// <summary>
    /// 请求要求用户的身份认证
    /// </summary>
    [Description("请求要求用户的身份认证")]
    Unauthorized = 401,

    /// <summary>
    /// 服务器理解请求客户端的请求，但是拒绝执行此请求
    /// </summary>
    [Description("服务器理解请求客户端的请求，但是拒绝执行此请求")]
    Forbidden = 403,

    /// <summary>
    /// 服务器无法根据客户端的请求找到资源
    /// </summary>
    [Description("服务器无法根据客户端的请求找到资源")]
    NotFound = 404,

    /// <summary>
    /// 服务器无法根据客户端请求的内容特性完成请求
    /// </summary>
    [Description("服务器无法根据客户端请求的内容特性完成请求")]
    NotAcceptable = 406,

    /// <summary>
    /// 服务器内部错误，无法完成请求
    /// </summary>
    [Description("服务器内部错误，无法完成请求")]
    InternalServerError = 500,
}
