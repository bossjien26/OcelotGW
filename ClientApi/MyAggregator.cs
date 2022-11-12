using System.Net;
using Newtonsoft.Json;
using Ocelot.Middleware;
using Ocelot.Multiplexer;

namespace ClientApi
{
    public class MyAggregator : IDefinedAggregator
    {
        private readonly ILogger<MyAggregator> _logger;

        public MyAggregator(ILogger<MyAggregator> logger)
        {
            _logger = logger;
        }

        public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        {
            var responseBodies = responses.Select(response =>
            {
                _logger.LogDebug($"Status code {response.Response.StatusCode}.");

                using var responseReader = new StreamReader(response.Response.Body);
                return responseReader.ReadToEnd();
            }).ToList();
            Console.WriteLine(JsonConvert.SerializeObject(responseBodies));
            return new DownstreamResponse(
                new StringContent(JsonConvert.SerializeObject(responseBodies)),
                HttpStatusCode.OK,
                new List<Header>(),
                "OK");
        }
    }
}