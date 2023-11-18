using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System;

namespace OpmlRender.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public List<Feed> FeedsList { get; set; }
        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            FeedsList = new List<Feed>();
            _httpClientFactory = httpClientFactory;
        }

        public List<RssItem> RssItemList { get; set; } = new List<RssItem>();
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; private set; } = 20;
        public List<RssItem> FavoriteItemsList { get; set; } = new List<RssItem>();

        public async Task<IActionResult> OnGet(int pageNumber = 1)
        {
            var httpClient = _httpClientFactory.CreateClient();
            HttpResponseMessage opmlResponse = await httpClient.GetAsync("https://blue.feedland.org/opml?screenname=dave");

            if (opmlResponse.IsSuccessStatusCode)
            {
                string opmlContent = await opmlResponse.Content.ReadAsStringAsync();
                XmlDocument opmlDocument = new XmlDocument();
                opmlDocument.LoadXml(opmlContent);

                XmlNodeList outlineNodes = opmlDocument.GetElementsByTagName("outline");
                foreach (XmlNode outlineNode in outlineNodes)
                {
                    if (outlineNode.Attributes["xmlUrl"] != null)
                    {
                        Feed feed = new Feed();
                        feed.XmlUrl = outlineNode.Attributes["xmlUrl"].Value;
                        feed.HtmlUrl = outlineNode.Attributes["htmlUrl"].Value;
                        feed.Text = outlineNode.Attributes["text"].Value;

                        FeedsList.Add(feed);
                    }
                }

                List<RssItem> rssItems = new List<RssItem>();
                foreach (var feed in FeedsList)
                {
                    string xml = await httpClient.GetStringAsync(feed.XmlUrl);
                    XmlDocument rssDocument = new XmlDocument();
                    rssDocument.LoadXml(xml);
                    XmlNodeList itemNodes = rssDocument.GetElementsByTagName("item");

                    foreach (XmlElement itemNode in itemNodes)
                    {
                        RssItem item = new RssItem();
                        item.Title = itemNode.SelectSingleNode("title")?.InnerText;
                        item.Description = itemNode.SelectSingleNode("description")?.InnerText;
                        item.Link = itemNode.SelectSingleNode("link")?.InnerText;
                        item.PubDate = itemNode.SelectSingleNode("pubDate")?.InnerText;

                        rssItems.Add(item);
                    }
                }

                RssItemList = rssItems;

                TotalPages = (int)Math.Ceiling((double)RssItemList.Count / PageSize);
                PageNumber = pageNumber;

                RssItemList = RssItemList
                    .Skip((pageNumber - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();
            }
            else
            {
                return StatusCode((int)opmlResponse.StatusCode);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostStar(string link)
        {
            var favoriteItemsJson = Request.Cookies["FavoriteItems"];
            Dictionary<string, RssItem> favoriteItems = new Dictionary<string, RssItem>();
            if (!string.IsNullOrEmpty(favoriteItemsJson))
            {
                favoriteItems = JsonSerializer.Deserialize<Dictionary<string, RssItem>>(favoriteItemsJson);
            }

            if (!favoriteItems.ContainsKey(link))
            {
                RssItem item = new RssItem { Link = link, IsFavorite = true };
                favoriteItems.Add(link, item);
            }

            // serialize the favoriteItems dictionary and update the cookie
            Response.Cookies.Append("FavoriteItems", JsonSerializer.Serialize(favoriteItems));

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUnstar(string link)
        {
            var favoriteItemsJson = Request.Cookies["FavoriteItems"];
            Dictionary<string, RssItem> favoriteItems = new Dictionary<string, RssItem>();
            if (!string.IsNullOrEmpty(favoriteItemsJson))
            {
                favoriteItems = JsonSerializer.Deserialize<Dictionary<string, RssItem>>(favoriteItemsJson);
            }

            if (favoriteItems.ContainsKey(link))
            {
                favoriteItems.Remove(link);
            }

            // serialize the favoriteItems dictionary and update the cookie
            Response.Cookies.Append("FavoriteItems", JsonSerializer.Serialize(favoriteItems));

            return RedirectToPage();
        }
    }

    public class RssItem
    {
        public string Title { get; set; }
        public string PubDate { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public bool IsFavorite { get; set; }
    }

    public class Feed
    {
        public string XmlUrl { get; set; }
        public string HtmlUrl { get; set; }
        public string Text { get; set; }
    }
}