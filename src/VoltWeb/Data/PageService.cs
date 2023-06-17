using Markdig;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using VoltWeb.Models;

namespace VoltWeb.Data;

public class PageService
{
    private readonly IDbContextFactory<VoltWebContext> factory;
    
    public PageService(IDbContextFactory<VoltWebContext> contextFactory)
    {
        this.factory = contextFactory;
    }

    public PageData? GetPage(string id)
    {
        //TODO: Memory caching

        using VoltWebContext context = factory.CreateDbContext();
        Page? foundPage = context.Page.FirstOrDefault(x => x.Id == id);
        if (foundPage == null)
            return null;

        return new PageData
        {
            Name = foundPage.Title,
            //HtmlContent = (MarkupString)Markdown.ToHtml(foundPage.Content)
        };
    }
}