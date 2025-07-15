// Crew management functionality

// Extended crew data
const extendedCrewData = {
  flightCrew: [
    {
      id: "FC001",
      name: "Captain John Smith",
      age: 45,
      gender: "Male",
      nationality: "Turkish",
      languages: ["Turkish", "English", "German"],
      seniority: "Senior",
      vehicleRestriction: "Boeing 777",
      allowedRange: 15000,
      position: "Captain",
      status: "Available",
    },
    {
      id: "FC002",
      name: "First Officer Sarah Johnson",
      age: 32,
      gender: "Female",
      nationality: "American",
      languages: ["English", "Spanish"],
      seniority: "Junior",
      vehicleRestriction: "Boeing 777",
      allowedRange: 12000,
      position: "First Officer",
      status: "Available",
    },
    {
      id: "FC003",
      name: "Captain Ahmed Yilmaz",
      age: 52,
      gender: "Male",
      nationality: "Turkish",
      languages: ["Turkish", "English", "Arabic"],
      seniority: "Senior",
      vehicleRestriction: "Airbus A320",
      allowedRange: 8000,
      position: "Captain",
      status: "On Flight",
    },
    {
      id: "FC004",
      name: "First Officer Emma Wilson",
      age: 29,
      gender: "Female",
      nationality: "British",
      languages: ["English", "French"],
      seniority: "Junior",
      vehicleRestriction: "Airbus A320",
      allowedRange: 6000,
      position: "First Officer",
      status: "Available",
    },
    {
      id: "FC005",
      name: "Trainee Pilot Mark Davis",
      age: 25,
      gender: "Male",
      nationality: "Canadian",
      languages: ["English"],
      seniority: "Trainee",
      vehicleRestriction: "Boeing 737",
      allowedRange: 4000,
      position: "Trainee Pilot",
      status: "Training",
    },
  ],
  cabinCrew: [
    {
      id: "CC001",
      name: "Maria Rodriguez",
      age: 35,
      gender: "Female",
      nationality: "Spanish",
      languages: ["Spanish", "English", "Turkish"],
      type: "Chief",
      vehicleRestrictions: ["Boeing 777", "Airbus A320"],
      position: "Chief Flight Attendant",
      status: "Available",
    },
    {
      id: "CC002",
      name: "Ahmed Hassan",
      age: 28,
      gender: "Male",
      nationality: "Turkish",
      languages: ["Turkish", "Arabic", "English"],
      type: "Regular",
      vehicleRestrictions: ["Boeing 777", "Airbus A320"],
      position: "Flight Attendant",
      status: "On Flight",
    },
    {
      id: "CC003",
      name: "Chef Pierre Dubois",
      age: 42,
      gender: "Male",
      nationality: "French",
      languages: ["French", "English"],
      type: "Chef",
      vehicleRestrictions: ["Boeing 777"],
      recipes: ["Coq au Vin", "Beef Bourguignon", "Ratatouille"],
      position: "Chef",
      status: "Available",
    },
    {
      id: "CC004",
      name: "Lisa Chen",
      age: 26,
      gender: "Female",
      nationality: "Chinese",
      languages: ["Chinese", "English"],
      type: "Regular",
      vehicleRestrictions: ["Boeing 777", "Airbus A320"],
      position: "Flight Attendant",
      status: "Available",
    },
    {
      id: "CC005",
      name: "Anna Kowalski",
      age: 31,
      gender: "Female",
      nationality: "Polish",
      languages: ["Polish", "English", "German"],
      type: "Chief",
      vehicleRestrictions: ["Airbus A320", "Boeing 737"],
      position: "Chief Flight Attendant",
      status: "Available",
    },
    {
      id: "CC006",
      name: "Chef Marco Rossi",
      age: 38,
      gender: "Male",
      nationality: "Italian",
      languages: ["Italian", "English"],
      type: "Chef",
      vehicleRestrictions: ["Boeing 777", "Airbus A320"],
      recipes: ["Risotto Milanese", "Osso Buco", "Tiramisu"],
      position: "Chef",
      status: "Available",
    },
    {
      id: "CC007",
      name: "Fatima Al-Zahra",
      age: 27,
      gender: "Female",
      nationality: "Moroccan",
      languages: ["Arabic", "French", "English"],
      type: "Regular",
      vehicleRestrictions: ["Boeing 777", "Airbus A320", "Boeing 737"],
      position: "Flight Attendant",
      status: "Available",
    },
    {
      id: "CC008",
      name: "James Thompson",
      age: 33,
      gender: "Male",
      nationality: "Australian",
      languages: ["English"],
      type: "Regular",
      vehicleRestrictions: ["Boeing 777", "Airbus A320"],
      position: "Flight Attendant",
      status: "Available",
    },
  ],
}

// Show crew tab
function showCrewTab(tabName) {
  // Hide all tab contents
  const tabContents = document.querySelectorAll(".tab-content")
  tabContents.forEach((content) => {
    content.classList.remove("active")
  })

  // Remove active class from all tab buttons
  const tabButtons = document.querySelectorAll(".tab-btn")
  tabButtons.forEach((button) => {
    button.classList.remove("active")
  })

  // Show selected tab content
  const selectedTab = document.getElementById(`${tabName}-crew-tab`)
  if (selectedTab) {
    selectedTab.classList.add("active")
  }

  // Add active class to clicked button
  event.target.classList.add("active")
}

// Initialize crew management page
function initializeCrewManagement() {
  updateCrewStatistics()
  updateFlightCrewManagementTable()
  updateCabinCrewManagementTable()
  setupSearchAndFilter()
}

