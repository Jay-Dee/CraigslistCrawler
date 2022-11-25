using ScrapySharp.Network;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ScrapySharp.Extensions;

namespace CraigslistCrawler {
    internal class DutySheetCrawler {

        public void Crawl() {
            string url = @"https://secure.dutysheet.com/p1/?lo=1";

            var browserSimulator = new ScrapingBrowser();

            var dutySheetLoginPage  = browserSimulator.NavigateToPage(new Uri(url));
            var loginForm = dutySheetLoginPage.FindForm("form1");
            loginForm["ForceID"] = "1";
            loginForm["DSUsername"] = "s529195";
            loginForm["DSPassword"] = "jdSpecial02";
            loginForm.Method = HttpVerb.Post;
            var userPage = loginForm.Submit();

            var messagesPage = userPage.Browser.NavigateToPage(new Uri("https://secure.dutysheet.com/mymessages/"));
            var contentNode = messagesPage.Html.Descendants().Where(n => n.Id == "ja-wrapper").Select(n => n).FirstOrDefault()
            .Descendants().Where(n => n.Id == "ja-content").Select(n => n).FirstOrDefault()
            .Descendants().Where(n => n.Id == "contenthere").Select(n => n).FirstOrDefault();

            var a = userPage.Browser.AjaxDownloadString(new Uri("https://secure.dutysheet.com/mymessages/"));

            foreach (var n in contentNode.Descendants()) {
                if (!string.IsNullOrEmpty(n.Id.Trim())) {
                    Console.WriteLine(n.Id);
                }
            }

        }
    }
}
