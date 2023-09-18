using AirlineSendAgent.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AirlineSendAgent.Client
{
    public class WebhookClient : IWebhookClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public WebhookClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task SendWebhookNotification(FlightDetailChangePayloadDto flightDetailChangePayloadDto)
        {
            var serializedPayload = JsonSerializer.Serialize(flightDetailChangePayloadDto);
            var httpClient = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, serializedPayload);
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(serializedPayload);
            request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            try
            {
                using(var response = await httpClient.SendAsync(request))
                {
                    await Console.Out.WriteLineAsync("Success");

                    response.EnsureSuccessStatusCode();
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("UnSuccessful");
            }
        }
    }
}
