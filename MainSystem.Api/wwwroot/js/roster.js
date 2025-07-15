// Roster specific functionality

const currentRoster = null // Declare the currentRoster variable

// Show roster tab
function showRosterTab(tabName, event) {
  // Hide all tab contents
  const tabContents = document.querySelectorAll("#roster-views .tab-content")
  tabContents.forEach((content) => {
    content.classList.remove("active")
  })

  // Remove active class from all tab buttons
  const tabButtons = document.querySelectorAll("#roster-views .tab-btn")
  tabButtons.forEach((button) => {
    button.classList.remove("active")
  })

  // Show selected tab content
  const selectedTab = document.getElementById(`${tabName}-view`)
  if (selectedTab) {
    selectedTab.classList.add("active")
  }

  // Add active class to clicked button
  event.target.classList.add("active")

  // Update content based on tab
  if (currentRoster) {
    switch (tabName) {
      case "tabular":
        updateTabularView(currentRoster)
        break
      case "plane":
        updatePlaneView(currentRoster)
        break
      case "extended":
        updateExtendedView(currentRoster)
        break
    }
  }
}

// Update roster views
function updateRosterViews(roster) {
  updateTabularView(roster)
  updatePlaneView(roster)
  updateExtendedView(roster)
}

// Update tabular view
function updateTabularView(roster) {
  const tbody = document.getElementById("roster-table-body")
  if (!tbody) return

  let html = ""

  // Flight crew
  roster.flightCrew.forEach((crew) => {
    html += `
            <tr>
                <td><strong>${crew.name}</strong></td>
                <td>${crew.id}</td>
                <td><span class="badge badge-active">Flight Crew</span></td>
                <td>${crew.position}</td>
                <td>${crew.nationality}</td>
                <td>${crew.seniority} Pilot</td>
            </tr>
        `
  })

  // Cabin crew
  roster.cabinCrew.forEach((crew) => {
    html += `
            <tr>
                <td><strong>${crew.name}</strong></td>
                <td>${crew.id}</td>
                <td><span class="badge badge-completed">Cabin Crew</span></td>
                <td>${crew.position}</td>
                <td>${crew.nationality}</td>
                <td>${crew.type}</td>
            </tr>
        `
  })

  // Passengers
  roster.passengers.forEach((passenger) => {
    html += `
            <tr>
                <td><strong>${passenger.name}</strong></td>
                <td>${passenger.id}</td>
                <td><span class="badge badge-scheduled">Passenger</span></td>
                <td>${passenger.seatNumber || "Unassigned"}</td>
                <td>${passenger.nationality}</td>
                <td>${passenger.seatType}</td>
            </tr>
        `
  })

  tbody.innerHTML = html
}

// Update plane view
function updatePlaneView(roster) {
  updateCockpitSeats(roster.flightCrew)
  updateCrewSeats(roster.cabinCrew)
  updatePassengerSeats(roster.passengers, roster.flightInfo.totalSeats)
}

// Update cockpit seats
function updateCockpitSeats(flightCrew) {
  const cockpitSeats = document.getElementById("cockpit-seats")
  if (!cockpitSeats) return

  let html = ""
  flightCrew.forEach((crew, index) => {
    html += `
            <div class="seat crew" title="${crew.name} - ${crew.seniority} Pilot">
                <div style="font-size: 0.6rem; font-weight: bold;">${crew.name
                  .split(" ")
                  .map((n) => n[0])
                  .join("")}</div>
                <div style="font-size: 0.5rem;">${crew.seniority}</div>
            </div>
        `
  })

  cockpitSeats.innerHTML = html
}

// Update crew seats
function updateCrewSeats(cabinCrew) {
  const crewSeats = document.getElementById("crew-seats")
  if (!crewSeats) return

  let html = ""
  cabinCrew.forEach((crew, index) => {
    const seatClass = crew.type === "Chef" ? "chef" : "cabin-crew"
    html += `
            <div class="seat ${seatClass}" title="${crew.name} - ${crew.type}">
                <div style="font-size: 0.6rem; font-weight: bold;">${crew.name
                  .split(" ")
                  .map((n) => n[0])
                  .join("")}</div>
                <div style="font-size: 0.5rem;">${crew.type}</div>
            </div>
        `
  })

  crewSeats.innerHTML = html
}

