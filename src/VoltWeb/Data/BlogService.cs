using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Graph;
using VoltWeb.Models;

namespace VoltWeb.Data;

public partial class BlogService
{
    private readonly ILogger<BlogService> logger;
    private readonly IDbContextFactory<VoltWebContext> factory;

    [GeneratedRegex(@"\b\w+\b")]
    private static partial Regex WordCount();
    
    public BlogService(ILogger<BlogService> logger, IDbContextFactory<VoltWebContext> contextFactory)
    {
        this.logger = logger;
        this.factory = contextFactory;
    }

    public BlogData? GetBlogPost(int year, int month, int day, string id)
    {
        using VoltWebContext context = factory.CreateDbContext();
        Blog? foundPage = context.Blog.Include(x => x.Hero).FirstOrDefault(x => x.Id == id && x.PostDate.Year == year && x.PostDate.Month == month && x.PostDate.Day == day);
        if (foundPage == null)
            return null;
        
        BlogAuthor[] authors = context.BlogAuthors.Where(x => x.BlogId == foundPage.Id).ToArray();
        foreach (BlogAuthor blogAuthor in authors)
        {
           //TODO: Process authors
        }

        //Word count
        int wordCount = WordCount().Count(foundPage.Content);
        return new BlogData
        {
            Title = foundPage.Title,
            ReadingTime = wordCount < 400 ? 2 : wordCount / 200,
            PublishedDate = foundPage.PostDate,
            HtmlContent = (MarkupString)Markdig.Markdown.ToHtml(foundPage.Content),
            HeroData = new HeroData
            {
                Title = foundPage.Hero.Title,
                Text = foundPage.Hero.Text
            }
        };
    }
}