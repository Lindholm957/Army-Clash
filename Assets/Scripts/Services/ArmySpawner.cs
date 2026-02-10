using UnityEngine;
using System.Collections.Generic;
using ArmyClash.Battle;
using ArmyClash.Models;

namespace ArmyClash.Services
{
    public class ArmySpawner
    {
        private readonly List<GameObject> _battleUnits = new List<GameObject>();

        public void SpawnArmy(
            IReadOnlyList<UnitBlueprint> blueprints,
            BattleArmy myArmy,
            BattleArmy enemyArmy,
            Vector3 center,
            int lookDirection)
        {
            if (blueprints == null)
            {
                Debug.LogError("SpawnArmy: blueprints равен null!");
                return;
            }

            if (myArmy == null)
            {
                Debug.LogError("SpawnArmy: myArmy равен null!");
                return;
            }

            if (enemyArmy == null)
            {
                Debug.LogError("SpawnArmy: enemyArmy равен null!");
                return;
            }

            const int columns = 5;
            const float spacing = 2.2f;

            for (var i = 0; i < blueprints.Count; i++)
            {
                var row = i / columns;
                var col = i % columns;
                var x = center.x + (col - 2) * spacing * 0.8f;
                var z = center.z + (row - 2) * spacing;

                var unitObject = CreateUnit(blueprints[i]);
                unitObject.transform.position = new Vector3(x, 0.5f, z);
                unitObject.transform.rotation = Quaternion.Euler(0f, lookDirection == 1 ? 90f : -90f, 0f);

                var agent = unitObject.AddComponent<UnitAgent>();
                
                var stats = UnitStatCalculator.Calculate(blueprints[i]);
                if (stats.Hp == 0f && stats.Atk == 0f)
                {
                    Debug.LogError("UnitStatCalculator не инициализирован!");
                    return;
                }
                
                agent.Initialize(blueprints[i], stats, myArmy, enemyArmy);

                myArmy.Add(agent);
                _battleUnits.Add(unitObject);
            }
        }

        private GameObject CreateUnit(UnitBlueprint blueprint)
        {
            return ArmyBuilder.CreateUnitObject(blueprint);
        }

        public void CleanupBattleUnits()
        {
            for (var i = _battleUnits.Count - 1; i >= 0; i--)
            {
                if (_battleUnits[i] != null)
                {
                    Object.Destroy(_battleUnits[i]);
                }
                _battleUnits.RemoveAt(i);
            }
        }
    }
}