using System.Text.Json;
using AssemblyService.DTOs;
using System.Net.Http;
using System.Threading.Tasks;

namespace AssemblyService.Utilities.CommunicationClientUtility
{
    public class CommunicationClientUtility
    {
        private readonly HttpClient httpClient;

        public CommunicationClientUtility(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<AdminDTO> GetAssigneeData(int id)
        {
            // POST request instead of GET
            var response = await httpClient.PostAsJsonAsync($"http://localhost:5103/api/InternalAdmin/{id}", new { id });
            response.EnsureSuccessStatusCode();  // Ensures HTTP response is successful
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AdminDTO>(content);
        }

        public async Task<VehicleDTO> GetVehicleData(int id)
        {
            // POST request instead of GET
            var response = await httpClient.PostAsJsonAsync($"http://localhost:5025/api/InternalAccounts/vehicle/{id}", new { id });
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<VehicleDTO>(content);
        }

        public async Task<WorkerDTO> GetWorkerData(int id)
        {
            // POST request instead of GET
            var response = await httpClient.PostAsJsonAsync($"http://localhost:5025/api/InternalAccounts/worker/{id}", new { id });
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<WorkerDTO>(content);
        }
    }
}
