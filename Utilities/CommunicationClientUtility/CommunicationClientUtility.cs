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
        private readonly string? adminServiceUrl;
        private readonly string? accountsServiceVehicleUrl;
        private readonly string? accountsServiceWorkerUrl;

        public CommunicationClientUtility(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;

            adminServiceUrl = configuration["ServiceUrls:AdminServiceGetAdmin"];
            accountsServiceVehicleUrl = configuration["ServiceUrls:AccountsServiceGetVehicle"];
            accountsServiceWorkerUrl = configuration["ServiceUrls:AccountsServiceGetWorker"];
        }

        public async Task<AdminDTO?> GetAssigneeData(int id)
        {
            var response = await httpClient.PostAsJsonAsync($"{adminServiceUrl}{id}", new { id });
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            if (content != null) 
            {
                return JsonSerializer.Deserialize<AdminDTO>(content);
            }
            return null;
        }

        public async Task<VehicleDTO?> GetVehicleData(int id)
        {
            var response = await httpClient.PostAsJsonAsync($"{accountsServiceVehicleUrl}{id}", new { id });
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            if (content != null)
            {
                return JsonSerializer.Deserialize<VehicleDTO>(content);
            }
            return null;
        }

        public async Task<WorkerDTO?> GetWorkerData(int id)
        {
            var response = await httpClient.PostAsJsonAsync($"{accountsServiceWorkerUrl}{id}", new { id });
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            if (content != null)
            {
                return JsonSerializer.Deserialize<WorkerDTO>(content);
            }
            return null;
        }
    }
}
