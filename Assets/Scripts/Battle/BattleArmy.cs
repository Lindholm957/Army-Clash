using System.Collections.Generic;
using UnityEngine;

namespace ArmyClash.Battle
{
    public class BattleArmy
    {
        private readonly List<UnitAgent> _units = new();

        public BattleArmy(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public int AliveCount { get; private set; }

        public void Add(UnitAgent unit)
        {
            _units.Add(unit);
            AliveCount++;
        }

        public UnitAgent GetClosestAlive(Vector3 from)
        {
            UnitAgent best = null;
            var bestDistanceSqr = float.MaxValue;

            foreach (var unit in _units)
            {
                if (unit == null || unit.IsDead)
                {
                    continue;
                }

                var distanceSqr = (unit.transform.position - from).sqrMagnitude;
                if (distanceSqr >= bestDistanceSqr)
                {
                    continue;
                }

                bestDistanceSqr = distanceSqr;
                best = unit;
            }

            return best;
        }

        public void NotifyUnitDied(UnitAgent unit)
        {
            AliveCount = Mathf.Max(AliveCount - 1, 0);
        }
    }
}