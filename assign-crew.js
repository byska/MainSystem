// Enhanced Assign Crew functionality

let selectedFlight = null
let selectedPosition = null
const assignedCrew = {
  captain: null,
  "first-officer": null,
  "chief-attendant": null,
  "flight-attendant-1": null,
  "flight-attendant-2": null,
  chef: null,
}

// Mock flight data with detailed information
const flightDatabase = {
  TK1234: {
    flightNumber: "TK1234",
    date: "2024-01-15",
    time: "14:30",
    duration: "8h 45m",
    distance: "8,000 km",
    source: { code: "IST", city: "Istanbul", country: "Turkey" },
    destination: { code: "JFK", city: "New York", country: "USA" },
    aircraftType: "Boeing 777",
    totalSeats: 396,
    status: "Scheduled",
  },
  TK5678: {
    flightNumber: "TK5678",
    date: "2024-01-16",
    time: "10:15",
    duration: "4h 20m",
    distance: "2,500 km",
    source: { code: "IST", city: "Istanbul", country: "Turkey" },
    destination: { code: "LHR", city: "London", country: "UK" },
    aircraftType: "Airbus A320",
    totalSeats: 180,
    status: "Scheduled",
  },
  TK9012: {
    flightNumber: "TK9012",
    date: "2024-01-17",
    time: "16:45",
    duration: "3h 55m",
    distance: "2,200 km",
    source: { code: "IST", city: "Istanbul", country: "Turkey" },
    destination: { code: "CDG", city: "Paris", country: "France" },
    aircraftType: "Airbus A320",
    totalSeats: 180,
    status: "Scheduled",
  },
  TK7890: {
    flightNumber: "TK7890",
    date: "2024-01-18",
    time: "22:00",
    duration: "12h 30m",
    distance: "11,500 km",
    source: { code: "IST", city: "Istanbul", country: "Turkey" },
    destination: { code: "NRT", city: "Tokyo", country: "Japan" },
    aircraftType: "Boeing 777",
    totalSeats: 396,
    status: "Scheduled",
  },
}

// Enhanced crew database with more details
const crewDatabase = {
  flightCrew: [
    {
      id: "FC001",
      name: "Captain John Smith",
      age: 45,
      nationality: "Turkish",
      languages: ["Turkish", "English", "German"],
      seniority: "Senior",
      vehicleRestriction: "Boeing 777",
      allowedRange: 15000,
      position: "Captain",
      available: true,
      currentFlight: null,
      experience: "15 years",
      certifications: ["ATPL", "Type Rating Boeing 777"],
    },
    {
      id: "FC002",
      name: "First Officer Sarah Johnson",
      age: 32,
      nationality: "American",
      languages: ["English", "Spanish"],
      seniority: "Junior",
      vehicleRestriction: "Boeing 777",
      allowedRange: 12000,
      position: "First Officer",
      available: true,
      currentFlight: null,
      experience: "8 years",
      certifications: ["CPL", "Type Rating Boeing 777"],
    },
    {
      id: "FC003",
      name: "Captain Ahmed Yilmaz",
      age: 52,
      nationality: "Turkish",
      languages: ["Turkish", "English", "Arabic"],
      seniority: "Senior",
      vehicleRestriction: "Airbus A320",
      allowedRange: 8000,
      position: "Captain",
      available: false,
      currentFlight: "TK5678",
      experience: "20 years",
      certifications: ["ATPL", "Type Rating Airbus A320"],
    },
    {
      id: "FC004",
      name: "First Officer Emma Wilson",
      age: 29,
      nationality: "British",
      languages: ["English", "French"],
      seniority: "Junior",
      vehicleRestriction: "Airbus A320",
      allowedRange: 6000,
      position: "First Officer",
      available: true,
      currentFlight: null,
      experience: "5 years",
      certifications: ["CPL", "Type Rating Airbus A320"],
    },
    {
      id: "FC005",
      name: "Captain Michael Brown",
      age: 48,
      nationality: "Canadian",
      languages: ["English", "French"],
      seniority: "Senior",
      vehicleRestriction: "Boeing 777",
      allowedRange: 16000,
      position: "Captain",
      available: true,
      currentFlight: null,
      experience: "18 years",
      certifications: ["ATPL", "Type Rating Boeing 777", "Instructor"],
    },
  ],
  cabinCrew: [
    {
      id: "CC001",
      name: "Maria Rodriguez",
      age: 35,
      nationality: "Spanish",
      languages: ["Spanish", "English", "Turkish"],
      type: "Chief",
      vehicleRestrictions: ["Boeing 777", "Airbus A320"],
      position: "Chief Flight Attendant",
      available: true,
      currentFlight: null,
      experience: "12 years",
      specializations: ["Safety Training", "Customer Service"],
    },
    {
      id: "CC002",
      name: "Ahmed Hassan",
      age: 28,
      nationality: "Turkish",
      languages: ["Turkish", "Arabic", "English"],
      type: "Regular",
      vehicleRestrictions: ["Boeing 777", "Airbus A320"],
      position: "Flight Attendant",
      available: false,
      currentFlight: "TK1234",
      experience: "6 years",
      specializations: ["First Aid", "Multilingual Service"],
    },
    {
      id: "CC003",
      name: "Chef Pierre Dubois",
      age: 42,
      nationality: "French",
      languages: ["French", "English"],
      type: "Chef",
      vehicleRestrictions: ["Boeing 777"],
      recipes: ["Coq au Vin", "Beef Bourguignon", "Ratatouille"],
      position: "Chef",
      available: true,
      currentFlight: null,
      experience: "15 years",
      specializations: ["French Cuisine", "International Menu Planning"],
    },
    {
      id: "CC004",
      name: "Lisa Chen",
      age: 26,
      nationality: "Chinese",
      languages: ["Chinese", "English"],
      type: "Regular",
      vehicleRestrictions: ["Boeing 777", "Airbus A320"],
      position: "Flight Attendant",
      available: true,
      currentFlight: null,
      experience: "4 years",
      specializations: ["Asian Languages", "Cultural Sensitivity"],
    },
    {
      id: "CC005",
      name: "Anna Kowalski",
      age: 31,
      nationality: "Polish",
      languages: ["Polish", "English", "German"],
      type: "Chief",
      vehicleRestrictions: ["Airbus A320", "Boeing 737"],
      position: "Chief Flight Attendant",
      available: true,
      currentFlight: null,
      experience: "9 years",
      specializations: ["Team Leadership", "Emergency Procedures"],
    },
    {
      id: "CC006",
      name: "James Thompson",
      age: 33,
      nationality: "Australian",
      languages: ["English"],
      type: "Regular",
      vehicleRestrictions: ["Boeing 777", "Airbus A320"],
      position: "Flight Attendant",
      available: true,
      currentFlight: null,
      experience: "7 years",
      specializations: ["Safety Equipment", "Passenger Relations"],
    },
  ],
}

