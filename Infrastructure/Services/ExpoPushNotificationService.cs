using Application.Interfaces.Services;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class ExpoPushNotificationService : IPushNotificationService
    {
        private readonly HttpClient _httpClient;
        private readonly ExpoPushOptions _options;

        public ExpoPushNotificationService(HttpClient httpClient, IOptions<ExpoPushOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task SendAsync(IReadOnlyList<string> tokens, string title, string body, CancellationToken cancellationToken = default)
        {
            if (tokens.Count == 0)
            {
                return;
            }

            object[] payload = tokens
                .Select(token => new
                {
                    to = token,
                    title,
                    body,
                    sound = "default"
                })
                .Cast<object>()
                .ToArray();

            string json = JsonSerializer.Serialize(payload);
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "push/send");
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrWhiteSpace(_options.AccessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.AccessToken);
            }

            using HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
    }
}