namespace Aurora.Application.Contract.Systems.Dtos.UserDtos; 

public class CaptchaDto {
    /// <summary>
    /// base64 编码的图形验证码
    /// </summary>
    public string Image { get; set; }
    
    /// <summary>
    /// 图形认证码识别id
    /// </summary>
    public string ImageId { get; set; }
}