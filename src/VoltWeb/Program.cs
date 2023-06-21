using BlazorBootstrap;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VoltWeb.Data;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string[]? initialScopes = builder.Configuration["DownstreamApi:Scopes"]?.Split(' ');

//Setup Azure AD Auth
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
        .AddMicrosoftGraph(builder.Configuration.GetSection("DownstreamApi"))
    .AddInMemoryTokenCaches();

builder.Services.AddControllersWithViews()
    .AddMicrosoftIdentityUI();

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy
    options.FallbackPolicy = options.FallbackPolicy;
});

//YouTube API
//Setup settings first
builder.Services.Configure<BaseClientService.Initializer>(builder.Configuration.GetSection("GoogleApi"));
builder.Services.AddSingleton<YouTubeService>(services => 
    new YouTubeService(services.GetRequiredService<IOptions<BaseClientService.Initializer>>().Value));

//DB Services
builder.Services.AddDbContextFactory<VoltWebContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<VoltWebContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//Add our services
//builder.Services.AddHttpClient();
builder.Services.AddSingleton<YouTubeApiService>();
builder.Services.AddSingleton<MarkdownService>();
builder.Services.AddScoped<PageService>();
builder.Services.AddScoped<BlogService>();

//Add pages
builder.Services.AddBlazorBootstrap();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddMicrosoftIdentityConsentHandler();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