// Show toast functionality
function showToast(message, type = "success") {
  const toast = document.createElement("div")
  toast.className = `toast ${type}`
  toast.textContent = message
  document.body.appendChild(toast)

  setTimeout(() => {
    document.body.removeChild(toast)
  }, 3000)
}

// Select flight function
function selectFlight() {
  const flightSelect = document.getElementById("assign-flight-select")
  const flightNumber = flightSelect.value

  if (!flightNumber) {
    hideFlightDetails()
    document.getElementById("proceed-btn").disabled = true
    return
  }

  selectedFlight = flightDatabase[flightNumber]
  if (!selectedFlight) {
    showToast("Flight not found", "error")
    return
  }

  showFlightDetails(selectedFlight)
  document.getElementById("proceed-btn").disabled = false

  showToast(`Flight ${flightNumber} selected`)
}

// Show flight details
function showFlightDetails(flight) {
  const detailsDiv = document.getElementById("selected-flight-details")

  document.getElementById("flight-route").textContent = `${flight.source.code} → ${flight.destination.code}`
  document.getElementById("flight-cities").textContent = `${flight.source.city} to ${flight.destination.city}`
  document.getElementById("flight-datetime").textContent = `${flight.date} ${flight.time}`
  document.getElementById("flight-duration").textContent = flight.duration
  document.getElementById("flight-aircraft").textContent = flight.aircraftType
  document.getElementById("flight-seats").textContent = `${flight.totalSeats} seats`

  detailsDiv.style.display = "block"
}

// Hide flight details
function hideFlightDetails() {
  document.getElementById("selected-flight-details").style.display = "none"
}

// Proceed to crew selection
function proceedToCrewSelection() {
  if (!selectedFlight) {
    showToast("Please select a flight first", "error")
    return
  }

  // Update step indicators
  document.getElementById("step-2-number").classList.add("active")

  // Show crew assignment step
  document.getElementById("crew-assignment-step").style.display = "block"

  // Load compatible crew
  loadCompatibleCrew()

  // Update aircraft type in the crew section
  document.getElementById("selected-aircraft-type").textContent = selectedFlight.aircraftType

  // Scroll to crew assignment section
  document.getElementById("crew-assignment-step").scrollIntoView({ behavior: "smooth" })

  showToast("Ready to assign crew members")
}