// Update passenger seats
function updatePassengerSeats(passengers, totalSeats) {
  const passengerSeats = document.getElementById("passenger-seats")
  if (!passengerSeats) return

  const seatsPerRow = 6
  const rows = Math.ceil(totalSeats / seatsPerRow)

  // Create seat map
  const seatMap = {}
  passengers.forEach((passenger) => {
    if (passenger.seatNumber) {
      seatMap[passenger.seatNumber] = passenger
    }
  })

  let html = ""
  for (let row = 1; row <= Math.min(rows, 50); row++) {
    // Limit to 50 rows for demo
    html += '<div class="seat-row">'
    html += `<div class="row-number">${row}</div>`

    for (let seat = 0; seat < seatsPerRow; seat++) {
      const seatLetter = String.fromCharCode(65 + seat)
      const seatNumber = `${row}${seatLetter}`
      const passenger = seatMap[seatNumber]

      const seatClass = passenger ? (passenger.seatType === "business" ? "business" : "occupied") : ""

      const title = passenger ? `${passenger.name} - ${passenger.seatType}` : `Seat ${seatNumber} - Available`

      html += `<div class="seat ${seatClass}" title="${title}">`
      if (passenger) {
        html += passenger.name
          .split(" ")
          .map((n) => n[0])
          .join("")
      } else {
        html += seatNumber
      }
      html += "</div>"

      // Add aisle space after seat C
      if (seat === 2) {
        html += '<div class="aisle"></div>'
      }
    }

    html += "</div>"
  }

  passengerSeats.innerHTML = html
}

// Update extended view
function updateExtendedView(roster) {
  updateFlightCrewTable(roster.flightCrew)
  updateCabinCrewTable(roster.cabinCrew)
  updatePassengersTable(roster.passengers)
}

// Update flight crew table
function updateFlightCrewTable(flightCrew) {
  const tbody = document.getElementById("flight-crew-body")
  if (!tbody) return

  let html = ""
  flightCrew.forEach((crew) => {
    const seniorityClass =
      crew.seniority === "Senior" ? "badge-active" : crew.seniority === "Junior" ? "badge-completed" : "badge-scheduled"

    html += `
            <tr>
                <td><strong>${crew.name}</strong></td>
                <td>${crew.age}</td>
                <td><span class="badge ${seniorityClass}">${crew.seniority}</span></td>
                <td>${crew.languages.join(", ")}</td>
                <td>${crew.vehicleRestriction}</td>
                <td>${crew.allowedRange.toLocaleString()} km</td>
            </tr>
        `
  })

  tbody.innerHTML = html
}

// Update cabin crew table
function updateCabinCrewTable(cabinCrew) {
  const tbody = document.getElementById("cabin-crew-body")
  if (!tbody) return

  let html = ""
  cabinCrew.forEach((crew) => {
    const typeClass =
      crew.type === "Chief" ? "badge-active" : crew.type === "Chef" ? "badge-completed" : "badge-scheduled"

    html += `
            <tr>
                <td><strong>${crew.name}</strong></td>
                <td>${crew.age}</td>
                <td><span class="badge ${typeClass}">${crew.type}</span></td>
                <td>${crew.languages.join(", ")}</td>
                <td>${crew.vehicleRestrictions.join(", ")}</td>
                <td>${crew.recipes ? crew.recipes.join(", ") : "N/A"}</td>
            </tr>
        `
  })

  tbody.innerHTML = html
}

// Update passengers table
function updatePassengersTable(passengers) {
  const tbody = document.getElementById("passengers-body")
  if (!tbody) return

  let html = ""
  passengers.forEach((passenger) => {
    const seatTypeClass = passenger.seatType === "business" ? "badge-active" : "badge-completed"
    const specialNotes = passenger.age <= 2 ? "Infant" : passenger.parentId ? "With Parent" : "Regular"

    html += `
            <tr>
                <td><strong>${passenger.name}</strong></td>
                <td>${passenger.age}</td>
                <td>${passenger.seatNumber || "Unassigned"}</td>
                <td><span class="badge ${seatTypeClass}">${passenger.seatType}</span></td>
                <td>${passenger.nationality}</td>
                <td>${specialNotes}</td>
            </tr>
        `
  })

  tbody.innerHTML = html
}
