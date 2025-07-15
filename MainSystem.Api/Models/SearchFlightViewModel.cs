using MainSystem.Domain.Enums;

namespace MainSystem.Api.Models
{
    public class SearchFlightViewModel
    {
        public string? FlightNumber { get; set; }
        public DateOnly? Date { get; set; }
        public string? SourceCode { get; set; }
        public string? DestCode { get; set; }
        public AircraftType? Aircraft { get; set; }
    }
}
