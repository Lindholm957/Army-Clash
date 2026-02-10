using System;
using UnityEngine;

namespace ArmyClash.Battle
{
    public static class UnitStatCalculator
    {
        private static StatConfiguration _config;

        public static void Initialize(StatConfiguration config)
        {
            _config = config;
        }

        public static UnitStats Calculate(UnitBlueprint blueprint)
        {
            if (_config == null)
            {
                Debug.LogError("StatCalculator не инициализирован");
                return new UnitStats(100f, 10f, 10f, 1f);
            }

            var baseStats = _config.baseStats;
            if (baseStats == null)
            {
                Debug.LogError("BaseStats не настроен в StatConfiguration");
                return new UnitStats(100f, 10f, 10f, 1f);
            }

            var finalHp = _config.CalculateFinalStat(baseStats.baseHp, blueprint.Shape, blueprint.Size, blueprint.Color,
                m => m.hpModifier, m => m.hpModifier, m => m.hpModifier);
                
            var finalAtk = _config.CalculateFinalStat(baseStats.baseAtk, blueprint.Shape, blueprint.Size, blueprint.Color,
                m => m.atkModifier, m => m.atkModifier, m => m.atkModifier);
                
            var finalSpeed = _config.CalculateFinalStat(baseStats.baseSpeed, blueprint.Shape, blueprint.Size, blueprint.Color,
                m => m.speedModifier, m => m.speedModifier, m => m.speedModifier);
                
            var finalAtkSpd = _config.CalculateFinalStat(baseStats.baseAtkSpd, blueprint.Shape, blueprint.Size, blueprint.Color,
                m => m.atkSpdModifier, m => m.atkSpdModifier, m => m.atkSpdModifier);

            var safeHp = MathF.Max(1f, finalHp);
            var safeAtk = MathF.Max(1f, finalAtk);
            var safeSpeed = MathF.Max(0.2f, finalSpeed);
            var safeAtkSpd = MathF.Max(0.1f, finalAtkSpd);

            return new UnitStats(safeHp, safeAtk, safeSpeed, safeAtkSpd);
        }

        public static Material GetUnitMaterial(UnitColor color)
        {
            if (_config == null)
            {
                Debug.LogError("StatCalculator не инициализирован");
                return null;
            }

            return _config.GetUnitMaterial(color);
        }
    }
}
