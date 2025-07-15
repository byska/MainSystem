using MainSystem.Infrastructure.External.Adapters.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MainSystem.Infrastructure.External.Adapters
{
    public sealed class FlightInfoHttpAdapter : IFlightInfoAdapter
    {
        private readonly HttpClient _http;
        private readonly JsonSerializerOptions _opt = new(JsonSerializerDefaults.Web);
        public FlightInfoHttpAdapter(HttpClient http) => _http = http;

        public async Task<IReadOnlyList<FlightDto>> ListAllAsync(CancellationToken ct = default)
        {
            return await _http.GetFromJsonAsync<IReadOnlyList<FlightDto>>("api/flight", _opt, ct)!;
        }
        public async Task<FlightDto> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var resp = await _http.GetAsync($"api/flight/by-id/{id}", ct);

            if (resp.StatusCode == HttpStatusCode.NotFound)
                return null;

            resp.EnsureSuccessStatusCode();

            FlightDto dto = await resp.Content.ReadFromJsonAsync<FlightDto>(_opt, ct);
            return dto;
        }

        public async Task<FlightDto> GetByFlightNoAsync(string flightNo, CancellationToken ct = default)
        {
            var resp = await _http.GetAsync($"api/flight/by-number/{flightNo}", ct);

            if (resp.StatusCode == HttpStatusCode.NotFound)
                return null;

            resp.EnsureSuccessStatusCode();

            FlightDto dto = await resp.Content.ReadFromJsonAsync<FlightDto>(_opt, ct);
            return dto;
        }
    }
}