// Update crew statistics
function updateCrewStatistics() {
  const flightCrewCount = extendedCrewData.flightCrew.length
  const cabinCrewCount = extendedCrewData.cabinCrew.length
  const chefCount = extendedCrewData.cabinCrew.filter((crew) => crew.type === "Chef").length

  document.getElementById("flight-crew-count").textContent = flightCrewCount
  document.getElementById("cabin-crew-count").textContent = cabinCrewCount
  document.getElementById("chef-count").textContent = chefCount
  document.getElementById("flight-crew-total").textContent = flightCrewCount
  document.getElementById("cabin-crew-total").textContent = cabinCrewCount
}

// Update flight crew management table
function updateFlightCrewManagementTable() {
  const tbody = document.getElementById("flight-crew-management-body")
  if (!tbody) return

  let html = ""
  extendedCrewData.flightCrew.forEach((crew) => {
    const seniorityClass =
      crew.seniority === "Senior" ? "badge-active" : crew.seniority === "Junior" ? "badge-completed" : "badge-scheduled"

    const statusClass =
      crew.status === "Available" ? "badge-active" : crew.status === "On Flight" ? "badge-completed" : "badge-scheduled"

    html += `
            <tr>
                <td><strong>${crew.name}</strong></td>
                <td>${crew.age}</td>
                <td>${crew.nationality}</td>
                <td><span class="badge ${seniorityClass}">${crew.seniority}</span></td>
                <td>${crew.vehicleRestriction}</td>
                <td>${crew.allowedRange.toLocaleString()} km</td>
                <td>${crew.languages.join(", ")}</td>
                <td><span class="badge ${statusClass}">${crew.status}</span></td>
                <td>
                    <button class="btn btn-sm" onclick="viewCrewMember('${crew.id}')">View</button>
                    <button class="btn btn-sm" onclick="editCrewMember('${crew.id}')">Edit</button>
                </td>
            </tr>
        `
  })

  tbody.innerHTML = html
}

// Update cabin crew management table
function updateCabinCrewManagementTable() {
  const tbody = document.getElementById("cabin-crew-management-body")
  if (!tbody) return

  let html = ""
  extendedCrewData.cabinCrew.forEach((crew) => {
    const typeClass =
      crew.type === "Chief" ? "badge-active" : crew.type === "Chef" ? "badge-completed" : "badge-scheduled"

    const statusClass =
      crew.status === "Available" ? "badge-active" : crew.status === "On Flight" ? "badge-completed" : "badge-scheduled"

    html += `
            <tr>
                <td><strong>${crew.name}</strong></td>
                <td>${crew.age}</td>
                <td>${crew.nationality}</td>
                <td><span class="badge ${typeClass}">${crew.type}</span></td>
                <td>${crew.vehicleRestrictions.join(", ")}</td>
                <td>${crew.languages.join(", ")}</td>
                <td>${crew.recipes ? crew.recipes.join(", ") : "N/A"}</td>
                <td><span class="badge ${statusClass}">${crew.status}</span></td>
                <td>
                    <button class="btn btn-sm" onclick="viewCrewMember('${crew.id}')">View</button>
                    <button class="btn btn-sm" onclick="editCrewMember('${crew.id}')">Edit</button>
                </td>
            </tr>
        `
  })

  tbody.innerHTML = html
}

// Setup search and filter functionality
function setupSearchAndFilter() {
  const searchInput = document.getElementById("crew-search")
  const filterSelect = document.getElementById("crew-filter")

  if (searchInput) {
    searchInput.addEventListener("input", filterCrewMembers)
  }

  if (filterSelect) {
    filterSelect.addEventListener("change", filterCrewMembers)
  }
}

// Filter crew members
function filterCrewMembers() {
  const searchTerm = document.getElementById("crew-search").value.toLowerCase()
  const filterType = document.getElementById("crew-filter").value

  // Filter flight crew
  const flightCrewRows = document.querySelectorAll("#flight-crew-management-body tr")
  flightCrewRows.forEach((row) => {
    const name = row.querySelector("td:first-child").textContent.toLowerCase()
    const shouldShow = (filterType === "all" || filterType === "flight") && name.includes(searchTerm)
    row.style.display = shouldShow ? "" : "none"
  })

  // Filter cabin crew
  const cabinCrewRows = document.querySelectorAll("#cabin-crew-management-body tr")
  cabinCrewRows.forEach((row) => {
    const name = row.querySelector("td:first-child").textContent.toLowerCase()
    const shouldShow = (filterType === "all" || filterType === "cabin") && name.includes(searchTerm)
    row.style.display = shouldShow ? "" : "none"
  })

  // Show/hide tabs based on filter
  const flightTab = document.querySelector(".tab-btn[onclick=\"showCrewTab('flight')\"]")
  const cabinTab = document.querySelector(".tab-btn[onclick=\"showCrewTab('cabin')\"]")

  if (filterType === "flight") {
    flightTab.click()
    cabinTab.style.display = "none"
  } else if (filterType === "cabin") {
    cabinTab.click()
    flightTab.style.display = "none"
  } else {
    flightTab.style.display = ""
    cabinTab.style.display = ""
  }
}

// View crew member
function viewCrewMember(crewId) {
  alert(`Viewing details for crew member ${crewId}`)
}

// Edit crew member
function editCrewMember(crewId) {
  alert(`Editing crew member ${crewId}`)
}

// Initialize when page loads
document.addEventListener("DOMContentLoaded", () => {
  if (window.location.pathname.includes("crew.html")) {
    initializeCrewManagement()
  }
})
