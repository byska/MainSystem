using MainSystem.Application.Abstraction;
using MainSystem.Domain.Entities;
using MainSystem.Domain.Enums;
using MainSystem.Domain.Services.Builders;
using MainSystem.Domain.Services.Specifications;
using MainSystem.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MainSystem.Application.UseCases.FlightRosterUseCases.Commands
{
    public sealed record CreateRosterCommand(string FlightNo) : IRequest<Guid>;
    public class CreateRosterCommandHandler : IRequestHandler<CreateRosterCommand, Guid>
    {
        private readonly IFlightInfoRepository _flights;
        private readonly IPassengerRepository _passengerRepository;
        private readonly IPilotPoolRepository _pilotRepository;
        private readonly ICabinCrewPoolRepository _cabinRepository;
        private readonly IFlightRosterRepository _rosterRepository;
        private readonly FlightRosterBuilder _builder;
        private readonly IUnitOfWork _unitOfWork;


        public CreateRosterCommandHandler(
            IFlightInfoRepository flights, IPassengerRepository passengerRepository,
            IPilotPoolRepository pilotRepository, ICabinCrewPoolRepository cabinRepository, IFlightRosterRepository rosterRepository, FlightRosterBuilder builder, IUnitOfWork unitOfWork)
        {
            _flights = flights;
            _passengerRepository = passengerRepository;
            _pilotRepository = pilotRepository;
            _cabinRepository = cabinRepository;
            _rosterRepository = rosterRepository;
            _builder = builder;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateRosterCommand request, CancellationToken cancellationToken)
        {
            var flight = await _flights.GetByFlightAsync(
                         new FlightNumber(request.FlightNo), cancellationToken)
                     ?? throw new InvalidOperationException($"Uçuş bulunamadı: {request.FlightNo}");

            /* ---------- 2. Yolcular ---------- */
            var passengers = await _passengerRepository.ListByFlightAsync(flight.FlightNo, cancellationToken);

            CompositeSpecification<PilotMember> spec = new TrueSpec<PilotMember>();
            if (flight.Aircraft !=null )
                spec = spec.And(new PilotVehicleSpec(flight.Aircraft));
            if (flight.DistanceKm != null)
                spec = spec.And(new PilotRangeSpec(flight.DistanceKm));
            /* ---------- 3. Pilot seçimi ---------- */

            var pilotPool = await _pilotRepository.ListAsync(spec, cancellationToken);

            var pilots = SelectPilots(pilotPool);

            /* ---------- 4. Kabin ekibi seçimi ---------- */
            var cabinSpec = new CabinCrewVehicleSpec(flight.Aircraft);
            var cabinPool = await _cabinRepository.ListAsync(cabinSpec, cancellationToken);
            var cabinCrew = SelectCabinCrew(cabinPool);

            var crewCountSpec = new CabinCrewCountSpec();

            if (!crewCountSpec.IsSatisfiedBy(cabinCrew))
                throw new InvalidOperationException(crewCountSpec.ErrorMessage!);
            /* ---------- 5. Roster oluştur ---------- */
            var roster = _builder
                .ForFlight(flight)
                .WithPassengers(passengers)
                .WithPilots(pilots)
                .WithCabinCrew(cabinCrew)
                .Build();

            /* ---------- 6. Kaydet ve GUID döndür ---------- */
            await _rosterRepository.AddAsync(roster, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return roster.Id;
        }
        private static IReadOnlyList<PilotMember> SelectPilots(IEnumerable<PilotMember> pool)
        {
            var pilots = new List<PilotMember>();

            var senior = pool.FirstOrDefault(p => p.Seniority == PilotSeniorityLevel.Senior);
            var junior = pool.FirstOrDefault(p => p.Seniority == PilotSeniorityLevel.Junior);
            var trainees = pool.Where(p => p.Seniority == PilotSeniorityLevel.Trainee)
                               .Take(2)        
                               .ToList();

            if (senior is null || junior is null)
                throw new InvalidOperationException("En az bir Senior ve bir Junior pilot gerekli.");

            pilots.Add(senior);
            pilots.Add(junior);
            pilots.AddRange(trainees);

            return pilots;
        }

        private static IReadOnlyList<CabinAttendantMember> SelectCabinCrew(IEnumerable<CabinAttendantMember> pool)
        {
            var chiefs = pool.Where(c => c.Type == AttendantType.Chief).Take(2).ToList();   // 1-4, biz 2 aldık
            var chefs = pool.Where(c => c.Type == AttendantType.Chef).Take(1).ToList();   // 0-2, biz 1 aldık
            var juniors = pool.Where(c => c.Type == AttendantType.Regular)
                              .Take(6).ToList();

            if (chiefs.Count < 1 || juniors.Count < 4)
                throw new InvalidOperationException("Kabin ekibi sayı kuralı sağlanmıyor.");

            var crew = new List<CabinAttendantMember>();
            crew.AddRange(chiefs);
            crew.AddRange(juniors);
            crew.AddRange(chefs);
            return crew;
        }
    }
}
