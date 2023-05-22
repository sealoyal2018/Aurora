using Aurora.Domain.Shared.Cons;
using Aurora.Domain.Shared.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace Aurora.Domain.Shared.Helpers;

[HtmlTargetElement("a", Attributes = PermissionAttributeName)]
[HtmlTargetElement("button", Attributes = PermissionAttributeName)]
[HtmlTargetElement("input", Attributes = PermissionAttributeName, TagStructure = TagStructure.WithoutEndTag)]
internal class PermissionTagHelper : TagHelper {

    private const string PermissionAttributeName = "asp-code";

    [HtmlAttributeName(PermissionAttributeName)]
    public string Code { get; set; }

    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        if(Code == null) {
            return;
        }

        var services = ViewContext.HttpContext.RequestServices;
        var currrentUser = services.GetService<ICurrentUser>();
        var cache = services.GetService<IDistributedCache>();
        var bytes = cache.Get(string.Format(CacheCons.PermissionKey, currrentUser.UserId));
        if (bytes.IsNullOrEmpty()) {
            output.SuppressOutput();
            return;
        }

        var permissionCodeString = Encoding.UTF8.GetString(bytes);
        var codes = permissionCodeString.Split(',');
        if (Code.IsNotIn(codes)) {
            output.SuppressOutput();
            return;
        }
        base.Process(context, output);
    }
}
