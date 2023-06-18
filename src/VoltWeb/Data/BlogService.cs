using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using VoltWeb.Collections;
using VoltWeb.Models;

namespace VoltWeb.Data;

/// <summary>
///     This service is responsible for getting blog posts
/// </summary>
public partial class BlogService
{
    private readonly ILogger<BlogService> logger;
    private readonly VoltWebContext dbContext;
    private readonly GraphServiceClient graphServiceClient;
    private readonly IMemoryCache memoryCache;
    private readonly MarkdownService markdownService;

    [GeneratedRegex(@"\b\w+\b")]
    private static partial Regex WordCount();

    /// <summary>
    ///     Creates a new <see cref="BlogService"/> instance
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="dbContext"></param>
    /// <param name="graphServiceClient"></param>
    /// <param name="memoryCache"></param>
    /// <param name="markdownService"></param>
    public BlogService(
        ILogger<BlogService> logger,
        VoltWebContext dbContext,
        GraphServiceClient graphServiceClient,
        IMemoryCache memoryCache,
        MarkdownService markdownService)
    {
        this.logger = logger;
        this.dbContext = dbContext;
        this.graphServiceClient = graphServiceClient;
        this.memoryCache = memoryCache;
        this.markdownService = markdownService;
    }

    public async Task<PaginatedList<BlogPost>> GetBlogPosts(int lastId)
    {
        IQueryable<BlogPost> postsIq = dbContext.Blog.Where(x => x.PublishedDate != null);
        return await PaginatedList<BlogPost>.CreateAsync(postsIq, lastId, 5);
    }

    /// <summary>
    ///     Gets a blog post
    /// </summary>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <param name="day"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<BlogPostData?> GetBlogPost(int year, int month, int day, string id)
    {
        string blogCacheId = $"BlogPost-{year}-{month}-{day}-{id}";
        BlogPostData? blogData = await memoryCache.GetOrCreateAsync<BlogPostData?>(blogCacheId, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6);
            
            BlogPost? foundPage = dbContext.Blog
                .Include(x => x.Hero)
                .Where(x => 
                    x.PostShortId == id)
                .Take(1)
                .FirstOrDefault();
            if (foundPage == null)
                return null;
            
            //Get authors
            BlogAuthor[] authors = dbContext.BlogAuthors.Where(x => x.BlogId == foundPage.Id).ToArray();
            List<BlogPostData.Author> blogAuthors = new();
            foreach (BlogAuthor blogAuthor in authors)
            {
                try
                {
                    BlogPostData.Author? user = await GetAzureUser(blogAuthor.AzureId);

                    if (user == null)
                    {
                        logger.LogWarning("The user {Id} was not found in Azure!", blogAuthor.AzureId);
                        continue;
                    }
                    
                    blogAuthors.Add(user.Value);
                    logger.LogDebug("Got user {DisplayName}", user.Value.Name);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error getting user {UserId} from Microsoft Graph!", blogAuthor.AzureId);
                }
            }

            //Word count
            int wordCount = WordCount().Count(foundPage.Content);
            
            //Parse markdown
            MarkdownResult markdownResult = markdownService.Parse(foundPage.Content);
            
            return new BlogPostData
            {
                Title = foundPage.Title,
                ReadingTime = wordCount < 400 ? 2 : wordCount / 200,
                PublishedDate = foundPage.PublishedDate!.Value,
                HtmlContent = (MarkupString)markdownResult.HtmlContent,
                ContainsCodeBlocks = markdownResult.ContainsCodeBlocks,
                Authors = blogAuthors.ToArray(),
                Headings = markdownResult.Headings,
                HeroData = new HeroData
                {
                    Title = foundPage.Hero.Title,
                    Text = foundPage.Hero.Text
                }
            };
        });

        return blogData;
    }

    private async Task<BlogPostData.Author?> GetAzureUser(string azureId)
    {
        BlogPostData.Author? user = await memoryCache.GetOrCreateAsync($"AzureAd-{azureId}", async entry =>
        {
            BlogPostData.Author? author = null;
            IUserRequestBuilder userBuilder = graphServiceClient.Users[azureId];
            User? user = await userBuilder.Request().WithAppOnly().GetAsync();
            if (user != null)
            {
                //Get photo stream
                Stream? photoStream = await userBuilder.Photo.Content
                    .Request(new Option[]{new QueryOption("size", "64x64")})
                    .WithAppOnly()
                    .GetAsync();
                
                //Read the photo's bytes
                StreamReader streamReader = new(new CryptoStream(photoStream, new ToBase64Transform(), CryptoStreamMode.Read));
                string photoBytes = await streamReader.ReadToEndAsync();

                //Create author
                author = new BlogPostData.Author
                {
                    Name = user.DisplayName,
                    ImageBytes = photoBytes
                };
            }

            //We will expire the cache in 6 hours
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6);

            return author;
        });

        return user;
    }
}