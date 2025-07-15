// Flight schedule functionality

// Extended flight data
const extendedFlightData = {
  flights: [
    {
      flightNumber: "TK1234",
      date: "2024-01-15",
      time: "14:30",
      duration: "8h 45m",
      distance: "8,000 km",
      source: {
        country: "Turkey",
        city: "Istanbul",
        airport: "Istanbul Airport",
        code: "IST",
      },
      destination: {
        country: "United States",
        city: "New York",
        airport: "John F. Kennedy International Airport",
        code: "JFK",
      },
      aircraftType: "Boeing 777-300ER",
      totalSeats: 396,
      status: "Active",
      sharedFlight: null,
    },
    {
      flightNumber: "TK5678",
      date: "2024-01-16",
      time: "10:15",
      duration: "4h 20m",
      distance: "2,500 km",
      source: {
        country: "Turkey",
        city: "Istanbul",
        airport: "Istanbul Airport",
        code: "IST",
      },
      destination: {
        country: "United Kingdom",
        city: "London",
        airport: "Heathrow Airport",
        code: "LHR",
      },
      aircraftType: "Airbus A320",
      totalSeats: 180,
      status: "Scheduled",
      sharedFlight: null,
    },
    {
      flightNumber: "TK9012",
      date: "2024-01-17",
      time: "16:45",
      duration: "3h 55m",
      distance: "2,200 km",
      source: {
        country: "Turkey",
        city: "Istanbul",
        airport: "Istanbul Airport",
        code: "IST",
      },
      destination: {
        country: "France",
        city: "Paris",
        airport: "Charles de Gaulle Airport",
        code: "CDG",
      },
      aircraftType: "Airbus A320",
      totalSeats: 180,
      status: "Scheduled",
      sharedFlight: null,
    },
    {
      flightNumber: "TK3456",
      date: "2024-01-14",
      time: "09:30",
      duration: "6h 15m",
      distance: "4,500 km",
      source: {
        country: "Turkey",
        city: "Istanbul",
        airport: "Istanbul Airport",
        code: "IST",
      },
      destination: {
        country: "Germany",
        city: "Frankfurt",
        airport: "Frankfurt Airport",
        code: "FRA",
      },
      aircraftType: "Boeing 737-800",
      totalSeats: 189,
      status: "Completed",
      sharedFlight: {
        partnerFlightNumber: "LH7890",
        partnerAirline: "Lufthansa",
      },
    },
    {
      flightNumber: "TK7890",
      date: "2024-01-18",
      time: "22:00",
      duration: "12h 30m",
      distance: "11,500 km",
      source: {
        country: "Turkey",
        city: "Istanbul",
        airport: "Istanbul Airport",
        code: "IST",
      },
      destination: {
        country: "Japan",
        city: "Tokyo",
        airport: "Narita International Airport",
        code: "NRT",
      },
      aircraftType: "Boeing 777-300ER",
      totalSeats: 396,
      status: "Scheduled",
      sharedFlight: null,
    },
    {
      flightNumber: "TK2468",
      date: "2024-01-13",
      time: "07:45",
      duration: "2h 45m",
      distance: "1,200 km",
      source: {
        country: "Turkey",
        city: "Istanbul",
        airport: "Istanbul Airport",
        code: "IST",
      },
      destination: {
        country: "Italy",
        city: "Rome",
        airport: "Leonardo da Vinci Airport",
        code: "FCO",
      },
      aircraftType: "Airbus A320",
      totalSeats: 180,
      status: "Cancelled",
      sharedFlight: null,
    },
  ],
}

// Function to show toast messages
function showToast(message) {
  console.log(message)
  // Implement toast message functionality here
}

// Initialize flight schedule page
function initializeFlightSchedule() {
  updateFlightStatistics()
  updateFlightsTable()
  setupFlightSearchAndFilter()
}

