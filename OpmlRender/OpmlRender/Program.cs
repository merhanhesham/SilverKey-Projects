using OpmlRender.Pages;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpmlRender.Pages;
using System;
using System.Collections.Generic;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapPost("/toggle", async (HttpContext context) =>
{
    var antiforgery = context.RequestServices.GetRequiredService<IAntiforgery>();
    await antiforgery.ValidateRequestAsync(context);

    var link = context.Request.Form["link"];
    var title = context.Request.Form["title"];
    var pubDate = context.Request.Form["pubDate"];

    var favoriteFeedsJson = context.Request.Cookies["FavoriteFeeds"];

    var favoriteFeeds = !string.IsNullOrEmpty(favoriteFeedsJson)
        ? JsonSerializer.Deserialize<Dictionary<string, RssItem>>(favoriteFeedsJson)
        : new Dictionary<string, RssItem>();

    if (favoriteFeeds.ContainsKey(link))
    {
        favoriteFeeds.Remove(link);
    }
    else
    {
        var feed = new RssItem { Link = link, Title = title, IsFavorite = true, PubDate = pubDate };
        favoriteFeeds.Add(link, feed);
    }

    var options = new CookieOptions
    {
        Expires = DateTime.Now.AddDays(30),
        HttpOnly = true,
        SameSite = SameSiteMode.Strict,
        Secure = true,
    };

    context.Response.Cookies.Append("FavoriteFeeds", JsonSerializer.Serialize(favoriteFeeds), options);
});
app.MapRazorPages();

app.Run();