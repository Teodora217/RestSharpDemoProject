using RestSharp;
using RestSharp.Authenticators;
using System.Text.Json;

namespace RestSharpDemoProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RestClient client = new RestClient("https://api.github.com");

            client.Authenticator = new HttpBasicAuthenticator("Teodora217", "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");


            RestRequest request = new RestRequest("/repos/{user}/{repoName}/issues", Method.Post);
            request.AddHeader("Content-Type", "application/json");
           


            var issueBody = new
            {
                title = "Test isssue from RestSharp " + DateTime.Now.Ticks,
                body = "Some body for my issue",
                labels = new string[] { "bug", "critical", "release" }

            };
            
            
            request.AddBody(issueBody);
 
            request.AddUrlSegment("user", "Teodora217");
            request.AddUrlSegment("repoName", "Postman");
            //request.AddUrlSegment("id", "1");

            var response = client.Execute(request);

            Console.WriteLine("STATUS CODE: " + response.StatusCode);

            var issue = JsonSerializer.Deserialize<Issue>(response.Content!);
            //var labels = JsonSerializer.Deserialize<Labels>(response.Content);


            Console.WriteLine("Issue name: " + issue.title);
            Console.WriteLine("Issue number: " + issue.number);

        }
    }
}