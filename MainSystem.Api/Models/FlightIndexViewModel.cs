namespace MainSystem.Api.Models
{
    public class FlightIndexViewModel
    {
        public DashboardViewModel Dashboard { get; set; } = new();
        public SearchFlightViewModel Search { get; set; } = new();
    }
}
