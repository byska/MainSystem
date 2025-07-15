using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MainSystem.Infrastructure
{
    public sealed record PersonInfo
    {
        public string Name { get; }
        public int Age { get; }
        public string Gender { get; }
        public string Nationality { get; }
        public IReadOnlyCollection<string> Languages { get; }

        private PersonInfo() { }

        public PersonInfo(string name,
                          int age,
                          string gender,
                          string nationality,
                          IEnumerable<string> languages)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("İsim boş olamaz.", nameof(name));

            if (age is < 0 or > 120)
                throw new ArgumentOutOfRangeException(nameof(age), "Yaş 0-120 aralığında olmalı.");

            if (!Regex.IsMatch(gender, "^(Kadın|Erkek|Diğer)$", RegexOptions.IgnoreCase))
                throw new ArgumentException("Geçersiz cinsiyet.", nameof(gender));

            if (string.IsNullOrWhiteSpace(nationality))
                throw new ArgumentException("Uyruk boş olamaz.", nameof(nationality));

            var langList = languages?.Distinct(StringComparer.OrdinalIgnoreCase).ToList()
                          ?? throw new ArgumentNullException(nameof(languages));

            if (langList.Count == 0)
                throw new ArgumentException("En az bir dil belirtilmeli.", nameof(languages));

            Name = name.Trim();
            Age = age;
            Gender = gender.Trim();
            Nationality = nationality.Trim();
            Languages = langList.AsReadOnly();
        }
    }


}