// Load compatible crew for selected flight
function loadCompatibleCrew() {
  const crewList = document.getElementById("compatible-crew-list")
  const aircraftType = selectedFlight.aircraftType

  let html = ""

  // Filter and display flight crew
  const compatibleFlightCrew = crewDatabase.flightCrew.filter((crew) =>
    crew.vehicleRestriction.includes(aircraftType.split(" ")[0]),
  )

  compatibleFlightCrew.forEach((crew) => {
    const availabilityClass = crew.available ? "available" : "unavailable"
    const availabilityIcon = crew.available ? "check-circle" : "times-circle"
    const availabilityText = crew.available ? "Available" : `On ${crew.currentFlight}`

    html += `
      <div class="crew-card ${availabilityClass}" data-crew-id="${crew.id}" data-crew-type="flight" onclick="selectCrewForAssignment('${crew.id}')">
        <div class="crew-card-header">
          <span class="crew-name">${crew.name}</span>
          <span class="crew-type-badge flight">${crew.seniority}</span>
        </div>
        <div class="crew-details">
          <div class="crew-detail">
            <i class="fas fa-plane"></i>
            <span>${crew.vehicleRestriction}</span>
          </div>
          <div class="crew-detail">
            <i class="fas fa-globe"></i>
            <span>${crew.nationality}</span>
          </div>
          <div class="crew-detail">
            <i class="fas fa-language"></i>
            <span>${crew.languages.slice(0, 2).join(", ")}</span>
          </div>
          <div class="crew-detail">
            <i class="fas fa-clock"></i>
            <span>${crew.experience}</span>
          </div>
        </div>
        <div class="availability-status ${availabilityClass}">
          <i class="fas fa-${availabilityIcon}"></i>
          <span>${availabilityText}</span>
        </div>
      </div>
    `
  })

  // Filter and display cabin crew
  const compatibleCabinCrew = crewDatabase.cabinCrew.filter((crew) =>
    crew.vehicleRestrictions.some((restriction) => restriction.includes(aircraftType.split(" ")[0])),
  )

  compatibleCabinCrew.forEach((crew) => {
    const availabilityClass = crew.available ? "available" : "unavailable"
    const availabilityIcon = crew.available ? "check-circle" : "times-circle"
    const availabilityText = crew.available ? "Available" : `On ${crew.currentFlight}`
    const badgeClass = crew.type === "Chef" ? "chef" : "cabin"

    html += `
      <div class="crew-card ${availabilityClass}" data-crew-id="${crew.id}" data-crew-type="cabin" onclick="selectCrewForAssignment('${crew.id}')">
        <div class="crew-card-header">
          <span class="crew-name">${crew.name}</span>
          <span class="crew-type-badge ${badgeClass}">${crew.type}</span>
        </div>
        <div class="crew-details">
          <div class="crew-detail">
            <i class="fas fa-plane"></i>
            <span>${crew.vehicleRestrictions.join(", ")}</span>
          </div>
          <div class="crew-detail">
            <i class="fas fa-globe"></i>
            <span>${crew.nationality}</span>
          </div>
          <div class="crew-detail">
            <i class="fas fa-language"></i>
            <span>${crew.languages.slice(0, 2).join(", ")}</span>
          </div>
          <div class="crew-detail">
            <i class="fas fa-clock"></i>
            <span>${crew.experience}</span>
          </div>
        </div>
        <div class="availability-status ${availabilityClass}">
          <i class="fas fa-${availabilityIcon}"></i>
          <span>${availabilityText}</span>
        </div>
      </div>
    `
  })

  if (html === "") {
    html =
      '<div class="loading-crew"><i class="fas fa-exclamation-triangle"></i>No compatible crew found for this aircraft type</div>'
  }

  crewList.innerHTML = html
}

// Filter compatible crew
function filterCompatibleCrew() {
  const typeFilter = document.getElementById("crew-type-filter").value
  const searchFilter = document.getElementById("crew-search").value.toLowerCase()

  const crewCards = document.querySelectorAll(".crew-card")

  crewCards.forEach((card) => {
    const crewType = card.dataset.crewType
    const crewName = card.querySelector(".crew-name").textContent.toLowerCase()

    const matchesType = typeFilter === "all" || crewType === typeFilter
    const matchesSearch = crewName.includes(searchFilter)

    card.style.display = matchesType && matchesSearch ? "block" : "none"
  })
}

// Select crew for assignment (when clicking on crew card)
function selectCrewForAssignment(crewId) {
  const crew = findCrewById(crewId)
  if (!crew || !crew.available) {
    showToast("This crew member is not available", "error")
    return
  }

  // Show position selection modal
  showCrewSelectionModal(crew)
}

// Select position (when clicking on position card)
function selectPosition(position) {
  selectedPosition = position

  // Get compatible crew for this position
  const compatibleCrew = getCompatibleCrewForPosition(position)

  if (compatibleCrew.length === 0) {
    showToast("No compatible crew available for this position", "error")
    return
  }

  // Show crew selection modal
  showPositionCrewModal(position, compatibleCrew)
}

