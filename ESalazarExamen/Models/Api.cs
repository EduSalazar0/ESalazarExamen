using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ESalazarExamen.Models
{
    public class Api
    {
        public string Nombre { get; set; }
        public string Region { get; set; }
        public string LinkMaps { get; set; }
        public string MiNombre { get; set; }



        private static readonly HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://restcountries.com/v3.1/")
        };

        public static async Task<Api> ObtenerPaisPorNombre(string nombre)
        {
            try
            {
                var response = await _httpClient.GetAsync($"name/{nombre}?fields=name,region,maps");
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var paises = JsonSerializer.Deserialize<List<Api>>(jsonResponse);

                return paises?.Count > 0 ? paises[0] : null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

