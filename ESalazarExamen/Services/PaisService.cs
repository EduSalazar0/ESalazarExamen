using ESalazarExamen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ESalazarExamen.Services
{
    public class PaisService
    {
        private readonly HttpClient _httpClient;

        public PaisService()
        {
            _httpClient = new HttpClient();
        }
        public async Task<List<Pais>> GetPaisAsync()
        {
            var url = "https://restcountries.com/v3.1/name/?fields=name,region,maps";
            var responde = await _httpClient.GetStringAsync(url);
            return JsonSerializer.Deserialize<List<Pais>>(responde, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
