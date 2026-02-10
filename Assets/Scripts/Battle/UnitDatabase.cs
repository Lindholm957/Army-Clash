using UnityEngine;
using System.Collections.Generic;

namespace ArmyClash.Battle
{
    [CreateAssetMenu(fileName = "UnitDatabase", menuName = "Army Clash/Unit Database")]
    public class UnitDatabase : ScriptableObject
    {
        public List<UnitBlueprint> GetRandomArmyBlueprints()
        {
            var armySize = 20;
            var blueprints = new List<UnitBlueprint>();
            
            for (int i = 0; i < armySize; i++)
            {
                var shapes = new[] { UnitShape.Cube, UnitShape.Sphere };
                var sizes = new[] { UnitSize.Small, UnitSize.Big };
                var colors = new[] { UnitColor.Blue, UnitColor.Green, UnitColor.Red };
                
                var randomShape = shapes[Random.Range(0, shapes.Length)];
                var randomSize = sizes[Random.Range(0, sizes.Length)];
                var randomColor = colors[Random.Range(0, colors.Length)];
                
                blueprints.Add(new UnitBlueprint(randomShape, randomSize, randomColor));
            }
            
            return blueprints;
        }
    }
}