// Update flight statistics
function updateFlightStatistics() {
  const totalFlights = extendedFlightData.flights.length
  const activeFlights = extendedFlightData.flights.filter((f) => f.status === "Active").length
  const scheduledFlights = extendedFlightData.flights.filter((f) => f.status === "Scheduled").length
  const destinations = new Set(extendedFlightData.flights.map((f) => f.destination.code)).size

  document.getElementById("total-flights").textContent = totalFlights
  document.getElementById("active-flights").textContent = activeFlights
  document.getElementById("scheduled-flights").textContent = scheduledFlights
  document.getElementById("destinations").textContent = destinations
  document.getElementById("flights-count").textContent = totalFlights
}

// Update flights table
function updateFlightsTable(flights = extendedFlightData.flights) {
  const tbody = document.getElementById("flights-table-body")
  if (!tbody) return

  let html = ""
  flights.forEach((flight) => {
    const statusClass =
      flight.status === "Active"
        ? "badge-active"
        : flight.status === "Completed"
          ? "badge-completed"
          : flight.status === "Cancelled"
            ? "badge-scheduled"
            : "badge-scheduled"

    html += `
            <tr>
                <td><strong>${flight.flightNumber}</strong></td>
                <td>
                    <div class="route-info">
                        <span>${flight.source.code}</span>
                        <i class="fas fa-plane route-icon"></i>
                        <span>${flight.destination.code}</span>
                    </div>
                    <div class="route-cities">${flight.source.city} → ${flight.destination.city}</div>
                </td>
                <td>
                    <div>${flight.date}</div>
                    <div class="flight-time">${flight.time}</div>
                </td>
                <td>${flight.duration}</td>
                <td>${flight.aircraftType}</td>
                <td>${flight.distance}</td>
                <td><span class="badge ${statusClass}">${flight.status}</span></td>
                <td>
                    <button class="btn btn-sm" onclick="viewFlight('${flight.flightNumber}')">View</button>
                    <button class="btn btn-sm" onclick="editFlight('${flight.flightNumber}')">Edit</button>
                </td>
            </tr>
        `
  })

  tbody.innerHTML = html
}

// Setup search and filter functionality
function setupFlightSearchAndFilter() {
  const searchInput = document.getElementById("flight-search")
  const dateFilter = document.getElementById("date-filter")
  const statusFilter = document.getElementById("status-filter")

  if (searchInput) {
    searchInput.addEventListener("input", filterFlights)
  }

  if (dateFilter) {
    dateFilter.addEventListener("change", filterFlights)
  }

  if (statusFilter) {
    statusFilter.addEventListener("change", filterFlights)
  }
}

// Filter flights
function filterFlights() {
  const searchTerm = document.getElementById("flight-search").value.toLowerCase()
  const selectedDate = document.getElementById("date-filter").value
  const selectedStatus = document.getElementById("status-filter").value

  let filteredFlights = extendedFlightData.flights

  // Filter by search term
  if (searchTerm) {
    filteredFlights = filteredFlights.filter(
      (flight) =>
        flight.flightNumber.toLowerCase().includes(searchTerm) ||
        flight.source.city.toLowerCase().includes(searchTerm) ||
        flight.destination.city.toLowerCase().includes(searchTerm) ||
        flight.source.code.toLowerCase().includes(searchTerm) ||
        flight.destination.code.toLowerCase().includes(searchTerm),
    )
  }

  // Filter by date
  if (selectedDate) {
    filteredFlights = filteredFlights.filter((flight) => flight.date === selectedDate)
  }

  // Filter by status
  if (selectedStatus !== "all") {
    filteredFlights = filteredFlights.filter((flight) => flight.status.toLowerCase() === selectedStatus.toLowerCase())
  }

  updateFlightsTable(filteredFlights)
  document.getElementById("flights-count").textContent = filteredFlights.length
}

// View flight
function viewFlight(flightNumber) {
  showToast(`Viewing details for flight ${flightNumber}`)
  // Redirect to roster page with selected flight
  window.location.href = `roster.html?flight=${flightNumber}`
}

// Edit flight
function editFlight(flightNumber) {
  showToast(`Editing flight ${flightNumber}`)
}

// Initialize when page loads
document.addEventListener("DOMContentLoaded", () => {
  if (window.location.pathname.includes("flights.html")) {
    initializeFlightSchedule()
  }
})
