using HtmlAgilityPack;
using System.Collections.Generic;
using ScrapySharp.Extensions;
using System.Linq;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace CraigslistCrawler {
    internal class CraigslistCrawler {
        private readonly string _urlToCrawl;

        public string Title { get; }
        public DateTime PostedOn { get; }
        public double Price { get; }
        public string Link { get; }

        public CraigslistCrawler(string urlToCrawl) {
            _urlToCrawl = urlToCrawl;
        }

        public async Task<IEnumerable<Listing>> Crawl() {
            HtmlWeb web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(_urlToCrawl);           

            return ConvertToListings(doc);
        }

        private IEnumerable<Listing> ConvertToListings(HtmlDocument doc) {
            var listings = doc.DocumentNode.CssSelect("li.result-row");

            foreach (var listing in listings) {
                DateTime postedTime;
                decimal price;

                var title = listing.CssSelect("a.result-title.hdrlnk").FirstOrDefault().InnerText;
                var link = listing.CssSelect("a").FirstOrDefault().GetAttributeValue("href");
                Decimal.TryParse(listing.CssSelect("span.result-price").FirstOrDefault().InnerText, NumberStyles.Currency | NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out price);
                DateTime.TryParse(listing.CssSelect("time.result-date").FirstOrDefault().Attributes.Where(a => a.Name == "datetime").Select(a => a).FirstOrDefault().Value, out postedTime);

                yield return new Listing(title, postedTime, price, link);
            }
        }
    }

    internal class Listing {
        public Listing(string title, DateTime? postedOn, decimal price, string link) {
            Title = title;
            PostedOn = postedOn;
            Price = price;
            Link = link;
        }

        public string Title { get; private set; }
        public DateTime? PostedOn { get; private set; }
        public decimal? Price { get; private set; }
        public string Link { get; private set; }

        public override string ToString() {
            return $"{Title ?? "UnknownTitle"}, {PostedOn ?? DateTime.MinValue}, {Price ?? 0}, {Link ?? "No link available"}";
        }
    }
}
