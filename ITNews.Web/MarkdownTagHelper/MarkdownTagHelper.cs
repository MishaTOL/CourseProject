using CommonMark;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITNews.Web.MarkdownTagHelper
{
    [HtmlTargetElement("p", Attributes = "markdown")]
    [HtmlTargetElement("markdown")]
    [OutputElementHint("p")]
    public class MarkdownTagHelper : TagHelper
    {
        public async override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (output.TagName == "markdown")
            {
                output.TagName = null;
            }
            output.Attributes.RemoveAll("markdown");

            var target = await output.GetChildContentAsync();
            var content = target.GetContent();
            var markdown = content;
            var html = CommonMarkConverter.Convert(markdown);
            output.Content.SetHtmlContent(html ?? "");
        }
    }
}