// Get compatible crew for specific position
function getCompatibleCrewForPosition(position) {
  const aircraftType = selectedFlight.aircraftType
  let compatibleCrew = []

  if (position === "captain") {
    compatibleCrew = crewDatabase.flightCrew.filter(
      (crew) =>
        crew.available && crew.seniority === "Senior" && crew.vehicleRestriction.includes(aircraftType.split(" ")[0]),
    )
  } else if (position === "first-officer") {
    compatibleCrew = crewDatabase.flightCrew.filter(
      (crew) =>
        crew.available &&
        (crew.seniority === "Junior" || crew.seniority === "Senior") &&
        crew.vehicleRestriction.includes(aircraftType.split(" ")[0]),
    )
  } else if (position === "chief-attendant") {
    compatibleCrew = crewDatabase.cabinCrew.filter(
      (crew) =>
        crew.available &&
        crew.type === "Chief" &&
        crew.vehicleRestrictions.some((restriction) => restriction.includes(aircraftType.split(" ")[0])),
    )
  } else if (position.includes("flight-attendant")) {
    compatibleCrew = crewDatabase.cabinCrew.filter(
      (crew) =>
        crew.available &&
        (crew.type === "Regular" || crew.type === "Chief") &&
        crew.vehicleRestrictions.some((restriction) => restriction.includes(aircraftType.split(" ")[0])),
    )
  } else if (position === "chef") {
    compatibleCrew = crewDatabase.cabinCrew.filter(
      (crew) =>
        crew.available &&
        crew.type === "Chef" &&
        crew.vehicleRestrictions.some((restriction) => restriction.includes(aircraftType.split(" ")[0])),
    )
  }

  return compatibleCrew
}

// Show position crew selection modal
function showPositionCrewModal(position, compatibleCrew) {
  const modal = document.getElementById("crew-selection-modal")
  const modalTitle = document.getElementById("modal-position-title")
  const modalCrewList = document.getElementById("modal-crew-list")

  const positionTitles = {
    captain: "Captain",
    "first-officer": "First Officer",
    "chief-attendant": "Chief Flight Attendant",
    "flight-attendant-1": "Flight Attendant",
    "flight-attendant-2": "Flight Attendant",
    chef: "Chef",
  }

  modalTitle.textContent = positionTitles[position]

  let html = ""
  compatibleCrew.forEach((crew) => {
    const badgeClass = crew.vehicleRestriction ? "flight" : crew.type === "Chef" ? "chef" : "cabin"
    const crewType = crew.seniority || crew.type

    html += `
      <div class="modal-crew-card" onclick="assignCrewToPosition('${crew.id}', '${position}')">
        <div class="crew-card-header">
          <span class="crew-name">${crew.name}</span>
          <span class="crew-type-badge ${badgeClass}">${crewType}</span>
        </div>
        <div class="crew-details">
          <div class="crew-detail">
            <i class="fas fa-globe"></i>
            <span>${crew.nationality}</span>
          </div>
          <div class="crew-detail">
            <i class="fas fa-language"></i>
            <span>${crew.languages.join(", ")}</span>
          </div>
          <div class="crew-detail">
            <i class="fas fa-clock"></i>
            <span>${crew.experience}</span>
          </div>
          ${
            crew.specializations
              ? `
            <div class="crew-detail">
              <i class="fas fa-star"></i>
              <span>${crew.specializations.join(", ")}</span>
            </div>
          `
              : ""
          }
        </div>
      </div>
    `
  })

  modalCrewList.innerHTML = html
  modal.classList.add("show")
}

// Assign crew to position
function assignCrewToPosition(crewId, position) {
  const crew = findCrewById(crewId)
  if (!crew) return

  // Remove from any previous assignment
  removeCrewFromAllPositions(crewId)

  // Assign to new position
  assignedCrew[position] = crew

  // Update position card UI
  updatePositionCard(position, crew)

  // Update progress
  updateProgress()

  // Close modal
  closeCrewSelectionModal()


  // Update crew card selection
  updateCrewCardSelection()

  showToast(`${crew.name} assigned as ${position.replace("-", " ")}`)
}

// Update position card UI
function updatePositionCard(position, crew) {
  const positionContent = document.getElementById(`${position}-assignment`)
  const positionCard = positionContent.closest(".position-card")

  if (crew) {
    positionContent.innerHTML = `
      <div class="assigned-crew-info">
        <div class="assigned-crew-name">${crew.name}</div>
        <div class="assigned-crew-details">
          ${crew.nationality} • ${crew.languages[0]} • ${crew.experience}
        </div>
        <button class="remove-assignment-btn" onclick="removeCrewFromPosition('${position}')">
          <i class="fas fa-times"></i> Remove
        </button>
      </div>
    `
    positionCard.classList.add("assigned", "assign-animation")
    setTimeout(() => positionCard.classList.remove("assign-animation"), 400)
  } else {
    const positionLabels = {
      captain: "Click to assign",
      "first-officer": "Click to assign",
      "chief-attendant": "Click to assign",
      "flight-attendant-1": "Click to assign",
      "flight-attendant-2": "Click to assign",
      chef: "Click to assign",
    }

    positionContent.innerHTML = `
      <div class="empty-position">
        <i class="fas fa-plus"></i>
        <span>${positionLabels[position]}</span>
      </div>
    `
    positionCard.classList.remove("assigned")
  }
}

// Remove crew from position
function removeCrewFromPosition(position) {
  const crew = assignedCrew[position]
  if (!crew) return

  assignedCrew[position] = null
  updatePositionCard(position, null)
  updateProgress()
  updateCrewCardSelection()

  showToast(`${crew.name} removed from ${position.replace("-", " ")}`)
}

