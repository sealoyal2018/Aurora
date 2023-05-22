using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Aurora.Domain.Shared.Core;

public class ImageHelper {
    private const string CHARTS = "abcdefghijklmnoprstuvwxyzABCDEFGHIJKLMNOPRSTUVWXYZ1234567890";
    private static readonly char[] _operators = { '+', '-', '/', '*' };
    private static readonly Color[] Colors =
    {
        Color.Linen,
        Color.Aqua,
        Color.Beige,
        Color.Blue
    };

    /// <summary>
    /// 获取文字型图形验证码
    /// </summary>
    /// <param name="length">文字长度</param>
    /// <param name="width">图片宽度</param>
    /// <param name="height">图片高度</param>
    /// <returns></returns>
    public static async Task<(string, byte[])> GeneratorTextCaptchaAsync(int length, int width, int height) {
        using var image = new Image<Rgba32>(width, height);
        var random = new Random();
        var codes = Enumerable.Range(0, length).Select(p => CHARTS[random.Next(CHARTS.Length)].ToString()).ToList();
        var font = SystemFonts.CreateFont(SystemFonts.Families.First().Name, 25, FontStyle.Bold);
        image.Mutate(ctx => {
            ctx.BackgroundColor(Color.White);
            var gup = width / (codes.Count + 1);
            ;
            // 文字.
            foreach (var i in Enumerable.Range(0, codes.Count)) {
                ctx.DrawText(codes[i], font, Color.Black, new PointF(gup / codes.Count + gup * i, random.Next(height / 4, height / 2)));
            }

            // 画线
            foreach (var index in Enumerable.Range(0, 10)) {
                var x1 = random.Next(image.Width);
                var x2 = random.Next(image.Width);
                var y1 = random.Next(image.Height);
                var y2 = random.Next(image.Height);
                var colorIndex = random.Next(Colors.Length);
                ctx.DrawLines(Colors[colorIndex], random.Next(1, 3), new[] { new PointF(x1, y1), new PointF(x2, y2) });
            }
            // 噪点
            foreach (var index in Enumerable.Range(0, 20)) {
                var x1 = random.Next(image.Width);
                var y1 = random.Next(image.Height);
                var colorIndex = random.Next(Colors.Length);
                ctx.DrawLines(Colors[colorIndex], random.Next(1, 3), new[] { new PointF(x1, y1), new PointF(x1 + 1, y1 + 1) });
            }
        });

        using var ms = new MemoryStream();
        await image.SaveAsPngAsync(ms);
        return (string.Join("", codes), ms.ToArray());
    }

    /// <summary>
    /// 获取计算型图形验证码
    /// </summary>
    /// <param name="width">图片宽度</param>
    /// <param name="height">图片高度</param>
    /// <returns></returns>
    public static async Task<(string, byte[])> GeneratorCalcCaptchaAsync(int width, int height) {
        using var image = new Image<Rgba32>(width, height);
        var random = new Random();
        var operatorIndex = random.Next(_operators.Length);
        var seeds = Enumerable.Range(0, 2).Select(_ => random.Next(1, 9)).ToList();

        var codes = new List<string>();
        var result = 0;
        switch (_operators[operatorIndex]) {
            case '+':
                codes.Add(seeds[0].ToString());
                codes.Add("+");
                codes.Add(seeds[1].ToString());
                codes.Add("=");
                result = seeds[1] + seeds[0];
                break;
            case '-':
                codes.Add(seeds.Max().ToString());
                codes.Add("-");
                codes.Add(seeds.Min().ToString());
                codes.Add("=");
                result = seeds.Max() - seeds.Min();
                break;
            case '/':
                codes.Add((seeds[0] * seeds[1]).ToString());
                codes.Add("÷");
                codes.Add(seeds.Min().ToString());
                codes.Add("=");
                result = seeds.Max();
                break;
            case '*':
                codes.Add((seeds[0]).ToString());
                codes.Add("×");
                codes.Add(seeds[1].ToString());
                codes.Add("=");
                result = seeds[0] * seeds[1];
                break;
            default:
                break;
        }
        var font = SystemFonts.CreateFont(SystemFonts.Families.First().Name, 25, FontStyle.Bold);
        image.Mutate(ctx => {
            ctx.BackgroundColor(Color.White);
            // 文字.
            foreach (var i in Enumerable.Range(0, codes.Count)) {
                ctx.DrawText(codes[i], font, Color.Black, new PointF(width / 5 * i + width / 10, (int)random.Next(height / 4, height / 2)));
            }

            // 画线
            foreach (var index in Enumerable.Range(0, 10)) {
                var x1 = random.Next(image.Width);
                var x2 = random.Next(image.Width);
                var y1 = random.Next(image.Height);
                var y2 = random.Next(image.Height);
                var colorIndex = random.Next(Colors.Length);
                ctx.DrawLines(Colors[colorIndex], random.Next(1, 3), new[] { new PointF(x1, y1), new PointF(x2, y2) });
            }
            // 噪点
            foreach (var index in Enumerable.Range(0, 70)) {
                var x1 = random.Next(image.Width);
                var y1 = random.Next(image.Height);
                var colorIndex = random.Next(Colors.Length);
                ctx.DrawLines(Colors[colorIndex], random.Next(1, 3), new[] { new PointF(x1, y1), new PointF(x1 + 1, y1 + 1) });
            }
        });

        using var ms = new MemoryStream();
        await image.SaveAsPngAsync(ms);
        return (result.ToString(), ms.ToArray());
    }

}