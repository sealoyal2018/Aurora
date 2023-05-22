namespace Aurora.Domain.Shared.Exceptions;

/// <summary>
/// 业务异常类
/// </summary>
public class BusException : Exception {
    /// <summary>
    /// 错误码
    /// </summary>
    public BusCodeType Code { get; }

    /// <summary>
    /// 是否成功
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// 附带信息
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// 构建新的业务异常
    /// </summary>
    /// <param name="isSuccess"></param>
    /// <param name="message"></param>
    /// <param name="code"></param>
    public BusException(bool isSuccess, string message, BusCodeType code) {
        IsSuccess = isSuccess;
        Message = message;
        Code = code;
    }

    /// <summary>
    /// 附带信息
    /// </summary>
    public BusException(string message, BusCodeType code) : this(false, message, code) { }


    /// <summary>
    /// 附带信息
    /// </summary>
    public BusException(string message) : this(false, message, BusCodeType.NotAcceptable) { }
}
