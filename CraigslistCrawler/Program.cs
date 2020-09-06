using Newtonsoft.Json;
using System;

namespace CraigslistCrawler {
    class Program {
        static async System.Threading.Tasks.Task Main(string[] args) {
            var crawler = new CraigslistCrawler("https://london.craigslist.org/d/computers/search/sya");

            var listings = await crawler.Crawl();

            foreach(var listing in listings) {
                Console.WriteLine(listing);
            }

            listings.WriteToFile("craigslist_computer_listings");

            Console.ReadKey();
        }
    }
}
