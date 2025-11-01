using ClientBlazorAPI.Models;
using System.Net.Http.Json;

namespace ClientBlazorAPI.Services
{
    public class WSServiceNotation
    {
        private readonly HttpClient httpClient;

        public WSServiceNotation(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<NotationDTO>?> GetAllNotationsAsync()
        {
            try
            {
                // Appel à l'endpoint /api/Utilisateurs/notations
                return await httpClient.GetFromJsonAsync<List<NotationDTO>>("Utilisateurs/notations");
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}