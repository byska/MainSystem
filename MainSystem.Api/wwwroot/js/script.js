// Global variables
let currentRoster = null

// Navigation functionality
document.addEventListener("DOMContentLoaded", () => {
  const hamburger = document.getElementById("hamburger")
  const navMenu = document.getElementById("nav-menu")

  if (hamburger && navMenu) {
    hamburger.addEventListener("click", () => {
      navMenu.classList.toggle("active")
    })
  }
})

// Tab functionality
function showTab(tabName) {
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
  const selectedTab = document.getElementById(tabName)
  if (selectedTab) {
    selectedTab.classList.add("active")
  }

  // Add active class to clicked button
  event.target.classList.add("active")
}

// Toast notification
function showToast(message, type = "success") {
  const toast = document.getElementById("toast")
  const toastMessage = document.getElementById("toast-message")

  if (toast && toastMessage) {
    toastMessage.textContent = message
    toast.classList.add("show")

    // Change toast color based on type
    if (type === "error") {
      toast.style.background = "#ef4444"
    } else {
      toast.style.background = "#10b981"
    }

    setTimeout(() => {
      toast.classList.remove("show")
    }, 3000)
  }
}

// Mock data
const mockFlightData = {
  TK1234: {
    flightNumber: "TK1234",
    date: "2024-01-15 14:30",
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
  },
  TK5678: {
    flightNumber: "TK5678",
    date: "2024-01-16 10:15",
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
  },
}

const mockCrewData = {
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
    },
  ],
}

// Generate passengers
function generatePassengers(flightNumber, totalSeats) {
  const passengers = []
  const passengerCount = Math.floor(totalSeats * 0.85)

  const firstNames = ["John", "Sarah", "Michael", "Emma", "David", "Lisa", "Robert", "Anna", "James", "Maria"]
  const lastNames = [
    "Smith",
    "Johnson",
    "Williams",
    "Brown",
    "Jones",
    "Garcia",
    "Miller",
    "Davis",
    "Rodriguez",
    "Martinez",
  ]
  const nationalities = [
    "American",
    "British",
    "German",
    "French",
    "Turkish",
    "Spanish",
    "Italian",
    "Chinese",
    "Japanese",
    "Canadian",
  ]

  for (let i = 0; i < passengerCount; i++) {
    const firstName = firstNames[Math.floor(Math.random() * firstNames.length)]
    const lastName = lastNames[Math.floor(Math.random() * lastNames.length)]
    const age = Math.floor(Math.random() * 70) + 18
    const seatType = Math.random() < 0.2 ? "business" : "economy"

    const row = Math.floor(i / 6) + 1
    const seatLetter = String.fromCharCode(65 + (i % 6))
    const seatNumber = `${row}${seatLetter}`

    passengers.push({
      id: `P${String(i + 1).padStart(3, "0")}`,
      name: `${firstName} ${lastName}`,
      age,
      gender: Math.random() < 0.5 ? "Male" : "Female",
      nationality: nationalities[Math.floor(Math.random() * nationalities.length)],
      seatType,
      seatNumber,
    })
  }

  return passengers
}

// Generate roster
function generateRoster() {
  const flightSelect = document.getElementById("flight-select") || document.getElementById("flight-number")
  const flightNumber = flightSelect ? flightSelect.value : "TK1234"

  if (!flightNumber) {
    showToast("Please select a flight number", "error")
    return
  }

  // Show loading
  const generateBtn = document.querySelector('button[onclick="generateRoster()"]')
  const generateText = document.getElementById("generate-text")
  const generateLoading = document.getElementById("generate-loading")

  if (generateBtn) {
    generateBtn.disabled = true
    if (generateText) generateText.style.display = "none"
    if (generateLoading) generateLoading.style.display = "inline-block"
  }

  // Simulate API call
  setTimeout(() => {
    const flightInfo = mockFlightData[flightNumber]
    if (!flightInfo) {
      showToast("Flight not found", "error")
      return
    }

    const passengers = generatePassengers(flightNumber, flightInfo.totalSeats)

    currentRoster = {
      flightInfo,
      flightCrew: mockCrewData.flightCrew,
      cabinCrew: mockCrewData.cabinCrew,
      passengers,
    }

    // Update UI
    updateFlightInfo(currentRoster.flightInfo)
    // Placeholder for updateRosterViews function call
    console.log("updateRosterViews function needs to be defined.")

    // Show sections
    const flightInfoSection = document.getElementById("flight-info")
    const rosterViewsSection = document.getElementById("roster-views")
    const exportBtn = document.getElementById("export-btn")

    if (flightInfoSection) flightInfoSection.style.display = "block"
    if (rosterViewsSection) rosterViewsSection.style.display = "block"
    if (exportBtn) exportBtn.disabled = false

    // Reset button
    if (generateBtn) {
      generateBtn.disabled = false
      if (generateText) generateText.style.display = "inline"
      if (generateLoading) generateLoading.style.display = "none"
    }

    showToast(`Flight roster for ${flightNumber} has been generated successfully.`)
  }, 1500)
}

// Update flight info
function updateFlightInfo(flightInfo) {
  const elements = {
    "flight-number": flightInfo.flightNumber,
    "flight-route": `${flightInfo.source.code} → ${flightInfo.destination.code}`,
    "flight-cities": `${flightInfo.source.city} to ${flightInfo.destination.city}`,
    "flight-date": flightInfo.date,
    "flight-duration": flightInfo.duration,
    "aircraft-type": flightInfo.aircraftType,
    "aircraft-seats": `${flightInfo.totalSeats} seats`,
    "flight-distance": flightInfo.distance,
  }

  Object.entries(elements).forEach(([id, value]) => {
    const element = document.getElementById(id)
    if (element) element.textContent = value
  })
}

// Export roster
function exportRoster() {
  if (!currentRoster) {
    showToast("No roster data to export", "error")
    return
  }

  const dataStr = JSON.stringify(currentRoster, null, 2)
  const dataUri = "data:application/json;charset=utf-8," + encodeURIComponent(dataStr)

  const exportFileDefaultName = `roster_${currentRoster.flightInfo.flightNumber}_${new Date().toISOString().split("T")[0]}.json`

  const linkElement = document.createElement("a")
  linkElement.setAttribute("href", dataUri)
  linkElement.setAttribute("download", exportFileDefaultName)
  linkElement.click()

  showToast("Roster has been exported as JSON file.")
}

// Initialize page
document.addEventListener("DOMContentLoaded", () => {
  // Auto-generate roster for demo
  if (window.location.pathname.includes("roster.html")) {
    setTimeout(() => {
      generateRoster()
    }, 500)
  }
})

// Placeholder for updateRosterViews function definition
function updateRosterViews(roster) {
  // Function implementation goes here
  console.log("updateRosterViews function is not implemented yet.")
}
