using System.Text.Json;
using RestSharp;
using System.Net;
using static System.Net.WebRequestMethods;
using RestSharp.Authenticators;

namespace GitHubApiTests
{
    public class ApiTests
    {
        private RestClient client;
        private const string baseUrl = "https://api.github.com";
        private const string partialUrl = "repos/Teodora217/Postman/issues";
        private const string username = "Teodora217";
        private const string password = "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";

        [SetUp]
        public void SetUp()
        {
            this.client = new RestClient(baseUrl);
            this.client.Authenticator = new HttpBasicAuthenticator(username, password);
        }

        [Test]
        [Timeout(1000)]
        public void Test_GetSingleIssue()
        {

            var request = new RestRequest($"{partialUrl}/2", Method.Get);
            var response = this.client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "HTTP Status Code property");

            var issue = JsonSerializer.Deserialize<Issue>(response.Content);

            Assert.That(issue.title, Is.EqualTo("Second Issue"));
            Assert.That(issue.number, Is.EqualTo(2));
        }

        [Test]
        public void Test_GetSingleIssueWithLabels()
        {

            var request = new RestRequest($"{partialUrl}/1", Method.Get);
            var response = this.client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "HTTP Status Code property");

            var issue = JsonSerializer.Deserialize<Issue>(response.Content);

            //for (var i = 0; i < issue.labels.Count; i++)
            //{
            //  Assert.That(issue.labels[i], Is.Not.Empty);
            //}
        }

        [Test]
        public void Test_GetAllIssues()
        {


            var request = new RestRequest(partialUrl, Method.Get);
            var response = this.client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "HTTP Status Code property");

            var issues = JsonSerializer.Deserialize<List<Issue>>(response.Content);

            foreach (var issue in issues)
            {
                Assert.That(issue.title, Is.Not.Empty);
                Assert.That(issue.number, Is.GreaterThan(0));
            }

        }
        [Test]
        public void Test_CreateNewIssue()
        {
            var issueBody = new
            {
                title = "Test issue from RestSharp" + DateTime.Now.Ticks,
                body = "some body for my issue",
                labels = new string[] { "bug", "critical", "release" }
            };

            var issue = CreateIssue(issueBody);

            //Assert


            Assert.That(issue.number, Is.GreaterThan(0));
            Assert.That(issue.title, Is.EqualTo(issueBody.title));
            Assert.That(issue.body, Is.EqualTo(issueBody.body));
        }

        [Test]
        public void Test_EditIssue()
        {
            var issueBody = new
            {
                title = "EDITED: Test issue from RestSharp" + DateTime.Now.Ticks,

            };

            var issue = EditIssue(issueBody);

            //Assert


            Assert.That(issue.number, Is.GreaterThan(0));
            Assert.That(issue.title, Is.EqualTo(issueBody.title));

        }


        private Issue CreateIssue(object body)
        {

            // Arange 
            var request = new RestRequest(partialUrl, Method.Post);
            request.AddBody(body);

            //Act
            var response = this.client.Execute(request);
            var issue = JsonSerializer.Deserialize<Issue>(response.Content);

            return issue;
        }
        private Issue EditIssue(object body)
        {

            // Arange 
            var request = new RestRequest($"{partialUrl}/1", Method.Patch);
            request.AddBody(body);

            //Act
            var response = this.client.Execute(request);
            var issue = JsonSerializer.Deserialize<Issue>(response.Content);

            return issue;
        }
        [TestCase("US", "90210", "United States")]
        [TestCase("BG", "1000", "Bulgaria")]
        [TestCase("DE", "01067", "Germany")]
        public void Test_Zippopotamus_DD(string countryCode, string zipCode, string expectedCountry)
        {
            var restClient = new RestClient("http://api.zippopotam.us");
            var request = new RestRequest(countryCode + "/" + zipCode, Method.Get);

            var response = restClient.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "HTTP Status Code property");
            var location = JsonSerializer.Deserialize<Location>(response.Content);

            Assert.That(location.Country, Is.EqualTo(expectedCountry));
        }
    }
}