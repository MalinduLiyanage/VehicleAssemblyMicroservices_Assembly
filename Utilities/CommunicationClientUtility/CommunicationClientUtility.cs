using System.Text.Json;
using AssemblyService.DTOs;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace AssemblyService.Utilities.CommunicationClientUtility
{
    public class CommunicationClientUtility
    {
        private readonly HttpClient httpClient;
        private readonly string adminServiceUrl;
        private readonly string accountsServiceVehicleUrl;
        private readonly string accountsServiceWorkerUrl;

        public CommunicationClientUtility(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;

            // Ensuring no double slashes in URLs
            adminServiceUrl = configuration["ServiceUrls:AdminServiceGetAdmin"];
            accountsServiceVehicleUrl = configuration["ServiceUrls:AccountsServiceGetVehicle"];
            accountsServiceWorkerUrl = configuration["ServiceUrls:AccountsServiceGetWorker"];
        }

        public async Task<AdminDTO> GetAssigneeData(int id)
        {
            var response = await httpClient.PostAsJsonAsync($"{adminServiceUrl}{id}", new { id });
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AdminDTO>(content);
        }

        public async Task<VehicleDTO> GetVehicleData(int id)
        {
            var response = await httpClient.PostAsJsonAsync($"{accountsServiceVehicleUrl}{id}", new { id });
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<VehicleDTO>(content);
        }

        public async Task<WorkerDTO> GetWorkerData(int id)
        {
            var response = await httpClient.PostAsJsonAsync($"{accountsServiceWorkerUrl}{id}", new { id });
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<WorkerDTO>(content);
        }
    }
}
