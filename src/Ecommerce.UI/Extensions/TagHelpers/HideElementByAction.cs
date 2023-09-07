using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Ecommerce.UI.Extensions.TagHelpers
{
    [HtmlTargetElement("*",Attributes = "supress-by-action")]
    public class HideElementByActionTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        [HtmlAttributeName("supress-by-action")]
        public string ActionName { get; set; }

        public HideElementByActionTagHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            if (output is null) throw new ArgumentNullException(nameof(output));

            var action = _httpContextAccessor.HttpContext.GetRouteData().Values["action"].ToString();

            if (ActionName.Contains(action)) return;

            output.SuppressOutput();
        }
    }
}
