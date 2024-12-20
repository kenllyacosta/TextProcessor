using Mecalux.Domain.Models;
using Mecalux.Wpf.Interfaces;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Mecalux.Wpf.Services
{
    public class DataService(HttpClient client) : IDataService
    {
        private readonly HttpClient Client = client;
        private readonly JsonSerializerOptions jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

        public async Task<string> GetOrderOptions()
        {
            string Result = string.Empty;
            try
            {
                HttpResponseMessage response = await Client.GetAsync("home/order-options");
                if (response.IsSuccessStatusCode)
                    Result = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return Result;
        }

        public async Task<TextStatistics> GetStatistics(string textToAnalyze)
        {
            TextStatistics? Result = default;
            try
            {
                HttpResponseMessage response = await Client.PostAsync("home/statistics", new StringContent(JsonSerializer.Serialize(textToAnalyze), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Result = JsonSerializer.Deserialize<TextStatistics>(responseContent, jsonSerializerOptions);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return Result!;
        }

        public async Task<string[]> OrderedText(OrderTextRequest orderTextRequest)
        {
            string[] Result = [];
            try
            {
                HttpResponseMessage response = await Client.PostAsync("home/ordered-text", new StringContent(JsonSerializer.Serialize(orderTextRequest), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Result = JsonSerializer.Deserialize<string[]>(responseContent)!;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return Result;
        }
    }
}