// Remove crew from all positions
function removeCrewFromAllPositions(crewId) {
  Object.keys(assignedCrew).forEach((position) => {
    if (assignedCrew[position] && assignedCrew[position].id === crewId) {
      assignedCrew[position] = null
      updatePositionCard(position, null)
    }
  })
}

// Update crew card selection visual feedback
function updateCrewCardSelection() {
  const crewCards = document.querySelectorAll(".crew-card")
  const assignedCrewIds = Object.values(assignedCrew)
    .filter((crew) => crew)
    .map((crew) => crew.id)

  crewCards.forEach((card) => {
    const crewId = card.dataset.crewId
    if (assignedCrewIds.includes(crewId)) {
      card.classList.add("selected")
    } else {
      card.classList.remove("selected")
    }
  })
}

// Update progress bars
function updateProgress() {
  const flightCrewAssigned = [assignedCrew.captain, assignedCrew["first-officer"]].filter((c) => c).length
  const cabinCrewAssigned = [
    assignedCrew["chief-attendant"],
    assignedCrew["flight-attendant-1"],
    assignedCrew["flight-attendant-2"],
  ].filter((c) => c).length

  // Update flight crew progress
  const flightProgress = (flightCrewAssigned / 2) * 100
  document.getElementById("flight-crew-progress").style.width = `${flightProgress}%`
  document.getElementById("flight-crew-assigned").textContent = flightCrewAssigned

  // Update cabin crew progress
  const cabinProgress = (cabinCrewAssigned / 3) * 100
  document.getElementById("cabin-crew-progress").style.width = `${cabinProgress}%`
  document.getElementById("cabin-crew-assigned").textContent = cabinCrewAssigned

  // Enable/disable save button
  const saveBtn = document.getElementById("save-assignments-btn")
  const canSave = flightCrewAssigned >= 2 && cabinCrewAssigned >= 1
  saveBtn.disabled = !canSave
}

// Find crew by ID
function findCrewById(crewId) {
  let crew = crewDatabase.flightCrew.find((c) => c.id === crewId)
  if (!crew) {
    crew = crewDatabase.cabinCrew.find((c) => c.id === crewId)
  }
  return crew
}

// Add new crew member
function addNewCrewMember() {
  document.getElementById("add-crew-modal").classList.add("show")
}

// Update crew type fields
function updateCrewTypeFields() {
  const crewType = document.getElementById("new-crew-type").value
  const flightFields = document.getElementById("flight-crew-fields")
  const cabinFields = document.getElementById("cabin-crew-fields")
  const chefFields = document.getElementById("chef-fields")

  flightFields.style.display = crewType === "flight" ? "block" : "none"
  cabinFields.style.display = crewType === "cabin" ? "block" : "none"

  // Show/hide chef fields based on cabin crew type
  const cabinType = document.getElementById("new-crew-cabin-type")
  if (cabinType) {
    cabinType.addEventListener("change", function () {
      chefFields.style.display = this.value === "Chef" ? "block" : "none"
    })
  }
}

