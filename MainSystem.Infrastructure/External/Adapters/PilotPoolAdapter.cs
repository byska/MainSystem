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
    public class PilotPoolAdapter : IPilotPoolAdapter
    {
        private readonly HttpClient _http;
        private readonly JsonSerializerOptions _opt = new(JsonSerializerDefaults.Web);
        public PilotPoolAdapter(HttpClient http) => _http = http;

        public async Task<PilotDto> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var resp = await _http.GetAsync($"api/pilot/{id}", ct);

            if (resp.StatusCode == HttpStatusCode.NotFound)
                return null;

            resp.EnsureSuccessStatusCode();

            PilotDto dto = await resp.Content.ReadFromJsonAsync<PilotDto>(_opt, ct);
            return dto;
        }

        public async Task<IReadOnlyList<PilotDto>> ListAllAsync(CancellationToken ct = default)
        {
            return await _http.GetFromJsonAsync<IReadOnlyList<PilotDto>>("api/passengers", _opt, ct)!;
        }
    }
}
