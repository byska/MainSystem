using MainSystem.Domain.Services.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace MainSystem.Domain.Services.Factories
{
    /// <summary>
    /// Config veya kullanıcı seçimine göre uygun koltuk atama stratejisini üretir.
    /// </summary>
    public interface ISeatAssignmentStrategyFactory
    {
        /// <param name="key">
        /// Örneğin "Greedy", "Group", "Random"… Anahtar harf duyarsızdır.
        /// </param>
        ISeatAssignmentStrategy Create(string key);
    }

    public sealed class SeatAssignmentStrategyFactory : ISeatAssignmentStrategyFactory
    {
        private readonly IReadOnlyDictionary<string, ISeatAssignmentStrategy> _map;

        public SeatAssignmentStrategyFactory(IEnumerable<ISeatAssignmentStrategy> strategies)
        {
            _map = strategies.ToDictionary(s => s.Key, StringComparer.OrdinalIgnoreCase);
        }

        public ISeatAssignmentStrategy Create(string key) =>
            _map.TryGetValue(key, out var s)
                ? s
                : throw new NotSupportedException($"Tanımsız strateji: {key}");

        public static ISeatAssignmentStrategy Create(ISeatPlan plan) =>
      plan switch
      {
          AirbusA320Plan => new GroupAwareSeatAssignmentStrategy(),
          Boeing737Plan => new GroupAwareSeatAssignmentStrategy(),
          _ => new RandomSeatAssignmentStrategy()
      };
    }
}
