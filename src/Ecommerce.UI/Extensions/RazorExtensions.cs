using Microsoft.AspNetCore.Mvc.Razor;

namespace Ecommerce.UI.Extensions
{
    public static class RazorExtensions
    {
        public static string FormatDocument(this RazorPage page , int supplierType , string document)
        {
            string documentFormated = supplierType switch
            {
                1 => Convert.ToUInt64(document).ToString(@"000\.000\.000\-00"),
                2 => Convert.ToUInt64(document).ToString(@"00\.000\.000\/0000\-00"),
                _ => string.Empty 
            };

            return documentFormated;
        }
    }
}
