using MainSystem.Domain.ValueObjects;
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
    public sealed class PassengerHttpAdapter : IPassengerAdapter
    {
        private readonly HttpClient _http;
        private readonly JsonSerializerOptions _opt = new(JsonSerializerDefaults.Web);

        public PassengerHttpAdapter(HttpClient http) => _http = http;

        public async Task<PassengerDto> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var resp = await _http.GetAsync($"api/passengers/{id}", ct);

            if (resp.StatusCode == HttpStatusCode.NotFound)
                return null;

            resp.EnsureSuccessStatusCode();

            PassengerDto dto = await resp.Content.ReadFromJsonAsync<PassengerDto>(_opt, ct);
            return dto;
        }

        public async Task<IReadOnlyList<PassengerDto>> ListAllAsync(CancellationToken ct = default)
        {
           return await _http.GetFromJsonAsync<IReadOnlyList<PassengerDto>>("api/passengers", _opt, ct)!;
        }

        public async Task<IReadOnlyList<PassengerDto>> ListByFlightAsync(string flightNumber, CancellationToken ct = default)
        {
            return await _http.GetFromJsonAsync<IReadOnlyList<PassengerDto>>(
            $"api/passengers/by-flight/{flightNumber}", _opt, ct)!;
        }
    }
}
