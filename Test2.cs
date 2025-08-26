using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using System.Net;
using System.Text.RegularExpressions;

namespace PartnerSolution
{
    public static class PartnerSolutionTest
    {
        // Render the html page using a headless browser (Selenium with ChromeDriver)
        private static string GetRenderedHtml(string url)
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("headless");
            using (var driver = new ChromeDriver(chromeOptions))
            {
                driver.Navigate().GoToUrl(url);
                return driver.PageSource;
            }
        }

        // [Default] Returns 5 items per page
        // [TODO] Iterate through all pages to get all partners
        public static List<Partner> GetPartners()
        {
            var partners = new List<Partner>();
            using (var httpClient = new HttpClient())
            {
                string htmlContent = GetRenderedHtml("https://www.opentext.com/partners/partner-directory");
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlContent);

                // Multiple nodes found with same xpath, filter by it's parent node class "card-layout"
                var nodes = htmlDocument.DocumentNode.SelectNodes("//div/div/h3/a")
                    .Where(n => n.ParentNode?.ParentNode?.ParentNode?.HasClass("card-layout") == true);

                if (nodes != null)
                {
                    foreach (var node in nodes)
                    {
                        var partnetName = WebUtility.HtmlDecode(node.InnerText?.Trim());
                        if (!string.IsNullOrWhiteSpace(partnetName))
                        {
                            partners.Add(new Partner { Name = partnetName });
                        }
                    }
                }
            }
            return partners;
        }

        // [Default] Returns 5 items per page
        // [TODO] Iterate through all pages to get all solutions
        public static List<Solution> GetSolutions()
        {
            var solutions = new List<Solution>();
            using (var httpClient = new HttpClient())
            {
                string htmlContent = GetRenderedHtml("https://www.opentext.com/products-and-solutions/partners-and-alliances/partner-solutions-catalog");
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlContent);

                var nodes = htmlDocument.DocumentNode.SelectNodes("//div")
                    .Where(n => n.ParentNode?.HasClass("card-body") == true);

                if (nodes != null)
                {
                    foreach (var node in nodes)
                    {
                        // Assumption: logo image alt text contains the partner name that matches with partner names received from GetPartners() method
                        // We can extract it using regex
                        var logoNode = node.SelectSingleNode(".//img");
                        string pattern = @"alt=""(.*?)\slogo""";
                        if (!string.IsNullOrWhiteSpace(logoNode?.OuterHtml))
                        {
                            Match match = Regex.Match(logoNode.OuterHtml, pattern, RegexOptions.IgnoreCase);
                            if (match.Success && !string.IsNullOrWhiteSpace(match.Groups[1]?.Value))
                            {
                                var partnerName = WebUtility.HtmlDecode(match.Groups[1].Value);
                                var solutionName = WebUtility.HtmlDecode(node.SelectSingleNode(".//h3/a")?.InnerText?.Trim());
                                if (!string.IsNullOrWhiteSpace(solutionName))
                                {
                                    solutions.Add(new Solution
                                    {
                                        PartnerName = partnerName,
                                        Name = solutionName,
                                    });
                                }
                            }
                        }
                    }
                }
            }
            return solutions;
        }
    }

    public class Partner
    {
        public required string Name { get; set; }
    }

    public class Solution
    {
        public required string Name { get; set; }
        public required string PartnerName { get; set; }
    }

    public class PartnerSolution
    {
        public string PartnerName { get; set; }
        public string SolutionName { get; set; }
    }
}
