using MainSystem.Domain.Entities;
using MainSystem.Domain.Enums;
using MainSystem.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSystem.Domain.Services.Specifications
{
    public sealed class PassengerOnFlightSpec : ISpecification<PassengerMember>
    {
        private readonly FlightNumber _flightNo;
        public PassengerOnFlightSpec(FlightNumber flightNo) => _flightNo = flightNo;

        public bool IsSatisfiedBy(PassengerMember p) => p.FlightNumber == _flightNo;
        public string? ErrorMessage => $"Uçuş {_flightNo.Value} için yolcu değil.";
    }
    public sealed class PassengerAgeGreaterThanSpec : ISpecification<PassengerMember>
    {
        private readonly int _age;
        public PassengerAgeGreaterThanSpec(int age) => _age = age;

        public bool IsSatisfiedBy(PassengerMember p) => p.Info.Age >= _age;
        public string? ErrorMessage => $"Yaş {_age}’den küçük.";
    }

    
}
