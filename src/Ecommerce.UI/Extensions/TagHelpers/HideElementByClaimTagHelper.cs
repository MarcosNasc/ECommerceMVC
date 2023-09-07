using Ecommerce.UI.Authorization;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Ecommerce.UI.Extensions.TagHelpers
{
    [HtmlTargetElement("*",Attributes = "supress-by-claim-name" )]
    [HtmlTargetElement("*",Attributes = "supress-by-claim-value" )]
    public class HideElementByClaimTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        [HtmlAttributeName("supress-by-claim-name")]
        public string IdentityClaimName { get; set; }

        [HtmlAttributeName("supress-by-claim-value")]
        public string IdentityClaimValue { get; set; }

        public HideElementByClaimTagHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            if (output is null) throw new ArgumentNullException(nameof(output));

            var hasAccess = CustomAuthorization.ValidateUserClaims(_httpContextAccessor.HttpContext,IdentityClaimName,IdentityClaimValue);

            if (hasAccess) return;

            output.SuppressOutput();    
        }
    }
}
