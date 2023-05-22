using System.ComponentModel.DataAnnotations;

namespace Aurora.Application.Contract.Systems.Dtos.UserDtos; 

public class SignInDto {
    /// <summary>
    /// 用户名
    /// </summary>
    [Required]
    public string UserName { get; set; }

    /// <summary>
    /// 用户密码
    /// </summary>
    [Required]
    public string Password { get; set; }

    /// <summary>
    /// 用户图形验证码
    /// </summary>
    [Required]
    public string Captcha { get; set; }
    
    /// <summary>
    /// 用户图形验证码id
    /// </summary>
    [Required]
    public string CaptchaId { get; set; }
}