// Save new crew member
function saveNewCrewMember() {
  const form = document.getElementById("add-crew-form")
  const formData = new FormData(form)

  // Basic validation
  const name = document.getElementById("new-crew-name").value

  // Load flight details when flight is selected
  function loadFlightDetails() {
    const flightSelect = document.getElementById("assign-flight-select")
    const flightNumber = flightSelect.value

    if (!flightNumber) {
      hideAssignmentSections()
      return
    }

    // Get flight data (using mock data from previous files)
    const flightData = flightDatabase[flightNumber]
    if (!flightData) return

    selectedFlight = flightData

    // Update flight details
    document.getElementById("flight-date").value = flightData.date.split(" ")[0]
    document.getElementById("aircraft-info").value = flightData.aircraftType
    document.getElementById("flight-duration").value = flightData.duration

    // Show assignment sections
    showAssignmentSections()

    // Load available crew
    loadAvailableCrew()

    // Update progress
    updateProgress()

    showToast(`Flight ${flightNumber} selected`)
  }

  // Show assignment sections
  function showAssignmentSections() {
    document.getElementById("assignment-progress").style.display = "block"
    document.getElementById("crew-assignment").style.display = "grid"
    document.getElementById("assignment-summary").style.display = "block"
  }

  // Hide assignment sections
  function hideAssignmentSections() {
    document.getElementById("assignment-progress").style.display = "none"
    document.getElementById("crew-assignment").style.display = "none"
    document.getElementById("assignment-summary").style.display = "none"
  }

  // Load available crew
  function loadAvailableCrew() {
    const crewList = document.getElementById("available-crew-list")
    let html = ""

    // Flight crew
    crewDatabase.flightCrew.forEach((crew) => {
      const isCompatible = checkCrewCompatibility(crew)
      const availabilityClass = crew.available && isCompatible ? "" : "unavailable"

      html += `
        <div class="crew-item ${availabilityClass}"
             draggable="${crew.available && isCompatible}"
             data-crew-id="${crew.id}"
             data-crew-type="flight"
             ondragstart="handleDragStart(event)">
            <div class="crew-item-header">
                <span class="crew-name">${crew.name}</span>
                <span class="crew-type flight">${crew.seniority}</span>
            </div>
            <div class="crew-details">
                <div class="crew-detail">
                    <i class="fas fa-plane"></i>
                    <span>${crew.vehicleRestriction}</span>
                </div>
                <div class="crew-detail">
                    <i class="fas fa-globe"></i>
                    <span>${crew.nationality}</span>
                </div>
                <div class="crew-detail">
                    <i class="fas fa-language"></i>
                    <span>${crew.languages.slice(0, 2).join(", ")}</span>
                </div>
                ${!crew.available ? `<div class="crew-detail" style="color: #ef4444;"><i class="fas fa-plane"></i><span>On ${crew.currentFlight}</span></div>` : ""}
            </div>
        </div>
      `
    })

    // Cabin crew
    crewDatabase.cabinCrew.forEach((crew) => {
      const isCompatible = checkCrewCompatibility(crew)
      const availabilityClass = crew.available && isCompatible ? "" : "unavailable"

      html += `
        <div class="crew-item ${availabilityClass}"
             draggable="${crew.available && isCompatible}"
             data-crew-id="${crew.id}"
             data-crew-type="cabin"
             ondragstart="handleDragStart(event)">
            <div class="crew-item-header">
                <span class="crew-name">${crew.name}</span>
                <span class="crew-type ${crew.type === "Chef" ? "chef" : "cabin"}">${crew.type}</span>
            </div>
            <div class="crew-details">
                <div class="crew-detail">
                    <i class="fas fa-plane"></i>
                    <span>${crew.vehicleRestrictions.join(", ")}</span>
                </div>
                <div class="crew-detail">
                    <i class="fas fa-globe"></i>
                    <span>${crew.nationality}</span>
                </div>
                <div class="crew-detail">
                    <i class="fas fa-language"></i>
                    <span>${crew.languages.slice(0, 2).join(", ")}</span>
                </div>
                ${!crew.available ? `<div class="crew-detail" style="color: #ef4444;"><i class="fas fa-plane"></i><span>On ${crew.currentFlight}</span></div>` : ""}
            </div>
        </div>
      `
    })

    crewList.innerHTML = html

    // Setup drag and drop
    setupDragAndDrop()
  }

  // Check crew compatibility with selected flight
  function checkCrewCompatibility(crew) {
    if (!selectedFlight) return false

    if (crew.vehicleRestriction) {
      // Flight crew - check aircraft type
      return crew.vehicleRestriction.includes(selectedFlight.aircraftType.split(" ")[0])
    } else if (crew.vehicleRestrictions) {
      // Cabin crew - check aircraft type
      return crew.vehicleRestrictions.some((restriction) =>
        restriction.includes(selectedFlight.aircraftType.split(" ")[0]),
      )
    }

    return true
  }

  // Filter available crew
  function filterAvailableCrew() {
    const typeFilter = document.getElementById("crew-type-filter").value
    const searchFilter = document.getElementById("crew-search-filter").value.toLowerCase()

    const crewItems = document.querySelectorAll(".crew-item")

    crewItems.forEach((item) => {
      const crewType = item.dataset.crewType
      const crewName = item.querySelector(".crew-name").textContent.toLowerCase()

      const matchesType = typeFilter === "all" || crewType === typeFilter
      const matchesSearch = crewName.includes(searchFilter)

      item.style.display = matchesType && matchesSearch ? "block" : "none"
    })
  }

  // Setup drag and drop
  function setupDragAndDrop() {
    const positionSlots = document.querySelectorAll(".position-slot")

    positionSlots.forEach((slot) => {
      slot.addEventListener("dragover", handleDragOver)
      slot.addEventListener("drop", handleDrop)
      slot.addEventListener("dragleave", handleDragLeave)
    })
  }

  // Drag and drop handlers
  function handleDragStart(event) {
    event.dataTransfer.setData("text/plain", event.target.dataset.crewId)
    event.target.classList.add("dragging")
  }

  function handleDragOver(event) {
    event.preventDefault()
    event.currentTarget.classList.add("drag-over")
  }

  function handleDragLeave(event) {
    event.currentTarget.classList.remove("drag-over")
  }

  function handleDrop(event) {
    event.preventDefault()
    event.currentTarget.classList.remove("drag-over")

    const crewId = event.dataTransfer.getData("text/plain")
    const position = event.currentTarget.dataset.position

    assignCrewToPosition(crewId, position)

    // Remove dragging class
    document.querySelector(`[data-crew-id="${crewId}"]`).classList.remove("dragging")
  }

  // Assign crew to position
  function assignCrewToPosition(crewId, position) {
    // Find crew member
    const crew = findCrewById(crewId)
    if (!crew) return

    // Check if position is valid for crew type
    if (!isValidPosition(crew, position)) {
      showToast("Invalid position for this crew member", "error")
      return
    }

    // Remove from previous position if assigned
    removeCrewFromAllPositions(crewId)

    // Assign to new position
    assignedCrew[position] = crew

    // Update UI
    updatePositionSlot(position, crew)
    updateProgress()
    updateSummary()

    // Hide crew from available list
    const crewItem = document.querySelector(`[data-crew-id="${crewId}"]`)
    if (crewItem) {
      crewItem.style.display = "none"
    }

    showToast(`${crew.name} assigned to ${position.replace("-", " ")}`)
  }

  // Find crew by ID
  function findCrewById(crewId) {
    let crew = crewDatabase.flightCrew.find((c) => c.id === crewId)
    if (!crew) {
      crew = crewDatabase.cabinCrew.find((c) => c.id === crewId)
    }
    return crew
  }

  // Check if position is valid for crew
  function isValidPosition(crew, position) {
    const flightPositions = ["captain", "first-officer"]
    const cabinPositions = ["chief-attendant", "flight-attendant-1", "flight-attendant-2", "chef"]

    if (crew.vehicleRestriction) {
      // Flight crew
      if (position === "captain" && crew.seniority !== "Senior") return false
      return flightPositions.includes(position)
    } else {
      // Cabin crew
      if (position === "chief-attendant" && crew.type !== "Chief") return false
      if (position === "chef" && crew.type !== "Chef") return false
      return cabinPositions.includes(position)
    }
  }

  // Remove crew from all positions
  function removeCrewFromAllPositions(crewId) {
    Object.keys(assignedCrew).forEach((position) => {
      if (assignedCrew[position] && assignedCrew[position].id === crewId) {
        assignedCrew[position] = null
        updatePositionSlot(position, null)
      }
    })
  }

  // Update position slot UI
  function updatePositionSlot(position, crew) {
    const slot = document.getElementById(`${position}-slot`)
    if (!slot) return

    if (crew) {
      slot.innerHTML = `
        <div class="assigned-crew-item">
            <button class="remove-btn" onclick="removeCrewFromPosition('${position}')">×</button>
            <div class="crew-name">${crew.name}</div>
            <div class="crew-details">
                <small>${crew.nationality} • ${crew.languages[0]}</small>
            </div>
        </div>
      `
      slot.parentElement.classList.add("assigned")
      setTimeout(() => slot.parentElement.classList.remove("assigned"), 300)
    } else {
      const positionLabels = {
        captain: "Assign Captain",
        "first-officer": "Assign First Officer",
        "chief-attendant": "Assign Chief",
        "flight-attendant-1": "Assign Attendant",
        "flight-attendant-2": "Assign Attendant",
        chef: "Assign Chef",
      }

      slot.innerHTML = `
        <div class="empty-slot">
            <i class="fas fa-plus"></i>
            <span>${positionLabels[position]}</span>
        </div>
      `
    }
  }

  // Remove crew from position
  function removeCrewFromPosition(position) {
    const crew = assignedCrew[position]
    if (!crew) return

    assignedCrew[position] = null
    updatePositionSlot(position, null)

    // Show crew in available list again
    const crewItem = document.querySelector(`[data-crew-id="${crew.id}"]`)
    if (crewItem) {
      crewItem.style.display = "block"
    }

    updateProgress()
    updateSummary()

    showToast(`${crew.name} removed from ${position.replace("-", " ")}`)
  }

  // Update progress bars
  function updateProgress() {
    const flightCrewAssigned = [assignedCrew.captain, assignedCrew["first-officer"]].filter((c) => c).length
    const cabinCrewAssigned = [
      assignedCrew["chief-attendant"],
      assignedCrew["flight-attendant-1"],
      assignedCrew["flight-attendant-2"],
    ].filter((c) => c).length

    // Update flight crew progress
    const flightProgress = (flightCrewAssigned / 2) * 100
    document.getElementById("flight-crew-progress").style.width = `${flightProgress}%`
    document.getElementById("flight-crew-assigned").textContent = flightCrewAssigned

    // Update cabin crew progress
    const cabinProgress = (cabinCrewAssigned / 3) * 100
    document.getElementById("cabin-crew-progress").style.width = `${cabinProgress}%`
    document.getElementById("cabin-crew-assigned").textContent = cabinCrewAssigned

    // Enable/disable save button
    const saveBtn = document.getElementById("save-btn")
    const canSave = flightCrewAssigned >= 2 && cabinCrewAssigned >= 1
    saveBtn.disabled = !canSave
  }

  // Update summary
  function updateSummary() {
    if (!selectedFlight) return

    // Flight info
    const flightInfo = document.getElementById("summary-flight-info")
    flightInfo.innerHTML = `
      <div class="summary-list">
          <div class="summary-item"><strong>Flight:</strong> ${selectedFlight.flightNumber}</div>
          <div class="summary-item"><strong>Route:</strong> ${selectedFlight.source.code} → ${selectedFlight.destination.code}</div>
          <div class="summary-item"><strong>Aircraft:</strong> ${selectedFlight.aircraftType}</div>
          <div class="summary-item"><strong>Date:</strong> ${selectedFlight.date}</div>
      </div>
    `

    // Crew list
    const crewList = document.getElementById("summary-crew-list")
    const assignedCrewList = Object.values(assignedCrew).filter((c) => c)

    if (assignedCrewList.length > 0) {
      crewList.innerHTML = `
        <div class="summary-list">
            ${assignedCrewList
              .map(
                (crew) => `
                <div class="summary-item">
                    <strong>${crew.name}</strong> - ${crew.position || crew.type}
                </div>
            `,
              )
              .join("")}
        </div>
      `
    } else {
      crewList.innerHTML = "<p>No crew assigned yet</p>"
    }

    // Requirements check
    const requirements = document.getElementById("summary-requirements")
    const flightCrewCount = [assignedCrew.captain, assignedCrew["first-officer"]].filter((c) => c).length
    const cabinCrewCount = [
      assignedCrew["chief-attendant"],
      assignedCrew["flight-attendant-1"],
      assignedCrew["flight-attendant-2"],
    ].filter((c) => c).length

    requirements.innerHTML = `
      <div class="requirement-item ${flightCrewCount >= 2 ? "met" : "unmet"}">
          <i class="fas fa-${flightCrewCount >= 2 ? "check" : "times"}"></i>
          <span>Flight Crew: ${flightCrewCount}/2 required</span>
      </div>
      <div class="requirement-item ${cabinCrewCount >= 1 ? "met" : "unmet"}">
          <i class="fas fa-${cabinCrewCount >= 1 ? "check" : "times"}"></i>
          <span>Cabin Crew: ${cabinCrewCount}/3 minimum</span>
      </div>
      <div class="requirement-item ${assignedCrew.captain ? "met" : "unmet"}">
          <i class="fas fa-${assignedCrew.captain ? "check" : "times"}"></i>
          <span>Captain assigned</span>
      </div>
      <div class="requirement-item ${assignedCrew["chief-attendant"] ? "met" : "unmet"}">
          <i class="fas fa-${assignedCrew["chief-attendant"] ? "check" : "times"}"></i>
          <span>Chief Flight Attendant assigned</span>
      </div>
    `
  }

  // Clear all assignments
  function clearAssignments() {
    if (confirm("Are you sure you want to clear all assignments?")) {
      Object.keys(assignedCrew).forEach((position) => {
        if (assignedCrew[position]) {
          removeCrewFromPosition(position)
        }
      })

      showToast("All assignments cleared")
    }
  }

  // Save assignments
  function saveAssignments() {
    const modal = document.getElementById("confirmation-modal")
    const modalDetails = document.getElementById("modal-assignment-details")

    const assignedList = Object.entries(assignedCrew)
      .filter(([position, crew]) => crew)
      .map(([position, crew]) => `<li>${crew.name} - ${position.replace("-", " ")}</li>`)
      .join("")

    modalDetails.innerHTML = `
      <div style="margin-top: 1rem;">
          <strong>Flight:</strong> ${selectedFlight.flightNumber}<br>
          <strong>Assigned Crew:</strong>
          <ul style="margin-top: 0.5rem; padding-left: 1.5rem;">
              ${assignedList}
          </ul>
      </div>
    `

    modal.classList.add("show")
  }

  // Confirm save assignments
  function confirmSaveAssignments() {
    // Here you would typically send the data to a server
    console.log("Saving assignments:", {
      flight: selectedFlight,
      crew: assignedCrew,
    })

    closeModal()
    showToast("Crew assignments saved successfully!")

    // Optionally redirect to roster page
    setTimeout(() => {
      window.location.href = `roster.html?flight=${selectedFlight.flightNumber}`
    }, 2000)
  }

  // Close modal
  function closeModal() {
    document.getElementById("confirmation-modal").classList.remove("show")
  }

  // Initialize page
  document.addEventListener("DOMContentLoaded", () => {
    if (window.location.pathname.includes("assign-crew.html")) {
      // Setup initial state
      hideAssignmentSections()

      // Check if flight is specified in URL
      const urlParams = new URLSearchParams(window.location.search)
      const flightParam = urlParams.get("flight")
      if (flightParam) {
        document.getElementById("assign-flight-select").value = flightParam
        loadFlightDetails()
      }
    }
  })
}

// Show crew selection modal
function showCrewSelectionModal(crew) {
  // Implementation for showing crew selection modal
  console.log("Show crew selection modal for:", crew)
}

// Close crew selection modal
function closeCrewSelectionModal() {
  // Implementation for closing crew selection modal
  document.getElementById("crew-selection-modal").classList.remove("show")
}
function closeAddCrewModal() {
  document.getElementById("add-crew-modal").classList.remove("show");
}
